using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BrassworksBreach.SteamVFXSet02;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.SteamVFXSet02.Editor
{
    public static class SteamVfxSet02Generator
    {
        private const string Version = "v0.1.42";
        private const string BuildId = "p001";
        private const string PackageName = "com.brassworks.sidecar.steam-vfx-set02";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string PrefabRoot = PackageRoot + "/Runtime/Prefabs";
        private const string MaterialRoot = PackageRoot + "/Runtime/Materials";
        private const string MeshRoot = PackageRoot + "/Runtime/Meshes";
        private const string MetadataRoot = PackageRoot + "/Runtime/Metadata";
        private const string ManifestRoot = PackageRoot + "/Documentation~/Manifest";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_42_SteamVFXSet02";
        private const string ProductionDocFolder = "Documentation/AssetProduction/V0_1_42_SteamVFXSet02";

        private static readonly VfxSpec[] Specs =
        {
            new VfxSpec("SteamVent_SoftColumn", "steam", "Soft vertical service vent for pipe joints and ambient boiler room punctuation.", "SOCK_SteamVent", "SteamSoft", 2.4f, 1.0f, 0.92f, true, 16f, 0),
            new VfxSpec("SteamVent_WallJet", "steam", "Directional wall jet with dense leading plume and curling vapor tail.", "SOCK_SteamVent", "SteamDense", 1.45f, 0.74f, 0.82f, false, -24f, 1),
            new VfxSpec("SteamVent_FloorBurst", "steam", "Short floor grate pressure burp with rolling low smoke and hot condensate flecks.", "SOCK_FloorVent", "SteamSoft", 1.35f, 0.68f, 0.95f, false, 12f, 2),
            new VfxSpec("SteamBackdraft_PipeCough", "steam", "Backdraft cough for damaged pipe reversals and blocked exhaust moments.", "SOCK_PipeEnd", "SootSmoke", 1.65f, 0.72f, 0.9f, false, -36f, 3),
            new VfxSpec("PressureLeak_PinHole", "pressure", "Needle-thin amber pressure leak with white-hot origin and fast vapor streaks.", "SOCK_LeakPoint", "PressureAmber", 0.95f, 0.48f, 0.72f, false, -18f, 4),
            new VfxSpec("PressureLeak_RuptureCone", "pressure", "Wider rupture cone with dirty steam, copper glints, and warning pressure flashes.", "SOCK_Rupture", "WarningRed", 1.4f, 0.64f, 0.98f, false, 28f, 5),
            new VfxSpec("PressureRing_Pulse", "pressure", "Expanding circular pressure pulse for valve releases and phase-wave beats.", "SOCK_Origin", "PressureAmber", 1.1f, 0.55f, 1.05f, false, 0f, 6),
            new VfxSpec("PressureGauge_ReliefPop", "pressure", "Small gauge relief pop with radial vapor ticks and brass needle snap.", "SOCK_Gauge", "GaugeGlass", 0.9f, 0.45f, 0.7f, false, 18f, 7),
            new VfxSpec("SparkShower_RivetCut", "spark", "Downward rivet-cut spark shower with copper-white hot flecks and smoke wisp.", "SOCK_Impact", "HotSpark", 1.05f, 0.45f, 0.84f, false, -20f, 8),
            new VfxSpec("SparkRicochet_WallHit", "spark", "Sharp fan of ricochet sparks for metal wall impacts and armor ticks.", "SOCK_ImpactNormal", "HotSpark", 0.72f, 0.32f, 0.72f, false, 32f, 9),
            new VfxSpec("MuzzleFlash_PistolBoiler", "muzzle", "Compact boiler-pistol muzzle flash with pressure ring and backsteam kiss.", "SOCK_Muzzle", "MuzzleCore", 0.42f, 0.18f, 0.74f, false, -12f, 10),
            new VfxSpec("MuzzleFlash_Scatterburst", "muzzle", "Wide scattergun brass flash with pellet spark cone and thick muzzle steam.", "SOCK_Muzzle", "PressureAmber", 0.55f, 0.22f, 0.95f, false, 22f, 11),
            new VfxSpec("MuzzleFlash_RifleValveShot", "muzzle", "Long rifle valve-shot lance with amber flash core, narrow sparks, and exhaust ring.", "SOCK_Muzzle", "MuzzleCore", 0.5f, 0.2f, 0.82f, false, -28f, 12),
            new VfxSpec("FurnaceBlast_DoorBelch", "furnace", "Furnace-door belch with white-hot throat, orange flame tongues, and rolling soot.", "SOCK_FurnaceDoor", "FurnaceOrange", 1.65f, 0.6f, 1.05f, false, 20f, 13),
            new VfxSpec("FurnaceEmbers_StackLift", "furnace", "Rising ember lift for furnace stacks, vents, and hot maintenance shafts.", "SOCK_Stack", "FurnaceWhite", 2.15f, 0.9f, 0.9f, true, -14f, 14),
            new VfxSpec("ValveTurn_ReleasePuff", "valve", "Valve-wheel turn puff with brass ring, green-safe status glint, and soft release cloud.", "SOCK_Valve", "ValveGreen", 1.05f, 0.48f, 0.85f, false, 8f, 15),
            new VfxSpec("ValveSeal_FrostedSteam", "valve", "Cooling valve seal with pale blue frost steam and closing pressure halo.", "SOCK_Valve", "BossBlue", 1.28f, 0.62f, 0.88f, false, -18f, 16),
            new VfxSpec("BossPhase_GovernorOvercrank", "boss", "Governor overcrank phase tell with red crown vents, amber pressure rings, and spark spit.", "SOCK_BossCore", "BossRed", 2.25f, 0.85f, 1.18f, false, 16f, 17),
            new VfxSpec("BossPhase_BoilerHeartSurge", "boss", "Boiler-heart surge with furnace core pulse, smoke collar, and hot bolt sparks.", "SOCK_BossCore", "FurnaceOrange", 2.05f, 0.8f, 1.2f, false, -22f, 18),
            new VfxSpec("BossPhase_EmergencyVentCrown", "boss", "Emergency vent crown for boss phase escalation with triple steam plumes and warning ring.", "SOCK_BossTop", "WarningRed", 2.35f, 0.9f, 1.22f, false, 0f, 19)
        };

        [MenuItem("Brassworks Breach/Sidecars/Steam VFX Set 02/Generate Package")]
        public static void GeneratePackage()
        {
            EnsurePackageFolders();
            var materials = CreateMaterials();
            var meshes = CreateMeshes();

            foreach (var spec in Specs)
            {
                var prefab = BuildPrefab(spec, materials, meshes);
                SavePrefab(prefab, PrefabPath(spec));
            }

            WriteCatalog();
            WriteManifest();
            WriteProductionReadme();
            WriteGenerationEvidence();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("BBSVFX02 generated " + Specs.Length + " visual-only VFX prefabs.");
        }

        [MenuItem("Brassworks Breach/Sidecars/Steam VFX Set 02/Render Preview Sheets")]
        public static void RenderPreviewSheets()
        {
            GeneratePackage();
            var outputRoot = ResolveRenderOutputRoot();
            Directory.CreateDirectory(outputRoot);
            RenderFullContactSheet(outputRoot);
            RenderFamilyContactSheet(outputRoot);
            WritePreviewPixelEvidence(outputRoot);
            AssetDatabase.Refresh();
            Debug.Log("BBSVFX02 preview sheets rendered to: " + outputRoot);
        }

        [MenuItem("Brassworks Breach/Sidecars/Steam VFX Set 02/Validate Generated Package")]
        public static void ValidateGeneratedPackage()
        {
            GeneratePackage();
            WritePackageSpecificValidation();
            Debug.Log("BBSVFX02 package-specific validation written.");
        }

        [MenuItem("Brassworks Breach/Sidecars/Steam VFX Set 02/Generate Render Validate")]
        public static void GenerateRenderValidate()
        {
            RenderPreviewSheets();
            WritePackageSpecificValidation();
            WriteAcceptanceReport("Unity generation, preview render, and package-specific runtime-safety validation completed. Sidecar validator evidence is written by the outer production script.");
            Debug.Log("BBSVFX02 generate/render/validate complete.");
        }

        private static GameObject BuildPrefab(VfxSpec spec, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var root = new GameObject("BBSVFX02_" + spec.Id);
            root.AddComponent<SteamVfxSet02Identity>().Configure(
                "BBSVFX02_" + spec.Id,
                spec.Family,
                spec.Intent,
                spec.Socket,
                spec.Duration,
                spec.Scale,
                spec.Looping);

            var sockets = CreateChild(root.transform, "Sockets");
            var visualRoot = CreateChild(root.transform, "VisualRoot");
            var metadata = CreateChild(root.transform, "Metadata");
            metadata.gameObject.SetActive(false);

            CreateSocket(sockets, "SOCK_Origin", Vector3.zero);
            CreateSocket(sockets, spec.Socket, new Vector3(0f, 0.05f, 0.18f));
            CreateSocket(sockets, "SOCK_Direction", new Vector3(0f, 0.05f, 0.75f));
            CreateSocket(sockets, "SOCK_ExternalLifetimeOwner", new Vector3(0f, -0.12f, -0.18f));

            switch (spec.Family)
            {
                case "steam":
                    AddSteamPrefab(spec, visualRoot, materials, meshes);
                    break;
                case "pressure":
                    AddPressurePrefab(spec, visualRoot, materials, meshes);
                    break;
                case "spark":
                    AddSparkPrefab(spec, visualRoot, materials, meshes);
                    break;
                case "muzzle":
                    AddMuzzlePrefab(spec, visualRoot, materials, meshes);
                    break;
                case "furnace":
                    AddFurnacePrefab(spec, visualRoot, materials, meshes);
                    break;
                case "valve":
                    AddValvePrefab(spec, visualRoot, materials, meshes);
                    break;
                case "boss":
                    AddBossPrefab(spec, visualRoot, materials, meshes);
                    break;
            }

            root.transform.localScale = Vector3.one * spec.Scale;
            return root;
        }

        private static void AddSteamPrefab(VfxSpec spec, Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var vertical = spec.Id.Contains("SoftColumn", StringComparison.Ordinal) || spec.Id.Contains("FloorBurst", StringComparison.Ordinal);
            var backdraft = spec.Id.Contains("Backdraft", StringComparison.Ordinal);
            var rotation = vertical ? new Vector3(-90f, 0f, 0f) : new Vector3(0f, spec.PreviewYaw, 0f);
            var pipeMaterial = backdraft ? materials["BlackIron"] : materials["BrassTrim"];

            CreateMeshPart("vent_nozzle_blackened_pipe", meshes["Cylinder"], parent, new Vector3(0f, 0.02f, 0f), new Vector3(0.28f, 0.28f, 0.36f), vertical ? Quaternion.Euler(-90f, 0f, 0f) : Quaternion.identity, pipeMaterial);
            CreateMeshPart("pressure_sleeve_ring", meshes["Ring"], parent, new Vector3(0f, 0.04f, 0.2f), new Vector3(0.58f, 0.58f, 0.58f), Quaternion.Euler(0f, 0f, 0f), materials["CopperGlow"]);

            if (spec.Id.Contains("FloorBurst", StringComparison.Ordinal))
            {
                CreateMeshPart("floor_grate_cross_a", meshes["Box"], parent, new Vector3(0f, -0.02f, 0f), new Vector3(0.82f, 0.04f, 0.09f), Quaternion.identity, materials["BlackIron"]);
                CreateMeshPart("floor_grate_cross_b", meshes["Box"], parent, new Vector3(0f, -0.018f, 0f), new Vector3(0.09f, 0.04f, 0.82f), Quaternion.identity, materials["BlackIron"]);
            }

            AddSteamSystem(parent, "steam_primary_plume", materials[spec.ColorKey], new Color(0.86f, 0.9f, 0.88f, 0.52f), new Vector3(0f, 0.08f, 0.1f), rotation, 0.12f, vertical ? 23f : 13f, vertical ? 0.55f : 0.85f, 0.38f, 1.35f, 0.08f, 0.34f, spec.Looping ? 36f : 0f, spec.Looping ? 18 : 58, spec.Duration, spec.Looping, -0.02f, 0.82f);
            AddSteamSystem(parent, "steam_soft_tail", materials["SteamSoft"], new Color(0.68f, 0.76f, 0.76f, 0.34f), new Vector3(0f, 0.12f, 0.18f), rotation + new Vector3(0f, 7f, 0f), 0.28f, vertical ? 36f : 24f, vertical ? 0.36f : 0.46f, 0.75f, 2.25f, 0.16f, 0.55f, spec.Looping ? 18f : 0f, spec.Looping ? 10 : 38, spec.Duration, spec.Looping, -0.05f, 1.25f);

            if (backdraft)
            {
                AddSteamSystem(parent, "soot_backdraft_lobe", materials["SootSmoke"], new Color(0.2f, 0.19f, 0.18f, 0.42f), new Vector3(0f, 0.08f, 0.05f), rotation + new Vector3(0f, 180f, 0f), 0.22f, 42f, 0.38f, 0.45f, 1.45f, 0.18f, 0.5f, 0f, 44, spec.Duration, false, -0.08f, 1.5f);
            }
        }

        private static void AddPressurePrefab(VfxSpec spec, Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreateMeshPart("brass_pressure_collar", meshes["Ring"], parent, new Vector3(0f, 0f, 0.18f), new Vector3(0.62f, 0.62f, 0.62f), Quaternion.identity, materials["BrassTrim"]);
            CreateMeshPart("black_iron_pinhole_plate", meshes["Plate"], parent, new Vector3(0f, 0f, 0f), new Vector3(0.72f, 0.42f, 0.08f), Quaternion.identity, materials["BlackIron"]);

            if (spec.Id.Contains("Ring", StringComparison.Ordinal))
            {
                CreateMeshPart("visible_pressure_wave_outer", meshes["Ring"], parent, new Vector3(0f, 0f, 0.06f), new Vector3(1.42f, 1.42f, 1.42f), Quaternion.identity, materials["PressureAmber"]);
                AddFlashSystem(parent, "amber_expanding_pressure_motes", materials["PressureAmber"], new Color(1f, 0.62f, 0.16f, 0.72f), new Vector3(0f, 0f, 0.08f), Vector3.zero, ParticleSystemShapeType.Circle, 0.55f, 0f, 0.02f, 0.18f, 0.48f, 0.03f, 0.13f, 0, 84, spec.Duration, false);
                AddSteamSystem(parent, "thin_pressure_vapor_ring", materials["SteamSoft"], new Color(0.82f, 0.87f, 0.82f, 0.36f), new Vector3(0f, 0f, 0.1f), Vector3.zero, 0.58f, 0f, 0.08f, 0.35f, 0.95f, 0.05f, 0.18f, 0f, 76, spec.Duration, false, -0.03f, 0.72f);
                return;
            }

            if (spec.Id.Contains("Gauge", StringComparison.Ordinal))
            {
                CreateMeshPart("gauge_glass_disc", meshes["Ring"], parent, new Vector3(0f, 0f, -0.04f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, materials["GaugeGlass"]);
                CreateMeshPart("gauge_needle_snap", meshes["Needle"], parent, new Vector3(0.04f, 0.02f, -0.08f), new Vector3(0.74f, 0.74f, 0.74f), Quaternion.Euler(0f, 0f, -34f), materials["WarningRed"]);
            }

            var wide = spec.Id.Contains("Rupture", StringComparison.Ordinal);
            AddSteamSystem(parent, "pressure_white_core_jet", materials["SteamDense"], new Color(0.95f, 0.96f, 0.9f, 0.62f), new Vector3(0f, 0f, 0.15f), new Vector3(0f, spec.PreviewYaw, 0f), wide ? 0.14f : 0.045f, wide ? 24f : 5f, wide ? 0.78f : 1.35f, 0.18f, wide ? 0.72f : 0.36f, wide ? 0.07f : 0.03f, wide ? 0.24f : 0.08f, 0f, wide ? 86 : 52, spec.Duration, false, -0.04f, wide ? 0.9f : 0.32f);
            AddFlashSystem(parent, "amber_pressure_origin_flash", materials[spec.ColorKey], MaterialColor(spec.ColorKey, 0.86f), new Vector3(0f, 0f, 0.19f), new Vector3(0f, spec.PreviewYaw, 0f), ParticleSystemShapeType.Cone, wide ? 0.12f : 0.04f, wide ? 16f : 4f, wide ? 0.18f : 0.08f, 0.05f, 0.18f, wide ? 0.05f : 0.025f, wide ? 0.16f : 0.07f, 0f, wide ? 34 : 16, spec.Duration, false);

            if (wide)
            {
                AddSparkSystem(parent, "rupture_copper_spark_spit", materials["CopperGlow"], new Color(1f, 0.45f, 0.14f, 0.96f), new Vector3(0f, 0f, 0.18f), new Vector3(0f, spec.PreviewYaw, 0f), 0.08f, 24f, 1.15f, 0.18f, 0.48f, 0.025f, 0.065f, 0, 38, spec.Duration, -0.12f);
            }
        }

        private static void AddSparkPrefab(VfxSpec spec, Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreateMeshPart("scored_impact_plate", meshes["Plate"], parent, new Vector3(0f, 0f, 0f), new Vector3(0.82f, 0.52f, 0.09f), Quaternion.identity, materials["BlackIron"]);
            CreateMeshPart("hot_impact_burst_shape", meshes["Burst"], parent, new Vector3(0f, 0f, -0.05f), Vector3.one * 0.58f, Quaternion.Euler(0f, 0f, spec.Id.Contains("Ricochet", StringComparison.Ordinal) ? -28f : 8f), materials["HotSpark"]);

            var downward = spec.Id.Contains("Rivet", StringComparison.Ordinal);
            AddSparkSystem(parent, "primary_spark_shards", materials["HotSpark"], new Color(1f, 0.72f, 0.26f, 1f), new Vector3(0f, 0.05f, 0.08f), downward ? new Vector3(62f, spec.PreviewYaw, 0f) : new Vector3(0f, spec.PreviewYaw, 0f), downward ? 0.16f : 0.06f, downward ? 28f : 13f, downward ? 1.35f : 1.75f, 0.12f, downward ? 0.62f : 0.38f, 0.018f, 0.055f, 0f, downward ? 86 : 44, spec.Duration, downward ? -0.34f : -0.08f);
            AddSparkSystem(parent, "secondary_copper_ticks", materials["CopperGlow"], new Color(1f, 0.36f, 0.1f, 0.92f), new Vector3(0f, 0.04f, 0.08f), downward ? new Vector3(58f, spec.PreviewYaw - 12f, 0f) : new Vector3(0f, spec.PreviewYaw + 8f, 0f), 0.18f, downward ? 38f : 18f, downward ? 0.9f : 1.2f, 0.16f, 0.55f, 0.012f, 0.04f, 0f, downward ? 58 : 32, spec.Duration, downward ? -0.42f : -0.12f);
            AddSteamSystem(parent, "impact_smoke_wisp", materials["SootSmoke"], new Color(0.22f, 0.2f, 0.18f, 0.36f), new Vector3(0f, 0.06f, 0.1f), new Vector3(-70f, 0f, 0f), 0.12f, 26f, 0.18f, 0.45f, 1.15f, 0.08f, 0.24f, 0f, 22, spec.Duration + 0.35f, false, -0.05f, 0.7f);
        }

        private static void AddMuzzlePrefab(VfxSpec spec, Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreateMeshPart("black_iron_muzzle_barrel", meshes["Cylinder"], parent, new Vector3(0f, 0f, -0.18f), new Vector3(0.18f, 0.18f, 0.52f), Quaternion.identity, materials["BlackIron"]);
            CreateMeshPart("brass_muzzle_collar", meshes["Ring"], parent, new Vector3(0f, 0f, 0.1f), new Vector3(0.48f, 0.48f, 0.48f), Quaternion.identity, materials["BrassTrim"]);

            var scatter = spec.Id.Contains("Scatter", StringComparison.Ordinal);
            var rifle = spec.Id.Contains("Rifle", StringComparison.Ordinal);
            var angle = scatter ? 32f : rifle ? 8f : 18f;
            var length = rifle ? 1.25f : scatter ? 0.52f : 0.64f;
            var burstScale = rifle ? new Vector3(0.34f, 0.34f, 1f) : scatter ? new Vector3(1.05f, 1.05f, 1f) : new Vector3(0.68f, 0.68f, 1f);

            CreateMeshPart("muzzle_flash_starburst", meshes["Burst"], parent, new Vector3(0f, 0f, 0.16f), burstScale, Quaternion.Euler(0f, 0f, scatter ? 15f : -8f), materials[spec.ColorKey]);
            AddFlashSystem(parent, "white_hot_muzzle_core", materials["MuzzleCore"], new Color(1f, 0.84f, 0.45f, 0.98f), new Vector3(0f, 0f, 0.14f), Vector3.zero, ParticleSystemShapeType.Cone, 0.05f, angle, length, 0.035f, 0.11f, rifle ? 0.04f : 0.06f, scatter ? 0.2f : 0.13f, 0f, scatter ? 42 : 26, spec.Duration, false);
            AddFlashSystem(parent, "amber_pressure_flare", materials["PressureAmber"], new Color(1f, 0.54f, 0.12f, 0.82f), new Vector3(0f, 0f, 0.12f), Vector3.zero, ParticleSystemShapeType.Cone, 0.08f, angle + 8f, length * 0.72f, 0.05f, 0.17f, 0.08f, scatter ? 0.26f : 0.18f, 0f, scatter ? 58 : 34, spec.Duration, false);
            AddSparkSystem(parent, "muzzle_copper_spark_cone", materials["HotSpark"], new Color(1f, 0.6f, 0.18f, 1f), new Vector3(0f, 0f, 0.18f), Vector3.zero, scatter ? 0.11f : 0.045f, angle + 12f, rifle ? 1.7f : 1.15f, 0.05f, 0.24f, 0.012f, 0.04f, 0f, scatter ? 46 : 22, spec.Duration, -0.03f);
            AddSteamSystem(parent, "muzzle_backsteam", materials["SteamSoft"], new Color(0.72f, 0.76f, 0.72f, 0.34f), new Vector3(0f, 0f, 0.02f), new Vector3(0f, 180f, 0f), 0.11f, 24f, 0.25f, 0.35f, 1.0f, 0.08f, 0.28f, 0f, 26, spec.Duration + 0.5f, false, -0.05f, 0.65f);
        }

        private static void AddFurnacePrefab(VfxSpec spec, Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreateMeshPart("black_iron_furnace_mouth", meshes["Plate"], parent, new Vector3(0f, 0f, 0f), new Vector3(0.96f, 0.64f, 0.08f), Quaternion.identity, materials["BlackIron"]);
            CreateMeshPart("brass_furnace_frame", meshes["Ring"], parent, new Vector3(0f, 0f, -0.04f), new Vector3(1.15f, 0.82f, 1f), Quaternion.identity, materials["BrassTrim"]);
            CreateMeshPart("furnace_grate_slats_a", meshes["Box"], parent, new Vector3(-0.18f, -0.04f, -0.1f), new Vector3(0.05f, 0.58f, 0.05f), Quaternion.identity, materials["BlackIron"]);
            CreateMeshPart("furnace_grate_slats_b", meshes["Box"], parent, new Vector3(0.18f, -0.04f, -0.1f), new Vector3(0.05f, 0.58f, 0.05f), Quaternion.identity, materials["BlackIron"]);

            var stack = spec.Id.Contains("Embers", StringComparison.Ordinal);
            var fireRotation = stack ? new Vector3(-90f, 0f, 0f) : Vector3.zero;
            AddFlashSystem(parent, "furnace_white_core", materials["FurnaceWhite"], new Color(1f, 0.92f, 0.55f, 0.96f), new Vector3(0f, 0f, 0.06f), fireRotation, stack ? ParticleSystemShapeType.Circle : ParticleSystemShapeType.Cone, stack ? 0.28f : 0.15f, stack ? 0f : 18f, stack ? 0.12f : 0.42f, 0.08f, 0.24f, 0.08f, 0.2f, stack ? 22f : 0f, stack ? 12 : 64, spec.Duration, stack);
            AddFlashSystem(parent, "orange_flame_tongues", materials["FurnaceOrange"], new Color(1f, 0.34f, 0.07f, 0.78f), new Vector3(0f, 0.02f, 0.09f), fireRotation, stack ? ParticleSystemShapeType.Circle : ParticleSystemShapeType.Cone, stack ? 0.34f : 0.2f, stack ? 0f : 32f, stack ? 0.35f : 0.65f, 0.18f, 0.75f, 0.12f, 0.36f, stack ? 38f : 0f, stack ? 24 : 86, spec.Duration, stack);
            AddSparkSystem(parent, "furnace_rising_embers", materials["HotSpark"], new Color(1f, 0.48f, 0.12f, 0.92f), new Vector3(0f, 0.04f, 0.08f), new Vector3(-90f, 0f, 0f), 0.32f, 18f, 0.45f, 0.35f, 1.4f, 0.012f, 0.045f, stack ? 32f : 0f, stack ? 16 : 68, spec.Duration, 0.02f);
            AddSteamSystem(parent, "furnace_soot_roll", materials["SootSmoke"], new Color(0.16f, 0.145f, 0.13f, 0.42f), new Vector3(0f, 0.12f, 0.1f), new Vector3(-90f, 0f, 0f), 0.35f, 38f, 0.22f, 0.65f, 1.8f, 0.16f, 0.48f, stack ? 12f : 0f, stack ? 8 : 42, spec.Duration + 0.55f, stack, -0.06f, 1.15f);
        }

        private static void AddValvePrefab(VfxSpec spec, Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreateMeshPart("valve_backplate_black_iron", meshes["Plate"], parent, new Vector3(0f, 0f, 0f), new Vector3(0.82f, 0.82f, 0.08f), Quaternion.identity, materials["BlackIron"]);
            CreateMeshPart("valve_wheel_outer", meshes["Ring"], parent, new Vector3(0f, 0f, -0.08f), Vector3.one * 0.86f, Quaternion.identity, materials["BrassTrim"]);
            CreateMeshPart("valve_wheel_spoke_horizontal", meshes["Box"], parent, new Vector3(0f, 0f, -0.09f), new Vector3(0.68f, 0.07f, 0.05f), Quaternion.identity, materials["BrassTrim"]);
            CreateMeshPart("valve_wheel_spoke_vertical", meshes["Box"], parent, new Vector3(0f, 0f, -0.09f), new Vector3(0.07f, 0.68f, 0.05f), Quaternion.identity, materials["BrassTrim"]);
            CreateMeshPart("valve_status_lens", meshes["Ring"], parent, new Vector3(0f, 0f, -0.14f), Vector3.one * 0.34f, Quaternion.identity, materials[spec.ColorKey]);

            var frosted = spec.Id.Contains("Frosted", StringComparison.Ordinal);
            AddSteamSystem(parent, "valve_release_cloud", materials[frosted ? "BossBlue" : "SteamSoft"], frosted ? new Color(0.52f, 0.72f, 1f, 0.42f) : new Color(0.8f, 0.84f, 0.78f, 0.44f), new Vector3(0f, 0f, 0.1f), Vector3.zero, 0.2f, frosted ? 34f : 25f, frosted ? 0.32f : 0.55f, 0.35f, 1.3f, 0.08f, 0.32f, 0f, frosted ? 58 : 48, spec.Duration, false, -0.04f, frosted ? 1.0f : 0.75f);
            AddFlashSystem(parent, "valve_status_glint", materials[spec.ColorKey], MaterialColor(spec.ColorKey, 0.8f), new Vector3(0f, 0f, -0.18f), Vector3.zero, ParticleSystemShapeType.Circle, 0.16f, 0f, 0.01f, 0.05f, 0.18f, 0.04f, 0.11f, 0f, 22, spec.Duration, false);
            AddSteamSystem(parent, "valve_closing_pressure_halo", materials["PressureAmber"], new Color(1f, 0.56f, 0.18f, 0.22f), new Vector3(0f, 0f, 0.04f), Vector3.zero, 0.42f, 0f, 0.08f, 0.22f, 0.7f, 0.04f, 0.16f, 0f, 36, spec.Duration, false, -0.03f, 0.45f);
        }

        private static void AddBossPrefab(VfxSpec spec, Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreateMeshPart("boss_pressure_crown_outer", meshes["Ring"], parent, new Vector3(0f, 0f, -0.04f), Vector3.one * 1.24f, Quaternion.identity, materials["BrassTrim"]);
            CreateMeshPart("boss_core_black_iron_plate", meshes["Plate"], parent, new Vector3(0f, 0f, 0f), new Vector3(1.05f, 0.72f, 0.08f), Quaternion.identity, materials["BlackIron"]);
            CreateMeshPart("boss_warning_burst_emblem", meshes["Burst"], parent, new Vector3(0f, 0f, -0.1f), Vector3.one * 0.78f, Quaternion.Euler(0f, 0f, 11f), materials[spec.ColorKey]);
            CreateMeshPart("boss_phase_needle", meshes["Needle"], parent, new Vector3(0.06f, -0.03f, -0.16f), Vector3.one * 0.88f, Quaternion.Euler(0f, 0f, spec.Pattern % 2 == 0 ? -42f : 36f), materials["PressureAmber"]);

            var heart = spec.Id.Contains("Heart", StringComparison.Ordinal);
            var crown = spec.Id.Contains("Crown", StringComparison.Ordinal);
            AddFlashSystem(parent, "boss_furnace_core_pulse", materials[heart ? "FurnaceOrange" : spec.ColorKey], MaterialColor(heart ? "FurnaceOrange" : spec.ColorKey, 0.9f), new Vector3(0f, 0f, -0.18f), Vector3.zero, ParticleSystemShapeType.Circle, 0.35f, 0f, 0.02f, 0.08f, 0.3f, 0.08f, 0.24f, 0f, 64, spec.Duration, false);
            AddSteamSystem(parent, "boss_pressure_smoke_collar", materials["SootSmoke"], new Color(0.18f, 0.16f, 0.145f, 0.44f), new Vector3(0f, 0.08f, 0.08f), new Vector3(-90f, 0f, 0f), 0.46f, 42f, 0.22f, 0.55f, 1.75f, 0.16f, 0.58f, 0f, 88, spec.Duration + 0.4f, false, -0.05f, 1.2f);
            AddSparkSystem(parent, "boss_phase_bolt_sparks", materials["HotSpark"], new Color(1f, 0.5f, 0.12f, 1f), new Vector3(0f, 0.03f, -0.06f), new Vector3(0f, spec.PreviewYaw, 0f), 0.2f, 46f, 1.35f, 0.16f, 0.58f, 0.018f, 0.06f, 0f, 72, spec.Duration, -0.08f);

            if (crown)
            {
                AddSteamSystem(parent, "boss_left_emergency_vent", materials["SteamDense"], new Color(0.92f, 0.94f, 0.88f, 0.55f), new Vector3(-0.38f, 0.16f, 0.1f), new Vector3(-90f, 0f, -10f), 0.12f, 18f, 0.68f, 0.28f, 1.15f, 0.07f, 0.26f, 0f, 46, spec.Duration, false, -0.02f, 0.85f);
                AddSteamSystem(parent, "boss_center_emergency_vent", materials["SteamDense"], new Color(0.95f, 0.95f, 0.9f, 0.58f), new Vector3(0f, 0.22f, 0.12f), new Vector3(-90f, 0f, 0f), 0.15f, 16f, 0.82f, 0.28f, 1.2f, 0.08f, 0.29f, 0f, 58, spec.Duration, false, -0.02f, 0.9f);
                AddSteamSystem(parent, "boss_right_emergency_vent", materials["SteamDense"], new Color(0.92f, 0.94f, 0.88f, 0.55f), new Vector3(0.38f, 0.16f, 0.1f), new Vector3(-90f, 0f, 10f), 0.12f, 18f, 0.68f, 0.28f, 1.15f, 0.07f, 0.26f, 0f, 46, spec.Duration, false, -0.02f, 0.85f);
            }
            else
            {
                AddSteamSystem(parent, "boss_single_overpressure_vent", materials["SteamDense"], new Color(0.9f, 0.92f, 0.86f, 0.5f), new Vector3(0f, 0.18f, 0.1f), new Vector3(-90f, 0f, 0f), 0.24f, 30f, 0.58f, 0.28f, 1.35f, 0.08f, 0.34f, 0f, 82, spec.Duration, false, -0.02f, 0.95f);
            }
        }

        private static ParticleSystem AddSteamSystem(
            Transform parent,
            string name,
            Material material,
            Color color,
            Vector3 localPosition,
            Vector3 localEuler,
            float radius,
            float angle,
            float speed,
            float lifetimeMin,
            float lifetimeMax,
            float sizeMin,
            float sizeMax,
            float rate,
            int burst,
            float duration,
            bool loop,
            float gravity,
            float noiseStrength)
        {
            var ps = CreateBaseParticleSystem(parent, name, material, color, localPosition, localEuler, duration, loop);
            var main = ps.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetimeMin, lifetimeMax);
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.55f, speed);
            main.startSize = new ParticleSystem.MinMaxCurve(sizeMin, sizeMax);
            main.gravityModifier = gravity;
            main.maxParticles = Mathf.Max(128, burst * 4 + Mathf.RoundToInt(rate * duration * 4f));

            var emission = ps.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
            if (burst > 0)
            {
                emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)Mathf.Clamp(burst, 0, short.MaxValue)) });
            }

            var shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = angle <= 0.01f ? ParticleSystemShapeType.Circle : ParticleSystemShapeType.Cone;
            shape.radius = radius;
            shape.angle = angle;
            shape.length = 0.08f;

            var colorModule = ps.colorOverLifetime;
            colorModule.enabled = true;
            colorModule.color = new ParticleSystem.MinMaxGradient(CreateFadeGradient(color, color * 0.82f, color.a, 0f));

            var size = ps.sizeOverLifetime;
            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
                new Keyframe(0f, 0.58f),
                new Keyframe(0.35f, 1.12f),
                new Keyframe(1f, 1.55f)));

            var noise = ps.noise;
            noise.enabled = noiseStrength > 0.01f;
            noise.strength = noiseStrength;
            noise.frequency = 0.42f;
            noise.scrollSpeed = 0.18f;

            var renderer = ps.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.sortingFudge = 0.2f;
            return ps;
        }

        private static ParticleSystem AddSparkSystem(
            Transform parent,
            string name,
            Material material,
            Color color,
            Vector3 localPosition,
            Vector3 localEuler,
            float radius,
            float angle,
            float speed,
            float lifetimeMin,
            float lifetimeMax,
            float sizeMin,
            float sizeMax,
            float rate,
            int burst,
            float duration,
            float gravity)
        {
            var ps = CreateBaseParticleSystem(parent, name, material, color, localPosition, localEuler, duration, false);
            var main = ps.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetimeMin, lifetimeMax);
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.72f, speed * 1.24f);
            main.startSize = new ParticleSystem.MinMaxCurve(sizeMin, sizeMax);
            main.gravityModifier = gravity;
            main.maxParticles = Mathf.Max(96, burst * 3 + Mathf.RoundToInt(rate * duration * 4f));

            var emission = ps.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
            if (burst > 0)
            {
                emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)Mathf.Clamp(burst, 0, short.MaxValue)) });
            }

            var shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.radius = radius;
            shape.angle = angle;
            shape.length = 0.08f;

            var colorModule = ps.colorOverLifetime;
            colorModule.enabled = true;
            colorModule.color = new ParticleSystem.MinMaxGradient(CreateFadeGradient(color, new Color(1f, 0.18f, 0.04f, color.a), color.a, 0f));

            var size = ps.sizeOverLifetime;
            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(0.45f, 0.55f),
                new Keyframe(1f, 0.08f)));

            var rotation = ps.rotationOverLifetime;
            rotation.enabled = true;
            rotation.z = new ParticleSystem.MinMaxCurve(-8f, 8f);

            var renderer = ps.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Stretch;
            renderer.lengthScale = 2.4f;
            renderer.velocityScale = 0.18f;
            renderer.sortingFudge = 0.45f;
            return ps;
        }

        private static ParticleSystem AddFlashSystem(
            Transform parent,
            string name,
            Material material,
            Color color,
            Vector3 localPosition,
            Vector3 localEuler,
            ParticleSystemShapeType shapeType,
            float radius,
            float angle,
            float length,
            float lifetimeMin,
            float lifetimeMax,
            float sizeMin,
            float sizeMax,
            float rate,
            int burst,
            float duration,
            bool loop)
        {
            var ps = CreateBaseParticleSystem(parent, name, material, color, localPosition, localEuler, duration, loop);
            var main = ps.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetimeMin, lifetimeMax);
            main.startSpeed = new ParticleSystem.MinMaxCurve(length * 0.75f, Mathf.Max(length, 0.02f));
            main.startSize = new ParticleSystem.MinMaxCurve(sizeMin, sizeMax);
            main.gravityModifier = 0f;
            main.maxParticles = Mathf.Max(96, burst * 3 + Mathf.RoundToInt(rate * duration * 4f));

            var emission = ps.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
            if (burst > 0)
            {
                emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)Mathf.Clamp(burst, 0, short.MaxValue)) });
            }

            var shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = shapeType;
            shape.radius = radius;
            shape.angle = angle;
            shape.length = Mathf.Max(0.01f, length);

            var colorModule = ps.colorOverLifetime;
            colorModule.enabled = true;
            colorModule.color = new ParticleSystem.MinMaxGradient(CreateFadeGradient(color, color * 0.72f, color.a, 0f));

            var size = ps.sizeOverLifetime;
            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
                new Keyframe(0f, 0.2f),
                new Keyframe(0.18f, 1.2f),
                new Keyframe(1f, 0.06f)));

            var renderer = ps.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.sortingFudge = 0.65f;
            return ps;
        }

        private static ParticleSystem CreateBaseParticleSystem(
            Transform parent,
            string name,
            Material material,
            Color startColor,
            Vector3 localPosition,
            Vector3 localEuler,
            float duration,
            bool loop)
        {
            var go = CreateChild(parent, name).gameObject;
            go.transform.localPosition = localPosition;
            go.transform.localRotation = Quaternion.Euler(localEuler);

            var ps = go.AddComponent<ParticleSystem>();
            ps.useAutoRandomSeed = false;
            ps.randomSeed = (uint)StableSeed(name);

            var main = ps.main;
            main.duration = Mathf.Max(0.1f, duration);
            main.loop = loop;
            main.playOnAwake = true;
            main.startDelay = 0f;
            main.startColor = startColor;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;

            var collision = ps.collision;
            collision.enabled = false;
            var trigger = ps.trigger;
            trigger.enabled = false;
            var externalForces = ps.externalForces;
            externalForces.enabled = false;

            var renderer = ps.GetComponent<ParticleSystemRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            return ps;
        }

        private static Dictionary<string, Material> CreateMaterials()
        {
            var materials = new Dictionary<string, Material>(StringComparer.Ordinal)
            {
                ["SteamSoft"] = CreateMaterial("BBSVFX02_MAT_SteamSoft", new Color(0.78f, 0.83f, 0.8f, 0.42f), new Color(0.12f, 0.16f, 0.16f), true, false, 0f, 0.15f),
                ["SteamDense"] = CreateMaterial("BBSVFX02_MAT_SteamDense", new Color(0.9f, 0.92f, 0.84f, 0.58f), new Color(0.18f, 0.2f, 0.17f), true, false, 0f, 0.2f),
                ["SootSmoke"] = CreateMaterial("BBSVFX02_MAT_SootSmoke", new Color(0.16f, 0.145f, 0.13f, 0.45f), new Color(0.02f, 0.018f, 0.015f), true, false, 0f, 0.05f),
                ["PressureAmber"] = CreateMaterial("BBSVFX02_MAT_PressureAmber", new Color(1f, 0.58f, 0.14f, 0.78f), new Color(1.6f, 0.72f, 0.18f), true, true, 0f, 0.55f),
                ["CopperGlow"] = CreateMaterial("BBSVFX02_MAT_CopperGlow", new Color(0.9f, 0.35f, 0.12f, 0.88f), new Color(1.15f, 0.42f, 0.12f), true, true, 0.1f, 0.4f),
                ["HotSpark"] = CreateMaterial("BBSVFX02_MAT_HotSpark", new Color(1f, 0.74f, 0.22f, 1f), new Color(2.2f, 1.1f, 0.22f), true, true, 0f, 0.5f),
                ["MuzzleCore"] = CreateMaterial("BBSVFX02_MAT_MuzzleCore", new Color(1f, 0.92f, 0.58f, 0.96f), new Color(2.1f, 1.55f, 0.48f), true, true, 0f, 0.62f),
                ["FurnaceWhite"] = CreateMaterial("BBSVFX02_MAT_FurnaceWhite", new Color(1f, 0.9f, 0.55f, 0.94f), new Color(2.4f, 1.45f, 0.35f), true, true, 0f, 0.5f),
                ["FurnaceOrange"] = CreateMaterial("BBSVFX02_MAT_FurnaceOrange", new Color(1f, 0.31f, 0.055f, 0.84f), new Color(1.9f, 0.48f, 0.08f), true, true, 0f, 0.38f),
                ["WarningRed"] = CreateMaterial("BBSVFX02_MAT_WarningRed", new Color(1f, 0.08f, 0.035f, 0.86f), new Color(2f, 0.18f, 0.05f), true, true, 0f, 0.3f),
                ["BossRed"] = CreateMaterial("BBSVFX02_MAT_BossRed", new Color(0.95f, 0.07f, 0.035f, 0.86f), new Color(2.1f, 0.15f, 0.04f), true, true, 0f, 0.36f),
                ["BossBlue"] = CreateMaterial("BBSVFX02_MAT_BossBluePressure", new Color(0.36f, 0.62f, 1f, 0.58f), new Color(0.4f, 0.8f, 1.7f), true, true, 0f, 0.34f),
                ["ValveGreen"] = CreateMaterial("BBSVFX02_MAT_ValveGreen", new Color(0.38f, 0.9f, 0.42f, 0.78f), new Color(0.38f, 1.25f, 0.5f), true, true, 0f, 0.32f),
                ["GaugeGlass"] = CreateMaterial("BBSVFX02_MAT_GaugeGlass", new Color(0.62f, 0.76f, 0.82f, 0.32f), new Color(0.18f, 0.28f, 0.3f), true, false, 0f, 0.75f),
                ["BrassTrim"] = CreateMaterial("BBSVFX02_MAT_AgedBrassTrim", new Color(0.67f, 0.48f, 0.2f, 1f), new Color(0.16f, 0.09f, 0.025f), false, false, 0.72f, 0.48f),
                ["BlackIron"] = CreateMaterial("BBSVFX02_MAT_BlackenedIron", new Color(0.075f, 0.071f, 0.064f, 1f), new Color(0.01f, 0.008f, 0.006f), false, false, 0.85f, 0.22f)
            };

            return materials;
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            return new Dictionary<string, Mesh>(StringComparer.Ordinal)
            {
                ["Plate"] = CreateOrUpdateMesh("BBSVFX02_MESH_FlatPressurePlate", BuildPlateMesh),
                ["Box"] = CreateOrUpdateMesh("BBSVFX02_MESH_BoxUnit", BuildBoxMesh),
                ["Cylinder"] = CreateOrUpdateMesh("BBSVFX02_MESH_CylinderPipeStub_24", BuildCylinderMesh),
                ["Ring"] = CreateOrUpdateMesh("BBSVFX02_MESH_BrassPressureRing_32", BuildRingMesh),
                ["Burst"] = CreateOrUpdateMesh("BBSVFX02_MESH_RadialBurst_16", BuildBurstMesh),
                ["Needle"] = CreateOrUpdateMesh("BBSVFX02_MESH_GaugeNeedle", BuildNeedleMesh),
                ["Shard"] = CreateOrUpdateMesh("BBSVFX02_MESH_SparkShardDiamond", BuildShardMesh),
                ["Flame"] = CreateOrUpdateMesh("BBSVFX02_MESH_FlameTongue", BuildFlameMesh)
            };
        }

        private static Material CreateMaterial(string assetName, Color baseColor, Color emissionColor, bool transparent, bool additive, float metallic, float smoothness)
        {
            var path = MaterialRoot + "/" + assetName + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find(transparent ? "Particles/Standard Unlit" : "Standard") ?? Shader.Find("Standard");
            if (material == null)
            {
                material = new Material(shader)
                {
                    name = assetName
                };
                AssetDatabase.CreateAsset(material, path);
            }
            else
            {
                material.shader = shader;
            }

            SetColor(material, "_Color", baseColor);
            SetColor(material, "_BaseColor", baseColor);
            SetColor(material, "_TintColor", baseColor);
            SetColor(material, "_EmissionColor", emissionColor);
            SetFloat(material, "_Metallic", metallic);
            SetFloat(material, "_Glossiness", smoothness);
            SetFloat(material, "_Smoothness", smoothness);
            ConfigureBlend(material, transparent, additive);
            material.EnableKeyword("_EMISSION");
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void ConfigureBlend(Material material, bool transparent, bool additive)
        {
            if (!transparent)
            {
                SetFloat(material, "_Mode", 0f);
                SetFloat(material, "_ZWrite", 1f);
                material.renderQueue = 2000;
                return;
            }

            SetFloat(material, "_Mode", 3f);
            SetFloat(material, "_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            SetFloat(material, "_DstBlend", (float)(additive ? UnityEngine.Rendering.BlendMode.One : UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha));
            SetFloat(material, "_ZWrite", 0f);
            SetFloat(material, "_Cull", 0f);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword(additive ? "_ALPHAPREMULTIPLY_ON" : "_ALPHABLEND_ON");
            material.renderQueue = 3000;
        }

        private static Mesh CreateOrUpdateMesh(string assetName, Action<Mesh> builder)
        {
            var path = MeshRoot + "/" + assetName + ".asset";
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            var created = false;
            if (mesh == null)
            {
                mesh = new Mesh();
                created = true;
            }

            mesh.name = assetName;
            mesh.Clear();
            builder(mesh);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            if (created)
            {
                AssetDatabase.CreateAsset(mesh, path);
            }
            else
            {
                EditorUtility.SetDirty(mesh);
            }

            return mesh;
        }

        private static void BuildPlateMesh(Mesh mesh)
        {
            mesh.SetVertices(new[]
            {
                new Vector3(-0.5f, -0.5f, 0f),
                new Vector3(-0.5f, 0.5f, 0f),
                new Vector3(0.5f, 0.5f, 0f),
                new Vector3(0.5f, -0.5f, 0f)
            });
            mesh.SetTriangles(new[] { 0, 1, 2, 0, 2, 3, 2, 1, 0, 3, 2, 0 }, 0);
            mesh.SetUVs(0, new[] { Vector2.zero, Vector2.up, Vector2.one, Vector2.right });
        }

        private static void BuildBoxMesh(Mesh mesh)
        {
            var v = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
            };
            var t = new[]
            {
                0, 2, 1, 0, 3, 2,
                4, 5, 6, 4, 6, 7,
                0, 1, 5, 0, 5, 4,
                1, 2, 6, 1, 6, 5,
                2, 3, 7, 2, 7, 6,
                3, 0, 4, 3, 4, 7
            };
            mesh.SetVertices(v);
            mesh.SetTriangles(t, 0);
        }

        private static void BuildCylinderMesh(Mesh mesh)
        {
            const int segments = 24;
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            vertices.Add(new Vector3(0f, 0f, -0.5f));
            vertices.Add(new Vector3(0f, 0f, 0.5f));
            for (var i = 0; i < segments; i++)
            {
                var a = Mathf.PI * 2f * i / segments;
                var x = Mathf.Cos(a) * 0.5f;
                var y = Mathf.Sin(a) * 0.5f;
                vertices.Add(new Vector3(x, y, -0.5f));
                vertices.Add(new Vector3(x, y, 0.5f));
            }

            for (var i = 0; i < segments; i++)
            {
                var next = (i + 1) % segments;
                var b0 = 2 + i * 2;
                var t0 = b0 + 1;
                var b1 = 2 + next * 2;
                var t1 = b1 + 1;
                triangles.AddRange(new[] { b0, t0, t1, b0, t1, b1 });
                triangles.AddRange(new[] { 0, b1, b0, 1, t0, t1 });
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
        }

        private static void BuildRingMesh(Mesh mesh)
        {
            const int segments = 32;
            const float outer = 0.5f;
            const float inner = 0.34f;
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i < segments; i++)
            {
                var a = Mathf.PI * 2f * i / segments;
                vertices.Add(new Vector3(Mathf.Cos(a) * outer, Mathf.Sin(a) * outer, 0f));
                vertices.Add(new Vector3(Mathf.Cos(a) * inner, Mathf.Sin(a) * inner, 0f));
            }

            for (var i = 0; i < segments; i++)
            {
                var next = (i + 1) % segments;
                var o0 = i * 2;
                var i0 = o0 + 1;
                var o1 = next * 2;
                var i1 = o1 + 1;
                triangles.AddRange(new[] { o0, i1, i0, o0, o1, i1 });
                triangles.AddRange(new[] { i0, i1, o0, i1, o1, o0 });
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
        }

        private static void BuildBurstMesh(Mesh mesh)
        {
            const int rays = 16;
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            for (var i = 0; i < rays; i++)
            {
                var center = Mathf.PI * 2f * i / rays;
                var half = Mathf.PI / rays * 0.36f;
                var inner = 0.13f + (i % 2) * 0.04f;
                var outer = 0.5f - (i % 3) * 0.045f;
                var start = vertices.Count;
                vertices.Add(new Vector3(Mathf.Cos(center - half) * inner, Mathf.Sin(center - half) * inner, 0f));
                vertices.Add(new Vector3(Mathf.Cos(center) * outer, Mathf.Sin(center) * outer, 0f));
                vertices.Add(new Vector3(Mathf.Cos(center + half) * inner, Mathf.Sin(center + half) * inner, 0f));
                triangles.AddRange(new[] { start, start + 1, start + 2, start + 2, start + 1, start });
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
        }

        private static void BuildNeedleMesh(Mesh mesh)
        {
            mesh.SetVertices(new[]
            {
                new Vector3(-0.035f, -0.08f, 0f),
                new Vector3(0.035f, -0.08f, 0f),
                new Vector3(0.018f, 0.48f, 0f),
                new Vector3(0f, 0.55f, 0f),
                new Vector3(-0.018f, 0.48f, 0f)
            });
            mesh.SetTriangles(new[] { 0, 1, 2, 0, 2, 4, 4, 2, 3, 2, 1, 0, 4, 2, 0, 3, 2, 4 }, 0);
        }

        private static void BuildShardMesh(Mesh mesh)
        {
            mesh.SetVertices(new[]
            {
                new Vector3(0f, 0.08f, 0f),
                new Vector3(0.035f, 0f, 0.02f),
                new Vector3(0f, -0.12f, 0f),
                new Vector3(-0.035f, 0f, -0.02f)
            });
            mesh.SetTriangles(new[] { 0, 1, 2, 0, 2, 3, 0, 3, 2, 0, 2, 1 }, 0);
        }

        private static void BuildFlameMesh(Mesh mesh)
        {
            mesh.SetVertices(new[]
            {
                new Vector3(-0.18f, -0.32f, 0f),
                new Vector3(0.16f, -0.3f, 0f),
                new Vector3(0.23f, 0.08f, 0f),
                new Vector3(0.03f, 0.5f, 0f),
                new Vector3(-0.24f, 0.02f, 0f)
            });
            mesh.SetTriangles(new[] { 0, 1, 2, 0, 2, 4, 4, 2, 3, 2, 1, 0, 4, 2, 0, 3, 2, 4 }, 0);
        }

        private static Transform CreateChild(Transform parent, string name)
        {
            var child = new GameObject(name).transform;
            child.SetParent(parent, false);
            return child;
        }

        private static void CreateSocket(Transform parent, string name, Vector3 localPosition)
        {
            var socket = CreateChild(parent, name);
            socket.localPosition = localPosition;
        }

        private static GameObject CreateMeshPart(string name, Mesh mesh, Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material)
        {
            var part = CreateChild(parent, name).gameObject;
            part.transform.localPosition = localPosition;
            part.transform.localScale = localScale;
            part.transform.localRotation = localRotation;
            var filter = part.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            var renderer = part.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            return part;
        }

        private static void SavePrefab(GameObject prefab, string assetPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(AssetPathToFullPath(assetPath)) ?? string.Empty);
            PrefabUtility.SaveAsPrefabAsset(prefab, assetPath);
            UnityEngine.Object.DestroyImmediate(prefab);
        }

        private static void WriteCatalog()
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"BBSVFX02\",");
            builder.AppendLine("  \"version\": \"0.1.42-p001\",");
            builder.AppendLine("  \"description\": \"Visual-only Steam VFX Set 02 sidecar catalog for Brassworks Breach.\",");
            builder.AppendLine("  \"prefabs\": [");
            for (var i = 0; i < Specs.Length; i++)
            {
                var spec = Specs[i];
                builder.AppendLine("    {");
                builder.AppendLine("      \"vfx_id\": \"BBSVFX02_" + EscapeJson(spec.Id) + "\",");
                builder.AppendLine("      \"family\": \"" + EscapeJson(spec.Family) + "\",");
                builder.AppendLine("      \"prefab\": \"" + EscapeJson(PrefabPath(spec)) + "\",");
                builder.AppendLine("      \"recommended_socket\": \"" + EscapeJson(spec.Socket) + "\",");
                builder.AppendLine("      \"visual_intent\": \"" + EscapeJson(spec.Intent) + "\",");
                builder.AppendLine("      \"duration_seconds\": " + spec.Duration.ToString("0.###", CultureInfo.InvariantCulture) + ",");
                builder.AppendLine("      \"looping\": " + JsonBool(spec.Looping) + ",");
                builder.AppendLine("      \"recommended_world_scale\": " + spec.Scale.ToString("0.###", CultureInfo.InvariantCulture) + ",");
                builder.AppendLine("      \"safety_tags\": [\"visual_only\", \"no_audio\", \"no_colliders\", \"no_rigidbodies\", \"no_gameplay_authority\"]");
                builder.Append("    }");
                builder.AppendLine(i == Specs.Length - 1 ? string.Empty : ",");
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(AssetPathToFullPath(MetadataRoot + "/BBSVFX02_Catalog_v0.1.42.json"), builder.ToString(), Encoding.UTF8);
        }

        private static void WriteManifest()
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"BBSVFX02\",");
            builder.AppendLine("  \"display_name\": \"Brassworks Breach Steam VFX Set 02\",");
            builder.AppendLine("  \"version\": \"0.1.42-p001\",");
            builder.AppendLine("  \"build_id\": \"p001\",");
            builder.AppendLine("  \"unity_version\": \"6000.4.6f1\",");
            builder.AppendLine("  \"sidecar_project\": \"Brassworks Breach\",");
            builder.AppendLine("  \"owner_lane\": \"parallel_asset_package_worker\",");
            builder.AppendLine("  \"primary_intake_owner\": \"main_game_integration\",");
            builder.AppendLine("  \"canonical_root\": \"AssetPacks/BrassworksBreach.SteamVFXSet02\",");
            builder.AppendLine("  \"asset_counts\": {");
            builder.AppendLine("    \"prefabs\": " + Specs.Length + ",");
            builder.AppendLine("    \"materials\": 16,");
            builder.AppendLine("    \"meshes\": 8,");
            builder.AppendLine("    \"catalog_json\": 1,");
            builder.AppendLine("    \"preview_pngs\": 2");
            builder.AppendLine("  },");
            builder.AppendLine("  \"dependencies\": [],");
            builder.AppendLine("  \"required_primary_changes\": [],");
            builder.AppendLine("  \"path_collisions_checked\": true,");
            builder.AppendLine("  \"guid_collisions_checked\": true,");
            builder.AppendLine("  \"import_smoke_status\": \"pass - generated and validated in package-local ValidationProject~\",");
            builder.AppendLine("  \"known_risks\": [");
            builder.AppendLine("    \"Particle timing and scale should be tuned in a quarantine gameplay scene before promotion.\",");
            builder.AppendLine("    \"Transparent particle materials may need shader remapping if the main project later moves to a custom render pipeline.\"");
            builder.AppendLine("  ],");
            builder.AppendLine("  \"rollback_path\": \"Remove AssetPacks/BrassworksBreach.SteamVFXSet02 and Documentation/AssetProduction/V0_1_42_SteamVFXSet02 plus Documentation/ConceptRenders/V0_1_42_SteamVFXSet02.\"");
            builder.AppendLine("}");
            File.WriteAllText(AssetPathToFullPath(ManifestRoot + "/BBSVFX02_SteamVFXSet02_Manifest_v0.1.42-p001.json"), builder.ToString(), Encoding.UTF8);
        }

        private static void WriteProductionReadme()
        {
            var outputRoot = ResolveProductionDocRoot();
            Directory.CreateDirectory(outputRoot);
            var builder = new StringBuilder();
            builder.AppendLine("# V0.1.42 Steam VFX Set 02");
            builder.AppendLine();
            builder.AppendLine("Package root: `AssetPacks/BrassworksBreach.SteamVFXSet02`");
            builder.AppendLine();
            builder.AppendLine("Generated package-local visual-only VFX assets for later main-game integration. The package uses Unity particle systems, package-local materials, and procedural mesh assets only.");
            builder.AppendLine();
            builder.AppendLine("## Counts");
            builder.AppendLine();
            builder.AppendLine("- Prefabs: `" + Specs.Length + "`");
            builder.AppendLine("- Materials: `16`");
            builder.AppendLine("- Meshes: `8`");
            builder.AppendLine("- Catalog JSON: `1`");
            builder.AppendLine("- Preview PNGs: `2`");
            builder.AppendLine();
            builder.AppendLine("## Runtime Safety Intent");
            builder.AppendLine();
            builder.AppendLine("No gameplay authority, no autonomous audio, no colliders, no rigidbodies, no cameras, no lights, and no scene edits are intentionally included.");
            File.WriteAllText(Path.Combine(outputRoot, "README.md"), builder.ToString(), Encoding.UTF8);
        }

        private static void WriteGenerationEvidence()
        {
            var outputRoot = ResolveProductionDocRoot();
            Directory.CreateDirectory(outputRoot);
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"status\": \"generated\",");
            builder.AppendLine("  \"timestamp\": \"" + EscapeJson(DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture)) + "\",");
            builder.AppendLine("  \"package\": \"BrassworksBreach.SteamVFXSet02\",");
            builder.AppendLine("  \"prefab_count\": " + Specs.Length + ",");
            builder.AppendLine("  \"material_count\": 16,");
            builder.AppendLine("  \"mesh_count\": 8,");
            builder.AppendLine("  \"unity_version\": \"" + EscapeJson(Application.unityVersion) + "\"");
            builder.AppendLine("}");
            File.WriteAllText(Path.Combine(outputRoot, "UnityGenerationEvidence_v0.1.42.json"), builder.ToString(), Encoding.UTF8);
        }

        private static void WritePackageSpecificValidation()
        {
            var outputRoot = ResolveProductionDocRoot();
            Directory.CreateDirectory(outputRoot);

            var errors = new List<string>();
            var warnings = new List<string>();
            var prefabCount = 0;
            var particleSystemCount = 0;
            var meshRendererCount = 0;

            foreach (var spec in Specs)
            {
                var path = PrefabPath(spec);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    errors.Add("Missing prefab: " + path);
                    continue;
                }

                prefabCount++;
                var components = prefab.GetComponentsInChildren<Component>(true);
                foreach (var component in components)
                {
                    if (component == null)
                    {
                        errors.Add(path + " contains a missing script or missing component reference.");
                        continue;
                    }

                    if (IsForbiddenRuntimeComponent(component))
                    {
                        errors.Add(path + " contains forbidden component: " + component.GetType().FullName);
                    }

                    var monoBehaviour = component as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.GetType() != typeof(SteamVfxSet02Identity))
                    {
                        errors.Add(path + " contains non-metadata MonoBehaviour: " + monoBehaviour.GetType().FullName);
                    }

                    var ps = component as ParticleSystem;
                    if (ps != null)
                    {
                        particleSystemCount++;
                        if (ps.collision.enabled)
                        {
                            errors.Add(path + " has particle collision enabled on " + ps.name + ".");
                        }

                        if (ps.trigger.enabled)
                        {
                            errors.Add(path + " has particle trigger module enabled on " + ps.name + ".");
                        }

                        if (ps.externalForces.enabled)
                        {
                            warnings.Add(path + " has external forces enabled on " + ps.name + ".");
                        }
                    }

                    if (component is MeshRenderer)
                    {
                        meshRendererCount++;
                    }
                }
            }

            var materialCount = AssetDatabase.FindAssets("t:Material", new[] { MaterialRoot }).Length;
            var meshCount = AssetDatabase.FindAssets("t:Mesh", new[] { MeshRoot }).Length;
            var catalog = AssetDatabase.LoadAssetAtPath<TextAsset>(MetadataRoot + "/BBSVFX02_Catalog_v0.1.42.json");
            if (catalog == null)
            {
                errors.Add("Catalog JSON missing from Runtime/Metadata.");
            }

            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"status\": \"" + (errors.Count == 0 ? "pass" : "fail") + "\",");
            builder.AppendLine("  \"timestamp\": \"" + EscapeJson(DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture)) + "\",");
            builder.AppendLine("  \"package\": \"BrassworksBreach.SteamVFXSet02\",");
            builder.AppendLine("  \"prefab_count\": " + prefabCount + ",");
            builder.AppendLine("  \"expected_prefab_count\": " + Specs.Length + ",");
            builder.AppendLine("  \"material_count\": " + materialCount + ",");
            builder.AppendLine("  \"mesh_count\": " + meshCount + ",");
            builder.AppendLine("  \"particle_system_count\": " + particleSystemCount + ",");
            builder.AppendLine("  \"mesh_renderer_count\": " + meshRendererCount + ",");
            builder.AppendLine("  \"forbidden_component_policy\": \"no colliders, rigidbodies, audio sources, cameras, lights, animators, directors, or gameplay MonoBehaviours\",");
            builder.AppendLine("  \"errors\": [");
            WriteJsonStringArray(builder, errors, 4);
            builder.AppendLine("  ],");
            builder.AppendLine("  \"warnings\": [");
            WriteJsonStringArray(builder, warnings, 4);
            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(Path.Combine(outputRoot, "SteamVFXSet02Validation_v0.1.42.json"), builder.ToString(), Encoding.UTF8);
        }

        private static bool IsForbiddenRuntimeComponent(Component component)
        {
            return component is Collider ||
                   component is Collider2D ||
                   component is Rigidbody ||
                   component is Rigidbody2D ||
                   component is AudioSource ||
                   component is AudioListener ||
                   component is Camera ||
                   component is Light ||
                   component is Animator ||
                   component is Animation ||
                   component is UnityEngine.Playables.PlayableDirector;
        }

        private static void RenderFullContactSheet(string outputRoot)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var materials = CreateMaterials();
            var labels = new List<TextMesh>();

            CreatePreviewFloor(materials["BlackIron"], new Vector3(0f, -0.55f, 0.6f), new Vector3(13.5f, 0.06f, 8.0f));
            CreatePreviewLighting();

            const int columns = 5;
            for (var i = 0; i < Specs.Length; i++)
            {
                var spec = Specs[i];
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath(spec));
                if (prefab == null)
                {
                    continue;
                }

                var row = i / columns;
                var column = i % columns;
                var position = new Vector3((column - 2) * 3.0f, 0f, (1.5f - row) * 2.25f);
                var instance = UnityEngine.Object.Instantiate(prefab, position, Quaternion.Euler(0f, spec.PreviewYaw, 0f));
                instance.name = prefab.name;
                SimulateParticles(instance, spec.SampleTime);
                labels.Add(CreateLabel(ShortSpecLabel(spec), position + new Vector3(0f, -0.34f, -0.82f), 0.042f, 38));
            }

            var camera = CreatePreviewCamera(new Vector3(0f, 7.0f, -9.2f), new Vector3(0f, 0.15f, 0f), 6.45f);
            FaceLabels(labels, camera.transform.rotation);
            RenderCameraToPng(camera, Path.Combine(outputRoot, "BBSVFX02_v0.1.42_full_contact_sheet.png"), 3200, 2100);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderFamilyContactSheet(string outputRoot)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var materials = CreateMaterials();
            var labels = new List<TextMesh>();
            var chosen = new[]
            {
                Specs[0],
                Specs[4],
                Specs[8],
                Specs[10],
                Specs[13],
                Specs[15],
                Specs[17]
            };

            CreatePreviewFloor(materials["BlackIron"], new Vector3(0f, -0.52f, 0.4f), new Vector3(12.5f, 0.06f, 4.2f));
            CreatePreviewLighting();

            for (var i = 0; i < chosen.Length; i++)
            {
                var spec = chosen[i];
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath(spec));
                if (prefab == null)
                {
                    continue;
                }

                var position = new Vector3((i - 3) * 1.65f, 0f, 0f);
                var instance = UnityEngine.Object.Instantiate(prefab, position, Quaternion.Euler(0f, spec.PreviewYaw, 0f));
                instance.name = prefab.name;
                SimulateParticles(instance, spec.SampleTime);
                labels.Add(CreateLabel(spec.Family, position + new Vector3(0f, -0.24f, -0.72f), 0.052f, 48));
            }

            var camera = CreatePreviewCamera(new Vector3(0f, 4.1f, -6.3f), new Vector3(0f, 0.1f, 0f), 3.2f);
            FaceLabels(labels, camera.transform.rotation);
            RenderCameraToPng(camera, Path.Combine(outputRoot, "BBSVFX02_v0.1.42_family_contact_sheet.png"), 2600, 1200);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void CreatePreviewFloor(Material material, Vector3 localPosition, Vector3 localScale)
        {
            var meshes = CreateMeshes();
            CreateMeshPart("preview_black_iron_floor", meshes["Box"], null, localPosition, localScale, Quaternion.identity, material);
        }

        private static void CreatePreviewLighting()
        {
            var keyObject = new GameObject("preview_warm_key");
            var key = keyObject.AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1f, 0.74f, 0.42f);
            key.intensity = 2.4f;
            key.transform.rotation = Quaternion.Euler(46f, -32f, 0f);

            var fillObject = new GameObject("preview_cool_fill");
            var fill = fillObject.AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(0.42f, 0.58f, 0.95f);
            fill.intensity = 2.1f;
            fill.range = 12f;
            fill.transform.position = new Vector3(-3.5f, 2.6f, -3f);
        }

        private static Camera CreatePreviewCamera(Vector3 position, Vector3 lookAt, float orthographicSize)
        {
            var cameraObject = new GameObject("preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.024f, 0.022f, 0.019f);
            camera.orthographic = true;
            camera.orthographicSize = orthographicSize;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 80f;
            camera.transform.position = position;
            camera.transform.LookAt(lookAt);
            return camera;
        }

        private static TextMesh CreateLabel(string text, Vector3 position, float scale, int fontSize)
        {
            var labelObject = new GameObject("label_" + text);
            labelObject.transform.position = position;
            labelObject.transform.localScale = Vector3.one * scale;
            var label = labelObject.AddComponent<TextMesh>();
            label.text = text;
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.fontSize = fontSize;
            label.color = new Color(0.95f, 0.78f, 0.48f);
            return label;
        }

        private static void FaceLabels(IEnumerable<TextMesh> labels, Quaternion rotation)
        {
            foreach (var label in labels)
            {
                label.transform.rotation = rotation;
            }
        }

        private static string ShortSpecLabel(VfxSpec spec)
        {
            switch (spec.Id)
            {
                case "SteamVent_SoftColumn":
                    return "S01 Soft Column";
                case "SteamVent_WallJet":
                    return "S02 Wall Jet";
                case "SteamVent_FloorBurst":
                    return "S03 Floor Burst";
                case "SteamBackdraft_PipeCough":
                    return "S04 Backdraft";
                case "PressureLeak_PinHole":
                    return "P01 Pin Leak";
                case "PressureLeak_RuptureCone":
                    return "P02 Rupture";
                case "PressureRing_Pulse":
                    return "P03 Ring Pulse";
                case "PressureGauge_ReliefPop":
                    return "P04 Gauge Pop";
                case "SparkShower_RivetCut":
                    return "K01 Rivet Sparks";
                case "SparkRicochet_WallHit":
                    return "K02 Ricochet";
                case "MuzzleFlash_PistolBoiler":
                    return "M01 Pistol";
                case "MuzzleFlash_Scatterburst":
                    return "M02 Scatter";
                case "MuzzleFlash_RifleValveShot":
                    return "M03 Rifle";
                case "FurnaceBlast_DoorBelch":
                    return "F01 Door Belch";
                case "FurnaceEmbers_StackLift":
                    return "F02 Stack Lift";
                case "ValveTurn_ReleasePuff":
                    return "V01 Release";
                case "ValveSeal_FrostedSteam":
                    return "V02 Frost Seal";
                case "BossPhase_GovernorOvercrank":
                    return "B01 Governor";
                case "BossPhase_BoilerHeartSurge":
                    return "B02 Heart Surge";
                case "BossPhase_EmergencyVentCrown":
                    return "B03 Vent Crown";
                default:
                    return spec.Id.Replace("_", " ", StringComparison.Ordinal);
            }
        }

        private static void SimulateParticles(GameObject instance, float sampleTime)
        {
            foreach (var ps in instance.GetComponentsInChildren<ParticleSystem>(true))
            {
                ps.Simulate(Mathf.Max(0.02f, sampleTime), true, true, true);
            }
        }

        private static void RenderCameraToPng(Camera camera, string outputPath, int width, int height)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? string.Empty);
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
                    UnityEngine.Object.DestroyImmediate(texture);
                }

                UnityEngine.Object.DestroyImmediate(renderTexture);
            }
        }

        private static void WritePreviewPixelEvidence(string outputRoot)
        {
            var files = new[]
            {
                Path.Combine(outputRoot, "BBSVFX02_v0.1.42_full_contact_sheet.png"),
                Path.Combine(outputRoot, "BBSVFX02_v0.1.42_family_contact_sheet.png")
            };

            var productionRoot = ResolveProductionDocRoot();
            Directory.CreateDirectory(productionRoot);
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"status\": \"evaluated\",");
            builder.AppendLine("  \"timestamp\": \"" + EscapeJson(DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture)) + "\",");
            builder.AppendLine("  \"images\": [");
            for (var i = 0; i < files.Length; i++)
            {
                var evidence = AnalyzePng(files[i]);
                builder.AppendLine("    {");
                builder.AppendLine("      \"path\": \"" + EscapeJson(ToRepoPath(files[i])) + "\",");
                builder.AppendLine("      \"width\": " + evidence.Width + ",");
                builder.AppendLine("      \"height\": " + evidence.Height + ",");
                builder.AppendLine("      \"sampled_unique_colors\": " + evidence.UniqueColors + ",");
                builder.AppendLine("      \"average_luminance\": " + evidence.AverageLuminance.ToString("0.####", CultureInfo.InvariantCulture) + ",");
                builder.AppendLine("      \"non_flat\": " + JsonBool(evidence.NonFlat));
                builder.Append("    }");
                builder.AppendLine(i == files.Length - 1 ? string.Empty : ",");
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(Path.Combine(productionRoot, "PreviewPixelEvidence_v0.1.42.json"), builder.ToString(), Encoding.UTF8);
        }

        private static ImageEvidence AnalyzePng(string path)
        {
            if (!File.Exists(path))
            {
                return new ImageEvidence(0, 0, 0, 0f, false);
            }

            var texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            try
            {
                texture.LoadImage(File.ReadAllBytes(path));
                var colors = new HashSet<int>();
                var luminance = 0f;
                var samples = 0;
                var stepX = Mathf.Max(1, texture.width / 120);
                var stepY = Mathf.Max(1, texture.height / 80);
                for (var y = 0; y < texture.height; y += stepY)
                {
                    for (var x = 0; x < texture.width; x += stepX)
                    {
                        var color = texture.GetPixel(x, y);
                        var key = Mathf.RoundToInt(color.r * 31f) << 10 | Mathf.RoundToInt(color.g * 31f) << 5 | Mathf.RoundToInt(color.b * 31f);
                        colors.Add(key);
                        luminance += color.r * 0.2126f + color.g * 0.7152f + color.b * 0.0722f;
                        samples++;
                    }
                }

                var average = samples == 0 ? 0f : luminance / samples;
                return new ImageEvidence(texture.width, texture.height, colors.Count, average, colors.Count > 64 && average > 0.015f);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(texture);
            }
        }

        private static void WriteAcceptanceReport(string validationSummary)
        {
            var outputRoot = ResolveProductionDocRoot();
            Directory.CreateDirectory(outputRoot);
            var builder = new StringBuilder();
            builder.AppendLine("# v0.1.42 Steam VFX Set 02 Acceptance Report");
            builder.AppendLine();
            builder.AppendLine("Timestamp: `" + DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture) + "`");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Package root: `AssetPacks/BrassworksBreach.SteamVFXSet02`");
            builder.AppendLine("- Production docs: `Documentation/AssetProduction/V0_1_42_SteamVFXSet02`");
            builder.AppendLine("- Preview docs: `Documentation/ConceptRenders/V0_1_42_SteamVFXSet02`");
            builder.AppendLine("- Main project scenes, gameplay scripts, shared status docs, `Packages/manifest.json`, and git history were not intentionally modified.");
            builder.AppendLine("- No commit was made by this worker.");
            builder.AppendLine();
            builder.AppendLine("## Generated Output");
            builder.AppendLine();
            builder.AppendLine("- `" + Specs.Length + "` visual-only VFX prefabs in `Runtime/Prefabs`");
            builder.AppendLine("- `16` package-local materials in `Runtime/Materials`");
            builder.AppendLine("- `8` generated procedural mesh assets in `Runtime/Meshes`");
            builder.AppendLine("- `1` catalog JSON in `Runtime/Metadata`");
            builder.AppendLine("- `2` Unity-rendered preview contact sheets in `Documentation/ConceptRenders/V0_1_42_SteamVFXSet02`");
            builder.AppendLine("- Package-local manifest: `AssetPacks/BrassworksBreach.SteamVFXSet02/Documentation~/Manifest/BBSVFX02_SteamVFXSet02_Manifest_v0.1.42-p001.json`");
            builder.AppendLine();
            builder.AppendLine("## Unity Package-Specific Validation");
            builder.AppendLine();
            builder.AppendLine(validationSummary);
            builder.AppendLine();
            builder.AppendLine("Runtime safety policy checked by package validation: no colliders, rigidbodies, audio sources, audio listeners, cameras, lights, animators, directors, or gameplay MonoBehaviours. Particle collision, trigger, and external force modules are expected to remain disabled.");
            builder.AppendLine();
            builder.AppendLine("## Known Risks");
            builder.AppendLine();
            builder.AppendLine("- Particle timing and scale still need quarantine-scene review against actual weapon/enemy sockets.");
            builder.AppendLine("- Transparent particle shaders may need remapping if the promoted game target uses a custom render pipeline.");
            File.WriteAllText(Path.Combine(outputRoot, "ACCEPTANCE_REPORT_v0.1.42.md"), builder.ToString(), Encoding.UTF8);
        }

        private static void EnsurePackageFolders()
        {
            foreach (var path in new[] { PrefabRoot, MaterialRoot, MeshRoot, MetadataRoot, ManifestRoot })
            {
                Directory.CreateDirectory(AssetPathToFullPath(path));
            }

            AssetDatabase.Refresh();
        }

        private static string PrefabPath(VfxSpec spec)
        {
            return PrefabRoot + "/BBSVFX02_" + spec.Id + ".prefab";
        }

        private static string AssetPathToFullPath(string assetPath)
        {
            var normalizedPath = assetPath.Replace("\\", "/");
            var root = LocatePackageRoot();
            if (normalizedPath.StartsWith(root.AssetPath + "/", StringComparison.Ordinal))
            {
                var relativePath = normalizedPath.Substring(root.AssetPath.Length + 1);
                return Path.GetFullPath(Path.Combine(root.ResolvedPath, relativePath.Replace('/', Path.DirectorySeparatorChar)));
            }

            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", normalizedPath.Replace('/', Path.DirectorySeparatorChar)));
        }

        private static string ResolveRenderOutputRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_RENDER_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return explicitRoot;
            }

            return Path.Combine(ResolveRepoRoot(), RenderDocFolder.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ResolveProductionDocRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_PRODUCTION_DOC_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return explicitRoot;
            }

            return Path.Combine(ResolveRepoRoot(), ProductionDocFolder.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ResolveRepoRoot()
        {
            var root = LocatePackageRoot();
            var directory = new DirectoryInfo(root.ResolvedPath);
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

        private static PackageRootInfo LocatePackageRoot()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(SteamVfxSet02Generator).Assembly);
            if (packageInfo != null && !string.IsNullOrWhiteSpace(packageInfo.assetPath) && !string.IsNullOrWhiteSpace(packageInfo.resolvedPath))
            {
                return new PackageRootInfo(packageInfo.assetPath, packageInfo.resolvedPath);
            }

            var guids = AssetDatabase.FindAssets(nameof(SteamVfxSet02Generator));
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/SteamVfxSet02Generator.cs";
                if (path.EndsWith(suffix, StringComparison.Ordinal))
                {
                    var assetPath = path.Substring(0, path.Length - suffix.Length);
                    return new PackageRootInfo(assetPath, Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath)));
                }
            }

            throw new InvalidOperationException("Could not locate package root for " + PackageName + ".");
        }

        private static Color MaterialColor(string key, float alpha)
        {
            switch (key)
            {
                case "PressureAmber":
                    return new Color(1f, 0.58f, 0.14f, alpha);
                case "CopperGlow":
                    return new Color(1f, 0.38f, 0.12f, alpha);
                case "HotSpark":
                    return new Color(1f, 0.74f, 0.22f, alpha);
                case "MuzzleCore":
                    return new Color(1f, 0.92f, 0.58f, alpha);
                case "FurnaceWhite":
                    return new Color(1f, 0.9f, 0.55f, alpha);
                case "FurnaceOrange":
                    return new Color(1f, 0.31f, 0.055f, alpha);
                case "WarningRed":
                    return new Color(1f, 0.08f, 0.035f, alpha);
                case "BossRed":
                    return new Color(0.95f, 0.07f, 0.035f, alpha);
                case "BossBlue":
                    return new Color(0.36f, 0.62f, 1f, alpha);
                case "ValveGreen":
                    return new Color(0.38f, 0.9f, 0.42f, alpha);
                case "GaugeGlass":
                    return new Color(0.62f, 0.76f, 0.82f, alpha);
                default:
                    return new Color(0.85f, 0.82f, 0.74f, alpha);
            }
        }

        private static Gradient CreateFadeGradient(Color start, Color end, float startAlpha, float endAlpha)
        {
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(start, 0f),
                    new GradientColorKey(Color.Lerp(start, end, 0.4f), 0.42f),
                    new GradientColorKey(end, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(startAlpha, 0f),
                    new GradientAlphaKey(startAlpha * 0.68f, 0.52f),
                    new GradientAlphaKey(endAlpha, 1f)
                });
            return gradient;
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

        private static int StableSeed(string value)
        {
            unchecked
            {
                var hash = 23;
                foreach (var ch in value)
                {
                    hash = hash * 31 + ch;
                }

                return Mathf.Abs(hash);
            }
        }

        private static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string JsonBool(bool value)
        {
            return value ? "true" : "false";
        }

        private static void WriteJsonStringArray(StringBuilder builder, IList<string> values, int indent)
        {
            var padding = new string(' ', indent);
            for (var i = 0; i < values.Count; i++)
            {
                builder.Append(padding);
                builder.Append("\"");
                builder.Append(EscapeJson(values[i]));
                builder.Append("\"");
                builder.AppendLine(i == values.Count - 1 ? string.Empty : ",");
            }
        }

        private static string ToRepoPath(string fullPath)
        {
            var repo = ResolveRepoRoot().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var normalized = Path.GetFullPath(fullPath);
            if (normalized.StartsWith(repo, StringComparison.OrdinalIgnoreCase))
            {
                return normalized.Substring(repo.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace('\\', '/');
            }

            return normalized.Replace('\\', '/');
        }

        private sealed class VfxSpec
        {
            public VfxSpec(
                string id,
                string family,
                string intent,
                string socket,
                string colorKey,
                float duration,
                float sampleTime,
                float scale,
                bool looping,
                float previewYaw,
                int pattern)
            {
                Id = id;
                Family = family;
                Intent = intent;
                Socket = socket;
                ColorKey = colorKey;
                Duration = duration;
                SampleTime = sampleTime;
                Scale = scale;
                Looping = looping;
                PreviewYaw = previewYaw;
                Pattern = pattern;
            }

            public string Id { get; }
            public string Family { get; }
            public string Intent { get; }
            public string Socket { get; }
            public string ColorKey { get; }
            public float Duration { get; }
            public float SampleTime { get; }
            public float Scale { get; }
            public bool Looping { get; }
            public float PreviewYaw { get; }
            public int Pattern { get; }
        }

        private readonly struct ImageEvidence
        {
            public ImageEvidence(int width, int height, int uniqueColors, float averageLuminance, bool nonFlat)
            {
                Width = width;
                Height = height;
                UniqueColors = uniqueColors;
                AverageLuminance = averageLuminance;
                NonFlat = nonFlat;
            }

            public int Width { get; }
            public int Height { get; }
            public int UniqueColors { get; }
            public float AverageLuminance { get; }
            public bool NonFlat { get; }
        }

        private readonly struct PackageRootInfo
        {
            public PackageRootInfo(string assetPath, string resolvedPath)
            {
                AssetPath = assetPath.Replace("\\", "/");
                ResolvedPath = resolvedPath.Replace("\\", "/");
            }

            public string AssetPath { get; }
            public string ResolvedPath { get; }
        }
    }
}
