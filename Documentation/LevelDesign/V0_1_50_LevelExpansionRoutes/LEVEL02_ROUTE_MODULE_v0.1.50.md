# Level02 Route Module: L02_Route_PressureBypass

## Intent

Expand Level02 with a pressure bypass that turns one locked pressure-door segment into a readable combat loop. The route teaches valve objectives under light pressure, gives the player a flank onto the existing critical path, and hides a small boiler-service secret.

## Module Root

| Field | Value |
| --- | --- |
| Root object | `ROUTE_L02_PressureBypass_v0_1_50` |
| Local origin | Existing Level02 pressure-door approach centerline. |
| Recommended placement | Attach entry at existing route node before the first major pressure door. |
| Added footprint | `44m X x 30m Z`, vertical range `Y=0` to `Y=5` |
| Main path duration | 4-5 min |
| Completion state | Opens rejoin door and depressurizes shortcut pipe gate. |

## Top-Down Layout

```text
Z+
  24  [Secret Boiler Niche]
      S1----C5----R4
  16              |
      R2----C2----R3----C4----EXIT
       |          |
   C1  |          C3
       |          |
  0  ENTRY-------R1
      -20  -10    0    10    20    30 X+
```

## Geometry Schedule

| ID | Object Name | Center XYZ | Scale XYZ | Purpose | Notes |
| --- | --- | ---: | ---: | --- | --- |
| R1 | `GEO_L02_PressureDoor_EntryVestibule` | `(0,1.5,0)` | `(10,3,8)` | Entry room | Existing path side opens on west face. |
| C1 | `GEO_L02_PressureBypass_WestPipeCorridor` | `(-10,1.5,8)` | `(4,3,18)` | Northbound bypass corridor | Add two 90-degree sight breaks with wall bays. |
| R2 | `GEO_L02_PressureBypass_ValveRoomA` | `(-10,1.75,18)` | `(12,3.5,10)` | First valve objective room | Ceiling at `Y=3.5`; valve on north wall. |
| C2 | `GEO_L02_PressureBypass_CrossPipeHall` | `(0,1.5,18)` | `(16,3,4)` | Combat connector | Provides flank visibility into R3. |
| R3 | `GEO_L02_PressureBypass_PumpRoom` | `(10,2,18)` | `(12,4,12)` | Main combat pocket | Raised service deck on east half. |
| C3 | `GEO_L02_PressureBypass_ReturnDrop` | `(10,1.5,8)` | `(4,3,12)` | Return corridor | One-way drop optional; no fall damage. |
| C4 | `GEO_L02_PressureBypass_ExitSpine` | `(22,1.5,18)` | `(16,3,4)` | Rejoin corridor | Leads to existing critical path after pressure door. |
| C5 | `GEO_L02_PressureBypass_SecretServiceDuct` | `(0,1.25,26)` | `(18,2.5,2)` | Secret crawl/service route | No combat; width `2m` acceptable. |
| R4 | `GEO_L02_PressureBypass_SecretBoilerNiche` | `(10,1.5,26)` | `(8,3,6)` | Secret reward pocket | Hidden behind pressure grate state. |

## Doors, Exits, and Route State

| Object Name | Center XYZ | Size XYZ | Initial State | Opens When |
| --- | ---: | ---: | --- | --- |
| `AUTH_L02_Door_PressureBypassEntry` | `(-5,1.5,4)` | `(3,3,0.5)` | Open | Always available after Level02 route reaches module. |
| `AUTH_L02_Door_PumpRoomExit` | `(18,1.5,18)` | `(3,3,0.5)` | Locked | `AUTH_L02_Valve_BypassA` and `AUTH_L02_Valve_BypassB` both complete. |
| `AUTH_L02_Grate_SecretServiceDuct` | `(-1,1.25,26)` | `(2,2,0.4)` | Closed | Optional: opens after bypass pressure is reduced. |
| `AUTH_L02_Shortcut_PipeGate` | `(10,1.5,7)` | `(3,3,0.5)` | Closed from entry side | Opens from R3 after first pump-room fight. |

## Objective Interactions

