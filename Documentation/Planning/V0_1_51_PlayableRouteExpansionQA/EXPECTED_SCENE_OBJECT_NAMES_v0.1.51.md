# V0.1.51 Expected Scene Object Names

V0.1.51 QA expects the accepted V0.1.50 names to exist in the main-lane implementation. Optional secret objects may be absent only when the entire secret branch is intentionally cut.

## Required Route Roots

| Level | Route Root | Required Child Containers |
| --- | --- | --- |
| Level02 | `ROUTE_L02_PressureBypass_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L02_PressureBypass` |
| Level03 | `ROUTE_L03_FoundryGantry_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L03_FoundryGantry` |
| Level04 | `ROUTE_L04_ObservatoryPumpworks_v0_1_50` | `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_L04_ObservatoryPumpworks` |

## Level02 Pressure Bypass

| Category | Required Names |
| --- | --- |
| Geometry | `GEO_L02_PressureDoor_EntryVestibule`, `GEO_L02_PressureBypass_WestPipeCorridor`, `GEO_L02_PressureBypass_ValveRoomA`, `GEO_L02_PressureBypass_CrossPipeHall`, `GEO_L02_PressureBypass_PumpRoom`, `GEO_L02_PressureBypass_ReturnDrop`, `GEO_L02_PressureBypass_ExitSpine` |
| Optional secret geometry | `GEO_L02_PressureBypass_SecretServiceDuct`, `GEO_L02_PressureBypass_SecretBoilerNiche` |
| Doors and gates | `AUTH_L02_Door_PressureBypassEntry`, `AUTH_L02_Door_PumpRoomExit`, `AUTH_L02_Grate_SecretServiceDuct`, `AUTH_L02_Shortcut_PipeGate` |
| Objectives and triggers | `AUTH_L02_Valve_BypassA`, `AUTH_L02_Valve_BypassB`, `TRG_L02_Secret_BoilerNiche` |
| Hazards | `AUTH_L02_Hazard_SteamJet_C1_A`, `AUTH_L02_Hazard_PumpVent_R3_A` |
| Pickups | `SPAWN_L02_Pickup_Health_R2_A`, `SPAWN_L02_Pickup_Ammo_R3_A`, `SPAWN_L02_Pickup_ArmorShard_R3_A`, `SPAWN_L02_Secret_AmmoCache_R4_A`, `SPAWN_L02_Secret_Health_R4_A` |
| Encounter markers | `SPAWN_L02_R1_Scout_A`, `SPAWN_L02_R1_Scout_B`, `SPAWN_L02_C1_Ranged_A`, `SPAWN_L02_C1_Ranged_B`, `SPAWN_L02_Exit_Guard_A` |

## Level03 Foundry Gantry

| Category | Required Names |
| --- | --- |
| Geometry | `GEO_L03_FoundryGantry_EntryRun`, `GEO_L03_FoundryGantry_FurnacePit`, `GEO_L03_FoundryGantry_CentralLiftBase`, `GEO_L03_FoundryGantry_EastServiceRamp`, `GEO_L03_FoundryGantry_ControlMezzanine`, `GEO_L03_FoundryGantry_WestCoolantDuct`, `GEO_L03_FoundryGantry_CoolantPumpRoom`, `GEO_L03_FoundryGantry_UpperCatwalk`, `GEO_L03_FoundryGantry_RejoinStair`, `GEO_L03_FoundryGantry_HighRejoinBalcony` |
| Doors, gates, lift | `AUTH_L03_Gate_FurnaceEntry`, `AUTH_L03_Lift_CentralGantry`, `AUTH_L03_Gate_ControlMezzanine`, `AUTH_L03_Gate_HighRejoin` |
| Objectives and triggers | `AUTH_L03_CoolantValve_A`, `AUTH_L03_LiftOverride_B`, `TRG_L03_Secret_CrucibleShelf` |
| Hazards | `AUTH_L03_Hazard_FurnaceStrip_West`, `AUTH_L03_Hazard_FurnaceStrip_East`, `AUTH_L03_Hazard_SlagVent_R3_A` |
| Pickups | `SPAWN_L03_Pickup_Ammo_R1_A`, `SPAWN_L03_Pickup_Health_R1_A`, `SPAWN_L03_Pickup_Armor_R4_A`, `SPAWN_L03_Pickup_Ammo_C5_A`, `SPAWN_L03_Secret_CrucibleCache_A` |
| Encounter markers | `SPAWN_L03_R1_Ranged_Teacher` plus beat-group markers for R1 pinch, C4 detour, R4 valve commit, C5 catwalk, and R5 rejoin. |

