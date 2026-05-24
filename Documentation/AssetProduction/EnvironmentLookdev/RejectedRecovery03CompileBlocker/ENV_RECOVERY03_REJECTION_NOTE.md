# ENV Recovery03 Rejection Note

Date/time: 2026-05-24 01:45 -04:00

The Unity-only ENV Recovery03 renderer compiled and produced four individual JPGs plus a contact sheet, report, and metrics, but the output failed the shader-magenta acceptance gate. The contact sheet visually confirms hot-magenta shader-error materials, so the editor renderer has been quarantined out of `Assets/`.

Batchmode command used:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod UnityEnvironmentRecovery03ProofRenderer.RenderBatch -logFile 'D:\__MY APPS\Unity Doom\Logs\env-recovery03-unity-environment-lookdev.log'
```

Quarantined files:

- `Documentation/AssetProduction/EnvironmentLookdev/RejectedRecovery03CompileBlocker/UnityEnvironmentRecovery03ProofRenderer.cs.disabled`
- `Documentation/AssetProduction/EnvironmentLookdev/RejectedRecovery03CompileBlocker/UnityEnvironmentRecovery03ProofRenderer.cs.meta.disabled`

Failure evidence:

- `Documentation/AssetProduction/EnvironmentLookdev/ENV_RECOVERY03_UNITY_ENVIRONMENT_LOOKDEV_REPORT.md`
- `Documentation/AssetProduction/EnvironmentLookdev/env_recovery03_unity_environment_lookdev_metrics.json`
- `Documentation/ConceptRenders/CONTACTSHEET_ENV_Recovery03_unity_environment_lookdev_proof.jpg`

