# Brassworks Breach - Project Specification

## 1. Project Intent

Build an original steampunk first-person dungeon crawler/shooter in Unity for Windows. The current project is a working greybox proof of concept. The long-term game is `Brassworks Breach`: a fast, readable, stylized action game set inside a sealed industrial works where pressure systems and clockwork maintenance machines have turned lethal.

The genre target remains compact classic FPS exploration: movement, shooting, keys, locks, secrets, strong silhouettes, and clear level routes.

## 2. Current Version

Current state: `v0.0.39`.

Implemented:

- FPS movement and mouse look.
- Character collision.
- Hitscan pressure-pistol placeholder.
- Ammo, health, death, restart, pause, and quit flow.
- Primitive mechanical melee enemies.
- Gear-key pickup.
- Pressure gate.
- Service lift exit trigger.
- Plain HUD and crosshair.
- Muzzle flash, damage flash, bobbing pickups, sliding gate, colored accent lights.
- Procedural steamworks audio cues for firing, pickups, enemy hits/death, player hurt, gate feedback, and win.
- Scrapper attack windup with red-orange pressure tell.
- Scrapper obstacle probing and simple side-steering.
- Gear-key pedestal, gate/lift guide strips, and world labels.
- Procedural steampunk dressing pass with oil-dark stone patches, copper pipes, boiler stacks, and gate hazard details.
- First readable steampunk prop silhouette pass with gear key, pressure gauges, valve wheels, steam vents, and furnace prop.
- First brass instrument-panel HUD pass with health/ammo gauges and gear-key status lamp.
- First clockwork Scrapper silhouette pass with brass/iron body parts and cutter arms.
- Spark-burst impact feedback.
- Packaged auto-playthrough for the gear-key, pressure-gate, and service-lift objective chain.
- Packaged combat smoke for pressure-pistol damage and Scrapper death.
- Packaged pause-flow smoke for pause/resume/restart/quit.
- Unity editor smoke test, Windows build, and runtime smoke test.
- Two-level flow with Level01 and Level02.
- Ranged Lancer enemy prototype and ranged-combat smoke test.
- Centralized `GameBalance` values.
- Data-driven Pressure Pistol, Scrapper, and Lancer definitions.
- One-command V0 Windows build matrix runner.
- First-person interaction scanner, HUD prompt, and interactable gate/lift hooks.
- Data-driven health, ammo, and gear-key pickup definitions.
- Reusable level transition controller for service lifts and restarts.
- Data-driven platform quality profiles for Windows, Android, WebGL, PC VR, and Meta Quest.
- Expanded combat scenario automation for cooldowns, ammo accounting, and expected kill timing.
- Three-level service-lift campaign chain ending at Level03 Boilerheart Core.
- Boilerheart pressure-valve objective that locks the Level03 final lift until vented.

## 3. Target Platform

- Platform: Windows desktop.
- Engine: Unity `6000.4.6f1`.
- Input: mouse and keyboard.
- Build type: Windows x86_64 standalone.
- Performance target: stable 60 FPS on a mid-to-low level gaming PC.
- Visual target: heavily stylized steampunk, not realism-first.

Android, browser/WebGL, SteamVR, and Meta Quest are deferred platforms. Their constraints should still influence input, scale, comfort, and asset budgets.

## 4. Core Pitch

A pressure-runner enters the sealed Brassworks after the master governor jams, trapping workers below and turning maintenance automata into hostile enforcement machines. To descend deeper, the player must recover gear keys, open pressure gates, survive clockwork enemies, and reach service lifts before the entire works overloads.

## 5. Design Pillars

1. Steampunk through and through
   - Brass, copper, riveted iron, soot, oil, gaslight, gauges, valves, gears, pistons, and steam.

2. Mechanical enemy identity
   - Enemies are repurposed industrial tools: maintenance frames, boiler scouts, valve-rifle sentries, furnace-plated heavies, and pressure amplifiers.

3. Fast readable combat
   - Movement matters.
   - Enemy roles are obvious.
   - Attack tells and weak points are visible.

4. Clear objective routing
   - Gear keys open pressure gates.
   - Service lifts end levels.
   - Optional secrets can deepen the route without confusing the core path.

5. Story through environment
   - Minimal forced exposition.
   - Use signage, work orders, speaking tubes, stamped warnings, machinery layout, and room composition.

## 6. v0.2 Target

The next gameplay milestone is `v0.2 Steampunk Combat Feel Slice`.

Required:

- Manual Windows playthrough.
- Movement/enemy/combat tuning.
- Better mechanical enemy navigation/obstacle handling.
- Manual listen/tuning pass for the procedural steamworks audio set.
- Manual readability pass for Scrapper attack tells and objective labels.
- Steampunk objective text and object names throughout the build.

## 7. Gameplay Systems

### Player

- First-person controller.
- Mouse look.
- WASD movement.
- No jump for now.
- Fast, clean movement.
- Health and death.
- Pause, resume, restart, and quit.

### Weapons

Current:

- `Pressure Pistol` placeholder.
- Hitscan raycast.
- Ammo consumption.
- Fire cooldown.
- Muzzle flash and hit marker.

Future:

- Final brass-and-walnut `Pressure Pistol`.
- Steam scattergun.
- Arc-valve rifle or rivet launcher.
- Data-driven weapon definitions.

