# V0.1.45 Sidecar Import Readiness Plan

Generated: 2026-05-24

Purpose: give the main lane a concrete, Unity-only path for importing LevelAtmosphereSet03 and EnemyAnimationProxySet01 after v0.1.44 package acceptance/import. This packet is documentation-only and leaves manifest edits, package lock updates, validator code, scene generation, and lookdev execution to the main integration lane.

## Current State

| Package | UPM name | Package version | Manifest evidence | Generated asset counts | Static validation | Main manifest |
| --- | --- | --- | --- | --- | --- | --- |
| LevelAtmosphereSet03 | `com.brassworks.sidecar.level-atmosphere-set03` | `0.1.44-p001` | `SCLA_LevelAtmosphereSet03_Manifest_v0.1.44-p001.json` | 28 prefabs, 16 materials, 8 meshes, 10 preview renders | pass, 0 errors, 0 warnings | absent |
| EnemyAnimationProxySet01 | `com.brassworks.sidecar.enemy-animation-proxy-set01` | `0.1.44-p001` | `EAP01_EnemyAnimationProxySet01_Manifest_v0.1.44-p001.json` | 16 prefabs, 15 materials, 8 meshes, 4 placeholder clips, 16 preview renders | pass, 0 errors, 0 warnings | absent |

The current main lane after v0.1.43 references 11 sidecar packages and checks 81 representative assets. The v0.1.44 readiness packet for ObjectivePropsSet02 and SteamVFXSet02 proposes 2 more packages and 21 more asset checks, for an expected interim target of `packages=13 assets=102`. This packet is the next import wave and proposes 2 additional packages plus 21 additional asset checks, for an expected post-v0.1.45 target of `packages=15 assets=123`.

## Static Validator Rerun

Commands rerun on 2026-05-24:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.LevelAtmosphereSet03' -Json
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.EnemyAnimationProxySet01' -Json
```

Exact results:

- LevelAtmosphereSet03: `status=pass`, `package_count=1`, `errors=0`, `warnings=0`, `packages=["BrassworksBreach.LevelAtmosphereSet03"]`, `findings=[]`.
- EnemyAnimationProxySet01: `status=pass`, `package_count=1`, `errors=0`, `warnings=0`, `packages=["BrassworksBreach.EnemyAnimationProxySet01"]`, `findings=[]`.

## Import Sequence

1. Confirm the v0.1.44 package wave is either imported or intentionally skipped. If imported with the proposed inventory, the validator baseline before this wave should be `packages=13 assets=102`.
2. Review `git status --short` and coordinate with parallel workers before touching `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, or generated scenes.
3. Add only these two local UPM references to `Packages/manifest.json`:

```json
"com.brassworks.sidecar.level-atmosphere-set03": "file:../AssetPacks/BrassworksBreach.LevelAtmosphereSet03",
"com.brassworks.sidecar.enemy-animation-proxy-set01": "file:../AssetPacks/BrassworksBreach.EnemyAnimationProxySet01"
```

4. Open Unity and allow one package resolution and script compile before editing showcase code.
5. Extend `SidecarQuarantineImportValidator` with two new `PackageCheck` entries using the representative paths in `ASSET_PATH_INVENTORY_v0.1.45.md`.
6. Run `Project Tools/Validate Sidecar Quarantine Imports`. If v0.1.44 and v0.1.45 inventories are both active, expected result shape is `packages=15 assets=123`.
7. Add `V0SceneBuilder.cs` showcase placements only after the import validator is green. Keep every instance under `Sidecar Quarantine Showcase - <LevelXX>`.
8. Update `V0LevelValidator.cs` to require the new showcase object names and maintain zero colliders, zero rigidbodies, zero autonomous audio sources, and minimum renderer count checks.
9. Rebuild generated scenes through the existing Unity menu flow, then run `Project Tools/Validate v0 Levels`.
10. Perform manual lookdev in Unity only. Do not use Blender for render, socket, scale, pose, or atmosphere review.
11. Validate rollback by removing the two manifest references and corresponding validator/showcase changes in a throwaway check.

## Compile Batching

| Batch | Contents | Pass signal |
| --- | --- | --- |
| A | Manifest references plus `SidecarQuarantineImportValidator` additions | Unity compiles; import validator reports 15 packages and 123 assets if the v0.1.44 and v0.1.45 proposed inventories are both used. |
| B | `V0SceneBuilder` quarantine placements and `V0LevelValidator` required names | Scene generator compiles; no live gameplay references are introduced. |
| C | Generated scene rebuild | `Project Tools/Validate v0 Levels` passes across Level01 through Level05. |
| D | Manual Unity lookdev and rollback rehearsal | Atmosphere density, enemy-pose readability, route safety, and rollback notes are captured. |

## Readiness Gates

| Gate | Required pass signal |
| --- | --- |
| Package resolution | `PackageManager.PackageInfo.FindForAssetPath` resolves each new package to the expected UPM name. |
| Package-local evidence | Each `Documentation~/Manifest` JSON exists, parses, and matches the expected package name and counts. |
| Static package hygiene | `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1` remains pass with 0 errors and 0 warnings for both roots. |
| Asset loadability | Representative prefabs, materials, meshes, metadata, and placeholder clips load through `AssetDatabase`. |
| Presentation safety | New showcase instances add no colliders, rigidbodies, autonomous audio sources, gameplay authority scripts, pickups, triggers, AI controllers, or level transitions. |
| Atmosphere safety | Atmosphere props do not block routes, mask pickups, hide combat telegraphs, or create false lighting/audio expectations. |
| Animation proxy safety | Proxy enemy poses remain visual references only; no rigging, AI, damage, navigation, hitboxes, or combat timing is promoted by this wave. |
| Scene validation | `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild. |
| Rollback | Removing the two package refs and their showcase/validator entries restores the prior package lane without missing references. |

## Decision

Proceed to main-lane quarantine import after review owner accepts this packet. The packages are ready for controlled visual import, but they are not promotion-ready lighting systems, gameplay blockers, rigged animation, enemy AI, combat hitboxes, or tuned encounters.
