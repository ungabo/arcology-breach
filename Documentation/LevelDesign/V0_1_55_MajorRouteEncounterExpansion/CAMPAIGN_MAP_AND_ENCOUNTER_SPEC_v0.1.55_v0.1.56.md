# v0.1.55/v0.1.56 Campaign Map and Encounter Expansion Spec

Created: 2026-05-25

Scope: docs-only next-playable-batch level design plan. This file does not edit scenes, scripts, manifests, build settings, release notes, or shared status docs.

## Expansion Intent

The current game has a polished route through five prototype levels and recent v0.1.51-v0.1.53 route additions for Level02-Level04. The next visible leap should make the run feel like a fuller first version: every level gets a larger identity space, a stronger encounter arc, one optional mastery secret, and a clear objective/rejoin chain without abandoning the existing generated-scene foundation.

Recommended release split:

- `v0.1.55`: build the visible content leap. Import approved sidecar families, add the expansion roots, place route shells/dressing, add encounter waves using existing enemy authority, add scene-owned objective and secret hooks, and pass the new route/encounter smoke.
- `v0.1.56`: stabilize the same larger shape. Tighten objective state, wave pacing, secret totals, final-flow automation, performance density, and screenshot review evidence. Do not start a separate feature direction unless v0.1.55 lands cleanly.

## Shared Scale Rules

- `1 Unity unit = 1 meter`.
- Player height assumption: `1.8m`.
- Main corridors: `3m` to `5m` clear width.
- Side service corridors: `2.4m` minimum clear width unless non-combat secret-only spaces.
- Combat rooms: `12m x 12m` minimum; major arenas target `22m x 24m` or larger.
- No mandatory jump or crouch.
- Ramps and stairs stay VR-friendly later: broad turns, stable horizon, no sudden forced 180-degree pivots.
- Sidecar visuals never own collision, triggers, rigidbodies, audio, gameplay scripts, cameras, or lights unless a later main-lane promotion explicitly creates scene-owned authority beside them.

## Level01: Brassworks Intake Foundations

### Intent

Turn the tutorial intake from a compact key/gate/lift prototype into a first-level loop with a safe landmark, a key branch, a pressure-pump gallery, a low-risk secret, and a stronger lift sendoff.

### Module Summary

| Field | Value |
| --- | --- |
| Expansion root | `EXPAN_L01_IntakeFoundations_v0_1_55` |
| v0.1.56 polish root | `POLISH_L01_IntakeFoundations_v0_1_56` |
| Target footprint | `72m X x 48m Z`, vertical range `Y=0` to `Y=6` |
| Main path duration | 6-8 min |
| Peak active enemies | `6` |
| Completion state | Gear key opens pressure gate; service lift to Level02 remains the exit. |

### Top-Down Layout

```text
Z+
  44                 R6 Service Lift Sendoff
                     |
  36        C5-------R5 Pump Gallery
            |        |
  28   R3 Secret  C4 Return Catwalk
       |            |
  20   R2 Key Annex-C3 Gate Preview
       |            |
  10   C2 Repair Bay/R1 First Combat
       |
   0   ENTRY-C1 Intake Throat
      -24  -12   0   12   24   36 X+
```

### Geometry Schedule

| ID | Object Name | Center XYZ | Scale XYZ | Purpose | Notes |
| --- | --- | ---: | ---: | --- | --- |
| C1 | `GEO_L01_IntakeFoundations_IntakeThroat` | `(0,1.5,0)` | `(18,3,5)` | Safe arrival | Keep original spawn orientation readable. |
| R1 | `GEO_L01_IntakeFoundations_RepairBay` | `(0,2,10)` | `(20,4,14)` | First combat/read room | Two waist-high collision blocks max; no maze clutter. |
| C2 | `GEO_L01_IntakeFoundations_KeyBranchBend` | `(-9,1.5,15)` | `(5,3,12)` | Branch to key annex | Shows gate preview through grating. |
| R2 | `GEO_L01_IntakeFoundations_GearKeyAnnex` | `(-14,2,22)` | `(16,4,12)` | Objective pickup room | Preserve existing gear-key authority if reused. |
| R3 | `GEO_L01_IntakeFoundations_SecretPressureCache` | `(-20,1.5,30)` | `(10,3,8)` | Optional secret | Hidden by readable pressure-cache clue, not random wall search. |
| C3 | `GEO_L01_IntakeFoundations_GatePreviewHall` | `(10,1.5,20)` | `(18,3,4)` | Shows locked pressure gate | Gate must be visible before key pickup. |
| C4 | `GEO_L01_IntakeFoundations_ReturnCatwalk` | `(8,3.5,29)` | `(20,3,4)` | Return shortcut | Floor at `Y=2`; gentle ramp ends. |
| R5 | `GEO_L01_IntakeFoundations_PumpGallery` | `(18,2.5,36)` | `(22,5,16)` | Escalation room | Pump columns are visual; scene-owned `COL_` blocks only where needed. |
| C5 | `GEO_L01_IntakeFoundations_LiftRunway` | `(6,1.5,38)` | `(18,3,5)` | Final lift approach | Green lift language from v0.1.5 remains dominant. |
| R6 | `GEO_L01_IntakeFoundations_ServiceLiftSendoff` | `(6,2,44)` | `(14,4,10)` | Exit room | Connects to existing Level02 transition trigger. |

