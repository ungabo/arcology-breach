#!/usr/bin/env python3
"""Generate deterministic V1 placeholder WAVs and manifest files.

This script is dependency-free and only writes into the AudioV1 staging lane:

- Assets/_Project/ArtStaging/AudioV1/
- Documentation/AssetProduction/AudioV1/
"""

from __future__ import annotations

import argparse
import datetime as dt
import json
import math
import random
import struct
import wave
from pathlib import Path
from typing import Dict, Iterable, List, Sequence, Tuple


SAMPLE_RATE = 48000
BIT_DEPTH = 16

IMPORT_GROUPS = {
    "ambience_stereo_loop": {
        "force_to_mono": False,
        "load_type": "Streaming",
        "compression_format": "Vorbis",
        "quality": 0.45,
        "preload_audio_data": False,
        "load_in_background": True,
        "loop": True,
        "intended_source": "2D AudioSource on ambience bus, volume 0.08-0.18 per layer",
    },
    "spatial_loop_mono": {
        "force_to_mono": True,
        "load_type": "Compressed In Memory",
        "compression_format": "Vorbis",
        "quality": 0.40,
        "preload_audio_data": True,
        "load_in_background": False,
        "loop": True,
        "intended_source": "3D AudioSource, spatialBlend 1.0, minDistance 2-4m, maxDistance 14-24m",
    },
    "sfx_oneshot_mono": {
        "force_to_mono": True,
        "load_type": "Decompress On Load",
        "compression_format": "ADPCM",
        "quality": None,
        "preload_audio_data": True,
        "load_in_background": False,
        "loop": False,
        "intended_source": "PlayOneShot or PlayClipAtPoint, SFX bus, spatialBlend by gameplay context",
    },
    "boss_oneshot_mono": {
        "force_to_mono": True,
        "load_type": "Decompress On Load",
        "compression_format": "ADPCM",
        "quality": None,
        "preload_audio_data": True,
        "load_in_background": False,
        "loop": False,
        "intended_source": "3D boss AudioSource, spatialBlend 0.75-1.0, wider maxDistance than regular enemies",
    },
}


def sine(freq: float, t: float, phase: float = 0.0) -> float:
    return math.sin((2.0 * math.pi * freq * t) + phase)


def tri(freq: float, t: float) -> float:
    return (2.0 / math.pi) * math.asin(sine(freq, t))


def lerp(a: float, b: float, x: float) -> float:
    return a + (b - a) * max(0.0, min(1.0, x))


def clamp(value: float) -> float:
    return max(-1.0, min(1.0, value))


def db_to_amp(db: float) -> float:
    return math.pow(10.0, db / 20.0)


def amp_to_db(amp: float) -> float:
    return -120.0 if amp <= 0.0 else 20.0 * math.log10(amp)


def soft_clip(value: float, drive: float = 1.2) -> float:
    return math.tanh(value * drive) / math.tanh(drive)


def env(t: float, start: float, duration: float, attack: float, hold: float, curve: float) -> float:
    local = t - start
    if local < 0.0 or local > duration:
        return 0.0
    if attack > 0.0 and local < attack:
        return local / attack
    if local < hold:
        return 1.0
    remaining = max(0.0, (duration - local) / max(0.0001, duration - hold))
    return math.pow(remaining, curve)


def smooth(x: float) -> float:
    x = max(0.0, min(1.0, x))
    return x * x * (3.0 - 2.0 * x)


def loop_table(ctx: Dict[str, object], name: str, points: int) -> List[float]:
    tables = ctx.setdefault("tables", {})
    if name not in tables:
        rng = random.Random(int(ctx["seed"]) + len(name) * 313)
        values = [rng.uniform(-1.0, 1.0) for _ in range(points)]
        values.append(values[0])
        tables[name] = values
    return tables[name]  # type: ignore[return-value]


def loop_noise(ctx: Dict[str, object], name: str, i: int, total: int, points: int = 256) -> float:
    values = loop_table(ctx, name, points)
    pos = (i / float(total)) * points
    idx = int(pos) % points
    frac = smooth(pos - math.floor(pos))
    return lerp(values[idx], values[idx + 1], frac)


