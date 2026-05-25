# Concept Replica Gear Key Set13 - Implementation Plan

Generated: `2026-05-25 00:18:47 -04:00`

## Target
Recreate the north-star Gear Key crop as a focused Unity-only visual proof: round gear bow with missing/alternating teeth, inner ring and spokes, central riveted hub, long aged bronze shaft, collar ridges, lower side bit, dark studio backdrop, and warm brass glints.

## Method
- Custom Unity-generated meshes for the gear bow, rings, cylinders, collars, box/chip details, and domed rivets.
- Procedural PBR texture maps for aged brass, blackened recesses, worn edge glints, occlusion, and roughness variation.
- Render-only dark backdrop, warm upper-left key light, right rim glint, and dim fill to match the concept crop's low-key lighting.
- Visual-only prefab: no colliders, no scripts, no physics, no lights, no cameras.

## Promotion Notes
Use the prefab as a pickup visual only after a scale pass in the playable project. For final shipping art, follow with hand-authored UV refinement, extra bevel fidelity, and an in-game readability/material pass.
