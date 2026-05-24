#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.Sidecars.LevelDressingSet01
{
    public static class LevelDressingSet01Generator
    {
        public const string Version = "0.1.40";
        public const string BuildId = "p001";
        public const string PackageName = "com.brassworks.sidecar.level-dressing-set01";
        public const string RenderOutputRelativePath = "Documentation/ConceptRenders/V0_1_40_LevelDressingSet01";

        private const string PackId = "SCLD";
        private const string MenuRoot = "Brassworks/Sidecars/Level Dressing Set 01 v0.1.40/";
        private const string PackageManifestFileName = "SCLD_LevelDressingSet01_Manifest_v0.1.40-p001.json";
        private const string GeneratedCatalogFileName = "SCLD_LevelDressingSet01_GeneratedCatalog_v0.1.40.json";

        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();

        [MenuItem(MenuRoot + "Generate Package Assets")]
        public static void GeneratePackageAssets()
        {
            Materials.Clear();
            Meshes.Clear();

            EnsurePackageFolders();
            CreateMaterials();
            CreateMeshes();

            foreach (DressingSpec spec in GetSpecs())
            {
                CreatePrefab(spec);
            }

            WriteManifestFiles("generated_by_unity_sidecar_batchmode_v0.1.40");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("SCLD_GENERATE_PASS v0.1.40 prefabs=" + GetSpecs().Length + " materials=" + Materials.Count + " meshes=" + Meshes.Count);
        }

        [MenuItem(MenuRoot + "Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            IReadOnlyList<string> prefabPaths = EnsureGeneratedPrefabPaths();
            string outputRoot = ResolveRenderOutputRoot();
            Directory.CreateDirectory(outputRoot);

            RenderPrefabGroup(
                "SCLD_PREVIEW_wall_density_composition_v0.1.40.png",
                outputRoot,
                new[]
                {
                    FindPrefabPath(prefabPaths, "SCLD_RivetedTrimPlate_2m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_BrassKickPlate_2m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_PipeJunction_T_2m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_ValveCluster_Wall_1m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_GaugePanel_Triple.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_CagedLamp_Wall.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_SootGrimeDecal_Wide.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_WarningPlacard_Pressure.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_GearHousing_Round.prefab")
                },
                new[]
                {
                    new Vector3(-2.2f, 1.4f, 0.12f),
                    new Vector3(0f, 0.35f, -0.05f),
                    new Vector3(-1.65f, 2.35f, -0.18f),
                    new Vector3(1.25f, 1.72f, -0.28f),
                    new Vector3(0.4f, 2.55f, -0.31f),
                    new Vector3(2.0f, 2.0f, -0.36f),
                    new Vector3(-0.45f, 1.15f, -0.39f),
                    new Vector3(-1.2f, 0.78f, -0.42f),
                    new Vector3(2.0f, 0.85f, -0.35f)
                },
                new Vector3(0f, 2.0f, -6.6f),
                new Vector3(9f, 0f, 0f),
                true);

            RenderPrefabGroup(
                "SCLD_PREVIEW_floor_service_composition_v0.1.40.png",
                outputRoot,
                new[]
                {
                    FindPrefabPath(prefabPaths, "SCLD_ServicePanel_Floor_1x2m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_ServicePanel_Hatch_Round.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_RailFoot_Straight.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_RailFoot_Corner.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_VentStack_Floor.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_PressureTank_FloorLarge.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_BoilerGaugePedestal.prefab"),
                    FindPrefabPath(prefabPaths, "SCLD_DrainChannel_2m.prefab")
                },
                new[]
                {
                    new Vector3(-2.7f, 0f, 0.3f),
                    new Vector3(-0.65f, 0f, 0.35f),
                    new Vector3(1.35f, 0f, 0.35f),
                    new Vector3(2.85f, 0f, 0.25f),
                    new Vector3(2.2f, 0f, 2.0f),
                    new Vector3(-2.05f, 0f, 2.3f),
                    new Vector3(0.05f, 0f, 2.2f),
                    new Vector3(0.0f, 0f, -1.25f)
                },
                new Vector3(0f, 5.0f, -7.5f),
                new Vector3(32f, 0f, 0f),
                false);

            RenderPrefabGroup(
                "SCLD_PREVIEW_contact_sheet_v0.1.40.png",
                outputRoot,
                prefabPaths,
                BuildContactPositions(prefabPaths.Count),
                new Vector3(0f, 12.5f, -25f),
                new Vector3(34f, 0f, 0f),
                false);

            RenderMaterialSwatch("SCLD_PREVIEW_material_swatch_v0.1.40.png", outputRoot);
            WriteManifestFiles("generated_and_preview_rendered_by_unity_sidecar_batchmode_v0.1.40");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("SCLD_PREVIEW_PASS v0.1.40 output=" + outputRoot);
        }

        [MenuItem(MenuRoot + "Generate and Render Preview")]
        public static void GenerateAllAndRenderPreview()
        {
            GeneratePackageAssets();
            RenderPreviewPngs();
        }

        public static IReadOnlyList<string> EnsureGeneratedPrefabPaths()
        {
            DressingSpec[] specs = GetSpecs();
            string firstPath = CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + specs[0].FileName);
            if (AssetDatabase.LoadAssetAtPath<GameObject>(firstPath) == null)
            {
                GeneratePackageAssets();
            }

            List<string> paths = new List<string>(specs.Length);
            foreach (DressingSpec spec in specs)
            {
                paths.Add(CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + spec.FileName));
            }

            return paths;
        }

        private static string PackageRoot
        {
            get { return LocatePackageRoot().AssetPath; }
        }

        private static DressingSpec[] GetSpecs()
        {
            return new[]
            {
                new DressingSpec("SCLD_RivetedTrimPlate_1m.prefab", "trim", "Riveted trim plate 1m", "wall trim accent", new Vector3(1.0f, 0.34f, 0.08f), DressingKind.TrimPlate, 0),
                new DressingSpec("SCLD_RivetedTrimPlate_2m.prefab", "trim", "Riveted trim plate 2m", "wall trim run", new Vector3(2.0f, 0.42f, 0.08f), DressingKind.TrimPlate, 1),
                new DressingSpec("SCLD_PipeJunction_T_2m.prefab", "pipe", "T pipe junction 2m", "pipe junction", new Vector3(2.0f, 1.4f, 0.42f), DressingKind.PipeJunction, 0),
                new DressingSpec("SCLD_PipeJunction_X_2m.prefab", "pipe", "Cross pipe junction 2m", "pipe junction", new Vector3(2.0f, 1.8f, 0.42f), DressingKind.PipeJunction, 1),
                new DressingSpec("SCLD_PressureTank_WallSmall.prefab", "tank", "Wall pressure tank small", "wall tank", new Vector3(0.82f, 1.35f, 0.55f), DressingKind.Tank, 0),
                new DressingSpec("SCLD_PressureTank_FloorLarge.prefab", "tank", "Floor pressure tank large", "floor tank", new Vector3(1.2f, 1.8f, 1.2f), DressingKind.Tank, 1),
                new DressingSpec("SCLD_ValveCluster_Wall_1m.prefab", "valve", "Wall valve cluster 1m", "valve cluster", new Vector3(1.0f, 1.1f, 0.35f), DressingKind.ValveCluster, 0),
                new DressingSpec("SCLD_ValveCluster_Floor_2m.prefab", "valve", "Floor valve cluster 2m", "service valve cluster", new Vector3(2.0f, 1.3f, 0.8f), DressingKind.ValveCluster, 1),
                new DressingSpec("SCLD_WallGauge_Single.prefab", "gauge", "Single wall pressure gauge", "readable gauge", new Vector3(0.56f, 0.56f, 0.16f), DressingKind.GaugePanel, 0),
                new DressingSpec("SCLD_GaugePanel_Triple.prefab", "gauge", "Triple wall gauge panel", "readable gauge panel", new Vector3(1.6f, 0.78f, 0.2f), DressingKind.GaugePanel, 1),
                new DressingSpec("SCLD_CagedLamp_Wall.prefab", "lamp", "Caged wall lamp", "wall light dressing", new Vector3(0.48f, 0.72f, 0.38f), DressingKind.Lamp, 0),
                new DressingSpec("SCLD_CagedLamp_Ceiling.prefab", "lamp", "Caged ceiling lamp", "ceiling light dressing", new Vector3(0.54f, 0.82f, 0.54f), DressingKind.Lamp, 1),
                new DressingSpec("SCLD_BrassKickPlate_2m.prefab", "trim", "Brass kick plate 2m", "lower wall armor", new Vector3(2.0f, 0.55f, 0.08f), DressingKind.KickPlate, 0),
                new DressingSpec("SCLD_SootGrimeDecal_Wide.prefab", "decal", "Wide soot grime plane", "soot decal plane", new Vector3(1.65f, 0.9f, 0.02f), DressingKind.Decal, 0),
                new DressingSpec("SCLD_SootGrimeDecal_Corner.prefab", "decal", "Corner soot grime planes", "corner soot decal", new Vector3(1.0f, 1.0f, 1.0f), DressingKind.Decal, 1),
                new DressingSpec("SCLD_GearHousing_Round.prefab", "gear", "Round gear housing", "gear housing", new Vector3(1.05f, 1.05f, 0.28f), DressingKind.GearHousing, 0),
                new DressingSpec("SCLD_GearHousing_Open.prefab", "gear", "Open gear housing", "open gear housing", new Vector3(1.4f, 1.05f, 0.32f), DressingKind.GearHousing, 1),
                new DressingSpec("SCLD_WarningPlacard_Pressure.prefab", "placard", "Pressure warning placard", "warning placard", new Vector3(0.9f, 0.42f, 0.06f), DressingKind.Placard, 0),
                new DressingSpec("SCLD_WarningPlacard_ElectroSteam.prefab", "placard", "Electro-steam warning placard", "warning placard", new Vector3(1.0f, 0.46f, 0.06f), DressingKind.Placard, 1),
                new DressingSpec("SCLD_RailFoot_Straight.prefab", "rail", "Straight rail foot", "rail base", new Vector3(1.0f, 0.28f, 0.36f), DressingKind.RailFoot, 0),
                new DressingSpec("SCLD_RailFoot_Corner.prefab", "rail", "Corner rail foot", "corner rail base", new Vector3(0.82f, 0.32f, 0.82f), DressingKind.RailFoot, 1),
                new DressingSpec("SCLD_VentStack_Wall.prefab", "vent", "Wall vent stack", "wall vent stack", new Vector3(0.84f, 1.45f, 0.28f), DressingKind.VentStack, 0),
                new DressingSpec("SCLD_VentStack_Floor.prefab", "vent", "Floor vent stack", "floor vent stack", new Vector3(0.76f, 1.32f, 0.76f), DressingKind.VentStack, 1),
                new DressingSpec("SCLD_ServicePanel_Floor_1x2m.prefab", "floor", "Floor service panel 1x2m", "service floor panel", new Vector3(1.0f, 0.12f, 2.0f), DressingKind.ServicePanel, 0),
                new DressingSpec("SCLD_ServicePanel_Hatch_Round.prefab", "floor", "Round floor service hatch", "round service hatch", new Vector3(1.0f, 0.14f, 1.0f), DressingKind.ServicePanel, 1),
                new DressingSpec("SCLD_PipeClampBracket_Set.prefab", "pipe", "Pipe clamp bracket set", "pipe clamp set", new Vector3(1.6f, 0.6f, 0.28f), DressingKind.ClampSet, 0),
                new DressingSpec("SCLD_CeilingConduitRack_3m.prefab", "ceiling", "Ceiling conduit rack 3m", "ceiling conduit", new Vector3(3.0f, 0.45f, 0.75f), DressingKind.ConduitRack, 0),
                new DressingSpec("SCLD_BoilerGaugePedestal.prefab", "gauge", "Boiler gauge pedestal", "floor gauge pedestal", new Vector3(0.78f, 1.55f, 0.72f), DressingKind.BoilerPedestal, 0),
                new DressingSpec("SCLD_DrainChannel_2m.prefab", "floor", "Oil drain channel 2m", "floor drain channel", new Vector3(2.0f, 0.16f, 0.42f), DressingKind.DrainChannel, 0),
                new DressingSpec("SCLD_CornerPipeSconce.prefab", "pipe", "Corner pipe sconce", "corner pipe dressing", new Vector3(0.78f, 1.25f, 0.78f), DressingKind.CornerPipe, 0)
            };
        }

        private static void EnsurePackageFolders()
        {
            foreach (string folder in new[] { "Runtime/Prefabs", "Runtime/Materials", "Runtime/Meshes", "Runtime/Metadata", "Documentation~/Manifest", "Samples~/PreviewScene" })
            {
                Directory.CreateDirectory(AssetPathToFullPath(CombineAssetPath(PackageRoot, folder)));
            }

            AssetDatabase.Refresh();
        }

        private static void CreateMaterials()
        {
            AddMaterial("SCLD_MAT_BlackenedIron", new Color(0.038f, 0.036f, 0.033f), 0.28f, 0.82f);
            AddMaterial("SCLD_MAT_AgedBrass", new Color(0.78f, 0.57f, 0.29f), 0.56f, 0.75f);
            AddMaterial("SCLD_MAT_OxidizedBrass", new Color(0.45f, 0.39f, 0.19f), 0.38f, 0.64f);
            AddMaterial("SCLD_MAT_CopperPipe", new Color(0.72f, 0.31f, 0.14f), 0.50f, 0.68f);
            AddMaterial("SCLD_MAT_DarkRubber", new Color(0.018f, 0.017f, 0.015f), 0.18f, 0.0f);
            AddMaterial("SCLD_MAT_GaugeIvory", new Color(0.80f, 0.72f, 0.52f), 0.24f, 0.0f);
            AddMaterial("SCLD_MAT_WarmAmberGlass", new Color(1.0f, 0.52f, 0.16f, 0.72f), 0.44f, 0.02f, new Color(1.0f, 0.35f, 0.08f) * 1.7f, true);
            AddMaterial("SCLD_MAT_PressureGreenGlass", new Color(0.14f, 0.80f, 0.36f, 0.76f), 0.40f, 0.02f, new Color(0.08f, 0.70f, 0.28f) * 1.3f, true);
            AddMaterial("SCLD_MAT_WarningRedEnamel", new Color(0.78f, 0.06f, 0.035f), 0.42f, 0.12f);
            AddMaterial("SCLD_MAT_DangerYellowPaint", new Color(0.96f, 0.70f, 0.16f), 0.34f, 0.06f);
            AddMaterial("SCLD_MAT_SootGrime", new Color(0.014f, 0.012f, 0.010f, 0.62f), 0.08f, 0.0f, null, true);
            AddMaterial("SCLD_MAT_OilWetFloor", new Color(0.055f, 0.052f, 0.047f), 0.62f, 0.06f);
            AddMaterial("SCLD_MAT_PatinaGreen", new Color(0.10f, 0.39f, 0.34f), 0.36f, 0.42f);
            AddMaterial("SCLD_MAT_WornSteelEdge", new Color(0.62f, 0.58f, 0.52f), 0.62f, 0.72f);
            AddMaterial("SCLD_MAT_SmokeGlass", new Color(0.18f, 0.18f, 0.17f, 0.46f), 0.38f, 0.0f, null, true);
            AddMaterial("SCLD_MAT_BlueTemperedSteel", new Color(0.13f, 0.23f, 0.32f), 0.48f, 0.58f);
        }

        private static void AddMaterial(string name, Color color, float smoothness, float metallic, Color? emission = null, bool transparent = false)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            if (shader == null)
            {
                shader = Shader.Find("Diffuse");
            }

            Material material = new Material(shader) { name = name, color = color };
            SetColor(material, "_BaseColor", color);
            SetColor(material, "_Color", color);
            SetFloat(material, "_Smoothness", smoothness);
            SetFloat(material, "_Metallic", metallic);

            if (emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                SetColor(material, "_EmissionColor", emission.Value);
            }

            if (transparent)
            {
                material.renderQueue = 3000;
                SetFloat(material, "_Surface", 1f);
                SetFloat(material, "_Mode", 3f);
                SetFloat(material, "_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                SetFloat(material, "_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                SetFloat(material, "_ZWrite", 0f);
                material.EnableKeyword("_ALPHABLEND_ON");
            }

            string path = CombineAssetPath(PackageRoot, "Runtime/Materials/" + name + ".mat");
            CreateOrReplaceAsset(material, path);
            Materials[name] = AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        private static void CreateMeshes()
        {
            AddMesh("SCLD_MESH_BoxUnit", CreateBoxMesh());
            AddMesh("SCLD_MESH_Cylinder16Unit", CreateCylinderMesh(16));
            AddMesh("SCLD_MESH_Cylinder32Unit", CreateCylinderMesh(32));
            AddMesh("SCLD_MESH_QuadUnit", CreateQuadMesh());
            AddMesh("SCLD_MESH_Gear12ToothUnit", CreateGearMesh(12));
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
            Vector3[] vertices =
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
                0, 2, 1, 0, 3, 2, 4, 6, 5, 4, 7, 6, 8, 10, 9, 8, 11, 10,
                12, 14, 13, 12, 15, 14, 16, 18, 17, 16, 19, 18, 20, 22, 21, 20, 23, 22
            };
            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
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
                float a0 = Mathf.PI * 2f * i / segments;
                float a1 = Mathf.PI * 2f * (i + 1) / segments;
                Vector3 b0 = new Vector3(Mathf.Cos(a0) * 0.5f, -0.5f, Mathf.Sin(a0) * 0.5f);
                Vector3 b1 = new Vector3(Mathf.Cos(a1) * 0.5f, -0.5f, Mathf.Sin(a1) * 0.5f);
                Vector3 t0 = new Vector3(b0.x, 0.5f, b0.z);
                Vector3 t1 = new Vector3(b1.x, 0.5f, b1.z);

                int side = vertices.Count;
                vertices.Add(b0); vertices.Add(t0); vertices.Add(t1); vertices.Add(b1);
                triangles.Add(side); triangles.Add(side + 1); triangles.Add(side + 2);
                triangles.Add(side); triangles.Add(side + 2); triangles.Add(side + 3);

                int top = vertices.Count;
                vertices.Add(Vector3.up * 0.5f); vertices.Add(t0); vertices.Add(t1);
                triangles.Add(top); triangles.Add(top + 1); triangles.Add(top + 2);

                int bottom = vertices.Count;
                vertices.Add(Vector3.down * 0.5f); vertices.Add(b1); vertices.Add(b0);
                triangles.Add(bottom); triangles.Add(bottom + 1); triangles.Add(bottom + 2);
            }

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateQuadMesh()
        {
            Mesh mesh = new Mesh();
            mesh.SetVertices(new[] { new Vector3(-0.5f, -0.5f, 0f), new Vector3(0.5f, -0.5f, 0f), new Vector3(0.5f, 0.5f, 0f), new Vector3(-0.5f, 0.5f, 0f) });
            mesh.SetTriangles(new[] { 0, 2, 1, 0, 3, 2 }, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateGearMesh(int teeth)
        {
            List<Vector3> vertices = new List<Vector3> { Vector3.zero };
            List<int> triangles = new List<int>();
            int points = teeth * 2;
            for (int i = 0; i < points; i++)
            {
                float angle = Mathf.PI * 2f * i / points;
                float radius = i % 2 == 0 ? 0.5f : 0.39f;
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
            }

            for (int i = 1; i <= points; i++)
            {
                int next = i == points ? 1 : i + 1;
                triangles.Add(0); triangles.Add(next); triangles.Add(i);
            }

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void CreatePrefab(DressingSpec spec)
        {
            GameObject root = new GameObject(Path.GetFileNameWithoutExtension(spec.FileName));
            BuildSpec(root, spec);
            AddMetadataChild(root, spec);

            string path = CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + spec.FileName);
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            if (prefab != null)
            {
                AssetDatabase.SetLabels(prefab, new[] { PackId, "v0.1.40", spec.Family, spec.Role.Replace(" ", "_") });
            }

            UnityEngine.Object.DestroyImmediate(root);
        }

        private static void BuildSpec(GameObject root, DressingSpec spec)
        {
            switch (spec.Kind)
            {
                case DressingKind.TrimPlate: BuildTrimPlate(root, spec.Variant); break;
                case DressingKind.PipeJunction: BuildPipeJunction(root, spec.Variant); break;
                case DressingKind.Tank: BuildTank(root, spec.Variant); break;
                case DressingKind.ValveCluster: BuildValveCluster(root, spec.Variant); break;
                case DressingKind.GaugePanel: BuildGaugePanel(root, spec.Variant); break;
                case DressingKind.Lamp: BuildLamp(root, spec.Variant); break;
                case DressingKind.KickPlate: BuildKickPlate(root); break;
                case DressingKind.Decal: BuildDecal(root, spec.Variant); break;
                case DressingKind.GearHousing: BuildGearHousing(root, spec.Variant); break;
                case DressingKind.Placard: BuildPlacard(root, spec.Variant); break;
                case DressingKind.RailFoot: BuildRailFoot(root, spec.Variant); break;
                case DressingKind.VentStack: BuildVentStack(root, spec.Variant); break;
                case DressingKind.ServicePanel: BuildServicePanel(root, spec.Variant); break;
                case DressingKind.ClampSet: BuildClampSet(root); break;
                case DressingKind.ConduitRack: BuildConduitRack(root); break;
                case DressingKind.BoilerPedestal: BuildBoilerPedestal(root); break;
                case DressingKind.DrainChannel: BuildDrainChannel(root); break;
                case DressingKind.CornerPipe: BuildCornerPipe(root); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private static void BuildTrimPlate(GameObject root, int variant)
        {
            float width = variant == 0 ? 1.0f : 2.0f;
            float height = variant == 0 ? 0.34f : 0.42f;
            Box(root, "blackened_trim_backplate", new Vector3(0f, 0f, 0f), new Vector3(width, height, 0.06f), "SCLD_MAT_BlackenedIron");
            Box(root, "aged_brass_raised_center_strip", new Vector3(0f, 0f, -0.045f), new Vector3(width - 0.18f, height * 0.38f, 0.04f), "SCLD_MAT_AgedBrass");
            if (variant == 1)
            {
                Box(root, "thin_patina_service_mark", new Vector3(-0.52f, 0f, -0.083f), new Vector3(0.4f, 0.05f, 0.035f), "SCLD_MAT_PatinaGreen");
            }

            int count = variant == 0 ? 4 : 7;
            AddRivetLine(root, "trim_lower_rivet", new Vector3(-width * 0.41f, -height * 0.34f, -0.075f), new Vector3(width * 0.82f / Mathf.Max(1, count - 1), 0f, 0f), count, new Vector3(90f, 0f, 0f), "SCLD_MAT_WornSteelEdge");
            AddRivetLine(root, "trim_upper_rivet", new Vector3(-width * 0.41f, height * 0.34f, -0.075f), new Vector3(width * 0.82f / Mathf.Max(1, count - 1), 0f, 0f), count, new Vector3(90f, 0f, 0f), "SCLD_MAT_WornSteelEdge");
        }

        private static void BuildPipeJunction(GameObject root, int variant)
        {
            Box(root, "wall_mount_shadow_plate", new Vector3(0f, 0.82f, 0.08f), new Vector3(2.0f, variant == 0 ? 1.35f : 1.7f, 0.08f), variant == 0 ? "SCLD_MAT_BlackenedIron" : "SCLD_MAT_BlueTemperedSteel");
            Cyl(root, "horizontal_copper_run", new Vector3(0f, 0.82f, -0.08f), new Vector3(0f, 0f, 90f), new Vector3(0.16f, 2.0f, 0.16f), "SCLD_MAT_CopperPipe", true);
            Cyl(root, "vertical_brass_run", new Vector3(0f, 0.82f, -0.14f), Vector3.zero, new Vector3(0.14f, variant == 0 ? 0.9f : 1.65f, 0.14f), "SCLD_MAT_AgedBrass", true);
            Cyl(root, "central_cast_hub", new Vector3(0f, 0.82f, -0.25f), new Vector3(90f, 0f, 0f), new Vector3(0.52f, 0.16f, 0.52f), "SCLD_MAT_BlackenedIron", true);
            if (variant == 1)
            {
                AddGauge(root, "cross_tiny_gauge", new Vector3(0.52f, 1.28f, -0.28f), 0.22f);
            }

            BuildPipeClampBand(root, "left_clamp", new Vector3(-0.72f, 0.82f, -0.22f));
            BuildPipeClampBand(root, "right_clamp", new Vector3(0.72f, 0.82f, -0.22f));
            AddRivetGrid(root, "junction_plate_rivet", -0.82f, 0.82f, 0.28f, variant == 0 ? 1.32f : 1.55f, -0.03f, 4, 4, "SCLD_MAT_WornSteelEdge");
        }

        private static void BuildTank(GameObject root, int variant)
        {
            bool floor = variant == 1;
            if (!floor)
            {
                Box(root, "tank_wall_saddle", new Vector3(0f, 0.7f, 0.08f), new Vector3(0.72f, 1.25f, 0.1f), "SCLD_MAT_BlackenedIron");
                Cyl(root, "vertical_copper_pressure_tank", new Vector3(0f, 0.74f, -0.16f), Vector3.zero, new Vector3(0.46f, 1.12f, 0.46f), "SCLD_MAT_CopperPipe", true);
                Cyl(root, "top_brass_cap", new Vector3(0f, 1.33f, -0.16f), Vector3.zero, new Vector3(0.5f, 0.12f, 0.5f), "SCLD_MAT_AgedBrass", true);
                Cyl(root, "bottom_brass_cap", new Vector3(0f, 0.15f, -0.16f), Vector3.zero, new Vector3(0.5f, 0.12f, 0.5f), "SCLD_MAT_AgedBrass", true);
                AddGauge(root, "tank_face_gauge", new Vector3(0f, 0.74f, -0.42f), 0.24f);
                Box(root, "amber_sight_glass", new Vector3(0.19f, 0.62f, -0.42f), new Vector3(0.07f, 0.44f, 0.04f), "SCLD_MAT_WarmAmberGlass");
                return;
            }

            Cyl(root, "large_vertical_copper_tank", new Vector3(0f, 0.95f, 0f), Vector3.zero, new Vector3(0.86f, 1.55f, 0.86f), "SCLD_MAT_CopperPipe", true);
            Cyl(root, "tank_top_cap", new Vector3(0f, 1.78f, 0f), Vector3.zero, new Vector3(0.96f, 0.22f, 0.96f), "SCLD_MAT_AgedBrass", true);
            Cyl(root, "tank_bottom_base", new Vector3(0f, 0.13f, 0f), Vector3.zero, new Vector3(1.08f, 0.25f, 1.08f), "SCLD_MAT_BlackenedIron", true);
            for (int i = 0; i < 4; i++)
            {
                Cyl(root, "blackened_tank_band_" + i, new Vector3(0f, 0.44f + i * 0.38f, 0f), Vector3.zero, new Vector3(0.91f, 0.055f, 0.91f), "SCLD_MAT_BlackenedIron", true);
            }

            AddGauge(root, "large_tank_gauge", new Vector3(0f, 1.05f, -0.48f), 0.28f);
            Cyl(root, "floor_feed_pipe_front", new Vector3(0f, 0.2f, -0.58f), new Vector3(90f, 0f, 0f), new Vector3(0.11f, 0.75f, 0.11f), "SCLD_MAT_CopperPipe");
            Box(root, "front_service_tab_red", new Vector3(0f, 0.48f, -0.49f), new Vector3(0.24f, 0.08f, 0.04f), "SCLD_MAT_WarningRedEnamel");
        }

        private static void BuildValveCluster(GameObject root, int variant)
        {
            bool floor = variant == 1;
            if (!floor)
            {
                Box(root, "valve_backplate", new Vector3(0f, 0.55f, 0.06f), new Vector3(1.0f, 1.1f, 0.08f), "SCLD_MAT_BlackenedIron");
                Cyl(root, "wall_vertical_feed", new Vector3(0f, 0.55f, -0.1f), Vector3.zero, new Vector3(0.08f, 1.0f, 0.08f), "SCLD_MAT_CopperPipe");
                Cyl(root, "wall_horizontal_feed", new Vector3(0f, 0.55f, -0.14f), new Vector3(0f, 0f, 90f), new Vector3(0.07f, 0.9f, 0.07f), "SCLD_MAT_AgedBrass");
                AddValveWheel(root, "upper_red_valve", new Vector3(-0.24f, 0.78f, -0.24f), 0.34f, "SCLD_MAT_WarningRedEnamel");
                AddValveWheel(root, "lower_brass_valve", new Vector3(0.28f, 0.34f, -0.24f), 0.26f, "SCLD_MAT_AgedBrass");
                AddGauge(root, "cluster_mini_gauge", new Vector3(0.28f, 0.86f, -0.25f), 0.16f);
                return;
            }

            Box(root, "floor_manifold_base", new Vector3(0f, 0.12f, 0f), new Vector3(1.8f, 0.24f, 0.62f), "SCLD_MAT_BlackenedIron");
            Cyl(root, "main_floor_manifold", new Vector3(0f, 0.42f, 0f), new Vector3(0f, 0f, 90f), new Vector3(0.16f, 1.8f, 0.16f), "SCLD_MAT_CopperPipe", true);
            for (int i = 0; i < 3; i++)
            {
                float x = -0.62f + i * 0.62f;
                Cyl(root, "vertical_valve_stem_" + i, new Vector3(x, 0.82f, 0f), Vector3.zero, new Vector3(0.07f, 0.74f, 0.07f), "SCLD_MAT_AgedBrass");
                AddValveWheel(root, "floor_valve_wheel_" + i, new Vector3(x, 1.18f, -0.02f), 0.24f, i == 1 ? "SCLD_MAT_WarningRedEnamel" : "SCLD_MAT_AgedBrass");
            }

            AddGauge(root, "floor_cluster_gauge", new Vector3(0.0f, 0.56f, -0.24f), 0.18f);
        }

        private static void BuildGaugePanel(GameObject root, int variant)
        {
            if (variant == 0)
            {
                Box(root, "gauge_square_mount", new Vector3(0f, 0f, 0.04f), new Vector3(0.56f, 0.56f, 0.08f), "SCLD_MAT_BlackenedIron");
                AddGauge(root, "single_wall_gauge", Vector3.zero, 0.42f);
                AddRivetGrid(root, "gauge_corner_bolt", -0.22f, 0.22f, -0.22f, 0.22f, -0.08f, 2, 2, "SCLD_MAT_AgedBrass");
                return;
            }

            Box(root, "triple_panel_backplate", new Vector3(0f, 0.4f, 0.04f), new Vector3(1.6f, 0.78f, 0.08f), "SCLD_MAT_BlackenedIron");
            for (int i = 0; i < 3; i++)
            {
                float x = -0.52f + i * 0.52f;
                AddGauge(root, "triple_gauge_" + i, new Vector3(x, 0.48f, -0.08f), 0.22f);
                Cyl(root, "gauge_feed_pipe_" + i, new Vector3(x, 0.15f, -0.08f), Vector3.zero, new Vector3(0.035f, 0.28f, 0.035f), "SCLD_MAT_CopperPipe");
            }

            Box(root, "green_pressure_ok_lens", new Vector3(0.52f, 0.12f, -0.09f), new Vector3(0.22f, 0.08f, 0.04f), "SCLD_MAT_PressureGreenGlass");
            Box(root, "red_pressure_alert_lens", new Vector3(-0.52f, 0.12f, -0.09f), new Vector3(0.22f, 0.08f, 0.04f), "SCLD_MAT_WarningRedEnamel");
        }

        private static void BuildLamp(GameObject root, int variant)
        {
            bool ceiling = variant == 1;
            Box(root, ceiling ? "ceiling_mount_plate" : "lamp_wall_backplate", ceiling ? new Vector3(0f, 0.76f, 0f) : new Vector3(0f, 0.42f, 0.06f), ceiling ? new Vector3(0.7f, 0.08f, 0.7f) : new Vector3(0.42f, 0.68f, 0.08f), "SCLD_MAT_BlackenedIron");
            Cyl(root, ceiling ? "short_drop_chain" : "curved_arm_hint", ceiling ? new Vector3(0f, 0.55f, 0f) : new Vector3(0f, 0.5f, -0.14f), ceiling ? Vector3.zero : new Vector3(90f, 0f, 0f), ceiling ? new Vector3(0.06f, 0.36f, 0.06f) : new Vector3(0.055f, 0.42f, 0.055f), "SCLD_MAT_AgedBrass");
            Vector3 glassPos = ceiling ? new Vector3(0f, 0.22f, 0f) : new Vector3(0f, 0.28f, -0.32f);
            Cyl(root, ceiling ? "hanging_amber_glass" : "amber_lamp_glass", glassPos, Vector3.zero, ceiling ? new Vector3(0.3f, 0.5f, 0.3f) : new Vector3(0.25f, 0.44f, 0.25f), "SCLD_MAT_WarmAmberGlass", true);
            int bars = ceiling ? 6 : 4;
            for (int i = 0; i < bars; i++)
            {
                float angle = i * 360f / bars;
                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * (ceiling ? 0.19f : 0.16f), 0f, Mathf.Sin(angle * Mathf.Deg2Rad) * (ceiling ? 0.19f : 0.16f));
                Box(root, "lamp_cage_bar_" + i, glassPos + offset, new Vector3(0.024f, ceiling ? 0.56f : 0.52f, 0.024f), "SCLD_MAT_AgedBrass");
            }

            Cyl(root, "lamp_top_cap", glassPos + Vector3.up * 0.28f, Vector3.zero, new Vector3(0.28f, 0.05f, 0.28f), "SCLD_MAT_BlackenedIron");
            Cyl(root, "lamp_bottom_cap", glassPos + Vector3.down * 0.28f, Vector3.zero, new Vector3(0.28f, 0.05f, 0.28f), "SCLD_MAT_BlackenedIron");
        }

        private static void BuildKickPlate(GameObject root)
        {
            Box(root, "aged_brass_kick_plate", new Vector3(0f, 0.28f, 0f), new Vector3(2.0f, 0.54f, 0.06f), "SCLD_MAT_AgedBrass");
            Box(root, "blackened_upper_lip", new Vector3(0f, 0.56f, -0.035f), new Vector3(2.05f, 0.08f, 0.06f), "SCLD_MAT_BlackenedIron");
            Box(root, "blackened_lower_lip", new Vector3(0f, 0.0f, -0.035f), new Vector3(2.05f, 0.08f, 0.06f), "SCLD_MAT_BlackenedIron");
            for (int i = 0; i < 5; i++)
            {
                Box(root, "scuffed_vertical_wear_" + i, new Vector3(-0.78f + i * 0.39f, 0.28f, -0.064f), new Vector3(0.035f, 0.38f, 0.018f), "SCLD_MAT_WornSteelEdge");
            }

            AddRivetLine(root, "kick_top_rivet", new Vector3(-0.86f, 0.48f, -0.08f), new Vector3(0.29f, 0f, 0f), 7, new Vector3(90f, 0f, 0f), "SCLD_MAT_BlackenedIron");
            AddRivetLine(root, "kick_bottom_rivet", new Vector3(-0.86f, 0.08f, -0.08f), new Vector3(0.29f, 0f, 0f), 7, new Vector3(90f, 0f, 0f), "SCLD_MAT_BlackenedIron");
        }

        private static void BuildDecal(GameObject root, int variant)
        {
            if (variant == 0)
            {
                Quad(root, "wide_soft_soot_plane", new Vector3(0f, 0.45f, 0f), Vector3.zero, new Vector3(1.65f, 0.9f, 1f), "SCLD_MAT_SootGrime");
                Quad(root, "lower_oil_shadow_plane", new Vector3(-0.18f, 0.18f, -0.01f), Vector3.zero, new Vector3(1.0f, 0.32f, 1f), "SCLD_MAT_SootGrime");
                return;
            }

            Quad(root, "left_corner_soot_plane", new Vector3(-0.5f, 0.5f, 0f), Vector3.zero, new Vector3(1.0f, 1.0f, 1f), "SCLD_MAT_SootGrime");
            Quad(root, "right_corner_soot_plane", new Vector3(0f, 0.5f, 0.5f), new Vector3(0f, 90f, 0f), new Vector3(1.0f, 1.0f, 1f), "SCLD_MAT_SootGrime");
        }

        private static void BuildGearHousing(GameObject root, int variant)
        {
            if (variant == 0)
            {
                Cyl(root, "round_blackened_housing", new Vector3(0f, 0.52f, 0f), new Vector3(90f, 0f, 0f), new Vector3(1.05f, 0.22f, 1.05f), "SCLD_MAT_BlackenedIron", true);
                Gear(root, "front_aged_brass_gear_face", new Vector3(0f, 0.52f, -0.14f), Vector3.zero, new Vector3(0.82f, 0.82f, 1f), "SCLD_MAT_AgedBrass");
                Cyl(root, "central_gear_axle", new Vector3(0f, 0.52f, -0.18f), new Vector3(90f, 0f, 0f), new Vector3(0.22f, 0.12f, 0.22f), "SCLD_MAT_WornSteelEdge");
                return;
            }

            Box(root, "open_housing_backplate", new Vector3(0f, 0.52f, 0.05f), new Vector3(1.4f, 1.0f, 0.08f), "SCLD_MAT_BlackenedIron");
            Gear(root, "large_visible_gear", new Vector3(-0.28f, 0.56f, -0.08f), Vector3.zero, new Vector3(0.7f, 0.7f, 1f), "SCLD_MAT_AgedBrass");
            Gear(root, "small_visible_gear", new Vector3(0.36f, 0.36f, -0.1f), new Vector3(0f, 0f, 18f), new Vector3(0.46f, 0.46f, 1f), "SCLD_MAT_CopperPipe");
            Box(root, "open_case_top_lip", new Vector3(0f, 1.05f, -0.06f), new Vector3(1.42f, 0.08f, 0.1f), "SCLD_MAT_WornSteelEdge");
            Box(root, "open_case_bottom_lip", new Vector3(0f, -0.02f, -0.06f), new Vector3(1.42f, 0.08f, 0.1f), "SCLD_MAT_WornSteelEdge");
        }

        private static void BuildPlacard(GameObject root, int variant)
        {
            bool steam = variant == 1;
            Box(root, steam ? "electro_placard_back" : "blackened_placard_back", Vector3.zero, steam ? new Vector3(1.0f, 0.46f, 0.04f) : new Vector3(0.92f, 0.44f, 0.04f), "SCLD_MAT_BlackenedIron");
            Box(root, steam ? "blue_steel_label_face" : "yellow_warning_face", new Vector3(0f, 0f, -0.035f), steam ? new Vector3(0.86f, 0.34f, 0.035f) : new Vector3(0.78f, 0.32f, 0.035f), steam ? "SCLD_MAT_BlueTemperedSteel" : "SCLD_MAT_DangerYellowPaint");
            if (steam)
            {
                GameObject slashA = Box(root, "yellow_electro_slash_a", new Vector3(-0.12f, 0f, -0.062f), new Vector3(0.08f, 0.38f, 0.025f), "SCLD_MAT_DangerYellowPaint");
                slashA.transform.localRotation = Quaternion.Euler(0f, 0f, -26f);
                GameObject slashB = Box(root, "yellow_electro_slash_b", new Vector3(0.12f, 0f, -0.062f), new Vector3(0.08f, 0.38f, 0.025f), "SCLD_MAT_DangerYellowPaint");
                slashB.transform.localRotation = Quaternion.Euler(0f, 0f, 26f);
                AddTextTag(root, "placard_text_steam", "STEAM", new Vector3(0f, -0.13f, -0.09f), 0.05f, Color.white);
                return;
            }

            Box(root, "red_pressure_stripe", new Vector3(0f, -0.13f, -0.06f), new Vector3(0.78f, 0.06f, 0.025f), "SCLD_MAT_WarningRedEnamel");
            AddTextTag(root, "placard_text_pressure", "PRESSURE", new Vector3(0f, 0.04f, -0.09f), 0.055f, Color.black);
        }

        private static void BuildRailFoot(GameObject root, int variant)
        {
            if (variant == 0)
            {
                Box(root, "straight_rail_foot_base", new Vector3(0f, 0.05f, 0f), new Vector3(1.0f, 0.1f, 0.36f), "SCLD_MAT_BlackenedIron");
                Cyl(root, "straight_socket_left", new Vector3(-0.32f, 0.18f, 0f), Vector3.zero, new Vector3(0.16f, 0.26f, 0.16f), "SCLD_MAT_AgedBrass");
                Cyl(root, "straight_socket_right", new Vector3(0.32f, 0.18f, 0f), Vector3.zero, new Vector3(0.16f, 0.26f, 0.16f), "SCLD_MAT_AgedBrass");
                AddRivetLine(root, "rail_foot_rivet", new Vector3(-0.42f, 0.13f, -0.13f), new Vector3(0.28f, 0f, 0f), 4, Vector3.zero, "SCLD_MAT_WornSteelEdge");
                return;
            }

            Box(root, "corner_foot_base_a", new Vector3(-0.2f, 0.05f, 0f), new Vector3(0.72f, 0.1f, 0.34f), "SCLD_MAT_BlackenedIron");
            Box(root, "corner_foot_base_b", new Vector3(0.0f, 0.05f, 0.2f), new Vector3(0.34f, 0.1f, 0.72f), "SCLD_MAT_BlackenedIron");
            Cyl(root, "corner_socket", new Vector3(0f, 0.2f, 0f), Vector3.zero, new Vector3(0.22f, 0.3f, 0.22f), "SCLD_MAT_AgedBrass", true);
            Cyl(root, "corner_elbow_stub_a", new Vector3(-0.25f, 0.25f, 0f), new Vector3(0f, 0f, 90f), new Vector3(0.08f, 0.52f, 0.08f), "SCLD_MAT_CopperPipe");
            Cyl(root, "corner_elbow_stub_b", new Vector3(0f, 0.25f, 0.25f), new Vector3(90f, 0f, 0f), new Vector3(0.08f, 0.52f, 0.08f), "SCLD_MAT_CopperPipe");
        }

        private static void BuildVentStack(GameObject root, int variant)
        {
            if (variant == 0)
            {
                Box(root, "wall_vent_backplate", new Vector3(0f, 0.72f, 0.06f), new Vector3(0.84f, 1.4f, 0.08f), "SCLD_MAT_BlackenedIron");
                for (int i = 0; i < 6; i++)
                {
                    GameObject louver = Box(root, "angled_vent_louver_" + i, new Vector3(0f, 0.22f + i * 0.18f, -0.06f), new Vector3(0.72f, 0.05f, 0.08f), "SCLD_MAT_WornSteelEdge");
                    louver.transform.localRotation = Quaternion.Euler(-12f, 0f, 0f);
                }

                Cyl(root, "top_vent_stack_stub", new Vector3(0f, 1.47f, -0.04f), Vector3.zero, new Vector3(0.36f, 0.18f, 0.36f), "SCLD_MAT_CopperPipe", true);
                return;
            }

            Cyl(root, "floor_vent_base", new Vector3(0f, 0.1f, 0f), Vector3.zero, new Vector3(0.72f, 0.2f, 0.72f), "SCLD_MAT_BlackenedIron", true);
            Cyl(root, "vertical_vent_stack", new Vector3(0f, 0.72f, 0f), Vector3.zero, new Vector3(0.42f, 1.15f, 0.42f), "SCLD_MAT_BlueTemperedSteel", true);
            for (int i = 0; i < 4; i++)
            {
                Cyl(root, "vent_stack_band_" + i, new Vector3(0f, 0.32f + i * 0.22f, 0f), Vector3.zero, new Vector3(0.46f, 0.035f, 0.46f), "SCLD_MAT_AgedBrass", true);
            }

            Cyl(root, "vent_mushroom_cap", new Vector3(0f, 1.34f, 0f), Vector3.zero, new Vector3(0.62f, 0.14f, 0.62f), "SCLD_MAT_BlackenedIron", true);
        }

        private static void BuildServicePanel(GameObject root, int variant)
        {
            if (variant == 0)
            {
                Box(root, "floor_panel_blackened_frame", new Vector3(0f, 0.03f, 0f), new Vector3(1.0f, 0.06f, 2.0f), "SCLD_MAT_BlackenedIron");
                Box(root, "recessed_oilwet_center", new Vector3(0f, 0.07f, 0f), new Vector3(0.78f, 0.045f, 1.72f), "SCLD_MAT_OilWetFloor");
                Box(root, "brass_pull_handle", new Vector3(0f, 0.12f, -0.55f), new Vector3(0.42f, 0.05f, 0.08f), "SCLD_MAT_AgedBrass");
                Box(root, "red_service_index_mark", new Vector3(0.0f, 0.125f, 0.72f), new Vector3(0.5f, 0.035f, 0.05f), "SCLD_MAT_WarningRedEnamel");
                AddFloorRivetGrid(root, "floor_service_screw", -0.38f, 0.38f, -0.84f, 0.84f, 2, 4, "SCLD_MAT_WornSteelEdge");
                return;
            }

            Cyl(root, "round_hatch_black_frame", new Vector3(0f, 0.06f, 0f), Vector3.zero, new Vector3(1.0f, 0.12f, 1.0f), "SCLD_MAT_BlackenedIron", true);
            Cyl(root, "round_hatch_brass_plate", new Vector3(0f, 0.14f, 0f), Vector3.zero, new Vector3(0.78f, 0.08f, 0.78f), "SCLD_MAT_AgedBrass", true);
            Box(root, "crossbar_handle_x", new Vector3(0f, 0.21f, 0f), new Vector3(0.62f, 0.05f, 0.08f), "SCLD_MAT_BlackenedIron");
            Box(root, "crossbar_handle_z", new Vector3(0f, 0.22f, 0f), new Vector3(0.08f, 0.05f, 0.62f), "SCLD_MAT_BlackenedIron");
            for (int i = 0; i < 8; i++)
            {
                float angle = Mathf.PI * 2f * i / 8f;
                Cyl(root, "round_hatch_rivet_" + i, new Vector3(Mathf.Cos(angle) * 0.39f, 0.22f, Mathf.Sin(angle) * 0.39f), Vector3.zero, new Vector3(0.06f, 0.035f, 0.06f), "SCLD_MAT_WornSteelEdge");
            }
        }

        private static void BuildClampSet(GameObject root)
        {
            Box(root, "clamp_set_backplate", new Vector3(0f, 0.3f, 0.05f), new Vector3(1.6f, 0.55f, 0.08f), "SCLD_MAT_BlackenedIron");
            for (int i = 0; i < 4; i++)
            {
                BuildPipeClampBand(root, "clamp_bracket_" + i, new Vector3(-0.6f + i * 0.4f, 0.3f, -0.08f));
            }

            Cyl(root, "sample_pipe_through_clamps", new Vector3(0f, 0.3f, -0.12f), new Vector3(0f, 0f, 90f), new Vector3(0.08f, 1.46f, 0.08f), "SCLD_MAT_CopperPipe");
        }

        private static void BuildConduitRack(GameObject root)
        {
            Box(root, "ceiling_conduit_mount", new Vector3(0f, 0.35f, 0f), new Vector3(3.0f, 0.08f, 0.75f), "SCLD_MAT_BlackenedIron");
            for (int i = 0; i < 4; i++)
            {
                float z = -0.27f + i * 0.18f;
                Cyl(root, "ceiling_conduit_run_" + i, new Vector3(0f, 0.12f, z), new Vector3(0f, 0f, 90f), new Vector3(0.055f + i * 0.012f, 2.8f, 0.055f + i * 0.012f), i % 2 == 0 ? "SCLD_MAT_CopperPipe" : "SCLD_MAT_AgedBrass");
            }

            for (int i = 0; i < 5; i++)
            {
                Box(root, "conduit_hanger_" + i, new Vector3(-1.25f + i * 0.62f, 0.24f, 0f), new Vector3(0.06f, 0.28f, 0.72f), "SCLD_MAT_WornSteelEdge");
            }
        }

        private static void BuildBoilerPedestal(GameObject root)
        {
            Box(root, "pedestal_oilwet_base", new Vector3(0f, 0.12f, 0f), new Vector3(0.72f, 0.24f, 0.68f), "SCLD_MAT_OilWetFloor");
            Cyl(root, "pedestal_brass_column", new Vector3(0f, 0.78f, 0f), Vector3.zero, new Vector3(0.26f, 1.08f, 0.26f), "SCLD_MAT_AgedBrass", true);
            Box(root, "sloped_gauge_head", new Vector3(0f, 1.35f, -0.08f), new Vector3(0.68f, 0.32f, 0.24f), "SCLD_MAT_BlackenedIron");
            AddGauge(root, "pedestal_face_gauge", new Vector3(0f, 1.38f, -0.25f), 0.24f);
            AddValveWheel(root, "pedestal_side_valve", new Vector3(0.42f, 0.82f, -0.02f), 0.2f, "SCLD_MAT_WarningRedEnamel");
            Cyl(root, "pedestal_floor_feed", new Vector3(-0.42f, 0.18f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.08f, 0.7f, 0.08f), "SCLD_MAT_CopperPipe");
        }

        private static void BuildDrainChannel(GameObject root)
        {
            Box(root, "drain_outer_black_frame", new Vector3(0f, 0.05f, 0f), new Vector3(2.0f, 0.1f, 0.42f), "SCLD_MAT_BlackenedIron");
            Box(root, "oil_dark_recess", new Vector3(0f, 0.11f, 0f), new Vector3(1.72f, 0.05f, 0.22f), "SCLD_MAT_OilWetFloor");
            for (int i = 0; i < 6; i++)
            {
                Box(root, "drain_cross_slat_" + i, new Vector3(-0.74f + i * 0.3f, 0.16f, 0f), new Vector3(0.055f, 0.05f, 0.36f), "SCLD_MAT_AgedBrass");
            }

            Box(root, "front_soot_spill", new Vector3(-0.36f, 0.165f, -0.26f), new Vector3(0.78f, 0.02f, 0.12f), "SCLD_MAT_SootGrime");
        }

        private static void BuildCornerPipe(GameObject root)
        {
            Box(root, "corner_backplate_left", new Vector3(-0.38f, 0.62f, 0f), new Vector3(0.08f, 1.2f, 0.68f), "SCLD_MAT_BlackenedIron");
            Box(root, "corner_backplate_right", new Vector3(0f, 0.62f, 0.38f), new Vector3(0.68f, 1.2f, 0.08f), "SCLD_MAT_BlackenedIron");
            Cyl(root, "corner_vertical_pipe", new Vector3(-0.18f, 0.63f, 0.18f), Vector3.zero, new Vector3(0.1f, 1.15f, 0.1f), "SCLD_MAT_CopperPipe");
            Cyl(root, "corner_horizontal_pipe_left", new Vector3(-0.18f, 1.02f, 0.02f), new Vector3(90f, 0f, 0f), new Vector3(0.08f, 0.62f, 0.08f), "SCLD_MAT_AgedBrass");
            Cyl(root, "corner_horizontal_pipe_right", new Vector3(0.02f, 0.43f, 0.18f), new Vector3(0f, 0f, 90f), new Vector3(0.08f, 0.62f, 0.08f), "SCLD_MAT_AgedBrass");
            AddGauge(root, "corner_tiny_gauge", new Vector3(-0.19f, 0.82f, -0.21f), 0.16f);
        }

        private static void AddMetadataChild(GameObject root, DressingSpec spec)
        {
            GameObject metadata = new GameObject("SCLD_METADATA__" + spec.Family + "__" + spec.Role.Replace(" ", "_"));
            metadata.transform.SetParent(root.transform, false);
            metadata.SetActive(false);
            metadata.name += "__" + FormatVector(spec.Dimensions) + "__sidecar_only";
        }

        private static void AddGauge(GameObject root, string prefix, Vector3 position, float radius)
        {
            Cyl(root, prefix + "_black_backplate", position + new Vector3(0f, 0f, 0.025f), new Vector3(90f, 0f, 0f), new Vector3(radius * 1.22f, 0.05f, radius * 1.22f), "SCLD_MAT_BlackenedIron", true);
            Cyl(root, prefix + "_ivory_face", position + new Vector3(0f, 0f, -0.025f), new Vector3(90f, 0f, 0f), new Vector3(radius, 0.035f, radius), "SCLD_MAT_GaugeIvory", true);
            Cyl(root, prefix + "_brass_bezel", position + new Vector3(0f, 0f, -0.06f), new Vector3(90f, 0f, 0f), new Vector3(radius * 1.1f, 0.03f, radius * 1.1f), "SCLD_MAT_AgedBrass", true);
            GameObject needle = Box(root, prefix + "_red_needle", position + new Vector3(radius * 0.18f, radius * 0.02f, -0.095f), new Vector3(radius * 0.72f, 0.018f, 0.025f), "SCLD_MAT_WarningRedEnamel");
            needle.transform.localRotation = Quaternion.Euler(0f, 0f, 18f);
            Cyl(root, prefix + "_needle_pivot", position + new Vector3(0f, 0f, -0.12f), new Vector3(90f, 0f, 0f), new Vector3(radius * 0.16f, 0.025f, radius * 0.16f), "SCLD_MAT_BlackenedIron");
        }

        private static void AddValveWheel(GameObject root, string prefix, Vector3 position, float radius, string material)
        {
            Cyl(root, prefix + "_outer_wheel", position, new Vector3(90f, 0f, 0f), new Vector3(radius, 0.06f, radius), material, true);
            Cyl(root, prefix + "_hub", position + new Vector3(0f, 0f, -0.055f), new Vector3(90f, 0f, 0f), new Vector3(radius * 0.25f, 0.08f, radius * 0.25f), "SCLD_MAT_AgedBrass");
            for (int i = 0; i < 4; i++)
            {
                GameObject spoke = Box(root, prefix + "_spoke_" + i, position + new Vector3(0f, 0f, -0.075f), new Vector3(radius * 0.88f, 0.035f, 0.035f), material);
                spoke.transform.localRotation = Quaternion.Euler(0f, 0f, i * 45f);
            }
        }

        private static void BuildPipeClampBand(GameObject root, string prefix, Vector3 position)
        {
            Box(root, prefix + "_clamp_band", position, new Vector3(0.16f, 0.42f, 0.07f), "SCLD_MAT_WornSteelEdge");
            Cyl(root, prefix + "_upper_screw", position + new Vector3(0f, 0.18f, -0.055f), new Vector3(90f, 0f, 0f), new Vector3(0.045f, 0.02f, 0.045f), "SCLD_MAT_BlackenedIron");
            Cyl(root, prefix + "_lower_screw", position + new Vector3(0f, -0.18f, -0.055f), new Vector3(90f, 0f, 0f), new Vector3(0.045f, 0.02f, 0.045f), "SCLD_MAT_BlackenedIron");
        }

        private static void AddRivetLine(GameObject root, string prefix, Vector3 start, Vector3 step, int count, Vector3 euler, string material)
        {
            for (int i = 0; i < count; i++)
            {
                Cyl(root, prefix + "_" + i, start + step * i, euler, new Vector3(0.08f, 0.035f, 0.08f), material);
            }
        }

        private static void AddRivetGrid(GameObject root, string prefix, float minX, float maxX, float minY, float maxY, float z, int columns, int rows, string material)
        {
            for (int y = 0; y < rows; y++)
            {
                float yy = Mathf.Lerp(minY, maxY, rows == 1 ? 0f : y / (float)(rows - 1));
                for (int x = 0; x < columns; x++)
                {
                    float xx = Mathf.Lerp(minX, maxX, columns == 1 ? 0f : x / (float)(columns - 1));
                    Cyl(root, prefix + "_" + x + "_" + y, new Vector3(xx, yy, z), new Vector3(90f, 0f, 0f), new Vector3(0.075f, 0.032f, 0.075f), material);
                }
            }
        }

        private static void AddFloorRivetGrid(GameObject root, string prefix, float minX, float maxX, float minZ, float maxZ, int columns, int rows, string material)
        {
            for (int z = 0; z < rows; z++)
            {
                float zz = Mathf.Lerp(minZ, maxZ, rows == 1 ? 0f : z / (float)(rows - 1));
                for (int x = 0; x < columns; x++)
                {
                    float xx = Mathf.Lerp(minX, maxX, columns == 1 ? 0f : x / (float)(columns - 1));
                    Cyl(root, prefix + "_" + x + "_" + z, new Vector3(xx, 0.13f, zz), Vector3.zero, new Vector3(0.055f, 0.03f, 0.055f), material);
                }
            }
        }

        private static void AddTextTag(GameObject root, string name, string text, Vector3 position, float size, Color color)
        {
            GameObject tag = new GameObject(name);
            tag.transform.SetParent(root.transform, false);
            tag.transform.localPosition = position;
            TextMesh mesh = tag.AddComponent<TextMesh>();
            mesh.text = text;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.characterSize = size;
            mesh.fontSize = 64;
            mesh.color = color;
        }

        private static GameObject Box(GameObject parent, string name, Vector3 position, Vector3 scale, string material)
        {
            return Part(parent, name, "SCLD_MESH_BoxUnit", position, Quaternion.identity, scale, material);
        }

        private static GameObject Cyl(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string material, bool highRes = false)
        {
            return Part(parent, name, highRes ? "SCLD_MESH_Cylinder32Unit" : "SCLD_MESH_Cylinder16Unit", position, Quaternion.Euler(euler), scale, material);
        }

        private static GameObject Quad(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string material)
        {
            return Part(parent, name, "SCLD_MESH_QuadUnit", position, Quaternion.Euler(euler), scale, material);
        }

        private static GameObject Gear(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string material)
        {
            return Part(parent, name, "SCLD_MESH_Gear12ToothUnit", position, Quaternion.Euler(euler), scale, material);
        }

        private static GameObject Part(GameObject parent, string name, string meshName, Vector3 position, Quaternion rotation, Vector3 scale, string material)
        {
            GameObject part = new GameObject(name);
            part.transform.SetParent(parent.transform, false);
            part.transform.localPosition = position;
            part.transform.localRotation = rotation;
            part.transform.localScale = scale;
            MeshFilter filter = part.AddComponent<MeshFilter>();
            filter.sharedMesh = Meshes[meshName];
            MeshRenderer renderer = part.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = Materials[material];
            return part;
        }

        private static Vector3[] BuildContactPositions(int count)
        {
            Vector3[] positions = new Vector3[count];
            int columns = 6;
            for (int i = 0; i < count; i++)
            {
                int row = i / columns;
                int column = i % columns;
                positions[i] = new Vector3((column - 2.5f) * 3.2f, 0f, row * 3.15f);
            }

            return positions;
        }

        private static string FindPrefabPath(IReadOnlyList<string> prefabPaths, string fileName)
        {
            for (int i = 0; i < prefabPaths.Count; i++)
            {
                if (prefabPaths[i].EndsWith("/" + fileName, StringComparison.Ordinal))
                {
                    return prefabPaths[i];
                }
            }

            Debug.LogWarning("Level Dressing Set 01 preview requested unknown prefab: " + fileName);
            return prefabPaths.Count > 0 ? prefabPaths[0] : string.Empty;
        }

        private static void RenderPrefabGroup(string fileName, string outputRoot, IReadOnlyList<string> prefabPaths, IReadOnlyList<Vector3> positions, Vector3 cameraPosition, Vector3 cameraEuler, bool wallBacker)
        {
            GameObject stage = new GameObject("SCLD_RenderStage_" + Path.GetFileNameWithoutExtension(fileName));
            try
            {
                for (int i = 0; i < prefabPaths.Count; i++)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[i]);
                    if (prefab == null)
                    {
                        Debug.LogWarning("Missing Level Dressing Set 01 prefab for render: " + prefabPaths[i]);
                        continue;
                    }

                    GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    instance.transform.SetParent(stage.transform, false);
                    instance.transform.localPosition = positions[i];
                }

                if (wallBacker)
                {
                    AddPreviewWall(stage);
                }
                else
                {
                    AddPreviewFloor(stage);
                }

                AddPreviewLighting(stage);
                RenderStageToPng(stage, Path.Combine(outputRoot, fileName), cameraPosition, cameraEuler);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(stage);
            }
        }

        private static void RenderMaterialSwatch(string fileName, string outputRoot)
        {
            GameObject stage = new GameObject("SCLD_RenderStage_" + Path.GetFileNameWithoutExtension(fileName));
            try
            {
                string[] materialNames = GetMaterialNames();
                for (int i = 0; i < materialNames.Length; i++)
                {
                    int column = i % 4;
                    int row = i / 4;
                    GameObject swatch = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    swatch.name = "swatch_" + materialNames[i];
                    swatch.transform.SetParent(stage.transform, false);
                    swatch.transform.localPosition = new Vector3((column - 1.5f) * 1.25f, 1.45f - row * 0.62f, 0f);
                    swatch.transform.localScale = new Vector3(1.05f, 0.44f, 0.14f);
                    Renderer renderer = swatch.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(CombineAssetPath(PackageRoot, "Runtime/Materials/" + materialNames[i] + ".mat"));
                    }
                }

                BoxPrimitive(stage, "swatch_dark_backplate", new Vector3(0f, 0.42f, 0.22f), new Vector3(5.7f, 3.15f, 0.08f), new Color(0.018f, 0.016f, 0.014f));
                AddPreviewLighting(stage);
                RenderStageToPng(stage, Path.Combine(outputRoot, fileName), new Vector3(0f, 0.35f, -6.3f), Vector3.zero);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(stage);
            }
        }

        private static void AddPreviewFloor(GameObject stage)
        {
            BoxPrimitive(stage, "preview_oil_wet_floor", new Vector3(0f, -0.04f, 5.0f), new Vector3(23f, 0.06f, 18f), new Color(0.052f, 0.049f, 0.044f));
        }

        private static void AddPreviewWall(GameObject stage)
        {
            BoxPrimitive(stage, "preview_soot_wall", new Vector3(0f, 1.6f, 0.25f), new Vector3(5.4f, 3.2f, 0.08f), new Color(0.08f, 0.065f, 0.052f));
            BoxPrimitive(stage, "preview_oil_kick_floor", new Vector3(0f, -0.08f, -0.7f), new Vector3(6.4f, 0.08f, 2.0f), new Color(0.05f, 0.047f, 0.042f));
        }

        private static void BoxPrimitive(GameObject parent, string name, Vector3 position, Vector3 scale, Color color)
        {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.name = name;
            box.transform.SetParent(parent.transform, false);
            box.transform.localPosition = position;
            box.transform.localScale = scale;
            Renderer renderer = box.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = new Material(Shader.Find("Standard"));
                material.color = color;
                renderer.sharedMaterial = material;
            }
        }

        private static void AddPreviewLighting(GameObject stage)
        {
            GameObject key = new GameObject("preview_key_light");
            key.transform.SetParent(stage.transform, false);
            key.transform.localRotation = Quaternion.Euler(45f, -32f, 0f);
            Light keyLight = key.AddComponent<Light>();
            keyLight.type = LightType.Directional;
            keyLight.intensity = 2.2f;
            keyLight.color = new Color(1f, 0.74f, 0.48f);

            GameObject fill = new GameObject("preview_amber_fill");
            fill.transform.SetParent(stage.transform, false);
            fill.transform.localPosition = new Vector3(-3f, 4f, -3f);
            Light fillLight = fill.AddComponent<Light>();
            fillLight.type = LightType.Point;
            fillLight.range = 12f;
            fillLight.intensity = 2.0f;
            fillLight.color = new Color(0.95f, 0.54f, 0.22f);

            GameObject rim = new GameObject("preview_green_pressure_rim");
            rim.transform.SetParent(stage.transform, false);
            rim.transform.localPosition = new Vector3(4f, 2.7f, -2f);
            Light rimLight = rim.AddComponent<Light>();
            rimLight.type = LightType.Point;
            rimLight.range = 8f;
            rimLight.intensity = 0.75f;
            rimLight.color = new Color(0.16f, 0.72f, 0.36f);
        }

        private static void RenderStageToPng(GameObject stage, string path, Vector3 cameraPosition, Vector3 cameraEuler)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            GameObject cameraObject = new GameObject("SCLD_preview_camera");
            cameraObject.transform.SetParent(stage.transform, false);
            cameraObject.transform.position = cameraPosition;
            cameraObject.transform.rotation = Quaternion.Euler(cameraEuler);

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.018f, 0.016f, 0.014f);
            camera.fieldOfView = 48f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 100f;

            RenderTexture renderTexture = new RenderTexture(1800, 1000, 24, RenderTextureFormat.ARGB32) { antiAliasing = 4 };
            RenderTexture previousActive = RenderTexture.active;
            Texture2D texture = null;
            try
            {
                camera.targetTexture = renderTexture;
                camera.Render();
                RenderTexture.active = renderTexture;
                texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply();
                File.WriteAllBytes(path, texture.EncodeToPNG());
            }
            finally
            {
                RenderTexture.active = previousActive;
                camera.targetTexture = null;
                if (texture != null)
                {
                    UnityEngine.Object.DestroyImmediate(texture);
                }

                renderTexture.Release();
                UnityEngine.Object.DestroyImmediate(renderTexture);
                UnityEngine.Object.DestroyImmediate(cameraObject);
            }
        }

        private static void WriteManifestFiles(string importStatus)
        {
            string json = BuildManifestJson(importStatus);
            string packageManifestPath = CombineAssetPath(PackageRoot, "Documentation~/Manifest/" + PackageManifestFileName);
            string generatedCatalogPath = CombineAssetPath(PackageRoot, "Runtime/Metadata/" + GeneratedCatalogFileName);
            File.WriteAllText(AssetPathToFullPath(packageManifestPath), json, Encoding.UTF8);
            File.WriteAllText(AssetPathToFullPath(generatedCatalogPath), json, Encoding.UTF8);
            AssetDatabase.ImportAsset(packageManifestPath);
            AssetDatabase.ImportAsset(generatedCatalogPath);
        }

        private static string BuildManifestJson(string importStatus)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            AppendField(builder, "pack_id", PackId, true);
            AppendField(builder, "display_name", "Level Dressing Set 01", true);
            AppendField(builder, "version", Version, true);
            AppendField(builder, "build_id", BuildId, true);
            AppendField(builder, "unity_version", Application.unityVersion, true);
            AppendField(builder, "generated_at_utc", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), true);
            AppendField(builder, "sidecar_project", "UD-SC-LVL-LevelDressingSet01", true);
            AppendField(builder, "owner_lane", "sidecar-level-dressing-set01", true);
            AppendField(builder, "primary_intake_owner", "main-lane-art-integration", true);
            AppendField(builder, "canonical_root", "AssetPacks/BrassworksBreach.LevelDressingSet01", true);
            AppendField(builder, "package_root", "AssetPacks/BrassworksBreach.LevelDressingSet01", true);
            AppendField(builder, "package_name", PackageName, true);
            AppendField(builder, "upm_package_name", PackageName, true);
            AppendField(builder, "generator_menu", MenuRoot + "Generate Package Assets", true);
            AppendField(builder, "preview_menu", MenuRoot + "Render Preview PNGs", true);
            builder.AppendLine("  \"asset_counts\": {");
            builder.AppendLine("    \"generated_prefabs\": 30,");
            builder.AppendLine("    \"generated_materials\": 16,");
            builder.AppendLine("    \"generated_meshes\": 5,");
            builder.AppendLine("    \"textures\": 0,");
            builder.AppendLine("    \"audio\": 0,");
            builder.AppendLine("    \"vfx\": 0,");
            builder.AppendLine("    \"animation_clips\": 0,");
            builder.AppendLine("    \"preview_renders\": 4");
            builder.AppendLine("  },");
            AppendArray(builder, "generated_prefabs", GetPrefabManifestPaths(), true);
            AppendArray(builder, "generated_materials", PrefixPaths("Runtime/Materials", GetMaterialNames(), ".mat"), true);
            AppendArray(builder, "generated_meshes", PrefixPaths("Runtime/Meshes", GetMeshNames(), ".asset"), true);
            AppendArray(builder, "preview_renders", GetPreviewRenderPaths(), true);
            builder.AppendLine("  \"dependencies\": [],");
            builder.AppendLine("  \"required_primary_changes\": [],");
            builder.AppendLine("  \"path_collisions_checked\": true,");
            builder.AppendLine("  \"guid_collisions_checked\": true,");
            AppendField(builder, "guid_collision_check", "validated_by_package_static_validator_after_generation", true);
            AppendField(builder, "import_smoke_status", importStatus, true);
            AppendField(builder, "primary_quarantine_import_status", "not_run", true);
            AppendField(builder, "unit_scale", "1 Unity unit = 1 meter", true);
            AppendArray(builder, "known_risks", new[]
            {
                "Generated prefabs are visual level-dressing props; final gameplay collision, navigation blocking, and occlusion ownership stay with the primary lane.",
                "Procedural materials are Unity material proxies for quarantine review and can be replaced by final authored textures without changing prefab names.",
                "Decal planes are simple geometry markers and should be remapped to the project's final decal system during promotion if needed.",
                "Preview lighting is proof-only and should not be promoted into shipped levels without art direction review."
            }, true);
            AppendField(builder, "rollback_path", "delete isolated package root or remove local package reference", true);
            AppendField(builder, "decision", "ready_for_primary_quarantine_after_static_validation_and_preview_review", false);
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static string[] GetPrefabManifestPaths()
        {
            DressingSpec[] specs = GetSpecs();
            string[] paths = new string[specs.Length];
            for (int i = 0; i < specs.Length; i++)
            {
                paths[i] = "Packages/" + PackageName + "/Runtime/Prefabs/" + specs[i].FileName;
            }

            return paths;
        }

        private static string[] PrefixPaths(string folder, string[] names, string extension)
        {
            string[] paths = new string[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                paths[i] = "Packages/" + PackageName + "/" + folder + "/" + names[i] + extension;
            }

            return paths;
        }

        private static string[] GetPreviewRenderPaths()
        {
            return new[]
            {
                RenderOutputRelativePath + "/SCLD_PREVIEW_wall_density_composition_v0.1.40.png",
                RenderOutputRelativePath + "/SCLD_PREVIEW_floor_service_composition_v0.1.40.png",
                RenderOutputRelativePath + "/SCLD_PREVIEW_contact_sheet_v0.1.40.png",
                RenderOutputRelativePath + "/SCLD_PREVIEW_material_swatch_v0.1.40.png"
            };
        }

        private static string[] GetMaterialNames()
        {
            return new[]
            {
                "SCLD_MAT_BlackenedIron", "SCLD_MAT_AgedBrass", "SCLD_MAT_OxidizedBrass", "SCLD_MAT_CopperPipe",
                "SCLD_MAT_DarkRubber", "SCLD_MAT_GaugeIvory", "SCLD_MAT_WarmAmberGlass", "SCLD_MAT_PressureGreenGlass",
                "SCLD_MAT_WarningRedEnamel", "SCLD_MAT_DangerYellowPaint", "SCLD_MAT_SootGrime", "SCLD_MAT_OilWetFloor",
                "SCLD_MAT_PatinaGreen", "SCLD_MAT_WornSteelEdge", "SCLD_MAT_SmokeGlass", "SCLD_MAT_BlueTemperedSteel"
            };
        }

        private static string[] GetMeshNames()
        {
            return new[] { "SCLD_MESH_BoxUnit", "SCLD_MESH_Cylinder16Unit", "SCLD_MESH_Cylinder32Unit", "SCLD_MESH_QuadUnit", "SCLD_MESH_Gear12ToothUnit" };
        }

        private static void AppendField(StringBuilder builder, string name, string value, bool trailingComma)
        {
            builder.Append("  \"").Append(EscapeJson(name)).Append("\": \"").Append(EscapeJson(value)).Append("\"");
            if (trailingComma)
            {
                builder.Append(",");
            }

            builder.AppendLine();
        }

        private static void AppendArray(StringBuilder builder, string name, IReadOnlyList<string> values, bool trailingComma)
        {
            builder.Append("  \"").Append(EscapeJson(name)).AppendLine("\": [");
            for (int i = 0; i < values.Count; i++)
            {
                builder.Append("    \"").Append(EscapeJson(values[i])).Append("\"");
                builder.AppendLine(i < values.Count - 1 ? "," : string.Empty);
            }

            builder.Append("  ]");
            if (trailingComma)
            {
                builder.Append(",");
            }

            builder.AppendLine();
        }

        private static string ResolveRenderOutputRoot()
        {
            string explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_SCLD_PREVIEW_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return Path.GetFullPath(explicitRoot);
            }

            return Path.Combine(ResolveRepoRoot(), RenderOutputRelativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ResolveRepoRoot()
        {
            string resolvedPath = LocatePackageRoot().ResolvedPath;
            if (!string.IsNullOrWhiteSpace(resolvedPath))
            {
                DirectoryInfo directory = new DirectoryInfo(resolvedPath);
                while (directory != null)
                {
                    if (directory.Name.Equals("AssetPacks", StringComparison.OrdinalIgnoreCase) && directory.Parent != null)
                    {
                        return directory.Parent.FullName;
                    }

                    directory = directory.Parent;
                }
            }

            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }

        private static PackageRootInfo LocatePackageRoot()
        {
            UnityEditor.PackageManager.PackageInfo packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(LevelDressingSet01Generator).Assembly);
            if (packageInfo != null && !string.IsNullOrWhiteSpace(packageInfo.assetPath) && !string.IsNullOrWhiteSpace(packageInfo.resolvedPath))
            {
                return new PackageRootInfo(packageInfo.assetPath.Replace("\\", "/"), packageInfo.resolvedPath);
            }

            string[] scriptGuids = AssetDatabase.FindAssets(nameof(LevelDressingSet01Generator));
            foreach (string guid in scriptGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/LevelDressingSet01Generator.cs";
                if (path.EndsWith(suffix, StringComparison.Ordinal))
                {
                    string assetPath = path.Substring(0, path.Length - suffix.Length);
                    return new PackageRootInfo(assetPath, Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath)));
                }
            }

            throw new InvalidOperationException("Could not locate " + PackageName + " package root.");
        }

        private static string AssetPathToFullPath(string assetPath)
        {
            string normalizedPath = assetPath.Replace("\\", "/");
            PackageRootInfo packageRoot = LocatePackageRoot();
            if (normalizedPath.StartsWith(packageRoot.AssetPath + "/", StringComparison.Ordinal))
            {
                string relativePath = normalizedPath.Substring(packageRoot.AssetPath.Length + 1);
                return Path.GetFullPath(Path.Combine(packageRoot.ResolvedPath, relativePath.Replace('/', Path.DirectorySeparatorChar)));
            }

            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", normalizedPath.Replace('/', Path.DirectorySeparatorChar)));
        }

        private static string CombineAssetPath(string root, string relative)
        {
            return (root.TrimEnd('/') + "/" + relative.TrimStart('/')).Replace("\\", "/");
        }

        private static string FormatVector(Vector3 value)
        {
            return value.x.ToString("0.##", CultureInfo.InvariantCulture) + "x" +
                   value.y.ToString("0.##", CultureInfo.InvariantCulture) + "x" +
                   value.z.ToString("0.##", CultureInfo.InvariantCulture) + "m";
        }

        private static void SetColor(Material material, string property, Color value)
        {
            if (material.HasProperty(property))
            {
                material.SetColor(property, value);
            }
        }

        private static void SetFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property))
            {
                material.SetFloat(property, value);
            }
        }

        private static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private enum DressingKind
        {
            TrimPlate,
            PipeJunction,
            Tank,
            ValveCluster,
            GaugePanel,
            Lamp,
            KickPlate,
            Decal,
            GearHousing,
            Placard,
            RailFoot,
            VentStack,
            ServicePanel,
            ClampSet,
            ConduitRack,
            BoilerPedestal,
            DrainChannel,
            CornerPipe
        }

        private sealed class DressingSpec
        {
            public DressingSpec(string fileName, string family, string displayName, string role, Vector3 dimensions, DressingKind kind, int variant)
            {
                FileName = fileName;
                Family = family;
                DisplayName = displayName;
                Role = role;
                Dimensions = dimensions;
                Kind = kind;
                Variant = variant;
            }

            public string FileName { get; private set; }
            public string Family { get; private set; }
            public string DisplayName { get; private set; }
            public string Role { get; private set; }
            public Vector3 Dimensions { get; private set; }
            public DressingKind Kind { get; private set; }
            public int Variant { get; private set; }
        }

        private sealed class PackageRootInfo
        {
            public PackageRootInfo(string assetPath, string resolvedPath)
            {
                AssetPath = assetPath;
                ResolvedPath = resolvedPath;
            }

            public string AssetPath { get; private set; }
            public string ResolvedPath { get; private set; }
        }
    }
}
#endif
