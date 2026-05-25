# CAML10 Isolated Unity Corridor Assembly Lookdev

This folder owns a small, render-only Unity project used to assemble accepted or completed sidecar families into non-shipping corridor concept renders. It references the source sidecar packages read-only through the nested Unity project manifest and writes final images/reports only to the assigned CAML10 documentation roots.

Run command:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe" -batchmode -quit -projectPath "D:\__MY APPS\Unity Doom\Documentation\AssetProduction\V0_1_55_CorridorAssemblyLookdev10\UnityLookdevProject" -executeMethod CorridorAssemblyLookdev10Renderer.RenderBatch -logFile "D:\__MY APPS\Unity Doom\Documentation\AssetProduction\V0_1_55_CorridorAssemblyLookdev10\CAML10_UnityRender.log"
```

The script builds temporary in-memory scenes and does not create game-build scenes.
