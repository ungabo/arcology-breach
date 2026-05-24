# Enemy Readability Batch Manifest

Date: 2026-05-24
Worker scope: staged mechanical-enemy readability/art package for Scrapper, Lancer, Bulwark, and Warden.
Unity path: `Assets/_Project/ArtStaging/EnemyReadabilityBatch/`

## Unity Import Contract

- Units: `1 Unity unit = 1 meter`.
- Axis: `+Y` up, `+Z` forward.
- Source format: generated OBJ/MTL proxy geometry plus Unity Standard `.mat` proxies.
- Material source: `Assets/_Project/ArtStaging/EnemyReadabilityBatch/Materials/ENEMY_ERB_ReadabilityProxyMaterials.mtl`.
- Import settings: import materials from local MTL for first look, calculate normals if Unity requests them, disable mesh colliders, preserve mesh group names for art review.
- Intent: readability staging only. These are not final skinned meshes, not prefab assemblies, and not gameplay-authoritative enemy definitions.

## Asset Index

| Enemy | Main staged mesh | Shutdown fragments | Preview | Coverage |
| --- | --- | --- | --- | --- |
| Scrapper | `Meshes/ENEMY_ERB_Scrapper_ReadabilityUpgrade_LOD0.obj` | `Meshes/FRAG_ERB_Scrapper_ShutdownFragments.obj` | `Previews/PREVIEW_ERB_Scrapper_ReadabilityBoard.png` | Furnace eyes, chest lamp, paired pressure tanks, cutter wheel tell, hammer tell, dim fragments. |
| Lancer | `Meshes/ENEMY_ERB_Lancer_ReadabilityUpgrade_LOD0.obj` | `Meshes/FRAG_ERB_Lancer_ShutdownFragments.obj` | `Previews/PREVIEW_ERB_Lancer_ReadabilityBoard.png` | Furnace eyes, sternum lamp, back pressure tank, long lance read, blue bolt coils, muzzle tell, dim fragments. |
| Bulwark | `Meshes/ENEMY_ERB_Bulwark_ReadabilityUpgrade_LOD0.obj` | `Meshes/FRAG_ERB_Bulwark_ShutdownFragments.obj` | `Previews/PREVIEW_ERB_Bulwark_ReadabilityBoard.png` | Furnace brow eyes, side weak lamps, shield-door mass, shoulder pressure tanks, hammer tell, dim fragments. |
| Warden | `Meshes/ENEMY_ERB_Warden_ReadabilityUpgrade_LOD0.obj` | `Meshes/FRAG_ERB_Warden_ShutdownFragments.obj` | `Previews/PREVIEW_ERB_Warden_ReadabilityBoard.png` | Furnace command eyes, central lamp, cage/tower silhouette, crown tanks, overhead blue bolt tell, dim fragments. |

Contact sheet:

- `Assets/_Project/ArtStaging/EnemyReadabilityBatch/Previews/PREVIEW_ERB_EnemyReadabilityBatch_ContactSheet.png`
- `Documentation/AssetProduction/EnemyReadabilityBatch/Previews/PREVIEW_ERB_EnemyReadabilityBatch_ContactSheet.png`

Machine-readable manifest:

- `Assets/_Project/ArtStaging/EnemyReadabilityBatch/Metadata/ERB_EnemyReadabilityBatch_Manifest.json`
- `Documentation/AssetProduction/EnemyReadabilityBatch/ERB_EnemyReadabilityBatch_Manifest.json`

## Material Roles

| Material | Role |
| --- | --- |
| `MAT_ERB_BlackenedIron` | Main chassis, heavy armor, shield plates, weapon bases, foot mass. |
| `MAT_ERB_AgedBrass` | Boiler bands, rings, retainers, pressure tank collars, cage ribs. |
| `MAT_ERB_CopperPipe` | Pressure rods, lance barrel accents, piston lines, service pipes. |
| `MAT_ERB_DarkRubber` | Hoses, command conduits, gaskets. |
| `MAT_ERB_CreamEnamel` | Face plates and enemy identity masks. |
| `MAT_ERB_FurnaceEyeAmber` | Furnace eyes and hot inner glass. |
| `MAT_ERB_WeakPointLamp` | Readable damage lamps and lens elements. |
| `MAT_ERB_PressureTankRed` | Danger-colored pressure vessels. |
| `MAT_ERB_BoltTellBlue` | Blue bolt, charge, and ranged-attack telegraph elements. |
| `MAT_ERB_HazardEnamel` | Cutter teeth, hammer faces, shield warning stripes, melee windup accents. |
| `MAT_ERB_ShutdownFragmentDim` | Detached shutdown debris and dimmed breakaway reads. |
| `MAT_ERB_SootShadow` | Visor slits and dark cavity separation. |

## Mesh Stats

| Asset | Vertices | Faces |
| --- | ---: | ---: |
| `ENEMY_ERB_Scrapper_ReadabilityUpgrade_LOD0` | 868 | 524 |
| `ENEMY_ERB_Lancer_ReadabilityUpgrade_LOD0` | 804 | 472 |
| `ENEMY_ERB_Bulwark_ReadabilityUpgrade_LOD0` | 740 | 444 |
| `ENEMY_ERB_Warden_ReadabilityUpgrade_LOD0` | 916 | 540 |

Fragment OBJs are intentionally small, separate review pieces for death/shutdown dressing and future VFX handoff. They are not authored as spawned gameplay objects here.

## Package Boundaries

Included:

- Staged OBJ silhouettes for all four enemies.
- Detached fragment OBJ kits for shutdown/death readability.
- MTL proxy material set.
- Unity Standard `.mat` proxy assets and `.meta` files inside the batch folder.
- PNG readability boards in both documentation and Unity staging paths.
- JSON metadata manifest.
- Reproducible PowerShell generator.

Not included:

- Gameplay code.
- Animation controllers.
- Prefabs.
- Scene placement.
- Colliders or hitboxes.
- NavMesh obstacles.
- Blender files or external DCC dependencies.
