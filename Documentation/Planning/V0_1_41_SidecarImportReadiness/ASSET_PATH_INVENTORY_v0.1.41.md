# V0.1.41 Sidecar Asset Path Inventory

Purpose: list representative package assets the main lane should validate after adding the four accepted packages to `Packages/manifest.json`. Paths use Package Manager resolution form because `SidecarQuarantineImportValidator` loads assets through `AssetDatabase`.

## Package Counts From Manifests

| Package | Prefabs | Materials | Meshes | Textures | Preview renders | Notes |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| MaterialsSet01 | 0 | 16 | 0 | 48 | 18 | 256x256 albedo, normal, and mask textures. |
| LevelDressingSet01 | 30 | 16 | 5 | 0 | 4 | Visual-only dense corridor and room props. |
| MechanicalEnemyVisualSet01 | 13 | 15 | 8 | 0 | 19 | Visual-only enemy silhouettes; manifest records zero colliders. |
| WeaponPropsSet02 | 16 | 12 | 4 | 0 | 17 | Weapon and interactable prop lookdev candidates; no gameplay binding. |

## MaterialsSet01 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.materials-set01/Documentation~/Manifest/MSET01_MaterialsSet01_Manifest_v0.1.39-p001.json` | Package-local evidence exists. |
| Material | `Packages/com.brassworks.sidecar.materials-set01/Runtime/Materials/MSET01_MAT_AgedBrass.mat` | Core brass response for weapons, trim, valves. |
| Material | `Packages/com.brassworks.sidecar.materials-set01/Runtime/Materials/MSET01_MAT_OilyBlackenedIron.mat` | Heavy metal response for enemies, doors, rails. |
| Material | `Packages/com.brassworks.sidecar.materials-set01/Runtime/Materials/MSET01_MAT_RivetedWallPlate.mat` | Corridor paneling material. |
| Material | `Packages/com.brassworks.sidecar.materials-set01/Runtime/Materials/MSET01_MAT_PressureGaugeGlass.mat` | Transparent/glossy gauge material. |
| Texture | `Packages/com.brassworks.sidecar.materials-set01/Runtime/Textures/Albedo/MSET01_MAT_AgedBrass_ALB.png` | Albedo import and texture binding source. |
| Texture | `Packages/com.brassworks.sidecar.materials-set01/Runtime/Textures/Normal/MSET01_MAT_AgedBrass_NRM.png` | Normal-map role should import as a normal texture. |
| Texture | `Packages/com.brassworks.sidecar.materials-set01/Runtime/Textures/Mask/MSET01_MAT_AgedBrass_MSK.png` | Mask import and packed texture role. |

## LevelDressingSet01 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.level-dressing-set01/Documentation~/Manifest/SCLD_LevelDressingSet01_Manifest_v0.1.40-p001.json` | Package-local evidence exists. |
| Prefab | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_RivetedTrimPlate_2m.prefab` | Wall/floor trim baseline. |
| Prefab | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_PipeJunction_T_2m.prefab` | Pipe routing kit coverage. |
| Prefab | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_PressureTank_FloorLarge.prefab` | Large floor dressing and silhouette scale. |
| Prefab | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_ValveCluster_Floor_2m.prefab` | Objective-adjacent valve cluster shape. |
| Prefab | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_CagedLamp_Wall.prefab` | Practical-light visual candidate. |
| Prefab | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_SootGrimeDecal_Wide.prefab` | Decal plane risk coverage. |
| Prefab | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Prefabs/SCLD_ServicePanel_Floor_1x2m.prefab` | Floor dressing and path readability. |
| Material | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Materials/SCLD_MAT_AgedBrass.mat` | Shared brass material proxy. |
| Mesh | `Packages/com.brassworks.sidecar.level-dressing-set01/Runtime/Meshes/SCLD_MESH_BoxUnit.asset` | Reusable procedural mesh load check. |

## MechanicalEnemyVisualSet01 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Documentation~/Manifest/MEV01_MechanicalEnemyVisualSet01_Manifest_v0.1.40-p001.json` | Package-local evidence exists. |
| Prefab | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_SawScrapper_A_BoilerSaw.prefab` | Low fast melee silhouette. |
| Prefab | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_RivetLancer_B_RailLance.prefab` | Tall ranged/lancer silhouette. |
| Prefab | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_BulwarkFurnace_A_ShieldBoiler.prefab` | Large blocker silhouette. |
| Prefab | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_BellowsSupport_B_PressureNode.prefab` | Support-node silhouette. |
| Prefab | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Prefabs/MEV01_WardenOverseer_A_TallWarden.prefab` | Boss/elite silhouette. |
| Material | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Materials/MEV01_MAT_FurnaceOrangeGlow.mat` | Emissive/readability material. |
| Mesh | `Packages/com.brassworks.sidecar.mechanical-enemy-visual-set01/Runtime/Meshes/MEV01_MESH_28ToothSawBlade.asset` | High-signal custom mesh load check. |

## WeaponPropsSet02 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.weapon-props-set02/Documentation~/Manifest/WPS02_WeaponPropsSet02_Manifest_v0.1.40-p001.json` | Package-local evidence exists. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_PressurePistol_Frame_A.prefab` | First-person pistol candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_PressurePistol_BarrelAssembly.prefab` | Muzzle/barrel alignment candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_Scattergun_Body_TwinBoiler.prefab` | Second weapon silhouette candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_AmmoCartridge_Cluster.prefab` | Pickup readability candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_WallWeaponRack_ThreeSlot.prefab` | Dressing/interactable prop candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Prefabs/BB_WPS02_GearKey_Housing.prefab` | Objective prop candidate. |
| Material | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Materials/WPS02_MAT_AgedBrass.mat` | Weapon brass lookdev material. |
| Mesh | `Packages/com.brassworks.sidecar.weapon-props-set02/Runtime/Meshes/WPS02_Mesh_BeveledBox.asset` | Reusable custom mesh load check. |

## Validator Asset Count Recommendation

If the main lane uses all representative assets above as `RequiredAssetPaths`, the new additions contribute 31 loadable asset checks across four packages. Combined with the current 20 checks, the validator should report 51 assets after all eight package entries are active.
