# Brassworks Breach - Windows Route QA Packet v0.1.37

Generated: `2026-05-24 15:54 -04:00`

## Verified Build

- Executable: `Builds/Windows/v0.1.37/BrassworksBreach_v0.1.37.exe`
- Route audit: `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.37.md`
- Package: `Builds/WindowsPackages/v0.1.37/BrassworksBreach_v0.1.37_Windows.zip`
- Package SHA-256: `B7FA153CA7129BA59AF8D306E1A294F5159945477AF40A65A03A5501B8FB9ADF`

## Route Matrix

| Scene | Core Route Objects | Enemies | Pickups | Hazards | Secrets | Transition / Exit | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Level01 | Gate:yes Lift:yes | S:4 L:0 B:0 N:0 W:0 | H:2 A:2 K:1 W:0 | Steam:0 Furnace:0 | 1 | to Level02 open | Plaques:1 Spawn->gate 22.5m Spawn->lift 34.6m |
| Level02 | Valve:yes Lift:yes | S:1 L:1 B:0 N:0 W:0 | H:2 A:2 K:0 W:0 | Steam:0 Furnace:0 | 1 | to Level03 locked | Plaques:1 Spawn->valve 20.6m Spawn->lift 23.2m |
| Level03 | Valve:yes Lift:yes | S:2 L:0 B:0 N:1 W:0 | H:1 A:1 K:0 W:1 | Steam:2 Furnace:0 | 0 | to Level04 locked | Plaques:1 Spawn->valve 18.3m Spawn->lift 24.3m |
| Level04 | Emergency lift:yes | S:2 L:1 B:1 N:0 W:0 | H:2 A:2 K:0 W:0 | Steam:2 Furnace:2 | 1 | to Level05 open | Plaques:1 Spawn->lift 28.3m |
| Level05 | Warden:yes Guardian lock:yes Exit:yes | S:1 L:1 B:1 N:0 W:1 | H:1 A:1 K:0 W:0 | Steam:1 Furnace:1 | 0 | final exit locked | Plaques:1 Warden->exit 4.5m Spawn->Warden 24.1m |

## Manual Test Order

1. `01_FIRST_RUN_ROUTE.md` - complete the five-level route from a fresh launch.
2. `02_COMBAT_FEEL_ROUTE.md` - focus on Pressure Pistol, Steam Scattergun, machine tells, and death feedback.
3. `03_SECRET_HUNT_ROUTE.md` - confirm Level01, Level02, and Level04 secrets remain discoverable.
4. `04_HAZARD_ROUTE.md` - check steam/furnace readability and damage timing.
5. `05_BOSS_ROUTE.md` - test the Governor Warden lock, fight, boss HUD, and final hoist.
6. `06_ACCESSIBILITY_READABILITY_ROUTE.md` - check settings, contrast, prompts, objective text, and HUD readability.

## Pass Criteria

- The tester can explain the objective in each level without consulting automation logs.
- Route-critical pickups, valves, lifts, gates, boss lock, hazards, and final exit are readable from normal play distance.
- Combat failures feel attributable to player action or clear enemy tells.
- The tester can pause, restart, and quit without confusion.
- Any block over `2 minutes` is logged with location, objective text, and what visual/audio cue was missing.

## Evidence To Record

- Build path and package hash.
- Route sheet name.
- Settings used: resolution, fullscreen/windowed, sensitivity, volume, flash intensity, high contrast.
- Timing notes per level.
- Screenshots or short descriptions for confusing routes, unreadable enemies, unclear hazards, or audio balance issues.

Next-step directive: continue immediately with the next highest-impact unfinished task.
