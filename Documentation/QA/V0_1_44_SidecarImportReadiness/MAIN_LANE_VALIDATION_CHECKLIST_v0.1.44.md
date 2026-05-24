# V0.1.44 Main-Lane Validation Checklist

Purpose: QA checklist for importing ObjectivePropsSet02 and SteamVFXSet02 into the primary Unity project.

## Preflight

- [ ] `git status --short` reviewed before any main-lane edit.
- [ ] Confirm no parallel worker is editing `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, or generated scenes.
- [ ] Confirm target package roots exist:
  - `AssetPacks/BrassworksBreach.ObjectivePropsSet02`
  - `AssetPacks/BrassworksBreach.SteamVFXSet02`
- [ ] Run package-specific static validation:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.ObjectivePropsSet02'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.SteamVFXSet02'
```

- [ ] Expected static validation result for each package: pass, 0 errors, 0 warnings.

## Package Import

- [ ] Add the two local UPM package references to `Packages/manifest.json`.
- [ ] Open Unity and allow one clean package resolution and script compile.
- [ ] Confirm no target package asks for unapproved third-party dependencies or ProjectSettings changes.
- [ ] Confirm both packages resolve under the expected UPM names.
- [ ] Confirm `ObjectivePropsSet02Identity` and `SteamVfxSet02Identity` compile cleanly before any scene builder work.

## Quarantine Import Validator

- [ ] Extend `SidecarQuarantineImportValidator` with two `PackageCheck` entries.
- [ ] Use the representative required asset paths from `Documentation/Planning/V0_1_44_SidecarImportReadiness/ASSET_PATH_INVENTORY_v0.1.44.md`.
- [ ] Run `Project Tools/Validate Sidecar Quarantine Imports`.
- [ ] Expected result if the full v0.1.44 inventory is used: no exception, 13 packages checked, 102 representative assets checked.
- [ ] If higher-accuracy helpers are added, assert package manifest counts for OPS02 and BBSVFX02.
- [ ] If prefab safety helpers are added, assert representative prefabs have no colliders, rigidbodies, autonomous audio sources, gameplay controllers, damage scripts, pickups, objectives, doors, lifts, bridges, or transitions.
- [ ] If particle helpers are added, assert BBSVFX02 particle collision, trigger, and external-force modules remain disabled unless explicitly owned.

## Showcase And Scene Validation

- [ ] Update `V0SceneBuilder` to place representative visuals under `Sidecar Quarantine Showcase - <LevelXX>`.
- [ ] Add OPS02 objective props only as presentation shells, not live interactables.
- [ ] Add BBSVFX02 particle effects only as visual tests, not gameplay damage or weapon/socket authority.
- [ ] Update `V0LevelValidator` required names after placement names are final.
- [ ] Rebuild scenes through the existing Unity menu flow.
- [ ] Run `Project Tools/Validate v0 Levels`.
- [ ] Confirm every showcase root has zero colliders, zero rigidbodies, zero autonomous audio sources, no gameplay authority scripts, and no route blockers.

## Manual Unity Lookdev QA

- [ ] Level01: inspect `OPS02KeyedLockTriGearVault` and `BBSVFX02SteamVentSoftColumn` from player-height view.
- [ ] Level02: inspect `OPS02ValvePanelTwinPressurePuzzle` and `BBSVFX02PressureLeakRuptureCone` against pipeworks lighting.
- [ ] Level03: inspect `OPS02LiftCallStationBrassCage` and `BBSVFX02FurnaceBlastDoorBelch` for prompt readability and particle opacity.
- [ ] Level04: inspect `OPS02ActuatorBridgeThrowLever` and `BBSVFX02SparkRicochetWallHit` for false-interaction risk and VFX brightness.
- [ ] Level05: inspect `OPS02GovernorOverrideBossKillSwitch` and `BBSVFX02BossPhaseGovernorOvercrank` near final-route lighting.
- [ ] Check muzzle and spark VFX from first-person camera height before any future promotion.
- [ ] Confirm sidecar visuals do not block movement, pickups, combat sightlines, doors, objectives, secrets, or level transitions.
- [ ] Do all visual review in Unity only; do not use Blender.

## Rollback Validation

- [ ] Remove the two new package references from `Packages/manifest.json` in a throwaway rollback check.
- [ ] Remove matching validator entries, showcase placements, and required names.
- [ ] Let Unity re-resolve packages and compile.
- [ ] Rebuild generated scenes if showcase references were added.
- [ ] Confirm generated scenes do not retain missing package references.
- [ ] Confirm `git status --short` shows no unexpected edits outside planned main-lane files.

## Pass Criteria

- [ ] Package-specific static validation stays at 0 errors and 0 warnings.
- [ ] `SidecarQuarantineImportValidator` passes with 13 packages and 102 representative assets if the full proposed inventory is used.
- [ ] `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- [ ] Manual Unity lookdev confirms each target package has useful, readable, quarantine-safe placements.
- [ ] Rollback remains reference removal plus scene/validator cleanup; no package-root edits are needed.
