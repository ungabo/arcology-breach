# V0.1.38 Sidecar Quarantine Readiness Report

Generated: 2026-05-24T14:28:34.8774481-04:00

Purpose: inventory sidecar asset packages before primary-lane quarantine import. This report is static and non-destructive; it does not edit Unity manifests, import packages, or modify the primary project.

## Summary

| Field | Value |
| --- | --- |
| Project | D:\__MY APPS\Unity Doom |
| Asset packs | D:\__MY APPS\Unity Doom\AssetPacks |
| Package pattern | BrassworksBreach.* |
| Packages checked | 4 |
| Errors | 0 |
| Warnings | 21 |

## Package Readiness

| Package | UPM name | Version | Manifests | Decision | Errors | Warnings |
| --- | --- | --- | ---: | --- | ---: | ---: |
| BrassworksBreach.FeedbackFXAudio | com.brassworks.sidecar.feedback-fx-audio | 0.1.38-p001 | 0 | needs_generation_or_remediation | 0 | 1 |
| BrassworksBreach.MechanicalEnemies | com.brassworks.sidecar.mechanical-enemies | 0.1.37-p001 | 1 | ready_for_primary_quarantine | 0 | 0 |
| BrassworksBreach.SteampunkWeapons | com.brassworks.sidecar.steampunk-weapons | 0.1.37 | 1 | ready_for_primary_quarantine | 0 | 0 |
| BrassworksBreach.SteamworksLevelKit | com.brassworks.sidecar.steamworks-level-kit | 0.1.37-p001 | 1 | needs_generation_or_remediation | 0 | 20 |

## Manifest Asset Checks

### BrassworksBreach.FeedbackFXAudio

No package-local manifest was found.

### BrassworksBreach.MechanicalEnemies

Manifest: AssetPacks/BrassworksBreach.MechanicalEnemies/Documentation~/Manifest/SCENM_MechanicalEnemies_Manifest_v0.1.37-p001.json

| Category | Expected | Manifest refs | Existing refs | Missing refs | Placeholders | Disk count |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| generated_prefabs | 5 | 5 | 5 | 0 | 0 | 5 |
| generated_materials | 10 | 10 | 10 | 0 | 0 | 10 |
| generated_meshes | 7 | 7 | 7 | 0 | 0 | 8 |
| preview_renders | 6 | 6 | 6 | 0 | 0 | 6 |

### BrassworksBreach.SteampunkWeapons

Manifest: AssetPacks/BrassworksBreach.SteampunkWeapons/Documentation~/Manifest/SCWPN_SteampunkWeapons_Manifest_v0.1.37-p001.json

| Category | Expected | Manifest refs | Existing refs | Missing refs | Placeholders | Disk count |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| generated_prefabs | 7 | 7 | 7 | 0 | 0 | 7 |
| generated_materials | 7 | 7 | 7 | 0 | 0 | 7 |
| generated_meshes | 1 | 1 | 1 | 0 | 0 | 1 |
| preview_renders | 7 | 7 | 7 | 0 | 0 | 7 |

### BrassworksBreach.SteamworksLevelKit

Manifest: AssetPacks/BrassworksBreach.SteamworksLevelKit/Documentation~/Manifest/SCLVL_SteamworksLevelKit_Manifest_v0.1.37-p001.json

| Category | Expected | Manifest refs | Existing refs | Missing refs | Placeholders | Disk count |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| generated_prefabs | 13 | 13 | 0 | 13 | 0 | 0 |
| generated_materials | 12 | 1 | 0 | 0 | 1 | 0 |
| generated_meshes | 3 | 1 | 0 | 0 | 1 | 1 |
| preview_renders | 3 | 1 | 0 | 0 | 1 | 0 |

## Findings

| Severity | Package | Path | Message |
| --- | --- | --- | --- |
| warning | BrassworksBreach.FeedbackFXAudio | AssetPacks/BrassworksBreach.FeedbackFXAudio | No package-local manifest JSON found under Documentation~/Manifest. |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_CorridorStraight_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_CorridorStraight_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_CorridorCorner_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_CorridorCorner_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_TJunction_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_TJunction_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_BoilerAlcove_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_BoilerAlcove_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_GaugeWall_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_GaugeWall_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_RivetedVaultDoor_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_RivetedVaultDoor_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_PressureLockDoorFrame_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_PressureLockDoorFrame_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_PipeRailing_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_PipeRailing_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_CatwalkFloor_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_CatwalkFloor_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_WallColumn_3m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_WallColumn_3m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_CeilingPipeCluster_4m.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_CeilingPipeCluster_4m.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_ValveConsole.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_ValveConsole.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Runtime/Prefabs/SCLVL_VentSmokeEmitterAnchor.prefab | Manifest lists missing generated_prefabs asset: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Prefabs/SCLVL_VentSmokeEmitterAnchor.prefab |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit | generated_prefabs count expects 13 but only 0 matching file(s) are on disk. |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Documentation~/Manifest/SCLVL_SteamworksLevelKit_Manifest_v0.1.37-p001.json | Manifest uses placeholder generated_materials reference: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Materials/generated_by_SteamworksLevelKitGenerator |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit | generated_materials count expects 12 but only 0 matching file(s) are on disk. |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Documentation~/Manifest/SCLVL_SteamworksLevelKit_Manifest_v0.1.37-p001.json | Manifest uses placeholder generated_meshes reference: Packages/com.brassworks.sidecar.steamworks-level-kit/Runtime/Meshes/generated_by_SteamworksLevelKitGenerator |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit | generated_meshes count expects 3 but only 1 matching file(s) are on disk. |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit/Documentation~/Manifest/SCLVL_SteamworksLevelKit_Manifest_v0.1.37-p001.json | Manifest uses placeholder preview_renders reference: Documentation/ConceptRenders/V0_1_37_SteamworksLevelKitSidecar/generated_by_SteamworksLevelKitPreviewRenderer |
| warning | BrassworksBreach.SteamworksLevelKit | AssetPacks/BrassworksBreach.SteamworksLevelKit | preview_renders count expects 3 but only 0 matching file(s) are on disk. |

## How To Use This Report

- ready_for_primary_quarantine: static inventory is clean enough for primary-lane quarantine import after the clean throwaway import evidence is reviewed.
- needs_generation_or_remediation: the package is useful but still has placeholder or missing generated assets; run its sidecar generator/render pass before quarantine import.
- blocked_static_errors: fix package/manifest structure before any Unity import work.

Next-step directive: continue immediately with the next highest-impact unfinished task.

