# V0.1.44 Sidecar Import Readiness Plan

Generated: 2026-05-24

Purpose: give the main lane a concrete, Unity-only path for importing ObjectivePropsSet02 and SteamVFXSet02 after package review. This packet is documentation-only and leaves manifest edits, validator code, scene generation, and lookdev execution to the main integration lane.

## Current State

| Package | UPM name | Package version | Manifest evidence | Generated asset counts | Static validation | Main manifest |
| --- | --- | --- | --- | --- | --- | --- |
| ObjectivePropsSet02 | `com.brassworks.sidecar.objective-props-set02` | `0.1.42` | `OPS02_ObjectivePropsSet02_Manifest_v0.1.42-p001.json` | 24 prefabs, 17 materials, 11 meshes, 1 runtime script, 25 preview renders | pass, 0 errors, 0 warnings | absent |
| SteamVFXSet02 | `com.brassworks.sidecar.steam-vfx-set02` | `0.1.42-p001` | `BBSVFX02_SteamVFXSet02_Manifest_v0.1.42-p001.json` | 20 prefabs, 16 materials, 8 meshes, 1 runtime script, 2 preview PNGs | pass, 0 errors, 0 warnings | absent |

The current main lane already references 11 sidecar packages: feedback FX audio, corridor kit set 02, encounter enemy set 02, level dressing set 01, materials set 01, mechanical enemies, mechanical enemy visual set 01, steampunk weapons, steamworks level kit, weapon viewmodel set 03, and weapon props set 02. `SidecarQuarantineImportValidator` currently checks 11 packages and 81 representative assets.

## Static Validator Rerun

Commands rerun on 2026-05-24:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.ObjectivePropsSet02' -Json
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.SteamVFXSet02' -Json
```

Exact results:

- ObjectivePropsSet02: `status=pass`, `package_count=1`, `errors=0`, `warnings=0`, `packages=["BrassworksBreach.ObjectivePropsSet02"]`, `findings=[]`.
- SteamVFXSet02: `status=pass`, `package_count=1`, `errors=0`, `warnings=0`, `packages=["BrassworksBreach.SteamVFXSet02"]`, `findings=[]`.

## Import Sequence

1. Review `git status --short` and coordinate with parallel workers before touching `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/_Project/Editor/SidecarQuarantineImportValidator.cs`, `Assets/_Project/Editor/V0SceneBuilder.cs`, `Assets/_Project/Editor/V0LevelValidator.cs`, or generated scenes.
2. Add only these two local UPM references to `Packages/manifest.json`:

```json
"com.brassworks.sidecar.objective-props-set02": "file:../AssetPacks/BrassworksBreach.ObjectivePropsSet02",
"com.brassworks.sidecar.steam-vfx-set02": "file:../AssetPacks/BrassworksBreach.SteamVFXSet02",
```

3. Open Unity and allow one package resolution and script compile before editing showcase code.
4. Extend `SidecarQuarantineImportValidator` with two new `PackageCheck` entries using the representative paths in `ASSET_PATH_INVENTORY_v0.1.44.md`.
5. Run `Project Tools/Validate Sidecar Quarantine Imports`. If the full proposed v0.1.44 inventory is used, expected result shape is `packages=13 assets=102`.
6. Add `V0SceneBuilder.cs` showcase placements only after the import validator is green. Keep every instance under `Sidecar Quarantine Showcase - <LevelXX>`.
7. Update `V0LevelValidator.cs` to require the new showcase object names and maintain zero colliders, zero rigidbodies, zero autonomous audio sources, and minimum renderer count checks.
8. Rebuild generated scenes through the existing Unity menu flow, then run `Project Tools/Validate v0 Levels`.
9. Perform manual lookdev in Unity only. Do not use Blender for render, lookdev, socket, scale, or particle review.
10. Validate rollback by removing the two manifest references and corresponding validator/showcase changes in a throwaway check.

## Compile Batching

| Batch | Contents | Pass signal |
| --- | --- | --- |
| A | Manifest references plus `SidecarQuarantineImportValidator` additions | Unity compiles; import validator reports 13 packages and 102 assets if all proposed checks are used. |
| B | `V0SceneBuilder` quarantine placements and `V0LevelValidator` required names | Scene generator compiles; no live gameplay references are introduced. |
| C | Generated scene rebuild | `Project Tools/Validate v0 Levels` passes across Level01 through Level05. |
| D | Manual lookdev and rollback rehearsal | Objective readability, particle scale/timing, and rollback notes are captured. |

## Readiness Gates

| Gate | Required pass signal |
| --- | --- |
| Package resolution | `PackageManager.PackageInfo.FindForAssetPath` resolves each new package to the expected UPM name. |
| Package-local evidence | Each `Documentation~/Manifest` JSON exists, parses, and matches the expected package name and counts. |
| Static package hygiene | `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1` remains pass with 0 errors and 0 warnings for both roots. |
| Asset loadability | Representative prefabs, materials, meshes, metadata, and runtime identity scripts load through `AssetDatabase`. |
| Presentation safety | New showcase instances add no colliders, rigidbodies, autonomous audio sources, gameplay authority scripts, pickups, triggers, or level transitions. |
| Particle safety | Steam VFX prefabs remain visual-only; particle collision, trigger, and external-force modules remain disabled unless explicitly owned by gameplay. |
| Scene validation | `V0LevelValidator.ValidateProjectScenes()` passes after scene rebuild. |
| Rollback | Removing the two package refs and their showcase/validator entries restores the prior 11-package lane without missing references. |

## Decision

Proceed to main-lane quarantine import after review owner accepts this packet. The packages are ready for controlled visual import, but they are not promotion-ready gameplay interactables or tuned gameplay VFX.
