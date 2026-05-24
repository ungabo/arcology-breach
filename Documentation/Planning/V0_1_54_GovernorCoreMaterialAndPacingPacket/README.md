# v0.1.54 Governor Core Material and Pacing Packet

Created: 2026-05-24 19:10:23 -04:00

Scope: implementation-ready docs packet for one ambitious playable Level05/Governor Core pass after the v0.1.53 Level02-Level04 route material and pacing work.

This packet is intentionally docs-only. It does not edit gameplay scripts, generated scenes, package manifests, shared status docs, build docs, or version docs.

## Batch Intent

Implement v0.1.54 as a large finale-quality batch for Level05 rather than a chain of tiny single-object edits. The main lane should bind accepted Set08 surface materials into the Governor Core arena, improve boss readability and pressure tells, lock down final exit/restart flow, strengthen validation, regenerate Level05, and run the full build matrix only after the whole finale pass is coherent.

The batch should improve:

- First-read identity of the Governor Core as the steampunk finale room.
- Navigation from entry, to Warden pressure phase, to core breach, to final exit.
- Warden pressure tells, safe pockets, and escalation timing.
- Material language for brass, black iron, wet stone, amber glass, red pressure enamel, gauge enamel, scorched metal, and grime.
- Route labels and lighting cues that do not compete with boss combat.
- Automated and human review gates for finale readability.

## Target Level

Primary target scene: `Level05`

Recommended generated root for new v0.1.54 work: `ROUTE_L05_GovernorCore_v0_1_54`

Recommended visual-only root: `VISUALONLY_L05_GovernorCore_v0_1_54`

Recommended authoritative/gameplay root: `AUTH_L05_GovernorCore_v0_1_54`

Recommended spawn/combat root: `SPAWN_L05_GovernorCore_v0_1_54`

Recommended trigger/flow root: `TRG_L05_GovernorCore_v0_1_54`

## Source Evidence To Read Before Implementation

- `Documentation/Planning/V0_1_53_MaterialBindingMap/material_binding_candidates_v0.1.53.json`
- `Documentation/Planning/V0_1_53_RoutePolishImplementationPacket/route_polish_implementation_packet.json`
- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Documentation/GAME_DESIGN_SPEC.md`
- `Documentation/STORY_BIBLE.md`
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Editor/V0LevelValidator.cs`
- `Assets/_Project/Scripts/Utility/RuntimeClimaxFlowTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeBossCombatTest.cs`, if present
- `Tools/RunV0BuildMatrix.ps1`

## P0 Group A - Governor Core Material Identity

Goal: make the finale look like the north-star steampunk direction using accepted Set08 materials without destabilizing earlier level bindings.

Recommended Set08 bindings:

- Wet stone floor: `SMD08_MAT_WetBlackStoneSlab`
- Heavy wall plates: `SMD08_MAT_ChippedBlackIronWallPanel`
- Pipe spine and rail trim: `SMD08_MAT_WornBrassPipe`
- Core coils and exchanger loops: `SMD08_MAT_OxidizedCopperCoil`
- Hazard and lockdown plates: `SMD08_MAT_RedPressureEnamel`
- Gauge faces and pressure dials: `SMD08_MAT_GaugeFaceEnamel`
- Lamps, core windows, and exit confirmation glass: `SMD08_MAT_AmberGaslightGlass`
- Furnace scars and Warden impact plates: `SMD08_MAT_ScorchedFurnaceMetal`
- Oil pooling or wet safety contrast, if available: `SMD08_MAT_BlackOilWetFloor`

Implementation notes:

- Keep Level05-specific materials as new project material assets, for example `M_L05_Set08_WetBlackStoneSlab`, instead of replacing broad shared materials.
- Use Set08 materials on larger readable surfaces first: arena floor, wall frames, core housing, pressure conduits, boss tell fixtures, and final exit arch.
- Use red pressure enamel only for danger, lockdown, and overpressure language. Do not use it as ordinary decoration.
- Use amber glass for safe/readable objective and exit confirmation cues, not active damage.
- If Set08 RMA texture channel support is not fully mapped, bind albedo/normal first and treat roughness/metallic detail as a controlled follow-up.

Validation target: add Level05 material-binding checks that require the key Level05 project materials and confirm their textures point at the Set08 package paths.

## P0 Group B - Arena Readability and Safe Pockets

Goal: the player should understand where to move before the Warden pressure starts to punish them.

Recommended arena landmarks:

- `Label - L05 Governor Core Entry`
- `Label - L05 Pressure Ring`
- `Label - L05 Safe Pocket North`
- `Label - L05 Safe Pocket South`
- `Label - L05 Manual Relief Valve`
- `Label - L05 Core Breach`
- `Label - L05 Final Exit`

Recommended geometry/readability objects:

- `L05 Governor Core Brass Sightline Ring`
- `L05 Governor Core Wet Stone Safe Pocket North`
- `L05 Governor Core Wet Stone Safe Pocket South`
- `L05 Governor Core Red Pressure Hazard Ring`
- `L05 Governor Core Amber Exit Confirmation Arch`
- `L05 Governor Core Gauge Wall A`
- `L05 Governor Core Gauge Wall B`

Implementation notes:

