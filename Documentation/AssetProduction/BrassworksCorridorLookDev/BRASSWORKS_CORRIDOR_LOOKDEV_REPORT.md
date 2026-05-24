# Brassworks Corridor Unity LookDev Report

Status: Technical pass for Unity-only lookdev render generation; production art and human north-star review remain pending.
Date/time: 2026-05-24 04:18:36 -04:00
Unity version: 6000.4.6f1
Batchmode entrypoint: `BrassworksCorridorLookDevRenderer.RenderBatch`
Unity command prepared: `"C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe" -batchmode -quit -projectPath "D:\__MY APPS\Unity Doom" -executeMethod BrassworksCorridorLookDevRenderer.RenderBatch -logFile "D:\__MY APPS\Unity Doom\Documentation\AssetProduction\BrassworksCorridorLookDev\unity_brassworks_corridor_lookdev_batch.log"`
Tool lane: Unity-only editor batch render. Temporary in-memory scenes, procedural primitives, procedural material textures, Camera.Render, PNG export. No Blender or external DCC.
Write scope: Assets/_Project/Editor/BrassworksCorridorLookDevRenderer.cs plus Documentation/ConceptRenders/BrassworksCorridor/** and Documentation/AssetProduction/BrassworksCorridorLookDev/**.

## Outputs

| File | Purpose | Dimensions |
| --- | --- | ---: |
| `Documentation/ConceptRenders/BrassworksCorridor/BBW_CORRIDOR_001_modular_corridor_wet_pipe_lamps.png` | Corridor module with pipes, lamps, and wet floor | 1920x1080 |
| `Documentation/ConceptRenders/BrassworksCorridor/BBW_CORRIDOR_002_hero_round_vault_door.png` | Hero round vault door module | 1920x1080 |
| `Documentation/ConceptRenders/BrassworksCorridor/BBW_CORRIDOR_003_wall_kit_component_sheet.png` | Reusable wall-kit component sheet | 2200x1400 |
| `Documentation/ConceptRenders/BrassworksCorridor/BBW_CORRIDOR_CONTACTSHEET_unity_lookdev.png` | Contact sheet for the three Brassworks corridor lookdev renders | 2400x1500 |
| `Documentation/AssetProduction/BrassworksCorridorLookDev/brassworks_corridor_lookdev_metrics.json` | Machine-readable metrics and gates | n/a |

## Components Built

- Corridor module: wet tiled floor, soot-brick side walls, blackened iron ribs, brass hand rails, overhead and wall pipe runs, caged amber lamps, pressure tanks, gauges, valves, steam vents, oil smears, and broken warm reflections.
- Hero vault door module: layered round pressure door, brass outer seal, blackened steel slab, radial braces, rivet rings, central valve wheel, side gauges/valves, tanks, lamps, pipes, and wet foreground.
- Reusable wall-kit sheet: pipe cluster, riveted panel, caged lamp, valve wheel, pressure tank, and wet floor tile isolated as future modular asset targets.

## Material Approach

All lookdev materials are generated in the editor script. The pass uses built-in Unity `Standard`, `Unlit/Color`, and `Unlit/Transparent` shaders with procedural grime/scratch textures, generated normal-like noise, metalness/smoothness settings, transparent wet highlights, soft steam billboards, and warm unlit lamp cores. No Blender, OBJ/FBX import, authored mesh asset, or external texture package is used by this renderer.

Material roles: wet oil-dark stone, soot brick, blackened steel, dark pressure metal, aged brass, dark brass, copper, cream enamel gauge face, amber lamp glass, lamp core glow, wet reflection, oil film, steam, warning red, line dark, iron edge, and dark inspection backplate.

## Count Metrics

| Metric | Count |
| --- | ---: |
| Procedural material roles | 17 |
| Primitive objects | 853 |
| Rivets/bolts | 418 |
| Pipe segments | 22 |
| Lamps | 7 |
| Steam puffs | 7 |
| Gauges | 2 |
| Valves | 5 |
| Pressure tanks | 5 |
| Floor tiles | 17 |
| Wall panels | 13 |
| Wet/oil highlight patches | 7 |

## Automated Render Checks

| Image | Avg luminance | Contrast | Content % | Near-black % | Warm metal % | Magenta % | Buckets | Nonblank | No magenta | Material sep. | Framing |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- | --- | --- | --- |
| `Documentation/ConceptRenders/BrassworksCorridor/BBW_CORRIDOR_001_modular_corridor_wet_pipe_lamps.png` | 0.104 | 0.883 | 99.5 | 26.6 | 8.3 | 0.000 | 30 | pass | pass | pass | pass |
| `Documentation/ConceptRenders/BrassworksCorridor/BBW_CORRIDOR_002_hero_round_vault_door.png` | 0.100 | 0.948 | 75.4 | 41.1 | 20.4 | 0.000 | 25 | pass | pass | pass | pass |
| `Documentation/ConceptRenders/BrassworksCorridor/BBW_CORRIDOR_003_wall_kit_component_sheet.png` | 0.044 | 0.725 | 32.8 | 91.2 | 4.8 | 0.000 | 21 | pass | pass | pass | pass |

## Gates

| Gate | Status | Evidence |
| --- | --- | --- |
| Unity-only procedural path | pass | The renderer builds temporary Unity scenes from primitive cubes, cylinders, spheres, quads, generated textures, and built-in materials. No external DCC or mesh import is used. |
| Required view coverage | pass | Rendered corridor module, hero vault door module, and reusable wall-kit component sheet. |
| Nonblank images | pass | Each required PNG must have average luminance > 0.04, contrast > 0.08, and content pixels > 18%. |
| No magenta/material-error pixels | pass | Maximum sampled magenta-like pixel rate is 0.000%; threshold is < 0.05%. |
| Material separation | pass | 17 procedural material roles, minimum sampled color buckets 21, with warm brass/copper and dark steel/stone bins present in each render. |
| Reasonable framing | pass | Content bounds are required to occupy at least 38% width and 32% height with center offset under 28%. |
| Production asset readiness | partial | This is lookdev only. It has no authored meshes, pivots, snap metadata, collision, UV2/lightmap proof, LODs, or prefab variants. |
| Human north-star review | pending | Objective render checks can catch blank/shader/framing failures, but final visual acceptance still requires human comparison against the north-star concept art. |

## Acceptance Criteria

- Three required PNG views exist under `Documentation/ConceptRenders/BrassworksCorridor`.
- Each view is generated by Unity batch rendering from in-memory procedural primitives/materials only.
- Corridor view clearly includes pipes, lamps, steam vents, and a wet reflective floor.
- Vault-door view clearly includes a heavy round pressure door with brass/steel layering, rivets, valve hardware, tanks, and warm practical light.
- Component sheet shows the reusable kit vocabulary: pipe clusters, riveted panels, lamp, valve wheel, pressure tank, and floor tile.
- Automated render checks record nonblank image, no sampled magenta shader-error pixels, material separation, and framing.

## Honest Gaps Versus North-Star

This pass is a stronger Unity-only direction proof, not final environment art. Primitive cylinders and boxes cannot match the north-star image's authored bevels, sculpted seams, true grime placement, dense occlusion, parallax, volumetric steam, screen-space wet reflections, or material microdetail. The steam is intentionally restrained to avoid opaque billboard slabs. The vault door and kit parts still need real modular meshes, pivots, snap rules, colliders, UVs, lightmap/LOD proof, prefab variants, decal placement, and in-level gameplay validation before promotion.

## PM Visual Review

Status: integrate as reference-only lookdev, not as final environment art.

- The corridor and vault-door views are useful for mood, scale, warm lamp language, wet-floor direction, pipe density, and brass/blackened-metal vocabulary.
- The wall-kit component sheet has visible overlapping labels, so it should not be used as a presentation contact sheet or final acceptance artifact.
- The corridor still reads as primitive greybox construction with better lighting/material roles, not north-star-level authored geometry.
- Next environment step should convert the best corridor/vault nouns into a real modular mesh/prefab plan with snap points, pivots, collision, LOD expectations, and in-level placement tests.
