# Scene Placement Matrix

Use this as the first quarantine placement pass. It is intentionally visual-only; gameplay ownership stays with existing main-lane systems.

| Family | Prefab count | Suggested scene role | Placement notes |
| --- | ---: | --- | --- |
| `pressure_levers` | 3 | Bridge, boiler, vent-release set dressing | Pair with separate main-lane `AUTH_` interact trigger only if needed. |
| `keyed_locks` | 3 | Doors, vault sockets, service lift keyplates | Place at eye height beside locked routes; do not use as inventory authority. |
| `crank_panels` | 3 | Bulkhead, sluice, iris-door readables | Use as decorative crank targets; separate gameplay state elsewhere. |
| `fuse_boxes` | 3 | Service-door and power-restoration readables | Good wall-mounted objective hints near machinery. |
| `breaker_gauges` | 3 | Power reset banks and redline state readables | Use as non-authoritative state language near breaker rooms. |
| `valve_routing_puzzles` | 3 | Steam routing and pipe puzzle readables | Place above or beside real puzzle triggers; keep puzzle logic outside package. |
| `boss_override_terminals` | 3 | Governor/boss failsafe setpieces | Use as high-contrast objective focus points during boss shutdown moments. |
| `lift_call_stations` | 3 | Lift doors, platforms, hoist calls | Pair with existing lift call logic as sibling authority objects. |
| `pickups` | 3 | Gear key, pressure cell, override fuse plinths | Visual pickup presentations only; real pickup objects remain main-lane. |
| `objective_signage` | 3 | Route arrows, key-required plaque, override warning | Place as readable decals/sign boards without colliders. |

Recommended parent hierarchy:

```text
VisualOnly_ObjectiveInteractablesSet05
  OIS05_PressureLevers
  OIS05_KeyedLocks
  OIS05_CrankPanels
  OIS05_FuseBoxes
  OIS05_BreakerGauges
  OIS05_ValveRouting
  OIS05_BossOverride
  OIS05_LiftCallStations
  OIS05_Pickups
  OIS05_ObjectiveSignage
```
