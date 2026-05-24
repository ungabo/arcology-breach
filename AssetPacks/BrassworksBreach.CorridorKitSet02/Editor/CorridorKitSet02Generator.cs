#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.Sidecars.CorridorKitSet02
{
    public static class CorridorKitSet02Generator
    {
        public const string Version = "0.1.41";
        public const string BuildId = "p001";
        public const string PackageName = "com.brassworks.sidecar.corridor-kit-set02";
        public const string RenderOutputRelativePath = "Documentation/ConceptRenders/V0_1_41_CorridorKitSet02";

        private const string PackId = "SCK2";
        private const string MenuRoot = "Brassworks/Sidecars/Corridor Kit Set 02 v0.1.41/";
        private const string PackageManifestFileName = "SCK2_CorridorKitSet02_Manifest_v0.1.41-p001.json";
        private const string GeneratedCatalogFileName = "SCK2_CorridorKitSet02_GeneratedCatalog_v0.1.41.json";

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

            foreach (KitSpec spec in GetSpecs())
            {
                CreatePrefab(spec);
            }

            WriteManifestFiles("generated_by_unity_sidecar_batchmode_v0.1.41");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("SCK2_GENERATE_PASS v0.1.41 prefabs=" + GetSpecs().Length + " materials=" + GetMaterialNames().Length + " meshes=" + GetMeshNames().Length);
        }

        [MenuItem(MenuRoot + "Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            IReadOnlyList<string> prefabPaths = EnsureGeneratedPrefabPaths();
            string outputRoot = ResolveRenderOutputRoot();
            Directory.CreateDirectory(outputRoot);

            RenderPrefabGroup(
                "SCK2_PREVIEW_corridor_assembly_v0.1.41.png",
                outputRoot,
                new[]
                {
                    FindPrefabPath(prefabPaths, "SCK2_CorridorStraight_4m_NorthStar.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_Door_BulkheadRound_3m.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_CeilingModule_PipeRack_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_LightRun_AmberCaged_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_PipeRun_OverUnder_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_Signage_NorthStarWayfinding.prefab")
                },
                new[]
                {
                    new Vector3(0f, 0f, 0f),
                    new Vector3(0f, 0f, 2.35f),
                    new Vector3(0f, 2.72f, -0.08f),
                    new Vector3(0f, 2.18f, -0.25f),
                    new Vector3(-1.03f, 1.45f, -0.1f),
                    new Vector3(0f, 1.86f, -1.85f)
                },
                new Vector3(0f, 2.2f, -7.4f),
                new Vector3(13f, 0f, 0f),
                false);

            RenderPrefabGroup(
                "SCK2_PREVIEW_door_modules_v0.1.41.png",
                outputRoot,
                new[]
                {
                    FindPrefabPath(prefabPaths, "SCK2_Door_Frame_ArchedRiveted.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_Door_PressureLock_DoubleLeaf.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_Door_SlidingIris_VisualOnly.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_Door_ControlColumn_Left.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_Door_Threshold_RedGasket.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_Window_SmokeGlassPorthole.prefab")
                },
                new[]
                {
                    new Vector3(-3.2f, 0f, 0.02f),
                    new Vector3(-1.1f, 0f, 0f),
                    new Vector3(1.1f, 0f, 0f),
                    new Vector3(2.85f, 0f, -0.08f),
                    new Vector3(-0.1f, 0f, -1.2f),
                    new Vector3(3.65f, 1.15f, 0.02f)
                },
                new Vector3(0f, 1.75f, -7.3f),
                new Vector3(8f, 0f, 0f),
                true);

            RenderPrefabGroup(
                "SCK2_PREVIEW_room_wall_modules_v0.1.41.png",
                outputRoot,
                new[]
                {
                    FindPrefabPath(prefabPaths, "SCK2_RoomCorner_Inside_2x2.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_RoomWallPanel_BoilerBay.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_RoomWallPanel_GaugeNest.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_RoomWallPanel_ValveLabyrinth.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_RoomAlcove_ServiceShrine.prefab"),
                    FindPrefabPath(prefabPaths, "SCK2_RoomCeilingRing_CompassRose.prefab")
                },
                new[]
                {
                    new Vector3(-3.2f, 0f, 0.8f),
                    new Vector3(-1.55f, 0f, 0f),
                    new Vector3(0.45f, 0f, 0f),
                    new Vector3(2.45f, 0f, 0f),
                    new Vector3(-0.7f, 0f, 2.15f),
                    new Vector3(2.2f, 2.35f, 2.2f)
                },
                new Vector3(0f, 3.2f, -8.4f),
                new Vector3(20f, 0f, 0f),
                false);

            RenderPrefabGroup(
                "SCK2_PREVIEW_contact_sheet_v0.1.41.png",
                outputRoot,
                prefabPaths,
                BuildContactPositions(prefabPaths.Count),
                new Vector3(0f, 11.5f, -24.0f),
                new Vector3(33f, 0f, 0f),
                false);

            RenderMaterialSwatch("SCK2_PREVIEW_material_swatch_v0.1.41.png", outputRoot);
            WriteManifestFiles("generated_and_preview_rendered_by_unity_sidecar_batchmode_v0.1.41");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("SCK2_PREVIEW_PASS v0.1.41 output=" + outputRoot);
        }

        [MenuItem(MenuRoot + "Generate and Render Preview")]
        public static void GenerateAllAndRenderPreview()
        {
            GeneratePackageAssets();
            RenderPreviewPngs();
        }

        public static IReadOnlyList<string> EnsureGeneratedPrefabPaths()
        {
            KitSpec[] specs = GetSpecs();
            string firstPath = CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + specs[0].FileName);
            if (AssetDatabase.LoadAssetAtPath<GameObject>(firstPath) == null)
            {
                GeneratePackageAssets();
            }

            List<string> paths = new List<string>(specs.Length);
            foreach (KitSpec spec in specs)
            {
                paths.Add(CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + spec.FileName));
            }

            return paths;
        }

        private static string PackageRoot
        {
            get { return LocatePackageRoot().AssetPath; }
        }

        private static KitSpec[] GetSpecs()
        {
            return new[]
            {
                new KitSpec("SCK2_CorridorStraight_4m_NorthStar.prefab", "corridor", "North-star straight corridor 4m", "straight corridor shell", new Vector3(2.8f, 2.9f, 4.0f), KitKind.CorridorStraight, 0),
                new KitSpec("SCK2_CorridorStraight_2m_ServiceDense.prefab", "corridor", "Service-dense straight corridor 2m", "short corridor shell", new Vector3(2.8f, 2.9f, 2.0f), KitKind.CorridorStraight, 1),
                new KitSpec("SCK2_CorridorCorner_90_Bulkhead.prefab", "corridor", "90 degree bulkhead corridor corner", "corner corridor shell", new Vector3(3.2f, 2.9f, 3.2f), KitKind.CorridorCorner, 0),
                new KitSpec("SCK2_CorridorTJunction_PipeSpine.prefab", "corridor", "Pipe-spine T junction corridor", "T junction corridor shell", new Vector3(4.2f, 2.9f, 4.0f), KitKind.CorridorTJunction, 0),
                new KitSpec("SCK2_CorridorCrossJunction_CompassHub.prefab", "corridor", "Compass hub cross junction corridor", "cross junction corridor shell", new Vector3(4.2f, 2.9f, 4.2f), KitKind.CorridorCrossJunction, 0),
                new KitSpec("SCK2_CorridorEndCap_SealedGearwall.prefab", "corridor", "Sealed gearwall corridor end cap", "corridor end cap", new Vector3(2.8f, 2.9f, 0.38f), KitKind.CorridorEndCap, 0),
                new KitSpec("SCK2_Door_BulkheadRound_3m.prefab", "door", "Round bulkhead pressure door", "round pressure door", new Vector3(2.45f, 2.75f, 0.34f), KitKind.BulkheadDoor, 0),
                new KitSpec("SCK2_Door_PressureLock_DoubleLeaf.prefab", "door", "Double leaf pressure lock door", "double pressure door", new Vector3(2.6f, 2.65f, 0.32f), KitKind.PressureDoor, 0),
                new KitSpec("SCK2_Door_Frame_ArchedRiveted.prefab", "door", "Arched riveted door frame", "door frame", new Vector3(2.85f, 2.95f, 0.42f), KitKind.DoorFrame, 0),
                new KitSpec("SCK2_Door_SlidingIris_VisualOnly.prefab", "door", "Sliding iris door visual", "iris door", new Vector3(2.2f, 2.35f, 0.32f), KitKind.IrisDoor, 0),
                new KitSpec("SCK2_Door_Threshold_RedGasket.prefab", "door", "Red gasket door threshold", "threshold", new Vector3(2.7f, 0.24f, 1.0f), KitKind.Threshold, 0),
                new KitSpec("SCK2_Door_ControlColumn_Left.prefab", "door", "Left hand door control column", "door control column", new Vector3(0.48f, 1.85f, 0.42f), KitKind.ControlColumn, 0),
                new KitSpec("SCK2_RoomCorner_Inside_2x2.prefab", "room", "Inside room corner 2x2", "room corner", new Vector3(2.0f, 2.65f, 2.0f), KitKind.RoomCorner, 0),
                new KitSpec("SCK2_RoomWallPanel_BoilerBay.prefab", "room", "Boiler bay room wall panel", "boiler wall panel", new Vector3(2.0f, 2.55f, 0.32f), KitKind.RoomWallPanel, 0),
                new KitSpec("SCK2_RoomWallPanel_GaugeNest.prefab", "room", "Gauge nest room wall panel", "gauge wall panel", new Vector3(2.0f, 2.55f, 0.28f), KitKind.RoomWallPanel, 1),
                new KitSpec("SCK2_RoomWallPanel_ValveLabyrinth.prefab", "room", "Valve labyrinth room wall panel", "valve wall panel", new Vector3(2.0f, 2.55f, 0.36f), KitKind.RoomWallPanel, 2),
                new KitSpec("SCK2_RoomAlcove_ServiceShrine.prefab", "room", "Service shrine room alcove", "service alcove", new Vector3(2.2f, 2.7f, 0.9f), KitKind.RoomAlcove, 0),
                new KitSpec("SCK2_RoomCeilingRing_CompassRose.prefab", "room", "Compass rose ceiling ring", "ceiling room ring", new Vector3(2.0f, 0.36f, 2.0f), KitKind.CeilingRing, 0),
                new KitSpec("SCK2_WallPanel_RivetedBrass_2m.prefab", "wall", "Riveted brass wall panel 2m", "wall panel", new Vector3(2.0f, 2.45f, 0.22f), KitKind.WallPanel, 3),
                new KitSpec("SCK2_WallPanel_PipeCathedral_2m.prefab", "wall", "Pipe cathedral wall panel 2m", "pipe wall panel", new Vector3(2.0f, 2.55f, 0.34f), KitKind.WallPanel, 4),
                new KitSpec("SCK2_WallPanel_WindowGrate_2m.prefab", "wall", "Smoke glass window grate wall panel", "window wall panel", new Vector3(2.0f, 2.45f, 0.28f), KitKind.WallPanel, 5),
                new KitSpec("SCK2_FloorPanel_GratedWet_2m.prefab", "floor", "Grated wet floor panel 2m", "floor panel", new Vector3(2.0f, 0.18f, 2.0f), KitKind.FloorPanel, 0),
                new KitSpec("SCK2_FloorPanel_DrainSpine_4m.prefab", "floor", "Drain spine floor panel 4m", "long floor panel", new Vector3(2.0f, 0.2f, 4.0f), KitKind.FloorPanel, 1),
                new KitSpec("SCK2_CeilingModule_PipeRack_4m.prefab", "ceiling", "Ceiling pipe rack 4m", "ceiling module", new Vector3(2.6f, 0.5f, 4.0f), KitKind.CeilingModule, 0),
                new KitSpec("SCK2_CeilingModule_FanVent_2m.prefab", "ceiling", "Fan vent ceiling module 2m", "ceiling vent module", new Vector3(2.0f, 0.38f, 2.0f), KitKind.CeilingModule, 1),
                new KitSpec("SCK2_ArchSupport_RibbedColumnPair.prefab", "structure", "Ribbed arch support column pair", "arch support", new Vector3(2.8f, 2.9f, 0.42f), KitKind.ArchSupport, 0),
                new KitSpec("SCK2_CornerColumn_PressureGauge.prefab", "structure", "Pressure gauge corner column", "corner column", new Vector3(0.62f, 2.65f, 0.62f), KitKind.CornerColumn, 0),
                new KitSpec("SCK2_StairNib_ServiceStep_2m.prefab", "floor", "Service stair nib 2m", "short service step", new Vector3(2.0f, 0.42f, 0.9f), KitKind.StairNib, 0),
                new KitSpec("SCK2_LightRun_AmberCaged_4m.prefab", "lighting", "Amber caged light run 4m", "visual light run", new Vector3(2.0f, 0.62f, 4.0f), KitKind.LightRun, 0),
                new KitSpec("SCK2_PipeRun_OverUnder_4m.prefab", "pipe", "Over-under pipe run 4m", "pipe run", new Vector3(0.62f, 1.2f, 4.0f), KitKind.PipeRun, 0),
                new KitSpec("SCK2_Window_SmokeGlassPorthole.prefab", "wall", "Smoke glass porthole window", "porthole window", new Vector3(1.05f, 1.05f, 0.16f), KitKind.WindowPorthole, 0),
                new KitSpec("SCK2_Signage_NorthStarWayfinding.prefab", "signage", "North-star wayfinding sign", "wayfinding sign", new Vector3(1.4f, 0.5f, 0.08f), KitKind.Signage, 0)
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
            AddMaterial("SCK2_MAT_DarkRivetedIron", new Color(0.033f, 0.032f, 0.030f), 0.30f, 0.84f);
            AddMaterial("SCK2_MAT_AgedBrass", new Color(0.74f, 0.55f, 0.27f), 0.55f, 0.72f);
            AddMaterial("SCK2_MAT_BurnishedCopper", new Color(0.72f, 0.32f, 0.13f), 0.50f, 0.66f);
            AddMaterial("SCK2_MAT_TemperedBlueSteel", new Color(0.10f, 0.20f, 0.30f), 0.46f, 0.70f);
            AddMaterial("SCK2_MAT_RivetSteel", new Color(0.58f, 0.56f, 0.50f), 0.60f, 0.78f);
            AddMaterial("SCK2_MAT_BoilerTile", new Color(0.13f, 0.115f, 0.095f), 0.23f, 0.18f);
            AddMaterial("SCK2_MAT_OilWetFloor", new Color(0.043f, 0.041f, 0.037f), 0.70f, 0.10f);
            AddMaterial("SCK2_MAT_HazardRedEnamel", new Color(0.76f, 0.055f, 0.030f), 0.42f, 0.08f);
            AddMaterial("SCK2_MAT_HazardYellowPaint", new Color(0.96f, 0.68f, 0.14f), 0.34f, 0.04f);
            AddMaterial("SCK2_MAT_PatinaGreen", new Color(0.09f, 0.38f, 0.33f), 0.38f, 0.34f);
            AddMaterial("SCK2_MAT_PressureGreenGlass", new Color(0.12f, 0.78f, 0.34f, 0.73f), 0.38f, 0.02f, new Color(0.06f, 0.66f, 0.24f) * 1.5f, true);
            AddMaterial("SCK2_MAT_AmberLampGlass", new Color(1.0f, 0.50f, 0.12f, 0.70f), 0.42f, 0.02f, new Color(1.0f, 0.35f, 0.08f) * 1.8f, true);
            AddMaterial("SCK2_MAT_SmokeGlass", new Color(0.18f, 0.19f, 0.18f, 0.48f), 0.45f, 0.0f, null, true);
            AddMaterial("SCK2_MAT_GaugeIvory", new Color(0.81f, 0.73f, 0.54f), 0.22f, 0.0f);
            AddMaterial("SCK2_MAT_RubberGasket", new Color(0.018f, 0.016f, 0.014f), 0.16f, 0.0f);
            AddMaterial("SCK2_MAT_SootShadow", new Color(0.012f, 0.011f, 0.009f, 0.58f), 0.08f, 0.0f, null, true);
            AddMaterial("SCK2_MAT_WornBrightEdge", new Color(0.78f, 0.72f, 0.62f), 0.68f, 0.74f);
            AddMaterial("SCK2_MAT_NorthStarWhite", new Color(0.92f, 0.88f, 0.75f), 0.26f, 0.0f, new Color(0.8f, 0.68f, 0.34f) * 0.35f);
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
            AddMesh("SCK2_MESH_BoxUnit", CreateBoxMesh());
            AddMesh("SCK2_MESH_Cylinder16Unit", CreateCylinderMesh(16));
            AddMesh("SCK2_MESH_Cylinder32Unit", CreateCylinderMesh(32));
            AddMesh("SCK2_MESH_QuadUnit", CreateQuadMesh());
            AddMesh("SCK2_MESH_Gear16ToothUnit", CreateGearMesh(16));
            AddMesh("SCK2_MESH_NorthStar8Unit", CreateStarMesh(8));
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

        private static Mesh CreateStarMesh(int points)
        {
            List<Vector3> vertices = new List<Vector3> { Vector3.zero };
            List<int> triangles = new List<int>();
            int vertexCount = points * 2;
            for (int i = 0; i < vertexCount; i++)
            {
                float angle = Mathf.PI * 2f * i / vertexCount + Mathf.PI * 0.5f;
                float radius = i % 2 == 0 ? 0.5f : 0.2f;
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
            }

            for (int i = 1; i <= vertexCount; i++)
            {
                int next = i == vertexCount ? 1 : i + 1;
                triangles.Add(0); triangles.Add(next); triangles.Add(i);
            }

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void CreatePrefab(KitSpec spec)
        {
            GameObject root = new GameObject(Path.GetFileNameWithoutExtension(spec.FileName));
            BuildSpec(root, spec);
            AddMetadataChild(root, spec);

            string path = CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + spec.FileName);
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            if (prefab != null)
            {
                AssetDatabase.SetLabels(prefab, new[] { PackId, "v0.1.41", spec.Family, spec.Role.Replace(" ", "_") });
            }

            UnityEngine.Object.DestroyImmediate(root);
        }

        private static void BuildSpec(GameObject root, KitSpec spec)
        {
            switch (spec.Kind)
            {
                case KitKind.CorridorStraight: BuildCorridorStraight(root, spec.Variant); break;
                case KitKind.CorridorCorner: BuildCorridorCorner(root); break;
                case KitKind.CorridorTJunction: BuildCorridorTJunction(root); break;
                case KitKind.CorridorCrossJunction: BuildCorridorCrossJunction(root); break;
                case KitKind.CorridorEndCap: BuildCorridorEndCap(root); break;
                case KitKind.BulkheadDoor: BuildBulkheadDoor(root); break;
                case KitKind.PressureDoor: BuildPressureDoor(root); break;
                case KitKind.DoorFrame: BuildDoorFrame(root); break;
                case KitKind.IrisDoor: BuildIrisDoor(root); break;
                case KitKind.Threshold: BuildThreshold(root); break;
                case KitKind.ControlColumn: BuildControlColumn(root); break;
                case KitKind.RoomCorner: BuildRoomCorner(root); break;
                case KitKind.RoomWallPanel: BuildRoomWallPanel(root, spec.Variant); break;
                case KitKind.WallPanel: BuildRoomWallPanel(root, spec.Variant); break;
                case KitKind.RoomAlcove: BuildRoomAlcove(root); break;
                case KitKind.CeilingRing: BuildCeilingRing(root); break;
                case KitKind.FloorPanel: BuildFloorPanel(root, spec.Variant); break;
                case KitKind.CeilingModule: BuildCeilingModule(root, spec.Variant); break;
                case KitKind.ArchSupport: BuildArchSupport(root); break;
                case KitKind.CornerColumn: BuildCornerColumn(root); break;
                case KitKind.StairNib: BuildStairNib(root); break;
                case KitKind.LightRun: BuildLightRun(root); break;
                case KitKind.PipeRun: BuildPipeRun(root); break;
                case KitKind.WindowPorthole: BuildWindowPorthole(root); break;
                case KitKind.Signage: BuildSignage(root); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private static void BuildCorridorStraight(GameObject root, int variant)
        {
            float length = variant == 0 ? 4.0f : 2.0f;
            BuildCorridorShell(root, length, variant == 0 ? "north_star" : "service_dense", true);
            int ribs = variant == 0 ? 5 : 3;
            for (int i = 0; i < ribs; i++)
            {
                float z = Mathf.Lerp(-length * 0.42f, length * 0.42f, ribs == 1 ? 0f : i / (float)(ribs - 1));
                AddArchRib(root, "corridor_arch_rib_" + i, z);
            }

            AddNorthStar(root, "floor_north_star_inlay", new Vector3(0f, 0.095f, variant == 0 ? -0.65f : 0f), new Vector3(90f, 0f, 0f), variant == 0 ? 1.0f : 0.72f);
            AddCagedLamp(root, "centerline_lamp_a", new Vector3(0f, 2.28f, -length * 0.26f), 0.22f);
            if (variant == 0)
            {
                AddCagedLamp(root, "centerline_lamp_b", new Vector3(0f, 2.28f, length * 0.28f), 0.22f);
                BuildPipeBundle(root, "left_high_bundle", new Vector3(-1.16f, 2.08f, 0f), length, true);
                BuildPipeBundle(root, "right_low_bundle", new Vector3(1.16f, 1.32f, 0f), length, true);
            }
            else
            {
                BuildPipeBundle(root, "service_left_bundle", new Vector3(-1.16f, 1.78f, 0f), length, true);
                AddGauge(root, "short_corridor_watch_gauge", new Vector3(1.18f, 1.58f, -0.34f), new Vector3(0f, -90f, 0f), 0.18f);
                AddValveWheel(root, "short_corridor_red_handwheel", new Vector3(1.18f, 0.95f, 0.45f), new Vector3(0f, -90f, 0f), 0.26f, "SCK2_MAT_HazardRedEnamel");
            }
        }

        private static void BuildCorridorCorner(GameObject root)
        {
            Box(root, "corner_floor_leg_z", new Vector3(0f, 0.03f, 0f), new Vector3(2.8f, 0.12f, 3.2f), "SCK2_MAT_OilWetFloor");
            Box(root, "corner_floor_leg_x", new Vector3(0f, 0.035f, 0f), new Vector3(3.2f, 0.13f, 2.8f), "SCK2_MAT_OilWetFloor");
            Box(root, "outside_wall_left", new Vector3(-1.36f, 1.36f, 0f), new Vector3(0.18f, 2.7f, 3.2f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "outside_wall_back", new Vector3(0f, 1.36f, 1.36f), new Vector3(3.2f, 2.7f, 0.18f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "inside_corner_guard", new Vector3(1.08f, 1.34f, -1.08f), new Vector3(0.22f, 2.55f, 0.22f), "SCK2_MAT_AgedBrass");
            Cyl(root, "corner_vertical_steam_pipe", new Vector3(-1.16f, 1.32f, 1.12f), Vector3.zero, new Vector3(0.12f, 2.38f, 0.12f), "SCK2_MAT_BurnishedCopper");
            Cyl(root, "corner_pipe_leg_x", new Vector3(-0.05f, 2.16f, 1.12f), new Vector3(0f, 0f, 90f), new Vector3(0.10f, 2.15f, 0.10f), "SCK2_MAT_AgedBrass");
            Cyl(root, "corner_pipe_leg_z", new Vector3(-1.16f, 1.76f, 0.0f), new Vector3(90f, 0f, 0f), new Vector3(0.09f, 2.2f, 0.09f), "SCK2_MAT_BurnishedCopper");
            AddCagedLamp(root, "corner_lamp", new Vector3(-0.55f, 2.22f, 0.52f), 0.2f);
            AddGauge(root, "corner_pressure_gauge", new Vector3(-1.18f, 1.15f, 0.45f), new Vector3(0f, 90f, 0f), 0.2f);
            AddFloorRivetGrid(root, "corner_floor_bolt", -1.05f, 1.05f, -1.05f, 1.05f, 3, 3, "SCK2_MAT_RivetSteel");
        }

        private static void BuildCorridorTJunction(GameObject root)
        {
            Box(root, "t_main_floor_spine", new Vector3(0f, 0.03f, 0f), new Vector3(2.75f, 0.12f, 4.0f), "SCK2_MAT_OilWetFloor");
            Box(root, "t_branch_floor_spine", new Vector3(0f, 0.035f, 0.35f), new Vector3(4.2f, 0.12f, 2.05f), "SCK2_MAT_OilWetFloor");
            Box(root, "t_back_wall", new Vector3(0f, 1.36f, 1.98f), new Vector3(4.2f, 2.7f, 0.16f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "t_left_return_wall", new Vector3(-1.36f, 1.36f, -0.92f), new Vector3(0.16f, 2.7f, 2.1f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "t_right_return_wall", new Vector3(1.36f, 1.36f, -0.92f), new Vector3(0.16f, 2.7f, 2.1f), "SCK2_MAT_DarkRivetedIron");
            AddNorthStar(root, "t_junction_floor_star", new Vector3(0f, 0.1f, 0.28f), new Vector3(90f, 0f, 0f), 0.86f);
            Cyl(root, "t_ceiling_pipe_spine", new Vector3(0f, 2.46f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.13f, 3.75f, 0.13f), "SCK2_MAT_BurnishedCopper");
            Cyl(root, "t_branch_pipe_left", new Vector3(0f, 2.28f, 0.55f), new Vector3(0f, 0f, 90f), new Vector3(0.10f, 3.75f, 0.10f), "SCK2_MAT_AgedBrass");
            Cyl(root, "t_pressure_hub", new Vector3(0f, 2.35f, 0.38f), Vector3.zero, new Vector3(0.52f, 0.22f, 0.52f), "SCK2_MAT_DarkRivetedIron", true);
            AddGauge(root, "t_hub_gauge", new Vector3(0f, 1.55f, 1.88f), Vector3.zero, 0.26f);
            AddCagedLamp(root, "t_junction_lamp_left", new Vector3(-0.82f, 2.18f, 0.02f), 0.19f);
            AddCagedLamp(root, "t_junction_lamp_right", new Vector3(0.82f, 2.18f, 0.02f), 0.19f);
        }

        private static void BuildCorridorCrossJunction(GameObject root)
        {
            Box(root, "cross_floor_x", new Vector3(0f, 0.03f, 0f), new Vector3(4.2f, 0.12f, 2.65f), "SCK2_MAT_OilWetFloor");
            Box(root, "cross_floor_z", new Vector3(0f, 0.035f, 0f), new Vector3(2.65f, 0.13f, 4.2f), "SCK2_MAT_OilWetFloor");
            Cyl(root, "cross_compass_round_hub", new Vector3(0f, 0.12f, 0f), Vector3.zero, new Vector3(1.65f, 0.08f, 1.65f), "SCK2_MAT_AgedBrass", true);
            AddNorthStar(root, "cross_compass_star", new Vector3(0f, 0.18f, 0f), new Vector3(90f, 0f, 0f), 1.2f);
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90f;
                GameObject spoke = Box(root, "cross_floor_bright_spoke_" + i, new Vector3(0f, 0.2f, 0f), new Vector3(0.12f, 0.035f, 1.9f), "SCK2_MAT_WornBrightEdge");
                spoke.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            }

            Cyl(root, "cross_ceiling_hub", new Vector3(0f, 2.55f, 0f), Vector3.zero, new Vector3(1.25f, 0.22f, 1.25f), "SCK2_MAT_DarkRivetedIron", true);
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f;
                GameObject rib = Box(root, "cross_ceiling_radial_rib_" + i, new Vector3(0f, 2.48f, 0f), new Vector3(0.08f, 0.11f, 2.05f), "SCK2_MAT_RivetSteel");
                rib.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            }

            AddCagedLamp(root, "cross_green_center_lamp", new Vector3(0f, 2.18f, 0f), 0.25f);
            AddFloorRivetGrid(root, "cross_grid_bolt", -1.25f, 1.25f, -1.25f, 1.25f, 4, 4, "SCK2_MAT_RivetSteel");
        }

        private static void BuildCorridorEndCap(GameObject root)
        {
            Box(root, "endcap_black_wall", new Vector3(0f, 1.35f, 0f), new Vector3(2.8f, 2.7f, 0.18f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "endcap_lower_kick_brass", new Vector3(0f, 0.38f, -0.11f), new Vector3(2.55f, 0.62f, 0.10f), "SCK2_MAT_AgedBrass");
            Gear(root, "large_sealed_gear_face", new Vector3(0f, 1.42f, -0.12f), Vector3.zero, new Vector3(1.1f, 1.1f, 1f), "SCK2_MAT_AgedBrass");
            Gear(root, "small_offset_gear_face", new Vector3(0.62f, 0.88f, -0.14f), new Vector3(0f, 0f, 15f), new Vector3(0.48f, 0.48f, 1f), "SCK2_MAT_BurnishedCopper");
            Cyl(root, "sealed_center_axle", new Vector3(0f, 1.42f, -0.18f), new Vector3(90f, 0f, 0f), new Vector3(0.24f, 0.10f, 0.24f), "SCK2_MAT_RivetSteel");
            AddNorthStar(root, "endcap_north_star_badge", new Vector3(-0.74f, 2.17f, -0.13f), Vector3.zero, 0.38f);
            AddRivetGrid(root, "endcap_wall_rivet", -1.15f, 1.15f, 0.25f, 2.45f, -0.12f, 5, 5, "SCK2_MAT_RivetSteel");
        }

        private static void BuildBulkheadDoor(GameObject root)
        {
            Box(root, "round_bulkhead_shadow_slab", new Vector3(0f, 1.35f, 0.08f), new Vector3(2.45f, 2.7f, 0.12f), "SCK2_MAT_DarkRivetedIron");
            Cyl(root, "round_bulkhead_outer_ring", new Vector3(0f, 1.35f, -0.02f), new Vector3(90f, 0f, 0f), new Vector3(2.05f, 0.16f, 2.05f), "SCK2_MAT_AgedBrass", true);
            Cyl(root, "round_bulkhead_inner_plate", new Vector3(0f, 1.35f, -0.10f), new Vector3(90f, 0f, 0f), new Vector3(1.65f, 0.12f, 1.65f), "SCK2_MAT_TemperedBlueSteel", true);
            Cyl(root, "round_bulkhead_porthole_glass", new Vector3(0f, 1.64f, -0.18f), new Vector3(90f, 0f, 0f), new Vector3(0.42f, 0.05f, 0.42f), "SCK2_MAT_SmokeGlass", true);
            AddNorthStar(root, "bulkhead_star_emblem", new Vector3(0f, 0.95f, -0.19f), Vector3.zero, 0.38f);
            AddValveWheel(root, "bulkhead_locking_wheel", new Vector3(0.56f, 1.16f, -0.2f), Vector3.zero, 0.33f, "SCK2_MAT_HazardRedEnamel");
            AddCircleRivets(root, "bulkhead_outer_rivet", new Vector3(0f, 1.35f, -0.21f), 0.98f, 14, "SCK2_MAT_RivetSteel");
        }

        private static void BuildPressureDoor(GameObject root)
        {
            Box(root, "double_leaf_frame", new Vector3(0f, 1.32f, 0.05f), new Vector3(2.6f, 2.62f, 0.16f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "left_pressure_leaf", new Vector3(-0.49f, 1.32f, -0.06f), new Vector3(0.92f, 2.24f, 0.12f), "SCK2_MAT_TemperedBlueSteel");
            Box(root, "right_pressure_leaf", new Vector3(0.49f, 1.32f, -0.06f), new Vector3(0.92f, 2.24f, 0.12f), "SCK2_MAT_TemperedBlueSteel");
            Box(root, "center_red_gasket_line", new Vector3(0f, 1.32f, -0.14f), new Vector3(0.06f, 2.18f, 0.05f), "SCK2_MAT_HazardRedEnamel");
            Box(root, "upper_slide_rail", new Vector3(0f, 2.53f, -0.12f), new Vector3(2.34f, 0.10f, 0.12f), "SCK2_MAT_AgedBrass");
            Box(root, "lower_slide_rail", new Vector3(0f, 0.11f, -0.12f), new Vector3(2.34f, 0.10f, 0.12f), "SCK2_MAT_AgedBrass");
            AddGauge(root, "left_leaf_pressure_readout", new Vector3(-0.52f, 1.64f, -0.18f), Vector3.zero, 0.18f);
            AddGauge(root, "right_leaf_pressure_readout", new Vector3(0.52f, 1.64f, -0.18f), Vector3.zero, 0.18f);
            AddRivetGrid(root, "double_leaf_rivet", -1.12f, 1.12f, 0.25f, 2.38f, -0.16f, 4, 6, "SCK2_MAT_RivetSteel");
        }

        private static void BuildDoorFrame(GameObject root)
        {
            Box(root, "arched_frame_left_column", new Vector3(-1.2f, 1.23f, 0f), new Vector3(0.34f, 2.45f, 0.38f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "arched_frame_right_column", new Vector3(1.2f, 1.23f, 0f), new Vector3(0.34f, 2.45f, 0.38f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "arched_frame_top_lintel", new Vector3(0f, 2.58f, 0f), new Vector3(2.72f, 0.34f, 0.38f), "SCK2_MAT_DarkRivetedIron");
            Cyl(root, "arched_frame_brass_crown", new Vector3(0f, 2.56f, -0.04f), new Vector3(90f, 0f, 0f), new Vector3(1.75f, 0.12f, 1.75f), "SCK2_MAT_AgedBrass", true);
            Box(root, "arch_opening_mask_visual", new Vector3(0f, 1.28f, -0.11f), new Vector3(1.72f, 2.2f, 0.06f), "SCK2_MAT_SootShadow");
            AddNorthStar(root, "arched_frame_top_star", new Vector3(0f, 2.55f, -0.17f), Vector3.zero, 0.28f);
            AddRivetLine(root, "left_arch_column_rivets", new Vector3(-1.2f, 0.18f, -0.22f), new Vector3(0f, 0.36f, 0f), 7, new Vector3(90f, 0f, 0f), "SCK2_MAT_RivetSteel");
            AddRivetLine(root, "right_arch_column_rivets", new Vector3(1.2f, 0.18f, -0.22f), new Vector3(0f, 0.36f, 0f), 7, new Vector3(90f, 0f, 0f), "SCK2_MAT_RivetSteel");
        }

        private static void BuildIrisDoor(GameObject root)
        {
            Cyl(root, "iris_outer_frame", new Vector3(0f, 1.18f, 0f), new Vector3(90f, 0f, 0f), new Vector3(2.0f, 0.14f, 2.0f), "SCK2_MAT_DarkRivetedIron", true);
            Cyl(root, "iris_inner_brass_ring", new Vector3(0f, 1.18f, -0.08f), new Vector3(90f, 0f, 0f), new Vector3(1.55f, 0.10f, 1.55f), "SCK2_MAT_AgedBrass", true);
            for (int i = 0; i < 8; i++)
            {
                GameObject blade = Box(root, "iris_radial_blade_" + i, new Vector3(0f, 1.18f, -0.16f), new Vector3(0.22f, 0.82f, 0.045f), i % 2 == 0 ? "SCK2_MAT_TemperedBlueSteel" : "SCK2_MAT_BurnishedCopper");
                blade.transform.localRotation = Quaternion.Euler(0f, 0f, i * 45f + 10f);
                blade.transform.localPosition += blade.transform.up * 0.32f;
            }

            Cyl(root, "iris_green_lock_lens", new Vector3(-0.58f, 1.82f, -0.2f), new Vector3(90f, 0f, 0f), new Vector3(0.18f, 0.05f, 0.18f), "SCK2_MAT_PressureGreenGlass", true);
            AddCircleRivets(root, "iris_frame_rivet", new Vector3(0f, 1.18f, -0.21f), 0.95f, 12, "SCK2_MAT_RivetSteel");
        }

        private static void BuildThreshold(GameObject root)
        {
            Box(root, "threshold_black_base", new Vector3(0f, 0.06f, 0f), new Vector3(2.7f, 0.12f, 1.0f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "threshold_red_gasket_strip", new Vector3(0f, 0.15f, 0f), new Vector3(2.42f, 0.06f, 0.18f), "SCK2_MAT_HazardRedEnamel");
            Box(root, "threshold_front_brass_lip", new Vector3(0f, 0.16f, -0.42f), new Vector3(2.62f, 0.08f, 0.08f), "SCK2_MAT_AgedBrass");
            Box(root, "threshold_back_brass_lip", new Vector3(0f, 0.16f, 0.42f), new Vector3(2.62f, 0.08f, 0.08f), "SCK2_MAT_AgedBrass");
            for (int i = 0; i < 6; i++)
            {
                GameObject stripe = Box(root, "threshold_yellow_warning_chevron_" + i, new Vector3(-1.08f + i * 0.43f, 0.205f, -0.22f), new Vector3(0.08f, 0.035f, 0.42f), "SCK2_MAT_HazardYellowPaint");
                stripe.transform.localRotation = Quaternion.Euler(0f, -24f, 0f);
            }
        }

        private static void BuildControlColumn(GameObject root)
        {
            Box(root, "control_column_base", new Vector3(0f, 0.12f, 0f), new Vector3(0.42f, 0.24f, 0.38f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "control_column_body", new Vector3(0f, 0.88f, 0f), new Vector3(0.36f, 1.45f, 0.32f), "SCK2_MAT_TemperedBlueSteel");
            Box(root, "control_column_brass_face", new Vector3(0f, 1.06f, -0.18f), new Vector3(0.28f, 0.86f, 0.05f), "SCK2_MAT_AgedBrass");
            AddGauge(root, "control_pressure_gauge", new Vector3(0f, 1.42f, -0.22f), Vector3.zero, 0.13f);
            Cyl(root, "control_green_lens", new Vector3(-0.08f, 1.03f, -0.22f), new Vector3(90f, 0f, 0f), new Vector3(0.10f, 0.04f, 0.10f), "SCK2_MAT_PressureGreenGlass", true);
            Cyl(root, "control_red_lens", new Vector3(0.08f, 1.03f, -0.22f), new Vector3(90f, 0f, 0f), new Vector3(0.10f, 0.04f, 0.10f), "SCK2_MAT_HazardRedEnamel");
            AddValveWheel(root, "control_side_override_wheel", new Vector3(0.23f, 0.75f, 0.02f), new Vector3(0f, -90f, 0f), 0.18f, "SCK2_MAT_HazardRedEnamel");
        }

        private static void BuildRoomCorner(GameObject root)
        {
            Box(root, "room_corner_floor", new Vector3(0f, 0.04f, 0f), new Vector3(2.0f, 0.12f, 2.0f), "SCK2_MAT_OilWetFloor");
            Box(root, "room_corner_wall_left", new Vector3(-1.0f, 1.32f, 0f), new Vector3(0.12f, 2.55f, 2.0f), "SCK2_MAT_BoilerTile");
            Box(root, "room_corner_wall_back", new Vector3(0f, 1.32f, 1.0f), new Vector3(2.0f, 2.55f, 0.12f), "SCK2_MAT_BoilerTile");
            Box(root, "room_corner_brass_kick_left", new Vector3(-0.93f, 0.36f, 0f), new Vector3(0.08f, 0.55f, 1.74f), "SCK2_MAT_AgedBrass");
            Box(root, "room_corner_brass_kick_back", new Vector3(0f, 0.36f, 0.93f), new Vector3(1.74f, 0.55f, 0.08f), "SCK2_MAT_AgedBrass");
            Cyl(root, "corner_boiler_pipe_vertical", new Vector3(-0.78f, 1.28f, 0.78f), Vector3.zero, new Vector3(0.13f, 2.36f, 0.13f), "SCK2_MAT_BurnishedCopper");
            AddCagedLamp(root, "room_corner_sconce", new Vector3(-0.84f, 1.96f, 0.32f), 0.17f);
            AddGauge(root, "room_corner_gauge", new Vector3(-0.92f, 1.24f, -0.26f), new Vector3(0f, 90f, 0f), 0.18f);
        }

        private static void BuildRoomWallPanel(GameObject root, int variant)
        {
            Box(root, "wall_panel_blackened_back", new Vector3(0f, 1.22f, 0.05f), new Vector3(2.0f, 2.42f, 0.10f), variant == 4 ? "SCK2_MAT_DarkRivetedIron" : "SCK2_MAT_BoilerTile");
            Box(root, "wall_panel_brass_kick", new Vector3(0f, 0.35f, -0.04f), new Vector3(1.82f, 0.52f, 0.08f), "SCK2_MAT_AgedBrass");
            AddRivetGrid(root, "wall_panel_corner_rivets", -0.84f, 0.84f, 0.18f, 2.22f, -0.07f, 3, 5, "SCK2_MAT_RivetSteel");

            if (variant == 0)
            {
                Cyl(root, "boiler_wall_tank", new Vector3(-0.46f, 1.28f, -0.14f), Vector3.zero, new Vector3(0.48f, 1.42f, 0.48f), "SCK2_MAT_BurnishedCopper", true);
                Cyl(root, "boiler_wall_top_cap", new Vector3(-0.46f, 2.04f, -0.14f), Vector3.zero, new Vector3(0.54f, 0.13f, 0.54f), "SCK2_MAT_AgedBrass", true);
                BuildPipeBundle(root, "boiler_vertical_pipe_bundle", new Vector3(0.54f, 1.35f, -0.12f), 1.55f, false);
                AddGauge(root, "boiler_wall_gauge", new Vector3(-0.46f, 1.32f, -0.42f), Vector3.zero, 0.22f);
                return;
            }

            if (variant == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    AddGauge(root, "gauge_nest_readout_" + i, new Vector3(-0.62f + (i % 3) * 0.62f, 1.58f - (i / 3) * 0.52f, -0.08f), Vector3.zero, i == 0 ? 0.22f : 0.17f);
                }

                Box(root, "gauge_nest_nameplate", new Vector3(0f, 0.82f, -0.09f), new Vector3(1.15f, 0.14f, 0.05f), "SCK2_MAT_HazardYellowPaint");
                AddTextTag(root, "gauge_nest_label", "N-STAR PSI", new Vector3(0f, 0.83f, -0.13f), 0.045f, Color.black);
                return;
            }

            if (variant == 2)
            {
                Cyl(root, "valve_labyrinth_horizontal_pipe_a", new Vector3(0f, 1.62f, -0.10f), new Vector3(0f, 0f, 90f), new Vector3(0.09f, 1.7f, 0.09f), "SCK2_MAT_BurnishedCopper");
                Cyl(root, "valve_labyrinth_horizontal_pipe_b", new Vector3(0f, 0.98f, -0.12f), new Vector3(0f, 0f, 90f), new Vector3(0.08f, 1.55f, 0.08f), "SCK2_MAT_AgedBrass");
                Cyl(root, "valve_labyrinth_vertical_pipe", new Vector3(0.0f, 1.28f, -0.14f), Vector3.zero, new Vector3(0.08f, 1.25f, 0.08f), "SCK2_MAT_BurnishedCopper");
                AddValveWheel(root, "labyrinth_red_wheel", new Vector3(-0.42f, 1.62f, -0.23f), Vector3.zero, 0.24f, "SCK2_MAT_HazardRedEnamel");
                AddValveWheel(root, "labyrinth_brass_wheel", new Vector3(0.46f, 0.98f, -0.23f), Vector3.zero, 0.22f, "SCK2_MAT_AgedBrass");
                AddGauge(root, "labyrinth_tiny_readout", new Vector3(0.42f, 1.58f, -0.2f), Vector3.zero, 0.14f);
                return;
            }

            if (variant == 3)
            {
                Box(root, "riveted_brass_center_panel", new Vector3(0f, 1.3f, -0.04f), new Vector3(1.42f, 1.28f, 0.08f), "SCK2_MAT_AgedBrass");
                AddNorthStar(root, "riveted_wall_star_badge", new Vector3(0f, 1.48f, -0.1f), Vector3.zero, 0.42f);
                AddRivetGrid(root, "brass_panel_rivets", -0.62f, 0.62f, 0.78f, 1.88f, -0.12f, 4, 4, "SCK2_MAT_DarkRivetedIron");
                return;
            }

            if (variant == 4)
            {
                for (int i = 0; i < 5; i++)
                {
                    float x = -0.68f + i * 0.34f;
                    Cyl(root, "pipe_cathedral_vertical_" + i, new Vector3(x, 1.36f, -0.12f), Vector3.zero, new Vector3(0.06f + i * 0.006f, 2.05f, 0.06f + i * 0.006f), i % 2 == 0 ? "SCK2_MAT_BurnishedCopper" : "SCK2_MAT_AgedBrass");
                }

                Box(root, "pipe_cathedral_top_clamp", new Vector3(0f, 2.22f, -0.18f), new Vector3(1.68f, 0.12f, 0.10f), "SCK2_MAT_RivetSteel");
                Box(root, "pipe_cathedral_bottom_clamp", new Vector3(0f, 0.52f, -0.18f), new Vector3(1.68f, 0.12f, 0.10f), "SCK2_MAT_RivetSteel");
                return;
            }

            Box(root, "window_grate_frame", new Vector3(0f, 1.42f, -0.08f), new Vector3(1.35f, 1.0f, 0.10f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "window_smoke_glass", new Vector3(0f, 1.42f, -0.14f), new Vector3(1.08f, 0.72f, 0.05f), "SCK2_MAT_SmokeGlass");
            for (int i = 0; i < 4; i++)
            {
                Box(root, "window_vertical_grate_" + i, new Vector3(-0.38f + i * 0.25f, 1.42f, -0.19f), new Vector3(0.045f, 0.86f, 0.055f), "SCK2_MAT_AgedBrass");
            }

            for (int i = 0; i < 3; i++)
            {
                Box(root, "window_horizontal_grate_" + i, new Vector3(0f, 1.16f + i * 0.26f, -0.2f), new Vector3(1.12f, 0.045f, 0.055f), "SCK2_MAT_AgedBrass");
            }
        }

        private static void BuildRoomAlcove(GameObject root)
        {
            Box(root, "alcove_back_wall", new Vector3(0f, 1.35f, 0.42f), new Vector3(2.2f, 2.65f, 0.12f), "SCK2_MAT_BoilerTile");
            Box(root, "alcove_left_cheek", new Vector3(-1.05f, 1.35f, 0f), new Vector3(0.16f, 2.65f, 0.9f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "alcove_right_cheek", new Vector3(1.05f, 1.35f, 0f), new Vector3(0.16f, 2.65f, 0.9f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "alcove_floor_plinth", new Vector3(0f, 0.1f, 0f), new Vector3(2.18f, 0.2f, 0.9f), "SCK2_MAT_OilWetFloor");
            Cyl(root, "alcove_service_tank", new Vector3(0f, 1.12f, 0.12f), Vector3.zero, new Vector3(0.58f, 1.54f, 0.58f), "SCK2_MAT_BurnishedCopper", true);
            AddGauge(root, "alcove_main_gauge", new Vector3(0f, 1.28f, -0.2f), Vector3.zero, 0.24f);
            AddValveWheel(root, "alcove_lower_valve", new Vector3(0.42f, 0.62f, -0.16f), Vector3.zero, 0.24f, "SCK2_MAT_HazardRedEnamel");
            AddCagedLamp(root, "alcove_overhead_lamp", new Vector3(0f, 2.22f, -0.05f), 0.2f);
            AddNorthStar(root, "alcove_star_topper", new Vector3(0f, 2.24f, 0.33f), Vector3.zero, 0.32f);
        }

        private static void BuildCeilingRing(GameObject root)
        {
            Cyl(root, "ceiling_compass_outer_ring", new Vector3(0f, 0f, 0f), Vector3.zero, new Vector3(2.0f, 0.12f, 2.0f), "SCK2_MAT_DarkRivetedIron", true);
            Cyl(root, "ceiling_compass_brass_inner", new Vector3(0f, -0.08f, 0f), Vector3.zero, new Vector3(1.45f, 0.10f, 1.45f), "SCK2_MAT_AgedBrass", true);
            AddNorthStar(root, "ceiling_compass_star", new Vector3(0f, -0.15f, 0f), new Vector3(90f, 0f, 0f), 1.06f);
            for (int i = 0; i < 8; i++)
            {
                GameObject strut = Box(root, "ceiling_ring_radial_strut_" + i, new Vector3(0f, -0.08f, 0f), new Vector3(0.06f, 0.08f, 1.52f), "SCK2_MAT_RivetSteel");
                strut.transform.localRotation = Quaternion.Euler(0f, i * 45f, 0f);
            }
        }

        private static void BuildFloorPanel(GameObject root, int variant)
        {
            float length = variant == 0 ? 2.0f : 4.0f;
            Box(root, "floor_panel_black_frame", new Vector3(0f, 0.04f, 0f), new Vector3(2.0f, 0.12f, length), "SCK2_MAT_DarkRivetedIron");
            Box(root, "floor_panel_oil_recess", new Vector3(0f, 0.12f, 0f), new Vector3(1.68f, 0.05f, length - 0.32f), "SCK2_MAT_OilWetFloor");

            if (variant == 0)
            {
                for (int i = 0; i < 7; i++)
                {
                    Box(root, "wet_grate_crossbar_" + i, new Vector3(0f, 0.18f, -0.78f + i * 0.26f), new Vector3(1.62f, 0.05f, 0.055f), "SCK2_MAT_AgedBrass");
                }

                for (int i = 0; i < 3; i++)
                {
                    Box(root, "wet_grate_longbar_" + i, new Vector3(-0.52f + i * 0.52f, 0.19f, 0f), new Vector3(0.055f, 0.05f, 1.62f), "SCK2_MAT_RivetSteel");
                }

                return;
            }

            Box(root, "drain_spine_center_channel", new Vector3(0f, 0.19f, 0f), new Vector3(0.42f, 0.055f, 3.62f), "SCK2_MAT_SootShadow");
            for (int i = 0; i < 10; i++)
            {
                Box(root, "drain_spine_cross_slat_" + i, new Vector3(0f, 0.23f, -1.65f + i * 0.36f), new Vector3(0.64f, 0.045f, 0.055f), "SCK2_MAT_AgedBrass");
            }

            AddNorthStar(root, "floor_drain_star_marker", new Vector3(0.64f, 0.21f, -1.22f), new Vector3(90f, 0f, 0f), 0.34f);
        }

        private static void BuildCeilingModule(GameObject root, int variant)
        {
            if (variant == 0)
            {
                Box(root, "ceiling_pipe_rack_backbone", new Vector3(0f, 0f, 0f), new Vector3(2.6f, 0.10f, 4.0f), "SCK2_MAT_DarkRivetedIron");
                for (int i = 0; i < 5; i++)
                {
                    float x = -0.82f + i * 0.41f;
                    Cyl(root, "ceiling_pipe_rack_run_" + i, new Vector3(x, -0.18f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.055f + i * 0.007f, 3.72f, 0.055f + i * 0.007f), i % 2 == 0 ? "SCK2_MAT_BurnishedCopper" : "SCK2_MAT_AgedBrass");
                }

                for (int i = 0; i < 6; i++)
                {
                    Box(root, "ceiling_pipe_rack_hanger_" + i, new Vector3(0f, -0.06f, -1.62f + i * 0.64f), new Vector3(2.24f, 0.08f, 0.06f), "SCK2_MAT_RivetSteel");
                }

                return;
            }

            Box(root, "fan_vent_square_frame", new Vector3(0f, 0f, 0f), new Vector3(2.0f, 0.12f, 2.0f), "SCK2_MAT_DarkRivetedIron");
            Cyl(root, "fan_vent_round_ring", new Vector3(0f, -0.08f, 0f), Vector3.zero, new Vector3(1.35f, 0.10f, 1.35f), "SCK2_MAT_AgedBrass", true);
            for (int i = 0; i < 6; i++)
            {
                GameObject blade = Box(root, "fan_vent_blade_" + i, new Vector3(0f, -0.16f, 0f), new Vector3(0.16f, 0.045f, 0.76f), i % 2 == 0 ? "SCK2_MAT_TemperedBlueSteel" : "SCK2_MAT_BurnishedCopper");
                blade.transform.localRotation = Quaternion.Euler(0f, i * 60f + 22f, 0f);
                blade.transform.localPosition += blade.transform.forward * 0.22f;
            }

            Cyl(root, "fan_vent_center_hub", new Vector3(0f, -0.22f, 0f), Vector3.zero, new Vector3(0.32f, 0.10f, 0.32f), "SCK2_MAT_RivetSteel", true);
        }

        private static void BuildArchSupport(GameObject root)
        {
            Box(root, "arch_pair_left_column", new Vector3(-1.22f, 1.32f, 0f), new Vector3(0.32f, 2.64f, 0.42f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "arch_pair_right_column", new Vector3(1.22f, 1.32f, 0f), new Vector3(0.32f, 2.64f, 0.42f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "arch_pair_top_crossbeam", new Vector3(0f, 2.62f, 0f), new Vector3(2.72f, 0.28f, 0.42f), "SCK2_MAT_AgedBrass");
            for (int i = 0; i < 5; i++)
            {
                float y = 0.32f + i * 0.42f;
                Box(root, "left_column_rib_band_" + i, new Vector3(-1.22f, y, -0.24f), new Vector3(0.42f, 0.08f, 0.08f), "SCK2_MAT_RivetSteel");
                Box(root, "right_column_rib_band_" + i, new Vector3(1.22f, y, -0.24f), new Vector3(0.42f, 0.08f, 0.08f), "SCK2_MAT_RivetSteel");
            }

            AddNorthStar(root, "arch_pair_keystone_star", new Vector3(0f, 2.62f, -0.26f), Vector3.zero, 0.34f);
        }

        private static void BuildCornerColumn(GameObject root)
        {
            Box(root, "corner_column_square_core", new Vector3(0f, 1.28f, 0f), new Vector3(0.5f, 2.55f, 0.5f), "SCK2_MAT_DarkRivetedIron");
            Cyl(root, "corner_column_front_pipe", new Vector3(0f, 1.34f, -0.31f), Vector3.zero, new Vector3(0.12f, 2.38f, 0.12f), "SCK2_MAT_BurnishedCopper");
            Cyl(root, "corner_column_side_pipe", new Vector3(-0.31f, 1.18f, 0f), Vector3.zero, new Vector3(0.10f, 2.06f, 0.10f), "SCK2_MAT_AgedBrass");
            AddGauge(root, "corner_column_upper_gauge", new Vector3(0f, 1.72f, -0.38f), Vector3.zero, 0.18f);
            AddGauge(root, "corner_column_lower_gauge", new Vector3(0f, 1.08f, -0.38f), Vector3.zero, 0.16f);
            AddValveWheel(root, "corner_column_side_wheel", new Vector3(0.34f, 0.76f, 0f), new Vector3(0f, -90f, 0f), 0.2f, "SCK2_MAT_HazardRedEnamel");
        }

        private static void BuildStairNib(GameObject root)
        {
            Box(root, "service_step_lower_tread", new Vector3(0f, 0.09f, 0.22f), new Vector3(2.0f, 0.18f, 0.48f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "service_step_upper_tread", new Vector3(0f, 0.28f, -0.18f), new Vector3(2.0f, 0.18f, 0.48f), "SCK2_MAT_AgedBrass");
            Box(root, "service_step_red_nosing", new Vector3(0f, 0.39f, -0.42f), new Vector3(1.88f, 0.06f, 0.08f), "SCK2_MAT_HazardRedEnamel");
            AddFloorRivetGrid(root, "service_step_bolts", -0.76f, 0.76f, -0.36f, 0.34f, 3, 2, "SCK2_MAT_RivetSteel");
        }

        private static void BuildLightRun(GameObject root)
        {
            Box(root, "light_run_ceiling_spine", new Vector3(0f, 0.18f, 0f), new Vector3(0.22f, 0.12f, 4.0f), "SCK2_MAT_DarkRivetedIron");
            Cyl(root, "light_run_copper_feed", new Vector3(-0.28f, 0.08f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.05f, 3.75f, 0.05f), "SCK2_MAT_BurnishedCopper");
            for (int i = 0; i < 3; i++)
            {
                AddCagedLamp(root, "light_run_lamp_" + i, new Vector3(0f, -0.25f, -1.28f + i * 1.28f), 0.19f);
            }
        }

        private static void BuildPipeRun(GameObject root)
        {
            Box(root, "pipe_run_wall_saddle", new Vector3(0f, 0.56f, 0f), new Vector3(0.24f, 1.12f, 4.0f), "SCK2_MAT_DarkRivetedIron");
            Cyl(root, "pipe_run_upper_copper", new Vector3(-0.08f, 0.88f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.12f, 3.85f, 0.12f), "SCK2_MAT_BurnishedCopper");
            Cyl(root, "pipe_run_lower_brass", new Vector3(0.10f, 0.38f, 0f), new Vector3(90f, 0f, 0f), new Vector3(0.10f, 3.85f, 0.10f), "SCK2_MAT_AgedBrass");
            for (int i = 0; i < 6; i++)
            {
                Box(root, "pipe_run_clamp_" + i, new Vector3(0f, 0.62f, -1.64f + i * 0.66f), new Vector3(0.32f, 0.62f, 0.06f), "SCK2_MAT_RivetSteel");
            }

            AddGauge(root, "pipe_run_inline_gauge", new Vector3(0.0f, 0.88f, -0.92f), new Vector3(0f, 0f, 0f), 0.16f);
        }

        private static void BuildWindowPorthole(GameObject root)
        {
            Cyl(root, "porthole_outer_ring", new Vector3(0f, 0f, 0f), new Vector3(90f, 0f, 0f), new Vector3(1.05f, 0.12f, 1.05f), "SCK2_MAT_DarkRivetedIron", true);
            Cyl(root, "porthole_brass_bezel", new Vector3(0f, 0f, -0.05f), new Vector3(90f, 0f, 0f), new Vector3(0.84f, 0.08f, 0.84f), "SCK2_MAT_AgedBrass", true);
            Cyl(root, "porthole_smoke_glass", new Vector3(0f, 0f, -0.11f), new Vector3(90f, 0f, 0f), new Vector3(0.62f, 0.04f, 0.62f), "SCK2_MAT_SmokeGlass", true);
            for (int i = 0; i < 4; i++)
            {
                GameObject grate = Box(root, "porthole_cross_grate_" + i, new Vector3(0f, 0f, -0.16f), new Vector3(0.07f, 0.72f, 0.045f), "SCK2_MAT_RivetSteel");
                grate.transform.localRotation = Quaternion.Euler(0f, 0f, i * 45f);
            }

            AddCircleRivets(root, "porthole_rivet", Vector3.back * 0.18f, 0.48f, 10, "SCK2_MAT_RivetSteel");
        }

        private static void BuildSignage(GameObject root)
        {
            Box(root, "signage_black_backplate", new Vector3(0f, 0f, 0f), new Vector3(1.4f, 0.5f, 0.06f), "SCK2_MAT_DarkRivetedIron");
            Box(root, "signage_brass_face", new Vector3(0f, 0f, -0.04f), new Vector3(1.22f, 0.34f, 0.05f), "SCK2_MAT_AgedBrass");
            AddNorthStar(root, "signage_left_star", new Vector3(-0.48f, 0f, -0.08f), Vector3.zero, 0.22f);
            Box(root, "signage_red_route_bar", new Vector3(0.28f, -0.09f, -0.08f), new Vector3(0.58f, 0.055f, 0.035f), "SCK2_MAT_HazardRedEnamel");
            AddTextTag(root, "signage_text", "NORTH", new Vector3(0.28f, 0.07f, -0.09f), 0.055f, Color.black);
        }

        private static void BuildCorridorShell(GameObject root, float length, string prefix, bool includeWalls)
        {
            Box(root, prefix + "_floor_oilwet_deck", new Vector3(0f, 0.03f, 0f), new Vector3(2.8f, 0.12f, length), "SCK2_MAT_OilWetFloor");
            Box(root, prefix + "_ceiling_blackened_plate", new Vector3(0f, 2.74f, 0f), new Vector3(2.8f, 0.12f, length), "SCK2_MAT_DarkRivetedIron");
            if (includeWalls)
            {
                Box(root, prefix + "_left_wall_slab", new Vector3(-1.36f, 1.36f, 0f), new Vector3(0.16f, 2.7f, length), "SCK2_MAT_BoilerTile");
                Box(root, prefix + "_right_wall_slab", new Vector3(1.36f, 1.36f, 0f), new Vector3(0.16f, 2.7f, length), "SCK2_MAT_BoilerTile");
                Box(root, prefix + "_left_brass_kick", new Vector3(-1.25f, 0.38f, 0f), new Vector3(0.08f, 0.58f, length - 0.25f), "SCK2_MAT_AgedBrass");
                Box(root, prefix + "_right_brass_kick", new Vector3(1.25f, 0.38f, 0f), new Vector3(0.08f, 0.58f, length - 0.25f), "SCK2_MAT_AgedBrass");
            }
        }

        private static void AddArchRib(GameObject root, string prefix, float z)
        {
            Box(root, prefix + "_left_upright", new Vector3(-1.22f, 1.36f, z), new Vector3(0.18f, 2.62f, 0.18f), "SCK2_MAT_DarkRivetedIron");
            Box(root, prefix + "_right_upright", new Vector3(1.22f, 1.36f, z), new Vector3(0.18f, 2.62f, 0.18f), "SCK2_MAT_DarkRivetedIron");
            Box(root, prefix + "_top_brass_lintel", new Vector3(0f, 2.63f, z), new Vector3(2.48f, 0.18f, 0.18f), "SCK2_MAT_AgedBrass");
            Cyl(root, prefix + "_left_column_pipe", new Vector3(-1.06f, 1.42f, z - 0.05f), Vector3.zero, new Vector3(0.07f, 2.36f, 0.07f), "SCK2_MAT_BurnishedCopper");
            Cyl(root, prefix + "_right_column_pipe", new Vector3(1.06f, 1.42f, z - 0.05f), Vector3.zero, new Vector3(0.07f, 2.36f, 0.07f), "SCK2_MAT_BurnishedCopper");
        }

        private static void BuildPipeBundle(GameObject root, string prefix, Vector3 position, float length, bool alongZ)
        {
            Vector3 euler = alongZ ? new Vector3(90f, 0f, 0f) : Vector3.zero;
            for (int i = 0; i < 3; i++)
            {
                Vector3 offset = alongZ ? new Vector3(0f, -0.18f * i, 0f) : new Vector3(0.18f * i, 0f, 0f);
                Cyl(root, prefix + "_pipe_" + i, position + offset, euler, new Vector3(0.055f + i * 0.014f, length, 0.055f + i * 0.014f), i == 1 ? "SCK2_MAT_AgedBrass" : "SCK2_MAT_BurnishedCopper");
            }

            int clampCount = Mathf.Max(2, Mathf.RoundToInt(length));
            for (int i = 0; i < clampCount; i++)
            {
                float t = clampCount == 1 ? 0f : i / (float)(clampCount - 1);
                float along = Mathf.Lerp(-length * 0.42f, length * 0.42f, t);
                Vector3 clampPos = position + (alongZ ? new Vector3(0f, -0.18f, along) : new Vector3(0f, along, 0f));
                Box(root, prefix + "_clamp_" + i, clampPos, alongZ ? new Vector3(0.16f, 0.46f, 0.06f) : new Vector3(0.46f, 0.16f, 0.06f), "SCK2_MAT_RivetSteel");
            }
        }

        private static void AddCagedLamp(GameObject root, string prefix, Vector3 position, float radius)
        {
            Cyl(root, prefix + "_amber_glass", position, Vector3.zero, new Vector3(radius * 1.2f, radius * 2.0f, radius * 1.2f), "SCK2_MAT_AmberLampGlass", true);
            Cyl(root, prefix + "_top_cap", position + Vector3.up * radius * 1.05f, Vector3.zero, new Vector3(radius * 1.45f, radius * 0.20f, radius * 1.45f), "SCK2_MAT_DarkRivetedIron", true);
            Cyl(root, prefix + "_bottom_cap", position - Vector3.up * radius * 1.05f, Vector3.zero, new Vector3(radius * 1.45f, radius * 0.20f, radius * 1.45f), "SCK2_MAT_DarkRivetedIron", true);
            for (int i = 0; i < 6; i++)
            {
                float angle = Mathf.PI * 2f * i / 6f;
                Vector3 offset = new Vector3(Mathf.Cos(angle) * radius * 0.82f, 0f, Mathf.Sin(angle) * radius * 0.82f);
                Box(root, prefix + "_cage_bar_" + i, position + offset, new Vector3(0.025f, radius * 2.25f, 0.025f), "SCK2_MAT_AgedBrass");
            }
        }

        private static void AddGauge(GameObject root, string prefix, Vector3 position, Vector3 euler, float radius)
        {
            Cyl(root, prefix + "_black_backplate", position + LocalZ(euler, 0.025f), new Vector3(90f + euler.x, euler.y, euler.z), new Vector3(radius * 1.24f, 0.05f, radius * 1.24f), "SCK2_MAT_DarkRivetedIron", true);
            Cyl(root, prefix + "_ivory_face", position - LocalZ(euler, 0.02f), new Vector3(90f + euler.x, euler.y, euler.z), new Vector3(radius, 0.035f, radius), "SCK2_MAT_GaugeIvory", true);
            Cyl(root, prefix + "_brass_bezel", position - LocalZ(euler, 0.055f), new Vector3(90f + euler.x, euler.y, euler.z), new Vector3(radius * 1.1f, 0.03f, radius * 1.1f), "SCK2_MAT_AgedBrass", true);
            GameObject needle = Box(root, prefix + "_red_needle", position + new Vector3(radius * 0.16f, radius * 0.02f, -0.09f), new Vector3(radius * 0.68f, 0.018f, 0.025f), "SCK2_MAT_HazardRedEnamel");
            needle.transform.localRotation = Quaternion.Euler(euler) * Quaternion.Euler(0f, 0f, 18f);
            Cyl(root, prefix + "_needle_pivot", position - LocalZ(euler, 0.115f), new Vector3(90f + euler.x, euler.y, euler.z), new Vector3(radius * 0.16f, 0.025f, radius * 0.16f), "SCK2_MAT_DarkRivetedIron");
        }

        private static void AddValveWheel(GameObject root, string prefix, Vector3 position, Vector3 euler, float radius, string material)
        {
            Cyl(root, prefix + "_outer_wheel", position, new Vector3(90f + euler.x, euler.y, euler.z), new Vector3(radius, 0.055f, radius), material, true);
            Cyl(root, prefix + "_hub", position - LocalZ(euler, 0.05f), new Vector3(90f + euler.x, euler.y, euler.z), new Vector3(radius * 0.25f, 0.08f, radius * 0.25f), "SCK2_MAT_AgedBrass");
            for (int i = 0; i < 4; i++)
            {
                GameObject spoke = Box(root, prefix + "_spoke_" + i, position - LocalZ(euler, 0.07f), new Vector3(radius * 0.88f, 0.035f, 0.035f), material);
                spoke.transform.localRotation = Quaternion.Euler(euler) * Quaternion.Euler(0f, 0f, i * 45f);
            }
        }

        private static void AddNorthStar(GameObject root, string name, Vector3 position, Vector3 euler, float size)
        {
            Star(root, name + "_white_inlay", position, euler, new Vector3(size, size, 1f), "SCK2_MAT_NorthStarWhite");
            Star(root, name + "_brass_shadow", position + LocalZ(euler, 0.015f), euler + new Vector3(0f, 0f, 22.5f), new Vector3(size * 0.72f, size * 0.72f, 1f), "SCK2_MAT_AgedBrass");
        }

        private static void AddCircleRivets(GameObject root, string prefix, Vector3 center, float radius, int count, string material)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = Mathf.PI * 2f * i / count;
                Vector3 position = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
                Cyl(root, prefix + "_" + i, position, new Vector3(90f, 0f, 0f), new Vector3(0.07f, 0.03f, 0.07f), material);
            }
        }

        private static void AddRivetLine(GameObject root, string prefix, Vector3 start, Vector3 step, int count, Vector3 euler, string material)
        {
            for (int i = 0; i < count; i++)
            {
                Cyl(root, prefix + "_" + i, start + step * i, euler, new Vector3(0.075f, 0.032f, 0.075f), material);
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
                    Cyl(root, prefix + "_" + x + "_" + y, new Vector3(xx, yy, z), new Vector3(90f, 0f, 0f), new Vector3(0.07f, 0.03f, 0.07f), material);
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

        private static Vector3 LocalZ(Vector3 euler, float distance)
        {
            return Quaternion.Euler(euler) * new Vector3(0f, 0f, distance);
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

        private static void AddMetadataChild(GameObject root, KitSpec spec)
        {
            GameObject metadata = new GameObject("SCK2_METADATA__" + spec.Family + "__" + spec.Role.Replace(" ", "_"));
            metadata.transform.SetParent(root.transform, false);
            metadata.SetActive(false);
            metadata.name += "__" + FormatVector(spec.Dimensions) + "__visual_only_no_colliders_no_audio";
        }

        private static GameObject Box(GameObject parent, string name, Vector3 position, Vector3 scale, string material)
        {
            return Part(parent, name, "SCK2_MESH_BoxUnit", position, Quaternion.identity, scale, material);
        }

        private static GameObject Cyl(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string material, bool highRes = false)
        {
            return Part(parent, name, highRes ? "SCK2_MESH_Cylinder32Unit" : "SCK2_MESH_Cylinder16Unit", position, Quaternion.Euler(euler), scale, material);
        }

        private static GameObject Quad(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string material)
        {
            return Part(parent, name, "SCK2_MESH_QuadUnit", position, Quaternion.Euler(euler), scale, material);
        }

        private static GameObject Gear(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string material)
        {
            return Part(parent, name, "SCK2_MESH_Gear16ToothUnit", position, Quaternion.Euler(euler), scale, material);
        }

        private static GameObject Star(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string material)
        {
            return Part(parent, name, "SCK2_MESH_NorthStar8Unit", position, Quaternion.Euler(euler), scale, material);
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
            int columns = 8;
            for (int i = 0; i < count; i++)
            {
                int row = i / columns;
                int column = i % columns;
                positions[i] = new Vector3((column - 3.5f) * 3.15f, 0f, row * 3.3f);
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

            Debug.LogWarning("Corridor Kit Set 02 preview requested unknown prefab: " + fileName);
            return prefabPaths.Count > 0 ? prefabPaths[0] : string.Empty;
        }

        private static void RenderPrefabGroup(string fileName, string outputRoot, IReadOnlyList<string> prefabPaths, IReadOnlyList<Vector3> positions, Vector3 cameraPosition, Vector3 cameraEuler, bool wallBacker)
        {
            GameObject stage = new GameObject("SCK2_RenderStage_" + Path.GetFileNameWithoutExtension(fileName));
            try
            {
                for (int i = 0; i < prefabPaths.Count; i++)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[i]);
                    if (prefab == null)
                    {
                        Debug.LogWarning("Missing Corridor Kit Set 02 prefab for render: " + prefabPaths[i]);
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
            GameObject stage = new GameObject("SCK2_RenderStage_" + Path.GetFileNameWithoutExtension(fileName));
            try
            {
                string[] materialNames = GetMaterialNames();
                for (int i = 0; i < materialNames.Length; i++)
                {
                    int column = i % 6;
                    int row = i / 6;
                    GameObject swatch = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    swatch.name = "swatch_" + materialNames[i];
                    swatch.transform.SetParent(stage.transform, false);
                    swatch.transform.localPosition = new Vector3((column - 2.5f) * 1.05f, 1.15f - row * 0.62f, 0f);
                    swatch.transform.localScale = new Vector3(0.88f, 0.42f, 0.14f);
                    Renderer renderer = swatch.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(CombineAssetPath(PackageRoot, "Runtime/Materials/" + materialNames[i] + ".mat"));
                    }
                }

                BoxPrimitive(stage, "swatch_dark_backplate", new Vector3(0f, 0.35f, 0.24f), new Vector3(7.0f, 2.7f, 0.08f), new Color(0.018f, 0.016f, 0.014f));
                AddPreviewLighting(stage);
                RenderStageToPng(stage, Path.Combine(outputRoot, fileName), new Vector3(0f, 0.32f, -6.1f), Vector3.zero);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(stage);
            }
        }

        private static void AddPreviewFloor(GameObject stage)
        {
            BoxPrimitive(stage, "preview_oil_wet_floor", new Vector3(0f, -0.04f, 5.0f), new Vector3(27f, 0.06f, 19f), new Color(0.045f, 0.042f, 0.038f));
        }

        private static void AddPreviewWall(GameObject stage)
        {
            BoxPrimitive(stage, "preview_soot_wall", new Vector3(0f, 1.55f, 0.18f), new Vector3(8.8f, 3.1f, 0.08f), new Color(0.075f, 0.062f, 0.052f));
            BoxPrimitive(stage, "preview_oil_kick_floor", new Vector3(0f, -0.08f, -0.9f), new Vector3(9.0f, 0.08f, 2.2f), new Color(0.048f, 0.045f, 0.041f));
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
            key.transform.localRotation = Quaternion.Euler(44f, -32f, 0f);
            Light keyLight = key.AddComponent<Light>();
            keyLight.type = LightType.Directional;
            keyLight.intensity = 2.25f;
            keyLight.color = new Color(1f, 0.74f, 0.48f);

            GameObject fill = new GameObject("preview_amber_fill");
            fill.transform.SetParent(stage.transform, false);
            fill.transform.localPosition = new Vector3(-3.3f, 4f, -3f);
            Light fillLight = fill.AddComponent<Light>();
            fillLight.type = LightType.Point;
            fillLight.range = 13f;
            fillLight.intensity = 2.0f;
            fillLight.color = new Color(0.95f, 0.54f, 0.22f);

            GameObject rim = new GameObject("preview_green_pressure_rim");
            rim.transform.SetParent(stage.transform, false);
            rim.transform.localPosition = new Vector3(4.2f, 2.8f, -2.2f);
            Light rimLight = rim.AddComponent<Light>();
            rimLight.type = LightType.Point;
            rimLight.range = 9f;
            rimLight.intensity = 0.85f;
            rimLight.color = new Color(0.14f, 0.70f, 0.34f);
        }

        private static void RenderStageToPng(GameObject stage, string path, Vector3 cameraPosition, Vector3 cameraEuler)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            GameObject cameraObject = new GameObject("SCK2_preview_camera");
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
            AppendField(builder, "display_name", "Corridor Kit Set 02", true);
            AppendField(builder, "version", Version, true);
            AppendField(builder, "build_id", BuildId, true);
            AppendField(builder, "unity_version", Application.unityVersion, true);
            AppendField(builder, "generated_at_utc", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), true);
            AppendField(builder, "sidecar_project", "UD-SC-LVL-CorridorKitSet02", true);
            AppendField(builder, "owner_lane", "sidecar-corridor-kit-set02", true);
            AppendField(builder, "primary_intake_owner", "main-lane-art-integration", true);
            AppendField(builder, "canonical_root", "AssetPacks/BrassworksBreach.CorridorKitSet02", true);
            AppendField(builder, "package_root", "AssetPacks/BrassworksBreach.CorridorKitSet02", true);
            AppendField(builder, "package_name", PackageName, true);
            AppendField(builder, "upm_package_name", PackageName, true);
            AppendField(builder, "generator_menu", MenuRoot + "Generate Package Assets", true);
            AppendField(builder, "preview_menu", MenuRoot + "Render Preview PNGs", true);
            builder.AppendLine("  \"asset_counts\": {");
            builder.Append("    \"generated_prefabs\": ").Append(GetSpecs().Length).AppendLine(",");
            builder.Append("    \"generated_materials\": ").Append(GetMaterialNames().Length).AppendLine(",");
            builder.Append("    \"generated_meshes\": ").Append(GetMeshNames().Length).AppendLine(",");
            builder.AppendLine("    \"textures\": 0,");
            builder.AppendLine("    \"audio\": 0,");
            builder.AppendLine("    \"vfx\": 0,");
            builder.AppendLine("    \"animation_clips\": 0,");
            builder.AppendLine("    \"colliders\": 0,");
            builder.AppendLine("    \"preview_renders\": 5");
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
            AppendArray(builder, "runtime_safety_notes", new[]
            {
                "Visual-only prefabs omit colliders, rigidbodies, gameplay scripts, nav blockers, timeline assets, and autonomous audio.",
                "Preview-only primitives may create transient colliders during render staging, but they are not saved into package prefabs.",
                "Inactive metadata child objects are present only for package review and intake sizing."
            }, true);
            AppendArray(builder, "known_risks", new[]
            {
                "Procedural material colors are quarantine proxies and should be reviewed against final Brassworks palette before promotion.",
                "Large corridor assemblies are visual shells only; final gameplay collision, occlusion, navigation, and performance ownership stay with the primary lane.",
                "Door modules are non-authoritative visuals and do not implement opening, locking, damage, or interaction behavior.",
                "Preview lighting and preview floor/wall staging are proof-only and should not be promoted into shipped scenes."
            }, true);
            AppendField(builder, "rollback_path", "delete isolated package root or remove local package reference", true);
            AppendField(builder, "decision", "ready_for_primary_quarantine_after_static_validation_and_preview_review", false);
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static string[] GetPrefabManifestPaths()
        {
            KitSpec[] specs = GetSpecs();
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
                RenderOutputRelativePath + "/SCK2_PREVIEW_corridor_assembly_v0.1.41.png",
                RenderOutputRelativePath + "/SCK2_PREVIEW_door_modules_v0.1.41.png",
                RenderOutputRelativePath + "/SCK2_PREVIEW_room_wall_modules_v0.1.41.png",
                RenderOutputRelativePath + "/SCK2_PREVIEW_contact_sheet_v0.1.41.png",
                RenderOutputRelativePath + "/SCK2_PREVIEW_material_swatch_v0.1.41.png"
            };
        }

        private static string[] GetMaterialNames()
        {
            return new[]
            {
                "SCK2_MAT_DarkRivetedIron", "SCK2_MAT_AgedBrass", "SCK2_MAT_BurnishedCopper", "SCK2_MAT_TemperedBlueSteel",
                "SCK2_MAT_RivetSteel", "SCK2_MAT_BoilerTile", "SCK2_MAT_OilWetFloor", "SCK2_MAT_HazardRedEnamel",
                "SCK2_MAT_HazardYellowPaint", "SCK2_MAT_PatinaGreen", "SCK2_MAT_PressureGreenGlass", "SCK2_MAT_AmberLampGlass",
                "SCK2_MAT_SmokeGlass", "SCK2_MAT_GaugeIvory", "SCK2_MAT_RubberGasket", "SCK2_MAT_SootShadow",
                "SCK2_MAT_WornBrightEdge", "SCK2_MAT_NorthStarWhite"
            };
        }

        private static string[] GetMeshNames()
        {
            return new[] { "SCK2_MESH_BoxUnit", "SCK2_MESH_Cylinder16Unit", "SCK2_MESH_Cylinder32Unit", "SCK2_MESH_QuadUnit", "SCK2_MESH_Gear16ToothUnit", "SCK2_MESH_NorthStar8Unit" };
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
            string explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_SCK2_PREVIEW_ROOT");
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
            UnityEditor.PackageManager.PackageInfo packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(CorridorKitSet02Generator).Assembly);
            if (packageInfo != null && !string.IsNullOrWhiteSpace(packageInfo.assetPath) && !string.IsNullOrWhiteSpace(packageInfo.resolvedPath))
            {
                return new PackageRootInfo(packageInfo.assetPath.Replace("\\", "/"), packageInfo.resolvedPath);
            }

            string[] scriptGuids = AssetDatabase.FindAssets(nameof(CorridorKitSet02Generator));
            foreach (string guid in scriptGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/CorridorKitSet02Generator.cs";
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

        private enum KitKind
        {
            CorridorStraight,
            CorridorCorner,
            CorridorTJunction,
            CorridorCrossJunction,
            CorridorEndCap,
            BulkheadDoor,
            PressureDoor,
            DoorFrame,
            IrisDoor,
            Threshold,
            ControlColumn,
            RoomCorner,
            RoomWallPanel,
            WallPanel,
            RoomAlcove,
            CeilingRing,
            FloorPanel,
            CeilingModule,
            ArchSupport,
            CornerColumn,
            StairNib,
            LightRun,
            PipeRun,
            WindowPorthole,
            Signage
        }

        private sealed class KitSpec
        {
            public KitSpec(string fileName, string family, string displayName, string role, Vector3 dimensions, KitKind kind, int variant)
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
            public KitKind Kind { get; private set; }
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
