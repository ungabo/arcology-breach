# Brassworks Breach - Version Micro Roadmap

Last updated: 2026-05-24

## Purpose

This document lists planned small versioned development slices before they are built. It makes the rolling `v0.1.x` sequence visible instead of leaving every next step implicit.

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

## Completed Recent History

| Version | Planned Slice | Intent | Expected Verification |
| --- | --- | --- | --- |
| v0.0.94 | SignageDecalsV1 playable integration | Completed: static staged-signage and decal quads are generated in Level01, Level03, and Level05 for objective plates, warning signage, route arrows, machinery labels, and service/secret marks. | Full V0 matrix passed; scene validation checks expected signage objects and staged texture references. |
| v0.0.95 | FinalMaterialsV1 gameplay binding | Completed: active generated materials now reference staged BaseColor, Normal, and ORM texture maps for soot brick, wet stone, riveted iron, brass, copper, walnut, enamel gauge, amber glass, and hazard enamel. | Full V0 matrix passed; validator coverage checks texture references on gameplay materials. |
| v0.0.96 | North-star environment density pass | Completed: generated pipe canopies, caged amber gaslights, rivet bands, foundry rail details, and Governor regulator-crown dressing across the campaign. | Full V0 matrix passed; level validation checks named environment-density pieces. |
| v0.0.97 | UIHudV1 playable UI integration | Completed: active HUD, main menu, pause menu, reticle, prompt backplate, key lamp, objective panel, and boss gauge use staged steampunk UI sprites. | Full V0 matrix passed; validator coverage checks sprite import and UI wiring. |
| v0.0.98 | Prompt icons and denied key feedback | Completed: interaction prompts show context icons and locked pressure gates flash the denied key-lamp sprite. | Full V0 matrix passed; interaction smoke verifies prompt artwork and denial feedback. |
| v0.0.99 | AudioV1 authored cue integration | Completed: `SteamworksAudio` prefers staged AudioV1 WAV ambience and cue clips for every gameplay cue while retaining procedural fallback. | Full V0 matrix passed through `v099b`; validator and runtime smoke require authored AudioV1 routing. |
| v0.1.0 | Flash-intensity accessibility setting | Completed: main and pause menus expose a persisted flash-intensity slider, and that value scales HUD damage flash plus first-person player damage VFX. | Full V0 matrix passed; scene validation checks slider wiring/range and pause-flow smoke verifies runtime behavior. |
| v0.1.1 | Bulwark shutdown polish | Completed: Bulwark deaths now use a dedicated heavy boiler/furnace `MachineDeathVfx` style instead of the generic scaled machine burst. | Full V0 matrix passed; Bulwark combat smoke requires Bulwark-specific shutdown detail. |
| v0.1.2 | Internal Windows route audit and issue capture | Completed: added deterministic route audit tooling/reporting for Level01-Level05 and generated `ROUTE_AUDIT_v0.1.2.md`; no route-blocking composition issues were found. | Route audit passed; full V0 matrix passed. |
| v0.1.3 | Core movement and camera feel tuning | Completed: player movement now accelerates/decelerates from centralized `GameBalance` values, camera pitch/sensitivity are clamped, and packaged movement-feel smoke coverage is part of the matrix. | Route audit passed; full V0 matrix passed with `V0_MOVEMENT_FEEL_PASS`. |
| v0.1.4 | Weapon, ammo, and enemy pressure balance | Completed: tuned weapon damage/cadence/ammo, pickup amounts, and enemy pressure values while adding a packaged balance-envelope smoke test. | Route audit passed; full V0 matrix passed with `V0_BALANCE_ENVELOPE_PASS`. |
| v0.1.5 | Level01 Brassworks Intake flow polish | Completed: generated gate-preview sightline props, key-branch return cues, service-lift green runway/beacon cues, and secret-cache clue props, with dedicated packaged smoke coverage. | Route audit passed; full V0 matrix passed with `V0_LEVEL01_FLOW_PASS`. |
| v0.1.6 | Level02 and Level03 midgame pacing polish | Completed: generated Pipeworks locked-lift/valve/Lancer/secret cues and Boilerheart ring/scattergun/Bellows/valve-return/foundry-lift cues, with dedicated midgame smoke coverage. | Route audit passed; full V0 matrix passed with `V0_MIDGAME_FLOW_PASS`. |
| v0.1.7 | Level04 and Level05 climax polish | Completed: generated Foundry heat/Bulwark/hoist/secret cues and Governor Warden/final-hoist cues, with dedicated climax smoke coverage. | Route audit passed; full V0 matrix passed with `V0_CLIMAX_FLOW_PASS`. |
| v0.1.8 | AudioV1 mix and import tuning | Completed: `SteamworksAudio` now has serialized per-cue volume/spatial-intent mix bindings, AudioV1 loop/one-shot import settings are tuned, and a packaged audio-mix smoke test verifies the profile. | Route audit passed; full V0 matrix passed with `V0_AUDIO_MIX_PASS`. |
| v0.1.9 | Settings, readability, and Windows options polish | Completed: persisted resolution/fullscreen controls now exist on main and pause menus, HUD prompt/objective/message readability constraints are validated, stale Unity lock cleanup is in the runners, and packaged display-settings smoke coverage is part of the matrix. | Route audit passed; full V0 matrix passed with `V0_DISPLAY_SETTINGS_PASS`. |
| v0.1.10 | High-contrast readability controls | Completed: persisted high-contrast controls now exist on main and pause menus, HUD text/backplates respond at runtime, and packaged readability-settings smoke coverage is part of the matrix. | Route audit passed; full V0 matrix passed with `V0_READABILITY_SETTINGS_PASS`. |
| v0.1.11 | Pressure gauge asset promotion | Completed: kept lookdev renders reference-only, promoted one Unity-owned pressure-gauge component language into the Pressure Pistol viewmodel and Level01 pressure gate, and added `PressureGaugePrototype` metadata/validator coverage. | Route audit passed; full V0 matrix passed with pressure-gauge validation included. |
| v0.1.12 | Windows distribution packaging | Completed: added a Windows package script, wired it into the full matrix, and generated a package folder/ZIP with README, manifest, and SHA-256 hash. | Route audit passed; full V0 matrix passed with `V0_WINDOWS_PACKAGE_PASS`. |

