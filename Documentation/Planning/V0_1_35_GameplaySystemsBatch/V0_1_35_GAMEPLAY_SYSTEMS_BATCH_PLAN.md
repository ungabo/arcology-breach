# Brassworks Breach - v0.1.35 Gameplay Systems Batch Plan

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_GameplaySystemsBatch/`

## Goal

Ship a large, playable `v0.1.35` gameplay-feel batch that makes the existing route and combat loop read as a more finished game while preserving the route authority proven by the current V0 matrix.

This milestone is not an asset drop. It is a systems/feedback bundle that can use placeholder effects now and accept final art/audio assets later.

## Batch Identity

Suggested batch ID:

`v0.1.35_gameplay_systems_feedback_validation_batch`

Every implemented lane should be traceable in notes, logs, or object names to one of these categories:

- `weapon_feedback`
- `enemy_feedback`
- `pickup_readability`
- `objective_route_affordance`
- `secret_feedback`
- `pause_settings_polish`
- `audio_vfx_hook`

## Playable Leap

The batch should be accepted only if it improves the moment-to-moment loop across several systems at once. A candidate that only adds one weapon effect, one enemy flash, or one settings tweak is too small for `v0.1.35`.

Preferred milestone breadth:

- Weapon feedback/impact cues improve both Pressure Pistol and Steam Scattergun.
- Enemy hit/death feedback covers Scrapper, Lancer, Bellows Node, Bulwark, and Governor Warden.
- Pickup/readability cues cover health, ammo, key, weapon pickup, and one route-critical interaction.
- Objective/route affordances cover Level01 gate, Level02 valve/lift, Level03 valve/lift, Level04 emergency hoist, and Level05 Warden/final hoist.
- Secret feedback covers all currently validated secret expectations without adding new secret triggers.
- Pause/settings polish covers pause/resume, display settings, readability settings, and audio mix continuity.
- Audio/VFX hooks provide stable integration points for final assets but have a placeholder fallback.

Minimum acceptable milestone breadth:

- Both weapons receive feedback improvement.
- At least three enemy classes receive hit/death feedback, including either Bulwark or Warden.
- Health, ammo, and key pickup cues are improved.
- Objective affordances touch at least three levels, including Level05.
- Secret, pause, audio, and readability smokes remain covered by targeted validation.

Automatic scope fail:

- The candidate can be described as a single-system patch.
- Any lane adds new route colliders, triggers, nav blockers, pickup authority, objective authority, damage zones, save behavior, or scene transition authority.
- A temporary VFX/audio hook becomes required for gameplay correctness.

## System Slices

### 1. Weapon Feedback And Impact Cues

Main-lane implementation target:

- Add or tune muzzle flash, recoil pulse, pressure vent, impact puff/spark, dry-fire/empty read, pickup confirmation, and weapon-switch confirmation using existing weapon controllers.
- Keep Pressure Pistol precise and compact; keep Steam Scattergun heavier, broader, and louder.
- Ensure feedback does not hide crosshair, prompts, boss HUD, or objective text.

Acceptance target:

- Firing each weapon gives immediate visual/audio confirmation.
- Hits on enemies read differently from misses/world impacts if current code supports that distinction.
- Empty or invalid fire states read without feeling like input loss.
- Weapon pickup/switch feedback does not obscure route-critical prompts.

### 2. Enemy Hit And Death Feedback

Main-lane implementation target:

- Add hit flash, small knock/read pulse, damage confirmation, death shutdown, and boss defeat feedback around existing enemy health/death paths.
- Prefer per-enemy feedback profiles over new bespoke logic per scene.
- Bellows Node support feedback should clarify pulse/boost/shutdown state.
- Warden feedback must clarify reveal, phase/half-health if present, shutdown, and final-hoist unlock.

Acceptance target:

- Player can tell when damage landed.
- Player can tell when an enemy is dead versus still recovering or attacking.
- Enemy tells remain visible through feedback effects.
- Feedback never implies a new weak-point rule unless gameplay already owns that rule.

### 3. Pickups And Readability Cues

Main-lane implementation target:

- Add collection burst, short audio cue, optional HUD/toast confirmation, and pickup idle read for health, ammo, keys, and weapon pickup.
- Use consistent color/state language: health is recovery, ammo is resource, key is route authority, weapon is capability.
- Keep prop/readability work around existing pickup roots and triggers.

Acceptance target:

- Pickups read as interactive from normal approach distance.
- Collection is unmistakable under combat pressure.
- Pickups do not look like noninteractive set dressing.
- Pickup cues do not hide or move the actual pickup root.

### 4. Objective And Route Affordances

Main-lane implementation target:

- Improve feedback when a route state changes: locked rejection, key acquired, valve completed, lift restored, hoist unlocked, boss gate cleared.
- Add subtle state cues on existing authoritative objects instead of adding new route objects.
- Keep route color language stable: amber for attention/objective, red for locked/hostile/unsafe, green for restored/safe/exit.

Acceptance target:

- The player can see what changed after a key, valve, or Warden objective event.
- Rejection feedback tells the player why a route is blocked without implying a second objective.
- The final Level05 unlock reads clearly after Warden defeat.

### 5. Secret Feedback

Main-lane implementation target:

- Add discovery confirmation, optional subtle pre-discovery clue feedback, and post-discovery state change around existing secret definitions.
- Secret cues should reward discovery without pulling first-run players off the critical route during combat pressure.

Acceptance target:

- Secret discovery is audible/visible and distinguishable from pickup collection.
- Secret clues remain optional and lower-priority than main route signage.
- No new secret trigger or collider is added outside existing secret authority.

### 6. Pause And Settings Polish

Main-lane implementation target:

- Harden pause/resume from combat, objective, pickup, and transition-adjacent states.
- Keep display/readability settings feedback immediate and reversible.
- Ensure audio pause/resume and mix changes are understandable.

Acceptance target:

- Pause opens, closes, and returns control cleanly.
- Settings changes do not break input capture, time scale, cursor state, audio mix, or HUD visibility.
- Readability settings preserve route, pickup, enemy, hazard, and objective clarity.

### 7. Audio/VFX Hooks

Main-lane implementation target:

- Define stable event names or binding points for fire, impact, pickup, enemy hit, enemy death, objective complete, secret found, pause open, pause close, and settings changed.
- Use placeholder effects if final assets are unavailable.
- Keep hooks non-authoritative: missing final art/audio should degrade presentation only.

Acceptance target:

- Final art/audio teams can swap clips/effects without changing gameplay logic.
- Placeholder hooks are named and grouped consistently.
- Silence/missing asset fallback does not break validation.

## Suggested Implementation Order

1. Establish shared feedback hook naming and placeholder effect policy.
2. Implement weapon feedback and pickup confirmation together so resource and weapon loops can be smoked early.
3. Implement enemy hit/death feedback profiles, starting with Scrapper/Lancer then Bellows Node/Bulwark/Warden.
4. Implement objective route affordances on existing gate/valve/lift/hoist/boss-lock objects.
5. Implement secret discovery feedback using existing secret definitions only.
6. Harden pause/display/readability/audio settings and run targeted settings smokes.
7. Run targeted validation, fix readability issues, then run the full matrix.

## Non-Goals

- No new levels, route branches, locks, keys, transitions, secrets, enemies, weapons, or hazards.
- No new colliders/triggers/nav obstacles for feedback-only objects.
- No gameplay balance changes unless already covered by existing smoke expectations.
- No generated scene, builder, validator, package, release, or shared-status edits from this planning lane.

