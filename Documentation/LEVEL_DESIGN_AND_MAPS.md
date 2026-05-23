# Arcology Breach - Level Design and Maps

Last updated: 2026-05-22

## Purpose

This document keeps level layout, scale, progression, and inter-level mechanics from being overlooked. `Arcology Breach` should use compact, readable FPS maps with clear objective loops, secrets, and strong visual landmarks.

## Scale Rules

Unity unit scale:

- `1 Unity unit = 1 meter`.
- Player height target: about `1.8` meters.
- Main corridor width: `3` to `5` meters.
- Tight maintenance corridor width: `2` to `2.5` meters.
- Combat room minimum: about `10 x 10` meters.
- Small arena target: `14 x 18` to `20 x 24` meters.
- Door height: `2.4` to `3` meters.
- Door width: `2` to `4` meters depending on encounter flow.
- Stairs/ramps should remain VR-friendly later, with gentle slopes and avoid forced jumping.

Movement assumptions:

- No jump in early versions.
- No crouch requirement in early versions.
- Avoid narrow snag points.
- Avoid unavoidable fast camera turns that would be uncomfortable in VR later.

## Core Map Structure

Each main level should include:

1. Arrival space with immediate visual identity.
2. First safe read of the level threat.
3. First combat room.
4. Objective branch or key route.
5. Locked route that loops back into sight.
6. Optional secret or resource detour.
7. Escalation room with mixed enemy pressure.
8. Exit device or transit node.

## Level Transition Mechanics

Near-term:

- Green emergency lift/data gate ends the current level.
- Runtime shows win state for v0/v0.1/v0.2.
- Future transition can call a `LevelTransitionController` that loads the next scene by build index or scene reference.

Production target:

- Every level ends at a diegetic transit device: lift, maintenance tram, data conduit, service airlock, or breach pod.
- Player inventory should persist core weapons and health rules across levels.
- Temporary objective items such as access shards may be level-scoped unless explicitly marked campaign-scoped.
- Transition should autosave later, but no save system is required for v0.2.
- VR ports should replace abrupt fades with comfort-safe fade-to-black and stable spawn orientation.

## Campaign Map Ladder

### Level 01: Aster Gate Intake

Purpose:

- Teach movement, shooting, access shard, locked gate, and exit.

Approximate footprint:

- `55 x 40` meters total.
- Five to seven rooms.
- One locked route.
- One small loop.
- Two to four enemy groups.

Current rooms:

1. Flooded service intake.
2. Cable-lined maintenance throat.
3. Robot repair bay.
4. Access-shard kiosk.
5. Corporate lockdown gate.
6. Transit control node.
7. Emergency lift/data gate.

v0.2 map tasks:

- Keep the current generated layout small.
- Tune distances so the player sees the gate before finding the shard.
- Add cover and obstacle shapes that do not break enemy movement.
- Make exit direction visually green and unambiguous.

### Level 02: Transit Spine

Purpose:

- Introduce longer sightlines, moving machinery, and ranged enemies.

Approximate footprint:

- `70 x 45` meters.
- Tram platform, maintenance side route, control booth, service lift.

New mechanics:

- Transit power switch.
- First ranged `Lancer` enemy.
- Optional ammo cache secret.

### Level 03: Data Stack

Purpose:

- Introduce vertical-looking server spaces while keeping gameplay mostly ground-plane friendly.

Approximate footprint:

- `65 x 55` meters.
- Server aisles, cooling chamber, data lock hub, corrupted node room.

New mechanics:

- Data lock sequence.
- `Choir Node` support enemy.
- Environmental hazard zones.

### Level 04: Civic Machine Foundry

Purpose:

- Escalate mechanical enemy identity and industrial hazards.

Approximate footprint:

- `80 x 60` meters.
- Assembly floor, fabrication lanes, overhead gantry visuals, shutdown room.

New mechanics:

- Crusher or laser hazard lanes.
- First `Bulwark` heavy enemy.
- Optional weapon route.

### Level 05: Interdict Core

Purpose:

- Final prototype climax with the strongest cyberpunk machine-horror identity.

Approximate footprint:

- `75 x 75` meters.
- Core access ring, signal chambers, emergency bypass, final guardian room.

New mechanics:

- Multi-stage objective unlock.
- Mixed enemy groups.
- Boss or mini-boss encounter if scope allows.

## Map Documentation Template

Every future level should have:

- One-page map brief.
- Top-down blockout sketch or grid.
- Room list.
- Enemy placement table.
- Pickup placement table.
- Objective chain.
- Secret list.
- Performance notes.
- VR comfort notes.
- Android/browser simplification notes.

## Current Acceptance Criteria

For `v0.2`, this document is considered applied when:

- `Aster Gate Intake` stays playable at the intended scale.
- Gate, shard, and exit have clear spatial relationships.
- Follow-up tasks for level transition code and campaign map planning exist in `WORK_LEDGER.md`.
