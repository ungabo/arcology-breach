# Roomtest Stage Gates

The roomtest workflow should move through visible evidence gates instead of trial-and-error full-room renders.

## Gate 1 - Reference Analysis

Output:
- `Documentation/REFERENCE_ANALYSIS.md`

Acceptance:
- Names the target scale, color, lighting, reflection, and camera traits.
- Separates wall brick, ceiling brick, and wet floor slab requirements.

## Gate 2 - Texture PNGs

Output:
- `Assets/RoomTest/Textures/RT_DarkBrickWall_Albedo.png`
- `Assets/RoomTest/Textures/RT_WetStoneFloor_Albedo.png`
- `Assets/RoomTest/Textures/RT_SootBrickCeiling_Albedo.png`

Acceptance:
- Albedo maps look dark brown-black before lighting is applied.
- Mortar is visible but not black-grid dominant.
- Brick scale matches the reference: smaller wall/ceiling bricks, larger floor slabs.

## Gate 3 - Mapping PNGs

Output for each surface:
- `_Normal.png`
- `_Height.png`
- `_Occlusion.png`
- `_MetallicSmoothness.png`

Acceptance:
- Height shows recessed mortar and uneven chipped brick faces.
- Normal map supports edge relief without turning every brick into a cartoon bevel.
- Occlusion deepens mortar and corners.
- Smoothness is highest on floor and lower on walls/ceiling.

## Gate 4 - Material Block Preview

Output:
- A simple cube/plane render using one material at a time.

Acceptance:
- Wall material reads as dark aged masonry under neutral light.
- Floor material shows wet reflection without looking like polished orange metal.
- Ceiling material stays darker and less glossy than the floor.

## Gate 5 - Full Room Scene

Output:
- `Assets/RoomTest/Scenes/Roomtest_BrickLighting.unity`
- `Renders/roomtest_brick_lighting_v*.png`
- `Renders/roomtest_metrics_v*.json`

Acceptance:
- Camera framing matches the reference room.
- Two warm wall lights create localized halos, not whole-room orange wash.
- Floor reflections are present and warm, while the back wall remains darker.
- No surface looks flat, magenta, unlit, or obviously procedural.

## Gate 6 - Review And Revision

Output:
- A short note per render pass explaining pass/fail.

Current first-pass result:
- `roomtest_brick_lighting_v0.1.png` successfully proves the isolated Unity workflow.
- It fails the final look target because it is too bright, too orange, too clean, and the brick relief reads as a regular raised grid.

Next correction:
- Darken albedo and ambient levels.
- Reduce broad orange light spill.
- Make wall/ceiling bricks less uniformly raised.
- Keep the floor wetter than walls without turning the whole room glossy.
