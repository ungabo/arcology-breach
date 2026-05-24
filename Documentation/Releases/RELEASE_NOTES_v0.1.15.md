# Brassworks Breach - Release Notes v0.1.15

Generated: `2026-05-24 05:56 -04:00`

## Summary

`v0.1.15` is a Windows candidate-readiness automation slice. It keeps the playable build unchanged from the verified gameplay route while adding a final candidate evidence report that ties together the executable, packaged ZIP, package hash, route audit, route-QA packet, release notes, and required smoke-log markers.

## Added

- Added `Tools/GenerateWindowsCandidateReadiness.ps1`.
- Wired candidate-readiness generation into `Tools/RunV0BuildMatrix.ps1`.
- Added `V0_WINDOWS_CANDIDATE_PASS` as the final distribution-readiness marker before `V0_BUILD_MATRIX_PASS`.
- Added Markdown and JSON readiness output under `Documentation/Releases/CandidateReadiness/`.
- Refreshed route-audit next-action guidance for the post-candidate-readiness sequence.

## Verified Artifacts

- Executable: `Builds/Windows/v0.1.15/BrassworksBreach_v0.1.15.exe`
- Package: `Builds/WindowsPackages/v0.1.15/BrassworksBreach_v0.1.15_Windows.zip`
- Package SHA-256: `BA4C2322330BE0C20C694006470034D78B5A242EF03253B3377FA0D8CA9A20F7`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.15.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.15.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.15.md`

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
- `V0_WINDOWS_CANDIDATE_PASS`
- `V0_BUILD_MATRIX_PASS`

## Notes

- This is a Windows candidate snapshot, not Android, WebGL, SteamVR, or Meta Quest readiness.
- Manual route notes are still needed before assigning a true v1.0 release label.

Next-step directive: continue immediately with the next highest-impact unfinished task.
