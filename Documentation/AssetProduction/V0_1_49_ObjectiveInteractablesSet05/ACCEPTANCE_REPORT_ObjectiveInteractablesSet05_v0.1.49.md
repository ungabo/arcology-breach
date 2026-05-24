# Objective Interactables Set 05 Acceptance Report

Generated: 2026-05-24T17:41:11-04:00

Status: PASS

## Counts

- Prefabs: 30
- Materials: 18
- Reusable meshes: 12
- Renderer components: 639
- Preview PNGs: 32

## Families

- pressure_levers: 3
- keyed_locks: 3
- crank_panels: 3
- fuse_boxes: 3
- breaker_gauges: 3
- valve_routing_puzzles: 3
- boss_override_terminals: 3
- lift_call_stations: 3
- pickups: 3
- objective_signage: 3

## Runtime Safety

- Visual-only package.
- No gameplay authority, inventory, trigger logic, damage, door state, lift state, boss state, input, or autonomous audio scripts.
- Runtime prefabs omit MonoBehaviours, colliders, rigidbodies, audio sources, particle systems, cameras, and lights.
- Preview lighting and cameras are transient editor-scene objects only.

## Validation Evidence

- Unity command: `Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05/ValidationProject~ -executeMethod BrassworksBreach.ObjectiveInteractablesSet05.Editor.ObjectiveInteractablesSet05Generator.GenerateRenderValidate`
- Unity result marker: `OIS05_UNITY_VALIDATION_PASS v0.1.49 prefabs=30 materials=18 meshes=12 renderers=639 previews=32`
- Static sidecar validator command: `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1 -ProjectPath "D:/__MY APPS/Unity Doom" -PackageNamePattern "BrassworksBreach.ObjectiveInteractablesSet05" -Json`

## Findings

- None.
