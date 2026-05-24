# BBW_FinalMaterialsV1 Acceptance Report

Created: 2026-05-23T23:41:54-04:00

## Result

- Quality bar: 11 pass, 0 fail.
- Package is accepted for review/import staging only.
- No gameplay scripts, scenes, existing material packages, shared status docs, ConceptRenders, README, BUILD_STATUS, or WORK_LEDGER files were edited.

## Self-Quality Bar

| Material | Pass/Fail | Notes |
| --- | --- | --- |
| Aged Brass | pass | Reads as brass in contact sheets; suitable for trim, gauges, valves, and pressure hardware staging. |
| Blackened Riveted Iron | pass | Strong silhouette-safe rivet/seam read; needs final UV alignment before hero gate use. |
| Wet Oil-Dark Stone | pass | Oil/stone separation is clear in BaseColor and ORM; final engine lighting should tune wetness. |
| Soot Brick | pass | Brick courses and soot read cleanly at preview scale; repeat should be broken with decals in long halls. |
| Copper Pipe | pass | Copper/verdigris split is readable; pass as pipe material staging, not as a universal tileable metal. |
| Greasy Walnut | pass | Walnut color and grain read distinctly from leather; pass with UV-direction caveat. |
| Cream Enamel Gauge | pass | Pass as a gauge/enamel staging target; fail as a tileable material by design. |
| Amber Glass | pass | Pass as transparent/glass lookdev source; shader sorting and reflection behavior remain untested. |
| Leather Bellows | pass | Distinct from walnut and readable as bellows material; final mesh-specific fold spacing needed. |
| Hazard Enamel | pass | Hazard read is immediate and matches pressure-warning language; UV-specific variants still needed. |
| Scorch/Oil Decal Atlas | pass | Pass as a review/import atlas; decal shader setup and per-decal UV metadata still need integration work. |

## Approximation Notes

- Normals are generated from procedural height fields, not sculpt bakes or photogrammetry.
- ORM is plausible for preview PBR response but still needs Unity lighting/shader validation.
- Cream Enamel Gauge is intentionally non-tileable and should become a proper gauge-face atlas in final integration.
- Amber Glass uses BaseColor alpha for prototype opacity; transparent sorting, refraction, and reflection are not validated here.
- Scorch/Oil Decal Atlas uses BaseColor alpha plus a separate alpha helper; decal material/shader setup is future integration work.
- Copper Pipe, Greasy Walnut, and Leather Bellows are directional materials and should be UV-aligned during import.

## Acceptance Checks

- [x] Eleven steampunk material families staged.
- [x] Each material has BaseColor, Normal, and ORM maps at 2048x2048.
- [x] Decal atlas includes a separate 2048x2048 Alpha helper.
- [x] Contact sheets exist in the documentation preview folder.
- [x] Human-readable and machine-readable manifests exist.
- [x] Files were written only under the requested FinalMaterialsV1 asset/doc folders.

## Import Notes

- Import BaseColor as sRGB.
- Import Normal as Normal Map.
- Import ORM as linear/non-sRGB. This package uses R=AO, G=roughness, B=metallic.
- Do not create Unity `.mat` assets from these automatically; review the contact sheets first.
