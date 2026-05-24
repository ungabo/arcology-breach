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

namespace BrassworksBreach.EncounterEnemySet02.Editor
{
    public static class EncounterEnemySet02Generator
    {
        private const string PackageName = "com.brassworks.sidecar.encounter-enemy-set02";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string Version = "0.1.41";
        private const string BuildId = "p001";
        private const string PackId = "EE02";
        private const string PrefabRoot = PackageRoot + "/Runtime/Prefabs";
        private const string MaterialRoot = PackageRoot + "/Runtime/Materials";
        private const string MeshRoot = PackageRoot + "/Runtime/Meshes";
        private const string MetadataRoot = PackageRoot + "/Runtime/Metadata";
        private const string PackageManifestFolder = "Documentation~/Manifest";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_41_EncounterEnemySet02";
        private const string ProductionDocFolder = "Documentation/AssetProduction/V0_1_41_EncounterEnemySet02";
        private const int ExpectedMaterialCount = 16;
        private const int ExpectedMeshCount = 12;
        private const int ExpectedPreviewPngCount = 21;

        private static readonly Color BlackIron = new Color(0.055f, 0.052f, 0.047f, 1f);
        private static readonly Color DarkSteel = new Color(0.18f, 0.18f, 0.17f, 1f);
        private static readonly Color AgedBrass = new Color(0.68f, 0.46f, 0.20f, 1f);
        private static readonly Color BurntCopper = new Color(0.72f, 0.30f, 0.13f, 1f);
        private static readonly Color PatinaPipe = new Color(0.15f, 0.48f, 0.39f, 1f);
        private static readonly Color OiledLeather = new Color(0.17f, 0.085f, 0.040f, 1f);
        private static readonly Color FurnaceEye = new Color(1.00f, 0.32f, 0.055f, 1f);
        private static readonly Color CyanPressure = new Color(0.08f, 0.74f, 1.00f, 1f);
        private static readonly Color WarningRed = new Color(0.90f, 0.06f, 0.035f, 1f);
        private static readonly Color GaugeIvory = new Color(0.76f, 0.68f, 0.52f, 1f);
        private static readonly Color HazardYellow = new Color(0.92f, 0.64f, 0.10f, 1f);
        private static readonly Color Soot = new Color(0.018f, 0.016f, 0.014f, 1f);
        private static readonly Color GrimyGlass = new Color(0.22f, 0.35f, 0.32f, 0.58f);
        private static readonly Color HotSlot = new Color(1.00f, 0.13f, 0.025f, 1f);
        private static readonly Color ReadabilityGhost = new Color(0.58f, 0.76f, 0.80f, 0.20f);
        private static readonly Color OilBlue = new Color(0.05f, 0.12f, 0.18f, 1f);

        private static readonly CandidateSpec[] Candidates =
        {
            new CandidateSpec("EE02_AshcanReclaimer_A_IdleSawScout", "Ashcan Reclaimer", "Idle Saw Scout", "neutral scout read", 0, 1.42f, 1.12f, 1.02f, "saw/claw", "Low furnace eyes and low saw silhouette signal close-range scrapper behavior."),
            new CandidateSpec("EE02_AshcanReclaimer_B_ClawWindupTell", "Ashcan Reclaimer", "Claw Windup Tell", "right claw raised attack tell", 1, 1.58f, 1.26f, 1.08f, "claw/saw", "Raised claw, cyan pressure hose, and ghost sweep show a readable rake windup."),
            new CandidateSpec("EE02_AshcanReclaimer_C_OvercrankLungeTell", "Ashcan Reclaimer", "Overcrank Lunge Tell", "forward saw lunge tell", 2, 1.50f, 1.34f, 1.30f, "saw", "Forward saw blade, hot slots, and crouched stance show an imminent lunge."),
            new CandidateSpec("EE02_AshcanReclaimer_D_TankBackStaggerRead", "Ashcan Reclaimer", "Tank Back Stagger Read", "tank-heavy recovery read", 3, 1.64f, 1.32f, 1.18f, "claw/tank", "Offset tanks and lowered tools make a future stagger pose easy to distinguish."),

            new CandidateSpec("EE02_PressureSpindle_A_BracePikeIdle", "Pressure Spindle", "Brace Pike Idle", "upright pike brace", 0, 2.08f, 0.84f, 1.18f, "lance", "Tall narrow pike profile with cyan accumulator reads as a lancer analogue without reusing Set 01 names."),
            new CandidateSpec("EE02_PressureSpindle_B_NeedleThrustTell", "Pressure Spindle", "Needle Thrust Tell", "needle thrust attack tell", 1, 2.14f, 0.92f, 1.48f, "lance", "Forward lance, compressed legs, and cyan aim line telegraph a thrust."),
            new CandidateSpec("EE02_PressureSpindle_C_TwinDrillChargeTell", "Pressure Spindle", "Twin Drill Charge Tell", "paired drill charge tell", 2, 2.02f, 1.02f, 1.34f, "drill", "Twin drills and bright pressure spine make charge timing readable from the front."),
            new CandidateSpec("EE02_PressureSpindle_D_HarpoonRailAimTell", "Pressure Spindle", "Harpoon Rail Aim Tell", "rail harpoon aim tell", 3, 2.20f, 0.96f, 1.62f, "rail lance", "Long rail silhouette and muzzle socket provide a future ranged tell hook."),

            new CandidateSpec("EE02_GatehammerBastion_A_ShieldedIdle", "Gatehammer Bastion", "Shielded Idle", "wide shield idle", 0, 2.04f, 1.66f, 1.06f, "shield/hammer", "Broad shield, furnace chest, and squat legs read as a blocker silhouette."),
            new CandidateSpec("EE02_GatehammerBastion_B_HammerBackswingTell", "Gatehammer Bastion", "Hammer Backswing Tell", "hammer windup attack tell", 1, 2.18f, 1.82f, 1.18f, "hammer", "Hammer carried behind shoulder and amber warning lamps telegraph a heavy swing."),
            new CandidateSpec("EE02_GatehammerBastion_C_FurnaceVentGuard", "Gatehammer Bastion", "Furnace Vent Guard", "venting guard read", 2, 2.10f, 1.74f, 1.26f, "shield/furnace", "Open furnace vents, red heat slits, and shield clamps communicate a guarded pressure state."),
            new CandidateSpec("EE02_GatehammerBastion_D_KneelingBreachSlamTell", "Gatehammer Bastion", "Kneeling Breach Slam Tell", "low breach slam tell", 3, 1.88f, 1.94f, 1.40f, "hammer/shield", "Lowered chassis and ghost impact arc make a future slam startup pose obvious."),

            new CandidateSpec("EE02_GovernorWarden_A_TallCommandIdle", "Governor Warden", "Tall Command Idle", "tall command idle", 0, 2.68f, 1.34f, 1.24f, "command halo", "Tall governor profile, gear halo, and furnace monocle establish elite read."),
            new CandidateSpec("EE02_GovernorWarden_B_BellBeaconCastTell", "Governor Warden", "Bell Beacon Cast Tell", "beacon cast attack tell", 1, 2.82f, 1.48f, 1.34f, "beacon", "Raised bell beacon and cyan command nodes telegraph a future support cast."),
            new CandidateSpec("EE02_GovernorWarden_C_DualClawJudgementTell", "Governor Warden", "Dual Claw Judgement Tell", "dual claw execution tell", 2, 2.62f, 1.62f, 1.36f, "dual claw", "Symmetric raised claws and hot eye line make an execution tell readable."),
            new CandidateSpec("EE02_GovernorWarden_D_OverheatEnrageTell", "Governor Warden", "Overheat Enrage Tell", "overheat enrage read", 3, 2.90f, 1.72f, 1.44f, "halo/furnace", "Expanded halo, vent stacks, and red-hot core communicate an elite overheat state.")
        };

        [MenuItem("Brassworks Breach/Sidecars/Encounter Enemy Set 02/Generate Package Assets")]
        public static void GeneratePackageAssets()
        {
            EnsurePackageFolders();

            var materials = CreateMaterials();
            var meshes = CreateMeshes();

            foreach (var candidate in Candidates)
            {
                var instance = BuildCandidate(candidate, materials, meshes);
                SavePrefab(instance, PrefabRoot + "/" + candidate.Id + ".prefab");
            }

            WriteRuntimeCatalog();
            WritePackageManifest("generated_assets_pending_preview_validation", 0);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("EE02_GENERATE_PASS v0.1.41 prefabs=" + Candidates.Length + " materials=" + ExpectedMaterialCount + " meshes=" + ExpectedMeshCount);
        }

