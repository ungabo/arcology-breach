# Brassworks Breach v0.1.45 Release Notes

Date: 2026-05-24

## Overview

`v0.1.45` is a larger sidecar visual-import build for the Windows proof-of-concept. It imports Objective Props Set 02, Steam VFX Set 02, Level Atmosphere Set 03, and Enemy Animation Proxy Set 01 as local Unity packages, then places representative assets in visual-only quarantine showcases across all five generated gameplay levels.

## What Changed

- Raised sidecar import validation from `packages=11 assets=81` to `packages=15 assets=123`.
- Added objective-lock, valve-panel, lift-call, actuator, and final override prop candidates to the in-level showcase path.
- Added steam, pressure-leak, furnace-blast, spark, and boss-phase VFX candidates as presentation-only Unity prefabs.
- Added dense steampunk atmosphere candidates: caged pressure lamps, wall-leaking pipes, hanging chains, overhead pipe canopies, and corridor ambience clusters.
- Added static enemy animation proxy poses for scrapper, lancer, bulwark, and warden readability review.
- Expanded material swatches for the newly imported packages so brass, enamel, steam, glow, chain, copper, and warning materials can be reviewed in the generated levels.

## Verification

- Unity route audit passed for `Documentation/QA/RouteAudit/ROUTE_AUDIT_v0.1.45.md`.
- Full automated runtime suite passed through Windows build, runtime smoke, auto playthrough, combat, interaction, hazard, secret, pause, movement, balance, level flow, audio, display, readability, gameplay feedback, and world-label checks.
- Windows package created at `Builds/WindowsPackages/v0.1.45/BrassworksBreach_v0.1.45_Windows.zip`.
- Package SHA-256: `6A2F26B835636784965BB791F70B0666DF8A35E54484B1FA107370FB319D859A`.

## Notes

The imported assets remain quarantined as visual-review content. They do not add gameplay authority, colliders, rigidbodies, autonomous audio, AI, pickups, damage, or interactable state.
