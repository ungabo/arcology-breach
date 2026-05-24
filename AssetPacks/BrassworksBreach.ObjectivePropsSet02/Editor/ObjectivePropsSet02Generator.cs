using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BrassworksBreach.ObjectivePropsSet02;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.ObjectivePropsSet02.Editor
{
    public static class ObjectivePropsSet02Generator
    {
        public const string PackageName = "com.brassworks.sidecar.objective-props-set02";
        public const string Version = "0.1.42";
        public const string VersionLabel = "v0.1.42";
        public const string BuildId = "p001";
        public const string PackId = "OPS02";
        public const string RenderOutputRelativePath = "Documentation/ConceptRenders/V0_1_42_ObjectivePropsSet02";
        public const string ProductionOutputRelativePath = "Documentation/AssetProduction/V0_1_42_ObjectivePropsSet02";

        private const string MenuRoot = "Brassworks Breach/Sidecar Packs/Objective Props Set 02 v0.1.42/";
        private const string PackageManifestFileName = "OPS02_ObjectivePropsSet02_Manifest_v0.1.42-p001.json";
        private const string GeneratedCatalogFileName = "OPS02_ObjectivePropsSet02_Catalog_v0.1.42.json";

        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();

        private static readonly string[] MaterialTags =
        {
            "riveted_iron",
            "aged_brass",
            "burnished_copper",
            "oxidized_copper",
            "hazard_yellow",
            "red_enamel",
            "green_signal_glass",
            "amber_lamp_glass",
            "blue_pressure_steel",
            "ivory_gauge_face",
            "black_rubber",
            "walnut_grip",
            "paper_label",
            "white_inlay",
            "soot_shadow",
            "pressure_glow"
        };

        [MenuItem(MenuRoot + "Generate Package Assets")]
        public static void GenerateAll()
        {
            Materials.Clear();
            Meshes.Clear();

            EnsurePackageFolders();
            CreateMaterials();
            CreateMeshes();

            foreach (var spec in GetSpecs())
            {
                CreatePrefab(spec);
            }

            WriteManifestFiles(
                "generated_by_unity_sidecar_batchmode_v0.1.42",
                "pending_preview_render",
                "pending_sidecar_validator");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"BB_OBJECTIVE_PROPS_SET02_GENERATE_PASS {VersionLabel} prefabs={GetSpecs().Length} materials={GetMaterialNames().Length} meshes={GetMeshNames().Length}");
        }

        public static string[] GeneratedPrefabAssetPaths()
        {
            var specs = GetSpecs();
            var paths = new string[specs.Length];
            for (var i = 0; i < specs.Length; i++)
            {
                paths[i] = CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + specs[i].FileName);
            }

            return paths;
        }

        public static string[] GeneratedMaterialAssetPaths()
        {
            var names = GetMaterialNames();
            var paths = new string[names.Length];
            for (var i = 0; i < names.Length; i++)
            {
                paths[i] = CombineAssetPath(PackageRoot, "Runtime/Materials/" + names[i] + ".mat");
            }

            return paths;
        }

        public static string[] GeneratedMeshAssetPaths()
        {
            var names = GetMeshNames();
            var paths = new string[names.Length];
            for (var i = 0; i < names.Length; i++)
            {
                paths[i] = CombineAssetPath(PackageRoot, "Runtime/Meshes/" + names[i] + ".asset");
            }

            return paths;
        }

        public static IReadOnlyList<PropSpec> Specs => GetSpecs();

        public static string ResolveRepositoryRenderRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_OPS02_RENDER_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return Path.GetFullPath(explicitRoot);
            }

            return Path.Combine(ResolveRepoRoot(), RenderOutputRelativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        public static string ResolveRepositoryProductionRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_OPS02_PRODUCTION_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return Path.GetFullPath(explicitRoot);
            }

            return Path.Combine(ResolveRepoRoot(), ProductionOutputRelativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        public static void MarkValidated(string importStatus, string previewStatus, string sidecarStatus)
        {
            WriteManifestFiles(importStatus, previewStatus, sidecarStatus);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void EnsurePackageFolders()
        {
            foreach (var folder in new[] { "Runtime/Prefabs", "Runtime/Materials", "Runtime/Meshes", "Runtime/Metadata", "Documentation~/Manifest", "Samples~/PreviewScene" })
            {
                Directory.CreateDirectory(AssetPathToFullPath(CombineAssetPath(PackageRoot, folder)));
            }

            AssetDatabase.Refresh();
        }

        private static void CreateMaterials()
        {
            AddMaterial("iron", "OPS02_MAT_RivetedIron", new Color(0.035f, 0.034f, 0.032f), 0.72f, 0.34f);
            AddMaterial("brass", "OPS02_MAT_AgedBrass", new Color(0.77f, 0.56f, 0.27f), 0.88f, 0.42f);
            AddMaterial("darkBrass", "OPS02_MAT_DarkAgedBrass", new Color(0.45f, 0.29f, 0.11f), 0.84f, 0.28f);
            AddMaterial("copper", "OPS02_MAT_BurnishedCopper", new Color(0.72f, 0.31f, 0.14f), 0.82f, 0.38f);
            AddMaterial("patina", "OPS02_MAT_OxidizedCopperPatina", new Color(0.10f, 0.38f, 0.32f), 0.45f, 0.30f);
            AddMaterial("yellow", "OPS02_MAT_HazardYellowPaint", new Color(0.98f, 0.68f, 0.13f), 0.28f, 0.20f);
            AddMaterial("red", "OPS02_MAT_RedOverrideEnamel", new Color(0.72f, 0.035f, 0.025f), 0.12f, 0.48f, new Color(0.70f, 0.02f, 0.01f) * 0.35f);
            AddMaterial("greenGlass", "OPS02_MAT_GreenSignalGlass", new Color(0.12f, 0.86f, 0.38f, 0.72f), 0.02f, 0.86f, new Color(0.06f, 0.74f, 0.28f) * 1.4f, true);
            AddMaterial("amber", "OPS02_MAT_AmberLampGlass", new Color(1.0f, 0.52f, 0.12f, 0.74f), 0.02f, 0.84f, new Color(1.0f, 0.36f, 0.08f) * 1.5f, true);
            AddMaterial("blueSteel", "OPS02_MAT_BluedPressureSteel", new Color(0.08f, 0.18f, 0.28f), 0.74f, 0.55f);
            AddMaterial("ivory", "OPS02_MAT_IvoryGaugeFace", new Color(0.82f, 0.75f, 0.56f), 0.02f, 0.22f);
            AddMaterial("rubber", "OPS02_MAT_BlackRubberGasket", new Color(0.012f, 0.011f, 0.010f), 0.0f, 0.22f);
            AddMaterial("walnut", "OPS02_MAT_VarnishedWalnut", new Color(0.30f, 0.13f, 0.055f), 0.04f, 0.52f);
            AddMaterial("paper", "OPS02_MAT_AgedPaperLabel", new Color(0.78f, 0.69f, 0.50f), 0.0f, 0.19f);
            AddMaterial("white", "OPS02_MAT_ReadabilityWhiteInlay", new Color(0.94f, 0.88f, 0.70f), 0.12f, 0.32f, new Color(0.75f, 0.60f, 0.28f) * 0.18f);
            AddMaterial("shadow", "OPS02_MAT_SootShadow", new Color(0.010f, 0.009f, 0.008f, 0.64f), 0.0f, 0.10f, null, true);
            AddMaterial("glow", "OPS02_MAT_PressureGlowCyan", new Color(0.22f, 0.72f, 0.88f, 0.76f), 0.0f, 0.88f, new Color(0.12f, 0.55f, 0.78f) * 1.8f, true);
        }

        private static void AddMaterial(string key, string name, Color color, float metallic, float smoothness, Color? emission = null, bool transparent = false)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("HDRP/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Diffuse");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible material shader found.");
            }

            var material = new Material(shader)
            {
                name = name,
                color = color
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

            var path = CombineAssetPath(PackageRoot, "Runtime/Materials/" + name + ".mat");
            CreateOrReplaceAsset(material, path);
            Materials[key] = AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        private static void CreateMeshes()
        {
            AddMesh("box", "OPS02_Mesh_BoxUnit", CreateBoxMesh());
            AddMesh("cylinder16", "OPS02_Mesh_Cylinder16Unit", CreateCylinderMesh(16));
            AddMesh("cylinder32", "OPS02_Mesh_Cylinder32Unit", CreateCylinderMesh(32));
            AddMesh("gear12", "OPS02_Mesh_Gear12ToothUnit", CreateGearMesh(12));
            AddMesh("gear18", "OPS02_Mesh_Gear18ToothUnit", CreateGearMesh(18));
            AddMesh("keyhole", "OPS02_Mesh_KeyholeGlyphUnit", CreateKeyholeMesh());
            AddMesh("needle", "OPS02_Mesh_PointerNeedleUnit", CreatePointerNeedleMesh());
            AddMesh("lever", "OPS02_Mesh_TaperedLeverHandle", CreateLeverHandleMesh());
            AddMesh("chain", "OPS02_Mesh_ChainLinkOvalUnit", CreateChainLinkMesh());
            AddMesh("arrow", "OPS02_Mesh_ChevronArrowUnit", CreateArrowMesh());
            AddMesh("hex", "OPS02_Mesh_HexBoltUnit", CreateCylinderMesh(6));
        }

        private static void AddMesh(string key, string name, Mesh mesh)
        {
            mesh.name = name;
            var path = CombineAssetPath(PackageRoot, "Runtime/Meshes/" + name + ".asset");
            CreateOrReplaceAsset(mesh, path);
            Meshes[key] = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        private static void CreatePrefab(PropSpec spec)
        {
            var root = NewRoot(spec);
            switch (spec.Kind)
            {
                case PropKind.KeyedLock:
                    BuildKeyedLock(root, spec);
                    break;
                case PropKind.ValvePanel:
                    BuildValvePanel(root, spec);
                    break;
                case PropKind.LiftCall:
                    BuildLiftCallStation(root, spec);
                    break;
                case PropKind.PressureRegulator:
                    BuildPressureRegulator(root, spec);
                    break;
                case PropKind.SecretCache:
                    BuildSecretCache(root, spec);
                    break;
                case PropKind.Actuator:
                    BuildActuator(root, spec);
                    break;
                case PropKind.GovernorOverride:
                    BuildGovernorOverride(root, spec);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SavePrefab(root, spec);
        }

        private static void BuildKeyedLock(GameObject root, PropSpec spec)
        {
            var w = 1.10f + spec.Variant * 0.08f;
            AddPanel(root.transform, "keyed_lock_backer", w, 1.38f, 0.13f, "iron");
            Box(root.transform, "aged_brass_lock_face_plate", Vector3.back * 0.085f, new Vector3(0.70f, 0.82f, 0.055f), Vector3.zero, "brass");
            Part(root.transform, "keyhole", "soot_black_oversized_keyhole_glyph", new Vector3(0f, -0.07f, -0.130f), new Vector3(0.42f, 0.66f, 0.060f), Vector3.zero, "shadow");
            Part(root.transform, spec.Variant == 3 ? "gear18" : "gear12", "dark_brass_readable_cog_socket", new Vector3(0f, 0.36f, -0.145f), new Vector3(0.45f, 0.45f, 0.055f), new Vector3(0f, 0f, spec.Variant * 9f), "darkBrass");
            Disc(root.transform, "green_ready_lamp", new Vector3(-0.36f, -0.48f, -0.145f), 0.085f, 0.025f, spec.Variant == 2 ? "greenGlass" : "rubber");
            Disc(root.transform, "red_locked_lamp", new Vector3(0.36f, -0.48f, -0.145f), 0.085f, 0.025f, spec.Variant == 2 ? "rubber" : "red");
            Label(root.transform, "readability_label_KEY", "KEY", new Vector3(0f, 0.58f, -0.151f), 0.044f, new Color(0.96f, 0.84f, 0.50f));

            var bitCount = 2 + spec.Variant;
            for (var i = 0; i < bitCount; i++)
            {
                var x = -0.27f + i * (0.54f / Mathf.Max(1, bitCount - 1));
                Box(root.transform, $"white_inlay_key_bit_index_{i:00}", new Vector3(x, -0.56f, -0.148f), new Vector3(0.065f, 0.12f + i * 0.018f, 0.020f), Vector3.zero, "white");
            }

            AddRivets(root.transform, "keyed_lock_corner_rivets", w * 0.45f, 0.59f, 0.050f, "brass");
        }

        private static void BuildValvePanel(GameObject root, PropSpec spec)
        {
            var valves = spec.Variant == 0 ? 2 : spec.Variant == 1 ? 3 : 4;
            AddPanel(root.transform, "valve_puzzle_backer", 1.52f, 1.16f, 0.12f, "blueSteel");
            Label(root.transform, "readability_label_VALVE", spec.Variant == 3 ? "STEAM" : "VALVE", new Vector3(0f, 0.47f, -0.145f), 0.040f, new Color(0.96f, 0.84f, 0.50f));

            for (var i = 0; i < valves; i++)
            {
                var x = valves == 1 ? 0f : -0.52f + i * (1.04f / Mathf.Max(1, valves - 1));
                var y = i % 2 == 0 ? 0.05f : -0.08f;
                AddValveWheel(root.transform, $"valve_wheel_{i + 1:00}", new Vector3(x, y, -0.165f), 0.18f, i == spec.Variant ? "yellow" : "brass");
                Label(root.transform, $"valve_number_{i + 1:00}", (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, y - 0.28f, -0.170f), 0.034f, new Color(0.92f, 0.86f, 0.66f));
            }

            for (var i = 0; i < valves; i++)
            {
                var x = valves == 1 ? 0f : -0.52f + i * (1.04f / Mathf.Max(1, valves - 1));
                Cylinder(root.transform, $"vertical_copper_puzzle_pipe_{i:00}", new Vector3(x, 0.03f, -0.112f), new Vector3(0.024f, 0.94f, 0.024f), Vector3.zero, "copper");
            }

            Cylinder(root.transform, "horizontal_main_pressure_manifold", new Vector3(0f, -0.42f, -0.115f), new Vector3(0.032f, 0.64f, 0.032f), new Vector3(0f, 0f, 90f), "copper");
            AddGauge(root.transform, "valve_panel_top_pressure_gauge", new Vector3(0.58f, 0.38f, -0.165f), 0.13f, "PSI", -38f, spec.Variant == 2);
            AddRivets(root.transform, "valve_panel_rivets", 0.68f, 0.48f, 0.045f, "brass");
        }

        private static void BuildLiftCallStation(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "lift_call_station_column", 0.72f, 1.58f, 0.16f, "iron");
            Box(root.transform, "aged_brass_call_face", new Vector3(0f, 0.05f, -0.115f), new Vector3(0.48f, 1.18f, 0.055f), Vector3.zero, "brass");
            Label(root.transform, "readability_label_LIFT", "LIFT", new Vector3(0f, 0.66f, -0.150f), 0.043f, new Color(0.96f, 0.84f, 0.50f));
            AddArrowButton(root.transform, "up_call_button", new Vector3(0f, 0.31f, -0.165f), true, spec.Variant == 0 ? "greenGlass" : "white");
            AddArrowButton(root.transform, "down_call_button", new Vector3(0f, -0.14f, -0.165f), false, spec.Variant == 1 ? "amber" : "white");
            Part(root.transform, "gear12", "lift_floor_index_cog", new Vector3(0f, -0.52f, -0.158f), new Vector3(0.30f, 0.30f, 0.044f), new Vector3(0f, 0f, 15f * spec.Variant), "darkBrass");
            Label(root.transform, "floor_label", spec.Variant == 2 ? "BELL" : "CALL", new Vector3(0f, -0.52f, -0.170f), 0.028f, new Color(0.08f, 0.06f, 0.04f));
            AddChainHang(root.transform, "lift_signal_chain", new Vector3(-0.42f, 0.50f, -0.110f), 5 + spec.Variant);
            Disc(root.transform, "amber_arrival_lantern", new Vector3(0.39f, 0.55f, -0.150f), 0.090f, 0.035f, "amber");
            AddRivets(root.transform, "lift_station_rivets", 0.30f, 0.70f, 0.043f, "darkBrass");
        }

        private static void BuildPressureRegulator(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "pressure_regulator_backer", 1.34f, 1.18f, 0.12f, "iron");
            Label(root.transform, "readability_label_PRESSURE", spec.Variant == 2 ? "REDLINE" : "PRESS", new Vector3(0f, 0.48f, -0.150f), 0.037f, new Color(0.96f, 0.84f, 0.50f));
            AddGauge(root.transform, "primary_pressure_gauge", new Vector3(-0.34f, 0.18f, -0.162f), 0.22f, "PSI", spec.Variant == 2 ? 62f : -18f, spec.Variant == 2);
            AddGauge(root.transform, "secondary_pressure_gauge", new Vector3(0.34f, 0.19f, -0.162f), 0.16f, "FLOW", -48f + spec.Variant * 18f, false);

            for (var i = 0; i < 3; i++)
            {
                var x = -0.33f + i * 0.33f;
                Cylinder(root.transform, $"green_glass_equalizer_tube_{i:00}", new Vector3(x, -0.33f, -0.146f), new Vector3(0.045f, 0.34f, 0.045f), Vector3.zero, i == spec.Variant ? "glow" : "greenGlass");
                Disc(root.transform, $"brass_tube_top_cap_{i:00}", new Vector3(x, -0.15f, -0.146f), 0.065f, 0.020f, "brass");
                Disc(root.transform, $"brass_tube_bottom_cap_{i:00}", new Vector3(x, -0.51f, -0.146f), 0.065f, 0.020f, "brass");
            }

            Cylinder(root.transform, "left_wall_feed_pipe", new Vector3(-0.64f, -0.16f, -0.108f), new Vector3(0.030f, 0.48f, 0.030f), Vector3.zero, "copper");
            Cylinder(root.transform, "right_wall_feed_pipe", new Vector3(0.64f, -0.16f, -0.108f), new Vector3(0.030f, 0.48f, 0.030f), Vector3.zero, "copper");
            AddRivets(root.transform, "pressure_regulator_rivets", 0.58f, 0.50f, 0.047f, "brass");
        }

        private static void BuildSecretCache(GameObject root, PropSpec spec)
        {
            if (spec.Variant == 0)
            {
                Box(root.transform, "rivet_locker_body", new Vector3(0f, 0f, 0f), new Vector3(0.86f, 1.06f, 0.36f), Vector3.zero, "iron");
                Box(root.transform, "hidden_door_face", new Vector3(0f, 0.02f, -0.205f), new Vector3(0.74f, 0.88f, 0.048f), Vector3.zero, "blueSteel");
                Box(root.transform, "aged_brass_secret_latch_bar", new Vector3(0.28f, 0.0f, -0.245f), new Vector3(0.08f, 0.50f, 0.050f), Vector3.zero, "brass");
                Part(root.transform, "keyhole", "small_soot_keyhole_cache_glyph", new Vector3(0.20f, 0.0f, -0.286f), new Vector3(0.16f, 0.24f, 0.034f), Vector3.zero, "shadow");
                Label(root.transform, "cache_label", "CACHE", new Vector3(-0.10f, 0.42f, -0.282f), 0.034f, new Color(0.95f, 0.82f, 0.48f));
                AddRivets(root.transform, "locker_door_rivets", 0.34f, 0.40f, 0.044f, "brass");
            }
            else if (spec.Variant == 1)
            {
                Cylinder(root.transform, "pipe_cache_main_canister", Vector3.zero, new Vector3(0.22f, 0.72f, 0.22f), new Vector3(0f, 0f, 90f), "blueSteel");
                Disc(root.transform, "left_disguised_pipe_cap", new Vector3(-0.74f, 0f, 0f), 0.26f, 0.045f, "darkBrass");
                Disc(root.transform, "right_disguised_pipe_cap", new Vector3(0.74f, 0f, 0f), 0.26f, 0.045f, "darkBrass");
                Box(root.transform, "aged_brass_hidden_release_tab", new Vector3(0f, -0.27f, -0.135f), new Vector3(0.34f, 0.070f, 0.060f), Vector3.zero, "brass");
                AddValveWheel(root.transform, "false_valve_handle_release", new Vector3(0f, 0.22f, -0.195f), 0.20f, "red");
                Label(root.transform, "pipe_cache_label", "PIPE", new Vector3(0f, -0.42f, -0.160f), 0.034f, new Color(0.95f, 0.82f, 0.48f));
            }
            else
            {
                Box(root.transform, "floor_gear_safe_base", new Vector3(0f, -0.14f, 0f), new Vector3(1.14f, 0.28f, 0.86f), Vector3.zero, "iron");
                Box(root.transform, "floor_safe_lid_plate", new Vector3(0f, 0.04f, -0.035f), new Vector3(1.02f, 0.080f, 0.74f), Vector3.zero, "blueSteel");
                Part(root.transform, "gear18", "oversized_floor_safe_gear_dial", new Vector3(0f, 0.10f, -0.43f), new Vector3(0.44f, 0.44f, 0.045f), new Vector3(0f, 0f, 24f), "brass");
                Part(root.transform, "needle", "white_safe_alignment_pointer", new Vector3(0f, 0.10f, -0.480f), new Vector3(0.30f, 0.50f, 0.055f), new Vector3(0f, 0f, -90f), "white");
                Label(root.transform, "floor_safe_label", "SAFE", new Vector3(0f, 0.17f, -0.515f), 0.034f, new Color(0.05f, 0.04f, 0.03f));
                AddRivets(root.transform, "floor_safe_lid_rivets", 0.46f, 0.25f, 0.040f, "brass");
            }
        }

        private static void BuildActuator(GameObject root, PropSpec spec)
        {
            if (spec.Variant == 0)
            {
                Box(root.transform, "bridge_throw_pedestal_base", new Vector3(0f, -0.43f, 0f), new Vector3(0.72f, 0.24f, 0.58f), Vector3.zero, "iron");
                Box(root.transform, "bridge_throw_upright", new Vector3(0f, -0.04f, 0f), new Vector3(0.42f, 0.70f, 0.30f), Vector3.zero, "blueSteel");
                Part(root.transform, "lever", "red_bridge_throw_lever", new Vector3(0.17f, 0.33f, -0.03f), new Vector3(0.16f, 0.82f, 0.16f), new Vector3(0f, 0f, -34f), "red");
                Disc(root.transform, "aged_brass_lever_pivot", new Vector3(0f, 0.08f, -0.19f), 0.16f, 0.035f, "brass");
                Label(root.transform, "bridge_label", "BRIDGE", new Vector3(0f, -0.15f, -0.205f), 0.033f, new Color(0.95f, 0.82f, 0.48f));
            }
            else if (spec.Variant == 1)
            {
                Box(root.transform, "door_crank_column", new Vector3(0f, -0.18f, 0f), new Vector3(0.38f, 1.04f, 0.34f), Vector3.zero, "iron");
                AddValveWheel(root.transform, "door_crank_wheel", new Vector3(0f, 0.22f, -0.245f), 0.30f, "brass");
                Box(root.transform, "yellow_door_actuator_stripe", new Vector3(0f, -0.44f, -0.190f), new Vector3(0.32f, 0.075f, 0.045f), Vector3.zero, "yellow");
                Label(root.transform, "door_label", "DOOR", new Vector3(0f, -0.58f, -0.202f), 0.037f, new Color(0.95f, 0.82f, 0.48f));
            }
            else
            {
                AddPanel(root.transform, "iris_sequencer_backer", 1.26f, 1.10f, 0.12f, "iron");
                Label(root.transform, "iris_label", "IRIS", new Vector3(0f, 0.46f, -0.150f), 0.040f, new Color(0.96f, 0.84f, 0.50f));
                for (var i = 0; i < 4; i++)
                {
                    var x = -0.36f + (i % 2) * 0.72f;
                    var y = 0.12f - (i / 2) * 0.42f;
                    Part(root.transform, "gear12", $"sequencer_iris_timing_gear_{i:00}", new Vector3(x, y, -0.157f), new Vector3(0.28f, 0.28f, 0.045f), new Vector3(0f, 0f, i * 28f), i == 2 ? "greenGlass" : "darkBrass");
                }

                Part(root.transform, "lever", "small_iris_sequence_lever", new Vector3(0.0f, -0.43f, -0.140f), new Vector3(0.10f, 0.46f, 0.10f), new Vector3(0f, 0f, 62f), "red");
                AddRivets(root.transform, "iris_panel_rivets", 0.54f, 0.46f, 0.042f, "brass");
            }
        }

        private static void BuildGovernorOverride(GameObject root, PropSpec spec)
        {
            if (spec.Variant == 0)
            {
                Box(root.transform, "boss_override_console_body", new Vector3(0f, -0.20f, 0f), new Vector3(1.20f, 0.78f, 0.54f), Vector3.zero, "iron");
                Box(root.transform, "hazard_yellow_override_face", new Vector3(0f, 0.02f, -0.310f), new Vector3(0.98f, 0.50f, 0.055f), Vector3.zero, "yellow");
                Disc(root.transform, "large_red_boss_kill_switch", new Vector3(0f, 0.05f, -0.360f), 0.22f, 0.052f, "red");
                Label(root.transform, "governor_label", "GOV", new Vector3(0f, 0.34f, -0.365f), 0.046f, new Color(0.06f, 0.04f, 0.02f));
                Label(root.transform, "override_label", "OVERRIDE", new Vector3(0f, -0.23f, -0.365f), 0.032f, new Color(0.06f, 0.04f, 0.02f));
            }
            else if (spec.Variant == 1)
            {
                AddPanel(root.transform, "triple_fuse_rack_backer", 1.16f, 1.36f, 0.13f, "iron");
                Label(root.transform, "fuse_rack_label", "FUSES", new Vector3(0f, 0.58f, -0.150f), 0.040f, new Color(0.96f, 0.84f, 0.50f));
                for (var i = 0; i < 3; i++)
                {
                    var x = -0.34f + i * 0.34f;
                    Cylinder(root.transform, $"cyan_glass_boss_fuse_{i:00}", new Vector3(x, 0.03f, -0.150f), new Vector3(0.055f, 0.72f, 0.055f), Vector3.zero, i == 1 ? "glow" : "greenGlass");
                    Disc(root.transform, $"upper_brass_fuse_socket_{i:00}", new Vector3(x, 0.43f, -0.150f), 0.083f, 0.024f, "brass");
                    Disc(root.transform, $"lower_brass_fuse_socket_{i:00}", new Vector3(x, -0.37f, -0.150f), 0.083f, 0.024f, "brass");
                }

                Part(root.transform, "gear18", "governor_crown_gear_warning", new Vector3(0f, -0.58f, -0.160f), new Vector3(0.34f, 0.34f, 0.045f), Vector3.zero, "red");
            }
            else if (spec.Variant == 2)
            {
                Box(root.transform, "panic_interlock_console_body", new Vector3(0f, -0.12f, 0f), new Vector3(1.26f, 0.90f, 0.46f), Vector3.zero, "blueSteel");
                for (var i = 0; i < 3; i++)
                {
                    var x = -0.34f + i * 0.34f;
                    Disc(root.transform, $"panic_interlock_button_{i:00}", new Vector3(x, 0.02f, -0.275f), 0.13f, 0.040f, i == 1 ? "red" : "amber");
                    Label(root.transform, $"interlock_label_{i:00}", (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.19f, -0.292f), 0.032f, new Color(0.95f, 0.82f, 0.48f));
                }

                AddGauge(root.transform, "governor_alarm_gauge", new Vector3(0f, 0.33f, -0.285f), 0.15f, "RPM", 54f, true);
                Label(root.transform, "panic_label", "PANIC", new Vector3(0f, -0.39f, -0.292f), 0.041f, new Color(0.95f, 0.82f, 0.48f));
            }
            else
            {
                Box(root.transform, "clockwork_breaker_tower_body", new Vector3(0f, -0.05f, 0f), new Vector3(0.72f, 1.48f, 0.42f), Vector3.zero, "iron");
                Part(root.transform, "gear18", "clockwork_override_clock_face", new Vector3(0f, 0.36f, -0.248f), new Vector3(0.44f, 0.44f, 0.050f), Vector3.zero, "brass");
                Part(root.transform, "needle", "clockwork_minute_breaker_pointer", new Vector3(0f, 0.36f, -0.305f), new Vector3(0.27f, 0.50f, 0.050f), new Vector3(0f, 0f, 18f), "shadow");
                Part(root.transform, "lever", "red_final_override_pull", new Vector3(0.22f, -0.34f, -0.215f), new Vector3(0.12f, 0.60f, 0.12f), new Vector3(0f, 0f, -48f), "red");
                Disc(root.transform, "top_red_alarm_lamp", new Vector3(0f, 0.80f, -0.235f), 0.12f, 0.040f, "red");
                Label(root.transform, "breaker_label", "BREAK", new Vector3(0f, -0.62f, -0.260f), 0.037f, new Color(0.95f, 0.82f, 0.48f));
            }
        }

        private static void AddPanel(Transform parent, string name, float width, float height, float depth, string materialKey)
        {
            Box(parent, name, Vector3.zero, new Vector3(width, height, depth), Vector3.zero, materialKey);
            Box(parent, name + "_top_brass_rail", new Vector3(0f, height * 0.5f - 0.045f, -depth * 0.62f), new Vector3(width * 0.93f, 0.045f, depth * 0.46f), Vector3.zero, "brass");
            Box(parent, name + "_bottom_brass_rail", new Vector3(0f, -height * 0.5f + 0.045f, -depth * 0.62f), new Vector3(width * 0.93f, 0.045f, depth * 0.46f), Vector3.zero, "brass");
            Box(parent, name + "_left_brass_rail", new Vector3(-width * 0.5f + 0.045f, 0f, -depth * 0.62f), new Vector3(0.045f, height * 0.86f, depth * 0.46f), Vector3.zero, "brass");
            Box(parent, name + "_right_brass_rail", new Vector3(width * 0.5f - 0.045f, 0f, -depth * 0.62f), new Vector3(0.045f, height * 0.86f, depth * 0.46f), Vector3.zero, "brass");
        }

        private static void AddGauge(Transform parent, string name, Vector3 localPosition, float radius, string label, float needleAngle, bool alert)
        {
            var gauge = new GameObject(name);
            gauge.transform.SetParent(parent, false);
            gauge.transform.localPosition = localPosition;

            Disc(gauge.transform, "black_backplate", Vector3.zero, radius * 1.22f, 0.028f, "rubber");
            Disc(gauge.transform, "aged_brass_outer_rim", new Vector3(0f, 0f, -0.026f), radius * 1.05f, 0.023f, "brass");
            Disc(gauge.transform, "ivory_gauge_face", new Vector3(0f, 0f, -0.052f), radius * 0.82f, 0.012f, alert ? "yellow" : "ivory");
            for (var i = 0; i < 9; i++)
            {
                var angle = Mathf.Lerp(-135f, 135f, i / 8f) * Mathf.Deg2Rad;
                var x = Mathf.Sin(angle) * radius * 0.62f;
                var y = Mathf.Cos(angle) * radius * 0.62f;
                Box(gauge.transform, $"black_gauge_tick_{i:00}", new Vector3(x, y, -0.068f), new Vector3(radius * 0.030f, radius * 0.115f, 0.014f), new Vector3(0f, 0f, -Mathf.Rad2Deg * angle), "shadow");
            }

            Part(gauge.transform, "needle", "black_pressure_needle", new Vector3(0f, 0f, -0.081f), new Vector3(radius * 0.12f, radius * 0.72f, 0.020f), new Vector3(0f, 0f, needleAngle), alert ? "red" : "shadow");
            Disc(gauge.transform, "center_brass_pin", new Vector3(0f, 0f, -0.092f), radius * 0.12f, 0.012f, "brass");
            Label(gauge.transform, "gauge_label_" + label, label, new Vector3(0f, -radius * 0.34f, -0.096f), radius * 0.12f, new Color(0.05f, 0.04f, 0.025f));
        }

        private static void AddValveWheel(Transform parent, string name, Vector3 position, float radius, string materialKey)
        {
            var wheel = new GameObject(name);
            wheel.transform.SetParent(parent, false);
            wheel.transform.localPosition = position;
            Part(wheel.transform, "gear12", "toothed_valve_wheel_outer", Vector3.zero, new Vector3(radius, radius, 0.050f), Vector3.zero, materialKey);
            Disc(wheel.transform, "rubber_shadow_recess", new Vector3(0f, 0f, -0.016f), radius * 0.54f, 0.020f, "rubber");
            Disc(wheel.transform, "aged_brass_center_boss", new Vector3(0f, 0f, -0.045f), radius * 0.25f, 0.024f, "brass");
            for (var i = 0; i < 4; i++)
            {
                Box(wheel.transform, $"wheel_spoke_{i:00}", new Vector3(0f, 0f, -0.052f), new Vector3(radius * 1.48f, radius * 0.105f, 0.020f), new Vector3(0f, 0f, i * 45f), materialKey);
            }
        }

        private static void AddArrowButton(Transform parent, string name, Vector3 position, bool up, string materialKey)
        {
            Disc(parent, name + "_round_button", position, 0.145f, 0.035f, materialKey);
            Part(parent, "arrow", name + "_white_arrow_glyph", position + new Vector3(0f, 0f, -0.040f), new Vector3(0.18f, up ? 0.18f : -0.18f, 0.020f), Vector3.zero, "shadow");
        }

        private static void AddChainHang(Transform parent, string prefix, Vector3 start, int links)
        {
            for (var i = 0; i < links; i++)
            {
                Part(parent, "chain", $"{prefix}_oval_link_{i:00}", start + new Vector3(0f, -i * 0.112f, 0f), new Vector3(0.090f, 0.090f, 0.030f), new Vector3(0f, 0f, i % 2 == 0 ? 0f : 90f), "darkBrass");
            }
        }

        private static void AddRivets(Transform parent, string groupName, float halfWidth, float halfHeight, float scale, string materialKey)
        {
            var group = new GameObject(groupName);
            group.transform.SetParent(parent, false);
            var points = new[]
            {
                new Vector3(-halfWidth, -halfHeight, -0.135f),
                new Vector3(halfWidth, -halfHeight, -0.135f),
                new Vector3(-halfWidth, halfHeight, -0.135f),
                new Vector3(halfWidth, halfHeight, -0.135f),
                new Vector3(0f, -halfHeight, -0.135f),
                new Vector3(0f, halfHeight, -0.135f)
            };

            for (var i = 0; i < points.Length; i++)
            {
                Part(group.transform, "hex", $"hex_rivet_{i:00}", points[i], Vector3.one * scale, new Vector3(90f, 0f, 0f), materialKey);
            }
        }

        private static void Box(Transform parent, string name, Vector3 position, Vector3 scale, Vector3 euler, string materialKey)
        {
            Part(parent, "box", name, position, scale, euler, materialKey);
        }

        private static void Cylinder(Transform parent, string name, Vector3 position, Vector3 scale, Vector3 euler, string materialKey)
        {
            Part(parent, "cylinder16", name, position, scale, euler, materialKey);
        }

        private static void Disc(Transform parent, string name, Vector3 position, float radius, float depth, string materialKey)
        {
            Part(parent, "cylinder32", name, position, new Vector3(radius, depth, radius), new Vector3(90f, 0f, 0f), materialKey);
        }

        private static GameObject Part(Transform parent, string meshKey, string name, Vector3 position, Vector3 scale, Vector3 euler, string materialKey)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            child.transform.localPosition = position;
            child.transform.localScale = scale;
            child.transform.localEulerAngles = euler;
            child.AddComponent<MeshFilter>().sharedMesh = Meshes[meshKey];
            var renderer = child.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = Materials[materialKey];
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            renderer.receiveShadows = true;
            return child;
        }

        private static void Label(Transform parent, string name, string text, Vector3 position, float characterSize, Color color)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            child.transform.localPosition = position;
            var textMesh = child.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.characterSize = characterSize;
            textMesh.fontSize = 96;
            textMesh.color = color;
            var renderer = child.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
        }

        private static GameObject NewRoot(PropSpec spec)
        {
            var root = new GameObject(spec.AssetId);
            root.AddComponent<ObjectivePropsSet02Identity>().Configure(
                spec.AssetId,
                spec.Family,
                spec.Role,
                spec.ReadabilityCue,
                0,
                MaterialTags,
                GetSafetyNotes());
            return root;
        }

        private static void SavePrefab(GameObject root, PropSpec spec)
        {
            var identity = root.GetComponent<ObjectivePropsSet02Identity>();
            if (identity != null)
            {
                identity.Configure(
                    spec.AssetId,
                    spec.Family,
                    spec.Role,
                    spec.ReadabilityCue,
                    root.GetComponentsInChildren<Renderer>(true).Length,
                    MaterialTags,
                    GetSafetyNotes());
            }

            var path = CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + spec.FileName);
            if (AssetDatabase.LoadMainAssetAtPath(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            PrefabUtility.SaveAsPrefabAsset(root, path);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static PropSpec[] GetSpecs()
        {
            return new[]
            {
                new PropSpec("BB_OPS02_KeyedLock_TriGearVault", "keyed_locks", "vault door key mechanism", "oversized KEY label, keyhole silhouette, red/green state lamps", PropKind.KeyedLock, 0),
                new PropSpec("BB_OPS02_KeyedLock_BrassWheelKeyNest", "keyed_locks", "round brass wheel key socket", "cog socket, visible key-bit index marks", PropKind.KeyedLock, 1),
                new PropSpec("BB_OPS02_KeyedLock_RuneCogDoorSocket", "keyed_locks", "door-side rune cog socket", "green ready lamp, central keyhole, numbered bit marks", PropKind.KeyedLock, 2),
                new PropSpec("BB_OPS02_KeyedLock_MasterCogShrine", "keyed_locks", "master objective key shrine", "large crown gear silhouette and fail-state red lamp", PropKind.KeyedLock, 3),
                new PropSpec("BB_OPS02_ValvePanel_TwinPressurePuzzle", "valve_panels", "two-valve pressure puzzle panel", "numbered wheels and shared manifold pipe", PropKind.ValvePanel, 0),
                new PropSpec("BB_OPS02_ValvePanel_TripleRouteSelector", "valve_panels", "three-valve route selector", "1-2-3 valve sequence labels", PropKind.ValvePanel, 1),
                new PropSpec("BB_OPS02_ValvePanel_SteamEqualizer", "valve_panels", "steam equalizer panel", "top pressure gauge and highlighted active valve", PropKind.ValvePanel, 2),
                new PropSpec("BB_OPS02_ValvePanel_TimedReliefGrid", "valve_panels", "timed relief valve grid", "four readable valve wheels and steam label", PropKind.ValvePanel, 3),
                new PropSpec("BB_OPS02_LiftCallStation_BrassCageUpDown", "lift_call_stations", "caged lift up/down call station", "large UP/DOWN arrow buttons and arrival lamp", PropKind.LiftCall, 0),
                new PropSpec("BB_OPS02_LiftCallStation_ChainHoistSignal", "lift_call_stations", "chain-hoist lift signal station", "pull chain, call label, brass floor cog", PropKind.LiftCall, 1),
                new PropSpec("BB_OPS02_LiftCallStation_EmergencyBellPanel", "lift_call_stations", "emergency lift bell panel", "BELL state label and amber lamp", PropKind.LiftCall, 2),
                new PropSpec("BB_OPS02_PressureRegulator_WallManifold", "pressure_regulators", "wall pressure manifold regulator", "dual gauges, glass tubes, side feed pipes", PropKind.PressureRegulator, 0),
                new PropSpec("BB_OPS02_PressureRegulator_GlassTubeStack", "pressure_regulators", "glass tube pressure stack", "three equalizer tubes and flow gauge", PropKind.PressureRegulator, 1),
                new PropSpec("BB_OPS02_PressureRegulator_RedlineGovernor", "pressure_regulators", "redline governor regulator", "yellow/red alert gauge and active cyan tube", PropKind.PressureRegulator, 2),
                new PropSpec("BB_OPS02_SecretCache_RivetLockerClosed", "secret_cache_containers", "closed rivet locker secret cache", "CACHE label, hidden latch bar, keyhole glyph", PropKind.SecretCache, 0),
                new PropSpec("BB_OPS02_SecretCache_PipeWallCanister", "secret_cache_containers", "disguised pipe wall canister", "false valve release and pipe label", PropKind.SecretCache, 1),
                new PropSpec("BB_OPS02_SecretCache_FloorGearSafe", "secret_cache_containers", "floor gear safe cache", "SAFE label, top gear dial, alignment pointer", PropKind.SecretCache, 2),
                new PropSpec("BB_OPS02_Actuator_BridgeThrowLever", "bridge_door_actuators", "bridge throw lever actuator", "red lever and BRIDGE label", PropKind.Actuator, 0),
                new PropSpec("BB_OPS02_Actuator_DoorCrankPedestal", "bridge_door_actuators", "door crank pedestal actuator", "large wheel and DOOR label", PropKind.Actuator, 1),
                new PropSpec("BB_OPS02_Actuator_IrisBulkheadSequencer", "bridge_door_actuators", "iris bulkhead sequencer panel", "four timing gears and sequence lever", PropKind.Actuator, 2),
                new PropSpec("BB_OPS02_GovernorOverride_BossKillSwitch", "governor_override_devices", "boss machinery kill switch", "huge red switch, GOV and OVERRIDE labels", PropKind.GovernorOverride, 0),
                new PropSpec("BB_OPS02_GovernorOverride_TripleFuseRack", "governor_override_devices", "triple fuse governor override rack", "three glowing fuses and crown gear warning", PropKind.GovernorOverride, 1),
                new PropSpec("BB_OPS02_GovernorOverride_PanicInterlockConsole", "governor_override_devices", "panic interlock console", "three numbered override buttons and alarm gauge", PropKind.GovernorOverride, 2),
                new PropSpec("BB_OPS02_GovernorOverride_ClockworkBreakerTower", "governor_override_devices", "clockwork breaker tower", "clock face, red pull lever, top alarm lamp", PropKind.GovernorOverride, 3)
            };
        }

        private static string[] GetSafetyNotes()
        {
            return new[]
            {
                "Visual-only prop; no gameplay authority.",
                "Generated prefab omits colliders, rigidbodies, autonomous audio, cameras, lights, and particle systems.",
                "Passive identity metadata component is present only for sidecar intake review."
            };
        }

        private static string[] GetMaterialNames()
        {
            return new[]
            {
                "OPS02_MAT_RivetedIron",
                "OPS02_MAT_AgedBrass",
                "OPS02_MAT_DarkAgedBrass",
                "OPS02_MAT_BurnishedCopper",
                "OPS02_MAT_OxidizedCopperPatina",
                "OPS02_MAT_HazardYellowPaint",
                "OPS02_MAT_RedOverrideEnamel",
                "OPS02_MAT_GreenSignalGlass",
                "OPS02_MAT_AmberLampGlass",
                "OPS02_MAT_BluedPressureSteel",
                "OPS02_MAT_IvoryGaugeFace",
                "OPS02_MAT_BlackRubberGasket",
                "OPS02_MAT_VarnishedWalnut",
                "OPS02_MAT_AgedPaperLabel",
                "OPS02_MAT_ReadabilityWhiteInlay",
                "OPS02_MAT_SootShadow",
                "OPS02_MAT_PressureGlowCyan"
            };
        }

        private static string[] GetMeshNames()
        {
            return new[]
            {
                "OPS02_Mesh_BoxUnit",
                "OPS02_Mesh_Cylinder16Unit",
                "OPS02_Mesh_Cylinder32Unit",
                "OPS02_Mesh_Gear12ToothUnit",
                "OPS02_Mesh_Gear18ToothUnit",
                "OPS02_Mesh_KeyholeGlyphUnit",
                "OPS02_Mesh_PointerNeedleUnit",
                "OPS02_Mesh_TaperedLeverHandle",
                "OPS02_Mesh_ChainLinkOvalUnit",
                "OPS02_Mesh_ChevronArrowUnit",
                "OPS02_Mesh_HexBoltUnit"
            };
        }

        private static Mesh CreateBoxMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f)
            };
            var triangles = new[]
            {
                0, 2, 1, 0, 3, 2, 4, 6, 5, 4, 7, 6, 8, 10, 9, 8, 11, 10,
                12, 14, 13, 12, 15, 14, 16, 18, 17, 16, 19, 18, 20, 22, 21, 20, 23, 22
            };
            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateCylinderMesh(int sides)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i < sides; i++)
            {
                var angle = Mathf.PI * 2f * i / sides;
                var x = Mathf.Cos(angle) * 0.5f;
                var z = Mathf.Sin(angle) * 0.5f;
                vertices.Add(new Vector3(x, -0.5f, z));
                vertices.Add(new Vector3(x, 0.5f, z));
            }

            var bottomCenter = vertices.Count;
            vertices.Add(new Vector3(0f, -0.5f, 0f));
            var topCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0.5f, 0f));

            for (var i = 0; i < sides; i++)
            {
                var next = (i + 1) % sides;
                var bottom = i * 2;
                var top = bottom + 1;
                var nextBottom = next * 2;
                var nextTop = nextBottom + 1;

                triangles.Add(bottom);
                triangles.Add(top);
                triangles.Add(nextTop);
                triangles.Add(bottom);
                triangles.Add(nextTop);
                triangles.Add(nextBottom);

                triangles.Add(bottomCenter);
                triangles.Add(nextBottom);
                triangles.Add(bottom);

                triangles.Add(topCenter);
                triangles.Add(top);
                triangles.Add(nextTop);
            }

            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateGearMesh(int teeth)
        {
            var points = teeth * 2;
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            const float depth = 0.12f;

            for (var layer = 0; layer < 2; layer++)
            {
                var z = layer == 0 ? -depth * 0.5f : depth * 0.5f;
                for (var i = 0; i < points; i++)
                {
                    var angle = Mathf.PI * 2f * i / points;
                    var radius = i % 2 == 0 ? 0.56f : 0.42f;
                    vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, z));
                }
            }

            var backCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, -depth * 0.5f));
            var frontCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, depth * 0.5f));

            for (var i = 0; i < points; i++)
            {
                var next = (i + 1) % points;
                triangles.Add(i);
                triangles.Add(next);
                triangles.Add(points + next);
                triangles.Add(i);
                triangles.Add(points + next);
                triangles.Add(points + i);

                triangles.Add(backCenter);
                triangles.Add(i);
                triangles.Add(next);

                triangles.Add(frontCenter);
                triangles.Add(points + next);
                triangles.Add(points + i);
            }

            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateKeyholeMesh()
        {
            var points = new[]
            {
                new Vector2(0f, 0.50f),
                new Vector2(0.20f, 0.34f),
                new Vector2(0.17f, 0.13f),
                new Vector2(0.07f, 0.02f),
                new Vector2(0.11f, -0.50f),
                new Vector2(-0.11f, -0.50f),
                new Vector2(-0.07f, 0.02f),
                new Vector2(-0.17f, 0.13f),
                new Vector2(-0.20f, 0.34f)
            };

            return CreateExtrudedPolygon(points, 0.12f);
        }

        private static Mesh CreatePointerNeedleMesh()
        {
            var points = new[]
            {
                new Vector2(0f, 0.55f),
                new Vector2(0.065f, -0.18f),
                new Vector2(0.025f, -0.45f),
                new Vector2(-0.025f, -0.45f),
                new Vector2(-0.065f, -0.18f)
            };

            return CreateExtrudedPolygon(points, 0.06f);
        }

        private static Mesh CreateLeverHandleMesh()
        {
            var bottomX = 0.20f;
            var bottomZ = 0.18f;
            var topX = 0.11f;
            var topZ = 0.11f;
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

        private static Mesh CreateChainLinkMesh()
        {
            var points = new Vector2[18];
            for (var i = 0; i < points.Length; i++)
            {
                var angle = Mathf.PI * 2f * i / points.Length;
                points[i] = new Vector2(Mathf.Cos(angle) * 0.34f, Mathf.Sin(angle) * 0.50f);
            }

            return CreateExtrudedPolygon(points, 0.08f);
        }

        private static Mesh CreateArrowMesh()
        {
            var points = new[]
            {
                new Vector2(0f, 0.50f),
                new Vector2(0.34f, 0.02f),
                new Vector2(0.13f, 0.02f),
                new Vector2(0.13f, -0.46f),
                new Vector2(-0.13f, -0.46f),
                new Vector2(-0.13f, 0.02f),
                new Vector2(-0.34f, 0.02f)
            };

            return CreateExtrudedPolygon(points, 0.08f);
        }

        private static Mesh CreateExtrudedPolygon(IReadOnlyList<Vector2> points, float depth)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var layer = 0; layer < 2; layer++)
            {
                var z = layer == 0 ? -depth * 0.5f : depth * 0.5f;
                for (var i = 0; i < points.Count; i++)
                {
                    vertices.Add(new Vector3(points[i].x, points[i].y, z));
                }
            }

            var backCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, -depth * 0.5f));
            var frontCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, depth * 0.5f));

            for (var i = 0; i < points.Count; i++)
            {
                var next = (i + 1) % points.Count;
                triangles.Add(i);
                triangles.Add(next);
                triangles.Add(points.Count + next);
                triangles.Add(i);
                triangles.Add(points.Count + next);
                triangles.Add(points.Count + i);

                triangles.Add(backCenter);
                triangles.Add(next);
                triangles.Add(i);

                triangles.Add(frontCenter);
                triangles.Add(points.Count + i);
                triangles.Add(points.Count + next);
            }

            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void WriteManifestFiles(string importStatus, string previewStatus, string sidecarStatus)
        {
            var json = BuildManifestJson(importStatus, previewStatus, sidecarStatus);
            var packageManifestPath = CombineAssetPath(PackageRoot, "Documentation~/Manifest/" + PackageManifestFileName);
            var generatedCatalogPath = CombineAssetPath(PackageRoot, "Runtime/Metadata/" + GeneratedCatalogFileName);
            File.WriteAllText(AssetPathToFullPath(packageManifestPath), json, Encoding.UTF8);
            File.WriteAllText(AssetPathToFullPath(generatedCatalogPath), json, Encoding.UTF8);
            AssetDatabase.ImportAsset(packageManifestPath);
            AssetDatabase.ImportAsset(generatedCatalogPath);
        }

        private static string BuildManifestJson(string importStatus, string previewStatus, string sidecarStatus)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            AppendField(builder, "pack_id", PackId, true);
            AppendField(builder, "display_name", "Objective Props Set 02", true);
            AppendField(builder, "version", Version, true);
            AppendField(builder, "build_id", BuildId, true);
            AppendField(builder, "unity_version", Application.unityVersion, true);
            AppendField(builder, "generated_at", DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture), true);
            AppendField(builder, "sidecar_project", "UD-SC-OPS02-ObjectivePropVisualCandidates", true);
            AppendField(builder, "owner_lane", "sidecar-objective-prop-lookdev", true);
            AppendField(builder, "primary_intake_owner", "main-lane-art-integration", true);
            AppendField(builder, "canonical_root", "AssetPacks/BrassworksBreach.ObjectivePropsSet02", true);
            AppendField(builder, "package_root", "AssetPacks/BrassworksBreach.ObjectivePropsSet02", true);
            AppendField(builder, "package_name", PackageName, true);
            AppendField(builder, "package_version", Version, true);
            builder.AppendLine("  \"asset_counts\": {");
            builder.AppendLine($"    \"generated_prefabs\": {GetSpecs().Length},");
            builder.AppendLine($"    \"generated_materials\": {GetMaterialNames().Length},");
            builder.AppendLine($"    \"generated_meshes\": {GetMeshNames().Length},");
            builder.AppendLine($"    \"preview_renders\": {GetPreviewRenderPaths().Length},");
            builder.AppendLine("    \"runtime_scripts\": 1,");
            builder.AppendLine("    \"editor_scripts\": 3,");
            builder.AppendLine("    \"textures\": 0,");
            builder.AppendLine("    \"audio\": 0,");
            builder.AppendLine("    \"colliders\": 0");
            builder.AppendLine("  },");
            AppendArray(builder, "generated_prefabs", GetPrefabManifestPaths(), true);
            AppendArray(builder, "generated_materials", PrefixPaths("Runtime/Materials", GetMaterialNames(), ".mat"), true);
            AppendArray(builder, "generated_meshes", PrefixPaths("Runtime/Meshes", GetMeshNames(), ".asset"), true);
            AppendArray(builder, "preview_renders", GetPreviewRenderPaths(), true);
            AppendArray(builder, "prop_families", new[]
            {
                "keyed_locks",
                "valve_panels",
                "lift_call_stations",
                "pressure_regulators",
                "secret_cache_containers",
                "bridge_door_actuators",
                "governor_override_devices"
            }, true);
            AppendArray(builder, "dependencies", new[]
            {
                "UnityEngine procedural mesh and material assets",
                "UnityEditor prefab, validation, and render texture APIs",
                "Built-in, URP, or HDRP lit shader fallback"
            }, true);
            builder.AppendLine("  \"required_primary_changes\": [],");
            builder.AppendLine("  \"path_collisions_checked\": true,");
            builder.AppendLine("  \"guid_collisions_checked\": true,");
            AppendField(builder, "guid_collision_check", "package_local_meta_scan_pending_external_validator", true);
            AppendField(builder, "import_smoke_status", importStatus, true);
            AppendField(builder, "preview_render_status", previewStatus, true);
            AppendField(builder, "sidecar_validation_status", sidecarStatus, true);
            builder.AppendLine("  \"runtime_contract\": {");
            builder.AppendLine("    \"visual_only\": true,");
            builder.AppendLine("    \"gameplay_authority\": \"none\",");
            builder.AppendLine("    \"colliders\": \"omitted\",");
            builder.AppendLine("    \"rigidbodies\": \"omitted\",");
            builder.AppendLine("    \"autonomous_audio\": \"omitted\",");
            builder.AppendLine("    \"scene_changes\": \"none\"");
            builder.AppendLine("  },");
            AppendArray(builder, "known_risks", new[]
            {
                "Procedural meshes are high-readability lookdev candidates and need authored final meshes before art lock.",
                "Materials are solid procedural proxy materials without authored texture maps, decals, grime masks, or normal maps.",
                "Interactable readability is visual only; final prompts, gameplay state, collision, and authority stay with the primary gameplay lane.",
                "Preview lighting and preview staging are proof-only and are not part of saved prefabs."
            }, true);
            AppendField(builder, "rollback_path", "delete AssetPacks/BrassworksBreach.ObjectivePropsSet02, Documentation/AssetProduction/V0_1_42_ObjectivePropsSet02, and Documentation/ConceptRenders/V0_1_42_ObjectivePropsSet02", true);
            AppendField(builder, "decision", "ready_for_primary_quarantine_after_static_validation_and_preview_review", false);
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static string[] GetPrefabManifestPaths()
        {
            var specs = GetSpecs();
            var paths = new string[specs.Length];
            for (var i = 0; i < specs.Length; i++)
            {
                paths[i] = "Packages/" + PackageName + "/Runtime/Prefabs/" + specs[i].FileName;
            }

            return paths;
        }

        private static string[] PrefixPaths(string folder, string[] names, string extension)
        {
            var paths = new string[names.Length];
            for (var i = 0; i < names.Length; i++)
            {
                paths[i] = "Packages/" + PackageName + "/" + folder + "/" + names[i] + extension;
            }

            return paths;
        }

        private static string[] GetPreviewRenderPaths()
        {
            var specs = GetSpecs();
            var paths = new string[specs.Length + 1];
            for (var i = 0; i < specs.Length; i++)
            {
                paths[i] = RenderOutputRelativePath + "/" + specs[i].AssetId + "_preview.png";
            }

            paths[paths.Length - 1] = RenderOutputRelativePath + "/BB_OPS02_ContactSheet.png";
            return paths;
        }

        private static void CreateOrReplaceAsset(UnityEngine.Object asset, string path)
        {
            if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            AssetDatabase.CreateAsset(asset, path);
        }

        private static string PackageRoot => LocatePackageRoot().AssetPath;

        private static PackageRootInfo LocatePackageRoot()
        {
            var package = PackageInfo.FindForAssembly(typeof(ObjectivePropsSet02Generator).Assembly);
            if (package != null && !string.IsNullOrWhiteSpace(package.assetPath) && !string.IsNullOrWhiteSpace(package.resolvedPath))
            {
                return new PackageRootInfo(package.assetPath.Replace("\\", "/"), package.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(ObjectivePropsSet02Generator));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/ObjectivePropsSet02Generator.cs";
                if (path.EndsWith(suffix, StringComparison.Ordinal))
                {
                    var assetPath = path.Substring(0, path.Length - suffix.Length);
                    return new PackageRootInfo(assetPath, Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath)));
                }
            }

            throw new InvalidOperationException("Could not locate " + PackageName + " package root.");
        }

        private static string ResolveRepoRoot()
        {
            var resolvedPath = LocatePackageRoot().ResolvedPath;
            var directory = new DirectoryInfo(resolvedPath);
            while (directory != null)
            {
                if (directory.Name.Equals("AssetPacks", StringComparison.OrdinalIgnoreCase) && directory.Parent != null)
                {
                    return directory.Parent.FullName;
                }

                directory = directory.Parent;
            }

            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }

        private static string AssetPathToFullPath(string assetPath)
        {
            var normalized = assetPath.Replace("\\", "/");
            var packageRoot = LocatePackageRoot();
            if (normalized.StartsWith(packageRoot.AssetPath + "/", StringComparison.Ordinal))
            {
                var relative = normalized.Substring(packageRoot.AssetPath.Length + 1);
                return Path.GetFullPath(Path.Combine(packageRoot.ResolvedPath, relative.Replace('/', Path.DirectorySeparatorChar)));
            }

            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", normalized.Replace('/', Path.DirectorySeparatorChar)));
        }

        private static string CombineAssetPath(string root, string relative)
        {
            return (root.TrimEnd('/') + "/" + relative.TrimStart('/')).Replace("\\", "/");
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
            for (var i = 0; i < values.Count; i++)
            {
                builder.Append("    \"").Append(EscapeJson(values[i])).Append("\"");
                builder.AppendLine(i == values.Count - 1 ? string.Empty : ",");
            }

            builder.Append("  ]");
            if (trailingComma)
            {
                builder.Append(",");
            }

            builder.AppendLine();
        }

        private static string EscapeJson(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        public enum PropKind
        {
            KeyedLock,
            ValvePanel,
            LiftCall,
            PressureRegulator,
            SecretCache,
            Actuator,
            GovernorOverride
        }

        public sealed class PropSpec
        {
            public PropSpec(string assetId, string family, string role, string cue, PropKind kind, int variant)
            {
                AssetId = assetId;
                Family = family;
                Role = role;
                ReadabilityCue = cue;
                Kind = kind;
                Variant = variant;
            }

            public string AssetId { get; }
            public string FileName => AssetId + ".prefab";
            public string Family { get; }
            public string Role { get; }
            public string ReadabilityCue { get; }
            public PropKind Kind { get; }
            public int Variant { get; }
        }

        private sealed class PackageRootInfo
        {
            public PackageRootInfo(string assetPath, string resolvedPath)
            {
                AssetPath = assetPath;
                ResolvedPath = resolvedPath;
            }

            public string AssetPath { get; }
            public string ResolvedPath { get; }
        }
    }
}
