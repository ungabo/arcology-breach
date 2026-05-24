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
- Level02 service lift now loads `Level03` through `LevelTransitionTrigger`, but starts locked until the Pipeworks routing valve is turned.
- Level03 foundry service lift is pressure-locked until the Boilerheart pressure valve is vented, then loads `Level04`.
- Level04 emergency hoist now loads `Level05`.
- Level05 master override hoist is locked until the Governor Warden is destroyed, then triggers the win state.
- Auto-playthrough covers Level01 key/gate/lift, transition to Level02, transition to Level03, locked-foundry-lift rejection, Boilerheart pressure valve, transition to Level04, transition to Level05, locked-master-hoist rejection, Warden defeat, unlock, and the Level05 master override hoist.
- Auto-playthrough now also covers locked Level02 Boilerheart-lift rejection, Pipeworks routing-valve completion, and the resulting objective update.
- Health and ammo pickup VFX now appear during the Level01 route and are verified by auto-playthrough.
- Gear-key pickup now spawns a visible brass/amber pickup burst and is verified by auto-playthrough.
- Pressure-gate opening now spawns a visible green pressure/steam/spark burst and is verified by auto-playthrough.
- Service-lift transitions now pause briefly for a visible green pressure/steam activation cue before loading the next level.
- Persistent objective HUD now tracks the active route beat and is verified through the auto-playthrough.
- Warden combat now surfaces a top-center boss health HUD so the final guardian objective reads clearly during the Governor Core fight.
- Warden defeat now gets a visible shutdown burst so the final lock release has readable cause and effect.
- Animated machinery spinners now add motion to pressure gates, service lifts, valve wheels, and the menu steamworks gear.
- Procedural machine motion now gives Scrappers, Lancers, Bulwarks, and the Warden moving body/limb/pressure-part silhouettes.
- Lancer pressure bolts now carry visible glow/trail/spark VFX, with shared VFX also attached to Warden pressure bolts.
- Pressure-pistol hits now leave short-lived scorch/brass impact decal VFX, verified by combat-scenario smoke.
- The Pressure Pistol now includes a short-range right-mouse Pressure Burst alternate fire, giving close encounters a distinct ammo-cost tradeoff while keeping the core weapon VR-compatible.
- Level03 now includes a Steam Scattergun pickup prototype with real acquisition smoke coverage, and the unlock persists through later service-lift transitions.
- The Steam Scattergun now swaps to a distinct first-person viewmodel when equipped, preserving readable weapon identity for later VR hand mapping.
- Steam Scattergun fire now has a dedicated pressure-ring, steam-core, and brass-spark blast VFX so close-range weapon readability is not dependent on shared pistol feedback.
- Steam Scattergun fire now uses a dedicated low-pressure procedural audio cue so weapon identity is readable by sound as well as viewmodel/VFX.
- Steam Scattergun alternate fire now uses dedicated slug audio/VFX so the right-mouse precision shot reads differently from the close-range pellet blast.
- Weapon pickups now spawn a dedicated brass/steam acquisition burst and acquisition audio cue; the Steam Scattergun pickup verifies this feedback before combat smoke continues.
- Player damage now triggers first-person pressure/heat/brass hurt VFX, verified by combat-edge smoke.
- Each current gameplay level now includes an interactable archive plaque for short environmental lore, verified by level validation and interaction smoke.
- Each current gameplay level now has a procedural brassworks ambience loop through `SteamworksAudio`, verified by runtime smoke.
- Steam hazards now use animated low/high puffs and are verified by level validation and hazard smoke.
- Furnace heat hazards now pulse their phase signals and show active heat ripples, verified by level validation and hazard smoke.
- Hazard smoke covers Level03 steam damage and Level04 furnace-heat damage without ending the run from one tick/pulse. Level05 also includes validated steam and furnace-heat hazards.
- Each current level now has a scene-specific objective briefing at spawn.
- Venting the Boilerheart pressure valve shuts down the linked Level03 steam hazards.
- Level03 now includes a Bellows Node support-machine prototype near the Boilerheart core; its pressure pulse has dedicated audio, damages the player, briefly boosts nearby Scrappers with visible overdrive VFX, is verified by packaged Bellows Node smoke, and is disabled during deterministic objective/hazard automation.
- Level01 includes the first secret pressure cache reward space.
- Level02 includes a Pipeworks cartridge-cache secret reward space.
- Level04 includes a second foundry coal-cache secret reward space.
- Run secret stats persist across the current multi-level route and can display at win.
- Auto-playthrough validates that multi-level secret totals survive to final win.
- Auto-playthrough now expects at least three registered secrets across the current route.
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

