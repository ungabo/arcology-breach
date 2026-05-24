using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.MechanicalEnemies.Editor
{
    public static class MechanicalEnemySidecarGenerator
    {
        private const string PackageName = "com.brassworks.sidecar.mechanical-enemies";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string PrefabRoot = PackageRoot + "/Runtime/Prefabs";
        private const string MaterialRoot = PackageRoot + "/Runtime/Materials";
        private const string MeshRoot = PackageRoot + "/Runtime/Meshes";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_37_MechanicalEnemiesSidecar";

        private static readonly Color AgedBrass = new Color(0.72f, 0.47f, 0.22f, 1f);
        private static readonly Color BlackenedIron = new Color(0.08f, 0.075f, 0.065f, 1f);
        private static readonly Color OilyLeather = new Color(0.22f, 0.12f, 0.065f, 1f);
        private static readonly Color GrimyGlass = new Color(0.28f, 0.38f, 0.32f, 0.72f);
        private static readonly Color AmberGlow = new Color(1.0f, 0.52f, 0.11f, 1f);
        private static readonly Color CyanCharge = new Color(0.12f, 0.75f, 1.0f, 1f);
        private static readonly Color Soot = new Color(0.025f, 0.022f, 0.019f, 1f);
        private static readonly Color HazardPaint = new Color(0.92f, 0.65f, 0.14f, 1f);
        private static readonly Color HitboxMarker = new Color(0.2f, 0.9f, 0.75f, 0.22f);

        private static readonly List<string> GeneratedPrefabPaths = new List<string>
        {
            PrefabRoot + "/SCENM_SawScrapper.prefab",
            PrefabRoot + "/SCENM_RivetLancer.prefab",
            PrefabRoot + "/SCENM_BulwarkFurnace.prefab",
            PrefabRoot + "/SCENM_WardenSentinel.prefab",
            PrefabRoot + "/SCENM_FoundryOverseerBust.prefab"
        };

        [MenuItem("Brassworks Breach/Sidecars/Mechanical Enemies/Generate v0.1.37 Enemy Package")]
        public static void GenerateEnemyPackage()
        {
            EnsurePackageFolders();

            var materials = CreateMaterials();
            var meshes = CreateMeshes();

            SavePrefab(BuildSawScrapper(materials, meshes), GeneratedPrefabPaths[0]);
            SavePrefab(BuildRivetLancer(materials, meshes), GeneratedPrefabPaths[1]);
            SavePrefab(BuildBulwarkFurnace(materials, meshes), GeneratedPrefabPaths[2]);
            SavePrefab(BuildWardenSentinel(materials, meshes), GeneratedPrefabPaths[3]);
            SavePrefab(BuildFoundryOverseerBust(materials, meshes), GeneratedPrefabPaths[4]);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("SCENM v0.1.37 mechanical enemy package generated: 5 prefabs, 10 materials, 7 procedural mesh assets.");
        }

        [MenuItem("Brassworks Breach/Sidecars/Mechanical Enemies/Render v0.1.37 Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            GenerateEnemyPackage();

            var outputRoot = ResolveRenderOutputRoot();
            Directory.CreateDirectory(outputRoot);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreatePreviewEnvironment();
            var lineup = InstantiateLineup();
            var camera = CreatePreviewCamera(new Vector3(0f, 2.05f, -7.8f), new Vector3(0f, 1.25f, 0.1f), 52f);

            RenderCameraToPng(camera, Path.Combine(outputRoot, "SCENM_v0.1.37_lineup.png"), 1920, 1080);

            foreach (var instance in lineup)
            {
                FrameSingleEnemy(camera, instance.transform);
                RenderCameraToPng(camera, Path.Combine(outputRoot, instance.name + "_v0.1.37.png"), 1400, 1400);
            }

            EditorSceneManager.MarkSceneDirty(scene);
            Debug.Log("SCENM v0.1.37 preview PNGs rendered to: " + outputRoot);
        }

        public static void GenerateAllAndRenderPreview()
        {
            RenderPreviewPngs();
        }

        private static Dictionary<string, Material> CreateMaterials()
        {
            var materials = new Dictionary<string, Material>
            {
                ["Brass"] = UpsertMaterial("SCENM_MAT_AgedBrass", AgedBrass, 0.88f, 0.42f, Color.black, false),
                ["Iron"] = UpsertMaterial("SCENM_MAT_BlackenedIron", BlackenedIron, 0.9f, 0.24f, Color.black, false),
                ["Leather"] = UpsertMaterial("SCENM_MAT_OilyLeather", OilyLeather, 0.02f, 0.55f, Color.black, false),
                ["Glass"] = UpsertMaterial("SCENM_MAT_GrimyGlass", GrimyGlass, 0f, 0.75f, Color.black, true),
                ["Amber"] = UpsertMaterial("SCENM_MAT_AmberFurnaceGlow", AmberGlow, 0f, 0.35f, AmberGlow * 3.2f, false),
                ["Cyan"] = UpsertMaterial("SCENM_MAT_CyanChargeTell", CyanCharge, 0f, 0.35f, CyanCharge * 2.8f, false),
                ["Soot"] = UpsertMaterial("SCENM_MAT_SootAndOil", Soot, 0.35f, 0.18f, Color.black, false),
                ["Hazard"] = UpsertMaterial("SCENM_MAT_HazardPaint", HazardPaint, 0.18f, 0.32f, Color.black, false),
                ["Hitbox"] = UpsertMaterial("SCENM_MAT_HitboxMarker", HitboxMarker, 0f, 0.1f, HitboxMarker, true),
                ["Rivet"] = UpsertMaterial("SCENM_MAT_PolishedRivetWear", new Color(0.95f, 0.75f, 0.38f, 1f), 1f, 0.5f, Color.black, false)
            };

            return materials;
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            return new Dictionary<string, Mesh>
            {
                ["SawBlade"] = UpsertMesh("SCENM_MESH_24ToothSawBlade", CreateSawBladeMesh("SCENM_MESH_24ToothSawBlade", 24, 0.28f, 0.38f)),
                ["LanceTip"] = UpsertMesh("SCENM_MESH_RivetLanceTip", CreateConeMesh("SCENM_MESH_RivetLanceTip", 0.22f, 0.72f, 18)),
                ["ClawHook"] = UpsertMesh("SCENM_MESH_CurvedClawHook", CreateClawHookMesh("SCENM_MESH_CurvedClawHook")),
                ["ShieldPlate"] = UpsertMesh("SCENM_MESH_BulwarkShieldPlate", CreateShieldPlateMesh("SCENM_MESH_BulwarkShieldPlate")),
                ["GearRing"] = UpsertMesh("SCENM_MESH_GearRing", CreateGearRingMesh("SCENM_MESH_GearRing", 18, 0.24f, 0.36f)),
                ["GaugeFace"] = UpsertMesh("SCENM_MESH_PressureGaugeFace", CreateGaugeFaceMesh("SCENM_MESH_PressureGaugeFace")),
                ["ShoulderFin"] = UpsertMesh("SCENM_MESH_OverseerShoulderFin", CreateShoulderFinMesh("SCENM_MESH_OverseerShoulderFin"))
            };
        }

        private static GameObject BuildSawScrapper(Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var prefab = CreateBaseEnemy("SCENM_SawScrapper", "ScaleNotes_1p55m_low_fast_profile_2p4m_corridor_safe");
            var root = prefab.transform.Find("Root");
            var hips = root.Find("Hips");
            var body = hips.Find("Body");
            var head = root.Find("Head");
            var leftArm = root.Find("LeftArm");
            var rightArm = root.Find("RightArm");
            var leftLeg = root.Find("LeftLeg");
            var rightLeg = root.Find("RightLeg");
            var mounts = root.Find("WeaponMounts");
            var vfx = root.Find("VFXAnchors");

            CreatePrimitivePart("Boiler_Body_Squat", PrimitiveType.Cylinder, body, new Vector3(0f, 0.78f, 0f), new Vector3(0.62f, 0.48f, 0.62f), mat["Brass"]);
            CreatePrimitivePart("BlackIron_BellyBand", PrimitiveType.Cylinder, body, new Vector3(0f, 0.79f, 0f), new Vector3(0.66f, 0.055f, 0.66f), mat["Iron"]);
            CreatePrimitivePart("Low_Furnace_Head", PrimitiveType.Sphere, head, new Vector3(0f, 1.17f, 0.06f), new Vector3(0.52f, 0.34f, 0.44f), mat["Iron"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Left", new Vector3(-0.16f, 1.19f, -0.30f), mat["Amber"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Right", new Vector3(0.16f, 1.19f, -0.30f), mat["Amber"]);
            CreatePrimitivePart("Jaw_Grate", PrimitiveType.Cube, head, new Vector3(0f, 1.05f, -0.33f), new Vector3(0.32f, 0.16f, 0.05f), mat["Soot"]);

            BuildPistonLimb(leftArm, "LeftArm", new Vector3(-0.52f, 0.92f, -0.02f), -28f, mat);
            BuildPistonLimb(rightArm, "RightArm", new Vector3(0.52f, 0.92f, -0.02f), 28f, mat);
            BuildPistonLeg(leftLeg, "LeftLeg", new Vector3(-0.23f, 0.36f, 0.03f), mat);
            BuildPistonLeg(rightLeg, "RightLeg", new Vector3(0.23f, 0.36f, 0.03f), mat);

            CreateMeshPart("Left_SawBlade_24Tooth", mesh["SawBlade"], leftArm, new Vector3(-0.88f, 0.63f, -0.06f), new Vector3(1f, 1f, 1f), Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
            CreatePrimitivePart("Left_SawHub", PrimitiveType.Cylinder, leftArm, new Vector3(-0.88f, 0.63f, -0.06f), new Vector3(0.13f, 0.08f, 0.13f), mat["Rivet"], Quaternion.Euler(0f, 0f, 90f));
            CreateMeshPart("Right_ClawHook", mesh["ClawHook"], rightArm, new Vector3(0.88f, 0.62f, -0.08f), new Vector3(1.05f, 1.05f, 1.05f), Quaternion.Euler(0f, -90f, 0f), mat["Brass"]);
            CreateSteamPipeArc(body, "Back_SteamPipe_Left", new Vector3(-0.32f, 1.06f, 0.27f), mat["Iron"]);
            CreateSteamPipeArc(body, "Back_SteamPipe_Right", new Vector3(0.32f, 1.06f, 0.27f), mat["Iron"]);

            AddRivetBelt(body, 10, 0.37f, 0.8f, mat["Rivet"]);
            CreateSocket(mounts, "SOCK_LeftSawAxis", new Vector3(-0.88f, 0.63f, -0.06f));
            CreateSocket(mounts, "SOCK_RightClawPalm", new Vector3(0.88f, 0.62f, -0.08f));
            CreateSocket(vfx, "SOCK_SteamVent_Back", new Vector3(0f, 1.3f, 0.28f));
            CreateSocket(vfx, "SOCK_ShutdownBurst_Core", new Vector3(0f, 0.82f, 0f));
            AddHitboxSet(root, "SawScrapper", new Vector3(0f, 0.78f, 0f), new Vector3(0.86f, 1.15f, 0.78f), new Vector3(0f, 1.18f, -0.28f), mat["Hitbox"]);
            return prefab;
        }

        private static GameObject BuildRivetLancer(Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var prefab = CreateBaseEnemy("SCENM_RivetLancer", "ScaleNotes_1p90m_tall_narrow_lance_profile_2p4m_corridor_safe");
            var root = prefab.transform.Find("Root");
            var hips = root.Find("Hips");
            var body = hips.Find("Body");
            var head = root.Find("Head");
            var leftArm = root.Find("LeftArm");
            var rightArm = root.Find("RightArm");
            var leftLeg = root.Find("LeftLeg");
            var rightLeg = root.Find("RightLeg");
            var mounts = root.Find("WeaponMounts");
            var vfx = root.Find("VFXAnchors");

            CreatePrimitivePart("Tall_Boiler_Spine", PrimitiveType.Cylinder, body, new Vector3(0f, 1.0f, 0f), new Vector3(0.42f, 0.78f, 0.42f), mat["Brass"]);
            CreatePrimitivePart("BlackIron_Corset_Frame", PrimitiveType.Cube, body, new Vector3(0f, 0.98f, -0.04f), new Vector3(0.58f, 0.92f, 0.12f), mat["Iron"]);
            CreatePrimitivePart("Helmet_LongVisor", PrimitiveType.Sphere, head, new Vector3(0f, 1.73f, -0.02f), new Vector3(0.38f, 0.30f, 0.42f), mat["Iron"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Center", new Vector3(0f, 1.74f, -0.33f), mat["Amber"]);
            CreatePrimitivePart("Back_Cyan_Coil_A", PrimitiveType.Cylinder, body, new Vector3(-0.2f, 1.26f, 0.30f), new Vector3(0.11f, 0.48f, 0.11f), mat["Cyan"]);
            CreatePrimitivePart("Back_Cyan_Coil_B", PrimitiveType.Cylinder, body, new Vector3(0.2f, 1.26f, 0.30f), new Vector3(0.11f, 0.48f, 0.11f), mat["Cyan"]);

            BuildPistonLimb(leftArm, "LeftArm", new Vector3(-0.45f, 1.14f, -0.02f), -18f, mat);
            BuildPistonLimb(rightArm, "RightArm", new Vector3(0.45f, 1.14f, -0.02f), 18f, mat);
            BuildPistonLeg(leftLeg, "LeftLeg", new Vector3(-0.18f, 0.43f, 0.04f), mat);
            BuildPistonLeg(rightLeg, "RightLeg", new Vector3(0.18f, 0.43f, 0.04f), mat);

            CreatePrimitivePart("Forward_Lance_Shaft", PrimitiveType.Cylinder, mounts, new Vector3(0f, 1.02f, -0.63f), new Vector3(0.055f, 0.54f, 0.055f), mat["Iron"], Quaternion.Euler(90f, 0f, 0f));
            CreateMeshPart("Forward_Lance_Tip", mesh["LanceTip"], mounts, new Vector3(0f, 1.02f, -1.16f), new Vector3(1f, 1f, 1f), Quaternion.identity, mat["Rivet"]);
            CreatePrimitivePart("Lance_Cyan_ChargeBand", PrimitiveType.Cylinder, mounts, new Vector3(0f, 1.02f, -0.76f), new Vector3(0.085f, 0.08f, 0.085f), mat["Cyan"], Quaternion.Euler(90f, 0f, 0f));
            AddRivetBelt(body, 12, 0.30f, 1.02f, mat["Rivet"]);

            CreateSocket(mounts, "SOCK_LanceMuzzle", new Vector3(0f, 1.02f, -1.55f));
            CreateSocket(mounts, "SOCK_LanceGrip", new Vector3(0f, 1.02f, -0.42f));
            CreateSocket(vfx, "SOCK_CyanCharge_Backpack", new Vector3(0f, 1.47f, 0.38f));
            CreateSocket(vfx, "SOCK_ShutdownBurst_Core", new Vector3(0f, 1.0f, 0f));
            AddHitboxSet(root, "RivetLancer", new Vector3(0f, 1.0f, 0f), new Vector3(0.72f, 1.6f, 0.66f), new Vector3(0f, 1.74f, -0.33f), mat["Hitbox"]);
            AddToolHitbox(root, "HB_Lance_Forward_TriggerGuidance", new Vector3(0f, 1.02f, -0.9f), new Vector3(0.18f, 0.18f, 1.28f), mat["Hitbox"]);
            return prefab;
        }

        private static GameObject BuildBulwarkFurnace(Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var prefab = CreateBaseEnemy("SCENM_BulwarkFurnace", "ScaleNotes_2p15m_wide_shield_wall_profile_requires_1p8m_clearance");
            var root = prefab.transform.Find("Root");
            var hips = root.Find("Hips");
            var body = hips.Find("Body");
            var head = root.Find("Head");
            var leftArm = root.Find("LeftArm");
            var rightArm = root.Find("RightArm");
            var leftLeg = root.Find("LeftLeg");
            var rightLeg = root.Find("RightLeg");
            var mounts = root.Find("WeaponMounts");
            var vfx = root.Find("VFXAnchors");

            CreatePrimitivePart("Wide_Furnace_Boiler", PrimitiveType.Cylinder, body, new Vector3(0f, 1.07f, 0f), new Vector3(0.74f, 0.82f, 0.74f), mat["Brass"]);
            CreatePrimitivePart("Ribbed_Iron_Cage", PrimitiveType.Cube, body, new Vector3(0f, 1.04f, -0.05f), new Vector3(1.02f, 1.05f, 0.14f), mat["Iron"]);
            CreateMeshPart("Chest_Pressure_Gauge", mesh["GaugeFace"], body, new Vector3(0f, 1.2f, -0.43f), new Vector3(1.15f, 1.15f, 1.15f), Quaternion.Euler(0f, 180f, 0f), mat["Glass"]);
            CreatePrimitivePart("GuardBreak_Amber_Lamp", PrimitiveType.Sphere, body, new Vector3(0f, 1.2f, -0.47f), new Vector3(0.13f, 0.13f, 0.13f), mat["Amber"]);
            CreatePrimitivePart("Heavy_Domed_Head", PrimitiveType.Sphere, head, new Vector3(0f, 1.88f, -0.03f), new Vector3(0.46f, 0.34f, 0.42f), mat["Iron"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Left", new Vector3(-0.14f, 1.88f, -0.33f), mat["Amber"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Right", new Vector3(0.14f, 1.88f, -0.33f), mat["Amber"]);

            BuildPistonLimb(leftArm, "LeftArm", new Vector3(-0.72f, 1.12f, -0.02f), -12f, mat);
            BuildPistonLimb(rightArm, "RightArm", new Vector3(0.72f, 1.12f, -0.02f), 12f, mat);
            BuildHeavyLeg(leftLeg, "LeftLeg", new Vector3(-0.33f, 0.45f, 0.04f), mat);
            BuildHeavyLeg(rightLeg, "RightLeg", new Vector3(0.33f, 0.45f, 0.04f), mat);

            CreateMeshPart("Left_Shield_Plate", mesh["ShieldPlate"], mounts, new Vector3(-0.95f, 1.05f, -0.28f), new Vector3(1.18f, 1.18f, 1.18f), Quaternion.Euler(0f, 10f, 0f), mat["Iron"]);
            CreatePrimitivePart("Shield_Hazard_Trim", PrimitiveType.Cube, mounts, new Vector3(-0.95f, 1.05f, -0.55f), new Vector3(0.76f, 1.2f, 0.035f), mat["Hazard"]);
            CreatePrimitivePart("Right_SteamHammer_Head", PrimitiveType.Cube, mounts, new Vector3(0.96f, 0.68f, -0.2f), new Vector3(0.34f, 0.30f, 0.46f), mat["Brass"]);
            CreatePrimitivePart("Right_SteamHammer_Handle", PrimitiveType.Cylinder, mounts, new Vector3(0.78f, 0.88f, -0.09f), new Vector3(0.055f, 0.45f, 0.055f), mat["Iron"], Quaternion.Euler(18f, 0f, 16f));
            AddRivetBelt(body, 14, 0.48f, 1.04f, mat["Rivet"]);

            CreateSocket(mounts, "SOCK_ShieldCenter", new Vector3(-0.95f, 1.05f, -0.55f));
            CreateSocket(mounts, "SOCK_HammerImpact", new Vector3(0.98f, 0.56f, -0.25f));
            CreateSocket(vfx, "SOCK_FurnaceVent_Left", new Vector3(-0.42f, 1.58f, 0.34f));
            CreateSocket(vfx, "SOCK_FurnaceVent_Right", new Vector3(0.42f, 1.58f, 0.34f));
            CreateSocket(vfx, "SOCK_ShutdownBurst_Core", new Vector3(0f, 1.1f, 0f));
            AddHitboxSet(root, "BulwarkFurnace", new Vector3(0f, 1.1f, 0f), new Vector3(1.24f, 1.78f, 0.86f), new Vector3(0f, 1.2f, -0.48f), mat["Hitbox"]);
            AddToolHitbox(root, "HB_Shield_Block_Guidance", new Vector3(-0.94f, 1.05f, -0.52f), new Vector3(0.88f, 1.42f, 0.18f), mat["Hitbox"]);
            return prefab;
        }

        private static GameObject BuildWardenSentinel(Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var prefab = CreateBaseEnemy("SCENM_WardenSentinel", "ScaleNotes_2p35m_command_tower_profile_boss_doorway_safe");
            var root = prefab.transform.Find("Root");
            var hips = root.Find("Hips");
            var body = hips.Find("Body");
            var head = root.Find("Head");
            var leftArm = root.Find("LeftArm");
            var rightArm = root.Find("RightArm");
            var leftLeg = root.Find("LeftLeg");
            var rightLeg = root.Find("RightLeg");
            var mounts = root.Find("WeaponMounts");
            var vfx = root.Find("VFXAnchors");

            CreatePrimitivePart("Tall_Command_Boiler", PrimitiveType.Cylinder, body, new Vector3(0f, 1.18f, 0f), new Vector3(0.58f, 1.0f, 0.58f), mat["Brass"]);
            CreateMeshPart("Chest_GearRing_Command", mesh["GearRing"], body, new Vector3(0f, 1.28f, -0.43f), new Vector3(1.15f, 1.15f, 1.15f), Quaternion.Euler(0f, 180f, 0f), mat["Rivet"]);
            CreatePrimitivePart("Command_Furnace_Lamp", PrimitiveType.Sphere, body, new Vector3(0f, 1.28f, -0.49f), new Vector3(0.14f, 0.14f, 0.14f), mat["Amber"]);
            CreatePrimitivePart("Sentinel_Head_Cowl", PrimitiveType.Sphere, head, new Vector3(0f, 2.11f, -0.02f), new Vector3(0.48f, 0.34f, 0.44f), mat["Iron"]);
            CreatePrimitivePart("Crown_PressureGauge", PrimitiveType.Cylinder, head, new Vector3(0f, 2.34f, -0.02f), new Vector3(0.18f, 0.08f, 0.18f), mat["Glass"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Left", new Vector3(-0.16f, 2.11f, -0.34f), mat["Amber"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Right", new Vector3(0.16f, 2.11f, -0.34f), mat["Amber"]);

            BuildPistonLimb(leftArm, "LeftArm", new Vector3(-0.64f, 1.32f, -0.02f), -16f, mat);
            BuildPistonLimb(rightArm, "RightArm", new Vector3(0.64f, 1.32f, -0.02f), 16f, mat);
            BuildHeavyLeg(leftLeg, "LeftLeg", new Vector3(-0.26f, 0.48f, 0.04f), mat);
            BuildHeavyLeg(rightLeg, "RightLeg", new Vector3(0.26f, 0.48f, 0.04f), mat);

            CreatePrimitivePart("Left_Pincer_Base", PrimitiveType.Cylinder, mounts, new Vector3(-0.88f, 1.06f, -0.12f), new Vector3(0.09f, 0.18f, 0.09f), mat["Iron"], Quaternion.Euler(0f, 0f, 90f));
            CreateMeshPart("Left_Pincer_Claw_A", mesh["ClawHook"], mounts, new Vector3(-1.06f, 1.1f, -0.16f), new Vector3(0.75f, 0.75f, 0.75f), Quaternion.Euler(0f, 90f, 25f), mat["Brass"]);
            CreateMeshPart("Left_Pincer_Claw_B", mesh["ClawHook"], mounts, new Vector3(-1.06f, 1.0f, -0.16f), new Vector3(0.75f, 0.75f, 0.75f), Quaternion.Euler(0f, 90f, -25f), mat["Brass"]);
            CreatePrimitivePart("Right_Gavel_Head", PrimitiveType.Cube, mounts, new Vector3(0.96f, 0.95f, -0.24f), new Vector3(0.42f, 0.34f, 0.50f), mat["Brass"]);
            CreatePrimitivePart("Right_Gavel_Handle", PrimitiveType.Cylinder, mounts, new Vector3(0.75f, 1.12f, -0.12f), new Vector3(0.055f, 0.48f, 0.055f), mat["Iron"], Quaternion.Euler(18f, 0f, 22f));
            CreatePrimitivePart("Back_TwinChargeCoil_Left", PrimitiveType.Cylinder, body, new Vector3(-0.32f, 1.63f, 0.34f), new Vector3(0.12f, 0.55f, 0.12f), mat["Cyan"]);
            CreatePrimitivePart("Back_TwinChargeCoil_Right", PrimitiveType.Cylinder, body, new Vector3(0.32f, 1.63f, 0.34f), new Vector3(0.12f, 0.55f, 0.12f), mat["Cyan"]);
            AddRivetBelt(body, 16, 0.39f, 1.18f, mat["Rivet"]);

            CreateSocket(mounts, "SOCK_PincerGrabCenter", new Vector3(-1.08f, 1.05f, -0.2f));
            CreateSocket(mounts, "SOCK_GavelImpact", new Vector3(0.98f, 0.82f, -0.3f));
            CreateSocket(vfx, "SOCK_CommandPulse_CenterLamp", new Vector3(0f, 1.28f, -0.52f));
            CreateSocket(vfx, "SOCK_TwinChargeCoils", new Vector3(0f, 1.72f, 0.38f));
            AddHitboxSet(root, "WardenSentinel", new Vector3(0f, 1.2f, 0f), new Vector3(1.0f, 2.05f, 0.78f), new Vector3(0f, 1.28f, -0.5f), mat["Hitbox"]);
            return prefab;
        }

        private static GameObject BuildFoundryOverseerBust(Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var prefab = CreateBaseEnemy("SCENM_FoundryOverseerBust", "ScaleNotes_2p75m_miniboss_bust_blockout_not_corridor_roaming");
            var root = prefab.transform.Find("Root");
            var hips = root.Find("Hips");
            var body = hips.Find("Body");
            var head = root.Find("Head");
            var leftArm = root.Find("LeftArm");
            var rightArm = root.Find("RightArm");
            var leftLeg = root.Find("LeftLeg");
            var rightLeg = root.Find("RightLeg");
            var mounts = root.Find("WeaponMounts");
            var vfx = root.Find("VFXAnchors");

            CreatePrimitivePart("Pedestal_Foundry_Base", PrimitiveType.Cylinder, hips, new Vector3(0f, 0.42f, 0f), new Vector3(0.92f, 0.42f, 0.92f), mat["Iron"]);
            CreatePrimitivePart("Overseer_Furnace_Torso", PrimitiveType.Cylinder, body, new Vector3(0f, 1.42f, 0f), new Vector3(0.84f, 0.96f, 0.84f), mat["Brass"]);
            CreatePrimitivePart("Boss_Apron_IronPlate", PrimitiveType.Cube, body, new Vector3(0f, 1.36f, -0.5f), new Vector3(0.82f, 1.1f, 0.12f), mat["Iron"]);
            CreatePrimitivePart("Boss_Furnace_WeakLamp", PrimitiveType.Sphere, body, new Vector3(0f, 1.55f, -0.6f), new Vector3(0.2f, 0.2f, 0.2f), mat["Amber"]);
            CreateMeshPart("Boss_GearHalo", mesh["GearRing"], head, new Vector3(0f, 2.38f, 0.08f), new Vector3(1.45f, 1.45f, 1.45f), Quaternion.Euler(0f, 180f, 0f), mat["Rivet"]);
            CreatePrimitivePart("Overseer_Head_Blockout", PrimitiveType.Sphere, head, new Vector3(0f, 2.18f, -0.06f), new Vector3(0.62f, 0.42f, 0.54f), mat["Iron"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Left", new Vector3(-0.22f, 2.19f, -0.46f), mat["Amber"]);
            CreateEyeSocket(head, "SOCK_EyeAmber_Right", new Vector3(0.22f, 2.19f, -0.46f), mat["Amber"]);
            CreateMeshPart("Left_ShoulderFin", mesh["ShoulderFin"], body, new Vector3(-0.72f, 1.98f, -0.02f), new Vector3(1.05f, 1.05f, 1.05f), Quaternion.Euler(0f, 0f, 12f), mat["Hazard"]);
            CreateMeshPart("Right_ShoulderFin", mesh["ShoulderFin"], body, new Vector3(0.72f, 1.98f, -0.02f), new Vector3(-1.05f, 1.05f, 1.05f), Quaternion.Euler(0f, 0f, -12f), mat["Hazard"]);

            BuildHeavyArm(leftArm, "LeftArm", new Vector3(-0.86f, 1.45f, -0.02f), -12f, mat);
            BuildHeavyArm(rightArm, "RightArm", new Vector3(0.86f, 1.45f, -0.02f), 12f, mat);
            CreatePrimitivePart("LeftLeg_BlockoutStrut", PrimitiveType.Cylinder, leftLeg, new Vector3(-0.34f, 0.72f, 0.04f), new Vector3(0.10f, 0.54f, 0.10f), mat["Soot"]);
            CreatePrimitivePart("RightLeg_BlockoutStrut", PrimitiveType.Cylinder, rightLeg, new Vector3(0.34f, 0.72f, 0.04f), new Vector3(0.10f, 0.54f, 0.10f), mat["Soot"]);

            CreateMeshPart("Left_BossSaw", mesh["SawBlade"], mounts, new Vector3(-1.28f, 1.18f, -0.22f), new Vector3(1.2f, 1.2f, 1.2f), Quaternion.Euler(0f, 90f, 0f), mat["Hazard"]);
            CreatePrimitivePart("Right_BossHammer", PrimitiveType.Cube, mounts, new Vector3(1.24f, 1.06f, -0.24f), new Vector3(0.48f, 0.36f, 0.56f), mat["Brass"]);
            CreatePrimitivePart("Back_BossLanceShaft", PrimitiveType.Cylinder, mounts, new Vector3(0f, 1.78f, 0.58f), new Vector3(0.06f, 0.64f, 0.06f), mat["Iron"], Quaternion.Euler(42f, 0f, 0f));
            CreateMeshPart("Back_BossLanceTip", mesh["LanceTip"], mounts, new Vector3(0f, 2.2f, 0.12f), new Vector3(1.1f, 1.1f, 1.1f), Quaternion.Euler(42f, 0f, 0f), mat["Rivet"]);
            AddRivetBelt(body, 20, 0.55f, 1.42f, mat["Rivet"]);

            CreateSocket(mounts, "SOCK_BossSawAxis", new Vector3(-1.28f, 1.18f, -0.22f));
            CreateSocket(mounts, "SOCK_BossHammerImpact", new Vector3(1.26f, 0.9f, -0.3f));
            CreateSocket(mounts, "SOCK_BackLanceMuzzle", new Vector3(0f, 2.32f, -0.02f));
            CreateSocket(vfx, "SOCK_BossFurnaceVent", new Vector3(0f, 1.9f, 0.48f));
            CreateSocket(vfx, "SOCK_BossShutdownBurst_Core", new Vector3(0f, 1.45f, 0f));
            AddHitboxSet(root, "FoundryOverseerBust", new Vector3(0f, 1.48f, 0f), new Vector3(1.42f, 2.28f, 1.02f), new Vector3(0f, 1.55f, -0.62f), mat["Hitbox"]);
            return prefab;
        }

        private static GameObject CreateBaseEnemy(string prefabName, string scaleNoteName)
        {
            var prefab = new GameObject(prefabName);
            var root = CreateEmpty("Root", prefab.transform);
            var hips = CreateEmpty("Hips", root.transform);
            CreateEmpty("Body", hips.transform);
            CreateEmpty("Head", root.transform);
            CreateEmpty("LeftArm", root.transform);
            CreateEmpty("RightArm", root.transform);
            CreateEmpty("LeftLeg", root.transform);
            CreateEmpty("RightLeg", root.transform);
            CreateEmpty("WeaponMounts", root.transform);
            CreateEmpty("VFXAnchors", root.transform);
            CreateEmpty("Hitboxes", root.transform);
            CreateEmpty(scaleNoteName, root.transform);
            return prefab;
        }

        private static void BuildPistonLimb(Transform parent, string prefix, Vector3 shoulder, float angleZ, Dictionary<string, Material> mat)
        {
            CreatePrimitivePart(prefix + "_UpperPiston", PrimitiveType.Cylinder, parent, shoulder, new Vector3(0.07f, 0.34f, 0.07f), mat["Iron"], Quaternion.Euler(0f, 0f, angleZ));
            CreatePrimitivePart(prefix + "_BrassElbowJoint", PrimitiveType.Sphere, parent, shoulder + new Vector3(Mathf.Sign(shoulder.x) * 0.12f, -0.28f, -0.02f), new Vector3(0.16f, 0.16f, 0.16f), mat["Brass"]);
            CreatePrimitivePart(prefix + "_ForearmPiston", PrimitiveType.Cylinder, parent, shoulder + new Vector3(Mathf.Sign(shoulder.x) * 0.22f, -0.48f, -0.03f), new Vector3(0.055f, 0.32f, 0.055f), mat["Iron"], Quaternion.Euler(0f, 0f, angleZ * 0.6f));
            CreatePrimitivePart(prefix + "_LeatherHose", PrimitiveType.Cylinder, parent, shoulder + new Vector3(Mathf.Sign(shoulder.x) * 0.08f, -0.12f, 0.12f), new Vector3(0.035f, 0.34f, 0.035f), mat["Leather"], Quaternion.Euler(25f, 0f, angleZ));
        }

        private static void BuildHeavyArm(Transform parent, string prefix, Vector3 shoulder, float angleZ, Dictionary<string, Material> mat)
        {
            CreatePrimitivePart(prefix + "_HeavyUpperArm", PrimitiveType.Cylinder, parent, shoulder, new Vector3(0.12f, 0.44f, 0.12f), mat["Iron"], Quaternion.Euler(0f, 0f, angleZ));
            CreatePrimitivePart(prefix + "_HeavyElbowBoiler", PrimitiveType.Sphere, parent, shoulder + new Vector3(Mathf.Sign(shoulder.x) * 0.18f, -0.36f, -0.02f), new Vector3(0.24f, 0.24f, 0.24f), mat["Brass"]);
            CreatePrimitivePart(prefix + "_HeavyForearm", PrimitiveType.Cylinder, parent, shoulder + new Vector3(Mathf.Sign(shoulder.x) * 0.32f, -0.62f, -0.04f), new Vector3(0.1f, 0.4f, 0.1f), mat["Iron"], Quaternion.Euler(0f, 0f, angleZ * 0.5f));
        }

        private static void BuildPistonLeg(Transform parent, string prefix, Vector3 hip, Dictionary<string, Material> mat)
        {
            CreatePrimitivePart(prefix + "_UpperLegPiston", PrimitiveType.Cylinder, parent, hip, new Vector3(0.075f, 0.36f, 0.075f), mat["Iron"]);
            CreatePrimitivePart(prefix + "_KneeBoilerJoint", PrimitiveType.Sphere, parent, hip + new Vector3(0f, -0.32f, -0.02f), new Vector3(0.16f, 0.16f, 0.16f), mat["Brass"]);
            CreatePrimitivePart(prefix + "_LowerLegPiston", PrimitiveType.Cylinder, parent, hip + new Vector3(0f, -0.58f, -0.02f), new Vector3(0.065f, 0.34f, 0.065f), mat["Iron"]);
            CreatePrimitivePart(prefix + "_IronBoot", PrimitiveType.Cube, parent, hip + new Vector3(0f, -0.88f, -0.09f), new Vector3(0.26f, 0.12f, 0.42f), mat["Soot"]);
        }

        private static void BuildHeavyLeg(Transform parent, string prefix, Vector3 hip, Dictionary<string, Material> mat)
        {
            CreatePrimitivePart(prefix + "_HeavyUpperLeg", PrimitiveType.Cylinder, parent, hip, new Vector3(0.12f, 0.42f, 0.12f), mat["Iron"]);
            CreatePrimitivePart(prefix + "_HeavyKneeBoiler", PrimitiveType.Sphere, parent, hip + new Vector3(0f, -0.38f, -0.02f), new Vector3(0.22f, 0.22f, 0.22f), mat["Brass"]);
            CreatePrimitivePart(prefix + "_HeavyLowerLeg", PrimitiveType.Cylinder, parent, hip + new Vector3(0f, -0.7f, -0.02f), new Vector3(0.11f, 0.42f, 0.11f), mat["Iron"]);
            CreatePrimitivePart(prefix + "_WideIronBoot", PrimitiveType.Cube, parent, hip + new Vector3(0f, -1.04f, -0.12f), new Vector3(0.36f, 0.16f, 0.52f), mat["Soot"]);
        }

        private static void AddRivetBelt(Transform parent, int count, float radius, float y, Material material)
        {
            for (var i = 0; i < count; i++)
            {
                var angle = i * Mathf.PI * 2f / count;
                var x = Mathf.Sin(angle) * radius;
                var z = Mathf.Cos(angle) * radius;
                CreatePrimitivePart("Rivet_" + i.ToString("00"), PrimitiveType.Sphere, parent, new Vector3(x, y, z), new Vector3(0.055f, 0.055f, 0.055f), material);
            }
        }

        private static void CreateSteamPipeArc(Transform parent, string name, Vector3 position, Material material)
        {
            CreatePrimitivePart(name + "_Vertical", PrimitiveType.Cylinder, parent, position, new Vector3(0.045f, 0.32f, 0.045f), material);
            CreatePrimitivePart(name + "_Cap", PrimitiveType.Sphere, parent, position + new Vector3(0f, 0.32f, 0f), new Vector3(0.09f, 0.09f, 0.09f), material);
            CreatePrimitivePart(name + "_Vent", PrimitiveType.Cylinder, parent, position + new Vector3(0f, 0.42f, -0.09f), new Vector3(0.04f, 0.16f, 0.04f), material, Quaternion.Euler(90f, 0f, 0f));
        }

        private static void CreateEyeSocket(Transform parent, string name, Vector3 position, Material material)
        {
            var eye = CreatePrimitivePart(name, PrimitiveType.Sphere, parent, position, new Vector3(0.095f, 0.095f, 0.095f), material);
            var light = eye.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = AmberGlow;
            light.intensity = 2.2f;
            light.range = 1.4f;
        }

        private static void AddHitboxSet(Transform root, string prefix, Vector3 bodyCenter, Vector3 bodySize, Vector3 weakpointCenter, Material material)
        {
            var hitboxes = root.Find("Hitboxes");
            AddToolHitbox(root, "HB_" + prefix + "_Body_CapsuleGuidance", bodyCenter, bodySize, material);
            var head = CreatePrimitivePart("HB_" + prefix + "_WeakPoint_SphereGuidance", PrimitiveType.Sphere, hitboxes, weakpointCenter, new Vector3(0.24f, 0.24f, 0.24f), material);
            var sphere = head.GetComponent<SphereCollider>();
            if (sphere != null)
            {
                sphere.isTrigger = true;
            }
        }

        private static void AddToolHitbox(Transform root, string name, Vector3 center, Vector3 size, Material material)
        {
            var hitboxes = root.Find("Hitboxes");
            var marker = CreatePrimitivePart(name, PrimitiveType.Cube, hitboxes, center, size, material);
            var collider = marker.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
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
            if (collider != null && !name.StartsWith("HB_", StringComparison.Ordinal))
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

        private static void CreateSocket(Transform parent, string name, Vector3 localPosition)
        {
            var socket = CreateEmpty(name, parent);
            socket.transform.localPosition = localPosition;
        }

        private static void SavePrefab(GameObject instance, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            UnityEngine.Object.DestroyImmediate(instance);
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
            var packageRoot = packageInfo != null ? packageInfo.resolvedPath : Path.Combine(Directory.GetCurrentDirectory(), "AssetPacks", "BrassworksBreach.MechanicalEnemies");
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
                SetMaterialFloat(material, "_Surface", 1f);
                SetMaterialFloat(material, "_AlphaClip", 0f);
                material.renderQueue = -1;
            }
            else
            {
                material.SetOverrideTag("RenderType", "");
                SetMaterialFloat(material, "_Surface", 0f);
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

        private static Mesh CreateClawHookMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(-0.05f, -0.22f, -0.05f), new Vector3(0.12f, -0.08f, -0.05f), new Vector3(0.20f, 0.13f, -0.05f), new Vector3(0.05f, 0.28f, -0.05f),
                new Vector3(-0.05f, -0.22f, 0.05f), new Vector3(0.12f, -0.08f, 0.05f), new Vector3(0.20f, 0.13f, 0.05f), new Vector3(0.05f, 0.28f, 0.05f)
            };
            var triangles = new[]
            {
                0, 1, 2, 0, 2, 3,
                4, 6, 5, 4, 7, 6,
                0, 4, 5, 0, 5, 1,
                1, 5, 6, 1, 6, 2,
                2, 6, 7, 2, 7, 3,
                3, 7, 4, 3, 4, 0
            };
            return BuildMesh(name, vertices, triangles);
        }

        private static Mesh CreateShieldPlateMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(-0.42f, -0.65f, -0.04f), new Vector3(0.42f, -0.65f, -0.04f), new Vector3(0.52f, 0.22f, -0.04f), new Vector3(0.28f, 0.68f, -0.04f), new Vector3(-0.28f, 0.68f, -0.04f), new Vector3(-0.52f, 0.22f, -0.04f),
                new Vector3(-0.42f, -0.65f, 0.04f), new Vector3(0.42f, -0.65f, 0.04f), new Vector3(0.52f, 0.22f, 0.04f), new Vector3(0.28f, 0.68f, 0.04f), new Vector3(-0.28f, 0.68f, 0.04f), new Vector3(-0.52f, 0.22f, 0.04f)
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

        private static Mesh CreateGearRingMesh(string name, int teeth, float innerRadius, float outerRadius)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var points = teeth * 2;
            for (var i = 0; i < points; i++)
            {
                var angle = i * Mathf.PI * 2f / points;
                var radius = i % 2 == 0 ? outerRadius : outerRadius * 0.87f;
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

        private static Mesh CreateGaugeFaceMesh(string name)
        {
            return CreateGearRingMesh(name, 20, 0.02f, 0.18f);
        }

        private static Mesh CreateShoulderFinMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(0f, -0.15f, -0.08f), new Vector3(0.55f, 0.04f, -0.08f), new Vector3(0.1f, 0.42f, -0.08f),
                new Vector3(0f, -0.15f, 0.08f), new Vector3(0.55f, 0.04f, 0.08f), new Vector3(0.1f, 0.42f, 0.08f)
            };
            var triangles = new[] { 0, 1, 2, 3, 5, 4, 0, 3, 4, 0, 4, 1, 1, 4, 5, 1, 5, 2, 2, 5, 3, 2, 3, 0 };
            return BuildMesh(name, vertices, triangles);
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

        private static void CreatePreviewEnvironment()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.12f, 0.095f, 0.065f, 1f);

            var floorMat = new Material(FindLitShader());
            SetMaterialColor(floorMat, "_BaseColor", new Color(0.08f, 0.075f, 0.065f, 1f));
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Preview_Foundry_Floor";
            floor.transform.localPosition = new Vector3(0f, -0.05f, 0.1f);
            floor.transform.localScale = new Vector3(8.5f, 0.1f, 5f);
            floor.GetComponent<MeshRenderer>().sharedMaterial = floorMat;

            var key = new GameObject("Preview_Key_Amber_Light").AddComponent<Light>();
            key.type = LightType.Spot;
            key.color = new Color(1f, 0.64f, 0.32f);
            key.intensity = 850f;
            key.range = 9f;
            key.spotAngle = 54f;
            key.transform.position = new Vector3(-2.8f, 4.5f, -3.0f);
            key.transform.rotation = Quaternion.Euler(58f, 28f, 0f);

            var rim = new GameObject("Preview_Cyan_Rim_Light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = CyanCharge;
            rim.intensity = 3.5f;
            rim.range = 5f;
            rim.transform.position = new Vector3(2.7f, 1.7f, -1.6f);
        }

        private static List<GameObject> InstantiateLineup()
        {
            var instances = new List<GameObject>();
            var spacing = 1.55f;
            var start = -spacing * 2f;

            for (var i = 0; i < GeneratedPrefabPaths.Count; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(GeneratedPrefabPaths[i]);
                if (prefab == null)
                {
                    Debug.LogWarning("Missing generated prefab for preview: " + GeneratedPrefabPaths[i]);
                    continue;
                }

                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.name = prefab.name;
                instance.transform.position = new Vector3(start + spacing * i, 0f, 0f);
                instance.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                instances.Add(instance);
            }

            return instances;
        }

        private static Camera CreatePreviewCamera(Vector3 position, Vector3 target, float fieldOfView)
        {
            var cameraObject = new GameObject("Preview_Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.035f, 0.031f, 0.028f, 1f);
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 50f;
            camera.transform.position = position;
            camera.transform.LookAt(target);
            return camera;
        }

        private static void FrameSingleEnemy(Camera camera, Transform target)
        {
            camera.transform.position = target.position + new Vector3(0f, 1.45f, -3.0f);
            camera.transform.LookAt(target.position + new Vector3(0f, 1.15f, 0f));
            camera.fieldOfView = target.name.Contains("Overseer", StringComparison.Ordinal) ? 48f : 42f;
        }

        private static void RenderCameraToPng(Camera camera, string absolutePath, int width, int height)
        {
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

        private static string ResolveRenderOutputRoot()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly());
            var packageRoot = packageInfo != null ? packageInfo.resolvedPath : Path.Combine(Directory.GetCurrentDirectory(), "AssetPacks", "BrassworksBreach.MechanicalEnemies");
            var repoRoot = Path.GetFullPath(Path.Combine(packageRoot, "..", ".."));
            return Path.Combine(repoRoot, RenderDocFolder);
        }
    }
}
