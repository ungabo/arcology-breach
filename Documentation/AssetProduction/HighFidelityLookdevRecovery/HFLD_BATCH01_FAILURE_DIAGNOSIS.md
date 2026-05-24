# Batch01 Visual Failure Diagnosis

Verdict: Batch01 failed as a high-fidelity north-star match. It should remain useful only as a rough vocabulary/blockout record, not as visual proof.

## What Failed

1. The renders looked like flat diagrams instead of rendered industrial concept art.
   - Batch01 used simple silhouettes, broad color fills, and line-like borders.
   - The source uses dimensional forms, bevel highlights, layered grime, wet reflections, smoke, and practical lighting.

2. Scene density was far below the concept.
   - The source corridor is packed with pipes, collars, gauges, valves, lamps, railings, bricks, floor seams, steam plumes, boilers, and rivets.
   - Batch01 corridor reduced that world to a door graphic, a few gauges, horizontal pipe lines, and a grid floor.

3. Material response was not credible.
   - The concept has aged brass, blackened iron, wet stone, leather, glass, hot copper, soot, oxidation, and steam with different roughness/specular behavior.
   - Batch01 mostly reads as flat amber-on-dark vector art, so metal, glass, leather, wetness, and soot do not separate.

4. Lighting missed the north-star mood.
   - The concept is lit by warm practical lamps with strong amber glow, black shadows, and wet reflective highlights.
   - Batch01 has little practical-light logic, weak shadow shaping, and no convincing wet-floor reflection system.

5. Camera and composition were too schematic.
   - The concept corridor uses a cinematic low/wide perspective with a foreground floor plane and a pressure door focal point.
   - Batch01 corridor is a flat frontal diagram with a visible grid floor and little depth.
   - The concept pistol is a close first-person object with a glove, foreshortening, and material presence.
   - Batch01 pistol is a side-on schematic without enough hand scale, dimensionality, or overlapping parts.

6. Hero asset detail was underspecified in the render.
   - The concept pressure door has concentric rings, radial braces, lock bars, vents, thick panels, and many rivets.
   - The concept pistol has layered cylinders, an exposed coil, a gauge, leather glove, screws, brass plates, grime, and hot copper.
   - The concept enemy has a boiler torso, expressive amber eyes, grille, gear wheel, saw, claw, piston limbs, and heavy feet.
   - Batch01 included the nouns but not enough secondary/tertiary detail to sell them.

7. Atmosphere was decorative rather than integrated.
   - The source steam has localized vents and layered depth.
   - Batch01 smoke reads as broad soft blobs without pipe-source logic or depth.

8. The review label "High Fidelity" set the wrong expectation.
   - The assets and renders were blockout/planning quality.
   - Calling them high fidelity made the mismatch feel like a broken promise instead of a rough first artifact.

## Corrective Actions

1. Rename the next pass as recovery proof or planning until it passes the rubric.
2. Do a density greybox before material polish; count pipes, lamps, gauges, valves, rivets, and steam sources against the concept.
3. Build the corridor around practical amber lights and wet floor reflections first, then dress details into that lighting model.
4. Use real PBR/procedural materials with bevels, roughness variation, normals, grime decals, oxidation, soot, and water/oil breakup.
5. Stop using side-on diagram camera angles for hero proof renders.
6. Render the pistol from first-person 3/4 view with glove/hand scale, visible gauge, coil, muzzle, lower reservoir, and layered fasteners.
7. Render the enemy in a 3/4 pose with gear, saw, claw, stacks, eyes, torso, and feet visible.
8. Use an isolated Unity lookdev proof for the next render; do not generate another flat 2D placeholder and call it high fidelity.
9. Require one side-by-side annotated review image before accepting a render as closer to the source.
10. Keep all work isolated from gameplay scenes, scripts, generated scenes, and previous HighFidelityLookdev files.
