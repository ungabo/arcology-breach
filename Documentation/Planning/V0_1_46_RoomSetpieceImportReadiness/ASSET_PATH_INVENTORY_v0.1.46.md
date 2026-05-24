# V0.1.46 Asset Path Inventory

Purpose: list representative `RoomSetpieceKit04` package assets the main lane should validate after adding the package to `Packages/manifest.json`. Paths use Package Manager resolution form because `SidecarQuarantineImportValidator` loads assets through `AssetDatabase`.

## Package Counts

| Package | Prefabs | Materials | Meshes | Runtime scripts | Colliders | Rigidbodies | Audio sources | Preview renders | Notes |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| RoomSetpieceKit04 | 30 | 18 | 10 | 0 | 0 | 0 | 0 | 12 | Ten room-scale visual setpiece families, each with A/B/C variants. |

## Package Evidence Paths

| Category | Path | Validation reason |
| --- | --- | --- |
| Package manifest | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Documentation~/Manifest/RSK04_RoomSetpieceKit04_Manifest_v0.1.45-p001.json` | Package-local evidence records counts, visual-only authority, import smoke status, risks, and rollback path. |
| Runtime catalog | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Metadata/RSK04_RuntimeCatalog_v0.1.45.json` | Runtime metadata lists every generated prefab, material, mesh, bounds estimate, and `visual_only` flag. |

## Representative Validator Asset Paths

Use one prefab per family plus one material and one mesh for fast import coverage. The package manifest path is validated by the `PackageCheck` manifest field and is not counted as a representative asset path below.

| Category | Path | Validation reason |
| --- | --- | --- |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_BoilerChamberWallBay_A.prefab` | Boiler wall bay family coverage; wide wall-mounted room dressing. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_PressureVaultDoorAlcove_A.prefab` | Vault-door alcove family coverage; must not be used as gameplay door authority. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_CatwalkBalconyModule_A.prefab` | Balcony silhouette coverage; must stay visual-only and off traversal authority. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_RegulatorCoreMachinery_A.prefab` | Large machinery family coverage; checks dense center-mass silhouette and materials. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_PipeGalleryCeilingCluster_A.prefab` | Ceiling cluster coverage; checks overhead density and headroom readability. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_ServiceStairSilhouette_A.prefab` | Stair silhouette coverage; must not imply walkable stairs, route changes, or collision. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_FurnaceControlWall_A.prefab` | Furnace wall family coverage; checks orange glow material without gameplay or audio authority. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_BrassFloorTrimThreshold_A.prefab` | Threshold/floor trim coverage; must not block or redefine route boundaries. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_LargeWarningGaugeWall_A.prefab` | Warning gauge wall coverage; checks gauge readability without objective or hazard logic. |
| Prefab | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Prefabs/RSK04_RoomCornerClutterCluster_A.prefab` | Corner clutter coverage; route-edge clutter risk sample. |
| Material | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Materials/RSK04_MAT_AgedBrass.mat` | Baseline brass material import and shader fallback coverage. |
| Mesh | `Packages/com.brassworks.sidecar.room-setpiece-kit04/Runtime/Meshes/RSK04_MESH_Gear24Unit.asset` | Reusable mesh import coverage for mechanical detail. |

## Validator Asset Count Recommendation

If the main lane uses all non-manifest representative assets above as `RequiredAssetPaths`, this wave adds:

- RoomSetpieceKit04: 12 representative asset checks.

Starting from the expected post-v0.1.45 target of 15 packages and 123 representative assets, this wave should raise `SidecarQuarantineImportValidator` to 16 packages and 135 representative assets.
