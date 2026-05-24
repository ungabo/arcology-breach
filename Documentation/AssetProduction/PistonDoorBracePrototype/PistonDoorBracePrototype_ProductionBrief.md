# PistonDoorBracePrototype Production Brief

Status: preparation brief for a future Unity environment component
Component: `PistonDoorBracePrototype`
Target folder: `D:\__MY APPS\Unity Doom\Documentation\AssetProduction\PistonDoorBracePrototype\`
Prepared: 2026-05-24

## Intent

`PistonDoorBracePrototype` is a visual route-threshold dressing asset for Brassworks Breach. It should read as a pair of heavy steampunk pressure-door side braces that imply a reinforced industrial gate, emergency seal, or steam-locked transition point. The component must not own gameplay authority: no route logic, lock state, nav gating, trigger decisions, enemy blockers, damage behavior, save state, or progression rules. Future designers may align it near a route threshold, but the asset itself remains presentation-only unless a later gameplay ticket explicitly adds separate authority.

The north-star is heavy steampunk rather than light decorative brasswork: blackened iron structural brackets, aged brass piston rods, hydraulic/steam cylinders, oversize rivets, oil grime, soot staining, heat-darkened seams, and small amber pressure indicators. The final silhouette should make a doorway feel mechanically compressed and dangerous without making it visually confusing as a player-operated lock.

## Unity-Only Scope

Build the prototype inside Unity from primitives, ProBuilder-style meshes, procedural mesh helpers, or existing in-project generic materials only. Do not require Blender, Maya, Substance, Houdini, or new external authoring tools for the prototype stage. If later production wants bespoke meshes or texture baking, that should be a separate follow-up.

Allowed construction sources:

- Unity primitives: cubes, cylinders, capsules, spheres, planes, and quads.
- ProBuilder or equivalent Unity package mesh editing, if already available in the project.
- Procedural mesh scripts for bevel strips, rivet rows, hinge plates, or indicator housings, provided they are editor-only or asset-generation-only.
- Existing project shaders/material systems.
- New Unity materials or material instances, if created later in the actual Unity project by the parent implementation agent.

Out of scope for this documentation slice:

- Prefab creation.
- Scene placement.
- Code changes.
- Gameplay triggers, doors, locks, nav blockers, or route-state scripts.
- Build pipeline changes.
- Release notes or shared status-doc edits.

## Visual Design

The brace should frame the left and right sides of a future pressure-door or route aperture. Each side is a vertical blackened-iron assembly with piston hardware mounted across layered brackets. The design should look load-bearing and functional, not purely ornamental.

Primary read:

- Two tall side braces flank a doorway or threshold line.
- Each side brace has a broad iron spine plate bolted to the wall.
- Aged brass piston rods run diagonally or vertically between black iron clevis mounts.
- Steam/hydraulic cylinders sit proud of the wall, capped with iron collars.
- Rivets and bracket bolts define scale and construction logic.
- Oil streaks gather below moving rods and cylinder seals.
- Soot darkens upper edges, vent seams, and pressure-release outlets.
- Small amber indicators sit near the cylinder heads or cross-brace junctions.

Silhouette priorities:

- Strong vertical outer rails.
- Chunky upper and lower mounting plates.
- Cylinders offset from the wall so they catch highlights.
- Clear empty center path so the future route remains legible.
- No central moving slab in this prototype unless it is a non-authoritative background suggestion.

Mood targets:

- Industrial pressure containment.
- Aged brass machinery under soot.
- Black iron mass and heat wear.
- Functional threshold dressing for a hostile brassworks facility.

## Suggested Hierarchy

Recommended prefab-style hierarchy for future implementation:

```text
PistonDoorBracePrototype
  VisualRoot
    LeftBrace
      IronBackPlate_L
      OuterRail_L
      InnerRail_L
      UpperAnchor_L
      LowerAnchor_L
      CylinderBodyA_L
      CylinderBodyB_L
      BrassRodA_L
      BrassRodB_L
      ClevisMounts_L
      RivetRows_L
      IndicatorAmber_L
      GrimeDecals_L
    RightBrace
      IronBackPlate_R
      OuterRail_R
      InnerRail_R
      UpperAnchor_R
      LowerAnchor_R
      CylinderBodyA_R
      CylinderBodyB_R
      BrassRodA_R
      BrassRodB_R
      ClevisMounts_R
      RivetRows_R
      IndicatorAmber_R
      GrimeDecals_R
    OptionalTopCrossClamp
    OptionalBottomSillClamp
  NonGameplayBounds_EditorOnly
