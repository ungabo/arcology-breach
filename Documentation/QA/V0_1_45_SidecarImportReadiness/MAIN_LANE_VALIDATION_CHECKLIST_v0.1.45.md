# V0.1.45 Main-Lane Validation Checklist

Purpose: QA checklist for importing LevelAtmosphereSet03 and EnemyAnimationProxySet01 into the primary Unity project.

## Preflight

- [ ] `git status --short` reviewed before any main-lane edit.
- [ ] Confirm the v0.1.44 import wave is landed or explicitly folded into the same coordinated import batch.
- [ ] Confirm no parallel worker is editing `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, or generated scenes.
- [ ] Confirm target package roots exist:
  - `AssetPacks/BrassworksBreach.LevelAtmosphereSet03`
  - `AssetPacks/BrassworksBreach.EnemyAnimationProxySet01`
- [ ] Run package-specific static validation:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.LevelAtmosphereSet03'
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.EnemyAnimationProxySet01'
```

- [ ] Expected static validation result for each package: pass, 0 errors, 0 warnings.

## Package Import

- [ ] Add the two local UPM package references to `Packages/manifest.json`.
- [ ] Open Unity and allow one clean package resolution and script compile.
- [ ] Confirm no target package asks for unapproved third-party dependencies or ProjectSettings changes.
- [ ] Confirm both packages resolve under the expected UPM names.
- [ ] Confirm EnemyAnimationProxySet01 placeholder AnimationClip assets import cleanly before any scene builder work.

## Quarantine Import Validator

- [ ] Extend `SidecarQuarantineImportValidator` with two `PackageCheck` entries.
- [ ] Use the representative required asset paths from `Documentation/Planning/V0_1_45_SidecarImportReadiness/ASSET_PATH_INVENTORY_v0.1.45.md`.
- [ ] Run `Project Tools/Validate Sidecar Quarantine Imports`.
- [ ] Expected result if the v0.1.44 and v0.1.45 proposed inventories are both active: no exception, 15 packages checked, 123 representative assets checked.
- [ ] If higher-accuracy helpers are added, assert package manifest counts for SCLA and EAP01.
- [ ] If prefab safety helpers are added, assert representative prefabs have no colliders, rigidbodies, autonomous audio sources, gameplay controllers, damage scripts, pickups, objectives, doors, lifts, bridges, transitions, hitboxes, nav agents, or AI controllers.
- [ ] If animation clip helpers are added, assert EAP01 clips are placeholder loadability checks only and are not bound to runtime enemy controllers.

## Showcase And Scene Validation

- [ ] Update `V0SceneBuilder` to place representative visuals under `Sidecar Quarantine Showcase - <LevelXX>`.
- [ ] Add SCLA atmosphere props only as presentation shells, not route blockers, hazards, interactive lights, or autonomous audio.
- [ ] Add EAP01 enemy animation proxies only as visual pose references, not live enemies or combat authority.
- [ ] Update `V0LevelValidator` required names after placement names are final.
- [ ] Rebuild scenes through the existing Unity menu flow.
- [ ] Run `Project Tools/Validate v0 Levels`.
- [ ] Confirm every showcase root has zero colliders, zero rigidbodies, zero autonomous audio sources, no gameplay authority scripts, no route blockers, and no live AI.

## Manual Unity Lookdev QA

- [ ] Level01: inspect `SCLAPressureLampWallCagedA` and `EAP01ScrapperAshcanIdleBrace` from player-height view.
- [ ] Level02: inspect `SCLASteamPipeWallLeakerA` and `EAP01LancerPressureAimLine` against pipeworks lighting.
- [ ] Level03: inspect `SCLAHangingChainsTripleSlack` and `EAP01BulwarkHammerRaise` for vertical clutter, headroom, and heavy pose readability.
- [ ] Level04: inspect `SCLAOverheadPipeValveRun` and `EAP01WardenGovernorSignalRaise` for ceiling density and commander silhouette readability.
- [ ] Level05: inspect `SCLADenseAmbienceCorridorBite` and `EAP01ScrapperSawLunge` for dense-atmosphere stress and attack-pose clarity.
- [ ] Confirm atmosphere props do not hide pickups, enemy telegraphs, route signs, exits, doors, secrets, or boss mechanics.
- [ ] Confirm static enemy proxies are visibly quarantine/showcase assets and not mistaken for active enemies.
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
- [ ] `SidecarQuarantineImportValidator` passes with 15 packages and 123 representative assets if v0.1.44 and v0.1.45 proposed inventories are both used.
- [ ] `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- [ ] Manual Unity lookdev confirms each target package has useful, readable, quarantine-safe placements.
- [ ] Rollback remains reference removal plus scene/validator cleanup; no package-root edits are needed.
