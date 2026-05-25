using System;
using UnityEngine;

namespace BrassworksBreach.SteamCorridorDressingSet09
{
    public enum SteamDressingFamily
    {
        Wall,
        Floor,
        Ceiling,
        Doorway
    }

    public enum SteamMeshKey
    {
        Box,
        Cylinder16,
        Cylinder24,
        Cylinder32,
        Torus24,
        Torus32
    }

    public enum SteamMaterialKey
    {
        WetBlackStone,
        BlackenedBrick,
        OilyIron,
        AgedBrass,
        BurnishedCopper,
        VerdigrisCopper,
        AmberGaslightGlass,
        GaugeIvory,
        RedEnamelNeedle,
        WarningPaint,
        SteamMist,
        DarkCableRubber
    }

    [Serializable]
    public sealed class SteamMaterialDefinition
    {
        public SteamMaterialKey Key;
        public string DisplayName;
        public Color BaseColor;
        public float Metallic;
        public float Smoothness;
        public Color EmissionColor;
        public float EmissionStrength;

        public SteamMaterialDefinition(
            SteamMaterialKey key,
            string displayName,
            Color baseColor,
            float metallic,
            float smoothness,
            Color emissionColor,
            float emissionStrength)
        {
            Key = key;
            DisplayName = displayName;
            BaseColor = baseColor;
            Metallic = metallic;
            Smoothness = smoothness;
            EmissionColor = emissionColor;
            EmissionStrength = emissionStrength;
        }
    }

    [Serializable]
    public sealed class SteamPartDefinition
    {
        public string Name;
        public SteamMeshKey MeshKey;
        public SteamMaterialKey MaterialKey;
        public Vector3 LocalPosition;
        public Vector3 LocalEulerAngles;
        public Vector3 LocalScale;

        public SteamPartDefinition(
            string name,
            SteamMeshKey meshKey,
            SteamMaterialKey materialKey,
            Vector3 localPosition,
            Vector3 localEulerAngles,
            Vector3 localScale)
        {
            Name = name;
            MeshKey = meshKey;
            MaterialKey = materialKey;
            LocalPosition = localPosition;
            LocalEulerAngles = localEulerAngles;
            LocalScale = localScale;
        }
    }

    [Serializable]
    public sealed class SteamPieceDefinition
    {
        public string Id;
        public string DisplayName;
        public SteamDressingFamily Family;
        public string Anchor;
        public Vector3 FootprintMeters;
        public string Description;
        public string[] Tags;
        public SteamPartDefinition[] Parts;

        public SteamPieceDefinition(
            string id,
            string displayName,
            SteamDressingFamily family,
            string anchor,
            Vector3 footprintMeters,
            string description,
            string[] tags,
            SteamPartDefinition[] parts)
        {
            Id = id;
            DisplayName = displayName;
            Family = family;
            Anchor = anchor;
            FootprintMeters = footprintMeters;
            Description = description;
            Tags = tags;
            Parts = parts;
        }
    }

    public static class SteamCorridorDressingSet09Catalog
    {
        public const string PackageId = "com.brassworks.sidecar.steam-corridor-dressing-set09";
        public const string PackageName = "BrassworksBreach.SteamCorridorDressingSet09";
        public const string Version = "0.1.54-p001";
        public const string PackageRoot = "AssetPacks/BrassworksBreach.SteamCorridorDressingSet09";
        public const string GeneratedRoot = PackageRoot + "/Runtime/Generated";
        public const string CatalogAssetPath = PackageRoot + "/Runtime/Metadata/SCD09_SteamCorridorDressingSet09_Catalog_0.1.54-p001.json";

        public static readonly SteamMaterialDefinition[] Materials =
        {
            Mat(SteamMaterialKey.WetBlackStone, "Wet Black Stone", C(0.040f, 0.044f, 0.042f), 0.02f, 0.78f),
            Mat(SteamMaterialKey.BlackenedBrick, "Soot Blackened Brick", C(0.105f, 0.070f, 0.058f), 0.00f, 0.45f),
            Mat(SteamMaterialKey.OilyIron, "Oily Riveted Iron", C(0.055f, 0.057f, 0.058f), 0.80f, 0.58f),
            Mat(SteamMaterialKey.AgedBrass, "Aged Brass", C(0.690f, 0.470f, 0.205f), 1.00f, 0.42f),
            Mat(SteamMaterialKey.BurnishedCopper, "Burnished Copper", C(0.720f, 0.285f, 0.150f), 1.00f, 0.48f),
            Mat(SteamMaterialKey.VerdigrisCopper, "Verdigris Copper", C(0.170f, 0.505f, 0.455f), 0.65f, 0.36f),
            Mat(SteamMaterialKey.AmberGaslightGlass, "Amber Gaslight Glass", C(1.000f, 0.560f, 0.135f), 0.00f, 0.82f, C(1.000f, 0.380f, 0.060f), 0.65f),
            Mat(SteamMaterialKey.GaugeIvory, "Aged Gauge Ivory", C(0.815f, 0.735f, 0.565f), 0.00f, 0.38f),
            Mat(SteamMaterialKey.RedEnamelNeedle, "Red Enamel Needle", C(0.720f, 0.055f, 0.035f), 0.20f, 0.52f),
            Mat(SteamMaterialKey.WarningPaint, "Aged Warning Paint", C(0.820f, 0.610f, 0.140f), 0.10f, 0.32f),
            Mat(SteamMaterialKey.SteamMist, "Steam Mist White", C(0.680f, 0.720f, 0.700f), 0.00f, 0.26f),
            Mat(SteamMaterialKey.DarkCableRubber, "Dark Cable Rubber", C(0.018f, 0.017f, 0.015f), 0.00f, 0.34f)
        };

