# MAT_MaterialsPBR_Batch01 Manifest

Created: 2026-05-23 22:01:27 -04:00
Worker: Worker A
Status: production-staged procedural prototype

## Scope

This batch stages the first steampunk PBR material/texture set for Brassworks Breach. It contains eight material IDs, each with 1024x1024 BaseColor, Normal, and packed ORM maps, plus preview sheets for fast review.

No scenes, gameplay scripts, README files, root project docs, or existing integrated materials were modified.

## Packing

- BaseColor: sRGB PNG. `MAT_GrimyAmberGlass` uses BaseColor alpha as prototype opacity.
- Normal: tangent-space PNG generated from procedural height. Import as a Unity Normal Map.
- ORM: packed mask PNG. Red = ambient occlusion, Green = roughness, Blue = metallic. Import linear/non-sRGB.

## Materials

| Material ID | Intent | BaseColor | Normal | ORM |
| --- | --- | --- | --- | --- |
| `MAT_AgedBrass` | Trim, pipes, pressure hardware, ornate panels | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_AgedBrass_BaseColor.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_AgedBrass_Normal.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_AgedBrass_ORM.png` |
| `MAT_RivetedBlackenedIron` | Doors, enemy armor, wall plates, industrial braces | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_RivetedBlackenedIron_BaseColor.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_RivetedBlackenedIron_Normal.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_RivetedBlackenedIron_ORM.png` |
| `MAT_SootStainedBrick` | Furnace rooms, dungeon walls, chimney recesses | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_SootStainedBrick_BaseColor.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_SootStainedBrick_Normal.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_SootStainedBrick_ORM.png` |
| `MAT_WetOilDarkStone` | Floors, service tunnels, damp dungeon slabs | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_WetOilDarkStone_BaseColor.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_WetOilDarkStone_Normal.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_WetOilDarkStone_ORM.png` |
| `MAT_GreenOxidizedCopper` | Pipes, tanks, service machinery, antique trim | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_GreenOxidizedCopper_BaseColor.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_GreenOxidizedCopper_Normal.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_GreenOxidizedCopper_ORM.png` |
| `MAT_GrimyAmberGlass` | Gauge lenses, lamps, vats, small windows | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_GrimyAmberGlass_BaseColor.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_GrimyAmberGlass_Normal.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_GrimyAmberGlass_ORM.png` |
| `MAT_LeatherBellows` | Pump machinery, accordion joints, pressure organs | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_LeatherBellows_BaseColor.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_LeatherBellows_Normal.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_LeatherBellows_ORM.png` |
| `MAT_HazardEnamel` | Danger panels, doors, pressure signage, machinery guards | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_HazardEnamel_BaseColor.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_HazardEnamel_Normal.png` | `Assets/_Project/ArtStaging/MaterialsPBR/Textures/T_HazardEnamel_ORM.png` |

## Preview Sheets

- `Assets/_Project/ArtStaging/MaterialsPBR/Previews/T_MaterialsPBR_Batch01_MapTriplets_ContactSheet.png`
- `Assets/_Project/ArtStaging/MaterialsPBR/Previews/T_MaterialsPBR_Batch01_MapTriplets_ContactSheet.jpg`
- `Assets/_Project/ArtStaging/MaterialsPBR/Previews/T_MaterialsPBR_Batch01_BaseColorTiling_ContactSheet.png`
- `Assets/_Project/ArtStaging/MaterialsPBR/Previews/T_MaterialsPBR_Batch01_SwatchSheet.png`

## Notes

- These are real PNG/JPG texture files and can be inspected directly outside Unity.
- The JSON manifest includes dimensions, modes, and SHA-256 checksums for generated texture and preview files.
- Final Unity material assets were intentionally not created in this pass so the main integration agent can decide shader, import settings, and material placement.
