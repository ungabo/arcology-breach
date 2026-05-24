# Brassworks Breach - Release Notes v0.1.19

Generated: `2026-05-24 06:55 -04:00`

## Summary

`v0.1.19` is a modular environment component promotion slice. Manual route-triage notes were not present, so the roadmap fallback was used: promote the next reusable Unity-owned steampunk component into playable scenes.

## Added

- Added `BoilerControlConsolePrototype` runtime metadata.
- Added `Pipeworks Prototype Boiler Control Console` to Level02.
- Added `Boilerheart Prototype Boiler Control Console` to Level03.
- Added Unity-generated console geometry for blackened iron pedestal bases, angled control panels, aged brass rails/cheeks, lever banks, pressure gauges, indicator lamps, side valve wheels, rivets, and pressure pipes.
- Added editor validation for promotion version, placement roles, required named hierarchy, detail counts, material roles, and non-blocking route-safe collider footprint.
- Added production brief/status docs under `Documentation/AssetProduction/BoilerControlConsolePrototype/`.

## Verified Artifacts

- Executable: `Builds/Windows/v0.1.19/BrassworksBreach_v0.1.19.exe`
- Package: `Builds/WindowsPackages/v0.1.19/BrassworksBreach_v0.1.19_Windows.zip`
- Package SHA-256: `0AEA66B63441E2365542E098516BEB75EF770E93C56F0E5B375CDC7B19FED803`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.19.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.19.md`
- Issue triage packet: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.19.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.19.md`

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
- The console is intentionally non-blocking route dressing; generated primitives have no colliders and validation enforces the route-safe footprint.

Next-step directive: continue immediately with the next highest-impact unfinished task.
