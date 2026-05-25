using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RoomTestFinalBuilder
{
    private const int TextureSize = 2048;
    private const int RenderWidth = 1920;
    private const int RenderHeight = 1080;
    private const string AssetRoot = "Assets/RoomTest";
    private const string TextureRoot = AssetRoot + "/Textures";
    private const string MaterialRoot = AssetRoot + "/Materials";
    private const string SceneRoot = AssetRoot + "/Scenes";
    private const string PreviewScenePath = SceneRoot + "/Roomtest_FinalMaterialPreview_v0.3.unity";
    private const string RoomScenePath = SceneRoot + "/Roomtest_FinalBrickChamber_v0.3.unity";
    private const string PreviewRenderFileName = "roomtest_final_material_preview_v0.3.png";
    private const string RoomRenderFileName = "roomtest_final_brick_chamber_v0.3.png";
    private const string MetricsFileName = "roomtest_metrics_v0.3.json";
    private const string TargetAnalysisFileName = "ROOMTEST_V0_3_TARGET_ANALYSIS.md";
    private const string ReviewFileName = "ROOMTEST_V0_3_FINAL_LOOKDEV_REVIEW.md";
    private const string PreviewScenePathV04 = SceneRoot + "/Roomtest_RefinedMaterialPreview_v0.4.unity";
    private const string RoomScenePathV04 = SceneRoot + "/Roomtest_RefinedBrickChamber_v0.4.unity";
    private const string PreviewRenderFileNameV04 = "roomtest_refined_material_preview_v0.4.png";
    private const string RoomRenderFileNameV04 = "roomtest_refined_brick_chamber_v0.4.png";
    private const string MetricsFileNameV04 = "roomtest_metrics_v0.4.json";
    private const string TargetAnalysisFileNameV04 = "ROOMTEST_V0_4_TARGET_ANALYSIS.md";
    private const string ReviewFileNameV04 = "ROOMTEST_V0_4_REFINEMENT_REVIEW.md";
    private const string PreviewScenePathV05 = SceneRoot + "/Roomtest_MaterialDrivenPreview_v0.5.unity";
    private const string RoomScenePathV05 = SceneRoot + "/Roomtest_MaterialDrivenBrickRoom_v0.5.unity";
    private const string PreviewRenderFileNameV05 = "roomtest_material_driven_preview_v0.5.png";
    private const string RoomRenderFileNameV05 = "roomtest_material_driven_brick_room_v0.5.png";
    private const string MetricsFileNameV05 = "roomtest_metrics_v0.5.json";
    private const string TargetAnalysisFileNameV05 = "ROOMTEST_V0_5_TARGET_ANALYSIS.md";
    private const string ReviewFileNameV05 = "ROOMTEST_V0_5_ACCEPTANCE_REVIEW.md";
    private const string PreviewScenePathV06 = SceneRoot + "/Roomtest_ReferenceMatchPreview_v0.6.unity";
    private const string RoomScenePathV06 = SceneRoot + "/Roomtest_ReferenceMatchBrickRoom_v0.6.unity";
    private const string PreviewRenderFileNameV06 = "roomtest_reference_match_preview_v0.6.png";
    private const string RoomRenderFileNameV06 = "roomtest_reference_match_brick_room_v0.6.png";
    private const string MetricsFileNameV06 = "roomtest_metrics_v0.6.json";
    private const string TargetAnalysisFileNameV06 = "ROOMTEST_V0_6_TARGET_ANALYSIS.md";
    private const string ReviewFileNameV06 = "ROOMTEST_V0_6_ACCEPTANCE_REVIEW.md";

    private enum TextureProfile
    {
        Wall,
        Floor,
        Ceiling
    }

    private enum BrickSurface
    {
        BackWall,
        LeftWall,
        RightWall,
        Floor,
        Ceiling
    }

    [MenuItem("Roomtest/Build And Render Final Lookdev v0.3")]
    public static void BuildAndRenderFinalLookdevV03()
    {
        EnsureFolders();
        ConfigureProject();
        WriteTargetAnalysis();

        TextureSet wallTextures = GenerateTextureSet(
            "RTv03_DarkAgedBrickWall",
            TextureProfile.Wall,
            new Color(0.095f, 0.078f, 0.056f),
            new Color(0.018f, 0.016f, 0.014f),
            0.16f,
            311);
        TextureSet floorTextures = GenerateTextureSet(
            "RTv03_WetBlackFlagstoneFloor",
            TextureProfile.Floor,
            new Color(0.105f, 0.092f, 0.073f),
            new Color(0.019f, 0.018f, 0.016f),
            0.62f,
            337);
        TextureSet ceilingTextures = GenerateTextureSet(
            "RTv03_SootedLowBrickCeiling",
            TextureProfile.Ceiling,
            new Color(0.075f, 0.06f, 0.044f),
            new Color(0.015f, 0.014f, 0.013f),
            0.1f,
            353);
        ValidateBaseAlbedoFiles(wallTextures.Albedo, floorTextures.Albedo, ceilingTextures.Albedo);
        ValidateAssociatedMapFiles(wallTextures, floorTextures, ceilingTextures);

        Material wallBase = CreatePbrMaterial("RTv03_MAT_DarkAgedBrickWall_Base", wallTextures, new Vector2(1.15f, 1.05f), 0.16f, 0.025f, 0.85f);
        Material floorBase = CreatePbrMaterial("RTv03_MAT_WetBlackFlagstoneFloor_Base", floorTextures, new Vector2(0.85f, 0.9f), 0.65f, 0.035f, 0.75f);
        Material ceilingBase = CreatePbrMaterial("RTv03_MAT_SootedLowBrickCeiling_Base", ceilingTextures, new Vector2(1.05f, 0.95f), 0.1f, 0.02f, 0.55f);
        Material[] wallVariants = CreateMaterialVariants("RTv03_MAT_WallStone", wallBase, new[]
        {
            new Color(0.78f, 0.72f, 0.63f),
            new Color(0.9f, 0.82f, 0.7f),
            new Color(0.64f, 0.62f, 0.58f),
            new Color(0.84f, 0.74f, 0.6f),
            new Color(0.58f, 0.55f, 0.5f),
            new Color(0.96f, 0.88f, 0.72f)
        });
        Material[] floorVariants = CreateMaterialVariants("RTv03_MAT_WetFlagstone", floorBase, new[]
        {
            new Color(0.78f, 0.78f, 0.75f),
            new Color(0.9f, 0.86f, 0.78f),
            new Color(0.64f, 0.67f, 0.67f),
            new Color(0.82f, 0.76f, 0.66f)
        });
        Material[] ceilingVariants = CreateMaterialVariants("RTv03_MAT_CeilingBrick", ceilingBase, new[]
        {
            new Color(0.62f, 0.58f, 0.52f),
            new Color(0.75f, 0.66f, 0.54f),
            new Color(0.48f, 0.47f, 0.44f),
            new Color(0.68f, 0.6f, 0.5f)
        });

        Material mortarMaterial = CreateSimpleMaterial("RTv03_MAT_DeepRecessedMortar", new Color(0.012f, 0.011f, 0.01f), 0f, 0.05f);
        Material grimeMaterial = CreateSimpleMaterial("RTv03_MAT_CornerSootAndGrime", new Color(0.009f, 0.008f, 0.007f), 0f, 0.02f);
        Material lampGlassMaterial = CreateEmissiveMaterial("RTv03_MAT_BrightGaslightGlass", new Color(1f, 0.55f, 0.24f), 5.4f);
        Material brassMaterial = CreateSimpleMaterial("RTv03_MAT_AgedBrassLampFrame", new Color(0.42f, 0.28f, 0.13f), 0.18f, 0.38f);
        Material ironMaterial = CreateSimpleMaterial("RTv03_MAT_BlackIronLampBack", new Color(0.021f, 0.018f, 0.016f), 0.12f, 0.24f);

        BuildMaterialPreview(wallVariants, floorVariants, ceilingVariants, mortarMaterial, lampGlassMaterial);
        string previewPath = RenderSceneToFile(PreviewRenderFileName);

        BuildBrickChamber(wallVariants, floorVariants, ceilingVariants, mortarMaterial, grimeMaterial, lampGlassMaterial, brassMaterial, ironMaterial);
        string roomPath = RenderSceneToFile(RoomRenderFileName);
        WriteMetrics(roomPath);
        WriteReview(previewPath, roomPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ROOMTEST_FINAL_LOOKDEV_V03_COMPLETE " + roomPath);
    }

    [MenuItem("Roomtest/Build And Render Refined Lookdev v0.4")]
    public static void BuildAndRenderRefinedLookdevV04()
    {
        EnsureFolders();
        ConfigureProject();
        WriteTargetAnalysisV04();

        TextureSet wallTextures = GenerateTextureSet(
            "RTv04_DarkIrregularBrickWall",
            TextureProfile.Wall,
            new Color(0.072f, 0.061f, 0.047f),
            new Color(0.014f, 0.013f, 0.012f),
            0.13f,
            421);
        TextureSet floorTextures = GenerateTextureSet(
            "RTv04_WetAgedFlagstoneFloor",
            TextureProfile.Floor,
            new Color(0.082f, 0.075f, 0.064f),
            new Color(0.015f, 0.014f, 0.013f),
            0.58f,
            443);
        TextureSet ceilingTextures = GenerateTextureSet(
            "RTv04_SootedBrickCeiling",
            TextureProfile.Ceiling,
            new Color(0.055f, 0.047f, 0.038f),
            new Color(0.012f, 0.011f, 0.01f),
            0.08f,
            467);
        ValidateBaseAlbedoFiles(wallTextures.Albedo, floorTextures.Albedo, ceilingTextures.Albedo);
        ValidateAssociatedMapFiles(wallTextures, floorTextures, ceilingTextures);

        Material wallBase = CreatePbrMaterial("RTv04_MAT_DarkIrregularBrickWall_Base", wallTextures, new Vector2(1.05f, 1f), 0.13f, 0.018f, 0.62f);
        Material floorBase = CreatePbrMaterial("RTv04_MAT_WetAgedFlagstoneFloor_Base", floorTextures, new Vector2(0.78f, 0.82f), 0.6f, 0.028f, 0.58f);
        Material ceilingBase = CreatePbrMaterial("RTv04_MAT_SootedBrickCeiling_Base", ceilingTextures, new Vector2(1f, 0.95f), 0.08f, 0.014f, 0.46f);
        Material[] wallVariants = CreateMaterialVariants("RTv04_MAT_WallBrick", wallBase, new[]
        {
            new Color(0.64f, 0.61f, 0.55f),
            new Color(0.72f, 0.66f, 0.56f),
            new Color(0.55f, 0.54f, 0.51f),
            new Color(0.68f, 0.6f, 0.5f),
            new Color(0.5f, 0.48f, 0.45f)
        });
        Material[] floorVariants = CreateMaterialVariants("RTv04_MAT_WetFloorStone", floorBase, new[]
        {
            new Color(0.62f, 0.63f, 0.61f),
            new Color(0.75f, 0.7f, 0.62f),
            new Color(0.54f, 0.57f, 0.58f),
            new Color(0.66f, 0.61f, 0.54f)
        });
        Material[] ceilingVariants = CreateMaterialVariants("RTv04_MAT_CeilingBrick", ceilingBase, new[]
        {
            new Color(0.48f, 0.45f, 0.4f),
            new Color(0.57f, 0.5f, 0.42f),
            new Color(0.39f, 0.38f, 0.36f)
        });

        Material mortarMaterial = CreateSimpleMaterial("RTv04_MAT_NarrowDarkMortar", new Color(0.009f, 0.008f, 0.007f), 0f, 0.035f);
        Material grimeMaterial = CreateSimpleMaterial("RTv04_MAT_SoftCornerGrime", new Color(0.006f, 0.0055f, 0.005f), 0f, 0.015f);
        Material lampGlassMaterial = CreateEmissiveMaterial("RTv04_MAT_SoftAmberGaslightGlass", new Color(1f, 0.57f, 0.26f), 4.6f);
        Material brassMaterial = CreateSimpleMaterial("RTv04_MAT_TarnishedBrassLampFrame", new Color(0.35f, 0.23f, 0.11f), 0.16f, 0.32f);
        Material ironMaterial = CreateSimpleMaterial("RTv04_MAT_BlackIronLampBack", new Color(0.016f, 0.014f, 0.012f), 0.1f, 0.2f);

        BuildMaterialPreviewV04(wallVariants, floorVariants, ceilingVariants, mortarMaterial, lampGlassMaterial);
        string previewPath = RenderSceneToFile(PreviewRenderFileNameV04);

        BuildRefinedChamberV04(wallVariants, floorVariants, ceilingVariants, mortarMaterial, grimeMaterial, lampGlassMaterial, brassMaterial, ironMaterial);
        string roomPath = RenderSceneToFile(RoomRenderFileNameV04);
        WriteMetricsV04(roomPath);
        WriteReviewV04(previewPath, roomPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ROOMTEST_REFINED_LOOKDEV_V04_COMPLETE " + roomPath);
    }

    [MenuItem("Roomtest/Build And Render Material Driven Lookdev v0.5")]
    public static void BuildAndRenderMaterialDrivenLookdevV05()
    {
        EnsureFolders();
        ConfigureProject();
        WriteTargetAnalysisV05();

        TextureSet wallTextures = GenerateTextureSet(
            "RTv05_DarkWetBrickWall",
            TextureProfile.Wall,
            new Color(0.057f, 0.049f, 0.038f),
            new Color(0.012f, 0.011f, 0.01f),
            0.18f,
            521);
        TextureSet floorTextures = GenerateTextureSet(
            "RTv05_WetUnevenFlagstoneFloor",
            TextureProfile.Floor,
            new Color(0.069f, 0.061f, 0.051f),
            new Color(0.012f, 0.011f, 0.01f),
            0.62f,
            547);
        TextureSet ceilingTextures = GenerateTextureSet(
            "RTv05_DarkSootBrickCeiling",
            TextureProfile.Ceiling,
            new Color(0.054f, 0.045f, 0.036f),
            new Color(0.01f, 0.009f, 0.008f),
            0.1f,
            563);
        ValidateBaseAlbedoFiles(wallTextures.Albedo, floorTextures.Albedo, ceilingTextures.Albedo);
        ValidateAssociatedMapFiles(wallTextures, floorTextures, ceilingTextures);

        Material wallMaterial = CreatePbrMaterial("RTv05_MAT_DarkWetBrickWall", wallTextures, new Vector2(3.45f, 2.75f), 0.14f, 0.04f, 1.15f);
        Material floorMaterial = CreatePbrMaterial("RTv05_MAT_WetUnevenFlagstoneFloor", floorTextures, new Vector2(1.95f, 2.0f), 0.68f, 0.05f, 1.0f);
        Material ceilingMaterial = CreatePbrMaterial("RTv05_MAT_DarkSootBrickCeiling", ceilingTextures, new Vector2(3.35f, 2.32f), 0.08f, 0.032f, 0.82f);
        Material mortarEdgeMaterial = CreateSimpleMaterial("RTv05_MAT_DeepCornerSoot", new Color(0.006f, 0.0055f, 0.005f), 0f, 0.015f);
        Material lampGlassMaterial = CreateEmissiveMaterial("RTv05_MAT_WarmGaslightGlass", new Color(1f, 0.58f, 0.29f), 3.6f);
        Material brassMaterial = CreateSimpleMaterial("RTv05_MAT_AgedBrassLampFrame", new Color(0.33f, 0.22f, 0.11f), 0.16f, 0.34f);
        Material ironMaterial = CreateSimpleMaterial("RTv05_MAT_DarkIronLampBack", new Color(0.014f, 0.012f, 0.011f), 0.1f, 0.2f);

        BuildMaterialDrivenPreviewV05(wallMaterial, floorMaterial, ceilingMaterial, lampGlassMaterial);
        string previewPath = RenderSceneToFile(PreviewRenderFileNameV05);

        BuildMaterialDrivenChamberV05(wallMaterial, floorMaterial, ceilingMaterial, mortarEdgeMaterial, lampGlassMaterial, brassMaterial, ironMaterial);
        string roomPath = RenderSceneToFile(RoomRenderFileNameV05);
        WriteMetricsGeneric(roomPath, RoomRenderFileNameV05, MetricsFileNameV05, "material-driven continuous masonry room");
        WriteReviewV05(previewPath, roomPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ROOMTEST_MATERIAL_DRIVEN_LOOKDEV_V05_COMPLETE " + roomPath);
    }

    [MenuItem("Roomtest/Build And Render Reference Match Lookdev v0.6")]
    public static void BuildAndRenderReferenceMatchLookdevV06()
    {
        EnsureFolders();
        ConfigureProject();
        WriteTargetAnalysisV06();

        TextureSet wallTextures = GenerateTextureSet(
            "RTv06_DarkIrregularMasonryWall",
            TextureProfile.Wall,
            new Color(0.083f, 0.069f, 0.052f),
            new Color(0.009f, 0.008f, 0.007f),
            0.17f,
            631);
        TextureSet floorTextures = GenerateTextureSet(
            "RTv06_WetWarmFlagstoneFloor",
            TextureProfile.Floor,
            new Color(0.112f, 0.094f, 0.074f),
            new Color(0.01f, 0.009f, 0.008f),
            0.54f,
            647);
        TextureSet ceilingTextures = GenerateTextureSet(
            "RTv06_SootedSmallBrickCeiling",
            TextureProfile.Ceiling,
            new Color(0.062f, 0.052f, 0.041f),
            new Color(0.008f, 0.007f, 0.006f),
            0.1f,
            661);
        ValidateBaseAlbedoFiles(wallTextures.Albedo, floorTextures.Albedo, ceilingTextures.Albedo);
        ValidateAssociatedMapFiles(wallTextures, floorTextures, ceilingTextures);

        Material wallBase = CreatePbrMaterial("RTv06_MAT_DarkIrregularMasonryWall", wallTextures, new Vector2(2.85f, 2.35f), 0.16f, 0.04f, 1.1f);
        Material floorBase = CreatePbrMaterial("RTv06_MAT_WetWarmFlagstoneFloor", floorTextures, new Vector2(1.22f, 1.28f), 0.5f, 0.022f, 0.52f);
        Material ceilingBase = CreatePbrMaterial("RTv06_MAT_SootedSmallBrickCeiling", ceilingTextures, new Vector2(3.1f, 2.18f), 0.08f, 0.03f, 0.85f);
        Material[] wallMaterials = CreateMaterialVariants("RTv06_MAT_WallFace", wallBase, new[]
        {
            new Color(0.82f, 0.76f, 0.66f),
            new Color(0.96f, 0.88f, 0.74f),
            new Color(0.67f, 0.63f, 0.56f),
            new Color(1.08f, 0.93f, 0.76f),
            new Color(0.74f, 0.68f, 0.6f),
            new Color(0.9f, 0.78f, 0.62f)
        });
        Material[] floorMaterials = CreateMaterialVariants("RTv06_MAT_FloorFace", floorBase, new[]
        {
            new Color(0.86f, 0.78f, 0.67f),
            new Color(0.98f, 0.86f, 0.69f),
            new Color(0.72f, 0.68f, 0.6f),
            new Color(0.9f, 0.78f, 0.64f),
            new Color(0.8f, 0.72f, 0.61f)
        });
        Material[] ceilingMaterials = CreateMaterialVariants("RTv06_MAT_CeilingFace", ceilingBase, new[]
        {
            new Color(0.72f, 0.68f, 0.61f),
            new Color(0.86f, 0.78f, 0.65f),
            new Color(0.56f, 0.55f, 0.52f),
            new Color(0.68f, 0.6f, 0.5f)
        });
        Material mortarMaterial = CreateSimpleMaterial("RTv06_MAT_DeepWarmBlackMortar", new Color(0.008f, 0.0068f, 0.0055f), 0f, 0.06f);
        Material grimeMaterial = CreateSimpleMaterial("RTv06_MAT_LocalSootAndDamp", new Color(0.004f, 0.0036f, 0.003f), 0f, 0.03f);
        Material lampGlassMaterial = CreateEmissiveMaterial("RTv06_MAT_HotAmberLampGlass", new Color(1f, 0.62f, 0.31f), 5.2f);
        Material brassMaterial = CreateSimpleMaterial("RTv06_MAT_DulledBrassLampFrame", new Color(0.43f, 0.29f, 0.13f), 0.18f, 0.42f);
        Material ironMaterial = CreateSimpleMaterial("RTv06_MAT_BlackenedIronLampFrame", new Color(0.013f, 0.011f, 0.009f), 0.12f, 0.26f);

        BuildReferenceMatchPreviewV06(wallMaterials, floorMaterials, ceilingMaterials, mortarMaterial, lampGlassMaterial);
        string previewPath = RenderSceneToFile(PreviewRenderFileNameV06);

        BuildReferenceMatchChamberV06(wallMaterials, floorMaterials, ceilingMaterials, mortarMaterial, grimeMaterial, lampGlassMaterial, brassMaterial, ironMaterial);
        string roomPath = RenderSceneToFile(RoomRenderFileNameV06);
        WriteMetricsGeneric(roomPath, RoomRenderFileNameV06, MetricsFileNameV06, "reference-match dark warm wet masonry room");
        WriteReviewV06(previewPath, roomPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ROOMTEST_REFERENCE_MATCH_LOOKDEV_V06_COMPLETE " + roomPath);
    }

    private static void EnsureFolders()
    {
        CreateAssetFolder("Assets", "RoomTest");
        CreateAssetFolder(AssetRoot, "Textures");
        CreateAssetFolder(AssetRoot, "Materials");
        CreateAssetFolder(AssetRoot, "Scenes");
        Directory.CreateDirectory(ProjectPath("Renders"));
        Directory.CreateDirectory(ProjectPath("Documentation"));
    }

    private static void CreateAssetFolder(string parent, string child)
    {
        string path = parent + "/" + child;
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder(parent, child);
        }
    }

    private static void ConfigureProject()
    {
        PlayerSettings.colorSpace = ColorSpace.Linear;
        QualitySettings.antiAliasing = 8;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.shadows = ShadowQuality.All;
        QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
        QualitySettings.shadowDistance = 28f;
        QualitySettings.realtimeReflectionProbes = true;
    }

    private static void WriteTargetAnalysis()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.3 Target Analysis");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## Reference Targets");
        builder.AppendLine();
        builder.AppendLine("- Room shape: simple enclosed rectangular masonry chamber, wide first-person perspective, visible floor, ceiling, side walls, and back wall.");
        builder.AppendLine("- Wall brick: small dark brown-black aged bricks with recessed mortar, chipped irregular edges, soot, grime, and non-uniform color.");
        builder.AppendLine("- Floor stone: larger flagstones than the walls, darker wet surface, warm reflected lamp glints, no metallic orange material response.");
        builder.AppendLine("- Ceiling brick: small sooted brick courses, darker and less glossy than floor, visible perspective lines into the back of the room.");
        builder.AppendLine("- Lighting: two warm amber wall lamps with localized halos; the full room should stay dark rather than turning orange.");
        builder.AppendLine("- Depth: corners and the back wall should hold darkness, with readable but subdued surface relief.");
        builder.AppendLine();
        builder.AppendLine("## v0.3 Method");
        builder.AppendLine();
        builder.AppendLine("1. Generate and check base albedo PNGs before material creation.");
        builder.AppendLine("2. Generate and check normal, height, occlusion, and packed metallic/smoothness map PNGs.");
        builder.AppendLine("3. Build Unity Standard materials from those maps.");
        builder.AppendLine("4. Build an isolated material preview and a full room scene using real brick/slab geometry plus the maps.");
        builder.AppendLine("5. Render PNG evidence and record an acceptance note against the reference.");
        File.WriteAllText(ProjectPath("Documentation/" + TargetAnalysisFileName), builder.ToString());
    }

    private static void WriteTargetAnalysisV04()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.4 Target Analysis");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## v0.3 Problems To Correct");
        builder.AppendLine();
        builder.AppendLine("- Brick and slab geometry became too chunky, with wide black gaps instead of recessed mortar.");
        builder.AppendLine("- Ceiling read as large panels rather than small sooted masonry.");
        builder.AppendLine("- Side walls still picked up too much orange near the lamps.");
        builder.AppendLine("- Back wall was too dark to judge texture relief.");
        builder.AppendLine();
        builder.AppendLine("## v0.4 Targets");
        builder.AppendLine();
        builder.AppendLine("- Keep real Unity geometry, but make it shallow and tightly spaced so it reads like masonry, not floating blocks.");
        builder.AppendLine("- Push albedo darker and more neutral, with amber limited to lamp halos and wet floor reflections.");
        builder.AppendLine("- Keep the floor larger-scale than wall and ceiling brick.");
        builder.AppendLine("- Add enough low neutral fill to reveal the back wall while preserving corner darkness.");
        builder.AppendLine("- Maintain the same evidence gate order: analysis, base PNGs, map PNGs, material, scene, render, acceptance note.");
        File.WriteAllText(ProjectPath("Documentation/" + TargetAnalysisFileNameV04), builder.ToString());
    }

    private static void WriteTargetAnalysisV05()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.5 Target Analysis");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## v0.4 Problems To Correct");
        builder.AppendLine();
        builder.AppendLine("- Real brick cubes still read as construction blocks instead of aged masonry.");
        builder.AppendLine("- Side walls remained too orange near the lamps.");
        builder.AppendLine("- Ceiling geometry still looked panelized.");
        builder.AppendLine();
        builder.AppendLine("## v0.5 Target");
        builder.AppendLine();
        builder.AppendLine("- Use continuous room surfaces with procedural albedo/normal/height/occlusion/smoothness maps as the primary detail carrier.");
        builder.AppendLine("- Keep only subtle edge/corner grime geometry, avoiding chunky per-brick blocks.");
        builder.AppendLine("- Make wall/ceiling brick scale smaller than floor slabs through material tiling.");
        builder.AppendLine("- Use warm lamps for local halos and damp floor glints while preserving dark corners and a readable back wall.");
        builder.AppendLine("- Follow the same evidence order: analysis, base PNGs, associated map PNGs, material build, isolated scene, render, acceptance note.");
        File.WriteAllText(ProjectPath("Documentation/" + TargetAnalysisFileNameV05), builder.ToString());
    }

    private static void WriteTargetAnalysisV06()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.6 Target Analysis");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## v0.5 Problems To Correct");
        builder.AppendLine();
        builder.AppendLine("- The floor lighting read too blue/cold compared with the warm brown-black reference.");
        builder.AppendLine("- Brick and slab courses were still too regular and too clean.");
        builder.AppendLine("- Lamp fixtures were simplified and the wall glow still lacked the reference's warm localized bloom shape.");
        builder.AppendLine("- The room lacked enough surface-layered grime, chipped edges, and warm wet floor response.");
        builder.AppendLine();
        builder.AppendLine("## v0.6 Target");
        builder.AppendLine();
        builder.AppendLine("- Keep continuous material maps, but add very shallow individual brick/stone face meshes over dark mortar so relief reads without chunky blockout geometry.");
        builder.AppendLine("- Use warmer neutral fill and amber lamps instead of cool blue readability light.");
        builder.AppendLine("- Increase small-scale masonry irregularity, narrower mortar, chipped face corners, and soot/damp bands.");
        builder.AppendLine("- Make the floor larger-scale, wet, reflective, and warm-black rather than blue or metallic.");
        builder.AppendLine("- Preserve the staged evidence gate: reference analysis, generated base PNGs, map PNGs, material build, isolated scene, render, acceptance note.");
        File.WriteAllText(ProjectPath("Documentation/" + TargetAnalysisFileNameV06), builder.ToString());
    }

    private static void ValidateBaseAlbedoFiles(params string[] albedoAssetPaths)
    {
        foreach (string assetPath in albedoAssetPaths)
        {
            RequireGeneratedTexture(assetPath, "base albedo PNG");
        }

        Debug.Log("ROOMTEST_V03_BASE_TEXTURE_PNGS_CHECKED " + albedoAssetPaths.Length.ToString(CultureInfo.InvariantCulture));
    }

    private static void ValidateAssociatedMapFiles(params TextureSet[] textureSets)
    {
        int checkedCount = 0;
        foreach (TextureSet set in textureSets)
        {
            RequireGeneratedTexture(set.Normal, "normal map PNG");
            RequireGeneratedTexture(set.Height, "height map PNG");
            RequireGeneratedTexture(set.Occlusion, "occlusion map PNG");
            RequireGeneratedTexture(set.MetallicSmoothness, "packed metallic/smoothness map PNG");
            checkedCount += 4;
        }

        Debug.Log("ROOMTEST_V03_ASSOCIATED_MAP_PNGS_CHECKED " + checkedCount.ToString(CultureInfo.InvariantCulture));
    }

    private static void RequireGeneratedTexture(string assetPath, string label)
    {
        string fullPath = ProjectPath(assetPath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Missing roomtest " + label, fullPath);
        }

        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        if (texture == null)
        {
            throw new InvalidOperationException("Roomtest " + label + " did not import as a Texture2D: " + assetPath);
        }
    }

    private static TextureSet GenerateTextureSet(string name, TextureProfile profile, Color stoneBase, Color mortarBase, float smoothnessBase, int seed)
    {
        bool finalPass = seed >= 500;
        float[] height = new float[TextureSize * TextureSize];
        Color32[] albedoPixels = new Color32[height.Length];
        Color32[] heightPixels = new Color32[height.Length];
        Color32[] normalPixels;
        Color32[] aoPixels = new Color32[height.Length];
        Color32[] metallicSmoothPixels = new Color32[height.Length];

        for (int y = 0; y < TextureSize; y++)
        {
            float v = (y + 0.5f) / TextureSize;
            for (int x = 0; x < TextureSize; x++)
            {
                float u = (x + 0.5f) / TextureSize;
                PatternSample sample = SamplePattern(profile, u, v, seed);
                float fine = Fbm(u * (finalPass ? 155f : 120f) + 5.1f, v * (finalPass ? 146f : 112f) + 2.3f, seed + 17, 5);
                float grit = Fbm(u * (finalPass ? 430f : 360f) + 1.9f, v * (finalPass ? 390f : 310f) + 8.1f, seed + 31, 3);
                float broad = Fbm(u * 6.4f + 0.7f, v * 8.8f + 1.4f, seed + 47, 5);
                float verticalSoot = Fbm(u * 3.2f + 7.2f, v * 11.5f + 3.3f, seed + 61, 4);
                float crack = Fbm(u * 215f + 2.5f, v * 176f + 9.2f, seed + 89, 2);
                float edge = Mathf.Clamp01(1f - sample.edgeDistance * (profile == TextureProfile.Floor ? 4.6f : 6.4f));
                float chip = Mathf.Clamp01((fine - (finalPass ? 0.57f : 0.62f)) * (finalPass ? 3.7f : 3.2f)) * edge * Mathf.Lerp(0.45f, finalPass ? 1.55f : 1.25f, sample.irregularity);
                float pitting = Mathf.Clamp01((grit - 0.58f) * 2.4f);
                float hairline = crack > 0.73f && sample.edgeDistance > 0.15f ? Mathf.Lerp(0.06f, 0.18f, grit) : 0f;

                float h = sample.isMortar
                    ? (finalPass ? 0.018f + broad * 0.02f : 0.035f + broad * 0.03f)
                    : 0.48f + broad * 0.16f + fine * (finalPass ? 0.075f : 0.055f) - edge * (finalPass ? 0.21f : 0.16f) - chip * (finalPass ? 0.28f : 0.22f) - hairline;
                if (profile == TextureProfile.Floor)
                {
                    h += 0.035f * Fbm(u * 4.5f, v * 5.2f, seed + 103, 3);
                }

                h = Mathf.Clamp01(h);
                int index = y * TextureSize + x;
                height[index] = h;

                Color c = sample.isMortar
                    ? mortarBase * Mathf.Lerp(0.55f, 1.08f, broad)
                    : stoneBase * Mathf.Lerp(0.58f, 1.22f, sample.tone * 0.5f + broad * 0.5f);
                c += new Color(0.025f, 0.018f, 0.01f) * Mathf.Lerp(0.05f, 0.8f, fine);
                c += new Color(0.012f, 0.013f, 0.014f) * Mathf.Lerp(0f, 0.65f, grit);
                c *= Mathf.Lerp(finalPass ? 0.43f : 0.52f, 1f, 1f - sample.stain);
                c *= Mathf.Lerp(finalPass ? 0.47f : 0.55f, 1f, 1f - verticalSoot * (profile == TextureProfile.Ceiling ? 0.95f : 0.48f));
                c *= 1f - chip * (finalPass ? 0.58f : 0.45f) - hairline * 1.9f - pitting * (finalPass ? 0.15f : 0.1f);
                albedoPixels[index] = ToColor32(c, 1f);

                byte hByte = (byte)Mathf.RoundToInt(h * 255f);
                heightPixels[index] = new Color32(hByte, hByte, hByte, 255);

                float ao = sample.isMortar ? Mathf.Lerp(finalPass ? 0.08f : 0.12f, finalPass ? 0.24f : 0.32f, broad) : Mathf.Lerp(0.5f, 0.94f, Mathf.Clamp01(sample.edgeDistance * 3.3f));
                ao -= chip * 0.25f + hairline * 0.8f;
                byte aoByte = (byte)Mathf.RoundToInt(Mathf.Clamp01(ao) * 255f);
                aoPixels[index] = new Color32(aoByte, aoByte, aoByte, 255);

                float wetPatch = profile == TextureProfile.Floor ? Mathf.SmoothStep(0.5f, 0.9f, Fbm(u * 5.2f, v * 4.7f, seed + 127, 4)) : 0f;
                float smoothness = sample.isMortar
                    ? smoothnessBase * 0.18f
                    : smoothnessBase + wetPatch * 0.26f + fine * 0.03f - chip * 0.08f;
                if (profile != TextureProfile.Floor)
                {
                    smoothness *= Mathf.Lerp(0.68f, 1f, 1f - edge);
                }

                smoothness = Mathf.Clamp01(smoothness);
                metallicSmoothPixels[index] = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(smoothness * 255f));
            }
        }

        float normalStrength = profile == TextureProfile.Floor
            ? (finalPass ? 6.25f : 5.4f)
            : profile == TextureProfile.Wall
                ? (finalPass ? 5.35f : 4.3f)
                : (finalPass ? 4.5f : 3.8f);
        normalPixels = GenerateNormalPixels(height, normalStrength);
        string albedo = SaveTexture(name + "_Albedo", albedoPixels, TextureImporterType.Default, true);
        string normal = SaveTexture(name + "_Normal", normalPixels, TextureImporterType.NormalMap, false);
        string heightPath = SaveTexture(name + "_Height", heightPixels, TextureImporterType.Default, false);
        string occlusion = SaveTexture(name + "_Occlusion", aoPixels, TextureImporterType.Default, false);
        string metallicSmooth = SaveTexture(name + "_MetallicSmoothness", metallicSmoothPixels, TextureImporterType.Default, false);
        return new TextureSet(albedo, normal, heightPath, occlusion, metallicSmooth);
    }

    private static PatternSample SamplePattern(TextureProfile profile, float u, float v, int seed)
    {
        bool finalPass = seed >= 500;
        float columns = profile == TextureProfile.Floor ? (finalPass ? 5.35f : 5.8f) : (finalPass ? 16.4f : 14.6f);
        float rows = profile == TextureProfile.Floor ? (finalPass ? 4.85f : 5.1f) : (finalPass ? 11.2f : 9.8f);
        float mortarX = profile == TextureProfile.Floor ? (finalPass ? 0.014f : 0.025f) : (finalPass ? 0.019f : 0.028f);
        float mortarY = profile == TextureProfile.Floor ? (finalPass ? 0.018f : 0.03f) : (finalPass ? 0.022f : 0.034f);
        float rowWarp = (Fbm(u * 2.2f, v * 7.5f, seed + 131, 3) - 0.5f) * (finalPass ? 0.19f : 0.11f);
        float rowFloat = v * rows + rowWarp;
        int row = Mathf.FloorToInt(rowFloat);
        float rowOffset = ((row & 1) == 0 ? 0f : 0.48f / columns) + (Fbm(row * 0.41f, seed * 0.017f, seed + 149, 2) - 0.5f) * (finalPass ? 0.074f : 0.045f);
        float shiftedU = Frac(u + rowOffset + (Fbm(u * 4.4f, v * 3.7f, seed + 151, 3) - 0.5f) * (finalPass ? 0.032f : 0.018f));
        float colFloat = shiftedU * columns + (Fbm(u * 9.2f, v * 6.2f, seed + 163, 2) - 0.5f) * (finalPass ? 0.13f : 0.07f);
        int col = Mathf.FloorToInt(colFloat);
        float bx = Frac(colFloat);
        float by = Frac(rowFloat);
        float jointNoise = Fbm(u * 54f + 4.4f, v * 48f + 2.2f, seed + 167, 3);
        float localMortarX = mortarX * Mathf.Lerp(finalPass ? 0.55f : 0.45f, finalPass ? 1.45f : 2f, jointNoise);
        float localMortarY = mortarY * Mathf.Lerp(finalPass ? 0.55f : 0.5f, finalPass ? 1.42f : 1.85f, Fbm(u * 47f, v * 52f, seed + 173, 3));
        bool mortar = bx < localMortarX || bx > 1f - localMortarX || by < localMortarY || by > 1f - localMortarY;
        float edge = Mathf.Min(Mathf.Min(bx, 1f - bx), Mathf.Min(by, 1f - by));
        float tone = Hash01(col, row, seed);
        float irregularity = Fbm(col * 1.37f + u * 10f, row * 1.77f + v * 8f, seed + 179, 3);
        float stain = Mathf.SmoothStep(0.42f, 0.92f, Fbm(u * 4.1f + 0.9f, v * 9.7f + 1.6f, seed + 181, 5));
        return new PatternSample(mortar, Mathf.Clamp01(edge), tone, irregularity, stain);
    }

    private static void BuildMaterialPreview(Material[] wallMaterials, Material[] floorMaterials, Material[] ceilingMaterials, Material mortarMaterial, Material lampGlassMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings(new Color(0.04f, 0.036f, 0.031f), 0.28f);
        CreateBox("RTv03_Preview_MortarBase", new Vector3(0f, -0.07f, 0f), new Vector3(6.4f, 0.12f, 2.4f), mortarMaterial);
        CreateFloorSlabs("RTv03_Preview_FloorSlabs", floorMaterials, -2.8f, 2.8f, -0.7f, 1.4f, 3, 2, 601);
        CreateBackWallBricks("RTv03_Preview_WallBricks", wallMaterials, -3.1f, 3.1f, 0.25f, 2.25f, 1.65f, 9, 4, 607);
        CreateCeilingBricks("RTv03_Preview_CeilingBricks", ceilingMaterials, -3.1f, 3.1f, -0.8f, 1.3f, 2.55f, 9, 3, 613);

        CreatePointLight("RTv03_Preview_WarmInspectionLamp", new Vector3(-1.2f, 2.6f, -2.4f), new Color(1f, 0.66f, 0.39f), 4.8f, 6f);
        CreateSpotLight("RTv03_Preview_SoftTopInspection", new Vector3(2.4f, 4.3f, -3.2f), new Vector3(0f, 0.75f, 0.3f), new Color(0.9f, 0.86f, 0.78f), 2.1f, 52f, 8f);
        CreateBox("RTv03_Preview_GlassBrightnessPatch", new Vector3(-3.05f, 1.35f, 1.45f), new Vector3(0.25f, 0.25f, 0.04f), lampGlassMaterial);

        CreateCamera("RTv03_PreviewCamera", new Vector3(0f, 1.35f, -4.8f), new Vector3(0f, 1.05f, 0.35f), 45f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), PreviewScenePath);
    }

    private static void BuildBrickChamber(
        Material[] wallMaterials,
        Material[] floorMaterials,
        Material[] ceilingMaterials,
        Material mortarMaterial,
        Material grimeMaterial,
        Material lampGlassMaterial,
        Material brassMaterial,
        Material ironMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings(new Color(0.03f, 0.026f, 0.022f), 0.44f);

        CreateBox("RTv03_MortarBase_Floor", new Vector3(0f, -0.055f, -0.25f), new Vector3(12.4f, 0.1f, 13.1f), mortarMaterial);
        CreateBox("RTv03_MortarBase_BackWall", new Vector3(0f, 2.42f, 5.9f), new Vector3(12.4f, 4.84f, 0.1f), mortarMaterial);
        CreateBox("RTv03_MortarBase_LeftWall", new Vector3(-6.12f, 2.42f, -0.2f), new Vector3(0.1f, 4.84f, 12.3f), mortarMaterial);
        CreateBox("RTv03_MortarBase_RightWall", new Vector3(6.12f, 2.42f, -0.2f), new Vector3(0.1f, 4.84f, 12.3f), mortarMaterial);
        CreateBox("RTv03_MortarBase_Ceiling", new Vector3(0f, 4.88f, -0.2f), new Vector3(12.4f, 0.1f, 12.3f), mortarMaterial);

        CreateFloorSlabs("RTv03_Floor_IndividualWetFlagstones", floorMaterials, -5.95f, 5.95f, -6.15f, 5.55f, 8, 9, 701);
        CreateBackWallBricks("RTv03_BackWall_ChippedIndividualBricks", wallMaterials, -5.95f, 5.95f, 0.22f, 4.65f, 5.82f, 21, 12, 719);
        CreateSideWallBricks("RTv03_LeftWall_ChippedIndividualBricks", wallMaterials, -6.04f, -6.05f, 5.45f, 0.22f, 4.65f, 20, 12, true, 733);
        CreateSideWallBricks("RTv03_RightWall_ChippedIndividualBricks", wallMaterials, 6.04f, -6.05f, 5.45f, 0.22f, 4.65f, 20, 12, false, 751);
        CreateCeilingBricks("RTv03_Ceiling_IndividualSootedBricks", ceilingMaterials, -5.95f, 5.95f, -6.05f, 5.55f, 4.78f, 21, 10, 769);

        CreateCornerAndBaseGrime(grimeMaterial);
        CreateWallLamp("RTv03_Left_Gaslight", new Vector3(-6.0f, 2.42f, -0.65f), Quaternion.Euler(0f, 90f, 0f), lampGlassMaterial, brassMaterial, ironMaterial, false);
        CreateWallLamp("RTv03_Right_Gaslight", new Vector3(6.0f, 2.42f, -0.65f), Quaternion.Euler(0f, -90f, 0f), lampGlassMaterial, brassMaterial, ironMaterial, true);

        CreatePointLight("RTv03_Left_LocalAmberLampHalo", new Vector3(-5.55f, 2.42f, -0.58f), new Color(1f, 0.59f, 0.31f), 5.7f, 4.8f);
        CreatePointLight("RTv03_Right_LocalAmberLampHalo", new Vector3(5.55f, 2.42f, -0.58f), new Color(1f, 0.59f, 0.31f), 5.7f, 4.8f);
        CreateSpotLight("RTv03_Left_WetStoneReflectionBeam", new Vector3(-5.55f, 2.12f, -0.48f), new Vector3(-2.5f, 0.08f, -3.1f), new Color(1f, 0.55f, 0.27f), 3.4f, 58f, 7.6f);
        CreateSpotLight("RTv03_Right_WetStoneReflectionBeam", new Vector3(5.55f, 2.12f, -0.48f), new Vector3(2.5f, 0.08f, -3.1f), new Color(1f, 0.55f, 0.27f), 3.4f, 58f, 7.6f);
        CreateSpotLight("RTv03_BackWall_LowReadabilityReturn", new Vector3(0f, 3.25f, -3.3f), new Vector3(0f, 2.12f, 5.7f), new Color(0.82f, 0.78f, 0.7f), 0.9f, 70f, 12f);
        CreateSpotLight("RTv03_Ceiling_GrazingEdgeLight", new Vector3(0f, 4.25f, -3.9f), new Vector3(0f, 3.55f, 2.0f), new Color(1f, 0.58f, 0.32f), 0.58f, 64f, 9f);

        GameObject probeObject = new GameObject("RTv03_ChamberReflectionProbe");
        probeObject.transform.position = new Vector3(0f, 1.6f, -1.1f);
        ReflectionProbe probe = probeObject.AddComponent<ReflectionProbe>();
        probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
        probe.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        probe.size = new Vector3(12.1f, 4.8f, 12f);
        probe.intensity = 0.8f;
        probe.resolution = 512;

        CreateCamera("RTv03_RenderCamera", new Vector3(0f, 1.7f, -6.55f), new Vector3(0f, 2.0f, 4.9f), 68f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), RoomScenePath);
    }

    private static void BuildMaterialPreviewV04(Material[] wallMaterials, Material[] floorMaterials, Material[] ceilingMaterials, Material mortarMaterial, Material lampGlassMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings(new Color(0.045f, 0.041f, 0.036f), 0.34f);
        CreateBox("RTv04_Preview_DarkMortarBacking", new Vector3(0f, 1.15f, 1.08f), new Vector3(5.9f, 2.25f, 0.08f), mortarMaterial);
        CreateBackWallBricksTight("RTv04_Preview_TightWallBrick", wallMaterials, -2.9f, 2.9f, 0.24f, 2.25f, 1.0f, 13, 6, 821);
        CreateFloorSlabsTight("RTv04_Preview_TightWetFloor", floorMaterials, -2.9f, 2.9f, -1.0f, 1.35f, 4, 3, 823);
        CreateCeilingBricksTight("RTv04_Preview_TightCeilingBrick", ceilingMaterials, -2.9f, 2.9f, -1.0f, 1.35f, 2.8f, 13, 4, 827);
        CreateBox("RTv04_Preview_SmallAmberLampPatch", new Vector3(-2.8f, 1.35f, 0.88f), new Vector3(0.18f, 0.3f, 0.035f), lampGlassMaterial);
        CreatePointLight("RTv04_Preview_LocalAmber", new Vector3(-2.55f, 1.5f, -0.15f), new Color(1f, 0.58f, 0.3f), 2.6f, 3.2f);
        CreateSpotLight("RTv04_Preview_NeutralReadLight", new Vector3(1.8f, 3.7f, -3.0f), new Vector3(0f, 1.05f, 0.75f), new Color(0.86f, 0.82f, 0.74f), 2.7f, 48f, 7.5f);
        CreateCamera("RTv04_PreviewCamera", new Vector3(0f, 1.35f, -4.6f), new Vector3(0f, 1.05f, 0.55f), 45f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), PreviewScenePathV04);
    }

    private static void BuildRefinedChamberV04(
        Material[] wallMaterials,
        Material[] floorMaterials,
        Material[] ceilingMaterials,
        Material mortarMaterial,
        Material grimeMaterial,
        Material lampGlassMaterial,
        Material brassMaterial,
        Material ironMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings(new Color(0.026f, 0.023f, 0.02f), 0.52f);

        CreateBox("RTv04_Mortar_FloorPlane", new Vector3(0f, -0.035f, -0.25f), new Vector3(12.35f, 0.07f, 13.05f), mortarMaterial);
        CreateBox("RTv04_Mortar_BackWallPlane", new Vector3(0f, 2.43f, 5.86f), new Vector3(12.35f, 4.86f, 0.08f), mortarMaterial);
        CreateBox("RTv04_Mortar_LeftWallPlane", new Vector3(-6.08f, 2.43f, -0.2f), new Vector3(0.08f, 4.86f, 12.25f), mortarMaterial);
        CreateBox("RTv04_Mortar_RightWallPlane", new Vector3(6.08f, 2.43f, -0.2f), new Vector3(0.08f, 4.86f, 12.25f), mortarMaterial);
        CreateBox("RTv04_Mortar_CeilingPlane", new Vector3(0f, 4.84f, -0.2f), new Vector3(12.35f, 0.08f, 12.25f), mortarMaterial);

        CreateFloorSlabsTight("RTv04_Floor_TightWetFlagstones", floorMaterials, -5.95f, 5.95f, -6.15f, 5.55f, 9, 10, 831);
        CreateBackWallBricksTight("RTv04_BackWall_TightDarkBrick", wallMaterials, -5.95f, 5.95f, 0.18f, 4.66f, 5.79f, 25, 14, 839);
        CreateSideWallBricksTight("RTv04_LeftWall_TightDarkBrick", wallMaterials, -6.0f, -6.05f, 5.45f, 0.18f, 4.66f, 25, 14, true, 853);
        CreateSideWallBricksTight("RTv04_RightWall_TightDarkBrick", wallMaterials, 6.0f, -6.05f, 5.45f, 0.18f, 4.66f, 25, 14, false, 857);
        CreateCeilingBricksTight("RTv04_Ceiling_TightSootedBrick", ceilingMaterials, -5.95f, 5.95f, -6.05f, 5.55f, 4.77f, 25, 12, 859);
        CreateCornerAndBaseGrimeV04(grimeMaterial);

        CreateWallLamp("RTv04_Left_Gaslight", new Vector3(-5.98f, 2.42f, -0.62f), Quaternion.Euler(0f, 90f, 0f), lampGlassMaterial, brassMaterial, ironMaterial, false);
        CreateWallLamp("RTv04_Right_Gaslight", new Vector3(5.98f, 2.42f, -0.62f), Quaternion.Euler(0f, -90f, 0f), lampGlassMaterial, brassMaterial, ironMaterial, true);
        CreatePointLight("RTv04_Left_TightAmberHalo", new Vector3(-5.55f, 2.42f, -0.55f), new Color(1f, 0.58f, 0.3f), 4.05f, 3.75f);
        CreatePointLight("RTv04_Right_TightAmberHalo", new Vector3(5.55f, 2.42f, -0.55f), new Color(1f, 0.58f, 0.3f), 4.05f, 3.75f);
        CreateSpotLight("RTv04_Left_WetReflection", new Vector3(-5.45f, 2.05f, -0.55f), new Vector3(-2.4f, 0.06f, -3.25f), new Color(1f, 0.54f, 0.25f), 2.45f, 54f, 6.9f);
        CreateSpotLight("RTv04_Right_WetReflection", new Vector3(5.45f, 2.05f, -0.55f), new Vector3(2.4f, 0.06f, -3.25f), new Color(1f, 0.54f, 0.25f), 2.45f, 54f, 6.9f);
        CreateSpotLight("RTv04_BackWall_NeutralReadability", new Vector3(0f, 3.0f, -3.8f), new Vector3(0f, 2.05f, 5.7f), new Color(0.72f, 0.75f, 0.78f), 1.18f, 68f, 12.2f);
        CreateSpotLight("RTv04_Ceiling_GrazingWarm", new Vector3(0f, 4.25f, -3.9f), new Vector3(0f, 3.52f, 2.2f), new Color(1f, 0.56f, 0.3f), 0.34f, 60f, 8.8f);

        GameObject probeObject = new GameObject("RTv04_ReflectionProbe");
        probeObject.transform.position = new Vector3(0f, 1.55f, -1.0f);
        ReflectionProbe probe = probeObject.AddComponent<ReflectionProbe>();
        probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
        probe.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        probe.size = new Vector3(12.1f, 4.75f, 12f);
        probe.intensity = 0.7f;
        probe.resolution = 512;

        CreateCamera("RTv04_RenderCamera", new Vector3(0f, 1.68f, -6.55f), new Vector3(0f, 2.0f, 4.88f), 68f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), RoomScenePathV04);
    }

    private static void BuildMaterialDrivenPreviewV05(Material wallMaterial, Material floorMaterial, Material ceilingMaterial, Material lampGlassMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings(new Color(0.05f, 0.046f, 0.04f), 0.42f);
        CreateBox("RTv05_Preview_WallMaterialPanel", new Vector3(-2.5f, 1.35f, 0.8f), new Vector3(1.9f, 1.8f, 0.1f), wallMaterial);
        CreateBox("RTv05_Preview_FloorMaterialWetSlab", new Vector3(0f, 0.03f, 0.25f), new Vector3(2.25f, 0.06f, 2.2f), floorMaterial);
        CreateBox("RTv05_Preview_CeilingMaterialPanel", new Vector3(2.5f, 1.35f, 0.8f), new Vector3(1.9f, 1.8f, 0.1f), ceilingMaterial);
        CreateBox("RTv05_Preview_AmberLampReference", new Vector3(-3.25f, 1.42f, 0.7f), new Vector3(0.18f, 0.28f, 0.04f), lampGlassMaterial);
        CreatePointLight("RTv05_Preview_LocalWarmLamp", new Vector3(-2.9f, 1.55f, -0.1f), new Color(1f, 0.58f, 0.3f), 2.6f, 3.6f);
        CreateSpotLight("RTv05_Preview_NeutralSurfaceLight", new Vector3(1.4f, 3.2f, -3.2f), new Vector3(0f, 0.75f, 0.45f), new Color(0.8f, 0.82f, 0.8f), 3.0f, 50f, 8f);
        CreateCamera("RTv05_PreviewCamera", new Vector3(0f, 1.28f, -4.7f), new Vector3(0f, 0.95f, 0.42f), 45f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), PreviewScenePathV05);
    }

    private static void BuildMaterialDrivenChamberV05(Material wallMaterial, Material floorMaterial, Material ceilingMaterial, Material grimeMaterial, Material lampGlassMaterial, Material brassMaterial, Material ironMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings(new Color(0.034f, 0.03f, 0.026f), 0.68f);

        CreateBox("RTv05_Floor_ContinuousWetFlagstoneMaterial", new Vector3(0f, -0.045f, -0.25f), new Vector3(12.4f, 0.09f, 13.15f), floorMaterial);
        CreateBox("RTv05_BackWall_ContinuousDarkBrickMaterial", new Vector3(0f, 2.43f, 5.88f), new Vector3(12.4f, 4.86f, 0.1f), wallMaterial);
        CreateBox("RTv05_LeftWall_ContinuousDarkBrickMaterial", new Vector3(-6.12f, 2.43f, -0.2f), new Vector3(0.1f, 4.86f, 12.3f), wallMaterial);
        CreateBox("RTv05_RightWall_ContinuousDarkBrickMaterial", new Vector3(6.12f, 2.43f, -0.2f), new Vector3(0.1f, 4.86f, 12.3f), wallMaterial);
        CreateBox("RTv05_Ceiling_ContinuousSootedBrickMaterial", new Vector3(0f, 4.86f, -0.2f), new Vector3(12.4f, 0.1f, 12.3f), ceilingMaterial);

        CreateBox("RTv05_BackWall_LowerDampGrime", new Vector3(0f, 0.12f, 5.8f), new Vector3(12.0f, 0.2f, 0.035f), grimeMaterial);
        CreateBox("RTv05_LeftCorner_SootDepth", new Vector3(-6.04f, 2.32f, 5.76f), new Vector3(0.04f, 4.4f, 0.055f), grimeMaterial);
        CreateBox("RTv05_RightCorner_SootDepth", new Vector3(6.04f, 2.32f, 5.76f), new Vector3(0.04f, 4.4f, 0.055f), grimeMaterial);
        CreateBox("RTv05_Ceiling_BackSootBand", new Vector3(0f, 4.8f, 4.9f), new Vector3(12.0f, 0.04f, 0.45f), grimeMaterial);
        CreateBox("RTv05_Floor_FrontWetDarkBand", new Vector3(0f, 0.02f, -5.72f), new Vector3(12.0f, 0.035f, 0.18f), grimeMaterial);

        CreateWallLampV05("RTv05_Left_Gaslight", new Vector3(-6.05f, 2.42f, -0.62f), Quaternion.Euler(0f, 90f, 0f), lampGlassMaterial, brassMaterial, ironMaterial, false);
        CreateWallLampV05("RTv05_Right_Gaslight", new Vector3(6.05f, 2.42f, -0.62f), Quaternion.Euler(0f, -90f, 0f), lampGlassMaterial, brassMaterial, ironMaterial, true);
        CreatePointLight("RTv05_Left_TightAmberWallHalo", new Vector3(-5.72f, 2.42f, -0.58f), new Color(1f, 0.58f, 0.31f), 2.18f, 2.45f);
        CreatePointLight("RTv05_Right_TightAmberWallHalo", new Vector3(5.72f, 2.42f, -0.58f), new Color(1f, 0.58f, 0.31f), 2.18f, 2.45f);
        CreateSpotLight("RTv05_Left_WetFloorGleam", new Vector3(-5.55f, 2.12f, -0.5f), new Vector3(-2.6f, 0.03f, -3.45f), new Color(1f, 0.54f, 0.27f), 2.35f, 48f, 7.2f);
        CreateSpotLight("RTv05_Right_WetFloorGleam", new Vector3(5.55f, 2.12f, -0.5f), new Vector3(2.6f, 0.03f, -3.45f), new Color(1f, 0.54f, 0.27f), 2.35f, 48f, 7.2f);
        CreateSpotLight("RTv05_BackWall_NeutralLowReadability", new Vector3(0f, 2.9f, -4.1f), new Vector3(0f, 2.05f, 5.75f), new Color(0.72f, 0.69f, 0.62f), 0.92f, 68f, 12.8f);
        CreateSpotLight("RTv05_Ceiling_SubtleWarmGrazing", new Vector3(0f, 4.3f, -4.0f), new Vector3(0f, 3.55f, 2.3f), new Color(1f, 0.58f, 0.32f), 0.32f, 64f, 9f);

        GameObject probeObject = new GameObject("RTv05_ReflectionProbe");
        probeObject.transform.position = new Vector3(0f, 1.55f, -1.0f);
        ReflectionProbe probe = probeObject.AddComponent<ReflectionProbe>();
        probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
        probe.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        probe.size = new Vector3(12.1f, 4.75f, 12f);
        probe.intensity = 0.85f;
        probe.resolution = 512;

        CreateCamera("RTv05_RenderCamera", new Vector3(0f, 1.68f, -6.55f), new Vector3(0f, 2.0f, 4.88f), 68f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), RoomScenePathV05);
    }

    private static void BuildReferenceMatchPreviewV06(Material[] wallMaterials, Material[] floorMaterials, Material[] ceilingMaterials, Material mortarMaterial, Material lampGlassMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings(new Color(0.045f, 0.036f, 0.028f), 0.52f);

        CreateBox("RTv06_Preview_WallMortarBacking", new Vector3(-2.75f, 1.5f, 0.55f), new Vector3(4.2f, 2.75f, 0.05f), mortarMaterial);
        CreateMasonryFaceGridV06("RTv06_Preview_WallFaces", BrickSurface.BackWall, wallMaterials, -4.7f, -0.8f, 0.25f, 2.7f, 0.515f, 12, 7, 1701, 0.93f, 0.9f);
        CreateBox("RTv06_Preview_FloorMortarBacking", new Vector3(1.4f, -0.025f, 0.25f), new Vector3(4.2f, 0.045f, 2.6f), mortarMaterial);
        CreateMasonryFaceGridV06("RTv06_Preview_FloorFaces", BrickSurface.Floor, floorMaterials, -0.55f, 3.35f, -1.05f, 1.3f, 0.012f, 4, 3, 1733, 0.92f, 0.9f);
        CreateBox("RTv06_Preview_CeilingMortarBacking", new Vector3(3.65f, 2.65f, 0.55f), new Vector3(3.4f, 0.05f, 2.45f), mortarMaterial);
        CreateMasonryFaceGridV06("RTv06_Preview_CeilingFaces", BrickSurface.Ceiling, ceilingMaterials, 2.1f, 5.2f, -0.6f, 1.55f, 2.62f, 11, 5, 1747, 0.92f, 0.88f);

        CreateBox("RTv06_Preview_LampGlowChip", new Vector3(-4.1f, 1.65f, 0.48f), new Vector3(0.18f, 0.36f, 0.035f), lampGlassMaterial);
        CreatePointLight("RTv06_Preview_WarmLamp", new Vector3(-4.0f, 1.65f, -0.12f), new Color(1f, 0.62f, 0.34f), 3.6f, 4.0f);
        CreateSpotLight("RTv06_Preview_WetFloorGrazing", new Vector3(-1.1f, 2.0f, -2.1f), new Vector3(1.3f, 0.02f, 0.15f), new Color(1f, 0.64f, 0.36f), 2.4f, 56f, 6.2f);
        CreateCamera("RTv06_PreviewCamera", new Vector3(0.2f, 1.22f, -4.65f), new Vector3(0.45f, 1.08f, 0.52f), 43f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), PreviewScenePathV06);
    }

    private static void BuildReferenceMatchChamberV06(Material[] wallMaterials, Material[] floorMaterials, Material[] ceilingMaterials, Material mortarMaterial, Material grimeMaterial, Material lampGlassMaterial, Material brassMaterial, Material ironMaterial)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings(new Color(0.18f, 0.135f, 0.092f), 0.88f);
        RenderSettings.fogColor = new Color(0.032f, 0.024f, 0.017f);
        RenderSettings.fogDensity = 0.00065f;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
        RenderSettings.customReflectionTexture = null;
        RenderSettings.reflectionIntensity = 0.45f;
        Material floorMortarMaterial = CreateSimpleMaterial("RTv06_MAT_DeepFloorGrout", new Color(0.0025f, 0.002f, 0.0016f), 0f, 0.01f);

        CreateBox("RTv06_Floor_DeepMortarBacking", new Vector3(0f, -0.035f, -0.25f), new Vector3(12.4f, 0.06f, 13.15f), floorMortarMaterial);
        CreateBox("RTv06_BackWall_DeepMortarBacking", new Vector3(0f, 2.43f, 5.91f), new Vector3(12.4f, 4.86f, 0.07f), mortarMaterial);
        CreateBox("RTv06_LeftWall_DeepMortarBacking", new Vector3(-6.13f, 2.43f, -0.2f), new Vector3(0.07f, 4.86f, 12.3f), mortarMaterial);
        CreateBox("RTv06_RightWall_DeepMortarBacking", new Vector3(6.13f, 2.43f, -0.2f), new Vector3(0.07f, 4.86f, 12.3f), mortarMaterial);
        CreateBox("RTv06_Ceiling_DeepMortarBacking", new Vector3(0f, 4.87f, -0.2f), new Vector3(12.4f, 0.07f, 12.3f), mortarMaterial);

        CreateMasonryFaceGridV06("RTv06_BackWall_ShallowChippedFaces", BrickSurface.BackWall, wallMaterials, -6.0f, 6.0f, 0.14f, 4.64f, 5.85f, 34, 17, 1801, 0.94f, 0.9f);
        CreateMasonryFaceGridV06("RTv06_LeftWall_ShallowChippedFaces", BrickSurface.LeftWall, wallMaterials, -6.0f, 5.65f, 0.14f, 4.64f, -6.075f, 33, 17, 1819, 0.94f, 0.9f);
        CreateMasonryFaceGridV06("RTv06_RightWall_ShallowChippedFaces", BrickSurface.RightWall, wallMaterials, -6.0f, 5.65f, 0.14f, 4.64f, 6.075f, 33, 17, 1837, 0.94f, 0.9f);
        GameObject continuousFloor = CreateBox("RTv06_Floor_ContinuousWetFlagstoneMaterial", new Vector3(0f, 0.018f, -0.25f), new Vector3(12.24f, 0.035f, 13.02f), floorMaterials[1]);
        continuousFloor.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        CreateMasonryFaceGridV06("RTv06_Ceiling_SootedSmallFaces", BrickSurface.Ceiling, ceilingMaterials, -6.0f, 6.0f, -6.06f, 5.58f, 4.795f, 32, 13, 1871, 0.93f, 0.88f);

        CreateLocalizedGrimeV06(grimeMaterial);
        CreateWallLampV06("RTv06_Left_Gaslight", new Vector3(-6.03f, 2.35f, -1.28f), Quaternion.Euler(0f, 90f, 0f), lampGlassMaterial, brassMaterial, ironMaterial, false);
        CreateWallLampV06("RTv06_Right_Gaslight", new Vector3(6.03f, 2.35f, -1.28f), Quaternion.Euler(0f, -90f, 0f), lampGlassMaterial, brassMaterial, ironMaterial, true);

        CreatePointLight("RTv06_Left_LocalAmberBloom", new Vector3(-5.58f, 2.35f, -1.23f), new Color(1f, 0.64f, 0.36f), 2.95f, 2.45f);
        CreatePointLight("RTv06_Right_LocalAmberBloom", new Vector3(5.58f, 2.35f, -1.23f), new Color(1f, 0.64f, 0.36f), 2.95f, 2.45f);
        CreateSpotLight("RTv06_Left_WetFloorLampReflection", new Vector3(-5.55f, 2.02f, -1.12f), new Vector3(-2.25f, 0.03f, -3.15f), new Color(1f, 0.61f, 0.31f), 1.55f, 38f, 7.1f);
        CreateSpotLight("RTv06_Right_WetFloorLampReflection", new Vector3(5.55f, 2.02f, -1.12f), new Vector3(2.25f, 0.03f, -3.15f), new Color(1f, 0.61f, 0.31f), 1.55f, 38f, 7.1f);
        CreateSpotLight("RTv06_BackWall_WarmReadableBounce", new Vector3(0f, 3.05f, -4.4f), new Vector3(0f, 2.1f, 5.75f), new Color(0.92f, 0.78f, 0.6f), 3.1f, 78f, 13.4f);
        CreateSpotLight("RTv06_FrontLowWarmFill", new Vector3(0f, 2.4f, -6.15f), new Vector3(0f, 0.85f, 1.8f), new Color(0.82f, 0.62f, 0.42f), 3.25f, 86f, 10.5f);
        CreateSpotLight("RTv06_Ceiling_LowWarmGrazing", new Vector3(0f, 4.35f, -4.2f), new Vector3(0f, 3.55f, 2.7f), new Color(0.86f, 0.58f, 0.34f), 0.62f, 66f, 9.5f);
        CreatePointFillLight("RTv06_NoShadow_WarmRoomExposure", new Vector3(0f, 2.85f, -2.25f), new Color(0.92f, 0.72f, 0.52f), 2.75f, 10.5f);
        CreatePointFillLight("RTv06_NoShadow_BackWallSoftReadability", new Vector3(0f, 2.45f, 2.35f), new Color(0.72f, 0.58f, 0.43f), 1.55f, 7.5f);

        GameObject probeObject = new GameObject("RTv06_ReflectionProbe");
        probeObject.transform.position = new Vector3(0f, 1.45f, -1.15f);
        ReflectionProbe probe = probeObject.AddComponent<ReflectionProbe>();
        probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
        probe.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        probe.size = new Vector3(12.1f, 4.7f, 12f);
        probe.intensity = 1.08f;
        probe.resolution = 512;

        CreateCamera("RTv06_RenderCamera", new Vector3(0f, 1.58f, -6.58f), new Vector3(0f, 1.98f, 4.95f), 67f);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), RoomScenePathV06);
    }

    private static void ConfigureRenderSettings(Color ambient, float reflectionIntensity)
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = ambient;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(0.018f, 0.015f, 0.012f);
        RenderSettings.fogDensity = 0.0045f;
        RenderSettings.reflectionIntensity = reflectionIntensity;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
    }

    private static void CreateFloorSlabs(string rootName, Material[] materials, float minX, float maxX, float minZ, float maxZ, int columns, int rows, int seed)
    {
        GameObject root = new GameObject(rootName);
        float cellW = (maxX - minX) / columns;
        float cellD = (maxZ - minZ) / rows;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float hash = Hash01(col, row, seed);
                float x = minX + (col + 0.5f) * cellW + (hash - 0.5f) * 0.08f;
                float z = minZ + (row + 0.5f) * cellD + (Hash01(col, row, seed + 13) - 0.5f) * 0.08f;
                float sx = cellW * Mathf.Lerp(0.82f, 0.94f, Hash01(col, row, seed + 29));
                float sz = cellD * Mathf.Lerp(0.82f, 0.96f, Hash01(col, row, seed + 31));
                float h = Mathf.Lerp(0.055f, 0.095f, Hash01(col, row, seed + 41));
                GameObject slab = CreateBox(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, h * 0.5f, z), new Vector3(sx, h, sz), Pick(materials, col, row, seed));
                slab.transform.SetParent(root.transform, true);
            }
        }
    }

    private static void CreateBackWallBricks(string rootName, Material[] materials, float minX, float maxX, float minY, float maxY, float zFront, int columns, int rows, int seed)
    {
        GameObject root = new GameObject(rootName);
        float cellW = (maxX - minX) / columns;
        float cellH = (maxY - minY) / rows;
        for (int row = 0; row < rows; row++)
        {
            float shift = (row & 1) == 0 ? 0f : cellW * 0.48f;
            for (int col = 0; col < columns; col++)
            {
                float x = minX + (col + 0.5f) * cellW + shift;
                if (x > maxX - cellW * 0.2f)
                {
                    continue;
                }

                float y = minY + (row + 0.5f) * cellH + (Hash01(col, row, seed + 11) - 0.5f) * 0.025f;
                float sx = cellW * Mathf.Lerp(0.78f, 0.95f, Hash01(col, row, seed + 23));
                float sy = cellH * Mathf.Lerp(0.72f, 0.92f, Hash01(col, row, seed + 37));
                float depth = Mathf.Lerp(0.045f, 0.11f, Hash01(col, row, seed + 43));
                GameObject brick = CreateBox(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, y, zFront + depth * 0.5f), new Vector3(sx, sy, depth), Pick(materials, col, row, seed));
                brick.transform.SetParent(root.transform, true);
            }
        }
    }

    private static void CreateSideWallBricks(string rootName, Material[] materials, float xFront, float minZ, float maxZ, float minY, float maxY, int columns, int rows, bool leftWall, int seed)
    {
        GameObject root = new GameObject(rootName);
        float cellD = (maxZ - minZ) / columns;
        float cellH = (maxY - minY) / rows;
        for (int row = 0; row < rows; row++)
        {
            float shift = (row & 1) == 0 ? 0f : cellD * 0.48f;
            for (int col = 0; col < columns; col++)
            {
                float z = minZ + (col + 0.5f) * cellD + shift;
                if (z > maxZ - cellD * 0.2f)
                {
                    continue;
                }

                float y = minY + (row + 0.5f) * cellH + (Hash01(col, row, seed + 13) - 0.5f) * 0.025f;
                float sx = Mathf.Lerp(0.045f, 0.11f, Hash01(col, row, seed + 19));
                float sz = cellD * Mathf.Lerp(0.78f, 0.95f, Hash01(col, row, seed + 29));
                float sy = cellH * Mathf.Lerp(0.72f, 0.92f, Hash01(col, row, seed + 31));
                float x = leftWall ? xFront - sx * 0.5f : xFront + sx * 0.5f;
                GameObject brick = CreateBox(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, y, z), new Vector3(sx, sy, sz), Pick(materials, col, row, seed));
                brick.transform.SetParent(root.transform, true);
            }
        }
    }

    private static void CreateCeilingBricks(string rootName, Material[] materials, float minX, float maxX, float minZ, float maxZ, float yFront, int columns, int rows, int seed)
    {
        GameObject root = new GameObject(rootName);
        float cellW = (maxX - minX) / columns;
        float cellD = (maxZ - minZ) / rows;
        for (int row = 0; row < rows; row++)
        {
            float shift = (row & 1) == 0 ? 0f : cellW * 0.48f;
            for (int col = 0; col < columns; col++)
            {
                float x = minX + (col + 0.5f) * cellW + shift;
                if (x > maxX - cellW * 0.2f)
                {
                    continue;
                }

                float z = minZ + (row + 0.5f) * cellD;
                float sx = cellW * Mathf.Lerp(0.78f, 0.95f, Hash01(col, row, seed + 23));
                float sz = cellD * Mathf.Lerp(0.72f, 0.92f, Hash01(col, row, seed + 37));
                float depth = Mathf.Lerp(0.04f, 0.08f, Hash01(col, row, seed + 43));
                GameObject brick = CreateBox(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, yFront + depth * 0.5f, z), new Vector3(sx, depth, sz), Pick(materials, col, row, seed));
                brick.transform.SetParent(root.transform, true);
            }
        }
    }

    private static void CreateCornerAndBaseGrime(Material material)
    {
        CreateBox("RTv03_LeftBackCorner_SootColumn", new Vector3(-6.02f, 2.2f, 5.72f), new Vector3(0.07f, 4.35f, 0.08f), material);
        CreateBox("RTv03_RightBackCorner_SootColumn", new Vector3(6.02f, 2.2f, 5.72f), new Vector3(0.07f, 4.35f, 0.08f), material);
        CreateBox("RTv03_BackWall_BaseMoistureBand", new Vector3(0f, 0.17f, 5.72f), new Vector3(11.7f, 0.22f, 0.07f), material);
        CreateBox("RTv03_LeftWall_BaseMoistureBand", new Vector3(-6.0f, 0.17f, -0.2f), new Vector3(0.07f, 0.22f, 11.6f), material);
        CreateBox("RTv03_RightWall_BaseMoistureBand", new Vector3(6.0f, 0.17f, -0.2f), new Vector3(0.07f, 0.22f, 11.6f), material);
        CreateBox("RTv03_CeilingFrontSmokeGradient", new Vector3(0f, 4.72f, -5.55f), new Vector3(12.0f, 0.06f, 0.45f), material);
        CreateBox("RTv03_BackWall_UpperSmokeBand", new Vector3(0f, 4.43f, 5.72f), new Vector3(11.8f, 0.42f, 0.07f), material);
    }

    private static void CreateFloorSlabsTight(string rootName, Material[] materials, float minX, float maxX, float minZ, float maxZ, int columns, int rows, int seed)
    {
        GameObject root = new GameObject(rootName);
        float cellW = (maxX - minX) / columns;
        float cellD = (maxZ - minZ) / rows;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float h = Mathf.Lerp(0.022f, 0.048f, Hash01(col, row, seed + 41));
                float x = minX + (col + 0.5f) * cellW + (Hash01(col, row, seed + 7) - 0.5f) * 0.025f;
                float z = minZ + (row + 0.5f) * cellD + (Hash01(col, row, seed + 13) - 0.5f) * 0.025f;
                float sx = cellW * Mathf.Lerp(0.94f, 0.985f, Hash01(col, row, seed + 29));
                float sz = cellD * Mathf.Lerp(0.93f, 0.985f, Hash01(col, row, seed + 31));
                GameObject slab = CreateBox(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, h * 0.5f, z), new Vector3(sx, h, sz), Pick(materials, col, row, seed));
                slab.transform.SetParent(root.transform, true);
            }
        }
    }

    private static void CreateBackWallBricksTight(string rootName, Material[] materials, float minX, float maxX, float minY, float maxY, float zFront, int columns, int rows, int seed)
    {
        GameObject root = new GameObject(rootName);
        float cellW = (maxX - minX) / columns;
        float cellH = (maxY - minY) / rows;
        for (int row = 0; row < rows; row++)
        {
            float shift = (row & 1) == 0 ? 0f : cellW * 0.49f;
            for (int col = 0; col < columns; col++)
            {
                float x = minX + (col + 0.5f) * cellW + shift;
                if (x > maxX - cellW * 0.1f)
                {
                    continue;
                }

                float y = minY + (row + 0.5f) * cellH + (Hash01(col, row, seed + 11) - 0.5f) * 0.012f;
                float sx = cellW * Mathf.Lerp(0.89f, 0.965f, Hash01(col, row, seed + 23));
                float sy = cellH * Mathf.Lerp(0.84f, 0.955f, Hash01(col, row, seed + 37));
                float depth = Mathf.Lerp(0.018f, 0.045f, Hash01(col, row, seed + 43));
                GameObject brick = CreateBox(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, y, zFront + depth * 0.5f), new Vector3(sx, sy, depth), Pick(materials, col, row, seed));
                brick.transform.SetParent(root.transform, true);
            }
        }
    }

    private static void CreateSideWallBricksTight(string rootName, Material[] materials, float xFront, float minZ, float maxZ, float minY, float maxY, int columns, int rows, bool leftWall, int seed)
    {
        GameObject root = new GameObject(rootName);
        float cellD = (maxZ - minZ) / columns;
        float cellH = (maxY - minY) / rows;
        for (int row = 0; row < rows; row++)
        {
            float shift = (row & 1) == 0 ? 0f : cellD * 0.49f;
            for (int col = 0; col < columns; col++)
            {
                float z = minZ + (col + 0.5f) * cellD + shift;
                if (z > maxZ - cellD * 0.1f)
                {
                    continue;
                }

                float y = minY + (row + 0.5f) * cellH + (Hash01(col, row, seed + 13) - 0.5f) * 0.012f;
                float sx = Mathf.Lerp(0.018f, 0.045f, Hash01(col, row, seed + 19));
                float sz = cellD * Mathf.Lerp(0.89f, 0.965f, Hash01(col, row, seed + 29));
                float sy = cellH * Mathf.Lerp(0.84f, 0.955f, Hash01(col, row, seed + 31));
                float x = leftWall ? xFront - sx * 0.5f : xFront + sx * 0.5f;
                GameObject brick = CreateBox(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, y, z), new Vector3(sx, sy, sz), Pick(materials, col, row, seed));
                brick.transform.SetParent(root.transform, true);
            }
        }
    }

    private static void CreateCeilingBricksTight(string rootName, Material[] materials, float minX, float maxX, float minZ, float maxZ, float yFront, int columns, int rows, int seed)
    {
        GameObject root = new GameObject(rootName);
        float cellW = (maxX - minX) / columns;
        float cellD = (maxZ - minZ) / rows;
        for (int row = 0; row < rows; row++)
        {
            float shift = (row & 1) == 0 ? 0f : cellW * 0.49f;
            for (int col = 0; col < columns; col++)
            {
                float x = minX + (col + 0.5f) * cellW + shift;
                if (x > maxX - cellW * 0.1f)
                {
                    continue;
                }

                float z = minZ + (row + 0.5f) * cellD;
                float sx = cellW * Mathf.Lerp(0.89f, 0.965f, Hash01(col, row, seed + 23));
                float sz = cellD * Mathf.Lerp(0.84f, 0.955f, Hash01(col, row, seed + 37));
                float depth = Mathf.Lerp(0.014f, 0.035f, Hash01(col, row, seed + 43));
                GameObject brick = CreateBox(rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, yFront + depth * 0.5f, z), new Vector3(sx, depth, sz), Pick(materials, col, row, seed));
                brick.transform.SetParent(root.transform, true);
            }
        }
    }

    private static void CreateCornerAndBaseGrimeV04(Material material)
    {
        CreateBox("RTv04_LeftBackCorner_SoftSoot", new Vector3(-6.01f, 2.2f, 5.72f), new Vector3(0.045f, 4.25f, 0.055f), material);
        CreateBox("RTv04_RightBackCorner_SoftSoot", new Vector3(6.01f, 2.2f, 5.72f), new Vector3(0.045f, 4.25f, 0.055f), material);
        CreateBox("RTv04_BackWall_BaseDampBand", new Vector3(0f, 0.12f, 5.72f), new Vector3(11.8f, 0.16f, 0.045f), material);
        CreateBox("RTv04_LeftWall_BaseDampBand", new Vector3(-5.99f, 0.12f, -0.2f), new Vector3(0.045f, 0.16f, 11.65f), material);
        CreateBox("RTv04_RightWall_BaseDampBand", new Vector3(5.99f, 0.12f, -0.2f), new Vector3(0.045f, 0.16f, 11.65f), material);
        CreateBox("RTv04_BackWall_UpperSoot", new Vector3(0f, 4.48f, 5.72f), new Vector3(11.8f, 0.3f, 0.045f), material);
    }

    private static void CreateMasonryFaceGridV06(string rootName, BrickSurface surface, Material[] materials, float minA, float maxA, float minB, float maxB, float plane, int columns, int rows, int seed, float fillA, float fillB)
    {
        GameObject root = new GameObject(rootName);
        float cellA = (maxA - minA) / columns;
        float cellB = (maxB - minB) / rows;
        for (int row = 0; row < rows; row++)
        {
            float stagger = surface == BrickSurface.Floor ? ((row & 1) == 0 ? 0f : cellA * 0.18f) : ((row & 1) == 0 ? 0f : cellA * 0.48f);
            for (int col = -1; col <= columns; col++)
            {
                float randomA = (Hash01(col, row, seed + 11) - 0.5f) * cellA * 0.15f;
                float randomB = (Hash01(col, row, seed + 17) - 0.5f) * cellB * 0.13f;
                float centerA = minA + (col + 0.5f) * cellA + stagger + randomA;
                float centerB = minB + (row + 0.5f) * cellB + randomB;
                if (centerA < minA + cellA * 0.15f || centerA > maxA - cellA * 0.15f)
                {
                    continue;
                }

                if (surface == BrickSurface.Floor)
                {
                    float width = cellA * fillA * Mathf.Lerp(0.982f, 1.012f, Hash01(col, row, seed + 29));
                    float height = cellB * fillB * Mathf.Lerp(0.974f, 1.012f, Hash01(col, row, seed + 37));
                    width *= Mathf.Lerp(0.992f, 1.018f, Hash01(col, row, seed + 43));
                    height *= Mathf.Lerp(0.988f, 1.016f, Hash01(col, row, seed + 47));
                    CreateMasonryFaceGridItemV06(root, rootName, surface, materials, minA, maxA, minB, maxB, plane, col, row, seed, centerA, centerB, width, height);
                    continue;
                }

                CreateMasonryFaceGridItemV06(
                    root,
                    rootName,
                    surface,
                    materials,
                    minA,
                    maxA,
                    minB,
                    maxB,
                    plane,
                    col,
                    row,
                    seed,
                    centerA,
                    centerB,
                    cellA * fillA * Mathf.Lerp(0.88f, 1.02f, Hash01(col, row, seed + 29)),
                    cellB * fillB * Mathf.Lerp(0.86f, 1.03f, Hash01(col, row, seed + 37)));
            }
        }
    }

    private static void CreateMasonryFaceGridItemV06(GameObject root, string rootName, BrickSurface surface, Material[] materials, float minA, float maxA, float minB, float maxB, float plane, int col, int row, int seed, float centerA, float centerB, float width, float height)
    {
        float minU = Mathf.InverseLerp(minA, maxA, centerA - width * 0.5f);
        float maxU = Mathf.InverseLerp(minA, maxA, centerA + width * 0.5f);
        float minV = Mathf.InverseLerp(minB, maxB, centerB - height * 0.5f);
        float maxV = Mathf.InverseLerp(minB, maxB, centerB + height * 0.5f);
        string name = rootName + "_" + row.ToString("00", CultureInfo.InvariantCulture) + "_" + col.ToString("00", CultureInfo.InvariantCulture);
        Material material = Pick(materials, col, row, seed);
        GameObject face = CreateSurfaceQuadV06(name, surface, centerA, centerB, plane, width, height, new Vector4(minU, minV, maxU, maxV), seed + col * 13 + row * 31, material);
        face.transform.SetParent(root.transform, true);
    }

    private static GameObject CreateSurfaceQuadV06(string name, BrickSurface surface, float centerA, float centerB, float plane, float width, float height, Vector4 uvRect, int seed, Material material)
    {
        Vector3 center;
        Vector3 axisA;
        Vector3 axisB;
        bool flip;
        switch (surface)
        {
            case BrickSurface.LeftWall:
                center = new Vector3(plane, centerB, centerA);
                axisA = Vector3.forward;
                axisB = Vector3.up;
                flip = false;
                break;
            case BrickSurface.RightWall:
                center = new Vector3(plane, centerB, centerA);
                axisA = Vector3.forward;
                axisB = Vector3.up;
                flip = true;
                break;
            case BrickSurface.Floor:
                center = new Vector3(centerA, plane, centerB);
                axisA = Vector3.right;
                axisB = Vector3.forward;
                flip = false;
                break;
            case BrickSurface.Ceiling:
                center = new Vector3(centerA, plane, centerB);
                axisA = Vector3.right;
                axisB = Vector3.forward;
                flip = true;
                break;
            default:
                center = new Vector3(centerA, centerB, plane);
                axisA = Vector3.right;
                axisB = Vector3.up;
                flip = false;
                break;
        }

        float chip = Mathf.Min(width, height) * 0.055f;
        Vector3 p0 = center - axisA * width * 0.5f - axisB * height * 0.5f + CornerJitter(axisA, axisB, chip, seed + 1);
        Vector3 p1 = center - axisA * width * 0.5f + axisB * height * 0.5f + CornerJitter(axisA, axisB, chip, seed + 2);
        Vector3 p2 = center + axisA * width * 0.5f + axisB * height * 0.5f + CornerJitter(axisA, axisB, chip, seed + 3);
        Vector3 p3 = center + axisA * width * 0.5f - axisB * height * 0.5f + CornerJitter(axisA, axisB, chip, seed + 4);

        Mesh mesh = new Mesh();
        mesh.name = name + "_Mesh";
        mesh.vertices = new[] { p0, p1, p2, p3 };
        mesh.uv = new[]
        {
            new Vector2(uvRect.x, uvRect.y),
            new Vector2(uvRect.x, uvRect.w),
            new Vector2(uvRect.z, uvRect.w),
            new Vector2(uvRect.z, uvRect.y)
        };
        mesh.triangles = flip ? new[] { 0, 2, 1, 0, 3, 2 } : new[] { 0, 1, 2, 0, 2, 3 };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GameObject obj = new GameObject(name);
        obj.AddComponent<MeshFilter>().sharedMesh = mesh;
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = true;
        return obj;
    }

    private static Vector3 CornerJitter(Vector3 axisA, Vector3 axisB, float amount, int seed)
    {
        float a = (Hash01(seed, 19, seed + 101) - 0.5f) * amount;
        float b = (Hash01(seed, 23, seed + 107) - 0.5f) * amount;
        return axisA * a + axisB * b;
    }

    private static void CreateLocalizedGrimeV06(Material material)
    {
        CreateBox("RTv06_LeftBackCorner_SootColumn", new Vector3(-6.045f, 2.35f, 5.72f), new Vector3(0.04f, 4.45f, 0.07f), material);
        CreateBox("RTv06_RightBackCorner_SootColumn", new Vector3(6.045f, 2.35f, 5.72f), new Vector3(0.04f, 4.45f, 0.07f), material);
        CreateBox("RTv06_BackWall_DampBase", new Vector3(0f, 0.12f, 5.78f), new Vector3(12.0f, 0.18f, 0.035f), material);
        CreateBox("RTv06_LeftWall_DampBase", new Vector3(-6.04f, 0.12f, -0.22f), new Vector3(0.035f, 0.18f, 11.8f), material);
        CreateBox("RTv06_RightWall_DampBase", new Vector3(6.04f, 0.12f, -0.22f), new Vector3(0.035f, 0.18f, 11.8f), material);
        CreateBox("RTv06_BackWall_CeilingSoot", new Vector3(0f, 4.53f, 5.78f), new Vector3(12.0f, 0.28f, 0.035f), material);
        CreateBox("RTv06_Floor_FrontDampShadow", new Vector3(0f, 0.025f, -5.72f), new Vector3(12.0f, 0.03f, 0.22f), material);
        CreateBox("RTv06_Floor_BackDampShadow", new Vector3(0f, 0.024f, 5.45f), new Vector3(11.8f, 0.028f, 0.24f), material);
    }

    private static void CreateWallLamp(string name, Vector3 position, Quaternion rotation, Material glassMaterial, Material brassMaterial, Material ironMaterial, bool mirror)
    {
        GameObject root = new GameObject(name);
        root.transform.position = position;
        root.transform.rotation = rotation;

        GameObject backPlate = CreateLocalPrimitive(root, name + "_RivetedIronBackPlate", PrimitiveType.Cube, new Vector3(0f, 0f, 0f), new Vector3(0.44f, 0.76f, 0.07f), ironMaterial);
        GameObject glass = CreateLocalPrimitive(root, name + "_ConvexAmberGlass", PrimitiveType.Sphere, new Vector3(0f, 0f, -0.09f), new Vector3(0.26f, 0.35f, 0.13f), glassMaterial);
        GameObject brassRing = CreateLocalPrimitive(root, name + "_AgedBrassRim", PrimitiveType.Cylinder, new Vector3(0f, 0f, -0.112f), new Vector3(0.36f, 0.035f, 0.36f), brassMaterial);
        brassRing.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        for (int i = -1; i <= 1; i += 2)
        {
            CreateLocalPrimitive(root, name + "_HorizontalBrassCage_" + i.ToString(CultureInfo.InvariantCulture), PrimitiveType.Cube, new Vector3(0f, i * 0.24f, -0.135f), new Vector3(0.42f, 0.032f, 0.045f), brassMaterial);
            CreateLocalPrimitive(root, name + "_VerticalBrassCage_" + i.ToString(CultureInfo.InvariantCulture), PrimitiveType.Cube, new Vector3(i * 0.19f, 0f, -0.135f), new Vector3(0.032f, 0.52f, 0.045f), brassMaterial);
        }

        float stemX = mirror ? 0.34f : -0.34f;
        CreateLocalPrimitive(root, name + "_ShortBlackIronPipe", PrimitiveType.Cube, new Vector3(stemX, 0f, 0.005f), new Vector3(0.17f, 0.1f, 0.07f), ironMaterial);
        CreateLocalPrimitive(root, name + "_SmallPressureValve", PrimitiveType.Cylinder, new Vector3(stemX + (mirror ? 0.1f : -0.1f), 0f, -0.01f), new Vector3(0.12f, 0.035f, 0.12f), brassMaterial);
        UnityEngine.Object.DestroyImmediate(backPlate.GetComponent<Collider>());
        UnityEngine.Object.DestroyImmediate(glass.GetComponent<Collider>());
    }

    private static void CreateWallLampV05(string name, Vector3 position, Quaternion rotation, Material glassMaterial, Material brassMaterial, Material ironMaterial, bool mirror)
    {
        GameObject root = new GameObject(name);
        root.transform.position = position;
        root.transform.rotation = rotation;

        CreateLocalPrimitive(root, name + "_SlimSootedIronBackPlate", PrimitiveType.Cube, new Vector3(0f, 0f, 0.012f), new Vector3(0.26f, 0.52f, 0.04f), ironMaterial);
        GameObject glass = CreateLocalPrimitive(root, name + "_SmallWarmGlassLens", PrimitiveType.Sphere, new Vector3(0f, 0f, -0.074f), new Vector3(0.2f, 0.29f, 0.095f), glassMaterial);
        GameObject rim = CreateLocalPrimitive(root, name + "_ThinTarnishedBrassRim", PrimitiveType.Cylinder, new Vector3(0f, 0f, -0.092f), new Vector3(0.25f, 0.022f, 0.25f), brassMaterial);
        rim.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        CreateLocalPrimitive(root, name + "_TopBrassRetainer", PrimitiveType.Cube, new Vector3(0f, 0.17f, -0.105f), new Vector3(0.29f, 0.024f, 0.035f), brassMaterial);
        CreateLocalPrimitive(root, name + "_BottomBrassRetainer", PrimitiveType.Cube, new Vector3(0f, -0.17f, -0.105f), new Vector3(0.29f, 0.024f, 0.035f), brassMaterial);
        CreateLocalPrimitive(root, name + "_LeftBrassCageRib", PrimitiveType.Cube, new Vector3(-0.115f, 0f, -0.108f), new Vector3(0.024f, 0.37f, 0.033f), brassMaterial);
        CreateLocalPrimitive(root, name + "_RightBrassCageRib", PrimitiveType.Cube, new Vector3(0.115f, 0f, -0.108f), new Vector3(0.024f, 0.37f, 0.033f), brassMaterial);

        float pipeX = mirror ? 0.235f : -0.235f;
        CreateLocalPrimitive(root, name + "_BlackIronFeedPipe", PrimitiveType.Cube, new Vector3(pipeX, -0.07f, 0.01f), new Vector3(0.18f, 0.055f, 0.055f), ironMaterial);
        GameObject valve = CreateLocalPrimitive(root, name + "_SmallBrassValveCap", PrimitiveType.Cylinder, new Vector3(pipeX + (mirror ? 0.105f : -0.105f), -0.07f, -0.01f), new Vector3(0.075f, 0.022f, 0.075f), brassMaterial);
        valve.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        glass.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private static void CreateWallLampV06(string name, Vector3 position, Quaternion rotation, Material glassMaterial, Material brassMaterial, Material ironMaterial, bool mirror)
    {
        GameObject root = new GameObject(name);
        root.transform.position = position;
        root.transform.rotation = rotation;

        CreateLocalPrimitive(root, name + "_InsetBlackIronBackbox", PrimitiveType.Cube, new Vector3(0f, 0f, 0.018f), new Vector3(0.38f, 0.72f, 0.055f), ironMaterial);
        CreateLocalPrimitive(root, name + "_UpperSootCap", PrimitiveType.Cube, new Vector3(0f, 0.39f, -0.01f), new Vector3(0.42f, 0.05f, 0.08f), ironMaterial);
        CreateLocalPrimitive(root, name + "_LowerSootCap", PrimitiveType.Cube, new Vector3(0f, -0.39f, -0.01f), new Vector3(0.42f, 0.05f, 0.08f), ironMaterial);

        GameObject glass = CreateLocalPrimitive(root, name + "_TallHotAmberGlass", PrimitiveType.Sphere, new Vector3(0f, 0f, -0.105f), new Vector3(0.22f, 0.36f, 0.105f), glassMaterial);
        GameObject rim = CreateLocalPrimitive(root, name + "_OvalTarnishedBrassRim", PrimitiveType.Cylinder, new Vector3(0f, 0f, -0.128f), new Vector3(0.34f, 0.024f, 0.34f), brassMaterial);
        rim.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        for (int i = -1; i <= 1; i++)
        {
            float x = i * 0.115f;
            CreateLocalPrimitive(root, name + "_VerticalCageRib_" + i.ToString(CultureInfo.InvariantCulture), PrimitiveType.Cube, new Vector3(x, 0f, -0.147f), new Vector3(0.018f, 0.62f, 0.03f), brassMaterial);
        }

        CreateLocalPrimitive(root, name + "_TopCageRail", PrimitiveType.Cube, new Vector3(0f, 0.22f, -0.15f), new Vector3(0.32f, 0.022f, 0.03f), brassMaterial);
        CreateLocalPrimitive(root, name + "_BottomCageRail", PrimitiveType.Cube, new Vector3(0f, -0.22f, -0.15f), new Vector3(0.32f, 0.022f, 0.03f), brassMaterial);

        float pipeX = mirror ? 0.31f : -0.31f;
        CreateLocalPrimitive(root, name + "_WallFeedPipe", PrimitiveType.Cube, new Vector3(pipeX, -0.12f, 0.008f), new Vector3(0.22f, 0.055f, 0.055f), ironMaterial);
        GameObject valve = CreateLocalPrimitive(root, name + "_BrassValveWheel", PrimitiveType.Cylinder, new Vector3(pipeX + (mirror ? 0.135f : -0.135f), -0.12f, -0.02f), new Vector3(0.095f, 0.02f, 0.095f), brassMaterial);
        valve.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        glass.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private static GameObject CreateLocalPrimitive(GameObject parent, string name, PrimitiveType primitive, Vector3 localPosition, Vector3 localScale, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(primitive);
        obj.name = name;
        obj.transform.SetParent(parent.transform, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localScale = localScale;
        obj.GetComponent<Renderer>().sharedMaterial = material;
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            UnityEngine.Object.DestroyImmediate(collider);
        }

        return obj;
    }

    private static GameObject CreateBox(string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.position = position;
        box.transform.localScale = scale;
        box.GetComponent<Renderer>().sharedMaterial = material;
        Collider collider = box.GetComponent<Collider>();
        if (collider != null)
        {
            UnityEngine.Object.DestroyImmediate(collider);
        }

        return box;
    }

    private static void CreateCamera(string name, Vector3 position, Vector3 lookAt, float fieldOfView)
    {
        GameObject cameraObject = new GameObject(name);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = position;
        camera.transform.rotation = Quaternion.LookRotation(lookAt - position, Vector3.up);
        camera.fieldOfView = fieldOfView;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 40f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.01f, 0.009f, 0.008f);
        camera.allowHDR = true;
        camera.allowMSAA = true;
        camera.depth = 1f;
    }

    private static void CreatePointLight(string name, Vector3 position, Color color, float intensity, float range)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        Light light = obj.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 0.84f;
        light.shadowBias = 0.035f;
        light.shadowNormalBias = 0.22f;
    }

    private static void CreatePointFillLight(string name, Vector3 position, Color color, float intensity, float range)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        Light light = obj.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.None;
    }

    private static void CreateSpotLight(string name, Vector3 position, Vector3 target, Color color, float intensity, float spotAngle, float range)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.LookRotation(target - position, Vector3.up);
        Light light = obj.AddComponent<Light>();
        light.type = LightType.Spot;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        light.spotAngle = spotAngle;
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 0.78f;
        light.shadowBias = 0.035f;
    }

    private static Material CreatePbrMaterial(string name, TextureSet textures, Vector2 tiling, float smoothness, float parallax, float normalScale)
    {
        Material material = CreateOrLoadMaterial(name);
        Texture2D albedo = LoadTexture(textures.Albedo);
        Texture2D normal = LoadTexture(textures.Normal);
        Texture2D height = LoadTexture(textures.Height);
        Texture2D ao = LoadTexture(textures.Occlusion);
        Texture2D metallicSmooth = LoadTexture(textures.MetallicSmoothness);
        SetTexture(material, "_MainTex", albedo, tiling);
        SetTexture(material, "_BaseMap", albedo, tiling);
        SetTexture(material, "_BumpMap", normal, tiling);
        SetTexture(material, "_ParallaxMap", height, tiling);
        SetTexture(material, "_OcclusionMap", ao, tiling);
        SetTexture(material, "_MetallicGlossMap", metallicSmooth, tiling);
        SetFloat(material, "_Metallic", 0f);
        SetFloat(material, "_Glossiness", smoothness);
        SetFloat(material, "_Smoothness", smoothness);
        SetFloat(material, "_GlossMapScale", 1f);
        SetFloat(material, "_BumpScale", normalScale);
        SetFloat(material, "_Parallax", parallax);
        SetColor(material, "_Color", Color.white);
        SetColor(material, "_BaseColor", Color.white);
        material.EnableKeyword("_NORMALMAP");
        material.EnableKeyword("_PARALLAXMAP");
        material.EnableKeyword("_METALLICGLOSSMAP");
        material.EnableKeyword("_OCCLUSIONMAP");
        EditorUtility.SetDirty(material);
        return material;
    }

    private static Material[] CreateMaterialVariants(string namePrefix, Material source, Color[] tintColors)
    {
        Material[] materials = new Material[tintColors.Length];
        for (int i = 0; i < tintColors.Length; i++)
        {
            string path = MaterialRoot + "/" + namePrefix + "_" + i.ToString("00", CultureInfo.InvariantCulture) + ".mat";
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

            SetColor(material, "_Color", tintColors[i]);
            SetColor(material, "_BaseColor", tintColors[i]);
            EditorUtility.SetDirty(material);
            materials[i] = material;
        }

        return materials;
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
        SetColor(material, "_Color", color);
        SetColor(material, "_BaseColor", color);
        SetColor(material, "_EmissionColor", color * intensity);
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

    private static string SaveTexture(string name, Color32[] pixels, TextureImporterType importerType, bool srgb)
    {
        Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, true, !srgb);
        texture.SetPixels32(pixels);
        texture.Apply(true, false);
        string assetPath = TextureRoot + "/" + name + ".png";
        File.WriteAllBytes(ProjectPath(assetPath), texture.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = importerType;
            importer.sRGBTexture = srgb;
            importer.mipmapEnabled = true;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.filterMode = FilterMode.Trilinear;
            importer.anisoLevel = 9;
            importer.maxTextureSize = TextureSize;
            importer.SaveAndReimport();
        }

        return assetPath;
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
                pixels[y * TextureSize + x] = new Color32(
                    (byte)Mathf.RoundToInt((n.x * 0.5f + 0.5f) * 255f),
                    (byte)Mathf.RoundToInt((n.y * 0.5f + 0.5f) * 255f),
                    (byte)Mathf.RoundToInt((n.z * 0.5f + 0.5f) * 255f),
                    255);
            }
        }

        return pixels;
    }

    private static string RenderSceneToFile(string fileName)
    {
        Camera camera = UnityEngine.Object.FindAnyObjectByType<Camera>();
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

        string renderPath = ProjectPath("Renders/" + fileName);
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
        int warmHighlights = 0;
        int darkPixels = 0;
        int detailPixels = 0;
        float maxLuminance = 0f;
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 p = pixels[i];
            float r = p.r / 255f;
            float g = p.g / 255f;
            float b = p.b / 255f;
            float l = r * 0.2126f + g * 0.7152f + b * 0.0722f;
            luminanceSum += l;
            maxLuminance = Mathf.Max(maxLuminance, l);
            if (l > 0.42f && r > g && g > b)
            {
                warmHighlights++;
            }

            if (l < 0.16f)
            {
                darkPixels++;
            }

            if (l > 0.055f && l < 0.38f)
            {
                detailPixels++;
            }
        }

        double average = luminanceSum / pixels.Length;
        UnityEngine.Object.DestroyImmediate(texture);

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("{");
        builder.AppendLine("  \"schema\": \"brassworks.roomtest.metrics.v2\",");
        builder.AppendLine("  \"render\": \"" + RoomRenderFileName + "\",");
        builder.AppendLine("  \"target_reference\": \"dark warm wet aged brick room\",");
        builder.AppendLine("  \"texture_size\": " + TextureSize.ToString(CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"render_size\": \"" + RenderWidth.ToString(CultureInfo.InvariantCulture) + "x" + RenderHeight.ToString(CultureInfo.InvariantCulture) + "\",");
        builder.AppendLine("  \"average_luminance\": " + average.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"max_luminance\": " + maxLuminance.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"warm_highlight_pixel_ratio\": " + ((double)warmHighlights / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"dark_pixel_ratio\": " + ((double)darkPixels / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"mid_detail_pixel_ratio\": " + ((double)detailPixels / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"assessment_target\": \"visual review should show actual masonry relief, localized amber lamps, wet floor response, and no global orange wash\"");
        builder.AppendLine("}");
        File.WriteAllText(ProjectPath("Renders/" + MetricsFileName), builder.ToString());
    }

    private static void WriteMetricsV04(string renderPath)
    {
        byte[] bytes = File.ReadAllBytes(renderPath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        texture.LoadImage(bytes);
        Color32[] pixels = texture.GetPixels32();
        double luminanceSum = 0d;
        int warmHighlights = 0;
        int darkPixels = 0;
        int detailPixels = 0;
        float maxLuminance = 0f;
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 p = pixels[i];
            float r = p.r / 255f;
            float g = p.g / 255f;
            float b = p.b / 255f;
            float l = r * 0.2126f + g * 0.7152f + b * 0.0722f;
            luminanceSum += l;
            maxLuminance = Mathf.Max(maxLuminance, l);
            if (l > 0.38f && r > g && g > b)
            {
                warmHighlights++;
            }

            if (l < 0.16f)
            {
                darkPixels++;
            }

            if (l > 0.055f && l < 0.38f)
            {
                detailPixels++;
            }
        }

        double average = luminanceSum / pixels.Length;
        UnityEngine.Object.DestroyImmediate(texture);

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("{");
        builder.AppendLine("  \"schema\": \"brassworks.roomtest.metrics.v2\",");
        builder.AppendLine("  \"render\": \"" + RoomRenderFileNameV04 + "\",");
        builder.AppendLine("  \"target_reference\": \"dark warm wet aged brick room\",");
        builder.AppendLine("  \"texture_size\": " + TextureSize.ToString(CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"render_size\": \"" + RenderWidth.ToString(CultureInfo.InvariantCulture) + "x" + RenderHeight.ToString(CultureInfo.InvariantCulture) + "\",");
        builder.AppendLine("  \"average_luminance\": " + average.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"max_luminance\": " + maxLuminance.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"warm_highlight_pixel_ratio\": " + ((double)warmHighlights / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"dark_pixel_ratio\": " + ((double)darkPixels / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"mid_detail_pixel_ratio\": " + ((double)detailPixels / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"assessment_target\": \"v0.4 should reduce chunky gaps, reveal the back wall, keep warm lamps localized, and preserve wet floor glints\"");
        builder.AppendLine("}");
        File.WriteAllText(ProjectPath("Renders/" + MetricsFileNameV04), builder.ToString());
    }

    private static void WriteMetricsGeneric(string renderPath, string renderFileName, string metricsFileName, string target)
    {
        byte[] bytes = File.ReadAllBytes(renderPath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        texture.LoadImage(bytes);
        Color32[] pixels = texture.GetPixels32();
        double luminanceSum = 0d;
        int warmHighlights = 0;
        int darkPixels = 0;
        int detailPixels = 0;
        float maxLuminance = 0f;
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 p = pixels[i];
            float r = p.r / 255f;
            float g = p.g / 255f;
            float b = p.b / 255f;
            float l = r * 0.2126f + g * 0.7152f + b * 0.0722f;
            luminanceSum += l;
            maxLuminance = Mathf.Max(maxLuminance, l);
            if (l > 0.38f && r > g && g > b)
            {
                warmHighlights++;
            }

            if (l < 0.16f)
            {
                darkPixels++;
            }

            if (l > 0.055f && l < 0.38f)
            {
                detailPixels++;
            }
        }

        double average = luminanceSum / pixels.Length;
        UnityEngine.Object.DestroyImmediate(texture);

        StringBuilder builder = new StringBuilder();
        builder.AppendLine("{");
        builder.AppendLine("  \"schema\": \"brassworks.roomtest.metrics.v2\",");
        builder.AppendLine("  \"render\": \"" + renderFileName + "\",");
        builder.AppendLine("  \"target_reference\": \"" + target + "\",");
        builder.AppendLine("  \"texture_size\": " + TextureSize.ToString(CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"render_size\": \"" + RenderWidth.ToString(CultureInfo.InvariantCulture) + "x" + RenderHeight.ToString(CultureInfo.InvariantCulture) + "\",");
        builder.AppendLine("  \"average_luminance\": " + average.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"max_luminance\": " + maxLuminance.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"warm_highlight_pixel_ratio\": " + ((double)warmHighlights / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"dark_pixel_ratio\": " + ((double)darkPixels / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"mid_detail_pixel_ratio\": " + ((double)detailPixels / pixels.Length).ToString("0.0000", CultureInfo.InvariantCulture) + ",");
        builder.AppendLine("  \"assessment_target\": \"continuous mapped surfaces should look closer to the reference than block geometry\"");
        builder.AppendLine("}");
        File.WriteAllText(ProjectPath("Renders/" + metricsFileName), builder.ToString());
    }

    private static void WriteReview(string previewPath, string roomPath)
    {
        string metricsPath = ProjectPath("Renders/" + MetricsFileName);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.3 Final Lookdev Review");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## Files");
        builder.AppendLine();
        builder.AppendLine("- Material preview: `" + previewPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Room render: `" + roomPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Metrics: `" + metricsPath.Replace('\\', '/') + "`");
        builder.AppendLine();
        builder.AppendLine("## What Changed");
        builder.AppendLine();
        builder.AppendLine("- Moved from material-only planes to actual per-brick and per-slab Unity geometry over dark recessed mortar.");
        builder.AppendLine("- Generated v0.3 albedo, normal, height, occlusion, and metallic/smoothness maps for wall, floor, and ceiling.");
        builder.AppendLine("- Checked the base albedo PNGs before material creation, then checked associated map PNGs before building Unity materials.");
        builder.AppendLine("- Added material tint variants to break up repeated procedural texture color.");
        builder.AppendLine("- Rebuilt the lamps with brass rims, cage bars, black iron backing, localized point lights, and floor-grazing spot reflections.");
        builder.AppendLine("- Added base grime bands and corner soot to reduce the clean test-room look.");
        builder.AppendLine();
        builder.AppendLine("## Acceptance Comparison");
        builder.AppendLine();
        builder.AppendLine("- Texture relief: target is chipped, uneven masonry; v0.3 uses real brick/slab geometry plus normal and height maps so the relief should read stronger than v0.2.");
        builder.AppendLine("- Wet reflection: target is damp floor glints, not orange metal; v0.3 keeps metallic at zero and packs floor smoothness into the metallic/smoothness alpha channel.");
        builder.AppendLine("- Warm wall light: target is two localized amber wall halos; v0.3 uses point lights plus floor-grazing spots while preserving a dark center and back wall.");
        builder.AppendLine("- Ceiling/floor scale: target is small wall/ceiling brick with larger floor stones; v0.3 separates those scales through geometry and texture profiles.");
        builder.AppendLine("- Corner depth: target corners stay dark and grounded; v0.3 adds recessed mortar bases, corner soot, and base moisture bands.");
        builder.AppendLine();
        builder.AppendLine("## Blunt Assessment");
        builder.AppendLine();
        builder.AppendLine("- This pass should be much closer to the reference than v0.2 because the brick relief is now real geometry instead of only normal-map detail.");
        builder.AppendLine("- If it still fails, the next fixes should be artistic rather than architectural: more broken silhouettes, better lamp glass, darker mortar pooling, and hand-placed grime/decal cards.");
        File.WriteAllText(ProjectPath("Documentation/" + ReviewFileName), builder.ToString());
    }

    private static void WriteReviewV04(string previewPath, string roomPath)
    {
        string metricsPath = ProjectPath("Renders/" + MetricsFileNameV04);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.4 Refinement Review");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## Files");
        builder.AppendLine();
        builder.AppendLine("- Material preview: `" + previewPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Room render: `" + roomPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Metrics: `" + metricsPath.Replace('\\', '/') + "`");
        builder.AppendLine();
        builder.AppendLine("## What Changed From v0.3");
        builder.AppendLine();
        builder.AppendLine("- Reduced brick and slab protrusion so geometry reads as shallow masonry relief instead of floating blocks.");
        builder.AppendLine("- Tightened mortar gaps and darkened albedo to brown-black stone.");
        builder.AppendLine("- Added neutral back-wall readability light while keeping corners dark.");
        builder.AppendLine("- Lowered amber lamp spread and kept wet floor glints as the main bright reflection.");
        builder.AppendLine("- Kept floor stones larger than wall and ceiling brick.");
        builder.AppendLine();
        builder.AppendLine("## Acceptance Comparison");
        builder.AppendLine();
        builder.AppendLine("- Texture relief: improved if brick courses are visible without chunky panel gaps; still fails if the geometry looks like separate tiles.");
        builder.AppendLine("- Wet reflection: improved if front floor glints read damp and non-metallic; still fails if reflections become orange plates.");
        builder.AppendLine("- Warm wall light: improved if lamps create local halos only; still fails if the whole wall washes orange.");
        builder.AppendLine("- Ceiling/floor scale: improved if ceiling brick is smaller and darker than the floor stones.");
        builder.AppendLine("- Corner depth: improved if back corners stay dark but the back wall is readable.");
        builder.AppendLine();
        builder.AppendLine("## Blunt Assessment");
        builder.AppendLine();
        builder.AppendLine("- v0.4 is expected to be closer to the reference than v0.3 by trading exaggerated geometry for restrained relief.");
        builder.AppendLine("- Remaining final-quality work, if needed, should focus on finer chipped silhouettes, better lamp hardware, and grime/decal layering.");
        File.WriteAllText(ProjectPath("Documentation/" + ReviewFileNameV04), builder.ToString());
    }

    private static void WriteReviewV05(string previewPath, string roomPath)
    {
        string metricsPath = ProjectPath("Renders/" + MetricsFileNameV05);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.5 Acceptance Review");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## Files");
        builder.AppendLine();
        builder.AppendLine("- Material preview: `" + previewPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Room render: `" + roomPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Metrics: `" + metricsPath.Replace('\\', '/') + "`");
        builder.AppendLine();
        builder.AppendLine("## What Changed From v0.4");
        builder.AppendLine();
        builder.AppendLine("- Replaced individual brick/block construction with continuous mapped surfaces.");
        builder.AppendLine("- Used procedural PBR maps and tiling to create small wall/ceiling brick and larger floor slabs.");
        builder.AppendLine("- Kept geometry for only corner soot, damp bands, lamps, and reflection helpers.");
        builder.AppendLine("- Rebalanced lighting toward localized amber lamps, cool back-wall readability, and damp floor glints.");
        builder.AppendLine();
        builder.AppendLine("## Acceptance Comparison");
        builder.AppendLine();
        builder.AppendLine("- Texture relief: should now read as continuous aged brick/stone rather than blockout geometry; still fails if surfaces look flat.");
        builder.AppendLine("- Wet reflection: should show warm floor glints without metallic orange material.");
        builder.AppendLine("- Warm wall light: should create localized amber halos while leaving the center/back room dark.");
        builder.AppendLine("- Ceiling/floor scale: wall and ceiling brick should be smaller than floor flagstones.");
        builder.AppendLine("- Corner depth: back corners should remain dark while the back wall remains readable.");
        builder.AppendLine();
        builder.AppendLine("## Blunt Assessment");
        builder.AppendLine();
        builder.AppendLine("- This is the most reference-aligned approach so far: material-driven masonry with restrained geometry.");
        builder.AppendLine("- If this still misses final quality, the remaining gains require purpose-built decals, better lamp models, and more realistic authored texture noise.");
        File.WriteAllText(ProjectPath("Documentation/" + ReviewFileNameV05), builder.ToString());
    }

    private static void WriteReviewV06(string previewPath, string roomPath)
    {
        string metricsPath = ProjectPath("Renders/" + MetricsFileNameV06);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Roomtest v0.6 Acceptance Review");
        builder.AppendLine();
        builder.AppendLine("Generated: " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
        builder.AppendLine();
        builder.AppendLine("## Files");
        builder.AppendLine();
        builder.AppendLine("- Material preview: `" + previewPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Room render: `" + roomPath.Replace('\\', '/') + "`");
        builder.AppendLine("- Metrics: `" + metricsPath.Replace('\\', '/') + "`");
        builder.AppendLine();
        builder.AppendLine("## What Changed From v0.5");
        builder.AppendLine();
        builder.AppendLine("- Added shallow per-brick/per-stone face meshes over dark mortar backing so the image gets real edge relief without returning to chunky blockout cubes.");
        builder.AppendLine("- Replaced cool readability fill with warmer amber/neutral bounce to move the floor and walls closer to the supplied reference.");
        builder.AppendLine("- Added a larger, brighter gaslight fixture with brass cage pieces, black iron backing, and localized bloom lights.");
        builder.AppendLine("- Added v0.6 texture families with darker mortar, warmer albedo, stronger normal relief, and more chipped/stained faces.");
        builder.AppendLine("- Added localized soot, damp base bands, and front/back wet shadow bands.");
        builder.AppendLine();
        builder.AppendLine("## Acceptance Comparison");
        builder.AppendLine();
        builder.AppendLine("- Texture relief: should read less like a printed grid and more like individual worn masonry.");
        builder.AppendLine("- Wet reflection: should remain glossy and warm-black without blue cast or orange metal response.");
        builder.AppendLine("- Warm wall light: should be brighter and more visible at the fixture while remaining localized.");
        builder.AppendLine("- Ceiling/floor scale: ceiling bricks remain small; floor stones are larger and flatter.");
        builder.AppendLine("- Corner depth: corners and the back wall should hold darkness while still showing brick courses.");
        builder.AppendLine();
        builder.AppendLine("## Blunt Assessment");
        builder.AppendLine();
        builder.AppendLine("- v0.6 is intended to close the biggest visible gap from v0.5: regular flat material grids.");
        builder.AppendLine("- Remaining misses after visual inspection should be handled by authored grime masks, less rectangular silhouette breakup, and more physically plausible lamp bloom.");
        File.WriteAllText(ProjectPath("Documentation/" + ReviewFileNameV06), builder.ToString());
    }

    private static Material Pick(Material[] materials, int col, int row, int seed)
    {
        int index = Mathf.Abs((int)(Hash01(col, row, seed + 997) * materials.Length * 0.999f));
        return materials[index % materials.Length];
    }

    private static Texture2D LoadTexture(string path)
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (texture == null)
        {
            throw new FileNotFoundException("Generated roomtest texture is missing", path);
        }

        return texture;
    }

    private static void SetTexture(Material material, string property, Texture texture, Vector2 scale)
    {
        if (!material.HasProperty(property))
        {
            return;
        }

        material.SetTexture(property, texture);
        material.SetTextureScale(property, scale);
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
        float frequency = 1f;
        float total = 0f;
        for (int i = 0; i < octaves; i++)
        {
            value += ValueNoise(x * frequency, y * frequency, seed + i * 29) * amplitude;
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

    private readonly struct PatternSample
    {
        public PatternSample(bool isMortar, float edgeDistance, float tone, float irregularity, float stain)
        {
            this.isMortar = isMortar;
            this.edgeDistance = edgeDistance;
            this.tone = tone;
            this.irregularity = irregularity;
            this.stain = stain;
        }

        public readonly bool isMortar;
        public readonly float edgeDistance;
        public readonly float tone;
        public readonly float irregularity;
        public readonly float stain;
    }
}
