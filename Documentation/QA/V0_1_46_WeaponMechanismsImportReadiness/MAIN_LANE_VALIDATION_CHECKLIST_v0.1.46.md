# V0.1.46 Main-Lane Validation Checklist

Purpose: QA checklist for importing WeaponMechanismsSet04 into the primary Unity project.

## Preflight

- [ ] `git status --short` reviewed before any main-lane edit.
- [ ] Confirm no parallel worker is editing `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, generated scenes, package roots, or shared status docs.
- [ ] Confirm target package root exists: `AssetPacks/BrassworksBreach.WeaponMechanismsSet04`.
- [ ] Confirm target package is absent from the main manifest before this import.
- [ ] Run package-specific static validation:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.WeaponMechanismsSet04' -Json
```

- [ ] Expected static validation result: pass, 1 package, 0 errors, 0 warnings.

## Package Import

- [ ] Add the local UPM package reference to `Packages/manifest.json`:

```json
"com.brassworks.sidecar.weapon-mechanisms-set04": "file:../AssetPacks/BrassworksBreach.WeaponMechanismsSet04"
```

- [ ] Open Unity and allow one clean package resolution and script compile.
- [ ] Confirm the package resolves under `com.brassworks.sidecar.weapon-mechanisms-set04`.
- [ ] Confirm the package does not add unapproved third-party dependencies or ProjectSettings changes.
- [ ] Confirm production evidence remains available under `Documentation/AssetProduction/V0_1_45_WeaponMechanismsSet04`.
- [ ] Confirm preview renders remain available under `Documentation/ConceptRenders/V0_1_45_WeaponMechanismsSet04`.

## Quarantine Import Validator

- [ ] Extend `SidecarQuarantineImportValidator` with the WeaponMechanismsSet04 `PackageCheck` entry.
- [ ] Use the representative required asset paths from `Documentation/Planning/V0_1_46_WeaponMechanismsImportReadiness/ASSET_PATH_INVENTORY_v0.1.46.md`.
- [ ] Run `Project Tools/Validate Sidecar Quarantine Imports`.
- [ ] Expected result if the current v0.1.45 inventory and this v0.1.46 inventory are active: no exception, 16 packages checked, 138 representative assets checked.
- [ ] If higher-accuracy helpers are added, assert manifest counts: 29 prefabs, 20 materials, 11 meshes, 11 preview renders.
- [ ] If catalog helpers are added, assert sampled prefabs are marked `visual_only=true`.
- [ ] If prefab safety helpers are added, assert representative prefabs have no colliders, rigidbodies, autonomous audio sources, gameplay controllers, weapon configs, projectile spawners, damage scripts, pickups, inventory scripts, viewmodel animation controllers, hitboxes, nav agents, AI controllers, doors, lifts, bridges, objectives, or transitions.

## Showcase And Scene Validation

- [ ] Update `V0SceneBuilder` only after the import validator passes.
- [ ] Place WMS04 visuals under `Sidecar Quarantine Showcase - <LevelXX>`.
- [ ] Add only presentation objects: no firing, damage, projectile, pickup, ammo, reload, recoil, inventory, muzzle, weapon selection, or viewmodel authority.
- [ ] Add `V0LevelValidator` required names after placement names are final.
- [ ] Rebuild scenes through the existing Unity menu flow.
- [ ] Run `Project Tools/Validate v0 Levels`.
- [ ] Confirm every WMS04 showcase root has zero colliders, zero rigidbodies, zero autonomous audio sources, no gameplay authority scripts, no route blockers, and no live weapon or viewmodel authority.

## Manual Unity Lookdev QA

- [ ] Level01: inspect `WMS04PressurePistolCoilTripleAmberA` and `WMS04GaugeClusterTripleIvoryA` from player-height view.
- [ ] Level02: inspect `WMS04GripAssemblyWalnutLeatherA` and `WMS04ReceiverPlateBrassLatticeA` for grip scale, hand-clearance risk, and receiver detail.
- [ ] Level03: inspect `WMS04AmmoCylinderEightCellB` and `WMS04ScattergunPressureChamberQuadB` for ammo/reload implication risk and bulky silhouette readability.
- [ ] Level04: inspect `WMS04BoltThrowerRailChargedSlideB` and `WMS04MuzzleCrownCogBrakeB` for rail/muzzle promotion risk without projectile or damage authority.
- [ ] Level05: inspect `WMS04PressureTankTwinUnderbarrelB` and `WMS04GlovedHandRightGripA` for late-level lighting readability and future viewmodel hand risk.
- [ ] Confirm weapon components do not hide pickups, enemy telegraphs, route signs, exits, doors, secrets, or boss mechanics.
- [ ] Confirm the gloved-hand silhouette is visibly a quarantine visual reference, not live player hands.
- [ ] Do all visual review in Unity only; do not use Blender.

## Rollback Validation

- [ ] Remove the WMS04 package reference from `Packages/manifest.json` in a throwaway rollback check.
- [ ] Remove matching validator entry, showcase placements, and required names.
- [ ] Let Unity re-resolve packages and compile.
- [ ] Rebuild generated scenes if showcase references were added.
- [ ] Confirm generated scenes do not retain missing `WMS04_` or package references.
- [ ] Confirm `git status --short` shows no unexpected edits outside planned main-lane files.

## Pass Criteria

- [ ] Package-specific static validation stays at 0 errors and 0 warnings.
- [ ] `SidecarQuarantineImportValidator` passes with 16 packages and 138 representative assets if the proposed inventory is used.
- [ ] `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- [ ] Manual Unity lookdev confirms WMS04 assets are readable, useful, and quarantine-safe across all five levels.
- [ ] Rollback remains package-reference removal plus validator/showcase cleanup; no package-root edits are needed.
