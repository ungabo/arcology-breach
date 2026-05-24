# Scene Object Expectations V0.1.50

Use these names during later main-lane implementation so validation can detect missing route pieces without interpreting scene art.

## Required Route Roots

| Level | Required Root | Child Containers |
| --- | --- | --- |
| Level02 | `ROUTE_L02_PressureBypass_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L02_PressureBypass` |
| Level03 | `ROUTE_L03_FoundryGantry_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L03_FoundryGantry` |
| Level04 | `ROUTE_L04_ObservatoryPumpworks_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L04_ObservatoryPumpworks` |

## Level02 Expected Objects

| Category | Object Name | Required |
| --- | --- | --- |
| Geometry | `GEO_L02_PressureDoor_EntryVestibule` | Yes |
| Geometry | `GEO_L02_PressureBypass_WestPipeCorridor` | Yes |
| Geometry | `GEO_L02_PressureBypass_ValveRoomA` | Yes |
| Geometry | `GEO_L02_PressureBypass_CrossPipeHall` | Yes |
| Geometry | `GEO_L02_PressureBypass_PumpRoom` | Yes |
| Geometry | `GEO_L02_PressureBypass_ReturnDrop` | Yes |
| Geometry | `GEO_L02_PressureBypass_ExitSpine` | Yes |
| Geometry | `GEO_L02_PressureBypass_SecretServiceDuct` | Optional secret |
| Geometry | `GEO_L02_PressureBypass_SecretBoilerNiche` | Optional secret |
| Authority | `AUTH_L02_Valve_BypassA` | Yes |
| Authority | `AUTH_L02_Valve_BypassB` | Yes |
| Authority | `AUTH_L02_Door_PumpRoomExit` | Yes |
| Authority | `AUTH_L02_Shortcut_PipeGate` | Yes |
| Trigger | `TRG_L02_Secret_BoilerNiche` | Optional secret |
| Hazard | `AUTH_L02_Hazard_SteamJet_C1_A` | Yes |
| Hazard | `AUTH_L02_Hazard_PumpVent_R3_A` | Yes |

## Level03 Expected Objects

| Category | Object Name | Required |
| --- | --- | --- |
| Geometry | `GEO_L03_FoundryGantry_EntryRun` | Yes |
| Geometry | `GEO_L03_FoundryGantry_FurnacePit` | Yes |
| Geometry | `GEO_L03_FoundryGantry_CentralLiftBase` | Yes |
| Geometry | `GEO_L03_FoundryGantry_EastServiceRamp` | Yes |
| Geometry | `GEO_L03_FoundryGantry_ControlMezzanine` | Yes |
| Geometry | `GEO_L03_FoundryGantry_WestCoolantDuct` | Yes |
| Geometry | `GEO_L03_FoundryGantry_CoolantPumpRoom` | Yes |
| Geometry | `GEO_L03_FoundryGantry_UpperCatwalk` | Yes |
| Geometry | `GEO_L03_FoundryGantry_RejoinStair` | Yes |
| Geometry | `GEO_L03_FoundryGantry_HighRejoinBalcony` | Yes |
| Authority | `AUTH_L03_CoolantValve_A` | Yes |
| Authority | `AUTH_L03_LiftOverride_B` | Yes |
| Authority | `AUTH_L03_Lift_CentralGantry` | Yes |
| Trigger | `TRG_L03_Secret_CrucibleShelf` | Optional secret |
| Hazard | `AUTH_L03_Hazard_FurnaceStrip_West` | Yes |
| Hazard | `AUTH_L03_Hazard_FurnaceStrip_East` | Yes |
| Hazard | `AUTH_L03_Hazard_SlagVent_R3_A` | Yes |

## Level04 Expected Objects

| Category | Object Name | Required |
| --- | --- | --- |
| Geometry | `GEO_L04_Pumpworks_EntryConduit` | Yes |
| Geometry | `GEO_L04_Pumpworks_KeyedAntechamber` | Yes |
| Geometry | `GEO_L04_Pumpworks_LowerSpine` | Yes |
| Geometry | `GEO_L04_Pumpworks_PressureKeyRoom` | Yes |
| Geometry | `GEO_L04_Pumpworks_RegulatorRoom` | Yes |
| Geometry | `GEO_L04_Pumpworks_EastRiser` | Yes |
| Geometry | `GEO_L04_Pumpworks_VerticalPumpArena` | Yes |
| Geometry | `GEO_L04_Pumpworks_SecretReturnDuct` | Optional secret |
| Geometry | `GEO_L04_Pumpworks_ObservatoryOverlook` | Yes |
| Geometry | `GEO_L04_Pumpworks_MaintenanceStair` | Yes |
| Geometry | `GEO_L04_Pumpworks_RejoinLockroom` | Yes |
| Authority | `AUTH_L04_Item_PressureKey` | Yes |
| Authority | `AUTH_L04_PumpReroute_A` | Yes |
| Authority | `AUTH_L04_OverlookSwitch_B` | Optional secret |
| Trigger | `TRG_L04_Secret_ReturnDuct` | Optional secret |
| Hazard | `AUTH_L04_Hazard_PressureJet_R4_North` | Yes |
| Hazard | `AUTH_L04_Hazard_PressureJet_R4_South` | Yes |
| Hazard | `AUTH_L04_Hazard_GearSweep_R4_Deck` | Yes |
| Hazard | `AUTH_L04_Hazard_Overpressure_R3` | Yes |

## Spawn Marker Naming Requirements

| Marker Type | Pattern | Example |
| --- | --- | --- |
| Enemy | `SPAWN_L##_RoomOrBeat_Role_Index` | `SPAWN_L03_R1_Ranged_Teacher` |
| Pickup | `SPAWN_L##_Pickup_Type_A` | `SPAWN_L04_Pickup_Ammo_R4_A` |
| Secret pickup | `SPAWN_L##_Secret_Type_A` | `SPAWN_L02_Secret_AmmoCache_R4_A` |
| Checkpoint | `TRG_L##_Checkpoint_Module_Beat` | `TRG_L04_Checkpoint_Pumpworks_ArenaEntry` |

## Validator-Friendly Rules

- Every object listed as required must exist under its route root in the later implementation scene.
- Optional secret objects may be omitted only if the complete secret branch is intentionally cut; do not leave partial geometry without trigger/reward.
- `VISUALONLY_` containers may be empty during blockout validation, but must exist before visual placement begins.
- No object under `VISUALONLY_` may include colliders, rigidbodies, scripts, audio sources, cameras, lights, or particles.
- `AUTH_` and `TRG_` objects should be siblings of visual instances, not children of them.
