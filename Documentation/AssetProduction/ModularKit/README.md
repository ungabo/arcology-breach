# Brassworks Breach ModularKit 01

First production-staged modular environment kit for steampunk corridors and small rooms. Assets live in `Assets/_Project/ArtStaging/ModularKit` and are authored as Unity-friendly OBJ files with `1 OBJ unit = 1 Unity meter`.

## Preview

- Open `Assets/_Project/ArtStaging/ModularKit/PREVIEW_ModularKit_ContactSheet.png` for the full mesh contact sheet.
- Open `Assets/_Project/ArtStaging/ModularKit/PREVIEW_TextureSwatches.png` for the material texture sheet.
- Each OBJ also has an individual `PREVIEW_*.png`.

## Unity Import

1. In Unity, import or refresh `Assets/_Project/ArtStaging/ModularKit`.
2. Keep each OBJ beside `KIT_ModularKit_Materials.mtl` and the `KIT_MAT_*_BaseColor.png` textures.
3. Use scale factor `1.0`; the kit is authored in meters.
4. If Unity does not auto-bind MTL texture maps, assign the listed `KIT_MAT_*` base color textures to generated materials manually.
5. Use `KIT_ModularKit_ReferenceAssembly.obj` as a quick import sanity check, not as a final gameplay prefab.

## Authoring Notes

- Coordinate system: `+Y` up, `+Z` forward.
- Modular panels are built around 4m grid dimensions where applicable.
- This pass is staged LOD0 geometry only. It intentionally avoids scene, prefab, gameplay script, and root documentation edits.
- Collision should be authored separately with simple primitives; avoid using small visual rivets, grate bars, cage rods, and gauge details as gameplay collision.

