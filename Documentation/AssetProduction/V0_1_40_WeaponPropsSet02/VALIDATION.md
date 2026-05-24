# Weapon Props Set 02 Validation

## Commands

Generate assets and previews through the isolated package validation project:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\__MY APPS\Unity Doom\AssetPacks\BrassworksBreach.WeaponPropsSet02\ValidationProject~' -executeMethod BrassworksBreach.WeaponPropsSet02.Editor.WeaponPropsSet02PreviewRenderer.RenderPreviewSet -logFile 'D:\__MY APPS\Unity Doom\Logs\weapon-props-set02-unity.log'
```

Run sidecar validation for only this package:

```powershell
& 'D:\__MY APPS\Unity Doom\Tools\SidecarValidation\Test-SidecarAssetPacks.ps1' -ProjectPath 'D:\__MY APPS\Unity Doom' -PackageNamePattern 'BrassworksBreach.WeaponPropsSet02'
```

## Current Status

- Unity isolated import/generate/render: passed on 2026-05-24 at 15:54:26 -04:00.
- Sidecar validation: passed on 2026-05-24 at 15:55:01 -04:00.
- Sidecar validation result: 0 errors, 0 warnings.
- Documented warnings: none.

## Evidence

- Unity log: `Logs/weapon-props-set02-unity.log`
- Generator marker: `BB_WEAPON_PROPS_SET02_GENERATE_PASS v0.1.40 prefabs=16 materials=12 meshes=4`
- Render marker: `BB_WEAPON_PROPS_SET02_RENDER_PASS v0.1.40`
- Sidecar validator summary: `Packages checked: 1`, `Errors: 0`, `Warnings: 0`
