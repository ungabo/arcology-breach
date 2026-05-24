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

## Near-Term Proposed Sequence

| Version | Planned Slice | Intent | Expected Verification |
| --- | --- | --- | --- |
| v0.1.5 | Level01 Brassworks Intake flow polish | Tighten the opening map around key visibility, gate preview, readable combat cover, pickup placement, secret route clarity, and first-time player onboarding. | Full V0 matrix; auto-playthrough and level validation cover the revised route, with a packaged route playtest afterward. |
| v0.1.6 | Level02 and Level03 midgame pacing polish | Improve Pipeworks Annex sightlines, routing-valve readability, Boilerheart lift gating, Bellows Node staging, scattergun pickup approach, and hazard shutdown comprehension. | Full V0 matrix; ranged, Bellows, weapon-switch, hazard, secret, and auto-playthrough smokes remain green. |
| v0.1.7 | Level04 and Level05 climax polish | Strengthen foundry hazard choreography, Bulwark arena spacing, Governor Core staging, Warden approach, boss readability, and final hoist feedback. | Full V0 matrix; Bulwark, Warden, hazard, interaction, secret, and auto-playthrough smokes remain green. |
| v0.1.8 | AudioV1 listen, mix, and import tuning | Perform the first human-oriented audio mix pass: cue volumes, ambience loop balance, compression/import settings, hazard audibility, enemy tell audibility, and menu/gameplay loudness consistency. | Full V0 matrix; runtime smoke still confirms authored cue routing, plus a packaged listen pass records mix outcomes. |
| v0.1.9 | Settings, readability, and Windows options polish | Complete remaining settings/accessibility items: resolution/fullscreen controls, color/readability adjustments, prompt legibility, objective text sizing, and menu consistency. | Full V0 matrix; pause-flow and runtime smoke cover changed settings, and visual validation confirms no broken UI sprite wiring. |
| v0.1.10 | Final-direction asset promotion pass | Promote the best available Unity-only lookdev into playable assets where safe: pressure-pistol component improvements, environment material cleanup, non-magenta corridor-kit recovery, and richer mechanical enemy silhouettes. | Full V0 matrix; level validation covers promoted assets, and concept render acceptance rejects shader-error/magenta outputs before promotion. |
| v0.1.11 | Windows distribution polish pass | Harden the first polished Windows package: performance profile review, build naming, icon/splash metadata where available, readme/release notes, smoke-tested quit/restart flow, and distributable folder cleanup. | Full V0 matrix; packaged Windows executable launches, plays, pauses, restarts, quits, and reaches win state on the current route. |

## Later Follow-Up Areas

- Broader level-map rebuilds after the first Windows polish sequence proves the route.
- Final authored geometry, textures, animation, VFX, and audio replacement passes.
- Account-owned Unity Asset Store pack review through Package Manager if access becomes available.
- Platform asset rules for later Android, WebGL, SteamVR, and Meta Quest ports, kept deferred until the Windows game is ready.
- GitHub Issues mirroring after the near-term tuning pass produces stable task clusters.

Next-step directive: continue immediately with the next highest-impact unfinished task.
