# High Fidelity Lookdev Brief

Created: 2026-05-23 22:41:03 -04:00  
Worker: parallel HighFidelityLookdev agent  
Source concept: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`  
Status: lookdev, non-shipping

## North-Star Read

The concept art is a three-part target board:

- Corridor and pressure door: a wet, dark, oil-stained industrial dungeon corridor with black masonry, riveted iron wall plates, layered brass/copper pipes, warm amber cage lamps, pressure gauges, steam leaks, guardrails, and a large round vault-like door with a gear hub and radial braces.
- Scrapper-like monster: a squat mechanical enemy built from boiler forms, brass head and eyes, grille mouth, exposed gears, piston limbs, saw/claw tools, heavy plated feet, pressure gauge, and steam stacks.
- Pressure pistol: a first-person weapon with blackened iron barrel and reservoir, aged brass rings/frames, exposed copper coil window, cream pressure gauge, leather grip/glove read, side rivets, and steam venting.

The lookdev standard is not just "more detail." Every detail should imply pressure, heat, service access, fasteners, moving mechanisms, or age.

## Visual Standards

- Surfaces must read as steampunk first: brass, copper, blackened iron, oil-dark stone, amber glass, leather, cream enamel gauges, and steam haze.
- Hero silhouettes must be layered: cylinders over plates, pipes over masonry, rings over doors, gauges on machinery, rivets along seams, and exposed mechanisms at focal points.
- Lighting language should be warm amber key lights against dark wet materials; avoid clean sci-fi blues, neon, glossy black chrome, and sterile machinery.
- Repetition must be modular but broken by stains, valves, gauges, lamps, and pipe elbows so corridors do not read as greybox tiling.
- Important gameplay reads must survive at distance: door center gear, enemy eyes/weapons, pistol gauge/coil, hazard/exit affordances.

## Material IDs

| Material ID | Use | PBR intent |
| --- | --- | --- |
| `MAT_HFLD_AgedBrassHero` | Door rings, gear hubs, railings, collars, pistol trim, monster bands | Metallic 1.0, roughness around 0.35-0.45, polished edges, tarnished recesses |
| `MAT_HFLD_BlackenedRivetedIron` | Door plates, wall plates, barrels, armor shells, saws, feet | Metallic 1.0, roughness around 0.60-0.75, dark base with bright edge chips |
| `MAT_HFLD_OilWetStone` | Corridor masonry, wet floor slabs, dungeon ground | Nonmetal, roughness 0.18-0.35 in oil patches, high AO in cracks |
| `MAT_HFLD_GreenOxidizedCopper` | Older pipes, tanks, oxidized machinery accents | Metallic mixed 0.45-0.8, high roughness in verdigris, copper wear exposed |
| `MAT_HFLD_AmberGlassLit` | Lamps, enemy eyes, furnace glow, sight glass | Emissive/transparent candidate, low roughness where clean, grime breakup |
| `MAT_HFLD_LeatherGripDark` | Pistol grip, straps, bellows, wrapped handles | Nonmetal, roughness 0.65-0.85, creased and oil-polished high points |
| `MAT_HFLD_CreamGaugeFace` | Gauge faces and aged enamel labels | Nonmetal, readable tick marks, slight yellowing and grime at rim |
| `MAT_HFLD_CopperCoilHot` | Exposed pistol coils, pressure conduits | Metallic 1.0, roughness around 0.30-0.45, warm heat staining |

## First Batch Output

This pass created static, non-shipping lookdev placeholders:

- `HF_CorridorDoor_LookdevBlockout.obj`
- `HF_PressurePistol_LookdevBlockout.obj`
- `HF_ScrapperMonster_LookdevBlockout.obj`
- `MAT_HFLD_Batch01_LookdevMaterials.mtl`
- `MAT_HFLD_Batch01_MaterialSwatches.png`
- JPG render/contact-sheet previews under `Documentation/ConceptRenders/`

These are source/staging assets, not production meshes. They exist to lock down component vocabulary, material assignment, proportions, and reviewable direction before final modeling.

## Unity Lookdev Scene Proposal

No Unity scene was created in this pass to avoid disturbing the main build. If approved, create a separate scene later at:

`Assets/_Project/ArtStaging/HighFidelityLookdev/Scenes/HFLD_LookdevGallery.unity`

Suggested layout:

- Bay A: 6 m wide x 10 m deep corridor slice with pressure door at the far end.
- Bay B: rotating pedestal for `HF_PressurePistol_LookdevBlockout.obj` plus first-person camera framing.
- Bay C: rotating pedestal for `HF_ScrapperMonster_LookdevBlockout.obj` with neutral gray floor and warm rim light.
- Lighting: one warm amber key, one soft overhead fill, low-intensity fog volume, optional steam particle proxies disabled by default.
- Camera bookmarks: corridor wide, door close-up, pistol three-quarter, monster front, monster side silhouette.
