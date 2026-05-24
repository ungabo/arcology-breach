# Level Map Expansion Plan v0.1.37

## Purpose

This plan maps the Steamworks Level Kit modules to future larger level layouts without requiring immediate scene edits. It gives sidecar and main-lane workers a shared scale target for bigger batches.

## Module-to-Level Roles

| Module | Primary use | Placement rule |
| --- | --- | --- |
| Corridor straight | Main route spans, approach tunnels, lift exits | Chain on `4m` grid, vary wall dressing every second segment. |
| Corridor corner | Direction changes and ambush bends | Use with `4m` recovery space before combat starts. |
| T-junction | Route decision, secret branch, enemy reveal | Keep center clear; mount gauges on rear pressure wall. |
| Boiler alcove | Heat/hazard landmark | Wall-only, never inside dodge footprint. |
| Gauge wall | Objective read and pressure-state feedback | Place at route end, valve room, or lift lobby. |
| Riveted vault door | Locked landmark, boss gate, secret vault | Use as closed visual blocker unless main lane owns door behavior. |
| Pressure lock door frame | Open transition shell | Preserve center aperture and trigger zones. |
| Pipe railing | Catwalk/perimeter edge | Place only where collision can be simple and non-snagging. |
| Catwalk floor | Bridge or raised arena edge | Use with route smoke before any drop/edge hazard. |
| Wall column | Repeating rhythm, cover seam | Fine-snap to walls; non-colliding by default. |
| Ceiling pipe cluster | Overhead identity | Keep above `2.65m` clearance. |
| Valve console | Objective prop or locked-state read | Maintain `1.5m` prompt radius. |
| Vent smoke anchor | Steam VFX socket | Aim away from enemy tells and pickup silhouettes. |

## Level Expansion Blocks

### Level01 Intake Gate

- Replace plain intro corridor runs with two straight modules and one pressure-lock frame.
- Use gauge wall as first pressure-state teaching read.
- Use smoke anchor high on wall, not at player eye level.

### Level02 Condensate Spine

- Build a `T-junction` route decision around the first side objective.
- Use pipe railings and ceiling pipe clusters to reinforce the spine identity.
- Keep bridge/catwalk spaces at least `3.2m` clear.

### Level03 Boilerheart

- Use boiler alcoves as hazard-side silhouettes.
- Use valve console as objective prop candidate.
- Place vault door behind reward or weapon display.

### Level04 Furnace Row

- Use boiler alcoves and red enamel pressure strips to sell heat danger.
- Keep all bulky assets outside heavy enemy charge lanes.
- Catwalk floor modules can frame perimeter routes if route smoke stays clean.

### Level05 Core Ring

- Use vault door, gauge wall, and ceiling pipe clusters as final pressure-network read.
- Avoid dense railings inside boss arena center.
- Use green route-state lamps and valve console after boss route unlock.

## Follow-Up Work

- Generate and render the package in Unity.
- Run first-person screenshots against the current v0 blockouts.
- Promote a small module set into quarantine import before broad replacement.
- Add collider profiles after route smoke, not during sidecar generation.
