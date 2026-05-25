# V0.1.53 Smoke Test Marker Plan

This plan adds QA marker coverage for material binding and route polish. It complements the current runtime smoke test, which checks core gameplay components, settings, audio, performance profile, combat, interaction, hazard, secret, movement, level flow, climax flow, display, and readability systems when launched with `-v0RuntimeSmoke`.

## Preflight Smoke

| Marker | Action | Pass Condition |
| --- | --- | --- |
| `SMOKE_V0153_IMPORT_SMD08` | Open project after material batch import/binding. | No missing shader, missing material, missing texture, or import exception for SMD08 material assets. |
| `SMOKE_V0153_RUNTIME_BASELINE` | Run current runtime smoke path with `-v0RuntimeSmoke`. | Existing smoke finishes with `V0_RUNTIME_SMOKE_PASS`. |
| `SMOKE_V0153_ROUTE_ROOTS` | Inspect Level02-Level04 scenes or route implementation scene. | Required route roots and child containers exist. |
| `SMOKE_V0153_VISUALONLY_COMPONENTS` | Scan `VIS_` and `VISUALONLY_*` children. | No gameplay components or player-blocking collision live under visual-only containers. |

## Level02 Pressure Bypass

| Marker | Route Step | Pass Condition |
| --- | --- | --- |
| `SMOKE_L02_ENTRY_TO_R2` | Enter R1, clear scout beat, traverse C1 to R2. | C1 steam jet is readable; dark/wet stone or iron binding does not hide path or enemies. |
| `SMOKE_L02_VALVE_A` | Use `AUTH_L02_Valve_BypassA`. | Interaction prompt is readable; gauge/pipe state change is visible. |
| `SMOKE_L02_PUMP_ROOM` | Traverse C2 to R3 and fight pump-room beat. | `AUTH_L02_Hazard_PumpVent_R3_A` is readable; Valve B is visible from normal combat angles. |
| `SMOKE_L02_EXIT_GATE` | Use Valve B and open `AUTH_L02_Door_PumpRoomExit`. | Door opens only after both valves; shortcut pipe gate cannot strand player. |
| `SMOKE_L02_SECRET_OPTIONAL` | If secret branch exists, enter C5/R4. | Secret is reward-only and does not become required for completion. |

## Level03 Foundry Gantry

| Marker | Route Step | Pass Condition |
| --- | --- | --- |
| `SMOKE_L03_FURNACE_READ` | Enter C1/R1 and observe furnace strips. | `AUTH_L03_Hazard_FurnaceStrip_West/East` contrast against bound materials and are readable before damage. |
| `SMOKE_L03_COOLANT_ROUTE` | Take C4 to R4 and use `AUTH_L03_CoolantValve_A`. | Coolant state visibly changes furnace hazard state. |
| `SMOKE_L03_LIFT_OVERRIDE` | Use or recover `AUTH_L03_Lift_CentralGantry` through either coolant or override path. | Lift reaches intended upper route and cannot trap player above or below. |
| `SMOKE_L03_CATWALK` | Cross C5/C6 to R5. | Rail/edge collision is readable; iron/brass trim does not blur route edges. |
| `SMOKE_L03_REJOIN` | Open high rejoin gate. | Route completes without requiring secret shelf. |

## Level04 Observatory Pumpworks

| Marker | Route Step | Pass Condition |
| --- | --- | --- |
| `SMOKE_L04_KEY_READ` | Enter R1/R2 and collect `AUTH_L04_Item_PressureKey`. | Key remains visible against pedestal/floor material binding. |
| `SMOKE_L04_KEYED_DOOR` | Return to `AUTH_L04_Door_KeyedMaintenance_A`. | Door cannot be bypassed without key and opens after key pickup. |
| `SMOKE_L04_PUMP_REROUTE` | Use `AUTH_L04_PumpReroute_A` in R3. | Pump state change and overpressure pulse are readable and do not block exit. |
| `SMOKE_L04_ARENA_JETS` | Enter R4 and cross lower/upper arena routes. | Pressure jet lanes remain visible; at least two lower-to-upper routes are usable. |
| `SMOKE_L04_REJOIN_LOCKROOM` | Clear arena and open `AUTH_L04_Door_RejoinLockroom_D`. | Door opens only after arena clear; route rejoins critical path. |
| `SMOKE_L04_SECRET_OPTIONAL` | If C5/R5 secret branch exists, enter and return. | Secret remains optional and reward-bearing. |

## Report Output Targets

QA reports should include:

- Build or branch identifier.
- Whether `-v0RuntimeSmoke` passed.
- Screenshot or note for each material marker failure.
- Object name for any missing marker.
- Exact room/route beat where readability, soft-lock, or collision failed.
