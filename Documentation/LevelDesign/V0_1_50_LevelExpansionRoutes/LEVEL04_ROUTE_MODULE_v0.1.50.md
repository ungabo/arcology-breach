# Level04 Route Module: L04_Route_ObservatoryPumpworks

## Intent

Expand Level04 with a pumpworks wing that supports a larger objective branch: find a pressure key, reroute a pump, survive a vertical arena, and rejoin through an observatory maintenance door. The module should feel like late-level escalation without requiring boss systems.

## Module Root

| Field | Value |
| --- | --- |
| Root object | `ROUTE_L04_ObservatoryPumpworks_v0_1_50` |
| Local origin | Existing Level04 observatory service junction. |
| Recommended placement | Side branch before the final third of Level04. |
| Added footprint | `60m X x 46m Z`, vertical range `Y=0` to `Y=12` |
| Main path duration | 6-8 min |
| Completion state | Unlocks maintenance rejoin and optionally opens secret ammo/overcharge cache. |

## Top-Down Layout

```text
Z+
  40          R6 Rejoin Lockroom
              |
  32     R5---C6
         |    |
  24 C5--R4---C4
     |        |
  14 R2--C2--R3
     |        |
   4 ENTRY-C1-R1
     -24 -12  0   12   24   36 X+
```

## Elevation Notes

```text
Y=10.0 R5 observatory overlook
Y=7.0  C6 maintenance stair upper landing
Y=4.0  R4 pump arena upper deck
Y=0.0  ENTRY/R1/R2/R3 lower floor
```

## Geometry Schedule

| ID | Object Name | Center XYZ | Scale XYZ | Purpose | Notes |
| --- | --- | ---: | ---: | --- | --- |
| C1 | `GEO_L04_Pumpworks_EntryConduit` | `(-6,1.5,4)` | `(18,3,4)` | Entry corridor | Should bend from existing junction. |
| R1 | `GEO_L04_Pumpworks_KeyedAntechamber` | `(6,2,4)` | `(14,4,10)` | Locked door read | Shows maintenance lock before key pickup. |
| C2 | `GEO_L04_Pumpworks_LowerSpine` | `(0,1.5,14)` | `(24,3,4)` | Lower combat connector | Connects key room and pump arena. |
| R2 | `GEO_L04_Pumpworks_PressureKeyRoom` | `(-14,2,14)` | `(14,4,12)` | Objective key room | Contains pressure key and first secret tell. |
| R3 | `GEO_L04_Pumpworks_RegulatorRoom` | `(14,2,14)` | `(14,4,12)` | Pump reroute room | Contains pump interaction. |
| C4 | `GEO_L04_Pumpworks_EastRiser` | `(14,2.5,24)` | `(4,5,16)` | Stair/ramp to pump arena | Climbs to upper arena entry at `Y=4`. |
| R4 | `GEO_L04_Pumpworks_VerticalPumpArena` | `(0,5,24)` | `(22,10,18)` | Main combat arena | Lower floor `Y=0`, upper deck `Y=4`. |
| C5 | `GEO_L04_Pumpworks_SecretReturnDuct` | `(-16,4.75,24)` | `(4,2.5,20)` | Secret side route | Opens after pump reroute. |
| R5 | `GEO_L04_Pumpworks_ObservatoryOverlook` | `(-4,11.5,32)` | `(18,3,12)` | High reward/rejoin approach | Floor at `Y=10`. |
| C6 | `GEO_L04_Pumpworks_MaintenanceStair` | `(8,6,32)` | `(6,8,18)` | Stair/ramp to lockroom | Connects R4/R5/R6. |
| R6 | `GEO_L04_Pumpworks_RejoinLockroom` | `(8,8.5,40)` | `(14,5,10)` | Exit/rejoin lockroom | Rejoins existing Level04 path. |

## Doors, Locks, and Exits

| Object Name | Center XYZ | Size XYZ | Initial State | Opens When |
| --- | ---: | ---: | --- | --- |
| `AUTH_L04_Door_KeyedMaintenance_A` | `(6,1.5,9)` | `(4,4,0.5)` | Locked | Player has `AUTH_L04_Item_PressureKey`. |
| `AUTH_L04_Door_RegulatorSeal_B` | `(10,1.5,14)` | `(3,3,0.5)` | Closed | R1 antechamber entered. |
| `AUTH_L04_Gate_PumpArenaUpper_C` | `(8,4,24)` | `(4,4,0.5)` | Locked | `AUTH_L04_PumpReroute_A` complete. |
| `AUTH_L04_Door_RejoinLockroom_D` | `(8,8,36)` | `(4,4,0.5)` | Locked | R4 vertical arena clear. |
| `AUTH_L04_Grate_SecretReturn_E` | `(-16,4,18)` | `(2,2.5,0.4)` | Hidden/closed | Pump reroute complete and secret latch shot or used. |

## Objective Interactions

