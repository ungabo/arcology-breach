# Objective Interactables Set 05 Validation Summary

Generated: 2026-05-24

## Unity Generation And Validation

Command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -quit -projectPath 'D:\__MY APPS\Unity Doom\AssetPacks\BrassworksBreach.ObjectiveInteractablesSet05\ValidationProject~' -executeMethod BrassworksBreach.ObjectiveInteractablesSet05.Editor.ObjectiveInteractablesSet05Generator.GenerateRenderValidate -logFile 'D:\__MY APPS\Unity Doom\Documentation\AssetProduction\V0_1_49_ObjectiveInteractablesSet05\UnityGenerateRenderValidate_OIS05_v0.1.49.log'
```

Result:

```text
OIS05_UNITY_VALIDATION_PASS v0.1.49 prefabs=30 materials=18 meshes=12 renderers=639 previews=32
```

## Cheap Static Checks

```text
JSON_PARSE_PASS files=7
PREFAB_FORBIDDEN_COMPONENT_RG_PASS matches=0
SIDECAR_VALIDATOR_STATUS status=pass errors=0 warnings=0 packages=1
CHANGED_PATH_SCOPE_PASS checked=429 bad=0
```

Broad content `rg` for `OIS05|ObjectiveInteractablesSet05|objective-interactables-set05` found one tracked, pre-existing coordination reference in `Documentation/PARALLEL_WORKSTREAM_STATUS.md`. That file was not edited in this package task. Changed/untracked path scope stayed inside the assigned roots.

## Runtime Contract Checked

- No prefab text references to `MonoBehaviour`, `m_Script`, `Collider`, `Rigidbody`, `AudioSource`, `ParticleSystem`, `Camera:`, or `Light:`.
- Unity validation loaded all generated prefabs and counted zero runtime MonoBehaviours, colliders, rigidbodies, audio sources, particle systems, cameras, and lights.
- Sidecar package validator reported `status=pass`, `errors=0`, `warnings=0`.

## Evidence Files

- `Documentation/AssetProduction/V0_1_49_ObjectiveInteractablesSet05/UnityGenerateRenderValidate_OIS05_v0.1.49.log`
- `Documentation/AssetProduction/V0_1_49_ObjectiveInteractablesSet05/UnityValidationReport_ObjectiveInteractablesSet05_v0.1.49.json`
- `Documentation/QA/V0_1_49_ObjectiveInteractablesSet05ImportReadiness/SidecarValidator_ObjectiveInteractablesSet05_v0.1.49.json`
