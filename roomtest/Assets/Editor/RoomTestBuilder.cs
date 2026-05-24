using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RoomTestBuilder
{
    private const int TextureSize = 2048;
    private const int RenderWidth = 1920;
    private const int RenderHeight = 1080;
    private const string AssetRoot = "Assets/RoomTest";
    private const string TextureRoot = AssetRoot + "/Textures";
    private const string MaterialRoot = AssetRoot + "/Materials";
    private const string MeshRoot = AssetRoot + "/Meshes";
    private const string PrefabRoot = AssetRoot + "/Prefabs";
    private const string SceneRoot = AssetRoot + "/Scenes";
    private const string ScenePath = SceneRoot + "/Roomtest_BrickLighting.unity";
    private const string ScenePathV02 = SceneRoot + "/Roomtest_BrickLighting_v0.2.unity";
    private const string PreviewScenePathV02 = SceneRoot + "/Roomtest_MaterialBlockPreview_v0.2.unity";
    private const string RenderFileName = "roomtest_brick_lighting_v0.1.png";
    private const string MetricsFileName = "roomtest_metrics_v0.1.json";
    private const string RenderFileNameV02 = "roomtest_brick_lighting_v0.2.png";
    private const string PreviewRenderFileNameV02 = "roomtest_material_block_preview_v0.2.png";
    private const string MetricsFileNameV02 = "roomtest_metrics_v0.2.json";

    private enum SurfaceKind
    {
        WallBrick,
        FloorSlab,
        CeilingBrick
    }

    [MenuItem("Roomtest/Build And Render Reference Room")]
    public static void BuildAndRenderReferenceRoom()
    {
        EnsureFolders();
        ConfigureProjectForLookdev();

        SurfaceTextureSet wallTextures = GenerateSurfaceTextureSet(
            "RT_DarkBrickWall",
            SurfaceKind.WallBrick,
            new Color(0.22f, 0.18f, 0.13f),
            new Color(0.055f, 0.048f, 0.04f),
            0.36f,
            0.08f,
            17);
        SurfaceTextureSet floorTextures = GenerateSurfaceTextureSet(
            "RT_WetStoneFloor",
            SurfaceKind.FloorSlab,
            new Color(0.19f, 0.16f, 0.12f),
            new Color(0.05f, 0.045f, 0.04f),
            0.68f,
            0.18f,
            29);
        SurfaceTextureSet ceilingTextures = GenerateSurfaceTextureSet(
            "RT_SootBrickCeiling",
            SurfaceKind.CeilingBrick,
            new Color(0.18f, 0.145f, 0.105f),
            new Color(0.045f, 0.04f, 0.035f),
            0.28f,
            0.06f,
            43);

        Material wallMaterial = CreatePbrMaterial("RT_MAT_DarkBrickWall", wallTextures, new Vector2(3.6f, 2.35f), 0.32f, 0.035f);
        Material floorMaterial = CreatePbrMaterial("RT_MAT_WetStoneFloor", floorTextures, new Vector2(2.3f, 2.35f), 0.68f, 0.045f);
        Material ceilingMaterial = CreatePbrMaterial("RT_MAT_SootBrickCeiling", ceilingTextures, new Vector2(3.2f, 2.05f), 0.25f, 0.03f);
        Material lampMaterial = CreateEmissiveMaterial("RT_MAT_WarmGaslightGlass", new Color(1f, 0.65f, 0.32f), 2.6f);
        Material ironMaterial = CreateSimpleMaterial("RT_MAT_DarkIronFixture", new Color(0.055f, 0.048f, 0.042f), 0.12f, 0.28f);

        Mesh brickMesh = CreateBeveledBrickMesh();
        CreateBrickPrefab(brickMesh, wallMaterial);
        BuildScene(wallMaterial, floorMaterial, ceilingMaterial, lampMaterial, ironMaterial);

        string renderPath = RenderScene();
        WriteMetrics(renderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ROOMTEST_RENDER_COMPLETE " + renderPath);
    }

    [MenuItem("Roomtest/Build And Render Reference Room v0.2")]
    public static void BuildAndRenderReferenceRoomV02()
    {
        EnsureFolders();
        ConfigureProjectForLookdev();

        SurfaceTextureSet wallTextures = GenerateSurfaceTextureSetV02(
            "RTv02_DarkBrownBlackBrickWall",
            SurfaceKind.WallBrick,
            new Color(0.105f, 0.083f, 0.061f),
            new Color(0.021f, 0.019f, 0.016f),
            0.18f,
            0.025f,
            117);
        SurfaceTextureSet floorTextures = GenerateSurfaceTextureSetV02(
            "RTv02_WetBlackStoneFloor",
            SurfaceKind.FloorSlab,
            new Color(0.118f, 0.098f, 0.076f),
            new Color(0.026f, 0.023f, 0.02f),
            0.58f,
            0.25f,
            129);
        SurfaceTextureSet ceilingTextures = GenerateSurfaceTextureSetV02(
            "RTv02_SootedBrickCeiling",
            SurfaceKind.CeilingBrick,
            new Color(0.082f, 0.066f, 0.049f),
            new Color(0.018f, 0.016f, 0.014f),
            0.11f,
            0.01f,
            143);

        Material wallMaterial = CreatePbrMaterial("RTv02_MAT_DarkBrownBlackBrickWall", wallTextures, new Vector2(3.9f, 2.65f), 0.18f, 0.035f);
        Material floorMaterial = CreatePbrMaterial("RTv02_MAT_WetBlackStoneFloor", floorTextures, new Vector2(2.05f, 2.15f), 0.61f, 0.045f);
        Material ceilingMaterial = CreatePbrMaterial("RTv02_MAT_SootedBrickCeiling", ceilingTextures, new Vector2(3.6f, 2.35f), 0.11f, 0.028f);
        SetFloat(wallMaterial, "_BumpScale", 0.74f);
        SetFloat(floorMaterial, "_BumpScale", 0.82f);
        SetFloat(ceilingMaterial, "_BumpScale", 0.6f);

        Material lampMaterial = CreateEmissiveMaterial("RTv02_MAT_ContainedAmberLampGlass", new Color(1f, 0.58f, 0.25f), 3.4f);
        Material ironMaterial = CreateSimpleMaterial("RTv02_MAT_BlackenedIronFixture", new Color(0.025f, 0.022f, 0.019f), 0.18f, 0.23f);

        BuildMaterialBlockPreviewV02(wallMaterial, floorMaterial, ceilingMaterial);
        string previewRenderPath = RenderSceneToFile(PreviewRenderFileNameV02);

        BuildSceneV02(wallMaterial, floorMaterial, ceilingMaterial, lampMaterial, ironMaterial);
        string renderPath = RenderSceneToFile(RenderFileNameV02);
        WriteMetricsToFile(renderPath, RenderFileNameV02, MetricsFileNameV02);
        WriteV02ReviewNote(previewRenderPath, renderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ROOMTEST_RENDER_V02_COMPLETE " + renderPath);
    }

    private static void EnsureFolders()
    {
        CreateAssetFolder("Assets", "RoomTest");
        CreateAssetFolder(AssetRoot, "Textures");
        CreateAssetFolder(AssetRoot, "Materials");
        CreateAssetFolder(AssetRoot, "Meshes");
        CreateAssetFolder(AssetRoot, "Prefabs");
        CreateAssetFolder(AssetRoot, "Scenes");
        Directory.CreateDirectory(GetProjectPath("Renders"));
    }

    private static void CreateAssetFolder(string parent, string child)
    {
        string path = parent + "/" + child;
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder(parent, child);
        }
    }

    private static void ConfigureProjectForLookdev()
    {
        PlayerSettings.colorSpace = ColorSpace.Linear;
        QualitySettings.antiAliasing = 8;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
        QualitySettings.shadows = ShadowQuality.All;
        QualitySettings.shadowDistance = 24f;
    }

    private static SurfaceTextureSet GenerateSurfaceTextureSet(string name, SurfaceKind kind, Color brickBase, Color mortarBase, float smoothnessBase, float wetness, int seed)
    {
        float[] height = new float[TextureSize * TextureSize];
        Color32[] albedoPixels = new Color32[height.Length];
        Color32[] heightPixels = new Color32[height.Length];
        Color32[] aoPixels = new Color32[height.Length];
        Color32[] metallicSmoothPixels = new Color32[height.Length];

        for (int y = 0; y < TextureSize; y++)
        {
            float v = (y + 0.5f) / TextureSize;
            for (int x = 0; x < TextureSize; x++)
            {
                float u = (x + 0.5f) / TextureSize;
                BrickSample sample = SampleBrickPattern(kind, u, v, seed);
                float fineNoise = Fbm(u * 72f, v * 72f, seed + 3, 4);
                float broadNoise = Fbm(u * 9f, v * 9f, seed + 13, 4);
                float scratchNoise = Fbm(u * 190f + 7.1f, v * 190f + 1.3f, seed + 31, 3);
                float edgeHighlight = Mathf.Pow(Mathf.Clamp01(sample.edgeDistance / 0.12f), 0.7f);
                float crack = scratchNoise > 0.69f && sample.edgeDistance > 0.2f ? 0.12f : 0f;
                float chipped = fineNoise > 0.78f && sample.edgeDistance < 0.34f ? 0.12f : 0f;

                float surfaceHeight = sample.isMortar
                    ? 0.11f + broadNoise * 0.045f
                    : 0.48f + edgeHighlight * 0.34f + broadNoise * 0.16f + fineNoise * 0.055f - crack - chipped;
                surfaceHeight = Mathf.Clamp01(surfaceHeight);
                height[y * TextureSize + x] = surfaceHeight;

                Color baseColor = sample.isMortar
                    ? mortarBase * Mathf.Lerp(0.8f, 1.18f, broadNoise)
                    : brickBase * Mathf.Lerp(0.78f, 1.42f, sample.brickTone * 0.58f + broadNoise * 0.42f);
                baseColor += new Color(0.08f, 0.045f, 0.018f) * (edgeHighlight * 0.35f + wetness * 0.18f);
                baseColor *= 1f - crack * 1.6f - chipped * 0.65f;
                if (sample.isMortar)
                {
                    baseColor *= 0.72f;
                }

                int index = y * TextureSize + x;
                albedoPixels[index] = ToColor32(baseColor, 1f);
                byte heightByte = (byte)Mathf.RoundToInt(surfaceHeight * 255f);
                heightPixels[index] = new Color32(heightByte, heightByte, heightByte, 255);

                float ao = sample.isMortar ? 0.42f : Mathf.Lerp(0.58f, 0.94f, Mathf.Clamp01(sample.edgeDistance * 3.4f));
                ao *= Mathf.Lerp(0.78f, 1f, broadNoise);
                byte aoByte = (byte)Mathf.RoundToInt(Mathf.Clamp01(ao) * 255f);
                aoPixels[index] = new Color32(aoByte, aoByte, aoByte, 255);

                float smoothness = sample.isMortar
                    ? smoothnessBase * 0.35f
                    : smoothnessBase + wetness * Mathf.Lerp(0.05f, 0.24f, fineNoise) + (1f - Mathf.Clamp01(sample.edgeDistance * 4f)) * 0.08f;
                smoothness = Mathf.Clamp01(smoothness);
                metallicSmoothPixels[index] = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(smoothness * 255f));
            }
        }

        Color32[] normalPixels = GenerateNormalPixels(height, kind == SurfaceKind.FloorSlab ? 8.5f : 7.2f);

        string albedoPath = SaveTexture(name + "_Albedo", albedoPixels, TextureImporterType.Default, true, true);
        string normalPath = SaveTexture(name + "_Normal", normalPixels, TextureImporterType.NormalMap, false, true);
        string heightPath = SaveTexture(name + "_Height", heightPixels, TextureImporterType.Default, false, true);
        string aoPath = SaveTexture(name + "_Occlusion", aoPixels, TextureImporterType.Default, false, true);
        string metallicSmoothPath = SaveTexture(name + "_MetallicSmoothness", metallicSmoothPixels, TextureImporterType.Default, false, true);
        return new SurfaceTextureSet(albedoPath, normalPath, heightPath, aoPath, metallicSmoothPath);
    }

    private static SurfaceTextureSet GenerateSurfaceTextureSetV02(string name, SurfaceKind kind, Color brickBase, Color mortarBase, float smoothnessBase, float wetness, int seed)
    {
        float[] height = new float[TextureSize * TextureSize];
        Color32[] albedoPixels = new Color32[height.Length];
        Color32[] heightPixels = new Color32[height.Length];
        Color32[] aoPixels = new Color32[height.Length];
        Color32[] metallicSmoothPixels = new Color32[height.Length];

        for (int y = 0; y < TextureSize; y++)
        {
            float v = (y + 0.5f) / TextureSize;
            for (int x = 0; x < TextureSize; x++)
            {
                float u = (x + 0.5f) / TextureSize;
                BrickSampleV02 sample = SampleBrickPatternV02(kind, u, v, seed);
                float fineNoise = Fbm(u * 96f + 3.1f, v * 96f + 5.2f, seed + 3, 4);
                float broadNoise = Fbm(u * 8.5f + 9.3f, v * 8.5f + 1.7f, seed + 13, 5);
                float mineralNoise = Fbm(u * 230f + 1.4f, v * 190f + 6.8f, seed + 31, 3);
                float sootNoise = Fbm(u * 4.5f + 0.9f, v * 7.2f + 2.2f, seed + 47, 4);
                float crackNoise = Fbm(u * 165f + 11.5f, v * 132f + 4.4f, seed + 61, 2);
                float edgeWear = Mathf.Clamp01(1f - sample.edgeDistance * (kind == SurfaceKind.FloorSlab ? 4.2f : 5.4f));
                edgeWear *= Mathf.Lerp(0.38f, 1.3f, sample.irregularity);
                float chips = Mathf.Clamp01((fineNoise - 0.68f) * 3.5f) * edgeWear;
                float hairlineCrack = crackNoise > 0.735f && sample.edgeDistance > 0.17f ? Mathf.Lerp(0.04f, 0.15f, mineralNoise) : 0f;

                float surfaceHeight = sample.isMortar
                    ? 0.055f + broadNoise * 0.038f
                    : 0.44f + broadNoise * 0.18f + fineNoise * 0.045f - edgeWear * 0.13f - chips * 0.16f - hairlineCrack;
                if (kind == SurfaceKind.FloorSlab)
                {
                    surfaceHeight += wetness * 0.035f;
                }

                surfaceHeight = Mathf.Clamp01(surfaceHeight);
                int index = y * TextureSize + x;
                height[index] = surfaceHeight;

                Color baseColor = sample.isMortar
                    ? mortarBase * Mathf.Lerp(0.58f, 1.1f, broadNoise)
                    : brickBase * Mathf.Lerp(0.62f, 1.24f, sample.brickTone * 0.5f + broadNoise * 0.5f);
                baseColor += new Color(0.028f, 0.018f, 0.009f) * Mathf.Lerp(0.15f, 0.65f, fineNoise);
                baseColor += new Color(0.012f, 0.014f, 0.015f) * Mathf.Lerp(0f, 0.38f, mineralNoise);
                baseColor *= Mathf.Lerp(0.64f, 1f, 1f - sample.stain);
                baseColor *= Mathf.Lerp(0.72f, 0.98f, 1f - sootNoise * (kind == SurfaceKind.CeilingBrick ? 0.95f : 0.45f));
                baseColor *= 1f - chips * 0.32f - hairlineCrack * 2.1f;
                if (sample.isMortar)
                {
                    baseColor *= Mathf.Lerp(0.56f, 0.85f, sample.irregularity);
                }

                albedoPixels[index] = ToColor32(baseColor, 1f);
                byte heightByte = (byte)Mathf.RoundToInt(surfaceHeight * 255f);
                heightPixels[index] = new Color32(heightByte, heightByte, heightByte, 255);

                float ao = sample.isMortar
                    ? Mathf.Lerp(0.18f, 0.38f, broadNoise)
                    : Mathf.Lerp(0.52f, 0.9f, Mathf.Clamp01(sample.edgeDistance * 3.1f));
                ao *= Mathf.Lerp(0.74f, 1f, 1f - sample.stain);
                ao -= hairlineCrack * 0.9f;
                byte aoByte = (byte)Mathf.RoundToInt(Mathf.Clamp01(ao) * 255f);
                aoPixels[index] = new Color32(aoByte, aoByte, aoByte, 255);

                float puddleSheen = kind == SurfaceKind.FloorSlab ? Mathf.SmoothStep(0.46f, 0.82f, Fbm(u * 5.3f, v * 6.2f, seed + 79, 3)) : 0f;
                float smoothness = sample.isMortar
                    ? smoothnessBase * 0.24f
                    : smoothnessBase + wetness * Mathf.Lerp(0.05f, 0.34f, puddleSheen) + fineNoise * 0.035f - chips * 0.07f;
                if (kind != SurfaceKind.FloorSlab)
                {
                    smoothness -= edgeWear * 0.035f;
                }

                smoothness = Mathf.Clamp01(smoothness);
                metallicSmoothPixels[index] = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(smoothness * 255f));
            }
        }

        float normalStrength = kind == SurfaceKind.FloorSlab ? 5.9f : kind == SurfaceKind.CeilingBrick ? 4.15f : 4.7f;
        Color32[] normalPixels = GenerateNormalPixels(height, normalStrength);

        string albedoPath = SaveTexture(name + "_Albedo", albedoPixels, TextureImporterType.Default, true, true);
        string normalPath = SaveTexture(name + "_Normal", normalPixels, TextureImporterType.NormalMap, false, true);
        string heightPath = SaveTexture(name + "_Height", heightPixels, TextureImporterType.Default, false, true);
        string aoPath = SaveTexture(name + "_Occlusion", aoPixels, TextureImporterType.Default, false, true);
        string metallicSmoothPath = SaveTexture(name + "_MetallicSmoothness", metallicSmoothPixels, TextureImporterType.Default, false, true);
        return new SurfaceTextureSet(albedoPath, normalPath, heightPath, aoPath, metallicSmoothPath);
    }

    private static BrickSample SampleBrickPattern(SurfaceKind kind, float u, float v, int seed)
    {
        float columns = kind == SurfaceKind.FloorSlab ? 6f : 13f;
        float rows = kind == SurfaceKind.FloorSlab ? 6f : 9f;
        float mortarX = kind == SurfaceKind.FloorSlab ? 0.032f : 0.043f;
        float mortarY = kind == SurfaceKind.FloorSlab ? 0.034f : 0.052f;
        float rowFloat = v * rows;
        int row = Mathf.FloorToInt(rowFloat);
        float shiftedU = Frac(u + ((row & 1) == 0 ? 0f : 0.5f / columns));
        float colFloat = shiftedU * columns;
        int col = Mathf.FloorToInt(colFloat);
        float inBrickX = Frac(colFloat);
        float inBrickY = Frac(rowFloat);

        float wiggle = (Fbm(u * 36f, v * 36f, seed + 101, 2) - 0.5f) * 0.018f;
        float edgeDistance = Mathf.Min(Mathf.Min(inBrickX, 1f - inBrickX), Mathf.Min(inBrickY, 1f - inBrickY)) + wiggle;
        bool mortar = inBrickX < mortarX || inBrickX > 1f - mortarX || inBrickY < mortarY || inBrickY > 1f - mortarY;
        float brickTone = Hash01(col, row, seed);
        return new BrickSample(mortar, Mathf.Clamp01(edgeDistance), brickTone);
    }

    private static BrickSampleV02 SampleBrickPatternV02(SurfaceKind kind, float u, float v, int seed)
    {
        float columns = kind == SurfaceKind.FloorSlab ? 5.35f : 12.6f;
        float rows = kind == SurfaceKind.FloorSlab ? 5.2f : 8.9f;
        float mortarX = kind == SurfaceKind.FloorSlab ? 0.026f : 0.033f;
        float mortarY = kind == SurfaceKind.FloorSlab ? 0.028f : 0.038f;
        float courseWarp = (Fbm(u * 2.7f, v * 6.8f, seed + 101, 3) - 0.5f) * 0.09f;
        float rowFloat = v * rows + courseWarp;
        int row = Mathf.FloorToInt(rowFloat);
        float rowShift = ((row & 1) == 0 ? 0f : 0.5f / columns) + (Fbm(row * 0.37f, seed * 0.013f, seed + 103, 2) - 0.5f) * 0.032f;
        float shiftedU = Frac(u + rowShift + (Fbm(u * 3.2f, v * 4.1f, seed + 107, 3) - 0.5f) * 0.016f);
        float colFloat = shiftedU * columns + (Fbm(u * 8.2f, v * 5.6f, seed + 109, 2) - 0.5f) * 0.055f;
        int col = Mathf.FloorToInt(colFloat);
        float inBrickX = Frac(colFloat);
        float inBrickY = Frac(rowFloat);
        float jointNoise = Fbm(u * 44f + 2.3f, v * 44f + 5.7f, seed + 113, 3);
        float localMortarX = mortarX * Mathf.Lerp(0.58f, 1.72f, jointNoise);
        float localMortarY = mortarY * Mathf.Lerp(0.62f, 1.58f, Fbm(u * 37f, v * 51f, seed + 127, 3));
        bool mortar = inBrickX < localMortarX || inBrickX > 1f - localMortarX || inBrickY < localMortarY || inBrickY > 1f - localMortarY;
        float edgeDistance = Mathf.Min(Mathf.Min(inBrickX, 1f - inBrickX), Mathf.Min(inBrickY, 1f - inBrickY));
        float brickTone = Hash01(col, row, seed);
        float irregularity = Fbm(col * 1.7f + u * 8f, row * 1.3f + v * 8f, seed + 131, 3);
        float stain = Mathf.SmoothStep(0.48f, 0.92f, Fbm(u * 5.1f + 1.1f, v * 7.4f + 8.3f, seed + 139, 4));
        return new BrickSampleV02(mortar, Mathf.Clamp01(edgeDistance), brickTone, irregularity, stain);
    }

    private static Color32[] GenerateNormalPixels(float[] height, float strength)
    {
        Color32[] pixels = new Color32[height.Length];
        for (int y = 0; y < TextureSize; y++)
        {
            int ym = (y - 1 + TextureSize) % TextureSize;
            int yp = (y + 1) % TextureSize;
            for (int x = 0; x < TextureSize; x++)
            {
                int xm = (x - 1 + TextureSize) % TextureSize;
                int xp = (x + 1) % TextureSize;
                float dx = height[y * TextureSize + xp] - height[y * TextureSize + xm];
                float dy = height[yp * TextureSize + x] - height[ym * TextureSize + x];
                Vector3 normal = new Vector3(-dx * strength, -dy * strength, 1f).normalized;
                pixels[y * TextureSize + x] = new Color32(
                    (byte)Mathf.RoundToInt((normal.x * 0.5f + 0.5f) * 255f),
                    (byte)Mathf.RoundToInt((normal.y * 0.5f + 0.5f) * 255f),
                    (byte)Mathf.RoundToInt((normal.z * 0.5f + 0.5f) * 255f),
                    255);
            }
        }

        return pixels;
    }

    private static string SaveTexture(string name, Color32[] pixels, TextureImporterType importerType, bool sRgb, bool repeat)
    {
        Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, true, !sRgb);
        texture.SetPixels32(pixels);
        texture.Apply(true, false);

        string assetPath = TextureRoot + "/" + name + ".png";
        File.WriteAllBytes(GetProjectPath(assetPath), texture.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = importerType;
            importer.sRGBTexture = sRgb;
            importer.mipmapEnabled = true;
            importer.wrapMode = repeat ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Trilinear;
            importer.anisoLevel = 9;
            importer.maxTextureSize = TextureSize;
            importer.SaveAndReimport();
        }

        return assetPath;
    }

    private static Material CreatePbrMaterial(string name, SurfaceTextureSet textures, Vector2 tiling, float smoothness, float parallax)
    {
        Material material = CreateOrLoadMaterial(name);
        Texture2D albedo = LoadTexture(textures.AlbedoPath);
        Texture2D normal = LoadTexture(textures.NormalPath);
        Texture2D height = LoadTexture(textures.HeightPath);
        Texture2D occlusion = LoadTexture(textures.OcclusionPath);
        Texture2D metallicSmoothness = LoadTexture(textures.MetallicSmoothnessPath);

        SetTexture(material, "_MainTex", albedo, tiling);
        SetTexture(material, "_BaseMap", albedo, tiling);
        SetTexture(material, "_BumpMap", normal, tiling);
        SetTexture(material, "_ParallaxMap", height, tiling);
        SetTexture(material, "_OcclusionMap", occlusion, tiling);
        SetTexture(material, "_MetallicGlossMap", metallicSmoothness, tiling);

        SetFloat(material, "_Metallic", 0f);
        SetFloat(material, "_Glossiness", smoothness);
        SetFloat(material, "_Smoothness", smoothness);
        SetFloat(material, "_GlossMapScale", 1f);
        SetFloat(material, "_BumpScale", 1.05f);
        SetFloat(material, "_Parallax", parallax);
        material.mainTexture = albedo;
        material.mainTextureScale = tiling;
        material.EnableKeyword("_NORMALMAP");
        material.EnableKeyword("_PARALLAXMAP");
        material.EnableKeyword("_METALLICGLOSSMAP");
        material.EnableKeyword("_OCCLUSIONMAP");
        EditorUtility.SetDirty(material);
        return material;
    }

    private static Material CreateSimpleMaterial(string name, Color color, float metallic, float smoothness)
    {
        Material material = CreateOrLoadMaterial(name);
        SetColor(material, "_Color", color);
        SetColor(material, "_BaseColor", color);
        SetFloat(material, "_Metallic", metallic);
        SetFloat(material, "_Glossiness", smoothness);
        SetFloat(material, "_Smoothness", smoothness);
        EditorUtility.SetDirty(material);
        return material;
    }

    private static Material CreateEmissiveMaterial(string name, Color color, float intensity)
    {
        Material material = CreateOrLoadMaterial(name);
        Color emission = color * intensity;
        SetColor(material, "_Color", color);
        SetColor(material, "_BaseColor", color);
        SetColor(material, "_EmissionColor", emission);
        material.EnableKeyword("_EMISSION");
        EditorUtility.SetDirty(material);
        return material;
    }

    private static Material CreateOrLoadMaterial(string name)
    {
        string path = MaterialRoot + "/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material != null)
        {
            return material;
        }

        Shader shader = Shader.Find("Standard");
        if (shader == null)
        {
            shader = Shader.Find("Universal Render Pipeline/Lit");
        }

        if (shader == null)
        {
            shader = Shader.Find("Diffuse");
        }

        material = new Material(shader);
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static Texture2D LoadTexture(string assetPath)
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        if (texture == null)
        {
            throw new FileNotFoundException("Roomtest texture was not generated", assetPath);
        }

        return texture;
    }

    private static void SetTexture(Material material, string property, Texture texture, Vector2 tiling)
    {
        if (!material.HasProperty(property))
        {
            return;
        }

        material.SetTexture(property, texture);
        material.SetTextureScale(property, tiling);
    }

    private static void SetFloat(Material material, string property, float value)
    {
        if (material.HasProperty(property))
        {
            material.SetFloat(property, value);
        }
    }

    private static void SetColor(Material material, string property, Color value)
    {
        if (material.HasProperty(property))
        {
            material.SetColor(property, value);
        }
    }

    private static Mesh CreateBeveledBrickMesh()
    {
        string path = MeshRoot + "/RT_BrickUnit_Beveled.mesh";
        Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (existing != null)
        {
            return existing;
        }

        float x = 0.6f;
        float y = 0.12f;
        float z = 0.22f;
        float bevel = 0.055f;
        Vector3[] vertices =
        {
            new Vector3(-x + bevel, -y, -z), new Vector3(x - bevel, -y, -z), new Vector3(x, -y + bevel, -z), new Vector3(x, y - bevel, -z),
            new Vector3(x - bevel, y, -z), new Vector3(-x + bevel, y, -z), new Vector3(-x, y - bevel, -z), new Vector3(-x, -y + bevel, -z),
            new Vector3(-x + bevel, -y, z), new Vector3(x - bevel, -y, z), new Vector3(x, -y + bevel, z), new Vector3(x, y - bevel, z),
            new Vector3(x - bevel, y, z), new Vector3(-x + bevel, y, z), new Vector3(-x, y - bevel, z), new Vector3(-x, -y + bevel, z)
        };

        int[] triangles =
        {
            0, 1, 2, 0, 2, 7, 7, 2, 3, 7, 3, 6, 6, 3, 4, 6, 4, 5, 8, 10, 9, 8, 15, 10, 15, 11, 10, 15, 14, 11, 14, 12, 11, 14, 13, 12,
            0, 8, 9, 0, 9, 1, 1, 9, 10, 1, 10, 2, 2, 10, 11, 2, 11, 3, 3, 11, 12, 3, 12, 4, 4, 12, 13, 4, 13, 5, 5, 13, 14, 5, 14, 6, 6, 14, 15, 6, 15, 7, 7, 15, 8, 7, 8, 0
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(Mathf.InverseLerp(-x, x, vertices[i].x), Mathf.InverseLerp(-z, z, vertices[i].z));
        }

        Mesh mesh = new Mesh();
        mesh.name = "RT_BrickUnit_Beveled";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
        AssetDatabase.CreateAsset(mesh, path);
        return mesh;
    }

    private static void CreateBrickPrefab(Mesh mesh, Material material)
    {
        string path = PrefabRoot + "/RT_BrickUnitObject.prefab";
        GameObject root = new GameObject("RT_BrickUnitObject");
        MeshFilter meshFilter = root.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;
        MeshRenderer meshRenderer = root.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;
        PrefabUtility.SaveAsPrefabAsset(root, path);
        UnityEngine.Object.DestroyImmediate(root);
    }

    private static void BuildScene(Material wallMaterial, Material floorMaterial, Material ceilingMaterial, Material lampMaterial, Material ironMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.045f, 0.038f, 0.032f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(0.045f, 0.035f, 0.028f);
        RenderSettings.fogDensity = 0.012f;
        RenderSettings.reflectionIntensity = 0.72f;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;

        CreateCube("RT_Floor_WetReflectiveStoneSlabs", new Vector3(0f, -0.06f, 0f), new Vector3(12.4f, 0.12f, 13.2f), floorMaterial);
        CreateCube("RT_BackWall_DarkBrick", new Vector3(0f, 2.45f, 5.9f), new Vector3(12.4f, 4.9f, 0.14f), wallMaterial);
        CreateCube("RT_LeftWall_DarkBrick", new Vector3(-6.2f, 2.45f, -0.2f), new Vector3(0.14f, 4.9f, 12.35f), wallMaterial);
        CreateCube("RT_RightWall_DarkBrick", new Vector3(6.2f, 2.45f, -0.2f), new Vector3(0.14f, 4.9f, 12.35f), wallMaterial);
        CreateCube("RT_Ceiling_SootBrick", new Vector3(0f, 4.9f, -0.2f), new Vector3(12.4f, 0.12f, 12.35f), ceilingMaterial);

        CreateWallLamp("RT_LeftWall_Gaslight", new Vector3(-6.1f, 2.35f, -0.55f), Quaternion.Euler(0f, 90f, 0f), lampMaterial, ironMaterial, false);
        CreateWallLamp("RT_RightWall_Gaslight", new Vector3(6.1f, 2.35f, -0.55f), Quaternion.Euler(0f, -90f, 0f), lampMaterial, ironMaterial, true);
        CreateWarmPointLight("RT_LeftWall_Gaslight_Point", new Vector3(-5.65f, 2.38f, -0.48f), 5.2f, 8.2f);
        CreateWarmPointLight("RT_RightWall_Gaslight_Point", new Vector3(5.65f, 2.38f, -0.48f), 5.2f, 8.2f);
        CreateSpotLight("RT_LeftWall_Gaslight_FloorWash", new Vector3(-5.65f, 2.25f, -0.4f), new Vector3(-0.5f, 0.35f, 0.8f), 3.1f, 82f, 9f);
        CreateSpotLight("RT_RightWall_Gaslight_FloorWash", new Vector3(5.65f, 2.25f, -0.4f), new Vector3(0.5f, 0.35f, 0.8f), 3.1f, 82f, 9f);
        CreateSpotLight("RT_BackWall_SoftAmberReturn", new Vector3(0f, 3.8f, -3.9f), new Vector3(0f, 2.05f, 5.7f), 0.72f, 78f, 12f);

        GameObject probeObject = new GameObject("RT_RoomReflectionProbe");
        probeObject.transform.position = new Vector3(0f, 2.25f, 0.35f);
        ReflectionProbe probe = probeObject.AddComponent<ReflectionProbe>();
        probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
        probe.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        probe.size = new Vector3(12.2f, 4.8f, 12f);
        probe.intensity = 0.85f;
        probe.resolution = 256;

        GameObject cameraObject = new GameObject("RT_RenderCamera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = new Vector3(0f, 1.72f, -6.45f);
        camera.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 2.05f, 4.8f) - camera.transform.position, Vector3.up);
        camera.fieldOfView = 68f;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 32f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.02f, 0.018f, 0.015f);
        camera.allowHDR = true;
        camera.allowMSAA = true;
        camera.depth = 1f;

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ScenePath);
    }

    private static void BuildMaterialBlockPreviewV02(Material wallMaterial, Material floorMaterial, Material ceilingMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.068f, 0.064f, 0.058f);
        RenderSettings.fog = false;
        RenderSettings.reflectionIntensity = 0.35f;

        CreateCube("RTv02_Preview_WallMaterial_Block", new Vector3(-2.8f, 1.1f, 0.4f), new Vector3(1.8f, 1.8f, 0.12f), wallMaterial);
        CreateCube("RTv02_Preview_FloorMaterial_WetSlab", new Vector3(0f, 0.05f, 0.3f), new Vector3(2.1f, 0.1f, 2.1f), floorMaterial);
        CreateCube("RTv02_Preview_CeilingMaterial_Block", new Vector3(2.8f, 1.1f, 0.4f), new Vector3(1.8f, 1.8f, 0.12f), ceilingMaterial);

        GameObject keyObject = new GameObject("RTv02_Preview_NeutralKeyLight");
        keyObject.transform.position = new Vector3(-2.4f, 4.2f, -3.4f);
        keyObject.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0.7f, 0.4f) - keyObject.transform.position, Vector3.up);
        Light key = keyObject.AddComponent<Light>();
        key.type = LightType.Spot;
        key.color = new Color(0.92f, 0.86f, 0.78f);
        key.intensity = 5.0f;
        key.range = 8f;
        key.spotAngle = 52f;
        key.shadows = LightShadows.Soft;
        key.shadowStrength = 0.62f;

        GameObject rimObject = new GameObject("RTv02_Preview_SmallAmberRim");
        rimObject.transform.position = new Vector3(2.2f, 1.35f, -1.6f);
        Light rim = rimObject.AddComponent<Light>();
        rim.type = LightType.Point;
        rim.color = new Color(1f, 0.55f, 0.25f);
        rim.intensity = 1.6f;
        rim.range = 4.4f;

        GameObject cameraObject = new GameObject("RTv02_PreviewCamera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = new Vector3(0f, 1.45f, -5.0f);
        camera.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0.8f, 0.2f) - camera.transform.position, Vector3.up);
        camera.fieldOfView = 46f;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 20f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.018f, 0.017f, 0.016f);
        camera.allowHDR = true;
        camera.allowMSAA = true;

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), PreviewScenePathV02);
    }

    private static void BuildSceneV02(Material wallMaterial, Material floorMaterial, Material ceilingMaterial, Material lampMaterial, Material ironMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.024f, 0.022f, 0.019f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(0.023f, 0.02f, 0.017f);
        RenderSettings.fogDensity = 0.006f;
        RenderSettings.reflectionIntensity = 0.56f;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;

        CreateCube("RTv02_Floor_WetBlackStoneSlabs", new Vector3(0f, -0.06f, 0f), new Vector3(12.4f, 0.12f, 13.2f), floorMaterial);
        CreateCube("RTv02_BackWall_DarkIrregularBrick", new Vector3(0f, 2.45f, 5.9f), new Vector3(12.4f, 4.9f, 0.14f), wallMaterial);
        CreateCube("RTv02_LeftWall_DarkIrregularBrick", new Vector3(-6.2f, 2.45f, -0.2f), new Vector3(0.14f, 4.9f, 12.35f), wallMaterial);
        CreateCube("RTv02_RightWall_DarkIrregularBrick", new Vector3(6.2f, 2.45f, -0.2f), new Vector3(0.14f, 4.9f, 12.35f), wallMaterial);
        CreateCube("RTv02_Ceiling_SootedBrick", new Vector3(0f, 4.9f, -0.2f), new Vector3(12.4f, 0.12f, 12.35f), ceilingMaterial);

        CreateWallLampV02("RTv02_LeftWall_ContainedGaslight", new Vector3(-6.1f, 2.33f, -0.65f), Quaternion.Euler(0f, 90f, 0f), lampMaterial, ironMaterial, false);
        CreateWallLampV02("RTv02_RightWall_ContainedGaslight", new Vector3(6.1f, 2.33f, -0.65f), Quaternion.Euler(0f, -90f, 0f), lampMaterial, ironMaterial, true);
        CreatePointLightV02("RTv02_LeftWall_LocalAmberHalo", new Vector3(-5.75f, 2.36f, -0.58f), 4.15f, 4.55f);
        CreatePointLightV02("RTv02_RightWall_LocalAmberHalo", new Vector3(5.75f, 2.36f, -0.58f), 4.15f, 4.55f);
        CreateSpotLightV02("RTv02_LeftWall_WetFloorReflection", new Vector3(-5.55f, 2.12f, -0.55f), new Vector3(-2.75f, 0.06f, -2.75f), 2.05f, 64f, 6.4f);
        CreateSpotLightV02("RTv02_RightWall_WetFloorReflection", new Vector3(5.55f, 2.12f, -0.55f), new Vector3(2.75f, 0.06f, -2.75f), 2.05f, 64f, 6.4f);
        CreateSpotLightV02("RTv02_BackWallLowReadabilityFill", new Vector3(0f, 3.25f, -2.75f), new Vector3(0f, 2.05f, 5.8f), 0.55f, 72f, 11.5f);
        CreateSpotLightV02("RTv02_CeilingLowWarmEdge", new Vector3(0f, 4.15f, -3.85f), new Vector3(0f, 2.9f, 1.8f), 0.35f, 62f, 8.5f);

        GameObject probeObject = new GameObject("RTv02_RoomReflectionProbe");
        probeObject.transform.position = new Vector3(0f, 1.75f, -0.85f);
        ReflectionProbe probe = probeObject.AddComponent<ReflectionProbe>();
        probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
        probe.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        probe.size = new Vector3(12.1f, 4.75f, 11.8f);
        probe.intensity = 0.62f;
        probe.resolution = 256;

        GameObject cameraObject = new GameObject("RTv02_RenderCamera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = new Vector3(0f, 1.68f, -6.55f);
        camera.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 2.0f, 4.85f) - camera.transform.position, Vector3.up);
        camera.fieldOfView = 68f;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 32f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.012f, 0.011f, 0.01f);
        camera.allowHDR = true;
        camera.allowMSAA = true;
        camera.depth = 1f;

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ScenePathV02);
    }

    private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = name;
        cube.transform.position = position;
        cube.transform.localScale = scale;
        MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        return cube;
    }

    private static void CreateWallLamp(string name, Vector3 position, Quaternion rotation, Material lampMaterial, Material ironMaterial, bool mirror)
    {
        GameObject root = new GameObject(name);
        root.transform.position = position;
        root.transform.rotation = rotation;

        GameObject backPlate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backPlate.name = name + "_IronBackPlate";
        backPlate.transform.SetParent(root.transform, false);
        backPlate.transform.localPosition = Vector3.zero;
        backPlate.transform.localScale = new Vector3(0.52f, 0.72f, 0.08f);
        backPlate.GetComponent<MeshRenderer>().sharedMaterial = ironMaterial;

        GameObject glass = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        glass.name = name + "_AmberGlowGlass";
        glass.transform.SetParent(root.transform, false);
        glass.transform.localPosition = new Vector3(0f, 0f, -0.075f);
        glass.transform.localScale = new Vector3(0.32f, 0.32f, 0.12f);
        glass.GetComponent<MeshRenderer>().sharedMaterial = lampMaterial;

        for (int i = -1; i <= 1; i += 2)
        {
            GameObject cage = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cage.name = name + "_CageRib_" + (i > 0 ? "Upper" : "Lower");
            cage.transform.SetParent(root.transform, false);
            cage.transform.localPosition = new Vector3(0f, i * 0.23f, -0.145f);
            cage.transform.localScale = new Vector3(0.44f, 0.035f, 0.055f);
            cage.GetComponent<MeshRenderer>().sharedMaterial = ironMaterial;
        }

        GameObject stem = GameObject.CreatePrimitive(PrimitiveType.Cube);
        stem.name = name + "_PipeStem";
        stem.transform.SetParent(root.transform, false);
        stem.transform.localPosition = new Vector3(mirror ? 0.36f : -0.36f, 0f, 0f);
        stem.transform.localScale = new Vector3(0.16f, 0.1f, 0.08f);
        stem.GetComponent<MeshRenderer>().sharedMaterial = ironMaterial;
    }

    private static void CreateWallLampV02(string name, Vector3 position, Quaternion rotation, Material lampMaterial, Material ironMaterial, bool mirror)
    {
        GameObject root = new GameObject(name);
        root.transform.position = position;
        root.transform.rotation = rotation;

        GameObject backPlate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backPlate.name = name + "_BlackIronBackPlate";
        backPlate.transform.SetParent(root.transform, false);
        backPlate.transform.localPosition = Vector3.zero;
        backPlate.transform.localScale = new Vector3(0.42f, 0.62f, 0.055f);
        backPlate.GetComponent<MeshRenderer>().sharedMaterial = ironMaterial;

        GameObject glass = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        glass.name = name + "_SmallAmberGlass";
        glass.transform.SetParent(root.transform, false);
        glass.transform.localPosition = new Vector3(0f, 0f, -0.065f);
        glass.transform.localScale = new Vector3(0.22f, 0.28f, 0.09f);
        glass.GetComponent<MeshRenderer>().sharedMaterial = lampMaterial;

        for (int i = -1; i <= 1; i += 2)
        {
            GameObject rib = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rib.name = name + "_BlackCageRib_" + (i > 0 ? "Upper" : "Lower");
            rib.transform.SetParent(root.transform, false);
            rib.transform.localPosition = new Vector3(0f, i * 0.19f, -0.11f);
            rib.transform.localScale = new Vector3(0.34f, 0.028f, 0.042f);
            rib.GetComponent<MeshRenderer>().sharedMaterial = ironMaterial;
        }

        for (int i = -1; i <= 1; i += 2)
        {
            GameObject sideRib = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sideRib.name = name + "_VerticalCageRib_" + (i > 0 ? "Right" : "Left");
            sideRib.transform.SetParent(root.transform, false);
            sideRib.transform.localPosition = new Vector3(i * 0.19f, 0f, -0.11f);
            sideRib.transform.localScale = new Vector3(0.028f, 0.43f, 0.042f);
            sideRib.GetComponent<MeshRenderer>().sharedMaterial = ironMaterial;
        }

        GameObject pipeStub = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pipeStub.name = name + "_ShortPipeStub";
        pipeStub.transform.SetParent(root.transform, false);
        pipeStub.transform.localPosition = new Vector3(mirror ? 0.29f : -0.29f, 0f, 0f);
        pipeStub.transform.localScale = new Vector3(0.11f, 0.08f, 0.06f);
        pipeStub.GetComponent<MeshRenderer>().sharedMaterial = ironMaterial;
    }

    private static void CreateWarmPointLight(string name, Vector3 position, float intensity, float range)
    {
        GameObject lightObject = new GameObject(name);
        lightObject.transform.position = position;
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.67f, 0.38f);
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 0.74f;
        light.shadowBias = 0.045f;
        light.shadowNormalBias = 0.34f;
    }

    private static void CreateSpotLight(string name, Vector3 position, Vector3 target, float intensity, float angle, float range)
    {
        GameObject lightObject = new GameObject(name);
        lightObject.transform.position = position;
        lightObject.transform.rotation = Quaternion.LookRotation(target - position, Vector3.up);
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Spot;
        light.color = new Color(1f, 0.65f, 0.34f);
        light.intensity = intensity;
        light.range = range;
        light.spotAngle = angle;
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 0.68f;
    }

    private static void CreatePointLightV02(string name, Vector3 position, float intensity, float range)
    {
        GameObject lightObject = new GameObject(name);
        lightObject.transform.position = position;
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.59f, 0.27f);
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 0.86f;
        light.shadowBias = 0.035f;
        light.shadowNormalBias = 0.26f;
    }

    private static void CreateSpotLightV02(string name, Vector3 position, Vector3 target, float intensity, float angle, float range)
    {
        GameObject lightObject = new GameObject(name);
        lightObject.transform.position = position;
        lightObject.transform.rotation = Quaternion.LookRotation(target - position, Vector3.up);
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Spot;
        light.color = new Color(1f, 0.57f, 0.28f);
        light.intensity = intensity;
        light.range = range;
        light.spotAngle = angle;
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 0.82f;
        light.shadowBias = 0.035f;
    }

    private static string RenderScene()
    {
        return RenderSceneToFile(RenderFileName);
    }

    private static string RenderSceneToFile(string fileName)
    {
        Camera camera = UnityEngine.Object.FindObjectOfType<Camera>();
        if (camera == null)
        {
            throw new InvalidOperationException("Roomtest render camera is missing.");
        }

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(RenderWidth, RenderHeight, RenderTextureFormat.ARGB32, 24);
        descriptor.msaaSamples = 8;
        descriptor.sRGB = QualitySettings.activeColorSpace == ColorSpace.Linear;
        RenderTexture renderTexture = new RenderTexture(descriptor);
        Texture2D capture = new Texture2D(RenderWidth, RenderHeight, TextureFormat.RGBA32, false, QualitySettings.activeColorSpace == ColorSpace.Linear);

        RenderTexture previousActive = RenderTexture.active;
        RenderTexture previousTarget = camera.targetTexture;
        camera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        camera.Render();
        capture.ReadPixels(new Rect(0, 0, RenderWidth, RenderHeight), 0, 0);
        capture.Apply(false, false);

        string renderPath = GetProjectPath("Renders/" + fileName);
        File.WriteAllBytes(renderPath, capture.EncodeToPNG());

        camera.targetTexture = previousTarget;
        RenderTexture.active = previousActive;
        renderTexture.Release();
        UnityEngine.Object.DestroyImmediate(renderTexture);
        UnityEngine.Object.DestroyImmediate(capture);
        return renderPath;
    }

    private static void WriteMetrics(string renderPath)
    {
        WriteMetricsToFile(renderPath, RenderFileName, MetricsFileName);
    }

    private static void WriteMetricsToFile(string renderPath, string renderFileName, string metricsFileName)
    {
        byte[] bytes = File.ReadAllBytes(renderPath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        texture.LoadImage(bytes);
        Color32[] pixels = texture.GetPixels32();

        double luminanceSum = 0d;
        float maxLuminance = 0f;
        int warmHighlightPixels = 0;
        int darkPixels = 0;
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 p = pixels[i];
            float r = p.r / 255f;
            float g = p.g / 255f;
            float b = p.b / 255f;
            float luminance = r * 0.2126f + g * 0.7152f + b * 0.0722f;
            luminanceSum += luminance;
            maxLuminance = Mathf.Max(maxLuminance, luminance);
            if (luminance > 0.54f && r > g && g > b)
            {
                warmHighlightPixels++;
            }

            if (luminance < 0.16f)
            {
                darkPixels++;
            }
        }

        double averageLuminance = luminanceSum / pixels.Length;
        double warmRatio = (double)warmHighlightPixels / pixels.Length;
        double darkRatio = (double)darkPixels / pixels.Length;
        UnityEngine.Object.DestroyImmediate(texture);

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("{");
        builder.AppendLine("  \"schema\": \"brassworks.roomtest.metrics.v1\",");
        builder.AppendLine("  \"render\": \"" + renderFileName + "\",");
        builder.AppendLine("  \"target_reference\": \"dark warm wet brick room\",");
        builder.AppendLine("  \"texture_size\": " + TextureSize.ToString(CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"render_size\": \"" + RenderWidth.ToString(CultureInfo.InvariantCulture) + "x" + RenderHeight.ToString(CultureInfo.InvariantCulture) + "\",");
        builder.AppendLine("  \"average_luminance\": " + averageLuminance.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"max_luminance\": " + maxLuminance.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"warm_highlight_pixel_ratio\": " + warmRatio.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"dark_pixel_ratio\": " + darkRatio.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"acceptance_notes\": [");
        builder.AppendLine("    \"dark_pixel_ratio should stay high enough to preserve the dungeon mood\",");
        builder.AppendLine("    \"warm_highlight_pixel_ratio confirms amber lamps are present without flattening the whole room\",");
        builder.AppendLine("    \"visual review remains required for mortar relief, brick scale, and floor reflection quality\"");
        builder.AppendLine("  ]");
        builder.AppendLine("}");
        File.WriteAllText(GetProjectPath("Renders/" + metricsFileName), builder.ToString());
    }

    private static void WriteV02ReviewNote(string previewRenderPath, string roomRenderPath)
    {
        string notePath = GetProjectPath("Documentation/ROOMTEST_V0_2_REVIEW.md");
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.2 Review");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## Files");
        builder.AppendLine();
        builder.AppendLine("- Material preview: `" + previewRenderPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Room render: `" + roomRenderPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Metrics: `Renders/" + MetricsFileNameV02 + "`");
        builder.AppendLine();
        builder.AppendLine("## Intent");
        builder.AppendLine();
        builder.AppendLine("- Shifted albedo toward darker brown-black masonry instead of orange brick.");
        builder.AppendLine("- Generated versioned wall, floor, and ceiling maps with irregular mortar widths, chipped edges, grime, and less uniform raised-grid relief.");
        builder.AppendLine("- Added a neutral material block preview before the full room render.");
        builder.AppendLine("- Reduced broad room fill and used tighter amber point/spot lights for localized lamp halos.");
        builder.AppendLine("- Kept the floor smoother and wetter than walls while leaving all metallic channels at zero.");
        builder.AppendLine();
        builder.AppendLine("## Current v0.2 Assessment");
        builder.AppendLine();
        builder.AppendLine("- Improved: the room is no longer the v0.1 orange wash, the floor has darker wet glints, and the lamp light is more localized.");
        builder.AppendLine("- Still failing: the material block preview is too underlit, the wall texture still reads cleaner and flatter than the reference, and the full room needs stronger chipped edge readability without returning to a raised tile grid.");
        builder.AppendLine("- Next focus: improve texture legibility first, then add small geometry or decals for broken stone edges and grime buildup around wall corners.");
        builder.AppendLine();
        builder.AppendLine("## Blunt Assessment Targets");
        builder.AppendLine();
        builder.AppendLine("- Pass if the room is much darker than v0.1, wall halos are localized, floor highlights read wet rather than copper metal, and the brick pattern is less grid-perfect.");
        builder.AppendLine("- Still fails if the room remains globally orange, the floor produces rectangular glowing patches, the mortar reads as clean tile grout, or the brick relief looks like an even embossed grid.");
        File.WriteAllText(notePath, builder.ToString());
    }

    private static Color32 ToColor32(Color color, float alpha)
    {
        return new Color32(
            (byte)Mathf.RoundToInt(Mathf.Clamp01(color.r) * 255f),
            (byte)Mathf.RoundToInt(Mathf.Clamp01(color.g) * 255f),
            (byte)Mathf.RoundToInt(Mathf.Clamp01(color.b) * 255f),
            (byte)Mathf.RoundToInt(Mathf.Clamp01(alpha) * 255f));
    }

    private static float Fbm(float x, float y, int seed, int octaves)
    {
        float value = 0f;
        float amplitude = 0.5f;
        float frequency = 1f;
        float total = 0f;
        for (int i = 0; i < octaves; i++)
        {
            value += ValueNoise(x * frequency, y * frequency, seed + i * 19) * amplitude;
            total += amplitude;
            amplitude *= 0.5f;
            frequency *= 2f;
        }

        return value / total;
    }

    private static float ValueNoise(float x, float y, int seed)
    {
        int x0 = Mathf.FloorToInt(x);
        int y0 = Mathf.FloorToInt(y);
        float tx = SmoothStep(Frac(x));
        float ty = SmoothStep(Frac(y));
        float a = Hash01(x0, y0, seed);
        float b = Hash01(x0 + 1, y0, seed);
        float c = Hash01(x0, y0 + 1, seed);
        float d = Hash01(x0 + 1, y0 + 1, seed);
        return Mathf.Lerp(Mathf.Lerp(a, b, tx), Mathf.Lerp(c, d, tx), ty);
    }

    private static float Hash01(int x, int y, int seed)
    {
        unchecked
        {
            uint h = (uint)(x * 374761393 + y * 668265263 + seed * 1442695041);
            h = (h ^ (h >> 13)) * 1274126177u;
            h ^= h >> 16;
            return (h & 0x00FFFFFF) / 16777215f;
        }
    }

    private static float Frac(float value)
    {
        return value - Mathf.Floor(value);
    }

    private static float SmoothStep(float value)
    {
        return value * value * (3f - 2f * value);
    }

    private static string GetProjectPath(string relativePath)
    {
        return Path.GetFullPath(Path.Combine(Application.dataPath, "..", relativePath.Replace('/', Path.DirectorySeparatorChar)));
    }

    private readonly struct BrickSample
    {
        public BrickSample(bool isMortar, float edgeDistance, float brickTone)
        {
            this.isMortar = isMortar;
            this.edgeDistance = edgeDistance;
            this.brickTone = brickTone;
        }

        public readonly bool isMortar;
        public readonly float edgeDistance;
        public readonly float brickTone;
    }

    private readonly struct BrickSampleV02
    {
        public BrickSampleV02(bool isMortar, float edgeDistance, float brickTone, float irregularity, float stain)
        {
            this.isMortar = isMortar;
            this.edgeDistance = edgeDistance;
            this.brickTone = brickTone;
            this.irregularity = irregularity;
            this.stain = stain;
        }

        public readonly bool isMortar;
        public readonly float edgeDistance;
        public readonly float brickTone;
        public readonly float irregularity;
        public readonly float stain;
    }

    private readonly struct SurfaceTextureSet
    {
        public SurfaceTextureSet(string albedoPath, string normalPath, string heightPath, string occlusionPath, string metallicSmoothnessPath)
        {
            AlbedoPath = albedoPath;
            NormalPath = normalPath;
            HeightPath = heightPath;
            OcclusionPath = occlusionPath;
            MetallicSmoothnessPath = metallicSmoothnessPath;
        }

        public readonly string AlbedoPath;
        public readonly string NormalPath;
        public readonly string HeightPath;
        public readonly string OcclusionPath;
        public readonly string MetallicSmoothnessPath;
    }
}
