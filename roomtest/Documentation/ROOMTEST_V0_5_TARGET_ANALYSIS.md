# Roomtest v0.5 Target Analysis

Generated: 2026-05-25T00:34:07Z

## v0.4 Problems To Correct

- Real brick cubes still read as construction blocks instead of aged masonry.
- Side walls remained too orange near the lamps.
- Ceiling geometry still looked panelized.

## v0.5 Target

- Use continuous room surfaces with procedural albedo/normal/height/occlusion/smoothness maps as the primary detail carrier.
- Keep only subtle edge/corner grime geometry, avoiding chunky per-brick blocks.
- Make wall/ceiling brick scale smaller than floor slabs through material tiling.
- Use warm lamps for local halos and damp floor glints while preserving dark corners and a readable back wall.
- Follow the same evidence order: analysis, base PNGs, associated map PNGs, material build, isolated scene, render, acceptance note.