### Objective and Secret Chain

| ID | Object Name | Location | Interaction | Result |
| --- | --- | --- | --- | --- |
| O1 | `AUTH_L01_Item_GearKey_Expanded` | `(-15,1,23)` | Pickup | Unlocks or confirms existing pressure gate unlock. |
| O2 | `AUTH_L01_PressureGate_Expanded` | `(11,1.5,23)` | Existing gate interaction or automatic key check | Opens path to pump gallery. |
| O3 | `AUTH_L01_LiftCallBox_Expanded` | `(5,1,42)` | Use or existing lift trigger | Loads Level02 after green lift cue. |
| S1 | `TRG_L01_Secret_IntakePressureCache_Expanded` | `(-20,1,30)` | Enter trigger | Adds Level01 first-version secret count. |

### Encounter Beats

| Beat | Trigger | Enemy Composition | Placement | Design Goal |
| --- | --- | --- | --- | --- |
| SafeRead | Player leaves C1 | No enemies | R1 door line | Player reads route and gate preview before combat. |
| FirstContact | Enter R1 | 2 Scrappers | R1 east and west repair bays | Simple movement/shooting tutorial. |
| KeyCost | Pick up O1 | 2 Scrappers, 1 weak ranged if Lancer is too much use a delayed Scrapper | R2 return corners | Key pickup has consequence without trapping player. |
| GateReward | O2 opens | 1 Scrapper | Behind gate at safe distance | Confirms gate opened a live route. |
| PumpGallery | Cross R5 midpoint | 2 Scrappers, 1 Lancer | Floor lanes and pump balcony | First real mixed-pressure moment. |
| LiftSendoff | Enter R6 | 1 Scrapper or no enemy if player health below threshold | R6 side alcove | Light exit confirmation, never a hard spike. |

### Sidecar Dressing Anchors

| Visual Container | Suggested Packages | Anchor | Rule |
| --- | --- | --- | --- |
| `VISUALONLY_L01_IntakeFoundations_Shells` | Room Shell Set 07, Steam Corridor Dressing Set 09 | C1/R1/R5 walls and ceiling | No sidecar colliders; scene-owned `COL_` only. |
| `VISUALONLY_L01_IntakeFoundations_Objectives` | Objective Interactables Set 05 | O1/O2/O3 surrounds | Existing objective scripts remain on `AUTH_` objects. |
| `VISUALONLY_L01_IntakeFoundations_HazardRead` | Hazard Props Set 06, Steam FX Set 06 | Pump gallery pressure warning | VFX is decorative unless paired with existing scene-owned hazard authority. |

### Acceptance Criteria

- Player sees the pressure gate before collecting the gear key.
- The service lift remains readable from the pump gallery and cannot be confused with a side route.
- Level01 has exactly one first-version optional secret unless a later campaign-secret count update says otherwise.
- Peak active enemies do not exceed `6`.

## Level02: Pipeworks Pressure District

### Intent

Fold the v0.1.51/v0.1.53 pressure bypass into a larger Pipeworks district with longer sightlines, a valve chain, Lancer angles, and a stronger service-lift transition to Boilerheart.

### Module Summary

| Field | Value |
| --- | --- |
| Expansion root | `EXPAN_L02_PipeworksPressureDistrict_v0_1_55` |
| Existing route root to preserve | `ROUTE_L02_PressureBypass_v0_1_50` |
| v0.1.56 polish root | `POLISH_L02_PipeworksPressureDistrict_v0_1_56` |
| Target footprint | `88m X x 56m Z`, vertical range `Y=0` to `Y=7` |
| Main path duration | 8-10 min |
| Peak active enemies | `8` |
| Completion state | Pipeworks routing valve and bypass valves unlock Boilerheart service lift. |

### Top-Down Layout

```text
Z+
  52                     R8 Boilerheart Lift
                         |
  44          R7 Lift Lockhouse
              |
  36     C6--R6 North Lancer Hall
          |   |
  28 R5 Secret| C5 Main Rejoin
       |      |
  20   R3 Pump Room----C4 Exit Spine
       |       \        |
  12   C2 Crosspipe----R4 Valve Deck
       |
   4 ENTRY-R1 Pressure Door Read
      -28 -16  -4   8   20   32   44 X+
```

