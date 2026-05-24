# Brassworks Breach Surface Material Detail Set 08

Unity-only sidecar package for procedural steampunk surface detail. This package does not modify live `Assets`, scenes, build scripts, or existing source.

## Contents

- 24 Unity Standard `.mat` files.
- 96 generated PNG textures: albedo/base, normal/bump-like detail, roughness/metallic intent, and grime/edgewear masks.
- 20 preview PNGs in `Documentation/ConceptRenders/V0_1_52_SurfaceMaterialDetailSet08`.
- Status split: 17 final-candidate, 6 candidate, 1 placeholder.

## Texture Channels

- `*_ALB.png`: sRGB albedo/base color.
- `*_NRM.png`: linear tangent-space normal map.
- `*_RMA.png`: linear R=metallic intent, G=roughness intent, B=ambient occlusion/grime intent.
- `*_GRM.png`: linear R=edgewear, G=grime/soot, B=wetness/patina.

## Import Notes

- Import through a quarantine project first.
- v0.1.52 import/binding verdict: PASS WITH LIMITATIONS. Bind final-candidate wall, trim, pipe, oil-floor, furnace, amber glass, and gauge materials first.
- Treat overlay materials as candidates until a decal/transparent shader path is chosen.
- Use final-candidate wall, trim, pipe, oil-floor, furnace, amber glass, and gauge materials first; they are the highest-impact fix for the north-star gap.
- This package contains visual/material assets only: no meshes, prefabs, scenes, code authority, colliders, gameplay hooks, audio, or build configuration.
