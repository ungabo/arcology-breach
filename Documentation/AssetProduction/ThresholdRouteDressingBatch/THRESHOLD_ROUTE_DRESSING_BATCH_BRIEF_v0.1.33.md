# Threshold Route Dressing Batch Brief v0.1.33

Status: staged production package for Unity-only main-lane integration
Project: Brassworks Breach
Package: `ThresholdRouteDressingBatch`
Prepared: 2026-05-24
Write scope: `Documentation/AssetProduction/ThresholdRouteDressingBatch/` and `Assets/_Project/ArtStaging/ThresholdRouteDressingBatch/`

## Intent

`ThresholdRouteDressingBatch` is a substantial route-threshold dressing kit for Brassworks Breach. It is not a single prop brief. The batch gives environment art a reusable set of visual parts for doors, route apertures, lift mouths, pipework crossings, pressure-gate jambs, and service corridors.

The package is presentation-only. It must not own route state, gameplay locks, navigation blockers, triggers, interact prompts, save data, combat scripting, damage rules, or scene-builder decisions. Future main-lane integration can assemble prefabs from the staged notes, texture sheets, and primitive build specs, but scene placement and validation are intentionally outside this slice.

## Unity-Only Construction

Build the resulting prefabs inside Unity using primitives, ProBuilder-style meshes, project shaders, simple procedural mesh helpers, decals, and the staged PNG sheets. No Blender, Maya, Houdini, Substance, or external DCC dependency is expected for v0.1.33.

Allowed sources:

- Unity primitives: cube, cylinder, capsule, sphere, quad, plane.
- Existing in-project materials and shaders, plus material instances derived from the notes in this package.
- ProBuilder or equivalent in-Unity mesh editing, if already available.
- Editor-only procedural helpers for rivet rows, gasket strips, cable bundles, and simple panel bevels.
- The staged source PNG sheets under `Assets/_Project/ArtStaging/ThresholdRouteDressingBatch/Textures/`.

Out of scope:

- Scene builder changes.
- Validator changes.
- Generated scene edits.
- Shared documentation edits.
- Build, version, release, or git-state edits.
- Gameplay blockers, trigger colliders, interactables, route locks, or nav modifiers.

## Art Direction

The dressing should read as heavy brassworks threshold hardware: blackened iron structure, aged brass wear, amber industrial route signage, grimy pressure seams, cable and conduit clutter, lift-rail safety language, and practical rivet/wear accents.

Primary mood:

- Industrial pressure containment.
- Oily, soot-stained metal around route transitions.
- Diegetic amber route guidance that feels bolted to the world.
- Dense utility detail without blocking the player path.
- Worn steampunk craft, not clean sci-fi UI.

Avoid:

- Polished gold, plastic-black metal, neon objective markers, floating UI, or overly decorative filigree.
- Any asset silhouette that suggests an interactable button unless a later gameplay task explicitly owns that behavior.
- Dense low-height clutter in the walking path.

## Staged Asset Families

### Piston Door Braces

Purpose: reinforce pressure-door jambs, sealed route thresholds, and non-opening mechanical gates.

Build notes:

- Use paired vertical blackened-iron back plates on left and right sides of a route aperture.
- Add cylinder bodies as horizontal or diagonal cylinders, with slimmer aged-brass piston rods.
- Use clevis blocks, hinge knuckles, and rows of large rivets to make the assembly look load-bearing.
- Keep the center clearance readable. The braces dress the sides, not the doorway core.
- Add oil streak decals below rod seals and soot along top anchor blocks.

Suggested scale:

- Overall width: 3.2 m to 4.4 m.
- Overall height: 3.0 m to 4.2 m.
- Side brace width: 0.35 m to 0.65 m.
- Brace depth from wall: 0.18 m to 0.38 m.
- Minimum route clearance: 1.8 m wide by 2.4 m tall.

### Pipe Clamps And Couplers

Purpose: make route thresholds feel threaded through active steam, pressure, and service infrastructure.

Build notes:

- Use cylinders for pipes and split-ring clamp geometry from torus-like segmented cylinders or ProBuilder arcs.
- Add saddle blocks and bolted bridge straps where pipes cross a doorway side or top.
- Couplers should include banded sleeves, bolt lugs, small leak stains, and heat-blued edges.
- Create straight, elbow, wall-flange, and overhead crossing variants.
- Keep all pipework outside the main movement aperture unless it is above head height.

### Grime Panels

Purpose: provide local grime breakup behind door hardware, below pipe couplers, and around lift thresholds.

Build notes:

- Use thin quads or decal projectors with `T_TRD_v0133_GrimeGasketRivetAccents_1024.png`.
- Vertical streaks should originate below moving seals, pipe joints, vents, hinges, and gasket compression points.
- Soot should gather on upper seams and pressure-release vents.
- Oil should be darker and glossier than soot.
- Avoid random noise placement. Every grime mark should imply gravity, heat, or mechanical contact.

### Amber Route Indicator Plates

Purpose: diegetic route readability without relying on HUD markers.

Build notes:

- Use the amber plate atlas for small in-world signs: route arrows, lift access, boilerheart, valve run, core span, drain exit, and pressure gate labels.
- Mount plates on walls, brace caps, rail posts, or pipe supports.
- Keep emission subtle if used. The plates should glow like dirty industrial glass or enamel, not like sci-fi signage.
- Add protective blackened-iron rims, bolt heads, and grime around plate corners.

### Cable And Conduit Bundles

Purpose: add utility density and directional flow across thresholds.

Build notes:

- Build with Bezier-like procedural tubes, cylinders, or prefabbed cable splines inside Unity.
- Use varied cable diameters and materials: black rubber, brown cloth wrap, oxidized copper conduit, and brass clips.
- Bundle clamps should line up with wall studs, brace backs, or pipe brackets.
- Leave breathing room around nameplates and route indicators.
- Terminate cables into junction boxes or under clamp covers, not loose open ends.

### Threshold Nameplates

Purpose: label important route transitions with grounded Brassworks language.

Build notes:

- Use amber or tarnished-brass nameplates with blackened frames.
- Suggested labels: `THRESHOLD B-17`, `BOILERHEART`, `FREIGHT LIFT`, `PRESSURE GATE`, `VALVE RUN`, `CORE SPAN`, `FURNACE BAY`.
- Place over lintels, on brace center caps, or beside lift mouths.
- Include blank variants for future designer-authored destinations.
- Do not place labels where they read as interact prompts.

### Lift Safety Rails

Purpose: dress freight-lift thresholds and route drops with period-appropriate safety structure.

Build notes:

- Use aged yellow-brass or worn amber enamel rails, dark iron posts, and riveted floor plates.
- Include top rail, mid rail, vertical posts, corner elbows, and warning kick plate variants.
- Rails should define the edge of a lift or platform without blocking intended entry.
- Add scraped hand-contact wear on top rails and dark oil at post bases.

### Gasket Seams

Purpose: sell pressure sealing around heavy doors, lift frames, and pipe service panels.

Build notes:

- Use thin dark rubber strips with bolted retainers.
- Make vertical jamb strips, overhead lintel strips, floor sill strips, and short patch seams.
- Add compression shine, chipped retainers, and grime collecting at lower corners.
- Gasket strips should sit proud enough to catch light but remain low profile.

### Rivet And Wear Accents

Purpose: provide reusable scale cues and edge-wear breakup across every family.

Build notes:

- Use instanced rivets, bolt heads, washer caps, scraped edge decals, and worn corner masks.
- Favor consistent spacing for engineered rows with small interruptions from repairs or patch plates.
- Put heavier wear on player-facing corners, hand-contact rails, piston rod collars, and lower sill plates.
- Keep rivet density readable at gameplay distance.

## Suggested Unity Hierarchy