## Level04 Observatory Pumpworks

| Category | Required Names |
| --- | --- |
| Geometry | `GEO_L04_Pumpworks_EntryConduit`, `GEO_L04_Pumpworks_KeyedAntechamber`, `GEO_L04_Pumpworks_LowerSpine`, `GEO_L04_Pumpworks_PressureKeyRoom`, `GEO_L04_Pumpworks_RegulatorRoom`, `GEO_L04_Pumpworks_EastRiser`, `GEO_L04_Pumpworks_VerticalPumpArena`, `GEO_L04_Pumpworks_ObservatoryOverlook`, `GEO_L04_Pumpworks_MaintenanceStair`, `GEO_L04_Pumpworks_RejoinLockroom` |
| Optional secret geometry | `GEO_L04_Pumpworks_SecretReturnDuct` |
| Doors and gates | `AUTH_L04_Door_KeyedMaintenance_A`, `AUTH_L04_Door_RegulatorSeal_B`, `AUTH_L04_Gate_PumpArenaUpper_C`, `AUTH_L04_Door_RejoinLockroom_D`, `AUTH_L04_Grate_SecretReturn_E` |
| Objectives and triggers | `AUTH_L04_Item_PressureKey`, `AUTH_L04_PumpReroute_A`, `AUTH_L04_OverlookSwitch_B`, `TRG_L04_Secret_ReturnDuct` |
| Hazards | `AUTH_L04_Hazard_PressureJet_R4_North`, `AUTH_L04_Hazard_PressureJet_R4_South`, `AUTH_L04_Hazard_GearSweep_R4_Deck`, `AUTH_L04_Hazard_Overpressure_R3` |
| Pickups | `SPAWN_L04_Pickup_Ammo_R1_A`, `SPAWN_L04_Pickup_Health_R2_A`, `SPAWN_L04_Pickup_Armor_R3_A`, `SPAWN_L04_Pickup_Ammo_R4_A`, `SPAWN_L04_Pickup_Health_R4_B`, `SPAWN_L04_Secret_Overcharge_R5_A`, `SPAWN_L04_Secret_Ammo_R5_B` |
| Encounter markers | Beat-group markers for R1 scout, R2 key ambush, R3 pump commit, R4 hazard read, R4 arena pinch, C5/R5 secret, and R6 rejoin. |

## Suggested Validator Checks

| Check Name | Expected Result |
| --- | --- |
| `V0151_ROUTE_ROOTS_EXIST` | Finds all three route roots exactly. |
| `V0151_REQUIRED_CONTAINERS_EXIST` | Finds required child containers under every route root. |
| `V0151_REQUIRED_OBJECTS_EXIST` | Finds all required geometry, authority, trigger, hazard, and pickup names. |
| `V0151_OPTIONAL_SECRET_COMPLETE_OR_ABSENT` | Optional secret branches are either complete or fully absent. |
| `V0151_VISUALONLY_NO_GAMEPLAY_COMPONENTS` | Finds no gameplay components under `VISUALONLY_*`. |
| `V0151_COLLISION_PREFIX_OWNERSHIP` | Finds no player-blocking collision under `VIS_` or `VISUALONLY_`. |