- Put safe pockets outside the Warden's strongest pressure lane and make them visible from the entry.
- Keep safe pocket material contrast cool/wet/dark against warm brass and red hazard language.
- Ensure the final exit sightline is visible before the boss starts, but locked or unlit until the core breach is complete.
- Use gauge walls as readable pressure state indicators rather than background clutter.
- Avoid dense railing or pipe dressing in the player's critical movement lanes.

Validation target: Level05 validation requires the labels, safe pocket objects, and final exit arch. Runtime smoke verifies label order and safe pocket positions are not stacked at the same location.

## P0 Group C - Warden Pressure Tells

Goal: every major Warden pressure action has a visible tell, a fair reaction window, and a named validation hook.

Recommended tell objects:

- `AUTH_L05_WardenPressureTell_NorthSteam`
- `AUTH_L05_WardenPressureTell_SouthSteam`
- `AUTH_L05_WardenPressureTell_CorePulse`
- `AUTH_L05_WardenPressureTell_RingSweep`
- `AUTH_L05_WardenPressureTell_ExitUnlock`

Recommended tell fixtures:

- `L05 Warden North Steam Warning Gauge`
- `L05 Warden South Steam Warning Gauge`
- `L05 Warden Core Pulse Amber Glass`
- `L05 Warden Ring Sweep Red Enamel Plates`
- `L05 Warden Exit Unlock Gauge`

Implementation notes:

- Teach one pressure tell before combining tell types.
- Ring sweep pressure should telegraph on the floor/ring before damage or forced movement happens.
- Core pulse should use amber glass and gauge motion language, not the same red pressure enamel used for damage.
- Exit unlock tell should be visually distinct from combat tells so the player understands the fight state changed.
- Warden pressure should escalate in readable phases: teach, execute, combine, relief, breach.

Validation target: runtime boss/climax smoke requires the named tell objects and checks their sequence by phase marker or position. Hazard validation confirms tells exist before active hazards.

## P0 Group D - Final Exit and Restart Flow

Goal: finishing Level05 must feel complete, testable, and safe to restart.

Required final-flow objects:

- `TRG_L05_CoreBreachComplete`
- `AUTH_L05_FinalExitDoor`
- `AUTH_L05_FinalExitInteract`
- `Label - L05 Final Exit`
- `L05 Final Exit Amber Confirmation`
- `TRG_L05_CampaignComplete`
- `UI_L05_CampaignCompletePanel`
- `UI_L05_RestartRunButton`
- `UI_L05_QuitToDesktopButton`

Implementation notes:

- The exit door remains locked until `TRG_L05_CoreBreachComplete`.
- The campaign complete panel appears only after `TRG_L05_CampaignComplete`.
- Restart flow should return to Level01 or the campaign start scene with player state reset.
- Quit flow should call the existing quit path used by the v0 UI/menu work, not a one-off Level05 implementation.
- Runtime smoke should be able to complete the exit flow without manual input if automation mode is enabled.

Validation target: add a final-flow validator and runtime smoke marker such as `V0_FINAL_EXIT_FLOW_PASS`.

## P0 Group E - Validation Gates

Goal: make this finale pass hard to regress.

Required automated gates:

- `V0_LEVEL_VALIDATION_PASS`
- `V0_SET08_MATERIAL_BINDING_PASS`
- `V0_L05_GOVERNOR_CORE_PASS`
- `V0_WARDEN_PRESSURE_TELL_PASS`
- `V0_FINAL_EXIT_FLOW_PASS`
- `V0_CLIMAX_FLOW_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_HAZARD_PASS`
- `V0_BOSS_COMBAT_PASS`, if the project already has this marker or adds it in the same batch
- `V0_WORLD_LABEL_READABILITY_PASS`
- `V0_BUILD_MATRIX_PASS`

Implementation notes:

- Add a dedicated `ValidateV0154GovernorCoreMaterialAndPacing` helper instead of overloading earlier route helpers.
- Keep validation based on deterministic names and simple spatial/order assertions.
- Any new runtime smoke marker must be emitted only after the test has confirmed material anchors, Warden pressure tells, safe pockets, and final exit flow.
- The build should not be packaged as v0.1.54 until automated gates pass and a small human screenshot set is captured outside build assets.

## P1 Group F - Human Review Render Hooks

Goal: provide reviewable evidence of finale quality without shipping review-only files in the game build.

Recommended screenshots or Unity render captures:

- Level05 entry looking toward the locked Governor Core.
- Full boss arena from player height showing safe pockets and pressure ring.
- North safe pocket during a pressure tell.
- Core pulse tell using amber glass and gauges.
- Ring sweep tell using red enamel hazard language.
- Final exit arch before unlock.
- Final exit arch after unlock.
- Campaign complete panel with restart and quit options.

Save review-only outputs under `Documentation/ConceptRenders/V0_1_54_GovernorCoreMaterialAndPacing/` or another documentation-only render folder, not under runtime game asset roots.

## P1 Group G - Deferred Risks and Follow-Ups

- If the Warden does not yet have a full boss controller, implement this as a readable pressure-arena pass with named hooks for later boss AI.
- If UI restart/quit flow needs deeper menu work, add the buttons and validation hooks now, then list deeper styling polish as a future UI lane.
- If Set08 glass transparency is unstable, use an opaque amber fallback material for v0.1.54 and track true glass as a shader follow-up.
- If Level05 performance drops on mid/low Windows hardware, reduce decorative pipe density before reducing critical tell geometry.
- Do not add new mechanics that are not required for the finale read unless they are already supported by existing runtime tests.
