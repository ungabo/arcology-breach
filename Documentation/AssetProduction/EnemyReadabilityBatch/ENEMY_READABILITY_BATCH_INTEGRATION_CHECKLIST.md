# Enemy Readability Batch Integration Checklist

Date: 2026-05-24
Use this when the staged package moves from `ArtStaging/EnemyReadabilityBatch` into a future integration lane.

## Import Review

- [ ] Unity imports all four main OBJ meshes without missing-file errors.
- [ ] Unity imports all four shutdown fragment OBJs without missing-file errors.
- [ ] Local material names from `ENEMY_ERB_ReadabilityProxyMaterials.mtl` resolve or can be remapped to the matching `MAT_ERB_*` assets.
- [ ] No mesh colliders are generated during import.
- [ ] Mesh groups remain named enough for art review.
- [ ] PNG preview boards are visible in the Project window.
- [ ] `Metadata/ERB_EnemyReadabilityBatch_Manifest.json` is present and readable.

## Per-Enemy Visual Gates

- [ ] Scrapper reads as compact melee before materials are inspected.
- [ ] Scrapper keeps one cutter tell and one hammer tell.
- [ ] Scrapper chest weak-point lamp is separate from furnace eyes.
- [ ] Lancer reads as tall ranged before materials are inspected.
- [ ] Lancer lance direction points cleanly down +Z.
- [ ] Lancer blue bolt coils are not confused with the weak-point lamp.
- [ ] Bulwark reads as broad shield defender before materials are inspected.
- [ ] Bulwark side weak-point lamps remain visible around the shield mass.
- [ ] Bulwark hammer tell remains visible past the shield.
- [ ] Warden reads as tall cage/command unit before materials are inspected.
- [ ] Warden crown tanks and overhead bolt coils remain visible from front and side.
- [ ] Warden central weak-point lamp is separate from face/eyes.

## Material Gates

- [ ] Blackened iron is the dominant chassis/armor material.
- [ ] Aged brass is used as trim, bands, rings, and retainers.
- [ ] Copper is reserved for pressure lines, rods, and barrels.
- [ ] Cream enamel is limited to face/identity plates.
- [ ] Amber furnace eyes are distinct from weak-point lamp glass.
- [ ] Blue bolt tell material is reserved for charge/ranged telegraph elements.
- [ ] Red pressure tanks read as pressure vessels, not pickups.
- [ ] Dim fragment material reads as broken/deactivated debris.

## Future Prefab Assembly Notes

- Root prefab names should retain the enemy id and `ReadabilityUpgrade` source marker until final art replaces these staging meshes.
- Use child roots for `geometry`, `readability_tells`, `weak_points_visual_only`, `shutdown_fragments_visual_only`, and `metadata` if a prefab pass is created later.
- Do not add hitboxes, damage rules, movement logic, AI, NavMesh obstacles, or combat behavior from this package alone.
- If weak-point or destructible gameplay is added later, it should be owned by a separate gameplay integration pass and validated independently.

## Stop Conditions

Stop integration and return to art staging if:

- Any enemy loses its silhouette identity when viewed in flat lighting.
- A weak-point lamp reads as a muzzle flash, pickup, or generic decoration.
- Pressure tanks become the most dominant target on every enemy.
- Blue bolt tells appear on melee attacks or non-bolt behaviors.
- Shutdown fragments read like live pickups or interactables.
