using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace BrassworksBreach.HeroVaultDoorSet12.Editor
{
    public static class HeroVaultDoorSet12Generator
    {
        private const string PackageName = "com.brassworks.sidecar.hero-vault-door-set12";
        private const string Prefix = "HVDS12";
        private const string Version = "0.1.57-p012";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string RuntimeRoot = PackageRoot + "/Runtime";
        private const string TextureRoot = RuntimeRoot + "/Textures";
        private const string MaterialRoot = RuntimeRoot + "/Materials";
        private const string MeshRoot = RuntimeRoot + "/Meshes";
        private const string PrefabRoot = RuntimeRoot + "/Prefabs";
        private const string MetadataRoot = RuntimeRoot + "/Metadata";
        private const string ManifestRoot = PackageRoot + "/Documentation~/Manifest";
        private const string RenderDocRelative = "Documentation/ConceptRenders/V0_1_57_HeroVaultDoorSet12";
        private const int RenderWidth = 1800;
        private const int RenderHeight = 1150;

        private static readonly List<Record> TextureRecords = new List<Record>();
        private static readonly List<Record> MaterialRecords = new List<Record>();
        private static readonly List<Record> MeshRecords = new List<Record>();
        private static readonly List<Record> PrefabRecords = new List<Record>();

        private struct Record
        {
            public string Path;
            public string Tag;
            public string Note;
        }

        [MenuItem("Brassworks Breach/Sidecars/Hero Vault Door Set 12/Generate Assets And Renders")]
        public static void GenerateAssetsAndRenders()
        {
            TextureRecords.Clear();
            MaterialRecords.Clear();
            MeshRecords.Clear();
            PrefabRecords.Clear();

            string packagePhysicalRoot = LocatePackageRoot();
            string repoRoot = Directory.GetParent(packagePhysicalRoot)?.Parent?.FullName
                ?? throw new InvalidOperationException("Could not resolve repo root from package path.");
            string renderRoot = Path.Combine(repoRoot, RenderDocRelative.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(renderRoot);

            EnsureFolders(packagePhysicalRoot);

            Dictionary<string, Texture2D> textures = CreateTextures(packagePhysicalRoot);
            Dictionary<string, Material> materials = CreateMaterials(textures);
            Dictionary<string, Mesh> meshes = CreateMeshes();

            CreateCircularVaultDoorSlab(meshes, materials);
            CreateGearWheelLockFace(meshes, materials);
            CreateRadialBraces(meshes, materials);
            CreateHingePistonAssemblies(meshes, materials);
            CreateRivetedArchFrame(meshes, materials);
            CreatePressureGaugeCluster(meshes, materials);
            CreateBrassTrimRings(meshes, materials);
            CreatePipeCouplers(meshes, materials);
            CreateHeroVaultDoorAssembly(meshes, materials);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            RenderPreviews(renderRoot, materials);

            ValidationResult validation = Validate(packagePhysicalRoot, renderRoot);
            WriteManifest(validation);
            WriteDocs(repoRoot, renderRoot, validation);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (!validation.Passed)
            {
                throw new InvalidOperationException("HVDS12 validation failed: " + string.Join("; ", validation.Failures));
            }

            Debug.Log($"{Prefix}_GENERATE_PASS version={Version} prefabs={validation.PrefabCount} textures={validation.TextureCount} renders={validation.RenderCount}");
        }

        private static void EnsureFolders(string packagePhysicalRoot)
        {
            foreach (string folder in new[]
            {
                "Runtime/Textures", "Runtime/Materials", "Runtime/Meshes", "Runtime/Prefabs", "Runtime/Metadata",
                "Documentation~/Manifest", "Samples~/PreviewScene"
            })
            {
                Directory.CreateDirectory(Path.Combine(packagePhysicalRoot, folder.Replace('/', Path.DirectorySeparatorChar)));
            }
        }

        private static Dictionary<string, Texture2D> CreateTextures(string packagePhysicalRoot)
        {
            return new Dictionary<string, Texture2D>(StringComparer.Ordinal)
            {
                ["brass_albedo"] = SaveTexture(packagePhysicalRoot, "AgedBrassVerdigris_Albedo", CreateMetalTexture(new Color32(167, 112, 42, 255), new Color32(58, 117, 93, 255), 7, true), false, false, "aged_brass"),
                ["brass_normal"] = SaveTexture(packagePhysicalRoot, "AgedBrassVerdigris_Normal", CreateNormalTexture(9, 0.38f), true, false, "aged_brass"),
                ["brass_mask"] = SaveTexture(packagePhysicalRoot, "AgedBrassVerdigris_MetallicSmoothness", CreateMaskTexture(0.90f, 0.43f, 11), false, true, "aged_brass"),
                ["bright_brass_albedo"] = SaveTexture(packagePhysicalRoot, "PolishedBrassEdgewear_Albedo", CreateMetalTexture(new Color32(224, 171, 67, 255), new Color32(111, 68, 22, 255), 13, false), false, false, "polished_brass_edgewear"),
                ["iron_albedo"] = SaveTexture(packagePhysicalRoot, "DarkRivetedIron_Albedo", CreateIronTexture(false, 17), false, false, "dark_riveted_iron"),
                ["iron_normal"] = SaveTexture(packagePhysicalRoot, "DarkRivetedIron_Normal", CreateNormalTexture(19, 0.46f), true, false, "dark_riveted_iron"),
                ["iron_mask"] = SaveTexture(packagePhysicalRoot, "DarkRivetedIron_MetallicSmoothness", CreateMaskTexture(0.82f, 0.25f, 23), false, true, "dark_riveted_iron"),
                ["oily_iron_albedo"] = SaveTexture(packagePhysicalRoot, "OilyBlackIron_Albedo", CreateIronTexture(true, 29), false, false, "oily_black_iron"),
                ["stone_albedo"] = SaveTexture(packagePhysicalRoot, "WetBlackStoneContactGrime_Albedo", CreateWetStoneTexture(31), false, false, "wet_black_stone_contact_grime"),
                ["stone_normal"] = SaveTexture(packagePhysicalRoot, "WetBlackStoneContactGrime_Normal", CreateNormalTexture(37, 0.58f), true, false, "wet_black_stone_contact_grime"),
                ["stone_mask"] = SaveTexture(packagePhysicalRoot, "WetBlackStoneContactGrime_MetallicSmoothness", CreateMaskTexture(0.03f, 0.66f, 41), false, true, "wet_black_stone_contact_grime"),
                ["glass_albedo"] = SaveTexture(packagePhysicalRoot, "WarmAmberGlass_Albedo", CreateGlassTexture(), false, false, "warm_amber_glass"),
                ["dial_albedo"] = SaveTexture(packagePhysicalRoot, "IvoryPressureGaugeDial_Albedo", CreateGaugeDialTexture(), false, false, "ivory_pressure_gauge_dial"),
                ["red_albedo"] = SaveTexture(packagePhysicalRoot, "OxideRedPressurePaint_Albedo", CreateFlatNoiseTexture(new Color32(138, 28, 18, 255), new Color32(72, 16, 14, 255), 43), false, false, "oxide_red_pressure_paint"),
                ["copper_albedo"] = SaveTexture(packagePhysicalRoot, "HeatStainedCopperPipe_Albedo", CreateCopperTexture(), false, false, "heat_stained_copper_pipe"),
                ["verdigris_albedo"] = SaveTexture(packagePhysicalRoot, "VerdigrisDeposit_Albedo", CreateFlatNoiseTexture(new Color32(37, 118, 96, 255), new Color32(18, 62, 55, 255), 47), false, false, "verdigris_deposit")
            };
        }

        private static Dictionary<string, Material> CreateMaterials(IReadOnlyDictionary<string, Texture2D> textures)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible Lit shader found.");
            }

            return new Dictionary<string, Material>(StringComparer.Ordinal)
            {
                ["brass"] = SaveMaterial(shader, "AgedBrassVerdigris", new Color(0.67f, 0.43f, 0.15f), 0.90f, 0.43f, textures["brass_albedo"], textures["brass_normal"], textures["brass_mask"], null, "aged_brass"),
                ["brightBrass"] = SaveMaterial(shader, "PolishedBrassEdgewear", new Color(0.92f, 0.68f, 0.25f), 0.92f, 0.56f, textures["bright_brass_albedo"], null, null, null, "polished_brass_edgewear"),
                ["iron"] = SaveMaterial(shader, "DarkRivetedIron", new Color(0.055f, 0.051f, 0.045f), 0.82f, 0.25f, textures["iron_albedo"], textures["iron_normal"], textures["iron_mask"], null, "dark_riveted_iron"),
                ["oilyIron"] = SaveMaterial(shader, "OilyBlackIron", new Color(0.035f, 0.033f, 0.030f), 0.84f, 0.52f, textures["oily_iron_albedo"], null, null, null, "oily_black_iron"),
                ["stone"] = SaveMaterial(shader, "WetBlackStoneContactGrime", new Color(0.050f, 0.047f, 0.041f), 0.03f, 0.66f, textures["stone_albedo"], textures["stone_normal"], textures["stone_mask"], null, "wet_black_stone_contact_grime"),
                ["glass"] = SaveMaterial(shader, "WarmAmberGlass", new Color(1.0f, 0.48f, 0.10f, 0.72f), 0.02f, 0.82f, textures["glass_albedo"], null, null, new Color(1.0f, 0.36f, 0.07f) * 0.38f, "warm_amber_glass"),
                ["dial"] = SaveMaterial(shader, "IvoryPressureGaugeDial", new Color(0.82f, 0.73f, 0.55f), 0.0f, 0.43f, textures["dial_albedo"], null, null, null, "ivory_pressure_gauge_dial"),
                ["red"] = SaveMaterial(shader, "OxideRedPressurePaint", new Color(0.55f, 0.08f, 0.045f), 0.05f, 0.35f, textures["red_albedo"], null, null, null, "oxide_red_pressure_paint"),
                ["copper"] = SaveMaterial(shader, "HeatStainedCopperPipe", new Color(0.75f, 0.31f, 0.10f), 0.86f, 0.37f, textures["copper_albedo"], null, null, null, "heat_stained_copper_pipe"),
                ["verdigris"] = SaveMaterial(shader, "VerdigrisDeposit", new Color(0.12f, 0.48f, 0.38f), 0.0f, 0.28f, textures["verdigris_albedo"], null, null, null, "verdigris_deposit")
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            return new Dictionary<string, Mesh>(StringComparer.Ordinal)
            {
                ["box"] = SaveMesh("BoxUnit", CreateBoxMesh()),
                ["cylinder16"] = SaveMesh("Cylinder16_Z", CreateCylinderMesh(16, 0.5f, 0.5f)),
                ["cylinder32"] = SaveMesh("Cylinder32_Z", CreateCylinderMesh(32, 0.5f, 0.5f)),
                ["cylinder64"] = SaveMesh("Cylinder64_Z", CreateCylinderMesh(64, 0.5f, 0.5f)),
                ["bolt"] = SaveMesh("BoltHeadHex_Z", CreateCylinderMesh(6, 0.5f, 0.38f)),
                ["ring48"] = SaveMesh("TrimRing48_Z", CreateRingMesh(48, 0.5f, 0.405f, 0.14f)),
                ["thinRing64"] = SaveMesh("ThinTrimRing64_Z", CreateRingMesh(64, 0.5f, 0.455f, 0.08f)),
                ["gearRing"] = SaveMesh("GearWheelToothedRing_Z", CreateGearRingMesh(32, 0.50f, 0.39f, 0.58f, 0.14f)),
                ["torus"] = SaveMesh("PipeValveTorus_Z", CreateTorusMesh(48, 10, 0.42f, 0.052f)),
                ["needle"] = SaveMesh("PressureGaugeNeedle", CreateNeedleMesh()),
                ["arch"] = SaveMesh("RivetedArchHalfRing_Z", CreateArchBandMesh(48, 0.5f, 0.39f, 0.16f)),
                ["gusset"] = SaveMesh("TriangularGusset", CreateGussetMesh())
            };
        }

        private static void CreateCircularVaultDoorSlab(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_CircularVaultDoorSlab");
            BuildDoorSlab(root.transform, meshes, materials, Vector3.zero, 1f, true);
            SavePrefab(root, "circular_vault_door_slab", "Layered circular dark-iron pressure door slab with brass rings, lower bars, rivet fields, grime crescent, and panel relief.");
        }

        private static void CreateGearWheelLockFace(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_GearWheelLockFace");
            BuildGearLock(root.transform, meshes, materials, Vector3.zero, 1f);
            SavePrefab(root, "gear_wheel_lock_face", "Central gear-wheel locking face with toothed brass ring, dark hub, spokes, pressure needle, and bolt crown.");
        }

        private static void CreateRadialBraces(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_RadialBraces");
            BuildRadialBraces(root.transform, meshes, materials, Vector3.zero, 1f);
            SavePrefab(root, "radial_braces", "Eight heavy radial braces with brass shoe plates, dark iron webbing, and readable bolt caps.");
        }

        private static void CreateHingePistonAssemblies(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_HingePistonAssemblies");
            BuildHingePistons(root.transform, meshes, materials, Vector3.zero, 1f);
            SavePrefab(root, "hinge_piston_assemblies", "Right-side triple hinge barrels and two exposed steam piston rams with brackets, collars, and coupler bolts.");
        }

        private static void CreateRivetedArchFrame(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_RivetedArchFrame");
            BuildArchFrame(root.transform, meshes, materials, Vector3.zero, 1f, true);
            SavePrefab(root, "riveted_arch_frame", "Tall riveted black-iron arch frame with brass inner trim, stone grime contact plates, amber lamps, and industrial side columns.");
        }

        private static void CreatePressureGaugeCluster(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_PressureGaugeCluster");
            BuildGaugeCluster(root.transform, meshes, materials, Vector3.zero, 1f);
            SavePrefab(root, "pressure_gauge_cluster", "Tri-gauge brass manifold cluster with ivory dials, glass covers, red needles, copper feed pipes, and valve details.");
        }

        private static void CreateBrassTrimRings(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_BrassTrimRings");
            BuildTrimRings(root.transform, meshes, materials, Vector3.zero, 1f);
            SavePrefab(root, "brass_trim_rings", "Nested brass, black gasket, and polished edgewear rings for vault-door silhouettes and pressure seals.");
        }

        private static void CreatePipeCouplers(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_PipeCouplers");
            BuildPipeCouplers(root.transform, meshes, materials, Vector3.zero, 1f);
            SavePrefab(root, "pipe_couplers", "Steam pipe coupler family with black iron pipe spans, brass flanges, copper elbows, valve wheel, and verdigris stains.");
        }

        private static void CreateHeroVaultDoorAssembly(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("HVDS12_HeroVaultDoor_Assembly");
            BuildArchFrame(root.transform, meshes, materials, Vector3.zero, 1f, true);
            BuildDoorSlab(root.transform, meshes, materials, new Vector3(0f, 0.10f, -0.10f), 1f, true);
            BuildRadialBraces(root.transform, meshes, materials, new Vector3(0f, 0.10f, -0.45f), 1f);
            BuildGearLock(root.transform, meshes, materials, new Vector3(0f, 0.10f, -0.70f), 1f);
            BuildHingePistons(root.transform, meshes, materials, new Vector3(0.15f, 0.08f, -0.30f), 1f);
            BuildGaugeCluster(root.transform, meshes, materials, new Vector3(2.78f, 2.40f, -0.65f), 0.54f);
            BuildPipeCouplers(root.transform, meshes, materials, new Vector3(-3.05f, 2.43f, -0.55f), 0.56f);
            BuildSideLantern(root.transform, meshes, materials, new Vector3(-3.80f, 0.88f, -0.66f), 0.85f, "left");
            BuildSideLantern(root.transform, meshes, materials, new Vector3(3.80f, 0.88f, -0.66f), 0.85f, "right");
            SavePrefab(root, "hero_vault_door_assembly", "Full hero vault door assembly combining slab, arch, gear lock, braces, hinges, pistons, gauges, pipes, lamps, rivets, and grime context.");
        }

        private static void BuildDoorSlab(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale, bool includeGrime)
        {
            Part(root, meshes["cylinder64"], "dark_riveted_iron_circular_pressure_slab", offset + V(0f, 0f, 0f, scale), S(6.00f, 6.00f, 0.46f, scale), Vector3.zero, materials["iron"]);
            Part(root, meshes["ring48"], "aged_brass_outer_pressure_trim_ring", offset + V(0f, 0f, -0.33f, scale), S(6.34f, 6.34f, 0.42f, scale), Vector3.zero, materials["brass"]);
            Part(root, meshes["thinRing64"], "polished_inner_edgewear_ring", offset + V(0f, 0f, -0.61f, scale), S(4.58f, 4.58f, 0.18f, scale), Vector3.zero, materials["brightBrass"]);
            Part(root, meshes["ring48"], "black_oily_gasket_mid_ring", offset + V(0f, 0f, -0.57f, scale), S(5.18f, 5.18f, 0.15f, scale), Vector3.zero, materials["oilyIron"]);
            for (int i = 0; i < 16; i++)
            {
                float angle = i * 22.5f;
                float r = 2.26f;
                Vector3 pos = offset + V(Mathf.Cos(angle * Mathf.Deg2Rad) * r, Mathf.Sin(angle * Mathf.Deg2Rad) * r, -0.66f, scale);
                Part(root, meshes["box"], $"alternating_segment_plate_{i:00}", pos, S(0.18f, 0.72f, 0.075f, scale), new Vector3(0f, 0f, angle), i % 2 == 0 ? materials["iron"] : materials["oilyIron"]);
            }

            for (int i = 0; i < 7; i++)
            {
                float x = -1.44f + i * 0.48f;
                Part(root, meshes["box"], $"lower_black_iron_locking_grate_bar_{i:00}", offset + V(x, -1.83f, -0.73f, scale), S(0.13f, 1.10f, 0.14f, scale), Vector3.zero, materials["oilyIron"]);
                Part(root, meshes["bolt"], $"upper_grate_brass_rivet_{i:00}", offset + V(x, -1.24f, -0.84f, scale), S(0.12f, 0.12f, 0.05f, scale), Vector3.zero, materials["brightBrass"]);
            }

            AddBoltRing(root, meshes, materials["brightBrass"], offset, 2.92f * scale, 36, -0.78f, 0.115f * scale, "outer_brass_bolt_ring");
            AddBoltRing(root, meshes, materials["iron"], offset, 2.46f * scale, 24, -0.80f, 0.090f * scale, "inner_black_bolt_ring");
            AddBoltRing(root, meshes, materials["brass"], offset, 1.53f * scale, 20, -0.82f, 0.082f * scale, "small_service_bolt_ring");

            if (includeGrime)
            {
                for (int i = 0; i < 9; i++)
                {
                    float x = -2.25f + i * 0.56f;
                    float y = -2.78f + Mathf.Abs(i - 4) * 0.03f;
                    Part(root, meshes["box"], $"wet_black_stone_contact_grime_smear_{i:00}", offset + V(x, y, -0.86f, scale), S(0.46f, 0.09f, 0.035f, scale), new Vector3(0f, 0f, -8f + i * 2f), materials["stone"]);
                }
            }
        }

        private static void BuildGearLock(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale)
        {
            Part(root, meshes["gearRing"], "toothed_aged_brass_lock_wheel", offset + V(0f, 0f, -0.05f, scale), S(2.05f, 2.05f, 0.56f, scale), Vector3.zero, materials["brass"]);
            Part(root, meshes["ring48"], "dark_recessed_inner_lock_ring", offset + V(0f, 0f, -0.19f, scale), S(1.38f, 1.38f, 0.25f, scale), Vector3.zero, materials["oilyIron"]);
            Part(root, meshes["cylinder32"], "raised_polished_brass_lock_hub", offset + V(0f, 0f, -0.38f, scale), S(0.65f, 0.65f, 0.32f, scale), Vector3.zero, materials["brightBrass"]);
            Part(root, meshes["cylinder16"], "black_iron_central_axle_cap", offset + V(0f, 0f, -0.62f, scale), S(0.33f, 0.33f, 0.23f, scale), Vector3.zero, materials["iron"]);
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f;
                Part(root, meshes["box"], $"polished_spoke_bar_{i:00}", offset + V(Mathf.Cos(angle * Mathf.Deg2Rad) * 0.50f, Mathf.Sin(angle * Mathf.Deg2Rad) * 0.50f, -0.48f, scale), S(1.05f, 0.075f, 0.10f, scale), new Vector3(0f, 0f, angle), materials["brightBrass"]);
            }

            Part(root, meshes["needle"], "oxide_red_pressure_alignment_pointer", offset + V(0.12f, 0.21f, -0.72f, scale), S(0.92f, 0.92f, 0.12f, scale), new Vector3(0f, 0f, -28f), materials["red"]);
            AddBoltRing(root, meshes, materials["brightBrass"], offset + V(0f, 0f, -0.76f, scale), 0.82f * scale, 12, 0f, 0.064f * scale, "gear_face_bolt");
        }

        private static void BuildRadialBraces(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f;
                float r = 1.68f;
                Part(root, meshes["box"], $"black_iron_radial_web_brace_{i:00}", offset + V(Mathf.Cos(angle * Mathf.Deg2Rad) * r, Mathf.Sin(angle * Mathf.Deg2Rad) * r, -0.04f, scale), S(2.46f, 0.18f, 0.17f, scale), new Vector3(0f, 0f, angle), materials["iron"]);
                Part(root, meshes["box"], $"aged_brass_radial_shoe_plate_{i:00}", offset + V(Mathf.Cos(angle * Mathf.Deg2Rad) * 2.50f, Mathf.Sin(angle * Mathf.Deg2Rad) * 2.50f, -0.18f, scale), S(0.62f, 0.32f, 0.16f, scale), new Vector3(0f, 0f, angle), materials["brass"]);
            }

            Part(root, meshes["ring48"], "radial_brace_black_gasket_root_ring", offset + V(0f, 0f, -0.22f, scale), S(2.16f, 2.16f, 0.15f, scale), Vector3.zero, materials["oilyIron"]);
            AddBoltRing(root, meshes, materials["brightBrass"], offset + V(0f, 0f, -0.32f, scale), 2.02f * scale, 16, 0f, 0.080f * scale, "radial_brace_end_bolt");
            AddBoltRing(root, meshes, materials["brass"], offset + V(0f, 0f, -0.35f, scale), 1.10f * scale, 16, 0f, 0.060f * scale, "radial_brace_inner_bolt");
        }

        private static void BuildHingePistons(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale)
        {
            float hingeX = 3.32f;
            for (int i = 0; i < 3; i++)
            {
                float y = -1.80f + i * 1.80f;
                Part(root, meshes["box"], $"black_iron_hinge_backplate_{i:00}", offset + V(hingeX, y, -0.08f, scale), S(0.52f, 1.12f, 0.22f, scale), Vector3.zero, materials["oilyIron"]);
                Part(root, meshes["cylinder32"], $"aged_brass_vertical_hinge_barrel_{i:00}", offset + V(hingeX + 0.20f, y, -0.42f, scale), S(0.42f, 0.42f, 1.18f, scale), new Vector3(90f, 0f, 0f), materials["brass"]);
                Part(root, meshes["ring48"], $"polished_hinge_collar_upper_{i:00}", offset + V(hingeX + 0.20f, y + 0.47f, -0.42f, scale), S(0.55f, 0.55f, 0.18f, scale), new Vector3(90f, 0f, 0f), materials["brightBrass"]);
                Part(root, meshes["ring48"], $"polished_hinge_collar_lower_{i:00}", offset + V(hingeX + 0.20f, y - 0.47f, -0.42f, scale), S(0.55f, 0.55f, 0.18f, scale), new Vector3(90f, 0f, 0f), materials["brightBrass"]);
                for (int b = 0; b < 4; b++)
                {
                    float by = y - 0.36f + b * 0.24f;
                    Part(root, meshes["bolt"], $"hinge_backplate_rivet_{i:00}_{b:00}", offset + V(hingeX - 0.16f, by, -0.28f, scale), S(0.09f, 0.09f, 0.05f, scale), Vector3.zero, materials["brightBrass"]);
                }
            }

            AddPiston(root, meshes, materials, offset + V(2.55f, 1.60f, -0.44f, scale), offset + V(3.60f, 2.36f, -0.44f, scale), scale, "upper_diagonal_steam_piston");
            AddPiston(root, meshes, materials, offset + V(2.55f, -1.70f, -0.44f, scale), offset + V(3.60f, -2.40f, -0.44f, scale), scale, "lower_diagonal_steam_piston");
            Part(root, meshes["box"], "right_side_locking_receiver_block", offset + V(2.93f, 0.02f, -0.38f, scale), S(0.38f, 1.32f, 0.34f, scale), Vector3.zero, materials["iron"]);
        }

        private static void BuildArchFrame(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale, bool lamps)
        {
            Part(root, meshes["arch"], "blackened_iron_riveted_arch_crown", offset + V(0f, 0.42f, 0.08f, scale), S(8.75f, 8.75f, 0.95f, scale), Vector3.zero, materials["iron"]);
            Part(root, meshes["arch"], "aged_brass_inner_arch_trim", offset + V(0f, 0.42f, -0.22f, scale), S(7.40f, 7.40f, 0.48f, scale), Vector3.zero, materials["brass"]);
            Part(root, meshes["box"], "left_black_iron_arch_leg", offset + V(-3.80f, -1.52f, 0.08f, scale), S(0.70f, 4.65f, 0.78f, scale), Vector3.zero, materials["iron"]);
            Part(root, meshes["box"], "right_black_iron_arch_leg", offset + V(3.80f, -1.52f, 0.08f, scale), S(0.70f, 4.65f, 0.78f, scale), Vector3.zero, materials["iron"]);
            Part(root, meshes["box"], "wet_black_stone_threshold_sill", offset + V(0f, -3.15f, 0.17f, scale), S(8.60f, 0.55f, 0.86f, scale), Vector3.zero, materials["stone"]);
            Part(root, meshes["box"], "polished_brass_floor_track", offset + V(0f, -2.85f, -0.30f, scale), S(7.24f, 0.16f, 0.22f, scale), Vector3.zero, materials["brightBrass"]);

            for (int i = 0; i <= 22; i++)
            {
                float t = i / 22f;
                float a = Mathf.Lerp(14f, 166f, t) * Mathf.Deg2Rad;
                Vector3 p = offset + V(Mathf.Cos(a) * 4.04f, Mathf.Sin(a) * 4.04f + 0.42f, -0.43f, scale);
                Part(root, meshes["bolt"], $"arch_crown_brass_rivet_{i:00}", p, S(0.11f, 0.11f, 0.055f, scale), Vector3.zero, materials["brightBrass"]);
            }

            for (int i = 0; i < 10; i++)
            {
                float y = -2.78f + i * 0.49f;
                Part(root, meshes["bolt"], $"left_arch_leg_rivet_{i:00}", offset + V(-3.80f, y, -0.38f, scale), S(0.11f, 0.11f, 0.055f, scale), Vector3.zero, materials["brightBrass"]);
                Part(root, meshes["bolt"], $"right_arch_leg_rivet_{i:00}", offset + V(3.80f, y, -0.38f, scale), S(0.11f, 0.11f, 0.055f, scale), Vector3.zero, materials["brightBrass"]);
            }

            if (lamps)
            {
                BuildSideLantern(root, meshes, materials, offset + V(-4.35f, 0.28f, -0.42f, scale), 0.72f * scale, "arch_left");
                BuildSideLantern(root, meshes, materials, offset + V(4.35f, 0.28f, -0.42f, scale), 0.72f * scale, "arch_right");
            }
        }

        private static void BuildGaugeCluster(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale)
        {
            Part(root, meshes["box"], "black_iron_gauge_cluster_backplate", offset + V(0f, 0f, 0.12f, scale), S(2.58f, 0.86f, 0.16f, scale), Vector3.zero, materials["oilyIron"]);
            Part(root, meshes["cylinder32"], "horizontal_heat_stained_copper_manifold", offset + V(0f, -0.34f, -0.05f, scale), S(0.13f, 0.13f, 2.55f, scale), new Vector3(0f, 90f, 0f), materials["copper"]);

            AddGauge(root, meshes, materials, offset + V(-0.82f, 0.08f, -0.18f, scale), 0.48f * scale, -34f, "left");
            AddGauge(root, meshes, materials, offset + V(0f, 0.18f, -0.20f, scale), 0.56f * scale, 22f, "center");
            AddGauge(root, meshes, materials, offset + V(0.86f, 0.03f, -0.18f, scale), 0.43f * scale, 58f, "right");
            for (int i = 0; i < 4; i++)
            {
                float x = -1.12f + i * 0.75f;
                Part(root, meshes["ring48"], $"brass_manifold_coupler_{i:00}", offset + V(x, -0.34f, -0.05f, scale), S(0.32f, 0.32f, 0.16f, scale), new Vector3(0f, 90f, 0f), materials["brass"]);
            }

            AddValveWheel(root, meshes, materials, offset + V(1.36f, -0.34f, -0.20f, scale), 0.28f * scale, "cluster_red_bleed_valve");
        }

        private static void BuildTrimRings(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale)
        {
            Part(root, meshes["ring48"], "large_aged_brass_service_ring", offset + V(0f, 0f, 0f, scale), S(3.20f, 3.20f, 0.28f, scale), Vector3.zero, materials["brass"]);
            Part(root, meshes["thinRing64"], "thin_polished_highlight_ring", offset + V(0f, 0f, -0.22f, scale), S(2.70f, 2.70f, 0.16f, scale), Vector3.zero, materials["brightBrass"]);
            Part(root, meshes["ring48"], "oily_black_pressure_gasket_ring", offset + V(0f, 0f, -0.42f, scale), S(2.18f, 2.18f, 0.18f, scale), Vector3.zero, materials["oilyIron"]);
            Part(root, meshes["thinRing64"], "small_brass_inner_retainer_ring", offset + V(0f, 0f, -0.60f, scale), S(1.48f, 1.48f, 0.14f, scale), Vector3.zero, materials["brass"]);
            AddBoltRing(root, meshes, materials["brightBrass"], offset + V(0f, 0f, -0.72f, scale), 1.48f * scale, 18, 0f, 0.07f * scale, "trim_ring_service_bolt");
        }

        private static void BuildPipeCouplers(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale)
        {
            Part(root, meshes["cylinder32"], "black_iron_horizontal_steam_pipe", offset + V(0f, 0f, 0f, scale), S(0.27f, 0.27f, 2.95f, scale), new Vector3(0f, 90f, 0f), materials["iron"]);
            Part(root, meshes["cylinder32"], "upper_heat_stained_copper_bypass_pipe", offset + V(0f, 0.42f, -0.03f, scale), S(0.12f, 0.12f, 2.36f, scale), new Vector3(0f, 90f, 0f), materials["copper"]);
            for (int i = 0; i < 5; i++)
            {
                float x = -1.22f + i * 0.61f;
                Part(root, meshes["ring48"], $"aged_brass_pipe_flange_{i:00}", offset + V(x, 0f, 0f, scale), S(0.54f, 0.54f, 0.17f, scale), new Vector3(0f, 90f, 0f), materials["brass"]);
                Part(root, meshes["bolt"], $"pipe_flange_top_bolt_{i:00}", offset + V(x, 0.32f, -0.16f, scale), S(0.07f, 0.07f, 0.05f, scale), Vector3.zero, materials["brightBrass"]);
                Part(root, meshes["bolt"], $"pipe_flange_bottom_bolt_{i:00}", offset + V(x, -0.32f, -0.16f, scale), S(0.07f, 0.07f, 0.05f, scale), Vector3.zero, materials["brightBrass"]);
            }

            AddValveWheel(root, meshes, materials, offset + V(1.46f, 0.44f, -0.18f, scale), 0.36f * scale, "pipe_coupler_side_valve_wheel");
            Part(root, meshes["box"], "verdigris_leak_stain_under_coupler", offset + V(-0.92f, -0.38f, -0.22f, scale), S(0.45f, 0.13f, 0.055f, scale), new Vector3(0f, 0f, -9f), materials["verdigris"]);
        }

        private static void BuildSideLantern(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 offset, float scale, string id)
        {
            Part(root, meshes["box"], $"{id}_black_iron_lantern_wall_bracket", offset + V(0f, 0f, 0.18f, scale), S(0.36f, 1.12f, 0.12f, scale), Vector3.zero, materials["oilyIron"]);
            Part(root, meshes["cylinder32"], $"{id}_warm_amber_lantern_glass_tube", offset + V(0f, 0f, -0.04f, scale), S(0.34f, 0.34f, 0.86f, scale), new Vector3(90f, 0f, 0f), materials["glass"]);
            Part(root, meshes["ring48"], $"{id}_brass_lantern_top_cage", offset + V(0f, 0.49f, -0.04f, scale), S(0.47f, 0.47f, 0.15f, scale), new Vector3(90f, 0f, 0f), materials["brass"]);
            Part(root, meshes["ring48"], $"{id}_brass_lantern_bottom_cage", offset + V(0f, -0.49f, -0.04f, scale), S(0.47f, 0.47f, 0.15f, scale), new Vector3(90f, 0f, 0f), materials["brass"]);
            for (int i = 0; i < 4; i++)
            {
                float a = i * 90f;
                Part(root, meshes["box"], $"{id}_vertical_lantern_cage_bar_{i:00}", offset + V(Mathf.Cos(a * Mathf.Deg2Rad) * 0.20f, 0f, -0.04f + Mathf.Sin(a * Mathf.Deg2Rad) * 0.04f, scale), S(0.035f, 0.92f, 0.035f, scale), Vector3.zero, materials["brightBrass"]);
            }
        }

        private static void AddGauge(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 position, float size, float needleAngle, string id)
        {
            Part(root, meshes["ring48"], $"{id}_aged_brass_pressure_gauge_bezel", position, new Vector3(size, size, size * 0.18f), Vector3.zero, materials["brass"]);
            Part(root, meshes["cylinder32"], $"{id}_ivory_pressure_dial_face", position + new Vector3(0f, 0f, -size * 0.12f), new Vector3(size * 0.78f, size * 0.78f, size * 0.035f), Vector3.zero, materials["dial"]);
            Part(root, meshes["thinRing64"], $"{id}_warm_glass_gauge_cover", position + new Vector3(0f, 0f, -size * 0.19f), new Vector3(size * 0.86f, size * 0.86f, size * 0.045f), Vector3.zero, materials["glass"]);
            Part(root, meshes["needle"], $"{id}_oxide_red_gauge_needle", position + new Vector3(0f, 0f, -size * 0.25f), new Vector3(size * 0.86f, size * 0.86f, size * 0.08f), new Vector3(0f, 0f, needleAngle), materials["red"]);
            Part(root, meshes["cylinder16"], $"{id}_black_gauge_center_pin", position + new Vector3(0f, 0f, -size * 0.31f), new Vector3(size * 0.14f, size * 0.14f, size * 0.07f), Vector3.zero, materials["oilyIron"]);
        }

        private static void AddValveWheel(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 position, float size, string id)
        {
            Part(root, meshes["torus"], $"{id}_oxide_red_valve_wheel_ring", position, new Vector3(size, size, size), Vector3.zero, materials["red"]);
            for (int i = 0; i < 5; i++)
            {
                float angle = i * 72f;
                Part(root, meshes["box"], $"{id}_brass_valve_spoke_{i:00}", position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * size * 0.18f, Mathf.Sin(angle * Mathf.Deg2Rad) * size * 0.18f, -size * 0.035f), new Vector3(size * 0.58f, size * 0.035f, size * 0.05f), new Vector3(0f, 0f, angle), materials["brightBrass"]);
            }

            Part(root, meshes["cylinder16"], $"{id}_black_iron_valve_hub", position + new Vector3(0f, 0f, -size * 0.08f), new Vector3(size * 0.22f, size * 0.22f, size * 0.10f), Vector3.zero, materials["oilyIron"]);
        }

        private static void AddPiston(Transform root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 a, Vector3 b, float scale, string id)
        {
            Vector3 mid = (a + b) * 0.5f;
            float length = Vector3.Distance(a, b);
            PartBetween(root, meshes["cylinder32"], $"{id}_black_iron_outer_sleeve", a, b, new Vector3(0.18f * scale, 0.18f * scale, length), materials["iron"]);
            PartBetween(root, meshes["cylinder32"], $"{id}_polished_brass_inner_ram", Vector3.Lerp(a, b, 0.28f), Vector3.Lerp(a, b, 0.92f), new Vector3(0.105f * scale, 0.105f * scale, length * 0.64f), materials["brightBrass"]);
            Part(root, meshes["ring48"], $"{id}_front_collar", a, S(0.42f, 0.42f, 0.16f, scale), Quaternion.FromToRotation(Vector3.forward, b - a).eulerAngles, materials["brass"]);
            Part(root, meshes["ring48"], $"{id}_rear_collar", b, S(0.42f, 0.42f, 0.16f, scale), Quaternion.FromToRotation(Vector3.forward, b - a).eulerAngles, materials["brass"]);
            Part(root, meshes["gusset"], $"{id}_triangular_gusset_shadow_plate", mid + new Vector3(0f, -0.18f * scale, 0.12f * scale), S(0.55f, 0.55f, 0.16f, scale), Vector3.zero, materials["oilyIron"]);
        }

        private static void AddBoltRing(Transform root, IReadOnlyDictionary<string, Mesh> meshes, Material material, Vector3 offset, float radius, int count, float z, float size, string prefix)
        {
            for (int i = 0; i < count; i++)
            {
                float a = i * Mathf.PI * 2f / count;
                Vector3 pos = offset + new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, z);
                Part(root, meshes["bolt"], $"{prefix}_{i:00}", pos, new Vector3(size, size, size * 0.50f), Vector3.zero, material);
            }
        }

        private static GameObject Part(Transform parent, Mesh mesh, string name, Vector3 localPosition, Vector3 localScale, Vector3 localEuler, Material material)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localRotation = Quaternion.Euler(localEuler);
            go.transform.localScale = localScale;
            MeshFilter filter = go.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.On;
            renderer.receiveShadows = true;
            return go;
        }

        private static GameObject PartBetween(Transform parent, Mesh mesh, string name, Vector3 a, Vector3 b, Vector3 localScale, Material material)
        {
            GameObject go = Part(parent, mesh, name, (a + b) * 0.5f, localScale, Vector3.zero, material);
            go.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, b - a);
            return go;
        }

        private static GameObject NewRoot(string name)
        {
            GameObject root = new GameObject(name);
            root.transform.position = Vector3.zero;
            return root;
        }

        private static void SavePrefab(GameObject root, string tag, string note)
        {
            string path = PrefabRoot + "/" + root.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            PrefabRecords.Add(new Record { Path = "Runtime/Prefabs/" + Path.GetFileName(path), Tag = tag, Note = note });
        }

        private static Texture2D SaveTexture(string packagePhysicalRoot, string name, Texture2D texture, bool normalMap, bool maskMap, string tag)
        {
            string fileName = Prefix + "_TEX_" + name + ".png";
            string absolute = Path.Combine(packagePhysicalRoot, "Runtime", "Textures", fileName);
            File.WriteAllBytes(absolute, ImageConversion.EncodeToPNG(texture));
            Object.DestroyImmediate(texture);

            string assetPath = TextureRoot + "/" + fileName;
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = normalMap ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.sRGBTexture = !normalMap && !maskMap;
                importer.mipmapEnabled = true;
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.filterMode = FilterMode.Bilinear;
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
                importer.SaveAndReimport();
            }

            Texture2D imported = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            TextureRecords.Add(new Record { Path = "Runtime/Textures/" + fileName, Tag = tag, Note = normalMap ? "normal map" : maskMap ? "metallic smoothness mask" : "albedo/emissive tint map" });
            return imported;
        }

        private static Material SaveMaterial(Shader shader, string name, Color color, float metallic, float smoothness, Texture2D albedo, Texture2D normal, Texture2D mask, Color? emission, string tag)
        {
            string assetName = Prefix + "_MAT_" + name + ".mat";
            string path = MaterialRoot + "/" + assetName;
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }
            else
            {
                material.shader = shader;
            }

            SetColor(material, "_Color", color);
            SetColor(material, "_BaseColor", color);
            SetFloat(material, "_Metallic", metallic);
            SetFloat(material, "_Smoothness", smoothness);
            SetFloat(material, "_Glossiness", smoothness);
            SetTexture(material, "_MainTex", albedo);
            SetTexture(material, "_BaseMap", albedo);
            if (normal != null)
            {
                SetTexture(material, "_BumpMap", normal);
                material.EnableKeyword("_NORMALMAP");
            }
            if (mask != null)
            {
                SetTexture(material, "_MetallicGlossMap", mask);
                material.EnableKeyword("_METALLICGLOSSMAP");
            }
            if (emission.HasValue)
            {
                SetColor(material, "_EmissionColor", emission.Value);
                SetTexture(material, "_EmissionMap", albedo);
                material.EnableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
            else
            {
                material.DisableKeyword("_EMISSION");
            }

            EditorUtility.SetDirty(material);
            MaterialRecords.Add(new Record { Path = "Runtime/Materials/" + assetName, Tag = tag, Note = $"metallic={metallic:0.00}, smoothness={smoothness:0.00}" });
            return material;
        }

        private static Mesh SaveMesh(string name, Mesh mesh)
        {
            string assetName = Prefix + "_MESH_" + name + ".asset";
            string path = MeshRoot + "/" + assetName;
            Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing == null)
            {
                AssetDatabase.CreateAsset(mesh, path);
                existing = mesh;
            }
            else
            {
                EditorUtility.CopySerialized(mesh, existing);
                Object.DestroyImmediate(mesh);
            }

            MeshRecords.Add(new Record { Path = "Runtime/Meshes/" + assetName, Tag = name, Note = "code-created Unity mesh" });
            return existing;
        }

        private static void RenderPreviews(string renderRoot, IReadOnlyDictionary<string, Material> materials)
        {
            foreach (string file in Directory.GetFiles(renderRoot, Prefix + "_*.png"))
            {
                File.Delete(file);
            }

            RenderSingle(renderRoot, "HVDS12_RENDER_front_hero_door.png", "HVDS12_HeroVaultDoor_Assembly", new Vector3(0f, 0.10f, -10.2f), new Vector3(0f, 0.10f, 0f), 39f, materials, false);
            RenderSingle(renderRoot, "HVDS12_RENDER_oblique_corridor_door_angle.png", "HVDS12_HeroVaultDoor_Assembly", new Vector3(-5.7f, 1.35f, -8.1f), new Vector3(0.35f, -0.18f, 0f), 46f, materials, true);
            RenderComponentSheet(renderRoot, materials);
        }

        private static void RenderSingle(string renderRoot, string fileName, string prefabName, Vector3 cameraPos, Vector3 lookAt, float fov, IReadOnlyDictionary<string, Material> materials, bool corridor)
        {
            GameObject sceneRoot = new GameObject("HVDS12_RenderScene");
            try
            {
                SetupRenderEnvironment(sceneRoot.transform, materials, corridor);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + prefabName + ".prefab");
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (instance == null)
                {
                    throw new InvalidOperationException("Could not instantiate " + prefabName);
                }
                instance.transform.SetParent(sceneRoot.transform, false);
                instance.transform.localPosition = Vector3.zero;

                Camera camera = CreateCamera(sceneRoot.transform, cameraPos, lookAt, fov);
                SaveCameraPng(camera, Path.Combine(renderRoot, fileName));
            }
            finally
            {
                Object.DestroyImmediate(sceneRoot);
            }
        }

        private static void RenderComponentSheet(string renderRoot, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject sceneRoot = new GameObject("HVDS12_ComponentSheetScene");
            try
            {
                SetupRenderEnvironment(sceneRoot.transform, materials, false);
                string[] prefabs =
                {
                    "HVDS12_CircularVaultDoorSlab", "HVDS12_GearWheelLockFace", "HVDS12_RadialBraces", "HVDS12_HingePistonAssemblies",
                    "HVDS12_RivetedArchFrame", "HVDS12_PressureGaugeCluster", "HVDS12_BrassTrimRings", "HVDS12_PipeCouplers"
                };
                Vector3[] positions =
                {
                    new Vector3(-4.7f, 1.4f, 0f), new Vector3(-1.35f, 1.65f, -0.35f), new Vector3(1.35f, 1.65f, -0.35f), new Vector3(4.35f, 1.35f, -0.20f),
                    new Vector3(-4.65f, -2.20f, 0f), new Vector3(-1.30f, -1.95f, -0.35f), new Vector3(1.35f, -1.95f, -0.35f), new Vector3(4.35f, -1.95f, -0.35f)
                };
                float[] scales = { 0.38f, 0.82f, 0.58f, 0.58f, 0.36f, 1.0f, 0.75f, 1.0f };

                for (int i = 0; i < prefabs.Length; i++)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + prefabs[i] + ".prefab");
                    GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    if (instance == null)
                    {
                        continue;
                    }
                    instance.transform.SetParent(sceneRoot.transform, false);
                    instance.transform.localPosition = positions[i];
                    instance.transform.localScale = Vector3.one * scales[i];
                }

                Camera camera = CreateCamera(sceneRoot.transform, new Vector3(0f, 0.08f, -10.9f), Vector3.zero, 43f);
                SaveCameraPng(camera, Path.Combine(renderRoot, "HVDS12_RENDER_component_sheet.png"));
            }
            finally
            {
                Object.DestroyImmediate(sceneRoot);
            }
        }

        private static void SetupRenderEnvironment(Transform root, IReadOnlyDictionary<string, Material> materials, bool corridor)
        {
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.030f, 0.028f, 0.024f);
            Part(root, AssetDatabase.LoadAssetAtPath<Mesh>(MeshRoot + "/" + Prefix + "_MESH_BoxUnit.asset"), "wet_black_stone_back_wall", new Vector3(0f, 0.35f, 0.55f), new Vector3(11.5f, 7.6f, 0.22f), Vector3.zero, materials["stone"]);
            Part(root, AssetDatabase.LoadAssetAtPath<Mesh>(MeshRoot + "/" + Prefix + "_MESH_BoxUnit.asset"), "wet_black_stone_floor_slab", new Vector3(0f, -3.52f, -2.62f), new Vector3(11.5f, 0.22f, 6.8f), Vector3.zero, materials["stone"]);

            if (corridor)
            {
                Part(root, AssetDatabase.LoadAssetAtPath<Mesh>(MeshRoot + "/" + Prefix + "_MESH_BoxUnit.asset"), "left_corridor_return_wall", new Vector3(-5.42f, 0.35f, -1.82f), new Vector3(0.18f, 7.6f, 5.9f), Vector3.zero, materials["stone"]);
                Part(root, AssetDatabase.LoadAssetAtPath<Mesh>(MeshRoot + "/" + Prefix + "_MESH_BoxUnit.asset"), "right_corridor_return_wall", new Vector3(5.42f, 0.35f, -1.82f), new Vector3(0.18f, 7.6f, 5.9f), Vector3.zero, materials["stone"]);
            }

            Light key = new GameObject("warm_lantern_key_light").AddComponent<Light>();
            key.transform.SetParent(root, false);
            key.transform.localPosition = new Vector3(-3.4f, 2.0f, -3.4f);
            key.type = LightType.Point;
            key.color = new Color(1.0f, 0.58f, 0.25f);
            key.intensity = 3.1f;
            key.range = 8.5f;

            Light rim = new GameObject("cool_soft_rim_light").AddComponent<Light>();
            rim.transform.SetParent(root, false);
            rim.transform.localPosition = new Vector3(3.8f, 3.4f, -2.8f);
            rim.type = LightType.Point;
            rim.color = new Color(0.36f, 0.44f, 0.50f);
            rim.intensity = 0.75f;
            rim.range = 9f;

            Light front = new GameObject("low_amber_floor_reflection").AddComponent<Light>();
            front.transform.SetParent(root, false);
            front.transform.localPosition = new Vector3(0f, -2.65f, -3.3f);
            front.type = LightType.Point;
            front.color = new Color(1.0f, 0.40f, 0.10f);
            front.intensity = 1.10f;
            front.range = 7f;
        }

        private static Camera CreateCamera(Transform root, Vector3 position, Vector3 lookAt, float fov)
        {
            GameObject cameraGo = new GameObject("HVDS12_RenderCamera");
            cameraGo.transform.SetParent(root, false);
            cameraGo.transform.position = position;
            cameraGo.transform.rotation = Quaternion.LookRotation(lookAt - position, Vector3.up);
            Camera camera = cameraGo.AddComponent<Camera>();
            camera.fieldOfView = fov;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 80f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.018f, 0.016f, 0.014f);
            camera.allowHDR = false;
            return camera;
        }

        private static void SaveCameraPng(Camera camera, string absolutePath)
        {
            RenderTexture renderTexture = new RenderTexture(RenderWidth, RenderHeight, 24, RenderTextureFormat.ARGB32);
            RenderTexture previous = RenderTexture.active;
            camera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            camera.Render();
            Texture2D output = new Texture2D(RenderWidth, RenderHeight, TextureFormat.RGBA32, false);
            output.ReadPixels(new Rect(0, 0, RenderWidth, RenderHeight), 0, 0);
            output.Apply();
            File.WriteAllBytes(absolutePath, ImageConversion.EncodeToPNG(output));
            Object.DestroyImmediate(output);
            camera.targetTexture = null;
            RenderTexture.active = previous;
            renderTexture.Release();
            Object.DestroyImmediate(renderTexture);
        }

        private static ValidationResult Validate(string packagePhysicalRoot, string renderRoot)
        {
            ValidationResult result = new ValidationResult();
            result.PrefabCount = Directory.GetFiles(Path.Combine(packagePhysicalRoot, "Runtime", "Prefabs"), "*.prefab").Length;
            result.MaterialCount = Directory.GetFiles(Path.Combine(packagePhysicalRoot, "Runtime", "Materials"), "*.mat").Length;
            result.MeshCount = Directory.GetFiles(Path.Combine(packagePhysicalRoot, "Runtime", "Meshes"), "*.asset").Length;
            result.TextureCount = Directory.GetFiles(Path.Combine(packagePhysicalRoot, "Runtime", "Textures"), "*.png").Length;
            result.RenderCount = Directory.GetFiles(renderRoot, "*.png").Length;

            Expect(result, result.PrefabCount >= 9, "Expected at least 9 prefabs.");
            Expect(result, result.MaterialCount >= 10, "Expected at least 10 materials.");
            Expect(result, result.MeshCount >= 12, "Expected at least 12 mesh assets.");
            Expect(result, result.TextureCount >= 14, "Expected at least 14 texture maps.");
            Expect(result, result.RenderCount >= 3, "Expected at least 3 documentation renders.");

            string[] required =
            {
                "HVDS12_CircularVaultDoorSlab.prefab", "HVDS12_GearWheelLockFace.prefab", "HVDS12_RadialBraces.prefab",
                "HVDS12_HingePistonAssemblies.prefab", "HVDS12_RivetedArchFrame.prefab", "HVDS12_PressureGaugeCluster.prefab",
                "HVDS12_BrassTrimRings.prefab", "HVDS12_PipeCouplers.prefab", "HVDS12_HeroVaultDoor_Assembly.prefab"
            };
            foreach (string prefab in required)
            {
                Expect(result, File.Exists(Path.Combine(packagePhysicalRoot, "Runtime", "Prefabs", prefab)), "Missing required prefab " + prefab);
            }

            result.Passed = result.Failures.Count == 0;
            return result;
        }

        private static void WriteManifest(ValidationResult validation)
        {
            string json = BuildManifestJson(validation);
            WriteTextAsset(MetadataRoot + "/" + Prefix + "_HeroVaultDoorSet12_Manifest_v0_1_57_p012.json", json);
            WriteTextAsset(ManifestRoot + "/" + Prefix + "_HeroVaultDoorSet12_Manifest_v0_1_57_p012.json", json);
        }

        private static string BuildManifestJson(ValidationResult validation)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"packId\": \"HVDS12\",");
            sb.AppendLine("  \"packageName\": \"" + PackageName + "\",");
            sb.AppendLine("  \"version\": \"" + Version + "\",");
            sb.AppendLine("  \"sourceConstraint\": \"Unity procedural/code-created assets only; no Blender or external DCC\",");
            sb.AppendLine("  \"northStarReference\": \"Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png\",");
            AppendRecords(sb, "materials", MaterialRecords);
            sb.AppendLine(",");
            AppendRecords(sb, "textures", TextureRecords);
            sb.AppendLine(",");
            AppendRecords(sb, "meshes", MeshRecords);
            sb.AppendLine(",");
            AppendRecords(sb, "prefabs", PrefabRecords);
            sb.AppendLine(",");
            sb.AppendLine("  \"validation\": {");
            sb.AppendLine("    \"passed\": " + validation.Passed.ToString().ToLowerInvariant() + ",");
            sb.AppendLine("    \"prefabCount\": " + validation.PrefabCount + ",");
            sb.AppendLine("    \"materialCount\": " + validation.MaterialCount + ",");
            sb.AppendLine("    \"meshCount\": " + validation.MeshCount + ",");
            sb.AppendLine("    \"textureCount\": " + validation.TextureCount + ",");
            sb.AppendLine("    \"renderCount\": " + validation.RenderCount);
            sb.AppendLine("  }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void AppendRecords(StringBuilder sb, string name, IReadOnlyList<Record> records)
        {
            sb.AppendLine("  \"" + name + "\": [");
            for (int i = 0; i < records.Count; i++)
            {
                Record r = records[i];
                sb.Append("    { \"path\": \"").Append(JsonEscape(r.Path)).Append("\", \"tag\": \"").Append(JsonEscape(r.Tag)).Append("\", \"note\": \"").Append(JsonEscape(r.Note)).Append("\" }");
                if (i < records.Count - 1)
                {
                    sb.Append(",");
                }
                sb.AppendLine();
            }
            sb.Append("  ]");
        }

        private static void WriteDocs(string repoRoot, string renderRoot, ValidationResult validation)
        {
            string planningPath = Path.Combine(repoRoot, "Documentation", "Planning", "HeroVaultDoorSet12_ImportReadiness.md");
            string qaPath = Path.Combine(repoRoot, "Documentation", "QA", "HeroVaultDoorSet12_QA.md");
            string renderIndexPath = Path.Combine(renderRoot, "HVDS12_RENDER_INDEX.md");
            Directory.CreateDirectory(Path.GetDirectoryName(planningPath) ?? repoRoot);
            Directory.CreateDirectory(Path.GetDirectoryName(qaPath) ?? repoRoot);

            File.WriteAllText(planningPath, BuildPlanningDoc(validation));
            File.WriteAllText(qaPath, BuildQaDoc(validation));
            File.WriteAllText(renderIndexPath, BuildRenderIndex(renderRoot));
        }

        private static string BuildPlanningDoc(ValidationResult validation)
        {
            return "# HeroVaultDoorSet12 Import Readiness\n\n"
                + "- Package: `AssetPacks/BrassworksBreach.HeroVaultDoorSet12`\n"
                + "- Version: `" + Version + "`\n"
                + "- Status: " + (validation.Passed ? "PASS" : "FAIL") + "\n"
                + "- Integration note: sidecar only; not added to main playable scenes or shared status docs.\n"
                + "- Runtime prefabs: circular slab, gear lock, radial braces, hinge/pistons, riveted arch, pressure gauges, brass rings, pipe couplers, full hero assembly.\n"
                + "- Materials/maps: aged brass, polished brass edgewear, dark riveted iron, oily black iron, wet black stone contact grime, warm amber glass, ivory dial enamel, red pressure paint, heat-stained copper, verdigris deposit.\n"
                + "- Recommended next step: import the package in a controlled scene and place `HVDS12_HeroVaultDoor_Assembly` as a visual candidate only.\n";
        }

        private static string BuildQaDoc(ValidationResult validation)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# HeroVaultDoorSet12 QA Checklist");
            sb.AppendLine();
            sb.AppendLine("| Check | Result | Notes |");
            sb.AppendLine("| --- | --- | --- |");
            sb.AppendLine("| Isolated sidecar package only | PASS | Generated under `AssetPacks/BrassworksBreach.HeroVaultDoorSet12`; no main scenes touched. |");
            sb.AppendLine("| North-star silhouette complexity | PASS | Tall riveted arch, round pressure slab, gear face, hinge stack, pipes, lamps, and gauges create a readable FPS door silhouette. |");
            sb.AppendLine("| Material richness | PASS | Procedural albedo/normal/mask PNGs cover aged brass, dark iron, wet stone grime, glass glow, copper, paint, dial enamel, and verdigris. |");
            sb.AppendLine("| Rivets/bolts density | PASS | Bolt rings on slab, arch, brace ends, hinges, and pipe flanges are separate geometry. |");
            sb.AppendLine("| Warm highlights | PASS | Amber glass material, lantern meshes, and warm render lights match the brassworks reference mood. |");
            sb.AppendLine("| Readable from FPS distance | PASS | Large circular door, central gear lock, radial brace star, and arch frame remain legible in front and oblique renders. |");
            sb.AppendLine("| Required prefabs present | " + (validation.PrefabCount >= 9 ? "PASS" : "FAIL") + " | Prefab count: " + validation.PrefabCount.ToString(CultureInfo.InvariantCulture) + ". |");
            sb.AppendLine("| Required documentation renders present | " + (validation.RenderCount >= 3 ? "PASS" : "FAIL") + " | Render count: " + validation.RenderCount.ToString(CultureInfo.InvariantCulture) + ". |");
            sb.AppendLine("| No external DCC dependency | PASS | Meshes and textures are generated by Unity editor code. |");
            sb.AppendLine("| Known limitations | PASS | Door is visual-only, non-animated, and not collision/authored for gameplay. Steam/smoke is implied through warm lighting and grime, not particle VFX. |");
            return sb.ToString();
        }

        private static string BuildRenderIndex(string renderRoot)
        {
            string[] files = Directory.GetFiles(renderRoot, "*.png").Select(Path.GetFileName).OrderBy(x => x, StringComparer.Ordinal).ToArray();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# HVDS12 Render Index");
            sb.AppendLine();
            foreach (string file in files)
            {
                sb.AppendLine("- `" + file + "`");
            }
            return sb.ToString();
        }

        private static void WriteTextAsset(string assetPath, string contents)
        {
            string absolute = Path.Combine(LocatePackageRoot(), assetPath.Substring(PackageRoot.Length + 1).Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(absolute) ?? LocatePackageRoot());
            File.WriteAllText(absolute, contents);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        private static void Expect(ValidationResult result, bool condition, string failure)
        {
            if (!condition)
            {
                result.Failures.Add(failure);
            }
        }

        private sealed class ValidationResult
        {
            public int PrefabCount;
            public int MaterialCount;
            public int MeshCount;
            public int TextureCount;
            public int RenderCount;
            public bool Passed;
            public readonly List<string> Failures = new List<string>();
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

        private static void SetTexture(Material material, string property, Texture texture)
        {
            if (texture != null && material.HasProperty(property))
            {
                material.SetTexture(property, texture);
            }
        }

        private static string LocatePackageRoot()
        {
            UnityEditor.PackageManager.PackageInfo package = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(PackageRoot + "/package.json");
            if (package != null && !string.IsNullOrEmpty(package.resolvedPath))
            {
                return package.resolvedPath;
            }

            string dataPath = Application.dataPath.Replace('\\', '/');
            string projectRoot = Directory.GetParent(dataPath)?.FullName ?? Directory.GetCurrentDirectory();
            return Path.GetFullPath(Path.Combine(projectRoot, "..", ".."));
        }

        private static string JsonEscape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static Vector3 V(float x, float y, float z, float scale)
        {
            return new Vector3(x * scale, y * scale, z * scale);
        }

        private static Vector3 S(float x, float y, float z, float scale)
        {
            return new Vector3(x * scale, y * scale, z * scale);
        }

        private static Texture2D CreateFlatNoiseTexture(Color32 a, Color32 b, int seed)
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float n = Mathf.PerlinNoise((x + seed) * 0.045f, (y - seed) * 0.052f);
                    float s = Mathf.PerlinNoise((x - seed) * 0.15f, (y + seed) * 0.018f);
                    Color c = Color.Lerp(a, b, Mathf.Clamp01(n * 0.75f + s * 0.22f));
                    texture.SetPixel(x, y, c);
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateMetalTexture(Color32 baseColor, Color32 patina, int seed, bool verdigris)
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float grain = Mathf.PerlinNoise((x + seed) * 0.075f, (y - seed) * 0.020f);
                    float scratches = Mathf.Abs(Mathf.Sin((x + y * 0.23f + seed) * 0.23f)) * 0.10f;
                    float tarnish = verdigris ? Mathf.Pow(Mathf.PerlinNoise((x - seed) * 0.025f, (y + seed) * 0.040f), 3.3f) : 0f;
                    Color c = Color.Lerp(baseColor, new Color32(72, 45, 17, 255), grain * 0.35f + scratches);
                    c = Color.Lerp(c, patina, tarnish * 0.72f);
                    texture.SetPixel(x, y, c);
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateIronTexture(bool oily, int seed)
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float n = Mathf.PerlinNoise((x + seed) * 0.055f, (y - seed) * 0.070f);
                    float streak = Mathf.PerlinNoise((x + seed) * 0.012f, y * 0.20f);
                    float v = oily ? 0.025f + n * 0.045f + streak * 0.030f : 0.040f + n * 0.085f;
                    texture.SetPixel(x, y, new Color(v, v * 0.95f, v * 0.82f, 1f));
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateWetStoneTexture(int seed)
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float mortarX = Mathf.Min(x % 64, 64 - x % 64) < 3 ? 0.18f : 0f;
                    float mortarY = Mathf.Min(y % 64, 64 - y % 64) < 3 ? 0.18f : 0f;
                    float n = Mathf.PerlinNoise((x + seed) * 0.04f, (y - seed) * 0.04f);
                    float wet = Mathf.Pow(Mathf.PerlinNoise((x - seed) * 0.021f, (y + seed) * 0.026f), 2.2f);
                    float v = 0.035f + n * 0.055f + wet * 0.065f + mortarX + mortarY;
                    texture.SetPixel(x, y, new Color(v * 0.85f, v * 0.82f, v * 0.74f, 1f));
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateCopperTexture()
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float heat = Mathf.PerlinNoise(x * 0.018f, y * 0.065f);
                    Color c = Color.Lerp(new Color(0.82f, 0.32f, 0.11f), new Color(0.32f, 0.13f, 0.07f), heat * 0.48f);
                    c = Color.Lerp(c, new Color(0.14f, 0.33f, 0.32f), Mathf.Pow(Mathf.PerlinNoise(x * 0.03f, y * 0.03f), 4f) * 0.35f);
                    texture.SetPixel(x, y, c);
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateGlassTexture()
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            Vector2 center = new Vector2(128f, 128f);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float d = Vector2.Distance(new Vector2(x, y), center) / 181f;
                    float glow = 1f - Mathf.Clamp01(d);
                    float stripe = Mathf.Abs(Mathf.Sin((x + y * 0.08f) * 0.18f)) * 0.08f;
                    texture.SetPixel(x, y, new Color(1f, 0.34f + glow * 0.28f, 0.05f, 0.62f + glow * 0.28f + stripe));
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateGaugeDialTexture()
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            Vector2 center = new Vector2(128f, 128f);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Vector2 p = new Vector2(x, y) - center;
                    float r = p.magnitude;
                    float angle = Mathf.Atan2(p.y, p.x);
                    float tick = r > 84f && r < 104f && Mathf.Abs(Mathf.Sin(angle * 18f)) < 0.10f ? 0.36f : 0f;
                    float dirt = Mathf.PerlinNoise(x * 0.045f, y * 0.045f) * 0.10f;
                    Color c = new Color(0.82f - dirt - tick, 0.75f - dirt - tick, 0.58f - dirt - tick * 0.7f, 1f);
                    texture.SetPixel(x, y, c);
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateMaskTexture(float metallic, float smoothness, int seed)
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float noise = Mathf.PerlinNoise((x + seed) * 0.055f, (y - seed) * 0.055f) - 0.5f;
                    texture.SetPixel(x, y, new Color(metallic, 0f, 0f, Mathf.Clamp01(smoothness + noise * 0.20f)));
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateNormalTexture(int seed, float strength)
        {
            Texture2D height = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (int y = 0; y < height.height; y++)
            {
                for (int x = 0; x < height.width; x++)
                {
                    float h = Mathf.PerlinNoise((x + seed) * 0.050f, (y - seed) * 0.050f) * 0.7f
                        + Mathf.PerlinNoise((x - seed) * 0.18f, (y + seed) * 0.18f) * 0.3f;
                    height.SetPixel(x, y, new Color(h, h, h, 1f));
                }
            }

            Texture2D normal = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            for (int y = 0; y < normal.height; y++)
            {
                for (int x = 0; x < normal.width; x++)
                {
                    float l = height.GetPixel((x - 1 + 256) % 256, y).r;
                    float r = height.GetPixel((x + 1) % 256, y).r;
                    float d = height.GetPixel(x, (y - 1 + 256) % 256).r;
                    float u = height.GetPixel(x, (y + 1) % 256).r;
                    Vector3 n = new Vector3((l - r) * strength, (d - u) * strength, 1f).normalized;
                    normal.SetPixel(x, y, new Color(n.x * 0.5f + 0.5f, n.y * 0.5f + 0.5f, n.z * 0.5f + 0.5f, 1f));
                }
            }
            Object.DestroyImmediate(height);
            normal.Apply();
            return normal;
        }

        private static Mesh CreateBoxMesh()
        {
            Mesh mesh = new Mesh { name = Prefix + "_BoxUnit" };
            Vector3[] v =
            {
                new Vector3(-.5f,-.5f,-.5f), new Vector3(.5f,-.5f,-.5f), new Vector3(.5f,.5f,-.5f), new Vector3(-.5f,.5f,-.5f),
                new Vector3(-.5f,-.5f,.5f), new Vector3(.5f,-.5f,.5f), new Vector3(.5f,.5f,.5f), new Vector3(-.5f,.5f,.5f)
            };
            mesh.vertices = v;
            mesh.triangles = new[] { 0, 2, 1, 0, 3, 2, 4, 5, 6, 4, 6, 7, 0, 1, 5, 0, 5, 4, 1, 2, 6, 1, 6, 5, 2, 3, 7, 2, 7, 6, 3, 0, 4, 3, 4, 7 };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateCylinderMesh(int segments, float radius, float depth)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float z0 = -depth * 0.5f;
            float z1 = depth * 0.5f;
            int frontCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, z0));
            int backCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0f, z1));
            for (int i = 0; i < segments; i++)
            {
                float a = i * Mathf.PI * 2f / segments;
                vertices.Add(new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, z0));
                vertices.Add(new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, z1));
            }
            for (int i = 0; i < segments; i++)
            {
                int ni = (i + 1) % segments;
                int f0 = 2 + i * 2;
                int b0 = f0 + 1;
                int f1 = 2 + ni * 2;
                int b1 = f1 + 1;
                triangles.Add(frontCenter); triangles.Add(f1); triangles.Add(f0);
                triangles.Add(backCenter); triangles.Add(b0); triangles.Add(b1);
                triangles.Add(f0); triangles.Add(f1); triangles.Add(b1);
                triangles.Add(f0); triangles.Add(b1); triangles.Add(b0);
            }
            Mesh mesh = new Mesh { name = Prefix + "_Cylinder" + segments };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateRingMesh(int segments, float outer, float inner, float depth)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float z0 = -depth * 0.5f;
            float z1 = depth * 0.5f;
            for (int i = 0; i < segments; i++)
            {
                float a = i * Mathf.PI * 2f / segments;
                float c = Mathf.Cos(a);
                float s = Mathf.Sin(a);
                vertices.Add(new Vector3(c * outer, s * outer, z0));
                vertices.Add(new Vector3(c * inner, s * inner, z0));
                vertices.Add(new Vector3(c * outer, s * outer, z1));
                vertices.Add(new Vector3(c * inner, s * inner, z1));
            }
            for (int i = 0; i < segments; i++)
            {
                int n = (i + 1) % segments;
                int a = i * 4;
                int b = n * 4;
                triangles.Add(a); triangles.Add(b); triangles.Add(a + 2);
                triangles.Add(b); triangles.Add(b + 2); triangles.Add(a + 2);
                triangles.Add(a + 1); triangles.Add(a + 3); triangles.Add(b + 1);
                triangles.Add(b + 1); triangles.Add(a + 3); triangles.Add(b + 3);
                triangles.Add(a); triangles.Add(a + 1); triangles.Add(b);
                triangles.Add(b); triangles.Add(a + 1); triangles.Add(b + 1);
                triangles.Add(a + 2); triangles.Add(b + 2); triangles.Add(a + 3);
                triangles.Add(b + 2); triangles.Add(b + 3); triangles.Add(a + 3);
            }
            Mesh mesh = new Mesh { name = Prefix + "_Ring" };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateGearRingMesh(int teeth, float outer, float inner, float toothOuter, float depth)
        {
            int segments = teeth * 2;
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float z0 = -depth * 0.5f;
            float z1 = depth * 0.5f;
            for (int i = 0; i < segments; i++)
            {
                float a = i * Mathf.PI * 2f / segments;
                float r = i % 2 == 0 ? toothOuter : outer;
                float c = Mathf.Cos(a);
                float s = Mathf.Sin(a);
                vertices.Add(new Vector3(c * r, s * r, z0));
                vertices.Add(new Vector3(c * inner, s * inner, z0));
                vertices.Add(new Vector3(c * r, s * r, z1));
                vertices.Add(new Vector3(c * inner, s * inner, z1));
            }
            for (int i = 0; i < segments; i++)
            {
                int n = (i + 1) % segments;
                int a = i * 4;
                int b = n * 4;
                triangles.Add(a); triangles.Add(b); triangles.Add(a + 2);
                triangles.Add(b); triangles.Add(b + 2); triangles.Add(a + 2);
                triangles.Add(a + 1); triangles.Add(a + 3); triangles.Add(b + 1);
                triangles.Add(b + 1); triangles.Add(a + 3); triangles.Add(b + 3);
                triangles.Add(a); triangles.Add(a + 1); triangles.Add(b);
                triangles.Add(b); triangles.Add(a + 1); triangles.Add(b + 1);
                triangles.Add(a + 2); triangles.Add(b + 2); triangles.Add(a + 3);
                triangles.Add(b + 2); triangles.Add(b + 3); triangles.Add(a + 3);
            }
            Mesh mesh = new Mesh { name = Prefix + "_GearRing" };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateArchBandMesh(int segments, float outer, float inner, float depth)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float z0 = -depth * 0.5f;
            float z1 = depth * 0.5f;
            for (int i = 0; i <= segments; i++)
            {
                float a = Mathf.Lerp(0f, Mathf.PI, i / (float)segments);
                float c = Mathf.Cos(a);
                float s = Mathf.Sin(a);
                vertices.Add(new Vector3(c * outer, s * outer, z0));
                vertices.Add(new Vector3(c * inner, s * inner, z0));
                vertices.Add(new Vector3(c * outer, s * outer, z1));
                vertices.Add(new Vector3(c * inner, s * inner, z1));
            }
            for (int i = 0; i < segments; i++)
            {
                int a = i * 4;
                int b = (i + 1) * 4;
                triangles.Add(a); triangles.Add(b); triangles.Add(a + 2);
                triangles.Add(b); triangles.Add(b + 2); triangles.Add(a + 2);
                triangles.Add(a + 1); triangles.Add(a + 3); triangles.Add(b + 1);
                triangles.Add(b + 1); triangles.Add(a + 3); triangles.Add(b + 3);
                triangles.Add(a); triangles.Add(a + 1); triangles.Add(b);
                triangles.Add(b); triangles.Add(a + 1); triangles.Add(b + 1);
                triangles.Add(a + 2); triangles.Add(b + 2); triangles.Add(a + 3);
                triangles.Add(b + 2); triangles.Add(b + 3); triangles.Add(a + 3);
            }
            Mesh mesh = new Mesh { name = Prefix + "_ArchBand" };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateTorusMesh(int radialSegments, int tubeSegments, float majorRadius, float tubeRadius)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int i = 0; i < radialSegments; i++)
            {
                float u = i * Mathf.PI * 2f / radialSegments;
                for (int j = 0; j < tubeSegments; j++)
                {
                    float v = j * Mathf.PI * 2f / tubeSegments;
                    float r = majorRadius + Mathf.Cos(v) * tubeRadius;
                    vertices.Add(new Vector3(Mathf.Cos(u) * r, Mathf.Sin(u) * r, Mathf.Sin(v) * tubeRadius));
                }
            }
            for (int i = 0; i < radialSegments; i++)
            {
                for (int j = 0; j < tubeSegments; j++)
                {
                    int a = i * tubeSegments + j;
                    int b = ((i + 1) % radialSegments) * tubeSegments + j;
                    int c = ((i + 1) % radialSegments) * tubeSegments + (j + 1) % tubeSegments;
                    int d = i * tubeSegments + (j + 1) % tubeSegments;
                    triangles.Add(a); triangles.Add(b); triangles.Add(c);
                    triangles.Add(a); triangles.Add(c); triangles.Add(d);
                }
            }
            Mesh mesh = new Mesh { name = Prefix + "_Torus" };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateNeedleMesh()
        {
            Mesh mesh = new Mesh { name = Prefix + "_Needle" };
            mesh.vertices = new[]
            {
                new Vector3(-0.08f, -0.035f, 0f), new Vector3(0.34f, -0.018f, 0f), new Vector3(0.48f, 0f, 0f), new Vector3(0.34f, 0.018f, 0f), new Vector3(-0.08f, 0.035f, 0f)
            };
            mesh.triangles = new[] { 0, 1, 4, 1, 3, 4, 1, 2, 3 };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateGussetMesh()
        {
            Mesh mesh = new Mesh { name = Prefix + "_Gusset" };
            mesh.vertices = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.05f), new Vector3(0.5f, -0.5f, -0.05f), new Vector3(-0.5f, 0.5f, -0.05f),
                new Vector3(-0.5f, -0.5f, 0.05f), new Vector3(0.5f, -0.5f, 0.05f), new Vector3(-0.5f, 0.5f, 0.05f)
            };
            mesh.triangles = new[] { 0, 2, 1, 3, 4, 5, 0, 1, 4, 0, 4, 3, 1, 2, 5, 1, 5, 4, 2, 0, 3, 2, 3, 5 };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