### Enemies

Current:

- Primitive mechanical melee chaser.

Future enemy family:

- `Scrapper`: maintenance frame melee unit.
- `Boiler Tick`: small pressure scout.
- `Lancer`: ranged valve-rifle automaton.
- `Bulwark`: heavy furnace-plated machine.
- `Bellows Node`: stationary pressure amplifier.

### World

Current:

- Gear-key pickup.
- Pressure gate.
- Service lift exit.

Future:

- Valve wheels.
- Pressure locks.
- Gearboxes.
- Secret service hatches.
- Steam and furnace hazards.

### UI

Current:

- Plain text health/ammo/key state.
- Text crosshair.
- Pause/death/win messages.

Future:

- Brass gauge HUD.
- Red pressure damage feedback.
- Gear-key iconography.
- Accessibility options.

## 8. Visual Direction

Use the two north-star concept sheets in `Documentation/ConceptArt` as the visual target. The game should look like a compact industrial dungeon of brass, iron, oil, stone, and steam.

Core visual elements:

- Soot brick.
- Oil-dark stone.
- Riveted iron.
- Brass and copper pipes.
- Valve wheels.
- Pressure gauges.
- Furnace glow.
- Gear-driven doors.
- Clockwork joints.
- Steam vents.
- Wet floors and oil puddles.
- Mechanical instrument-panel UI.

Gameplay color language:

- Brass/amber: objectives, keys, useful machinery.
- Red-orange: danger, denied pressure locks, enemy attack tells, damage.
- Green: exits, restored lift systems, success states.
- Dark iron/oil brown: neutral architecture.
- Cream enamel: readable labels and gauge faces.

## 9. First Level Retheme

Current `Level01` becomes `Brassworks Intake`.

Rooms:

1. Soot-brick service entry.
2. Copper-pipe maintenance throat.
3. Clockwork repair bay.
4. Gear-key plinth.
5. Pressure gate.
6. Furnace control room.
7. Service lift.

Detailed level scale, map planning, and future level transition mechanics live in `LEVEL_DESIGN_AND_MAPS.md`.

## 10. Level and Progression Direction

Levels should be compact, readable FPS maps built around loops, locked routes, secrets, and strong steampunk landmarks.

Current level transition:

- The service lift ends the level and triggers the win state.

Future level transition:

- A diegetic lift, tram, or pressure elevator should load the next scene through a dedicated `LevelTransitionController`.
- Weapons and durable player state should carry forward.
- Level-scoped gear keys should reset per map unless a story reason makes them campaign-scoped.
- Android and browser versions may simplify level geometry and reduce encounter density.
- VR versions need stable spawn orientation, fade transitions, and no required jumping or forced abrupt camera motion.

Planned campaign map ladder:

1. `Brassworks Intake`: tutorial/combat proof, gear key, pressure gate.
2. `Pipeworks Spine`: longer sightlines, first ranged machine, pressure-routing objective.
3. `Gauge Hall`: lock sequences, valve puzzles, support-node enemy.
4. `Furnace Foundry`: industrial hazards, heavy enemy introduction.
5. `Governor Core`: final breach, mixed enemy groups, possible core guardian.

## 11. Asset Direction

Near-term assets:

- Brass/iron/oil-stone material set.
- Gear-key pickup.
- Pressure gate.
- Service lift.
- Scrapper enemy visual.
- Pressure Pistol visual.
- Steam/spark impact VFX.
- Procedural steamworks audio set.

Full asset detail lives in `AAA_ASSET_CATALOG.md`.

Installed/local Asset Store candidate packs are tracked in `ASSET_PACK_REVIEW.md`.

## 12. Testing Strategy

Automated:

- Unity editor smoke.
- Windows build.
- Packaged runtime smoke.
- Packaged auto-playthrough for objective progression.
- Packaged combat smoke for weapon/enemy regression.
- Packaged pause-flow smoke.

Manual:

- Full Windows playthrough.
- Death/restart/pause/quit.
- Gear-key/pressure-gate flow.
- Enemy combat tuning.
- HUD readability.
- Mouse lock and input.

## 13. Acceptance Criteria for v0.2

`v0.2` is complete when:

- The Windows build can be manually completed from start to service lift.
- The packaged auto-playthrough can complete the objective chain.
- The packaged combat smoke verifies the pressure pistol can kill a Scrapper.
- Mechanical enemy combat feels fair enough for iteration.
- Gear key and pressure gate are understandable.
- Steampunk objective text and presentation are in place.
- First lightweight steampunk dressing pass exists.
- First audio feedback pass exists and has been manually tuned.

## 14. Non-Goals for Next Milestone

- Full campaign.
- Complex cinematics.
- Large blind asset batch generation.
- Multiplayer.
- Save system.
- Final UI.
- Final enemy animation.
- Final weapon set.

## 15. Source Documents

- `STEAMPUNK_NORTH_STAR.md`
- `STORY_AND_LORE_BIBLE.md`
- `LEVEL_DESIGN_AND_MAPS.md`
- `AAA_VISION_AND_ROADMAP.md`
- `AAA_ASSET_CATALOG.md`
- `ASSET_PACK_REVIEW.md`
- `PRODUCTION_TRACKING_METHOD.md`
- `WORK_LEDGER.md`
