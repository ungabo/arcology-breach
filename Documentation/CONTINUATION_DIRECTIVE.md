# Brassworks Breach - Continuation Directive

This directive governs ongoing development.

At the end of each completed step:

1. Update the relevant docs, logs, and task records.
2. Run the applicable smoke/build tests for the changed surface.
3. Commit and push completed work when it is verified.
4. Choose the next highest-impact unfinished task from `WORK_LEDGER.md` or `IMPLEMENTATION_TODO.md`.
5. Begin that next task immediately without waiting for manual review.

If a task is blocked by tooling, missing assets, platform limits, or a design dependency, record the issue as a TBD/follow-up and continue with the next unblocked task. Keep development moving toward the complete Windows game first, while preserving Android, browser, SteamVR, and Meta compatibility paths.
