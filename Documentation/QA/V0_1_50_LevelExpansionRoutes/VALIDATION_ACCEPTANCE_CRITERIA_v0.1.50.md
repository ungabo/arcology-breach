# V0.1.50 Level Expansion Routes Validation Acceptance Criteria

## Promotion Gates

The later implementation branch may be promoted only when all gates below pass.

| Gate | Requirement |
| --- | --- |
| Scope | No unauthorized edits to sidecar packages, `Packages/manifest.json`, unrelated scenes, build scripts, or existing docs. |
| Object presence | All required route roots, containers, geometry, authority, trigger, hazard, and spawn markers exist. |
| Traversal | Each module can be completed from entry to rejoin in a normal player build. |
| Objective logic | Required locks, valves, pump reroutes, and keys gate only their intended route states. |
| Combat | Enemy peaks stay within budgets: Level02 `7`, Level03 `9`, Level04 `10`. |
| Collision | Gameplay collision is owned only by `GEO_`, `COL_`, `TRG_`, and `AUTH_` objects. |
| Visual isolation | `VIS_` and `VISUALONLY_` objects have no gameplay components. |
| Hazards | Hazard damage volumes match readable visual space and cannot soft-lock or unavoidable-kill the player. |
| Secrets | Secret branches are optional, reward-bearing, and not required for completion. |
| Performance | Added renderer, collider, particle, and frame-cost budgets are not exceeded without explicit review. |

## Rejection Conditions

Reject or hold the later implementation if any of these are true:

- A sidecar visual prefab owns a collider, runtime script, rigidbody, camera, light, audio source, or particle system.
- A required objective can be skipped by normal movement.
- A door, lift, or hazard can trap the player without recovery.
- A route rejoin places the player behind an unopened critical-path gate.
- Enemy spawns occur inside the player or within `2m` of the player without a deliberate visible reveal.
- A required combat space narrows below `3m` clear width because of visual dressing or collision proxies.
- Secret geometry exists without a trigger and reward, unless the entire secret branch is intentionally removed.
- Any module exceeds peak active enemy count or adds unbudgeted dynamic lights.

## Suggested Automated Checks

These checks are intentionally phrased for later tooling and do not require this documentation packet to run Unity.

| Check Name | Query |
| --- | --- |
| `V0150_ROUTE_ROOTS_EXIST` | Find exact route root names for Level02, Level03, Level04. |
| `V0150_REQUIRED_CONTAINERS_EXIST` | Under each route root, find `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and matching `VISUALONLY_*`. |
| `V0150_VISUALONLY_NO_GAMEPLAY_COMPONENTS` | Scan children of `VISUALONLY_*` for forbidden components. |
| `V0150_REQUIRED_OBJECTS_EXIST` | Compare scene object names to `SCENE_OBJECT_EXPECTATIONS_v0.1.50.md`. |
| `V0150_COLLISION_PREFIX_OWNERSHIP` | Confirm colliders exist only under allowed prefixes. |
| `V0150_SPAWN_BUDGETS` | Count enabled spawn markers per encounter group and compare to budget. |
| `V0150_HAZARD_TRIGGERS_LINKED` | Confirm every `AUTH_*_Hazard_*` has a visible read object or documented visual anchor. |

## Evidence To Capture

| Evidence | Required |
| --- | --- |
| Route audit log | Yes |
| Build matrix log | Yes |
| Three playthrough notes, one per level | Yes |
| Screenshots of each route entry, objective, hazard read, and rejoin | Yes |
| Object hierarchy screenshot or validator JSON for required names | Yes |
| Performance baseline comparison | Yes before promotion beyond quarantine |

## Known TBDs For Later Main-Lane Owner

- Exact hook points into the current Level02, Level03, and Level04 scene routes must be confirmed inside Unity before placement.
- Enemy archetype names should be mapped to the current combat roster at implementation time.
- Objective scripting should reuse existing valve, lock, pump, lift, key, and secret systems where available.
- Audio and VFX are intentionally not specified as sidecar-owned; later main-lane work should attach them to `AUTH_` or `FX_` objects.
- Lighting treatment is deferred to Unity lookdev; this packet forbids adding dynamic lights as part of the route budget.
