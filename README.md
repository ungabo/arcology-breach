# Arcology Breach

Current state: playable `v0.0.3` proof of concept.

Public repository:

`https://github.com/ungabo/arcology-breach`

This Unity project contains a simple first-person greybox cyberpunk dungeon crawler/shooter for Windows. It is intentionally minimal: primitive geometry, plain materials, text HUD, basic hitscan shooting, procedural cyberpunk audio cues, mechanical melee enemies, an access shard, a corporate lockdown gate, and an emergency exit.

Long-term direction: an original heavily stylized cyberpunk action game set inside a sealed corporate arcology where autonomous security systems have turned civic machines into predatory mechanical bodies.

## How to Open

Open this folder as a Unity project:

`D:\__MY APPS\Unity Doom`

Unity version used:

`6000.4.6f1`

## Scene

Main scene:

`Assets/_Project/Scenes/Level01.unity`

## Controls

- `WASD`: Move
- `Mouse`: Look
- `Left Mouse`: Fire
- `Escape`: Pause/resume
- `R`: Restart after death or win

Goal:

1. Find the access shard.
2. Return to the red corporate lockdown gate.
3. Let the gate open.
4. Reach the green emergency lift/data gate.

## Build

Windows build output:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.3\ArcologyBreach_v0.0.3.exe`

Checkpoint builds will use incrementing folders/names such as `v0.0.1`, `v0.0.2`, and so on when meaningful progress is ready to try.

## Verification

The project has passed:

- Unity project creation/import.
- Scene generation.
- Editor smoke test.
- Windows build.
- Packaged runtime smoke test.

Useful logs:

- `Logs\build-v003-scene.log`
- `Logs\v003-smoke-test.log`
- `Logs\v003-windows-build.log`
- `Logs\v003-runtime-smoke.log`

Smoke pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`

## Developer Commands

Rebuild the generated scene:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\build-v003-scene.log'
```

Run editor smoke test:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.RunSmokeTest -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v003-smoke-test.log'
```

Build Windows player:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildWindowsV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v003-windows-build.log'
```

Run packaged runtime smoke:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.3\ArcologyBreach_v0.0.3.exe' -batchmode -nographics -v0RuntimeSmoke -logFile 'D:\__MY APPS\Unity Doom\Logs\v003-runtime-smoke.log'
```

## What v0.1 Added

After the v0.0 loop passed, a small presentation pass added:

- Blocky first-person `Pulse Pistol` placeholder.
- Muzzle flash.
- Damage flash.
- Bobbing pickups.
- Sliding corporate lockdown gate.
- Colored access-shard/gate/exit lights.
- Primitive mechanical enemies with simple lens markers.

## What v0.0.2 Added

- Procedural cyberpunk audio system.
- Pulse pistol fire and empty-click sounds.
- Health, ammo, and access-shard pickup sounds.
- Scrapper hit and shutdown sounds.
- Player hurt sound.
- Lockdown gate denied/open sounds.
- Emergency lift win cue.

## What v0.0.3 Added

- Scrapper attack windup with magenta attack tell.
- Player shots interrupt Scrapper windup.
- Access shard pedestal.
- Floor guide strips for shard route, lockdown gate, and emergency lift.
- In-world labels for access shard, lockdown gate, and emergency lift.
- Deprecated Unity object lookup calls replaced.

## Good Next Steps

- Manually play through the Windows build and tune movement/enemy balance.
- Tune cyberpunk audio levels after a manual listen pass.
- Add stylized neon wall/floor materials.
- Replace primitive enemy visuals with mechanical `Scrapper` art.
- Replace the blocky weapon with a stylized `Pulse Pistol`.

## Planning Docs

- `Documentation/AAA_VISION_AND_ROADMAP.md`
- `Documentation/AAA_ASSET_CATALOG.md`
- `Documentation/CYBERPUNK_STORY_BIBLE.md`
- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Documentation/PRODUCTION_TRACKING_METHOD.md`
- `Documentation/WORK_LEDGER.md`
- `Documentation/HANDOFF.md`
- `Documentation/ASSET_PACK_REVIEW.md`
- `Documentation/TITLE_AND_BRANDING_TRACKER.md`
- `Documentation/PLATFORM_WINDOWS_TARGET.md`
- `Documentation/PLATFORM_ANDROID_PORT_NOTES.md`
- `Documentation/PLATFORM_WEB_BROWSER_PORT_NOTES.md`
- `Documentation/PLATFORM_VR_PORT_NOTES.md`
