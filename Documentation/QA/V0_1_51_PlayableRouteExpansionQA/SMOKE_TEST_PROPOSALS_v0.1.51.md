# V0.1.51 Smoke Test Proposals

Run these as implementation-support smoke tests before deeper balance, art, or performance QA. Each test should capture pass/fail, build identifier, scene name, route root name, checkpoint/reload notes, and one screenshot at entry, objective, hazard read, and rejoin.

## Shared Smoke Matrix

| Test | Level02 | Level03 | Level04 | Expected Result |
| --- | --- | --- | --- | --- |
| Fresh route completion | Run entry to rejoin. | Run entry to rejoin using either path. | Run entry to rejoin. | Player completes without console, exploit movement, or secret branch. |
| Fast route completion | Sprint through intended main path. | Sprint through lift override path. | Sprint through key, pump, arena, rejoin. | No trigger misses, door desync, lift trap, or late spawn inside player. |
| Slow collision sweep | Hug walls, railings, props, doors. | Sweep ramps, lift, catwalk rails. | Sweep stair, arena deck, pump column, lockroom. | No snag lips, invisible blockers, holes, or visual-only collision. |
| Death/reload | Reload after first objective. | Reload during lift/coolant state. | Reload after key and during arena. | Route state restores according to existing game rules; no permanent lock. |
| Combat cap spot check | Count active peak in R3. | Count active peak in R1/C5. | Count active peak in R4. | Level02 <= `7`, Level03 <= `9`, Level04 <= `10`. |
| Secret isolation | Enter optional secret when present. | Enter crucible shelf when present. | Enter secret return duct when present. | Secret rewards work and are never required for completion. |

## Level02 Pressure Bypass Smokes

| ID | Steps | Expected Result |
| --- | --- | --- |
| `L02-SMOKE-01 Main Path` | Enter `ROUTE_L02_PressureBypass_v0_1_50`, clear R1/C1/R2/R3, operate both valves, open pump-room exit, rejoin. | Both valves are required; `AUTH_L02_Door_PumpRoomExit` opens only after A+B complete. |
| `L02-SMOKE-02 Valve Order` | Complete `AUTH_L02_Valve_BypassB` first if reachable, then A; repeat A then B. | Door state is order-independent and never opens after only one valve. |
| `L02-SMOKE-03 Shortcut Gate` | After first R3 fight, use `AUTH_L02_Shortcut_PipeGate` from both sides. | Gate opens from R3, provides readable shortcut, and does not strand the player. |
| `L02-SMOKE-04 Hazard Read` | Approach C1 steam jet and R3 pump vent slowly, then cross during active/inactive windows. | Nozzle/gauge read appears before damage; timing is consistent and survivable. |
| `L02-SMOKE-05 Secret` | Open/enter secret service duct and collect R4 rewards. | Secret trigger fires once; rewards are collectable; returning to main path is clean. |

## Level03 Foundry Gantry Smokes

| ID | Steps | Expected Result |
| --- | --- | --- |
| `L03-SMOKE-01 Coolant Route` | Take C4/R4 detour, use `AUTH_L03_CoolantValve_A`, return through upper gantry, rejoin at R5. | Furnace strip hazards visibly change and completion path opens. |
| `L03-SMOKE-02 Override Route` | Skip coolant detour, use `AUTH_L03_LiftOverride_B`, ride/call `AUTH_L03_Lift_CentralGantry`, rejoin. | Lift path completes the module without requiring coolant. |
| `L03-SMOKE-03 Lift Recovery` | Trigger lift from below, ride up, step off, call/return as applicable, reload while lift is in motion. | Player cannot be trapped above or below; reload restores usable state. |
| `L03-SMOKE-04 Furnace Damage` | Step into west/east furnace strips before and after coolant. | Damage matches visible hot strips; disabled/cooled state no longer damages or is clearly reduced by design. |
| `L03-SMOKE-05 Catwalk Rails` | Strafe and jump-test C5/R5 rail edges where normal movement allows. | Rail collision prevents accidental falls unless a safe intentional drop is documented. |

## Level04 Observatory Pumpworks Smokes

| ID | Steps | Expected Result |
| --- | --- | --- |
| `L04-SMOKE-01 Key Gate` | Try `AUTH_L04_Door_KeyedMaintenance_A` before key, collect `AUTH_L04_Item_PressureKey`, retry. | Door blocks before key and opens after key; normal traversal cannot bypass it. |
| `L04-SMOKE-02 Pump Reroute` | Use `AUTH_L04_PumpReroute_A`, observe R3/R4 state change, enter upper arena gate. | Pump reroute opens `AUTH_L04_Gate_PumpArenaUpper_C` and reduces pressure jet frequency before arena peak. |
| `L04-SMOKE-03 Arena Routes` | Traverse R4 lower-to-upper and upper-to-lower through both available routes during combat. | At least two readable routes remain open; no single hazard blocks all movement. |
| `L04-SMOKE-04 Rejoin Lock` | Try `AUTH_L04_Door_RejoinLockroom_D` before and after R4 arena clear. | Door remains locked before arena clear and opens after clear. |
| `L04-SMOKE-05 Secret Return` | After pump reroute, discover C5/R5 secret path and use `AUTH_L04_OverlookSwitch_B`. | Secret is optional, trigger fires once, rewards are collectable, and main completion state is unchanged. |
