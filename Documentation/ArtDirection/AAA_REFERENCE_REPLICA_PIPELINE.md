# Brassworks Breach - AAA Reference Replica Pipeline

Last updated: `2026-05-24 23:58 -04:00`

## Phase Decision

The playable game scaffold is paused as the primary production target. Maps, mechanisms, combat, menus, packaging, route QA, and existing generated levels remain valuable infrastructure, but new development should not keep filling the game with approximate art.

The active art-production phase is now exact concept-art replication in Unity. Small objects and object families are recreated from the north-star images first, judged against the crop, then promoted into modular game-ready packages only after they pass.

## Why This Changed

Recent Unity renders were useful as blockout and lookdev evidence, but they did not meet the AAA visual target. The failure was not only asset quantity; it was pipeline quality:

- Meshes were too primitive and visibly procedural.
- Materials were too saturated, flat, or toy-like.
- Lighting and reflection calibration were inconsistent.
- Acceptance gates allowed broad package completion without proving an exact visual match.

The new method is stricter: one small reference crop, one Unity-only replica, one honest pass/fail review.

## Current First Targets

1. Gear key replica.
   - Reference: gear-shaped key crop from the original steampunk concept art.
   - Purpose: replace the playable gear-key pickup with a visually credible object once it passes.
   - Required read: dark aged brass/bronze, blackened recesses, gear teeth, inner spokes, central hub, ridged shaft, beveled bit, warm highlights, non-toy roughness.

2. Pipe bundle replica.
   - Reference: modular pipe-bundle crop from the original steampunk concept art.
   - Purpose: create reusable pipe, elbow, flange, collar, bracket, valve, and wall-contact modules for repeated use across levels.
   - Required read: aged copper/brass, blackened iron collars, oily grime, warm specular glints, wet masonry contact, steam-ready composition, modular construction.

## Acceptance Gates

An asset can be called `final-art candidate` only when all are true:

- Rendered in Unity, not mocked externally.
- Compared against the specific crop it is replicating.
- Shows a beauty render, detail breakdown, and material closeup/contact sheet.
- Includes metadata/catalog and QA notes.
- Avoids raw primitive appearance.
- Avoids saturated orange/copper toy materials.
- Uses believable bevels, collars, seams, fasteners, wear, grime, and roughness variation.
- Has an honest QA verdict with explicit gaps.

If it fails the visual target, it remains quarantine evidence and should be revised or replaced, not promoted into the playable scaffold.

Detailed scoring lives in `Documentation/QA/REFERENCE_REPLICA_ACCEPTANCE_CHECKLIST.md`. Promotion requires no score below `4` and an average score of at least `4.25`.

## Production Flow

1. Select one crop from the north-star concept art.
2. Describe silhouette, parts, materials, lighting, scale, and reusable modules.
3. Build the replica in an isolated Unity sidecar package or `roomtest`.
4. Produce render sheets and QA self-score.
5. Review visually against the crop.
6. If accepted, create a promotion task for the playable scaffold.
7. If rejected, fix the specific mismatch before widening scope.

## Current Worker Allocation

- Euclid the 2nd: `ConceptReplicaGearKeySet13`.
- Parfit the 2nd: `ConceptReplicaPipeBundleSet13`.
- Gibbs the 2nd: material proof redirected to aged gear-key and pipe-bundle materials.
- Galileo the 2nd: `roomtest` final-art environment proof remains active as supporting wall/floor/lighting evidence.

## Playable Scaffold Rule

No more broad playable art imports should occur until at least one exact concept-art replica passes the stricter gate. The playable scaffold resumes when approved replicas can replace or dress actual game objects without lowering quality.

Next-step directive: continue immediately with concept-replica production, review the gear key and pipe bundle first, then promote only accepted visual matches into the playable game.