        [MenuItem("Brassworks Breach/Sidecars/Encounter Enemy Set 02/Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            GeneratePackageAssets();

            var outputRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            Directory.CreateDirectory(outputRoot);

            RenderContactSheet(outputRoot, "EE02_v0.1.41_all_candidates_contact_sheet.png", Candidates, 4, 3000, 2100);

            foreach (var family in Candidates.Select(candidate => candidate.Family).Distinct())
            {
                var familyCandidates = Candidates.Where(candidate => candidate.Family == family).ToArray();
                RenderContactSheet(outputRoot, "EE02_v0.1.41_" + MakeSafeFileName(family) + "_contact_sheet.png", familyCandidates, 4, 2400, 1400);
            }

            foreach (var candidate in Candidates)
            {
                RenderSingleCandidate(outputRoot, candidate, 1400, 1400);
            }

            WritePreviewPixelEvidence(outputRoot);
            WritePackageManifest("unity_generation_and_preview_render_passed", Directory.GetFiles(outputRoot, "*.png").Length);
            AssetDatabase.Refresh();
            Debug.Log("EE02_PREVIEW_PASS v0.1.41 output=" + outputRoot);
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
            return new Dictionary<string, Material>(StringComparer.Ordinal)
            {
                ["BlackIron"] = UpsertMaterial("EE02_MAT_BlackenedIron", BlackIron, 0.92f, 0.23f, Color.black, false),
                ["DarkSteel"] = UpsertMaterial("EE02_MAT_WornDarkSteel", DarkSteel, 0.88f, 0.36f, Color.black, false),
                ["Brass"] = UpsertMaterial("EE02_MAT_AgedBrass", AgedBrass, 0.80f, 0.42f, Color.black, false),
                ["Copper"] = UpsertMaterial("EE02_MAT_BurntCopper", BurntCopper, 0.72f, 0.38f, Color.black, false),
                ["Patina"] = UpsertMaterial("EE02_MAT_PatinaPressurePipe", PatinaPipe, 0.55f, 0.30f, Color.black, false),
                ["Leather"] = UpsertMaterial("EE02_MAT_OiledBellowsLeather", OiledLeather, 0.05f, 0.48f, Color.black, false),
                ["FurnaceEye"] = UpsertMaterial("EE02_MAT_EmissiveFurnaceEye", FurnaceEye, 0.0f, 0.25f, FurnaceEye * 3.4f, false),
                ["Cyan"] = UpsertMaterial("EE02_MAT_CyanPressureTell", CyanPressure, 0.0f, 0.32f, CyanPressure * 2.8f, false),
                ["Red"] = UpsertMaterial("EE02_MAT_RedOverheatTell", WarningRed, 0.0f, 0.28f, WarningRed * 3.0f, false),
                ["Gauge"] = UpsertMaterial("EE02_MAT_SootStainedGaugeIvory", GaugeIvory, 0.0f, 0.33f, Color.black, false),
                ["Hazard"] = UpsertMaterial("EE02_MAT_ChippedHazardYellow", HazardYellow, 0.18f, 0.32f, Color.black, false),
                ["Soot"] = UpsertMaterial("EE02_MAT_SootBlackGrime", Soot, 0.20f, 0.16f, Color.black, false),
                ["Glass"] = UpsertMaterial("EE02_MAT_GrimyPressureGlass", GrimyGlass, 0.0f, 0.70f, Color.black, true),
                ["HotSlot"] = UpsertMaterial("EE02_MAT_RedHotFurnaceSlit", HotSlot, 0.20f, 0.24f, HotSlot * 3.2f, false),
                ["Ghost"] = UpsertMaterial("EE02_MAT_ReadabilityGhost", ReadabilityGhost, 0.0f, 0.18f, Color.black, true),
                ["OilBlue"] = UpsertMaterial("EE02_MAT_OilBlueTemperedSteel", OilBlue, 0.76f, 0.42f, Color.black, false)
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            return new Dictionary<string, Mesh>(StringComparer.Ordinal)
            {
                ["SawBlade"] = UpsertMesh("EE02_MESH_36ToothSawBlade", CreateSawBladeMesh("EE02_MESH_36ToothSawBlade", 36, 0.20f, 0.43f)),
                ["ClawTalon"] = UpsertMesh("EE02_MESH_SteamClawTalon", CreateTalonMesh("EE02_MESH_SteamClawTalon")),
                ["HammerHead"] = UpsertMesh("EE02_MESH_RivetHammerHead", CreateBoxMesh("EE02_MESH_RivetHammerHead", new Vector3(0.42f, 0.26f, 0.30f))),
                ["LanceTip"] = UpsertMesh("EE02_MESH_PressureNeedleLanceTip", CreateConeMesh("EE02_MESH_PressureNeedleLanceTip", 0.16f, 0.82f, 24)),
                ["DrillBit"] = UpsertMesh("EE02_MESH_TwinSpiralDrillBit", CreateDrillBitMesh("EE02_MESH_TwinSpiralDrillBit", 0.15f, 0.70f, 14)),
                ["GearHalo"] = UpsertMesh("EE02_MESH_GovernorCommandGearHalo", CreateGearRingMesh("EE02_MESH_GovernorCommandGearHalo", 28, 0.27f, 0.52f)),
                ["ShieldPlate"] = UpsertMesh("EE02_MESH_GatehammerShieldPlate", CreateShieldPlateMesh("EE02_MESH_GatehammerShieldPlate")),
                ["FurnaceGrate"] = UpsertMesh("EE02_MESH_FurnaceGrateFace", CreateGrateMesh("EE02_MESH_FurnaceGrateFace")),
                ["ValveWheel"] = UpsertMesh("EE02_MESH_ValveWheelTell", CreateGearRingMesh("EE02_MESH_ValveWheelTell", 12, 0.07f, 0.24f)),
                ["TankCap"] = UpsertMesh("EE02_MESH_PressureTankCap", CreateGearRingMesh("EE02_MESH_PressureTankCap", 18, 0.05f, 0.18f)),
                ["ShoulderFin"] = UpsertMesh("EE02_MESH_WardenShoulderFin", CreateFinMesh("EE02_MESH_WardenShoulderFin")),
                ["GaugeFace"] = UpsertMesh("EE02_MESH_BoilerGaugeFace", CreateGearRingMesh("EE02_MESH_BoilerGaugeFace", 20, 0.035f, 0.18f))
            };
        }

        private static GameObject BuildCandidate(CandidateSpec candidate, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var root = CreateBaseCandidate(candidate);
            var groups = GetGroups(root.transform);

            if (candidate.Family == "Ashcan Reclaimer")
            {
                BuildAshcanReclaimer(candidate, groups, mat, mesh);
            }
            else if (candidate.Family == "Pressure Spindle")
            {
                BuildPressureSpindle(candidate, groups, mat, mesh);
            }
            else if (candidate.Family == "Gatehammer Bastion")
            {
                BuildGatehammerBastion(candidate, groups, mat, mesh);
            }
            else
            {
                BuildGovernorWarden(candidate, groups, mat, mesh);
            }

            AddFutureRigSockets(candidate, groups["future_rig_sockets"], mat["Cyan"]);
            AddScaleEnvelope(candidate, groups["readable_tells"], mat["Ghost"]);
            return root;
        }

        private static GameObject CreateBaseCandidate(CandidateSpec candidate)
        {
            var root = new GameObject(candidate.Id);
            CreateEmpty("visual_only_no_gameplay_authority", root.transform);
            CreateEmpty("no_colliders_no_audio_no_ai", root.transform);
            CreateEmpty("future_primary_lane_adds_hitboxes_nav_damage_audio_and_animation", root.transform);
            CreateEmpty("family_" + MakeSafeFileName(candidate.Family), root.transform);
            CreateEmpty("pose_" + MakeSafeFileName(candidate.Pose), root.transform);
            CreateEmpty("module_" + MakeSafeFileName(candidate.WeaponModule), root.transform);

            CreateEmpty("chassis", root.transform);
            CreateEmpty("boiler_tanks", root.transform);
            CreateEmpty("head_lenses", root.transform);
            CreateEmpty("arms_modules", root.transform);
            CreateEmpty("pressure_lines", root.transform);
            CreateEmpty("armor_plates", root.transform);
            CreateEmpty("readable_tells", root.transform);
            CreateEmpty("rivets_gauges", root.transform);
            CreateEmpty("future_rig_sockets", root.transform);
            return root;
        }

        private static Dictionary<string, Transform> GetGroups(Transform root)
        {
            var groups = new Dictionary<string, Transform>(StringComparer.Ordinal);
            foreach (Transform child in root)
            {
                groups[child.name] = child;
            }

            return groups;
        }

        private static void BuildAshcanReclaimer(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var v = candidate.VariantIndex;
            var bodyY = 0.50f + (v == 3 ? 0.06f : 0f);

            CreatePrimitivePart("low_riveted_cradle", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, bodyY, 0.02f), new Vector3(0.72f + v * 0.04f, 0.24f, 0.72f), Quaternion.identity, mat["BlackIron"]);
            CreatePrimitivePart("forward_jaw_block", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, bodyY + 0.13f, -0.44f), new Vector3(0.48f, 0.18f, 0.20f), Quaternion.identity, mat["Soot"]);
            CreatePrimitivePart("rear_boiler_keg", PrimitiveType.Cylinder, groups["boiler_tanks"], new Vector3(0f, 0.78f, 0.22f), new Vector3(0.40f, 0.34f, 0.40f), Quaternion.identity, mat["Brass"]);
            AddTankCap(groups["boiler_tanks"], mesh, mat, "rear_boiler_front_cap", new Vector3(0f, 0.78f, -0.13f), 0.95f);
            AddTankCap(groups["boiler_tanks"], mesh, mat, "rear_boiler_back_cap", new Vector3(0f, 0.78f, 0.57f), 0.95f);

            CreatePrimitivePart("low_furnace_head", PrimitiveType.Sphere, groups["head_lenses"], new Vector3(0f, 1.09f + v * 0.03f, -0.18f), new Vector3(0.38f, 0.28f, 0.32f), Quaternion.identity, mat["DarkSteel"]);
            CreateLensPair(groups["head_lenses"], new Vector3(0f, 1.10f + v * 0.03f, -0.42f), 0.12f, 0.065f, mat);
            AddGauge(groups["rivets_gauges"], mesh, mat, "jaw_pressure_gauge", new Vector3(0.27f, 0.82f, -0.44f), Quaternion.Euler(0f, 20f, 0f));

            AddLeg(groups["chassis"], mat, "front_left", new Vector3(-0.28f, bodyY - 0.02f, -0.23f), new Vector3(-0.42f, 0.10f, -0.34f));
            AddLeg(groups["chassis"], mat, "front_right", new Vector3(0.28f, bodyY - 0.02f, -0.23f), new Vector3(0.42f, 0.10f, -0.34f));
            AddLeg(groups["chassis"], mat, "rear_left", new Vector3(-0.26f, bodyY - 0.02f, 0.24f), new Vector3(-0.40f, 0.10f, 0.32f));
            AddLeg(groups["chassis"], mat, "rear_right", new Vector3(0.26f, bodyY - 0.02f, 0.24f), new Vector3(0.40f, 0.10f, 0.32f));

            if (v == 0)
            {
                AddSawArm(groups["arms_modules"], mesh, mat, "left_idle_saw", new Vector3(-0.50f, 0.67f, -0.17f), new Vector3(-0.84f, 0.50f, -0.34f), Quaternion.Euler(0f, 90f, -12f), 0.92f);
                AddClawArm(groups["arms_modules"], mesh, mat, "right_idle_claw", new Vector3(0.50f, 0.70f, -0.10f), new Vector3(0.83f, 0.58f, -0.24f), Quaternion.Euler(0f, -32f, -20f), 0.90f);
            }
            else if (v == 1)
            {
                AddSawArm(groups["arms_modules"], mesh, mat, "left_floor_saw", new Vector3(-0.48f, 0.66f, -0.16f), new Vector3(-0.76f, 0.42f, -0.45f), Quaternion.Euler(0f, 90f, -20f), 0.88f);
                AddClawArm(groups["arms_modules"], mesh, mat, "right_raised_claw_windup", new Vector3(0.48f, 0.78f, -0.08f), new Vector3(0.75f, 1.22f, -0.28f), Quaternion.Euler(-38f, -28f, -68f), 1.08f);
                AddGhostArc(groups["readable_tells"], mat["Ghost"], "claw_rake_sweep_readability_arc", new Vector3(0.86f, 0.98f, -0.36f), new Vector3(0.08f, 0.82f, 0.42f), Quaternion.Euler(0f, 0f, -26f));
            }
            else if (v == 2)
            {
                AddSawArm(groups["arms_modules"], mesh, mat, "center_overcrank_lunge_saw", new Vector3(0f, 0.70f, -0.35f), new Vector3(0f, 0.66f, -0.82f), Quaternion.Euler(0f, 0f, 0f), 1.12f);
                AddClawArm(groups["arms_modules"], mesh, mat, "left_balance_claw", new Vector3(-0.48f, 0.74f, -0.02f), new Vector3(-0.72f, 0.66f, -0.34f), Quaternion.Euler(0f, 18f, -28f), 0.82f);
                AddClawArm(groups["arms_modules"], mesh, mat, "right_balance_claw", new Vector3(0.48f, 0.74f, -0.02f), new Vector3(0.72f, 0.66f, -0.34f), Quaternion.Euler(0f, -18f, 28f), 0.82f);
                AddTellStripe(groups["readable_tells"], mat["HotSlot"], "overcrank_hot_saw_warning", new Vector3(0f, 0.90f, -0.58f), new Vector3(0.52f, 0.035f, 0.05f));
            }
            else
            {
                AddClawArm(groups["arms_modules"], mesh, mat, "left_low_recovery_claw", new Vector3(-0.50f, 0.70f, -0.05f), new Vector3(-0.86f, 0.42f, -0.10f), Quaternion.Euler(0f, 16f, 12f), 0.92f);
                AddSawArm(groups["arms_modules"], mesh, mat, "right_dragging_saw", new Vector3(0.50f, 0.67f, -0.12f), new Vector3(0.88f, 0.42f, -0.28f), Quaternion.Euler(0f, 90f, 24f), 0.90f);
                AddPressureTank(groups["boiler_tanks"], mesh, mat, "offset_left_pressure_tank", new Vector3(-0.36f, 1.03f, 0.23f), 0.16f, 0.54f);
                AddPressureTank(groups["boiler_tanks"], mesh, mat, "offset_right_pressure_tank", new Vector3(0.36f, 0.98f, 0.27f), 0.14f, 0.48f);
            }

            AddPressurePipe(groups["pressure_lines"], mat["Patina"], "left_pressure_hose", new Vector3(-0.18f, 0.87f, 0.24f), new Vector3(-0.72f, 0.62f, -0.14f), 0.030f);
            AddPressurePipe(groups["pressure_lines"], mat["Patina"], "right_pressure_hose", new Vector3(0.18f, 0.87f, 0.24f), new Vector3(0.72f, 0.62f, -0.14f), 0.030f);
            AddRivetRow(groups["rivets_gauges"], mat["Brass"], "ashcan_front_rivets", 8, new Vector3(-0.34f, 0.66f, -0.54f), new Vector3(0.68f, 0f, 0f));
            AddWarningLamp(groups["readable_tells"], mat["Cyan"], "back_pressure_pilot_left", new Vector3(-0.22f, 1.14f, 0.42f), 0.055f);
            AddWarningLamp(groups["readable_tells"], mat["Cyan"], "back_pressure_pilot_right", new Vector3(0.22f, 1.14f, 0.42f), 0.055f);
        }

