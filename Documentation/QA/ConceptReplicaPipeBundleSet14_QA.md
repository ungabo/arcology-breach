# Concept Replica Pipe Bundle Set14 QA

Checkpoint: 2026-05-25 shutdown partial.

## Validation Status

- Unity generation: NOT RUN
- Root renders: NOT AVAILABLE
- Manifest/catalog JSON: NOT REGENERATED
- Visual-only prefab scan: NOT RUN
- Render folder expected: `Documentation/ConceptRenders/V0_1_58_ConceptReplicaPipeBundleSet14/`

Important: any `CRPB14_RENDER_*.png` files currently under `AssetPacks/BrassworksBreach.ConceptReplicaPipeBundleSet14/Documentation~/Previews/` are copied Set13-derived checkpoint files and are not accepted as final Set14 proof renders.

## Set13 Comparison

Set13 failed exact concept parity because the crop composition was only approximate, collars/flanges still felt procedural, masonry read as broad blocky rectangles, and contact/wetness lacked depth. The Set14 generator patch directly targets those failures by moving the assembly closer to the crop layout and adding denser collars, wall bleed shadows, branch contact grime, floor contact pooling, irregular brick depth, and wet highlight chips.

## Reference Checklist Scores

Scores are provisional code-review estimates only because no Set14 Unity render exists yet.

| Category | Score | Notes |
| --- | ---: | --- |
| Silhouette Match | 3/5 | Generator composition now explicitly places the left riser/elbow, low horizontal run, right paired verticals, and middle branch, but this is unverified in camera. |
| Geometry Detail | 3/5 | Added collars, flange lips, bolts, grime bands, masonry chips, and contact plates; still procedural and not render-verified. |
| Material Realism | 3/5 | Reuses Set13 procedural material system with more contact grime placement; authored masks are still missing. |
| Lighting And Render | 0/5 | No Set14 Unity render produced. |
| Modularity And Game Use | 3/5 | Sidecar visual-only package pattern is preserved, but prefab scan has not rerun. |
| Integration Readiness | 2/5 | Owned package exists, but generated assets/catalog need regeneration to clear copied Set13 residue. |

Average: 2.33/5.

Verdict: FAIL / PARTIAL CHECKPOINT. Not promotable and not final proof.

## Exact Next Fix

Run the Set14 generator in an isolated Unity validation project, regenerate the root PNGs/catalog/prefabs, then inspect the beauty render for crop framing before doing any additional iteration.
