# Brassworks Breach

Current state: playable `v0.0.71` proof of concept with automated Windows build/test matrix.

Public repository:

`https://github.com/ungabo/arcology-breach`

Note: the GitHub repo name still reflects the previous placeholder. The active game title, Unity product name, and executable stem are now `Brassworks Breach` / `BrassworksBreach`.

This Unity project contains a simple first-person steampunk dungeon crawler/shooter for Windows. It is intentionally compact: primitive geometry, procedural steamworks dressing, readable lore plaques, animated gears/valves/pulleys, brass HUD with persistent objective guidance and boss health readout, hitscan pressure-pistol shooting with impact decal VFX, first-person player damage VFX, procedural audio cues, mechanical melee/ranged/heavy/boss enemies with procedural machine motion, animated pressure-bolt projectiles, animated steam hazards, animated furnace-heat hazards, three multi-level secret caches, health/ammo/key pickup VFX, a pressure gate with opening VFX, service-lift transitions with activation VFX, a Pipeworks routing valve, a Boilerheart pressure valve that unlocks the foundry lift, a Furnace Foundry route, machine hit/shutdown VFX, and a Governor Core finale with a Warden shutdown effect and master override hoist.

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
4. Ride the service lifts into the Boilerheart.
5. Vent the Boilerheart pressure valve.
6. Ride the foundry lift.
7. Cross the Furnace Foundry.
8. Ride the emergency hoist to the Governor Core.
9. Defeat the Governor Warden and engage the master override hoist.

## Build

Windows build output:

`D:\__MY APPS\Unity Doom\Builds\Windows\v0.0.71\BrassworksBreach_v0.0.71.exe`

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
- Packaged Bulwark-combat smoke test.
- Packaged Warden-combat smoke test.
- Packaged interaction smoke test.
- Packaged hazard smoke test.
- Packaged secret smoke test.
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
- `V0_BULWARK_COMBAT_PASS`
- `V0_WARDEN_COMBAT_PASS`
- `V0_INTERACTION_SMOKE_PASS`
- `V0_HAZARD_PASS`
- `V0_SECRET_PASS`
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

## What v0.0.38 Adds

- Generated Level03 Boilerheart Core scene.
- Level02 service lift now transitions into Level03.
- Auto-playthrough now validates Level01 -> Level02 -> Level03 -> win.

## What v0.0.39 Adds

- Boilerheart pressure valve objective in Level03.
- Final lift remains locked until the valve is vented.
- Auto-playthrough verifies the locked lift, valve venting, and final win.

## What v0.0.40 Adds

- Reusable steam hazard trigger system.
- Two Boilerheart steam hazard volumes with primitive vent/puff visuals.
- Packaged hazard smoke test added to the one-command matrix.

## What v0.0.41 Adds

- Scene-specific objective briefing messages.
- Validation coverage for Level01, Level02, and Level03 objective text.
- Full matrix remains green with the updated scene messaging.

## What v0.0.42 Adds

- Boilerheart pressure valve now shuts down linked steam hazards.
- Auto-playthrough verifies valve completion, lift unlock, and hazard shutdown.
- Validation ensures the valve is linked to Boilerheart hazards.

## What v0.0.43 Adds

- First reusable `SecretArea` foundation.
- Intake pressure cache with health/ammo rewards.
- Packaged secret smoke test added to the one-command matrix.

## What v0.0.44 Adds

- Persistent `RunStats` secret tracking.
- Secret smoke now verifies run-stat registration/discovery.
- Win message can report secret progress.

## What v0.0.45 Adds

- Auto-playthrough now verifies secret totals persist to the final win state.
- Full build matrix remains green with secret persistence coverage.

## What v0.0.46 Adds

- Generated Level04 Furnace Foundry scene.
- Level03 Boilerheart lift is now pressure-locked until valve venting, then transitions to Level04.
- Level04 added foundry hazards, mixed Scrapper/Lancer pressure, pickups, furnace-row dressing, and an emergency-hoist route that later became the Level05 transition.
- Full build matrix remained green with the four-level auto-playthrough at introduction.

