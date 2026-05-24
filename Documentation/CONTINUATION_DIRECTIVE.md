# Brassworks Breach - Continuation Directive

This directive governs ongoing development.

## Default Production Mode

Starting after `v0.1.32`, default to ambitious batched production. A completed compile should normally be a visible milestone leap, not one tiny prop or one microscopic release.

Batch related environment dressing, gameplay readability, UI, audio, tuning, VFX, and validation improvements together when they share a route, feature, asset family, or player-facing goal. Use targeted checks while developing, then run the full route audit and package/QA/candidate matrix when the batch is coherent enough to become a versioned candidate.

The detailed batching rules live in `Documentation/ProductionManagement/BATCH_PRODUCTION_MODE.md` and should be included in side-agent instructions whenever agents are assigned planning, asset, validation, or lookdev work.

Asset, level, weapon, enemy, and QA work should run as parallel crews whenever write scopes can be separated. Do not plan production as `build asset A, then asset B, then asset C`; assign independent families to side agents and let the main lane integrate them into a single coherent milestone.

Parallel side-agent work should be assigned as whole bundles whenever possible: weapon/prop arsenal, mechanical enemy pack, level module/setpiece pack, gameplay-systems validation, UI/audio/VFX polish, or platform-readiness packets. If the available subagent slots are full, close completed or obsolete agents, then refill those slots with the next independent bundle. The main lane should continue implementation while agents work; it should not idle for side-agent completion unless a result is the immediate blocker.

## 15-Minute PM Review Automation

The active continuation heartbeat runs every 15 minutes. Treat each heartbeat as a project-manager review before continuing implementation:

1. Check active and completed subagents.
2. Review whether the current work is following batch production mode.
3. Ask whether this is the fastest safe development pace achievable with the available machine, Unity install, repo state, and subagent slots.
4. Identify slowdowns, over-testing loops, idle agent capacity, or serial asset work.
5. Decide whether separate Unity sidecar projects or importable asset-pack sandboxes would speed the current production wave without risking the primary game project.
6. Redirect side agents, assign new disjoint bundle work, adjust the next batch, or start a sidecar pipeline task when useful.
7. Continue the main Unity lane immediately after the review.

At the end of each completed step:

1. Update the relevant docs, logs, and task records.
2. Run the applicable smoke/build tests for the changed surface, preferring targeted tests during batch development and the full matrix at batch completion.
3. Commit and push completed work when it is verified.
4. Choose the next highest-impact unfinished task from `WORK_LEDGER.md` or `IMPLEMENTATION_TODO.md`.
5. Begin that next task immediately without waiting for manual review.

Every completed-step record should end with this line:

`Next-step directive: continue immediately with the next highest-impact unfinished task.`

If a task is blocked by tooling, missing assets, platform limits, or a design dependency, record the issue as a TBD/follow-up and continue with the next unblocked task. Keep development moving toward the complete Windows game first, while preserving Android, browser, SteamVR, and Meta compatibility paths.
