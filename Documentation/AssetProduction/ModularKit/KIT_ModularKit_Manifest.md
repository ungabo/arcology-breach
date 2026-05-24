# KIT ModularKit Manifest

Source asset folder: `Assets/_Project/ArtStaging/ModularKit`

Unit scale: `1 OBJ unit = 1 Unity meter`

Coordinate system: `+Y` up, `+Z` forward

Material library: `KIT_ModularKit_Materials.mtl`

## Mesh Inventory

| Mesh | Dimensions X/Y/Z (m) | Purpose | Material Slots |
| --- | ---: | --- | --- |
| `SM_WallPanel_RivetedTrim_4m.obj` | 4 x 3 x 0.29 | 4m wall panel with riveted brass trim and oil-dark masonry slabs | `MAT_AgedBrass`, `MAT_OilDarkMasonry`, `MAT_RivetedIron` |
| `SM_FloorTile_GrateSeams_4m.obj` | 4 x 0.21 x 4 | 4m floor tile with recessed grate, raised plate seams, brass trim | `MAT_AgedBrass`, `MAT_DarkGrate`, `MAT_RivetedIron` |
| `SM_CeilingPanel_PipeChannels_4m.obj` | 4 x 0.395 x 4 | 4m ceiling panel with underside pipe channels and straps | `MAT_AgedBrass`, `MAT_DarkGrate`, `MAT_OxidizedPipe`, `MAT_RivetedIron` |
| `SM_CorridorShell_Straight_4m.obj` | 4 x 3.3 x 4 | Straight corridor shell segment, open at both ends | `MAT_AgedBrass`, `MAT_OilDarkMasonry`, `MAT_OxidizedPipe`, `MAT_RivetedIron` |
| `SM_DoorwayThreshold_Frame_4m.obj` | 4 x 3.1 x 0.65 | Doorway frame and threshold module | `MAT_AgedBrass`, `MAT_DarkGrate`, `MAT_OilDarkMasonry`, `MAT_RivetedIron` |
| `SM_CornerIntersection_Marker_4m.obj` | 4 x 0.595 x 4 | Intersection/corner marker plate with brass cross and corner bollards | `MAT_AgedBrass`, `MAT_DarkGrate`, `MAT_RivetedIron` |
| `SM_PipeRun_Straight_4m.obj` | 4.02 x 0.56 x 0.44 | Straight pipe run with flanges and brackets | `MAT_AgedBrass`, `MAT_OxidizedPipe`, `MAT_RivetedIron` |
| `SM_PipeElbow_90deg.obj` | 1.01 x 0.4 x 1.01 | 90 degree pipe elbow with brass flanges | `MAT_AgedBrass`, `MAT_OxidizedPipe` |
| `SM_ValveWheel_Prop.obj` | 0.696 x 0.696 x 0.27 | Valve wheel prop with rim, spokes, hub, and stem | `MAT_AgedBrass`, `MAT_OxidizedPipe`, `MAT_RivetedIron` |
| `SM_PressureGauge_Prop.obj` | 0.49 x 0.66 x 0.193 | Pressure gauge prop with dial face, bezel, needle, and pipe socket | `MAT_AgedBrass`, `MAT_GaugeFace`, `MAT_GaugeNeedle`, `MAT_OxidizedPipe`, `MAT_RivetedIron` |
| `SM_LampHousing_Prop.obj` | 0.56 x 0.79 x 0.46 | Caged lamp housing with glass globe and warm glow material slot | `MAT_AgedBrass`, `MAT_LampGlass`, `MAT_RivetedIron`, `MAT_WarmLampGlow` |
| `KIT_ModularKit_ReferenceAssembly.obj` | 4 x 3.3 x 4 | Combined corridor bay for import sanity checks | `MAT_AgedBrass`, `MAT_LampGlass`, `MAT_OilDarkMasonry`, `MAT_OxidizedPipe`, `MAT_RivetedIron`, `MAT_WarmLampGlow` |

## Materials And Textures

| Material | Texture | Notes |
| --- | --- | --- |
| `MAT_AgedBrass` | `KIT_MAT_AgedBrass_BaseColor.png` | Tarnished brass, metallic, rough |
| `MAT_RivetedIron` | `KIT_MAT_RivetedIron_BaseColor.png` | Dark scratched iron, metallic |
| `MAT_OilDarkMasonry` | `KIT_MAT_OilDarkMasonry_BaseColor.png` | Dark masonry with block seams and oil staining |
| `MAT_DarkGrate` | `KIT_MAT_DarkGrate_BaseColor.png` | Deep grate recesses and dark metal |
| `MAT_OxidizedPipe` | `KIT_MAT_OxidizedPipe_BaseColor.png` | Oxidized pipe metal with green streaking |
| `MAT_LampGlass` | `KIT_MAT_LampGlass_BaseColor.png` | Glass slot; alpha noted in MTL/JSON manifest |
| `MAT_WarmLampGlow` | `KIT_MAT_WarmLampGlow_BaseColor.png` | Warm emissive candidate slot |
| `MAT_GaugeFace` | `KIT_MAT_GaugeFace_BaseColor.png` | Gauge dial face with ticks and markings |
| `MAT_GaugeNeedle` | none | Red needle geometry material |

## Collision Notes

- Wall, floor, ceiling, shell, doorway, and corner marker modules: use simple box colliders or a small set of box colliders.
- Pipe run: use capsule colliders or a simplified cylinder proxy.
- Pipe elbow: use two capsule colliders plus a small corner proxy, or convex mesh collision only if needed.
- Valve wheel, pressure gauge, and lamp: use primitive/convex proxy collision for interaction zones.
- Do not use rivets, grate bars, gauge needles, lamp cage rods, or other fine LOD0 detail as gameplay collision.

## LOD Notes

- Current staged assets are LOD0 only.
- Suggested LOD1: remove individual rivets, extra grate bars, lamp cage rods, and small face details.
- Suggested LOD2: use proxy slabs/cylinders only, preserving silhouette and snap dimensions.

## Machine Manifest

`Assets/_Project/ArtStaging/ModularKit/KIT_ModularKit_Manifest.json` mirrors this manifest with exact bounding boxes, material slots, face counts, and import notes.

