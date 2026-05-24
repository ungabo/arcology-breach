# Brassworks Breach - Rivet Band Prototype Brief v0.1.23

Generated: `2026-05-24`

## Purpose

Promote one reusable modular environment component for the Brassworks route kit: a Unity-generated steampunk rivet band that can dress walls, gates, machinery thresholds, and pressure-rated transitions while remaining route-safe, non-blocking, and free of collision. The component should add readable industrial fastening language without becoming trim that narrows traversal, catches the player, or disguises a blocked route.

## Promoted Component

Name: `RivetBandPrototype`

Promotion version: `v0.1.23`

Promoted placement roles:

- `intake_gate_rivet_band`
- `pipeworks_wall_rivet_band`
- `boilerheart_core_rivet_band`

## North Star

Build a compact horizontal or vertical riveted rail segment with a blackened iron backing rail, an aged brass face rail or raised rib, repeated visible rivets, capped ends, and a small pressure tag or detail plate. The silhouette should read from route speed as a sturdy modular fastening band: iron backing first, brass face rail second, rivet rhythm third, then the small pressure plate on closer inspection.

The asset should feel like a common Brassworks construction detail that ties together Intake gates, Pipeworks wall runs, and Boilerheart machinery thresholds. It should be repeatable, inspectable, and easy to place in modular level dressing while staying clearly outside the playable collision contract.

## Placement Intent

The component is reusable wall, gate, and machinery-threshold dressing for route-safe placement on non-traversed surfaces. Expected placement families:

- Intake: gate frames, entry thresholds, reinforced wall edges, and early pressure-lock trim.
- Pipeworks: wall bands, pipe-bay dividers, junction surrounds, and utility-panel borders.
- Boilerheart: core machine thresholds, furnace-facing ribs, pressure-door surrounds, and heavy brasswork seams.

Keep the band flush to walls, gates, frames, or machine faces. It should support material identity and route readability but must not add physical cover, collision clutter, snag points, doorway obstruction, or ambiguous climbable ledges.

## Required Asset Identity

- Metadata component name: `RivetBandPrototype`
- Promotion version: `v0.1.23`
- Asset family: reusable modular wall/gate/machinery threshold component
- Route role: route-safe non-blocking dressing
- Geometry source: Unity-owned geometry only
- Collision contract: no colliders in the promoted route-safe variant

## Geometry Requirements

- At least 1 blackened iron backing rail.
- At least 1 aged brass face rail, raised rib, or visible brass band layer.
- At least 8 repeated visible rivets distributed along the band.
- At least 2 end caps, one per exposed band end.
- At least 1 small pressure tag, pressure label plate, inspection tab, or detail plate. This detail may be visually optional by placement style, but the v0.1.23 promoted exemplar must include one for validation.
- Use a compact profile that reads as reinforcement trim rather than a broad wall block.
- Preserve modular repeatability so multiple bands can be placed in sequence without unique scene-only geometry.

## Material Roles

Use distinct, named material roles so validation can confirm the component is not a single flat material pass:

- `blackened_iron_backing`: backing rail, rear plate, or structural rail.
- `aged_brass_face`: face rail, raised rib, or primary brass band surface.
- `dark_rivet_metal`: repeated rivet heads and small fasteners.
- `aged_brass_end_cap`: left/right end caps or cap collars.
- `pressure_detail_plate`: small pressure tag, label plate, or inspection detail.
- `soot_dark_wear`: optional grime, edge-darkening, or route-context wear accents.

Equivalent names are acceptable if they preserve role clarity and material validation can identify the iron backing, brass face rail/rib, rivets, end caps, and pressure/detail plate.

## Hierarchy Expectations

The Unity hierarchy should be named and inspectable. Suggested child names:

- `RivetBandPrototype_Root`
- `BackingRail_BlackenedIron`
- `FaceRail_AgedBrass`
- `Rivets_DarkMetal`
- `Rivet_01`
- `Rivet_02`
- `Rivet_03`
- `Rivet_04`
- `Rivet_05`
- `Rivet_06`
- `Rivet_07`
- `Rivet_08`
- `EndCap_Left_AgedBrass`
- `EndCap_Right_AgedBrass`
- `PressureDetailPlate`

Equivalent names are acceptable if they preserve the role, material, and count clarity needed by validation.

## Route-Safe Constraints

This asset is dressing only. Do not add colliders to the v0.1.23 route-safe variant. Collider count must remain `0`.

The band must stay non-blocking and flush to static dressing surfaces. It must not narrow validated navigation paths, interrupt jump arcs, create cover, block doorways, introduce snag points, or imply an interactable gate unless a separate gameplay object owns that behavior. If a future collision-enabled or interactive variant is needed, it should be split into a separate explicit variant and reviewed against route clearance.

## Acceptance Gates

- Metadata component exists and is named `RivetBandPrototype`.
- Promotion version is recorded exactly as `v0.1.23`.
- Contains at least 1 blackened iron backing rail.
- Contains at least 1 aged brass face rail or raised rib.
- Contains at least 8 visible rivets.
- Contains at least 2 end caps.
- Contains at least 1 pressure/detail plate.
- Includes material-role validation for blackened iron backing, aged brass face rail/rib, rivets, end caps, and pressure/detail plate.
- Uses a named hierarchy with readable component roles and count clarity.
- Uses Unity-owned geometry only.
- Contains no colliders in the route-safe dressing variant.
- Preserves route-safe non-blocking placement for `intake_gate_rivet_band`, `pipeworks_wall_rivet_band`, and `boilerheart_core_rivet_band` roles.

## Production Status

Documentation has been integrated into the v0.1.23 promoted Unity component slice. Unity implementation, scene placement, route audit, build validation, package evidence, and release docs are now complete in the main lane.

Next-step directive: continue with Unity-only implementation and validation of the promoted route-safe rivet band asset.
