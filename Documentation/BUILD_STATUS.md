# Build Status

## Current Version

`v0.1` proof-of-concept build.

`v0.0` core loop is complete. A small `v0.1` presentation pass has also been added while keeping the project simple.

## Completed

- Unity project initialized in `D:\__MY APPS\Unity Doom`.
- Main scene generated at `Assets/_Project/Scenes/Level01.unity`.
- First-person player movement and mouse look.
- Character collision with greybox walls.
- Plain text HUD for health, ammo, and key state.
- Hitscan weapon with ammo and fire cooldown.
- Primitive melee enemies that chase, attack, take damage, and die.
- Health, ammo, and key pickups.
- Locked red door requiring key.
- Green exit trigger.
- Pause, death, win, and restart flow.
- Windows standalone build.

## v0.1 Presentation Additions

- Blocky camera-mounted weapon placeholder.
- Muzzle flash.
- Red damage flash.
- Bobbing pickups.
- Sliding locked door.
- Colored point lights for key, door, and exit.
- Primitive enemies with simple eye markers.

## Verification Results

- Editor smoke test: passed.
- Windows build: passed.
- Packaged runtime smoke test: passed.

Pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`

## Build Path

`D:\__MY APPS\Unity Doom\Builds\Windows\IronChapelV0.exe`

## Known Limitations

- Runtime smoke test verifies boot/object presence, not a full human playthrough.
- Enemy pathing is simple direct movement, not NavMesh.
- No custom audio yet.
- No generated texture/sprite pass yet.
- No main menu.
- No settings screen.

## Recommended Next Manual Test

Launch the Windows build and confirm:

1. Mouse locks and look works.
2. `WASD` movement feels comfortable.
3. Left mouse fires and consumes ammo.
4. Enemies can be killed before they kill the player.
5. Key pickup works.
6. Red door opens after key collection.
7. Green exit reaches win state.
8. `R` restarts after death/win.
