# V0.1.50 Level Expansion Routes QA Checklist

Use this checklist after a later main-lane implementation branch builds the three route modules. This packet itself is documentation-only.

## Scope Gate

| Check | Pass/Fail | Notes |
| --- | --- | --- |
| Changes are limited to the later approved main-lane branch, not this documentation packet. |  |  |
| Route roots exist for Level02, Level03, and Level04. |  |  |
| Required child containers exist: `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, `VISUALONLY_*`. |  |  |
| Visual-only sidecar instances are isolated under `VISUALONLY_*`. |  |  |
| Sidecar instances have no gameplay components. |  |  |

## Traversal

| Check | Level02 | Level03 | Level04 |
| --- | --- | --- | --- |
| Route can be completed from entry to rejoin without console commands. |  |  |  |
| No crouch/jump exploit is required for main path. |  |  |  |
| Corridor clear widths meet spec. |  |  |  |
| Player cannot snag on visual props or collision seams. |  |  |  |
| Backtracking works or is intentionally blocked with readable state. |  |  |  |
| Rejoin point returns to expected existing critical path. |  |  |  |

## Objectives

| Check | Level02 | Level03 | Level04 |
| --- | --- | --- | --- |
| Objective prompts appear only in intended interaction radius. |  |  |  |
| Objective progress cannot be completed through walls. |  |  |  |
| Required objective state persists across death/reload according to existing game rules. |  |  |  |
| Door/lock state matches objective completion. |  |  |  |
| Objective visuals change after interaction. |  |  |  |

## Combat

| Check | Level02 | Level03 | Level04 |
| --- | --- | --- | --- |
| Scout beat starts at intended trigger. |  |  |  |
| Peak active enemy count stays within module budget. |  |  |  |
| Enemies can path to player or hold intended ranged positions. |  |  |  |
| No spawn happens in direct player view unless scripted as visible reveal. |  |  |  |
| Player has a retreat option during each pinch beat. |  |  |  |
| Pickups are sufficient for the intended encounter pressure. |  |  |  |

## Hazards

| Check | Level02 | Level03 | Level04 |
| --- | --- | --- | --- |
| Hazard is visually readable before first damage opportunity. |  |  |  |
| Hazard damage volume matches visible danger area. |  |  |  |
| Hazard timing is consistent after reload/checkpoint. |  |  |  |
| Hazard cannot kill player during noninteractive door/lift animation. |  |  |  |
| Hazard state change after objective is visible and audible if audio exists. |  |  |  |

## Secrets

| Check | Level02 | Level03 | Level04 |
| --- | --- | --- | --- |
| Secret route is optional. |  |  |  |
| Secret trigger fires once. |  |  |  |
| Secret reward is collectable and not clipped into geometry. |  |  |  |
| Secret path cannot bypass required critical objective unless explicitly intended. |  |  |  |

## Performance

| Check | Level02 | Level03 | Level04 |
| --- | --- | --- | --- |
| Added static renderer count is within budget. |  |  |  |
| Added collider count is within budget. |  |  |  |
| No new dynamic lights from this route module. |  |  |  |
| Sidecar adds no runtime particles. |  |  |  |
| Module frame cost stays within CPU/GPU budget against current baseline. |  |  |  |

## Required Manual Runs

| Run | Expected Result |
| --- | --- |
| Fresh start route playthrough per level | Player reaches rejoin without blockers. |
| Death/reload during each objective | State restores according to existing checkpoint rules. |
| Slow traversal pass | No hidden collision snags or holes. |
| Fast combat pass | No unfair spawn, door, lift, or hazard overlaps. |
| Secret pass | Secret triggers and rewards behave without affecting main completion. |
