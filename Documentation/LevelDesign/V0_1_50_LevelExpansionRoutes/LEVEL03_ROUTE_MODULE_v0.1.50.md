# Level03 Route Module: L03_Route_FoundryGantry

## Intent

Expand Level03 with a foundry gantry that wraps around a furnace chamber. The module adds vertical combat, a coolant detour that can disable furnace floor damage, and a high-ground rejoin overlooking the existing route.

## Module Root

| Field | Value |
| --- | --- |
| Root object | `ROUTE_L03_FoundryGantry_v0_1_50` |
| Local origin | Foundry chamber entry threshold. |
| Recommended placement | Branch from the existing Level03 foundry approach before the major arena. |
| Added footprint | `52m X x 42m Z`, vertical range `Y=0` to `Y=9` |
| Main path duration | 5-7 min |
| Completion state | Opens overhead gantry gate and optional coolant-safe floor route. |

## Top-Down Layout

```text
Z+
  34        R5 High Rejoin
            | C6
  26   R4---C5---R3
       |          |
  16   C4   R2   C3
       |    |     |
   6 ENTRY-C1----R1 Furnace Pit
      -20  -8    6    18    30 X+
```

## Elevation Notes

```text
Y=8.0  R5 high rejoin balcony
Y=5.0  C5/C6 upper gantry
Y=2.5  R3 control mezzanine
Y=0.0  ENTRY/C1/R1 furnace floor
Y=-0.5 Furnace hazard volume visual depression only; player collision remains Y=0
```

## Geometry Schedule

| ID | Object Name | Center XYZ | Scale XYZ | Purpose | Notes |
| --- | --- | ---: | ---: | --- | --- |
| C1 | `GEO_L03_FoundryGantry_EntryRun` | `(-8,1.5,6)` | `(20,3,4)` | Entry corridor | Opens into furnace pit. |
| R1 | `GEO_L03_FoundryGantry_FurnacePit` | `(8,2.5,6)` | `(20,5,16)` | Primary hazard combat room | Floor hazard strips on east/west lanes. |
| R2 | `GEO_L03_FoundryGantry_CentralLiftBase` | `(0,2,16)` | `(10,4,10)` | Lift/lower platform hub | Lift is main-lane `AUTH_`, not sidecar. |
| C3 | `GEO_L03_FoundryGantry_EastServiceRamp` | `(14,1.5,16)` | `(4,3,20)` | Ramp from R1 to R3 | Ramp rise from `Y=0` to `Y=2.5`. |
| R3 | `GEO_L03_FoundryGantry_ControlMezzanine` | `(14,4.25,26)` | `(14,3.5,10)` | Coolant control room | Floor at `Y=2.5`. |
| C4 | `GEO_L03_FoundryGantry_WestCoolantDuct` | `(-12,1.5,20)` | `(4,3,20)` | Optional coolant detour | Narrow but combat-capable. |
| R4 | `GEO_L03_FoundryGantry_CoolantPumpRoom` | `(-12,2,30)` | `(14,4,12)` | Objective/pickup room | Contains coolant valve. |
| C5 | `GEO_L03_FoundryGantry_UpperCatwalk` | `(0,6.5,30)` | `(28,3,4)` | Upper bridge | Floor at `Y=5.0`; railing collision required. |
| C6 | `GEO_L03_FoundryGantry_RejoinStair` | `(12,6.5,34)` | `(4,3,12)` | Stair/ramp to high exit | Climbs from `Y=5.0` to `Y=8.0`. |
| R5 | `GEO_L03_FoundryGantry_HighRejoinBalcony` | `(18,9.5,34)` | `(12,3,8)` | Rejoin room | Floor at `Y=8.0`; overlooks existing route. |

## Doors, Lifts, and Exits

| Object Name | Center XYZ | Size XYZ | Initial State | Opens/Moves When |
| --- | ---: | ---: | --- | --- |
| `AUTH_L03_Gate_FurnaceEntry` | `(-2,1.5,6)` | `(3,3,0.5)` | Open | Always. |
| `AUTH_L03_Lift_CentralGantry` | `(0,0.15,16)` | `(5,0.3,5)` | Down | Moves to `Y=5.0` after `AUTH_L03_CoolantValve_A` or combat clear. |
| `AUTH_L03_Gate_ControlMezzanine` | `(12,2.5,22)` | `(3,3,0.5)` | Closed | Opens after R1 furnace pit encounter starts. |
| `AUTH_L03_Gate_HighRejoin` | `(16,8,34)` | `(4,4,0.5)` | Locked | Opens after coolant valve or lift override. |

