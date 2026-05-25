# Brassworks Breach - Reference Replica Acceptance Checklist

Last updated: `2026-05-25 00:03 -04:00`

Use this checklist before any concept-art replica is promoted from quarantine into the playable scaffold.

## Required Evidence

- Original crop named in the QA document.
- Unity-rendered beauty PNG at 1920x1080 or higher.
- Unity-rendered detail/breakdown sheet.
- Unity-rendered material closeup or grazing-light sheet.
- Metadata/catalog JSON.
- Prefab/asset file list.
- Honest pass/fail verdict with remaining gaps.

## Scoring

Score each category from `0` to `5`.

- `0`: absent.
- `1`: present but wrong.
- `2`: recognizable blockout.
- `3`: useful stylized candidate.
- `4`: strong near-final visual match.
- `5`: final-art candidate.

Promotion requires no category below `4`, and the average score must be at least `4.25`.

## Categories

### Silhouette Match

- Object reads like the crop at thumbnail size.
- Major proportions match the crop.
- Negative spaces, rings, teeth, bends, or large openings are recognizable.
- Orientation and pose support the same visual read.

### Geometry Detail

- No raw primitive look.
- Bevels, collars, seams, spokes, flanges, fasteners, dents, or edge variation are modeled where visible.
- Small details are readable but not noisy.
- Repetition is broken with scale, rotation, wear, or asymmetric detail.

### Material Realism

- Color avoids saturated toy orange/copper.
- Brass/copper/iron read as aged, tarnished, rough, and used.
- Crevices are darker than exposed edges.
- Roughness/smoothness variation supports warm highlights without mirror-flat plastic.
- Grime, soot, oil, wetness, or chipped wear appears caused by the object design.

### Lighting And Render

- Render was produced in Unity.
- Warm highlights resemble the reference without washing out color.
- Background and exposure support the object silhouette.
- Reflections and bloom are controlled.
- The object still reads without relying on text labels.

### Modularity And Game Use

- Pivot/origin, scale, and composition are appropriate for later game placement.
- Visual-only prefab has no unintended gameplay authority.
- Reusable modules are separated when the object family should repeat in levels.
- The asset can replace or dress a real gameplay object without blocking route readability.

### Integration Readiness

- No missing materials, textures, or meshes.
- No magenta shader failures.
- No colliders, rigidbodies, audio, cameras, lights, or gameplay scripts in saved visual-only prefabs unless explicitly intended and quarantined.
- Package path, render path, and metadata path are documented.
- Rollback path is clear.

## Automatic Rejection Triggers

- Looks like unmodified cylinders, cubes, planes, or flat swatches.
- Main color impression is bright orange/copper rather than aged brass/copper/iron.
- Beauty render is only a lineup/contact sheet and not a reference-style shot.
- QA compares against a generic idea instead of the specific crop.
- Worker claims AAA/final quality while scoring below the promotion threshold.
- Asset changes main gameplay, scenes, package manifest, or shared docs without an explicit main-lane promotion task.

Next-step directive: apply this checklist to the gear key and pipe bundle before any playable scaffold promotion.
