# Clockwork Enemy Parts Set 09 Import Readiness Plan

## Intake Goal

Import `com.brassworks.sidecar.clockwork-enemy-parts-set09` as an isolated local package for quarantine art review. The package should support enemy silhouette exploration for a small skitter unit, humanoid boiler brute, and wall/ceiling sentry without adding gameplay authority.

## Quarantine Steps

1. Add the local package reference only in a quarantine branch or validation project.
2. Inspect `Runtime/Metadata/CEPS09_ClockworkEnemyPartsCatalog_v0.1.54-p001.json` before opening prefabs.
3. Review the three archetype preview prefabs first to validate silhouette separation.
4. Review skitter, brute, and sentry part folders by prefix and tag approved modules for future rigging.
5. Confirm material response under the target foundry/cathedral lighting before promotion.
6. Keep `Documentation/ConceptRenders` PNGs outside runtime import; they are evidence only.

## Acceptance Gates

- Static counts match manifest: 32 prefabs, 22 materials, 16 meshes, 22 runtime texture PNGs, 1 metadata catalog.
- Preview evidence exists: 57 PNGs including all-parts, archetype, and material contact sheets.
- Runtime prefab component scan reports zero colliders, rigidbodies, animators, and MonoBehaviours.
- No `.blend`, `.fbx`, audio, scene, controller, animation clip, runtime script, skinned mesh, or gameplay asset is required.

## Rollback

Remove local package reference `com.brassworks.sidecar.clockwork-enemy-parts-set09` and delete the isolated package root. No main project manifest, scene, or gameplay script changes are required for rollback.