- Current prototype: pressure-routing valve objective that unlocks the Boilerheart lift, inter-level transition to `Level03`, and first ranged Lancer enemy.
- Planned: expanded ranged `Lancer` encounter routes.
- Current prototype: optional cartridge-cache secret.

v0.0.70 implementation notes:

- Added `Pipeworks Routing Valve Objective` near the north valve run.
- `Pipeworks Service Lift To Boilerheart` now starts pressure-locked until the routing valve is complete.
- The Level02 objective text now starts with routing pipe pressure before riding the Boilerheart lift.
- Auto-playthrough verifies the locked lift, valve completion, objective update, and transition to Level03.

v0.0.71 implementation notes:

- Added `Secret - Pipeworks Cartridge Cache` near the south-west pipe run.
- Added secret health and pressure-cartridge rewards plus pipe-rack/cache visual props.
- Level validation now requires the Level02 secret cache and pressure-cartridge reward.
- Auto-playthrough verifies at least three registered secrets persist to the final win state.

### Level 03: Boilerheart Core

Purpose:

- Add the first boilerheart/furnace pressure chamber and gate the descent into the foundry.

Approximate footprint:

- Current prototype: about `13 x 28` meters.
- Production target: `65 x 55` meters.
- Current rooms: arrival floor, furnace-core chamber, baffle lane, foundry service lift.

New mechanics:

- Current prototype: Boilerheart pressure-valve objective, locked foundry lift, and linked hazard shutdown.
- Current prototype: first Steam Scattergun pickup with dedicated acquisition VFX/audio and real pickup-route automation, introducing close-range weapon switching before the foundry escalation.
- Current prototype: `Bellows Node` stationary support machine with pressure-pulse damage, dedicated pulse audio, short nearby Scrapper boost, pulse VFX, and boost-state VFX.
- Planned: expanded valve/gauge lock sequence.
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

- Current prototype: foundry steam hazards, pulsing furnace heat-surge lanes, mixed melee/ranged/heavy pressure, and emergency-hoist transition to `Level05`.
- Planned: crusher or furnace hazard lanes.
- Current prototype: first `Bulwark` heavy enemy.
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
- Build order at introduction was MainMenu, Level01, Level02, Level03, Level04.
- Level03 foundry lift targets Level04 after Boilerheart valve completion.
- Added `Foundry Steam Hazard - Casting Leak` and `Foundry Steam Hazard - Crucible Bleed`.
- Added `Foundry Emergency Hoist` as the current campaign win device.
- Auto-playthrough validated the four-level route at introduction.

v0.0.47 implementation notes:

- Added reusable `FurnaceHeatHazard`.
- Added `Foundry Furnace Heat Hazard - Pour Lane` and `Foundry Furnace Heat Hazard - Hoist Lane`.
- Furnace heat hazards cycle through warning, active glow, safe damper visuals, and active heat-ripple VFX.
- Hazard smoke now validates Boilerheart steam damage, animated steam puffs, Foundry furnace-heat damage, and active furnace heat ripples.

v0.0.48 implementation notes:

- Added the first `Bulwark` heavy enemy role.
- Placed `Enemy - Foundry Hammer Bulwark` near the emergency-hoist lane.
- Added a primitive Bulwark silhouette with boiler body, furnace belly, pressure tank, piston legs, and hammer arms.
- Added packaged Bulwark combat smoke to verify heavy durability and death.

v0.0.49 implementation notes:

- Added `Secret - Foundry Coal Cache`.
- Added foundry secret health/ammo rewards and coal-bin visual props.
- Level validation now requires a Level04 secret and foundry cache visuals.
- Auto-playthrough requires at least two registered secrets at the final win state.

v0.0.50 implementation notes:

- Converted `Foundry Emergency Hoist` into a `LevelTransitionTrigger` targeting `Level05`.
- Auto-playthrough now treats Level04 as a transition level, not the final win level.

### Level 05: Governor Core

Purpose:

- Final prototype climax with the strongest pressure-machine identity.

Approximate footprint:

- Current prototype: about `15 x 32` meters.
- Production target: `75 x 75` meters.
- Current rooms: arrival floor, regulator ring, mixed enemy pressure lane, master override hoist.
- Production target: core access ring, gear chambers, emergency bypass, final guardian room.

