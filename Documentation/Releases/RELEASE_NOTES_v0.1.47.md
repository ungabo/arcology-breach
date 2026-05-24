# Brassworks Breach v0.1.47 Release Notes

Date: 2026-05-24

## Overview

`v0.1.47` is a larger visual-content import build for the Windows proof-of-concept. It imports Room Setpiece Kit 04 and Weapon Mechanisms Set 04 as local Unity packages, then places representative brassworks room modules, weapon-mechanism components, and material swatches in visual-only quarantine showcases across all five gameplay levels.

## What Changed

- Raised sidecar import validation from `packages=15 assets=123` to `packages=17 assets=150`.
- Added room-scale steampunk setpiece candidates: boiler chamber wall bays, pressure vault alcoves, catwalk balcony modules, pipe-gallery ceiling clusters, and regulator core machinery.
- Added detailed weapon-mechanism candidates: pressure pistol coils, gauge clusters, walnut/leather grip assembly, receiver lattice plates, ammo cylinders, scattergun chambers, bolt-thrower rails, muzzle crowns, pressure tanks, and gloved grip silhouettes.
- Expanded level showcase swatches with Room Setpiece Kit 04 and Weapon Mechanisms Set 04 materials including aged brass, wet stone, furnace glow, warning paint, blued steel, polished brass, verdigris, glove leather, smoked brass, and teal pressure glow.
- Rebuilt all generated gameplay scenes so the new package candidates are visible in-level without granting gameplay authority.

## Verification

- Unity route audit passed for `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.47.md`.
- Sidecar import validator passed: `SIDECAR_QUARANTINE_IMPORT_PASS packages=17 assets=150`.
- Full automated runtime suite passed through Windows build, runtime smoke, auto playthrough, combat, interaction, hazard, secret, pause, movement, balance, level flow, audio, display, readability, gameplay feedback, and world-label checks.
- Windows executable created at `Builds/Windows/v0.1.47/BrassworksBreach_v0.1.47.exe`.
- Windows package created at `Builds/WindowsPackages/v0.1.47/BrassworksBreach_v0.1.47_Windows.zip`.
- Executable SHA-256: `65D646C9285BF2CBBAB784992E3AD5AE9012BEF5E8A6B4FFA46209592AF9DDA2`.
- Package ZIP SHA-256: `547B8742B0122FF3C2065A1FC817695D543AB025E2538863134076947343A663`.

## Notes

The imported assets remain quarantined as visual-review content. They do not add gameplay authority, colliders, rigidbodies, autonomous audio, AI, pickups, damage, hit volumes, objective state, or interactable behavior.
