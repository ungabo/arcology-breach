# Brassworks Breach v0.1.30

Generated: `2026-05-24 10:45 -04:00`

## Release Focus

`v0.1.30` promotes `ServiceLiftCallBoxPrototype`, a route-safe steampunk lift/hoist control dressing asset placed beside the current five transition points.

## Changes

- Added `ServiceLiftCallBoxPrototype` metadata for Unity-owned generated geometry.
- Added five collider-free call box placements:
  - `intake_service_lift_call_box`
  - `pipeworks_service_lift_call_box`
  - `boilerheart_service_lift_call_box`
  - `foundry_emergency_hoist_call_box`
  - `governor_master_hoist_call_box`
- Added blackened iron backplates, aged brass lever/guard details, cream gauge faces, amber/red/green lift lamps, brass pressure pipes, stamped labels, rivets, and oil/scorch grime plates.
- Added validator coverage for metadata, named hierarchy, material roles, detail counts, zero colliders, zero `NavMeshObstacle` components, and no gameplay-authority components.
- Integrated the service-lift call box sidecar production brief/status docs.
- Built and packaged Windows candidate `v0.1.30`.

## Artifacts

- Executable: `Builds/Windows/v0.1.30/BrassworksBreach_v0.1.30.exe`
- Package: `Builds/WindowsPackages/v0.1.30/BrassworksBreach_v0.1.30_Windows.zip`
- Package SHA-256: `78BABF0B79472233E25AC4FB7392FCF6C25E91B281889EF02B02A035D529991E`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.30.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.30.md`
- Issue triage: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.30.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.30.md`

## Verification

Full V0 matrix passed with `V0_BUILD_MATRIX_PASS`, including route audit, scene rebuild, level validation, Windows build, packaged runtime tests, Windows package, QA packet, issue-triage packet, and candidate-readiness evidence.

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

Continue immediately with `v0.1.31`, using the next highest-impact route-safe asset promotion unless a manual QA blocker appears.
