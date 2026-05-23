# Brassworks Breach

Current state: playable `v0.0.37` proof of concept with automated Windows build/test matrix.

Public repository:

`https://github.com/ungabo/arcology-breach`

Note: the GitHub repo name still reflects the previous placeholder. The active game title, Unity product name, and executable stem are now `Brassworks Breach` / `BrassworksBreach`.

This Unity project contains a simple first-person steampunk dungeon crawler/shooter for Windows. It is intentionally compact: primitive geometry, procedural steamworks dressing, brass HUD, hitscan pressure-pistol shooting, procedural audio cues, mechanical melee/ranged enemies, a gear key, a pressure gate, and a service lift into a second level.

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

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.37\BrassworksBreach_v0.0.37.exe`

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
- Packaged combat-edge smoke test.
- Packaged combat-scenario smoke test.
- Packaged ranged-combat smoke test.
- Packaged interaction smoke test.
- Packaged pause-flow smoke test.
- One-command build matrix runner.

Smoke pass markers:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_COMBAT_EDGE_PASS`
- `V0_COMBAT_SCENARIO_PASS`
- `V0_RANGED_COMBAT_PASS`
- `V0_INTERACTION_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_BUILD_MATRIX_PASS`

## Developer Commands

Run the full current V0 Windows matrix:

```powershell
powershell -ExecutionPolicy Bypass -File Tools\RunV0BuildMatrix.ps1
```

The runner rebuilds generated scenes, validates the levels, runs editor smoke, builds the Windows player, launches every packaged smoke test, and checks each pass marker.

Individual Unity entry points remain available:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v032-scene.log'
```

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.4.6f1\Editor\Unity.exe' -batchmode -projectPath 'D:\__MY APPS\Unity Doom' -executeMethod V0SceneBuilder.BuildWindowsV0 -quit -logFile 'D:\__MY APPS\Unity Doom\Logs\v032-windows-build.log'
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

## What v0.0.9 Adds

- Brass instrument-panel HUD backplates.
- Health and ammo fill gauges.
- Gear-key status lamp.

## What v0.0.10 Adds

- Primitive clockwork Scrapper silhouette.
- Boiler torso, brass chest plate, furnace eye, pressure tank, piston arms, cutter blades, and blocky feet.

## What v0.0.11 Adds

- Spark-burst impact feedback replacing the yellow hit-marker sphere.

## What v0.0.32 Adds

- One-command V0 Windows build matrix runner.
- Automatic version detection from `GameBranding.BuildVersion`.
- Full pass-marker validation for scene rebuild, level validation, editor smoke, Windows build, runtime smoke, auto-playthrough, combat, combat-edge, ranged combat, and pause flow.

## What v0.0.33 Adds

- First-person `E` interaction scanner.
- HUD interaction prompt.
- Interactable pressure gate, service lift, and final lift hooks.
- Packaged interaction smoke test in the full build matrix.

## What v0.0.34 Adds

- Data-driven pickup definitions for health, ammo, and gear key.
- Definition-driven pickup messages, audio cues, and collect tuning.
- Pickup definition validation in the full build matrix.

## What v0.0.35 Adds

- Scene-local level transition controller.
- Service-lift and restart routing through the controller.
- Runtime smoke and level validation coverage for transition routing.

## What v0.0.36 Adds

- Platform quality profile assets for Windows, Android, WebGL, PC VR, and Meta Quest.
- Windows runtime profile now applies from a data asset.
- Runtime smoke and validation coverage for the active Windows quality profile.

## What v0.0.37 Adds

- Combat scenario smoke test for cooldown rejection, ammo accounting, and expected kill timing.
- New `V0_COMBAT_SCENARIO_PASS` marker in the full build matrix.

## Good Next Steps

- Continue replacing procedural placeholder geometry with steampunk assets from the asset catalog.
- Add platform asset-quality settings.
- Add reusable valve/switch objective mechanics on top of the interaction system.
- Continue Level01 combat/readability tuning.

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
