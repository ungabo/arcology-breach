# Brassworks Breach - v0.1.35 Batch Validation Gates

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_35_BatchValidation/`

## Purpose

Define acceptance gates for the `v0.1.35` gameplay-systems batch: player juice, enemy feedback, pickup readability, objective/route affordances, secret feedback, pause/settings polish, and audio/VFX hooks.

This validation packet is documentation-only. The Unity main lane owns implementation, generated scene changes, validators, build evidence, package output, release docs, shared status docs, and git state.

## Baseline To Preserve

The current V0 route authority is the source of truth. `v0.1.35` feedback work must preserve:

- Level01 key, pressure gate, service lift, first combat, first pickups, and first secret expectations.
- Level02 routing valve, Boilerheart lift rejection/restoration, first Lancer read, pickup cache, and secret expectations.
- Level03 scattergun pickup, steam hazard timing, Bellows Node chamber, pressure valve, and Foundry lift.
- Level04 furnace hazards, Bulwark arena, coal-cache secret, emergency hoist, and mixed combat readability.
- Level05 Warden reveal, boss HUD, guardian lock, Warden defeat, final-hoist unlock, and final exit.

No acceptance gate in this packet allows new route authority. All feedback must attach to existing authority owners or subscribe to their state.

## Batch Breadth Gate

Preferred pass target:

- Both current weapons receive improved fire/impact/switch or pickup feedback.
- All five current enemy classes receive hit/death/shutdown feedback review: Scrapper, Lancer, Bellows Node, Bulwark, and Governor Warden.
- Health, ammo, key, and weapon pickup feedback are improved.
- Objective affordances cover all five levels.
- Secret feedback is covered for existing secrets.
- Pause, display, readability, and audio mix flows are exercised.
- Audio/VFX hooks are named and have safe placeholder fallbacks.

Minimum pass target:

- Both weapons touched.
- At least three enemy classes touched, including Bulwark or Warden.
- Health, ammo, and key pickup feedback touched.
- At least three levels receive objective/route feedback, including Level05.
- Secret, pause/settings, audio, and readability smokes pass unchanged or with explicit evidence.

Fail:

- Candidate only touches one system.
- Candidate adds final assets without gameplay hook validation.
- Candidate improves presentation in screenshots but weakens first-person route or combat clarity.

## No-Authority Contract

Reject any feedback-only object, effect, or helper that introduces:

- Collider or trigger collider.
- `NavMeshObstacle` or navigation-area changes.
- Rigidbody/physics movement that can block, push, snag, or alter projectiles/player movement.
- Pickup, inventory, weapon-grant, prompt, interaction, save, objective, route-state, lock, transition, scene-loading, boss-lock, win-state, or damage authority.
- New enemy spawn, enemy movement, attack timing, hitbox, hurtbox, or weak-point rule.
- New secret discovery volume or route branch.
- Required art/audio dependency for gameplay correctness.

Allowed:

- Visual/audio children on existing authoritative objects.
- Presentation subscribers that respond to existing events/states.
- Placeholder VFX/audio/UI indicators with no collision, no triggers, and safe missing-reference fallback.
- Tuning of presentation intensity where gameplay route state is unchanged.

## Lane Acceptance Gates

### Weapon Feedback And Impact

- Pressure Pistol fire reads as precise, fast, and compact.
- Steam Scattergun fire reads as heavier, wider, and more forceful.
- Fire, hit, miss/world impact, empty, pickup, and weapon switch feedback are distinguishable where supported.
- Recoil/camera/flash intensity does not hide crosshair, pickup prompts, objective text, boss HUD, enemy tells, or hazard warnings.
- Feedback does not change ammo economy, damage, fire rate, spread, range, projectile behavior, or weapon unlock authority unless separately owned by main-lane gameplay work and covered by smokes.

### Enemy Hit/Death Feedback

- Hit confirmation is visible at normal combat distance.
- Death/shutdown confirmation is distinct from stagger, attack windup, or temporary hit flash.
- Scrapper melee tells remain readable through hit/death feedback.
- Lancer charge/projectile tells remain readable against machinery and pipe backgrounds.
- Bellows Node pulse/boost/shutdown states remain readable and do not hide nearby enemies.
- Bulwark windup, footprint, shield mass, and shutdown state remain readable.
- Warden reveal, boss HUD, phase/pressure tells, defeat, guardian-lock clear, and final-hoist unlock remain readable.
- Feedback does not add or imply a new weak-point rule.

### Pickup And Readability Cues

- Health, ammo, key, and weapon pickup cues are readable before collection and unmistakable after collection.
- Collection feedback differentiates resource, route-authority, and capability acquisition.
- Pickup burst/audio/UI confirmation does not hide the pickup prompt, pickup root, nearby hazard warning, or enemy tell.
- Pickup-adjacent visual work does not move, duplicate, or obscure the existing pickup trigger/authority.
- Pickup cues remain readable with display/readability settings enabled.

### Objective And Route Affordances

- Locked rejection feedback explains the block without implying a new objective.
- Key, valve, lift, hoist, boss-lock, and final-exit state changes are visible/audible from the intended route direction.
- Amber/red/green route language remains stable: amber means attention/objective, red means locked/hostile/unsafe, green means restored/safe/exit.
- Objective feedback attaches to existing authoritative objects.
- Objective affordances do not narrow route apertures or hide destination cues.

### Secret Feedback

- Secret discovery feedback is distinct from regular pickup feedback.
- Secret clue feedback remains optional and lower-priority than main-route objective language.
- Existing secret definitions remain the only discovery authority.
- Secret feedback does not add new colliders, triggers, routes, pickups, or save state.
- Secret cues do not pull the player into active lethal spaces or away from required combat reads.

### Pause And Settings Polish

- Pause can open and close cleanly during safe gameplay, combat-adjacent gameplay, and after pickup/objective feedback.
- Resume restores input, cursor, time scale, HUD state, audio state, and camera control.
- Display settings changes are visible and reversible.
- Readability settings preserve pickup, enemy, objective, hazard, secret, and UI clarity.
- Audio mix changes do not mask weapon feedback, enemy tells, pickup confirmation, objective feedback, low-health warnings, or pause confirmation.

### Audio/VFX Hook Safety

- Hooks are named consistently by event role.
- Missing final assets produce no crash, route blocker, softlock, or required-state failure.
- Placeholder effects are clearly presentation-only.
- VFX/audio intensity is validated in the real five-level flow, not only in isolated test scenes.

## Route-Safety Checks

### Global

- Route audit still reports `V0_ROUTE_AUDIT_PASS`.
- No added feedback object sits inside lift, hoist, gate, final exit, level transition, pickup trigger, hazard damage, enemy spawn, boss lock, or prompt ownership volumes unless it is a non-authoritative child of the existing owner.
- No feedback hides a required prompt, pickup, valve, gate, lift, hoist, Warden lock, boss HUD, final exit, or hazard warning.
- No VFX creates fake damage language or fake safe-lane language.
- No secret/objective/pickup cue creates a competing route destination.

### Level01

- First key acquisition, pressure-gate rejection/unlock, and service-lift boarding remain readable.
- First pickups teach collection feedback without hiding the first route.
- First secret feedback reads as optional.

### Level02

- Boilerheart lift rejection/restoration and routing valve feedback remain readable.
- Lancer tells are not hidden by impact, objective, or pickup effects.
- Secret feedback does not overpower the valve route.

### Level03

- Scattergun pickup feedback confirms capability acquisition without blocking the prompt/root.
- Steam hazard warnings remain readable through weapon and objective effects.
- Bellows Node shutdown/pulse feedback remains distinct from valve/lift route feedback.

### Level04

- Furnace hazard language remains stronger than decorative or secret feedback.
- Bulwark hit/death feedback does not hide windup or safe movement space.
- Emergency hoist feedback remains clear after combat.

### Level05

- Warden reveal, boss HUD, hit/death feedback, guardian lock, and final-hoist unlock form one understandable chain.
- VFX/audio does not trivialize or obscure Warden attack tells.
- Final exit feedback reads green/restored only after the correct Warden defeat state.

## Pass/Fail Rules

### Required Pass

A `v0.1.35` candidate may proceed to full matrix only when all are true:

- Batch breadth meets preferred or minimum target.
- Editor compile, level validation, and editor smoke pass.
- Route audit reports `V0_ROUTE_AUDIT_PASS`.
- Targeted player smokes pass for weapons, combat, pickups/interactions, secrets, pause, flow, audio, display, and readability.
- Human first-person review finds no P0/P1 route, combat, hazard, pickup, objective, secret, boss, or settings clarity issue.
- Any accepted P1 issue has explicit fix/defer ownership before release readiness.

### Automatic Fail

Fail immediately if any are true:

- Crash, hard hang, no quit path, or broken pause escape.
- P0 route blocker, transition blocker, lock dead-end, broken pickup, broken weapon switch, broken Warden unlock, or broken final exit.
- Any feedback-only hierarchy owns unauthorized collider, trigger, nav, damage, pickup, prompt, transition, objective, route-state, secret, save, or boss-lock authority.
- Enemy tell, hazard tell, pickup prompt, objective prompt, boss HUD, or final exit becomes materially less readable.
- Route audit contradicts manual evidence or reports a blocker.
- Missing final VFX/audio asset breaks gameplay behavior or validation.

### Hold / Tuning

Hold for tuning if any are true:

- Preferred breadth misses but minimum breadth passes.
- Effects are technically functional but visually noisy in first-person flow.
- Weapon juice feels good in isolation but obscures combat reads.
- Secret feedback is too strong relative to objective feedback.
- Settings changes pass automation but feel ambiguous in manual review.
- Audio feedback masks enemy, hazard, pickup, objective, or pause confirmation.

## Release-Readiness Rule

After targeted validation passes, the final release-candidate gate remains the existing full V0 matrix. `v0.1.35` should not be called release-ready until the main lane runs the full matrix with the chosen log prefix and regenerates candidate evidence.

