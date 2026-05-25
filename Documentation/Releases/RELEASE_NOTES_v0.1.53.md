# Brassworks Breach - Release Notes v0.1.53

Generated: `2026-05-24 20:05 -04:00`

## Summary

`v0.1.53` is a playable Windows material-binding and route-polish build. It brings Surface Material Detail Set 08 into the main Unity package manifest, binds its texture maps to active gameplay materials, and improves Level02-Level04 route readability after the larger route-expansion pass.

## Player-Facing Changes

- Active floor, wall, gate, brass, iron, warning, gauge, and glass materials now use Set08 albedo, normal, RMA, and grime maps.
- Level02 pressure bypass now has clearer manual-bleed, pump-vent, safe-pocket, rejoin, and secret-clue language.
- Level03 foundry gantry now has clearer floor/gantry/walkway/return/rejoin labels plus hazard-preview and breath-pocket gauges.
- Level04 observatory pumpworks now has clearer pump-state, return, arena warning, jet safe-pocket, and Bulwark-release readability cues.
- Windows QA packet generation is now more reliable because the build matrix generates the route audit before QA packet creation.

## Verified Build

- Executable: `Builds/Windows/v0.1.53/BrassworksBreach_v0.1.53.exe`
- Package: `Builds/WindowsPackages/v0.1.53/BrassworksBreach_v0.1.53_Windows.zip`
- Package SHA-256: `E2925407F8D4535044CB40D1E8D1AEDBEB8C170A684ECD746BC24C7E08C6594B`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.53.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.53.md`
- Issue triage: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.53.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.53.md`

## Verification

- `V0_LEVEL_VALIDATION_PASS`
- `SIDECAR_QUARANTINE_IMPORT_PASS packages=18 assets=161`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- Full player smoke suite through runtime, auto-playthrough, combat, interaction, hazard, secret, pause, movement, balance, level-flow, audio, display, readability, gameplay feedback, and world-label readability.
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_ROUTE_AUDIT_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`
- `V0_BUILD_MATRIX_PASS v0.1.53`

## Notes

Steam Corridor Dressing Set 09, Clockwork Enemy Parts Set 09, and `roomtest` v0.3 are parallel side-lane outputs for future review. They are not promoted into this playable build.

Next-step directive: continue immediately with the next highest-impact unfinished task.
