# Brassworks Breach - Version Micro Roadmap

Last updated: 2026-05-23

## Purpose

This document lists planned small versioned development slices before they are built. It makes the rolling `v0.0.x` sequence visible instead of leaving every next step implicit.

The roadmap is a living plan, not a promise that every version number is immutable. If a test failure, dependency, or better grouping changes the order, update this file and record the change in `SESSION_LOG.md`.

## Source Of Truth Chain

- `WORK_LEDGER.md`: active milestone, completed work, ready/backlog tasks.
- `IMPLEMENTATION_TODO.md`: versioned task breakdowns as they are started or completed.
- `AAA_ASSET_CATALOG.md`: asset, VFX, audio, animation, UI, enemy, and level needs.
- `LEVEL_DESIGN_AND_MAPS.md`: map scale, level flow, transition mechanics, platform constraints.
- `BUILD_STATUS.md`: latest verified version and test evidence.
- `SESSION_LOG.md`: timestamped execution record.

## Planning Rules

- Each `v0.0.x` should be one meaningful playable slice, not every tiny edit.
- Prefer batching related code, asset, audio, VFX, and smoke coverage into one version.
- Cheap checks can run during development; the full build matrix runs once per completed slice.
- Commit and push after each verified versioned slice.
- If a planned item proves too large, split it into the next open version numbers and update this document.

## Near-Term Planned Sequence

| Version | Planned Slice | Intent | Expected Verification |
| --- | --- | --- | --- |
| v0.0.84 | Steam Scattergun slug identity | Completed: right-mouse scattergun slug now has its own audio/VFX and smoke coverage. | Full V0 matrix passed; weapon-switch smoke verifies slug cue/VFX. |
| v0.0.85 | Scattergun world pickup art polish | Completed: Level03 world pickup now has a display stand, yoke, nameplate, brass/walnut weapon details, pressure coil, rear valve wheel, and shell rack. | Full V0 matrix passed; level validation checks named pickup visual pieces. |
| v0.0.86 | Scattergun pickup placement/readability | Completed: added brass route strips, floor chevrons, pressure pipes, lamps, and `BREACH TOOL` label around the Level03 pickup. | Full V0 matrix passed; level validation and weapon-switch smoke passed. |
| v0.0.87 | Pressure Pistol secondary feedback | Completed: Pressure Burst now has dedicated pressure-dump audio and pressure/steam/brass VFX. | Full V0 matrix passed; combat-scenario smoke verifies secondary feedback. |
| v0.0.88 | Pressure Pistol viewmodel polish | Completed: Pressure Burst now drives pistol gauge, valve, lever, chamber, and vent viewmodel motion cues; first staged PBR material, enemy, and weapon/prop packs were added. | Full V0 matrix passed; combat-scenario smoke verifies secondary viewmodel feedback. |
| v0.0.89 | Scrapper attack readability pass | Improve melee windup visuals/audio so the first enemy reads better in motion. | Full V0 matrix; combat-edge smoke verifies player damage path still works. |
| v0.0.90 | Scrapper shutdown polish | Add richer standard Scrapper shutdown pieces without changing combat balance. | Full V0 matrix; combat smoke verifies death VFX. |
| v0.0.91 | Lancer firing tell pass | Add clearer valve-rifle charge/readiness feedback before pressure bolts. | Full V0 matrix; ranged-combat smoke. |
| v0.0.92 | Lancer projectile impact feedback | Add distinct pressure-bolt impact feedback on player/world collision. | Full V0 matrix; ranged-combat and combat-edge smoke. |
| v0.0.93 | Bulwark attack readability pass | Improve heavy enemy hammer/windup threat cues. | Full V0 matrix; Bulwark combat smoke. |
| v0.0.94 | Bulwark shutdown polish | Add heavier furnace/boiler death feedback to distinguish it from smaller machines. | Full V0 matrix; Bulwark combat smoke. |
| v0.0.95 | Bellows Node encounter readability | Add stronger spatial/visual cues around the support-machine role and boost radius. | Full V0 matrix; Bellows Node smoke. |
| v0.0.96 | Bellows Node level dressing | Add local pipes, warnings, and service machinery around the Level03 Bellows Node. | Full V0 matrix; level validation plus Bellows Node smoke. |
| v0.0.97 | Furnace hazard audio/readability | Add dedicated hazard loop/pulse cues for steam and furnace heat hazards. | Full V0 matrix; hazard smoke. |
| v0.0.98 | Secret cache readability pass | Improve visual language for secret cache entrances/rewards without making them mandatory. | Full V0 matrix; secret smoke and auto-playthrough. |
| v0.0.99 | Objective device feedback pass | Add more readable feedback to valves/lifts/locks after interaction. | Full V0 matrix; auto-playthrough and interaction smoke. |
| v0.0.100 | First route cohesion pass | Tighten the five-level prototype route with accumulated readability and pacing adjustments. | Full V0 matrix; manual playtest recommended after build. |

## Beyond v0.0.100

Planned follow-up areas:

- Level01 combat-space rebuild toward a stronger first map.
- Level02 sightline and ranged-combat expansion.
- Level03 Boilerheart route expansion around valve, pickup, Bellows Node, and foundry lift.
- Level04 foundry encounter pacing and hazard choreography.
- Level05 Governor Core finale readability and boss staging.
- More final-direction steampunk material/mesh passes.
- Platform asset rules for later Android, WebGL, SteamVR, and Meta Quest ports.

Next-step directive: continue immediately with the next highest-impact unfinished task.
