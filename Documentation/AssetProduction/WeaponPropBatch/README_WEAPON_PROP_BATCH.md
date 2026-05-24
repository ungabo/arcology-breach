# Brassworks Breach WeaponPropBatch

Status: staged view package for upcoming Unity integration
Date: 2026-05-24
Scope: `Assets/_Project/ArtStaging/WeaponPropBatch/**` and `Documentation/AssetProduction/WeaponPropBatch/**` only
Rule: Unity-compatible loose assets only. No Blender, no scene files, no code, no prefabs, no gameplay wiring, and no shared documentation edits.

## Package Goal

This batch provides a single reviewable staging lane for the pressure-pistol and gameplay-prop language instead of pushing one isolated asset at a time. The files are fantasy prop/viewmodel proofs for scale, silhouette, material separation, and integration planning. They are not final production meshes and are not functional weapon reference.

## Unity Asset Contents

| Area | Files | Purpose |
| --- | --- | --- |
| Meshes | `Meshes/WPB_001_CoilPackRefinement_Staged.obj` through `Meshes/WPB_007_ScattergunDisplayPieces_Staged.obj` | Loose OBJ staging meshes with named subobjects and MTL material slots. |
| OBJ palette | `Meshes/WPB_Brassworks_BatchPalette.mtl` | Import palette for the staged OBJ material names. |
| Materials | `Materials/M_WPB_*.mat` | Local Unity Standard-shader color materials for brass, copper, iron, glass, walnut, leather, soot, patina, pressure red, ammo case, and furnace glow roles. |
| Draft textures | `Textures/WPB_*_Draft.png` | Gauge face, grip trim, fastener detail, and patina/soot mask drafts for material review. |
| Previews | `Previews/WPB_*_preview.png` | Contact sheet, material swatches, and mesh inventory previews for quick inspection outside a scene. |

## Mesh Coverage

| Mesh | Covers | Integration Note |
| --- | --- | --- |
| `WPB_001_CoilPackRefinement_Staged.obj` | Coil pack refinement with rail frame, heat core, copper loops, pressure sockets, screws, patina, and oil shadows. | Use as a material and silhouette reference for the pistol's steampunk identity beat. |
| `WPB_002_PressureChamberBarrel_Staged.obj` | Pressure chamber, barrel stack, muzzle crown, bore read, fill/relief fittings, bracket feet, soot, and bands. | Use to compare chamber mass against first-person readability before final layout. |
| `WPB_003_GaugeDialFace_Staged.obj` | Gauge cup, brass bezels, cream dial face, 40 tick marks, red needle, glass highlights, side sockets, and top valve. | Use as the dial readability target and texture handoff source. |
| `WPB_004_WalnutLeatherGripPlates_Staged.obj` | Walnut grip plates, leather wraps, brass tang, butt cap, trigger guard loop, screws, grain grooves, and stitch marks. | Use to anchor lower-right viewmodel material contrast. |
| `WPB_005_FastenerPlates_Staged.obj` | Inspection plates, repair strap, boiler band, angled brace, slotted screws, rivets, oil runs, and patina. | Use as a reusable fastener/repair language board. |
| `WPB_006_AmmoCartridgeCase_Staged.obj` | Warm brass cartridge case, base cap, primer, red pressure seal, copper neck, side cartridges, and cradle. | Use as pickup/ammo material and scale reference. |
| `WPB_007_ScattergunDisplayPieces_Staged.obj` | Twin barrels, collars, slug canister, stock socket plate, shell row, socket screws, soot, and patina. | Use as scattergun display-piece vocabulary, not a finished weapon asset. |

## Readiness

Ready for Unity import smoke check as loose staging assets. The package should import without requiring generated scenes, code, or shared material folders. Before integration into prefabs or gameplay, it still needs human art review, final authored topology/UVs, production texture assignment, collider decisions, LOD decisions, pivot/origin cleanup, and first-person scale checks.
