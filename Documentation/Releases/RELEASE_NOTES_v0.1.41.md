# Brassworks Breach v0.1.41 Release Notes

Build date: `2026-05-24`

## Headline

`v0.1.41` imports four additional accepted Unity sidecar packages into the primary project and turns them into a larger in-game visual showcase without giving those assets gameplay authority.

## Player-Facing Changes

- The sidecar showcase now includes Materials Set 01 swatches, Level Dressing Set 01 props, Mechanical Enemy Visual Set 01 candidates, and Weapon Props Set 02 candidates.
- Level01 now shows upgraded pressure-pistol component candidates, material swatches, wall lamps, and gauge panels.
- Level02 through Level05 now show additional pipe junctions, tanks, valve clusters, service panels, enemy visual candidates, scattergun/weapon props, racks, and material swatches.
- All new showcase content is presentation-only: no added gameplay colliders, rigidbodies, autonomous audio sources, or live gameplay authority.

## Production Changes

- Added local package references for:
  - `com.brassworks.sidecar.materials-set01`
  - `com.brassworks.sidecar.level-dressing-set01`
  - `com.brassworks.sidecar.mechanical-enemy-visual-set01`
  - `com.brassworks.sidecar.weapon-props-set02`
- Expanded `SidecarQuarantineImportValidator` from four to eight packages.
- Raised representative sidecar import coverage to `packages=8 assets=51`.
- Added material swatch generation for Materials Set 01, since that package has no prefabs.
- Expanded level validation so each level requires new showcase assets and material swatches while preserving no-physics/no-audio safety checks.
- Added the sidecar import validator to the standard Windows matrix and candidate-readiness evidence chain.
- Accepted the sidecar import-readiness packet from the parallel worker as planning/QA evidence for the import batch.

## Verification

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `SIDECAR_QUARANTINE_IMPORT_PASS packages=8 assets=51`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_GAMEPLAY_FEEDBACK_PASS`
- `V0_WORLD_LABEL_READABILITY_PASS`
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`

Package SHA-256:

`0130AAB1AAB43ED3EC9BEEBB5365034BE9F0FAF035993BFB5C2185E3ED0BE6FE`

Next-step directive: continue immediately with the next highest-impact unfinished task.
