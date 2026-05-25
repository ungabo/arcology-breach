# Concept Replica Set13 Failure Review

Generated: `2026-05-25`

Scope: Gear Key Set13 and Pipe Bundle Set13 only. This is a Unity-only review/planning document. No assets, scenes, packages, source files, or generated prefab files were changed.

Reference gate: `Documentation/QA/REFERENCE_REPLICA_ACCEPTANCE_CHECKLIST.md`

Reviewed evidence:
- `Documentation/QA/ConceptReplicaGearKeySet13_QA.md`
- `Documentation/QA/ConceptReplicaPipeBundleSet13_QA.md`
- `Documentation/QA/FinalPbrMaterialProofSet13_QA.md`
- `Documentation/ConceptRenders/V0_1_57_ConceptReplicaGearKeySet13/`
- `Documentation/ConceptRenders/V0_1_57_ConceptReplicaPipeBundleSet13/`

## Blunt Verdict

Set13 fails the crop-specific gate. Both objects are recognizable Unity proofs, but neither is an exact concept-art replica. The rendered outputs prove that the pipeline can create quarantined visual candidates; they do not prove crop-faithful final art.

The current playable scaffold remains paused until at least one replica passes the checklist with no category below `4` and an average score of at least `4.25`.

## Why Gear Key Fails

Automatic rejection applies: the render claims an exact reference-style beauty shot, but the visual comparison is still generic gear-key interpretation rather than crop-locked replica.

Specific crop-gate failures:
- Silhouette: the bow is too bulky, radial, and gear-like; the crop read needs a more exact ring diameter, tooth spacing, missing-zone rhythm, spoke width, and negative-space pattern.
- Proportion: the shaft is too heavy relative to the bow. The collars and bit blocks read oversized and procedural instead of crop-authored.
- Tooth language: the gear teeth look like repeated extruded chunks. The crop-specific tooth wear, broken intervals, and uneven profile are not matched.
- Negative space: the open ring and interior gaps do not carry enough of the reference identity at thumbnail size.
- Surface: scratches and pits exist, but they read as generated noise laid across forms instead of authored antique brass wear tied to exposed edges, contact zones, and crevices.
- Color: warm highlights still push orange/copper. The acceptance checklist explicitly rejects bright orange/copper as the main impression.
- Render composition: the beauty render includes label text and a broad presentation layout. The object should read without labels and should match the crop's compact painterly framing.
- Readiness: the self-QA says final-art readiness is `3.5/10`; this maps well below the checklist promotion threshold.

Set13 Gear Key may be kept as a pipeline proof or placeholder reference, but it must not be promoted as canonical playable pickup art.

## Why Pipe Bundle Fails

Automatic rejection is close enough to treat as failed: major forms still read as primitive cylinders, rectangular masonry blocks, and procedural collars rather than authored crop geometry.

Specific crop-gate failures:
- Silhouette/composition: the left elbow/riser, low run, right verticals, wall branch, and masonry backing are present, but the crop-specific spacing, vertical offsets, pipe thicknesses, and elbow/branch proportions are not locked.
- Geometry: the pipes remain smooth cylinders with simple rings. Collars, flanges, brackets, bolts, cut ends, and bevels need authored shape language, not repeated procedural bands.
- Masonry: the wall reads as rectangular block geometry with flat depth changes. The crop needs wet, irregular brick with broken edges, recessed mortar, darker cavities, and contact staining around pipe anchors.
- Material: copper/brass is darker than earlier attempts but still lacks edge-specific oxidation, wet specular breakup, grime accumulation, and convincing metal depth.
- Lighting: the beauty render is too flat and under-controlled for the crop. It lacks the reference's stronger dark contrast, bloom/specular accents, and wet gaslit depth.
- Contact integration: pipes, brackets, and masonry do not yet feel embedded into one industrial wall assembly. Contact shadows and stains are too weak or too procedural.
- Readiness: the self-QA says final-art readiness is `4/10`; this is a modular proof, not a checklist pass.

Set13 Pipe Bundle may inform modular layout testing, but it must not be promoted as final crop-replica wall dressing.

## Material Support Finding

`FinalPbrMaterialProofSet13_QA.md` is useful but not promotion evidence. It explicitly scores both objects at `2.33` average and labels the package material-support only. Use the material direction for pass B/C calibration, but do not let improved material sheets hide silhouette or geometry misses.

## Stop/Go Rule For Set14

Promote Set14 only if all of the following are true:
- Each candidate names the exact original crop and renders a crop-matched beauty PNG, detail/breakdown sheet, and material closeup from Unity.
- No acceptance category scores below `4`.
- Average score is at least `4.25`.
- The beauty render reads as the target crop at thumbnail size without labels.
- No automatic rejection trigger is present: no primitive look, no toy orange/copper main color, no contact-sheet-only beauty, no generic-reference comparison, and no main-lane scene/package/source changes.

Revise Set14 if the crop silhouette is recognizable but one or two categories score below `4`, or the average falls between `3.75` and `4.24`. Revision must be limited to quarantined visual assets and docs until the gate passes.

Abandon Set14 and restart the replica pass if the silhouette is still generic, if material improvements are masking wrong geometry, if primitive shapes dominate, or if any worker tries to promote below threshold.

Until a Gear Key or Pipe Bundle replica passes this rule, keep the playable scaffold paused.
