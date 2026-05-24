# V0.1.42 Main-Lane Validation Checklist

Purpose: QA checklist for importing CorridorKitSet02, EncounterEnemySet02, and WeaponViewmodelSet03 into the primary Unity project.

## Preflight

- [ ] `git status --short` reviewed before any main-lane edit.
- [ ] Confirm no parallel worker is editing `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, or generated scenes.
- [ ] Confirm target package roots exist:
  - `AssetPacks/BrassworksBreach.CorridorKitSet02`
  - `AssetPacks/BrassworksBreach.EncounterEnemySet02`
  - `AssetPacks/BrassworksBreach.WeaponViewmodelSet03`
- [ ] Run package-specific static validation:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.CorridorKitSet02'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.EncounterEnemySet02'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.WeaponViewmodelSet03'
```

- [ ] Expected static validation result for each package: pass, 0 errors, 0 warnings.

## Package Import

- [ ] Add the three local UPM package references to `Packages/manifest.json`.
- [ ] Open Unity and allow one clean package resolution and script compile.
- [ ] Confirm no target package asks for unapproved third-party dependencies or ProjectSettings changes.
- [ ] Confirm all three packages resolve under the expected UPM names.
- [ ] Confirm WVM03 runtime assembly compiles cleanly before any scene builder work.

## Quarantine Import Validator

- [ ] Extend `SidecarQuarantineImportValidator` with three `PackageCheck` entries.
- [ ] Use the representative required asset paths from `Documentation/Planning/V0_1_42_SidecarImportReadiness/ASSET_PATH_INVENTORY_v0.1.42.md`.
- [ ] Run `Project Tools/Validate Sidecar Quarantine Imports`.
- [ ] Expected result if the full v0.1.42 inventory is used: no exception, 11 packages checked, 81 representative assets checked.
- [ ] If higher-accuracy helpers are added, assert package manifest counts for SCK2, EE02, and WVM03.
- [ ] If prefab safety helpers are added, assert representative prefabs have no colliders, rigidbodies, autonomous audio sources, gameplay controllers, damage scripts, pickups, objectives, or transitions.

## Showcase And Scene Validation

- [ ] Update `V0SceneBuilder` to place representative visuals under `Sidecar Quarantine Showcase - <LevelXX>`.
- [ ] Add SCK2 corridor/door/wall assets only as presentation shells, not navigation or collision.
- [ ] Add EE02 enemy poses only as static readability silhouettes, not AI actors.
- [ ] Add WVM03 viewmodels only as scale/lookdev props, not active weapon prefabs.
- [ ] Update `V0LevelValidator` required names after placement names are final.
- [ ] Rebuild scenes through the existing Unity menu flow.
- [ ] Run `Project Tools/Validate v0 Levels`.
- [ ] Confirm every showcase root has zero colliders, zero rigidbodies, zero autonomous audio sources, and no gameplay authority scripts.

## Manual Unity Lookdev QA

- [ ] Level01: inspect SCK2 door frame, SCK2 wall panel, WVM03 pressure pistol, and EE02 Ashcan silhouette from player-height view.
- [ ] Level02: inspect SCK2 straight corridor, overhead pipe rack, EE02 Pressure Spindle tell, and WVM03 ammo pressure cell.
- [ ] Level03: inspect SCK2 T-junction, gauge wall, EE02 Gatehammer blocker, and WVM03 scattergun.
- [ ] Level04: inspect SCK2 bulkhead door, wet floor panel, EE02 hammer tell, and WVM03 bolt thrower.
- [ ] Level05: inspect SCK2 compass hub, north-star signage, EE02 Governor Warden bell tell, and WVM03 gauge cluster.
- [ ] Check WVM03 from first-person camera height around 1.65 meters before any future promotion.
- [ ] Confirm sidecar visuals do not block movement, pickups, combat sightlines, doors, objectives, or level transitions.
- [ ] Do all visual review in Unity only; do not use Blender.

## Rollback Validation

- [ ] Remove the three new package references from `Packages/manifest.json` in a throwaway rollback check.
- [ ] Remove matching validator entries, showcase placements, and required names.
- [ ] Let Unity re-resolve packages and compile.
- [ ] Rebuild generated scenes if showcase references were added.
- [ ] Confirm generated scenes do not retain missing package references.
- [ ] Confirm `git status --short` shows no unexpected edits outside planned main-lane files.

## Pass Criteria

- [ ] Package-specific static validation stays at 0 errors and 0 warnings.
- [ ] `SidecarQuarantineImportValidator` passes with 11 packages and 81 representative assets if the full proposed inventory is used.
- [ ] `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- [ ] Manual Unity lookdev confirms each target package has useful, readable, quarantine-safe placements.
- [ ] Rollback remains reference removal plus scene/validator cleanup; no package-root edits are needed.
