# Audio V1 Manifest

Generated: 2026-05-23T23:31:08-04:00

Scope: `Assets/_Project/ArtStaging/AudioV1/` and `Documentation/AssetProduction/AudioV1/` only.

Generation command:

```powershell
python Documentation\AssetProduction\AudioV1\generate_audio_v1_placeholders.py
```

## Global Source Format

- Sample rate: 48000 Hz
- Bit depth: 16-bit PCM WAV
- Loop points: loop assets use full-file loop points, sample 0 to clip sample count.
- Loudness: per-asset LUFS values are final mix targets; measured RMS/peak are placeholder proxy values.
- Current procedural reference: `Assets/_Project/Scripts/Utility/SteamworksAudio.cs` was read only for cue timing and sonic intent.

## Unity Import Groups

### ambience_stereo_loop

- force_to_mono: `False`
- load_type: `Streaming`
- compression_format: `Vorbis`
- quality: `0.45`
- preload_audio_data: `False`
- load_in_background: `True`
- loop: `True`
- intended_source: `2D AudioSource on ambience bus, volume 0.08-0.18 per layer`

### spatial_loop_mono

- force_to_mono: `True`
- load_type: `Compressed In Memory`
- compression_format: `Vorbis`
- quality: `0.4`
- preload_audio_data: `True`
- load_in_background: `False`
- loop: `True`
- intended_source: `3D AudioSource, spatialBlend 1.0, minDistance 2-4m, maxDistance 14-24m`

### sfx_oneshot_mono

- force_to_mono: `True`
- load_type: `Decompress On Load`
- compression_format: `ADPCM`
- quality: `None`
- preload_audio_data: `True`
- load_in_background: `False`
- loop: `False`
- intended_source: `PlayOneShot or PlayClipAtPoint, SFX bus, spatialBlend by gameplay context`

### boss_oneshot_mono

- force_to_mono: `True`
- load_type: `Decompress On Load`
- compression_format: `ADPCM`
- quality: `None`
- preload_audio_data: `True`
- load_in_background: `False`
- loop: `False`
- intended_source: `3D boss AudioSource, spatialBlend 0.75-1.0, wider maxDistance than regular enemies`

## Asset Inventory

