# V0.1.46 Main-Lane Validation Checklist

Purpose: QA checklist for importing RoomSetpieceKit04 into the primary Unity project.

## Preflight

- [ ] `git status --short` reviewed before any main-lane edit.
- [ ] Confirm no parallel worker is editing `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, or generated scenes.
- [ ] Confirm the target package root exists: `AssetPacks/BrassworksBreach.RoomSetpieceKit04`.
- [ ] Confirm source production evidence exists under `Documentation/AssetProduction/V0_1_45_RoomSetpieceKit04`.
- [ ] Confirm preview renders exist under `Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04`.
- [ ] Run package-specific static validation:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.RoomSetpieceKit04'
```

- [ ] Expected static validation result: pass, 1 package checked, 0 errors, 0 warnings.

## Package Import

- [ ] Add the local UPM package reference to `Packages/manifest.json`.
- [ ] Open Unity and allow one clean package resolution and script compile.
- [ ] Confirm the package resolves under `com.brassworks.sidecar.room-setpiece-kit04`.
- [ ] Confirm the package does not add unapproved third-party dependencies or ProjectSettings changes.
- [ ] Confirm package counts remain 30 prefabs, 18 materials, 10 meshes, and 12 preview renders.

## Quarantine Import Validator

- [ ] Extend `SidecarQuarantineImportValidator` with one `PackageCheck` entry.
- [ ] Use the representative required asset paths from `Documentation/Planning/V0_1_46_RoomSetpieceImportReadiness/ASSET_PATH_INVENTORY_v0.1.46.md`.
- [ ] Run `Project Tools/Validate Sidecar Quarantine Imports`.
- [ ] Expected result if the v0.1.45 import wave and this inventory are both active: no exception, 16 packages checked, 135 representative assets checked.
- [ ] If higher-accuracy helpers are added, assert the manifest counts for RoomSetpieceKit04.
- [ ] If runtime catalog helpers are added, assert every listed prefab is `visual_only`.
- [ ] If prefab safety helpers are added, assert representative prefabs have no colliders, rigidbodies, autonomous audio sources, gameplay controllers, damage scripts, pickups, objectives, doors, lifts, bridges, transitions, hitboxes, nav agents, or AI controllers.

## Showcase And Scene Validation

- [ ] Update `V0SceneBuilder` to place representative visuals under `Sidecar Quarantine Showcase - <LevelXX>`.
- [ ] Place RoomSetpieceKit04 assets only as presentation shells, not route blockers, hazards, doors, lifts, bridges, transitions, interactables, walkable geometry, objective targets, or autonomous audio/VFX.
- [ ] Update `V0LevelValidator` required names after placement names are final.
- [ ] Rebuild scenes through the existing Unity menu flow.
- [ ] Run `Project Tools/Validate v0 Levels`.
- [ ] Confirm every showcase root has zero colliders, zero rigidbodies, zero autonomous audio sources, no gameplay authority scripts, and no route blockers.

## Manual Unity Lookdev QA

- [ ] Level01: inspect `RSK04BoilerChamberWallBayA` from player-height view and confirm it does not narrow the first route.
- [ ] Level02: inspect `RSK04PressureVaultDoorAlcoveA` and confirm it does not read as an active door, lock, exit, or transition.
- [ ] Level03: inspect `RSK04CatwalkBalconyModuleA` and confirm it does not imply a reachable route or block combat sightlines.
- [ ] Level04: inspect `RSK04PipeGalleryCeilingClusterA` and confirm overhead density does not reduce camera clarity or headroom readability.
- [ ] Level05: inspect `RSK04RegulatorCoreMachineryA` and confirm it does not hide boss-route mechanics, pickups, exits, signs, or enemy telegraphs.
- [ ] Confirm alternate candidates are used only if the primary placements overcrowd a level.
- [ ] Confirm all visual review is performed in Unity only.

## Rollback Validation

- [ ] Remove the new package reference from `Packages/manifest.json` in a throwaway rollback check.
- [ ] Remove the matching validator entry, showcase placements, and required names.
- [ ] Let Unity re-resolve packages and compile.
- [ ] Rebuild generated scenes if showcase references were added.
- [ ] Confirm generated scenes do not retain `com.brassworks.sidecar.room-setpiece-kit04` or `RSK04_` references.
- [ ] Confirm `git status --short` shows no unexpected edits outside planned main-lane files.

## Pass Criteria

- [ ] Package-specific static validation stays at 0 errors and 0 warnings.
- [ ] `SidecarQuarantineImportValidator` passes with 16 packages and 135 representative assets if the proposed inventory is used on top of the v0.1.45 baseline.
- [ ] `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild.
- [ ] Manual Unity lookdev confirms every RoomSetpieceKit04 placement is readable and quarantine-safe.
- [ ] Rollback remains package-reference removal plus validator/showcase cleanup; no package-root edits are needed.