| ID | Object Name | Location | Interaction | Result |
| --- | --- | --- | --- | --- |
| O1 | `AUTH_L04_Item_PressureKey` | `(-17,1,15)` | Pickup | Unlocks `AUTH_L04_Door_KeyedMaintenance_A`. |
| O2 | `AUTH_L04_PumpReroute_A` | `(16,1,16)` | Hold/use for `2.0s` | Starts arena prep, opens upper pump gate, reduces pressure jets. |
| O3 | `AUTH_L04_OverlookSwitch_B` | `(-8,10.5,34)` | Hold/use for `1.0s` | Opens secret cache if secret route discovered. |
| O4 | `TRG_L04_Secret_ReturnDuct` | `(-16,4,24)` | Enter trigger | Marks secret route discovery. |

## Encounter Beats

| Beat | Trigger | Enemy Composition | Placement | Design Goal |
| --- | --- | --- | --- | --- |
| Scout | Enter R1 | 2 light, 1 ranged | R1 east and north wall bays | Establish Level04 branch danger. |
| RouteReward | Pick up O1 | 2 ambush light enemies | R2 entry return corners | Key pickup has cost but no lock-in. |
| ValveCommit | Start O2 | 1 medium, 2 ranged | R3 balcony grates and lower door | Exposed interaction beat. |
| HazardRead | Enter R4 | 2 light enemies visible across inactive jet lanes | Lower arena | Player reads pressure jets before main wave. |
| Pinch | R4 arena center crossed | 4 light, 2 ranged, 1 medium | Lower floor, upper deck, stair landing | Large route climax with vertical movement. |
| RouteReward | Enter C5/R5 secret path | No enemies or 1 light guard max | Secret return duct | Cooldown and reward. |
| Rejoin | R6 door opens | 1 medium, 2 light | Beyond lockroom | Confirms return to critical route. |

Peak active enemies: `10`.

## Hazards

| Object Name | Location | Gameplay | State Rule |
| --- | ---: | --- | --- |
| `AUTH_L04_Hazard_PressureJet_R4_North` | `(0,1,30)` | Horizontal burst crossing lower arena. | Reduced frequency after O2. |
| `AUTH_L04_Hazard_PressureJet_R4_South` | `(0,1,18)` | Horizontal burst crossing lower arena. | Reduced frequency after O2. |
| `AUTH_L04_Hazard_GearSweep_R4_Deck` | `(5,4.1,24)` | Slow rotating deck denial, low damage/knockback. | Always on; clear visual sweep radius. |
| `AUTH_L04_Hazard_Overpressure_R3` | `(14,1,16)` | Brief room shake/steam pulse after O2. | One-shot, never blocks exit. |

## Pickups and Secrets

| Object Name | Location | Type | Notes |
| --- | ---: | --- | --- |
| `SPAWN_L04_Pickup_Ammo_R1_A` | `(3,0.5,2)` | Primary ammo | Before route branches. |
| `SPAWN_L04_Pickup_Health_R2_A` | `(-10,0.5,16)` | Medium health | Key-room reward. |
| `SPAWN_L04_Pickup_Armor_R3_A` | `(18,0.5,12)` | Armor shard | Supports pump interaction. |
| `SPAWN_L04_Pickup_Ammo_R4_A` | `(-4,0.5,20)` | Primary ammo | Arena lower floor. |
| `SPAWN_L04_Pickup_Health_R4_B` | `(6,4.5,28)` | Small health | Upper deck. |
| `SPAWN_L04_Secret_Overcharge_R5_A` | `(-8,10.5,30)` | Secret high-value pickup | Requires secret duct and overlook switch. |
| `SPAWN_L04_Secret_Ammo_R5_B` | `(-2,10.5,34)` | Secret ammo | Overlook reward. |

## Visual Dressing Anchors

| Visual Container | Anchor | Rule |
| --- | --- | --- |
| `VIS_L04_ObservatoryLens_R5` | R5 west wall/window frame | Visual only; no camera/lens scripts. |
| `VIS_L04_PumpColumn_R4` | R4 center | Use simple sibling collision cylinder/box; keep player lane `>=4m`. |
| `VIS_L04_PressureKeyPedestal_R2` | Around O1 | Pedestal collision owned by `COL_L04_KeyPedestal_R2`. |
| `VIS_L04_RegulatorConsole_R3` | Around O2 | Console visual does not own interaction trigger. |
| `VIS_L04_GearSweepVisual_R4` | Around hazard sweep | Damage and movement authority stays in `AUTH_L04_Hazard_GearSweep_R4_Deck`. |

## Acceptance Criteria

- Pressure key is required for the keyed maintenance door and cannot be skipped from normal traversal.
- Pump reroute visibly changes arena pressure state before the arena peak.
- Vertical arena has at least two routes between lower floor and upper deck.
- Secret return duct is optional, readable after pump reroute, and never required for completion.
- Rejoin lockroom door cannot open before arena clear.
- Peak active enemies do not exceed `10`.
