# v0.1.55/v0.1.56 Major Route Encounter Expansion QA Checklist

Created: 2026-05-25

Use this checklist when the main lane implements the major route/encounter expansion packet. This docs lane did not edit code, scenes, manifests, generated assets, release notes, build docs, or shared status docs.

## Entry Gate

- [ ] Implementation lane has read:
  - [ ] `Documentation/LevelDesign/V0_1_55_MajorRouteEncounterExpansion/CAMPAIGN_MAP_AND_ENCOUNTER_SPEC_v0.1.55_v0.1.56.md`
  - [ ] `Documentation/LevelDesign/V0_1_55_MajorRouteEncounterExpansion/SCENE_OBJECT_EXPECTATIONS_v0.1.55_v0.1.56.md`
  - [ ] `Documentation/Planning/V0_1_55_MajorRouteEncounterExpansion/README.md`
  - [ ] `Documentation/Planning/V0_1_55_MajorRouteEncounterExpansion/major_route_encounter_expansion_packet.json`
- [ ] v0.1.55 is treated as a coherent playable content leap, not isolated object placement.
- [ ] v0.1.56 is reserved for stabilization of the same expanded campaign shape.
- [ ] Existing route roots and transition chain remain present unless deliberately migrated with validator coverage.
- [ ] No sidecar package is imported without a package-name, manifest, compile, and visual-only authority check.

## P0 Sidecar Import Gate

- [ ] Import only approved P0 packages unless a documented implementation owner accepts P1 scope:
  - [ ] `com.brassworks.sidecar.room-shell-set07`
  - [ ] `com.brassworks.sidecar.interior-dressing-set07`
  - [ ] `com.brassworks.sidecar.hazard-props-set06`
  - [ ] `com.brassworks.sidecar.steam-fx-set06`
  - [ ] `com.brassworks.sidecar.objective-interactables-set05`
  - [ ] `com.brassworks.sidecar.mechanical-enemy-elite-set05`
  - [ ] `com.brassworks.sidecar.steam-corridor-dressing-set09`
  - [ ] `com.brassworks.sidecar.clockwork-enemy-parts-set09`
- [ ] Sidecar import smoke reports exact imported package names.
- [ ] Sidecar import smoke reports package count delta from the v0.1.53 baseline rather than relying on stale hardcoded counts.
- [ ] All sidecar instances in gameplay scenes live under `VISUALONLY_` containers.
- [ ] No object under `VISUALONLY_` has colliders, rigidbodies, gameplay scripts, audio sources, cameras, lights, transition triggers, damage volumes, or objective authority.

## Required Validator Additions

- [ ] Add `ValidateV0155MajorRouteEncounterExpansion(sceneName)`.
- [ ] Add `ValidateV0155SidecarImportEnvelope()`.
- [ ] Add `ValidateV0155VisualOnlyAuthority(sceneName)`.
- [ ] Add `ValidateV0155ObjectiveSecretChain(sceneName)`.
- [ ] Add `ValidateV0155EncounterBudgets(sceneName)`.
- [ ] Add `ValidateV0155CampaignScaleAndRouteOrder(sceneName)`.
- [ ] During v0.1.56, add `ValidateV0156FirstVersionStabilization(sceneName)`.
- [ ] Expected validation markers are emitted only after real checks pass:
  - [ ] `V0_MAJOR_ROUTE_ENCOUNTER_PASS`
  - [ ] `V0_V0155_SIDECAR_IMPORT_PASS`
  - [ ] `V0_VISUALONLY_AUTHORITY_PASS`
  - [ ] `V0_FIRST_VERSION_OBJECTIVE_CHAIN_PASS`
  - [ ] `V0_ENCOUNTER_BUDGET_PASS`
  - [ ] `V0_CAMPAIGN_SCALE_ROUTE_ORDER_PASS`
  - [ ] `V0_FIRST_VERSION_STABILIZATION_PASS`, v0.1.56

## Scene Root Checks

