# V0.1.45 Validator Additions And Rollback Plan

Purpose: make the main-lane validator expansion concrete while keeping rollback small and reversible.

## Proposed `SidecarQuarantineImportValidator` Entries

Use the existing `PackageCheck` pattern. Add these two entries after the v0.1.44 ObjectivePropsSet02 and SteamVFXSet02 entries once `Packages/manifest.json` resolves the new packages.

```csharp
new PackageCheck(
    "Level Atmosphere Set 03",
    "com.brassworks.sidecar.level-atmosphere-set03",
    "Documentation~/Manifest/SCLA_LevelAtmosphereSet03_Manifest_v0.1.44-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_SteamPipeCluster_WallLeaker_A.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_SteamPipeCluster_CornerBleed.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_PressureLamp_WallCaged_A.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_WallGrimePanel_OilStreaks.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_HangingChains_TripleSlack.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_FloorDrainCover_LongGutter.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_WarningGauge_TripleRack.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_OverheadPipeCanopy_ValveRun.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_DenseAmbienceCombo_CorridorBite.prefab",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Materials/SCLA_MAT_AmberLampGlass.mat",
        "Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Meshes/SCLA_MESH_SteamWispUnit.asset"
    }),
new PackageCheck(
    "Enemy Animation Proxy Set 01",
    "com.brassworks.sidecar.enemy-animation-proxy-set01",
    "Documentation~/Manifest/EAP01_EnemyAnimationProxySet01_Manifest_v0.1.44-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_ScrapperAshcan_01_IdleBrace.prefab",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_ScrapperAshcan_03_SawLunge.prefab",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_LancerPressureSpindle_01_AimLine.prefab",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_LancerPressureSpindle_03_ThrustPeak.prefab",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_BulwarkGatehammer_02_HammerRaise.prefab",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_WardenGovernor_02_SignalRaise.prefab",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Materials/EAP01_MAT_FurnaceOrangeGlow.mat",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Meshes/EAP01_MESH_CommandGearHalo.asset",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/AnimationClips/EAP01_CLIP_ScrapperAshcan_PoseProxyOnly.anim",
        "Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/AnimationClips/EAP01_CLIP_WardenGovernor_PoseProxyOnly.anim"
    })
```

If all paths above are used, this wave adds 2 packages and 21 representative asset checks. Starting from the v0.1.43 baseline of 11 packages and 81 assets, then applying the v0.1.44 proposed wave of 2 packages and 21 assets, the expected post-v0.1.45 target is 15 packages and 123 assets.

## Higher-Accuracy Validator Additions

| Addition | Why it matters |
| --- | --- |
| Parse package-local manifest JSON and assert `asset_counts` for each new package | Catches partial import, stale package roots, wrong manifest selection, and package version drift. |
| Assert LevelAtmosphereSet03 dependency list is empty | Keeps the atmosphere pack from pulling in unplanned render, audio, or lighting dependencies. |
| Assert EnemyAnimationProxySet01 runtime scripts count is 0 | Confirms the proxy pack remains visual-only and cannot own gameplay authority. |
| Add prefab safety scans for representative prefabs | Confirms no saved `Collider`, `Rigidbody`, `AudioSource`, `NavMeshAgent`, gameplay controller, pickup, damage, hitbox, objective, door, lift, bridge, or transition components. |
| Add placeholder clip checks for EnemyAnimationProxySet01 | Confirms clip assets import but remain pose-proxy discussion assets, not gameplay animation timing. |
| Add renderer-count sanity checks for showcase roots | Prevents silent blank imports and catches overly dense quarantine placements. |
| Assert atmosphere props stay off route blockers and combat sightlines | Keeps dense atmosphere from degrading playability or readability. |
| Assert enemy proxies are placed as labeled quarantine visuals | Prevents the player from mistaking static proxies for live combatants. |

## Proposed Rollback Plan

1. Stop further scene generation after the first failure and identify whether the failing package is SCLA or EAP01.
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
| LevelAtmosphereSet03 | Remove SCLA manifest line, validator entry, SCLA placements, and SCLA required names | Visual-only atmosphere props; lighting, collision, audio, and navigation authority stay out of package. |
| EnemyAnimationProxySet01 | Remove EAP01 manifest line, validator entry, EAP01 placements, and EAP01 required names | Visual-only pose proxies; rigging, AI, damage, hitboxes, and timing remain future promotion work. |

## Rollback Pass Criteria

- Unity compiles after the package reference is removed.
- Import validator passes with the remaining package set.
- `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- `rg "com.brassworks.sidecar.(level-atmosphere-set03|enemy-animation-proxy-set01)" Assets/_Project/Scenes` returns no references for any package that was rolled back.
- `git status --short` shows only expected main-lane manifest, lock, validator, scene builder, level validator, and generated scene churn.
