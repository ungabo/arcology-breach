# ENV Recovery02 Modular Corridor Kit Unity Proof Report

Status: Fail: Unity proof generated, but at least one required technical/module gate failed.
Date/time: 2026-05-24 01:20:49 -04:00
Unity version: 6000.4.6f1
Subject: modular steampunk corridor kit proof beyond Recovery01 mood render
Tool lane: Unity editor/batchmode, C# procedural geometry, temporary in-memory scenes, Camera plus RenderTexture JPG export
Exact Unity command used: `"C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe" -batchmode -quit -projectPath "D:\__MY APPS\Unity Doom" -executeMethod UnityModularCorridorKitProofRenderer.RenderBatch -logFile "D:\__MY APPS\Unity Doom\Logs\env-recovery02-modular-corridor-kit-proof.log"`
Gameplay-write policy: no playable scenes, `V0SceneBuilder.cs`, `V0LevelValidator.cs`, build docs, ledger docs, or concept render index docs were modified by this renderer.

## Recovery02 Fix Pass

Fail: the rejected hot-magenta shader-error output was addressed by forcing this renderer onto batchmode-safe built-in shaders and adding a magenta-pixel gate.
Shader path: Recovery02 fix pass: batchmode-safe built-in shaders only; URP shader names are intentionally not used in this renderer.
Opaque texture shader: `Unlit/Texture`; transparent texture shader: `Unlit/Transparent`; color shader: `Unlit/Color`; transparent color shader: `Sprites/Default`.
Magenta gate: max individual render 100.000%, contact sheet 41.344%, threshold < 0.5%.

## Outputs

| File | Purpose | Dimensions |
| --- | --- | ---: |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_modular_wall_bay_unity_proof.jpg` | Modular Wall Bay proof render | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_pressure_door_module_unity_proof.jpg` | Pressure Door Module proof render | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_pipe_canopy_unity_proof.jpg` | Ceiling Pipe Canopy proof render | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_floor_wetness_unity_proof.jpg` | Wet Floor Slab proof render | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_assembled_corridor_slice_unity_proof.jpg` | Assembled Corridor Slice proof render | 1920x1080 |
| `Documentation/ConceptRenders/CONTACTSHEET_ENV_Recovery02_modular_corridor_kit_unity_proof.jpg` | Contact sheet for all Recovery02 module views | 2400x1700 |
| `Documentation/AssetProduction/EnvironmentLookdev/env_recovery02_modular_corridor_kit_metrics.json` | Machine-readable metrics | n/a |

## Module List

| Module | Status | Gameplay integration readiness | Notes |
| --- | --- | --- | --- |
| Modular Wall Bay | Pass | Prototype only: useful shape/material proof, not ready for playable integration. | 4m-ish wall bay uses brick field, iron kick plate, three reusable rivet strips, rail run, gauge, valve, and a vent cluster. |
| Pressure Door Module | Pass | Prototype only: useful shape/material proof, not ready for playable integration. | Door module has blackened iron slabs, brass frame/seal, hazard plate, lock socket, state lamps, gauge, valve, rivet grid, and sill vent. |
| Ceiling Pipe Canopy | Pass | Prototype only: useful shape/material proof, not ready for playable integration. | Ceiling canopy groups pipe runs, cross manifolds, brass clamps, two caged lamps, and a pressure-relief vent cluster. |
| Wet Floor Slab | Pass | Prototype only: useful shape/material proof, not ready for playable integration. | Floor slab proves wet stone, tile seams, brass threshold, drain grates, oil/scorch decals, and broken amber reflections. |
| Assembled Corridor Slice | Pass | Prototype only: useful shape/material proof, not ready for playable integration. | Assembled slice combines two wall bays, two floor slabs, canopy pipes, rails, lamps, door module, gauge/valve detail, decals, reflections, and vents. |

## Material Sources Used

- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_WetOilDarkStone_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_SootBrick_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_BlackenedRivetedIron_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_AgedBrass_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_CopperPipe_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_AmberGlass_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_CreamEnamelGauge_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_HazardEnamel_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_LeatherBellows_*_2048.png`
- `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_ScorchOilDecalAtlas_BaseColor/Alpha_2048.png`

## Count Metrics

