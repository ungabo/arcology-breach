# Brassworks Breach

Current state: playable `v0.0.8` proof of concept.

Public repository:

`https://github.com/ungabo/arcology-breach`

Note: the GitHub repo name still reflects the previous placeholder. The active game title, Unity product name, and executable stem are now `Brassworks Breach` / `BrassworksBreach`.

This Unity project contains a simple first-person steampunk dungeon crawler/shooter for Windows. It is intentionally compact: primitive geometry, procedural steamworks dressing, text HUD, basic hitscan shooting, procedural audio cues, mechanical melee enemies, a gear key, a pressure gate, and a service lift.

Long-term direction: an original heavily stylized steampunk action game set inside a sealed brassworks where pressure systems and clockwork machines have become hostile.

## North Star

Primary concept sheet:

`Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`

Supporting concept sheet:

`Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`

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

1. Find the gear key.
2. Return to the pressure gate.
3. Let the gate open.
4. Reach the service lift.

## Build

Windows build output:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.8\BrassworksBreach_v0.0.8.exe`

Versioned builds use incrementing folders/names such as `v0.0.1`, `v0.0.2`, and so on when meaningful progress is ready to try.

## Verification

The project test matrix includes:

- Unity project creation/import.
- Scene generation.
- Editor smoke test.
- Windows build.
- Packaged runtime smoke test.
- Packaged auto-playthrough objective-chain test.
- Packaged combat smoke test.
- Packaged pause-flow smoke test.

Smoke pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

## Developer Commands

Rebuild the generated scene:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\build-v008-scene.log'
```

Run editor smoke test:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.RunSmokeTest -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v008-smoke-test.log'
```

Build Windows player:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildWindowsV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v008-windows-build.log'
```

Run packaged runtime smoke:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.8\BrassworksBreach_v0.0.8.exe' -batchmode -nographics -v0RuntimeSmoke -logFile 'D:\__MY APPS\Unity Doom\Logs\v008-runtime-smoke.log'
```

Run packaged auto-playthrough:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.8\BrassworksBreach_v0.0.8.exe' -batchmode -nographics -v0AutoPlaythrough -logFile 'D:\__MY APPS\Unity Doom\Logs\v008-auto-playthrough.log'
```

Run packaged combat smoke:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.8\BrassworksBreach_v0.0.8.exe' -batchmode -nographics -v0CombatSmoke -logFile 'D:\__MY APPS\Unity Doom\Logs\v008-combat-smoke.log'
```

Run packaged pause flow:

```powershell
& 'D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.8\BrassworksBreach_v0.0.8.exe' -batchmode -nographics -v0PauseFlow -logFile 'D:\__MY APPS\Unity Doom\Logs\v008-pause-flow.log'
```

## What v0.0.7 Adds

- Steampunk north-star concept art imported into repo.
- Working title and Unity product metadata changed to `Brassworks Breach`.
- Scene language shifted to gear key, pressure gate, service lift, pressure pistol, and steamworks dressing.
- Pause menu with resume, restart, and quit buttons.
- Packaged pause-flow automation.

## What v0.0.8 Adds

- Gear-shaped key pickup with teeth, hub, shaft, and bit.
- Pressure-gauge details on the pistol, pressure gate, and service lift.
- Valve wheels, steam vents, and a coal furnace dressing pass.
- Additional brass, oil-stone, iron, steam, gauge, and furnace materials.

## Good Next Steps

- Continue replacing procedural placeholder geometry with steampunk assets from the asset catalog.
- Add first brass gauge HUD pass.
- Add first Scrapper visual pass.
- Add first Pressure Pistol visual pass.

## Planning Docs

- `Documentation/STEAMPUNK_NORTH_STAR.md`
- `Documentation/AAA_VISION_AND_ROADMAP.md`
- `Documentation/AAA_ASSET_CATALOG.md`
- `Documentation/STORY_AND_LORE_BIBLE.md`
- `Documentation/LEVEL_DESIGN_AND_MAPS.md`
- `Documentation/PRODUCTION_TRACKING_METHOD.md`
- `Documentation/WORK_LEDGER.md`
- `Documentation/ASSET_PACK_REVIEW.md`
- `Documentation/TITLE_AND_BRANDING_TRACKER.md`
- `Documentation/PLATFORM_WINDOWS_TARGET.md`
- `Documentation/PLATFORM_ANDROID_PORT_NOTES.md`
- `Documentation/PLATFORM_WEB_BROWSER_PORT_NOTES.md`
- `Documentation/PLATFORM_VR_PORT_NOTES.md`
