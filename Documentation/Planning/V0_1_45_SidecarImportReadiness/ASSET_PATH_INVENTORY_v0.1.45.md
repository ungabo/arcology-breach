# V0.1.45 Asset Path Inventory

Purpose: list representative package assets the main lane should validate after adding LevelAtmosphereSet03 and EnemyAnimationProxySet01 to `Packages/manifest.json`. Paths use Package Manager resolution form because `SidecarQuarantineImportValidator` loads assets through `AssetDatabase`.

## Package Counts From Manifests And File Inspection

| Package | Prefabs | Materials | Meshes | Placeholder clips | Runtime scripts | Textures | Preview renders | Notes |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| LevelAtmosphereSet03 | 28 | 16 | 8 | 0 | 0 | 0 | 10 | Steam pipe leaks, pressure lamps, wall grime, hanging chains, pulley silhouettes, floor drains, warning gauges, overhead pipe canopies. |
| EnemyAnimationProxySet01 | 16 | 15 | 8 | 4 | 0 | 0 | 16 | Visual-only mechanical enemy pose proxies for Scrapper/Ashcan, Lancer/Pressure Spindle, Bulwark/Gatehammer, and Warden/Governor families. |

## LevelAtmosphereSet03 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Documentation~/Manifest/SCLA_LevelAtmosphereSet03_Manifest_v0.1.44-p001.json` | Package-local evidence exists and records visual-only asset counts. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_SteamPipeCluster_WallLeaker_A.prefab` | Wall steam leak baseline for route-edge atmosphere review. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_SteamPipeCluster_CornerBleed.prefab` | Corner bleed silhouette and occlusion-risk coverage. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_PressureLamp_WallCaged_A.prefab` | Warm caged lamp readability without runtime light authority. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_WallGrimePanel_OilStreaks.prefab` | Wall grime layering and material fallback coverage. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_HangingChains_TripleSlack.prefab` | Hanging silhouette and route-clearance coverage. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_FloorDrainCover_LongGutter.prefab` | Floor detail scale coverage without collision authority. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_WarningGauge_TripleRack.prefab` | Gauge-readability and redline warning-language coverage. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_OverheadPipeCanopy_ValveRun.prefab` | Ceiling density and headroom readability coverage. |
| Prefab | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Prefabs/SCLA_DenseAmbienceCombo_CorridorBite.prefab` | Highest-density atmosphere prefab for renderer and clutter stress. |
| Material | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Materials/SCLA_MAT_AmberLampGlass.mat` | Emissive amber lamp material check. |
| Mesh | `Packages/com.brassworks.sidecar.level-atmosphere-set03/Runtime/Meshes/SCLA_MESH_SteamWispUnit.asset` | Custom steam-wisp mesh coverage. |

## EnemyAnimationProxySet01 Representative Assets

| Category | Path | Validation reason |
| --- | --- | --- |
| Manifest | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Documentation~/Manifest/EAP01_EnemyAnimationProxySet01_Manifest_v0.1.44-p001.json` | Package-local evidence exists and records no runtime authority, no colliders, no audio, and placeholder animation status. |
| Prefab | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_ScrapperAshcan_01_IdleBrace.prefab` | Baseline small enemy readable silhouette. |
| Prefab | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_ScrapperAshcan_03_SawLunge.prefab` | Attack-pose silhouette and saw read coverage. |
| Prefab | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_LancerPressureSpindle_01_AimLine.prefab` | Ranged/lancer windup readability coverage. |
| Prefab | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_LancerPressureSpindle_03_ThrustPeak.prefab` | Long-weapon extension and route-clearance coverage. |
| Prefab | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_BulwarkGatehammer_02_HammerRaise.prefab` | Heavy enemy pre-attack silhouette coverage. |
| Prefab | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Prefabs/EAP01_WardenGovernor_02_SignalRaise.prefab` | Commander silhouette and signaling pose coverage. |
| Material | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Materials/EAP01_MAT_FurnaceOrangeGlow.mat` | Overheat/glow material fallback coverage. |
| Mesh | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/Meshes/EAP01_MESH_CommandGearHalo.asset` | Warden/commander custom mesh coverage. |
| AnimationClip | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/AnimationClips/EAP01_CLIP_ScrapperAshcan_PoseProxyOnly.anim` | Placeholder clip import coverage; must remain non-authoritative. |
| AnimationClip | `Packages/com.brassworks.sidecar.enemy-animation-proxy-set01/Runtime/AnimationClips/EAP01_CLIP_WardenGovernor_PoseProxyOnly.anim` | Commander placeholder clip import coverage; must remain non-authoritative. |

## Validator Asset Count Recommendation

If the main lane uses all non-manifest representative assets above as `RequiredAssetPaths`, the new additions contribute 21 loadable asset checks across two packages. Manifest rows are package evidence and are validated through the `PackageCheck` manifest path, not counted as required asset paths.

- LevelAtmosphereSet03: 11 checks.
- EnemyAnimationProxySet01: 10 checks.

Combined with the current v0.1.43 baseline of 81 checks across 11 packages and the planned v0.1.44 wave of 21 checks across 2 packages, `SidecarQuarantineImportValidator` should report 15 packages and 123 representative assets after all four pending packages are active.
