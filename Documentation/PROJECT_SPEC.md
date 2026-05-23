# Arcology Breach - Project Specification

## 1. Project Intent

Build an original cyberpunk first-person action game in Unity for Windows. The current project is a working greybox proof of concept. The long-term game is `Arcology Breach`: a fast, stylized shooter set inside a sealed corporate arcology where autonomous security systems have converted civic machines into predatory mechanical bodies.

The broad genre target is fast movement, readable combat, compact levels, keys/locks/objectives, secrets, and strong enemy silhouettes, all expressed through a unique cyberpunk machine-horror identity.

## 2. Current Version

Current state: `v0.1`.

Implemented:

- FPS movement and mouse look.
- Character collision.
- Hitscan weapon.
- Ammo, health, death, restart.
- Primitive mechanical melee enemies.
- Access shard pickup.
- Red corporate lockdown gate.
- Green emergency exit trigger.
- Plain HUD and crosshair.
- Blocky `Pulse Pistol` placeholder.
- Muzzle flash, damage flash, bobbing pickups, sliding gate, colored accent lights.
- Unity editor smoke test, Windows build, and runtime smoke test.

## 3. Target Platform

- Platform: Windows desktop.
- Engine: Unity `6000.4.6f1`.
- Input: mouse and keyboard.
- Build type: Windows x86_64 standalone.
- Performance target: stable 60 FPS on a normal desktop machine.
- Visual target: heavily stylized cyberpunk, not realism-first.

## 4. Core Pitch

A black-market courier called `Vey` breaks into `Aster Gate`, a quarantined corporate arcology owned by Sable Meridian. Inside, the autonomous security algorithm `INTERDICT` has reprinted maintenance machines, drones, surgical rigs, and security frames into mechanical hunters. Vey needs an access shard, a route through the lockdown gates, and a way to reach the control stack before the algorithm expands into the lower city.

## 5. Design Pillars

1. Cyberpunk through and through
   - Neon, black chrome, holograms, rain, cables, corporate signage, surveillance, graffiti, drones, and dense machine spaces.

2. Mechanical enemy identity
   - Enemies are corrupted infrastructure: service frames, drones, riot-control units, medical machines, and server nodes.

3. Fast readable combat
   - Movement matters.
   - Enemy roles are obvious.
   - Attack tells and weak points are visible.

4. Clear objective routing
   - Access shard opens lockdown gate.
   - Green exit path is visually distinct.
   - Optional secrets can deepen the route without confusing the core path.

5. Story through environment
   - Minimal forced exposition.
   - Use signage, logs, intercom lines, graffiti, dead drops, and room composition.

## 6. v0.2 Target

The next milestone is `v0.2 Combat Feel Slice`.

Required:

- Manual Windows playthrough.
- Movement/enemy/combat tuning.
- Better mechanical enemy chase/attack readability.
- Simple cyberpunk audio set.
- Clearer access-shard and lockdown-gate feedback.
- Updated objective text and object names.

## 7. Gameplay Systems

### Player

- First-person controller.
- Mouse look.
- WASD movement.
- No jump for now.
- Fast, clean movement.
- Health and death.
- Restart after death/win.

### Weapons

Current:

- `Pulse Pistol` placeholder.
- Hitscan raycast.
- Ammo consumption.
- Fire cooldown.
- Muzzle flash and hit marker.

Future:

- Stylized cyberpunk `Pulse Pistol`.
- Rail shotgun.
- Arc weapon against drone swarms.
- Data-driven weapon definitions.

### Enemies

Current:

- Primitive mechanical melee chaser.

Future enemy family:

- `Scrapper`: maintenance frame melee unit.
- `Lancer`: ranged security chassis.
- `Bulwark`: heavy riot frame.
- `Needle Swarm`: fast surgical drone group.
- `Choir Node`: stationary control amplifier.

### World

Current:

- Access shard pickup.
- Red lockdown gate.
- Green emergency exit.

Future:

- Switch panels.
- Data locks.
- Security shutters.
- Secret service hatches.
- Environmental hazards.

### UI

Current:

- Plain text health/ammo/access state.
- Text crosshair.
- Death/win/pause messages.

Future:

