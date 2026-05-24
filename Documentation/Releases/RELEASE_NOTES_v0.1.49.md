# Brassworks Breach v0.1.49 Release Notes

Date: 2026-05-24

## Overview

`v0.1.49` is the first route-shell collision promotion pilot for the Windows proof-of-concept. It keeps imported sidecar assets visually quarantined, but proves a safer promotion pattern in Level02 Pipeworks Annex: sidecar prefabs provide visual dressing while main-scene-owned proxy objects provide any route-adjacent collision authority.

## What Changed

- Added the `V0149RouteShellPromotionPrototype` metadata component to describe build version, source packages, visual authority, collision authority, gameplay authority, and expected object counts.
- Added a Level02 route-shell pilot using corridor, room, and valve dressing from existing sidecar packages.
- Stripped presentation physics from the sidecar visual instances before placement.
- Added four named main-scene collision proxy objects for the promoted route-shell footprint.
- Added readability plates, floor sightline marks, oil-wear footprinting, and a `ROUTE SHELL` world label to make the pilot easy to inspect.
- Extended level validation to enforce the promotion boundary: sidecar visuals remain visual-only, collision proxies are scene-owned, and no gameplay/interactable/AI/audio authority is granted.

## Verification

- Unity route audit passed for `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.49.md`.
- Sidecar import validator passed: `SIDECAR_QUARANTINE_IMPORT_PASS packages=17 assets=150`.
- Full automated runtime suite passed through Windows build, runtime smoke, auto playthrough, combat, interaction, hazard, secret, pause, movement, balance, level flow, audio, display, readability, gameplay feedback, and world-label checks.
- Windows executable created at `Builds/Windows/v0.1.49/BrassworksBreach_v0.1.49.exe`.
- Windows package created at `Builds/WindowsPackages/v0.1.49/BrassworksBreach_v0.1.49_Windows.zip`.
- Executable SHA-256: `65D646C9285BF2CBBAB784992E3AD5AE9012BEF5E8A6B4FFA46209592AF9DDA2`.
- Package ZIP SHA-256: `8ED3E51AD5C43ED4D1D1CF22DB8F4413014EF8539CBF0A9D862E53F3AA504586`.

## Notes

The pilot does not promote sidecar gameplay authority. It exists to prove a pattern for using sidecar art as dressing while gameplay, collision, interaction, AI, damage, pickups, and audio remain owned by explicit main-scene objects and validator gates.