```text
ThresholdRouteDressingBatch
  VisualRoot
    PistonDoorBraces
      BraceVariant_A_WidePressureJamb
      BraceVariant_B_CompactServiceJamb
    PipeClampsCouplers
      ClampSet_StraightWallRun
      CouplerSet_OverheadCrossing
      CouplerSet_ValveSideBranch
    GrimePanels
      Decal_OilStreakVertical
      Decal_SootSwipeUpper
      Decal_LeakUnderClamp
    AmberRouteIndicatorPlates
      Plate_RouteArrowStraight
      Plate_RouteArrowLeftRight
      Plate_LiftAccess
      Plate_PressureGate
    CableConduitBundles
      Bundle_WallTrunk
      Bundle_OverLintelSag
      Bundle_JunctionDrop
    ThresholdNameplates
      Nameplate_Boilerheart
      Nameplate_FreightLift
      Nameplate_Blank
    LiftSafetyRails
      Rail_StraightEntry
      Rail_CornerGuard
      Rail_KickPlate
    GasketSeams
      Seam_VerticalJamb
      Seam_TopLintel
      Seam_SillStrip
    RivetWearAccents
      RivetRow_Large
      RivetRow_Small
      Wear_EdgeScrape
      Wear_CornerChip
  NonGameplayBounds_EditorOnly
```

Guidance:

- `VisualRoot` owns rendered objects only.
- Bounds helpers must be editor-only or explicitly non-authoritative.
- Any future collider should be visual support collision only and must not close a route aperture.
- Keep category roots separate so main-lane integration can lift individual families into separate prefabs.

## Material Targets

Blackened iron:

- Charcoal base with blue-gray heat variation.
- Metallic high, roughness medium-high.
- Localized oil gloss and scraped edge highlights.

Aged brass:

- Muted brass, ochre, tarnished brown, slight oxidation in recesses.
- Metallic high, roughness medium.
- Contact edges glossier on piston rods, rail tops, rivet heads, and plate corners.

Amber route enamel or glass:

- Dirty amber-orange base.
- Low to medium emission only when needed for readability.
- Edge grime and chipped paint prevent a clean UI read.

Rubber gasket:

- Dark rubber or soot-black base.
- Roughness medium with compression shine at contact seams.
- Oil accumulation at lower sill and hinge-side corners.

Grime:

- Soot is matte charcoal.
- Oil is brown-black and glossier.
- Oxidation is restrained green-brown, mostly inside recesses.

## Staged Sheets

The following staged images are ready for Unity import review:

- `Assets/_Project/ArtStaging/ThresholdRouteDressingBatch/Textures/T_TRD_v0133_MaterialSwatches_1024.png`
- `Assets/_Project/ArtStaging/ThresholdRouteDressingBatch/Textures/T_TRD_v0133_RouteIndicatorNameplates_1024.png`
- `Assets/_Project/ArtStaging/ThresholdRouteDressingBatch/Textures/T_TRD_v0133_GrimeGasketRivetAccents_1024.png`
- `Assets/_Project/ArtStaging/ThresholdRouteDressingBatch/Previews/PREVIEW_ThresholdRouteDressingBatch_ContactSheet_v0.1.33.png`

Import defaults:

- Texture type: `Default`.
- sRGB: on for color plates, nameplates, material swatch review, and contact sheet.
- Alpha transparency: on for route plates and grime/gasket/rivet accents.
- Wrap: clamp for decals and plates; repeat is acceptable for gasket/rivet strip tests.
- Mip maps: on for world use.
- Compression: high quality after readability review; uncompressed is acceptable during staging review.
- Max size: 1024 for the staged source sheets; contact sheet can remain preview-only.

## Acceptance Criteria

- The batch contains all requested families: piston door braces, pipe clamps/couplers, grime panels, amber route indicator plates, cable/conduit bundles, threshold nameplates, lift safety rails, gasket seams, and rivet/wear accents.
- Assets are Unity-only and do not require Blender or external DCC tools.
- Route aperture dressing remains presentation-only and non-authoritative.
- Art staging includes importable PNG sheets and a contact-sheet preview.
- Documentation and manifests live only inside the requested package folders.
- Main-lane integration can create prefabs/material instances from the notes without editing scene builder, validator, generated scenes, shared docs, build/version files, or git state.

## Main-Lane Handoff

Ready for main-lane integration as a staged source package. The next owner can:

- Import the PNG sheets.
- Create material instances from `ThresholdRouteDressingBatch_MaterialSet_v0.1.33.json` and the material notes.
- Assemble primitive/procedural prefab variants from the asset manifest.
- Place selected variants in scene-specific work only after the main lane owns placement.
