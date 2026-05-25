# Final PBR Material Proof Set 13 QA

Generator pass marker: `FINAL_PBR_MATERIAL_PROOF_SET13_MATERIAL_SUPPORT_CHECKLIST_MACRO_2026-05-25_B_MACRO_EXPOSURE_FIX`

Reference checklist used: `Documentation/QA/REFERENCE_REPLICA_ACCEPTANCE_CHECKLIST.md`.

Original crops named for this material-support pass: gear key crop and pipe bundle crop from the current concept-replica phase. This package does not include the original crop images and does not claim promotion readiness.

Required evidence status:

| Evidence | Status |
| --- | --- |
| Unity-rendered beauty PNG at 1920x1080 or higher | Partial: object-applied proxy renders exist, but geometry is not crop-accurate final art. |
| Unity-rendered detail/breakdown sheet | Partial: application sheet exists for material read. |
| Unity-rendered material closeup/grazing-light sheet | Pass: `FPM13_macro_material_support_sheet.png`. |
| Metadata/catalog JSON | Pass: `Runtime/Metadata/FPM13_MaterialCatalog.json`. |
| Prefab/asset file list | Partial: package contains materials/textures only; no accepted visual prefab. |
| Honest verdict | Pass: material-support only, not final art. |

Checklist scoring, 0-5:

| Category | Gear key | Pipe bundle | Notes |
| --- | ---: | ---: | --- |
| Silhouette Match | 2 | 2 | Recognizable proxy direction only; not exact crop geometry. |
| Geometry Detail | 2 | 2 | Simple Unity forms with bevel/collar hints; still proxy geometry. |
| Material Realism | 3 | 3 | Better aged bronze/copper, dark recesses, grime, and controlled warm highlights; macro sheet supports material read. |
| Lighting And Render | 3 | 3 | Unity renders with warm grazing light; proxy renders remain support evidence rather than final shots. |
| Modularity And Game Use | 1 | 1 | No final prefab or gameplay-ready module is included. Visual-only material package. |
| Integration Readiness | 3 | 3 | Materials, maps, JSON, and renders are present; no main manifest promotion and no final prefab acceptance. |

Average score: gear key `2.33`, pipe bundle `2.33`. Promotion threshold is average `4.25` with no category below `4`, so this package **fails final-art acceptance by design**.

Honest verdict: **material-support pass only, not accepted final art**. It can inform the next exact-replica modeling/material pass, but it must not be promoted into the playable scaffold as a final crop replica.
