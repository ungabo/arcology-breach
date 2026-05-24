# V0.1.44 Asset Path Inventory

Purpose: list representative package assets the main lane should validate after adding the two target packages to `Packages/manifest.json`. Paths use Package Manager resolution form because `SidecarQuarantineImportValidator` loads assets through `AssetDatabase`.

## Package Counts From Manifests And File Inspection

| Package | Prefabs | Materials | Meshes | Runtime scripts | Textures | Preview renders | Notes |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| ObjectivePropsSet02 | 24 | 17 | 11 | 1 | 0 | 25 | Keyed locks, valve panels, lift call stations, pressure regulators, secret caches, actuators, and governor override devices. |
| SteamVFXSet02 | 20 | 16 | 8 | 1 | 0 | 2 | Visual-only particle and mesh VFX for steam vents, pressure leaks, sparks, muzzle flashes, furnace bursts, valve puffs, and boss phase accents. |

## ObjectivePropsSet02 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.objective-props-set02/Documentation~/Manifest/OPS02_ObjectivePropsSet02_Manifest_v0.1.42-p001.json` | Package-local evidence exists and records visual-only runtime safety. |
| Prefab | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_KeyedLock_TriGearVault.prefab` | Keyed lock readability and gear silhouette baseline. |
| Prefab | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_KeyedLock_RuneCogDoorSocket.prefab` | Door-socket objective language without door authority. |
| Prefab | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_ValvePanel_TwinPressurePuzzle.prefab` | Puzzle panel and valve grouping coverage. |
| Prefab | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_LiftCallStation_BrassCageUpDown.prefab` | Lift-call silhouette and prompt-surface coverage. |
| Prefab | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_PressureRegulator_RedlineGovernor.prefab` | Redline pressure read and gauge coverage. |
| Prefab | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_SecretCache_FloorGearSafe.prefab` | Secret-cache scale and floor-adjacent readability. |
| Prefab | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_Actuator_BridgeThrowLever.prefab` | Bridge/door actuator language without gameplay state. |
| Prefab | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Prefabs/BB_OPS02_GovernorOverride_BossKillSwitch.prefab` | Final-level override device and danger color coverage. |
| Material | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Materials/OPS02_MAT_RedOverrideEnamel.mat` | High-signal objective/danger material check. |
| Mesh | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/Meshes/OPS02_Mesh_Gear18ToothUnit.asset` | Custom gear mesh coverage. |
| Runtime script | `Packages/com.brassworks.sidecar.objective-props-set02/Runtime/ObjectivePropsSet02Identity.cs` | Confirms passive identity metadata script imports. |

## SteamVFXSet02 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.steam-vfx-set02/Documentation~/Manifest/BBSVFX02_SteamVFXSet02_Manifest_v0.1.42-p001.json` | Package-local evidence exists and records visual-only VFX counts. |
| Prefab | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SteamVent_FloorBurst.prefab` | Floor vent burst baseline for route-edge placement. |
| Prefab | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SteamVent_WallJet.prefab` | Wall jet orientation and occlusion risk coverage. |
| Prefab | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_PressureLeak_RuptureCone.prefab` | Large pressure leak scale/timing candidate. |
| Prefab | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_MuzzleFlash_PistolBoiler.prefab` | Weapon socket VFX candidate coverage. |
| Prefab | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_SparkRicochet_WallHit.prefab` | Hit-feedback spark candidate coverage. |
| Prefab | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_FurnaceBlast_DoorBelch.prefab` | Furnace burst readability and brightness coverage. |
| Prefab | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Prefabs/BBSVFX02_BossPhase_GovernorOvercrank.prefab` | Boss phase accent coverage. |
| Material | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Materials/BBSVFX02_MAT_SteamDense.mat` | Transparent steam material check. |
| Mesh | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Meshes/BBSVFX02_MESH_RadialBurst_16.asset` | Radial burst mesh coverage. |
| Runtime script | `Packages/com.brassworks.sidecar.steam-vfx-set02/Runtime/Scripts/SteamVfxSet02Identity.cs` | Confirms passive identity metadata script imports. |

## Validator Asset Count Recommendation

If the main lane uses all representative assets above as `RequiredAssetPaths`, the new additions contribute 21 loadable asset checks across two packages. Combined with the current 81 checks across 11 packages, `SidecarQuarantineImportValidator` should report 13 packages and 102 representative assets after both new package entries are active.
