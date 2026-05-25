# V0.1.53 Risk List

## High Risks

| Risk | Why It Matters | QA Focus | Hold Condition |
| --- | --- | --- | --- |
| Dark/wet materials hide gameplay affordances | SMD08 intentionally adds dark stone, black iron, oil, soot, and wetness; these can reduce route, pickup, and hazard readability. | Check doors, valves, key pedestal, hazard lanes, enemy silhouettes, and pickups in normal play lighting. | Required objective, route exit, hazard tell, or pickup becomes hard to read from intended approach angle. |
| Visual-only dressing gains gameplay components | Route docs require collision and gameplay authority outside `VIS_` and `VISUALONLY_`. | Scan visual containers after material/route polish. | Any visual-only child blocks player/projectiles/enemies or owns gameplay scripts, audio, lights, cameras, or particles. |
| Material candidates promoted too early | Overlay and transparent/glass candidates have known shader limitations. | Verify candidate materials were either held or explicitly approved with a shader path. | Overlay/decal materials are placed as opaque grime sheets in playable views, or glass binding creates pink/missing/incorrect surfaces. |
| Level04 key/pump gate sequence regresses | Level04 depends on key, pump reroute, arena clear, and rejoin lockroom sequencing. | Smoke key door, pump reroute, arena gates, secret duct, and rejoin door in order. | Key can be skipped, rejoin opens before arena clear, or pump reroute is not visible before arena peak. |

## Medium Risks

| Risk | Why It Matters | QA Focus | Hold Condition |
| --- | --- | --- | --- |
| Furnace/coolant state loses contrast | Level03 requires coolant feedback to communicate hazard state change. | Compare furnace strip active/inactive states after material polish. | Coolant valve completes but furnace hazard state is visually ambiguous. |
| Wet/oily floors read as hazards everywhere | Black oil and wet stone can make safe floors feel dangerous or hide real hazard lanes. | Check Level03 furnace room and Level04 arena lower floor. | Safe route and damage route cannot be distinguished quickly during combat. |
| Catwalk and rail collision looks misleading | Level03 upper gantry needs readable edges and rail proxies. | Run along C5/C6 edges and fight from catwalk. | Player snags on invisible lips, falls through expected rail, or rail looks passable where blocked. |
| Enemy cap or encounter pacing drifts during polish | Route docs cap active enemies at 7/9/10 for Levels 02/03/04. | Check combat peaks after any route or spawn marker edits. | Active enemy count exceeds level cap during intended route play. |
| Optional secret branch becomes partial | Optional secrets may be fully present or fully absent only. | Inspect L02 boiler niche, L03 crucible shelf, L04 secret return duct. | Secret geometry exists without trigger/reward/return, or secret route becomes required. |

## Low Risks

| Risk | Why It Matters | QA Focus | Hold Condition |
| --- | --- | --- | --- |
| Placeholder gasket material distracts | `SMD08_MAT_CrackedBlackRubberGasket` is noted as placeholder quality. | Check whether it appears on hero-route surfaces. | Placeholder becomes a prominent route or objective material without explicit approval. |
| Red enamel overuse weakens hazard language | Red pressure enamel should be reserved for warnings and pressure affordances. | Scan broad walls/floors and non-hazard trim. | Red becomes a general decorative material and no longer reliably means pressure/hazard/attention. |
| Preview-to-Unity mismatch | SMD08 previews are procedural docs, not Unity scene captures. | Compare in-scene binding against preview intent without expecting exact match. | Material reads fundamentally unlike its role, such as brass reading flat yellow or wet floor reading matte black. |

## Mitigation Shortlist

- Bind final-candidate materials first; defer overlay and transparent candidates until shader behavior is explicit.
- Smoke routes before dense visual dressing, then repeat after material binding.
- Keep red enamel sparse and tied to hazard, pressure, gate, valve, or warning affordances.
- Place wet/oily materials in patches and preserve contrast near route exits, pickups, enemy lanes, and objective prompts.
- Treat missing object names, visual-only collision, and route soft-locks as release holds.