- [ ] Level01 includes `EXPAN_L01_IntakeFoundations_v0_1_55` with `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and `VISUALONLY_L01_IntakeFoundations`.
- [ ] Level02 includes `EXPAN_L02_PipeworksPressureDistrict_v0_1_55` with `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and `VISUALONLY_L02_PipeworksPressureDistrict`.
- [ ] Level03 includes `EXPAN_L03_BoilerheartFoundryBridge_v0_1_55` with `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and `VISUALONLY_L03_BoilerheartBridge`.
- [ ] Level04 includes `EXPAN_L04_FoundryAssemblyWorks_v0_1_55` with `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and `VISUALONLY_L04_FoundryAssembly`.
- [ ] Level05 includes `EXPAN_L05_GovernorCoreFinale_v0_1_55` with `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and `VISUALONLY_L05_GovernorFinale`.
- [ ] v0.1.56 `POLISH_...` roots are additive and do not rename v0.1.55 P0 objects.

## Objective And Secret Checks

### Level01

- [ ] Player can see the pressure gate before collecting the gear key.
- [ ] `AUTH_L01_Item_GearKey_Expanded` is reachable and cannot soft-lock the route.
- [ ] `AUTH_L01_PressureGate_Expanded` opens only when intended.
- [ ] `AUTH_L01_LiftCallBox_Expanded` or existing lift authority loads Level02.
- [ ] `TRG_L01_Secret_IntakePressureCache_Expanded` is optional.

### Level02

- [ ] Existing `ROUTE_L02_PressureBypass_v0_1_50` requirements still pass.
- [ ] `AUTH_L02_Valve_RoutingDeck_C` participates in the Boilerheart lift unlock if marked required.
- [ ] `AUTH_L02_LiftLock_BoilerheartExpanded` rejects early use and unlocks after required pressure state.
- [ ] `TRG_L02_Secret_CartridgeCacheExpanded` is optional and complete with reward objects if included.

### Level03

- [ ] Existing `ROUTE_L03_FoundryGantry_v0_1_50` requirements still pass.
- [ ] `AUTH_L03_Weapon_SteamScattergun_Expanded` confirms or grants the Scattergun before the practice lane.
- [ ] `AUTH_L03_BoilerheartPressureValve_Expanded` changes hazard/lift state clearly.
- [ ] `AUTH_L03_FoundryLiftLock_Expanded` loads Level04 only after required pressure state.
- [ ] `TRG_L03_Secret_CrucibleShelfExpanded` remains optional.

### Level04

- [ ] Existing `ROUTE_L04_ObservatoryPumpworks_v0_1_50` requirements still pass.
- [ ] `AUTH_L04_Item_PressureKey_Expanded` gates the regulator path if required.
- [ ] `AUTH_L04_PumpReroute_A` or its expanded equivalent visibly changes arena pressure before the Bulwark wave.
- [ ] `AUTH_L04_AssemblyFloorClear` cannot fire before the required arena clear.
- [ ] `AUTH_L04_EmergencyHoist_Expanded` loads Level05 only after route completion.
- [ ] Level04 secrets remain optional.

### Level05

- [ ] `AUTH_L05_ReliefValve_North` and `AUTH_L05_ReliefValve_South` are readable before Warden pressure combines.
- [ ] `AUTH_L05_WardenCoreBreach` unlocks `GEO_L05_GovernorFinale_CoreBreachGate` or its gate authority.
- [ ] `AUTH_L05_FinalExitInteract_Expanded` fires `TRG_L05_CampaignComplete_Expanded`.
- [ ] `UI_L05_CampaignCompletePanel_Expanded` appears only after campaign complete.
- [ ] Final exit does not require secret discovery.

## Encounter Budget Checks

- [ ] Level01 peak active enemies never exceeds `6`.
- [ ] Level02 peak active enemies never exceeds `8`.
- [ ] Level03 peak active enemies never exceeds `9`.
- [ ] Level04 peak active enemies never exceeds `10`.
- [ ] Level05 peak active enemies never exceeds `11`, including Warden/support pressure if measured together.
- [ ] Narrow catwalk/service corridors never spawn enough enemies to block movement.
- [ ] Lancer sightlines in Level02 and Level04 include cover breaks every `8m` to `10m`.
- [ ] Bulwark and Warden pressure starts only after the relevant room/readability cues are visible.
- [ ] Every major encounter has at least one ammo or health pickup placed before the spike.

## Required Runtime Smoke Additions

### RuntimeMajorRouteEncounterTest

- [ ] Loads each Level01-Level05 scene.
- [ ] Finds each `EXPAN_..._v0_1_55` root.
- [ ] Finds required child containers.
- [ ] Finds level labels listed in scene-object expectations.
- [ ] Emits `V0_MAJOR_ROUTE_ENCOUNTER_PASS`.

### RuntimeAutoPlaythroughTest Extensions

- [ ] Completes Level01 expanded key/gate/lift route.
- [ ] Completes Level02 routing deck and Boilerheart lift lock.
- [ ] Completes Level03 Scattergun confirmation, Boilerheart valve, and Foundry lift.
- [ ] Completes Level04 pressure key, pump reroute, arena clear, and emergency hoist.
- [ ] Completes Level05 relief valves, Warden/core breach, final exit, and campaign complete.
- [ ] Existing route transition and persistence checks still pass.

### RuntimeEncounterBudgetTest

- [ ] Spawns or simulates each named wave in deterministic order.
- [ ] Confirms active caps by level.
- [ ] Confirms wave roots use `SPAWN_L##_Wave_BeatName`.
- [ ] Emits `V0_ENCOUNTER_BUDGET_PASS`.

