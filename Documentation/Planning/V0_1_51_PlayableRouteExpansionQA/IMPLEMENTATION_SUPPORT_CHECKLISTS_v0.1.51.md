# V0.1.51 Implementation Support Checklists

Use these checklists before QA signs off on a main-lane Level02-Level04 route implementation. All object names and budgets derive from the accepted V0.1.50 route packet.

## Shared Preflight

| Check | Required Result |
| --- | --- |
| Scope | Implementation branch changes only intended Level02-Level04 route content and no unrelated source, package, build, or documentation files. |
| Route containers | Each route root has `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and matching `VISUALONLY_*` child containers. |
| Collision ownership | Player collision belongs only to `GEO_`, `COL_`, `TRG_`, or `AUTH_` objects. |
| Visual isolation | `VIS_` and `VISUALONLY_` objects have no colliders, rigidbodies, scripts, cameras, lights, audio sources, or particles. |
| Grid and traversal | Structure follows `0.5m` snap; main combat path keeps at least `3m` clear width and `2.8m` clear height. |
| Main path proof | Each level can be completed from module entry to rejoin without crouch, jump, physics exploit, console command, or secret route. |

## Level02 Pressure Bypass

| Area | Implementation Checklist |
| --- | --- |
| Root | Create `ROUTE_L02_PressureBypass_v0_1_50` with required child containers. |
| Main geometry | Build R1, C1, R2, C2, R3, C3, and C4 as required traversal volumes. |
| Optional secret | If C5/R4 are present, include `TRG_L02_Secret_BoilerNiche` plus secret rewards; otherwise omit the full secret branch. |
| Objectives | `AUTH_L02_Valve_BypassA` and `AUTH_L02_Valve_BypassB` both gate `AUTH_L02_Door_PumpRoomExit`. |
| Shortcut | `AUTH_L02_Shortcut_PipeGate` opens from R3 after the first pump-room fight and cannot strand the player. |
| Hazards | Steam jet and pump vent are readable before damage, low damage, and not active during unavoidable lock-in. |
| Combat cap | Peak active enemies never exceeds `7`. |
| Acceptance focus | Valve readability from combat angles, two-valve door gating, no secret soft-lock. |

## Level03 Foundry Gantry

| Area | Implementation Checklist |
| --- | --- |
| Root | Create `ROUTE_L03_FoundryGantry_v0_1_50` with required child containers. |
| Main geometry | Build C1, R1, R2, C3, R3, C4, R4, C5, C6, and R5 with the listed elevations. |
| Lift | `AUTH_L03_Lift_CentralGantry` reaches `Y=5.0` and has recovery/call access so it cannot trap the player. |
| Coolant route | `AUTH_L03_CoolantValve_A` disables or visibly cools both furnace strip hazards. |
| Override route | `AUTH_L03_LiftOverride_B` allows completion if the coolant detour is skipped. |
| Catwalk safety | C5/R5 edges use readable rail collision or intentionally safe drops; no surprise lethal fall. |
| Hazards | Furnace strip damage matches visible floor lanes; `AUTH_L03_Hazard_SlagVent_R3_A` remains timed and readable. |
| Combat cap | Peak active enemies never exceeds `9`. |
| Acceptance focus | Dual completion routes, coolant state feedback, lift non-trap, rail collision. |

## Level04 Observatory Pumpworks

| Area | Implementation Checklist |
| --- | --- |
| Root | Create `ROUTE_L04_ObservatoryPumpworks_v0_1_50` with required child containers. |
| Main geometry | Build C1, R1, C2, R2, R3, C4, R4, R5, C6, and R6 as the required route; C5 is optional secret-only. |
| Key gate | `AUTH_L04_Item_PressureKey` is required for `AUTH_L04_Door_KeyedMaintenance_A` and cannot be bypassed by normal traversal. |
| Pump reroute | `AUTH_L04_PumpReroute_A` opens `AUTH_L04_Gate_PumpArenaUpper_C` and reduces pressure jet frequency before arena peak. |
| Arena routes | R4 provides at least two readable routes between lower floor and upper deck. |
| Rejoin gate | `AUTH_L04_Door_RejoinLockroom_D` opens only after vertical arena clear. |
| Secret route | Secret return duct is optional, reward-bearing, and cannot replace required main completion. |
| Hazards | Pressure jets, gear sweep, and overpressure pulse are readable and never block the only exit. |
| Combat cap | Peak active enemies never exceeds `10`. |
| Acceptance focus | Key cannot be skipped, pump state changes before climax, arena clear controls rejoin. |
