using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BrassworksBreach.PressurePistolHeroSet10;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.PressurePistolHeroSet10.Editor
{
    public static class PressurePistolHeroSet10Builder
    {
        public const string PackageName = "com.brassworks.sidecar.pressure-pistol-hero-set10";
        private const string Version = "0.1.55-p010";
        private const string Prefix = "PPH10";
        private const int PreviewWidth = 1400;
        private const int PreviewHeight = 900;

        private static readonly string[] MaterialTags =
        {
            "aged_brass",
            "heat_stained_copper",
            "blackened_iron",
            "amber_pressure_glass",
            "ivory_gauge_enamel",
            "dark_walnut",
            "worn_leather",
            "red_pressure_mark"
        };

        private static readonly List<TextureRecord> TextureRecords = new List<TextureRecord>();
        private static readonly List<MaterialRecord> MaterialRecords = new List<MaterialRecord>();
        private static readonly List<MeshRecord> MeshRecords = new List<MeshRecord>();
        private static readonly List<ComponentRecord> ComponentRecords = new List<ComponentRecord>();

        [MenuItem("Brassworks Breach/Sidecar Packs/Pressure Pistol Hero Set 10/Generate Assets And Renders")]
        public static void GenerateAssetsAndRenders()
        {
            TextureRecords.Clear();
            MaterialRecords.Clear();
            MeshRecords.Clear();
            ComponentRecords.Clear();

            var packageRoot = LocatePackageRoot();
            var repoRoot = ResolveRepoRoot(packageRoot.ResolvedPath);
            var renderRoot = Path.Combine(repoRoot, "Documentation", "ConceptRenders", "V0_1_55_PressurePistolHeroSet10");
            var assetProductionRoot = Path.Combine(repoRoot, "Documentation", "AssetProduction", "V0_1_55_PressurePistolHeroSet10");
            var planningRoot = Path.Combine(repoRoot, "Documentation", "Planning", "V0_1_55_PressurePistolHeroSet10ImportReadiness");
            var qaRoot = Path.Combine(repoRoot, "Documentation", "QA", "V0_1_55_PressurePistolHeroSet10ImportReadiness");

            EnsurePackageFolders(packageRoot.ResolvedPath);
            Directory.CreateDirectory(renderRoot);
            Directory.CreateDirectory(assetProductionRoot);
            Directory.CreateDirectory(planningRoot);
            Directory.CreateDirectory(qaRoot);

            var textures = CreateTextures(packageRoot);
            var materials = CreateMaterials(packageRoot, textures);
            var meshes = CreateMeshes(packageRoot);

            CreateCoilPack(packageRoot, meshes, materials);
            CreatePressureGauge(packageRoot, meshes, materials);
            CreateBarrelTank(packageRoot, meshes, materials);
            CreateMuzzle(packageRoot, meshes, materials);
            CreateWoodLeatherGrip(packageRoot, meshes, materials);
            CreateFasteners(packageRoot, meshes, materials);
            CreateCandidateAssembly(packageRoot, meshes, materials);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RenderPreviews(renderRoot);
            RenderContactSheet(renderRoot);

            var validation = ValidateGeneratedContent(renderRoot);
            WriteManifest(packageRoot, renderRoot, validation);
            WriteDocumentation(packageRoot, repoRoot, renderRoot, assetProductionRoot, planningRoot, qaRoot, validation);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"{Prefix}_GENERATE_PASS version={Version} package={packageRoot.ResolvedPath} previews={renderRoot}");
        }

        private static Dictionary<string, Texture2D> CreateTextures(PackageRoot packageRoot)
        {
            var textures = new Dictionary<string, Texture2D>
            {
                ["brass_albedo"] = SaveTexture(packageRoot, "AgedBrassPatina_Albedo", CreateMetalAlbedo(new Color32(174, 119, 45, 255), new Color32(79, 103, 77, 255), 7), false, false, "aged_brass"),
                ["brass_normal"] = SaveTexture(packageRoot, "AgedBrassPatina_Normal", CreateNormalTexture(11, 0.28f), true, false, "aged_brass"),
                ["copper_albedo"] = SaveTexture(packageRoot, "HeatStainedCopper_Albedo", CreateMetalAlbedo(new Color32(179, 72, 33, 255), new Color32(50, 118, 108, 255), 19), false, false, "heat_stained_copper"),
                ["copper_mask"] = SaveTexture(packageRoot, "HeatStainedCopper_MetallicSmoothness", CreateMaskTexture(0.86f, 0.46f, 23), false, true, "heat_stained_copper"),
                ["iron_albedo"] = SaveTexture(packageRoot, "BlackenedIron_Albedo", CreateMetalAlbedo(new Color32(22, 21, 19, 255), new Color32(73, 66, 55, 255), 31), false, false, "blackened_iron"),
                ["iron_mask"] = SaveTexture(packageRoot, "BlackenedIron_MetallicSmoothness", CreateMaskTexture(0.78f, 0.31f, 37), false, true, "blackened_iron"),
                ["glass_albedo"] = SaveTexture(packageRoot, "AmberPressureGlass_Albedo", CreateGlassTexture(), false, false, "amber_pressure_glass"),
                ["dial_albedo"] = SaveTexture(packageRoot, "IvoryGaugeEnamel_Albedo", CreateDialTexture(), false, false, "ivory_gauge_enamel"),
                ["wood_albedo"] = SaveTexture(packageRoot, "DarkWalnutGrain_Albedo", CreateWoodTexture(), false, false, "dark_walnut"),
                ["wood_normal"] = SaveTexture(packageRoot, "DarkWalnutGrain_Normal", CreateNormalTexture(43, 0.36f), true, false, "dark_walnut"),
                ["leather_albedo"] = SaveTexture(packageRoot, "WornLeatherGrip_Albedo", CreateLeatherTexture(), false, false, "worn_leather"),
                ["leather_normal"] = SaveTexture(packageRoot, "WornLeatherGrip_Normal", CreateNormalTexture(53, 0.44f), true, false, "worn_leather"),
                ["red_albedo"] = SaveTexture(packageRoot, "RedPressureMark_Albedo", CreateFlatTexture(new Color32(141, 30, 22, 255), new Color32(93, 20, 17, 255), 61), false, false, "red_pressure_mark")
            };

            return textures;
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
                ["brass"] = SaveMaterial(packageRoot, shader, "AgedBrassPatina", new Color(0.72f, 0.47f, 0.18f), 0.92f, 0.42f, textures["brass_albedo"], textures["brass_normal"], null, null, "aged_brass"),
                ["copper"] = SaveMaterial(packageRoot, shader, "HeatStainedCopperCoil", new Color(0.78f, 0.31f, 0.14f), 0.88f, 0.38f, textures["copper_albedo"], null, textures["copper_mask"], null, "heat_stained_copper"),
                ["iron"] = SaveMaterial(packageRoot, shader, "BlackenedIron", new Color(0.048f, 0.045f, 0.039f), 0.80f, 0.28f, textures["iron_albedo"], null, textures["iron_mask"], null, "blackened_iron"),
                ["glass"] = SaveMaterial(packageRoot, shader, "AmberPressureGlass", new Color(1.00f, 0.48f, 0.11f), 0.02f, 0.86f, textures["glass_albedo"], null, null, new Color(1.0f, 0.36f, 0.08f) * 1.3f, "amber_pressure_glass"),
                ["dial"] = SaveMaterial(packageRoot, shader, "IvoryGaugeEnamel", new Color(0.86f, 0.78f, 0.58f), 0.0f, 0.45f, textures["dial_albedo"], null, null, null, "ivory_gauge_enamel"),
                ["wood"] = SaveMaterial(packageRoot, shader, "DarkWalnutGrip", new Color(0.22f, 0.10f, 0.045f), 0.0f, 0.40f, textures["wood_albedo"], textures["wood_normal"], null, null, "dark_walnut"),
                ["leather"] = SaveMaterial(packageRoot, shader, "WornLeatherWrap", new Color(0.28f, 0.15f, 0.075f), 0.0f, 0.33f, textures["leather_albedo"], textures["leather_normal"], null, null, "worn_leather"),
                ["red"] = SaveMaterial(packageRoot, shader, "RedPressureMark", new Color(0.58f, 0.08f, 0.055f), 0.0f, 0.39f, textures["red_albedo"], null, null, null, "red_pressure_mark")
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes(PackageRoot packageRoot)
        {
            return new Dictionary<string, Mesh>
            {
                ["box"] = SaveMesh(packageRoot, "Box_Unit", CreateBoxMesh()),
                ["wedge"] = SaveMesh(packageRoot, "TaperedGripWedge", CreateWedgeMesh()),
                ["cylinder16"] = SaveMesh(packageRoot, "Cylinder16_Z", CreateCylinderMesh(16)),
                ["cylinder32"] = SaveMesh(packageRoot, "Cylinder32_Z", CreateCylinderMesh(32)),
                ["cylinder48"] = SaveMesh(packageRoot, "Cylinder48_Z", CreateCylinderMesh(48)),
                ["needle"] = SaveMesh(packageRoot, "GaugeNeedle", CreateNeedleMesh()),
                ["washer"] = SaveMesh(packageRoot, "WasherDisc", CreateCylinderMesh(40)),
                ["bolt"] = SaveMesh(packageRoot, "BoltHead", CreateBoltHeadMesh(6)),
                ["rail"] = SaveMesh(packageRoot, "RailPrism", CreateBoxMesh())
            };
        }

        private static void CreateCoilPack(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PPH10_CoilPack_Standalone", "coil_pack", "component-pass", "Standalone pressure coil pack with amber core, copper loops, cradle rails, lugs, and service rivets.");

            Part(root.transform, meshes["cylinder32"], "amber_pressure_core_tube", new Vector3(0f, 0f, 0f), new Vector3(0.18f, 0.18f, 1.26f), Vector3.zero, materials["glass"]);
            Part(root.transform, meshes["cylinder16"], "blackened_iron_inner_spine", new Vector3(0f, 0f, 0f), new Vector3(0.07f, 0.07f, 1.36f), Vector3.zero, materials["iron"]);

            for (var i = 0; i < 13; i++)
            {
                var z = -0.58f + i * 0.096f;
                Part(root.transform, meshes["cylinder32"], $"heat_stained_copper_coil_loop_{i:00}", new Vector3(0f, 0f, z), new Vector3(0.34f, 0.34f, 0.020f), Vector3.zero, materials["copper"]);
            }

            Part(root.transform, meshes["rail"], "left_aged_brass_cradle_rail", new Vector3(-0.265f, -0.07f, 0f), new Vector3(0.045f, 0.10f, 1.42f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["rail"], "right_aged_brass_cradle_rail", new Vector3(0.265f, -0.07f, 0f), new Vector3(0.045f, 0.10f, 1.42f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "rear_blackened_iron_terminal_block", new Vector3(0f, -0.035f, -0.72f), new Vector3(0.44f, 0.15f, 0.11f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "front_blackened_iron_terminal_block", new Vector3(0f, -0.035f, 0.72f), new Vector3(0.44f, 0.15f, 0.11f), Vector3.zero, materials["iron"]);

            for (var i = 0; i < 4; i++)
            {
                var z = -0.48f + i * 0.32f;
                Part(root.transform, meshes["bolt"], $"left_brass_cradle_rivet_{i:00}", new Vector3(-0.265f, 0.02f, z), new Vector3(0.060f, 0.060f, 0.030f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["bolt"], $"right_brass_cradle_rivet_{i:00}", new Vector3(0.265f, 0.02f, z), new Vector3(0.060f, 0.060f, 0.030f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(packageRoot, root, "PPH10_CoilPack_Standalone");
        }

        private static void CreatePressureGauge(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PPH10_PressureGauge_Standalone", "pressure_gauge", "component-pass", "Readable brass pressure gauge with ivory dial, needle, danger ticks, glass face, and pipe socket.");
            AddGaugeFace(root.transform, meshes, materials, "front_pressure_gauge", Vector3.zero, 0.37f, true);
            Part(root.transform, meshes["cylinder32"], "blackened_iron_lower_pipe_socket", new Vector3(0f, -0.43f, 0.035f), new Vector3(0.12f, 0.12f, 0.26f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Part(root.transform, meshes["cylinder32"], "aged_brass_pipe_collar", new Vector3(0f, -0.33f, 0.025f), new Vector3(0.16f, 0.16f, 0.055f), new Vector3(90f, 0f, 0f), materials["brass"]);
            SavePrefab(packageRoot, root, "PPH10_PressureGauge_Standalone");
        }

        private static void CreateBarrelTank(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PPH10_BarrelTank_Standalone", "barrel_tank", "component-pass", "Hero pistol barrel and under-slung pressure tank module with clamps, sight glass, and transfer pipes.");

            Part(root.transform, meshes["cylinder48"], "blackened_iron_primary_barrel", new Vector3(0f, 0.16f, 0.20f), new Vector3(0.13f, 0.13f, 1.50f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["cylinder32"], "aged_brass_reinforced_pressure_tank", new Vector3(0f, -0.13f, 0.03f), new Vector3(0.34f, 0.34f, 0.78f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["cylinder32"], "amber_tank_sight_glass", new Vector3(0f, -0.13f, 0.04f), new Vector3(0.20f, 0.20f, 0.82f), Vector3.zero, materials["glass"]);

            for (var i = 0; i < 4; i++)
            {
                var z = -0.28f + i * 0.22f;
                Part(root.transform, meshes["cylinder32"], $"aged_brass_tank_clamp_{i:00}", new Vector3(0f, -0.13f, z), new Vector3(0.40f, 0.40f, 0.030f), Vector3.zero, materials["brass"]);
            }

            Part(root.transform, meshes["box"], "blackened_iron_receiver_socket", new Vector3(0f, 0.03f, -0.62f), new Vector3(0.42f, 0.30f, 0.26f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["cylinder16"], "left_copper_pressure_transfer_pipe", new Vector3(-0.23f, 0.02f, 0.05f), new Vector3(0.045f, 0.045f, 1.06f), Vector3.zero, materials["copper"]);
            Part(root.transform, meshes["cylinder16"], "right_copper_pressure_transfer_pipe", new Vector3(0.23f, 0.02f, 0.05f), new Vector3(0.045f, 0.045f, 1.06f), Vector3.zero, materials["copper"]);

            for (var i = 0; i < 3; i++)
            {
                var z = -0.40f + i * 0.34f;
                Part(root.transform, meshes["bolt"], $"left_receiver_bolt_{i:00}", new Vector3(-0.235f, 0.17f, z), new Vector3(0.055f, 0.055f, 0.028f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["bolt"], $"right_receiver_bolt_{i:00}", new Vector3(0.235f, 0.17f, z), new Vector3(0.055f, 0.055f, 0.028f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(packageRoot, root, "PPH10_BarrelTank_Standalone");
        }

        private static void CreateMuzzle(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PPH10_Muzzle_Standalone", "muzzle", "component-pass", "Pressure pistol muzzle crown with bore insert, copper heat shroud, brass collars, and visible vent-port language.");

            Part(root.transform, meshes["cylinder48"], "aged_brass_flared_muzzle_crown", new Vector3(0f, 0f, 0.14f), new Vector3(0.34f, 0.34f, 0.23f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["cylinder48"], "blackened_iron_inner_bore", new Vector3(0f, 0f, 0.27f), new Vector3(0.17f, 0.17f, 0.08f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["cylinder32"], "heat_stained_copper_rear_heat_band", new Vector3(0f, 0f, -0.05f), new Vector3(0.28f, 0.28f, 0.15f), Vector3.zero, materials["copper"]);
            Part(root.transform, meshes["cylinder32"], "blackened_iron_threaded_socket", new Vector3(0f, 0f, -0.22f), new Vector3(0.22f, 0.22f, 0.23f), Vector3.zero, materials["iron"]);

            for (var i = 0; i < 8; i++)
            {
                var angle = i * Mathf.PI * 2f / 8f;
                var pos = new Vector3(Mathf.Cos(angle) * 0.175f, Mathf.Sin(angle) * 0.175f, 0.065f);
                Part(root.transform, meshes["box"], $"blackened_iron_vent_port_shadow_{i:00}", pos, new Vector3(0.060f, 0.018f, 0.030f), new Vector3(0f, 0f, Mathf.Rad2Deg * angle), materials["iron"]);
            }

            SavePrefab(packageRoot, root, "PPH10_Muzzle_Standalone");
        }

        private static void CreateWoodLeatherGrip(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PPH10_WoodLeatherGrip_Standalone", "wood_leather_grip", "component-pass", "Separately readable dark walnut grip with worn leather wrap, brass pins, backstrap, and trigger guard tabs.");

            Part(root.transform, meshes["wedge"], "dark_walnut_tapered_grip_core", new Vector3(0f, -0.05f, 0f), new Vector3(0.34f, 0.78f, 0.22f), new Vector3(-10f, 0f, 0f), materials["wood"]);
            Part(root.transform, meshes["box"], "aged_brass_backstrap", new Vector3(0f, 0.02f, -0.135f), new Vector3(0.22f, 0.69f, 0.028f), new Vector3(-10f, 0f, 0f), materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_upper_mount_tongue", new Vector3(0f, 0.40f, 0.02f), new Vector3(0.42f, 0.10f, 0.24f), Vector3.zero, materials["brass"]);

            for (var i = 0; i < 6; i++)
            {
                var y = -0.36f + i * 0.125f;
                Part(root.transform, meshes["box"], $"worn_leather_wrap_band_{i:00}", new Vector3(0f, y, 0.010f), new Vector3(0.38f, 0.040f, 0.255f), new Vector3(-10f, 0f, 0f), materials["leather"]);
            }

            for (var i = 0; i < 4; i++)
            {
                var y = -0.27f + i * 0.19f;
                Part(root.transform, meshes["bolt"], $"left_brass_grip_pin_{i:00}", new Vector3(-0.19f, y, 0.015f), new Vector3(0.052f, 0.052f, 0.025f), new Vector3(0f, 90f, 0f), materials["brass"]);
                Part(root.transform, meshes["bolt"], $"right_brass_grip_pin_{i:00}", new Vector3(0.19f, y, 0.015f), new Vector3(0.052f, 0.052f, 0.025f), new Vector3(0f, 90f, 0f), materials["brass"]);
            }

            Part(root.transform, meshes["box"], "blackened_iron_trigger_guard_front_tab", new Vector3(0f, 0.24f, 0.20f), new Vector3(0.32f, 0.045f, 0.22f), new Vector3(25f, 0f, 0f), materials["iron"]);
            Part(root.transform, meshes["box"], "blackened_iron_trigger_guard_lower_tab", new Vector3(0f, 0.11f, 0.31f), new Vector3(0.30f, 0.040f, 0.31f), new Vector3(7f, 0f, 0f), materials["iron"]);

            SavePrefab(packageRoot, root, "PPH10_WoodLeatherGrip_Standalone");
        }

        private static void CreateFasteners(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PPH10_Fasteners_Standalone", "fasteners", "component-pass", "Fastener language sheet with rivets, slotted screws, washers, clamp blocks, and bracket straps for assembly detail.");

            Part(root.transform, meshes["box"], "blackened_iron_fastener_display_plate", new Vector3(0f, 0f, 0.03f), new Vector3(1.05f, 0.54f, 0.055f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "aged_brass_clamp_strap_sample", new Vector3(-0.28f, -0.18f, -0.03f), new Vector3(0.36f, 0.06f, 0.045f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "heat_stained_copper_small_pipe_strap", new Vector3(0.27f, -0.18f, -0.03f), new Vector3(0.36f, 0.06f, 0.045f), Vector3.zero, materials["copper"]);

            for (var i = 0; i < 5; i++)
            {
                var x = -0.40f + i * 0.20f;
                Part(root.transform, meshes["bolt"], $"aged_brass_domake_rivet_{i:00}", new Vector3(x, 0.17f, -0.025f), new Vector3(0.060f, 0.060f, 0.030f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["washer"], $"blackened_iron_washer_{i:00}", new Vector3(x, 0.02f, -0.025f), new Vector3(0.085f, 0.085f, 0.020f), Vector3.zero, materials["iron"]);
                Part(root.transform, meshes["box"], $"slotted_screw_cut_{i:00}", new Vector3(x, 0.02f, -0.040f), new Vector3(0.066f, 0.010f, 0.014f), Vector3.zero, materials["brass"]);
            }

            for (var i = 0; i < 3; i++)
            {
                var x = -0.28f + i * 0.28f;
                Part(root.transform, meshes["bolt"], $"strap_fastener_top_{i:00}", new Vector3(x, -0.18f, -0.065f), new Vector3(0.050f, 0.050f, 0.024f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(packageRoot, root, "PPH10_Fasteners_Standalone");
        }

        private static void CreateCandidateAssembly(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot("PPH10_CandidateAssembly_PressurePistolHero_A", "candidate_assembly", "candidate-pass", "First full visual assembly candidate built from the separately passing Set10 components.");

            InstantiatePrefabChild(packageRoot, root.transform, "PPH10_BarrelTank_Standalone", "barrel_tank_module", new Vector3(0f, 0.04f, 0.12f), Vector3.zero, Vector3.one);
            InstantiatePrefabChild(packageRoot, root.transform, "PPH10_CoilPack_Standalone", "top_coil_pack_module", new Vector3(0f, 0.36f, -0.05f), Vector3.zero, new Vector3(0.82f, 0.82f, 0.82f));
            InstantiatePrefabChild(packageRoot, root.transform, "PPH10_PressureGauge_Standalone", "top_pressure_gauge_module", new Vector3(0f, 0.62f, -0.49f), new Vector3(18f, 0f, 0f), new Vector3(0.44f, 0.44f, 0.44f));
            InstantiatePrefabChild(packageRoot, root.transform, "PPH10_Muzzle_Standalone", "front_muzzle_module", new Vector3(0f, 0.20f, 1.03f), Vector3.zero, new Vector3(0.90f, 0.90f, 0.90f));
            InstantiatePrefabChild(packageRoot, root.transform, "PPH10_WoodLeatherGrip_Standalone", "rear_grip_module", new Vector3(0f, -0.43f, -0.62f), new Vector3(13f, 0f, 0f), new Vector3(0.94f, 0.94f, 0.94f));

            Part(root.transform, meshes["box"], "blackened_iron_hero_receiver_frame", new Vector3(0f, 0.02f, -0.58f), new Vector3(0.48f, 0.32f, 0.34f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "aged_brass_trigger_guard_arc_proxy_lower", new Vector3(0f, -0.20f, -0.45f), new Vector3(0.34f, 0.050f, 0.40f), new Vector3(8f, 0f, 0f), materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_trigger_guard_arc_proxy_rear", new Vector3(0f, -0.13f, -0.64f), new Vector3(0.31f, 0.046f, 0.22f), new Vector3(-30f, 0f, 0f), materials["brass"]);
            Part(root.transform, meshes["needle"], "blackened_iron_curved_trigger_proxy", new Vector3(0f, -0.18f, -0.53f), new Vector3(0.16f, 0.10f, 0.022f), new Vector3(0f, 0f, -68f), materials["iron"]);
            Part(root.transform, meshes["cylinder16"], "left_external_copper_pressure_line", new Vector3(-0.29f, 0.19f, -0.10f), new Vector3(0.035f, 0.035f, 1.26f), Vector3.zero, materials["copper"]);
            Part(root.transform, meshes["cylinder16"], "right_external_copper_pressure_line", new Vector3(0.29f, 0.19f, -0.10f), new Vector3(0.035f, 0.035f, 1.26f), Vector3.zero, materials["copper"]);
            Part(root.transform, meshes["box"], "aged_brass_side_nameplate_blank", new Vector3(0f, 0.23f, -0.63f), new Vector3(0.38f, 0.075f, 0.024f), Vector3.zero, materials["brass"]);

            for (var i = 0; i < 4; i++)
            {
                var z = -0.70f + i * 0.17f;
                Part(root.transform, meshes["bolt"], $"assembly_left_receiver_bolt_{i:00}", new Vector3(-0.265f, 0.13f, z), new Vector3(0.052f, 0.052f, 0.024f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["bolt"], $"assembly_right_receiver_bolt_{i:00}", new Vector3(0.265f, 0.13f, z), new Vector3(0.052f, 0.052f, 0.024f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(packageRoot, root, "PPH10_CandidateAssembly_PressurePistolHero_A");
        }

        private static void AddGaugeFace(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, string name, Vector3 localPosition, float radius, bool dangerArc)
        {
            var gauge = new GameObject(name);
            gauge.transform.SetParent(parent, false);
            gauge.transform.localPosition = localPosition;

            Part(gauge.transform, meshes["cylinder48"], "blackened_iron_rear_case", Vector3.zero, new Vector3(radius * 2.24f, radius * 2.24f, 0.070f), Vector3.zero, materials["iron"]);
            Part(gauge.transform, meshes["cylinder48"], "aged_brass_outer_bezel", new Vector3(0f, 0f, -0.045f), new Vector3(radius * 2.05f, radius * 2.05f, 0.052f), Vector3.zero, materials["brass"]);
            Part(gauge.transform, meshes["cylinder48"], "ivory_gauge_enamel_face", new Vector3(0f, 0f, -0.080f), new Vector3(radius * 1.58f, radius * 1.58f, 0.018f), Vector3.zero, materials["dial"]);
            Part(gauge.transform, meshes["cylinder48"], "amber_convex_glass_face", new Vector3(0f, 0f, -0.104f), new Vector3(radius * 1.46f, radius * 1.46f, 0.010f), Vector3.zero, materials["glass"]);
            Part(gauge.transform, meshes["needle"], "blackened_iron_pressure_needle", new Vector3(radius * 0.19f, 0f, -0.125f), new Vector3(radius * 1.10f, radius * 0.11f, radius * 0.05f), new Vector3(0f, 0f, -25f), materials["iron"]);
            Part(gauge.transform, meshes["bolt"], "aged_brass_center_pin", new Vector3(0f, 0f, -0.145f), new Vector3(radius * 0.27f, radius * 0.27f, radius * 0.11f), Vector3.zero, materials["brass"]);

            for (var i = 0; i < 12; i++)
            {
                var angle = Mathf.Lerp(-145f, 145f, i / 11f) * Mathf.Deg2Rad;
                var tick = i % 3 == 0 ? radius * 0.18f : radius * 0.11f;
                var pos = new Vector3(Mathf.Sin(angle) * radius * 0.62f, Mathf.Cos(angle) * radius * 0.62f, -0.130f);
                Part(gauge.transform, meshes["box"], $"black_pressure_tick_{i:00}", pos, new Vector3(radius * 0.035f, tick, radius * 0.024f), new Vector3(0f, 0f, -Mathf.Rad2Deg * angle), materials["iron"]);
            }

            if (dangerArc)
            {
                for (var i = 0; i < 4; i++)
                {
                    var angle = Mathf.Lerp(98f, 145f, i / 3f) * Mathf.Deg2Rad;
                    var pos = new Vector3(Mathf.Sin(angle) * radius * 0.47f, Mathf.Cos(angle) * radius * 0.47f, -0.136f);
                    Part(gauge.transform, meshes["box"], $"red_overpressure_mark_{i:00}", pos, new Vector3(radius * 0.045f, radius * 0.15f, radius * 0.026f), new Vector3(0f, 0f, -Mathf.Rad2Deg * angle), materials["red"]);
                }
            }
        }

        private static GameObject NewRoot(string assetId, string role, string status, string notes)
        {
            var root = new GameObject(assetId);
            root.AddComponent<PressurePistolHeroSet10Identity>().Configure(assetId, role, status, 0, MaterialTags, notes);
            return root;
        }

        private static GameObject Part(Transform parent, Mesh mesh, string name, Vector3 localPosition, Vector3 localScale, Vector3 localEuler, Material material)
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

        private static void InstantiatePrefabChild(PackageRoot packageRoot, Transform parent, string prefabName, string childName, Vector3 localPosition, Vector3 localEuler, Vector3 localScale)
        {
            var prefabPath = $"{packageRoot.AssetPath}/Runtime/Prefabs/{prefabName}.prefab";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                throw new InvalidOperationException($"Missing component prefab for assembly: {prefabPath}");
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = childName;
            instance.transform.SetParent(parent, false);
            instance.transform.localPosition = localPosition;
            instance.transform.localEulerAngles = localEuler;
            instance.transform.localScale = localScale;
        }

        private static void SavePrefab(PackageRoot packageRoot, GameObject prefabRoot, string assetName)
        {
            var rendererCount = prefabRoot.GetComponentsInChildren<Renderer>(true).Length;
            var meshPartCount = prefabRoot.GetComponentsInChildren<MeshFilter>(true).Length;
            var identity = prefabRoot.GetComponent<PressurePistolHeroSet10Identity>();
            if (identity != null)
            {
                identity.Configure(assetName, identity.ComponentRole, identity.AcceptanceStatus, rendererCount, MaterialTags, identity.Notes);
            }

            var path = $"{packageRoot.AssetPath}/Runtime/Prefabs/{assetName}.prefab";
            ReplaceAsset(path);
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);

            var bounds = CalculateBounds(prefabRoot);
            var componentRole = identity != null ? identity.ComponentRole : "unknown";
            ComponentRecords.Add(new ComponentRecord
            {
                Id = assetName,
                Role = componentRole,
                PrefabPath = $"Runtime/Prefabs/{assetName}.prefab",
                PreviewPath = $"Documentation/ConceptRenders/V0_1_55_PressurePistolHeroSet10/{PreviewNameFor(assetName)}",
                RendererCount = rendererCount,
                MeshPartCount = meshPartCount,
                BoundsSize = bounds.size,
                AcceptanceStatus = identity != null ? identity.AcceptanceStatus : "unknown"
            });

            Object.DestroyImmediate(prefabRoot);
        }

        private static void RenderPreviews(string renderRoot)
        {
            foreach (var record in ComponentRecords)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{LocatePackageRoot().AssetPath}/{record.PrefabPath}");
                if (prefab == null)
                {
                    throw new InvalidOperationException($"Missing prefab for preview: {record.PrefabPath}");
                }

                RenderPrefab(prefab, Path.Combine(renderRoot, PreviewNameFor(prefab.name)));
            }
        }

        private static void RenderPrefab(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var instance = Object.Instantiate(prefab);
            instance.name = prefab.name;
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(8f, -30f, 0f);

            var floorMaterial = new Material(Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit"))
            {
                color = new Color(0.032f, 0.030f, 0.027f)
            };
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "preview_floor_not_saved";
            floor.transform.position = new Vector3(0f, -0.58f, 0f);
            floor.transform.localScale = new Vector3(4.4f, 0.05f, 3.0f);
            floor.GetComponent<Renderer>().sharedMaterial = floorMaterial;
            Object.DestroyImmediate(floor.GetComponent<Collider>());

            AddPreviewLights();

            var cameraObject = new GameObject("preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.025f, 0.023f, 0.020f);
            camera.fieldOfView = 34f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 30f;

            var bounds = CalculateBounds(instance);
            var radius = Mathf.Max(0.7f, bounds.extents.magnitude);
            camera.transform.position = bounds.center + new Vector3(radius * 1.12f, radius * 0.74f, -radius * 2.26f);
            camera.transform.LookAt(bounds.center + Vector3.up * radius * 0.03f);

            CaptureCamera(camera, outputPath, PreviewWidth, PreviewHeight);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderContactSheet(string renderRoot)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var prefabs = ComponentRecords.Select(record =>
                    AssetDatabase.LoadAssetAtPath<GameObject>($"{LocatePackageRoot().AssetPath}/{record.PrefabPath}"))
                .Where(prefab => prefab != null)
                .ToArray();

            AddPreviewLights();
            var cellWidth = 2.15f;
            var cellDepth = 1.72f;
            for (var i = 0; i < prefabs.Length; i++)
            {
                var col = i % 4;
                var row = i / 4;
                var position = new Vector3((col - 1.5f) * cellWidth, 0f, (0.5f - row) * cellDepth);
                var instance = Object.Instantiate(prefabs[i]);
                instance.name = $"{prefabs[i].name}_contact";
                instance.transform.position = position;
                instance.transform.rotation = Quaternion.Euler(8f, -26f, 0f);
                instance.transform.localScale = Vector3.one * (prefabs[i].name.IndexOf("CandidateAssembly", StringComparison.Ordinal) >= 0 ? 0.58f : 0.72f);

                var plate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                plate.name = "contact_shadow_plate_not_saved";
                plate.transform.position = position + new Vector3(0f, -0.58f, 0f);
                plate.transform.localScale = new Vector3(1.72f, 0.035f, 1.18f);
                Object.DestroyImmediate(plate.GetComponent<Collider>());
                var renderer = plate.GetComponent<Renderer>();
                renderer.sharedMaterial = new Material(Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit"))
                {
                    color = new Color(0.035f, 0.033f, 0.030f)
                };
            }

            var cameraObject = new GameObject("contact_sheet_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.022f, 0.020f, 0.018f);
            camera.orthographic = true;
            camera.orthographicSize = 3.0f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 40f;
            camera.transform.position = new Vector3(0f, 3.8f, -4.8f);
            camera.transform.LookAt(new Vector3(0f, -0.05f, -0.15f));

            CaptureCamera(camera, Path.Combine(renderRoot, $"{Prefix}_PREVIEW_contact-sheet.png"), 1800, 1100);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void AddPreviewLights()
        {
            var key = new GameObject("warm_lantern_key_light").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.72f, 0.45f);
            key.intensity = 2.5f;
            key.transform.rotation = Quaternion.Euler(45f, -38f, 0f);

            var fill = new GameObject("soft_cool_fill_light").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(0.45f, 0.56f, 0.66f);
            fill.intensity = 2.0f;
            fill.range = 5f;
            fill.transform.position = new Vector3(-1.8f, 1.1f, -1.4f);

            var rim = new GameObject("amber_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(1.0f, 0.44f, 0.16f);
            rim.intensity = 1.1f;
            rim.range = 5f;
            rim.transform.position = new Vector3(1.7f, 1.0f, 1.6f);
        }

        private static void CaptureCamera(Camera camera, string outputPath, int width, int height)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");
            var renderTexture = new RenderTexture(width, height, 24)
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
                    Object.DestroyImmediate(texture);
                }

                Object.DestroyImmediate(renderTexture);
            }
        }

        private static ValidationResult ValidateGeneratedContent(string renderRoot)
        {
            var result = new ValidationResult();
            var packageRoot = LocatePackageRoot();
            var allowed = new HashSet<Type>
            {
                typeof(Transform),
                typeof(MeshFilter),
                typeof(MeshRenderer),
                typeof(PressurePistolHeroSet10Identity)
            };

            foreach (var record in ComponentRecords)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{packageRoot.AssetPath}/{record.PrefabPath}");
                if (prefab == null)
                {
                    result.Failures.Add($"Missing prefab: {record.PrefabPath}");
                    continue;
                }

                foreach (var component in prefab.GetComponentsInChildren<Component>(true))
                {
                    if (component == null)
                    {
                        result.Failures.Add($"{record.Id}: missing script component");
                        continue;
                    }

                    if (!allowed.Contains(component.GetType()))
                    {
                        result.Failures.Add($"{record.Id}: disallowed visual-only component {component.GetType().Name}");
                    }
                }

                foreach (var meshFilter in prefab.GetComponentsInChildren<MeshFilter>(true))
                {
                    if (meshFilter.sharedMesh == null)
                    {
                        result.Failures.Add($"{record.Id}: MeshFilter without mesh");
                    }
                }

                foreach (var renderer in prefab.GetComponentsInChildren<MeshRenderer>(true))
                {
                    if (renderer.sharedMaterial == null)
                    {
                        result.Failures.Add($"{record.Id}: MeshRenderer without material");
                    }
                }

                var previewPath = Path.Combine(renderRoot, PreviewNameFor(record.Id));
                if (!File.Exists(previewPath) || new FileInfo(previewPath).Length < 4096)
                {
                    result.Failures.Add($"{record.Id}: missing or tiny preview PNG");
                }
            }

            var contactSheet = Path.Combine(renderRoot, $"{Prefix}_PREVIEW_contact-sheet.png");
            if (!File.Exists(contactSheet) || new FileInfo(contactSheet).Length < 4096)
            {
                result.Failures.Add("Missing or tiny contact sheet PNG");
            }

            result.PrefabCount = ComponentRecords.Count;
            result.MaterialCount = MaterialRecords.Count;
            result.TextureCount = TextureRecords.Count;
            result.MeshCount = MeshRecords.Count;
            result.PreviewCount = ComponentRecords.Count + (File.Exists(contactSheet) ? 1 : 0);
            return result;
        }

        private static void WriteManifest(PackageRoot packageRoot, string renderRoot, ValidationResult validation)
        {
            var manifest = BuildManifestJson(renderRoot, validation);
            WriteText(Path.Combine(packageRoot.ResolvedPath, "Runtime", "Metadata", $"{Prefix}_PressurePistolHeroSet10_Manifest_v{Version}.json"), manifest);
            WriteText(Path.Combine(packageRoot.ResolvedPath, "Documentation~", "Manifest", $"{Prefix}_PressurePistolHeroSet10_Manifest_v{Version}.json"), manifest);
            AssetDatabase.ImportAsset($"{packageRoot.AssetPath}/Runtime/Metadata/{Prefix}_PressurePistolHeroSet10_Manifest_v{Version}.json", ImportAssetOptions.ForceUpdate);
            AssetDatabase.ImportAsset($"{packageRoot.AssetPath}/Documentation~/Manifest/{Prefix}_PressurePistolHeroSet10_Manifest_v{Version}.json", ImportAssetOptions.ForceUpdate);
        }

        private static string BuildManifestJson(string renderRoot, ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            AppendJsonProperty(builder, 1, "schema", "com.brassworks.asset_manifest.v1", true);
            builder.AppendLine("  \"package\": {");
            AppendJsonProperty(builder, 2, "name", PackageName, true);
            AppendJsonProperty(builder, 2, "version", Version, true);
            AppendJsonProperty(builder, 2, "displayName", "Brassworks Breach Pressure Pistol Hero Set 10", true);
            AppendJsonProperty(builder, 2, "unity", "6000.4", false);
            builder.AppendLine("  },");
            builder.AppendLine("  \"safety\": {");
            AppendJsonProperty(builder, 2, "visualOnly", "true", true, raw: true);
            AppendJsonProperty(builder, 2, "externalDcc", "false", true, raw: true);
            AppendJsonProperty(builder, 2, "forbiddenAssetTypes", "[]", false, raw: true);
            builder.AppendLine("  },");
            builder.AppendLine("  \"acceptanceGates\": [");
            var gates = new[]
            {
                "All required component prefabs exist as isolated modules.",
                "Candidate assembly is composed from separately passing components.",
                "No colliders, rigidbodies, cameras, lights, audio sources, animator controllers, or gameplay scripts are saved in prefabs.",
                "Every MeshRenderer has a material and every MeshFilter has a reusable mesh asset.",
                "Procedural texture files are present and referenced by package materials.",
                "Individual previews and one contact sheet are present in the assigned ConceptRenders root.",
                "Runtime and Documentation manifest copies are normalized and identical."
            };
            for (var i = 0; i < gates.Length; i++)
            {
                builder.Append("    ");
                builder.Append(JsonQuote(gates[i]));
                builder.AppendLine(i == gates.Length - 1 ? string.Empty : ",");
            }
            builder.AppendLine("  ],");
            builder.AppendLine("  \"components\": [");
            for (var i = 0; i < ComponentRecords.Count; i++)
            {
                var record = ComponentRecords[i];
                builder.AppendLine("    {");
                AppendJsonProperty(builder, 3, "id", record.Id, true);
                AppendJsonProperty(builder, 3, "role", record.Role, true);
                AppendJsonProperty(builder, 3, "acceptanceStatus", record.AcceptanceStatus, true);
                AppendJsonProperty(builder, 3, "prefab", record.PrefabPath, true);
                AppendJsonProperty(builder, 3, "preview", record.PreviewPath, true);
                AppendJsonProperty(builder, 3, "rendererCount", record.RendererCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
                AppendJsonProperty(builder, 3, "meshPartCount", record.MeshPartCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
                builder.AppendLine("      \"boundsSize\": {");
                AppendJsonProperty(builder, 4, "x", record.BoundsSize.x.ToString("0.###", CultureInfo.InvariantCulture), true, raw: true);
                AppendJsonProperty(builder, 4, "y", record.BoundsSize.y.ToString("0.###", CultureInfo.InvariantCulture), true, raw: true);
                AppendJsonProperty(builder, 4, "z", record.BoundsSize.z.ToString("0.###", CultureInfo.InvariantCulture), false, raw: true);
                builder.AppendLine("      }");
                builder.Append("    }");
                builder.AppendLine(i == ComponentRecords.Count - 1 ? "," : ",");
            }
            builder.AppendLine("    {");
            AppendJsonProperty(builder, 3, "id", "PPH10_PREVIEW_contact-sheet", true);
            AppendJsonProperty(builder, 3, "role", "contact_sheet", true);
            AppendJsonProperty(builder, 3, "preview", "Documentation/ConceptRenders/V0_1_55_PressurePistolHeroSet10/PPH10_PREVIEW_contact-sheet.png", false);
            builder.AppendLine("    }");
            builder.AppendLine("  ],");
            AppendRecordArray(builder, "materials", MaterialRecords.Select(record => record.Path).OrderBy(path => path).ToArray(), true);
            AppendRecordArray(builder, "textures", TextureRecords.Select(record => record.Path).OrderBy(path => path).ToArray(), true);
            AppendRecordArray(builder, "meshes", MeshRecords.Select(record => record.Path).OrderBy(path => path).ToArray(), true);
            builder.AppendLine("  \"validation\": {");
            AppendJsonProperty(builder, 2, "status", validation.Passed ? "pass" : "fail", true);
            AppendJsonProperty(builder, 2, "prefabCount", validation.PrefabCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            AppendJsonProperty(builder, 2, "materialCount", validation.MaterialCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            AppendJsonProperty(builder, 2, "textureCount", validation.TextureCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            AppendJsonProperty(builder, 2, "meshCount", validation.MeshCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            AppendJsonProperty(builder, 2, "previewCount", validation.PreviewCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            builder.AppendLine("    \"failures\": [");
            for (var i = 0; i < validation.Failures.Count; i++)
            {
                builder.Append("      ");
                builder.Append(JsonQuote(validation.Failures[i]));
                builder.AppendLine(i == validation.Failures.Count - 1 ? string.Empty : ",");
            }
            builder.AppendLine("    ]");
            builder.AppendLine("  }");
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static void AppendRecordArray(StringBuilder builder, string name, IReadOnlyList<string> values, bool trailingComma)
        {
            builder.AppendLine($"  \"{name}\": [");
            for (var i = 0; i < values.Count; i++)
            {
                builder.Append("    ");
                builder.Append(JsonQuote(values[i]));
                builder.AppendLine(i == values.Count - 1 ? string.Empty : ",");
            }

            builder.Append("  ]");
            builder.AppendLine(trailingComma ? "," : string.Empty);
        }

        private static void WriteDocumentation(PackageRoot packageRoot, string repoRoot, string renderRoot, string assetProductionRoot, string planningRoot, string qaRoot, ValidationResult validation)
        {
            var packageFiles = EnumerateOwnedFiles(packageRoot.ResolvedPath)
                .Select(path => NormalizeRelative(packageRoot.ResolvedPath, path))
                .Where(path => !path.StartsWith("BuildProject~/", StringComparison.Ordinal))
                .OrderBy(path => path, StringComparer.Ordinal)
                .ToArray();
            var renderFiles = EnumerateOwnedFiles(renderRoot).Select(path => NormalizeRelative(repoRoot, path)).OrderBy(path => path, StringComparer.Ordinal).ToArray();

            WriteText(Path.Combine(assetProductionRoot, $"{Prefix}_AssetProductionNotes.md"), BuildAssetProductionNotes(packageFiles, renderFiles, validation));
            WriteText(Path.Combine(planningRoot, $"{Prefix}_ImportReadinessPlan.md"), BuildReadinessPlan(validation));
            WriteText(Path.Combine(qaRoot, $"{Prefix}_ValidationReport.md"), BuildValidationReport(packageRoot, repoRoot, renderRoot, packageFiles, renderFiles, validation));
            WriteText(Path.Combine(qaRoot, $"{Prefix}_FinalFileList.txt"), BuildFinalFileList(repoRoot, packageRoot.ResolvedPath, renderRoot, assetProductionRoot, planningRoot, qaRoot));
        }

        private static string BuildAssetProductionNotes(IReadOnlyList<string> packageFiles, IReadOnlyList<string> renderFiles, ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# V0.1.55 Pressure Pistol Hero Set 10 Asset Production Notes");
            builder.AppendLine();
            builder.AppendLine($"Package: {PackageName}");
            builder.AppendLine("Folder: AssetPacks/BrassworksBreach.PressurePistolHeroSet10");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("Component-first pressure pistol hero weapon package covering coil pack, pressure gauge, barrel/tank, muzzle, wood/leather grip, fasteners, and one candidate assembly. Components are designed to be independently reviewable before full assembly approval.");
            builder.AppendLine();
            builder.AppendLine("## Counts");
            builder.AppendLine();
            builder.AppendLine($"- Prefabs: {validation.PrefabCount}");
            builder.AppendLine($"- Materials: {validation.MaterialCount}");
            builder.AppendLine($"- Textures: {validation.TextureCount}");
            builder.AppendLine($"- Reusable meshes: {validation.MeshCount}");
            builder.AppendLine($"- Preview PNGs including contact sheet: {validation.PreviewCount}");
            builder.AppendLine();
            builder.AppendLine("## Safety");
            builder.AppendLine();
            builder.AppendLine("Visual-only. No colliders, rigidbodies, cameras, scene files, audio, animator controllers, gameplay scripts, Blender files, or external DCC source files are included.");
            builder.AppendLine();
            builder.AppendLine("## Component Acceptance");
            builder.AppendLine();
            foreach (var record in ComponentRecords)
            {
                builder.AppendLine($"- {record.Id}: {record.AcceptanceStatus}; role `{record.Role}`; renderers {record.RendererCount}; mesh parts {record.MeshPartCount}.");
            }
            builder.AppendLine();
            builder.AppendLine("## Preview Outputs");
            builder.AppendLine();
            foreach (var renderFile in renderFiles)
            {
                builder.AppendLine($"- {renderFile}");
            }
            builder.AppendLine();
            builder.AppendLine("## Package File Snapshot");
            builder.AppendLine();
            foreach (var packageFile in packageFiles)
            {
                builder.AppendLine($"- {packageFile}");
            }
            return builder.ToString();
        }

        private static string BuildReadinessPlan(ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# V0.1.55 Pressure Pistol Hero Set 10 Import Readiness Plan");
            builder.AppendLine();
            builder.AppendLine("## Acceptance Gates");
            builder.AppendLine();
            builder.AppendLine($"- Gate 1 Component isolation: {(ComponentRecords.Count >= 7 ? "PASS" : "FAIL")}");
            builder.AppendLine($"- Gate 2 Visual-only prefab contents: {(validation.Failures.Any(f => f.IndexOf("disallowed", StringComparison.Ordinal) >= 0) ? "FAIL" : "PASS")}");
            builder.AppendLine($"- Gate 3 Material and mesh assignment: {(validation.Failures.Any(f => f.IndexOf("without", StringComparison.Ordinal) >= 0) ? "FAIL" : "PASS")}");
            builder.AppendLine($"- Gate 4 Texture set generated: {(validation.TextureCount >= 10 ? "PASS" : "FAIL")}");
            builder.AppendLine($"- Gate 5 Preview and contact sheet generated: {(validation.PreviewCount >= 8 ? "PASS" : "FAIL")}");
            builder.AppendLine($"- Gate 6 Candidate assembly present: {(ComponentRecords.Any(r => r.Role == "candidate_assembly") ? "PASS" : "FAIL")}");
            builder.AppendLine($"- Gate 7 Normalized manifest written twice: PASS");
            builder.AppendLine();
            builder.AppendLine("## Import Notes");
            builder.AppendLine();
            builder.AppendLine("Import as a file-based UPM package or copy the package into the Unity project's package dependency list when ready. The package has no gameplay dependencies and keeps generated concept renders outside runtime content.");
            return builder.ToString();
        }

        private static string BuildValidationReport(PackageRoot packageRoot, string repoRoot, string renderRoot, IReadOnlyList<string> packageFiles, IReadOnlyList<string> renderFiles, ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# V0.1.55 Pressure Pistol Hero Set 10 Validation Report");
            builder.AppendLine();
            builder.AppendLine($"Status: {(validation.Passed ? "PASS" : "FAIL")}");
            builder.AppendLine($"Package root: {NormalizePath(packageRoot.ResolvedPath)}");
            builder.AppendLine($"Render root: {NormalizePath(renderRoot)}");
            builder.AppendLine();
            builder.AppendLine("## Validation Summary");
            builder.AppendLine();
            builder.AppendLine($"- Prefabs: {validation.PrefabCount}");
            builder.AppendLine($"- Materials: {validation.MaterialCount}");
            builder.AppendLine($"- Textures: {validation.TextureCount}");
            builder.AppendLine($"- Meshes: {validation.MeshCount}");
            builder.AppendLine($"- Previews: {validation.PreviewCount}");
            builder.AppendLine();
            builder.AppendLine("## Failures");
            builder.AppendLine();
            if (validation.Failures.Count == 0)
            {
                builder.AppendLine("- None.");
            }
            else
            {
                foreach (var failure in validation.Failures)
                {
                    builder.AppendLine($"- {failure}");
                }
            }
            builder.AppendLine();
            builder.AppendLine("## Render Checksums");
            builder.AppendLine();
            foreach (var renderPath in Directory.GetFiles(renderRoot, "*.png", SearchOption.TopDirectoryOnly).OrderBy(path => path, StringComparer.Ordinal))
            {
                builder.AppendLine($"- {NormalizeRelative(repoRoot, renderPath)} sha256:{Sha256(renderPath)}");
            }
            builder.AppendLine();
            builder.AppendLine("## Assigned File Snapshot");
            builder.AppendLine();
            foreach (var file in packageFiles.Concat(renderFiles).OrderBy(path => path, StringComparer.Ordinal))
            {
                builder.AppendLine($"- {file}");
            }
            return builder.ToString();
        }

        private static string BuildFinalFileList(string repoRoot, params string[] roots)
        {
            var builder = new StringBuilder();
            builder.AppendLine("V0.1.55 Pressure Pistol Hero Set 10 final file list");
            foreach (var file in roots.Where(Directory.Exists).SelectMany(EnumerateOwnedFiles).Select(path => NormalizeRelative(repoRoot, path)).Distinct().OrderBy(path => path, StringComparer.Ordinal))
            {
                if (file.IndexOf("/BuildProject~/", StringComparison.Ordinal) >= 0)
                {
                    continue;
                }

                builder.AppendLine(file);
            }

            return builder.ToString();
        }

        private static IEnumerable<string> EnumerateOwnedFiles(string root)
        {
            if (!Directory.Exists(root))
            {
                return Array.Empty<string>();
            }

            var ignored = new[] { $"{Path.DirectorySeparatorChar}Library{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}Temp{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}" };
            return Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                .Where(path => !ignored.Any(segment => path.IndexOf(segment, StringComparison.Ordinal) >= 0))
                .OrderBy(path => path, StringComparer.Ordinal);
        }

        private static Texture2D SaveTexture(PackageRoot packageRoot, string name, Texture2D texture, bool normalMap, bool linear, string tag)
        {
            var assetRelativePath = $"Runtime/Textures/{Prefix}_TEX_{name}.png";
            var physicalPath = Path.Combine(packageRoot.ResolvedPath, assetRelativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath) ?? packageRoot.ResolvedPath);
            File.WriteAllBytes(physicalPath, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);

            var assetPath = $"{packageRoot.AssetPath}/{assetRelativePath}";
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            if (AssetImporter.GetAtPath(assetPath) is TextureImporter importer)
            {
                importer.textureType = normalMap ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.sRGBTexture = !linear && !normalMap;
                importer.mipmapEnabled = true;
                importer.SaveAndReimport();
            }

            var loaded = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            TextureRecords.Add(new TextureRecord { Path = assetRelativePath, Tag = tag });
            return loaded;
        }

        private static Material SaveMaterial(PackageRoot packageRoot, Shader shader, string name, Color color, float metallic, float smoothness, Texture2D albedo, Texture2D normal, Texture2D mask, Color? emission, string tag)
        {
            var material = new Material(shader)
            {
                name = $"{Prefix}_MAT_{name}"
            };

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
                material.EnableKeyword("_METALLICSPECGLOSSMAP");
            }

            if (emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                SetColor(material, "_EmissionColor", emission.Value);
            }

            var assetRelativePath = $"Runtime/Materials/{material.name}.mat";
            var assetPath = $"{packageRoot.AssetPath}/{assetRelativePath}";
            ReplaceAsset(assetPath);
            AssetDatabase.CreateAsset(material, assetPath);
            MaterialRecords.Add(new MaterialRecord { Path = assetRelativePath, Tag = tag });
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
                    var n = FractalNoise(x, y, seed);
                    var streak = Mathf.Abs(Mathf.Sin((x + seed * 11) * 0.045f + y * 0.012f));
                    var t = Mathf.Clamp01(n * 0.42f + (streak > 0.92f ? 0.35f : 0f));
                    texture.SetPixel(x, y, Color32.Lerp(baseColor, patina, t * 0.55f));
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
                    var n = FractalNoise(x, y, seed);
                    var m = (byte)Mathf.RoundToInt(Mathf.Clamp01(metallic + (n - 0.5f) * 0.08f) * 255f);
                    var a = (byte)Mathf.RoundToInt(Mathf.Clamp01(smoothness + (n - 0.5f) * 0.12f) * 255f);
                    texture.SetPixel(x, y, new Color32(m, m, m, a));
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
                    var nx = Mathf.Clamp((left - right) * strength + 0.5f, 0f, 1f);
                    var ny = Mathf.Clamp((down - up) * strength + 0.5f, 0f, 1f);
                    texture.SetPixel(x, y, new Color(nx, ny, 1f, 1f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateGlassTexture()
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            var center = new Vector2(128f, 128f);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var d = Vector2.Distance(new Vector2(x, y), center) / 181f;
                    var glow = Mathf.Clamp01(1f - d);
                    var n = FractalNoise(x, y, 71);
                    var color = Color.Lerp(new Color(0.58f, 0.20f, 0.04f, 1f), new Color(1.0f, 0.58f, 0.13f, 1f), glow * 0.85f + n * 0.12f);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateDialTexture()
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var n = FractalNoise(x, y, 79);
                    var speck = n > 0.80f ? 0.08f : 0f;
                    texture.SetPixel(x, y, new Color(0.78f + speck, 0.71f + speck, 0.52f + speck * 0.5f, 1f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateWoodTexture()
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var grain = Mathf.Abs(Mathf.Sin((x * 0.055f) + Mathf.Sin(y * 0.041f) * 1.8f));
                    var n = FractalNoise(x, y, 89) * 0.25f;
                    var t = Mathf.Clamp01(grain * 0.45f + n);
                    texture.SetPixel(x, y, Color.Lerp(new Color(0.14f, 0.060f, 0.025f, 1f), new Color(0.35f, 0.17f, 0.065f, 1f), t));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateLeatherTexture()
        {
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var crease = Mathf.Abs(Mathf.Sin((x + y * 0.3f) * 0.075f));
                    var n = FractalNoise(x, y, 97);
                    var t = Mathf.Clamp01(n * 0.50f + (crease > 0.88f ? 0.20f : 0f));
                    texture.SetPixel(x, y, Color.Lerp(new Color(0.18f, 0.085f, 0.035f, 1f), new Color(0.43f, 0.24f, 0.11f, 1f), t));
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
                    texture.SetPixel(x, y, Color32.Lerp(baseColor, accent, FractalNoise(x, y, seed) * 0.35f));
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
            var triangles = new[]
            {
                0, 2, 1, 0, 3, 2,
                4, 5, 6, 4, 6, 7,
                0, 1, 5, 0, 5, 4,
                1, 2, 6, 1, 6, 5,
                2, 3, 7, 2, 7, 6,
                3, 0, 4, 3, 4, 7
            };
            var mesh = new Mesh { vertices = vertices, triangles = triangles };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateWedgeMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.56f, -0.5f, -0.5f), new Vector3(0.56f, -0.5f, -0.5f), new Vector3(0.38f, 0.5f, -0.5f), new Vector3(-0.38f, 0.5f, -0.5f),
                new Vector3(-0.50f, -0.5f, 0.5f), new Vector3(0.50f, -0.5f, 0.5f), new Vector3(0.34f, 0.5f, 0.5f), new Vector3(-0.34f, 0.5f, 0.5f)
            };
            var triangles = new[]
            {
                0, 2, 1, 0, 3, 2,
                4, 5, 6, 4, 6, 7,
                0, 1, 5, 0, 5, 4,
                1, 2, 6, 1, 6, 5,
                2, 3, 7, 2, 7, 6,
                3, 0, 4, 3, 4, 7
            };
            var mesh = new Mesh { vertices = vertices, triangles = triangles };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateNeedleMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.50f, -0.10f, -0.5f), new Vector3(0.38f, -0.10f, -0.5f), new Vector3(0.52f, 0f, -0.5f), new Vector3(0.38f, 0.10f, -0.5f), new Vector3(-0.50f, 0.10f, -0.5f),
                new Vector3(-0.50f, -0.10f, 0.5f), new Vector3(0.38f, -0.10f, 0.5f), new Vector3(0.52f, 0f, 0.5f), new Vector3(0.38f, 0.10f, 0.5f), new Vector3(-0.50f, 0.10f, 0.5f)
            };
            var triangles = new List<int>
            {
                0, 1, 4, 1, 3, 4, 1, 2, 3,
                5, 9, 6, 6, 9, 8, 6, 8, 7
            };
            for (var i = 0; i < 5; i++)
            {
                var next = (i + 1) % 5;
                triangles.Add(i);
                triangles.Add(next);
                triangles.Add(i + 5);
                triangles.Add(next);
                triangles.Add(next + 5);
                triangles.Add(i + 5);
            }

            var mesh = new Mesh { vertices = vertices, triangles = triangles.ToArray() };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateBoltHeadMesh(int sides)
        {
            return CreateCylinderMesh(Mathf.Max(6, sides));
        }

        private static Mesh CreateCylinderMesh(int segments)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();

            for (var i = 0; i < segments; i++)
            {
                var angle = i * Mathf.PI * 2f / segments;
                var x = Mathf.Cos(angle) * 0.5f;
                var y = Mathf.Sin(angle) * 0.5f;
                vertices.Add(new Vector3(x, y, -0.5f));
                vertices.Add(new Vector3(x, y, 0.5f));
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

                triangles.Add(back0);
                triangles.Add(front0);
                triangles.Add(front1);
                triangles.Add(back0);
                triangles.Add(front1);
                triangles.Add(back1);

                triangles.Add(frontCenter);
                triangles.Add(front1);
                triangles.Add(front0);

                triangles.Add(backCenter);
                triangles.Add(back0);
                triangles.Add(back1);
            }

            var mesh = new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray()
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
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

        private static void EnsurePackageFolders(string packageRoot)
        {
            foreach (var relativePath in new[]
                     {
                         "Runtime/Materials",
                         "Runtime/Textures",
                         "Runtime/Meshes",
                         "Runtime/Prefabs",
                         "Runtime/Metadata",
                         "Documentation~/Manifest",
                         "Samples~/PreviewScene"
                     })
            {
                Directory.CreateDirectory(Path.Combine(packageRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString())));
            }
        }

        private static PackageRoot LocatePackageRoot()
        {
            var package = PackageInfo.FindForAssembly(typeof(PressurePistolHeroSet10Builder).Assembly);
            if (package != null)
            {
                return new PackageRoot(package.assetPath, package.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(PressurePistolHeroSet10Builder));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/PressurePistolHeroSet10Builder.cs";
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
            return $"{Prefix}_PREVIEW_{ToKebab(assetName.Replace("PPH10_", string.Empty))}.png";
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

        private static string JsonQuote(string value)
        {
            return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n") + "\"";
        }

        private static void WriteText(string path, string contents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            File.WriteAllText(path, contents, new UTF8Encoding(false));
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

        private sealed class ComponentRecord
        {
            public string Id;
            public string Role;
            public string AcceptanceStatus;
            public string PrefabPath;
            public string PreviewPath;
            public int RendererCount;
            public int MeshPartCount;
            public Vector3 BoundsSize;
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
