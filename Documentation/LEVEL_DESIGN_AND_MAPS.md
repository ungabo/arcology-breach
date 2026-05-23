# Brassworks Breach - Level Design and Maps

Last updated: 2026-05-23

## Purpose

This document keeps level layout, scale, progression, and inter-level mechanics from being overlooked. `Brassworks Breach` should use compact, readable FPS maps with clear objective loops, secrets, and strong steampunk industrial landmarks.

## Scale Rules

Unity unit scale:

- `1 Unity unit = 1 meter`.
- Player height target: about `1.8` meters.
- Main corridor width: `3` to `5` meters.
- Tight pipe corridor width: `2` to `2.5` meters.
- Combat room minimum: about `10 x 10` meters.
- Small arena target: `14 x 18` to `20 x 24` meters.
- Door height: `2.4` to `3` meters.
- Door width: `2` to `4` meters depending on encounter flow.
- Stairs/ramps should remain VR-friendly later, with gentle slopes and no forced jumping.

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

- Level01 service lift now loads `Level02` through `LevelTransitionTrigger`.
- Level02 service lift now loads `Level03` through `LevelTransitionTrigger`.
- Level03 final service lift is pressure-locked until the Boilerheart pressure valve is vented, then triggers the current win state.
- Auto-playthrough covers Level01 key/gate/lift, transition to Level02, transition to Level03, locked-final-lift rejection, Boilerheart pressure valve, and Level03 final lift.
- Health and ammo persist across scene transitions.
- Future weapon inventory and campaign flags still need expanded persistence.

Production target:

- Every level ends at a diegetic transit device: service lift, pressure elevator, maintenance tram, breach pod, or furnace hoist.
- Player inventory should persist core weapons and health rules across levels.
- Temporary objective items such as gear keys may be level-scoped unless explicitly marked campaign-scoped.
- Transition should autosave later, but no save system is required for v0.2.
- VR ports should replace abrupt fades with comfort-safe fade-to-black and stable spawn orientation.

## Campaign Map Ladder

### Level 01: Brassworks Intake

Purpose:

- Teach movement, shooting, gear key, pressure gate, and service lift.

Approximate footprint:

- `55 x 40` meters total.
- Five to seven rooms.
- One locked route.
- One small loop.
- Two to four enemy groups.

Current rooms:

1. Soot-brick service entry.
2. Copper-pipe maintenance throat.
3. Clockwork repair bay.
4. Gear-key plinth.
5. Pressure gate.
6. Furnace control room.
7. Service lift.

v0.2 map tasks:

- Keep the current generated layout small.
- Tune distances so the player sees the gate before finding the key.
- Add cover and obstacle shapes that do not break enemy movement. First collision-cover pass added in `v0.0.28`.
- Make the service lift direction visually green and unambiguous.

### Level 02: Pipeworks Annex

Purpose:

- Introduce longer sightlines, pipeworks visual identity, first ranged pressure, and the second service-lift transition.

Approximate footprint:

- Current prototype: about `12 x 26` meters.
- Production target: `70 x 45` meters.
- Current rooms: narrow pipeworks entry, baffle corridor, small Scrapper/Lancer encounter lane, transition service lift.

New mechanics:

- Current prototype: inter-level transition to `Level03` and first ranged Lancer enemy.
- Planned: valve wheel pressure-routing objective.
- Planned: first ranged `Lancer` enemy.
- Planned: optional ammo cache secret.

### Level 03: Boilerheart Core

Purpose:

- Add the first boilerheart/furnace pressure chamber and extend the campaign chain to a three-level playable route.

Approximate footprint:

- Current prototype: about `13 x 28` meters.
- Production target: `65 x 55` meters.
- Current rooms: arrival floor, furnace-core chamber, baffle lane, final service lift.

New mechanics:

- Current prototype: three-level transition chain, Boilerheart pressure-valve objective, and locked final lift win state.
- Planned: expanded valve/gauge lock sequence.
- Planned: `Bellows Node` support enemy.
- Planned: steam hazard zones.

Current top-down sketch:

```text
          N
  +----------------------+
  |      FINAL LIFT      |
  |   pipes/signage      |
  |          |           |
  |  cover   |   cover   |
  |          |           |
  |    [FURNACE CORE]    |
  |   glow/steam/gauge   |
  |          |           |
  |  health      ammo    |
  |          |           |
  |      ARRIVAL         |
  +----------------------+
          S
```

v0.0.38 implementation notes:

- Generated at `Assets/_Project/Scenes/Level03.unity`.
- Build order is MainMenu, Level01, Level02, Level03.
- Level02 lift targets Level03.
- Level03 final lift triggers the current win state.
- Auto-playthrough validates the three-level chain.

v0.0.39 implementation notes:

- Added `Boilerheart Pressure Valve Objective`.
- Final service lift remains pressure-locked until the valve is vented.
- Auto-playthrough validates that the final lift does not win early, vents the valve, then completes the run.

### Level 04: Furnace Foundry

Purpose:

- Escalate mechanical enemy identity and industrial hazards.

Approximate footprint:

- `80 x 60` meters.
- Assembly floor, furnace lanes, overhead gantry visuals, shutdown room.

New mechanics:

- Crusher or furnace hazard lanes.
- First `Bulwark` heavy enemy.
- Optional weapon route.

### Level 05: Governor Core

Purpose:

- Final prototype climax with the strongest pressure-machine identity.

Approximate footprint:

- `75 x 75` meters.
- Core access ring, gear chambers, emergency bypass, final guardian room.

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

- `Brassworks Intake` stays playable at the intended scale.
- Gate, key, and lift have clear spatial relationships.
- Level01 transitions cleanly into the current `Pipeworks Annex` prototype.
- Follow-up tasks for expanded run-state persistence and campaign map expansion exist in `WORK_LEDGER.md`.
