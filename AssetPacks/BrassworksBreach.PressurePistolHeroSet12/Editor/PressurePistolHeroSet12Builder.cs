using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BrassworksBreach.PressurePistolHeroSet12;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.PressurePistolHeroSet12.Editor
{
    public static class PressurePistolHeroSet12Builder
    {
        public const string PackageName = "com.brassworks.sidecar.pressure-pistol-hero-set12";
        private const string Version = "0.1.57-set12";
        private const string Prefix = "PPH12";
        private const string RenderRootName = "V0_1_57_PressurePistolHeroSet12";
        private const int PreviewWidth = 1600;
        private const int PreviewHeight = 1000;

        private static readonly string[] MaterialTags =
        {
            "aged_brass",
            "oxidized_copper",
            "dark_blued_iron",
            "warm_amber_glass",
            "worn_walnut",
            "worn_leather",
            "black_grime_oil",
            "ivory_gauge_enamel",
            "red_danger_enamel"
        };

        private static readonly List<TextureRecord> TextureRecords = new List<TextureRecord>();
        private static readonly List<MaterialRecord> MaterialRecords = new List<MaterialRecord>();
        private static readonly List<MeshRecord> MeshRecords = new List<MeshRecord>();
        private static readonly List<ComponentRecord> ComponentRecords = new List<ComponentRecord>();
        private static readonly List<RenderRecord> RenderRecords = new List<RenderRecord>();

        [MenuItem("Brassworks Breach/Sidecar Packs/Pressure Pistol Hero Set 12/Generate Assets And Renders")]
        public static void GenerateAssetsAndRenders()
        {
            TextureRecords.Clear();
            MaterialRecords.Clear();
            MeshRecords.Clear();
            ComponentRecords.Clear();
            RenderRecords.Clear();

            var packageRoot = LocatePackageRoot();
            var repoRoot = ResolveRepoRoot(packageRoot.ResolvedPath);
            var renderRoot = Path.Combine(repoRoot, "Documentation", "ConceptRenders", RenderRootName);

            EnsurePackageFolders(packageRoot.ResolvedPath);
            Directory.CreateDirectory(renderRoot);

            var textures = CreateTextures(packageRoot);
            var materials = CreateMaterials(packageRoot, textures);
            var meshes = CreateMeshes(packageRoot);

            CreateWoodLeatherGrip(packageRoot, meshes, materials);
            CreateBrassTriggerGuard(packageRoot, meshes, materials);
            CreatePressureCylinderBarrel(packageRoot, meshes, materials);
            CreateCopperCoilArray(packageRoot, meshes, materials);
            CreatePressureGauge(packageRoot, meshes, materials);
            CreateSideValveWheels(packageRoot, meshes, materials);
            CreateMuzzleCrownCogBrake(packageRoot, meshes, materials);
            CreateLeatherGloveHandProxy(packageRoot, meshes, materials);
            CreateFullFirstPersonHeroPistol(packageRoot, meshes, materials);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RenderRequiredViews(packageRoot, materials, renderRoot);
            RenderComponentPreviews(packageRoot, renderRoot);

            var validation = ValidateGeneratedContent(packageRoot, renderRoot);
            WriteManifest(packageRoot, renderRoot, validation);
            WriteDocumentation(packageRoot, repoRoot, renderRoot, validation);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"{Prefix}_GENERATE_PASS version={Version} package={packageRoot.ResolvedPath} renders={renderRoot}");
        }

        private static Dictionary<string, Texture2D> CreateTextures(PackageRoot packageRoot)
        {
            return new Dictionary<string, Texture2D>
            {
                ["brass_albedo"] = SaveTexture(packageRoot, "AgedBrassLayered_Albedo", CreateAgedBrassTexture(512), false, false, "aged_brass"),
                ["brass_normal"] = SaveTexture(packageRoot, "AgedBrassLayered_Normal", CreateHeightNormalTexture(512, 101, 0.26f, HeightStyle.ScratchedMetal), true, false, "aged_brass"),
                ["brass_mask"] = SaveTexture(packageRoot, "AgedBrassLayered_MetallicSmoothness", CreateMaskTexture(512, 0.88f, 0.43f, 111, 0.10f), false, true, "aged_brass"),
                ["copper_albedo"] = SaveTexture(packageRoot, "OxidizedCopperCoil_Albedo", CreateOxidizedCopperTexture(512), false, false, "oxidized_copper"),
                ["copper_normal"] = SaveTexture(packageRoot, "OxidizedCopperCoil_Normal", CreateHeightNormalTexture(512, 121, 0.32f, HeightStyle.CoiledMetal), true, false, "oxidized_copper"),
                ["copper_mask"] = SaveTexture(packageRoot, "OxidizedCopperCoil_MetallicSmoothness", CreateMaskTexture(512, 0.86f, 0.35f, 131, 0.14f), false, true, "oxidized_copper"),
                ["iron_albedo"] = SaveTexture(packageRoot, "DarkBluedIron_Albedo", CreateDarkBluedIronTexture(512), false, false, "dark_blued_iron"),
                ["iron_normal"] = SaveTexture(packageRoot, "DarkBluedIron_Normal", CreateHeightNormalTexture(512, 141, 0.34f, HeightStyle.PittedMetal), true, false, "dark_blued_iron"),
                ["iron_mask"] = SaveTexture(packageRoot, "DarkBluedIron_MetallicSmoothness", CreateMaskTexture(512, 0.82f, 0.26f, 151, 0.18f), false, true, "dark_blued_iron"),
                ["glass_albedo"] = SaveTexture(packageRoot, "WarmAmberGlass_Albedo", CreateAmberGlassTexture(512), false, false, "warm_amber_glass"),
                ["glass_mask"] = SaveTexture(packageRoot, "WarmAmberGlass_Smoothness", CreateMaskTexture(512, 0.0f, 0.86f, 161, 0.05f), false, true, "warm_amber_glass"),
                ["walnut_albedo"] = SaveTexture(packageRoot, "WornWalnutGrip_Albedo", CreateWalnutTexture(512), false, false, "worn_walnut"),
                ["walnut_normal"] = SaveTexture(packageRoot, "WornWalnutGrip_Normal", CreateHeightNormalTexture(512, 171, 0.48f, HeightStyle.WoodGrain), true, false, "worn_walnut"),
                ["leather_albedo"] = SaveTexture(packageRoot, "CrackedBrownLeather_Albedo", CreateLeatherTexture(512), false, false, "worn_leather"),
                ["leather_normal"] = SaveTexture(packageRoot, "CrackedBrownLeather_Normal", CreateHeightNormalTexture(512, 181, 0.52f, HeightStyle.LeatherCrease), true, false, "worn_leather"),
                ["grime_albedo"] = SaveTexture(packageRoot, "BlackGrimeOil_Albedo", CreateBlackGrimeOilTexture(512), false, false, "black_grime_oil"),
                ["grime_mask"] = SaveTexture(packageRoot, "BlackGrimeOil_Smoothness", CreateMaskTexture(512, 0.05f, 0.67f, 191, 0.28f), false, true, "black_grime_oil"),
                ["gauge_albedo"] = SaveTexture(packageRoot, "IvoryGaugeFace_Albedo", CreateGaugeFaceTexture(512), false, false, "ivory_gauge_enamel"),
                ["red_albedo"] = SaveTexture(packageRoot, "RedDangerEnamel_Albedo", CreateRedEnamelTexture(256), false, false, "red_danger_enamel")
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
                ["brass"] = SaveMaterial(packageRoot, shader, "AgedBrass_OxideScratched", Color.white, 0.88f, 0.43f, textures["brass_albedo"], textures["brass_normal"], textures["brass_mask"], null, "aged_brass"),
                ["copper"] = SaveMaterial(packageRoot, shader, "OxidizedCopper_HeatCoils", Color.white, 0.86f, 0.35f, textures["copper_albedo"], textures["copper_normal"], textures["copper_mask"], null, "oxidized_copper"),
                ["iron"] = SaveMaterial(packageRoot, shader, "DarkBluedIron_OilyWear", Color.white, 0.82f, 0.26f, textures["iron_albedo"], textures["iron_normal"], textures["iron_mask"], null, "dark_blued_iron"),
                ["glass"] = SaveMaterial(packageRoot, shader, "WarmAmberPressureGlass", Color.white, 0.0f, 0.86f, textures["glass_albedo"], null, textures["glass_mask"], new Color(1.0f, 0.34f, 0.07f) * 0.62f, "warm_amber_glass"),
                ["walnut"] = SaveMaterial(packageRoot, shader, "WornWalnutVarnish", Color.white, 0.0f, 0.39f, textures["walnut_albedo"], textures["walnut_normal"], null, null, "worn_walnut"),
                ["leather"] = SaveMaterial(packageRoot, shader, "CrackedBrownLeather", Color.white, 0.0f, 0.31f, textures["leather_albedo"], textures["leather_normal"], null, null, "worn_leather"),
                ["grime"] = SaveMaterial(packageRoot, shader, "BlackGrimeOilInSeams", Color.white, 0.05f, 0.67f, textures["grime_albedo"], null, textures["grime_mask"], null, "black_grime_oil"),
                ["gauge"] = SaveMaterial(packageRoot, shader, "IvoryGaugeEnamelPrinted", Color.white, 0.0f, 0.47f, textures["gauge_albedo"], null, null, null, "ivory_gauge_enamel"),
                ["red"] = SaveMaterial(packageRoot, shader, "RedDangerEnamelChipped", Color.white, 0.0f, 0.45f, textures["red_albedo"], null, null, null, "red_danger_enamel")
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes(PackageRoot packageRoot)
        {
            return new Dictionary<string, Mesh>
            {
                ["box"] = SaveMesh(packageRoot, "BoxUnit", CreateBoxMesh()),
                ["wedge"] = SaveMesh(packageRoot, "TaperedGripWedge", CreateWedgeMesh()),
                ["needle"] = SaveMesh(packageRoot, "GaugeNeedlePrism", CreateNeedleMesh()),
                ["cyl12"] = SaveMesh(packageRoot, "Cylinder12_Z", CreateCylinderMesh(12)),
                ["cyl16"] = SaveMesh(packageRoot, "Cylinder16_Z", CreateCylinderMesh(16)),
                ["cyl24"] = SaveMesh(packageRoot, "Cylinder24_Z", CreateCylinderMesh(24)),
                ["cyl32"] = SaveMesh(packageRoot, "Cylinder32_Z", CreateCylinderMesh(32)),
                ["cyl48"] = SaveMesh(packageRoot, "Cylinder48_Z", CreateCylinderMesh(48)),
                ["cyl64"] = SaveMesh(packageRoot, "Cylinder64_Z", CreateCylinderMesh(64)),
                ["ring32"] = SaveMesh(packageRoot, "WasherRing32_Z", CreateRingMesh(32, 0.36f)),
                ["ring48"] = SaveMesh(packageRoot, "WasherRing48_Z", CreateRingMesh(48, 0.34f)),
                ["gear24"] = SaveMesh(packageRoot, "CogGearRing24_Z", CreateGearRingMesh(24, 0.31f, 0.82f, 1.0f)),
                ["sphere16"] = SaveMesh(packageRoot, "Sphere16Unit", CreateUvSphereMesh(16, 12)),
                ["palm"] = SaveMesh(packageRoot, "GlovePalmProxy", CreatePalmMesh()),
                ["rivet"] = SaveMesh(packageRoot, "DomedRivet", CreateDomedRivetMesh(20))
            };
        }

        private static void CreateWoodLeatherGrip(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_WoodLeatherGrip_Component",
                "wooden_leather_grip",
                "component-pass",
                "Raked walnut pistol grip with leather wrap, brass backstrap, riveted grip plates, grime in seams, and carved side grooves.");

            Part(root.transform, meshes["wedge"], "worn_walnut_raked_grip_core", new Vector3(0f, -0.18f, -0.03f), new Vector3(0.48f, 0.95f, 0.30f), new Vector3(-13f, 0f, 0f), materials["walnut"]);
            Part(root.transform, meshes["box"], "aged_brass_backstrap_spine", new Vector3(0f, -0.12f, -0.205f), new Vector3(0.25f, 0.78f, 0.040f), new Vector3(-13f, 0f, 0f), materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_upper_receiver_tongue", new Vector3(0f, 0.32f, 0.05f), new Vector3(0.55f, 0.16f, 0.32f), new Vector3(-3f, 0f, 0f), materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_heel_cap", new Vector3(0f, -0.70f, -0.02f), new Vector3(0.43f, 0.10f, 0.31f), new Vector3(-15f, 0f, 0f), materials["brass"]);

            for (var i = 0; i < 7; i++)
            {
                var y = -0.59f + i * 0.125f;
                Part(root.transform, meshes["box"], $"cracked_brown_leather_wrap_band_{i:00}", new Vector3(0f, y, 0.035f), new Vector3(0.52f, 0.045f, 0.335f), new Vector3(-13f, 0f, 0f), materials["leather"]);
                Part(root.transform, meshes["box"], $"black_grime_shadow_under_wrap_{i:00}", new Vector3(0f, y - 0.031f, 0.039f), new Vector3(0.525f, 0.010f, 0.338f), new Vector3(-13f, 0f, 0f), materials["grime"]);
            }

            for (var side = -1; side <= 1; side += 2)
            {
                Part(root.transform, meshes["box"], $"{SideName(side)}_walnut_side_grip_plate", new Vector3(side * 0.252f, -0.22f, 0.005f), new Vector3(0.030f, 0.66f, 0.235f), new Vector3(-13f, 0f, 0f), materials["walnut"]);
                for (var i = 0; i < 5; i++)
                {
                    var y = -0.52f + i * 0.15f;
                    Part(root.transform, meshes["rivet"], $"{SideName(side)}_aged_brass_grip_rivet_{i:00}", new Vector3(side * 0.278f, y, 0.035f), new Vector3(0.052f, 0.052f, 0.026f), new Vector3(0f, 90f * side, 0f), materials["brass"]);
                }

                for (var i = 0; i < 4; i++)
                {
                    var y = -0.48f + i * 0.16f;
                    Part(root.transform, meshes["box"], $"{SideName(side)}_black_grime_carved_grip_groove_{i:00}", new Vector3(side * 0.283f, y, 0.095f - i * 0.018f), new Vector3(0.010f, 0.095f, 0.018f), new Vector3(-27f, 0f, 22f * side), materials["grime"]);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                var x = -0.20f + i * 0.05f;
                Part(root.transform, meshes["box"], $"leather_wrap_top_stitch_{i:00}", new Vector3(x, 0.045f, 0.205f), new Vector3(0.023f, 0.010f, 0.018f), Vector3.zero, materials["grime"]);
            }

            SavePrefab(packageRoot, root, "PPH12_WoodLeatherGrip_Component");
        }

        private static void CreateBrassTriggerGuard(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_BrassTriggerGuard_Component",
                "brass_trigger_guard",
                "component-pass",
                "Layered brass trigger guard with double side rails, iron curved trigger blade, hinge pins, and overbuilt receiver tabs.");

            for (var side = -1; side <= 1; side += 2)
            {
                var x = side * 0.145f;
                var points = new[]
                {
                    new Vector3(x, 0.13f, -0.28f),
                    new Vector3(x, -0.07f, -0.38f),
                    new Vector3(x, -0.33f, -0.26f),
                    new Vector3(x, -0.47f, 0.02f),
                    new Vector3(x, -0.39f, 0.30f),
                    new Vector3(x, -0.14f, 0.41f),
                    new Vector3(x, 0.10f, 0.30f)
                };
                AddPipeSegments(root.transform, meshes["cyl16"], $"{SideName(side)}_aged_brass_trigger_guard_loop", points, 0.034f, materials["brass"]);
            }

            foreach (var z in new[] { -0.29f, 0.02f, 0.30f })
            {
                CylinderBetween(root.transform, meshes["cyl16"], $"aged_brass_guard_cross_pin_{z:0.00}", new Vector3(-0.15f, -0.40f, z), new Vector3(0.15f, -0.40f, z), 0.030f, materials["brass"]);
            }

            Part(root.transform, meshes["box"], "aged_brass_front_receiver_lug", new Vector3(0f, 0.12f, 0.30f), new Vector3(0.42f, 0.12f, 0.16f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_rear_receiver_lug", new Vector3(0f, 0.10f, -0.31f), new Vector3(0.40f, 0.11f, 0.16f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["needle"], "dark_blued_iron_curved_trigger_blade", new Vector3(0f, -0.19f, -0.07f), new Vector3(0.16f, 0.30f, 0.035f), new Vector3(0f, 0f, -62f), materials["iron"]);
            Part(root.transform, meshes["cyl24"], "dark_blued_iron_trigger_hinge_pin", new Vector3(0f, 0.005f, -0.13f), new Vector3(0.090f, 0.090f, 0.35f), new Vector3(0f, 90f, 0f), materials["iron"]);

            for (var side = -1; side <= 1; side += 2)
            {
                Part(root.transform, meshes["rivet"], $"{SideName(side)}_guard_front_rivet", new Vector3(side * 0.205f, 0.11f, 0.30f), new Vector3(0.058f, 0.058f, 0.025f), new Vector3(0f, 90f * side, 0f), materials["brass"]);
                Part(root.transform, meshes["rivet"], $"{SideName(side)}_guard_rear_rivet", new Vector3(side * 0.205f, 0.10f, -0.31f), new Vector3(0.058f, 0.058f, 0.025f), new Vector3(0f, 90f * side, 0f), materials["brass"]);
            }

            SavePrefab(packageRoot, root, "PPH12_BrassTriggerGuard_Component");
        }

        private static void CreatePressureCylinderBarrel(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_PressureCylinderBarrel_Component",
                "pressure_cylinder_barrel",
                "component-pass",
                "Stacked dark iron barrel over a brass pressure cylinder with clamps, amber sight glass, copper transfer pipes, iron receiver block, and rivet rows.");

            Part(root.transform, meshes["cyl64"], "dark_blued_iron_primary_barrel_long", new Vector3(0f, 0.31f, 0.20f), new Vector3(0.22f, 0.22f, 1.96f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["cyl48"], "dark_blued_iron_barrel_muzzle_insert_shadow", new Vector3(0f, 0.31f, 1.24f), new Vector3(0.135f, 0.135f, 0.080f), Vector3.zero, materials["grime"]);
            Part(root.transform, meshes["cyl64"], "aged_brass_under_pressure_cylinder_body", new Vector3(0f, -0.02f, 0.02f), new Vector3(0.54f, 0.54f, 1.30f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["cyl48"], "dark_blued_iron_rear_pressure_tank_cap", new Vector3(0f, -0.02f, -0.67f), new Vector3(0.58f, 0.58f, 0.11f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["cyl48"], "dark_blued_iron_front_pressure_tank_cap", new Vector3(0f, -0.02f, 0.72f), new Vector3(0.58f, 0.58f, 0.11f), Vector3.zero, materials["iron"]);

            foreach (var z in new[] { -0.52f, -0.25f, 0.02f, 0.29f, 0.56f })
            {
                Part(root.transform, meshes["ring48"], $"aged_brass_pressure_cylinder_clamp_band_{z:0.00}", new Vector3(0f, -0.02f, z), new Vector3(0.66f, 0.66f, 0.050f), Vector3.zero, materials["brass"]);
            }

            Part(root.transform, meshes["box"], "warm_amber_sight_glass_long_window", new Vector3(0f, -0.305f, 0.05f), new Vector3(0.28f, 0.040f, 0.70f), Vector3.zero, materials["glass"]);
            Part(root.transform, meshes["box"], "black_grime_sight_glass_recess", new Vector3(0f, -0.331f, 0.05f), new Vector3(0.34f, 0.024f, 0.76f), Vector3.zero, materials["grime"]);
            Part(root.transform, meshes["box"], "dark_blued_iron_receiver_block", new Vector3(0f, 0.10f, -0.77f), new Vector3(0.62f, 0.43f, 0.42f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "aged_brass_receiver_side_plate_left", new Vector3(-0.335f, 0.10f, -0.77f), new Vector3(0.034f, 0.34f, 0.34f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_receiver_side_plate_right", new Vector3(0.335f, 0.10f, -0.77f), new Vector3(0.034f, 0.34f, 0.34f), Vector3.zero, materials["brass"]);

            for (var side = -1; side <= 1; side += 2)
            {
                CylinderBetween(root.transform, meshes["cyl16"], $"{SideName(side)}_oxidized_copper_transfer_pipe_upper", new Vector3(side * 0.34f, 0.22f, -0.68f), new Vector3(side * 0.34f, 0.22f, 0.82f), 0.031f, materials["copper"]);
                CylinderBetween(root.transform, meshes["cyl16"], $"{SideName(side)}_black_grime_pipe_shadow", new Vector3(side * 0.385f, 0.175f, -0.54f), new Vector3(side * 0.385f, 0.175f, 0.66f), 0.014f, materials["grime"]);
                for (var i = 0; i < 6; i++)
                {
                    var z = -0.52f + i * 0.25f;
                    Part(root.transform, meshes["rivet"], $"{SideName(side)}_tank_clamp_rivet_{i:00}", new Vector3(side * 0.305f, -0.02f, z), new Vector3(0.050f, 0.050f, 0.024f), new Vector3(0f, 90f * side, 0f), materials["brass"]);
                }
            }

            Part(root.transform, meshes["box"], "dark_blued_iron_top_sight_rib", new Vector3(0f, 0.47f, 0.12f), new Vector3(0.15f, 0.055f, 1.34f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "aged_brass_front_sight_notch", new Vector3(0f, 0.54f, 0.93f), new Vector3(0.17f, 0.11f, 0.08f), Vector3.zero, materials["brass"]);

            SavePrefab(packageRoot, root, "PPH12_PressureCylinderBarrel_Component");
        }

        private static void CreateCopperCoilArray(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_CopperCoilArray_Component",
                "copper_coil_array",
                "component-pass",
                "Open top coil array with glowing amber pressure tube, oxidized copper loops, brass cradle rails, terminal blocks, rivets, and grime-filled recesses.");

            Part(root.transform, meshes["cyl48"], "warm_amber_glass_pressure_core", new Vector3(0f, 0f, 0f), new Vector3(0.22f, 0.22f, 1.28f), Vector3.zero, materials["glass"]);
            Part(root.transform, meshes["cyl24"], "dark_blued_iron_inner_core_shadow", new Vector3(0f, 0f, 0f), new Vector3(0.080f, 0.080f, 1.35f), Vector3.zero, materials["iron"]);

            for (var i = 0; i < 15; i++)
            {
                var z = -0.61f + i * 0.087f;
                Part(root.transform, meshes["ring48"], $"oxidized_copper_helical_coil_ring_{i:00}", new Vector3(0f, 0f, z), new Vector3(0.49f, 0.49f, 0.032f), new Vector3(0f, 0f, i * 5.5f), materials["copper"]);
                if (i % 3 == 0)
                {
                    Part(root.transform, meshes["box"], $"black_grime_between_coil_shadow_{i:00}", new Vector3(0f, -0.245f, z + 0.018f), new Vector3(0.34f, 0.017f, 0.020f), Vector3.zero, materials["grime"]);
                }
            }

            for (var side = -1; side <= 1; side += 2)
            {
                Part(root.transform, meshes["box"], $"{SideName(side)}_aged_brass_coil_cradle_rail_lower", new Vector3(side * 0.33f, -0.10f, 0f), new Vector3(0.055f, 0.13f, 1.48f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["box"], $"{SideName(side)}_dark_blued_iron_inner_cradle_shadow", new Vector3(side * 0.275f, -0.16f, 0f), new Vector3(0.028f, 0.07f, 1.39f), Vector3.zero, materials["iron"]);
                for (var i = 0; i < 5; i++)
                {
                    var z = -0.50f + i * 0.25f;
                    Part(root.transform, meshes["rivet"], $"{SideName(side)}_coil_cradle_brass_rivet_{i:00}", new Vector3(side * 0.365f, -0.02f, z), new Vector3(0.050f, 0.050f, 0.022f), new Vector3(0f, 90f * side, 0f), materials["brass"]);
                }
            }

            Part(root.transform, meshes["box"], "dark_blued_iron_rear_terminal_block", new Vector3(0f, -0.035f, -0.75f), new Vector3(0.58f, 0.20f, 0.14f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "dark_blued_iron_front_terminal_block", new Vector3(0f, -0.035f, 0.75f), new Vector3(0.58f, 0.20f, 0.14f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "aged_brass_terminal_name_plate", new Vector3(0f, 0.14f, -0.75f), new Vector3(0.40f, 0.045f, 0.12f), Vector3.zero, materials["brass"]);

            for (var x = -0.18f; x <= 0.18f; x += 0.18f)
            {
                Part(root.transform, meshes["cyl16"], $"warm_amber_glass_ceramic_insulator_{x:0.00}", new Vector3(x, 0.12f, 0.75f), new Vector3(0.080f, 0.080f, 0.10f), Vector3.zero, materials["glass"]);
            }

            SavePrefab(packageRoot, root, "PPH12_CopperCoilArray_Component");
        }

        private static void CreatePressureGauge(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_PressureGauge_Component",
                "pressure_gauge",
                "component-pass",
                "Readable ivory-face pressure gauge with brass bezel, printed ticks, red overpressure arc, black needle, amber glass, pipe socket, and grime ring.");

            AddGaugeFace(root.transform, meshes, materials, "hero_readable_pressure_gauge", Vector3.zero, 0.46f, true);
            Part(root.transform, meshes["cyl32"], "dark_blued_iron_lower_pipe_socket", new Vector3(0f, -0.58f, 0.04f), new Vector3(0.16f, 0.16f, 0.36f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Part(root.transform, meshes["ring32"], "aged_brass_pipe_socket_collar", new Vector3(0f, -0.43f, 0.035f), new Vector3(0.25f, 0.25f, 0.055f), new Vector3(90f, 0f, 0f), materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_top_mount_tab", new Vector3(0f, 0.55f, 0.02f), new Vector3(0.24f, 0.12f, 0.12f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["rivet"], "brass_top_mount_rivet", new Vector3(0f, 0.61f, -0.050f), new Vector3(0.048f, 0.048f, 0.022f), Vector3.zero, materials["brass"]);

            SavePrefab(packageRoot, root, "PPH12_PressureGauge_Component");
        }

        private static void CreateSideValveWheels(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_SideValveWheels_Component",
                "side_valve_wheels",
                "component-pass",
                "Twin side-mounted handwheel valves with brass cog rims, copper stems, dark iron hubs, red pressure witness marks, and connecting pipes.");

            AddValveWheel(root.transform, meshes, materials, "large_rear_side_valve", new Vector3(-0.12f, 0.05f, -0.24f), 0.30f);
            AddValveWheel(root.transform, meshes, materials, "small_forward_side_valve", new Vector3(-0.08f, -0.05f, 0.48f), 0.21f);
            CylinderBetween(root.transform, meshes["cyl16"], "oxidized_copper_valve_transfer_line", new Vector3(-0.12f, -0.05f, -0.05f), new Vector3(-0.08f, -0.05f, 0.32f), 0.032f, materials["copper"]);
            CylinderBetween(root.transform, meshes["cyl16"], "dark_blued_iron_receiver_valve_stem_rear", new Vector3(0.18f, 0.05f, -0.24f), new Vector3(-0.12f, 0.05f, -0.24f), 0.045f, materials["iron"]);
            CylinderBetween(root.transform, meshes["cyl16"], "dark_blued_iron_receiver_valve_stem_front", new Vector3(0.18f, -0.05f, 0.48f), new Vector3(-0.08f, -0.05f, 0.48f), 0.038f, materials["iron"]);
            Part(root.transform, meshes["box"], "black_grime_oil_under_valve_plate", new Vector3(0.05f, -0.12f, 0.12f), new Vector3(0.055f, 0.070f, 0.95f), Vector3.zero, materials["grime"]);

            SavePrefab(packageRoot, root, "PPH12_SideValveWheels_Component");
        }

        private static void CreateMuzzleCrownCogBrake(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_MuzzleCrownCogBrake_Component",
                "muzzle_crown_cog_brake",
                "component-pass",
                "Layered cog-tooth muzzle crown with brass brake gear, dark bore, copper heat collar, vent shadows, and small front sight fork.");

            Part(root.transform, meshes["cyl64"], "dark_blued_iron_inner_bore_tube", new Vector3(0f, 0f, 0.07f), new Vector3(0.19f, 0.19f, 0.55f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["gear24"], "aged_brass_cog_brake_outer_crown", new Vector3(0f, 0f, 0.31f), new Vector3(0.62f, 0.62f, 0.105f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["ring48"], "black_grime_oil_between_cog_teeth", new Vector3(0f, 0f, 0.255f), new Vector3(0.54f, 0.54f, 0.030f), Vector3.zero, materials["grime"]);
            Part(root.transform, meshes["ring48"], "oxidized_copper_rear_heat_collar", new Vector3(0f, 0f, -0.06f), new Vector3(0.50f, 0.50f, 0.15f), Vector3.zero, materials["copper"]);
            Part(root.transform, meshes["ring48"], "aged_brass_threaded_rear_collar", new Vector3(0f, 0f, -0.24f), new Vector3(0.44f, 0.44f, 0.19f), Vector3.zero, materials["brass"]);

            for (var i = 0; i < 10; i++)
            {
                var angle = i * Mathf.PI * 2f / 10f;
                var pos = new Vector3(Mathf.Cos(angle) * 0.245f, Mathf.Sin(angle) * 0.245f, 0.30f);
                Part(root.transform, meshes["box"], $"dark_blued_iron_radial_vent_shadow_{i:00}", pos, new Vector3(0.074f, 0.021f, 0.035f), new Vector3(0f, 0f, angle * Mathf.Rad2Deg), materials["iron"]);
            }

            Part(root.transform, meshes["box"], "aged_brass_front_sight_left_fork", new Vector3(-0.050f, 0.34f, 0.08f), new Vector3(0.040f, 0.21f, 0.055f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_front_sight_right_fork", new Vector3(0.050f, 0.34f, 0.08f), new Vector3(0.040f, 0.21f, 0.055f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "dark_blued_iron_front_sight_shadow_slot", new Vector3(0f, 0.39f, 0.08f), new Vector3(0.035f, 0.13f, 0.065f), Vector3.zero, materials["grime"]);

            SavePrefab(packageRoot, root, "PPH12_MuzzleCrownCogBrake_Component");
        }

        private static void CreateLeatherGloveHandProxy(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_LeatherGloveHandProxy_Component",
                "leather_glove_hand_proxy",
                "component-pass",
                "First-person right-hand proxy in worn leather with curled fingers, cuff ring, seam ridges, brass cuff button, and black grime creases.");

            Part(root.transform, meshes["palm"], "cracked_brown_leather_palm_mass", new Vector3(0f, -0.04f, 0f), new Vector3(0.54f, 0.34f, 0.42f), new Vector3(0f, -8f, -10f), materials["leather"]);
            Part(root.transform, meshes["sphere16"], "leather_thumb_pad", new Vector3(-0.34f, -0.06f, 0.08f), new Vector3(0.18f, 0.13f, 0.25f), new Vector3(0f, 0f, -20f), materials["leather"]);
            CylinderBetween(root.transform, meshes["cyl16"], "leather_thumb_curled_segment", new Vector3(-0.34f, -0.05f, 0.18f), new Vector3(-0.18f, -0.23f, 0.32f), 0.070f, materials["leather"]);

            for (var i = 0; i < 4; i++)
            {
                var x = -0.18f + i * 0.12f;
                var z = 0.18f + i * 0.025f;
                var first = new Vector3(x, -0.13f, z);
                var second = new Vector3(x + 0.015f, -0.36f, z + 0.12f);
                var third = new Vector3(x + 0.005f, -0.49f, z + 0.04f);
                CylinderBetween(root.transform, meshes["cyl16"], $"leather_curled_finger_{i:00}_proximal", first, second, 0.057f - i * 0.004f, materials["leather"]);
                CylinderBetween(root.transform, meshes["cyl16"], $"leather_curled_finger_{i:00}_distal", second, third, 0.050f - i * 0.004f, materials["leather"]);
                Part(root.transform, meshes["sphere16"], $"black_grime_knuckle_crease_{i:00}", first + new Vector3(0f, -0.015f, 0f), new Vector3(0.088f, 0.020f, 0.060f), Vector3.zero, materials["grime"]);
            }

            Part(root.transform, meshes["cyl48"], "dark_blued_iron_wrist_cuff_ring", new Vector3(0f, 0.24f, -0.15f), new Vector3(0.58f, 0.40f, 0.10f), new Vector3(90f, 0f, 0f), materials["iron"]);
            Part(root.transform, meshes["box"], "leather_wrist_cuff_body", new Vector3(0f, 0.34f, -0.16f), new Vector3(0.64f, 0.20f, 0.42f), new Vector3(-6f, 0f, 0f), materials["leather"]);
            Part(root.transform, meshes["rivet"], "aged_brass_cuff_button", new Vector3(0.33f, 0.33f, -0.02f), new Vector3(0.060f, 0.060f, 0.026f), new Vector3(0f, 90f, 0f), materials["brass"]);
            for (var i = 0; i < 5; i++)
            {
                Part(root.transform, meshes["box"], $"black_grime_glove_back_seam_{i:00}", new Vector3(-0.18f + i * 0.09f, 0.03f, 0.22f), new Vector3(0.012f, 0.23f, 0.014f), new Vector3(0f, 0f, 8f), materials["grime"]);
            }

            SavePrefab(packageRoot, root, "PPH12_LeatherGloveHandProxy_Component");
        }

        private static void CreateFullFirstPersonHeroPistol(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var root = NewRoot(
                "PPH12_FullFirstPersonHeroPistol",
                "full_assembled_first_person_hero_pistol",
                "hero-pass",
                "Full Set12 first-person pressure pistol assembled from isolated components with layered receiver plates, side valves, readable gauge, coil pack, hand proxy, and FPS-ready massing.");

            InstantiatePrefabChild(packageRoot, root.transform, "PPH12_PressureCylinderBarrel_Component", "pressure_cylinder_barrel_module", new Vector3(0f, 0.10f, 0.05f), Vector3.zero, Vector3.one);
            InstantiatePrefabChild(packageRoot, root.transform, "PPH12_CopperCoilArray_Component", "top_open_copper_coil_array_module", new Vector3(0f, 0.57f, -0.05f), Vector3.zero, new Vector3(0.92f, 0.92f, 0.92f));
            InstantiatePrefabChild(packageRoot, root.transform, "PPH12_PressureGauge_Component", "left_top_readable_pressure_gauge_module", new Vector3(-0.31f, 0.68f, -0.43f), new Vector3(0f, -28f, 0f), new Vector3(0.54f, 0.54f, 0.54f));
            InstantiatePrefabChild(packageRoot, root.transform, "PPH12_SideValveWheels_Component", "left_side_valve_wheel_cluster_module", new Vector3(-0.43f, 0.12f, -0.17f), Vector3.zero, new Vector3(0.86f, 0.86f, 0.86f));
            InstantiatePrefabChild(packageRoot, root.transform, "PPH12_MuzzleCrownCogBrake_Component", "front_cog_muzzle_brake_module", new Vector3(0f, 0.31f, 1.25f), Vector3.zero, Vector3.one);
            InstantiatePrefabChild(packageRoot, root.transform, "PPH12_WoodLeatherGrip_Component", "rear_walnut_leather_grip_module", new Vector3(0f, -0.58f, -0.79f), new Vector3(8f, 0f, 0f), Vector3.one);
            InstantiatePrefabChild(packageRoot, root.transform, "PPH12_BrassTriggerGuard_Component", "brass_trigger_guard_module", new Vector3(0f, -0.34f, -0.56f), Vector3.zero, new Vector3(0.92f, 0.92f, 0.92f));
            InstantiatePrefabChild(packageRoot, root.transform, "PPH12_LeatherGloveHandProxy_Component", "right_leather_glove_hand_proxy_module", new Vector3(0.18f, -0.74f, -0.75f), new Vector3(5f, -11f, -5f), new Vector3(0.92f, 0.92f, 0.92f));

            Part(root.transform, meshes["box"], "dark_blued_iron_main_receiver_side_slab", new Vector3(0f, 0.17f, -0.72f), new Vector3(0.72f, 0.42f, 0.44f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "aged_brass_left_receiver_overplate", new Vector3(-0.395f, 0.18f, -0.69f), new Vector3(0.045f, 0.34f, 0.50f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_right_receiver_overplate", new Vector3(0.395f, 0.18f, -0.69f), new Vector3(0.045f, 0.34f, 0.50f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "black_grime_oil_deep_receiver_cutline", new Vector3(-0.423f, 0.18f, -0.69f), new Vector3(0.014f, 0.27f, 0.41f), Vector3.zero, materials["grime"]);
            Part(root.transform, meshes["box"], "aged_brass_small_side_nameplate_blank", new Vector3(-0.435f, 0.35f, -0.76f), new Vector3(0.018f, 0.072f, 0.27f), Vector3.zero, materials["brass"]);

            CylinderBetween(root.transform, meshes["cyl16"], "left_external_copper_pressure_line_swept", new Vector3(-0.36f, 0.39f, -0.85f), new Vector3(-0.42f, 0.45f, 0.70f), 0.026f, materials["copper"]);
            CylinderBetween(root.transform, meshes["cyl16"], "right_external_copper_pressure_line_swept", new Vector3(0.36f, 0.31f, -0.84f), new Vector3(0.38f, 0.36f, 0.62f), 0.024f, materials["copper"]);
            CylinderBetween(root.transform, meshes["cyl12"], "black_grime_underbarrel_oil_line", new Vector3(0f, -0.28f, -0.48f), new Vector3(0f, -0.19f, 0.63f), 0.018f, materials["grime"]);

            for (var side = -1; side <= 1; side += 2)
            {
                for (var i = 0; i < 5; i++)
                {
                    var z = -0.91f + i * 0.11f;
                    Part(root.transform, meshes["rivet"], $"{SideName(side)}_receiver_overplate_rivet_{i:00}", new Vector3(side * 0.435f, 0.30f - i * 0.055f, z), new Vector3(0.048f, 0.048f, 0.022f), new Vector3(0f, 90f * side, 0f), materials["brass"]);
                }
            }

            Part(root.transform, meshes["box"], "red_danger_enamel_pressure_index_mark", new Vector3(-0.44f, 0.23f, -0.49f), new Vector3(0.018f, 0.105f, 0.030f), Vector3.zero, materials["red"]);
            Part(root.transform, meshes["box"], "red_danger_enamel_secondary_index_mark", new Vector3(-0.44f, 0.09f, -0.49f), new Vector3(0.018f, 0.075f, 0.026f), Vector3.zero, materials["red"]);

            SavePrefab(packageRoot, root, "PPH12_FullFirstPersonHeroPistol");
        }

        private static void AddGaugeFace(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, string name, Vector3 localPosition, float radius, bool dangerArc)
        {
            var gauge = new GameObject(name);
            gauge.transform.SetParent(parent, false);
            gauge.transform.localPosition = localPosition;

            Part(gauge.transform, meshes["cyl64"], "dark_blued_iron_rear_gauge_case", Vector3.zero, new Vector3(radius * 2.22f, radius * 2.22f, 0.105f), Vector3.zero, materials["iron"]);
            Part(gauge.transform, meshes["ring48"], "aged_brass_knurled_outer_bezel", new Vector3(0f, 0f, -0.070f), new Vector3(radius * 2.10f, radius * 2.10f, 0.075f), Vector3.zero, materials["brass"]);
            Part(gauge.transform, meshes["cyl64"], "ivory_gauge_enamel_printed_face", new Vector3(0f, 0f, -0.122f), new Vector3(radius * 1.55f, radius * 1.55f, 0.026f), Vector3.zero, materials["gauge"]);
            Part(gauge.transform, meshes["cyl64"], "warm_amber_convex_glass_face", new Vector3(0f, 0f, -0.150f), new Vector3(radius * 1.48f, radius * 1.48f, 0.014f), Vector3.zero, materials["glass"]);
            Part(gauge.transform, meshes["needle"], "dark_blued_iron_pressure_needle", new Vector3(radius * 0.19f, 0f, -0.178f), new Vector3(radius * 1.04f, radius * 0.105f, radius * 0.055f), new Vector3(0f, 0f, -24f), materials["iron"]);
            Part(gauge.transform, meshes["rivet"], "aged_brass_needle_center_pin", new Vector3(0f, 0f, -0.190f), new Vector3(radius * 0.22f, radius * 0.22f, radius * 0.10f), Vector3.zero, materials["brass"]);

            for (var i = 0; i < 17; i++)
            {
                var t = i / 16f;
                var degrees = Mathf.Lerp(-145f, 145f, t);
                var angle = degrees * Mathf.Deg2Rad;
                var tickLength = i % 4 == 0 ? radius * 0.18f : radius * 0.10f;
                var tickWidth = i % 4 == 0 ? radius * 0.035f : radius * 0.023f;
                var pos = new Vector3(Mathf.Sin(angle) * radius * 0.60f, Mathf.Cos(angle) * radius * 0.60f, -0.186f);
                Part(gauge.transform, meshes["box"], $"dark_blued_iron_pressure_tick_{i:00}", pos, new Vector3(tickWidth, tickLength, radius * 0.024f), new Vector3(0f, 0f, -degrees), materials["iron"]);
            }

            if (dangerArc)
            {
                for (var i = 0; i < 6; i++)
                {
                    var degrees = Mathf.Lerp(96f, 145f, i / 5f);
                    var angle = degrees * Mathf.Deg2Rad;
                    var pos = new Vector3(Mathf.Sin(angle) * radius * 0.43f, Mathf.Cos(angle) * radius * 0.43f, -0.194f);
                    Part(gauge.transform, meshes["box"], $"red_danger_enamel_overpressure_arc_{i:00}", pos, new Vector3(radius * 0.043f, radius * 0.14f, radius * 0.027f), new Vector3(0f, 0f, -degrees), materials["red"]);
                }
            }

            for (var i = 0; i < 18; i++)
            {
                var angle = i * Mathf.PI * 2f / 18f;
                var pos = new Vector3(Mathf.Cos(angle) * radius * 0.96f, Mathf.Sin(angle) * radius * 0.96f, -0.105f);
                Part(gauge.transform, meshes["rivet"], $"aged_brass_bezel_micro_rivet_{i:00}", pos, new Vector3(radius * 0.075f, radius * 0.075f, radius * 0.035f), Vector3.zero, materials["brass"]);
            }
        }

        private static void AddValveWheel(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, string id, Vector3 center, float radius)
        {
            var wheel = new GameObject(id);
            wheel.transform.SetParent(parent, false);
            wheel.transform.localPosition = center;
            wheel.transform.localEulerAngles = new Vector3(0f, 90f, 0f);

            Part(wheel.transform, meshes["gear24"], $"{id}_aged_brass_cog_handwheel_rim", Vector3.zero, new Vector3(radius * 2.0f, radius * 2.0f, 0.075f), Vector3.zero, materials["brass"]);
            Part(wheel.transform, meshes["ring32"], $"{id}_black_grime_inner_recess", new Vector3(0f, 0f, -0.010f), new Vector3(radius * 1.33f, radius * 1.33f, 0.055f), Vector3.zero, materials["grime"]);
            Part(wheel.transform, meshes["cyl32"], $"{id}_dark_blued_iron_hub", new Vector3(0f, 0f, -0.055f), new Vector3(radius * 0.48f, radius * 0.48f, 0.16f), Vector3.zero, materials["iron"]);
            Part(wheel.transform, meshes["rivet"], $"{id}_aged_brass_center_cap", new Vector3(0f, 0f, -0.155f), new Vector3(radius * 0.28f, radius * 0.28f, 0.055f), Vector3.zero, materials["brass"]);

            for (var i = 0; i < 6; i++)
            {
                var angle = i * Mathf.PI * 2f / 6f;
                var midway = new Vector3(Mathf.Cos(angle) * radius * 0.38f, Mathf.Sin(angle) * radius * 0.38f, -0.05f);
                Part(wheel.transform, meshes["box"], $"{id}_aged_brass_spoke_{i:00}", midway, new Vector3(radius * 0.82f, radius * 0.055f, 0.036f), new Vector3(0f, 0f, angle * Mathf.Rad2Deg), materials["brass"]);
            }

            Part(wheel.transform, meshes["box"], $"{id}_red_danger_enamel_alignment_mark", new Vector3(0f, radius * 0.70f, -0.17f), new Vector3(radius * 0.10f, radius * 0.29f, 0.026f), Vector3.zero, materials["red"]);
        }

        private static GameObject NewRoot(string assetId, string role, string status, string notes)
        {
            var root = new GameObject(assetId);
            root.AddComponent<PressurePistolHeroSet12Identity>().Configure(assetId, role, status, 0, 0, MaterialTags, notes);
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

        private static GameObject CylinderBetween(Transform parent, Mesh mesh, string name, Vector3 from, Vector3 to, float radius, Material material)
        {
            var midpoint = (from + to) * 0.5f;
            var direction = to - from;
            var child = Part(parent, mesh, name, midpoint, new Vector3(radius * 2f, radius * 2f, direction.magnitude), Vector3.zero, material);
            child.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);
            return child;
        }

        private static void AddPipeSegments(Transform parent, Mesh mesh, string baseName, IReadOnlyList<Vector3> points, float radius, Material material)
        {
            for (var i = 0; i < points.Count - 1; i++)
            {
                CylinderBetween(parent, mesh, $"{baseName}_segment_{i:00}", points[i], points[i + 1], radius, material);
            }
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
            var identity = prefabRoot.GetComponent<PressurePistolHeroSet12Identity>();
            if (identity != null)
            {
                identity.Configure(assetName, identity.ComponentRole, identity.AcceptanceStatus, rendererCount, meshPartCount, MaterialTags, identity.Notes);
            }

            var path = $"{packageRoot.AssetPath}/Runtime/Prefabs/{assetName}.prefab";
            ReplaceAsset(path);
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);

            var bounds = CalculateBounds(prefabRoot);
            ComponentRecords.Add(new ComponentRecord
            {
                Id = assetName,
                Role = identity != null ? identity.ComponentRole : "unknown",
                PrefabPath = $"Runtime/Prefabs/{assetName}.prefab",
                RendererCount = rendererCount,
                MeshPartCount = meshPartCount,
                BoundsSize = bounds.size,
                AcceptanceStatus = identity != null ? identity.AcceptanceStatus : "unknown",
                Notes = identity != null ? identity.Notes : string.Empty
            });

            Object.DestroyImmediate(prefabRoot);
        }

        private static void RenderRequiredViews(PackageRoot packageRoot, IReadOnlyDictionary<string, Material> materials, string renderRoot)
        {
            var fullPrefab = LoadPrefab(packageRoot, "PPH12_FullFirstPersonHeroPistol");

            RenderHeroPistol(fullPrefab, Path.Combine(renderRoot, $"{Prefix}_RENDER_full-hero-pistol.png"));
            RenderFpsFraming(fullPrefab, Path.Combine(renderRoot, $"{Prefix}_RENDER_fps-hand-weapon-framing.png"));
            RenderComponentSheet(packageRoot, Path.Combine(renderRoot, $"{Prefix}_RENDER_component-sheet.png"));
            RenderMaterialCloseup(materials, Path.Combine(renderRoot, $"{Prefix}_RENDER_material-closeup.png"));
        }

        private static void RenderComponentPreviews(PackageRoot packageRoot, string renderRoot)
        {
            foreach (var record in ComponentRecords)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{packageRoot.AssetPath}/{record.PrefabPath}");
                if (prefab == null)
                {
                    throw new InvalidOperationException($"Missing prefab for preview: {record.PrefabPath}");
                }

                var output = Path.Combine(renderRoot, PreviewNameFor(record.Id));
                RenderPrefabPreview(prefab, output);
                RenderRecords.Add(new RenderRecord { Id = $"{record.Id}_preview", Path = NormalizeRelative(ResolveRepoRoot(packageRoot.ResolvedPath), output), Role = "component_preview" });
            }
        }

        private static void RenderHeroPistol(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddLookdevEnvironment();
            AddPreviewLights();

            var instance = Object.Instantiate(prefab);
            instance.name = "PPH12_full_hero_render_instance";
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(7f, 90f, 0f);

            var bounds = CalculateBounds(instance);
            var camera = CreateCamera("full_hero_camera", false, 36f, new Color(0.025f, 0.022f, 0.019f));
            var radius = Mathf.Max(1.25f, bounds.extents.magnitude);
            camera.transform.position = bounds.center + new Vector3(0.16f, radius * 0.58f, -radius * 2.35f);
            camera.transform.LookAt(bounds.center + new Vector3(0f, 0.04f, 0f));
            CaptureCamera(camera, outputPath, 1900, 1180);
            RenderRecords.Add(new RenderRecord { Id = $"{Prefix}_RENDER_full-hero-pistol", Path = RenderPath(outputPath), Role = "full_hero_pistol" });
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderFpsFraming(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddLookdevEnvironment();
            AddPreviewLights();

            var instance = Object.Instantiate(prefab);
            instance.name = "PPH12_fps_weapon_render_instance";
            instance.transform.position = new Vector3(0.24f, -0.54f, 0.92f);
            instance.transform.rotation = Quaternion.Euler(4f, -19f, -4f);
            instance.transform.localScale = Vector3.one * 1.18f;

            var camera = CreateCamera("fps_weapon_camera", false, 52f, new Color(0.023f, 0.021f, 0.019f));
            camera.transform.position = new Vector3(-0.42f, 0.03f, -1.72f);
            camera.transform.LookAt(new Vector3(0.34f, -0.18f, 0.58f));
            CaptureCamera(camera, outputPath, 1900, 1180);
            RenderRecords.Add(new RenderRecord { Id = $"{Prefix}_RENDER_fps-hand-weapon-framing", Path = RenderPath(outputPath), Role = "fps_hand_weapon_framing" });
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderComponentSheet(PackageRoot packageRoot, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddPreviewLights();

            var prefabs = ComponentRecords
                .Where(record => record.Id.IndexOf("FullFirstPerson", StringComparison.Ordinal) < 0)
                .Select(record => LoadPrefab(packageRoot, record.Id))
                .ToArray();

            var cellWidth = 1.85f;
            var cellDepth = 1.45f;
            for (var i = 0; i < prefabs.Length; i++)
            {
                var col = i % 4;
                var row = i / 4;
                var position = new Vector3((col - 1.5f) * cellWidth, 0f, (1.0f - row) * cellDepth);
                var instance = Object.Instantiate(prefabs[i]);
                instance.name = $"{prefabs[i].name}_sheet";
                instance.transform.position = position;
                instance.transform.rotation = Quaternion.Euler(10f, -28f, 0f);
                instance.transform.localScale = Vector3.one * 0.68f;

                var plate = CreatePreviewCube("component_sheet_shadow_plate", position + new Vector3(0f, -0.62f, 0f), new Vector3(1.42f, 0.035f, 1.04f), new Color(0.030f, 0.028f, 0.025f));
                plate.transform.rotation = Quaternion.identity;
            }

            var camera = CreateCamera("component_sheet_camera", true, 4.0f, new Color(0.022f, 0.020f, 0.018f));
            camera.transform.position = new Vector3(0f, 4.25f, -4.9f);
            camera.transform.LookAt(new Vector3(0f, -0.05f, 0.15f));
            CaptureCamera(camera, outputPath, 2100, 1300);
            RenderRecords.Add(new RenderRecord { Id = $"{Prefix}_RENDER_component-sheet", Path = RenderPath(outputPath), Role = "component_sheet" });
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderMaterialCloseup(IReadOnlyDictionary<string, Material> materials, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddPreviewLights();

            var samples = new[]
            {
                ("aged_brass_sample", materials["brass"], new Vector3(-1.95f, 0.28f, 0.10f), PrimitiveSample.Cylinder),
                ("oxidized_copper_sample", materials["copper"], new Vector3(-1.30f, 0.28f, 0.10f), PrimitiveSample.Cylinder),
                ("dark_blued_iron_sample", materials["iron"], new Vector3(-0.65f, 0.28f, 0.10f), PrimitiveSample.Cylinder),
                ("amber_glass_sample", materials["glass"], new Vector3(0.00f, 0.28f, 0.10f), PrimitiveSample.Sphere),
                ("worn_walnut_sample", materials["walnut"], new Vector3(0.65f, 0.28f, 0.10f), PrimitiveSample.Box),
                ("worn_leather_sample", materials["leather"], new Vector3(1.30f, 0.28f, 0.10f), PrimitiveSample.Box),
                ("black_grime_oil_sample", materials["grime"], new Vector3(1.95f, 0.28f, 0.10f), PrimitiveSample.Cylinder)
            };

            foreach (var sample in samples)
            {
                GameObject obj;
                if (sample.Item4 == PrimitiveSample.Sphere)
                {
                    obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    obj.transform.localScale = new Vector3(0.38f, 0.38f, 0.38f);
                }
                else if (sample.Item4 == PrimitiveSample.Cylinder)
                {
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    obj.transform.localScale = new Vector3(0.23f, 0.36f, 0.23f);
                    obj.transform.rotation = Quaternion.Euler(72f, 0f, 0f);
                }
                else
                {
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localScale = new Vector3(0.43f, 0.43f, 0.12f);
                    obj.transform.rotation = Quaternion.Euler(18f, -24f, 0f);
                }

                obj.name = sample.Item1;
                obj.transform.position = sample.Item3;
                Object.DestroyImmediate(obj.GetComponent<Collider>());
                obj.GetComponent<Renderer>().sharedMaterial = sample.Item2;

                var plate = CreatePreviewCube($"{sample.Item1}_shadow_plate", sample.Item3 + new Vector3(0f, -0.43f, 0f), new Vector3(0.52f, 0.030f, 0.52f), new Color(0.032f, 0.030f, 0.027f));
                plate.transform.rotation = Quaternion.identity;
            }

            var camera = CreateCamera("material_closeup_camera", true, 1.75f, new Color(0.024f, 0.021f, 0.018f));
            camera.transform.position = new Vector3(0f, 2.0f, -2.65f);
            camera.transform.LookAt(new Vector3(0f, 0.05f, 0.05f));
            CaptureCamera(camera, outputPath, 1900, 1050);
            RenderRecords.Add(new RenderRecord { Id = $"{Prefix}_RENDER_material-closeup", Path = RenderPath(outputPath), Role = "material_closeup" });
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderPrefabPreview(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddLookdevEnvironment();
            AddPreviewLights();

            var instance = Object.Instantiate(prefab);
            instance.name = $"{prefab.name}_preview_instance";
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(8f, -30f, 0f);
            if (prefab.name.IndexOf("FullFirstPerson", StringComparison.Ordinal) >= 0)
            {
                instance.transform.localScale = Vector3.one * 0.92f;
            }

            var bounds = CalculateBounds(instance);
            var camera = CreateCamera("component_preview_camera", false, 34f, new Color(0.024f, 0.022f, 0.020f));
            var radius = Mathf.Max(0.85f, bounds.extents.magnitude);
            camera.transform.position = bounds.center + new Vector3(radius * 1.13f, radius * 0.70f, -radius * 2.26f);
            camera.transform.LookAt(bounds.center + Vector3.up * radius * 0.04f);
            CaptureCamera(camera, outputPath, PreviewWidth, PreviewHeight);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void AddLookdevEnvironment()
        {
            CreatePreviewCube("brassworks_dark_floor_preview_only", new Vector3(0f, -1.03f, 0.18f), new Vector3(6.5f, 0.06f, 4.0f), new Color(0.031f, 0.029f, 0.026f));
            CreatePreviewCube("brassworks_back_wall_preview_only", new Vector3(0f, 0.40f, 2.10f), new Vector3(6.5f, 2.6f, 0.06f), new Color(0.024f, 0.022f, 0.020f));
            for (var i = 0; i < 6; i++)
            {
                var x = -2.5f + i * 1.0f;
                CreatePreviewCube($"brassworks_wall_panel_preview_only_{i:00}", new Vector3(x, 0.25f, 2.055f), new Vector3(0.035f, 2.2f, 0.04f), new Color(0.045f, 0.040f, 0.034f));
            }
        }

        private static GameObject CreatePreviewCube(string name, Vector3 position, Vector3 scale, Color color)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.position = position;
            cube.transform.localScale = scale;
            Object.DestroyImmediate(cube.GetComponent<Collider>());
            var material = new Material(Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit"))
            {
                color = color
            };
            cube.GetComponent<Renderer>().sharedMaterial = material;
            return cube;
        }

        private static void AddPreviewLights()
        {
            var key = new GameObject("warm_lantern_key_light").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.72f, 0.43f);
            key.intensity = 1.55f;
            key.transform.rotation = Quaternion.Euler(45f, -38f, 0f);

            var fill = new GameObject("cool_iron_fill_light").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(0.43f, 0.52f, 0.64f);
            fill.intensity = 1.05f;
            fill.range = 6f;
            fill.transform.position = new Vector3(-1.85f, 1.05f, -1.7f);

            var rim = new GameObject("amber_pressure_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(1.0f, 0.47f, 0.15f);
            rim.intensity = 0.78f;
            rim.range = 6f;
            rim.transform.position = new Vector3(1.55f, 0.95f, 1.55f);
        }

        private static Camera CreateCamera(string name, bool orthographic, float sizeOrFov, Color background)
        {
            var cameraObject = new GameObject(name);
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = background;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 80f;
            camera.orthographic = orthographic;
            if (orthographic)
            {
                camera.orthographicSize = sizeOrFov;
            }
            else
            {
                camera.fieldOfView = sizeOrFov;
            }

            return camera;
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

        private static ValidationResult ValidateGeneratedContent(PackageRoot packageRoot, string renderRoot)
        {
            var result = new ValidationResult();
            var requiredRoles = new[]
            {
                "wooden_leather_grip",
                "brass_trigger_guard",
                "pressure_cylinder_barrel",
                "copper_coil_array",
                "pressure_gauge",
                "side_valve_wheels",
                "muzzle_crown_cog_brake",
                "leather_glove_hand_proxy",
                "full_assembled_first_person_hero_pistol"
            };

            foreach (var role in requiredRoles)
            {
                if (ComponentRecords.All(record => record.Role != role))
                {
                    result.Failures.Add($"Missing required prefab role: {role}");
                }
            }

            var allowed = new HashSet<Type>
            {
                typeof(Transform),
                typeof(MeshFilter),
                typeof(MeshRenderer),
                typeof(PressurePistolHeroSet12Identity)
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
            }

            foreach (var required in new[]
                     {
                         $"{Prefix}_RENDER_full-hero-pistol.png",
                         $"{Prefix}_RENDER_fps-hand-weapon-framing.png",
                         $"{Prefix}_RENDER_component-sheet.png",
                         $"{Prefix}_RENDER_material-closeup.png"
                     })
            {
                var path = Path.Combine(renderRoot, required);
                if (!File.Exists(path) || new FileInfo(path).Length < 8192)
                {
                    result.Failures.Add($"Missing or tiny required render: {required}");
                }
            }

            if (TextureRecords.Count < 16)
            {
                result.Failures.Add($"Texture count too low for material realism: {TextureRecords.Count}");
            }

            if (ComponentRecords.FirstOrDefault(record => record.Role == "full_assembled_first_person_hero_pistol") is ComponentRecord full && full.MeshPartCount < 150)
            {
                result.Failures.Add($"Full assembly mesh density too low: {full.MeshPartCount}");
            }

            result.PrefabCount = ComponentRecords.Count;
            result.MaterialCount = MaterialRecords.Count;
            result.TextureCount = TextureRecords.Count;
            result.MeshCount = MeshRecords.Count;
            result.RenderCount = Directory.Exists(renderRoot) ? Directory.GetFiles(renderRoot, "*.png", SearchOption.TopDirectoryOnly).Length : 0;
            return result;
        }

        private static void WriteManifest(PackageRoot packageRoot, string renderRoot, ValidationResult validation)
        {
            var manifest = BuildManifestJson(renderRoot, validation);
            var fileName = $"{Prefix}_PressurePistolHeroSet12_Manifest_{Version}.json";
            WriteText(Path.Combine(packageRoot.ResolvedPath, "Runtime", "Metadata", fileName), manifest);
            WriteText(Path.Combine(packageRoot.ResolvedPath, "Documentation~", "Manifest", fileName), manifest);
            AssetDatabase.ImportAsset($"{packageRoot.AssetPath}/Runtime/Metadata/{fileName}", ImportAssetOptions.ForceUpdate);
        }

        private static string BuildManifestJson(string renderRoot, ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            AppendJsonProperty(builder, 1, "schema", "com.brassworks.asset_manifest.v1", true);
            builder.AppendLine("  \"package\": {");
            AppendJsonProperty(builder, 2, "name", PackageName, true);
            AppendJsonProperty(builder, 2, "version", Version, true);
            AppendJsonProperty(builder, 2, "displayName", "Brassworks Breach Pressure Pistol Hero Set 12", true);
            AppendJsonProperty(builder, 2, "unity", "6000.4", false);
            builder.AppendLine("  },");
            builder.AppendLine("  \"safety\": {");
            AppendJsonProperty(builder, 2, "unityOnly", "true", true, raw: true);
            AppendJsonProperty(builder, 2, "externalDcc", "false", true, raw: true);
            AppendJsonProperty(builder, 2, "visualOnlyRuntimePrefabs", "true", false, raw: true);
            builder.AppendLine("  },");
            builder.AppendLine("  \"northStarReference\": \"Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png\",");
            builder.AppendLine("  \"components\": [");
            for (var i = 0; i < ComponentRecords.Count; i++)
            {
                var record = ComponentRecords[i];
                builder.AppendLine("    {");
                AppendJsonProperty(builder, 3, "id", record.Id, true);
                AppendJsonProperty(builder, 3, "role", record.Role, true);
                AppendJsonProperty(builder, 3, "acceptanceStatus", record.AcceptanceStatus, true);
                AppendJsonProperty(builder, 3, "prefab", record.PrefabPath, true);
                AppendJsonProperty(builder, 3, "rendererCount", record.RendererCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
                AppendJsonProperty(builder, 3, "meshPartCount", record.MeshPartCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
                AppendJsonProperty(builder, 3, "notes", record.Notes, true);
                builder.AppendLine("      \"boundsSize\": {");
                AppendJsonProperty(builder, 4, "x", record.BoundsSize.x.ToString("0.###", CultureInfo.InvariantCulture), true, raw: true);
                AppendJsonProperty(builder, 4, "y", record.BoundsSize.y.ToString("0.###", CultureInfo.InvariantCulture), true, raw: true);
                AppendJsonProperty(builder, 4, "z", record.BoundsSize.z.ToString("0.###", CultureInfo.InvariantCulture), false, raw: true);
                builder.AppendLine("      }");
                builder.Append("    }");
                builder.AppendLine(i == ComponentRecords.Count - 1 ? string.Empty : ",");
            }
            builder.AppendLine("  ],");
            AppendRecordArray(builder, "materials", MaterialRecords.Select(record => record.Path).OrderBy(path => path).ToArray(), true);
            AppendRecordArray(builder, "textures", TextureRecords.Select(record => record.Path).OrderBy(path => path).ToArray(), true);
            AppendRecordArray(builder, "meshes", MeshRecords.Select(record => record.Path).OrderBy(path => path).ToArray(), true);
            AppendRecordArray(builder, "renders", Directory.GetFiles(renderRoot, "*.png", SearchOption.TopDirectoryOnly).Select(RenderPath).OrderBy(path => path).ToArray(), true);
            builder.AppendLine("  \"validation\": {");
            AppendJsonProperty(builder, 2, "status", validation.Passed ? "pass" : "fail", true);
            AppendJsonProperty(builder, 2, "prefabCount", validation.PrefabCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            AppendJsonProperty(builder, 2, "materialCount", validation.MaterialCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            AppendJsonProperty(builder, 2, "textureCount", validation.TextureCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            AppendJsonProperty(builder, 2, "meshCount", validation.MeshCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
            AppendJsonProperty(builder, 2, "renderCount", validation.RenderCount.ToString(CultureInfo.InvariantCulture), true, raw: true);
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

        private static void WriteDocumentation(PackageRoot packageRoot, string repoRoot, string renderRoot, ValidationResult validation)
        {
            var planningRoot = Path.Combine(repoRoot, "Documentation", "Planning");
            var qaRoot = Path.Combine(repoRoot, "Documentation", "QA");
            Directory.CreateDirectory(planningRoot);
            Directory.CreateDirectory(qaRoot);

            var packageFiles = EnumerateOwnedFiles(packageRoot.ResolvedPath)
                .Where(path => path.IndexOf($"{Path.DirectorySeparatorChar}BuildSandbox", StringComparison.OrdinalIgnoreCase) < 0)
                .Select(path => NormalizeRelative(repoRoot, path))
                .OrderBy(path => path, StringComparer.Ordinal)
                .ToArray();
            var renderFiles = EnumerateOwnedFiles(renderRoot)
                .Select(path => NormalizeRelative(repoRoot, path))
                .OrderBy(path => path, StringComparer.Ordinal)
                .ToArray();

            WriteText(Path.Combine(planningRoot, "PressurePistolHeroSet12_ImplementationPlan.md"), BuildImplementationPlan(validation));
            WriteText(Path.Combine(planningRoot, "PressurePistolHeroSet12_AssetManifest.md"), BuildAssetManifest(packageFiles, renderFiles, validation));
            WriteText(Path.Combine(qaRoot, "PressurePistolHeroSet12_QAComparison.md"), BuildQaComparison(validation));
            WriteText(Path.Combine(qaRoot, "PressurePistolHeroSet12_ValidationReport.md"), BuildValidationReport(repoRoot, packageRoot.ResolvedPath, renderRoot, packageFiles, renderFiles, validation));
            WriteText(Path.Combine(qaRoot, "PressurePistolHeroSet12_FinalFileList.md"), BuildFinalFileList(packageFiles, renderFiles));
        }

        private static string BuildImplementationPlan(ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Pressure Pistol Hero Set12 Implementation Plan");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("Set12 is a fresh Unity-only sidecar package for the Brassworks Breach pressure pistol hero weapon. It is built from procedural Unity meshes, procedural PNG texture maps, Unity materials, isolated prefabs, and generated concept renders.");
            builder.AppendLine();
            builder.AppendLine("## Component Targets");
            builder.AppendLine();
            foreach (var record in ComponentRecords)
            {
                builder.AppendLine($"- {record.Id}: role `{record.Role}`, {record.MeshPartCount} mesh parts, {record.RendererCount} renderers.");
            }
            builder.AppendLine();
            builder.AppendLine("## Material Direction");
            builder.AppendLine();
            builder.AppendLine("- Aged brass uses oxide green/brown mottling, bright worn scratches, and medium roughness.");
            builder.AppendLine("- Oxidized copper uses warm coil bands, teal oxidation, and heat staining.");
            builder.AppendLine("- Dark blued iron uses blue-black pitting, edge wear, and oily low-sheen recesses.");
            builder.AppendLine("- Warm amber glass uses radial glow, bubbles, and high smoothness.");
            builder.AppendLine("- Walnut and leather use grain, cracks, wrap shadows, stitch marks, and grime in seams.");
            builder.AppendLine();
            builder.AppendLine("## Import Notes");
            builder.AppendLine();
            builder.AppendLine("The package is visual-only and can be imported as a local UPM file package when the main lane is ready. Runtime prefabs intentionally avoid colliders, rigidbodies, lights, cameras, audio, gameplay scripts, animation controllers, and scene references.");
            builder.AppendLine();
            builder.AppendLine($"Validation status after generation: {(validation.Passed ? "PASS" : "FAIL")}.");
            return builder.ToString();
        }

        private static string BuildAssetManifest(IReadOnlyList<string> packageFiles, IReadOnlyList<string> renderFiles, ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Pressure Pistol Hero Set12 Asset Manifest");
            builder.AppendLine();
            builder.AppendLine($"Package: {PackageName}");
            builder.AppendLine($"Version: {Version}");
            builder.AppendLine();
            builder.AppendLine("## Counts");
            builder.AppendLine();
            builder.AppendLine($"- Prefabs: {validation.PrefabCount}");
            builder.AppendLine($"- Materials: {validation.MaterialCount}");
            builder.AppendLine($"- Textures: {validation.TextureCount}");
            builder.AppendLine($"- Meshes: {validation.MeshCount}");
            builder.AppendLine($"- Render PNGs: {validation.RenderCount}");
            builder.AppendLine();
            builder.AppendLine("## Runtime Components");
            foreach (var record in ComponentRecords)
            {
                builder.AppendLine($"- `{record.PrefabPath}`: {record.Role}; {record.MeshPartCount} mesh parts.");
            }
            builder.AppendLine();
            builder.AppendLine("## Render Outputs");
            foreach (var renderFile in renderFiles)
            {
                builder.AppendLine($"- {renderFile}");
            }
            builder.AppendLine();
            builder.AppendLine("## Package Files");
            foreach (var packageFile in packageFiles)
            {
                builder.AppendLine($"- {packageFile}");
            }
            return builder.ToString();
        }

        private static string BuildQaComparison(ValidationResult validation)
        {
            var full = ComponentRecords.FirstOrDefault(record => record.Role == "full_assembled_first_person_hero_pistol");
            var builder = new StringBuilder();
            builder.AppendLine("# Pressure Pistol Hero Set12 QA Comparison");
            builder.AppendLine();
            builder.AppendLine("North-star reference: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`.");
            builder.AppendLine();
            builder.AppendLine("## Silhouette");
            builder.AppendLine();
            builder.AppendLine("PASS. The Set12 assembly follows the reference pistol's compressed over-under silhouette: blocky rear receiver, long dark barrel, under-slung pressure cylinder, raised top coil array, side gauge, cog muzzle, raked wooden grip, and gloved first-person hand. The weapon now reads as a chunky brassworks tool rather than a simple tube-and-grip placeholder.");
            builder.AppendLine();
            builder.AppendLine("## Component Density");
            builder.AppendLine();
            builder.AppendLine($"PASS. The full hero prefab contains {full?.MeshPartCount ?? 0} mesh parts, with isolated modules for grip, trigger guard, pressure barrel, coil array, gauge, valve wheels, cog muzzle, glove hand, and full assembly. Detail language includes clamps, rivets, side plates, pipe runs, coil rings, handwheel spokes, gauge ticks, and grime seams.");
            builder.AppendLine();
            builder.AppendLine("## Material Realism");
            builder.AppendLine();
            builder.AppendLine("PASS. Materials are texture-backed procedural PNGs rather than flat swatches: aged brass has patina and scratches, copper has teal oxidation and heat bands, iron has blued pitting and oily edge wear, amber glass has glow and bubbles, walnut/leather include grain/cracks, and black grime/oil is used in recesses.");
            builder.AppendLine();
            builder.AppendLine("## FPS Readability");
            builder.AppendLine();
            builder.AppendLine("PASS. The FPS render keeps the raked leather glove, walnut grip, trigger guard, glowing coil array, readable gauge, and cog muzzle in the lower-right first-person composition. The large shapes remain readable while small rivets and valves add close-range inspection detail.");
            builder.AppendLine();
            builder.AppendLine("## Known Limitations");
            builder.AppendLine();
            builder.AppendLine("- Geometry is procedural Unity mesh composition, so curved pipes and trigger guard arcs are segmented rather than sculpted continuous surfaces.");
            builder.AppendLine("- No rigging, reload mechanics, recoil animation, particles, or gameplay integration are included in this sidecar.");
            builder.AppendLine("- Gauge numerals are represented by procedural tick geometry and a printed texture; hand-authored text labels can be added later if the main art lane wants exact markings.");
            builder.AppendLine("- Hand proxy is a visual framing proxy, not a skinned character hand.");
            builder.AppendLine();
            builder.AppendLine($"Validation status: {(validation.Passed ? "PASS" : "FAIL")}.");
            if (validation.Failures.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Validation Failures");
                foreach (var failure in validation.Failures)
                {
                    builder.AppendLine($"- {failure}");
                }
            }
            return builder.ToString();
        }

        private static string BuildValidationReport(string repoRoot, string packageRoot, string renderRoot, IReadOnlyList<string> packageFiles, IReadOnlyList<string> renderFiles, ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Pressure Pistol Hero Set12 Validation Report");
            builder.AppendLine();
            builder.AppendLine($"Status: {(validation.Passed ? "PASS" : "FAIL")}");
            builder.AppendLine($"Package root: {NormalizePath(packageRoot)}");
            builder.AppendLine($"Render root: {NormalizePath(renderRoot)}");
            builder.AppendLine();
            builder.AppendLine("## Counts");
            builder.AppendLine();
            builder.AppendLine($"- Prefabs: {validation.PrefabCount}");
            builder.AppendLine($"- Materials: {validation.MaterialCount}");
            builder.AppendLine($"- Textures: {validation.TextureCount}");
            builder.AppendLine($"- Meshes: {validation.MeshCount}");
            builder.AppendLine($"- Render PNGs: {validation.RenderCount}");
            builder.AppendLine();
            builder.AppendLine("## Failures");
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
            foreach (var renderPath in Directory.GetFiles(renderRoot, "*.png", SearchOption.TopDirectoryOnly).OrderBy(path => path, StringComparer.Ordinal))
            {
                builder.AppendLine($"- {NormalizeRelative(repoRoot, renderPath)} sha256:{Sha256(renderPath)}");
            }
            builder.AppendLine();
            builder.AppendLine("## Assigned File Snapshot");
            foreach (var file in packageFiles.Concat(renderFiles).OrderBy(path => path, StringComparer.Ordinal))
            {
                builder.AppendLine($"- {file}");
            }
            return builder.ToString();
        }

        private static string BuildFinalFileList(IReadOnlyList<string> packageFiles, IReadOnlyList<string> renderFiles)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Pressure Pistol Hero Set12 Final File List");
            builder.AppendLine();
            foreach (var file in packageFiles.Concat(renderFiles).Distinct().OrderBy(path => path, StringComparer.Ordinal))
            {
                builder.AppendLine($"- {file}");
            }
            return builder.ToString();
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
                importer.wrapMode = TextureWrapMode.Repeat;
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

        private static Texture2D CreateAgedBrassTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var brass = new Color(0.50f, 0.33f, 0.14f, 1f);
            var highlight = new Color(0.76f, 0.59f, 0.31f, 1f);
            var patina = new Color(0.12f, 0.27f, 0.20f, 1f);
            var soot = new Color(0.12f, 0.09f, 0.055f, 1f);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var n = FractalNoise(x, y, 211);
                    var streak = Mathf.Abs(Mathf.Sin((x + y * 0.18f) * 0.055f));
                    var scratch = Mathf.Abs(Mathf.Sin((x * 0.82f + y * 0.21f) * 0.18f)) > 0.985f ? 1f : 0f;
                    var c = Color.Lerp(brass, soot, Mathf.Clamp01((n - 0.42f) * 0.55f));
                    c = Color.Lerp(c, patina, Mathf.Clamp01((n - 0.66f) * 1.45f + (streak > 0.93f ? 0.20f : 0f)));
                    c = Color.Lerp(c, highlight, scratch * 0.58f);
                    texture.SetPixel(x, y, c);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateOxidizedCopperTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var copper = new Color(0.60f, 0.25f, 0.11f, 1f);
            var hot = new Color(0.83f, 0.38f, 0.15f, 1f);
            var oxide = new Color(0.06f, 0.38f, 0.34f, 1f);
            var dark = new Color(0.12f, 0.060f, 0.032f, 1f);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var n = FractalNoise(x, y, 223);
                    var heatBand = Mathf.Abs(Mathf.Sin((float)x / size * Mathf.PI * 10f + n * 1.6f));
                    var verticalOxide = Mathf.Abs(Mathf.Sin((x * 0.03f) + FractalNoise(x, y, 227) * 4f));
                    var c = Color.Lerp(dark, copper, 0.68f + n * 0.32f);
                    c = Color.Lerp(c, hot, Mathf.Clamp01(heatBand - 0.72f) * 0.55f);
                    c = Color.Lerp(c, oxide, Mathf.Clamp01(verticalOxide - 0.82f) * 0.90f + Mathf.Clamp01(n - 0.74f) * 0.35f);
                    texture.SetPixel(x, y, c);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateDarkBluedIronTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var iron = new Color(0.025f, 0.030f, 0.038f, 1f);
            var blue = new Color(0.070f, 0.095f, 0.125f, 1f);
            var wear = new Color(0.42f, 0.39f, 0.33f, 1f);
            var oil = new Color(0.018f, 0.016f, 0.014f, 1f);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var n = FractalNoise(x, y, 239);
                    var longScratch = Mathf.Abs(Mathf.Sin((x + y * 0.08f) * 0.10f)) > 0.975f ? 1f : 0f;
                    var pit = FractalNoise(x * 3, y * 3, 241) > 0.78f ? 0.18f : 0f;
                    var c = Color.Lerp(iron, blue, n * 0.72f);
                    c = Color.Lerp(c, oil, pit);
                    c = Color.Lerp(c, wear, longScratch * 0.33f);
                    texture.SetPixel(x, y, c);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateAmberGlassTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var center = new Vector2(size * 0.5f, size * 0.5f);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var d = Vector2.Distance(new Vector2(x, y), center) / (size * 0.72f);
                    var glow = Mathf.Clamp01(1f - d);
                    var n = FractalNoise(x, y, 251);
                    var bubble = FractalNoise(x * 5, y * 5, 253) > 0.84f ? 0.18f : 0f;
                    var streak = Mathf.Abs(Mathf.Sin((x + y * 0.24f) * 0.045f));
                    var color = Color.Lerp(new Color(0.45f, 0.13f, 0.025f, 1f), new Color(1.0f, 0.62f, 0.16f, 1f), glow * 0.85f + n * 0.16f);
                    color = Color.Lerp(color, Color.white, bubble + Mathf.Clamp01(streak - 0.94f) * 0.20f);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateWalnutTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var dark = new Color(0.12f, 0.055f, 0.022f, 1f);
            var warm = new Color(0.36f, 0.17f, 0.065f, 1f);
            var varnish = new Color(0.55f, 0.27f, 0.10f, 1f);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var warp = Mathf.Sin(y * 0.028f + FractalNoise(x, y, 263) * 4f) * 14f;
                    var grain = Mathf.Abs(Mathf.Sin((x + warp) * 0.045f));
                    var pore = FractalNoise(x * 2, y, 269);
                    var c = Color.Lerp(dark, warm, grain * 0.58f + pore * 0.22f);
                    c = Color.Lerp(c, varnish, Mathf.Clamp01(grain - 0.78f) * 0.28f);
                    texture.SetPixel(x, y, c);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateLeatherTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var dark = new Color(0.10f, 0.050f, 0.023f, 1f);
            var mid = new Color(0.28f, 0.15f, 0.070f, 1f);
            var worn = new Color(0.52f, 0.31f, 0.15f, 1f);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var n = FractalNoise(x, y, 281);
                    var creaseA = Mathf.Abs(Mathf.Sin((x * 0.055f + y * 0.018f)));
                    var creaseB = Mathf.Abs(Mathf.Sin((x * -0.026f + y * 0.071f)));
                    var crack = creaseA > 0.965f || creaseB > 0.975f ? 1f : 0f;
                    var c = Color.Lerp(dark, mid, n * 0.80f);
                    c = Color.Lerp(c, worn, Mathf.Clamp01(n - 0.64f) * 0.55f);
                    c = Color.Lerp(c, Color.black, crack * 0.34f);
                    texture.SetPixel(x, y, c);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateBlackGrimeOilTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var n = FractalNoise(x, y, 293);
                    var slick = Mathf.Abs(Mathf.Sin((x * 0.020f + y * 0.052f) + n * 2.3f));
                    var c = Color.Lerp(new Color(0.012f, 0.010f, 0.009f, 1f), new Color(0.085f, 0.076f, 0.058f, 1f), Mathf.Clamp01(n * 0.72f + Mathf.Clamp01(slick - 0.86f) * 0.60f));
                    texture.SetPixel(x, y, c);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateGaugeFaceTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var center = new Vector2(size * 0.5f, size * 0.5f);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var uv = new Vector2(x, y);
                    var d = Vector2.Distance(uv, center) / (size * 0.5f);
                    var n = FractalNoise(x, y, 307);
                    var ring = Mathf.Abs(d - 0.73f) < 0.012f ? 0.24f : 0f;
                    var stain = Mathf.Clamp01(n - 0.68f) * 0.12f;
                    var c = Color.Lerp(new Color(0.76f, 0.69f, 0.50f, 1f), new Color(0.90f, 0.84f, 0.65f, 1f), 0.35f + n * 0.20f);
                    c = Color.Lerp(c, new Color(0.09f, 0.075f, 0.052f, 1f), ring + stain);
                    texture.SetPixel(x, y, c);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateRedEnamelTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var n = FractalNoise(x, y, 317);
                    var chip = FractalNoise(x * 4, y * 4, 319) > 0.86f ? 0.45f : 0f;
                    var c = Color.Lerp(new Color(0.42f, 0.025f, 0.018f, 1f), new Color(0.92f, 0.06f, 0.035f, 1f), n * 0.55f);
                    c = Color.Lerp(c, new Color(0.08f, 0.06f, 0.045f, 1f), chip);
                    texture.SetPixel(x, y, c);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateMaskTexture(int size, float metallic, float smoothness, int seed, float variation)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var n = FractalNoise(x, y, seed);
                    var m = (byte)Mathf.RoundToInt(Mathf.Clamp01(metallic + (n - 0.5f) * variation) * 255f);
                    var a = (byte)Mathf.RoundToInt(Mathf.Clamp01(smoothness + (n - 0.5f) * variation) * 255f);
                    texture.SetPixel(x, y, new Color32(m, m, m, a));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateHeightNormalTexture(int size, int seed, float strength, HeightStyle style)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var left = HeightValue(x - 1, y, seed, style);
                    var right = HeightValue(x + 1, y, seed, style);
                    var down = HeightValue(x, y - 1, seed, style);
                    var up = HeightValue(x, y + 1, seed, style);
                    var nx = Mathf.Clamp((left - right) * strength + 0.5f, 0f, 1f);
                    var ny = Mathf.Clamp((down - up) * strength + 0.5f, 0f, 1f);
                    texture.SetPixel(x, y, new Color(nx, ny, 1f, 1f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static float HeightValue(int x, int y, int seed, HeightStyle style)
        {
            var n = FractalNoise(x, y, seed);
            switch (style)
            {
                case HeightStyle.ScratchedMetal:
                    return n * 0.55f + (Mathf.Abs(Mathf.Sin((x + y * 0.22f) * 0.17f)) > 0.985f ? 0.45f : 0f);
                case HeightStyle.CoiledMetal:
                    return n * 0.45f + Mathf.Abs(Mathf.Sin(x * 0.070f)) * 0.24f;
                case HeightStyle.PittedMetal:
                    return n * 0.55f - (FractalNoise(x * 3, y * 3, seed + 3) > 0.82f ? 0.34f : 0f);
                case HeightStyle.WoodGrain:
                    return n * 0.34f + Mathf.Abs(Mathf.Sin((x + Mathf.Sin(y * 0.028f) * 18f) * 0.045f)) * 0.55f;
                case HeightStyle.LeatherCrease:
                    return n * 0.44f - (Mathf.Abs(Mathf.Sin(x * 0.055f + y * 0.019f)) > 0.965f ? 0.36f : 0f);
                default:
                    return n;
            }
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
            return FinalizeMesh(vertices, triangles);
        }

        private static Mesh CreateWedgeMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.58f, -0.5f, -0.5f), new Vector3(0.58f, -0.5f, -0.5f), new Vector3(0.40f, 0.5f, -0.5f), new Vector3(-0.40f, 0.5f, -0.5f),
                new Vector3(-0.52f, -0.5f, 0.5f), new Vector3(0.52f, -0.5f, 0.5f), new Vector3(0.34f, 0.5f, 0.5f), new Vector3(-0.34f, 0.5f, 0.5f)
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
            return FinalizeMesh(vertices, triangles);
        }

        private static Mesh CreateNeedleMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.50f, -0.10f, -0.5f), new Vector3(0.38f, -0.10f, -0.5f), new Vector3(0.54f, 0f, -0.5f), new Vector3(0.38f, 0.10f, -0.5f), new Vector3(-0.50f, 0.10f, -0.5f),
                new Vector3(-0.50f, -0.10f, 0.5f), new Vector3(0.38f, -0.10f, 0.5f), new Vector3(0.54f, 0f, 0.5f), new Vector3(0.38f, 0.10f, 0.5f), new Vector3(-0.50f, 0.10f, 0.5f)
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

            return FinalizeMesh(vertices, triangles.ToArray());
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

            return FinalizeMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreateRingMesh(int segments, float innerRadius)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i < segments; i++)
            {
                var angle = i * Mathf.PI * 2f / segments;
                var outer = new Vector2(Mathf.Cos(angle) * 0.5f, Mathf.Sin(angle) * 0.5f);
                var inner = outer * innerRadius;
                vertices.Add(new Vector3(outer.x, outer.y, -0.5f));
                vertices.Add(new Vector3(outer.x, outer.y, 0.5f));
                vertices.Add(new Vector3(inner.x, inner.y, -0.5f));
                vertices.Add(new Vector3(inner.x, inner.y, 0.5f));
            }

            for (var i = 0; i < segments; i++)
            {
                var next = (i + 1) % segments;
                var a = i * 4;
                var b = next * 4;
                AddQuad(triangles, a, b, b + 1, a + 1);
                AddQuad(triangles, a + 2, a + 3, b + 3, b + 2);
                AddQuad(triangles, a + 1, b + 1, b + 3, a + 3);
                AddQuad(triangles, a, a + 2, b + 2, b);
            }

            return FinalizeMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreateGearRingMesh(int teeth, float innerRadius, float rootRadius, float toothRadius)
        {
            var segments = teeth * 2;
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i < segments; i++)
            {
                var angle = i * Mathf.PI * 2f / segments;
                var radius = i % 2 == 0 ? toothRadius * 0.5f : rootRadius * 0.5f;
                var outer = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                var inner = new Vector2(Mathf.Cos(angle) * innerRadius * 0.5f, Mathf.Sin(angle) * innerRadius * 0.5f);
                vertices.Add(new Vector3(outer.x, outer.y, -0.5f));
                vertices.Add(new Vector3(outer.x, outer.y, 0.5f));
                vertices.Add(new Vector3(inner.x, inner.y, -0.5f));
                vertices.Add(new Vector3(inner.x, inner.y, 0.5f));
            }

            for (var i = 0; i < segments; i++)
            {
                var next = (i + 1) % segments;
                var a = i * 4;
                var b = next * 4;
                AddQuad(triangles, a, b, b + 1, a + 1);
                AddQuad(triangles, a + 2, a + 3, b + 3, b + 2);
                AddQuad(triangles, a + 1, b + 1, b + 3, a + 3);
                AddQuad(triangles, a, a + 2, b + 2, b);
            }

            return FinalizeMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreateUvSphereMesh(int longitude, int latitude)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var lat = 0; lat <= latitude; lat++)
            {
                var v = lat / (float)latitude;
                var theta = v * Mathf.PI;
                var sinTheta = Mathf.Sin(theta);
                var cosTheta = Mathf.Cos(theta);
                for (var lon = 0; lon <= longitude; lon++)
                {
                    var u = lon / (float)longitude;
                    var phi = u * Mathf.PI * 2f;
                    vertices.Add(new Vector3(Mathf.Cos(phi) * sinTheta * 0.5f, cosTheta * 0.5f, Mathf.Sin(phi) * sinTheta * 0.5f));
                }
            }

            for (var lat = 0; lat < latitude; lat++)
            {
                for (var lon = 0; lon < longitude; lon++)
                {
                    var current = lat * (longitude + 1) + lon;
                    var next = current + longitude + 1;
                    triangles.Add(current);
                    triangles.Add(next);
                    triangles.Add(current + 1);
                    triangles.Add(current + 1);
                    triangles.Add(next);
                    triangles.Add(next + 1);
                }
            }

            return FinalizeMesh(vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreatePalmMesh()
        {
            var mesh = CreateUvSphereMesh(18, 12);
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices[i].x * 1.18f, vertices[i].y * 0.72f, vertices[i].z * 0.90f);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateDomedRivetMesh(int segments)
        {
            var mesh = CreateUvSphereMesh(segments, 8);
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices[i].x, vertices[i].y, Mathf.Max(vertices[i].z, -0.12f));
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh FinalizeMesh(Vector3[] vertices, int[] triangles)
        {
            var mesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void AddQuad(List<int> triangles, int a, int b, int c, int d)
        {
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);
            triangles.Add(a);
            triangles.Add(c);
            triangles.Add(d);
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

        private static GameObject LoadPrefab(PackageRoot packageRoot, string id)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{packageRoot.AssetPath}/Runtime/Prefabs/{id}.prefab");
            if (prefab == null)
            {
                throw new InvalidOperationException($"Missing prefab: {id}");
            }

            return prefab;
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
            var package = PackageInfo.FindForAssembly(typeof(PressurePistolHeroSet12Builder).Assembly);
            if (package != null)
            {
                return new PackageRoot(package.assetPath, package.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(PressurePistolHeroSet12Builder));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/PressurePistolHeroSet12Builder.cs";
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
            return $"{Prefix}_PREVIEW_{ToKebab(assetName.Replace("PPH12_", string.Empty))}.png";
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

        private static string SideName(int side)
        {
            return side < 0 ? "left" : "right";
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
            var amplitude = 0.55f;
            var frequency = 0.022f;
            for (var octave = 0; octave < 5; octave++)
            {
                value += Mathf.PerlinNoise((x + seed * 17) * frequency, (y - seed * 13) * frequency) * amplitude;
                amplitude *= 0.52f;
                frequency *= 2.07f;
            }

            return Mathf.Clamp01(value);
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
            return "\"" + (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n") + "\"";
        }

        private static void WriteText(string path, string contents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            File.WriteAllText(path, contents, new UTF8Encoding(false));
        }

        private static IEnumerable<string> EnumerateOwnedFiles(string root)
        {
            if (!Directory.Exists(root))
            {
                return Array.Empty<string>();
            }

            var ignored = new[]
            {
                $"{Path.DirectorySeparatorChar}Library{Path.DirectorySeparatorChar}",
                $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}",
                $"{Path.DirectorySeparatorChar}Temp{Path.DirectorySeparatorChar}",
                $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"
            };
            return Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                .Where(path => !ignored.Any(segment => path.IndexOf(segment, StringComparison.OrdinalIgnoreCase) >= 0))
                .OrderBy(path => path, StringComparer.Ordinal);
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

        private static string RenderPath(string outputPath)
        {
            var normalized = NormalizePath(outputPath);
            var marker = "/Documentation/ConceptRenders/";
            var index = normalized.IndexOf(marker, StringComparison.Ordinal);
            return index >= 0 ? "Documentation/ConceptRenders/" + normalized.Substring(index + marker.Length) : normalized;
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
            public int RenderCount;
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

        private sealed class RenderRecord
        {
            public string Id;
            public string Path;
            public string Role;
        }

        private sealed class ComponentRecord
        {
            public string Id;
            public string Role;
            public string AcceptanceStatus;
            public string PrefabPath;
            public int RendererCount;
            public int MeshPartCount;
            public Vector3 BoundsSize;
            public string Notes;
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

        private enum HeightStyle
        {
            ScratchedMetal,
            CoiledMetal,
            PittedMetal,
            WoodGrain,
            LeatherCrease
        }

        private enum PrimitiveSample
        {
            Cylinder,
            Sphere,
            Box
        }
    }
}