New mechanics:

- Current prototype: final master override hoist is Warden-locked before win state.
- Current prototype: mixed Scrapper/Lancer/Bulwark pressure plus the first Governor Warden final guardian.
- Current prototype: steam hazard and pulsing furnace-heat surge inside the regulator lane.
- Planned: multi-stage objective unlock.
- Planned: boss or mini-boss encounter if scope allows.

Current top-down sketch:

```text
          N
  +-------------------------+
  |  MASTER OVERRIDE HOIST  |
  |  green signal / pipes   |
  |          |              |
  |    Bulwark pressure     |
  |          |              |
  |  [REGULATOR CORE RING]  |
  |  heat surge / steam     |
  |    lancer sightline     |
  |          |              |
  |  health      ammo       |
  |          |              |
  |       ARRIVAL           |
  +-------------------------+
          S
```

v0.0.50 implementation notes:

- Generated at `Assets/_Project/Scenes/Level05.unity`.
- Build order is MainMenu, Level01, Level02, Level03, Level04, Level05.
- Level04 emergency hoist targets Level05.
- Added `Governor Core Steam Hazard - Regulator Leak`.
- Added `Governor Core Furnace Heat Hazard - Regulator Surge`.
- Added `Enemy - Governor Core Bulwark` plus Scrapper/Lancer support.
- Added `Governor Core Master Override Hoist` as the current final win device.
- Auto-playthrough validates the five-level route through the Governor Core win state.

v0.0.51 implementation notes:

- Added `Enemy - Governor Core Warden` as the first final guardian prototype.
- Added Warden stomp, pressure-bolt, and enraged half-health behavior.
- Added Warden primitive silhouette pieces: core body, furnace heart, pressure crown, back boiler, piston arms, stomp plates, and pressure cannon muzzle.
- Added `RuntimeWardenCombatTest` to validate Warden durability/death in the packaged matrix.

v0.0.52 implementation notes:

- Added `GuardianDefeatObjective`.
- Linked `Governor Core Master Override Hoist` to the Warden defeat objective.
- Added `Governor Warden Lock Red Signal` and `Governor Warden Lock Green Signal`.
- Auto-playthrough validates that the hoist stays locked before Warden defeat and unlocks afterward.

v0.0.53 implementation notes:

- Added top-center Warden boss health HUD with a brass backplate, red pressure fill, and boss label.
- `GovernorWardenController` shows, updates, and hides the boss gauge as the Warden takes damage and dies.
- Level validation and runtime smoke require the boss HUD fields to be wired.
- `RuntimeWardenCombatTest` verifies the boss bar appears and drops after damage.

v0.0.54 implementation notes:

- Added `WardenShutdownVfx`.
- Governor Warden death now spawns steam jets, brass sparks, and an expanding pressure ring.
- Warden combat smoke verifies the shutdown effect exists and has visible primitive pieces.

v0.0.55 implementation notes:

- Added a persistent brass objective HUD below the top-left screen edge.
- Objective text updates after gear key pickup, pressure gate opening, Boilerheart valve venting, Warden defeat, death, and win.
- Runtime smoke validates active objective HUD wiring.
- Auto-playthrough validates objective text changes across the current route.

v0.0.56 implementation notes:

- Added `MachineDeathVfx` for regular mechanical enemy shutdowns.
- Scrappers and Lancers spawn a compact steam/spark burst on death.
- Bulwarks spawn a scaled-up machine shutdown burst.
- Combat and Bulwark combat smoke verify the death VFX exists with visible primitive pieces.

v0.0.57 implementation notes:

- Added `SteamworksSpinner` for simple looping local-axis machinery motion.
- Attached spinner motion to pressure-gate gears, service-lift pulley gears, environment valve wheels, the Boilerheart pressure valve wheel, and the main-menu gear.
- Level validation and runtime smoke now require configured spinner components so the machinery-motion pass stays present in generated scenes.

v0.0.69 implementation notes:

- Added `Lore Plaque - Intake Archive`, `Lore Plaque - Pipeworks Archive`, `Lore Plaque - Boilerheart Archive`, `Lore Plaque - Foundry Archive`, and `Lore Plaque - Governor Archive`.
- Plaques are optional route flavor, not mandatory progression stops.
- Interaction smoke verifies at least one plaque can be read and shows archive text on the HUD.

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
