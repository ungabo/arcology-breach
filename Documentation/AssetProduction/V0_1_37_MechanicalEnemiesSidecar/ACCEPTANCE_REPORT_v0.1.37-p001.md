# V0.1.37 Mechanical Enemies Sidecar Acceptance Report

Pack: Mechanical Enemies Sidecar  
Version: `v0.1.37`  
Build ID: `p001`  
Unity version target: `6000.4.6f1`  
Package root: `AssetPacks/BrassworksBreach.MechanicalEnemies`  
Package name: `com.brassworks.sidecar.mechanical-enemies`

## Scope Gate

Status: pass

- The package is isolated under one sidecar root.
- It does not touch primary scenes, scripts, project settings, package manifests, or existing art staging folders.
- The runtime output is visual-only mechanical enemy content.
- The only code is editor tooling used to generate and render package assets.
- No gameplay behavior, AI, spawning, combat, route, damage, or progression logic is included.

## Content Gate

Status: pass

Enemy families covered:

- Saw Scrapper: low fast saw/claw unit, 1.55 m.
- Rivet Lancer: tall lance and cyan coil unit, 1.90 m.
- Bulwark Furnace: wide shield and hammer blocker, 2.15 m.
- Warden Sentinel: command elite with pincer/gavel/twin coils, 2.35 m.
- Foundry Overseer Bust: miniboss bust/blockout with boss tools, 2.75 m.

Each generated prefab is designed to include:

- `Root`, `Hips`, `Body`, `Head`, `LeftArm`, `RightArm`, `LeftLeg`, `RightLeg`, `WeaponMounts`, `VFXAnchors`, and `Hitboxes`.
- Amber eye or furnace-light sockets.
- Readable weapon/tool silhouette parts.
- Translucent hitbox marker children with trigger colliders for later gameplay review.
- Scale-note child naming for corridor compatibility.

Generated runtime assets:

- 5 prefabs under `Runtime/Prefabs`.
- 10 materials under `Runtime/Materials`.
- 7 procedural mesh assets under `Runtime/Meshes`.
- 6 Unity-rendered preview PNGs under `Documentation/ConceptRenders/V0_1_37_MechanicalEnemiesSidecar`.

## Import Gate

Status: clean disposable Unity smoke passed; primary quarantine not yet run

The package was imported by local file path into a disposable Unity project matching `6000.4.6f1`. The generator and preview renderer completed successfully at `2026-05-24 14:02:46 -04:00` without C# compile errors or log errors. The package is intentionally not wired into the primary game manifest during this sidecar pass. Primary quarantine import should be done by a main-lane integration task using either a local package reference or a copied isolated quarantine root.

Completed smoke:

1. Parsed `package.json` and manifest JSON.
2. Scanned package and v0.1.37 documentation folders for conflict markers.
3. Ran a C# structure scan for required generator methods and balanced braces.
4. Imported the local package into a disposable Unity project.
5. Ran `BrassworksBreach.MechanicalEnemies.Editor.MechanicalEnemySidecarGenerator.GenerateAllAndRenderPreview`.
6. Confirmed five prefab assets, ten material assets, and seven mesh assets exist under the package runtime folders.
7. Confirmed six preview PNGs exist and have expected nonzero dimensions.
8. Scanned the successful Unity smoke log for compile errors, exceptions, missing references, and null references.

Remaining primary-lane smoke:

1. Import or reference `AssetPacks/BrassworksBreach.MechanicalEnemies` in a disposable Unity project matching `6000.4.6f1`.
2. Run `Brassworks Breach/Sidecars/Mechanical Enemies/Generate v0.1.37 Enemy Package`.
3. Confirm five prefab assets, ten material assets, and seven mesh assets exist under the package runtime folders.
4. Open each prefab in Prefab Mode and check for missing scripts, missing materials, and missing meshes.
5. Run `Brassworks Breach/Sidecars/Mechanical Enemies/Render v0.1.37 Preview PNGs`.
6. Confirm preview PNGs are written to `Documentation/ConceptRenders/V0_1_37_MechanicalEnemiesSidecar`.

## Risks

- Prefabs are generated assets; they are not promoted directly into main gameplay.
- Hitbox markers are visual review aids and must not be treated as final combat tuning.
- The preview renderer uses Unity lighting and render textures, but not a hand-authored scene.
- Main-project quarantine import still needs to confirm the package does not cause dependency drift when referenced by the primary project.

Decision: ready for primary quarantine import.