## What v0.0.47 Adds

- Reusable pulsing `FurnaceHeatHazard`.
- Two Furnace Foundry heat-surge lanes with warning/active/safe phase visuals.
- Hazard smoke now verifies both Boilerheart steam damage and foundry furnace-heat damage.

## What v0.0.48 Adds

- First heavy `Bulwark` enemy role in the Furnace Foundry.
- Data-driven `BulwarkDefinition.asset`.
- Primitive Bulwark silhouette with riveted boiler body, furnace belly, pressure tank, piston legs, and hammer arms.
- New packaged Bulwark combat smoke in the full build matrix.

## What v0.0.49 Adds

- Second secret cache in Level04, the `Secret - Foundry Coal Cache`.
- Foundry secret reward pickups and coal-bin visuals.
- Auto-playthrough now verifies multi-level secret totals persist to the final win state.

## What v0.0.50 Adds

- Generated Level05 Governor Core scene.
- Level04 emergency hoist now transitions into Level05 instead of ending the run.
- Level05 adds mixed Scrapper/Lancer/Bulwark pressure, steam and furnace-heat hazards, Governor Core dressing, and the master override hoist final win device.
- Build order and auto-playthrough now cover MainMenu, Level01, Level02, Level03, Level04, and Level05.

## What v0.0.51 Adds

- First `Governor Warden` final guardian prototype in Level05.
- Data-driven `GovernorWardenDefinition.asset`.
- Primitive Warden silhouette with core body, furnace heart, pressure crown, back boiler, piston arms, stomp plates, and pressure cannon muzzle.
- Warden controller with durable boss health, stomp attack, pressure-bolt attack, and enraged half-health state.
- New packaged Warden combat smoke in the full build matrix.

## What v0.0.52 Adds

- `GuardianDefeatObjective` foundation.
- Level05 master override hoist now starts locked until the Governor Warden is destroyed.
- Red/green Warden lock signal props near the final hoist.
- Auto-playthrough now verifies locked final hoist rejection, Warden defeat, final hoist unlock, and win.

## What v0.0.53 Adds

- Top-center Warden boss health HUD with brass backplate, red pressure fill, and boss label.
- Governor Warden now shows, updates, and hides its boss health readout during combat.
- HUD boss-field wiring is validated in editor and packaged runtime smoke.
- Warden combat smoke now verifies the boss bar appears and drops after damage.

## What v0.0.54 Adds

- `WardenShutdownVfx` runtime effect for the Governor Warden death moment.
- Warden defeat now vents steam jets, brass sparks, and an expanding pressure ring.
- Warden defeat message changed to reinforce the pressure-machine shutdown fantasy.
- Warden combat smoke now verifies the shutdown VFX spawns with visible pieces.

## What v0.0.55 Adds

- Persistent brass objective HUD under the top-left screen edge.
- Objective updates for gear key pickup, pressure gate opening, Boilerheart valve venting, Warden defeat, death, and win.
- Editor and runtime smoke validation for objective HUD wiring.
- Auto-playthrough verifies objective text changes across the current five-level route.

## What v0.0.56 Adds

- `MachineDeathVfx` runtime effect for standard mechanical enemies.
- Scrappers, Lancers, and Bulwarks now vent steam and brass sparks on death.
- Combat smoke verifies Scrapper death VFX visible pieces.
- Bulwark combat smoke verifies scaled heavy-machine death VFX.

## What v0.0.57 Adds

- `SteamworksSpinner` reusable motion component for simple steampunk machinery loops.
- Animated pressure-gate gears, valve wheels, service-lift pulley gears, and main-menu gear.
- Level validation verifies active machinery spinners are present and configured.
- Runtime smoke now requires the machinery-motion component.

## What v0.0.58 Adds

- `MachineHitVfx` runtime effect for non-lethal mechanical enemy hits.
- Scrappers, Lancers, Bulwarks, and the Governor Warden now spark and vent steam when damaged before death.
- Combat-scenario smoke verifies Scrapper hit VFX.
- Bulwark and Warden combat smokes verify scaled hit VFX before shutdown.

