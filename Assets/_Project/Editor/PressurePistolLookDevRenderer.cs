using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class PressurePistolLookDevRenderer
{
    private const int ComponentWidth = 1600;
    private const int ComponentHeight = 1000;
    private const int ContactWidth = 2200;
    private const int ContactHeight = 1200;
    private const string RenderFolderRelativePath = "Documentation/ConceptRenders/PressurePistolComponents";
    private const string ReportFolderRelativePath = "Documentation/AssetProduction/PressurePistolLookDev";
    private const string CoilRenderRelativePath = RenderFolderRelativePath + "/PPCOMP_001_copper_brass_coil_pack.png";
    private const string GaugeRenderRelativePath = RenderFolderRelativePath + "/PPCOMP_002_pressure_gauge_dial.png";
    private const string ContactRelativePath = RenderFolderRelativePath + "/PPCOMP_CONTACTSHEET_001_coil_pack_gauge_dial.png";
    private const string MetricsRelativePath = ReportFolderRelativePath + "/pressure_pistol_component_lookdev_metrics.json";
    private const string ReportRelativePath = ReportFolderRelativePath + "/PRESSURE_PISTOL_COMPONENT_LOOKDEV_RENDER_REPORT.md";

    [MenuItem("Project Tools/Lookdev/Render Pressure Pistol Component LookDev")]
    public static void RenderFromMenu()
    {
        RenderBatch();
    }

    public static void RenderBatch()
    {
        try
        {
            RenderLookDev();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }

            throw;
        }
    }

    private static void RenderLookDev()
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        Directory.CreateDirectory(Path.Combine(projectRoot, RenderFolderRelativePath));
        Directory.CreateDirectory(Path.Combine(projectRoot, ReportFolderRelativePath));

        ComponentLaneMetrics metrics = new ComponentLaneMetrics
        {
            timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture),
            unityVersion = Application.unityVersion,
            batchmodeEntrypoint = "PressurePistolLookDevRenderer.RenderBatch",
            renderFolder = RenderFolderRelativePath,
            contactSheetPath = ContactRelativePath,
            reportPath = ReportRelativePath,
            metricsPath = MetricsRelativePath,
            componentWidth = ComponentWidth,
            componentHeight = ComponentHeight,
            contactSheetWidth = ContactWidth,
            contactSheetHeight = ContactHeight,
            laneScope = "Unity-only temporary editor lookdev for isolated pressure-pistol components. No gameplay assets, tests, audio, or full-gun integration are touched.",
            sourceMaterialFolder = "Assets/_Project/ArtStaging/FinalMaterialsV1/Textures",
            sourceConcept = "Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png"
        };

        List<ComponentRenderResult> results = new List<ComponentRenderResult>();
        Texture2D contactSheet = null;

        try
        {
            results.Add(RenderSingleComponent(projectRoot, "coil_pack", "Copper/brass coil pack", CoilRenderRelativePath, new Vector3(0.18f, 0.08f, -2.55f), new Vector3(0f, -0.02f, -0.04f), 28f, BuildCoilPack));
            results.Add(RenderSingleComponent(projectRoot, "gauge_dial", "Pressure gauge/dial", GaugeRenderRelativePath, new Vector3(0.0f, 0.04f, -2.18f), new Vector3(0f, 0.0f, -0.03f), 24f, BuildGaugeDial));

            metrics.components = new ComponentMetric[results.Count];
            for (int i = 0; i < results.Count; i++)
            {
                metrics.components[i] = results[i].Metric;
            }

            contactSheet = BuildContactSheet(projectRoot, results);
            File.WriteAllBytes(Path.Combine(projectRoot, ContactRelativePath), contactSheet.EncodeToPNG());
            File.WriteAllText(Path.Combine(projectRoot, MetricsRelativePath), JsonUtility.ToJson(metrics, true));
            File.WriteAllText(Path.Combine(projectRoot, ReportRelativePath), BuildReport(metrics));

            Debug.Log("Pressure pistol component contact sheet written to " + Path.Combine(projectRoot, ContactRelativePath));
            Debug.Log("Pressure pistol component report written to " + Path.Combine(projectRoot, ReportRelativePath));
        }
        finally
        {
            DestroyTexture(contactSheet);
            for (int i = 0; i < results.Count; i++)
            {
                DestroyTexture(results[i].Texture);
            }

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }
    }

    private static ComponentRenderResult RenderSingleComponent(string projectRoot, string key, string title, string relativePath, Vector3 cameraPosition, Vector3 lookAt, float fieldOfView, ComponentBuilder builder)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings();

        LookDevMaterials materials = LookDevMaterials.Create(projectRoot);
        ComponentMetric metric = new ComponentMetric
        {
            key = key,
            title = title,
            renderPath = relativePath,
            width = ComponentWidth,
            height = ComponentHeight,
            cameraPosition = FormatVector(cameraPosition),
            cameraLookAt = FormatVector(lookAt),
            fieldOfView = fieldOfView,
            usesUnityOnlyTemporaryGeometry = true,
            usesFinalMaterialTextures = materials.LoadedTextureFamilies,
            status = "initial pass"
        };

        Texture2D image = null;
        try
        {
            GameObject background = new GameObject("PressurePistolComponents_Background");
            BuildComponentBackdrop(background.transform, materials);

            GameObject root = new GameObject("PressurePistolComponent_" + key);
            builder(root.transform, materials, metric);

            BuildLighting(key);
            Camera camera = CreateCamera(cameraPosition, lookAt, fieldOfView);
            image = CaptureCamera(camera, ComponentWidth, ComponentHeight);
            image.hideFlags = HideFlags.DontUnloadUnusedAsset;
            AnalyzeImage(image, metric);

            string outputPath = Path.Combine(projectRoot, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            File.WriteAllBytes(outputPath, image.EncodeToPNG());
            Debug.Log("Pressure pistol component render written to " + outputPath);

            return new ComponentRenderResult
            {
                Key = key,
                Title = title,
                Texture = image,
                Metric = metric
            };
        }
        catch
        {
            DestroyTexture(image);
            throw;
        }
        finally
        {
            materials.DestroyAll();
        }
    }

    private delegate void ComponentBuilder(Transform root, LookDevMaterials materials, ComponentMetric metric);

    private static void ConfigureRenderSettings()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.028f, 0.023f, 0.019f, 1f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.025f, 0.021f, 0.018f, 1f);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.012f;
        RenderSettings.reflectionIntensity = 0.62f;
    }

    private static void BuildComponentBackdrop(Transform root, LookDevMaterials materials)
    {
        CreateBox("dark riveted iron inspection backing", new Vector3(0f, 0f, 0.62f), new Vector3(4.4f, 2.55f, 0.08f), Quaternion.identity, materials.ContactBack, root);
        CreateBox("low oily brassworks ledge", new Vector3(0f, -0.84f, 0.08f), new Vector3(4.3f, 0.055f, 1.3f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("warm reflected ledge streak", new Vector3(-0.38f, -0.805f, -0.17f), new Vector3(2.2f, 0.016f, 0.018f), Quaternion.identity, materials.WetHighlight, root);
        CreateCylinderX("rear brass service pipe", new Vector3(-0.18f, 0.82f, 0.46f), 3.35f, 0.022f, materials.DarkBrass, root);
        CreateCylinderX("rear dark pressure pipe", new Vector3(0.12f, 0.98f, 0.45f), 3.65f, 0.034f, materials.DarkPipeMetal, root);
        CreateCylinderX("rear copper heat line", new Vector3(0.3f, -0.64f, 0.38f), 2.55f, 0.018f, materials.AgedCopper, root);
        AddRivetRow("backing upper rivet", new Vector3(-2.02f, 1.02f, 0.565f), new Vector3(2.02f, 1.02f, 0.565f), 17, 0.012f, materials.AgedBrass, root, null);
        AddRivetRow("backing lower rivet", new Vector3(-2.02f, -0.68f, 0.565f), new Vector3(2.02f, -0.68f, 0.565f), 17, 0.012f, materials.DarkBrass, root, null);
    }

    private static void BuildLighting(string key)
    {
        CreateLight("warm amber component key", LightType.Spot, new Vector3(-2.2f, 1.9f, -2.15f), Quaternion.Euler(42f, 36f, 0f), new Color(1f, 0.58f, 0.23f), 5.1f, 46f, true);
        CreateLight("cool black-iron fill", LightType.Directional, new Vector3(1.2f, 1.25f, -1.2f), Quaternion.Euler(35f, -134f, 0f), new Color(0.16f, 0.18f, 0.22f), 0.25f, 0f, false);
        CreateLight("brass rim skim", LightType.Spot, new Vector3(1.65f, 1.15f, 0.95f), Quaternion.Euler(139f, -33f, 0f), new Color(1f, 0.66f, 0.3f), 2.55f, 38f, true);

        if (key == "coil_pack")
        {
            CreateLight("coil embedded heat glow", LightType.Point, new Vector3(0f, 0f, -0.32f), Quaternion.identity, new Color(1f, 0.25f, 0.055f), 1.85f, 0f, false);
        }

        if (key == "gauge_dial")
        {
            CreateLight("gauge glass pin glint", LightType.Point, new Vector3(-0.48f, 0.44f, -0.72f), Quaternion.identity, new Color(1f, 0.84f, 0.46f), 1.2f, 0f, false);
        }
    }

    private static Camera CreateCamera(Vector3 position, Vector3 lookAt, float fieldOfView)
    {
        GameObject cameraObject = new GameObject("Pressure pistol component lookdev camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = position;
        camera.transform.LookAt(lookAt);
        camera.fieldOfView = fieldOfView;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 30f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.012f, 0.011f, 0.01f, 1f);
        camera.allowHDR = true;
        camera.allowMSAA = true;
        return camera;
    }

    private static void BuildCoilPack(Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        root.rotation = Quaternion.Euler(0f, -7f, 0f);

        CreateBox("coil blackened recessed backplate", new Vector3(0f, 0f, 0.04f), new Vector3(2.08f, 0.86f, 0.115f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("coil sooty inner shadow window", new Vector3(0f, 0f, -0.035f), new Vector3(1.78f, 0.58f, 0.045f), Quaternion.identity, materials.LineDark, root);
        CreateBox("coil upper aged brass frame rail", new Vector3(0f, 0.5f, -0.055f), new Vector3(2.2f, 0.09f, 0.1f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil lower aged brass frame rail", new Vector3(0f, -0.5f, -0.055f), new Vector3(2.2f, 0.09f, 0.1f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil left bolted brass cheek", new Vector3(-1.1f, 0f, -0.04f), new Vector3(0.1f, 0.95f, 0.11f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil right bolted brass cheek", new Vector3(1.1f, 0f, -0.04f), new Vector3(0.1f, 0.95f, 0.11f), Quaternion.identity, materials.AgedBrass, root);
        metric.platesOrBrackets += 6;

        CreateCylinderX("coil upper copper manifold", new Vector3(0f, 0.33f, -0.17f), 1.55f, 0.026f, materials.AgedCopper, root);
        CreateCylinderX("coil lower copper manifold", new Vector3(0f, -0.33f, -0.17f), 1.55f, 0.026f, materials.AgedCopper, root);
        CreateCylinderX("coil warm pressure core", new Vector3(0f, 0f, -0.205f), 1.48f, 0.04f, materials.GlowAmber, root);
        CreateCylinderX("coil hottest center core", new Vector3(0f, 0f, -0.255f), 0.58f, 0.022f, materials.HeatOrange, root);
        metric.pipesOrManifolds += 4;
        metric.emissiveAccents += 2;

        int turns = 11;
        for (int i = 0; i < turns; i++)
        {
            float x = -0.78f + i * 0.156f;
            Material material = materials.AgedCopper;
            if (i == 5)
            {
                material = materials.HeatOrange;
            }
            else if (i == 4 || i == 6)
            {
                material = materials.HotCopper;
            }
            else if (i % 3 == 0)
            {
                material = materials.CopperHighlight;
            }

            CreateCopperCoilLoop("separate nested copper coil turn " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0f, -0.19f - (i % 2) * 0.012f), 0.305f, 0.135f, 0.0175f, material, root);
            CreateBox("coil turn dark occlusion slot " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.01f, 0.014f), new Vector3(0.045f, 0.61f, 0.018f), Quaternion.identity, materials.LineDark, root);
            CreateBox("coil individual heat strip " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0f, -0.307f), new Vector3(0.04f, 0.47f, 0.012f), Quaternion.identity, i == 5 ? materials.HeatOrange : materials.HeatRed, root);
            metric.coilTurns++;
            metric.occlusionBreakups++;
            metric.emissiveAccents++;
        }

        CreateCylinderBetween("coil left cloth-wrapped pressure lead", new Vector3(-1.1f, 0.29f, -0.16f), new Vector3(-0.78f, 0.48f, -0.2f), 0.019f, materials.AgedCopper, root);
        CreateCylinderBetween("coil right cloth-wrapped pressure lead", new Vector3(1.1f, -0.29f, -0.16f), new Vector3(0.78f, -0.48f, -0.2f), 0.019f, materials.AgedCopper, root);
        metric.pipesOrManifolds += 2;

        AddRivetRow("coil top brass slotted screw", new Vector3(-0.94f, 0.52f, -0.15f), new Vector3(0.94f, 0.52f, -0.15f), 10, 0.024f, materials.BrassEdge, root, metric);
        AddRivetRow("coil bottom dark screw", new Vector3(-0.94f, -0.52f, -0.15f), new Vector3(0.94f, -0.52f, -0.15f), 10, 0.021f, materials.DarkBrass, root, metric);
        AddRivetRow("coil left cheek rivets", new Vector3(-1.11f, -0.35f, -0.15f), new Vector3(-1.11f, 0.35f, -0.15f), 4, 0.024f, materials.BrassEdge, root, metric);
        AddRivetRow("coil right cheek rivets", new Vector3(1.11f, -0.35f, -0.15f), new Vector3(1.11f, 0.35f, -0.15f), 4, 0.024f, materials.BrassEdge, root, metric);

        for (int i = 0; i < 7; i++)
        {
            float x = -0.74f + i * 0.25f;
            CreateBox("coil polished edge scratch upper " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.555f, -0.116f), new Vector3(0.1f, 0.008f, 0.008f), Quaternion.Euler(0f, 0f, -8f + i * 3f), materials.BrassEdge, root);
            CreateBox("coil soot vertical grime " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x + 0.04f, -0.46f, -0.118f), new Vector3(0.01f, 0.075f, 0.008f), Quaternion.Euler(0f, 0f, 5f), materials.SootBlack, root);
            metric.surfaceWearMarks += 2;
        }

        CreateSphere("coil left verdigris tarnish", new Vector3(-0.92f, 0.4f, -0.18f), new Vector3(0.07f, 0.033f, 0.014f), materials.PatinaGreen, root);
        CreateSphere("coil right verdigris tarnish", new Vector3(0.86f, -0.42f, -0.18f), new Vector3(0.06f, 0.03f, 0.014f), materials.PatinaGreen, root);
        metric.materialRoles = 7;
        metric.notes = "Coil pack is split into brass frame, dark recessed cavity, separate copper loop turns, hot core, manifolds, lead pipes, rivets, edge scratches, soot strips, and small patina spots.";
    }

    private static void BuildGaugeDial(Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        root.rotation = Quaternion.Euler(0f, -4f, 0f);

        CreateBox("gauge blackened mounting yoke", new Vector3(0f, -0.46f, 0.02f), new Vector3(0.9f, 0.22f, 0.14f), Quaternion.identity, materials.BlackenedIron, root);
        CreateCylinderX("gauge lower pressure pipe", new Vector3(0f, -0.58f, -0.02f), 1.25f, 0.036f, materials.DarkPipeMetal, root);
        CreateCylinderY("gauge top valve post", new Vector3(0f, 0.62f, -0.02f), 0.28f, 0.05f, materials.AgedBrass, root);
        CreateCylinderY("gauge top knurled cap", new Vector3(0f, 0.79f, -0.02f), 0.075f, 0.112f, materials.DarkBrass, root);
        metric.pipesOrManifolds += 2;
        metric.platesOrBrackets += 2;

        CreateCylinderZ("gauge black iron rear cup", new Vector3(0f, 0f, 0.035f), 0.16f, 0.62f, materials.BlackenedIron, root);
        CreateCylinderZ("gauge aged brass outer bezel", new Vector3(0f, 0f, -0.04f), 0.09f, 0.57f, materials.AgedBrass, root);
        CreateCylinderZ("gauge dark inner aged rim", new Vector3(0f, 0f, -0.09f), 0.035f, 0.49f, materials.DarkBrass, root);
        CreateCylinderZ("gauge cream enamel face", new Vector3(0f, 0f, -0.12f), 0.026f, 0.455f, materials.GaugeFace, root);
        metric.bevelRingsOrCollars += 4;

        for (int i = 0; i < 36; i++)
        {
            float angle = -140f + i * (280f / 35f);
            float radians = angle * Mathf.Deg2Rad;
            bool major = i % 5 == 0;
            float radius = major ? 0.362f : 0.377f;
            Vector3 position = new Vector3(Mathf.Cos(radians) * radius, Mathf.Sin(radians) * radius, -0.151f);
            Vector3 scale = major ? new Vector3(0.018f, 0.085f, 0.009f) : new Vector3(0.009f, 0.052f, 0.008f);
            CreateBox("gauge radial tick " + i.ToString(CultureInfo.InvariantCulture), position, scale, Quaternion.Euler(0f, 0f, angle - 90f), materials.LineDark, root);
            metric.gaugeTickMarks++;
        }

        CreateGaugeFaceLabel("0", new Vector3(-0.31f, -0.21f, -0.17f), 0.082f, root);
        CreateGaugeFaceLabel("40", new Vector3(-0.17f, 0.25f, -0.17f), 0.065f, root);
        CreateGaugeFaceLabel("80", new Vector3(0.18f, 0.25f, -0.17f), 0.065f, root);
        CreateGaugeFaceLabel("120", new Vector3(0.32f, -0.2f, -0.17f), 0.06f, root);
        CreateGaugeFaceLabel("PSI", new Vector3(0f, -0.305f, -0.17f), 0.052f, root);
        metric.gaugeNumeralsOrMarkers += 5;

        CreateBox("gauge red pressure needle", new Vector3(0.105f, 0.095f, -0.181f), new Vector3(0.028f, 0.32f, 0.012f), Quaternion.Euler(0f, 0f, -47f), materials.WarningRed, root);
        CreateBox("gauge dark counterweight needle tail", new Vector3(-0.06f, -0.055f, -0.182f), new Vector3(0.024f, 0.16f, 0.011f), Quaternion.Euler(0f, 0f, -47f), materials.LineDark, root);
        CreateSphere("gauge center brass needle hub", new Vector3(0f, 0f, -0.195f), new Vector3(0.095f, 0.095f, 0.022f), materials.BrassEdge, root);
        metric.needles = 1;

        CreateCylinderZ("gauge translucent amber glass", new Vector3(0f, 0f, -0.205f), 0.012f, 0.462f, materials.Glass, root);
        AddGlassArc("gauge upper glass crescent", -154f, -82f, 8, 0.372f, 0.01f, materials.GlassHighlight, root);
        AddGlassArc("gauge lower glass hairline", 12f, 54f, 5, 0.31f, 0.006f, materials.GlassHighlight, root);
        metric.glassHighlights += 2;

        for (int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2f / 10f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.552f, Mathf.Sin(angle) * 0.552f, -0.185f);
            CreateSphere("gauge bezel raised screw " + i.ToString(CultureInfo.InvariantCulture), position, new Vector3(0.052f, 0.052f, 0.026f), materials.BrassEdge, root);
            CreateBox("gauge screw slot " + i.ToString(CultureInfo.InvariantCulture), position + new Vector3(0f, 0f, -0.018f), new Vector3(0.042f, 0.006f, 0.006f), Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg + 20f), materials.LineDark, root);
            metric.fasteners++;
        }

        for (int i = 0; i < 8; i++)
        {
            float angle = i * Mathf.PI * 2f / 8f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.515f, Mathf.Sin(angle) * 0.515f, -0.19f);
            CreateBox("gauge polished rim bite " + i.ToString(CultureInfo.InvariantCulture), position, new Vector3(0.09f, 0.011f, 0.009f), Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg), materials.BrassEdge, root);
            metric.surfaceWearMarks++;
        }

        CreateCylinderBetween("gauge left threaded side port", new Vector3(-0.56f, -0.02f, -0.04f), new Vector3(-0.8f, -0.02f, -0.04f), 0.042f, materials.DarkPipeMetal, root);
        CreateCylinderBetween("gauge right brass vent stub", new Vector3(0.56f, -0.02f, -0.04f), new Vector3(0.79f, -0.02f, -0.04f), 0.035f, materials.AgedBrass, root);
        CreateCylinderZ("gauge left pipe collar", new Vector3(-0.57f, -0.02f, -0.04f), 0.06f, 0.088f, materials.DarkBrass, root);
        CreateCylinderZ("gauge right pipe collar", new Vector3(0.57f, -0.02f, -0.04f), 0.06f, 0.079f, materials.DarkBrass, root);
        metric.pipesOrManifolds += 2;
        metric.bevelRingsOrCollars += 2;
        metric.materialRoles = 6;
        metric.notes = "Gauge is built as a separate rear cup, brass bezel stack, cream enamel face, radial tick set, numerals, red needle, glass layer, side ports, and raised screw heads.";
    }

    private static Texture2D CaptureCamera(Camera camera, int width, int height)
    {
        RenderTexture previous = RenderTexture.active;
        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        renderTexture.antiAliasing = 8;
        renderTexture.Create();

        try
        {
            camera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            camera.Render();

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
            texture.Apply(false, false);
            return texture;
        }
        finally
        {
            camera.targetTexture = null;
            RenderTexture.active = previous;
            renderTexture.Release();
            UnityEngine.Object.DestroyImmediate(renderTexture);
        }
    }

    private static Texture2D BuildContactSheet(string projectRoot, List<ComponentRenderResult> results)
    {
        Texture2D contact = new Texture2D(ContactWidth, ContactHeight, TextureFormat.RGB24, false);
        FillTexture(contact, new Color(0.018f, 0.015f, 0.012f, 1f));

        int margin = 70;
        int gap = 46;
        int cellWidth = (ContactWidth - margin * 2 - gap) / 2;
        int cellHeight = 920;

        for (int i = 0; i < results.Count && i < 2; i++)
        {
            Texture2D source = LoadRenderedPng(projectRoot, results[i].Metric.renderPath);
            int x = margin + i * (cellWidth + gap);
            int y = 170;
            RectInt cell = new RectInt(x, y, cellWidth, cellHeight);
            FillRect(contact, cell, new Color(0.03f, 0.025f, 0.02f, 1f));
            BlitScaled(source, contact, FitRect(source.width, source.height, cell));
            DrawRect(contact, cell, new Color(0.58f, 0.35f, 0.13f, 1f), 3);
            DestroyTexture(source);
        }

        DrawRect(contact, new RectInt(34, 34, ContactWidth - 68, ContactHeight - 68), new Color(0.36f, 0.22f, 0.1f, 1f), 2);
        contact.Apply(false, false);
        return contact;
    }

    private static Texture2D LoadRenderedPng(string projectRoot, string relativePath)
    {
        string path = Path.Combine(projectRoot, relativePath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
        texture.name = "PPCOMP_ContactSource_" + Path.GetFileNameWithoutExtension(relativePath);
        if (!texture.LoadImage(File.ReadAllBytes(path), false))
        {
            throw new InvalidOperationException("Failed to load rendered PNG for contact sheet: " + path);
        }

        texture.Apply(false, false);
        return texture;
    }

    private static string BuildReport(ComponentLaneMetrics metrics)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Pressure Pistol Component Lookdev Render Report");
        builder.AppendLine();
        builder.AppendLine("Status: initial Unity-only component lookdev pass");
        builder.AppendLine("Date: " + metrics.timestamp);
        builder.AppendLine("Unity: " + metrics.unityVersion);
        builder.AppendLine("Entrypoint: `" + metrics.batchmodeEntrypoint + "`");
        builder.AppendLine();
        builder.AppendLine("## Outputs");
        builder.AppendLine();
        builder.AppendLine("- Coil pack: `" + CoilRenderRelativePath + "`");
        builder.AppendLine("- Gauge/dial: `" + GaugeRenderRelativePath + "`");
        builder.AppendLine("- Contact sheet: `" + ContactRelativePath + "`");
        builder.AppendLine("- Metrics: `" + MetricsRelativePath + "`");
        builder.AppendLine();
        builder.AppendLine("## Component Evidence");
        builder.AppendLine();
        builder.AppendLine("| Component | Geometry/detail evidence | Material evidence | Image check |");
        builder.AppendLine("| --- | --- | --- | --- |");
        for (int i = 0; i < metrics.components.Length; i++)
        {
            ComponentMetric component = metrics.components[i];
            string detail = component.coilTurns.ToString(CultureInfo.InvariantCulture) + " coil turns, " +
                            component.gaugeTickMarks.ToString(CultureInfo.InvariantCulture) + " ticks, " +
                            component.fasteners.ToString(CultureInfo.InvariantCulture) + " fasteners, " +
                            component.bevelRingsOrCollars.ToString(CultureInfo.InvariantCulture) + " rings/collars, " +
                            component.surfaceWearMarks.ToString(CultureInfo.InvariantCulture) + " wear marks";
            string materials = component.materialRoles.ToString(CultureInfo.InvariantCulture) + " visual roles, " +
                               component.usesFinalMaterialTextures.ToString(CultureInfo.InvariantCulture) + " FinalMaterialsV1 texture families loaded";
            string image = "avg luminance " + component.averageLuminance.ToString("0.000", CultureInfo.InvariantCulture) +
                           ", near-black " + component.nearBlackPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) +
                           "%, warm highlight " + component.warmHighlightPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + "%";
            builder.AppendLine("| " + component.title + " | " + detail + " | " + materials + " | " + image + " |");
        }

        builder.AppendLine();
        builder.AppendLine("## Read");
        builder.AppendLine();
        builder.AppendLine("This pass deliberately avoids the whole gun. The coil and gauge are isolated components with separate nested Unity primitives, material-family assignments, rivets, scratches, grime, heat/glass accents, and camera/lighting proof. These renders are nonshipping lookdev only; they are intended to decide the component language before a later reassembly pass.");
        builder.AppendLine();
        builder.AppendLine("## Known Gaps");
        builder.AppendLine();
        builder.AppendLine("- Boiler chamber, barrel rings, grip, valves, screw/rivet atlas strategy, and full lighting/camera integration remain checklist-only in this pass.");
        builder.AppendLine("- Geometry is temporary Unity primitive construction, not production mesh topology.");
        builder.AppendLine("- No gameplay prefab, weapon definition, audio, tests, build matrix, or general project status document was touched.");
        return builder.ToString();
    }

    private static void AnalyzeImage(Texture2D image, ComponentMetric metric)
    {
        long sampleCount = 0;
        double luminance = 0.0;
        int nearBlack = 0;
        int warmHighlight = 0;

        for (int y = 0; y < image.height; y += 4)
        {
            for (int x = 0; x < image.width; x += 4)
            {
                Color color = image.GetPixel(x, y);
                float luma = color.r * 0.2126f + color.g * 0.7152f + color.b * 0.0722f;
                luminance += luma;
                if (luma < 0.035f)
                {
                    nearBlack++;
                }

                if (color.r > 0.43f && color.g > 0.2f && color.b < 0.18f)
                {
                    warmHighlight++;
                }

                sampleCount++;
            }
        }

        metric.averageLuminance = sampleCount > 0 ? (float)(luminance / sampleCount) : 0f;
        metric.nearBlackPixelPercent = sampleCount > 0 ? nearBlack * 100f / sampleCount : 0f;
        metric.warmHighlightPixelPercent = sampleCount > 0 ? warmHighlight * 100f / sampleCount : 0f;
        metric.passesNotEmptyCheck = metric.averageLuminance > 0.025f && metric.nearBlackPixelPercent < 92f;
    }

    private static void CreateCopperCoilLoop(string name, Vector3 center, float radiusY, float radiusZ, float tubeRadius, Material material, Transform parent)
    {
        int segments = 20;
        for (int i = 0; i < segments; i++)
        {
            float angleA = i * Mathf.PI * 2f / segments;
            float angleB = (i + 1) * Mathf.PI * 2f / segments;
            Vector3 pointA = center + new Vector3(0f, Mathf.Sin(angleA) * radiusY, Mathf.Cos(angleA) * radiusZ);
            Vector3 pointB = center + new Vector3(0f, Mathf.Sin(angleB) * radiusY, Mathf.Cos(angleB) * radiusZ);
            CreateCylinderBetween(name + " segment " + i.ToString(CultureInfo.InvariantCulture), pointA, pointB, tubeRadius, material, parent);
        }
    }

    private static void AddGlassArc(string prefix, float startAngle, float endAngle, int segments, float radius, float tubeRadius, Material material, Transform parent)
    {
        for (int i = 0; i < segments; i++)
        {
            float t0 = i / (float)segments;
            float t1 = (i + 1) / (float)segments;
            float angleA = Mathf.Lerp(startAngle, endAngle, t0) * Mathf.Deg2Rad;
            float angleB = Mathf.Lerp(startAngle, endAngle, t1) * Mathf.Deg2Rad;
            Vector3 pointA = new Vector3(Mathf.Cos(angleA) * radius, Mathf.Sin(angleA) * radius, -0.218f);
            Vector3 pointB = new Vector3(Mathf.Cos(angleB) * radius, Mathf.Sin(angleB) * radius, -0.218f);
            CreateCylinderBetween(prefix + " segment " + i.ToString(CultureInfo.InvariantCulture), pointA, pointB, tubeRadius, material, parent);
        }
    }

    private static void AddRivetRow(string prefix, Vector3 start, Vector3 end, int count, float radius, Material material, Transform parent, ComponentMetric metric)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            CreateSphere(prefix + " " + i.ToString(CultureInfo.InvariantCulture), Vector3.Lerp(start, end, t), Vector3.one * (radius * 2f), material, parent);
            if (metric != null)
            {
                metric.fasteners++;
            }
        }
    }

    private static Light CreateLight(string name, LightType type, Vector3 position, Quaternion rotation, Color color, float intensity, float spotAngle, bool shadows)
    {
        GameObject lightObject = new GameObject(name);
        lightObject.transform.position = position;
        lightObject.transform.rotation = rotation;
        Light light = lightObject.AddComponent<Light>();
        light.type = type;
        light.color = color;
        light.intensity = intensity;
        light.range = type == LightType.Point ? 4.5f : 10f;
        if (type == LightType.Spot)
        {
            light.spotAngle = spotAngle;
            light.range = 10f;
        }

        light.shadows = shadows ? LightShadows.Soft : LightShadows.None;
        light.shadowStrength = 0.62f;
        return light;
    }

    private static GameObject CreateCylinderX(string name, Vector3 position, float length, float radius, Material material, Transform parent)
    {
        return CreateCylinder(name, position, Quaternion.Euler(0f, 0f, 90f), length, radius, material, parent);
    }

    private static GameObject CreateCylinderY(string name, Vector3 position, float length, float radius, Material material, Transform parent)
    {
        return CreateCylinder(name, position, Quaternion.identity, length, radius, material, parent);
    }

    private static GameObject CreateCylinderZ(string name, Vector3 position, float length, float radius, Material material, Transform parent)
    {
        return CreateCylinder(name, position, Quaternion.Euler(90f, 0f, 0f), length, radius, material, parent);
    }

    private static GameObject CreateCylinder(string name, Vector3 position, Quaternion rotation, float length, float radius, Material material, Transform parent)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, true);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.transform.localScale = new Vector3(radius * 2f, length * 0.5f, radius * 2f);
        AssignMaterial(gameObject, material);
        return gameObject;
    }

    private static GameObject CreateCylinderBetween(string name, Vector3 start, Vector3 end, float radius, Material material, Transform parent)
    {
        Vector3 midpoint = (start + end) * 0.5f;
        Vector3 direction = end - start;
        float length = direction.magnitude;
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, true);
        gameObject.transform.position = midpoint;
        gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
        gameObject.transform.localScale = new Vector3(radius * 2f, length * 0.5f, radius * 2f);
        AssignMaterial(gameObject, material);
        return gameObject;
    }

    private static GameObject CreateBox(string name, Vector3 position, Vector3 scale, Quaternion rotation, Material material, Transform parent)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, true);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.transform.localScale = scale;
        AssignMaterial(gameObject, material);
        return gameObject;
    }

    private static GameObject CreateSphere(string name, Vector3 position, Vector3 scale, Material material, Transform parent)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, true);
        gameObject.transform.position = position;
        gameObject.transform.localScale = scale;
        AssignMaterial(gameObject, material);
        return gameObject;
    }

    private static void CreateGaugeFaceLabel(string text, Vector3 position, float characterSize, Transform parent)
    {
        GameObject labelObject = new GameObject("gauge face label " + text);
        labelObject.transform.SetParent(parent, true);
        labelObject.transform.position = position;
        labelObject.transform.rotation = Quaternion.identity;
        TextMesh textMesh = labelObject.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.characterSize = characterSize;
        textMesh.fontSize = 96;
        textMesh.color = new Color(0.038f, 0.027f, 0.018f, 1f);
    }

    private static void AssignMaterial(GameObject gameObject, Material material)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }
    }

    private static void FillTexture(Texture2D texture, Color color)
    {
        Color32 fill = color;
        Color32[] pixels = new Color32[texture.width * texture.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = fill;
        }

        texture.SetPixels32(pixels);
    }

    private static void FillRect(Texture2D texture, RectInt rect, Color color)
    {
        int xMin = Mathf.Clamp(rect.xMin, 0, texture.width);
        int xMax = Mathf.Clamp(rect.xMax, 0, texture.width);
        int yMin = Mathf.Clamp(rect.yMin, 0, texture.height);
        int yMax = Mathf.Clamp(rect.yMax, 0, texture.height);
        for (int y = yMin; y < yMax; y++)
        {
            for (int x = xMin; x < xMax; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
    }

    private static void DrawRect(Texture2D texture, RectInt rect, Color color, int thickness)
    {
        FillRect(texture, new RectInt(rect.xMin, rect.yMin, rect.width, thickness), color);
        FillRect(texture, new RectInt(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        FillRect(texture, new RectInt(rect.xMin, rect.yMin, thickness, rect.height), color);
        FillRect(texture, new RectInt(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }

    private static RectInt FitRect(int sourceWidth, int sourceHeight, RectInt bounds)
    {
        float scale = Mathf.Min(bounds.width / (float)sourceWidth, bounds.height / (float)sourceHeight);
        int width = Mathf.RoundToInt(sourceWidth * scale);
        int height = Mathf.RoundToInt(sourceHeight * scale);
        return new RectInt(bounds.xMin + (bounds.width - width) / 2, bounds.yMin + (bounds.height - height) / 2, width, height);
    }

    private static void BlitScaled(Texture2D source, Texture2D destination, RectInt target)
    {
        for (int y = 0; y < target.height; y++)
        {
            float v = y / Mathf.Max(1f, target.height - 1f);
            int sourceY = Mathf.Clamp(Mathf.RoundToInt(v * (source.height - 1)), 0, source.height - 1);
            for (int x = 0; x < target.width; x++)
            {
                float u = x / Mathf.Max(1f, target.width - 1f);
                int sourceX = Mathf.Clamp(Mathf.RoundToInt(u * (source.width - 1)), 0, source.width - 1);
                destination.SetPixel(target.xMin + x, target.yMin + y, source.GetPixel(sourceX, sourceY));
            }
        }
    }

    private static string FormatVector(Vector3 vector)
    {
        return vector.x.ToString("0.00", CultureInfo.InvariantCulture) + ", " +
               vector.y.ToString("0.00", CultureInfo.InvariantCulture) + ", " +
               vector.z.ToString("0.00", CultureInfo.InvariantCulture);
    }

    private static void DestroyTexture(Texture2D texture)
    {
        if (texture != null)
        {
            UnityEngine.Object.DestroyImmediate(texture);
        }
    }

    private static void SetMaterialColor(Material material, Color color)
    {
        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }

        if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", color);
        }

        material.color = color;
    }

    private static void SetMaterialColor(Material material, string property, Color color)
    {
        if (material.HasProperty(property))
        {
            material.SetColor(property, color);
        }
    }

    private static void SetMaterialTexture(Material material, string property, Texture texture)
    {
        if (texture != null && material.HasProperty(property))
        {
            material.SetTexture(property, texture);
        }
    }

    private static void SetMaterialFloat(Material material, string property, float value)
    {
        if (material.HasProperty(property))
        {
            material.SetFloat(property, value);
        }
    }

    private static void SetMaterialInt(Material material, string property, int value)
    {
        if (material.HasProperty(property))
        {
            material.SetInt(property, value);
        }
    }

    private sealed class LookDevMaterials
    {
        private readonly List<Texture> ownedTextures = new List<Texture>();
        private readonly List<Material> ownedMaterials = new List<Material>();

        public Material BlackenedIron;
        public Material AgedBrass;
        public Material DarkBrass;
        public Material BrassEdge;
        public Material AgedCopper;
        public Material HotCopper;
        public Material CopperHighlight;
        public Material GaugeFace;
        public Material Glass;
        public Material GlassHighlight;
        public Material DarkPipeMetal;
        public Material LineDark;
        public Material WarningRed;
        public Material GlowAmber;
        public Material HeatOrange;
        public Material HeatRed;
        public Material SootBlack;
        public Material PatinaGreen;
        public Material ContactBack;
        public Material WetHighlight;
        public int LoadedTextureFamilies;

        public static LookDevMaterials Create(string projectRoot)
        {
            LookDevMaterials materials = new LookDevMaterials();
            materials.BlackenedIron = materials.CreateFinalMaterial(projectRoot, "BlackenedRivetedIron", "PPCOMP_BlackenedRivetedIron", 0.82f, 0.35f, new Color(0.82f, 0.8f, 0.76f, 1f), new Vector2(1.4f, 1.4f));
            materials.AgedBrass = materials.CreateFinalMaterial(projectRoot, "AgedBrass", "PPCOMP_AgedBrass", 0.9f, 0.58f, Color.white, new Vector2(1.2f, 1.2f));
            materials.AgedCopper = materials.CreateFinalMaterial(projectRoot, "CopperPipe", "PPCOMP_CopperPipe", 0.88f, 0.55f, Color.white, new Vector2(1.5f, 1f));
            materials.GaugeFace = materials.CreateFinalMaterial(projectRoot, "CreamEnamelGauge", "PPCOMP_CreamGauge", 0f, 0.42f, Color.white, Vector2.one);
            materials.DarkPipeMetal = materials.CreateFinalMaterial(projectRoot, "BlackenedRivetedIron", "PPCOMP_DarkPipeMetal", 0.86f, 0.28f, new Color(0.64f, 0.61f, 0.57f, 1f), new Vector2(1.8f, 1.8f));

            materials.DarkBrass = materials.CreateColorMaterial("PPCOMP_DarkAgedBrass", new Color(0.36f, 0.21f, 0.08f, 1f), 0.88f, 0.42f, false);
            materials.BrassEdge = materials.CreateColorMaterial("PPCOMP_PolishedBrassEdge", new Color(0.98f, 0.66f, 0.27f, 1f), 0.9f, 0.72f, false);
            materials.HotCopper = materials.CreateColorMaterial("PPCOMP_HotCopper", new Color(0.86f, 0.32f, 0.12f, 1f), 0.82f, 0.56f, true);
            materials.CopperHighlight = materials.CreateColorMaterial("PPCOMP_CopperHighlight", new Color(1f, 0.49f, 0.17f, 1f), 0.78f, 0.67f, true);
            materials.Glass = materials.CreateTransparentMaterial("PPCOMP_AmberGaugeGlass", new Color(0.92f, 0.68f, 0.34f, 0.27f), 0f, 0.95f);
            materials.GlassHighlight = materials.CreateTransparentMaterial("PPCOMP_GlassHighlight", new Color(1f, 0.86f, 0.58f, 0.58f), 0f, 0.98f);
            materials.LineDark = materials.CreateUnlitMaterial("PPCOMP_DarkInk", new Color(0.03f, 0.022f, 0.015f, 1f), false);
            materials.WarningRed = materials.CreateColorMaterial("PPCOMP_RedNeedle", new Color(0.78f, 0.055f, 0.028f, 1f), 0.02f, 0.5f, false);
            materials.GlowAmber = materials.CreateUnlitMaterial("PPCOMP_PressureCoreGlow", new Color(1f, 0.34f, 0.045f, 1f), true);
            materials.HeatOrange = materials.CreateUnlitMaterial("PPCOMP_HeatOrange", new Color(1f, 0.43f, 0.06f, 1f), true);
            materials.HeatRed = materials.CreateUnlitMaterial("PPCOMP_DullHeatRed", new Color(0.54f, 0.08f, 0.025f, 1f), true);
            materials.SootBlack = materials.CreateUnlitMaterial("PPCOMP_SootBlack", new Color(0.012f, 0.01f, 0.008f, 1f), false);
            materials.PatinaGreen = materials.CreateColorMaterial("PPCOMP_PatinaGreen", new Color(0.08f, 0.34f, 0.26f, 1f), 0.05f, 0.45f, false);
            materials.ContactBack = materials.CreateUnlitMaterial("PPCOMP_ContactBack", new Color(0.023f, 0.019f, 0.016f, 1f), false);
            materials.WetHighlight = materials.CreateTransparentMaterial("PPCOMP_WetWarmReflection", new Color(1f, 0.62f, 0.22f, 0.22f), 0f, 0.93f);
            return materials;
        }

        public void DestroyAll()
        {
            for (int i = 0; i < ownedMaterials.Count; i++)
            {
                if (ownedMaterials[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(ownedMaterials[i]);
                }
            }

            for (int i = 0; i < ownedTextures.Count; i++)
            {
                if (ownedTextures[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(ownedTextures[i]);
                }
            }

            ownedMaterials.Clear();
            ownedTextures.Clear();
        }

        private Material CreateFinalMaterial(string projectRoot, string key, string name, float metallic, float smoothness, Color tint, Vector2 tiling)
        {
            Material material = CreateColorMaterial(name, tint, metallic, smoothness, false);
            Texture2D baseMap = LoadTexture(projectRoot, key, "BaseColor", false);
            Texture2D normalMap = LoadTexture(projectRoot, key, "Normal", true);
            Texture2D ormMap = LoadTexture(projectRoot, key, "ORM", true);
            Texture2D metallicSmoothness = ormMap != null ? CreateMetallicSmoothnessFromOrm(ormMap, name + "_MetallicSmoothness") : null;
            Texture2D occlusion = ormMap != null ? CreateOcclusionFromOrm(ormMap, name + "_Occlusion") : null;

            SetMaterialTexture(material, "_MainTex", baseMap);
            SetMaterialTexture(material, "_BaseMap", baseMap);
            SetMaterialTexture(material, "_BumpMap", normalMap);
            SetMaterialTexture(material, "_OcclusionMap", occlusion);
            SetMaterialTexture(material, "_MetallicGlossMap", metallicSmoothness);
            SetMaterialFloat(material, "_Metallic", metallic);
            SetMaterialFloat(material, "_Glossiness", smoothness);
            SetMaterialFloat(material, "_Smoothness", smoothness);
            SetTextureScale(material, "_MainTex", tiling);
            SetTextureScale(material, "_BaseMap", tiling);
            if (baseMap != null)
            {
                material.mainTexture = baseMap;
            }

            if (normalMap != null)
            {
                material.EnableKeyword("_NORMALMAP");
            }

            if (occlusion != null)
            {
                material.EnableKeyword("_OCCLUSIONMAP");
            }

            if (metallicSmoothness != null)
            {
                material.EnableKeyword("_METALLICGLOSSMAP");
            }

            if (baseMap != null || normalMap != null || ormMap != null)
            {
                LoadedTextureFamilies++;
            }

            TrackTexture(baseMap);
            TrackTexture(normalMap);
            TrackTexture(ormMap);
            TrackTexture(metallicSmoothness);
            TrackTexture(occlusion);
            return material;
        }

        private Material CreateColorMaterial(string name, Color color, float metallic, float smoothness, bool emission)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            Material material = new Material(shader);
            material.name = name;
            SetMaterialColor(material, color);
            SetMaterialFloat(material, "_Metallic", metallic);
            SetMaterialFloat(material, "_Glossiness", smoothness);
            SetMaterialFloat(material, "_Smoothness", smoothness);
            if (emission)
            {
                SetMaterialColor(material, "_EmissionColor", color * 1.8f);
                material.EnableKeyword("_EMISSION");
            }

            ownedMaterials.Add(material);
            return material;
        }

        private Material CreateUnlitMaterial(string name, Color color, bool emission)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Unlit/Color");
            }

            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            Material material = new Material(shader);
            material.name = name;
            SetMaterialColor(material, color);
            if (emission)
            {
                SetMaterialColor(material, "_EmissionColor", color * 1.75f);
                material.EnableKeyword("_EMISSION");
            }

            ownedMaterials.Add(material);
            return material;
        }

        private Material CreateTransparentMaterial(string name, Color color, float metallic, float smoothness)
        {
            Material material = CreateColorMaterial(name, color, metallic, smoothness, false);
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = (int)RenderQueue.Transparent + 10;
            SetMaterialFloat(material, "_Surface", 1f);
            SetMaterialFloat(material, "_Mode", 3f);
            SetMaterialInt(material, "_SrcBlend", (int)BlendMode.SrcAlpha);
            SetMaterialInt(material, "_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            SetMaterialInt(material, "_ZWrite", 0);
            SetMaterialInt(material, "_Cull", (int)CullMode.Off);
            material.EnableKeyword("_ALPHABLEND_ON");
            return material;
        }

        private Texture2D LoadTexture(string projectRoot, string key, string role, bool linear)
        {
            string relativePath = "Assets/_Project/ArtStaging/FinalMaterialsV1/Textures/T_BBW_" + key + "_" + role + "_2048.png";
            string absolutePath = Path.Combine(projectRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (!File.Exists(absolutePath))
            {
                return null;
            }

            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, true, linear);
            texture.name = "PPCOMP_Loaded_" + key + "_" + role;
            if (!texture.LoadImage(File.ReadAllBytes(absolutePath), false))
            {
                UnityEngine.Object.DestroyImmediate(texture);
                return null;
            }

            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 4;
            return texture;
        }

        private Texture2D CreateMetallicSmoothnessFromOrm(Texture2D orm, string name)
        {
            Color32[] source = orm.GetPixels32();
            Color32[] output = new Color32[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                byte metallic = source[i].b;
                byte smoothness = (byte)(255 - source[i].g);
                output[i] = new Color32(metallic, metallic, metallic, smoothness);
            }

            Texture2D texture = new Texture2D(orm.width, orm.height, TextureFormat.RGBA32, true, true);
            texture.name = name;
            texture.SetPixels32(output);
            texture.Apply(true, false);
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 4;
            return texture;
        }

        private Texture2D CreateOcclusionFromOrm(Texture2D orm, string name)
        {
            Color32[] source = orm.GetPixels32();
            Color32[] output = new Color32[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                byte occlusion = source[i].r;
                output[i] = new Color32(occlusion, occlusion, occlusion, 255);
            }

            Texture2D texture = new Texture2D(orm.width, orm.height, TextureFormat.RGBA32, true, true);
            texture.name = name;
            texture.SetPixels32(output);
            texture.Apply(true, false);
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 4;
            return texture;
        }

        private void SetTextureScale(Material material, string property, Vector2 scale)
        {
            if (material.HasProperty(property))
            {
                material.SetTextureScale(property, scale);
            }
        }

        private void TrackTexture(Texture texture)
        {
            if (texture != null && !ownedTextures.Contains(texture))
            {
                ownedTextures.Add(texture);
            }
        }
    }

    [Serializable]
    private sealed class ComponentLaneMetrics
    {
        public string timestamp;
        public string unityVersion;
        public string batchmodeEntrypoint;
        public string renderFolder;
        public string contactSheetPath;
        public string reportPath;
        public string metricsPath;
        public string laneScope;
        public string sourceMaterialFolder;
        public string sourceConcept;
        public int componentWidth;
        public int componentHeight;
        public int contactSheetWidth;
        public int contactSheetHeight;
        public ComponentMetric[] components;
    }

    [Serializable]
    private sealed class ComponentMetric
    {
        public string key;
        public string title;
        public string renderPath;
        public string status;
        public string notes;
        public int width;
        public int height;
        public string cameraPosition;
        public string cameraLookAt;
        public float fieldOfView;
        public bool usesUnityOnlyTemporaryGeometry;
        public int usesFinalMaterialTextures;
        public int coilTurns;
        public int gaugeTickMarks;
        public int gaugeNumeralsOrMarkers;
        public int fasteners;
        public int platesOrBrackets;
        public int pipesOrManifolds;
        public int bevelRingsOrCollars;
        public int needles;
        public int glassHighlights;
        public int emissiveAccents;
        public int occlusionBreakups;
        public int surfaceWearMarks;
        public int materialRoles;
        public float averageLuminance;
        public float nearBlackPixelPercent;
        public float warmHighlightPixelPercent;
        public bool passesNotEmptyCheck;
    }

    private sealed class ComponentRenderResult
    {
        public string Key;
        public string Title;
        public Texture2D Texture;
        public ComponentMetric Metric;
    }
}
