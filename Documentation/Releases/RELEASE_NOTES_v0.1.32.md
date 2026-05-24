# Brassworks Breach v0.1.32

Generated: `2026-05-24 11:52 -04:00`

## Release Focus

`v0.1.32` promotes `ValveWheelConsolePrototype`, a route-safe steampunk wall-console dressing asset for the Pipeworks and Boilerheart pressure routes.

## Changes

- Added `ValveWheelConsolePrototype` metadata for Unity-owned generated geometry.
- Added collider-free pressure-console placements:
  - `pipeworks_pressure_console`
  - `boilerheart_pressure_console`
- Added blackened iron backplates and raised panels, aged brass valve wheel rings, blackened iron spokes, brass grips/hubs, cream pressure gauges, dark gauge needles, amber pilot lamps, pressure pipes, brass rivets, oil grime, soot, and worn rim highlights.
- Added validator coverage for promotion version, placement roles, visual-only gameplay authority, named hierarchy, material roles, detail counts, zero colliders, zero `NavMeshObstacle` components, and no gameplay-authority components.
- Integrated the previously prepared `ValveWheelConsolePrototype` sidecar production brief/status docs and marked the component verified.
- Prepared a docs-only `PistonDoorBracePrototype` brief/status packet for the next environment batch.
- Built and packaged Windows candidate `v0.1.32`.

## Artifacts

- Executable: `Builds/Windows/v0.1.32/BrassworksBreach_v0.1.32.exe`
- Package: `Builds/WindowsPackages/v0.1.32/BrassworksBreach_v0.1.32_Windows.zip`
- Package SHA-256: `4A4E6452875756E8D5A4FCA5CFF7D24844D9F476C52B4345BB34BF78E7A607AC`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.32.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.32.md`
- Issue triage: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.32.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.32.md`

## Verification

Full V0 matrix passed with `V0_BUILD_MATRIX_PASS`, including route audit, scene rebuild, level validation, editor smoke, Windows build, packaged runtime tests, Windows package, QA packet, issue-triage packet, and candidate-readiness evidence.

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

Switch to batched production mode for `v0.1.33+`: implement several related environment/readability assets per verified slice, use targeted validation while developing, and reserve full package/QA/candidate runs for coherent milestones.