| ID | Object Name | Location | Interaction | Feedback |
| --- | --- | --- | --- | --- |
| O1 | `AUTH_L02_Valve_BypassA` | `(-10,1,23)` | Hold/use for `1.2s` | Pipe knocks, gauge moves from red to amber. |
| O2 | `AUTH_L02_Valve_BypassB` | `(14,1,18)` | Hold/use for `1.2s` while exposed on service deck | Door pressure meter reaches green. |
| O3 | `TRG_L02_Secret_BoilerNiche` | `(4,1,26)` | Enter trigger | Marks secret and plays main-lane stinger. |

## Encounter Beats

| Beat | Trigger | Enemy Composition | Placement | Design Goal |
| --- | --- | --- | --- | --- |
| Scout | Player enters R1 | 2 light melee or basic clockwork enemies | `SPAWN_L02_R1_Scout_A/B` at `(3,0,2)`, `(3,0,-2)` | Let player read entry scale. |
| Pinch | Player crosses midpoint of C1 | 2 ranged light enemies | `SPAWN_L02_C1_Ranged_A/B` in wall bays at `(-12,0,10)`, `(-8,0,15)` | Gentle crossfire without lock-in. |
| ValveCommit | O1 starts | 1 melee, 1 ranged | Spawn from R2 south and C2 east | Punish tunnel vision, not lethal. |
| Pinch | Player enters R3 | 3 light, 1 medium | West floor, east deck, north pump alcove | Teach vertical service deck. |
| RouteReward | Secret found | No enemies | Secret pickup only | Calm reward pocket. |
| Rejoin | Door opens | 1 medium guard beyond exit | `SPAWN_L02_Exit_Guard_A` | Confirms route has rejoined active critical path. |

Peak active enemies: `7`.

## Hazards

| Object Name | Location | Gameplay | Safe Read |
| --- | ---: | --- | --- |
| `AUTH_L02_Hazard_SteamJet_C1_A` | `(-10,1,12)` | Timed lateral steam burst, low damage, `1.0s` on / `2.0s` off. | Visible nozzle and floor scorch before first burst. |
| `AUTH_L02_Hazard_PumpVent_R3_A` | `(8,1,20)` | Intermittent area denial on service deck edge. | Gauge flashes amber before venting. |

## Pickups and Secret Rewards

| Object Name | Location | Type | Notes |
| --- | ---: | --- | --- |
| `SPAWN_L02_Pickup_Health_R2_A` | `(-14,0.5,18)` | Small health | Visible after C1 pinch. |
| `SPAWN_L02_Pickup_Ammo_R3_A` | `(6,0.5,15)` | Primary ammo | Supports R3 fight. |
| `SPAWN_L02_Pickup_ArmorShard_R3_A` | `(14,1.5,21)` | Armor shard | On service deck near Valve B. |
| `SPAWN_L02_Secret_AmmoCache_R4_A` | `(10,0.5,27)` | Secret ammo cache | Behind service duct. |
| `SPAWN_L02_Secret_Health_R4_A` | `(13,0.5,25)` | Medium health | Secret reward. |

## Visual Dressing Anchors

| Visual Container | Anchor | Rule |
| --- | --- | --- |
| `VIS_L02_PressurePipes_C1` | Along C1 ceiling edges | Keep below `Y=3.0` only if outside player collision. |
| `VIS_L02_BrassGaugeCluster_R2` | North wall of R2 near O1 | Gauge needles can animate only via main-lane authority. |
| `VIS_L02_PumpAssembly_R3` | Center `(10,0,20)` | Requires sibling `COL_L02_PumpAssembly_R3` if visually solid. |
| `VIS_L02_SecretBoiler_R4` | East wall of R4 | No collision; use wall proxy if needed. |

## Acceptance Criteria

- Player can complete the route without crouch, jump, or physics exploits.
- Both valves are readable from normal combat angles.
- Exit door cannot open unless both valves have completed.
- Secret is optional and cannot soft-lock the player.
- R3 combat peak never exceeds `7` active enemies.
- All collision is owned by `GEO_`, `COL_`, `TRG_`, or `AUTH_` objects, never by visual-only sidecar instances.
