# Brassworks Breach v0.1.51 Release Notes

Date: 2026-05-24

## Overview

`v0.1.51` is a playable route-expansion build for the Windows proof-of-concept. It adds named Level02-Level04 route modules for the pressure bypass, foundry gantry, and observatory pumpworks while keeping sidecar packages quarantined from gameplay authority.

## What Changed

- Added `ROUTE_L02_PressureBypass_v0_1_50` to Level02 with required `GEO`, `COL`, `TRG`, `AUTH`, `SPAWN`, and `VISUALONLY_L02_PressureBypass` containers.
- Added `ROUTE_L03_FoundryGantry_v0_1_50` to Level03 with foundry gantry geometry, hazards, pickups, enemies, authority props, and visual-only dressing.
- Added `ROUTE_L04_ObservatoryPumpworks_v0_1_50` to Level04 with pumpworks route geometry, hazards, pickups, enemies, pressure-key prop, regulator props, and visual-only dressing.
- Extended level validation to enforce the v0.1.51 route roots, required scene object names, child containers, and visual-only isolation.
- Tightened the ranged-combat runtime smoke test so it targets the canonical `Enemy - Pipeworks Lancer` instead of relying on Unity object discovery order after optional route enemies are added.
- Recorded Set07 sidecar review evidence and committed the Unity-rendered Hero Room Render Set 07 corridor concepts as review art. Raw Set07 package roots and the procedural/non-Unity weapon assembly proof remain local quarantine artifacts, not final promoted gameplay assets.

## Verification

- Unity route audit passed for `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.51.md`.
- Level validation passed with the new Level02-Level04 route expansion checks.
- Sidecar quarantine import validator passed: `SIDECAR_QUARANTINE_IMPORT_PASS packages=17 assets=150`.
- Full automated Windows runtime suite passed through scene rebuild, editor smoke, Windows build, runtime smoke, auto playthrough, combat, ranged combat, Bellows Node, Bulwark, Warden, interaction, hazards, secrets, pause flow, movement, balance, level flow, audio, display, readability, gameplay feedback, and world-label readability.
- Windows executable created at `Builds/Windows/v0.1.51/BrassworksBreach_v0.1.51.exe`.
- Windows package created at `Builds/WindowsPackages/v0.1.51/BrassworksBreach_v0.1.51_Windows.zip`.
- Executable SHA-256: `65D646C9285BF2CBBAB784992E3AD5AE9012BEF5E8A6B4FFA46209592AF9DDA2`.
- Package ZIP SHA-256: `95AFF4E508ED8324EE159F4661CD844EAAC686B966A6355A916DA1D1B419E3F8`.

## Notes

The new route modules are intentionally conservative: they add playable content, encounter objects, hazards, rewards, and readability dressing while leaving the original critical path intact. The current visual sidecar renders show useful direction, but the art pass still needs stronger material/shader work, grime, rivets, wet stone, and believable brass/iron surface response before it should be considered final.
