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
    private const string RenderFileName = "roomtest_brick_lighting_v0.1.png";
    private const string MetricsFileName = "roomtest_metrics_v0.1.json";

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

    private static string RenderScene()
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

        string renderPath = GetProjectPath("Renders/" + RenderFileName);
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

        string metricsPath = GetProjectPath("Renders/" + MetricsFileName);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("{");
        builder.AppendLine("  \"schema\": \"brassworks.roomtest.metrics.v1\",");
        builder.AppendLine("  \"render\": \"" + RenderFileName + "\",");
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
        File.WriteAllText(metricsPath, builder.ToString());
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
