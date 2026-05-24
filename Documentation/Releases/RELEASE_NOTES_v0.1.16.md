# Brassworks Breach - Release Notes v0.1.16

Generated: `2026-05-24 06:06 -04:00`

## Summary

`v0.1.16` is a Windows route-QA issue-triage slice. It adds a generated issue-triage packet so manual playtest notes can be converted into severity-ranked, bucketed work items without waiting for a new planning pass.

## Added

- Added `Tools/GenerateWindowsIssueTriagePacket.ps1`.
- Wired issue-triage packet generation into `Tools/RunV0BuildMatrix.ps1`.
- Added `V0_WINDOWS_ISSUE_TRIAGE_PASS` to the full Windows matrix.
- Added Markdown and JSON issue-triage output under `Documentation/QA/WindowsRouteQA/`.
- Updated Windows candidate readiness to require and link the issue-triage packet.
- Refreshed the manual playtest index with the current issue-triage packet path.

## Verified Artifacts

- Executable: `Builds/Windows/v0.1.16/BrassworksBreach_v0.1.16.exe`
- Package: `Builds/WindowsPackages/v0.1.16/BrassworksBreach_v0.1.16_Windows.zip`
- Package SHA-256: `DBAD1E70DEF2A4BCF72F372F8135529EAF577B591627BDECDF0129700921D186`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.16.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.16.md`
- Issue triage packet: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.16.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.16.md`

## Verification

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_COMBAT_EDGE_PASS`
- `V0_COMBAT_SCENARIO_PASS`
- `V0_WEAPON_SWITCH_PASS`
- `V0_BELLOWS_NODE_PASS`
- `V0_RANGED_COMBAT_PASS`
- `V0_BULWARK_COMBAT_PASS`
- `V0_WARDEN_COMBAT_PASS`
- `V0_INTERACTION_SMOKE_PASS`
- `V0_HAZARD_PASS`
- `V0_SECRET_PASS`
- `V0_PAUSE_FLOW_PASS`
- `V0_MOVEMENT_FEEL_PASS`
- `V0_BALANCE_ENVELOPE_PASS`
- `V0_LEVEL01_FLOW_PASS`
- `V0_MIDGAME_FLOW_PASS`
- `V0_CLIMAX_FLOW_PASS`
- `V0_AUDIO_MIX_PASS`
- `V0_DISPLAY_SETTINGS_PASS`
- `V0_READABILITY_SETTINGS_PASS`
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`
- `V0_BUILD_MATRIX_PASS`

## Notes

- This does not invent manual findings; it creates the intake structure for turning real tester notes into prioritized tasks.
- P0 route, quit/restart, crash, or hard-block issues remain release-candidate blockers.

Next-step directive: continue immediately with the next highest-impact unfinished task.
