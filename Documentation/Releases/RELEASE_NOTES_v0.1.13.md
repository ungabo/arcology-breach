# Brassworks Breach - Release Notes v0.1.13

Date: 2026-05-24

## Summary

`v0.1.13` is a Windows route-QA automation slice. It keeps the playable build unchanged in intent while making each verified package easier to hand to a human tester with exact route, build, package, hash, and manual-sheet references.

## Added

- `Tools/GenerateWindowsQAPacket.ps1`.
- Generated route-QA Markdown packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.13.md`.
- Generated route-QA JSON manifest: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.13.json`.
- Automatic refresh of `Documentation/QA/ManualPlaytestV1/README.md` with the exact current executable and QA packet path.
- `V0_WINDOWS_QA_PACKET_PASS` step at the end of the full Windows build matrix.

## Verified Outputs

- Windows executable: `Builds/Windows/v0.1.13/BrassworksBreach_v0.1.13.exe`.
- Windows package: `Builds/WindowsPackages/v0.1.13/BrassworksBreach_v0.1.13_Windows.zip`.
- Package SHA-256: `D9D5FD892A38E858753114C8CA8A9A2F72A59ED39AA4913C3D363F3111ED704B`.
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.13.md`.

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
- `V0_BUILD_MATRIX_PASS`

Next-step directive: continue immediately with the next highest-impact unfinished task.
