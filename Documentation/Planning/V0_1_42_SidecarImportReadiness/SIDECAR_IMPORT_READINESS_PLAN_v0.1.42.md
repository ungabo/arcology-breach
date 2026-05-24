# V0.1.42 Sidecar Import Readiness Plan

Generated: 2026-05-24

Purpose: give the main lane a concrete, Unity-only path for importing CorridorKitSet02, EncounterEnemySet02, and WeaponViewmodelSet03 after package review. This packet is documentation-only and leaves manifest edits, validator code, scene generation, and lookdev execution to the main integration lane.

## Current State

| Package | UPM name | Package version | Manifest evidence | Generated asset counts | Static validation | Main manifest |
| --- | --- | --- | --- | --- | --- | --- |
| CorridorKitSet02 | `com.brassworks.sidecar.corridor-kit-set02` | `0.1.41-p001` | `SCK2_CorridorKitSet02_Manifest_v0.1.41-p001.json` | 32 prefabs, 18 materials, 6 meshes, 0 textures, 5 preview renders | pass, 0 errors, 0 warnings | absent |
| EncounterEnemySet02 | `com.brassworks.sidecar.encounter-enemy-set02` | `0.1.41-p001` | `EE02_EncounterEnemySet02_Manifest_v0.1.41-p001.json` | 16 prefabs, 16 materials, 12 meshes, 0 runtime scripts, 21 preview renders | pass, 0 errors, 0 warnings | absent |
| WeaponViewmodelSet03 | `com.brassworks.sidecar.weapon-viewmodel-set03` | `0.1.41` | `WVM03_WeaponViewmodelSet03_Manifest_v0.1.41-p001.json` | 20 prefabs, 14 materials, 7 meshes, 1 runtime script, 21 preview renders | pass, 0 errors, 0 warnings | absent |

The current `Packages/manifest.json` already references these earlier sidecar packages: feedback FX audio, materials set 01, level dressing set 01, mechanical enemies, mechanical enemy visual set 01, steampunk weapons, steamworks level kit, and weapon props set 02. `SidecarQuarantineImportValidator` currently checks eight packages and 51 representative assets.

## Import Sequence

1. Review `git status --short` and coordinate with parallel workers before touching `Packages/manifest.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, or generated scenes.
2. Add only these three local UPM references to `Packages/manifest.json`:

```json
"com.brassworks.sidecar.corridor-kit-set02": "file:../AssetPacks/BrassworksBreach.CorridorKitSet02",
"com.brassworks.sidecar.encounter-enemy-set02": "file:../AssetPacks/BrassworksBreach.EncounterEnemySet02",
"com.brassworks.sidecar.weapon-viewmodel-set03": "file:../AssetPacks/BrassworksBreach.WeaponViewmodelSet03",
```

3. Open Unity and allow one package resolution and script compile before editing showcase code.
4. Extend `SidecarQuarantineImportValidator` with three new `PackageCheck` entries using the representative paths in `ASSET_PATH_INVENTORY_v0.1.42.md`.
5. Run `Project Tools/Validate Sidecar Quarantine Imports`. If the full proposed v0.1.42 inventory is used, expected result shape is `packages=11 assets=81`.
6. Add `V0SceneBuilder.cs` showcase placements only after the import validator is green. Keep every instance under `Sidecar Quarantine Showcase - <LevelXX>`.
7. Update `V0LevelValidator.cs` to require the new showcase object names and maintain zero colliders, zero rigidbodies, zero autonomous audio sources, and minimum renderer count checks.
8. Rebuild generated scenes through the existing Unity menu flow, then run `Project Tools/Validate v0 Levels`.
9. Perform manual lookdev in Unity only. Do not use Blender for render, lookdev, socket, or scale review.
10. Validate rollback by removing the three manifest references and the corresponding validator/showcase changes in a throwaway check.

## Compile Batching

| Batch | Contents | Pass signal |
| --- | --- | --- |
| A | Manifest references plus `SidecarQuarantineImportValidator` additions | Unity compiles; import validator reports 11 packages and 81 assets if all proposed checks are used. |
| B | `V0SceneBuilder` quarantine placements and `V0LevelValidator` required names | Scene generator compiles; no live gameplay references are introduced. |
| C | Generated scene rebuild | `Project Tools/Validate v0 Levels` passes across Level01 through Level05. |
| D | Manual lookdev and rollback rehearsal | Scale/readability notes captured; removing the three package refs leaves no missing scene references. |

## Readiness Gates

| Gate | Required pass signal |
| --- | --- |
| Package resolution | `PackageManager.PackageInfo.FindForAssetPath` resolves each new package to the expected UPM name. |
| Package-local evidence | Each `Documentation~/Manifest` JSON exists, parses, and matches the expected package name and core counts. |
| Static package hygiene | `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1` remains pass with 0 errors and 0 warnings for all three target roots. |
| Asset loadability | Representative prefabs, materials, meshes, and the WVM03 runtime identity script load through `AssetDatabase`. |
| Presentation safety | New showcase instances add no colliders, rigidbodies, autonomous audio sources, AI scripts, damage scripts, pickups, or level triggers. |
| Scene validation | `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild. |
| Rollback | Removing the three package refs and their showcase/validator entries restores the prior eight-package lane without missing references. |

## Decision

Proceed to main-lane quarantine import after review owner accepts this packet. The packages are ready for a controlled import batch, but they are not promotion-ready gameplay assets.
