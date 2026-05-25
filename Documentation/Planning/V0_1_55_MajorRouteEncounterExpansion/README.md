# v0.1.55/v0.1.56 Major Route Encounter Expansion Plan

Created: 2026-05-25

Scope: implementation-ready docs packet for the next playable content leap. This packet is documentation/specification only and intentionally does not edit Unity scenes, gameplay scripts, package manifests, generated assets, release notes, build settings, or shared project status docs.

## Batch Intent

Move `Brassworks Breach` from a route-polished prototype toward a fuller first version by expanding all five levels in one coherent playable arc:

- Level01 becomes a real tutorial loop with a pressure gate preview, key branch, pump gallery, and lift sendoff.
- Level02 becomes a larger Pipeworks district around the pressure bypass, with a routing valve, Lancer hall, cartridge-cache secret, and Boilerheart lift lock.
- Level03 becomes the midgame pivot with Scattergun practice, Boilerheart valve pressure, Bellows support-machine pressure, coolant/gantry traversal, and a high Foundry lift rejoin.
- Level04 becomes the first large Foundry battle level with assembly lanes, pumpworks reroute, vertical arena, Bulwark pressure, and emergency hoist payoff.
- Level05 becomes a fuller finale with relief valves, Warden pressure phases, core breach, final exit chamber, and campaign complete flow.

## Recommended Release Split

| Version | Role | Ship Criteria |
| --- | --- | --- |
| `v0.1.55` | Visible content leap | Imports P0 sidecars, places expansion roots across Level01-Level05, adds scene-owned objective/secret hooks, places encounter waves using existing enemy authority, regenerates scenes, and passes route/encounter validation plus full matrix. |
| `v0.1.56` | First-version stabilization | Tightens objective order, encounter budgets, final-flow automation, secret totals, performance density, manual screenshot review, and packages a more stable candidate from the same expanded level shape. |

If schedule pressure allows only one big implementation release, prioritize `v0.1.55` P0 structure, objective chain, encounter beats, and validation. Hold visual density overflow and secret polish for `v0.1.56`.

## Source Evidence Read

- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Documentation/VERSION_MICRO_ROADMAP.md`
- `Documentation/LevelDesign/V0_1_50_LevelExpansionRoutes/*.md`
- `Documentation/Planning/V0_1_53_RoutePolishImplementationPacket/README.md`
- `Documentation/QA/V0_1_53_RoutePolishImplementationPacket/QA_Checklist.md`
- `Documentation/Planning/V0_1_54_GovernorCoreMaterialAndPacingPacket/README.md`
- `Documentation/QA/V0_1_54_GovernorCoreMaterialAndPacingPacket/QA_Checklist.md`
- `Documentation/Planning/V0_1_54_SteamCorridorDressingSet09ImportReadiness/README.md`
- `Documentation/Planning/V0_1_54_ClockworkEnemyPartsSet09ImportReadiness/IMPORT_READINESS_PLAN_v0.1.54.md`
- `Packages/manifest.json`

## Sidecar Package Import Plan

Current main manifest already includes eighteen sidecar packages through v0.1.53, including Set08 material detail. The v0.1.55 visible leap should import only packages that directly support the larger level/encounter read.

### P0 Import Into Main Manifest For v0.1.55

| Package | Path | Reason | Authority Rule |
| --- | --- | --- | --- |
| `com.brassworks.sidecar.room-shell-set07` | `AssetPacks/BrassworksBreach.RoomShellSet07` | Modular room/corridor shells for expanded Level01-Level05 layouts. | Visual-only; scene-owned `GEO_`/`COL_` provide blocking. |
| `com.brassworks.sidecar.interior-dressing-set07` | `AssetPacks/BrassworksBreach.InteriorDressingSet07` | Dense steampunk props for pump rooms, service corridors, and secrets. | Visual-only. |
| `com.brassworks.sidecar.hazard-props-set06` | `AssetPacks/BrassworksBreach.HazardPropsSet06` | Warning plates, gauges, scorch cards, and hazard readability props. | Visual-only; damage remains `AUTH_`. |
| `com.brassworks.sidecar.steam-fx-set06` | `AssetPacks/BrassworksBreach.SteamFXSet06` | Steam/vent/puff dressing for pressure-state feedback. | Visual-only unless paired with existing scene-owned hazard authority. |
| `com.brassworks.sidecar.objective-interactables-set05` | `AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05` | Valve, lock, lift station, and terminal dressing for expanded objectives. | Existing or new `AUTH_` objects own interactions. |
| `com.brassworks.sidecar.mechanical-enemy-elite-set05` | `AssetPacks/BrassworksBreach.MechanicalEnemyEliteSet05` | Bulwark/Warden readability upgrades and weak-point visual language. | Visual proxy only; existing enemy controllers/hitboxes remain authoritative. |
| `com.brassworks.sidecar.steam-corridor-dressing-set09` | `AssetPacks/BrassworksBreach.SteamCorridorDressingSet09` | New corridor density and threshold readability after v0.1.54 review. | Visual-only; no collision or gameplay. |
| `com.brassworks.sidecar.clockwork-enemy-parts-set09` | `AssetPacks/BrassworksBreach.ClockworkEnemyPartsSet09` | Modular enemy silhouette dressing for encounter variety. | Visual proxy only; no new AI in this batch. |

### P1 Optional Or Reference-Only

| Package or Output | Use | Rule |
| --- | --- | --- |
| `com.brassworks.sidecar.mechanical-enemy-parts-set07` | Optional enemy silhouette dressing if Set09 is too large. | Import only if validation confirms zero authority components. |
| `com.brassworks.sidecar.weapon-component-set07` | Optional Pressure Pistol/Scattergun display and bay lookdev. | Do not replace weapon gameplay in v0.1.55. |
| `com.brassworks.sidecar.hero-room-render-set07` | Lookdev reference for assembled room targets. | Keep review-only unless main lane explicitly approves package import. |
| `BrassworksBreach.UnityMaterialCorridorValidation09` | Unity material validation project/reference. | Do not import as a package. Use findings only. |
| `BrassworksBreach.UnityWeaponMaterialCellLookdev09` | Unity weapon/material lookdev project/reference. | Do not import as a package. Use findings only. |
| `roomtest` v0.3-v0.5 outputs | Visual reference for brick chamber material direction. | Keep outside runtime game assets. |

## P0 Group A - Expansion Roots And Scene Structure

Goal: give every level a stable root so later implementation can regenerate, validate, and selectively tune without mixing sidecar visual content into existing route roots.

Required roots:

- `EXPAN_L01_IntakeFoundations_v0_1_55`
- `EXPAN_L02_PipeworksPressureDistrict_v0_1_55`
- `EXPAN_L03_BoilerheartFoundryBridge_v0_1_55`
- `EXPAN_L04_FoundryAssemblyWorks_v0_1_55`
- `EXPAN_L05_GovernorCoreFinale_v0_1_55`

Each root requires `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and one `VISUALONLY_...` child container named in the scene expectations file.

Implementation notes:

- Keep existing generated scenes and transition objects as the authority baseline.
- Stitch new roots to existing route nodes rather than replacing the whole scene at once.
- Prefer scene-owned primitive blockers for collision during v0.1.55, with sidecar shells as visual dressing only.
- Add checkpoint triggers at major entry/arena/rejoin beats only if existing checkpoint patterns support them.

Validation target: `ValidateV0155MajorRouteEncounterExpansion(sceneName)` requires roots, containers, key geometry names, and no sidecar authority leakage.

## P0 Group B - Objective And Secret Chain

Goal: every level should have a readable objective loop and one optional mastery secret by v0.1.56.

### Level01

- Required: gear key, gate preview, pressure gate, pump gallery, service lift sendoff.
- Secret: `TRG_L01_Secret_IntakePressureCache_Expanded`.
- Do not make the secret part of the lift route.

### Level02

- Required: pressure bypass state, `AUTH_L02_Valve_RoutingDeck_C`, Boilerheart lift lock.
- Secret: `TRG_L02_Secret_CartridgeCacheExpanded`.
- Existing bypass valves remain required only if the bypass is on the main route; if made optional, validator wording must change.

### Level03

- Required: Scattergun acquisition or confirmation, Boilerheart pressure valve, hazard/lift pressure state, Foundry lift.
- Secret: `TRG_L03_Secret_CrucibleShelfExpanded`.
- Coolant valve can be optional or required, but the implementation must label it honestly.

### Level04

- Required: pressure key, pump reroute, assembly-floor/pump-arena clear, emergency hoist.
- Secrets: coal cache and optional return duct if existing pumpworks secret is preserved.
- Hoist lockroom must not open before the required arena clear.

### Level05

- Required: north relief valve, south relief valve, Warden/core breach, final exit interaction, campaign complete trigger/panel.
- Secret: `TRG_L05_Secret_RegulatorNicheExpanded`.
- Final exit must never require secret discovery.

Validation target: `ValidateV0155ObjectiveSecretChain(sceneName)` checks required `AUTH_` and `TRG_` names, optional-secret completeness, and objective order where positions are deterministic.

## P0 Group C - Encounter Beats And Budgets

Goal: use existing enemy gameplay roles to create a visible combat escalation without blocking on new AI.

| Level | Encounter Identity | Peak Active Cap |
| --- | --- | ---: |
| Level01 | Tutorial Scrapper pressure plus first mixed pump-gallery fight. | `6` |
| Level02 | Pipeworks cover, bypass valves, Lancer sightlines, and lift lockhouse guard. | `8` |
| Level03 | Scattergun close-range practice, Bellows pressure, coolant/gantry vertical fight. | `9` |
| Level04 | Wide assembly-floor fight, pump reroute pressure, Bulwark vertical arena. | `10` |
| Level05 | Relief-side objectives, Warden teach/combine phases, final exit breath. | `11` |

Implementation notes:

- Do not introduce new enemy gameplay classes in this batch. Use Scrapper, Lancer, Bulwark, Warden, and existing support machines.
- Clockwork Enemy Parts Set 09 and Elite Set 05 can visually differentiate enemies only if authority stays on the existing enemy objects.
- Encounter waves should be named under `SPAWN_L##_Wave_BeatName`.
- Every major combat room needs at least one pickup placed before the spike, not after the player is already trapped.

Validation target: `ValidateV0155EncounterBudgets(sceneName)` checks named wave roots, active caps, and minimum combat-room widths.

## P0 Group D - Level-Specific Visible Leaps

### Level01

- Add the pump gallery as the first "this is bigger now" room.
- Add gate-preview sightline before gear key.
- Add Steam Corridor Dressing Set 09 along the lift runway and pump gallery margins.

### Level02

- Add the north Lancer hall with strong cover rhythm.
- Extend the pressure bypass into a district-scale objective chain.
- Add cartridge-cache secret as readable pressure-system mastery.

### Level03

- Make the Scattergun bay and practice lane a memorable weapon moment.
- Connect Boilerheart valve, Bellows Node, coolant route, and high gantry into one coherent bridge.
- Use enemy visual dressing sparingly so combat clarity survives the vertical route.

### Level04

- Add a large assembly floor and a vertical pump arena with Bulwark pressure.
- Make pump reroute visibly change pressure jets before the arena wave.
- Hoist lockroom should feel like a earned exit, not a hallway afterthought.

### Level05

- Add a readable pressure ring, relief side routes, Warden pressure phases, core breach, and final exit chamber.
- Use Set08 material language from v0.1.53/v0.1.54: red for danger, amber for objective/exit confirmation, wet black stone for safe movement contrast.
- Let the final exit breathe after Warden/core breach.

## P0 Group E - Validation Hooks

Recommended editor validator helpers:

- `ValidateV0155MajorRouteEncounterExpansion(sceneName)`
- `ValidateV0155SidecarImportEnvelope()`
- `ValidateV0155VisualOnlyAuthority(sceneName)`
- `ValidateV0155ObjectiveSecretChain(sceneName)`
- `ValidateV0155EncounterBudgets(sceneName)`
- `ValidateV0155CampaignScaleAndRouteOrder(sceneName)`
- `ValidateV0156FirstVersionStabilization(sceneName)`, added during v0.1.56 once v0.1.55 roots are stable.

Expected pass markers:

- `V0_MAJOR_ROUTE_ENCOUNTER_PASS`
- `V0_V0155_SIDECAR_IMPORT_PASS`
- `V0_VISUALONLY_AUTHORITY_PASS`
- `V0_FIRST_VERSION_OBJECTIVE_CHAIN_PASS`
- `V0_ENCOUNTER_BUDGET_PASS`
- `V0_CAMPAIGN_SCALE_ROUTE_ORDER_PASS`
- `V0_FIRST_VERSION_STABILIZATION_PASS`, v0.1.56

## P0 Group F - Runtime Smoke Additions

Recommended runtime tests or extensions:

- Add `RuntimeMajorRouteEncounterTest` to load Level01-Level05 and verify expansion roots, labels, objective hooks, and wave roots.
- Extend `RuntimeAutoPlaythroughTest` through the new objective chain: Level01 key/gate/lift, Level02 routing deck/lift, Level03 scattergun/Boilerheart valve/lift, Level04 key/reroute/arena/hoist, Level05 relief valves/core breach/final exit.
- Add `RuntimeEncounterBudgetTest` to run deterministic wave spawns with player protection and confirm peak active caps.
- Extend `RuntimeSecretTest` to confirm one optional secret per level by v0.1.56, while still allowing v0.1.55 to ship with documented optional cuts.
- Extend `RuntimeHazardTest` for new hazard tells, not just damage volumes.
- Extend `RuntimeWardenCombatTest` or add `RuntimeGovernorFinaleFlowTest` for relief valves, Warden pressure tell order, core breach, and campaign complete.
- Extend sidecar quarantine smoke so it reports exact imported P0 packages and confirms all sidecar instances are under `VISUALONLY_` roots.

## Main-Lane Implementation Order

1. Read this packet and the two level-design files in the same folder group.
2. Import only the P0 sidecar packages that pass quarantine checks.
3. Add validator support for package names and visual-only authority before placing sidecar instances.
4. Add expansion roots/containers to all five generated levels.
5. Block out `GEO_`, `COL_`, `AUTH_`, `TRG_`, and `SPAWN_` objects first.
6. Place sidecar visuals only under `VISUALONLY_` containers.
7. Wire objective state using existing systems wherever possible.
8. Add encounter waves with existing enemy roles and enforce active caps.
9. Add runtime smoke coverage for objective order, encounter budgets, secrets, hazards, sidecar authority, and final flow.
10. Regenerate scenes, run validation, route audit, runtime smoke, full Windows matrix, QA packet, issue triage, candidate readiness, and packaging.
11. Use v0.1.56 for stabilization passes and any cut P1 visual density only after v0.1.55 P0 gates pass.

## Human Review Evidence

Capture screenshots outside runtime game assets:

- Level01: gate preview, pump gallery, lift sendoff.
- Level02: valve deck, north Lancer hall, Boilerheart lift lock.
- Level03: Scattergun bay, Boilerheart core, upper gantry/high rejoin.
- Level04: assembly floor, pump reroute, vertical pump arena, emergency hoist.
- Level05: pressure ring, relief valves, Warden core, core breach gate, final exit chamber.

Save review-only images under a documentation render folder if a later lane owns that root. This packet does not create render outputs.

## Key Risks

- Importing too many packages at once can obscure authority regressions. Add sidecar authority validation before scene dressing.
- Level05 Warden AI may not support every proposed pressure phase. If so, keep the relief valves and tell objects as deterministic hooks and ship the finale as a pressure-arena flow pass.
- Larger levels can become confusing if labels and rejoin cues lag behind geometry. Place labels and objective light language before decorative density.
- Encounter caps are part of the design, not soft advice; raising them should require a balancing note and smoke update.
- v0.1.56 should stabilize v0.1.55, not start a new unrelated feature branch.
