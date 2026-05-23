# Build Status

## Current Version

`v0.0.28` versioned build for `Brassworks Breach`.

`v0.0` core loop is complete. The current build adds collision cover to the Level01 repair bay, key room, and final room on top of the steampunk environment signage, first-person Pressure Pistol pass, service-lift, gear-key/gate art passes, generated material textures, two-level flow, ranged Lancer combat, level validation, runtime performance profile, retheme, menu/settings flow, brass HUD, pickup visuals, and impact sparks.

## Completed

- Unity project initialized in `D:\__MY APPS\Unity Doom`.
- Main scene generated at `Assets/_Project/Scenes/Level01.unity`.
- First-person player movement and mouse look.
- Character collision with greybox walls.
- Plain text HUD for health, ammo, and gear-key state.
- Hitscan pressure-pistol placeholder with ammo and fire cooldown.
- Primitive mechanical melee enemies that chase, attack, take damage, and die.
- Health, ammo, and gear-key pickups.
- Pressure gate requiring the gear key.
- Service lift exit trigger.
- Procedural steamworks audio cues for weapon, pickups, enemies, player hurt, gate feedback, and win.
- Scrapper attack windup with red-orange pressure tell.
- Scrapper obstacle probing and simple side-steering.
- In-world labels and floor guide strips for gear key, pressure gate, and service lift.
- Procedural steampunk dressing: oil-dark stone patches, pipe runs, boiler stacks, and gate hazard details.
- First readable steampunk prop pass: gear-key visual, pressure gauges, valve wheels, steam vents, and furnace prop.
- Brass HUD backplates, health/ammo fill gauges, and gear-key status lamp.
- Primitive clockwork Scrapper silhouette with boiler torso, brass chest plate, furnace eye, pressure tank, piston arms, cutter blades, and blocky feet.
- Spark-burst impact feedback replacing the yellow hit-marker sphere.
- Packaged automated playthrough test for the key/gate/lift objective chain.
- Packaged combat smoke test for pressure-pistol damage against a Scrapper.
- Pause menu with resume, restart, and quit.
- Main menu with start, quit, sensitivity, and volume controls.
- Generated Level02 Pipeworks Annex scene.
- Service lift transition from Level01 into Level02.
- Windows standalone build flow.
- Procedural texture assets for oil-dark stone floors, riveted-iron panels, and brass/copper pipe materials.
- Upright clockwork gear-key visual and heavy pressure-gate visual with keyed socket, riveted slabs, frame, header gear, lamps, and validation coverage.
- Brass service-lift cage visuals with platform deck, grate, rails, chains, pulley gear, call box, signal lamps, and validation coverage.
- Pressure Pistol viewmodel visual pass with pressure tank, muzzle crown, sights, steam vent, bolt handle, walnut grip plates, red pressure line, rivets, and validation coverage.
- Environmental work-order boards and three-pipe bundles in Level01 and Level02, with validation coverage.
- Level01 collision-cover pass for repair bay, key room, and final room, with validation coverage and objective auto-playthrough retained.

## Verification Results

Latest fully verified build: `v0.0.28`.

Current `v0.0.28` verification:

- Editor level validation: passed.
- Editor smoke test: passed.
- Windows build: passed.
- Packaged runtime smoke test: passed.
- Packaged auto-playthrough test: passed.
- Packaged combat smoke test: passed.
- Packaged combat-edge smoke test: passed.
- Packaged ranged combat smoke test: passed.
- Packaged pause-flow smoke test: passed.

Pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_COMBAT_EDGE_PASS`
- `V0_RANGED_COMBAT_PASS`
- `V0_PAUSE_FLOW_PASS`
- `V0_LEVEL_VALIDATION_PASS`

## Build Path

Current target:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.28\BrassworksBreach_v0.0.28.exe`

## Latest Build Verification

