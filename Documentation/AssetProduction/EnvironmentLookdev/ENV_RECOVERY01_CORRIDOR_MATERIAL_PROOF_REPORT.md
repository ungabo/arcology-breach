# ENV Recovery01 Corridor Material Unity Proof Report

Status: Lookdev pass / production fail: Unity proof meets the requested material-direction checks, but is not final art.
Date/time: 2026-05-24 00:42:48 -04:00
Unity version: 6000.4.6f1
Subject: v0.0.95+ small steampunk corridor material/lookdev bay
Tool lane: Unity editor batchmode, temporary in-memory scene, Camera plus RenderTexture JPG export
Batchmode command entrypoint: `UnityCorridorMaterialProofRenderer.RenderBatch`
Playable-scene policy: no playable scene, Build Settings, or `V0SceneBuilder` edits were made by this renderer.

## Outputs

| File | Purpose | Dimensions |
| --- | --- | ---: |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery01_corridor_wide_unity_proof.jpg` | Player-height corridor proof toward pressure gate hint | 1920x1080 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery01_floor_wetness_unity_proof.jpg` | Low wet-stone floor/specular detail view | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery01_pressure_gate_detail_unity_proof.jpg` | Pressure-gate, gauge, lamp, and rivet detail view | 1600x1000 |
| `Documentation/ConceptRenders/CONTACTSHEET_ENV_Recovery01_corridor_material_unity_proof.jpg` | Contact sheet with hero/detail views and pass bars | 2200x1400 |
| `Documentation/AssetProduction/EnvironmentLookdev/env_recovery01_corridor_material_metrics.json` | Machine-readable render/material metrics | n/a |

## Methodology

- Created a new unsaved Unity editor scene in memory and discarded it after rendering.
- Loaded staged PNG maps from `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures` into temporary materials. ORM was interpreted as `R=AO`, `G=roughness`, `B=metallic` and converted to temporary Unity metallic/smoothness and occlusion maps.
- Built a small 8m corridor bay with wet dark stone floor, soot brick walls, blackened iron ribs/panels, brass/copper pipes, amber glass lamps, gauges, valves, rivets, hazard enamel, and scorch/oil decals.
- Used warm practical lamps, a destination gate wash, low cool fill, and a low floor rake light to keep dark stone readable without flattening the mood.
- Captured one player-height hero view plus two detail views through Unity cameras into RenderTextures, then wrote JPGs.

## Count Metrics

| Check | Result |
| --- | ---: |
| Texture files loaded | 32 / 32 |
| Material families used | 11 |
| Pipe/valve segments | 42 |
| Amber lamps | 5 |
| Gauges | 5 |
| Valves | 5 |
| Gauge tick marks | 60 |
| Rivets/bolts | 250 |
| Oil/scorch decals | 6 |
| Wet highlight patches | 3 |
| Tiling breakups | 18 |

## Image Readability Metrics

| Image | Average luminance | Near-black % | Bright % | Warm highlight % | Empty check |
| --- | ---: | ---: | ---: | ---: | --- |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery01_corridor_wide_unity_proof.jpg` | 0.097 | 1.3 | 1.8 | 28.0 | pass |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery01_floor_wetness_unity_proof.jpg` | 0.142 | 0.8 | 7.1 | 45.4 | pass |
| `Documentation/ConceptRenders/RENDER_ENV_Recovery01_pressure_gate_detail_unity_proof.jpg` | 0.093 | 0.9 | 5.5 | 16.6 | pass |

## Gates

| Gate | Status | Evidence |
| --- | --- | --- |
| Scope | Pass | Renderer creates an unsaved temporary editor scene and writes only the requested proof/report/metrics paths. |
| Unity-only render path | Pass | All hero/detail images are captured from Unity cameras into RenderTextures. |
| Existing staged texture usage | Pass | 32/32 expected FinalMaterialsV1 texture files loaded. |
| Material role separation | Pass | Wet stone, soot brick, blackened iron, brass, copper, amber glass, cream gauge, hazard enamel, walnut/leather, and oil/scorch decal roles are assigned. |
| Corridor density | Pass | 42 pipe/valve segments, 5 lamps, 5 gauges, 5 valves, 250 rivets, 6 wear decals. |
| Wet floor read | Pass | Broken translucent highlight patches plus low amber rake light; measured warm highlight in wide/floor views is 28.0% / 45.4%. |
| No dark empty render | Pass | Wide average luminance 0.097, near-black 1.3%, bright 1.8%. |
| Production asset readiness | Fail | This is a temporary primitive lookdev bay, not final modular mesh art, collision, LOD, lightmap UVs, or playable-scene integration. |
| Human north-star review | Fail until reviewed | The proof can only claim material/lookdev direction until an art review accepts it against the north-star corridor mood. |

## Frank Visual Read

This is a useful Unity-only material/corridor lookdev proof if the generated images show the intended readable depth: dark wet stone foreground, soot brick/blackened iron side structure, amber practicals, brass/copper pipework, gauge/valve details, and a pressure-gate destination. It should not be called final corridor art. The bay is primitive geometry with temporary material instances, no authored modular meshes, no collision, no LODs, no lightmap UV proof, no post-processing stack, and no gameplay integration. The next art pass should turn this into real modular wall/floor/gate pieces with UV-controlled material breakup and engine-validated lighting presets.

## Next Steps

- Promote only the material/lighting lessons, not the temporary primitive geometry.
- Build real modular 4m and 8m corridor pieces with the same material roles and repeated-rib depth language.
- Author decal placement variants so soot/oil breakup is not camera-dependent.
- Add a real pressure-gate module with open/locked states, sockets, collision, LODs, and lightmap readiness notes.
- Re-run a Unity screenshot from player height after modular meshes exist, then compare against the north-star corridor mood in human review.
