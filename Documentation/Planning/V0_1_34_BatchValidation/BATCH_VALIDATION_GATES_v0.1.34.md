# Brassworks Breach - v0.1.34 Batch Validation Gates

Created: `2026-05-24`

Owned scope: `Documentation/Planning/V0_1_34_BatchValidation/`

## Purpose

Define the validator criteria, route-safety checks, and pass/fail rules for integrating several visible upgrades at once in `v0.1.34`.

The intended milestone combines:

- Weapon/prop upgrades: first-person weapon readability, world pickup/display clarity, ammo/health/resource prop clarity, and pickup feedback.
- Enemy readability upgrades: silhouette, attack tell, shutdown feedback, boss/elite readability, and combat lane clarity.
- Level-density polish: visible authored density at route decisions, combat landmarks, hazards, secrets, and exits without changing gameplay authority.

This packet is documentation-only. The Unity main lane owns actual validators, scene builder changes, generated scenes, build evidence, release docs, shared status docs, and git state.

## Baseline To Preserve

`v0.1.33` route audit reports no deterministic route blockers. `v0.1.34` validation must preserve the same five-level route spine while accepting added visual density.

| Scene | Core route baseline | Encounter baseline | Pickup/hazard/secret baseline | Route safety focus |
| --- | --- | --- | --- | --- |
| Level01 | Pressure gate and service lift present. | 4 Scrappers. | 2 health, 2 ammo, 1 key, 1 secret. | Spawn-to-key, key-to-gate, gate-to-lift readability. |
| Level02 | Routing valve and Boilerheart lift present. | 1 Scrapper, 1 Lancer. | 2 health, 2 ammo, 1 secret. | Locked lift rejection, valve prompt, restored return route, first Lancer read. |
| Level03 | Pressure valve and Foundry lift present. | 2 Scrappers, 1 Bellows Node. | 1 health, 1 ammo, 1 weapon pickup, 2 steam hazards. | Scattergun pickup, steam approach, Bellows Node chamber, valve-to-lift route. |
| Level04 | Emergency lift present. | 2 Scrappers, 1 Lancer, 1 Bulwark. | 2 health, 2 ammo, 2 steam hazards, 2 furnace hazards, 1 secret. | Furnace safe lanes, Bulwark footprint, secret clue, emergency hoist. |
| Level05 | Warden, guardian lock, and final exit present. | 1 Scrapper, 1 Lancer, 1 Bulwark, 1 Warden. | 1 health, 1 ammo, 1 steam hazard, 1 furnace hazard. | Warden reveal, boss HUD, final-hoist lock/unlock, Warden-to-exit spacing. |

## Validator Criteria

### Batch Identity

Every accepted `v0.1.34` visible upgrade should be traceable to the combined batch.

- Root names include a stable component/family name and placement or usage role.
- Metadata, naming, or validator-visible data identifies promotion version `v0.1.34`.
- Batch ID should be `v0.1.34_combined_readability_density_milestone` or an equivalent exact shared string chosen by the main lane.
- Each root is tagged or named into one lane: `weapon_prop`, `enemy_readability`, or `level_density`.
- The validator reports per-lane counts and per-level placement counts so the candidate cannot pass as a single-prop release.

### Batch Breadth

Preferred milestone target:

- At least 2 weapon/prop readability upgrades, including at least one weapon-facing item and one world-facing pickup/prop item.
- All five current machine readability classes covered by validator-visible checks: Scrapper, Lancer, Bellows Node, Bulwark, and Governor Warden.
- Level01 through Level05 each receive visible density/readability work near at least 2 route-critical beats.

Minimum acceptable target if route crowding forces cuts:

- At least 1 weapon/prop upgrade.
- At least 3 enemy readability classes, including either Bulwark or Warden.
- All five levels still receive at least 1 route-critical density/readability touch.

Fail condition:

- Fewer than 3 levels touched, no enemy readability upgrade, no weapon/prop upgrade, or a result that reads as a single isolated asset promotion.

