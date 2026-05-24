# Brassworks Breach v0.1.43 Release Notes

Build date: `2026-05-24`

## Headline

`v0.1.43` imports the second accepted Unity sidecar wave into the primary project and expands the in-game quarantine showcase with corridor, encounter-enemy, and first-person weapon-viewmodel candidates.

## Player-Facing Changes

- The five-level route now displays additional steampunk corridor modules, bulkhead doors, pressure-lock doors, room gauge nests, amber caged light runs, and pressure-green glass material swatches.
- Encounter showcase content now includes Ashcan Reclaimer, Pressure Spindle, Gatehammer Bastion, and Governor Warden visual/readability candidates.
- Weapon showcase content now includes pressure-pistol, scattergun, bolt-thrower, glove, and pressure-cell viewmodel candidates for first-person art direction review.
- All new showcase content remains presentation-only: it adds no gameplay colliders, rigidbodies, autonomous audio sources, or live gameplay authority.

## Production Changes

- Added local package references for:
  - `com.brassworks.sidecar.corridor-kit-set02`
  - `com.brassworks.sidecar.encounter-enemy-set02`
  - `com.brassworks.sidecar.weapon-viewmodel-set03`
- Expanded `SidecarQuarantineImportValidator` from eight to eleven packages.
- Raised representative sidecar import coverage to `packages=11 assets=81`.
- Added visual-only sidecar showcase placements and material swatches for the three imported packages across all five generated gameplay levels.
- Raised scene validation thresholds and required-name checks so the new showcase families are proven present without changing route physics.
- Used the v0.1.42 import-readiness packet as the planning and QA basis for the import batch.

## Verification

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `SIDECAR_QUARANTINE_IMPORT_PASS packages=11 assets=81`
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

`3640B5D98785CADC26C03A1CD979E9A0882BD2492C1AF6DCDED527AA1C7FFE02`

Next-step directive: continue immediately with the next highest-impact unfinished task.
