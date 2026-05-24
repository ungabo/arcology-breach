# Brassworks Breach - Pipe Canopy Prototype Brief v0.1.22

Generated: `2026-05-24`

## Purpose

Promote one reusable overhead environment component for the Brassworks route kit: a Unity-generated pipe canopy that reads as a heavy steampunk pressure-run above the player route while remaining route-safe, non-blocking, and free of collision. The component should add dense industrial ceiling language to generated levels without becoming cover, doorway clutter, or navigation geometry.

## Promoted Component

Name: `PipeCanopyPrototype`

Promotion version: `v0.1.22`

Promoted placement roles:

- `intake_route_pipe_canopy`
- `pipeworks_route_pipe_canopy`
- `boilerheart_route_pipe_canopy`
- `foundry_route_pipe_canopy`
- `governor_route_pipe_canopy`

## North Star

Build a compact overhead bundle of aged brass pipes held by blackened iron collars and brackets. The silhouette should read from route speed as a pressure-service canopy: parallel brass pipes first, iron clamp rhythm second, rivets and couplers third, then a small valve or pressure detail on closer inspection.

The asset should feel like part of the Boilerheart pressure network extending through Intake, Pipeworks, Foundry, and Governor spaces. It should be sturdy, modular, and repeatable, with enough detail density to support heavy steampunk levels while staying visually clear above the traversal lane.

## Placement Intent

The component is reusable overhead dressing for route-safe placement above corridors, service bays, catwalk approaches, and machine-facing transitions. Expected placement families:

- Intake: early pressure infrastructure above entry corridors and service thresholds.
- Pipeworks: repeated overhead bundles at junctions, bends, and long pipe corridors.
- Boilerheart: denser canopy runs near machinery faces, pressure doors, and catwalk edges.
- Foundry: soot-dark overhead utility runs near casting bays and heat-shielded walls.
- Governor: more deliberate canopy segments near control stations, inspection platforms, and valve banks.

Keep the canopy above traversal lanes and outside jump arcs, combat pathing, doorway clearance, and camera-critical sightlines. It should support route readability and atmosphere but must not become physical cover, collision clutter, or an occluding prop.

## Required Asset Identity

- Metadata component name: `PipeCanopyPrototype`
- Promotion version: `v0.1.22`
- Asset family: reusable modular overhead environment component
- Route role: route-safe non-blocking ceiling or high-wall dressing
- Geometry source: Unity-owned geometry only

## Geometry Requirements

- At least 4 visible aged brass pipe runs in the bundle.
- At least 5 blackened iron collars or brackets distributed across the canopy span.
- At least 10 visible collar rivets across the collars, brackets, or mounting plates.
- At least 2 pipe couplers or sleeve joints interrupting the pipe runs.
- At least 1 valve, pressure gauge, pressure cap, or similar pressure-service detail.
- Include enough spacing between pipes and collars for the bundle to read clearly from below.
- Avoid broad solid plates that turn the canopy into a ceiling block or visual wall.

## Material Roles

Use distinct, named material roles so validation can confirm the component is not a single flat material pass:

- `aged_brass_pipe`: primary pipe bundle and selected small fittings.
- `blackened_iron`: collars, brackets, clamp straps, and support arms.
- `dark_rivet_metal`: visible rivets and small fasteners.
- `aged_brass_coupler`: couplers, sleeve joints, and pipe unions.
- `pressure_detail`: valve wheel, pressure gauge, cap, or service fitting.
- `soot_dark_shadow`: optional underside grime, heat staining, or bracket shadow accents.

Equivalent names are acceptable if they preserve role clarity and material validation can identify brass pipes, blackened iron structure, rivets, couplers, and pressure detail.

## Hierarchy Expectations

The Unity hierarchy should be named and inspectable. Suggested child names:

- `PipeCanopyPrototype_Root`
- `PipeBundle_AgedBrass`
- `Pipe_01_AgedBrass`
- `Pipe_02_AgedBrass`
- `Pipe_03_AgedBrass`
- `Pipe_04_AgedBrass`
- `Collars_BlackenedIron`
- `Brackets_BlackenedIron`
- `Couplers_AgedBrass`
- `Rivets_CollarFasteners`
- `ValveOrPressureDetail`

Equivalent names are acceptable if they preserve the role, material, and count clarity needed by validation.

## Route-Safe Constraints

This asset is dressing only. Do not add colliders to the v0.1.22 route-safe variant. Collider count must remain `0`.

The canopy must stay non-blocking and overhead. It must not narrow validated navigation paths, interrupt jump arcs, create cover, block doorways, or add physical snag points. If a future collision-enabled variant is needed, it should be split into a separate explicit variant and reviewed against route clearance.

## Acceptance Gates

- Metadata component exists and is named `PipeCanopyPrototype`.
- Promotion version is recorded exactly as `v0.1.22`.
- Contains at least 4 aged brass pipe runs.
- Contains at least 5 blackened iron collars or brackets.
- Contains at least 10 visible collar rivets.
- Contains at least 2 pipe couplers or sleeve joints.
- Contains at least 1 valve, gauge, cap, or pressure-service detail.
- Includes material-role validation for aged brass pipes, blackened iron collars/brackets, rivets, couplers, and pressure detail.
- Uses a named hierarchy with readable component roles and count clarity.
- Uses Unity-owned geometry only.
- Contains no colliders in the route-safe dressing variant.
- Preserves route-safe non-blocking overhead placement for Intake, Pipeworks, Boilerheart, Foundry, and Governor roles.

## Production Status

Documentation is ready for v0.1.22 promotion preparation. This brief is intentionally documentation-only and does not claim Unity implementation, scene placement, build validation, or route-audit completion.

Next-step directive: continue with Unity-only implementation and validation of the promoted route-safe overhead asset.
