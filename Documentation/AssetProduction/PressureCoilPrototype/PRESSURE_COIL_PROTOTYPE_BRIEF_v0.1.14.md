# Pressure Coil Prototype Brief v0.1.14

Status: promoted gameplay prototype  
Date: 2026-05-24  
Owner: main Unity integration lane  
Promoted component: `PressureCoilPrototype`

## Purpose

Promote one pressure-pistol component from the Unity-only component-first lookdev direction into active gameplay without attempting a full weapon art replacement. The coil pack is meant to add a clear pressure-machine read to the first-person pistol while preserving all existing weapon animation, muzzle alignment, recoil, and route/combat tests.

## In-Game Placement

- Scene-generated object: `Pressure Pistol Prototype Copper Coil Pack`.
- Parent: `Pressure Pistol Viewmodel`.
- Placement role: `viewmodel`.
- Promotion marker: `PressureCoilPrototype`.
- Promotion version: `v0.1.14`.

## Required Hierarchy

- `Blackened Iron Backing Plate`
- `Aged Brass Upper Rail`
- `Aged Brass Lower Rail`
- `Dull Red Ceramic Heat Core`
- `Upper Copper Manifold`
- `Lower Copper Manifold`
- `Coil Turn Root`
- `Rivet Root`
- `Pressure Lead Root`

## Visual Intent

- Blackened iron backplate keeps the component visually seated in the weapon instead of floating as bright decoration.
- Aged brass rails tie it into the gauge and receiver material language.
- Copper manifold/coil forms provide the "steam pressure machine" read from the north-star pressure pistol sheet.
- Red ceramic heat core gives a readable powered state without becoming a neon sci-fi emitter.
- Rivets, pressure leads, and patina marks add believable steampunk layering while remaining cheap primitive geometry.

## Validation Gates

- `PressureCoilPrototype.promotionVersion == "v0.1.14"`.
- `placementRole == "viewmodel"`.
- Required renderer references are populated.
- `coilTurnRoot` has at least 18 generated detail children.
- `rivetRoot` has at least 18 generated detail children.
- `pressureLeadRoot` has at least 4 generated detail children.
- Backing plate material name contains `Iron`.
- Upper/lower rail material names contain `Brass`.
- Heat core material name contains `PressureWarning`.
- Full `V0LevelValidator` pass is required before release.

## Current Verification

- `V0_ROUTE_AUDIT_PASS`
- `V0_LEVEL_VALIDATION_PASS`
- `V0_BUILD_MATRIX_PASS`
- Build: `Builds/Windows/v0.1.14/BrassworksBreach_v0.1.14.exe`
- QA packet: `Documentation/QA/WindowsRouteQA/QA_PACKET_v0.1.14.md`

## Limits

This is a promoted prototype component, not final AAA viewmodel art. It does not include authored bevel meshes, UV unwraps, custom normal maps, animation-specific pivots beyond the existing viewmodel root, or final first-person hand/grip treatment. It is intentionally narrow so the next art slice can build on a verified component language.

Next-step directive: continue immediately with the next highest-impact unfinished task.
