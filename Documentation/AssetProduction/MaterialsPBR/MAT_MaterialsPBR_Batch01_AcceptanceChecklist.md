# MAT_MaterialsPBR_Batch01 Acceptance Checklist

Created: 2026-05-23 22:01:27 -04:00
Worker: Worker A

## Complete

- [x] Eight steampunk-first material IDs staged: aged brass, riveted blackened iron, soot-stained brick, wet oil-dark stone, green oxidized copper, grimy amber glass, leather bellows, and hazard enamel.
- [x] Each material has a 1024x1024 BaseColor PNG.
- [x] Each material has a 1024x1024 Normal PNG generated from procedural height detail.
- [x] Each material has a 1024x1024 packed ORM PNG with R=AO, G=roughness, B=metallic.
- [x] `MAT_GrimyAmberGlass` BaseColor includes prototype alpha for opacity.
- [x] Preview contact sheets exist as PNG/JPG files.
- [x] Machine-readable JSON manifest exists with map paths, dimensions, image modes, and SHA-256 hashes.
- [x] Human-readable Markdown manifest exists.
- [x] Files are staged only under the Worker A ownership folders.

## Procedural Prototype Quality

- [x] Surface language matches the north star: aged brass, rivets, blackened iron, oil-dark masonry, amber glass, green oxidation, leather pressure fittings, and hazard enamel.
- [x] Textures are viewable and usable for material block-in, look development, and early replacement of procedural primitives.
- [x] Noise, scratches, pitting, grime, soot, rivets, seams, folds, and chips are generated procedurally rather than artist-authored from sculpted or scanned sources.
- [x] BaseColor tiling has been previewed with a 2x2 contact sheet, but not all directional stains are guaranteed final-quality tileables.
- [x] Normals are height-derived approximations and should be visually tuned in-engine before final adoption.
- [x] ORM values are plausible for preview PBR response but still need Unity shader validation.

## Replace Later With Final Artist-Authored Assets

- [ ] Replace procedural noise detail with authored/sculpted wear maps for hero props, weapons, enemies, and close-range VR surfaces.
- [ ] Author UV-aware versions for large modular walls, floors, pipes, doors, bellows, and signage so seams, rivets, and stripe scale land intentionally.
- [ ] Create production import presets or Unity `.mat` assets after the main integration agent chooses shader settings.
- [ ] Add platform-specific downscale variants once Windows baseline material response is approved.
- [ ] Add final albedo/roughness calibration passes under target lighting.
- [ ] Create additional variants for clean, damaged, burned, wet, and objective-highlighted states.

## Intake Notes

- Import BaseColor maps as sRGB.
- Import Normal maps as Normal Map textures.
- Import ORM maps as linear/non-sRGB mask textures.
- Treat the batch as production-staged art direction, not final shipped material art.
