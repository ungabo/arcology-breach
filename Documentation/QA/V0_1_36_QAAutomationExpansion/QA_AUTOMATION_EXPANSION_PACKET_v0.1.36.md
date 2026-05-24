# Brassworks Breach - QA Automation Expansion Packet v0.1.36

Generated: `2026-05-24`

Owned scope: `Documentation/QA/V0_1_36_QAAutomationExpansion/`

## Purpose

Summarize the proposed `v0.1.36` QA automation expansion from a QA execution perspective. This packet complements the planning docs and remains docs-only.

## Automation Lanes To Prepare

| Lane | Marker | Manual Companion Sheet |
| --- | --- | --- |
| Route authority | `QA_ROUTE_AUTHORITY_PASS` | Route authority drift sheet |
| Gameplay feedback | `QA_GAMEPLAY_FEEDBACK_PASS` | Gameplay feedback sheet |
| Staged asset imports | `QA_STAGED_ASSET_IMPORT_PASS` | Asset import promotion sheet |
| Enemy readability | `QA_ENEMY_READABILITY_PASS` | Enemy readability sheet |
| Setpiece density | `QA_SETPIECE_DENSITY_PASS` | Density walk sheet |
| Low/mid PC performance | `QA_PC_PERFORMANCE_PASS` | Performance perception sheet |

## Expected Evidence

- Searchable pass markers in automation logs.
- Per-lane result table with `PASS`, `FAIL`, `HOLD`, or `SKIPPED`.
- Scene and route-step identifiers for all failures.
- Manual sheet references linked by automation run ID.
- No requirement for the user to pause development while workers collect targeted evidence.

## Recommended QA Flow

1. Run route authority smoke first.
2. Run staged asset import smoke before promoting art into gameplay scenes.
3. Run gameplay feedback and enemy readability smokes after feedback hooks are in place.
4. Run setpiece density smoke after placement batches.
5. Run low/mid PC performance samples before release-candidate packaging.
6. Assign one manual sheet per worker for short targeted review.
7. Escalate only P0/P1 findings that block route, clarity, or build viability.

## Pass Criteria

- All required lane markers pass or have an explicit `SKIPPED` reason for lanes not implemented yet.
- No route authority drift.
- No staged import creates unauthorized colliders, triggers, prompt ownership, damage, route state, transition state, save state, secret state, or boss-lock state.
- Enemy tells, pickups, objectives, hazards, boss HUD, and final exit remain readable from normal play distance.
- Low/mid PC samples identify hot spots before content density grows further.
- Manual sheets do not find a P0/P1 issue missed by automation.

## Fail Criteria

- Any softlock, crash, hard hang, broken transition, broken pickup, broken weapon grant, broken boss unlock, or broken final exit.
- Any presentation-only asset becomes gameplay authority.
- Any required prompt, pickup, valve, gate, lift, hoist, hazard warning, enemy tell, boss HUD, or final exit is hidden by feedback or set dressing.
- Any smoke cannot identify the scene or route step where a failure happened.

## Hold Criteria

- Visual or audio feedback works but is too noisy in first-person play.
- Setpiece density is technically nonblocking but weakens route orientation.
- Performance stays playable but shows a repeated hitch cluster.
- Manual review contradicts automation but lacks enough evidence for fail.

## Handoff To Main Lane

When implementation begins, the main lane should choose exact runner names, log locations, hardware targets, and thresholds. This packet only defines the intended evidence shape and pass/fail language.
