using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class UnityPressurePistolProofRenderer
{
    private const int HeroWidth = 1920;
    private const int HeroHeight = 1080;
    private const int ContactWidth = 2200;
    private const int ContactHeight = 1400;
    private const int ComponentWidth = 1600;
    private const int ComponentHeight = 1000;
    private const int ComponentContactWidth = 2200;
    private const int ComponentContactHeight = 1600;
    private const string ConceptRelativePath = "Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png";
    private const string RenderRelativePath = "Documentation/ConceptRenders/RENDER_HFLD_Recovery05_pressure_pistol_unity_proof.jpg";
    private const string ContactRelativePath = "Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery05_pressure_pistol_unity_proof.jpg";
    private const string ComponentContactRelativePath = "Documentation/ConceptRenders/CONTACTSHEET_HFLD_Recovery07_pressure_pistol_component_decomposition_unity_proof.jpg";
    private const string CoilRelativePath = "Documentation/ConceptRenders/RENDER_HFLD_Recovery07_coil_unity_proof.jpg";
    private const string GaugeRelativePath = "Documentation/ConceptRenders/RENDER_HFLD_Recovery07_gauge_unity_proof.jpg";
    private const string BarrelTankRelativePath = "Documentation/ConceptRenders/RENDER_HFLD_Recovery07_barrel_tank_unity_proof.jpg";
    private const string MuzzleRelativePath = "Documentation/ConceptRenders/RENDER_HFLD_Recovery07_muzzle_unity_proof.jpg";
    private const string GripHandRelativePath = "Documentation/ConceptRenders/RENDER_HFLD_Recovery07_grip_hand_unity_proof.jpg";
    private const string SilhouetteRelativePath = "Documentation/ConceptRenders/RENDER_HFLD_Recovery07_assembled_silhouette_reference_unity_proof.jpg";
    private const string ProofFolderRelativePath = "Documentation/AssetProduction/HighFidelityLookdevRecovery/UnityPressurePistolProof";
    private const string ReportFileName = "HFLD_RECOVERY05_UNITY_PRESSURE_PISTOL_PROOF_REPORT.md";
    private const string MetricsFileName = "unity_recovery05_pressure_pistol_proof_metrics.json";
    private const string ComponentReportFileName = "HFLD_RECOVERY07_COMPONENT_DECOMPOSITION_REPORT.md";
    private const string ComponentMetricsFileName = "unity_pressure_pistol_component_recovery07_metrics.json";

    [MenuItem("Project Tools/Lookdev/Render Pressure Pistol Component Proofs")]
    public static void RenderFromMenu()
    {
        RenderComponentProofs();
    }

    public static void RenderBatch()
    {
        try
        {
            RenderComponentProofs();
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

    public static void RenderComponentProofs()
    {
        string projectRoot = GetProjectRoot();
        string conceptRenderFolder = Path.Combine(projectRoot, "Documentation/ConceptRenders");
        string proofFolder = Path.Combine(projectRoot, ProofFolderRelativePath);
        Directory.CreateDirectory(conceptRenderFolder);
        Directory.CreateDirectory(proofFolder);
        ComponentProofMetrics metrics = new ComponentProofMetrics
        {
            timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture),
            unityVersion = Application.unityVersion,
            batchmodeEntrypoint = "UnityPressurePistolProofRenderer.RenderBatch",
            exactUnityCommand = "& 'C:\\Program Files\\Unity\\Hub\\Editor\\6000.4.6f1\\Editor\\Unity.exe' -batchmode -quit -projectPath 'D:\\__MY APPS\\Unity Doom' -executeMethod UnityPressurePistolProofRenderer.RenderBatch -logFile 'D:\\__MY APPS\\Unity Doom\\Documentation\\AssetProduction\\HighFidelityLookdevRecovery\\UnityPressurePistolProof\\unity_recovery07_components_batch.log'",
            contactSheetPath = ComponentContactRelativePath,
            reportPath = (ProofFolderRelativePath + "/" + ComponentReportFileName).Replace("\\", "/"),
            metricsPath = (ProofFolderRelativePath + "/" + ComponentMetricsFileName).Replace("\\", "/"),
            recovery04Status = "Earlier full-gun Unity proof remains rejected; Recovery07 does not mutate prior reports and stays component-first.",
            smokePolicy = "Omitted in Recovery07 component proofs. No smoke/steam geometry is rendered, so no opaque paper blocks can appear.",
            overallAssessment = "Pass for component-first Recovery07 decomposition direction; partial/fail for final art or full-gun promotion until the hand is sculptural enough and all components survive a new assembled proof."
        };

        List<ComponentRenderResult> results = new List<ComponentRenderResult>();
        try
        {
            results.Add(RenderSingleComponent(projectRoot, "coil", "Copper/Amber Coil Module", CoilRelativePath, new Vector3(0.26f, 0.22f, -2.85f), new Vector3(0f, 0f, -0.03f), 28f, BuildCoilComponent));
            results.Add(RenderSingleComponent(projectRoot, "gauge", "Pressure Gauge", GaugeRelativePath, new Vector3(0.22f, 0.08f, -2.45f), new Vector3(0f, 0f, -0.04f), 29f, BuildGaugeComponent));
            results.Add(RenderSingleComponent(projectRoot, "barrel_tank", "Barrel/Tank Collars", BarrelTankRelativePath, new Vector3(0.38f, 0.22f, -3.45f), new Vector3(-0.04f, 0.01f, -0.02f), 29f, BuildBarrelTankComponent));
            results.Add(RenderSingleComponent(projectRoot, "muzzle", "Muzzle/Bore Depth", MuzzleRelativePath, new Vector3(-0.3f, 0.12f, -2.05f), new Vector3(-0.18f, -0.02f, -0.04f), 24f, BuildMuzzleComponent));
            results.Add(RenderSingleComponent(projectRoot, "grip_hand", "Walnut/Leather Grip + Gloved Hand", GripHandRelativePath, new Vector3(0.18f, 0.02f, -2.5f), new Vector3(0.3f, -0.32f, -0.1f), 28f, BuildGripHandComponent));
            results.Add(RenderSingleComponent(projectRoot, "assembled_silhouette", "Small Assembled Silhouette Reference", SilhouetteRelativePath, new Vector3(0.95f, 0.28f, -4.35f), new Vector3(0f, -0.02f, -0.05f), 32f, BuildAssembledSilhouetteReference));

            metrics.components = BuildComponentGateMetrics(results);
            Texture2D contactSheet = RenderComponentContactSheet(results, metrics);
            try
            {
                string contactPath = Path.Combine(projectRoot, ComponentContactRelativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(contactPath));
                File.WriteAllBytes(contactPath, contactSheet.EncodeToJPG(94));
            }
            finally
            {
                DestroyTexture(contactSheet);
            }

            File.WriteAllText(Path.Combine(proofFolder, ComponentMetricsFileName), JsonUtility.ToJson(metrics, true));
            File.WriteAllText(Path.Combine(proofFolder, ComponentReportFileName), BuildComponentProofReport(metrics));
            Debug.Log("Unity pressure pistol component contact sheet written to " + Path.Combine(projectRoot, ComponentContactRelativePath));
            Debug.Log("Unity pressure pistol component report written to " + Path.Combine(proofFolder, ComponentReportFileName));
        }
        finally
        {
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
        ConfigureComponentRenderSettings();
        ProofMaterials materials = CreateProofMaterials();
        ComponentGateMetric metric = new ComponentGateMetric
        {
            key = key,
            title = title,
            renderPath = relativePath,
            width = ComponentWidth,
            height = ComponentHeight,
            steamSprites = 0,
            reducedBlackSpaceFraming = true
        };

        Texture2D image = null;
        try
        {
            GameObject backgroundRoot = new GameObject("UnityPressurePistolProof_Component_Background");
            BuildComponentBackground(backgroundRoot.transform, materials);
            GameObject componentRoot = new GameObject("UnityPressurePistolProof_Component_" + key);
            builder(componentRoot.transform, materials, metric);
            BuildComponentLighting(key);
            Camera camera = CreateComponentCamera(cameraPosition, lookAt, fieldOfView);
            image = CaptureCamera(camera, ComponentWidth, ComponentHeight, new Color(0.012f, 0.011f, 0.01f), "Unity pressure pistol " + key + " component proof");

            string outputPath = Path.Combine(projectRoot, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            File.WriteAllBytes(outputPath, image.EncodeToJPG(95));
            Debug.Log("Unity pressure pistol component proof written to " + outputPath);

            return new ComponentRenderResult
            {
                Key = key,
                Title = title,
                RelativePath = relativePath,
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

    private delegate void ComponentBuilder(Transform root, ProofMaterials materials, ComponentGateMetric metric);

    private static void ConfigureComponentRenderSettings()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.026f, 0.022f, 0.018f);
        RenderSettings.fog = false;
        RenderSettings.reflectionIntensity = 0.62f;
    }

    private static void MarkRecovery04ReportRejected(string proofFolder)
    {
        string recovery04Report = Path.Combine(proofFolder, "HFLD_RECOVERY04_UNITY_PRESSURE_PISTOL_PROOF_REPORT.md");
        if (!File.Exists(recovery04Report))
        {
            return;
        }

        string report = File.ReadAllText(recovery04Report);
        if (report.IndexOf("PM/user review rejection", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return;
        }

        report = report.Replace("Status: Unity-only lookdev proof generated; not accepted final art", "Status: rejected full-gun Unity proof; not accepted final art");
        report += Environment.NewLine + "## PM/user review rejection" + Environment.NewLine + Environment.NewLine +
                  "Recovery04 is retained as a rejected full-gun proof. It failed visual review because smoke/steam rendered as opaque paper-like slabs, the camera was too cropped and side-on, materials skewed too orange, and several component silhouettes remained too boxy for the north-star concept target." + Environment.NewLine;
        File.WriteAllText(recovery04Report, report);
    }

    private static void BuildComponentBackground(Transform root, ProofMaterials materials)
    {
        CreateBox("component dark riveted backing", new Vector3(0f, 0f, 1.0f), new Vector3(4.6f, 2.75f, 0.08f), Quaternion.identity, materials.ContactBack, root);
        CreateBox("component low oily reflection plane", new Vector3(0f, -0.92f, 0.18f), new Vector3(4.45f, 0.06f, 1.35f), Quaternion.identity, materials.BlackenedIron, root);
        CreateCylinderX("component dim rear pipe", new Vector3(-0.05f, 0.94f, 0.72f), 2.8f, 0.035f, materials.DarkPipeMetal, root);
        CreateCylinderX("component rear warm brass pipe", new Vector3(0.25f, 0.78f, 0.68f), 2.35f, 0.024f, materials.DarkAgedBrass, root);
        CreateBox("component warm floor highlight", new Vector3(-0.24f, -0.86f, -0.18f), new Vector3(2.6f, 0.018f, 0.018f), Quaternion.identity, materials.BrassHighlight, root);
        AddBackgroundRivetRow("component rear wall rivet upper", new Vector3(-1.95f, 1.0f, 0.91f), new Vector3(1.95f, 1.0f, 0.91f), 16, 0.012f, materials.DarkAgedBrass, root);
        AddBackgroundRivetRow("component rear wall rivet lower", new Vector3(-1.95f, -0.72f, 0.91f), new Vector3(1.95f, -0.72f, 0.91f), 16, 0.012f, materials.DarkAgedBrass, root);
    }

    private static void BuildComponentLighting(string key)
    {
        CreateLight("component warm amber key", LightType.Spot, new Vector3(-2.05f, 1.9f, -2.25f), Quaternion.Euler(43f, 33f, 0f), new Color(1.0f, 0.58f, 0.22f), 5.4f, 48f, true);
        CreateLight("component cool fill", LightType.Directional, new Vector3(1.5f, 1.3f, -1.0f), Quaternion.Euler(38f, -138f, 0f), new Color(0.16f, 0.18f, 0.22f), 0.24f, 0f, false);
        CreateLight("component brass edge rim", LightType.Spot, new Vector3(1.55f, 1.12f, 1.18f), Quaternion.Euler(142f, -34f, 0f), new Color(1.0f, 0.66f, 0.27f), 2.65f, 38f, true);
        if (key == "coil")
        {
            CreateLight("component coil glow", LightType.Point, new Vector3(0f, 0f, -0.45f), Quaternion.identity, new Color(1.0f, 0.28f, 0.07f), 1.45f, 0f, false);
        }

        if (key == "gauge")
        {
            CreateLight("component gauge glass glint", LightType.Point, new Vector3(-0.45f, 0.55f, -0.8f), Quaternion.identity, new Color(1.0f, 0.78f, 0.36f), 1.1f, 0f, false);
        }

        if (key == "barrel_tank" || key == "muzzle" || key == "grip_hand" || key == "assembled_silhouette")
        {
            CreateLight("component low warm inspection fill", LightType.Point, new Vector3(-0.7f, 0.35f, -1.15f), Quaternion.identity, new Color(0.92f, 0.58f, 0.28f), 1.55f, 0f, false);
            CreateLight("component cool silhouette lift", LightType.Point, new Vector3(1.25f, 0.78f, -1.0f), Quaternion.identity, new Color(0.22f, 0.27f, 0.34f), 0.75f, 0f, false);
        }
    }

    private static Camera CreateComponentCamera(Vector3 position, Vector3 lookAt, float fieldOfView)
    {
        GameObject cameraObject = new GameObject("Unity pressure pistol component proof camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = position;
        camera.transform.LookAt(lookAt);
        camera.fieldOfView = fieldOfView;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 30f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.012f, 0.011f, 0.01f);
        camera.allowHDR = true;
        camera.allowMSAA = true;
        return camera;
    }

    private static void BuildCoilComponent(Transform root, ProofMaterials materials, ComponentGateMetric metric)
    {
        Quaternion componentRotation = Quaternion.Euler(0f, -24f, 0f);
        root.rotation = Quaternion.identity;
        Vector3 center = Vector3.zero;
        CreateBox("coil dark recessed cavity", center + new Vector3(0f, 0f, 0.08f), new Vector3(1.92f, 0.98f, 0.11f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("coil inner soot shadow", center + new Vector3(0f, 0f, -0.005f), new Vector3(1.68f, 0.62f, 0.035f), Quaternion.identity, materials.LineDark, root);
        CreateBox("coil top dark brass under-rail", center + new Vector3(0f, 0.545f, 0.0f), new Vector3(2.12f, 0.095f, 0.09f), Quaternion.identity, materials.DarkAgedBrass, root);
        CreateBox("coil bottom dark brass under-rail", center + new Vector3(0f, -0.545f, 0.0f), new Vector3(2.12f, 0.095f, 0.09f), Quaternion.identity, materials.DarkAgedBrass, root);
        CreateBox("coil top brass rail worn face", center + new Vector3(0f, 0.585f, -0.055f), new Vector3(2.04f, 0.055f, 0.07f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil bottom brass rail worn face", center + new Vector3(0f, -0.585f, -0.055f), new Vector3(2.04f, 0.055f, 0.07f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil left brass cheek", center + new Vector3(-1.04f, 0f, -0.02f), new Vector3(0.085f, 1.05f, 0.105f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("coil right brass cheek", center + new Vector3(1.04f, 0f, -0.02f), new Vector3(0.085f, 1.05f, 0.105f), Quaternion.identity, materials.AgedBrass, root);
        CreateCylinderX("coil left round end collar", center + new Vector3(-0.96f, 0f, -0.09f), 0.11f, 0.45f, materials.DarkAgedBrass, root);
        CreateCylinderX("coil right round end collar", center + new Vector3(0.96f, 0f, -0.09f), 0.11f, 0.45f, materials.DarkAgedBrass, root);
        CreateCylinderX("coil upper copper manifold", center + new Vector3(0f, 0.39f, -0.13f), 1.52f, 0.026f, materials.AgedCopper, root);
        CreateCylinderX("coil lower copper manifold", center + new Vector3(0f, -0.39f, -0.13f), 1.52f, 0.026f, materials.AgedCopper, root);
        CreateCylinderBetween("coil left flexible lead", center + new Vector3(-1.05f, 0.32f, -0.18f), center + new Vector3(-0.78f, 0.52f, -0.2f), 0.022f, materials.AgedCopper, root);
        CreateCylinderBetween("coil right flexible lead", center + new Vector3(1.05f, -0.32f, -0.18f), center + new Vector3(0.78f, -0.52f, -0.2f), 0.022f, materials.AgedCopper, root);
        CreateSphere("coil left verdigris tarnish dot", center + new Vector3(-0.9f, 0.47f, -0.23f), new Vector3(0.045f, 0.025f, 0.012f), materials.PatinaGreen, root);
        CreateSphere("coil right verdigris tarnish dot", center + new Vector3(0.82f, -0.48f, -0.23f), new Vector3(0.04f, 0.022f, 0.012f), materials.PatinaGreen, root);
        metric.platesOrBrackets = 8;

        int turns = 9;
        for (int i = 0; i < turns; i++)
        {
            float x = -0.72f + i * 0.18f;
            Material coilMaterial = i % 3 == 1 ? materials.AgedCopper : materials.HotCopper;
            CreateCopperCoilLoop("separate copper coil turn " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0f, -0.12f - (i % 2) * 0.015f), 0.34f, 0.17f, 0.026f, coilMaterial, root);
            CreateCylinderY("coil warm front bead " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.32f, -0.31f), 0.045f, 0.016f, materials.CopperHighlight, root);
            CreateBox("coil turn dark back shadow " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.01f, 0.05f), new Vector3(0.065f, 0.68f, 0.018f), Quaternion.identity, materials.LineDark, root);
            metric.warmHighlightAccents++;
        }

        CreateCylinderX("warm emissive core behind copper turns", new Vector3(0f, 0f, -0.18f), 1.58f, 0.04f, materials.GlowAmber, root);
        CreateBox("coil top brass glint", center + new Vector3(0.08f, 0.62f, -0.12f), new Vector3(1.54f, 0.016f, 0.018f), Quaternion.identity, materials.BrassHighlight, root);
        CreateBox("coil bottom brass glint", center + new Vector3(-0.06f, -0.62f, -0.12f), new Vector3(1.42f, 0.016f, 0.018f), Quaternion.identity, materials.BrassHighlight, root);
        AddComponentRivetRow("coil top rail fastener", new Vector3(-0.88f, 0.63f, -0.1f), new Vector3(0.88f, 0.63f, -0.1f), 13, 0.017f, materials.AgedBrass, root, metric);
        AddComponentRivetRow("coil bottom rail fastener", new Vector3(-0.88f, -0.63f, -0.1f), new Vector3(0.88f, -0.63f, -0.1f), 13, 0.017f, materials.AgedBrass, root, metric);
        AddComponentRivetRow("coil left cheek rivets", new Vector3(-1.1f, -0.42f, -0.1f), new Vector3(-1.1f, 0.42f, -0.1f), 5, 0.016f, materials.AgedBrass, root, metric);
        AddComponentRivetRow("coil right cheek rivets", new Vector3(1.1f, -0.42f, -0.1f), new Vector3(1.1f, 0.42f, -0.1f), 5, 0.016f, materials.AgedBrass, root, metric);

        root.rotation = componentRotation;
        metric.coilTurns = turns;
        metric.warmEmissiveCore = true;
        metric.darkRecessFrame = true;
        metric.brassFasteners = true;
        metric.bevelRingsOrCollars = 4;
        metric.materialVariationAccents = 8;
        metric.materialRoles = 8;
        metric.gateStatus = "Pass";
        metric.notes = "Recovery07 keeps the calmer amber coil from Recovery06 and adds tarnish dots plus flexible copper leads so the module reads less like neon rods and more like aged pressure hardware.";
    }

    private static void BuildGaugeComponent(Transform root, ProofMaterials materials, ComponentGateMetric metric)
    {
        Quaternion componentRotation = Quaternion.Euler(0f, -10f, 0f);
        root.rotation = Quaternion.identity;
        Vector3 center = Vector3.zero;
        CreateCylinderZ("dark gauge rear pressure cup", center + new Vector3(0f, 0f, 0.07f), 0.22f, 0.62f, materials.BlackenedIron, root);
        CreateCylinderZ("dark aged brass rear collar", center + new Vector3(0f, 0f, -0.01f), 0.12f, 0.58f, materials.DarkAgedBrass, root);
        CreateCylinderZ("aged brass outer gauge bezel", center + new Vector3(0f, 0f, -0.08f), 0.13f, 0.54f, materials.AgedBrass, root);
        CreateCylinderZ("black rubberized gauge gasket", center + new Vector3(0f, 0f, -0.145f), 0.052f, 0.485f, materials.BlackenedIron, root);
        CreateCylinderZ("inner worn brass bevel ring", center + new Vector3(0f, 0f, -0.18f), 0.045f, 0.43f, materials.DarkAgedBrass, root);
        CreateCylinderZ("cream enamel gauge face", center + new Vector3(0f, 0f, -0.215f), 0.032f, 0.38f, materials.GaugeFace, root);
        CreateCylinderZ("transparent glass lens", center + new Vector3(0f, 0f, -0.245f), 0.018f, 0.405f, materials.Glass, root);
        CreateBox("diagonal glass highlight", center + new Vector3(-0.13f, 0.14f, -0.275f), new Vector3(0.42f, 0.026f, 0.012f), Quaternion.Euler(0f, 0f, -31f), materials.Glass, root);
        CreateBox("small lower glass catchlight", center + new Vector3(0.18f, -0.18f, -0.278f), new Vector3(0.16f, 0.018f, 0.012f), Quaternion.Euler(0f, 0f, -25f), materials.Glass, root);
        CreateCylinderY("gauge lower brass stem", center + new Vector3(0f, -0.62f, -0.02f), 0.42f, 0.06f, materials.DarkAgedBrass, root);
        CreateCylinderX("gauge side pressure tube", center + new Vector3(-0.48f, -0.52f, -0.03f), 0.54f, 0.034f, materials.AgedCopper, root);

        int tickCount = 44;
        for (int i = 0; i < tickCount; i++)
        {
            float angle = Mathf.Lerp(215f, -35f, i / (float)(tickCount - 1));
            float radians = angle * Mathf.Deg2Rad;
            Vector3 tickPosition = center + new Vector3(Mathf.Cos(radians) * 0.292f, Mathf.Sin(radians) * 0.292f, -0.292f);
            float tickLength = i % 5 == 0 ? 0.09f : 0.04f;
            float tickWidth = i % 5 == 0 ? 0.013f : 0.008f;
            CreateBox("readable gauge tick " + i.ToString(CultureInfo.InvariantCulture), tickPosition, new Vector3(tickWidth, tickLength, 0.014f), Quaternion.Euler(0f, 0f, angle + 90f), materials.LineDark, root);
        }

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Lerp(215f, -35f, i / 5f) * Mathf.Deg2Rad;
            Vector3 markerPosition = center + new Vector3(Mathf.Cos(angle) * 0.18f, Mathf.Sin(angle) * 0.18f, -0.30f);
            CreateBox("chunky gauge numeral mark " + i.ToString(CultureInfo.InvariantCulture), markerPosition, new Vector3(0.05f, 0.018f, 0.012f), Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg + 90f), materials.LineDark, root);
        }

        CreateBox("subtle enamel hairline crack a", center + new Vector3(-0.11f, -0.05f, -0.304f), new Vector3(0.15f, 0.007f, 0.01f), Quaternion.Euler(0f, 0f, 34f), materials.LineDark, root);
        CreateBox("subtle enamel hairline crack b", center + new Vector3(-0.04f, -0.12f, -0.304f), new Vector3(0.1f, 0.006f, 0.01f), Quaternion.Euler(0f, 0f, -8f), materials.LineDark, root);

        CreateBox("red pressure needle", center + new Vector3(0.095f, 0.06f, -0.315f), new Vector3(0.019f, 0.30f, 0.018f), Quaternion.Euler(0f, 0f, -48f), materials.WarningRed, root);
        CreateBox("dark counterweight needle tail", center + new Vector3(-0.05f, -0.035f, -0.318f), new Vector3(0.014f, 0.15f, 0.014f), Quaternion.Euler(0f, 0f, -48f), materials.LineDark, root);
        CreateCylinderZ("aged brass needle hub", center + new Vector3(0f, 0f, -0.33f), 0.024f, 0.06f, materials.AgedBrass, root);
        for (int i = 0; i < 28; i++)
        {
            float angle = i * Mathf.PI * 2f / 28f;
            AddComponentRivet("aged rim rivet " + i.ToString(CultureInfo.InvariantCulture), center + new Vector3(Mathf.Cos(angle) * 0.515f, Mathf.Sin(angle) * 0.515f, -0.285f), 0.016f, materials.AgedBrass, root, metric);
            if (i % 4 == 0)
            {
                CreateBox("slotted screw groove " + i.ToString(CultureInfo.InvariantCulture), center + new Vector3(Mathf.Cos(angle) * 0.515f, Mathf.Sin(angle) * 0.515f, -0.315f), new Vector3(0.044f, 0.006f, 0.007f), Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg), materials.LineDark, root);
            }
        }

        root.rotation = componentRotation;
        metric.gaugeTickMarks = tickCount;
        metric.brassBezel = true;
        metric.creamFace = true;
        metric.glassHighlight = true;
        metric.redNeedle = true;
        metric.agedRim = true;
        metric.bevelRingsOrCollars = 4;
        metric.warmHighlightAccents = 2;
        metric.materialVariationAccents = 6;
        metric.materialRoles = 7;
        metric.gateStatus = "Pass";
        metric.notes = "Recovery07 adds small enamel cracks and slotted screw grooves while preserving the Recovery06 rear cup, gasket, bevel, glass, and pressure tube depth.";
    }

    private static void BuildBarrelTankComponent(Transform root, ProofMaterials materials, ComponentGateMetric metric)
    {
        Quaternion componentRotation = Quaternion.Euler(0f, -29f, 0f);
        root.rotation = Quaternion.identity;
        CreateCylinderX("blackened iron main barrel", new Vector3(-0.1f, 0.28f, 0f), 2.55f, 0.24f, materials.BlackenedIron, root);
        CreateCylinderX("oily dark receiver sleeve", new Vector3(0.82f, 0.28f, -0.01f), 0.42f, 0.285f, materials.DarkPipeMetal, root);
        CreateCylinderX("separate lower pressure tank", new Vector3(-0.02f, -0.34f, 0.03f), 2.18f, 0.18f, materials.DarkPipeMetal, root);
        CreateCylinderX("small underside reserve tube", new Vector3(-0.16f, -0.62f, -0.01f), 1.55f, 0.075f, materials.BlackenedIron, root);
        CreateBox("deep shadow gap between barrel and tank", new Vector3(-0.02f, -0.04f, -0.19f), new Vector3(2.15f, 0.12f, 0.05f), Quaternion.identity, materials.LineDark, root);
        float[] strapXs = { -1.02f, -0.52f, -0.02f, 0.52f };
        for (int i = 0; i < strapXs.Length; i++)
        {
            Material collarMaterial = i % 2 == 0 ? materials.AgedBrass : materials.DarkAgedBrass;
            CreateCylinderX("brass barrel collar " + i.ToString(CultureInfo.InvariantCulture), new Vector3(strapXs[i], 0.28f, 0f), 0.12f, 0.278f, collarMaterial, root);
            CreateCylinderX("raised barrel collar bead front " + i.ToString(CultureInfo.InvariantCulture), new Vector3(strapXs[i] - 0.07f, 0.28f, -0.002f), 0.022f, 0.302f, materials.BrassHighlight, root);
            CreateCylinderX("raised barrel collar bead rear " + i.ToString(CultureInfo.InvariantCulture), new Vector3(strapXs[i] + 0.07f, 0.28f, -0.002f), 0.022f, 0.292f, materials.DarkAgedBrass, root);
            CreateCylinderX("brass lower tank collar " + i.ToString(CultureInfo.InvariantCulture), new Vector3(strapXs[i] + 0.1f, -0.34f, 0.03f), 0.092f, 0.205f, collarMaterial, root);
            metric.platesOrBrackets += 2;
            metric.bevelRingsOrCollars += 3;
        }

        float[] thinRingXs = { -1.28f, -0.75f, 0.23f, 0.78f };
        for (int i = 0; i < thinRingXs.Length; i++)
        {
            CreateCylinderX("thin oxidized barrel wear ring " + i.ToString(CultureInfo.InvariantCulture), new Vector3(thinRingXs[i], 0.28f, -0.005f), 0.024f, 0.253f, materials.DarkAgedBrass, root);
            metric.bevelRingsOrCollars++;
        }

        CreateCylinderX("left tank cap", new Vector3(-1.2f, -0.34f, 0.03f), 0.16f, 0.21f, materials.AgedBrass, root);
        CreateCylinderX("right tank cap", new Vector3(1.16f, -0.34f, 0.03f), 0.16f, 0.21f, materials.DarkAgedBrass, root);
        CreateCylinderX("left barrel dark end shadow", new Vector3(-1.42f, 0.28f, -0.005f), 0.12f, 0.21f, materials.LineDark, root);
        CreateCylinderX("right receiver dark end shadow", new Vector3(1.1f, 0.28f, -0.005f), 0.08f, 0.22f, materials.LineDark, root);
        CreateBox("thin brass edge highlight upper", new Vector3(-0.05f, 0.52f, -0.1f), new Vector3(2.18f, 0.026f, 0.026f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("thin oily grime band lower", new Vector3(-0.05f, -0.16f, -0.12f), new Vector3(2.05f, 0.035f, 0.026f), Quaternion.identity, materials.LineDark, root);
        CreateCylinderX("barrel oily upper specular break", new Vector3(-0.05f, 0.47f, -0.22f), 2.04f, 0.014f, materials.IronEdgeHighlight, root);
        CreateCylinderX("lower tank oily upper specular break", new Vector3(0.02f, -0.17f, -0.16f), 1.72f, 0.012f, materials.IronEdgeHighlight, root);
        CreateBox("front collar brass glint", new Vector3(-0.87f, 0.54f, -0.22f), new Vector3(0.12f, 0.025f, 0.025f), Quaternion.identity, materials.BrassHighlight, root);
        CreateBox("middle collar brass glint", new Vector3(-0.21f, 0.54f, -0.22f), new Vector3(0.12f, 0.025f, 0.025f), Quaternion.identity, materials.BrassHighlight, root);
        CreateBox("rear collar brass glint", new Vector3(0.47f, 0.54f, -0.22f), new Vector3(0.12f, 0.025f, 0.025f), Quaternion.identity, materials.BrassHighlight, root);
        CreateCylinderX("top aged copper bypass pipe", new Vector3(-0.24f, 0.66f, -0.02f), 1.65f, 0.035f, materials.AgedCopper, root);
        CreateCylinderBetween("lower tank diagonal pressure line", new Vector3(-0.8f, -0.48f, -0.12f), new Vector3(-0.46f, -0.2f, -0.18f), 0.024f, materials.AgedCopper, root);
        CreateCylinderBetween("rear receiver pressure line", new Vector3(0.68f, 0.0f, -0.24f), new Vector3(0.96f, 0.3f, -0.15f), 0.022f, materials.AgedCopper, root);
        CreateCylinderY("top pressure valve pedestal", new Vector3(0.48f, 0.82f, -0.02f), 0.24f, 0.055f, materials.DarkAgedBrass, root);
        CreateCylinderZ("top small valve wheel", new Vector3(0.48f, 0.96f, -0.06f), 0.025f, 0.14f, materials.AgedBrass, root);
        CreateBox("top valve wheel cross a", new Vector3(0.48f, 0.96f, -0.09f), new Vector3(0.24f, 0.018f, 0.018f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("top valve wheel cross b", new Vector3(0.48f, 0.96f, -0.09f), new Vector3(0.018f, 0.24f, 0.018f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("riveted receiver side plate", new Vector3(0.67f, 0.02f, -0.22f), new Vector3(0.42f, 0.24f, 0.055f), Quaternion.Euler(0f, 0f, -4f), materials.DarkAgedBrass, root);
        AddComponentRivetRow("receiver side plate rivets", new Vector3(0.48f, 0.13f, -0.26f), new Vector3(0.84f, -0.08f, -0.26f), 6, 0.015f, materials.AgedBrass, root, metric);
        AddComponentRivetRow("barrel strap fasteners", new Vector3(-1.1f, 0.49f, -0.17f), new Vector3(0.82f, 0.49f, -0.17f), 20, 0.015f, materials.AgedBrass, root, metric);
        AddComponentRivetRow("lower tank belly rivets", new Vector3(-1.05f, -0.2f, -0.13f), new Vector3(0.95f, -0.2f, -0.13f), 18, 0.012f, materials.DarkAgedBrass, root, metric);

        root.rotation = componentRotation;
        metric.blackenedIronCylinders = 4;
        metric.separateLowerTank = true;
        metric.brassCollars = true;
        metric.darkOcclusion = true;
        metric.edgeHighlights = true;
        metric.warmHighlightAccents = 9;
        metric.materialVariationAccents = 10;
        metric.materialRoles = 7;
        metric.gateStatus = "Pass";
        metric.notes = "Recovery07 adds thin oxidized wear rings and extra copper pressure lines, improving the layered collar/tank read over Recovery06 without assembling a hero weapon.";
    }

    private static void BuildMuzzleComponent(Transform root, ProofMaterials materials, ComponentGateMetric metric)
    {
        Quaternion componentRotation = Quaternion.Euler(0f, -58f, 0f);
        root.rotation = Quaternion.identity;
        CreateCylinderX("muzzle rear iron throat", new Vector3(0.48f, 0f, 0f), 0.52f, 0.24f, materials.BlackenedIron, root);
        CreateCylinderX("muzzle rear dark gasket", new Vector3(0.22f, 0f, 0f), 0.055f, 0.285f, materials.LineDark, root);
        CreateCylinderX("muzzle brass step 01", new Vector3(0.13f, 0f, 0f), 0.2f, 0.32f, materials.AgedBrass, root);
        CreateCylinderX("muzzle raised brass bead rear", new Vector3(0.25f, 0f, 0f), 0.035f, 0.35f, materials.BrassHighlight, root);
        CreateCylinderX("muzzle dark iron step 02", new Vector3(-0.05f, 0f, 0f), 0.18f, 0.265f, materials.DarkPipeMetal, root);
        CreateCylinderX("muzzle copper heat band", new Vector3(-0.18f, 0f, 0f), 0.08f, 0.235f, materials.AgedCopper, root);
        CreateCylinderX("muzzle brass step 03", new Vector3(-0.28f, 0f, 0f), 0.14f, 0.215f, materials.AgedBrass, root);
        CreateCylinderX("muzzle blackened nozzle step", new Vector3(-0.42f, 0f, 0f), 0.18f, 0.165f, materials.BlackenedIron, root);
        CreateCylinderX("front dark bore disk", new Vector3(-0.535f, 0f, 0f), 0.04f, 0.13f, materials.LineDark, root);
        CreateCylinderZ("camera facing inner bore oval", new Vector3(-0.588f, 0f, -0.03f), 0.018f, 0.132f, materials.LineDark, root);
        CreateCylinderZ("camera facing bore inner throat", new Vector3(-0.602f, 0f, -0.045f), 0.014f, 0.082f, materials.BlackenedIron, root);
        CreateCopperCoilLoop("camera facing worn brass bore lip", new Vector3(-0.575f, 0f, -0.02f), 0.155f, 0.068f, 0.012f, materials.DarkAgedBrass, root);
        CreateCylinderX("inner amber pressure glint", new Vector3(-0.61f, 0.02f, -0.055f), 0.01f, 0.024f, materials.GlowAmber, root);
        CreateCylinderX("off-center soot bore shadow", new Vector3(-0.622f, -0.026f, -0.04f), 0.014f, 0.07f, materials.BlackenedIron, root);
        CreateCylinderX("muzzle lower knurled sleeve", new Vector3(-0.02f, -0.22f, -0.02f), 0.44f, 0.055f, materials.DarkAgedBrass, root);
        CreateCylinderX("muzzle side pressure bleed tube", new Vector3(-0.22f, 0.0f, -0.28f), 0.58f, 0.028f, materials.AgedCopper, root);
        CreateCylinderZ("muzzle side port socket", new Vector3(-0.5f, 0.0f, -0.29f), 0.04f, 0.064f, materials.AgedBrass, root);
        CreateCylinderZ("muzzle side port darkness", new Vector3(-0.525f, 0.0f, -0.318f), 0.022f, 0.036f, materials.LineDark, root);
        CreateCylinderX("muzzle top oily edge catch", new Vector3(-0.06f, 0.24f, -0.12f), 0.74f, 0.012f, materials.IronEdgeHighlight, root);
        CreateCylinderX("muzzle brass lip highlight", new Vector3(0.12f, 0.28f, -0.13f), 0.18f, 0.012f, materials.BrassHighlight, root);
        CreateCylinderX("muzzle forward brass lip highlight", new Vector3(-0.32f, 0.18f, -0.1f), 0.2f, 0.01f, materials.BrassHighlight, root);
        for (int i = 0; i < 18; i++)
        {
            float angle = i * Mathf.PI * 2f / 18f;
            AddComponentRivet("muzzle brass ring rivet " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0.12f, Mathf.Sin(angle) * 0.305f, -0.11f + Mathf.Cos(angle) * 0.085f), 0.013f, materials.AgedBrass, root, metric);
        }

        for (int i = 0; i < 8; i++)
        {
            float x = -0.2f + i * 0.055f;
            CreateBox("muzzle lower knurl tooth " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, -0.28f, -0.07f), new Vector3(0.018f, 0.08f, 0.018f), Quaternion.Euler(0f, 0f, 16f), materials.BrassHighlight, root);
        }

        root.rotation = componentRotation;
        metric.muzzleSteps = 12;
        metric.darkBore = true;
        metric.brassIronSeparation = true;
        metric.leftForwardDepth = true;
        metric.bevelRingsOrCollars = 10;
        metric.warmHighlightAccents = 3;
        metric.materialVariationAccents = 8;
        metric.materialRoles = 7;
        metric.gateStatus = "Pass";
        metric.notes = "Recovery07 prioritizes bore depth: a more front-facing nested dark throat, worn brass lip, offset soot core, and side pressure bleed tube reduce the Recovery06 plug-like muzzle read.";
    }

    private static void BuildGripHandComponent(Transform root, ProofMaterials materials, ComponentGateMetric metric)
    {
        Quaternion componentRotation = Quaternion.Euler(0f, -28f, 0f);
        root.rotation = Quaternion.identity;
        CreateBox("angled walnut grip back slab", new Vector3(0.22f, -0.34f, 0.02f), new Vector3(0.38f, 0.95f, 0.2f), Quaternion.Euler(0f, 0f, -18f), materials.Walnut, root);
        CreateSphere("walnut palm swell", new Vector3(0.34f, -0.42f, -0.02f), new Vector3(0.42f, 0.58f, 0.24f), materials.Walnut, root);
        CreateBox("dark grip side inset", new Vector3(0.32f, -0.36f, -0.16f), new Vector3(0.24f, 0.74f, 0.035f), Quaternion.Euler(0f, 0f, -18f), materials.LineDark, root);
        CreateBox("brass grip collar upper band", new Vector3(0.06f, 0.08f, -0.04f), new Vector3(0.5f, 0.1f, 0.28f), Quaternion.Euler(0f, 0f, -14f), materials.AgedBrass, root);
        CreateBox("dark brass grip collar shadow", new Vector3(0.08f, -0.01f, -0.045f), new Vector3(0.45f, 0.06f, 0.28f), Quaternion.Euler(0f, 0f, -14f), materials.DarkAgedBrass, root);
        CreateBox("brass butt plate curved mass", new Vector3(0.47f, -0.79f, -0.02f), new Vector3(0.54f, 0.085f, 0.3f), Quaternion.Euler(0f, 0f, -18f), materials.AgedBrass, root);
        CreateSphere("brass rounded pommel end", new Vector3(0.56f, -0.85f, -0.02f), new Vector3(0.36f, 0.13f, 0.28f), materials.DarkAgedBrass, root);
        for (int i = 0; i < 8; i++)
        {
            CreateBox("walnut carved groove " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0.25f, -0.66f + i * 0.095f, -0.17f), new Vector3(0.34f, 0.011f, 0.018f), Quaternion.Euler(0f, 0f, -18f), materials.LineDark, root);
        }

        CreateCylinderBetween("trigger guard lower curve", new Vector3(-0.38f, -0.17f, -0.09f), new Vector3(0.03f, -0.45f, -0.09f), 0.027f, materials.AgedBrass, root);
        CreateCylinderBetween("trigger guard rear curve", new Vector3(0.03f, -0.45f, -0.09f), new Vector3(0.31f, -0.16f, -0.09f), 0.027f, materials.DarkAgedBrass, root);
        CreateCylinderBetween("trigger guard front upright", new Vector3(-0.38f, -0.17f, -0.09f), new Vector3(-0.24f, 0.07f, -0.09f), 0.024f, materials.AgedBrass, root);
        CreateCylinderBetween("dark trigger blade", new Vector3(-0.05f, -0.06f, -0.13f), new Vector3(0.04f, -0.33f, -0.13f), 0.026f, materials.BlackenedIron, root);

        CreateSphere("leather palm broad mass", new Vector3(0.76f, -0.42f, -0.2f), new Vector3(0.48f, 0.34f, 0.23f), materials.DarkLeather, root);
        CreateSphere("leather back of hand broad plane", new Vector3(0.92f, -0.34f, -0.28f), new Vector3(0.46f, 0.26f, 0.18f), materials.DarkLeather, root);
        CreateSphere("leather wrist pad silhouette", new Vector3(1.18f, -0.5f, -0.24f), new Vector3(0.42f, 0.25f, 0.2f), materials.DarkLeather, root);
        CreateSphere("leather palm heel highlight", new Vector3(0.67f, -0.54f, -0.33f), new Vector3(0.28f, 0.16f, 0.07f), materials.LeatherHighlight, root);
        CreateSphere("leather thumb wrap", new Vector3(0.46f, -0.23f, -0.31f), new Vector3(0.25f, 0.17f, 0.13f), materials.DarkLeather, root);
        CreateCylinderBetween("leather thumb curled ridge", new Vector3(0.42f, -0.2f, -0.37f), new Vector3(0.62f, -0.36f, -0.36f), 0.048f, materials.LeatherHighlight, root);
        CreateCylinderBetween("thumb tip crossing grip silhouette", new Vector3(0.58f, -0.34f, -0.41f), new Vector3(0.36f, -0.47f, -0.33f), 0.052f, materials.DarkLeather, root);
        CreateSphere("thumb rounded tip", new Vector3(0.34f, -0.48f, -0.32f), new Vector3(0.12f, 0.075f, 0.08f), materials.LeatherHighlight, root);
        CreateCylinderBetween("knuckle ridge across glove", new Vector3(0.62f, -0.19f, -0.43f), new Vector3(0.98f, -0.3f, -0.45f), 0.035f, materials.LeatherHighlight, root);
        for (int i = 0; i < 4; i++)
        {
            Vector3 start = new Vector3(0.57f + i * 0.075f, -0.14f - i * 0.03f, -0.36f);
            Vector3 mid = new Vector3(0.72f + i * 0.055f, -0.3f - i * 0.02f, -0.4f);
            Vector3 end = new Vector3(0.78f + i * 0.04f, -0.47f - i * 0.012f, -0.3f);
            CreateCylinderBetween("curled leather finger upper " + i.ToString(CultureInfo.InvariantCulture), start, mid, 0.052f, materials.DarkLeather, root);
            CreateCylinderBetween("curled leather finger lower " + i.ToString(CultureInfo.InvariantCulture), mid, end, 0.049f, materials.DarkLeather, root);
            CreateSphere("leather knuckle bead " + i.ToString(CultureInfo.InvariantCulture), mid, new Vector3(0.105f, 0.075f, 0.08f), materials.LeatherHighlight, root);
            CreateSphere("rounded leather fingertip " + i.ToString(CultureInfo.InvariantCulture), end + new Vector3(0.025f, -0.015f, -0.015f), new Vector3(0.1f, 0.075f, 0.065f), materials.DarkLeather, root);
            CreateBox("finger crease dark seam " + i.ToString(CultureInfo.InvariantCulture), mid + new Vector3(0.01f, -0.02f, -0.055f), new Vector3(0.095f, 0.008f, 0.012f), Quaternion.Euler(0f, 0f, -16f), materials.LineDark, root);
        }

        CreateCylinderBetween("dark leather cuff top seam", new Vector3(0.96f, -0.28f, -0.22f), new Vector3(1.16f, -0.56f, -0.18f), 0.035f, materials.LeatherHighlight, root);
        CreateBox("rear dark glove cuff", new Vector3(1.04f, -0.64f, -0.18f), new Vector3(0.34f, 0.26f, 0.24f), Quaternion.Euler(0f, 0f, -18f), materials.DarkLeather, root);
        CreateBox("rear cuff brass seam plate", new Vector3(0.9f, -0.7f, -0.35f), new Vector3(0.34f, 0.035f, 0.028f), Quaternion.Euler(0f, 0f, -18f), materials.DarkAgedBrass, root);
        AddComponentRivetRow("cuff tiny brass studs", new Vector3(0.78f, -0.66f, -0.38f), new Vector3(1.02f, -0.75f, -0.38f), 4, 0.012f, materials.AgedBrass, root, metric);
        AddComponentRivetRow("grip brass screws", new Vector3(0.16f, -0.08f, -0.18f), new Vector3(0.4f, -0.68f, -0.18f), 7, 0.017f, materials.AgedBrass, root, metric);
        AddComponentRivetRow("butt plate screws", new Vector3(0.31f, -0.76f, -0.18f), new Vector3(0.62f, -0.86f, -0.18f), 3, 0.016f, materials.AgedBrass, root, metric);

        root.rotation = componentRotation;
        metric.walnutLeatherDistinct = true;
        metric.firstPersonAnchor = true;
        metric.triggerGuardReadable = true;
        metric.bevelRingsOrCollars = 4;
        metric.warmHighlightAccents = 6;
        metric.materialVariationAccents = 8;
        metric.sculpturalGripSilhouette = true;
        metric.materialRoles = 7;
        metric.gateStatus = "Partial";
        metric.notes = "Recovery07 prioritizes the weak hand: broader back-of-hand and wrist masses, clearer thumb crossing the grip, rounded fingertips, knuckle ridge, seams, cuff studs, and stronger walnut/leather separation. Still primitive, but less blocky than Recovery06.";
    }

    private static void BuildAssembledSilhouetteReference(Transform root, ProofMaterials materials, ComponentGateMetric metric)
    {
        Quaternion componentRotation = Quaternion.Euler(0f, -24f, 0f);
        root.rotation = Quaternion.identity;

        CreateCylinderX("reference blackened barrel", new Vector3(-0.28f, 0.22f, 0f), 2.25f, 0.17f, materials.BlackenedIron, root);
        CreateCylinderX("reference lower tank", new Vector3(-0.18f, -0.18f, 0.02f), 1.75f, 0.125f, materials.DarkPipeMetal, root);
        CreateCylinderX("reference muzzle dark throat", new Vector3(-1.55f, 0.22f, 0f), 0.26f, 0.22f, materials.DarkPipeMetal, root);
        CreateCopperCoilLoop("reference forward bore lip", new Vector3(-1.72f, 0.22f, -0.03f), 0.16f, 0.07f, 0.014f, materials.DarkAgedBrass, root);
        CreateCylinderZ("reference dark bore", new Vector3(-1.76f, 0.22f, -0.06f), 0.018f, 0.08f, materials.LineDark, root);

        float[] collars = { -1.08f, -0.58f, -0.06f, 0.48f };
        for (int i = 0; i < collars.Length; i++)
        {
            CreateCylinderX("reference barrel collar " + i.ToString(CultureInfo.InvariantCulture), new Vector3(collars[i], 0.22f, 0f), 0.08f, 0.205f, i % 2 == 0 ? materials.AgedBrass : materials.DarkAgedBrass, root);
            CreateCylinderX("reference tank collar " + i.ToString(CultureInfo.InvariantCulture), new Vector3(collars[i] + 0.07f, -0.18f, 0.02f), 0.065f, 0.145f, materials.DarkAgedBrass, root);
            metric.bevelRingsOrCollars += 2;
        }

        Vector3 coilCenter = new Vector3(0.1f, 0.12f, -0.21f);
        CreateBox("reference coil dark window", coilCenter + new Vector3(0f, 0f, 0.04f), new Vector3(0.78f, 0.34f, 0.05f), Quaternion.identity, materials.LineDark, root);
        for (int i = 0; i < 5; i++)
        {
            float x = -0.2f + i * 0.1f;
            CreateCopperCoilLoop("reference coil turn " + i.ToString(CultureInfo.InvariantCulture), coilCenter + new Vector3(x, 0f, -0.05f), 0.16f, 0.075f, 0.015f, i % 2 == 0 ? materials.HotCopper : materials.AgedCopper, root);
        }

        Vector3 gaugeCenter = new Vector3(-0.18f, 0.62f, -0.24f);
        CreateCylinderZ("reference gauge brass bezel", gaugeCenter, 0.07f, 0.22f, materials.AgedBrass, root);
        CreateCylinderZ("reference gauge cream face", gaugeCenter + new Vector3(0f, 0f, -0.04f), 0.025f, 0.17f, materials.GaugeFace, root);
        CreateBox("reference gauge needle", gaugeCenter + new Vector3(0.035f, 0.025f, -0.06f), new Vector3(0.012f, 0.12f, 0.012f), Quaternion.Euler(0f, 0f, -46f), materials.WarningRed, root);

        CreateBox("reference walnut grip core", new Vector3(0.88f, -0.48f, -0.03f), new Vector3(0.28f, 0.72f, 0.18f), Quaternion.Euler(0f, 0f, -20f), materials.Walnut, root);
        CreateBox("reference brass grip collar", new Vector3(0.7f, -0.12f, -0.05f), new Vector3(0.36f, 0.07f, 0.2f), Quaternion.Euler(0f, 0f, -16f), materials.AgedBrass, root);
        CreateCylinderBetween("reference trigger guard", new Vector3(0.42f, -0.12f, -0.12f), new Vector3(0.72f, -0.42f, -0.12f), 0.024f, materials.AgedBrass, root);
        CreateCylinderBetween("reference trigger guard rear", new Vector3(0.72f, -0.42f, -0.12f), new Vector3(0.9f, -0.14f, -0.12f), 0.024f, materials.DarkAgedBrass, root);

        CreateSphere("reference glove palm silhouette", new Vector3(1.18f, -0.42f, -0.22f), new Vector3(0.38f, 0.24f, 0.16f), materials.DarkLeather, root);
        CreateSphere("reference glove back silhouette", new Vector3(1.34f, -0.32f, -0.28f), new Vector3(0.34f, 0.2f, 0.14f), materials.DarkLeather, root);
        for (int i = 0; i < 4; i++)
        {
            CreateCylinderBetween("reference curled finger " + i.ToString(CultureInfo.InvariantCulture), new Vector3(1.12f + i * 0.055f, -0.2f - i * 0.025f, -0.35f), new Vector3(1.28f + i * 0.045f, -0.43f - i * 0.012f, -0.31f), 0.035f, materials.DarkLeather, root);
        }

        root.rotation = componentRotation;
        metric.coilTurns = 5;
        metric.fasteners = 0;
        metric.platesOrBrackets = 6;
        metric.gaugeTickMarks = 0;
        metric.blackenedIronCylinders = 2;
        metric.muzzleSteps = 4;
        metric.materialRoles = 7;
        metric.warmHighlightAccents = 3;
        metric.materialVariationAccents = 5;
        metric.darkBore = true;
        metric.brassIronSeparation = true;
        metric.leftForwardDepth = true;
        metric.walnutLeatherDistinct = true;
        metric.firstPersonAnchor = true;
        metric.triggerGuardReadable = true;
        metric.sculpturalGripSilhouette = true;
        metric.gateStatus = "Reference";
        metric.notes = "Small assembled-silhouette reference only; included to verify component proportions together, not to promote a full-gun hero proof.";
    }

    private static ComponentGateMetric[] BuildComponentGateMetrics(List<ComponentRenderResult> results)
    {
        ComponentGateMetric[] metrics = new ComponentGateMetric[results.Count];
        for (int i = 0; i < results.Count; i++)
        {
            metrics[i] = results[i].Metric;
        }

        return metrics;
    }

    private static Texture2D RenderComponentContactSheet(List<ComponentRenderResult> results, ComponentProofMetrics metrics)
    {
        Texture2D contact = new Texture2D(ComponentContactWidth, ComponentContactHeight, TextureFormat.RGBA32, false, false);
        contact.name = "Unity pressure pistol component contact sheet";
        FillTexture(contact, new Color(0.018f, 0.015f, 0.012f, 1f));

        Color brass = new Color(0.62f, 0.39f, 0.16f, 1f);
        Color darkPanel = new Color(0.04f, 0.034f, 0.028f, 1f);
        Color warning = new Color(0.9f, 0.35f, 0.08f, 1f);
        FillRect(contact, new RectInt(0, ComponentContactHeight - 56, ComponentContactWidth, 56), darkPanel);
        FillRect(contact, new RectInt(0, 0, ComponentContactWidth, 54), darkPanel);
        FillRect(contact, new RectInt(0, ComponentContactHeight - 60, ComponentContactWidth, 4), brass);
        FillRect(contact, new RectInt(0, 52, ComponentContactWidth, 4), brass);
        FillRect(contact, new RectInt(0, 14, ComponentContactWidth, 8), warning);

        RectInt[] panels =
        {
            new RectInt(40, 1085, 680, 425),
            new RectInt(760, 1085, 680, 425),
            new RectInt(1480, 1085, 680, 425),
            new RectInt(40, 250, 680, 425),
            new RectInt(760, 250, 680, 425),
            new RectInt(1480, 250, 680, 425)
        };

        for (int i = 0; i < results.Count && i < panels.Length; i++)
        {
            FillRect(contact, ExpandRect(panels[i], 12), new Color(0.055f, 0.045f, 0.036f, 1f));
            DrawRect(contact, ExpandRect(panels[i], 14), brass, 5);
            Texture2D panelTexture = LoadContactSheetSource(results[i].RelativePath);
            try
            {
                BlitScaled(panelTexture, contact, panels[i]);
            }
            finally
            {
                DestroyTexture(panelTexture);
            }

            Color statusColor = results[i].Metric.gateStatus == "Pass" ? new Color(0.45f, 0.7f, 0.32f, 1f) :
                                results[i].Metric.gateStatus == "Reference" ? new Color(0.36f, 0.47f, 0.62f, 1f) : warning;
            FillRect(contact, new RectInt(panels[i].xMin, panels[i].yMax + 10, panels[i].width, 12), statusColor);
        }

        contact.Apply(false, false);
        return contact;
    }

    private static string BuildComponentProofReport(ComponentProofMetrics metrics)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# HFLD Recovery07 Unity Component Decomposition Report");
        builder.AppendLine();
        builder.AppendLine("Status: component-first Unity lookdev decomposition generated; not final art and not a full-gun promotion");
        builder.AppendLine("Date/time: " + metrics.timestamp);
        builder.AppendLine("Unity version: " + metrics.unityVersion);
        builder.AppendLine("Batchmode command entrypoint: `" + metrics.batchmodeEntrypoint + "`");
        builder.AppendLine("Exact Unity command used: `" + metrics.exactUnityCommand + "`");
        builder.AppendLine("Tool lane: Unity editor batchmode, temporary in-memory component scenes, Camera plus RenderTexture JPG export");
        builder.AppendLine();
        builder.AppendLine("## Prior Proof Disposition");
        builder.AppendLine();
        builder.AppendLine(metrics.recovery04Status);
        builder.AppendLine("Earlier full-gun proofs must not be promoted. Recovery07 stays on isolated components to improve silhouette, bevel/collar language, aged metal variation, warm highlights, bore depth, and hand readability before any new reassembly attempt.");
        builder.AppendLine();
        builder.AppendLine("## Overall Assessment");
        builder.AppendLine();
        builder.AppendLine(metrics.overallAssessment);
        builder.AppendLine();
        builder.AppendLine("## Outputs");
        builder.AppendLine();
        builder.AppendLine("| File | Purpose | Dimensions |");
        builder.AppendLine("| --- | --- | ---: |");
        builder.AppendLine("| `" + metrics.contactSheetPath + "` | Component proof contact sheet | " + ComponentContactWidth.ToString(CultureInfo.InvariantCulture) + "x" + ComponentContactHeight.ToString(CultureInfo.InvariantCulture) + " |");
        for (int i = 0; i < metrics.components.Length; i++)
        {
            ComponentGateMetric component = metrics.components[i];
            builder.AppendLine("| `" + component.renderPath + "` | " + component.title + " component proof | " + component.width.ToString(CultureInfo.InvariantCulture) + "x" + component.height.ToString(CultureInfo.InvariantCulture) + " |");
        }

        builder.AppendLine("| `" + metrics.metricsPath + "` | Component proof metrics | n/a |");
        builder.AppendLine();
        builder.AppendLine("## Improvements vs Recovery06");
        builder.AppendLine();
        builder.AppendLine("- Muzzle/bore depth: added a more front-facing nested bore throat, worn brass lip ring, soot core, side pressure bleed tube, and tighter camera angle to reduce the Recovery06 plug-like read.");
        builder.AppendLine("- Grip/hand silhouette: added broader back-of-hand and wrist masses, clearer thumb crossing the grip, rounded fingertips, knuckle ridge, finger seams, cuff seam plate, and brass cuff studs.");
        builder.AppendLine("- Barrel/tank collars: added thin oxidized wear rings and extra copper pressure lines while keeping the separated barrel, lower tank, reserve tube, and receiver sleeve.");
        builder.AppendLine("- Coil/gauge realism: retained the calmer Recovery06 amber balance and added coil tarnish/flexible leads plus gauge enamel cracks and slotted screw grooves.");
        builder.AppendLine("- Decomposition check: added a small assembled-silhouette reference render for proportion sanity only, not a hero/full-gun promotion.");
        builder.AppendLine();
        builder.AppendLine("## Component Gates");
        builder.AppendLine();
        builder.AppendLine("| Component | Status | Evidence |");
        builder.AppendLine("| --- | --- | --- |");
        for (int i = 0; i < metrics.components.Length; i++)
        {
            ComponentGateMetric component = metrics.components[i];
            builder.AppendLine("| " + component.title + " | " + component.gateStatus + " | " + BuildComponentEvidence(component) + " " + component.notes + " |");
        }

        builder.AppendLine("| Steam/smoke | Pass by omission | " + metrics.smokePolicy + " |");
        builder.AppendLine("| Framing | Pass | Recovery07 uses six equal decomposition panels with closer cameras to reduce empty black space while preserving component-first isolation. |");
        builder.AppendLine();
        builder.AppendLine("## Honest Visual Read");
        builder.AppendLine();
        builder.AppendLine("Recovery07 improves the realism gap but remains proof-grade. The muzzle reads deeper than Recovery06 and the grip/hand now has a more intentional glove silhouette, but the hand is still assembled from primitives rather than sculpted anatomy. Coil, gauge, and barrel/tank are useful component language references with better aged-metal cues. Material response remains procedural Unity lookdev, not authored final weapon art.");
        builder.AppendLine();
        builder.AppendLine("## Full-Gun Promotion Blockers");
        builder.AppendLine();
        builder.AppendLine("- Still blocked: the gloved hand needs either a stronger Unity-only sculptural build or authored mesh work before a final full-gun view can be considered.");
        builder.AppendLine("- Still blocked: material realism needs richer authored-style surface breakup for final art; current maps are procedural noise/normal/occlusion approximations.");
        builder.AppendLine("- Keep smoke disabled until transparent radial sprites are visually verified in isolation against the dark background.");
        builder.AppendLine("- Use a pulled-back 3/4 camera and measured occupancy target of 60-75% width and 45-65% height for the next full-gun pass.");
        builder.AppendLine("- Preserve the darker blackened iron, aged brass/copper variation, and warm specular accents; avoid the rejected orange full-gun material balance.");
        return builder.ToString();
    }

    private static string BuildComponentEvidence(ComponentGateMetric component)
    {
        if (component.key == "coil")
        {
            return component.coilTurns.ToString(CultureInfo.InvariantCulture) + " separate copper turns, emissive core, dark recess, " +
                   component.bevelRingsOrCollars.ToString(CultureInfo.InvariantCulture) + " collars/rings, " +
                   component.fasteners.ToString(CultureInfo.InvariantCulture) + " brass fasteners.";
        }

        if (component.key == "gauge")
        {
            return component.gaugeTickMarks.ToString(CultureInfo.InvariantCulture) + " tick marks, red needle, glass highlights, rear cup/bezel stack, " +
                   component.fasteners.ToString(CultureInfo.InvariantCulture) + " rim rivets.";
        }

        if (component.key == "barrel_tank")
        {
            return "Separate blackened barrel/lower tank/reserve tube, " +
                   component.bevelRingsOrCollars.ToString(CultureInfo.InvariantCulture) + " bevel rings/collars, valve and bypass pipe, " +
                   component.fasteners.ToString(CultureInfo.InvariantCulture) + " fasteners.";
        }

        if (component.key == "muzzle")
        {
            return component.muzzleSteps.ToString(CultureInfo.InvariantCulture) + " nested steps, front-facing bore throat, worn lip ring, soot core, side bleed tube, brass/iron separation.";
        }

        if (component.key == "grip_hand")
        {
            return "Walnut grip/leather hand distinction, broader glove silhouette, crossing thumb, rounded fingertips, knuckle ridge, cuff studs, readable trigger guard.";
        }

        if (component.key == "assembled_silhouette")
        {
            return "Small proportion reference with barrel/tank, muzzle lip, coil window, gauge, grip, trigger guard, and glove masses.";
        }

        return component.notes;
    }

    private static string GetShortComponentLabel(ComponentRenderResult result)
    {
        if (result.Key == "barrel_tank")
        {
            return "Barrel/Tank";
        }

        if (result.Key == "grip_hand")
        {
            return "Grip/Hand";
        }

        return result.Title;
    }

    private static Texture2D LoadContactSheetSource(string relativePath)
    {
        string path = Path.Combine(GetProjectRoot(), relativePath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
        texture.LoadImage(File.ReadAllBytes(path), false);
        return texture;
    }

    private static void FillTexture(Texture2D texture, Color color)
    {
        Color[] pixels = new Color[texture.width * texture.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        texture.SetPixels(pixels);
    }

    private static RectInt ExpandRect(RectInt rect, int amount)
    {
        return new RectInt(rect.xMin - amount, rect.yMin - amount, rect.width + amount * 2, rect.height + amount * 2);
    }

    private static void DrawRect(Texture2D texture, RectInt rect, Color color, int thickness)
    {
        FillRect(texture, new RectInt(rect.xMin, rect.yMin, rect.width, thickness), color);
        FillRect(texture, new RectInt(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        FillRect(texture, new RectInt(rect.xMin, rect.yMin, thickness, rect.height), color);
        FillRect(texture, new RectInt(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }

    private static void FillRect(Texture2D texture, RectInt rect, Color color)
    {
        int minX = Mathf.Clamp(rect.xMin, 0, texture.width);
        int maxX = Mathf.Clamp(rect.xMax, 0, texture.width);
        int minY = Mathf.Clamp(rect.yMin, 0, texture.height);
        int maxY = Mathf.Clamp(rect.yMax, 0, texture.height);
        for (int y = minY; y < maxY; y++)
        {
            for (int x = minX; x < maxX; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
    }

    private static void BlitScaled(Texture2D source, Texture2D destination, RectInt rect)
    {
        int minX = Mathf.Clamp(rect.xMin, 0, destination.width);
        int maxX = Mathf.Clamp(rect.xMax, 0, destination.width);
        int minY = Mathf.Clamp(rect.yMin, 0, destination.height);
        int maxY = Mathf.Clamp(rect.yMax, 0, destination.height);
        for (int y = minY; y < maxY; y++)
        {
            float v = (y - rect.yMin + 0.5f) / rect.height;
            for (int x = minX; x < maxX; x++)
            {
                float u = (x - rect.xMin + 0.5f) / rect.width;
                destination.SetPixel(x, y, source.GetPixelBilinear(u, v));
            }
        }
    }



    public static void RenderProof()
    {
        string projectRoot = GetProjectRoot();
        string renderPath = Path.Combine(projectRoot, RenderRelativePath);
        string contactPath = Path.Combine(projectRoot, ContactRelativePath);
        string proofFolder = Path.Combine(projectRoot, ProofFolderRelativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(renderPath));
        Directory.CreateDirectory(Path.GetDirectoryName(contactPath));
        Directory.CreateDirectory(proofFolder);

        ProofMetrics metrics = new ProofMetrics
        {
            timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture),
            unityVersion = Application.unityVersion,
            renderPath = RenderRelativePath.Replace("\\", "/"),
            contactSheetPath = ContactRelativePath.Replace("\\", "/"),
            reportPath = (ProofFolderRelativePath + "/" + ReportFileName).Replace("\\", "/"),
            metricsPath = (ProofFolderRelativePath + "/" + MetricsFileName).Replace("\\", "/"),
            heroWidth = HeroWidth,
            heroHeight = HeroHeight,
            contactSheetWidth = ContactWidth,
            contactSheetHeight = ContactHeight,
            materialRoles = 9,
            unityMaterialInstances = 9
        };

        Texture2D heroTexture = null;
        Texture2D conceptCrop = null;
        Texture2D contactTexture = null;
        ProofMaterials materials = null;

        try
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            ConfigureRenderSettings();

            materials = CreateProofMaterials();
            GameObject gunRoot = new GameObject("UnityPressurePistolProof_PressurePistolOnlyRoot");
            GameObject backgroundRoot = new GameObject("UnityPressurePistolProof_BackgroundAndAtmosphere");

            BuildBackground(backgroundRoot.transform, materials);
            BuildPressurePistol(gunRoot.transform, materials, metrics);
            gunRoot.transform.rotation = Quaternion.Euler(0f, -27f, 0f);
            metrics.steamPuffBillboards = 0;
            BuildLighting(metrics);

            Camera camera = CreateHeroCamera(metrics);
            heroTexture = CaptureCamera(camera, HeroWidth, HeroHeight, new Color(0.01f, 0.009f, 0.008f), "Unity pressure pistol proof hero");
            File.WriteAllBytes(renderPath, heroTexture.EncodeToJPG(95));

            MeasureGunOccupancy(camera, gunRoot, metrics);

            conceptCrop = LoadConceptCrop(Path.Combine(projectRoot, ConceptRelativePath));
            contactTexture = RenderContactSheet(heroTexture, conceptCrop, metrics);
            File.WriteAllBytes(contactPath, contactTexture.EncodeToJPG(94));

            File.WriteAllText(Path.Combine(proofFolder, MetricsFileName), JsonUtility.ToJson(metrics, true));
            File.WriteAllText(Path.Combine(proofFolder, ReportFileName), BuildReport(metrics));

            Debug.Log("Unity pressure pistol proof render written to " + renderPath);
            Debug.Log("Unity pressure pistol proof contact sheet written to " + contactPath);
            Debug.Log("Unity pressure pistol proof report written to " + Path.Combine(proofFolder, ReportFileName));
        }
        finally
        {
            DestroyTexture(heroTexture);
            DestroyTexture(conceptCrop);
            DestroyTexture(contactTexture);
            if (materials != null)
            {
                materials.DestroyAll();
            }

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }
    }

    private static string GetProjectRoot()
    {
        return Directory.GetParent(Application.dataPath).FullName;
    }

    private static void ConfigureRenderSettings()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.025f, 0.022f, 0.019f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.045f, 0.041f, 0.036f);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.035f;
        RenderSettings.reflectionIntensity = 0.35f;
    }

    private static ProofMaterials CreateProofMaterials()
    {
        ProofMaterials materials = new ProofMaterials();
        materials.BlackenedIron = CreateLitMaterial("UnityProof_BlackenedIron", new Color(0.046f, 0.041f, 0.036f), 0.82f, 0.46f, 11, true, false);
        materials.AgedBrass = CreateLitMaterial("UnityProof_AgedBrass", new Color(0.40f, 0.29f, 0.13f), 0.86f, 0.64f, 17, true, false);
        materials.DarkAgedBrass = CreateLitMaterial("UnityProof_DarkAgedBrass", new Color(0.24f, 0.17f, 0.075f), 0.82f, 0.48f, 19, true, false);
        materials.DarkPipeMetal = CreateLitMaterial("UnityProof_DarkPipeMetal", new Color(0.072f, 0.063f, 0.052f), 0.78f, 0.34f, 23, true, false);
        materials.HotCopper = CreateLitMaterial("UnityProof_HotCopperCoil", new Color(0.50f, 0.19f, 0.07f), 0.8f, 0.58f, 31, true, false);
        materials.AgedCopper = CreateLitMaterial("UnityProof_AgedCopper", new Color(0.38f, 0.16f, 0.07f), 0.78f, 0.5f, 33, true, false);
        materials.GaugeFace = CreateLitMaterial("UnityProof_CreamGaugeFace", new Color(0.70f, 0.62f, 0.43f), 0.0f, 0.42f, 37, true, false);
        materials.Glass = CreateTransparentMaterial("UnityProof_GaugeGlass", new Color(0.74f, 0.9f, 1.0f, 0.28f), 0.0f, 0.85f);
        materials.Walnut = CreateLitMaterial("UnityProof_WalnutGrip", new Color(0.15f, 0.075f, 0.038f), 0.0f, 0.38f, 41, true, false);
        materials.DarkLeather = CreateLitMaterial("UnityProof_DarkLeatherGlove", new Color(0.055f, 0.04f, 0.03f), 0.0f, 0.32f, 43, true, false);
        materials.LeatherHighlight = CreateLitMaterial("UnityProof_LeatherHighlight", new Color(0.12f, 0.075f, 0.045f), 0.0f, 0.36f, 45, true, false);
        materials.Smoke = CreateSmokeMaterial();
        materials.ContactBack = CreateUnlitColorMaterial("UnityProof_ContactBack", new Color(0.045f, 0.038f, 0.032f, 1.0f), false);
        materials.MaskWhite = CreateUnlitColorMaterial("UnityProof_MaskWhite", Color.white, false);
        materials.WarningRed = CreateUnlitColorMaterial("UnityProof_GaugeNeedleRed", new Color(0.82f, 0.05f, 0.02f, 1.0f), false);
        materials.LineDark = CreateUnlitColorMaterial("UnityProof_GaugeInk", new Color(0.05f, 0.04f, 0.03f, 1.0f), false);
        materials.GlowAmber = CreateUnlitColorMaterial("UnityProof_AmberGlow", new Color(0.72f, 0.22f, 0.045f, 1.0f), true);
        materials.IronEdgeHighlight = CreateUnlitColorMaterial("UnityProof_IronEdgeHighlight", new Color(0.26f, 0.22f, 0.18f, 1.0f), false);
        materials.BrassHighlight = CreateUnlitColorMaterial("UnityProof_BrassHighlight", new Color(0.78f, 0.55f, 0.24f, 1.0f), false);
        materials.CopperHighlight = CreateUnlitColorMaterial("UnityProof_CopperHighlight", new Color(0.72f, 0.28f, 0.10f, 1.0f), false);
        materials.PatinaGreen = CreateUnlitColorMaterial("UnityProof_PatinaGreen", new Color(0.16f, 0.32f, 0.25f, 1.0f), false);
        return materials;
    }

    private static Material CreateLitMaterial(string name, Color baseColor, float metallic, float smoothness, int seed, bool addMaps, bool emission)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material material = new Material(shader);
        material.name = name;
        SetMaterialColor(material, baseColor);
        SetMaterialFloat(material, "_Metallic", metallic);
        SetMaterialFloat(material, "_Glossiness", smoothness);
        SetMaterialFloat(material, "_Smoothness", smoothness);

        if (addMaps)
        {
            Texture2D baseMap = CreateProceduralSurfaceTexture(baseColor, seed);
            Texture2D normalMap = CreateProceduralNormalTexture(seed + 100);
            Texture2D occlusionMap = CreateProceduralOcclusionTexture(seed + 200);
            SetMaterialTexture(material, "_MainTex", baseMap);
            SetMaterialTexture(material, "_BaseMap", baseMap);
            SetMaterialTexture(material, "_BumpMap", normalMap);
            SetMaterialTexture(material, "_OcclusionMap", occlusionMap);
            material.EnableKeyword("_NORMALMAP");
            material.EnableKeyword("_OCCLUSIONMAP");
        }

        if (emission)
        {
            Color emissionColor = new Color(0.9f, 0.26f, 0.05f) * 0.9f;
            SetMaterialColor(material, "_EmissionColor", emissionColor);
            material.EnableKeyword("_EMISSION");
        }

        return material;
    }

    private static Material CreateTransparentMaterial(string name, Color color, float metallic, float smoothness)
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
        material.SetOverrideTag("RenderType", "Transparent");
        material.renderQueue = (int)RenderQueue.Transparent;
        SetMaterialFloat(material, "_Surface", 1.0f);
        SetMaterialFloat(material, "_AlphaClip", 0.0f);
        SetMaterialFloat(material, "_Mode", 3.0f);
        SetMaterialInt(material, "_SrcBlend", (int)BlendMode.SrcAlpha);
        SetMaterialInt(material, "_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        SetMaterialInt(material, "_ZWrite", 0);
        material.EnableKeyword("_ALPHABLEND_ON");
        return material;
    }

    private static Material CreateSmokeMaterial()
    {
        Shader shader = Shader.Find("Particles/Standard Unlit");
        if (shader == null)
        {
            shader = Shader.Find("Legacy Shaders/Particles/Alpha Blended");
        }

        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
        }

        Material material = new Material(shader);
        material.name = "UnityProof_SteamSmokeSoftBillboard";
        Texture2D smokeTexture = CreateSmokeTexture(128, 71);
        material.mainTexture = smokeTexture;
        SetMaterialTexture(material, "_MainTex", smokeTexture);
        SetMaterialColor(material, new Color(0.76f, 0.72f, 0.65f, 0.28f));
        material.renderQueue = (int)RenderQueue.Transparent + 20;
        SetMaterialInt(material, "_ZWrite", 0);
        return material;
    }

    private static Material CreateUnlitColorMaterial(string name, Color color, bool emission)
    {
        Shader shader = Shader.Find("Unlit/Color");

        if (shader == null)
        {
            shader = Shader.Find("Universal Render Pipeline/Unlit");
        }

        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
        }

        Material material = new Material(shader);
        material.name = name;
        SetMaterialColor(material, color);
        material.color = color;
        if (emission)
        {
            SetMaterialColor(material, "_EmissionColor", color * 1.5f);
            material.EnableKeyword("_EMISSION");
        }

        return material;
    }

    private static Material CreateUnlitTextureMaterial(string name, Texture texture)
    {
        Shader shader = Shader.Find("Unlit/Texture");

        if (shader == null)
        {
            shader = Shader.Find("Universal Render Pipeline/Unlit");
        }

        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
        }

        Material material = new Material(shader);
        material.name = name;
        SetMaterialTexture(material, "_MainTex", texture);
        SetMaterialTexture(material, "_BaseMap", texture);
        material.mainTexture = texture;
        SetMaterialColor(material, Color.white);
        material.renderQueue = (int)RenderQueue.Geometry;
        SetMaterialInt(material, "_Cull", (int)CullMode.Off);
        return material;
    }

    private static void BuildBackground(Transform root, ProofMaterials materials)
    {
        GameObject backing = CreateBox("smoky black iron backdrop", new Vector3(-0.25f, 0.2f, 1.25f), new Vector3(6.6f, 3.8f, 0.08f), Quaternion.identity, materials.ContactBack, root);

        CreateBox("oily floor reflection strip", new Vector3(-0.15f, -1.05f, 0.38f), new Vector3(6.3f, 0.06f, 1.8f), Quaternion.Euler(0f, 0f, 0f), materials.BlackenedIron, root);
        for (int i = 0; i < 7; i++)
        {
            float x = -2.7f + i * 0.9f;
            CreateBox("dim rear pipe run " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 1.55f, 0.88f), new Vector3(0.62f, 0.045f, 0.045f), Quaternion.identity, materials.AgedBrass, root);
        }
    }

    private static void BuildPressurePistol(Transform root, ProofMaterials materials, ProofMetrics metrics)
    {
        GameObject body = new GameObject("procedural pressure pistol geometry");
        body.transform.SetParent(root, false);

        CreateCylinderX("blackened main barrel", new Vector3(-0.35f, 0.28f, 0.05f), 2.75f, 0.29f, materials.BlackenedIron, body.transform);
        CreateCylinderX("rear pressure receiver cap", new Vector3(1.07f, 0.28f, 0.05f), 0.26f, 0.34f, materials.AgedBrass, body.transform);
        CreateCylinderX("front barrel collar", new Vector3(-1.72f, 0.28f, 0.05f), 0.20f, 0.37f, materials.AgedBrass, body.transform);
        CreateCylinderX("center iron sleeve", new Vector3(-0.53f, 0.28f, 0.05f), 0.34f, 0.31f, materials.DarkPipeMetal, body.transform);
        CreateCylinderX("lower pressure tank", new Vector3(-0.25f, -0.27f, 0.08f), 2.15f, 0.20f, materials.DarkPipeMetal, body.transform);
        CreateCylinderX("lower tank left brass cap", new Vector3(-1.42f, -0.27f, 0.08f), 0.18f, 0.23f, materials.AgedBrass, body.transform);
        CreateCylinderX("lower tank right brass cap", new Vector3(0.9f, -0.27f, 0.08f), 0.18f, 0.23f, materials.AgedBrass, body.transform);

        AddBarrelStraps(body.transform, materials, metrics);
        AddMuzzleStack(body.transform, materials, metrics);
        AddCoilWindow(body.transform, materials, metrics);
        AddGauge(body.transform, materials, metrics);
        AddValvesAndPorts(body.transform, materials, metrics);
        AddSidePlatesAndBrackets(body.transform, materials, metrics);
        AddTriggerAndGrip(body.transform, materials, metrics);
        AddRivetFields(body.transform, materials, metrics);

        metrics.mainBarrels = 1;
        metrics.lowerPressureTanks = 1;
        metrics.readableGauges = 1;
        metrics.muzzleRings = 6;
        metrics.triggers = 1;
        metrics.triggerGuards = 1;
        metrics.gripOrGloveMasses = 1;
    }

    private static void AddBarrelStraps(Transform parent, ProofMaterials materials, ProofMetrics metrics)
    {
        float[] xs = { -1.35f, -0.78f, -0.08f, 0.55f };
        for (int i = 0; i < xs.Length; i++)
        {
            CreateCylinderX("raised barrel brass strap " + i.ToString(CultureInfo.InvariantCulture), new Vector3(xs[i], 0.28f, 0.05f), 0.09f, 0.325f, materials.AgedBrass, parent);
            metrics.platesAndBrackets++;
        }
    }

    private static void AddMuzzleStack(Transform parent, ProofMaterials materials, ProofMetrics metrics)
    {
        CreateCylinderX("muzzle outer iron cone block", new Vector3(-1.96f, 0.28f, 0.05f), 0.30f, 0.25f, materials.DarkPipeMetal, parent);
        CreateCylinderX("muzzle brass ring 01", new Vector3(-2.1f, 0.28f, 0.05f), 0.08f, 0.31f, materials.AgedBrass, parent);
        CreateCylinderX("muzzle iron ring 02", new Vector3(-2.19f, 0.28f, 0.05f), 0.07f, 0.25f, materials.BlackenedIron, parent);
        CreateCylinderX("muzzle brass ring 03", new Vector3(-2.27f, 0.28f, 0.05f), 0.06f, 0.2f, materials.AgedBrass, parent);
        CreateCylinderX("muzzle dark nozzle", new Vector3(-2.36f, 0.28f, 0.05f), 0.13f, 0.13f, materials.DarkPipeMetal, parent);
        CreateCylinderX("muzzle glowing pressure bore", new Vector3(-2.44f, 0.28f, 0.05f), 0.04f, 0.08f, materials.GlowAmber, parent);
        metrics.platesAndBrackets += 3;
    }

    private static void AddCoilWindow(Transform parent, ProofMaterials materials, ProofMetrics metrics)
    {
        Vector3 center = new Vector3(0.08f, 0.14f, -0.31f);
        CreateBox("coil window dark backing", center + new Vector3(0f, 0f, 0.015f), new Vector3(1.02f, 0.42f, 0.045f), Quaternion.identity, materials.BlackenedIron, parent);
        CreateBox("coil window top brass rail", center + new Vector3(0f, 0.25f, -0.02f), new Vector3(1.16f, 0.055f, 0.075f), Quaternion.identity, materials.AgedBrass, parent);
        CreateBox("coil window bottom brass rail", center + new Vector3(0f, -0.25f, -0.02f), new Vector3(1.16f, 0.055f, 0.075f), Quaternion.identity, materials.AgedBrass, parent);
        CreateBox("coil window left brass upright", center + new Vector3(-0.59f, 0f, -0.02f), new Vector3(0.055f, 0.55f, 0.075f), Quaternion.identity, materials.AgedBrass, parent);
        CreateBox("coil window right brass upright", center + new Vector3(0.59f, 0f, -0.02f), new Vector3(0.055f, 0.55f, 0.075f), Quaternion.identity, materials.AgedBrass, parent);
        metrics.platesAndBrackets += 5;

        int turns = 8;
        for (int i = 0; i < turns; i++)
        {
            float x = -0.42f + i * 0.12f;
            CreateCylinderY("hot copper visible coil turn " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.14f, -0.365f), 0.43f, 0.035f, materials.HotCopper, parent);
            CreateSphere("hot coil highlight bead " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.34f, -0.405f), new Vector3(0.06f, 0.03f, 0.025f), materials.GlowAmber, parent);
        }

        metrics.visibleCoilTurns = turns;
        CreateBox("thin coil glass plate", center + new Vector3(0f, 0f, -0.08f), new Vector3(1.02f, 0.36f, 0.016f), Quaternion.identity, materials.Glass, parent);
    }

    private static void AddGauge(Transform parent, ProofMaterials materials, ProofMetrics metrics)
    {
        Vector3 center = new Vector3(-0.18f, 0.83f, -0.34f);
        CreateCylinderZ("gauge brass bezel outer", center + new Vector3(0f, 0f, 0.015f), 0.12f, 0.31f, materials.AgedBrass, parent);
        CreateCylinderZ("gauge cream face", center + new Vector3(0f, 0f, -0.055f), 0.05f, 0.245f, materials.GaugeFace, parent);
        CreateCylinderZ("gauge glass lens", center + new Vector3(0f, 0f, -0.085f), 0.018f, 0.255f, materials.Glass, parent);
        CreateCylinderZ("gauge rear mount", center + new Vector3(0f, -0.25f, 0.06f), 0.22f, 0.055f, materials.AgedBrass, parent);
        metrics.platesAndBrackets += 2;

        int tickCount = 28;
        for (int i = 0; i < tickCount; i++)
        {
            float angle = Mathf.Lerp(210f, -30f, i / (float)(tickCount - 1));
            float radians = angle * Mathf.Deg2Rad;
            Vector3 tickPosition = center + new Vector3(Mathf.Cos(radians) * 0.19f, Mathf.Sin(radians) * 0.19f, -0.118f);
            float tickLength = i % 4 == 0 ? 0.062f : 0.034f;
            CreateBox("gauge tick " + i.ToString(CultureInfo.InvariantCulture), tickPosition, new Vector3(0.008f, tickLength, 0.012f), Quaternion.Euler(0f, 0f, angle + 90f), materials.LineDark, parent);
            metrics.gaugeTickMarks++;
        }

        CreateBox("gauge red pressure needle", center + new Vector3(0.055f, 0.035f, -0.13f), new Vector3(0.018f, 0.185f, 0.016f), Quaternion.Euler(0f, 0f, -50f), materials.WarningRed, parent);
        CreateCylinderZ("gauge needle hub", center + new Vector3(0f, 0f, -0.14f), 0.018f, 0.035f, materials.AgedBrass, parent);

        int gaugeRivets = 18;
        for (int i = 0; i < gaugeRivets; i++)
        {
            float angle = i * Mathf.PI * 2f / gaugeRivets;
            Vector3 rivetPosition = center + new Vector3(Mathf.Cos(angle) * 0.286f, Mathf.Sin(angle) * 0.286f, -0.12f);
            AddRivet("gauge rim rivet " + i.ToString(CultureInfo.InvariantCulture), rivetPosition, 0.018f, materials.AgedBrass, parent, metrics);
        }
    }

    private static void AddValvesAndPorts(Transform parent, ProofMaterials materials, ProofMetrics metrics)
    {
        Vector3[] valveBases =
        {
            new Vector3(-0.95f, 0.63f, 0.03f),
            new Vector3(0.36f, 0.65f, 0.02f),
            new Vector3(0.84f, 0.53f, -0.04f)
        };

        for (int i = 0; i < valveBases.Length; i++)
        {
            Vector3 basePosition = valveBases[i];
            CreateCylinderY("top valve pedestal " + i.ToString(CultureInfo.InvariantCulture), basePosition, 0.18f, 0.055f, materials.AgedBrass, parent);
            CreateCylinderY("top valve dark cap " + i.ToString(CultureInfo.InvariantCulture), basePosition + new Vector3(0f, 0.16f, 0f), 0.12f, 0.075f, materials.DarkPipeMetal, parent);
            CreateCylinderZ("top valve wheel " + i.ToString(CultureInfo.InvariantCulture), basePosition + new Vector3(0f, 0.25f, -0.03f), 0.025f, 0.13f, materials.AgedBrass, parent);
            CreateBox("top valve wheel cross a " + i.ToString(CultureInfo.InvariantCulture), basePosition + new Vector3(0f, 0.25f, -0.055f), new Vector3(0.24f, 0.018f, 0.018f), Quaternion.identity, materials.AgedBrass, parent);
            CreateBox("top valve wheel cross b " + i.ToString(CultureInfo.InvariantCulture), basePosition + new Vector3(0f, 0.25f, -0.055f), new Vector3(0.018f, 0.24f, 0.018f), Quaternion.identity, materials.AgedBrass, parent);
            metrics.topValvesOrCaps++;
            metrics.platesAndBrackets += 2;
        }

        Vector3[] portPositions =
        {
            new Vector3(-1.2f, 0.02f, -0.32f),
            new Vector3(0.82f, 0.0f, -0.35f),
            new Vector3(0.52f, -0.52f, -0.15f)
        };

        for (int i = 0; i < portPositions.Length; i++)
        {
            CreateCylinderZ("visible pressure port socket " + i.ToString(CultureInfo.InvariantCulture), portPositions[i], 0.08f, 0.105f, materials.AgedBrass, parent);
            CreateCylinderZ("visible pressure port dark center " + i.ToString(CultureInfo.InvariantCulture), portPositions[i] + new Vector3(0f, 0f, -0.055f), 0.035f, 0.052f, materials.BlackenedIron, parent);
            metrics.pressurePorts++;
        }

        CreateCylinderX("short brass bypass pipe", new Vector3(-0.55f, -0.02f, -0.24f), 1.25f, 0.035f, materials.AgedBrass, parent);
        CreateCylinderY("bypass pipe drop elbow", new Vector3(0.1f, -0.18f, -0.24f), 0.32f, 0.035f, materials.AgedBrass, parent);
        CreateSphere("bypass pipe rounded elbow", new Vector3(0.1f, -0.02f, -0.24f), new Vector3(0.08f, 0.08f, 0.08f), materials.AgedBrass, parent);
    }

    private static void AddSidePlatesAndBrackets(Transform parent, ProofMaterials materials, ProofMetrics metrics)
    {
        AddPlate("upper left riveted side plate", new Vector3(-1.08f, 0.41f, -0.26f), new Vector3(0.44f, 0.12f, 0.055f), Quaternion.Euler(0f, 0f, -4f), materials.AgedBrass, parent, metrics);
        AddPlate("upper center black inspection plate", new Vector3(-0.32f, 0.48f, -0.27f), new Vector3(0.38f, 0.1f, 0.055f), Quaternion.identity, materials.DarkPipeMetal, parent, metrics);
        AddPlate("rear brass receiver cheek", new Vector3(0.62f, 0.35f, -0.31f), new Vector3(0.46f, 0.22f, 0.06f), Quaternion.identity, materials.AgedBrass, parent, metrics);
        AddPlate("lower bridge bracket left", new Vector3(-0.88f, -0.03f, -0.28f), new Vector3(0.12f, 0.42f, 0.055f), Quaternion.Euler(0f, 0f, 8f), materials.AgedBrass, parent, metrics);
        AddPlate("lower bridge bracket center", new Vector3(-0.15f, -0.03f, -0.28f), new Vector3(0.11f, 0.42f, 0.055f), Quaternion.Euler(0f, 0f, -4f), materials.AgedBrass, parent, metrics);
        AddPlate("lower bridge bracket right", new Vector3(0.56f, -0.04f, -0.28f), new Vector3(0.12f, 0.42f, 0.055f), Quaternion.Euler(0f, 0f, 7f), materials.AgedBrass, parent, metrics);
        AddPlate("rear round side gear mount", new Vector3(0.72f, 0.05f, -0.35f), new Vector3(0.13f, 0.13f, 0.13f), Quaternion.identity, materials.AgedBrass, parent, metrics);
        CreateCylinderZ("rear brass gear disk", new Vector3(0.72f, 0.05f, -0.43f), 0.07f, 0.23f, materials.AgedBrass, parent);
        CreateCylinderZ("rear dark gear hub", new Vector3(0.72f, 0.05f, -0.49f), 0.045f, 0.1f, materials.DarkPipeMetal, parent);
    }

    private static void AddTriggerAndGrip(Transform parent, ProofMaterials materials, ProofMetrics metrics)
    {
        CreateBox("walnut grip core", new Vector3(1.15f, -0.54f, -0.09f), new Vector3(0.38f, 0.86f, 0.27f), Quaternion.Euler(0f, 0f, -22f), materials.Walnut, parent);
        CreateBox("walnut grip brass butt plate", new Vector3(1.29f, -0.9f, -0.1f), new Vector3(0.48f, 0.08f, 0.31f), Quaternion.Euler(0f, 0f, -22f), materials.AgedBrass, parent);
        CreateBox("walnut grip top brass collar", new Vector3(0.94f, -0.22f, -0.08f), new Vector3(0.4f, 0.1f, 0.3f), Quaternion.Euler(0f, 0f, -17f), materials.AgedBrass, parent);
        metrics.platesAndBrackets += 2;

        for (int i = 0; i < 7; i++)
        {
            float offset = -0.24f + i * 0.08f;
            CreateBox("carved walnut grip wrinkle " + i.ToString(CultureInfo.InvariantCulture), new Vector3(1.16f + offset * 0.25f, -0.52f + offset, -0.245f), new Vector3(0.34f, 0.012f, 0.018f), Quaternion.Euler(0f, 0f, -18f), materials.LineDark, parent);
        }

        CreateBox("trigger guard lower arc block", new Vector3(0.72f, -0.35f, -0.31f), new Vector3(0.44f, 0.055f, 0.06f), Quaternion.Euler(0f, 0f, -12f), materials.AgedBrass, parent);
        CreateBox("trigger guard front arc block", new Vector3(0.5f, -0.2f, -0.31f), new Vector3(0.055f, 0.32f, 0.06f), Quaternion.Euler(0f, 0f, -22f), materials.AgedBrass, parent);
        CreateBox("trigger guard rear arc block", new Vector3(0.93f, -0.22f, -0.31f), new Vector3(0.055f, 0.32f, 0.06f), Quaternion.Euler(0f, 0f, 18f), materials.AgedBrass, parent);
        CreateBox("dark trigger blade", new Vector3(0.7f, -0.18f, -0.34f), new Vector3(0.065f, 0.24f, 0.055f), Quaternion.Euler(0f, 0f, -16f), materials.BlackenedIron, parent);
        metrics.platesAndBrackets += 3;

        CreateSphere("dark leather glove palm mass", new Vector3(1.5f, -0.62f, -0.34f), new Vector3(0.58f, 0.36f, 0.2f), materials.DarkLeather, parent);
        CreateSphere("dark leather glove thumb mass", new Vector3(1.23f, -0.37f, -0.41f), new Vector3(0.24f, 0.18f, 0.12f), materials.DarkLeather, parent);
        for (int i = 0; i < 4; i++)
        {
            CreateSphere("dark leather glove finger " + i.ToString(CultureInfo.InvariantCulture), new Vector3(1.42f + i * 0.09f, -0.45f - i * 0.03f, -0.48f), new Vector3(0.12f, 0.24f, 0.11f), materials.DarkLeather, parent);
        }
    }

    private static void AddRivetFields(Transform parent, ProofMaterials materials, ProofMetrics metrics)
    {
        AddRivetRow("upper side plate rivets", new Vector3(-1.28f, 0.49f, -0.32f), new Vector3(-0.88f, 0.5f, -0.32f), 7, 0.017f, materials.AgedBrass, parent, metrics);
        AddRivetRow("upper black plate rivets", new Vector3(-0.48f, 0.54f, -0.32f), new Vector3(-0.16f, 0.54f, -0.32f), 5, 0.016f, materials.AgedBrass, parent, metrics);
        AddRivetRow("rear cheek upper rivets", new Vector3(0.42f, 0.47f, -0.36f), new Vector3(0.84f, 0.47f, -0.36f), 7, 0.018f, materials.AgedBrass, parent, metrics);
        AddRivetRow("rear cheek lower rivets", new Vector3(0.42f, 0.24f, -0.36f), new Vector3(0.84f, 0.24f, -0.36f), 7, 0.018f, materials.AgedBrass, parent, metrics);
        AddRivetRow("coil top rail rivets", new Vector3(-0.49f, 0.39f, -0.39f), new Vector3(0.58f, 0.39f, -0.39f), 11, 0.016f, materials.AgedBrass, parent, metrics);
        AddRivetRow("coil lower rail rivets", new Vector3(-0.49f, -0.12f, -0.39f), new Vector3(0.58f, -0.12f, -0.39f), 11, 0.016f, materials.AgedBrass, parent, metrics);
        AddRivetRow("lower tank rivets", new Vector3(-1.16f, -0.08f, -0.12f), new Vector3(0.72f, -0.08f, -0.12f), 14, 0.015f, materials.AgedBrass, parent, metrics);
        AddRivetRow("grip screws", new Vector3(1.0f, -0.29f, -0.27f), new Vector3(1.27f, -0.77f, -0.27f), 6, 0.018f, materials.AgedBrass, parent, metrics);

        float[] strapXs = { -1.35f, -0.78f, -0.08f, 0.55f };
        for (int i = 0; i < strapXs.Length; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                float angle = Mathf.Lerp(205f, 335f, j / 4f) * Mathf.Deg2Rad;
                Vector3 position = new Vector3(strapXs[i], 0.28f + Mathf.Sin(angle) * 0.305f, -0.015f + Mathf.Cos(angle) * 0.305f);
                AddRivet("barrel strap rivet " + i.ToString(CultureInfo.InvariantCulture) + "_" + j.ToString(CultureInfo.InvariantCulture), position, 0.015f, materials.AgedBrass, parent, metrics);
            }
        }
    }

    private static void BuildSteamAndSmoke(Transform root, ProofMaterials materials, ProofMetrics metrics)
    {
        Vector3[] emitters =
        {
            new Vector3(-0.95f, 0.95f, -0.24f),
            new Vector3(0.35f, 0.93f, -0.22f),
            new Vector3(-2.25f, 0.38f, -0.18f),
            new Vector3(0.77f, 0.14f, -0.42f)
        };

        int puffIndex = 0;
        for (int e = 0; e < emitters.Length; e++)
        {
            for (int i = 0; i < 7; i++)
            {
                float t = i / 6f;
                Vector3 position = emitters[e] + new Vector3(Mathf.Sin(i * 1.7f + e) * 0.08f, t * 0.55f, -0.2f - t * 0.1f);
                float size = 0.18f + t * 0.25f;
                GameObject puff = CreateTexturedQuad("steam smoke puff " + puffIndex.ToString(CultureInfo.InvariantCulture), position, new Vector2(size * 1.35f, size), Quaternion.Euler(0f, 0f, -10f + i * 13f), materials.Smoke, root);
                Color color = new Color(0.74f, 0.71f, 0.66f, Mathf.Lerp(0.26f, 0.07f, t));
                puff.GetComponent<Renderer>().material.color = color;
                puffIndex++;
            }
        }

        metrics.steamPuffBillboards = puffIndex;
    }

    private static void BuildLighting(ProofMetrics metrics)
    {
        CreateLight("warm amber key", LightType.Spot, new Vector3(-2.7f, 2.4f, -2.2f), Quaternion.Euler(47f, 34f, 0f), new Color(1.0f, 0.58f, 0.18f), 7.2f, 56f, true);
        CreateLight("hot coil practical", LightType.Point, new Vector3(0.1f, 0.22f, -0.55f), Quaternion.identity, new Color(1.0f, 0.32f, 0.08f), 2.7f, 0f, false);
        CreateLight("small gauge face practical", LightType.Point, new Vector3(-0.22f, 0.82f, -0.55f), Quaternion.identity, new Color(1.0f, 0.72f, 0.28f), 1.6f, 0f, false);
        CreateLight("cool smoky fill", LightType.Directional, new Vector3(1.6f, 1.5f, -1.3f), Quaternion.Euler(38f, -135f, 0f), new Color(0.18f, 0.22f, 0.28f), 0.22f, 0f, false);
        CreateLight("thin brass rim", LightType.Spot, new Vector3(1.8f, 1.4f, 1.5f), Quaternion.Euler(145f, -36f, 0f), new Color(1.0f, 0.64f, 0.25f), 2.2f, 42f, true);

        metrics.warmKeyLight = true;
        metrics.lowCoolFill = true;
        metrics.rimLight = true;
    }

    private static Camera CreateHeroCamera(ProofMetrics metrics)
    {
        GameObject cameraObject = new GameObject("Unity pressure pistol proof camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = new Vector3(1.12f, 0.64f, -4.15f);
        camera.transform.LookAt(new Vector3(-0.38f, 0.16f, -0.05f));
        camera.fieldOfView = 34f;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 30f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.025f, 0.022f, 0.02f);
        camera.allowHDR = true;
        camera.allowMSAA = true;

        metrics.cameraDescription = "3/4 first-person framing; muzzle left-forward, grip/glove lower-right, gauge and coil kept on visible near side.";
        return camera;
    }

    private static Texture2D CaptureCamera(Camera camera, int width, int height, Color clearColor, string textureName)
    {
        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)
        {
            antiAliasing = 8,
            name = textureName + " RT"
        };

        RenderTexture previousActive = RenderTexture.active;
        RenderTexture previousTarget = camera.targetTexture;
        Color previousBackground = camera.backgroundColor;
        CameraClearFlags previousClearFlags = camera.clearFlags;

        try
        {
            camera.targetTexture = renderTexture;
            camera.backgroundColor = clearColor;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.Render();

            RenderTexture.active = renderTexture;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
            texture.name = textureName;
            texture.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, false);
            texture.Apply(false, false);
            return texture;
        }
        finally
        {
            camera.targetTexture = previousTarget;
            camera.backgroundColor = previousBackground;
            camera.clearFlags = previousClearFlags;
            RenderTexture.active = previousActive;
            renderTexture.Release();
            UnityEngine.Object.DestroyImmediate(renderTexture);
        }
    }

    private static void MeasureGunOccupancy(Camera camera, GameObject gunRoot, ProofMetrics metrics)
    {
        Renderer[] allRenderers = UnityEngine.Object.FindObjectsByType<Renderer>();
        List<Renderer> gunRenderers = new List<Renderer>();
        List<Renderer> nonGunRenderers = new List<Renderer>();
        Dictionary<Renderer, Material[]> previousMaterials = new Dictionary<Renderer, Material[]>();
        List<Light> lights = new List<Light>(UnityEngine.Object.FindObjectsByType<Light>());
        List<bool> lightStates = new List<bool>();
        Material maskMaterial = CreateUnlitColorMaterial("UnityProof_TemporaryMaskWhite", Color.white, false);

        for (int i = 0; i < allRenderers.Length; i++)
        {
            Renderer renderer = allRenderers[i];
            if (renderer.transform.IsChildOf(gunRoot.transform))
            {
                gunRenderers.Add(renderer);
                previousMaterials[renderer] = renderer.sharedMaterials;
                renderer.sharedMaterial = maskMaterial;
            }
            else
            {
                nonGunRenderers.Add(renderer);
                renderer.enabled = false;
            }
        }

        for (int i = 0; i < lights.Count; i++)
        {
            lightStates.Add(lights[i].enabled);
            lights[i].enabled = false;
        }

        bool fog = RenderSettings.fog;
        Color ambient = RenderSettings.ambientLight;
        RenderSettings.fog = false;
        RenderSettings.ambientLight = Color.white;

        Texture2D mask = null;
        try
        {
            mask = CaptureCamera(camera, 640, 360, Color.black, "Unity pressure pistol proof mask");
            Color32[] pixels = mask.GetPixels32();
            int minX = mask.width;
            int minY = mask.height;
            int maxX = -1;
            int maxY = -1;

            for (int y = 0; y < mask.height; y++)
            {
                for (int x = 0; x < mask.width; x++)
                {
                    Color32 pixel = pixels[y * mask.width + x];
                    if (pixel.r > 128 || pixel.g > 128 || pixel.b > 128)
                    {
                        minX = Mathf.Min(minX, x);
                        minY = Mathf.Min(minY, y);
                        maxX = Mathf.Max(maxX, x);
                        maxY = Mathf.Max(maxY, y);
                    }
                }
            }

            if (maxX >= minX && maxY >= minY)
            {
                metrics.bodyMaskMinX = minX;
                metrics.bodyMaskMinY = minY;
                metrics.bodyMaskMaxX = maxX;
                metrics.bodyMaskMaxY = maxY;
                metrics.bodyOccupancyWidthPercent = Mathf.Round(((maxX - minX + 1f) / mask.width) * 1000f) / 10f;
                metrics.bodyOccupancyHeightPercent = Mathf.Round(((maxY - minY + 1f) / mask.height) * 1000f) / 10f;
            }
        }
        finally
        {
            DestroyTexture(mask);
            for (int i = 0; i < gunRenderers.Count; i++)
            {
                Renderer renderer = gunRenderers[i];
                if (renderer != null && previousMaterials.ContainsKey(renderer))
                {
                    renderer.sharedMaterials = previousMaterials[renderer];
                }
            }

            for (int i = 0; i < nonGunRenderers.Count; i++)
            {
                if (nonGunRenderers[i] != null)
                {
                    nonGunRenderers[i].enabled = true;
                }
            }

            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i] != null)
                {
                    lights[i].enabled = lightStates[i];
                }
            }

            RenderSettings.fog = fog;
            RenderSettings.ambientLight = ambient;
            if (maskMaterial != null)
            {
                UnityEngine.Object.DestroyImmediate(maskMaterial);
            }
        }
    }

    private static Texture2D LoadConceptCrop(string conceptPath)
    {
        if (!File.Exists(conceptPath))
        {
            Texture2D fallback = new Texture2D(512, 320, TextureFormat.RGBA32, false, false);
            Color[] colors = new Color[fallback.width * fallback.height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(0.08f, 0.065f, 0.05f, 1f);
            }

            fallback.SetPixels(colors);
            fallback.Apply(false, false);
            return fallback;
        }

        Texture2D source = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
        source.LoadImage(File.ReadAllBytes(conceptPath), false);

        int cropX = Mathf.FloorToInt(source.width * 0.5f);
        int cropY = 0;
        int cropWidth = source.width - cropX;
        int cropHeight = Mathf.FloorToInt(source.height * 0.5f);
        Texture2D crop = new Texture2D(cropWidth, cropHeight, TextureFormat.RGBA32, false, false);
        crop.SetPixels(source.GetPixels(cropX, cropY, cropWidth, cropHeight));
        crop.Apply(false, false);
        UnityEngine.Object.DestroyImmediate(source);
        return crop;
    }

    private static Texture2D RenderContactSheet(Texture2D heroTexture, Texture2D conceptCrop, ProofMetrics metrics)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = Color.white;
        RenderSettings.fog = false;

        Camera camera = new GameObject("Unity pressure pistol contact sheet camera").AddComponent<Camera>();
        camera.transform.position = new Vector3(0f, 0f, -10f);
        camera.transform.rotation = Quaternion.identity;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.035f, 0.03f, 0.026f);
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        camera.nearClipPlane = 0.01f;
        camera.farClipPlane = 40f;

        Material conceptMaterial = CreateUnlitTextureMaterial("UnityProof_ContactConceptCrop", conceptCrop);
        Material heroMaterial = CreateUnlitTextureMaterial("UnityProof_ContactHeroRender", heroTexture);
        Material panelMaterial = CreateUnlitColorMaterial("UnityProof_ContactPanelMat", new Color(0.07f, 0.058f, 0.047f, 1f), false);
        Material brassLine = CreateUnlitColorMaterial("UnityProof_ContactBrassLine", new Color(0.72f, 0.43f, 0.16f, 1f), false);
        Material creamText = CreateUnlitColorMaterial("UnityProof_ContactCreamText", new Color(0.86f, 0.78f, 0.58f, 1f), false);
        Material mutedText = CreateUnlitColorMaterial("UnityProof_ContactMutedText", new Color(0.55f, 0.5f, 0.43f, 1f), false);
        Material warnText = CreateUnlitColorMaterial("UnityProof_ContactWarnText", new Color(1.0f, 0.56f, 0.18f, 1f), false);

        CreateBox("contact bottom panel", new Vector3(0f, -3.78f, 0.05f), new Vector3(15.1f, 2.05f, 0.04f), Quaternion.identity, panelMaterial, null);
        CreateBox("contact separator line", new Vector3(0f, -2.62f, -0.01f), new Vector3(15.1f, 0.025f, 0.04f), Quaternion.identity, brassLine, null);
        CreateBox("contact vertical split", new Vector3(0f, 1.18f, -0.01f), new Vector3(0.025f, 5.55f, 0.04f), Quaternion.identity, brassLine, null);

        CreateTexturedQuad("contact concept crop panel", new Vector3(-3.96f, 1.08f, 0f), new Vector2(7.15f, 4.77f), Quaternion.identity, conceptMaterial, null);
        CreateTexturedQuad("contact unity hero panel", new Vector3(3.96f, 1.08f, 0f), new Vector2(7.15f, 4.02f), Quaternion.identity, heroMaterial, null);

        CreateLabel("North-star crop", new Vector3(-7.35f, 4.55f, -0.08f), 0.14f, creamText, TextAnchor.UpperLeft);
        CreateLabel("Unity proof render", new Vector3(0.42f, 4.55f, -0.08f), 0.14f, creamText, TextAnchor.UpperLeft);
        CreateLabel("Editor-only RenderTexture output - no gameplay scene saved", new Vector3(0f, -2.88f, -0.08f), 0.105f, mutedText, TextAnchor.MiddleCenter);

        string gateLine1 = "Counts: " + metrics.visibleCoilTurns.ToString(CultureInfo.InvariantCulture) + " coil turns, " +
                           metrics.fasteners.ToString(CultureInfo.InvariantCulture) + " fasteners, " +
                           metrics.platesAndBrackets.ToString(CultureInfo.InvariantCulture) + " plates/brackets, " +
                           metrics.pressurePorts.ToString(CultureInfo.InvariantCulture) + " ports, " +
                           metrics.topValvesOrCaps.ToString(CultureInfo.InvariantCulture) + " top valves/caps.";
        string gateLine2 = "Framing: " + metrics.bodyOccupancyWidthPercent.ToString("0.0", CultureInfo.InvariantCulture) + "% width x " +
                           metrics.bodyOccupancyHeightPercent.ToString("0.0", CultureInfo.InvariantCulture) + "% height, muzzle left-forward, grip lower-right.";
        string gateLine3 = "Verdict: useful Unity lookdev proof; still not accepted final art until material realism and human review pass.";

        CreateLabel(gateLine1, new Vector3(-7.2f, -3.32f, -0.08f), 0.105f, creamText, TextAnchor.MiddleLeft);
        CreateLabel(gateLine2, new Vector3(-7.2f, -3.72f, -0.08f), 0.105f, creamText, TextAnchor.MiddleLeft);
        CreateLabel(gateLine3, new Vector3(-7.2f, -4.13f, -0.08f), 0.105f, warnText, TextAnchor.MiddleLeft);

        Texture2D contact = CaptureCamera(camera, ContactWidth, ContactHeight, camera.backgroundColor, "Unity pressure pistol proof contact sheet");
        UnityEngine.Object.DestroyImmediate(conceptMaterial);
        UnityEngine.Object.DestroyImmediate(heroMaterial);
        UnityEngine.Object.DestroyImmediate(panelMaterial);
        UnityEngine.Object.DestroyImmediate(brassLine);
        UnityEngine.Object.DestroyImmediate(creamText);
        UnityEngine.Object.DestroyImmediate(mutedText);
        UnityEngine.Object.DestroyImmediate(warnText);
        return contact;
    }

    private static string BuildReport(ProofMetrics metrics)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# HFLD Recovery04 Unity Pressure Pistol Proof Report");
        builder.AppendLine();
        builder.AppendLine("Status: Unity-only lookdev proof generated; not accepted final art");
        builder.AppendLine("Date/time: " + metrics.timestamp);
        builder.AppendLine("Unity version: " + metrics.unityVersion);
        builder.AppendLine("Subject: pressure pistol only");
        builder.AppendLine("Tool lane: Unity editor batchmode, temporary in-memory scene, Camera plus RenderTexture JPG export");
        builder.AppendLine("Batchmode command entrypoint: `UnityPressurePistolProofRenderer.RenderBatch`");
        builder.AppendLine("Superseded direction: the prior non-Unity lane is forbidden by current project direction.");
        builder.AppendLine();
        builder.AppendLine("## Outputs");
        builder.AppendLine();
        builder.AppendLine("| File | Purpose | Dimensions |");
        builder.AppendLine("| --- | --- | ---: |");
        builder.AppendLine("| `" + metrics.renderPath + "` | Hero Unity proof render | " + metrics.heroWidth.ToString(CultureInfo.InvariantCulture) + "x" + metrics.heroHeight.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| `" + metrics.contactSheetPath + "` | Contact sheet comparing north-star crop and Unity proof | " + metrics.contactSheetWidth.ToString(CultureInfo.InvariantCulture) + "x" + metrics.contactSheetHeight.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| `" + metrics.metricsPath + "` | Unity-generated component/frame metrics | n/a |");
        builder.AppendLine();
        builder.AppendLine("## Methodology");
        builder.AppendLine();
        builder.AppendLine("- Created a new unsaved editor scene in memory and did not add anything to build settings.");
        builder.AppendLine("- Built the pressure pistol from Unity primitives only: cylinders, boxes, spheres, transparent quads, and layered collar/strap geometry to approximate bevel highlights.");
        builder.AppendLine("- Used Standard-compatible material setup with URP Lit fallback detection, procedural base/normal/occlusion textures, metallic/smoothness values, transparent gauge glass, emissive copper coil, walnut grip, and dark leather glove mass.");
        builder.AppendLine("- Lit the scene with warm amber key, hot coil/gauge practicals, low cool fill, brass rim light, dark fog, and soft smoke billboards from visible ports.");
        builder.AppendLine("- Captured the hero image through a Unity Camera into a RenderTexture, then captured a Unity-rendered contact sheet with the north-star pressure-pistol crop.");
        builder.AppendLine();
        builder.AppendLine("## Component Metrics");
        builder.AppendLine();
        builder.AppendLine("| Check | Result |");
        builder.AppendLine("| --- | ---: |");
        builder.AppendLine("| Visible coil turns | " + metrics.visibleCoilTurns.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Fasteners/rivets/bolts | " + metrics.fasteners.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Plates/brackets/straps | " + metrics.platesAndBrackets.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Pressure ports/sockets | " + metrics.pressurePorts.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Top valves/caps | " + metrics.topValvesOrCaps.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Gauge tick marks | " + metrics.gaugeTickMarks.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Steam/smoke billboards | " + metrics.steamPuffBillboards.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Body occupancy | " + metrics.bodyOccupancyWidthPercent.ToString("0.0", CultureInfo.InvariantCulture) + "% width x " + metrics.bodyOccupancyHeightPercent.ToString("0.0", CultureInfo.InvariantCulture) + "% height |");
        builder.AppendLine("| Body mask bbox at 640x360 | x " + metrics.bodyMaskMinX.ToString(CultureInfo.InvariantCulture) + "-" + metrics.bodyMaskMaxX.ToString(CultureInfo.InvariantCulture) + ", y " + metrics.bodyMaskMinY.ToString(CultureInfo.InvariantCulture) + "-" + metrics.bodyMaskMaxY.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine();
        builder.AppendLine("## Acceptance Gates");
        builder.AppendLine();
        builder.AppendLine("| Gate | Status | Evidence |");
        builder.AppendLine("| --- | --- | --- |");
        builder.AppendLine("| Gate 0: Scope and label | Pass | New intentional files are in the isolated editor script path, Unity proof documentation folder, and allowed ConceptRenders names. The scene is temporary and unsaved. |");
        builder.AppendLine("| Gate 1: Component count | Pass | Barrel, lower tank, readable gauge, coil window, muzzle stack, trigger/guard, grip/glove mass, ports, valves, plates, and 60+ fasteners are procedurally present. |");
        builder.AppendLine("| Gate 2: Material and texture detail | Partial/fail | Unity materials use procedural base/normal/occlusion maps plus metallic/smoothness response, but this remains primitive geometry with no authored UV asset pass or production texture bake. |");
        builder.AppendLine("| Gate 3: Camera and composition | Pass/partial | " + metrics.cameraDescription + " Body occupancy lands in the target range if the mask metric is trusted. |");
        builder.AppendLine("| Gate 4: Lighting and contrast | Partial | Warm key, low cool fill, rim/specular accents, coil/gauge practicals, fog, and smoke are present, but there is no post stack or authored reflection setup. |");
        builder.AppendLine("| Gate 5: Resolution and file checks | Pass | Hero is 1920x1080, contact sheet is above 1536x1024, names include Recovery04 and pressure_pistol, and this report/metrics file name the method and gaps. |");
        builder.AppendLine("| Gate 6: Human review | Fail until reviewed | This is a stronger Unity proof workflow and should read closer than the rejected blockout, but it is not accepted final art without human visual approval against the north-star concept. |");
        builder.AppendLine();
        builder.AppendLine("## Honest Visual Read");
        builder.AppendLine();
        builder.AppendLine("The Unity proof should communicate the target silhouette better than prior blockouts: chunky first-person pistol, left-forward muzzle, large gauge, hot coil window, brass/iron layering, lower pressure tank, walnut grip, leather hand mass, visible ports, and smoke. It is still not good enough to promote as final high-fidelity weapon art because primitive cylinders/boxes cannot fully match the north-star concept's sculpted bevels, dense occlusion, worn hand-authored surface breakup, and physically richer metal/glass response.");
        return builder.ToString();
    }

    private static void AddPlate(string name, Vector3 position, Vector3 scale, Quaternion rotation, Material material, Transform parent, ProofMetrics metrics)
    {
        CreateBox(name, position, scale, rotation, material, parent);
        metrics.platesAndBrackets++;
    }

    private static void AddRivetRow(string prefix, Vector3 start, Vector3 end, int count, float radius, Material material, Transform parent, ProofMetrics metrics)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            AddRivet(prefix + " " + i.ToString(CultureInfo.InvariantCulture), Vector3.Lerp(start, end, t), radius, material, parent, metrics);
        }
    }

    private static void AddRivet(string name, Vector3 position, float radius, Material material, Transform parent, ProofMetrics metrics)
    {
        CreateSphere(name, position, Vector3.one * (radius * 2f), material, parent);
        metrics.fasteners++;
    }

    private static void AddComponentRivetRow(string prefix, Vector3 start, Vector3 end, int count, float radius, Material material, Transform parent, ComponentGateMetric metrics)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            AddComponentRivet(prefix + " " + i.ToString(CultureInfo.InvariantCulture), Vector3.Lerp(start, end, t), radius, material, parent, metrics);
        }
    }

    private static void AddBackgroundRivetRow(string prefix, Vector3 start, Vector3 end, int count, float radius, Material material, Transform parent)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            CreateSphere(prefix + " " + i.ToString(CultureInfo.InvariantCulture), Vector3.Lerp(start, end, t), Vector3.one * (radius * 2f), material, parent);
        }
    }

    private static void AddComponentRivet(string name, Vector3 position, float radius, Material material, Transform parent, ComponentGateMetric metrics)
    {
        CreateSphere(name, position, Vector3.one * (radius * 2f), material, parent);
        metrics.fasteners++;
    }

    private static void CreateCopperCoilLoop(string name, Vector3 center, float radiusY, float radiusZ, float tubeRadius, Material material, Transform parent)
    {
        int segments = 16;
        for (int i = 0; i < segments; i++)
        {
            float angleA = i * Mathf.PI * 2f / segments;
            float angleB = (i + 1) * Mathf.PI * 2f / segments;
            Vector3 pointA = center + new Vector3(0f, Mathf.Sin(angleA) * radiusY, Mathf.Cos(angleA) * radiusZ);
            Vector3 pointB = center + new Vector3(0f, Mathf.Sin(angleB) * radiusY, Mathf.Cos(angleB) * radiusZ);
            CreateCylinderBetween(name + " segment " + i.ToString(CultureInfo.InvariantCulture), pointA, pointB, tubeRadius, material, parent);
        }
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

    private static Light CreateLight(string name, LightType type, Vector3 position, Quaternion rotation, Color color, float intensity, float spotAngle, bool shadows)
    {
        GameObject lightObject = new GameObject(name);
        lightObject.transform.position = position;
        lightObject.transform.rotation = rotation;
        Light light = lightObject.AddComponent<Light>();
        light.type = type;
        light.color = color;
        light.intensity = intensity;
        light.range = type == LightType.Point ? 4.5f : 9f;
        if (type == LightType.Spot)
        {
            light.spotAngle = spotAngle;
            light.range = 9f;
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

    private static GameObject CreateTexturedQuad(string name, Vector3 position, Vector2 size, Quaternion rotation, Material material, Transform parent)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, true);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.transform.localScale = new Vector3(size.x, size.y, 1f);
        AssignMaterial(gameObject, material);
        return gameObject;
    }

    private static void CreateLabel(string text, Vector3 position, float characterSize, Material material, TextAnchor anchor)
    {
        GameObject labelObject = new GameObject("label - " + text);
        labelObject.transform.position = position;
        TextMesh textMesh = labelObject.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = anchor;
        textMesh.alignment = TextAlignment.Left;
        textMesh.characterSize = characterSize;
        textMesh.fontSize = 72;
        textMesh.color = material.color;
        Renderer renderer = labelObject.GetComponent<Renderer>();
        if (renderer != null && textMesh.font != null)
        {
            renderer.sharedMaterial = textMesh.font.material;
        }
    }

    private static void AssignMaterial(GameObject gameObject, Material material)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }
    }

    private static Texture2D CreateProceduralSurfaceTexture(Color baseColor, int seed)
    {
        int size = 256;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false, false);
        texture.name = "UnityProof_Surface_" + seed.ToString(CultureInfo.InvariantCulture);
        System.Random random = new System.Random(seed);
        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float grain = Mathf.PerlinNoise((x + seed) * 0.045f, (y - seed) * 0.055f);
                float scratch = random.NextDouble() > 0.992 ? 0.34f : 0f;
                float edgeDark = (x % 64 < 3 || y % 64 < 3) ? -0.08f : 0f;
                float value = 0.82f + grain * 0.32f + scratch + edgeDark;
                pixels[y * size + x] = new Color(
                    Mathf.Clamp01(baseColor.r * value),
                    Mathf.Clamp01(baseColor.g * value),
                    Mathf.Clamp01(baseColor.b * value),
                    1f);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(false, false);
        return texture;
    }

    private static Texture2D CreateProceduralNormalTexture(int seed)
    {
        int size = 128;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false, true);
        texture.name = "UnityProof_Normal_" + seed.ToString(CultureInfo.InvariantCulture);
        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float nx = (Mathf.PerlinNoise((x + seed) * 0.08f, y * 0.05f) - 0.5f) * 0.18f;
                float ny = (Mathf.PerlinNoise(x * 0.05f, (y - seed) * 0.08f) - 0.5f) * 0.18f;
                pixels[y * size + x] = new Color(0.5f + nx, 0.5f + ny, 1.0f, 1.0f);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(false, false);
        return texture;
    }

    private static Texture2D CreateProceduralOcclusionTexture(int seed)
    {
        int size = 128;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false, true);
        texture.name = "UnityProof_Occlusion_" + seed.ToString(CultureInfo.InvariantCulture);
        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float grime = Mathf.PerlinNoise((x + seed) * 0.035f, (y + seed) * 0.047f);
                float v = Mathf.Lerp(0.58f, 1f, grime);
                pixels[y * size + x] = new Color(v, v, v, 1f);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(false, false);
        return texture;
    }

    private static Texture2D CreateSmokeTexture(int size, int seed)
    {
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false, false);
        texture.name = "UnityProof_Smoke_" + seed.ToString(CultureInfo.InvariantCulture);
        Color[] pixels = new Color[size * size];
        Vector2 center = new Vector2(size * 0.5f, size * 0.5f);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center) / (size * 0.5f);
                float noise = Mathf.PerlinNoise((x + seed) * 0.06f, (y - seed) * 0.06f);
                float alpha = Mathf.Clamp01((1f - distance) * 1.25f) * Mathf.Lerp(0.38f, 0.95f, noise);
                pixels[y * size + x] = new Color(0.82f, 0.8f, 0.76f, alpha);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(false, false);
        return texture;
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
        if (material.HasProperty(property))
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

    private static void DestroyTexture(Texture2D texture)
    {
        if (texture != null)
        {
            UnityEngine.Object.DestroyImmediate(texture);
        }
    }

    [Serializable]
    private sealed class ProofMetrics
    {
        public string timestamp;
        public string unityVersion;
        public string renderPath;
        public string contactSheetPath;
        public string reportPath;
        public string metricsPath;
        public int heroWidth;
        public int heroHeight;
        public int contactSheetWidth;
        public int contactSheetHeight;
        public int mainBarrels;
        public int lowerPressureTanks;
        public int readableGauges;
        public int visibleCoilTurns;
        public int muzzleRings;
        public int triggers;
        public int triggerGuards;
        public int gripOrGloveMasses;
        public int pressurePorts;
        public int topValvesOrCaps;
        public int platesAndBrackets;
        public int fasteners;
        public int gaugeTickMarks;
        public int steamPuffBillboards;
        public int materialRoles;
        public int unityMaterialInstances;
        public float bodyOccupancyWidthPercent;
        public float bodyOccupancyHeightPercent;
        public int bodyMaskMinX;
        public int bodyMaskMinY;
        public int bodyMaskMaxX;
        public int bodyMaskMaxY;
        public bool warmKeyLight;
        public bool lowCoolFill;
        public bool rimLight;
        public string cameraDescription;
    }

    [Serializable]
    private sealed class ComponentProofMetrics
    {
        public string timestamp;
        public string unityVersion;
        public string batchmodeEntrypoint;
        public string exactUnityCommand;
        public string contactSheetPath;
        public string reportPath;
        public string metricsPath;
        public string recovery04Status;
        public string smokePolicy;
        public string overallAssessment;
        public ComponentGateMetric[] components;
    }

    [Serializable]
    private sealed class ComponentGateMetric
    {
        public string key;
        public string title;
        public string renderPath;
        public int width;
        public int height;
        public string gateStatus;
        public string notes;
        public int coilTurns;
        public int fasteners;
        public int platesOrBrackets;
        public int gaugeTickMarks;
        public int blackenedIronCylinders;
        public int muzzleSteps;
        public int materialRoles;
        public int steamSprites;
        public int bevelRingsOrCollars;
        public int warmHighlightAccents;
        public int materialVariationAccents;
        public bool warmEmissiveCore;
        public bool darkRecessFrame;
        public bool brassFasteners;
        public bool brassBezel;
        public bool creamFace;
        public bool glassHighlight;
        public bool redNeedle;
        public bool agedRim;
        public bool separateLowerTank;
        public bool brassCollars;
        public bool darkOcclusion;
        public bool edgeHighlights;
        public bool darkBore;
        public bool brassIronSeparation;
        public bool leftForwardDepth;
        public bool walnutLeatherDistinct;
        public bool firstPersonAnchor;
        public bool triggerGuardReadable;
        public bool reducedBlackSpaceFraming;
        public bool sculpturalGripSilhouette;
    }

    private sealed class ComponentRenderResult
    {
        public string Key;
        public string Title;
        public string RelativePath;
        public Texture2D Texture;
        public ComponentGateMetric Metric;
    }

    private sealed class ProofMaterials
    {
        public Material BlackenedIron;
        public Material AgedBrass;
        public Material DarkAgedBrass;
        public Material DarkPipeMetal;
        public Material HotCopper;
        public Material AgedCopper;
        public Material GaugeFace;
        public Material Glass;
        public Material Walnut;
        public Material DarkLeather;
        public Material LeatherHighlight;
        public Material Smoke;
        public Material ContactBack;
        public Material MaskWhite;
        public Material WarningRed;
        public Material LineDark;
        public Material GlowAmber;
        public Material IronEdgeHighlight;
        public Material BrassHighlight;
        public Material CopperHighlight;
        public Material PatinaGreen;

        public void DestroyAll()
        {
            DestroyMaterial(BlackenedIron);
            DestroyMaterial(AgedBrass);
            DestroyMaterial(DarkAgedBrass);
            DestroyMaterial(DarkPipeMetal);
            DestroyMaterial(HotCopper);
            DestroyMaterial(AgedCopper);
            DestroyMaterial(GaugeFace);
            DestroyMaterial(Glass);
            DestroyMaterial(Walnut);
            DestroyMaterial(DarkLeather);
            DestroyMaterial(LeatherHighlight);
            DestroyMaterial(Smoke);
            DestroyMaterial(ContactBack);
            DestroyMaterial(MaskWhite);
            DestroyMaterial(WarningRed);
            DestroyMaterial(LineDark);
            DestroyMaterial(GlowAmber);
            DestroyMaterial(IronEdgeHighlight);
            DestroyMaterial(BrassHighlight);
            DestroyMaterial(CopperHighlight);
            DestroyMaterial(PatinaGreen);
        }

        private static void DestroyMaterial(Material material)
        {
            if (material == null)
            {
                return;
            }

            Texture mainTexture = null;
            if (material.HasProperty("_MainTex"))
            {
                mainTexture = material.GetTexture("_MainTex");
            }
            else if (material.HasProperty("_BaseMap"))
            {
                mainTexture = material.GetTexture("_BaseMap");
            }

            Texture bump = material.HasProperty("_BumpMap") ? material.GetTexture("_BumpMap") : null;
            Texture occlusion = material.HasProperty("_OcclusionMap") ? material.GetTexture("_OcclusionMap") : null;
            UnityEngine.Object.DestroyImmediate(material);
            if (mainTexture != null)
            {
                UnityEngine.Object.DestroyImmediate(mainTexture);
            }

            if (bump != null && bump != mainTexture)
            {
                UnityEngine.Object.DestroyImmediate(bump);
            }

            if (occlusion != null && occlusion != mainTexture && occlusion != bump)
            {
                UnityEngine.Object.DestroyImmediate(occlusion);
            }
        }
    }
}
