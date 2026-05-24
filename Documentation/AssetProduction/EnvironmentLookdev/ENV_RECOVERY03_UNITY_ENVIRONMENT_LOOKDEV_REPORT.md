# ENV Recovery03 Unity Environment Lookdev Report

Status: Technical fail: proof generated, but at least one render/material gate failed.
Date/time: 2026-05-24 01:45:01 -04:00
Unity version: 6000.4.6f1
Subject: Unity-only replacement for failed ENV Recovery02 modular corridor proof
Tool lane: Unity editor batchmode, temporary in-memory scenes, Camera plus RenderTexture JPG export
Batchmode command entrypoint: `UnityEnvironmentRecovery03ProofRenderer.RenderBatch`
Exact Unity command used: `"C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe" -batchmode -quit -projectPath "D:\__MY APPS\Unity Doom" -executeMethod UnityEnvironmentRecovery03ProofRenderer.RenderBatch -logFile "D:\__MY APPS\Unity Doom\Logs\env-recovery03-unity-environment-lookdev.log"`
Unity log path: `Logs/env-recovery03-unity-environment-lookdev.log`
Gameplay-write policy: no generated gameplay scenes, `V0SceneBuilder`, `V0LevelValidator`, gameplay scripts, build settings, or shared status docs were edited by this renderer.

## Outputs

| File | Purpose | Dimensions |
| --- | --- | ---: |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery03_modular_wall_bay_unity_proof.jpg` | Modular wall bay proof with brick, iron ribs, brass rail, gauge, valve, steam, and wet floor | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery03_pipe_canopy_gauge_cluster_unity_proof.jpg` | Pipe canopy and gauge-cluster proof with overhead brass/copper pipework and caged gaslights | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery03_pressure_door_module_unity_proof.jpg` | Pressure vault door module proof with circular seals, spokes, gauges, valves, lamps, vents, and rivets | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery03_assembled_corridor_slice_unity_proof.jpg` | Assembled corridor slice proof combining wall bays, pipe canopy, rail, wet floor, and pressure door | 1920x1080 |
| `Documentation/ConceptRenders/CONTACTSHEET_ENV_Recovery03_unity_environment_lookdev_proof.jpg` | Contact sheet for all Recovery03 environment views | 2400x1700 |
| `Documentation/AssetProduction/EnvironmentLookdev/env_recovery03_unity_environment_lookdev_metrics.json` | Machine-readable metrics and gates | n/a |

## Methodology

- Created temporary Unity editor scenes in memory and discarded them after rendering.
- Used URP Lit materials loaded from the staged `FinalMaterialsV1` texture PNGs, including ORM conversion for metallic/smoothness and occlusion.
- Rebuilt the failed Recovery02 idea as four smaller readable module proofs: wall bay, pipe canopy/gauge cluster, pressure door, and assembled corridor.
- Added separate material roles for wet dark stone, soot brick, blackened iron, aged brass, copper pipe, amber glass, cream enamel gauges, hazard enamel, steam, wet reflections, and oil/scorch decals.
- Captured the result from Unity cameras into RenderTextures, wrote JPGs, and ran luminance plus magenta-pixel checks.

## Count Metrics

| Check | Result |
| --- | ---: |
| Texture files loaded | 31 / 31 |
| Material families used | 13 |
| Rendered individual images | 4 |
| Modular wall bay markers | 36 |
| Floor tile pieces | 145 |
| Iron ribs | 54 |
| Wall detail pieces | 40 |
| Ceiling pipe canopies | 4 |
| Pipe/rail segments | 126 |
| Gaslights | 9 |
| Gauges | 7 |
| Gauge tick marks | 84 |
| Valves | 6 |
| Pressure door modules | 2 |
| Rivets/bolts | 648 |
| Steam vent clusters | 7 |
| Steam puffs | 43 |
| Oil/scorch decals | 3 |
| Wet highlight patches | 12 |

## Image Readability

| Image | Average luminance | Near-black % | Bright % | Warm highlight % | Saturated accent % | Magenta % | Empty check | No magenta check |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | --- | --- |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery03_modular_wall_bay_unity_proof.jpg` | 0.164 | 43.7 | 0.0 | 0.0 | 55.0 | 54.643 | fail | fail |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery03_pipe_canopy_gauge_cluster_unity_proof.jpg` | 0.210 | 26.8 | 0.0 | 0.0 | 72.3 | 72.059 | fail | fail |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery03_pressure_door_module_unity_proof.jpg` | 0.259 | 9.0 | 0.0 | 0.0 | 90.3 | 90.102 | fail | fail |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery03_assembled_corridor_slice_unity_proof.jpg` | 0.145 | 51.5 | 0.0 | 0.0 | 48.1 | 47.992 | fail | fail |
| `Documentation/ConceptRenders/CONTACTSHEET_ENV_Recovery03_unity_environment_lookdev_proof.jpg` | 0.144 | 26.1 | 0.5 | 2.7 | 44.4 | 41.526 | pass | fail |

## Gates

| Gate | Status | Evidence |
| --- | --- | --- |
| Scope | Pass | Renderer writes only ENV Recovery03 editor lookdev proof outputs under Documentation/ConceptRenders and Documentation/AssetProduction/EnvironmentLookdev. |
| Unity-only render path | Pass | All images are temporary Unity editor scenes captured by Unity Camera.Render into RenderTextures. |
| Texture source load | Pass | 31 / 31 texture files loaded from FinalMaterialsV1. |
| Required module coverage | Pass | Renders include modular wall bay, pipe canopy/gauge cluster, pressure door module, and assembled corridor slice. |
| North-star material separation | Pass | Wet dark tile, soot brick, blackened iron, aged brass, copper pipes, amber glass, gauges, rivets, steam, oil/scorch decals, and warm lights are separately assigned. |
| Readable renders | Fail | At least one individual render failed the non-empty luminance/highlight check. |
| No magenta shader errors | Fail | Magenta-pixel detector exceeded 0.5%; shader/material path needs repair. |
| Production readiness | Fail | This is a Unity procedural lookdev proof, not authored modular meshes, prefab pivots, collision, LODs, UV2/lightmap proof, or shipping art. |
| Human north-star review | Pending | Human review is still required against the north-star steampunk corridor image. |

## Frank Visual Read

Recovery03 is intentionally narrower than the failed Recovery02 pass: it proves the corridor language through four focused Unity renders instead of trying to solve a whole kit at once. It should read closer to the north-star image because the values are darker, the materials are separated, the brass/copper pipe silhouette is denser, the pressure door is a clear destination, and warm gaslight plus steam/fog are present. It is still procedural Unity lookdev, not final authored AAA environment art.

## Next Art Steps

- Convert the accepted shapes into real modular prefabs with authored pivots, snap metadata, colliders, LODs, UV2/lightmap proof, and material-instance policy.
- Keep the component-first method: pressure door, wall bay, pipe canopy, rail, gauge, lamp, and wet floor should each get their own Unity proof before final assembly.
- Preserve the dark wet-stone/brass/gaslight/steam value range when the main build integrates this into playable spaces.
