# SurfaceBreakupDecalSet12 QA Checklist

Version: 0.1.57-p001

## North-Star Comparison

- Breaks flatness: soot, chips, scuffs, and corner cards interrupt clean brick planes and hard right angles without new wall meshes.
- Wet reflective surfaces: puddle strips, oil trails, and warm light stains provide glossy alpha layers for gaslit floor reflections and darker machinery leaks.
- Readable but not noisy: each decal has soft alpha falloff plus internal breakup, with warm/cool variants so corridors avoid one-note black smudges.
- Roomtest brick fit: dark brick chips, lime dust, rust halos, and corner grime sit over brick/mortar patterns without hiding the base material read.

## Asset Checks

- 20 prefabs cover soot, oil, damp puddles, warm light stains, pipe leaks, brass edge wear, chipped wall grime, floor scuffs, corner darkness, and rivet rust halos.
- Albedo textures carry transparent alpha; mask maps pack metallic/occlusion/alpha/smoothness for Standard fallback and URP-compatible reuse.
- Prefabs are visual-only quad cards: no colliders, lights, probes, animation, gameplay scripts, or scene dependencies.

## Performance Notes

Use material instancing and static placement. Avoid stacking more than 3-4 full-screen transparent cards in the same camera slice on low settings. Keep wet/oil cards near light pools where their smoothness read matters most.
