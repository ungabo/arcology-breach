# V0.1.42 Asset Path Inventory

Purpose: list representative package assets the main lane should validate after adding the three target packages to `Packages/manifest.json`. Paths use Package Manager resolution form because `SidecarQuarantineImportValidator` loads assets through `AssetDatabase`.

## Package Counts From Manifests And File Inspection

| Package | Prefabs | Materials | Meshes | Runtime scripts | Textures | Preview renders | Notes |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| CorridorKitSet02 | 32 | 18 | 6 | 0 | 0 | 5 | Corridor, door, wall, floor, ceiling, room, signage, and light-run visual shells. |
| EncounterEnemySet02 | 16 | 16 | 12 | 0 | 0 | 21 | Four pose/readability enemy families; visual-only and not rigged gameplay actors. |
| WeaponViewmodelSet03 | 20 | 14 | 7 | 1 | 0 | 21 | FPS weapon assemblies, modules, glove silhouettes, ammo props, and identity metadata. |

## CorridorKitSet02 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.corridor-kit-set02/Documentation~/Manifest/SCK2_CorridorKitSet02_Manifest_v0.1.41-p001.json` | Package-local evidence exists and records zero saved colliders. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorStraight_4m_NorthStar.prefab` | Baseline straight corridor shell. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorStraight_2m_ServiceDense.prefab` | Dense short corridor variation. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorCorner_90_Bulkhead.prefab` | Corner module scale and rotation coverage. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorTJunction_PipeSpine.prefab` | Junction module coverage. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_CorridorCrossJunction_CompassHub.prefab` | Large junction renderer-count and silhouette coverage. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Door_BulkheadRound_3m.prefab` | Door visual that must not imply gameplay authority. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_Door_PressureLock_DoubleLeaf.prefab` | Lock language and hazard color coverage. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_RoomWallPanel_GaugeNest.prefab` | Room wall/gauge detail coverage. |
| Prefab | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Prefabs/SCK2_LightRun_AmberCaged_4m.prefab` | Practical light visual coverage. |
| Material | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Materials/SCK2_MAT_PressureGreenGlass.mat` | Transparent/glass lookdev material. |
| Mesh | `Packages/com.brassworks.sidecar.corridor-kit-set02/Runtime/Meshes/SCK2_MESH_NorthStar8Unit.asset` | High-signal custom mesh load check. |

## EncounterEnemySet02 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Documentation~/Manifest/EE02_EncounterEnemySet02_Manifest_v0.1.41-p001.json` | Package-local evidence exists and records visual-only runtime safety. |
| Prefab | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_AshcanReclaimer_A_IdleSawScout.prefab` | Low fast melee family baseline. |
| Prefab | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_AshcanReclaimer_B_ClawWindupTell.prefab` | Melee windup readability pose. |
| Prefab | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_PressureSpindle_B_NeedleThrustTell.prefab` | Tall thrust/ranged tell coverage. |
| Prefab | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GatehammerBastion_A_ShieldedIdle.prefab` | Large blocker silhouette coverage. |
| Prefab | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Prefabs/EE02_GovernorWarden_B_BellBeaconCastTell.prefab` | Elite/boss support tell coverage. |
| Material | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Materials/EE02_MAT_RedOverheatTell.mat` | Readability material for danger/tell checks. |
| Material | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Materials/EE02_MAT_ReadabilityGhost.mat` | Non-final ghost/readability material coverage. |
| Mesh | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Meshes/EE02_MESH_36ToothSawBlade.asset` | Ashcan/Reclaimer custom mesh coverage. |
| Mesh | `Packages/com.brassworks.sidecar.encounter-enemy-set02/Runtime/Meshes/EE02_MESH_GovernorCommandGearHalo.asset` | Governor family custom mesh coverage. |

## WeaponViewmodelSet03 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Documentation~/Manifest/WVM03_WeaponViewmodelSet03_Manifest_v0.1.41-p001.json` | Package-local evidence exists and records first-person review risks. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_PressurePistol_FullAssembly_A.prefab` | Main pressure pistol viewmodel candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_PressurePistol_FullAssembly_B_DualGauge.prefab` | Alternate gauge-heavy pistol candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_Scattergun_FullAssembly_A.prefab` | Scattergun viewmodel candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_BoltThrower_FullAssembly_A.prefab` | Third weapon silhouette and rail clearance candidate. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_GloveSilhouette_RightGrip.prefab` | Hand scale and right grip proxy coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_GloveSilhouette_LeftSupport.prefab` | Off-hand support silhouette coverage. |
| Prefab | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/BB_WVM03_AmmoPressureCell_Single.prefab` | Ammo prop readability coverage. |
| Material | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Materials/WVM03_MAT_GreenGaugeGlass.mat` | Gauge/glass material coverage. |
| Mesh | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Meshes/WVM03_Mesh_GlovePalm.asset` | Glove silhouette mesh coverage. |
| Runtime script | `Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/WeaponViewmodelSet03Identity.cs` | Confirms the package runtime assembly and identity metadata asset are imported. |

## Validator Asset Count Recommendation

If the main lane uses all representative assets above as `RequiredAssetPaths`, the new additions contribute 30 loadable asset checks across three packages. Combined with the current 51 checks across eight packages, `SidecarQuarantineImportValidator` should report 11 packages and 81 representative assets after all three new package entries are active.
