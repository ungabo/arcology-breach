# Brassworks Breach Objective Props Set 02

Unity-only sidecar asset package for high-readability steampunk objective props and interactable visual candidates.

## Contents

- 24 visual-only prefabs across keyed locks, valve panels, lift call stations, pressure regulators, secret caches, bridge/door actuators, and Governor override machinery.
- Reusable procedural Unity mesh assets and material lookdev swatches.
- Package-local catalog and manifest JSON under `Runtime/Metadata` and `Documentation~/Manifest`.
- Editor generation, preview rendering, and package validation commands.

## Safety Contract

- No gameplay authority.
- No autonomous audio.
- Generated prefabs omit colliders, rigidbodies, cameras, lights, particle systems, and gameplay scripts.
- The only runtime component is passive identity metadata for intake review.

## Unity Commands

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\__MY APPS\Unity Doom\AssetPacks\BrassworksBreach.ObjectivePropsSet02\ValidationProject~' -executeMethod BrassworksBreach.ObjectivePropsSet02.Editor.ObjectivePropsSet02PreviewRenderer.RenderPreviewSet
```

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\__MY APPS\Unity Doom\AssetPacks\BrassworksBreach.ObjectivePropsSet02\ValidationProject~' -executeMethod BrassworksBreach.ObjectivePropsSet02.Editor.ObjectivePropsSet02Validation.ValidateGeneratedAssets
```
