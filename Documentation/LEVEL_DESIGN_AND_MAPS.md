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
- Level03 foundry service lift is pressure-locked until the Boilerheart pressure valve is vented, then loads `Level04`.
- Level04 emergency hoist currently triggers the win state.
- Auto-playthrough covers Level01 key/gate/lift, transition to Level02, transition to Level03, locked-foundry-lift rejection, Boilerheart pressure valve, transition to Level04, and Level04 emergency hoist.
- Hazard smoke covers Level03 steam damage and Level04 furnace-heat damage without ending the run from one tick/pulse.
- Each current level now has a scene-specific objective briefing at spawn.
- Venting the Boilerheart pressure valve shuts down the linked Level03 steam hazards.
- Level01 includes the first secret pressure cache reward space.
- Run secret stats persist across the current multi-level route and can display at win.
- Auto-playthrough validates the run secret total survives to final win.
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

- Add the first boilerheart/furnace pressure chamber and gate the descent into the foundry.

Approximate footprint:

- Current prototype: about `13 x 28` meters.
- Production target: `65 x 55` meters.
- Current rooms: arrival floor, furnace-core chamber, baffle lane, foundry service lift.

New mechanics:

- Current prototype: Boilerheart pressure-valve objective, locked foundry lift, and linked hazard shutdown.
- Planned: expanded valve/gauge lock sequence.
- Planned: `Bellows Node` support enemy.
- Current prototype: steam hazard zones with vent/puff visuals.

Current top-down sketch:

```text
          N
  +----------------------+
  |    FOUNDRY LIFT      |
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
- Build order was MainMenu, Level01, Level02, Level03 at introduction.
- Level02 lift targets Level03.
- Level03 final lift originally triggered the win state before Level04 existed.
- Auto-playthrough validated the three-level chain at that point.

v0.0.39 implementation notes:

- Added `Boilerheart Pressure Valve Objective`.
- Final service lift remained pressure-locked until the valve was vented.
- Auto-playthrough validates that the final lift does not win early, vents the valve, then completes the run.

v0.0.40 implementation notes:

- Added reusable `SteamHazard`.
- Placed `Boilerheart Steam Hazard - Furnace Leak` and `Boilerheart Steam Hazard - Core Bleed`.
- Added packaged hazard smoke test and matrix coverage.

v0.0.42 implementation notes:

- Linked the Boilerheart pressure valve to the two current steam hazards.
- Auto-playthrough validates lift unlock and hazard shutdown after venting.

v0.0.43 implementation notes:

- Added reusable `SecretArea`.
- Added `Secret - Intake Pressure Cache` with health and ammo rewards.
- Added packaged secret smoke test and matrix coverage.

v0.0.44 implementation notes:

- Added persistent `RunStats` secret totals and discoveries.
- Win message can include `SECRETS discovered/total`.

v0.0.45 implementation notes:

- Auto-playthrough validates secret totals persist to the final win state.

v0.0.46 implementation notes:

- Level03 final win lift was converted into `Boilerheart Service Lift To Foundry`.
- The foundry lift remains pressure-locked until the Boilerheart pressure valve is vented.
- After venting, the foundry lift transitions to `Level04`.
- Auto-playthrough validates the locked lift, valve venting, hazard shutdown, and transition to the foundry.

### Level 04: Furnace Foundry

Purpose:

- Escalate mechanical enemy identity and industrial hazards.

Approximate footprint:

- Current prototype: about `14 x 32` meters.
- Production target: `80 x 60` meters.
- Current rooms: arrival floor, furnace baffle lane, mixed Scrapper/Lancer foundry floor, emergency hoist.
- Production target: assembly floor, furnace lanes, overhead gantry visuals, shutdown room.

New mechanics:

- Current prototype: foundry steam hazards, pulsing furnace heat-surge lanes, mixed melee/ranged pressure, and emergency-hoist win state.
- Planned: crusher or furnace hazard lanes.
- Planned: first `Bulwark` heavy enemy.
- Optional weapon route.

Current top-down sketch:

```text
          N
  +------------------------+
  |    EMERGENCY HOIST     |
  |  pipe bundle / green   |
  |       low barrier      |
  |   scrapper pressure    |
  |          |             |
  |  furnace lane + steam  |
  |    lancer sightline    |
  |          |             |
  |  health      ammo      |
  |          |             |
  |       ARRIVAL          |
  +------------------------+
          S
```

v0.0.46 implementation notes:

- Generated at `Assets/_Project/Scenes/Level04.unity`.
- Build order is MainMenu, Level01, Level02, Level03, Level04.
- Level03 foundry lift targets Level04 after Boilerheart valve completion.
- Added `Foundry Steam Hazard - Casting Leak` and `Foundry Steam Hazard - Crucible Bleed`.
- Added `Foundry Emergency Hoist` as the current campaign win device.
- Auto-playthrough validates the four-level route.

v0.0.47 implementation notes:

- Added reusable `FurnaceHeatHazard`.
- Added `Foundry Furnace Heat Hazard - Pour Lane` and `Foundry Furnace Heat Hazard - Hoist Lane`.
- Furnace heat hazards cycle through warning, active glow, and safe damper visuals.
- Hazard smoke now validates both Boilerheart steam damage and Foundry furnace-heat damage.

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
