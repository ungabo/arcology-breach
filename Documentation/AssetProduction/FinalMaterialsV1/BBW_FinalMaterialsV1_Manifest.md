# BBW_FinalMaterialsV1 Manifest

Created: 2026-05-23T23:41:54-04:00
Status: final-material-v1 staging package; no Unity scenes, gameplay scripts, shared status docs, README files, ConceptRenders, existing material packages, or active material assets were modified.

## Scope

This package stages final-material V1 texture sources for the steampunk art direction. It is intentionally outside active Unity material integration: texture PNGs are placed in the isolated ArtStaging folder, while previews and reports live in this documentation folder.

## Map Packing

- BaseColor: sRGB PNG. Amber Glass and Scorch/Oil Decal Atlas include alpha in BaseColor.
- Normal: height-derived tangent-space approximation; import as a Unity Normal Map.
- ORM: R = ambient occlusion, G = roughness, B = metallic, matching the existing MaterialsPBR staging convention.
- Alpha: separate opacity helper only for the decal atlas.

## Materials

| Material ID | Display | Tileable | BaseColor | Normal | ORM | Extra | Quality |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `MAT_BBW_AgedBrass_V1` | Aged Brass | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_AgedBrass_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_AgedBrass_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_AgedBrass_ORM_2048.png` |  | pass |
| `MAT_BBW_BlackenedRivetedIron_V1` | Blackened Riveted Iron | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_BlackenedRivetedIron_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_BlackenedRivetedIron_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_BlackenedRivetedIron_ORM_2048.png` |  | pass |
| `MAT_BBW_WetOilDarkStone_V1` | Wet Oil-Dark Stone | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_WetOilDarkStone_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_WetOilDarkStone_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_WetOilDarkStone_ORM_2048.png` |  | pass |
| `MAT_BBW_SootBrick_V1` | Soot Brick | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_SootBrick_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_SootBrick_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_SootBrick_ORM_2048.png` |  | pass |
| `MAT_BBW_CopperPipe_V1` | Copper Pipe | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_CopperPipe_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_CopperPipe_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_CopperPipe_ORM_2048.png` |  | pass |
| `MAT_BBW_GreasyWalnut_V1` | Greasy Walnut | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_GreasyWalnut_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_GreasyWalnut_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_GreasyWalnut_ORM_2048.png` |  | pass |
| `MAT_BBW_CreamEnamelGauge_V1` | Cream Enamel Gauge | false | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_CreamEnamelGauge_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_CreamEnamelGauge_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_CreamEnamelGauge_ORM_2048.png` |  | pass |
| `MAT_BBW_AmberGlass_V1` | Amber Glass | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_AmberGlass_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_AmberGlass_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_AmberGlass_ORM_2048.png` |  | pass |
| `MAT_BBW_LeatherBellows_V1` | Leather Bellows | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_LeatherBellows_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_LeatherBellows_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_LeatherBellows_ORM_2048.png` |  | pass |
| `MAT_BBW_HazardEnamel_V1` | Hazard Enamel | true | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_HazardEnamel_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_HazardEnamel_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_HazardEnamel_ORM_2048.png` |  | pass |
| `MAT_BBW_ScorchOilDecalAtlas_V1` | Scorch/Oil Decal Atlas | false | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_ScorchOilDecalAtlas_BaseColor_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_ScorchOilDecalAtlas_Normal_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_ScorchOilDecalAtlas_ORM_2048.png` | `Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_ScorchOilDecalAtlas_Alpha_2048.png` | pass |

## Preview Sheets

- `Documentation/AssetProduction/FinalMaterialsV1/Previews/PREVIEW_BBW_FinalMaterialsV1_BaseColorContactSheet.png` (1440x1306, RGB)
- `Documentation/AssetProduction/FinalMaterialsV1/Previews/PREVIEW_BBW_FinalMaterialsV1_MapTripletsContactSheet.png` (1120x3414, RGB)
- `Documentation/AssetProduction/FinalMaterialsV1/Previews/PREVIEW_BBW_FinalMaterialsV1_TilingContactSheet.png` (1020x1224, RGB)
- `Documentation/AssetProduction/FinalMaterialsV1/Previews/PREVIEW_BBW_FinalMaterialsV1_DecalAtlasAlpha.png` (1300x720, RGB)

## Counts

- Materials: 11
- BaseColor maps: 11
- Normal maps: 11
- ORM maps: 11
- Alpha helper maps: 1
- Preview sheets: 4

## Source Inputs Read

- `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`
- `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`
- `Documentation/AAA_ASSET_CATALOG.md`
- `Documentation/PARALLEL_ASSET_GENERATION_BRIEFS.md`
- `Assets/_Project/ArtStaging/MaterialsPBR/ (read-only reference)`