def one_shot_value(recipe: Sequence[Tuple], t: float, rng: random.Random) -> float:
    value = 0.0
    for layer in recipe:
        kind = layer[0]
        if kind == "tone":
            _, amp, f0, f1, start, dur, attack, hold, curve = layer
            e = env(t, start, dur, attack, hold, curve)
            local = max(0.0, t - start)
            value += amp * sine(lerp(f0, f1, local / max(0.0001, dur)), local) * e
        elif kind == "noise":
            _, amp, start, dur, attack, hold, curve = layer
            value += amp * rng.uniform(-1.0, 1.0) * env(t, start, dur, attack, hold, curve)
        elif kind == "pulse":
            _, amp, rate, duty, f0, f1, start, dur, attack, hold, curve = layer
            e = env(t, start, dur, attack, hold, curve)
            local = max(0.0, t - start)
            gate = 1.0 if ((local * rate) % 1.0) < duty else -1.0
            value += amp * gate * sine(lerp(f0, f1, local / max(0.0001, dur)), local) * e
        elif kind == "hit":
            _, amp, center, width, freq = layer
            distance = abs(t - center)
            if distance < width:
                value += amp * math.pow(1.0 - distance / width, 2.0) * sine(freq, t)
    return soft_clip(value)


def loop_value(key: str, t: float, i: int, total: int, ctx: Dict[str, object]) -> Tuple[float, ...]:
    if key == "amb_boiler":
        bed = 0.58 * sine(36.0, t) + 0.25 * sine(60.0, t, 0.7)
        bed += 0.12 * sine(96.0, t) * (0.7 + 0.3 * sine(0.5, t))
        bed += 0.07 * loop_noise(ctx, "boiler_grit", i, total, 128)
        pan = 0.08 * sine(0.125, t)
        return (soft_clip(bed) * (1.0 - pan), soft_clip(bed) * (1.0 + pan))
    if key == "amb_steam":
        left = (0.26 * loop_noise(ctx, "steam_l", i, total, 96) + 0.18 * loop_noise(ctx, "steam_fast", i, total, 512))
        right = (0.24 * loop_noise(ctx, "steam_r", i, total, 96) + 0.16 * loop_noise(ctx, "steam_fast", i, total, 512))
        swell = 0.70 + 0.30 * sine(0.25, t)
        return (left * swell, right * (0.78 + 0.22 * sine(0.375, t, 1.0)))
    if key == "amb_machinery":
        gear = 0.11 * tri(72.0, t) + 0.08 * sine(144.0, t, 0.3)
        piston = max(0.0, sine(1.5, t)) * 0.28 * sine(120.0, t)
        belts = 0.09 * sine(24.0, t) * (0.6 + 0.4 * sine(0.75, t))
        grit = 0.05 * loop_noise(ctx, "machinery_grit", i, total, 192)
        pan = 0.16 * sine(0.25, t)
        value = soft_clip(gear + piston + belts + grit)
        return (value * (1.0 - pan), value * (1.0 + pan))
    if key == "amb_pipe_knocks":
        value = 0.05 * loop_noise(ctx, "pipe_bed", i, total, 128)
        for center, pitch, amp in [(0.8, 430.0, 0.55), (2.35, 290.0, 0.36), (5.1, 520.0, 0.42), (6.7, 240.0, 0.28)]:
            d = abs((t % 8.0) - center)
            if d < 0.045:
                value += amp * math.pow(1.0 - d / 0.045, 2.0) * (sine(pitch, t) + 0.35 * sine(pitch * 1.5, t))
        pan = 0.28 * sine(0.125, t)
        value = soft_clip(value, 1.8)
        return (value * (1.0 - pan), value * (1.0 + pan))
    if key == "amb_mix":
        b = loop_value("amb_boiler", t, i, total, ctx)
        s = loop_value("amb_steam", t, i, total, ctx)
        m = loop_value("amb_machinery", t, i, total, ctx)
        k = loop_value("amb_pipe_knocks", t, i, total, ctx)
        return (
            soft_clip(b[0] * 0.58 + s[0] * 0.42 + m[0] * 0.35 + k[0] * 0.23),
            soft_clip(b[1] * 0.58 + s[1] * 0.42 + m[1] * 0.35 + k[1] * 0.23),
        )
    if key == "steam_hazard_loop":
        bed = 0.28 * loop_noise(ctx, "haz_steam_bed", i, total, 384)
        chuff = 0.24 * max(0.0, sine(4.0, t)) * loop_noise(ctx, "haz_steam_chuff", i, total, 128)
        return (soft_clip(bed + chuff + 0.12 * sine(120.0, t) * (0.75 + 0.25 * sine(2.0, t))),)
    if key == "furnace_warning_loop":
        warning = 0.34 * sine(55.0, t) * (0.52 + 0.48 * max(0.0, sine(2.0, t)))
        flame = 0.18 * loop_noise(ctx, "furnace_flame", i, total, 256)
        metal = 0.10 * sine(220.0, t) * (0.5 + 0.5 * sine(1.0, t))
        return (soft_clip(warning + flame + metal),)
    raise ValueError(f"Unknown loop recipe {key}")


def tone(amp: float, f0: float, f1: float, start: float, dur: float, attack: float, hold: float, curve: float) -> Tuple:
    return ("tone", amp, f0, f1, start, dur, attack, hold, curve)


