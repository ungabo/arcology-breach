# HeroCorridorLightingLookdevSet11 Production Report

Status: visual evidence lookdev packet, not gameplay import.
Generated: 2026-05-24 21:47:34 -04:00
Unity version: 6000.4.6f1
Batch entrypoint: `HeroCorridorLightingLookdevSet11Renderer.RenderBatch`
Unity command: `"C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe" -batchmode -quit -projectPath "D:/__MY APPS/Unity Doom/TempUnity/HeroCorridorLightingLookdevSet11" -executeMethod HeroCorridorLightingLookdevSet11Renderer.RenderBatch -logFile "D:/__MY APPS/Unity Doom/Documentation/AssetProduction/V0_1_56_HeroCorridorLightingLookdevSet11/unity_HeroCorridorLightingLookdevSet11_batch.log"`

## Scope

This packet stages a final-ish corridor render in Unity only. It uses accepted sidecar families for the room materials, wetness decals, steam/fog, gaslights, door/vault mechanisms, pipe/tank/gauge dressing, and Set11 high-fidelity corridor dressing. It writes no playable scenes, no package manifests, no shared status files, and no asset packages.

## Outputs

| File | Purpose | Dimensions | Status |
| --- | --- | ---: | --- |
| `Documentation/ConceptRenders/V0_1_56_HeroCorridorLightingLookdevSet11/HCLL11_RENDER_01_wide_corridor.png` | Wide corridor view: dark masonry room, wet flagstones, warm wall lamps, pipes, gauges, tanks, pressure door target, and steam depth. | 1920x1080 | pass |
| `Documentation/ConceptRenders/V0_1_56_HeroCorridorLightingLookdevSet11/HCLL11_RENDER_02_material_lighting_closeup.png` | Close view of wet flagstone reflections, dark masonry breakup, amber gaslight spill, damp bands, and soot overlays. | 1600x1200 | pass |
| `Documentation/ConceptRenders/V0_1_56_HeroCorridorLightingLookdevSet11/HCLL11_RENDER_03_object_dressing_detail.png` | Object dressing close-up: layered sidecar pipe run, gauge cluster, boiler tank, valve bank, lamp cage, and nearby steam cards. | 1600x1200 | pass |
| `Documentation/ConceptRenders/V0_1_56_HeroCorridorLightingLookdevSet11/HCLL11_CONTACTSHEET.png` | Optional contact sheet combining wide, material/lighting close-up, and object dressing close-up. | 2400x1500 | pass |

## Scene Assembly

- Dark masonry shell and wet flagstone floor use RoomMaterialSet10 materials with additional Unity primitive breakup.
- Wet shine, soot curtains, damp bands, puddles, and oil cards combine GrimeDecalWetnessSet10 prefabs with Unity transparent glints.
- SteamAtmosphereVfxSet10 prefabs and Unity fog cards provide low mist, backlit door fog, gaslight haze, and pipe leak steam.
- DoorVaultSet10 and BrassworksDoorMechanismSet10 create the pressure-door destination and locking mechanism hints.
- PipeTankGaugeSet10, GaslightPipeDressingSet10, and SteamCorridorDressingHighFidelitySet11 supply the layered pipes, gauges, tanks, valves, lamp cages, vents, grates, threshold trim, and object density.

## Counts

| Metric | Count |
| --- | ---: |
| Unity primitive staging objects | 752 |
| Accepted sidecar prefab instances | 34 |
| Unity point lights | 7 |
| Steam/fog cards | 7 |
| Wetness/decal cards | 7 |
| DoorVaultSet10 prefab instances | 2 |
| BrassworksDoorMechanismSet10 prefab instances | 3 |
| SteamCorridorDressingHighFidelitySet11 prefab instances | 17 |
| PipeTankGaugeSet10 prefab instances | 3 |
| GaslightPipeDressingSet10 prefab instances | 2 |
| GrimeDecalWetnessSet10 prefab instances | 3 |
| SteamAtmosphereVfxSet10 prefab instances | 4 |

## Important Limitation

These images are lookdev evidence only. The packet does not create shipping meshes, scene assets, pivots, collisions, lightmap UVs, LODs, gameplay hooks, prefab variants, or manifest entries for the main game.
