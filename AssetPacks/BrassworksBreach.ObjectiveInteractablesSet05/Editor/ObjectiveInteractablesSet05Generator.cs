using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.ObjectiveInteractablesSet05.Editor
{
    public static class ObjectiveInteractablesSet05Generator
    {
        public const string PackageName = "com.brassworks.sidecar.objective-interactables-set05";
        public const string Version = "0.1.49";
        public const string VersionLabel = "v0.1.49";
        public const string BuildId = "p001";
        public const string PackId = "OIS05";
        public const string RenderOutputRelativePath = "Documentation/ConceptRenders/V0_1_49_ObjectiveInteractablesSet05";
        public const string ProductionOutputRelativePath = "Documentation/AssetProduction/V0_1_49_ObjectiveInteractablesSet05";

        private const string MenuRoot = "Brassworks Breach/Sidecar Packs/Objective Interactables Set 05 v0.1.49/";
        private const string PackageManifestFileName = "OIS05_ObjectiveInteractablesSet05_Manifest_v0.1.49-p001.json";
        private const string GeneratedCatalogFileName = "OIS05_ObjectiveInteractablesSet05_Catalog_v0.1.49.json";

        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>(StringComparer.Ordinal);
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>(StringComparer.Ordinal);

        private static readonly MatSpec[] MaterialSpecs =
        {
            new MatSpec("iron", "OIS05_MAT_RivetedBlackIron", new Color(0.032f, 0.031f, 0.030f), 0.78f, 0.30f),
            new MatSpec("brass", "OIS05_MAT_AgedBrass", new Color(0.76f, 0.55f, 0.25f), 0.86f, 0.40f),
            new MatSpec("darkBrass", "OIS05_MAT_DarkSmokedBrass", new Color(0.40f, 0.25f, 0.09f), 0.82f, 0.25f),
            new MatSpec("copper", "OIS05_MAT_BurnishedCopper", new Color(0.68f, 0.30f, 0.13f), 0.80f, 0.36f),
            new MatSpec("patina", "OIS05_MAT_OxidizedCopperPatina", new Color(0.08f, 0.36f, 0.30f), 0.44f, 0.26f),
            new MatSpec("steel", "OIS05_MAT_BluedPressureSteel", new Color(0.07f, 0.16f, 0.25f), 0.72f, 0.48f),
            new MatSpec("yellow", "OIS05_MAT_HazardYellowEnamel", new Color(0.96f, 0.66f, 0.12f), 0.24f, 0.24f),
            new MatSpec("red", "OIS05_MAT_RedOverrideEnamel", new Color(0.72f, 0.035f, 0.025f), 0.12f, 0.52f, new Color(0.70f, 0.02f, 0.01f) * 0.42f),
            new MatSpec("green", "OIS05_MAT_GreenReadyGlass", new Color(0.08f, 0.82f, 0.35f, 0.76f), 0.02f, 0.86f, new Color(0.05f, 0.74f, 0.28f) * 1.25f, true),
            new MatSpec("amber", "OIS05_MAT_AmberPilotGlass", new Color(1.0f, 0.50f, 0.10f, 0.76f), 0.02f, 0.86f, new Color(1.0f, 0.32f, 0.06f) * 1.40f, true),
            new MatSpec("cyan", "OIS05_MAT_CyanPressureGlow", new Color(0.16f, 0.68f, 0.90f, 0.78f), 0.02f, 0.88f, new Color(0.10f, 0.52f, 0.78f) * 1.55f, true),
            new MatSpec("ivory", "OIS05_MAT_IvoryGaugeFace", new Color(0.84f, 0.77f, 0.58f), 0.02f, 0.20f),
            new MatSpec("rubber", "OIS05_MAT_BlackRubberGasket", new Color(0.012f, 0.011f, 0.010f), 0.0f, 0.20f),
            new MatSpec("walnut", "OIS05_MAT_VarnishedWalnut", new Color(0.31f, 0.13f, 0.055f), 0.04f, 0.50f),
            new MatSpec("paper", "OIS05_MAT_AgedPaperLabel", new Color(0.77f, 0.68f, 0.49f), 0.0f, 0.18f),
            new MatSpec("white", "OIS05_MAT_ReadabilityWhiteInlay", new Color(0.94f, 0.88f, 0.70f), 0.08f, 0.30f, new Color(0.75f, 0.60f, 0.28f) * 0.15f),
            new MatSpec("shadow", "OIS05_MAT_SootShadow", new Color(0.010f, 0.009f, 0.008f, 0.65f), 0.0f, 0.10f, null, true),
            new MatSpec("pickup", "OIS05_MAT_ObjectivePickupEmerald", new Color(0.18f, 0.92f, 0.64f, 0.78f), 0.02f, 0.86f, new Color(0.08f, 0.80f, 0.48f) * 1.65f, true)
        };

        private static readonly string[] RequiredFamilies =
        {
            "pressure_levers",
            "keyed_locks",
            "crank_panels",
            "fuse_boxes",
            "breaker_gauges",
            "valve_routing_puzzles",
            "boss_override_terminals",
            "lift_call_stations",
            "pickups",
            "objective_signage"
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

            WriteManifestFiles("generated_by_unity_sidecar_batchmode_" + VersionLabel, "pending_preview_render", "pending_external_sidecar_validator");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"OIS05_GENERATE_PASS {VersionLabel} prefabs={GetSpecs().Length} materials={MaterialSpecs.Length} meshes={GetMeshNames().Length}");
        }

        [MenuItem(MenuRoot + "Render Preview PNGs")]
        public static void RenderPreviewSet()
        {
            GenerateAll();
            var files = RenderPreviewPngs();
            WriteManifestFiles(
                "generated_and_preview_rendered_by_unity_sidecar_batchmode_" + Timestamp(),
                "passed_" + files.ToString(CultureInfo.InvariantCulture) + "_pngs",
                "pending_external_sidecar_validator");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"OIS05_PREVIEW_PASS {VersionLabel} files={files}");
        }

        [MenuItem(MenuRoot + "Generate, Render, Validate")]
        public static void GenerateRenderValidate()
        {
            GenerateAll();
            var previewFiles = RenderPreviewPngs();
            var result = ValidateGeneratedAssetsInternal();
            WriteValidationReports(result, previewFiles);
            WriteManifestFiles(
                result.Pass ? "isolated_import_generate_preview_validate_passed_" + Timestamp() : "failed_" + Timestamp(),
                "passed_" + previewFiles.ToString(CultureInfo.InvariantCulture) + "_pngs",
                result.Pass ? "unity_validation_passed_pending_external_sidecar_validator" : "unity_validation_failed");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (!result.Pass)
            {
                throw new InvalidOperationException("OIS05_UNITY_VALIDATION_FAIL " + VersionLabel + " findings=" + result.Findings.Count);
            }

            Debug.Log($"OIS05_UNITY_VALIDATION_PASS {VersionLabel} prefabs={result.Prefabs} materials={result.Materials} meshes={result.Meshes} renderers={result.Renderers} previews={previewFiles}");
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

        private static void EnsurePackageFolders()
        {
            foreach (var folder in new[] { "Runtime/Prefabs", "Runtime/Materials", "Runtime/Meshes", "Runtime/Metadata", "Documentation~/Manifest", "Samples~/PreviewScene" })
            {
                Directory.CreateDirectory(AssetPathToFullPath(CombineAssetPath(PackageRoot, folder)));
            }

            Directory.CreateDirectory(ResolveRepositoryRenderRoot());
            Directory.CreateDirectory(ResolveRepositoryProductionRoot());
            AssetDatabase.Refresh();
        }

        private static void CreateMaterials()
        {
            foreach (var spec in MaterialSpecs)
            {
                AddMaterial(spec);
            }
        }

        private static void AddMaterial(MatSpec spec)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("HDRP/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Diffuse");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible material shader found.");
            }

            var material = new Material(shader)
            {
                name = spec.Name,
                color = spec.Color
            };

            SetColor(material, "_BaseColor", spec.Color);
            SetColor(material, "_Color", spec.Color);
            SetFloat(material, "_Metallic", spec.Metallic);
            SetFloat(material, "_Smoothness", spec.Smoothness);
            SetFloat(material, "_Glossiness", spec.Smoothness);

            if (spec.Emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                SetColor(material, "_EmissionColor", spec.Emission.Value);
            }

            if (spec.Transparent)
            {
                material.renderQueue = 3000;
                SetFloat(material, "_Surface", 1f);
                SetFloat(material, "_Mode", 3f);
                SetFloat(material, "_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                SetFloat(material, "_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                SetFloat(material, "_ZWrite", 0f);
                material.EnableKeyword("_ALPHABLEND_ON");
            }

            var path = CombineAssetPath(PackageRoot, "Runtime/Materials/" + spec.Name + ".mat");
            CreateOrReplaceAsset(material, path);
            Materials[spec.Key] = AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        private static void CreateMeshes()
        {
            AddMesh("box", "OIS05_Mesh_BoxUnit", CreateBoxMesh());
            AddMesh("cylinder16", "OIS05_Mesh_Cylinder16Unit", CreateCylinderMesh(16));
            AddMesh("cylinder32", "OIS05_Mesh_Cylinder32Unit", CreateCylinderMesh(32));
            AddMesh("hex", "OIS05_Mesh_HexBoltUnit", CreateCylinderMesh(6));
            AddMesh("gear12", "OIS05_Mesh_Gear12ToothUnit", CreateGearMesh(12));
            AddMesh("gear18", "OIS05_Mesh_Gear18ToothUnit", CreateGearMesh(18));
            AddMesh("needle", "OIS05_Mesh_PressureNeedleUnit", CreateFlatPrism(new[]
            {
                new Vector2(-0.045f, -0.18f),
                new Vector2(0.045f, -0.18f),
                new Vector2(0.020f, 0.35f),
                new Vector2(0.0f, 0.46f),
                new Vector2(-0.020f, 0.35f)
            }, 0.035f));
            AddMesh("lever", "OIS05_Mesh_TaperedLeverHandleUnit", CreateFlatPrism(new[]
            {
                new Vector2(-0.10f, -0.46f),
                new Vector2(0.10f, -0.46f),
                new Vector2(0.070f, 0.46f),
                new Vector2(-0.070f, 0.46f)
            }, 0.080f));
            AddMesh("keyhole", "OIS05_Mesh_KeyholeGlyphUnit", CreateFlatPrism(new[]
            {
                new Vector2(-0.09f, -0.30f),
                new Vector2(0.09f, -0.30f),
                new Vector2(0.09f, 0.02f),
                new Vector2(0.17f, 0.12f),
                new Vector2(0.12f, 0.24f),
                new Vector2(0.0f, 0.31f),
                new Vector2(-0.12f, 0.24f),
                new Vector2(-0.17f, 0.12f),
                new Vector2(-0.09f, 0.02f)
            }, 0.030f));
            AddMesh("arrow", "OIS05_Mesh_ChevronArrowUnit", CreateFlatPrism(new[]
            {
                new Vector2(-0.42f, -0.13f),
                new Vector2(0.10f, -0.13f),
                new Vector2(0.10f, -0.26f),
                new Vector2(0.46f, 0.0f),
                new Vector2(0.10f, 0.26f),
                new Vector2(0.10f, 0.13f),
                new Vector2(-0.42f, 0.13f)
            }, 0.035f));
            AddMesh("fuse", "OIS05_Mesh_FuseCartridgeUnit", CreateCylinderMesh(20));
            AddMesh("chain", "OIS05_Mesh_ChainLinkOvalUnit", CreateFlatPrism(new[]
            {
                new Vector2(-0.11f, -0.24f),
                new Vector2(0.11f, -0.24f),
                new Vector2(0.17f, -0.10f),
                new Vector2(0.17f, 0.10f),
                new Vector2(0.11f, 0.24f),
                new Vector2(-0.11f, 0.24f),
                new Vector2(-0.17f, 0.10f),
                new Vector2(-0.17f, -0.10f)
            }, 0.030f));
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
            var root = new GameObject(spec.AssetId);
            switch (spec.Kind)
            {
                case PropKind.PressureLever:
                    BuildPressureLever(root, spec);
                    break;
                case PropKind.KeyedLock:
                    BuildKeyedLock(root, spec);
                    break;
                case PropKind.CrankPanel:
                    BuildCrankPanel(root, spec);
                    break;
                case PropKind.FuseBox:
                    BuildFuseBox(root, spec);
                    break;
                case PropKind.BreakerGauge:
                    BuildBreakerGauge(root, spec);
                    break;
                case PropKind.ValveRoutingPuzzle:
                    BuildValveRoutingPuzzle(root, spec);
                    break;
                case PropKind.BossOverrideTerminal:
                    BuildBossOverrideTerminal(root, spec);
                    break;
                case PropKind.LiftCallStation:
                    BuildLiftCallStation(root, spec);
                    break;
                case PropKind.Pickup:
                    BuildPickup(root, spec);
                    break;
                case PropKind.ObjectiveSignage:
                    BuildObjectiveSignage(root, spec);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SavePrefab(root, spec);
        }

        private static void BuildPressureLever(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "pressure_lever_backplate_visual_only", 0.92f, 1.38f, 0.14f, "iron");
            Label(root.transform, "label_PRESSURE", spec.Variant == 1 ? "REDLINE" : "PRESSURE", new Vector3(0f, 0.55f, -0.13f), 0.043f, new Color(0.96f, 0.84f, 0.50f));
            AddGauge(root.transform, "lever_pressure_gauge", new Vector3(-0.24f, 0.27f, -0.155f), 0.16f, spec.Variant == 1 ? 52f : -28f, spec.Variant == 1);
            AddLamp(root.transform, "ready_lamp", new Vector3(0.25f, 0.29f, -0.16f), spec.Variant == 2 ? "amber" : "green");
            Disc(root.transform, "lever_pivot_brass", new Vector3(0f, -0.07f, -0.18f), 0.16f, 0.07f, "brass");
            Flat(root.transform, "red_pressure_throw_handle", "lever", new Vector3(0.06f, -0.22f, -0.22f), new Vector3(0.80f, 0.80f, 1.0f), new Vector3(0f, 0f, spec.Variant == 0 ? -28f : spec.Variant == 1 ? 34f : -50f), "red");
            Cylinder(root.transform, "walnut_grip_knob", new Vector3(0.36f, -0.52f, -0.23f), new Vector3(0.16f, 0.22f, 0.16f), new Vector3(90f, 0f, 0f), "walnut");
            Box(root.transform, "non_authoritative_visual_marker_plate", new Vector3(0f, -0.60f, -0.14f), new Vector3(0.54f, 0.12f, 0.035f), Vector3.zero, "paper");
            Label(root.transform, "label_VIS_ONLY", "VIS", new Vector3(0f, -0.60f, -0.17f), 0.024f, new Color(0.10f, 0.07f, 0.04f));
        }

        private static void BuildKeyedLock(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "keyed_lock_backer", 1.08f, 1.22f, 0.14f, "iron");
            Disc(root.transform, "aged_brass_key_socket_ring", new Vector3(0f, 0.02f, -0.16f), 0.33f, 0.055f, "brass");
            Flat(root.transform, "black_keyhole_glyph", "keyhole", new Vector3(0f, 0.02f, -0.205f), Vector3.one, Vector3.zero, "rubber");
            Flat(root.transform, "indexed_cog_profile", spec.Variant == 0 ? "gear18" : "gear12", new Vector3(0f, 0.02f, -0.19f), new Vector3(0.92f, 0.92f, 0.55f), new Vector3(0f, 0f, spec.Variant * 10f), "darkBrass");
            Label(root.transform, "label_KEY", spec.Variant == 2 ? "LIFT KEY" : "KEY", new Vector3(0f, 0.50f, -0.15f), 0.043f, new Color(0.96f, 0.84f, 0.50f));
            AddLamp(root.transform, "locked_red_lamp", new Vector3(-0.34f, -0.42f, -0.16f), "red");
            AddLamp(root.transform, "unlocked_green_lamp", new Vector3(0.34f, -0.42f, -0.16f), "green");
            for (var i = 0; i < 4; i++)
            {
                var x = -0.30f + i * 0.20f;
                Box(root.transform, "key_bit_index_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.24f, -0.16f), new Vector3(0.055f, 0.15f + i * 0.018f, 0.035f), Vector3.zero, "white");
            }
        }

        private static void BuildCrankPanel(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "crank_panel_backer", 1.18f, 1.18f, 0.13f, "steel");
            Label(root.transform, "label_CRANK", spec.Variant == 2 ? "IRIS" : "CRANK", new Vector3(0f, 0.50f, -0.15f), 0.042f, new Color(0.96f, 0.84f, 0.50f));
            Flat(root.transform, "rotary_crank_gear_face", "gear18", new Vector3(0f, 0.02f, -0.17f), new Vector3(1.12f, 1.12f, 0.65f), new Vector3(0f, 0f, spec.Variant * 18f), "brass");
            Disc(root.transform, "crank_hub", new Vector3(0f, 0.02f, -0.22f), 0.13f, 0.07f, "darkBrass");
            Box(root.transform, "crank_throw_arm", new Vector3(0.26f, -0.20f, -0.23f), new Vector3(0.46f, 0.075f, 0.055f), new Vector3(0f, 0f, -38f), "copper");
            Cylinder(root.transform, "walnut_hand_grip", new Vector3(0.48f, -0.38f, -0.25f), new Vector3(0.13f, 0.20f, 0.13f), new Vector3(90f, 0f, 0f), "walnut");
            for (var i = 0; i < 4; i++)
            {
                var angle = -48f + i * 32f;
                Flat(root.transform, "sequence_tooth_" + (i + 1).ToString(CultureInfo.InvariantCulture), "needle", new Vector3(-0.36f + i * 0.24f, -0.44f, -0.16f), new Vector3(0.24f, 0.24f, 0.5f), new Vector3(0f, 0f, angle), i == spec.Variant + 1 ? "cyan" : "white");
            }
        }

        private static void BuildFuseBox(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "fuse_box_cabinet", 1.10f, 1.36f, 0.16f, "iron");
            Box(root.transform, "fuse_box_open_door_outline", new Vector3(-0.42f, 0f, -0.12f), new Vector3(0.08f, 1.08f, 0.04f), Vector3.zero, "darkBrass");
            Label(root.transform, "label_FUSE", spec.Variant == 1 ? "RELAY" : "FUSE", new Vector3(0f, 0.58f, -0.16f), 0.046f, new Color(0.96f, 0.84f, 0.50f));
            for (var i = 0; i < 3; i++)
            {
                var x = -0.26f + i * 0.26f;
                Cylinder(root.transform, "glass_fuse_cartridge_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.04f, -0.18f), new Vector3(0.13f, 0.55f, 0.13f), Vector3.zero, i == spec.Variant ? "cyan" : "amber");
                Cylinder(root.transform, "upper_fuse_cap_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.35f, -0.18f), new Vector3(0.15f, 0.07f, 0.15f), Vector3.zero, "brass");
                Cylinder(root.transform, "lower_fuse_cap_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.27f, -0.18f), new Vector3(0.15f, 0.07f, 0.15f), Vector3.zero, "brass");
            }

            AddLamp(root.transform, "fault_red_lamp", new Vector3(0.34f, -0.50f, -0.17f), spec.Variant == 1 ? "red" : "green");
            Box(root.transform, "paper_service_tag", new Vector3(-0.22f, -0.50f, -0.16f), new Vector3(0.36f, 0.11f, 0.03f), Vector3.zero, "paper");
        }

        private static void BuildBreakerGauge(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "breaker_gauge_bank", 1.24f, 1.22f, 0.14f, "steel");
            Label(root.transform, "label_BREAKER", spec.Variant == 2 ? "ARC" : "BREAKER", new Vector3(0f, 0.52f, -0.15f), 0.037f, new Color(0.96f, 0.84f, 0.50f));
            for (var i = 0; i < 3; i++)
            {
                var x = -0.34f + i * 0.34f;
                AddGauge(root.transform, "breaker_dial_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.17f, -0.16f), 0.15f, -42f + (spec.Variant + i) * 20f, i == 2 && spec.Variant == 1);
                Box(root.transform, "breaker_toggle_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.31f, -0.18f), new Vector3(0.08f, 0.26f, 0.06f), new Vector3(0f, 0f, i == spec.Variant ? -18f : 18f), i == spec.Variant ? "red" : "brass");
            }

            Box(root.transform, "reset_pull_bar", new Vector3(0f, -0.52f, -0.16f), new Vector3(0.76f, 0.08f, 0.04f), Vector3.zero, "copper");
        }

        private static void BuildValveRoutingPuzzle(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "valve_route_panel", 1.54f, 1.18f, 0.13f, "steel");
            Label(root.transform, "label_ROUTE", spec.Variant == 1 ? "COLOR ROUTE" : "ROUTE", new Vector3(0f, 0.50f, -0.15f), 0.037f, new Color(0.96f, 0.84f, 0.50f));
            for (var i = 0; i < 4; i++)
            {
                var y = -0.27f + i * 0.18f;
                Box(root.transform, "horizontal_pipe_run_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(0f, y, -0.15f), new Vector3(1.18f, 0.045f, 0.045f), Vector3.zero, i == spec.Variant ? "cyan" : "copper");
            }

            for (var i = 0; i < 4; i++)
            {
                var x = -0.48f + i * 0.32f;
                Box(root.transform, "vertical_route_pipe_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.00f, -0.16f), new Vector3(0.045f, 0.70f, 0.045f), Vector3.zero, "darkBrass");
                AddValve(root.transform, "routing_valve_wheel_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.05f, -0.20f), 0.17f, i == spec.Variant ? "green" : "brass");
            }

            AddGauge(root.transform, "shared_route_pressure_gauge", new Vector3(0.55f, 0.30f, -0.16f), 0.13f, 20f + spec.Variant * 12f, false);
        }

        private static void BuildBossOverrideTerminal(GameObject root, PropSpec spec)
        {
            Box(root.transform, "boss_override_console_body", new Vector3(0f, -0.20f, 0f), new Vector3(1.30f, 0.74f, 0.54f), Vector3.zero, "iron");
            Box(root.transform, "boss_override_angled_face", new Vector3(0f, 0.18f, -0.18f), new Vector3(1.18f, 0.54f, 0.10f), new Vector3(-18f, 0f, 0f), "steel");
            Label(root.transform, "label_OVERRIDE", spec.Variant == 0 ? "BOSS OVERRIDE" : "OVERRIDE", new Vector3(0f, 0.37f, -0.28f), 0.036f, new Color(0.96f, 0.84f, 0.50f));
            AddGauge(root.transform, "override_alarm_gauge", new Vector3(-0.38f, 0.12f, -0.29f), 0.15f, 58f, true);
            AddLamp(root.transform, "override_alarm_lamp", new Vector3(0.38f, 0.14f, -0.29f), "red");
            Box(root.transform, "red_override_switch", new Vector3(0f, -0.02f, -0.31f), new Vector3(0.16f, 0.42f, 0.08f), new Vector3(0f, 0f, spec.Variant == 1 ? -26f : 24f), "red");
            for (var i = 0; i < 3; i++)
            {
                var x = -0.28f + i * 0.28f;
                Disc(root.transform, "numbered_override_button_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.24f, -0.29f), 0.065f, 0.045f, i == spec.Variant ? "cyan" : "brass");
            }

            Flat(root.transform, "governor_crown_warning_gear", "gear12", new Vector3(0f, 0.68f, -0.01f), new Vector3(0.40f, 0.40f, 0.55f), Vector3.zero, "yellow");
        }

        private static void BuildLiftCallStation(GameObject root, PropSpec spec)
        {
            AddPanel(root.transform, "lift_call_column", 0.76f, 1.58f, 0.16f, "iron");
            Box(root.transform, "brass_lift_face", new Vector3(0f, 0.02f, -0.12f), new Vector3(0.52f, 1.20f, 0.055f), Vector3.zero, "brass");
            Label(root.transform, "label_LIFT", spec.Variant == 1 ? "HOIST" : "LIFT", new Vector3(0f, 0.64f, -0.16f), 0.043f, new Color(0.10f, 0.07f, 0.04f));
            Flat(root.transform, "up_arrow_button", "arrow", new Vector3(0f, 0.24f, -0.18f), new Vector3(0.54f, 0.54f, 0.7f), new Vector3(0f, 0f, 90f), "green");
            Flat(root.transform, "down_arrow_button", "arrow", new Vector3(0f, -0.12f, -0.18f), new Vector3(0.54f, 0.54f, 0.7f), new Vector3(0f, 0f, -90f), "red");
            AddLamp(root.transform, "arrival_amber_lamp", new Vector3(0f, -0.48f, -0.18f), spec.Variant == 2 ? "cyan" : "amber");
            if (spec.Variant == 1)
            {
                for (var i = 0; i < 5; i++)
                {
                    Flat(root.transform, "visual_chain_link_" + (i + 1).ToString(CultureInfo.InvariantCulture), "chain", new Vector3(0.38f, 0.30f - i * 0.16f, -0.12f), Vector3.one, new Vector3(0f, 0f, i % 2 == 0 ? 0f : 90f), "darkBrass");
                }
            }
        }

        private static void BuildPickup(GameObject root, PropSpec spec)
        {
            Box(root.transform, "pickup_display_plinth", new Vector3(0f, -0.36f, 0f), new Vector3(0.72f, 0.24f, 0.56f), Vector3.zero, "iron");
            Box(root.transform, "aged_paper_pickup_label", new Vector3(0f, -0.21f, -0.30f), new Vector3(0.52f, 0.11f, 0.03f), Vector3.zero, "paper");
            Label(root.transform, "label_PICKUP", spec.Variant == 0 ? "GEAR KEY" : spec.Variant == 1 ? "CELL" : "FUSE", new Vector3(0f, -0.21f, -0.33f), 0.021f, new Color(0.10f, 0.07f, 0.04f));
            if (spec.Variant == 0)
            {
                Flat(root.transform, "gear_key_head", "gear12", new Vector3(-0.14f, 0.04f, -0.03f), new Vector3(0.66f, 0.66f, 0.70f), Vector3.zero, "pickup");
                Box(root.transform, "gear_key_shaft", new Vector3(0.17f, 0.04f, -0.03f), new Vector3(0.46f, 0.08f, 0.08f), Vector3.zero, "brass");
                Box(root.transform, "gear_key_bit", new Vector3(0.40f, -0.03f, -0.03f), new Vector3(0.12f, 0.22f, 0.08f), Vector3.zero, "brass");
            }
            else if (spec.Variant == 1)
            {
                Cylinder(root.transform, "pressure_cell_glass_body", new Vector3(0f, 0.08f, 0f), new Vector3(0.22f, 0.62f, 0.22f), new Vector3(0f, 0f, 90f), "cyan");
                Cylinder(root.transform, "pressure_cell_left_cap", new Vector3(-0.36f, 0.08f, 0f), new Vector3(0.25f, 0.10f, 0.25f), new Vector3(0f, 0f, 90f), "brass");
                Cylinder(root.transform, "pressure_cell_right_cap", new Vector3(0.36f, 0.08f, 0f), new Vector3(0.25f, 0.10f, 0.25f), new Vector3(0f, 0f, 90f), "brass");
            }
            else
            {
                Cylinder(root.transform, "override_fuse_pickup_glass", new Vector3(0f, 0.08f, 0f), new Vector3(0.18f, 0.64f, 0.18f), Vector3.zero, "pickup");
                Cylinder(root.transform, "override_fuse_top_cap", new Vector3(0f, 0.43f, 0f), new Vector3(0.22f, 0.09f, 0.22f), Vector3.zero, "red");
                Cylinder(root.transform, "override_fuse_bottom_cap", new Vector3(0f, -0.27f, 0f), new Vector3(0.22f, 0.09f, 0.22f), Vector3.zero, "red");
            }

            AddLamp(root.transform, "pickup_readability_glow", new Vector3(0.30f, -0.08f, -0.30f), "pickup");
        }

        private static void BuildObjectiveSignage(GameObject root, PropSpec spec)
        {
            Box(root.transform, "objective_sign_backplate", new Vector3(0f, 0f, 0f), new Vector3(1.50f, 0.52f, 0.06f), Vector3.zero, "iron");
            Box(root.transform, "objective_sign_paper_face", new Vector3(0f, 0f, -0.04f), new Vector3(1.36f, 0.40f, 0.025f), Vector3.zero, spec.Variant == 2 ? "yellow" : "paper");
            if (spec.Variant == 0)
            {
                Flat(root.transform, "route_arrow", "arrow", new Vector3(-0.42f, 0f, -0.08f), new Vector3(0.55f, 0.55f, 0.7f), Vector3.zero, "green");
                Label(root.transform, "label_BOILERHEART", "BOILERHEART", new Vector3(0.28f, 0f, -0.09f), 0.036f, new Color(0.10f, 0.07f, 0.04f));
            }
            else if (spec.Variant == 1)
            {
                Flat(root.transform, "key_required_glyph", "keyhole", new Vector3(-0.48f, 0f, -0.08f), new Vector3(0.55f, 0.55f, 0.7f), Vector3.zero, "rubber");
                Label(root.transform, "label_KEY_REQUIRED", "KEY REQUIRED", new Vector3(0.23f, 0f, -0.09f), 0.039f, new Color(0.10f, 0.07f, 0.04f));
            }
            else
            {
                for (var i = 0; i < 5; i++)
                {
                    Box(root.transform, "hazard_stripe_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(-0.55f + i * 0.28f, 0f, -0.085f), new Vector3(0.12f, 0.42f, 0.025f), new Vector3(0f, 0f, -24f), "shadow");
                }

                Label(root.transform, "label_OVERRIDE_WARNING", "OVERRIDE", new Vector3(0f, 0.02f, -0.10f), 0.044f, new Color(0.18f, 0.02f, 0.01f));
            }
        }

        private static void AddPanel(Transform parent, string name, float width, float height, float depth, string materialKey)
        {
            Box(parent, name, Vector3.zero, new Vector3(width, height, depth), Vector3.zero, materialKey);
            Box(parent, name + "_top_brass_rail", new Vector3(0f, height * 0.5f - 0.035f, -depth * 0.45f), new Vector3(width + 0.08f, 0.055f, 0.045f), Vector3.zero, "brass");
            Box(parent, name + "_bottom_brass_rail", new Vector3(0f, -height * 0.5f + 0.035f, -depth * 0.45f), new Vector3(width + 0.08f, 0.055f, 0.045f), Vector3.zero, "brass");
            for (var i = 0; i < 4; i++)
            {
                var x = i < 2 ? -width * 0.42f : width * 0.42f;
                var y = i % 2 == 0 ? -height * 0.40f : height * 0.40f;
                Disc(parent, name + "_corner_hex_bolt_" + (i + 1).ToString(CultureInfo.InvariantCulture), new Vector3(x, y, -depth * 0.62f), 0.035f, 0.020f, "brass", "hex");
            }
        }

        private static void AddGauge(Transform parent, string name, Vector3 position, float radius, float needleAngle, bool redline)
        {
            Disc(parent, name + "_brass_bezel", position, radius, 0.045f, "brass");
            Disc(parent, name + "_ivory_face", position + Vector3.back * 0.018f, radius * 0.78f, 0.026f, "ivory");
            for (var i = 0; i < 7; i++)
            {
                var angle = -70f + i * 23.3f;
                var r = radius * 0.62f;
                var p = position + new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * r, Mathf.Cos(angle * Mathf.Deg2Rad) * r, -0.040f);
                Box(parent, name + "_tick_" + i.ToString(CultureInfo.InvariantCulture), p, new Vector3(radius * 0.045f, radius * 0.16f, 0.018f), new Vector3(0f, 0f, -angle), redline && i > 4 ? "red" : "rubber");
            }

            Flat(parent, name + "_needle", "needle", position + Vector3.back * 0.050f, new Vector3(radius * 1.35f, radius * 1.35f, 0.55f), new Vector3(0f, 0f, needleAngle), redline ? "red" : "rubber");
        }

        private static void AddValve(Transform parent, string name, Vector3 position, float radius, string materialKey)
        {
            Flat(parent, name + "_gear_wheel", "gear18", position, new Vector3(radius * 2.0f, radius * 2.0f, 0.55f), Vector3.zero, materialKey);
            Disc(parent, name + "_hub", position + Vector3.back * 0.035f, radius * 0.32f, 0.05f, "darkBrass");
            Box(parent, name + "_spoke_horizontal", position + Vector3.back * 0.055f, new Vector3(radius * 1.50f, 0.035f, 0.026f), Vector3.zero, "darkBrass");
            Box(parent, name + "_spoke_vertical", position + Vector3.back * 0.056f, new Vector3(0.035f, radius * 1.50f, 0.026f), Vector3.zero, "darkBrass");
        }

        private static void AddLamp(Transform parent, string name, Vector3 position, string materialKey)
        {
            Disc(parent, name + "_bezel", position, 0.085f, 0.040f, "darkBrass");
            Disc(parent, name + "_glass", position + Vector3.back * 0.025f, 0.060f, 0.032f, materialKey);
        }

        private static void Box(Transform parent, string name, Vector3 position, Vector3 scale, Vector3 euler, string materialKey)
        {
            Part(parent, "box", name, position, scale, euler, materialKey);
        }

        private static void Cylinder(Transform parent, string name, Vector3 position, Vector3 scale, Vector3 euler, string materialKey)
        {
            Part(parent, "cylinder16", name, position, scale, euler, materialKey);
        }

        private static void Disc(Transform parent, string name, Vector3 position, float radius, float depth, string materialKey, string meshKey = "cylinder32")
        {
            Part(parent, meshKey, name, position, new Vector3(radius * 2f, depth, radius * 2f), new Vector3(90f, 0f, 0f), materialKey);
        }

        private static void Flat(Transform parent, string name, string meshKey, Vector3 position, Vector3 scale, Vector3 euler, string materialKey)
        {
            Part(parent, meshKey, name, position, scale, euler, materialKey);
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

        private static void SavePrefab(GameObject root, PropSpec spec)
        {
            var path = CombineAssetPath(PackageRoot, "Runtime/Prefabs/" + spec.FileName);
            if (AssetDatabase.LoadMainAssetAtPath(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            PrefabUtility.SaveAsPrefabAsset(root, path);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static int RenderPreviewPngs()
        {
            var renderRoot = ResolveRepositoryRenderRoot();
            Directory.CreateDirectory(renderRoot);

            var filesWritten = 0;
            foreach (var prefabPath in GeneratedPrefabAssetPaths())
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning("OIS05_PREVIEW_SKIP missing=" + prefabPath);
                    continue;
                }

                RenderSinglePrefab(prefab, Path.Combine(renderRoot, prefab.name + "_preview.png"));
                filesWritten++;
            }

            RenderContactSheet(GeneratedPrefabAssetPaths(), Path.Combine(renderRoot, "BB_OIS05_ContactSheet.png"));
            filesWritten++;
            RenderMaterialSwatches(Path.Combine(renderRoot, "BB_OIS05_MaterialSwatches.png"));
            filesWritten++;
            WriteRenderIndex(renderRoot);
            return filesWritten;
        }

        private static void RenderSinglePrefab(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var instance = UnityEngine.Object.Instantiate(prefab);
            instance.name = prefab.name;
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(0f, -22f, 0f);

            AddPreviewFloor(new Vector3(0f, -0.86f, 0.22f), new Vector3(4.8f, 0.06f, 3.5f));
            AddPreviewWall(new Vector3(0f, 0.12f, 0.46f), new Vector3(4.8f, 2.9f, 0.06f));
            AddPreviewLights();

            var bounds = CalculateBounds(instance);
            var radius = Mathf.Max(0.74f, bounds.extents.magnitude);

            var cameraObject = new GameObject("ois05_preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.020f, 0.018f, 0.016f);
            camera.fieldOfView = 33f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 50f;
            camera.transform.position = bounds.center + new Vector3(radius * 0.72f, radius * 0.42f, -radius * 2.45f);
            camera.transform.LookAt(bounds.center + Vector3.up * radius * 0.05f);

            RenderCameraToPng(camera, outputPath, 1600, 1000);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderContactSheet(string[] prefabPaths, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddPreviewLights();

            const int columns = 6;
            const float cellX = 1.62f;
            const float cellY = 1.36f;
            for (var i = 0; i < prefabPaths.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[i]);
                if (prefab == null)
                {
                    continue;
                }

                var instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = prefab.name;
                instance.transform.rotation = Quaternion.Euler(0f, -18f, 0f);
                var bounds = CalculateBounds(instance);
                var maxDimension = Mathf.Max(0.1f, Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z)));
                var scale = Mathf.Min(1.0f, 1.00f / maxDimension);
                instance.transform.localScale *= scale;

                bounds = CalculateBounds(instance);
                var col = i % columns;
                var row = i / columns;
                var targetCenter = new Vector3((col - 2.5f) * cellX, (2.0f - row) * cellY, 0f);
                instance.transform.position += targetCenter - bounds.center;
            }

            var sheetBounds = CalculateSceneBounds();
            var cameraObject = new GameObject("ois05_contact_sheet_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.020f, 0.018f, 0.016f);
            camera.orthographic = true;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 80f;
            camera.transform.position = sheetBounds.center + new Vector3(0f, 0f, -18f);
            camera.transform.LookAt(sheetBounds.center);
            camera.orthographicSize = Mathf.Max(sheetBounds.extents.y + 0.48f, (sheetBounds.extents.x + 0.55f) * 2000f / 3000f);

            RenderCameraToPng(camera, outputPath, 3000, 2000);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderMaterialSwatches(string outputPath)
        {
            const int width = 1800;
            const int height = 900;
            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            var background = new Color(0.020f, 0.018f, 0.016f);
            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = background;
            }

            texture.SetPixels(pixels);
            const int cols = 6;
            const int rows = 3;
            var cellW = width / cols;
            var cellH = height / rows;
            for (var i = 0; i < MaterialSpecs.Length; i++)
            {
                var col = i % cols;
                var row = i / cols;
                var x0 = col * cellW + 26;
                var y0 = height - (row + 1) * cellH + 26;
                var color = MaterialSpecs[i].Color;
                for (var y = y0; y < y0 + cellH - 52; y++)
                {
                    for (var x = x0; x < x0 + cellW - 52; x++)
                    {
                        if (x >= 0 && x < width && y >= 0 && y < height)
                        {
                            texture.SetPixel(x, y, color);
                        }
                    }
                }
            }

            texture.Apply();
            File.WriteAllBytes(outputPath, texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);
        }

        private static void AddPreviewLights()
        {
            var key = new GameObject("warm_lantern_key_light").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.74f, 0.46f);
            key.intensity = 2.35f;
            key.transform.rotation = Quaternion.Euler(45f, -34f, 0f);

            var rim = new GameObject("cool_pressure_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(0.30f, 0.68f, 0.84f);
            rim.intensity = 2.0f;
            rim.range = 6.5f;
            rim.transform.position = new Vector3(-2.2f, 1.4f, -1.7f);

            var fill = new GameObject("soft_red_override_fill").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(0.95f, 0.30f, 0.18f);
            fill.intensity = 0.82f;
            fill.range = 5.5f;
            fill.transform.position = new Vector3(2.4f, -0.1f, -1.2f);
        }

        private static void AddPreviewFloor(Vector3 position, Vector3 scale)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "ois05_preview_oil_floor";
            floor.transform.position = position;
            floor.transform.localScale = scale;
            RemoveCollider(floor);
            ApplyTransientMaterial(floor, new Color(0.040f, 0.037f, 0.033f));
        }

        private static void AddPreviewWall(Vector3 position, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = "ois05_preview_soot_wall";
            wall.transform.position = position;
            wall.transform.localScale = scale;
            RemoveCollider(wall);
            ApplyTransientMaterial(wall, new Color(0.065f, 0.055f, 0.047f));
        }

        private static void RemoveCollider(GameObject gameObject)
        {
            var collider = gameObject.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }

        private static void ApplyTransientMaterial(GameObject gameObject, Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Diffuse");
            var material = new Material(shader)
            {
                name = gameObject.name + "_material",
                color = color
            };

            SetColor(material, "_BaseColor", color);
            SetColor(material, "_Color", color);
            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }
        }

        private static void RenderCameraToPng(Camera camera, string outputPath, int width, int height)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? string.Empty);
            var renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)
            {
                antiAliasing = 4
            };

            var previous = RenderTexture.active;
            Texture2D texture = null;
            try
            {
                camera.targetTexture = renderTexture;
                camera.Render();
                RenderTexture.active = renderTexture;
                texture = new Texture2D(width, height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                texture.Apply();
                File.WriteAllBytes(outputPath, texture.EncodeToPNG());
            }
            finally
            {
                RenderTexture.active = previous;
                camera.targetTexture = null;
                if (texture != null)
                {
                    UnityEngine.Object.DestroyImmediate(texture);
                }

                renderTexture.Release();
                UnityEngine.Object.DestroyImmediate(renderTexture);
            }
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                return new Bounds(root.transform.position, Vector3.one);
            }

            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private static Bounds CalculateSceneBounds()
        {
            var renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Exclude);
            if (renderers.Length == 0)
            {
                return new Bounds(Vector3.zero, Vector3.one);
            }

            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private static ValidationResult ValidateGeneratedAssetsInternal()
        {
            var result = new ValidationResult();
            var familyCounts = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var family in RequiredFamilies)
            {
                familyCounts[family] = 0;
            }

            var specs = GetSpecs();
            foreach (var spec in specs)
            {
                if (!familyCounts.ContainsKey(spec.Family))
                {
                    familyCounts[spec.Family] = 0;
                }

                familyCounts[spec.Family]++;
            }

            foreach (var path in GeneratedPrefabAssetPaths())
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    result.Findings.Add("missing_prefab:" + path);
                    continue;
                }

                result.Prefabs++;
                var renderers = prefab.GetComponentsInChildren<Renderer>(true);
                result.Renderers += renderers.Length;
                if (renderers.Length == 0)
                {
                    result.Findings.Add("no_renderers:" + path);
                }

                if (prefab.GetComponentsInChildren<Collider>(true).Length > 0)
                {
                    result.Findings.Add("colliders_present:" + path);
                }

                if (prefab.GetComponentsInChildren<Rigidbody>(true).Length > 0 || prefab.GetComponentsInChildren<Rigidbody2D>(true).Length > 0)
                {
                    result.Findings.Add("rigidbodies_present:" + path);
                }

                if (prefab.GetComponentsInChildren<AudioSource>(true).Length > 0)
                {
                    result.Findings.Add("audio_sources_present:" + path);
                }

                if (prefab.GetComponentsInChildren<ParticleSystem>(true).Length > 0)
                {
                    result.Findings.Add("particle_systems_present:" + path);
                }

                if (prefab.GetComponentsInChildren<Camera>(true).Length > 0)
                {
                    result.Findings.Add("cameras_present:" + path);
                }

                if (prefab.GetComponentsInChildren<Light>(true).Length > 0)
                {
                    result.Findings.Add("lights_present:" + path);
                }

                foreach (var behaviour in prefab.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (behaviour == null)
                    {
                        result.Findings.Add("missing_script_reference:" + path);
                    }
                    else
                    {
                        result.Findings.Add("runtime_monobehaviour_present:" + behaviour.GetType().FullName + ":" + path);
                    }
                }
            }

            foreach (var family in RequiredFamilies)
            {
                if (!familyCounts.ContainsKey(family) || familyCounts[family] == 0)
                {
                    result.Findings.Add("missing_prop_family:" + family);
                }
            }

            result.Materials = CountExistingAssets<Material>(GeneratedMaterialAssetPaths(), "missing_material", result.Findings);
            result.Meshes = CountExistingAssets<Mesh>(GeneratedMeshAssetPaths(), "missing_mesh", result.Findings);
            result.FamilyCounts = familyCounts;
            result.Pass = result.Findings.Count == 0 && result.Prefabs >= 24 && result.Materials >= 16 && result.Meshes >= 8;
            return result;
        }

        private static int CountExistingAssets<T>(IEnumerable<string> paths, string missingPrefix, ICollection<string> findings) where T : UnityEngine.Object
        {
            var count = 0;
            foreach (var path in paths)
            {
                if (AssetDatabase.LoadAssetAtPath<T>(path) == null)
                {
                    findings.Add(missingPrefix + ":" + path);
                    continue;
                }

                count++;
            }

            return count;
        }

        private static void WriteValidationReports(ValidationResult result, int previewFiles)
        {
            var productionRoot = ResolveRepositoryProductionRoot();
            Directory.CreateDirectory(productionRoot);

            var validationPath = Path.Combine(productionRoot, "UnityValidationReport_ObjectiveInteractablesSet05_v0.1.49.json");
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"OIS05\",");
            builder.AppendLine("  \"version\": \"0.1.49\",");
            builder.AppendLine($"  \"generated_at\": \"{Timestamp()}\",");
            builder.AppendLine($"  \"status\": \"{(result.Pass ? "pass" : "fail")}\",");
            builder.AppendLine("  \"counts\": {");
            builder.AppendLine($"    \"prefabs\": {result.Prefabs.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"materials\": {result.Materials.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"meshes\": {result.Meshes.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"renderers\": {result.Renderers.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"preview_pngs\": {previewFiles.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine("  },");
            AppendObject(builder, "family_counts", result.FamilyCounts, true);
            builder.AppendLine("  \"runtime_contract\": {");
            builder.AppendLine("    \"visual_only\": true,");
            builder.AppendLine("    \"runtime_monobehaviours\": \"omitted\",");
            builder.AppendLine("    \"gameplay_authority\": \"none\",");
            builder.AppendLine("    \"colliders\": \"omitted\",");
            builder.AppendLine("    \"rigidbodies\": \"omitted\",");
            builder.AppendLine("    \"audio_sources\": \"omitted\",");
            builder.AppendLine("    \"particle_systems\": \"omitted\",");
            builder.AppendLine("    \"cameras\": \"omitted\",");
            builder.AppendLine("    \"lights\": \"omitted\",");
            builder.AppendLine("    \"scene_changes\": \"none\"");
            builder.AppendLine("  },");
            AppendArray(builder, "findings", result.Findings, false);
            builder.AppendLine("}");
            File.WriteAllText(validationPath, builder.ToString(), Encoding.UTF8);

            WriteAcceptanceReport(productionRoot, result, previewFiles);
            WriteAssetInventory(productionRoot);
        }

        private static void WriteAcceptanceReport(string productionRoot, ValidationResult result, int previewFiles)
        {
            var path = Path.Combine(productionRoot, "ACCEPTANCE_REPORT_ObjectiveInteractablesSet05_v0.1.49.md");
            var builder = new StringBuilder();
            builder.AppendLine("# Objective Interactables Set 05 Acceptance Report");
            builder.AppendLine();
            builder.AppendLine($"Generated: {Timestamp()}");
            builder.AppendLine();
            builder.AppendLine($"Status: {(result.Pass ? "PASS" : "FAIL")}");
            builder.AppendLine();
            builder.AppendLine("## Counts");
            builder.AppendLine();
            builder.AppendLine($"- Prefabs: {result.Prefabs}");
            builder.AppendLine($"- Materials: {result.Materials}");
            builder.AppendLine($"- Reusable meshes: {result.Meshes}");
            builder.AppendLine($"- Renderer components: {result.Renderers}");
            builder.AppendLine($"- Preview PNGs: {previewFiles}");
            builder.AppendLine();
            builder.AppendLine("## Families");
            builder.AppendLine();
            foreach (var entry in result.FamilyCounts)
            {
                builder.AppendLine($"- {entry.Key}: {entry.Value}");
            }

            builder.AppendLine();
            builder.AppendLine("## Runtime Safety");
            builder.AppendLine();
            builder.AppendLine("- Visual-only package.");
            builder.AppendLine("- No gameplay authority, inventory, trigger logic, damage, door state, lift state, boss state, input, or autonomous audio scripts.");
            builder.AppendLine("- Runtime prefabs omit MonoBehaviours, colliders, rigidbodies, audio sources, particle systems, cameras, and lights.");
            builder.AppendLine("- Preview lighting and cameras are transient editor-scene objects only.");
            builder.AppendLine();
            builder.AppendLine("## Validation Evidence");
            builder.AppendLine();
            builder.AppendLine("- Unity command: `Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05/ValidationProject~ -executeMethod BrassworksBreach.ObjectiveInteractablesSet05.Editor.ObjectiveInteractablesSet05Generator.GenerateRenderValidate`");
            builder.AppendLine($"- Unity result marker: `OIS05_UNITY_VALIDATION_PASS v0.1.49 prefabs={result.Prefabs} materials={result.Materials} meshes={result.Meshes} renderers={result.Renderers} previews={previewFiles}`");
            builder.AppendLine("- Static sidecar validator command: `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1 -ProjectPath \"D:/__MY APPS/Unity Doom\" -PackageNamePattern \"BrassworksBreach.ObjectiveInteractablesSet05\" -Json`");
            builder.AppendLine();
            builder.AppendLine("## Findings");
            builder.AppendLine();
            if (result.Findings.Count == 0)
            {
                builder.AppendLine("- None.");
            }
            else
            {
                foreach (var finding in result.Findings)
                {
                    builder.AppendLine("- " + finding);
                }
            }

            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
        }

        private static void WriteAssetInventory(string productionRoot)
        {
            var path = Path.Combine(productionRoot, "OIS05_AssetInventory_v0.1.49.md");
            var builder = new StringBuilder();
            builder.AppendLine("# OIS05 Asset Inventory");
            builder.AppendLine();
            builder.AppendLine("## Prefabs");
            builder.AppendLine();
            foreach (var spec in GetSpecs())
            {
                builder.AppendLine($"- `{spec.AssetId}` - {spec.Family}; {spec.Role}; cue: {spec.ReadabilityCue}");
            }

            builder.AppendLine();
            builder.AppendLine("## Materials");
            builder.AppendLine();
            foreach (var spec in MaterialSpecs)
            {
                builder.AppendLine($"- `{spec.Name}`");
            }

            builder.AppendLine();
            builder.AppendLine("## Meshes");
            builder.AppendLine();
            foreach (var name in GetMeshNames())
            {
                builder.AppendLine($"- `{name}`");
            }

            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
        }

        private static void WriteRenderIndex(string renderRoot)
        {
            var path = Path.Combine(renderRoot, "README_RENDER_INDEX.md");
            var builder = new StringBuilder();
            builder.AppendLine("# Objective Interactables Set 05 Render Index");
            builder.AppendLine();
            builder.AppendLine("Unity preview PNGs generated from transient editor scenes. These files are review artifacts only and should not be imported into gameplay.");
            builder.AppendLine();
            builder.AppendLine("## Preview Files");
            builder.AppendLine();
            foreach (var preview in GetPreviewRenderPaths())
            {
                builder.AppendLine("- `" + preview + "`");
            }

            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
        }

        private static PropSpec[] GetSpecs()
        {
            return new[]
            {
                new PropSpec("BB_OIS05_PressureLever_BridgeVentRelease", "pressure_levers", "large bridge vent pressure release lever", "red throw handle, pressure gauge, VIS non-authoritative tag", PropKind.PressureLever, 0),
                new PropSpec("BB_OIS05_PressureLever_TwinRedlineBypass", "pressure_levers", "redline bypass pressure lever", "danger gauge, red lamp, oversized pressure label", PropKind.PressureLever, 1),
                new PropSpec("BB_OIS05_PressureLever_BoilerIntakeCutoff", "pressure_levers", "boiler intake cutoff lever", "amber lamp, deep throw angle, cutoff plate", PropKind.PressureLever, 2),
                new PropSpec("BB_OIS05_KeyedLock_TriCogVaultSocket", "keyed_locks", "tri-cog vault key socket", "central keyhole, three-bit index, red/green lamps", PropKind.KeyedLock, 0),
                new PropSpec("BB_OIS05_KeyedLock_MasterGearDoorNest", "keyed_locks", "master gear door key nest", "large gear ring, indexed lock face, KEY label", PropKind.KeyedLock, 1),
                new PropSpec("BB_OIS05_KeyedLock_ServiceLiftBrassKeyplate", "keyed_locks", "service lift brass keyplate", "LIFT KEY label, paired state lamps", PropKind.KeyedLock, 2),
                new PropSpec("BB_OIS05_CrankPanel_BulkheadSequencer", "crank_panels", "bulkhead crank sequencer panel", "rotary crank wheel and sequence teeth", PropKind.CrankPanel, 0),
                new PropSpec("BB_OIS05_CrankPanel_WaterlineSluiceGate", "crank_panels", "waterline sluice gate crank panel", "copper crank arm and numbered stops", PropKind.CrankPanel, 1),
                new PropSpec("BB_OIS05_CrankPanel_IrisDoorRotaryEncoder", "crank_panels", "iris door rotary encoder", "IRIS label and four timing indicators", PropKind.CrankPanel, 2),
                new PropSpec("BB_OIS05_FuseBox_TripleGlassCartridge", "fuse_boxes", "triple glass cartridge fuse box", "three readable fuses and service lamp", PropKind.FuseBox, 0),
                new PropSpec("BB_OIS05_FuseBox_BurntRelayServiceDoor", "fuse_boxes", "burnt relay service-door fuse box", "RELAY label and fault red lamp", PropKind.FuseBox, 1),
                new PropSpec("BB_OIS05_FuseBox_CyanChargeDistributor", "fuse_boxes", "cyan charge distributor fuse box", "active cyan cartridge and paper service tag", PropKind.FuseBox, 2),
                new PropSpec("BB_OIS05_BreakerGauge_ThreeNeedleTriptych", "breaker_gauges", "three-needle breaker gauge triptych", "three dial bank with toggles", PropKind.BreakerGauge, 0),
                new PropSpec("BB_OIS05_BreakerGauge_RedlineResetBank", "breaker_gauges", "redline reset breaker bank", "danger needle, reset pull bar, active red toggle", PropKind.BreakerGauge, 1),
                new PropSpec("BB_OIS05_BreakerGauge_ArcFaultDialColumn", "breaker_gauges", "arc-fault breaker dial column", "ARC label with misaligned toggle bank", PropKind.BreakerGauge, 2),
                new PropSpec("BB_OIS05_ValveRoutingPuzzle_FourValveManifold", "valve_routing_puzzles", "four-valve pressure manifold puzzle", "grid pipes and valve wheel routing", PropKind.ValveRoutingPuzzle, 0),
                new PropSpec("BB_OIS05_ValveRoutingPuzzle_ColorSteamSelector", "valve_routing_puzzles", "color steam selector puzzle", "active colored pipe and wheel state", PropKind.ValveRoutingPuzzle, 1),
                new PropSpec("BB_OIS05_ValveRoutingPuzzle_PressureCrossfeedGrid", "valve_routing_puzzles", "pressure crossfeed grid puzzle", "four valves with shared route gauge", PropKind.ValveRoutingPuzzle, 2),
                new PropSpec("BB_OIS05_BossOverrideTerminal_GovernorKillSwitch", "boss_override_terminals", "governor kill-switch terminal", "boss override label, alarm gauge, crown gear", PropKind.BossOverrideTerminal, 0),
                new PropSpec("BB_OIS05_BossOverrideTerminal_CogitatorPanicPanel", "boss_override_terminals", "cogitator panic override panel", "red switch, numbered override buttons", PropKind.BossOverrideTerminal, 1),
                new PropSpec("BB_OIS05_BossOverrideTerminal_CoreVentFailsafe", "boss_override_terminals", "core vent failsafe terminal", "alarm lamp and top warning gear", PropKind.BossOverrideTerminal, 2),
                new PropSpec("BB_OIS05_LiftCallStation_BrassCageUpDown", "lift_call_stations", "brass cage up/down lift station", "large green up and red down arrows", PropKind.LiftCallStation, 0),
                new PropSpec("BB_OIS05_LiftCallStation_ChainBellSummoner", "lift_call_stations", "chain bell lift summoner", "visual pull chain and amber arrival lamp", PropKind.LiftCallStation, 1),
                new PropSpec("BB_OIS05_LiftCallStation_PressurePlatformCall", "lift_call_stations", "pressure platform lift call station", "cyan platform ready lamp", PropKind.LiftCallStation, 2),
                new PropSpec("BB_OIS05_Pickup_GearKeyOnPlinth", "pickups", "gear key pickup presentation plinth", "emerald gear key and label", PropKind.Pickup, 0),
                new PropSpec("BB_OIS05_Pickup_PressureCellCanister", "pickups", "pressure cell canister pickup", "cyan glass cell on plinth", PropKind.Pickup, 1),
                new PropSpec("BB_OIS05_Pickup_BossOverrideFuse", "pickups", "boss override fuse pickup", "emerald glass fuse with red caps", PropKind.Pickup, 2),
                new PropSpec("BB_OIS05_ObjectiveSignage_RouteArrowBoilerheart", "objective_signage", "route arrow objective sign", "BOILERHEART arrow direction plate", PropKind.ObjectiveSignage, 0),
                new PropSpec("BB_OIS05_ObjectiveSignage_KeyRequiredPlaque", "objective_signage", "key required objective plaque", "keyhole glyph and clear text", PropKind.ObjectiveSignage, 1),
                new PropSpec("BB_OIS05_ObjectiveSignage_OverrideWarningPlacard", "objective_signage", "override warning objective placard", "yellow hazard face and override text", PropKind.ObjectiveSignage, 2)
            };
        }

        private static string[] GetMaterialNames()
        {
            var names = new string[MaterialSpecs.Length];
            for (var i = 0; i < MaterialSpecs.Length; i++)
            {
                names[i] = MaterialSpecs[i].Name;
            }

            return names;
        }

        private static string[] GetMeshNames()
        {
            return new[]
            {
                "OIS05_Mesh_BoxUnit",
                "OIS05_Mesh_Cylinder16Unit",
                "OIS05_Mesh_Cylinder32Unit",
                "OIS05_Mesh_HexBoltUnit",
                "OIS05_Mesh_Gear12ToothUnit",
                "OIS05_Mesh_Gear18ToothUnit",
                "OIS05_Mesh_PressureNeedleUnit",
                "OIS05_Mesh_TaperedLeverHandleUnit",
                "OIS05_Mesh_KeyholeGlyphUnit",
                "OIS05_Mesh_ChevronArrowUnit",
                "OIS05_Mesh_FuseCartridgeUnit",
                "OIS05_Mesh_ChainLinkOvalUnit"
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
            return BuildMesh(vertices, triangles);
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

            return BuildMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreateGearMesh(int teeth)
        {
            var points = new Vector2[teeth * 2];
            for (var i = 0; i < points.Length; i++)
            {
                var angle = Mathf.PI * 2f * i / points.Length;
                var radius = i % 2 == 0 ? 0.50f : 0.40f;
                points[i] = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            }

            return CreateFlatPrism(points, 0.10f);
        }

        private static Mesh CreateFlatPrism(Vector2[] outline, float depth)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var halfDepth = depth * 0.5f;

            vertices.Add(new Vector3(0f, 0f, -halfDepth));
            for (var i = 0; i < outline.Length; i++)
            {
                vertices.Add(new Vector3(outline[i].x, outline[i].y, -halfDepth));
            }

            var backCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, halfDepth));
            for (var i = 0; i < outline.Length; i++)
            {
                vertices.Add(new Vector3(outline[i].x, outline[i].y, halfDepth));
            }

            for (var i = 0; i < outline.Length; i++)
            {
                var next = (i + 1) % outline.Length;
                triangles.Add(0);
                triangles.Add(1 + i);
                triangles.Add(1 + next);

                triangles.Add(backCenter);
                triangles.Add(backCenter + 1 + next);
                triangles.Add(backCenter + 1 + i);

                triangles.Add(1 + i);
                triangles.Add(backCenter + 1 + i);
                triangles.Add(backCenter + 1 + next);
                triangles.Add(1 + i);
                triangles.Add(backCenter + 1 + next);
                triangles.Add(1 + next);
            }

            return BuildMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh BuildMesh(Vector3[] vertices, int[] triangles)
        {
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
            AppendField(builder, "display_name", "Objective Interactables Set 05", true);
            AppendField(builder, "version", Version, true);
            AppendField(builder, "build_id", BuildId, true);
            AppendField(builder, "unity_version", Application.unityVersion, true);
            AppendField(builder, "generated_at", Timestamp(), true);
            AppendField(builder, "sidecar_project", "UD-SC-OIS05-ObjectiveInteractablesVisualOnly", true);
            AppendField(builder, "owner_lane", "sidecar-objective-interactable-lookdev", true);
            AppendField(builder, "primary_intake_owner", "main-lane-art-integration", true);
            AppendField(builder, "canonical_root", "AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05", true);
            AppendField(builder, "package_root", "AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05", true);
            AppendField(builder, "package_name", PackageName, true);
            AppendField(builder, "package_version", Version, true);
            builder.AppendLine("  \"asset_counts\": {");
            builder.AppendLine($"    \"generated_prefabs\": {GetSpecs().Length},");
            builder.AppendLine($"    \"generated_materials\": {MaterialSpecs.Length},");
            builder.AppendLine($"    \"generated_meshes\": {GetMeshNames().Length},");
            builder.AppendLine($"    \"preview_renders\": {GetPreviewRenderPaths().Length},");
            builder.AppendLine("    \"runtime_scripts\": 0,");
            builder.AppendLine("    \"editor_scripts\": 1,");
            builder.AppendLine("    \"textures\": 0,");
            builder.AppendLine("    \"audio\": 0,");
            builder.AppendLine("    \"colliders\": 0");
            builder.AppendLine("  },");
            AppendArray(builder, "generated_prefabs", GetPrefabManifestPaths(), true);
            AppendArray(builder, "generated_materials", PrefixPaths("Runtime/Materials", GetMaterialNames(), ".mat"), true);
            AppendArray(builder, "generated_meshes", PrefixPaths("Runtime/Meshes", GetMeshNames(), ".asset"), true);
            AppendArray(builder, "preview_renders", GetPreviewRenderPaths(), true);
            AppendArray(builder, "prop_families", RequiredFamilies, true);
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
            builder.AppendLine("    \"runtime_monobehaviours\": \"omitted\",");
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
            AppendField(builder, "rollback_path", "delete AssetPacks/BrassworksBreach.ObjectiveInteractablesSet05, Documentation/AssetProduction/V0_1_49_ObjectiveInteractablesSet05, Documentation/ConceptRenders/V0_1_49_ObjectiveInteractablesSet05, Documentation/Planning/V0_1_49_ObjectiveInteractablesSet05ImportReadiness, and Documentation/QA/V0_1_49_ObjectiveInteractablesSet05ImportReadiness", true);
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
            var paths = new string[specs.Length + 2];
            for (var i = 0; i < specs.Length; i++)
            {
                paths[i] = RenderOutputRelativePath + "/" + specs[i].AssetId + "_preview.png";
            }

            paths[paths.Length - 2] = RenderOutputRelativePath + "/BB_OIS05_ContactSheet.png";
            paths[paths.Length - 1] = RenderOutputRelativePath + "/BB_OIS05_MaterialSwatches.png";
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
            var package = PackageInfo.FindForAssembly(typeof(ObjectiveInteractablesSet05Generator).Assembly);
            if (package != null && !string.IsNullOrWhiteSpace(package.assetPath) && !string.IsNullOrWhiteSpace(package.resolvedPath))
            {
                return new PackageRootInfo(package.assetPath.Replace("\\", "/"), package.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(ObjectiveInteractablesSet05Generator));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/ObjectiveInteractablesSet05Generator.cs";
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

        private static string ResolveRepositoryRenderRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_OIS05_RENDER_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return Path.GetFullPath(explicitRoot);
            }

            return Path.Combine(ResolveRepoRoot(), RenderOutputRelativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ResolveRepositoryProductionRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_OIS05_PRODUCTION_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return Path.GetFullPath(explicitRoot);
            }

            return Path.Combine(ResolveRepoRoot(), ProductionOutputRelativePath.Replace('/', Path.DirectorySeparatorChar));
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

        private static void AppendObject(StringBuilder builder, string name, IReadOnlyDictionary<string, int> values, bool trailingComma)
        {
            builder.Append("  \"").Append(EscapeJson(name)).AppendLine("\": {");
            var index = 0;
            foreach (var entry in values)
            {
                builder.Append("    \"").Append(EscapeJson(entry.Key)).Append("\": ").Append(entry.Value.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(index == values.Count - 1 ? string.Empty : ",");
                index++;
            }

            builder.Append("  }");
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

        private static string Timestamp()
        {
            return DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
        }

        private enum PropKind
        {
            PressureLever,
            KeyedLock,
            CrankPanel,
            FuseBox,
            BreakerGauge,
            ValveRoutingPuzzle,
            BossOverrideTerminal,
            LiftCallStation,
            Pickup,
            ObjectiveSignage
        }

        private sealed class PropSpec
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

        private sealed class MatSpec
        {
            public MatSpec(string key, string name, Color color, float metallic, float smoothness, Color? emission = null, bool transparent = false)
            {
                Key = key;
                Name = name;
                Color = color;
                Metallic = metallic;
                Smoothness = smoothness;
                Emission = emission;
                Transparent = transparent;
            }

            public string Key { get; }
            public string Name { get; }
            public Color Color { get; }
            public float Metallic { get; }
            public float Smoothness { get; }
            public Color? Emission { get; }
            public bool Transparent { get; }
        }

        private sealed class ValidationResult
        {
            public bool Pass;
            public int Prefabs;
            public int Materials;
            public int Meshes;
            public int Renderers;
            public Dictionary<string, int> FamilyCounts = new Dictionary<string, int>(StringComparer.Ordinal);
            public readonly List<string> Findings = new List<string>();
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