### Shared No-Authority Contract

Visual/readability additions must not own gameplay unless the main lane explicitly registers that root as an existing gameplay object upgrade.

Reject any visual-only hierarchy containing:

- `Collider` or trigger collider.
- `NavMeshObstacle`.
- Route-state controller.
- Level transition trigger or scene loading behavior.
- Pickup, objective, prompt, interaction, damage, save, boss-lock, or win-state authority.
- Rigidbody/physics behavior that can move, fall, snag, or block the route.
- New dynamic lights in bulk without a validator-visible performance/readability reason.

Allowed exception:

- Existing gameplay-authoritative objects such as a real weapon pickup, enemy, hazard, lock, valve, or transition may receive visual children only if the original authority component remains on the expected owner and no duplicate authority is introduced.

### Weapon And Prop Criteria

Weapon/prop upgrades pass only when the readable upgrade is visible without weakening gameplay clarity.

- First-person weapon geometry must preserve muzzle direction, fire/alternate-fire feedback, reload/pressure readability if present, and HUD/objective visibility.
- World weapon pickups must keep silhouette, prompt range, pickup burst VFX, pickup audio, and acquisition route clear.
- Ammo and health props must remain distinguishable from noninteractive set dressing.
- Display stands, plaques, crates, rails, or prop frames must not imply extra interactions unless they already own an interaction.
- Pickup-adjacent density must not hide the exact pickup root, pickup trigger, or VFX spawn point.
- Material roles should avoid making weapon props blend into same-value wall density from normal approach angles.

### Enemy Readability Criteria

Enemy upgrades pass only when they make combat fairer to read in the shipped arenas.

- Scrapper melee tell remains visible before damage from Level01 and later mixed encounters.
- Lancer aim/fire tell remains visible against Level02/Level05 pipe and machinery backgrounds.
- Bellows Node pulse and boost state remain visible without hiding nearby Scrappers.
- Bulwark windup and shutdown feedback remain visible in Level04/Level05 heavy-combat spaces.
- Warden reveal, boss HUD, stomp/pressure-bolt tells, half-health state, shutdown, and final-hoist unlock read are not hidden by arena density.
- Enemy silhouette contrast must improve or stay stable from intended approach angles.
- Enemy readability VFX/audio must not create false damage zones or false objective state language.
- Added enemy detail must not change hitboxes, attack timing, movement blockers, spawn ownership, or route transitions unless the main lane explicitly owns a gameplay balance change and updates the smoke expectations.

### Level-Density Criteria

Level-density upgrades pass only when they improve authored readability while staying presentation-only.

- Density appears at route decisions, objective machines, combat landmarks, hazards, secrets, and exits instead of being evenly sprinkled.
- Each level keeps the critical route object readable from the normal approach direction.
- Amber/red/green state language follows the existing route convention: amber means attention/objective direction, red means locked/hostile/unsafe, green means restored/safe/exit.
- Floor trim and low detail remain near-flush and noncolliding.
- Wall and ceiling pipe density stays outside walkable/projectile lanes unless above safe head clearance.
- Hazard dressing amplifies existing steam/furnace tells and does not replace or mask them.
- Secret clues stay weaker than main route signage and do not pull the first-run player away during immediate combat pressure.

## Route-Safety Checks

### Global Checks

- No new root sits inside lift, hoist, gate, final exit, level transition, pickup trigger, hazard damage, enemy spawn, boss lock, or prompt ownership volumes.
- No route aperture is visually narrowed enough to hide the intended destination, state lamp, prompt, or enemy reveal.
- No added object creates a fake ramp, fake cover, fake interactable, false locked state, or false safe-lane marking.
- No background density uses the same brightness/color as enemy tells directly behind active combat silhouettes.
- No VFX, steam, glow, or grime overlay hides HUD objective text, boss HUD, pickup prompt, valve prompt, or lock feedback.
- No scene receives density only in low-value story clutter while route-critical beats remain unchanged.

