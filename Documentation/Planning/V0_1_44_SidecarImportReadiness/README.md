# V0.1.44 Sidecar Import Readiness

Purpose: prepare the main lane to import and review the two newly completed sidecar package roots pending v0.1.44 quarantine intake.

Target packages:

- `com.brassworks.sidecar.objective-props-set02`
- `com.brassworks.sidecar.steam-vfx-set02`

Scope rule for this packet: documentation only. This pass does not edit package roots, `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project`, scenes, shared status docs, or git history.

Baseline observed on 2026-05-24:

- The main manifest already references the current eleven accepted sidecars from the prior intake state.
- These two target packages are not referenced by `Packages/manifest.json`.
- `SidecarQuarantineImportValidator` currently contains 11 package checks and 81 representative asset checks.
- Package-local static validation was rerun for both target roots and returned pass, 0 errors, 0 warnings.

Files in this planning bundle:

- `SIDECAR_IMPORT_READINESS_PLAN_v0.1.44.md` - main-lane import sequence, batching, readiness gates, and expected validator totals.
- `ASSET_PATH_INVENTORY_v0.1.44.md` - representative package paths for validator coverage.
- `RISK_MATRIX_AND_SHOWCASE_PLACEMENTS_v0.1.44.md` - package risks and proposed per-level quarantine placements.
- `VALIDATOR_ADDITIONS_AND_ROLLBACK_PLAN_v0.1.44.md` - concrete validator entries, higher-accuracy checks, and rollback sequence.

QA companion:

- `Documentation/QA/V0_1_44_SidecarImportReadiness/MAIN_LANE_VALIDATION_CHECKLIST_v0.1.44.md`
- `Documentation/QA/V0_1_44_SidecarImportReadiness/SIDECAR_IMPORT_READINESS_REVIEW_v0.1.44.json`