def noise(amp: float, start: float, dur: float, attack: float, hold: float, curve: float) -> Tuple:
    return ("noise", amp, start, dur, attack, hold, curve)


def pulse(amp: float, rate: float, duty: float, f0: float, f1: float, start: float, dur: float, attack: float, hold: float, curve: float) -> Tuple:
    return ("pulse", amp, rate, duty, f0, f1, start, dur, attack, hold, curve)


def hit(amp: float, center: float, width: float, freq: float) -> Tuple:
    return ("hit", amp, center, width, freq)


def asset(asset_id: str, filename: str, title: str, category: str, recipe, duration: float, channels: int, loop: bool, group: str, peak: float, lufs: float, reference: str, notes: str) -> Dict[str, object]:
    return {
        "id": asset_id,
        "filename": filename,
        "title": title,
        "category": category,
        "recipe": recipe,
        "duration": duration,
        "channels": channels,
        "loop": loop,
        "unity_group": group,
        "target_peak_dbfs": peak,
        "target_lufs": lufs,
        "reference": reference,
        "notes": notes,
    }


ASSETS = [
    asset("AUDV1-AMB-001", "AUDV1_AMB_BoilerRumble_loop.wav", "Boiler Rumble Ambience Layer", "Ambience", "amb_boiler", 8.0, 2, True, "ambience_stereo_loop", -6.0, -30.0, "SteamworksAudio AmbienceSample rumble component", "Low boiler and furnace pressure bed."),
    asset("AUDV1-AMB-002", "AUDV1_AMB_SteamBed_loop.wav", "Steam Bed Ambience Layer", "Ambience", "amb_steam", 8.0, 2, True, "ambience_stereo_loop", -8.0, -32.0, "SteamworksAudio AmbienceSample steam component", "Wide non-directional steam wash."),
    asset("AUDV1-AMB-003", "AUDV1_AMB_DistantMachinery_loop.wav", "Distant Machinery Ambience Layer", "Ambience", "amb_machinery", 8.0, 2, True, "ambience_stereo_loop", -7.0, -31.0, "SteamworksAudio AmbienceSample flywheel and piston components", "Gear and piston bed for larger industrial rooms."),
    asset("AUDV1-AMB-004", "AUDV1_AMB_PipeKnocks_loop.wav", "Pipe Knocks Ambience Layer", "Ambience", "amb_pipe_knocks", 8.0, 2, True, "ambience_stereo_loop", -7.0, -33.0, "Catalog AUD-012 future pipe knocks", "Sparse loopable pipe knocks."),
    asset("AUDV1-AMB-005", "AUDV1_AMB_BrassworksMix_loop.wav", "Brassworks Ambience Preview Mix", "Ambience", "amb_mix", 8.0, 2, True, "ambience_stereo_loop", -5.0, -28.0, "SteamworksAudio Brassworks Ambience Loop", "Combined audition mix; final levels should prefer separate layers."),
    asset("AUDV1-WPN-001", "AUDV1_WPN_PressurePistolFire.wav", "Pressure Pistol Fire", "Weapon", [tone(0.90, 980, 210, 0, 0.18, 0.001, 0.035, 3.2), tone(0.34, 1480, 1480, 0, 0.045, 0.001, 0.009, 2.5), noise(0.28, 0, 0.18, 0.001, 0.020, 2.0)], 0.18, 1, False, "sfx_oneshot_mono", -1.0, -14.0, "SteamworksAudioCue.PressureFire", "Sharp pneumatic snap, smaller than scattergun."),
    asset("AUDV1-WPN-002", "AUDV1_WPN_PressureBurst.wav", "Pressure Burst Alternate Fire", "Weapon", [tone(0.65, 720, 150, 0, 0.26, 0.001, 0.120, 1.6), tone(0.22, 310, 310, 0, 0.26, 0.001, 0.140, 2.0), noise(0.45, 0, 0.26, 0.001, 0.190, 1.4), tone(0.28, 1280, 1280, 0, 0.040, 0.001, 0.008, 2.8)], 0.26, 1, False, "sfx_oneshot_mono", -1.0, -13.0, "SteamworksAudioCue.PressureBurst", "Valve dump and pressure wash for pistol alternate fire."),
    asset("AUDV1-WPN-003", "AUDV1_WPN_ScattergunBlast.wav", "Steam Scattergun Blast", "Weapon", [tone(0.88, 190, 48, 0, 0.32, 0.001, 0.200, 1.5), tone(0.27, 880, 880, 0, 0.055, 0.001, 0.018, 2.3), tone(0.22, 116, 116, 0, 0.32, 0.003, 0.190, 2.0), noise(0.45, 0, 0.32, 0.001, 0.250, 1.1)], 0.32, 1, False, "sfx_oneshot_mono", -1.0, -12.0, "SteamworksAudioCue.SteamScattergunFire", "Heavy low-pressure blast with brass clack and steam body."),
    asset("AUDV1-WPN-004", "AUDV1_WPN_ScattergunSlug.wav", "Steam Scattergun Slug Shot", "Weapon", [tone(0.70, 360, 82, 0, 0.36, 0.001, 0.190, 1.7), tone(0.30, 1160, 1160, 0, 0.065, 0.001, 0.022, 2.5), tone(0.20, 980, 420, 0, 0.36, 0.004, 0.250, 1.6), noise(0.32, 0, 0.36, 0.001, 0.280, 1.3)], 0.36, 1, False, "sfx_oneshot_mono", -1.0, -13.0, "SteamworksAudioCue.SteamScattergunSlug", "Narrow pressure crack with pipe whistle."),
    asset("AUDV1-WPN-005", "AUDV1_WPN_EmptyClick.wav", "Dry Pressure Click", "Weapon", [tone(0.55, 1360, 1360, 0, 0.032, 0.001, 0.004, 3.0), tone(0.32, 740, 740, 0.034, 0.045, 0.001, 0.006, 2.4), noise(0.10, 0, 0.10, 0.001, 0.035, 2.0)], 0.10, 1, False, "sfx_oneshot_mono", -3.0, -18.0, "SteamworksAudioCue.EmptyClick", "Short mechanical dry click."),
    asset("AUDV1-IMP-001", "AUDV1_IMP_MachineHit.wav", "Machine Hit Impact", "Impact", [tone(0.30, 330, 330, 0, 0.18, 0.001, 0.080, 2.0), noise(0.42, 0, 0.18, 0.001, 0.060, 2.8), tone(0.36, 190, 190, 0, 0.11, 0.001, 0.040, 2.4)], 0.18, 1, False, "sfx_oneshot_mono", -2.0, -15.0, "SteamworksAudioCue.EnemyHit", "Metal strain, plate ping, and spark crackle."),
    asset("AUDV1-IMP-002", "AUDV1_IMP_MachineDeathShort.wav", "Machine Shutdown Short", "Impact", [tone(0.50, 620, 88, 0, 0.58, 0.003, 0.420, 1.4), tone(0.42, 68, 68, 0, 0.58, 0.020, 0.500, 1.8), noise(0.22, 0, 0.58, 0.020, 0.520, 1.5)], 0.58, 1, False, "sfx_oneshot_mono", -2.0, -15.0, "SteamworksAudioCue.EnemyDeath", "Compact machine spin-down and metal drop."),
    asset("AUDV1-ENY-001", "AUDV1_ENY_ScrapperAttackTell.wav", "Scrapper Attack Tell", "Enemy", [pulse(0.35, 28, 0.24, 1020, 1020, 0, 0.10, 0.001, 0.070, 1.8), tone(0.42, 220, 560, 0, 0.20, 0.008, 0.150, 1.1), noise(0.26, 0, 0.20, 0.001, 0.160, 1.4)], 0.20, 1, False, "sfx_oneshot_mono", -2.0, -16.0, "SteamworksAudioCue.EnemyAttackTell", "Ratchet, pressure rise, and cutter scrape before melee damage."),
    asset("AUDV1-ENY-002", "AUDV1_ENY_LancerFireTell.wav", "Lancer Fire Tell", "Enemy", [tone(0.28, 1280, 1280, 0, 0.052, 0.001, 0.018, 2.5), tone(0.45, 330, 900, 0, 0.22, 0.010, 0.170, 1.1), noise(0.20, 0, 0.22, 0.002, 0.180, 1.3)], 0.22, 1, False, "sfx_oneshot_mono", -2.0, -16.0, "SteamworksAudioCue.LancerFireTell", "Valve tick and rising coil charge."),
    asset("AUDV1-ENY-003", "AUDV1_ENY_BulwarkAttackTell.wav", "Bulwark Attack Tell", "Enemy", [pulse(0.25, 18, 0.30, 820, 820, 0, 0.16, 0.002, 0.120, 1.8), tone(0.58, 92, 310, 0, 0.38, 0.006, 0.290, 1.2), noise(0.32, 0, 0.38, 0.003, 0.320, 1.2), tone(0.44, 54, 54, 0.25, 0.14, 0.001, 0.070, 1.8)], 0.38, 1, False, "sfx_oneshot_mono", -1.5, -14.5, "SteamworksAudioCue.BulwarkAttackTell", "Heavy boiler rise, chain drag, and pre-impact knock."),
    asset("AUDV1-BOSS-001", "AUDV1_BOSS_WardenStompTell.wav", "Governor Warden Stomp Tell", "Boss", [tone(0.55, 70, 150, 0, 0.58, 0.010, 0.420, 1.2), pulse(0.30, 9, 0.35, 240, 240, 0, 0.58, 0.005, 0.380, 1.4), tone(0.50, 42, 42, 0.43, 0.20, 0.001, 0.120, 2.2)], 0.58, 1, False, "boss_oneshot_mono", -1.5, -14.0, "Governor Warden stomp attack concept", "Large piston windup with low stomp pre-hit."),
    asset("AUDV1-BOSS-002", "AUDV1_BOSS_WardenPressureBoltTell.wav", "Governor Warden Pressure Bolt Tell", "Boss", [tone(0.42, 180, 760, 0, 0.36, 0.020, 0.330, 0.8), tone(0.20, 1260, 1180, 0, 0.36, 0.004, 0.260, 0.8), noise(0.24, 0, 0.36, 0.020, 0.330, 0.9)], 0.36, 1, False, "boss_oneshot_mono", -2.0, -15.0, "Governor Warden pressure-bolt attack concept", "Boss-scale pressure charge distinct from Lancer."),
    asset("AUDV1-BOSS-003", "AUDV1_BOSS_WardenEnrage.wav", "Governor Warden Enrage", "Boss", [tone(0.46, 170, 310, 0, 0.92, 0.040, 0.720, 1.0), tone(0.42, 56, 56, 0, 0.92, 0.010, 0.740, 1.0), noise(0.34, 0, 0.92, 0.020, 0.800, 1.0)], 0.92, 1, False, "boss_oneshot_mono", -2.0, -14.0, "Governor Warden half-health behavior", "Pressure siren and boiler swell for phase transition."),
    asset("AUDV1-BOSS-004", "AUDV1_BOSS_WardenShutdown.wav", "Governor Warden Shutdown", "Boss", [tone(0.55, 760, 48, 0, 1.35, 0.005, 0.950, 1.3), tone(0.55, 36, 36, 0.75, 0.50, 0.002, 0.220, 2.1), noise(0.34, 0, 1.35, 0.030, 1.050, 1.2), tone(0.15, 520, 520, 0.40, 0.65, 0.004, 0.340, 1.6)], 1.35, 1, False, "boss_oneshot_mono", -1.5, -13.5, "WardenShutdownVfx and current Warden defeat flow", "Long boss spin-down and body drop."),
    asset("AUDV1-PCK-001", "AUDV1_PCK_HealthPickup.wav", "Health Vial Pickup", "Pickup", [tone(0.34, 560, 840, 0, 0.22, 0.004, 0.120, 1.9), tone(0.26, 1320, 1320, 0, 0.052, 0.001, 0.020, 2.3), noise(0.10, 0, 0.22, 0.005, 0.180, 1.7)], 0.22, 1, False, "sfx_oneshot_mono", -3.0, -18.0, "SteamworksAudioCue.HealthPickup", "Glass and medicinal tick."),
    asset("AUDV1-PCK-002", "AUDV1_PCK_AmmoPickup.wav", "Pressure Cartridge Pickup", "Pickup", [tone(0.32, 410, 760, 0, 0.20, 0.003, 0.100, 1.8), tone(0.24, 990, 990, 0, 0.044, 0.001, 0.018, 2.5), noise(0.12, 0, 0.20, 0.002, 0.160, 1.6)], 0.20, 1, False, "sfx_oneshot_mono", -3.0, -18.0, "SteamworksAudioCue.AmmoPickup", "Small pressure-cartridge tick with short hiss."),
    asset("AUDV1-PCK-003", "AUDV1_PCK_GearKeyPickup.wav", "Gear Key Pickup", "Pickup", [tone(0.36, 720, 720, 0, 0.20, 0.002, 0.080, 1.8), tone(0.30, 960, 960, 0.11, 0.20, 0.002, 0.080, 1.8), tone(0.24, 1320, 1320, 0.22, 0.20, 0.002, 0.140, 1.8), noise(0.06, 0, 0.44, 0.008, 0.280, 1.8)], 0.44, 1, False, "sfx_oneshot_mono", -2.5, -17.0, "SteamworksAudioCue.GearKey", "Three-step brass chime for objective pickup."),
    asset("AUDV1-PCK-004", "AUDV1_PCK_WeaponPickup.wav", "Weapon Pickup Acquisition", "Pickup", [tone(0.32, 920, 920, 0, 0.09, 0.001, 0.050, 2.0), tone(0.30, 260, 760, 0, 0.48, 0.006, 0.360, 1.2), tone(0.18, 980, 980, 0.13, 0.22, 0.002, 0.120, 1.8), tone(0.14, 1320, 1320, 0.25, 0.22, 0.002, 0.150, 1.8), noise(0.20, 0, 0.48, 0.012, 0.390, 1.2)], 0.48, 1, False, "sfx_oneshot_mono", -2.0, -15.5, "SteamworksAudioCue.WeaponPickup", "Brass latch, pressure rise, and acquisition chime."),
    asset("AUDV1-INT-001", "AUDV1_INT_GateOpen.wav", "Pressure Gate Open", "Interaction", [tone(0.52, 52, 52, 0, 0.72, 0.020, 0.660, 1.0), tone(0.24, 180, 95, 0, 0.72, 0.010, 0.550, 1.2), noise(0.22, 0, 0.72, 0.020, 0.600, 1.2), tone(0.30, 96, 96, 0.56, 0.20, 0.002, 0.080, 2.0)], 0.72, 1, False, "sfx_oneshot_mono", -2.0, -15.0, "SteamworksAudioCue.GateOpen", "Motor slide, servo, and final latch."),
    asset("AUDV1-INT-002", "AUDV1_INT_GateDenied.wav", "Pressure Gate Denied", "Interaction", [tone(0.42, 112, 112, 0, 0.26, 0.002, 0.120, 1.2), tone(0.24, 56, 56, 0, 0.26, 0.002, 0.160, 1.4), tone(0.28, 170, 170, 0, 0.07, 0.001, 0.030, 2.0)], 0.26, 1, False, "sfx_oneshot_mono", -2.0, -16.0, "SteamworksAudioCue.GateDenied", "Warning buzzer and mechanical clonk."),
    asset("AUDV1-INT-003", "AUDV1_INT_LiftActivate.wav", "Service Lift Activate", "Interaction", [tone(0.36, 76, 76, 0, 0.82, 0.030, 0.680, 1.0), tone(0.26, 520, 520, 0.12, 0.28, 0.005, 0.160, 1.6), pulse(0.22, 13, 0.35, 220, 220, 0, 0.82, 0.020, 0.580, 1.2), noise(0.20, 0, 0.82, 0.040, 0.620, 1.2)], 0.82, 1, False, "sfx_oneshot_mono", -2.0, -15.0, "SteamworksAudioCue.Win and LiftActivationVfx", "Pulley motor, green-system chime, and steam vent."),
    asset("AUDV1-INT-004", "AUDV1_INT_ValveTurn.wav", "Valve Turn", "Interaction", [pulse(0.28, 20, 0.22, 760, 760, 0, 0.34, 0.002, 0.280, 1.1), tone(0.24, 180, 180, 0, 0.34, 0.004, 0.260, 1.2), noise(0.16, 0, 0.34, 0.002, 0.280, 1.0)], 0.34, 1, False, "sfx_oneshot_mono", -3.0, -18.0, "Pipeworks routing valve objective", "Ratchet clicks and pipe strain."),
    asset("AUDV1-INT-005", "AUDV1_INT_ValveVented.wav", "Valve Vented Pressure Release", "Interaction", [noise(0.44, 0, 0.62, 0.004, 0.460, 1.0), tone(0.38, 240, 88, 0, 0.62, 0.008, 0.420, 1.3), tone(0.22, 620, 620, 0.42, 0.14, 0.002, 0.040, 2.2)], 0.62, 1, False, "sfx_oneshot_mono", -2.0, -15.5, "SteamValveObjective hazard shutdown", "Pressure release and close tick."),
    asset("AUDV1-HAZ-001", "AUDV1_HAZ_SteamHazardLoop.wav", "Steam Hazard Loop", "Hazard", "steam_hazard_loop", 3.0, 1, True, "spatial_loop_mono", -5.0, -23.0, "SteamHazardVfx and planned hazard audio", "Spatial active steam vent loop."),
    asset("AUDV1-HAZ-002", "AUDV1_HAZ_FurnaceWarningLoop.wav", "Furnace Warning Loop", "Hazard", "furnace_warning_loop", 3.0, 1, True, "spatial_loop_mono", -6.0, -24.0, "FurnaceHeatHazardVfx warning phase", "Low danger pulse before active furnace heat."),
    asset("AUDV1-HAZ-003", "AUDV1_HAZ_FurnaceHeatPulse.wav", "Furnace Heat Active Pulse", "Hazard", [tone(0.52, 92, 44, 0, 0.88, 0.015, 0.720, 1.0), noise(0.42, 0, 0.88, 0.010, 0.680, 1.0), tone(0.16, 660, 660, 0, 0.08, 0.001, 0.040, 2.2)], 0.88, 1, False, "sfx_oneshot_mono", -2.0, -15.0, "FurnaceHeatHazardVfx active phase", "Heat bloom for active furnace surge."),
    asset("AUDV1-HAZ-004", "AUDV1_HAZ_BellowsNodePulse.wav", "Bellows Node Pulse", "Hazard", [tone(0.54, 168, 58, 0, 0.44, 0.003, 0.320, 1.4), tone(0.40, 42, 42, 0, 0.44, 0.001, 0.260, 1.7), tone(0.22, 760, 760, 0, 0.075, 0.001, 0.045, 2.0), noise(0.34, 0, 0.44, 0.004, 0.370, 1.2)], 0.44, 1, False, "sfx_oneshot_mono", -1.5, -14.0, "SteamworksAudioCue.BellowsNodePulse", "Support-machine pressure pulse."),
    asset("AUDV1-PLR-001", "AUDV1_PLR_PlayerHurt.wav", "Player Hurt", "Player", [tone(0.52, 84, 84, 0, 0.26, 0.001, 0.120, 1.8), noise(0.20, 0, 0.26, 0.002, 0.080, 2.2), tone(0.18, 230, 230, 0, 0.26, 0.002, 0.100, 2.0)], 0.26, 1, False, "sfx_oneshot_mono", -2.0, -15.5, "SteamworksAudioCue.PlayerHurt", "Short impact thud and pressure scrape."),
]


