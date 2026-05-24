using System;
using System.Collections.Generic;
using System.IO;
using BrassworksBreach.WeaponPropsSet02;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.WeaponPropsSet02.Editor
{
    public static class WeaponPropsSet02Generator
    {
        public const string PackageName = "com.brassworks.sidecar.weapon-props-set02";
        private const string Version = "v0.1.40";

        private static readonly string[] MaterialTags =
        {
            "aged_brass",
            "dark_brass",
            "oxidized_copper",
            "blackened_iron",
            "edge_worn_gunmetal",
            "varnished_walnut",
            "worn_leather",
            "amber_pressure_glass",
            "green_gauge_glass",
            "red_sealing_wax",
            "oily_steel",
            "aged_label_paper"
        };

        private static readonly string[] PrefabNames =
        {
            "BB_WPS02_PressurePistol_Frame_A",
            "BB_WPS02_PressurePistol_Overcoil_B",
            "BB_WPS02_PressurePistol_Snub_C",
            "BB_WPS02_PressurePistol_BarrelAssembly",
            "BB_WPS02_Scattergun_Body_Single",
            "BB_WPS02_Scattergun_Body_TwinBoiler",
            "BB_WPS02_Scattergun_Body_SawedSteam",
            "BB_WPS02_AmmoCartridge_Long",
            "BB_WPS02_AmmoCartridge_Cluster",
            "BB_WPS02_WallWeaponRack_ThreeSlot",
            "BB_WPS02_AmmoCabinet_Shell_Open",
            "BB_WPS02_GearKey_Housing",
            "BB_WPS02_PressureCell_Canister",
            "BB_WPS02_TuningDialGauge_Panel",
            "BB_WPS02_RepairTool_Clutter_A",
            "BB_WPS02_RepairTool_Clutter_B"
        };

        private static readonly string[] MaterialNames =
        {
            "WPS02_MAT_AgedBrass",
            "WPS02_MAT_DarkBrass",
            "WPS02_MAT_OxidizedCopper",
            "WPS02_MAT_BlackenedIron",
            "WPS02_MAT_EdgeWornGunmetal",
            "WPS02_MAT_VarnishedWalnut",
            "WPS02_MAT_WornLeather",
            "WPS02_MAT_AmberPressureGlass",
            "WPS02_MAT_GreenGaugeGlass",
            "WPS02_MAT_RedSealingWax",
            "WPS02_MAT_OilySteel",
            "WPS02_MAT_AgedLabelPaper"
        };

        private static readonly string[] MeshNames =
        {
            "WPS02_Mesh_BeveledBox",
            "WPS02_Mesh_TaperedGrip",
            "WPS02_Mesh_GaugeNeedle",
            "WPS02_Mesh_HexBolt"
        };

        [MenuItem("Brassworks Breach/Sidecar Packs/Generate Weapon Props Set 02 v0.1.40")]
        public static void GenerateAll()
        {
            var packageRoot = LocatePackageRoot();
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Materials");
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Meshes");
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Prefabs");

            var materials = CreateMaterials(packageRoot.AssetPath);
            var meshes = CreateMeshes(packageRoot.AssetPath);

            CreatePressurePistolVariant(packageRoot.AssetPath, materials, meshes, PrefabNames[0], "pressure pistol frame candidate A: long barrel and exposed front pressure coil", 1.03f, 9, 0.30f, false, true);
            CreatePressurePistolVariant(packageRoot.AssetPath, materials, meshes, PrefabNames[1], "pressure pistol overcoil candidate B: raised coil spine and readable gauge cluster", 0.86f, 12, 0.38f, true, true);
            CreatePressurePistolVariant(packageRoot.AssetPath, materials, meshes, PrefabNames[2], "pressure pistol snub candidate C: compact hand silhouette for close range pickup or NPC prop", 0.54f, 6, 0.24f, false, false);
            CreateBarrelAssembly(packageRoot.AssetPath, materials, meshes);

            CreateScattergunVariant(packageRoot.AssetPath, materials, meshes, PrefabNames[4], "scattergun body single-boiler candidate with broad pump rail", 1, false, 0.92f);
            CreateScattergunVariant(packageRoot.AssetPath, materials, meshes, PrefabNames[5], "scattergun body twin-boiler candidate with paired pressure tanks", 2, true, 1.08f);
            CreateScattergunVariant(packageRoot.AssetPath, materials, meshes, PrefabNames[6], "sawed steam scattergun candidate with shortened barrel and chunky receiver", 2, false, 0.62f);

            CreateAmmoCartridgeLong(packageRoot.AssetPath, materials, meshes);
            CreateAmmoCartridgeCluster(packageRoot.AssetPath, materials, meshes);
            CreateWallWeaponRack(packageRoot.AssetPath, materials, meshes);
            CreateAmmoCabinetShellOpen(packageRoot.AssetPath, materials, meshes);
            CreateGearKeyHousing(packageRoot.AssetPath, materials, meshes);
            CreatePressureCellCanister(packageRoot.AssetPath, materials, meshes);
            CreateTuningDialGaugePanel(packageRoot.AssetPath, materials, meshes);
            CreateRepairToolClutterA(packageRoot.AssetPath, materials, meshes);
            CreateRepairToolClutterB(packageRoot.AssetPath, materials, meshes);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"BB_WEAPON_PROPS_SET02_GENERATE_PASS {Version} prefabs={PrefabNames.Length} materials={MaterialNames.Length} meshes={MeshNames.Length} root={packageRoot.AssetPath}");
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
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_WPS02_RENDER_ROOT");
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
                "V0_1_40_WeaponPropsSet02"));
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
                ["brass"] = SaveMaterial(root, shader, "WPS02_MAT_AgedBrass", new Color(0.79f, 0.55f, 0.25f), 0.86f, 0.43f),
                ["darkBrass"] = SaveMaterial(root, shader, "WPS02_MAT_DarkBrass", new Color(0.47f, 0.31f, 0.12f), 0.82f, 0.30f),
                ["copper"] = SaveMaterial(root, shader, "WPS02_MAT_OxidizedCopper", new Color(0.72f, 0.30f, 0.16f), 0.84f, 0.35f),
                ["iron"] = SaveMaterial(root, shader, "WPS02_MAT_BlackenedIron", new Color(0.038f, 0.036f, 0.034f), 0.75f, 0.26f),
                ["gunmetal"] = SaveMaterial(root, shader, "WPS02_MAT_EdgeWornGunmetal", new Color(0.26f, 0.28f, 0.28f), 0.82f, 0.46f),
                ["walnut"] = SaveMaterial(root, shader, "WPS02_MAT_VarnishedWalnut", new Color(0.28f, 0.13f, 0.055f), 0.05f, 0.57f),
                ["leather"] = SaveMaterial(root, shader, "WPS02_MAT_WornLeather", new Color(0.34f, 0.18f, 0.08f), 0.02f, 0.32f),
                ["amber"] = SaveMaterial(root, shader, "WPS02_MAT_AmberPressureGlass", new Color(1.0f, 0.50f, 0.10f), 0.0f, 0.82f, new Color(1.0f, 0.40f, 0.10f) * 1.25f),
                ["greenGlass"] = SaveMaterial(root, shader, "WPS02_MAT_GreenGaugeGlass", new Color(0.18f, 0.70f, 0.48f), 0.0f, 0.76f, new Color(0.10f, 0.50f, 0.32f) * 0.8f),
                ["redSeal"] = SaveMaterial(root, shader, "WPS02_MAT_RedSealingWax", new Color(0.55f, 0.035f, 0.026f), 0.0f, 0.44f),
                ["oilySteel"] = SaveMaterial(root, shader, "WPS02_MAT_OilySteel", new Color(0.11f, 0.12f, 0.115f), 0.68f, 0.70f),
                ["paper"] = SaveMaterial(root, shader, "WPS02_MAT_AgedLabelPaper", new Color(0.78f, 0.70f, 0.55f), 0.0f, 0.24f)
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes(string root)
        {
            return new Dictionary<string, Mesh>
            {
                ["beveledBox"] = SaveMesh(root, "WPS02_Mesh_BeveledBox", CreateBoxMesh()),
                ["taperedGrip"] = SaveMesh(root, "WPS02_Mesh_TaperedGrip", CreateTaperedGripMesh()),
                ["gaugeNeedle"] = SaveMesh(root, "WPS02_Mesh_GaugeNeedle", CreateGaugeNeedleMesh()),
                ["hexBolt"] = SaveMesh(root, "WPS02_Mesh_HexBolt", CreateHexBoltMesh())
            };
        }

        private static void CreatePressurePistolVariant(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, string assetName, string role, float barrelLength, int coilCount, float chamberRadius, bool raisedSpine, bool sideCanister)
        {
            var pistol = NewRoot(assetName, role, "Promote as first-person/world pistol component variant after silhouette review.", "Named receiver, chamber, coil, grip, gauge, and trigger subparts are intentionally separated.");

            MeshPart(pistol.transform, meshes["beveledBox"], "blackened_iron_receiver_block", new Vector3(0f, 0.03f, -0.36f), new Vector3(0.38f, 0.27f, 0.34f), Vector3.zero, materials["iron"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "gunmetal_primary_barrel", new Vector3(0f, 0.14f, 0.16f), new Vector3(0.075f, barrelLength * 0.50f, 0.075f), new Vector3(90f, 0f, 0f), materials["gunmetal"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "aged_brass_muzzle_ring", new Vector3(0f, 0.14f, 0.17f + barrelLength), new Vector3(0.13f, 0.048f, 0.13f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "dark_brass_pressure_chamber", new Vector3(0f, -0.10f, 0.03f), new Vector3(chamberRadius, 0.29f, chamberRadius), new Vector3(90f, 0f, 0f), materials["darkBrass"]);
            Primitive(pistol.transform, PrimitiveType.Cylinder, "amber_glass_pressure_window", new Vector3(0f, -0.10f, 0.03f), new Vector3(chamberRadius * 0.58f, 0.315f, chamberRadius * 0.58f), new Vector3(90f, 0f, 0f), materials["amber"]);

            for (var i = 0; i < coilCount; i++)
            {
                var z = -0.13f + i * 0.065f;
                var y = raisedSpine ? 0.25f : 0.15f;
                Primitive(pistol.transform, PrimitiveType.Cylinder, $"oxidized_copper_barrel_coil_{i:00}", new Vector3(0f, y, z), new Vector3(0.118f, 0.010f, 0.118f), new Vector3(90f, 0f, 0f), materials["copper"]);
            }

            if (raisedSpine)
            {
                Primitive(pistol.transform, PrimitiveType.Cylinder, "upper_amber_pressure_spine", new Vector3(0f, 0.31f, 0.10f), new Vector3(0.045f, 0.48f, 0.045f), new Vector3(90f, 0f, 0f), materials["amber"]);
                Primitive(pistol.transform, PrimitiveType.Cube, "aged_brass_spine_front_clamp", new Vector3(0f, 0.31f, 0.55f), new Vector3(0.20f, 0.08f, 0.05f), Vector3.zero, materials["brass"]);
                Primitive(pistol.transform, PrimitiveType.Cube, "aged_brass_spine_rear_clamp", new Vector3(0f, 0.31f, -0.37f), new Vector3(0.20f, 0.08f, 0.05f), Vector3.zero, materials["brass"]);
            }

            MeshPart(pistol.transform, meshes["taperedGrip"], "varnished_walnut_tapered_grip", new Vector3(0f, -0.46f, -0.58f), new Vector3(0.52f, 0.70f, 0.36f), new Vector3(15f, 0f, 0f), materials["walnut"]);
            for (var i = 0; i < 4; i++)
            {
                Primitive(pistol.transform, PrimitiveType.Cube, $"worn_leather_grip_band_{i:00}", new Vector3(0f, -0.63f + i * 0.13f, -0.59f), new Vector3(0.30f, 0.035f, 0.22f), new Vector3(15f, 0f, 0f), materials["leather"]);
            }

            Primitive(pistol.transform, PrimitiveType.Cube, "aged_brass_trigger_guard_front", new Vector3(0f, -0.15f, -0.35f), new Vector3(0.28f, 0.045f, 0.20f), new Vector3(24f, 0f, 0f), materials["brass"]);
            Primitive(pistol.transform, PrimitiveType.Cube, "aged_brass_trigger_guard_lower", new Vector3(0f, -0.25f, -0.42f), new Vector3(0.28f, 0.035f, 0.30f), Vector3.zero, materials["brass"]);
            Primitive(pistol.transform, PrimitiveType.Cube, "blackened_iron_trigger_hook", new Vector3(0f, -0.20f, -0.48f), new Vector3(0.055f, 0.17f, 0.040f), new Vector3(-21f, 0f, 0f), materials["iron"]);

            if (sideCanister)
            {
                Primitive(pistol.transform, PrimitiveType.Cylinder, "left_green_pressure_balancer", new Vector3(-0.25f, -0.03f, 0.07f), new Vector3(0.070f, 0.33f, 0.070f), new Vector3(90f, 0f, 0f), materials["greenGlass"]);
                Primitive(pistol.transform, PrimitiveType.Cylinder, "left_brass_balancer_cap_front", new Vector3(-0.25f, -0.03f, 0.40f), new Vector3(0.082f, 0.026f, 0.082f), new Vector3(90f, 0f, 0f), materials["brass"]);
                Primitive(pistol.transform, PrimitiveType.Cylinder, "left_brass_balancer_cap_rear", new Vector3(-0.25f, -0.03f, -0.26f), new Vector3(0.082f, 0.026f, 0.082f), new Vector3(90f, 0f, 0f), materials["brass"]);
            }

            AddGaugeFace(pistol.transform, "top_pressure_gauge_cluster", new Vector3(0f, 0.34f, -0.31f), 0.15f, materials, meshes, true);
            AddRivetRow(pistol.transform, "left_receiver_hex_bolts", -0.215f, materials["brass"], meshes["hexBolt"]);
            AddRivetRow(pistol.transform, "right_receiver_hex_bolts", 0.215f, materials["brass"], meshes["hexBolt"]);

            SavePrefab(root, pistol, assetName);
        }

        private static void CreateBarrelAssembly(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var barrel = NewRoot(PrefabNames[3], "modular pressure pistol barrel assembly with coils, muzzle collars, and front sight", "Can be promoted as a shared muzzle/barrel module.", "Subparts separate muzzle, coils, sight, and heat-shield ribs.");

            Primitive(barrel.transform, PrimitiveType.Cylinder, "blackened_iron_barrel_core", Vector3.zero, new Vector3(0.070f, 0.78f, 0.070f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Primitive(barrel.transform, PrimitiveType.Cylinder, "aged_brass_muzzle_collar_outer", new Vector3(0f, 0f, 0.81f), new Vector3(0.145f, 0.065f, 0.145f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(barrel.transform, PrimitiveType.Cylinder, "dark_brass_breech_collar_outer", new Vector3(0f, 0f, -0.81f), new Vector3(0.135f, 0.060f, 0.135f), new Vector3(90f, 0f, 0f), materials["darkBrass"]);
            for (var i = 0; i < 13; i++)
            {
                Primitive(barrel.transform, PrimitiveType.Cylinder, $"oxidized_copper_heat_coil_{i:00}", new Vector3(0f, 0f, -0.58f + i * 0.095f), new Vector3(0.105f, 0.010f, 0.105f), new Vector3(90f, 0f, 0f), materials["copper"]);
            }

            MeshPart(barrel.transform, meshes["beveledBox"], "gunmetal_front_sight_block", new Vector3(0f, 0.135f, 0.62f), new Vector3(0.08f, 0.10f, 0.12f), Vector3.zero, materials["gunmetal"]);
            MeshPart(barrel.transform, meshes["gaugeNeedle"], "aged_brass_front_sight_blade", new Vector3(0f, 0.225f, 0.62f), new Vector3(0.22f, 0.75f, 0.35f), new Vector3(0f, 0f, 90f), materials["brass"]);
            SavePrefab(root, barrel, PrefabNames[3]);
        }

        private static void CreateScattergunVariant(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, string assetName, string role, int barrelCount, bool twinBoiler, float barrelLength)
        {
            var scattergun = NewRoot(assetName, role, "Promote after deciding first-person silhouette width and reload readability.", "Receiver, barrels, pressure tanks, stock, pump rail, hinges, and gauges are discrete subparts.");

            MeshPart(scattergun.transform, meshes["beveledBox"], "blackened_iron_broad_receiver", new Vector3(0f, 0f, -0.36f), new Vector3(0.58f, 0.34f, 0.44f), Vector3.zero, materials["iron"]);
            MeshPart(scattergun.transform, meshes["taperedGrip"], "varnished_walnut_shoulder_stock", new Vector3(0f, -0.03f, -0.95f), new Vector3(0.58f, 0.52f, 0.78f), new Vector3(-75f, 0f, 0f), materials["walnut"]);
            Primitive(scattergun.transform, PrimitiveType.Cube, "worn_leather_stock_pad", new Vector3(0f, -0.25f, -1.28f), new Vector3(0.52f, 0.08f, 0.16f), new Vector3(-8f, 0f, 0f), materials["leather"]);
            Primitive(scattergun.transform, PrimitiveType.Cube, "aged_brass_receiver_top_strap", new Vector3(0f, 0.22f, -0.34f), new Vector3(0.64f, 0.055f, 0.50f), Vector3.zero, materials["brass"]);

            for (var i = 0; i < barrelCount; i++)
            {
                var x = barrelCount == 1 ? 0f : -0.11f + i * 0.22f;
                Primitive(scattergun.transform, PrimitiveType.Cylinder, $"oily_steel_scatter_barrel_{i:00}", new Vector3(x, 0.08f, 0.18f), new Vector3(0.082f, barrelLength * 0.52f, 0.082f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
                Primitive(scattergun.transform, PrimitiveType.Cylinder, $"dark_brass_muzzle_reinforcement_{i:00}", new Vector3(x, 0.08f, 0.22f + barrelLength), new Vector3(0.115f, 0.047f, 0.115f), new Vector3(90f, 0f, 0f), materials["darkBrass"]);
            }

            Primitive(scattergun.transform, PrimitiveType.Cube, "varnished_walnut_pump_foregrip", new Vector3(0f, -0.17f, 0.42f), new Vector3(0.44f, 0.17f, 0.44f), Vector3.zero, materials["walnut"]);
            for (var i = 0; i < 5; i++)
            {
                Primitive(scattergun.transform, PrimitiveType.Cube, $"worn_leather_pump_groove_{i:00}", new Vector3(-0.24f + i * 0.12f, -0.075f, 0.42f), new Vector3(0.030f, 0.07f, 0.48f), Vector3.zero, materials["leather"]);
            }

            var tankCount = twinBoiler ? 2 : 1;
            for (var i = 0; i < tankCount; i++)
            {
                var x = tankCount == 1 ? 0f : -0.21f + i * 0.42f;
                Primitive(scattergun.transform, PrimitiveType.Cylinder, $"amber_underbarrel_pressure_tank_{i:00}", new Vector3(x, -0.26f, 0.22f), new Vector3(0.095f, 0.47f, 0.095f), new Vector3(90f, 0f, 0f), materials["amber"]);
                Primitive(scattergun.transform, PrimitiveType.Cylinder, $"aged_brass_tank_front_cap_{i:00}", new Vector3(x, -0.26f, 0.70f), new Vector3(0.11f, 0.030f, 0.11f), new Vector3(90f, 0f, 0f), materials["brass"]);
                Primitive(scattergun.transform, PrimitiveType.Cylinder, $"aged_brass_tank_rear_cap_{i:00}", new Vector3(x, -0.26f, -0.26f), new Vector3(0.11f, 0.030f, 0.11f), new Vector3(90f, 0f, 0f), materials["brass"]);
            }

            AddGaugeFace(scattergun.transform, "left_receiver_pressure_meter", new Vector3(-0.36f, 0.08f, -0.42f), 0.14f, materials, meshes, false);
            AddBoltGrid(scattergun.transform, "receiver_hex_bolt_grid", materials["brass"], meshes["hexBolt"], 3, 2, 0.22f, 0.13f, new Vector3(0f, 0.205f, -0.37f));
            SavePrefab(root, scattergun, assetName);
        }

        private static void CreateAmmoCartridgeLong(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var cartridge = NewRoot(PrefabNames[7], "long pressure cartridge pickup candidate with label band and sealing wax", "Promote as ammo pickup visual after scale pass.", "Glass vial, caps, paper label, red seal, and copper rings are discrete.");
            AddPressureCartridge(cartridge.transform, "long", Vector3.zero, 1.0f, materials, meshes);
            SavePrefab(root, cartridge, PrefabNames[7]);
        }

        private static void CreateAmmoCartridgeCluster(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var cluster = NewRoot(PrefabNames[8], "clustered pressure cartridges in walnut feed tray", "Promote as ammo shelf filler or pickup cluster.", "Tray and every cartridge row remain named for later mesh replacement.");
            Primitive(cluster.transform, PrimitiveType.Cube, "varnished_walnut_slotted_tray", new Vector3(0f, -0.10f, 0f), new Vector3(0.94f, 0.08f, 0.66f), Vector3.zero, materials["walnut"]);
            Primitive(cluster.transform, PrimitiveType.Cube, "dark_brass_front_tray_lip", new Vector3(0f, -0.035f, 0.36f), new Vector3(0.98f, 0.08f, 0.045f), Vector3.zero, materials["darkBrass"]);
            for (var row = 0; row < 2; row++)
            {
                for (var col = 0; col < 4; col++)
                {
                    AddPressureCartridge(cluster.transform, $"tray_{row:00}_{col:00}", new Vector3(-0.33f + col * 0.22f, 0.02f, -0.16f + row * 0.28f), 0.52f, materials, meshes);
                }
            }

            SavePrefab(root, cluster, PrefabNames[8]);
        }

        private static void CreateWallWeaponRack(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var rack = NewRoot(PrefabNames[9], "wall-mounted three-slot weapon rack with brass rails and service tags", "Promote as interactable world prop shell after collision pass.", "Mount planks, hooks, rails, shadow plates, tags, and sample silhouettes are isolated.");
            Primitive(rack.transform, PrimitiveType.Cube, "varnished_walnut_back_plank", new Vector3(0f, 0f, 0.09f), new Vector3(2.35f, 1.12f, 0.12f), Vector3.zero, materials["walnut"]);
            Primitive(rack.transform, PrimitiveType.Cube, "blackened_iron_wall_shadow_plate", new Vector3(0f, 0f, 0.17f), new Vector3(2.48f, 1.24f, 0.045f), Vector3.zero, materials["iron"]);
            for (var i = 0; i < 3; i++)
            {
                var y = -0.34f + i * 0.34f;
                Primitive(rack.transform, PrimitiveType.Cylinder, $"aged_brass_horizontal_pressure_rail_{i:00}", new Vector3(0f, y, -0.02f), new Vector3(0.038f, 1.05f, 0.038f), new Vector3(0f, 0f, 90f), materials["brass"]);
            }

            for (var i = 0; i < 6; i++)
            {
                var x = -0.88f + i * 0.35f;
                MeshPart(rack.transform, meshes["beveledBox"], $"blackened_iron_drop_hook_{i:00}", new Vector3(x, -0.03f, -0.16f), new Vector3(0.055f, 0.34f, 0.070f), new Vector3(0f, 0f, i % 2 == 0 ? 15f : -15f), materials["iron"]);
                MeshPart(rack.transform, meshes["hexBolt"], $"aged_brass_hook_bolt_{i:00}", new Vector3(x, 0.24f, -0.18f), Vector3.one * 0.08f, new Vector3(90f, 0f, 0f), materials["brass"]);
            }

            Primitive(rack.transform, PrimitiveType.Cylinder, "sample_pressure_pistol_rack_barrel", new Vector3(-0.48f, 0.05f, -0.27f), new Vector3(0.060f, 0.42f, 0.060f), new Vector3(90f, 0f, 0f), materials["gunmetal"]);
            Primitive(rack.transform, PrimitiveType.Cylinder, "sample_scattergun_rack_barrel", new Vector3(0.45f, 0.00f, -0.27f), new Vector3(0.072f, 0.58f, 0.072f), new Vector3(90f, 0f, 0f), materials["oilySteel"]);
            Primitive(rack.transform, PrimitiveType.Cube, "aged_paper_service_tag_left", new Vector3(-0.96f, -0.43f, -0.11f), new Vector3(0.24f, 0.14f, 0.018f), Vector3.zero, materials["paper"]);
            Primitive(rack.transform, PrimitiveType.Cube, "aged_paper_service_tag_right", new Vector3(0.96f, -0.43f, -0.11f), new Vector3(0.24f, 0.14f, 0.018f), Vector3.zero, materials["paper"]);
            SavePrefab(root, rack, PrefabNames[9]);
        }

        private static void CreateAmmoCabinetShellOpen(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var cabinet = NewRoot(PrefabNames[10], "open ammo cabinet shell with pressure cartridges and gauge latch", "Promote as interactable prop shell only; gameplay storage logic is intentionally absent.", "Door, hinge, shelves, rows, latch, and pressure indicator are separated.");
            Primitive(cabinet.transform, PrimitiveType.Cube, "blackened_iron_rear_wall_plate", new Vector3(0f, 0f, 0.12f), new Vector3(1.38f, 1.64f, 0.08f), Vector3.zero, materials["iron"]);
            Primitive(cabinet.transform, PrimitiveType.Cube, "varnished_walnut_cabinet_box", new Vector3(0f, 0f, 0.02f), new Vector3(1.18f, 1.42f, 0.22f), Vector3.zero, materials["walnut"]);
            Primitive(cabinet.transform, PrimitiveType.Cube, "aged_brass_top_frame", new Vector3(0f, 0.74f, -0.08f), new Vector3(1.30f, 0.07f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(cabinet.transform, PrimitiveType.Cube, "aged_brass_bottom_frame", new Vector3(0f, -0.74f, -0.08f), new Vector3(1.30f, 0.07f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(cabinet.transform, PrimitiveType.Cube, "aged_brass_left_frame", new Vector3(-0.66f, 0f, -0.08f), new Vector3(0.07f, 1.52f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(cabinet.transform, PrimitiveType.Cube, "aged_brass_right_frame", new Vector3(0.66f, 0f, -0.08f), new Vector3(0.07f, 1.52f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(cabinet.transform, PrimitiveType.Cube, "open_amber_glass_door_panel", new Vector3(0.78f, 0.02f, -0.22f), new Vector3(0.48f, 1.20f, 0.040f), new Vector3(0f, -28f, 0f), materials["amber"]);
            Primitive(cabinet.transform, PrimitiveType.Cube, "blackened_iron_hinge_spine", new Vector3(0.62f, 0f, -0.18f), new Vector3(0.050f, 1.22f, 0.060f), Vector3.zero, materials["iron"]);
            for (var row = 0; row < 3; row++)
            {
                var y = 0.40f - row * 0.38f;
                Primitive(cabinet.transform, PrimitiveType.Cube, $"varnished_walnut_shelf_{row:00}", new Vector3(0f, y - 0.15f, -0.14f), new Vector3(1.00f, 0.045f, 0.24f), Vector3.zero, materials["walnut"]);
                for (var col = 0; col < 4; col++)
                {
                    AddPressureCartridge(cabinet.transform, $"cabinet_{row:00}_{col:00}", new Vector3(-0.34f + col * 0.23f, y, -0.19f), 0.42f, materials, meshes);
                }
            }

            AddGaugeFace(cabinet.transform, "door_latch_pressure_indicator", new Vector3(0.47f, 0.56f, -0.20f), 0.13f, materials, meshes, true);
            SavePrefab(root, cabinet, PrefabNames[10]);
        }

        private static void CreateGearKeyHousing(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var housing = NewRoot(PrefabNames[11], "gear-key housing interactable prop candidate with exposed lock wheel", "Promote as wall interaction visual; no gameplay trigger is included.", "Key socket, gear teeth, indicator glass, and mount bolts are separate.");
            Primitive(housing.transform, PrimitiveType.Cube, "blackened_iron_square_mount_plate", new Vector3(0f, 0f, 0.09f), new Vector3(1.03f, 1.03f, 0.10f), Vector3.zero, materials["iron"]);
            Primitive(housing.transform, PrimitiveType.Cube, "aged_brass_inner_lock_box", Vector3.zero, new Vector3(0.76f, 0.76f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(housing.transform, PrimitiveType.Cylinder, "dark_brass_gear_key_socket", new Vector3(0f, 0f, -0.12f), new Vector3(0.24f, 0.06f, 0.24f), new Vector3(90f, 0f, 0f), materials["darkBrass"]);
            Primitive(housing.transform, PrimitiveType.Cylinder, "green_glass_ready_lens", new Vector3(0f, 0.31f, -0.16f), new Vector3(0.10f, 0.018f, 0.10f), new Vector3(90f, 0f, 0f), materials["greenGlass"]);
            for (var i = 0; i < 12; i++)
            {
                var angle = i * Mathf.PI * 2f / 12f;
                var pos = new Vector3(Mathf.Cos(angle) * 0.32f, Mathf.Sin(angle) * 0.32f, -0.14f);
                Primitive(housing.transform, PrimitiveType.Cube, $"aged_brass_lock_gear_tooth_{i:00}", pos, new Vector3(0.12f, 0.040f, 0.055f), new Vector3(0f, 0f, Mathf.Rad2Deg * angle), materials["brass"]);
            }

            AddBoltGrid(housing.transform, "corner_mount_hex_bolts", materials["darkBrass"], meshes["hexBolt"], 2, 2, 0.74f, 0.74f, new Vector3(0f, 0f, -0.15f));
            SavePrefab(root, housing, PrefabNames[11]);
        }

        private static void CreatePressureCellCanister(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var canister = NewRoot(PrefabNames[12], "portable pressure-cell canister prop with cage ribs, valve, and gauge", "Promote as reload prop, pickup, or set dressing after collision scale pass.", "Cage ribs, caps, valve wheel, label strip, and pressure gauge remain named.");
            Primitive(canister.transform, PrimitiveType.Cylinder, "amber_glass_main_pressure_cell", Vector3.zero, new Vector3(0.20f, 0.54f, 0.20f), new Vector3(90f, 0f, 0f), materials["amber"]);
            Primitive(canister.transform, PrimitiveType.Cylinder, "aged_brass_front_cap", new Vector3(0f, 0f, 0.56f), new Vector3(0.24f, 0.050f, 0.24f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(canister.transform, PrimitiveType.Cylinder, "aged_brass_rear_cap", new Vector3(0f, 0f, -0.56f), new Vector3(0.24f, 0.050f, 0.24f), new Vector3(90f, 0f, 0f), materials["brass"]);
            for (var i = 0; i < 6; i++)
            {
                var angle = i * Mathf.PI * 2f / 6f;
                Primitive(canister.transform, PrimitiveType.Cube, $"blackened_iron_cage_rib_{i:00}", new Vector3(Mathf.Cos(angle) * 0.22f, Mathf.Sin(angle) * 0.22f, 0f), new Vector3(0.035f, 0.035f, 1.18f), new Vector3(0f, 0f, Mathf.Rad2Deg * angle), materials["iron"]);
            }

            Primitive(canister.transform, PrimitiveType.Cube, "aged_paper_pressure_rating_label", new Vector3(0f, -0.21f, 0.05f), new Vector3(0.30f, 0.018f, 0.38f), Vector3.zero, materials["paper"]);
            Primitive(canister.transform, PrimitiveType.Cylinder, "red_sealing_wax_safety_plug", new Vector3(0f, 0.25f, 0.34f), new Vector3(0.065f, 0.026f, 0.065f), new Vector3(90f, 0f, 0f), materials["redSeal"]);
            AddGaugeFace(canister.transform, "rear_mini_pressure_gauge", new Vector3(0f, 0.28f, -0.44f), 0.11f, materials, meshes, true);
            SavePrefab(root, canister, PrefabNames[12]);
        }

        private static void CreateTuningDialGaugePanel(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var panel = NewRoot(PrefabNames[13], "tuning dial and gauge panel clutter/interactable shell", "Promote as wall console accessory after UX readability pass.", "Dial knobs, gauge faces, needles, labels, and bolt grids are separate.");
            Primitive(panel.transform, PrimitiveType.Cube, "blackened_iron_rect_panel_back", new Vector3(0f, 0f, 0.06f), new Vector3(1.42f, 0.82f, 0.08f), Vector3.zero, materials["iron"]);
            Primitive(panel.transform, PrimitiveType.Cube, "dark_brass_panel_faceplate", Vector3.zero, new Vector3(1.26f, 0.68f, 0.08f), Vector3.zero, materials["darkBrass"]);
            for (var i = 0; i < 3; i++)
            {
                AddGaugeFace(panel.transform, $"calibration_pressure_gauge_{i:00}", new Vector3(-0.42f + i * 0.42f, 0.18f, -0.09f), 0.12f, materials, meshes, i % 2 == 0);
                Primitive(panel.transform, PrimitiveType.Cylinder, $"blackened_iron_tuning_dial_{i:00}", new Vector3(-0.42f + i * 0.42f, -0.20f, -0.08f), new Vector3(0.095f, 0.035f, 0.095f), new Vector3(90f, 0f, 0f), materials["iron"]);
                MeshPart(panel.transform, meshes["gaugeNeedle"], $"aged_brass_dial_pointer_{i:00}", new Vector3(-0.42f + i * 0.42f, -0.20f, -0.13f), new Vector3(0.15f, 0.40f, 0.20f), new Vector3(0f, 0f, i * 34f), materials["brass"]);
                Primitive(panel.transform, PrimitiveType.Cube, $"aged_paper_calibration_label_{i:00}", new Vector3(-0.42f + i * 0.42f, -0.34f, -0.07f), new Vector3(0.27f, 0.075f, 0.018f), Vector3.zero, materials["paper"]);
            }

            AddBoltGrid(panel.transform, "panel_corner_hex_bolts", materials["brass"], meshes["hexBolt"], 2, 2, 1.05f, 0.52f, new Vector3(0f, 0f, -0.10f));
            SavePrefab(root, panel, PrefabNames[13]);
        }

        private static void CreateRepairToolClutterA(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var clutter = NewRoot(PrefabNames[14], "repair-tool clutter tray with wrench, driver, oil flask, and fasteners", "Promote as bench/floor set dressing after density pass.", "Tray, tools, oil flask, and loose bolts are named as separate pickup-scale subparts.");
            Primitive(clutter.transform, PrimitiveType.Cube, "varnished_walnut_tool_tray_floor", new Vector3(0f, -0.06f, 0f), new Vector3(1.16f, 0.05f, 0.66f), Vector3.zero, materials["walnut"]);
            Primitive(clutter.transform, PrimitiveType.Cube, "dark_brass_tool_tray_front_lip", new Vector3(0f, 0.02f, 0.35f), new Vector3(1.18f, 0.08f, 0.05f), Vector3.zero, materials["darkBrass"]);
            Primitive(clutter.transform, PrimitiveType.Cylinder, "oily_steel_pressure_wrench_handle", new Vector3(-0.30f, 0.06f, 0.02f), new Vector3(0.035f, 0.36f, 0.035f), new Vector3(0f, 0f, 58f), materials["oilySteel"]);
            Primitive(clutter.transform, PrimitiveType.Cylinder, "aged_brass_pressure_wrench_head", new Vector3(-0.55f, 0.18f, 0.03f), new Vector3(0.095f, 0.022f, 0.095f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(clutter.transform, PrimitiveType.Cylinder, "blackened_iron_screwdriver_shaft", new Vector3(0.18f, 0.06f, -0.05f), new Vector3(0.020f, 0.38f, 0.020f), new Vector3(0f, 0f, -66f), materials["iron"]);
            MeshPart(clutter.transform, meshes["taperedGrip"], "varnished_walnut_screwdriver_handle", new Vector3(0.48f, 0.00f, -0.09f), new Vector3(0.16f, 0.28f, 0.16f), new Vector3(0f, 0f, -66f), materials["walnut"]);
            Primitive(clutter.transform, PrimitiveType.Cylinder, "green_glass_oil_flask_body", new Vector3(0.37f, 0.12f, 0.20f), new Vector3(0.095f, 0.13f, 0.095f), Vector3.zero, materials["greenGlass"]);
            Primitive(clutter.transform, PrimitiveType.Cylinder, "red_sealing_wax_flask_stop", new Vector3(0.37f, 0.28f, 0.20f), new Vector3(0.035f, 0.028f, 0.035f), Vector3.zero, materials["redSeal"]);
            for (var i = 0; i < 5; i++)
            {
                MeshPart(clutter.transform, meshes["hexBolt"], $"loose_aged_brass_hex_bolt_{i:00}", new Vector3(-0.05f + i * 0.08f, 0.04f, 0.23f - i * 0.035f), Vector3.one * 0.055f, new Vector3(90f, 0f, i * 17f), materials["brass"]);
            }

            SavePrefab(root, clutter, PrefabNames[14]);
        }

        private static void CreateRepairToolClutterB(string root, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var clutter = NewRoot(PrefabNames[15], "repair-tool clutter scatter with caliper, hammer, gear teeth, and tag stack", "Promote as floor or workbench set dressing after LOD pass.", "Loose tools and tags are separated for later authored meshes.");
            Primitive(clutter.transform, PrimitiveType.Cube, "aged_paper_folded_work_order_stack", new Vector3(-0.36f, 0.00f, 0.16f), new Vector3(0.34f, 0.025f, 0.25f), new Vector3(0f, 16f, 0f), materials["paper"]);
            Primitive(clutter.transform, PrimitiveType.Cube, "aged_paper_top_repair_tag", new Vector3(-0.34f, 0.035f, 0.14f), new Vector3(0.30f, 0.018f, 0.20f), new Vector3(0f, 10f, 0f), materials["paper"]);
            Primitive(clutter.transform, PrimitiveType.Cylinder, "oily_steel_hammer_handle", new Vector3(0.12f, 0.08f, 0.00f), new Vector3(0.030f, 0.43f, 0.030f), new Vector3(0f, 0f, 86f), materials["oilySteel"]);
            MeshPart(clutter.transform, meshes["beveledBox"], "blackened_iron_hammer_head", new Vector3(0.48f, 0.08f, 0.00f), new Vector3(0.32f, 0.11f, 0.13f), Vector3.zero, materials["iron"]);
            Primitive(clutter.transform, PrimitiveType.Cylinder, "aged_brass_caliper_arm_left", new Vector3(-0.05f, 0.08f, -0.23f), new Vector3(0.015f, 0.28f, 0.015f), new Vector3(0f, 0f, 28f), materials["brass"]);
            Primitive(clutter.transform, PrimitiveType.Cylinder, "aged_brass_caliper_arm_right", new Vector3(0.12f, 0.08f, -0.23f), new Vector3(0.015f, 0.28f, 0.015f), new Vector3(0f, 0f, -28f), materials["brass"]);
            for (var i = 0; i < 8; i++)
            {
                var angle = i * Mathf.PI * 2f / 8f;
                Primitive(clutter.transform, PrimitiveType.Cube, $"loose_dark_brass_gear_tooth_{i:00}", new Vector3(0.40f + Mathf.Cos(angle) * 0.13f, 0.035f, -0.25f + Mathf.Sin(angle) * 0.13f), new Vector3(0.070f, 0.030f, 0.035f), new Vector3(0f, Mathf.Rad2Deg * angle, 0f), materials["darkBrass"]);
            }

            MeshPart(clutter.transform, meshes["hexBolt"], "red_sealed_inspection_bolt", new Vector3(-0.58f, 0.045f, -0.12f), Vector3.one * 0.070f, new Vector3(90f, 0f, 0f), materials["redSeal"]);
            SavePrefab(root, clutter, PrefabNames[15]);
        }

        private static void AddPressureCartridge(Transform parent, string prefix, Vector3 localPosition, float scale, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes)
        {
            var cartridge = new GameObject($"{prefix}_pressure_cartridge");
            cartridge.transform.SetParent(parent, false);
            cartridge.transform.localPosition = localPosition;
            cartridge.transform.localScale = Vector3.one * scale;
            cartridge.transform.localEulerAngles = new Vector3(0f, 0f, 90f);

            Primitive(cartridge.transform, PrimitiveType.Cylinder, $"{prefix}_amber_pressure_vial", Vector3.zero, new Vector3(0.085f, 0.42f, 0.085f), new Vector3(90f, 0f, 0f), materials["amber"]);
            Primitive(cartridge.transform, PrimitiveType.Cylinder, $"{prefix}_aged_brass_front_cap", new Vector3(0f, 0f, 0.44f), new Vector3(0.11f, 0.040f, 0.11f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(cartridge.transform, PrimitiveType.Cylinder, $"{prefix}_aged_brass_rear_cap", new Vector3(0f, 0f, -0.44f), new Vector3(0.11f, 0.040f, 0.11f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(cartridge.transform, PrimitiveType.Cube, $"{prefix}_aged_paper_charge_label", new Vector3(0f, 0.088f, 0.02f), new Vector3(0.14f, 0.018f, 0.25f), Vector3.zero, materials["paper"]);
            Primitive(cartridge.transform, PrimitiveType.Sphere, $"{prefix}_red_wax_charge_seal", new Vector3(0f, 0.105f, 0.16f), Vector3.one * 0.030f, Vector3.zero, materials["redSeal"]);
            for (var i = 0; i < 3; i++)
            {
                Primitive(cartridge.transform, PrimitiveType.Cylinder, $"{prefix}_oxidized_copper_retaining_band_{i:00}", new Vector3(0f, 0f, -0.20f + i * 0.20f), new Vector3(0.098f, 0.010f, 0.098f), new Vector3(90f, 0f, 0f), materials["copper"]);
            }

            MeshPart(cartridge.transform, meshes["hexBolt"], $"{prefix}_tiny_stamp_bolt", new Vector3(0f, 0.115f, -0.20f), Vector3.one * 0.032f, new Vector3(90f, 0f, 0f), materials["darkBrass"]);
        }

        private static void AddGaugeFace(Transform parent, string name, Vector3 localPosition, float radius, IReadOnlyDictionary<string, Material> materials, IReadOnlyDictionary<string, Mesh> meshes, bool amberFace)
        {
            var gauge = new GameObject(name);
            gauge.transform.SetParent(parent, false);
            gauge.transform.localPosition = localPosition;

            Primitive(gauge.transform, PrimitiveType.Cylinder, "blackened_iron_backplate", Vector3.zero, new Vector3(radius * 1.08f, 0.030f, radius * 1.08f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Primitive(gauge.transform, PrimitiveType.Cylinder, "aged_brass_outer_rim", new Vector3(0f, 0f, -0.025f), new Vector3(radius, 0.022f, radius), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(gauge.transform, PrimitiveType.Cylinder, "glowing_glass_gauge_face", new Vector3(0f, 0f, -0.050f), new Vector3(radius * 0.76f, 0.010f, radius * 0.76f), new Vector3(90f, 0f, 0f), amberFace ? materials["amber"] : materials["greenGlass"]);
            MeshPart(gauge.transform, meshes["gaugeNeedle"], "blackened_iron_pressure_needle", new Vector3(radius * 0.10f, 0f, -0.075f), new Vector3(radius * 0.84f, radius * 0.70f, radius * 0.45f), new Vector3(0f, 0f, -25f), materials["iron"]);
            MeshPart(gauge.transform, meshes["hexBolt"], "aged_brass_center_pin", new Vector3(0f, 0f, -0.090f), Vector3.one * radius * 0.20f, new Vector3(90f, 0f, 0f), materials["brass"]);

            for (var i = 0; i < 8; i++)
            {
                var angle = i * Mathf.PI * 2f / 8f;
                var pos = new Vector3(Mathf.Cos(angle) * radius * 0.70f, Mathf.Sin(angle) * radius * 0.70f, -0.080f);
                Primitive(gauge.transform, PrimitiveType.Cube, $"black_pressure_tick_{i:00}", pos, new Vector3(radius * 0.032f, radius * 0.11f, radius * 0.024f), new Vector3(0f, 0f, Mathf.Rad2Deg * angle), materials["iron"]);
            }
        }

        private static void AddRivetRow(Transform parent, string rowName, float x, Material material, Mesh boltMesh)
        {
            var row = new GameObject(rowName);
            row.transform.SetParent(parent, false);

            for (var i = 0; i < 4; i++)
            {
                MeshPart(row.transform, boltMesh, $"hex_receiver_bolt_{i:00}", new Vector3(x, -0.05f + i * 0.08f, -0.57f), Vector3.one * 0.050f, new Vector3(90f, 0f, 0f), material);
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
                    MeshPart(grid.transform, boltMesh, $"hex_mount_bolt_{y:00}_{x:00}", center + new Vector3(px, py, 0f), Vector3.one * 0.065f, new Vector3(90f, 0f, 0f), material);
                }
            }
        }

        private static GameObject NewRoot(string assetId, string role, params string[] promotionNotes)
        {
            var root = new GameObject(assetId);
            root.AddComponent<WeaponPropsSet02Identity>().Configure(assetId, role, 0, MaterialTags, promotionNotes);
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

        private static void SavePrefab(string root, GameObject prefabRoot, string assetName)
        {
            var identity = prefabRoot.GetComponent<WeaponPropsSet02Identity>();
            if (identity != null)
            {
                identity.Configure(assetName, identity.AssetRole, prefabRoot.GetComponentsInChildren<Renderer>().Length, MaterialTags, identity.PromotionNotes);
            }

            var path = $"{root}/Runtime/Prefabs/{assetName}.prefab";
            ReplaceAsset(path);
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            UnityEngine.Object.DestroyImmediate(prefabRoot);
        }

        private static PackageRoot LocatePackageRoot()
        {
            var package = PackageInfo.FindForAssembly(typeof(WeaponPropsSet02Generator).Assembly);
            if (package != null)
            {
                return new PackageRoot(package.assetPath, package.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(WeaponPropsSet02Generator));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/WeaponPropsSet02Generator.cs";
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
