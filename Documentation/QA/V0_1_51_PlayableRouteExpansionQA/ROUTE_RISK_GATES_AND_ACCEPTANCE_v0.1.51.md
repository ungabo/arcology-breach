# V0.1.51 Route Risk Gates And Acceptance

These gates decide whether the main-lane Level02-Level04 playable expansion is ready for broader testing. Hold the build on any high-risk gate failure.

## Highest-Risk Validation Gates

| Gate | Level | Hold Condition | Must Pass Before Promotion |
| --- | --- | --- | --- |
| Two-valve pressure door | Level02 | `AUTH_L02_Door_PumpRoomExit` opens with only one valve, never opens with both valves, or desyncs after reload. | Door opens only after `AUTH_L02_Valve_BypassA` and `AUTH_L02_Valve_BypassB` complete, in either order, across reload. |
| Shortcut non-trap | Level02 | `AUTH_L02_Shortcut_PipeGate` traps player, bypasses required valves, or closes behind player without recovery. | Shortcut is useful after R3 fight and never replaces required completion logic. |
| Coolant/lift dual route | Level03 | Either coolant route or lift override route cannot finish the module. | Player can complete by coolant route or override route. |
| Lift recovery | Level03 | `AUTH_L03_Lift_CentralGantry` strands player above/below, clips player, or reloads into unusable state. | Lift has reliable call/recovery behavior and reload-safe state. |
| Furnace hazard state | Level03 | Furnace strips continue damaging after coolant without clear design reason or visual state lies to player. | Coolant visibly changes hazard state and damage behavior matches that state. |
| Pressure key gate | Level04 | `AUTH_L04_Door_KeyedMaintenance_A` can be bypassed through normal traversal or opens before key. | `AUTH_L04_Item_PressureKey` is required and cannot be skipped. |
| Pump-to-arena state | Level04 | R4 arena starts before pump reroute feedback, or pressure jets keep full frequency with no readable change. | Pump reroute visibly changes route state before arena peak. |
| Arena rejoin lock | Level04 | `AUTH_L04_Door_RejoinLockroom_D` opens before arena clear or never opens after clear. | Rejoin is gated only by intended arena clear state. |
| Visual-only isolation | All | Any `VIS_` or `VISUALONLY_` child owns collision, script, rigidbody, light, audio, camera, or particle authority. | Gameplay authority remains under `GEO_`, `COL_`, `TRG_`, `AUTH_`, `SPAWN_`, or approved `FX_`. |
| Combat budget | All | Peak active enemies exceed Level02 `7`, Level03 `9`, or Level04 `10`. | Combat peaks stay within accepted budgets. |

## Per-Level Acceptance Criteria

### Level02 Pressure Bypass

- Player completes entry-to-rejoin through R1/C1/R2/R3/C4 without crouch, jump, or exploit movement.
- Both valves are readable under combat pressure and can be completed in either order.
- `AUTH_L02_Door_PumpRoomExit` never opens unless both valves are complete.
- Steam and pump hazards telegraph before damage and do not overlap unavoidable interaction lock-in.
- Optional secret branch is either complete with trigger and rewards or fully absent.
- R3 combat peak stays at or below `7` active enemies.

### Level03 Foundry Gantry

- Player completes through the coolant route and through the lift override route.
- `AUTH_L03_CoolantValve_A` changes furnace hazard visuals and damage state.
- `AUTH_L03_Lift_CentralGantry` cannot trap the player and survives checkpoint/reload.
- Upper catwalk and high rejoin have rail collision or documented safe drops.
- No combat-capable route narrows below `3m` clear width.
- Catwalk combat peak stays at or below `9` active enemies.

### Level04 Observatory Pumpworks

- Pressure key is required for the keyed maintenance door and cannot be skipped by normal traversal.
- Pump reroute visibly changes arena pressure state before the vertical arena peak.
- Vertical arena has at least two routes between lower floor and upper deck during combat.
- Secret return duct is optional, readable after pump reroute, and never required for completion.
- Rejoin lockroom door opens only after arena clear.
- Arena combat peak stays at or below `10` active enemies.

## Promotion Evidence

| Evidence | Required |
| --- | --- |
| Object hierarchy export or screenshots | Yes, showing route roots and required child containers. |
| Smoke-test notes | Yes, covering all `L02-SMOKE`, `L03-SMOKE`, and `L04-SMOKE` cases. |
| Reload notes | Yes, for Level02 valve state, Level03 lift/coolant state, and Level04 key/pump/arena state. |
| Combat peak count | Yes, one count per level with scene/build identifier. |
| Hazard read screenshots | Yes, before first damage exposure in each route. |
| Secret branch result | Yes, complete-or-absent note for each optional secret. |
| Scope audit | Yes, confirming no unauthorized files changed. |

## Rejection Conditions

- Required route root or child container is missing.
- Required route object name is silently renamed without an approved migration note.
- A main path requires secret traversal, exploit movement, console command, or physics abuse.
- Door, lift, hazard, or checkpoint state can soft-lock the player.
- Visual dressing blocks traversal or owns gameplay authority.
- Hazard damage volume does not match visible readable danger space.
- Rejoin places player behind an unopened critical-path gate.
- Performance or active enemy caps exceed the accepted V0.1.50 budgets without explicit review.
