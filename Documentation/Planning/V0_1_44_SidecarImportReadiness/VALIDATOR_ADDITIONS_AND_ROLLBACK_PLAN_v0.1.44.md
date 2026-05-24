# V0.1.44 Validator Additions And Rollback Plan

Purpose: make the main-lane validator expansion concrete while keeping rollback small and reversible.

## Proposed `SidecarQuarantineImportValidator` Entries

Use the existing `PackageCheck` pattern. Add these two entries after the current 11 package checks once `Packages/manifest.json` resolves the new packages.

```csharp
new PackageCheck(
    "Objective Props Set 02",
    "com.brassworks.sidecar.objective-props-set02",
    "Documentation~/Manifest/OPS02_ObjectivePropsSet02_Manifest_v0.1.42-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_KeyedLock_TriGearVault.prefab",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_KeyedLock_RuneCogDoorSocket.prefab",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_ValvePanel_TwinPressurePuzzle.prefab",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_LiftCallStation_BrassCageUpDown.prefab",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_PressureRegulator_RedlineGovernor.prefab",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_SecretCache_FloorGearSafe.prefab",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_Actuator_BridgeThrowLever.prefab",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_GovernorOverride_BossKillSwitch.prefab",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Materials/OPS02_MAT_RedOverrideEnamel.mat",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Meshes/OPS02_Mesh_Gear18ToothUnit.asset",
        "Packages/com.brassworks.sidecar.objective-props-set02/Runtime/ObjectivePropsSet02Identity.cs"
    }),
new PackageCheck(
    "Steam VFX Set 02",
    "com.brassworks.sidecar.steam-vfx-set02",
    "Documentation~/Manifest/BBSVFX02_SteamVFXSet02_Manifest_v0.1.42-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SteamVent_FloorBurst.prefab",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SteamVent_WallJet.prefab",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_PressureLeak_RuptureCone.prefab",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_MuzzleFlash_PistolBoiler.prefab",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SparkRicochet_WallHit.prefab",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_FurnaceBlast_DoorBelch.prefab",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_BossPhase_GovernorOvercrank.prefab",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Materials/BBSVFX02_MAT_SteamDense.mat",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Meshes/BBSVFX02_MESH_RadialBurst_16.asset",
        "Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Scripts/SteamVfxSet02Identity.cs"
    })
```

If all paths above are used, the validator should grow from 11 packages and 81 assets to 13 packages and 102 assets.

## Higher-Accuracy Validator Additions

| Addition | Why it matters |
| --- | --- |
| Parse package-local manifest JSON and assert `asset_counts` for each new package | Catches partial import, stale package roots, and wrong manifest selection. |
| Assert dependencies are empty or explicitly approved | OPS02 declares package-side dependency notes in its manifest; SteamVFXSet02 declares no package dependencies. Main-lane dependency creep should stay visible. |
| Add prefab safety scans for representative prefabs | Confirms no saved `Collider`, `Rigidbody`, `AudioSource`, gameplay controller, pickup, damage, objective, door, lift, bridge, or transition components. |
| Add particle-system safety scans for SteamVFXSet02 | Confirms no particle collision, trigger, or external-force modules are enabled without gameplay ownership. |
| Add renderer/particle-count sanity checks for showcase roots | Prevents silent blank imports and catches overly dense quarantine placements. |
| Assert identity scripts compile and are passive | Confirms `ObjectivePropsSet02Identity` and `SteamVfxSet02Identity` survive import without introducing authority. |
| Assert OPS02 props are not placed over real interactables | Keeps visual lock, lift, actuator, and boss-control language from implying false gameplay state. |

## Proposed Rollback Plan

1. Stop further scene generation after the first failure and identify whether the failing package is OPS02 or BBSVFX02.
2. If Unity fails during package resolution or script compile, remove only the failed package's manifest line first.
3. Remove the matching `PackageCheck` entry from `SidecarQuarantineImportValidator`.
4. Remove only the matching `SidecarPrefabPlacement` rows and `V0LevelValidator` required names for that package.
5. Rebuild generated scenes through Unity after the code compiles.
6. Run `Project Tools/Validate Sidecar Quarantine Imports` and `Project Tools/Validate v0 Levels`.
7. Search generated scenes for the removed package name to confirm no stale package reference remains.
8. Leave the package root in `AssetPacks` unless the review decision explicitly rejects and deletes that root. Rollback for quarantine import should be reference removal, not asset-root surgery.

## Package-Specific Rollback Notes

| Package | Fastest safe rollback | Why |
| --- | --- | --- |
| ObjectivePropsSet02 | Remove OPS02 manifest line, validator entry, OPS02 placements, and OPS02 required names | Visual-only objective props; interaction authority stays out of package. |
| SteamVFXSet02 | Remove BBSVFX02 manifest line, validator entry, BBSVFX02 placements, and BBSVFX02 required names | Visual-only VFX; particle tuning and socket binding are promotion work. |

## Rollback Pass Criteria

- Unity compiles after the package reference is removed.
- Import validator passes with the remaining package set.
- `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- `rg "com.brassworks.sidecar.(objective-props-set02|steam-vfx-set02)" Assets/_Project/Scenes` returns no references for any package that was rolled back.
- `git status --short` shows only expected main-lane manifest, lock, validator, scene builder, level validator, and generated scene churn.
