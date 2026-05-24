# Brassworks Breach - v0.1.36 Manual Verification Sheets

Created: `2026-05-24`

Owned scope: `Documentation/QA/V0_1_36_QAAutomationExpansion/`

## Purpose

Provide short manual sheets that complement future automation without requiring the user to stop development. Each sheet is designed for a focused worker pass and should link back to an automation `run_id` when available.

## Shared Header

Use this header for every manual pass:

| Field | Value |
| --- | --- |
| Automation run ID |  |
| Build/version |  |
| Tester/worker |  |
| Date/time |  |
| Sheet |  |
| Result | PASS / HOLD / FAIL |
| Top issue |  |
| Evidence path or note |  |

## Sheet 01 - Route Authority Drift

Time target: `10-15 minutes`

Focus:

- Level01 key, pressure gate, service lift.
- Level02 routing valve and Boilerheart lift.
- Level03 scattergun pickup, steam hazards, Bellows Node, pressure valve, Foundry lift.
- Level04 furnace hazards, Bulwark arena, emergency hoist.
- Level05 Warden reveal, guardian lock, defeat, final hoist, final exit.

Checks:

| Check | PASS/HOLD/FAIL | Notes |
| --- | --- | --- |
| Route-critical interactions still belong to expected objects. |  |  |
| No feedback/art/setpiece object appears to own route state. |  |  |
| No new collider or prop blocks route-critical movement. |  |  |
| Locks reject and unlock in the expected order. |  |  |
| Final exit only reads restored after Warden defeat chain. |  |  |

Escalate immediately:

- Softlock.
- Unauthorized route, pickup, objective, transition, save, secret, damage, or boss-lock authority.
- Final exit available too early or unavailable after correct chain.

## Sheet 02 - Gameplay Feedback

Time target: `8-12 minutes`

Focus:

- Pressure Pistol fire, hit, miss, empty, switch, pickup.
- Steam Scattergun fire, hit, miss, empty, switch, pickup.
- Health, ammo, key, weapon, objective, and secret feedback.
- Pause/resume after feedback bursts.

Checks:

| Check | PASS/HOLD/FAIL | Notes |
| --- | --- | --- |
| Weapon feedback improves confirmation without hiding crosshair or enemy tells. |  |  |
| Pickup feedback distinguishes resources from route authority and weapon unlocks. |  |  |
| Objective feedback explains lock/unlock state without implying a new route. |  |  |
| Secret feedback feels optional and lower priority than main route. |  |  |
| Pause/resume restores input, camera, cursor, HUD, time scale, and audio. |  |  |

Escalate immediately:

- Feedback changes ammo, damage, fire rate, pickup ownership, objective state, route state, or secret discovery.
- Feedback hides a required prompt, hazard warning, enemy tell, boss HUD, or final exit.

## Sheet 03 - Enemy Readability

Time target: `10-15 minutes`

Focus:

- Scrapper melee tell, hit confirm, death.
- Lancer windup/projectile tell, hit confirm, death.
- Bellows Node pulse/boost/shutdown.
- Bulwark windup, body mass, hit confirm, shutdown.
- Governor Warden reveal, attack tells, HUD, defeat, lock clear.

Checks:

| Check | PASS/HOLD/FAIL | Notes |
| --- | --- | --- |
| Each enemy is identifiable at intended combat distance. |  |  |
| Attack tells are readable before damage. |  |  |
| Hit feedback is visible but does not hide the next tell. |  |  |
| Death/shutdown is distinct from stagger or attack windup. |  |  |
| Warden chain reads clearly from reveal to final-hoist unlock. |  |  |

Escalate immediately:

- Combat damage feels unexplained.
- Hit/death feedback hides attack tells.
- Warden defeat does not clearly clear the guardian lock/final route.

## Sheet 04 - Staged Asset Import Promotion

Time target: `8-12 minutes`

Focus:

- Newly imported props, weapons, enemy meshes, setpiece pieces, VFX placeholders, and material swaps.
- Promotion stage: imported, placed, readable, non-authoritative, gameplay-owned.

Checks:

| Check | PASS/HOLD/FAIL | Notes |
| --- | --- | --- |
| Asset is visible, scaled plausibly, and has assigned materials. |  |  |
| Missing references are not visible in play or console evidence. |  |  |
| Presentation-only asset has no unauthorized collider or trigger behavior. |  |  |
| Asset does not overlap prompts, pickups, hazards, lifts, hoists, gates, valves, boss lock, or final exit. |  |  |
| Asset stage matches actual gameplay ownership. |  |  |

Escalate immediately:

- Imported asset blocks movement or interaction.
- Missing reference breaks play.
- Presentation-only asset acts like gameplay authority.

## Sheet 05 - Setpiece Density Walk

Time target: `10-15 minutes`

Focus:

- Dense corridors, arenas, lift/hoist spaces, hazard rooms, route turns, secret-adjacent spaces, and boss arena.

Checks:

| Check | PASS/HOLD/FAIL | Notes |
| --- | --- | --- |
| Route direction remains readable through added dressing. |  |  |
| Player movement does not snag on decorative density. |  |  |
| Pickup and objective prompts are not hidden. |  |  |
| Hazard warning language remains stronger than decoration. |  |  |
| Enemy silhouettes remain readable against backgrounds. |  |  |

Escalate immediately:

- Decorative density blocks a route-critical path.
- A required prompt or hazard warning becomes hard to find.
- Combat arena density hides enemy tells or safe movement space.

## Sheet 06 - Low/Mid PC Performance Perception

Time target: `10-15 minutes per profile`

Focus:

- Level loads.
- Weapon feedback bursts.
- Enemy clusters.
- Steam/furnace hazards.
- Dense setpieces.
- Warden reveal/fight/defeat/final route.

Checks:

| Check | PASS/HOLD/FAIL | Notes |
| --- | --- | --- |
| Movement and aim feel responsive. |  |  |
| Combat feedback does not cause visible hitching. |  |  |
| Scene transitions do not stall long enough to feel broken. |  |  |
| Warden reveal/fight remains playable. |  |  |
| Any repeated hitch has scene, route step, and likely cause noted. |  |  |

Escalate immediately:

- Repeated hitch during combat causes player damage or loss of control.
- Scene transition appears frozen or broken.
- Warden sequence becomes unreadable or unplayable.

## Worker-Friendly Reporting Format

Use one short result line per sheet:

`<SHEET> | <RUN_ID> | <PASS/HOLD/FAIL> | <SCENE/STEP> | <TOP_ISSUE>`

Examples:

- `Enemy Readability | run-001 | HOLD | Level04 Bulwark | shutdown read is clear, windup is partly hidden by impact flash`
- `Route Authority | run-002 | FAIL | Level05 final exit | exit appears restored before Warden defeat`
- `Asset Import | run-003 | PASS | Level03 valve room | staged pipe kit visible and non-authoritative`