        public static readonly SteamPieceDefinition[] Pieces =
        {
            Piece(
                "SCD09_001_WallPipeTripleRun_A",
                "Wall Pipe Triple Run A",
                SteamDressingFamily.Wall,
                "wall_center",
                V(3.80f, 2.15f, 0.34f),
                "Layered brass and copper pipe runs over blackened brick with iron clamp bands.",
                Tags("pipework", "blackened-stone", "brass", "corridor-wall"),
                Parts(
                    Part("Brick Backer", SteamMeshKey.Box, SteamMaterialKey.BlackenedBrick, V(0f, 1.05f, 0.08f), E(0f, 0f, 0f), V(3.80f, 2.10f, 0.12f)),
                    Part("Upper Brass Pipe", SteamMeshKey.Cylinder24, SteamMaterialKey.AgedBrass, V(0f, 1.62f, -0.03f), E(0f, 0f, 90f), V(0.18f, 3.42f, 0.18f)),
                    Part("Middle Copper Pipe", SteamMeshKey.Cylinder24, SteamMaterialKey.BurnishedCopper, V(0.18f, 1.14f, -0.05f), E(0f, 0f, 90f), V(0.15f, 3.08f, 0.15f)),
                    Part("Lower Dark Pipe", SteamMeshKey.Cylinder16, SteamMaterialKey.OilyIron, V(-0.12f, 0.68f, -0.04f), E(0f, 0f, 90f), V(0.14f, 3.34f, 0.14f)),
                    Part("Left Clamp", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(-1.36f, 1.15f, -0.16f), E(0f, 0f, 0f), V(0.14f, 1.26f, 0.18f)),
                    Part("Right Clamp", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(1.34f, 1.15f, -0.16f), E(0f, 0f, 0f), V(0.14f, 1.26f, 0.18f))
                )),

            Piece(
                "SCD09_002_WallGaugeManifold_B",
                "Wall Gauge Manifold B",
                SteamDressingFamily.Wall,
                "wall_center",
                V(2.85f, 2.35f, 0.36f),
                "Pressure gauges and red needles on a riveted iron manifold plate.",
                Tags("pressure-gauge", "manifold", "rivet", "brass"),
                Parts(
                    Part("Iron Manifold Plate", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 1.08f, 0.05f), E(0f, 0f, 0f), V(2.70f, 1.80f, 0.10f)),
                    Part("Top Brass Pipe", SteamMeshKey.Cylinder24, SteamMaterialKey.AgedBrass, V(0f, 1.95f, -0.04f), E(0f, 0f, 90f), V(0.12f, 2.30f, 0.12f)),
                    Part("Left Gauge Face", SteamMeshKey.Cylinder32, SteamMaterialKey.GaugeIvory, V(-0.72f, 1.18f, -0.10f), E(90f, 0f, 0f), V(0.48f, 0.055f, 0.48f)),
                    Part("Left Gauge Rim", SteamMeshKey.Torus32, SteamMaterialKey.AgedBrass, V(-0.72f, 1.18f, -0.135f), E(0f, 0f, 0f), V(0.64f, 0.64f, 0.64f)),
                    Part("Left Red Needle", SteamMeshKey.Box, SteamMaterialKey.RedEnamelNeedle, V(-0.67f, 1.23f, -0.18f), E(0f, 0f, -28f), V(0.030f, 0.36f, 0.035f)),
                    Part("Right Gauge Face", SteamMeshKey.Cylinder32, SteamMaterialKey.GaugeIvory, V(0.72f, 1.18f, -0.10f), E(90f, 0f, 0f), V(0.48f, 0.055f, 0.48f)),
                    Part("Right Gauge Rim", SteamMeshKey.Torus32, SteamMaterialKey.AgedBrass, V(0.72f, 1.18f, -0.135f), E(0f, 0f, 0f), V(0.64f, 0.64f, 0.64f)),
                    Part("Right Red Needle", SteamMeshKey.Box, SteamMaterialKey.RedEnamelNeedle, V(0.78f, 1.24f, -0.18f), E(0f, 0f, 34f), V(0.030f, 0.36f, 0.035f)),
                    Part("Lower Copper Run", SteamMeshKey.Cylinder16, SteamMaterialKey.BurnishedCopper, V(0f, 0.47f, -0.05f), E(0f, 0f, 90f), V(0.11f, 2.05f, 0.11f))
                )),