| Check | Result |
| --- | ---: |
| Texture files loaded | 29 / 29 |
| Rendered individual images | 5 |
| Wall bay module instances | 5 |
| Wet floor slab instances | 3 |
| Ceiling pipe canopy modules | 1 |
| Caged gaslights | 4 |
| Pressure door modules | 2 |
| Catwalk rail modules | 3 |
| Rivet strips | 7 |
| Gauges | 4 |
| Valves | 3 |
| Steam vent clusters | 6 |
| Pipe segments | 30 |
| Rivets/bolts | 222 |
| Oil/scorch decals | 4 |
| Wet highlight patches | 3 |

## Image Readability

| Image | Average luminance | Near-black % | Bright % | Warm highlight % | Saturated accent % | Magenta % | Empty check | No magenta check |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | --- | --- |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_modular_wall_bay_unity_proof.jpg` | 0.249 | 0.0 | 0.0 | 0.0 | 86.4 | 86.306 | fail | fail |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_pressure_door_module_unity_proof.jpg` | 0.257 | 0.0 | 0.0 | 0.0 | 89.5 | 89.431 | fail | fail |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_pipe_canopy_unity_proof.jpg` | 0.197 | 0.0 | 0.0 | 0.0 | 66.7 | 66.644 | fail | fail |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_floor_wetness_unity_proof.jpg` | 0.227 | 0.0 | 0.0 | 0.0 | 78.1 | 78.052 | fail | fail |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery02_assembled_corridor_slice_unity_proof.jpg` | 0.285 | 0.0 | 0.0 | 0.0 | 100.0 | 100.000 | fail | fail |
| `Documentation/ConceptRenders/CONTACTSHEET_ENV_Recovery02_modular_corridor_kit_unity_proof.jpg` | 0.152 | 37.4 | 4.2 | 3.1 | 46.0 | 41.344 | fail | fail |

## Concept Gates

| Gate | Status | Evidence |
| --- | --- | --- |
| Scope | Pass | New renderer and ENV Recovery02 outputs only; no gameplay scenes, V0SceneBuilder, V0LevelValidator, build docs, ledger docs, or concept index docs are written. |
| Unity-only method | Pass | All modules are Unity primitive/procedural geometry rendered by Unity cameras into RenderTextures. |
| Staged material sources | Pass | 29/29 required FinalMaterialsV1 texture files loaded. |
| Required module coverage | Pass | Wall bay, floor slab, pipe canopy, gaslight, pressure door, rail, rivet strips, gauges/valves, and steam vent clusters are all represented. |
| Dense north-star silhouette | Pass | 30 pipe segments, 222 rivets, 4 lamps, 4 wear decals, 3 wet highlights. |
| Readable renders | Fail | At least one individual JPG failed the non-empty luminance/highlight check. |
| No magenta shader-error output | Fail | Magenta shader-error pixels must stay under 0.5%; max individual 100.000%, contact sheet 41.344%. |
| Gameplay integration readiness | Partial | Modules are named and separated as future asset candidates, but have no authored pivots, prefabs, collision, UV2/lightmap proof, LODs, occlusion setup, or snap metadata. |
| Production art readiness | Fail | This is still procedural proof geometry, not final modular kit art. |
| Human north-star review | Fail until reviewed | Only art review can accept whether the modules match the north-star brassworks corridor mood. |

## Gameplay Integration Readiness

Not ready for gameplay integration. The proof now separates the corridor language into reusable-looking Unity-authored modules, which is a step beyond Recovery01, but these are not prefabs or shipping assets. They still need authored pivots, snap rules, colliders, lightmap UVs, LODs, occlusion/static flags, material-instance policy, prefab variants, and validation in a playable corridor width before the main thread can integrate them.

## Frank Visual Read

The Recovery02 proof should read closer to a modular kit than Recovery01: a distinct wall bay, floor slab, ceiling pipe canopy, pressure door, caged lamps, rail/rivet strips, gauges/valves, and vent clusters are visible as reusable pieces. It still fails production-art readiness because the modules are primitive procedural geometry with no authored bevels, UV layout, collision, or prefab metadata. Human review should judge whether the blackened brick/iron, brass/copper density, amber light, wet floor, and pressure machinery are now close enough to guide the real corridor kit build.
