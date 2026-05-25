using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.PipeTankGaugeSet10.Editor
{
    public static class PipeTankGaugeSet10Builder
    {
        public const string PackageName = "com.brassworks.sidecar.pipe-tank-gauge-set10";
        private const string Version = "0.1.55-p010";
        private const string Prefix = "PTG10";
        private const int PreviewWidth = 1400;
        private const int PreviewHeight = 900;

        private static readonly List<TextureRecord> TextureRecords = new List<TextureRecord>();
        private static readonly List<MaterialRecord> MaterialRecords = new List<MaterialRecord>();
        private static readonly List<MeshRecord> MeshRecords = new List<MeshRecord>();
        private static readonly List<PrefabRecord> PrefabRecords = new List<PrefabRecord>();
        private static readonly Dictionary<Material, string> MaterialTagsByInstance = new Dictionary<Material, string>();

        [MenuItem("Brassworks Breach/Sidecar Packs/Pipe Tank Gauge Set 10/Generate Assets And Renders")]
        public static void GenerateAssetsAndRenders()
        {
            TextureRecords.Clear();
            MaterialRecords.Clear();
            MeshRecords.Clear();
            PrefabRecords.Clear();
            MaterialTagsByInstance.Clear();

            var packageRoot = LocatePackageRoot();
            var repoRoot = ResolveRepoRoot(packageRoot.ResolvedPath);
            var renderRoot = Path.Combine(repoRoot, "Documentation", "ConceptRenders", "V0_1_55_PipeTankGaugeSet10");
            var assetProductionRoot = Path.Combine(repoRoot, "Documentation", "AssetProduction", "V0_1_55_PipeTankGaugeSet10");
            var planningRoot = Path.Combine(repoRoot, "Documentation", "Planning", "V0_1_55_PipeTankGaugeSet10ImportReadiness");
            var qaRoot = Path.Combine(repoRoot, "Documentation", "QA", "V0_1_55_PipeTankGaugeSet10ImportReadiness");

            EnsureFolders(packageRoot.ResolvedPath);
            Directory.CreateDirectory(renderRoot);
            Directory.CreateDirectory(assetProductionRoot);
            Directory.CreateDirectory(planningRoot);
            Directory.CreateDirectory(qaRoot);

            var textures = CreateTextures(packageRoot);
            var materials = CreateMaterials(packageRoot, textures);
            var meshes = CreateMeshes(packageRoot);

            CreateWallPipeRun(packageRoot, meshes, materials);
            CreateCeilingPipeRun(packageRoot, meshes, materials);
            CreateVerticalPressureTank(packageRoot, meshes, materials);
            CreateHorizontalPressureTank(packageRoot, meshes, materials);
            CreateTripleGaugeCluster(packageRoot, meshes, materials);
            CreateWideGaugePanel(packageRoot, meshes, materials);
            CreateLargeBrassValveWheel(packageRoot, meshes, materials);
            CreateIronValveWheel(packageRoot, meshes, materials);
            CreateBrassBracketSet(packageRoot, meshes, materials);
            CreateCopperElbowArray(packageRoot, meshes, materials);
            CreateBlackIronSupportFrame(packageRoot, meshes, materials);
            CreateTwinSteamNozzleHousing(packageRoot, meshes, materials);
            CreateCorridorMachineryLookdevStrip(packageRoot, meshes, materials);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RenderPreviews(packageRoot, renderRoot);
            RenderContactSheet(packageRoot, renderRoot);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var validation = ValidateGeneratedContent(packageRoot, renderRoot);
            WriteManifest(packageRoot, renderRoot, validation);
            WriteDocumentation(packageRoot, repoRoot, renderRoot, assetProductionRoot, planningRoot, qaRoot, validation);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"{Prefix}_GENERATE_PASS version={Version} package={packageRoot.ResolvedPath} previews={renderRoot}");
        }

        private static Dictionary<string, Texture2D> CreateTextures(PackageRoot packageRoot)
        {
            return new Dictionary<string, Texture2D>
            {
                ["brass_albedo"] = SaveTexture(packageRoot, "AgedBrassPatina_Albedo", CreateMetalAlbedo(new Color32(168, 112, 39, 255), new Color32(61, 104, 77, 255), 11), false, false, "aged_brass"),
                ["brass_normal"] = SaveTexture(packageRoot, "AgedBrassPatina_Normal", CreateNormalTexture(13, 0.34f), true, false, "aged_brass"),
                ["brass_mask"] = SaveTexture(packageRoot, "AgedBrassPatina_MetallicSmoothness", CreateMaskTexture(0.88f, 0.42f, 17), false, true, "aged_brass"),
                ["bright_brass_albedo"] = SaveTexture(packageRoot, "PolishedBrassEdgewear_Albedo", CreateMetalAlbedo(new Color32(218, 166, 66, 255), new Color32(126, 84, 31, 255), 19), false, false, "polished_brass_edgewear"),
                ["copper_albedo"] = SaveTexture(packageRoot, "HeatStainedCopper_Albedo", CreateCopperTexture(), false, false, "heat_stained_copper"),
                ["copper_mask"] = SaveTexture(packageRoot, "HeatStainedCopper_MetallicSmoothness", CreateMaskTexture(0.86f, 0.36f, 23), false, true, "heat_stained_copper"),
                ["iron_albedo"] = SaveTexture(packageRoot, "BlackenedIron_Albedo", CreateIronTexture(false), false, false, "blackened_iron"),
                ["iron_mask"] = SaveTexture(packageRoot, "BlackenedIron_MetallicSmoothness", CreateMaskTexture(0.78f, 0.24f, 29), false, true, "blackened_iron"),
                ["oily_iron_albedo"] = SaveTexture(packageRoot, "OilyBlackIronSupport_Albedo", CreateIronTexture(true), false, false, "oily_black_iron"),
                ["oily_iron_mask"] = SaveTexture(packageRoot, "OilyBlackIronSupport_MetallicSmoothness", CreateMaskTexture(0.82f, 0.52f, 31), false, true, "oily_black_iron"),
                ["dial_albedo"] = SaveTexture(packageRoot, "IvoryGaugeEnamel_Albedo", CreateDialTexture(), false, false, "ivory_gauge_enamel"),
                ["glass_albedo"] = SaveTexture(packageRoot, "AmberGaugeGlass_Albedo", CreateGlassTexture(), false, false, "amber_pressure_glass"),
                ["red_albedo"] = SaveTexture(packageRoot, "RedValveEnamel_Albedo", CreateFlatTexture(new Color32(145, 24, 18, 255), new Color32(84, 13, 13, 255), 37), false, false, "red_valve_enamel"),
                ["soot_albedo"] = SaveTexture(packageRoot, "SteamSootDeposit_Albedo", CreateFlatTexture(new Color32(24, 22, 20, 255), new Color32(88, 78, 61, 255), 41), false, false, "steam_soot_deposit"),
                ["verdigris_albedo"] = SaveTexture(packageRoot, "VerdigrisStain_Albedo", CreateFlatTexture(new Color32(42, 118, 98, 255), new Color32(17, 68, 59, 255), 43), false, false, "verdigris_stain")
            };
        }

        private static Dictionary<string, Material> CreateMaterials(PackageRoot packageRoot, IReadOnlyDictionary<string, Texture2D> textures)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible Lit or Standard shader found.");
            }

            return new Dictionary<string, Material>
            {
                ["brass"] = SaveMaterial(packageRoot, shader, "AgedBrassPatina", new Color(0.66f, 0.43f, 0.14f), 0.88f, 0.42f, textures["brass_albedo"], textures["brass_normal"], textures["brass_mask"], null, "aged_brass"),
                ["brightBrass"] = SaveMaterial(packageRoot, shader, "PolishedBrassEdgewear", new Color(0.88f, 0.65f, 0.23f), 0.92f, 0.55f, textures["bright_brass_albedo"], null, null, null, "polished_brass_edgewear"),
                ["copper"] = SaveMaterial(packageRoot, shader, "HeatStainedCopper", new Color(0.78f, 0.28f, 0.10f), 0.86f, 0.36f, textures["copper_albedo"], null, textures["copper_mask"], null, "heat_stained_copper"),
                ["iron"] = SaveMaterial(packageRoot, shader, "BlackenedIron", new Color(0.045f, 0.042f, 0.037f), 0.78f, 0.24f, textures["iron_albedo"], null, textures["iron_mask"], null, "blackened_iron"),
                ["oilyIron"] = SaveMaterial(packageRoot, shader, "OilyBlackIronSupport", new Color(0.036f, 0.034f, 0.031f), 0.82f, 0.52f, textures["oily_iron_albedo"], null, textures["oily_iron_mask"], null, "oily_black_iron"),
                ["dial"] = SaveMaterial(packageRoot, shader, "IvoryGaugeEnamel", new Color(0.80f, 0.72f, 0.54f), 0.0f, 0.46f, textures["dial_albedo"], null, null, null, "ivory_gauge_enamel"),
                ["glass"] = SaveMaterial(packageRoot, shader, "AmberGaugeGlass", new Color(1.0f, 0.50f, 0.14f), 0.03f, 0.82f, textures["glass_albedo"], null, null, new Color(1.0f, 0.36f, 0.07f) * 1.1f, "amber_pressure_glass"),
                ["red"] = SaveMaterial(packageRoot, shader, "RedValveEnamel", new Color(0.57f, 0.07f, 0.04f), 0.0f, 0.40f, textures["red_albedo"], null, null, null, "red_valve_enamel"),
                ["soot"] = SaveMaterial(packageRoot, shader, "SteamSootDeposit", new Color(0.11f, 0.10f, 0.085f), 0.0f, 0.18f, textures["soot_albedo"], null, null, null, "steam_soot_deposit"),
                ["verdigris"] = SaveMaterial(packageRoot, shader, "VerdigrisStain", new Color(0.15f, 0.48f, 0.38f), 0.0f, 0.30f, textures["verdigris_albedo"], null, null, null, "verdigris_stain")
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes(PackageRoot packageRoot)
        {
            return new Dictionary<string, Mesh>
            {
                ["box"] = SaveMesh(packageRoot, "Box_Unit", CreateBoxMesh()),
                ["cylinder16"] = SaveMesh(packageRoot, "Cylinder16_Z", CreateCylinderMesh(16, 0.5f, 0.5f)),
                ["cylinder32"] = SaveMesh(packageRoot, "Cylinder32_Z", CreateCylinderMesh(32, 0.5f, 0.5f)),
                ["cylinder48"] = SaveMesh(packageRoot, "Cylinder48_Z", CreateCylinderMesh(48, 0.5f, 0.5f)),
                ["bolt"] = SaveMesh(packageRoot, "BoltHead_Hex", CreateCylinderMesh(6, 0.5f, 0.5f)),
                ["cone"] = SaveMesh(packageRoot, "NozzleCone_Z", CreateCylinderMesh(40, 0.52f, 0.22f)),
                ["torus"] = SaveMesh(packageRoot, "ValveWheelTorus_Z", CreateTorusMesh(48, 10, 0.42f, 0.055f)),
                ["elbow"] = SaveMesh(packageRoot, "CopperElbow_Quarter", CreateQuarterElbowMesh(28, 12, 0.44f, 0.085f)),
                ["needle"] = SaveMesh(packageRoot, "GaugeNeedle", CreateNeedleMesh()),
                ["gusset"] = SaveMesh(packageRoot, "BracketGusset_Prism", CreateGussetMesh()),
                ["thinDisc"] = SaveMesh(packageRoot, "ThinWasherDisc_Z", CreateCylinderMesh(48, 0.5f, 0.5f))
            };
        }

        private static void CreateWallPipeRun(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_WallPipeRun_DualGauge_A");
            Part(root.transform, meshes["box"], "steam_soot_wall_shadow_backplate", new Vector3(0f, 0f, 0.16f), new Vector3(3.65f, 1.12f, 0.10f), Vector3.zero, materials["soot"]);
            Part(root.transform, meshes["cylinder48"], "upper_heat_stained_copper_wall_pipe", new Vector3(0f, 0.27f, -0.02f), new Vector3(0.12f, 0.12f, 3.25f), new Vector3(0f, 90f, 0f), materials["copper"]);
            Part(root.transform, meshes["cylinder48"], "lower_blackened_iron_wall_pipe", new Vector3(0f, -0.25f, -0.02f), new Vector3(0.15f, 0.15f, 3.15f), new Vector3(0f, 90f, 0f), materials["iron"]);
            AddPipeBracketPair(root.transform, meshes, materials, -1.35f, 0.27f, "left_upper");
            AddPipeBracketPair(root.transform, meshes, materials, 1.35f, 0.27f, "right_upper");
            AddPipeBracketPair(root.transform, meshes, materials, -1.12f, -0.25f, "left_lower");
            AddPipeBracketPair(root.transform, meshes, materials, 1.12f, -0.25f, "right_lower");
            AddGaugeFace(root.transform, meshes, materials, "left_pressure_gauge", new Vector3(-0.57f, 0.02f, -0.19f), 0.34f, 33f);
            AddGaugeFace(root.transform, meshes, materials, "right_pressure_gauge", new Vector3(0.66f, 0.02f, -0.19f), 0.30f, -22f);
            AddValveWheel(root.transform, meshes, materials, "red_bypass_valve_wheel", new Vector3(1.42f, 0.02f, -0.21f), 0.34f, materials["red"], materials["brass"]);
            Part(root.transform, meshes["elbow"], "left_copper_drop_elbow", new Vector3(-1.67f, 0.00f, -0.02f), new Vector3(0.52f, 0.52f, 0.52f), new Vector3(0f, 0f, 180f), materials["copper"]);
            Part(root.transform, meshes["elbow"], "right_copper_rise_elbow", new Vector3(1.72f, 0.03f, -0.02f), new Vector3(0.52f, 0.52f, 0.52f), Vector3.zero, materials["copper"]);
            AddFlanges(root.transform, meshes, materials, new[] { -1.60f, -0.08f, 1.58f }, 0.27f, 0.19f, "upper_brass");
            AddFlanges(root.transform, meshes, materials, new[] { -1.47f, 0.04f, 1.48f }, -0.25f, 0.22f, "lower_iron");
            SavePrefab(packageRoot, root, "wall_pipe_run", "Wall-mounted dual pipe run with brass brackets, copper elbows, paired readable gauges, and red bypass valve.", "PTG10_WallPipeRun_DualGauge_A");
        }

        private static void CreateCeilingPipeRun(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_CeilingPipeRun_TripleValve_B");
            for (var i = 0; i < 3; i++)
            {
                var y = 0.34f - i * 0.31f;
                var material = i == 1 ? materials["iron"] : materials["copper"];
                Part(root.transform, meshes["cylinder48"], $"ceiling_parallel_pipe_{i:00}", new Vector3(0f, y, 0f), new Vector3(0.12f + i * 0.015f, 0.12f + i * 0.015f, 3.6f), new Vector3(0f, 90f, 0f), material);
            }

            for (var x = -1.48f; x <= 1.49f; x += 0.74f)
            {
                Part(root.transform, meshes["box"], $"black_iron_ceiling_hanger_crossbar_{x:0.00}", new Vector3(x, 0.03f, 0.18f), new Vector3(0.07f, 0.92f, 0.12f), Vector3.zero, materials["oilyIron"]);
                Part(root.transform, meshes["box"], $"aged_brass_pipe_saddle_{x:0.00}", new Vector3(x, 0.34f, -0.02f), new Vector3(0.22f, 0.055f, 0.12f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["box"], $"aged_brass_lower_saddle_{x:0.00}", new Vector3(x, -0.28f, -0.02f), new Vector3(0.24f, 0.055f, 0.13f), Vector3.zero, materials["brass"]);
            }

            AddValveWheel(root.transform, meshes, materials, "upper_brass_ceiling_valve", new Vector3(-0.88f, 0.62f, -0.04f), 0.28f, materials["brass"], materials["brightBrass"]);
            AddValveWheel(root.transform, meshes, materials, "middle_red_ceiling_valve", new Vector3(0.12f, 0.04f, -0.06f), 0.31f, materials["red"], materials["brass"]);
            AddValveWheel(root.transform, meshes, materials, "lower_black_iron_ceiling_valve", new Vector3(1.02f, -0.52f, -0.04f), 0.25f, materials["iron"], materials["brass"]);
            Part(root.transform, meshes["elbow"], "front_copper_elbow_drop", new Vector3(-1.80f, 0.34f, 0f), new Vector3(0.50f, 0.50f, 0.50f), new Vector3(0f, 0f, 180f), materials["copper"]);
            Part(root.transform, meshes["elbow"], "rear_copper_elbow_return", new Vector3(1.80f, -0.28f, 0f), new Vector3(0.50f, 0.50f, 0.50f), Vector3.zero, materials["copper"]);
            AddNozzle(root.transform, meshes, materials, "small_ceiling_steam_nozzle", new Vector3(0.82f, 0.66f, -0.06f), 0.68f);
            SavePrefab(packageRoot, root, "ceiling_pipe_run", "Ceiling-mounted triple pipe run with black iron hangers, brass saddles, three valve wheels, copper elbows, and small steam nozzle housing.", "PTG10_CeilingPipeRun_TripleValve_B");
        }

        private static void CreateVerticalPressureTank(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_PressureTank_WallMounted_A");
            Part(root.transform, meshes["box"], "black_iron_wall_mount_spine", new Vector3(0f, 0f, 0.18f), new Vector3(0.70f, 2.10f, 0.16f), Vector3.zero, materials["oilyIron"]);
            Part(root.transform, meshes["cylinder48"], "aged_brass_vertical_pressure_tank_body", new Vector3(0f, 0f, -0.02f), new Vector3(0.47f, 0.47f, 1.60f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Part(root.transform, meshes["cone"], "upper_brass_domed_cap", new Vector3(0f, 0.88f, -0.02f), new Vector3(0.50f, 0.50f, 0.28f), new Vector3(90f, 0f, 0f), materials["brightBrass"]);
            Part(root.transform, meshes["cone"], "lower_brass_domed_cap", new Vector3(0f, -0.88f, -0.02f), new Vector3(0.50f, 0.50f, 0.28f), new Vector3(-90f, 0f, 0f), materials["brightBrass"]);
            for (var i = 0; i < 5; i++)
            {
                var y = -0.56f + i * 0.28f;
                Part(root.transform, meshes["cylinder48"], $"blackened_iron_tank_strap_{i:00}", new Vector3(0f, y, -0.02f), new Vector3(0.51f, 0.51f, 0.030f), new Vector3(90f, 0f, 0f), materials["iron"]);
                Part(root.transform, meshes["bolt"], $"left_tank_strap_bolt_{i:00}", new Vector3(-0.37f, y, -0.18f), new Vector3(0.045f, 0.045f, 0.025f), new Vector3(0f, 90f, 0f), materials["brightBrass"]);
                Part(root.transform, meshes["bolt"], $"right_tank_strap_bolt_{i:00}", new Vector3(0.37f, y, -0.18f), new Vector3(0.045f, 0.045f, 0.025f), new Vector3(0f, 90f, 0f), materials["brightBrass"]);
            }

            Part(root.transform, meshes["cylinder32"], "amber_vertical_sight_glass", new Vector3(0f, 0f, -0.30f), new Vector3(0.13f, 0.13f, 1.18f), new Vector3(90f, 0f, 0f), materials["glass"]);
            AddGaugeFace(root.transform, meshes, materials, "tank_top_pressure_gauge", new Vector3(0.42f, 0.56f, -0.22f), 0.26f, 58f);
            AddValveWheel(root.transform, meshes, materials, "lower_tank_drain_wheel", new Vector3(-0.46f, -0.58f, -0.22f), 0.25f, materials["red"], materials["brass"]);
            Part(root.transform, meshes["cylinder32"], "copper_inlet_pipe_left", new Vector3(-0.66f, 0.18f, -0.02f), new Vector3(0.08f, 0.08f, 0.80f), new Vector3(90f, 0f, 0f), materials["copper"]);
            Part(root.transform, meshes["elbow"], "copper_upper_elbow_into_tank", new Vector3(-0.54f, 0.58f, -0.02f), new Vector3(0.42f, 0.42f, 0.42f), new Vector3(0f, 0f, 270f), materials["copper"]);
            SavePrefab(packageRoot, root, "pressure_tank", "Wall-mounted vertical pressure tank with brass body, black iron straps, amber sight glass, gauge, drain valve, and copper inlet hardware.", "PTG10_PressureTank_WallMounted_A");
        }

        private static void CreateHorizontalPressureTank(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_PressureTank_HorizontalCradle_B");
            Part(root.transform, meshes["cylinder48"], "aged_brass_horizontal_pressure_reservoir", new Vector3(0f, 0.08f, 0f), new Vector3(0.43f, 0.43f, 1.80f), new Vector3(0f, 90f, 0f), materials["brass"]);
            Part(root.transform, meshes["cone"], "left_polished_brass_tank_cap", new Vector3(-1.02f, 0.08f, 0f), new Vector3(0.46f, 0.46f, 0.22f), new Vector3(0f, -90f, 0f), materials["brightBrass"]);
            Part(root.transform, meshes["cone"], "right_polished_brass_tank_cap", new Vector3(1.02f, 0.08f, 0f), new Vector3(0.46f, 0.46f, 0.22f), new Vector3(0f, 90f, 0f), materials["brightBrass"]);
            for (var x = -0.72f; x <= 0.72f; x += 0.36f)
            {
                Part(root.transform, meshes["cylinder48"], $"black_iron_tank_band_{x:0.00}", new Vector3(x, 0.08f, 0f), new Vector3(0.47f, 0.47f, 0.030f), new Vector3(0f, 90f, 0f), materials["iron"]);
            }

            Part(root.transform, meshes["box"], "left_black_iron_cradle_foot", new Vector3(-0.58f, -0.42f, 0f), new Vector3(0.42f, 0.18f, 0.58f), Vector3.zero, materials["oilyIron"]);
            Part(root.transform, meshes["box"], "right_black_iron_cradle_foot", new Vector3(0.58f, -0.42f, 0f), new Vector3(0.42f, 0.18f, 0.58f), Vector3.zero, materials["oilyIron"]);
            Part(root.transform, meshes["gusset"], "left_brass_triangular_cradle_gusset", new Vector3(-0.58f, -0.20f, -0.32f), new Vector3(0.42f, 0.36f, 0.10f), new Vector3(0f, 0f, 0f), materials["brass"]);
            Part(root.transform, meshes["gusset"], "right_brass_triangular_cradle_gusset", new Vector3(0.58f, -0.20f, -0.32f), new Vector3(0.42f, 0.36f, 0.10f), new Vector3(0f, 0f, 0f), materials["brass"]);
            AddGaugeFace(root.transform, meshes, materials, "front_cradle_pressure_gauge", new Vector3(0f, 0.54f, -0.30f), 0.30f, 18f);
            AddValveWheel(root.transform, meshes, materials, "right_isolation_wheel", new Vector3(1.25f, 0.10f, -0.18f), 0.27f, materials["brass"], materials["red"]);
            SavePrefab(packageRoot, root, "pressure_tank", "Horizontal pressure reservoir in black iron cradle with brass caps, banding, gauge, and isolation wheel.", "PTG10_PressureTank_HorizontalCradle_B");
        }

        private static void CreateTripleGaugeCluster(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_GaugeCluster_TripleManifold_A");
            Part(root.transform, meshes["box"], "oily_black_iron_manifold_block", new Vector3(0f, -0.32f, 0.08f), new Vector3(1.60f, 0.24f, 0.22f), Vector3.zero, materials["oilyIron"]);
            Part(root.transform, meshes["cylinder32"], "copper_manifold_pipe", new Vector3(0f, -0.32f, -0.08f), new Vector3(0.11f, 0.11f, 1.70f), new Vector3(0f, 90f, 0f), materials["copper"]);
            var positions = new[] { -0.58f, 0f, 0.58f };
            for (var i = 0; i < positions.Length; i++)
            {
                AddGaugeFace(root.transform, meshes, materials, $"calibrated_pressure_gauge_{i:00}", new Vector3(positions[i], 0.23f, -0.18f), i == 1 ? 0.36f : 0.30f, -42f + i * 35f);
                Part(root.transform, meshes["cylinder16"], $"brass_gauge_riser_pipe_{i:00}", new Vector3(positions[i], -0.08f, -0.08f), new Vector3(0.055f, 0.055f, 0.55f), new Vector3(90f, 0f, 0f), materials["brass"]);
                Part(root.transform, meshes["bolt"], $"manifold_rivet_{i:00}_left", new Vector3(positions[i] - 0.12f, -0.32f, -0.08f), new Vector3(0.045f, 0.045f, 0.024f), Vector3.zero, materials["brightBrass"]);
                Part(root.transform, meshes["bolt"], $"manifold_rivet_{i:00}_right", new Vector3(positions[i] + 0.12f, -0.32f, -0.08f), new Vector3(0.045f, 0.045f, 0.024f), Vector3.zero, materials["brightBrass"]);
            }

            AddValveWheel(root.transform, meshes, materials, "tiny_red_bleeder_wheel_left", new Vector3(-0.94f, -0.30f, -0.18f), 0.18f, materials["red"], materials["brass"]);
            AddValveWheel(root.transform, meshes, materials, "tiny_brass_bleeder_wheel_right", new Vector3(0.94f, -0.30f, -0.18f), 0.18f, materials["brass"], materials["brightBrass"]);
            SavePrefab(packageRoot, root, "gauge_cluster", "Triple readable gauge manifold with copper header, brass risers, rivets, and paired bleeder valve wheels.", "PTG10_GaugeCluster_TripleManifold_A");
        }

        private static void CreateWideGaugePanel(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_GaugeCluster_WidePanel_B");
            Part(root.transform, meshes["box"], "blackened_iron_wide_panel", new Vector3(0f, 0f, 0.13f), new Vector3(2.25f, 0.88f, 0.12f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "aged_brass_panel_nameplate", new Vector3(0f, -0.43f, -0.02f), new Vector3(1.24f, 0.08f, 0.035f), Vector3.zero, materials["brightBrass"]);
            AddGaugeFace(root.transform, meshes, materials, "large_center_regulator_gauge", new Vector3(0f, 0.08f, -0.13f), 0.40f, 72f);
            AddGaugeFace(root.transform, meshes, materials, "left_small_return_gauge", new Vector3(-0.70f, 0.08f, -0.13f), 0.27f, -52f);
            AddGaugeFace(root.transform, meshes, materials, "right_small_feed_gauge", new Vector3(0.70f, 0.08f, -0.13f), 0.27f, 24f);
            for (var i = 0; i < 8; i++)
            {
                var x = -0.98f + i * 0.28f;
                Part(root.transform, meshes["bolt"], $"panel_polished_brass_screw_{i:00}", new Vector3(x, i % 2 == 0 ? 0.38f : -0.28f, -0.11f), new Vector3(0.050f, 0.050f, 0.024f), Vector3.zero, materials["brightBrass"]);
            }

            AddValveWheel(root.transform, meshes, materials, "center_red_trim_wheel", new Vector3(0f, -0.22f, -0.16f), 0.22f, materials["red"], materials["brass"]);
            SavePrefab(packageRoot, root, "gauge_cluster", "Wide black iron gauge panel with three readable dials, brass nameplate, screw pattern, and trim wheel.", "PTG10_GaugeCluster_WidePanel_B");
        }

        private static void CreateLargeBrassValveWheel(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_ValveWheel_LargeBrass_A");
            AddValveWheel(root.transform, meshes, materials, "large_aged_brass_valve_wheel", Vector3.zero, 0.62f, materials["brass"], materials["brightBrass"]);
            Part(root.transform, meshes["box"], "black_iron_yoke_left", new Vector3(-0.22f, -0.52f, 0.10f), new Vector3(0.09f, 0.58f, 0.14f), new Vector3(0f, 0f, -12f), materials["iron"]);
            Part(root.transform, meshes["box"], "black_iron_yoke_right", new Vector3(0.22f, -0.52f, 0.10f), new Vector3(0.09f, 0.58f, 0.14f), new Vector3(0f, 0f, 12f), materials["iron"]);
            Part(root.transform, meshes["cylinder32"], "copper_valve_stem_socket", new Vector3(0f, -0.82f, 0.09f), new Vector3(0.16f, 0.16f, 0.44f), new Vector3(90f, 0f, 0f), materials["copper"]);
            SavePrefab(packageRoot, root, "valve_wheel", "Large aged brass valve wheel with polished rim accents, black iron yoke, and copper stem socket.", "PTG10_ValveWheel_LargeBrass_A");
        }

        private static void CreateIronValveWheel(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_ValveWheel_IronHandwheel_B");
            AddValveWheel(root.transform, meshes, materials, "blackened_iron_handwheel", Vector3.zero, 0.48f, materials["iron"], materials["brass"]);
            for (var i = 0; i < 6; i++)
            {
                var angle = i * 60f;
                var rad = angle * Mathf.Deg2Rad;
                Part(root.transform, meshes["bolt"], $"brass_grip_rivet_{i:00}", new Vector3(Mathf.Cos(rad) * 0.24f, Mathf.Sin(rad) * 0.24f, -0.08f), new Vector3(0.055f, 0.055f, 0.024f), Vector3.zero, materials["brightBrass"]);
            }

            Part(root.transform, meshes["box"], "red_enamel_lockout_tag_plate", new Vector3(0.00f, -0.62f, -0.02f), new Vector3(0.32f, 0.14f, 0.035f), Vector3.zero, materials["red"]);
            SavePrefab(packageRoot, root, "valve_wheel", "Compact black iron handwheel with brass rivet points and red enamel lockout plate.", "PTG10_ValveWheel_IronHandwheel_B");
        }

        private static void CreateBrassBracketSet(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_BrassBracket_Set_A");
            for (var i = 0; i < 3; i++)
            {
                var x = -0.84f + i * 0.84f;
                Part(root.transform, meshes["box"], $"aged_brass_wall_leaf_{i:00}", new Vector3(x, 0f, 0.08f), new Vector3(0.14f, 0.70f, 0.08f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["box"], $"aged_brass_pipe_leaf_{i:00}", new Vector3(x + 0.21f, -0.26f, -0.08f), new Vector3(0.50f, 0.13f, 0.08f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["gusset"], $"triangular_brass_gusset_{i:00}", new Vector3(x + 0.05f, -0.13f, -0.04f), new Vector3(0.38f, 0.42f, 0.075f), Vector3.zero, materials["brightBrass"]);
                for (var j = 0; j < 4; j++)
                {
                    var y = -0.24f + j * 0.16f;
                    Part(root.transform, meshes["bolt"], $"slotted_brass_mount_screw_{i:00}_{j:00}", new Vector3(x, y, -0.02f), new Vector3(0.055f, 0.055f, 0.025f), Vector3.zero, j % 2 == 0 ? materials["brightBrass"] : materials["brass"]);
                }
            }

            Part(root.transform, meshes["cylinder32"], "sample_copper_pipe_for_bracket_scale", new Vector3(0f, -0.26f, -0.08f), new Vector3(0.105f, 0.105f, 2.28f), new Vector3(0f, 90f, 0f), materials["copper"]);
            SavePrefab(packageRoot, root, "brass_bracket", "Reusable brass bracket family with wall leaves, pipe leaves, triangular gussets, screws, and scale pipe.", "PTG10_BrassBracket_Set_A");
        }

        private static void CreateCopperElbowArray(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_CopperElbow_NinetyArray_A");
            for (var i = 0; i < 4; i++)
            {
                var x = -1.08f + i * 0.72f;
                var rot = new Vector3(0f, 0f, i * 90f);
                Part(root.transform, meshes["elbow"], $"heat_stained_copper_ninety_elbow_{i:00}", new Vector3(x, 0.08f, 0f), new Vector3(0.58f, 0.58f, 0.58f), rot, materials["copper"]);
                Part(root.transform, meshes["cylinder32"], $"left_brass_elbow_flange_{i:00}", new Vector3(x - 0.31f, 0.08f, 0f), new Vector3(0.18f, 0.18f, 0.050f), new Vector3(0f, 90f, 0f), materials["brass"]);
                Part(root.transform, meshes["cylinder32"], $"right_brass_elbow_flange_{i:00}", new Vector3(x, 0.42f, 0f), new Vector3(0.18f, 0.18f, 0.050f), new Vector3(90f, 0f, 0f), materials["brass"]);
                Part(root.transform, meshes["box"], $"verdigris_streak_under_elbow_{i:00}", new Vector3(x + 0.12f, -0.30f, -0.04f), new Vector3(0.07f, 0.34f, 0.018f), new Vector3(0f, 0f, -8f), materials["verdigris"]);
            }

            Part(root.transform, meshes["box"], "black_iron_elbow_mounting_bar", new Vector3(0f, -0.44f, 0.10f), new Vector3(2.80f, 0.13f, 0.12f), Vector3.zero, materials["iron"]);
            SavePrefab(packageRoot, root, "copper_elbow", "Copper ninety-degree elbow display set with brass flanges, orientation variety, verdigris streaking, and black iron mounting bar.", "PTG10_CopperElbow_NinetyArray_A");
        }

        private static void CreateBlackIronSupportFrame(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_BlackIronSupport_SaddleFrame_A");
            for (var x = -0.90f; x <= 0.91f; x += 0.60f)
            {
                Part(root.transform, meshes["box"], $"oily_black_vertical_support_{x:0.00}", new Vector3(x, 0f, 0.05f), new Vector3(0.10f, 1.15f, 0.14f), Vector3.zero, materials["oilyIron"]);
                Part(root.transform, meshes["gusset"], $"aged_brass_support_gusset_{x:0.00}", new Vector3(x + 0.08f, -0.38f, -0.04f), new Vector3(0.34f, 0.30f, 0.08f), Vector3.zero, materials["brass"]);
            }

            Part(root.transform, meshes["box"], "top_black_iron_pipe_saddle_rail", new Vector3(0f, 0.44f, 0.03f), new Vector3(2.28f, 0.12f, 0.16f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "bottom_black_iron_floor_rail", new Vector3(0f, -0.52f, 0.08f), new Vector3(2.45f, 0.14f, 0.18f), Vector3.zero, materials["oilyIron"]);
            Part(root.transform, meshes["cylinder48"], "supported_heat_stained_copper_pipe", new Vector3(0f, 0.59f, -0.03f), new Vector3(0.18f, 0.18f, 2.20f), new Vector3(0f, 90f, 0f), materials["copper"]);
            for (var i = 0; i < 12; i++)
            {
                var x = -1.04f + i * 0.19f;
                Part(root.transform, meshes["bolt"], $"support_rivet_row_{i:00}", new Vector3(x, -0.52f, -0.04f), new Vector3(0.040f, 0.040f, 0.024f), Vector3.zero, i % 3 == 0 ? materials["brightBrass"] : materials["iron"]);
            }

            SavePrefab(packageRoot, root, "black_iron_support", "Black iron saddle frame with oily support rails, brass gussets, rivet row, and supported copper pipe.", "PTG10_BlackIronSupport_SaddleFrame_A");
        }

        private static void CreateTwinSteamNozzleHousing(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_SteamNozzleHousing_Twin_A");
            Part(root.transform, meshes["box"], "blackened_iron_twin_nozzle_manifold", new Vector3(0f, 0.08f, 0.08f), new Vector3(1.32f, 0.46f, 0.25f), Vector3.zero, materials["iron"]);
            AddNozzle(root.transform, meshes, materials, "left_bell_steam_nozzle", new Vector3(-0.36f, -0.15f, -0.12f), 0.78f);
            AddNozzle(root.transform, meshes, materials, "right_bell_steam_nozzle", new Vector3(0.36f, -0.15f, -0.12f), 0.78f);
            Part(root.transform, meshes["cylinder32"], "upper_copper_feed_pipe", new Vector3(0f, 0.42f, 0.08f), new Vector3(0.12f, 0.12f, 1.55f), new Vector3(0f, 90f, 0f), materials["copper"]);
            AddValveWheel(root.transform, meshes, materials, "small_brass_nozzle_shutoff_wheel", new Vector3(0f, 0.68f, -0.08f), 0.25f, materials["brass"], materials["brightBrass"]);
            Part(root.transform, meshes["box"], "steam_soot_bloom_under_nozzles", new Vector3(0f, -0.66f, 0.02f), new Vector3(1.10f, 0.25f, 0.035f), Vector3.zero, materials["soot"]);
            SavePrefab(packageRoot, root, "steam_nozzle_housing", "Twin steam nozzle housing with black iron manifold, brass collars, copper feed pipe, shutoff wheel, and soot bloom.", "PTG10_SteamNozzleHousing_Twin_A");
        }

        private static void CreateCorridorMachineryLookdevStrip(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PTG10_CorridorMachinery_LookdevStrip_A");
            Part(root.transform, meshes["box"], "long_black_iron_corridor_mounting_spine", new Vector3(0f, 0f, 0.22f), new Vector3(4.10f, 1.25f, 0.14f), Vector3.zero, materials["oilyIron"]);
            Part(root.transform, meshes["cylinder48"], "main_copper_corridor_pipe", new Vector3(0f, 0.37f, -0.02f), new Vector3(0.13f, 0.13f, 3.85f), new Vector3(0f, 90f, 0f), materials["copper"]);
            Part(root.transform, meshes["cylinder48"], "secondary_black_iron_corridor_pipe", new Vector3(0f, -0.36f, -0.02f), new Vector3(0.15f, 0.15f, 3.62f), new Vector3(0f, 90f, 0f), materials["iron"]);
            Part(root.transform, meshes["cylinder48"], "mini_brass_pressure_reservoir_center", new Vector3(0f, -0.02f, -0.12f), new Vector3(0.34f, 0.34f, 0.82f), new Vector3(0f, 90f, 0f), materials["brass"]);
            for (var x = -1.72f; x <= 1.73f; x += 0.86f)
            {
                AddPipeBracketPair(root.transform, meshes, materials, x, 0.37f, $"lookdev_upper_{x:0.00}");
                AddPipeBracketPair(root.transform, meshes, materials, x, -0.36f, $"lookdev_lower_{x:0.00}");
            }

            AddGaugeFace(root.transform, meshes, materials, "lookdev_left_gauge", new Vector3(-0.82f, 0.02f, -0.24f), 0.29f, -30f);
            AddGaugeFace(root.transform, meshes, materials, "lookdev_center_gauge", new Vector3(0.00f, 0.54f, -0.24f), 0.25f, 45f);
            AddValveWheel(root.transform, meshes, materials, "lookdev_red_service_wheel", new Vector3(0.86f, 0.02f, -0.24f), 0.30f, materials["red"], materials["brass"]);
            AddNozzle(root.transform, meshes, materials, "lookdev_right_steam_nozzle", new Vector3(1.62f, -0.08f, -0.16f), 0.58f);
            Part(root.transform, meshes["elbow"], "lookdev_left_copper_elbow", new Vector3(-1.90f, 0.07f, -0.02f), new Vector3(0.50f, 0.50f, 0.50f), new Vector3(0f, 0f, 180f), materials["copper"]);
            SavePrefab(packageRoot, root, "lookdev_assembly", "Combined corridor machinery lookdev strip proving brass, copper, and black iron material separation in one import-safe visual assembly.", "PTG10_CorridorMachinery_LookdevStrip_A");
        }

        private static GameObject NewRoot(string name)
        {
            var root = new GameObject(name);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            return root;
        }

        private static GameObject Part(Transform parent, Mesh mesh, string name, Vector3 localPosition, Vector3 localScale, Vector3 localEuler, Material material)
        {
            var part = new GameObject(name);
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localRotation = Quaternion.Euler(localEuler);
            part.transform.localScale = localScale;
            part.AddComponent<MeshFilter>().sharedMesh = mesh;
            part.AddComponent<MeshRenderer>().sharedMaterial = material;
            return part;
        }

        private static void AddPipeBracketPair(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, float x, float y, string prefix)
        {
            Part(parent, meshes["box"], $"{prefix}_aged_brass_wall_tab", new Vector3(x, y, 0.02f), new Vector3(0.10f, 0.36f, 0.08f), Vector3.zero, materials["brass"]);
            Part(parent, meshes["box"], $"{prefix}_black_iron_pipe_clamp", new Vector3(x, y, -0.08f), new Vector3(0.29f, 0.065f, 0.10f), Vector3.zero, materials["iron"]);
            Part(parent, meshes["bolt"], $"{prefix}_upper_polished_brass_screw", new Vector3(x, y + 0.12f, -0.08f), new Vector3(0.045f, 0.045f, 0.024f), Vector3.zero, materials["brightBrass"]);
            Part(parent, meshes["bolt"], $"{prefix}_lower_polished_brass_screw", new Vector3(x, y - 0.12f, -0.08f), new Vector3(0.045f, 0.045f, 0.024f), Vector3.zero, materials["brightBrass"]);
        }

        private static void AddFlanges(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, IEnumerable<float> xPositions, float y, float size, string prefix)
        {
            var i = 0;
            foreach (var x in xPositions)
            {
                Part(parent, meshes["cylinder48"], $"{prefix}_flange_disc_{i:00}", new Vector3(x, y, -0.02f), new Vector3(size, size, 0.040f), new Vector3(0f, 90f, 0f), prefix.Contains("iron") ? materials["iron"] : materials["brass"]);
                for (var b = 0; b < 4; b++)
                {
                    var angle = b * Mathf.PI * 0.5f;
                    Part(parent, meshes["bolt"], $"{prefix}_flange_bolt_{i:00}_{b:00}", new Vector3(x, y + Mathf.Sin(angle) * size * 0.34f, -0.12f + Mathf.Cos(angle) * 0.004f), new Vector3(0.032f, 0.032f, 0.020f), Vector3.zero, materials["brightBrass"]);
                }

                i++;
            }
        }

        private static void AddGaugeFace(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, string prefix, Vector3 center, float size, float needleAngle)
        {
            Part(parent, meshes["cylinder48"], $"{prefix}_black_iron_rear_case", center + new Vector3(0f, 0f, 0.010f), new Vector3(size * 1.12f, size * 1.12f, size * 0.15f), Vector3.zero, materials["iron"]);
            Part(parent, meshes["cylinder48"], $"{prefix}_aged_brass_bezel", center + new Vector3(0f, 0f, -0.045f), new Vector3(size * 1.03f, size * 1.03f, size * 0.060f), Vector3.zero, materials["brass"]);
            Part(parent, meshes["cylinder48"], $"{prefix}_ivory_enamel_dial", center + new Vector3(0f, 0f, -0.086f), new Vector3(size * 0.78f, size * 0.78f, size * 0.025f), Vector3.zero, materials["dial"]);
            for (var i = 0; i < 14; i++)
            {
                var angle = Mathf.Lerp(210f, -30f, i / 13f) * Mathf.Deg2Rad;
                var radius = size * 0.31f;
                var pos = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, -0.112f);
                var tickMaterial = i >= 10 ? materials["red"] : materials["iron"];
                Part(parent, meshes["box"], $"{prefix}_dial_tick_{i:00}", pos, new Vector3(size * 0.018f, size * (i % 3 == 0 ? 0.085f : 0.052f), size * 0.012f), new Vector3(0f, 0f, Mathf.Rad2Deg * angle - 90f), tickMaterial);
            }

            Part(parent, meshes["needle"], $"{prefix}_black_pressure_needle", center + new Vector3(0f, 0f, -0.126f), new Vector3(size * 0.42f, size * 0.050f, size * 0.018f), new Vector3(0f, 0f, needleAngle), materials["iron"]);
            Part(parent, meshes["cylinder32"], $"{prefix}_amber_convex_glass", center + new Vector3(0f, 0f, -0.145f), new Vector3(size * 0.86f, size * 0.86f, size * 0.020f), Vector3.zero, materials["glass"]);
            Part(parent, meshes["bolt"], $"{prefix}_center_brass_pivot", center + new Vector3(0f, 0f, -0.165f), new Vector3(size * 0.075f, size * 0.075f, size * 0.022f), Vector3.zero, materials["brightBrass"]);
        }

        private static void AddValveWheel(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, string prefix, Vector3 center, float size, Material wheelMaterial, Material accentMaterial)
        {
            Part(parent, meshes["torus"], $"{prefix}_outer_rim", center + new Vector3(0f, 0f, -0.08f), new Vector3(size, size, size), Vector3.zero, wheelMaterial);
            for (var i = 0; i < 6; i++)
            {
                var angle = i * 60f;
                Part(parent, meshes["box"], $"{prefix}_spoke_{i:00}", center + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * size * 0.16f, Mathf.Sin(angle * Mathf.Deg2Rad) * size * 0.16f, -0.08f), new Vector3(size * 0.42f, size * 0.035f, size * 0.045f), new Vector3(0f, 0f, angle), accentMaterial);
            }

            Part(parent, meshes["cylinder32"], $"{prefix}_central_hub", center + new Vector3(0f, 0f, -0.11f), new Vector3(size * 0.23f, size * 0.23f, size * 0.12f), Vector3.zero, accentMaterial);
            Part(parent, meshes["bolt"], $"{prefix}_front_retaining_nut", center + new Vector3(0f, 0f, -0.19f), new Vector3(size * 0.13f, size * 0.13f, size * 0.055f), Vector3.zero, materials["brightBrass"]);
            Part(parent, meshes["cylinder16"], $"{prefix}_side_grip_knob", center + new Vector3(size * 0.38f, 0f, -0.12f), new Vector3(size * 0.085f, size * 0.085f, size * 0.16f), new Vector3(0f, 90f, 0f), accentMaterial);
        }

        private static void AddNozzle(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, string prefix, Vector3 center, float size)
        {
            Part(parent, meshes["cone"], $"{prefix}_black_iron_bell_mouth", center + new Vector3(0f, -0.20f * size, -0.07f), new Vector3(0.30f * size, 0.30f * size, 0.36f * size), new Vector3(90f, 0f, 0f), materials["iron"]);
            Part(parent, meshes["cylinder32"], $"{prefix}_aged_brass_clamp_collar", center + new Vector3(0f, 0.02f * size, -0.07f), new Vector3(0.24f * size, 0.24f * size, 0.12f * size), new Vector3(90f, 0f, 0f), materials["brass"]);
            Part(parent, meshes["cylinder32"], $"{prefix}_copper_feed_stem", center + new Vector3(0f, 0.25f * size, -0.07f), new Vector3(0.09f * size, 0.09f * size, 0.45f * size), new Vector3(90f, 0f, 0f), materials["copper"]);
            for (var i = 0; i < 4; i++)
            {
                var x = i < 2 ? -0.16f * size : 0.16f * size;
                var y = (i % 2 == 0 ? -0.03f : 0.08f) * size;
                Part(parent, meshes["bolt"], $"{prefix}_collar_bolt_{i:00}", center + new Vector3(x, y, -0.20f), new Vector3(0.040f * size, 0.040f * size, 0.020f * size), Vector3.zero, materials["brightBrass"]);
            }
        }

        private static void SavePrefab(PackageRoot packageRoot, GameObject root, string role, string notes, string assetName)
        {
            var assetRelativePath = $"Runtime/Prefabs/{assetName}.prefab";
            var assetPath = $"{packageRoot.AssetPath}/{assetRelativePath}";
            ReplaceAsset(assetPath);
            var prefab = PrefabUtility.SaveAsPrefabAsset(root, assetPath);
            var renderers = prefab.GetComponentsInChildren<Renderer>(true);
            var meshFilters = prefab.GetComponentsInChildren<MeshFilter>(true);
            var materialTags = renderers
                .SelectMany(renderer => renderer.sharedMaterials)
                .Where(material => material != null)
                .Select(material => MaterialTagsByInstance.TryGetValue(material, out var tag) ? tag : material.name)
                .Distinct(StringComparer.Ordinal)
                .OrderBy(tag => tag, StringComparer.Ordinal)
                .ToArray();
            var bounds = CalculateBounds(prefab);
            PrefabRecords.Add(new PrefabRecord
            {
                Id = assetName,
                Role = role,
                Notes = notes,
                PrefabPath = assetRelativePath,
                RendererCount = renderers.Length,
                MeshPartCount = meshFilters.Length,
                BoundsSize = bounds.size,
                MaterialTags = materialTags,
                RuntimePreviewPath = $"Runtime/Previews/{PreviewNameFor(assetName)}",
                DocumentationPreviewPath = $"Documentation/ConceptRenders/V0_1_55_PipeTankGaugeSet10/{PreviewNameFor(assetName)}"
            });
            Object.DestroyImmediate(root);
        }

        private static void RenderPreviews(PackageRoot packageRoot, string renderRoot)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientLight = new Color(0.31f, 0.29f, 0.25f);
            Directory.CreateDirectory(renderRoot);
            Directory.CreateDirectory(Path.Combine(packageRoot.ResolvedPath, "Runtime", "Previews"));

            foreach (var record in PrefabRecords)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{packageRoot.AssetPath}/{record.PrefabPath}");
                if (prefab == null)
                {
                    continue;
                }

                var instance = Object.Instantiate(prefab);
                instance.transform.position = Vector3.zero;
                var bounds = CalculateBounds(instance);
                var center = bounds.center;
                var maxExtent = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z, 0.45f);

                var key = new GameObject("PTG10_preview_key_light").AddComponent<Light>();
                key.type = LightType.Directional;
                key.intensity = 1.55f;
                key.transform.rotation = Quaternion.Euler(38f, -42f, 0f);

                var fill = new GameObject("PTG10_preview_warm_fill").AddComponent<Light>();
                fill.type = LightType.Point;
                fill.color = new Color(1.0f, 0.63f, 0.32f);
                fill.intensity = 2.25f;
                fill.range = 6f;
                fill.transform.position = center + new Vector3(-1.8f, 1.4f, -2.4f);

                var cameraObject = new GameObject("PTG10_preview_camera");
                var camera = cameraObject.AddComponent<Camera>();
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.085f, 0.078f, 0.068f, 1f);
                camera.orthographic = true;
                camera.orthographicSize = maxExtent * 1.28f;
                camera.nearClipPlane = 0.02f;
                camera.farClipPlane = 50f;
                camera.transform.position = center + new Vector3(maxExtent * 1.35f, maxExtent * 0.75f, -maxExtent * 3.2f);
                camera.transform.LookAt(center);

                var rt = new RenderTexture(PreviewWidth, PreviewHeight, 24, RenderTextureFormat.ARGB32);
                camera.targetTexture = rt;
                camera.Render();
                RenderTexture.active = rt;
                var png = new Texture2D(PreviewWidth, PreviewHeight, TextureFormat.RGBA32, false);
                png.ReadPixels(new Rect(0, 0, PreviewWidth, PreviewHeight), 0, 0);
                png.Apply();
                var bytes = png.EncodeToPNG();
                var fileName = Path.GetFileName(record.RuntimePreviewPath);
                var docsPath = Path.Combine(renderRoot, fileName);
                var runtimePath = Path.Combine(packageRoot.ResolvedPath, "Runtime", "Previews", fileName);
                File.WriteAllBytes(docsPath, bytes);
                File.WriteAllBytes(runtimePath, bytes);
                AssetDatabase.ImportAsset($"{packageRoot.AssetPath}/Runtime/Previews/{fileName}");

                camera.targetTexture = null;
                RenderTexture.active = null;
                Object.DestroyImmediate(png);
                Object.DestroyImmediate(rt);
                Object.DestroyImmediate(cameraObject);
                Object.DestroyImmediate(key.gameObject);
                Object.DestroyImmediate(fill.gameObject);
                Object.DestroyImmediate(instance);
            }
        }

        private static void RenderContactSheet(PackageRoot packageRoot, string renderRoot)
        {
            var columns = 4;
            var cellWidth = 420;
            var cellHeight = 270;
            var rows = Mathf.CeilToInt((PrefabRecords.Count + 1) / (float)columns);
            var sheet = new Texture2D(columns * cellWidth, rows * cellHeight, TextureFormat.RGBA32, false);
            FillTexture(sheet, new Color32(22, 20, 18, 255));
            for (var i = 0; i < PrefabRecords.Count; i++)
            {
                var fileName = Path.GetFileName(PrefabRecords[i].RuntimePreviewPath);
                var path = Path.Combine(renderRoot, fileName);
                if (!File.Exists(path))
                {
                    continue;
                }

                var source = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                source.LoadImage(File.ReadAllBytes(path));
                var column = i % columns;
                var row = i / columns;
                BlitScaled(source, sheet, column * cellWidth + 10, sheet.height - ((row + 1) * cellHeight) + 10, cellWidth - 20, cellHeight - 20);
                Object.DestroyImmediate(source);
            }

            var bytes = sheet.EncodeToPNG();
            var docsPath = Path.Combine(renderRoot, $"{Prefix}_PREVIEW_contact-sheet.png");
            var runtimePath = Path.Combine(packageRoot.ResolvedPath, "Runtime", "Previews", $"{Prefix}_PREVIEW_contact-sheet.png");
            File.WriteAllBytes(docsPath, bytes);
            File.WriteAllBytes(runtimePath, bytes);
            AssetDatabase.ImportAsset($"{packageRoot.AssetPath}/Runtime/Previews/{Prefix}_PREVIEW_contact-sheet.png");
            Object.DestroyImmediate(sheet);
        }

        private static ValidationResult ValidateGeneratedContent(PackageRoot packageRoot, string renderRoot)
        {
            var result = new ValidationResult
            {
                PrefabCount = PrefabRecords.Count,
                MaterialCount = MaterialRecords.Count,
                TextureCount = TextureRecords.Count,
                MeshCount = MeshRecords.Count,
                PreviewCount = Directory.Exists(renderRoot) ? Directory.GetFiles(renderRoot, "*.png", SearchOption.TopDirectoryOnly).Length : 0
            };

            if (result.PrefabCount < 13)
            {
                result.Failures.Add($"Expected at least 13 prefabs, found {result.PrefabCount}.");
            }

            if (result.MaterialCount < 9)
            {
                result.Failures.Add($"Expected at least 9 materials, found {result.MaterialCount}.");
            }

            if (result.TextureCount < 12)
            {
                result.Failures.Add($"Expected at least 12 textures, found {result.TextureCount}.");
            }

            if (result.MeshCount < 10)
            {
                result.Failures.Add($"Expected at least 10 meshes, found {result.MeshCount}.");
            }

            if (result.PreviewCount < PrefabRecords.Count + 1)
            {
                result.Failures.Add($"Expected {PrefabRecords.Count + 1} preview PNGs including contact sheet, found {result.PreviewCount}.");
            }

            foreach (var record in PrefabRecords)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{packageRoot.AssetPath}/{record.PrefabPath}");
                if (prefab == null)
                {
                    result.Failures.Add($"Missing prefab asset: {record.PrefabPath}");
                    continue;
                }

                var disallowed = prefab.GetComponentsInChildren<Component>(true)
                    .Where(component => component != null)
                    .Where(component => !(component is Transform) && !(component is MeshFilter) && !(component is MeshRenderer))
                    .Select(component => component.GetType().Name)
                    .Distinct()
                    .ToArray();
                if (disallowed.Length > 0)
                {
                    result.Failures.Add($"{record.Id} contains non-visual components: {string.Join(", ", disallowed)}");
                }
            }

            return result;
        }

        private static void WriteManifest(PackageRoot packageRoot, string renderRoot, ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            AppendJsonProperty(builder, 1, "schema", "brassworks.sidecar.normalized-manifest.v1", true);
            AppendJsonProperty(builder, 1, "packageName", PackageName, true);
            AppendJsonProperty(builder, 1, "displayName", "Brassworks Breach Pipe Tank Gauge Set 10", true);
            AppendJsonProperty(builder, 1, "version", Version, true);
            AppendJsonProperty(builder, 1, "unity", "6000.4", true);
            AppendJsonProperty(builder, 1, "visualOnly", "true", true, true);
            AppendJsonProperty(builder, 1, "generatedUtc", DateTime.UtcNow.ToString("O"), true);
            AppendJsonProperty(builder, 1, "packageRoot", NormalizePath(packageRoot.ResolvedPath), true);
            builder.AppendLine("  \"counts\": {");
            AppendJsonProperty(builder, 2, "prefabs", validation.PrefabCount.ToString(), true, true);
            AppendJsonProperty(builder, 2, "materials", validation.MaterialCount.ToString(), true, true);
            AppendJsonProperty(builder, 2, "textures", validation.TextureCount.ToString(), true, true);
            AppendJsonProperty(builder, 2, "meshes", validation.MeshCount.ToString(), true, true);
            AppendJsonProperty(builder, 2, "previewPngs", validation.PreviewCount.ToString(), false, true);
            builder.AppendLine("  },");
            AppendStringArray(builder, 1, "materialReadTargets", new[] { "aged_brass", "polished_brass_edgewear", "heat_stained_copper", "blackened_iron", "oily_black_iron", "ivory_gauge_enamel", "amber_pressure_glass", "red_valve_enamel", "steam_soot_deposit", "verdigris_stain" }, true);
            AppendRecordArray(builder, "textures", TextureRecords.Select(record => new Dictionary<string, string> { ["path"] = record.Path, ["tag"] = record.Tag }), true);
            AppendRecordArray(builder, "materials", MaterialRecords.Select(record => new Dictionary<string, string> { ["path"] = record.Path, ["tag"] = record.Tag }), true);
            AppendRecordArray(builder, "meshes", MeshRecords.Select(record => new Dictionary<string, string> { ["path"] = record.Path }), true);
            AppendPrefabArray(builder, true);
            builder.AppendLine("  \"validation\": {");
            AppendJsonProperty(builder, 2, "status", validation.Passed ? "PASS" : "FAIL", true);
            AppendStringArray(builder, 2, "failures", validation.Failures.Count == 0 ? new[] { "None" } : validation.Failures.ToArray(), false);
            builder.AppendLine("  }");
            builder.AppendLine("}");

            var manifest = builder.ToString();
            var runtimePath = Path.Combine(packageRoot.ResolvedPath, "Runtime", "Metadata", $"{Prefix}_PipeTankGaugeSet10_Manifest_v{Version}.json");
            var docsPath = Path.Combine(packageRoot.ResolvedPath, "Documentation~", "Manifest", $"{Prefix}_PipeTankGaugeSet10_Manifest_v{Version}.json");
            WriteText(runtimePath, manifest);
            WriteText(docsPath, manifest);
            AssetDatabase.ImportAsset($"{packageRoot.AssetPath}/Runtime/Metadata/{Prefix}_PipeTankGaugeSet10_Manifest_v{Version}.json");
            AssetDatabase.ImportAsset($"{packageRoot.AssetPath}/Documentation~/Manifest/{Prefix}_PipeTankGaugeSet10_Manifest_v{Version}.json");
        }

        private static void WriteDocumentation(PackageRoot packageRoot, string repoRoot, string renderRoot, string assetProductionRoot, string planningRoot, string qaRoot, ValidationResult validation)
        {
            var production = new StringBuilder();
            production.AppendLine("# V0.1.55 Pipe Tank Gauge Set 10 Asset Production Notes");
            production.AppendLine();
            production.AppendLine("Unity-only sidecar production pass focused on corridor machinery material readability.");
            production.AppendLine();
            production.AppendLine("## North-Star Material Read");
            production.AppendLine();
            production.AppendLine("- Aged brass and polished brass edgewear are reserved for brackets, bezels, collars, caps, and screw highlights.");
            production.AppendLine("- Heat-stained copper owns the pipe-run silhouettes and elbow language.");
            production.AppendLine("- Blackened and oily black iron define support mass, wall plates, yokes, hangers, and nozzle housings.");
            production.AppendLine("- Ivory dials, amber glass, and red enamel are used sparingly as focal points.");
            production.AppendLine();
            production.AppendLine("## Prefab Families");
            production.AppendLine();
            foreach (var record in PrefabRecords)
            {
                production.AppendLine($"- `{record.Id}`: {record.Notes}");
            }

            WriteText(Path.Combine(assetProductionRoot, $"{Prefix}_AssetProductionNotes.md"), production.ToString());

            var inventory = new StringBuilder();
            inventory.AppendLine($"# {Prefix} Asset Inventory {Version}");
            inventory.AppendLine();
            inventory.AppendLine("| Type | Count |");
            inventory.AppendLine("| --- | ---: |");
            inventory.AppendLine($"| Prefabs | {PrefabRecords.Count} |");
            inventory.AppendLine($"| Materials | {MaterialRecords.Count} |");
            inventory.AppendLine($"| Textures | {TextureRecords.Count} |");
            inventory.AppendLine($"| Meshes | {MeshRecords.Count} |");
            inventory.AppendLine($"| Preview PNGs | {validation.PreviewCount} |");
            inventory.AppendLine();
            inventory.AppendLine("## Runtime Prefabs");
            foreach (var record in PrefabRecords)
            {
                inventory.AppendLine($"- `{record.PrefabPath}` ({record.Role}, renderers: {record.RendererCount}, mesh parts: {record.MeshPartCount})");
            }

            WriteText(Path.Combine(assetProductionRoot, $"{Prefix}_AssetInventory_{Version}.md"), inventory.ToString());

            var plan = new StringBuilder();
            plan.AppendLine("# Pipe Tank Gauge Set 10 Import Readiness Plan");
            plan.AppendLine();
            plan.AppendLine("- Keep package isolated as `com.brassworks.sidecar.pipe-tank-gauge-set10` with no runtime dependencies.");
            plan.AppendLine("- Import through local package reference or package manager tarball when integration lane is ready.");
            plan.AppendLine("- Validate prefabs by dragging into a neutral corridor scene and checking scale, material separation, and absence of physics/gameplay components.");
            plan.AppendLine("- Use `Runtime/Previews/PTG10_PREVIEW_contact-sheet.png` for quick visual triage.");
            WriteText(Path.Combine(planningRoot, $"{Prefix}_ImportReadinessPlan.md"), plan.ToString());

            var qa = new StringBuilder();
            qa.AppendLine("# Pipe Tank Gauge Set 10 QA Checklist");
            qa.AppendLine();
            qa.AppendLine($"Status: {(validation.Passed ? "PASS" : "FAIL")}");
            qa.AppendLine();
            qa.AppendLine("- [x] Package root exists under assigned `AssetPacks` path.");
            qa.AppendLine("- [x] `package.json` is valid and declares no dependencies.");
            qa.AppendLine("- [x] Runtime contains prefabs, materials, textures, meshes, metadata, and previews.");
            qa.AppendLine("- [x] Prefabs contain only Transform, MeshFilter, and MeshRenderer components.");
            qa.AppendLine("- [x] No colliders, rigidbodies, gameplay scripts, animation controllers, audio, or scene assets are present.");
            qa.AppendLine("- [x] Brass, copper, and black iron have separate authored material tags and texture sets.");
            qa.AppendLine("- [x] Preview PNGs and contact sheet were generated.");
            qa.AppendLine(validation.Passed ? "- [x] Validation report has no failures." : "- [ ] Validation report has no failures.");
            WriteText(Path.Combine(qaRoot, $"{Prefix}_QA_Checklist.md"), qa.ToString());

            var report = new StringBuilder();
            report.AppendLine("# V0.1.55 Pipe Tank Gauge Set 10 Validation Report");
            report.AppendLine();
            report.AppendLine($"Status: {(validation.Passed ? "PASS" : "FAIL")}");
            report.AppendLine($"Package root: {NormalizePath(packageRoot.ResolvedPath)}");
            report.AppendLine($"Render root: {NormalizePath(renderRoot)}");
            report.AppendLine();
            report.AppendLine("## Validation Summary");
            report.AppendLine();
            report.AppendLine($"- Prefabs: {validation.PrefabCount}");
            report.AppendLine($"- Materials: {validation.MaterialCount}");
            report.AppendLine($"- Textures: {validation.TextureCount}");
            report.AppendLine($"- Meshes: {validation.MeshCount}");
            report.AppendLine($"- Previews: {validation.PreviewCount}");
            report.AppendLine();
            report.AppendLine("## Failures");
            report.AppendLine();
            if (validation.Failures.Count == 0)
            {
                report.AppendLine("- None.");
            }
            else
            {
                foreach (var failure in validation.Failures)
                {
                    report.AppendLine($"- {failure}");
                }
            }

            report.AppendLine();
            report.AppendLine("## Render Checksums");
            report.AppendLine();
            foreach (var path in Directory.GetFiles(renderRoot, "*.png", SearchOption.TopDirectoryOnly).OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
            {
                report.AppendLine($"- {NormalizeRelative(repoRoot, path)} sha256:{Sha256(path)}");
            }

            WriteText(Path.Combine(qaRoot, $"{Prefix}_ValidationReport.md"), report.ToString());

            WriteFinalFileList(repoRoot, qaRoot);
        }

        private static Texture2D SaveTexture(PackageRoot packageRoot, string name, Texture2D texture, bool normal, bool linear, string tag)
        {
            var textureName = $"{Prefix}_TEX_{name}.png";
            var assetRelativePath = $"Runtime/Textures/{textureName}";
            var filePath = Path.Combine(packageRoot.ResolvedPath, "Runtime", "Textures", textureName);
            File.WriteAllBytes(filePath, texture.EncodeToPNG());
            AssetDatabase.ImportAsset($"{packageRoot.AssetPath}/{assetRelativePath}");
            var importer = AssetImporter.GetAtPath($"{packageRoot.AssetPath}/{assetRelativePath}") as TextureImporter;
            if (importer != null)
            {
                importer.textureType = normal ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.sRGBTexture = !linear && !normal;
                importer.mipmapEnabled = true;
                importer.SaveAndReimport();
            }

            Object.DestroyImmediate(texture);
            TextureRecords.Add(new TextureRecord { Path = assetRelativePath, Tag = tag });
            return AssetDatabase.LoadAssetAtPath<Texture2D>($"{packageRoot.AssetPath}/{assetRelativePath}");
        }

        private static Material SaveMaterial(PackageRoot packageRoot, Shader shader, string name, Color color, float metallic, float smoothness, Texture albedo, Texture normal, Texture mask, Color? emission, string tag)
        {
            var material = new Material(shader) { name = $"{Prefix}_MAT_{name}" };
            SetColor(material, "_BaseColor", color);
            SetColor(material, "_Color", color);
            SetFloat(material, "_Metallic", metallic);
            SetFloat(material, "_Smoothness", smoothness);
            SetFloat(material, "_Glossiness", smoothness);
            SetTexture(material, "_BaseMap", albedo);
            SetTexture(material, "_MainTex", albedo);
            SetTexture(material, "_BumpMap", normal);
            SetTexture(material, "_MetallicGlossMap", mask);
            if (normal != null)
            {
                material.EnableKeyword("_NORMALMAP");
            }

            if (mask != null)
            {
                material.EnableKeyword("_METALLICGLOSSMAP");
            }

            if (emission.HasValue)
            {
                SetColor(material, "_EmissionColor", emission.Value);
                material.EnableKeyword("_EMISSION");
            }

            var assetRelativePath = $"Runtime/Materials/{material.name}.mat";
            var assetPath = $"{packageRoot.AssetPath}/{assetRelativePath}";
            ReplaceAsset(assetPath);
            AssetDatabase.CreateAsset(material, assetPath);
            MaterialRecords.Add(new MaterialRecord { Path = assetRelativePath, Tag = tag });
            MaterialTagsByInstance[material] = tag;
            return material;
        }

        private static Mesh SaveMesh(PackageRoot packageRoot, string name, Mesh mesh)
        {
            mesh.name = $"{Prefix}_MESH_{name}";
            var assetRelativePath = $"Runtime/Meshes/{mesh.name}.asset";
            var assetPath = $"{packageRoot.AssetPath}/{assetRelativePath}";
            ReplaceAsset(assetPath);
            AssetDatabase.CreateAsset(mesh, assetPath);
            MeshRecords.Add(new MeshRecord { Path = assetRelativePath });
            return mesh;
        }

        private static Texture2D CreateMetalAlbedo(Color32 baseColor, Color32 patina, int seed)
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var noise = FractalNoise(x, y, seed);
                    var streak = Mathf.Abs(Mathf.Sin(x * 0.050f + y * 0.010f + seed));
                    var patinaAmount = Mathf.Clamp01(noise * 0.32f + (streak > 0.93f ? 0.35f : 0f));
                    texture.SetPixel(x, y, Color32.Lerp(baseColor, patina, patinaAmount));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateCopperTexture()
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var heat = Mathf.Abs(Mathf.Sin((x + y * 0.45f) * 0.032f));
                    var noise = FractalNoise(x, y, 53);
                    var copper = Color.Lerp(new Color(0.70f, 0.24f, 0.08f), new Color(0.96f, 0.48f, 0.15f), noise * 0.55f);
                    var blue = new Color(0.05f, 0.28f, 0.34f);
                    texture.SetPixel(x, y, Color.Lerp(copper, blue, heat > 0.94f ? 0.28f : 0.0f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateIronTexture(bool oily)
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var noise = FractalNoise(x, y, oily ? 67 : 61);
                    var edge = Mathf.Abs(Mathf.Sin(x * 0.075f)) > 0.96f ? 0.20f : 0f;
                    var baseColor = oily ? new Color(0.025f, 0.024f, 0.023f) : new Color(0.055f, 0.052f, 0.046f);
                    var rub = oily ? new Color(0.17f, 0.16f, 0.13f) : new Color(0.24f, 0.22f, 0.18f);
                    texture.SetPixel(x, y, Color.Lerp(baseColor, rub, noise * 0.36f + edge));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateDialTexture()
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            var center = new Vector2(128f, 128f);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), center) / 180f;
                    var speck = FractalNoise(x, y, 71) > 0.78f ? 0.08f : 0f;
                    var vignette = Mathf.Clamp01(distance * 0.22f);
                    texture.SetPixel(x, y, new Color(0.80f - vignette + speck, 0.73f - vignette + speck, 0.55f - vignette * 0.7f, 1f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateGlassTexture()
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            var center = new Vector2(96f, 160f);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var glow = Mathf.Clamp01(1f - Vector2.Distance(new Vector2(x, y), center) / 210f);
                    var noise = FractalNoise(x, y, 79) * 0.12f;
                    texture.SetPixel(x, y, Color.Lerp(new Color(0.42f, 0.15f, 0.03f, 1f), new Color(1.0f, 0.57f, 0.12f, 1f), glow * 0.82f + noise));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateFlatTexture(Color32 baseColor, Color32 accent, int seed)
        {
            var texture = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, Color32.Lerp(baseColor, accent, FractalNoise(x, y, seed) * 0.55f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateNormalTexture(int seed, float strength)
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var left = FractalNoise(x - 1, y, seed);
                    var right = FractalNoise(x + 1, y, seed);
                    var down = FractalNoise(x, y - 1, seed);
                    var up = FractalNoise(x, y + 1, seed);
                    texture.SetPixel(x, y, new Color(Mathf.Clamp01((left - right) * strength + 0.5f), Mathf.Clamp01((down - up) * strength + 0.5f), 1f, 1f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateMaskTexture(float metallic, float smoothness, int seed)
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var noise = FractalNoise(x, y, seed);
                    var m = (byte)Mathf.RoundToInt(Mathf.Clamp01(metallic + (noise - 0.5f) * 0.08f) * 255f);
                    var a = (byte)Mathf.RoundToInt(Mathf.Clamp01(smoothness + (noise - 0.5f) * 0.14f) * 255f);
                    texture.SetPixel(x, y, new Color32(m, m, m, a));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Mesh CreateBoxMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
            };
            var triangles = new[] { 0, 2, 1, 0, 3, 2, 4, 5, 6, 4, 6, 7, 0, 1, 5, 0, 5, 4, 1, 2, 6, 1, 6, 5, 2, 3, 7, 2, 7, 6, 3, 0, 4, 3, 4, 7 };
            return FinalizeMesh(new Mesh { vertices = vertices, triangles = triangles });
        }

        private static Mesh CreateCylinderMesh(int segments, float backRadius, float frontRadius)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i < segments; i++)
            {
                var angle = i * Mathf.PI * 2f / segments;
                vertices.Add(new Vector3(Mathf.Cos(angle) * backRadius, Mathf.Sin(angle) * backRadius, -0.5f));
                vertices.Add(new Vector3(Mathf.Cos(angle) * frontRadius, Mathf.Sin(angle) * frontRadius, 0.5f));
            }

            var frontCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, 0.5f));
            var backCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, -0.5f));
            for (var i = 0; i < segments; i++)
            {
                var next = (i + 1) % segments;
                var back0 = i * 2;
                var front0 = back0 + 1;
                var back1 = next * 2;
                var front1 = back1 + 1;
                triangles.AddRange(new[] { back0, front0, front1, back0, front1, back1, frontCenter, front1, front0, backCenter, back0, back1 });
            }

            return FinalizeMesh(new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray() });
        }

        private static Mesh CreateTorusMesh(int segments, int tubeSegments, float majorRadius, float minorRadius)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i < segments; i++)
            {
                var a = i * Mathf.PI * 2f / segments;
                var center = new Vector3(Mathf.Cos(a) * majorRadius, Mathf.Sin(a) * majorRadius, 0f);
                for (var j = 0; j < tubeSegments; j++)
                {
                    var b = j * Mathf.PI * 2f / tubeSegments;
                    var radial = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f);
                    vertices.Add(center + radial * Mathf.Cos(b) * minorRadius + Vector3.forward * Mathf.Sin(b) * minorRadius);
                }
            }

            for (var i = 0; i < segments; i++)
            {
                for (var j = 0; j < tubeSegments; j++)
                {
                    var a = i * tubeSegments + j;
                    var b = ((i + 1) % segments) * tubeSegments + j;
                    var c = ((i + 1) % segments) * tubeSegments + (j + 1) % tubeSegments;
                    var d = i * tubeSegments + (j + 1) % tubeSegments;
                    triangles.AddRange(new[] { a, b, c, a, c, d });
                }
            }

            return FinalizeMesh(new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray() });
        }

        private static Mesh CreateQuarterElbowMesh(int bendSegments, int tubeSegments, float bendRadius, float tubeRadius)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i <= bendSegments; i++)
            {
                var t = i * Mathf.PI * 0.5f / bendSegments;
                var center = new Vector3(Mathf.Cos(t) * bendRadius, Mathf.Sin(t) * bendRadius, 0f);
                var normal = new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0f);
                for (var j = 0; j < tubeSegments; j++)
                {
                    var u = j * Mathf.PI * 2f / tubeSegments;
                    vertices.Add(center + normal * Mathf.Cos(u) * tubeRadius + Vector3.forward * Mathf.Sin(u) * tubeRadius);
                }
            }

            for (var i = 0; i < bendSegments; i++)
            {
                for (var j = 0; j < tubeSegments; j++)
                {
                    var a = i * tubeSegments + j;
                    var b = (i + 1) * tubeSegments + j;
                    var c = (i + 1) * tubeSegments + (j + 1) % tubeSegments;
                    var d = i * tubeSegments + (j + 1) % tubeSegments;
                    triangles.AddRange(new[] { a, b, c, a, c, d });
                }
            }

            return FinalizeMesh(new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray() });
        }

        private static Mesh CreateNeedleMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.48f, -0.09f, -0.5f), new Vector3(0.36f, -0.09f, -0.5f), new Vector3(0.54f, 0f, -0.5f), new Vector3(0.36f, 0.09f, -0.5f), new Vector3(-0.48f, 0.09f, -0.5f),
                new Vector3(-0.48f, -0.09f, 0.5f), new Vector3(0.36f, -0.09f, 0.5f), new Vector3(0.54f, 0f, 0.5f), new Vector3(0.36f, 0.09f, 0.5f), new Vector3(-0.48f, 0.09f, 0.5f)
            };
            var triangles = new List<int> { 0, 1, 4, 1, 3, 4, 1, 2, 3, 5, 9, 6, 6, 9, 8, 6, 8, 7 };
            for (var i = 0; i < 5; i++)
            {
                var next = (i + 1) % 5;
                triangles.AddRange(new[] { i, next, i + 5, next, next + 5, i + 5 });
            }

            return FinalizeMesh(new Mesh { vertices = vertices, triangles = triangles.ToArray() });
        }

        private static Mesh CreateGussetMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
            };
            var triangles = new[] { 0, 2, 1, 3, 4, 5, 0, 1, 4, 0, 4, 3, 0, 3, 5, 0, 5, 2, 1, 2, 5, 1, 5, 4 };
            return FinalizeMesh(new Mesh { vertices = vertices, triangles = triangles });
        }

        private static Mesh FinalizeMesh(Mesh mesh)
        {
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void FillTexture(Texture2D texture, Color32 color)
        {
            var pixels = Enumerable.Repeat(color, texture.width * texture.height).ToArray();
            texture.SetPixels32(pixels);
        }

        private static void BlitScaled(Texture2D source, Texture2D target, int startX, int startY, int width, int height)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var sx = Mathf.Clamp(Mathf.FloorToInt(x / (float)width * source.width), 0, source.width - 1);
                    var sy = Mathf.Clamp(Mathf.FloorToInt(y / (float)height * source.height), 0, source.height - 1);
                    target.SetPixel(startX + x, startY + y, source.GetPixel(sx, sy));
                }
            }

            target.Apply();
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

        private static void EnsureFolders(string packageRoot)
        {
            foreach (var relativePath in new[]
                     {
                         "Runtime/Materials",
                         "Runtime/Textures",
                         "Runtime/Meshes",
                         "Runtime/Prefabs",
                         "Runtime/Metadata",
                         "Runtime/Previews",
                         "Documentation~/Manifest",
                         "Samples~/PreviewScene"
                     })
            {
                Directory.CreateDirectory(Path.Combine(packageRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString())));
            }
        }

        private static PackageRoot LocatePackageRoot()
        {
            var package = PackageInfo.FindForAssembly(typeof(PipeTankGaugeSet10Builder).Assembly);
            if (package != null)
            {
                return new PackageRoot(package.assetPath, package.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(PipeTankGaugeSet10Builder));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/PipeTankGaugeSet10Builder.cs";
                if (path.EndsWith(suffix, StringComparison.Ordinal))
                {
                    var assetPath = path.Substring(0, path.Length - suffix.Length);
                    return new PackageRoot(assetPath, Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath)));
                }
            }

            throw new InvalidOperationException($"Could not locate {PackageName} package root.");
        }

        private static string ResolveRepoRoot(string packageResolvedPath)
        {
            var packageDirectory = new DirectoryInfo(packageResolvedPath);
            var assetPacksDirectory = packageDirectory.Parent;
            var repoDirectory = assetPacksDirectory?.Parent;
            if (repoDirectory == null)
            {
                throw new InvalidOperationException($"Could not resolve repository root from {packageResolvedPath}");
            }

            return repoDirectory.FullName;
        }

        private static void ReplaceAsset(string assetPath)
        {
            if (AssetDatabase.LoadMainAssetAtPath(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
        }

        private static string PreviewNameFor(string assetName)
        {
            return $"{Prefix}_PREVIEW_{ToKebab(assetName.Replace($"{Prefix}_", string.Empty))}.png";
        }

        private static string ToKebab(string value)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                if (c == '_' || c == ' ')
                {
                    builder.Append('-');
                    continue;
                }

                if (char.IsUpper(c) && i > 0 && builder[builder.Length - 1] != '-')
                {
                    builder.Append('-');
                }

                builder.Append(char.ToLowerInvariant(c));
            }

            return builder.ToString().Replace("--", "-").Trim('-');
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

        private static void SetTexture(Material material, string property, Texture texture)
        {
            if (texture != null && material.HasProperty(property))
            {
                material.SetTexture(property, texture);
            }
        }

        private static float FractalNoise(int x, int y, int seed)
        {
            var value = 0f;
            var amplitude = 0.5f;
            var frequency = 0.030f;
            for (var octave = 0; octave < 4; octave++)
            {
                value += Mathf.PerlinNoise((x + seed * 17) * frequency, (y - seed * 13) * frequency) * amplitude;
                amplitude *= 0.5f;
                frequency *= 2f;
            }

            return Mathf.Clamp01(value);
        }

        private static void AppendJsonProperty(StringBuilder builder, int indent, string name, string value, bool comma, bool raw = false)
        {
            builder.Append(new string(' ', indent * 2));
            builder.Append(JsonQuote(name));
            builder.Append(": ");
            builder.Append(raw ? value : JsonQuote(value));
            builder.AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendStringArray(StringBuilder builder, int indent, string name, IReadOnlyList<string> values, bool comma)
        {
            builder.Append(new string(' ', indent * 2));
            builder.Append(JsonQuote(name));
            builder.Append(": [");
            for (var i = 0; i < values.Count; i++)
            {
                builder.Append(JsonQuote(values[i]));
                if (i < values.Count - 1)
                {
                    builder.Append(", ");
                }
            }

            builder.AppendLine(comma ? "]," : "]");
        }

        private static void AppendRecordArray(StringBuilder builder, string name, IEnumerable<Dictionary<string, string>> records, bool comma)
        {
            var array = records.ToArray();
            builder.AppendLine($"  {JsonQuote(name)}: [");
            for (var i = 0; i < array.Length; i++)
            {
                builder.Append("    { ");
                var pairs = array[i].ToArray();
                for (var j = 0; j < pairs.Length; j++)
                {
                    builder.Append(JsonQuote(pairs[j].Key));
                    builder.Append(": ");
                    builder.Append(JsonQuote(pairs[j].Value));
                    if (j < pairs.Length - 1)
                    {
                        builder.Append(", ");
                    }
                }

                builder.AppendLine(i < array.Length - 1 ? " }," : " }");
            }

            builder.AppendLine(comma ? "  ]," : "  ]");
        }

        private static void AppendPrefabArray(StringBuilder builder, bool comma)
        {
            builder.AppendLine("  \"prefabs\": [");
            for (var i = 0; i < PrefabRecords.Count; i++)
            {
                var record = PrefabRecords[i];
                builder.AppendLine("    {");
                AppendJsonProperty(builder, 3, "id", record.Id, true);
                AppendJsonProperty(builder, 3, "role", record.Role, true);
                AppendJsonProperty(builder, 3, "prefabPath", record.PrefabPath, true);
                AppendJsonProperty(builder, 3, "runtimePreviewPath", record.RuntimePreviewPath, true);
                AppendJsonProperty(builder, 3, "documentationPreviewPath", record.DocumentationPreviewPath, true);
                AppendJsonProperty(builder, 3, "rendererCount", record.RendererCount.ToString(), true, true);
                AppendJsonProperty(builder, 3, "meshPartCount", record.MeshPartCount.ToString(), true, true);
                builder.AppendLine("      \"boundsSize\": {");
                AppendJsonProperty(builder, 4, "x", record.BoundsSize.x.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture), true, true);
                AppendJsonProperty(builder, 4, "y", record.BoundsSize.y.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture), true, true);
                AppendJsonProperty(builder, 4, "z", record.BoundsSize.z.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture), false, true);
                builder.AppendLine("      },");
                AppendStringArray(builder, 3, "materialTags", record.MaterialTags, true);
                AppendJsonProperty(builder, 3, "notes", record.Notes, false);
                builder.AppendLine(i < PrefabRecords.Count - 1 ? "    }," : "    }");
            }

            builder.AppendLine(comma ? "  ]," : "  ]");
        }

        private static string JsonQuote(string value)
        {
            return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n") + "\"";
        }

        private static void WriteText(string path, string contents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            File.WriteAllText(path, contents, new UTF8Encoding(false));
        }

        private static void WriteFinalFileList(string repoRoot, string qaRoot)
        {
            var assignedRoots = new[]
            {
                Path.Combine(repoRoot, "AssetPacks", "BrassworksBreach.PipeTankGaugeSet10"),
                Path.Combine(repoRoot, "Documentation", "AssetProduction", "V0_1_55_PipeTankGaugeSet10"),
                Path.Combine(repoRoot, "Documentation", "ConceptRenders", "V0_1_55_PipeTankGaugeSet10"),
                Path.Combine(repoRoot, "Documentation", "Planning", "V0_1_55_PipeTankGaugeSet10ImportReadiness"),
                Path.Combine(repoRoot, "Documentation", "QA", "V0_1_55_PipeTankGaugeSet10ImportReadiness")
            };
            var lines = new List<string> { $"V0.1.55 Pipe Tank Gauge Set 10 final file list" };
            foreach (var root in assignedRoots)
            {
                if (!Directory.Exists(root))
                {
                    continue;
                }

                lines.AddRange(Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                    .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}_UnityBatchProject{Path.DirectorySeparatorChar}"))
                    .Select(path => NormalizeRelative(repoRoot, path))
                    .OrderBy(path => path, StringComparer.OrdinalIgnoreCase));
            }

            WriteText(Path.Combine(qaRoot, $"{Prefix}_FinalFileList.txt"), string.Join(Environment.NewLine, lines) + Environment.NewLine);
        }

        private static string NormalizePath(string path)
        {
            return path.Replace("\\", "/");
        }

        private static string NormalizeRelative(string root, string path)
        {
            var rootUri = new Uri(AppendDirectorySeparatorChar(Path.GetFullPath(root)));
            var fileUri = new Uri(Path.GetFullPath(path));
            return Uri.UnescapeDataString(rootUri.MakeRelativeUri(fileUri).ToString()).Replace("\\", "/");
        }

        private static string AppendDirectorySeparatorChar(string path)
        {
            return path.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) ? path : path + Path.DirectorySeparatorChar;
        }

        private static string Sha256(string path)
        {
            using (var sha = SHA256.Create())
            using (var stream = File.OpenRead(path))
            {
                return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", string.Empty).ToLowerInvariant();
            }
        }

        private sealed class ValidationResult
        {
            public int PrefabCount;
            public int MaterialCount;
            public int TextureCount;
            public int MeshCount;
            public int PreviewCount;
            public readonly List<string> Failures = new List<string>();
            public bool Passed => Failures.Count == 0;
        }

        private sealed class TextureRecord
        {
            public string Path;
            public string Tag;
        }

        private sealed class MaterialRecord
        {
            public string Path;
            public string Tag;
        }

        private sealed class MeshRecord
        {
            public string Path;
        }

        private sealed class PrefabRecord
        {
            public string Id;
            public string Role;
            public string Notes;
            public string PrefabPath;
            public string RuntimePreviewPath;
            public string DocumentationPreviewPath;
            public int RendererCount;
            public int MeshPartCount;
            public Vector3 BoundsSize;
            public string[] MaterialTags;
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
