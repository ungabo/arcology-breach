using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BrassworksBreach.FeedbackFXAudio;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.FeedbackFXAudio.Editor
{
    public static class FeedbackFxAudioSidecarGenerator
    {
        private const string Version = "v0.1.38";
        private const string BuildId = "p001";
        private const string PackageName = "com.brassworks.sidecar.feedback-fx-audio";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string PrefabRoot = PackageRoot + "/Runtime/Prefabs";
        private const string MaterialRoot = PackageRoot + "/Runtime/Materials";
        private const string AudioRoot = PackageRoot + "/Runtime/Audio";
        private const string MetadataRoot = PackageRoot + "/Runtime/Metadata";
        private const string MeshRoot = PackageRoot + "/Runtime/Meshes";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_38_FeedbackFXAudioSidecar";

        private static readonly EventSpec[] Specs =
        {
            new EventSpec("WeaponFired", "weapon", "Pressure muzzle flash, copper coil flare, and forward steam ring.", "Brass clack with a short pressure bark.", "Amber", 1.0f, true, 0.30f, 156f, 0),
            new EventSpec("WeaponImpact", "weapon", "Rivet spark impact and steam splash on contact.", "Metal tick, low thud, and fast steam hiss.", "Copper", 0.95f, true, 0.34f, 118f, 1),
            new EventSpec("WeaponEmpty", "weapon", "Dry pressure gauge and red valve stutter.", "Hollow dry click with a falling gauge tick.", "Warning", 0.78f, false, 0.24f, 192f, 2),
            new EventSpec("WeaponSwitched", "weapon", "Selector gear snaps between pistol and scattergun positions.", "Ratchet turn with brass latch settle.", "Brass", 0.82f, false, 0.36f, 132f, 3),
            new EventSpec("PickupCollected", "pickup", "Cartridge lift, amber glint, and small intake puff.", "Bright gauge chime with soft valve inhale.", "Green", 0.82f, true, 0.38f, 240f, 4),
            new EventSpec("EnemyHit", "combat", "Hot rivet spark and dented black-iron hit marker.", "Sharp hammer ping and brief pressure leak.", "Orange", 1.0f, true, 0.28f, 175f, 5),
            new EventSpec("EnemyDeath", "combat", "Gear rupture, broken boiler cap, and heavy steam bloom.", "Boiler thunk with gear scatter and vent release.", "Warning", 1.35f, true, 0.62f, 86f, 6),
            new EventSpec("ObjectiveUpdated", "objective", "Gauge needle tick and brass task plaque pulse.", "Measured clock tick with tiny steam tick.", "Blue", 0.74f, false, 0.32f, 205f, 7),
            new EventSpec("ObjectiveCompleted", "objective", "Valve wheel opens into green lamp completion glow.", "Valve thunk with rising brass chime.", "Green", 1.0f, true, 0.54f, 262f, 8),
            new EventSpec("RouteBlocked", "navigation", "Pressure-locked red bulkhead mark with crossed pipes.", "Heavy lock thunk and blocked steam cough.", "Warning", 1.1f, true, 0.45f, 92f, 9),
            new EventSpec("SecretFound", "navigation", "Hidden cyan gauge aperture and reveal shimmer.", "Glass chime, soft ratchet, and secret steam whisper.", "Cyan", 0.92f, true, 0.58f, 311f, 10),
            new EventSpec("PauseOpened", "ui", "Brass menu shutters open around a pressure dial.", "Soft panel slide with clockwork stop.", "Brass", 0.70f, false, 0.28f, 147f, 11),
            new EventSpec("PauseClosed", "ui", "Brass shutters close and extinguish the menu glow.", "Panel close, latch, and short air release.", "Iron", 0.70f, false, 0.26f, 124f, 12),
            new EventSpec("SettingChanged", "ui", "Toggle lever clicks over a small calibration gauge.", "Small toggle click with a clean gauge tick.", "Blue", 0.66f, false, 0.22f, 220f, 13),
            new EventSpec("BossPhaseChanged", "boss", "Governor alarm gear with red/amber phase lamps and vent surge.", "Large warning bell, furnace swell, and pressure vent.", "Boss", 1.55f, true, 0.78f, 72f, 14)
        };

        [MenuItem("Brassworks Breach/Sidecars/Feedback FX Audio/Generate v0.1.38 Package")]
        public static void GenerateFeedbackPackage()
        {
            EnsurePackageFolders();
            var materials = CreateMaterials();
            var meshes = CreateMeshes();

            foreach (var spec in Specs)
            {
                var prefab = BuildCuePrefab(spec, materials, meshes);
                SavePrefab(prefab, PrefabPath(spec));
                WriteCueWav(spec, AudioPath(spec));
            }

            WriteCueCatalog();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("SCFX v0.1.38 feedback FX audio package generated: 15 prefabs, 12 materials, 3 meshes, 15 WAV cues.");
        }

        [MenuItem("Brassworks Breach/Sidecars/Feedback FX Audio/Render v0.1.38 Contact Sheets")]
        public static void RenderContactSheets()
        {
            GenerateFeedbackPackage();

            var outputRoot = ResolveRenderOutputRoot();
            Directory.CreateDirectory(outputRoot);
            RenderVisualContactSheet(outputRoot);
            RenderAudioCueSheet(outputRoot);

            AssetDatabase.Refresh();
            Debug.Log("SCFX v0.1.38 contact sheets rendered to: " + outputRoot);
        }

        [MenuItem("Brassworks Breach/Sidecars/Feedback FX Audio/Generate and Render v0.1.38")]
        public static void GenerateAllAndRenderPreview()
        {
            RenderContactSheets();
        }

        private static GameObject BuildCuePrefab(EventSpec spec, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var root = new GameObject("SCFX_EVT_" + spec.EventType);
            root.AddComponent<FeedbackFxAudioCueIdentity>().Configure(
                spec.EventType,
                "SCFX_" + spec.EventType,
                spec.Family,
                spec.VisualIntent,
                spec.AudioIntent,
                spec.WorldScale,
                spec.PrefersWorldPosition);

            var visualRoot = CreateChild(root.transform, "VisualRoot");
            var sockets = CreateChild(root.transform, "Sockets");
            var metadata = CreateChild(root.transform, "Metadata");
            metadata.gameObject.SetActive(false);

            CreateSocket(sockets, "SOCK_EventOrigin", Vector3.zero);
            CreateSocket(sockets, "SOCK_AudioEmitter", new Vector3(0f, 0.3f, -0.15f));
            CreateSocket(sockets, "SOCK_PrimaryGlow", new Vector3(0f, 0.42f, -0.08f));
            CreateSocket(sockets, "SOCK_SteamVent", new Vector3(0f, 0.72f, 0.08f));

            CreateMeshPart("Clockwork_Halo", meshes["GearHalo"], visualRoot, new Vector3(0f, 0.42f, 0f), Vector3.one * 0.72f, Quaternion.Euler(0f, 180f, 0f), materials["Brass"]);
            CreateMeshPart("Signal_Backplate", meshes["SignalPlate"], visualRoot, new Vector3(0f, 0.42f, 0.02f), Vector3.one * 0.62f, Quaternion.Euler(0f, 180f, 0f), materials["Iron"]);
            CreatePrimitivePart("Event_Glow_Lens_" + spec.MaterialKey, PrimitiveType.Sphere, visualRoot, new Vector3(0f, 0.42f, -0.08f), new Vector3(0.18f, 0.18f, 0.07f), materials[spec.MaterialKey]);
            CreateSteamPuffs(visualRoot, materials["Steam"], spec.WorldScale);

            switch (spec.EventType)
            {
                case "WeaponFired":
                    AddWeaponFiredMotif(visualRoot, materials, meshes);
                    break;
                case "WeaponImpact":
                    AddImpactMotif(visualRoot, materials, meshes);
                    break;
                case "WeaponEmpty":
                    AddGaugeMotif(visualRoot, materials, "Warning", -42f);
                    break;
                case "WeaponSwitched":
                    AddSelectorMotif(visualRoot, materials);
                    break;
                case "PickupCollected":
                    AddPickupMotif(visualRoot, materials);
                    break;
                case "EnemyHit":
                    AddImpactMotif(visualRoot, materials, meshes);
                    AddRivetSparkFan(visualRoot, materials["Orange"]);
                    break;
                case "EnemyDeath":
                    AddRuptureMotif(visualRoot, materials, meshes);
                    break;
                case "ObjectiveUpdated":
                    AddGaugeMotif(visualRoot, materials, "Blue", 18f);
                    break;
                case "ObjectiveCompleted":
                    AddValveMotif(visualRoot, materials, "Green");
                    break;
                case "RouteBlocked":
                    AddRouteBlockedMotif(visualRoot, materials);
                    break;
                case "SecretFound":
                    AddSecretMotif(visualRoot, materials);
                    break;
                case "PauseOpened":
                    AddPauseMotif(visualRoot, materials, true);
                    break;
                case "PauseClosed":
                    AddPauseMotif(visualRoot, materials, false);
                    break;
                case "SettingChanged":
                    AddSettingMotif(visualRoot, materials);
                    break;
                case "BossPhaseChanged":
                    AddBossPhaseMotif(visualRoot, materials, meshes);
                    break;
            }

            root.transform.localScale = Vector3.one * spec.WorldScale;
            return root;
        }

        private static void AddWeaponFiredMotif(Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreatePrimitivePart("Pressure_Barrel", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.25f, -0.42f), new Vector3(0.12f, 0.64f, 0.12f), materials["Iron"], Quaternion.Euler(90f, 0f, 0f));
            CreatePrimitivePart("Copper_Coil_Left", PrimitiveType.Cylinder, parent, new Vector3(-0.18f, 0.25f, -0.25f), new Vector3(0.035f, 0.5f, 0.035f), materials["Copper"], Quaternion.Euler(90f, 0f, 0f));
            CreatePrimitivePart("Copper_Coil_Right", PrimitiveType.Cylinder, parent, new Vector3(0.18f, 0.25f, -0.25f), new Vector3(0.035f, 0.5f, 0.035f), materials["Copper"], Quaternion.Euler(90f, 0f, 0f));
            CreateMeshPart("Amber_Muzzle_Burst", meshes["Burst"], parent, new Vector3(0f, 0.25f, -0.78f), Vector3.one * 0.52f, Quaternion.Euler(0f, 180f, 0f), materials["Amber"]);
        }

        private static void AddImpactMotif(Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreatePrimitivePart("Dented_Impact_Plate", PrimitiveType.Cube, parent, new Vector3(0f, 0.12f, -0.18f), new Vector3(0.72f, 0.12f, 0.08f), materials["Iron"]);
            CreateMeshPart("Impact_Spark_Burst", meshes["Burst"], parent, new Vector3(0f, 0.25f, -0.32f), Vector3.one * 0.48f, Quaternion.Euler(0f, 180f, 18f), materials["Orange"]);
            AddRivetSparkFan(parent, materials["Copper"]);
        }

        private static void AddGaugeMotif(Transform parent, Dictionary<string, Material> materials, string materialKey, float needleAngle)
        {
            CreatePrimitivePart("Gauge_Brass_Rim", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.25f, -0.25f), new Vector3(0.33f, 0.035f, 0.33f), materials["Brass"], Quaternion.Euler(90f, 0f, 0f));
            CreatePrimitivePart("Gauge_Glass_Face", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.25f, -0.29f), new Vector3(0.26f, 0.018f, 0.26f), materials["Glass"], Quaternion.Euler(90f, 0f, 0f));
            var needle = CreatePrimitivePart("Gauge_Needle_" + materialKey, PrimitiveType.Cube, parent, new Vector3(0f, 0.25f, -0.32f), new Vector3(0.035f, 0.24f, 0.025f), materials[materialKey]);
            needle.transform.localRotation = Quaternion.Euler(0f, 0f, needleAngle);
            CreatePrimitivePart("Gauge_Pivot", PrimitiveType.Sphere, parent, new Vector3(0f, 0.25f, -0.34f), new Vector3(0.06f, 0.06f, 0.035f), materials["Brass"]);
        }

        private static void AddSelectorMotif(Transform parent, Dictionary<string, Material> materials)
        {
            CreatePrimitivePart("Selector_Dial", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.18f, -0.22f), new Vector3(0.36f, 0.05f, 0.36f), materials["Brass"], Quaternion.Euler(90f, 0f, 0f));
            CreatePrimitivePart("Pistol_Index_Mark", PrimitiveType.Cube, parent, new Vector3(-0.18f, 0.18f, -0.32f), new Vector3(0.24f, 0.04f, 0.05f), materials["Amber"]);
            CreatePrimitivePart("Scattergun_Index_Mark", PrimitiveType.Cube, parent, new Vector3(0.18f, 0.18f, -0.32f), new Vector3(0.24f, 0.04f, 0.05f), materials["Copper"]);
            CreatePrimitivePart("Selector_Latch", PrimitiveType.Cube, parent, new Vector3(0f, 0.47f, -0.28f), new Vector3(0.12f, 0.3f, 0.06f), materials["Iron"]);
        }

        private static void AddPickupMotif(Transform parent, Dictionary<string, Material> materials)
        {
            CreatePrimitivePart("Pressure_Cartridge_Body", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.18f, -0.18f), new Vector3(0.16f, 0.48f, 0.16f), materials["Copper"], Quaternion.Euler(0f, 0f, 90f));
            CreatePrimitivePart("Cartridge_Glass_Core", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.18f, -0.19f), new Vector3(0.09f, 0.5f, 0.09f), materials["Green"], Quaternion.Euler(0f, 0f, 90f));
            CreatePrimitivePart("Pickup_Glint", PrimitiveType.Sphere, parent, new Vector3(0f, 0.64f, -0.22f), new Vector3(0.12f, 0.12f, 0.12f), materials["Green"]);
        }

        private static void AddRuptureMotif(Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreatePrimitivePart("Broken_Boiler_Core", PrimitiveType.Sphere, parent, new Vector3(0f, 0.16f, -0.18f), new Vector3(0.36f, 0.26f, 0.28f), materials["Iron"]);
            CreateMeshPart("Ruptured_Gear_Left", meshes["GearHalo"], parent, new Vector3(-0.28f, 0.46f, -0.3f), Vector3.one * 0.42f, Quaternion.Euler(12f, 180f, -28f), materials["Brass"]);
            CreateMeshPart("Ruptured_Gear_Right", meshes["GearHalo"], parent, new Vector3(0.3f, 0.32f, -0.34f), Vector3.one * 0.36f, Quaternion.Euler(-8f, 180f, 34f), materials["Copper"]);
            CreatePrimitivePart("Death_Vent_Plume", PrimitiveType.Sphere, parent, new Vector3(0f, 0.82f, -0.22f), new Vector3(0.58f, 0.34f, 0.2f), materials["Steam"]);
        }

        private static void AddValveMotif(Transform parent, Dictionary<string, Material> materials, string materialKey)
        {
            CreatePrimitivePart("Valve_Stem", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.21f, -0.24f), new Vector3(0.06f, 0.18f, 0.06f), materials["Iron"], Quaternion.Euler(90f, 0f, 0f));
            CreatePrimitivePart("Valve_Wheel_Vertical", PrimitiveType.Cube, parent, new Vector3(0f, 0.21f, -0.35f), new Vector3(0.08f, 0.58f, 0.05f), materials["Brass"]);
            CreatePrimitivePart("Valve_Wheel_Horizontal", PrimitiveType.Cube, parent, new Vector3(0f, 0.21f, -0.35f), new Vector3(0.58f, 0.08f, 0.05f), materials["Brass"]);
            CreatePrimitivePart("Completion_Lamp", PrimitiveType.Sphere, parent, new Vector3(0f, 0.62f, -0.28f), new Vector3(0.16f, 0.16f, 0.08f), materials[materialKey]);
        }

        private static void AddRouteBlockedMotif(Transform parent, Dictionary<string, Material> materials)
        {
            CreatePrimitivePart("Blocked_Bulkhead", PrimitiveType.Cube, parent, new Vector3(0f, 0.2f, -0.18f), new Vector3(0.82f, 0.56f, 0.08f), materials["Iron"]);
            CreatePrimitivePart("Pressure_Lock_Bar_A", PrimitiveType.Cube, parent, new Vector3(0f, 0.2f, -0.28f), new Vector3(0.9f, 0.08f, 0.06f), materials["Warning"], Quaternion.Euler(0f, 0f, 32f));
            CreatePrimitivePart("Pressure_Lock_Bar_B", PrimitiveType.Cube, parent, new Vector3(0f, 0.2f, -0.29f), new Vector3(0.9f, 0.08f, 0.06f), materials["Warning"], Quaternion.Euler(0f, 0f, -32f));
            CreatePrimitivePart("Blocked_Status_Lamp", PrimitiveType.Sphere, parent, new Vector3(0f, 0.55f, -0.31f), new Vector3(0.12f, 0.12f, 0.06f), materials["Warning"]);
        }

        private static void AddSecretMotif(Transform parent, Dictionary<string, Material> materials)
        {
            CreatePrimitivePart("Secret_Aperture_Door_Left", PrimitiveType.Cube, parent, new Vector3(-0.18f, 0.24f, -0.18f), new Vector3(0.22f, 0.46f, 0.06f), materials["Iron"], Quaternion.Euler(0f, 0f, 14f));
            CreatePrimitivePart("Secret_Aperture_Door_Right", PrimitiveType.Cube, parent, new Vector3(0.18f, 0.24f, -0.18f), new Vector3(0.22f, 0.46f, 0.06f), materials["Iron"], Quaternion.Euler(0f, 0f, -14f));
            CreatePrimitivePart("Cyan_Hidden_Lens", PrimitiveType.Sphere, parent, new Vector3(0f, 0.28f, -0.31f), new Vector3(0.18f, 0.18f, 0.08f), materials["Cyan"]);
            CreatePrimitivePart("Secret_Ray", PrimitiveType.Cube, parent, new Vector3(0f, 0.52f, -0.33f), new Vector3(0.05f, 0.44f, 0.04f), materials["Cyan"], Quaternion.Euler(0f, 0f, 42f));
        }

        private static void AddPauseMotif(Transform parent, Dictionary<string, Material> materials, bool opened)
        {
            var offset = opened ? 0.28f : 0.08f;
            CreatePrimitivePart("Menu_Shutter_Left", PrimitiveType.Cube, parent, new Vector3(-offset, 0.25f, -0.18f), new Vector3(0.34f, 0.58f, 0.06f), materials["Brass"]);
            CreatePrimitivePart("Menu_Shutter_Right", PrimitiveType.Cube, parent, new Vector3(offset, 0.25f, -0.18f), new Vector3(0.34f, 0.58f, 0.06f), materials["Brass"]);
            CreatePrimitivePart("Pause_Dial", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.25f, -0.29f), new Vector3(0.18f, 0.035f, 0.18f), opened ? materials["Blue"] : materials["Iron"], Quaternion.Euler(90f, 0f, 0f));
            CreatePrimitivePart(opened ? "Pause_Open_Latch" : "Pause_Close_Latch", PrimitiveType.Cube, parent, new Vector3(0f, 0.58f, -0.24f), new Vector3(0.42f, 0.07f, 0.05f), materials["Iron"]);
        }

        private static void AddSettingMotif(Transform parent, Dictionary<string, Material> materials)
        {
            CreatePrimitivePart("Setting_Backplate", PrimitiveType.Cube, parent, new Vector3(0f, 0.22f, -0.18f), new Vector3(0.72f, 0.36f, 0.06f), materials["Iron"]);
            CreatePrimitivePart("Toggle_Rail", PrimitiveType.Cube, parent, new Vector3(0f, 0.22f, -0.28f), new Vector3(0.52f, 0.06f, 0.05f), materials["Brass"]);
            CreatePrimitivePart("Toggle_Lever", PrimitiveType.Cylinder, parent, new Vector3(0.12f, 0.35f, -0.33f), new Vector3(0.035f, 0.26f, 0.035f), materials["Blue"], Quaternion.Euler(0f, 0f, -28f));
            CreatePrimitivePart("Calibration_Tick", PrimitiveType.Sphere, parent, new Vector3(0.28f, 0.22f, -0.33f), new Vector3(0.08f, 0.08f, 0.04f), materials["Blue"]);
        }

        private static void AddBossPhaseMotif(Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            CreateMeshPart("Governor_Alarm_Gear", meshes["GearHalo"], parent, new Vector3(0f, 0.31f, -0.18f), Vector3.one * 1.05f, Quaternion.Euler(0f, 180f, 0f), materials["Brass"]);
            CreatePrimitivePart("Boss_Red_Lamp_Left", PrimitiveType.Sphere, parent, new Vector3(-0.3f, 0.52f, -0.32f), new Vector3(0.17f, 0.17f, 0.08f), materials["Warning"]);
            CreatePrimitivePart("Boss_Amber_Lamp_Right", PrimitiveType.Sphere, parent, new Vector3(0.3f, 0.52f, -0.32f), new Vector3(0.17f, 0.17f, 0.08f), materials["Amber"]);
            CreatePrimitivePart("Phase_Pressure_Bell", PrimitiveType.Cylinder, parent, new Vector3(0f, -0.05f, -0.2f), new Vector3(0.36f, 0.22f, 0.36f), materials["Copper"], Quaternion.Euler(90f, 0f, 0f));
            CreatePrimitivePart("Boss_Steam_Surge", PrimitiveType.Sphere, parent, new Vector3(0f, 0.92f, -0.22f), new Vector3(0.72f, 0.32f, 0.18f), materials["Steam"]);
        }

        private static void AddRivetSparkFan(Transform parent, Material material)
        {
            for (var i = 0; i < 5; i++)
            {
                var angle = -52f + i * 26f;
                var spark = CreatePrimitivePart("Rivet_Spark_" + i, PrimitiveType.Cube, parent, new Vector3((i - 2) * 0.08f, 0.34f + i * 0.02f, -0.38f), new Vector3(0.035f, 0.26f, 0.035f), material);
                spark.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private static void CreateSteamPuffs(Transform parent, Material steam, float scale)
        {
            for (var i = 0; i < 3; i++)
            {
                var x = (i - 1) * 0.18f;
                var y = 0.67f + i * 0.08f;
                var puff = CreatePrimitivePart("Steam_Puff_" + i, PrimitiveType.Sphere, parent, new Vector3(x, y, -0.14f), new Vector3(0.22f, 0.14f, 0.1f) * Mathf.Max(0.8f, scale), steam);
                puff.transform.localRotation = Quaternion.Euler(0f, 0f, i * 19f);
            }
        }

        private static Dictionary<string, Material> CreateMaterials()
        {
            return new Dictionary<string, Material>
            {
                ["Brass"] = UpsertMaterial("SCFX_MAT_AgedBrass", new Color(0.72f, 0.48f, 0.22f, 1f), 0.82f, 0.36f, Color.black, false),
                ["Copper"] = UpsertMaterial("SCFX_MAT_BurnishedCopper", new Color(0.78f, 0.33f, 0.16f, 1f), 0.88f, 0.42f, Color.black, false),
                ["Iron"] = UpsertMaterial("SCFX_MAT_BlackenedIron", new Color(0.055f, 0.05f, 0.045f, 1f), 0.75f, 0.24f, Color.black, false),
                ["Amber"] = UpsertMaterial("SCFX_MAT_AmberPressureGlow", new Color(1f, 0.54f, 0.09f, 1f), 0.05f, 0.25f, new Color(1f, 0.42f, 0.08f, 1f) * 2.4f, false),
                ["Orange"] = UpsertMaterial("SCFX_MAT_HotRivetSpark", new Color(1f, 0.25f, 0.05f, 1f), 0.02f, 0.22f, new Color(1f, 0.18f, 0.05f, 1f) * 2.6f, false),
                ["Warning"] = UpsertMaterial("SCFX_MAT_RedPressureWarning", new Color(0.95f, 0.06f, 0.03f, 1f), 0.02f, 0.18f, new Color(1f, 0.04f, 0.02f, 1f) * 2.9f, false),
                ["Green"] = UpsertMaterial("SCFX_MAT_ValveGreenComplete", new Color(0.28f, 0.95f, 0.48f, 1f), 0.02f, 0.22f, new Color(0.18f, 0.9f, 0.35f, 1f) * 2.2f, false),
                ["Blue"] = UpsertMaterial("SCFX_MAT_GaugeBlueUpdate", new Color(0.27f, 0.66f, 1f, 1f), 0.02f, 0.24f, new Color(0.1f, 0.38f, 1f, 1f) * 1.9f, false),
                ["Cyan"] = UpsertMaterial("SCFX_MAT_SecretCyanReveal", new Color(0.2f, 0.95f, 1f, 1f), 0.02f, 0.18f, new Color(0.1f, 0.85f, 1f, 1f) * 2.5f, false),
                ["Boss"] = UpsertMaterial("SCFX_MAT_BossPhaseFurnace", new Color(1f, 0.22f, 0.05f, 1f), 0.05f, 0.2f, new Color(1f, 0.12f, 0.02f, 1f) * 3.1f, false),
                ["Steam"] = UpsertMaterial("SCFX_MAT_TranslucentSteam", new Color(0.78f, 0.72f, 0.62f, 0.38f), 0f, 0.88f, new Color(0.25f, 0.22f, 0.18f, 1f), true),
                ["Glass"] = UpsertMaterial("SCFX_MAT_SootedGaugeGlass", new Color(0.35f, 0.42f, 0.36f, 0.48f), 0f, 0.72f, Color.black, true)
            };
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            return new Dictionary<string, Mesh>
            {
                ["GearHalo"] = UpsertMesh("SCFX_MESH_GearHalo_20Tooth", CreateRingMesh("SCFX_MESH_GearHalo_20Tooth", 40, 0.46f, 0.62f, true)),
                ["SignalPlate"] = UpsertMesh("SCFX_MESH_BevelSignalPlate", CreateRingMesh("SCFX_MESH_BevelSignalPlate", 32, 0.0f, 0.42f, false)),
                ["Burst"] = UpsertMesh("SCFX_MESH_RadialPressureBurst", CreateBurstMesh("SCFX_MESH_RadialPressureBurst", 18, 0.12f, 0.58f))
            };
        }

        private static Material UpsertMaterial(string name, Color color, float metallic, float smoothness, Color emission, bool transparent)
        {
            var path = MaterialRoot + "/" + name + ".mat";
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Sprites/Default");
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            SetColor(material, "_BaseColor", color);
            SetColor(material, "_Color", color);
            SetFloat(material, "_Metallic", metallic);
            SetFloat(material, "_Smoothness", smoothness);
            SetFloat(material, "_Glossiness", smoothness);

            if (emission.maxColorComponent > 0.001f)
            {
                material.EnableKeyword("_EMISSION");
                SetColor(material, "_EmissionColor", emission);
            }
            else
            {
                material.DisableKeyword("_EMISSION");
            }

            if (transparent)
            {
                SetFloat(material, "_Surface", 1f);
                SetFloat(material, "_Mode", 3f);
                SetFloat(material, "_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                SetFloat(material, "_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = 3000;
            }
            else
            {
                SetFloat(material, "_Surface", 0f);
                SetFloat(material, "_Mode", 0f);
                SetFloat(material, "_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                SetFloat(material, "_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
                material.DisableKeyword("_ALPHABLEND_ON");
                material.renderQueue = -1;
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static Mesh UpsertMesh(string name, Mesh mesh)
        {
            var path = MeshRoot + "/" + name + ".asset";
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing == null)
            {
                AssetDatabase.CreateAsset(mesh, path);
                return mesh;
            }

            EditorUtility.CopySerialized(mesh, existing);
            existing.name = name;
            EditorUtility.SetDirty(existing);
            return existing;
        }

        private static Mesh CreateRingMesh(string name, int segments, float innerRadius, float outerRadius, bool gearTeeth)
        {
            if (innerRadius <= 0.001f)
            {
                var vertices = new Vector3[segments + 1];
                var triangles = new int[segments * 3];
                vertices[0] = Vector3.zero;
                for (var i = 0; i < segments; i++)
                {
                    var radius = gearTeeth && i % 2 == 1 ? outerRadius * 0.86f : outerRadius;
                    var angle = Mathf.PI * 2f * i / segments;
                    vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
                }

                var t = 0;
                for (var i = 0; i < segments; i++)
                {
                    triangles[t++] = 0;
                    triangles[t++] = i + 1;
                    triangles[t++] = (i + 1) % segments + 1;
                }

                return FinalizeMesh(name, vertices, triangles);
            }

            var ringVertices = new Vector3[segments * 2];
            var ringTriangles = new int[segments * 6];
            for (var i = 0; i < segments; i++)
            {
                var angle = Mathf.PI * 2f * i / segments;
                var outer = gearTeeth && i % 2 == 1 ? outerRadius * 0.84f : outerRadius;
                ringVertices[i * 2] = new Vector3(Mathf.Cos(angle) * innerRadius, Mathf.Sin(angle) * innerRadius, 0f);
                ringVertices[i * 2 + 1] = new Vector3(Mathf.Cos(angle) * outer, Mathf.Sin(angle) * outer, 0f);
            }

            var tri = 0;
            for (var i = 0; i < segments; i++)
            {
                var inner = i * 2;
                var outer = inner + 1;
                var nextInner = ((i + 1) % segments) * 2;
                var nextOuter = nextInner + 1;
                ringTriangles[tri++] = inner;
                ringTriangles[tri++] = nextInner;
                ringTriangles[tri++] = outer;
                ringTriangles[tri++] = outer;
                ringTriangles[tri++] = nextInner;
                ringTriangles[tri++] = nextOuter;
            }

            return FinalizeMesh(name, ringVertices, ringTriangles);
        }

        private static Mesh CreateBurstMesh(string name, int spokes, float innerRadius, float outerRadius)
        {
            var vertices = new Vector3[spokes * 3];
            var triangles = new int[spokes * 3];
            for (var i = 0; i < spokes; i++)
            {
                var centerAngle = Mathf.PI * 2f * i / spokes;
                var leftAngle = centerAngle - Mathf.PI / spokes * 0.45f;
                var rightAngle = centerAngle + Mathf.PI / spokes * 0.45f;
                vertices[i * 3] = new Vector3(Mathf.Cos(leftAngle) * innerRadius, Mathf.Sin(leftAngle) * innerRadius, 0f);
                vertices[i * 3 + 1] = new Vector3(Mathf.Cos(rightAngle) * innerRadius, Mathf.Sin(rightAngle) * innerRadius, 0f);
                vertices[i * 3 + 2] = new Vector3(Mathf.Cos(centerAngle) * outerRadius, Mathf.Sin(centerAngle) * outerRadius, 0f);
                triangles[i * 3] = i * 3;
                triangles[i * 3 + 1] = i * 3 + 1;
                triangles[i * 3 + 2] = i * 3 + 2;
            }

            return FinalizeMesh(name, vertices, triangles);
        }

        private static Mesh FinalizeMesh(string name, Vector3[] vertices, int[] triangles)
        {
            var mesh = new Mesh
            {
                name = name,
                vertices = vertices,
                triangles = triangles
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Transform CreateChild(Transform parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            return child.transform;
        }

        private static GameObject CreatePrimitivePart(string name, PrimitiveType type, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            return CreatePrimitivePart(name, type, parent, localPosition, localScale, material, Quaternion.identity);
        }

        private static GameObject CreatePrimitivePart(string name, PrimitiveType type, Transform parent, Vector3 localPosition, Vector3 localScale, Material material, Quaternion localRotation)
        {
            var part = GameObject.CreatePrimitive(type);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localRotation = localRotation;
            part.transform.localScale = localScale;

            var collider = part.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = part.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
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

        private static void CreateSocket(Transform parent, string name, Vector3 localPosition)
        {
            var socket = new GameObject(name);
            socket.transform.SetParent(parent, false);
            socket.transform.localPosition = localPosition;
        }

        private static void SavePrefab(GameObject root, string assetPath)
        {
            PrefabUtility.SaveAsPrefabAsset(root, assetPath, out var success);
            UnityEngine.Object.DestroyImmediate(root);
            if (!success)
            {
                throw new InvalidOperationException("Failed to save feedback prefab: " + assetPath);
            }
        }

        private static void WriteCueWav(EventSpec spec, string assetPath)
        {
            var fullPath = AssetPathToFullPath(assetPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);

            const int sampleRate = 22050;
            var sampleCount = Mathf.Max(1, Mathf.CeilToInt(sampleRate * spec.Duration));
            var samples = new short[sampleCount];
            var seed = StableSeed(spec.EventType);
            var random = new System.Random(seed);

            for (var i = 0; i < sampleCount; i++)
            {
                var t = i / (float)sampleRate;
                var n = i / (float)sampleCount;
                var decay = Mathf.Exp(-5.2f * n);
                var tone = Mathf.Sin(Mathf.PI * 2f * spec.ToneHz * t);
                var upperTone = Mathf.Sin(Mathf.PI * 2f * spec.ToneHz * 1.62f * t) * 0.34f;
                var noise = ((float)random.NextDouble() * 2f - 1f) * 0.22f;
                var click = n < 0.08f ? Mathf.Sin(Mathf.PI * 2f * (620f + spec.Pattern * 11f) * t) * (1f - n / 0.08f) : 0f;
                var valvePulse = Mathf.Sin(Mathf.PI * 2f * (spec.ToneHz * 0.5f) * t) * Mathf.Clamp01(1f - Mathf.Abs(n - 0.35f) / 0.18f) * 0.45f;
                var phase = spec.Pattern % 5;
                var value = tone * 0.34f + upperTone + noise;
                if (phase == 0 || phase == 2)
                {
                    value += click * 0.72f;
                }

                if (phase == 3 || phase == 4)
                {
                    value += valvePulse;
                }

                value *= decay * 0.32f;
                samples[i] = (short)Mathf.Clamp(Mathf.RoundToInt(value * short.MaxValue), short.MinValue, short.MaxValue);
            }

            using (var stream = File.Open(fullPath, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                var dataLength = samples.Length * sizeof(short);
                writer.Write(Encoding.ASCII.GetBytes("RIFF"));
                writer.Write(36 + dataLength);
                writer.Write(Encoding.ASCII.GetBytes("WAVE"));
                writer.Write(Encoding.ASCII.GetBytes("fmt "));
                writer.Write(16);
                writer.Write((short)1);
                writer.Write((short)1);
                writer.Write(sampleRate);
                writer.Write(sampleRate * sizeof(short));
                writer.Write((short)sizeof(short));
                writer.Write((short)16);
                writer.Write(Encoding.ASCII.GetBytes("data"));
                writer.Write(dataLength);
                foreach (var sample in samples)
                {
                    writer.Write(sample);
                }
            }
        }

        private static void WriteCueCatalog()
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"SCFX\",");
            builder.AppendLine("  \"version\": \"0.1.38-p001\",");
            builder.AppendLine("  \"taxonomy_source\": \"v0.1.35 GameplayFeedbackController\",");
            builder.AppendLine("  \"events\": [");
            for (var i = 0; i < Specs.Length; i++)
            {
                var spec = Specs[i];
                builder.AppendLine("    {");
                builder.AppendLine("      \"event_type\": \"" + EscapeJson(spec.EventType) + "\",");
                builder.AppendLine("      \"family\": \"" + EscapeJson(spec.Family) + "\",");
                builder.AppendLine("      \"prefab\": \"" + EscapeJson(PrefabPath(spec)) + "\",");
                builder.AppendLine("      \"audio\": \"" + EscapeJson(AudioPath(spec)) + "\",");
                builder.AppendLine("      \"visual_intent\": \"" + EscapeJson(spec.VisualIntent) + "\",");
                builder.AppendLine("      \"audio_intent\": \"" + EscapeJson(spec.AudioIntent) + "\",");
                builder.AppendLine("      \"duration_seconds\": " + spec.Duration.ToString("0.###", CultureInfo.InvariantCulture));
                builder.Append("    }");
                builder.AppendLine(i == Specs.Length - 1 ? string.Empty : ",");
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(AssetPathToFullPath(MetadataRoot + "/SCFX_EventCueCatalog_v0.1.38.json"), builder.ToString(), Encoding.UTF8);
        }

        private static void RenderVisualContactSheet(string outputRoot)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var materials = CreateMaterials();

            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "preview_oily_stone_floor";
            floor.transform.position = new Vector3(0f, -0.46f, 0.2f);
            floor.transform.localScale = new Vector3(12.5f, 0.08f, 7.2f);
            floor.GetComponent<Renderer>().sharedMaterial = materials["Iron"];

            var key = new GameObject("warm_lantern_key").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1f, 0.74f, 0.45f);
            key.intensity = 2.25f;
            key.transform.rotation = Quaternion.Euler(44f, -34f, 0f);

            var rim = new GameObject("cool_gauge_rim").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(0.5f, 0.68f, 0.9f);
            rim.intensity = 3.0f;
            rim.range = 12f;
            rim.transform.position = new Vector3(-4.4f, 3.4f, -3.2f);

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
                var position = new Vector3((column - 2) * 2.25f, 0f, (1 - row) * 2.05f);
                var instance = UnityEngine.Object.Instantiate(prefab, position, Quaternion.Euler(0f, -22f, 0f));
                instance.name = prefab.name;

                var textObject = new GameObject(spec.EventType + "_label");
                textObject.transform.position = position + new Vector3(0f, -0.18f, -0.76f);
                textObject.transform.localScale = Vector3.one * 0.08f;
                var label = textObject.AddComponent<TextMesh>();
                label.text = spec.EventType;
                label.anchor = TextAnchor.MiddleCenter;
                label.alignment = TextAlignment.Center;
                label.fontSize = 54;
                label.color = new Color(0.95f, 0.8f, 0.54f);
            }

            var cameraObject = new GameObject("preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.024f, 0.022f, 0.02f);
            camera.orthographic = true;
            camera.orthographicSize = 5.9f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 60f;
            camera.transform.position = new Vector3(0f, 6.6f, -8.9f);
            camera.transform.LookAt(new Vector3(0f, 0.22f, 0f));

            foreach (var text in UnityEngine.Object.FindObjectsByType<TextMesh>(FindObjectsInactive.Exclude))
            {
                text.transform.rotation = camera.transform.rotation;
            }

            RenderCameraToPng(camera, Path.Combine(outputRoot, "SCFX_v0.1.38_feedback_visual_contact_sheet.png"), 2400, 1600);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderAudioCueSheet(string outputRoot)
        {
            const int width = 2400;
            const int height = 1400;
            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            Fill(texture, new Color(0.027f, 0.024f, 0.021f));
            DrawRect(texture, 0, height - 92, width, 92, new Color(0.11f, 0.075f, 0.04f));
            DrawRect(texture, 0, 0, width, 6, new Color(0.74f, 0.47f, 0.18f));

            var columns = 3;
            var cellWidth = width / columns;
            var cellHeight = 260;
            for (var i = 0; i < Specs.Length; i++)
            {
                var spec = Specs[i];
                var column = i % columns;
                var row = i / columns;
                var x = column * cellWidth + 54;
                var y = height - 150 - row * cellHeight - cellHeight;
                DrawRect(texture, x, y, cellWidth - 108, cellHeight - 38, new Color(0.055f, 0.049f, 0.043f));
                DrawRect(texture, x, y + cellHeight - 66, cellWidth - 108, 28, FamilyColor(spec));
                DrawWaveform(texture, x + 32, y + 38, cellWidth - 172, cellHeight - 128, spec);
            }

            texture.Apply();
            File.WriteAllBytes(Path.Combine(outputRoot, "SCFX_v0.1.38_audio_waveform_contact_sheet.png"), texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);
        }

        private static void DrawWaveform(Texture2D texture, int x, int y, int width, int height, EventSpec spec)
        {
            var centerY = y + height / 2;
            var previousX = x;
            var previousY = centerY;
            var color = FamilyColor(spec) * 1.35f;
            for (var i = 0; i < width; i++)
            {
                var n = i / (float)Mathf.Max(1, width - 1);
                var decay = Mathf.Exp(-4.6f * n);
                var value = Mathf.Sin(n * Mathf.PI * 2f * (3.5f + spec.Pattern * 0.16f)) * decay;
                value += Mathf.Sin(n * Mathf.PI * 2f * (9f + spec.Pattern * 0.3f)) * 0.25f * decay;
                if (n < 0.12f)
                {
                    value += (1f - n / 0.12f) * 0.55f;
                }

                var currentX = x + i;
                var currentY = centerY + Mathf.RoundToInt(value * height * 0.38f);
                DrawLine(texture, previousX, previousY, currentX, currentY, color);
                previousX = currentX;
                previousY = currentY;
            }

            DrawLine(texture, x, centerY, x + width, centerY, new Color(0.24f, 0.19f, 0.14f));
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

        private static void Fill(Texture2D texture, Color color)
        {
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
        }

        private static void DrawRect(Texture2D texture, int x, int y, int width, int height, Color color)
        {
            var minX = Mathf.Clamp(x, 0, texture.width - 1);
            var maxX = Mathf.Clamp(x + width, 0, texture.width);
            var minY = Mathf.Clamp(y, 0, texture.height - 1);
            var maxY = Mathf.Clamp(y + height, 0, texture.height);
            for (var yy = minY; yy < maxY; yy++)
            {
                for (var xx = minX; xx < maxX; xx++)
                {
                    texture.SetPixel(xx, yy, color);
                }
            }
        }

        private static void DrawLine(Texture2D texture, int x0, int y0, int x1, int y1, Color color)
        {
            var dx = Mathf.Abs(x1 - x0);
            var dy = -Mathf.Abs(y1 - y0);
            var sx = x0 < x1 ? 1 : -1;
            var sy = y0 < y1 ? 1 : -1;
            var err = dx + dy;

            while (true)
            {
                if (x0 >= 0 && x0 < texture.width && y0 >= 0 && y0 < texture.height)
                {
                    texture.SetPixel(x0, y0, color);
                }

                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                var e2 = 2 * err;
                if (e2 >= dy)
                {
                    err += dy;
                    x0 += sx;
                }

                if (e2 <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        private static Color FamilyColor(EventSpec spec)
        {
            switch (spec.MaterialKey)
            {
                case "Amber":
                    return new Color(1f, 0.56f, 0.16f);
                case "Copper":
                    return new Color(0.9f, 0.34f, 0.14f);
                case "Warning":
                    return new Color(1f, 0.1f, 0.05f);
                case "Green":
                    return new Color(0.3f, 0.95f, 0.42f);
                case "Blue":
                    return new Color(0.3f, 0.58f, 1f);
                case "Cyan":
                    return new Color(0.16f, 0.92f, 1f);
                case "Boss":
                    return new Color(1f, 0.18f, 0.04f);
                default:
                    return new Color(0.82f, 0.56f, 0.25f);
            }
        }

        private static void EnsurePackageFolders()
        {
            foreach (var path in new[] { PrefabRoot, MaterialRoot, AudioRoot, MetadataRoot, MeshRoot })
            {
                Directory.CreateDirectory(AssetPathToFullPath(path));
            }

            AssetDatabase.Refresh();
        }

        private static string PrefabPath(EventSpec spec)
        {
            return PrefabRoot + "/SCFX_EVT_" + spec.EventType + ".prefab";
        }

        private static string AudioPath(EventSpec spec)
        {
            return AudioRoot + "/SCFX_AUD_" + spec.EventType + ".wav";
        }

        private static string AssetPathToFullPath(string assetPath)
        {
            var normalizedPath = assetPath.Replace("\\", "/");
            var packageRoot = LocatePackageRoot();
            if (normalizedPath.StartsWith(packageRoot.AssetPath + "/", StringComparison.Ordinal))
            {
                var relativePath = normalizedPath.Substring(packageRoot.AssetPath.Length + 1);
                return Path.GetFullPath(Path.Combine(packageRoot.ResolvedPath, relativePath.Replace('/', Path.DirectorySeparatorChar)));
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

        private static string ResolveRepoRoot()
        {
            var packageRoot = LocatePackageRoot();
            if (!string.IsNullOrWhiteSpace(packageRoot.ResolvedPath))
            {
                var directory = new DirectoryInfo(packageRoot.ResolvedPath);
                while (directory != null)
                {
                    if (directory.Name.Equals("AssetPacks", StringComparison.OrdinalIgnoreCase) && directory.Parent != null)
                    {
                        return directory.Parent.FullName;
                    }

                    directory = directory.Parent;
                }
            }

            return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        }

        private static PackageRootInfo LocatePackageRoot()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(FeedbackFxAudioSidecarGenerator).Assembly);
            if (packageInfo != null && !string.IsNullOrWhiteSpace(packageInfo.assetPath) && !string.IsNullOrWhiteSpace(packageInfo.resolvedPath))
            {
                return new PackageRootInfo(packageInfo.assetPath, packageInfo.resolvedPath);
            }

            var scriptGuids = AssetDatabase.FindAssets(nameof(FeedbackFxAudioSidecarGenerator));
            foreach (var guid in scriptGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                const string suffix = "/Editor/FeedbackFxAudioSidecarGenerator.cs";
                if (path.EndsWith(suffix, StringComparison.Ordinal))
                {
                    var assetPath = path.Substring(0, path.Length - suffix.Length);
                    return new PackageRootInfo(assetPath, Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath)));
                }
            }

            throw new InvalidOperationException("Could not locate " + PackageName + " package root.");
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
                var hash = 17;
                foreach (var ch in value)
                {
                    hash = hash * 31 + ch;
                }

                return hash;
            }
        }

        private static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private sealed class EventSpec
        {
            public EventSpec(
                string eventType,
                string family,
                string visualIntent,
                string audioIntent,
                string materialKey,
                float worldScale,
                bool prefersWorldPosition,
                float duration,
                float toneHz,
                int pattern)
            {
                EventType = eventType;
                Family = family;
                VisualIntent = visualIntent;
                AudioIntent = audioIntent;
                MaterialKey = materialKey;
                WorldScale = worldScale;
                PrefersWorldPosition = prefersWorldPosition;
                Duration = duration;
                ToneHz = toneHz;
                Pattern = pattern;
            }

            public string EventType { get; }
            public string Family { get; }
            public string VisualIntent { get; }
            public string AudioIntent { get; }
            public string MaterialKey { get; }
            public float WorldScale { get; }
            public bool PrefersWorldPosition { get; }
            public float Duration { get; }
            public float ToneHz { get; }
            public int Pattern { get; }
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
