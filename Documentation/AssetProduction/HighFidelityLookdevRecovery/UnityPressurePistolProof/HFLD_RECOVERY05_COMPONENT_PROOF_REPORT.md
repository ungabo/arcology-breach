# HFLD Recovery05 Unity Component Proof Report

Status: component-first Unity lookdev proof generated; not a full-gun promotion
Date/time: 2026-05-24 00:25:50 -04:00
Unity version: 6000.4.6f1
Batchmode command entrypoint: `UnityPressurePistolProofRenderer.RenderBatch`
Tool lane: Unity editor batchmode, temporary in-memory component scenes, Camera plus RenderTexture JPG export

## Recovery04 Disposition

Rejected full-gun Unity proof; retained as a blocker record and not promoted.
Recovery04 must not be promoted. It failed visible review for opaque smoke-paper blocks, over-cropped/side-on framing, orange material balance, and boxy component shapes.

## Outputs

| File | Purpose | Dimensions |
| --- | --- | ---: |
| `Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery05_pressure_pistol_components_unity_proof.jpg` | Component proof contact sheet | 2200x1600 |
| `Documentation/ConceptRenders/RENDER_HFLD_Recovery05_coil_unity_proof.jpg` | Coil component proof | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_HFLD_Recovery05_gauge_unity_proof.jpg` | Gauge component proof | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_HFLD_Recovery05_barrel_tank_unity_proof.jpg` | Barrel + Lower Tank component proof | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_HFLD_Recovery05_muzzle_unity_proof.jpg` | Muzzle component proof | 1600x1000 |
| `Documentation/ConceptRenders/RENDER_HFLD_Recovery05_grip_hand_unity_proof.jpg` | Grip + Hand component proof | 1600x1000 |
| `Documentation/AssetProduction/HighFidelityLookdevRecovery/UnityPressurePistolProof/unity_pressure_pistol_component_metrics.json` | Component proof metrics | n/a |

## Component Gates

| Component | Status | Evidence |
| --- | --- | --- |
| Coil | Pass | 7 separate copper turns, emissive core, dark recess, 18 brass fasteners. Separate torus-like copper loops replace the rejected flat yellow rods. Smoke omitted. |
| Gauge | Pass | 34 tick marks, red needle, glass highlight, aged brass rim, 20 rim rivets. Gauge is still primitive, but reads as layered bezel/face/glass/needle instead of a flat bright disc. |
| Barrel + Lower Tank | Pass | Separate blackened barrel/lower tank, brass collars, shadow gap, edge highlights, 16 fasteners. Main barrel and lower tank are separated by a visible shadow gap and distinct collar sets. |
| Muzzle | Pass | 6 nested steps, dark bore, brass/iron separation, left-forward depth. Nested cylinders give stepped depth and a readable dark bore; this is a better sub-assembly target for reassembly. |
| Grip + Hand | Partial | Walnut grip/leather hand distinction, readable trigger guard, lower-right first-person anchor. Material roles and trigger guard read, but the hand still needs sculpted/art-authored form before full-gun reassembly. |
| Steam/smoke | Pass by omission | Omitted in Recovery05 component proofs. No smoke/steam geometry is rendered, so no opaque paper blocks can appear. |

## Before Full-Gun Reassembly

- Reassemble only after the full-gun layout uses the passing coil, gauge, barrel/tank, and muzzle component language.
- Treat the grip/hand as partial: replace the placeholder hand with a sculpted or more anatomically assembled Unity-only form before a new hero proof.
- Keep smoke disabled until transparent radial sprites are visually verified in isolation against the dark background.
- Use a pulled-back 3/4 camera and measured occupancy target of 60-75% width and 45-65% height for the next full-gun pass.
- Preserve the darker blackened iron and restrained brass palette; avoid the rejected orange full-gun material balance.
