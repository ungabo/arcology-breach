# Production Tracking Method

## 1. Purpose

This project will grow quickly if we keep adding features and assets. The tracking method must keep four things clear:

1. What is done.
2. What is being worked on now.
3. What needs to happen next.
4. What new work was discovered while building.

## 2. Source of Truth

Use these files as the source of truth:

- `Documentation/BUILD_STATUS.md`: current working state and verification results.
- `Documentation/IMPLEMENTATION_TODO.md`: detailed implementation checklist.
- `Documentation/AAA_VISION_AND_ROADMAP.md`: long-term direction and milestone ladder.
- `Documentation/AAA_ASSET_CATALOG.md`: required assets and status.
- `Documentation/ASSET_PACK_REVIEW.md`: local/available Unity Asset Store packs and import decisions.
- `Documentation/LEVEL_DESIGN_AND_MAPS.md`: level scale, map ladder, and transition mechanics.
- `Documentation/WORK_LEDGER.md`: active task ledger.

GitHub should mirror this structure:

- Issues for tasks/assets/bugs once the backlog becomes large.
- Milestones for versions.
- Labels for area, type, priority, and status.

## 3. Version Milestones

Use simple milestone names:

- `v0.0`: proof of concept.
- `v0.1`: simple presentation pass.
- `v0.2`: combat feel slice.
- `v0.3`: art direction slice.
- `v0.4`: systems foundation.
- `v0.5`: first vertical slice.
- `v1.0`: public prototype release.

Every new task should be assigned to a target milestone or marked `untriaged`.

## 4. Task Format

Every task should include:

- ID.
- Title.
- Type.
- Priority.
- Status.
- Target milestone.
- Owner/agent.
- Acceptance criteria.
- Verification method.
- Links to files/assets if relevant.
- Notes/discovered follow-up work.

Recommended task ID prefixes:

- `CODE`: gameplay, systems, tools.
- `LVL`: level design and layout.
- `ART`: visual assets.
- `ANIM`: animation.
- `AUD`: audio.
- `VFX`: effects.
- `UI`: HUD, menus, settings.
- `TOOL`: build, validation, editor, and import tooling.
- `BUG`: defects.
- `DOC`: documentation.
- `TEST`: validation and QA.

## 5. Status Workflow

Use these statuses:

- `backlog`: known work, not started.
- `ready`: clear enough to begin.
- `in-progress`: actively being changed.
- `blocked`: cannot continue without a decision/tool/fix.
- `review`: implemented, needs inspection.
- `verified`: tested and accepted.
- `deferred`: intentionally postponed.
- `cut`: removed from scope.

Only one or two tasks should be `in-progress` at the same time unless work is intentionally split.

## 6. Priority Rules

- `P0`: blocks the current milestone.
- `P1`: needed for milestone quality.
- `P2`: important but can slip.
- `P3`: polish, expansion, or idea parking.

When priorities conflict, playable game loop comes first:

1. Build stability.
2. Player control.
3. Combat loop.
4. Objective progression.
5. Readability.
6. Presentation.
7. Expansion.

## 7. Daily/Session Workflow

At the start of a work session:

1. Check `git status --short --branch`.
2. Check `BUILD_STATUS.md`.
3. Pick the next `ready` P0/P1 task.
4. Mark it `in-progress` in `WORK_LEDGER.md`.

During work:

1. Keep changes scoped.
2. Add newly discovered work to `WORK_LEDGER.md`.
3. Update asset statuses when assets are created/imported.
4. Run the smallest relevant verification.
5. For a full current Windows V0 matrix, run `powershell -ExecutionPolicy Bypass -File Tools\RunV0BuildMatrix.ps1`.

At the end of a session:

1. Update `BUILD_STATUS.md`.
2. Update `WORK_LEDGER.md`.
3. Commit meaningful changes.
4. Push to GitHub.

## 8. Verification Levels

Use increasing verification strength:

- `compile`: Unity compiles scripts.
- `scene-smoke`: generated/target scene loads and required objects exist.
- `runtime-smoke`: built executable boots and required objects exist.
- `auto-playthrough`: built executable proves a deterministic gameplay route or scenario.
- `manual-playtest`: a human plays through the target flow.
- `regression-playtest`: repeat manual checklist after changes.
- `performance-pass`: frame time, memory, and build size are checked.

Current automated pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_COMBAT_EDGE_PASS`
- `V0_COMBAT_SCENARIO_PASS`
- `V0_RANGED_COMBAT_PASS`
- `V0_BULWARK_COMBAT_PASS`
- `V0_WARDEN_COMBAT_PASS`
- `V0_INTERACTION_SMOKE_PASS`
- `V0_HAZARD_PASS`
- `V0_SECRET_PASS`
- `V0_PAUSE_FLOW_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_BUILD_MATRIX_PASS`

Completed-step records should end with:

`Next-step directive: continue immediately with the next highest-impact unfinished task.`

## 9. Change Control

Before adding a major feature:

1. Add or update a task.
2. Add or update affected assets.
3. Define acceptance criteria.
4. Decide target milestone.
5. Keep implementation small enough to verify.

If a new idea appears mid-task:

- Add it to `WORK_LEDGER.md` as `backlog` or `deferred`.
- Do not derail the current task unless it is a true blocker.

## 10. GitHub Issue Mapping

When GitHub Issues are used:

Labels:

- `type:code`
- `type:art`
- `type:audio`
- `type:level`
- `type:ui`
- `type:bug`
- `type:docs`
- `priority:p0`
- `priority:p1`
- `priority:p2`
- `priority:p3`
- `status:ready`
- `status:blocked`
- `status:review`

Milestones:

- `v0.2 Combat Feel Slice`
- `v0.3 Art Direction Slice`
- `v0.4 Systems Foundation`
- `v0.5 Vertical Slice`

Issue body template:

```markdown
## Goal

## Scope

## Acceptance Criteria

## Verification

## Notes
```

## 11. Continuity Rule

Keep `BUILD_STATUS.md`, `WORK_LEDGER.md`, and `SESSION_LOG.md` current enough that development can continue without pausing. Use `HANDOFF.md` only if a future explicit handoff is requested.
