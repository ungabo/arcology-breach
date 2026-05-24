# Brassworks Breach Steam FX Set 06

Package name: `com.brassworks.sidecar.steam-fx-set06`

This is an isolated sidecar package for high-impact steampunk atmosphere effects. It is import-safe and visual-only: no gameplay scripts, colliders, rigidbodies, cameras, scene files, or audio assets are required by the runtime prefabs.

## Contents

- 20 presentation-only effect prefabs under `Runtime/Prefabs`
- 12 VFX materials under `Runtime/Materials`
- 20 PNG preview cards under `Runtime/Textures/PreviewCards`
- Package manifest/catalog JSON under `Documentation~/Manifest` and `Runtime/Metadata`
- Editor-only generator and validator under `Editor`

## Placement

Place prefabs under a scene object named `VisualOnly_SteamFXSet06` or another art-owned visual grouping. ParticleSystem components are configured for presentation only and do not provide gameplay authority, damage, hit detection, physics, audio, or camera behavior.

## Validation

Use an isolated Unity validation project or add the package as a local dependency in a disposable project, then run:

```powershell
Unity.exe -batchmode -quit -projectPath "<package-root>\ValidationProject~" -executeMethod BrassworksBreach.SteamFXSet06.Editor.SteamFXSet06Generator.GenerateRenderValidate
```
