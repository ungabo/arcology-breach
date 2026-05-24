# V0.1.46 Validator Additions And Rollback Plan

Purpose: make the WeaponMechanismsSet04 main-lane validator expansion concrete while keeping rollback small and reversible.

## Proposed `SidecarQuarantineImportValidator` Entry

Use the existing `PackageCheck` pattern. Add this entry only after `Packages/manifest.json` resolves `com.brassworks.sidecar.weapon-mechanisms-set04`.

```csharp
new PackageCheck(
    "Weapon Mechanisms Set 04",
    "com.brassworks.sidecar.weapon-mechanisms-set04",
    "Documentation~/Manifest/WMS04_WeaponMechanismsSet04_Manifest_v0.1.45-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_PressurePistolCoil_TripleAmber_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GaugeCluster_TripleIvory_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GripAssembly_WalnutLeather_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ReceiverPlate_BrassLattice_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_MuzzleCrown_CogBrake_B.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_PressureTank_TwinUnderbarrel_B.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ValveLever_RedSafety_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_AmmoCylinder_EightCell_B.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_ScattergunPressureChamber_Quad_B.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_BoltThrowerRail_ChargedSlide_B.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_GlovedHandSilhouette_RightGrip_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Prefabs/WMS04_MaterialSwatch_MetalsAndGlass_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Materials/WMS04_MAT_AgedBrassBrushed.mat",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Materials/WMS04_MAT_TealPressureGlow.mat",
        "Packages/com.brassworks.sidecar.weapon-mechanisms-set04/Runtime/Meshes/WMS04_MESH_Gear24Ring.asset"
    })
```

If the 15 non-manifest paths above are used as loadable asset checks, this wave adds 1 package and 15 representative assets. Starting from the current v0.1.45 target of `packages=15 assets=123`, the expected post-v0.1.46 result is `packages=16 assets=138`.

## Higher-Accuracy Validator Additions

| Addition | Why it matters |
| --- | --- |
| Parse the package-local manifest JSON and assert `asset_counts.generated_prefabs=29`, `generated_materials=20`, `generated_meshes=11`, and `preview_renders=11` | Catches stale package roots, partial imports, and wrong manifest selection. |
| Parse `Runtime/Metadata/WMS04_RuntimeCatalog_v0.1.45.json` and assert every sampled prefab has `visual_only=true` | Confirms the runtime catalog agrees with quarantine-only usage. |
| Assert package dependencies are empty | Prevents a visual sidecar from pulling in unplanned runtime systems. |
| Add prefab safety scans for representative prefabs | Confirms no `Collider`, `Rigidbody`, `AudioSource`, `NavMeshAgent`, gameplay controller, pickup, damage, hitbox, projectile, weapon config, or viewmodel animation controller components are present. |
| Add name-family coverage checks | Ensures the sample includes coils, gauges, grips, receiver plates, muzzle crowns, pressure tanks, valves, cylinders, scattergun chambers, rails, gloved-hand silhouettes, materials, and meshes. |
| Add showcase root safety checks | Confirms all scene instances live under `Sidecar Quarantine Showcase - <LevelXX>` and carry no gameplay authority. |
| Add future-promotion guard checks | Prevents gloved hands, muzzle crowns, ammo cylinders, rails, and coils from being silently connected to first-person weapon systems during import. |

## Proposed Rollback Plan

1. Stop further scene generation after the first failure and identify whether the failure is package resolution, validator loadability, scene placement, or visual QA.
2. If Unity fails during package resolution or compile, remove only the `com.brassworks.sidecar.weapon-mechanisms-set04` manifest line first.
3. Remove only the WeaponMechanismsSet04 `PackageCheck` entry from `SidecarQuarantineImportValidator`.
4. Remove only WMS04 showcase placement rows and WMS04 required names from level validation.
5. Rebuild generated scenes through Unity if any WMS04 instances were created.
6. Run `Project Tools/Validate Sidecar Quarantine Imports`.
7. Run `Project Tools/Validate v0 Levels`.
8. Search generated scenes for `com.brassworks.sidecar.weapon-mechanisms-set04` and `WMS04_` to confirm no stale package references remain.
9. Leave `AssetPacks/BrassworksBreach.WeaponMechanismsSet04` intact unless a separate review explicitly rejects and deletes the sidecar root.

## Rollback Pass Criteria

- Unity compiles after the package reference is removed.
- Import validator passes with the remaining package set.
- `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- Generated scenes have no stale `WMS04_` or `com.brassworks.sidecar.weapon-mechanisms-set04` references.
- `git status --short` shows only expected main-lane manifest, lock, validator, scene builder, level validator, and generated scene changes from the import attempt.
