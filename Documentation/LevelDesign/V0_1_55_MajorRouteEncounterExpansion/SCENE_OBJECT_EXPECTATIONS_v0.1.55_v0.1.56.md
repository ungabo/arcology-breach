# Scene Object Expectations v0.1.55/v0.1.56

Use these names during later main-lane implementation so validation can detect missing expansion pieces without interpreting scene art.

This document is docs-only. It does not create scene objects.

## Required Expansion Roots

| Level | v0.1.55 Required Root | v0.1.56 Polish Root | Required Child Containers |
| --- | --- | --- | --- |
| Level01 | `EXPAN_L01_IntakeFoundations_v0_1_55` | `POLISH_L01_IntakeFoundations_v0_1_56` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L01_IntakeFoundations` |
| Level02 | `EXPAN_L02_PipeworksPressureDistrict_v0_1_55` | `POLISH_L02_PipeworksPressureDistrict_v0_1_56` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L02_PipeworksPressureDistrict` |
| Level03 | `EXPAN_L03_BoilerheartFoundryBridge_v0_1_55` | `POLISH_L03_BoilerheartFoundryBridge_v0_1_56` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L03_BoilerheartBridge` |
| Level04 | `EXPAN_L04_FoundryAssemblyWorks_v0_1_55` | `POLISH_L04_FoundryAssemblyWorks_v0_1_56` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L04_FoundryAssembly` |
| Level05 | `EXPAN_L05_GovernorCoreFinale_v0_1_55` | `POLISH_L05_GovernorCoreFinale_v0_1_56` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L05_GovernorFinale` |

## Existing Roots To Preserve

| Level | Existing Required Root | Preservation Rule |
| --- | --- | --- |
| Level02 | `ROUTE_L02_PressureBypass_v0_1_50` | Keep all v0.1.50/v0.1.51 route-required objects unless deliberately migrated with validator coverage. |
| Level03 | `ROUTE_L03_FoundryGantry_v0_1_50` | Keep all v0.1.50/v0.1.51 gantry objects unless deliberately migrated with validator coverage. |
| Level04 | `ROUTE_L04_ObservatoryPumpworks_v0_1_50` | Keep all v0.1.50/v0.1.51 pumpworks objects unless deliberately migrated with validator coverage. |
| Level05 | `ROUTE_L05_GovernorCore_v0_1_54` | Preserve if v0.1.54 Governor Core material/pacing root lands before this batch. |

## Level01 Expected Objects

| Category | Object Name | Required |
| --- | --- | --- |
| Geometry | `GEO_L01_IntakeFoundations_IntakeThroat` | Yes |
| Geometry | `GEO_L01_IntakeFoundations_RepairBay` | Yes |
| Geometry | `GEO_L01_IntakeFoundations_KeyBranchBend` | Yes |
| Geometry | `GEO_L01_IntakeFoundations_GearKeyAnnex` | Yes |
| Geometry | `GEO_L01_IntakeFoundations_SecretPressureCache` | Optional secret |
| Geometry | `GEO_L01_IntakeFoundations_GatePreviewHall` | Yes |
| Geometry | `GEO_L01_IntakeFoundations_ReturnCatwalk` | Yes |
| Geometry | `GEO_L01_IntakeFoundations_PumpGallery` | Yes |
| Geometry | `GEO_L01_IntakeFoundations_LiftRunway` | Yes |
| Geometry | `GEO_L01_IntakeFoundations_ServiceLiftSendoff` | Yes |
| Authority | `AUTH_L01_Item_GearKey_Expanded` | Yes |
| Authority | `AUTH_L01_PressureGate_Expanded` | Yes |
| Authority | `AUTH_L01_LiftCallBox_Expanded` | Yes |
| Trigger | `TRG_L01_Secret_IntakePressureCache_Expanded` | Optional secret |
| Label | `Label - L01 Gate Preview` | Yes |
| Label | `Label - L01 Pump Gallery` | Yes |
| Label | `Label - L01 Service Lift Sendoff` | Yes |

## Level02 Expected Objects

| Category | Object Name | Required |
| --- | --- | --- |
| Geometry | `GEO_L02_PressureDistrict_PressureDoorRead` | Yes |
| Geometry | `GEO_L02_PressureDistrict_CrosspipeLower` | Yes |
| Geometry | `GEO_L02_PressureDistrict_PumpRoomExpanded` | Yes |
| Geometry | `GEO_L02_PressureDistrict_ValveDeck` | Yes |
| Geometry | `GEO_L02_PressureDistrict_RejoinSpine` | Yes |
| Geometry | `GEO_L02_PressureDistrict_CartridgeCacheSecret` | Optional secret |
| Geometry | `GEO_L02_PressureDistrict_MainRejoinHall` | Yes |
| Geometry | `GEO_L02_PressureDistrict_NorthLancerHall` | Yes |
| Geometry | `GEO_L02_PressureDistrict_SecretServiceReturn` | Optional secret |
| Geometry | `GEO_L02_PressureDistrict_LiftLockhouse` | Yes |
| Geometry | `GEO_L02_PressureDistrict_BoilerheartLift` | Yes |
| Authority | `AUTH_L02_Valve_RoutingDeck_C` | Yes |
| Authority | `AUTH_L02_LiftLock_BoilerheartExpanded` | Yes |
| Trigger | `TRG_L02_Secret_CartridgeCacheExpanded` | Optional secret |
| Label | `Label - L02 Pressure District` | Yes |
| Label | `Label - L02 North Lancer Hall` | Yes |
| Label | `Label - L02 Boilerheart Lift Lock` | Yes |

## Level03 Expected Objects

| Category | Object Name | Required |
| --- | --- | --- |
| Geometry | `GEO_L03_BoilerheartBridge_ArrivalFloor` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_FurnaceRead` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_ScattergunBay` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_PracticeLane` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_BoilerheartCore` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_BellowsPlatform` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_CoolantDuct` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_CoolantPumpRoom` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_UpperGantry` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_ControlWalkway` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_HighRejoinBalcony` | Yes |
| Geometry | `GEO_L03_BoilerheartBridge_FoundryLift` | Yes |
| Authority | `AUTH_L03_Weapon_SteamScattergun_Expanded` | Yes |
| Authority | `AUTH_L03_BoilerheartPressureValve_Expanded` | Yes |
| Authority | `AUTH_L03_FoundryLiftLock_Expanded` | Yes |
| Trigger | `TRG_L03_Secret_CrucibleShelfExpanded` | Optional secret |
| Label | `Label - L03 Scattergun Bay` | Yes |
| Label | `Label - L03 Boilerheart Core` | Yes |
| Label | `Label - L03 Foundry Lift` | Yes |

## Level04 Expected Objects

| Category | Object Name | Required |
| --- | --- | --- |
| Geometry | `GEO_L04_FoundryAssembly_Arrival` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_CoalSecret` | Optional secret |
| Geometry | `GEO_L04_FoundryAssembly_PressureKeyRoom` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_MainAssemblyFloor` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_FurnaceBypass` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_CrusherLane` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_RegulatorRoom` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_VerticalPumpArena` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_ObservatoryOverlook` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_UpperReturn` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_HoistLockroom` | Yes |
| Geometry | `GEO_L04_FoundryAssembly_EmergencyHoist` | Yes |
| Authority | `AUTH_L04_Item_PressureKey_Expanded` | Yes |
| Authority | `AUTH_L04_AssemblyFloorClear` | Yes |
| Authority | `AUTH_L04_EmergencyHoist_Expanded` | Yes |
| Trigger | `TRG_L04_Secret_CoalCacheExpanded` | Optional secret |
| Label | `Label - L04 Assembly Floor` | Yes |
| Label | `Label - L04 Pump Arena` | Yes |
| Label | `Label - L04 Emergency Hoist` | Yes |

