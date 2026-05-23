# Build Status

## Current Version

`v0.0.7` versioned build for `Brassworks Breach`.

`v0.0` core loop is complete. The current build rethemes the project to the steampunk north star and adds pause/quit flow.

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
- Packaged automated playthrough test for the key/gate/lift objective chain.
- Packaged combat smoke test for pressure-pistol damage against a Scrapper.
- Pause menu with resume, restart, and quit.
- Windows standalone build flow.

## Verification Results

Latest fully verified build: `v0.0.7`.

Current `v0.0.7` verification:

- Editor smoke test: passed.
- Windows build: passed.
- Packaged runtime smoke test: passed.
- Packaged auto-playthrough test: passed.
- Packaged combat smoke test: passed.
- Packaged pause-flow smoke test: passed.

Pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

## Build Path

Current target:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.7\BrassworksBreach_v0.0.7.exe`

## Latest Build Verification

- `2026-05-23 00:49 -04:00`: `v0.0.7` scene rebuild passed.
- `2026-05-23 00:49 -04:00`: `v0.0.7` editor smoke passed.
- `2026-05-23 00:49 -04:00`: `v0.0.7` Windows build passed.
- `2026-05-23 00:50 -04:00`: `v0.0.7` packaged runtime smoke passed.
- `2026-05-23 00:50 -04:00`: `v0.0.7` packaged auto-playthrough passed.
- `2026-05-23 00:50 -04:00`: `v0.0.7` packaged combat smoke passed.
- `2026-05-23 00:50 -04:00`: `v0.0.7` packaged pause-flow smoke passed.

Future builds should increment as `v0.0.8`, `v0.0.9`, etc. when meaningful progress is ready for local playtesting.

## Known Limitations

- Runtime smoke test verifies boot/object presence. Auto-playthrough verifies objective flow but not human combat feel.
- Combat smoke verifies weapon raycast damage and enemy death, not full combat movement.
- Visual dressing is still procedural primitive art, not final generated assets.
- Enemy pathing uses simple side-steering, not NavMesh.
- Scrapper attack windup is smoke-tested but still needs manual feel tuning.
- Audio is procedural placeholder content and still needs a human listen/tuning pass.
- No generated texture/sprite pass yet.
- No main menu.
- No settings screen.
- Android, browser/WebGL, SteamVR/OpenXR, and Meta Quest builds are planned but deferred.