### RuntimeSecretTest Extensions

- [ ] By v0.1.56, confirms at least one optional secret per level.
- [ ] Confirms secrets do not block required campaign completion.
- [ ] Confirms campaign secret totals persist to final win.
- [ ] Existing `V0_SECRET_PASS` remains valid.

### RuntimeHazardTest Extensions

- [ ] Confirms new pressure/furnace hazard tells exist before damage state.
- [ ] Confirms relief valves and pump reroutes visibly change tell or hazard state where specified.
- [ ] Existing `V0_HAZARD_PASS` remains valid.

### RuntimeGovernorFinaleFlowTest

- [ ] Confirms Level05 relief valves are findable.
- [ ] Confirms Warden/core breach unlock path is findable.
- [ ] Confirms final exit starts unavailable and becomes available after breach.
- [ ] Confirms campaign complete panel appears after final exit.
- [ ] Emits a finale-specific marker or extends existing Warden/final-flow marker.

## Human Playtest Checklist

- [ ] Level01 feels like a real opening level rather than a single key tutorial.
- [ ] Level02 pressure district reads as one connected Pipeworks space, not three disconnected branches.
- [ ] Level03 has a clear weapon/practice/Boilerheart/gantry/lift escalation.
- [ ] Level04 feels like the largest pre-finale combat level, with the Bulwark arena as the peak.
- [ ] Level05 gives the player a clear finale read before the Warden/core pressure starts.
- [ ] No mandatory route requires jumping, crouching, or physics exploits.
- [ ] No sidecar dressing creates snag points or hides required interactables.
- [ ] Route labels and amber/green objective language remain readable against Set08/Set09 visual density.

## Screenshot Review Set

- [ ] Level01: gate preview, pump gallery, service lift sendoff.
- [ ] Level02: valve deck, north Lancer hall, Boilerheart lift lock.
- [ ] Level03: Scattergun bay, Boilerheart core, upper gantry/high rejoin.
- [ ] Level04: assembly floor, pump reroute, vertical pump arena, emergency hoist.
- [ ] Level05: pressure ring, relief valves, Warden core, core breach gate, final exit chamber.
- [ ] Screenshots are saved outside runtime game asset roots.

## Automated Exit Gate

- [ ] Run level validation and confirm:
  - [ ] `V0_LEVEL_VALIDATION_PASS`
  - [ ] `V0_MAJOR_ROUTE_ENCOUNTER_PASS`
  - [ ] `V0_V0155_SIDECAR_IMPORT_PASS`
  - [ ] `V0_VISUALONLY_AUTHORITY_PASS`
  - [ ] `V0_FIRST_VERSION_OBJECTIVE_CHAIN_PASS`
  - [ ] `V0_ENCOUNTER_BUDGET_PASS`
  - [ ] `V0_CAMPAIGN_SCALE_ROUTE_ORDER_PASS`
- [ ] Run route audit and confirm `V0_ROUTE_AUDIT_PASS`.
- [ ] Run full runtime smoke and confirm existing markers remain valid:
  - [ ] `V0_RUNTIME_SMOKE_PASS`
  - [ ] `V0_AUTO_PLAYTHROUGH_PASS`
  - [ ] `V0_MIDGAME_FLOW_PASS`
  - [ ] `V0_CLIMAX_FLOW_PASS`
  - [ ] `V0_HAZARD_PASS`
  - [ ] `V0_SECRET_PASS`
  - [ ] `V0_RANGED_COMBAT_PASS`
  - [ ] `V0_BULWARK_COMBAT_PASS`
  - [ ] `V0_WORLD_LABEL_READABILITY_PASS`
- [ ] Run full Windows matrix and confirm `V0_BUILD_MATRIX_PASS`.
- [ ] Generate package, route QA packet, issue triage, candidate readiness, and release notes only after P0 automated gates pass.
- [ ] For v0.1.56, confirm `V0_FIRST_VERSION_STABILIZATION_PASS` before packaging.

## Deferred Or Risk Notes

- [ ] If all five expansions are too large for v0.1.55, keep roots and objective chain for all five levels, then cut visual density first.
- [ ] If Set09 or ClockworkEnemyPartsSet09 import is unstable, ship v0.1.55 with Set07/Set06 P0 packages and record Set09 as v0.1.56 import follow-up.
- [ ] If Warden phase scripting is not ready, keep Level05 relief valves, tell objects, and core-breach hooks deterministic while using existing Warden defeat as the gameplay authority.
- [ ] If performance drops, reduce decorative sidecar density before removing objective labels, safe pockets, or encounter cover.
- [ ] Do not add unrelated platform, save-system, or new-weapon scope to this batch.
