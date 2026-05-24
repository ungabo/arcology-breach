using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace BrassworksBreach.MechanicalEnemyEliteSet05.Editor
{
    public static class MechanicalEnemyEliteSet05Generator
    {
        private const string PackageName = "com.brassworks.sidecar.mechanical-enemy-elite-set05";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string PrefabRoot = PackageRoot + "/Runtime/Prefabs";
        private const string MaterialRoot = PackageRoot + "/Runtime/Materials";
        private const string MeshRoot = PackageRoot + "/Runtime/Meshes";
        private const string ManifestAssetPath = PackageRoot + "/Documentation~/Manifest/MEES05_MechanicalEnemyEliteSet05_Manifest_v0.1.49-p001.json";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_49_MechanicalEnemyEliteSet05";
        private const string ProductionDocFolder = "Documentation/AssetProduction/V0_1_49_MechanicalEnemyEliteSet05";
        private const int ExpectedPrefabs = 25;
        private const int ExpectedMaterials = 18;
        private const int ExpectedMeshes = 12;
        private const int ExpectedPngMinimum = 32;

        private static readonly MatSpec[] Materials =
        {
            new MatSpec("Brass", "MEES05_MAT_AgedBrass", new Color(0.70f, 0.47f, 0.22f, 1f), 0.86f, 0.38f, Color.black, false),
            new MatSpec("Iron", "MEES05_MAT_BlackenedIron", new Color(0.055f, 0.052f, 0.047f, 1f), 0.92f, 0.24f, Color.black, false),
            new MatSpec("Copper", "MEES05_MAT_BurnishedCopper", new Color(0.68f, 0.30f, 0.16f, 1f), 0.78f, 0.34f, Color.black, false),
            new MatSpec("Steel", "MEES05_MAT_HeatBluedSteel", new Color(0.21f, 0.30f, 0.38f, 1f), 0.90f, 0.31f, Color.black, false),
            new MatSpec("Soot", "MEES05_MAT_SootCarbon", new Color(0.020f, 0.019f, 0.017f, 1f), 0.22f, 0.16f, Color.black, false),
            new MatSpec("Furnace", "MEES05_MAT_FurnaceOrangeGlow", new Color(1f, 0.32f, 0.07f, 1f), 0f, 0.22f, new Color(3.2f, 1.0f, 0.22f, 1f), false),
            new MatSpec("Eye", "MEES05_MAT_FurnaceEyeWhiteHot", new Color(1f, 0.83f, 0.42f, 1f), 0f, 0.20f, new Color(2.8f, 2.2f, 1.0f, 1f), false),
            new MatSpec("Weak", "MEES05_MAT_WeakPointRedGlass", new Color(1f, 0.06f, 0.035f, 1f), 0f, 0.46f, new Color(3.4f, 0.14f, 0.09f, 1f), false),
            new MatSpec("Cyan", "MEES05_MAT_PressureCyanRail", new Color(0.08f, 0.72f, 1f, 1f), 0f, 0.35f, new Color(0.22f, 2.0f, 2.8f, 1f), false),
            new MatSpec("Patina", "MEES05_MAT_VerdigrisPressurePipe", new Color(0.15f, 0.49f, 0.41f, 1f), 0.68f, 0.36f, Color.black, false),
            new MatSpec("Shield", "MEES05_MAT_BoilerShieldEnamel", new Color(0.78f, 0.76f, 0.66f, 1f), 0.18f, 0.30f, Color.black, false),
            new MatSpec("Hazard", "MEES05_MAT_ChippedHazardOchre", new Color(0.92f, 0.64f, 0.10f, 1f), 0.12f, 0.29f, Color.black, false),
            new MatSpec("Command", "MEES05_MAT_CommandHaloGold", new Color(1f, 0.71f, 0.24f, 1f), 0.88f, 0.45f, new Color(1.6f, 1.1f, 0.34f, 1f), false),
            new MatSpec("Ceramic", "MEES05_MAT_WardenBoneCeramic", new Color(0.74f, 0.67f, 0.55f, 1f), 0f, 0.26f, Color.black, false),
            new MatSpec("Leather", "MEES05_MAT_OilyBellowsLeather", new Color(0.18f, 0.085f, 0.040f, 1f), 0f, 0.42f, Color.black, false),
            new MatSpec("Ghost", "MEES05_MAT_GhostReadabilityEnvelope", new Color(0.53f, 0.66f, 0.70f, 0.22f), 0f, 0.18f, Color.black, true),
            new MatSpec("Impact", "MEES05_MAT_ImpactSparkYellow", new Color(1f, 0.89f, 0.23f, 1f), 0f, 0.28f, new Color(2.6f, 2.2f, 0.45f, 1f), false),
            new MatSpec("Overcrank", "MEES05_MAT_OvercrankMagentaSeal", new Color(0.78f, 0.15f, 0.38f, 1f), 0f, 0.32f, new Color(2.2f, 0.38f, 1.05f, 1f), false)
        };

        private static readonly Spec[] Specs =
        {
            new Spec("MEES05_Scrapper_A_IdleReadability_BoilerSaw", "Scrapper", "IdleReadability", 1.58f, 1.20f, 1.10f),
            new Spec("MEES05_Scrapper_B_AttackWindup_FoldingSaw", "Scrapper", "AttackWindup", 1.72f, 1.34f, 1.18f),
            new Spec("MEES05_Scrapper_C_ImpactProxy_TwinSawSmash", "Scrapper", "ImpactProxy", 1.62f, 1.46f, 1.28f),
            new Spec("MEES05_Scrapper_D_WeakPointMarked_ValveBack", "Scrapper", "WeakPointMarked", 1.66f, 1.28f, 1.16f),
            new Spec("MEES05_Scrapper_E_EliteOvercrank_CinderMaw", "Scrapper", "EliteOvercrank", 1.90f, 1.55f, 1.34f),
            new Spec("MEES05_Lancer_A_IdleReadability_RailLance", "Lancer", "IdleReadability", 2.22f, 0.92f, 1.52f),
            new Spec("MEES05_Lancer_B_AttackWindup_PikeCharge", "Lancer", "AttackWindup", 2.38f, 1.02f, 1.86f),
            new Spec("MEES05_Lancer_C_ImpactProxy_DrillPierce", "Lancer", "ImpactProxy", 2.16f, 1.02f, 1.94f),
            new Spec("MEES05_Lancer_D_WeakPointMarked_PressureSpine", "Lancer", "WeakPointMarked", 2.30f, 0.98f, 1.58f),
            new Spec("MEES05_Lancer_E_EliteOvercrank_DualRail", "Lancer", "EliteOvercrank", 2.50f, 1.18f, 2.05f),
            new Spec("MEES05_Bulwark_A_IdleReadability_BoilerShield", "Bulwark", "IdleReadability", 2.18f, 1.72f, 1.10f),
            new Spec("MEES05_Bulwark_B_AttackWindup_ShieldBash", "Bulwark", "AttackWindup", 2.32f, 1.92f, 1.24f),
            new Spec("MEES05_Bulwark_C_ImpactProxy_AnchorCrush", "Bulwark", "ImpactProxy", 2.28f, 2.02f, 1.38f),
            new Spec("MEES05_Bulwark_D_WeakPointMarked_FurnaceCore", "Bulwark", "WeakPointMarked", 2.26f, 1.86f, 1.18f),
            new Spec("MEES05_Bulwark_E_EliteOvercrank_IronGate", "Bulwark", "EliteOvercrank", 2.55f, 2.12f, 1.34f),
            new Spec("MEES05_Warden_A_IdleReadability_CommandHalo", "Warden", "IdleReadability", 2.72f, 1.22f, 1.18f),
            new Spec("MEES05_Warden_B_AttackWindup_SignalRaise", "Warden", "AttackWindup", 2.92f, 1.36f, 1.22f),
            new Spec("MEES05_Warden_C_ImpactProxy_BellShock", "Warden", "ImpactProxy", 2.80f, 1.50f, 1.30f),
            new Spec("MEES05_Warden_D_WeakPointMarked_CrownValve", "Warden", "WeakPointMarked", 2.86f, 1.34f, 1.20f),
            new Spec("MEES05_Warden_E_EliteOvercrank_TwinHalo", "Warden", "EliteOvercrank", 3.12f, 1.56f, 1.32f),
            new Spec("MEES05_BossPhase_A_Phase01_Readability_GovernorMass", "BossPhase", "IdleReadability", 3.10f, 2.35f, 1.70f),
            new Spec("MEES05_BossPhase_B_Phase02_AttackWindup_OvercrankRails", "BossPhase", "AttackWindup", 3.38f, 2.60f, 2.05f),
            new Spec("MEES05_BossPhase_C_Phase03_ImpactProxy_FurnaceSlam", "BossPhase", "ImpactProxy", 3.22f, 2.72f, 2.10f),
            new Spec("MEES05_BossPhase_D_Phase04_WeakPointMarked_CentralBoiler", "BossPhase", "WeakPointMarked", 3.28f, 2.58f, 1.92f),
            new Spec("MEES05_BossPhase_E_Phase05_FinalSilhouette_CommandCrown", "BossPhase", "EliteOvercrank", 3.62f, 2.86f, 2.18f)
        };

        [MenuItem("Brassworks Breach/Sidecars/Mechanical Enemy Elite Set 05/Generate Package")]
        public static void GeneratePackage()
        {
            EnsureFolders();
            Dictionary<string, Material> mat = CreateMaterials();
            Dictionary<string, Mesh> mesh = CreateMeshes();
            for (int i = 0; i < Specs.Length; i++)
            {
                GameObject root = BuildPrefab(Specs[i], mat, mesh);
                PrefabUtility.SaveAsPrefabAsset(root, PrefabRoot + "/" + Specs[i].Id + ".prefab");
                UnityEngine.Object.DestroyImmediate(root);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            WriteManifest("generated_prefabs");
        }

        [MenuItem("Brassworks Breach/Sidecars/Mechanical Enemy Elite Set 05/Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            GeneratePackage();
            string outRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            Directory.CreateDirectory(outRoot);
            RenderContactSheet(outRoot, "MEES05_v0.1.49_all_prefabs_contact_sheet.png", Specs, 5, 3600, 2400);
            foreach (string family in Specs.Select(s => s.Family).Distinct())
            {
                RenderContactSheet(outRoot, "MEES05_v0.1.49_" + Safe(family) + "_contact_sheet.png", Specs.Where(s => s.Family == family).ToArray(), 5, 2600, 1100);
            }

            for (int i = 0; i < Specs.Length; i++)
            {
                RenderSingle(outRoot, Specs[i], 1400, 1400);
            }

            WriteMaterialSwatches(outRoot);
            AssetDatabase.Refresh();
            WriteManifest("preview_renders_generated");
        }

        public static void GenerateValidateAndQuit()
        {
            int errors = 0;
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

        private static GameObject BuildPrefab(Spec spec, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = new GameObject(spec.Id);
            Dictionary<string, Transform> groups = CreateGroups(root.transform, spec);
            AddCore(spec, groups, mat, mesh);

            if (spec.Family == "Scrapper")
            {
                AddScrapper(groups, spec, mat, mesh);
            }
            else if (spec.Family == "Lancer")
            {
                AddLancer(groups, spec, mat, mesh);
            }
            else if (spec.Family == "Bulwark")
            {
                AddBulwark(groups, spec, mat, mesh);
            }
            else if (spec.Family == "Warden")
            {
                AddWarden(groups, spec, mat, mesh);
            }
            else
            {
                AddBossPhase(groups, spec, mat, mesh);
            }

            AddPoseProxy(groups, spec, mat, mesh);
            return root;
        }

        private static Dictionary<string, Transform> CreateGroups(Transform root, Spec spec)
        {
            string[] names =
            {
                "visual_only_no_gameplay_authority",
                "visual_authority_none_runtime_systems_absent",
                "family_" + Safe(spec.Family),
                "pose_proxy_" + Safe(spec.Pose),
                "chassis",
                "boiler",
                "weak_point_markers",
                "furnace_eyes",
                "saw_arms",
                "lance_rails",
                "boiler_shields",
                "command_halos",
                "pressure_lines",
                "rivets",
                "warning_lamps",
                "smoke_stacks",
                "armor_plates",
                "readability_pose",
                "attack_windup_pose",
                "impact_proxy_pose"
            };

            Dictionary<string, Transform> groups = new Dictionary<string, Transform>(StringComparer.Ordinal);
            for (int i = 0; i < names.Length; i++)
            {
                GameObject child = new GameObject(names[i]);
                child.transform.SetParent(root, false);
                groups[names[i]] = child.transform;
            }

            return groups;
        }

        private static void AddCore(Spec s, Dictionary<string, Transform> g, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            Part("chassis_floor_contact_iron", PrimitiveType.Cube, g["chassis"], new Vector3(0f, 0.18f, 0.04f), new Vector3(s.Width * 0.72f, 0.20f, s.Depth * 0.52f), mat["Iron"]);
            Part("boiler_main_riveted_pressure_body", PrimitiveType.Cylinder, g["boiler"], new Vector3(0f, s.Height * 0.46f, 0.06f), new Vector3(s.Width * 0.34f, s.Height * 0.22f, s.Width * 0.34f), mat["Brass"]);
            Part("boiler_blackened_belly_band", PrimitiveType.Cylinder, g["boiler"], new Vector3(0f, s.Height * 0.46f, 0.06f), new Vector3(s.Width * 0.36f, 0.035f, s.Width * 0.36f), mat["Iron"]);
            Pipe(g["pressure_lines"], "pressure_line_left_copper_loop", new Vector3(-s.Width * 0.20f, s.Height * 0.46f, 0.28f), new Vector3(-s.Width * 0.42f, s.Height * 0.28f, -0.20f), 0.032f, mat["Patina"]);
            Pipe(g["pressure_lines"], "pressure_line_right_copper_loop", new Vector3(s.Width * 0.20f, s.Height * 0.46f, 0.28f), new Vector3(s.Width * 0.42f, s.Height * 0.28f, -0.20f), 0.032f, mat["Patina"]);
            RivetRing(g["rivets"], "rivet_belt_primary", 18, s.Width * 0.25f, s.Height * 0.58f, mat["Command"]);
            WeakMarker(g["weak_point_markers"], "weak_point_marker_chest_proxy", new Vector3(0f, s.Height * 0.52f, -s.Depth * 0.36f), Quaternion.identity, s.Width * 0.30f, s.Pose == "WeakPointMarked", mat, mesh);
            Part("readability_pose_scale_envelope", PrimitiveType.Cube, g["readability_pose"], new Vector3(0f, s.Height * 0.5f, 0f), new Vector3(s.Width, s.Height, s.Depth), mat["Ghost"]);
            Part("readability_pose_floor_shadow", PrimitiveType.Cube, g["readability_pose"], new Vector3(0f, 0.025f, 0f), new Vector3(s.Width, 0.035f, s.Depth), mat["Ghost"]);
        }

        private static void AddScrapper(Dictionary<string, Transform> g, Spec s, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            float lift = s.Pose == "AttackWindup" ? 0.30f : 0f;
            EyePair(g["furnace_eyes"], "furnace_eyes_scrapper", new Vector3(0f, s.Height * 0.70f, -s.Depth * 0.38f), s.Width * 0.13f, mat[s.Pose == "EliteOvercrank" ? "Overcrank" : "Eye"], mesh);
            PistonArm(g["saw_arms"], "saw_arm_left", new Vector3(-s.Width * 0.42f, s.Height * 0.42f + lift, -0.12f), -30f, mat, mesh);
            PistonArm(g["saw_arms"], "saw_arm_right", new Vector3(s.Width * 0.42f, s.Height * 0.40f, -0.12f), 30f, mat, mesh);
            MeshPart("saw_arm_left_34tooth_blade", mesh["Saw"], g["saw_arms"], new Vector3(-s.Width * 0.64f, s.Height * 0.34f + lift, -s.Depth * 0.34f), Vector3.one * 1.05f, Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
            MeshPart("saw_arm_right_34tooth_blade", mesh["Saw"], g["saw_arms"], new Vector3(s.Width * 0.64f, s.Height * 0.34f, -s.Depth * 0.34f), Vector3.one * 0.95f, Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
            Stack(g["smoke_stacks"], "smoke_stack_scrapper", new Vector3(0f, s.Height * 0.76f, 0.28f), 0.06f, 0.35f, mat);
        }

        private static void AddLancer(Dictionary<string, Transform> g, Spec s, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            float railZ = s.Pose == "AttackWindup" || s.Pose == "ImpactProxy" ? -s.Depth * 0.78f : -s.Depth * 0.54f;
            EyeSingle(g["furnace_eyes"], "furnace_eye_lancer_mono", new Vector3(0f, s.Height * 0.78f, -s.Depth * 0.24f), s.Width * 0.16f, mat["Cyan"], mesh);
            MeshPart("lance_rail_left_notched", mesh["Rail"], g["lance_rails"], new Vector3(-s.Width * 0.17f, s.Height * 0.55f, railZ), new Vector3(0.72f, 0.72f, s.Depth * 1.15f), Quaternion.identity, mat["Steel"]);
            MeshPart("lance_rail_right_notched", mesh["Rail"], g["lance_rails"], new Vector3(s.Width * 0.17f, s.Height * 0.55f, railZ), new Vector3(0.72f, 0.72f, s.Depth * 1.15f), Quaternion.identity, mat["Steel"]);
            Pipe(g["lance_rails"], "lance_rail_cyan_charge_line", new Vector3(0f, s.Height * 0.55f, -0.05f), new Vector3(0f, s.Height * 0.55f, railZ - s.Depth * 0.60f), 0.035f, mat[s.Pose == "EliteOvercrank" ? "Overcrank" : "Cyan"]);
            MeshPart("lance_rail_long_tip", mesh["LanceTip"], g["lance_rails"], new Vector3(0f, s.Height * 0.55f, railZ - s.Depth * 0.78f), Vector3.one, Quaternion.identity, mat["Command"]);
            PistonArm(g["saw_arms"], "lance_support_arm_left", new Vector3(-s.Width * 0.40f, s.Height * 0.48f, -0.02f), -16f, mat, mesh);
            PistonArm(g["saw_arms"], "lance_support_arm_right", new Vector3(s.Width * 0.40f, s.Height * 0.48f, -0.02f), 16f, mat, mesh);
        }

        private static void AddBulwark(Dictionary<string, Transform> g, Spec s, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            EyePair(g["furnace_eyes"], "furnace_eyes_bulwark", new Vector3(0f, s.Height * 0.70f, -s.Depth * 0.45f), s.Width * 0.11f, mat["Eye"], mesh);
            MeshPart("boiler_shield_primary_heavy_plate", mesh["Shield"], g["boiler_shields"], new Vector3(s.Pose == "AttackWindup" ? -0.18f : 0f, s.Height * 0.45f, -s.Depth * 0.55f), new Vector3(s.Width, s.Height * 0.75f, 1f), Quaternion.identity, mat["Shield"]);
            Part("boiler_shield_hazard_lower_lip", PrimitiveType.Cube, g["boiler_shields"], new Vector3(0f, s.Height * 0.18f, -s.Depth * 0.58f), new Vector3(s.Width * 0.60f, 0.10f, 0.06f), mat["Hazard"]);
            PistonArm(g["saw_arms"], "hammer_saw_proxy_arm", new Vector3(-s.Width * 0.46f, s.Height * 0.50f, -0.08f), -18f, mat, mesh);
            MeshPart("hammer_saw_roundel", mesh["Saw"], g["saw_arms"], new Vector3(-s.Width * 0.64f, s.Height * 0.40f, -s.Depth * 0.30f), Vector3.one * 0.80f, Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
            Stack(g["smoke_stacks"], "smoke_stack_bulwark_left", new Vector3(-s.Width * 0.16f, s.Height * 0.84f, 0.30f), 0.06f, 0.42f, mat);
            Stack(g["smoke_stacks"], "smoke_stack_bulwark_right", new Vector3(s.Width * 0.16f, s.Height * 0.84f, 0.30f), 0.06f, 0.42f, mat);
        }

        private static void AddWarden(Dictionary<string, Transform> g, Spec s, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            EyeSingle(g["furnace_eyes"], "furnace_eye_warden_mono", new Vector3(0f, s.Height * 0.78f, -s.Depth * 0.22f), s.Width * 0.16f, mat[s.Pose == "EliteOvercrank" ? "Overcrank" : "Eye"], mesh);
            MeshPart("command_halo_primary_gear", mesh["Halo"], g["command_halos"], new Vector3(0f, s.Height * 0.82f, 0.22f), Vector3.one * (s.Pose == "EliteOvercrank" ? 1.25f : 1f), Quaternion.identity, mat["Command"]);
            if (s.Pose == "EliteOvercrank")
            {
                MeshPart("command_halo_secondary_overcrank_gear", mesh["Halo"], g["command_halos"], new Vector3(0f, s.Height * 0.82f, 0.16f), Vector3.one * 0.82f, Quaternion.Euler(0f, 0f, 16f), mat["Overcrank"]);
            }

            PistonArm(g["saw_arms"], "signal_arm_left", new Vector3(-s.Width * 0.42f, s.Height * (s.Pose == "AttackWindup" ? 0.70f : 0.55f), -0.04f), -22f, mat, mesh);
            Part("command_staff_signal_rail", PrimitiveType.Cylinder, g["lance_rails"], new Vector3(-s.Width * 0.55f, s.Height * 0.58f, -0.18f), new Vector3(0.045f, s.Height * 0.25f, 0.045f), mat["Steel"]);
            MeshPart("command_staff_lance_tip", mesh["LanceTip"], g["lance_rails"], new Vector3(-s.Width * 0.55f, s.Height * 0.86f, -0.18f), Vector3.one * 0.70f, Quaternion.Euler(90f, 0f, 0f), mat["Command"]);
            Stack(g["smoke_stacks"], "smoke_stack_warden_censer", new Vector3(0f, s.Height * 0.88f, 0.32f), 0.05f, 0.35f, mat);
        }

        private static void AddBossPhase(Dictionary<string, Transform> g, Spec s, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            EyePair(g["furnace_eyes"], "furnace_eyes_boss_phase", new Vector3(0f, s.Height * 0.70f, -s.Depth * 0.38f), s.Width * 0.10f, mat[s.Pose == "EliteOvercrank" ? "Overcrank" : "Eye"], mesh);
            MeshPart("command_halo_boss_rear_gear", mesh["Halo"], g["command_halos"], new Vector3(0f, s.Height * 0.80f, 0.30f), Vector3.one * 1.35f, Quaternion.identity, mat["Command"]);
            MeshPart("command_halo_boss_crown", mesh["Crown"], g["command_halos"], new Vector3(0f, s.Height * 0.94f, 0.02f), Vector3.one * (s.Pose == "EliteOvercrank" ? 1.35f : 1.05f), Quaternion.identity, mat[s.Pose == "EliteOvercrank" ? "Overcrank" : "Command"]);
            MeshPart("boiler_shield_left_phase_plate", mesh["Shield"], g["boiler_shields"], new Vector3(-s.Width * 0.33f, s.Height * 0.46f, -s.Depth * 0.45f), Vector3.one * 1.20f, Quaternion.Euler(0f, 0f, -8f), mat["Shield"]);
            MeshPart("boiler_shield_right_phase_plate", mesh["Shield"], g["boiler_shields"], new Vector3(s.Width * 0.33f, s.Height * 0.46f, -s.Depth * 0.45f), Vector3.one * 1.20f, Quaternion.Euler(0f, 0f, 8f), mat["Shield"]);
            MeshPart("lance_rail_boss_left", mesh["Rail"], g["lance_rails"], new Vector3(-s.Width * 0.22f, s.Height * 0.62f, -s.Depth * 0.58f), new Vector3(0.92f, 0.92f, s.Depth * 1.20f), Quaternion.identity, mat["Steel"]);
            MeshPart("lance_rail_boss_right", mesh["Rail"], g["lance_rails"], new Vector3(s.Width * 0.22f, s.Height * 0.62f, -s.Depth * 0.58f), new Vector3(0.92f, 0.92f, s.Depth * 1.20f), Quaternion.identity, mat["Steel"]);
            PistonArm(g["saw_arms"], "boss_saw_arm_left", new Vector3(-s.Width * 0.48f, s.Height * 0.43f, -0.08f), -28f, mat, mesh);
            PistonArm(g["saw_arms"], "boss_saw_arm_right", new Vector3(s.Width * 0.48f, s.Height * 0.43f, -0.08f), 28f, mat, mesh);
            MeshPart("boss_saw_arm_left_blade", mesh["Saw"], g["saw_arms"], new Vector3(-s.Width * 0.62f, s.Height * 0.35f, -s.Depth * 0.25f), Vector3.one * 1.0f, Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
            MeshPart("boss_saw_arm_right_blade", mesh["Saw"], g["saw_arms"], new Vector3(s.Width * 0.62f, s.Height * 0.35f, -s.Depth * 0.25f), Vector3.one * 1.0f, Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
            Stack(g["smoke_stacks"], "smoke_stack_boss_left", new Vector3(-s.Width * 0.16f, s.Height * 0.82f, 0.45f), 0.075f, 0.50f, mat);
            Stack(g["smoke_stacks"], "smoke_stack_boss_right", new Vector3(s.Width * 0.16f, s.Height * 0.82f, 0.45f), 0.075f, 0.50f, mat);
        }

        private static void AddPoseProxy(Dictionary<string, Transform> g, Spec s, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            if (s.Pose == "AttackWindup")
            {
                Part("attack_windup_pose_amber_intent_left", PrimitiveType.Cube, g["attack_windup_pose"], new Vector3(-s.Width * 0.30f, s.Height * 0.65f, -s.Depth * 0.62f), new Vector3(0.075f, s.Height * 0.40f, 0.075f), mat["Impact"]);
                Part("attack_windup_pose_amber_intent_right", PrimitiveType.Cube, g["attack_windup_pose"], new Vector3(s.Width * 0.30f, s.Height * 0.65f, -s.Depth * 0.62f), new Vector3(0.075f, s.Height * 0.40f, 0.075f), mat["Impact"]);
            }

            if (s.Pose == "ImpactProxy")
            {
                MeshPart("impact_proxy_pose_radial_burst_visual_only", mesh["Burst"], g["impact_proxy_pose"], new Vector3(0f, s.Height * 0.25f, -s.Depth * 0.70f), Vector3.one * s.Width, Quaternion.Euler(0f, 180f, 0f), mat["Impact"]);
            }

            if (s.Pose == "EliteOvercrank")
            {
                Part("warning_lamp_overcrank_pose_top", PrimitiveType.Sphere, g["warning_lamps"], new Vector3(0f, s.Height * 0.90f, -0.18f), Vector3.one * 0.16f, mat["Overcrank"]);
            }
        }

        private static void PistonArm(Transform parent, string name, Vector3 position, float angleZ, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, angleZ);
            MeshPart(name + "_diamond_strut", mesh["Strut"], parent, position, Vector3.one * 0.82f, rotation, mat["Steel"]);
            Pipe(parent, name + "_copper_piston", position + rotation * new Vector3(0f, -0.18f, 0f), position + rotation * new Vector3(0f, -0.58f, -0.08f), 0.034f, mat["Copper"]);
            Part(name + "_blackened_joint", PrimitiveType.Sphere, parent, position, Vector3.one * 0.13f, mat["Iron"]);
        }

        private static void EyePair(Transform parent, string prefix, Vector3 center, float radius, Material material, Dictionary<string, Mesh> mesh)
        {
            EyeSingle(parent, prefix + "_left", center + new Vector3(-radius * 0.9f, 0f, 0f), radius, material, mesh);
            EyeSingle(parent, prefix + "_right", center + new Vector3(radius * 0.9f, 0f, 0f), radius, material, mesh);
        }

        private static void EyeSingle(Transform parent, string name, Vector3 position, float radius, Material material, Dictionary<string, Mesh> mesh)
        {
            MeshPart(name, mesh["Eye"], parent, position, new Vector3(radius * 2f, radius * 1.35f, 1f), Quaternion.Euler(0f, 180f, 0f), material);
        }

        private static void WeakMarker(Transform parent, string name, Vector3 position, Quaternion rotation, float scale, bool emphasized, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            MeshPart(name + "_outer_ring", mesh["WeakRing"], parent, position, Vector3.one * scale, rotation, emphasized ? mat["Weak"] : mat["Overcrank"]);
            Part(name + "_inner_dot", PrimitiveType.Sphere, parent, position + new Vector3(0f, 0f, -0.035f), Vector3.one * (emphasized ? 0.17f : 0.11f), emphasized ? mat["Weak"] : mat["Impact"]);
            if (emphasized)
            {
                Part(name + "_crossbar_horizontal", PrimitiveType.Cube, parent, position + new Vector3(0f, 0f, -0.055f), new Vector3(scale * 0.48f, 0.035f, 0.035f), mat["Weak"]);
                Part(name + "_crossbar_vertical", PrimitiveType.Cube, parent, position + new Vector3(0f, 0f, -0.060f), new Vector3(0.035f, scale * 0.48f, 0.035f), mat["Weak"]);
            }
        }

        private static void Stack(Transform parent, string name, Vector3 position, float radius, float height, Dictionary<string, Material> mat)
        {
            Part(name + "_pipe", PrimitiveType.Cylinder, parent, position + new Vector3(0f, height * 0.5f, 0f), new Vector3(radius * 2f, height * 0.5f, radius * 2f), mat["Iron"]);
            Part(name + "_hot_lip", PrimitiveType.Cylinder, parent, position + new Vector3(0f, height + 0.02f, 0f), new Vector3(radius * 2.5f, 0.035f, radius * 2.5f), mat["Furnace"]);
        }

        private static void RivetRing(Transform parent, string prefix, int count, float radius, float y, Material material)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = i * Mathf.PI * 2f / count;
                Part(prefix + "_" + i.ToString("00"), PrimitiveType.Sphere, parent, new Vector3(Mathf.Cos(angle) * radius, y, Mathf.Sin(angle) * radius), Vector3.one * 0.05f, material);
            }
        }

        private static GameObject Part(string name, PrimitiveType primitiveType, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            GameObject part = GameObject.CreatePrimitive(primitiveType);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localScale = localScale;
            part.GetComponent<MeshRenderer>().sharedMaterial = material;
            Collider collider = part.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            return part;
        }

        private static GameObject MeshPart(string name, Mesh mesh, Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material)
        {
            GameObject part = new GameObject(name);
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localScale = localScale;
            part.transform.localRotation = localRotation;
            part.AddComponent<MeshFilter>().sharedMesh = mesh;
            part.AddComponent<MeshRenderer>().sharedMaterial = material;
            return part;
        }

        private static GameObject Pipe(Transform parent, string name, Vector3 from, Vector3 to, float radius, Material material)
        {
            Vector3 delta = to - from;
            GameObject pipe = Part(name, PrimitiveType.Cylinder, parent, (from + to) * 0.5f, new Vector3(radius * 2f, Mathf.Max(0.001f, delta.magnitude) * 0.5f, radius * 2f), material);
            pipe.transform.localRotation = Quaternion.FromToRotation(Vector3.up, delta.normalized);
            return pipe;
        }

        private static Dictionary<string, Material> CreateMaterials()
        {
            Dictionary<string, Material> result = new Dictionary<string, Material>(StringComparer.Ordinal);
            for (int i = 0; i < Materials.Length; i++)
            {
                result[Materials[i].Key] = UpsertMaterial(Materials[i]);
            }

            return result;
        }

        private static Material UpsertMaterial(MatSpec spec)
        {
            string path = MaterialRoot + "/" + spec.AssetName + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(FindLitShader());
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = FindLitShader();
            SetColor(material, "_BaseColor", spec.Color);
            SetColor(material, "_Color", spec.Color);
            SetFloat(material, "_Metallic", spec.Metallic);
            SetFloat(material, "_Smoothness", spec.Smoothness);
            SetFloat(material, "_Glossiness", spec.Smoothness);
            SetFloat(material, "_Cull", (float)CullMode.Off);

            if (spec.Emission.maxColorComponent > 0.01f)
            {
                material.EnableKeyword("_EMISSION");
                SetColor(material, "_EmissionColor", spec.Emission);
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
            else
            {
                material.DisableKeyword("_EMISSION");
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
            }

            if (spec.Transparent)
            {
                material.SetOverrideTag("RenderType", "Transparent");
                SetFloat(material, "_Mode", 3f);
                SetFloat(material, "_Surface", 1f);
                material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = (int)RenderQueue.Transparent;
            }
            else
            {
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHABLEND_ON");
                material.renderQueue = -1;
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            Dictionary<string, Mesh> mesh = new Dictionary<string, Mesh>(StringComparer.Ordinal);
            mesh["Saw"] = UpsertMesh("MEES05_MESH_34ToothSawBlade", SawBlade("MEES05_MESH_34ToothSawBlade", 34, 0.18f, 0.43f));
            mesh["LanceTip"] = UpsertMesh("MEES05_MESH_RivetLanceTipLong", Cone("MEES05_MESH_RivetLanceTipLong", 0.18f, 0.92f, 24));
            mesh["Rail"] = UpsertMesh("MEES05_MESH_LanceRailNotched", Box("MEES05_MESH_LanceRailNotched", new Vector3(0.16f, 0.16f, 1.0f)));
            mesh["Shield"] = UpsertMesh("MEES05_MESH_BoilerShieldPlateHeavy", Shield("MEES05_MESH_BoilerShieldPlateHeavy"));
            mesh["Halo"] = UpsertMesh("MEES05_MESH_CommandGearHalo32", GearRing("MEES05_MESH_CommandGearHalo32", 32, 0.28f, 0.54f));
            mesh["WeakRing"] = UpsertMesh("MEES05_MESH_WeakPointMarkerRing", GearRing("MEES05_MESH_WeakPointMarkerRing", 16, 0.18f, 0.28f));
            mesh["Eye"] = UpsertMesh("MEES05_MESH_FurnaceEyeSlit", OvalDisc("MEES05_MESH_FurnaceEyeSlit", 0.24f, 0.08f, 24));
            mesh["Strut"] = UpsertMesh("MEES05_MESH_PistonStrutDiamond", Diamond("MEES05_MESH_PistonStrutDiamond", 0.10f, 0.52f));
            mesh["Rib"] = UpsertMesh("MEES05_MESH_BoilerRibBand", Box("MEES05_MESH_BoilerRibBand", new Vector3(0.72f, 0.10f, 0.18f)));
            mesh["Burst"] = UpsertMesh("MEES05_MESH_ImpactBurstProxy", Burst("MEES05_MESH_ImpactBurstProxy", 18, 0.12f, 0.62f));
            mesh["Crown"] = UpsertMesh("MEES05_MESH_BossCommandCrown", Crown("MEES05_MESH_BossCommandCrown"));
            mesh["Bellows"] = UpsertMesh("MEES05_MESH_BellowsRibSegment", Box("MEES05_MESH_BellowsRibSegment", new Vector3(0.52f, 0.18f, 0.08f)));
            return mesh;
        }

        private static Mesh UpsertMesh(string assetName, Mesh mesh)
        {
            string path = MeshRoot + "/" + assetName + ".asset";
            Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
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

        private static void RenderContactSheet(string outputRoot, string fileName, Spec[] specs, int columns, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SetupPreviewWorld();
            List<GameObject> roots = new List<GameObject>();
            for (int i = 0; i < specs.Length; i++)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + specs[i].Id + ".prefab");
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.transform.position = new Vector3((i % columns - (columns - 1) * 0.5f) * 3.9f, 0f, (i / columns) * 3.15f);
                roots.Add(instance);
            }

            Camera camera = PreviewCamera(specs.Length > 5 ? 33f : 31f);
            Frame(camera, BoundsFor(roots), specs.Length > 5 ? 1.25f : 1.32f, width / (float)height);
            RenderCamera(camera, Path.Combine(outputRoot, fileName), width, height);
        }

        private static void RenderSingle(string outputRoot, Spec spec, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SetupPreviewWorld();
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + spec.Id + ".prefab");
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Camera camera = PreviewCamera(32f);
            Frame(camera, BoundsFor(new[] { instance }), 1.34f, width / (float)height);
            RenderCamera(camera, Path.Combine(outputRoot, spec.Id + "_preview.png"), width, height);
        }

        private static void SetupPreviewWorld()
        {
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.24f, 0.21f, 0.18f, 1f);
            Material floorMaterial = new Material(FindLitShader());
            SetColor(floorMaterial, "_BaseColor", new Color(0.070f, 0.064f, 0.055f, 1f));
            SetColor(floorMaterial, "_Color", new Color(0.070f, 0.064f, 0.055f, 1f));
            Part("preview_foundry_floor", PrimitiveType.Cube, null, new Vector3(0f, -0.055f, 3f), new Vector3(22f, 0.10f, 19f), floorMaterial);
            Part("preview_soot_wall", PrimitiveType.Cube, null, new Vector3(0f, 2.2f, 8.4f), new Vector3(22f, 4.5f, 0.12f), floorMaterial);

            Light key = new GameObject("preview_key_amber_light").AddComponent<Light>();
            key.type = LightType.Spot;
            key.color = new Color(1f, 0.68f, 0.38f, 1f);
            key.intensity = 900f;
            key.range = 16f;
            key.spotAngle = 58f;
            key.transform.position = new Vector3(-4.6f, 6.0f, -4.8f);
            key.transform.rotation = Quaternion.Euler(58f, 34f, 0f);

            Light rim = new GameObject("preview_cyan_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(0.08f, 0.72f, 1f, 1f);
            rim.intensity = 4.2f;
            rim.range = 7f;
            rim.transform.position = new Vector3(4.4f, 2.8f, -3.0f);
        }

        private static Camera PreviewCamera(float fov)
        {
            Camera camera = new GameObject("preview_camera").AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.032f, 0.029f, 0.025f, 1f);
            camera.fieldOfView = fov;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 100f;
            return camera;
        }

        private static Bounds BoundsFor(IEnumerable<GameObject> roots)
        {
            Renderer[] renderers = roots.Where(r => r != null).SelectMany(r => r.GetComponentsInChildren<Renderer>()).ToArray();
            if (renderers.Length == 0)
            {
                return new Bounds(Vector3.up, Vector3.one);
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private static void Frame(Camera camera, Bounds bounds, float padding, float aspect)
        {
            float vertical = Mathf.Max(0.5f, bounds.extents.y);
            float horizontal = Mathf.Max(0.5f, bounds.extents.x / Mathf.Max(0.1f, aspect));
            float halfSize = Mathf.Max(vertical, horizontal) * padding;
            float distance = halfSize / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            camera.transform.position = bounds.center + new Vector3(0f, bounds.extents.y * 0.18f, -distance - bounds.extents.z - 0.85f);
            camera.transform.LookAt(bounds.center + new Vector3(0f, bounds.extents.y * 0.12f, 0f));
        }

        private static void RenderCamera(Camera camera, string absolutePath, int width, int height)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
            RenderTexture texture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            RenderTexture previous = RenderTexture.active;
            camera.targetTexture = texture;
            RenderTexture.active = texture;
            camera.Render();
            Texture2D image = new Texture2D(width, height, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
            image.Apply();
            File.WriteAllBytes(absolutePath, ImageConversion.EncodeToPNG(image));
            camera.targetTexture = null;
            RenderTexture.active = previous;
            UnityEngine.Object.DestroyImmediate(image);
            UnityEngine.Object.DestroyImmediate(texture);
        }

        private static void WriteMaterialSwatches(string outputRoot)
        {
            string swatchRoot = Path.Combine(outputRoot, "MaterialSwatches");
            Directory.CreateDirectory(swatchRoot);
            int size = 128;
            for (int i = 0; i < Materials.Length; i++)
            {
                Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
                Fill(texture, Materials[i].Color);
                File.WriteAllBytes(Path.Combine(swatchRoot, Materials[i].AssetName + "_swatch.png"), ImageConversion.EncodeToPNG(texture));
                UnityEngine.Object.DestroyImmediate(texture);
            }

            Texture2D sheet = new Texture2D(size * 6, size * 3, TextureFormat.RGBA32, false);
            Fill(sheet, Color.black);
            for (int i = 0; i < Materials.Length; i++)
            {
                int x0 = (i % 6) * size;
                int y0 = sheet.height - ((i / 6) + 1) * size;
                RectFill(sheet, x0 + 4, y0 + 4, size - 8, size - 8, Materials[i].Color);
            }

            sheet.Apply();
            File.WriteAllBytes(Path.Combine(outputRoot, "MEES05_v0.1.49_material_swatch_sheet.png"), ImageConversion.EncodeToPNG(sheet));
            UnityEngine.Object.DestroyImmediate(sheet);
        }

        private static int WriteUnityValidationReport()
        {
            AssetDatabase.Refresh();
            string renderRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            string reportRoot = ResolveRepoRelativeFolder(ProductionDocFolder);
            Directory.CreateDirectory(reportRoot);
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot });
            string[] materialGuids = AssetDatabase.FindAssets("t:Material", new[] { MaterialRoot });
            string[] meshGuids = AssetDatabase.FindAssets("t:Mesh", new[] { MeshRoot });
            int pngCount = Directory.Exists(renderRoot) ? Directory.GetFiles(renderRoot, "*.png", SearchOption.AllDirectories).Length : 0;
            int colliderCount = 0;
            int animatorCount = 0;
            int rigidbodyCount = 0;
            int monoCount = 0;
            for (int i = 0; i < prefabGuids.Length; i++)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(prefabGuids[i]));
                colliderCount += prefab.GetComponentsInChildren<Collider>(true).Length;
                animatorCount += prefab.GetComponentsInChildren<Animator>(true).Length;
                rigidbodyCount += prefab.GetComponentsInChildren<Rigidbody>(true).Length;
                monoCount += prefab.GetComponentsInChildren<MonoBehaviour>(true).Length;
            }

            int errors = 0;
            if (prefabGuids.Length != ExpectedPrefabs) errors++;
            if (materialGuids.Length != ExpectedMaterials) errors++;
            if (meshGuids.Length != ExpectedMeshes) errors++;
            if (pngCount < ExpectedPngMinimum) errors++;
            if (colliderCount != 0 || animatorCount != 0 || rigidbodyCount != 0 || monoCount != 0) errors++;

            string json = "{\n" +
                "  \"status\": \"" + (errors == 0 ? "pass" : "fail") + "\",\n" +
                "  \"prefabs\": " + prefabGuids.Length + ",\n" +
                "  \"materials\": " + materialGuids.Length + ",\n" +
                "  \"meshes\": " + meshGuids.Length + ",\n" +
                "  \"preview_pngs\": " + pngCount + ",\n" +
                "  \"colliders_in_prefabs\": " + colliderCount + ",\n" +
                "  \"animators_in_prefabs\": " + animatorCount + ",\n" +
                "  \"rigidbodies_in_prefabs\": " + rigidbodyCount + ",\n" +
                "  \"monobehaviours_in_prefabs\": " + monoCount + ",\n" +
                "  \"expected_prefabs\": " + ExpectedPrefabs + ",\n" +
                "  \"expected_materials\": " + ExpectedMaterials + ",\n" +
                "  \"expected_meshes\": " + ExpectedMeshes + ",\n" +
                "  \"expected_preview_pngs_minimum\": " + ExpectedPngMinimum + "\n" +
                "}\n";
            File.WriteAllText(Path.Combine(reportRoot, "unity_validation_report_v0.1.49.json"), json);
            Debug.Log("MEES05_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL"));
            return errors;
        }

        private static void WriteManifest(string status)
        {
            string diskPath = Path.Combine(PackageDiskRoot(), "Documentation~", "Manifest", "MEES05_MechanicalEnemyEliteSet05_Manifest_v0.1.49-p001.json");
            Directory.CreateDirectory(Path.GetDirectoryName(diskPath));
            string renderRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            int pngCount = Directory.Exists(renderRoot) ? Directory.GetFiles(renderRoot, "*.png", SearchOption.AllDirectories).Length : 0;
            StringBuilder b = new StringBuilder();
            b.AppendLine("{");
            b.AppendLine("  \"pack_id\": \"MEES05\",");
            b.AppendLine("  \"display_name\": \"Mechanical Enemy Elite Set 05\",");
            b.AppendLine("  \"version\": \"0.1.49\",");
            b.AppendLine("  \"build_id\": \"p001\",");
            b.AppendLine("  \"unity_version\": \"" + Application.unityVersion + "\",");
            b.AppendLine("  \"canonical_root\": \"AssetPacks/BrassworksBreach.MechanicalEnemyEliteSet05\",");
            b.AppendLine("  \"package_name\": \"" + PackageName + "\",");
            b.AppendLine("  \"package_version\": \"0.1.49-p001\",");
            b.AppendLine("  \"asset_counts\": { \"generated_prefabs\": 25, \"generated_materials\": 18, \"generated_meshes\": 12, \"preview_pngs_current\": " + pngCount + ", \"runtime_scripts\": 0, \"animation_clips\": 0, \"animator_controllers\": 0, \"colliders\": 0 },");
            AppendArray(b, "generated_prefabs", Specs.Select(s => PrefabRoot + "/" + s.Id + ".prefab"));
            b.AppendLine(",");
            AppendArray(b, "generated_materials", Materials.Select(m => MaterialRoot + "/" + m.AssetName + ".mat"));
            b.AppendLine(",");
            AppendArray(b, "visual_contract", new[] { "visual_only_no_gameplay_authority", "pose_proxy_variants_only", "no_runtime_scripts", "no_colliders", "no_animator_controllers", "no_ai_or_damage_authority" });
            b.AppendLine(",");
            b.AppendLine("  \"validation\": { \"unity_generation_status\": \"" + status + "\", \"unity_validation_report\": \"Documentation/AssetProduction/V0_1_49_MechanicalEnemyEliteSet05/unity_validation_report_v0.1.49.json\" },");
            b.AppendLine("  \"rollback_path\": \"remove package reference com.brassworks.sidecar.mechanical-enemy-elite-set05 and delete the isolated imported package root\"");
            b.AppendLine("}");
            File.WriteAllText(diskPath, b.ToString());
            AssetDatabase.ImportAsset(ManifestAssetPath);
        }

        private static void AppendArray(StringBuilder b, string name, IEnumerable<string> values)
        {
            string[] array = values.ToArray();
            b.AppendLine("  \"" + name + "\": [");
            for (int i = 0; i < array.Length; i++)
            {
                b.Append("    \"" + array[i] + "\"");
                if (i < array.Length - 1) b.Append(",");
                b.AppendLine();
            }
            b.Append("  ]");
        }

        private static void EnsureFolders()
        {
            string[] paths = { PrefabRoot, MaterialRoot, MeshRoot, PackageRoot + "/Documentation~/Manifest" };
            for (int i = 0; i < paths.Length; i++)
            {
                string relative = paths[i].Substring(PackageRoot.Length).TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
                Directory.CreateDirectory(Path.Combine(PackageDiskRoot(), relative));
            }
            AssetDatabase.Refresh();
        }

        private static string PackageDiskRoot()
        {
            UnityEditor.PackageManager.PackageInfo packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly());
            return packageInfo != null ? packageInfo.resolvedPath : Path.Combine(Directory.GetCurrentDirectory(), "AssetPacks", "BrassworksBreach.MechanicalEnemyEliteSet05");
        }

        private static string ResolveRepoRelativeFolder(string relativeFolder)
        {
            string repoRoot = Path.GetFullPath(Path.Combine(PackageDiskRoot(), "..", ".."));
            return Path.Combine(repoRoot, relativeFolder.Replace('/', Path.DirectorySeparatorChar));
        }

        private static Shader FindLitShader()
        {
            return Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("HDRP/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Sprites/Default");
        }

        private static void SetColor(Material material, string property, Color color)
        {
            if (material.HasProperty(property)) material.SetColor(property, color);
        }

        private static void SetFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property)) material.SetFloat(property, value);
        }

        private static void Fill(Texture2D texture, Color color)
        {
            RectFill(texture, 0, 0, texture.width, texture.height, color);
            texture.Apply();
        }

        private static void RectFill(Texture2D texture, int x0, int y0, int width, int height, Color color)
        {
            for (int y = y0; y < y0 + height; y++)
                for (int x = x0; x < x0 + width; x++)
                    texture.SetPixel(x, y, color);
        }

        private static Mesh SawBlade(string name, int teeth, float inner, float outer)
        {
            List<Vector3> v = new List<Vector3> { Vector3.zero };
            List<int> t = new List<int>();
            int points = teeth * 2;
            for (int i = 0; i < points; i++)
            {
                float radius = i % 2 == 0 ? outer : inner;
                float angle = i * Mathf.PI * 2f / points;
                v.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
            }
            for (int i = 1; i <= points; i++) { t.Add(0); t.Add(i); t.Add(i == points ? 1 : i + 1); }
            return BuildMesh(name, v.ToArray(), t.ToArray());
        }

        private static Mesh Cone(string name, float radius, float length, int segments)
        {
            List<Vector3> v = new List<Vector3> { new Vector3(0f, 0f, -length * 0.5f), new Vector3(0f, 0f, length * 0.5f) };
            List<int> t = new List<int>();
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                v.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, -length * 0.5f));
            }
            for (int i = 0; i < segments; i++)
            {
                int a = 2 + i;
                int b = 2 + ((i + 1) % segments);
                t.Add(1); t.Add(a); t.Add(b); t.Add(0); t.Add(b); t.Add(a);
            }
            return BuildMesh(name, v.ToArray(), t.ToArray());
        }

        private static Mesh GearRing(string name, int teeth, float inner, float outer)
        {
            List<Vector3> v = new List<Vector3>();
            List<int> t = new List<int>();
            int points = teeth * 2;
            for (int i = 0; i < points; i++)
            {
                float angle = i * Mathf.PI * 2f / points;
                float radius = i % 2 == 0 ? outer : outer * 0.84f;
                v.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
                v.Add(new Vector3(Mathf.Cos(angle) * inner, Mathf.Sin(angle) * inner, 0f));
            }
            for (int i = 0; i < points; i++)
            {
                int oa = i * 2; int ia = oa + 1; int ob = ((i + 1) % points) * 2; int ib = ob + 1;
                t.Add(oa); t.Add(ia); t.Add(ob); t.Add(ob); t.Add(ia); t.Add(ib);
            }
            return BuildMesh(name, v.ToArray(), t.ToArray());
        }

        private static Mesh OvalDisc(string name, float width, float height, int segments)
        {
            List<Vector3> v = new List<Vector3> { Vector3.zero };
            List<int> t = new List<int>();
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                v.Add(new Vector3(Mathf.Cos(angle) * width, Mathf.Sin(angle) * height, 0f));
            }
            for (int i = 1; i <= segments; i++) { t.Add(0); t.Add(i); t.Add(i == segments ? 1 : i + 1); }
            return BuildMesh(name, v.ToArray(), t.ToArray());
        }

        private static Mesh Burst(string name, int rays, float inner, float outer)
        {
            return SawBlade(name, rays, inner, outer);
        }

        private static Mesh Diamond(string name, float radius, float length)
        {
            Vector3[] v =
            {
                new Vector3(0f, 0f, -length * 0.5f), new Vector3(radius, 0f, 0f), new Vector3(0f, radius, 0f), new Vector3(-radius, 0f, 0f), new Vector3(0f, -radius, 0f), new Vector3(0f, 0f, length * 0.5f)
            };
            int[] t = { 0,2,1, 0,3,2, 0,4,3, 0,1,4, 5,1,2, 5,2,3, 5,3,4, 5,4,1 };
            return BuildMesh(name, v, t);
        }

        private static Mesh Box(string name, Vector3 size)
        {
            float x = size.x * 0.5f, y = size.y * 0.5f, z = size.z * 0.5f;
            Vector3[] v = { new Vector3(-x,-y,-z), new Vector3(x,-y,-z), new Vector3(x,y,-z), new Vector3(-x,y,-z), new Vector3(-x,-y,z), new Vector3(x,-y,z), new Vector3(x,y,z), new Vector3(-x,y,z) };
            int[] t = { 0,2,1, 0,3,2, 4,5,6, 4,6,7, 0,1,5, 0,5,4, 1,2,6, 1,6,5, 2,3,7, 2,7,6, 3,0,4, 3,4,7 };
            return BuildMesh(name, v, t);
        }

        private static Mesh Shield(string name)
        {
            Vector3[] v =
            {
                new Vector3(-0.48f,-0.68f,-0.05f), new Vector3(0.48f,-0.68f,-0.05f), new Vector3(0.58f,0.16f,-0.05f), new Vector3(0.32f,0.72f,-0.05f), new Vector3(-0.32f,0.72f,-0.05f), new Vector3(-0.58f,0.16f,-0.05f),
                new Vector3(-0.48f,-0.68f,0.05f), new Vector3(0.48f,-0.68f,0.05f), new Vector3(0.58f,0.16f,0.05f), new Vector3(0.32f,0.72f,0.05f), new Vector3(-0.32f,0.72f,0.05f), new Vector3(-0.58f,0.16f,0.05f)
            };
            int[] t =
            {
                0,1,2, 0,2,5, 5,2,3, 5,3,4, 6,8,7, 6,11,8, 11,9,8, 11,10,9,
                0,6,7, 0,7,1, 1,7,8, 1,8,2, 2,8,9, 2,9,3, 3,9,10, 3,10,4, 4,10,11, 4,11,5, 5,11,6, 5,6,0
            };
            return BuildMesh(name, v, t);
        }

        private static Mesh Crown(string name)
        {
            Vector3[] v = { new Vector3(-0.62f,-0.12f,0f), new Vector3(0.62f,-0.12f,0f), new Vector3(0.44f,0.18f,0f), new Vector3(0.28f,0.48f,0f), new Vector3(0.10f,0.14f,0f), new Vector3(0f,0.62f,0f), new Vector3(-0.10f,0.14f,0f), new Vector3(-0.28f,0.48f,0f), new Vector3(-0.44f,0.18f,0f) };
            int[] t = { 0,1,2, 0,2,8, 8,2,3, 8,3,7, 7,3,4, 7,4,6, 6,4,5 };
            return BuildMesh(name, v, t);
        }

        private static Mesh BuildMesh(string name, Vector3[] vertices, int[] triangles)
        {
            Mesh mesh = new Mesh { name = name };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static string Safe(string text)
        {
            return text.Replace(" ", "_").Replace("/", "_").Replace("\\", "_").Replace("-", "_").ToLowerInvariant();
        }

        private sealed class Spec
        {
            public Spec(string id, string family, string pose, float height, float width, float depth)
            {
                Id = id; Family = family; Pose = pose; Height = height; Width = width; Depth = depth;
            }
            public string Id { get; private set; }
            public string Family { get; private set; }
            public string Pose { get; private set; }
            public float Height { get; private set; }
            public float Width { get; private set; }
            public float Depth { get; private set; }
        }

        private sealed class MatSpec
        {
            public MatSpec(string key, string assetName, Color color, float metallic, float smoothness, Color emission, bool transparent)
            {
                Key = key; AssetName = assetName; Color = color; Metallic = metallic; Smoothness = smoothness; Emission = emission; Transparent = transparent;
            }
            public string Key { get; private set; }
            public string AssetName { get; private set; }
            public Color Color { get; private set; }
            public float Metallic { get; private set; }
            public float Smoothness { get; private set; }
            public Color Emission { get; private set; }
            public bool Transparent { get; private set; }
        }
    }
}
