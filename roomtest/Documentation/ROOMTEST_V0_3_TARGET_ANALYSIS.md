# Roomtest v0.3 Target Analysis

Generated: 2026-05-24T23:57:50Z

## Reference Targets

- Room shape: simple enclosed rectangular masonry chamber, wide first-person perspective, visible floor, ceiling, side walls, and back wall.
- Wall brick: small dark brown-black aged bricks with recessed mortar, chipped irregular edges, soot, grime, and non-uniform color.
- Floor stone: larger flagstones than the walls, darker wet surface, warm reflected lamp glints, no metallic orange material response.
- Ceiling brick: small sooted brick courses, darker and less glossy than floor, visible perspective lines into the back of the room.
- Lighting: two warm amber wall lamps with localized halos; the full room should stay dark rather than turning orange.
- Depth: corners and the back wall should hold darkness, with readable but subdued surface relief.

## v0.3 Method

1. Generate and check base albedo PNGs before material creation.
2. Generate and check normal, height, occlusion, and packed metallic/smoothness map PNGs.
3. Build Unity Standard materials from those maps.
4. Build an isolated material preview and a full room scene using real brick/slab geometry plus the maps.
5. Render PNG evidence and record an acceptance note against the reference.
