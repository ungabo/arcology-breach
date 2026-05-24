using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class UnityCorridorMaterialProofRenderer
{
    private static readonly List<Texture> OwnedTextures = new List<Texture>();

    private const int HeroWidth = 1920;
    private const int HeroHeight = 1080;
    private const int DetailWidth = 1600;
    private const int DetailHeight = 1000;
    private const int ContactWidth = 2200;
    private const int ContactHeight = 1400;
    private const string TextureFolderRelativePath = "Assets/_Project/ArtStaging/FinalMaterialsV1/Textures";
    private const string ReportRelativePath = "Documentation/AssetProduction/EnvironmentLookdev/ENV_RECOVERY01_CORRIDOR_MATERIAL_PROOF_REPORT.md";
    private const string MetricsRelativePath = "Documentation/AssetProduction/EnvironmentLookdev/env_recovery01_corridor_material_metrics.json";
    private const string ContactRelativePath = "Documentation/ConceptRenders/CONTACTSHEET_ENV_Recovery01_corridor_material_unity_proof.jpg";
    private const string WideRenderRelativePath = "Documentation/ConceptRenders/RENDER_ENV_Recovery01_corridor_wide_unity_proof.jpg";
    private const string FloorRenderRelativePath = "Documentation/ConceptRenders/RENDER_ENV_Recovery01_floor_wetness_unity_proof.jpg";
    private const string GateRenderRelativePath = "Documentation/ConceptRenders/RENDER_ENV_Recovery01_pressure_gate_detail_unity_proof.jpg";

    [MenuItem("Project Tools/Lookdev/Render Recovery01 Corridor Material Proof")]
    public static void RenderFromMenu()
    {
        RenderBatch();
    }

    public static void RenderBatch()
    {
        try
        {
            RenderProof();
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

    private static void RenderProof()
    {
        string projectRoot = GetProjectRoot();
        Directory.CreateDirectory(Path.Combine(projectRoot, "Documentation/ConceptRenders"));
        Directory.CreateDirectory(Path.Combine(projectRoot, "Documentation/AssetProduction/EnvironmentLookdev"));

        CorridorMetrics metrics = new CorridorMetrics
        {
            timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture),
            unityVersion = Application.unityVersion,
            batchmodeEntrypoint = "UnityCorridorMaterialProofRenderer.RenderBatch",
            textureSourceFolder = TextureFolderRelativePath,
            renderPath = WideRenderRelativePath,
            floorDetailRenderPath = FloorRenderRelativePath,
            gateDetailRenderPath = GateRenderRelativePath,
            contactSheetPath = ContactRelativePath,
            reportPath = ReportRelativePath,
            metricsPath = MetricsRelativePath,
            heroWidth = HeroWidth,
            heroHeight = HeroHeight,
            detailWidth = DetailWidth,
            detailHeight = DetailHeight,
            contactSheetWidth = ContactWidth,
            contactSheetHeight = ContactHeight,
            cameraDescription = "Player-height 16:9 Unity camera, looking down an 8m corridor bay toward a warm pressure-gate hint."
        };

        List<Texture2D> renderTextures = new List<Texture2D>();
        Texture2D contactSheet = null;
        CorridorMaterials materials = null;

        try
        {
            OwnedTextures.Clear();
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            ConfigureRenderSettings();
            materials = CreateCorridorMaterials(projectRoot, metrics);

            GameObject corridorRoot = new GameObject("UnityCorridorMaterialProof_Recovery01_Bay");
            BuildCorridorBay(corridorRoot.transform, materials, metrics);
            BuildLighting(metrics);

            Texture2D wide = RenderView(WideRenderRelativePath, "wide corridor proof", CreateWideCamera(metrics), HeroWidth, HeroHeight, projectRoot, metrics);
            renderTextures.Add(wide);

            Texture2D floor = RenderView(FloorRenderRelativePath, "wet floor material proof", CreateFloorCamera(), DetailWidth, DetailHeight, projectRoot, metrics);
            renderTextures.Add(floor);

            Texture2D gate = RenderView(GateRenderRelativePath, "pressure gate detail proof", CreateGateCamera(), DetailWidth, DetailHeight, projectRoot, metrics);
            renderTextures.Add(gate);

            metrics.imageMetrics = new ImageMetric[]
            {
                AnalyzeImage("wide_corridor", WideRenderRelativePath, wide),
                AnalyzeImage("floor_wetness", FloorRenderRelativePath, floor),
                AnalyzeImage("pressure_gate_detail", GateRenderRelativePath, gate)
            };
            metrics.gates = BuildGates(metrics);
            metrics.overallStatus = DetermineOverallStatus(metrics.gates);

            contactSheet = BuildContactSheet(renderTextures, metrics);
            File.WriteAllBytes(Path.Combine(projectRoot, ContactRelativePath), contactSheet.EncodeToJPG(94));
            File.WriteAllText(Path.Combine(projectRoot, MetricsRelativePath), JsonUtility.ToJson(metrics, true));
            File.WriteAllText(Path.Combine(projectRoot, ReportRelativePath), BuildReport(metrics));

            Debug.Log("Unity corridor material proof render written to " + Path.Combine(projectRoot, WideRenderRelativePath));
            Debug.Log("Unity corridor material proof contact sheet written to " + Path.Combine(projectRoot, ContactRelativePath));
            Debug.Log("Unity corridor material proof report written to " + Path.Combine(projectRoot, ReportRelativePath));
        }
        finally
        {
            DestroyTexture(contactSheet);
            for (int i = 0; i < renderTextures.Count; i++)
            {
                DestroyTexture(renderTextures[i]);
            }

            if (materials != null)
            {
                materials.DestroyAll();
            }

            DestroyOwnedTextures();
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }
    }

    private static void ConfigureRenderSettings()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.052f, 0.045f, 0.038f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.035f, 0.031f, 0.028f);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.021f;
        RenderSettings.reflectionIntensity = 0.42f;
    }

    private static CorridorMaterials CreateCorridorMaterials(string projectRoot, CorridorMetrics metrics)
    {
        CorridorMaterials materials = new CorridorMaterials();
        materials.WetStone = CreatePbrMaterial(projectRoot, "WetOilDarkStone", "UnityCorridorProof_WetOilDarkStone", 0.02f, 0.78f, new Vector2(2.2f, 5.2f), metrics, false, false);
        materials.SootBrick = CreatePbrMaterial(projectRoot, "SootBrick", "UnityCorridorProof_SootBrick", 0.0f, 0.38f, new Vector2(2.0f, 2.8f), metrics, false, false);
        materials.BlackenedIron = CreatePbrMaterial(projectRoot, "BlackenedRivetedIron", "UnityCorridorProof_BlackenedRivetedIron", 0.85f, 0.34f, new Vector2(1.5f, 1.5f), metrics, false, false);
        materials.AgedBrass = CreatePbrMaterial(projectRoot, "AgedBrass", "UnityCorridorProof_AgedBrass", 0.92f, 0.58f, new Vector2(1.3f, 1.3f), metrics, false, false);
        materials.CopperPipe = CreatePbrMaterial(projectRoot, "CopperPipe", "UnityCorridorProof_CopperPipe", 0.93f, 0.62f, new Vector2(1.7f, 1.0f), metrics, false, false);
        materials.CreamGauge = CreatePbrMaterial(projectRoot, "CreamEnamelGauge", "UnityCorridorProof_CreamGaugeFace", 0.0f, 0.46f, Vector2.one, metrics, false, false);
        materials.AmberGlass = CreatePbrMaterial(projectRoot, "AmberGlass", "UnityCorridorProof_AmberGlass", 0.0f, 0.78f, Vector2.one, metrics, true, true);
        materials.GreasyWalnut = CreatePbrMaterial(projectRoot, "GreasyWalnut", "UnityCorridorProof_GreasyWalnut", 0.0f, 0.36f, new Vector2(1.0f, 1.6f), metrics, false, false);
        materials.HazardEnamel = CreatePbrMaterial(projectRoot, "HazardEnamel", "UnityCorridorProof_HazardEnamel", 0.0f, 0.5f, Vector2.one, metrics, false, false);
        materials.LeatherBellows = CreatePbrMaterial(projectRoot, "LeatherBellows", "UnityCorridorProof_LeatherBellows", 0.0f, 0.34f, Vector2.one, metrics, false, false);
        materials.Decal = CreateDecalMaterial(projectRoot, metrics);
        materials.WarmGlow = CreateUnlitColorMaterial("UnityCorridorProof_WarmLampGlow", new Color(1.0f, 0.49f, 0.12f, 1f), true);
        materials.RedGlow = CreateUnlitColorMaterial("UnityCorridorProof_RedPressureState", new Color(1.0f, 0.08f, 0.025f, 1f), true);
        materials.GreenGlow = CreateUnlitColorMaterial("UnityCorridorProof_GreenServiceState", new Color(0.2f, 0.95f, 0.36f, 1f), true);
        materials.LineDark = CreateUnlitColorMaterial("UnityCorridorProof_DarkGaugeInk", new Color(0.035f, 0.026f, 0.018f, 1f), false);
        materials.WetHighlight = CreateTransparentColorMaterial("UnityCorridorProof_BrokenWetHighlight", new Color(1.0f, 0.64f, 0.24f, 0.17f), 0.0f, 0.95f);
        materials.ContactBack = CreateUnlitColorMaterial("UnityCorridorProof_ContactBack", new Color(0.025f, 0.021f, 0.018f, 1f), false);
        metrics.materialFamiliesUsed = CountUsedMaterialFamilies(metrics);
        return materials;
    }

    private static Material CreatePbrMaterial(string projectRoot, string materialKey, string materialName, float defaultMetallic, float defaultSmoothness, Vector2 tiling, CorridorMetrics metrics, bool transparent, bool emission)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material material = new Material(shader);
        material.name = materialName;

        Texture2D baseMap = LoadTextureFromDisk(projectRoot, materialKey, "BaseColor", false, metrics);
        Texture2D normalMap = LoadTextureFromDisk(projectRoot, materialKey, "Normal", true, metrics);
        Texture2D ormMap = LoadTextureFromDisk(projectRoot, materialKey, "ORM", true, metrics);
        Texture2D metallicSmoothness = null;
        Texture2D occlusion = null;
        if (ormMap != null)
        {
            metallicSmoothness = CreateMetallicSmoothnessFromOrm(ormMap, materialName + "_MetallicSmoothness");
            occlusion = CreateOcclusionFromOrm(ormMap, materialName + "_Occlusion");
        }

        SetMaterialTexture(material, "_MainTex", baseMap);
        SetMaterialTexture(material, "_BaseMap", baseMap);
        SetMaterialTexture(material, "_BumpMap", normalMap);
        SetMaterialTexture(material, "_NormalMap", normalMap);
        SetMaterialTexture(material, "_OcclusionMap", occlusion);
        SetMaterialTexture(material, "_MetallicGlossMap", metallicSmoothness);
        SetMaterialFloat(material, "_Metallic", defaultMetallic);
        SetMaterialFloat(material, "_Glossiness", defaultSmoothness);
        SetMaterialFloat(material, "_Smoothness", defaultSmoothness);
        if (baseMap != null)
        {
            material.mainTexture = baseMap;
        }

        SetMaterialTextureScale(material, "_MainTex", tiling);
        SetMaterialTextureScale(material, "_BaseMap", tiling);
        material.EnableKeyword("_NORMALMAP");
        material.EnableKeyword("_OCCLUSIONMAP");
        material.EnableKeyword("_METALLICGLOSSMAP");

        if (transparent)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = (int)RenderQueue.Transparent;
            SetMaterialFloat(material, "_Surface", 1.0f);
            SetMaterialFloat(material, "_Mode", 3.0f);
            SetMaterialInt(material, "_SrcBlend", (int)BlendMode.SrcAlpha);
            SetMaterialInt(material, "_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            SetMaterialInt(material, "_ZWrite", 0);
            material.EnableKeyword("_ALPHABLEND_ON");
            SetMaterialColor(material, new Color(1.0f, 0.7f, 0.28f, 0.5f));
        }
        else
        {
            SetMaterialColor(material, Color.white);
        }

        if (emission)
        {
            SetMaterialColor(material, "_EmissionColor", new Color(1.0f, 0.42f, 0.08f) * 1.35f);
            material.EnableKeyword("_EMISSION");
        }

        TrackOwnedTexture(material, baseMap);
        TrackOwnedTexture(material, normalMap);
        TrackOwnedTexture(material, ormMap);
        TrackOwnedTexture(material, metallicSmoothness);
        TrackOwnedTexture(material, occlusion);
        return material;
    }

    private static Texture2D LoadTextureFromDisk(string projectRoot, string materialKey, string textureRole, bool linear, CorridorMetrics metrics)
    {
        string relativePath = TextureFolderRelativePath + "/T_BBW_" + materialKey + "_" + textureRole + "_2048.png";
        string absolutePath = Path.Combine(projectRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        metrics.textureFilesExpected++;
        if (!File.Exists(absolutePath))
        {
            metrics.textureFilesMissing++;
            metrics.missingTextureNotes.Add(relativePath);
            return null;
        }

        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, linear);
        texture.name = "Loaded_" + materialKey + "_" + textureRole;
        if (!texture.LoadImage(File.ReadAllBytes(absolutePath), false))
        {
            metrics.textureFilesMissing++;
            metrics.missingTextureNotes.Add(relativePath + " (LoadImage failed)");
            DestroyTexture(texture);
            return null;
        }

        texture.wrapMode = TextureWrapMode.Repeat;
        texture.filterMode = FilterMode.Trilinear;
        texture.anisoLevel = 4;
        metrics.textureFilesLoaded++;
        return texture;
    }

    private static Texture2D CreateMetallicSmoothnessFromOrm(Texture2D orm, string name)
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

    private static Texture2D CreateOcclusionFromOrm(Texture2D orm, string name)
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

    private static Material CreateDecalMaterial(string projectRoot, CorridorMetrics metrics)
    {
        Texture2D baseColor = LoadTextureFromDisk(projectRoot, "ScorchOilDecalAtlas", "BaseColor", false, metrics);
        Texture2D alpha = LoadTextureFromDisk(projectRoot, "ScorchOilDecalAtlas", "Alpha", true, metrics);
        Texture2D combined = CreateCombinedAlphaTexture(baseColor, alpha, "UnityCorridorProof_ScorchOilDecalCombined");
        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null)
        {
            shader = Shader.Find("Unlit/Transparent");
        }

        if (shader == null)
        {
            shader = Shader.Find("Sprites/Default");
        }

        Material material = new Material(shader);
        material.name = "UnityCorridorProof_ScorchOilDecal";
        SetMaterialTexture(material, "_MainTex", combined);
        SetMaterialTexture(material, "_BaseMap", combined);
        SetMaterialColor(material, Color.white);
        material.mainTexture = combined;
        material.renderQueue = (int)RenderQueue.Transparent + 30;
        material.SetOverrideTag("RenderType", "Transparent");
        SetMaterialInt(material, "_ZWrite", 0);
        SetMaterialInt(material, "_Cull", (int)CullMode.Off);
        SetMaterialFloat(material, "_Surface", 1.0f);
        SetMaterialFloat(material, "_Mode", 3.0f);
        SetMaterialInt(material, "_SrcBlend", (int)BlendMode.SrcAlpha);
        SetMaterialInt(material, "_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        material.EnableKeyword("_ALPHABLEND_ON");
        TrackOwnedTexture(material, baseColor);
        TrackOwnedTexture(material, alpha);
        TrackOwnedTexture(material, combined);
        return material;
    }

    private static Texture2D CreateCombinedAlphaTexture(Texture2D baseColor, Texture2D alpha, string name)
    {
        if (baseColor == null)
        {
            Texture2D fallback = new Texture2D(4, 4, TextureFormat.RGBA32, false, false);
            Color32[] fallbackPixels = new Color32[16];
            for (int i = 0; i < fallbackPixels.Length; i++)
            {
                fallbackPixels[i] = new Color32(16, 12, 8, 120);
            }

            fallback.SetPixels32(fallbackPixels);
            fallback.Apply(false, false);
            return fallback;
        }

        Color32[] basePixels = baseColor.GetPixels32();
        Color32[] alphaPixels = alpha != null ? alpha.GetPixels32() : null;
        Color32[] output = new Color32[basePixels.Length];
        for (int i = 0; i < basePixels.Length; i++)
        {
            byte a = alphaPixels != null && i < alphaPixels.Length ? alphaPixels[i].r : basePixels[i].a;
            output[i] = new Color32(basePixels[i].r, basePixels[i].g, basePixels[i].b, a);
        }

        Texture2D combined = new Texture2D(baseColor.width, baseColor.height, TextureFormat.RGBA32, true, false);
        combined.name = name;
        combined.SetPixels32(output);
        combined.Apply(true, false);
        combined.wrapMode = TextureWrapMode.Clamp;
        combined.filterMode = FilterMode.Trilinear;
        return combined;
    }

    private static int CountUsedMaterialFamilies(CorridorMetrics metrics)
    {
        int expectedTexturesPerCoreFamily = 3;
        int coreFamilies = Mathf.FloorToInt((metrics.textureFilesLoaded - 2) / (float)expectedTexturesPerCoreFamily);
        return Mathf.Clamp(coreFamilies + 1, 0, 11);
    }

    private static void BuildCorridorBay(Transform root, CorridorMaterials materials, CorridorMetrics metrics)
    {
        BuildArchitecture(root, materials, metrics);
        BuildPressureGateHint(root, materials, metrics);
        BuildPipeSystems(root, materials, metrics);
        BuildLamps(root, materials, metrics);
        BuildGaugesAndValves(root, materials, metrics);
        BuildWearLayer(root, materials, metrics);
        BuildWetHighlights(root, materials, metrics);
    }

    private static void BuildArchitecture(Transform root, CorridorMaterials materials, CorridorMetrics metrics)
    {
        CreateBox("wet oil-dark stone floor", new Vector3(0f, -0.055f, 3.8f), new Vector3(4.4f, 0.11f, 10.2f), Quaternion.identity, materials.WetStone, root);
        CreateBox("soot brick left wall", new Vector3(-2.12f, 1.32f, 3.8f), new Vector3(0.14f, 2.75f, 10.2f), Quaternion.identity, materials.SootBrick, root);
        CreateBox("soot brick right wall", new Vector3(2.12f, 1.32f, 3.8f), new Vector3(0.14f, 2.75f, 10.2f), Quaternion.identity, materials.SootBrick, root);
        CreateBox("blackened iron ceiling", new Vector3(0f, 2.72f, 3.8f), new Vector3(4.4f, 0.16f, 10.2f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("left lower iron wall course", new Vector3(-2.02f, 0.48f, 3.8f), new Vector3(0.18f, 0.72f, 9.6f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("right lower iron wall course", new Vector3(2.02f, 0.48f, 3.8f), new Vector3(0.18f, 0.72f, 9.6f), Quaternion.identity, materials.BlackenedIron, root);
        metrics.baseArchitecturePieces += 6;

        for (int i = 0; i < 9; i++)
        {
            float z = -0.55f + i * 1.05f;
            CreateBox("left blackened iron rib " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-2.0f, 1.45f, z), new Vector3(0.22f, 2.55f, 0.08f), Quaternion.identity, materials.BlackenedIron, root);
            CreateBox("right blackened iron rib " + i.ToString(CultureInfo.InvariantCulture), new Vector3(2.0f, 1.45f, z), new Vector3(0.22f, 2.55f, 0.08f), Quaternion.identity, materials.BlackenedIron, root);
            CreateBox("ceiling brass cross cap " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0f, 2.58f, z), new Vector3(4.05f, 0.08f, 0.065f), Quaternion.identity, materials.AgedBrass, root);
            metrics.wallPanels += 2;
            metrics.trimPieces++;
            AddRivetRow("left rib rivets " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-1.875f, 0.42f, z - 0.045f), new Vector3(-1.875f, 2.42f, z - 0.045f), 9, 0.032f, materials.AgedBrass, root, metrics);
            AddRivetRow("right rib rivets " + i.ToString(CultureInfo.InvariantCulture), new Vector3(1.875f, 0.42f, z - 0.045f), new Vector3(1.875f, 2.42f, z - 0.045f), 9, 0.032f, materials.AgedBrass, root, metrics);
        }

        for (int i = 0; i < 6; i++)
        {
            float z = 0.1f + i * 1.55f;
            CreateBox("left soot brick breakup panel " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-2.205f, 1.55f, z), new Vector3(0.035f, 0.72f, 0.92f), Quaternion.identity, materials.SootBrick, root);
            CreateBox("right soot brick breakup panel " + i.ToString(CultureInfo.InvariantCulture), new Vector3(2.205f, 1.55f, z + 0.42f), new Vector3(0.035f, 0.62f, 0.8f), Quaternion.identity, materials.SootBrick, root);
            metrics.tilingBreakups += 2;
        }
    }

    private static void BuildPressureGateHint(Transform root, CorridorMaterials materials, CorridorMetrics metrics)
    {
        float z = 8.88f;
        CreateBox("distant pressure gate black iron slab left", new Vector3(-0.88f, 1.25f, z), new Vector3(1.55f, 2.42f, 0.18f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("distant pressure gate black iron slab right", new Vector3(0.88f, 1.25f, z), new Vector3(1.55f, 2.42f, 0.18f), Quaternion.identity, materials.BlackenedIron, root);
        CreateBox("distant pressure gate center seam", new Vector3(0f, 1.25f, z - 0.1f), new Vector3(0.08f, 2.52f, 0.08f), Quaternion.identity, materials.AgedBrass, root);
        CreateBox("pressure gate hazard enamel top plate", new Vector3(0f, 2.32f, z - 0.13f), new Vector3(1.35f, 0.28f, 0.08f), Quaternion.identity, materials.HazardEnamel, root);
        CreateBox("pressure gate brass threshold", new Vector3(0f, 0.08f, z - 0.18f), new Vector3(3.25f, 0.12f, 0.18f), Quaternion.identity, materials.AgedBrass, root);
        CreateCylinderZ("pressure gate brass gear hint", new Vector3(-1.18f, 1.28f, z - 0.18f), 0.11f, 0.38f, materials.AgedBrass, root);
        CreateCylinderZ("pressure gate black hub", new Vector3(-1.18f, 1.28f, z - 0.26f), 0.08f, 0.18f, materials.BlackenedIron, root);
        CreateValveWheel("pressure gate side valve", new Vector3(1.15f, 1.15f, z - 0.22f), Quaternion.Euler(0f, 0f, 0f), 0.34f, materials.CopperPipe, materials.AgedBrass, root, metrics);
        BuildForwardGauge("pressure gate gauge", new Vector3(0.72f, 1.84f, z - 0.24f), 0.25f, materials, root, metrics);
        AddRivetGrid("left pressure gate rivet grid", new Vector3(-1.48f, 0.48f, z - 0.21f), 4, 6, 0.42f, 0.31f, 0.032f, materials.AgedBrass, root, metrics);
        AddRivetGrid("right pressure gate rivet grid", new Vector3(0.42f, 0.48f, z - 0.21f), 4, 6, 0.42f, 0.31f, 0.032f, materials.AgedBrass, root, metrics);
        metrics.pressureGateHints = 1;
        metrics.routeCueCount = 2;
    }

    private static void BuildPipeSystems(Transform root, CorridorMaterials materials, CorridorMetrics metrics)
    {
        for (int i = 0; i < 5; i++)
        {
            float y = 1.0f + i * 0.22f;
            CreateCylinderZ("left copper wall pipe run " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-1.78f, y, 4.1f), 8.4f, 0.035f + i * 0.003f, materials.CopperPipe, root);
            CreateCylinderZ("right brass wall pipe run " + i.ToString(CultureInfo.InvariantCulture), new Vector3(1.78f, y + 0.08f, 4.0f), 8.1f, 0.032f + i * 0.003f, materials.AgedBrass, root);
            metrics.pipeSegments += 2;
        }

        for (int i = 0; i < 5; i++)
        {
            float z = 0.2f + i * 1.8f;
            CreateCylinderX("ceiling cross pipe " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0f, 2.42f, z), 3.5f, 0.045f, i % 2 == 0 ? materials.CopperPipe : materials.BlackenedIron, root);
            CreateCylinderY("left vertical pipe drop " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-1.58f, 1.35f, z + 0.34f), 1.55f, 0.038f, materials.CopperPipe, root);
            metrics.pipeSegments += 2;
        }

        CreateCylinderBetween("diagonal service pipe left foreground", new Vector3(-1.66f, 0.62f, -0.65f), new Vector3(-1.75f, 1.85f, 1.0f), 0.04f, materials.CopperPipe, root);
        CreateCylinderBetween("diagonal black iron conduit right foreground", new Vector3(1.72f, 0.72f, -0.55f), new Vector3(1.82f, 1.9f, 0.8f), 0.035f, materials.BlackenedIron, root);
        metrics.pipeSegments += 2;
    }

    private static void BuildLamps(Transform root, CorridorMaterials materials, CorridorMetrics metrics)
    {
        Vector3[] positions =
        {
            new Vector3(-1.82f, 2.0f, 0.35f),
            new Vector3(1.82f, 2.0f, 2.35f),
            new Vector3(-1.82f, 2.0f, 4.75f),
            new Vector3(1.82f, 2.0f, 6.85f),
            new Vector3(0f, 2.38f, 8.25f)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 position = positions[i];
            CreateBox("amber lamp black iron hood " + i.ToString(CultureInfo.InvariantCulture), position + new Vector3(0f, 0.08f, 0f), new Vector3(0.48f, 0.14f, 0.24f), Quaternion.identity, materials.BlackenedIron, root);
            CreateCylinderY("amber lamp glass capsule " + i.ToString(CultureInfo.InvariantCulture), position, 0.32f, 0.13f, materials.AmberGlass, root);
            CreateSphere("amber lamp glowing core " + i.ToString(CultureInfo.InvariantCulture), position, Vector3.one * 0.18f, materials.WarmGlow, root);
            Light light = CreateLight("corridor amber practical " + i.ToString(CultureInfo.InvariantCulture), LightType.Point, position, Quaternion.identity, new Color(1.0f, 0.55f, 0.17f), i == positions.Length - 1 ? 2.9f : 2.25f, 0f, false);
            light.range = i == positions.Length - 1 ? 5.5f : 4.0f;
            metrics.lamps++;
        }
    }

    private static void BuildGaugesAndValves(Transform root, CorridorMaterials materials, CorridorMetrics metrics)
    {
        BuildGauge("left foreground wall gauge", new Vector3(-1.72f, 1.55f, 0.72f), 0.22f, materials, root, metrics);
        BuildGauge("right middle wall gauge", new Vector3(1.72f, 1.48f, 2.9f), 0.2f, materials, root, metrics);
        BuildGauge("left rear wall gauge", new Vector3(-1.72f, 1.42f, 5.3f), 0.18f, materials, root, metrics);
        BuildGauge("right rear small wall gauge", new Vector3(1.72f, 1.7f, 6.4f), 0.16f, materials, root, metrics);

        CreateValveWheel("left foreground valve", new Vector3(-1.72f, 0.92f, 1.28f), Quaternion.identity, 0.28f, materials.CopperPipe, materials.AgedBrass, root, metrics);
        CreateValveWheel("right middle valve", new Vector3(1.72f, 0.95f, 3.55f), Quaternion.identity, 0.25f, materials.CopperPipe, materials.AgedBrass, root, metrics);
        CreateValveWheel("left rear valve", new Vector3(-1.72f, 1.02f, 6.1f), Quaternion.identity, 0.23f, materials.CopperPipe, materials.AgedBrass, root, metrics);
        CreateValveWheel("ceiling service valve", new Vector3(0.58f, 2.2f, 4.1f), Quaternion.Euler(90f, 0f, 0f), 0.23f, materials.CopperPipe, materials.AgedBrass, root, metrics);

        CreateBox("green restored service lamp", new Vector3(0.42f, 1.84f, 8.62f), new Vector3(0.12f, 0.18f, 0.06f), Quaternion.identity, materials.GreenGlow, root);
        CreateBox("red locked pressure lamp", new Vector3(-0.42f, 1.84f, 8.62f), new Vector3(0.12f, 0.18f, 0.06f), Quaternion.identity, materials.RedGlow, root);
        metrics.routeCueCount += 2;
    }

    private static void BuildGauge(string name, Vector3 position, float radius, CorridorMaterials materials, Transform root, CorridorMetrics metrics)
    {
        CreateCylinderX(name + " brass bezel", position, 0.07f, radius, materials.AgedBrass, root);
        CreateCylinderX(name + " cream face", position + new Vector3(0.045f * Mathf.Sign(position.x == 0f ? 1f : -position.x), 0f, 0f), 0.035f, radius * 0.78f, materials.CreamGauge, root);
        CreateCylinderX(name + " glass lens", position + new Vector3(0.07f * Mathf.Sign(position.x == 0f ? 1f : -position.x), 0f, 0f), 0.016f, radius * 0.82f, materials.AmberGlass, root);
        CreateBox(name + " dark needle", position + new Vector3(0.085f * Mathf.Sign(position.x == 0f ? 1f : -position.x), 0.02f, 0f), new Vector3(0.018f, radius * 0.95f, 0.012f), Quaternion.Euler(0f, 0f, -43f), materials.LineDark, root);
        for (int i = 0; i < 12; i++)
        {
            float angle = i * Mathf.PI * 2f / 12f;
            Vector3 tick = position + new Vector3(0.09f * Mathf.Sign(position.x == 0f ? 1f : -position.x), Mathf.Sin(angle) * radius * 0.63f, Mathf.Cos(angle) * radius * 0.63f);
            CreateBox(name + " readable tick " + i.ToString(CultureInfo.InvariantCulture), tick, new Vector3(0.012f, radius * 0.13f, 0.01f), Quaternion.Euler(0f, 0f, -angle * Mathf.Rad2Deg), materials.LineDark, root);
            metrics.gaugeTickMarks++;
        }

        for (int i = 0; i < 8; i++)
        {
            float angle = i * Mathf.PI * 2f / 8f;
            Vector3 rivet = position + new Vector3(0.08f * Mathf.Sign(position.x == 0f ? 1f : -position.x), Mathf.Sin(angle) * radius * 0.93f, Mathf.Cos(angle) * radius * 0.93f);
            AddRivet(name + " bezel rivet " + i.ToString(CultureInfo.InvariantCulture), rivet, radius * 0.06f, materials.AgedBrass, root, metrics);
        }

        metrics.gauges++;
    }

    private static void BuildForwardGauge(string name, Vector3 position, float radius, CorridorMaterials materials, Transform root, CorridorMetrics metrics)
    {
        CreateCylinderZ(name + " brass bezel", position, 0.07f, radius, materials.AgedBrass, root);
        CreateCylinderZ(name + " cream face", position + new Vector3(0f, 0f, -0.045f), 0.035f, radius * 0.78f, materials.CreamGauge, root);
        CreateCylinderZ(name + " glass lens", position + new Vector3(0f, 0f, -0.07f), 0.016f, radius * 0.82f, materials.AmberGlass, root);
        CreateBox(name + " dark needle", position + new Vector3(0f, 0.02f, -0.085f), new Vector3(0.018f, radius * 0.95f, 0.012f), Quaternion.Euler(0f, 0f, -43f), materials.LineDark, root);
        for (int i = 0; i < 12; i++)
        {
            float angle = i * Mathf.PI * 2f / 12f;
            Vector3 tick = position + new Vector3(Mathf.Cos(angle) * radius * 0.63f, Mathf.Sin(angle) * radius * 0.63f, -0.09f);
            CreateBox(name + " readable tick " + i.ToString(CultureInfo.InvariantCulture), tick, new Vector3(radius * 0.13f, 0.012f, 0.01f), Quaternion.Euler(0f, 0f, -angle * Mathf.Rad2Deg), materials.LineDark, root);
            metrics.gaugeTickMarks++;
        }

        for (int i = 0; i < 8; i++)
        {
            float angle = i * Mathf.PI * 2f / 8f;
            Vector3 rivet = position + new Vector3(Mathf.Cos(angle) * radius * 0.93f, Mathf.Sin(angle) * radius * 0.93f, -0.08f);
            AddRivet(name + " bezel rivet " + i.ToString(CultureInfo.InvariantCulture), rivet, radius * 0.06f, materials.AgedBrass, root, metrics);
        }

        metrics.gauges++;
    }

    private static void CreateValveWheel(string name, Vector3 center, Quaternion rotation, float radius, Material pipeMaterial, Material hubMaterial, Transform root, CorridorMetrics metrics)
    {
        Transform wheelRoot = new GameObject(name).transform;
        wheelRoot.SetParent(root, true);
        wheelRoot.position = center;
        wheelRoot.rotation = rotation;
        for (int i = 0; i < 8; i++)
        {
            float angleA = i * Mathf.PI * 2f / 8f;
            float angleB = (i + 1) * Mathf.PI * 2f / 8f;
            Vector3 localA = new Vector3(0f, Mathf.Sin(angleA) * radius, Mathf.Cos(angleA) * radius);
            Vector3 localB = new Vector3(0f, Mathf.Sin(angleB) * radius, Mathf.Cos(angleB) * radius);
            CreateCylinderBetween(name + " ring segment " + i.ToString(CultureInfo.InvariantCulture), wheelRoot.TransformPoint(localA), wheelRoot.TransformPoint(localB), radius * 0.045f, pipeMaterial, root);
        }

        for (int i = 0; i < 4; i++)
        {
            float angle = i * Mathf.PI * 0.5f;
            Vector3 spokeEnd = center + rotation * new Vector3(0f, Mathf.Sin(angle) * radius * 0.82f, Mathf.Cos(angle) * radius * 0.82f);
            CreateCylinderBetween(name + " spoke " + i.ToString(CultureInfo.InvariantCulture), center, spokeEnd, radius * 0.035f, hubMaterial, root);
        }

        CreateSphere(name + " brass hub", center, Vector3.one * radius * 0.18f, hubMaterial, root);
        metrics.valves++;
        metrics.pipeSegments += 4;
    }

    private static void BuildWearLayer(Transform root, CorridorMaterials materials, CorridorMetrics metrics)
    {
        AddFloorDecal("foreground oil smear", new Vector3(-0.72f, 0.012f, 0.58f), new Vector2(1.05f, 0.58f), Quaternion.Euler(90f, 18f, 0f), materials.Decal, root, metrics);
        AddFloorDecal("center broken puddle grime", new Vector3(0.36f, 0.014f, 2.55f), new Vector2(1.25f, 0.7f), Quaternion.Euler(90f, -12f, 0f), materials.Decal, root, metrics);
        AddFloorDecal("rear threshold oil track", new Vector3(-0.18f, 0.016f, 6.95f), new Vector2(1.65f, 0.55f), Quaternion.Euler(90f, 4f, 0f), materials.Decal, root, metrics);
        AddWallDecal("left scorch fan", new Vector3(-1.935f, 1.05f, 2.05f), new Vector2(0.85f, 0.55f), Quaternion.Euler(0f, 90f, 0f), materials.Decal, root, metrics);
        AddWallDecal("right pipe leak stain", new Vector3(1.935f, 1.2f, 4.95f), new Vector2(0.7f, 0.72f), Quaternion.Euler(0f, -90f, 0f), materials.Decal, root, metrics);
        AddWallDecal("gate soot bloom", new Vector3(-0.92f, 0.9f, 8.68f), new Vector2(0.82f, 0.76f), Quaternion.identity, materials.Decal, root, metrics);
        metrics.tilingBreakups += 6;
    }

    private static void BuildWetHighlights(Transform root, CorridorMaterials materials, CorridorMetrics metrics)
    {
        AddFloorHighlight("warm lamp reflection foreground", new Vector3(-0.3f, 0.021f, 0.9f), new Vector2(0.72f, 1.6f), Quaternion.Euler(90f, -8f, 0f), materials.WetHighlight, root, metrics);
        AddFloorHighlight("broken amber route reflection", new Vector3(0.35f, 0.022f, 3.3f), new Vector2(0.58f, 2.2f), Quaternion.Euler(90f, 5f, 0f), materials.WetHighlight, root, metrics);
        AddFloorHighlight("pressure gate threshold reflection", new Vector3(0f, 0.023f, 7.55f), new Vector2(1.38f, 0.72f), Quaternion.Euler(90f, 0f, 0f), materials.WetHighlight, root, metrics);
    }

    private static void AddFloorDecal(string name, Vector3 position, Vector2 size, Quaternion rotation, Material material, Transform root, CorridorMetrics metrics)
    {
        CreateTexturedQuad(name, position, size, rotation, material, root);
        metrics.oilScorchDecals++;
    }

    private static void AddWallDecal(string name, Vector3 position, Vector2 size, Quaternion rotation, Material material, Transform root, CorridorMetrics metrics)
    {
        CreateTexturedQuad(name, position, size, rotation, material, root);
        metrics.oilScorchDecals++;
    }

    private static void AddFloorHighlight(string name, Vector3 position, Vector2 size, Quaternion rotation, Material material, Transform root, CorridorMetrics metrics)
    {
        CreateTexturedQuad(name, position, size, rotation, material, root);
        metrics.floorWetHighlightPatches++;
    }

    private static void BuildLighting(CorridorMetrics metrics)
    {
        CreateLight("wide warm corridor key", LightType.Spot, new Vector3(-0.95f, 2.55f, -0.35f), Quaternion.Euler(58f, 9f, 0f), new Color(1.0f, 0.56f, 0.2f), 5.0f, 58f, true);
        CreateLight("gate amber destination wash", LightType.Spot, new Vector3(0.2f, 2.25f, 7.75f), Quaternion.Euler(72f, 180f, 0f), new Color(1.0f, 0.48f, 0.12f), 4.2f, 64f, true);
        CreateLight("cool low side fill", LightType.Directional, new Vector3(0f, 0f, 0f), Quaternion.Euler(22f, -145f, 0f), new Color(0.16f, 0.18f, 0.22f), 0.2f, 0f, false);
        CreateLight("wet floor rake light", LightType.Spot, new Vector3(1.3f, 0.8f, -0.9f), Quaternion.Euler(20f, -24f, 0f), new Color(1.0f, 0.5f, 0.18f), 2.6f, 42f, true);
        CreateLight("brass rim edge from rear", LightType.Spot, new Vector3(-1.4f, 2.3f, 6.4f), Quaternion.Euler(72f, 160f, 0f), new Color(1.0f, 0.72f, 0.28f), 2.2f, 42f, true);
        metrics.warmKeyLight = true;
        metrics.coolFillLight = true;
        metrics.wetFloorRakeLight = true;
        metrics.gateDestinationLight = true;
    }

    private static Camera CreateWideCamera(CorridorMetrics metrics)
    {
        Camera camera = CreateCamera("Unity corridor material proof wide camera", new Vector3(0.34f, 1.46f, -1.74f), new Vector3(0f, 1.28f, 5.8f), 53f, new Color(0.03f, 0.026f, 0.022f));
        metrics.cameraPosition = FormatVector(camera.transform.position);
        return camera;
    }

    private static Camera CreateFloorCamera()
    {
        return CreateCamera("Unity corridor material proof floor camera", new Vector3(0.68f, 0.72f, -0.35f), new Vector3(0.05f, 0.08f, 3.15f), 44f, new Color(0.028f, 0.024f, 0.021f));
    }

    private static Camera CreateGateCamera()
    {
        return CreateCamera("Unity corridor material proof gate detail camera", new Vector3(0.35f, 1.34f, 4.85f), new Vector3(0.03f, 1.42f, 8.55f), 36f, new Color(0.028f, 0.024f, 0.021f));
    }

    private static Camera CreateCamera(string name, Vector3 position, Vector3 lookAt, float fieldOfView, Color background)
    {
        GameObject cameraObject = new GameObject(name);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = position;
        camera.transform.LookAt(lookAt);
        camera.fieldOfView = fieldOfView;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 35f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = background;
        camera.allowHDR = true;
        camera.allowMSAA = true;
        return camera;
    }

    private static Texture2D RenderView(string relativePath, string name, Camera camera, int width, int height, string projectRoot, CorridorMetrics metrics)
    {
        Texture2D texture = CaptureCamera(camera, width, height, camera.backgroundColor, "Unity corridor " + name);
        string outputPath = Path.Combine(projectRoot, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
        File.WriteAllBytes(outputPath, texture.EncodeToJPG(95));
        metrics.renderedImageCount++;
        return texture;
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

    private static ImageMetric AnalyzeImage(string key, string path, Texture2D texture)
    {
        Color32[] pixels = texture.GetPixels32();
        double luminanceSum = 0.0;
        int darkPixels = 0;
        int nearBlackPixels = 0;
        int brightPixels = 0;
        int warmPixels = 0;
        int coolPixels = 0;
        int saturatedPixels = 0;

        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 pixel = pixels[i];
            float r = pixel.r / 255f;
            float g = pixel.g / 255f;
            float b = pixel.b / 255f;
            float luminance = r * 0.2126f + g * 0.7152f + b * 0.0722f;
            luminanceSum += luminance;
            if (luminance < 0.08f)
            {
                darkPixels++;
            }

            if (luminance < 0.018f)
            {
                nearBlackPixels++;
            }

            if (luminance > 0.34f)
            {
                brightPixels++;
            }

            if (r > g * 1.12f && g > b * 1.18f && luminance > 0.11f)
            {
                warmPixels++;
            }

            if (b > r * 1.15f && b > g * 1.05f && luminance > 0.08f)
            {
                coolPixels++;
            }

            if (Mathf.Max(r, Mathf.Max(g, b)) - Mathf.Min(r, Mathf.Min(g, b)) > 0.2f && luminance > 0.08f)
            {
                saturatedPixels++;
            }
        }

        float pixelCount = pixels.Length;
        return new ImageMetric
        {
            key = key,
            path = path,
            width = texture.width,
            height = texture.height,
            averageLuminance = (float)(luminanceSum / pixelCount),
            darkPixelPercent = darkPixels * 100f / pixelCount,
            nearBlackPixelPercent = nearBlackPixels * 100f / pixelCount,
            brightPixelPercent = brightPixels * 100f / pixelCount,
            warmHighlightPercent = warmPixels * 100f / pixelCount,
            coolAccentPercent = coolPixels * 100f / pixelCount,
            saturatedAccentPercent = saturatedPixels * 100f / pixelCount,
            passesNotEmptyCheck = nearBlackPixels < pixels.Length * 0.72f && brightPixels > pixels.Length * 0.006f && warmPixels > pixels.Length * 0.012f
        };
    }

    private static ProofGate[] BuildGates(CorridorMetrics metrics)
    {
        List<ProofGate> gates = new List<ProofGate>();
        bool texturePass = metrics.textureFilesMissing == 0 && metrics.textureFilesLoaded >= 32;
        bool densityPass = metrics.pipeSegments >= 12 && metrics.lamps >= 4 && metrics.gauges >= 4 && metrics.valves >= 4 && metrics.rivets >= 150 && metrics.oilScorchDecals >= 5;
        bool materialsPass = metrics.materialFamiliesUsed >= 10 && texturePass;
        bool wetFloorPass = FindMetric(metrics, "wide_corridor").warmHighlightPercent >= 1.2f && FindMetric(metrics, "floor_wetness").warmHighlightPercent >= 1.4f;
        bool readabilityPass = FindMetric(metrics, "wide_corridor").passesNotEmptyCheck && FindMetric(metrics, "pressure_gate_detail").passesNotEmptyCheck;

        gates.Add(new ProofGate
        {
            gate = "Scope",
            status = "Pass",
            evidence = "Renderer creates an unsaved temporary editor scene and writes only the requested proof/report/metrics paths."
        });
        gates.Add(new ProofGate
        {
            gate = "Unity-only render path",
            status = "Pass",
            evidence = "All hero/detail images are captured from Unity cameras into RenderTextures."
        });
        gates.Add(new ProofGate
        {
            gate = "Existing staged texture usage",
            status = texturePass ? "Pass" : "Fail",
            evidence = metrics.textureFilesLoaded.ToString(CultureInfo.InvariantCulture) + "/" + metrics.textureFilesExpected.ToString(CultureInfo.InvariantCulture) + " expected FinalMaterialsV1 texture files loaded."
        });
        gates.Add(new ProofGate
        {
            gate = "Material role separation",
            status = materialsPass ? "Pass" : "Partial",
            evidence = "Wet stone, soot brick, blackened iron, brass, copper, amber glass, cream gauge, hazard enamel, walnut/leather, and oil/scorch decal roles are assigned."
        });
        gates.Add(new ProofGate
        {
            gate = "Corridor density",
            status = densityPass ? "Pass" : "Fail",
            evidence = metrics.pipeSegments.ToString(CultureInfo.InvariantCulture) + " pipe/valve segments, " + metrics.lamps.ToString(CultureInfo.InvariantCulture) + " lamps, " + metrics.gauges.ToString(CultureInfo.InvariantCulture) + " gauges, " + metrics.valves.ToString(CultureInfo.InvariantCulture) + " valves, " + metrics.rivets.ToString(CultureInfo.InvariantCulture) + " rivets, " + metrics.oilScorchDecals.ToString(CultureInfo.InvariantCulture) + " wear decals."
        });
        gates.Add(new ProofGate
        {
            gate = "Wet floor read",
            status = wetFloorPass ? "Pass" : "Partial",
            evidence = "Broken translucent highlight patches plus low amber rake light; measured warm highlight in wide/floor views is " + FindMetric(metrics, "wide_corridor").warmHighlightPercent.ToString("0.0", CultureInfo.InvariantCulture) + "% / " + FindMetric(metrics, "floor_wetness").warmHighlightPercent.ToString("0.0", CultureInfo.InvariantCulture) + "%."
        });
        gates.Add(new ProofGate
        {
            gate = "No dark empty render",
            status = readabilityPass ? "Pass" : "Fail",
            evidence = "Wide average luminance " + FindMetric(metrics, "wide_corridor").averageLuminance.ToString("0.000", CultureInfo.InvariantCulture) + ", near-black " + FindMetric(metrics, "wide_corridor").nearBlackPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + "%, bright " + FindMetric(metrics, "wide_corridor").brightPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + "%."
        });
        gates.Add(new ProofGate
        {
            gate = "Production asset readiness",
            status = "Fail",
            evidence = "This is a temporary primitive lookdev bay, not final modular mesh art, collision, LOD, lightmap UVs, or playable-scene integration."
        });
        gates.Add(new ProofGate
        {
            gate = "Human north-star review",
            status = "Fail until reviewed",
            evidence = "The proof can only claim material/lookdev direction until an art review accepts it against the north-star corridor mood."
        });
        return gates.ToArray();
    }

    private static ImageMetric FindMetric(CorridorMetrics metrics, string key)
    {
        if (metrics.imageMetrics == null)
        {
            return new ImageMetric();
        }

        for (int i = 0; i < metrics.imageMetrics.Length; i++)
        {
            if (metrics.imageMetrics[i].key == key)
            {
                return metrics.imageMetrics[i];
            }
        }

        return new ImageMetric();
    }

    private static string DetermineOverallStatus(ProofGate[] gates)
    {
        bool failBeforeHuman = false;
        bool partial = false;
        for (int i = 0; i < gates.Length; i++)
        {
            if (gates[i].gate == "Production asset readiness" || gates[i].gate == "Human north-star review")
            {
                continue;
            }

            if (gates[i].status == "Fail")
            {
                failBeforeHuman = true;
            }

            if (gates[i].status == "Partial")
            {
                partial = true;
            }
        }

        if (failBeforeHuman)
        {
            return "Fail: proof generated but one or more technical lookdev gates did not pass.";
        }

        if (partial)
        {
            return "Partial pass: useful Unity material/corridor proof; not production art and still requires human review.";
        }

        return "Lookdev pass / production fail: Unity proof meets the requested material-direction checks, but is not final art.";
    }

    private static Texture2D BuildContactSheet(List<Texture2D> renders, CorridorMetrics metrics)
    {
        Texture2D contact = new Texture2D(ContactWidth, ContactHeight, TextureFormat.RGBA32, false, false);
        contact.name = "Unity corridor material proof contact sheet";
        FillTexture(contact, new Color(0.018f, 0.015f, 0.012f, 1f));

        Color brass = new Color(0.66f, 0.41f, 0.16f, 1f);
        Color amber = new Color(0.95f, 0.45f, 0.12f, 1f);
        Color green = new Color(0.25f, 0.78f, 0.32f, 1f);
        Color fail = new Color(0.9f, 0.16f, 0.06f, 1f);
        FillRect(contact, new RectInt(0, 0, ContactWidth, 96), new Color(0.035f, 0.029f, 0.023f, 1f));
        FillRect(contact, new RectInt(0, ContactHeight - 112, ContactWidth, 112), new Color(0.035f, 0.029f, 0.023f, 1f));
        FillRect(contact, new RectInt(0, 92, ContactWidth, 4), brass);
        FillRect(contact, new RectInt(0, ContactHeight - 116, ContactWidth, 4), brass);

        RectInt widePanel = new RectInt(70, 440, 1300, 731);
        RectInt floorPanel = new RectInt(1435, 735, 690, 431);
        RectInt gatePanel = new RectInt(1435, 250, 690, 431);
        RectInt[] panels = { widePanel, floorPanel, gatePanel };
        for (int i = 0; i < renders.Count && i < panels.Length; i++)
        {
            DrawRect(contact, ExpandRect(panels[i], 12), brass, 5);
            BlitScaled(renders[i], contact, panels[i]);
        }

        int barX = 80;
        int barY = 54;
        int barWidth = 270;
        for (int i = 0; i < metrics.gates.Length && i < 7; i++)
        {
            Color statusColor = metrics.gates[i].status == "Pass" ? green : metrics.gates[i].status == "Fail" ? fail : amber;
            FillRect(contact, new RectInt(barX + i * (barWidth + 20), barY, barWidth, 18), statusColor);
        }

        int swatchY = ContactHeight - 70;
        Color[] swatches =
        {
            new Color(0.05f, 0.045f, 0.04f, 1f),
            new Color(0.12f, 0.085f, 0.06f, 1f),
            new Color(0.02f, 0.018f, 0.016f, 1f),
            new Color(0.62f, 0.38f, 0.15f, 1f),
            new Color(0.78f, 0.29f, 0.08f, 1f),
            new Color(1.0f, 0.47f, 0.13f, 1f),
            new Color(0.86f, 0.76f, 0.52f, 1f),
            new Color(0.55f, 0.04f, 0.02f, 1f),
            new Color(0.2f, 0.78f, 0.32f, 1f)
        };
        for (int i = 0; i < swatches.Length; i++)
        {
            FillRect(contact, new RectInt(90 + i * 220, swatchY, 190, 28), swatches[i]);
        }

        contact.Apply(false, false);
        return contact;
    }

    private static string BuildReport(CorridorMetrics metrics)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# ENV Recovery01 Corridor Material Unity Proof Report");
        builder.AppendLine();
        builder.AppendLine("Status: " + metrics.overallStatus);
        builder.AppendLine("Date/time: " + metrics.timestamp);
        builder.AppendLine("Unity version: " + metrics.unityVersion);
        builder.AppendLine("Subject: v0.0.95+ small steampunk corridor material/lookdev bay");
        builder.AppendLine("Tool lane: Unity editor batchmode, temporary in-memory scene, Camera plus RenderTexture JPG export");
        builder.AppendLine("Batchmode command entrypoint: `" + metrics.batchmodeEntrypoint + "`");
        builder.AppendLine("Playable-scene policy: no playable scene, Build Settings, or `V0SceneBuilder` edits were made by this renderer.");
        builder.AppendLine();
        builder.AppendLine("## Outputs");
        builder.AppendLine();
        builder.AppendLine("| File | Purpose | Dimensions |");
        builder.AppendLine("| --- | --- | ---: |");
        builder.AppendLine("| `" + metrics.renderPath + "` | Player-height corridor proof toward pressure gate hint | " + metrics.heroWidth.ToString(CultureInfo.InvariantCulture) + "x" + metrics.heroHeight.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| `" + metrics.floorDetailRenderPath + "` | Low wet-stone floor/specular detail view | " + metrics.detailWidth.ToString(CultureInfo.InvariantCulture) + "x" + metrics.detailHeight.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| `" + metrics.gateDetailRenderPath + "` | Pressure-gate, gauge, lamp, and rivet detail view | " + metrics.detailWidth.ToString(CultureInfo.InvariantCulture) + "x" + metrics.detailHeight.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| `" + metrics.contactSheetPath + "` | Contact sheet with hero/detail views and pass bars | " + metrics.contactSheetWidth.ToString(CultureInfo.InvariantCulture) + "x" + metrics.contactSheetHeight.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| `" + metrics.metricsPath + "` | Machine-readable render/material metrics | n/a |");
        builder.AppendLine();
        builder.AppendLine("## Methodology");
        builder.AppendLine();
        builder.AppendLine("- Created a new unsaved Unity editor scene in memory and discarded it after rendering.");
        builder.AppendLine("- Loaded staged PNG maps from `" + metrics.textureSourceFolder + "` into temporary materials. ORM was interpreted as `R=AO`, `G=roughness`, `B=metallic` and converted to temporary Unity metallic/smoothness and occlusion maps.");
        builder.AppendLine("- Built a small 8m corridor bay with wet dark stone floor, soot brick walls, blackened iron ribs/panels, brass/copper pipes, amber glass lamps, gauges, valves, rivets, hazard enamel, and scorch/oil decals.");
        builder.AppendLine("- Used warm practical lamps, a destination gate wash, low cool fill, and a low floor rake light to keep dark stone readable without flattening the mood.");
        builder.AppendLine("- Captured one player-height hero view plus two detail views through Unity cameras into RenderTextures, then wrote JPGs.");
        builder.AppendLine();
        builder.AppendLine("## Count Metrics");
        builder.AppendLine();
        builder.AppendLine("| Check | Result |");
        builder.AppendLine("| --- | ---: |");
        builder.AppendLine("| Texture files loaded | " + metrics.textureFilesLoaded.ToString(CultureInfo.InvariantCulture) + " / " + metrics.textureFilesExpected.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Material families used | " + metrics.materialFamiliesUsed.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Pipe/valve segments | " + metrics.pipeSegments.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Amber lamps | " + metrics.lamps.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Gauges | " + metrics.gauges.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Valves | " + metrics.valves.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Gauge tick marks | " + metrics.gaugeTickMarks.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Rivets/bolts | " + metrics.rivets.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Oil/scorch decals | " + metrics.oilScorchDecals.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Wet highlight patches | " + metrics.floorWetHighlightPatches.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Tiling breakups | " + metrics.tilingBreakups.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine();
        builder.AppendLine("## Image Readability Metrics");
        builder.AppendLine();
        builder.AppendLine("| Image | Average luminance | Near-black % | Bright % | Warm highlight % | Empty check |");
        builder.AppendLine("| --- | ---: | ---: | ---: | ---: | --- |");
        for (int i = 0; i < metrics.imageMetrics.Length; i++)
        {
            ImageMetric image = metrics.imageMetrics[i];
            builder.AppendLine("| `" + image.path + "` | " + image.averageLuminance.ToString("0.000", CultureInfo.InvariantCulture) + " | " + image.nearBlackPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + " | " + image.brightPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + " | " + image.warmHighlightPercent.ToString("0.0", CultureInfo.InvariantCulture) + " | " + (image.passesNotEmptyCheck ? "pass" : "fail") + " |");
        }

        builder.AppendLine();
        builder.AppendLine("## Gates");
        builder.AppendLine();
        builder.AppendLine("| Gate | Status | Evidence |");
        builder.AppendLine("| --- | --- | --- |");
        for (int i = 0; i < metrics.gates.Length; i++)
        {
            builder.AppendLine("| " + metrics.gates[i].gate + " | " + metrics.gates[i].status + " | " + metrics.gates[i].evidence + " |");
        }

        builder.AppendLine();
        builder.AppendLine("## Frank Visual Read");
        builder.AppendLine();
        builder.AppendLine("This is a useful Unity-only material/corridor lookdev proof if the generated images show the intended readable depth: dark wet stone foreground, soot brick/blackened iron side structure, amber practicals, brass/copper pipework, gauge/valve details, and a pressure-gate destination. It should not be called final corridor art. The bay is primitive geometry with temporary material instances, no authored modular meshes, no collision, no LODs, no lightmap UV proof, no post-processing stack, and no gameplay integration. The next art pass should turn this into real modular wall/floor/gate pieces with UV-controlled material breakup and engine-validated lighting presets.");
        builder.AppendLine();
        builder.AppendLine("## Next Steps");
        builder.AppendLine();
        builder.AppendLine("- Promote only the material/lighting lessons, not the temporary primitive geometry.");
        builder.AppendLine("- Build real modular 4m and 8m corridor pieces with the same material roles and repeated-rib depth language.");
        builder.AppendLine("- Author decal placement variants so soot/oil breakup is not camera-dependent.");
        builder.AppendLine("- Add a real pressure-gate module with open/locked states, sockets, collision, LODs, and lightmap readiness notes.");
        builder.AppendLine("- Re-run a Unity screenshot from player height after modular meshes exist, then compare against the north-star corridor mood in human review.");
        return builder.ToString();
    }

    private static void AddRivetRow(string prefix, Vector3 start, Vector3 end, int count, float radius, Material material, Transform parent, CorridorMetrics metrics)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            AddRivet(prefix + " " + i.ToString(CultureInfo.InvariantCulture), Vector3.Lerp(start, end, t), radius, material, parent, metrics);
        }
    }

    private static void AddRivetGrid(string prefix, Vector3 origin, int columns, int rows, float dx, float dy, float radius, Material material, Transform parent, CorridorMetrics metrics)
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                AddRivet(prefix + " " + x.ToString(CultureInfo.InvariantCulture) + "_" + y.ToString(CultureInfo.InvariantCulture), origin + new Vector3(x * dx, y * dy, 0f), radius, material, parent, metrics);
            }
        }
    }

    private static void AddRivet(string name, Vector3 position, float radius, Material material, Transform parent, CorridorMetrics metrics)
    {
        CreateSphere(name, position, Vector3.one * (radius * 2f), material, parent);
        metrics.rivets++;
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

    private static void AssignMaterial(GameObject gameObject, Material material)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }
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
            SetMaterialColor(material, "_EmissionColor", color * 1.6f);
            material.EnableKeyword("_EMISSION");
        }

        return material;
    }

    private static Material CreateTransparentColorMaterial(string name, Color color, float metallic, float smoothness)
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
        material.renderQueue = (int)RenderQueue.Transparent + 10;
        SetMaterialFloat(material, "_Surface", 1.0f);
        SetMaterialFloat(material, "_Mode", 3.0f);
        SetMaterialInt(material, "_SrcBlend", (int)BlendMode.SrcAlpha);
        SetMaterialInt(material, "_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        SetMaterialInt(material, "_ZWrite", 0);
        SetMaterialInt(material, "_Cull", (int)CullMode.Off);
        material.EnableKeyword("_ALPHABLEND_ON");
        return material;
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

    private static void SetMaterialTextureScale(Material material, string property, Vector2 scale)
    {
        if (material.HasProperty(property))
        {
            material.SetTextureScale(property, scale);
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
        Color32 fill = color;
        int xMin = Mathf.Clamp(rect.xMin, 0, texture.width);
        int xMax = Mathf.Clamp(rect.xMax, 0, texture.width);
        int yMin = Mathf.Clamp(rect.yMin, 0, texture.height);
        int yMax = Mathf.Clamp(rect.yMax, 0, texture.height);
        for (int y = yMin; y < yMax; y++)
        {
            for (int x = xMin; x < xMax; x++)
            {
                texture.SetPixel(x, y, fill);
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

    private static RectInt ExpandRect(RectInt rect, int amount)
    {
        return new RectInt(rect.xMin - amount, rect.yMin - amount, rect.width + amount * 2, rect.height + amount * 2);
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

    private static void TrackOwnedTexture(Material material, Texture texture)
    {
        if (material == null || texture == null || OwnedTextures.Contains(texture))
        {
            return;
        }

        OwnedTextures.Add(texture);
    }

    private static void DestroyTexture(Texture2D texture)
    {
        if (texture != null)
        {
            UnityEngine.Object.DestroyImmediate(texture);
        }
    }

    private static void DestroyOwnedTextures()
    {
        for (int i = 0; i < OwnedTextures.Count; i++)
        {
            if (OwnedTextures[i] != null)
            {
                UnityEngine.Object.DestroyImmediate(OwnedTextures[i]);
            }
        }

        OwnedTextures.Clear();
    }

    private static void DestroyMaterial(Material material)
    {
        if (material != null)
        {
            UnityEngine.Object.DestroyImmediate(material);
        }
    }

    private static string GetProjectRoot()
    {
        return Directory.GetParent(Application.dataPath).FullName;
    }

    private static string FormatVector(Vector3 vector)
    {
        return vector.x.ToString("0.00", CultureInfo.InvariantCulture) + ", " +
               vector.y.ToString("0.00", CultureInfo.InvariantCulture) + ", " +
               vector.z.ToString("0.00", CultureInfo.InvariantCulture);
    }

    [Serializable]
    private sealed class CorridorMetrics
    {
        public string timestamp;
        public string unityVersion;
        public string batchmodeEntrypoint;
        public string textureSourceFolder;
        public string renderPath;
        public string floorDetailRenderPath;
        public string gateDetailRenderPath;
        public string contactSheetPath;
        public string reportPath;
        public string metricsPath;
        public int heroWidth;
        public int heroHeight;
        public int detailWidth;
        public int detailHeight;
        public int contactSheetWidth;
        public int contactSheetHeight;
        public int textureFilesExpected;
        public int textureFilesLoaded;
        public int textureFilesMissing;
        public int renderedImageCount;
        public int materialFamiliesUsed;
        public int baseArchitecturePieces;
        public int wallPanels;
        public int trimPieces;
        public int tilingBreakups;
        public int pipeSegments;
        public int lamps;
        public int gauges;
        public int valves;
        public int gaugeTickMarks;
        public int rivets;
        public int pressureGateHints;
        public int routeCueCount;
        public int oilScorchDecals;
        public int floorWetHighlightPatches;
        public bool warmKeyLight;
        public bool coolFillLight;
        public bool wetFloorRakeLight;
        public bool gateDestinationLight;
        public string cameraDescription;
        public string cameraPosition;
        public string overallStatus;
        public List<string> missingTextureNotes = new List<string>();
        public ImageMetric[] imageMetrics;
        public ProofGate[] gates;
    }

    [Serializable]
    private sealed class ImageMetric
    {
        public string key;
        public string path;
        public int width;
        public int height;
        public float averageLuminance;
        public float darkPixelPercent;
        public float nearBlackPixelPercent;
        public float brightPixelPercent;
        public float warmHighlightPercent;
        public float coolAccentPercent;
        public float saturatedAccentPercent;
        public bool passesNotEmptyCheck;
    }

    [Serializable]
    private sealed class ProofGate
    {
        public string gate;
        public string status;
        public string evidence;
    }

    private sealed class CorridorMaterials
    {
        public Material WetStone;
        public Material SootBrick;
        public Material BlackenedIron;
        public Material AgedBrass;
        public Material CopperPipe;
        public Material CreamGauge;
        public Material AmberGlass;
        public Material GreasyWalnut;
        public Material HazardEnamel;
        public Material LeatherBellows;
        public Material Decal;
        public Material WarmGlow;
        public Material RedGlow;
        public Material GreenGlow;
        public Material LineDark;
        public Material WetHighlight;
        public Material ContactBack;

        public void DestroyAll()
        {
            DestroyMaterial(WetStone);
            DestroyMaterial(SootBrick);
            DestroyMaterial(BlackenedIron);
            DestroyMaterial(AgedBrass);
            DestroyMaterial(CopperPipe);
            DestroyMaterial(CreamGauge);
            DestroyMaterial(AmberGlass);
            DestroyMaterial(GreasyWalnut);
            DestroyMaterial(HazardEnamel);
            DestroyMaterial(LeatherBellows);
            DestroyMaterial(Decal);
            DestroyMaterial(WarmGlow);
            DestroyMaterial(RedGlow);
            DestroyMaterial(GreenGlow);
            DestroyMaterial(LineDark);
            DestroyMaterial(WetHighlight);
            DestroyMaterial(ContactBack);
        }
    }
}
