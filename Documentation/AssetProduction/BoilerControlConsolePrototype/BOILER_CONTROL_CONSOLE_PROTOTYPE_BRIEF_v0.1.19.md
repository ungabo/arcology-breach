# Brassworks Breach - Boiler Control Console Prototype Brief v0.1.19

Generated: `2026-05-24 06:44 -04:00`

## Purpose

Promote one reusable playable environment component for the brassworks route kit: a steampunk boiler and control console that can read as both machinery dressing and a route-safe interaction anchor. The component should be self-contained, Unity-generated, and suitable for repeated placement in boiler rooms, pipeworks alcoves, and control nooks without requiring external art packages.

## Promoted Component

Name: `BoilerControlConsolePrototype`

Promoted placements:

- `Pipeworks Prototype Boiler Control Console`
- `Boilerheart Prototype Boiler Control Console`

## Visual Requirements

- Blackened iron base with a stable floor footprint.
- Aged brass bevels, rails, trim strips, and protective edge language.
- Angled control panel readable from the player route.
- Lever bank with at least three distinct brass-handled levers.
- Gauge cluster with at least two cream enamel pressure gauges.
- One red/brass valve wheel mounted as a readable control feature.
- At least three colored indicator lamps.
- At least twelve visible rivets distributed across the base, rails, and panel.
- Pressure pipes that connect the console language back into the boiler route kit.
- Material-role separation for iron, brass/copper, cream gauge faces, lamp glass, and warning red.

## Technical Requirements

- Must use Unity-generated geometry only.
- Must not rely on Blender or external render tooling.
- Must use `BoilerControlConsolePrototype` metadata with `promotionVersion = v0.1.19`.
- Must expose a named hierarchy for base, angled panel, lever bank, gauge cluster, valve wheel, indicator lamps, rivets, and pressure pipes.
- Must pass material-role validation for the required iron, brass/copper, cream, glass, and warning-red roles.
- Must keep a route-safe collision footprint that does not block navigation through validated paths.
- Must support reusable prefab-style placement without scene-specific mesh dependencies.

## Acceptance Gates

- Metadata component present: `BoilerControlConsolePrototype`.
- Promotion version exactly `v0.1.19`.
- Minimum lever count: 3.
- Minimum gauge count: 2.
- Minimum indicator lamp count: 3.
- Minimum rivet count: 12.
- Material-role validation passes.
- Named hierarchy validation passes.
- Route-safe collision footprint validation passes.

## Validation Targets

- Level02 placement role: `pipeworks_route_console`.
- Level03 placement role: `boilerheart_route_console`.
- `V0_LEVEL_VALIDATION_PASS`.
- Full Windows matrix plus package, route-QA packet, issue-triage packet, and candidate-readiness evidence.

Next-step directive: continue immediately with Unity-only implementation and validation of the promoted playable asset.
