# Brassworks Breach v0.1.31

Generated: `2026-05-24 11:10 -04:00`

## Release Focus

`v0.1.31` promotes `GearKeyPlinthPrototype`, a route-safe steampunk presentation plinth for the existing Level01 gear-key pickup.

## Changes

- Added `GearKeyPlinthPrototype` metadata for Unity-owned generated geometry.
- Added the `intake_gear_key_plinth` placement around the current Level01 gear-key pickup without taking pickup authority away from the existing pickup object.
- Added blackened iron pedestal parts, aged brass gear cradle detail, ten brass gear teeth, cream enamel gauge/label elements, amber key-ready lamp detail, rivets, trim, oil streaks, soot, and worn-edge highlights.
- Added validator coverage for promotion version, placement role, named hierarchy, material roles, child-count gates, zero colliders, zero `NavMeshObstacle` components, no gameplay-authority components, and preserved pickup reachability.
- Integrated the gear-key plinth sidecar production brief/status docs.
- Prepared a docs-only side-agent brief for `ValveWheelConsolePrototype` as the next likely route-safe steampunk console prop slice.
- Built and packaged Windows candidate `v0.1.31`.

## Artifacts

- Executable: `Builds/Windows/v0.1.31/BrassworksBreach_v0.1.31.exe`
- Package: `Builds/WindowsPackages/v0.1.31/BrassworksBreach_v0.1.31_Windows.zip`
- Package SHA-256: `E8BECB40042BE5F20BBC1DA1AE98E04D23F84D4A0E9937639B4D9EC9E39103F5`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.31.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.31.md`
- Issue triage: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.31.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.31.md`

## Verification

Full V0 matrix passed with `V0_BUILD_MATRIX_PASS`, including route audit, level validation, editor smoke, Windows build, packaged runtime tests, Windows package, QA packet, issue-triage packet, and candidate-readiness evidence.

Key markers:

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`
- `V0_BUILD_MATRIX_PASS`

## Next

Continue immediately with `v0.1.32`, using the prepared `ValveWheelConsolePrototype` brief unless a higher-impact route or distribution hardening task emerges.
