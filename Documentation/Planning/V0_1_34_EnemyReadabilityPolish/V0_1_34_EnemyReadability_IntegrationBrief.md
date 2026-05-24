# V0.1.34 Enemy Readability Polish Integration Brief

Date: 2026-05-24
Package: `V0_1_34_EnemyReadabilityPolish`

## Goal

Make Scrapper, Lancer, Bulwark, and Warden readable in combat before final rigging by staging a compact overlay package for silhouette, tell language, weak-point separation, and material remap planning.

## Reviewed Source

- `Assets/_Project/ArtStaging/EnemyReadabilityBatch/`
- `Documentation/AssetProduction/EnemyReadabilityBatch/`

## Integration Shape

Use this package as a visual overlay/reference layer, not as a final enemy replacement. The OBJ meshes are intentionally lightweight and should be placed beside or temporarily parented under current enemy prefabs for lookdev review.

Recommended child organization for a prefab trial:

- `readability_silhouette_proxy`
- `readability_attack_tells`
- `readability_weak_points_visual_only`
- `readability_pressure_language`
- `readability_shutdown_fragments_visual_only`

## Enemy Direction

Scrapper should read as the short melee unit: hunched, low, asymmetric, with a round cutter tell on one side and a block hammer tell on the other. Keep the chest weak lamp separate from the paired furnace eyes.

Lancer should read as the tall ranged unit: thin body, uninterrupted +Z lance line, cyan charge rings, and a muzzle cue that cannot be confused with the sternum weak lamp.

Bulwark should read as the broad defender: shield-door mass first, brow eyes above the shield, side weak lamps visible around the shield, and a hammer tell visible past the shield edge.

Warden should read as the command unit: tall cage/tower profile, brass ribs, crown pressure tanks, central tower weak lamp, and overhead cyan command coils. It must not collapse into a taller Lancer.

## Import Notes

- OBJ units are meters.
- Axis is `+Y` up and `+Z` forward.
- Keep mesh colliders disabled.
- Preserve material names for first-pass remapping.
- Treat all weak-point lamps as visual affordances only until gameplay owns the rule.

## Top Integration Recommendations

1. Do a silhouette-only graybox pass first, then enable material colors.
2. Keep weak-point lamps physically separate from furnace eyes and muzzle/bolt effects.
3. Reserve cyan/blue only for Lancer and Warden charge language.
4. Preserve Scrapper asymmetry and Bulwark side-lamp visibility during rig or prefab cleanup.
5. Use shutdown fragments as small dim breakaway cues, not pickups or interactables.