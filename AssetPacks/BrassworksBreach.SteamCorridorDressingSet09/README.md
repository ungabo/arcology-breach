# BrassworksBreach.SteamCorridorDressingSet09

Unity-only steampunk corridor dressing family sidecar for Brassworks Breach.

This package is a deterministic generator package rather than a checked-in prefab YAML drop. Import it as an embedded/local Unity package, then run:

`Brassworks Breach/Sidecars/Steam Corridor Dressing Set 09/Generate Package Assets`

The generator creates materials, meshes, 20 visual-only piece prefabs, and one full-family palette prefab under `Runtime/Generated` inside this package.

## Contents

- Runtime catalog code: `Runtime/Definitions/SteamCorridorDressingSet09Catalog.cs`.
- Runtime metadata catalog: `Runtime/Metadata/SCD09_SteamCorridorDressingSet09_Catalog_0.1.54-p001.json`.
- Editor generator: `Editor/SteamCorridorDressingSet09Generator.cs`.
- Package manifest: `package.json`.
- Package manifest mirror: `Documentation~/Manifest/SCD09_SteamCorridorDressingSet09_Manifest_0.1.54-p001.json`.
- Preview contact sheet generator: `Tools/GenerateContactSheet.ps1`.

## Generated Asset Plan

- 12 Unity materials: wet black stone, blackened brick, oily iron, aged brass, copper, verdigris, amber glass, gauge ivory, red enamel, warning paint, steam mist marker, dark cable rubber.
- 6 generated mesh assets: box, 16/24/32-sided cylinders, 24/32-segment torus rings.
- 20 generated prop prefabs grouped into wall, floor, ceiling, and doorway families.
- 1 generated palette prefab that lays out the whole family for review.

## Piece Families

Wall:

- `SCD09_001_WallPipeTripleRun_A`
- `SCD09_002_WallGaugeManifold_B`
- `SCD09_003_WallTankStrapped_C`
- `SCD09_004_WallValveBattery_D`
- `SCD09_005_WallGaslightSconce_E`
- `SCD09_006_WallCableBracketRail_F`

Floor:

- `SCD09_007_FloorWetDrainPlate_A`
- `SCD09_008_FloorGrateChannel_B`
- `SCD09_009_FloorSteamVentLow_C`
- `SCD09_010_FloorHandrailStanchion_D`
- `SCD09_011_FloorInspectionHatch_E`

Ceiling:

- `SCD09_012_CeilingPipeCluster_A`
- `SCD09_013_CeilingCondensateTray_B`
- `SCD09_014_CeilingLampCage_C`
- `SCD09_015_CeilingCableLoom_D`
- `SCD09_016_CeilingSteamNozzle_E`

Doorway:

- `SCD09_017_DoorwayRivetedHeader_A`
- `SCD09_018_DoorwayPressureLockValve_B`
- `SCD09_019_DoorwayThresholdDrain_C`
- `SCD09_020_DoorwaySideTankPair_D`

## Visual Contract

The look target is blackened stone, wet floors, brass/copper pipework, pressure gauges, amber gaslight lamps, riveted iron, valves, vents, handrails, cable brackets, floor drains, ceiling pipe clusters, and wall tanks.

Generated prefabs are visual-only by design. They should contain only `Transform`, `MeshFilter`, and `MeshRenderer` components. Collision, occlusion, lights, particles, audio, triggers, animation, and gameplay hooks must be authored later by the integration owner.

## Import Notes

1. Import the sidecar into a quarantine Unity project first.
2. Run the Set09 generator menu item.
3. Inspect `Runtime/Generated/Prefabs`.
4. Place one prefab from each family plus `SCD09_PREFAB_000_FullCorridorDressingPalette`.
5. Promote approved generated outputs through the normal Brassworks asset gate.