## Near-Term Proposed Sequence

| Version | Planned Slice | Intent | Expected Verification |
| --- | --- | --- | --- |
| v0.1.13 | Route cohesion and manual-playtest automation prep | Convert the current route audit/manual sheets into a tighter repeatable QA loop for Windows route feel and later platform passes. | Route audit plus full V0 matrix; manual-playtest docs updated with exact current build paths and route notes. |
| v0.1.14 | Next safe asset-promotion slice | Promote the next single component after review, likely a pressure-pistol coil pack or reusable wall/pipe gauge cluster, with the same metadata/validation discipline used in `v0.1.11`. | Full V0 matrix; validation covers named component hierarchy/material roles and no route readability regression. |
| v0.1.15 | Windows candidate-release cleanup | Fold in manual route feedback, tighten release docs, and prepare the first candidate Windows distribution checklist. | Full V0 matrix plus package verification and updated release notes. |

## Later Follow-Up Areas

- Broader level-map rebuilds after the first Windows polish sequence proves the route.
- Final authored geometry, textures, animation, VFX, and audio replacement passes.
- Account-owned Unity Asset Store pack review through Package Manager if access becomes available.
- Platform asset rules for later Android, WebGL, SteamVR, and Meta Quest ports, kept deferred until the Windows game is ready.
- GitHub Issues mirroring after the near-term tuning pass produces stable task clusters.

Next-step directive: continue immediately with the next highest-impact unfinished task.
