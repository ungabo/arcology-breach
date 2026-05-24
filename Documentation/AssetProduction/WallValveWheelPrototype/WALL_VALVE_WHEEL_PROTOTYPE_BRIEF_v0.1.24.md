# WallValveWheelPrototype Production Brief

Promotion version: v0.1.24  
Component owner: Unity  
Geometry source: Unity-owned geometry only  
Implementation state: promoted  
Validation state: full_matrix_passed

## North-Star Style

WallValveWheelPrototype is a route-safe steampunk wall dressing component for Brassworks Breach. It should read as a heavy industrial control fixture without functioning as gameplay unless a separate gameplay script explicitly owns that behavior later.

The visual target is:

- Blackened iron backplate mounted flat to a wall surface.
- Aged brass valve wheel centered on the fixture.
- Central spindle and raised hub tying the wheel to the backplate.
- Visible rivets or bolts on the backplate and hub assembly.
- Pressure label or metal tag mounted on or near the plate.
- Small pointer or index mark that gives the wheel a calibrated mechanical read.

The finish should feel worn, oily, heat-exposed, and utilitarian rather than decorative. Materials should support Brassworks Breach's brassworks language: dark iron mass, warmer aged brass controls, readable metal tags, and small high-contrast marks for mechanical orientation.

## Route Safety

This component is wall dressing only.

- Must not include colliders.
- Must be non-blocking.
- Must not narrow navigation lanes.
- Must remain flush or close to the wall enough to preserve player movement.
- Must not imply interactability through prompts, glow states, animation, audio, or scripting.
- Any future interaction must be owned by a separate gameplay script and tracked as a different implementation decision.

## Placement Roles

Expected placement roles:

- `intake_wall_valve_wheel`
- `pipeworks_route_valve_wheel`
- `boilerheart_core_valve_wheel`

The same promoted component may be reused across these roles with scale, material intensity, or surrounding dressing changes, provided the named hierarchy and route-safety requirements remain intact.

## Required Hierarchy

The promoted prefab or scene-owned prototype should use a named hierarchy that clearly exposes the authored parts:

- `WallValveWheelPrototype`
- `Backplate_BlackenedIron`
- `Wheel_AgedBrass`
- `Hub_CentralSpindle`
- `Fasteners_RivetsBolts`
- `Label_PressureTag`
- `Pointer_IndexMark`

Additional children are allowed when needed for construction detail, but the required names must remain present and recognizable.

## Material Roles

Required material roles:

- `blackened_iron_backplate`
- `aged_brass_valve_wheel`
- `dark_spindle_hub`
- `rivet_bolt_metal`
- `pressure_label_tag`
- `pointer_index_mark`

Materials may share atlases or shader settings, but these roles must remain identifiable in metadata and visual review.

## Minimum Counts

Minimum visible detail counts:

- Backplate: 1
- Valve wheel: 1
- Central spindle/hub: 1
- Visible rivets or bolts: 4
- Pressure label/tag: 1
- Pointer/index mark: 1
- Colliders: 0

## Acceptance Gates

For promotion in v0.1.24, the component must pass these gates:

- Metadata component present on the promoted root.
- Promotion version exactly `v0.1.24`.
- Named hierarchy contains the required root and child roles.
- Material roles are assigned and visually match the Brassworks Breach steampunk north star.
- Minimum counts are met for wheel, hub, fasteners, label/tag, and pointer/index mark.
- No collider components are present on the root or children.
- Placement remains route-safe, non-blocking, and does not narrow navigation.
- Component does not advertise interaction unless a separate gameplay script owns that behavior in a later task.

## Notes For Implementation

Build from Unity-owned geometry only. Do not depend on external mesh sources or untracked imported assets for the promoted geometry. Keep the silhouette compact enough for corridor and machinery-wall use, with detail concentrated on the face so it reads clearly from oblique route angles.

## Production Status

Documentation has been integrated into the v0.1.24 promoted Unity component slice. Unity implementation, scene placement, route audit, build validation, package evidence, and release docs are now complete in the main lane.

Next-step directive: continue immediately with the next highest-impact unfinished task.
