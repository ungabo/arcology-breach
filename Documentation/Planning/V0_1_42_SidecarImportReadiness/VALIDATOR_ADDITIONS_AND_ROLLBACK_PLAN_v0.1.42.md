# V0.1.42 Validator Additions And Rollback Plan

Purpose: make the main-lane validator expansion concrete while keeping rollback small and reversible.

## Proposed `SidecarQuarantineImportValidator` Entries

Use the existing `PackageCheck` pattern. Add these three entries after the current eight package checks once `Packages/manifest.json` resolves the new packages.

```csharp
new PackageCheck(
    "Corridor Kit Set 02",
    "com.brassworks.sidecar.corridor-kit-set02",
    "Documentation~/Manifest/SCK2_CorridorKitSet02_Manifest_v0.1.41-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorStraight_4m_NorthStar.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorStraight_2m_ServiceDense.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorCorner_90_Bulkhead.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorTJunction_PipeSpine.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorCrossJunction_CompassHub.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Door_BulkheadRound_3m.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Door_PressureLock_DoubleLeaf.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_RoomWallPanel_GaugeNest.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_LightRun_AmberCaged_4m.prefab",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Materials/SCK2_MAT_PressureGreenGlass.mat",
        "Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Meshes/SCK2_MESH_NorthStar8Unit.asset"
    }),
new PackageCheck(
    "Encounter Enemy Set 02",
    "com.brassworks.sidecar.encounter-enemy-set02",
    "Documentation~/Manifest/EE02_EncounterEnemySet02_Manifest_v0.1.41-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_AshcanReclaimer_A_IdleSawScout.prefab",
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_AshcanReclaimer_B_ClawWindupTell.prefab",
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_PressureSpindle_B_NeedleThrustTell.prefab",
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GatehammerBastion_A_ShieldedIdle.prefab",
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GovernorWarden_B_BellBeaconCastTell.prefab",
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Materials/EE02_MAT_RedOverheatTell.mat",
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Materials/EE02_MAT_ReadabilityGhost.mat",
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Meshes/EE02_MESH_36ToothSawBlade.asset",
        "Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Meshes/EE02_MESH_GovernorCommandGearHalo.asset"
    }),
new PackageCheck(
    "Weapon Viewmodel Set 03",
    "com.brassworks.sidecar.weapon-viewmodel-set03",
    "Documentation~/Manifest/WVM03_WeaponViewmodelSet03_Manifest_v0.1.41-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_PressurePistol_FullAssembly_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_PressurePistol_FullAssembly_B_DualGauge.prefab",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_Scattergun_FullAssembly_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_BoltThrower_FullAssembly_A.prefab",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_GloveSilhouette_RightGrip.prefab",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_GloveSilhouette_LeftSupport.prefab",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_AmmoPressureCell_Single.prefab",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Materials/WVM03_MAT_GreenGaugeGlass.mat",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Meshes/WVM03_Mesh_GlovePalm.asset",
        "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/WeaponViewmodelSet03Identity.cs"
    })
```

If all paths above are used, the validator should grow from 8 packages and 51 assets to 11 packages and 81 assets.

## Higher-Accuracy Validator Additions

| Addition | Why it matters |
| --- | --- |
| Parse package-local manifest JSON and assert `asset_counts` for each new package | Catches partial import, stale package roots, and wrong manifest selection. |
| Assert `dependencies` is empty or explicitly approved | Prevents hidden third-party dependency creep. |
| Add a prefab safety scan for representative prefabs | Confirms no saved `Collider`, `Rigidbody`, `AudioSource`, gameplay controller, pickup, damage, objective, or transition components. |
| Add renderer-count sanity checks for showcase roots | Prevents a silent drop to blank or under-rendered showcase objects. |
| For WVM03, assert the `WeaponViewmodelSet03Identity` type compiles and appears on full assembly candidates if intended | Raises confidence that identity metadata survived package import. |
| For SCK2, assert door modules are not placed over real level transitions | Keeps quarantine visuals from implying false gameplay state. |
| For EE02, assert visual enemy prefabs are not wired to combat controllers | Keeps pose/readability assets out of AI authority. |

## Proposed Rollback Plan

1. Stop further scene generation after the first failure and identify whether the failing package is SCK2, EE02, or WVM03.
2. If Unity fails during package resolution or script compile, remove only the failed package's manifest line first. WVM03 should be isolated first if the failure is compile-related because it contains a runtime assembly.
3. Remove the matching `PackageCheck` entry from `SidecarQuarantineImportValidator`.
4. Remove only the matching `SidecarPrefabPlacement` rows and `V0LevelValidator` required names for that package.
5. Rebuild generated scenes through Unity after the code compiles.
6. Run `Project Tools/Validate Sidecar Quarantine Imports` and `Project Tools/Validate v0 Levels`.
7. Search generated scenes for the removed package name to confirm no stale package reference remains.
8. Leave the package root in `AssetPacks` unless the review decision explicitly rejects and deletes that root. Rollback for quarantine import should be reference removal, not asset-root surgery.

## Package-Specific Rollback Notes

| Package | Fastest safe rollback | Why |
| --- | --- | --- |
| CorridorKitSet02 | Remove SCK2 manifest line, validator entry, SCK2 placements, and SCK2 required names | Visual-only kit; no runtime script. |
| EncounterEnemySet02 | Remove EE02 manifest line, validator entry, EE02 placements, and EE02 required names | Visual-only enemy poses; no gameplay authority. |
| WeaponViewmodelSet03 | Remove WVM03 manifest line first if compile fails, then validator and showcase references | Contains runtime assembly and identity script, so compile rollback should happen before scene rollback. |

## Rollback Pass Criteria

- Unity compiles after the package reference is removed.
- Import validator passes with the remaining package set.
- `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- `rg "com.brassworks.sidecar.(corridor-kit-set02|encounter-enemy-set02|weapon-viewmodel-set03)" Assets/_Project/Scenes` returns no references for any package that was rolled back.
- `git status --short` shows only expected main-lane manifest, lock, validator, scene builder, level validator, and generated scene churn.
