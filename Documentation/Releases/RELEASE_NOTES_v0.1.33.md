# Brassworks Breach v0.1.33

Generated: `2026-05-24 12:44 -04:00`

## Release Focus

`v0.1.33` is the first ambitious milestone-batched environment pass after the process change. It promotes a 10-family threshold-and-route dressing set across all five playable levels instead of shipping another one-prop release.

## Changes

- Added `ThresholdRouteDressingPrototype` metadata for Unity-owned generated route dressing.
- Added 50 new collider-free visual placements across Level01 through Level05.
- Promoted 10 route-dressing families:
  - `PistonDoorBracePrototype`
  - `PipeClampCouplerSetPrototype`
  - `OilSootGrimePanelSetPrototype`
  - `AmberIndicatorPlatePrototype`
  - `BrassThresholdKickPlatePrototype`
  - `RivetedPatchRepairPlatePrototype`
  - `PressureSealGasketRingPrototype`
  - `RouteReturnPipeMarkerPrototype`
  - `SteamVentResidueCollarPrototype`
  - `HoistChainAnchorPlatePrototype`
- Added threshold braces, pipe clamps, soot panels, amber route plates, brass kick plates, wall repair plates, gasket seals, return-flow markers, vent residue collars, hoist anchor plates, rivets, grime, and safe-hoist signal ticks around key gates, pressure valves, lifts, hoists, and route-return sightlines.
- Added validator coverage for promotion version, batch ID, component family, placement role, visual-only gameplay authority, required named hierarchy, material roles, zero colliders, zero `NavMeshObstacle` components, and no pickup/objective/interaction/route-authority components.
- Integrated the side-agent threshold-route dressing plan and level-density placement plan into the main scene-generation lane.
- Built and packaged Windows candidate `v0.1.33`.

## Artifacts

- Executable: `Builds/Windows/v0.1.33/BrassworksBreach_v0.1.33.exe`
- Package: `Builds/WindowsPackages/v0.1.33/BrassworksBreach_v0.1.33_Windows.zip`
- Package SHA-256: `8E62D8C37E053E8F3F7DE75B22598D7C4CC13C286DA7EBCFE01EB67F9AF19E7A`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.33.md`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.33.md`
- Issue triage: `Documentation/QA/WindowsRouteQA/ISSUE_TRIAGE_v0.1.33.md`
- Candidate readiness: `Documentation/Releases/CandidateReadiness/CANDIDATE_READINESS_v0.1.33.md`

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

Continue the large-batch cadence for `v0.1.34`: combine staged weapon/prop improvements, enemy readability upgrades, and level-density polish into one visible gameplay/art leap, with side agents preparing disjoint packages while the main Unity lane owns integration and verification.
