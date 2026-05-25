using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace BrassworksBreach.MechanicalSentinelHeroSet10.Editor
{
    public static class MechanicalSentinelHeroSet10Generator
    {
        private const string PackageName = "com.brassworks.sidecar.mechanical-sentinel-hero-set10";
        private const string DisplayName = "Brassworks Breach Mechanical Sentinel Hero Set 10";
        private const string Version = "0.1.55-p001";
        private const string PackageAssetRoot = "Packages/" + PackageName;
        private const int TextureSize = 512;
        private static readonly string RepoRoot = Environment.GetEnvironmentVariable("BRASSWORKS_REPO_ROOT") ?? "D:/__MY APPS/Unity Doom";
        private static readonly string AssetProductionRoot = NormalizePath(Path.Combine(RepoRoot, "Documentation/AssetProduction/V0_1_55_MechanicalSentinelHeroSet10"));
        private static readonly string ConceptRenderRoot = NormalizePath(Path.Combine(RepoRoot, "Documentation/ConceptRenders/V0_1_55_MechanicalSentinelHeroSet10"));
        private static readonly string PlanningRoot = NormalizePath(Path.Combine(RepoRoot, "Documentation/Planning/V0_1_55_MechanicalSentinelHeroSet10ImportReadiness"));
        private static readonly string QaRoot = NormalizePath(Path.Combine(RepoRoot, "Documentation/QA/V0_1_55_MechanicalSentinelHeroSet10ImportReadiness"));

        private static string _packageRoot = string.Empty;
        private static readonly Dictionary<string, string> MaterialPaths = new Dictionary<string, string>();
        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private static readonly List<PrefabRecord> Prefabs = new List<PrefabRecord>();
        private static readonly List<PreviewRecord> Previews = new List<PreviewRecord>();
        private static readonly List<TextureRecord> Textures = new List<TextureRecord>();
        private static readonly List<MeshRecord> MeshRecords = new List<MeshRecord>();

        [MenuItem("Brassworks Breach/Sidecars/Generate Mechanical Sentinel Hero Set 10")]
        public static void GeneratePackageAssets()
        {
            _packageRoot = ResolvePackageRoot();
            if (string.IsNullOrWhiteSpace(_packageRoot))
            {
                throw new InvalidOperationException("Unable to resolve package root for " + PackageName);
            }

            Debug.Log("MSH10_GENERATE_BEGIN " + _packageRoot);
            ResetOwnedGeneratedRoots();
            EnsureDirectory(AssetProductionRoot);
            EnsureDirectory(ConceptRenderRoot);
            EnsureDirectory(PlanningRoot);
            EnsureDirectory(QaRoot);
            WritePackageMetadata();
            AssetDatabase.Refresh();

            GenerateTexturesAndMaterials();
            GenerateMeshes();
            GeneratePrefabs();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GeneratePreviewPngs();
            WriteCatalogAndReports();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("MSH10_GENERATE_PASS prefabs=" + Prefabs.Count + " materials=" + Materials.Count + " textures=" + Textures.Count + " meshes=" + MeshRecords.Count + " previews=" + Previews.Count);
        }

        private static string ResolvePackageRoot()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(PackageAssetRoot + "/package.json");
            if (packageInfo != null && !string.IsNullOrWhiteSpace(packageInfo.resolvedPath))
            {
                return NormalizePath(packageInfo.resolvedPath);
            }

            return NormalizePath(Path.Combine(RepoRoot, "AssetPacks/BrassworksBreach.MechanicalSentinelHeroSet10"));
        }

        private static void ResetOwnedGeneratedRoots()
        {
            DeleteDirectoryIfExists(Path.Combine(_packageRoot, "Runtime"));
            DeleteDirectoryIfExists(Path.Combine(_packageRoot, "Documentation~"));
            DeleteDirectoryIfExists(Path.Combine(_packageRoot, "Samples~"));
            DeleteDirectoryIfExists(ConceptRenderRoot);
            DeleteDirectoryIfExists(PlanningRoot);
            DeleteDirectoryIfExists(QaRoot);
            DeleteFilesByPattern(AssetProductionRoot, "MSH10_*.md");
            DeleteFilesByPattern(AssetProductionRoot, "MSH10_*.json");
            DeleteFilesByPattern(AssetProductionRoot, "MSH10_*.txt");

            foreach (var relative in new[] { "Runtime/Materials", "Runtime/Textures", "Runtime/Meshes", "Runtime/Prefabs", "Runtime/Previews", "Runtime/Metadata", "Documentation~/Previews", "Samples~/PreviewScene" })
            {
                EnsureDirectory(Path.Combine(_packageRoot, relative));
            }
        }

        private static void WritePackageMetadata()
        {
            var packageJson = @"{
  ""name"": """ + PackageName + @""",
  ""version"": """ + Version + @""",
  ""displayName"": """ + DisplayName + @""",
  ""description"": ""Unity-only visual sidecar package for a reusable steampunk mechanical sentinel hero enemy assembly and separated future-rigging component family."",
  ""unity"": ""6000.4"",
  ""author"": {
    ""name"": ""Brassworks Breach Sidecar Production""
  },
  ""keywords"": [
    ""brassworks"",
    ""sidecar"",
    ""visual-only"",
    ""steampunk"",
    ""mechanical-enemy"",
    ""sentinel"",
    ""boiler"",
    ""furnace"",
    ""flywheel"",
    ""saw"",
    ""claw"",
    ""rigging-sockets""
  ],
  ""dependencies"": {},
  ""samples"": [
    {
      ""displayName"": ""Preview Notes"",
      ""description"": ""Import-safe notes for the Mechanical Sentinel Hero Set 10 visual-only package."",
      ""path"": ""Samples~/PreviewScene""
    }
  ]
}
";
            File.WriteAllText(Path.Combine(_packageRoot, "package.json"), packageJson);
            File.WriteAllText(Path.Combine(_packageRoot, "README.md"), "# Mechanical Sentinel Hero Set 10\n\nUnity-only visual sidecar package for a steampunk mechanical monster hero assembly. Assets are visual-only and intentionally contain no gameplay authority, colliders, AI, animation controllers, audio, lighting ownership, or route logic.\n");
            File.WriteAllText(Path.Combine(_packageRoot, "Samples~/PreviewScene/README.md"), "# Preview Notes\n\nImport this package as a local package and inspect the Runtime/Prefabs folder. The hero assembly includes named SOCKET_ transforms for future rigging and gameplay hookup.\n");
        }

        private static void GenerateTexturesAndMaterials()
        {
            var specs = new[]
            {
                new MaterialSpec("AgedBrassPatina", "aged_brass", new Color(0.72f, 0.46f, 0.18f), 0.86f, 0.46f, Color.black, false, false),
                new MaterialSpec("PolishedBrassEdgewear", "polished_brass_edgewear", new Color(0.98f, 0.72f, 0.27f), 0.9f, 0.58f, Color.black, false, false),
                new MaterialSpec("BlackenedRivetedIron", "blackened_iron", new Color(0.06f, 0.055f, 0.05f), 0.84f, 0.3f, Color.black, false, false),
                new MaterialSpec("OilWetIron", "oil_wet_iron", new Color(0.035f, 0.032f, 0.029f), 0.88f, 0.72f, Color.black, false, false),
                new MaterialSpec("BurnishedCopperTube", "burnished_copper", new Color(0.68f, 0.28f, 0.12f), 0.88f, 0.5f, Color.black, false, false),
                new MaterialSpec("HeatStainedSawSteel", "heat_stained_steel", new Color(0.52f, 0.49f, 0.43f), 0.95f, 0.38f, Color.black, false, false),
                new MaterialSpec("IvoryGaugeEnamel", "ivory_gauge_enamel", new Color(0.82f, 0.73f, 0.57f), 0.08f, 0.36f, Color.black, false, false),
                new MaterialSpec("RedNeedleEnamel", "red_needle_enamel", new Color(0.76f, 0.05f, 0.025f), 0.18f, 0.44f, Color.black, false, false),
                new MaterialSpec("AmberFurnaceGlow", "amber_furnace_glow", new Color(1.0f, 0.46f, 0.07f), 0.0f, 0.62f, new Color(1.0f, 0.34f, 0.04f), false, true),
                new MaterialSpec("SmokedAmberGlass", "smoked_amber_glass", new Color(0.95f, 0.44f, 0.08f, 0.54f), 0.02f, 0.86f, new Color(0.7f, 0.18f, 0.02f), true, true),
                new MaterialSpec("SootGrimeDeposit", "soot_grime", new Color(0.025f, 0.022f, 0.019f), 0.0f, 0.14f, Color.black, false, false),
                new MaterialSpec("SteamMistTranslucent", "steam_mist", new Color(0.68f, 0.66f, 0.6f, 0.35f), 0.0f, 0.18f, Color.black, true, false)
            };

            foreach (var spec in specs)
            {
                var albedo = GenerateTexture(spec, "Albedo", false);
                var normal = GenerateTexture(spec, "Normal", true);
                var metallic = GenerateMetallicSmoothnessTexture(spec);
                CreateMaterial(spec, albedo, normal, metallic);
            }
        }

        private static void GenerateMeshes()
        {
            CreateMeshAsset("MSH10_MESH_Box_Unit", CreateBoxMesh());
            CreateMeshAsset("MSH10_MESH_Cylinder16_Y", CreateCylinderMesh(16));
            CreateMeshAsset("MSH10_MESH_Cylinder24_Y", CreateCylinderMesh(24));
            CreateMeshAsset("MSH10_MESH_Cylinder32_Y", CreateCylinderMesh(32));
            CreateMeshAsset("MSH10_MESH_Cylinder48_Y", CreateCylinderMesh(48));
            CreateMeshAsset("MSH10_MESH_Torus32", CreateTorusMesh(32, 8, 0.5f, 0.065f));
            CreateMeshAsset("MSH10_MESH_FlywheelTorus48", CreateTorusMesh(48, 10, 0.5f, 0.055f));
            CreateMeshAsset("MSH10_MESH_SawBlade32", CreateSawBladeMesh(32));
            CreateMeshAsset("MSH10_MESH_ClawBlade", CreateClawBladeMesh());
            CreateMeshAsset("MSH10_MESH_GaugeNeedle", CreateGaugeNeedleMesh());
            CreateMeshAsset("MSH10_MESH_HexBolt", CreateCylinderMesh(6));
            CreateMeshAsset("MSH10_MESH_SteamCone", CreateConeMesh(24));
            CreateMeshAsset("MSH10_MESH_BracketGusset", CreateGussetMesh());
        }

        private static void GeneratePrefabs()
        {
            SavePrefab("MSH10_MechanicalSentinelHeroAssembly", "hero_assembly", "Full mechanical sentinel: boiler torso, furnace head, chest gauge, back flywheel, saw arm, claw arm, piston legs, copper tubes, amber glow cores, rivets, and future rigging sockets.", root =>
            {
                BuildFullSentinel(root.transform);
                AddSocketSkeleton(root.transform);
            });

            SavePrefab("MSH10_BoilerTorso_Module", "boiler_torso", "Standalone boiler torso with brass bands, blackened iron frame, chest gauge mount, rivet fields, and future spine/shoulder sockets.", root =>
            {
                BuildTorso(root.transform);
                AddSocket(root.transform, "SOCKET_SpineRoot", new Vector3(0, 0.15f, 0));
                AddSocket(root.transform, "SOCKET_ChestGauge", new Vector3(0, 0.66f, -0.62f));
                AddSocket(root.transform, "SOCKET_LeftShoulder", new Vector3(-0.72f, 0.92f, 0));
                AddSocket(root.transform, "SOCKET_RightShoulder", new Vector3(0.72f, 0.92f, 0));
            });

            SavePrefab("MSH10_FurnaceHead_Module", "furnace_head", "Standalone furnace head with amber glass eyes, jaw grille, crown steam stacks, cheek rivets, and neck socket.", root =>
            {
                BuildHead(root.transform);
                AddSocket(root.transform, "SOCKET_Neck", new Vector3(0, 0.02f, 0));
                AddSocket(root.transform, "SOCKET_EyeGlow_Left", new Vector3(-0.18f, 0.2f, -0.35f));
                AddSocket(root.transform, "SOCKET_EyeGlow_Right", new Vector3(0.18f, 0.2f, -0.35f));
            });

            SavePrefab("MSH10_GaugeChest_Module", "pressure_gauge_chest", "Readable chest pressure gauge assembly with brass bezel, ivory enamel, red danger needle, and amber sight glass.", root => BuildChestGauge(root.transform));
            SavePrefab("MSH10_BackFlywheel_Module", "back_flywheel", "Back-mounted flywheel and black iron shoulder frame, separated for future idle/spin animation hookup.", root => BuildFlywheelBackpack(root.transform));
            SavePrefab("MSH10_LeftSawArm_Module", "saw_blade_arm", "Left tool arm with piston bicep, brass elbow hub, heat-stained circular saw, and SOCKET_LeftWristSaw.", root => BuildSawArm(root.transform, 1f));
            SavePrefab("MSH10_RightClawArm_Module", "claw_arm", "Right manipulator arm with paired pistons, three claw blades, pressure tubes, and SOCKET_RightWristClaw.", root => BuildClawArm(root.transform, -1f));
            SavePrefab("MSH10_PistonLegPair_Module", "piston_legs", "Separated piston leg pair with hip hubs, knee cylinders, iron feet, brass caps, and future locomotion sockets.", root => BuildLegs(root.transform));
            SavePrefab("MSH10_CopperTubeHarness_Module", "copper_tube_harness", "Reusable copper tube harness with valve collars and pressure leads for chest-to-shoulder routing.", root => BuildTubeHarness(root.transform));
            SavePrefab("MSH10_RivetBandSet_Module", "rivet_band_set", "Reusable rivet bands and bolt rows for boiler torso, door, or machinery dressing.", root => BuildRivetBandSet(root.transform));
            SavePrefab("MSH10_AmberGlowCore_Module", "amber_glow_core", "Reusable amber furnace core with glass tube, brass collars, and emission material for enemy weak points.", root => BuildAmberGlowCore(root.transform));
            SavePrefab("MSH10_SteamShoulderStack_Module", "steam_shoulder_stack", "Paired shoulder exhaust stacks with soot collars, brass caps, and translucent steam puffs.", root => BuildSteamStacks(root.transform));
            SavePrefab("MSH10_RigSocketSkeleton_Module", "rig_socket_skeleton", "Named empty SOCKET transforms for future rigging without animation dependencies.", AddSocketSkeleton);
            SavePrefab("MSH10_MaterialPaletteBoard", "material_palette", "Material palette board for visual review of brass, copper, iron, glass, furnace glow, grime, and steam treatments.", BuildMaterialPaletteBoard);
        }

        private static void BuildFullSentinel(Transform parent)
        {
            BuildTorso(parent);
            BuildHead(parent);
            BuildChestGauge(parent);
            BuildFlywheelBackpack(parent);
            BuildSawArm(parent, 1f);
            BuildClawArm(parent, -1f);
            BuildLegs(parent);
            BuildTubeHarness(parent);
            BuildSteamStacks(parent);
        }

        private static void BuildTorso(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_BoilerTorso", Vector3.zero);
            Part("BoilerShell_AgedBrass", section, "MSH10_MESH_Cylinder48_Y", "AgedBrassPatina", new Vector3(0, 0.55f, 0), Quaternion.identity, new Vector3(0.72f, 1.38f, 0.72f));
            Part("BoilerBackplate_BlackIron", section, "MSH10_MESH_Box_Unit", "BlackenedRivetedIron", new Vector3(0, 0.58f, 0.42f), Quaternion.identity, new Vector3(1.2f, 1.35f, 0.15f));
            Part("ChestIronFrame", section, "MSH10_MESH_Box_Unit", "BlackenedRivetedIron", new Vector3(0, 0.55f, -0.68f), Quaternion.identity, new Vector3(1.02f, 0.88f, 0.1f));
            Part("LowerBoilerCap", section, "MSH10_MESH_Cylinder48_Y", "PolishedBrassEdgewear", new Vector3(0, -0.18f, 0), Quaternion.identity, new Vector3(0.8f, 0.13f, 0.8f));
            Part("UpperBoilerCap", section, "MSH10_MESH_Cylinder48_Y", "PolishedBrassEdgewear", new Vector3(0, 1.28f, 0), Quaternion.identity, new Vector3(0.78f, 0.12f, 0.78f));

            foreach (var y in new[] { 0.05f, 0.46f, 0.92f })
            {
                Part("BrassBand_" + y.ToString("0.00", CultureInfo.InvariantCulture), section, "MSH10_MESH_Torus32", "PolishedBrassEdgewear", new Vector3(0, y, 0), Quaternion.identity, new Vector3(1.5f, 1.5f, 1.5f));
            }

            for (var i = 0; i < 18; i++)
            {
                var x = Mathf.Lerp(-0.46f, 0.46f, i / 17f);
                AddFrontBolt(section, "ChestRivetTop_" + i, new Vector3(x, 0.98f, -0.75f), 0.035f);
                AddFrontBolt(section, "ChestRivetBottom_" + i, new Vector3(x, 0.18f, -0.75f), 0.035f);
            }

            for (var i = 0; i < 12; i++)
            {
                var angle = i * Mathf.PI * 2f / 12f;
                AddRadialBolt(section, "BoilerCrownRivet_" + i, new Vector3(Mathf.Cos(angle) * 0.56f, 1.34f, Mathf.Sin(angle) * 0.56f), angle, 0.035f);
                AddRadialBolt(section, "BoilerBellyRivet_" + i, new Vector3(Mathf.Cos(angle) * 0.56f, 0.03f, Mathf.Sin(angle) * 0.56f), angle, 0.03f);
            }
        }

        private static void BuildHead(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_FurnaceHead", new Vector3(0, 1.72f, -0.02f));
            Part("HeadBoilerShell", section, "MSH10_MESH_Cylinder32_Y", "BlackenedRivetedIron", Vector3.zero, Quaternion.identity, new Vector3(0.48f, 0.58f, 0.42f));
            Part("HeadBrassBrow", section, "MSH10_MESH_Box_Unit", "PolishedBrassEdgewear", new Vector3(0, 0.15f, -0.36f), Quaternion.identity, new Vector3(0.82f, 0.11f, 0.08f));
            Part("JawGrilleFrame", section, "MSH10_MESH_Box_Unit", "AgedBrassPatina", new Vector3(0, -0.2f, -0.38f), Quaternion.identity, new Vector3(0.62f, 0.25f, 0.08f));
            for (var i = 0; i < 5; i++)
            {
                Part("JawGrilleBar_" + i, section, "MSH10_MESH_Box_Unit", "BlackenedRivetedIron", new Vector3(Mathf.Lerp(-0.24f, 0.24f, i / 4f), -0.2f, -0.45f), Quaternion.identity, new Vector3(0.035f, 0.28f, 0.05f));
            }

            foreach (var x in new[] { -0.18f, 0.18f })
            {
                Part("FurnaceEyeBezel_" + x, section, "MSH10_MESH_Cylinder24_Y", "PolishedBrassEdgewear", new Vector3(x, 0.07f, -0.42f), Quaternion.Euler(90f, 0, 0), new Vector3(0.17f, 0.065f, 0.17f));
                Part("FurnaceEyeGlow_" + x, section, "MSH10_MESH_Cylinder24_Y", "AmberFurnaceGlow", new Vector3(x, 0.07f, -0.47f), Quaternion.Euler(90f, 0, 0), new Vector3(0.105f, 0.035f, 0.105f));
            }

            foreach (var x in new[] { -0.26f, 0.26f })
            {
                Part("CrownSteamStack_" + x, section, "MSH10_MESH_Cylinder16_Y", "BurnishedCopperTube", new Vector3(x, 0.47f, 0.02f), Quaternion.identity, new Vector3(0.11f, 0.46f, 0.11f));
                Part("CrownStackCap_" + x, section, "MSH10_MESH_Cylinder16_Y", "PolishedBrassEdgewear", new Vector3(x, 0.72f, 0.02f), Quaternion.identity, new Vector3(0.14f, 0.06f, 0.14f));
            }
        }

        private static void BuildChestGauge(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_ChestPressureGauge", new Vector3(0, 0.72f, -0.81f));
            Part("GaugeIronPlate", section, "MSH10_MESH_Box_Unit", "BlackenedRivetedIron", new Vector3(0, 0, 0.08f), Quaternion.identity, new Vector3(0.52f, 0.52f, 0.08f));
            Part("GaugeBrassBezel", section, "MSH10_MESH_Cylinder48_Y", "PolishedBrassEdgewear", Vector3.zero, Quaternion.Euler(90f, 0, 0), new Vector3(0.31f, 0.07f, 0.31f));
            Part("GaugeIvoryFace", section, "MSH10_MESH_Cylinder48_Y", "IvoryGaugeEnamel", new Vector3(0, 0, -0.055f), Quaternion.Euler(90f, 0, 0), new Vector3(0.245f, 0.025f, 0.245f));
            Part("GaugeAmberGlass", section, "MSH10_MESH_Cylinder48_Y", "SmokedAmberGlass", new Vector3(0, 0, -0.08f), Quaternion.Euler(90f, 0, 0), new Vector3(0.255f, 0.01f, 0.255f));
            Part("GaugeNeedleDanger", section, "MSH10_MESH_GaugeNeedle", "RedNeedleEnamel", new Vector3(0, 0, -0.105f), Quaternion.Euler(0, 0, -37f), new Vector3(0.78f, 0.78f, 0.78f));
            for (var i = 0; i < 13; i++)
            {
                var a = Mathf.Lerp(215f, -35f, i / 12f) * Mathf.Deg2Rad;
                Part("GaugeTick_" + i, section, "MSH10_MESH_Box_Unit", "BlackenedRivetedIron", new Vector3(Mathf.Cos(a) * 0.18f, Mathf.Sin(a) * 0.18f, -0.11f), Quaternion.Euler(0, 0, a * Mathf.Rad2Deg + 90f), new Vector3(0.012f, 0.055f, 0.012f));
            }
        }

        private static void BuildFlywheelBackpack(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_BackFlywheel", new Vector3(0, 0.82f, 0.73f));
            Part("FlywheelOuterRing", section, "MSH10_MESH_FlywheelTorus48", "BlackenedRivetedIron", Vector3.zero, Quaternion.Euler(90f, 0, 0), new Vector3(1.15f, 1.15f, 1.15f));
            Part("FlywheelInnerRing", section, "MSH10_MESH_Torus32", "PolishedBrassEdgewear", Vector3.zero, Quaternion.Euler(90f, 0, 0), new Vector3(0.68f, 0.68f, 0.68f));
            Part("FlywheelHub", section, "MSH10_MESH_Cylinder32_Y", "AgedBrassPatina", Vector3.zero, Quaternion.Euler(90f, 0, 0), new Vector3(0.22f, 0.16f, 0.22f));
            for (var i = 0; i < 8; i++)
            {
                var a = i * Mathf.PI * 2f / 8f;
                var p = new Vector3(Mathf.Cos(a) * 0.27f, Mathf.Sin(a) * 0.27f, -0.02f);
                Part("FlywheelSpoke_" + i, section, "MSH10_MESH_Box_Unit", "AgedBrassPatina", p, Quaternion.Euler(0, 0, a * Mathf.Rad2Deg), new Vector3(0.43f, 0.04f, 0.045f));
                AddFrontBolt(section, "FlywheelRivet_" + i, new Vector3(Mathf.Cos(a) * 0.5f, Mathf.Sin(a) * 0.5f, -0.08f), 0.025f);
            }
        }

        private static void BuildSawArm(Transform parent, float side)
        {
            var section = Section(parent, "MSH10_SECTION_LeftSawArm", new Vector3(-side * 0.86f, 0.72f, -0.04f));
            Part("SawShoulderHub", section, "MSH10_MESH_Cylinder32_Y", "PolishedBrassEdgewear", Vector3.zero, Quaternion.Euler(0, 0, 90f), new Vector3(0.22f, 0.28f, 0.22f));
            Part("SawUpperPiston", section, "MSH10_MESH_Cylinder16_Y", "BlackenedRivetedIron", new Vector3(-side * 0.34f, -0.1f, -0.02f), Quaternion.Euler(0, 0, 54f * side), new Vector3(0.08f, 0.7f, 0.08f));
            Part("SawCopperHydraulic", section, "MSH10_MESH_Cylinder16_Y", "BurnishedCopperTube", new Vector3(-side * 0.28f, -0.18f, -0.12f), Quaternion.Euler(0, 0, 54f * side), new Vector3(0.045f, 0.78f, 0.045f));
            Part("SawElbowHub", section, "MSH10_MESH_Cylinder24_Y", "AgedBrassPatina", new Vector3(-side * 0.58f, -0.42f, -0.03f), Quaternion.Euler(0, 0, 90f), new Vector3(0.17f, 0.24f, 0.17f));
            Part("SawForearmIron", section, "MSH10_MESH_Cylinder16_Y", "OilWetIron", new Vector3(-side * 0.8f, -0.67f, -0.04f), Quaternion.Euler(0, 0, 38f * side), new Vector3(0.08f, 0.48f, 0.08f));
            Part("SawBladeDisc", section, "MSH10_MESH_SawBlade32", "HeatStainedSawSteel", new Vector3(-side * 1.08f, -0.9f, -0.08f), Quaternion.Euler(0, 0, 0), new Vector3(0.45f, 0.45f, 0.45f));
            Part("SawBladeHub", section, "MSH10_MESH_Cylinder24_Y", "PolishedBrassEdgewear", new Vector3(-side * 1.08f, -0.9f, -0.13f), Quaternion.Euler(90f, 0, 0), new Vector3(0.09f, 0.08f, 0.09f));
            AddSocket(section, "SOCKET_LeftWristSaw", new Vector3(-side * 1.08f, -0.9f, -0.08f));
        }

        private static void BuildClawArm(Transform parent, float side)
        {
            var section = Section(parent, "MSH10_SECTION_RightClawArm", new Vector3(-side * 0.86f, 0.72f, -0.04f));
            Part("ClawShoulderHub", section, "MSH10_MESH_Cylinder32_Y", "PolishedBrassEdgewear", Vector3.zero, Quaternion.Euler(0, 0, 90f), new Vector3(0.22f, 0.28f, 0.22f));
            Part("ClawUpperPistonA", section, "MSH10_MESH_Cylinder16_Y", "BlackenedRivetedIron", new Vector3(-side * 0.33f, -0.1f, -0.02f), Quaternion.Euler(0, 0, -54f * side), new Vector3(0.08f, 0.68f, 0.08f));
            Part("ClawUpperPistonB", section, "MSH10_MESH_Cylinder16_Y", "BurnishedCopperTube", new Vector3(-side * 0.26f, -0.22f, -0.14f), Quaternion.Euler(0, 0, -54f * side), new Vector3(0.045f, 0.76f, 0.045f));
            Part("ClawPalm", section, "MSH10_MESH_Box_Unit", "BlackenedRivetedIron", new Vector3(-side * 0.85f, -0.72f, -0.08f), Quaternion.Euler(0, 0, 8f * side), new Vector3(0.28f, 0.24f, 0.22f));
            for (var i = -1; i <= 1; i++)
            {
                Part("ClawBlade_" + i, section, "MSH10_MESH_ClawBlade", "HeatStainedSawSteel", new Vector3(-side * 1.02f, -0.74f + i * 0.1f, -0.1f), Quaternion.Euler(0, 0, (-18f + i * 18f) * side), new Vector3(0.35f, 0.35f, 0.35f));
            }
            AddSocket(section, "SOCKET_RightWristClaw", new Vector3(-side * 0.88f, -0.72f, -0.08f));
        }

        private static void BuildLegs(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_PistonLegs", Vector3.zero);
            foreach (var x in new[] { -0.32f, 0.32f })
            {
                Part("HipHub_" + x, section, "MSH10_MESH_Cylinder24_Y", "AgedBrassPatina", new Vector3(x, -0.22f, 0.02f), Quaternion.Euler(0, 0, 90f), new Vector3(0.16f, 0.22f, 0.16f));
                Part("UpperLegPiston_" + x, section, "MSH10_MESH_Cylinder16_Y", "OilWetIron", new Vector3(x, -0.62f, 0), Quaternion.identity, new Vector3(0.095f, 0.58f, 0.095f));
                Part("LegCopperBypass_" + x, section, "MSH10_MESH_Cylinder16_Y", "BurnishedCopperTube", new Vector3(x + Mathf.Sign(x) * 0.08f, -0.62f, -0.12f), Quaternion.identity, new Vector3(0.04f, 0.6f, 0.04f));
                Part("KneeHub_" + x, section, "MSH10_MESH_Cylinder24_Y", "PolishedBrassEdgewear", new Vector3(x, -0.95f, 0), Quaternion.Euler(0, 0, 90f), new Vector3(0.15f, 0.2f, 0.15f));
                Part("LowerLegPiston_" + x, section, "MSH10_MESH_Cylinder16_Y", "BlackenedRivetedIron", new Vector3(x, -1.28f, 0.03f), Quaternion.identity, new Vector3(0.105f, 0.62f, 0.105f));
                Part("IronFoot_" + x, section, "MSH10_MESH_Box_Unit", "BlackenedRivetedIron", new Vector3(x, -1.62f, -0.12f), Quaternion.identity, new Vector3(0.42f, 0.15f, 0.68f));
                AddSocket(section, "SOCKET_Hip_" + (x < 0 ? "L" : "R"), new Vector3(x, -0.22f, 0));
                AddSocket(section, "SOCKET_Knee_" + (x < 0 ? "L" : "R"), new Vector3(x, -0.95f, 0));
                AddSocket(section, "SOCKET_Ankle_" + (x < 0 ? "L" : "R"), new Vector3(x, -1.5f, 0));
            }
        }

        private static void BuildTubeHarness(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_CopperTubeHarness", Vector3.zero);
            var routes = new[]
            {
                new Tuple<Vector3, Vector3, float>(new Vector3(-0.5f, 0.25f, -0.76f), new Vector3(-0.7f, 0.95f, -0.3f), -18f),
                new Tuple<Vector3, Vector3, float>(new Vector3(0.5f, 0.25f, -0.76f), new Vector3(0.7f, 0.95f, -0.3f), 18f),
                new Tuple<Vector3, Vector3, float>(new Vector3(-0.2f, 0.1f, -0.78f), new Vector3(0.2f, 0.1f, -0.78f), 90f)
            };
            for (var i = 0; i < routes.Length; i++)
            {
                var start = routes[i].Item1;
                var end = routes[i].Item2;
                var mid = (start + end) * 0.5f;
                var length = Vector3.Distance(start, end);
                var angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg - 90f;
                Part("CopperTubeRoute_" + i, section, "MSH10_MESH_Cylinder16_Y", "BurnishedCopperTube", mid, Quaternion.Euler(0, 0, angle), new Vector3(0.038f, length, 0.038f));
                Part("TubeCollarA_" + i, section, "MSH10_MESH_Cylinder16_Y", "PolishedBrassEdgewear", start, Quaternion.Euler(0, 0, angle), new Vector3(0.065f, 0.06f, 0.065f));
                Part("TubeCollarB_" + i, section, "MSH10_MESH_Cylinder16_Y", "PolishedBrassEdgewear", end, Quaternion.Euler(0, 0, angle), new Vector3(0.065f, 0.06f, 0.065f));
            }
        }

        private static void BuildRivetBandSet(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_RivetBandSet", Vector3.zero);
            for (var row = 0; row < 4; row++)
            {
                Part("RivetBandIronStrip_" + row, section, "MSH10_MESH_Box_Unit", "BlackenedRivetedIron", new Vector3(0, row * 0.2f, 0), Quaternion.identity, new Vector3(1.7f, 0.045f, 0.04f));
                for (var i = 0; i < 14; i++)
                {
                    AddFrontBolt(section, "RivetBandBolt_" + row + "_" + i, new Vector3(Mathf.Lerp(-0.78f, 0.78f, i / 13f), row * 0.2f, -0.04f), 0.025f);
                }
            }
        }

        private static void BuildAmberGlowCore(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_AmberGlowCore", Vector3.zero);
            Part("CoreGlassTube", section, "MSH10_MESH_Cylinder32_Y", "SmokedAmberGlass", Vector3.zero, Quaternion.identity, new Vector3(0.22f, 0.82f, 0.22f));
            Part("CoreHotRod", section, "MSH10_MESH_Cylinder24_Y", "AmberFurnaceGlow", Vector3.zero, Quaternion.identity, new Vector3(0.1f, 0.88f, 0.1f));
            Part("CoreUpperCap", section, "MSH10_MESH_Cylinder24_Y", "PolishedBrassEdgewear", new Vector3(0, 0.46f, 0), Quaternion.identity, new Vector3(0.27f, 0.08f, 0.27f));
            Part("CoreLowerCap", section, "MSH10_MESH_Cylinder24_Y", "PolishedBrassEdgewear", new Vector3(0, -0.46f, 0), Quaternion.identity, new Vector3(0.27f, 0.08f, 0.27f));
        }

        private static void BuildSteamStacks(Transform parent)
        {
            var section = Section(parent, "MSH10_SECTION_SteamShoulderStacks", Vector3.zero);
            foreach (var x in new[] { -0.42f, 0.42f })
            {
                Part("ShoulderStackPipe_" + x, section, "MSH10_MESH_Cylinder24_Y", "BlackenedRivetedIron", new Vector3(x, 0.2f, 0), Quaternion.identity, new Vector3(0.12f, 0.72f, 0.12f));
                Part("ShoulderStackCap_" + x, section, "MSH10_MESH_Cylinder24_Y", "PolishedBrassEdgewear", new Vector3(x, 0.6f, 0), Quaternion.identity, new Vector3(0.17f, 0.08f, 0.17f));
                Part("SteamPuff_" + x, section, "MSH10_MESH_SteamCone", "SteamMistTranslucent", new Vector3(x, 0.88f, 0), Quaternion.identity, new Vector3(0.36f, 0.42f, 0.36f));
            }
        }

        private static void BuildMaterialPaletteBoard(GameObject root)
        {
            var section = Section(root.transform, "MSH10_SECTION_MaterialPaletteBoard", Vector3.zero);
            var names = Materials.Keys.ToArray();
            for (var i = 0; i < names.Length; i++)
            {
                var x = (i % 4 - 1.5f) * 0.52f;
                var y = (1 - i / 4) * 0.42f;
                Part("MaterialTile_" + names[i], section, "MSH10_MESH_Box_Unit", names[i], new Vector3(x, y, 0), Quaternion.identity, new Vector3(0.44f, 0.28f, 0.04f));
            }
        }

        private static void AddSocketSkeleton(GameObject root)
        {
            AddSocketSkeleton(root.transform);
        }

        private static void AddSocketSkeleton(Transform parent)
        {
            var skeleton = Section(parent, "MSH10_SOCKET_Skeleton", Vector3.zero);
            AddSocket(skeleton, "SOCKET_Root", new Vector3(0, 0, 0));
            AddSocket(skeleton, "SOCKET_SpineLower", new Vector3(0, 0.15f, 0));
            AddSocket(skeleton, "SOCKET_SpineUpper", new Vector3(0, 1.12f, 0));
            AddSocket(skeleton, "SOCKET_Head", new Vector3(0, 1.72f, -0.02f));
            AddSocket(skeleton, "SOCKET_ChestGauge", new Vector3(0, 0.72f, -0.81f));
            AddSocket(skeleton, "SOCKET_BackFlywheel", new Vector3(0, 0.82f, 0.73f));
            AddSocket(skeleton, "SOCKET_LeftShoulder", new Vector3(-0.86f, 0.72f, -0.04f));
            AddSocket(skeleton, "SOCKET_LeftElbow", new Vector3(-1.44f, 0.3f, -0.07f));
            AddSocket(skeleton, "SOCKET_LeftWristSaw", new Vector3(-1.94f, -0.18f, -0.12f));
            AddSocket(skeleton, "SOCKET_RightShoulder", new Vector3(0.86f, 0.72f, -0.04f));
            AddSocket(skeleton, "SOCKET_RightElbow", new Vector3(1.44f, 0.3f, -0.07f));
            AddSocket(skeleton, "SOCKET_RightWristClaw", new Vector3(1.74f, 0f, -0.12f));
            AddSocket(skeleton, "SOCKET_LeftHip", new Vector3(-0.32f, -0.22f, 0.02f));
            AddSocket(skeleton, "SOCKET_RightHip", new Vector3(0.32f, -0.22f, 0.02f));
            AddSocket(skeleton, "SOCKET_SteamEmitters", new Vector3(0, 2.6f, 0));
        }

        private static Transform Section(Transform parent, string name, Vector3 localPosition)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            return go.transform;
        }

        private static GameObject Part(string name, Transform parent, string meshName, string materialName, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localRotation = localRotation;
            go.transform.localScale = localScale;
            var filter = go.AddComponent<MeshFilter>();
            filter.sharedMesh = Meshes[meshName];
            var renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = Materials[materialName];
            return go;
        }

        private static void AddSocket(Transform parent, string name, Vector3 localPosition)
        {
            var socket = new GameObject(name);
            socket.transform.SetParent(parent, false);
            socket.transform.localPosition = localPosition;
        }

        private static void AddFrontBolt(Transform parent, string name, Vector3 position, float size)
        {
            Part(name, parent, "MSH10_MESH_HexBolt", "PolishedBrassEdgewear", position, Quaternion.Euler(90f, 0, 0), new Vector3(size, size * 0.65f, size));
        }

        private static void AddRadialBolt(Transform parent, string name, Vector3 position, float angle, float size)
        {
            Part(name, parent, "MSH10_MESH_HexBolt", "PolishedBrassEdgewear", position, Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0), new Vector3(size, size * 0.6f, size));
        }

        private static void SavePrefab(string fileName, string role, string notes, Action<GameObject> build)
        {
            var root = new GameObject(fileName);
            build(root);
            RemoveColliders(root);
            var assetPath = PackageAssetRoot + "/Runtime/Prefabs/" + fileName + ".prefab";
            EnsureDirectory(Path.Combine(_packageRoot, "Runtime/Prefabs"));
            AssetDatabase.DeleteAsset(assetPath);
            var prefab = PrefabUtility.SaveAsPrefabAsset(root, assetPath);
            var bounds = CalculateBounds(root);
            var rendererCount = root.GetComponentsInChildren<MeshRenderer>(true).Length;
            UnityEngine.Object.DestroyImmediate(root);
            Prefabs.Add(new PrefabRecord
            {
                id = fileName,
                role = role,
                prefabPath = "Runtime/Prefabs/" + fileName + ".prefab",
                rendererCount = rendererCount,
                bounds = bounds.size,
                notes = notes
            });
            if (prefab == null)
            {
                throw new InvalidOperationException("Failed to save prefab " + assetPath);
            }
        }

        private static void GeneratePreviewPngs()
        {
            RenderPreview("MSH10_PREVIEW_01_hero_front.png", "Runtime/Prefabs/MSH10_MechanicalSentinelHeroAssembly.prefab", new Vector3(0, 0, 0), "Full hero assembly front view: boiler torso, furnace eyes, saw arm, claw arm, piston legs.");
            RenderPreview("MSH10_PREVIEW_02_hero_three_quarter.png", "Runtime/Prefabs/MSH10_MechanicalSentinelHeroAssembly.prefab", new Vector3(0, -28f, 0), "Three-quarter hero view showing back flywheel depth and side tool arms.");
            RenderPreview("MSH10_PREVIEW_03_boiler_torso.png", "Runtime/Prefabs/MSH10_BoilerTorso_Module.prefab", Vector3.zero, "Boiler torso module for material and silhouette review.");
            RenderPreview("MSH10_PREVIEW_04_furnace_head_gauge.png", "Runtime/Prefabs/MSH10_FurnaceHead_Module.prefab", Vector3.zero, "Furnace head module with amber eyes and grille detail.");
            RenderPreview("MSH10_PREVIEW_05_saw_and_claw_arms.png", "Runtime/Prefabs/MSH10_LeftSawArm_Module.prefab", Vector3.zero, "Saw arm module preview.");
            RenderPreview("MSH10_PREVIEW_06_piston_leg_pair.png", "Runtime/Prefabs/MSH10_PistonLegPair_Module.prefab", Vector3.zero, "Piston leg pair with socket naming.");
            RenderPreview("MSH10_PREVIEW_07_back_flywheel.png", "Runtime/Prefabs/MSH10_BackFlywheel_Module.prefab", Vector3.zero, "Back flywheel module for future spin animation.");
            RenderPreview("MSH10_PREVIEW_08_material_palette.png", "Runtime/Prefabs/MSH10_MaterialPaletteBoard.prefab", Vector3.zero, "Material palette board.");
            RenderContactSheet();
        }

        private static void RenderPreview(string fileName, string prefabRelativePath, Vector3 eulerRotation, string notes)
        {
            var prefabPath = PackageAssetRoot + "/" + prefabRelativePath;
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                throw new InvalidOperationException("Missing preview prefab " + prefabPath);
            }

            var previewRoot = CreatePreviewStage();
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.transform.SetParent(previewRoot.transform, false);
            instance.transform.localRotation = Quaternion.Euler(eulerRotation);
            CenterOnGround(instance.transform);
            FitCameraToInstance(previewRoot, instance);
            var outputRuntime = Path.Combine(_packageRoot, "Runtime/Previews/" + fileName);
            var outputDocs = Path.Combine(ConceptRenderRoot, fileName);
            RenderStage(previewRoot, outputRuntime, outputDocs, 1536, 768);
            UnityEngine.Object.DestroyImmediate(previewRoot);
            Previews.Add(new PreviewRecord
            {
                fileName = fileName,
                runtimePath = "Runtime/Previews/" + fileName,
                documentationPath = "Documentation/ConceptRenders/V0_1_55_MechanicalSentinelHeroSet10/" + fileName,
                notes = notes
            });
        }

        private static void RenderContactSheet()
        {
            var previewRoot = CreatePreviewStage();
            var ids = new[]
            {
                "MSH10_MechanicalSentinelHeroAssembly",
                "MSH10_BoilerTorso_Module",
                "MSH10_FurnaceHead_Module",
                "MSH10_BackFlywheel_Module",
                "MSH10_LeftSawArm_Module",
                "MSH10_RightClawArm_Module",
                "MSH10_PistonLegPair_Module",
                "MSH10_AmberGlowCore_Module",
                "MSH10_MaterialPaletteBoard"
            };

            for (var i = 0; i < ids.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageAssetRoot + "/Runtime/Prefabs/" + ids[i] + ".prefab");
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.transform.SetParent(previewRoot.transform, false);
                instance.transform.localPosition = new Vector3((i % 3 - 1) * 2.25f, 1.55f - (i / 3) * 1.65f, 0);
                instance.transform.localRotation = Quaternion.Euler(0, -18f, 0);
                instance.transform.localScale = Vector3.one * 0.54f;
            }

            var outputRuntime = Path.Combine(_packageRoot, "Runtime/Previews/MSH10_PREVIEW_09_contact_sheet.png");
            var outputDocs = Path.Combine(ConceptRenderRoot, "MSH10_PREVIEW_09_contact_sheet.png");
            RenderStage(previewRoot, outputRuntime, outputDocs, 1536, 1024);
            UnityEngine.Object.DestroyImmediate(previewRoot);
            Previews.Add(new PreviewRecord
            {
                fileName = "MSH10_PREVIEW_09_contact_sheet.png",
                runtimePath = "Runtime/Previews/MSH10_PREVIEW_09_contact_sheet.png",
                documentationPath = "Documentation/ConceptRenders/V0_1_55_MechanicalSentinelHeroSet10/MSH10_PREVIEW_09_contact_sheet.png",
                notes = "Contact sheet for the hero assembly and separated rigging-friendly components."
            });
        }

        private static GameObject CreatePreviewStage()
        {
            var root = new GameObject("MSH10_PreviewStage");
            var floorMat = new Material(Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit"));
            floorMat.name = "MSH10_PREVIEW_FloorMat";
            SetMaterialColor(floorMat, new Color(0.025f, 0.023f, 0.021f));
            SetMaterialFloat(floorMat, "_Metallic", 0.2f);
            SetMaterialFloat(floorMat, "_Glossiness", 0.45f);
            SetMaterialFloat(floorMat, "_Smoothness", 0.45f);
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.DestroyImmediate(floor.GetComponent<Collider>());
            floor.name = "PreviewWetIronFloor";
            floor.transform.SetParent(root.transform, false);
            floor.transform.localPosition = new Vector3(0, -1.75f, 0.7f);
            floor.transform.localScale = new Vector3(7f, 0.04f, 4f);
            floor.GetComponent<MeshRenderer>().sharedMaterial = floorMat;

            var back = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.DestroyImmediate(back.GetComponent<Collider>());
            back.name = "PreviewDarkBackdrop";
            back.transform.SetParent(root.transform, false);
            back.transform.localPosition = new Vector3(0, 0.15f, 1.55f);
            back.transform.localScale = new Vector3(7f, 4.5f, 0.04f);
            back.GetComponent<MeshRenderer>().sharedMaterial = floorMat;

            var key = new GameObject("WarmKeyLight").AddComponent<Light>();
            key.transform.SetParent(root.transform, false);
            key.type = LightType.Point;
            key.color = new Color(1f, 0.68f, 0.35f);
            key.intensity = 9.2f;
            key.range = 8f;
            key.transform.localPosition = new Vector3(-2.3f, 2.2f, -2.4f);

            var rim = new GameObject("CopperRimLight").AddComponent<Light>();
            rim.transform.SetParent(root.transform, false);
            rim.type = LightType.Point;
            rim.color = new Color(0.95f, 0.44f, 0.18f);
            rim.intensity = 4.6f;
            rim.range = 6f;
            rim.transform.localPosition = new Vector3(2.5f, 1.8f, -1.5f);

            var fill = new GameObject("CoolIronFill").AddComponent<Light>();
            fill.transform.SetParent(root.transform, false);
            fill.type = LightType.Directional;
            fill.color = new Color(0.32f, 0.36f, 0.42f);
            fill.intensity = 0.45f;
            fill.transform.localRotation = Quaternion.Euler(35f, 135f, 0);

            var camera = new GameObject("PreviewCamera").AddComponent<Camera>();
            camera.transform.SetParent(root.transform, false);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.015f, 0.014f, 0.013f);
            camera.fieldOfView = 34f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 50f;
            camera.transform.localPosition = new Vector3(0, 0.45f, -7.2f);
            camera.transform.LookAt(new Vector3(0, 0.15f, 0));
            return root;
        }

        private static void FitCameraToInstance(GameObject previewRoot, GameObject instance)
        {
            var camera = previewRoot.GetComponentInChildren<Camera>();
            var bounds = CalculateBounds(instance);
            var maxExtent = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
            var distance = Mathf.Clamp(maxExtent / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * 1.65f, 3.2f, 9.5f);
            var target = bounds.center + new Vector3(0, bounds.size.y * 0.04f, 0);
            camera.transform.position = target + new Vector3(0, bounds.size.y * 0.08f, -distance);
            camera.transform.LookAt(target);
        }

        private static void CenterOnGround(Transform instance)
        {
            var bounds = CalculateBounds(instance.gameObject);
            instance.position -= new Vector3(bounds.center.x, bounds.min.y + 1.55f, bounds.center.z);
        }

        private static void RenderStage(GameObject root, string runtimeOutput, string docsOutput, int width, int height)
        {
            var camera = root.GetComponentInChildren<Camera>();
            var rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)
            {
                antiAliasing = 4
            };
            camera.targetTexture = rt;
            RenderTexture.active = rt;
            camera.Render();
            var image = new Texture2D(width, height, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            image.Apply();
            EnsureDirectory(Path.GetDirectoryName(runtimeOutput));
            EnsureDirectory(Path.GetDirectoryName(docsOutput));
            File.WriteAllBytes(runtimeOutput, image.EncodeToPNG());
            File.WriteAllBytes(docsOutput, image.EncodeToPNG());
            camera.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(image);
            UnityEngine.Object.DestroyImmediate(rt);
            AssetDatabase.ImportAsset(ToAssetPath(runtimeOutput));
        }

        private static void WriteCatalogAndReports()
        {
            var manifestPath = Path.Combine(_packageRoot, "Runtime/Metadata/MSH10_MechanicalSentinelHeroSet10_Manifest_v0.1.55-p001.json");
            var catalogPath = Path.Combine(_packageRoot, "Runtime/Metadata/MSH10_MechanicalSentinelHeroSet10_Catalog_v0.1.55-p001.json");
            EnsureDirectory(Path.Combine(_packageRoot, "Runtime/Metadata"));
            var manifestJson = BuildManifestJson();
            var catalogJson = BuildCatalogJson();
            File.WriteAllText(manifestPath, manifestJson);
            File.WriteAllText(catalogPath, catalogJson);
            AssetDatabase.ImportAsset(ToAssetPath(manifestPath));
            AssetDatabase.ImportAsset(ToAssetPath(catalogPath));

            var manifestParsed = JsonUtility.FromJson<ManifestProbe>(manifestJson) != null;
            var catalogParsed = JsonUtility.FromJson<CatalogProbe>(catalogJson) != null;
            var inventory = GetInventory();
            var validation = new ValidationSummary
            {
                manifestParsed = manifestParsed,
                catalogParsed = catalogParsed,
                prefabCount = CountFiles(_packageRoot, "*.prefab"),
                materialCount = CountFiles(_packageRoot, "*.mat"),
                textureCount = CountFiles(_packageRoot, "*.png", "Runtime/Textures"),
                meshCount = CountFiles(_packageRoot, "*.asset", "Runtime/Meshes"),
                runtimePreviewCount = CountFiles(_packageRoot, "*.png", "Runtime/Previews"),
                documentationPreviewCount = CountFiles(ConceptRenderRoot, "*.png"),
                rendererCountInHero = GetPrefabRendererCount("Runtime/Prefabs/MSH10_MechanicalSentinelHeroAssembly.prefab"),
                socketCountInHero = GetPrefabSocketCount("Runtime/Prefabs/MSH10_MechanicalSentinelHeroAssembly.prefab")
            };

            File.WriteAllText(Path.Combine(AssetProductionRoot, "MSH10_AssetInventory_0.1.55-p001.md"), BuildInventoryMarkdown(inventory));
            File.WriteAllText(Path.Combine(AssetProductionRoot, "MSH10_ProductionReport_0.1.55-p001.md"), BuildProductionReport(validation));
            File.WriteAllText(Path.Combine(AssetProductionRoot, "MSH10_GeneratorSourceLocation_0.1.55-p001.txt"), PackageAssetRoot + "/Editor/MechanicalSentinelHeroSet10Generator.cs\n");
            File.WriteAllText(Path.Combine(PlanningRoot, "MSH10_ImportReadinessNotes_0.1.55-p001.md"), BuildImportReadinessNotes());
            File.WriteAllText(Path.Combine(QaRoot, "MSH10_ValidationReport_0.1.55-p001.md"), BuildValidationReport(validation));
            File.WriteAllText(Path.Combine(QaRoot, "MSH10_FileValidationReport_0.1.55-p001.json"), BuildValidationJson(validation));
            File.WriteAllText(Path.Combine(QaRoot, "MSH10_FinalFileList.txt"), string.Join("\n", inventory));
        }

        private static string BuildManifestJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            AppendProp(sb, "schema", "brassworks.sidecar.normalized-manifest.v1", 1, true);
            AppendProp(sb, "packageName", PackageName, 1, true);
            AppendProp(sb, "displayName", DisplayName, 1, true);
            AppendProp(sb, "version", Version, 1, true);
            AppendProp(sb, "unity", "6000.4", 1, true);
            AppendProp(sb, "visualOnly", true, 1, true);
            AppendProp(sb, "generatedUtc", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture), 1, true);
            AppendProp(sb, "packageRoot", _packageRoot, 1, true);
            sb.AppendLine("  \"counts\": {");
            AppendProp(sb, "prefabs", Prefabs.Count, 2, true);
            AppendProp(sb, "materials", Materials.Count, 2, true);
            AppendProp(sb, "textures", Textures.Count, 2, true);
            AppendProp(sb, "meshes", MeshRecords.Count, 2, true);
            AppendProp(sb, "previewPngs", Previews.Count, 2, false);
            sb.AppendLine("  },");
            AppendStringArray(sb, "materialReadTargets", Materials.Keys.Select(ToSnake).ToArray(), 1, true);
            AppendRecords(sb, "textures", Textures.Select(t => new Dictionary<string, object> { { "path", t.path }, { "tag", t.tag }, { "type", t.kind } }).ToList(), 1, true);
            AppendRecords(sb, "materials", MaterialPaths.Select(kvp => new Dictionary<string, object> { { "path", "Runtime/Materials/MSH10_MAT_" + kvp.Key + ".mat" }, { "tag", ToSnake(kvp.Key) } }).ToList(), 1, true);
            AppendRecords(sb, "meshes", MeshRecords.Select(m => new Dictionary<string, object> { { "path", m.path }, { "role", m.role } }).ToList(), 1, true);
            AppendRecords(sb, "prefabs", Prefabs.Select(p => new Dictionary<string, object>
            {
                { "id", p.id },
                { "role", p.role },
                { "prefabPath", p.prefabPath },
                { "rendererCount", p.rendererCount },
                { "boundsSize", new Dictionary<string, object> { { "x", Round(p.bounds.x) }, { "y", Round(p.bounds.y) }, { "z", Round(p.bounds.z) } } },
                { "notes", p.notes }
            }).ToList(), 1, true);
            AppendRecords(sb, "previews", Previews.Select(p => new Dictionary<string, object> { { "runtimePath", p.runtimePath }, { "documentationPath", p.documentationPath }, { "notes", p.notes } }).ToList(), 1, true);
            AppendStringArray(sb, "riggingSockets", new[]
            {
                "SOCKET_Root", "SOCKET_SpineLower", "SOCKET_SpineUpper", "SOCKET_Head", "SOCKET_ChestGauge", "SOCKET_BackFlywheel",
                "SOCKET_LeftShoulder", "SOCKET_LeftElbow", "SOCKET_LeftWristSaw", "SOCKET_RightShoulder", "SOCKET_RightElbow",
                "SOCKET_RightWristClaw", "SOCKET_LeftHip", "SOCKET_RightHip", "SOCKET_SteamEmitters"
            }, 1, true);
            AppendStringArray(sb, "importConstraints", new[]
            {
                "visual-only",
                "no-colliders",
                "no-ai-authority",
                "no-damage-authority",
                "no-animation-controller",
                "no-audio-authority",
                "no-route-collision-authority",
                "no-shared-manifest-edits"
            }, 1, false);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string BuildCatalogJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            AppendProp(sb, "schema", "brassworks.sidecar.asset-catalog.v1", 1, true);
            AppendProp(sb, "packageName", PackageName, 1, true);
            AppendProp(sb, "version", Version, 1, true);
            AppendProp(sb, "northStar", "steampunk mechanical sentinel with boiler torso, furnace eyes, chest gauge, flywheel, saw arm, claw arm, piston legs, brass rivets, blackened iron, copper tubes, amber glow", 1, true);
            AppendRecords(sb, "assets", Prefabs.Select(p => new Dictionary<string, object>
            {
                { "id", p.id },
                { "role", p.role },
                { "path", p.prefabPath },
                { "futureUse", p.role == "hero_assembly" ? "enemy hero visual shell, rigging reference, boss/miniboss silhouette" : "component library for rigging, animation, encounter variants, and corridor set dressing" },
                { "limitations", "Visual-only. Needs rig, LODs, animation, hitboxes, gameplay scripts, audio, and final shader/material polish before production promotion." }
            }).ToList(), 1, false);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string BuildInventoryMarkdown(List<string> inventory)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# MSH10 Asset Inventory");
            sb.AppendLine();
            sb.AppendLine("Generated: `" + DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) + "`");
            sb.AppendLine();
            sb.AppendLine("- Package: `" + PackageName + "`");
            sb.AppendLine("- Prefabs: `" + Prefabs.Count + "`");
            sb.AppendLine("- Materials: `" + Materials.Count + "`");
            sb.AppendLine("- Textures: `" + Textures.Count + "`");
            sb.AppendLine("- Meshes: `" + MeshRecords.Count + "`");
            sb.AppendLine("- Preview PNGs: `" + Previews.Count + "`");
            sb.AppendLine();
            sb.AppendLine("## Files");
            foreach (var file in inventory)
            {
                sb.AppendLine("- `" + file + "`");
            }
            return sb.ToString();
        }

        private static string BuildProductionReport(ValidationSummary validation)
        {
            return @"# Mechanical Sentinel Hero Set 10 Production Report

Generated: `" + DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) + @"`

## Goal

Create a Unity-only, visual-only, rigging-aware steampunk mechanical monster hero package that moves the enemy look toward the north-star concept: boiler torso, furnace eyes, pressure gauge chest, flywheel/back wheel, saw arm, claw arm, piston legs, brass rivets, blackened iron frame, copper tubes, and amber glow.

## Output

- Prefabs: `" + validation.prefabCount + @"`
- Materials: `" + validation.materialCount + @"`
- Runtime textures: `" + validation.textureCount + @"`
- Mesh assets: `" + validation.meshCount + @"`
- Runtime previews: `" + validation.runtimePreviewCount + @"`
- Documentation previews: `" + validation.documentationPreviewCount + @"`
- Hero renderer count: `" + validation.rendererCountInHero + @"`
- Hero socket count: `" + validation.socketCountInHero + @"`

## Production Notes

The package uses procedural Unity mesh primitives and generated material texture maps rather than external DCC output. The hero assembly is intentionally separated into modules so later work can rig the sockets, animate the flywheel/saw/claw/pistons, create LODs, and wire gameplay hitboxes without redesigning the visual hierarchy.

## Limitations

This is not final AAA geometry. It is a high-density Unity-only sidecar candidate for visual direction and component decomposition. It still needs sculpted final meshes, authored UVs, LODs, rigging, animation, gameplay colliders, enemy AI hookup, sound, VFX, and final lighting integration before production promotion.
";
        }

        private static string BuildImportReadinessNotes()
        {
            return @"# MSH10 Import Readiness Notes

## Status

`STATIC READY WITH LIMITATIONS`

## Safe Import Rules

- Import as a local Unity package only after visual review.
- Keep all prefabs visual-only until main-lane gameplay ownership is assigned.
- Do not add colliders, AI scripts, hitboxes, animation controllers, lights, audio, damage, route collision, or objective behavior from this package during quarantine import.
- Use `MSH10_MechanicalSentinelHeroAssembly` as the first hero visual shell candidate.
- Use named `SOCKET_` transforms for future rig/animation planning.

## Recommended Next Work

- Review the contact sheet and hero previews against the north-star mechanical monster.
- If accepted visually, integrate only as a quarantined showcase object first.
- Assign later rigging/animation work to a separate lane after the visual hierarchy is accepted.
";
        }

        private static string BuildValidationReport(ValidationSummary validation)
        {
            return @"# MSH10 Validation Report

Generated: `" + DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) + @"`

## Result

`MSH10_VALIDATION_PASS_WITH_LIMITATIONS`

## Checks

- Manifest JSON parsed: `" + validation.manifestParsed + @"`
- Catalog JSON parsed: `" + validation.catalogParsed + @"`
- Prefabs found: `" + validation.prefabCount + @"`
- Materials found: `" + validation.materialCount + @"`
- Runtime texture PNGs found: `" + validation.textureCount + @"`
- Mesh assets found: `" + validation.meshCount + @"`
- Runtime preview PNGs found: `" + validation.runtimePreviewCount + @"`
- Documentation preview PNGs found: `" + validation.documentationPreviewCount + @"`
- Hero assembly renderers: `" + validation.rendererCountInHero + @"`
- Hero assembly sockets: `" + validation.socketCountInHero + @"`

## Limitations

Unity generation and preview rendering passed, but this package remains a visual-only sidecar candidate. It needs art-direction review before import, plus later final mesh/rig/animation/gameplay work before use in a production enemy.
";
        }

        private static string BuildValidationJson(ValidationSummary v)
        {
            return @"{
  ""schema"": ""brassworks.sidecar.validation.v1"",
  ""packageName"": """ + PackageName + @""",
  ""version"": """ + Version + @""",
  ""generatedUtc"": """ + DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) + @""",
  ""result"": ""MSH10_VALIDATION_PASS_WITH_LIMITATIONS"",
  ""manifestJsonParsed"": " + v.manifestParsed.ToString().ToLowerInvariant() + @",
  ""catalogJsonParsed"": " + v.catalogParsed.ToString().ToLowerInvariant() + @",
  ""prefabCount"": " + v.prefabCount + @",
  ""materialCount"": " + v.materialCount + @",
  ""textureCount"": " + v.textureCount + @",
  ""meshCount"": " + v.meshCount + @",
  ""runtimePreviewCount"": " + v.runtimePreviewCount + @",
  ""documentationPreviewCount"": " + v.documentationPreviewCount + @",
  ""heroRendererCount"": " + v.rendererCountInHero + @",
  ""heroSocketCount"": " + v.socketCountInHero + @",
  ""limitations"": [
    ""visual-only"",
    ""no-rig-yet"",
    ""no-animation-yet"",
    ""no-gameplay-authority"",
    ""requires art-direction review before import""
  ]
}
";
        }

        private static void CreateMaterial(MaterialSpec spec, string albedoPath, string normalPath, string metallicPath)
        {
            var shader = Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Unlit/Color");
            var material = new Material(shader)
            {
                name = "MSH10_MAT_" + spec.name
            };
            SetMaterialColor(material, spec.baseColor);
            SetMaterialFloat(material, "_Metallic", spec.metallic);
            SetMaterialFloat(material, "_Smoothness", spec.smoothness);
            SetMaterialFloat(material, "_Glossiness", spec.smoothness);
            material.SetTexture("_MainTex", AssetDatabase.LoadAssetAtPath<Texture2D>(albedoPath));
            material.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(albedoPath));
            material.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture2D>(normalPath));
            material.SetTexture("_MetallicGlossMap", AssetDatabase.LoadAssetAtPath<Texture2D>(metallicPath));
            material.EnableKeyword("_NORMALMAP");
            material.EnableKeyword("_METALLICGLOSSMAP");
            if (spec.emissive)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", spec.emissionColor * 2.4f);
            }

            if (spec.transparent)
            {
                material.SetFloat("_Mode", 3f);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = 3000;
            }

            EnsureDirectory(Path.Combine(_packageRoot, "Runtime/Materials"));
            var materialPath = PackageAssetRoot + "/Runtime/Materials/MSH10_MAT_" + spec.name + ".mat";
            AssetDatabase.DeleteAsset(materialPath);
            AssetDatabase.CreateAsset(material, materialPath);
            MaterialPaths[spec.name] = materialPath;
            Materials[spec.name] = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        }

        private static string GenerateTexture(MaterialSpec spec, string kind, bool normal)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
            var seed = spec.name.GetHashCode() ^ kind.GetHashCode();
            var height = new float[TextureSize, TextureSize];
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var n = FractalNoise(x * 0.035f, y * 0.035f, seed);
                    var scratches = Mathf.Sin((x + seed % 17) * 0.07f + Mathf.Sin(y * 0.11f)) * 0.04f;
                    height[x, y] = Mathf.Clamp01(n * 0.78f + scratches + 0.13f);
                }
            }

            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    if (normal)
                    {
                        var hL = height[Mathf.Max(0, x - 1), y];
                        var hR = height[Mathf.Min(TextureSize - 1, x + 1), y];
                        var hD = height[x, Mathf.Max(0, y - 1)];
                        var hU = height[x, Mathf.Min(TextureSize - 1, y + 1)];
                        var dx = (hL - hR) * 0.85f;
                        var dy = (hD - hU) * 0.85f;
                        texture.SetPixel(x, y, new Color(0.5f + dx, 0.5f + dy, 1f, 1f));
                    }
                    else
                    {
                        var wear = Mathf.Pow(height[x, y], 1.7f);
                        var edge = ((x < 12 || y < 12 || x > TextureSize - 13 || y > TextureSize - 13) ? 0.13f : 0f);
                        var c = spec.baseColor * Mathf.Lerp(0.62f, 1.22f, wear + edge);
                        c.a = spec.baseColor.a;
                        if (spec.tag.Contains("iron") || spec.tag.Contains("soot"))
                        {
                            c += new Color(0.015f, 0.012f, 0.01f) * Mathf.Sin(x * 0.12f + y * 0.05f);
                        }
                        if (spec.tag.Contains("copper") || spec.tag.Contains("brass"))
                        {
                            c += new Color(0.04f, 0.025f, 0.005f) * Mathf.Sin(x * 0.18f);
                        }
                        texture.SetPixel(x, y, ClampColor(c));
                    }
                }
            }

            texture.Apply();
            var fileName = "MSH10_TEX_" + spec.name + "_" + kind + ".png";
            EnsureDirectory(Path.Combine(_packageRoot, "Runtime/Textures"));
            var fullPath = Path.Combine(_packageRoot, "Runtime/Textures/" + fileName);
            File.WriteAllBytes(fullPath, texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);
            var assetPath = PackageAssetRoot + "/Runtime/Textures/" + fileName;
            AssetDatabase.ImportAsset(assetPath);
            ConfigureTextureImporter(assetPath, normal, false);
            Textures.Add(new TextureRecord { path = "Runtime/Textures/" + fileName, tag = spec.tag, kind = kind });
            return assetPath;
        }

        private static string GenerateMetallicSmoothnessTexture(MaterialSpec spec)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
            var seed = spec.name.GetHashCode() ^ 13579;
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var n = FractalNoise(x * 0.045f, y * 0.045f, seed);
                    var smooth = Mathf.Clamp01(spec.smoothness * Mathf.Lerp(0.72f, 1.14f, n));
                    var metal = Mathf.Clamp01(spec.metallic * Mathf.Lerp(0.85f, 1.05f, n));
                    texture.SetPixel(x, y, new Color(metal, 0.5f, 0.5f, smooth));
                }
            }

            texture.Apply();
            var fileName = "MSH10_TEX_" + spec.name + "_MetallicSmoothness.png";
            EnsureDirectory(Path.Combine(_packageRoot, "Runtime/Textures"));
            var fullPath = Path.Combine(_packageRoot, "Runtime/Textures/" + fileName);
            File.WriteAllBytes(fullPath, texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);
            var assetPath = PackageAssetRoot + "/Runtime/Textures/" + fileName;
            AssetDatabase.ImportAsset(assetPath);
            ConfigureTextureImporter(assetPath, false, true);
            Textures.Add(new TextureRecord { path = "Runtime/Textures/" + fileName, tag = spec.tag, kind = "MetallicSmoothness" });
            return assetPath;
        }

        private static void ConfigureTextureImporter(string assetPath, bool normal, bool linear)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            importer.textureType = normal ? TextureImporterType.NormalMap : TextureImporterType.Default;
            importer.sRGBTexture = !linear && !normal;
            importer.mipmapEnabled = true;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.SaveAndReimport();
        }

        private static void CreateMeshAsset(string name, Mesh mesh)
        {
            mesh.name = name;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
            var path = PackageAssetRoot + "/Runtime/Meshes/" + name + ".asset";
            EnsureDirectory(Path.Combine(_packageRoot, "Runtime/Meshes"));
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(mesh, path);
            Meshes[name] = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            MeshRecords.Add(new MeshRecord { path = "Runtime/Meshes/" + name + ".asset", role = name.Replace("MSH10_MESH_", string.Empty).ToLowerInvariant() });
        }

        private static Mesh CreateBoxMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,-0.5f), new Vector3(0.5f,0.5f,-0.5f), new Vector3(-0.5f,0.5f,-0.5f),
                new Vector3(-0.5f,-0.5f,0.5f), new Vector3(0.5f,-0.5f,0.5f), new Vector3(0.5f,0.5f,0.5f), new Vector3(-0.5f,0.5f,0.5f)
            };
            var triangles = new[]
            {
                0,2,1, 0,3,2, 1,2,6, 1,6,5, 5,6,7, 5,7,4,
                4,7,3, 4,3,0, 3,7,6, 3,6,2, 4,0,1, 4,1,5
            };
            return new Mesh { vertices = vertices, triangles = triangles, uv = vertices.Select(v => new Vector2(v.x + 0.5f, v.y + 0.5f)).ToArray() };
        }

        private static Mesh CreateCylinderMesh(int sides)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            vertices.Add(new Vector3(0, 0.5f, 0));
            vertices.Add(new Vector3(0, -0.5f, 0));
            for (var i = 0; i < sides; i++)
            {
                var a = i * Mathf.PI * 2f / sides;
                vertices.Add(new Vector3(Mathf.Cos(a) * 0.5f, 0.5f, Mathf.Sin(a) * 0.5f));
                vertices.Add(new Vector3(Mathf.Cos(a) * 0.5f, -0.5f, Mathf.Sin(a) * 0.5f));
            }

            for (var i = 0; i < sides; i++)
            {
                var next = (i + 1) % sides;
                var top = 2 + i * 2;
                var bottom = top + 1;
                var topNext = 2 + next * 2;
                var bottomNext = topNext + 1;
                triangles.AddRange(new[] { 0, top, topNext });
                triangles.AddRange(new[] { 1, bottomNext, bottom });
                triangles.AddRange(new[] { top, bottom, bottomNext, top, bottomNext, topNext });
            }

            return new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray(), uv = vertices.Select(v => new Vector2(v.x + 0.5f, v.z + 0.5f)).ToArray() };
        }

        private static Mesh CreateConeMesh(int sides)
        {
            var vertices = new List<Vector3> { new Vector3(0, 0.5f, 0), new Vector3(0, -0.5f, 0) };
            var triangles = new List<int>();
            for (var i = 0; i < sides; i++)
            {
                var a = i * Mathf.PI * 2f / sides;
                vertices.Add(new Vector3(Mathf.Cos(a) * 0.5f, -0.5f, Mathf.Sin(a) * 0.5f));
            }
            for (var i = 0; i < sides; i++)
            {
                var next = 2 + ((i + 1) % sides);
                var cur = 2 + i;
                triangles.AddRange(new[] { 0, cur, next });
                triangles.AddRange(new[] { 1, next, cur });
            }
            return new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray(), uv = vertices.Select(v => new Vector2(v.x + 0.5f, v.z + 0.5f)).ToArray() };
        }

        private static Mesh CreateTorusMesh(int segments, int tubeSegments, float majorRadius, float minorRadius)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();
            for (var i = 0; i < segments; i++)
            {
                var u = i * Mathf.PI * 2f / segments;
                for (var j = 0; j < tubeSegments; j++)
                {
                    var v = j * Mathf.PI * 2f / tubeSegments;
                    var r = majorRadius + minorRadius * Mathf.Cos(v);
                    vertices.Add(new Vector3(Mathf.Cos(u) * r, minorRadius * Mathf.Sin(v), Mathf.Sin(u) * r));
                    uvs.Add(new Vector2(i / (float)segments, j / (float)tubeSegments));
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
            return new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray(), uv = uvs.ToArray() };
        }

        private static Mesh CreateSawBladeMesh(int teeth)
        {
            var vertices = new List<Vector3> { Vector3.zero };
            var triangles = new List<int>();
            for (var i = 0; i < teeth * 2; i++)
            {
                var a = i * Mathf.PI * 2f / (teeth * 2);
                var r = i % 2 == 0 ? 0.5f : 0.38f;
                vertices.Add(new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, 0));
            }
            for (var i = 1; i <= teeth * 2; i++)
            {
                triangles.AddRange(new[] { 0, i, i == teeth * 2 ? 1 : i + 1 });
            }
            return new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray(), uv = vertices.Select(v => new Vector2(v.x + 0.5f, v.y + 0.5f)).ToArray() };
        }

        private static Mesh CreateClawBladeMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.45f, -0.08f, -0.035f), new Vector3(0.15f, -0.12f, -0.035f), new Vector3(0.5f, 0, -0.035f), new Vector3(0.15f, 0.12f, -0.035f),
                new Vector3(-0.45f, -0.08f, 0.035f), new Vector3(0.15f, -0.12f, 0.035f), new Vector3(0.5f, 0, 0.035f), new Vector3(0.15f, 0.12f, 0.035f)
            };
            var triangles = new[] { 0,1,2, 0,2,3, 4,6,5, 4,7,6, 0,4,5, 0,5,1, 1,5,6, 1,6,2, 2,6,7, 2,7,3, 3,7,4, 3,4,0 };
            return new Mesh { vertices = vertices, triangles = triangles, uv = vertices.Select(v => new Vector2(v.x + 0.5f, v.y + 0.5f)).ToArray() };
        }

        private static Mesh CreateGaugeNeedleMesh()
        {
            var vertices = new[] { new Vector3(-0.025f, -0.04f, 0), new Vector3(0.025f, -0.04f, 0), new Vector3(0.0f, 0.28f, 0), new Vector3(-0.06f, -0.075f, 0), new Vector3(0.06f, -0.075f, 0) };
            var triangles = new[] { 0, 1, 2, 3, 0, 4, 0, 1, 4 };
            return new Mesh { vertices = vertices, triangles = triangles, uv = vertices.Select(v => new Vector2(v.x + 0.5f, v.y + 0.5f)).ToArray() };
        }

        private static Mesh CreateGussetMesh()
        {
            var vertices = new[]
            {
                new Vector3(-0.5f,-0.5f,-0.05f), new Vector3(0.5f,-0.5f,-0.05f), new Vector3(-0.5f,0.5f,-0.05f),
                new Vector3(-0.5f,-0.5f,0.05f), new Vector3(0.5f,-0.5f,0.05f), new Vector3(-0.5f,0.5f,0.05f)
            };
            var triangles = new[] { 0,1,2, 3,5,4, 0,3,4, 0,4,1, 1,4,5, 1,5,2, 2,5,3, 2,3,0 };
            return new Mesh { vertices = vertices, triangles = triangles, uv = vertices.Select(v => new Vector2(v.x + 0.5f, v.y + 0.5f)).ToArray() };
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                return new Bounds(Vector3.zero, Vector3.one);
            }
            var bounds = renderers[0].bounds;
            foreach (var renderer in renderers.Skip(1))
            {
                bounds.Encapsulate(renderer.bounds);
            }
            return bounds;
        }

        private static void RemoveColliders(GameObject root)
        {
            foreach (var collider in root.GetComponentsInChildren<Collider>(true))
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }

        private static List<string> GetInventory()
        {
            var roots = new[] { _packageRoot, AssetProductionRoot, ConceptRenderRoot, PlanningRoot, QaRoot };
            return roots
                .Where(Directory.Exists)
                .SelectMany(root => Directory.GetFiles(root, "*", SearchOption.AllDirectories))
                .Select(NormalizePath)
                .Where(path => !path.Contains("/Library/"))
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static int GetPrefabRendererCount(string prefabRelativePath)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageAssetRoot + "/" + prefabRelativePath);
            return prefab == null ? 0 : prefab.GetComponentsInChildren<MeshRenderer>(true).Length;
        }

        private static int GetPrefabSocketCount(string prefabRelativePath)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageAssetRoot + "/" + prefabRelativePath);
            if (prefab == null)
            {
                return 0;
            }
            return prefab.GetComponentsInChildren<Transform>(true).Count(t => t.name.StartsWith("SOCKET_", StringComparison.Ordinal));
        }

        private static int CountFiles(string root, string pattern, string relativeSubRoot = "")
        {
            var path = string.IsNullOrWhiteSpace(relativeSubRoot) ? root : Path.Combine(root, relativeSubRoot);
            return Directory.Exists(path) ? Directory.GetFiles(path, pattern, SearchOption.AllDirectories).Length : 0;
        }

        private static void AppendProp(StringBuilder sb, string name, string value, int indent, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(name).Append("\": \"").Append(EscapeJson(value)).Append('"').AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendProp(StringBuilder sb, string name, int value, int indent, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(name).Append("\": ").Append(value).AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendProp(StringBuilder sb, string name, bool value, int indent, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(name).Append("\": ").Append(value ? "true" : "false").AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendStringArray(StringBuilder sb, string name, IReadOnlyList<string> values, int indent, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(name).Append("\": [");
            for (var i = 0; i < values.Count; i++)
            {
                sb.Append('"').Append(EscapeJson(values[i])).Append('"');
                if (i < values.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(']').AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendRecords(StringBuilder sb, string name, IReadOnlyList<Dictionary<string, object>> records, int indent, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(name).AppendLine("\": [");
            for (var i = 0; i < records.Count; i++)
            {
                sb.Append(' ', (indent + 1) * 2).Append("{ ");
                var pairs = records[i].ToArray();
                for (var j = 0; j < pairs.Length; j++)
                {
                    sb.Append('"').Append(pairs[j].Key).Append("\": ");
                    AppendJsonValue(sb, pairs[j].Value);
                    if (j < pairs.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(" }").AppendLine(i < records.Count - 1 ? "," : string.Empty);
            }
            sb.Append(' ', indent * 2).Append(']').AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendJsonValue(StringBuilder sb, object value)
        {
            if (value is string stringValue)
            {
                sb.Append('"').Append(EscapeJson(stringValue)).Append('"');
            }
            else if (value is int intValue)
            {
                sb.Append(intValue);
            }
            else if (value is float floatValue)
            {
                sb.Append(Round(floatValue).ToString(CultureInfo.InvariantCulture));
            }
            else if (value is double doubleValue)
            {
                sb.Append(doubleValue.ToString("0.###", CultureInfo.InvariantCulture));
            }
            else if (value is Dictionary<string, object> dictionary)
            {
                sb.Append("{ ");
                var pairs = dictionary.ToArray();
                for (var i = 0; i < pairs.Length; i++)
                {
                    sb.Append('"').Append(pairs[i].Key).Append("\": ");
                    AppendJsonValue(sb, pairs[i].Value);
                    if (i < pairs.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(" }");
            }
            else
            {
                sb.Append('"').Append(EscapeJson(Convert.ToString(value, CultureInfo.InvariantCulture))).Append('"');
            }
        }

        private static float Round(float value)
        {
            return Mathf.Round(value * 1000f) / 1000f;
        }

        private static string EscapeJson(string value)
        {
            return NormalizePath(value).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string ToSnake(string value)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                if (char.IsUpper(c) && i > 0)
                {
                    sb.Append('_');
                }
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }

        private static float FractalNoise(float x, float y, int seed)
        {
            var total = 0f;
            var amplitude = 0.5f;
            var frequency = 1f;
            for (var i = 0; i < 4; i++)
            {
                total += ValueNoise(x * frequency, y * frequency, seed + i * 101) * amplitude;
                frequency *= 2f;
                amplitude *= 0.5f;
            }
            return Mathf.Clamp01(total);
        }

        private static float ValueNoise(float x, float y, int seed)
        {
            var xi = Mathf.FloorToInt(x);
            var yi = Mathf.FloorToInt(y);
            var xf = x - xi;
            var yf = y - yi;
            var a = Hash01(xi, yi, seed);
            var b = Hash01(xi + 1, yi, seed);
            var c = Hash01(xi, yi + 1, seed);
            var d = Hash01(xi + 1, yi + 1, seed);
            var u = xf * xf * (3f - 2f * xf);
            var v = yf * yf * (3f - 2f * yf);
            return Mathf.Lerp(Mathf.Lerp(a, b, u), Mathf.Lerp(c, d, u), v);
        }

        private static float Hash01(int x, int y, int seed)
        {
            unchecked
            {
                var h = x * 374761393 + y * 668265263 + seed * 1442695041;
                h = (h ^ (h >> 13)) * 1274126177;
                return ((h ^ (h >> 16)) & 0x7fffffff) / (float)int.MaxValue;
            }
        }

        private static Color ClampColor(Color color)
        {
            return new Color(Mathf.Clamp01(color.r), Mathf.Clamp01(color.g), Mathf.Clamp01(color.b), Mathf.Clamp01(color.a));
        }

        private static void SetMaterialColor(Material material, Color color)
        {
            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }
        }

        private static void SetMaterialFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property))
            {
                material.SetFloat(property, value);
            }
        }

        private static string ToAssetPath(string absolutePath)
        {
            var normalized = NormalizePath(absolutePath);
            var normalizedRoot = NormalizePath(_packageRoot);
            if (normalized.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
            {
                return PackageAssetRoot + normalized.Substring(normalizedRoot.Length);
            }
            return normalized;
        }

        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }

        private static void EnsureDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        private static void DeleteDirectoryIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            var metaPath = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + ".meta";
            if (File.Exists(metaPath))
            {
                File.Delete(metaPath);
            }
        }

        private static void DeleteFilesByPattern(string root, string pattern)
        {
            if (!Directory.Exists(root))
            {
                return;
            }
            foreach (var file in Directory.GetFiles(root, pattern, SearchOption.TopDirectoryOnly))
            {
                File.Delete(file);
            }
        }

        [Serializable]
        private sealed class ManifestProbe
        {
            public string schema;
            public string packageName;
            public string version;
        }

        [Serializable]
        private sealed class CatalogProbe
        {
            public string schema;
            public string packageName;
            public string version;
        }

        private sealed class MaterialSpec
        {
            public readonly string name;
            public readonly string tag;
            public readonly Color baseColor;
            public readonly float metallic;
            public readonly float smoothness;
            public readonly Color emissionColor;
            public readonly bool transparent;
            public readonly bool emissive;

            public MaterialSpec(string name, string tag, Color baseColor, float metallic, float smoothness, Color emissionColor, bool transparent, bool emissive)
            {
                this.name = name;
                this.tag = tag;
                this.baseColor = baseColor;
                this.metallic = metallic;
                this.smoothness = smoothness;
                this.emissionColor = emissionColor;
                this.transparent = transparent;
                this.emissive = emissive;
            }
        }

        private sealed class PrefabRecord
        {
            public string id = string.Empty;
            public string role = string.Empty;
            public string prefabPath = string.Empty;
            public int rendererCount;
            public Vector3 bounds;
            public string notes = string.Empty;
        }

        private sealed class PreviewRecord
        {
            public string fileName = string.Empty;
            public string runtimePath = string.Empty;
            public string documentationPath = string.Empty;
            public string notes = string.Empty;
        }

        private sealed class TextureRecord
        {
            public string path = string.Empty;
            public string tag = string.Empty;
            public string kind = string.Empty;
        }

        private sealed class MeshRecord
        {
            public string path = string.Empty;
            public string role = string.Empty;
        }

        private sealed class ValidationSummary
        {
            public bool manifestParsed;
            public bool catalogParsed;
            public int prefabCount;
            public int materialCount;
            public int textureCount;
            public int meshCount;
            public int runtimePreviewCount;
            public int documentationPreviewCount;
            public int rendererCountInHero;
            public int socketCountInHero;
        }
    }
}
