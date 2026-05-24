# Brassworks Breach - v0.1.34 Batch Validation Packet

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_34_BatchValidation/`

## Purpose

This packet defines validation and smoke-test gates for the `v0.1.34` combined milestone: staged weapon/prop upgrades, enemy readability upgrades, and level-density polish integrated as one visible Unity-only batch.

The goal is large-batch progress, not one-asset-at-a-time acceptance. A candidate should only pass if the integrated result improves the five-level run while preserving route safety, combat readability, pickup/interaction clarity, and the existing runtime smoke matrix.

## Inputs Reviewed

- `Documentation/Releases/RELEASE_NOTES_v0.1.33.md`
- `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.33.md`
- `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.33.md`
- `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.33.md`
- `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.33.md`
- `Tools/RunV0BuildMatrix.ps1`
- `Tools/RunV0RouteAudit.ps1`
- `Assets/_Project/Scripts/Utility/RuntimeSmokeTest.cs`
- `Documentation/Planning/V0_1_33_BatchPlan/THRESHOLD_ROUTE_DRESSING_BATCH_PLAN_v0.1.33.md`
- `Documentation/Planning/V0_1_33_LevelDensityBatch/LEVEL_DENSITY_BATCH_PLAN_v0.1.33.md`

## Packet Contents

- `BATCH_VALIDATION_GATES_v0.1.34.md` - validator criteria, route-safety checks, and pass/fail rules for the combined batch.
- `TARGETED_SMOKE_COMMANDS_v0.1.34.md` - suggested targeted editor, route-audit, player-smoke, and full-matrix commands using the existing V0 harness.

## Top Validation Gates

1. Batch breadth gate: `v0.1.34` must integrate meaningful work from weapon/prop, enemy readability, and level-density lanes before it can be called a milestone.
2. Route authority gate: decorative density must not add colliders, triggers, nav obstacles, pickup authority, objective authority, damage, transition logic, or route-state ownership.
3. Readability gate: weapon pickups/viewmodels, enemy silhouettes/tells, hazards, route colors, prompts, and objective paths must remain readable from normal play distance.
4. Route audit gate: the five-level route audit must still report no deterministic blockers and preserve the v0.1.33 core route object expectations.
5. Smoke matrix gate: targeted player smokes for weapon switching, combat readability, flow, hazards, interaction, audio, and settings must pass before the full V0 build matrix is accepted.
