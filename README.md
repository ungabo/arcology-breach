# Unity Doom Experiment

Current state: playable v0/v0.1 proof of concept.

Public repository:

`https://github.com/ungabo/unity-doom-experiment`

This Unity project contains a simple first-person greybox dungeon crawler/shooter for Windows. It is intentionally minimal: primitive geometry, plain materials, text HUD, basic hitscan shooting, melee enemies, a key, a locked door, and an exit.

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

1. Find the key.
2. Return to the red locked door.
3. Let the door open.
4. Reach the green exit.

## Build

Windows build output:

`D:\__MY APPS\Unity Doom\Builds\Windows\IronChapelV0.exe`

## Verification

The project has passed:

- Unity project creation/import.
- Scene generation.
- Editor smoke test.
- Windows build.
- Packaged runtime smoke test.

Useful logs:

- `Logs\build-v01-scene.log`
- `Logs\v01-smoke-test.log`
- `Logs\v01-windows-build.log`
- `Logs\v01-runtime-smoke.log`

Smoke pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`

## Developer Commands

Rebuild the generated scene:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\build-v01-scene.log'
```

Run editor smoke test:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.RunSmokeTest -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v01-smoke-test.log'
```

Build Windows player:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildWindowsV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v01-windows-build.log'
```

Run packaged runtime smoke:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\IronChapelV0.exe' -batchmode -nographics -v0RuntimeSmoke -logFile 'D:\__MY APPS\Unity Doom\Logs\v01-runtime-smoke.log'
```

## What v0.1 Added

After the v0.0 loop passed, a small presentation pass added:

- Blocky first-person weapon placeholder.
- Muzzle flash.
- Damage flash.
- Bobbing pickups.
- Sliding locked door.
- Colored key/door/exit lights.
- Primitive enemies with simple eye markers.

## Good Next Steps

- Manually play through the Windows build and tune movement/enemy balance.
- Add simple audio feedback.
- Add low-resolution wall/floor textures.
- Replace primitive enemy visuals with a billboard sprite.
- Replace the blocky weapon with a centered sprite.

## Planning Docs

- `Documentation/AAA_VISION_AND_ROADMAP.md`
- `Documentation/AAA_ASSET_CATALOG.md`
- `Documentation/PRODUCTION_TRACKING_METHOD.md`
- `Documentation/WORK_LEDGER.md`
- `Documentation/HANDOFF.md`
