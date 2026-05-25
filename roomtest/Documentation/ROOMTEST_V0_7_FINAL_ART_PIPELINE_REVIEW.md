# Roomtest v0.7 Final-Art Pipeline Review

Generated: 2026-05-25T04:34:18Z

## Required Files

- Beauty render: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_v0.7_final_art_beauty.png`
- Material closeup: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_v0.7_material_closeup.png`
- Contact sheet: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_v0.7_contact_sheet.png`
- Metrics: `D:/__MY APPS/Unity Doom/roomtest/Renders/roomtest_metrics_v0.7.json`

## Unity-Only Technique

- Built-in renderer only; no URP/HDRP, Blender, external DCC, or external asset generator.
- Generated 2048 PBR texture families for dark wet brick, wet black flagstone, and sooted ceiling brick: albedo, normal, height, occlusion, and packed metallic/smoothness.
- Added actual beveled mesh relief for wall bricks, ceiling bricks, and floor flagstones over recessed black mortar, instead of relying on flat planes.
- Added tarnished brass/copper/black-iron gaslight fixtures, pipe runs, dark soot bands, glossy black oil films, warm gaslight, reflection probe, and controlled fill lights.
- Rendered a 1920x1080 beauty shot, a material closeup, and a contact sheet with notes.

## Self-Score Against North-Star

- Dark irregular wet masonry: 6.5/10, pass for proof. Actual bevel relief and procedural roughness are visible, but still less nuanced than authored/scanned AAA masonry.
- Wet floor reflections: 6/10, pass for proof. Glossy wet response exists without mirror-flatness, but reflection shaping is still simpler than the north-star.
- Warm controlled gaslight: 6/10, pass for proof. The tone is warm and localized, but built-in rendering lacks the subtle bloom/volumetric richness of the target.
- Steampunk material restraint: 6.5/10, pass for proof. Brass/copper are used as tarnished accents rather than toy-orange surfaces.
- AAA final quality: fail. This is a credible Unity-only pipeline proof, not a final AAA asset set.

## Known Gaps

- No scanned/photogrammetry source, no hand-sculpted high-poly bake, and no authored decal atlas.
- Beveled block meshes improve depth but still have too much procedural regularity at close inspection.
- Built-in renderer limits bloom, screen-space reflections, color grading, and physically rich wet-surface behavior.
- Lamps and pipes need proper modeled silhouettes, bolts, seams, gauges, glass thickness, soot masks, and LODs.

## Next Concrete Fixes

- Author a dedicated grime/decal atlas for edge soot, oil streaks, mortar stains, lamp scorch marks, and chipped stone corners.
- Build reusable modular wall/floor meshes with baked bevels, corner damage, and vertex color masks.
- Add a supported post stack or migrate this isolated lookdev project to URP/HDRP if allowed later.
- Replace primitive lamp and pipe assemblies with hand-modeled Unity mesh assemblies and validated material closeups.