        private static void BuildPressureSpindle(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var v = candidate.VariantIndex;
            CreatePrimitivePart("needle_stilt_frame", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 1.00f, 0.02f), new Vector3(0.38f, 1.08f, 0.25f), Quaternion.identity, mat["BlackIron"]);
            CreatePrimitivePart("vertical_pressure_boiler", PrimitiveType.Cylinder, groups["boiler_tanks"], new Vector3(0f, 1.22f, 0.08f), new Vector3(0.30f, 0.58f, 0.30f), Quaternion.identity, mat["Brass"]);
            CreatePrimitivePart("cyan_accumulator_spine", PrimitiveType.Cylinder, groups["boiler_tanks"], new Vector3(0f, 1.30f, 0.38f), new Vector3(0.075f, 0.50f, 0.075f), Quaternion.identity, mat["Cyan"]);
            CreatePrimitivePart("long_visor_head", PrimitiveType.Sphere, groups["head_lenses"], new Vector3(0f, 1.86f + v * 0.03f, -0.05f), new Vector3(0.34f, 0.24f, 0.36f), Quaternion.identity, mat["DarkSteel"]);
            CreateLensPair(groups["head_lenses"], new Vector3(0f, 1.86f + v * 0.03f, -0.34f), 0.09f, 0.058f, mat);

            AddLeg(groups["chassis"], mat, "left_stilt", new Vector3(-0.13f, 0.52f, -0.03f), new Vector3(-0.36f, 0.08f, -0.18f));
            AddLeg(groups["chassis"], mat, "right_stilt", new Vector3(0.13f, 0.52f, -0.03f), new Vector3(0.36f, 0.08f, -0.18f));
            AddLeg(groups["chassis"], mat, "rear_left_stilt", new Vector3(-0.13f, 0.52f, 0.14f), new Vector3(-0.32f, 0.08f, 0.30f));
            AddLeg(groups["chassis"], mat, "rear_right_stilt", new Vector3(0.13f, 0.52f, 0.14f), new Vector3(0.32f, 0.08f, 0.30f));

            if (v == 0)
            {
                AddLanceArm(groups["arms_modules"], mesh, mat, "brace_pike", new Vector3(0.24f, 1.34f, -0.04f), new Vector3(0.42f, 1.10f, -0.84f), Quaternion.Euler(18f, 0f, -12f), 1.00f);
                AddClawArm(groups["arms_modules"], mesh, mat, "left_counter_clamp", new Vector3(-0.28f, 1.22f, -0.02f), new Vector3(-0.54f, 1.02f, -0.20f), Quaternion.Euler(0f, 20f, -20f), 0.62f);
            }
            else if (v == 1)
            {
                AddLanceArm(groups["arms_modules"], mesh, mat, "needle_thrust_forward", new Vector3(0f, 1.38f, -0.26f), new Vector3(0f, 1.36f, -1.12f), Quaternion.identity, 1.22f);
                AddGhostArc(groups["readable_tells"], mat["Ghost"], "needle_thrust_cyan_aim_column", new Vector3(0f, 1.36f, -0.86f), new Vector3(0.07f, 0.07f, 0.92f), Quaternion.identity);
                AddTellStripe(groups["readable_tells"], mat["Cyan"], "compressed_thrust_charge_bar", new Vector3(0f, 1.50f, -0.42f), new Vector3(0.38f, 0.035f, 0.045f));
            }
            else if (v == 2)
            {
                AddDrillArm(groups["arms_modules"], mesh, mat, "left_charging_drill", new Vector3(-0.28f, 1.30f, -0.14f), new Vector3(-0.34f, 1.22f, -0.86f), Quaternion.Euler(0f, 0f, 0f), 1.02f);
                AddDrillArm(groups["arms_modules"], mesh, mat, "right_charging_drill", new Vector3(0.28f, 1.30f, -0.14f), new Vector3(0.34f, 1.22f, -0.86f), Quaternion.Euler(0f, 0f, 0f), 1.02f);
                AddTellStripe(groups["readable_tells"], mat["Cyan"], "twin_drill_charge_meter", new Vector3(0f, 1.70f, -0.31f), new Vector3(0.46f, 0.04f, 0.05f));
            }
            else
            {
                AddLanceArm(groups["arms_modules"], mesh, mat, "harpoon_rail_long_barrel", new Vector3(0f, 1.46f, -0.28f), new Vector3(0f, 1.52f, -1.28f), Quaternion.identity, 1.38f);
                CreatePrimitivePart("rail_left_copper_track", PrimitiveType.Cube, groups["arms_modules"], new Vector3(-0.075f, 1.55f, -0.78f), new Vector3(0.035f, 0.035f, 0.92f), Quaternion.identity, mat["Copper"]);
                CreatePrimitivePart("rail_right_copper_track", PrimitiveType.Cube, groups["arms_modules"], new Vector3(0.075f, 1.55f, -0.78f), new Vector3(0.035f, 0.035f, 0.92f), Quaternion.identity, mat["Copper"]);
                AddWarningLamp(groups["readable_tells"], mat["Cyan"], "harpoon_muzzle_socket_marker", new Vector3(0f, 1.54f, -1.48f), 0.055f);
            }

            AddPressureTank(groups["boiler_tanks"], mesh, mat, "left_back_pressure_tank", new Vector3(-0.25f, 1.30f, 0.36f), 0.13f, 0.56f);
            AddPressureTank(groups["boiler_tanks"], mesh, mat, "right_back_pressure_tank", new Vector3(0.25f, 1.30f, 0.36f), 0.13f, 0.56f);
            AddPressurePipe(groups["pressure_lines"], mat["Patina"], "lance_feed_hose", new Vector3(0f, 1.46f, 0.34f), new Vector3(0f, 1.35f, -0.65f), 0.028f);
            AddGauge(groups["rivets_gauges"], mesh, mat, "spindle_back_gauge", new Vector3(-0.24f, 1.55f, 0.22f), Quaternion.Euler(0f, -26f, 0f));
            AddRivetRow(groups["rivets_gauges"], mat["Brass"], "spindle_front_rivets", 6, new Vector3(-0.18f, 1.28f, -0.18f), new Vector3(0.36f, 0f, 0f));
        }

        private static void BuildGatehammerBastion(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var v = candidate.VariantIndex;
            var lowered = v == 3 ? -0.18f : 0f;
            CreatePrimitivePart("broad_blocker_chassis", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 1.02f + lowered, 0.02f), new Vector3(1.12f, 1.16f, 0.48f), Quaternion.identity, mat["BlackIron"]);
            CreatePrimitivePart("ribbed_boiler_belly", PrimitiveType.Cylinder, groups["boiler_tanks"], new Vector3(0f, 1.08f + lowered, -0.10f), new Vector3(0.54f, 0.42f, 0.54f), Quaternion.Euler(90f, 0f, 0f), mat["Brass"]);
            AddFurnaceCore(groups["head_lenses"], mesh, mat, "bastion_furnace_core", new Vector3(0f, 1.28f + lowered, -0.38f), 1.0f + v * 0.08f);
            CreatePrimitivePart("helmet_slab", PrimitiveType.Cube, groups["head_lenses"], new Vector3(0f, 1.88f + lowered, -0.05f), new Vector3(0.62f, 0.24f, 0.36f), Quaternion.identity, mat["DarkSteel"]);
            CreateLensPair(groups["head_lenses"], new Vector3(0f, 1.88f + lowered, -0.30f), 0.13f, 0.060f, mat);