def render(asset_def: Dict[str, object], index: int) -> Tuple[List[Tuple[float, ...]], Dict[str, float]]:
    sample_count = int(round(float(asset_def["duration"]) * SAMPLE_RATE))
    channels = int(asset_def["channels"])
    rng = random.Random(91357 + index * 431)
    ctx: Dict[str, object] = {"seed": 91357 + index * 431, "tables": {}}
    frames: List[Tuple[float, ...]] = []
    recipe = asset_def["recipe"]

    for i in range(sample_count):
        t = i / SAMPLE_RATE
        if isinstance(recipe, str):
            frame = loop_value(recipe, t, i, sample_count, ctx)
        else:
            frame = (one_shot_value(recipe, t, rng),)
        if len(frame) != channels:
            frame = (sum(frame) / len(frame),) if channels == 1 else (frame[0], frame[0])
        frames.append(tuple(clamp(v) for v in frame))

    peak = max(abs(v) for frame in frames for v in frame) if frames else 0.0
    target = db_to_amp(float(asset_def["target_peak_dbfs"]))
    if peak > 0.0:
        scale = target / peak
        frames = [tuple(clamp(v * scale) for v in frame) for frame in frames]

    peak = max(abs(v) for frame in frames for v in frame) if frames else 0.0
    rms_values = [v * v for frame in frames for v in frame]
    rms = math.sqrt(sum(rms_values) / len(rms_values)) if rms_values else 0.0
    return frames, {"measured_peak_dbfs": round(amp_to_db(peak), 2), "measured_rms_dbfs": round(amp_to_db(rms), 2)}


