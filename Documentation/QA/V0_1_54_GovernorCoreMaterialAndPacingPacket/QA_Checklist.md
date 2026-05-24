# v0.1.54 Governor Core Material and Pacing QA Checklist

Created: 2026-05-24 19:10:23 -04:00

Use this checklist when the main lane implements the Governor Core material and pacing packet. This docs lane did not edit code, scenes, manifests, shared status docs, build docs, or version docs.

## Entry Gate

- [ ] v0.1.53 route-material and route-polish work is reviewed enough that Set08 material bindings can be applied safely to Level05.
- [ ] v0.1.54 is implemented as one coherent finale batch, not as isolated single-object slices.
- [ ] New Level05 objects live under deterministic v0.1.54 roots:
  - `ROUTE_L05_GovernorCore_v0_1_54`
  - `VISUALONLY_L05_GovernorCore_v0_1_54`
  - `AUTH_L05_GovernorCore_v0_1_54`
  - `SPAWN_L05_GovernorCore_v0_1_54`
  - `TRG_L05_GovernorCore_v0_1_54`
- [ ] Existing Level05 completion, transition, and combat objects remain present unless deliberately migrated with validator coverage.
- [ ] Review-only images are saved under documentation render folders, not runtime game asset roots.

## Required Set08 Material Checks

- [ ] Add or verify Level05-specific project materials rather than replacing broad shared materials:
  - [ ] `M_L05_Set08_WetBlackStoneSlab`
  - [ ] `M_L05_Set08_ChippedBlackIronWallPanel`
  - [ ] `M_L05_Set08_WornBrassPipe`
  - [ ] `M_L05_Set08_OxidizedCopperCoil`
  - [ ] `M_L05_Set08_RedPressureEnamel`
  - [ ] `M_L05_Set08_GaugeFaceEnamel`
  - [ ] `M_L05_Set08_AmberGaslightGlass`
  - [ ] `M_L05_Set08_ScorchedFurnaceMetal`, if the P1 breach/impact pass is included.
- [ ] Material validation confirms texture paths point to the accepted Set08 package.
- [ ] Red pressure enamel is used only for danger, lockdown, or active pressure state.
- [ ] Amber glass is used for objective, core pulse, and final exit confirmation language.
- [ ] Safe pockets remain readable against wet black stone and surrounding brass/iron detail.
- [ ] If glass transparency or RMA texture mapping is incomplete, use a documented fallback without blocking the P0 finale pass.

## Required Validator Additions

- [ ] Add a dedicated validator helper, recommended name: `ValidateV0154GovernorCoreMaterialAndPacing`.
- [ ] Add a Level05 Set08 material-binding helper, recommended name: `ValidateSet08Level05MaterialBindings`.
- [ ] Require the following labels:
  - [ ] `Label - L05 Governor Core Entry`
  - [ ] `Label - L05 Pressure Ring`
  - [ ] `Label - L05 Safe Pocket North`
  - [ ] `Label - L05 Safe Pocket South`
  - [ ] `Label - L05 Manual Relief Valve`
  - [ ] `Label - L05 Core Breach`
  - [ ] `Label - L05 Final Exit`
- [ ] Require the following arena readability objects:
  - [ ] `L05 Governor Core Brass Sightline Ring`
  - [ ] `L05 Governor Core Wet Stone Safe Pocket North`
  - [ ] `L05 Governor Core Wet Stone Safe Pocket South`
  - [ ] `L05 Governor Core Red Pressure Hazard Ring`
  - [ ] `L05 Governor Core Amber Exit Confirmation Arch`
  - [ ] `L05 Governor Core Gauge Wall A`
  - [ ] `L05 Governor Core Gauge Wall B`
- [ ] Require Warden pressure tell objects:
  - [ ] `AUTH_L05_WardenPressureTell_NorthSteam`
  - [ ] `AUTH_L05_WardenPressureTell_SouthSteam`
  - [ ] `AUTH_L05_WardenPressureTell_CorePulse`
  - [ ] `AUTH_L05_WardenPressureTell_RingSweep`
  - [ ] `AUTH_L05_WardenPressureTell_ExitUnlock`
- [ ] Require final flow objects:
  - [ ] `TRG_L05_CoreBreachComplete`
  - [ ] `AUTH_L05_FinalExitDoor`
  - [ ] `AUTH_L05_FinalExitInteract`
  - [ ] `L05 Final Exit Amber Confirmation`
  - [ ] `TRG_L05_CampaignComplete`
  - [ ] `UI_L05_CampaignCompletePanel`
  - [ ] `UI_L05_RestartRunButton`
  - [ ] `UI_L05_QuitToDesktopButton`
- [ ] Validator checks order/position, not just existence:
  - [ ] Entry label appears before pressure ring.
  - [ ] Safe pocket labels are separated and not stacked.
  - [ ] Warden tell objects appear before corresponding active hazards or state changes.
  - [ ] Final exit confirmation appears after core breach completion.

## Required Runtime Smoke Additions

