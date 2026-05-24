# V0.1.41 Sidecar Import Readiness Plan

Generated: 2026-05-24

Purpose: give the main lane a concrete, Unity-only import path for four accepted sidecar packages that are not yet in the primary `Packages/manifest.json`. This review is documentation-only and intentionally leaves manifest edits, scene generation, and validator code changes to the main integration lane.

## Current State

| Package | UPM name | Version | Static validator | Package-local evidence | Main manifest |
| --- | --- | --- | --- | --- | --- |
| MaterialsSet01 | `com.brassworks.sidecar.materials-set01` | `0.1.39-p001` | pass, 0 errors, 0 warnings | `MSET01_MaterialsSet01_Manifest_v0.1.39-p001.json` | absent |
| LevelDressingSet01 | `com.brassworks.sidecar.level-dressing-set01` | `0.1.40-p001` | pass, 0 errors, 0 warnings | `SCLD_LevelDressingSet01_Manifest_v0.1.40-p001.json` | absent |
| MechanicalEnemyVisualSet01 | `com.brassworks.sidecar.mechanical-enemy-visual-set01` | `0.1.40-p001` | pass, 0 errors, 0 warnings | `MEV01_MechanicalEnemyVisualSet01_Manifest_v0.1.40-p001.json` | absent |
| WeaponPropsSet02 | `com.brassworks.sidecar.weapon-props-set02` | `0.1.40` | pass, 0 errors, 0 warnings | `WPS02_WeaponPropsSet02_Manifest_v0.1.40-p001.json` | absent |

Main manifest currently resolves the older accepted sidecars only: feedback FX audio, mechanical enemies, steampunk weapons, and steamworks level kit.

## Import Sequence

1. Confirm `git status --short` and stop if unrelated main-lane files are mid-edit in `Packages`, `Assets/_Project/Editor`, or generated scenes.
2. Main lane adds these local UPM dependencies to `Packages/manifest.json` in one manifest-only change:

```json
"com.brassworks.sidecar.materials-set01": "file:../AssetPacks/BrassworksBreach.MaterialsSet01",
"com.brassworks.sidecar.level-dressing-set01": "file:../AssetPacks/BrassworksBreach.LevelDressingSet01",
"com.brassworks.sidecar.mechanical-enemy-visual-set01": "file:../AssetPacks/BrassworksBreach.MechanicalEnemyVisualSet01",
"com.brassworks.sidecar.weapon-props-set02": "file:../AssetPacks/BrassworksBreach.WeaponPropsSet02",
```

3. Let Unity resolve and compile once before touching showcase code.
4. Extend `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs` with four new `PackageCheck` entries using the representative assets in `ASSET_PATH_INVENTORY_v0.1.41.md`.
5. Run `Project Tools/Validate Sidecar Quarantine Imports`. Expected shape after the four additions: eight packages checked, no thrown exceptions, and a higher asset count than the current `packages=4 assets=20`.
6. Update `V0SceneBuilder.cs` showcase placements only after the import validator is green. Keep presentation-only instances under `Sidecar Quarantine Showcase - <LevelXX>`.
7. Update `V0LevelValidator.cs` showcase requirements to assert at least one required visual from each newly imported content family and continue enforcing zero colliders, rigidbodies, and autonomous audio sources.
8. Rebuild generated scenes through the existing Unity menu flow, then run `Project Tools/Validate v0 Levels`.
9. Do a manual lookdev pass in Unity for scale, silhouette, material contrast, and first-person clearance. Do not use Blender for this review.
10. If any package blocks import, rollback by removing only its manifest line and re-running Unity package resolution.

## Expected `SidecarQuarantineImportValidator` Extensions

Use the existing `PackageCheck` pattern. Keep each package check representative rather than exhaustive so compile cycles stay fast while still catching resolution, manifest, prefab, material, mesh, and texture failures.

Recommended package checks:

- Materials Set 01: manifest plus loadable materials and three texture roles for one metal family.
- Level Dressing Set 01: manifest plus trim, pipe, tank, valve, lamp, decal, service panel, material, and mesh coverage.
- Mechanical Enemy Visual Set 01: manifest plus one candidate from each enemy family, one high-signal material, and one custom mesh.
- Weapon Props Set 02: manifest plus pressure pistol, scattergun, ammo, rack/cabinet, gear-key/pressure-cell props, material, and mesh coverage.

Add these paths as `RequiredAssetPaths` after Package Manager import succeeds. If the validator later grows importer-specific helpers, add texture checks for `*_NRM.png` normal-map import type and material shader/property checks for Standard/Lit fallback.

## Main-Lane Validation Targets

| Gate | Required pass signal |
| --- | --- |
| Package resolution | `PackageManager.PackageInfo.FindForAssetPath` resolves each new package to the expected UPM name. |
| Package-local manifests | Each manifest under `Documentation~/Manifest` exists and is non-empty. |
| Static package hygiene | `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1` remains pass for all four packages. |
| Asset loading | Representative prefabs/materials/textures/meshes load through `AssetDatabase.LoadAssetAtPath`. |
| Showcase safety | Presentation roots add no gameplay colliders, rigidbodies, AI scripts, damage scripts, or autonomous audio sources. |
| Scene validation | Existing `V0LevelValidator.ValidateProjectScenes()` still passes after generated scene rebuild. |
| Rollback | Removing the four manifest lines restores the previous package set with no live scene references left behind. |

## Compile Batching

Batch one should only add manifest dependencies and `SidecarQuarantineImportValidator` entries. Batch two should add showcase placements and validator scene assertions. Batch three should rebuild scenes and perform visual QA. This gives the main lane bigger leaps than single-asset import work while still separating package resolution failures from scene-generation failures.