## Level05 Expected Objects

| Category | Object Name | Required |
| --- | --- | --- |
| Geometry | `GEO_L05_GovernorFinale_Arrival` | Yes |
| Geometry | `GEO_L05_GovernorFinale_FinaleRead` | Yes |
| Geometry | `GEO_L05_GovernorFinale_SupplyAlcove` | Yes |
| Geometry | `GEO_L05_GovernorFinale_SecretRegulatorNiche` | Optional secret |
| Geometry | `GEO_L05_GovernorFinale_WestService` | Yes |
| Geometry | `GEO_L05_GovernorFinale_EastService` | Yes |
| Geometry | `GEO_L05_GovernorFinale_WardenCore` | Yes |
| Geometry | `GEO_L05_GovernorFinale_PressureRing` | Yes |
| Geometry | `GEO_L05_GovernorFinale_NorthRelief` | Yes |
| Geometry | `GEO_L05_GovernorFinale_SouthRelief` | Yes |
| Geometry | `GEO_L05_GovernorFinale_CoreBreachGate` | Yes |
| Geometry | `GEO_L05_GovernorFinale_FinalExitChamber` | Yes |
| Authority | `AUTH_L05_ReliefValve_North` | Yes |
| Authority | `AUTH_L05_ReliefValve_South` | Yes |
| Authority | `AUTH_L05_WardenCoreBreach` | Yes |
| Authority | `AUTH_L05_FinalExitInteract_Expanded` | Yes |
| Trigger | `TRG_L05_Secret_RegulatorNicheExpanded` | Optional secret |
| Trigger | `TRG_L05_CampaignComplete_Expanded` | Yes |
| UI | `UI_L05_CampaignCompletePanel_Expanded` | Yes |
| Label | `Label - L05 Relief Valve North` | Yes |
| Label | `Label - L05 Relief Valve South` | Yes |
| Label | `Label - L05 Core Breach Gate` | Yes |
| Label | `Label - L05 Final Exit Chamber` | Yes |

## Spawn Marker Naming Requirements

| Marker Type | Pattern | Example |
| --- | --- | --- |
| Enemy wave root | `SPAWN_L##_Wave_BeatName` | `SPAWN_L04_Wave_PumpArena` |
| Enemy marker | `SPAWN_L##_Beat_Role_Index` | `SPAWN_L02_NorthHall_Lancer_A` |
| Pickup | `SPAWN_L##_Pickup_Type_Index` | `SPAWN_L05_Pickup_Health_A` |
| Secret pickup | `SPAWN_L##_Secret_Type_Index` | `SPAWN_L03_Secret_CrucibleCache_A` |
| Checkpoint | `TRG_L##_Checkpoint_Expansion_Beat` | `TRG_L04_Checkpoint_FoundryAssembly_ArenaEntry` |

## Validator-Friendly Rules

- Every required root and child container must exist before visual placement begins.
- Required `GEO_`, `AUTH_`, `TRG_`, `SPAWN_`, label, and UI objects must be findable by exact name.
- Optional secret objects may be omitted only if the complete secret branch is intentionally cut for that level; do not leave partial geometry without trigger/reward.
- `VISUALONLY_` containers may include sidecar instances, mesh renderers, and materials only.
- No object under `VISUALONLY_` may include colliders, rigidbodies, gameplay scripts, audio sources, cameras, lights, particles with damage authority, or transition triggers.
- All physical blocking belongs to `GEO_` and `COL_` objects.
- All gameplay state belongs to `AUTH_` and `TRG_` objects.
- v0.1.56 polish objects must be additive and must not rename v0.1.55 P0 objects.