### Geometry Schedule

| ID | Object Name | Center XYZ | Scale XYZ | Purpose | Notes |
| --- | --- | ---: | ---: | --- | --- |
| R1 | `GEO_L02_PressureDistrict_PressureDoorRead` | `(-8,2,4)` | `(18,4,10)` | Locked-route read | Connects existing Level02 path to expanded district. |
| C2 | `GEO_L02_PressureDistrict_CrosspipeLower` | `(-8,1.5,12)` | `(5,3,18)` | Lower bypass connector | May stitch into existing v0.1.50 C1/C2. |
| R3 | `GEO_L02_PressureDistrict_PumpRoomExpanded` | `(0,2.5,20)` | `(20,5,16)` | Main fight room | Replaces or surrounds current pump-room scale. |
| R4 | `GEO_L02_PressureDistrict_ValveDeck` | `(18,3.5,18)` | `(16,5,14)` | Second valve objective | Floor at `Y=2`; ramp from C2. |
| C4 | `GEO_L02_PressureDistrict_RejoinSpine` | `(22,1.5,28)` | `(5,3,18)` | Mainline rejoin | Keep v0.1.53 rejoin gauge visible. |
| R5 | `GEO_L02_PressureDistrict_CartridgeCacheSecret` | `(-22,1.5,30)` | `(12,3,8)` | Optional secret | Reuses Pipeworks cartridge-cache language. |
| C5 | `GEO_L02_PressureDistrict_MainRejoinHall` | `(10,1.5,34)` | `(24,3,5)` | Joins north hall | Provides combat breath pocket. |
| R6 | `GEO_L02_PressureDistrict_NorthLancerHall` | `(24,2.5,38)` | `(26,5,16)` | Ranged pressure room | Pillars/pipe columns create readable cover. |
| C6 | `GEO_L02_PressureDistrict_SecretServiceReturn` | `(-4,1.5,38)` | `(20,3,4)` | Optional return from secret | One-way latch acceptable. |
| R7 | `GEO_L02_PressureDistrict_LiftLockhouse` | `(24,2,46)` | `(18,4,12)` | Final objective check | Locked lift visible before unlock. |
| R8 | `GEO_L02_PressureDistrict_BoilerheartLift` | `(24,2,54)` | `(14,4,10)` | Level03 transition | Existing transition authority remains scene-owned. |

### Objective and Secret Chain

| ID | Object Name | Location | Interaction | Result |
| --- | --- | --- | --- | --- |
| O1 | `AUTH_L02_Valve_BypassA` | Existing v0.1.50/v0.1.51 route | Existing hold/use | Required if pressure bypass remains in main path. |
| O2 | `AUTH_L02_Valve_BypassB` | Existing v0.1.50/v0.1.51 route | Existing hold/use | Required if pressure bypass remains in main path. |
| O3 | `AUTH_L02_Valve_RoutingDeck_C` | `(19,2.5,19)` | Hold/use for `1.2s` | Unlocks lift lockhouse pressure state. |
| O4 | `AUTH_L02_LiftLock_BoilerheartExpanded` | `(24,1.5,50)` | Use/lift trigger | Loads Level03 after all required pressure state is green. |
| S1 | `TRG_L02_Secret_CartridgeCacheExpanded` | `(-22,1,30)` | Enter trigger | Optional ammo/health route mastery reward. |

### Encounter Beats

| Beat | Trigger | Enemy Composition | Placement | Design Goal |
| --- | --- | --- | --- | --- |
| PressureDoorRead | Enter R1 | 1 Scrapper | R1 far side | Keeps the gate read low pressure. |
| CrosspipePinch | Mid C2 | 2 Scrappers, 1 Lancer | C2 bend and R3 balcony | Teaches cover against ranged fire. |
| ValveCommit | Start O3 | 2 Scrappers, 1 Lancer | R4 lower door and deck flank | Objective exposure under known cover. |
| SecretReward | Enter R5 | No enemies | Secret only | Lets secret feel like mastery. |
| NorthHallFight | Enter R6 | 2 Scrappers, 2 Lancers | Pipe columns and far hall | Longer sightline combat; avoid stacked hits. |
| LiftLockhouse | O4 becomes available | 1 medium guard or Bulwark-lite visual-only dressing on a Scrapper authority | R7 gate face | Confirms route completion. |

### Sidecar Dressing Anchors

| Visual Container | Suggested Packages | Anchor | Rule |
| --- | --- | --- | --- |
| `VISUALONLY_L02_PipeworksPressureDistrict_Shells` | Room Shell Set 07, Steam Corridor Dressing Set 09 | R3/R6/R7 | Use shell pieces for density, not route blockers. |
| `VISUALONLY_L02_PipeworksPressureDistrict_Hazards` | Hazard Props Set 06, Steam FX Set 06 | C2/R4 pressure vents | Scene-owned `AUTH_` hazards remain the only damage authority. |
| `VISUALONLY_L02_PipeworksPressureDistrict_Objectives` | Objective Interactables Set 05 | O3/O4 | Objective props are siblings of `AUTH_`, never parents. |

