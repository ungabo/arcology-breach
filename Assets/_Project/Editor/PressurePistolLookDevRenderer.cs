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
    private const int ContactWidth = 3000;
    private const int ContactHeight = 2200;
    private const string RenderFolderRelativePath = "Documentation/ConceptRenders/PressurePistolComponents";
    private const string ReportFolderRelativePath = "Documentation/AssetProduction/PressurePistolLookDev";
    private const string CoilRenderRelativePath = RenderFolderRelativePath + "/PPCOMP_001_copper_brass_coil_pack.png";
    private const string GaugeRenderRelativePath = RenderFolderRelativePath + "/PPCOMP_002_pressure_gauge_dial.png";
    private const string BoilerRenderRelativePath = RenderFolderRelativePath + "/PPCOMP_003_boiler_pressure_chamber.png";
    private const string BarrelRenderRelativePath = RenderFolderRelativePath + "/PPCOMP_004_barrel_muzzle_assembly.png";
    private const string ValveRenderRelativePath = RenderFolderRelativePath + "/PPCOMP_005_valve_manifold_block.png";
    private const string GripRenderRelativePath = RenderFolderRelativePath + "/PPCOMP_006_leather_grip_trigger_guard.png";
    private const string ContactRelativePath = RenderFolderRelativePath + "/PPCOMP_CONTACTSHEET_002_six_component_refinement.png";
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
            results.Add(RenderSingleComponent(projectRoot, "coil_pack", "Aged copper/brass coil pack", CoilRenderRelativePath, new Vector3(0.36f, 0.14f, -4.25f), new Vector3(0f, -0.03f, -0.06f), 35f, BuildCoilPack));
            results.Add(RenderSingleComponent(projectRoot, "gauge_dial", "Pressure gauge/dial with fittings", GaugeRenderRelativePath, new Vector3(0.06f, 0.04f, -3.9f), new Vector3(0f, 0f, -0.04f), 36f, BuildGaugeDial));
            results.Add(RenderSingleComponent(projectRoot, "boiler_chamber", "Boiler pressure chamber", BoilerRenderRelativePath, new Vector3(0.42f, 0.14f, -4.3f), new Vector3(0f, -0.03f, -0.02f), 35f, BuildBoilerChamber));
            results.Add(RenderSingleComponent(projectRoot, "barrel_muzzle", "Barrel and muzzle assembly", BarrelRenderRelativePath, new Vector3(0.38f, 0.09f, -4.25f), new Vector3(0f, -0.03f, -0.03f), 34f, BuildBarrelMuzzle));
            results.Add(RenderSingleComponent(projectRoot, "valve_manifold", "Valve and manifold block", ValveRenderRelativePath, new Vector3(0.28f, 0.15f, -4.4f), new Vector3(0f, 0f, -0.02f), 36f, BuildValveManifold));
            results.Add(RenderSingleComponent(projectRoot, "grip_trigger", "Leather grip and trigger guard", GripRenderRelativePath, new Vector3(0.28f, 0.07f, -2.95f), new Vector3(0f, -0.04f, -0.04f), 30f, BuildGripTriggerGuard));

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
            status = "refinement pass"
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
            AnalyzeFraming(camera, root.transform, metric);
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
        RenderSettings.ambientLight = new Color(0.02f, 0.017f, 0.014f, 1f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.023f, 0.02f, 0.018f, 1f);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.reflectionIntensity = 0.58f;
    }

    private static void BuildComponentBackdrop(Transform root, LookDevMaterials materials)
    {
        CreateBox("dark riveted iron inspection backing", new Vector3(0f, 0f, 0.78f), new Vector3(4.8f, 2.7f, 0.08f), Quaternion.identity, materials.ContactBack, root);
        CreateBox("low oily brassworks ledge", new Vector3(0f, -0.86f, 0.04f), new Vector3(4.6f, 0.06f, 1.4f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("broken oil reflection one", new Vector3(-0.55f, -0.815f, -0.19f), new Vector3(1.55f, 0.014f, 0.018f), Quaternion.Euler(0f, 0f, 1.5f), materials.WetHighlight, root);
        CreateBox("broken oil reflection two", new Vector3(0.72f, -0.803f, -0.12f), new Vector3(1.05f, 0.012f, 0.016f), Quaternion.Euler(0f, 0f, -2.0f), materials.WetHighlight, root);
        CreateCylinderX("rear brass service pipe", new Vector3(-0.2f, 0.84f, 0.57f), 3.8f, 0.022f, materials.DarkBrass, root);
        CreateCylinderX("rear dark pressure pipe", new Vector3(0.18f, 1.02f, 0.56f), 4.0f, 0.035f, materials.DarkPipeMetal, root);
        CreateCylinderX("rear copper heat line", new Vector3(0.32f, -0.65f, 0.5f), 2.9f, 0.019f, materials.AgedCopper, root);
        AddRivetRow("backing upper rivet", new Vector3(-2.18f, 1.05f, 0.71f), new Vector3(2.18f, 1.05f, 0.71f), 19, 0.011f, materials.DarkBrass, root, null);
        AddRivetRow("backing lower rivet", new Vector3(-2.18f, -0.71f, 0.71f), new Vector3(2.18f, -0.71f, 0.71f), 19, 0.011f, materials.DarkBrass, root, null);
    }

    private static void BuildLighting(string key)
    {
        CreateLight("warm amber component key", LightType.Spot, new Vector3(-2.25f, 1.9f, -2.15f), Quaternion.Euler(42f, 36f, 0f), new Color(1f, 0.55f, 0.22f), 4.6f, 46f, true);
        CreateLight("cool black-iron fill", LightType.Directional, new Vector3(1.2f, 1.25f, -1.2f), Quaternion.Euler(35f, -134f, 0f), new Color(0.14f, 0.16f, 0.2f), 0.22f, 0f, false);
        CreateLight("brass rim skim", LightType.Spot, new Vector3(1.65f, 1.14f, 1.05f), Quaternion.Euler(139f, -33f, 0f), new Color(1f, 0.62f, 0.28f), 2.35f, 40f, true);
        CreateLight("low furnace bounce", LightType.Point, new Vector3(-0.45f, -0.52f, -1.05f), Quaternion.identity, new Color(0.9f, 0.34f, 0.09f), 0.7f, 0f, false);

        if (key == "coil_pack")
        {
            CreateLight("subtle coil embedded heat glow", LightType.Point, new Vector3(0f, 0f, -0.32f), Quaternion.identity, new Color(0.9f, 0.18f, 0.045f), 1.0f, 0f, false);
        }
        else if (key == "gauge_dial")
        {
            CreateLight("gauge glass pin glint", LightType.Point, new Vector3(-0.55f, 0.5f, -0.9f), Quaternion.identity, new Color(1f, 0.78f, 0.42f), 0.95f, 0f, false);
        }
        else if (key == "barrel_muzzle")
        {
            CreateLight("muzzle bore edge glint", LightType.Point, new Vector3(-0.88f, 0.15f, -0.85f), Quaternion.identity, new Color(1f, 0.58f, 0.25f), 0.85f, 0f, false);
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
        root.rotation = Quaternion.Euler(0f, -12f, 0f);

        CreateBox("coil blackened rear plate", new Vector3(0f, 0f, 0.08f), new Vector3(2.22f, 0.92f, 0.13f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("coil deep soot cavity", new Vector3(0f, 0f, -0.02f), new Vector3(1.88f, 0.59f, 0.06f), Quaternion.identity, materials.LineDark, root);
        CreateBox("coil oily lower shadow lip", new Vector3(0f, -0.335f, -0.09f), new Vector3(1.95f, 0.04f, 0.04f), Quaternion.identity, materials.OilBlack, root);
        CreateBox("coil upper blackened steel backing rail", new Vector3(0f, 0.52f, -0.01f), new Vector3(2.3f, 0.12f, 0.115f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("coil lower blackened steel backing rail", new Vector3(0f, -0.52f, -0.01f), new Vector3(2.3f, 0.12f, 0.115f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("coil upper aged brass cap rail", new Vector3(0f, 0.575f, -0.105f), new Vector3(2.18f, 0.055f, 0.07f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil lower aged brass cap rail", new Vector3(0f, -0.575f, -0.105f), new Vector3(2.18f, 0.055f, 0.07f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil left layered cheek plate", new Vector3(-1.12f, 0f, -0.06f), new Vector3(0.12f, 0.98f, 0.12f), Quaternion.identity, materials.DarkBrass, root);
        CreateBox("coil right layered cheek plate", new Vector3(1.12f, 0f, -0.06f), new Vector3(0.12f, 0.98f, 0.12f), Quaternion.identity, materials.DarkBrass, root);
        CreateBox("coil left polished cheek face", new Vector3(-1.18f, 0f, -0.16f), new Vector3(0.055f, 0.82f, 0.045f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil right polished cheek face", new Vector3(1.18f, 0f, -0.16f), new Vector3(0.055f, 0.82f, 0.045f), Quaternion.identity, materials.AgedBrass, root);
        metric.platesOrBrackets += 11;

        CreateCylinderX("coil upper oxidized copper manifold", new Vector3(0f, 0.34f, -0.19f), 1.62f, 0.026f, materials.AgedCopper, root);
        CreateCylinderX("coil lower oxidized copper manifold", new Vector3(0f, -0.34f, -0.19f), 1.62f, 0.026f, materials.AgedCopper, root);
        CreateCylinderX("coil dull red ceramic core", new Vector3(0f, 0f, -0.245f), 1.45f, 0.035f, materials.HeatRed, root);
        CreateCylinderX("coil small hottest core slit", new Vector3(0f, 0f, -0.3f), 0.48f, 0.012f, materials.HeatOrange, root);
        metric.pipesOrManifolds += 4;
        metric.emissiveAccents += 2;

        int turns = 12;
        for (int i = 0; i < turns; i++)
        {
            float x = -0.82f + i * 0.149f;
            Material material = materials.AgedCopper;
            if (i == 5 || i == 6)
            {
                material = materials.HotCopper;
            }
            else if (i == 4 || i == 7)
            {
                material = materials.CopperHighlight;
            }

            CreateCopperCoilLoop("separate oxidized copper coil turn " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0f, -0.21f - (i % 2) * 0.016f), 0.305f, 0.135f, 0.016f, material, root);
            CreateBox("coil vertical deep gap " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.01f, 0.004f), new Vector3(0.05f, 0.61f, 0.02f), Quaternion.identity, materials.LineDark, root);
            CreateBox("coil lower soot stain " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x + 0.018f, -0.355f, -0.17f), new Vector3(0.025f, 0.12f, 0.012f), Quaternion.Euler(0f, 0f, -4f), materials.SootBlack, root);
            metric.coilTurns++;
            metric.occlusionBreakups += 2;
        }

        CreateCylinderBetween("coil left braided pressure lead", new Vector3(-1.16f, 0.28f, -0.18f), new Vector3(-0.82f, 0.49f, -0.24f), 0.02f, materials.DarkPipeMetal, root);
        CreateCylinderBetween("coil left copper lead glint", new Vector3(-1.12f, 0.25f, -0.245f), new Vector3(-0.84f, 0.43f, -0.29f), 0.009f, materials.AgedCopper, root);
        CreateCylinderBetween("coil right braided pressure lead", new Vector3(1.16f, -0.28f, -0.18f), new Vector3(0.82f, -0.49f, -0.24f), 0.02f, materials.DarkPipeMetal, root);
        CreateCylinderBetween("coil right copper lead glint", new Vector3(1.12f, -0.25f, -0.245f), new Vector3(0.84f, -0.43f, -0.29f), 0.009f, materials.AgedCopper, root);
        metric.pipesOrManifolds += 4;

        AddSlottedRivetRow("coil top slotted screw", new Vector3(-0.98f, 0.585f, -0.17f), new Vector3(0.98f, 0.585f, -0.17f), 11, 0.022f, materials.BrassEdge, materials.LineDark, root, metric);
        AddSlottedRivetRow("coil bottom slotted dark screw", new Vector3(-0.98f, -0.585f, -0.17f), new Vector3(0.98f, -0.585f, -0.17f), 11, 0.02f, materials.DarkBrass, materials.LineDark, root, metric);
        AddSlottedRivetRow("coil left cheek screw", new Vector3(-1.2f, -0.36f, -0.19f), new Vector3(-1.2f, 0.36f, -0.19f), 5, 0.021f, materials.BrassEdge, materials.LineDark, root, metric);
        AddSlottedRivetRow("coil right cheek screw", new Vector3(1.2f, -0.36f, -0.19f), new Vector3(1.2f, 0.36f, -0.19f), 5, 0.021f, materials.BrassEdge, materials.LineDark, root, metric);

        for (int i = 0; i < 9; i++)
        {
            float x = -0.82f + i * 0.205f;
            CreateBox("coil brass scratch " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.602f, -0.144f), new Vector3(0.1f, 0.006f, 0.007f), Quaternion.Euler(0f, 0f, -10f + i * 3f), materials.BrassEdge, root);
            CreateBox("coil oil run " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x + 0.03f, -0.485f, -0.13f), new Vector3(0.012f, 0.105f, 0.009f), Quaternion.Euler(0f, 0f, 4f), materials.OilBlack, root);
            metric.surfaceWearMarks += 2;
        }

        CreateSphere("coil left patina bloom", new Vector3(-0.94f, 0.42f, -0.2f), new Vector3(0.075f, 0.035f, 0.015f), materials.PatinaGreen, root);
        CreateSphere("coil right patina bloom", new Vector3(0.9f, -0.42f, -0.2f), new Vector3(0.065f, 0.032f, 0.014f), materials.PatinaGreen, root);
        metric.materialRoles = 8;
        metric.notes = "Refined darker coil: aged brass and blackened steel frame, separate oxidized copper turns, smaller heat core, oil runs, soot, patina, slotted screws, and visible pressure leads. Still a primitive proof rather than authored mesh detail.";
    }

    private static void BuildGaugeDial(Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        root.rotation = Quaternion.Euler(0f, -7f, 0f);

        CreateBox("gauge blackened mounting yoke", new Vector3(0f, -0.5f, 0.03f), new Vector3(1.12f, 0.24f, 0.16f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("gauge brass yoke face", new Vector3(0f, -0.48f, -0.075f), new Vector3(0.98f, 0.08f, 0.045f), Quaternion.identity, materials.DarkBrass, root);
        CreateCylinderX("gauge lower blackened pressure pipe", new Vector3(0f, -0.64f, -0.02f), 1.58f, 0.038f, materials.DarkPipeMetal, root);
        CreateCylinderX("gauge lower brass sleeve", new Vector3(0f, -0.64f, -0.095f), 0.7f, 0.047f, materials.DarkBrass, root);
        CreateCylinderY("gauge top valve post", new Vector3(0f, 0.63f, -0.02f), 0.3f, 0.052f, materials.AgedBrass, root);
        CreateCylinderY("gauge top knurled cap", new Vector3(0f, 0.82f, -0.02f), 0.08f, 0.116f, materials.DarkBrass, root);
        metric.pipesOrManifolds += 4;
        metric.platesOrBrackets += 3;

        CreateCylinderZ("gauge black iron rear cup", new Vector3(0f, 0f, 0.05f), 0.19f, 0.64f, materials.BlackenedIron, root);
        CreateCylinderZ("gauge rear shadow collar", new Vector3(0f, 0f, -0.015f), 0.06f, 0.615f, materials.OilBlack, root);
        CreateCylinderZ("gauge aged brass outer bezel", new Vector3(0f, 0f, -0.07f), 0.09f, 0.58f, materials.AgedBrass, root);
        CreateCylinderZ("gauge dark inner aged rim", new Vector3(0f, 0f, -0.12f), 0.04f, 0.505f, materials.DarkBrass, root);
        CreateCylinderZ("gauge cream enamel face", new Vector3(0f, 0f, -0.152f), 0.025f, 0.455f, materials.GaugeFace, root);
        metric.bevelRingsOrCollars += 5;

        for (int i = 0; i < 40; i++)
        {
            float angle = -145f + i * (290f / 39f);
            float radians = angle * Mathf.Deg2Rad;
            bool major = i % 5 == 0;
            float radius = major ? 0.358f : 0.378f;
            Vector3 position = new Vector3(Mathf.Cos(radians) * radius, Mathf.Sin(radians) * radius, -0.18f);
            Vector3 scale = major ? new Vector3(0.018f, 0.085f, 0.008f) : new Vector3(0.008f, 0.052f, 0.007f);
            CreateBox("gauge radial tick " + i.ToString(CultureInfo.InvariantCulture), position, scale, Quaternion.Euler(0f, 0f, angle - 90f), materials.LineDark, root);
            metric.gaugeTickMarks++;
        }

        CreateGaugeFaceLabel("0", new Vector3(-0.31f, -0.22f, -0.2f), 0.078f, root);
        CreateGaugeFaceLabel("40", new Vector3(-0.18f, 0.25f, -0.2f), 0.062f, root);
        CreateGaugeFaceLabel("80", new Vector3(0.18f, 0.25f, -0.2f), 0.062f, root);
        CreateGaugeFaceLabel("120", new Vector3(0.32f, -0.2f, -0.2f), 0.058f, root);
        CreateGaugeFaceLabel("PSI", new Vector3(0f, -0.31f, -0.2f), 0.05f, root);
        metric.gaugeNumeralsOrMarkers += 5;

        CreateBox("gauge red pressure needle", new Vector3(0.12f, 0.105f, -0.212f), new Vector3(0.024f, 0.33f, 0.01f), Quaternion.Euler(0f, 0f, -49f), materials.WarningRed, root);
        CreateBox("gauge black counterweight needle tail", new Vector3(-0.07f, -0.065f, -0.214f), new Vector3(0.02f, 0.17f, 0.01f), Quaternion.Euler(0f, 0f, -49f), materials.LineDark, root);
        CreateSphere("gauge center brass needle hub", new Vector3(0f, 0f, -0.225f), new Vector3(0.095f, 0.095f, 0.022f), materials.BrassEdge, root);
        metric.needles = 1;

        CreateCylinderZ("gauge transparent amber glass", new Vector3(0f, 0f, -0.232f), 0.012f, 0.465f, materials.Glass, root);
        AddGlassArc("gauge upper glass crescent", -154f, -82f, 8, 0.373f, 0.01f, materials.GlassHighlight, root);
        AddGlassArc("gauge lower glass hairline", 12f, 54f, 5, 0.31f, 0.006f, materials.GlassHighlight, root);
        metric.glassHighlights += 2;

        for (int i = 0; i < 12; i++)
        {
            float angle = i * Mathf.PI * 2f / 12f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.565f, Mathf.Sin(angle) * 0.565f, -0.21f);
            CreateSphere("gauge bezel raised screw " + i.ToString(CultureInfo.InvariantCulture), position, new Vector3(0.047f, 0.047f, 0.023f), materials.BrassEdge, root);
            CreateBox("gauge screw slot " + i.ToString(CultureInfo.InvariantCulture), position + new Vector3(0f, 0f, -0.018f), new Vector3(0.038f, 0.005f, 0.006f), Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg + 20f), materials.LineDark, root);
            metric.fasteners++;
        }

        for (int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2f / 10f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.522f, Mathf.Sin(angle) * 0.522f, -0.22f);
            CreateBox("gauge polished rim bite " + i.ToString(CultureInfo.InvariantCulture), position, new Vector3(0.075f, 0.009f, 0.008f), Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg), materials.BrassEdge, root);
            metric.surfaceWearMarks++;
        }

        CreateCylinderBetween("gauge left threaded side port", new Vector3(-0.58f, -0.02f, -0.05f), new Vector3(-0.88f, -0.02f, -0.05f), 0.042f, materials.DarkPipeMetal, root);
        CreateCylinderBetween("gauge right brass vent stub", new Vector3(0.58f, -0.02f, -0.05f), new Vector3(0.88f, -0.02f, -0.05f), 0.036f, materials.AgedBrass, root);
        CreateCylinderZ("gauge left pipe collar", new Vector3(-0.59f, -0.02f, -0.05f), 0.06f, 0.088f, materials.DarkBrass, root);
        CreateCylinderZ("gauge right pipe collar", new Vector3(0.59f, -0.02f, -0.05f), 0.06f, 0.079f, materials.DarkBrass, root);
        metric.pipesOrManifolds += 2;
        metric.bevelRingsOrCollars += 2;
        metric.materialRoles = 7;
        metric.notes = "Refined gauge uses a wider camera, more breathing room around side fittings, blackened rear cup, oily yoke, brass collar stack, cream dial, red needle, glass glints, raised slotted screws, and polished rim bites.";
    }

    private static void BuildBoilerChamber(Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        root.rotation = Quaternion.Euler(0f, -18f, 0f);

        CreateCylinderX("boiler main blackened pressure tank", new Vector3(0f, 0f, -0.08f), 1.74f, 0.34f, materials.BlackenedIron, root);
        CreateSphere("boiler left domed cap", new Vector3(-0.88f, 0f, -0.08f), new Vector3(0.2f, 0.68f, 0.68f), materials.BlackenedIron, root);
        CreateSphere("boiler right domed cap", new Vector3(0.88f, 0f, -0.08f), new Vector3(0.2f, 0.68f, 0.68f), materials.BlackenedIron, root);
        metric.platesOrBrackets += 3;

        for (int i = 0; i < 5; i++)
        {
            float x = -0.68f + i * 0.34f;
            CreateCylinderX("boiler aged brass band " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0f, -0.09f), 0.075f, 0.365f, i == 2 ? materials.AgedBrass : materials.DarkBrass, root);
            AddSlottedRivetRow("boiler band rivet row top " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.35f, -0.16f), new Vector3(x, 0.35f, -0.02f), 3, 0.017f, materials.BrassEdge, materials.LineDark, root, metric);
            AddSlottedRivetRow("boiler band rivet row low " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.35f, -0.16f), new Vector3(x, -0.35f, -0.02f), 3, 0.017f, materials.DarkBrass, materials.LineDark, root, metric);
            metric.bevelRingsOrCollars++;
        }

        CreateCylinderY("boiler top fill neck", new Vector3(-0.36f, 0.42f, -0.1f), 0.22f, 0.055f, materials.AgedBrass, root);
        CreateCylinderY("boiler top fill cap", new Vector3(-0.36f, 0.56f, -0.1f), 0.07f, 0.12f, materials.DarkBrass, root);
        CreateCylinderY("boiler relief valve stem", new Vector3(0.34f, 0.42f, -0.08f), 0.2f, 0.038f, materials.DarkPipeMetal, root);
        CreateCylinderY("boiler small brass relief cap", new Vector3(0.34f, 0.56f, -0.08f), 0.055f, 0.088f, materials.AgedBrass, root);
        CreateCylinderBetween("boiler rear copper feed pipe", new Vector3(-0.8f, -0.18f, -0.1f), new Vector3(-1.12f, -0.45f, -0.18f), 0.029f, materials.AgedCopper, root);
        CreateCylinderBetween("boiler front dark discharge pipe", new Vector3(0.8f, -0.18f, -0.1f), new Vector3(1.12f, -0.45f, -0.18f), 0.035f, materials.DarkPipeMetal, root);
        metric.pipesOrManifolds += 6;

        CreateBox("boiler left bracket foot", new Vector3(-0.46f, -0.48f, -0.08f), new Vector3(0.32f, 0.09f, 0.16f), Quaternion.identity, materials.DarkBrass, root);
        CreateBox("boiler right bracket foot", new Vector3(0.46f, -0.48f, -0.08f), new Vector3(0.32f, 0.09f, 0.16f), Quaternion.identity, materials.DarkBrass, root);
        metric.platesOrBrackets += 2;

        AddSootPatches("boiler soot scar", 9, new Vector3(-0.55f, 0.09f, -0.42f), 0.16f, 0.34f, root, materials, metric);
        AddOilRuns("boiler vertical oil run", 9, -0.7f, 0.2f, -0.24f, root, materials, metric);
        metric.materialRoles = 6;
        metric.notes = "New boiler component: blackened pressure tank with domed caps, brass bands, fill and relief caps, feed/discharge pipes, bracket feet, rivets, soot scars, and oil runs. It reads chunky and mechanical but still needs real mesh bevels for production.";
    }

    private static void BuildBarrelMuzzle(Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        root.rotation = Quaternion.Euler(0f, -21f, 0f);

        CreateCylinderX("barrel long blackened steel tube", new Vector3(0.1f, 0f, -0.08f), 1.9f, 0.115f, materials.BlackenedIron, root);
        CreateCylinderX("barrel inner dark bore line", new Vector3(-0.1f, 0f, -0.18f), 1.7f, 0.064f, materials.LineDark, root);
        CreateCylinderX("barrel top brass rail", new Vector3(0.05f, 0.17f, -0.12f), 1.58f, 0.025f, materials.DarkBrass, root);
        CreateCylinderX("barrel lower pressure return tube", new Vector3(0.12f, -0.19f, -0.13f), 1.55f, 0.038f, materials.DarkPipeMetal, root);
        metric.pipesOrManifolds += 4;

        float[] ringXs = { -0.82f, -0.62f, -0.42f, -0.05f, 0.43f, 0.8f };
        float[] ringR = { 0.23f, 0.19f, 0.17f, 0.15f, 0.165f, 0.19f };
        for (int i = 0; i < ringXs.Length; i++)
        {
            CreateCylinderX("barrel stepped muzzle/collar ring " + i.ToString(CultureInfo.InvariantCulture), new Vector3(ringXs[i], 0f, -0.08f), i < 3 ? 0.105f : 0.08f, ringR[i], i % 2 == 0 ? materials.AgedBrass : materials.DarkBrass, root);
            metric.bevelRingsOrCollars++;
        }

        CreateCylinderX("barrel soot-black muzzle mouth", new Vector3(-0.93f, 0f, -0.08f), 0.065f, 0.155f, materials.SootBlack, root);
        CreateCylinderX("barrel inner bore darkness", new Vector3(-0.97f, 0f, -0.08f), 0.04f, 0.105f, materials.LineDark, root);
        CreateCylinderY("barrel small front sight post", new Vector3(-0.42f, 0.29f, -0.13f), 0.12f, 0.025f, materials.BrassEdge, root);
        CreateBox("barrel front sight bead", new Vector3(-0.42f, 0.37f, -0.13f), new Vector3(0.07f, 0.025f, 0.035f), Quaternion.identity, materials.BrassEdge, root);
        metric.platesOrBrackets += 1;

        AddSlottedRivetRow("barrel rail screw", new Vector3(-0.63f, 0.205f, -0.17f), new Vector3(0.7f, 0.205f, -0.17f), 8, 0.018f, materials.BrassEdge, materials.LineDark, root, metric);
        AddSootPatches("muzzle soot halo", 7, new Vector3(-0.82f, -0.03f, -0.25f), 0.12f, 0.18f, root, materials, metric);
        AddOilRuns("barrel oil streak", 8, -0.5f, 0.48f, -0.19f, root, materials, metric);
        metric.materialRoles = 6;
        metric.notes = "New barrel/muzzle component: nested brass collars, dark blackened tube, visible bore darkness, lower pressure pipe, front sight, slotted rail screws, soot at muzzle, and oil streaks. It is component proof only, not the whole pistol barrel layout.";
    }

    private static void BuildValveManifold(Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        root.rotation = Quaternion.Euler(0f, -13f, 0f);

        CreateBox("manifold blackened steel block", new Vector3(0f, 0f, -0.05f), new Vector3(1.15f, 0.5f, 0.22f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("manifold brass face plate", new Vector3(0f, 0.01f, -0.19f), new Vector3(1.02f, 0.39f, 0.055f), Quaternion.identity, materials.DarkBrass, root);
        CreateBox("manifold oily gasket layer", new Vector3(0f, 0.01f, -0.225f), new Vector3(0.9f, 0.28f, 0.025f), Quaternion.identity, materials.OilBlack, root);
        metric.platesOrBrackets += 3;

        CreateCylinderX("manifold left copper inlet", new Vector3(-0.88f, 0.12f, -0.12f), 0.64f, 0.044f, materials.AgedCopper, root);
        CreateCylinderX("manifold right black outlet", new Vector3(0.88f, -0.1f, -0.12f), 0.64f, 0.044f, materials.DarkPipeMetal, root);
        CreateCylinderY("manifold top pressure neck", new Vector3(0.28f, 0.36f, -0.13f), 0.34f, 0.045f, materials.AgedBrass, root);
        CreateCylinderY("manifold top cap", new Vector3(0.28f, 0.58f, -0.13f), 0.07f, 0.09f, materials.DarkBrass, root);
        CreateCylinderBetween("manifold diagonal copper bypass", new Vector3(-0.44f, -0.28f, -0.13f), new Vector3(0.22f, -0.55f, -0.18f), 0.028f, materials.AgedCopper, root);
        metric.pipesOrManifolds += 5;

        BuildValveWheel("front brass valve wheel", new Vector3(-0.24f, 0.03f, -0.32f), 0.24f, materials.AgedBrass, materials.LineDark, root, metric);
        BuildValveWheel("small dark bleed wheel", new Vector3(0.48f, 0.07f, -0.31f), 0.16f, materials.DarkBrass, materials.LineDark, root, metric);

        AddSlottedRivetRow("manifold face screws top", new Vector3(-0.44f, 0.22f, -0.26f), new Vector3(0.44f, 0.22f, -0.26f), 5, 0.018f, materials.BrassEdge, materials.LineDark, root, metric);
        AddSlottedRivetRow("manifold face screws low", new Vector3(-0.44f, -0.2f, -0.26f), new Vector3(0.44f, -0.2f, -0.26f), 5, 0.018f, materials.BrassEdge, materials.LineDark, root, metric);
        AddOilRuns("manifold oil drip", 8, -0.42f, 0.5f, -0.28f, root, materials, metric);
        CreateSphere("manifold green patina in left seam", new Vector3(-0.55f, 0.2f, -0.265f), new Vector3(0.055f, 0.025f, 0.012f), materials.PatinaGreen, root);
        metric.materialRoles = 7;
        metric.notes = "New valve/manifold component: layered block, gasket, inlet/outlet pipes, bypass line, two valve wheels, caps, screws, oil drips, and patina. The functional pipe logic is readable, though real production would replace the segmented wheels with authored geometry.";
    }

    private static void BuildGripTriggerGuard(Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        root.rotation = Quaternion.Euler(0f, -12f, 0f);

        CreateBox("grip blackened receiver tang", new Vector3(-0.12f, 0.32f, -0.04f), new Vector3(1.08f, 0.18f, 0.16f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("grip brass upper tang plate", new Vector3(-0.12f, 0.39f, -0.135f), new Vector3(0.96f, 0.055f, 0.055f), Quaternion.identity, materials.DarkBrass, root);
        CreateBox("grip rear brass butt cap", new Vector3(0.38f, -0.42f, -0.08f), new Vector3(0.27f, 0.16f, 0.13f), Quaternion.Euler(0f, 0f, -12f), materials.AgedBrass, root);
        metric.platesOrBrackets += 3;

        CreateBox("walnut grip core angled", new Vector3(0.2f, -0.12f, -0.06f), new Vector3(0.42f, 0.82f, 0.18f), Quaternion.Euler(0f, 0f, -13f), materials.Walnut, root);
        CreateBox("dark leather palm wrap", new Vector3(0.2f, -0.13f, -0.185f), new Vector3(0.46f, 0.72f, 0.06f), Quaternion.Euler(0f, 0f, -13f), materials.DarkLeather, root);
        for (int i = 0; i < 7; i++)
        {
            float y = 0.19f - i * 0.1f;
            CreateBox("leather wrap crease " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0.12f + i * 0.012f, y, -0.228f), new Vector3(0.48f, 0.012f, 0.01f), Quaternion.Euler(0f, 0f, -10f), i % 2 == 0 ? materials.LeatherHighlight : materials.OilBlack, root);
            metric.surfaceWearMarks++;
        }

        BuildTriggerGuard(root, materials, metric);
        CreateBox("curved dark trigger blade", new Vector3(-0.23f, -0.05f, -0.18f), new Vector3(0.08f, 0.34f, 0.045f), Quaternion.Euler(0f, 0f, -18f), materials.LineDark, root);
        CreateSphere("trigger brass pivot screw", new Vector3(-0.25f, 0.14f, -0.215f), new Vector3(0.055f, 0.055f, 0.02f), materials.BrassEdge, root);
        metric.needles += 1;
        metric.fasteners++;

        AddSlottedRivetRow("grip tang screw row", new Vector3(-0.52f, 0.39f, -0.18f), new Vector3(0.26f, 0.39f, -0.18f), 5, 0.018f, materials.BrassEdge, materials.LineDark, root, metric);
        AddSlottedRivetRow("grip leather wrap rivets", new Vector3(-0.02f, 0.18f, -0.24f), new Vector3(0.42f, -0.4f, -0.24f), 6, 0.016f, materials.DarkBrass, materials.LineDark, root, metric);
        AddOilRuns("grip oil-dark crease", 5, -0.04f, 0.32f, -0.245f, root, materials, metric);
        metric.materialRoles = 7;
        metric.notes = "New grip/trigger component: walnut core, dark leather wrap, brass tang and butt cap, segmented trigger guard, trigger blade, rivets, leather creases, and oil-dark wear. It anchors the future first-person lower-right read, but the grip silhouette remains blockout-like.";
    }

    private static void BuildTriggerGuard(Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        Vector3[] points =
        {
            new Vector3(-0.48f, 0.22f, -0.19f),
            new Vector3(-0.64f, 0.02f, -0.2f),
            new Vector3(-0.58f, -0.25f, -0.2f),
            new Vector3(-0.28f, -0.39f, -0.2f),
            new Vector3(0.08f, -0.33f, -0.2f)
        };

        for (int i = 0; i < points.Length - 1; i++)
        {
            CreateCylinderBetween("segmented brass trigger guard " + i.ToString(CultureInfo.InvariantCulture), points[i], points[i + 1], 0.028f, materials.DarkBrass, root);
            metric.bevelRingsOrCollars++;
        }

        CreateCylinderY("trigger guard front mounting lug", points[0] + new Vector3(0f, 0.02f, 0f), 0.08f, 0.045f, materials.AgedBrass, root);
        CreateCylinderY("trigger guard rear mounting lug", points[points.Length - 1] + new Vector3(0f, 0.02f, 0f), 0.08f, 0.045f, materials.AgedBrass, root);
        metric.platesOrBrackets += 2;
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
        FillTexture(contact, new Color(0.017f, 0.014f, 0.011f, 1f));

        int margin = 70;
        int gap = 38;
        int columns = 3;
        int rows = 2;
        int cellWidth = (ContactWidth - margin * 2 - gap * (columns - 1)) / columns;
        int cellHeight = (ContactHeight - margin * 2 - gap * (rows - 1)) / rows;

        for (int i = 0; i < results.Count; i++)
        {
            Texture2D source = LoadRenderedPng(projectRoot, results[i].Metric.renderPath);
            int col = i % columns;
            int row = rows - 1 - (i / columns);
            int x = margin + col * (cellWidth + gap);
            int y = margin + row * (cellHeight + gap);
            RectInt cell = new RectInt(x, y, cellWidth, cellHeight);
            FillRect(contact, cell, new Color(0.028f, 0.023f, 0.019f, 1f));
            BlitScaled(source, contact, FitRect(source.width, source.height, cell));
            DrawRect(contact, cell, new Color(0.54f, 0.32f, 0.12f, 1f), 3);
            DestroyTexture(source);
        }

        DrawRect(contact, new RectInt(34, 34, ContactWidth - 68, ContactHeight - 68), new Color(0.34f, 0.2f, 0.09f, 1f), 2);
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
        builder.AppendLine("Status: Unity-only component refinement pass");
        builder.AppendLine("Date: " + metrics.timestamp);
        builder.AppendLine("Unity: " + metrics.unityVersion);
        builder.AppendLine("Entrypoint: `" + metrics.batchmodeEntrypoint + "`");
        builder.AppendLine();
        builder.AppendLine("## Run Result");
        builder.AppendLine();
        builder.AppendLine("Batch renderer completed and wrote six isolated component PNGs, an updated contact sheet, metrics JSON, and this report. The renderer uses Unity Editor/batch APIs plus procedural primitives/materials only; no Blender or external DCC is involved.");
        builder.AppendLine();
        builder.AppendLine("Command used:");
        builder.AppendLine();
        builder.AppendLine("```powershell");
        builder.AppendLine("& 'C:\\Program Files\\Unity\\Hub\\Editor\\6000.4.6f1\\Editor\\Unity.exe' -batchmode -quit -projectPath 'D:\\__MY APPS\\Unity Doom' -executeMethod PressurePistolLookDevRenderer.RenderBatch -logFile 'D:\\__MY APPS\\Unity Doom\\Documentation\\AssetProduction\\PressurePistolLookDev\\pressure_pistol_component_renderer_batch.log'");
        builder.AppendLine("```");
        builder.AppendLine();
        builder.AppendLine("## Outputs");
        builder.AppendLine();
        for (int i = 0; i < metrics.components.Length; i++)
        {
            builder.AppendLine("- " + metrics.components[i].title + ": `" + metrics.components[i].renderPath + "`");
        }
        builder.AppendLine("- Contact sheet: `" + metrics.contactSheetPath + "`");
        builder.AppendLine("- Metrics: `" + metrics.metricsPath + "`");
        builder.AppendLine("- Batch log: `Documentation/AssetProduction/PressurePistolLookDev/pressure_pistol_component_renderer_batch.log`");
        builder.AppendLine();
        builder.AppendLine("## Component Evidence");
        builder.AppendLine();
        builder.AppendLine("| Component | Detail evidence | Material/read evidence | Automated checks |");
        builder.AppendLine("| --- | --- | --- | --- |");
        for (int i = 0; i < metrics.components.Length; i++)
        {
            ComponentMetric component = metrics.components[i];
            string detail = component.coilTurns.ToString(CultureInfo.InvariantCulture) + " coils, " +
                            component.gaugeTickMarks.ToString(CultureInfo.InvariantCulture) + " ticks, " +
                            component.fasteners.ToString(CultureInfo.InvariantCulture) + " fasteners, " +
                            component.platesOrBrackets.ToString(CultureInfo.InvariantCulture) + " plates/brackets, " +
                            component.pipesOrManifolds.ToString(CultureInfo.InvariantCulture) + " pipes/manifolds, " +
                            component.bevelRingsOrCollars.ToString(CultureInfo.InvariantCulture) + " rings/collars, " +
                            component.surfaceWearMarks.ToString(CultureInfo.InvariantCulture) + " wear marks";
            string material = component.materialRoles.ToString(CultureInfo.InvariantCulture) + " visual roles, " +
                              component.usesFinalMaterialTextures.ToString(CultureInfo.InvariantCulture) + " FinalMaterialsV1 families, warm " +
                              component.warmMaterialPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + "%, dark " +
                              component.darkMaterialPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + "%";
            string checks = "nonblank " + PassFail(component.passesNotEmptyCheck) +
                            ", magenta " + PassFail(component.passesNoMagentaCheck) +
                            ", separation " + PassFail(component.passesMaterialSeparationCheck) +
                            ", framing " + PassFail(component.passesCameraFramingCheck);
            builder.AppendLine("| " + component.title + " | " + detail + " | " + material + " | " + checks + " |");
        }

        builder.AppendLine();
        builder.AppendLine("## Per-Component Acceptance Notes");
        builder.AppendLine();
        for (int i = 0; i < metrics.components.Length; i++)
        {
            ComponentMetric component = metrics.components[i];
            builder.AppendLine("### " + component.title);
            builder.AppendLine();
            builder.AppendLine(component.notes);
            builder.AppendLine();
            builder.AppendLine("- Framing occupancy: x `" + component.viewportMinX.ToString("0.000", CultureInfo.InvariantCulture) + "` to `" + component.viewportMaxX.ToString("0.000", CultureInfo.InvariantCulture) + "`, y `" + component.viewportMinY.ToString("0.000", CultureInfo.InvariantCulture) + "` to `" + component.viewportMaxY.ToString("0.000", CultureInfo.InvariantCulture) + "`.");
            builder.AppendLine("- Pixel checks: avg luminance `" + component.averageLuminance.ToString("0.000", CultureInfo.InvariantCulture) + "`, near-black `" + component.nearBlackPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + "%`, magenta samples `" + component.magentaPixelSamples.ToString(CultureInfo.InvariantCulture) + "`.");
            builder.AppendLine();
        }

        builder.AppendLine("## Candid North-Star Self-Critique");
        builder.AppendLine();
        builder.AppendLine("- Better: the pass is now component-first and broader: coil, gauge, boiler chamber, barrel/muzzle, valve/manifold, and leather grip all exist as independent Unity renders with denser rivets, soot/oil, collars, and fittings.");
        builder.AppendLine("- Better: the gauge camera is no longer as claustrophobic; side pipes, top cap, yoke, and full bezel now have review breathing room.");
        builder.AppendLine("- Better: the coil is darker and less toy-bright, with blackened steel backing, aged brass, patina, smaller heat, and more recessed grime.");
        builder.AppendLine("- Still weak: all geometry is primitive lookdev. The north-star art has molded bevel continuity, chipped edges, surface dents, and authored silhouettes that these blocks/cylinders only approximate.");
        builder.AppendLine("- Still weak: leather and walnut read as material roles, but the grip needs a real sculpted profile before it will match the concept's handmade Victorian feel.");
        builder.AppendLine("- Still weak: the material response is useful but not final PBR art; true production needs mesh UVs, trim/atlas choices, baked normal detail, and hand-placed grime masks.");
        builder.AppendLine("- Recommendation: keep this as a component acceptance/reference lane and do not promote it to gameplay viewmodel art. Next pass should refine one component at a time into authored Unity mesh assets or a procedural mesh builder inside the same scope.");
        builder.AppendLine();
        builder.AppendLine("## Scope Guard");
        builder.AppendLine();
        builder.AppendLine("This lane touched only the owned renderer and lookdev documentation/render outputs. It did not edit gameplay scripts, audio files, build scripts, status docs, version docs, or the active weapon definition.");
        return builder.ToString();
    }

    private static string PassFail(bool value)
    {
        return value ? "pass" : "fail";
    }

    private static void AnalyzeImage(Texture2D image, ComponentMetric metric)
    {
        long sampleCount = 0;
        double luminance = 0.0;
        int nearBlack = 0;
        int magenta = 0;
        int dark = 0;
        int warmMetal = 0;
        int hotCopper = 0;
        int cream = 0;
        int red = 0;
        int leatherWood = 0;
        int patina = 0;

        for (int y = 0; y < image.height; y += 4)
        {
            for (int x = 0; x < image.width; x += 4)
            {
                Color color = image.GetPixel(x, y);
                float r = color.r;
                float g = color.g;
                float b = color.b;
                float luma = r * 0.2126f + g * 0.7152f + b * 0.0722f;
                luminance += luma;
                if (luma < 0.035f)
                {
                    nearBlack++;
                }

                if (r > 0.94f && g < 0.08f && b > 0.94f)
                {
                    magenta++;
                }

                if (luma >= 0.035f && luma < 0.18f)
                {
                    dark++;
                }

                if (r > 0.31f && g > 0.17f && b < 0.28f && r > g && g > b)
                {
                    warmMetal++;
                }

                if (r > 0.46f && g > 0.1f && g < 0.52f && b < 0.26f)
                {
                    hotCopper++;
                }

                if (r > 0.52f && g > 0.44f && b > 0.31f && r - b < 0.42f)
                {
                    cream++;
                }

                if (r > 0.42f && g < 0.26f && b < 0.26f)
                {
                    red++;
                }

                if (r > 0.16f && r < 0.48f && g > 0.08f && g < 0.32f && b < 0.18f && r > g)
                {
                    leatherWood++;
                }

                if (g > 0.22f && g > r * 1.25f && b > 0.14f && r < 0.32f)
                {
                    patina++;
                }

                sampleCount++;
            }
        }

        metric.averageLuminance = sampleCount > 0 ? (float)(luminance / sampleCount) : 0f;
        metric.nearBlackPixelPercent = sampleCount > 0 ? nearBlack * 100f / sampleCount : 0f;
        metric.magentaPixelSamples = magenta;
        metric.darkMaterialPixelPercent = sampleCount > 0 ? dark * 100f / sampleCount : 0f;
        metric.warmMaterialPixelPercent = sampleCount > 0 ? warmMetal * 100f / sampleCount : 0f;
        metric.hotCopperPixelPercent = sampleCount > 0 ? hotCopper * 100f / sampleCount : 0f;
        metric.creamPixelPercent = sampleCount > 0 ? cream * 100f / sampleCount : 0f;
        metric.redPixelPercent = sampleCount > 0 ? red * 100f / sampleCount : 0f;
        metric.leatherWoodPixelPercent = sampleCount > 0 ? leatherWood * 100f / sampleCount : 0f;
        metric.patinaPixelPercent = sampleCount > 0 ? patina * 100f / sampleCount : 0f;
        metric.passesNotEmptyCheck = metric.averageLuminance > 0.025f && metric.nearBlackPixelPercent < 92f;
        metric.passesNoMagentaCheck = metric.magentaPixelSamples == 0;
        metric.passesMaterialSeparationCheck = metric.darkMaterialPixelPercent > 4f &&
                                               (metric.warmMaterialPixelPercent + metric.hotCopperPixelPercent + metric.creamPixelPercent + metric.redPixelPercent + metric.leatherWoodPixelPercent + metric.patinaPixelPercent) > 3.5f;
    }

    private static void AnalyzeFraming(Camera camera, Transform componentRoot, ComponentMetric metric)
    {
        Renderer[] renderers = componentRoot.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            metric.passesCameraFramingCheck = false;
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        Vector3[] corners =
        {
            new Vector3(min.x, min.y, min.z),
            new Vector3(min.x, min.y, max.z),
            new Vector3(min.x, max.y, min.z),
            new Vector3(min.x, max.y, max.z),
            new Vector3(max.x, min.y, min.z),
            new Vector3(max.x, min.y, max.z),
            new Vector3(max.x, max.y, min.z),
            new Vector3(max.x, max.y, max.z)
        };

        float minX = 1f;
        float minY = 1f;
        float maxX = 0f;
        float maxY = 0f;
        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 viewport = camera.WorldToViewportPoint(corners[i]);
            minX = Mathf.Min(minX, viewport.x);
            minY = Mathf.Min(minY, viewport.y);
            maxX = Mathf.Max(maxX, viewport.x);
            maxY = Mathf.Max(maxY, viewport.y);
        }

        metric.viewportMinX = minX;
        metric.viewportMinY = minY;
        metric.viewportMaxX = maxX;
        metric.viewportMaxY = maxY;
        metric.viewportWidthPercent = Mathf.Max(0f, maxX - minX) * 100f;
        metric.viewportHeightPercent = Mathf.Max(0f, maxY - minY) * 100f;
        metric.passesCameraFramingCheck = minX > 0.03f && minY > 0.04f && maxX < 0.97f && maxY < 0.96f;
    }

    private static void CreateCopperCoilLoop(string name, Vector3 center, float radiusY, float radiusZ, float tubeRadius, Material material, Transform parent)
    {
        int segments = 22;
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
            Vector3 pointA = new Vector3(Mathf.Cos(angleA) * radius, Mathf.Sin(angleA) * radius, -0.245f);
            Vector3 pointB = new Vector3(Mathf.Cos(angleB) * radius, Mathf.Sin(angleB) * radius, -0.245f);
            CreateCylinderBetween(prefix + " segment " + i.ToString(CultureInfo.InvariantCulture), pointA, pointB, tubeRadius, material, parent);
        }
    }

    private static void BuildValveWheel(string prefix, Vector3 center, float radius, Material rimMaterial, Material darkMaterial, Transform parent, ComponentMetric metric)
    {
        int segments = 14;
        for (int i = 0; i < segments; i++)
        {
            float a = i * Mathf.PI * 2f / segments;
            float b = (i + 1) * Mathf.PI * 2f / segments;
            Vector3 start = center + new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, 0f);
            Vector3 end = center + new Vector3(Mathf.Cos(b) * radius, Mathf.Sin(b) * radius, 0f);
            CreateCylinderBetween(prefix + " rim segment " + i.ToString(CultureInfo.InvariantCulture), start, end, 0.014f, rimMaterial, parent);
        }

        for (int i = 0; i < 4; i++)
        {
            float angle = (45f + i * 90f) * Mathf.Deg2Rad;
            Vector3 end = center + new Vector3(Mathf.Cos(angle) * (radius * 0.82f), Mathf.Sin(angle) * (radius * 0.82f), 0f);
            CreateCylinderBetween(prefix + " spoke " + i.ToString(CultureInfo.InvariantCulture), center, end, 0.014f, rimMaterial, parent);
        }

        CreateCylinderZ(prefix + " dark center hub", center + new Vector3(0f, 0f, -0.01f), 0.045f, radius * 0.24f, darkMaterial, parent);
        CreateSphere(prefix + " brass center cap", center + new Vector3(0f, 0f, -0.04f), Vector3.one * (radius * 0.22f), rimMaterial, parent);
        metric.bevelRingsOrCollars += 1;
        metric.platesOrBrackets += 1;
    }

    private static void AddSootPatches(string prefix, int count, Vector3 center, float spreadX, float spreadY, Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            float x = center.x + Mathf.Sin(i * 1.9f) * spreadX;
            float y = center.y + (t - 0.5f) * spreadY;
            CreateSphere(prefix + " " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, y, center.z), new Vector3(0.09f + 0.025f * (i % 3), 0.035f, 0.012f), i % 2 == 0 ? materials.SootBlack : materials.OilBlack, root);
            metric.surfaceWearMarks++;
        }
    }

    private static void AddOilRuns(string prefix, int count, float startX, float endX, float z, Transform root, LookDevMaterials materials, ComponentMetric metric)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            float x = Mathf.Lerp(startX, endX, t);
            float y = 0.22f - Mathf.Abs(Mathf.Sin(i * 1.3f)) * 0.42f;
            CreateBox(prefix + " " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, y, z), new Vector3(0.01f, 0.08f + (i % 3) * 0.025f, 0.009f), Quaternion.Euler(0f, 0f, -4f + i % 4), materials.OilBlack, root);
            metric.surfaceWearMarks++;
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

    private static void AddSlottedRivetRow(string prefix, Vector3 start, Vector3 end, int count, float radius, Material material, Material slotMaterial, Transform parent, ComponentMetric metric)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            Vector3 position = Vector3.Lerp(start, end, t);
            CreateSphere(prefix + " head " + i.ToString(CultureInfo.InvariantCulture), position, Vector3.one * (radius * 2f), material, parent);
            CreateBox(prefix + " slot " + i.ToString(CultureInfo.InvariantCulture), position + new Vector3(0f, 0f, -radius * 0.62f), new Vector3(radius * 1.45f, radius * 0.22f, radius * 0.25f), Quaternion.Euler(0f, 0f, i * 17f), slotMaterial, parent);
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
        public Material OilBlack;
        public Material PatinaGreen;
        public Material Walnut;
        public Material DarkLeather;
        public Material LeatherHighlight;
        public Material ContactBack;
        public Material WetHighlight;
        public int LoadedTextureFamilies;

        public static LookDevMaterials Create(string projectRoot)
        {
            LookDevMaterials materials = new LookDevMaterials();
            materials.BlackenedIron = materials.CreateFinalMaterial(projectRoot, "BlackenedRivetedIron", "PPCOMP_BlackenedRivetedIron", 0.86f, 0.3f, new Color(0.72f, 0.7f, 0.67f, 1f), new Vector2(1.5f, 1.5f));
            materials.AgedBrass = materials.CreateFinalMaterial(projectRoot, "AgedBrass", "PPCOMP_AgedBrass", 0.9f, 0.54f, new Color(0.86f, 0.78f, 0.65f, 1f), new Vector2(1.3f, 1.3f));
            materials.AgedCopper = materials.CreateFinalMaterial(projectRoot, "CopperPipe", "PPCOMP_CopperPipe", 0.88f, 0.48f, new Color(0.8f, 0.68f, 0.58f, 1f), new Vector2(1.7f, 1f));
            materials.GaugeFace = materials.CreateFinalMaterial(projectRoot, "CreamEnamelGauge", "PPCOMP_CreamGauge", 0f, 0.42f, Color.white, Vector2.one);
            materials.DarkPipeMetal = materials.CreateFinalMaterial(projectRoot, "BlackenedRivetedIron", "PPCOMP_DarkPipeMetal", 0.88f, 0.24f, new Color(0.48f, 0.46f, 0.43f, 1f), new Vector2(1.9f, 1.9f));
            materials.Walnut = materials.CreateFinalMaterial(projectRoot, "GreasyWalnut", "PPCOMP_GreasyWalnut", 0f, 0.34f, new Color(0.78f, 0.68f, 0.56f, 1f), new Vector2(0.8f, 1.7f));
            materials.DarkLeather = materials.CreateFinalMaterial(projectRoot, "LeatherBellows", "PPCOMP_DarkLeather", 0f, 0.31f, new Color(0.42f, 0.28f, 0.2f, 1f), new Vector2(1f, 1.4f));

            materials.DarkBrass = materials.CreateColorMaterial("PPCOMP_DarkAgedBrass", new Color(0.31f, 0.18f, 0.07f, 1f), 0.88f, 0.38f, false);
            materials.BrassEdge = materials.CreateColorMaterial("PPCOMP_PolishedBrassEdge", new Color(0.88f, 0.56f, 0.21f, 1f), 0.9f, 0.68f, false);
            materials.HotCopper = materials.CreateColorMaterial("PPCOMP_HotCopper", new Color(0.62f, 0.18f, 0.08f, 1f), 0.82f, 0.5f, true);
            materials.CopperHighlight = materials.CreateColorMaterial("PPCOMP_CopperHighlight", new Color(0.84f, 0.34f, 0.12f, 1f), 0.78f, 0.62f, true);
            materials.Glass = materials.CreateTransparentMaterial("PPCOMP_AmberGaugeGlass", new Color(0.92f, 0.68f, 0.34f, 0.24f), 0f, 0.95f);
            materials.GlassHighlight = materials.CreateTransparentMaterial("PPCOMP_GlassHighlight", new Color(1f, 0.82f, 0.54f, 0.54f), 0f, 0.98f);
            materials.LineDark = materials.CreateUnlitMaterial("PPCOMP_DarkInk", new Color(0.028f, 0.021f, 0.015f, 1f), false);
            materials.WarningRed = materials.CreateColorMaterial("PPCOMP_RedNeedle", new Color(0.72f, 0.05f, 0.025f, 1f), 0.02f, 0.45f, false);
            materials.GlowAmber = materials.CreateUnlitMaterial("PPCOMP_PressureCoreGlow", new Color(0.75f, 0.2f, 0.035f, 1f), true);
            materials.HeatOrange = materials.CreateUnlitMaterial("PPCOMP_HeatOrange", new Color(0.95f, 0.32f, 0.04f, 1f), true);
            materials.HeatRed = materials.CreateUnlitMaterial("PPCOMP_DullHeatRed", new Color(0.36f, 0.045f, 0.02f, 1f), true);
            materials.SootBlack = materials.CreateUnlitMaterial("PPCOMP_SootBlack", new Color(0.01f, 0.008f, 0.006f, 1f), false);
            materials.OilBlack = materials.CreateTransparentMaterial("PPCOMP_OilBlack", new Color(0.015f, 0.011f, 0.007f, 0.82f), 0f, 0.86f);
            materials.PatinaGreen = materials.CreateColorMaterial("PPCOMP_PatinaGreen", new Color(0.055f, 0.24f, 0.18f, 1f), 0.04f, 0.42f, false);
            materials.LeatherHighlight = materials.CreateColorMaterial("PPCOMP_LeatherHighlight", new Color(0.48f, 0.27f, 0.14f, 1f), 0f, 0.42f, false);
            materials.ContactBack = materials.CreateUnlitMaterial("PPCOMP_ContactBack", new Color(0.021f, 0.017f, 0.014f, 1f), false);
            materials.WetHighlight = materials.CreateTransparentMaterial("PPCOMP_WetWarmReflection", new Color(0.85f, 0.45f, 0.12f, 0.18f), 0f, 0.93f);
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
                SetMaterialColor(material, "_EmissionColor", color * 1.45f);
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
                SetMaterialColor(material, "_EmissionColor", color * 1.55f);
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
        public int magentaPixelSamples;
        public float darkMaterialPixelPercent;
        public float warmMaterialPixelPercent;
        public float hotCopperPixelPercent;
        public float creamPixelPercent;
        public float redPixelPercent;
        public float leatherWoodPixelPercent;
        public float patinaPixelPercent;
        public bool passesNotEmptyCheck;
        public bool passesNoMagentaCheck;
        public bool passesMaterialSeparationCheck;
        public bool passesCameraFramingCheck;
        public float viewportMinX;
        public float viewportMinY;
        public float viewportMaxX;
        public float viewportMaxY;
        public float viewportWidthPercent;
        public float viewportHeightPercent;
    }

    private sealed class ComponentRenderResult
    {
        public string Key;
        public string Title;
        public Texture2D Texture;
        public ComponentMetric Metric;
    }
}
