# V0.1.41 Main-Lane Validation Checklist

Purpose: QA checklist for importing MaterialsSet01, LevelDressingSet01, MechanicalEnemyVisualSet01, and WeaponPropsSet02 into the primary Unity project.

## Preflight

- [ ] `git status --short` reviewed before any main-lane edit.
- [ ] Confirm no parallel worker is editing `Packages/manifest.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, or generated scenes.
- [ ] Run package-specific static validation:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.MaterialsSet01'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.LevelDressingSet01'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.MechanicalEnemyVisualSet01'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.WeaponPropsSet02'
```

- [ ] Expected static validation result for each package: pass, 0 errors, 0 warnings.

## Package Import

- [ ] Add the four local UPM package references to `Packages/manifest.json`.
- [ ] Open Unity and allow one clean package resolution and script compile.
- [ ] Confirm no package asks for new third-party dependencies or ProjectSettings changes.
- [ ] Confirm all four packages resolve through Package Manager under the expected UPM names.
- [ ] Confirm no files changed outside expected Unity package lock/manifest churn before continuing.

## Quarantine Import Validator

- [ ] Extend `SidecarQuarantineImportValidator` with four `PackageCheck` entries.
- [ ] Use the representative required asset paths from `Documentation/Planning/V0_1_41_SidecarImportReadiness/ASSET_PATH_INVENTORY_v0.1.41.md`.
- [ ] Run `Project Tools/Validate Sidecar Quarantine Imports`.
- [ ] Expected result after adding all four checks: no exception; eight packages checked; 51 representative assets checked if the full v0.1.41 inventory is used.
- [ ] If texture-specific helpers are added, verify `MSET01_*_NRM.png` textures import as normal maps.

## Showcase And Scene Validation

- [ ] Update `V0SceneBuilder` to place representative visuals under `Sidecar Quarantine Showcase - <LevelXX>`.
- [ ] For MaterialsSet01, create non-colliding material swatch cubes or plinths under the showcase root.
- [ ] Update `V0LevelValidator` to require at least one new showcase object from each relevant package family.
- [ ] Rebuild scenes through the existing Unity menu flow.
- [ ] Run `Project Tools/Validate v0 Levels`.
- [ ] Confirm every showcase root has zero colliders, zero rigidbodies, zero autonomous audio sources, and no gameplay scripts.

## Manual Lookdev QA

- [ ] Level01: compare WPS02 pressure pistol and gear-key housing against existing first-room readability.
- [ ] Level02: inspect SCLD pipe/tank/valve props for corridor scale and route readability.
- [ ] Level03: inspect scattergun, boiler gauge pedestal, and Bellows support visual at short and medium range.
- [ ] Level04: inspect Bulwark candidate, vent stack, drain channel, wet stone, and soot materials under foundry lighting.
- [ ] Level05: inspect Warden/Overseer silhouettes, gauge panel, and pressure glass swatch near boss-route lighting.
- [ ] Check first-person weapon candidates from camera height around 1.65 meters before any gameplay promotion.
- [ ] Confirm sidecar visuals do not block movement, pickups, combat sightlines, doors, objectives, or level transitions.

## Rollback Validation

- [ ] Remove the four new package references from `Packages/manifest.json` in a throwaway rollback check.
- [ ] Let Unity re-resolve packages.
- [ ] Confirm generated scenes do not retain missing package references after the showcase batch is reverted or rebuilt.
- [ ] Confirm `git status --short` shows no unexpected edits outside planned main-lane files.

## Pass Criteria

- [ ] Package-specific static validation stays at 0 errors and 0 warnings.
- [ ] `SidecarQuarantineImportValidator` passes with all eight accepted packages.
- [ ] `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- [ ] Manual lookdev confirms each new package has at least one useful, readable, quarantine-safe placement.
- [ ] Rollback remains manifest-line removal plus showcase change revert; no asset root surgery is needed.