## Objective Interactions

| ID | Object Name | Location | Interaction | Result |
| --- | --- | --- | --- | --- |
| O1 | `AUTH_L03_CoolantValve_A` | `(-16,1,32)` | Hold/use for `1.5s` | Disables furnace strip damage in R1 and changes hazard visuals to cooled. |
| O2 | `AUTH_L03_LiftOverride_B` | `(14,3,28)` | Hold/use for `1.0s` | Calls central lift if coolant route skipped. |
| O3 | `TRG_L03_Secret_CrucibleShelf` | `(-6,5,30)` | Enter upper catwalk side shelf | Marks secret reward. |

## Encounter Beats

| Beat | Trigger | Enemy Composition | Placement | Design Goal |
| --- | --- | --- | --- | --- |
| HazardRead | Enter C1 | 1 ranged enemy beyond visible furnace strips | `SPAWN_L03_R1_Ranged_Teacher` `(4,0,8)` | Forces player to notice hot floor. |
| Pinch | Enter R1 | 3 melee, 2 ranged | Floor lanes and upper east perch | Vertical pressure around hazard lanes. |
| RouteReward | Take C4 | 2 light enemies | Coolant duct bends | Optional route has resistance and reward. |
| ValveCommit | Start O1 | 1 medium, 2 light | Pump room side doors | Objective exposure under controlled pressure. |
| Pinch | Reach C5 | 2 ranged, 1 flyer/jumper if available | Opposite catwalk and rejoin balcony | High-ground firefight. |
| Rejoin | Open R5 gate | 1 medium guard | Beyond high gate | Short confirmation beat. |

Peak active enemies: `9`.

## Hazards

| Object Name | Location | Gameplay | Disabled By |
| --- | ---: | --- | --- |
| `AUTH_L03_Hazard_FurnaceStrip_West` | `(3,0.05,6)` scale `(3,0.1,12)` | Damage over time when active. | `AUTH_L03_CoolantValve_A` |
| `AUTH_L03_Hazard_FurnaceStrip_East` | `(13,0.05,6)` scale `(3,0.1,12)` | Damage over time when active. | `AUTH_L03_CoolantValve_A` |
| `AUTH_L03_Hazard_SlagVent_R3_A` | `(14,2.55,24)` | Timed vent blocks one mezzanine lane. | Never fully disabled; timing readable. |

## Pickups and Secrets

| Object Name | Location | Type | Notes |
| --- | ---: | --- | --- |
| `SPAWN_L03_Pickup_Ammo_R1_A` | `(0,0.5,4)` | Primary ammo | Before furnace fight. |
| `SPAWN_L03_Pickup_Health_R1_A` | `(12,0.5,12)` | Small health | Risk/reward near hazard. |
| `SPAWN_L03_Pickup_Armor_R4_A` | `(-14,0.5,28)` | Armor shard | Reward for coolant detour. |
| `SPAWN_L03_Pickup_Ammo_C5_A` | `(0,5.5,31)` | Secondary ammo | Supports catwalk fight. |
| `SPAWN_L03_Secret_CrucibleCache_A` | `(-6,5.5,30)` | Secret cache | On shelf off upper catwalk. |

## Visual Dressing Anchors

| Visual Container | Anchor | Rule |
| --- | --- | --- |
| `VIS_L03_FurnaceCore_R1` | Center of R1, below catwalk sightline | Visual core does not own damage; sibling `AUTH_` hazards do. |
| `VIS_L03_CatwalkRail_C5` | Both C5 edges | Requires simple `COL_` rail proxies where player can touch. |
| `VIS_L03_CoolantPipes_C4_R4` | C4 ceiling and R4 west wall | Color/readable state may swap through main-lane material controller. |
| `VIS_L03_OverheadChains_R5` | R5 ceiling | Must stay above `Y=9.6` and outside weapon line of sight. |

## Acceptance Criteria

- Player can finish through either coolant route or lift override route.
- Coolant valve clearly changes furnace hazard state.
- Upper catwalk rail collision prevents falling unless level design intentionally provides a safe drop.
- No active combat space has less than `3m` clear width.
- Peak active enemies do not exceed `9`.
- Lift cannot trap the player below or above; call controls exist on both ends if lift remains reusable.
