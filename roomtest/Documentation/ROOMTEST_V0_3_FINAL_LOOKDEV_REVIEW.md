# Roomtest v0.3 Final Lookdev Review

Generated: 2026-05-25T00:00:46Z

## Files

- Material preview: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_final_material_preview_v0.3.png`
- Room render: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_final_brick_chamber_v0.3.png`
- Metrics: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_metrics_v0.3.json`

## What Changed

- Moved from material-only planes to actual per-brick and per-slab Unity geometry over dark recessed mortar.
- Generated v0.3 albedo, normal, height, occlusion, and metallic/smoothness maps for wall, floor, and ceiling.
- Checked the base albedo PNGs before material creation, then checked associated map PNGs before building Unity materials.
- Added material tint variants to break up repeated procedural texture color.
- Rebuilt the lamps with brass rims, cage bars, black iron backing, localized point lights, and floor-grazing spot reflections.
- Added base grime bands and corner soot to reduce the clean test-room look.

## Acceptance Comparison

- Texture relief: target is chipped, uneven masonry; v0.3 uses real brick/slab geometry plus normal and height maps so the relief should read stronger than v0.2.
- Wet reflection: target is damp floor glints, not orange metal; v0.3 keeps metallic at zero and packs floor smoothness into the metallic/smoothness alpha channel.
- Warm wall light: target is two localized amber wall halos; v0.3 uses point lights plus floor-grazing spots while preserving a dark center and back wall.
- Ceiling/floor scale: target is small wall/ceiling brick with larger floor stones; v0.3 separates those scales through geometry and texture profiles.
- Corner depth: target corners stay dark and grounded; v0.3 adds recessed mortar bases, corner soot, and base moisture bands.

## Blunt Assessment

- This pass should be much closer to the reference than v0.2 because the brick relief is now real geometry instead of only normal-map detail.
- If it still fails, the next fixes should be artistic rather than architectural: more broken silhouettes, better lamp glass, darker mortar pooling, and hand-placed grime/decal cards.
