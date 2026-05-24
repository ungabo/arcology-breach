# Brassworks Breach Encounter Enemy Set 02

Unity-only sidecar package for visual mechanical enemy candidates. The set is intentionally isolated from gameplay systems: no runtime scripts, no autonomous audio, no physics authority, and generated prefabs contain no colliders.

## Contents

- 16 generated visual-only enemy prefabs across four encounter families.
- Reusable procedural mesh assets for saw, claw, lance, hammer, shield, furnace, gauge, valve, tank, fin, and gear modules.
- Procedural materials with furnace-eye emission, cyan pressure tells, worn iron, aged brass, copper, grime, glass, and readability ghosting.
- Runtime catalog JSON with family, pose, module, and future rig socket notes.
- Package-local manifest under `Documentation~/Manifest`.
- Unity-generated preview PNGs under `Documentation/ConceptRenders/V0_1_41_EncounterEnemySet02`.

## Runtime Safety

The prefabs are authored for quarantine review and future rigging only. Integration should add gameplay scripts, hit proxies, colliders, animation, VFX, and audio in the primary lane after review.
