# Handoff

Last updated: 2026-05-22

## Project

`Arcology Breach` is an original cyberpunk first-person dungeon crawler/shooter proof of concept. It now has a working checkpoint build flow, a story/lore bible, level-map planning, asset-pack review notes, a first procedural audio pass, objective/combat readability improvements, packaged objective/combat automation, and a first procedural cyberpunk dressing pass.

Local path:

`D:\__MY APPS\Unity Doom`

GitHub repo:

`https://github.com/ungabo/arcology-breach`

Unity version:

`6000.4.6f1`

## Current Working State

The project contains:

- Unity project source.
- Generated `Level01` scene.
- Greybox dungeon layout.
- FPS movement and mouse look.
- Hitscan weapon.
- Ammo, health, access-shard state.
- Primitive mechanical melee enemies.
- Access shard pickup.
- Red corporate lockdown gate.
- Green emergency exit trigger.
- Pause, death, win, restart.
- Text HUD and crosshair.
- v0.1 polish: blocky `Pulse Pistol`, muzzle flash, damage flash, bobbing pickups, sliding gate, colored lights, enemy lens markers.
- v0.0.2 audio: procedural pulse pistol, empty click, pickup, enemy hit/death, player hurt, gate, and win cues.
- v0.0.3 readability: Scrapper attack windup, objective labels, floor guide strips, and access-shard pedestal.
- v0.0.4 testing/navigation: packaged auto-playthrough for shard/gate/exit and simple Scrapper obstacle side-steering.
- v0.0.5 testing: packaged combat smoke for pulse-pistol damage and Scrapper death.
- v0.0.6 visuals: wet floor patches, server stacks, cable trunks, hazard strips, and gate header.

## Build

Windows executable path:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.6\ArcologyBreach_v0.0.6.exe`

The build folder is ignored by git. Rebuild it locally when needed.

## Verified

Latest known pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`

Latest checkpoint:

- `v0.0.6`
- Build: `Builds/Windows/v0.0.6/ArcologyBreach_v0.0.6.exe`
- Verified by editor smoke, Windows build, runtime smoke, packaged auto-playthrough, and packaged combat smoke on 2026-05-22 at 23:39 -04:00.

Important logs:

- `Logs\build-v006-scene.log`
- `Logs\v006-smoke-test.log`
- `Logs\v006-windows-build.log`
- `Logs\v006-runtime-smoke.log`
- `Logs\v006-auto-playthrough.log`
- `Logs\v006-combat-smoke.log`

Logs are ignored by git.

## Important Files

- `README.md`
- `Documentation/BUILD_STATUS.md`
- `Documentation/V0_SCOPE.md`
- `Documentation/CYBERPUNK_STORY_BIBLE.md`
- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Documentation/AAA_VISION_AND_ROADMAP.md`
- `Documentation/AAA_ASSET_CATALOG.md`
- `Documentation/ASSET_PACK_REVIEW.md`
- `Documentation/PRODUCTION_TRACKING_METHOD.md`
- `Documentation/WORK_LEDGER.md`
- `Documentation/SESSION_LOG.md`
- `Documentation/TITLE_AND_BRANDING_TRACKER.md`
- `Documentation/PLATFORM_WINDOWS_TARGET.md`
- `Documentation/PLATFORM_ANDROID_PORT_NOTES.md`
- `Documentation/PLATFORM_WEB_BROWSER_PORT_NOTES.md`
- `Documentation/PLATFORM_VR_PORT_NOTES.md`
- `Assets/_Project/Editor/V0SceneBuilder.cs`
- `Assets/_Project/Scenes/Level01.unity`
- `Assets/_Project/Scripts/Player/PlayerController.cs`
- `Assets/_Project/Scripts/Weapons/WeaponController.cs`
- `Assets/_Project/Scripts/Enemies/EnemyController.cs`
- `Assets/_Project/Scripts/Utility/CyberpunkAudio.cs`
- `Assets/_Project/Scripts/Utility/RuntimeAutoPlaythroughTest.cs`
- `Assets/_Project/Scripts/Utility/RuntimeCombatTest.cs`
- `Assets/_Project/Scripts/World/GameStateController.cs`

## Commands

Rebuild generated scene:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\build-v006-scene.log'
```

Editor smoke:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.RunSmokeTest -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v006-smoke-test.log'
```

Windows build:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildWindowsV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v006-windows-build.log'
```

Packaged runtime smoke:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.6\ArcologyBreach_v0.0.6.exe' -batchmode -nographics -v0RuntimeSmoke -logFile 'D:\__MY APPS\Unity Doom\Logs\v006-runtime-smoke.log'
```

Packaged auto-playthrough:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.6\ArcologyBreach_v0.0.6.exe' -batchmode -nographics -v0AutoPlaythrough -logFile 'D:\__MY APPS\Unity Doom\Logs\v006-auto-playthrough.log'
```

Packaged combat smoke:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.6\ArcologyBreach_v0.0.6.exe' -batchmode -nographics -v0CombatSmoke -logFile 'D:\__MY APPS\Unity Doom\Logs\v006-combat-smoke.log'
```

## Next Best Work

1. Manually play the Windows build.
2. Record tuning issues in `WORK_LEDGER.md`.
3. Tune movement, enemy speed, enemy damage, player health, ammo, and room layout.
4. Manually listen to the procedural audio cues and tune levels/tones.
5. Improve mechanical enemy navigation and obstacle handling.
6. Use `LEVEL_DESIGN_AND_MAPS.md` when changing room scale, objective flow, or future level transitions.
7. Review `ASSET_PACK_REVIEW.md` before importing local Asset Store content.

## Avoid Redoing

- Do not recreate the Unity project from scratch.
- Do not commit `Library`, `Builds`, `Logs`, `Temp`, or `UserSettings`.
- Do not generate a large art batch before manual playtesting and v0.2 combat tuning.
- Do not replace `V0SceneBuilder` without preserving its smoke/build methods.
- Keep the project identity cyberpunk and original.
- Keep systems modular enough for later Android, browser/WebGL, SteamVR/OpenXR, and Meta Quest versions.

## Current Git State Expectation

The public GitHub repo should have:

- Initial v0/v0.1 Unity source commit.
- Documentation expansion commit.
- Arcology Breach rebrand/platform planning/checkpoint commit.
- v0.0.2 procedural audio checkpoint commit.
- v0.0.3 combat/objective readability checkpoint commit.
- v0.0.4 auto-playthrough/navigation checkpoint commit.
- v0.0.5 combat automation checkpoint commit.
- v0.0.6 cyberpunk dressing checkpoint commit.

Before starting new work, run:

```powershell
git status --short --branch
git pull --ff-only
```
