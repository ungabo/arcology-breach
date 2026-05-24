# HP06 Unity Smoke Summary

Unity executable found:

```text
C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe
```

Commands run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File "D:\__MY APPS\Unity Doom\Documentation\AssetProduction\V0_1_50_HazardPropsSet06\GenerateHazardPropsSet06.ps1"
& "C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe" -batchmode -quit -createProject "D:\__MY APPS\Unity Doom\Documentation\QA\V0_1_50_HazardPropsSet06ImportReadiness\UnityImportSmokeProject" -logFile "D:\__MY APPS\Unity Doom\Documentation\QA\V0_1_50_HazardPropsSet06ImportReadiness\HP06_UnityCreateProject_v0.1.50.log"
```

Results:

- Sidecar static validation: PASS.
- Unity batchmode create-project smoke test: exit code 0.
- Unity package resolution/import smoke test: PASS. `packages-lock.json` resolved `com.brassworks.sidecar.hazard-props-set06` as a local package from `D:\__MY APPS\Unity Doom\AssetPacks\BrassworksBreach.HazardPropsSet06`; `HP06_UnityImportSmoke_v0.1.50.log` records package import for Runtime, Previews, Metadata, Meshes, Prefabs, and Materials.
- Temporary Unity validation project was removed after the smoke test to avoid committing generated Library/Temp noise; retained QA logs summarize the run.
- Main project `Assets`, `Packages/manifest.json`, scenes, build scripts, and existing docs outside assigned roots were not modified.
