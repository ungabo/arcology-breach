# Objective Interactables Set 05 Import Readiness

Package: `AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05`
UPM name: `com.brassworks.sidecar.objective-interactables-set05`
Version: `0.1.49`

## Current Status

Ready for primary-lane quarantine import after static validator review and visual preview review.

- Package manifest: `AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05/package.json`
- Package-local manifest: `AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05/Documentation~/Manifest/OIS05_ObjectiveInteractablesSet05_Manifest_v0.1.49-p001.json`
- Runtime catalog: `AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05/Runtime/Metadata/OIS05_ObjectiveInteractablesSet05_Catalog_v0.1.49.json`
- Preview renders: `Documentation/ConceptRenders/V0_1_49_ObjectiveInteractablesSet05/`
- Unity validation report: `Documentation/AssetProduction/V0_1_49_ObjectiveInteractablesSet05/UnityValidationReport_ObjectiveInteractablesSet05_v0.1.49.json`

## Safety Contract

- Visual-only import.
- Runtime prefabs contain no gameplay scripts, runtime MonoBehaviours, colliders, rigidbodies, audio sources, particle systems, cameras, or lights.
- Main-lane gameplay triggers, prompts, pickups, boss override logic, lift calls, doors, and puzzle state must remain authoritative outside this package.

## Exact Main Manifest Step

In the main project `Packages/manifest.json`, add this dependency inside `dependencies`:

```json
"com.brassworks.sidecar.objective-interactables-set05": "file:../AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05"
```

Do not edit the sidecar package while adding the main manifest dependency.

## Exact Lockfile Expectation

After Unity resolves packages, `Packages/packages-lock.json` should contain:

```json
"com.brassworks.sidecar.objective-interactables-set05": {
  "version": "file:../AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05",
  "depth": 0,
  "source": "local",
  "dependencies": {}
}
```

If Unity rewrites path separators, accept Unity's normalized format but keep `source` as `local` and `depth` as `0`.

## Required Validators After Import

Run the sidecar static validator before scene placement:

```powershell
.\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1 -ProjectPath "D:\__MY APPS\Unity Doom" -PackageNamePattern "BrassworksBreach.ObjectiveInteractablesSet05" -Json
```

After scene placement, run the main validation path for the receiving branch:

```powershell
.\Tools\RunV0RouteAudit.ps1 -ProjectPath "D:\__MY APPS\Unity Doom" -UnityPath "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe"
.\Tools\RunV0BuildMatrix.ps1 -ProjectPath "D:\__MY APPS\Unity Doom" -UnityPath "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe"
```

## Scene Placement Rules

1. Import into a quarantine branch or duplicate integration scene first.
2. Create one parent container per target scene: `VisualOnly_ObjectiveInteractablesSet05`.
3. Place package prefabs only from `Packages/com.brassworks.sidecar.objective-interactables-set05/Runtime/Prefabs/`.
4. Group placements under family children such as `OIS05_PressureLevers`, `OIS05_KeyedLocks`, `OIS05_FuseBoxes`, `OIS05_ValveRouting`, and `OIS05_ObjectiveSignage`.
5. Do not add colliders, trigger colliders, rigidbodies, scripts, audio sources, cameras, lights, or particle systems to package prefab instances.
6. If gameplay authority is needed, create separate main-lane objects named with an `AUTH_` prefix and keep them siblings of visual instances, not children of package prefab instances.
7. If a visual needs per-scene transforms or material overrides, create a main-lane prefab variant outside `AssetPacks/` and keep the original package untouched.
8. Re-run route audit and build matrix before promoting beyond quarantine.

## Rollback

Remove the main `Packages/manifest.json` dependency, let Unity remove the lock entry, and delete scene instances under `VisualOnly_ObjectiveInteractablesSet05`. The sidecar package root can remain untouched for later review.