### Governor Core Flow

- [ ] Add or extend a Level05 runtime smoke that emits `V0_L05_GOVERNOR_CORE_PASS`.
- [ ] Confirm Level05 loads and finds the v0.1.54 route root.
- [ ] Confirm entry label, pressure ring, safe pockets, core breach label, and final exit label exist.
- [ ] Confirm north and south safe pockets are spatially separated.
- [ ] Confirm pressure ring and final exit sightline are visible before completion.

### Warden Pressure Tells

- [ ] Add or extend a runtime smoke that emits `V0_WARDEN_PRESSURE_TELL_PASS`.
- [ ] Confirm `AUTH_L05_WardenPressureTell_NorthSteam` exists before north pressure damage.
- [ ] Confirm `AUTH_L05_WardenPressureTell_SouthSteam` exists before south pressure damage.
- [ ] Confirm `AUTH_L05_WardenPressureTell_CorePulse` exists before core breach state.
- [ ] Confirm `AUTH_L05_WardenPressureTell_RingSweep` exists before ring sweep damage.
- [ ] Confirm `AUTH_L05_WardenPressureTell_ExitUnlock` exists after core breach complete.

### Final Exit Flow

- [ ] Add or extend a runtime smoke that emits `V0_FINAL_EXIT_FLOW_PASS`.
- [ ] Confirm final exit door starts locked.
- [ ] Confirm `TRG_L05_CoreBreachComplete` unlocks the final exit.
- [ ] Confirm final exit interaction can fire `TRG_L05_CampaignComplete`.
- [ ] Confirm campaign complete panel appears after completion trigger.
- [ ] Confirm restart button resets run state and returns to campaign start or Level01.
- [ ] Confirm quit button uses the existing quit-to-desktop flow.

## Existing Smoke Expectations

- [ ] `V0_LEVEL_VALIDATION_PASS` still passes.
- [ ] `V0_SET08_MATERIAL_BINDING_PASS` passes after Level05 material additions.
- [ ] `V0_CLIMAX_FLOW_PASS` still passes after Level05 pacing changes.
- [ ] `V0_AUTO_PLAYTHROUGH_PASS` reaches or simulates the Level05 final exit flow.
- [ ] `V0_HAZARD_PASS` still passes after Warden pressure tell changes.
- [ ] `V0_BOSS_COMBAT_PASS` passes if the marker exists or is added in this batch.
- [ ] `V0_WORLD_LABEL_READABILITY_PASS` passes with the new Level05 labels.
- [ ] `V0_BUILD_MATRIX_PASS` passes before packaging v0.1.54.

## Human Finale QA

- [ ] From Level05 entry, the Governor Core reads as the finale room before combat starts.
- [ ] The pressure ring is visible and not confused with safe pockets.
- [ ] North and south safe pockets are visible from normal player height.
- [ ] The first Warden pressure tell is visible before the player can be damaged by it.
- [ ] Core pulse tell uses amber glass/gauge language, not red hazard language.
- [ ] Ring sweep danger uses red pressure enamel and is readable against the floor.
- [ ] The final exit is visible before unlock but clearly unavailable.
- [ ] After core breach, the final exit unlock state is unmistakable.
- [ ] Campaign complete panel appears only after the final exit trigger.
- [ ] Restart and quit controls are visible, readable, and functional.

## Automated Exit Gate

- [ ] Run `V0LevelValidator.RunValidation` and confirm:
  - [ ] `V0_LEVEL_VALIDATION_PASS`
  - [ ] `V0_SET08_MATERIAL_BINDING_PASS`
  - [ ] `V0_L05_GOVERNOR_CORE_PASS`
- [ ] Run Level05/finale runtime smoke and confirm:
  - [ ] `V0_WARDEN_PRESSURE_TELL_PASS`
  - [ ] `V0_FINAL_EXIT_FLOW_PASS`
  - [ ] `V0_CLIMAX_FLOW_PASS`
  - [ ] `V0_AUTO_PLAYTHROUGH_PASS`
  - [ ] `V0_HAZARD_PASS`
  - [ ] `V0_BOSS_COMBAT_PASS`, if available.
- [ ] Run full build matrix and confirm `V0_BUILD_MATRIX_PASS`.
- [ ] Capture the P1 human review images into documentation-only render folders.
- [ ] Package v0.1.54 only after all P0 automated gates pass.

## Deferred or Risk Notes

- [ ] If Warden AI is not ready, document v0.1.54 as a pressure-arena finale readability pass with boss hooks, not complete final boss AI.
- [ ] If Set08 amber glass transparency is unstable, use an opaque amber fallback and track shader polish separately.
- [ ] If restart flow exposes broader state reset bugs, keep deterministic Level05 hooks and file the larger reset issue separately.
- [ ] If performance dips on mid/low Windows hardware, reduce decorative dressing before reducing core tells, safe pockets, or labels.
- [ ] Do not add unrelated mechanics, secret-count changes, or new platform work in this batch.
