# Objective Props Set 02 Acceptance Report

Generated: 2026-05-24T16:43:45-04:00

Status: PASS

## Counts

- Prefabs: 24
- Materials: 17
- Reusable meshes: 11
- Renderer components: 664

## Families

- keyed_locks: 4
- valve_panels: 4
- lift_call_stations: 3
- pressure_regulators: 3
- secret_cache_containers: 3
- bridge_door_actuators: 3
- governor_override_devices: 4

## Runtime Safety

- Visual-only package.
- No gameplay authority, inventory, trigger logic, damage, door state, bridge state, hoist state, input, or autonomous audio scripts.
- Colliders, rigidbodies, audio sources, particle systems, cameras, and lights are omitted from generated prefabs.
- Passive identity metadata component is the only runtime MonoBehaviour.
- Preview scenes are transient editor scenes and are not saved.

## Findings

- None.

## Validation Evidence

- Unity render command: `Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.ObjectivePropsSet02/ValidationProject~ -executeMethod BrassworksBreach.ObjectivePropsSet02.Editor.ObjectivePropsSet02PreviewRenderer.RenderPreviewSet`
- Unity render result: `BB_OBJECTIVE_PROPS_SET02_RENDER_PASS v0.1.42 files=25`
- Unity validation command: `Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.ObjectivePropsSet02/ValidationProject~ -executeMethod BrassworksBreach.ObjectivePropsSet02.Editor.ObjectivePropsSet02Validation.ValidateGeneratedAssets`
- Unity validation result: `BB_OBJECTIVE_PROPS_SET02_VALIDATION_PASS v0.1.42 prefabs=24 materials=17 meshes=11 renderers=664`
- Sidecar validator command: `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1 -ProjectPath "D:/__MY APPS/Unity Doom" -PackageNamePattern "BrassworksBreach.ObjectivePropsSet02" -Json`
- Sidecar validator result: `pass`, `errors=0`, `warnings=0`; see `SidecarValidator_ObjectivePropsSet02_v0.1.42.json`.

## Known Risks

- Procedural meshes are strong lookdev candidates, not final authored production meshes.
- Materials are solid proxy materials without texture maps, grime masks, decals, or normals.
- Gameplay prompt placement, collision, door/hoist/bridge authority, and state machines remain primary-lane work.