def write_wav(path: Path, frames: Sequence[Tuple[float, ...]], channels: int) -> None:
    with wave.open(str(path), "wb") as wav:
        wav.setnchannels(channels)
        wav.setsampwidth(BIT_DEPTH // 8)
        wav.setframerate(SAMPLE_RATE)
        payload = bytearray()
        for frame in frames:
            for value in frame:
                payload.extend(struct.pack("<h", int(round(clamp(value) * 32767.0))))
        wav.writeframes(bytes(payload))


def asset_record(asset_def: Dict[str, object], frames: Sequence[Tuple[float, ...]], stats: Dict[str, float]) -> Dict[str, object]:
    sample_count = len(frames)
    loop_points = None
    if asset_def["loop"]:
        loop_points = {"start_sample": 0, "end_sample": sample_count, "start_sec": 0.0, "end_sec": round(sample_count / SAMPLE_RATE, 6)}
    return {
        "id": asset_def["id"],
        "filename": asset_def["filename"],
        "path": f"Assets/_Project/ArtStaging/AudioV1/{asset_def['filename']}",
        "title": asset_def["title"],
        "category": asset_def["category"],
        "duration_sec": round(sample_count / SAMPLE_RATE, 6),
        "sample_rate_hz": SAMPLE_RATE,
        "bit_depth": BIT_DEPTH,
        "channels": asset_def["channels"],
        "loop": asset_def["loop"],
        "loop_points": loop_points,
        "target_lufs": asset_def["target_lufs"],
        "target_peak_dbfs": asset_def["target_peak_dbfs"],
        "measured_peak_dbfs": stats["measured_peak_dbfs"],
        "measured_rms_dbfs": stats["measured_rms_dbfs"],
        "normalization_target": f"Peak normalized to {asset_def['target_peak_dbfs']} dBFS; LUFS is mix target for final authored asset.",
        "unity_group": asset_def["unity_group"],
        "unity_import": IMPORT_GROUPS[str(asset_def["unity_group"])],
        "procedural_reference": asset_def["reference"],
        "notes": asset_def["notes"],
    }


def write_markdown(path: Path, manifest: Dict[str, object]) -> None:
    lines = [
        "# Audio V1 Manifest",
        "",
        f"Generated: {manifest['generated_at']}",
        "",
        "Scope: `Assets/_Project/ArtStaging/AudioV1/` and `Documentation/AssetProduction/AudioV1/` only.",
        "",
        "Generation command:",
        "",
        "```powershell",
        "python Documentation\\AssetProduction\\AudioV1\\generate_audio_v1_placeholders.py",
        "```",
        "",
        "## Global Source Format",
        "",
        f"- Sample rate: {SAMPLE_RATE} Hz",
        f"- Bit depth: {BIT_DEPTH}-bit PCM WAV",
        "- Loop points: loop assets use full-file loop points, sample 0 to clip sample count.",
        "- Loudness: per-asset LUFS values are final mix targets; measured RMS/peak are placeholder proxy values.",
        "- Current procedural reference: `Assets/_Project/Scripts/Utility/SteamworksAudio.cs` was read only for cue timing and sonic intent.",
        "",
        "## Unity Import Groups",
        "",
    ]
    for group, settings in IMPORT_GROUPS.items():
        lines.append(f"### {group}")
        lines.append("")
        for key, value in settings.items():
            lines.append(f"- {key}: `{value}`")
        lines.append("")
    lines.extend([
        "## Asset Inventory",
        "",
        "| ID | File | Category | Ch | Duration | Loop Points | Target | Measured | Unity Group | Notes |",
        "| --- | --- | --- | ---: | ---: | --- | --- | --- | --- | --- |",
    ])
    for item in manifest["assets"]:  # type: ignore[index]
        loop_points = item["loop_points"]
        loop_text = "none" if not loop_points else f"{loop_points['start_sample']}-{loop_points['end_sample']} samples"
        target = f"{item['target_lufs']} LUFS, peak {item['target_peak_dbfs']} dBFS"
        measured = f"peak {item['measured_peak_dbfs']} dBFS, RMS {item['measured_rms_dbfs']} dBFS"
        lines.append(f"| {item['id']} | `{item['filename']}` | {item['category']} | {item['channels']} | {item['duration_sec']:.3f}s | {loop_text} | {target} | {measured} | `{item['unity_group']}` | {item['notes']} |")
    lines.extend([
        "",
        "## Quality Notes",
        "",
        "- These are synthetic placeholders, not final authored audio.",
        "- No external recordings or samples are used.",
        "- Loop assets are built from periodic oscillators/noise; manual listening still decides if the seam is acceptable.",
        "- One-shots intentionally keep short tails so they can be swapped into current gameplay without masking combat.",
        "",
    ])
    path.write_text("\n".join(lines), encoding="utf-8")


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--project-root", type=Path, default=Path(__file__).resolve().parents[3])
    args = parser.parse_args()

    project_root = args.project_root.resolve()
    wav_dir = project_root / "Assets" / "_Project" / "ArtStaging" / "AudioV1"
    doc_dir = project_root / "Documentation" / "AssetProduction" / "AudioV1"
    wav_dir.mkdir(parents=True, exist_ok=True)
    doc_dir.mkdir(parents=True, exist_ok=True)

    records = []
    for index, asset_def in enumerate(ASSETS):
        frames, stats = render(asset_def, index)
        write_wav(wav_dir / str(asset_def["filename"]), frames, int(asset_def["channels"]))
        records.append(asset_record(asset_def, frames, stats))

    manifest = {
        "project": "Brassworks Breach",
        "generated_at": dt.datetime.now().astimezone().isoformat(timespec="seconds"),
        "generator": "Documentation/AssetProduction/AudioV1/generate_audio_v1_placeholders.py",
        "source_sample_rate_hz": SAMPLE_RATE,
        "source_bit_depth": BIT_DEPTH,
        "scope": ["Assets/_Project/ArtStaging/AudioV1", "Documentation/AssetProduction/AudioV1"],
        "unity_import_groups": IMPORT_GROUPS,
        "quality_notes": [
            "Synthetic placeholder WAVs, not final authored audio.",
            "No external samples or recordings are used; generation is deterministic.",
            "Loudness values are production targets plus measured RMS/peak proxies, not BS.1770 LUFS measurements.",
        ],
        "assets": records,
    }
    (doc_dir / "AUDIO_V1_MANIFEST.json").write_text(json.dumps(manifest, indent=2), encoding="utf-8")
    write_markdown(doc_dir / "AUDIO_V1_MANIFEST.md", manifest)

    print(f"Generated {len(records)} WAV files in {wav_dir}")
    print(f"Wrote {doc_dir / 'AUDIO_V1_MANIFEST.json'}")
    print(f"Wrote {doc_dir / 'AUDIO_V1_MANIFEST.md'}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
