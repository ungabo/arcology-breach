# Threshold Route Dressing Batch Material Notes v0.1.33

These notes define the intended Unity material instances for the staged `ThresholdRouteDressingBatch` source package. They are ready for main-lane conversion into actual `.mat` assets using the project's active render pipeline and shader conventions.

## Source Sheets

- `Textures/T_TRD_v0133_MaterialSwatches_1024.png`: visual review swatches for the full material palette.
- `Textures/T_TRD_v0133_RouteIndicatorNameplates_1024.png`: amber route indicator and threshold nameplate atlas with transparent background.
- `Textures/T_TRD_v0133_GrimeGasketRivetAccents_1024.png`: transparent grime, gasket, rivet, and edge-wear atlas.
- `Previews/PREVIEW_ThresholdRouteDressingBatch_ContactSheet_v0.1.33.png`: preview-only contact sheet.

## Import Notes

Use `Default` texture type for all staged sheets. Keep sRGB on for the material swatch sheet, plate atlas, and contact sheet. Keep alpha transparency enabled for route/nameplate and grime/gasket/rivet sheets.

Recommended settings:

- Max size: 1024 for source sheets, 2048 only if future redraws need more text clarity.
- Filter mode: Bilinear.
- Mip maps: on for world use.
- Wrap mode: Clamp for plates and grime decals. Repeat may be tested on gasket/rivet strip regions only.
- Compression: use high quality after readability review. Keep uncompressed during first art review if text edges need inspection.

## Material Instances

### TRD_BlackenedIron

Use on brace back plates, rails, clamp bodies, coupler lugs, gasket retainers, pipe supports, and nameplate frames.

- Base color: charcoal black with slight warm-brown and blue-gray variation.
- Metallic: high.
- Smoothness or roughness: medium-high roughness.
- Detail: pitted edge wear, soot dust, scraped corners, oily contact marks.
- Avoid: pure black plastic, mirror gloss, or clean gunmetal.

### TRD_AgedBrass

Use on piston rods, plate rims, clamp highlights, rail wear edges, bolt heads, and select decorative but functional hardware.

- Base color: muted brass, dark ochre, tarnished brown.
- Metallic: high.
- Smoothness or roughness: medium roughness with glossier contact edges.
- Detail: recess tarnish, hand polish, oil near moving seals.
- Avoid: saturated yellow gold.

### TRD_OilyGunmetal

Use on steam cylinders, pipe bodies, coupler sleeves, and heavy moving housings.

- Base color: dark gray gunmetal.
- Metallic: high.
- Surface: rough industrial metal with localized glossy oil.
- Detail: heat discoloration near seams, brown-black leaks below couplers.

### TRD_AmberRouteGlass

Use on small indicator lenses and protected route lamps.

- Base color: dark amber orange.
- Emission: low to medium. Keep it practical, not UI-bright.
- Surface: dirty glass or enamel with chipped rim grime.
- Placement: pressure indicators, lift access tabs, small route lenses.

### TRD_WornAmberEnamel

Use on route indicator plates and threshold nameplates.

- Base color: amber enamel with darker edge grime.
- Metallic: low to medium depending on shader setup.
- Roughness: medium-high, with chipped metal exposure at edges.
- Optional emission: very low only where readability needs help.
- Atlas: `T_TRD_v0133_RouteIndicatorNameplates_1024.png`.

### TRD_RubberGasket

Use on pressure seams, sill strips, jamb gaskets, and cylinder seals.

- Base color: soot-black rubber.
- Metallic: zero.
- Roughness: medium.
- Detail: compression shine, oil at lower corners, dusty top edge.
- Avoid: modern clean plastic.

### TRD_SootGrimeDecal

Use for matte deposits around vents, lintels, pipe exits, and upper pressure-release areas.

- Base color: charcoal to brown-black.
- Metallic: zero.
- Roughness: high.
- Alpha: decal or transparent quad.
- Atlas: `T_TRD_v0133_GrimeGasketRivetAccents_1024.png`.

### TRD_OilStreakDecal

Use under piston rods, hinge points, pipe couplers, valve joints, and junction boxes.

- Base color: brown-black.
- Metallic: zero.
- Roughness: lower than soot, with a mild wet gloss.
- Alpha: decal or transparent quad.
- Placement: gravity-aligned vertical streaks only.

### TRD_CableSheathing

Use on cable/conduit bundles.

- Base colors: black rubber, brown cloth wrap, oxidized dark copper, and soot-stained gray conduit.
- Roughness: medium-high.
- Detail: clamp compression marks, dust on upper curves, worn corners at contact points.

### TRD_LiftSafetyRailPaint

Use on lift safety rails, kick plates, and warning-edge guard pieces.

- Base color: aged amber-yellow enamel over metal.
- Roughness: medium-high.
- Detail: scraped top rails, chipped post corners, oil at foot plates.
- Avoid: modern bright hazard yellow unless heavily toned down.

### TRD_RivetWearAccent

Use for rivet tops, washer caps, scraped edges, and corner chips.

- Base color: exposed iron or tarnished brass depending on parent material.
- Roughness: medium.
- Detail: small edge highlights, dark dirt caught around bolt bases.
- Atlas: `T_TRD_v0133_GrimeGasketRivetAccents_1024.png`.

## Main-Lane Material Assembly

1. Import staged PNGs with the settings above.
2. Create project-native material instances using the active Unity render pipeline.
3. Assign blackened iron as the dominant structural material.
4. Layer aged brass only where contact, function, or signage needs emphasis.
5. Use amber emission sparingly and test in low-light combat contexts.
6. Validate that decals read as environmental wear, not interact prompts.

