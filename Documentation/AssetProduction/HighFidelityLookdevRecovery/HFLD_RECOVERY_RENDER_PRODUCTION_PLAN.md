# Revised High Fidelity Lookdev Production Plan

## Best Route

Current active focus is pressure pistol only. The earlier broad three-subject route is paused because it was too diffuse for recovery. Use `HFLD_PRESSURE_PISTOL_RECOVERY_CHECKLIST.md` and `HFLD_PRESSURE_PISTOL_ACCEPTANCE_GATES.md` for the next visual pass.

Use a Unity-only recovery process:

1. Isolated Unity editor render proof for the next visual pass.
   - Best for checking what can survive in the actual game renderer, asset pipeline, and target platform budgets.
   - The goal is to prove the image can match the north-star mood without touching gameplay scenes.

2. Unity production task breakdown after the proof passes.
   - Accepted proof elements become concrete mesh/material/VFX tasks.
   - Failed proof elements stay in the Unity lookdev lane until the gap is named and corrected.

The immediate artifact from this recovery pass is an annotated target-breakdown JPG, not a success render. A true improved proof render should come after the density and material gates are met.

## Unity Scene Path If Used

If a saved scene becomes necessary, create only this scene for Unity recovery lookdev:

`Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/Scenes/HFLD_Recovery_Lookdev.unity`

Isolation rules:

- Do not open, save, or modify `Assets/_Project/Scenes/Level01.unity` through `Level05.unity` or `MainMenu.unity`.
- Do not add the recovery scene to Build Settings.
- Do not reference gameplay scripts, generated scenes, combat prefabs, player controllers, save systems, or runtime managers.
- Use duplicated or generated assets only under `Assets/_Project/ArtStaging/HighFidelityLookdevRecovery/`.
- Prefix recovery assets with `HFLDR_` and materials with `MAT_HFLDR_`.
- Export review renders only to `Documentation/ConceptRenders/`.
- Keep previous `HighFidelityLookdev` files read-only for comparison.

## Phase A: Reference Lock

Deliverables:

- `CONTACTSHEET_HFLD_Recovery01_reference_breakdown_planning.jpg`
- Density ledger for corridor, pistol, enemy, and pressure door.
- Palette/material target board.

Exit criteria:

- Reviewer agrees the plan describes the actual north-star image.
- Batch01 failures are acknowledged without defending them.
- Next visual pass has measurable targets.

## Phase B: Density Greybox

Deliverables:

- One corridor greybox render with exact visible counts:
  12+ pipe runs/segments, 4+ lamps, 4+ gauges, 4+ valves, 5+ steam plumes, 150+ rivets/bolts on door/walls.
- One pressure pistol greybox render:
  main barrel, lower reservoir, gauge, coil with 6+ turns, glove/grip, trigger, muzzle, 60+ fasteners.
- One Scrapper greybox render:
  boiler torso, head, eyes, grille, stacks, gear, saw, claw, piston legs, heavy feet, belly gauge, 100+ fasteners.

Exit criteria:

- Greybox reads as dense and dimensional before material work.
- Camera angles match target composition.
- No flat vector/diagram-only output is accepted as the hero proof.

## Phase C: Material And Lighting Calibration

Deliverables:

- Material swatch board with aged brass, blackened iron, dark pipe metal, hot copper, wet stone, amber glass, cream gauge face, leather, soot/steam.
- Lamp and reflection test with wet floor, amber practicals, bloom, and shadow falloff.
- Notes for color temperature, roughness ranges, metalness, emission, and decal layers.

Exit criteria:

- Materials separate by roughness and specular response.
- Warm lamp cores and dark wall/floor shadows are both preserved.
- Wet floor reflections look intentional and directional.

## Phase D: First Hero Proof

Recommended first proof: pressure pistol.

Reason:

- User direction narrowed the active visual proof to one object first.
- The pistol is the most obvious first-person identity asset and provides a compact test of brass, iron, walnut/leather, glass, coil glow, fasteners, smoke, and camera framing.
- If the pistol fails in Unity, the failure is faster to diagnose than a full corridor/monster scene.

Deliverables:

- `RENDER_HFLD_Recovery04_pressure_pistol_unity_proof.jpg`
- Optional annotated copy:
  `CONTACTSHEET_HFLD_Recovery04_pressure_pistol_unity_proof.jpg`
- Render note listing Unity version, render script, generated counts, camera, light setup, and known gaps.

Exit criteria:

- Passes the global rubric and pressure-pistol density gate.
- Human reviewer says it is recognizably closer to the source concept than Batch01.
- Any fail is documented with concrete missing assets or material/lighting gaps.

## Phase E: Corridor And Enemy Proofs

Deliverables:

- `RENDER_LOOKDEV_HFLDR_Recovery05_corridor_pressure_door_unity_proof.jpg`
- `RENDER_LOOKDEV_HFLDR_Recovery04_scrapper_proof.jpg`
- Optional combined recovery contact sheet.

Exit criteria:

- Pistol and enemy pass their component/count gates.
- They share the corridor's material/lighting language.
- They look like assets from the same world when placed on one contact sheet.

## Phase F: Unity Validation

Deliverables:

- Isolated Unity scene at the exact path listed above.
- One Unity screenshot matching the offline proof camera as closely as practical.
- Notes on what changed because of Unity renderer limits.

Exit criteria:

- Unity validation remains close enough to the offline proof to justify production integration.
- Any required shader, lighting, or asset work is listed before production promotion.

## Current Honest Status

- Planning and diagnosis are underway.
- The source concept has been inspected.
- Batch01 has been diagnosed as visually failed.
- A planning/reference contact sheet has been generated.
- No actual closer high-fidelity render has been produced yet because the current available Batch01 assets are too sparse and flat to meet the concept bar honestly.
