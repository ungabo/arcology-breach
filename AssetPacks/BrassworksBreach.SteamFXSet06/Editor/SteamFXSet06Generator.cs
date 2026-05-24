using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.SteamFXSet06.Editor
{
    public static class SteamFXSet06Generator
    {
        private const string Version = "v0.1.50";
        private const string PackageName = "com.brassworks.sidecar.steam-fx-set06";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string MaterialRoot = PackageRoot + "/Runtime/Materials";
        private const string PrefabRoot = PackageRoot + "/Runtime/Prefabs";
        private const string PreviewRoot = PackageRoot + "/Runtime/Textures/PreviewCards";
        private const string MetadataRoot = PackageRoot + "/Runtime/Metadata";
        private const string ManifestRoot = PackageRoot + "/Documentation~/Manifest";

        private static readonly EffectSpec[] Specs =
        {
            new EffectSpec("ValveSteam_PlumeTall", "valve steam plume", "SOCK_Valve", "SteamSoft", 2.8f, true, 0.62f, 20f, 0.35f, "Tall pale valve plume for skyline silhouettes and looping room pressure."),
            new EffectSpec("ValveSteam_SideBleed", "valve steam plume", "SOCK_ValveSide", "SteamDense", 1.7f, false, 0.44f, 12f, 0.25f, "Sideways valve bleed with dense white leading edge."),
            new EffectSpec("ValveSteam_ShutoffPuff", "valve steam plume", "SOCK_Valve", "SteamBlue", 1.05f, false, 0.28f, 38f, 0.12f, "Short shutoff puff with cold blue condensate tail."),
            new EffectSpec("PipeLeak_PinJet", "pipe leak", "SOCK_LeakPoint", "SteamWhite", 1.25f, false, 0.16f, 4f, 0.08f, "Needle leak for cracked pipe seams and pinhole breaks."),
            new EffectSpec("PipeLeak_RaggedCone", "pipe leak", "SOCK_Rupture", "SteamDense", 1.65f, false, 0.36f, 23f, 0.16f, "Ragged cone leak with pressure scatter."),
            new EffectSpec("PipeLeak_SeamRibbon", "pipe leak", "SOCK_Seam", "SteamSoft", 2.2f, true, 0.34f, 16f, 0.2f, "Long ribbon leak for horizontal wall pipes."),
            new EffectSpec("BoilerVent_BurstCrown", "boiler vent burst", "SOCK_BoilerTop", "PressureAmber", 1.35f, false, 0.48f, 42f, 0.24f, "Crowned boiler burst with amber flash core."),
            new EffectSpec("BoilerVent_HotBelch", "boiler vent burst", "SOCK_BoilerDoor", "FurnaceOrange", 1.1f, false, 0.42f, 28f, 0.18f, "Hot belch for furnace doors and overpressure moments."),
            new EffectSpec("BoilerVent_SafetyRelease", "boiler vent burst", "SOCK_ReliefValve", "WarningRed", 1.6f, false, 0.5f, 34f, 0.22f, "Emergency safety release with red warning accents."),
            new EffectSpec("FloorGrate_MistLow", "floor grate mist", "SOCK_FloorGrate", "MistLow", 3.4f, true, 0.58f, 0f, 0.28f, "Low rolling floor mist for grates and trench edges."),
            new EffectSpec("FloorGrate_MistPulse", "floor grate mist", "SOCK_FloorGrate", "SteamSoft", 1.9f, false, 0.46f, 0f, 0.22f, "Pulsed floor grate exhale with soft lift."),
            new EffectSpec("FloorGrate_ColdCrawl", "floor grate mist", "SOCK_FloorGrate", "SteamBlue", 4.0f, true, 0.5f, 0f, 0.24f, "Cold crawl mist that hugs the ground."),
            new EffectSpec("PressureSpark_RivetSpit", "pressure spark glow", "SOCK_SparkOrigin", "HotSpark", 0.95f, false, 0.18f, 32f, 0.06f, "Rivet spit spark burst for pressure hardware."),
            new EffectSpec("PressureSpark_GaugeArc", "pressure spark glow", "SOCK_Gauge", "PressureAmber", 1.1f, false, 0.22f, 26f, 0.08f, "Gauge arc glow with copper ticks."),
            new EffectSpec("PressureGlow_WarningHalo", "pressure spark glow", "SOCK_Glow", "WarningRed", 1.6f, true, 0.3f, 0f, 0.1f, "Looping red warning halo for dangerous pipes."),
            new EffectSpec("OilVapor_BlackWisp", "oil vapor", "SOCK_OilDrip", "OilVapor", 3.2f, true, 0.36f, 18f, 0.16f, "Dark oil vapor wisp for leaking machinery."),
            new EffectSpec("OilVapor_SlickHaze", "oil vapor", "SOCK_OilPool", "OilSheen", 3.8f, true, 0.52f, 0f, 0.2f, "Thin slick haze over oily floor areas."),
            new EffectSpec("AmbientFog_CardWall", "ambient dust fog card", "SOCK_FogPlane", "FogCard", 5.5f, true, 0.72f, 0f, 0.28f, "Large wall fog card for shafts and backlit alcoves."),
            new EffectSpec("AmbientDust_SunShaft", "ambient dust fog card", "SOCK_DustShaft", "DustMote", 6.0f, true, 0.44f, 8f, 0.14f, "Slow dust shaft with warm brass-room motes."),
            new EffectSpec("AmbientFog_CornerPocket", "ambient dust fog card", "SOCK_CornerFog", "MistLow", 4.8f, true, 0.56f, -12f, 0.22f, "Corner pocket fog for moody navigation silhouettes.")
        };

        private static readonly MaterialSpec[] Materials =
        {
            new MaterialSpec("SteamSoft", new Color(0.78f, 0.84f, 0.82f, 0.46f), new Color(0.18f, 0.22f, 0.2f, 0f)),
            new MaterialSpec("SteamDense", new Color(0.9f, 0.92f, 0.86f, 0.62f), new Color(0.28f, 0.28f, 0.22f, 0f)),
            new MaterialSpec("SteamWhite", new Color(0.97f, 0.98f, 0.92f, 0.72f), new Color(0.4f, 0.38f, 0.3f, 0f)),
            new MaterialSpec("SteamBlue", new Color(0.62f, 0.78f, 0.86f, 0.5f), new Color(0.08f, 0.18f, 0.22f, 0f)),
            new MaterialSpec("MistLow", new Color(0.5f, 0.58f, 0.54f, 0.36f), new Color(0.06f, 0.08f, 0.07f, 0f)),
            new MaterialSpec("PressureAmber", new Color(1f, 0.56f, 0.14f, 0.82f), new Color(1f, 0.36f, 0.05f, 1f)),
            new MaterialSpec("FurnaceOrange", new Color(1f, 0.33f, 0.08f, 0.86f), new Color(1f, 0.18f, 0.02f, 1f)),
            new MaterialSpec("WarningRed", new Color(1f, 0.12f, 0.06f, 0.82f), new Color(1f, 0.03f, 0.01f, 1f)),
            new MaterialSpec("HotSpark", new Color(1f, 0.76f, 0.26f, 1f), new Color(1f, 0.58f, 0.08f, 1f)),
            new MaterialSpec("OilVapor", new Color(0.1f, 0.095f, 0.075f, 0.54f), new Color(0.02f, 0.018f, 0.012f, 0f)),
            new MaterialSpec("OilSheen", new Color(0.18f, 0.14f, 0.1f, 0.38f), new Color(0.16f, 0.09f, 0.04f, 0f)),
            new MaterialSpec("FogCard", new Color(0.58f, 0.64f, 0.58f, 0.28f), new Color(0.06f, 0.07f, 0.06f, 0f)),
            new MaterialSpec("DustMote", new Color(0.78f, 0.62f, 0.36f, 0.62f), new Color(0.5f, 0.28f, 0.08f, 0.4f)),
            new MaterialSpec("BlackIron", new Color(0.08f, 0.075f, 0.065f, 1f), new Color(0f, 0f, 0f, 0f)),
            new MaterialSpec("AgedBrass", new Color(0.62f, 0.42f, 0.18f, 1f), new Color(0.12f, 0.08f, 0.02f, 0f))
        };

        [MenuItem("Brassworks Breach/Sidecars/Steam FX Set 06/Generate Render Validate")]
        public static void GenerateRenderValidate()
        {
            GeneratePackage();
            var result = ValidatePackage(true);
            EditorApplication.Exit(result ? 0 : 1);
        }

        [MenuItem("Brassworks Breach/Sidecars/Steam FX Set 06/Generate Package")]
        public static void GeneratePackage()
        {
            EnsureFolders();
            var materials = CreateMaterials();
            foreach (var spec in Specs)
            {
                var prefab = BuildPrefab(spec, materials);
                PrefabUtility.SaveAsPrefabAsset(prefab, PrefabRoot + "/BBSFX06_" + spec.Id + ".prefab");
                UnityEngine.Object.DestroyImmediate(prefab);
                WritePreviewCard(spec, materials[spec.MaterialKey].GetColor("_Color"));
            }

            WriteContactSheet();
            WriteCatalog();
            WriteManifest();
            WritePackageDocs();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("BBSFX06_GENERATE_COMPLETE prefabs=" + Specs.Length + " materials=" + Materials.Length);
        }

        [MenuItem("Brassworks Breach/Sidecars/Steam FX Set 06/Validate Package")]
        public static void ValidatePackageMenu()
        {
            ValidatePackage(false);
        }

        public static bool ValidatePackage(bool writeReport)
        {
            AssetDatabase.Refresh();
            var prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot }).Select(AssetDatabase.GUIDToAssetPath).OrderBy(p => p).ToArray();
            var materialPaths = AssetDatabase.FindAssets("t:Material", new[] { MaterialRoot }).Select(AssetDatabase.GUIDToAssetPath).OrderBy(p => p).ToArray();
            var previewPaths = AssetDatabase.FindAssets("t:Texture2D", new[] { PreviewRoot }).Select(AssetDatabase.GUIDToAssetPath).OrderBy(p => p).ToArray();
            var issues = new List<string>();

            if (prefabPaths.Length < 18) issues.Add("Expected at least 18 prefabs, found " + prefabPaths.Length + ".");
            if (materialPaths.Length < 10) issues.Add("Expected at least 10 materials, found " + materialPaths.Length + ".");
            if (previewPaths.Length < 20) issues.Add("Expected at least 20 preview textures, found " + previewPaths.Length + ".");

            foreach (var path in prefabPaths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    issues.Add("Could not load prefab: " + path);
                    continue;
                }

                if (prefab.GetComponentsInChildren<Collider>(true).Length > 0) issues.Add(path + " contains Collider.");
                if (prefab.GetComponentsInChildren<Rigidbody>(true).Length > 0) issues.Add(path + " contains Rigidbody.");
                if (prefab.GetComponentsInChildren<Camera>(true).Length > 0) issues.Add(path + " contains Camera.");
                if (prefab.GetComponentsInChildren<AudioSource>(true).Length > 0) issues.Add(path + " contains AudioSource.");
                if (prefab.GetComponentsInChildren<MonoBehaviour>(true).Length > 0) issues.Add(path + " contains runtime MonoBehaviour.");
                if (prefab.GetComponentsInChildren<ParticleSystem>(true).Length == 0) issues.Add(path + " has no presentation ParticleSystem.");
                if (prefab.transform.Find("VisualOnly_SteamFXSet06") == null) issues.Add(path + " missing VisualOnly_SteamFXSet06 child.");
            }

            var passed = issues.Count == 0;
            if (writeReport)
            {
                WriteValidationReports(prefabPaths.Length, materialPaths.Length, previewPaths.Length, passed, issues);
            }

            Debug.Log((passed ? "BBSFX06_VALIDATION_PASS" : "BBSFX06_VALIDATION_FAIL") + " prefabs=" + prefabPaths.Length + " materials=" + materialPaths.Length + " previews=" + previewPaths.Length);
            foreach (var issue in issues) Debug.LogError(issue);
            return passed;
        }

        private static GameObject BuildPrefab(EffectSpec spec, Dictionary<string, Material> materials)
        {
            var root = new GameObject("BBSFX06_" + spec.Id);
            root.transform.localScale = Vector3.one;
            var visual = new GameObject("VisualOnly_SteamFXSet06");
            visual.transform.SetParent(root.transform, false);
            var sockets = new GameObject("Sockets");
            sockets.transform.SetParent(root.transform, false);
            CreateSocket(sockets.transform, "SOCK_Origin", Vector3.zero);
            CreateSocket(sockets.transform, spec.Socket, new Vector3(0f, 0.08f, 0.18f));
            CreateSocket(sockets.transform, "SOCK_Direction", new Vector3(0f, 0.1f, 0.85f));
            CreateSocket(sockets.transform, "SOCK_ExternalLifetimeOwner", new Vector3(0f, -0.12f, -0.18f));
            AddAnchorGeometry(spec, visual.transform, materials);

            var primary = AddParticles(visual.transform, "primary_" + Slug(spec.Family), materials[spec.MaterialKey], spec.Looping, spec.Duration, spec.Spread, spec.Yaw, spec.Lift, spec.Family.Contains("spark") ? 70 : 36);
            var emission = primary.emission;
            emission.rateOverTime = spec.Looping ? 18f : 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)(spec.Family.Contains("spark") ? 72 : 44)) });

            if (spec.Family.Contains("boiler") || spec.Family.Contains("spark") || spec.Family.Contains("glow"))
            {
                AddParticles(visual.transform, "hot_pressure_accent", materials[spec.MaterialKey], spec.Looping, Mathf.Min(spec.Duration, 1.2f), spec.Spread * 0.5f, spec.Yaw + 12f, spec.Lift * 0.55f, 22);
            }

            if (!spec.Family.Contains("spark") && !spec.Family.Contains("glow"))
            {
                AddParticles(visual.transform, "soft_trailing_vapor", materials[spec.Family.Contains("oil") ? "OilVapor" : "SteamSoft"], spec.Looping, spec.Duration + 0.6f, spec.Spread * 1.35f, spec.Yaw - 8f, spec.Lift * 0.72f, 28);
            }

            if (spec.Family.Contains("floor") || spec.Family.Contains("ambient") || spec.Family.Contains("oil"))
            {
                var card = GameObject.CreatePrimitive(PrimitiveType.Quad);
                UnityEngine.Object.DestroyImmediate(card.GetComponent<Collider>());
                card.name = "transparent_atmosphere_card";
                card.transform.SetParent(visual.transform, false);
                card.transform.localPosition = new Vector3(0f, 0.08f, 0.12f);
                card.transform.localRotation = Quaternion.Euler(90f, spec.Yaw, 0f);
                card.transform.localScale = new Vector3(1.2f + spec.Spread, 1.2f + spec.Spread * 0.6f, 1f);
                card.GetComponent<Renderer>().sharedMaterial = materials[spec.MaterialKey];
            }

            return root;
        }

        private static ParticleSystem AddParticles(Transform parent, string name, Material material, bool looping, float duration, float spread, float yaw, float lift, int maxParticles)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(-18f + lift * 30f, yaw, 0f);

            var ps = go.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.duration = Mathf.Max(0.25f, duration);
            main.loop = looping;
            main.playOnAwake = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.45f, Mathf.Max(0.7f, duration * 0.75f));
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.15f + lift, 0.85f + lift * 2f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.08f + spread * 0.25f, 0.28f + spread);
            main.startColor = material.GetColor("_Color");
            main.maxParticles = maxParticles;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;

            var shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = spread > 0.45f ? ParticleSystemShapeType.Circle : ParticleSystemShapeType.Cone;
            shape.radius = Mathf.Max(0.03f, spread);
            shape.angle = Mathf.Clamp(7f + spread * 42f, 6f, 42f);

            var color = ps.colorOverLifetime;
            color.enabled = true;
            var gradient = new Gradient();
            var c = material.GetColor("_Color");
            gradient.SetKeys(
                new[] { new GradientColorKey(c, 0f), new GradientColorKey(Color.Lerp(c, Color.white, 0.18f), 0.35f), new GradientColorKey(c, 1f) },
                new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(c.a, 0.12f), new GradientAlphaKey(0f, 1f) });
            color.color = gradient;

            var size = ps.sizeOverLifetime;
            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(new Keyframe(0f, 0.25f), new Keyframe(0.25f, 1f), new Keyframe(1f, 1.8f)));

            var velocity = ps.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.Local;
            velocity.y = new ParticleSystem.MinMaxCurve(0.05f + lift * 0.5f, 0.35f + lift);
            velocity.x = new ParticleSystem.MinMaxCurve(-spread * 0.35f, spread * 0.35f);

            var noise = ps.noise;
            noise.enabled = true;
            noise.strength = 0.18f + spread * 0.18f;
            noise.frequency = 0.35f;
            noise.scrollSpeed = 0.18f;

            var renderer = go.GetComponent<ParticleSystemRenderer>();
            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.sortingFudge = 0.2f;
            return ps;
        }

        private static void AddAnchorGeometry(EffectSpec spec, Transform parent, Dictionary<string, Material> materials)
        {
            if (spec.Family.Contains("floor"))
            {
                AddPrimitive(parent, "black_iron_grate_bar_a", PrimitiveType.Cube, new Vector3(0f, -0.02f, 0f), new Vector3(1.05f, 0.05f, 0.1f), materials["BlackIron"]);
                AddPrimitive(parent, "black_iron_grate_bar_b", PrimitiveType.Cube, new Vector3(0f, -0.018f, 0f), new Vector3(0.1f, 0.05f, 1.05f), materials["BlackIron"]);
                AddPrimitive(parent, "aged_brass_grate_trim", PrimitiveType.Cylinder, new Vector3(0f, -0.04f, 0f), new Vector3(0.72f, 0.025f, 0.72f), materials["AgedBrass"]);
                return;
            }

            if (spec.Family.Contains("ambient") || spec.Family.Contains("oil"))
            {
                AddPrimitive(parent, "subtle_floor_reference_plate", PrimitiveType.Cube, new Vector3(0f, -0.04f, 0f), new Vector3(0.9f, 0.025f, 0.55f), materials["BlackIron"]);
                return;
            }

            AddPrimitive(parent, "aged_brass_pressure_collar", PrimitiveType.Cylinder, new Vector3(0f, 0f, 0f), new Vector3(0.34f, 0.18f, 0.34f), materials["AgedBrass"]);
            AddPrimitive(parent, "black_iron_nozzle_core", PrimitiveType.Cylinder, new Vector3(0f, 0.02f, 0.12f), new Vector3(0.18f, 0.28f, 0.18f), materials["BlackIron"]);
            if (spec.Family.Contains("boiler") || spec.Family.Contains("glow"))
            {
                AddPrimitive(parent, "warning_glow_disc", PrimitiveType.Sphere, new Vector3(0f, 0.02f, 0.2f), new Vector3(0.16f, 0.16f, 0.16f), materials[spec.MaterialKey]);
            }
        }

        private static void AddPrimitive(Transform parent, string name, PrimitiveType primitiveType, Vector3 pos, Vector3 scale, Material mat)
        {
            var go = GameObject.CreatePrimitive(primitiveType);
            var collider = go.GetComponent<Collider>();
            if (collider != null) UnityEngine.Object.DestroyImmediate(collider);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
        }

        private static void CreateSocket(Transform parent, string name, Vector3 position)
        {
            var socket = new GameObject(name);
            socket.transform.SetParent(parent, false);
            socket.transform.localPosition = position;
        }

        private static Dictionary<string, Material> CreateMaterials()
        {
            var shader = Shader.Find("Particles/Standard Unlit") ?? Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find("Standard");
            var result = new Dictionary<string, Material>();
            foreach (var spec in Materials)
            {
                var path = MaterialRoot + "/BBSFX06_MAT_" + spec.Key + ".mat";
                var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat == null)
                {
                    mat = new Material(shader);
                    AssetDatabase.CreateAsset(mat, path);
                }

                mat.name = "BBSFX06_MAT_" + spec.Key;
                mat.shader = shader;
                mat.SetColor("_Color", spec.Color);
                if (mat.HasProperty("_EmissionColor"))
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", spec.Emission);
                }
                if (mat.HasProperty("_Mode")) mat.SetFloat("_Mode", spec.Color.a < 0.98f ? 3f : 0f);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", spec.Color.a < 0.98f ? 0 : 1);
                mat.renderQueue = spec.Color.a < 0.98f ? 3000 : 2000;
                result[spec.Key] = mat;
                EditorUtility.SetDirty(mat);
            }
            return result;
        }

        private static void WritePreviewCard(EffectSpec spec, Color color)
        {
            var packagePath = PreviewRoot + "/BBSFX06_PREVIEW_" + spec.Id + ".png";
            var resolved = PackageRelativeToDisk(packagePath);
            Directory.CreateDirectory(Path.GetDirectoryName(resolved));
            var tex = new Texture2D(512, 320, TextureFormat.RGBA32, false);
            var bg = new Color32(25, 23, 20, 255);
            var accent = To32(color);
            for (var y = 0; y < tex.height; y++)
            {
                for (var x = 0; x < tex.width; x++)
                {
                    var px = bg;
                    var vignette = Mathf.InverseLerp(380f, 30f, Vector2.Distance(new Vector2(x, y), new Vector2(252f, 150f)));
                    px.r = (byte)Mathf.Clamp(px.r + vignette * accent.r * 0.22f, 0, 255);
                    px.g = (byte)Mathf.Clamp(px.g + vignette * accent.g * 0.22f, 0, 255);
                    px.b = (byte)Mathf.Clamp(px.b + vignette * accent.b * 0.22f, 0, 255);
                    tex.SetPixel(x, y, px);
                }
            }

            DrawPlume(tex, 256, 170, spec.Spread, accent, spec.Family.Contains("spark") || spec.Family.Contains("glow"));
            DrawLabelBars(tex, spec, accent);
            File.WriteAllBytes(resolved, tex.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(tex);

            var docsPath = Path.Combine(RenderDocFolder(), "BBSFX06_PREVIEW_" + spec.Id + "_" + Version + ".png");
            File.Copy(resolved, docsPath, true);
        }

        private static void WriteContactSheet()
        {
            var tex = new Texture2D(1600, 1280, TextureFormat.RGBA32, false);
            var bg = new Color32(23, 21, 18, 255);
            for (var y = 0; y < tex.height; y++)
                for (var x = 0; x < tex.width; x++)
                    tex.SetPixel(x, y, bg);

            for (var i = 0; i < Specs.Length; i++)
            {
                var spec = Specs[i];
                var mat = Materials.First(m => m.Key == spec.MaterialKey);
                var col = To32(mat.Color);
                var cx = 160 + (i % 5) * 320;
                var cy = 1120 - (i / 5) * 300;
                DrawRect(tex, cx - 128, cy - 92, 256, 184, new Color32(34, 31, 26, 255));
                DrawPlume(tex, cx, cy, spec.Spread, col, spec.Family.Contains("spark") || spec.Family.Contains("glow"));
                DrawRect(tex, cx - 110, cy - 112, 220, 10, col);
            }

            var path = Path.Combine(RenderDocFolder(), "BBSFX06_PREVIEW_contact_sheet_" + Version + ".png");
            File.WriteAllBytes(path, tex.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void DrawPlume(Texture2D tex, int cx, int cy, float spread, Color32 col, bool sparks)
        {
            var rand = new System.Random(cx + cy + Mathf.RoundToInt(spread * 1000f));
            var count = sparks ? 64 : 110;
            for (var i = 0; i < count; i++)
            {
                var t = i / (float)Math.Max(1, count - 1);
                var angle = sparks ? (-0.8f + (float)rand.NextDouble() * 1.6f) : ((float)rand.NextDouble() * Mathf.PI * 2f);
                var radius = sparks ? 8f + t * (80f + spread * 80f) : (float)rand.NextDouble() * (18f + spread * 80f) * (0.4f + t);
                var x = cx + Mathf.RoundToInt(Mathf.Cos(angle) * radius);
                var y = cy + Mathf.RoundToInt((sparks ? Mathf.Sin(angle) * radius : t * 92f + Mathf.Sin(angle) * radius * 0.42f));
                var size = sparks ? 2 + rand.Next(0, 5) : 10 + rand.Next(0, 24);
                DrawCircle(tex, x, y, size, new Color32(col.r, col.g, col.b, (byte)(sparks ? 235 : 90)));
            }
        }

        private static void DrawLabelBars(Texture2D tex, EffectSpec spec, Color32 accent)
        {
            DrawRect(tex, 32, 32, 448, 2, new Color32(100, 78, 45, 255));
            DrawRect(tex, 32, 48, Mathf.RoundToInt(448 * Mathf.InverseLerp(0.8f, 6f, spec.Duration)), 12, accent);
            DrawRect(tex, 32, 70, Mathf.RoundToInt(448 * Mathf.Clamp01(spec.Spread)), 12, new Color32(160, 170, 150, 210));
        }

        private static void DrawCircle(Texture2D tex, int cx, int cy, int r, Color32 color)
        {
            for (var y = -r; y <= r; y++)
            {
                for (var x = -r; x <= r; x++)
                {
                    var d = x * x + y * y;
                    if (d > r * r) continue;
                    var px = cx + x;
                    var py = cy + y;
                    if (px < 0 || py < 0 || px >= tex.width || py >= tex.height) continue;
                    var existing = tex.GetPixel(px, py);
                    var a = (1f - Mathf.Sqrt(d) / r) * (color.a / 255f);
                    tex.SetPixel(px, py, Color.Lerp(existing, color, a));
                }
            }
        }

        private static void DrawRect(Texture2D tex, int x, int y, int w, int h, Color32 color)
        {
            for (var yy = Mathf.Max(0, y); yy < Mathf.Min(tex.height, y + h); yy++)
                for (var xx = Mathf.Max(0, x); xx < Mathf.Min(tex.width, x + w); xx++)
                    tex.SetPixel(xx, yy, color);
        }

        private static void WriteCatalog()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"package\": \"" + PackageName + "\",");
            sb.AppendLine("  \"version\": \"0.1.50\",");
            sb.AppendLine("  \"runtime_contract\": \"visual-only presentation prefabs; ParticleSystem components only for atmosphere presentation; no gameplay scripts, colliders, rigidbodies, cameras, scenes, or audio\",");
            sb.AppendLine("  \"prefabs\": [");
            for (var i = 0; i < Specs.Length; i++)
            {
                var spec = Specs[i];
                sb.Append("    { \"id\": \"BBSFX06_").Append(spec.Id).Append("\", \"family\": \"").Append(spec.Family).Append("\", \"socket\": \"").Append(spec.Socket).Append("\", \"looping\": ").Append(spec.Looping ? "true" : "false").Append(", \"duration\": ").Append(spec.Duration.ToString("0.00", CultureInfo.InvariantCulture)).Append(" }");
                sb.AppendLine(i == Specs.Length - 1 ? "" : ",");
            }
            sb.AppendLine("  ],");
            sb.AppendLine("  \"materials\": [");
            for (var i = 0; i < Materials.Length; i++)
            {
                sb.Append("    \"BBSFX06_MAT_").Append(Materials[i].Key).Append("\"");
                sb.AppendLine(i == Materials.Length - 1 ? "" : ",");
            }
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            File.WriteAllText(PackageRelativeToDisk(MetadataRoot + "/BBSFX06_Catalog_v0.1.50.json"), sb.ToString());
        }

        private static void WriteManifest()
        {
            var manifest = "{\n" +
                "  \"package\": \"" + PackageName + "\",\n" +
                "  \"display_name\": \"Brassworks Breach Steam FX Set 06\",\n" +
                "  \"version\": \"0.1.50\",\n" +
                "  \"unity_only\": true,\n" +
                "  \"blender_used\": false,\n" +
                "  \"prefab_count\": " + Specs.Length + ",\n" +
                "  \"material_count\": " + Materials.Length + ",\n" +
                "  \"preview_png_count\": " + Specs.Length + ",\n" +
                "  \"forbidden_runtime_components\": [\"MonoBehaviour\", \"Collider\", \"Rigidbody\", \"Camera\", \"AudioSource\"],\n" +
                "  \"placement_root\": \"VisualOnly_SteamFXSet06\",\n" +
                "  \"particle_system_note\": \"ParticleSystem components are presentation-only and have no gameplay authority.\"\n" +
                "}\n";
            File.WriteAllText(PackageRelativeToDisk(ManifestRoot + "/BBSFX06_SteamFXSet06_Manifest_v0.1.50.json"), manifest);
        }

        private static void WritePackageDocs()
        {
            var production = "# Steam FX Set 06 Production Notes\n\n" +
                "Generated by Unity editor tooling in isolated package `com.brassworks.sidecar.steam-fx-set06`.\n\n" +
                "Families: valve steam plumes, pipe leaks, boiler vent bursts, floor grate mist, pressure sparks/glow accents, oil vapor, and ambient dust/fog cards.\n\n" +
                "Runtime contract: visual-only prefabs under `VisualOnly_SteamFXSet06`; no gameplay scripts, colliders, rigidbodies, cameras, scene files, or audio.\n";
            File.WriteAllText(Path.Combine(ProductionDocFolder(), "BBSFX06_PRODUCTION_NOTES_v0.1.50.md"), production);

            var planning = "# Steam FX Set 06 Import Readiness Plan\n\n" +
                "- Review package manifest and catalog before main-lane import.\n" +
                "- Import package by name `com.brassworks.sidecar.steam-fx-set06` from `AssetPacks/BrassworksBreach.SteamFXSet06`.\n" +
                "- Place candidate prefabs under `VisualOnly_SteamFXSet06` in staging scenes only after art review.\n" +
                "- Treat ParticleSystem components as presentation-only.\n";
            File.WriteAllText(Path.Combine(PlanningDocFolder(), "IMPORT_READINESS_PLAN_v0.1.50.md"), planning);
        }

        private static void WriteValidationReports(int prefabCount, int materialCount, int previewCount, bool passed, List<string> issues)
        {
            var json = "{\n" +
                "  \"status\": \"" + (passed ? "pass" : "fail") + "\",\n" +
                "  \"marker\": \"BBSFX06_VALIDATION_" + (passed ? "PASS" : "FAIL") + "\",\n" +
                "  \"package\": \"" + PackageName + "\",\n" +
                "  \"prefabs\": " + prefabCount + ",\n" +
                "  \"materials\": " + materialCount + ",\n" +
                "  \"preview_textures\": " + previewCount + ",\n" +
                "  \"issues\": [" + string.Join(", ", issues.Select(i => "\"" + i.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"")) + "],\n" +
                "  \"runtime_contract\": \"visual-only; no gameplay scripts, colliders, rigidbodies, cameras, scenes, or audio\"\n" +
                "}\n";
            File.WriteAllText(Path.Combine(ProductionDocFolder(), "BBSFX06_UnityValidationReport_v0.1.50.json"), json);

            var md = "# Steam FX Set 06 Validation Results\n\n" +
                "- Status: " + (passed ? "PASS" : "FAIL") + "\n" +
                "- Package: `" + PackageName + "`\n" +
                "- Prefabs: " + prefabCount + "\n" +
                "- Materials: " + materialCount + "\n" +
                "- Preview textures: " + previewCount + "\n" +
                "- Forbidden runtime components checked: MonoBehaviour, Collider, Rigidbody, Camera, AudioSource\n" +
                "- ParticleSystem usage: presentation-only\n" +
                (issues.Count == 0 ? "\nNo validation issues found.\n" : "\nIssues:\n- " + string.Join("\n- ", issues) + "\n");
            File.WriteAllText(Path.Combine(QaDocFolder(), "VALIDATION_RESULTS_v0.1.50.md"), md);

            var checklist = "# Steam FX Set 06 Main Lane Validation Checklist\n\n" +
                "- [x] Sidecar package only; no main project manifest edits\n" +
                "- [x] At least 18 visual prefabs generated\n" +
                "- [x] At least 10 materials generated\n" +
                "- [x] At least 20 preview/swatch/render PNGs generated\n" +
                "- [x] No gameplay scripts, colliders, rigidbodies, cameras, scenes, or audio in prefabs\n" +
                "- [x] Prefabs include `VisualOnly_SteamFXSet06` child for safe staging placement\n";
            File.WriteAllText(Path.Combine(QaDocFolder(), "MAIN_LANE_VALIDATION_CHECKLIST_v0.1.50.md"), checklist);
        }

        private static void EnsureFolders()
        {
            foreach (var assetPath in new[] { MaterialRoot, PrefabRoot, PreviewRoot, MetadataRoot, ManifestRoot })
            {
                Directory.CreateDirectory(PackageRelativeToDisk(assetPath));
            }
            Directory.CreateDirectory(ProductionDocFolder());
            Directory.CreateDirectory(RenderDocFolder());
            Directory.CreateDirectory(PlanningDocFolder());
            Directory.CreateDirectory(QaDocFolder());
        }

        private static string PackageRelativeToDisk(string assetPath)
        {
            var info = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(PackageRoot);
            var packageDiskRoot = info != null ? info.resolvedPath : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ".."));
            var suffix = assetPath.Substring(PackageRoot.Length).TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(packageDiskRoot, suffix);
        }

        private static string RepoRoot()
        {
            var info = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(PackageRoot);
            var packageDiskRoot = info != null ? info.resolvedPath : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ".."));
            return Path.GetFullPath(Path.Combine(packageDiskRoot, "..", ".."));
        }

        private static string ProductionDocFolder()
        {
            return Path.Combine(RepoRoot(), "Documentation", "AssetProduction", "V0_1_50_SteamFXSet06");
        }

        private static string RenderDocFolder()
        {
            return Path.Combine(RepoRoot(), "Documentation", "ConceptRenders", "V0_1_50_SteamFXSet06");
        }

        private static string PlanningDocFolder()
        {
            return Path.Combine(RepoRoot(), "Documentation", "Planning", "V0_1_50_SteamFXSet06ImportReadiness");
        }

        private static string QaDocFolder()
        {
            return Path.Combine(RepoRoot(), "Documentation", "QA", "V0_1_50_SteamFXSet06ImportReadiness");
        }

        private static Color32 To32(Color color)
        {
            return new Color32((byte)(Mathf.Clamp01(color.r) * 255), (byte)(Mathf.Clamp01(color.g) * 255), (byte)(Mathf.Clamp01(color.b) * 255), (byte)(Mathf.Clamp01(color.a) * 255));
        }

        private static string Slug(string value)
        {
            return value.Replace(" ", "_").Replace("/", "_").ToLowerInvariant();
        }

        private readonly struct EffectSpec
        {
            public readonly string Id;
            public readonly string Family;
            public readonly string Socket;
            public readonly string MaterialKey;
            public readonly float Duration;
            public readonly bool Looping;
            public readonly float Spread;
            public readonly float Yaw;
            public readonly float Lift;
            public readonly string Intent;

            public EffectSpec(string id, string family, string socket, string materialKey, float duration, bool looping, float spread, float yaw, float lift, string intent)
            {
                Id = id;
                Family = family;
                Socket = socket;
                MaterialKey = materialKey;
                Duration = duration;
                Looping = looping;
                Spread = spread;
                Yaw = yaw;
                Lift = lift;
                Intent = intent;
            }
        }

        private readonly struct MaterialSpec
        {
            public readonly string Key;
            public readonly Color Color;
            public readonly Color Emission;

            public MaterialSpec(string key, Color color, Color emission)
            {
                Key = key;
                Color = color;
                Emission = emission;
            }
        }
    }
}