## What v0.0.59 Adds

- `GateOpenVfx` runtime effect for pressure-gate unlock/open feedback.
- Pressure gate opening now emits green pressure wash, steam jets, and brass/green sparks.
- Auto-playthrough verifies the gate VFX appears during the real gear-key route.

## What v0.0.60 Adds

- `LiftActivationVfx` runtime effect for service-lift engagement.
- Level transitions now have a short pressure-engage delay before scene load.
- Auto-playthrough verifies the lift VFX appears before the Level01-to-Level02 transition.

## What v0.0.61 Adds

- `GearKeyPickupVfx` runtime effect for the first objective pickup.
- Gear-key pickup now emits a brass ring, center glow, and tooth sparks.
- Auto-playthrough verifies the key VFX appears after inventory/objective update.

## What v0.0.62 Adds

- `ResourcePickupVfx` runtime effect for health and ammo pickups.
- Health pickups now emit red medicinal pickup bursts.
- Pressure cartridge pickups now emit brass/cyan pickup bursts.
- Auto-playthrough verifies both health and ammo pickup VFX before the key route.

## What v0.0.63 Adds

- `SteamHazardVfx` animates low/high steam puffs on generated hazards.
- Steam hazard builder attaches and wires visible animated puffs.
- Level validation and hazard smoke require animated steam puffs.

## What v0.0.64 Adds

- `FurnaceHeatHazardVfx` animates warning/active/safe furnace hazard signals.
- Active furnace hazards now show animated heat-ripple pieces.
- Level validation and hazard smoke require active furnace heat VFX.

## What v0.0.65 Adds

- `MachineMotionVfx` adds reusable procedural motion for mechanical enemies.
- Scrappers, Lancers, Bulwarks, and the Governor Warden now get bobbing bodies, swinging limbs, and pressure-part pulsing.
- Level validation and runtime smoke require configured machine motion.

## What v0.0.66 Adds

- `PressureBoltVfx` adds core glow, trailing pressure puffs, and side sparks to enemy projectiles.
- Lancer and Governor Warden pressure bolts now orient along travel direction and carry the shared projectile VFX.
- Ranged combat smoke verifies visible pressure-bolt VFX before accepting projectile damage.

## What v0.0.67 Adds

- `ImpactDecalVfx` replaces the spark-only pressure-pistol hit marker.
- Pressure-pistol hits now create a short-lived scorch disc, brass impact plate, and sparks.
- Combat-scenario smoke verifies impact decal VFX on a non-lethal hit.

## What v0.0.68 Adds

- `PlayerDamageVfx` adds a first-person hurt burst on every player damage event.
- Player damage now combines HUD flash with pressure slashes, heat edges, and brass sparks.
- Combat-edge smoke verifies the player damage VFX after a Scrapper melee hit.

## What v0.0.69 Adds

- `LorePlaque` adds first-person readable archive plaques to every current gameplay level.
- Each plaque carries short Brassworks lore through the HUD while keeping in-world text compact.
- Level validation and interaction smoke now verify plaque triggers, prompts, narrative text, and HUD read feedback.

## What v0.0.70 Adds

- Level02 now has a required `Pipeworks Routing Valve Objective`.
- The Boilerheart lift starts pressure-locked until the routing valve is turned.
- Auto-playthrough now proves the Level02 lock, valve interaction, objective HUD update, and lift transition.

## What v0.0.71 Adds

- Level02 now has `Secret - Pipeworks Cartridge Cache` with health and pressure-cartridge rewards.
- Level validation requires the Pipeworks secret cache and reward prop.
- Auto-playthrough now verifies at least three registered secrets persist to the final win state.

## Good Next Steps

- Continue replacing procedural placeholder geometry with steampunk assets from the asset catalog.
- Add platform asset-quality settings.
- Add more reusable valve/switch objective mechanics on top of the interaction system.
- Continue combat/readability tuning across the current five-level route.

Next-step directive: continue immediately with the next highest-impact unfinished task.

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
