# Audio V1 Asset Plan

Scope: independent V1 audio production and staging lane. This package does not edit gameplay scripts, scenes, shared docs, README, build status files, or current procedural audio code.

Staged asset root: `Assets/_Project/ArtStaging/AudioV1/`

Documentation root: `Documentation/AssetProduction/AudioV1/`

Generation command:

```powershell
python Documentation\AssetProduction\AudioV1\generate_audio_v1_placeholders.py
```

## Intent

V1 audio should move the project from single procedural cues toward swappable, mixable WAV assets while keeping the current gameplay implementation untouched. `SteamworksAudio.cs` was used as read-only reference for cue names, timing, and current procedural tone. These WAVs are placeholders for production staging, not final authored audio.

## Source Format

- Source files: 48 kHz, 16-bit PCM WAV.
- One-shots: mono, short tails, peak normalized per manifest.
- Ambience: stereo loops, full-file loop points.
- Spatial loops: mono loops for hazards and localized machines.
- Loudness targets: stored per asset in `AUDIO_V1_MANIFEST.json` and `AUDIO_V1_MANIFEST.md`.
- Provenance: deterministic procedural synthesis, no external recordings or third-party samples.

## Families

| Family | Staged Files | Production Goal |
| --- | ---: | --- |
| Brassworks ambience | 5 | Layer boiler rumble, steam bed, distant machinery, pipe knocks, and preview mix. |
| Weapons | 5 | Separate pistol primary, Pressure Burst, scattergun blast, slug shot, and empty click. |
| Impacts | 2 | Machine hit and compact shutdown cues. |
| Enemy tells | 3 | Scrapper, Lancer, and Bulwark pre-damage/pre-shot readability. |
| Warden boss | 4 | Stomp tell, pressure-bolt tell, enrage, and shutdown cues. |
| Pickups | 4 | Health, ammo, gear key, and weapon acquisition. |
| Interactions | 5 | Gate open, gate denied, lift activate, valve turn, valve vented. |
| Hazards | 4 | Steam hazard loop, furnace warning loop, furnace heat pulse, Bellows Node pulse. |
| Player | 1 | Player hurt placeholder for mix comparison. |

## Priority Pass

1. Combat readability: weapon shots, machine hit, Scrapper/Lancer/Bulwark tells, Warden tells.
2. Objective feedback: pickups, gate denied/open, lift activate, valve turn/vent.
3. Hazard readability: steam/furnace/Bellows cues with spatial rolloff and low fatigue.
4. Ambience identity: layer quiet brassworks beds without masking combat.

## Replacement Mapping

| Current Procedural Cue | V1 Candidate |
| --- | --- |
| `PressureFire` | `AUDV1_WPN_PressurePistolFire.wav` |
| `PressureBurst` | `AUDV1_WPN_PressureBurst.wav` |
| `SteamScattergunFire` | `AUDV1_WPN_ScattergunBlast.wav` |
| `SteamScattergunSlug` | `AUDV1_WPN_ScattergunSlug.wav` |
| `EmptyClick` | `AUDV1_WPN_EmptyClick.wav` |
| `EnemyHit` | `AUDV1_IMP_MachineHit.wav` |
| `EnemyDeath` | `AUDV1_IMP_MachineDeathShort.wav` |
| `EnemyAttackTell` | `AUDV1_ENY_ScrapperAttackTell.wav` |
| `LancerFireTell` | `AUDV1_ENY_LancerFireTell.wav` |
| `BulwarkAttackTell` | `AUDV1_ENY_BulwarkAttackTell.wav` |
| `HealthPickup` | `AUDV1_PCK_HealthPickup.wav` |
| `AmmoPickup` | `AUDV1_PCK_AmmoPickup.wav` |
| `GearKey` | `AUDV1_PCK_GearKeyPickup.wav` |
| `WeaponPickup` | `AUDV1_PCK_WeaponPickup.wav` |
| `GateOpen` | `AUDV1_INT_GateOpen.wav` |
| `GateDenied` | `AUDV1_INT_GateDenied.wav` |
| `Win` or lift activation feedback | `AUDV1_INT_LiftActivate.wav` |
| `BellowsNodePulse` | `AUDV1_HAZ_BellowsNodePulse.wav` |
| `PlayerHurt` | `AUDV1_PLR_PlayerHurt.wav` |

## Quality Bar For Authored Replacement

- Cues must remain readable at current gameplay tempo and not exceed placeholder duration by more than 25 percent unless code timing is updated later.
- Enemy tells must communicate threat before damage or projectile release.
- Weapon family hierarchy should be obvious: pistol small, Pressure Burst wider, scattergun largest, slug narrow and punchy.
- Ambience and hazards must survive repeated 3-minute listening without obvious loop ticks or fatigue.
- Mobile and WebGL variants can use shorter loops, lower Vorbis quality, and fewer simultaneous ambience layers.