- Diegetic cyberpunk HUD.
- Glitch-style damage feedback.
- Corporate objective prompts.
- Accessibility options.

## 8. Visual Direction

The game should look like a stylized neon corporate machine-space, not a generic sci-fi shooter.

Core visual elements:

- Wet concrete.
- Black chrome.
- Cyan/magenta/amber neon.
- Holographic ads and warning strips.
- Sable Meridian corporate signage.
- Exposed cable trunks.
- Server heat vents.
- Maintenance rails.
- Surveillance cameras.
- Machine lens clusters.
- Graffiti from the lower-city resistance.
- Glitch patterns and signal noise.

Gameplay color language:

- Cyan: player tech and friendly systems.
- Magenta: hostile signal corruption and elite threats.
- Amber/yellow: access shards, pickups, warnings.
- Red: locked, denied, damage.
- Green: exits, restore, bypass success.

## 9. First Level Retheme

Current `Level01` becomes `Aster Gate Intake`.

Rooms:

1. Flooded service intake.
2. Cable-lined maintenance throat.
3. Robot repair bay.
4. Access-shard kiosk.
5. Red corporate lockdown gate.
6. Transit control node.
7. Green emergency lift/data gate.

Detailed level scale, map planning, and future level transition mechanics live in `LEVEL_DESIGN_AND_MAPS.md`.

## 10. Level and Progression Direction

Levels should be compact, readable FPS maps built around loops, locked routes, secrets, and strong cyberpunk landmarks.

Current level transition:

- The green emergency lift/data gate ends the level and triggers the win state.

Future level transition:

- A diegetic transit device should load the next scene through a dedicated `LevelTransitionController`.
- Weapons and durable player state should carry forward.
- Level-scoped access shards should reset per map unless a story reason makes them campaign-scoped.
- Android and browser versions may simplify level geometry and reduce encounter density.
- VR versions need stable spawn orientation, fade transitions, and no required jumping or forced abrupt camera motion.

Planned campaign map ladder:

1. `Aster Gate Intake`: tutorial/combat proof, access shard, lockdown gate.
2. `Transit Spine`: longer sightlines, first ranged security chassis, transit power objective.
3. `Data Stack`: server maze, data locks, support-node enemy.
4. `Civic Machine Foundry`: industrial hazards, heavy enemy introduction.
5. `Interdict Core`: final breach, mixed enemy groups, possible core guardian.

## 11. Asset Direction

The project should generate assets in disciplined passes, not huge blind batches.

Near-term assets:

- Cyberpunk wall/floor material set.
- Access shard pickup.
- Lockdown gate.
- Emergency lift/data gate.
- Scrapper enemy visual.
- Pulse Pistol visual.
- Muzzle/impact VFX.
- Simple cyberpunk audio set.

Full asset detail lives in `AAA_ASSET_CATALOG.md`.

Installed/local Asset Store candidate packs are tracked in `ASSET_PACK_REVIEW.md`.

## 12. Testing Strategy

Automated:

- Unity editor smoke.
- Windows build.
- Packaged runtime smoke.

Manual:

- Full Windows playthrough.
- Death/restart.
- Access shard/gate flow.
- Enemy combat tuning.
- HUD readability.
- Mouse lock and input.

## 13. Acceptance Criteria for v0.2

`v0.2` is complete when:

- The Windows build can be manually completed from start to exit.
- Mechanical enemy combat feels fair enough for iteration.
- Access shard and lockdown gate are understandable.
- Cyberpunk objective text and presentation are in place.
- First audio feedback pass exists.
- Handoff, build status, and work ledger are updated.

## 14. Non-Goals for Next Milestone

- Full campaign.
- Complex cinematics.
- Large asset batch generation.
- Multiplayer.
- Save system.
- Final UI.
- Final enemy animation.
- Final weapon set.

## 15. Source Documents

- `CYBERPUNK_STORY_BIBLE.md`
- `LEVEL_DESIGN_AND_MAPS.md`
- `AAA_VISION_AND_ROADMAP.md`
- `AAA_ASSET_CATALOG.md`
- `ASSET_PACK_REVIEW.md`
- `PRODUCTION_TRACKING_METHOD.md`
- `WORK_LEDGER.md`
- `HANDOFF.md`
