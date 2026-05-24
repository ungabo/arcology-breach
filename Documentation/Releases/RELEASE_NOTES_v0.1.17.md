# Brassworks Breach - Release Notes v0.1.17

Generated: `2026-05-24 06:22 -04:00`

## Summary

`v0.1.17` is a modular environment component promotion slice. It promotes a reusable wall-mounted pipe, gauge, and valve cluster into generated gameplay scenes, with metadata and validator gates matching the pressure gauge and pressure coil promotion pattern.

## Added

- Added `WallPipeGaugeClusterPrototype` runtime metadata.
- Added `Pipeworks Prototype Wall Pipe Gauge Cluster` to Level02.
- Added `Boilerheart Prototype Wall Pipe Gauge Cluster` to Level03.
- Added Unity-generated component geometry for blackened iron mounting plates, aged brass rails, five pipe elements, two cream enamel gauges, one valve wheel, and visible rivets.
- Added editor validation for placement roles, required named hierarchy, detail counts, and material roles.
- Added production brief/status docs under `Documentation/AssetProduction/WallPipeGaugeClusterPrototype/`.

## Verified Artifacts

- Executable: `Builds/Windows/v0.1.17/BrassworksBreach_v0.1.17.exe`
- Package: `Builds/WindowsPackages/v0.1.17/BrassworksBreach_v0.1.17_Windows.zip`
- Package SHA-256: `4EFCD44346BA805D7D7407EABDAA8954CB10CC3C139490978ADC61318E517D92`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.17.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.17.md`
- Issue triage packet: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.17.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.17.md`

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

- This is still Unity-generated prototype geometry, not final authored mesh art.
- The component is intentionally lightweight and wall-mounted so it does not change route collision or encounter flow.

Next-step directive: continue immediately with the next highest-impact unfinished task.
