using System;
using System.Collections.Generic;
using System.IO;
using BrassworksBreach.SteampunkWeapons;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.SteampunkWeapons.Editor
{
    public static class SteampunkWeaponPackGenerator
    {
        public const string PackageName = "com.brassworks.sidecar.steampunk-weapons";
        private const string Version = "v0.1.37";

        private static readonly string[] MaterialTags =
        {
            "aged_brass",
            "oxidized_copper",
            "blackened_iron",
            "dark_varnished_wood",
            "worn_leather",
            "glowing_amber_glass",
            "oily_wet_stone"
        };

        [MenuItem("Brassworks Breach/Sidecar Packs/Generate Steampunk Weapon Pack v0.1.37")]
        public static void GenerateAll()
        {
            var packageRoot = LocatePackageRoot();
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Materials");
            EnsurePhysicalFolder(packageRoot.ResolvedPath, "Runtime/Prefabs");

            var materials = CreateMaterials(packageRoot.AssetPath);

            CreatePressurePistolCore(packageRoot.AssetPath, materials);
            CreateCopperCoilAssembly(packageRoot.AssetPath, materials);
            CreateBrassDialGaugeAssembly(packageRoot.AssetPath, materials);
            CreateLeatherGrip(packageRoot.AssetPath, materials);
            CreatePressureCartridge(packageRoot.AssetPath, materials);
            CreateAmmoCabinetShell(packageRoot.AssetPath, materials);
            CreateWallWeaponDisplay(packageRoot.AssetPath, materials);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"BB_STEAMPUNK_WEAPONS_GENERATE_PASS {Version} root={packageRoot.AssetPath}");
        }

        public static string[] GeneratedPrefabAssetPaths()
        {
            var root = LocatePackageRoot().AssetPath;
            return new[]
            {
                $"{root}/Runtime/Prefabs/BB_V0137_PressurePistolCore.prefab",
                $"{root}/Runtime/Prefabs/BB_V0137_CopperCoilAssembly.prefab",
                $"{root}/Runtime/Prefabs/BB_V0137_BrassDialGaugeAssembly.prefab",
                $"{root}/Runtime/Prefabs/BB_V0137_LeatherGrip.prefab",
                $"{root}/Runtime/Prefabs/BB_V0137_PressureCartridge.prefab",
                $"{root}/Runtime/Prefabs/BB_V0137_AmmoCabinetShell.prefab",
                $"{root}/Runtime/Prefabs/BB_V0137_WallWeaponDisplay.prefab"
            };
        }

        private static Dictionary<string, Material> CreateMaterials(string root)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible Lit or Standard shader found.");
            }

            return new Dictionary<string, Material>
            {
                ["brass"] = SaveMaterial(root, shader, "BB_AgedBrass", new Color(0.83f, 0.55f, 0.24f), 0.92f, 0.42f),
                ["copper"] = SaveMaterial(root, shader, "BB_OxidizedCopper", new Color(0.78f, 0.32f, 0.16f), 0.88f, 0.36f),
                ["iron"] = SaveMaterial(root, shader, "BB_BlackenedIron", new Color(0.045f, 0.04f, 0.035f), 0.76f, 0.28f),
                ["wood"] = SaveMaterial(root, shader, "BB_DarkVarnishedWood", new Color(0.24f, 0.105f, 0.045f), 0.05f, 0.50f),
                ["leather"] = SaveMaterial(root, shader, "BB_WornLeather", new Color(0.30f, 0.17f, 0.09f), 0.02f, 0.34f),
                ["amber"] = SaveMaterial(root, shader, "BB_GlowingAmberGlass", new Color(1.0f, 0.48f, 0.09f), 0.0f, 0.82f, new Color(1.0f, 0.35f, 0.06f) * 1.35f),
                ["stone"] = SaveMaterial(root, shader, "BB_OilyWetStone", new Color(0.055f, 0.052f, 0.047f), 0.02f, 0.72f)
            };
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

        private static void CreatePressurePistolCore(string root, IReadOnlyDictionary<string, Material> materials)
        {
            var weapon = NewRoot("BB_V0137_PressurePistolCore", "primary hand weapon with pressure chamber, coils, gauge, and leather grip");

            Primitive(weapon, PrimitiveType.Cylinder, "blackened_iron_long_barrel", new Vector3(0f, 0.16f, 0.30f), new Vector3(0.11f, 0.82f, 0.11f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Primitive(weapon, PrimitiveType.Cylinder, "brass_muzzle_collar", new Vector3(0f, 0.16f, 1.12f), new Vector3(0.16f, 0.08f, 0.16f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(weapon, PrimitiveType.Cylinder, "brass_pressure_chamber", new Vector3(0f, -0.10f, 0.22f), new Vector3(0.24f, 0.44f, 0.24f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(weapon, PrimitiveType.Cylinder, "amber_glass_pressure_core", new Vector3(0f, -0.10f, 0.23f), new Vector3(0.13f, 0.47f, 0.13f), new Vector3(90f, 0f, 0f), materials["amber"]);

            for (var i = 0; i < 9; i++)
            {
                var z = -0.30f + i * 0.075f;
                Primitive(weapon, PrimitiveType.Cylinder, $"oxidized_copper_coil_band_{i:00}", new Vector3(0f, 0.16f, z), new Vector3(0.15f, 0.012f, 0.15f), new Vector3(90f, 0f, 0f), materials["copper"]);
            }

            Primitive(weapon, PrimitiveType.Cube, "blackened_iron_receiver_block", new Vector3(0f, 0.04f, -0.42f), new Vector3(0.36f, 0.28f, 0.30f), Vector3.zero, materials["iron"]);
            Primitive(weapon, PrimitiveType.Cube, "dark_wood_grip_core", new Vector3(0f, -0.44f, -0.55f), new Vector3(0.24f, 0.54f, 0.16f), new Vector3(18f, 0f, 0f), materials["wood"]);
            Primitive(weapon, PrimitiveType.Cube, "worn_leather_grip_wrap_front", new Vector3(0f, -0.43f, -0.49f), new Vector3(0.27f, 0.43f, 0.035f), new Vector3(18f, 0f, 0f), materials["leather"]);
            Primitive(weapon, PrimitiveType.Cube, "worn_leather_grip_wrap_back", new Vector3(0f, -0.46f, -0.63f), new Vector3(0.27f, 0.38f, 0.035f), new Vector3(18f, 0f, 0f), materials["leather"]);
            Primitive(weapon, PrimitiveType.Cube, "brass_trigger_guard_lower", new Vector3(0f, -0.23f, -0.35f), new Vector3(0.30f, 0.035f, 0.34f), new Vector3(8f, 0f, 0f), materials["brass"]);
            Primitive(weapon, PrimitiveType.Cube, "brass_trigger_guard_rear", new Vector3(0f, -0.14f, -0.50f), new Vector3(0.30f, 0.035f, 0.18f), new Vector3(-36f, 0f, 0f), materials["brass"]);
            Primitive(weapon, PrimitiveType.Cube, "blackened_iron_trigger", new Vector3(0f, -0.18f, -0.42f), new Vector3(0.055f, 0.18f, 0.035f), new Vector3(-22f, 0f, 0f), materials["iron"]);

            AddGaugeFace(weapon.transform, "top_brass_pressure_gauge", new Vector3(0f, 0.38f, -0.29f), 0.17f, materials);
            AddRivetRow(weapon.transform, "left_receiver_rivets", -0.205f, materials["brass"]);
            AddRivetRow(weapon.transform, "right_receiver_rivets", 0.205f, materials["brass"]);

            SavePrefab(root, weapon, "BB_V0137_PressurePistolCore");
        }

        private static void CreateCopperCoilAssembly(string root, IReadOnlyDictionary<string, Material> materials)
        {
            var coil = NewRoot("BB_V0137_CopperCoilAssembly", "modular visible copper coil assembly for weapons and wall machinery");

            Primitive(coil, PrimitiveType.Cylinder, "amber_pressure_tube", Vector3.zero, new Vector3(0.12f, 0.68f, 0.12f), new Vector3(90f, 0f, 0f), materials["amber"]);
            Primitive(coil, PrimitiveType.Cylinder, "blackened_iron_inner_spine", Vector3.zero, new Vector3(0.055f, 0.75f, 0.055f), new Vector3(90f, 0f, 0f), materials["iron"]);
            for (var i = 0; i < 14; i++)
            {
                Primitive(coil, PrimitiveType.Cylinder, $"oxidized_copper_pressure_loop_{i:00}", new Vector3(0f, 0f, -0.64f + i * 0.10f), new Vector3(0.19f, 0.014f, 0.19f), new Vector3(90f, 0f, 0f), materials["copper"]);
            }

            Primitive(coil, PrimitiveType.Cube, "brass_left_cradle", new Vector3(-0.27f, -0.12f, 0f), new Vector3(0.055f, 0.16f, 1.42f), Vector3.zero, materials["brass"]);
            Primitive(coil, PrimitiveType.Cube, "brass_right_cradle", new Vector3(0.27f, -0.12f, 0f), new Vector3(0.055f, 0.16f, 1.42f), Vector3.zero, materials["brass"]);
            for (var i = 0; i < 4; i++)
            {
                var z = -0.54f + i * 0.36f;
                Primitive(coil, PrimitiveType.Sphere, $"brass_cradle_rivet_left_{i:00}", new Vector3(-0.27f, -0.02f, z), new Vector3(0.055f, 0.055f, 0.055f), Vector3.zero, materials["brass"]);
                Primitive(coil, PrimitiveType.Sphere, $"brass_cradle_rivet_right_{i:00}", new Vector3(0.27f, -0.02f, z), new Vector3(0.055f, 0.055f, 0.055f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(root, coil, "BB_V0137_CopperCoilAssembly");
        }

        private static void CreateBrassDialGaugeAssembly(string root, IReadOnlyDictionary<string, Material> materials)
        {
            var gauge = NewRoot("BB_V0137_BrassDialGaugeAssembly", "readable brass pressure dial with glass face and needle");
            AddGaugeFace(gauge.transform, "front_pressure_gauge", Vector3.zero, 0.38f, materials);
            Primitive(gauge, PrimitiveType.Cylinder, "blackened_iron_pipe_socket", new Vector3(0f, -0.38f, 0.06f), new Vector3(0.10f, 0.22f, 0.10f), Vector3.zero, materials["iron"]);
            SavePrefab(root, gauge, "BB_V0137_BrassDialGaugeAssembly");
        }

        private static void CreateLeatherGrip(string root, IReadOnlyDictionary<string, Material> materials)
        {
            var grip = NewRoot("BB_V0137_LeatherGrip", "standalone dark wood and worn leather grip module");

            Primitive(grip, PrimitiveType.Cube, "dark_varnished_wood_spine", Vector3.zero, new Vector3(0.28f, 0.72f, 0.18f), new Vector3(12f, 0f, 0f), materials["wood"]);
            for (var i = 0; i < 5; i++)
            {
                Primitive(grip, PrimitiveType.Cube, $"worn_leather_wrap_band_{i:00}", new Vector3(0f, -0.27f + i * 0.135f, -0.02f), new Vector3(0.31f, 0.045f, 0.22f), new Vector3(12f, 0f, 0f), materials["leather"]);
            }

            for (var i = 0; i < 3; i++)
            {
                var y = -0.22f + i * 0.20f;
                Primitive(grip, PrimitiveType.Cylinder, $"brass_pin_left_{i:00}", new Vector3(-0.17f, y, -0.02f), new Vector3(0.035f, 0.02f, 0.035f), new Vector3(0f, 0f, 90f), materials["brass"]);
                Primitive(grip, PrimitiveType.Cylinder, $"brass_pin_right_{i:00}", new Vector3(0.17f, y, -0.02f), new Vector3(0.035f, 0.02f, 0.035f), new Vector3(0f, 0f, 90f), materials["brass"]);
            }

            SavePrefab(root, grip, "BB_V0137_LeatherGrip");
        }

        private static void CreatePressureCartridge(string root, IReadOnlyDictionary<string, Material> materials)
        {
            var cartridge = NewRoot("BB_V0137_PressureCartridge", "handheld amber pressure cartridge pickup module");

            Primitive(cartridge, PrimitiveType.Cylinder, "amber_glass_pressure_vial", Vector3.zero, new Vector3(0.10f, 0.44f, 0.10f), new Vector3(90f, 0f, 0f), materials["amber"]);
            Primitive(cartridge, PrimitiveType.Cylinder, "brass_front_cap", new Vector3(0f, 0f, 0.47f), new Vector3(0.13f, 0.055f, 0.13f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(cartridge, PrimitiveType.Cylinder, "brass_rear_cap", new Vector3(0f, 0f, -0.47f), new Vector3(0.13f, 0.055f, 0.13f), new Vector3(90f, 0f, 0f), materials["brass"]);
            for (var i = 0; i < 4; i++)
            {
                Primitive(cartridge, PrimitiveType.Cylinder, $"copper_pressure_band_{i:00}", new Vector3(0f, 0f, -0.30f + i * 0.20f), new Vector3(0.125f, 0.012f, 0.125f), new Vector3(90f, 0f, 0f), materials["copper"]);
            }

            for (var i = 0; i < 4; i++)
            {
                Primitive(cartridge, PrimitiveType.Cube, $"brass_stamped_notch_{i:00}", new Vector3(0f, 0.105f, -0.22f + i * 0.15f), new Vector3(0.035f, 0.018f, 0.06f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(root, cartridge, "BB_V0137_PressureCartridge");
        }

        private static void CreateAmmoCabinetShell(string root, IReadOnlyDictionary<string, Material> materials)
        {
            var cabinet = NewRoot("BB_V0137_AmmoCabinetShell", "wall-mounted ammo cabinet shell with cartridge rows, brass frame, and gauge");

            Primitive(cabinet, PrimitiveType.Cube, "oily_wet_stone_backplate", new Vector3(0f, 0f, 0.08f), new Vector3(1.35f, 1.55f, 0.08f), Vector3.zero, materials["stone"]);
            Primitive(cabinet, PrimitiveType.Cube, "dark_wood_cabinet_body", Vector3.zero, new Vector3(1.18f, 1.34f, 0.16f), Vector3.zero, materials["wood"]);
            Primitive(cabinet, PrimitiveType.Cube, "brass_top_frame", new Vector3(0f, 0.71f, -0.03f), new Vector3(1.28f, 0.06f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(cabinet, PrimitiveType.Cube, "brass_bottom_frame", new Vector3(0f, -0.71f, -0.03f), new Vector3(1.28f, 0.06f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(cabinet, PrimitiveType.Cube, "brass_left_frame", new Vector3(-0.64f, 0f, -0.03f), new Vector3(0.06f, 1.44f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(cabinet, PrimitiveType.Cube, "brass_right_frame", new Vector3(0.64f, 0f, -0.03f), new Vector3(0.06f, 1.44f, 0.18f), Vector3.zero, materials["brass"]);
            Primitive(cabinet, PrimitiveType.Cube, "amber_glass_door_panel", new Vector3(0f, 0f, -0.135f), new Vector3(1.02f, 1.04f, 0.035f), Vector3.zero, materials["amber"]);

            for (var row = 0; row < 3; row++)
            {
                var y = 0.38f - row * 0.36f;
                Primitive(cabinet, PrimitiveType.Cube, $"dark_wood_shelf_{row:00}", new Vector3(0f, y - 0.14f, -0.17f), new Vector3(1.02f, 0.04f, 0.22f), Vector3.zero, materials["wood"]);
                for (var col = 0; col < 5; col++)
                {
                    var x = -0.40f + col * 0.20f;
                    Primitive(cabinet, PrimitiveType.Cylinder, $"amber_pressure_cartridge_{row:00}_{col:00}", new Vector3(x, y, -0.20f), new Vector3(0.04f, 0.135f, 0.04f), new Vector3(0f, 0f, 0f), materials["amber"]);
                    Primitive(cabinet, PrimitiveType.Cylinder, $"brass_cartridge_cap_{row:00}_{col:00}", new Vector3(x, y + 0.15f, -0.20f), new Vector3(0.047f, 0.018f, 0.047f), Vector3.zero, materials["brass"]);
                }
            }

            AddGaugeFace(cabinet.transform, "cabinet_pressure_meter", new Vector3(0.42f, 0.56f, -0.19f), 0.13f, materials);
            SavePrefab(root, cabinet, "BB_V0137_AmmoCabinetShell");
        }

        private static void CreateWallWeaponDisplay(string root, IReadOnlyDictionary<string, Material> materials)
        {
            var display = NewRoot("BB_V0137_WallWeaponDisplay", "brass and dark wood wall display with hooks, rails, and weapon silhouette");

            Primitive(display, PrimitiveType.Cube, "dark_wood_wall_plank", new Vector3(0f, 0f, 0.08f), new Vector3(1.85f, 1.02f, 0.12f), Vector3.zero, materials["wood"]);
            Primitive(display, PrimitiveType.Cube, "oily_wet_stone_mount_shadow", new Vector3(0f, 0f, 0.16f), new Vector3(2.0f, 1.15f, 0.045f), Vector3.zero, materials["stone"]);

            for (var i = 0; i < 3; i++)
            {
                var y = -0.32f + i * 0.32f;
                Primitive(display, PrimitiveType.Cylinder, $"brass_horizontal_pipe_rail_{i:00}", new Vector3(0f, y, -0.04f), new Vector3(0.035f, 0.86f, 0.035f), new Vector3(0f, 0f, 90f), materials["brass"]);
            }

            for (var i = 0; i < 4; i++)
            {
                var x = -0.66f + i * 0.44f;
                Primitive(display, PrimitiveType.Cube, $"blackened_iron_weapon_hook_{i:00}", new Vector3(x, -0.02f, -0.17f), new Vector3(0.07f, 0.36f, 0.07f), new Vector3(0f, 0f, 18f), materials["iron"]);
                Primitive(display, PrimitiveType.Sphere, $"brass_hook_rivet_{i:00}", new Vector3(x, 0.24f, -0.17f), new Vector3(0.06f, 0.06f, 0.06f), Vector3.zero, materials["brass"]);
            }

            Primitive(display, PrimitiveType.Cylinder, "display_pressure_pistol_barrel", new Vector3(0f, 0.03f, -0.27f), new Vector3(0.07f, 0.60f, 0.07f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Primitive(display, PrimitiveType.Cylinder, "display_pressure_chamber", new Vector3(-0.22f, -0.08f, -0.27f), new Vector3(0.13f, 0.24f, 0.13f), new Vector3(90f, 0f, 0f), materials["brass"]);
            for (var i = 0; i < 7; i++)
            {
                Primitive(display, PrimitiveType.Cylinder, $"display_copper_coil_{i:00}", new Vector3(-0.05f + i * 0.06f, 0.03f, -0.27f), new Vector3(0.085f, 0.008f, 0.085f), new Vector3(90f, 0f, 0f), materials["copper"]);
            }

            Primitive(display, PrimitiveType.Cube, "display_worn_leather_grip", new Vector3(-0.48f, -0.34f, -0.27f), new Vector3(0.14f, 0.36f, 0.08f), new Vector3(0f, 0f, -22f), materials["leather"]);
            AddGaugeFace(display.transform, "display_tag_pressure_gauge", new Vector3(0.68f, 0.33f, -0.16f), 0.11f, materials);

            SavePrefab(root, display, "BB_V0137_WallWeaponDisplay");
        }

        private static GameObject NewRoot(string assetId, string role)
        {
            var root = new GameObject(assetId);
            root.AddComponent<SteampunkWeaponPackIdentity>().Configure(assetId, role, 0, MaterialTags);
            return root;
        }

        private static GameObject Primitive(GameObject root, PrimitiveType type, string name, Vector3 localPosition, Vector3 localScale, Vector3 localEuler, Material material)
        {
            return Primitive(root.transform, type, name, localPosition, localScale, localEuler, material);
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

        private static void AddGaugeFace(Transform parent, string name, Vector3 localPosition, float radius, IReadOnlyDictionary<string, Material> materials)
        {
            var gauge = new GameObject(name);
            gauge.transform.SetParent(parent, false);
            gauge.transform.localPosition = localPosition;

            Primitive(gauge.transform, PrimitiveType.Cylinder, "blackened_iron_backplate", Vector3.zero, new Vector3(radius * 1.05f, 0.035f, radius * 1.05f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Primitive(gauge.transform, PrimitiveType.Cylinder, "aged_brass_outer_rim", new Vector3(0f, 0f, -0.025f), new Vector3(radius, 0.026f, radius), new Vector3(90f, 0f, 0f), materials["brass"]);
            Primitive(gauge.transform, PrimitiveType.Cylinder, "amber_glass_gauge_face", new Vector3(0f, 0f, -0.055f), new Vector3(radius * 0.78f, 0.010f, radius * 0.78f), new Vector3(90f, 0f, 0f), materials["amber"]);
            Primitive(gauge.transform, PrimitiveType.Cube, "blackened_iron_pressure_needle", new Vector3(radius * 0.15f, 0f, -0.075f), new Vector3(radius * 0.52f, radius * 0.035f, radius * 0.030f), new Vector3(0f, 0f, -28f), materials["iron"]);
            Primitive(gauge.transform, PrimitiveType.Sphere, "brass_center_pin", new Vector3(0f, 0f, -0.09f), Vector3.one * radius * 0.15f, Vector3.zero, materials["brass"]);

            for (var i = 0; i < 8; i++)
            {
                var angle = i * Mathf.PI * 2f / 8f;
                var pos = new Vector3(Mathf.Cos(angle) * radius * 0.72f, Mathf.Sin(angle) * radius * 0.72f, -0.08f);
                Primitive(gauge.transform, PrimitiveType.Cube, $"black_pressure_tick_{i:00}", pos, new Vector3(radius * 0.035f, radius * 0.12f, radius * 0.025f), new Vector3(0f, 0f, Mathf.Rad2Deg * angle), materials["iron"]);
            }
        }

        private static void AddRivetRow(Transform parent, string rowName, float x, Material material)
        {
            var row = new GameObject(rowName);
            row.transform.SetParent(parent, false);

            for (var i = 0; i < 4; i++)
            {
                Primitive(row.transform, PrimitiveType.Sphere, $"brass_receiver_rivet_{i:00}", new Vector3(x, -0.02f + i * 0.08f, -0.58f), new Vector3(0.035f, 0.035f, 0.035f), Vector3.zero, material);
            }
        }

        private static void SavePrefab(string root, GameObject prefabRoot, string assetName)
        {
            var identity = prefabRoot.GetComponent<SteampunkWeaponPackIdentity>();
            if (identity != null)
            {
                identity.Configure(assetName, identity.AssetRole, prefabRoot.GetComponentsInChildren<Renderer>().Length, MaterialTags);
            }

            var path = $"{root}/Runtime/Prefabs/{assetName}.prefab";
            ReplaceAsset(path);
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            UnityEngine.Object.DestroyImmediate(prefabRoot);
        }

        private static PackageRoot LocatePackageRoot()
        {
            var package = PackageInfo.FindForAssembly(typeof(SteampunkWeaponPackGenerator).Assembly);
            if (package != null)
            {
                return new PackageRoot(package.assetPath, package.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(SteampunkWeaponPackGenerator));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/SteampunkWeaponPackGenerator.cs";
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
