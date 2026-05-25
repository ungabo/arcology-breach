# V0.1.53 Expected Object and Material Markers

This file gives QA and validator authors a compact marker target list for the v0.1.53 material-route polish batch.

## Required Route Roots

| Level | Route Root | Required Child Containers |
| --- | --- | --- |
| Level02 | `ROUTE_L02_PressureBypass_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L02_PressureBypass` |
| Level03 | `ROUTE_L03_FoundryGantry_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L03_FoundryGantry` |
| Level04 | `ROUTE_L04_ObservatoryPumpworks_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L04_ObservatoryPumpworks` |

## Material IDs to Verify First

| Role | Material IDs |
| --- | --- |
| Wet/dark stone | `SMD08_MAT_WetBlackStoneSlab`, `SMD08_MAT_WetBlackStoneMortar` |
| Black iron panels | `SMD08_MAT_ChippedBlackIronWallPanel`, `SMD08_MAT_ChippedBlackIronServicePlate` |
| Brass/copper pressure hardware | `SMD08_MAT_WornBrassPipe`, `SMD08_MAT_WornBrassValveBody`, `SMD08_MAT_OxidizedCopperCoil`, `SMD08_MAT_OxidizedCopperRunoff` |
| Hazard/readability accents | `SMD08_MAT_RedPressureEnamel`, `SMD08_MAT_ChippedRedEnamelEdge` |
| Glass/gauge language | `SMD08_MAT_AmberGaslightGlass`, `SMD08_MAT_SmokedAmberGaugeGlass`, `SMD08_MAT_GaugeFaceEnamel`, `SMD08_MAT_BakedIvoryGaugeFace` |
| Furnace/oily surfaces | `SMD08_MAT_BlackOilWetFloor`, `SMD08_MAT_OilyBlackStonePuddle`, `SMD08_MAT_ScorchedFurnaceMetal`, `SMD08_MAT_HeatTintedBoilerIron` |
| Trim and borders | `SMD08_MAT_RivetedBrassTrim`, `SMD08_MAT_RivetedBlackIronTrim` |
| Hold unless shader path is approved | `SMD08_MAT_SootDepositOverlay`, `SMD08_MAT_VerticalGrimeStreakOverlay` |
| Low-priority placeholder | `SMD08_MAT_CrackedBlackRubberGasket` |

## Suggested Binding Marker Names

These markers are suggestions for QA labels, temporary scene notes, or validator report keys. They do not require scene edits by this sidecar packet.

| Marker | Expected Evidence |
| --- | --- |
| `QA_L02_MAT_C1_WetStone_Readable` | C1 route remains readable after wet/dark material binding. |
| `QA_L02_MAT_R2_GaugeValve_Readable` | Valve A gauge/pipe language remains visible from combat approach angles. |
| `QA_L02_MAT_R3_Pump_RedPressure_Clear` | Pump room hazard/valve accents use red pressure language without hiding safe lanes. |
| `QA_L03_MAT_R1_Furnace_HazardContrast` | Furnace strips stand out against scorched/wet metal and floor material binding. |
| `QA_L03_MAT_C5_Catwalk_RailContrast` | Upper catwalk rails and edges remain readable after iron/brass trim binding. |
| `QA_L03_MAT_R4_Coolant_StateContrast` | Coolant valve state change remains visible against copper/brass/iron materials. |
| `QA_L04_MAT_R2_KeyPedestal_Readable` | Pressure key pedestal remains readable against dark floor and brass/copper dressing. |
| `QA_L04_MAT_R3_PumpConsole_Readable` | Pump reroute console remains visible and distinguishable from background panels. |
| `QA_L04_MAT_R4_Arena_JetLanes_Clear` | Pressure jet lanes remain readable after oily/wet floor accents. |
| `QA_L04_MAT_R5_Overlook_GlassTrim_Clear` | Observatory glass/gauge/trim polish does not obscure secret or rejoin cues. |

## Object Markers to Spot Check

| Level | Required Marker Focus |
| --- | --- |
| Level02 | `AUTH_L02_Valve_BypassA`, `AUTH_L02_Valve_BypassB`, `AUTH_L02_Door_PumpRoomExit`, `AUTH_L02_Hazard_SteamJet_C1_A`, `AUTH_L02_Hazard_PumpVent_R3_A`, `AUTH_L02_Shortcut_PipeGate` |
| Level03 | `AUTH_L03_CoolantValve_A`, `AUTH_L03_LiftOverride_B`, `AUTH_L03_Lift_CentralGantry`, `AUTH_L03_Hazard_FurnaceStrip_West`, `AUTH_L03_Hazard_FurnaceStrip_East`, `AUTH_L03_Hazard_SlagVent_R3_A` |
| Level04 | `AUTH_L04_Item_PressureKey`, `AUTH_L04_PumpReroute_A`, `AUTH_L04_Door_KeyedMaintenance_A`, `AUTH_L04_Gate_PumpArenaUpper_C`, `AUTH_L04_Door_RejoinLockroom_D`, `AUTH_L04_Hazard_PressureJet_R4_North`, `AUTH_L04_Hazard_PressureJet_R4_South` |

## Validator-Friendly Checks

| Check Name | Expected Result |
| --- | --- |
| `V0153_ROUTE_ROOTS_STABLE` | Finds all required Level02-Level04 route roots and child containers. |
| `V0153_REQUIRED_ROUTE_OBJECTS_PRESENT` | Finds all required `GEO_`, `AUTH_`, `TRG_`, and `SPAWN_` objects from the v0.1.50/v0.1.51 route packet. |
| `V0153_VISUALONLY_NON_BLOCKING` | Finds no colliders, rigidbodies, scripts, cameras, lights, audio sources, or particles under `VISUALONLY_*`. |
| `V0153_SMD08_FINAL_CANDIDATES_BOUND` | Confirms final-candidate SMD08 materials are available and bound on representative Level02-Level04 surfaces. |
| `V0153_SMD08_NO_MISSING_BINDINGS` | Reports no pink/missing material renderers and no missing SMD08 texture references. |
| `V0153_DARK_SURFACE_READABILITY_PASS` | Manual/visual QA confirms dark, wet, oily, and scorched surfaces do not hide gameplay affordances. |
