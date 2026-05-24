# V0.1.46 Validator Additions And Rollback Plan

Purpose: make the main-lane validator expansion concrete while keeping rollback small and reversible.

## Proposed `SidecarQuarantineImportValidator` Entry

Use the existing `PackageCheck` pattern. Add this entry after the v0.1.45 sidecar entries once `Packages/manifest.json` resolves `com.brassworks.sidecar.room-setpiece-kit04`.

```csharp
new PackageCheck(
    "Room Setpiece Kit 04",
    "com.brassworks.sidecar.room-setpiece-kit04",
    "Documentation~/Manifest/RSK04_RoomSetpieceKit04_Manifest_v0.1.45-p001.json",
    new[]
    {
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_BoilerChamberWallBay_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_PressureVaultDoorAlcove_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_CatwalkBalconyModule_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_RegulatorCoreMachinery_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_PipeGalleryCeilingCluster_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_ServiceStairSilhouette_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_FurnaceControlWall_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_BrassFloorTrimThreshold_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_LargeWarningGaugeWall_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_RoomCornerClutterCluster_A.prefab",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Materials/RSK04_MAT_AgedBrass.mat",
        "Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Meshes/RSK04_MESH_Gear24Unit.asset"
    })
```

If all paths above are used, this wave adds 1 package and 12 representative asset checks. Starting from the expected post-v0.1.45 target of 15 packages and 123 assets, the expected post-v0.1.46 target is 16 packages and 135 assets.

## Higher-Accuracy Validator Additions

| Addition | Why it matters |
| --- | --- |
| Parse the package-local manifest and assert `asset_counts.generated_prefabs == 30`, `generated_materials == 18`, `generated_meshes == 10`, and `preview_renders == 12` | Catches partial imports, stale package roots, wrong manifest selection, or version drift. |
| Parse the runtime catalog and assert all `prefabs[*].visual_only == true` | Confirms the package metadata continues to declare visual-only authority. |
| Assert the package dependency list is empty | Prevents a room-dressing package from pulling unplanned runtime, render, audio, physics, or gameplay dependencies. |
| Add prefab safety scans for the 10 representative prefab families | Confirms no saved `Collider`, `Rigidbody`, `AudioSource`, `NavMeshAgent`, gameplay controller, pickup, damage, hitbox, objective, door, lift, bridge, or transition components. |
| Add bounds sanity checks from the runtime catalog | Flags accidental oversized placements before they block routes or camera views. |
| Add renderer-count sanity checks for showcase roots | Prevents silent blank imports and catches overly dense quarantine placements. |
| Assert showcase roots have zero gameplay authority components | Keeps the import visual-only after scene builder placement. |
| Assert door, threshold, catwalk, and stair families are not bound to gameplay route logic | Prevents visual setpieces from becoming accidental doors, blockers, walkable routes, or transitions. |

## Proposed Rollback Plan

1. Stop further scene generation after the first failure and identify whether the failure is package resolution, validator loadability, scene placement, or level validation.
2. If Unity fails during package resolution or compile, remove only the `com.brassworks.sidecar.room-setpiece-kit04` line from `Packages/manifest.json`.
3. Remove the matching `PackageCheck` entry from `SidecarQuarantineImportValidator`.
4. Remove only the matching RoomSetpieceKit04 showcase placements and `V0LevelValidator` required names.
5. Rebuild generated scenes through Unity after the code compiles.
6. Run `Project Tools/Validate Sidecar Quarantine Imports` and `Project Tools/Validate v0 Levels`.
7. Search generated scenes for `com.brassworks.sidecar.room-setpiece-kit04` and `RSK04_` to confirm no stale package references remain.
8. Leave `AssetPacks/BrassworksBreach.RoomSetpieceKit04` intact unless the review decision explicitly rejects and deletes the sidecar package root. Rollback for quarantine import should be reference removal, not package-root surgery.

## Rollback Pass Criteria

- Unity compiles after the package reference is removed.
- Import validator passes with the remaining package set.
- `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- Generated scenes contain no stale `com.brassworks.sidecar.room-setpiece-kit04` references after rollback.
- `git status --short` shows only expected main-lane manifest, lock, validator, scene builder, level validator, and generated scene churn.
