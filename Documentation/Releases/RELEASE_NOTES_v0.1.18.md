# Brassworks Breach - Release Notes v0.1.18

Generated: `2026-05-24 06:45 -04:00`

## Summary

`v0.1.18` is a Windows distribution polish slice. It improves the packaged ZIP with a launcher, quickstart, support/build-info sheet, stronger package manifest fields, and candidate-readiness checks that prove those distribution files are staged and included in the ZIP.

## Added

- Added `LAUNCH_BRASSWORKS_BREACH.bat` to the Windows package staging folder.
- Added `QUICKSTART_WINDOWS.txt` with extraction, launch, control, quit, and first-run notes.
- Added `SUPPORT_INFO_WINDOWS.txt` with issue-reporting details and candidate scope notes.
- Expanded `README_WINDOWS.txt` with direct launch, folder integrity, and quit/close guidance.
- Expanded the Windows package manifest with launcher, README, quickstart, and support-info paths.
- Expanded candidate-readiness automation to require those files and verify they are present inside the package ZIP.

## Verified Artifacts

- Executable: `Builds/Windows/v0.1.18/BrassworksBreach_v0.1.18.exe`
- Package: `Builds/WindowsPackages/v0.1.18/BrassworksBreach_v0.1.18_Windows.zip`
- Package SHA-256: `8AE31EE0FB1DB296EE6E3236C1563A365BAFD8C3836A728A4306CAF2F447AA0A`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.18.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.18.md`
- Issue triage packet: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.18.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.18.md`

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

Next-step directive: continue immediately with the next highest-impact unfinished task.