            AddLeg(groups["chassis"], mat, "left_heavy_leg", new Vector3(-0.36f, 0.55f + lowered, -0.02f), new Vector3(-0.54f, 0.10f, -0.20f));
            AddLeg(groups["chassis"], mat, "right_heavy_leg", new Vector3(0.36f, 0.55f + lowered, -0.02f), new Vector3(0.54f, 0.10f, -0.20f));
            AddLeg(groups["chassis"], mat, "left_rear_brace", new Vector3(-0.36f, 0.55f + lowered, 0.20f), new Vector3(-0.56f, 0.10f, 0.36f));
            AddLeg(groups["chassis"], mat, "right_rear_brace", new Vector3(0.36f, 0.55f + lowered, 0.20f), new Vector3(0.56f, 0.10f, 0.36f));

            if (v == 0)
            {
                AddShield(groups["armor_plates"], mesh, mat, "center_gate_shield_idle", new Vector3(-0.48f, 1.16f, -0.46f), Quaternion.Euler(0f, -10f, 0f), 1.02f);
                AddHammerArm(groups["arms_modules"], mesh, mat, "right_idle_hammer", new Vector3(0.60f, 1.34f, -0.10f), new Vector3(0.84f, 0.82f, -0.32f), Quaternion.Euler(0f, 0f, -18f), 0.92f);
            }
            else if (v == 1)
            {
                AddShield(groups["armor_plates"], mesh, mat, "left_shoulder_shield_counterweight", new Vector3(-0.62f, 1.24f, -0.40f), Quaternion.Euler(0f, -20f, 0f), 0.86f);
                AddHammerArm(groups["arms_modules"], mesh, mat, "right_hammer_backswing_read", new Vector3(0.58f, 1.45f, -0.02f), new Vector3(1.08f, 1.86f, -0.08f), Quaternion.Euler(0f, 0f, -72f), 1.14f);
                AddGhostArc(groups["readable_tells"], mat["Ghost"], "hammer_backswing_arc", new Vector3(0.95f, 1.52f, -0.24f), new Vector3(0.12f, 0.92f, 0.50f), Quaternion.Euler(0f, 0f, -38f));
            }
            else if (v == 2)
            {
                AddShield(groups["armor_plates"], mesh, mat, "clamped_guard_shield_left", new Vector3(-0.58f, 1.10f, -0.48f), Quaternion.Euler(0f, -4f, 0f), 0.98f);
                AddShield(groups["armor_plates"], mesh, mat, "clamped_guard_shield_right", new Vector3(0.58f, 1.10f, -0.48f), Quaternion.Euler(0f, 4f, 0f), 0.98f);
                AddTellStripe(groups["readable_tells"], mat["HotSlot"], "furnace_vent_hot_left", new Vector3(-0.18f, 1.30f + lowered, -0.52f), new Vector3(0.08f, 0.34f, 0.035f));
                AddTellStripe(groups["readable_tells"], mat["HotSlot"], "furnace_vent_hot_right", new Vector3(0.18f, 1.30f + lowered, -0.52f), new Vector3(0.08f, 0.34f, 0.035f));
            }
            else
            {
                AddShield(groups["armor_plates"], mesh, mat, "low_breach_shield", new Vector3(-0.60f, 0.88f, -0.48f), Quaternion.Euler(0f, -16f, 0f), 0.92f);
                AddHammerArm(groups["arms_modules"], mesh, mat, "low_breach_slam_hammer", new Vector3(0.50f, 0.92f, -0.18f), new Vector3(0.78f, 0.28f, -0.64f), Quaternion.Euler(52f, 0f, -12f), 1.20f);
                AddGhostArc(groups["readable_tells"], mat["Ghost"], "breach_slam_impact_wedge", new Vector3(0.44f, 0.28f, -0.74f), new Vector3(0.84f, 0.05f, 0.46f), Quaternion.Euler(0f, 0f, 0f));
            }

