# Brassworks Breach - v0.1.36 Smoke Expansion Plan

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_36_QAAutomationExpansion/`

## Purpose

Define a proposed automated smoke expansion for upcoming gameplay and art milestone batches. This plan is for future main-lane implementation and does not introduce scripts, scenes, validators, or route behavior.

## Expansion Principles

- Smokes should answer one production question each.
- Every smoke should emit a compact pass marker that can be searched in logs.
- Automation should catch regressions before a full human route pass, not replace first-person review.
- Route authority remains the highest-priority invariant.
- Presentation tests should verify clarity and safety without granting gameplay authority to presentation objects.

## Proposed Smoke Lanes

| Lane | Production Question | Proposed Coverage | Primary Pass Marker | Fail Fast On |
| --- | --- | --- | --- | --- |
| Gameplay feedback | Did feedback make actions clearer without changing rules? | Weapon fire, impact, pickup, objective, secret, pause/resume | `QA_GAMEPLAY_FEEDBACK_PASS` | Hidden prompts, rule drift, missing feedback event |
| Staged asset imports | Can imported art be promoted safely? | Asset presence, bounds, material assignment, collider policy, missing references | `QA_STAGED_ASSET_IMPORT_PASS` | Unauthorized collider/trigger, missing renderer, broken material |
| Enemy readability | Are enemies readable at combat distances? | Spawn, silhouette, tell, hit, death, boss HUD chain | `QA_ENEMY_READABILITY_PASS` | Tell hidden, boss unlock unclear, unreadable death |
| Setpiece density | Did density improve without blocking play? | Corridor width, lift/hoist clearance, hazard sightline, prop budget | `QA_SETPIECE_DENSITY_PASS` | Route aperture narrowed, nav snag, hazard hidden |
| Low/mid PC performance | Does the route remain playable on target tiers? | Frame budget sample, scene load, effect burst, combat encounter | `QA_PC_PERFORMANCE_PASS` | Sustained frame breach, memory spike, hitch cluster |
| Route authority | Did the five-level route remain authoritative? | Gates, keys, valves, lifts, hoists, boss lock, final exit | `QA_ROUTE_AUTHORITY_PASS` | Softlock, unauthorized authority owner, route audit mismatch |

## Lane Details

### Gameplay Feedback

Proposed automated checks:

- Verify each supported weapon emits fire, hit, miss, empty, switch, and pickup feedback markers where applicable.
- Verify feedback markers attach to existing authoritative objects or presentation-only children.
- Confirm pickup, objective, and secret feedback events are distinct.
- Exercise pause/resume after feedback bursts.
- Confirm feedback does not change ammo, damage, unlock, pickup, route, or objective state.

Expected evidence:

- Event count summary per weapon, pickup type, objective event, and secret event.
- State-before/state-after snapshot for ammo, health, keys, weapon ownership, objective state, and scene transition state.
- Searchable pass marker: `QA_GAMEPLAY_FEEDBACK_PASS`.

### Staged Asset Imports

Proposed automated checks:

- Inspect promoted asset roots for renderer presence, assigned materials, valid scale, sane bounds, and missing-reference safety.
- Flag runtime colliders/triggers on presentation-only import roots unless explicitly whitelisted by main-lane gameplay ownership.
- Check that imported prop bounds do not intersect route-critical volumes.
- Confirm asset staging categories: `Imported`, `Placed`, `Readable`, `NonAuthoritative`, `GameplayOwned`.

Expected evidence:

- Asset import table with root path, category, renderer count, material count, bounds, collider count, and authority policy.
- Route-overlap result for gates, valves, lifts, hoists, pickups, hazards, boss lock, and exits.
- Searchable pass marker: `QA_STAGED_ASSET_IMPORT_PASS`.

### Enemy Readability

Proposed automated checks:

- Spawn each enemy class in expected route contexts and verify required readability markers are emitted.
- Confirm combat tells are logged before damaging actions.
- Confirm hit and death markers are distinct from attack windups.
- Confirm boss reveal, HUD visibility, guardian lock, defeat, final-hoist unlock, and final exit chain.
- Sample camera-facing distance bands: near, intended combat distance, long approach.

Expected evidence:

- Enemy readability table for Scrapper, Lancer, Bellows Node, Bulwark, and Governor Warden.
- Tell-to-damage timing summary.
- Boss-chain event order summary.
- Searchable pass marker: `QA_ENEMY_READABILITY_PASS`.

### Setpiece Density

Proposed automated checks:

- Measure route aperture clearances at authored choke points before and after density batches.
- Verify decorative additions do not overlap interaction prompts, pickup roots, enemy spawns, hazard warnings, lift/hoist boarding space, or final exit.
- Count setpiece roots by scene and density zone.
- Flag presentation objects that sit inside route-critical volumes without being children of existing owners.

Expected evidence:

- Per-level density zone table.
- Clearance deltas for route-critical points.
- Overlap findings with severity.
- Searchable pass marker: `QA_SETPIECE_DENSITY_PASS`.

### Low/Mid PC Performance

Proposed automated checks:

- Run a deterministic route sample on target low and mid settings.
- Capture frame time around weapon bursts, enemy clusters, hazard rooms, setpieces, scene transitions, Warden reveal, and final exit.
- Track hitch clusters, memory allocations, effect burst counts, active renderers, active lights, and particle counts where available.
- Separate tuning holds from hard fails.

Expected evidence:

- Low and mid PC sample summaries.
- Worst frame-time windows per scene.
- Asset/effect hot spot candidates.
- Searchable pass marker: `QA_PC_PERFORMANCE_PASS`.

### Route Authority

Proposed automated checks:

- Reconfirm the five-level route matrix after each milestone batch.
- Snapshot authoritative owners for gates, keys, valves, lifts, hoists, transitions, pickups, hazards, boss lock, final exit, and secrets.
- Diff authority owners against the previous accepted candidate.
- Fail on any new authority owner introduced by feedback, art, setpiece, or readability-only work.

Expected evidence:

- Route matrix with authority-owner digest.
- Authority drift table.
- Full chain marker from Level01 spawn through Level05 final exit.
- Searchable pass marker: `QA_ROUTE_AUTHORITY_PASS`.

## Batch Escalation Rules

| Result | Meaning | Recommended Action |
| --- | --- | --- |
| All lane markers pass | Candidate is ready for the full V0 matrix and human route review. | Continue to release-candidate evidence. |
| Route authority fails | Candidate may be softlocked or authority drifted. | Stop batch promotion until fixed. |
| Gameplay or enemy readability fails | Player clarity may be worse despite prettier output. | Hold for tuning or targeted fix. |
| Asset import fails | Imported content is not safe to promote. | Keep asset staged, not gameplay-owned. |
| Setpiece density fails | Composition may block or confuse the route. | Remove, move, or reduce offending density. |
| Performance fails | Batch may be too expensive for target PC tier. | Capture hot spot list and tune before adding more art. |

## Suggested Smoke Order

1. Route authority.
2. Staged asset imports.
3. Gameplay feedback.
4. Enemy readability.
5. Setpiece density.
6. Low/mid PC performance.
7. Existing full V0 matrix.
8. Manual sheets for human perception gaps.
