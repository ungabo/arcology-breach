# V0.1.45 Sidecar Import Readiness

Generated: 2026-05-24

This docs-only packet prepares the next quarantine import wave for:

- `BrassworksBreach.LevelAtmosphereSet03`
- `BrassworksBreach.EnemyAnimationProxySet01`

It assumes the current verified main-lane baseline after v0.1.43 is `packages=11 assets=81`, and that the v0.1.44 readiness wave for ObjectivePropsSet02 plus SteamVFXSet02 is accepted before this one, raising the expected validator target to `packages=13 assets=102`.

This packet proposes 2 additional package checks and 21 additional representative asset checks. After the v0.1.44 wave and this v0.1.45 wave are both imported, the expected `SidecarQuarantineImportValidator` result is `packages=15 assets=123`.

## Files

- `SIDECAR_IMPORT_READINESS_PLAN_v0.1.45.md`
- `ASSET_PATH_INVENTORY_v0.1.45.md`
- `RISK_MATRIX_AND_SHOWCASE_PLACEMENTS_v0.1.45.md`
- `VALIDATOR_ADDITIONS_AND_ROLLBACK_PLAN_v0.1.45.md`
- `../../QA/V0_1_45_SidecarImportReadiness/MAIN_LANE_VALIDATION_CHECKLIST_v0.1.45.md`
- `../../QA/V0_1_45_SidecarImportReadiness/SIDECAR_IMPORT_READINESS_REVIEW_v0.1.45.json`

## Boundaries

This packet does not edit package roots, `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project`, generated scenes, shared status docs, or git history. Import, showcase placement, scene rebuild, and build validation remain main-lane work.

Use Unity only for render, lookdev, import validation, and scene validation. Do not use Blender for this intake.
