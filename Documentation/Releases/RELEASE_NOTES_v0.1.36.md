# Brassworks Breach v0.1.36 Release Notes

Build date: `2026-05-24`

## Headline

`v0.1.36` imports four Unity sidecar packages into the primary project and promotes them into visible, validation-covered level showcases.

## Player-Facing Changes

- Added sidecar showcase visuals across all five levels.
- Level01 now displays imported pressure-pistol, coil, gauge, and feedback-event visuals.
- Level02 through Level05 now display imported modular level-kit pieces, mechanical enemy visuals, and feedback FX markers.
- All showcase placements are presentation-only: no added colliders, rigidbodies, autonomous audio sources, or gameplay authority.

## Production Changes

- Added local package references for:
  - `com.brassworks.sidecar.steampunk-weapons`
  - `com.brassworks.sidecar.mechanical-enemies`
  - `com.brassworks.sidecar.feedback-fx-audio`
  - `com.brassworks.sidecar.steamworks-level-kit`
- Added `SidecarQuarantineImportValidator` with `SIDECAR_QUARANTINE_IMPORT_PASS` coverage.
- Completed/recovered the Steamworks Level Kit sidecar to `v0.1.39-p001`: 24 prefabs, 16 materials, 4 meshes, and 4 preview renders.
- Restarted the parallel Materials Set 01 sidecar lane after the network interruption.

## Verification

- `SIDECAR_QUARANTINE_IMPORT_PASS packages=4 assets=20`
- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_BUILD_MATRIX_PASS v0.1.36`
- `V0_WINDOWS_PACKAGE_PASS`
- `V0_WINDOWS_QA_PACKET_PASS`
- `V0_WINDOWS_ISSUE_TRIAGE_PASS`
- `V0_WINDOWS_CANDIDATE_PASS`

Package SHA-256:

`A93D7AF997A21FB333F161562900D0E6B0C7393AF6BBB200637D8B88FAB57C0E`

Next-step directive: continue immediately with the next highest-impact unfinished task.