### Acceptance Criteria

- Existing v0.1.50/v0.1.51 pressure bypass required objects remain present.
- `AUTH_L02_Valve_RoutingDeck_C` cannot be skipped from normal traversal if it gates the lift.
- R6 cover breaks Lancer sightlines every `8m` to `10m`.
- Peak active enemies do not exceed `8`.

## Level03: Boilerheart Foundry Bridge

### Intent

Make Boilerheart feel like the midgame pivot: the player uses the Steam Scattergun, vents the core, chooses or sees the coolant/gantry route, and exits toward Foundry through a high-pressure bridge.

### Module Summary

| Field | Value |
| --- | --- |
| Expansion root | `EXPAN_L03_BoilerheartFoundryBridge_v0_1_55` |
| Existing route root to preserve | `ROUTE_L03_FoundryGantry_v0_1_50` |
| v0.1.56 polish root | `POLISH_L03_BoilerheartFoundryBridge_v0_1_56` |
| Target footprint | `84m X x 66m Z`, vertical range `Y=0` to `Y=10` |
| Main path duration | 9-11 min |
| Peak active enemies | `9` |
| Completion state | Boilerheart valve vents hazards and unlocks Foundry service lift. |

### Top-Down Layout

```text
Z+
  62                    R9 Foundry Lift
                        |
  54          R8 High Rejoin Balcony
              |
  44     C7 Upper Gantry----R7 Control Walkway
          |                  |
  34 R6 Coolant Pump Room----C6 Coolant Duct
          |                  |
  24     R4 Boilerheart Core-R5 Bellows Platform
          |                  |
  14     R3 Scattergun Bay---C3 Practice Lane
          |
   4 ENTRY-R1 Arrival/R2 Furnace Read
      -30 -18  -6   6   18   30   42 X+
```

### Elevation Notes

```text
Y=8.0  R8 high rejoin balcony and lift approach
Y=5.0  C7 upper gantry
Y=2.5  R7 control walkway and parts of R5
Y=0.0  Arrival, scattergun bay, boilerheart floor
```

### Geometry Schedule

| ID | Object Name | Center XYZ | Scale XYZ | Purpose | Notes |
| --- | --- | ---: | ---: | --- | --- |
| R1 | `GEO_L03_BoilerheartBridge_ArrivalFloor` | `(-8,2,4)` | `(18,4,10)` | Arrival read | Shows furnace glow without immediate damage. |
| R2 | `GEO_L03_BoilerheartBridge_FurnaceRead` | `(6,2,6)` | `(16,4,12)` | Hazard preview | Existing steam/furnace tells remain readable. |
| R3 | `GEO_L03_BoilerheartBridge_ScattergunBay` | `(-10,2,16)` | `(18,4,12)` | Weapon room | Existing Steam Scattergun pickup can be reused. |
| C3 | `GEO_L03_BoilerheartBridge_PracticeLane` | `(8,1.5,16)` | `(20,3,5)` | Close-range weapon practice | Scrapper lanes; no Lancer sniping here. |
| R4 | `GEO_L03_BoilerheartBridge_BoilerheartCore` | `(0,3,28)` | `(24,6,18)` | Main objective room | Contains valve and linked hazard state. |
| R5 | `GEO_L03_BoilerheartBridge_BellowsPlatform` | `(20,3.25,28)` | `(16,5,14)` | Support-machine encounter | Leave lateral dodge lanes `>=4m`. |
| C6 | `GEO_L03_BoilerheartBridge_CoolantDuct` | `(18,1.5,40)` | `(5,3,20)` | Optional/required coolant route | If required, label before commitment. |
| R6 | `GEO_L03_BoilerheartBridge_CoolantPumpRoom` | `(-4,2,42)` | `(18,4,14)` | Valve objective | Can stitch into existing FoundryGantry R4. |
| C7 | `GEO_L03_BoilerheartBridge_UpperGantry` | `(6,6.5,50)` | `(30,3,5)` | High traversal | Rail collision required on scene-owned `COL_`. |
| R7 | `GEO_L03_BoilerheartBridge_ControlWalkway` | `(24,4.25,48)` | `(18,3.5,12)` | Control room/combat read | Use height for readable rejoin sightline. |
| R8 | `GEO_L03_BoilerheartBridge_HighRejoinBalcony` | `(10,9.5,58)` | `(18,3,10)` | Exit approach | Looks down toward boilerheart core. |
| R9 | `GEO_L03_BoilerheartBridge_FoundryLift` | `(10,9.5,64)` | `(14,3,8)` | Level04 transition | Existing lift authority remains scene-owned. |

