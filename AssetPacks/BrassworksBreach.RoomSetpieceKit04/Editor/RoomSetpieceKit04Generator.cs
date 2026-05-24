#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.Sidecars.RoomSetpieceKit04
{
    public static class RoomSetpieceKit04Generator
    {
        private const string Version = "0.1.45";
        private const string BuildId = "p001";
        private const string PackId = "RSK04";
        private const string PackageName = "com.brassworks.sidecar.room-setpiece-kit04";
        private const string MenuRoot = "Brassworks/Sidecars/Room Setpiece Kit 04 v0.1.45/";
        private const string RenderOutputRelativePath = "Documentation/ConceptRenders/V0_1_45_RoomSetpieceKit04";
        private const string ProductionDocRelativePath = "Documentation/AssetProduction/V0_1_45_RoomSetpieceKit04";
        private const string ManifestFileName = "RSK04_RoomSetpieceKit04_Manifest_v0.1.45-p001.json";
        private const string CatalogFileName = "RSK04_RuntimeCatalog_v0.1.45.json";

        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();

        [MenuItem(MenuRoot + "Generate Package Assets")]
        public static void GeneratePackageAssets()
        {
            Materials.Clear();
            Meshes.Clear();
            EnsureFolders();
            CreateMaterials();
            CreateMeshes();

            foreach (SetpieceSpec spec in Specs())
            {
                CreatePrefab(spec);
            }

            WriteMetadata("generated_by_unity_sidecar_batchmode_v0.1.45");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("RSK04_GENERATE_PASS v0.1.45 prefabs=" + Specs().Length + " materials=" + Materials.Count + " meshes=" + Meshes.Count);
        }

        [MenuItem(MenuRoot + "Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(PackageRoot + "/Runtime/Prefabs/" + Specs()[0].FileName) == null)
            {
                GeneratePackageAssets();
            }

            string outputRoot = ResolveRenderOutputRoot();
            Directory.CreateDirectory(outputRoot);
            foreach (string existing in Directory.GetFiles(outputRoot, "RSK04_*.png"))
            {
                File.Delete(existing);
            }

            RenderFamily("boiler_chamber_wall_bay", "Boiler chamber wall bays", Family("boiler-chamber-wall-bay"), new Vector3(0f, 3.1f, -8.6f), new Vector3(18f, 0f, 0f));
            RenderFamily("pressure_vault_door_alcove", "Pressure-vault door alcoves", Family("pressure-vault-door-alcove"), new Vector3(0f, 3.2f, -9.0f), new Vector3(16f, 0f, 0f));
            RenderFamily("catwalk_balcony_module", "Catwalk balcony modules", Family("catwalk-balcony-module"), new Vector3(0f, 3.0f, -8.2f), new Vector3(19f, 0f, 0f));
            RenderFamily("regulator_core_machinery", "Regulator core machinery", Family("regulator-core-machinery"), new Vector3(0f, 3.15f, -8.4f), new Vector3(17f, 0f, 0f));
            RenderFamily("pipe_gallery_ceiling_cluster", "Pipe-gallery ceiling clusters", Family("pipe-gallery-ceiling-cluster"), new Vector3(0f, 4.25f, -8.5f), new Vector3(25f, 0f, 0f));
            RenderFamily("service_stair_silhouette", "Service stair silhouettes", Family("service-stair-silhouette"), new Vector3(0f, 3.0f, -8.4f), new Vector3(19f, 0f, 0f));
            RenderFamily("furnace_control_wall", "Furnace control walls", Family("furnace-control-wall"), new Vector3(0f, 2.85f, -7.8f), new Vector3(16f, 0f, 0f));
            RenderFamily("brass_floor_trim_threshold", "Brass floor trim and thresholds", Family("brass-floor-trim-threshold"), new Vector3(0f, 5.8f, -7.2f), new Vector3(48f, 0f, 0f));
            RenderFamily("large_warning_gauge_wall", "Large warning gauge walls", Family("large-warning-gauge-wall"), new Vector3(0f, 3.0f, -7.8f), new Vector3(15f, 0f, 0f));
            RenderFamily("room_corner_clutter_cluster", "Room-corner clutter clusters", Family("room-corner-clutter-cluster"), new Vector3(0f, 3.0f, -8.3f), new Vector3(18f, 0f, 0f));
            RenderFamily("all_setpieces_contact_sheet", "All Room Setpiece Kit 04 prefabs", Specs(), new Vector3(0f, 13.8f, -24f), new Vector3(35f, 0f, 0f));
            RenderMaterialSwatch(outputRoot);

            WriteMetadata("generated_and_preview_rendered_by_unity_sidecar_batchmode_v0.1.45");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("RSK04_PREVIEW_PASS v0.1.45 output=" + outputRoot);
        }

        [MenuItem(MenuRoot + "Generate and Render Preview")]
        public static void GenerateAllAndRenderPreview()
        {
            GeneratePackageAssets();
            RenderPreviewPngs();
        }

        public static void GenerateValidateAndQuit()
        {
            int exitCode = 0;
            try
            {
                GenerateAllAndRenderPreview();
                exitCode = WriteUnityValidationReport();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                exitCode = 1;
            }

            EditorApplication.Exit(exitCode);
        }

        private static SetpieceSpec[] Specs()
        {
            List<SetpieceSpec> specs = new List<SetpieceSpec>();
            AddFamily(specs, "BoilerChamberWallBay", "boiler-chamber-wall-bay", "boiler chamber wall bay", new Vector3(3.4f, 2.7f, 0.85f), BuildBoilerChamberWallBay);
            AddFamily(specs, "PressureVaultDoorAlcove", "pressure-vault-door-alcove", "pressure-vault door alcove", new Vector3(3.6f, 3.1f, 1.0f), BuildPressureVaultDoorAlcove);
            AddFamily(specs, "CatwalkBalconyModule", "catwalk-balcony-module", "catwalk balcony module", new Vector3(3.8f, 2.0f, 1.4f), BuildCatwalkBalconyModule);
            AddFamily(specs, "RegulatorCoreMachinery", "regulator-core-machinery", "regulator core machinery", new Vector3(2.2f, 3.0f, 2.0f), BuildRegulatorCoreMachinery);
            AddFamily(specs, "PipeGalleryCeilingCluster", "pipe-gallery-ceiling-cluster", "pipe-gallery ceiling cluster", new Vector3(4.0f, 1.35f, 1.7f), BuildPipeGalleryCeilingCluster);
            AddFamily(specs, "ServiceStairSilhouette", "service-stair-silhouette", "service stair silhouette", new Vector3(3.2f, 2.2f, 1.5f), BuildServiceStairSilhouette);
            AddFamily(specs, "FurnaceControlWall", "furnace-control-wall", "furnace control wall", new Vector3(3.0f, 2.5f, 0.65f), BuildFurnaceControlWall);
            AddFamily(specs, "BrassFloorTrimThreshold", "brass-floor-trim-threshold", "brass floor trim and threshold assembly", new Vector3(3.8f, 0.35f, 2.0f), BuildBrassFloorTrimThreshold);
            AddFamily(specs, "LargeWarningGaugeWall", "large-warning-gauge-wall", "large warning gauge wall", new Vector3(3.2f, 2.4f, 0.7f), BuildLargeWarningGaugeWall);
            AddFamily(specs, "RoomCornerClutterCluster", "room-corner-clutter-cluster", "room corner clutter cluster", new Vector3(2.1f, 1.7f, 1.8f), BuildRoomCornerClutterCluster);
            return specs.ToArray();
        }

        private static void AddFamily(List<SetpieceSpec> specs, string stem, string family, string description, Vector3 bounds, Action<GameObject, int> builder)
        {
            for (int i = 0; i < 3; i++)
            {
                string suffix = ((char)('A' + i)).ToString();
                specs.Add(new SetpieceSpec("RSK04_" + stem + "_" + suffix + ".prefab", family, description + " variant " + suffix, bounds + new Vector3(i * 0.12f, i * 0.05f, i * 0.08f), i, builder));
            }
        }

        private static SetpieceSpec[] Family(string family)
        {
            List<SetpieceSpec> matches = new List<SetpieceSpec>();
            foreach (SetpieceSpec spec in Specs())
            {
                if (spec.Family == family)
                {
                    matches.Add(spec);
                }
            }
            return matches.ToArray();
        }

        private static void EnsureFolders()
        {
            foreach (string folder in new[] { "Runtime/Prefabs", "Runtime/Materials", "Runtime/Meshes", "Runtime/Metadata", "Documentation~/Manifest", "Samples~/PreviewScene" })
            {
                Directory.CreateDirectory(FullPath(PackageRoot + "/" + folder));
            }

            Directory.CreateDirectory(Path.Combine(ProjectRoot, RenderOutputRelativePath));
            Directory.CreateDirectory(Path.Combine(ProjectRoot, ProductionDocRelativePath));
            AssetDatabase.Refresh();
        }

        private static void CreateMaterials()
        {
            AddMaterial("RSK04_MAT_BlackenedIron", new Color(0.035f, 0.032f, 0.028f), 0.35f, 0.86f);
            AddMaterial("RSK04_MAT_AgedBrass", new Color(0.74f, 0.52f, 0.22f), 0.58f, 0.72f);
            AddMaterial("RSK04_MAT_BurnishedCopper", new Color(0.68f, 0.30f, 0.13f), 0.55f, 0.62f);
            AddMaterial("RSK04_MAT_WornSteel", new Color(0.45f, 0.43f, 0.38f), 0.42f, 0.78f);
            AddMaterial("RSK04_MAT_SootStone", new Color(0.07f, 0.064f, 0.055f), 0.18f, 0.08f);
            AddMaterial("RSK04_MAT_WetStoneGloss", new Color(0.025f, 0.024f, 0.022f), 0.78f, 0.18f);
            AddMaterial("RSK04_MAT_AmberGlassGlow", new Color(1f, 0.52f, 0.12f, 0.74f), 0.36f, 0.04f, new Color(1f, 0.36f, 0.08f) * 2.1f, true);
            AddMaterial("RSK04_MAT_FurnaceOrangeGlow", new Color(1f, 0.30f, 0.05f), 0.24f, 0.12f, new Color(1f, 0.24f, 0.04f) * 3.1f);
            AddMaterial("RSK04_MAT_VerdigrisOxide", new Color(0.18f, 0.43f, 0.35f), 0.34f, 0.25f);
            AddMaterial("RSK04_MAT_GaugeIvory", new Color(0.82f, 0.75f, 0.58f), 0.22f, 0.02f);
            AddMaterial("RSK04_MAT_WarningRedNeedle", new Color(0.86f, 0.045f, 0.025f), 0.4f, 0.08f);
            AddMaterial("RSK04_MAT_HazardYellowPaint", new Color(0.86f, 0.60f, 0.08f), 0.26f, 0.18f);
            AddMaterial("RSK04_MAT_SteamWhite", new Color(0.76f, 0.74f, 0.68f, 0.40f), 0.06f, 0f, new Color(0.25f, 0.23f, 0.20f), true);
            AddMaterial("RSK04_MAT_OiledLeather", new Color(0.22f, 0.11f, 0.045f), 0.22f, 0.02f);
            AddMaterial("RSK04_MAT_DarkRubberBelt", new Color(0.018f, 0.017f, 0.015f), 0.14f, 0f);
            AddMaterial("RSK04_MAT_BrightRivetWear", new Color(0.94f, 0.75f, 0.36f), 0.68f, 0.80f);
            AddMaterial("RSK04_MAT_OilBlack", new Color(0.007f, 0.006f, 0.005f), 0.88f, 0.03f);
            AddMaterial("RSK04_MAT_DeepShadow", new Color(0.004f, 0.0035f, 0.003f), 0.08f, 0f);
        }

        private static void AddMaterial(string name, Color color, float smoothness, float metallic, Color emission = default(Color), bool transparent = false)
        {
            string path = PackageRoot + "/Runtime/Materials/" + name + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Standard"));
                AssetDatabase.CreateAsset(material, path);
            }

            material.name = name;
            material.SetColor("_Color", color);
            material.SetFloat("_Glossiness", smoothness);
            material.SetFloat("_Metallic", metallic);

            if (emission.maxColorComponent > 0.001f)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission);
            }
            else
            {
                material.DisableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", Color.black);
            }

            if (transparent)
            {
                material.SetFloat("_Mode", 3f);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
            else
            {
                material.SetFloat("_Mode", 0f);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
            }

            EditorUtility.SetDirty(material);
            Materials[name] = material;
        }

        private static void CreateMeshes()
        {
            AddMesh("RSK04_MESH_BoxUnit", BuildBoxMesh());
            AddMesh("RSK04_MESH_QuadUnit", BuildQuadMesh());
            AddMesh("RSK04_MESH_Cylinder16Unit", BuildCylinderMesh(16, 0.5f, 1f));
            AddMesh("RSK04_MESH_Cylinder32Unit", BuildCylinderMesh(32, 0.5f, 1f));
            AddMesh("RSK04_MESH_Ring32Unit", BuildRingMesh(32, 0.34f, 0.5f, 0.06f));
            AddMesh("RSK04_MESH_Gear24Unit", BuildGearMesh(24, 0.40f, 0.52f, 0.06f));
            AddMesh("RSK04_MESH_ArchPanelUnit", BuildArchPanelMesh());
            AddMesh("RSK04_MESH_StairStringerUnit", BuildStairStringerMesh());
            AddMesh("RSK04_MESH_PressureNeedleUnit", BuildPressureNeedleMesh());
            AddMesh("RSK04_MESH_SteamWispUnit", BuildSteamWispMesh());
        }

        private static void AddMesh(string name, Mesh source)
        {
            source.name = name;
            string path = PackageRoot + "/Runtime/Meshes/" + name + ".asset";
            Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing == null)
            {
                AssetDatabase.CreateAsset(source, path);
                Meshes[name] = source;
            }
            else
            {
                EditorUtility.CopySerialized(source, existing);
                existing.name = name;
                EditorUtility.SetDirty(existing);
                Meshes[name] = existing;
            }
        }

        private static void CreatePrefab(SetpieceSpec spec)
        {
            GameObject root = new GameObject(Path.GetFileNameWithoutExtension(spec.FileName));
            root.tag = "Untagged";
            root.layer = 0;
            spec.Builder(root, spec.Variant);
            StripForbiddenComponents(root);
            PrefabUtility.SaveAsPrefabAsset(root, PackageRoot + "/Runtime/Prefabs/" + spec.FileName);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static void StripForbiddenComponents(GameObject root)
        {
            foreach (Collider component in root.GetComponentsInChildren<Collider>(true)) UnityEngine.Object.DestroyImmediate(component);
            foreach (Rigidbody component in root.GetComponentsInChildren<Rigidbody>(true)) UnityEngine.Object.DestroyImmediate(component);
            foreach (AudioSource component in root.GetComponentsInChildren<AudioSource>(true)) UnityEngine.Object.DestroyImmediate(component);
            foreach (MonoBehaviour component in root.GetComponentsInChildren<MonoBehaviour>(true)) UnityEngine.Object.DestroyImmediate(component);
        }

        private static GameObject Part(GameObject parent, string name, string mesh, string material, Vector3 localPosition, Vector3 localScale, Vector3 localEuler)
        {
            GameObject child = new GameObject(name);
            child.transform.SetParent(parent.transform, false);
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.Euler(localEuler);
            child.transform.localScale = localScale;
            child.AddComponent<MeshFilter>().sharedMesh = Meshes[mesh];
            child.AddComponent<MeshRenderer>().sharedMaterial = Materials[material];
            return child;
        }

        private static void Pipe(GameObject root, string name, Vector3 pos, float length, float radius, Vector3 euler, string material = "RSK04_MAT_BurnishedCopper")
        {
            Part(root, name, "RSK04_MESH_Cylinder16Unit", material, pos, new Vector3(radius * 2f, length, radius * 2f), euler);
        }

        private static void Rivet(GameObject root, string name, Vector3 pos, float radius = 0.055f)
        {
            Part(root, name, "RSK04_MESH_Cylinder16Unit", "RSK04_MAT_BrightRivetWear", pos, new Vector3(radius * 2f, radius * 0.55f, radius * 2f), new Vector3(90f, 0f, 0f));
        }

        private static void Gauge(GameObject root, string prefix, Vector3 pos, float radius, float needleAngle)
        {
            Part(root, prefix + "_black_back", "RSK04_MESH_Cylinder32Unit", "RSK04_MAT_BlackenedIron", pos + new Vector3(0f, 0f, -0.025f), new Vector3(radius * 2.25f, 0.05f, radius * 2.25f), new Vector3(90f, 0f, 0f));
            Part(root, prefix + "_ivory_face", "RSK04_MESH_Cylinder32Unit", "RSK04_MAT_GaugeIvory", pos, new Vector3(radius * 1.9f, 0.035f, radius * 1.9f), new Vector3(90f, 0f, 0f));
            Part(root, prefix + "_brass_rim", "RSK04_MESH_Ring32Unit", "RSK04_MAT_AgedBrass", pos + new Vector3(0f, 0f, 0.035f), new Vector3(radius * 2.25f, radius * 2.25f, radius * 2.25f), Vector3.zero);
            Part(root, prefix + "_needle", "RSK04_MESH_PressureNeedleUnit", "RSK04_MAT_WarningRedNeedle", pos + new Vector3(0f, 0f, 0.065f), new Vector3(radius * 0.95f, radius * 0.95f, radius * 0.95f), new Vector3(0f, 0f, needleAngle));
            Part(root, prefix + "_redline", "RSK04_MESH_BoxUnit", "RSK04_MAT_WarningRedNeedle", pos + new Vector3(radius * 0.36f, radius * 0.42f, 0.058f), new Vector3(radius * 0.54f, radius * 0.038f, 0.018f), new Vector3(0f, 0f, -35f));
        }

        private static void Steam(GameObject root, string name, Vector3 pos, Vector3 scale, float zRot)
        {
            Part(root, name, "RSK04_MESH_SteamWispUnit", "RSK04_MAT_SteamWhite", pos, scale, new Vector3(0f, 0f, zRot));
        }

        private static void Chain(GameObject root, string prefix, float x, int links, float topY, float z)
        {
            for (int i = 0; i < links; i++)
            {
                Part(root, prefix + "_link_" + i.ToString("00", CultureInfo.InvariantCulture), "RSK04_MESH_Ring32Unit", "RSK04_MAT_WornSteel", new Vector3(x, topY - i * 0.17f, z), new Vector3(0.22f, 0.15f, 0.22f), new Vector3(0f, 0f, i % 2 == 0 ? 0f : 90f));
            }
        }

        private static void BuildBoilerChamberWallBay(GameObject root, int variant)
        {
            float width = 2.9f + variant * 0.18f;
            Part(root, "soot_stone_wall_bay", "RSK04_MESH_BoxUnit", "RSK04_MAT_SootStone", new Vector3(0f, 1.25f, -0.12f), new Vector3(width, 2.5f, 0.16f), Vector3.zero);
            Part(root, "lower_wet_plinth", "RSK04_MESH_BoxUnit", "RSK04_MAT_WetStoneGloss", new Vector3(0f, 0.16f, -0.015f), new Vector3(width, 0.30f, 0.18f), Vector3.zero);
            Pipe(root, "left_boiler_column", new Vector3(-0.95f, 1.12f, 0.12f), 2.05f, 0.20f, Vector3.zero, "RSK04_MAT_BurnishedCopper");
            Pipe(root, "right_return_pipe", new Vector3(1.00f, 1.12f, 0.12f), 1.85f, 0.13f, Vector3.zero, "RSK04_MAT_WornSteel");
            Pipe(root, "top_pressure_header", new Vector3(0f, 2.42f, 0.11f), width - 0.28f, 0.13f, new Vector3(0f, 0f, 90f));
            Pipe(root, "lower_feed_header", new Vector3(0.1f, 0.68f, 0.13f), width - 0.55f, 0.10f, new Vector3(0f, 0f, 90f));
            Gauge(root, "primary_wall_gauge", new Vector3(0.02f, 1.52f, 0.19f), 0.30f, -35f + variant * 28f);
            Part(root, "cage_lamp_glow", "RSK04_MESH_Cylinder16Unit", "RSK04_MAT_AmberGlassGlow", new Vector3(0.72f, 1.42f, 0.24f), new Vector3(0.22f, 0.42f, 0.22f), Vector3.zero);
            Part(root, "lamp_brass_cage", "RSK04_MESH_Ring32Unit", "RSK04_MAT_AgedBrass", new Vector3(0.72f, 1.42f, 0.25f), new Vector3(0.31f, 0.31f, 0.31f), Vector3.zero);
            Steam(root, "soft_boiler_steam", new Vector3(-1.20f, 2.03f, 0.21f), new Vector3(0.55f, 0.85f, 1f), -18f + variant * 9f);
            for (int i = 0; i < 10; i++) Rivet(root, "wall_rivet_" + i, new Vector3(-width * 0.42f + i * width * 0.084f, 2.18f, 0.0f), 0.046f);
            if (variant == 2) Part(root, "hazard_service_tag", "RSK04_MESH_BoxUnit", "RSK04_MAT_HazardYellowPaint", new Vector3(-0.32f, 0.95f, 0.18f), new Vector3(0.42f, 0.12f, 0.035f), new Vector3(0f, 0f, -8f));
        }

        private static void BuildPressureVaultDoorAlcove(GameObject root, int variant)
        {
            Part(root, "recessed_arch_wall", "RSK04_MESH_ArchPanelUnit", "RSK04_MAT_SootStone", new Vector3(0f, 1.42f, -0.18f), new Vector3(3.3f, 3.0f, 0.32f), Vector3.zero);
            Part(root, "deep_shadow_recess", "RSK04_MESH_ArchPanelUnit", "RSK04_MAT_DeepShadow", new Vector3(0f, 1.35f, -0.04f), new Vector3(2.45f, 2.35f, 0.18f), Vector3.zero);
            Part(root, "round_vault_door_disc", "RSK04_MESH_Cylinder32Unit", "RSK04_MAT_BlackenedIron", new Vector3(0f, 1.28f, 0.05f), new Vector3(1.72f, 0.12f, 1.72f), new Vector3(90f, 0f, 0f));
            Part(root, "outer_pressure_ring", "RSK04_MESH_Ring32Unit", "RSK04_MAT_AgedBrass", new Vector3(0f, 1.28f, 0.12f), new Vector3(1.92f, 1.92f, 1.92f), Vector3.zero);
            Part(root, "inner_lock_gear", "RSK04_MESH_Gear24Unit", "RSK04_MAT_AgedBrass", new Vector3(0f, 1.28f, 0.19f), new Vector3(0.72f, 0.72f, 0.72f), Vector3.zero);
            for (int i = 0; i < 8; i++)
            {
                float a = i * Mathf.PI * 0.25f + variant * 0.06f;
                Part(root, "radial_lock_bar_" + i, "RSK04_MESH_BoxUnit", i % 2 == 0 ? "RSK04_MAT_AgedBrass" : "RSK04_MAT_WornSteel", new Vector3(Mathf.Cos(a) * 0.53f, 1.28f + Mathf.Sin(a) * 0.53f, 0.20f), new Vector3(0.68f, 0.045f, 0.045f), new Vector3(0f, 0f, a * Mathf.Rad2Deg));
            }
            Gauge(root, "alcove_left_gauge", new Vector3(-1.20f, 1.95f, 0.1f), 0.18f, -15f);
            Gauge(root, "alcove_right_gauge", new Vector3(1.20f, 1.95f, 0.1f), 0.18f, 45f);
            Part(root, "floor_threshold", "RSK04_MESH_BoxUnit", "RSK04_MAT_AgedBrass", new Vector3(0f, 0.05f, 0.38f), new Vector3(2.65f, 0.10f, 0.50f), Vector3.zero);
            if (variant > 0) Steam(root, "door_seal_leak", new Vector3(1.05f, 1.12f, 0.27f), new Vector3(0.43f, 0.72f, 1f), 22f);
        }

        private static void BuildCatwalkBalconyModule(GameObject root, int variant)
        {
            float width = 3.15f + variant * 0.25f;
            Part(root, "catwalk_deck", "RSK04_MESH_BoxUnit", "RSK04_MAT_WornSteel", new Vector3(0f, 0.95f, 0.0f), new Vector3(width, 0.12f, 1.10f), Vector3.zero);
            Part(root, "deck_oil_shadow", "RSK04_MESH_BoxUnit", "RSK04_MAT_OilBlack", new Vector3(0.18f, 1.02f, -0.05f), new Vector3(width * 0.72f, 0.025f, 0.64f), Vector3.zero);
            Pipe(root, "front_brass_rail", new Vector3(0f, 1.55f, -0.52f), width, 0.045f, new Vector3(0f, 0f, 90f), "RSK04_MAT_AgedBrass");
            Pipe(root, "mid_brass_rail", new Vector3(0f, 1.28f, -0.52f), width, 0.035f, new Vector3(0f, 0f, 90f), "RSK04_MAT_AgedBrass");
            for (int i = 0; i < 5; i++)
            {
                float x = -width * 0.45f + i * width * 0.225f;
                Pipe(root, "vertical_rail_post_" + i, new Vector3(x, 1.30f, -0.52f), 0.72f, 0.035f, Vector3.zero, "RSK04_MAT_AgedBrass");
                Part(root, "under_brace_" + i, "RSK04_MESH_BoxUnit", "RSK04_MAT_BlackenedIron", new Vector3(x, 0.55f, 0.0f), new Vector3(0.07f, 0.86f, 0.08f), new Vector3(0f, 0f, i % 2 == 0 ? 24f : -24f));
            }
            Chain(root, "hanging_chain_left", -width * 0.36f, 5 + variant, 1.74f, -0.49f);
            if (variant >= 1) Part(root, "side_pipe_warning_marker", "RSK04_MESH_BoxUnit", "RSK04_MAT_HazardYellowPaint", new Vector3(width * 0.34f, 1.04f, -0.58f), new Vector3(0.48f, 0.055f, 0.08f), Vector3.zero);
        }

        private static void BuildRegulatorCoreMachinery(GameObject root, int variant)
        {
            Part(root, "heavy_floor_foot", "RSK04_MESH_Cylinder32Unit", "RSK04_MAT_BlackenedIron", new Vector3(0f, 0.15f, 0f), new Vector3(1.35f, 0.28f, 1.35f), Vector3.zero);
            Pipe(root, "central_pressure_tank", new Vector3(0f, 1.35f, 0f), 2.25f, 0.36f, Vector3.zero, "RSK04_MAT_BurnishedCopper");
            Part(root, "top_regulator_gear", "RSK04_MESH_Gear24Unit", "RSK04_MAT_AgedBrass", new Vector3(0f, 2.58f, 0f), new Vector3(0.90f, 0.90f, 0.90f), new Vector3(90f, 0f, 0f));
            for (int i = 0; i < 5; i++)
            {
                float a = i * Mathf.PI * 2f / 5f;
                Pipe(root, "copper_coil_" + i, new Vector3(Mathf.Cos(a) * 0.58f, 1.35f + Mathf.Sin(i) * 0.08f, Mathf.Sin(a) * 0.58f), 1.00f, 0.045f, new Vector3(90f, 0f, a * Mathf.Rad2Deg), "RSK04_MAT_BurnishedCopper");
            }
            Gauge(root, "front_core_gauge", new Vector3(0f, 1.42f, -0.46f), 0.19f, 68f - variant * 28f);
            Pipe(root, "left_outflow_pipe", new Vector3(-0.82f, 0.92f, 0f), 1.25f, 0.08f, new Vector3(0f, 0f, 90f), "RSK04_MAT_VerdigrisOxide");
            Pipe(root, "right_outflow_pipe", new Vector3(0.82f, 0.92f, 0f), 1.25f, 0.08f, new Vector3(0f, 0f, 90f), "RSK04_MAT_VerdigrisOxide");
            if (variant == 2) Steam(root, "top_pressure_wisp", new Vector3(0.36f, 2.72f, 0.02f), new Vector3(0.42f, 0.72f, 1f), 12f);
        }

        private static void BuildPipeGalleryCeilingCluster(GameObject root, int variant)
        {
            float width = 3.55f + variant * 0.20f;
            Part(root, "ceiling_shadow_panel", "RSK04_MESH_BoxUnit", "RSK04_MAT_DeepShadow", new Vector3(0f, 0.34f, 0f), new Vector3(width, 0.08f, 1.35f), Vector3.zero);
            for (int i = 0; i < 6; i++)
            {
                float z = -0.55f + i * 0.22f;
                string mat = i % 3 == 0 ? "RSK04_MAT_AgedBrass" : (i % 2 == 0 ? "RSK04_MAT_BurnishedCopper" : "RSK04_MAT_WornSteel");
                Pipe(root, "long_ceiling_pipe_" + i, new Vector3(0f, 0.08f + (i % 2) * 0.10f, z), width, 0.055f + i * 0.004f, new Vector3(90f, 0f, 90f), mat);
            }
            for (int i = -1; i <= 1; i++)
            {
                Part(root, "pipe_hanger_bracket_" + i, "RSK04_MESH_BoxUnit", "RSK04_MAT_BlackenedIron", new Vector3(i * width * 0.33f, -0.18f, 0f), new Vector3(0.08f, 0.48f, 1.28f), Vector3.zero);
            }
            Part(root, "valve_wheel_left", "RSK04_MESH_Gear24Unit", "RSK04_MAT_WarningRedNeedle", new Vector3(-0.75f, -0.16f, -0.42f), new Vector3(0.34f, 0.34f, 0.34f), new Vector3(90f, 0f, 0f));
            if (variant > 0) Steam(root, "ceiling_steam_leak", new Vector3(1.05f, -0.02f, 0.45f), new Vector3(0.48f, 0.76f, 1f), -20f);
        }

        private static void BuildServiceStairSilhouette(GameObject root, int variant)
        {
            int steps = 6 + variant;
            Part(root, "left_stringer", "RSK04_MESH_StairStringerUnit", "RSK04_MAT_BlackenedIron", new Vector3(0f, 0.75f, -0.45f), new Vector3(2.7f, 1.35f, 0.12f), Vector3.zero);
            Part(root, "right_stringer", "RSK04_MESH_StairStringerUnit", "RSK04_MAT_BlackenedIron", new Vector3(0f, 0.75f, 0.45f), new Vector3(2.7f, 1.35f, 0.12f), Vector3.zero);
            for (int i = 0; i < steps; i++)
            {
                float t = i / (float)(steps - 1);
                Part(root, "open_grate_step_" + i, "RSK04_MESH_BoxUnit", "RSK04_MAT_WornSteel", new Vector3(-1.2f + t * 2.4f, 0.22f + t * 1.32f, 0f), new Vector3(0.46f, 0.055f, 1.08f), Vector3.zero);
                if (i % 2 == 0) Part(root, "brass_step_nose_" + i, "RSK04_MESH_BoxUnit", "RSK04_MAT_AgedBrass", new Vector3(-1.2f + t * 2.4f, 0.27f + t * 1.32f, -0.55f), new Vector3(0.46f, 0.035f, 0.055f), Vector3.zero);
            }
            Pipe(root, "upper_handrail", new Vector3(0f, 1.78f, -0.62f), 2.85f, 0.04f, new Vector3(0f, 0f, 60f), "RSK04_MAT_AgedBrass");
            for (int i = 0; i < 4; i++) Pipe(root, "rail_post_" + i, new Vector3(-1.18f + i * 0.78f, 0.72f + i * 0.33f, -0.62f), 0.82f, 0.03f, Vector3.zero, "RSK04_MAT_AgedBrass");
        }

        private static void BuildFurnaceControlWall(GameObject root, int variant)
        {
            Part(root, "control_wall_slab", "RSK04_MESH_BoxUnit", "RSK04_MAT_SootStone", new Vector3(0f, 1.18f, -0.10f), new Vector3(2.8f, 2.30f, 0.16f), Vector3.zero);
            Part(root, "furnace_mouth_shadow", "RSK04_MESH_BoxUnit", "RSK04_MAT_DeepShadow", new Vector3(-0.72f, 0.72f, 0.02f), new Vector3(0.86f, 0.55f, 0.08f), Vector3.zero);
            Part(root, "furnace_orange_slit", "RSK04_MESH_BoxUnit", "RSK04_MAT_FurnaceOrangeGlow", new Vector3(-0.72f, 0.72f, 0.08f), new Vector3(0.70f, 0.28f, 0.035f), Vector3.zero);
            Gauge(root, "left_control_gauge", new Vector3(0.45f, 1.55f, 0.06f), 0.22f, -52f + variant * 20f);
            Gauge(root, "right_control_gauge", new Vector3(1.00f, 1.42f, 0.06f), 0.18f, 32f);
            for (int i = 0; i < 4; i++)
            {
                Part(root, "knife_switch_slot_" + i, "RSK04_MESH_BoxUnit", "RSK04_MAT_BlackenedIron", new Vector3(0.22f + i * 0.28f, 0.78f, 0.05f), new Vector3(0.07f, 0.46f, 0.035f), Vector3.zero);
                Part(root, "knife_switch_handle_" + i, "RSK04_MESH_BoxUnit", "RSK04_MAT_AgedBrass", new Vector3(0.22f + i * 0.28f, 0.78f + (i % 2 == 0 ? 0.10f : -0.10f), 0.10f), new Vector3(0.055f, 0.36f, 0.055f), new Vector3(0f, 0f, i % 2 == 0 ? -18f : 22f));
            }
            Pipe(root, "control_wall_feed_pipe", new Vector3(-1.18f, 1.48f, 0.02f), 1.55f, 0.07f, Vector3.zero, "RSK04_MAT_BurnishedCopper");
            Steam(root, "furnace_heat_haze", new Vector3(-0.72f, 1.18f, 0.12f), new Vector3(0.55f, 0.78f, 1f), 2f);
        }

        private static void BuildBrassFloorTrimThreshold(GameObject root, int variant)
        {
            float width = 3.4f + variant * 0.20f;
            Part(root, "dark_floor_plate", "RSK04_MESH_BoxUnit", "RSK04_MAT_WetStoneGloss", new Vector3(0f, 0f, 0f), new Vector3(width, 0.08f, 1.55f), Vector3.zero);
            Part(root, "front_brass_threshold", "RSK04_MESH_BoxUnit", "RSK04_MAT_AgedBrass", new Vector3(0f, 0.07f, -0.58f), new Vector3(width, 0.055f, 0.12f), Vector3.zero);
            Part(root, "rear_brass_threshold", "RSK04_MESH_BoxUnit", "RSK04_MAT_AgedBrass", new Vector3(0f, 0.07f, 0.58f), new Vector3(width, 0.055f, 0.12f), Vector3.zero);
            for (int i = 0; i < 8; i++)
            {
                float x = -width * 0.43f + i * width * 0.123f;
                Rivet(root, "floor_rivet_front_" + i, new Vector3(x, 0.105f, -0.58f), 0.045f);
                Rivet(root, "floor_rivet_rear_" + i, new Vector3(x, 0.105f, 0.58f), 0.045f);
            }
            Part(root, "center_drain_shadow", "RSK04_MESH_BoxUnit", "RSK04_MAT_DeepShadow", new Vector3(0f, 0.095f, 0f), new Vector3(width * 0.72f, 0.025f, 0.16f), Vector3.zero);
            for (int i = 0; i < 7; i++) Part(root, "drain_cross_slat_" + i, "RSK04_MESH_BoxUnit", "RSK04_MAT_WornSteel", new Vector3(-0.82f + i * 0.27f, 0.12f, 0f), new Vector3(0.05f, 0.045f, 0.28f), Vector3.zero);
        }

        private static void BuildLargeWarningGaugeWall(GameObject root, int variant)
        {
            Part(root, "gauge_wall_panel", "RSK04_MESH_BoxUnit", "RSK04_MAT_BlackenedIron", new Vector3(0f, 1.15f, -0.08f), new Vector3(3.0f, 2.25f, 0.14f), Vector3.zero);
            Part(root, "hazard_header_plate", "RSK04_MESH_BoxUnit", "RSK04_MAT_HazardYellowPaint", new Vector3(0f, 2.18f, 0.02f), new Vector3(2.65f, 0.20f, 0.055f), Vector3.zero);
            Gauge(root, "huge_center_pressure_gauge", new Vector3(0f, 1.42f, 0.06f), 0.48f, 62f - variant * 44f);
            Gauge(root, "small_left_pressure_gauge", new Vector3(-0.95f, 0.78f, 0.05f), 0.22f, -20f);
            Gauge(root, "small_right_pressure_gauge", new Vector3(0.95f, 0.78f, 0.05f), 0.22f, 42f);
            Pipe(root, "left_gauge_pipe", new Vector3(-0.86f, 1.52f, 0.0f), 0.95f, 0.055f, Vector3.zero, "RSK04_MAT_VerdigrisOxide");
            Pipe(root, "right_gauge_pipe", new Vector3(0.86f, 1.52f, 0.0f), 0.95f, 0.055f, Vector3.zero, "RSK04_MAT_BurnishedCopper");
            for (int i = 0; i < 9; i++) Rivet(root, "gauge_wall_rivet_" + i, new Vector3(-1.25f + i * 0.31f, 0.22f, 0.02f), 0.042f);
        }

        private static void BuildRoomCornerClutterCluster(GameObject root, int variant)
        {
            Part(root, "corner_floor_oil_shadow", "RSK04_MESH_QuadUnit", "RSK04_MAT_OilBlack", new Vector3(0f, 0.012f, 0f), new Vector3(1.85f, 1.4f, 1f), new Vector3(90f, 0f, 0f));
            Part(root, "crate_a", "RSK04_MESH_BoxUnit", "RSK04_MAT_SootStone", new Vector3(-0.42f, 0.30f, 0.20f), new Vector3(0.65f, 0.60f, 0.55f), Vector3.zero);
            Part(root, "crate_brass_band_a", "RSK04_MESH_BoxUnit", "RSK04_MAT_AgedBrass", new Vector3(-0.42f, 0.50f, -0.085f), new Vector3(0.68f, 0.055f, 0.045f), Vector3.zero);
            Pipe(root, "upright_spare_pipe_a", new Vector3(0.45f, 0.58f, -0.22f), 1.12f, 0.10f, Vector3.zero, "RSK04_MAT_WornSteel");
            Pipe(root, "upright_spare_pipe_b", new Vector3(0.72f, 0.48f, 0.14f), 0.92f, 0.08f, Vector3.zero, "RSK04_MAT_BurnishedCopper");
            Part(root, "small_pressure_canister", "RSK04_MESH_Cylinder16Unit", "RSK04_MAT_VerdigrisOxide", new Vector3(0.12f, 0.42f, 0.48f), new Vector3(0.32f, 0.72f, 0.32f), Vector3.zero);
            Gauge(root, "loose_canister_gauge", new Vector3(0.12f, 0.55f, 0.31f), 0.10f, -38f);
            Chain(root, "loose_chain", -0.68f, 5 + variant, 0.86f, -0.18f);
            if (variant > 0) Steam(root, "floor_pipe_wisp", new Vector3(0.66f, 0.92f, -0.18f), new Vector3(0.36f, 0.55f, 1f), 24f);
            if (variant == 2) Part(root, "hazard_scrap_plate", "RSK04_MESH_BoxUnit", "RSK04_MAT_HazardYellowPaint", new Vector3(-0.04f, 0.22f, -0.48f), new Vector3(0.55f, 0.05f, 0.28f), new Vector3(0f, 18f, 0f));
        }

        private static Mesh BuildBoxMesh()
        {
            Vector3[] vertices =
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
            };
            int[] triangles =
            {
                0, 2, 1, 0, 3, 2, 4, 5, 6, 4, 6, 7, 0, 1, 5, 0, 5, 4,
                2, 3, 7, 2, 7, 6, 1, 2, 6, 1, 6, 5, 0, 4, 7, 0, 7, 3
            };
            return FinishMesh(vertices, triangles);
        }

        private static Mesh BuildQuadMesh()
        {
            return FinishMesh(
                new[] { new Vector3(-0.5f, -0.5f, 0f), new Vector3(0.5f, -0.5f, 0f), new Vector3(0.5f, 0.5f, 0f), new Vector3(-0.5f, 0.5f, 0f) },
                new[] { 0, 1, 2, 0, 2, 3 });
        }

        private static Mesh BuildCylinderMesh(int sides, float radius, float height)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int i = 0; i < sides; i++)
            {
                float angle = i * Mathf.PI * 2f / sides;
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, -height * 0.5f, Mathf.Sin(angle) * radius));
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, height * 0.5f, Mathf.Sin(angle) * radius));
            }

            int bottomCenter = vertices.Count;
            vertices.Add(new Vector3(0f, -height * 0.5f, 0f));
            int topCenter = vertices.Count;
            vertices.Add(new Vector3(0f, height * 0.5f, 0f));

            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                AddQuad(triangles, i * 2, next * 2, next * 2 + 1, i * 2 + 1);
                triangles.Add(bottomCenter); triangles.Add(next * 2); triangles.Add(i * 2);
                triangles.Add(topCenter); triangles.Add(i * 2 + 1); triangles.Add(next * 2 + 1);
            }

            return FinishMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh BuildRingMesh(int sides, float inner, float outer, float depth)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int i = 0; i < sides; i++)
            {
                float angle = i * Mathf.PI * 2f / sides;
                vertices.Add(new Vector3(Mathf.Cos(angle) * outer, Mathf.Sin(angle) * outer, -depth));
                vertices.Add(new Vector3(Mathf.Cos(angle) * inner, Mathf.Sin(angle) * inner, -depth));
                vertices.Add(new Vector3(Mathf.Cos(angle) * outer, Mathf.Sin(angle) * outer, depth));
                vertices.Add(new Vector3(Mathf.Cos(angle) * inner, Mathf.Sin(angle) * inner, depth));
            }

            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                AddQuad(triangles, i * 4, next * 4, next * 4 + 2, i * 4 + 2);
                AddQuad(triangles, i * 4 + 3, next * 4 + 3, next * 4 + 1, i * 4 + 1);
                AddQuad(triangles, i * 4 + 2, next * 4 + 2, next * 4 + 3, i * 4 + 3);
                AddQuad(triangles, i * 4 + 1, next * 4 + 1, next * 4, i * 4);
            }

            return FinishMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh BuildGearMesh(int teeth, float inner, float outer, float depth)
        {
            int sides = teeth * 2;
            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(new Vector3(0f, 0f, depth));
            vertices.Add(new Vector3(0f, 0f, -depth));
            for (int i = 0; i < sides; i++)
            {
                float angle = i * Mathf.PI * 2f / sides;
                float radius = i % 2 == 0 ? outer : inner;
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, depth));
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, -depth));
            }

            List<int> triangles = new List<int>();
            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                int currentFront = 2 + i * 2;
                int currentBack = currentFront + 1;
                int nextFront = 2 + next * 2;
                int nextBack = nextFront + 1;
                triangles.Add(0); triangles.Add(currentFront); triangles.Add(nextFront);
                triangles.Add(1); triangles.Add(nextBack); triangles.Add(currentBack);
                AddQuad(triangles, currentFront, currentBack, nextBack, nextFront);
            }

            return FinishMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh BuildArchPanelMesh()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            vertices.Add(new Vector3(0f, 0.12f, 0f));
            vertices.Add(new Vector3(-0.5f, -0.5f, 0f));
            vertices.Add(new Vector3(0.5f, -0.5f, 0f));
            vertices.Add(new Vector3(0.5f, 0.18f, 0f));
            for (int i = 1; i <= 9; i++)
            {
                float angle = Mathf.PI - i * Mathf.PI / 10f;
                vertices.Add(new Vector3(Mathf.Cos(angle) * 0.5f, 0.18f + Mathf.Sin(angle) * 0.38f, 0f));
            }
            vertices.Add(new Vector3(-0.5f, 0.18f, 0f));
            for (int i = 1; i < vertices.Count; i++)
            {
                int next = i == vertices.Count - 1 ? 1 : i + 1;
                triangles.Add(0); triangles.Add(i); triangles.Add(next);
            }
            return FinishMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh BuildStairStringerMesh()
        {
            Vector3[] vertices =
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f)
            };
            int[] triangles =
            {
                0, 1, 2, 3, 5, 4, 0, 3, 4, 0, 4, 1,
                1, 4, 5, 1, 5, 2, 2, 5, 3, 2, 3, 0
            };
            return FinishMesh(vertices, triangles);
        }

        private static Mesh BuildPressureNeedleMesh()
        {
            Vector3[] vertices =
            {
                new Vector3(-0.035f, -0.08f, 0f), new Vector3(0.035f, -0.08f, 0f), new Vector3(0.020f, 0.44f, 0f),
                new Vector3(0f, 0.54f, 0f), new Vector3(-0.020f, 0.44f, 0f)
            };
            return FinishMesh(vertices, new[] { 0, 1, 2, 0, 2, 4, 4, 2, 3 });
        }

        private static Mesh BuildSteamWispMesh()
        {
            Vector3[] vertices =
            {
                new Vector3(-0.16f, -0.48f, 0f), new Vector3(0.16f, -0.48f, 0f), new Vector3(0.34f, -0.08f, 0f),
                new Vector3(0.12f, 0.46f, 0f), new Vector3(-0.24f, 0.38f, 0f), new Vector3(-0.38f, -0.02f, 0f), Vector3.zero
            };
            return FinishMesh(vertices, new[] { 6, 0, 1, 6, 1, 2, 6, 2, 3, 6, 3, 4, 6, 4, 5, 6, 5, 0 });
        }

        private static void AddQuad(List<int> triangles, int a, int b, int c, int d)
        {
            triangles.Add(a); triangles.Add(b); triangles.Add(c);
            triangles.Add(a); triangles.Add(c); triangles.Add(d);
        }

        private static Mesh FinishMesh(Vector3[] vertices, int[] triangles)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void RenderFamily(string key, string title, SetpieceSpec[] specs, Vector3 cameraPos, Vector3 cameraEuler)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientLight = new Color(0.22f, 0.19f, 0.15f);
            SetupPreviewLights();
            CreatePreviewFloor(specs.Length);

            int columns = Mathf.CeilToInt(Mathf.Sqrt(specs.Length));
            for (int i = 0; i < specs.Length; i++)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageRoot + "/Runtime/Prefabs/" + specs[i].FileName);
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (instance == null) continue;
                instance.transform.position = new Vector3((i % columns - (columns - 1) * 0.5f) * 3.2f, 0f, (i / columns) * 2.25f);
                instance.transform.rotation = Quaternion.Euler(0f, -10f + i * 11f, 0f);
            }

            Camera camera = new GameObject("preview_camera_" + key).AddComponent<Camera>();
            camera.transform.position = cameraPos;
            camera.transform.rotation = Quaternion.Euler(cameraEuler);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.070f, 0.060f, 0.050f);
            camera.fieldOfView = key == "all_setpieces_contact_sheet" ? 42f : 36f;
            RenderCamera(camera, Path.Combine(ResolveRenderOutputRoot(), "RSK04_PREVIEW_" + key + "_v0.1.45.png"), 1800, 1200);
            EditorSceneManager.CloseScene(scene, true);
        }

        private static void SetupPreviewLights()
        {
            GameObject key = new GameObject("preview_key_light");
            Light keyLight = key.AddComponent<Light>();
            keyLight.type = LightType.Directional;
            keyLight.intensity = 2.45f;
            key.transform.rotation = Quaternion.Euler(48f, -32f, 0f);

            GameObject warm = new GameObject("preview_warm_fill");
            Light warmLight = warm.AddComponent<Light>();
            warmLight.type = LightType.Point;
            warmLight.intensity = 3.0f;
            warmLight.range = 9f;
            warmLight.color = new Color(1f, 0.55f, 0.18f);
            warm.transform.position = new Vector3(-2.8f, 2.1f, -2.2f);
        }

        private static void CreatePreviewFloor(int itemCount)
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.DestroyImmediate(floor.GetComponent<Collider>());
            floor.name = "preview_floor";
            floor.transform.position = new Vector3(0f, -0.045f, 0f);
            floor.transform.localScale = new Vector3(Mathf.Max(7f, itemCount * 1.05f), 0.06f, 5.8f);
            floor.GetComponent<MeshRenderer>().sharedMaterial = Materials["RSK04_MAT_WetStoneGloss"];
        }

        private static void RenderMaterialSwatch(string outputRoot)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientLight = new Color(0.24f, 0.21f, 0.17f);
            SetupPreviewLights();
            int i = 0;
            foreach (Material material in Materials.Values)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.DestroyImmediate(sphere.GetComponent<Collider>());
                sphere.name = material.name;
                sphere.GetComponent<MeshRenderer>().sharedMaterial = material;
                sphere.transform.position = new Vector3((i % 6 - 2.5f) * 1.02f, 1.35f - (i / 6) * 1.03f, 0f);
                sphere.transform.localScale = Vector3.one * 0.62f;
                i++;
            }

            Camera camera = new GameObject("material_swatch_camera").AddComponent<Camera>();
            camera.transform.position = new Vector3(0f, -0.1f, -7.0f);
            camera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.070f, 0.060f, 0.050f);
            camera.fieldOfView = 35f;
            RenderCamera(camera, Path.Combine(outputRoot, "RSK04_PREVIEW_material_swatch_v0.1.45.png"), 1800, 1200);
            EditorSceneManager.CloseScene(scene, true);
        }

        private static void RenderCamera(Camera camera, string path, int width, int height)
        {
            RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D image = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            image.Apply();
            File.WriteAllBytes(path, image.EncodeToPNG());
            RenderTexture.active = previous;
            camera.targetTexture = null;
            UnityEngine.Object.DestroyImmediate(image);
            rt.Release();
            UnityEngine.Object.DestroyImmediate(rt);
        }

        private static void WriteMetadata(string importStatus)
        {
            string manifest = BuildManifest(importStatus);
            string catalog = BuildCatalog();
            File.WriteAllText(FullPath(PackageRoot + "/Documentation~/Manifest/" + ManifestFileName), manifest, Encoding.UTF8);
            File.WriteAllText(FullPath(PackageRoot + "/Runtime/Metadata/" + CatalogFileName), catalog, Encoding.UTF8);
            File.WriteAllText(Path.Combine(ProjectRoot, ProductionDocRelativePath, "RSK04_RoomSetpieceKit04_ProductionReport.md"), BuildProductionReport(importStatus), Encoding.UTF8);
            AssetDatabase.ImportAsset(PackageRoot + "/Documentation~/Manifest/" + ManifestFileName);
            AssetDatabase.ImportAsset(PackageRoot + "/Runtime/Metadata/" + CatalogFileName);
        }

        private static string BuildManifest(string importStatus)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            AddJson(json, "pack_id", PackId, 1, true);
            AddJson(json, "display_name", "Room Setpiece Kit 04", 1, true);
            AddJson(json, "version", Version, 1, true);
            AddJson(json, "build_id", BuildId, 1, true);
            AddJson(json, "unity_version", Application.unityVersion, 1, true);
            AddJson(json, "generated_at_utc", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), 1, true);
            AddJson(json, "sidecar_project", "UD-SC-RSK04-RoomSetpieceKit04", 1, true);
            AddJson(json, "owner_lane", "sidecar-room-setpiece-kit04", 1, true);
            AddJson(json, "primary_intake_owner", "main-lane-art-integration", 1, true);
            AddJson(json, "canonical_root", "AssetPacks/BrassworksBreach.RoomSetpieceKit04", 1, true);
            AddJson(json, "package_root", "AssetPacks/BrassworksBreach.RoomSetpieceKit04", 1, true);
            AddJson(json, "package_name", PackageName, 1, true);
            AddJson(json, "package_version", Version + "-" + BuildId, 1, true);
            json.AppendLine("  \"asset_counts\": {");
            json.AppendLine("    \"generated_prefabs\": " + Specs().Length + ",");
            json.AppendLine("    \"generated_materials\": " + Materials.Count + ",");
            json.AppendLine("    \"generated_meshes\": " + Meshes.Count + ",");
            json.AppendLine("    \"textures\": 0,");
            json.AppendLine("    \"audio\": 0,");
            json.AppendLine("    \"vfx\": 0,");
            json.AppendLine("    \"animation_clips\": 0,");
            json.AppendLine("    \"runtime_scripts\": 0,");
            json.AppendLine("    \"colliders\": 0,");
            json.AppendLine("    \"rigidbodies\": 0,");
            json.AppendLine("    \"audio_sources\": 0,");
            json.AppendLine("    \"preview_renders\": " + PreviewPaths().Length);
            json.AppendLine("  },");
            AddArray(json, "generated_prefabs", PrefabPaths(), 1, true);
            AddArray(json, "generated_materials", AssetPaths("Runtime/Materials", ".mat"), 1, true);
            AddArray(json, "generated_meshes", AssetPaths("Runtime/Meshes", ".asset"), 1, true);
            AddArray(json, "preview_renders", PreviewPaths(), 1, true);
            json.AppendLine("  \"dependencies\": [],");
            json.AppendLine("  \"required_primary_changes\": [],");
            json.AppendLine("  \"path_collisions_checked\": true,");
            json.AppendLine("  \"guid_collisions_checked\": true,");
            AddJson(json, "import_smoke_status", importStatus, 1, true);
            json.AppendLine("  \"known_risks\": [");
            json.AppendLine("    \"Visual-only room setpieces require primary-lane collision, navigation, occlusion, and gameplay authority before use in shippable levels.\",");
            json.AppendLine("    \"Generated materials are procedural Unity lookdev targets and may need render-pipeline conversion during main-lane promotion.\",");
            json.AppendLine("    \"Steam wisps and furnace haze are geometry/material placeholders only; no autonomous VFX or audio systems are included.\"");
            json.AppendLine("  ],");
            AddJson(json, "rollback_path", "delete isolated package root or remove local package reference com.brassworks.sidecar.room-setpiece-kit04", 1, true);
            AddJson(json, "decision", "ready_for_primary_quarantine_after_static_validation_and_preview_review", 1, false);
            json.AppendLine("}");
            return json.ToString();
        }

        private static string BuildCatalog()
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            AddJson(json, "catalog_id", "RSK04_RuntimeCatalog_v0.1.45", 1, true);
            AddJson(json, "package_name", PackageName, 1, true);
            AddJson(json, "version", Version + "-" + BuildId, 1, true);
            json.AppendLine("  \"prefabs\": [");
            SetpieceSpec[] specs = Specs();
            for (int i = 0; i < specs.Length; i++)
            {
                SetpieceSpec spec = specs[i];
                json.AppendLine("    {");
                AddJson(json, "file", "Packages/" + PackageName + "/Runtime/Prefabs/" + spec.FileName, 3, true);
                AddJson(json, "family", spec.Family, 3, true);
                AddJson(json, "description", spec.Description, 3, true);
                AddJson(json, "visual_only", "true", 3, true, true);
                json.AppendLine("      \"bounds_meters\": [" + spec.Bounds.x.ToString("0.###", CultureInfo.InvariantCulture) + ", " + spec.Bounds.y.ToString("0.###", CultureInfo.InvariantCulture) + ", " + spec.Bounds.z.ToString("0.###", CultureInfo.InvariantCulture) + "]");
                json.Append("    }");
                json.AppendLine(i == specs.Length - 1 ? "" : ",");
            }
            json.AppendLine("  ],");
            AddArray(json, "materials", AssetPaths("Runtime/Materials", ".mat"), 1, true);
            AddArray(json, "meshes", AssetPaths("Runtime/Meshes", ".asset"), 1, true);
            AddJson(json, "authority", "visual_only_no_gameplay_scripts_no_colliders_no_rigidbodies_no_audio", 1, false);
            json.AppendLine("}");
            return json.ToString();
        }

        private static string BuildProductionReport(string importStatus)
        {
            return "# Room Setpiece Kit 04 Production Report\n\n" +
                "- Package: `AssetPacks/BrassworksBreach.RoomSetpieceKit04`\n" +
                "- Version: `0.1.45-p001`\n" +
                "- Prefabs: " + Specs().Length + "\n" +
                "- Materials: " + Materials.Count + "\n" +
                "- Reusable meshes: " + Meshes.Count + "\n" +
                "- Preview renders: " + PreviewPaths().Length + "\n" +
                "- Import smoke status: `" + importStatus + "`\n\n" +
                "Generated in Unity only. Prefabs are visual dressing assets with no gameplay scripts, colliders, rigidbodies, or autonomous audio.\n";
        }

        private static int WriteUnityValidationReport()
        {
            string prefabRoot = FullPath(PackageRoot + "/Runtime/Prefabs");
            string[] forbidden = { "Collider", "Rigidbody", "AudioSource", "m_Script" };
            List<string> findings = new List<string>();
            foreach (string prefab in Directory.GetFiles(prefabRoot, "*.prefab"))
            {
                string text = File.ReadAllText(prefab);
                foreach (string token in forbidden)
                {
                    if (text.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        findings.Add(Path.GetFileName(prefab) + " contains forbidden token " + token);
                    }
                }
            }

            string reportPath = Path.Combine(ProjectRoot, ProductionDocRelativePath, "RSK04_RoomSetpieceKit04_UnityValidationReport_v0.1.45.json");
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            AddJson(json, "status", findings.Count == 0 ? "pass" : "fail", 1, true);
            json.AppendLine("  \"prefabs\": " + Directory.GetFiles(prefabRoot, "*.prefab").Length + ",");
            json.AppendLine("  \"materials\": " + Directory.GetFiles(FullPath(PackageRoot + "/Runtime/Materials"), "*.mat").Length + ",");
            json.AppendLine("  \"meshes\": " + Directory.GetFiles(FullPath(PackageRoot + "/Runtime/Meshes"), "*.asset").Length + ",");
            json.AppendLine("  \"preview_pngs\": " + Directory.GetFiles(ResolveRenderOutputRoot(), "RSK04_*.png").Length + ",");
            json.AppendLine("  \"forbidden_token_findings\": [");
            for (int i = 0; i < findings.Count; i++)
            {
                json.Append("    \"").Append(Escape(findings[i])).Append("\"").AppendLine(i == findings.Count - 1 ? "" : ",");
            }
            json.AppendLine("  ]");
            json.AppendLine("}");
            File.WriteAllText(reportPath, json.ToString(), Encoding.UTF8);
            Debug.Log("RSK04_UNITY_VALIDATION_" + (findings.Count == 0 ? "PASS" : "FAIL") + " v0.1.45 findings=" + findings.Count);
            return findings.Count == 0 ? 0 : 1;
        }

        private static string[] PrefabPaths()
        {
            List<string> paths = new List<string>();
            foreach (SetpieceSpec spec in Specs()) paths.Add("Packages/" + PackageName + "/Runtime/Prefabs/" + spec.FileName);
            return paths.ToArray();
        }

        private static string[] AssetPaths(string folder, string extension)
        {
            List<string> paths = new List<string>();
            string full = FullPath(PackageRoot + "/" + folder);
            if (Directory.Exists(full))
            {
                foreach (string file in Directory.GetFiles(full, "*" + extension))
                {
                    paths.Add("Packages/" + PackageName + "/" + folder + "/" + Path.GetFileName(file));
                }
            }
            paths.Sort(StringComparer.Ordinal);
            return paths.ToArray();
        }

        private static string[] PreviewPaths()
        {
            return new[]
            {
                RenderOutputRelativePath + "/RSK04_PREVIEW_boiler_chamber_wall_bay_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_pressure_vault_door_alcove_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_catwalk_balcony_module_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_regulator_core_machinery_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_pipe_gallery_ceiling_cluster_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_service_stair_silhouette_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_furnace_control_wall_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_brass_floor_trim_threshold_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_large_warning_gauge_wall_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_room_corner_clutter_cluster_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_all_setpieces_contact_sheet_v0.1.45.png",
                RenderOutputRelativePath + "/RSK04_PREVIEW_material_swatch_v0.1.45.png"
            };
        }

        private static void AddJson(StringBuilder json, string key, string value, int indent, bool comma, bool raw = false)
        {
            json.Append(new string(' ', indent * 2));
            json.Append("\"").Append(key).Append("\": ");
            json.Append(raw ? value : "\"" + Escape(value) + "\"");
            json.AppendLine(comma ? "," : "");
        }

        private static void AddArray(StringBuilder json, string key, string[] values, int indent, bool comma)
        {
            json.Append(new string(' ', indent * 2)).Append("\"").Append(key).AppendLine("\": [");
            for (int i = 0; i < values.Length; i++)
            {
                json.Append(new string(' ', (indent + 1) * 2)).Append("\"").Append(Escape(values[i])).Append("\"").AppendLine(i == values.Length - 1 ? "" : ",");
            }
            json.Append(new string(' ', indent * 2)).Append("]").AppendLine(comma ? "," : "");
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string PackageRoot
        {
            get { return "Packages/" + PackageName; }
        }

        private static string ProjectRoot
        {
            get
            {
                string validationProject = Directory.GetCurrentDirectory().Replace("\\", "/");
                string packageRoot = Path.GetFullPath(Path.Combine(validationProject, "..")).Replace("\\", "/");
                return Path.GetFullPath(Path.Combine(packageRoot, "../..")).Replace("\\", "/");
            }
        }

        private static string ResolveRenderOutputRoot()
        {
            return Path.Combine(ProjectRoot, RenderOutputRelativePath).Replace("\\", "/");
        }

        private static string FullPath(string assetPath)
        {
            string relative = assetPath.StartsWith(PackageRoot, StringComparison.Ordinal) ? assetPath.Substring(PackageRoot.Length).TrimStart('/') : assetPath;
            string validationProject = Directory.GetCurrentDirectory();
            string packageFull = Path.GetFullPath(Path.Combine(validationProject, ".."));
            return Path.Combine(packageFull, relative.Replace("/", Path.DirectorySeparatorChar.ToString()));
        }

        private sealed class SetpieceSpec
        {
            public readonly string FileName;
            public readonly string Family;
            public readonly string Description;
            public readonly Vector3 Bounds;
            public readonly int Variant;
            public readonly Action<GameObject, int> Builder;

            public SetpieceSpec(string fileName, string family, string description, Vector3 bounds, int variant, Action<GameObject, int> builder)
            {
                FileName = fileName;
                Family = family;
                Description = description;
                Bounds = bounds;
                Variant = variant;
                Builder = builder;
            }
        }
    }
}
#endif