```

Notes:

- `VisualRoot` should contain all rendered geometry.
- Any bounds helper must be editor-only or clearly non-gameplay.
- Do not add trigger colliders, interactables, door controllers, route-state components, or nav modifiers.
- If static collision is desired later for decoration, it should be simple wall-hugging collision and must not block the intended route aperture.

## Unity Primitive And Procedural Construction

Use a modular primitive approach so the prototype can be tuned directly in a Unity scene.

### Scale

Baseline dimensions for a human-scale industrial threshold:

- Overall visual width: 3.2 to 4.2 meters.
- Overall visual height: 3.0 to 4.0 meters.
- Each side brace width: 0.35 to 0.55 meters.
- Center route clearance: at least 1.8 meters wide and 2.4 meters tall unless a specific door kit requires otherwise.
- Brace depth from wall: 0.15 to 0.35 meters.
- Cylinder body diameter: 0.12 to 0.22 meters.
- Brass rod diameter: 0.035 to 0.07 meters.

### Iron Back Plates

Construct each back plate from a flattened cube:

- Scale as tall vertical plate.
- Add bevels through ProBuilder or bevel shader/mesh helper if available.
- Segment into upper, middle, and lower plate sections if a single large plate feels too plain.
- Add inset strips along both vertical edges to catch rim light.

### Rails And Clamp Blocks

Use cubes for rails and clamp blocks:

- Outer rail: thick blackened iron rectangular bar.
- Inner rail: thinner blackened iron bar near the route opening.
- Upper and lower anchor blocks: broad caps that look bolted into the wall.
- Optional top cross clamp: short overhead bridge set back from the route, useful for reinforcing the threshold read.
- Optional bottom sill clamp: low iron footing that avoids becoming a trip-height gameplay blocker.

### Cylinders

Use cylinders oriented along their local length:

- Cylinder bodies should be blackened iron or dark gunmetal with brass caps.
- Add slightly larger cylinder caps at both ends.
- Place cylinders proud of the back plate, supported by clevis mounts.
- Pair one upper and one lower cylinder per side for a dense pressure-machine read.
- Consider diagonal rods crossing from upper outer mount to lower inner mount for dynamic tension, but keep the center route clear.

### Brass Piston Rods

Use slim cylinders for rods:

- Material target: aged brass with bright worn edges.
- Rods should visibly pass into cylinder collars.
- Rod ends should terminate in clevis forks or circular hinge knuckles.
- Rods may be partially extended asymmetrically, but both sides should remain compositionally balanced.

### Rivets And Bolts

Use small cylinders, UV spheres, or procedural instanced rivets:

- Place rows along outer rails and back plates.
- Use larger bolt heads on anchor blocks and clevis mounts.
- Keep rivet spacing regular enough to read as engineered, with a few size variations for hand-built grime.
- Avoid excessive rivet density that turns into visual noise at gameplay distance.

### Amber Pressure Indicators

Use small emissive lenses:

- Place one amber indicator per side near the upper cylinder head or central pressure manifold.
- Shape options: small circular lens, vertical capsule gauge, or tiny protected lamp.
- Include a dark metal cage, rim, or bracket around the lens.
- Emission should be subtle and readable, not a bright sci-fi UI marker.

### Grime, Oil, And Soot

Use Unity decals, material masks, vertex colors, or layered transparent planes:

- Oil streaks: dark glossy vertical marks below cylinder seals and hinge points.
- Soot: matte dark deposits above vents, seams, and upper pressure-release areas.
- Edge wear: lighter exposed metal on corners, bolt tops, rod contact points, and handles.
- Brass oxidation: dull brown/green tint in creases, not a bright polished gold finish.

## Material Targets

The prototype should establish material intent even if final texture assets arrive later.

### Blackened Iron

- Base color: near-black charcoal with blue-gray or brown heat variation.
- Roughness: medium-high, broken by oily glossy streaks.
- Metallic: high if using a metallic workflow.
- Detail: chipped edges, soot dust, scraped corners, and subtle pitted noise.
- Avoid: clean flat black, plastic sheen, or uniformly bright gunmetal.

### Aged Brass

- Base color: muted brass, dark ochre, aged gold, and brown tarnish.
- Roughness: medium, with worn rod edges slightly glossier.
- Metallic: high.
- Detail: oxidation in recesses, polish on contact surfaces, oil smears near seals.
- Avoid: saturated yellow gold or clean showroom brass.

### Steam/Hydraulic Cylinder Rubber And Seals

- Base color: dark rubber or soot-stained black.
- Roughness: medium.
- Detail: narrow rings around piston collars, subtle oil shine.
- Avoid: readable modern plastic unless heavily aged.

### Amber Indicator Glass

- Base color: dark amber/orange.
- Emission: low to medium, tuned for visible close-range glow.
- Detail: small highlight, grime around rim, optional flicker material parameter for future use.
- Avoid: bright warning-light authority that implies an interactable objective.

### Oil And Soot

- Oil: dark brown/black, low roughness, vertical streaking.
- Soot: charcoal matte, high roughness, soft feathered edges.
- Grime: layered around lower braces, bolt clusters, and hinge collars.

## Acceptance Criteria

The future Unity asset should pass all of the following:

- The asset reads immediately as heavy steampunk pressure-door side bracing.
- The center route remains visually legible and physically unobstructed by the decorative braces.
- Blackened iron is the dominant structural material.
- Aged brass piston rods are visible and important to the silhouette.
- Steam or hydraulic cylinder bodies are present on both sides.
- Rivets or bolts communicate industrial assembly and scale.
- Oil grime and soot are visible in plausible mechanical locations.
- Small amber pressure indicators exist on both sides and remain subtle.
- The asset does not include gameplay authority components.
- The asset does not include trigger colliders, route-lock scripts, nav blockers, damage scripts, or interaction prompts.
- The asset can be built or revised inside Unity using primitives, ProBuilder, or procedural generation.
- Geometry density is appropriate for an environment dressing asset and can be optimized with instancing for repeated rivets.
- Materials can be tuned under the existing project lighting without requiring external texture baking.
- The silhouette holds up from gameplay distance and in close inspection.
- The asset name and hierarchy clearly identify it as `PistonDoorBracePrototype`.

## Validation Checklist

Before considering the prototype ready for parent-agent integration:

- Confirm all files and generated notes for this preparation slice live only under `Documentation\AssetProduction\PistonDoorBracePrototype\`.
- Confirm no Unity scenes, scripts, prefabs, build settings, release notes, or shared status docs were changed by this slice.
- Confirm the planned hierarchy has no gameplay controller components.
- Confirm any collider proposal is marked visual/static only and not route-authoritative.
- Confirm route aperture clearance remains readable from first-person camera height.
- Confirm the brace can be mirrored cleanly left/right without obvious UV or grime repetition problems.
- Confirm rivet counts are either instanced or low enough for the target scene budget.
- Confirm amber indicators do not look like quest markers, pickups, or interactive buttons.
- Confirm soot and oil placement follows gravity, heat, and mechanical contact points.
- Confirm materials remain readable under warm brassworks lighting and darker combat lighting.

## Unity Test-Render Evaluation

When the future Unity prototype exists, evaluate it with a small test-render scene rather than judging only in Scene view.

Recommended setup:

- Place the brace around a neutral doorway-sized opening.
- Use the same approximate camera height and FOV as the target game.
- Add warm industrial key light from above/front-left.
- Add dim cool fill from the route interior.
- Add a low amber practical light near one indicator to test emission balance.
- Use the project's normal post-processing stack if available.

Required captures:

- Front-on gameplay-distance capture.
- Three-quarter angle capture showing cylinder depth.
- Close-up capture of piston rod, clevis, rivets, grime, and amber indicator.
- Low-light capture to ensure blackened iron does not collapse into a flat silhouette.
- Bright-light capture to ensure brass does not become clean gold.

Evaluation questions:

- Does the asset read as a threshold brace in under two seconds?
- Is the route opening still obvious?
- Are rods and cylinders identifiable without explanatory text?
- Do material differences survive the lighting setup?
- Does amber glow feel diegetic and subtle?
- Does grime reinforce mechanical use rather than random dirt?
- Are there any shapes that imply interactability or route authority?
- Does the asset feel at home in the heavy steampunk Brassworks Breach direction?

Pass condition:

The test render should make the threshold feel reinforced, oily, soot-stained, and mechanically pressurized while leaving gameplay ownership to separate systems.

## Handoff Notes

This brief is intentionally documentation-only. The parent v0.1.32 implementation can continue independently. A future implementation ticket can consume this folder as the production target for a Unity prefab or prototype scene, but that future work should be scoped separately and should preserve the non-authoritative visual-dressing intent unless explicitly changed.
