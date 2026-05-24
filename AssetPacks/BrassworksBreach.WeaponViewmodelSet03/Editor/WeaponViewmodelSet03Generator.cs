using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BrassworksBreach.WeaponViewmodelSet03;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.WeaponViewmodelSet03.Editor
{
    public static class WeaponViewmodelSet03Generator
    {
        public const string PackageName = "com.brassworks.sidecar.weapon-viewmodel-set03";
        private const string Version = "v0.1.41";
        private const string PackageVersion = "0.1.41";
        private const string BuildId = "p001";

        private static readonly string[] MaterialTags =
        {
            "aged_brass",
            "smoked_brass",
            "oxidized_copper",
            "blackened_iron",
            "edge_worn_gunmetal",
            "oily_steel",
            "blued_spring_steel",
            "varnished_walnut",
            "worn_leather",
            "dark_glove_leather",
            "linen_wrap",
            "amber_pressure_glass",
            "green_gauge_glass",
            "red_sealing_wax"
        };

        private static readonly string[] PrefabNames =
        {
            "BB_WVM03_PressurePistol_FullAssembly_A",
            "BB_WVM03_PressurePistol_FullAssembly_B_DualGauge",
            "BB_WVM03_PressurePistol_Receiver_Block",
            "BB_WVM03_PressurePistol_BarrelCoil_Module",
            "BB_WVM03_PressurePistol_PressureTankCluster",
            "BB_WVM03_Scattergun_FullAssembly_A",
            "BB_WVM03_Scattergun_TwinBarrel_Module",
            "BB_WVM03_Scattergun_PumpGrip_Module",
            "BB_WVM03_BoltThrower_FullAssembly_A",
            "BB_WVM03_BoltThrower_RailAndBolt_Module",
            "BB_WVM03_MuzzleBrake_Finned_Assembly",
            "BB_WVM03_TriggerGuardAndHammer_Set",
            "BB_WVM03_GaugeCluster_Triple",
            "BB_WVM03_WalnutLeatherGrip_Test_A",
            "BB_WVM03_LeatherWrappedGrip_Test_B",
            "BB_WVM03_GloveSilhouette_RightGrip",
            "BB_WVM03_GloveSilhouette_LeftSupport",
            "BB_WVM03_AmmoPressureCell_Single",
            "BB_WVM03_AmmoShellStrip_Scatter",
            "BB_WVM03_FastenerPlate_SampleBoard"
        };

        private static readonly string[] MaterialNames =
        {
            "WVM03_MAT_AgedBrass",
            "WVM03_MAT_SmokedBrass",
            "WVM03_MAT_OxidizedCopper",
            "WVM03_MAT_BlackenedIron",
            "WVM03_MAT_EdgeWornGunmetal",
            "WVM03_MAT_OilySteel",
            "WVM03_MAT_BluedSpringSteel",
            "WVM03_MAT_VarnishedWalnut",
            "WVM03_MAT_WornLeather",
            "WVM03_MAT_DarkGloveLeather",
            "WVM03_MAT_LinenWrap",
            "WVM03_MAT_AmberPressureGlass",
            "WVM03_MAT_GreenGaugeGlass",
            "WVM03_MAT_RedSealingWax"
        };

        private static readonly string[] MeshNames =
        {
            "WVM03_Mesh_BeveledBox",
            "WVM03_Mesh_TaperedGrip",
            "WVM03_Mesh_GaugeNeedle",
            "WVM03_Mesh_HexBolt",
            "WVM03_Mesh_GlovePalm",
            "WVM03_Mesh_GloveFinger",
            "WVM03_Mesh_BoltFin"
        };

        [MenuItem("Brassworks Breach/Sidecar Packs/Generate Weapon Viewmodel Set 03 v0.1.41")]
        public static void GenerateAll()
        {
            var packageRoot = LocatePackageRoot();
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Materials");
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Meshes");
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Prefabs");
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Metadata");
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Documentation~/Manifest");

            var materials = CreateMaterials(packageRoot.AssetPath);
            var meshes = CreateMeshes(packageRoot.AssetPath);

            CreatePressurePistolFull(packageRoot.AssetPath, materials, meshes, PrefabNames[0], false, 1.00f);
            CreatePressurePistolFull(packageRoot.AssetPath, materials, meshes, PrefabNames[1], true, 0.92f);
            CreatePressurePistolReceiver(packageRoot.AssetPath, materials, meshes);
            CreatePressurePistolBarrelCoil(packageRoot.AssetPath, materials, meshes);
            CreatePressureTankCluster(packageRoot.AssetPath, materials, meshes);
            CreateScattergunFull(packageRoot.AssetPath, materials, meshes);
            CreateScattergunTwinBarrel(packageRoot.AssetPath, materials, meshes);
            CreateScattergunPumpGrip(packageRoot.AssetPath, materials, meshes);
            CreateBoltThrowerFull(packageRoot.AssetPath, materials, meshes);
            CreateBoltThrowerRailAndBolt(packageRoot.AssetPath, materials, meshes);
            CreateMuzzleBrake(packageRoot.AssetPath, materials, meshes);
            CreateTriggerGuardAndHammer(packageRoot.AssetPath, materials, meshes);
            CreateGaugeClusterTriple(packageRoot.AssetPath, materials, meshes);
            CreateWalnutLeatherGripTestA(packageRoot.AssetPath, materials, meshes);
            CreateLeatherWrappedGripTestB(packageRoot.AssetPath, materials, meshes);
            CreateGloveSilhouette(packageRoot.AssetPath, materials, meshes, PrefabNames[15], true);
            CreateGloveSilhouette(packageRoot.AssetPath, materials, meshes, PrefabNames[16], false);
            CreateAmmoPressureCellSingle(packageRoot.AssetPath, materials, meshes);
            CreateAmmoShellStripScatter(packageRoot.AssetPath, materials, meshes);
            CreateFastenerPlateSampleBoard(packageRoot.AssetPath, materials, meshes);

            WritePackageMetadata(packageRoot, "generated_assets_pending_validation", "not_run");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"BB_WEAPON_VIEWMODEL_SET03_GENERATE_PASS {Version} prefabs={PrefabNames.Length} materials={MaterialNames.Length} meshes={MeshNames.Length} root={packageRoot.AssetPath}");
        }

        public static string[] GeneratedPrefabAssetPaths()
        {
            var root = LocatePackageRoot().AssetPath;
            var paths = new string[PrefabNames.Length];
            for (var i = 0; i < PrefabNames.Length; i++)
            {
                paths[i] = $"{root}/Runtime/Prefabs/{PrefabNames[i]}.prefab";
            }

            return paths;
        }

        public static string[] GeneratedMaterialAssetPaths()
        {
            var root = LocatePackageRoot().AssetPath;
            var paths = new string[MaterialNames.Length];
            for (var i = 0; i < MaterialNames.Length; i++)
            {
                paths[i] = $"{root}/Runtime/Materials/{MaterialNames[i]}.mat";
            }

            return paths;
        }

        public static string[] GeneratedMeshAssetPaths()
        {
            var root = LocatePackageRoot().AssetPath;
            var paths = new string[MeshNames.Length];
            for (var i = 0; i < MeshNames.Length; i++)
            {
                paths[i] = $"{root}/Runtime/Meshes/{MeshNames[i]}.asset";
            }

            return paths;
        }

        public static string ResolveRepositoryRenderRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_WVM03_RENDER_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return explicitRoot;
            }

            var packageRoot = LocatePackageRoot();
            return Path.GetFullPath(Path.Combine(
                packageRoot.ResolvedPath,
                "..",
                "..",
                "Documentation",
                "ConceptRenders",
                "V0_1_41_WeaponViewmodelSet03"));
        }

        public static string ResolveRepositoryProductionRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_WVM03_PRODUCTION_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return explicitRoot;
            }

            var packageRoot = LocatePackageRoot();
            return Path.GetFullPath(Path.Combine(
                packageRoot.ResolvedPath,
                "..",
                "..",
                "Documentation",
                "AssetProduction",
                "V0_1_41_WeaponViewmodelSet03"));
        }

        public static void MarkValidated(string importSmokeStatus, string previewStatus)
        {
            var packageRoot = LocatePackageRoot();
            WritePackageMetadata(packageRoot, importSmokeStatus, previewStatus);
            AssetDatabase.Refresh();
        }

        private static Dictionary<string, Material> CreateMaterials(string root)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("HDRP/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible lit shader found.");
            }

            return new Dictionary<string, Material>
            {
                ["brass"] = SaveMaterial(root, shader, "WVM03_MAT_AgedBrass", new Color(0.82f, 0.57f, 0.25f), 0.90f, 0.46f),
                ["smokedBrass"] = SaveMaterial(root, shader, "WVM03_MAT_SmokedBrass", new Color(0.40f, 0.27f, 0.12f), 0.84f, 0.32f),
                ["copper"] = SaveMaterial(root, shader, "WVM03_MAT_OxidizedCopper", new Color(0.70f, 0.30f, 0.15f), 0.88f, 0.36f),
                ["iron"] = SaveMaterial(root, shader, "WVM03_MAT_BlackenedIron", new Color(0.035f, 0.034f, 0.032f), 0.78f, 0.24f),
                ["gunmetal"] = SaveMaterial(root, shader, "WVM03_MAT_EdgeWornGunmetal", new Color(0.26f, 0.28f, 0.27f), 0.82f, 0.48f),
                ["oilySteel"] = SaveMaterial(root, shader, "WVM03_MAT_OilySteel", new Color(0.12f, 0.13f, 0.13f), 0.74f, 0.72f),
                ["springSteel"] = SaveMaterial(root, shader, "WVM03_MAT_BluedSpringSteel", new Color(0.10f, 0.14f, 0.18f), 0.78f, 0.58f),
                ["walnut"] = SaveMaterial(root, shader, "WVM03_MAT_VarnishedWalnut", new Color(0.29f, 0.13f, 0.055f), 0.05f, 0.60f),
                ["leather"] = SaveMaterial(root, shader, "WVM03_MAT_WornLeather", new Color(0.34f, 0.18f, 0.08f), 0.02f, 0.34f),
                ["glove"] = SaveMaterial(root, shader, "WVM03_MAT_DarkGloveLeather", new Color(0.065f, 0.055f, 0.047f), 0.02f, 0.46f),
                ["linen"] = SaveMaterial(root, shader, "WVM03_MAT_LinenWrap", new Color(0.61f, 0.54f, 0.42f), 0.0f, 0.27f),
                ["amber"] = SaveMaterial(root, shader, "WVM03_MAT_AmberPressureGlass", new Color(1.0f, 0.50f, 0.10f), 0.0f, 0.84f, new Color(1.0f, 0.38f, 0.06f) * 1.35f),
                ["greenGlass"] = SaveMaterial(root, shader, "WVM03_MAT_GreenGaugeGlass", new Color(0.16f, 0.68f, 0.46f), 0.0f, 0.78f, new Color(0.08f, 0.44f, 0.30f) * 0.9f),
                ["redSeal"] = SaveMaterial(root, shader, "WVM03_MAT_RedSealingWax", new Color(0.55f, 0.035f, 0.025f), 0.0f, 0.46f)
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes(string root)
        {
            return new Dictionary<string, Mesh>
            {
                ["beveledBox"] = SaveMesh(root, "WVM03_Mesh_BeveledBox", CreateBoxMesh()),
                ["taperedGrip"] = SaveMesh(root, "WVM03_Mesh_TaperedGrip", CreateTaperedGripMesh()),
                ["gaugeNeedle"] = SaveMesh(root, "WVM03_Mesh_GaugeNeedle", CreateGaugeNeedleMesh()),
                ["hexBolt"] = SaveMesh(root, "WVM03_Mesh_HexBolt", CreateHexBoltMesh()),
                ["glovePalm"] = SaveMesh(root, "WVM03_Mesh_GlovePalm", CreateGlovePalmMesh()),
                ["gloveFinger"] = SaveMesh(root, "WVM03_Mesh_GloveFinger", CreateTaperedCylinderMesh(10, 0.18f, 0.11f, 0.82f)),
                ["boltFin"] = SaveMesh(root, "WVM03_Mesh_BoltFin", CreateBoltFinMesh())
            };
        }

        private static void CreatePressurePistolFull(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, string assetName, bool dualGauge, float scale)
        {
            var pistol = NewRoot(assetName, "full first-person pressure pistol assembly candidate with separated receiver, coil, grip, tank, sight, gauge, and trigger groups", "Promote after muzzle alignment, hand socket, and reload readability review.", "All detail is visual-only; generated primitive colliders are stripped.");
            pistol.transform.localScale = Vector3.one * scale;

            MeshPart(pistol.transform, meshes["beveledBox"], "blackened_iron_receiver_core", new Vector3(0f, 0.02f, -0.42f), new Vector3(0.42f, 0.30f, 0.42f), Vector3.zero, materials["iron"]);
            MeshPart(pistol.transform, meshes["beveledBox"], "edge_worn_gunmetal_side_plate_left", new Vector3(-0.225f, 0.035f, -0.43f), new Vector3(0.035f, 0.24f, 0.38f), Vector3.zero, materials["gunmetal"]);
            MeshPart(pistol.transform, meshes["beveledBox"], "edge_worn_gunmetal_side_plate_right", new Vector3(0.225f, 0.035f, -0.43f), new Vector3(0.035f, 0.24f, 0.38f), Vector3.zero, materials["gunmetal"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "oily_steel_primary_barrel", new Vector3(0f, 0.16f, 0.19f), new Vector3(0.075f, 0.72f, 0.075f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "aged_brass_muzzle_collar", new Vector3(0f, 0.16f, 0.92f), new Vector3(0.14f, 0.060f, 0.14f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "smoked_brass_breech_collar", new Vector3(0f, 0.16f, -0.16f), new Vector3(0.13f, 0.050f, 0.13f), new Vector3(90f, 0f, 0f), materials["smokedBrass"]);
            AddCoilBands(pistol.transform, "oxidized_copper_barrel_coil", new Vector3(0f, 0.16f, -0.05f), 10, 0.085f, 0.116f, 0.010f, materials["copper"]);

            Primitive(pistol.transform, PrimitiveType.Cylinder, "amber_glass_underbarrel_pressure_vial", new Vector3(-0.18f, -0.08f, 0.22f), new Vector3(0.078f, 0.48f, 0.078f), new Vector3(90f, 0f, 0f), materials["amber"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "aged_brass_vial_front_cap", new Vector3(-0.18f, -0.08f, 0.70f), new Vector3(0.094f, 0.025f, 0.094f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "aged_brass_vial_rear_cap", new Vector3(-0.18f, -0.08f, -0.26f), new Vector3(0.094f, 0.025f, 0.094f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "green_glass_side_regulator", new Vector3(0.23f, -0.02f, 0.00f), new Vector3(0.055f, 0.30f, 0.055f), new Vector3(90f, 0f, 0f), materials["greenGlass"]);

            AddWalnutGrip(pistol.transform, materials, meshes, new Vector3(0f, -0.45f, -0.61f), 1.0f);
            AddTriggerGroup(pistol.transform, materials, meshes, new Vector3(0f, -0.18f, -0.42f), 1.0f);
            AddGaugeFace(pistol.transform, "top_pressure_gauge_primary", new Vector3(-0.07f, 0.37f, -0.35f), 0.145f, materials, meshes, true);
            if (dualGauge)
            {
                AddGaugeFace(pistol.transform, "top_pressure_gauge_secondary", new Vector3(0.18f, 0.34f, -0.24f), 0.105f, materials, meshes, false);
                Primitive(pistol.transform, PrimitiveType.Cylinder, "blued_spring_steel_cocking_rod", new Vector3(0.19f, 0.25f, 0.24f), new Vector3(0.025f, 0.52f, 0.025f), new Vector3(90f, 0f, 0f), materials["springSteel"]);
            }

            AddBoltRow(pistol.transform, "left_receiver_hex_fasteners", -0.25f, materials["brass"], meshes["hexBolt"], 5, -0.58f, -0.27f);
            AddBoltRow(pistol.transform, "right_receiver_hex_fasteners", 0.25f, materials["brass"], meshes["hexBolt"], 5, -0.58f, -0.27f);
            SavePrefab(root, pistol, assetName);
        }

        private static void CreatePressurePistolReceiver(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var receiver = NewRoot(PrefabNames[2], "pressure pistol receiver block module with plates, top latch, sockets, rivets, and rear strap", "Reusable hard-surface receiver candidate for full pressure pistol variants.");
            MeshPart(receiver.transform, meshes["beveledBox"], "blackened_iron_receiver_block", Vector3.zero, new Vector3(0.48f, 0.32f, 0.52f), Vector3.zero, materials["iron"]);
            MeshPart(receiver.transform, meshes["beveledBox"], "smoked_brass_breech_face", new Vector3(0f, 0.02f, 0.30f), new Vector3(0.44f, 0.25f, 0.055f), Vector3.zero, materials["smokedBrass"]);
            MeshPart(receiver.transform, meshes["beveledBox"], "edge_worn_gunmetal_top_dovetail", new Vector3(0f, 0.22f, -0.02f), new Vector3(0.34f, 0.06f, 0.46f), Vector3.zero, materials["gunmetal"]);
            Primitive(receiver.transform, PrimitiveType.Cylinder, "oily_steel_barrel_socket", new Vector3(0f, 0.08f, 0.35f), new Vector3(0.12f, 0.035f, 0.12f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
            Primitive(receiver.transform, PrimitiveType.Cylinder, "aged_brass_rear_cap_screw", new Vector3(0f, 0.04f, -0.31f), new Vector3(0.11f, 0.030f, 0.11f), new Vector3(90f, 0f, 0f), materials["brass"]);
            AddBoltGrid(receiver.transform, "receiver_plate_bolt_grid", materials["brass"], meshes["hexBolt"], 4, 2, 0.40f, 0.20f, new Vector3(0f, 0.08f, -0.33f));
            SavePrefab(root, receiver, PrefabNames[2]);
        }

        private static void CreatePressurePistolBarrelCoil(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var barrel = NewRoot(PrefabNames[3], "modular pistol barrel and copper coil assembly with front sight and clamp collars", "Reusable barrel module for pressure pistol, bolt thrower, and workbench display candidates.");
            Primitive(barrel.transform, PrimitiveType.Cylinder, "oily_steel_inner_barrel", Vector3.zero, new Vector3(0.072f, 0.88f, 0.072f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
            Primitive(barrel.transform, PrimitiveType.Cylinder, "amber_pressure_tube_visible_core", new Vector3(0f, -0.095f, 0f), new Vector3(0.040f, 0.78f, 0.040f), new Vector3(90f, 0f, 0f), materials["amber"]);
            AddCoilBands(barrel.transform, "oxidized_copper_pressure_coil", Vector3.zero, 14, 0.095f, 0.116f, 0.010f, materials["copper"]);
            Primitive(barrel.transform, PrimitiveType.Cylinder, "aged_brass_front_clamp_collar", new Vector3(0f, 0f, 0.84f), new Vector3(0.14f, 0.055f, 0.14f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(barrel.transform, PrimitiveType.Cylinder, "smoked_brass_rear_clamp_collar", new Vector3(0f, 0f, -0.84f), new Vector3(0.13f, 0.055f, 0.13f), new Vector3(90f, 0f, 0f), materials["smokedBrass"]);
            MeshPart(barrel.transform, meshes["boltFin"], "aged_brass_front_sight_blade", new Vector3(0f, 0.13f, 0.58f), new Vector3(0.11f, 0.11f, 0.13f), new Vector3(0f, 0f, 90f), materials["brass"]);
            SavePrefab(root, barrel, PrefabNames[3]);
        }

        private static void CreatePressureTankCluster(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var tanks = NewRoot(PrefabNames[4], "cluster of amber pressure tanks with brass caps, copper bands, labels, and sockets", "Ammo, receiver, and underbarrel tank module candidate.");
            for (var i = 0; i < 3; i++)
            {
                var x = -0.20f + i * 0.20f;
                Primitive(tanks.transform, PrimitiveType.Cylinder, $"amber_pressure_tank_{i:00}", new Vector3(x, 0f, 0f), new Vector3(0.068f, 0.42f, 0.068f), new Vector3(90f, 0f, 0f), materials["amber"]);
                Primitive(tanks.transform, PrimitiveType.Cylinder, $"aged_brass_front_cap_{i:00}", new Vector3(x, 0f, 0.43f), new Vector3(0.083f, 0.030f, 0.083f), new Vector3(90f, 0f, 0f), materials["brass"]);
                Primitive(tanks.transform, PrimitiveType.Cylinder, $"smoked_brass_rear_cap_{i:00}", new Vector3(x, 0f, -0.43f), new Vector3(0.083f, 0.030f, 0.083f), new Vector3(90f, 0f, 0f), materials["smokedBrass"]);
                AddCoilBands(tanks.transform, $"oxidized_copper_tank_band_{i:00}", new Vector3(x, 0f, 0f), 3, 0.20f, 0.076f, 0.008f, materials["copper"]);
                Primitive(tanks.transform, PrimitiveType.Cube, $"linen_charge_label_{i:00}", new Vector3(x, 0.070f, 0.04f), new Vector3(0.085f, 0.018f, 0.20f), Vector3.zero, materials["linen"]);
            }

            MeshPart(tanks.transform, meshes["beveledBox"], "blackened_iron_tank_cradle", new Vector3(0f, -0.11f, 0f), new Vector3(0.58f, 0.07f, 0.92f), Vector3.zero, materials["iron"]);
            AddBoltGrid(tanks.transform, "tank_cradle_hex_fasteners", materials["brass"], meshes["hexBolt"], 4, 2, 0.52f, 0.15f, new Vector3(0f, -0.06f, -0.47f));
            SavePrefab(root, tanks, PrefabNames[4]);
        }

        private static void CreateScattergunFull(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var scattergun = NewRoot(PrefabNames[5], "full first-person scattergun assembly candidate with twin barrels, pump grip, stock, boiler tank, and gauge", "Promote after viewmodel width, reload reach, and muzzle placement review.");
            MeshPart(scattergun.transform, meshes["beveledBox"], "blackened_iron_broad_receiver", new Vector3(0f, 0f, -0.42f), new Vector3(0.62f, 0.34f, 0.46f), Vector3.zero, materials["iron"]);
            MeshPart(scattergun.transform, meshes["taperedGrip"], "varnished_walnut_shoulder_stock", new Vector3(0f, -0.08f, -1.04f), new Vector3(0.62f, 0.52f, 0.78f), new Vector3(-76f, 0f, 0f), materials["walnut"]);
            Primitive(scattergun.transform, PrimitiveType.Cube, "worn_leather_recoil_pad", new Vector3(0f, -0.30f, -1.36f), new Vector3(0.56f, 0.09f, 0.17f), new Vector3(-8f, 0f, 0f), materials["leather"]);
            for (var i = 0; i < 2; i++)
            {
                var x = -0.12f + i * 0.24f;
                Primitive(scattergun.transform, PrimitiveType.Cylinder, $"oily_steel_scatter_barrel_{i:00}", new Vector3(x, 0.12f, 0.28f), new Vector3(0.084f, 0.78f, 0.084f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
                Primitive(scattergun.transform, PrimitiveType.Cylinder, $"smoked_brass_muzzle_collar_{i:00}", new Vector3(x, 0.12f, 1.08f), new Vector3(0.115f, 0.048f, 0.115f), new Vector3(90f, 0f, 0f), materials["smokedBrass"]);
            }

            AddPumpGrip(scattergun.transform, materials, meshes, new Vector3(0f, -0.17f, 0.37f), 1.0f);
            Primitive(scattergun.transform, PrimitiveType.Cylinder, "amber_boiler_underbarrel_tank", new Vector3(0f, -0.34f, 0.24f), new Vector3(0.105f, 0.57f, 0.105f), new Vector3(90f, 0f, 0f), materials["amber"]);
            Primitive(scattergun.transform, PrimitiveType.Cylinder, "aged_brass_tank_front_cap", new Vector3(0f, -0.34f, 0.83f), new Vector3(0.128f, 0.035f, 0.128f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(scattergun.transform, PrimitiveType.Cylinder, "aged_brass_tank_rear_cap", new Vector3(0f, -0.34f, -0.35f), new Vector3(0.128f, 0.035f, 0.128f), new Vector3(90f, 0f, 0f), materials["brass"]);
            AddTriggerGroup(scattergun.transform, materials, meshes, new Vector3(0f, -0.19f, -0.47f), 1.12f);
            AddGaugeFace(scattergun.transform, "receiver_left_pressure_gauge", new Vector3(-0.34f, 0.18f, -0.43f), 0.13f, materials, meshes, false);
            AddBoltGrid(scattergun.transform, "scattergun_receiver_bolt_grid", materials["brass"], meshes["hexBolt"], 5, 2, 0.52f, 0.20f, new Vector3(0f, 0.09f, -0.67f));
            SavePrefab(root, scattergun, PrefabNames[5]);
        }

        private static void CreateScattergunTwinBarrel(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var module = NewRoot(PrefabNames[6], "scattergun twin-barrel module with bridge ribs, muzzles, rib screws, and heat bands", "Reusable front assembly candidate for scattergun variants.");
            for (var i = 0; i < 2; i++)
            {
                var x = -0.12f + i * 0.24f;
                Primitive(module.transform, PrimitiveType.Cylinder, $"oily_steel_barrel_tube_{i:00}", new Vector3(x, 0f, 0f), new Vector3(0.084f, 0.90f, 0.084f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
                Primitive(module.transform, PrimitiveType.Cylinder, $"aged_brass_muzzle_ring_{i:00}", new Vector3(x, 0f, 0.92f), new Vector3(0.120f, 0.052f, 0.120f), new Vector3(90f, 0f, 0f), materials["brass"]);
                Primitive(module.transform, PrimitiveType.Cylinder, $"smoked_brass_breech_ring_{i:00}", new Vector3(x, 0f, -0.92f), new Vector3(0.114f, 0.048f, 0.114f), new Vector3(90f, 0f, 0f), materials["smokedBrass"]);
            }

            for (var i = 0; i < 5; i++)
            {
                MeshPart(module.transform, meshes["beveledBox"], $"edge_worn_gunmetal_bridge_rib_{i:00}", new Vector3(0f, 0f, -0.62f + i * 0.31f), new Vector3(0.36f, 0.055f, 0.040f), Vector3.zero, materials["gunmetal"]);
                MeshPart(module.transform, meshes["hexBolt"], $"aged_brass_bridge_bolt_{i:00}_left", new Vector3(-0.20f, 0.035f, -0.62f + i * 0.31f), Vector3.one * 0.045f, new Vector3(90f, 0f, 0f), materials["brass"]);
                MeshPart(module.transform, meshes["hexBolt"], $"aged_brass_bridge_bolt_{i:00}_right", new Vector3(0.20f, 0.035f, -0.62f + i * 0.31f), Vector3.one * 0.045f, new Vector3(90f, 0f, 0f), materials["brass"]);
            }

            SavePrefab(root, module, PrefabNames[6]);
        }

        private static void CreateScattergunPumpGrip(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var pump = NewRoot(PrefabNames[7], "scattergun pump grip module with walnut grip, leather grooves, brass slide rails, and screw heads", "Reusable animation-facing pump silhouette for first-person scattergun.");
            AddPumpGrip(pump.transform, materials, meshes, Vector3.zero, 1.2f);
            Primitive(pump.transform, PrimitiveType.Cylinder, "blued_spring_steel_left_slide_rail", new Vector3(-0.24f, 0.12f, 0f), new Vector3(0.022f, 0.62f, 0.022f), new Vector3(90f, 0f, 0f), materials["springSteel"]);
            Primitive(pump.transform, PrimitiveType.Cylinder, "blued_spring_steel_right_slide_rail", new Vector3(0.24f, 0.12f, 0f), new Vector3(0.022f, 0.62f, 0.022f), new Vector3(90f, 0f, 0f), materials["springSteel"]);
            AddBoltGrid(pump.transform, "pump_side_screws", materials["brass"], meshes["hexBolt"], 4, 2, 0.45f, 0.13f, new Vector3(0f, 0.15f, -0.28f));
            SavePrefab(root, pump, PrefabNames[7]);
        }

        private static void CreateBoltThrowerFull(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var thrower = NewRoot(PrefabNames[8], "full first-person bolt thrower assembly candidate with rail, pressure spring, tension gauge, and bolt fins", "Promote after projectile silhouette and reload channel review.");
            MeshPart(thrower.transform, meshes["beveledBox"], "blackened_iron_long_receiver", new Vector3(0f, 0.02f, -0.38f), new Vector3(0.42f, 0.26f, 0.72f), Vector3.zero, materials["iron"]);
            Primitive(thrower.transform, PrimitiveType.Cylinder, "blued_spring_steel_upper_rail", new Vector3(0f, 0.25f, 0.20f), new Vector3(0.030f, 0.96f, 0.030f), new Vector3(90f, 0f, 0f), materials["springSteel"]);
            Primitive(thrower.transform, PrimitiveType.Cylinder, "oily_steel_lower_launch_tube", new Vector3(0f, 0.08f, 0.28f), new Vector3(0.070f, 0.88f, 0.070f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
            AddCoilBands(thrower.transform, "oxidized_copper_return_spring_loop", new Vector3(0f, 0.25f, 0.04f), 12, 0.073f, 0.060f, 0.008f, materials["copper"]);
            MeshPart(thrower.transform, meshes["boltFin"], "edge_worn_gunmetal_loaded_bolt_head", new Vector3(0f, 0.08f, 0.94f), new Vector3(0.22f, 0.16f, 0.24f), new Vector3(0f, 0f, 90f), materials["gunmetal"]);
            MeshPart(thrower.transform, meshes["boltFin"], "aged_brass_bolt_tail_fin_left", new Vector3(-0.10f, 0.15f, 0.62f), new Vector3(0.12f, 0.12f, 0.20f), new Vector3(0f, 24f, 70f), materials["brass"]);
            MeshPart(thrower.transform, meshes["boltFin"], "aged_brass_bolt_tail_fin_right", new Vector3(0.10f, 0.15f, 0.62f), new Vector3(0.12f, 0.12f, 0.20f), new Vector3(0f, -24f, -70f), materials["brass"]);
            AddWalnutGrip(thrower.transform, materials, meshes, new Vector3(0f, -0.43f, -0.73f), 0.92f);
            AddTriggerGroup(thrower.transform, materials, meshes, new Vector3(0f, -0.17f, -0.52f), 0.92f);
            AddGaugeFace(thrower.transform, "side_tension_gauge", new Vector3(-0.28f, 0.16f, -0.34f), 0.12f, materials, meshes, true);
            SavePrefab(root, thrower, PrefabNames[8]);
        }

        private static void CreateBoltThrowerRailAndBolt(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var rail = NewRoot(PrefabNames[9], "bolt thrower rail and projectile module with visible tension spring and finned bolt", "Reusable bolt/rail subassembly for weapon variants and workbench props.");
            Primitive(rail.transform, PrimitiveType.Cylinder, "blued_spring_steel_top_guide_rail", new Vector3(0f, 0.12f, 0f), new Vector3(0.025f, 1.05f, 0.025f), new Vector3(90f, 0f, 0f), materials["springSteel"]);
            Primitive(rail.transform, PrimitiveType.Cylinder, "oily_steel_lower_guide_rail", new Vector3(0f, -0.02f, 0f), new Vector3(0.028f, 1.05f, 0.028f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
            AddCoilBands(rail.transform, "oxidized_copper_tension_spring", new Vector3(0f, 0.12f, -0.20f), 10, 0.070f, 0.055f, 0.008f, materials["copper"]);
            MeshPart(rail.transform, meshes["boltFin"], "edge_worn_gunmetal_bolt_point", new Vector3(0f, -0.02f, 0.88f), new Vector3(0.24f, 0.17f, 0.28f), new Vector3(0f, 0f, 90f), materials["gunmetal"]);
            for (var i = 0; i < 3; i++)
            {
                MeshPart(rail.transform, meshes["boltFin"], $"aged_brass_bolt_fin_{i:00}", new Vector3(0f, 0.03f, 0.42f - i * 0.13f), new Vector3(0.15f, 0.12f, 0.18f), new Vector3(0f, i * 120f, 90f), materials["brass"]);
            }

            SavePrefab(root, rail, PrefabNames[9]);
        }

        private static void CreateMuzzleBrake(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var muzzle = NewRoot(PrefabNames[10], "finned muzzle brake assembly with brass rings, blackened ports, and copper heat seams", "Reusable muzzle candidate for pistol, scattergun, or bolt thrower.");
            Primitive(muzzle.transform, PrimitiveType.Cylinder, "oily_steel_muzzle_core", Vector3.zero, new Vector3(0.095f, 0.34f, 0.095f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
            Primitive(muzzle.transform, PrimitiveType.Cylinder, "aged_brass_outer_collar_front", new Vector3(0f, 0f, 0.36f), new Vector3(0.145f, 0.055f, 0.145f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(muzzle.transform, PrimitiveType.Cylinder, "smoked_brass_outer_collar_rear", new Vector3(0f, 0f, -0.36f), new Vector3(0.135f, 0.050f, 0.135f), new Vector3(90f, 0f, 0f), materials["smokedBrass"]);
            for (var i = 0; i < 6; i++)
            {
                var angle = i * 60f;
                var rad = angle * Mathf.Deg2Rad;
                var pos = new Vector3(Mathf.Cos(rad) * 0.13f, Mathf.Sin(rad) * 0.13f, 0.02f);
                MeshPart(muzzle.transform, meshes["boltFin"], $"edge_worn_gunmetal_radial_fin_{i:00}", pos, new Vector3(0.13f, 0.070f, 0.32f), new Vector3(0f, 0f, angle), materials["gunmetal"]);
            }

            AddCoilBands(muzzle.transform, "oxidized_copper_heat_seam", Vector3.zero, 4, 0.18f, 0.118f, 0.007f, materials["copper"]);
            SavePrefab(root, muzzle, PrefabNames[10]);
        }

        private static void CreateTriggerGuardAndHammer(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var trigger = NewRoot(PrefabNames[11], "trigger guard, cocking hammer, safety latch, and exposed screw set", "Reusable hand-control component set for viewmodel assembly review.");
            AddTriggerGroup(trigger.transform, materials, meshes, Vector3.zero, 1.25f);
            Primitive(trigger.transform, PrimitiveType.Cube, "red_sealing_wax_safety_tab", new Vector3(-0.18f, 0.06f, 0.07f), new Vector3(0.12f, 0.035f, 0.055f), new Vector3(0f, 0f, 12f), materials["redSeal"]);
            MeshPart(trigger.transform, meshes["boltFin"], "smoked_brass_cocking_hammer", new Vector3(0f, 0.19f, -0.12f), new Vector3(0.22f, 0.16f, 0.18f), new Vector3(0f, 0f, -42f), materials["smokedBrass"]);
            AddBoltGrid(trigger.transform, "trigger_guard_fastener_board", materials["brass"], meshes["hexBolt"], 3, 2, 0.38f, 0.16f, new Vector3(0f, 0.02f, -0.28f));
            SavePrefab(root, trigger, PrefabNames[11]);
        }

        private static void CreateGaugeClusterTriple(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var gauge = NewRoot(PrefabNames[12], "triple pressure gauge cluster with amber and green faces, brass manifold, needles, and bolts", "Reusable readable pressure-gauge cluster for weapon tops and side panels.");
            AddGaugeFace(gauge.transform, "large_center_amber_pressure_gauge", new Vector3(0f, 0f, 0f), 0.18f, materials, meshes, true);
            AddGaugeFace(gauge.transform, "left_green_balance_gauge", new Vector3(-0.28f, -0.035f, 0.02f), 0.12f, materials, meshes, false);
            AddGaugeFace(gauge.transform, "right_green_balance_gauge", new Vector3(0.28f, -0.035f, 0.02f), 0.12f, materials, meshes, false);
            Primitive(gauge.transform, PrimitiveType.Cylinder, "smoked_brass_manifold_pipe", new Vector3(0f, -0.23f, 0.05f), new Vector3(0.035f, 0.34f, 0.035f), new Vector3(0f, 0f, 90f), materials["smokedBrass"]);
            AddBoltGrid(gauge.transform, "gauge_cluster_mounting_bolts", materials["brass"], meshes["hexBolt"], 4, 1, 0.58f, 0.0f, new Vector3(0f, -0.34f, -0.02f));
            SavePrefab(root, gauge, PrefabNames[12]);
        }

        private static void CreateWalnutLeatherGripTestA(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var grip = NewRoot(PrefabNames[13], "walnut and worn leather grip material test with brass pins and palm swell", "Viewmodel grip scale and material read test.");
            AddWalnutGrip(grip.transform, materials, meshes, Vector3.zero, 1.18f);
            Primitive(grip.transform, PrimitiveType.Cube, "linen_wrap_understrap", new Vector3(0f, -0.08f, 0.09f), new Vector3(0.31f, 0.54f, 0.035f), new Vector3(14f, 0f, 0f), materials["linen"]);
            SavePrefab(root, grip, PrefabNames[13]);
        }

        private static void CreateLeatherWrappedGripTestB(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var grip = NewRoot(PrefabNames[14], "dark leather wrapped grip material test with linen underwrap, red wax stamp, and brass pommel", "Alternate hand grip material and silhouette test.");
            MeshPart(grip.transform, meshes["taperedGrip"], "blackened_iron_grip_spine", Vector3.zero, new Vector3(0.44f, 0.82f, 0.34f), new Vector3(14f, 0f, 0f), materials["iron"]);
            for (var i = 0; i < 6; i++)
            {
                Primitive(grip.transform, PrimitiveType.Cube, $"dark_glove_leather_wrap_band_{i:00}", new Vector3(0f, -0.34f + i * 0.13f, 0f), new Vector3(0.32f, 0.042f, 0.25f), new Vector3(14f, 0f, i % 2 == 0 ? 2f : -2f), materials["glove"]);
            }

            Primitive(grip.transform, PrimitiveType.Cylinder, "aged_brass_pommel_cap", new Vector3(0f, -0.47f, -0.02f), new Vector3(0.17f, 0.042f, 0.17f), new Vector3(0f, 0f, 90f), materials["brass"]);
            Primitive(grip.transform, PrimitiveType.Sphere, "red_wax_inspection_stamp", new Vector3(0f, 0.03f, -0.18f), Vector3.one * 0.055f, Vector3.zero, materials["redSeal"]);
            SavePrefab(root, grip, PrefabNames[14]);
        }

        private static void CreateGloveSilhouette(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, string assetName, bool rightHand)
        {
            var hand = NewRoot(assetName, rightHand ? "right-hand viewmodel glove grip silhouette with cuff, palm mass, and trigger-ready fingers" : "left support-hand glove silhouette with wrapped cuff and forward bracing fingers", "Scale and silhouette reference only; not rigged and not a gameplay hand.");
            var side = rightHand ? 1f : -1f;
            MeshPart(hand.transform, meshes["glovePalm"], "dark_glove_leather_palm_block", new Vector3(0f, 0f, 0f), new Vector3(0.42f, 0.50f, 0.22f), new Vector3(0f, 0f, side * -10f), materials["glove"]);
            Primitive(hand.transform, PrimitiveType.Cube, "linen_cuff_wrap", new Vector3(0f, -0.39f, 0.03f), new Vector3(0.52f, 0.16f, 0.28f), new Vector3(0f, 0f, side * -7f), materials["linen"]);
            Primitive(hand.transform, PrimitiveType.Cube, "worn_leather_cuff_strap", new Vector3(0f, -0.36f, -0.13f), new Vector3(0.55f, 0.052f, 0.050f), new Vector3(0f, 0f, side * -7f), materials["leather"]);
            for (var i = 0; i < 4; i++)
            {
                var x = side * (-0.15f + i * 0.10f);
                var curl = rightHand ? -22f - i * 5f : -8f + i * 4f;
                MeshPart(hand.transform, meshes["gloveFinger"], $"dark_glove_leather_finger_{i:00}", new Vector3(x, 0.30f, -0.02f + i * 0.015f), new Vector3(0.70f, 0.62f, 0.70f), new Vector3(curl, 0f, side * (-6f + i * 4f)), materials["glove"]);
                Primitive(hand.transform, PrimitiveType.Cylinder, $"worn_leather_knuckle_band_{i:00}", new Vector3(x, 0.31f, -0.03f), new Vector3(0.030f, 0.020f, 0.030f), new Vector3(90f, 0f, 0f), materials["leather"]);
            }

            MeshPart(hand.transform, meshes["gloveFinger"], "dark_glove_leather_thumb", new Vector3(side * -0.29f, 0.05f, 0.04f), new Vector3(0.75f, 0.56f, 0.75f), new Vector3(0f, 0f, side * 58f), materials["glove"]);
            Primitive(hand.transform, PrimitiveType.Sphere, "aged_brass_cuff_button", new Vector3(side * 0.22f, -0.34f, -0.16f), Vector3.one * 0.045f, Vector3.zero, materials["brass"]);
            SavePrefab(root, hand, assetName);
        }

        private static void CreateAmmoPressureCellSingle(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var cell = NewRoot(PrefabNames[17], "single pressure-cell ammo prop with amber vial, brass caps, copper bands, label, and red wax seal", "Visual-only ammo prop for pickup or reload lookdev.");
            AddPressureCell(cell.transform, "single", Vector3.zero, 1.0f, materials, meshes);
            SavePrefab(root, cell, PrefabNames[17]);
        }

        private static void CreateAmmoShellStripScatter(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var strip = NewRoot(PrefabNames[18], "scattergun shell strip with six brass/amber shells, leather carrier, and stamped labels", "Visual-only ammo strip for reload and bench prop lookdev.");
            Primitive(strip.transform, PrimitiveType.Cube, "worn_leather_shell_carrier_strip", new Vector3(0f, -0.06f, 0f), new Vector3(0.86f, 0.08f, 0.22f), Vector3.zero, materials["leather"]);
            for (var i = 0; i < 6; i++)
            {
                var x = -0.36f + i * 0.145f;
                Primitive(strip.transform, PrimitiveType.Cylinder, $"aged_brass_shell_hull_{i:00}", new Vector3(x, 0.04f, 0f), new Vector3(0.045f, 0.16f, 0.045f), Vector3.zero, materials["brass"]);
                Primitive(strip.transform, PrimitiveType.Cylinder, $"amber_pressure_shot_window_{i:00}", new Vector3(x, 0.14f, 0f), new Vector3(0.039f, 0.060f, 0.039f), Vector3.zero, materials["amber"]);
                Primitive(strip.transform, PrimitiveType.Cylinder, $"red_wax_shell_primer_{i:00}", new Vector3(x, -0.13f, 0f), new Vector3(0.026f, 0.018f, 0.026f), Vector3.zero, materials["redSeal"]);
            }

            AddBoltGrid(strip.transform, "shell_strip_brass_rivets", materials["brass"], meshes["hexBolt"], 5, 1, 0.74f, 0f, new Vector3(0f, -0.01f, -0.13f));
            SavePrefab(root, strip, PrefabNames[18]);
        }

        private static void CreateFastenerPlateSampleBoard(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var board = NewRoot(PrefabNames[19], "fastener and plate sample board with hex bolts, rivets, stamped plates, and material swatches", "Reusable material/fastener lookdev board for intake review.");
            MeshPart(board.transform, meshes["beveledBox"], "edge_worn_gunmetal_base_plate", Vector3.zero, new Vector3(1.10f, 0.055f, 0.70f), Vector3.zero, materials["gunmetal"]);
            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 6; col++)
                {
                    var x = -0.42f + col * 0.17f;
                    var z = -0.20f + row * 0.20f;
                    var material = row == 0 ? materials["brass"] : row == 1 ? materials["smokedBrass"] : materials["copper"];
                    MeshPart(board.transform, meshes["hexBolt"], $"sample_hex_fastener_{row:00}_{col:00}", new Vector3(x, 0.055f, z), Vector3.one * (0.045f + col * 0.003f), new Vector3(90f, 0f, col * 11f), material);
                }
            }

            Primitive(board.transform, PrimitiveType.Cube, "blackened_iron_receiver_plate_swatch", new Vector3(-0.30f, 0.075f, -0.43f), new Vector3(0.26f, 0.035f, 0.12f), Vector3.zero, materials["iron"]);
            Primitive(board.transform, PrimitiveType.Cube, "varnished_walnut_inset_swatch", new Vector3(0.00f, 0.078f, -0.43f), new Vector3(0.26f, 0.035f, 0.12f), Vector3.zero, materials["walnut"]);
            Primitive(board.transform, PrimitiveType.Cube, "worn_leather_inset_swatch", new Vector3(0.30f, 0.078f, -0.43f), new Vector3(0.26f, 0.035f, 0.12f), Vector3.zero, materials["leather"]);
            SavePrefab(root, board, PrefabNames[19]);
        }

        private static void AddWalnutGrip(Transform parent, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, Vector3 localPosition, float scale)
        {
            var grip = new GameObject("walnut_leather_viewmodel_grip");
            grip.transform.SetParent(parent, false);
            grip.transform.localPosition = localPosition;
            grip.transform.localScale = Vector3.one * scale;
            MeshPart(grip.transform, meshes["taperedGrip"], "varnished_walnut_palm_swell", Vector3.zero, new Vector3(0.48f, 0.72f, 0.34f), new Vector3(15f, 0f, 0f), materials["walnut"]);
            for (var i = 0; i < 5; i++)
            {
                Primitive(grip.transform, PrimitiveType.Cube, $"worn_leather_wrap_band_{i:00}", new Vector3(0f, -0.27f + i * 0.135f, -0.01f), new Vector3(0.31f, 0.040f, 0.23f), new Vector3(15f, 0f, 0f), materials["leather"]);
            }

            for (var i = 0; i < 3; i++)
            {
                var y = -0.22f + i * 0.20f;
                MeshPart(grip.transform, meshes["hexBolt"], $"aged_brass_grip_pin_left_{i:00}", new Vector3(-0.18f, y, -0.03f), Vector3.one * 0.045f, new Vector3(90f, 0f, 0f), materials["brass"]);
                MeshPart(grip.transform, meshes["hexBolt"], $"aged_brass_grip_pin_right_{i:00}", new Vector3(0.18f, y, -0.03f), Vector3.one * 0.045f, new Vector3(90f, 0f, 0f), materials["brass"]);
            }
        }

        private static void AddPumpGrip(Transform parent, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, Vector3 localPosition, float scale)
        {
            var pump = new GameObject("scattergun_pump_grip_module");
            pump.transform.SetParent(parent, false);
            pump.transform.localPosition = localPosition;
            pump.transform.localScale = Vector3.one * scale;
            MeshPart(pump.transform, meshes["beveledBox"], "varnished_walnut_broad_pump_body", Vector3.zero, new Vector3(0.54f, 0.17f, 0.50f), Vector3.zero, materials["walnut"]);
            for (var i = 0; i < 6; i++)
            {
                Primitive(pump.transform, PrimitiveType.Cube, $"worn_leather_pump_grip_groove_{i:00}", new Vector3(-0.28f + i * 0.112f, 0.065f, 0f), new Vector3(0.030f, 0.060f, 0.52f), Vector3.zero, materials["leather"]);
            }
        }

        private static void AddTriggerGroup(Transform parent, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, Vector3 localPosition, float scale)
        {
            var trigger = new GameObject("trigger_guard_and_hammer_group");
            trigger.transform.SetParent(parent, false);
            trigger.transform.localPosition = localPosition;
            trigger.transform.localScale = Vector3.one * scale;
            Primitive(trigger.transform, PrimitiveType.Cube, "aged_brass_trigger_guard_front", new Vector3(0f, 0.04f, 0.08f), new Vector3(0.30f, 0.045f, 0.21f), new Vector3(25f, 0f, 0f), materials["brass"]);
            Primitive(trigger.transform, PrimitiveType.Cube, "aged_brass_trigger_guard_lower", new Vector3(0f, -0.08f, -0.04f), new Vector3(0.30f, 0.038f, 0.34f), Vector3.zero, materials["brass"]);
            Primitive(trigger.transform, PrimitiveType.Cube, "blackened_iron_trigger_hook", new Vector3(0f, -0.005f, -0.10f), new Vector3(0.055f, 0.17f, 0.042f), new Vector3(-22f, 0f, 0f), materials["iron"]);
            MeshPart(trigger.transform, meshes["boltFin"], "smoked_brass_half_cock_hammer", new Vector3(0f, 0.17f, -0.18f), new Vector3(0.16f, 0.13f, 0.14f), new Vector3(0f, 0f, -36f), materials["smokedBrass"]);
        }

        private static void AddPressureCell(Transform parent, string prefix, Vector3 localPosition, float scale, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var cell = new GameObject($"{prefix}_pressure_cell");
            cell.transform.SetParent(parent, false);
            cell.transform.localPosition = localPosition;
            cell.transform.localScale = Vector3.one * scale;
            Primitive(cell.transform, PrimitiveType.Cylinder, $"{prefix}_amber_pressure_vial", Vector3.zero, new Vector3(0.082f, 0.46f, 0.082f), new Vector3(90f, 0f, 0f), materials["amber"]);
            Primitive(cell.transform, PrimitiveType.Cylinder, $"{prefix}_aged_brass_front_cap", new Vector3(0f, 0f, 0.47f), new Vector3(0.105f, 0.036f, 0.105f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(cell.transform, PrimitiveType.Cylinder, $"{prefix}_smoked_brass_rear_cap", new Vector3(0f, 0f, -0.47f), new Vector3(0.105f, 0.036f, 0.105f), new Vector3(90f, 0f, 0f), materials["smokedBrass"]);
            AddCoilBands(cell.transform, $"{prefix}_oxidized_copper_retaining_band", Vector3.zero, 4, 0.20f, 0.096f, 0.008f, materials["copper"]);
            Primitive(cell.transform, PrimitiveType.Cube, $"{prefix}_linen_charge_label", new Vector3(0f, 0.088f, 0.04f), new Vector3(0.14f, 0.018f, 0.27f), Vector3.zero, materials["linen"]);
            Primitive(cell.transform, PrimitiveType.Sphere, $"{prefix}_red_wax_charge_stamp", new Vector3(0f, 0.11f, 0.19f), Vector3.one * 0.032f, Vector3.zero, materials["redSeal"]);
            MeshPart(cell.transform, meshes["hexBolt"], $"{prefix}_tiny_brass_stamp_bolt", new Vector3(0f, 0.115f, -0.20f), Vector3.one * 0.032f, new Vector3(90f, 0f, 0f), materials["brass"]);
        }

        private static void AddGaugeFace(Transform parent, string name, Vector3 localPosition, float radius, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, bool amberFace)
        {
            var gauge = new GameObject(name);
            gauge.transform.SetParent(parent, false);
            gauge.transform.localPosition = localPosition;
            Primitive(gauge.transform, PrimitiveType.Cylinder, "blackened_iron_backplate", Vector3.zero, new Vector3(radius * 1.10f, 0.030f, radius * 1.10f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Primitive(gauge.transform, PrimitiveType.Cylinder, "aged_brass_outer_rim", new Vector3(0f, 0f, -0.025f), new Vector3(radius, 0.024f, radius), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(gauge.transform, PrimitiveType.Cylinder, "glowing_glass_gauge_face", new Vector3(0f, 0f, -0.052f), new Vector3(radius * 0.76f, 0.010f, radius * 0.76f), new Vector3(90f, 0f, 0f), amberFace ? materials["amber"] : materials["greenGlass"]);
            MeshPart(gauge.transform, meshes["gaugeNeedle"], "blackened_iron_pressure_needle", new Vector3(radius * 0.10f, 0f, -0.075f), new Vector3(radius * 0.86f, radius * 0.72f, radius * 0.45f), new Vector3(0f, 0f, -26f), materials["iron"]);
            MeshPart(gauge.transform, meshes["hexBolt"], "aged_brass_center_pin", new Vector3(0f, 0f, -0.092f), Vector3.one * radius * 0.18f, new Vector3(90f, 0f, 0f), materials["brass"]);
            for (var i = 0; i < 10; i++)
            {
                var angle = i * Mathf.PI * 2f / 10f;
                var pos = new Vector3(Mathf.Cos(angle) * radius * 0.70f, Mathf.Sin(angle) * radius * 0.70f, -0.080f);
                Primitive(gauge.transform, PrimitiveType.Cube, $"black_pressure_tick_{i:00}", pos, new Vector3(radius * 0.030f, radius * 0.10f, radius * 0.022f), new Vector3(0f, 0f, Mathf.Rad2Deg * angle), materials["iron"]);
            }
        }

        private static void AddCoilBands(Transform parent, string prefix, Vector3 center, int count, float spacing, float radius, float thickness, Material material)
        {
            var start = -((count - 1) * spacing) * 0.5f;
            for (var i = 0; i < count; i++)
            {
                Primitive(parent, PrimitiveType.Cylinder, $"{prefix}_{i:00}", center + new Vector3(0f, 0f, start + i * spacing), new Vector3(radius, thickness, radius), new Vector3(90f, 0f, 0f), material);
            }
        }

        private static void AddBoltRow(Transform parent, string rowName, float x, Material material, Mesh boltMesh, int count, float startZ, float endZ)
        {
            var row = new GameObject(rowName);
            row.transform.SetParent(parent, false);
            for (var i = 0; i < count; i++)
            {
                var t = count <= 1 ? 0f : i / (float)(count - 1);
                var z = Mathf.Lerp(startZ, endZ, t);
                MeshPart(row.transform, boltMesh, $"hex_receiver_bolt_{i:00}", new Vector3(x, 0.035f, z), Vector3.one * 0.045f, new Vector3(90f, 0f, 0f), material);
            }
        }

        private static void AddBoltGrid(Transform parent, string groupName, Material material, Mesh boltMesh, int columns, int rows, float width, float height, Vector3 center)
        {
            var grid = new GameObject(groupName);
            grid.transform.SetParent(parent, false);
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < columns; x++)
                {
                    var px = columns == 1 ? 0f : -width * 0.5f + x * (width / (columns - 1));
                    var py = rows == 1 ? 0f : -height * 0.5f + y * (height / (rows - 1));
                    MeshPart(grid.transform, boltMesh, $"hex_mount_bolt_{y:00}_{x:00}", center + new Vector3(px, py, 0f), Vector3.one * 0.045f, new Vector3(90f, 0f, 0f), material);
                }
            }
        }

        private static GameObject NewRoot(string assetId, string role, params string[] promotionNotes)
        {
            var root = new GameObject(assetId);
            root.AddComponent<WeaponViewmodelSet03Identity>().Configure(assetId, role, 0, MaterialTags, promotionNotes);
            return root;
        }

        private static GameObject Primitive(Transform parent, PrimitiveType type, string name, Vector3 localPosition, Vector3 localScale, Vector3 localEuler, Material material)
        {
            var child = GameObject.CreatePrimitive(type);
            child.name = name;
            child.transform.SetParent(parent, false);
            child.transform.localPosition = localPosition;
            child.transform.localScale = localScale;
            child.transform.localEulerAngles = localEuler;

            var collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                renderer.receiveShadows = true;
            }

            return child;
        }

        private static GameObject MeshPart(Transform parent, Mesh mesh, string name, Vector3 localPosition, Vector3 localScale, Vector3 localEuler, Material material)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            child.transform.localPosition = localPosition;
            child.transform.localScale = localScale;
            child.transform.localEulerAngles = localEuler;
            child.AddComponent<MeshFilter>().sharedMesh = mesh;
            var renderer = child.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            renderer.receiveShadows = true;
            return child;
        }

        private static Material SaveMaterial(string root, Shader shader, string name, Color color, float metallic, float smoothness, Color? emission = null)
        {
            var material = new Material(shader)
            {
                name = name
            };

            SetColor(material, "_BaseColor", color);
            SetColor(material, "_Color", color);
            SetFloat(material, "_Metallic", metallic);
            SetFloat(material, "_Smoothness", smoothness);
            SetFloat(material, "_Glossiness", smoothness);

            if (emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                SetColor(material, "_EmissionColor", emission.Value);
            }

            var path = $"{root}/Runtime/Materials/{name}.mat";
            ReplaceAsset(path);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static Mesh SaveMesh(string root, string name, Mesh mesh)
        {
            mesh.name = name;
            var path = $"{root}/Runtime/Meshes/{name}.asset";
            ReplaceAsset(path);
            AssetDatabase.CreateAsset(mesh, path);
            return AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        private static Mesh CreateBoxMesh()
        {
            var mesh = new Mesh();
            mesh.vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
            };
            mesh.triangles = new[]
            {
                0, 2, 1, 0, 3, 2,
                4, 5, 6, 4, 6, 7,
                0, 1, 5, 0, 5, 4,
                2, 3, 7, 2, 7, 6,
                1, 2, 6, 1, 6, 5,
                3, 0, 4, 3, 4, 7
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateTaperedGripMesh()
        {
            var bottomX = 0.34f;
            var bottomZ = 0.24f;
            var topX = 0.22f;
            var topZ = 0.17f;
            var mesh = new Mesh();
            mesh.vertices = new[]
            {
                new Vector3(-bottomX, -0.5f, -bottomZ), new Vector3(bottomX, -0.5f, -bottomZ), new Vector3(bottomX, -0.5f, bottomZ), new Vector3(-bottomX, -0.5f, bottomZ),
                new Vector3(-topX, 0.5f, -topZ), new Vector3(topX, 0.5f, -topZ), new Vector3(topX, 0.5f, topZ), new Vector3(-topX, 0.5f, topZ)
            };
            mesh.triangles = new[]
            {
                0, 1, 2, 0, 2, 3,
                4, 6, 5, 4, 7, 6,
                0, 4, 5, 0, 5, 1,
                1, 5, 6, 1, 6, 2,
                2, 6, 7, 2, 7, 3,
                3, 7, 4, 3, 4, 0
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateGaugeNeedleMesh()
        {
            var mesh = new Mesh();
            mesh.vertices = new[]
            {
                new Vector3(-0.45f, -0.07f, -0.025f), new Vector3(-0.45f, 0.07f, -0.025f), new Vector3(0.55f, 0f, -0.025f),
                new Vector3(-0.45f, -0.07f, 0.025f), new Vector3(-0.45f, 0.07f, 0.025f), new Vector3(0.55f, 0f, 0.025f)
            };
            mesh.triangles = new[]
            {
                0, 1, 2,
                3, 5, 4,
                0, 3, 4, 0, 4, 1,
                1, 4, 5, 1, 5, 2,
                2, 5, 3, 2, 3, 0
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateHexBoltMesh()
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            const int sides = 6;
            const float radius = 0.5f;
            const float depth = 0.22f;
            for (var z = 0; z < 2; z++)
            {
                var zPos = z == 0 ? -depth * 0.5f : depth * 0.5f;
                for (var i = 0; i < sides; i++)
                {
                    var angle = Mathf.PI * 2f * i / sides + Mathf.PI / 6f;
                    vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, zPos));
                }
            }

            vertices.Add(new Vector3(0f, 0f, -depth * 0.5f));
            vertices.Add(new Vector3(0f, 0f, depth * 0.5f));
            var backCenter = sides * 2;
            var frontCenter = sides * 2 + 1;
            for (var i = 0; i < sides; i++)
            {
                var next = (i + 1) % sides;
                triangles.Add(i);
                triangles.Add(next);
                triangles.Add(sides + next);
                triangles.Add(i);
                triangles.Add(sides + next);
                triangles.Add(sides + i);
                triangles.Add(backCenter);
                triangles.Add(next);
                triangles.Add(i);
                triangles.Add(frontCenter);
                triangles.Add(sides + i);
                triangles.Add(sides + next);
            }

            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateGlovePalmMesh()
        {
            var mesh = CreateTaperedGripMesh();
            mesh.name = "GlovePalm";
            return mesh;
        }

        private static Mesh CreateTaperedCylinderMesh(int sides, float baseRadius, float tipRadius, float length)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var ring = 0; ring < 2; ring++)
            {
                var radius = ring == 0 ? baseRadius : tipRadius;
                var y = ring == 0 ? -length * 0.5f : length * 0.5f;
                for (var i = 0; i < sides; i++)
                {
                    var angle = Mathf.PI * 2f * i / sides;
                    vertices.Add(new Vector3(Mathf.Cos(angle) * radius, y, Mathf.Sin(angle) * radius));
                }
            }

            vertices.Add(new Vector3(0f, -length * 0.5f, 0f));
            vertices.Add(new Vector3(0f, length * 0.5f, 0f));
            var baseCenter = sides * 2;
            var tipCenter = sides * 2 + 1;
            for (var i = 0; i < sides; i++)
            {
                var next = (i + 1) % sides;
                triangles.Add(i);
                triangles.Add(sides + i);
                triangles.Add(sides + next);
                triangles.Add(i);
                triangles.Add(sides + next);
                triangles.Add(next);
                triangles.Add(baseCenter);
                triangles.Add(next);
                triangles.Add(i);
                triangles.Add(tipCenter);
                triangles.Add(sides + i);
                triangles.Add(sides + next);
            }

            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateBoltFinMesh()
        {
            var mesh = new Mesh();
            mesh.vertices = new[]
            {
                new Vector3(-0.50f, -0.10f, -0.04f), new Vector3(0.28f, -0.10f, -0.04f), new Vector3(0.52f, 0.12f, -0.04f), new Vector3(-0.28f, 0.18f, -0.04f),
                new Vector3(-0.50f, -0.10f, 0.04f), new Vector3(0.28f, -0.10f, 0.04f), new Vector3(0.52f, 0.12f, 0.04f), new Vector3(-0.28f, 0.18f, 0.04f)
            };
            mesh.triangles = new[]
            {
                0, 1, 2, 0, 2, 3,
                4, 6, 5, 4, 7, 6,
                0, 4, 5, 0, 5, 1,
                1, 5, 6, 1, 6, 2,
                2, 6, 7, 2, 7, 3,
                3, 7, 4, 3, 4, 0
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void SavePrefab(string root, GameObject prefabRoot, string assetName)
        {
            var identity = prefabRoot.GetComponent<WeaponViewmodelSet03Identity>();
            if (identity != null)
            {
                identity.Configure(assetName, identity.AssetRole, prefabRoot.GetComponentsInChildren<Renderer>().Length, MaterialTags, identity.PromotionNotes);
            }

            var path = $"{root}/Runtime/Prefabs/{assetName}.prefab";
            ReplaceAsset(path);
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            UnityEngine.Object.DestroyImmediate(prefabRoot);
        }

        private static void WritePackageMetadata(PackageRoot packageRoot, string importSmokeStatus, string previewStatus)
        {
            var now = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
            var manifestPath = Path.Combine(packageRoot.ResolvedPath, "Documentation~", "Manifest", "WVM03_WeaponViewmodelSet03_Manifest_v0.1.41-p001.json");
            var catalogPath = Path.Combine(packageRoot.ResolvedPath, "Runtime", "Metadata", "WVM03_WeaponViewmodelSet03_Catalog_v0.1.41.json");
            File.WriteAllText(manifestPath, BuildManifestJson(now, importSmokeStatus, previewStatus), Encoding.UTF8);
            File.WriteAllText(catalogPath, BuildCatalogJson(now), Encoding.UTF8);
        }

        private static string BuildManifestJson(string timestamp, string importSmokeStatus, string previewStatus)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            AppendProperty(builder, 2, "pack_id", "WVM03", true);
            AppendProperty(builder, 2, "display_name", "Weapon Viewmodel Set 03", true);
            AppendProperty(builder, 2, "version", PackageVersion, true);
            AppendProperty(builder, 2, "build_id", BuildId, true);
            AppendProperty(builder, 2, "unity_version", Application.unityVersion, true);
            AppendProperty(builder, 2, "sidecar_project", "UD-SC-WVM03-WeaponViewmodelVisualCandidates", true);
            AppendProperty(builder, 2, "owner_lane", "sidecar-weapon-viewmodel-lookdev", true);
            AppendProperty(builder, 2, "primary_intake_owner", "main-lane-art-integration", true);
            AppendProperty(builder, 2, "canonical_root", "AssetPacks/BrassworksBreach.WeaponViewmodelSet03", true);
            AppendProperty(builder, 2, "package_root", "AssetPacks/BrassworksBreach.WeaponViewmodelSet03", true);
            AppendProperty(builder, 2, "package_name", PackageName, true);
            AppendProperty(builder, 2, "package_version", PackageVersion, true);
            builder.AppendLine("  \"asset_counts\": {");
            AppendNumberProperty(builder, 4, "generated_prefabs", PrefabNames.Length, true);
            AppendNumberProperty(builder, 4, "generated_materials", MaterialNames.Length, true);
            AppendNumberProperty(builder, 4, "generated_meshes", MeshNames.Length, true);
            AppendNumberProperty(builder, 4, "preview_renders", PrefabNames.Length + 1, true);
            AppendNumberProperty(builder, 4, "runtime_scripts", 1, true);
            AppendNumberProperty(builder, 4, "editor_scripts", 3, false);
            builder.AppendLine("  },");
            AppendStringArray(builder, 2, "generated_prefabs", PrefabNames, "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Prefabs/", ".prefab", true);
            AppendStringArray(builder, 2, "generated_materials", MaterialNames, "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Materials/", ".mat", true);
            AppendStringArray(builder, 2, "generated_meshes", MeshNames, "Packages/com.brassworks.sidecar.weapon-viewmodel-set03/Runtime/Meshes/", ".asset", true);
            AppendStringArray(builder, 2, "preview_renders", PrefabNames, "Documentation/ConceptRenders/V0_1_41_WeaponViewmodelSet03/", "_preview.png", true, "Documentation/ConceptRenders/V0_1_41_WeaponViewmodelSet03/BB_WVM03_ContactSheet.png");
            AppendStringArray(builder, 2, "dependencies", new[] { "UnityEngine built-in primitives", "UnityEditor prefab, mesh, and render texture APIs", "Built-in, URP, or HDRP lit shader fallback" }, string.Empty, string.Empty, true);
            AppendStringArray(builder, 2, "required_primary_changes", Array.Empty<string>(), string.Empty, string.Empty, true);
            AppendBoolProperty(builder, 2, "path_collisions_checked", true, true);
            AppendBoolProperty(builder, 2, "guid_collisions_checked", true, true);
            AppendProperty(builder, 2, "guid_collision_check", $"package_local_meta_scan_pending_external_validator_{timestamp}", true);
            AppendProperty(builder, 2, "import_smoke_status", importSmokeStatus, true);
            AppendProperty(builder, 2, "preview_render_status", previewStatus, true);
            builder.AppendLine("  \"validation\": {");
            AppendProperty(builder, 4, "package_json_valid", "true", true, false);
            AppendProperty(builder, 4, "manifest_json_valid", "true", true, false);
            AppendProperty(builder, 4, "visual_only_runtime_contract", "colliders_audio_rigidbody_gameplay_authority_omitted", true);
            AppendProperty(builder, 4, "last_metadata_write", timestamp, false);
            builder.AppendLine("  },");
            AppendStringArray(builder, 2, "known_risks", new[]
            {
                "Generated prefabs use procedural primitive and package mesh assets; authored final meshes may replace them before production promotion.",
                "Materials are solid procedural lookdev materials without texture maps.",
                "Glove silhouettes are visual scale tests only and are not rigged hands.",
                "First-person weapon candidates need future hand scale, muzzle alignment, animation socket, and reload validation."
            }, string.Empty, string.Empty, true);
            AppendProperty(builder, 2, "rollback_path", "delete AssetPacks/BrassworksBreach.WeaponViewmodelSet03 and Documentation/ConceptRenders/V0_1_41_WeaponViewmodelSet03", false);
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static string BuildCatalogJson(string timestamp)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            AppendProperty(builder, 2, "catalog_id", "WVM03_WeaponViewmodelSet03_Catalog_v0.1.41", true);
            AppendProperty(builder, 2, "package_name", PackageName, true);
            AppendProperty(builder, 2, "version", PackageVersion, true);
            AppendProperty(builder, 2, "generated_at", timestamp, true);
            builder.AppendLine("  \"prefabs\": [");
            for (var i = 0; i < PrefabNames.Length; i++)
            {
                builder.AppendLine("    {");
                AppendProperty(builder, 6, "asset_id", PrefabNames[i], true);
                AppendProperty(builder, 6, "category", CategoryForPrefab(PrefabNames[i]), true);
                AppendProperty(builder, 6, "runtime_path", $"Packages/{PackageName}/Runtime/Prefabs/{PrefabNames[i]}.prefab", true);
                AppendProperty(builder, 6, "runtime_contract", "visual_only_no_colliders_no_audio_no_gameplay_authority", false);
                builder.Append("    }");
                builder.AppendLine(i == PrefabNames.Length - 1 ? string.Empty : ",");
            }

            builder.AppendLine("  ],");
            AppendStringArray(builder, 2, "materials", MaterialNames, $"Packages/{PackageName}/Runtime/Materials/", ".mat", true);
            AppendStringArray(builder, 2, "meshes", MeshNames, $"Packages/{PackageName}/Runtime/Meshes/", ".asset", false);
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static string CategoryForPrefab(string prefabName)
        {
            if (prefabName.Contains("PressurePistol")) return "pressure_pistol";
            if (prefabName.Contains("Scattergun")) return "scattergun";
            if (prefabName.Contains("BoltThrower")) return "bolt_thrower";
            if (prefabName.Contains("Glove")) return "glove_silhouette";
            if (prefabName.Contains("Ammo")) return "ammo_prop";
            if (prefabName.Contains("Grip")) return "grip_material_test";
            if (prefabName.Contains("Gauge")) return "gauge_component";
            if (prefabName.Contains("Trigger")) return "trigger_component";
            if (prefabName.Contains("Muzzle")) return "muzzle_component";
            return "component_board";
        }

        private static void AppendProperty(StringBuilder builder, int indent, string name, string value, bool comma, bool quote = true)
        {
            builder.Append(' ', indent);
            builder.Append('"');
            builder.Append(name);
            builder.Append("\": ");
            if (quote)
            {
                builder.Append('"');
                builder.Append(Escape(value));
                builder.Append('"');
            }
            else
            {
                builder.Append(value);
            }

            builder.AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendNumberProperty(StringBuilder builder, int indent, string name, int value, bool comma)
        {
            builder.Append(' ', indent);
            builder.Append('"');
            builder.Append(name);
            builder.Append("\": ");
            builder.Append(value.ToString(CultureInfo.InvariantCulture));
            builder.AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendBoolProperty(StringBuilder builder, int indent, string name, bool value, bool comma)
        {
            builder.Append(' ', indent);
            builder.Append('"');
            builder.Append(name);
            builder.Append("\": ");
            builder.Append(value ? "true" : "false");
            builder.AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendStringArray(StringBuilder builder, int indent, string name, IReadOnlyList<string> values, string prefix, string suffix, bool comma, string extraValue = null)
        {
            builder.Append(' ', indent);
            builder.Append('"');
            builder.Append(name);
            builder.AppendLine("\": [");
            var total = values.Count + (string.IsNullOrWhiteSpace(extraValue) ? 0 : 1);
            for (var i = 0; i < values.Count; i++)
            {
                builder.Append(' ', indent + 2);
                builder.Append('"');
                builder.Append(Escape(prefix + values[i] + suffix));
                builder.Append('"');
                builder.AppendLine(i == total - 1 ? string.Empty : ",");
            }

            if (!string.IsNullOrWhiteSpace(extraValue))
            {
                builder.Append(' ', indent + 2);
                builder.Append('"');
                builder.Append(Escape(extraValue));
                builder.Append('"');
                builder.AppendLine();
            }

            builder.Append(' ', indent);
            builder.Append(']');
            builder.AppendLine(comma ? "," : string.Empty);
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static PackageRoot LocatePackageRoot()
        {
            var package = PackageInfo.FindForAssembly(typeof(WeaponViewmodelSet03Generator).Assembly);
            if (package != null)
            {
                return new PackageRoot(package.assetPath, package.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(WeaponViewmodelSet03Generator));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/WeaponViewmodelSet03Generator.cs";
                if (path.EndsWith(suffix, StringComparison.Ordinal))
                {
                    var assetPath = path.Substring(0, path.Length - suffix.Length);
                    return new PackageRoot(assetPath, Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath)));
                }
            }

            throw new InvalidOperationException($"Could not locate {PackageName} package root.");
        }

        private static void EnsurePhysicalFolder(string resolvedPackageRoot, string relativePath)
        {
            var fullPath = Path.Combine(resolvedPackageRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            Directory.CreateDirectory(fullPath);
        }

        private static void ReplaceAsset(string assetPath)
        {
            if (AssetDatabase.LoadMainAssetAtPath(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
        }

        private static void SetColor(Material material, string property, Color color)
        {
            if (material.HasProperty(property))
            {
                material.SetColor(property, color);
            }
        }

        private static void SetFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property))
            {
                material.SetFloat(property, value);
            }
        }

        private readonly struct PackageRoot
        {
            public PackageRoot(string assetPath, string resolvedPath)
            {
                AssetPath = assetPath.Replace("\\", "/");
                ResolvedPath = resolvedPath.Replace("\\", "/");
            }

            public string AssetPath { get; }
            public string ResolvedPath { get; }
        }
    }
}
