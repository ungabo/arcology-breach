# Concept Replica Pass B/C Correction Brief

Generated: `2026-05-25`

Scope: correction plan for failed Set13 Gear Key and Pipe Bundle attempts. This is a planning document only. Do not generate assets from this brief unless a later implementation task explicitly authorizes that work.

Gate to beat: `Documentation/QA/REFERENCE_REPLICA_ACCEPTANCE_CHECKLIST.md`

Promotion target: no category below `4`; average score at least `4.25`; exact crop read without labels.

The current playable scaffold remains paused until at least one replica passes.

## Pass B Priority: Gear Key

P0 - lock the crop silhouette before material polish.
- Match the crop camera angle, diagonal pose, object scale, and image framing first.
- Rebuild the bow profile against the crop: outer diameter, inner ring diameter, tooth count/spacing, broken tooth rhythm, missing zones, spoke positions, and hub size.
- Reduce the chunky gear-bow read. The bow must look like the crop's specific worn mechanical key head, not a generic cog.
- Thin and rebalance the shaft relative to the bow. The current shaft reads too heavy and cylindrical.
- Reposition and rescale the lower side bit so its blocks match the crop's exact offset, length, thickness, and silhouette.

P1 - replace procedural-looking geometry with authored forms.
- Add bevel variation to teeth, collars, shaft edges, bit blocks, and hub rings.
- Break repetition manually: uneven tooth lengths, chipped edges, bent/slightly misaligned teeth, and asymmetric rim damage.
- Author collars as crop-specific narrow bands instead of decorative generic rings.
- Make negative spaces readable in silhouette: open bow gaps, spoke gaps, and bit gaps must survive thumbnail review.

P2 - material pass after silhouette approval.
- Use aged brass/bronze as the base read, not orange copper.
- Darken crevices and ring interiors; keep exposed edges warmer and slightly polished.
- Replace uniform scratch/pit noise with authored masks: edge wear, contact scratches, blackened seams, oxidized recesses, and handled/worn grip zones.
- Calibrate roughness so highlights are warm but not plastic, mirror-flat, or neon orange.

P3 - render evidence.
- Produce a label-free beauty PNG matching the crop's compact painterly composition.
- Produce one silhouette overlay/detail sheet that proves bow, shaft, collars, and bit proportions.
- Produce one grazing-light/material closeup that proves tarnish, roughness breakup, and blackened crevices.

Pass B exit rule for Gear Key: continue to pass C only if silhouette, geometry detail, material realism, and lighting each score at least `4`. If silhouette or bow shape is still below `4`, do not spend pass C on polish; rebuild the shape again.

## Pass B Priority: Pipe Bundle

P0 - lock the crop composition and wall assembly.
- Match the crop framing, pipe run placement, left elbow/riser position, low horizontal run height, right paired vertical pipe spacing, wall branch location, and valve/secondary detail placement.
- Set pipe diameters and offsets from the crop before adding new detail.
- Make the wall and pipes read as one attached industrial fixture, not pipes floating in front of block geometry.

P1 - rebuild primitive-looking parts.
- Replace simple cylinders with beveled, slightly irregular pipe sections with visible seams, dents, cut lips, and thickness at open ends.
- Rebuild collars/flanges as authored clamps: varied lip thickness, bolt positions, bevels, dark inner seams, and non-identical ring wear.
- Rework brackets so they visibly anchor into masonry, with believable straps, fasteners, and compression/contact shadows.
- Rebuild the left elbow and wall branch to match crop-specific bend radius and flange scale.

P2 - masonry correction.
- Replace rectangular block read with irregular wet brick: varied brick sizes, chipped corners, recessed mortar, uneven depth, dark cavities, and stains around pipe penetrations.
- Avoid a flat tiled wall impression. The wall needs sculpted relief and grime patterns that explain the pipes' placement.
- Add contact dirt, soot, dampness, and mineral staining where pipes meet wall, brackets, and floor/ledge surfaces.

P3 - material and lighting correction.
- Keep copper/brass dark, oxidized, and used. Do not let orange highlights become the main color impression.
- Add wet specular breakup, edge oxidation, blackened bands, soot in collar seams, and grime trails below joints.
- Increase contrast and controlled bloom/specular accents so the crop reads as damp, gaslit, and heavy.
- Ensure all closeups still show authored geometry, not material noise compensating for simple shapes.

P4 - render evidence.
- Produce a label-free crop-matched beauty PNG.
- Produce a detail sheet showing pipe diameters, elbow, branch, collars, brackets, bolts, and wall contact points.
- Produce a grazing-light closeup focused on wet metal, masonry relief, seams, stains, and contact shadows.

Pass B exit rule for Pipe Bundle: continue to pass C only if silhouette/composition, geometry detail, wall integration, material realism, and lighting each score at least `4`. If masonry or primitive-cylinder read remains below `4`, revise geometry before polish.

## Pass C Scope

Pass C is not a second blockout. Use it only after pass B clears the crop silhouette/geometry gate.

Allowed pass C work:
- Tighten authored wear masks and material maps.
- Tune lighting, exposure, bloom, contrast, and background for the crop render.
- Add small bevels, chips, bolt wear, contact stains, and local roughness breakup.
- Clean prefab organization and metadata evidence for final QA.

Not allowed in pass C:
- Promoting to the playable scaffold without checklist pass.
- Treating modular usefulness as a replacement for exact crop match.
- Using labels, contact sheets, or material macro renders as substitutes for a reference-style beauty render.
- Editing main scenes, package manifest, source, shared generated scenes, or asset package files.

## Set14 Promotion Decision

Promote Set14 only when the checklist passes exactly: no category below `4`, average at least `4.25`, complete required evidence, label-free crop-matched Unity beauty render, and no automatic rejection trigger.

Revise Set14 when the crop read is close but one or two fixable categories miss the threshold. The correction must stay in quarantined visual work until QA passes.

Abandon Set14 when the result is still a generic interpretation, dominated by primitive forms, primarily orange/copper, or dependent on material noise/post-processing to hide wrong geometry.
