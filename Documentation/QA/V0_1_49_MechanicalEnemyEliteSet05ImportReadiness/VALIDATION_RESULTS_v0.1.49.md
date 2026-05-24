# Validation Results v0.1.49

Machine-readable Unity result:

`Documentation/AssetProduction/V0_1_49_MechanicalEnemyEliteSet05/unity_validation_report_v0.1.49.json`

## Results

- Unity batch generation and validation: pass.
- Prefabs: 25 of 25 expected.
- Materials: 18 of 18 expected.
- Meshes: 12 of 12 expected.
- Preview PNGs: 50, above the 32 minimum.
- Colliders in prefabs: 0.
- Animator components in prefabs: 0.
- Rigidbody components in prefabs: 0.
- Runtime MonoBehaviours in prefabs: 0.
- JSON parse: pass for `package.json`, package-local validation manifest, package-local sidecar manifest, and Unity validation report.
- Runtime authority grep: pass for animator/controller/script/rigidbody/collider/nav/damage/health authority terms in `Runtime`.
- Scope grep: pass, 190 files found under the five assigned roots and no files outside those roots.
- Existing sidecar validator support check: not run because `SidecarQuarantineImportValidator.cs` does not yet contain `MEES05` or `com.brassworks.sidecar.mechanical-enemy-elite-set05`.
