# CagedGaslightPrototype Brief v0.1.21

## Purpose

`CagedGaslightPrototype` is the v0.1.21 modular asset-promotion fallback for a reusable Unity-generated environment dressing component. It should read immediately as a steampunk gaslight fixture: blackened iron structure, aged brass hardware, amber glass, visible soot, and a warm point-light glow that can be repeated across the Brassworks Breach routes without creating navigation risk.

## North Star

Build a compact wall or ceiling lamp with a blackened iron wall bracket or cage, aged brass caps and ribbing, an amber glass globe, and a warm light emitter inside the globe. The silhouette should be sturdy and industrial, with a soot-dark mounting plate, visible rivets, and a small pipe feed with valve detail that makes the fixture feel plumbed into the Boilerheart pressure network rather than simply attached to a wall.

The promoted asset should be legible at route speed from mid distance: amber glow first, caged globe second, brass caps/ribs third, pipe feed and rivets on closer inspection.

## Placement Intent

The component is route-safe, reusable dressing for non-blocking wall and ceiling placement. Expected placement families:

- Pipeworks: repeated wall-bracket lamps along pipe corridors, junction turns, and service alcoves.
- Boilerheart: ceiling or high-wall lamps near machinery faces, pressure doors, and catwalk edges.
- Foundry: soot-heavy lamps on iron backplates near casting bays and heat-shielded walls.
- Governor: more deliberate bracket placement near control panels, valve wheels, and inspection platforms.

Keep the fixture outside traversal lanes, jump arcs, combat pathing, and doorway clearance. It should support route readability with warm illumination but must not become physical cover, collision clutter, or an occluding prop.

## Required Asset Identity

- Metadata component name: `CagedGaslightPrototype`
- Promotion version: `v0.1.21`
- Asset family: reusable modular environment component
- Route role: non-blocking route-safe dressing
- Supported mount styles: wall bracket, ceiling mount

## Geometry Requirements

- At least 1 amber glass globe.
- At least 4 cage ribs around or over the globe.
- At least 2 aged brass caps, such as top and bottom globe caps.
- At least 1 mounting bracket or soot-dark backplate.
- At least 1 warm light emitter positioned to read through the amber glass.
- At least 6 visible rivets on the bracket, backplate, cage, or caps.
- Include a small pipe feed and valve detail where practical for the mount style.

## Material Roles

Use distinct, named material roles so validation can confirm the fixture is not a single flat material pass:

- `blackened_iron`: cage ribs, bracket arms, structural frame.
- `aged_brass`: caps, collars, selected rib accents, valve hardware.
- `amber_glass`: globe with warm translucent or emissive read.
- `soot_dark_plate`: mounting plate or ceiling base.
- `warm_light`: point-light emitter or emissive core.
- `rivet_dark_metal`: visible rivets and small fasteners.

## Hierarchy Expectations

The Unity hierarchy should be named and inspectable. Suggested child names:

- `CagedGaslightPrototype_Root`
- `Mount_Backplate`
- `Bracket_BlackenedIron`
- `PipeFeed_Valve`
- `Globe_AmberGlass`
- `Cap_Top_AgedBrass`
- `Cap_Bottom_AgedBrass`
- `CageRibs_BlackenedIron`
- `Rivets`
- `LightEmitter_Warm`

Equivalent names are acceptable if they preserve the role, material, and count clarity.

## Route-Safe Constraints

This asset is dressing only. Do not add colliders for route-safe placement. If a future collision variant is needed, it must be split into a separate explicit variant and reviewed against route clearance. The v0.1.21 promotion target remains non-blocking.

## Acceptance Gates

- Metadata component exists and is named `CagedGaslightPrototype`.
- Promotion version is recorded as `v0.1.21`.
- Contains at least 1 amber glass globe.
- Contains at least 4 cage ribs.
- Contains at least 2 brass caps.
- Contains at least 1 mounting bracket or backplate.
- Contains at least 1 warm light emitter.
- Contains at least 6 visible rivets.
- Includes material-role validation for iron, brass, amber glass, soot-dark mount, warm light, and rivets.
- Uses a named hierarchy with readable component roles.
- Contains no colliders in the route-safe dressing variant.
- Supports Pipeworks, Boilerheart, Foundry, and Governor placement without blocking routes.

## Production Status

Documentation is ready for v0.1.21 promotion preparation. This brief is intentionally scoped to the Unity-generated `CagedGaslightPrototype` environment component and does not depend on manual route-triage notes.
