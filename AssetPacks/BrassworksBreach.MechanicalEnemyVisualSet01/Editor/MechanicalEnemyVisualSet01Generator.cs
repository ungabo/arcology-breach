using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace BrassworksBreach.MechanicalEnemyVisualSet01.Editor
{
    public static class MechanicalEnemyVisualSet01Generator
    {
        private const string PackageName = "com.brassworks.sidecar.mechanical-enemy-visual-set01";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string PrefabRoot = PackageRoot + "/Runtime/Prefabs";
        private const string MaterialRoot = PackageRoot + "/Runtime/Materials";
        private const string MeshRoot = PackageRoot + "/Runtime/Meshes";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_40_MechanicalEnemyVisualSet01";
        private const string ProductionDocFolder = "Documentation/AssetProduction/V0_1_40_MechanicalEnemyVisualSet01";

        private static readonly Color AgedBrass = new Color(0.70f, 0.47f, 0.22f, 1f);
        private static readonly Color BlackenedIron = new Color(0.065f, 0.062f, 0.055f, 1f);
        private static readonly Color BurnishedCopper = new Color(0.68f, 0.28f, 0.14f, 1f);
        private static readonly Color BellowsLeather = new Color(0.21f, 0.105f, 0.045f, 1f);
        private static readonly Color GrimyGlass = new Color(0.24f, 0.36f, 0.32f, 0.62f);
        private static readonly Color AmberLamp = new Color(1.0f, 0.58f, 0.16f, 1f);
        private static readonly Color CyanPilot = new Color(0.11f, 0.72f, 1.0f, 1f);
        private static readonly Color FurnaceOrange = new Color(1.0f, 0.31f, 0.075f, 1f);
        private static readonly Color SootBlack = new Color(0.023f, 0.020f, 0.017f, 1f);
        private static readonly Color HazardPaint = new Color(0.92f, 0.66f, 0.12f, 1f);
        private static readonly Color PolishedRivet = new Color(0.96f, 0.77f, 0.38f, 1f);
        private static readonly Color BoilerCeramic = new Color(0.62f, 0.54f, 0.43f, 1f);
        private static readonly Color RedHotIron = new Color(1.0f, 0.15f, 0.05f, 1f);
        private static readonly Color VerdigrisPipe = new Color(0.18f, 0.50f, 0.42f, 1f);
        private static readonly Color GhostedReadability = new Color(0.55f, 0.68f, 0.72f, 0.24f);

        private static readonly CandidateSpec[] Candidates =
        {
            new CandidateSpec("MEV01_SawScrapper_A_BoilerSaw", "Saw Scrapper", "Boiler Saw", 0, 1.55f, 1.02f, 0.92f, "low asymmetric saw threat"),
            new CandidateSpec("MEV01_SawScrapper_B_RipperCrawler", "Saw Scrapper", "Ripper Crawler", 1, 1.42f, 1.24f, 1.02f, "wide crouched dual-saw silhouette"),
            new CandidateSpec("MEV01_SawScrapper_C_Chainjaw", "Saw Scrapper", "Chainjaw", 2, 1.68f, 1.06f, 1.10f, "forward jaw saw and tall stack"),
            new CandidateSpec("MEV01_RivetLancer_A_PressurePike", "Rivet Lancer", "Pressure Pike", 0, 1.92f, 0.74f, 1.34f, "narrow pike read with cyan charge"),
            new CandidateSpec("MEV01_RivetLancer_B_RailLance", "Rivet Lancer", "Rail Lance", 1, 2.04f, 0.82f, 1.44f, "long ranged lance and backpack coils"),
            new CandidateSpec("MEV01_RivetLancer_C_TwinDrill", "Rivet Lancer", "Twin Drill", 2, 1.88f, 0.90f, 1.12f, "paired drill-lance silhouette"),
            new CandidateSpec("MEV01_BulwarkFurnace_A_ShieldBoiler", "Bulwark Furnace", "Shield Boiler", 0, 2.10f, 1.52f, 1.00f, "wide shield wall with furnace weakpoint"),
            new CandidateSpec("MEV01_BulwarkFurnace_B_IroncladGate", "Bulwark Furnace", "Ironclad Gate", 1, 2.22f, 1.72f, 1.04f, "doorlike armor slab and hammer arm"),
            new CandidateSpec("MEV01_BulwarkFurnace_C_CinderAnchor", "Bulwark Furnace", "Cinder Anchor", 2, 2.00f, 1.46f, 1.20f, "stocky furnace anchor with hot braces"),
            new CandidateSpec("MEV01_BellowsSupport_A_SootMedic", "Bellows Support", "Soot Medic", 0, 1.36f, 0.98f, 0.90f, "compact support node with visible bellows"),
            new CandidateSpec("MEV01_BellowsSupport_B_PressureNode", "Bellows Support", "Pressure Node", 1, 1.58f, 1.10f, 0.96f, "tripod pressure node and lamp array"),
            new CandidateSpec("MEV01_WardenOverseer_A_TallWarden", "Warden Overseer", "Tall Warden", 0, 2.48f, 1.28f, 1.12f, "elite tower silhouette with command halo"),
            new CandidateSpec("MEV01_WardenOverseer_B_OverseerBell", "Warden Overseer", "Overseer Bell", 1, 2.72f, 1.54f, 1.26f, "boss bell silhouette and shoulder fins")
        };

        [MenuItem("Brassworks Breach/Sidecars/Mechanical Enemy Visual Set 01/Generate Package")]
        public static void GenerateVisualSet()
        {
            EnsurePackageFolders();

            var materials = CreateMaterials();
            var meshes = CreateMeshes();

            foreach (var candidate in Candidates)
            {
                var instance = BuildCandidate(candidate, materials, meshes);
                SavePrefab(instance, PrefabRoot + "/" + candidate.Id + ".prefab");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("MEV01 v0.1.40 generated: " + Candidates.Length + " prefabs, 15 materials, 8 reusable meshes.");
        }

        [MenuItem("Brassworks Breach/Sidecars/Mechanical Enemy Visual Set 01/Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            GenerateVisualSet();

            var outputRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            Directory.CreateDirectory(outputRoot);

            RenderContactSheet(outputRoot, "MEV01_v0.1.40_all_candidates_contact_sheet.png", Candidates, 4, 2600, 1700);

            foreach (var family in Candidates.Select(candidate => candidate.Family).Distinct())
            {
                var familyCandidates = Candidates.Where(candidate => candidate.Family == family).ToArray();
                var fileName = "MEV01_v0.1.40_" + MakeSafeFileName(family) + "_contact_sheet.png";
                RenderContactSheet(outputRoot, fileName, familyCandidates, Math.Min(3, familyCandidates.Length), 1800, 1200);
            }

            foreach (var candidate in Candidates)
            {
                RenderSingleCandidate(outputRoot, candidate, 1400, 1400);
            }

            AssetDatabase.Refresh();
            Debug.Log("MEV01 v0.1.40 preview PNGs rendered to: " + outputRoot);
        }

        public static void GenerateAllAndRenderPreview()
        {
            RenderPreviewPngs();
        }

        public static void GenerateValidateAndQuit()
        {
            var errors = 0;
            try
            {
                RenderPreviewPngs();
                errors = WriteUnityValidationReport();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                errors = 1;
            }

            EditorApplication.Exit(errors == 0 ? 0 : 1);
        }

        private static Dictionary<string, Material> CreateMaterials()
        {
            return new Dictionary<string, Material>
            {
                ["Brass"] = UpsertMaterial("MEV01_MAT_AgedBrass", AgedBrass, 0.86f, 0.38f, Color.black, false),
                ["Iron"] = UpsertMaterial("MEV01_MAT_BlackenedIron", BlackenedIron, 0.92f, 0.24f, Color.black, false),
                ["Copper"] = UpsertMaterial("MEV01_MAT_BurnishedCopper", BurnishedCopper, 0.78f, 0.34f, Color.black, false),
                ["Leather"] = UpsertMaterial("MEV01_MAT_OilyBellowsLeather", BellowsLeather, 0.0f, 0.46f, Color.black, false),
                ["Glass"] = UpsertMaterial("MEV01_MAT_GrimyGlassLens", GrimyGlass, 0.0f, 0.80f, Color.black, true),
                ["Amber"] = UpsertMaterial("MEV01_MAT_AmberWarningLamp", AmberLamp, 0.0f, 0.38f, AmberLamp * 2.8f, false),
                ["Cyan"] = UpsertMaterial("MEV01_MAT_CyanPressurePilot", CyanPilot, 0.0f, 0.34f, CyanPilot * 2.4f, false),
                ["Furnace"] = UpsertMaterial("MEV01_MAT_FurnaceOrangeGlow", FurnaceOrange, 0.0f, 0.30f, FurnaceOrange * 3.0f, false),
                ["Soot"] = UpsertMaterial("MEV01_MAT_SootBlack", SootBlack, 0.30f, 0.16f, Color.black, false),
                ["Hazard"] = UpsertMaterial("MEV01_MAT_ChippedHazardPaint", HazardPaint, 0.16f, 0.30f, Color.black, false),
                ["Rivet"] = UpsertMaterial("MEV01_MAT_PolishedRivetWear", PolishedRivet, 1.0f, 0.52f, Color.black, false),
                ["Ceramic"] = UpsertMaterial("MEV01_MAT_BoilerCeramic", BoilerCeramic, 0.0f, 0.25f, Color.black, false),
                ["HotIron"] = UpsertMaterial("MEV01_MAT_RedHotIronSlit", RedHotIron, 0.35f, 0.28f, RedHotIron * 2.6f, false),
                ["Patina"] = UpsertMaterial("MEV01_MAT_VerdigrisPressurePipe", VerdigrisPipe, 0.72f, 0.38f, Color.black, false),
                ["Ghost"] = UpsertMaterial("MEV01_MAT_GhostedSilhouetteEnvelope", GhostedReadability, 0.0f, 0.18f, Color.black, true)
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            return new Dictionary<string, Mesh>
            {
                ["SawBlade"] = UpsertMesh("MEV01_MESH_28ToothSawBlade", CreateSawBladeMesh("MEV01_MESH_28ToothSawBlade", 28, 0.22f, 0.40f)),
                ["LanceTip"] = UpsertMesh("MEV01_MESH_RivetLanceTip", CreateConeMesh("MEV01_MESH_RivetLanceTip", 0.18f, 0.78f, 20)),
                ["ShieldPlate"] = UpsertMesh("MEV01_MESH_BulwarkShieldPlate", CreateShieldPlateMesh("MEV01_MESH_BulwarkShieldPlate")),
                ["BellowsRib"] = UpsertMesh("MEV01_MESH_BellowsRib", CreateBellowsRibMesh("MEV01_MESH_BellowsRib")),
                ["GearHalo"] = UpsertMesh("MEV01_MESH_CommandGearHalo", CreateGearRingMesh("MEV01_MESH_CommandGearHalo", 24, 0.30f, 0.48f)),
                ["OverseerFin"] = UpsertMesh("MEV01_MESH_OverseerShoulderFin", CreateFinMesh("MEV01_MESH_OverseerShoulderFin")),
                ["GaugeFace"] = UpsertMesh("MEV01_MESH_PressureGaugeFace", CreateGearRingMesh("MEV01_MESH_PressureGaugeFace", 18, 0.035f, 0.20f)),
                ["DrillBit"] = UpsertMesh("MEV01_MESH_TwinnedDrillBit", CreateDrillBitMesh("MEV01_MESH_TwinnedDrillBit", 0.16f, 0.62f, 10))
            };
        }

        private static GameObject BuildCandidate(CandidateSpec candidate, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var prefab = CreateBaseCandidate(candidate);
            var groups = GetPartGroups(prefab.transform);

            if (candidate.Family == "Saw Scrapper")
            {
                BuildSawScrapper(candidate, groups, mat, mesh);
            }
            else if (candidate.Family == "Rivet Lancer")
            {
                BuildRivetLancer(candidate, groups, mat, mesh);
            }
            else if (candidate.Family == "Bulwark Furnace")
            {
                BuildBulwarkFurnace(candidate, groups, mat, mesh);
            }
            else if (candidate.Family == "Bellows Support")
            {
                BuildBellowsSupport(candidate, groups, mat, mesh);
            }
            else
            {
                BuildWardenOverseer(candidate, groups, mat, mesh);
            }

            AddScaleEnvelope(candidate, groups, mat["Ghost"]);
            return prefab;
        }

        private static GameObject CreateBaseCandidate(CandidateSpec candidate)
        {
            var root = new GameObject(candidate.Id);
            CreateEmpty("visual_only_no_gameplay_authority", root.transform);
            CreateEmpty("scale_note_" + MakeSafeFileName(candidate.ScaleNote), root.transform);
            CreateEmpty("family_" + MakeSafeFileName(candidate.Family), root.transform);
            CreateEmpty("variant_" + MakeSafeFileName(candidate.Variant), root.transform);

            CreateEmpty("chassis", root.transform);
            CreateEmpty("boiler", root.transform);
            CreateEmpty("lens", root.transform);
            CreateEmpty("saw_limb", root.transform);
            CreateEmpty("pressure_lines", root.transform);
            CreateEmpty("rivets", root.transform);
            CreateEmpty("warning_lamps", root.transform);
            CreateEmpty("smoke_stacks", root.transform);
            CreateEmpty("armor_plates", root.transform);
            return root;
        }

        private static Dictionary<string, Transform> GetPartGroups(Transform root)
        {
            var groups = new Dictionary<string, Transform>(StringComparer.Ordinal);
            foreach (Transform child in root)
            {
                groups[child.name] = child;
            }

            return groups;
        }

        private static void BuildSawScrapper(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var variant = candidate.VariantIndex;
            var chassisWidth = 0.70f + variant * 0.08f;
            var boilerY = 0.70f + variant * 0.05f;

            CreatePrimitivePart("chassis_low_riveted_cradle", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 0.46f, 0.02f), new Vector3(chassisWidth, 0.22f, 0.58f), mat["Iron"]);
            CreatePrimitivePart("chassis_forward_jaw_block", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 0.62f, -0.36f), new Vector3(0.52f, 0.20f, 0.18f), mat["Soot"]);
            CreatePrimitivePart("boiler_squat_pressure_keg", PrimitiveType.Cylinder, groups["boiler"], new Vector3(0f, boilerY, 0.04f), new Vector3(0.43f, 0.38f, 0.43f), mat["Brass"]);
            CreatePrimitivePart("boiler_black_belly_band", PrimitiveType.Cylinder, groups["boiler"], new Vector3(0f, boilerY, 0.04f), new Vector3(0.45f, 0.045f, 0.45f), mat["Iron"]);
            CreatePrimitivePart("lens_low_furnace_head", PrimitiveType.Sphere, groups["lens"], new Vector3(0f, 1.08f + variant * 0.07f, -0.06f), new Vector3(0.36f, 0.25f, 0.31f), mat["Iron"]);
            CreateLens(groups["lens"], "lens_amber_left", new Vector3(-0.13f, 1.10f + variant * 0.07f, -0.30f), 0.070f, mat);
            CreateLens(groups["lens"], "lens_amber_right", new Vector3(0.13f, 1.10f + variant * 0.07f, -0.30f), 0.070f, mat);

            CreatePipe(groups["pressure_lines"], "pressure_line_left_elbow", new Vector3(-0.28f, 0.86f, 0.20f), new Vector3(-0.72f, 0.64f, -0.08f), 0.035f, mat["Patina"]);
            CreatePipe(groups["pressure_lines"], "pressure_line_right_elbow", new Vector3(0.28f, 0.86f, 0.20f), new Vector3(0.72f, 0.64f, -0.08f), 0.035f, mat["Patina"]);

            if (variant == 0)
            {
                AddPistonLimb(groups["saw_limb"], "left_saw_limb", new Vector3(-0.55f, 0.74f, -0.05f), -28f, mat);
                CreateMeshPart("saw_limb_left_28tooth_blade", mesh["SawBlade"], groups["saw_limb"], new Vector3(-0.88f, 0.58f, -0.18f), new Vector3(1.05f, 1.05f, 1.05f), Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
                CreatePrimitivePart("saw_limb_right_hook_counterweight", PrimitiveType.Cube, groups["saw_limb"], new Vector3(0.68f, 0.62f, -0.15f), new Vector3(0.20f, 0.42f, 0.18f), mat["Brass"]);
            }
            else if (variant == 1)
            {
                AddPistonLimb(groups["saw_limb"], "left_low_saw_limb", new Vector3(-0.58f, 0.60f, -0.04f), -36f, mat);
                AddPistonLimb(groups["saw_limb"], "right_low_saw_limb", new Vector3(0.58f, 0.60f, -0.04f), 36f, mat);
                CreateMeshPart("saw_limb_left_floor_blade", mesh["SawBlade"], groups["saw_limb"], new Vector3(-0.88f, 0.42f, -0.19f), new Vector3(0.95f, 0.95f, 0.95f), Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
                CreateMeshPart("saw_limb_right_floor_blade", mesh["SawBlade"], groups["saw_limb"], new Vector3(0.88f, 0.42f, -0.19f), new Vector3(0.95f, 0.95f, 0.95f), Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
            }
            else
            {
                CreateMeshPart("saw_limb_forward_chainjaw_blade", mesh["SawBlade"], groups["saw_limb"], new Vector3(0f, 0.73f, -0.62f), new Vector3(1.12f, 1.12f, 1.12f), Quaternion.identity, mat["Hazard"]);
                CreatePrimitivePart("saw_limb_upper_jaw_guard", PrimitiveType.Cube, groups["saw_limb"], new Vector3(0f, 0.90f, -0.57f), new Vector3(0.62f, 0.13f, 0.20f), mat["Iron"]);
                AddPistonLimb(groups["saw_limb"], "side_balancing_limb_left", new Vector3(-0.54f, 0.76f, -0.02f), -18f, mat);
                AddPistonLimb(groups["saw_limb"], "side_balancing_limb_right", new Vector3(0.54f, 0.76f, -0.02f), 18f, mat);
            }

            AddRivetBelt(groups["rivets"], "rivet_belt_boiler", 14, 0.33f, 0.31f, boilerY, mat["Rivet"]);
            AddWarningLamp(groups["warning_lamps"], "warning_lamp_back_left", new Vector3(-0.22f, 1.12f, 0.29f), 0.070f, mat["Amber"]);
            AddWarningLamp(groups["warning_lamps"], "warning_lamp_back_right", new Vector3(0.22f, 1.12f, 0.29f), 0.070f, mat["Amber"]);
            AddSmokeStack(groups["smoke_stacks"], "smoke_stack_chopped_pipe", new Vector3(0.0f, 1.25f + variant * 0.10f, 0.24f), 0.065f, 0.38f, mat);
            AddArmorPlate(groups["armor_plates"], "armor_plate_scrapper_brow", new Vector3(0f, 1.22f + variant * 0.07f, -0.24f), new Vector3(0.48f, 0.08f, 0.08f), mat["Iron"]);
            AddArmorPlate(groups["armor_plates"], "armor_plate_side_left", new Vector3(-0.47f, 0.78f, -0.02f), new Vector3(0.08f, 0.46f, 0.34f), mat["Iron"]);
            AddArmorPlate(groups["armor_plates"], "armor_plate_side_right", new Vector3(0.47f, 0.78f, -0.02f), new Vector3(0.08f, 0.46f, 0.34f), mat["Iron"]);
        }

        private static void BuildRivetLancer(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var variant = candidate.VariantIndex;
            var bodyHeight = 0.62f + variant * 0.06f;
            CreatePrimitivePart("chassis_tall_stilt_frame", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 0.80f, 0.02f), new Vector3(0.40f, 1.00f, 0.26f), mat["Iron"]);
            CreatePrimitivePart("boiler_vertical_rivet_tank", PrimitiveType.Cylinder, groups["boiler"], new Vector3(0f, 1.05f, 0.04f), new Vector3(0.31f, bodyHeight, 0.31f), mat["Brass"]);
            CreatePrimitivePart("boiler_cyan_charge_spine", PrimitiveType.Cylinder, groups["boiler"], new Vector3(0f, 1.18f, 0.34f), new Vector3(0.075f, 0.48f, 0.075f), mat["Cyan"]);
            CreatePrimitivePart("lens_long_visor_helmet", PrimitiveType.Sphere, groups["lens"], new Vector3(0f, 1.72f + variant * 0.06f, -0.05f), new Vector3(0.30f, 0.22f, 0.34f), mat["Iron"]);
            CreateLens(groups["lens"], "lens_single_pressure_eye", new Vector3(0f, 1.72f + variant * 0.06f, -0.33f), 0.075f, mat);

            CreatePipe(groups["pressure_lines"], "pressure_line_backpack_to_lance", new Vector3(0f, 1.34f, 0.32f), new Vector3(0f, 1.12f, -0.48f), 0.035f, mat["Patina"]);
            CreatePipe(groups["pressure_lines"], "pressure_line_left_knee", new Vector3(-0.16f, 0.72f, 0.04f), new Vector3(-0.34f, 0.25f, -0.03f), 0.030f, mat["Copper"]);
            CreatePipe(groups["pressure_lines"], "pressure_line_right_knee", new Vector3(0.16f, 0.72f, 0.04f), new Vector3(0.34f, 0.25f, -0.03f), 0.030f, mat["Copper"]);

            AddPistonLimb(groups["saw_limb"], "left_lance_support_limb", new Vector3(-0.42f, 1.06f, -0.04f), -18f, mat);
            AddPistonLimb(groups["saw_limb"], "right_lance_support_limb", new Vector3(0.42f, 1.06f, -0.04f), 18f, mat);

            if (variant == 2)
            {
                CreateMeshPart("saw_limb_left_twinned_drill", mesh["DrillBit"], groups["saw_limb"], new Vector3(-0.25f, 1.05f, -0.74f), new Vector3(1f, 1f, 1f), Quaternion.identity, mat["Rivet"]);
                CreateMeshPart("saw_limb_right_twinned_drill", mesh["DrillBit"], groups["saw_limb"], new Vector3(0.25f, 1.05f, -0.74f), new Vector3(1f, 1f, 1f), Quaternion.identity, mat["Rivet"]);
                CreatePrimitivePart("saw_limb_cyan_drill_bridge", PrimitiveType.Cube, groups["saw_limb"], new Vector3(0f, 1.05f, -0.42f), new Vector3(0.58f, 0.10f, 0.12f), mat["Cyan"]);
            }
            else
            {
                var shaftLength = variant == 0 ? 0.72f : 0.94f;
                CreatePrimitivePart("saw_limb_forward_rivet_lance_shaft", PrimitiveType.Cylinder, groups["saw_limb"], new Vector3(0f, 1.08f, -0.62f), new Vector3(0.045f, shaftLength, 0.045f), mat["Iron"], Quaternion.Euler(90f, 0f, 0f));
                CreateMeshPart("saw_limb_forward_rivet_lance_tip", mesh["LanceTip"], groups["saw_limb"], new Vector3(0f, 1.08f, -1.15f - variant * 0.18f), new Vector3(1f + variant * 0.10f, 1f + variant * 0.10f, 1f + variant * 0.10f), Quaternion.identity, mat["Rivet"]);
                CreatePrimitivePart("saw_limb_cyan_charge_band", PrimitiveType.Cylinder, groups["saw_limb"], new Vector3(0f, 1.08f, -0.72f), new Vector3(0.075f, 0.07f, 0.075f), mat["Cyan"], Quaternion.Euler(90f, 0f, 0f));
            }

            AddRivetBelt(groups["rivets"], "rivet_belt_lancer_chest", 12, 0.25f, 0.25f, 1.08f, mat["Rivet"]);
            AddWarningLamp(groups["warning_lamps"], "warning_lamp_lance_ready", new Vector3(0f, 1.46f, -0.28f), 0.060f, mat["Amber"]);
            AddSmokeStack(groups["smoke_stacks"], "smoke_stack_backpack_left", new Vector3(-0.17f, 1.58f, 0.30f), 0.050f, 0.30f, mat);
            AddSmokeStack(groups["smoke_stacks"], "smoke_stack_backpack_right", new Vector3(0.17f, 1.58f, 0.30f), 0.050f, 0.30f, mat);
            AddArmorPlate(groups["armor_plates"], "armor_plate_lancer_visor_brow", new Vector3(0f, 1.82f + variant * 0.06f, -0.25f), new Vector3(0.40f, 0.065f, 0.08f), mat["Iron"]);
            AddArmorPlate(groups["armor_plates"], "armor_plate_lancer_shin_left", new Vector3(-0.20f, 0.42f, -0.05f), new Vector3(0.13f, 0.48f, 0.11f), mat["Iron"]);
            AddArmorPlate(groups["armor_plates"], "armor_plate_lancer_shin_right", new Vector3(0.20f, 0.42f, -0.05f), new Vector3(0.13f, 0.48f, 0.11f), mat["Iron"]);
        }

        private static void BuildBulwarkFurnace(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var variant = candidate.VariantIndex;
            var width = 0.90f + variant * 0.12f;
            CreatePrimitivePart("chassis_heavy_iron_sled", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 0.56f, 0.03f), new Vector3(width, 0.34f, 0.70f), mat["Iron"]);
            CreatePrimitivePart("boiler_wide_furnace_body", PrimitiveType.Cylinder, groups["boiler"], new Vector3(0f, 1.08f, 0.05f), new Vector3(0.54f + variant * 0.04f, 0.64f, 0.54f + variant * 0.04f), mat["Brass"]);
            CreatePrimitivePart("boiler_ceramic_furnace_door", PrimitiveType.Cube, groups["boiler"], new Vector3(0f, 1.02f, -0.43f), new Vector3(0.52f, 0.44f, 0.06f), mat["Ceramic"]);
            CreatePrimitivePart("boiler_red_hot_furnace_slit", PrimitiveType.Cube, groups["boiler"], new Vector3(0f, 1.03f, -0.48f), new Vector3(0.38f, 0.08f, 0.035f), mat["HotIron"]);
            CreatePrimitivePart("lens_domed_guard_head", PrimitiveType.Sphere, groups["lens"], new Vector3(0f, 1.82f + variant * 0.04f, -0.04f), new Vector3(0.40f, 0.28f, 0.36f), mat["Iron"]);
            CreateLens(groups["lens"], "lens_furnace_left", new Vector3(-0.14f, 1.83f + variant * 0.04f, -0.32f), 0.070f, mat);
            CreateLens(groups["lens"], "lens_furnace_right", new Vector3(0.14f, 1.83f + variant * 0.04f, -0.32f), 0.070f, mat);

            AddPistonLimb(groups["saw_limb"], "left_heavy_brace_limb", new Vector3(-0.66f - variant * 0.08f, 1.02f, -0.04f), -10f, mat);
            AddPistonLimb(groups["saw_limb"], "right_heavy_hammer_limb", new Vector3(0.66f + variant * 0.08f, 1.02f, -0.04f), 10f, mat);
            CreateMeshPart("armor_plates_left_bulwark_shield_mesh", mesh["ShieldPlate"], groups["armor_plates"], new Vector3(-0.82f - variant * 0.10f, 1.05f, -0.36f), new Vector3(1.10f + variant * 0.10f, 1.12f, 1.10f), Quaternion.Euler(0f, 8f, 0f), mat["Iron"]);
            CreatePrimitivePart("armor_plates_shield_hazard_bar", PrimitiveType.Cube, groups["armor_plates"], new Vector3(-0.82f - variant * 0.10f, 1.05f, -0.60f), new Vector3(0.68f, 1.08f, 0.030f), mat["Hazard"]);

            if (variant == 1)
            {
                CreateMeshPart("armor_plates_right_gate_shield_mesh", mesh["ShieldPlate"], groups["armor_plates"], new Vector3(0.82f, 1.05f, -0.36f), new Vector3(0.96f, 1.05f, 0.96f), Quaternion.Euler(0f, -8f, 0f), mat["Iron"]);
                CreatePrimitivePart("saw_limb_hidden_hammer_core", PrimitiveType.Cube, groups["saw_limb"], new Vector3(0.72f, 0.72f, -0.20f), new Vector3(0.32f, 0.30f, 0.38f), mat["Brass"]);
            }
            else
            {
                CreatePrimitivePart("saw_limb_right_steamhammer_handle", PrimitiveType.Cylinder, groups["saw_limb"], new Vector3(0.74f + variant * 0.08f, 0.92f, -0.12f), new Vector3(0.055f, 0.42f, 0.055f), mat["Iron"], Quaternion.Euler(20f, 0f, 14f));
                CreatePrimitivePart("saw_limb_right_steamhammer_head", PrimitiveType.Cube, groups["saw_limb"], new Vector3(0.90f + variant * 0.08f, 0.68f, -0.25f), new Vector3(0.34f, 0.26f, 0.44f), mat["Brass"]);
            }

            CreatePipe(groups["pressure_lines"], "pressure_line_furnace_left", new Vector3(-0.42f, 1.36f, 0.24f), new Vector3(-0.72f, 1.02f, -0.20f), 0.040f, mat["Patina"]);
            CreatePipe(groups["pressure_lines"], "pressure_line_furnace_right", new Vector3(0.42f, 1.36f, 0.24f), new Vector3(0.72f, 1.02f, -0.20f), 0.040f, mat["Patina"]);
            AddRivetBelt(groups["rivets"], "rivet_belt_furnace_core", 16, 0.43f + variant * 0.03f, 0.43f + variant * 0.03f, 1.09f, mat["Rivet"]);
            AddWarningLamp(groups["warning_lamps"], "warning_lamp_guard_break_center", new Vector3(0f, 1.25f, -0.52f), 0.085f, mat["Amber"]);
            AddSmokeStack(groups["smoke_stacks"], "smoke_stack_furnace_left", new Vector3(-0.30f, 1.62f, 0.32f), 0.065f, 0.42f, mat);
            AddSmokeStack(groups["smoke_stacks"], "smoke_stack_furnace_right", new Vector3(0.30f, 1.62f, 0.32f), 0.065f, 0.42f, mat);
        }

        private static void BuildBellowsSupport(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var variant = candidate.VariantIndex;
            CreatePrimitivePart("chassis_tripod_pressure_foot_left", PrimitiveType.Cube, groups["chassis"], new Vector3(-0.36f, 0.22f, -0.10f), new Vector3(0.38f, 0.12f, 0.20f), mat["Iron"]);
            CreatePrimitivePart("chassis_tripod_pressure_foot_right", PrimitiveType.Cube, groups["chassis"], new Vector3(0.36f, 0.22f, -0.10f), new Vector3(0.38f, 0.12f, 0.20f), mat["Iron"]);
            CreatePrimitivePart("chassis_rear_pressure_foot", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 0.22f, 0.36f), new Vector3(0.44f, 0.12f, 0.20f), mat["Iron"]);
            CreatePrimitivePart("boiler_round_support_tank", PrimitiveType.Sphere, groups["boiler"], new Vector3(0f, 0.84f + variant * 0.08f, 0.03f), new Vector3(0.46f, 0.42f, 0.46f), mat["Brass"]);
            CreatePrimitivePart("boiler_leather_bellows_center", PrimitiveType.Cube, groups["boiler"], new Vector3(0f, 0.72f + variant * 0.06f, -0.25f), new Vector3(0.50f, 0.34f, 0.24f), mat["Leather"]);

            for (var i = 0; i < 5 + variant; i++)
            {
                var z = -0.36f - i * 0.055f;
                CreateMeshPart("boiler_bellows_rib_" + i.ToString("00"), mesh["BellowsRib"], groups["boiler"], new Vector3(0f, 0.72f + variant * 0.06f, z), new Vector3(0.86f, 0.72f, 0.86f), Quaternion.identity, mat["Rivet"]);
            }

            CreatePrimitivePart("lens_support_node_head", PrimitiveType.Sphere, groups["lens"], new Vector3(0f, 1.16f + variant * 0.10f, -0.10f), new Vector3(0.30f, 0.24f, 0.30f), mat["Iron"]);
            CreateLens(groups["lens"], "lens_support_primary", new Vector3(0f, 1.17f + variant * 0.10f, -0.34f), 0.075f, mat);

            AddPistonLimb(groups["saw_limb"], "left_small_service_limb", new Vector3(-0.46f, 0.80f, -0.02f), -24f, mat);
            AddPistonLimb(groups["saw_limb"], "right_small_service_limb", new Vector3(0.46f, 0.80f, -0.02f), 24f, mat);
            if (variant == 0)
            {
                CreatePrimitivePart("saw_limb_repair_nozzle", PrimitiveType.Cylinder, groups["saw_limb"], new Vector3(0.0f, 0.85f, -0.74f), new Vector3(0.050f, 0.42f, 0.050f), mat["Copper"], Quaternion.Euler(90f, 0f, 0f));
                AddWarningLamp(groups["warning_lamps"], "warning_lamp_soot_medic_left", new Vector3(-0.20f, 1.32f, -0.18f), 0.060f, mat["Amber"]);
                AddWarningLamp(groups["warning_lamps"], "warning_lamp_soot_medic_right", new Vector3(0.20f, 1.32f, -0.18f), 0.060f, mat["Amber"]);
            }
            else
            {
                CreatePrimitivePart("saw_limb_pressure_tuning_fork_left", PrimitiveType.Cylinder, groups["saw_limb"], new Vector3(-0.18f, 0.96f, -0.70f), new Vector3(0.045f, 0.42f, 0.045f), mat["Cyan"], Quaternion.Euler(90f, 0f, 0f));
                CreatePrimitivePart("saw_limb_pressure_tuning_fork_right", PrimitiveType.Cylinder, groups["saw_limb"], new Vector3(0.18f, 0.96f, -0.70f), new Vector3(0.045f, 0.42f, 0.045f), mat["Cyan"], Quaternion.Euler(90f, 0f, 0f));
                AddWarningLamp(groups["warning_lamps"], "warning_lamp_pressure_node_top", new Vector3(0f, 1.48f, -0.04f), 0.070f, mat["Amber"]);
            }

            CreatePipe(groups["pressure_lines"], "pressure_line_bellows_to_tank_left", new Vector3(-0.24f, 0.74f, -0.34f), new Vector3(-0.30f, 0.98f, 0.18f), 0.030f, mat["Patina"]);
            CreatePipe(groups["pressure_lines"], "pressure_line_bellows_to_tank_right", new Vector3(0.24f, 0.74f, -0.34f), new Vector3(0.30f, 0.98f, 0.18f), 0.030f, mat["Patina"]);
            AddRivetBelt(groups["rivets"], "rivet_belt_support_tank", 12, 0.32f, 0.32f, 0.86f + variant * 0.08f, mat["Rivet"]);
            AddSmokeStack(groups["smoke_stacks"], "smoke_stack_bellows_filter", new Vector3(0f, 1.30f + variant * 0.12f, 0.25f), 0.055f, 0.36f, mat);
            AddArmorPlate(groups["armor_plates"], "armor_plate_support_forehead", new Vector3(0f, 1.28f + variant * 0.10f, -0.24f), new Vector3(0.38f, 0.06f, 0.08f), mat["Iron"]);
            AddArmorPlate(groups["armor_plates"], "armor_plate_bellows_guard", new Vector3(0f, 0.88f + variant * 0.05f, -0.50f), new Vector3(0.62f, 0.10f, 0.055f), mat["Hazard"]);
        }

        private static void BuildWardenOverseer(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var variant = candidate.VariantIndex;
            CreatePrimitivePart("chassis_command_tower_frame", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 0.92f, 0.03f), new Vector3(0.58f + variant * 0.12f, 1.26f, 0.34f), mat["Iron"]);
            CreatePrimitivePart("boiler_command_spine", PrimitiveType.Cylinder, groups["boiler"], new Vector3(0f, 1.25f, 0.05f), new Vector3(0.40f + variant * 0.06f, 0.84f + variant * 0.08f, 0.40f + variant * 0.06f), mat["Brass"]);
            CreateMeshPart("boiler_command_gear_halo", mesh["GearHalo"], groups["boiler"], new Vector3(0f, 1.42f + variant * 0.12f, -0.43f), new Vector3(1.05f + variant * 0.18f, 1.05f + variant * 0.18f, 1.05f), Quaternion.Euler(0f, 180f, 0f), mat["Rivet"]);
            CreatePrimitivePart("boiler_command_furnace_core", PrimitiveType.Sphere, groups["boiler"], new Vector3(0f, 1.42f + variant * 0.12f, -0.49f), new Vector3(0.13f + variant * 0.03f, 0.13f + variant * 0.03f, 0.13f + variant * 0.03f), mat["Furnace"]);
            CreatePrimitivePart("lens_overseer_cowl", PrimitiveType.Sphere, groups["lens"], new Vector3(0f, 2.14f + variant * 0.16f, -0.05f), new Vector3(0.42f + variant * 0.08f, 0.30f, 0.40f), mat["Iron"]);
            CreateLens(groups["lens"], "lens_overseer_left", new Vector3(-0.15f, 2.15f + variant * 0.16f, -0.35f), 0.075f, mat);
            CreateLens(groups["lens"], "lens_overseer_right", new Vector3(0.15f, 2.15f + variant * 0.16f, -0.35f), 0.075f, mat);
            CreatePrimitivePart("lens_crown_gauge_glass", PrimitiveType.Cylinder, groups["lens"], new Vector3(0f, 2.38f + variant * 0.20f, -0.04f), new Vector3(0.17f + variant * 0.04f, 0.055f, 0.17f + variant * 0.04f), mat["Glass"], Quaternion.Euler(90f, 0f, 0f));

            AddPistonLimb(groups["saw_limb"], "left_command_limb", new Vector3(-0.58f - variant * 0.10f, 1.30f, -0.02f), -16f, mat);
            AddPistonLimb(groups["saw_limb"], "right_command_limb", new Vector3(0.58f + variant * 0.10f, 1.30f, -0.02f), 16f, mat);
            CreateMeshPart("saw_limb_left_command_pincer", mesh["LanceTip"], groups["saw_limb"], new Vector3(-0.82f - variant * 0.10f, 1.02f, -0.36f), new Vector3(0.82f, 0.82f, 0.82f), Quaternion.Euler(0f, -35f, 0f), mat["Rivet"]);
            CreateMeshPart("saw_limb_right_command_pincer", mesh["LanceTip"], groups["saw_limb"], new Vector3(0.82f + variant * 0.10f, 1.02f, -0.36f), new Vector3(0.82f, 0.82f, 0.82f), Quaternion.Euler(0f, 35f, 0f), mat["Rivet"]);

            if (variant == 1)
            {
                CreatePrimitivePart("boiler_overseer_bell_skirt", PrimitiveType.Cylinder, groups["boiler"], new Vector3(0f, 1.05f, 0.01f), new Vector3(0.62f, 0.34f, 0.62f), mat["Copper"]);
                CreateMeshPart("armor_plates_left_overseer_fin", mesh["OverseerFin"], groups["armor_plates"], new Vector3(-0.70f, 1.82f, 0.02f), new Vector3(1.08f, 1.08f, 1.08f), Quaternion.Euler(0f, 180f, 18f), mat["Iron"]);
                CreateMeshPart("armor_plates_right_overseer_fin", mesh["OverseerFin"], groups["armor_plates"], new Vector3(0.70f, 1.82f, 0.02f), new Vector3(1.08f, 1.08f, 1.08f), Quaternion.Euler(0f, 0f, -18f), mat["Iron"]);
            }
            else
            {
                AddArmorPlate(groups["armor_plates"], "armor_plate_left_warden_pauldron", new Vector3(-0.58f, 1.72f, -0.08f), new Vector3(0.32f, 0.18f, 0.34f), mat["Iron"]);
                AddArmorPlate(groups["armor_plates"], "armor_plate_right_warden_pauldron", new Vector3(0.58f, 1.72f, -0.08f), new Vector3(0.32f, 0.18f, 0.34f), mat["Iron"]);
            }

            CreatePipe(groups["pressure_lines"], "pressure_line_command_left", new Vector3(-0.32f, 1.62f, 0.28f), new Vector3(-0.62f - variant * 0.10f, 1.22f, -0.16f), 0.040f, mat["Patina"]);
            CreatePipe(groups["pressure_lines"], "pressure_line_command_right", new Vector3(0.32f, 1.62f, 0.28f), new Vector3(0.62f + variant * 0.10f, 1.22f, -0.16f), 0.040f, mat["Patina"]);
            AddRivetBelt(groups["rivets"], "rivet_belt_command_spine", 18, 0.33f + variant * 0.05f, 0.33f + variant * 0.05f, 1.28f, mat["Rivet"]);
            AddWarningLamp(groups["warning_lamps"], "warning_lamp_command_center", new Vector3(0f, 1.74f + variant * 0.12f, -0.38f), 0.080f, mat["Amber"]);
            AddWarningLamp(groups["warning_lamps"], "warning_lamp_command_left", new Vector3(-0.32f, 1.58f, -0.32f), 0.055f, mat["Amber"]);
            AddWarningLamp(groups["warning_lamps"], "warning_lamp_command_right", new Vector3(0.32f, 1.58f, -0.32f), 0.055f, mat["Amber"]);
            AddSmokeStack(groups["smoke_stacks"], "smoke_stack_command_left", new Vector3(-0.34f, 2.04f + variant * 0.14f, 0.28f), 0.060f, 0.44f, mat);
            AddSmokeStack(groups["smoke_stacks"], "smoke_stack_command_right", new Vector3(0.34f, 2.04f + variant * 0.14f, 0.28f), 0.060f, 0.44f, mat);
        }

        private static void AddScaleEnvelope(CandidateSpec candidate, Dictionary<string, Transform> groups, Material material)
        {
            var envelope = CreatePrimitivePart("chassis_readability_envelope_" + candidate.Height.ToString("0.00m").Replace(".", "p"), PrimitiveType.Cube, groups["chassis"], new Vector3(0f, candidate.Height * 0.5f, 0.16f), new Vector3(candidate.Width, candidate.Height, 0.025f), material);
            envelope.hideFlags = HideFlags.NotEditable;
        }

        private static void AddPistonLimb(Transform parent, string prefix, Vector3 center, float angleZ, Dictionary<string, Material> mat)
        {
            CreatePrimitivePart(prefix + "_upper_iron_piston", PrimitiveType.Cylinder, parent, center + new Vector3(0f, 0.10f, 0f), new Vector3(0.045f, 0.30f, 0.045f), mat["Iron"], Quaternion.Euler(0f, 0f, angleZ));
            CreatePrimitivePart(prefix + "_lower_brass_sleeve", PrimitiveType.Cylinder, parent, center + new Vector3(0f, -0.18f, -0.08f), new Vector3(0.060f, 0.24f, 0.060f), mat["Brass"], Quaternion.Euler(18f, 0f, angleZ));
            CreatePrimitivePart(prefix + "_round_rivet_joint", PrimitiveType.Sphere, parent, center + new Vector3(0f, -0.38f, -0.10f), new Vector3(0.105f, 0.105f, 0.105f), mat["Rivet"]);
        }

        private static void AddRivetBelt(Transform parent, string name, int count, float radiusX, float radiusZ, float y, Material material)
        {
            var belt = CreateEmpty(name, parent);
            for (var i = 0; i < count; i++)
            {
                var angle = i * Mathf.PI * 2f / count;
                var position = new Vector3(Mathf.Cos(angle) * radiusX, y, Mathf.Sin(angle) * radiusZ + 0.04f);
                CreatePrimitivePart("rivet_" + i.ToString("00"), PrimitiveType.Sphere, belt.transform, position, new Vector3(0.040f, 0.040f, 0.040f), material);
            }
        }

        private static void AddWarningLamp(Transform parent, string name, Vector3 position, float radius, Material material)
        {
            var lamp = CreatePrimitivePart(name, PrimitiveType.Sphere, parent, position, new Vector3(radius, radius, radius), material);
            var light = lamp.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = AmberLamp;
            light.intensity = 2.3f;
            light.range = 1.2f;
        }

        private static void CreateLens(Transform parent, string name, Vector3 position, float radius, Dictionary<string, Material> mat)
        {
            CreatePrimitivePart(name + "_glass", PrimitiveType.Sphere, parent, position, new Vector3(radius, radius, radius * 0.48f), mat["Glass"]);
            CreatePrimitivePart(name + "_inner_lamp", PrimitiveType.Sphere, parent, position + new Vector3(0f, 0f, -0.015f), new Vector3(radius * 0.62f, radius * 0.62f, radius * 0.32f), mat["Amber"]);
        }

        private static void AddSmokeStack(Transform parent, string name, Vector3 basePosition, float radius, float height, Dictionary<string, Material> mat)
        {
            CreatePrimitivePart(name + "_blackened_pipe", PrimitiveType.Cylinder, parent, basePosition + new Vector3(0f, height * 0.5f, 0f), new Vector3(radius, height * 0.5f, radius), mat["Iron"]);
            CreatePrimitivePart(name + "_soot_cap", PrimitiveType.Cylinder, parent, basePosition + new Vector3(0f, height, 0f), new Vector3(radius * 1.22f, 0.035f, radius * 1.22f), mat["Soot"]);
            CreatePrimitivePart(name + "_amber_heat_mark", PrimitiveType.Cylinder, parent, basePosition + new Vector3(0f, height * 0.23f, 0f), new Vector3(radius * 1.06f, 0.025f, radius * 1.06f), mat["HotIron"]);
        }

        private static void AddArmorPlate(Transform parent, string name, Vector3 position, Vector3 scale, Material material)
        {
            CreatePrimitivePart(name, PrimitiveType.Cube, parent, position, scale, material);
        }

        private static void CreatePipe(Transform parent, string name, Vector3 start, Vector3 end, float radius, Material material)
        {
            var direction = end - start;
            var length = direction.magnitude;
            if (length < 0.0001f)
            {
                return;
            }

            var part = CreatePrimitivePart(name, PrimitiveType.Cylinder, parent, start + direction * 0.5f, new Vector3(radius, length * 0.5f, radius), material);
            part.transform.localRotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
        }

        private static GameObject CreatePrimitivePart(string name, PrimitiveType primitive, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            return CreatePrimitivePart(name, primitive, parent, localPosition, localScale, material, Quaternion.identity);
        }

        private static GameObject CreatePrimitivePart(string name, PrimitiveType primitive, Transform parent, Vector3 localPosition, Vector3 localScale, Material material, Quaternion localRotation)
        {
            var part = GameObject.CreatePrimitive(primitive);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localRotation = localRotation;
            part.transform.localScale = localScale;

            var renderer = part.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }

            var collider = part.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            return part;
        }

        private static GameObject CreateMeshPart(string name, Mesh mesh, Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material)
        {
            var part = new GameObject(name);
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localRotation = localRotation;
            part.transform.localScale = localScale;
            part.AddComponent<MeshFilter>().sharedMesh = mesh;
            part.AddComponent<MeshRenderer>().sharedMaterial = material;
            return part;
        }

        private static GameObject CreateEmpty(string name, Transform parent)
        {
            var empty = new GameObject(name);
            empty.transform.SetParent(parent, false);
            return empty;
        }

        private static void SavePrefab(GameObject instance, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            UnityEngine.Object.DestroyImmediate(instance);
        }

        private static void RenderContactSheet(string outputRoot, string fileName, CandidateSpec[] candidates, int columns, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreatePreviewEnvironment();

            var instances = new List<GameObject>();
            var spacingX = 2.10f;
            var spacingZ = 2.35f;
            var rows = Mathf.CeilToInt((float)candidates.Length / columns);

            for (var i = 0; i < candidates.Length; i++)
            {
                var candidate = candidates[i];
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + candidate.Id + ".prefab");
                if (prefab == null)
                {
                    Debug.LogWarning("Missing generated prefab for preview: " + candidate.Id);
                    continue;
                }

                var col = i % columns;
                var row = i / columns;
                var x = (col - (columns - 1) * 0.5f) * spacingX;
                var z = (row - (rows - 1) * 0.5f) * spacingZ;
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.name = candidate.Id;
                instance.transform.position = new Vector3(x, 0f, z);
                instances.Add(instance);
                AddPreviewLabel(candidate, new Vector3(x, 0.05f, z - 0.95f));
            }

            var camera = CreatePreviewCamera(50f);
            FrameBounds(camera, CalculateBounds(instances), 1.22f, (float)width / height);
            RenderCameraToPng(camera, Path.Combine(outputRoot, fileName), width, height);
        }

        private static void RenderSingleCandidate(string outputRoot, CandidateSpec candidate, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreatePreviewEnvironment();

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + candidate.Id + ".prefab");
            if (prefab == null)
            {
                Debug.LogWarning("Missing generated prefab for single preview: " + candidate.Id);
                return;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = candidate.Id;
            instance.transform.position = Vector3.zero;
            AddPreviewLabel(candidate, new Vector3(0f, 0.05f, -1.05f));

            var camera = CreatePreviewCamera(candidate.Family == "Warden Overseer" ? 42f : 38f);
            FrameBounds(camera, CalculateBounds(new[] { instance }), 1.30f, (float)width / height);
            RenderCameraToPng(camera, Path.Combine(outputRoot, candidate.Id + "_v0.1.40.png"), width, height);
        }

        private static void AddPreviewLabel(CandidateSpec candidate, Vector3 position)
        {
            var label = new GameObject("preview_label_" + candidate.Id);
            label.transform.position = position;
            label.transform.rotation = Quaternion.Euler(70f, 0f, 0f);
            var text = label.AddComponent<TextMesh>();
            text.text = candidate.Family + "\n" + candidate.Variant;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.075f;
            text.fontSize = 42;
            text.color = new Color(0.88f, 0.78f, 0.58f, 1f);
        }

        private static void CreatePreviewEnvironment()
        {
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.13f, 0.105f, 0.075f, 1f);

            var floorMaterial = new Material(FindLitShader());
            SetMaterialColor(floorMaterial, "_BaseColor", new Color(0.070f, 0.064f, 0.055f, 1f));
            SetMaterialColor(floorMaterial, "_Color", new Color(0.070f, 0.064f, 0.055f, 1f));

            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "preview_foundry_floor";
            floor.transform.localPosition = new Vector3(0f, -0.055f, 0.25f);
            floor.transform.localScale = new Vector3(12f, 0.10f, 9f);
            floor.GetComponent<MeshRenderer>().sharedMaterial = floorMaterial;
            UnityEngine.Object.DestroyImmediate(floor.GetComponent<Collider>());

            var backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backWall.name = "preview_soot_wall";
            backWall.transform.localPosition = new Vector3(0f, 1.85f, 3.10f);
            backWall.transform.localScale = new Vector3(12f, 3.8f, 0.12f);
            backWall.GetComponent<MeshRenderer>().sharedMaterial = floorMaterial;
            UnityEngine.Object.DestroyImmediate(backWall.GetComponent<Collider>());

            var key = new GameObject("preview_key_amber_light").AddComponent<Light>();
            key.type = LightType.Spot;
            key.color = new Color(1f, 0.68f, 0.38f, 1f);
            key.intensity = 900f;
            key.range = 12f;
            key.spotAngle = 56f;
            key.transform.position = new Vector3(-3.4f, 5.0f, -4.0f);
            key.transform.rotation = Quaternion.Euler(58f, 32f, 0f);

            var rim = new GameObject("preview_cyan_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = CyanPilot;
            rim.intensity = 4.0f;
            rim.range = 6f;
            rim.transform.position = new Vector3(3.2f, 2.0f, -2.5f);
        }

        private static Camera CreatePreviewCamera(float fieldOfView)
        {
            var cameraObject = new GameObject("preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.032f, 0.029f, 0.025f, 1f);
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 100f;
            return camera;
        }

        private static Bounds CalculateBounds(IEnumerable<GameObject> roots)
        {
            var renderers = roots.Where(root => root != null).SelectMany(root => root.GetComponentsInChildren<Renderer>()).ToArray();
            if (renderers.Length == 0)
            {
                return new Bounds(Vector3.up, Vector3.one);
            }

            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private static void FrameBounds(Camera camera, Bounds bounds, float padding, float aspect)
        {
            var vertical = Mathf.Max(0.5f, bounds.extents.y);
            var horizontal = Mathf.Max(0.5f, bounds.extents.x / Mathf.Max(0.1f, aspect));
            var halfSize = Mathf.Max(vertical, horizontal) * padding;
            var distance = halfSize / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            camera.transform.position = bounds.center + new Vector3(0f, bounds.extents.y * 0.16f, -distance - bounds.extents.z - 0.55f);
            camera.transform.LookAt(bounds.center + new Vector3(0f, bounds.extents.y * 0.12f, 0f));
        }

        private static void RenderCameraToPng(Camera camera, string absolutePath, int width, int height)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
            var texture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            var previousTarget = camera.targetTexture;
            var previousActive = RenderTexture.active;
            camera.targetTexture = texture;
            RenderTexture.active = texture;
            camera.Render();

            var image = new Texture2D(width, height, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
            image.Apply();
            File.WriteAllBytes(absolutePath, ImageConversion.EncodeToPNG(image));

            camera.targetTexture = previousTarget;
            RenderTexture.active = previousActive;
            UnityEngine.Object.DestroyImmediate(image);
            UnityEngine.Object.DestroyImmediate(texture);
        }

        private static int WriteUnityValidationReport()
        {
            AssetDatabase.Refresh();
            var prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot });
            var materialGuids = AssetDatabase.FindAssets("t:Material", new[] { MaterialRoot });
            var meshGuids = AssetDatabase.FindAssets("t:Mesh", new[] { MeshRoot });
            var pngCount = Directory.Exists(ResolveRepoRelativeFolder(RenderDocFolder)) ? Directory.GetFiles(ResolveRepoRelativeFolder(RenderDocFolder), "*.png").Length : 0;
            var colliderCount = 0;

            foreach (var guid in prefabGuids)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (prefab != null)
                {
                    colliderCount += prefab.GetComponentsInChildren<Collider>(true).Length;
                }
            }

            var errors = 0;
            if (prefabGuids.Length != Candidates.Length) errors++;
            if (materialGuids.Length != 15) errors++;
            if (meshGuids.Length != 8) errors++;
            if (pngCount < 19) errors++;
            if (colliderCount != 0) errors++;

            var reportRoot = ResolveRepoRelativeFolder(ProductionDocFolder);
            Directory.CreateDirectory(reportRoot);
            var reportPath = Path.Combine(reportRoot, "unity_validation_report_v0.1.40.json");
            var json = "{\n" +
                "  \"status\": \"" + (errors == 0 ? "pass" : "fail") + "\",\n" +
                "  \"prefabs\": " + prefabGuids.Length + ",\n" +
                "  \"materials\": " + materialGuids.Length + ",\n" +
                "  \"meshes\": " + meshGuids.Length + ",\n" +
                "  \"preview_pngs\": " + pngCount + ",\n" +
                "  \"colliders_in_prefabs\": " + colliderCount + ",\n" +
                "  \"expected_prefabs\": " + Candidates.Length + ",\n" +
                "  \"expected_materials\": 15,\n" +
                "  \"expected_meshes\": 8,\n" +
                "  \"expected_preview_pngs_minimum\": 19\n" +
                "}\n";
            File.WriteAllText(reportPath, json);
            Debug.Log("MEV01_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL") + " report=" + reportPath);
            return errors;
        }

        private static void EnsurePackageFolders()
        {
            EnsureDirectoryForAssetPath(PrefabRoot);
            EnsureDirectoryForAssetPath(MaterialRoot);
            EnsureDirectoryForAssetPath(MeshRoot);
            AssetDatabase.Refresh();
        }

        private static void EnsureDirectoryForAssetPath(string assetPath)
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly());
            var packageRoot = packageInfo != null ? packageInfo.resolvedPath : Path.Combine(Directory.GetCurrentDirectory(), "AssetPacks", "BrassworksBreach.MechanicalEnemyVisualSet01");
            var relative = assetPath.Substring(PackageRoot.Length).TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
            Directory.CreateDirectory(Path.Combine(packageRoot, relative));
        }

        private static Material UpsertMaterial(string assetName, Color baseColor, float metallic, float smoothness, Color emission, bool transparent)
        {
            var path = MaterialRoot + "/" + assetName + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(FindLitShader());
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = FindLitShader();
            SetMaterialColor(material, "_BaseColor", baseColor);
            SetMaterialColor(material, "_Color", baseColor);
            SetMaterialFloat(material, "_Metallic", metallic);
            SetMaterialFloat(material, "_Smoothness", smoothness);
            SetMaterialFloat(material, "_Glossiness", smoothness);

            if (emission.maxColorComponent > 0.01f)
            {
                material.EnableKeyword("_EMISSION");
                SetMaterialColor(material, "_EmissionColor", emission);
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
            else
            {
                material.DisableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
            }

            if (transparent)
            {
                material.SetOverrideTag("RenderType", "Transparent");
                SetMaterialFloat(material, "_Mode", 3f);
                SetMaterialFloat(material, "_Surface", 1f);
                SetMaterialFloat(material, "_AlphaClip", 0f);
                material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = (int)RenderQueue.Transparent;
            }
            else
            {
                material.SetOverrideTag("RenderType", "");
                SetMaterialFloat(material, "_Mode", 0f);
                SetMaterialFloat(material, "_Surface", 0f);
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHABLEND_ON");
                material.renderQueue = -1;
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static Mesh UpsertMesh(string assetName, Mesh mesh)
        {
            var path = MeshRoot + "/" + assetName + ".asset";
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing == null)
            {
                AssetDatabase.CreateAsset(mesh, path);
                return mesh;
            }

            EditorUtility.CopySerialized(mesh, existing);
            UnityEngine.Object.DestroyImmediate(mesh);
            EditorUtility.SetDirty(existing);
            return existing;
        }

        private static Shader FindLitShader()
        {
            return Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("HDRP/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Sprites/Default");
        }

        private static void SetMaterialColor(Material material, string property, Color color)
        {
            if (material.HasProperty(property))
            {
                material.SetColor(property, color);
            }
        }

        private static void SetMaterialFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property))
            {
                material.SetFloat(property, value);
            }
        }

        private static Mesh CreateSawBladeMesh(string name, int teeth, float innerRadius, float outerRadius)
        {
            var vertices = new List<Vector3> { Vector3.zero };
            var triangles = new List<int>();
            var points = teeth * 2;
            for (var i = 0; i < points; i++)
            {
                var radius = i % 2 == 0 ? outerRadius : innerRadius;
                var angle = i * Mathf.PI * 2f / points;
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
            }

            for (var i = 1; i <= points; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i == points ? 1 : i + 1);
            }

            return BuildMesh(name, vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreateConeMesh(string name, float radius, float length, int segments)
        {
            var vertices = new List<Vector3> { new Vector3(0f, 0f, -length * 0.5f), new Vector3(0f, 0f, length * 0.5f) };
            var triangles = new List<int>();
            for (var i = 0; i < segments; i++)
            {
                var angle = i * Mathf.PI * 2f / segments;
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, -length * 0.5f));
            }

            for (var i = 0; i < segments; i++)
            {
                var a = 2 + i;
                var b = 2 + ((i + 1) % segments);
                triangles.Add(1);
                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(0);
                triangles.Add(b);
                triangles.Add(a);
            }

            return BuildMesh(name, vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreateShieldPlateMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(-0.46f, -0.66f, -0.045f), new Vector3(0.46f, -0.66f, -0.045f), new Vector3(0.56f, 0.18f, -0.045f), new Vector3(0.30f, 0.70f, -0.045f), new Vector3(-0.30f, 0.70f, -0.045f), new Vector3(-0.56f, 0.18f, -0.045f),
                new Vector3(-0.46f, -0.66f, 0.045f), new Vector3(0.46f, -0.66f, 0.045f), new Vector3(0.56f, 0.18f, 0.045f), new Vector3(0.30f, 0.70f, 0.045f), new Vector3(-0.30f, 0.70f, 0.045f), new Vector3(-0.56f, 0.18f, 0.045f)
            };
            var triangles = new[]
            {
                0,1,2, 0,2,5, 5,2,3, 5,3,4,
                6,8,7, 6,11,8, 11,9,8, 11,10,9,
                0,6,7, 0,7,1, 1,7,8, 1,8,2, 2,8,9, 2,9,3,
                3,9,10, 3,10,4, 4,10,11, 4,11,5, 5,11,6, 5,6,0
            };
            return BuildMesh(name, vertices, triangles);
        }

        private static Mesh CreateBellowsRibMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(-0.34f, -0.18f, -0.035f), new Vector3(0.34f, -0.18f, -0.035f), new Vector3(0.28f, 0.18f, -0.035f), new Vector3(-0.28f, 0.18f, -0.035f),
                new Vector3(-0.34f, -0.18f, 0.035f), new Vector3(0.34f, -0.18f, 0.035f), new Vector3(0.28f, 0.18f, 0.035f), new Vector3(-0.28f, 0.18f, 0.035f)
            };
            var triangles = new[]
            {
                0,1,2, 0,2,3,
                4,6,5, 4,7,6,
                0,4,5, 0,5,1,
                1,5,6, 1,6,2,
                2,6,7, 2,7,3,
                3,7,4, 3,4,0
            };
            return BuildMesh(name, vertices, triangles);
        }

        private static Mesh CreateGearRingMesh(string name, int teeth, float innerRadius, float outerRadius)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var points = teeth * 2;
            for (var i = 0; i < points; i++)
            {
                var angle = i * Mathf.PI * 2f / points;
                var radius = i % 2 == 0 ? outerRadius : outerRadius * 0.84f;
                vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
                vertices.Add(new Vector3(Mathf.Cos(angle) * innerRadius, Mathf.Sin(angle) * innerRadius, 0f));
            }

            for (var i = 0; i < points; i++)
            {
                var outerA = i * 2;
                var innerA = outerA + 1;
                var outerB = ((i + 1) % points) * 2;
                var innerB = outerB + 1;
                triangles.Add(outerA);
                triangles.Add(innerA);
                triangles.Add(outerB);
                triangles.Add(outerB);
                triangles.Add(innerA);
                triangles.Add(innerB);
            }

            return BuildMesh(name, vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreateFinMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(0f, -0.16f, -0.075f), new Vector3(0.58f, 0.02f, -0.075f), new Vector3(0.12f, 0.44f, -0.075f),
                new Vector3(0f, -0.16f, 0.075f), new Vector3(0.58f, 0.02f, 0.075f), new Vector3(0.12f, 0.44f, 0.075f)
            };
            var triangles = new[] { 0, 1, 2, 3, 5, 4, 0, 3, 4, 0, 4, 1, 1, 4, 5, 1, 5, 2, 2, 5, 3, 2, 3, 0 };
            return BuildMesh(name, vertices, triangles);
        }

        private static Mesh CreateDrillBitMesh(string name, float radius, float length, int segments)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i <= segments; i++)
            {
                var z = Mathf.Lerp(-length * 0.5f, length * 0.5f, i / (float)segments);
                var twist = i * Mathf.PI * 0.55f;
                vertices.Add(new Vector3(Mathf.Cos(twist) * radius, Mathf.Sin(twist) * radius, z));
                vertices.Add(new Vector3(Mathf.Cos(twist + Mathf.PI) * radius, Mathf.Sin(twist + Mathf.PI) * radius, z));
            }

            for (var i = 0; i < segments; i++)
            {
                var a = i * 2;
                var b = a + 1;
                var c = a + 2;
                var d = a + 3;
                triangles.Add(a);
                triangles.Add(c);
                triangles.Add(b);
                triangles.Add(b);
                triangles.Add(c);
                triangles.Add(d);
            }

            return BuildMesh(name, vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh BuildMesh(string name, Vector3[] vertices, int[] triangles)
        {
            var mesh = new Mesh { name = name };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static string ResolveRepoRelativeFolder(string relativeFolder)
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly());
            var packageRoot = packageInfo != null ? packageInfo.resolvedPath : Path.Combine(Directory.GetCurrentDirectory(), "AssetPacks", "BrassworksBreach.MechanicalEnemyVisualSet01");
            var repoRoot = Path.GetFullPath(Path.Combine(packageRoot, "..", ".."));
            return Path.Combine(repoRoot, relativeFolder.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string MakeSafeFileName(string text)
        {
            var safe = text.Replace(" ", "_").Replace("/", "_").Replace("\\", "_").Replace("-", "_").ToLowerInvariant();
            return safe;
        }

        private sealed class CandidateSpec
        {
            public CandidateSpec(string id, string family, string variant, int variantIndex, float height, float width, float depth, string scaleNote)
            {
                Id = id;
                Family = family;
                Variant = variant;
                VariantIndex = variantIndex;
                Height = height;
                Width = width;
                Depth = depth;
                ScaleNote = scaleNote;
            }

            public string Id { get; private set; }
            public string Family { get; private set; }
            public string Variant { get; private set; }
            public int VariantIndex { get; private set; }
            public float Height { get; private set; }
            public float Width { get; private set; }
            public float Depth { get; private set; }
            public string ScaleNote { get; private set; }
        }
    }
}
