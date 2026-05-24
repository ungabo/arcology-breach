#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace BrassworksBreach.Sidecars.SteamworksLevelKit
{
    public static class SteamworksLevelKitGenerator
    {
        public const string Version = "0.1.37";
        public const string BuildId = "p001";
        public const string PackageName = "com.brassworks.sidecar.steamworks-level-kit";
        public const string RenderOutputRelativePath = "Documentation/ConceptRenders/V0_1_37_SteamworksLevelKitSidecar";

        private const string MenuRoot = "Brassworks/Sidecars/Steamworks Level Kit v0.1.37/";

        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private static readonly List<GeneratedPrefabRecord> GeneratedPrefabs = new List<GeneratedPrefabRecord>();

        [MenuItem(MenuRoot + "Generate Package Assets")]
        public static void GeneratePackageAssets()
        {
            GeneratedPrefabs.Clear();
            Materials.Clear();
            Meshes.Clear();

            EnsureFolders();
            CreateMaterials();
            CreateMeshes();

            CreatePrefab("SCLVL_CorridorStraight_4m.prefab", "corridor", "Straight 4m corridor module", new Vector3(4f, 3.2f, 4f), "route shell", BuildCorridorStraight);
            CreatePrefab("SCLVL_CorridorCorner_4m.prefab", "corridor", "Ninety-degree 4m corner corridor", new Vector3(4f, 3.2f, 4f), "route turn", BuildCorridorCorner);
            CreatePrefab("SCLVL_TJunction_4m.prefab", "corridor", "4m T-junction combat read module", new Vector3(8f, 3.2f, 4f), "junction", BuildTJunction);
            CreatePrefab("SCLVL_BoilerAlcove_4m.prefab", "setpiece", "Boiler alcove wall module", new Vector3(4f, 3.4f, 1.3f), "landmark wall", BuildBoilerAlcove);
            CreatePrefab("SCLVL_GaugeWall_4m.prefab", "setpiece", "Gauge wall and pressure read module", new Vector3(4f, 3.2f, 0.65f), "objective wall", BuildGaugeWall);
            CreatePrefab("SCLVL_RivetedVaultDoor_4m.prefab", "door", "Riveted round vault door", new Vector3(4.4f, 3.6f, 0.8f), "closed landmark door", BuildRivetedVaultDoor);
            CreatePrefab("SCLVL_PressureLockDoorFrame_4m.prefab", "door", "Pressure-lock door frame with clear aperture", new Vector3(4.2f, 3.2f, 0.65f), "open transition frame", BuildPressureLockDoorFrame);
            CreatePrefab("SCLVL_PipeRailing_4m.prefab", "trim", "Pipe railing module", new Vector3(4f, 1.2f, 0.2f), "edge guard", BuildPipeRailing);
            CreatePrefab("SCLVL_CatwalkFloor_4m.prefab", "floor", "Riveted catwalk floor plate", new Vector3(4f, 0.22f, 4f), "walkable floor", BuildCatwalkFloor);
            CreatePrefab("SCLVL_WallColumn_3m.prefab", "trim", "Riveted wall column", new Vector3(0.55f, 3.2f, 0.45f), "wall rhythm", BuildWallColumn);
            CreatePrefab("SCLVL_CeilingPipeCluster_4m.prefab", "ceiling", "Ceiling pipe cluster", new Vector3(4f, 0.65f, 1.2f), "ceiling dressing", BuildCeilingPipeCluster);
            CreatePrefab("SCLVL_ValveConsole.prefab", "prop", "Valve console with pressure read", new Vector3(1.35f, 1.45f, 0.9f), "objective prop", BuildValveConsole);
            CreatePrefab("SCLVL_VentSmokeEmitterAnchor.prefab", "vfx_anchor", "Vent and smoke emitter anchor", new Vector3(0.9f, 0.75f, 0.6f), "vfx socket", BuildVentSmokeEmitterAnchor);

            WriteGeneratedManifest();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Steamworks Level Kit v0.1.37 generated " + GeneratedPrefabs.Count + " prefabs, " + Materials.Count + " materials, and " + Meshes.Count + " mesh assets.");
        }

        public static IReadOnlyList<string> EnsureGeneratedPrefabPaths()
        {
            string prefabPath = CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_CorridorStraight_4m.prefab");
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) == null)
            {
                GeneratePackageAssets();
            }

            return new[]
            {
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_CorridorStraight_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_CorridorCorner_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_TJunction_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_BoilerAlcove_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_GaugeWall_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_RivetedVaultDoor_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_PressureLockDoorFrame_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_PipeRailing_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_CatwalkFloor_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_WallColumn_3m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_CeilingPipeCluster_4m.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_ValveConsole.prefab"),
                CombineAssetPath(PackageRoot, "Runtime/Prefabs/SCLVL_VentSmokeEmitterAnchor.prefab")
            };
        }

        private static string PackageRoot
        {
            get
            {
                PackageInfo packageInfo = PackageInfo.FindForAssembly(typeof(SteamworksLevelKitGenerator).Assembly);
                if (packageInfo != null && !string.IsNullOrEmpty(packageInfo.assetPath))
                {
                    return NormalizeAssetPath(packageInfo.assetPath);
                }

                return "Packages/" + PackageName;
            }
        }

        private static string ProjectRoot
        {
            get { return Directory.GetParent(Application.dataPath).FullName; }
        }

        private static void EnsureFolders()
        {
            EnsureAssetFolder(CombineAssetPath(PackageRoot, "Runtime/Prefabs"));
            EnsureAssetFolder(CombineAssetPath(PackageRoot, "Runtime/Materials"));
            EnsureAssetFolder(CombineAssetPath(PackageRoot, "Runtime/Meshes"));
            EnsureAssetFolder(CombineAssetPath(PackageRoot, "Runtime/Metadata"));
            EnsureAssetFolder(CombineAssetPath(PackageRoot, "Samples~/PreviewScene"));
        }

        private static void EnsureAssetFolder(string assetPath)
        {
            Directory.CreateDirectory(ToSystemPath(assetPath));
        }

        private static string ToSystemPath(string assetPath)
        {
            return Path.GetFullPath(Path.Combine(ProjectRoot, assetPath.Replace("/", Path.DirectorySeparatorChar.ToString())));
        }

        private static string CombineAssetPath(string root, string relative)
        {
            return NormalizeAssetPath(root.TrimEnd('/') + "/" + relative.TrimStart('/'));
        }

        private static string NormalizeAssetPath(string path)
        {
            return path.Replace("\\", "/");
        }

        private static void CreateMaterials()
        {
            AddMaterial("SCLVL_BlackenedIron", new Color(0.045f, 0.042f, 0.038f), 0.24f, 0.78f);
            AddMaterial("SCLVL_AgedBrass", new Color(0.76f, 0.55f, 0.26f), 0.58f, 0.72f);
            AddMaterial("SCLVL_CopperSteamPipe", new Color(0.72f, 0.32f, 0.15f), 0.52f, 0.62f);
            AddMaterial("SCLVL_SootBrick", new Color(0.16f, 0.105f, 0.082f), 0.12f, 0.02f);
            AddMaterial("SCLVL_OilWetStone", new Color(0.075f, 0.071f, 0.064f), 0.5f, 0.04f);
            AddMaterial("SCLVL_WarmAmberGlass", new Color(1.0f, 0.52f, 0.16f), 0.45f, 0.08f, new Color(1.0f, 0.34f, 0.08f) * 1.8f);
            AddMaterial("SCLVL_PressureGreenGlass", new Color(0.16f, 0.82f, 0.38f), 0.38f, 0.06f, new Color(0.06f, 0.72f, 0.28f) * 1.2f);
            AddMaterial("SCLVL_HeatRedEnamel", new Color(0.75f, 0.055f, 0.035f), 0.42f, 0.18f);
            AddMaterial("SCLVL_BoilerGlow", new Color(1.0f, 0.28f, 0.05f), 0.25f, 0.05f, new Color(1.0f, 0.18f, 0.035f) * 2.7f);
            AddMaterial("SCLVL_GaugeIvory", new Color(0.78f, 0.69f, 0.49f), 0.28f, 0.01f);
            AddMaterial("SCLVL_RubberGasket", new Color(0.018f, 0.017f, 0.015f), 0.16f, 0.0f);
            AddMaterial("SCLVL_SteamWhite", new Color(0.75f, 0.72f, 0.67f, 0.55f), 0.1f, 0.0f);
        }

        private static void AddMaterial(string name, Color color, float smoothness, float metallic, Color? emission = null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            Material material = new Material(shader)
            {
                name = name,
                color = color
            };

            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            if (material.HasProperty("_Color")) material.SetColor("_Color", color);
            if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", smoothness);
            if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", metallic);
            if (emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                if (material.HasProperty("_EmissionColor")) material.SetColor("_EmissionColor", emission.Value);
            }

            string path = CombineAssetPath(PackageRoot, "Runtime/Materials/" + name + ".mat");
            CreateOrReplaceAsset(material, path);
            Materials[name] = AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        private static void CreateMeshes()
        {
            AddMesh("SCLVL_BoxUnit", CreateBoxMesh());
            AddMesh("SCLVL_Cylinder16Unit", CreateCylinderMesh(16));
            AddMesh("SCLVL_Cylinder24Unit", CreateCylinderMesh(24));
        }

        private static void AddMesh(string name, Mesh mesh)
        {
            mesh.name = name;
            string path = CombineAssetPath(PackageRoot, "Runtime/Meshes/" + name + ".asset");
            CreateOrReplaceAsset(mesh, path);
            Meshes[name] = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        private static void CreateOrReplaceAsset(UnityEngine.Object asset, string path)
        {
            if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            AssetDatabase.CreateAsset(asset, path);
        }

        private static Mesh CreateBoxMesh()
        {
            Vector3[] p =
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f)
            };

            int[] triangles =
            {
                0, 2, 1, 0, 3, 2,
                4, 6, 5, 4, 7, 6,
                8, 10, 9, 8, 11, 10,
                12, 14, 13, 12, 15, 14,
                16, 18, 17, 16, 19, 18,
                20, 22, 21, 20, 23, 22
            };

            Mesh mesh = new Mesh();
            mesh.SetVertices(p);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateCylinderMesh(int segments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int i = 0; i < segments; i++)
            {
                float angle0 = Mathf.PI * 2f * i / segments;
                float angle1 = Mathf.PI * 2f * (i + 1) / segments;
                Vector3 bottom0 = new Vector3(Mathf.Cos(angle0) * 0.5f, -0.5f, Mathf.Sin(angle0) * 0.5f);
                Vector3 bottom1 = new Vector3(Mathf.Cos(angle1) * 0.5f, -0.5f, Mathf.Sin(angle1) * 0.5f);
                Vector3 top0 = new Vector3(bottom0.x, 0.5f, bottom0.z);
                Vector3 top1 = new Vector3(bottom1.x, 0.5f, bottom1.z);

                int sideStart = vertices.Count;
                vertices.Add(bottom0);
                vertices.Add(top0);
                vertices.Add(top1);
                vertices.Add(bottom1);
                triangles.Add(sideStart);
                triangles.Add(sideStart + 1);
                triangles.Add(sideStart + 2);
                triangles.Add(sideStart);
                triangles.Add(sideStart + 2);
                triangles.Add(sideStart + 3);

                int topStart = vertices.Count;
                vertices.Add(Vector3.up * 0.5f);
                vertices.Add(top0);
                vertices.Add(top1);
                triangles.Add(topStart);
                triangles.Add(topStart + 1);
                triangles.Add(topStart + 2);

                int bottomStart = vertices.Count;
                vertices.Add(Vector3.down * 0.5f);
                vertices.Add(bottom1);
                vertices.Add(bottom0);
                triangles.Add(bottomStart);
                triangles.Add(bottomStart + 1);
                triangles.Add(bottomStart + 2);
            }

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void CreatePrefab(string fileName, string family, string displayName, Vector3 dimensions, string role, Action<GameObject> builder)
        {
            GameObject root = new GameObject(Path.GetFileNameWithoutExtension(fileName));
            root.transform.position = Vector3.zero;
            builder(root);
            AddMetadataChild(root, family, displayName, dimensions, role);
            string path = CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + fileName);
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            if (prefab != null)
            {
                AssetDatabase.SetLabels(prefab, new[] { "SCLVL", "v0.1.37", family, role.Replace(" ", "_") });
            }

            UnityEngine.Object.DestroyImmediate(root);
            GeneratedPrefabs.Add(new GeneratedPrefabRecord(path, family, displayName, dimensions, role));
        }

        private static void AddMetadataChild(GameObject root, string family, string displayName, Vector3 dimensions, string role)
        {
            GameObject metadata = new GameObject("SCLVL_METADATA__" + family + "__" + role.Replace(" ", "_"));
            metadata.transform.SetParent(root.transform, false);
            metadata.SetActive(false);
            metadata.name += "__" + FormatVector(dimensions) + "__grid4m";
        }

        private static string FormatVector(Vector3 value)
        {
            return value.x.ToString("0.##", CultureInfo.InvariantCulture) + "x" +
                   value.y.ToString("0.##", CultureInfo.InvariantCulture) + "x" +
                   value.z.ToString("0.##", CultureInfo.InvariantCulture) + "m";
        }

        private static Material Mat(string name)
        {
            return Materials[name];
        }

        private static Mesh Mesh(string name)
        {
            return Meshes[name];
        }

        private static GameObject Box(GameObject parent, string name, Vector3 position, Vector3 scale, string material)
        {
            return Part(parent, name, "SCLVL_BoxUnit", position, Quaternion.identity, scale, material);
        }

        private static GameObject Cyl(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string material, bool highRes = false)
        {
            return Part(parent, name, highRes ? "SCLVL_Cylinder24Unit" : "SCLVL_Cylinder16Unit", position, Quaternion.Euler(euler), scale, material);
        }

        private static GameObject Part(GameObject parent, string name, string meshName, Vector3 position, Quaternion rotation, Vector3 scale, string material)
        {
            GameObject part = new GameObject(name);
            part.transform.SetParent(parent.transform, false);
            part.transform.localPosition = position;
            part.transform.localRotation = rotation;
            part.transform.localScale = scale;
            MeshFilter filter = part.AddComponent<MeshFilter>();
            filter.sharedMesh = Mesh(meshName);
            MeshRenderer renderer = part.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = Mat(material);
            return part;
        }

        private static void BuildCorridorStraight(GameObject root)
        {
            Box(root, "floor_oilwet_stone_4x4", new Vector3(0f, -0.05f, 0f), new Vector3(4f, 0.1f, 4f), "SCLVL_OilWetStone");
            Box(root, "ceiling_blackened_iron_plate", new Vector3(0f, 3.15f, 0f), new Vector3(4f, 0.1f, 4f), "SCLVL_BlackenedIron");
            Box(root, "left_soot_brick_wall", new Vector3(-2.05f, 1.55f, 0f), new Vector3(0.12f, 3.1f, 4f), "SCLVL_SootBrick");
            Box(root, "right_soot_brick_wall", new Vector3(2.05f, 1.55f, 0f), new Vector3(0.12f, 3.1f, 4f), "SCLVL_SootBrick");
            AddWallPanels(root, -1.98f, -1f, true);
            AddWallPanels(root, 1.98f, -1f, true);
            AddFloorRivets(root, 4, 4);
            BuildCeilingPipeClusterAt(root, Vector3.zero);
        }

        private static void BuildCorridorCorner(GameObject root)
        {
            Box(root, "corner_floor_l_shape_a", new Vector3(0f, -0.05f, 0f), new Vector3(4f, 0.1f, 4f), "SCLVL_OilWetStone");
            Box(root, "outer_left_wall", new Vector3(-2.05f, 1.55f, 0f), new Vector3(0.12f, 3.1f, 4f), "SCLVL_SootBrick");
            Box(root, "outer_back_wall", new Vector3(0f, 1.55f, 2.05f), new Vector3(4f, 3.1f, 0.12f), "SCLVL_SootBrick");
            Box(root, "inner_corner_guard", new Vector3(1.15f, 1.2f, -1.15f), new Vector3(0.6f, 2.4f, 0.6f), "SCLVL_BlackenedIron");
            Cyl(root, "corner_vertical_pressure_pipe", new Vector3(1.15f, 1.55f, -1.15f), Vector3.zero, new Vector3(0.18f, 3.0f, 0.18f), "SCLVL_CopperSteamPipe");
            Cyl(root, "amber_turn_lamp", new Vector3(0.92f, 2.2f, -0.9f), new Vector3(90f, 0f, 0f), new Vector3(0.22f, 0.11f, 0.22f), "SCLVL_WarmAmberGlass", true);
            AddFloorRivets(root, 3, 3);
        }

        private static void BuildTJunction(GameObject root)
        {
            Box(root, "main_floor_plate", new Vector3(0f, -0.05f, 0f), new Vector3(8f, 0.1f, 4f), "SCLVL_OilWetStone");
            Box(root, "back_floor_branch", new Vector3(0f, -0.04f, 2f), new Vector3(4f, 0.08f, 4f), "SCLVL_OilWetStone");
            Box(root, "rear_pressure_wall", new Vector3(0f, 1.55f, 4.05f), new Vector3(4f, 3.1f, 0.12f), "SCLVL_SootBrick");
            Box(root, "left_wing_wall", new Vector3(-4.05f, 1.55f, 0f), new Vector3(0.12f, 3.1f, 4f), "SCLVL_SootBrick");
            Box(root, "right_wing_wall", new Vector3(4.05f, 1.55f, 0f), new Vector3(0.12f, 3.1f, 4f), "SCLVL_SootBrick");
            BuildGaugeCluster(root, new Vector3(0f, 2.05f, 3.92f), Quaternion.Euler(90f, 0f, 0f));
            Cyl(root, "junction_overhead_main_pipe", new Vector3(0f, 2.95f, 0f), new Vector3(0f, 0f, 90f), new Vector3(0.2f, 7.8f, 0.2f), "SCLVL_CopperSteamPipe", true);
            Cyl(root, "junction_branch_pipe", new Vector3(0f, 2.78f, 2.0f), new Vector3(90f, 0f, 0f), new Vector3(0.16f, 4.0f, 0.16f), "SCLVL_BlackenedIron");
            AddFloorRivets(root, 6, 3);
        }

        private static void BuildBoilerAlcove(GameObject root)
        {
            Box(root, "rear_soot_brick_bay", new Vector3(0f, 1.65f, 0.35f), new Vector3(4f, 3.3f, 0.14f), "SCLVL_SootBrick");
            Box(root, "left_riveted_pier", new Vector3(-1.9f, 1.65f, -0.05f), new Vector3(0.32f, 3.3f, 0.9f), "SCLVL_BlackenedIron");
            Box(root, "right_riveted_pier", new Vector3(1.9f, 1.65f, -0.05f), new Vector3(0.32f, 3.3f, 0.9f), "SCLVL_BlackenedIron");
            Cyl(root, "horizontal_boiler_tank", new Vector3(0f, 2.35f, -0.05f), new Vector3(0f, 0f, 90f), new Vector3(0.62f, 2.6f, 0.62f), "SCLVL_CopperSteamPipe", true);
            Box(root, "furnace_mouth_glow", new Vector3(0f, 0.92f, -0.22f), new Vector3(2.35f, 0.95f, 0.12f), "SCLVL_BoilerGlow");
            Box(root, "heat_warning_enamel_strip", new Vector3(0f, 0.36f, -0.31f), new Vector3(3.2f, 0.12f, 0.08f), "SCLVL_HeatRedEnamel");
            BuildGaugeCluster(root, new Vector3(-1.02f, 2.25f, -0.43f), Quaternion.Euler(90f, 0f, 0f));
            AddRivetLine(root, new Vector3(-1.9f, 3.05f, -0.51f), 6, 0.0f, 0.44f);
            AddRivetLine(root, new Vector3(1.9f, 3.05f, -0.51f), 6, 0.0f, 0.44f);
        }

        private static void BuildGaugeWall(GameObject root)
        {
            Box(root, "blackened_pressure_panel", new Vector3(0f, 1.6f, 0f), new Vector3(4f, 3.2f, 0.18f), "SCLVL_BlackenedIron");
            Box(root, "brass_horizontal_bus", new Vector3(0f, 1.12f, -0.13f), new Vector3(3.6f, 0.12f, 0.1f), "SCLVL_AgedBrass");
            Box(root, "brass_top_bus", new Vector3(0f, 2.72f, -0.13f), new Vector3(3.6f, 0.12f, 0.1f), "SCLVL_AgedBrass");
            for (int i = 0; i < 4; i++)
            {
                float x = -1.35f + i * 0.9f;
                BuildGaugeCluster(root, new Vector3(x, 2.05f, -0.16f), Quaternion.Euler(90f, 0f, 0f));
                Cyl(root, "vertical_feed_pipe_" + i, new Vector3(x, 1.25f, -0.22f), Vector3.zero, new Vector3(0.08f, 1.45f, 0.08f), "SCLVL_CopperSteamPipe");
            }
            Box(root, "green_route_state_lens", new Vector3(1.35f, 0.55f, -0.16f), new Vector3(0.52f, 0.18f, 0.08f), "SCLVL_PressureGreenGlass");
            Box(root, "red_overpressure_lens", new Vector3(-1.35f, 0.55f, -0.16f), new Vector3(0.52f, 0.18f, 0.08f), "SCLVL_HeatRedEnamel");
        }

        private static void BuildRivetedVaultDoor(GameObject root)
        {
            Cyl(root, "round_blackened_vault_slab", new Vector3(0f, 1.75f, 0f), new Vector3(90f, 0f, 0f), new Vector3(3.05f, 0.34f, 3.05f), "SCLVL_BlackenedIron", true);
            Cyl(root, "outer_brass_pressure_ring", new Vector3(0f, 1.75f, -0.22f), new Vector3(90f, 0f, 0f), new Vector3(3.45f, 0.08f, 3.45f), "SCLVL_AgedBrass", true);
            Cyl(root, "central_locking_hub", new Vector3(0f, 1.75f, -0.38f), new Vector3(90f, 0f, 0f), new Vector3(0.58f, 0.22f, 0.58f), "SCLVL_AgedBrass", true);
            for (int i = 0; i < 8; i++)
            {
                GameObject spoke = Box(root, "radial_lock_spoke_" + i, new Vector3(0f, 1.75f, -0.43f), new Vector3(1.55f, 0.08f, 0.08f), "SCLVL_CopperSteamPipe");
                spoke.transform.localRotation = Quaternion.Euler(0f, 0f, i * 45f);
            }
            for (int i = 0; i < 16; i++)
            {
                float angle = Mathf.PI * 2f * i / 16f;
                Vector3 pos = new Vector3(Mathf.Cos(angle) * 1.48f, 1.75f + Mathf.Sin(angle) * 1.48f, -0.45f);
                Cyl(root, "outer_ring_rivet_" + i, pos, new Vector3(90f, 0f, 0f), new Vector3(0.09f, 0.04f, 0.09f), "SCLVL_AgedBrass");
            }
            Box(root, "vault_floor_anchor_left", new Vector3(-1.75f, 0.18f, 0f), new Vector3(0.45f, 0.36f, 0.58f), "SCLVL_OilWetStone");
            Box(root, "vault_floor_anchor_right", new Vector3(1.75f, 0.18f, 0f), new Vector3(0.45f, 0.36f, 0.58f), "SCLVL_OilWetStone");
        }

        private static void BuildPressureLockDoorFrame(GameObject root)
        {
            Box(root, "left_pressure_jamb", new Vector3(-2f, 1.55f, 0f), new Vector3(0.34f, 3.1f, 0.5f), "SCLVL_BlackenedIron");
            Box(root, "right_pressure_jamb", new Vector3(2f, 1.55f, 0f), new Vector3(0.34f, 3.1f, 0.5f), "SCLVL_BlackenedIron");
            Box(root, "top_pressure_lintel", new Vector3(0f, 3.0f, 0f), new Vector3(4.25f, 0.32f, 0.5f), "SCLVL_BlackenedIron");
            Box(root, "bottom_gasket_sill", new Vector3(0f, 0.08f, 0f), new Vector3(4f, 0.12f, 0.5f), "SCLVL_RubberGasket");
            Cyl(root, "left_copper_piston", new Vector3(-1.58f, 1.55f, -0.28f), Vector3.zero, new Vector3(0.16f, 2.45f, 0.16f), "SCLVL_CopperSteamPipe");
            Cyl(root, "right_copper_piston", new Vector3(1.58f, 1.55f, -0.28f), Vector3.zero, new Vector3(0.16f, 2.45f, 0.16f), "SCLVL_CopperSteamPipe");
            Cyl(root, "top_lock_gear", new Vector3(0f, 2.62f, -0.31f), new Vector3(90f, 0f, 0f), new Vector3(0.62f, 0.1f, 0.62f), "SCLVL_AgedBrass", true);
            BuildGaugeCluster(root, new Vector3(0.8f, 2.55f, -0.38f), Quaternion.Euler(90f, 0f, 0f));
            Box(root, "amber_lock_status", new Vector3(-0.82f, 2.55f, -0.36f), new Vector3(0.52f, 0.16f, 0.08f), "SCLVL_WarmAmberGlass");
        }

        private static void BuildPipeRailing(GameObject root)
        {
            Box(root, "blackened_kick_plate", new Vector3(0f, 0.18f, 0f), new Vector3(4f, 0.36f, 0.1f), "SCLVL_BlackenedIron");
            for (int i = 0; i < 5; i++)
            {
                float x = -1.9f + i * 0.95f;
                Cyl(root, "vertical_brass_post_" + i, new Vector3(x, 0.72f, 0f), Vector3.zero, new Vector3(0.06f, 1.08f, 0.06f), "SCLVL_AgedBrass");
                Box(root, "post_foot_" + i, new Vector3(x, 0.04f, 0f), new Vector3(0.28f, 0.08f, 0.22f), "SCLVL_BlackenedIron");
            }
            Cyl(root, "top_copper_handrail", new Vector3(0f, 1.18f, 0f), new Vector3(0f, 0f, 90f), new Vector3(0.07f, 4.1f, 0.07f), "SCLVL_CopperSteamPipe");
            Cyl(root, "middle_brass_handrail", new Vector3(0f, 0.76f, 0f), new Vector3(0f, 0f, 90f), new Vector3(0.055f, 4.1f, 0.055f), "SCLVL_AgedBrass");
        }

        private static void BuildCatwalkFloor(GameObject root)
        {
            Box(root, "catwalk_blackened_plate", new Vector3(0f, 0f, 0f), new Vector3(4f, 0.12f, 4f), "SCLVL_BlackenedIron");
            for (int i = 0; i < 5; i++)
            {
                float x = -1.6f + i * 0.8f;
                Box(root, "raised_grate_slat_x_" + i, new Vector3(x, 0.09f, 0f), new Vector3(0.12f, 0.08f, 3.7f), "SCLVL_AgedBrass");
                Box(root, "raised_grate_slat_z_" + i, new Vector3(0f, 0.10f, x), new Vector3(3.7f, 0.07f, 0.1f), "SCLVL_CopperSteamPipe");
            }
            AddFloorRivets(root, 4, 4);
        }

        private static void BuildWallColumn(GameObject root)
        {
            Box(root, "rivet_column_core", new Vector3(0f, 1.6f, 0f), new Vector3(0.42f, 3.2f, 0.34f), "SCLVL_BlackenedIron");
            Cyl(root, "left_column_pipe", new Vector3(-0.18f, 1.6f, -0.21f), Vector3.zero, new Vector3(0.08f, 3.0f, 0.08f), "SCLVL_CopperSteamPipe");
            Cyl(root, "right_column_pipe", new Vector3(0.18f, 1.6f, -0.21f), Vector3.zero, new Vector3(0.08f, 3.0f, 0.08f), "SCLVL_AgedBrass");
            AddRivetLine(root, new Vector3(0f, 0.35f, -0.21f), 7, 0.42f, 0f);
        }

        private static void BuildCeilingPipeCluster(GameObject root)
        {
            BuildCeilingPipeClusterAt(root, Vector3.zero);
            Box(root, "ceiling_mount_plate", new Vector3(0f, 0.1f, 0f), new Vector3(4f, 0.1f, 1.1f), "SCLVL_BlackenedIron");
        }

        private static void BuildValveConsole(GameObject root)
        {
            Box(root, "console_black_iron_body", new Vector3(0f, 0.65f, 0f), new Vector3(1.1f, 1.1f, 0.72f), "SCLVL_BlackenedIron");
            Box(root, "sloped_brass_face", new Vector3(0f, 1.18f, -0.22f), new Vector3(1.0f, 0.22f, 0.52f), "SCLVL_AgedBrass");
            Cyl(root, "main_valve_wheel", new Vector3(0f, 1.25f, -0.48f), new Vector3(90f, 0f, 0f), new Vector3(0.58f, 0.08f, 0.58f), "SCLVL_CopperSteamPipe", true);
            BuildGaugeCluster(root, new Vector3(-0.32f, 1.04f, -0.51f), Quaternion.Euler(90f, 0f, 0f));
            Box(root, "green_pressure_ready_lamp", new Vector3(0.38f, 1.02f, -0.52f), new Vector3(0.22f, 0.13f, 0.08f), "SCLVL_PressureGreenGlass");
            Cyl(root, "floor_feed_pipe", new Vector3(0f, 0.18f, 0.45f), new Vector3(90f, 0f, 0f), new Vector3(0.14f, 0.9f, 0.14f), "SCLVL_CopperSteamPipe");
        }

        private static void BuildVentSmokeEmitterAnchor(GameObject root)
        {
            Box(root, "vent_wall_mount_plate", new Vector3(0f, 0.38f, 0f), new Vector3(0.9f, 0.72f, 0.12f), "SCLVL_BlackenedIron");
            Cyl(root, "vent_brass_ring", new Vector3(0f, 0.38f, -0.1f), new Vector3(90f, 0f, 0f), new Vector3(0.55f, 0.08f, 0.55f), "SCLVL_AgedBrass", true);
            Box(root, "vent_slat_upper", new Vector3(0f, 0.52f, -0.15f), new Vector3(0.55f, 0.05f, 0.06f), "SCLVL_CopperSteamPipe");
            Box(root, "vent_slat_mid", new Vector3(0f, 0.38f, -0.15f), new Vector3(0.55f, 0.05f, 0.06f), "SCLVL_CopperSteamPipe");
            Box(root, "vent_slat_lower", new Vector3(0f, 0.24f, -0.15f), new Vector3(0.55f, 0.05f, 0.06f), "SCLVL_CopperSteamPipe");
            GameObject socket = new GameObject("FX_SOCKET_SteamVent_Forward");
            socket.transform.SetParent(root.transform, false);
            socket.transform.localPosition = new Vector3(0f, 0.38f, -0.34f);
            socket.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            GameObject clearance = new GameObject("CLEARANCE_NOTE_keep_1m_forward_open");
            clearance.transform.SetParent(root.transform, false);
            clearance.transform.localPosition = new Vector3(0f, 0.38f, -0.85f);
        }

        private static void AddWallPanels(GameObject root, float x, float zOffset, bool lengthwise)
        {
            for (int i = 0; i < 3; i++)
            {
                float z = zOffset + i * 1.0f;
                Box(root, "wall_panel_" + x + "_" + i, new Vector3(x, 1.55f, z), new Vector3(0.08f, 1.45f, 0.72f), "SCLVL_BlackenedIron");
                Cyl(root, "panel_rivet_top_" + x + "_" + i, new Vector3(x, 2.35f, z - 0.28f), new Vector3(90f, 0f, 0f), new Vector3(0.055f, 0.025f, 0.055f), "SCLVL_AgedBrass");
                Cyl(root, "panel_rivet_bottom_" + x + "_" + i, new Vector3(x, 0.75f, z + 0.28f), new Vector3(90f, 0f, 0f), new Vector3(0.055f, 0.025f, 0.055f), "SCLVL_AgedBrass");
            }
        }

        private static void AddFloorRivets(GameObject root, int xCount, int zCount)
        {
            for (int ix = 0; ix < xCount; ix++)
            {
                for (int iz = 0; iz < zCount; iz++)
                {
                    float x = -1.5f + ix * (3f / Math.Max(1, xCount - 1));
                    float z = -1.5f + iz * (3f / Math.Max(1, zCount - 1));
                    Cyl(root, "floor_rivet_" + ix + "_" + iz, new Vector3(x, 0.045f, z), Vector3.zero, new Vector3(0.06f, 0.025f, 0.06f), "SCLVL_AgedBrass");
                }
            }
        }

        private static void AddRivetLine(GameObject root, Vector3 start, int count, float yStep, float xStep)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = start + new Vector3(i * xStep, i * yStep, 0f);
                Cyl(root, "rivet_line_" + i, position, new Vector3(90f, 0f, 0f), new Vector3(0.06f, 0.025f, 0.06f), "SCLVL_AgedBrass");
            }
        }

        private static void BuildGaugeCluster(GameObject root, Vector3 position, Quaternion rotation)
        {
            GameObject cluster = new GameObject("gauge_cluster");
            cluster.transform.SetParent(root.transform, false);
            cluster.transform.localPosition = position;
            cluster.transform.localRotation = rotation;
            Cyl(cluster, "gauge_face_ivory", Vector3.zero, Vector3.zero, new Vector3(0.34f, 0.045f, 0.34f), "SCLVL_GaugeIvory", true);
            Cyl(cluster, "gauge_brass_bezel", new Vector3(0f, 0.012f, 0f), Vector3.zero, new Vector3(0.43f, 0.035f, 0.43f), "SCLVL_AgedBrass", true);
            Box(cluster, "gauge_needle", new Vector3(0.08f, 0.04f, 0f), new Vector3(0.22f, 0.016f, 0.025f), "SCLVL_HeatRedEnamel");
        }

        private static void BuildCeilingPipeClusterAt(GameObject root, Vector3 origin)
        {
            Cyl(root, "ceiling_copper_pipe_main", origin + new Vector3(-0.72f, 2.92f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.16f, 3.8f, 0.16f), "SCLVL_CopperSteamPipe", true);
            Cyl(root, "ceiling_blackened_pipe_aux", origin + new Vector3(0.28f, 2.96f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.13f, 3.7f, 0.13f), "SCLVL_BlackenedIron");
            Cyl(root, "ceiling_brass_signal_pipe", origin + new Vector3(0.92f, 2.86f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.09f, 3.4f, 0.09f), "SCLVL_AgedBrass");
            for (int i = 0; i < 3; i++)
            {
                float z = -1.3f + i * 1.3f;
                Box(root, "ceiling_pipe_clamp_" + i, origin + new Vector3(0.16f, 2.92f, z), new Vector3(2.15f, 0.1f, 0.14f), "SCLVL_AgedBrass");
            }
        }

        private static void WriteGeneratedManifest()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"SCLVL\",");
            builder.AppendLine("  \"display_name\": \"Steamworks Level Kit\",");
            builder.AppendLine("  \"version\": \"" + Version + "\",");
            builder.AppendLine("  \"build_id\": \"" + BuildId + "\",");
            builder.AppendLine("  \"unity_version\": \"" + Application.unityVersion + "\",");
            builder.AppendLine("  \"generated_at_utc\": \"" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture) + "\",");
            builder.AppendLine("  \"prefabs\": [");
            for (int i = 0; i < GeneratedPrefabs.Count; i++)
            {
                GeneratedPrefabRecord record = GeneratedPrefabs[i];
                builder.AppendLine("    {");
                builder.AppendLine("      \"path\": \"" + record.Path + "\",");
                builder.AppendLine("      \"family\": \"" + record.Family + "\",");
                builder.AppendLine("      \"display_name\": \"" + record.DisplayName + "\",");
                builder.AppendLine("      \"role\": \"" + record.Role + "\",");
                builder.AppendLine("      \"dimensions_meters\": \"" + FormatVector(record.Dimensions) + "\"");
                builder.Append("    }");
                if (i < GeneratedPrefabs.Count - 1)
                {
                    builder.Append(",");
                }
                builder.AppendLine();
            }
            builder.AppendLine("  ],");
            builder.AppendLine("  \"material_count\": " + Materials.Count + ",");
            builder.AppendLine("  \"mesh_count\": " + Meshes.Count + ",");
            builder.AppendLine("  \"required_primary_changes\": [],");
            builder.AppendLine("  \"import_status\": \"generated_in_current_project_only\"");
            builder.AppendLine("}");

            File.WriteAllText(ToSystemPath(CombineAssetPath(PackageRoot, "Runtime/Metadata/SCLVL_SteamworksLevelKit_GeneratedManifest.json")), builder.ToString());
        }

        private readonly struct GeneratedPrefabRecord
        {
            public readonly string Path;
            public readonly string Family;
            public readonly string DisplayName;
            public readonly Vector3 Dimensions;
            public readonly string Role;

            public GeneratedPrefabRecord(string path, string family, string displayName, Vector3 dimensions, string role)
            {
                Path = path;
                Family = family;
                DisplayName = displayName;
                Dimensions = dimensions;
                Role = role;
            }
        }
    }
}
#endif
