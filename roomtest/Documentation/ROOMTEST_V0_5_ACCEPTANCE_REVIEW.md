# Roomtest v0.5 Acceptance Review

Generated: 2026-05-25T00:36:45Z

## Files

- Material preview: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_material_driven_preview_v0.5.png`
- Room render: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_material_driven_brick_room_v0.5.png`
- Metrics: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_metrics_v0.5.json`

## What Changed From v0.4

- Replaced individual brick/block construction with continuous mapped surfaces.
- Used procedural PBR maps and tiling to create small wall/ceiling brick and larger floor slabs.
- Kept geometry for only corner soot, damp bands, lamps, and reflection helpers.
- Rebalanced lighting toward localized amber lamps, neutral low back-wall readability, and damp floor glints.
- v0.5 final rerun narrowed mortar, darkened wall/floor albedo, increased v0.5-only noise/chipping, strengthened normal maps, reduced lamp halo range, and brightened the lamp glass without increasing room wash.

## Acceptance Comparison

- Texture relief: should now read as continuous aged brick/stone rather than blockout geometry; still fails if surfaces look flat.
- Wet reflection: should show warm floor glints without metallic orange material.
- Warm wall light: should create localized amber halos while leaving the center/back room dark.
- Ceiling/floor scale: wall and ceiling brick should be smaller than floor flagstones.
- Corner depth: back corners should remain dark while the back wall remains readable.

## Blunt Assessment

- This is the most reference-aligned approach so far: material-driven masonry with restrained geometry and a darker room center.
- Pass: the render no longer looks like flat swatches or chunky blockout geometry, the floor reads wet, and the amber lamp light is localized instead of filling the whole room.
- Still fails reference parity: floor grout and brick courses are still more regular than the supplied image, the lamp fixture is still simplified, and the room needs authored grime/decal layering for true production realism.