### Objective and Secret Chain

| ID | Object Name | Location | Interaction | Result |
| --- | --- | --- | --- | --- |
| O1 | `AUTH_L03_Weapon_SteamScattergun_Expanded` | `(-12,1,17)` | Pickup or existing acquisition | Confirms scattergun ownership before close-range lane. |
| O2 | `AUTH_L03_BoilerheartPressureValve_Expanded` | `(-2,1,30)` | Hold/use for `1.5s` | Vents linked hazards and unlocks foundry-lift pressure path. |
| O3 | `AUTH_L03_CoolantValve_A` | Existing or R6 | Existing hold/use | Optional if v0.1.50 gantry route remains alternate; required only if signposted. |
| O4 | `AUTH_L03_FoundryLiftLock_Expanded` | `(10,8.5,62)` | Use/lift trigger | Loads Level04 after pressure state is clear. |
| S1 | `TRG_L03_Secret_CrucibleShelfExpanded` | `(0,5.5,50)` | Enter trigger | Optional upper-gantry secret. |

### Encounter Beats

| Beat | Trigger | Enemy Composition | Placement | Design Goal |
| --- | --- | --- | --- | --- |
| ArrivalHeatRead | Enter R2 | 1 Lancer or delayed ranged teacher | Across safe furnace preview | Player reads hazard and ranged line separately. |
| ScattergunPractice | Pick up O1 | 4 Scrappers in two staggered pairs | C3 near/mid lanes | Lets the weapon shine without unfair ranged pressure. |
| ValveCommit | Start O2 | 2 Scrappers, 1 Lancer | R4 side doors and upper rail | Makes valve exposure meaningful. |
| BellowsPressure | Enter R5 | Existing Bellows Node, 2 Scrappers | Around R5 platform | Support-machine lesson; no narrow snagging. |
| CoolantRoute | Enter C6/R6 | 2 Scrappers, 1 Lancer | Duct bends and pump room | Optional route has resistance and reward. |
| UpperGantry | Reach C7 | 2 Lancers, 2 Scrappers | Opposite catwalk and R7 entry | Vertical fight, not a catwalk clog. |
| FoundryLift | O4 unlocked | 1 medium guard | R8/R9 threshold | Final confirmation. |

### Sidecar Dressing Anchors

| Visual Container | Suggested Packages | Anchor | Rule |
| --- | --- | --- | --- |
| `VISUALONLY_L03_BoilerheartBridge_Shells` | Room Shell Set 07, Steam Corridor Dressing Set 09 | R4/R6/C7/R8 | Keep `>=3m` combat width after dressing. |
| `VISUALONLY_L03_BoilerheartBridge_EnemyLookdev` | Clockwork Enemy Parts Set 09, Mechanical Enemy Parts Set 07 | Bellows and upper gantry enemy silhouettes | Visual proxies follow existing enemy authority only. |
| `VISUALONLY_L03_BoilerheartBridge_WeaponLookdev` | Weapon Component Set 07 | Scattergun bay | Do not replace weapon scripts in this batch. |

### Acceptance Criteria

- Scattergun pickup and use are validated before the upper-gantry encounter.
- Boilerheart pressure valve visibly changes hazard state and lift lock state.
- Upper gantry has rail collision and at least one breath pocket before exit.
- Peak active enemies do not exceed `9`.

## Level04: Foundry Assembly Works

### Intent

Turn Foundry into the first large industrial battle level: assembly lanes, furnace hazards, pumpworks routing, Bulwark pressure, a meaningful secret return, and an emergency hoist that feels earned.

### Module Summary

| Field | Value |
| --- | --- |
| Expansion root | `EXPAN_L04_FoundryAssemblyWorks_v0_1_55` |
| Existing route root to preserve | `ROUTE_L04_ObservatoryPumpworks_v0_1_50` |
| v0.1.56 polish root | `POLISH_L04_FoundryAssemblyWorks_v0_1_56` |
| Target footprint | `96m X x 72m Z`, vertical range `Y=0` to `Y=12` |
| Main path duration | 10-12 min |
| Peak active enemies | `10` |
| Completion state | Pump reroute and assembly-floor clear unlock emergency hoist to Governor Core. |

### Top-Down Layout

```text
Z+
  70                         R10 Emergency Hoist
                              |
  60               R9 Hoist Lockroom
                   |
  50        C8----R8 Observatory Overlook
            |      |
  40   R7 Vertical Pump Arena
       |           |
  30   R5 Crusher Lane----R6 Regulator Room
       |           |
  20   R4 Assembly Floor--C4 Furnace Bypass
       |
  10   R2 Coal Secret/R3 Pressure Key Room
       |
   0   ENTRY-R1 Foundry Arrival
      -34 -22 -10   2   14   26   38   50 X+
```

