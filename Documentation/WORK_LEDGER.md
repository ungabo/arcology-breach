# Work Ledger

This file tracks current and upcoming work. It should be updated as tasks are completed, discovered, blocked, or deferred.

Status values:

- `backlog`
- `ready`
- `in-progress`
- `blocked`
- `review`
- `verified`
- `deferred`
- `cut`

## Active Milestone

Current target: `v0.2 Combat Feel Slice`

Primary goal: make the simple FPS loop feel good before major art expansion.

## Current State

| ID | Task | Type | Priority | Status | Milestone | Verification |
| --- | --- | --- | --- | --- | --- | --- |
| CODE-001 | Build v0.0 proof of concept loop | code | P0 | verified | v0.0 | editor/build/runtime smoke |
| CODE-002 | Add light v0.1 presentation feedback | code | P1 | verified | v0.1 | editor/build/runtime smoke |
| DOC-001 | Publish current project to GitHub | docs | P0 | verified | v0.1 | repo pushed |
| DOC-002 | Add AAA roadmap and production tracking docs | docs | P0 | verified | v0.1 | doc review |

## Ready Next

| ID | Task | Type | Priority | Status | Milestone | Acceptance Criteria | Verification |
| --- | --- | --- | --- | --- | --- | --- | --- |
| TEST-001 | Manual Windows playthrough | test | P0 | ready | v0.2 | Complete start-to-exit, confirm death/restart, note tuning issues. | manual-playtest |
| CODE-003 | Tune movement/enemy combat values | code | P0 | ready | v0.2 | Movement, enemy speed, damage, ammo, and health feel fair in manual test. | manual-playtest |
| AUD-001 | Add first simple audio set | audio | P1 | ready | v0.2 | Weapon, pickup, enemy, door, hurt, and win sounds play at correct moments. | scene-smoke + manual-playtest |
| CODE-004 | Improve enemy navigation and obstacle handling | code | P1 | ready | v0.2 | Enemies can pursue in current level without obvious wall pushing failures. | manual-playtest |
| CODE-005 | Add interaction prompts and clearer locked door feedback | code | P1 | ready | v0.2 | Player understands why red door is locked and when it opens. | manual-playtest |

## Backlog

| ID | Task | Type | Priority | Status | Milestone | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| ART-001 | Generate first industrial wall/floor texture set | art | P1 | backlog | v0.3 | Use asset catalog MAT-003 through MAT-005. |
| ART-002 | Generate red key and red door visuals | art | P1 | backlog | v0.3 | Replace cube placeholders. |
| ART-003 | Generate first enemy visual | art | P1 | backlog | v0.3 | Ash Imp, readable melee enemy. |
| ART-004 | Generate centered Iron Pistol visual | art | P1 | backlog | v0.3 | Replace blocky gun. |
| VFX-001 | Replace hit marker sphere with impact spark | vfx | P1 | backlog | v0.3 | Wall and enemy impact readability. |
| UI-001 | Replace text HUD with styled prototype HUD | ui | P2 | backlog | v0.3 | Keep compact and readable. |
| LVL-001 | Rework Level01 into stronger combat slice | level | P1 | backlog | v0.2 | Keep small, add better arena loops. |
| CODE-006 | Convert weapons to data-driven definitions | code | P2 | backlog | v0.4 | Useful before adding more weapons. |
| CODE-007 | Convert enemies to data-driven definitions | code | P2 | backlog | v0.4 | Useful before adding more enemy types. |
| TOOL-001 | Add level validation checks | tool | P2 | backlog | v0.4 | Missing colliders, required objects, objective chain. |

## Discovered Follow-Ups

Add new work here first, then triage it into Ready Next or Backlog.

| ID | Task | Source | Status | Notes |
| --- | --- | --- | --- | --- |
| DISC-001 | Decide whether GitHub Issues should mirror this ledger now or after v0.2 | GitHub publish | backlog | Docs define labels/milestones, but issues have not been created yet. |

## Recently Verified

- `2026-05-22`: v0.1 editor smoke passed with `V0_SMOKE_TEST_PASS`.
- `2026-05-22`: v0.1 Windows build passed with `V0_WINDOWS_BUILD_PASS`.
- `2026-05-22`: v0.1 packaged runtime smoke passed with `V0_RUNTIME_SMOKE_PASS`.
- `2026-05-22`: AAA roadmap, asset catalog, production method, work ledger, and handoff docs created.