            AddPressureTank(groups["boiler_tanks"], mesh, mat, "left_shoulder_tank", new Vector3(-0.52f, 1.80f + lowered, 0.20f), 0.15f, 0.52f);
            AddPressureTank(groups["boiler_tanks"], mesh, mat, "right_shoulder_tank", new Vector3(0.52f, 1.80f + lowered, 0.20f), 0.15f, 0.52f);
            AddPressurePipe(groups["pressure_lines"], mat["Copper"], "left_shield_feed_pipe", new Vector3(-0.38f, 1.50f + lowered, 0.18f), new Vector3(-0.58f, 1.08f + lowered, -0.46f), 0.035f);
            AddPressurePipe(groups["pressure_lines"], mat["Copper"], "right_hammer_feed_pipe", new Vector3(0.38f, 1.50f + lowered, 0.18f), new Vector3(0.72f, 1.10f + lowered, -0.30f), 0.035f);
            AddRivetRow(groups["rivets_gauges"], mat["Brass"], "bastion_brow_rivets", 9, new Vector3(-0.42f, 2.03f + lowered, -0.24f), new Vector3(0.84f, 0f, 0f));
            AddGauge(groups["rivets_gauges"], mesh, mat, "bastion_furnace_pressure_gauge", new Vector3(0.42f, 1.54f + lowered, -0.42f), Quaternion.Euler(0f, 0f, 0f));
        }

        private static void BuildGovernorWarden(CandidateSpec candidate, Dictionary<string, Transform> groups, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            var v = candidate.VariantIndex;
            CreatePrimitivePart("tall_governor_spine", PrimitiveType.Cube, groups["chassis"], new Vector3(0f, 1.35f, 0.04f), new Vector3(0.54f, 1.72f, 0.36f), Quaternion.identity, mat["BlackIron"]);
            CreatePrimitivePart("bell_boiler_torso", PrimitiveType.Cylinder, groups["boiler_tanks"], new Vector3(0f, 1.44f, 0.02f), new Vector3(0.46f, 0.58f, 0.46f), Quaternion.identity, mat["Brass"]);
            CreatePrimitivePart("warden_head_bell", PrimitiveType.Sphere, groups["head_lenses"], new Vector3(0f, 2.26f + v * 0.03f, -0.04f), new Vector3(0.44f, 0.34f, 0.38f), Quaternion.identity, mat["DarkSteel"]);
            CreateLensPair(groups["head_lenses"], new Vector3(0f, 2.26f + v * 0.03f, -0.35f), 0.15f, 0.070f, mat);
            CreateMeshPart("command_gear_halo", mesh["GearHalo"], groups["readable_tells"], new Vector3(0f, 2.34f + v * 0.05f, 0.08f), new Vector3(1.05f + v * 0.08f, 1.05f + v * 0.08f, 1.05f), Quaternion.identity, mat["Brass"]);
            AddFurnaceCore(groups["head_lenses"], mesh, mat, "warden_chest_furnace", new Vector3(0f, 1.46f, -0.36f), 0.82f + v * 0.04f);

            AddLeg(groups["chassis"], mat, "left_long_leg", new Vector3(-0.20f, 0.72f, -0.02f), new Vector3(-0.42f, 0.10f, -0.18f));
            AddLeg(groups["chassis"], mat, "right_long_leg", new Vector3(0.20f, 0.72f, -0.02f), new Vector3(0.42f, 0.10f, -0.18f));
            AddLeg(groups["chassis"], mat, "left_rear_tripod_leg", new Vector3(-0.18f, 0.74f, 0.18f), new Vector3(-0.34f, 0.10f, 0.40f));
            AddLeg(groups["chassis"], mat, "right_rear_tripod_leg", new Vector3(0.18f, 0.74f, 0.18f), new Vector3(0.34f, 0.10f, 0.40f));

            CreateMeshPart("left_shoulder_fin", mesh["ShoulderFin"], groups["armor_plates"], new Vector3(-0.48f, 2.05f, -0.02f), new Vector3(1.0f, 1.0f, 1.0f), Quaternion.Euler(0f, 180f, 12f), mat["OilBlue"]);
            CreateMeshPart("right_shoulder_fin", mesh["ShoulderFin"], groups["armor_plates"], new Vector3(0.48f, 2.05f, -0.02f), new Vector3(1.0f, 1.0f, 1.0f), Quaternion.Euler(0f, 0f, -12f), mat["OilBlue"]);

            if (v == 0)
            {
                AddLanceArm(groups["arms_modules"], mesh, mat, "left_command_cane", new Vector3(-0.38f, 1.68f, -0.08f), new Vector3(-0.64f, 1.06f, -0.34f), Quaternion.Euler(0f, 12f, 24f), 0.78f);
                AddClawArm(groups["arms_modules"], mesh, mat, "right_idle_judgement_claw", new Vector3(0.38f, 1.70f, -0.06f), new Vector3(0.66f, 1.28f, -0.28f), Quaternion.Euler(0f, -20f, 10f), 0.72f);
            }
            else if (v == 1)
            {
                AddBellBeacon(groups["readable_tells"], mesh, mat, "raised_bell_beacon", new Vector3(0f, 2.86f, -0.02f), 1.0f);
                AddLanceArm(groups["arms_modules"], mesh, mat, "left_casting_cane", new Vector3(-0.38f, 1.78f, -0.08f), new Vector3(-0.74f, 2.12f, -0.32f), Quaternion.Euler(-42f, 16f, 42f), 0.75f);
                AddWarningLamp(groups["readable_tells"], mat["Cyan"], "command_node_left", new Vector3(-0.36f, 2.56f, -0.16f), 0.055f);
                AddWarningLamp(groups["readable_tells"], mat["Cyan"], "command_node_right", new Vector3(0.36f, 2.56f, -0.16f), 0.055f);
            }
            else if (v == 2)
            {
                AddClawArm(groups["arms_modules"], mesh, mat, "left_raised_execution_claw", new Vector3(-0.40f, 1.72f, -0.06f), new Vector3(-0.78f, 2.12f, -0.34f), Quaternion.Euler(-44f, 18f, 48f), 1.08f);
                AddClawArm(groups["arms_modules"], mesh, mat, "right_raised_execution_claw", new Vector3(0.40f, 1.72f, -0.06f), new Vector3(0.78f, 2.12f, -0.34f), Quaternion.Euler(-44f, -18f, -48f), 1.08f);
                AddGhostArc(groups["readable_tells"], mat["Ghost"], "dual_claw_execution_window", new Vector3(0f, 1.86f, -0.48f), new Vector3(1.18f, 0.10f, 0.42f), Quaternion.identity);
            }
            else
            {
                AddBellBeacon(groups["readable_tells"], mesh, mat, "overheat_expanded_beacon", new Vector3(0f, 2.96f, -0.02f), 1.22f);
                AddClawArm(groups["arms_modules"], mesh, mat, "left_overheat_claw", new Vector3(-0.44f, 1.78f, -0.08f), new Vector3(-0.94f, 1.72f, -0.34f), Quaternion.Euler(0f, 26f, -20f), 0.92f);
                AddClawArm(groups["arms_modules"], mesh, mat, "right_overheat_claw", new Vector3(0.44f, 1.78f, -0.08f), new Vector3(0.94f, 1.72f, -0.34f), Quaternion.Euler(0f, -26f, 20f), 0.92f);
                AddTellStripe(groups["readable_tells"], mat["Red"], "overheat_brow_bar", new Vector3(0f, 2.42f, -0.34f), new Vector3(0.46f, 0.04f, 0.055f));
                AddPressureTank(groups["boiler_tanks"], mesh, mat, "enrage_left_aux_tank", new Vector3(-0.46f, 1.52f, 0.35f), 0.14f, 0.56f);
                AddPressureTank(groups["boiler_tanks"], mesh, mat, "enrage_right_aux_tank", new Vector3(0.46f, 1.52f, 0.35f), 0.14f, 0.56f);
            }

            AddPressurePipe(groups["pressure_lines"], mat["Patina"], "warden_left_pressure_line", new Vector3(-0.25f, 1.72f, 0.22f), new Vector3(-0.66f, 1.50f, -0.22f), 0.030f);
            AddPressurePipe(groups["pressure_lines"], mat["Patina"], "warden_right_pressure_line", new Vector3(0.25f, 1.72f, 0.22f), new Vector3(0.66f, 1.50f, -0.22f), 0.030f);
            AddGauge(groups["rivets_gauges"], mesh, mat, "warden_core_gauge", new Vector3(-0.30f, 1.56f, -0.35f), Quaternion.Euler(0f, -8f, 0f));
            AddRivetRow(groups["rivets_gauges"], mat["Brass"], "warden_spine_rivets", 8, new Vector3(0f, 0.82f, -0.18f), new Vector3(0f, 1.18f, 0f));
        }

        private static void AddFutureRigSockets(CandidateSpec candidate, Transform parent, Material markerMaterial)
        {
            AddSocket(parent, "SOCKET_root_pelvis", new Vector3(0f, 0.72f, 0f));
            AddSocket(parent, "SOCKET_core_furnace", new Vector3(0f, candidate.Height * 0.55f, -0.34f));
            AddSocket(parent, "SOCKET_head_pan", new Vector3(0f, candidate.Height * 0.82f, -0.10f));
            AddSocket(parent, "SOCKET_left_arm", new Vector3(-candidate.Width * 0.38f, candidate.Height * 0.58f, -0.12f));
            AddSocket(parent, "SOCKET_right_arm", new Vector3(candidate.Width * 0.38f, candidate.Height * 0.58f, -0.12f));
            AddSocket(parent, "SOCKET_weapon_mount", new Vector3(0f, candidate.Height * 0.56f, -candidate.Depth * 0.54f));
            AddSocket(parent, "SOCKET_back_tank", new Vector3(0f, candidate.Height * 0.55f, candidate.Depth * 0.34f));
            AddSocket(parent, "SOCKET_fx_left_eye", new Vector3(-0.10f, candidate.Height * 0.83f, -candidate.Depth * 0.35f));
            AddSocket(parent, "SOCKET_fx_right_eye", new Vector3(0.10f, candidate.Height * 0.83f, -candidate.Depth * 0.35f));
            AddSocket(parent, "SOCKET_fx_muzzle_or_cast", new Vector3(0f, candidate.Height * 0.58f, -candidate.Depth * 0.72f));
            AddSocket(parent, "SOCKET_ground_shadow", new Vector3(0f, 0.04f, 0f));

            CreateEmpty("NOTE_future_rig_socket_positions_are_visual_placeholders", parent);
            CreateEmpty("NOTE_no_hitboxes_no_damage_no_movement_authority", parent);
        }

        private static void AddSocket(Transform parent, string name, Vector3 position)
        {
            var socket = CreateEmpty(name, parent);
            socket.transform.localPosition = position;
        }

        private static void AddScaleEnvelope(CandidateSpec candidate, Transform parent, Material ghost)
        {
            CreatePrimitivePart("readability_envelope_" + MakeSafeFileName(candidate.Variant), PrimitiveType.Cube, parent, new Vector3(0f, candidate.Height * 0.50f, 0.08f), new Vector3(candidate.Width, candidate.Height, 0.035f), Quaternion.identity, ghost);
            CreateEmpty("NOTE_envelope_marks_expected_future_rig_bounds_" + candidate.Height.ToString("0.00") + "m", parent);
        }

        private static void AddSawArm(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 basePosition, Vector3 bladePosition, Quaternion bladeRotation, float scale)
        {
            AddPressurePipe(parent, mat["DarkSteel"], name + "_piston_upper", basePosition, Vector3.Lerp(basePosition, bladePosition, 0.65f), 0.045f);
            AddPressurePipe(parent, mat["Copper"], name + "_thin_hose", basePosition + new Vector3(0f, 0.06f, 0.03f), bladePosition + new Vector3(0f, 0.02f, 0.03f), 0.020f);
            CreateMeshPart(name + "_36tooth_saw_module", mesh["SawBlade"], parent, bladePosition, Vector3.one * scale, bladeRotation, mat["Hazard"]);
            CreatePrimitivePart(name + "_black_guard", PrimitiveType.Cube, parent, bladePosition + new Vector3(0f, 0.06f, 0.05f), new Vector3(0.38f * scale, 0.08f, 0.16f), Quaternion.identity, mat["BlackIron"]);
        }

        private static void AddClawArm(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 basePosition, Vector3 palmPosition, Quaternion palmRotation, float scale)
        {
            AddPressurePipe(parent, mat["DarkSteel"], name + "_forearm_piston", basePosition, palmPosition, 0.042f);
            var palm = CreatePrimitivePart(name + "_palm_actuator", PrimitiveType.Cube, parent, palmPosition, new Vector3(0.18f, 0.16f, 0.16f) * scale, palmRotation, mat["BlackIron"]);
            for (var i = 0; i < 3; i++)
            {
                var offset = new Vector3((i - 1) * 0.085f * scale, -0.02f * scale, -0.13f * scale);
                var talonRotation = palmRotation * Quaternion.Euler(18f + i * 6f, 0f, (i - 1) * 12f);
                CreateMeshPart(name + "_talon_" + i, mesh["ClawTalon"], parent, palm.transform.localPosition + offset, Vector3.one * scale, talonRotation, mat["DarkSteel"]);
            }
        }

        private static void AddHammerArm(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 basePosition, Vector3 headPosition, Quaternion headRotation, float scale)
        {
            AddPressurePipe(parent, mat["DarkSteel"], name + "_hammer_handle", basePosition, headPosition, 0.055f * scale);
            CreateMeshPart(name + "_rivet_hammer_head", mesh["HammerHead"], parent, headPosition, Vector3.one * scale, headRotation, mat["DarkSteel"]);
            AddRivetRow(parent, mat["Brass"], name + "_hammer_face_rivets", 3, headPosition + new Vector3(-0.14f * scale, 0.14f * scale, -0.12f), new Vector3(0.28f * scale, 0f, 0f));
        }

        private static void AddLanceArm(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 basePosition, Vector3 tipPosition, Quaternion tipRotation, float scale)
        {
            AddPressurePipe(parent, mat["DarkSteel"], name + "_shaft", basePosition, tipPosition + new Vector3(0f, 0f, 0.20f * scale), 0.040f * scale);
            CreateMeshPart(name + "_needle_tip", mesh["LanceTip"], parent, tipPosition, Vector3.one * scale, tipRotation, mat["DarkSteel"]);
            CreatePrimitivePart(name + "_cyan_charge_sleeve", PrimitiveType.Cylinder, parent, Vector3.Lerp(basePosition, tipPosition, 0.58f), new Vector3(0.07f * scale, 0.09f * scale, 0.07f * scale), Quaternion.Euler(90f, 0f, 0f), mat["Cyan"]);
        }

        private static void AddDrillArm(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 basePosition, Vector3 bitPosition, Quaternion bitRotation, float scale)
        {
            AddPressurePipe(parent, mat["DarkSteel"], name + "_drill_shaft", basePosition, bitPosition + new Vector3(0f, 0f, 0.20f * scale), 0.045f * scale);
            CreateMeshPart(name + "_spiral_drill_bit", mesh["DrillBit"], parent, bitPosition, Vector3.one * scale, bitRotation, mat["DarkSteel"]);
            AddWarningLamp(parent, mat["Cyan"], name + "_cyan_charge_bearing", Vector3.Lerp(basePosition, bitPosition, 0.66f), 0.040f * scale);
        }

        private static void AddShield(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 position, Quaternion rotation, float scale)
        {
            CreateMeshPart(name + "_shield_plate", mesh["ShieldPlate"], parent, position, Vector3.one * scale, rotation, mat["DarkSteel"]);
            CreatePrimitivePart(name + "_brass_crossbar", PrimitiveType.Cube, parent, position + new Vector3(0f, 0.02f, -0.06f), new Vector3(0.58f * scale, 0.08f * scale, 0.06f * scale), rotation, mat["Brass"]);
            CreatePrimitivePart(name + "_hazard_lower_stripe", PrimitiveType.Cube, parent, position + new Vector3(0f, -0.28f * scale, -0.07f), new Vector3(0.48f * scale, 0.055f * scale, 0.06f * scale), rotation, mat["Hazard"]);
        }

        private static void AddFurnaceCore(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 position, float scale)
        {
            CreateMeshPart(name + "_grate", mesh["FurnaceGrate"], parent, position, Vector3.one * scale, Quaternion.identity, mat["BlackIron"]);
            CreatePrimitivePart(name + "_hot_vertical_slit_left", PrimitiveType.Cube, parent, position + new Vector3(-0.10f * scale, 0.00f, -0.035f), new Vector3(0.040f * scale, 0.28f * scale, 0.025f), Quaternion.identity, mat["HotSlot"]);
            CreatePrimitivePart(name + "_hot_vertical_slit_right", PrimitiveType.Cube, parent, position + new Vector3(0.10f * scale, 0.00f, -0.035f), new Vector3(0.040f * scale, 0.28f * scale, 0.025f), Quaternion.identity, mat["HotSlot"]);
            AddWarningLamp(parent, mat["FurnaceEye"], name + "_furnace_eye", position + new Vector3(0f, 0.17f * scale, -0.055f), 0.055f * scale);
        }

        private static void AddBellBeacon(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 position, float scale)
        {
            CreatePrimitivePart(name + "_bell_cup", PrimitiveType.Cylinder, parent, position, new Vector3(0.30f * scale, 0.18f * scale, 0.30f * scale), Quaternion.identity, mat["Brass"]);
            CreateMeshPart(name + "_valve_wheel_crown", mesh["ValveWheel"], parent, position + new Vector3(0f, 0.22f * scale, -0.02f), Vector3.one * scale, Quaternion.identity, mat["Cyan"]);
            AddWarningLamp(parent, mat["Cyan"], name + "_beacon_lens", position + new Vector3(0f, 0.04f * scale, -0.27f * scale), 0.065f * scale);
        }

        private static void AddPressureTank(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 position, float radius, float height)
        {
            CreatePrimitivePart(name + "_cylinder", PrimitiveType.Cylinder, parent, position, new Vector3(radius * 2f, height * 0.50f, radius * 2f), Quaternion.identity, mat["Brass"]);
            AddTankCap(parent, mesh, mat, name + "_top_cap", position + new Vector3(0f, height * 0.52f, 0f), radius / 0.18f);
            AddTankCap(parent, mesh, mat, name + "_bottom_cap", position + new Vector3(0f, -height * 0.52f, 0f), radius / 0.18f);
            CreatePrimitivePart(name + "_black_band", PrimitiveType.Cylinder, parent, position, new Vector3(radius * 2.08f, 0.020f, radius * 2.08f), Quaternion.identity, mat["BlackIron"]);
        }

        private static void AddTankCap(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 position, float scale)
        {
            CreateMeshPart(name, mesh["TankCap"], parent, position, Vector3.one * scale, Quaternion.Euler(90f, 0f, 0f), mat["Copper"]);
        }

        private static void AddGauge(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string name, Vector3 position, Quaternion rotation)
        {
            CreateMeshPart(name + "_face", mesh["GaugeFace"], parent, position, Vector3.one, rotation, mat["Gauge"]);
            CreatePrimitivePart(name + "_needle", PrimitiveType.Cube, parent, position + new Vector3(0f, 0.03f, -0.03f), new Vector3(0.025f, 0.14f, 0.018f), rotation * Quaternion.Euler(0f, 0f, -24f), mat["Red"]);
        }

        private static void CreateLensPair(Transform parent, Vector3 center, float halfWidth, float radius, Dictionary<string, Material> mat)
        {
            AddWarningLamp(parent, mat["FurnaceEye"], "left_emissive_furnace_eye", center + new Vector3(-halfWidth, 0f, 0f), radius);
            AddWarningLamp(parent, mat["FurnaceEye"], "right_emissive_furnace_eye", center + new Vector3(halfWidth, 0f, 0f), radius);
            CreatePrimitivePart("grimy_glass_lens_left", PrimitiveType.Sphere, parent, center + new Vector3(-halfWidth, 0f, -0.010f), Vector3.one * radius * 2.25f, Quaternion.identity, mat["Glass"]);
            CreatePrimitivePart("grimy_glass_lens_right", PrimitiveType.Sphere, parent, center + new Vector3(halfWidth, 0f, -0.010f), Vector3.one * radius * 2.25f, Quaternion.identity, mat["Glass"]);
        }

        private static void AddWarningLamp(Transform parent, Material material, string name, Vector3 position, float radius)
        {
            CreatePrimitivePart(name, PrimitiveType.Sphere, parent, position, Vector3.one * radius * 2f, Quaternion.identity, material);
        }

        private static void AddTellStripe(Transform parent, Material material, string name, Vector3 position, Vector3 scale)
        {
            CreatePrimitivePart(name, PrimitiveType.Cube, parent, position, scale, Quaternion.identity, material);
        }

        private static void AddGhostArc(Transform parent, Material material, string name, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            CreatePrimitivePart(name, PrimitiveType.Cube, parent, position, scale, rotation, material);
        }

        private static void AddLeg(Transform parent, Dictionary<string, Material> mat, string name, Vector3 hip, Vector3 foot)
        {
            AddPressurePipe(parent, mat["DarkSteel"], name + "_piston", hip, foot + new Vector3(0f, 0.08f, 0f), 0.042f);
            CreatePrimitivePart(name + "_claw_foot", PrimitiveType.Cube, parent, foot, new Vector3(0.22f, 0.08f, 0.30f), Quaternion.identity, mat["BlackIron"]);
        }

        private static void AddPressurePipe(Transform parent, Material material, string name, Vector3 start, Vector3 end, float radius)
        {
            var midpoint = (start + end) * 0.5f;
            var direction = end - start;
            var length = Mathf.Max(0.001f, direction.magnitude);
            var rotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
            CreatePrimitivePart(name, PrimitiveType.Cylinder, parent, midpoint, new Vector3(radius * 2f, length * 0.5f, radius * 2f), rotation, material);
        }

        private static void AddRivetRow(Transform parent, Material material, string name, int count, Vector3 start, Vector3 span)
        {
            for (var i = 0; i < count; i++)
            {
                var t = count <= 1 ? 0f : i / (float)(count - 1);
                CreatePrimitivePart(name + "_" + i.ToString("00"), PrimitiveType.Sphere, parent, start + span * t, Vector3.one * 0.055f, Quaternion.identity, material);
            }
        }

        private static GameObject CreatePrimitivePart(string name, PrimitiveType primitiveType, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, Material material)
        {
            var part = GameObject.CreatePrimitive(primitiveType);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = position;
            part.transform.localScale = scale;
            part.transform.localRotation = rotation;

            var collider = part.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = part.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }

            return part;
        }

        private static GameObject CreateMeshPart(string name, Mesh mesh, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, Material material)
        {
            var part = new GameObject(name);
            part.transform.SetParent(parent, false);
            part.transform.localPosition = position;
            part.transform.localScale = scale;
            part.transform.localRotation = rotation;
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

        private static Mesh CreateTalonMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(-0.035f, -0.08f, 0.02f), new Vector3(0.035f, -0.08f, 0.02f), new Vector3(0.025f, 0.08f, 0.02f), new Vector3(-0.025f, 0.08f, 0.02f),
                new Vector3(-0.030f, -0.08f, -0.02f), new Vector3(0.030f, -0.08f, -0.02f), new Vector3(0f, 0.24f, -0.04f)
            };
            var triangles = new[]
            {
                0,1,2, 0,2,3,
                4,6,5,
                0,4,5, 0,5,1,
                1,5,6, 1,6,2,
                2,6,3,
                3,6,4, 3,4,0
            };
            return BuildMesh(name, vertices, triangles);
        }

        private static Mesh CreateConeMesh(string name, float radius, float length, int segments)
        {
            var vertices = new List<Vector3>
            {
                new Vector3(0f, 0f, -length * 0.5f),
                new Vector3(0f, 0f, length * 0.5f)
            };
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

        private static Mesh CreateDrillBitMesh(string name, float radius, float length, int segments)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i <= segments; i++)
            {
                var z = Mathf.Lerp(-length * 0.5f, length * 0.5f, i / (float)segments);
                var twist = i * Mathf.PI * 0.62f;
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

        private static Mesh CreateShieldPlateMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(-0.48f, -0.68f, -0.045f), new Vector3(0.48f, -0.68f, -0.045f), new Vector3(0.58f, 0.12f, -0.045f), new Vector3(0.32f, 0.72f, -0.045f), new Vector3(-0.32f, 0.72f, -0.045f), new Vector3(-0.58f, 0.12f, -0.045f),
                new Vector3(-0.48f, -0.68f, 0.045f), new Vector3(0.48f, -0.68f, 0.045f), new Vector3(0.58f, 0.12f, 0.045f), new Vector3(0.32f, 0.72f, 0.045f), new Vector3(-0.32f, 0.72f, 0.045f), new Vector3(-0.58f, 0.12f, 0.045f)
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

        private static Mesh CreateGrateMesh(string name)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            AddBox(vertices, triangles, new Vector3(0f, 0f, 0f), new Vector3(0.42f, 0.54f, 0.045f));
            for (var i = 0; i < 3; i++)
            {
                AddBox(vertices, triangles, new Vector3(-0.14f + i * 0.14f, 0f, -0.035f), new Vector3(0.035f, 0.48f, 0.035f));
            }

            return BuildMesh(name, vertices.ToArray(), triangles.ToArray());
        }

        private static Mesh CreateFinMesh(string name)
        {
            var vertices = new[]
            {
                new Vector3(0f, -0.16f, -0.075f), new Vector3(0.58f, 0.02f, -0.075f), new Vector3(0.12f, 0.46f, -0.075f),
                new Vector3(0f, -0.16f, 0.075f), new Vector3(0.58f, 0.02f, 0.075f), new Vector3(0.12f, 0.46f, 0.075f)
            };
            var triangles = new[] { 0, 1, 2, 3, 5, 4, 0, 3, 4, 0, 4, 1, 1, 4, 5, 1, 5, 2, 2, 5, 3, 2, 3, 0 };
            return BuildMesh(name, vertices, triangles);
        }

        private static Mesh CreateBoxMesh(string name, Vector3 size)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            AddBox(vertices, triangles, Vector3.zero, size);
            return BuildMesh(name, vertices.ToArray(), triangles.ToArray());
        }

        private static void AddBox(List<Vector3> vertices, List<int> triangles, Vector3 center, Vector3 size)
        {
            var start = vertices.Count;
            var half = size * 0.5f;
            vertices.Add(center + new Vector3(-half.x, -half.y, -half.z));
            vertices.Add(center + new Vector3(half.x, -half.y, -half.z));
            vertices.Add(center + new Vector3(half.x, half.y, -half.z));
            vertices.Add(center + new Vector3(-half.x, half.y, -half.z));
            vertices.Add(center + new Vector3(-half.x, -half.y, half.z));
            vertices.Add(center + new Vector3(half.x, -half.y, half.z));
            vertices.Add(center + new Vector3(half.x, half.y, half.z));
            vertices.Add(center + new Vector3(-half.x, half.y, half.z));
            var local = new[]
            {
                0,2,1, 0,3,2,
                4,5,6, 4,6,7,
                0,1,5, 0,5,4,
                1,2,6, 1,6,5,
                2,3,7, 2,7,6,
                3,0,4, 3,4,7
            };
            foreach (var index in local)
            {
                triangles.Add(start + index);
            }
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

        private static void RenderContactSheet(string outputRoot, string fileName, CandidateSpec[] candidates, int columns, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreatePreviewEnvironment();

            var roots = new List<GameObject>();
            var rows = Mathf.CeilToInt(candidates.Length / (float)columns);
            for (var i = 0; i < candidates.Length; i++)
            {
                var prefabPath = PrefabRoot + "/" + candidates[i].Id + ".prefab";
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    continue;
                }

                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                var col = i % columns;
                var row = i / columns;
                instance.transform.position = new Vector3((col - (columns - 1) * 0.5f) * 2.65f, 0f, row * 2.35f);
                instance.transform.rotation = Quaternion.Euler(0f, -10f + col * 7f, 0f);
                roots.Add(instance);
            }

            var camera = CreatePreviewCamera(36f);
            var bounds = CalculateBounds(roots);
            bounds.Expand(new Vector3(0.80f, 0.35f, rows * 0.40f));
            FrameBounds(camera, bounds, 1.18f, width / (float)height);
            RenderCameraToPng(camera, Path.Combine(outputRoot, fileName), width, height);
        }

        private static void RenderSingleCandidate(string outputRoot, CandidateSpec candidate, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreatePreviewEnvironment();

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + candidate.Id + ".prefab");
            if (prefab == null)
            {
                throw new InvalidOperationException("Missing generated prefab for preview: " + candidate.Id);
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(0f, -12f, 0f);

            var camera = CreatePreviewCamera(34f);
            var bounds = CalculateBounds(new[] { instance });
            FrameBounds(camera, bounds, 1.24f, width / (float)height);
            RenderCameraToPng(camera, Path.Combine(outputRoot, candidate.Id + "_v0.1.41.png"), width, height);
        }

        private static void CreatePreviewEnvironment()
        {
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.18f, 0.15f, 0.12f, 1f);

            var floorMaterial = new Material(FindLitShader());
            SetMaterialColor(floorMaterial, "_BaseColor", new Color(0.070f, 0.064f, 0.055f, 1f));
            SetMaterialColor(floorMaterial, "_Color", new Color(0.070f, 0.064f, 0.055f, 1f));

            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "preview_foundry_floor";
            floor.transform.position = new Vector3(0f, -0.055f, 2.7f);
            floor.transform.localScale = new Vector3(16f, 0.10f, 12f);
            floor.GetComponent<MeshRenderer>().sharedMaterial = floorMaterial;
            UnityEngine.Object.DestroyImmediate(floor.GetComponent<Collider>());

            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = "preview_soot_wall";
            wall.transform.position = new Vector3(0f, 2.1f, 6.4f);
            wall.transform.localScale = new Vector3(16f, 4.3f, 0.12f);
            wall.GetComponent<MeshRenderer>().sharedMaterial = floorMaterial;
            UnityEngine.Object.DestroyImmediate(wall.GetComponent<Collider>());

            var key = new GameObject("preview_warm_key_light").AddComponent<Light>();
            key.type = LightType.Spot;
            key.color = new Color(1f, 0.68f, 0.36f, 1f);
            key.intensity = 980f;
            key.range = 13f;
            key.spotAngle = 58f;
            key.transform.position = new Vector3(-4.0f, 5.2f, -4.4f);
            key.transform.rotation = Quaternion.Euler(58f, 34f, 0f);

            var rim = new GameObject("preview_cyan_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = CyanPressure;
            rim.intensity = 4.2f;
            rim.range = 7f;
            rim.transform.position = new Vector3(3.7f, 2.4f, -2.9f);
        }

        private static Camera CreatePreviewCamera(float fieldOfView)
        {
            var cameraObject = new GameObject("preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.030f, 0.027f, 0.023f, 1f);
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 120f;
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
            camera.transform.position = bounds.center + new Vector3(0f, bounds.extents.y * 0.10f, -distance - bounds.extents.z - 0.65f);
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
            var catalogGuids = AssetDatabase.FindAssets("EE02_EncounterEnemySet02_Catalog", new[] { MetadataRoot });
            var outputRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            var pngCount = Directory.Exists(outputRoot) ? Directory.GetFiles(outputRoot, "*.png").Length : 0;
            var colliderCount = 0;
            var audioSourceCount = 0;
            var monoBehaviourCount = 0;
            var socketWarnings = 0;

            foreach (var guid in prefabGuids)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (prefab == null)
                {
                    continue;
                }

                colliderCount += prefab.GetComponentsInChildren<Collider>(true).Length;
                audioSourceCount += prefab.GetComponentsInChildren<AudioSource>(true).Length;
                monoBehaviourCount += prefab.GetComponentsInChildren<MonoBehaviour>(true).Length;
                var socketRoot = prefab.transform.Find("future_rig_sockets");
                if (socketRoot == null || socketRoot.Cast<Transform>().Count(child => child.name.StartsWith("SOCKET_", StringComparison.Ordinal)) < 10)
                {
                    socketWarnings++;
                }
            }

            var runtimeScriptCount = Directory.Exists(PhysicalPackageRelative("Runtime")) ? Directory.GetFiles(PhysicalPackageRelative("Runtime"), "*.cs", SearchOption.AllDirectories).Length : 0;
            var errors = 0;
            if (prefabGuids.Length != Candidates.Length) errors++;
            if (materialGuids.Length != ExpectedMaterialCount) errors++;
            if (meshGuids.Length != ExpectedMeshCount) errors++;
            if (catalogGuids.Length != 1) errors++;
            if (pngCount < ExpectedPreviewPngCount) errors++;
            if (colliderCount != 0) errors++;
            if (audioSourceCount != 0) errors++;
            if (monoBehaviourCount != 0) errors++;
            if (runtimeScriptCount != 0) errors++;
            if (socketWarnings != 0) errors++;

            var reportRoot = ResolveRepoRelativeFolder(ProductionDocFolder);
            Directory.CreateDirectory(reportRoot);
            var reportPath = Path.Combine(reportRoot, "unity_validation_report_v0.1.41.json");
            var json = "{\n" +
                "  \"status\": \"" + (errors == 0 ? "pass" : "fail") + "\",\n" +
                "  \"prefabs\": " + prefabGuids.Length + ",\n" +
                "  \"materials\": " + materialGuids.Length + ",\n" +
                "  \"meshes\": " + meshGuids.Length + ",\n" +
                "  \"runtime_catalogs\": " + catalogGuids.Length + ",\n" +
                "  \"preview_pngs\": " + pngCount + ",\n" +
                "  \"colliders_in_prefabs\": " + colliderCount + ",\n" +
                "  \"audio_sources_in_prefabs\": " + audioSourceCount + ",\n" +
                "  \"mono_behaviours_in_prefabs\": " + monoBehaviourCount + ",\n" +
                "  \"runtime_scripts\": " + runtimeScriptCount + ",\n" +
                "  \"socket_warnings\": " + socketWarnings + ",\n" +
                "  \"expected_prefabs\": " + Candidates.Length + ",\n" +
                "  \"expected_materials\": " + ExpectedMaterialCount + ",\n" +
                "  \"expected_meshes\": " + ExpectedMeshCount + ",\n" +
                "  \"expected_preview_pngs_minimum\": " + ExpectedPreviewPngCount + "\n" +
                "}\n";
            File.WriteAllText(reportPath, json);
            WritePackageManifest(errors == 0 ? "unity_generation_preview_and_validation_passed" : "unity_validation_failed", pngCount);
            Debug.Log("EE02_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL") + " report=" + reportPath);
            return errors;
        }

        private static void WriteRuntimeCatalog()
        {
            var path = MetadataRoot + "/EE02_EncounterEnemySet02_Catalog_v0.1.41.json";
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"EE02\",");
            builder.AppendLine("  \"display_name\": \"Encounter Enemy Set 02\",");
            builder.AppendLine("  \"version\": \"0.1.41-p001\",");
            builder.AppendLine("  \"runtime_safety\": {");
            builder.AppendLine("    \"visual_only\": true,");
            builder.AppendLine("    \"runtime_scripts\": 0,");
            builder.AppendLine("    \"colliders\": 0,");
            builder.AppendLine("    \"autonomous_audio\": false,");
            builder.AppendLine("    \"gameplay_authority\": false");
            builder.AppendLine("  },");
            builder.AppendLine("  \"future_rig_sockets\": [");
            builder.AppendLine("    \"SOCKET_root_pelvis\",");
            builder.AppendLine("    \"SOCKET_core_furnace\",");
            builder.AppendLine("    \"SOCKET_head_pan\",");
            builder.AppendLine("    \"SOCKET_left_arm\",");
            builder.AppendLine("    \"SOCKET_right_arm\",");
            builder.AppendLine("    \"SOCKET_weapon_mount\",");
            builder.AppendLine("    \"SOCKET_back_tank\",");
            builder.AppendLine("    \"SOCKET_fx_left_eye\",");
            builder.AppendLine("    \"SOCKET_fx_right_eye\",");
            builder.AppendLine("    \"SOCKET_fx_muzzle_or_cast\",");
            builder.AppendLine("    \"SOCKET_ground_shadow\"");
            builder.AppendLine("  ],");
            builder.AppendLine("  \"candidates\": [");
            for (var i = 0; i < Candidates.Length; i++)
            {
                var candidate = Candidates[i];
                builder.AppendLine("    {");
                builder.AppendLine("      \"id\": \"" + JsonEscape(candidate.Id) + "\",");
                builder.AppendLine("      \"family\": \"" + JsonEscape(candidate.Family) + "\",");
                builder.AppendLine("      \"variant\": \"" + JsonEscape(candidate.Variant) + "\",");
                builder.AppendLine("      \"pose\": \"" + JsonEscape(candidate.Pose) + "\",");
                builder.AppendLine("      \"weapon_module\": \"" + JsonEscape(candidate.WeaponModule) + "\",");
                builder.AppendLine("      \"attack_tell_note\": \"" + JsonEscape(candidate.AttackTell) + "\",");
                builder.AppendLine("      \"prefab\": \"" + PrefabRoot + "/" + candidate.Id + ".prefab\",");
                builder.AppendLine("      \"height_m\": " + candidate.Height.ToString("0.00") + ",");
                builder.AppendLine("      \"width_m\": " + candidate.Width.ToString("0.00") + ",");
                builder.AppendLine("      \"depth_m\": " + candidate.Depth.ToString("0.00"));
                builder.Append("    }");
                builder.AppendLine(i == Candidates.Length - 1 ? string.Empty : ",");
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            WriteTextAsset(path, builder.ToString());
        }

        private static void WritePackageManifest(string importStatus, int previewCount)
        {
            var physicalFolder = Path.Combine(PhysicalPackageRoot(), PackageManifestFolder.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(physicalFolder);
            var manifestPath = Path.Combine(physicalFolder, "EE02_EncounterEnemySet02_Manifest_v0.1.41-p001.json");
            var previewRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            var previewFiles = Directory.Exists(previewRoot) ? Directory.GetFiles(previewRoot, "*.png").Select(ToRepoPath).OrderBy(path => path).ToArray() : Array.Empty<string>();

            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"EE02\",");
            builder.AppendLine("  \"display_name\": \"Encounter Enemy Set 02\",");
            builder.AppendLine("  \"version\": \"0.1.41\",");
            builder.AppendLine("  \"build_id\": \"p001\",");
            builder.AppendLine("  \"unity_version\": \"6000.4.6f1\",");
            builder.AppendLine("  \"generated_at_utc\": \"" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",");
            builder.AppendLine("  \"sidecar_project\": \"UD-SC-EE02-EncounterEnemySet02\",");
            builder.AppendLine("  \"owner_lane\": \"sidecar-encounter-enemy-set02\",");
            builder.AppendLine("  \"primary_intake_owner\": \"main-lane-art-integration\",");
            builder.AppendLine("  \"canonical_root\": \"AssetPacks/BrassworksBreach.EncounterEnemySet02\",");
            builder.AppendLine("  \"package_root\": \"AssetPacks/BrassworksBreach.EncounterEnemySet02\",");
            builder.AppendLine("  \"package_name\": \"" + PackageName + "\",");
            builder.AppendLine("  \"package_version\": \"0.1.41-p001\",");
            builder.AppendLine("  \"asset_counts\": {");
            builder.AppendLine("    \"generated_prefabs\": " + Candidates.Length + ",");
            builder.AppendLine("    \"generated_materials\": " + ExpectedMaterialCount + ",");
            builder.AppendLine("    \"generated_meshes\": " + ExpectedMeshCount + ",");
            builder.AppendLine("    \"runtime_catalogs\": 1,");
            builder.AppendLine("    \"preview_renders\": " + previewCount + ",");
            builder.AppendLine("    \"runtime_scripts\": 0,");
            builder.AppendLine("    \"audio\": 0,");
            builder.AppendLine("    \"colliders\": 0");
            builder.AppendLine("  },");
            builder.AppendLine("  \"families\": [");
            AppendJsonArray(builder, Candidates.Select(candidate => candidate.Family).Distinct().ToArray(), "  ");
            builder.AppendLine("  ],");
            builder.AppendLine("  \"generated_prefabs\": [");
            AppendJsonArray(builder, Candidates.Select(candidate => PrefabRoot + "/" + candidate.Id + ".prefab").ToArray(), "  ");
            builder.AppendLine("  ],");
            builder.AppendLine("  \"preview_renders\": [");
            AppendJsonArray(builder, previewFiles, "  ");
            builder.AppendLine("  ],");
            builder.AppendLine("  \"dependencies\": [");
            AppendJsonArray(builder, new[] { "UnityEngine built-in primitives", "UnityEditor prefab and render texture APIs", "Built-in, URP, or HDRP lit shader fallback" }, "  ");
            builder.AppendLine("  ],");
            builder.AppendLine("  \"required_primary_changes\": [],");
            builder.AppendLine("  \"path_collisions_checked\": true,");
            builder.AppendLine("  \"guid_collisions_checked\": true,");
            builder.AppendLine("  \"import_smoke_status\": \"" + JsonEscape(importStatus) + "\",");
            builder.AppendLine("  \"runtime_safety\": {");
            builder.AppendLine("    \"visual_only\": true,");
            builder.AppendLine("    \"gameplay_authority\": false,");
            builder.AppendLine("    \"autonomous_audio\": false,");
            builder.AppendLine("    \"prefab_colliders\": 0");
            builder.AppendLine("  },");
            builder.AppendLine("  \"known_risks\": [");
            AppendJsonArray(builder, new[]
            {
                "Visual candidates are pose/readability lookdev assets, not final rigged or animated enemies.",
                "Procedural materials are package-local Unity proxies and may need final texture/material replacement during promotion.",
                "Socket transforms are future rigging placeholders and require animation, hit-proxy, VFX, and combat integration ownership in the primary lane.",
                "Preview lighting is evidence-only and should not be promoted into gameplay scenes."
            }, "  ");
            builder.AppendLine("  ],");
            builder.AppendLine("  \"rollback_path\": \"delete isolated package root or remove local package reference " + PackageName + "\",");
            builder.AppendLine("  \"decision\": \"ready_for_primary_quarantine_after_static_validation_and_preview_review\"");
            builder.AppendLine("}");

            File.WriteAllText(manifestPath, builder.ToString());
        }

        private static void AppendJsonArray(StringBuilder builder, string[] values, string indent)
        {
            for (var i = 0; i < values.Length; i++)
            {
                builder.Append(indent);
                builder.Append("  \"");
                builder.Append(JsonEscape(values[i]));
                builder.Append("\"");
                builder.AppendLine(i == values.Length - 1 ? string.Empty : ",");
            }
        }

        private static void WriteTextAsset(string assetPath, string content)
        {
            var physicalPath = Path.Combine(PhysicalPackageRoot(), assetPath.Substring(PackageRoot.Length).TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath));
            File.WriteAllText(physicalPath, content);
            AssetDatabase.ImportAsset(assetPath);
        }

        private static void WritePreviewPixelEvidence(string outputRoot)
        {
            var reportRoot = ResolveRepoRelativeFolder(ProductionDocFolder);
            Directory.CreateDirectory(reportRoot);
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"status\": \"pass\",");
            builder.AppendLine("  \"preview_root\": \"" + JsonEscape(ToRepoPath(outputRoot)) + "\",");
            builder.AppendLine("  \"files\": [");

            var files = Directory.GetFiles(outputRoot, "*.png").OrderBy(path => path).ToArray();
            for (var i = 0; i < files.Length; i++)
            {
                var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                var bytes = File.ReadAllBytes(files[i]);
                ImageConversion.LoadImage(texture, bytes);
                var unique = CountUniqueSampledPixels(texture);
                var nonFlat = unique >= 10;
                builder.AppendLine("    {");
                builder.AppendLine("      \"path\": \"" + JsonEscape(ToRepoPath(files[i])) + "\",");
                builder.AppendLine("      \"width\": " + texture.width + ",");
                builder.AppendLine("      \"height\": " + texture.height + ",");
                builder.AppendLine("      \"sampled_unique_pixels\": " + unique + ",");
                builder.AppendLine("      \"non_flat\": " + (nonFlat ? "true" : "false"));
                builder.Append("    }");
                builder.AppendLine(i == files.Length - 1 ? string.Empty : ",");
                UnityEngine.Object.DestroyImmediate(texture);
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(Path.Combine(reportRoot, "PreviewPixelEvidence_EncounterEnemySet02_v0.1.41.json"), builder.ToString());
        }

        private static int CountUniqueSampledPixels(Texture2D texture)
        {
            var colors = new HashSet<string>(StringComparer.Ordinal);
            var xSteps = 7;
            var ySteps = 7;
            for (var y = 0; y < ySteps; y++)
            {
                for (var x = 0; x < xSteps; x++)
                {
                    var px = Mathf.Clamp(Mathf.RoundToInt((x + 0.5f) * texture.width / xSteps), 0, texture.width - 1);
                    var py = Mathf.Clamp(Mathf.RoundToInt((y + 0.5f) * texture.height / ySteps), 0, texture.height - 1);
                    var color = texture.GetPixel(px, py);
                    colors.Add(Mathf.RoundToInt(color.r * 255f) + ":" + Mathf.RoundToInt(color.g * 255f) + ":" + Mathf.RoundToInt(color.b * 255f));
                }
            }

            return colors.Count;
        }

        private static void EnsurePackageFolders()
        {
            foreach (var relative in new[] { "Runtime/Prefabs", "Runtime/Materials", "Runtime/Meshes", "Runtime/Metadata", "Documentation~/Manifest" })
            {
                Directory.CreateDirectory(PhysicalPackageRelative(relative));
            }

            Directory.CreateDirectory(ResolveRepoRelativeFolder(RenderDocFolder));
            Directory.CreateDirectory(ResolveRepoRelativeFolder(ProductionDocFolder));
            AssetDatabase.Refresh();
        }

        private static string PhysicalPackageRoot()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly());
            if (packageInfo != null && !string.IsNullOrWhiteSpace(packageInfo.resolvedPath))
            {
                return packageInfo.resolvedPath;
            }

            return Path.Combine(Directory.GetCurrentDirectory(), "AssetPacks", "BrassworksBreach.EncounterEnemySet02");
        }

        private static string PhysicalPackageRelative(string relative)
        {
            return Path.Combine(PhysicalPackageRoot(), relative.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ResolveRepoRelativeFolder(string relativeFolder)
        {
            var packageRoot = PhysicalPackageRoot();
            var repoRoot = Path.GetFullPath(Path.Combine(packageRoot, "..", ".."));
            return Path.Combine(repoRoot, relativeFolder.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ToRepoPath(string absolutePath)
        {
            var repoRoot = Path.GetFullPath(Path.Combine(PhysicalPackageRoot(), "..", "..")).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var full = Path.GetFullPath(absolutePath);
            if (full.StartsWith(repoRoot, StringComparison.OrdinalIgnoreCase))
            {
                return full.Substring(repoRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace('\\', '/');
            }

            return full.Replace('\\', '/');
        }

        private static string MakeSafeFileName(string text)
        {
            return text.Replace(" ", "_").Replace("/", "_").Replace("\\", "_").Replace("-", "_").ToLowerInvariant();
        }

        private static string JsonEscape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private sealed class CandidateSpec
        {
            public CandidateSpec(string id, string family, string variant, string pose, int variantIndex, float height, float width, float depth, string weaponModule, string attackTell)
            {
                Id = id;
                Family = family;
                Variant = variant;
                Pose = pose;
                VariantIndex = variantIndex;
                Height = height;
                Width = width;
                Depth = depth;
                WeaponModule = weaponModule;
                AttackTell = attackTell;
            }

            public string Id { get; private set; }
            public string Family { get; private set; }
            public string Variant { get; private set; }
            public string Pose { get; private set; }
            public int VariantIndex { get; private set; }
            public float Height { get; private set; }
            public float Width { get; private set; }
            public float Depth { get; private set; }
            public string WeaponModule { get; private set; }
            public string AttackTell { get; private set; }
        }
    }
}