            Piece(
                "SCD09_003_WallTankStrapped_C",
                "Wall Tank Strapped C",
                SteamDressingFamily.Wall,
                "wall_lower",
                V(2.55f, 2.80f, 0.62f),
                "Vertical copper wall tank with iron straps, valve cap, and wet stone backing.",
                Tags("wall-tank", "copper", "valve", "wet-stone"),
                Parts(
                    Part("Wet Stone Slab", SteamMeshKey.Box, SteamMaterialKey.WetBlackStone, V(0f, 1.32f, 0.10f), E(0f, 0f, 0f), V(2.40f, 2.70f, 0.14f)),
                    Part("Copper Tank Body", SteamMeshKey.Cylinder32, SteamMaterialKey.BurnishedCopper, V(0f, 1.32f, -0.12f), E(0f, 0f, 0f), V(0.58f, 1.84f, 0.58f)),
                    Part("Top Cap", SteamMeshKey.Cylinder32, SteamMaterialKey.AgedBrass, V(0f, 2.30f, -0.12f), E(0f, 0f, 0f), V(0.66f, 0.16f, 0.66f)),
                    Part("Bottom Cap", SteamMeshKey.Cylinder32, SteamMaterialKey.AgedBrass, V(0f, 0.34f, -0.12f), E(0f, 0f, 0f), V(0.66f, 0.16f, 0.66f)),
                    Part("Upper Strap", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 1.86f, -0.47f), E(0f, 0f, 0f), V(0.90f, 0.10f, 0.16f)),
                    Part("Lower Strap", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.78f, -0.47f), E(0f, 0f, 0f), V(0.90f, 0.10f, 0.16f)),
                    Part("Valve Wheel", SteamMeshKey.Torus24, SteamMaterialKey.WarningPaint, V(0.64f, 1.18f, -0.35f), E(0f, 0f, 0f), V(0.42f, 0.42f, 0.42f))
                )),

            Piece(
                "SCD09_004_WallValveBattery_D",
                "Wall Valve Battery D",
                SteamDressingFamily.Wall,
                "wall_center",
                V(3.20f, 1.90f, 0.44f),
                "Three valve wheels branching from a grimy wall pipe battery.",
                Tags("valves", "pipework", "warning-paint", "industrial"),
                Parts(
                    Part("Back Rail", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.96f, 0.05f), E(0f, 0f, 0f), V(2.96f, 0.32f, 0.12f)),
                    Part("Main Pipe", SteamMeshKey.Cylinder24, SteamMaterialKey.BurnishedCopper, V(0f, 0.96f, -0.08f), E(0f, 0f, 90f), V(0.16f, 2.88f, 0.16f)),
                    Part("Left Stem", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(-0.92f, 1.18f, -0.10f), E(0f, 0f, 0f), V(0.08f, 0.48f, 0.08f)),
                    Part("Center Stem", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(0f, 1.24f, -0.10f), E(0f, 0f, 0f), V(0.08f, 0.60f, 0.08f)),
                    Part("Right Stem", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(0.92f, 1.18f, -0.10f), E(0f, 0f, 0f), V(0.08f, 0.48f, 0.08f)),
                    Part("Left Wheel", SteamMeshKey.Torus24, SteamMaterialKey.WarningPaint, V(-0.92f, 1.48f, -0.10f), E(0f, 0f, 0f), V(0.44f, 0.44f, 0.44f)),
                    Part("Center Wheel", SteamMeshKey.Torus24, SteamMaterialKey.RedEnamelNeedle, V(0f, 1.58f, -0.10f), E(0f, 0f, 0f), V(0.50f, 0.50f, 0.50f)),
                    Part("Right Wheel", SteamMeshKey.Torus24, SteamMaterialKey.WarningPaint, V(0.92f, 1.48f, -0.10f), E(0f, 0f, 0f), V(0.44f, 0.44f, 0.44f))
                )),

            Piece(
                "SCD09_005_WallGaslightSconce_E",
                "Wall Gaslight Sconce E",
                SteamDressingFamily.Wall,
                "wall_upper",
                V(1.20f, 1.95f, 0.44f),
                "Amber gaslight lens caged in riveted brass with a soot-black wall bracket.",
                Tags("gaslight", "amber", "cage", "wall-light"),
                Parts(
                    Part("Iron Wall Bracket", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 1.10f, 0.04f), E(0f, 0f, 0f), V(0.70f, 1.40f, 0.12f)),
                    Part("Brass Arm", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(0f, 1.05f, -0.18f), E(90f, 0f, 0f), V(0.12f, 0.52f, 0.12f)),
                    Part("Amber Lens", SteamMeshKey.Cylinder32, SteamMaterialKey.AmberGaslightGlass, V(0f, 1.05f, -0.44f), E(0f, 0f, 0f), V(0.46f, 0.62f, 0.46f)),
                    Part("Top Cage Ring", SteamMeshKey.Torus24, SteamMaterialKey.AgedBrass, V(0f, 1.38f, -0.44f), E(90f, 0f, 0f), V(0.58f, 0.58f, 0.58f)),
                    Part("Bottom Cage Ring", SteamMeshKey.Torus24, SteamMaterialKey.AgedBrass, V(0f, 0.72f, -0.44f), E(90f, 0f, 0f), V(0.58f, 0.58f, 0.58f)),
                    Part("Left Cage Bar", SteamMeshKey.Cylinder16, SteamMaterialKey.OilyIron, V(-0.25f, 1.05f, -0.44f), E(0f, 0f, 0f), V(0.035f, 0.70f, 0.035f)),
                    Part("Right Cage Bar", SteamMeshKey.Cylinder16, SteamMaterialKey.OilyIron, V(0.25f, 1.05f, -0.44f), E(0f, 0f, 0f), V(0.035f, 0.70f, 0.035f))
                )),

            Piece(
                "SCD09_006_WallCableBracketRail_F",
                "Wall Cable Bracket Rail F",
                SteamDressingFamily.Wall,
                "wall_mid",
                V(3.60f, 1.30f, 0.28f),
                "Dark rubber cable loom held by brass brackets and iron wall tabs.",
                Tags("cable", "bracket", "wall-utility", "rubber"),
                Parts(
                    Part("Iron Mounting Strip", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.70f, 0.05f), E(0f, 0f, 0f), V(3.30f, 0.20f, 0.10f)),
                    Part("Upper Cable", SteamMeshKey.Cylinder16, SteamMaterialKey.DarkCableRubber, V(0f, 0.86f, -0.07f), E(0f, 0f, 90f), V(0.09f, 3.18f, 0.09f)),
                    Part("Lower Cable", SteamMeshKey.Cylinder16, SteamMaterialKey.DarkCableRubber, V(0.12f, 0.58f, -0.07f), E(0f, 0f, 90f), V(0.08f, 2.72f, 0.08f)),
                    Part("Left Brass Bracket", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(-1.12f, 0.72f, -0.12f), E(0f, 0f, 0f), V(0.16f, 0.56f, 0.14f)),
                    Part("Center Brass Bracket", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(0f, 0.72f, -0.12f), E(0f, 0f, 0f), V(0.16f, 0.56f, 0.14f)),
                    Part("Right Brass Bracket", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(1.12f, 0.72f, -0.12f), E(0f, 0f, 0f), V(0.16f, 0.56f, 0.14f))
                )),

            Piece(
                "SCD09_007_FloorWetDrainPlate_A",
                "Floor Wet Drain Plate A",
                SteamDressingFamily.Floor,
                "floor_center",
                V(2.40f, 0.18f, 1.30f),
                "Wet black stone drain plate with brass rim and dark slotted runoff.",
                Tags("floor-drain", "wet-floor", "stone", "brass-rim"),
                Parts(
                    Part("Wet Stone Plate", SteamMeshKey.Box, SteamMaterialKey.WetBlackStone, V(0f, 0.04f, 0f), E(0f, 0f, 0f), V(2.40f, 0.08f, 1.30f)),
                    Part("Brass Drain Rim", SteamMeshKey.Torus32, SteamMaterialKey.AgedBrass, V(0f, 0.105f, 0f), E(90f, 0f, 0f), V(0.82f, 0.82f, 0.82f)),
                    Part("Drain Darkness", SteamMeshKey.Cylinder32, SteamMaterialKey.OilyIron, V(0f, 0.112f, 0f), E(0f, 0f, 0f), V(0.60f, 0.04f, 0.60f)),
                    Part("Left Runoff Slot", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(-0.74f, 0.115f, 0f), E(0f, 0f, 0f), V(0.52f, 0.035f, 0.13f)),
                    Part("Right Runoff Slot", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0.74f, 0.115f, 0f), E(0f, 0f, 0f), V(0.52f, 0.035f, 0.13f))
                )),

            Piece(
                "SCD09_008_FloorGrateChannel_B",
                "Floor Grate Channel B",
                SteamDressingFamily.Floor,
                "floor_strip",
                V(3.60f, 0.22f, 0.82f),
                "Modular iron floor grate channel with wet gaps and warning-painted edge tabs.",
                Tags("floor-grate", "wet-floor", "iron", "warning"),
                Parts(
                    Part("Wet Recess", SteamMeshKey.Box, SteamMaterialKey.WetBlackStone, V(0f, 0.02f, 0f), E(0f, 0f, 0f), V(3.60f, 0.06f, 0.82f)),
                    Part("Left Edge Rail", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.12f, -0.34f), E(0f, 0f, 0f), V(3.60f, 0.12f, 0.08f)),
                    Part("Right Edge Rail", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.12f, 0.34f), E(0f, 0f, 0f), V(3.60f, 0.12f, 0.08f)),
                    Part("Grate Slat 01", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(-1.38f, 0.15f, 0f), E(0f, 0f, 0f), V(0.10f, 0.12f, 0.70f)),
                    Part("Grate Slat 02", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(-0.82f, 0.15f, 0f), E(0f, 0f, 0f), V(0.10f, 0.12f, 0.70f)),
                    Part("Grate Slat 03", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(-0.26f, 0.15f, 0f), E(0f, 0f, 0f), V(0.10f, 0.12f, 0.70f)),
                    Part("Grate Slat 04", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0.30f, 0.15f, 0f), E(0f, 0f, 0f), V(0.10f, 0.12f, 0.70f)),
                    Part("Grate Slat 05", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0.86f, 0.15f, 0f), E(0f, 0f, 0f), V(0.10f, 0.12f, 0.70f)),
                    Part("Grate Slat 06", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(1.42f, 0.15f, 0f), E(0f, 0f, 0f), V(0.10f, 0.12f, 0.70f)),
                    Part("Warning Edge Tab", SteamMeshKey.Box, SteamMaterialKey.WarningPaint, V(-1.70f, 0.19f, -0.43f), E(0f, 0f, 0f), V(0.40f, 0.05f, 0.06f))
                )),

            Piece(
                "SCD09_009_FloorSteamVentLow_C",
                "Floor Steam Vent Low C",
                SteamDressingFamily.Floor,
                "floor_corner",
                V(1.60f, 0.56f, 1.15f),
                "Low iron steam vent cap with brass bolts and pale mist volume marker.",
                Tags("steam-vent", "floor", "iron", "mist-marker"),
                Parts(
                    Part("Iron Base Box", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.16f, 0f), E(0f, 0f, 0f), V(1.40f, 0.32f, 0.92f)),
                    Part("Vent Drum", SteamMeshKey.Cylinder24, SteamMaterialKey.AgedBrass, V(0f, 0.42f, 0f), E(90f, 0f, 90f), V(0.40f, 0.92f, 0.40f)),
                    Part("Front Mouth", SteamMeshKey.Cylinder24, SteamMaterialKey.OilyIron, V(0f, 0.42f, -0.50f), E(90f, 0f, 0f), V(0.52f, 0.10f, 0.52f)),
                    Part("Mist Volume Marker", SteamMeshKey.Cylinder16, SteamMaterialKey.SteamMist, V(0f, 0.55f, -0.68f), E(90f, 0f, 0f), V(0.34f, 0.56f, 0.34f))
                )),

            Piece(
                "SCD09_010_FloorHandrailStanchion_D",
                "Floor Handrail Stanchion D",
                SteamDressingFamily.Floor,
                "floor_edge",
                V(2.40f, 1.22f, 0.42f),
                "Short modular brass handrail with oily iron foot plates for corridor edges.",
                Tags("handrail", "floor-edge", "brass", "safety"),
                Parts(
                    Part("Left Foot Plate", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(-0.92f, 0.04f, 0f), E(0f, 0f, 0f), V(0.34f, 0.08f, 0.36f)),
                    Part("Right Foot Plate", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0.92f, 0.04f, 0f), E(0f, 0f, 0f), V(0.34f, 0.08f, 0.36f)),
                    Part("Left Post", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(-0.92f, 0.56f, 0f), E(0f, 0f, 0f), V(0.11f, 1.04f, 0.11f)),
                    Part("Right Post", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(0.92f, 0.56f, 0f), E(0f, 0f, 0f), V(0.11f, 1.04f, 0.11f)),
                    Part("Top Rail", SteamMeshKey.Cylinder24, SteamMaterialKey.AgedBrass, V(0f, 1.10f, 0f), E(0f, 0f, 90f), V(0.13f, 2.08f, 0.13f)),
                    Part("Middle Rail", SteamMeshKey.Cylinder16, SteamMaterialKey.BurnishedCopper, V(0f, 0.72f, 0f), E(0f, 0f, 90f), V(0.09f, 1.86f, 0.09f))
                )),

            Piece(
                "SCD09_011_FloorInspectionHatch_E",
                "Floor Inspection Hatch E",
                SteamDressingFamily.Floor,
                "floor_center",
                V(1.80f, 0.18f, 1.80f),
                "Square riveted inspection hatch with brass pull ring and oily seams.",
                Tags("inspection-hatch", "floor", "rivet", "brass-ring"),
                Parts(
                    Part("Hatch Plate", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.06f, 0f), E(0f, 0f, 0f), V(1.64f, 0.12f, 1.64f)),
                    Part("Stone Reveal", SteamMeshKey.Box, SteamMaterialKey.WetBlackStone, V(0f, 0.02f, 0f), E(0f, 0f, 0f), V(1.80f, 0.04f, 1.80f)),
                    Part("Brass Pull Ring", SteamMeshKey.Torus24, SteamMaterialKey.AgedBrass, V(0f, 0.145f, 0f), E(90f, 0f, 0f), V(0.54f, 0.54f, 0.54f)),
                    Part("Warning Paint Tab", SteamMeshKey.Box, SteamMaterialKey.WarningPaint, V(0.62f, 0.15f, -0.62f), E(0f, 45f, 0f), V(0.52f, 0.035f, 0.08f))
                )),

            Piece(
                "SCD09_012_CeilingPipeCluster_A",
                "Ceiling Pipe Cluster A",
                SteamDressingFamily.Ceiling,
                "ceiling_center",
                V(3.80f, 0.72f, 1.20f),
                "Dense ceiling pipe bundle with alternating brass, copper, and iron runs.",
                Tags("ceiling-pipes", "pipe-cluster", "brass", "copper"),
                Parts(
                    Part("Mounting Rail", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, -0.05f, 0f), E(0f, 0f, 0f), V(3.80f, 0.12f, 0.24f)),
                    Part("Brass Pipe Run", SteamMeshKey.Cylinder24, SteamMaterialKey.AgedBrass, V(0f, -0.22f, -0.34f), E(0f, 0f, 90f), V(0.16f, 3.60f, 0.16f)),
                    Part("Copper Pipe Run", SteamMeshKey.Cylinder24, SteamMaterialKey.BurnishedCopper, V(0f, -0.34f, 0f), E(0f, 0f, 90f), V(0.13f, 3.42f, 0.13f)),
                    Part("Iron Pipe Run", SteamMeshKey.Cylinder16, SteamMaterialKey.OilyIron, V(0f, -0.18f, 0.32f), E(0f, 0f, 90f), V(0.12f, 3.76f, 0.12f)),
                    Part("Left Ceiling Clamp", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(-1.22f, -0.25f, 0f), E(0f, 0f, 0f), V(0.14f, 0.48f, 0.98f)),
                    Part("Right Ceiling Clamp", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(1.22f, -0.25f, 0f), E(0f, 0f, 0f), V(0.14f, 0.48f, 0.98f))
                )),

            Piece(
                "SCD09_013_CeilingCondensateTray_B",
                "Ceiling Condensate Tray B",
                SteamDressingFamily.Ceiling,
                "ceiling_edge",
                V(2.80f, 0.44f, 0.78f),
                "Shallow blackened tray under sweating pipes with greened copper drip points.",
                Tags("condensate", "ceiling", "wet", "verdigris"),
                Parts(
                    Part("Tray Pan", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, -0.08f, 0f), E(0f, 0f, 0f), V(2.80f, 0.10f, 0.78f)),
                    Part("Left Tray Lip", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(0f, -0.02f, -0.42f), E(0f, 0f, 0f), V(2.80f, 0.16f, 0.08f)),
                    Part("Right Tray Lip", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(0f, -0.02f, 0.42f), E(0f, 0f, 0f), V(2.80f, 0.16f, 0.08f)),
                    Part("Drip Pipe", SteamMeshKey.Cylinder16, SteamMaterialKey.VerdigrisCopper, V(-0.74f, -0.20f, 0f), E(0f, 0f, 0f), V(0.08f, 0.34f, 0.08f)),
                    Part("Short Drip Pipe", SteamMeshKey.Cylinder16, SteamMaterialKey.VerdigrisCopper, V(0.80f, -0.16f, 0f), E(0f, 0f, 0f), V(0.07f, 0.24f, 0.07f))
                )),

            Piece(
                "SCD09_014_CeilingLampCage_C",
                "Ceiling Lamp Cage C",
                SteamDressingFamily.Ceiling,
                "ceiling_center",
                V(1.12f, 1.24f, 1.12f),
                "Hanging amber cage lamp for smoky corridor beats and threshold reveals.",
                Tags("ceiling-lamp", "gaslight", "amber", "cage"),
                Parts(
                    Part("Ceiling Cap", SteamMeshKey.Cylinder24, SteamMaterialKey.OilyIron, V(0f, 0f, 0f), E(0f, 0f, 0f), V(0.48f, 0.12f, 0.48f)),
                    Part("Hanging Stem", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(0f, -0.34f, 0f), E(0f, 0f, 0f), V(0.08f, 0.58f, 0.08f)),
                    Part("Amber Globe", SteamMeshKey.Cylinder32, SteamMaterialKey.AmberGaslightGlass, V(0f, -0.78f, 0f), E(0f, 0f, 0f), V(0.52f, 0.56f, 0.52f)),
                    Part("Upper Cage Ring", SteamMeshKey.Torus24, SteamMaterialKey.AgedBrass, V(0f, -0.46f, 0f), E(90f, 0f, 0f), V(0.72f, 0.72f, 0.72f)),
                    Part("Lower Cage Ring", SteamMeshKey.Torus24, SteamMaterialKey.AgedBrass, V(0f, -1.10f, 0f), E(90f, 0f, 0f), V(0.72f, 0.72f, 0.72f)),
                    Part("Cage Bar A", SteamMeshKey.Cylinder16, SteamMaterialKey.OilyIron, V(-0.30f, -0.78f, 0f), E(0f, 0f, 0f), V(0.04f, 0.64f, 0.04f)),
                    Part("Cage Bar B", SteamMeshKey.Cylinder16, SteamMaterialKey.OilyIron, V(0.30f, -0.78f, 0f), E(0f, 0f, 0f), V(0.04f, 0.64f, 0.04f))
                )),

            Piece(
                "SCD09_015_CeilingCableLoom_D",
                "Ceiling Cable Loom D",
                SteamDressingFamily.Ceiling,
                "ceiling_edge",
                V(3.50f, 0.40f, 0.72f),
                "Bundled dark cable loom with brass ceiling saddles for industrial density.",
                Tags("ceiling-cable", "loom", "bracket", "rubber"),
                Parts(
                    Part("Cable A", SteamMeshKey.Cylinder16, SteamMaterialKey.DarkCableRubber, V(0f, -0.10f, -0.18f), E(0f, 0f, 90f), V(0.08f, 3.38f, 0.08f)),
                    Part("Cable B", SteamMeshKey.Cylinder16, SteamMaterialKey.DarkCableRubber, V(0f, -0.16f, 0.02f), E(0f, 0f, 90f), V(0.07f, 3.18f, 0.07f)),
                    Part("Cable C", SteamMeshKey.Cylinder16, SteamMaterialKey.DarkCableRubber, V(0f, -0.09f, 0.22f), E(0f, 0f, 90f), V(0.06f, 3.42f, 0.06f)),
                    Part("Left Saddle", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(-1.18f, -0.08f, 0.02f), E(0f, 0f, 0f), V(0.14f, 0.26f, 0.62f)),
                    Part("Center Saddle", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(0f, -0.08f, 0.02f), E(0f, 0f, 0f), V(0.14f, 0.26f, 0.62f)),
                    Part("Right Saddle", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(1.18f, -0.08f, 0.02f), E(0f, 0f, 0f), V(0.14f, 0.26f, 0.62f))
                )),

            Piece(
                "SCD09_016_CeilingSteamNozzle_E",
                "Ceiling Steam Nozzle E",
                SteamDressingFamily.Ceiling,
                "ceiling_spot",
                V(0.90f, 0.72f, 0.90f),
                "Small ceiling nozzle with copper collar and visible mist placeholder cone.",
                Tags("steam-nozzle", "ceiling", "copper", "mist-marker"),
                Parts(
                    Part("Ceiling Mount", SteamMeshKey.Cylinder24, SteamMaterialKey.OilyIron, V(0f, -0.02f, 0f), E(0f, 0f, 0f), V(0.58f, 0.10f, 0.58f)),
                    Part("Copper Collar", SteamMeshKey.Cylinder24, SteamMaterialKey.BurnishedCopper, V(0f, -0.14f, 0f), E(0f, 0f, 0f), V(0.36f, 0.20f, 0.36f)),
                    Part("Brass Nozzle", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(0f, -0.35f, 0f), E(0f, 0f, 0f), V(0.20f, 0.30f, 0.20f)),
                    Part("Mist Marker", SteamMeshKey.Cylinder16, SteamMaterialKey.SteamMist, V(0f, -0.58f, 0f), E(0f, 0f, 0f), V(0.34f, 0.40f, 0.34f))
                )),

            Piece(
                "SCD09_017_DoorwayRivetedHeader_A",
                "Doorway Riveted Header A",
                SteamDressingFamily.Doorway,
                "doorway_top",
                V(3.40f, 0.72f, 0.38f),
                "Riveted iron doorway lintel with brass pressure strip and black stone return.",
                Tags("doorway", "header", "rivet", "iron"),
                Parts(
                    Part("Black Stone Header Backer", SteamMeshKey.Box, SteamMaterialKey.WetBlackStone, V(0f, 0.36f, 0.08f), E(0f, 0f, 0f), V(3.40f, 0.72f, 0.12f)),
                    Part("Iron Lintel", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.36f, -0.04f), E(0f, 0f, 0f), V(3.16f, 0.34f, 0.18f)),
                    Part("Brass Pressure Strip", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(0f, 0.18f, -0.16f), E(0f, 0f, 0f), V(2.88f, 0.08f, 0.10f)),
                    Part("Left Rivet", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(-1.26f, 0.44f, -0.16f), E(90f, 0f, 0f), V(0.12f, 0.05f, 0.12f)),
                    Part("Mid Rivet", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(0f, 0.44f, -0.16f), E(90f, 0f, 0f), V(0.12f, 0.05f, 0.12f)),
                    Part("Right Rivet", SteamMeshKey.Cylinder16, SteamMaterialKey.AgedBrass, V(1.26f, 0.44f, -0.16f), E(90f, 0f, 0f), V(0.12f, 0.05f, 0.12f))
                )),

            Piece(
                "SCD09_018_DoorwayPressureLockValve_B",
                "Doorway Pressure Lock Valve B",
                SteamDressingFamily.Doorway,
                "doorway_side",
                V(1.10f, 2.40f, 0.40f),
                "Door-side pressure lock with gauge, red handwheel, and copper side feed.",
                Tags("doorway", "pressure-lock", "valve", "gauge"),
                Parts(
                    Part("Side Plate", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 1.16f, 0.05f), E(0f, 0f, 0f), V(0.90f, 2.20f, 0.12f)),
                    Part("Vertical Copper Feed", SteamMeshKey.Cylinder16, SteamMaterialKey.BurnishedCopper, V(-0.36f, 1.16f, -0.07f), E(0f, 0f, 0f), V(0.10f, 1.86f, 0.10f)),
                    Part("Gauge Face", SteamMeshKey.Cylinder32, SteamMaterialKey.GaugeIvory, V(0.14f, 1.58f, -0.11f), E(90f, 0f, 0f), V(0.38f, 0.05f, 0.38f)),
                    Part("Gauge Rim", SteamMeshKey.Torus24, SteamMaterialKey.AgedBrass, V(0.14f, 1.58f, -0.15f), E(0f, 0f, 0f), V(0.52f, 0.52f, 0.52f)),
                    Part("Needle", SteamMeshKey.Box, SteamMaterialKey.RedEnamelNeedle, V(0.18f, 1.61f, -0.18f), E(0f, 0f, 22f), V(0.025f, 0.28f, 0.03f)),
                    Part("Red Lock Wheel", SteamMeshKey.Torus24, SteamMaterialKey.RedEnamelNeedle, V(0.12f, 0.78f, -0.12f), E(0f, 0f, 0f), V(0.50f, 0.50f, 0.50f))
                )),

            Piece(
                "SCD09_019_DoorwayThresholdDrain_C",
                "Doorway Threshold Drain C",
                SteamDressingFamily.Doorway,
                "doorway_floor",
                V(3.20f, 0.20f, 0.72f),
                "Wet brass-and-iron threshold drain strip for pressure-door bases.",
                Tags("doorway", "threshold", "floor-drain", "wet"),
                Parts(
                    Part("Wet Threshold Bed", SteamMeshKey.Box, SteamMaterialKey.WetBlackStone, V(0f, 0.04f, 0f), E(0f, 0f, 0f), V(3.20f, 0.08f, 0.72f)),
                    Part("Front Iron Rail", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.14f, -0.30f), E(0f, 0f, 0f), V(3.10f, 0.10f, 0.08f)),
                    Part("Back Iron Rail", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.14f, 0.30f), E(0f, 0f, 0f), V(3.10f, 0.10f, 0.08f)),
                    Part("Brass Center Strip", SteamMeshKey.Box, SteamMaterialKey.AgedBrass, V(0f, 0.16f, 0f), E(0f, 0f, 0f), V(2.80f, 0.08f, 0.12f)),
                    Part("Drain Slot Left", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(-0.88f, 0.18f, 0f), E(0f, 0f, 0f), V(0.42f, 0.04f, 0.42f)),
                    Part("Drain Slot Right", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0.88f, 0.18f, 0f), E(0f, 0f, 0f), V(0.42f, 0.04f, 0.42f))
                )),

            Piece(
                "SCD09_020_DoorwaySideTankPair_D",
                "Doorway Side Tank Pair D",
                SteamDressingFamily.Doorway,
                "doorway_side_pair",
                V(1.40f, 2.65f, 0.70f),
                "Paired side tanks for doors with strapped copper cylinders and gauge repeat.",
                Tags("doorway", "side-tank", "copper", "gauge"),
                Parts(
                    Part("Door Side Backer", SteamMeshKey.Box, SteamMaterialKey.WetBlackStone, V(0f, 1.30f, 0.10f), E(0f, 0f, 0f), V(1.22f, 2.50f, 0.12f)),
                    Part("Upper Copper Tank", SteamMeshKey.Cylinder24, SteamMaterialKey.BurnishedCopper, V(0f, 1.78f, -0.18f), E(0f, 0f, 0f), V(0.42f, 0.86f, 0.42f)),
                    Part("Lower Copper Tank", SteamMeshKey.Cylinder24, SteamMaterialKey.BurnishedCopper, V(0f, 0.76f, -0.18f), E(0f, 0f, 0f), V(0.42f, 0.70f, 0.42f)),
                    Part("Upper Strap", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 1.78f, -0.48f), E(0f, 0f, 0f), V(0.66f, 0.09f, 0.12f)),
                    Part("Lower Strap", SteamMeshKey.Box, SteamMaterialKey.OilyIron, V(0f, 0.76f, -0.48f), E(0f, 0f, 0f), V(0.66f, 0.09f, 0.12f)),
                    Part("Small Gauge Face", SteamMeshKey.Cylinder32, SteamMaterialKey.GaugeIvory, V(0.38f, 1.28f, -0.13f), E(90f, 0f, 0f), V(0.28f, 0.04f, 0.28f)),
                    Part("Small Gauge Rim", SteamMeshKey.Torus24, SteamMaterialKey.AgedBrass, V(0.38f, 1.28f, -0.16f), E(0f, 0f, 0f), V(0.38f, 0.38f, 0.38f))
                ))
        };

        public static string GetPrefabName(SteamPieceDefinition piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }

            return piece.Id.Replace("SCD09_", "SCD09_PREFAB_");
        }

        private static SteamMaterialDefinition Mat(
            SteamMaterialKey key,
            string displayName,
            Color baseColor,
            float metallic,
            float smoothness)
        {
            return Mat(key, displayName, baseColor, metallic, smoothness, Color.black, 0f);
        }

        private static SteamMaterialDefinition Mat(
            SteamMaterialKey key,
            string displayName,
            Color baseColor,
            float metallic,
            float smoothness,
            Color emissionColor,
            float emissionStrength)
        {
            return new SteamMaterialDefinition(key, displayName, baseColor, metallic, smoothness, emissionColor, emissionStrength);
        }

        private static SteamPieceDefinition Piece(
            string id,
            string displayName,
            SteamDressingFamily family,
            string anchor,
            Vector3 footprintMeters,
            string description,
            string[] tags,
            SteamPartDefinition[] parts)
        {
            return new SteamPieceDefinition(id, displayName, family, anchor, footprintMeters, description, tags, parts);
        }

        private static SteamPartDefinition Part(
            string name,
            SteamMeshKey meshKey,
            SteamMaterialKey materialKey,
            Vector3 localPosition,
            Vector3 localEulerAngles,
            Vector3 localScale)
        {
            return new SteamPartDefinition(name, meshKey, materialKey, localPosition, localEulerAngles, localScale);
        }

        private static SteamPartDefinition[] Parts(params SteamPartDefinition[] parts)
        {
            return parts;
        }

        private static string[] Tags(params string[] tags)
        {
            return tags;
        }

        private static Vector3 V(float x, float y, float z)
        {
            return new Vector3(x, y, z);
        }

        private static Vector3 E(float x, float y, float z)
        {
            return new Vector3(x, y, z);
        }

        private static Color C(float r, float g, float b)
        {
            return new Color(r, g, b, 1f);
        }
    }
}
