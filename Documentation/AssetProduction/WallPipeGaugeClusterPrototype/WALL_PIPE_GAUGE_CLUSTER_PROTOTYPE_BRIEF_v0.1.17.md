# Brassworks Breach - Wall Pipe Gauge Cluster Prototype Brief v0.1.17

Generated: `2026-05-24 06:13 -04:00`

## Purpose

Promote one reusable environment component that can become part of the final brassworks corridor kit: a wall-mounted pipe, gauge, and valve cluster. The component is intentionally modular, lightweight, and safe for generated gameplay levels while pointing toward the north-star steampunk corridor language.

## Promoted Component

Name: `WallPipeGaugeClusterPrototype`

Promoted placements:

- `Pipeworks Prototype Wall Pipe Gauge Cluster`
- `Boilerheart Prototype Wall Pipe Gauge Cluster`

## Visual Requirements

- Blackened iron mounting plate.
- Aged brass upper and lower rails.
- Five visible pipe elements with copper feed and blackened return language.
- Two cream enamel pressure gauges.
- One red/brass valve wheel.
- At least fourteen visible rivets.
- Material-role separation for iron, brass/copper, cream gauge face, and warning red.

## Technical Requirements

- Must use Unity-generated geometry only.
- Must not rely on Blender or external render tooling.
- Must keep route collision unobtrusive by sitting on wall surfaces.
- Must use `WallPipeGaugeClusterPrototype` metadata with `promotionVersion = v0.1.17`.
- Must pass editor validation for required parts, detail counts, placement roles, and material-name roles.

## Validation Targets

- Level02 placement role: `pipeworks_route_wall`.
- Level03 placement role: `boilerheart_route_wall`.
- `V0_LEVEL_VALIDATION_PASS`.
- Full Windows matrix plus package, route-QA packet, issue-triage packet, and candidate-readiness evidence.

Next-step directive: continue immediately with the next highest-impact unfinished task.
