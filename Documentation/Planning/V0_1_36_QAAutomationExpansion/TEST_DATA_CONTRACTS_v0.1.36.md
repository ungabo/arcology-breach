# Brassworks Breach - v0.1.36 Test Data Contracts

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_36_QAAutomationExpansion/`

## Purpose

Define future data contracts and expected pass markers for main-lane QA automation implementation. These contracts are planning targets only.

## Shared Contract Shape

Every smoke should emit:

- `run_id`
- `target_version`
- `build_path`
- `scene`
- `lane`
- `started_at`
- `finished_at`
- `result`
- `pass_marker`
- `failure_code`
- `evidence`
- `notes`

Allowed `result` values:

- `PASS`
- `FAIL`
- `HOLD`
- `SKIPPED`

Failure codes should be short, searchable, and stable. Example: `ROUTE_AUTHORITY_DRIFT`, `PROMPT_OCCLUDED`, `ENEMY_TELL_HIDDEN`, `PERF_FRAME_BUDGET`.

## Pass Marker Registry

| Lane | Required Marker | Minimum Meaning |
| --- | --- | --- |
| Route authority | `QA_ROUTE_AUTHORITY_PASS` | Five-level route chain and authority owner checks passed. |
| Gameplay feedback | `QA_GAMEPLAY_FEEDBACK_PASS` | Feedback fired, stayed presentation-only, and did not alter gameplay state. |
| Staged asset imports | `QA_STAGED_ASSET_IMPORT_PASS` | Imported/placed assets meet visual, reference, bounds, and authority policy. |
| Enemy readability | `QA_ENEMY_READABILITY_PASS` | Required enemy tells, hit confirms, deaths, and boss chain remain readable. |
| Setpiece density | `QA_SETPIECE_DENSITY_PASS` | Density additions do not block, occlude, or confuse route-critical spaces. |
| PC performance | `QA_PC_PERFORMANCE_PASS` | Low/mid PC sample stays inside agreed frame, memory, and hitch budgets. |
| Manual supplement | `QA_MANUAL_SUPPLEMENT_REVIEWED` | Human sheet was completed and linked to automation run ID. |

## Route Authority Contract

Required fields:

| Field | Type | Notes |
| --- | --- | --- |
| `scene` | string | `Level01` through `Level05`. |
| `route_step` | string | Gate, key, valve, lift, hoist, transition, boss lock, final exit, or secret. |
| `authority_owner` | string | Existing authoritative object/component path. |
| `owner_digest` | string | Stable digest for drift comparison. |
| `state_before` | object | Minimal route state before interaction. |
| `state_after` | object | Minimal route state after interaction. |
| `unauthorized_authority_count` | number | Must be `0`. |
| `blocking_overlap_count` | number | Must be `0`. |

Expected pass conditions:

- No unauthorized authority owners.
- No route-critical softlocks.
- No new feedback/art object controls route, pickup, prompt, objective, damage, transition, save, secret, or boss-lock state.
- Existing route sequence remains complete from Level01 spawn to Level05 final exit.

Pass marker:

`QA_ROUTE_AUTHORITY_PASS`

## Gameplay Feedback Contract

Required fields:

| Field | Type | Notes |
| --- | --- | --- |
| `feedback_role` | string | Fire, hit, miss, empty, pickup, switch, objective, secret, pause, resume. |
| `source_authority` | string | Existing authoritative owner that produced or authorized the state. |
| `presentation_root` | string | Visual/audio/UI child root. |
| `event_count` | number | Expected to be greater than `0` for exercised events. |
| `state_delta` | object | Health, ammo, keys, weapon, objective, secret, route state. |
| `occlusion_risk` | string | `none`, `low`, `hold`, or `fail`. |

Expected pass conditions:

- Feedback marker fires for exercised event.
- `state_delta` only changes where the underlying gameplay event requires it.
- Presentation roots do not own unauthorized colliders, triggers, damage, pickup, prompt, transition, objective, save, or route state.
- Pause/resume restores input, cursor, time scale, HUD state, camera control, and audio state.

Pass marker:

`QA_GAMEPLAY_FEEDBACK_PASS`

## Staged Asset Import Contract

Required fields:

| Field | Type | Notes |
| --- | --- | --- |
| `asset_id` | string | Stable imported asset identifier. |
| `stage` | string | Imported, placed, readable, non-authoritative, gameplay-owned. |
| `root_path` | string | Scene or prefab root path. |
| `renderer_count` | number | Must be greater than `0` for visible assets. |
| `material_count` | number | Must be greater than `0` unless intentionally invisible. |
| `missing_reference_count` | number | Must be `0`. |
| `bounds` | object | Center, extents, and route-zone classification. |
| `collider_count` | number | Allowed only by stage and ownership policy. |
| `trigger_count` | number | Allowed only by stage and ownership policy. |
| `authority_policy` | string | Presentation-only, gameplay-owned, or rejected. |

Expected pass conditions:

- Visible imported assets have renderers, materials, sane scale, and no missing references.
- Presentation-only imports have no unauthorized colliders/triggers.
- Bounds do not intersect route-critical volumes unless owned by the correct gameplay authority.
- Stage value matches actual behavior.

Pass marker:

`QA_STAGED_ASSET_IMPORT_PASS`

## Enemy Readability Contract

Required fields:

| Field | Type | Notes |
| --- | --- | --- |
| `enemy_type` | string | Scrapper, Lancer, BellowsNode, Bulwark, GovernorWarden. |
| `distance_band` | string | Near, intended, long. |
| `tell_event_count` | number | Must be greater than `0` for attacks. |
| `hit_confirm_count` | number | Must be greater than `0` when damaged. |
| `death_confirm_count` | number | Must be greater than `0` when killed. |
| `boss_chain_order` | array | Required for Warden lane. |
| `readability_result` | string | PASS, FAIL, or HOLD. |

Expected pass conditions:

- Enemy tells precede damaging action.
- Hit, stagger, attack windup, death, and shutdown markers are distinguishable.
- Warden reveal, boss HUD, guardian lock, defeat, final-hoist unlock, and final exit state are ordered and readable.
- Feedback does not imply a new weak-point or route rule.

Pass marker:

`QA_ENEMY_READABILITY_PASS`

## Setpiece Density Contract

Required fields:

| Field | Type | Notes |
| --- | --- | --- |
| `density_zone` | string | Authored zone name or route segment. |
| `scene` | string | Level name. |
| `setpiece_count` | number | Count of roots in zone. |
| `route_clearance_min` | number | Minimum clear route width or clearance. |
| `critical_overlap_count` | number | Must be `0`. |
| `prompt_occlusion_count` | number | Must be `0`. |
| `hazard_sightline_result` | string | PASS, HOLD, or FAIL. |

Expected pass conditions:

- Route-critical clearances remain playable.
- Prompts, pickups, valves, gates, lifts, hoists, hazard warnings, enemy tells, boss HUD, and final exit remain visible.
- Density objects do not become authority owners unless they are explicitly main-lane gameplay objects.

Pass marker:

`QA_SETPIECE_DENSITY_PASS`

## PC Performance Contract

Required fields:

| Field | Type | Notes |
| --- | --- | --- |
| `hardware_tier` | string | LowPC or MidPC. |
| `settings_profile` | string | Stable quality profile name. |
| `sample_window` | string | Route segment or encounter name. |
| `avg_frame_ms` | number | Target budget to be finalized by main lane. |
| `p95_frame_ms` | number | Target budget to be finalized by main lane. |
| `hitch_count` | number | Count above agreed hitch threshold. |
| `memory_mb` | number | Peak sampled memory. |
| `active_renderer_count` | number | Optional hot spot indicator. |
| `active_light_count` | number | Optional hot spot indicator. |
| `particle_count` | number | Optional hot spot indicator. |

Expected pass conditions:

- Average and p95 frame time stay inside chosen LowPC/MidPC budgets.
- No repeated hitch cluster during route-critical combat, hazard, transition, or boss moments.
- Performance evidence identifies hot spots when result is `HOLD` or `FAIL`.

Pass marker:

`QA_PC_PERFORMANCE_PASS`

## Manual Supplement Contract

Required fields:

| Field | Type | Notes |
| --- | --- | --- |
| `automation_run_id` | string | Links sheet to smoke run. |
| `tester` | string | Person or worker identifier. |
| `sheet_name` | string | Manual sheet used. |
| `duration_minutes` | number | Keep short where possible. |
| `result` | string | PASS, FAIL, HOLD. |
| `top_issue` | string | One-sentence highest-risk observation. |
| `evidence_path` | string | Optional screenshot/video/log reference. |

Expected pass conditions:

- Human review finds no P0/P1 clarity issue in the target lane.
- Any HOLD/FAIL includes exact scene, route step, and observed confusion.
- Manual review is linked to the automation run ID.

Pass marker:

`QA_MANUAL_SUPPLEMENT_REVIEWED`
