# Brassworks Breach Room Material Set 10

Unity-only visual sidecar package for final-direction dark wet masonry room materials inspired by the accepted roomtest v0.5/north-star look.

## Contents

- 6 Unity Standard .mat files.
- 30 generated 512x512 procedural PNG texture maps: albedo/base, normal, roughness/metallic/AO, grime/edge/wetness mask, and height.
- 4 package-local preview PNGs in Documentation~/Previews.
- Package-local material catalog and sidecar manifest.
- Import readiness, QA, and concept preview docs under Documentation.

## Texture Channels

- *_ALB.png: sRGB albedo/base color; overlay families include alpha.
- *_NRM.png: linear tangent-space normal detail.
- *_RMA.png: linear R=metallic intent, G=roughness intent, B=ambient occlusion/grime intent, A=Unity Standard smoothness.
- *_GRM.png: linear R=edge/chip mask, G=grime/soot mask, B=wetness mask, A=overlay alpha where relevant.
- *_HGT.png: linear height/parallax intent.

## Contract

Visual/material-only. No meshes, prefabs, colliders, scenes, audio, gameplay scripts, runtime authority, package-manager edits, or build configuration changes.

## Import Notes

- Import through a quarantine Unity project first.
- Use wall and ceiling brick at smaller tiling scale than the flagstone floor.
- Treat RMS10_MAT_EdgeDampnessOverlay and RMS10_MAT_SootDecalOverlay as overlay candidates until the main project confirms the transparent/decal shader path.
- Rollback is deleting the isolated local package root and its local package reference, if one is added later.