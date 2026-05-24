# Brassworks Breach - Riveted Pressure Door Frame Prototype Brief v0.1.20

Generated: `2026-05-24 07:15 -04:00`

## Purpose

Promote one reusable playable environment component for the brassworks route kit: a Unity-generated steampunk pressure/vault door frame that reads as heavy industrial threshold dressing while remaining route-safe and non-blocking. The component should frame passages, pressure locks, service bays, and vault-like route transitions without becoming a sealed door, combat blocker, or navigation snag.

## Promoted Component

Name: `RivetedPressureDoorFramePrototype`

Promoted placements:

- `Pipeworks Prototype Riveted Pressure Door Frame`
- `Boilerheart Prototype Riveted Pressure Door Frame`

## Visual Requirements

- Blackened iron arch that establishes the primary vault-door silhouette.
- Aged brass ribs layered across the arch and vertical frame members.
- One circular gear hub used as the central mechanical read.
- Cross-braces that make the frame feel pressure-rated without closing the route.
- At least two pressure-cylinder side columns, one on each side when placement allows.
- At least two amber warning lamps mounted high enough to read from the player route.
- One cream enamel pressure gauge with dark needle and readable bezel.
- At least sixteen visible rivets distributed across the arch, ribs, braces, cylinders, and base plates.
- Material-role separation for blackened iron, aged brass/copper, cream gauge face, amber lamp glass, and dark mechanical hardware.
- Route-safe non-blocking frame language: the player should read a pressure threshold, not a closed door.

## Technical Requirements

- Must use Unity-generated geometry only.
- Must not rely on Blender or external render tooling.
- Must use `RivetedPressureDoorFramePrototype` metadata with `promotionVersion = v0.1.20`.
- Must expose a named hierarchy for arch frame, brass ribs, gear hub, cross-braces, pressure cylinders, warning lamps, pressure gauge, rivets, and route-safe collision.
- Must pass material-role validation for the required iron, brass/copper, cream, amber glass, and dark hardware roles.
- Must keep a route-safe collision footprint that does not block or narrow validated navigation paths.
- Must support reusable prefab-style placement without scene-specific mesh dependencies.

## Acceptance Gates

- Metadata component present: `RivetedPressureDoorFramePrototype`.
- Promotion version exactly `v0.1.20`.
- Minimum pressure cylinder count: 2.
- Minimum warning lamp count: 2.
- Minimum pressure gauge count: 1.
- Minimum gear hub count: 1.
- Minimum rivet count: 16.
- Material-role validation passes.
- Named hierarchy validation passes.
- Route-safe collision footprint validation passes.

## Validation Targets

- Level02 placement role: `pipeworks_route_pressure_frame`.
- Level03 placement role: `boilerheart_route_pressure_frame`.
- `V0_LEVEL_VALIDATION_PASS`.
- Full Windows matrix plus package, route-QA packet, issue-triage packet, and candidate-readiness evidence once the Unity-only implementation is present.

Next-step directive: continue immediately with Unity-only implementation and validation of the promoted playable asset.
