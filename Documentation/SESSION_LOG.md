# Session Log

## 2026-05-23 00:34 -04:00

User clarified that the desired style is heavily steampunk rather than the previous direction. Generated and imported two steampunk concept sheets as the visual north star:

- `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`
- `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`

Current project direction:

- Working title changed to `Brassworks Breach`.
- Unity product metadata changed to `Brassworks Breach`.
- Executable stem changed to `BrassworksBreach`.
- Repo remains `https://github.com/ungabo/arcology-breach` until a deliberate repo rename.

Implementation changes:

- Prior procedural audio system renamed/rethemed to `SteamworksAudio`.
- In-game vocabulary shifted to `Pressure Pistol`, `Gear Key`, `Pressure Gate`, and `Service Lift`.
- Generated scene dressing shifted toward brass, copper, oil stone, riveted iron, boiler stacks, and pipe runs.
- Pause menu and packaged pause-flow test completed for `v0.0.7`.

Verification completed:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

Build executable:

`Builds/Windows/v0.0.7/BrassworksBreach_v0.0.7.exe`

## Prior Verified Builds

- `v0.0.1`: basic Windows build and runtime smoke.
- `v0.0.2`: first procedural audio pass.
- `v0.0.3`: combat/objective readability.
- `v0.0.4`: packaged objective auto-playthrough and improved obstacle steering.
- `v0.0.5`: packaged combat smoke.
- `v0.0.6`: first procedural dressing pass.
- `v0.0.7`: steampunk retheme, concept-art north star, pause/quit menu, and pause-flow automation.

## 2026-05-23 00:56 -04:00

Completed `v0.0.8` brassworks prop silhouette pass.

Added:

- Gear-shaped key pickup.
- Pressure gauges on the weapon, gate, and lift.
- Valve wheels.
- Steam vents.
- Coal furnace prop.
- New simple materials for gauge faces, steam puffs, and furnace glow.

Verification completed:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

Build executable:

`Builds/Windows/v0.0.8/BrassworksBreach_v0.0.8.exe`

## 2026-05-23 01:02 -04:00

Completed `v0.0.9` brass gauge HUD pass.

Added:

- HUD backplates.
- Health fill gauge.
- Ammo fill gauge.
- Gear-key status lamp.

Verification completed:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

Build executable:

`Builds/Windows/v0.0.9/BrassworksBreach_v0.0.9.exe`

## 2026-05-23 01:06 -04:00

Completed `v0.0.10` primitive Scrapper visual pass.

Added:

- Boiler torso.
- Brass chest plate.
- Furnace eye.
- Pressure tank.
- Piston arms.
- Cutter blades.
- Blocky feet.

Verification completed:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

Build executable:

`Builds/Windows/v0.0.10/BrassworksBreach_v0.0.10.exe`

## 2026-05-23 01:09 -04:00

Completed `v0.0.11` impact spark pass.

Added:

- Replaced yellow hit-marker sphere with a short-lived hot spark cluster at raycast impact points.

Verification completed:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

Build executable:

`Builds/Windows/v0.0.11/BrassworksBreach_v0.0.11.exe`

## 2026-05-23 10:21 -04:00

Completed `v0.0.12` pickup visual pass.

Added:

- Replaced the cube health pickup with a brass-and-glass medicinal vial.
- Replaced the cube ammo pickup with a brass pressure-cartridge pack.
- Added frosted glass and red medicinal fluid materials.

Verification completed:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

Build executable:

`Builds/Windows/v0.0.12/BrassworksBreach_v0.0.12.exe`

## 2026-05-23 10:27 -04:00

Completed `v0.0.13` pressure pistol viewmodel pass.

Added:

- Replaced the blocky pressure pistol viewmodel with a primitive brass-and-walnut pneumatic sidearm.
- Added barrel cylinders, pressure tube, brass receiver, iron backplate, trigger guard, trigger, gauge, valve wheel, and side pipes.

Verification completed:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

Build executable:

`Builds/Windows/v0.0.13/BrassworksBreach_v0.0.13.exe`

## 2026-05-23 10:36 -04:00

Completed `v0.0.14` main menu flow.

Added:

- Generated `MainMenu` scene with steampunk backdrop, start button, quit button, and version label.
- Added `MainMenuController`.
- Updated build scene order so real builds start at the menu and automated test runs route into `Level01`.

Verification completed:

- `V0_SMOKE_TEST_PASS`
- `V0_WINDOWS_BUILD_PASS`
- `V0_RUNTIME_SMOKE_PASS`
- `V0_AUTO_PLAYTHROUGH_PASS`
- `V0_COMBAT_SMOKE_PASS`
- `V0_PAUSE_FLOW_PASS`

Build executable:

`Builds/Windows/v0.0.14/BrassworksBreach_v0.0.14.exe`
