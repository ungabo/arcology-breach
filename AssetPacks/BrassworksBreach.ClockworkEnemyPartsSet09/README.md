# Brassworks Breach Clockwork Enemy Parts Set 09

Unity-only sidecar package for a full clockwork mechanical enemy parts family. The pack delivers three named archetype sets rather than a single monster: a small skitter unit, a humanoid boiler brute, and a wall/ceiling sentry.

## Visual-Only Contract

- Runtime assets are mesh/material/texture/prefab/metadata visuals only.
- No gameplay scripts, colliders, rigidbodies, cameras, scenes, audio, animator controllers, animation clips, skeletons, or skinned meshes are authored by this package.
- Prefabs include named `SOCK_*` transforms for future rigging, VFX, and integration ownership.
- Archetype preview prefabs are visual contact assemblies, not gameplay-ready actors.

## Contents

- `Runtime/Meshes`: generated reusable mesh assets.
- `Runtime/Materials`: generated lookdev material assets.
- `Runtime/Textures`: deterministic procedural PNG base maps.
- `Runtime/Prefabs`: skitter, brute, sentry, shared, and archetype preview prefabs.
- `Runtime/Metadata`: catalog of archetype sets, socket expectations, and deferred rigging dependencies.
- `Documentation~/Manifest`: generated sidecar package manifest.
- `Samples~/PreviewScene`: preview/import notes.

## Generation

Run from the package validation project:

```powershell
& "C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe" -batchmode -projectPath "D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.ClockworkEnemyPartsSet09/ValidationProject~" -executeMethod BrassworksBreach.ClockworkEnemyPartsSet09.Editor.ClockworkEnemyPartsSet09Generator.GenerateAllAndRenderPreview -quit
```

Preview PNGs are written under `Documentation/ConceptRenders/V0_1_54_ClockworkEnemyPartsSet09`.
