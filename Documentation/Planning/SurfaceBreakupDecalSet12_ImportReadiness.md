# SurfaceBreakupDecalSet12 Import Readiness

Version: 0.1.57-p001

Set12 is an isolated visual-only package for corridor grime and surface breakup. It adds transparent quad decal helpers instead of changing shared scenes or base architecture.

## Immediate Integration

- Import AssetPacks/BrassworksBreach.SurfaceBreakupDecalSet12 as a local Unity package or copy Runtime assets into the target art folder.
- Place prefab cards slightly proud of walls/floors to avoid z-fighting.
- Start with corner darkness, soot streaks, pipe leak grime, damp strips, and floor scuffs; add brass wear and rivet halos as close-read dressing.

## Performance Budget

All maps are 512 px with mipmaps and compression metadata. Prefabs are single built-in Quad renderers with no shadows, probes, colliders, or scripts. Batch by material where possible and prefer a few large cards over many tiny overlapping transparent cards on low-end Windows PCs.
