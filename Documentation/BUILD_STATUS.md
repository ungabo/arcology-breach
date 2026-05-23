# Build Status

## Current Version

`v0.0.5` checkpoint build for `Arcology Breach`.

`v0.0` core loop is complete. A small `v0.1` presentation pass has also been added while keeping the project simple.

## Completed

- Unity project initialized in `D:\__MY APPS\Unity Doom`.
- Main scene generated at `Assets/_Project/Scenes/Level01.unity`.
- First-person player movement and mouse look.
- Character collision with greybox walls.
- Plain text HUD for health, ammo, and access-shard state.
- Hitscan weapon with ammo and fire cooldown.
- Primitive mechanical melee enemies that chase, attack, take damage, and die.
- Health, ammo, and access-shard pickups.
- Red corporate lockdown gate requiring the access shard.
- Green emergency exit trigger.
- Procedural cyberpunk audio cues for weapon, pickups, enemies, player hurt, gate feedback, and win.
- Scrapper attack windup with magenta attack tell.
- Scrapper obstacle probing and simple side-steering.
- In-world labels and floor guide strips for access shard, lockdown gate, and emergency lift.
- Packaged automated playthrough test for the shard/gate/exit objective chain.
- Packaged combat smoke test for pulse-pistol damage against a Scrapper.
- Pause, death, win, and restart flow.
- Windows standalone build.

## v0.1 Presentation Additions

- Blocky camera-mounted `Pulse Pistol` placeholder.
- Muzzle flash.
- Red damage flash.
- Bobbing pickups.
- Sliding corporate lockdown gate.
- Colored point lights for access shard, gate, and exit.
- Primitive mechanical enemies with simple lens markers.

## Verification Results

- Editor smoke test: passed.
- Windows build: passed.
- Packaged runtime smoke test: passed.
- Packaged auto-playthrough test: passed.
- Packaged combat smoke test: passed.

Pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`

Latest checkpoint verification:

- `2026-05-22 20:52 -04:00`: `v0.0.1` editor smoke passed.
- `2026-05-22 20:52 -04:00`: `v0.0.1` Windows checkpoint build passed.
- `2026-05-22 20:52 -04:00`: `v0.0.1` packaged runtime smoke passed.
- `2026-05-22 21:05 -04:00`: `v0.0.2` editor smoke passed.
- `2026-05-22 21:05 -04:00`: `v0.0.2` Windows checkpoint build passed.
- `2026-05-22 21:05 -04:00`: `v0.0.2` packaged runtime smoke passed.
- `2026-05-22 21:11 -04:00`: `v0.0.3` editor smoke passed.
- `2026-05-22 21:11 -04:00`: `v0.0.3` Windows checkpoint build passed.
- `2026-05-22 21:11 -04:00`: `v0.0.3` packaged runtime smoke passed.
- `2026-05-22 23:29 -04:00`: `v0.0.4` editor smoke passed.
- `2026-05-22 23:29 -04:00`: `v0.0.4` Windows checkpoint build passed.
- `2026-05-22 23:29 -04:00`: `v0.0.4` packaged runtime smoke passed.
- `2026-05-22 23:29 -04:00`: `v0.0.4` packaged auto-playthrough passed.
- `2026-05-22 23:35 -04:00`: `v0.0.5` editor smoke passed.
- `2026-05-22 23:35 -04:00`: `v0.0.5` Windows checkpoint build passed.
- `2026-05-22 23:35 -04:00`: `v0.0.5` packaged runtime smoke passed.
- `2026-05-22 23:35 -04:00`: `v0.0.5` packaged auto-playthrough passed.
- `2026-05-22 23:35 -04:00`: `v0.0.5` packaged combat smoke passed.

## Build Path

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.5\ArcologyBreach_v0.0.5.exe`

Future checkpoints should increment as `v0.0.6`, `v0.0.7`, etc. when meaningful progress is ready for local playtesting.

## Known Limitations

- Runtime smoke test verifies boot/object presence. Auto-playthrough verifies objective flow but not human combat feel.
- Combat smoke verifies weapon raycast damage and enemy death, not full combat movement.
- Enemy pathing uses simple side-steering, not NavMesh.
- Scrapper attack windup is smoke-tested but still needs manual feel tuning.
- Audio is procedural placeholder content and still needs a human listen/tuning pass.
- No generated texture/sprite pass yet.
- No main menu.
- No settings screen.
- Android, browser/WebGL, SteamVR/OpenXR, and Meta Quest builds are planned but deferred.

## Recommended Next Manual Test

Launch the Windows build and confirm:

1. Mouse locks and look works.
2. `WASD` movement feels comfortable.
3. Left mouse fires and consumes ammo.
4. Enemies can be killed before they kill the player.
5. Access shard pickup works.
6. Red lockdown gate opens after shard collection.
7. Green emergency exit reaches win state.
8. `R` restarts after death/win.