### Elevation Notes

```text
Y=10.0 R8 observatory overlook
Y=6.0  C8 upper return
Y=4.0  R7 upper deck
Y=0.0  arrival, key room, assembly floor, crusher lane
```

### Geometry Schedule

| ID | Object Name | Center XYZ | Scale XYZ | Purpose | Notes |
| --- | --- | ---: | ---: | --- | --- |
| R1 | `GEO_L04_FoundryAssembly_Arrival` | `(-6,2,2)` | `(18,4,10)` | Arrival identity | Show furnace skyline and hoist far-light if possible. |
| R2 | `GEO_L04_FoundryAssembly_CoalSecret` | `(-22,1.5,12)` | `(12,3,8)` | Existing/expanded secret | Optional, no combat required. |
| R3 | `GEO_L04_FoundryAssembly_PressureKeyRoom` | `(0,2,12)` | `(18,4,12)` | Key/objective room | Can reuse v0.1.50 pressure-key logic. |
| R4 | `GEO_L04_FoundryAssembly_MainAssemblyFloor` | `(4,3,24)` | `(34,6,20)` | Main encounter room | Multiple routes around furnace obstruction. |
| C4 | `GEO_L04_FoundryAssembly_FurnaceBypass` | `(26,1.5,24)` | `(5,3,20)` | Hazard bypass | Safe alternative to central heat lane. |
| R5 | `GEO_L04_FoundryAssembly_CrusherLane` | `(-8,2.5,38)` | `(24,5,16)` | Hazard/combat lane | Crusher can be visual-only unless an existing hazard supports it. |
| R6 | `GEO_L04_FoundryAssembly_RegulatorRoom` | `(18,2,38)` | `(18,4,14)` | Pump reroute objective | Contains `AUTH_L04_PumpReroute_A` or expanded equivalent. |
| R7 | `GEO_L04_FoundryAssembly_VerticalPumpArena` | `(2,5,48)` | `(28,10,20)` | Large arena | Two routes between floor and upper deck. |
| R8 | `GEO_L04_FoundryAssembly_ObservatoryOverlook` | `(16,11.5,58)` | `(20,3,12)` | High rejoin/secret view | Shows hoist lockroom. |
| C8 | `GEO_L04_FoundryAssembly_UpperReturn` | `(-4,7.5,58)` | `(24,3,5)` | Upper connector | Rail collision required. |
| R9 | `GEO_L04_FoundryAssembly_HoistLockroom` | `(16,7.5,66)` | `(18,5,10)` | Exit lock state | Must not open before arena clear. |
| R10 | `GEO_L04_FoundryAssembly_EmergencyHoist` | `(16,7.5,72)` | `(14,5,8)` | Level05 transition | Existing transition authority remains scene-owned. |

### Objective and Secret Chain

| ID | Object Name | Location | Interaction | Result |
| --- | --- | --- | --- | --- |
| O1 | `AUTH_L04_Item_PressureKey_Expanded` | `(0,1,13)` | Pickup | Unlocks regulator room or keyed maintenance path. |
| O2 | `AUTH_L04_PumpReroute_A` | Existing v0.1.50 or `(19,1,39)` | Hold/use for `2.0s` | Opens upper pump arena and reduces pressure jets. |
| O3 | `AUTH_L04_AssemblyFloorClear` | R4/R7 fight state | Combat clear trigger | Unlocks hoist lockroom after required arena wave. |
| O4 | `AUTH_L04_EmergencyHoist_Expanded` | `(16,7,70)` | Use/lift trigger | Loads Level05. |
| S1 | `TRG_L04_Secret_CoalCacheExpanded` | `(-22,1,12)` | Enter trigger | Optional Foundry secret reward. |
| S2 | `TRG_L04_Secret_ReturnDuct` | Existing v0.1.50 | Enter trigger | Keep optional; do not require for completion. |

### Encounter Beats

| Beat | Trigger | Enemy Composition | Placement | Design Goal |
| --- | --- | --- | --- | --- |
| ArrivalPressure | Enter R1 | 2 Scrappers | Arrival side lanes | Establish Foundry aggression. |
| KeyRoomCost | Pick up O1 | 2 Scrappers, 1 Lancer | R3 return and R4 preview | Makes key meaningful but escapable. |
| AssemblyFloor | Enter R4 | 3 Scrappers, 2 Lancers | Floor lanes, furnace bypass, high pipe ledge | First wide mixed fight. |
| CrusherRead | Enter R5 | 2 Scrappers | Lane edges | Teach crusher/furnace visual language without a spike. |
| RerouteCommit | Start O2 | 1 Lancer, 2 Scrappers | R6 side doors | Objective exposure under pressure. |
| PumpArena | Cross R7 center | 3 Scrappers, 2 Lancers, 1 Bulwark | Lower floor, upper deck, stair landing | Level climax. |
| HoistRejoin | O3 complete | 1 medium guard or no enemy if health below threshold | R9 threshold | Avoid undercutting the hard arena clear. |

