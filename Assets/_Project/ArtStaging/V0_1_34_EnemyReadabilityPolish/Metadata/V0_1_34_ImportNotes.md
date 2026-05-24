# V0.1.34 Enemy Readability Polish Import Notes

Unity path: `Assets/_Project/ArtStaging/V0_1_34_EnemyReadabilityPolish/`

## Contract

- Overlay/readability proxies only.
- OBJ units are meters.
- Axis is `+Y` up and `+Z` forward.
- Keep mesh colliders disabled.
- Preserve material names for lookdev remap.
- Do not infer gameplay hitboxes, damage rules, AI behavior, or rig requirements from these files.

## Primary Files

- `Meshes/ENEMY_V0134_Scrapper_ReadabilityOverlay.obj`
- `Meshes/ENEMY_V0134_Lancer_ReadabilityOverlay.obj`
- `Meshes/ENEMY_V0134_Bulwark_ReadabilityOverlay.obj`
- `Meshes/ENEMY_V0134_Warden_ReadabilityOverlay.obj`
- `Meshes/FRAG_V0134_EnemyShutdownCueFragments.obj`
- `Materials/ENEMY_V0134_ReadabilityPolishMaterials.mtl`
- `Materials/MAT_V0134_*.mat`
- `Previews/PREVIEW_V0134_EnemyReadabilityPolish_SwatchSheet.png`
- `Metadata/V0_1_34_EnemyReadability_Manifest.json`

## Main-Lane Use

Parent the overlay OBJ under current enemy prefabs only long enough to review silhouette and tell placement. Replace or merge the geometry by hand during the final art pass.