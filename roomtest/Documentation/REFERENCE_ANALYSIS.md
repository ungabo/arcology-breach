# Reference Analysis

Source: user-provided dark brick room image, 16:9, centered camera looking into a rectangular stone-brick chamber.

## Target Visual Traits

- Enclosure: simple rectangular room with side walls, back wall, ceiling, and floor all using dark aged masonry.
- Materials: dark brown-black stone/brick with recessed mortar, chipped rounded brick edges, uneven surface height, soot, grime, and subtle warm color variation.
- Floor: larger wet stone slabs, glossier than the walls, with visible warm reflected highlights near the camera and below the wall lights.
- Walls and ceiling: smaller brick courses with strong mortar lines, rounded/high-relief brick edges, and less gloss than the floor.
- Lighting: two warm amber wall lights mounted left and right at roughly player head height, symmetric enough to frame the room but not perfectly flat.
- Reflections: floor should catch broad warm streaks and specular glints; walls should catch smaller edge highlights without becoming shiny plastic.
- Camera: wide first-person field of view, low-to-mid human eye height, strong perspective lines toward the back wall, with both ceiling and floor visible.

## Deterministic Build Strategy

- Generate procedural PBR maps rather than hand-painting one-off textures.
- Use texture and normal detail as the primary brick detail carrier for the full room.
- Include a beveled brick object/prefab as a component proof, but avoid thousands of individual bricks in the first room test.
- Use warm point and spot lights with shadows, plus low ambient fill and reflection probes.
- Render a fixed camera angle and write objective metrics so later passes can compare brightness, highlight warmth, and material response.
