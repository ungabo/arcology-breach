# Surface Material Detail Set 08 QA Checklist

## Automated File Checks

- material_count: pass (24)
- texture_png_count: pass (96)
- preview_png_count: pass (20)
- png_integrity_dimensions: pass (96 material PNGs at 512x512; 20 preview boards at 960x720)
- material_guid_reference_integrity: pass (144 material texture GUID references resolve package-local)
- forbidden_file_types: pass (no Blender, FBX, prefab, or Unity scene files)
- owned_roots_only: pass (generator asserts every write against allowed roots)
- runtime_contract: pass (no scenes, prefabs, meshes, scripts, colliders, audio, or build settings authored)

## v0.1.52 Binding Decision

PASS WITH LIMITATIONS. The strongest 17 final-candidate materials are good enough for quarantine import and selective binding. Candidate overlays need a decal/transparent shader path before production use, and `CrackedBlackRubberGasket` remains placeholder quality.

## Unity Import Checks

1. Import package into a quarantine Unity project.
2. Confirm 24 materials import under Standard shader.
3. Confirm 96 texture PNGs import with albedo as sRGB, normal maps as normal textures, and masks as linear textures.
4. Review at least one plane/panel and one cylinder/pipe setup for each major material group.
5. Compare results to the 20 preview PNGs and the north-star concept art.

## Visual Acceptance

- Walls/floors should no longer read as flat grey blocks.
- Iron should read black, riveted, chipped, and oily.
- Brass/copper should show tarnish, patina, and rubbed highlights.
- Wet black stone and oily floor materials should catch warm amber highlights without becoming unreadably black.
- Gauge enamel and amber glass should remain readable focal accents.