| ID | File | Category | Ch | Duration | Loop Points | Target | Measured | Unity Group | Notes |
| --- | --- | --- | ---: | ---: | --- | --- | --- | --- | --- |
| AUDV1-AMB-001 | `AUDV1_AMB_BoilerRumble_loop.wav` | Ambience | 2 | 8.000s | 0-384000 samples | -30.0 LUFS, peak -6.0 dBFS | peak -6.0 dBFS, RMS -11.1 dBFS | `ambience_stereo_loop` | Low boiler and furnace pressure bed. |
| AUDV1-AMB-002 | `AUDV1_AMB_SteamBed_loop.wav` | Ambience | 2 | 8.000s | 0-384000 samples | -32.0 LUFS, peak -8.0 dBFS | peak -8.0 dBFS, RMS -17.69 dBFS | `ambience_stereo_loop` | Wide non-directional steam wash. |
| AUDV1-AMB-003 | `AUDV1_AMB_DistantMachinery_loop.wav` | Ambience | 2 | 8.000s | 0-384000 samples | -31.0 LUFS, peak -7.0 dBFS | peak -7.0 dBFS, RMS -18.33 dBFS | `ambience_stereo_loop` | Gear and piston bed for larger industrial rooms. |
| AUDV1-AMB-004 | `AUDV1_AMB_PipeKnocks_loop.wav` | Ambience | 2 | 8.000s | 0-384000 samples | -33.0 LUFS, peak -7.0 dBFS | peak -7.0 dBFS, RMS -30.16 dBFS | `ambience_stereo_loop` | Sparse loopable pipe knocks. |
| AUDV1-AMB-005 | `AUDV1_AMB_BrassworksMix_loop.wav` | Ambience | 2 | 8.000s | 0-384000 samples | -28.0 LUFS, peak -5.0 dBFS | peak -5.0 dBFS, RMS -11.05 dBFS | `ambience_stereo_loop` | Combined audition mix; final levels should prefer separate layers. |
| AUDV1-WPN-001 | `AUDV1_WPN_PressurePistolFire.wav` | Weapon | 1 | 0.180s | none | -14.0 LUFS, peak -1.0 dBFS | peak -1.0 dBFS, RMS -8.62 dBFS | `sfx_oneshot_mono` | Sharp pneumatic snap, smaller than scattergun. |
| AUDV1-WPN-002 | `AUDV1_WPN_PressureBurst.wav` | Weapon | 1 | 0.260s | none | -13.0 LUFS, peak -1.0 dBFS | peak -1.0 dBFS, RMS -6.85 dBFS | `sfx_oneshot_mono` | Valve dump and pressure wash for pistol alternate fire. |
| AUDV1-WPN-003 | `AUDV1_WPN_ScattergunBlast.wav` | Weapon | 1 | 0.320s | none | -12.0 LUFS, peak -1.0 dBFS | peak -1.0 dBFS, RMS -4.75 dBFS | `sfx_oneshot_mono` | Heavy low-pressure blast with brass clack and steam body. |
| AUDV1-WPN-004 | `AUDV1_WPN_ScattergunSlug.wav` | Weapon | 1 | 0.360s | none | -13.0 LUFS, peak -1.0 dBFS | peak -1.0 dBFS, RMS -6.6 dBFS | `sfx_oneshot_mono` | Narrow pressure crack with pipe whistle. |
| AUDV1-WPN-005 | `AUDV1_WPN_EmptyClick.wav` | Weapon | 1 | 0.100s | none | -18.0 LUFS, peak -3.0 dBFS | peak -3.0 dBFS, RMS -15.4 dBFS | `sfx_oneshot_mono` | Short mechanical dry click. |
| AUDV1-IMP-001 | `AUDV1_IMP_MachineHit.wav` | Impact | 1 | 0.180s | none | -15.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -11.55 dBFS | `sfx_oneshot_mono` | Metal strain, plate ping, and spark crackle. |
| AUDV1-IMP-002 | `AUDV1_IMP_MachineDeathShort.wav` | Impact | 1 | 0.580s | none | -15.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -7.65 dBFS | `sfx_oneshot_mono` | Compact machine spin-down and metal drop. |
| AUDV1-ENY-001 | `AUDV1_ENY_ScrapperAttackTell.wav` | Enemy | 1 | 0.200s | none | -16.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -9.37 dBFS | `sfx_oneshot_mono` | Ratchet, pressure rise, and cutter scrape before melee damage. |
| AUDV1-ENY-002 | `AUDV1_ENY_LancerFireTell.wav` | Enemy | 1 | 0.220s | none | -16.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -9.27 dBFS | `sfx_oneshot_mono` | Valve tick and rising coil charge. |
| AUDV1-ENY-003 | `AUDV1_ENY_BulwarkAttackTell.wav` | Enemy | 1 | 0.380s | none | -14.5 LUFS, peak -1.5 dBFS | peak -1.5 dBFS, RMS -6.93 dBFS | `sfx_oneshot_mono` | Heavy boiler rise, chain drag, and pre-impact knock. |
| AUDV1-BOSS-001 | `AUDV1_BOSS_WardenStompTell.wav` | Boss | 1 | 0.580s | none | -14.0 LUFS, peak -1.5 dBFS | peak -1.5 dBFS, RMS -7.08 dBFS | `boss_oneshot_mono` | Large piston windup with low stomp pre-hit. |
| AUDV1-BOSS-002 | `AUDV1_BOSS_WardenPressureBoltTell.wav` | Boss | 1 | 0.360s | none | -15.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -8.5 dBFS | `boss_oneshot_mono` | Boss-scale pressure charge distinct from Lancer. |
| AUDV1-BOSS-003 | `AUDV1_BOSS_WardenEnrage.wav` | Boss | 1 | 0.920s | none | -14.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -7.64 dBFS | `boss_oneshot_mono` | Pressure siren and boiler swell for phase transition. |
| AUDV1-BOSS-004 | `AUDV1_BOSS_WardenShutdown.wav` | Boss | 1 | 1.350s | none | -13.5 LUFS, peak -1.5 dBFS | peak -1.5 dBFS, RMS -7.31 dBFS | `boss_oneshot_mono` | Long boss spin-down and body drop. |
| AUDV1-PCK-001 | `AUDV1_PCK_HealthPickup.wav` | Pickup | 1 | 0.220s | none | -18.0 LUFS, peak -3.0 dBFS | peak -3.0 dBFS, RMS -12.23 dBFS | `sfx_oneshot_mono` | Glass and medicinal tick. |
| AUDV1-PCK-002 | `AUDV1_PCK_AmmoPickup.wav` | Pickup | 1 | 0.200s | none | -18.0 LUFS, peak -3.0 dBFS | peak -3.0 dBFS, RMS -12.4 dBFS | `sfx_oneshot_mono` | Small pressure-cartridge tick with short hiss. |
| AUDV1-PCK-003 | `AUDV1_PCK_GearKeyPickup.wav` | Pickup | 1 | 0.440s | none | -17.0 LUFS, peak -2.5 dBFS | peak -2.5 dBFS, RMS -10.56 dBFS | `sfx_oneshot_mono` | Three-step brass chime for objective pickup. |
| AUDV1-PCK-004 | `AUDV1_PCK_WeaponPickup.wav` | Pickup | 1 | 0.480s | none | -15.5 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -10.61 dBFS | `sfx_oneshot_mono` | Brass latch, pressure rise, and acquisition chime. |
| AUDV1-INT-001 | `AUDV1_INT_GateOpen.wav` | Interaction | 1 | 0.720s | none | -15.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -8.05 dBFS | `sfx_oneshot_mono` | Motor slide, servo, and final latch. |
| AUDV1-INT-002 | `AUDV1_INT_GateDenied.wav` | Interaction | 1 | 0.260s | none | -16.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -10.02 dBFS | `sfx_oneshot_mono` | Warning buzzer and mechanical clonk. |
| AUDV1-INT-003 | `AUDV1_INT_LiftActivate.wav` | Interaction | 1 | 0.820s | none | -15.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -9.99 dBFS | `sfx_oneshot_mono` | Pulley motor, green-system chime, and steam vent. |
| AUDV1-INT-004 | `AUDV1_INT_ValveTurn.wav` | Interaction | 1 | 0.340s | none | -18.0 LUFS, peak -3.0 dBFS | peak -3.0 dBFS, RMS -10.43 dBFS | `sfx_oneshot_mono` | Ratchet clicks and pipe strain. |
| AUDV1-INT-005 | `AUDV1_INT_ValveVented.wav` | Interaction | 1 | 0.620s | none | -15.5 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -9.35 dBFS | `sfx_oneshot_mono` | Pressure release and close tick. |
| AUDV1-HAZ-001 | `AUDV1_HAZ_SteamHazardLoop.wav` | Hazard | 1 | 3.000s | 0-144000 samples | -23.0 LUFS, peak -5.0 dBFS | peak -5.0 dBFS, RMS -14.85 dBFS | `spatial_loop_mono` | Spatial active steam vent loop. |
| AUDV1-HAZ-002 | `AUDV1_HAZ_FurnaceWarningLoop.wav` | Hazard | 1 | 3.000s | 0-144000 samples | -24.0 LUFS, peak -6.0 dBFS | peak -6.0 dBFS, RMS -14.13 dBFS | `spatial_loop_mono` | Low danger pulse before active furnace heat. |
| AUDV1-HAZ-003 | `AUDV1_HAZ_FurnaceHeatPulse.wav` | Hazard | 1 | 0.880s | none | -15.0 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -7.96 dBFS | `sfx_oneshot_mono` | Heat bloom for active furnace surge. |
| AUDV1-HAZ-004 | `AUDV1_HAZ_BellowsNodePulse.wav` | Hazard | 1 | 0.440s | none | -14.0 LUFS, peak -1.5 dBFS | peak -1.5 dBFS, RMS -6.96 dBFS | `sfx_oneshot_mono` | Support-machine pressure pulse. |
| AUDV1-PLR-001 | `AUDV1_PLR_PlayerHurt.wav` | Player | 1 | 0.260s | none | -15.5 LUFS, peak -2.0 dBFS | peak -2.0 dBFS, RMS -9.77 dBFS | `sfx_oneshot_mono` | Short impact thud and pressure scrape. |

## Quality Notes

- These are synthetic placeholders, not final authored audio.
- No external recordings or samples are used.
- Loop assets are built from periodic oscillators/noise; manual listening still decides if the seam is acceptable.
- One-shots intentionally keep short tails so they can be swapped into current gameplay without masking combat.
