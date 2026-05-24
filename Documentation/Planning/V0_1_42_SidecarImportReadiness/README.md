# V0.1.42 Sidecar Import Readiness

Purpose: prepare the main lane to import and review the three completed v0.1.41 sidecar package roots that are now pending v0.1.42 quarantine intake.

Target packages:

- `com.brassworks.sidecar.corridor-kit-set02`
- `com.brassworks.sidecar.encounter-enemy-set02`
- `com.brassworks.sidecar.weapon-viewmodel-set03`

Scope rule for this packet: documentation only. This pass does not edit `Packages/manifest.json`, package roots, generated scenes, gameplay scripts, shared status docs, or git history.

Baseline observed on 2026-05-24:

- The main manifest already references the eight earlier accepted sidecars.
- These three target packages are not yet referenced by `Packages/manifest.json`.
- Package-local static validation was run for all three target roots and returned pass, 0 errors, 0 warnings.

Files in this planning bundle:

- `SIDECAR_IMPORT_READINESS_PLAN_v0.1.42.md` - main-lane import sequence, batching, and readiness gates.
- `ASSET_PATH_INVENTORY_v0.1.42.md` - representative package paths for validator coverage.
- `RISK_MATRIX_AND_SHOWCASE_PLACEMENTS_v0.1.42.md` - risk matrix and proposed per-level quarantine placements.
- `VALIDATOR_ADDITIONS_AND_ROLLBACK_PLAN_v0.1.42.md` - concrete validator entries, higher-accuracy checks, and rollback sequence.

QA companion:

- `Documentation/QA/V0_1_42_SidecarImportReadiness/MAIN_LANE_VALIDATION_CHECKLIST_v0.1.42.md`
- `Documentation/QA/V0_1_42_SidecarImportReadiness/SIDECAR_IMPORT_READINESS_REVIEW_v0.1.42.json`
