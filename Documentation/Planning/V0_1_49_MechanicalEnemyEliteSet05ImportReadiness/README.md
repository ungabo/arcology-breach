# Mechanical Enemy Elite Set 05 Import Readiness

This plan keeps Mechanical Enemy Elite Set 05 isolated until a future main-lane art integration pass deliberately promotes it.

## Manifest Step

Add the local package reference only in a quarantine branch or validation project:

```json
"com.brassworks.sidecar.mechanical-enemy-elite-set05": "file:../AssetPacks/BrassworksBreach.MechanicalEnemyEliteSet05"
```

The package-local validation project already uses:

```json
"com.brassworks.sidecar.mechanical-enemy-elite-set05": "file:../.."
```

## Lock Step

1. Open Unity once so Package Manager resolves the local sidecar.
2. Confirm `Packages/packages-lock.json` records `com.brassworks.sidecar.mechanical-enemy-elite-set05` as a local/file dependency.
3. Do not commit unrelated lock-file churn.

## Validator Step

Run the existing sidecar quarantine validator only after it is intentionally extended to include:

- Package name: `com.brassworks.sidecar.mechanical-enemy-elite-set05`
- Manifest path: `Documentation~/Manifest/MEES05_MechanicalEnemyEliteSet05_Manifest_v0.1.49-p001.json`
- Sample assets from `Runtime/Prefabs`, `Runtime/Materials`, and `Runtime/Meshes`

This task does not edit the main validator because it is outside the assigned roots.

## Scene Placement Step

1. Create a quarantine review scene, not a gameplay scene.
2. Place one prefab from each family at floor height on a 2 m grid.
3. Inspect weak points, furnace eyes, saw arms, lance rails, boiler shields, and command halos.
4. Do not add colliders, AI scripts, damage scripts, nav components, animation clips, or animator controllers.
5. If approved, duplicate selected visual child groups under a future placeholder replacement prefab while preserving gameplay authority on the existing gameplay root.