### Sidecar Dressing Anchors

| Visual Container | Suggested Packages | Anchor | Rule |
| --- | --- | --- | --- |
| `VISUALONLY_L04_FoundryAssembly_Shells` | Room Shell Set 07, Steam Corridor Dressing Set 09 | R4/R7/R9 | Density along margins, not combat lanes. |
| `VISUALONLY_L04_FoundryAssembly_Hazards` | Hazard Props Set 06, Steam FX Set 06 | R5/R7 furnace and pressure reads | Real damage remains `AUTH_` only. |
| `VISUALONLY_L04_FoundryAssembly_EliteEnemyDress` | Mechanical Enemy Elite Set 05, Clockwork Enemy Parts Set 09 | Bulwark arena and hoist guard | Visual upgrades do not alter hitboxes in v0.1.55. |

### Acceptance Criteria

- Pump reroute visibly changes the pump arena before the Bulwark wave starts.
- Hoist lockroom cannot open before required arena clear.
- Both Level04 secrets remain optional.
- Peak active enemies do not exceed `10`.

## Level05: Governor Core First-Version Finale

### Intent

Combine the v0.1.54 Governor Core material/pacing direction with a fuller final run: arrival ring, two relief-side objectives, Warden pressure phases, support enemies, core breach, final exit, and campaign complete UI.

### Module Summary

| Field | Value |
| --- | --- |
| Expansion root | `EXPAN_L05_GovernorCoreFinale_v0_1_55` |
| Related v0.1.54 root to preserve if implemented | `ROUTE_L05_GovernorCore_v0_1_54` |
| v0.1.56 polish root | `POLISH_L05_GovernorCoreFinale_v0_1_56` |
| Target footprint | `88m X x 82m Z`, vertical range `Y=0` to `Y=14` |
| Main path duration | 10-13 min |
| Peak active enemies | `11`, including Warden pressure if implemented as one active boss plus support cap |
| Completion state | Warden defeat/core breach unlocks final exit and campaign complete panel. |

### Top-Down Layout

```text
Z+
  82                         R10 Final Exit Chamber
                              |
  70               R9 Core Breach Gate
                   |
  58       R7 North Relief----R8 South Relief
            \       |        /
  44          R6 Governor Pressure Ring
             /   R5 Warden Core   \
  30     C4 West Service       C5 East Service
          |                         |
  18     R3 Supply Alcove      R4 Secret Regulator Niche
          \                         /
   6          ENTRY-R1 Arrival/R2 Finale Read
      -40 -28 -16  -4   8   20   32   44 X+
```

### Elevation Notes

```text
Y=12.0 Final exit chamber upper lightwell
Y=6.0  Relief side platforms
Y=2.0  Pressure ring deck
Y=0.0  Arrival, service routes, ring floor
```

### Geometry Schedule

| ID | Object Name | Center XYZ | Scale XYZ | Purpose | Notes |
| --- | --- | ---: | ---: | --- | --- |
| R1 | `GEO_L05_GovernorFinale_Arrival` | `(0,2,6)` | `(18,4,10)` | Arrival reset | Give ammo/health read before finale. |
| R2 | `GEO_L05_GovernorFinale_FinaleRead` | `(8,3,12)` | `(24,6,14)` | First sight of core | Shows final exit locked above/behind core. |
| R3 | `GEO_L05_GovernorFinale_SupplyAlcove` | `(-18,2,20)` | `(14,4,10)` | Resource pocket | No hidden mandatory objective. |
| R4 | `GEO_L05_GovernorFinale_SecretRegulatorNiche` | `(24,2,20)` | `(12,4,10)` | Optional secret | Open after relief-side clue or core pressure change. |
| C4 | `GEO_L05_GovernorFinale_WestService` | `(-18,1.5,34)` | `(5,3,24)` | Route to north relief | Low-mid pressure route. |
| C5 | `GEO_L05_GovernorFinale_EastService` | `(24,1.5,34)` | `(5,3,24)` | Route to south relief | Mirrors west route enough for readability. |
| R5 | `GEO_L05_GovernorFinale_WardenCore` | `(4,4,44)` | `(24,8,24)` | Boss/core center | Warden authority remains existing controller if available. |
| R6 | `GEO_L05_GovernorFinale_PressureRing` | `(4,3,44)` | `(38,6,32)` | Main arena ring | Safe pockets north/south/east/west are visible. |
| R7 | `GEO_L05_GovernorFinale_NorthRelief` | `(-10,7,58)` | `(18,4,12)` | Relief valve objective | Floor at `Y=6`; route back to ring. |
| R8 | `GEO_L05_GovernorFinale_SouthRelief` | `(20,7,58)` | `(18,4,12)` | Relief valve objective | Floor at `Y=6`; route back to ring. |
| R9 | `GEO_L05_GovernorFinale_CoreBreachGate` | `(4,3,70)` | `(18,6,10)` | Unlock gate | Opens after Warden/core breach complete. |
| R10 | `GEO_L05_GovernorFinale_FinalExitChamber` | `(4,4,82)` | `(20,8,14)` | Campaign complete | Contains final exit and complete UI trigger. |