- `2026-05-23 00:49 -04:00`: `v0.0.7` scene rebuild passed.
- `2026-05-23 00:49 -04:00`: `v0.0.7` editor smoke passed.
- `2026-05-23 00:49 -04:00`: `v0.0.7` Windows build passed.
- `2026-05-23 00:50 -04:00`: `v0.0.7` packaged runtime smoke passed.
- `2026-05-23 00:50 -04:00`: `v0.0.7` packaged auto-playthrough passed.
- `2026-05-23 00:50 -04:00`: `v0.0.7` packaged combat smoke passed.
- `2026-05-23 00:50 -04:00`: `v0.0.7` packaged pause-flow smoke passed.
- `2026-05-23 00:55 -04:00`: `v0.0.8` scene rebuild passed.
- `2026-05-23 00:55 -04:00`: `v0.0.8` editor smoke passed.
- `2026-05-23 00:55 -04:00`: `v0.0.8` Windows build passed.
- `2026-05-23 00:56 -04:00`: `v0.0.8` packaged runtime smoke passed.
- `2026-05-23 00:56 -04:00`: `v0.0.8` packaged auto-playthrough passed.
- `2026-05-23 00:56 -04:00`: `v0.0.8` packaged combat smoke passed.
- `2026-05-23 00:56 -04:00`: `v0.0.8` packaged pause-flow smoke passed.
- `2026-05-23 01:01 -04:00`: `v0.0.9` scene rebuild passed.
- `2026-05-23 01:01 -04:00`: `v0.0.9` editor smoke passed.
- `2026-05-23 01:01 -04:00`: `v0.0.9` Windows build passed.
- `2026-05-23 01:02 -04:00`: `v0.0.9` packaged runtime smoke passed.
- `2026-05-23 01:02 -04:00`: `v0.0.9` packaged auto-playthrough passed.
- `2026-05-23 01:02 -04:00`: `v0.0.9` packaged combat smoke passed.
- `2026-05-23 01:02 -04:00`: `v0.0.9` packaged pause-flow smoke passed.
- `2026-05-23 01:05 -04:00`: `v0.0.10` scene rebuild passed.
- `2026-05-23 01:05 -04:00`: `v0.0.10` editor smoke passed.
- `2026-05-23 01:05 -04:00`: `v0.0.10` Windows build passed.
- `2026-05-23 01:06 -04:00`: `v0.0.10` packaged runtime smoke passed.
- `2026-05-23 01:06 -04:00`: `v0.0.10` packaged auto-playthrough passed.
- `2026-05-23 01:06 -04:00`: `v0.0.10` packaged combat smoke passed.
- `2026-05-23 01:06 -04:00`: `v0.0.10` packaged pause-flow smoke passed.
- `2026-05-23 01:08 -04:00`: `v0.0.11` editor smoke passed.
- `2026-05-23 01:08 -04:00`: `v0.0.11` Windows build passed.
- `2026-05-23 01:09 -04:00`: `v0.0.11` packaged runtime smoke passed.
- `2026-05-23 01:09 -04:00`: `v0.0.11` packaged auto-playthrough passed.
- `2026-05-23 01:09 -04:00`: `v0.0.11` packaged combat smoke passed.
- `2026-05-23 01:09 -04:00`: `v0.0.11` packaged pause-flow smoke passed.
- `2026-05-23 10:19 -04:00`: `v0.0.12` scene rebuild passed.
- `2026-05-23 10:20 -04:00`: `v0.0.12` editor smoke passed.
- `2026-05-23 10:20 -04:00`: `v0.0.12` Windows build passed.
- `2026-05-23 10:21 -04:00`: `v0.0.12` packaged runtime smoke passed.
- `2026-05-23 10:21 -04:00`: `v0.0.12` packaged auto-playthrough passed.
- `2026-05-23 10:21 -04:00`: `v0.0.12` packaged combat smoke passed.
- `2026-05-23 10:21 -04:00`: `v0.0.12` packaged pause-flow smoke passed.
- `2026-05-23 10:24 -04:00`: `v0.0.13` scene rebuild passed.
- `2026-05-23 10:25 -04:00`: `v0.0.13` editor smoke passed.
- `2026-05-23 10:26 -04:00`: `v0.0.13` Windows build passed.
- `2026-05-23 10:27 -04:00`: `v0.0.13` packaged runtime smoke passed.
- `2026-05-23 10:27 -04:00`: `v0.0.13` packaged auto-playthrough passed.
- `2026-05-23 10:27 -04:00`: `v0.0.13` packaged combat smoke passed.
- `2026-05-23 10:27 -04:00`: `v0.0.13` packaged pause-flow smoke passed.
- `2026-05-23 10:32 -04:00`: `v0.0.14` scene rebuild passed.
- `2026-05-23 10:33 -04:00`: `v0.0.14` editor smoke passed.
- `2026-05-23 10:34 -04:00`: `v0.0.14` Windows build passed.
- `2026-05-23 10:35 -04:00`: `v0.0.14` packaged runtime smoke passed.
- `2026-05-23 10:36 -04:00`: `v0.0.14` packaged auto-playthrough passed.
- `2026-05-23 10:36 -04:00`: `v0.0.14` packaged combat smoke passed.
- `2026-05-23 10:36 -04:00`: `v0.0.14` packaged pause-flow smoke passed.
- `2026-05-23 10:39 -04:00`: `v0.0.15` scene rebuild passed.
- `2026-05-23 10:40 -04:00`: `v0.0.15` editor smoke passed.
- `2026-05-23 10:41 -04:00`: `v0.0.15` Windows build passed.
- `2026-05-23 10:42 -04:00`: `v0.0.15` packaged runtime smoke passed.
- `2026-05-23 10:42 -04:00`: `v0.0.15` packaged auto-playthrough passed.
- `2026-05-23 10:43 -04:00`: `v0.0.15` packaged combat smoke passed.
- `2026-05-23 10:43 -04:00`: `v0.0.15` packaged pause-flow smoke passed.
- `2026-05-23 10:48 -04:00`: `v0.0.16` scene rebuild passed.
- `2026-05-23 10:49 -04:00`: `v0.0.16` editor smoke passed.
- `2026-05-23 10:51 -04:00`: `v0.0.16` Windows build passed.
- `2026-05-23 10:52 -04:00`: `v0.0.16` packaged runtime smoke passed.
- `2026-05-23 10:52 -04:00`: `v0.0.16` packaged auto-playthrough passed.
- `2026-05-23 10:53 -04:00`: `v0.0.16` packaged combat smoke passed.
- `2026-05-23 10:53 -04:00`: `v0.0.16` packaged pause-flow smoke passed.
- `2026-05-23 10:57 -04:00`: `v0.0.17` scene rebuild passed.
- `2026-05-23 10:58 -04:00`: `v0.0.17` editor smoke passed.
- `2026-05-23 10:59 -04:00`: `v0.0.17` Windows build passed.
- `2026-05-23 11:00 -04:00`: `v0.0.17` packaged runtime smoke passed.
- `2026-05-23 11:00 -04:00`: `v0.0.17` packaged auto-playthrough passed.
- `2026-05-23 11:01 -04:00`: `v0.0.17` packaged combat smoke passed.
- `2026-05-23 11:01 -04:00`: `v0.0.17` packaged pause-flow smoke passed.
- `2026-05-23 11:05 -04:00`: `v0.0.18` scene rebuild passed.
- `2026-05-23 11:06 -04:00`: `v0.0.18` editor smoke passed.
- `2026-05-23 11:07 -04:00`: `v0.0.18` Windows build passed.
- `2026-05-23 11:08 -04:00`: `v0.0.18` packaged runtime smoke passed.
- `2026-05-23 11:08 -04:00`: `v0.0.18` packaged auto-playthrough passed.
- `2026-05-23 11:09 -04:00`: `v0.0.18` packaged combat smoke passed.
- `2026-05-23 11:09 -04:00`: `v0.0.18` packaged pause-flow smoke passed.
- `2026-05-23 11:13 -04:00`: `v0.0.19` scene rebuild passed.
- `2026-05-23 11:14 -04:00`: `v0.0.19` editor smoke passed.
- `2026-05-23 11:15 -04:00`: `v0.0.19` Windows build passed.
- `2026-05-23 11:16 -04:00`: `v0.0.19` packaged runtime smoke passed.
- `2026-05-23 11:16 -04:00`: `v0.0.19` packaged auto-playthrough passed.
- `2026-05-23 11:16 -04:00`: `v0.0.19` packaged combat smoke passed.
- `2026-05-23 11:17 -04:00`: `v0.0.19` packaged ranged combat smoke passed.
- `2026-05-23 11:17 -04:00`: `v0.0.19` packaged pause-flow smoke passed.
- `2026-05-23 11:32 -04:00`: `v0.0.20` scene rebuild passed.
- `2026-05-23 11:33 -04:00`: `v0.0.20` editor smoke passed.
- `2026-05-23 11:35 -04:00`: `v0.0.20` Windows build passed.
- `2026-05-23 11:36 -04:00`: `v0.0.20` packaged runtime smoke passed.
- `2026-05-23 11:36 -04:00`: `v0.0.20` packaged auto-playthrough passed.
- `2026-05-23 11:37 -04:00`: `v0.0.20` packaged combat smoke passed.
- `2026-05-23 11:37 -04:00`: `v0.0.20` packaged combat-edge smoke passed.
- `2026-05-23 11:38 -04:00`: `v0.0.20` packaged ranged combat smoke passed.
- `2026-05-23 11:38 -04:00`: `v0.0.20` packaged pause-flow smoke passed.
- `2026-05-23 11:40 -04:00`: `v0.0.21` scene rebuild passed.
- `2026-05-23 11:41 -04:00`: `v0.0.21` editor smoke passed.
- `2026-05-23 11:43 -04:00`: `v0.0.21` Windows build passed.
- `2026-05-23 11:43 -04:00`: `v0.0.21` packaged runtime smoke passed.
- `2026-05-23 11:44 -04:00`: `v0.0.21` packaged auto-playthrough passed.
- `2026-05-23 11:44 -04:00`: `v0.0.21` packaged combat smoke passed.
- `2026-05-23 11:44 -04:00`: `v0.0.21` packaged combat-edge smoke passed.
- `2026-05-23 11:45 -04:00`: `v0.0.21` packaged ranged combat smoke passed.
- `2026-05-23 11:45 -04:00`: `v0.0.21` packaged pause-flow smoke passed.
- `2026-05-23 11:48 -04:00`: `v0.0.22` scene rebuild passed.
- `2026-05-23 11:49 -04:00`: `v0.0.22` level validation passed.
- `2026-05-23 11:50 -04:00`: `v0.0.22` editor smoke passed.
- `2026-05-23 11:51 -04:00`: `v0.0.22` Windows build passed.
- `2026-05-23 11:52 -04:00`: `v0.0.22` packaged runtime smoke passed.
- `2026-05-23 11:52 -04:00`: `v0.0.22` packaged auto-playthrough passed.
- `2026-05-23 11:53 -04:00`: `v0.0.22` packaged combat smoke passed.
- `2026-05-23 11:53 -04:00`: `v0.0.22` packaged combat-edge smoke passed.
- `2026-05-23 11:54 -04:00`: `v0.0.22` packaged ranged combat smoke passed.
- `2026-05-23 11:54 -04:00`: `v0.0.22` packaged pause-flow smoke passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` scene rebuild passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` level validation passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` editor smoke passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` Windows build passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` packaged runtime smoke passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` packaged auto-playthrough passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` packaged combat smoke passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` packaged combat-edge smoke passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` packaged ranged combat smoke passed.
- `2026-05-23 12:03 -04:00`: `v0.0.23` packaged pause-flow smoke passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` scene rebuild passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` level validation passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` editor smoke passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` Windows build passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` packaged runtime smoke passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` packaged auto-playthrough passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` packaged combat smoke passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` packaged combat-edge smoke passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` packaged ranged combat smoke passed.
- `2026-05-23 12:12 -04:00`: `v0.0.24` packaged pause-flow smoke passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` scene rebuild passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` level validation passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` editor smoke passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` Windows build passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` packaged runtime smoke passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` packaged auto-playthrough passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` packaged combat smoke passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` packaged combat-edge smoke passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` packaged ranged combat smoke passed.
- `2026-05-23 12:18 -04:00`: `v0.0.25` packaged pause-flow smoke passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` scene rebuild passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` level validation passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` editor smoke passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` Windows build passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` packaged runtime smoke passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` packaged auto-playthrough passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` packaged combat smoke passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` packaged combat-edge smoke passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` packaged ranged combat smoke passed.
- `2026-05-23 12:23 -04:00`: `v0.0.26` packaged pause-flow smoke passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` scene rebuild passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` level validation passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` editor smoke passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` Windows build passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` packaged runtime smoke passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` packaged auto-playthrough passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` packaged combat smoke passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` packaged combat-edge smoke passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` packaged ranged combat smoke passed.
- `2026-05-23 12:28 -04:00`: `v0.0.27` packaged pause-flow smoke passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` scene rebuild passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` level validation passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` editor smoke passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` Windows build passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` packaged runtime smoke passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` packaged auto-playthrough passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` packaged combat smoke passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` packaged combat-edge smoke passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` packaged ranged combat smoke passed.
- `2026-05-23 12:33 -04:00`: `v0.0.28` packaged pause-flow smoke passed.

Future builds should increment as `v0.0.29`, `v0.0.30`, etc. when meaningful progress is ready for local playtesting.

## Known Limitations

- Runtime smoke test verifies boot/object presence. Auto-playthrough verifies the current two-level objective flow but not human combat feel.
- Combat smoke verifies weapon raycast damage and enemy death; combat-edge smoke verifies empty ammo, Scrapper melee damage, and player death state; ranged combat smoke verifies Lancer projectile damage.
- Visual dressing is still procedural primitive art, though the objective props now have stronger final-direction silhouettes.
- Windows runtime performance profile exists, but visible performance profiling still needs a real play session.
- Enemy pathing uses simple side-steering, not NavMesh.
- Scrapper attack windup is smoke-tested but still needs manual feel tuning.
- Audio is procedural placeholder content and still needs a human listen/tuning pass.
- Generated material textures exist; sprites, decals, and final raster art still need production passes.
- Health and ammo persist across level transitions; future weapon inventory and campaign flags still need expansion.
- Settings exist for sensitivity and master volume only; resolution, flash intensity, and color readability are still planned.
- Android, browser/WebGL, SteamVR/OpenXR, and Meta Quest builds are planned but deferred.