### Level01 Checks

- From spawn, the main route still reads within a few seconds and the first movement throat does not look narrower.
- The key branch remains more visually compelling than optional clutter until the key is collected.
- The pressure gate shows a clear locked state before key acquisition and a clear continuation after unlock.
- Service lift boarding space stays visually open and prompt/transition feedback remains clear.
- The Level01 secret clue reads as optional, not as the main route.

### Level02 Checks

- The locked Boilerheart lift rejection remains readable before valve completion.
- The routing valve prompt and valve silhouette are not hidden by machinery detail.
- The restored route back to the lift reads after valve completion.
- The first Lancer silhouette and projectile tell remain visible from the spine/bridge approach.
- Cartridge-cache clue density does not overpower the main valve route.

### Level03 Checks

- Steam hazard warnings remain visible before the player enters damage timing.
- Scattergun pickup display and pickup burst remain unobstructed.
- Bellows Node pulse warning remains visible from chamber entry and does not blend with wall machinery.
- The pressure valve reads as the objective machine, not just dense background.
- Foundry lift route is clear after valve completion.

### Level04 Checks

- Furnace hazard edge language matches actual damage lanes.
- The Bulwark arena keeps broad movement space and a visible windup silhouette.
- Coal-cache secret path remains reachable from a safe position and does not cross active lethal heat.
- Emergency hoist threshold remains clear and does not crowd the final Level04 transition.
- Dense furnace glow does not wash out Scrapper/Lancer/Bulwark tells.

### Level05 Checks

- Core Ring orientation remains readable after density: side arms, Warden arena, and final hoist must not blur together.
- Warden reveal and boss HUD are visible before the player is under heavy pressure.
- Cover/density does not fully trivialize Warden pressure-bolt or stomp line-of-sight.
- Locked final hoist feedback remains readable before Warden defeat.
- Green unlock/final exit language is visible after Warden shutdown without conflicting red/amber cues.

## Pass/Fail Rules

### Required Pass

A `v0.1.34` candidate may proceed to full matrix only when all are true:

- Batch breadth meets the preferred or minimum target above.
- Editor compile and level validation pass.
- Route audit reports `V0_ROUTE_AUDIT_PASS` and no blocker findings.
- Targeted player smokes pass for the lanes touched by the batch.
- Human first-person screenshot/readability review does not identify a P0/P1 route, combat, hazard, pickup, or boss clarity issue.
- Any accepted P1 readability issue has an explicit fix or defer decision before release readiness.

### Automatic Fail

Fail the batch immediately if any are true:

- Crash, hard hang, or no quit path.
- Any P0 route blocker, transition blocker, lock dead-end, broken pickup, broken weapon switch, broken Warden unlock, or broken final exit.
- New visual-only hierarchy owns unauthorized collider, trigger, nav, damage, pickup, prompt, transition, objective, or route-state behavior.
- Route audit contradicts manual evidence or shows a blocker.
- Existing smoke marker is missing for a targeted lane.
- Enemy tell or hazard tell becomes materially less readable than `v0.1.33`.
- A level-density addition hides a required prompt, pickup, valve, gate, lift, hoist, Warden lock, boss HUD, or final exit.

### Partial / Hold

Hold the batch for tuning if any are true:

- Preferred breadth misses but minimum breadth passes.
- A level receives visible work but no route-critical beat improves.
- First-person review shows confusing but nonblocking route color language.
- Combat remains finishable but an enemy silhouette/tell is weakened in one arena.
- Hazard markings are readable in screenshots but ambiguous during movement.
- Audio/VFX upgrade passes automation but needs a human listen/readability note.

### Release-Readiness Rule

After targeted validation passes, the final release-candidate gate remains the existing full V0 matrix. `v0.1.34` should not be called release-ready until `Tools/RunV0BuildMatrix.ps1 -LogPrefix v044` passes and candidate readiness evidence is regenerated by the main lane.