### Objective and Secret Chain

| ID | Object Name | Location | Interaction | Result |
| --- | --- | --- | --- | --- |
| O1 | `AUTH_L05_ReliefValve_North` | `(-12,6.5,58)` | Hold/use for `1.5s` | Reduces north pressure tell intensity. |
| O2 | `AUTH_L05_ReliefValve_South` | `(22,6.5,58)` | Hold/use for `1.5s` | Reduces south pressure tell intensity. |
| O3 | `AUTH_L05_WardenCoreBreach` | R5/R6 fight state | Boss defeat or pressure-phase clear | Unlocks core breach gate and final exit. |
| O4 | `AUTH_L05_FinalExitInteract_Expanded` | `(4,1,84)` | Use or enter trigger | Fires campaign complete trigger. |
| S1 | `TRG_L05_Secret_RegulatorNicheExpanded` | `(24,1,20)` | Enter trigger | Optional final-level cache. |

### Encounter Beats

| Beat | Trigger | Enemy Composition | Placement | Design Goal |
| --- | --- | --- | --- | --- |
| FinaleRead | Enter R2 | No enemies | Core sightline | The room reads before pressure starts. |
| ServiceSplit | Choose C4 or C5 | 2 Scrappers, 1 Lancer total per side route | Service bends | Makes relief path active but fair. |
| ReliefCommitNorth | Start O1 | 2 Scrappers | R7 side vents | Objective exposure. |
| ReliefCommitSouth | Start O2 | 2 Scrappers | R8 side vents | Mirrors north without feeling duplicated. |
| WardenPhaseTeach | Enter R6 after relief objective | Governor Warden, no more than 2 support Scrappers | R5 center, ring edge | Boss pressure teach. |
| WardenPhaseCombine | Warden below half or both relief valves complete | Warden, 1 Lancer, 1 Bulwark or 2 Scrappers if Bulwark budget is too high | Ring edge and safe-pocket opposite | Finale spike with clear safe pockets. |
| CoreBreach | O3 complete | No new enemies | R9 | Let unlock read breathe. |
| FinalExit | Enter R10/use O4 | No enemies | Final chamber | Campaign complete, no cheap final hit. |

### Sidecar Dressing Anchors

| Visual Container | Suggested Packages | Anchor | Rule |
| --- | --- | --- | --- |
| `VISUALONLY_L05_GovernorFinale_Shells` | Room Shell Set 07, Steam Corridor Dressing Set 09 | R2/R6/R10 | Keep final sightline clear. |
| `VISUALONLY_L05_GovernorFinale_Materials` | Surface Material Detail Set 08 | Ring floor, wall panels, red pressure plates, amber exit | Reuse already imported Set08; no new broad material replacements. |
| `VISUALONLY_L05_GovernorFinale_EnemyDress` | Mechanical Enemy Elite Set 05, Clockwork Enemy Parts Set 09 | Warden/support silhouettes | Existing Warden controller remains gameplay authority. |
| `VISUALONLY_L05_GovernorFinale_HazardTells` | Hazard Props Set 06, Steam FX Set 06 | Pressure tells and relief valves | Tells must exist before damage or state change. |

### Acceptance Criteria

- Final exit is visible before unlock and unmistakably available after core breach.
- North and south safe pockets are separated by at least `18m`.
- Warden phase support never raises peak active enemies above `11`.
- Campaign complete can be reached by automation without manual secret discovery.

## Campaign-Level Acceptance

- Five levels retain their existing transition chain: Level01 -> Level02 -> Level03 -> Level04 -> Level05 -> campaign complete.
- Every new expansion root has child containers for `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and level-specific `VISUALONLY_`.
- No sidecar visual-only object owns gameplay authority.
- At least one optional secret exists in every level by v0.1.56, with the campaign secret target documented in validation before implementation.
- New encounter waves use existing enemy roles unless a separate enemy-implementation lane adds and validates new gameplay.
- Full run duration target after v0.1.56: `43-54 min` for a first-time careful player, `22-30 min` for an automation/known-route pass.
