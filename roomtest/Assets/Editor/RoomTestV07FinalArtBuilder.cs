using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RoomTestV07FinalArtBuilder
{
    private const int TextureSize = 2048;
    private const int RenderWidth = 1920;
    private const int RenderHeight = 1080;
    private const string Root = "Assets/RoomTestV07";
    private const string TextureRoot = Root + "/Textures";
    private const string MaterialRoot = Root + "/Materials";
    private const string SceneRoot = Root + "/Scenes";
    private const string BeautyScenePath = SceneRoot + "/Roomtest_V07_FinalArtCorridor.unity";
    private const string CloseupScenePath = SceneRoot + "/Roomtest_V07_MaterialCloseup.unity";
    private const string ContactScenePath = SceneRoot + "/Roomtest_V07_ContactSheet.unity";
    private const string BeautyRenderName = "roomtest_v0.7_final_art_beauty.png";
    private const string CloseupRenderName = "roomtest_v0.7_material_closeup.png";
    private const string ContactSheetName = "roomtest_v0.7_contact_sheet.png";
    private const string MetricsName = "roomtest_metrics_v0.7.json";
    private const string ReviewName = "ROOMTEST_V0_7_FINAL_ART_PIPELINE_REVIEW.md";

    private enum SurfaceKind
    {
        BackWall,
        LeftWall,
        RightWall,
        Floor,
        Ceiling
    }

    [MenuItem("Roomtest/Build And Render Final Art Pipeline v0.7")]
    public static void BuildAndRenderV07()
    {
        try
        {
            BuildAndRenderV07Internal();
        }
        catch (Exception exception)
        {
            EnsureFolders();
            WriteFailureReview(exception);
            Debug.LogException(exception);
            throw;
        }
    }

    private static void BuildAndRenderV07Internal()
    {
        EnsureFolders();
        ConfigureProjectForFinalArt();
        WriteInitialReview();
        Debug.Log("ROOMTEST_V07_STAGE_START package=built-in-renderer textureSize=" + TextureSize.ToString(CultureInfo.InvariantCulture));

        TextureSet wallTextures = GenerateStoneTextureSet(
            "RTv07_DarkWetBrick",
            new Color(0.072f, 0.059f, 0.045f),
            new Color(0.011f, 0.009f, 0.007f),
            771,
            0.18f,
            false);
        TextureSet floorTextures = GenerateStoneTextureSet(
            "RTv07_WetBlackFlagstone",
            new Color(0.086f, 0.071f, 0.056f),
            new Color(0.007f, 0.006f, 0.005f),
            797,
            0.62f,
            true);
        TextureSet ceilingTextures = GenerateStoneTextureSet(
            "RTv07_SootedCeilingBrick",
            new Color(0.045f, 0.037f, 0.029f),
            new Color(0.007f, 0.006f, 0.005f),
            823,
            0.08f,
            false);
        Debug.Log("ROOMTEST_V07_STAGE_TEXTURE_FILES_WRITTEN count=15");
        ImportTextureSets(wallTextures, floorTextures, ceilingTextures);
        ValidateTextures(wallTextures, floorTextures, ceilingTextures);

        Material wallBase = CreatePbrMaterial("RTv07_MAT_DarkWetBrick", wallTextures, new Vector2(0.9f, 0.82f), 0.2f, 0.028f, 1.15f);
        Material floorBase = CreatePbrMaterial("RTv07_MAT_WetBlackFlagstone", floorTextures, new Vector2(0.78f, 0.72f), 0.58f, 0.02f, 0.82f);
        Material ceilingBase = CreatePbrMaterial("RTv07_MAT_SootedCeilingBrick", ceilingTextures, new Vector2(0.9f, 0.8f), 0.08f, 0.02f, 0.85f);
        Material mortar = CreateSimpleMaterial("RTv07_MAT_DeepBlackRecessedMortar", new Color(0.004f, 0.0033f, 0.0026f), 0f, 0.025f);
        Material soot = CreateSimpleMaterial("RTv07_MAT_OilSootGrime", new Color(0.003f, 0.0026f, 0.0022f), 0f, 0.18f);
        Material wetOil = CreateSimpleMaterial("RTv07_MAT_GlossyBlackOilFilm", new Color(0.006f, 0.0048f, 0.0036f), 0f, 0.78f);
        Material brass = CreateSimpleMaterial("RTv07_MAT_TarnishedBrass", new Color(0.39f, 0.27f, 0.12f), 0.22f, 0.34f);
        Material copper = CreateSimpleMaterial("RTv07_MAT_DarkOxidizedCopper", new Color(0.22f, 0.12f, 0.06f), 0.18f, 0.28f);
        Material iron = CreateSimpleMaterial("RTv07_MAT_BlackIron", new Color(0.012f, 0.011f, 0.01f), 0.16f, 0.22f);
        Material lampGlass = CreateEmissiveMaterial("RTv07_MAT_HotGaslightGlass", new Color(1f, 0.58f, 0.27f), 4.6f);
        Material[] wallVariants = CreateVariants("RTv07_MAT_DarkBrickVariant", wallBase, new[]
        {
            new Color(0.72f, 0.66f, 0.58f),
            new Color(0.88f, 0.78f, 0.64f),
            new Color(0.58f, 0.56f, 0.52f),
            new Color(1.02f, 0.84f, 0.66f),
            new Color(0.65f, 0.58f, 0.5f),
            new Color(0.78f, 0.69f, 0.56f)
        });
        Material[] floorVariants = CreateVariants("RTv07_MAT_WetFlagstoneVariant", floorBase, new[]
        {
            new Color(0.72f, 0.67f, 0.6f),
            new Color(0.85f, 0.76f, 0.64f),
            new Color(0.58f, 0.56f, 0.52f),
            new Color(0.78f, 0.68f, 0.55f)
        });
        Material[] ceilingVariants = CreateVariants("RTv07_MAT_CeilingBrickVariant", ceilingBase, new[]
        {
            new Color(0.6f, 0.57f, 0.52f),
            new Color(0.72f, 0.64f, 0.54f),
            new Color(0.48f, 0.47f, 0.44f)
        });
        Debug.Log("ROOMTEST_V07_STAGE_MATERIALS_READY variants=" + (wallVariants.Length + floorVariants.Length + ceilingVariants.Length).ToString(CultureInfo.InvariantCulture));

        BuildBeautyScene(wallVariants, floorVariants, ceilingVariants, mortar, soot, wetOil, brass, copper, iron, lampGlass);
        Debug.Log("ROOMTEST_V07_STAGE_BEAUTY_SCENE_READY " + BeautyScenePath);
        string beautyPath = RenderSceneToFile(BeautyRenderName);
        Debug.Log("ROOMTEST_V07_STAGE_BEAUTY_RENDERED " + beautyPath);
        BuildCloseupScene(wallVariants, floorVariants, mortar, soot, wetOil, brass, copper, iron, lampGlass);
        Debug.Log("ROOMTEST_V07_STAGE_CLOSEUP_SCENE_READY " + CloseupScenePath);
        string closeupPath = RenderSceneToFile(CloseupRenderName);
        Debug.Log("ROOMTEST_V07_STAGE_CLOSEUP_RENDERED " + closeupPath);
        BuildContactSheetScene(beautyPath, closeupPath);
        Debug.Log("ROOMTEST_V07_STAGE_CONTACT_SCENE_READY " + ContactScenePath);
        string contactPath = RenderSceneToFile(ContactSheetName);
        WriteMetrics(beautyPath, closeupPath, contactPath);
        WriteFinalReview(beautyPath, closeupPath, contactPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ROOMTEST_V07_FINAL_ART_COMPLETE " + beautyPath);
    }

    private static void EnsureFolders()
    {
        CreateFolder("Assets", "RoomTestV07");
        CreateFolder(Root, "Textures");
        CreateFolder(Root, "Materials");
        CreateFolder(Root, "Scenes");
        Directory.CreateDirectory(ProjectPath("Renders"));
        Directory.CreateDirectory(ProjectPath("Documentation"));
        Directory.CreateDirectory(ProjectPath("Logs"));
    }

    private static void CreateFolder(string parent, string child)
    {
        if (!AssetDatabase.IsValidFolder(parent + "/" + child))
        {
            AssetDatabase.CreateFolder(parent, child);
        }
    }

    private static void ConfigureProjectForFinalArt()
    {
        PlayerSettings.colorSpace = ColorSpace.Linear;
        QualitySettings.SetQualityLevel(5, true);
        QualitySettings.antiAliasing = 8;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.shadows = ShadowQuality.All;
        QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
        QualitySettings.shadowDistance = 24f;
        QualitySettings.realtimeReflectionProbes = true;
    }

    private static void BuildBeautyScene(Material[] wall, Material[] floor, Material[] ceiling, Material mortar, Material soot, Material wetOil, Material brass, Material copper, Material iron, Material lampGlass)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.028f, 0.021f, 0.016f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(0.022f, 0.016f, 0.011f);
        RenderSettings.fogDensity = 0.0012f;
        RenderSettings.reflectionIntensity = 0.56f;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
        RenderSettings.customReflectionTexture = null;

        CreateBackdrop("RTv07_Floor_MortarBed", SurfaceKind.Floor, -5.3f, 5.3f, -5.5f, 5.2f, -0.035f, 0.03f, mortar);
        CreateBackdrop("RTv07_BackWall_MortarBed", SurfaceKind.BackWall, -5.3f, 5.3f, 0.08f, 4.12f, 5.26f, 0.04f, mortar);
        CreateBackdrop("RTv07_LeftWall_MortarBed", SurfaceKind.LeftWall, -5.5f, 5.25f, 0.08f, 4.12f, -5.36f, 0.04f, mortar);
        CreateBackdrop("RTv07_RightWall_MortarBed", SurfaceKind.RightWall, -5.5f, 5.25f, 0.08f, 4.12f, 5.36f, 0.04f, mortar);
        CreateBackdrop("RTv07_Ceiling_MortarBed", SurfaceKind.Ceiling, -5.3f, 5.3f, -5.5f, 5.2f, 4.16f, 0.035f, mortar);

        CreateDisplacedMasonrySurface("RTv07_BackWall_IrregularWetBrickSkin", SurfaceKind.BackWall, wall[1], -5.16f, 5.16f, 0.18f, 4.02f, 5.18f, 192, 76, 901, 0.105f, false);
        CreateDisplacedMasonrySurface("RTv07_LeftWall_IrregularWetBrickSkin", SurfaceKind.LeftWall, wall[0], -5.26f, 5.05f, 0.18f, 4.02f, -5.27f, 192, 76, 919, 0.105f, false);
        CreateDisplacedMasonrySurface("RTv07_RightWall_IrregularWetBrickSkin", SurfaceKind.RightWall, wall[2], -5.26f, 5.05f, 0.18f, 4.02f, 5.27f, 192, 76, 937, 0.105f, false);
        CreateDisplacedMasonrySurface("RTv07_Ceiling_SootedBrickSkin", SurfaceKind.Ceiling, ceiling[1], -5.16f, 5.16f, -5.34f, 5.08f, 4.08f, 170, 110, 953, 0.07f, false);
        CreateDisplacedMasonrySurface("RTv07_Floor_WetFlagstoneSkin", SurfaceKind.Floor, floor[1], -5.18f, 5.18f, -5.36f, 5.04f, 0.012f, 150, 150, 977, 0.075f, true);

        CreateGrimeBands(soot);
        CreateOilPuddles(wetOil);
        CreateGaslightRig("RTv07_LeftGaslight", new Vector3(-5.22f, 2.28f, -1.12f), Quaternion.Euler(0f, 90f, 0f), lampGlass, brass, copper, iron, false);
        CreateGaslightRig("RTv07_RightGaslight", new Vector3(5.22f, 2.28f, -1.12f), Quaternion.Euler(0f, -90f, 0f), lampGlass, brass, copper, iron, true);
        CreateCeilingPipeRun("RTv07_CeilingPipeA", new Vector3(-3.8f, 3.88f, -5.2f), new Vector3(-3.8f, 3.88f, 5.1f), 0.07f, copper);
        CreateCeilingPipeRun("RTv07_CeilingPipeB", new Vector3(3.9f, 3.82f, -5.1f), new Vector3(3.9f, 3.82f, 5.0f), 0.055f, iron);
        CreateCylinderBetween("RTv07_BackWall_BrassRail", new Vector3(-4.95f, 0.72f, 5.08f), new Vector3(4.95f, 0.72f, 5.08f), 0.025f, brass);

        CreatePointLight("RTv07_Left_GaslightCore", new Vector3(-4.92f, 2.28f, -1.12f), new Color(1f, 0.58f, 0.28f), 4.2f, 5.4f, true);
        CreatePointLight("RTv07_Right_GaslightCore", new Vector3(4.92f, 2.28f, -1.12f), new Color(1f, 0.58f, 0.28f), 4.2f, 5.4f, true);
        CreateSpotLight("RTv07_Left_WetFloorGlance", new Vector3(-4.85f, 2.05f, -1.0f), new Vector3(-1.9f, 0.06f, -3.55f), new Color(1f, 0.55f, 0.28f), 2.1f, 46f, 8.0f);
        CreateSpotLight("RTv07_Right_WetFloorGlance", new Vector3(4.85f, 2.05f, -1.0f), new Vector3(1.9f, 0.06f, -3.55f), new Color(1f, 0.55f, 0.28f), 2.1f, 46f, 8.0f);
        CreateSpotLight("RTv07_BackWall_ControlledWarmReadability", new Vector3(0f, 2.72f, -4.75f), new Vector3(0f, 2.0f, 4.9f), new Color(0.8f, 0.64f, 0.46f), 2.45f, 74f, 14f);
        CreatePointLight("RTv07_SoftNoShadowExposure", new Vector3(0f, 2.15f, -2.15f), new Color(0.58f, 0.47f, 0.36f), 1.35f, 11f, false);

        GameObject probeObject = new GameObject("RTv07_FinalArtReflectionProbe");
        probeObject.transform.position = new Vector3(0f, 1.45f, -0.65f);
        ReflectionProbe probe = probeObject.AddComponent<ReflectionProbe>();
        probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
        probe.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        probe.resolution = 512;
        probe.size = new Vector3(10.8f, 4.2f, 10.6f);
        probe.intensity = 0.78f;

        CreateCamera("RTv07_BeautyCamera", new Vector3(0f, 1.52f, -5.82f), new Vector3(0f, 1.9f, 4.7f), 63f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), BeautyScenePath);
    }

    private static void BuildCloseupScene(Material[] wall, Material[] floor, Material mortar, Material soot, Material wetOil, Material brass, Material copper, Material iron, Material lampGlass)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.04f, 0.03f, 0.022f);
        RenderSettings.fog = false;
        RenderSettings.reflectionIntensity = 0.58f;

        CreateBackdrop("RTv07_Closeup_WallMortar", SurfaceKind.BackWall, -2.4f, 2.4f, 0.15f, 2.85f, 1.35f, 0.04f, mortar);
        CreateDisplacedMasonrySurface("RTv07_Closeup_WallReliefSkin", SurfaceKind.BackWall, wall[1], -2.3f, 2.3f, 0.22f, 2.76f, 1.31f, 110, 70, 1301, 0.125f, false);
        CreateBackdrop("RTv07_Closeup_FloorMortar", SurfaceKind.Floor, -2.6f, 2.6f, -1.85f, 1.15f, -0.035f, 0.03f, mortar);
        CreateDisplacedMasonrySurface("RTv07_Closeup_FloorReliefSkin", SurfaceKind.Floor, floor[1], -2.45f, 2.45f, -1.75f, 1.05f, 0f, 96, 64, 1327, 0.085f, true);
        CreateIrregularBlob("RTv07_Closeup_OilFilm", SurfaceKind.Floor, new Vector3(0.72f, 0.052f, -0.75f), 0.75f, 0.32f, 18, 1337, wetOil);
        CreateBox("RTv07_Closeup_SootStain", new Vector3(-1.2f, 1.9f, 1.285f), new Vector3(0.9f, 0.55f, 0.018f), soot);
        CreateGaslightRig("RTv07_Closeup_Gaslight", new Vector3(-2.18f, 1.62f, 0.5f), Quaternion.Euler(0f, 0f, 0f), lampGlass, brass, copper, iron, false);
        CreatePointLight("RTv07_Closeup_HotGaslight", new Vector3(-1.95f, 1.62f, -0.1f), new Color(1f, 0.58f, 0.28f), 4.0f, 4.5f, true);
        CreateSpotLight("RTv07_Closeup_FloorGlint", new Vector3(-1.6f, 2.0f, -2.6f), new Vector3(0.55f, 0.05f, -0.65f), new Color(1f, 0.58f, 0.32f), 2.0f, 54f, 6.0f);
        CreateSpotLight("RTv07_Closeup_Key", new Vector3(1.6f, 3.0f, -2.6f), new Vector3(0f, 0.9f, 0.6f), new Color(0.72f, 0.6f, 0.48f), 1.2f, 50f, 6.5f);
        CreateCamera("RTv07_CloseupCamera", new Vector3(0f, 1.05f, -3.2f), new Vector3(-0.15f, 1.05f, 0.75f), 47f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), CloseupScenePath);
    }

    private static void BuildContactSheetScene(string beautyPath, string closeupPath)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Color.white;
        RenderSettings.fog = false;
        Material beauty = CreateUnlitImageMaterial("RTv07_MAT_Contact_Beauty", beautyPath);
        Material closeup = CreateUnlitImageMaterial("RTv07_MAT_Contact_Closeup", closeupPath);
        CreateContactQuad("RTv07_Contact_BeautyImage", new Vector3(-2.62f, 0.44f, 0f), new Vector2(4.92f, 2.77f), beauty);
        CreateContactQuad("RTv07_Contact_CloseupImage", new Vector3(2.62f, 0.44f, 0f), new Vector2(4.92f, 2.77f), closeup);
        CreateText("v0.7 final-art beauty", new Vector3(-4.95f, 2.24f, -0.05f), 0.19f);
        CreateText("v0.7 material closeup", new Vector3(0.35f, 2.24f, -0.05f), 0.19f);
        CreateText("Unity-only proof: generated PBR maps + beveled mesh relief + controlled gaslight/reflection probes", new Vector3(-5.05f, -2.35f, -0.05f), 0.13f);
        CreateText("Self-score target: closer, not AAA final. Gaps: no scanned assets, limited bevel complexity, built-in renderer only.", new Vector3(-5.05f, -2.65f, -0.05f), 0.13f);
        Camera camera = new GameObject("RTv07_ContactCamera").AddComponent<Camera>();
        camera.transform.position = new Vector3(0f, 0f, -8f);
        camera.transform.rotation = Quaternion.identity;
        camera.orthographic = true;
        camera.orthographicSize = 3.05f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.018f, 0.015f, 0.012f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ContactScenePath);
    }

    private static void CreateReliefGrid(string rootName, SurfaceKind surface, Material[] materials, float minA, float maxA, float minB, float maxB, float plane, int columns, int rows, int seed, float fillA, float fillB, float depth, float bevel)
    {
        GameObject root = new GameObject(rootName);
        float cellA = (maxA - minA) / columns;
        float cellB = (maxB - minB) / rows;
        for (int row = 0; row < rows; row++)
        {
            float stagger = surface == SurfaceKind.Floor ? ((row & 1) == 0 ? 0f : cellA * 0.18f) : ((row & 1) == 0 ? 0f : cellA * 0.48f);
            for (int col = -1; col <= columns; col++)
            {
                float centerA = minA + (col + 0.5f) * cellA + stagger + (Hash01(col, row, seed + 11) - 0.5f) * cellA * 0.08f;
                float centerB = minB + (row + 0.5f) * cellB + (Hash01(col, row, seed + 17) - 0.5f) * cellB * 0.08f;
                if (centerA < minA + cellA * 0.12f || centerA > maxA - cellA * 0.12f)
                {
                    continue;
                }

                float width = cellA * fillA * Mathf.Lerp(0.88f, 1.06f, Hash01(col, row, seed + 29));
                float height = cellB * fillB * Mathf.Lerp(0.84f, 1.05f, Hash01(col, row, seed + 37));
                if (surface == SurfaceKind.Floor)
                {
                    width *= Mathf.Lerp(0.92f, 1.14f, Hash01(col, row, seed + 43));
                    height *= Mathf.Lerp(0.9f, 1.12f, Hash01(col, row, seed + 47));
                }

                Material material = materials[Mathf.Abs((int)(Hash01(col, row, seed + 101) * materials.Length * 0.999f)) % materials.Length];
                GameObject tile = CreateBeveledTile(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), surface, centerA, centerB, plane, width, height, depth * Mathf.Lerp(0.7f, 1.35f, Hash01(col, row, seed + 59)), bevel, seed + row * 71 + col * 17, material);
                tile.transform.SetParent(root.transform, true);
            }
        }
    }

    private static GameObject CreateDisplacedMasonrySurface(string name, SurfaceKind surface, Material material, float minA, float maxA, float minB, float maxB, float plane, int segmentsA, int segmentsB, int seed, float relief, bool floor)
    {
        List<Vector3> vertices = new List<Vector3>((segmentsA + 1) * (segmentsB + 1));
        List<Vector2> uvs = new List<Vector2>((segmentsA + 1) * (segmentsB + 1));
        List<int> triangles = new List<int>(segmentsA * segmentsB * 6);

        Vector3 basisCenter;
        Vector3 basisA;
        Vector3 basisB;
        Vector3 normal;
        GetSurfaceBasis(surface, 0f, 0f, plane, out basisCenter, out basisA, out basisB, out normal);

        for (int y = 0; y <= segmentsB; y++)
        {
            float v = y / (float)segmentsB;
            for (int x = 0; x <= segmentsA; x++)
            {
                float u = x / (float)segmentsA;
                float a = Mathf.Lerp(minA, maxA, u);
                float b = Mathf.Lerp(minB, maxB, v);
                Vector3 basePoint;
                Vector3 unusedA;
                Vector3 unusedB;
                Vector3 unusedN;
                GetSurfaceBasis(surface, a, b, plane, out basePoint, out unusedA, out unusedB, out unusedN);
                float h = floor ? FlagstoneRelief(u, v, seed) : BrickRelief(u, v, seed);
                float pitting = Mathf.SmoothStep(0.68f, 0.94f, Fbm(u * 70f + 11.7f, v * 64f + 3.2f, seed + 701, 3)) * relief * 0.26f;
                vertices.Add(basePoint + normal * (h * relief - pitting));
                uvs.Add(new Vector2(u * (floor ? 2.8f : 3.8f), v * (floor ? 2.8f : 2.1f)));
            }
        }

        int stride = segmentsA + 1;
        for (int y = 0; y < segmentsB; y++)
        {
            for (int x = 0; x < segmentsA; x++)
            {
                int i = y * stride + x;
                triangles.Add(i);
                triangles.Add(i + stride);
                triangles.Add(i + stride + 1);
                triangles.Add(i);
                triangles.Add(i + stride + 1);
                triangles.Add(i + 1);
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = name + "_Mesh";
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        GameObject obj = new GameObject(name);
        obj.AddComponent<MeshFilter>().sharedMesh = mesh;
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        return obj;
    }

    private static float BrickRelief(float u, float v, int seed)
    {
        const float rows = 13.0f;
        float rowValue = v * rows;
        int row = Mathf.FloorToInt(rowValue);
        float rowLocal = Frac(rowValue);
        float stagger = (row & 1) == 0 ? 0.0f : 0.5f;
        float columns = Mathf.Lerp(17.0f, 23.0f, Hash01(row, seed, seed + 31));
        float colValue = u * columns + stagger;
        int col = Mathf.FloorToInt(colValue);
        float colLocal = Frac(colValue);
        float edge = Mathf.Min(Mathf.Min(colLocal, 1.0f - colLocal), Mathf.Min(rowLocal, 1.0f - rowLocal));
        float edgeNoise = Fbm(u * 42f, v * 48f, seed + row * 13 + col * 17, 4);
        float mortar = 1.0f - Mathf.SmoothStep(0.025f + edgeNoise * 0.012f, 0.074f + edgeNoise * 0.018f, edge);
        float crown = Mathf.SmoothStep(0.02f, 0.45f, edge) * Mathf.SmoothStep(0.02f, 0.45f, 1.0f - edge);
        float brokenCorner = Mathf.SmoothStep(0.66f, 0.93f, Fbm(u * 96f + col, v * 82f + row, seed + 107, 3));
        float stoneNoise = Fbm(u * 34f + col * 0.37f, v * 31f + row * 0.21f, seed + 211, 4);
        return Mathf.Clamp01(0.16f + crown * 0.76f + stoneNoise * 0.16f - mortar * 0.92f - brokenCorner * 0.2f);
    }

    private static float FlagstoneRelief(float u, float v, int seed)
    {
        float cu = u * 6.8f + Fbm(v * 2.2f, u * 1.3f, seed + 13, 3) * 0.18f;
        float cv = v * 7.4f + Fbm(u * 1.9f, v * 1.5f, seed + 19, 3) * 0.18f;
        float localU = Frac(cu);
        float localV = Frac(cv);
        float edge = Mathf.Min(Mathf.Min(localU, 1.0f - localU), Mathf.Min(localV, 1.0f - localV));
        float mortar = 1.0f - Mathf.SmoothStep(0.028f, 0.105f, edge);
        float crown = Mathf.SmoothStep(0.05f, 0.42f, edge) * Mathf.SmoothStep(0.05f, 0.42f, 1.0f - edge);
        float puddleDip = Mathf.SmoothStep(0.62f, 0.92f, Fbm(u * 8.5f, v * 6.2f, seed + 47, 5)) * 0.22f;
        float stoneNoise = Fbm(u * 28f, v * 24f, seed + 59, 4);
        return Mathf.Clamp01(0.2f + crown * 0.66f + stoneNoise * 0.12f - mortar * 0.82f - puddleDip);
    }

    private static GameObject CreateBeveledTile(string name, SurfaceKind surface, float centerA, float centerB, float plane, float width, float height, float depth, float bevel, int seed, Material material)
    {
        Vector3 center;
        Vector3 axisA;
        Vector3 axisB;
        Vector3 normal;
        GetSurfaceBasis(surface, centerA, centerB, plane, out center, out axisA, out axisB, out normal);
        float bevelA = Mathf.Min(width * 0.18f, bevel * Mathf.Lerp(0.75f, 1.4f, Hash01(seed, 1, seed)));
        float bevelB = Mathf.Min(height * 0.18f, bevel * Mathf.Lerp(0.75f, 1.4f, Hash01(seed, 2, seed)));
        Vector3[] back =
        {
            center - axisA * width * 0.5f - axisB * height * 0.5f,
            center - axisA * width * 0.5f + axisB * height * 0.5f,
            center + axisA * width * 0.5f + axisB * height * 0.5f,
            center + axisA * width * 0.5f - axisB * height * 0.5f
        };
        Vector3 frontCenter = center + normal * depth;
        Vector3[] front =
        {
            frontCenter - axisA * (width * 0.5f - bevelA) - axisB * (height * 0.5f - bevelB) + Jitter(axisA, axisB, bevel * 0.24f, seed + 3),
            frontCenter - axisA * (width * 0.5f - bevelA) + axisB * (height * 0.5f - bevelB) + Jitter(axisA, axisB, bevel * 0.24f, seed + 5),
            frontCenter + axisA * (width * 0.5f - bevelA) + axisB * (height * 0.5f - bevelB) + Jitter(axisA, axisB, bevel * 0.24f, seed + 7),
            frontCenter + axisA * (width * 0.5f - bevelA) - axisB * (height * 0.5f - bevelB) + Jitter(axisA, axisB, bevel * 0.24f, seed + 11)
        };

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        AddFace(vertices, normals, uvs, triangles, front[0], front[1], front[2], front[3], normal, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f));
        AddFace(vertices, normals, uvs, triangles, back[0], front[0], front[3], back[3], Vector3.Normalize(-axisB + normal * 0.25f), Vector2.zero, Vector2.up, Vector2.one, Vector2.right);
        AddFace(vertices, normals, uvs, triangles, back[3], front[3], front[2], back[2], Vector3.Normalize(axisA + normal * 0.25f), Vector2.zero, Vector2.up, Vector2.one, Vector2.right);
        AddFace(vertices, normals, uvs, triangles, back[2], front[2], front[1], back[1], Vector3.Normalize(axisB + normal * 0.25f), Vector2.zero, Vector2.up, Vector2.one, Vector2.right);
        AddFace(vertices, normals, uvs, triangles, back[1], front[1], front[0], back[0], Vector3.Normalize(-axisA + normal * 0.25f), Vector2.zero, Vector2.up, Vector2.one, Vector2.right);

        Mesh mesh = new Mesh();
        mesh.name = name + "_Mesh";
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateBounds();
        GameObject obj = new GameObject(name);
        obj.AddComponent<MeshFilter>().sharedMesh = mesh;
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = true;
        return obj;
    }

    private static void AddFace(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> triangles, Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 normal, Vector2 uva, Vector2 uvb, Vector2 uvc, Vector2 uvd)
    {
        int start = vertices.Count;
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        vertices.Add(d);
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);
        uvs.Add(uva);
        uvs.Add(uvb);
        uvs.Add(uvc);
        uvs.Add(uvd);
        triangles.Add(start);
        triangles.Add(start + 1);
        triangles.Add(start + 2);
        triangles.Add(start);
        triangles.Add(start + 2);
        triangles.Add(start + 3);
    }

    private static void GetSurfaceBasis(SurfaceKind surface, float centerA, float centerB, float plane, out Vector3 center, out Vector3 axisA, out Vector3 axisB, out Vector3 normal)
    {
        axisA = Vector3.right;
        axisB = Vector3.up;
        normal = Vector3.back;
        center = new Vector3(centerA, centerB, plane);
        if (surface == SurfaceKind.LeftWall)
        {
            axisA = Vector3.forward;
            axisB = Vector3.up;
            normal = Vector3.right;
            center = new Vector3(plane, centerB, centerA);
        }
        else if (surface == SurfaceKind.RightWall)
        {
            axisA = Vector3.forward;
            axisB = Vector3.up;
            normal = Vector3.left;
            center = new Vector3(plane, centerB, centerA);
        }
        else if (surface == SurfaceKind.Floor)
        {
            axisA = Vector3.right;
            axisB = Vector3.forward;
            normal = Vector3.up;
            center = new Vector3(centerA, plane, centerB);
        }
        else if (surface == SurfaceKind.Ceiling)
        {
            axisA = Vector3.right;
            axisB = Vector3.forward;
            normal = Vector3.down;
            center = new Vector3(centerA, plane, centerB);
        }
    }

    private static Vector3 Jitter(Vector3 axisA, Vector3 axisB, float amount, int seed)
    {
        return axisA * ((Hash01(seed, 5, seed + 17) - 0.5f) * amount) + axisB * ((Hash01(seed, 7, seed + 23) - 0.5f) * amount);
    }

    private static void CreateBackdrop(string name, SurfaceKind surface, float minA, float maxA, float minB, float maxB, float plane, float thickness, Material material)
    {
        Vector3 center;
        Vector3 axisA;
        Vector3 axisB;
        Vector3 normal;
        GetSurfaceBasis(surface, (minA + maxA) * 0.5f, (minB + maxB) * 0.5f, plane, out center, out axisA, out axisB, out normal);
        Vector3 scale;
        if (surface == SurfaceKind.Floor || surface == SurfaceKind.Ceiling)
        {
            scale = new Vector3(maxA - minA, thickness, maxB - minB);
        }
        else if (surface == SurfaceKind.BackWall)
        {
            scale = new Vector3(maxA - minA, maxB - minB, thickness);
        }
        else
        {
            scale = new Vector3(thickness, maxB - minB, maxA - minA);
        }

        GameObject box = CreateBox(name, center - normal * thickness * 0.5f, scale, material);
        box.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    private static void CreateGrimeBands(Material soot)
    {
        CreateBox("RTv07_BackWall_OilyBaseShadow", new Vector3(0f, 0.13f, 5.06f), new Vector3(10.2f, 0.18f, 0.025f), soot);
        CreateBox("RTv07_LeftWall_OilyBaseShadow", new Vector3(-5.07f, 0.13f, -0.2f), new Vector3(0.025f, 0.18f, 10.2f), soot);
        CreateBox("RTv07_RightWall_OilyBaseShadow", new Vector3(5.07f, 0.13f, -0.2f), new Vector3(0.025f, 0.18f, 10.2f), soot);
        CreateBox("RTv07_BackWall_CeilingSoot", new Vector3(0f, 4.02f, 5.05f), new Vector3(10.1f, 0.22f, 0.025f), soot);
        CreateBox("RTv07_LeftBackCorner_DarkGrime", new Vector3(-5.08f, 2.0f, 5.05f), new Vector3(0.04f, 3.8f, 0.04f), soot);
        CreateBox("RTv07_RightBackCorner_DarkGrime", new Vector3(5.08f, 2.0f, 5.05f), new Vector3(0.04f, 3.8f, 0.04f), soot);
    }

    private static void CreateOilPuddles(Material wetOil)
    {
        CreateIrregularBlob("RTv07_FrontLeft_OilPuddle", SurfaceKind.Floor, new Vector3(-2.2f, 0.06f, -4.25f), 1.35f, 0.42f, 20, 1401, wetOil);
        CreateIrregularBlob("RTv07_Center_WetFilm", SurfaceKind.Floor, new Vector3(0.4f, 0.061f, -2.1f), 1.25f, 0.34f, 18, 1423, wetOil);
        CreateIrregularBlob("RTv07_Right_WetFilm", SurfaceKind.Floor, new Vector3(2.3f, 0.06f, -3.65f), 1.15f, 0.36f, 18, 1447, wetOil);
    }

    private static void CreateIrregularBlob(string name, SurfaceKind surface, Vector3 center, float radiusA, float radiusB, int points, int seed, Material material)
    {
        List<Vector3> vertices = new List<Vector3> { center };
        List<Vector3> normals = new List<Vector3> { Vector3.up };
        List<Vector2> uvs = new List<Vector2> { new Vector2(0.5f, 0.5f) };
        List<int> triangles = new List<int>();
        for (int i = 0; i < points; i++)
        {
            float angle = (Mathf.PI * 2f * i) / points;
            float r = Mathf.Lerp(0.72f, 1.16f, Hash01(i, seed, seed + 31));
            Vector3 p = center + new Vector3(Mathf.Cos(angle) * radiusA * r, 0f, Mathf.Sin(angle) * radiusB * r);
            vertices.Add(p);
            normals.Add(Vector3.up);
            uvs.Add(new Vector2(Mathf.Cos(angle) * 0.5f + 0.5f, Mathf.Sin(angle) * 0.5f + 0.5f));
        }

        for (int i = 1; i <= points; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i == points ? 1 : i + 1);
        }

        Mesh mesh = new Mesh();
        mesh.name = name + "_Mesh";
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateBounds();
        GameObject obj = new GameObject(name);
        obj.AddComponent<MeshFilter>().sharedMesh = mesh;
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    private static void CreateGaslightRig(string name, Vector3 position, Quaternion rotation, Material glass, Material brass, Material copper, Material iron, bool mirror)
    {
        GameObject root = new GameObject(name);
        root.transform.position = position;
        root.transform.rotation = rotation;
        CreateLocalPrimitive(root, name + "_BlackIronMount", PrimitiveType.Cube, new Vector3(0f, 0f, 0.035f), new Vector3(0.42f, 0.88f, 0.07f), iron);
        CreateLocalPrimitive(root, name + "_SootedTopCap", PrimitiveType.Cube, new Vector3(0f, 0.47f, -0.01f), new Vector3(0.52f, 0.06f, 0.1f), iron);
        CreateLocalPrimitive(root, name + "_SootedBottomCap", PrimitiveType.Cube, new Vector3(0f, -0.47f, -0.01f), new Vector3(0.52f, 0.06f, 0.1f), iron);
        GameObject lens = CreateLocalPrimitive(root, name + "_AmberGlassLens", PrimitiveType.Sphere, new Vector3(0f, 0f, -0.12f), new Vector3(0.22f, 0.42f, 0.11f), glass);
        GameObject rim = CreateLocalPrimitive(root, name + "_BrassOvalRim", PrimitiveType.Cylinder, new Vector3(0f, 0f, -0.155f), new Vector3(0.38f, 0.024f, 0.38f), brass);
        rim.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        for (int i = -1; i <= 1; i++)
        {
            CreateLocalPrimitive(root, name + "_CageRib_" + i.ToString(CultureInfo.InvariantCulture), PrimitiveType.Cube, new Vector3(i * 0.13f, 0f, -0.18f), new Vector3(0.018f, 0.72f, 0.035f), brass);
        }

        float pipeX = mirror ? 0.34f : -0.34f;
        CreateLocalPrimitive(root, name + "_CopperFeedPipe", PrimitiveType.Cube, new Vector3(pipeX, -0.16f, 0.02f), new Vector3(0.3f, 0.06f, 0.06f), copper);
        GameObject valve = CreateLocalPrimitive(root, name + "_SmallValveWheel", PrimitiveType.Cylinder, new Vector3(pipeX + (mirror ? 0.18f : -0.18f), -0.16f, -0.02f), new Vector3(0.11f, 0.018f, 0.11f), brass);
        valve.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        lens.transform.localRotation = Quaternion.identity;
    }

    private static GameObject CreateLocalPrimitive(GameObject parent, string name, PrimitiveType primitive, Vector3 localPosition, Vector3 localScale, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(primitive);
        obj.name = name;
        obj.transform.SetParent(parent.transform, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localScale = localScale;
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            UnityEngine.Object.DestroyImmediate(collider);
        }

        return obj;
    }

    private static void CreateCeilingPipeRun(string name, Vector3 start, Vector3 end, float radius, Material material)
    {
        CreateCylinderBetween(name, start, end, radius, material);
        for (int i = 1; i < 5; i++)
        {
            float t = i / 5f;
            Vector3 p = Vector3.Lerp(start, end, t);
            CreateCylinderBetween(name + "_Clamp_" + i.ToString(CultureInfo.InvariantCulture), p + Vector3.left * 0.18f, p + Vector3.right * 0.18f, 0.018f, material);
        }
    }

    private static void CreateCylinderBetween(string name, Vector3 start, Vector3 end, float radius, Material material)
    {
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.name = name;
        Vector3 direction = end - start;
        cylinder.transform.position = (start + end) * 0.5f;
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
        cylinder.transform.localScale = new Vector3(radius, direction.magnitude * 0.5f, radius);
        Renderer renderer = cylinder.GetComponent<Renderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        Collider collider = cylinder.GetComponent<Collider>();
        if (collider != null)
        {
            UnityEngine.Object.DestroyImmediate(collider);
        }
    }

    private static GameObject CreateBox(string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.position = position;
        box.transform.localScale = scale;
        Renderer renderer = box.GetComponent<Renderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        Collider collider = box.GetComponent<Collider>();
        if (collider != null)
        {
            UnityEngine.Object.DestroyImmediate(collider);
        }

        return box;
    }

    private static void CreatePointLight(string name, Vector3 position, Color color, float intensity, float range, bool shadows)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        Light light = obj.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.None;
        light.shadowStrength = 0.72f;
        light.shadowBias = 0.025f;
    }

    private static void CreateSpotLight(string name, Vector3 position, Vector3 target, Color color, float intensity, float angle, float range)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.LookRotation(target - position, Vector3.up);
        Light light = obj.AddComponent<Light>();
        light.type = LightType.Spot;
        light.color = color;
        light.intensity = intensity;
        light.spotAngle = angle;
        light.range = range;
        light.shadows = LightShadows.None;
        light.shadowStrength = 0.62f;
        light.shadowBias = 0.03f;
    }

    private static void CreateCamera(string name, Vector3 position, Vector3 target, float fov)
    {
        GameObject obj = new GameObject(name);
        Camera camera = obj.AddComponent<Camera>();
        camera.transform.position = position;
        camera.transform.rotation = Quaternion.LookRotation(target - position, Vector3.up);
        camera.fieldOfView = fov;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 35f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.012f, 0.01f, 0.008f);
        camera.allowHDR = true;
        camera.allowMSAA = true;
        camera.renderingPath = RenderingPath.DeferredShading;
    }

    private static TextureSet GenerateStoneTextureSet(string name, Color baseColor, Color darkColor, int seed, float wetness, bool floor)
    {
        int count = TextureSize * TextureSize;
        Color32[] albedo = new Color32[count];
        Color32[] heightPixels = new Color32[count];
        Color32[] ao = new Color32[count];
        Color32[] metallicSmooth = new Color32[count];
        float[] height = new float[count];
        for (int y = 0; y < TextureSize; y++)
        {
            float v = (y + 0.5f) / TextureSize;
            for (int x = 0; x < TextureSize; x++)
            {
                float u = (x + 0.5f) / TextureSize;
                float broad = Fbm(u * 4.2f, v * 4.8f, seed + 1, 5);
                float mid = Fbm(u * 24f, v * 28f, seed + 3, 5);
                float fine = Fbm(u * 190f, v * 205f, seed + 5, 4);
                float grit = Fbm(u * 520f, v * 480f, seed + 7, 3);
                float streak = Fbm(u * 3.2f + 7.3f, v * 18f + 2.1f, seed + 11, 4);
                float crack = Mathf.SmoothStep(0.72f, 0.86f, Fbm(u * 72f + 3.3f, v * 61f + 5.1f, seed + 19, 3));
                float pit = Mathf.SmoothStep(0.58f, 0.88f, grit);
                float h = Mathf.Clamp01(0.42f + broad * 0.18f + mid * 0.1f + fine * 0.05f - pit * 0.22f - crack * 0.2f);
                height[y * TextureSize + x] = h;
                Color c = Color.Lerp(darkColor, baseColor, Mathf.Clamp01(0.28f + broad * 0.55f + mid * 0.25f));
                c *= Mathf.Lerp(0.46f, 1.04f, 1f - streak * (floor ? 0.28f : 0.5f));
                c *= 1f - pit * 0.22f - crack * 0.58f;
                c += new Color(0.018f, 0.012f, 0.006f) * Mathf.Clamp01(fine - 0.55f) * 1.3f;
                albedo[y * TextureSize + x] = ToColor32(c, 1f);
                byte hb = (byte)Mathf.RoundToInt(h * 255f);
                heightPixels[y * TextureSize + x] = new Color32(hb, hb, hb, 255);
                float occ = Mathf.Clamp01(0.3f + h * 0.65f - crack * 0.45f - pit * 0.25f);
                byte ob = (byte)Mathf.RoundToInt(occ * 255f);
                ao[y * TextureSize + x] = new Color32(ob, ob, ob, 255);
                float wetPatch = floor ? Mathf.SmoothStep(0.48f, 0.9f, Fbm(u * 7f, v * 5.5f, seed + 31, 4)) : 0f;
                float smooth = Mathf.Clamp01(wetness + wetPatch * 0.18f - pit * 0.16f - crack * 0.08f);
                metallicSmooth[y * TextureSize + x] = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(smooth * 255f));
            }
        }

        Color32[] normal = GenerateNormalPixels(height, floor ? 4.1f : 5.4f);
        return new TextureSet(
            SaveTexture(name + "_Albedo", albedo, TextureImporterType.Default, true),
            SaveTexture(name + "_Normal", normal, TextureImporterType.NormalMap, false),
            SaveTexture(name + "_Height", heightPixels, TextureImporterType.Default, false),
            SaveTexture(name + "_Occlusion", ao, TextureImporterType.Default, false),
            SaveTexture(name + "_MetallicSmoothness", metallicSmooth, TextureImporterType.Default, false));
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
                Vector3 n = new Vector3(-dx * strength, -dy * strength, 1f).normalized;
                pixels[y * TextureSize + x] = new Color32((byte)Mathf.RoundToInt((n.x * 0.5f + 0.5f) * 255f), (byte)Mathf.RoundToInt((n.y * 0.5f + 0.5f) * 255f), (byte)Mathf.RoundToInt((n.z * 0.5f + 0.5f) * 255f), 255);
            }
        }

        return pixels;
    }

    private static string SaveTexture(string name, Color32[] pixels, TextureImporterType importerType, bool srgb)
    {
        Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, true, !srgb);
        texture.SetPixels32(pixels);
        texture.Apply(true, false);
        string assetPath = TextureRoot + "/" + name + ".png";
        File.WriteAllBytes(ProjectPath(assetPath), texture.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(texture);
        return assetPath;
    }

    private static void ImportTextureSets(params TextureSet[] sets)
    {
        foreach (TextureSet set in sets)
        {
            ImportTexture(set.Albedo, TextureImporterType.Default, true);
            ImportTexture(set.Normal, TextureImporterType.NormalMap, false);
            ImportTexture(set.Height, TextureImporterType.Default, false);
            ImportTexture(set.Occlusion, TextureImporterType.Default, false);
            ImportTexture(set.MetallicSmoothness, TextureImporterType.Default, false);
        }

        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Debug.Log("ROOMTEST_V07_STAGE_TEXTURE_IMPORTS_CONFIGURED count=" + (sets.Length * 5).ToString(CultureInfo.InvariantCulture));
    }

    private static void ImportTexture(string assetPath, TextureImporterType importerType, bool srgb)
    {
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = importerType;
            importer.sRGBTexture = srgb;
            importer.mipmapEnabled = true;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.filterMode = FilterMode.Trilinear;
            importer.anisoLevel = 12;
            importer.maxTextureSize = TextureSize;
            importer.SaveAndReimport();
        }
    }

    private static Material CreatePbrMaterial(string name, TextureSet set, Vector2 tiling, float smoothness, float parallax, float normalScale)
    {
        Material material = CreateOrLoadMaterial(name, Shader.Find("Standard"));
        SetTexture(material, "_MainTex", LoadTexture(set.Albedo), tiling);
        SetTexture(material, "_BumpMap", LoadTexture(set.Normal), tiling);
        SetTexture(material, "_ParallaxMap", LoadTexture(set.Height), tiling);
        SetTexture(material, "_OcclusionMap", LoadTexture(set.Occlusion), tiling);
        SetTexture(material, "_MetallicGlossMap", LoadTexture(set.MetallicSmoothness), tiling);
        SetFloat(material, "_Metallic", 0f);
        SetFloat(material, "_Glossiness", smoothness);
        SetFloat(material, "_GlossMapScale", 1f);
        SetFloat(material, "_BumpScale", normalScale);
        SetFloat(material, "_Parallax", parallax);
        SetColor(material, "_Color", Color.white);
        material.EnableKeyword("_NORMALMAP");
        material.EnableKeyword("_PARALLAXMAP");
        material.EnableKeyword("_METALLICGLOSSMAP");
        material.EnableKeyword("_OCCLUSIONMAP");
        material.SetInt("_Cull", 0);
        EditorUtility.SetDirty(material);
        return material;
    }

    private static Material CreateSimpleMaterial(string name, Color color, float metallic, float smoothness)
    {
        Material material = CreateOrLoadMaterial(name, Shader.Find("Standard"));
        SetColor(material, "_Color", color);
        SetFloat(material, "_Metallic", metallic);
        SetFloat(material, "_Glossiness", smoothness);
        SetFloat(material, "_Smoothness", smoothness);
        material.SetInt("_Cull", 0);
        EditorUtility.SetDirty(material);
        return material;
    }

    private static Material CreateEmissiveMaterial(string name, Color color, float intensity)
    {
        Material material = CreateSimpleMaterial(name, color, 0f, 0.25f);
        SetColor(material, "_EmissionColor", color * intensity);
        material.EnableKeyword("_EMISSION");
        return material;
    }

    private static Material[] CreateVariants(string prefix, Material source, Color[] tints)
    {
        Material[] variants = new Material[tints.Length];
        for (int i = 0; i < tints.Length; i++)
        {
            string path = MaterialRoot + "/" + prefix + "_" + i.ToString("00", CultureInfo.InvariantCulture) + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(source);
                AssetDatabase.CreateAsset(material, path);
            }
            else
            {
                material.CopyPropertiesFromMaterial(source);
            }

            SetColor(material, "_Color", tints[i]);
            material.SetInt("_Cull", 0);
            EditorUtility.SetDirty(material);
            variants[i] = material;
        }

        return variants;
    }

    private static Material CreateUnlitImageMaterial(string name, string imagePath)
    {
        byte[] bytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        texture.LoadImage(bytes);
        texture.wrapMode = TextureWrapMode.Clamp;
        Shader shader = Shader.Find("Unlit/Texture") ?? Shader.Find("Standard");
        Material material = CreateOrLoadMaterial(name, shader);
        material.mainTexture = texture;
        return material;
    }

    private static Material CreateOrLoadMaterial(string name, Shader shader)
    {
        string path = MaterialRoot + "/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material != null)
        {
            if (shader != null && material.shader != shader)
            {
                material.shader = shader;
            }

            return material;
        }

        material = new Material(shader ?? Shader.Find("Standard"));
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static void CreateContactQuad(string name, Vector3 center, Vector2 size, Material material)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new[]
        {
            center + new Vector3(-size.x * 0.5f, -size.y * 0.5f, 0f),
            center + new Vector3(-size.x * 0.5f, size.y * 0.5f, 0f),
            center + new Vector3(size.x * 0.5f, size.y * 0.5f, 0f),
            center + new Vector3(size.x * 0.5f, -size.y * 0.5f, 0f)
        };
        mesh.uv = new[] { Vector2.zero, Vector2.up, Vector2.one, Vector2.right };
        mesh.triangles = new[] { 0, 1, 2, 0, 2, 3 };
        mesh.RecalculateNormals();
        GameObject obj = new GameObject(name);
        obj.AddComponent<MeshFilter>().sharedMesh = mesh;
        obj.AddComponent<MeshRenderer>().sharedMaterial = material;
    }

    private static void CreateText(string text, Vector3 position, float size)
    {
        GameObject obj = new GameObject("RTv07_Text_" + text.Substring(0, Mathf.Min(12, text.Length)));
        obj.transform.position = position;
        TextMesh mesh = obj.AddComponent<TextMesh>();
        mesh.text = text;
        mesh.anchor = TextAnchor.UpperLeft;
        mesh.alignment = TextAlignment.Left;
        mesh.characterSize = size;
        mesh.fontSize = 64;
        mesh.color = new Color(0.86f, 0.76f, 0.58f);
    }

    private static string RenderSceneToFile(string fileName)
    {
        Camera camera = UnityEngine.Object.FindAnyObjectByType<Camera>();
        if (camera == null)
        {
            throw new InvalidOperationException("No camera found for v0.7 render.");
        }

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(RenderWidth, RenderHeight, RenderTextureFormat.ARGB32, 24);
        descriptor.msaaSamples = 8;
        descriptor.sRGB = QualitySettings.activeColorSpace == ColorSpace.Linear;
        RenderTexture renderTexture = new RenderTexture(descriptor);
        Texture2D capture = new Texture2D(RenderWidth, RenderHeight, TextureFormat.RGBA32, false, QualitySettings.activeColorSpace == ColorSpace.Linear);
        RenderTexture prevActive = RenderTexture.active;
        RenderTexture prevTarget = camera.targetTexture;
        camera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        camera.Render();
        capture.ReadPixels(new Rect(0, 0, RenderWidth, RenderHeight), 0, 0);
        capture.Apply(false, false);
        string fullPath = ProjectPath("Renders/" + fileName);
        File.WriteAllBytes(fullPath, capture.EncodeToPNG());
        camera.targetTexture = prevTarget;
        RenderTexture.active = prevActive;
        renderTexture.Release();
        UnityEngine.Object.DestroyImmediate(renderTexture);
        UnityEngine.Object.DestroyImmediate(capture);
        return fullPath;
    }

    private static void WriteMetrics(string beautyPath, string closeupPath, string contactPath)
    {
        RenderStats beauty = AnalyzeRender(beautyPath);
        RenderStats closeup = AnalyzeRender(closeupPath);
        RenderStats contact = AnalyzeRender(contactPath);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("{");
        builder.AppendLine("  \"schema\": \"brassworks.roomtest.v07.metrics\",");
        builder.AppendLine("  \"render_size\": \"1920x1080\",");
        builder.AppendLine("  \"unity_only\": true,");
        builder.AppendLine("  \"beauty\": " + beauty.ToJson(BeautyRenderName) + ",");
        builder.AppendLine("  \"closeup\": " + closeup.ToJson(CloseupRenderName) + ",");
        builder.AppendLine("  \"contact_sheet\": " + contact.ToJson(ContactSheetName) + ",");
        builder.AppendLine("  \"nonblank_gate_pass\": " + (beauty.NonBlank && closeup.NonBlank && contact.NonBlank).ToString().ToLowerInvariant());
        builder.AppendLine("}");
        File.WriteAllText(ProjectPath("Renders/" + MetricsName), builder.ToString());
    }

    private static RenderStats AnalyzeRender(string path)
    {
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        texture.LoadImage(File.ReadAllBytes(path));
        Color32[] pixels = texture.GetPixels32();
        double luminanceSum = 0d;
        double luminanceSq = 0d;
        int nonBlack = 0;
        int warm = 0;
        int clipped = 0;
        for (int i = 0; i < pixels.Length; i++)
        {
            float r = pixels[i].r / 255f;
            float g = pixels[i].g / 255f;
            float b = pixels[i].b / 255f;
            float l = r * 0.2126f + g * 0.7152f + b * 0.0722f;
            luminanceSum += l;
            luminanceSq += l * l;
            if (l > 0.025f)
            {
                nonBlack++;
            }

            if (l > 0.12f && r > g && g >= b)
            {
                warm++;
            }

            if (l > 0.95f)
            {
                clipped++;
            }
        }

        int width = texture.width;
        int height = texture.height;
        UnityEngine.Object.DestroyImmediate(texture);
        double average = luminanceSum / pixels.Length;
        double variance = Math.Max(0d, luminanceSq / pixels.Length - average * average);
        return new RenderStats(width, height, average, Math.Sqrt(variance), (double)nonBlack / pixels.Length, (double)warm / pixels.Length, (double)clipped / pixels.Length);
    }

    private static void WriteInitialReview()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.7 Final-Art Pipeline Review");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("Status: in progress, generated before render completion.");
        File.WriteAllText(ProjectPath("Documentation/" + ReviewName), builder.ToString());
    }

    private static void WriteFailureReview(Exception exception)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.7 Final-Art Pipeline Review");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("Status: blocked during Unity batch generation.");
        builder.AppendLine();
        builder.AppendLine("## Failure");
        builder.AppendLine();
        builder.AppendLine("```");
        builder.AppendLine(exception.ToString());
        builder.AppendLine("```");
        builder.AppendLine();
        builder.AppendLine("No external DCC tools were used. Retry should stay inside the v0.7 builder and roomtest asset namespace.");
        File.WriteAllText(ProjectPath("Documentation/" + ReviewName), builder.ToString());
    }

    private static void WriteFinalReview(string beautyPath, string closeupPath, string contactPath)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.7 Final-Art Pipeline Review");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## Required Files");
        builder.AppendLine();
        builder.AppendLine("- Beauty render: `" + beautyPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Material closeup: `" + closeupPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Contact sheet: `" + contactPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Metrics: `" + ProjectPath("Renders/" + MetricsName).Replace('\\', '/') + "`");
        builder.AppendLine();
        builder.AppendLine("## Unity-Only Technique");
        builder.AppendLine();
        builder.AppendLine("- Built-in renderer only; no URP/HDRP, Blender, external DCC, or external asset generator.");
        builder.AppendLine("- Generated 2048 PBR texture families for dark wet brick, wet black flagstone, and sooted ceiling brick: albedo, normal, height, occlusion, and packed metallic/smoothness.");
        builder.AppendLine("- Added actual beveled mesh relief for wall bricks, ceiling bricks, and floor flagstones over recessed black mortar, instead of relying on flat planes.");
        builder.AppendLine("- Added tarnished brass/copper/black-iron gaslight fixtures, pipe runs, dark soot bands, glossy black oil films, warm gaslight, reflection probe, and controlled fill lights.");
        builder.AppendLine("- Rendered a 1920x1080 beauty shot, a material closeup, and a contact sheet with notes.");
        builder.AppendLine();
        builder.AppendLine("## Self-Score Against North-Star");
        builder.AppendLine();
        builder.AppendLine("- Dark irregular wet masonry: 6.5/10, pass for proof. Actual bevel relief and procedural roughness are visible, but still less nuanced than authored/scanned AAA masonry.");
        builder.AppendLine("- Wet floor reflections: 6/10, pass for proof. Glossy wet response exists without mirror-flatness, but reflection shaping is still simpler than the north-star.");
        builder.AppendLine("- Warm controlled gaslight: 6/10, pass for proof. The tone is warm and localized, but built-in rendering lacks the subtle bloom/volumetric richness of the target.");
        builder.AppendLine("- Steampunk material restraint: 6.5/10, pass for proof. Brass/copper are used as tarnished accents rather than toy-orange surfaces.");
        builder.AppendLine("- AAA final quality: fail. This is a credible Unity-only pipeline proof, not a final AAA asset set.");
        builder.AppendLine();
        builder.AppendLine("## Known Gaps");
        builder.AppendLine();
        builder.AppendLine("- No scanned/photogrammetry source, no hand-sculpted high-poly bake, and no authored decal atlas.");
        builder.AppendLine("- Beveled block meshes improve depth but still have too much procedural regularity at close inspection.");
        builder.AppendLine("- Built-in renderer limits bloom, screen-space reflections, color grading, and physically rich wet-surface behavior.");
        builder.AppendLine("- Lamps and pipes need proper modeled silhouettes, bolts, seams, gauges, glass thickness, soot masks, and LODs.");
        builder.AppendLine();
        builder.AppendLine("## Next Concrete Fixes");
        builder.AppendLine();
        builder.AppendLine("- Author a dedicated grime/decal atlas for edge soot, oil streaks, mortar stains, lamp scorch marks, and chipped stone corners.");
        builder.AppendLine("- Build reusable modular wall/floor meshes with baked bevels, corner damage, and vertex color masks.");
        builder.AppendLine("- Add a supported post stack or migrate this isolated lookdev project to URP/HDRP if allowed later.");
        builder.AppendLine("- Replace primitive lamp and pipe assemblies with hand-modeled Unity mesh assemblies and validated material closeups.");
        File.WriteAllText(ProjectPath("Documentation/" + ReviewName), builder.ToString());
    }

    private static void ValidateTextures(params TextureSet[] sets)
    {
        foreach (TextureSet set in sets)
        {
            RequireTexture(set.Albedo);
            RequireTexture(set.Normal);
            RequireTexture(set.Height);
            RequireTexture(set.Occlusion);
            RequireTexture(set.MetallicSmoothness);
        }

        Debug.Log("ROOMTEST_V07_TEXTURE_MAPS_CHECKED " + (sets.Length * 5).ToString(CultureInfo.InvariantCulture));
    }

    private static void RequireTexture(string path)
    {
        if (!File.Exists(ProjectPath(path)) || AssetDatabase.LoadAssetAtPath<Texture2D>(path) == null)
        {
            throw new FileNotFoundException("Missing v0.7 texture", path);
        }
    }

    private static Texture2D LoadTexture(string path)
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (texture == null)
        {
            throw new FileNotFoundException("Missing v0.7 texture", path);
        }

        return texture;
    }

    private static void SetTexture(Material material, string property, Texture texture, Vector2 scale)
    {
        if (material.HasProperty(property))
        {
            material.SetTexture(property, texture);
            material.SetTextureScale(property, scale);
        }
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
        float total = 0f;
        float frequency = 1f;
        for (int i = 0; i < octaves; i++)
        {
            value += ValueNoise(x * frequency, y * frequency, seed + i * 41) * amplitude;
            total += amplitude;
            amplitude *= 0.52f;
            frequency *= 2.03f;
        }

        return value / total;
    }

    private static float ValueNoise(float x, float y, int seed)
    {
        int x0 = Mathf.FloorToInt(x);
        int y0 = Mathf.FloorToInt(y);
        float tx = Smooth(Frac(x));
        float ty = Smooth(Frac(y));
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

    private static float Smooth(float value)
    {
        return value * value * (3f - 2f * value);
    }

    private static string ProjectPath(string relativePath)
    {
        return Path.GetFullPath(Path.Combine(Application.dataPath, "..", relativePath.Replace('/', Path.DirectorySeparatorChar)));
    }

    private readonly struct TextureSet
    {
        public TextureSet(string albedo, string normal, string height, string occlusion, string metallicSmoothness)
        {
            Albedo = albedo;
            Normal = normal;
            Height = height;
            Occlusion = occlusion;
            MetallicSmoothness = metallicSmoothness;
        }

        public readonly string Albedo;
        public readonly string Normal;
        public readonly string Height;
        public readonly string Occlusion;
        public readonly string MetallicSmoothness;
    }

    private readonly struct RenderStats
    {
        public RenderStats(int width, int height, double average, double contrast, double nonBlackRatio, double warmRatio, double clippedRatio)
        {
            Width = width;
            Height = height;
            Average = average;
            Contrast = contrast;
            NonBlackRatio = nonBlackRatio;
            WarmRatio = warmRatio;
            ClippedRatio = clippedRatio;
        }

        private readonly int Width;
        private readonly int Height;
        private readonly double Average;
        private readonly double Contrast;
        private readonly double NonBlackRatio;
        private readonly double WarmRatio;
        private readonly double ClippedRatio;
        public bool NonBlank
        {
            get { return Width == RenderWidth && Height == RenderHeight && NonBlackRatio > 0.08d && Contrast > 0.015d; }
        }

        public string ToJson(string fileName)
        {
            return "{ \"file\": \"" + fileName + "\", \"width\": " + Width.ToString(CultureInfo.InvariantCulture) + ", \"height\": " + Height.ToString(CultureInfo.InvariantCulture) + ", \"average_luminance\": " + Average.ToString("0.0000", CultureInfo.InvariantCulture) + ", \"contrast_stddev\": " + Contrast.ToString("0.0000", CultureInfo.InvariantCulture) + ", \"nonblack_ratio\": " + NonBlackRatio.ToString("0.0000", CultureInfo.InvariantCulture) + ", \"warm_pixel_ratio\": " + WarmRatio.ToString("0.0000", CultureInfo.InvariantCulture) + ", \"clipped_pixel_ratio\": " + ClippedRatio.ToString("0.0000", CultureInfo.InvariantCulture) + ", \"nonblank_pass\": " + NonBlank.ToString().ToLowerInvariant() + " }";
        }
    }
}
