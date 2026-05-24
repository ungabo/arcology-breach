# Enemy Readability Batch Import Notes

Unity path: `Assets/_Project/ArtStaging/EnemyReadabilityBatch/`

## Import Contract

- OBJ units are meters.
- Axis is `+Y` up, `+Z` forward.
- Use local MTL material names for first-pass review.
- Keep mesh colliders disabled.
- Treat all meshes as staged art/readability proxies, not final gameplay enemies.

## Primary Files

- `Meshes/ENEMY_ERB_Scrapper_ReadabilityUpgrade_LOD0.obj`
- `Meshes/ENEMY_ERB_Lancer_ReadabilityUpgrade_LOD0.obj`
- `Meshes/ENEMY_ERB_Bulwark_ReadabilityUpgrade_LOD0.obj`
- `Meshes/ENEMY_ERB_Warden_ReadabilityUpgrade_LOD0.obj`
- `Meshes/FRAG_ERB_*_ShutdownFragments.obj`
- `Materials/ENEMY_ERB_ReadabilityProxyMaterials.mtl`
- `Materials/MAT_ERB_*.mat`
- `Previews/PREVIEW_ERB_EnemyReadabilityBatch_ContactSheet.png`
- `Metadata/ERB_EnemyReadabilityBatch_Manifest.json`

## Material Intent

- `MAT_ERB_BlackenedIron`: silhouette mass and armor.
- `MAT_ERB_AgedBrass`: trims, bands, rings, cage ribs.
- `MAT_ERB_CopperPipe`: pressure pipes, pistons, lance accents.
- `MAT_ERB_CreamEnamel`: face plates.
- `MAT_ERB_FurnaceEyeAmber`: identity/alert eyes.
- `MAT_ERB_WeakPointLamp`: damage-read lamp glass.
- `MAT_ERB_PressureTankRed`: pressure vessel read.
- `MAT_ERB_BoltTellBlue`: charge/ranged tell.
- `MAT_ERB_HazardEnamel`: cutter, hammer, shield warning marks.
- `MAT_ERB_ShutdownFragmentDim`: detached shutdown debris.

