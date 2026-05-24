using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class V0SceneBuilder
{
    private const string MainMenuScenePath = "Assets/_Project/Scenes/MainMenu.unity";
    private const string ScenePath = "Assets/_Project/Scenes/Level01.unity";
    private const string Level02ScenePath = "Assets/_Project/Scenes/Level02.unity";
    private const string Level03ScenePath = "Assets/_Project/Scenes/Level03.unity";
    private const string Level04ScenePath = "Assets/_Project/Scenes/Level04.unity";
    private const string Level05ScenePath = "Assets/_Project/Scenes/Level05.unity";
    private const string MaterialFolder = "Assets/_Project/Materials";
    private const string TextureFolder = "Assets/_Project/Textures";
    private const string DataFolder = "Assets/_Project/Data";
    private const string FinalMaterialsTextureFolder = "Assets/_Project/ArtStaging/FinalMaterialsV1/Textures";
    private const string SignageDecalsTextureFolder = "Assets/_Project/ArtStaging/SignageDecalsV1/Textures";
    private const string UIHudTextureFolder = "Assets/_Project/ArtStaging/UIHudV1";
    private const string AudioV1Folder = "Assets/_Project/ArtStaging/AudioV1";
    private const float SignageDecalsAtlasPixels = 2048f;
    private const string WindowsBuildFolder = "Builds/Windows";

    private enum ProceduralTextureKind
    {
        OilStone,
        RivetedIron,
        BrassPipe
    }

    private enum SignageDecalSheet
    {
        ObjectivePlates,
        WarningHazardStrips,
        RouteArrowsChevrons,
        StencilMachineryLore,
        SecretServiceMarks
    }

    [MenuItem("Project Tools/Rebuild v0.0 Scene")]
    public static void BuildV0()
    {
        EnsureFolders();
        ApplyAudioV1ImportSettings();

        Material wallMaterial = CreateMaterial("M_Greybox_SootBrickWall", new Color(0.34f, 0.26f, 0.2f));
        Material floorMaterial = CreateMaterial("M_Greybox_OilStoneFloor", new Color(0.12f, 0.1f, 0.08f));
        Material doorMaterial = CreateMaterial("M_Greybox_PressureGate", new Color(0.62f, 0.25f, 0.12f));
        Material keyMaterial = CreateMaterial("M_Greybox_GearKey", new Color(1f, 0.72f, 0.18f));
        Material exitMaterial = CreateMaterial("M_Greybox_ServiceLift", new Color(0.25f, 0.75f, 0.42f));
        Material enemyMaterial = CreateMaterial("M_Greybox_ClockworkEnemy", new Color(0.8f, 0.42f, 0.14f));
        Material enemyEyeMaterial = CreateMaterial("M_Greybox_FurnaceEyes", new Color(1f, 0.55f, 0.12f));
        Material healthMaterial = CreateMaterial("M_Greybox_Health", new Color(0.95f, 0.1f, 0.1f));
        Material ammoMaterial = CreateMaterial("M_Greybox_Ammo", new Color(0.84f, 0.54f, 0.18f));
        Material gunMaterial = CreateMaterial("M_Greybox_WalnutGrip", new Color(0.22f, 0.12f, 0.055f));
        Material gunTrimMaterial = CreateMaterial("M_Greybox_BrassTrim", new Color(0.86f, 0.58f, 0.24f));
        Material muzzleFlashMaterial = CreateMaterial("M_Greybox_MuzzleFlash", new Color(1f, 0.72f, 0.08f));
        Material brassGuideMaterial = CreateMaterial("M_Greybox_BrassGuide", new Color(0.85f, 0.56f, 0.22f));
        Material pressureWarningMaterial = CreateMaterial("M_Greybox_PressureWarning", new Color(0.9f, 0.18f, 0.06f));
        Material rivetedIronMaterial = CreateMaterial("M_Steam_RivetedIron", new Color(0.075f, 0.07f, 0.065f));
        Material oilStoneMaterial = CreateMaterial("M_Steam_OilDarkStone", new Color(0.065f, 0.055f, 0.045f));
        Material brassHazardMaterial = CreateMaterial("M_Steam_BrassHazard", new Color(0.95f, 0.54f, 0.08f));
        Material gaugeFaceMaterial = CreateMaterial("M_Steam_CreamGaugeFace", new Color(0.86f, 0.78f, 0.58f));
        Material steamPuffMaterial = CreateMaterial("M_Steam_SteamPuff", new Color(0.72f, 0.72f, 0.68f));
        Material furnaceGlowMaterial = CreateMaterial("M_Steam_FurnaceGlow", new Color(1f, 0.36f, 0.08f));
        Material glassVialMaterial = CreateMaterial("M_Steam_FrostedGlassVial", new Color(0.58f, 0.78f, 0.8f));
        Material medicinalFluidMaterial = CreateMaterial("M_Steam_RedMedicinalFluid", new Color(0.82f, 0.05f, 0.04f));
        ApplyFinalMaterialTextureSet(wallMaterial, "SootBrick", new Vector2(2.2f, 2.2f), 0f, 0.2f);
        ApplyFinalMaterialTextureSet(floorMaterial, "WetOilDarkStone", new Vector2(2.8f, 2.8f), 0f, 0.62f);
        ApplyFinalMaterialTextureSet(doorMaterial, "BlackenedRivetedIron", new Vector2(1.35f, 1.35f), 0.72f, 0.34f);
        ApplyFinalMaterialTextureSet(keyMaterial, "AgedBrass", new Vector2(1.2f, 1.2f), 0.85f, 0.48f);
        ApplyFinalMaterialTextureSet(exitMaterial, "AgedBrass", new Vector2(1.35f, 1.35f), 0.78f, 0.42f);
        ApplyFinalMaterialTextureSet(enemyMaterial, "AgedBrass", new Vector2(1.25f, 1.25f), 0.82f, 0.4f);
        ApplyFinalMaterialTextureSet(ammoMaterial, "CopperPipe", new Vector2(1.8f, 1.05f), 0.84f, 0.43f);
        ApplyFinalMaterialTextureSet(gunMaterial, "GreasyWalnut", new Vector2(1.2f, 1.2f), 0f, 0.34f);
        ApplyFinalMaterialTextureSet(gunTrimMaterial, "AgedBrass", new Vector2(1.1f, 1.1f), 0.85f, 0.48f);
        ApplyFinalMaterialTextureSet(brassGuideMaterial, "CopperPipe", new Vector2(1.8f, 1.05f), 0.84f, 0.43f);
        ApplyFinalMaterialTextureSet(pressureWarningMaterial, "HazardEnamel", new Vector2(1.35f, 1.35f), 0.18f, 0.34f);
        ApplyFinalMaterialTextureSet(rivetedIronMaterial, "BlackenedRivetedIron", new Vector2(1.45f, 1.45f), 0.72f, 0.34f);
        ApplyFinalMaterialTextureSet(oilStoneMaterial, "WetOilDarkStone", new Vector2(2.8f, 2.8f), 0f, 0.62f);
        ApplyFinalMaterialTextureSet(brassHazardMaterial, "AgedBrass", new Vector2(1.3f, 1.3f), 0.82f, 0.45f);
        ApplyFinalMaterialTextureSet(gaugeFaceMaterial, "CreamEnamelGauge", new Vector2(1f, 1f), 0f, 0.32f);
        ApplyFinalMaterialTextureSet(glassVialMaterial, "AmberGlass", new Vector2(1f, 1f), 0f, 0.7f);
        WeaponDefinition pressurePistolDefinition = CreatePressurePistolDefinition();
        WeaponDefinition steamScattergunDefinition = CreateSteamScattergunDefinition();
        EnemyDefinition scrapperDefinition = CreateScrapperDefinition();
        EnemyDefinition lancerDefinition = CreateLancerDefinition();
        EnemyDefinition bellowsNodeDefinition = CreateBellowsNodeDefinition();
        EnemyDefinition bulwarkDefinition = CreateBulwarkDefinition();
        EnemyDefinition governorWardenDefinition = CreateGovernorWardenDefinition();
        PickupDefinition healthPickupDefinition = CreateHealthPickupDefinition();
        PickupDefinition ammoPickupDefinition = CreateAmmoPickupDefinition();
        PickupDefinition gearKeyDefinition = CreateGearKeyDefinition();
        PickupDefinition steamScattergunPickupDefinition = CreateSteamScattergunPickupDefinition();
        PlatformQualityProfile windowsQualityProfile = CreatePlatformQualityProfiles();

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.55f);

        CreateLighting();
        CreateGreyboxLevel(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud, windowsQualityProfile, "Find the gear key. Open the pressure gate.");
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, glassVialMaterial, rivetedIronMaterial, pressureWarningMaterial, pressurePistolDefinition, steamScattergunDefinition);
        CreateEnemy("Enemy - First Room", new Vector3(0f, 1f, 16.5f), enemyMaterial, enemyEyeMaterial, brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, scrapperDefinition);
        CreateEnemy("Enemy - Key Room", new Vector3(14.5f, 1f, 17f), enemyMaterial, enemyEyeMaterial, brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, scrapperDefinition);
        CreateEnemy("Enemy - Final Left", new Vector3(-3.2f, 1f, 30.5f), enemyMaterial, enemyEyeMaterial, brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, scrapperDefinition);
        CreateEnemy("Enemy - Final Right", new Vector3(3.2f, 1f, 32.5f), enemyMaterial, enemyEyeMaterial, brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, scrapperDefinition);
        CreateHealthVialPickup("Pickup - Health Vial", new Vector3(-3.6f, 0.65f, 20f), healthMaterial, glassVialMaterial, medicinalFluidMaterial, brassGuideMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Pressure Cartridge Pack", new Vector3(4.2f, 0.55f, 19f), ammoMaterial, rivetedIronMaterial, brassGuideMaterial, ammoPickupDefinition);
        CreateGearKeyPickup("Pickup - Gear Key", new Vector3(16f, 0.55f, 17f), Vector3.one * 1.1f, keyMaterial, rivetedIronMaterial, gearKeyDefinition);
        CreateLockedDoor(doorMaterial, brassGuideMaterial, rivetedIronMaterial, gaugeFaceMaterial, glassVialMaterial, pressureWarningMaterial);
        CreateLevelTransitionLift(exitMaterial, rivetedIronMaterial, brassGuideMaterial, gaugeFaceMaterial, "Level02");
        CreateAccentLights();
        CreateObjectiveGuides(brassGuideMaterial, pressureWarningMaterial, keyMaterial, exitMaterial);
        CreateLevel01FlowPolish(brassGuideMaterial, pressureWarningMaterial, keyMaterial, exitMaterial, rivetedIronMaterial, gaugeFaceMaterial, furnaceGlowMaterial);
        CreateSteamworksDressing(rivetedIronMaterial, oilStoneMaterial, brassGuideMaterial, pressureWarningMaterial, brassHazardMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);
        CreateSecretCache(brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, healthMaterial, glassVialMaterial, medicinalFluidMaterial, ammoMaterial, healthPickupDefinition, ammoPickupDefinition);
        CreateLevel01SignageDecalsV1();

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ScenePath);
        CreatePipeworksAnnexScene(wallMaterial, floorMaterial, exitMaterial, enemyMaterial, enemyEyeMaterial, healthMaterial, ammoMaterial, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, brassGuideMaterial, pressureWarningMaterial, rivetedIronMaterial, oilStoneMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial, glassVialMaterial, medicinalFluidMaterial, pressurePistolDefinition, steamScattergunDefinition, scrapperDefinition, lancerDefinition, healthPickupDefinition, ammoPickupDefinition, windowsQualityProfile);
        CreateBoilerheartScene(wallMaterial, floorMaterial, exitMaterial, enemyMaterial, enemyEyeMaterial, healthMaterial, ammoMaterial, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, brassGuideMaterial, pressureWarningMaterial, rivetedIronMaterial, oilStoneMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial, glassVialMaterial, medicinalFluidMaterial, pressurePistolDefinition, steamScattergunDefinition, scrapperDefinition, bellowsNodeDefinition, healthPickupDefinition, ammoPickupDefinition, steamScattergunPickupDefinition, windowsQualityProfile);
        CreateFurnaceFoundryScene(wallMaterial, floorMaterial, exitMaterial, enemyMaterial, enemyEyeMaterial, healthMaterial, ammoMaterial, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, brassGuideMaterial, pressureWarningMaterial, rivetedIronMaterial, oilStoneMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial, glassVialMaterial, medicinalFluidMaterial, pressurePistolDefinition, steamScattergunDefinition, scrapperDefinition, lancerDefinition, bulwarkDefinition, healthPickupDefinition, ammoPickupDefinition, windowsQualityProfile);
        CreateGovernorCoreScene(wallMaterial, floorMaterial, exitMaterial, enemyMaterial, enemyEyeMaterial, healthMaterial, ammoMaterial, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, brassGuideMaterial, pressureWarningMaterial, rivetedIronMaterial, oilStoneMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial, glassVialMaterial, medicinalFluidMaterial, pressurePistolDefinition, steamScattergunDefinition, scrapperDefinition, lancerDefinition, bulwarkDefinition, governorWardenDefinition, healthPickupDefinition, ammoPickupDefinition, windowsQualityProfile);
        CreateMainMenuScene(brassGuideMaterial, rivetedIronMaterial, gaugeFaceMaterial, furnaceGlowMaterial, oilStoneMaterial, windowsQualityProfile);
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(MainMenuScenePath, true),
            new EditorBuildSettingsScene(ScenePath, true),
            new EditorBuildSettingsScene(Level02ScenePath, true),
            new EditorBuildSettingsScene(Level03ScenePath, true),
            new EditorBuildSettingsScene(Level04ScenePath, true),
            new EditorBuildSettingsScene(Level05ScenePath, true)
        };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("V0 scenes rebuilt at " + MainMenuScenePath + ", " + ScenePath + ", " + Level02ScenePath + ", " + Level03ScenePath + ", " + Level04ScenePath + ", and " + Level05ScenePath);
    }

    public static void RunSmokeTest()
    {
        if (!File.Exists(MainMenuScenePath))
        {
            throw new FileNotFoundException("Missing main menu scene", MainMenuScenePath);
        }

        if (!File.Exists(ScenePath))
        {
            throw new FileNotFoundException("Missing v0 scene", ScenePath);
        }

        if (!File.Exists(Level02ScenePath))
        {
            throw new FileNotFoundException("Missing level 02 scene", Level02ScenePath);
        }

        if (!File.Exists(Level03ScenePath))
        {
            throw new FileNotFoundException("Missing level 03 scene", Level03ScenePath);
        }

        if (!File.Exists(Level04ScenePath))
        {
            throw new FileNotFoundException("Missing level 04 scene", Level04ScenePath);
        }

        if (!File.Exists(Level05ScenePath))
        {
            throw new FileNotFoundException("Missing level 05 scene", Level05ScenePath);
        }

        EditorSceneManager.OpenScene(ScenePath);

        RequireObject<PlayerController>("PlayerController");
        RequireObject<PlayerHealth>("PlayerHealth");
        RequireObject<PlayerInventory>("PlayerInventory");
        RequireObject<WeaponController>("WeaponController");
        RequireObject<GameStateController>("GameStateController");
        RequireObject<PauseMenuController>("PauseMenuController");
        RequireObject<SteamworksAudio>("SteamworksAudio");
        RequireObject<RuntimePerformanceProfile>("RuntimePerformanceProfile");
        RequireObject<RuntimeAutoPlaythroughTest>("RuntimeAutoPlaythroughTest");
        RequireObject<RuntimeCombatTest>("RuntimeCombatTest");
        RequireObject<RuntimeCombatEdgeTest>("RuntimeCombatEdgeTest");
        RequireObject<RuntimeRangedCombatTest>("RuntimeRangedCombatTest");
        RequireObject<RuntimeBellowsNodeTest>("RuntimeBellowsNodeTest");
        RequireObject<RuntimeBulwarkCombatTest>("RuntimeBulwarkCombatTest");
        RequireObject<RuntimeWardenCombatTest>("RuntimeWardenCombatTest");
        RequireObject<RuntimeHazardTest>("RuntimeHazardTest");
        RequireObject<RuntimeSecretTest>("RuntimeSecretTest");
        RequireObject<RuntimeWeaponSwitchTest>("RuntimeWeaponSwitchTest");
        RequireObject<RuntimePauseFlowTest>("RuntimePauseFlowTest");
        RequireObject<RuntimeMovementFeelTest>("RuntimeMovementFeelTest");
        RequireObject<RuntimeBalanceEnvelopeTest>("RuntimeBalanceEnvelopeTest");
        RequireObject<RuntimeAudioMixTest>("RuntimeAudioMixTest");
        RequireObject<HUDController>("HUDController");
        RequireObject<EnemyController>("EnemyController");
        RequireObject<Pickup>("Pickup");
        RequireObject<LockedDoor>("LockedDoor");
        RequireObject<LevelTransitionTrigger>("LevelTransitionTrigger");
        RequireObject<SecretArea>("SecretArea");

        EditorSceneManager.OpenScene(Level02ScenePath);
        RequireObject<PlayerController>("Level02 PlayerController");
        RequireObject<GameStateController>("Level02 GameStateController");
        RequireObject<EnemyController>("Level02 EnemyController");
        RequireObject<RangedEnemyController>("Level02 RangedEnemyController");
        RequireObject<LevelTransitionTrigger>("Level02 LevelTransitionTrigger");

        EditorSceneManager.OpenScene(Level03ScenePath);
        RequireObject<PlayerController>("Level03 PlayerController");
        RequireObject<GameStateController>("Level03 GameStateController");
        RequireObject<EnemyController>("Level03 EnemyController");
        RequireObject<BellowsNodeController>("Level03 BellowsNodeController");
        RequireObject<LevelTransitionTrigger>("Level03 LevelTransitionTrigger");
        RequireObject<SteamHazard>("Level03 SteamHazard");

        EditorSceneManager.OpenScene(Level04ScenePath);
        RequireObject<PlayerController>("Level04 PlayerController");
        RequireObject<GameStateController>("Level04 GameStateController");
        RequireObject<EnemyController>("Level04 EnemyController");
        RequireObject<RangedEnemyController>("Level04 RangedEnemyController");
        RequireObject<BulwarkEnemyController>("Level04 BulwarkEnemyController");
        RequireObject<LevelTransitionTrigger>("Level04 LevelTransitionTrigger");
        RequireObject<SteamHazard>("Level04 SteamHazard");
        RequireObject<FurnaceHeatHazard>("Level04 FurnaceHeatHazard");

        EditorSceneManager.OpenScene(Level05ScenePath);
        RequireObject<PlayerController>("Level05 PlayerController");
        RequireObject<GameStateController>("Level05 GameStateController");
        RequireObject<EnemyController>("Level05 EnemyController");
        RequireObject<RangedEnemyController>("Level05 RangedEnemyController");
        RequireObject<BulwarkEnemyController>("Level05 BulwarkEnemyController");
        RequireObject<GovernorWardenController>("Level05 GovernorWardenController");
        RequireObject<GuardianDefeatObjective>("Level05 GuardianDefeatObjective");
        RequireObject<ExitTrigger>("Level05 ExitTrigger");
        RequireObject<SteamHazard>("Level05 SteamHazard");
        RequireObject<FurnaceHeatHazard>("Level05 FurnaceHeatHazard");

        EditorSceneManager.OpenScene(MainMenuScenePath);
        RequireObject<MainMenuController>("MainMenuController");
        RequireObject<RuntimePerformanceProfile>("MainMenu RuntimePerformanceProfile");

        V0LevelValidator.ValidateProjectScenes();

        if (EditorBuildSettings.scenes.Length < 6 || EditorBuildSettings.scenes[0].path != MainMenuScenePath || EditorBuildSettings.scenes[1].path != ScenePath || EditorBuildSettings.scenes[2].path != Level02ScenePath || EditorBuildSettings.scenes[3].path != Level03ScenePath || EditorBuildSettings.scenes[4].path != Level04ScenePath || EditorBuildSettings.scenes[5].path != Level05ScenePath)
        {
            throw new InvalidOperationException("MainMenu, Level01, Level02, Level03, Level04, and Level05 are not the first enabled build scenes.");
        }

        Debug.Log("V0_SMOKE_TEST_PASS");
    }

    public static void BuildWindowsV0()
    {
        RunSmokeTest();

        string buildDirectory = Path.Combine(Directory.GetCurrentDirectory(), WindowsBuildFolder, GameBranding.BuildVersion);
        Directory.CreateDirectory(buildDirectory);

        string executablePath = Path.Combine(buildDirectory, GameBranding.ExecutableStem + "_" + GameBranding.BuildVersion + ".exe");

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { MainMenuScenePath, ScenePath, Level02ScenePath, Level03ScenePath, Level04ScenePath, Level05ScenePath },
            locationPathName = executablePath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new InvalidOperationException($"Windows build failed: {report.summary.result}");
        }

        Debug.Log("V0_WINDOWS_BUILD_PASS " + executablePath);
    }

    private static void EnsureFolders()
    {
        string[] folders =
        {
            "Assets/_Project",
            "Assets/_Project/Scenes",
            "Assets/_Project/Scripts",
            "Assets/_Project/Prefabs",
            MaterialFolder,
            TextureFolder,
            DataFolder
        };

        foreach (string folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                string parent = Path.GetDirectoryName(folder)?.Replace("\\", "/");
                string name = Path.GetFileName(folder);
                AssetDatabase.CreateFolder(parent, name);
            }
        }
    }

    private static Material CreateMaterial(string name, Color color)
    {
        string path = $"{MaterialFolder}/{name}.mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Lit");
            }

            material = new Material(shader);
            AssetDatabase.CreateAsset(material, path);
        }

        if (material.HasProperty("_Color"))
        {
            material.color = color;
        }
        else if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }

        EditorUtility.SetDirty(material);
        return material;
    }

    private static void ApplyFinalMaterialTextureSet(Material material, string familyName, Vector2 tiling, float metallic, float smoothness)
    {
        Texture2D baseColor = LoadFinalMaterialTexture(familyName, "BaseColor", TextureImporterType.Default, true);
        Texture2D normal = LoadFinalMaterialTexture(familyName, "Normal", TextureImporterType.NormalMap, false);
        Texture2D orm = LoadFinalMaterialTexture(familyName, "ORM", TextureImporterType.Default, false);

        SetMaterialTexture(material, "_MainTex", baseColor, tiling);
        SetMaterialTexture(material, "_BaseMap", baseColor, tiling);
        SetMaterialTexture(material, "_BumpMap", normal, tiling);
        SetMaterialTexture(material, "_OcclusionMap", orm, tiling);

        if (material.HasProperty("_Metallic"))
        {
            material.SetFloat("_Metallic", metallic);
        }

        if (material.HasProperty("_Glossiness"))
        {
            material.SetFloat("_Glossiness", smoothness);
        }

        if (material.HasProperty("_Smoothness"))
        {
            material.SetFloat("_Smoothness", smoothness);
        }

        if (material.HasProperty("_BumpScale"))
        {
            material.SetFloat("_BumpScale", 0.92f);
        }

        material.mainTexture = baseColor;
        material.mainTextureScale = tiling;
        material.EnableKeyword("_NORMALMAP");
        material.EnableKeyword("_OCCLUSIONMAP");
        EditorUtility.SetDirty(material);
    }

    private static Texture2D LoadFinalMaterialTexture(string familyName, string suffix, TextureImporterType textureType, bool sRgb)
    {
        string path = $"{FinalMaterialsTextureFolder}/T_BBW_{familyName}_{suffix}_2048.png";
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (texture == null)
        {
            throw new FileNotFoundException("Missing FinalMaterialsV1 texture", path);
        }

        ConfigureFinalMaterialTextureImporter(path, textureType, sRgb);
        return texture;
    }

    private static void ConfigureFinalMaterialTextureImporter(string path, TextureImporterType textureType, bool sRgb)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
        {
            return;
        }

        bool changed = false;
        if (importer.textureType != textureType)
        {
            importer.textureType = textureType;
            changed = true;
        }

        if (textureType == TextureImporterType.Default && importer.sRGBTexture != sRgb)
        {
            importer.sRGBTexture = sRgb;
            changed = true;
        }

        if (importer.mipmapEnabled == false)
        {
            importer.mipmapEnabled = true;
            changed = true;
        }

        if (importer.wrapMode != TextureWrapMode.Repeat)
        {
            importer.wrapMode = TextureWrapMode.Repeat;
            changed = true;
        }

        if (importer.maxTextureSize != 2048)
        {
            importer.maxTextureSize = 2048;
            changed = true;
        }

        if (changed)
        {
            importer.SaveAndReimport();
        }
    }

    private static void SetMaterialTexture(Material material, string propertyName, Texture texture, Vector2 tiling)
    {
        if (!material.HasProperty(propertyName))
        {
            return;
        }

        material.SetTexture(propertyName, texture);
        material.SetTextureScale(propertyName, tiling);
    }

    private static WeaponDefinition CreatePressurePistolDefinition()
    {
        string path = $"{DataFolder}/PressurePistolDefinition.asset";
        WeaponDefinition definition = AssetDatabase.LoadAssetAtPath<WeaponDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<WeaponDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Pressure Pistol";
        definition.weaponId = WeaponController.PressurePistolId;
        definition.damage = GameBalance.PressurePistolDamage;
        definition.ammoCost = GameBalance.PressurePistolAmmoCost;
        definition.pelletCount = GameBalance.PressurePistolPelletCount;
        definition.fireCooldown = GameBalance.PressurePistolCooldown;
        definition.range = GameBalance.PressurePistolRange;
        definition.spread = GameBalance.PressurePistolSpread;
        definition.secondaryDamage = GameBalance.PressureBurstDamage;
        definition.secondaryPelletCount = GameBalance.PressureBurstPelletCount;
        definition.secondaryAmmoCost = GameBalance.PressureBurstAmmoCost;
        definition.secondaryCooldown = GameBalance.PressureBurstCooldown;
        definition.secondaryRange = GameBalance.PressureBurstRange;
        definition.secondarySpread = GameBalance.PressureBurstSpread;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static WeaponDefinition CreateSteamScattergunDefinition()
    {
        string path = $"{DataFolder}/SteamScattergunDefinition.asset";
        WeaponDefinition definition = AssetDatabase.LoadAssetAtPath<WeaponDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<WeaponDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Steam Scattergun";
        definition.weaponId = WeaponController.SteamScattergunId;
        definition.damage = GameBalance.SteamScattergunDamage;
        definition.ammoCost = GameBalance.SteamScattergunAmmoCost;
        definition.pelletCount = GameBalance.SteamScattergunPelletCount;
        definition.fireCooldown = GameBalance.SteamScattergunCooldown;
        definition.range = GameBalance.SteamScattergunRange;
        definition.spread = GameBalance.SteamScattergunSpread;
        definition.secondaryDamage = GameBalance.SteamScattergunSlugDamage;
        definition.secondaryPelletCount = GameBalance.SteamScattergunSlugPelletCount;
        definition.secondaryAmmoCost = GameBalance.SteamScattergunSlugAmmoCost;
        definition.secondaryCooldown = GameBalance.SteamScattergunSlugCooldown;
        definition.secondaryRange = GameBalance.SteamScattergunSlugRange;
        definition.secondarySpread = GameBalance.SteamScattergunSlugSpread;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static EnemyDefinition CreateScrapperDefinition()
    {
        string path = $"{DataFolder}/ScrapperDefinition.asset";
        EnemyDefinition definition = AssetDatabase.LoadAssetAtPath<EnemyDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<EnemyDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Scrapper";
        definition.attackStyle = EnemyAttackStyle.Melee;
        definition.maxHealth = GameBalance.ScrapperHealth;
        definition.detectionRange = GameBalance.ScrapperDetectionRange;
        definition.moveSpeed = GameBalance.ScrapperMoveSpeed;
        definition.attackRange = GameBalance.ScrapperAttackRange;
        definition.attackDamage = GameBalance.ScrapperAttackDamage;
        definition.attackCooldown = GameBalance.ScrapperAttackCooldown;
        definition.attackWindup = GameBalance.ScrapperAttackWindup;
        definition.obstacleProbeDistance = GameBalance.ScrapperObstacleProbeDistance;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static EnemyDefinition CreateLancerDefinition()
    {
        string path = $"{DataFolder}/LancerDefinition.asset";
        EnemyDefinition definition = AssetDatabase.LoadAssetAtPath<EnemyDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<EnemyDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Lancer";
        definition.attackStyle = EnemyAttackStyle.Ranged;
        definition.maxHealth = GameBalance.LancerHealth;
        definition.detectionRange = GameBalance.LancerDetectionRange;
        definition.moveSpeed = GameBalance.LancerMoveSpeed;
        definition.fireRange = GameBalance.LancerFireRange;
        definition.fireCooldown = GameBalance.LancerFireCooldown;
        definition.fireWindup = GameBalance.LancerFireWindup;
        definition.projectileDamage = GameBalance.LancerProjectileDamage;
        definition.projectileSpeed = GameBalance.LancerProjectileSpeed;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static EnemyDefinition CreateBellowsNodeDefinition()
    {
        string path = $"{DataFolder}/BellowsNodeDefinition.asset";
        EnemyDefinition definition = AssetDatabase.LoadAssetAtPath<EnemyDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<EnemyDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Bellows Node";
        definition.attackStyle = EnemyAttackStyle.Support;
        definition.maxHealth = GameBalance.BellowsNodeHealth;
        definition.detectionRange = GameBalance.BellowsNodeDetectionRange;
        definition.moveSpeed = 0f;
        definition.attackRange = GameBalance.BellowsNodePulseRange;
        definition.attackDamage = GameBalance.BellowsNodePulseDamage;
        definition.attackCooldown = GameBalance.BellowsNodePulseCooldown;
        definition.attackWindup = GameBalance.BellowsNodePulseWindup;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static EnemyDefinition CreateBulwarkDefinition()
    {
        string path = $"{DataFolder}/BulwarkDefinition.asset";
        EnemyDefinition definition = AssetDatabase.LoadAssetAtPath<EnemyDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<EnemyDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Bulwark";
        definition.attackStyle = EnemyAttackStyle.Heavy;
        definition.maxHealth = GameBalance.BulwarkHealth;
        definition.detectionRange = GameBalance.BulwarkDetectionRange;
        definition.moveSpeed = GameBalance.BulwarkMoveSpeed;
        definition.attackRange = GameBalance.BulwarkAttackRange;
        definition.attackDamage = GameBalance.BulwarkAttackDamage;
        definition.attackCooldown = GameBalance.BulwarkAttackCooldown;
        definition.attackWindup = GameBalance.BulwarkAttackWindup;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static EnemyDefinition CreateGovernorWardenDefinition()
    {
        string path = $"{DataFolder}/GovernorWardenDefinition.asset";
        EnemyDefinition definition = AssetDatabase.LoadAssetAtPath<EnemyDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<EnemyDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Governor Warden";
        definition.attackStyle = EnemyAttackStyle.Boss;
        definition.maxHealth = GameBalance.GovernorWardenHealth;
        definition.detectionRange = GameBalance.GovernorWardenDetectionRange;
        definition.moveSpeed = GameBalance.GovernorWardenMoveSpeed;
        definition.attackRange = GameBalance.GovernorWardenStompRange;
        definition.attackDamage = GameBalance.GovernorWardenStompDamage;
        definition.attackCooldown = GameBalance.GovernorWardenStompCooldown;
        definition.attackWindup = GameBalance.GovernorWardenStompWindup;
        definition.fireRange = GameBalance.GovernorWardenFireRange;
        definition.fireCooldown = GameBalance.GovernorWardenFireCooldown;
        definition.fireWindup = GameBalance.GovernorWardenFireWindup;
        definition.projectileDamage = GameBalance.GovernorWardenProjectileDamage;
        definition.projectileSpeed = GameBalance.GovernorWardenProjectileSpeed;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static PickupDefinition CreateHealthPickupDefinition()
    {
        string path = $"{DataFolder}/HealthVialDefinition.asset";
        PickupDefinition definition = AssetDatabase.LoadAssetAtPath<PickupDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<PickupDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Health Vial";
        definition.kind = PickupKind.Health;
        definition.amount = GameBalance.HealthPickupAmount;
        definition.collectRadius = 0.9f;
        definition.spinDegreesPerSecond = 82f;
        definition.bobAmplitude = 0.1f;
        definition.bobSpeed = 3f;
        definition.audioCue = SteamworksAudioCue.HealthPickup;
        definition.collectMessage = "+" + GameBalance.HealthPickupAmount + " health";
        definition.weaponUnlockId = string.Empty;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static PickupDefinition CreateAmmoPickupDefinition()
    {
        string path = $"{DataFolder}/PressureCartridgeDefinition.asset";
        PickupDefinition definition = AssetDatabase.LoadAssetAtPath<PickupDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<PickupDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Pressure Cartridge Pack";
        definition.kind = PickupKind.Ammo;
        definition.amount = GameBalance.AmmoPickupAmount;
        definition.collectRadius = 0.9f;
        definition.spinDegreesPerSecond = 74f;
        definition.bobAmplitude = 0.1f;
        definition.bobSpeed = 2.8f;
        definition.audioCue = SteamworksAudioCue.AmmoPickup;
        definition.collectMessage = "+" + GameBalance.AmmoPickupAmount + " ammo";
        definition.weaponUnlockId = string.Empty;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static PickupDefinition CreateGearKeyDefinition()
    {
        string path = $"{DataFolder}/GearKeyDefinition.asset";
        PickupDefinition definition = AssetDatabase.LoadAssetAtPath<PickupDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<PickupDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Clockwork Gear Key";
        definition.kind = PickupKind.Key;
        definition.amount = 0;
        definition.collectRadius = 1f;
        definition.spinDegreesPerSecond = 64f;
        definition.bobAmplitude = 0.12f;
        definition.bobSpeed = 2.6f;
        definition.audioCue = SteamworksAudioCue.GearKey;
        definition.collectMessage = "Gear key acquired";
        definition.weaponUnlockId = string.Empty;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static PickupDefinition CreateSteamScattergunPickupDefinition()
    {
        string path = $"{DataFolder}/SteamScattergunPickupDefinition.asset";
        PickupDefinition definition = AssetDatabase.LoadAssetAtPath<PickupDefinition>(path);
        if (definition == null)
        {
            definition = ScriptableObject.CreateInstance<PickupDefinition>();
            AssetDatabase.CreateAsset(definition, path);
        }

        definition.displayName = "Steam Scattergun";
        definition.kind = PickupKind.Weapon;
        definition.amount = 0;
        definition.collectRadius = 1.15f;
        definition.spinDegreesPerSecond = 44f;
        definition.bobAmplitude = 0.08f;
        definition.bobSpeed = 2.3f;
        definition.audioCue = SteamworksAudioCue.WeaponPickup;
        definition.collectMessage = "Steam Scattergun acquired";
        definition.weaponUnlockId = WeaponController.SteamScattergunId;
        EditorUtility.SetDirty(definition);
        return definition;
    }

    private static PlatformQualityProfile CreatePlatformQualityProfiles()
    {
        PlatformQualityProfile windows = CreatePlatformQualityProfile(
            "WindowsMidLowQualityProfile",
            PlatformQualityTarget.WindowsMidLow,
            RuntimePerformanceProfile.WindowsTargetFrameRate,
            RuntimePerformanceProfile.WindowsVSyncCount,
            RuntimePerformanceProfile.WindowsPixelLightCount,
            RuntimePerformanceProfile.WindowsAntiAliasing,
            RuntimePerformanceProfile.WindowsShadowDistance,
            RuntimePerformanceProfile.WindowsLodBias,
            1024,
            3,
            650,
            "Primary Windows target for mid-to-low gaming PCs.");

        CreatePlatformQualityProfile(
            "AndroidPhoneQualityProfile",
            PlatformQualityTarget.AndroidPhone,
            30,
            0,
            1,
            0,
            16f,
            0.65f,
            512,
            1,
            250,
            "Future Android phone profile: reduced textures, lights, shadows, and download size.");

        CreatePlatformQualityProfile(
            "WebGLBrowserQualityProfile",
            PlatformQualityTarget.WebGLBrowser,
            30,
            0,
            1,
            0,
            12f,
            0.6f,
            512,
            1,
            180,
            "Future browser/WebGL profile: small download, low memory, simple materials.");

        CreatePlatformQualityProfile(
            "PcVrQualityProfile",
            PlatformQualityTarget.PcVr,
            90,
            0,
            2,
            0,
            24f,
            0.8f,
            1024,
            2,
            750,
            "Future Steam/OpenXR PC VR profile: high frame rate with restrained realtime lighting.");

        CreatePlatformQualityProfile(
            "MetaQuestQualityProfile",
            PlatformQualityTarget.MetaQuest,
            72,
            0,
            1,
            0,
            12f,
            0.55f,
            512,
            1,
            300,
            "Future Meta Quest standalone profile: Android-like budget with VR frame-rate pressure.");

        return windows;
    }

    private static PlatformQualityProfile CreatePlatformQualityProfile(string assetName, PlatformQualityTarget target, int targetFrameRate, int vSyncCount, int pixelLightCount, int antiAliasing, float shadowDistance, float lodBias, int maxTextureSize, int maxDynamicLights, int targetBuildSizeMegabytes, string notes)
    {
        string path = $"{DataFolder}/{assetName}.asset";
        PlatformQualityProfile profile = AssetDatabase.LoadAssetAtPath<PlatformQualityProfile>(path);
        if (profile == null)
        {
            profile = ScriptableObject.CreateInstance<PlatformQualityProfile>();
            AssetDatabase.CreateAsset(profile, path);
        }

        profile.target = target;
        profile.targetFrameRate = targetFrameRate;
        profile.vSyncCount = vSyncCount;
        profile.pixelLightCount = pixelLightCount;
        profile.antiAliasing = antiAliasing;
        profile.shadowDistance = shadowDistance;
        profile.lodBias = lodBias;
        profile.realtimeReflectionProbes = false;
        profile.softParticles = false;
        profile.allowCameraMsaa = false;
        profile.allowDynamicResolution = false;
        profile.maxTextureSize = maxTextureSize;
        profile.maxDynamicLights = maxDynamicLights;
        profile.targetBuildSizeMegabytes = targetBuildSizeMegabytes;
        profile.notes = notes;
        EditorUtility.SetDirty(profile);
        return profile;
    }

    private static void ApplyProceduralTexture(Material material, string textureName, ProceduralTextureKind kind)
    {
        Texture2D texture = CreateProceduralTexture(textureName, kind);
        if (material.HasProperty("_MainTex"))
        {
            material.SetTexture("_MainTex", texture);
        }

        if (material.HasProperty("_BaseMap"))
        {
            material.SetTexture("_BaseMap", texture);
        }

        material.mainTextureScale = kind == ProceduralTextureKind.BrassPipe ? new Vector2(1.5f, 1f) : new Vector2(2f, 2f);
        EditorUtility.SetDirty(material);
    }

    private static Texture2D CreateProceduralTexture(string name, ProceduralTextureKind kind)
    {
        const int size = 128;
        string path = $"{TextureFolder}/{name}.asset";
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (texture == null || texture.width != size || texture.height != size)
        {
            if (texture != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            texture = new Texture2D(size, size, TextureFormat.RGBA32, true);
            AssetDatabase.CreateAsset(texture, path);
        }

        texture.name = name;
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.filterMode = FilterMode.Bilinear;

        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                pixels[y * size + x] = SampleProceduralTexture(kind, x, y, size);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(true, false);
        EditorUtility.SetDirty(texture);
        return texture;
    }

    private static Color SampleProceduralTexture(ProceduralTextureKind kind, int x, int y, int size)
    {
        float noise = Mathf.PerlinNoise(x * 0.085f + 13.1f, y * 0.085f + 7.7f);
        switch (kind)
        {
            case ProceduralTextureKind.OilStone:
                return SampleOilStone(x, y, noise);
            case ProceduralTextureKind.RivetedIron:
                return SampleRivetedIron(x, y, noise);
            case ProceduralTextureKind.BrassPipe:
                return SampleBrassPipe(x, y, size, noise);
            default:
                return Color.magenta;
        }
    }

    private static Color SampleOilStone(int x, int y, float noise)
    {
        bool seam = x % 32 == 0 || y % 32 == 0;
        float oil = Mathf.PerlinNoise(x * 0.19f, y * 0.21f);
        Color baseColor = Color.Lerp(new Color(0.045f, 0.04f, 0.035f), new Color(0.14f, 0.12f, 0.1f), noise);
        if (oil > 0.68f)
        {
            baseColor = Color.Lerp(baseColor, new Color(0.018f, 0.017f, 0.014f), 0.42f);
        }

        return seam ? Color.Lerp(baseColor, Color.black, 0.38f) : baseColor;
    }

    private static Color SampleRivetedIron(int x, int y, float noise)
    {
        bool panelSeam = x % 48 == 0 || y % 48 == 0;
        int localX = x % 48;
        int localY = y % 48;
        float rivetA = Vector2.Distance(new Vector2(localX, localY), new Vector2(8f, 8f));
        float rivetB = Vector2.Distance(new Vector2(localX, localY), new Vector2(40f, 40f));
        bool rivet = rivetA < 3.8f || rivetB < 3.8f;
        Color baseColor = Color.Lerp(new Color(0.055f, 0.052f, 0.048f), new Color(0.14f, 0.135f, 0.125f), noise);
        if (panelSeam)
        {
            baseColor = Color.Lerp(baseColor, Color.black, 0.45f);
        }

        if (rivet)
        {
            baseColor = Color.Lerp(baseColor, new Color(0.34f, 0.29f, 0.22f), 0.7f);
        }

        return baseColor;
    }

    private static Color SampleBrassPipe(int x, int y, int size, float noise)
    {
        float verticalHighlight = Mathf.Abs(Mathf.Sin((x / (float)size) * Mathf.PI));
        bool ring = y % 28 < 3;
        Color baseColor = Color.Lerp(new Color(0.48f, 0.25f, 0.08f), new Color(0.95f, 0.66f, 0.24f), verticalHighlight * 0.55f + noise * 0.25f);
        if (ring)
        {
            baseColor = Color.Lerp(baseColor, new Color(0.18f, 0.11f, 0.05f), 0.45f);
        }

        return baseColor;
    }

    private static void CreateLighting()
    {
        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
        lightObject.transform.rotation = Quaternion.Euler(50f, -35f, 0f);
    }

    private static void CreateAccentLights()
    {
        CreatePointLight("Door Red Light", new Vector3(0f, 2.4f, 21.4f), new Color(1f, 0.08f, 0.05f), 2.4f, 5f);
        CreatePointLight("Key Yellow Light", new Vector3(16f, 2.2f, 17f), new Color(1f, 0.82f, 0.08f), 2.2f, 5f);
        CreatePointLight("Exit Green Light", new Vector3(0f, 2.4f, 34.2f), new Color(0.1f, 1f, 0.3f), 3.2f, 7f);
    }

    private static void CreatePointLight(string name, Vector3 position, Color color, float intensity, float range)
    {
        GameObject lightObject = new GameObject(name);
        lightObject.transform.position = position;

        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
    }

    private static void CreateGreyboxLevel(Material wallMaterial, Material floorMaterial)
    {
        GameObject parent = new GameObject("Brassworks Intake Blockout");

        CreateCube("Floor", new Vector3(5f, -0.1f, 16f), new Vector3(36f, 0.2f, 46f), floorMaterial, parent.transform);

        // Start room.
        Wall("Start South Wall", 0f, -4.25f, 8.5f, true, wallMaterial, parent.transform);
        Wall("Start West Wall", -4.25f, 0f, 8.5f, false, wallMaterial, parent.transform);
        Wall("Start East Wall", 4.25f, 0f, 8.5f, false, wallMaterial, parent.transform);
        Wall("Start North Left", -2.9f, 4.25f, 2.7f, true, wallMaterial, parent.transform);
        Wall("Start North Right", 2.9f, 4.25f, 2.7f, true, wallMaterial, parent.transform);

        // Corridor to first room.
        Wall("Corridor West", -1.75f, 8f, 8f, false, wallMaterial, parent.transform);
        Wall("Corridor East", 1.75f, 8f, 8f, false, wallMaterial, parent.transform);

        // First combat / hub room.
        Wall("Fight South Left", -4f, 11.75f, 4f, true, wallMaterial, parent.transform);
        Wall("Fight South Right", 4f, 11.75f, 4f, true, wallMaterial, parent.transform);
        Wall("Fight West", -6.25f, 17f, 10.5f, false, wallMaterial, parent.transform);
        Wall("Fight East Lower", 6.25f, 14f, 4f, false, wallMaterial, parent.transform);
        Wall("Fight East Upper", 6.25f, 20f, 4f, false, wallMaterial, parent.transform);
        Wall("Fight North Left", -4f, 22.25f, 4.5f, true, wallMaterial, parent.transform);
        Wall("Fight North Right", 4f, 22.25f, 4.5f, true, wallMaterial, parent.transform);

        // Side path and key room.
        Wall("Key Corridor North", 8f, 18.25f, 4f, true, wallMaterial, parent.transform);
        Wall("Key Corridor South", 8f, 15.75f, 4f, true, wallMaterial, parent.transform);
        Wall("Key Room West Lower", 9.75f, 15f, 2f, false, wallMaterial, parent.transform);
        Wall("Key Room West Upper", 9.75f, 19f, 2f, false, wallMaterial, parent.transform);
        Wall("Key Room East", 18.25f, 17f, 6.5f, false, wallMaterial, parent.transform);
        Wall("Key Room South", 14f, 13.75f, 8.5f, true, wallMaterial, parent.transform);
        Wall("Key Room North", 14f, 20.25f, 8.5f, true, wallMaterial, parent.transform);

        // Locked door corridor.
        Wall("Door Corridor West", -1.75f, 24f, 4f, false, wallMaterial, parent.transform);
        Wall("Door Corridor East", 1.75f, 24f, 4f, false, wallMaterial, parent.transform);

        // Final room.
        Wall("Final South Left", -4.3f, 25.75f, 5.4f, true, wallMaterial, parent.transform);
        Wall("Final South Right", 4.3f, 25.75f, 5.4f, true, wallMaterial, parent.transform);
        Wall("Final West", -7.25f, 31f, 10.5f, false, wallMaterial, parent.transform);
        Wall("Final East", 7.25f, 31f, 10.5f, false, wallMaterial, parent.transform);
        Wall("Final North", 0f, 36.25f, 14.5f, true, wallMaterial, parent.transform);

        CreateIntakeCombatCover(wallMaterial, parent.transform);
    }

    private static void CreateIntakeCombatCover(Material material, Transform parent)
    {
        CreateCube("Repair Bay Cover Boiler Left", new Vector3(-3.2f, 0.62f, 15.2f), new Vector3(1.25f, 1.24f, 1.45f), material, parent);
        CreateCube("Repair Bay Cover Crate Right", new Vector3(3.05f, 0.48f, 18.7f), new Vector3(1.35f, 0.96f, 1.15f), material, parent);
        CreateCube("Repair Bay Low Pipe Barrier", new Vector3(-0.2f, 0.42f, 19.8f), new Vector3(2.35f, 0.84f, 0.55f), material, parent);
        CreateCube("Key Room Cover Workbench", new Vector3(14.2f, 0.48f, 15.3f), new Vector3(1.8f, 0.96f, 0.65f), material, parent);
        CreateCube("Final Room Cover Stack West", new Vector3(-3.25f, 0.58f, 30.1f), new Vector3(1.45f, 1.16f, 1.45f), material, parent);
        CreateCube("Final Room Cover Stack East", new Vector3(3.15f, 0.58f, 32.3f), new Vector3(1.45f, 1.16f, 1.45f), material, parent);
        CreateCube("Final Room Low Center Barrier", new Vector3(0f, 0.42f, 32.2f), new Vector3(2.1f, 0.84f, 0.58f), material, parent);
    }

    private static void CreatePipeworksAnnexScene(Material wallMaterial, Material floorMaterial, Material exitMaterial, Material enemyMaterial, Material enemyEyeMaterial, Material healthMaterial, Material ammoMaterial, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material brassMaterial, Material warningMaterial, Material ironMaterial, Material oilStoneMaterial, Material gaugeFaceMaterial, Material steamPuffMaterial, Material furnaceGlowMaterial, Material glassMaterial, Material fluidMaterial, WeaponDefinition pressurePistolDefinition, WeaponDefinition steamScattergunDefinition, EnemyDefinition scrapperDefinition, EnemyDefinition lancerDefinition, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition, PlatformQualityProfile windowsQualityProfile)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.44f, 0.42f, 0.38f);

        CreateLighting();
        CreatePipeworksAnnexBlockout(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud, windowsQualityProfile, "Route pipe pressure. Ride the lift to the Boilerheart.");
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, glassMaterial, ironMaterial, warningMaterial, pressurePistolDefinition, steamScattergunDefinition);

        CreateEnemy("Enemy - Pipeworks Gatehouse", new Vector3(-2.2f, 1f, 9.5f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateLancerEnemy("Enemy - Pipeworks Lancer", new Vector3(2.2f, 1f, 17.5f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, lancerDefinition);
        CreateHealthVialPickup("Pickup - Annex Health Vial", new Vector3(-3.2f, 0.65f, 14f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Annex Pressure Cartridge Pack", new Vector3(3.2f, 0.55f, 13.5f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        LevelTransitionTrigger boilerheartLift = CreateLevelTransitionLiftAt("Pipeworks Service Lift To Boilerheart", new Vector3(0f, 1.1f, 23.2f), exitMaterial, ironMaterial, brassMaterial, gaugeFaceMaterial, "Level03", "Service lift grinding toward the Boilerheart").GetComponent<LevelTransitionTrigger>();
        SteamValveObjective routingValve = CreatePipeworksRoutingValve(ironMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial);
        boilerheartLift.requiredValve = routingValve;
        boilerheartLift.lockedPrompt = "route pipe pressure first";
        boilerheartLift.lockedMessage = "The Boilerheart lift is pressure-locked. Route the Pipeworks valve first.";
        CreatePipeworksDressing(ironMaterial, oilStoneMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);
        CreatePipeworksSecretCache(brassMaterial, ironMaterial, warningMaterial, healthMaterial, glassMaterial, fluidMaterial, ammoMaterial, healthPickupDefinition, ammoPickupDefinition);
        CreatePipeworksFlowPolish(brassMaterial, warningMaterial, exitMaterial, ironMaterial, gaugeFaceMaterial, furnaceGlowMaterial);
        CreatePointLight("Pipeworks Exit Green Light", new Vector3(0f, 2.4f, 22.7f), new Color(0.1f, 1f, 0.3f), 2.8f, 7f);
        CreatePointLight("Pipeworks Furnace Light", new Vector3(-4.1f, 1.6f, 16f), new Color(1f, 0.36f, 0.08f), 2.2f, 5f);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), Level02ScenePath);
    }

    private static void CreateBoilerheartScene(Material wallMaterial, Material floorMaterial, Material exitMaterial, Material enemyMaterial, Material enemyEyeMaterial, Material healthMaterial, Material ammoMaterial, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material brassMaterial, Material warningMaterial, Material ironMaterial, Material oilStoneMaterial, Material gaugeFaceMaterial, Material steamPuffMaterial, Material furnaceGlowMaterial, Material glassMaterial, Material fluidMaterial, WeaponDefinition pressurePistolDefinition, WeaponDefinition steamScattergunDefinition, EnemyDefinition scrapperDefinition, EnemyDefinition bellowsNodeDefinition, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition, PickupDefinition steamScattergunPickupDefinition, PlatformQualityProfile windowsQualityProfile)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.38f, 0.32f, 0.26f);

        CreateLighting();
        CreateBoilerheartBlockout(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud, windowsQualityProfile, "Vent the Boilerheart pressure valve. Ride the foundry lift.");
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, glassMaterial, ironMaterial, warningMaterial, pressurePistolDefinition, steamScattergunDefinition);

        CreateEnemy("Enemy - Boilerheart Floor Guard", new Vector3(-2.6f, 1f, 12.8f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateEnemy("Enemy - Boilerheart Lift Guard", new Vector3(2.4f, 1f, 19.2f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateBellowsNodeEnemy("Enemy - Boilerheart Bellows Node", new Vector3(3.6f, 0.95f, 15.1f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, bellowsNodeDefinition);
        CreateHealthVialPickup("Pickup - Boilerheart Health Vial", new Vector3(-3.6f, 0.65f, 9.4f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Boilerheart Pressure Cartridge Pack", new Vector3(3.4f, 0.55f, 9.8f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        CreateSteamScattergunPickup("Pickup - Steam Scattergun", new Vector3(0f, 0.72f, 13.9f), gunMaterial, brassMaterial, ironMaterial, warningMaterial, steamScattergunPickupDefinition);
        CreateSteamScattergunPickupReadabilityCues(brassMaterial, warningMaterial, ironMaterial, gaugeFaceMaterial);
        SteamHazard[] boilerheartHazards = CreateBoilerheartDressing(ironMaterial, oilStoneMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);
        LevelTransitionTrigger foundryLift = CreateLevelTransitionLiftAt("Boilerheart Service Lift To Foundry", new Vector3(0f, 1.1f, 24.3f), exitMaterial, ironMaterial, brassMaterial, gaugeFaceMaterial, "Level04", "Service lift climbing toward the Furnace Foundry").GetComponent<LevelTransitionTrigger>();
        SteamValveObjective pressureValve = CreateBoilerheartPressureValve(ironMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial);
        pressureValve.hazardsToDisableOnComplete = boilerheartHazards;
        foundryLift.requiredValve = pressureValve;
        foundryLift.lockedMessage = "The foundry lift is pressure-locked. Vent the Boilerheart first.";
        CreateBoilerheartFlowPolish(brassMaterial, warningMaterial, exitMaterial, ironMaterial, gaugeFaceMaterial, furnaceGlowMaterial);
        CreateLevel03SignageDecalsV1();
        CreatePointLight("Boilerheart Furnace Light", new Vector3(0f, 2.6f, 15.8f), new Color(1f, 0.32f, 0.08f), 4f, 10f);
        CreatePointLight("Boilerheart Foundry Lift Green Light", new Vector3(0f, 2.6f, 23.8f), new Color(0.1f, 1f, 0.35f), 2.8f, 7f);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), Level03ScenePath);
    }

    private static void CreateFurnaceFoundryScene(Material wallMaterial, Material floorMaterial, Material exitMaterial, Material enemyMaterial, Material enemyEyeMaterial, Material healthMaterial, Material ammoMaterial, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material brassMaterial, Material warningMaterial, Material ironMaterial, Material oilStoneMaterial, Material gaugeFaceMaterial, Material steamPuffMaterial, Material furnaceGlowMaterial, Material glassMaterial, Material fluidMaterial, WeaponDefinition pressurePistolDefinition, WeaponDefinition steamScattergunDefinition, EnemyDefinition scrapperDefinition, EnemyDefinition lancerDefinition, EnemyDefinition bulwarkDefinition, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition, PlatformQualityProfile windowsQualityProfile)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.32f, 0.26f, 0.22f);

        CreateLighting();
        CreateFurnaceFoundryBlockout(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud, windowsQualityProfile, "Cross the Furnace Foundry. Reach the emergency hoist.");
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, glassMaterial, ironMaterial, warningMaterial, pressurePistolDefinition, steamScattergunDefinition);

        CreateEnemy("Enemy - Foundry Intake Scrapper", new Vector3(-2.9f, 1f, 9.2f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateLancerEnemy("Enemy - Foundry Catwalk Lancer", new Vector3(2.8f, 1f, 15.8f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, lancerDefinition);
        CreateBulwarkEnemy("Enemy - Foundry Hammer Bulwark", new Vector3(2.6f, 1.15f, 22.8f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, bulwarkDefinition);
        CreateEnemy("Enemy - Foundry Hoist Scrapper", new Vector3(-2.4f, 1f, 23.4f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateHealthVialPickup("Pickup - Foundry Health Vial", new Vector3(-4.35f, 0.65f, 13.2f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Foundry Pressure Cartridge Pack", new Vector3(4.25f, 0.55f, 18.4f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        CreateFoundrySecretCache(brassMaterial, ironMaterial, warningMaterial, healthMaterial, glassMaterial, fluidMaterial, ammoMaterial, healthPickupDefinition, ammoPickupDefinition);
        CreateFurnaceFoundryDressing(ironMaterial, oilStoneMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);
        CreateFoundryClimaxPolish(brassMaterial, warningMaterial, exitMaterial, ironMaterial, gaugeFaceMaterial, furnaceGlowMaterial);
        CreateLevelTransitionLiftAt("Foundry Emergency Hoist", new Vector3(0f, 1.1f, 28.3f), exitMaterial, ironMaterial, brassMaterial, gaugeFaceMaterial, "Level05", "Emergency hoist rising toward the Governor Core");
        CreatePointLight("Foundry Furnace Light West", new Vector3(-4.65f, 2.1f, 14.8f), new Color(1f, 0.32f, 0.08f), 3.2f, 8f);
        CreatePointLight("Foundry Furnace Light East", new Vector3(4.65f, 2.1f, 20.4f), new Color(1f, 0.28f, 0.07f), 3.2f, 8f);
        CreatePointLight("Foundry Emergency Hoist Green Light", new Vector3(0f, 2.6f, 27.8f), new Color(0.1f, 1f, 0.35f), 2.8f, 7f);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), Level04ScenePath);
    }

    private static void CreateGovernorCoreScene(Material wallMaterial, Material floorMaterial, Material exitMaterial, Material enemyMaterial, Material enemyEyeMaterial, Material healthMaterial, Material ammoMaterial, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material brassMaterial, Material warningMaterial, Material ironMaterial, Material oilStoneMaterial, Material gaugeFaceMaterial, Material steamPuffMaterial, Material furnaceGlowMaterial, Material glassMaterial, Material fluidMaterial, WeaponDefinition pressurePistolDefinition, WeaponDefinition steamScattergunDefinition, EnemyDefinition scrapperDefinition, EnemyDefinition lancerDefinition, EnemyDefinition bulwarkDefinition, EnemyDefinition governorWardenDefinition, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition, PlatformQualityProfile windowsQualityProfile)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.3f, 0.28f, 0.24f);

        CreateLighting();
        CreateGovernorCoreBlockout(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud, windowsQualityProfile, "Breach the Governor Core. Reach the master override hoist.");
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, glassMaterial, ironMaterial, warningMaterial, pressurePistolDefinition, steamScattergunDefinition);

        CreateEnemy("Enemy - Governor Core Intake Scrapper", new Vector3(-2.9f, 1f, 8.8f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateLancerEnemy("Enemy - Governor Core Lancer", new Vector3(3.2f, 1f, 15.8f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, lancerDefinition);
        CreateBulwarkEnemy("Enemy - Governor Core Bulwark", new Vector3(-3.15f, 1.15f, 21f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, bulwarkDefinition);
        GovernorWardenController warden = CreateGovernorWardenEnemy("Enemy - Governor Core Warden", new Vector3(0f, 1.45f, 24.1f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, governorWardenDefinition);
        CreateHealthVialPickup("Pickup - Governor Health Vial", new Vector3(-4.55f, 0.65f, 13.5f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Governor Pressure Cartridge Pack", new Vector3(4.45f, 0.55f, 20.2f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        CreateGovernorCoreDressing(ironMaterial, oilStoneMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);
        CreateGovernorClimaxPolish(brassMaterial, warningMaterial, exitMaterial, ironMaterial, gaugeFaceMaterial, furnaceGlowMaterial);
        ExitTrigger finalHoist = CreateExitAt("Governor Core Master Override Hoist", new Vector3(0f, 1.1f, 28.6f), exitMaterial, ironMaterial, brassMaterial, gaugeFaceMaterial).GetComponent<ExitTrigger>();
        GuardianDefeatObjective guardianObjective = CreateGovernorWardenDefeatObjective(warden, ironMaterial, brassMaterial, warningMaterial);
        finalHoist.requiredGuardian = guardianObjective;
        finalHoist.guardianLockedPrompt = "defeat the Governor Warden first";
        finalHoist.guardianLockedMessage = "The master override hoist is guarded. Destroy the Governor Warden first.";
        CreateLevel05SignageDecalsV1();
        CreatePointLight("Governor Core Regulator Light", new Vector3(0f, 2.8f, 16.2f), new Color(1f, 0.36f, 0.08f), 4.2f, 10f);
        CreatePointLight("Governor Core Hoist Green Light", new Vector3(0f, 2.6f, 28f), new Color(0.1f, 1f, 0.35f), 2.8f, 7f);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), Level05ScenePath);
    }

    private static void CreateGovernorCoreBlockout(Material wallMaterial, Material floorMaterial)
    {
        GameObject parent = new GameObject("Governor Core Blockout");

        CreateCube("Governor Core Floor", new Vector3(0f, -0.1f, 14f), new Vector3(15f, 0.2f, 32f), floorMaterial, parent.transform);
        CreateCube("Governor Core South Wall", new Vector3(0f, 1.5f, -2f), new Vector3(14.5f, 3f, 0.5f), wallMaterial, parent.transform);
        CreateCube("Governor Core North Wall", new Vector3(0f, 1.5f, 30f), new Vector3(14.5f, 3f, 0.5f), wallMaterial, parent.transform);
        CreateCube("Governor Core West Wall", new Vector3(-7.25f, 1.5f, 14f), new Vector3(0.5f, 3f, 32f), wallMaterial, parent.transform);
        CreateCube("Governor Core East Wall", new Vector3(7.25f, 1.5f, 14f), new Vector3(0.5f, 3f, 32f), wallMaterial, parent.transform);
        CreateCube("Governor Core Ring West", new Vector3(-3.5f, 1.5f, 15.4f), new Vector3(0.5f, 3f, 7.2f), wallMaterial, parent.transform);
        CreateCube("Governor Core Ring East", new Vector3(3.5f, 1.5f, 18.8f), new Vector3(0.5f, 3f, 7.2f), wallMaterial, parent.transform);
        CreateCube("Governor Core Low Gear Cover A", new Vector3(-1.8f, 0.5f, 13.6f), new Vector3(1.6f, 1f, 1.2f), wallMaterial, parent.transform);
        CreateCube("Governor Core Low Gear Cover B", new Vector3(1.8f, 0.5f, 21.4f), new Vector3(1.6f, 1f, 1.2f), wallMaterial, parent.transform);
    }

    private static void CreateGovernorCoreDressing(Material ironMaterial, Material floorPatchMaterial, Material brassMaterial, Material warningMaterial, Material gaugeFaceMaterial, Material steamMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Governor Core Dressing");

        CreateCube("Governor Core Oil Ring", new Vector3(0f, 0.02f, 16.2f), new Vector3(4.6f, 0.04f, 4.6f), floorPatchMaterial, parent.transform);
        CreateCube("Governor Core Regulator Pillar", new Vector3(0f, 1.25f, 16.2f), new Vector3(1.25f, 2.5f, 1.25f), ironMaterial, parent.transform);
        CreateCube("Governor Core Regulator Glow A", new Vector3(0f, 1.3f, 15.55f), new Vector3(0.78f, 1.4f, 0.08f), glowMaterial, parent.transform);
        CreateCube("Governor Core Regulator Glow B", new Vector3(0f, 1.3f, 16.85f), new Vector3(0.78f, 1.4f, 0.08f), glowMaterial, parent.transform);
        CreatePipeBundle("Governor Core Triple Pipe Bundle", new Vector3(0f, 2.35f, 29.72f), Quaternion.Euler(0f, 90f, 0f), 4.8f, brassMaterial, ironMaterial, parent.transform);
        CreatePressureGauge("Governor Core Gauge A", new Vector3(-6.95f, 1.65f, 12.4f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateValveWheel("Governor Core Valve A", new Vector3(6.95f, 1.35f, 20.4f), Quaternion.Euler(0f, -90f, 0f), brassMaterial, warningMaterial, parent.transform);
        CreateSteamVent("Governor Core Steam Vent A", new Vector3(-4.8f, 0.05f, 20.8f), brassMaterial, steamMaterial, parent.transform);
        CreatePipeCanopy("North Star Governor Pipe Canopy", new Vector3(0f, 2.86f, 16.2f), Quaternion.Euler(0f, 90f, 0f), 6.4f, brassMaterial, ironMaterial, parent.transform);
        CreateCagedGaslight("North Star Governor Gaslight Left", new Vector3(-4.85f, 2.08f, 12.8f), Quaternion.Euler(0f, 90f, 0f), ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreateCagedGaslight("North Star Governor Gaslight Right", new Vector3(4.85f, 2.08f, 20.2f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreateRegulatorCrown("North Star Governor Regulator Crown", new Vector3(0f, 2.72f, 16.2f), ironMaterial, brassMaterial, warningMaterial, parent.transform);
        CreateSteamHazard("Governor Core Steam Hazard - Regulator Leak", new Vector3(-4.8f, 0.75f, 20.8f), new Vector3(1.25f, 1.5f, 1.25f), ironMaterial, steamMaterial, warningMaterial, parent.transform);
        CreateFurnaceHeatHazard("Governor Core Furnace Heat Hazard - Regulator Surge", new Vector3(0f, 0.75f, 18.9f), new Vector3(4.4f, 1.5f, 1.25f), ironMaterial, glowMaterial, warningMaterial, parent.transform, 0.8f);
        CreateWorkOrderBoard("Work Order Board - Governor Core", "GOVERNOR CORE\nMASTER OVERRIDE\nDO NOT STALL", new Vector3(-6.95f, 1.55f, 15.4f), Quaternion.Euler(0f, 90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateLorePlaque("Lore Plaque - Governor Archive", "Governor Archive", "The Warden protects the stalled governor because no order ever told it the workers were worth more than the machine.", new Vector3(-6.95f, 1.55f, 17.8f), Quaternion.Euler(0f, 90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
    }

    private static void CreateFurnaceFoundryBlockout(Material wallMaterial, Material floorMaterial)
    {
        GameObject parent = new GameObject("Furnace Foundry Blockout");

        CreateCube("Foundry Floor", new Vector3(0f, -0.1f, 14f), new Vector3(14f, 0.2f, 32f), floorMaterial, parent.transform);
        CreateCube("Foundry South Wall", new Vector3(0f, 1.5f, -2f), new Vector3(13.5f, 3f, 0.5f), wallMaterial, parent.transform);
        CreateCube("Foundry North Wall", new Vector3(0f, 1.5f, 30f), new Vector3(13.5f, 3f, 0.5f), wallMaterial, parent.transform);
        CreateCube("Foundry West Wall", new Vector3(-6.75f, 1.5f, 14f), new Vector3(0.5f, 3f, 32f), wallMaterial, parent.transform);
        CreateCube("Foundry East Wall", new Vector3(6.75f, 1.5f, 14f), new Vector3(0.5f, 3f, 32f), wallMaterial, parent.transform);
        CreateCube("Foundry Intake Baffle West", new Vector3(-3.35f, 1.5f, 8.6f), new Vector3(0.5f, 3f, 5.4f), wallMaterial, parent.transform);
        CreateCube("Foundry Pour Channel East", new Vector3(3.35f, 1.5f, 17.2f), new Vector3(0.5f, 3f, 5.8f), wallMaterial, parent.transform);
        CreateCube("Foundry Hoist Baffle West", new Vector3(-3.15f, 1.5f, 24.3f), new Vector3(0.5f, 3f, 4.2f), wallMaterial, parent.transform);
        CreateCube("Foundry Crucible Cover A", new Vector3(-1.75f, 0.55f, 13.4f), new Vector3(1.35f, 1.1f, 1.35f), wallMaterial, parent.transform);
        CreateCube("Foundry Crucible Cover B", new Vector3(1.95f, 0.55f, 20.4f), new Vector3(1.35f, 1.1f, 1.35f), wallMaterial, parent.transform);
        CreateCube("Foundry Low Pipe Barrier", new Vector3(0f, 0.42f, 23.8f), new Vector3(2.4f, 0.84f, 0.58f), wallMaterial, parent.transform);
    }

    private static SteamHazard[] CreateFurnaceFoundryDressing(Material ironMaterial, Material floorPatchMaterial, Material brassMaterial, Material warningMaterial, Material gaugeFaceMaterial, Material steamMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Furnace Foundry Dressing");

        CreateCube("Foundry Oil Patch A", new Vector3(-2.7f, 0.02f, 11.4f), new Vector3(2.2f, 0.04f, 1.45f), floorPatchMaterial, parent.transform);
        CreateCube("Foundry Oil Patch B", new Vector3(2.55f, 0.02f, 21.2f), new Vector3(2.1f, 0.04f, 1.55f), floorPatchMaterial, parent.transform);
        CreateCube("Foundry Overhead Main Pipe", new Vector3(0f, 2.55f, 18.2f), new Vector3(11.6f, 0.16f, 0.16f), brassMaterial, parent.transform);
        CreateCube("Foundry Red Pressure Pipe", new Vector3(5.95f, 2.1f, 18.8f), new Vector3(0.14f, 0.14f, 20f), warningMaterial, parent.transform);
        CreatePipeBundle("Foundry Triple Pipe Bundle", new Vector3(0f, 2.35f, 29.72f), Quaternion.Euler(0f, 90f, 0f), 4.4f, brassMaterial, ironMaterial, parent.transform);
        CreatePipeCanopy("North Star Foundry Pipe Canopy", new Vector3(0f, 2.86f, 16.4f), Quaternion.Euler(0f, 90f, 0f), 7.4f, brassMaterial, ironMaterial, parent.transform);
        CreateCatwalkRail("North Star Foundry Catwalk Rail", new Vector3(0f, 1.02f, 19.05f), 5.8f, ironMaterial, brassMaterial, parent.transform);
        CreateCagedGaslight("North Star Foundry Gaslight West", new Vector3(-5.88f, 2.06f, 13.4f), Quaternion.Euler(0f, 90f, 0f), ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreateCagedGaslight("North Star Foundry Gaslight East", new Vector3(5.88f, 2.06f, 21.2f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreatePressureGauge("Foundry Gauge A", new Vector3(-6.45f, 1.65f, 12.2f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreatePressureGauge("Foundry Gauge B", new Vector3(6.45f, 1.65f, 21.6f), Quaternion.Euler(0f, -90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateValveWheel("Foundry Valve A", new Vector3(-6.45f, 1.35f, 19.2f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, warningMaterial, parent.transform);
        CreateSteamVent("Foundry Steam Vent A", new Vector3(-4.7f, 0.05f, 14.2f), brassMaterial, steamMaterial, parent.transform);
        CreateSteamVent("Foundry Steam Vent B", new Vector3(3.65f, 0.05f, 21.5f), brassMaterial, steamMaterial, parent.transform);
        SteamHazard castingLeak = CreateSteamHazard("Foundry Steam Hazard - Casting Leak", new Vector3(-4.7f, 0.75f, 14.2f), new Vector3(1.25f, 1.5f, 1.25f), ironMaterial, steamMaterial, warningMaterial, parent.transform);
        SteamHazard crucibleBleed = CreateSteamHazard("Foundry Steam Hazard - Crucible Bleed", new Vector3(3.65f, 0.75f, 21.5f), new Vector3(1.2f, 1.5f, 1.2f), ironMaterial, steamMaterial, warningMaterial, parent.transform);
        CreateFurnaceHeatHazard("Foundry Furnace Heat Hazard - Pour Lane", new Vector3(0f, 0.75f, 16.7f), new Vector3(4.6f, 1.5f, 1.35f), ironMaterial, glowMaterial, warningMaterial, parent.transform, 0f);
        CreateFurnaceHeatHazard("Foundry Furnace Heat Hazard - Hoist Lane", new Vector3(0f, 0.75f, 23.6f), new Vector3(4.2f, 1.5f, 1.25f), ironMaterial, glowMaterial, warningMaterial, parent.transform, 1.65f);
        CreateFoundryFurnaceRow(ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreateWorkOrderBoard("Work Order Board - Foundry", "FOUNDRY ORDER\nHOIST ROUTE OPEN\nKEEP FIRES FED", new Vector3(-6.45f, 1.55f, 16.1f), Quaternion.Euler(0f, 90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateLorePlaque("Lore Plaque - Foundry Archive", "Foundry Archive", "The foundry made the Bulwarks as rescue frames. Their hammers remember only containment.", new Vector3(-6.45f, 1.55f, 18.5f), Quaternion.Euler(0f, 90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);

        return new[] { castingLeak, crucibleBleed };
    }

    private static void CreateFoundryFurnaceRow(Material ironMaterial, Material brassMaterial, Material glowMaterial, Transform parent)
    {
        GameObject row = new GameObject("Foundry Furnace Row");
        row.transform.SetParent(parent);
        CreateFurnace("Foundry Furnace Row A", new Vector3(-5.95f, 0.95f, 10.4f), ironMaterial, brassMaterial, glowMaterial, row.transform);
        CreateFurnace("Foundry Furnace Row B", new Vector3(-5.95f, 0.95f, 13.4f), ironMaterial, brassMaterial, glowMaterial, row.transform);
        CreateFurnace("Foundry Furnace Row C", new Vector3(5.95f, 0.95f, 20.1f), ironMaterial, brassMaterial, glowMaterial, row.transform);
    }

    private static void CreateBoilerheartBlockout(Material wallMaterial, Material floorMaterial)
    {
        GameObject parent = new GameObject("Boilerheart Blockout");

        CreateCube("Boilerheart Floor", new Vector3(0f, -0.1f, 12f), new Vector3(13f, 0.2f, 28f), floorMaterial, parent.transform);
        CreateCube("Boilerheart South Wall", new Vector3(0f, 1.5f, -2f), new Vector3(12.5f, 3f, 0.5f), wallMaterial, parent.transform);
        CreateCube("Boilerheart North Wall", new Vector3(0f, 1.5f, 26f), new Vector3(12.5f, 3f, 0.5f), wallMaterial, parent.transform);
        CreateCube("Boilerheart West Wall", new Vector3(-6.25f, 1.5f, 12f), new Vector3(0.5f, 3f, 28f), wallMaterial, parent.transform);
        CreateCube("Boilerheart East Wall", new Vector3(6.25f, 1.5f, 12f), new Vector3(0.5f, 3f, 28f), wallMaterial, parent.transform);
        CreateCube("Boilerheart Furnace Core", new Vector3(0f, 1.3f, 15.6f), new Vector3(2.2f, 2.6f, 2.2f), wallMaterial, parent.transform);
        CreateCube("Boilerheart West Baffle", new Vector3(-3.5f, 1.5f, 8.4f), new Vector3(0.5f, 3f, 5.6f), wallMaterial, parent.transform);
        CreateCube("Boilerheart East Baffle", new Vector3(3.5f, 1.5f, 18.4f), new Vector3(0.5f, 3f, 5.6f), wallMaterial, parent.transform);
        CreateCube("Boilerheart Low Cover West", new Vector3(-2.6f, 0.5f, 14.2f), new Vector3(1.4f, 1f, 1.1f), wallMaterial, parent.transform);
        CreateCube("Boilerheart Low Cover East", new Vector3(2.8f, 0.5f, 20.4f), new Vector3(1.4f, 1f, 1.1f), wallMaterial, parent.transform);
    }

    private static SteamHazard[] CreateBoilerheartDressing(Material ironMaterial, Material floorPatchMaterial, Material brassMaterial, Material warningMaterial, Material gaugeFaceMaterial, Material steamMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Boilerheart Dressing");

        CreateCube("Boilerheart Core Glow Front", new Vector3(0f, 1.25f, 14.45f), new Vector3(1.2f, 1.5f, 0.08f), glowMaterial, parent.transform);
        CreateCube("Boilerheart Core Glow Back", new Vector3(0f, 1.25f, 16.75f), new Vector3(1.2f, 1.5f, 0.08f), glowMaterial, parent.transform);
        CreateCube("Boilerheart Oil Patch", new Vector3(-1.8f, 0.02f, 11.6f), new Vector3(2f, 0.04f, 1.4f), floorPatchMaterial, parent.transform);
        CreateCube("Boilerheart Overhead Pipe", new Vector3(0f, 2.45f, 20.6f), new Vector3(9f, 0.16f, 0.16f), brassMaterial, parent.transform);
        CreatePipeBundle("Boilerheart Triple Pipe Bundle", new Vector3(0f, 2.35f, 25.72f), Quaternion.Euler(0f, 90f, 0f), 4.2f, brassMaterial, ironMaterial, parent.transform);
        CreatePipeCanopy("North Star Boilerheart Pipe Canopy", new Vector3(0f, 2.82f, 15.6f), Quaternion.Euler(0f, 90f, 0f), 6.8f, brassMaterial, ironMaterial, parent.transform);
        CreateCagedGaslight("North Star Boilerheart Lamp Cage", new Vector3(-5.52f, 2.02f, 15.1f), Quaternion.Euler(0f, 90f, 0f), ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreateRivetBand("North Star Boilerheart Core Rivet Band", new Vector3(0f, 2.66f, 14.38f), Quaternion.identity, 2.45f, ironMaterial, brassMaterial, parent.transform);
        CreateWallPipeGaugeClusterPrototype("Boilerheart Prototype Wall Pipe Gauge Cluster", new Vector3(-5.94f, 1.58f, 10.8f), Quaternion.Euler(0f, 90f, 0f), 0.94f, brassMaterial, ironMaterial, gaugeFaceMaterial, warningMaterial, "boilerheart_route_wall", parent.transform);
        CreateBoilerControlConsolePrototype("Boilerheart Prototype Boiler Control Console", new Vector3(5.52f, 0.62f, 18.62f), Quaternion.Euler(0f, -90f, 0f), 0.92f, brassMaterial, ironMaterial, gaugeFaceMaterial, warningMaterial, "boilerheart_route_console", parent.transform);
        CreatePressureGauge("Boilerheart Gauge A", new Vector3(-5.95f, 1.65f, 12.4f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateValveWheel("Boilerheart Valve A", new Vector3(5.95f, 1.45f, 17.4f), Quaternion.Euler(0f, -90f, 0f), brassMaterial, warningMaterial, parent.transform);
        CreateSteamVent("Boilerheart Steam Vent A", new Vector3(-4.8f, 0.05f, 20.8f), brassMaterial, steamMaterial, parent.transform);
        SteamHazard furnaceLeak = CreateSteamHazard("Boilerheart Steam Hazard - Furnace Leak", new Vector3(-4.8f, 0.75f, 20.8f), new Vector3(1.25f, 1.5f, 1.25f), ironMaterial, steamMaterial, warningMaterial, parent.transform);
        SteamHazard coreBleed = CreateSteamHazard("Boilerheart Steam Hazard - Core Bleed", new Vector3(2.35f, 0.75f, 15.4f), new Vector3(1.15f, 1.5f, 1.15f), ironMaterial, steamMaterial, warningMaterial, parent.transform);
        CreateWorkOrderBoard("Work Order Board - Boilerheart", "BOILERHEART ORDER\nCORE PRESSURE HIGH\nFINAL LIFT LIVE", new Vector3(5.95f, 1.55f, 10.6f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateLorePlaque("Lore Plaque - Boilerheart Archive", "Boilerheart Archive", "The heart valve was built to save workers from overload. Vented by hand, it can still break a pressure lock.", new Vector3(5.95f, 1.55f, 13f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);

        return new[] { furnaceLeak, coreBleed };
    }

    private static SteamHazard CreateSteamHazard(string name, Vector3 position, Vector3 triggerSize, Material ironMaterial, Material steamMaterial, Material warningMaterial, Transform parent)
    {
        GameObject hazardRoot = new GameObject(name);
        hazardRoot.transform.SetParent(parent);
        hazardRoot.transform.position = position;

        BoxCollider trigger = hazardRoot.AddComponent<BoxCollider>();
        trigger.size = triggerSize;
        trigger.isTrigger = true;

        SteamHazard hazard = hazardRoot.AddComponent<SteamHazard>();
        hazard.damagePerTick = GameBalance.SteamHazardDamage;
        hazard.tickInterval = GameBalance.SteamHazardTickInterval;
        SteamHazardVfx hazardVfx = hazardRoot.AddComponent<SteamHazardVfx>();

        CreateLocalCube(name + " Warning Floor Plate", hazardRoot.transform, new Vector3(0f, -0.73f, 0f), new Vector3(triggerSize.x, 0.04f, triggerSize.z), warningMaterial);
        GameObject lowPuff = CreateLocalPrimitive(name + " Steam Puff Low", PrimitiveType.Sphere, hazardRoot.transform, new Vector3(-0.12f, -0.15f, 0.04f), new Vector3(0.38f, 0.32f, 0.38f), steamMaterial);
        GameObject highPuff = CreateLocalPrimitive(name + " Steam Puff High", PrimitiveType.Sphere, hazardRoot.transform, new Vector3(0.16f, 0.38f, -0.05f), new Vector3(0.3f, 0.46f, 0.3f), steamMaterial);
        hazardVfx.puffs = new[] { lowPuff.transform, highPuff.transform };
        CreateLocalCube(name + " Brass Vent Grate", hazardRoot.transform, new Vector3(0f, -0.68f, 0f), new Vector3(triggerSize.x * 0.68f, 0.08f, triggerSize.z * 0.68f), ironMaterial);

        return hazard;
    }

    private static FurnaceHeatHazard CreateFurnaceHeatHazard(string name, Vector3 position, Vector3 triggerSize, Material ironMaterial, Material glowMaterial, Material warningMaterial, Transform parent, float phaseOffset)
    {
        GameObject hazardRoot = new GameObject(name);
        hazardRoot.transform.SetParent(parent);
        hazardRoot.transform.position = position;

        BoxCollider trigger = hazardRoot.AddComponent<BoxCollider>();
        trigger.size = triggerSize;
        trigger.isTrigger = true;

        FurnaceHeatHazard hazard = hazardRoot.AddComponent<FurnaceHeatHazard>();
        hazard.damagePerTick = GameBalance.FurnaceHeatHazardDamage;
        hazard.tickInterval = GameBalance.FurnaceHeatHazardTickInterval;
        hazard.warningDuration = GameBalance.FurnaceHeatHazardWarningDuration;
        hazard.activeDuration = GameBalance.FurnaceHeatHazardActiveDuration;
        hazard.cooldownDuration = GameBalance.FurnaceHeatHazardCooldownDuration;
        hazard.phaseOffset = phaseOffset;
        FurnaceHeatHazardVfx hazardVfx = hazardRoot.AddComponent<FurnaceHeatHazardVfx>();

        CreateLocalCube(name + " Iron Pour Trough", hazardRoot.transform, new Vector3(0f, -0.72f, 0f), new Vector3(triggerSize.x, 0.08f, triggerSize.z * 0.86f), ironMaterial);
        CreateLocalCube(name + " Brass Warning Strip A", hazardRoot.transform, new Vector3(0f, -0.65f, -triggerSize.z * 0.42f), new Vector3(triggerSize.x, 0.06f, 0.08f), warningMaterial);
        CreateLocalCube(name + " Brass Warning Strip B", hazardRoot.transform, new Vector3(0f, -0.65f, triggerSize.z * 0.42f), new Vector3(triggerSize.x, 0.06f, 0.08f), warningMaterial);
        hazard.warningSignal = CreateLocalCube(name + " Amber Warning Signal", hazardRoot.transform, new Vector3(0f, -0.54f, 0f), new Vector3(triggerSize.x * 0.62f, 0.06f, triggerSize.z * 0.48f), warningMaterial);
        hazard.activeSignal = CreateLocalCube(name + " Furnace Glow Plate", hazardRoot.transform, new Vector3(0f, -0.5f, 0f), new Vector3(triggerSize.x * 0.7f, 0.08f, triggerSize.z * 0.56f), glowMaterial);
        hazard.safeSignal = CreateLocalCube(name + " Closed Iron Damper", hazardRoot.transform, new Vector3(0f, -0.46f, 0f), new Vector3(triggerSize.x * 0.46f, 0.06f, triggerSize.z * 0.32f), ironMaterial);
        GameObject heatWaveA = CreateLocalCube(name + " Heat Ripple A", hazardRoot.transform, new Vector3(-triggerSize.x * 0.18f, -0.22f, -triggerSize.z * 0.14f), new Vector3(0.08f, 0.28f, triggerSize.z * 0.48f), glowMaterial);
        GameObject heatWaveB = CreateLocalCube(name + " Heat Ripple B", hazardRoot.transform, new Vector3(triggerSize.x * 0.18f, -0.16f, triggerSize.z * 0.1f), new Vector3(0.08f, 0.34f, triggerSize.z * 0.38f), glowMaterial);
        hazardVfx.warningSignal = hazard.warningSignal.transform;
        hazardVfx.activeSignal = hazard.activeSignal.transform;
        hazardVfx.safeSignal = hazard.safeSignal.transform;
        hazardVfx.heatWaves = new[] { heatWaveA.transform, heatWaveB.transform };

        return hazard;
    }

    private static SteamValveObjective CreateBoilerheartPressureValve(Material ironMaterial, Material brassMaterial, Material warningMaterial, Material gaugeFaceMaterial, Material steamMaterial)
    {
        GameObject valveRoot = new GameObject("Boilerheart Pressure Valve Objective");
        valveRoot.transform.position = new Vector3(5.82f, 1.35f, 17.4f);
        valveRoot.transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        BoxCollider trigger = valveRoot.AddComponent<BoxCollider>();
        trigger.size = new Vector3(1.3f, 1.45f, 0.5f);
        trigger.isTrigger = true;

        SteamValveObjective valve = valveRoot.AddComponent<SteamValveObjective>();
        valve.prompt = "E - vent Boilerheart pressure";
        valve.completePrompt = "Boilerheart pressure vented";
        valve.completeMessage = "Boilerheart pressure vented. Final lift unlocked.";
        valve.objectiveAfterComplete = "Ride the foundry lift.";

        CreateLocalCube("Boilerheart Pressure Valve Backplate", valveRoot.transform, Vector3.zero, new Vector3(1.12f, 1.28f, 0.08f), ironMaterial);
        GameObject wheelAssembly = CreateLocalEmpty("Boilerheart Pressure Valve Wheel Assembly", valveRoot.transform, new Vector3(0f, 0.08f, -0.12f), Quaternion.Euler(90f, 0f, 0f));
        AddSpinner(wheelAssembly, 18f);
        CreateLocalPrimitive("Boilerheart Pressure Valve Wheel", PrimitiveType.Cylinder, wheelAssembly.transform, Vector3.zero, new Vector3(0.5f, 0.045f, 0.5f), brassMaterial);
        CreateLocalCube("Boilerheart Pressure Valve Spoke Horizontal", wheelAssembly.transform, new Vector3(0f, -0.04f, 0f), new Vector3(0.92f, 0.045f, 0.045f), warningMaterial);
        CreateLocalCube("Boilerheart Pressure Valve Spoke Vertical", wheelAssembly.transform, new Vector3(0f, -0.04f, 0f), new Vector3(0.045f, 0.045f, 0.92f), warningMaterial);
        CreateLocalPrimitive("Boilerheart Pressure Valve Hub", PrimitiveType.Sphere, wheelAssembly.transform, new Vector3(0f, -0.08f, 0f), new Vector3(0.14f, 0.14f, 0.14f), brassMaterial);

        GameObject gauge = CreateLocalPrimitive("Boilerheart Pressure Valve Gauge", PrimitiveType.Cylinder, valveRoot.transform, new Vector3(0.32f, 0.48f, -0.14f), new Vector3(0.2f, 0.03f, 0.2f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Boilerheart Pressure Valve Gauge Needle", valveRoot.transform, new Vector3(0.37f, 0.48f, -0.18f), new Vector3(0.16f, 0.018f, 0.018f), warningMaterial);
        valve.lockedSignal = CreateLocalPrimitive("Boilerheart Valve Locked Lamp", PrimitiveType.Sphere, valveRoot.transform, new Vector3(-0.34f, 0.48f, -0.16f), new Vector3(0.13f, 0.13f, 0.13f), warningMaterial);
        valve.ventedSignal = CreateLocalPrimitive("Boilerheart Valve Vented Lamp", PrimitiveType.Sphere, valveRoot.transform, new Vector3(-0.34f, 0.48f, -0.17f), new Vector3(0.13f, 0.13f, 0.13f), steamMaterial);
        CreateLocalCube("Boilerheart Pressure Valve Outlet Pipe", valveRoot.transform, new Vector3(0f, -0.52f, -0.12f), new Vector3(0.18f, 0.62f, 0.18f), brassMaterial);

        return valve;
    }

    private static SteamValveObjective CreatePipeworksRoutingValve(Material ironMaterial, Material brassMaterial, Material warningMaterial, Material gaugeFaceMaterial, Material steamMaterial)
    {
        GameObject valveRoot = new GameObject("Pipeworks Routing Valve Objective");
        valveRoot.transform.position = new Vector3(-4.95f, 1.35f, 20f);
        valveRoot.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        BoxCollider trigger = valveRoot.AddComponent<BoxCollider>();
        trigger.size = new Vector3(1.3f, 1.45f, 0.5f);
        trigger.isTrigger = true;

        SteamValveObjective valve = valveRoot.AddComponent<SteamValveObjective>();
        valve.prompt = "E - route pipe pressure";
        valve.completePrompt = "pipe pressure routed";
        valve.completeMessage = "Pipeworks pressure routed. Boilerheart lift unlocked.";
        valve.objectiveAfterComplete = "Ride the Boilerheart lift.";

        CreateLocalCube("Pipeworks Routing Valve Backplate", valveRoot.transform, Vector3.zero, new Vector3(1.12f, 1.28f, 0.08f), ironMaterial);
        GameObject wheelAssembly = CreateLocalEmpty("Pipeworks Routing Valve Wheel Assembly", valveRoot.transform, new Vector3(0f, 0.08f, -0.12f), Quaternion.Euler(90f, 0f, 0f));
        AddSpinner(wheelAssembly, 20f);
        CreateLocalPrimitive("Pipeworks Routing Valve Wheel", PrimitiveType.Cylinder, wheelAssembly.transform, Vector3.zero, new Vector3(0.5f, 0.045f, 0.5f), brassMaterial);
        CreateLocalCube("Pipeworks Routing Valve Spoke Horizontal", wheelAssembly.transform, new Vector3(0f, -0.04f, 0f), new Vector3(0.92f, 0.045f, 0.045f), warningMaterial);
        CreateLocalCube("Pipeworks Routing Valve Spoke Vertical", wheelAssembly.transform, new Vector3(0f, -0.04f, 0f), new Vector3(0.045f, 0.045f, 0.92f), warningMaterial);
        CreateLocalPrimitive("Pipeworks Routing Valve Hub", PrimitiveType.Sphere, wheelAssembly.transform, new Vector3(0f, -0.08f, 0f), new Vector3(0.14f, 0.14f, 0.14f), brassMaterial);

        GameObject gauge = CreateLocalPrimitive("Pipeworks Routing Valve Gauge", PrimitiveType.Cylinder, valveRoot.transform, new Vector3(0.32f, 0.48f, -0.14f), new Vector3(0.2f, 0.03f, 0.2f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Pipeworks Routing Valve Gauge Needle", valveRoot.transform, new Vector3(0.37f, 0.48f, -0.18f), new Vector3(0.16f, 0.018f, 0.018f), warningMaterial);
        valve.lockedSignal = CreateLocalPrimitive("Pipeworks Routing Valve Locked Lamp", PrimitiveType.Sphere, valveRoot.transform, new Vector3(-0.34f, 0.48f, -0.16f), new Vector3(0.13f, 0.13f, 0.13f), warningMaterial);
        valve.ventedSignal = CreateLocalPrimitive("Pipeworks Routing Valve Vented Lamp", PrimitiveType.Sphere, valveRoot.transform, new Vector3(-0.34f, 0.48f, -0.17f), new Vector3(0.13f, 0.13f, 0.13f), steamMaterial);
        CreateLocalCube("Pipeworks Routing Valve Outlet Pipe", valveRoot.transform, new Vector3(0f, -0.52f, -0.12f), new Vector3(0.18f, 0.62f, 0.18f), brassMaterial);

        return valve;
    }

    private static void CreatePipeworksAnnexBlockout(Material wallMaterial, Material floorMaterial)
    {
        GameObject parent = new GameObject("Pipeworks Annex Blockout");

        CreateCube("Pipeworks Floor", new Vector3(0f, -0.1f, 11f), new Vector3(12f, 0.2f, 26f), floorMaterial, parent.transform);
        CreateCube("Pipeworks South Wall", new Vector3(0f, 1.5f, -2f), new Vector3(10.5f, 3f, 0.5f), wallMaterial, parent.transform);
        CreateCube("Pipeworks North Wall", new Vector3(0f, 1.5f, 24f), new Vector3(10.5f, 3f, 0.5f), wallMaterial, parent.transform);
        CreateCube("Pipeworks West Wall", new Vector3(-5.25f, 1.5f, 11f), new Vector3(0.5f, 3f, 26f), wallMaterial, parent.transform);
        CreateCube("Pipeworks East Wall", new Vector3(5.25f, 1.5f, 11f), new Vector3(0.5f, 3f, 26f), wallMaterial, parent.transform);
        CreateCube("Pipeworks Mid Left Baffle", new Vector3(-2.4f, 1.5f, 7f), new Vector3(0.5f, 3f, 5.2f), wallMaterial, parent.transform);
        CreateCube("Pipeworks Mid Right Baffle", new Vector3(2.4f, 1.5f, 15.5f), new Vector3(0.5f, 3f, 5.6f), wallMaterial, parent.transform);
    }

    private static void CreatePipeworksDressing(Material ironMaterial, Material floorPatchMaterial, Material brassMaterial, Material warningMaterial, Material gaugeFaceMaterial, Material steamMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Pipeworks Annex Dressing");
        CreateCube("Pipeworks Oil Patch A", new Vector3(-2.9f, 0.02f, 6.5f), new Vector3(1.6f, 0.04f, 1.2f), floorPatchMaterial, parent.transform);
        CreateCube("Pipeworks Oil Patch B", new Vector3(2.8f, 0.02f, 18.2f), new Vector3(1.8f, 0.04f, 1.4f), floorPatchMaterial, parent.transform);
        CreateCube("Pipeworks Left Pipe Run", new Vector3(-5.05f, 1.2f, 10f), new Vector3(0.16f, 0.16f, 18f), brassMaterial, parent.transform);
        CreateCube("Pipeworks Right Pipe Run", new Vector3(5.05f, 2f, 13f), new Vector3(0.14f, 0.14f, 16f), brassMaterial, parent.transform);
        CreateCube("Pipeworks Furnace Body", new Vector3(-4.55f, 0.75f, 16f), new Vector3(0.8f, 1.3f, 1.1f), ironMaterial, parent.transform);
        CreateCube("Pipeworks Furnace Glow", new Vector3(-4.1f, 0.75f, 16f), new Vector3(0.08f, 0.72f, 0.58f), glowMaterial, parent.transform);
        CreatePipeCanopy("North Star Pipeworks Pipe Canopy", new Vector3(0f, 2.82f, 12f), Quaternion.identity, 7.6f, brassMaterial, ironMaterial, parent.transform);
        CreateCagedGaslight("North Star Pipeworks Gaslight", new Vector3(4.86f, 2.02f, 9.2f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreateRivetBand("North Star Pipeworks Wall Rivet Band", new Vector3(-5.02f, 2.52f, 12.5f), Quaternion.Euler(0f, 90f, 0f), 4.8f, ironMaterial, brassMaterial, parent.transform);
        CreateWallPipeGaugeClusterPrototype("Pipeworks Prototype Wall Pipe Gauge Cluster", new Vector3(4.94f, 1.58f, 5.7f), Quaternion.Euler(0f, -90f, 0f), 1f, brassMaterial, ironMaterial, gaugeFaceMaterial, warningMaterial, "pipeworks_route_wall", parent.transform);
        CreateBoilerControlConsolePrototype("Pipeworks Prototype Boiler Control Console", new Vector3(-4.82f, 0.62f, 18.72f), Quaternion.Euler(0f, 90f, 0f), 0.96f, brassMaterial, ironMaterial, gaugeFaceMaterial, warningMaterial, "pipeworks_route_console", parent.transform);
        CreatePressureGauge("Pipeworks Gauge A", new Vector3(4.95f, 1.65f, 7f), Quaternion.Euler(0f, -90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateSteamVent("Pipeworks Steam Vent A", new Vector3(3.8f, 0.05f, 5.5f), brassMaterial, steamMaterial, parent.transform);
        CreatePipeBundle("Pipeworks Triple Pipe Bundle", new Vector3(0f, 2.35f, 23.72f), Quaternion.Euler(0f, 90f, 0f), 3.6f, brassMaterial, ironMaterial, parent.transform);
        CreateWorkOrderBoard("Work Order Board - Pipeworks", "PIPEWORKS NOTICE\nBOLT FEED LIVE\nKEEP DISTANCE", new Vector3(4.95f, 1.55f, 12f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateLorePlaque("Lore Plaque - Pipeworks Archive", "Pipeworks Archive", "The bolt feed carried messages before it carried weapons; now every valve-rifle repeats the shutdown command.", new Vector3(4.95f, 1.55f, 14.4f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
    }

    private static void Wall(string name, float x, float z, float length, bool horizontal, Material material, Transform parent)
    {
        Vector3 scale = horizontal ? new Vector3(length, 3f, 0.5f) : new Vector3(0.5f, 3f, length);
        CreateCube(name, new Vector3(x, 1.5f, z), scale, material, parent);
    }

    private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Material material, Transform parent = null)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = name;
        cube.transform.position = position;
        cube.transform.localScale = scale;

        if (parent != null)
        {
            cube.transform.SetParent(parent);
        }

        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }

        return cube;
    }

    private static Sprite LoadUiSprite(string relativePath, Vector4 spriteBorder = default(Vector4))
    {
        string path = UIHudTextureFolder + "/" + relativePath;
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
        {
            throw new FileNotFoundException("Missing UIHudV1 texture", path);
        }

        bool changed = importer.textureType != TextureImporterType.Sprite
            || importer.spriteImportMode != SpriteImportMode.Single
            || importer.mipmapEnabled
            || !importer.alphaIsTransparency
            || importer.textureCompression != TextureImporterCompression.Uncompressed
            || importer.filterMode != FilterMode.Bilinear
            || importer.spriteBorder != spriteBorder;

        if (changed)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.filterMode = FilterMode.Bilinear;
            importer.spritePixelsPerUnit = 100f;
            importer.spriteBorder = spriteBorder;
            importer.SaveAndReimport();
        }

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null)
        {
            throw new InvalidOperationException("UIHudV1 texture did not import as a sprite: " + path);
        }

        return sprite;
    }

    private static void ApplyUiSprite(Image image, string relativePath, Image.Type imageType, Vector4 spriteBorder = default(Vector4))
    {
        image.sprite = LoadUiSprite(relativePath, spriteBorder);
        image.type = imageType;
        image.color = Color.white;
        image.raycastTarget = false;
    }

    private static Image CreateAnchoredSpriteImage(string name, Transform parent, string relativePath, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 rectSize, Image.Type imageType = Image.Type.Simple, Vector4 spriteBorder = default(Vector4))
    {
        Image image = CreateAnchoredImage(name, parent, Color.white, anchorMin, anchorMax, pivot, anchoredPosition, rectSize, false);
        ApplyUiSprite(image, relativePath, imageType, spriteBorder);
        return image;
    }

    private static void CreateMainMenuScene(Material brassMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material glowMaterial, Material floorMaterial, PlatformQualityProfile qualityProfile)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.28f, 0.24f, 0.2f);

        GameObject cameraObject = new GameObject("Main Menu Camera");
        cameraObject.transform.position = new Vector3(0f, 1.85f, -7f);
        cameraObject.transform.rotation = Quaternion.Euler(8f, 0f, 0f);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.045f, 0.038f, 0.032f);

        GameObject lightObject = new GameObject("Menu Furnace Key Light");
        lightObject.transform.position = new Vector3(-1.8f, 3.2f, -3f);
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.58f, 0.16f);
        light.intensity = 3.5f;
        light.range = 8f;

        GameObject propRoot = new GameObject("Menu Brassworks Backdrop");
        CreateCube("Menu Oil Stone Floor", new Vector3(0f, -0.18f, -0.2f), new Vector3(8f, 0.18f, 5f), floorMaterial, propRoot.transform);
        CreateCube("Menu Iron Rear Plate", new Vector3(0f, 1.45f, 0.2f), new Vector3(7.4f, 3.2f, 0.2f), ironMaterial, propRoot.transform);
        CreateCube("Menu Brass Lower Pipe", new Vector3(0f, 0.55f, -0.05f), new Vector3(6.6f, 0.12f, 0.12f), brassMaterial, propRoot.transform);
        CreateCube("Menu Brass Upper Pipe", new Vector3(0f, 2.45f, -0.05f), new Vector3(6.2f, 0.1f, 0.1f), brassMaterial, propRoot.transform);

        GameObject menuGear = CreateLocalEmpty("Menu Center Gear Assembly", propRoot.transform, new Vector3(0f, 1.55f, -0.02f), Quaternion.Euler(90f, 0f, 0f));
        AddSpinner(menuGear, 12f);
        CreateLocalPrimitive("Menu Center Gear", PrimitiveType.Cylinder, menuGear.transform, Vector3.zero, new Vector3(0.85f, 0.07f, 0.85f), brassMaterial);
        for (int i = 0; i < 10; i++)
        {
            float angle = i * 36f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.88f, -0.03f, -Mathf.Cos(radians) * 0.88f);
            GameObject tooth = CreateLocalCube("Menu Gear Tooth " + i, menuGear.transform, toothPosition, new Vector3(0.18f, 0.09f, 0.2f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, -angle, 0f);
        }

        GameObject gauge = CreateLocalPrimitive("Menu Pressure Gauge", PrimitiveType.Cylinder, propRoot.transform, new Vector3(-2.35f, 1.65f, -0.12f), new Vector3(0.46f, 0.04f, 0.46f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Menu Gauge Needle", propRoot.transform, new Vector3(-2.24f, 1.65f, -0.16f), new Vector3(0.28f, 0.025f, 0.025f), glowMaterial);

        GameObject canvasObject = new GameObject("Main Menu Canvas");
        RuntimePerformanceProfile performanceProfile = canvasObject.AddComponent<RuntimePerformanceProfile>();
        performanceProfile.activeProfile = qualityProfile;
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        canvasObject.AddComponent<GraphicRaycaster>();

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
        {
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        CreateAnchoredImage("Menu Soot Vignette", canvasObject.transform, new Color(0.01f, 0.008f, 0.006f, 0.36f), Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, false);
        CreateAnchoredSpriteImage("Menu Brass Panel UIHudV1", canvasObject.transform, "Panels/PANEL_Menu_BrassPanel_768x384.png", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -74f), new Vector2(760f, 700f), Image.Type.Sliced, new Vector4(48f, 48f, 48f, 48f));
        CreateAnchoredSpriteImage("Menu Header Panel UIHudV1", canvasObject.transform, "Panels/PANEL_Menu_Header_768x96.png", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 214f), new Vector2(780f, 92f), Image.Type.Sliced, new Vector4(48f, 22f, 48f, 22f));
        Text menuTitle = CreateText("Menu Title", canvasObject.transform, font, GameBranding.WorkingTitle.ToUpperInvariant(), 58, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 212f), new Vector2(860f, 92f));
        ConfigureBestFit(menuTitle, 40, 58);
        Text subtitle = CreateText("Menu Subtitle", canvasObject.transform, font, "PRESSURE BELOW. BRASS ABOVE.", 24, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 154f), new Vector2(620f, 46f));
        subtitle.color = new Color(1f, 0.78f, 0.42f);

        MainMenuController mainMenu = canvasObject.AddComponent<MainMenuController>();
        mainMenu.startButton = CreatePauseButton("Start Button", "START GAME", canvasObject.transform, font, new Vector2(0f, 78f));
        mainMenu.quitButton = CreatePauseButton("Quit Button", "QUIT", canvasObject.transform, font, new Vector2(0f, 12f));
        mainMenu.resolutionButton = CreateSettingsButton("Menu Resolution Button", "RESOLUTION", canvasObject.transform, font, new Vector2(0f, -62f), out Text menuResolutionValue);
        mainMenu.resolutionValueText = menuResolutionValue;
        mainMenu.fullscreenToggle = CreateSettingsToggle("Menu Fullscreen Toggle", "FULLSCREEN", canvasObject.transform, font, new Vector2(0f, -114f), out Text menuFullscreenValue);
        mainMenu.fullscreenValueText = menuFullscreenValue;
        mainMenu.highContrastToggle = CreateSettingsToggle("Menu High Contrast Toggle", "CONTRAST", canvasObject.transform, font, new Vector2(0f, -166f), out Text menuHighContrastValue);
        mainMenu.highContrastValueText = menuHighContrastValue;
        mainMenu.sensitivitySlider = CreateSettingsSlider("Menu Sensitivity Slider", "MOUSE", canvasObject.transform, font, new Vector2(0f, -222f), 0.6f, 5f, GameSettings.DefaultMouseSensitivity, out Text menuSensitivityValue);
        mainMenu.sensitivityValueText = menuSensitivityValue;
        mainMenu.volumeSlider = CreateSettingsSlider("Menu Volume Slider", "VOLUME", canvasObject.transform, font, new Vector2(0f, -276f), 0f, 1f, GameSettings.DefaultMasterVolume, out Text menuVolumeValue);
        mainMenu.volumeValueText = menuVolumeValue;
        mainMenu.flashSlider = CreateSettingsSlider("Menu Flash Slider", "FLASH", canvasObject.transform, font, new Vector2(0f, -330f), GameSettings.MinFlashIntensity, GameSettings.MaxFlashIntensity, GameSettings.DefaultFlashIntensity, out Text menuFlashValue);
        mainMenu.flashValueText = menuFlashValue;
        CreateText("Menu Version", canvasObject.transform, font, GameBranding.BuildVersion, 18, TextAnchor.LowerRight, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-18f, 12f), new Vector2(220f, 32f));

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), MainMenuScenePath);
    }

    private static HUDController CreateHud()
    {
        GameObject canvasObject = new GameObject("HUD Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        canvasObject.AddComponent<GraphicRaycaster>();

        HUDController hud = canvasObject.AddComponent<HUDController>();

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
        {
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        hud.damageFlashImage = CreateScreenImage("Damage Flash", canvasObject.transform, new Color(1f, 0f, 0f, 0f));
        CreateAnchoredSpriteImage("Health Gauge Frame UIHudV1", canvasObject.transform, "Gauges/HUD_HealthGauge_Frame_512x96.png", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(14f, 14f), new Vector2(384f, 72f), Image.Type.Sliced, new Vector4(34f, 22f, 34f, 22f));
        hud.healthFillImage = CreateAnchoredSpriteImage("Health Gauge Fill UIHudV1", canvasObject.transform, "Gauges/HUD_HealthGauge_Fill_Red_384x32.png", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(32f, 24f), new Vector2(238f, 20f), Image.Type.Filled);
        hud.healthFillImage.fillMethod = Image.FillMethod.Horizontal;
        hud.healthFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        hud.healthFillImage.fillAmount = 1f;
        CreateAnchoredSpriteImage("Ammo Gauge Frame UIHudV1", canvasObject.transform, "Gauges/HUD_PressureAmmoGauge_Frame_512x96.png", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-14f, 14f), new Vector2(384f, 72f), Image.Type.Sliced, new Vector4(34f, 22f, 34f, 22f));
        hud.ammoFillImage = CreateAnchoredSpriteImage("Ammo Gauge Fill UIHudV1", canvasObject.transform, "Gauges/HUD_PressureAmmoGauge_Fill_Amber_384x32.png", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-32f, 24f), new Vector2(238f, 20f), Image.Type.Filled);
        hud.ammoFillImage.fillMethod = Image.FillMethod.Horizontal;
        hud.ammoFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        hud.ammoFillImage.fillAmount = 1f;
        CreateAnchoredImage("Gear Key Backplate", canvasObject.transform, new Color(0.16f, 0.085f, 0.035f, 0.86f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 14f), new Vector2(320f, 56f), false);
        hud.keyLampOffSprite = LoadUiSprite("Icons/HUD_KeyLamp_Off_96x96.png");
        hud.keyLampOnSprite = LoadUiSprite("Icons/HUD_KeyLamp_On_96x96.png");
        hud.keyLampDeniedSprite = LoadUiSprite("Icons/HUD_KeyLamp_Denied_96x96.png");
        hud.promptInteractSprite = LoadUiSprite("Icons/ICON_Prompt_InteractE_96x96.png");
        hud.promptGearKeySprite = LoadUiSprite("Icons/ICON_Prompt_GearKey_96x96.png");
        hud.promptValveSprite = LoadUiSprite("Icons/ICON_Prompt_Valve_96x96.png");
        hud.promptLiftSprite = LoadUiSprite("Icons/ICON_Prompt_Lift_96x96.png");
        hud.promptAmmoSprite = LoadUiSprite("Icons/ICON_Prompt_Ammo_96x96.png");
        hud.promptHealthSprite = LoadUiSprite("Icons/ICON_Prompt_Health_96x96.png");
        hud.promptWarningSprite = LoadUiSprite("Icons/ICON_Prompt_Warning_96x96.png");
        hud.promptSecretSprite = LoadUiSprite("Icons/ICON_Prompt_Secret_96x96.png");
        hud.promptPauseSprite = LoadUiSprite("Icons/ICON_Prompt_Pause_96x96.png");
        hud.promptMouseRightSprite = LoadUiSprite("Icons/ICON_Prompt_MouseRight_96x96.png");
        hud.keyLampImage = CreateAnchoredSpriteImage("Gear Key Lamp UIHudV1", canvasObject.transform, "Icons/HUD_KeyLamp_Off_96x96.png", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(-128f, 22f), new Vector2(44f, 44f));
        hud.objectiveBackplateImage = CreateAnchoredSpriteImage("Objective Backplate UIHudV1", canvasObject.transform, "Panels/HUD_ObjectiveBackplate_640x72.png", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(18f, -82f), new Vector2(640f, 72f), Image.Type.Sliced, new Vector4(32f, 20f, 32f, 20f));
        hud.bossBackplateImage = CreateAnchoredSpriteImage("Boss Gauge Frame UIHudV1", canvasObject.transform, "Gauges/HUD_BossPressureGauge_Frame_768x96.png", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -18f), new Vector2(640f, 78f), Image.Type.Sliced, new Vector4(42f, 22f, 42f, 22f));
        hud.bossFillImage = CreateAnchoredSpriteImage("Boss Gauge Fill UIHudV1", canvasObject.transform, "Gauges/HUD_BossPressureGauge_Fill_Red_704x24.png", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -58f), new Vector2(540f, 18f), Image.Type.Filled);
        hud.bossFillImage.fillMethod = Image.FillMethod.Horizontal;
        hud.bossFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        hud.bossFillImage.fillAmount = 1f;
        hud.healthText = CreateText("Health Text", canvasObject.transform, font, "HEALTH 100/100", 22, TextAnchor.LowerLeft, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(28f, 18f), new Vector2(360f, 52f));
        hud.ammoText = CreateText("Ammo Text", canvasObject.transform, font, "AMMO 30", 22, TextAnchor.LowerRight, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-28f, 18f), new Vector2(360f, 52f));
        hud.keyText = CreateText("Gear Key Text", canvasObject.transform, font, "GEAR KEY NO", 22, TextAnchor.LowerCenter, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 18f), new Vector2(300f, 45f));
        hud.objectiveText = CreateText("Objective Text", canvasObject.transform, font, string.Empty, 20, TextAnchor.UpperLeft, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(54f, -94f), new Vector2(560f, 44f));
        ConfigureReadableHudText(hud.objectiveText, 16, 20);
        hud.bossNameText = CreateText("Boss Name Text", canvasObject.transform, font, string.Empty, 20, TextAnchor.UpperCenter, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -20f), new Vector2(560f, 34f));
        ConfigureBestFit(hud.bossNameText, 16, 20);
        CreateAnchoredSpriteImage("Brass Reticle UIHudV1", canvasObject.transform, "Reticles/RETICLE_BrassCrosshair_64x64.png", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(46f, 46f));
        hud.interactionBackplateImage = CreateAnchoredSpriteImage("Interaction Prompt Backplate UIHudV1", canvasObject.transform, "Panels/HUD_PromptBackplate_640x80.png", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -102f), new Vector2(640f, 80f), Image.Type.Sliced, new Vector4(34f, 22f, 34f, 22f));
        hud.interactionIconImage = CreateAnchoredSpriteImage("Interaction Prompt Icon UIHudV1", canvasObject.transform, "Icons/ICON_Prompt_InteractE_96x96.png", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-274f, -101f), new Vector2(48f, 48f));
        hud.interactionText = CreateText("Interaction Prompt Text", canvasObject.transform, font, string.Empty, 24, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(30f, -101f), new Vector2(540f, 54f));
        ConfigureReadableHudText(hud.interactionText, 18, 24);
        hud.messageText = CreateText("Message Text", canvasObject.transform, font, string.Empty, 34, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 80f), new Vector2(760f, 220f));
        ConfigureReadableHudText(hud.messageText, 24, 34);
        hud.ClearObjective();
        hud.HideBossHealth();
        CreatePauseMenu(canvasObject.transform, font);

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();

        return hud;
    }

    private static PauseMenuController CreatePauseMenu(Transform canvasTransform, Font font)
    {
        GameObject controllerObject = new GameObject("Pause Menu Controller");
        controllerObject.transform.SetParent(canvasTransform, false);
        PauseMenuController pauseMenu = controllerObject.AddComponent<PauseMenuController>();

        GameObject root = new GameObject("Pause Menu");
        root.transform.SetParent(controllerObject.transform, false);

        RectTransform rootRect = root.AddComponent<RectTransform>();
        rootRect.anchorMin = Vector2.zero;
        rootRect.anchorMax = Vector2.one;
        rootRect.pivot = new Vector2(0.5f, 0.5f);
        rootRect.anchoredPosition = Vector2.zero;
        rootRect.sizeDelta = Vector2.zero;

        Image overlay = root.AddComponent<Image>();
        overlay.color = new Color(0.01f, 0.012f, 0.018f, 0.82f);

        CreateAnchoredSpriteImage("Pause Brass Panel UIHudV1", root.transform, "Panels/PANEL_Menu_BrassPanel_768x384.png", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -90f), new Vector2(720f, 720f), Image.Type.Sliced, new Vector4(48f, 48f, 48f, 48f));
        CreateAnchoredSpriteImage("Pause Header Panel UIHudV1", root.transform, "Panels/PANEL_Menu_Header_768x96.png", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 218f), new Vector2(700f, 88f), Image.Type.Sliced, new Vector4(48f, 22f, 48f, 22f));
        Text pauseTitle = CreateText("Pause Title", root.transform, font, GameBranding.WorkingTitle.ToUpperInvariant(), 42, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 218f), new Vector2(620f, 72f));
        ConfigureBestFit(pauseTitle, 32, 42);
        CreateText("Pause Subtitle", root.transform, font, "PRESSURE PAUSED", 24, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 168f), new Vector2(420f, 48f));

        pauseMenu.root = root;
        pauseMenu.resumeButton = CreatePauseButton("Resume Button", "RESUME", root.transform, font, new Vector2(0f, 88f));
        pauseMenu.restartButton = CreatePauseButton("Restart Button", "RESTART", root.transform, font, new Vector2(0f, 24f));
        pauseMenu.quitButton = CreatePauseButton("Quit Button", "QUIT", root.transform, font, new Vector2(0f, -40f));
        pauseMenu.resolutionButton = CreateSettingsButton("Pause Resolution Button", "RESOLUTION", root.transform, font, new Vector2(0f, -112f), out Text pauseResolutionValue);
        pauseMenu.resolutionValueText = pauseResolutionValue;
        pauseMenu.fullscreenToggle = CreateSettingsToggle("Pause Fullscreen Toggle", "FULLSCREEN", root.transform, font, new Vector2(0f, -164f), out Text pauseFullscreenValue);
        pauseMenu.fullscreenValueText = pauseFullscreenValue;
        pauseMenu.highContrastToggle = CreateSettingsToggle("Pause High Contrast Toggle", "CONTRAST", root.transform, font, new Vector2(0f, -216f), out Text pauseHighContrastValue);
        pauseMenu.highContrastValueText = pauseHighContrastValue;
        pauseMenu.sensitivitySlider = CreateSettingsSlider("Pause Sensitivity Slider", "MOUSE", root.transform, font, new Vector2(0f, -272f), 0.6f, 5f, GameSettings.DefaultMouseSensitivity, out Text pauseSensitivityValue);
        pauseMenu.sensitivityValueText = pauseSensitivityValue;
        pauseMenu.volumeSlider = CreateSettingsSlider("Pause Volume Slider", "VOLUME", root.transform, font, new Vector2(0f, -326f), 0f, 1f, GameSettings.DefaultMasterVolume, out Text pauseVolumeValue);
        pauseMenu.volumeValueText = pauseVolumeValue;
        pauseMenu.flashSlider = CreateSettingsSlider("Pause Flash Slider", "FLASH", root.transform, font, new Vector2(0f, -380f), GameSettings.MinFlashIntensity, GameSettings.MaxFlashIntensity, GameSettings.DefaultFlashIntensity, out Text pauseFlashValue);
        pauseMenu.flashValueText = pauseFlashValue;

        return pauseMenu;
    }

    private static Button CreatePauseButton(string name, string label, Transform parent, Font font, Vector2 anchoredPosition)
    {
        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(parent, false);

        RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(320f, 64f);

        Image image = buttonObject.AddComponent<Image>();
        ApplyUiSprite(image, "Panels/PANEL_Menu_Button_Normal_320x64.png", Image.Type.Sliced, new Vector4(20f, 16f, 20f, 16f));

        Button button = buttonObject.AddComponent<Button>();
        button.transition = Selectable.Transition.SpriteSwap;
        button.targetGraphic = image;
        button.spriteState = new SpriteState
        {
            highlightedSprite = LoadUiSprite("Panels/PANEL_Menu_Button_Hover_320x64.png", new Vector4(20f, 16f, 20f, 16f)),
            pressedSprite = LoadUiSprite("Panels/PANEL_Menu_Button_Pressed_320x64.png", new Vector4(20f, 16f, 20f, 16f)),
            selectedSprite = LoadUiSprite("Panels/PANEL_Menu_Button_Hover_320x64.png", new Vector4(20f, 16f, 20f, 16f))
        };

        Text buttonText = CreateText(name + " Text", buttonObject.transform, font, label, 24, TextAnchor.MiddleCenter, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        buttonText.color = new Color(1f, 0.84f, 0.48f);

        return button;
    }

    private static Button CreateSettingsButton(string name, string label, Transform parent, Font font, Vector2 anchoredPosition, out Text valueText)
    {
        CreateText(name + " Label", parent, font, label, 18, TextAnchor.MiddleLeft, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), anchoredPosition + new Vector2(-190f, 0f), new Vector2(150f, 34f)).color = new Color(1f, 0.84f, 0.48f);

        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(parent, false);

        RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(260f, 44f);

        Image image = buttonObject.AddComponent<Image>();
        ApplyUiSprite(image, "Panels/PANEL_Menu_Button_Normal_320x64.png", Image.Type.Sliced, new Vector4(20f, 16f, 20f, 16f));

        Button button = buttonObject.AddComponent<Button>();
        button.transition = Selectable.Transition.SpriteSwap;
        button.targetGraphic = image;
        button.spriteState = new SpriteState
        {
            highlightedSprite = LoadUiSprite("Panels/PANEL_Menu_Button_Hover_320x64.png", new Vector4(20f, 16f, 20f, 16f)),
            pressedSprite = LoadUiSprite("Panels/PANEL_Menu_Button_Pressed_320x64.png", new Vector4(20f, 16f, 20f, 16f)),
            selectedSprite = LoadUiSprite("Panels/PANEL_Menu_Button_Hover_320x64.png", new Vector4(20f, 16f, 20f, 16f))
        };

        valueText = CreateText(name + " Value", buttonObject.transform, font, string.Empty, 18, TextAnchor.MiddleCenter, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        valueText.color = new Color(1f, 0.84f, 0.48f);

        return button;
    }

    private static Toggle CreateSettingsToggle(string name, string label, Transform parent, Font font, Vector2 anchoredPosition, out Text valueText)
    {
        CreateText(name + " Label", parent, font, label, 18, TextAnchor.MiddleLeft, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), anchoredPosition + new Vector2(-190f, 0f), new Vector2(150f, 34f)).color = new Color(1f, 0.84f, 0.48f);

        GameObject toggleObject = new GameObject(name);
        toggleObject.transform.SetParent(parent, false);

        RectTransform toggleRect = toggleObject.AddComponent<RectTransform>();
        toggleRect.anchorMin = new Vector2(0.5f, 0.5f);
        toggleRect.anchorMax = new Vector2(0.5f, 0.5f);
        toggleRect.pivot = new Vector2(0.5f, 0.5f);
        toggleRect.anchoredPosition = anchoredPosition + new Vector2(-112f, 0f);
        toggleRect.sizeDelta = new Vector2(44f, 44f);

        Image backgroundImage = toggleObject.AddComponent<Image>();
        backgroundImage.color = new Color(0.08f, 0.052f, 0.03f, 0.96f);

        Toggle toggle = toggleObject.AddComponent<Toggle>();
        toggle.targetGraphic = backgroundImage;

        GameObject checkObject = new GameObject(name + " Brass Check");
        checkObject.transform.SetParent(toggleObject.transform, false);
        RectTransform checkRect = checkObject.AddComponent<RectTransform>();
        checkRect.anchorMin = new Vector2(0.5f, 0.5f);
        checkRect.anchorMax = new Vector2(0.5f, 0.5f);
        checkRect.pivot = new Vector2(0.5f, 0.5f);
        checkRect.anchoredPosition = Vector2.zero;
        checkRect.sizeDelta = new Vector2(28f, 28f);
        Image checkImage = checkObject.AddComponent<Image>();
        checkImage.color = new Color(0.95f, 0.58f, 0.16f, 1f);
        toggle.graphic = checkImage;
        toggle.isOn = GameSettings.DefaultFullscreen;

        valueText = CreateText(name + " Value", parent, font, string.Empty, 18, TextAnchor.MiddleRight, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), anchoredPosition + new Vector2(190f, 0f), new Vector2(120f, 34f));
        valueText.color = new Color(1f, 0.84f, 0.48f);

        return toggle;
    }

    private static Slider CreateSettingsSlider(string name, string label, Transform parent, Font font, Vector2 anchoredPosition, float minValue, float maxValue, float defaultValue, out Text valueText)
    {
        CreateText(name + " Label", parent, font, label, 18, TextAnchor.MiddleLeft, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), anchoredPosition + new Vector2(-190f, 0f), new Vector2(120f, 34f)).color = new Color(1f, 0.84f, 0.48f);
        valueText = CreateText(name + " Value", parent, font, string.Empty, 18, TextAnchor.MiddleRight, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), anchoredPosition + new Vector2(190f, 0f), new Vector2(120f, 34f));
        valueText.color = new Color(1f, 0.84f, 0.48f);

        GameObject sliderObject = new GameObject(name);
        sliderObject.transform.SetParent(parent, false);

        RectTransform sliderRect = sliderObject.AddComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0.5f, 0.5f);
        sliderRect.anchorMax = new Vector2(0.5f, 0.5f);
        sliderRect.pivot = new Vector2(0.5f, 0.5f);
        sliderRect.anchoredPosition = anchoredPosition;
        sliderRect.sizeDelta = new Vector2(360f, 40f);

        Slider slider = sliderObject.AddComponent<Slider>();
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = defaultValue;

        GameObject backgroundObject = new GameObject(name + " Track");
        backgroundObject.transform.SetParent(sliderObject.transform, false);
        RectTransform backgroundRect = backgroundObject.AddComponent<RectTransform>();
        backgroundRect.anchorMin = Vector2.zero;
        backgroundRect.anchorMax = Vector2.one;
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;
        Image backgroundImage = backgroundObject.AddComponent<Image>();
        ApplyUiSprite(backgroundImage, "Panels/PANEL_Menu_SliderTrack_360x40.png", Image.Type.Sliced, new Vector4(18f, 10f, 18f, 10f));

        GameObject fillAreaObject = new GameObject(name + " Fill Area");
        fillAreaObject.transform.SetParent(sliderObject.transform, false);
        RectTransform fillAreaRect = fillAreaObject.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0f, 0.35f);
        fillAreaRect.anchorMax = new Vector2(1f, 0.65f);
        fillAreaRect.offsetMin = Vector2.zero;
        fillAreaRect.offsetMax = Vector2.zero;

        GameObject fillObject = new GameObject(name + " Brass Fill");
        fillObject.transform.SetParent(fillAreaObject.transform, false);
        RectTransform fillRect = fillObject.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        Image fillImage = fillObject.AddComponent<Image>();
        fillImage.color = new Color(0.85f, 0.54f, 0.16f, 1f);

        GameObject handleObject = new GameObject(name + " Valve Handle");
        handleObject.transform.SetParent(sliderObject.transform, false);
        RectTransform handleRect = handleObject.AddComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(48f, 48f);
        Image handleImage = handleObject.AddComponent<Image>();
        ApplyUiSprite(handleImage, "Panels/PANEL_Menu_SliderHandle_48x48.png", Image.Type.Simple);

        slider.fillRect = fillRect;
        slider.handleRect = handleRect;
        slider.targetGraphic = handleImage;

        ColorBlock colors = slider.colors;
        colors.normalColor = handleImage.color;
        colors.highlightedColor = new Color(1f, 0.86f, 0.42f, 1f);
        colors.pressedColor = new Color(1f, 0.58f, 0.18f, 1f);
        slider.colors = colors;

        return slider;
    }

    private static Image CreateScreenImage(string name, Transform parent, Color color)
    {
        GameObject imageObject = new GameObject(name);
        imageObject.transform.SetParent(parent, false);

        RectTransform rectTransform = imageObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;

        Image image = imageObject.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;

        return image;
    }

    private static Image CreateAnchoredImage(string name, Transform parent, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 rectSize, bool fillHorizontal)
    {
        GameObject imageObject = new GameObject(name);
        imageObject.transform.SetParent(parent, false);

        RectTransform rectTransform = imageObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = pivot;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = rectSize;

        Image image = imageObject.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;

        if (fillHorizontal)
        {
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = (int)Image.OriginHorizontal.Left;
            image.fillAmount = 1f;
        }

        return image;
    }

    private static Text CreateText(string name, Transform parent, Font font, string value, int size, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 rectSize)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = pivot;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = rectSize;

        Text text = textObject.AddComponent<Text>();
        text.font = font;
        text.text = value;
        text.fontSize = size;
        text.alignment = alignment;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        Shadow shadow = textObject.AddComponent<Shadow>();
        shadow.effectColor = Color.black;
        shadow.effectDistance = new Vector2(2f, -2f);

        return text;
    }

    private static void ConfigureBestFit(Text text, int minSize, int maxSize)
    {
        if (text == null)
        {
            return;
        }

        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = minSize;
        text.resizeTextMaxSize = maxSize;
    }

    private static void ConfigureReadableHudText(Text text, int minSize, int maxSize)
    {
        if (text == null)
        {
            return;
        }

        ConfigureBestFit(text, minSize, maxSize);
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
    }

    private static void CreateGameState(HUDController hud, PlatformQualityProfile qualityProfile, string startMessage)
    {
        GameObject stateObject = new GameObject("Game State");
        GameStateController state = stateObject.AddComponent<GameStateController>();
        state.hud = hud;
        state.pauseMenu = UnityEngine.Object.FindAnyObjectByType<PauseMenuController>();
        state.startMessage = startMessage;
        stateObject.AddComponent<LevelTransitionController>();
        ConfigureSteamworksAudioV1(stateObject.AddComponent<SteamworksAudio>());
        RuntimePerformanceProfile performanceProfile = stateObject.AddComponent<RuntimePerformanceProfile>();
        performanceProfile.activeProfile = qualityProfile;
        stateObject.AddComponent<RuntimeSmokeTest>();
        stateObject.AddComponent<RuntimeAutoPlaythroughTest>();
        stateObject.AddComponent<RuntimeCombatTest>();
        stateObject.AddComponent<RuntimeCombatEdgeTest>();
        stateObject.AddComponent<RuntimeCombatScenarioTest>();
        stateObject.AddComponent<RuntimeRangedCombatTest>();
        stateObject.AddComponent<RuntimeBellowsNodeTest>();
        stateObject.AddComponent<RuntimeBulwarkCombatTest>();
        stateObject.AddComponent<RuntimeWardenCombatTest>();
        stateObject.AddComponent<RuntimeInteractionTest>();
        stateObject.AddComponent<RuntimeHazardTest>();
        stateObject.AddComponent<RuntimeSecretTest>();
        stateObject.AddComponent<RuntimePauseFlowTest>();
        stateObject.AddComponent<RuntimeWeaponSwitchTest>();
        stateObject.AddComponent<RuntimeMovementFeelTest>();
        stateObject.AddComponent<RuntimeBalanceEnvelopeTest>();
        stateObject.AddComponent<RuntimeLevel01FlowTest>();
        stateObject.AddComponent<RuntimeMidgameFlowTest>();
        stateObject.AddComponent<RuntimeClimaxFlowTest>();
        stateObject.AddComponent<RuntimeAudioMixTest>();
        stateObject.AddComponent<RuntimeDisplaySettingsTest>();
        stateObject.AddComponent<RuntimeReadabilitySettingsTest>();
    }

    private static void ConfigureSteamworksAudioV1(SteamworksAudio audio)
    {
        audio.preferAuthoredClips = true;
        audio.ambienceVolume = 0.24f;
        audio.authoredAmbienceLoop = LoadAudioV1Clip("AUDV1_AMB_BrassworksMix_loop.wav");
        audio.authoredCueClips = new[]
        {
            AudioBinding(SteamworksAudioCue.PressureFire, "AUDV1_WPN_PressurePistolFire.wav"),
            AudioBinding(SteamworksAudioCue.EmptyClick, "AUDV1_WPN_EmptyClick.wav"),
            AudioBinding(SteamworksAudioCue.HealthPickup, "AUDV1_PCK_HealthPickup.wav"),
            AudioBinding(SteamworksAudioCue.AmmoPickup, "AUDV1_PCK_AmmoPickup.wav"),
            AudioBinding(SteamworksAudioCue.GearKey, "AUDV1_PCK_GearKeyPickup.wav"),
            AudioBinding(SteamworksAudioCue.GateOpen, "AUDV1_INT_GateOpen.wav"),
            AudioBinding(SteamworksAudioCue.GateDenied, "AUDV1_INT_GateDenied.wav"),
            AudioBinding(SteamworksAudioCue.EnemyHit, "AUDV1_IMP_MachineHit.wav"),
            AudioBinding(SteamworksAudioCue.EnemyDeath, "AUDV1_IMP_MachineDeathShort.wav"),
            AudioBinding(SteamworksAudioCue.PlayerHurt, "AUDV1_PLR_PlayerHurt.wav"),
            AudioBinding(SteamworksAudioCue.Win, "AUDV1_INT_LiftActivate.wav"),
            AudioBinding(SteamworksAudioCue.SteamScattergunFire, "AUDV1_WPN_ScattergunBlast.wav"),
            AudioBinding(SteamworksAudioCue.BellowsNodePulse, "AUDV1_HAZ_BellowsNodePulse.wav"),
            AudioBinding(SteamworksAudioCue.WeaponPickup, "AUDV1_PCK_WeaponPickup.wav"),
            AudioBinding(SteamworksAudioCue.SteamScattergunSlug, "AUDV1_WPN_ScattergunSlug.wav"),
            AudioBinding(SteamworksAudioCue.PressureBurst, "AUDV1_WPN_PressureBurst.wav"),
            AudioBinding(SteamworksAudioCue.EnemyAttackTell, "AUDV1_ENY_ScrapperAttackTell.wav"),
            AudioBinding(SteamworksAudioCue.LancerFireTell, "AUDV1_ENY_LancerFireTell.wav"),
            AudioBinding(SteamworksAudioCue.BulwarkAttackTell, "AUDV1_ENY_BulwarkAttackTell.wav")
        };
        audio.mixBindings = new[]
        {
            AudioMix(SteamworksAudioCue.PressureFire, 0.86f, false),
            AudioMix(SteamworksAudioCue.EmptyClick, 0.5f, false),
            AudioMix(SteamworksAudioCue.HealthPickup, 0.64f, false),
            AudioMix(SteamworksAudioCue.AmmoPickup, 0.62f, false),
            AudioMix(SteamworksAudioCue.GearKey, 0.75f, false),
            AudioMix(SteamworksAudioCue.GateOpen, 0.86f, true),
            AudioMix(SteamworksAudioCue.GateDenied, 0.72f, false),
            AudioMix(SteamworksAudioCue.EnemyHit, 0.7f, true),
            AudioMix(SteamworksAudioCue.EnemyDeath, 0.82f, true),
            AudioMix(SteamworksAudioCue.PlayerHurt, 0.78f, false),
            AudioMix(SteamworksAudioCue.Win, 0.8f, false),
            AudioMix(SteamworksAudioCue.SteamScattergunFire, 0.95f, false),
            AudioMix(SteamworksAudioCue.BellowsNodePulse, 0.92f, true),
            AudioMix(SteamworksAudioCue.WeaponPickup, 0.78f, false),
            AudioMix(SteamworksAudioCue.SteamScattergunSlug, 0.86f, false),
            AudioMix(SteamworksAudioCue.PressureBurst, 0.98f, false),
            AudioMix(SteamworksAudioCue.EnemyAttackTell, 0.88f, true),
            AudioMix(SteamworksAudioCue.LancerFireTell, 0.9f, true),
            AudioMix(SteamworksAudioCue.BulwarkAttackTell, 0.96f, true)
        };
    }

    private static SteamworksAudioClipBinding AudioBinding(SteamworksAudioCue cue, string fileName)
    {
        return new SteamworksAudioClipBinding
        {
            cue = cue,
            clip = LoadAudioV1Clip(fileName)
        };
    }

    private static SteamworksAudioMixBinding AudioMix(SteamworksAudioCue cue, float volumeMultiplier, bool intendedSpatial)
    {
        return new SteamworksAudioMixBinding
        {
            cue = cue,
            volumeMultiplier = volumeMultiplier,
            intendedSpatial = intendedSpatial
        };
    }

    private static AudioClip LoadAudioV1Clip(string fileName)
    {
        string path = $"{AudioV1Folder}/{fileName}";
        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        if (clip == null)
        {
            throw new FileNotFoundException("Missing AudioV1 clip", path);
        }

        return clip;
    }

    private static void ApplyAudioV1ImportSettings()
    {
        string[] audioGuids = AssetDatabase.FindAssets("t:AudioClip", new[] { AudioV1Folder });
        for (int i = 0; i < audioGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(audioGuids[i]);
            if (!path.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            AudioImporter importer = AssetImporter.GetAtPath(path) as AudioImporter;
            if (importer == null)
            {
                continue;
            }

            bool isLoop = IsAudioV1Loop(path);
            AudioImporterSampleSettings settings = importer.defaultSampleSettings;
            AudioClipLoadType expectedLoadType = isLoop ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
            AudioCompressionFormat expectedCompression = isLoop ? AudioCompressionFormat.Vorbis : AudioCompressionFormat.ADPCM;
            float expectedQuality = isLoop ? 0.72f : 1f;

            bool changed = settings.loadType != expectedLoadType
                || settings.compressionFormat != expectedCompression
                || !Mathf.Approximately(settings.quality, expectedQuality)
                || settings.sampleRateSetting != AudioSampleRateSetting.PreserveSampleRate
                || settings.preloadAudioData == isLoop
                || importer.forceToMono
                || importer.loadInBackground != isLoop;

            if (!changed)
            {
                continue;
            }

            settings.loadType = expectedLoadType;
            settings.compressionFormat = expectedCompression;
            settings.quality = expectedQuality;
            settings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
            settings.preloadAudioData = !isLoop;
            importer.defaultSampleSettings = settings;
            importer.forceToMono = false;
            importer.loadInBackground = isLoop;
            importer.SaveAndReimport();
        }
    }

    private static bool IsAudioV1Loop(string path)
    {
        return path.IndexOf("_loop", StringComparison.OrdinalIgnoreCase) >= 0
            || path.IndexOf("Loop.wav", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static void CreatePlayer(Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material gaugeFaceMaterial, Material gaugeGlassMaterial, Material ironMaterial, Material warningMaterial, WeaponDefinition weaponDefinition, WeaponDefinition steamScattergunDefinition)
    {
        GameObject player = new GameObject("Player");
        player.transform.position = new Vector3(0f, 0f, 0f);

        CharacterController controller = player.AddComponent<CharacterController>();
        controller.height = 1.8f;
        controller.radius = 0.35f;
        controller.center = new Vector3(0f, 0.9f, 0f);

        GameObject cameraObject = new GameObject("Player Camera");
        cameraObject.transform.SetParent(player.transform);
        cameraObject.transform.localPosition = new Vector3(0f, 1.6f, 0f);
        cameraObject.transform.localRotation = Quaternion.identity;

        Camera camera = cameraObject.AddComponent<Camera>();
        camera.fieldOfView = 82f;
        camera.nearClipPlane = 0.05f;
        cameraObject.AddComponent<AudioListener>();

        PlayerController playerController = player.AddComponent<PlayerController>();
        playerController.playerCamera = cameraObject.transform;
        playerController.moveSpeed = GameBalance.PlayerMoveSpeed;

        PlayerHealth health = player.AddComponent<PlayerHealth>();
        health.maxHealth = 100;

        PlayerInventory inventory = player.AddComponent<PlayerInventory>();
        inventory.startingAmmo = GameBalance.StartingAmmo;
        PlayerInteraction interaction = player.AddComponent<PlayerInteraction>();
        interaction.viewTransform = cameraObject.transform;
        player.AddComponent<RunProgressApplier>();

        WeaponController weapon = player.AddComponent<WeaponController>();
        weapon.definition = weaponDefinition;
        weapon.steamScattergunDefinition = steamScattergunDefinition;
        weapon.aimCamera = camera;
        weapon.inventory = inventory;
        weapon.damage = GameBalance.PressurePistolDamage;
        weapon.ammoCost = GameBalance.PressurePistolAmmoCost;
        weapon.pelletCount = GameBalance.PressurePistolPelletCount;
        weapon.fireCooldown = GameBalance.PressurePistolCooldown;
        weapon.spread = GameBalance.PressurePistolSpread;
        weapon.secondaryDamage = GameBalance.PressureBurstDamage;
        weapon.secondaryPelletCount = GameBalance.PressureBurstPelletCount;
        weapon.secondaryAmmoCost = GameBalance.PressureBurstAmmoCost;
        weapon.secondaryCooldown = GameBalance.PressureBurstCooldown;
        weapon.secondaryRange = GameBalance.PressureBurstRange;
        weapon.secondarySpread = GameBalance.PressureBurstSpread;

        WeaponView weaponView = CreateWeaponView(cameraObject.transform, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, gaugeGlassMaterial, ironMaterial, warningMaterial);
        WeaponView scattergunView = CreateSteamScattergunView(cameraObject.transform, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, ironMaterial, warningMaterial);
        weapon.weaponView = weaponView;
        weapon.pressurePistolView = weaponView;
        weapon.steamScattergunView = scattergunView;
    }

    private static WeaponView CreateWeaponView(Transform cameraTransform, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material gaugeFaceMaterial, Material gaugeGlassMaterial, Material ironMaterial, Material warningMaterial)
    {
        GameObject weaponRoot = new GameObject("Pressure Pistol Viewmodel");
        weaponRoot.transform.SetParent(cameraTransform, false);
        weaponRoot.transform.localPosition = new Vector3(0.22f, -0.55f, 0.82f);
        weaponRoot.transform.localRotation = Quaternion.Euler(-2f, -4f, 0f);

        CreateLocalCube("Pressure Pistol Walnut Grip", weaponRoot.transform, new Vector3(-0.06f, -0.24f, -0.18f), new Vector3(0.22f, 0.44f, 0.18f), gunMaterial).transform.localRotation = Quaternion.Euler(-10f, 0f, 0f);
        CreateLocalCube("Pressure Pistol Iron Trigger Guard", weaponRoot.transform, new Vector3(0f, -0.16f, 0.04f), new Vector3(0.28f, 0.08f, 0.22f), ironMaterial);
        CreateLocalCube("Pressure Pistol Trigger", weaponRoot.transform, new Vector3(0f, -0.19f, 0.1f), new Vector3(0.05f, 0.18f, 0.04f), warningMaterial).transform.localRotation = Quaternion.Euler(-18f, 0f, 0f);
        CreateLocalCube("Pressure Pistol Brass Receiver", weaponRoot.transform, new Vector3(0f, 0.01f, 0.06f), new Vector3(0.46f, 0.22f, 0.48f), gunTrimMaterial);
        CreateLocalCube("Pressure Pistol Iron Backplate", weaponRoot.transform, new Vector3(0f, 0.01f, -0.23f), new Vector3(0.5f, 0.26f, 0.06f), ironMaterial);
        CreateLocalCube("Pressure Pistol Left Walnut Grip Plate", weaponRoot.transform, new Vector3(-0.15f, -0.24f, -0.18f), new Vector3(0.045f, 0.38f, 0.16f), gunMaterial);
        CreateLocalCube("Pressure Pistol Right Walnut Grip Plate", weaponRoot.transform, new Vector3(0.15f, -0.24f, -0.18f), new Vector3(0.045f, 0.38f, 0.16f), gunMaterial);

        GameObject upperBarrel = CreateLocalPrimitive("Pressure Pistol Upper Barrel", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0f, 0.09f, 0.46f), new Vector3(0.08f, 0.5f, 0.08f), gunTrimMaterial);
        upperBarrel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject lowerPressureTube = CreateLocalPrimitive("Pressure Pistol Lower Pressure Tube", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0f, -0.08f, 0.38f), new Vector3(0.12f, 0.42f, 0.12f), ironMaterial);
        lowerPressureTube.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject pressureTank = CreateLocalPrimitive("Pressure Pistol Pressure Tank", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0f, -0.18f, 0.28f), new Vector3(0.16f, 0.5f, 0.16f), ironMaterial);
        pressureTank.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        for (int i = 0; i < 3; i++)
        {
            float z = 0.02f + i * 0.25f;
            GameObject tankBand = CreateLocalPrimitive("Pressure Pistol Tank Brass Band " + i, PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0f, -0.18f, z), new Vector3(0.18f, 0.025f, 0.18f), gunTrimMaterial);
            tankBand.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        GameObject rearRing = CreateLocalPrimitive("Pressure Pistol Rear Barrel Ring", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0f, 0.09f, 0.2f), new Vector3(0.13f, 0.04f, 0.13f), ironMaterial);
        rearRing.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject frontRing = CreateLocalPrimitive("Pressure Pistol Front Barrel Ring", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0f, 0.09f, 0.66f), new Vector3(0.13f, 0.04f, 0.13f), ironMaterial);
        frontRing.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject muzzleCrown = CreateLocalPrimitive("Pressure Pistol Muzzle Crown", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0f, 0.09f, 0.78f), new Vector3(0.16f, 0.055f, 0.16f), gunTrimMaterial);
        muzzleCrown.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Pressure Pistol Rear Sight", weaponRoot.transform, new Vector3(0f, 0.23f, -0.08f), new Vector3(0.2f, 0.045f, 0.045f), ironMaterial);
        CreateLocalCube("Pressure Pistol Front Sight", weaponRoot.transform, new Vector3(0f, 0.25f, 0.7f), new Vector3(0.06f, 0.12f, 0.045f), ironMaterial);

        PressureGaugePrototype gaugePrototype = CreatePressureGaugePrototype("Pressure Pistol Prototype Pressure Gauge", weaponRoot.transform, new Vector3(-0.18f, 0.18f, 0.01f), 0.46f, gunTrimMaterial, ironMaterial, gaugeFaceMaterial, gaugeGlassMaterial, warningMaterial, "viewmodel");
        CreatePressureCoilPrototype("Pressure Pistol Prototype Copper Coil Pack", weaponRoot.transform, new Vector3(0.18f, 0.17f, 0.34f), 0.62f, gunTrimMaterial, ironMaterial, warningMaterial, "viewmodel");
        CreateLocalCube("Pressure Pistol Bolt Handle", weaponRoot.transform, new Vector3(0.31f, 0.02f, 0.05f), new Vector3(0.18f, 0.05f, 0.05f), ironMaterial);
        CreateLocalPrimitive("Pressure Pistol Bolt Knob", PrimitiveType.Sphere, weaponRoot.transform, new Vector3(0.42f, 0.02f, 0.05f), new Vector3(0.075f, 0.075f, 0.075f), gunTrimMaterial);
        GameObject pressureDumpLever = CreateLocalCube("Pressure Pistol Pressure Dump Lever", weaponRoot.transform, new Vector3(0.31f, 0.15f, 0.16f), new Vector3(0.06f, 0.2f, 0.035f), warningMaterial);
        pressureDumpLever.transform.localRotation = Quaternion.Euler(-8f, 0f, -12f);
        GameObject steamVent = CreateLocalPrimitive("Pressure Pistol Steam Vent Chimney", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0.16f, 0.25f, 0.16f), new Vector3(0.045f, 0.16f, 0.045f), ironMaterial);
        steamVent.transform.localRotation = Quaternion.identity;
        CreateLocalPrimitive("Pressure Pistol Steam Vent Cap", PrimitiveType.Sphere, weaponRoot.transform, new Vector3(0.16f, 0.42f, 0.16f), new Vector3(0.06f, 0.04f, 0.06f), gunTrimMaterial);
        GameObject reliefNozzle = CreateLocalPrimitive("Pressure Pistol Pressure Relief Nozzle", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0.26f, 0.28f, 0.2f), new Vector3(0.035f, 0.16f, 0.035f), ironMaterial);
        reliefNozzle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        GameObject valveWheel = CreateLocalPrimitive("Pressure Pistol Valve Wheel", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0.23f, 0.1f, 0.08f), new Vector3(0.12f, 0.025f, 0.12f), gunTrimMaterial);
        valveWheel.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        CreateLocalCube("Pressure Pistol Valve Spoke A", weaponRoot.transform, new Vector3(0.25f, 0.1f, 0.08f), new Vector3(0.04f, 0.22f, 0.025f), ironMaterial);
        CreateLocalCube("Pressure Pistol Valve Spoke B", weaponRoot.transform, new Vector3(0.25f, 0.1f, 0.08f), new Vector3(0.04f, 0.025f, 0.22f), ironMaterial);
        CreateLocalCube("Pressure Pistol Steam Pipe Left", weaponRoot.transform, new Vector3(-0.27f, -0.03f, 0.21f), new Vector3(0.05f, 0.07f, 0.48f), ironMaterial);
        CreateLocalCube("Pressure Pistol Steam Pipe Right", weaponRoot.transform, new Vector3(0.27f, -0.03f, 0.21f), new Vector3(0.05f, 0.07f, 0.48f), ironMaterial);
        CreateLocalCube("Pressure Pistol Red Pressure Line", weaponRoot.transform, new Vector3(-0.2f, -0.18f, 0.28f), new Vector3(0.045f, 0.045f, 0.56f), warningMaterial);
        for (int i = 0; i < 4; i++)
        {
            float x = i % 2 == 0 ? -0.2f : 0.2f;
            float y = i < 2 ? 0.11f : -0.08f;
            CreateLocalPrimitive("Pressure Pistol Receiver Rivet " + i, PrimitiveType.Sphere, weaponRoot.transform, new Vector3(x, y, -0.18f), new Vector3(0.035f, 0.035f, 0.035f), gunTrimMaterial);
        }

        GameObject pressureDumpFlash = CreateLocalPrimitive("Pressure Pistol Pressure Dump Flash", PrimitiveType.Sphere, weaponRoot.transform, new Vector3(0.4f, 0.28f, 0.2f), new Vector3(0.24f, 0.14f, 0.14f), muzzleFlashMaterial);
        pressureDumpFlash.SetActive(false);
        GameObject flash = CreateLocalCube("Muzzle Flash", weaponRoot.transform, new Vector3(0f, 0.09f, 0.91f), new Vector3(0.45f, 0.45f, 0.08f), muzzleFlashMaterial);
        flash.SetActive(false);

        WeaponView weaponView = weaponRoot.AddComponent<WeaponView>();
        weaponView.muzzleFlash = flash;
        weaponView.pressureDumpFlash = pressureDumpFlash;
        weaponView.pressureGaugeNeedle = gaugePrototype.needlePivot;
        weaponView.pressureValveWheel = valveWheel.transform;
        weaponView.pressureDumpLever = pressureDumpLever.transform;
        weaponView.pressureChamber = pressureTank.transform;
        weaponView.secondaryRecoilOffset = new Vector3(0f, -0.06f, -0.15f);
        weaponView.secondaryFlashDuration = 0.14f;
        return weaponView;
    }

    private static WeaponView CreateSteamScattergunView(Transform cameraTransform, Material gripMaterial, Material brassMaterial, Material muzzleFlashMaterial, Material gaugeFaceMaterial, Material ironMaterial, Material warningMaterial)
    {
        GameObject weaponRoot = new GameObject("Steam Scattergun Viewmodel");
        weaponRoot.transform.SetParent(cameraTransform, false);
        weaponRoot.transform.localPosition = new Vector3(0.2f, -0.58f, 0.78f);
        weaponRoot.transform.localRotation = Quaternion.Euler(-3f, -5f, 0f);

        CreateLocalCube("Steam Scattergun Walnut Stock", weaponRoot.transform, new Vector3(0f, -0.22f, -0.26f), new Vector3(0.28f, 0.34f, 0.54f), gripMaterial).transform.localRotation = Quaternion.Euler(-7f, 0f, 0f);
        CreateLocalCube("Steam Scattergun Iron Trigger Guard", weaponRoot.transform, new Vector3(0f, -0.2f, 0.1f), new Vector3(0.34f, 0.09f, 0.22f), ironMaterial);
        CreateLocalCube("Steam Scattergun Trigger", weaponRoot.transform, new Vector3(0f, -0.24f, 0.16f), new Vector3(0.055f, 0.17f, 0.04f), warningMaterial).transform.localRotation = Quaternion.Euler(-18f, 0f, 0f);
        CreateLocalCube("Steam Scattergun Brass Receiver", weaponRoot.transform, new Vector3(0f, 0.02f, 0.12f), new Vector3(0.58f, 0.28f, 0.46f), brassMaterial);
        CreateLocalCube("Steam Scattergun Iron Top Strap", weaponRoot.transform, new Vector3(0f, 0.2f, 0.15f), new Vector3(0.64f, 0.055f, 0.5f), ironMaterial);
        CreateLocalCube("Steam Scattergun Red Pressure Line", weaponRoot.transform, new Vector3(0.34f, -0.02f, 0.26f), new Vector3(0.045f, 0.055f, 0.74f), warningMaterial);

        for (int i = 0; i < 3; i++)
        {
            float x = (i - 1) * 0.17f;
            GameObject barrel = CreateLocalPrimitive("Steam Scattergun Barrel " + i, PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(x, 0.06f, 0.58f), new Vector3(0.07f, 0.72f, 0.07f), ironMaterial);
            barrel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            GameObject muzzleRing = CreateLocalPrimitive("Steam Scattergun Muzzle Ring " + i, PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(x, 0.06f, 0.96f), new Vector3(0.1f, 0.055f, 0.1f), brassMaterial);
            muzzleRing.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        GameObject pressureDrum = CreateLocalPrimitive("Steam Scattergun Pressure Drum", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(-0.39f, -0.02f, 0.08f), new Vector3(0.18f, 0.34f, 0.18f), ironMaterial);
        pressureDrum.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        GameObject drumBandA = CreateLocalPrimitive("Steam Scattergun Drum Brass Band A", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(-0.48f, -0.02f, 0.08f), new Vector3(0.2f, 0.025f, 0.2f), brassMaterial);
        drumBandA.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        GameObject drumBandB = CreateLocalPrimitive("Steam Scattergun Drum Brass Band B", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(-0.3f, -0.02f, 0.08f), new Vector3(0.2f, 0.025f, 0.2f), brassMaterial);
        drumBandB.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        GameObject gaugeBezel = CreateLocalPrimitive("Steam Scattergun Gauge Bezel", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0.28f, 0.22f, -0.02f), new Vector3(0.17f, 0.03f, 0.17f), brassMaterial);
        gaugeBezel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject gaugeFace = CreateLocalPrimitive("Steam Scattergun Gauge Face", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0.28f, 0.22f, -0.05f), new Vector3(0.14f, 0.02f, 0.14f), gaugeFaceMaterial);
        gaugeFace.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Steam Scattergun Gauge Needle", weaponRoot.transform, new Vector3(0.32f, 0.22f, -0.075f), new Vector3(0.11f, 0.012f, 0.012f), warningMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, 28f);
        CreateLocalCube("Steam Scattergun Pump Handle", weaponRoot.transform, new Vector3(0f, -0.11f, 0.54f), new Vector3(0.42f, 0.08f, 0.18f), gripMaterial);

        for (int i = 0; i < 6; i++)
        {
            float x = i % 2 == 0 ? -0.25f : 0.25f;
            float z = -0.04f + (i / 2) * 0.16f;
            CreateLocalPrimitive("Steam Scattergun Receiver Rivet " + i, PrimitiveType.Sphere, weaponRoot.transform, new Vector3(x, 0.1f, z), new Vector3(0.035f, 0.035f, 0.035f), brassMaterial);
        }

        GameObject flash = CreateLocalCube("Steam Scattergun Muzzle Flash", weaponRoot.transform, new Vector3(0f, 0.06f, 1.12f), new Vector3(0.72f, 0.54f, 0.1f), muzzleFlashMaterial);
        flash.SetActive(false);

        WeaponView weaponView = weaponRoot.AddComponent<WeaponView>();
        weaponView.muzzleFlash = flash;
        weaponView.recoilOffset = new Vector3(0f, -0.045f, -0.14f);
        weaponView.flashDuration = 0.075f;
        return weaponView;
    }

    private static void CreateEnemy(string name, Vector3 position, Material material, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material bladeMaterial, EnemyDefinition definition)
    {
        GameObject enemy = new GameObject(name);
        enemy.name = name;
        enemy.transform.position = position;

        CreateScrapperVisual(enemy.transform, material, eyeMaterial, brassMaterial, ironMaterial, bladeMaterial);
        AddMachineMotion(enemy, 1f,
            "Scrapper Boiler Torso",
            new[] { "Scrapper Left Piston Arm", "Scrapper Left Cutter", "Scrapper Left Leg", "Scrapper Left Foot" },
            new[] { "Scrapper Right Piston Arm", "Scrapper Right Cutter", "Scrapper Right Leg", "Scrapper Right Foot" },
            new[] { "Scrapper Furnace Eye", "Scrapper Pressure Tank" });
        ScrapperAttackTellVfx attackTell = enemy.AddComponent<ScrapperAttackTellVfx>();
        attackTell.leftCutter = enemy.transform.Find("Scrapper Left Cutter");
        attackTell.rightCutter = enemy.transform.Find("Scrapper Right Cutter");
        attackTell.furnaceEye = enemy.transform.Find("Scrapper Furnace Eye");
        attackTell.pressureTank = enemy.transform.Find("Scrapper Pressure Tank");

        CharacterController controller = enemy.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.42f;
        controller.center = Vector3.zero;

        EnemyController enemyController = enemy.AddComponent<EnemyController>();
        enemyController.definition = definition;
        enemyController.attackTellVfx = attackTell;
        enemyController.maxHealth = GameBalance.ScrapperHealth;
        enemyController.moveSpeed = GameBalance.ScrapperMoveSpeed;
        enemyController.detectionRange = GameBalance.ScrapperDetectionRange;
        enemyController.attackRange = GameBalance.ScrapperAttackRange;
        enemyController.attackDamage = GameBalance.ScrapperAttackDamage;
        enemyController.attackCooldown = GameBalance.ScrapperAttackCooldown;
        enemyController.attackWindup = GameBalance.ScrapperAttackWindup;
        enemyController.obstacleProbeDistance = GameBalance.ScrapperObstacleProbeDistance;
    }

    private static void CreateLancerEnemy(string name, Vector3 position, Material material, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial, EnemyDefinition definition)
    {
        GameObject enemy = new GameObject(name);
        enemy.transform.position = position;

        Transform muzzle = CreateLancerVisual(enemy.transform, material, eyeMaterial, brassMaterial, ironMaterial, warningMaterial);
        AddMachineMotion(enemy, 0.85f,
            "Lancer Narrow Boiler Torso",
            new[] { "Lancer Left Tripod Leg" },
            new[] { "Lancer Right Tripod Leg", "Lancer Valve Rifle Barrel" },
            new[] { "Lancer Furnace Lens", "Lancer Hot Pressure Coil", "Lancer Back Pressure Tank" });
        LancerFireTellVfx fireTell = enemy.AddComponent<LancerFireTellVfx>();
        fireTell.muzzle = muzzle;
        fireTell.hotPressureCoil = enemy.transform.Find("Lancer Hot Pressure Coil");
        fireTell.furnaceLens = enemy.transform.Find("Lancer Furnace Lens");
        fireTell.backPressureTank = enemy.transform.Find("Lancer Back Pressure Tank");

        CharacterController controller = enemy.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.36f;
        controller.center = Vector3.zero;

        RangedEnemyController ranged = enemy.AddComponent<RangedEnemyController>();
        ranged.definition = definition;
        ranged.muzzle = muzzle;
        ranged.fireTellVfx = fireTell;
        ranged.maxHealth = GameBalance.LancerHealth;
        ranged.detectionRange = GameBalance.LancerDetectionRange;
        ranged.fireRange = GameBalance.LancerFireRange;
        ranged.moveSpeed = GameBalance.LancerMoveSpeed;
        ranged.fireCooldown = GameBalance.LancerFireCooldown;
        ranged.fireWindup = GameBalance.LancerFireWindup;
        ranged.projectileDamage = GameBalance.LancerProjectileDamage;
        ranged.projectileSpeed = GameBalance.LancerProjectileSpeed;
    }

    private static void CreateBellowsNodeEnemy(string name, Vector3 position, Material material, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial, EnemyDefinition definition)
    {
        GameObject enemy = new GameObject(name);
        enemy.transform.position = position;

        CreateBellowsNodeVisual(enemy.transform, material, eyeMaterial, brassMaterial, ironMaterial, warningMaterial);
        AddMachineMotion(enemy, 0.7f,
            "Bellows Node Brass Bellows Body",
            new[] { "Bellows Node Left Clamp", "Bellows Node Left Pipe" },
            new[] { "Bellows Node Right Clamp", "Bellows Node Right Pipe" },
            new[] { "Bellows Node Furnace Lens", "Bellows Node Pressure Bladder", "Bellows Node Exhaust Horn" });

        CharacterController controller = enemy.AddComponent<CharacterController>();
        controller.height = 1.7f;
        controller.radius = 0.5f;
        controller.center = Vector3.zero;

        BellowsNodeController node = enemy.AddComponent<BellowsNodeController>();
        node.definition = definition;
        node.maxHealth = GameBalance.BellowsNodeHealth;
        node.detectionRange = GameBalance.BellowsNodeDetectionRange;
        node.pulseRange = GameBalance.BellowsNodePulseRange;
        node.pulseDamage = GameBalance.BellowsNodePulseDamage;
        node.pulseCooldown = GameBalance.BellowsNodePulseCooldown;
        node.pulseWindup = GameBalance.BellowsNodePulseWindup;
        node.boostDuration = GameBalance.BellowsNodeBoostDuration;
        node.boostMultiplier = GameBalance.BellowsNodeBoostMultiplier;
    }

    private static void CreateBulwarkEnemy(string name, Vector3 position, Material material, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial, EnemyDefinition definition)
    {
        GameObject enemy = new GameObject(name);
        enemy.transform.position = position;

        CreateBulwarkVisual(enemy.transform, material, eyeMaterial, brassMaterial, ironMaterial, warningMaterial);
        AddMachineMotion(enemy, 1.25f,
            "Bulwark Riveted Boiler Body",
            new[] { "Bulwark Left Hammer Arm", "Bulwark Left Hammer Head", "Bulwark Left Piston Leg", "Bulwark Left Heavy Foot" },
            new[] { "Bulwark Right Hammer Arm", "Bulwark Right Hammer Head", "Bulwark Right Piston Leg", "Bulwark Right Heavy Foot" },
            new[] { "Bulwark Furnace Belly", "Bulwark Back Pressure Tank" });
        BulwarkAttackTellVfx attackTell = enemy.AddComponent<BulwarkAttackTellVfx>();
        attackTell.leftHammerArm = enemy.transform.Find("Bulwark Left Hammer Arm");
        attackTell.rightHammerArm = enemy.transform.Find("Bulwark Right Hammer Arm");
        attackTell.leftHammerHead = enemy.transform.Find("Bulwark Left Hammer Head");
        attackTell.rightHammerHead = enemy.transform.Find("Bulwark Right Hammer Head");
        attackTell.furnaceBelly = enemy.transform.Find("Bulwark Furnace Belly");
        attackTell.backPressureTank = enemy.transform.Find("Bulwark Back Pressure Tank");

        CharacterController controller = enemy.AddComponent<CharacterController>();
        controller.height = 2.35f;
        controller.radius = 0.55f;
        controller.center = Vector3.zero;

        BulwarkEnemyController bulwark = enemy.AddComponent<BulwarkEnemyController>();
        bulwark.definition = definition;
        bulwark.attackTellVfx = attackTell;
        bulwark.maxHealth = GameBalance.BulwarkHealth;
        bulwark.detectionRange = GameBalance.BulwarkDetectionRange;
        bulwark.moveSpeed = GameBalance.BulwarkMoveSpeed;
        bulwark.attackRange = GameBalance.BulwarkAttackRange;
        bulwark.attackDamage = GameBalance.BulwarkAttackDamage;
        bulwark.attackCooldown = GameBalance.BulwarkAttackCooldown;
        bulwark.attackWindup = GameBalance.BulwarkAttackWindup;
    }

    private static GovernorWardenController CreateGovernorWardenEnemy(string name, Vector3 position, Material material, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial, EnemyDefinition definition)
    {
        GameObject enemy = new GameObject(name);
        enemy.transform.position = position;

        Transform muzzle = CreateGovernorWardenVisual(enemy.transform, material, eyeMaterial, brassMaterial, ironMaterial, warningMaterial);
        AddMachineMotion(enemy, 1.55f,
            "Governor Warden Core Body",
            new[] { "Governor Warden Left Piston Arm", "Governor Warden Left Stomp Plate" },
            new[] { "Governor Warden Right Piston Arm", "Governor Warden Right Stomp Plate", "Governor Warden Pressure Cannon Muzzle" },
            new[] { "Governor Warden Furnace Heart", "Governor Warden Pressure Crown", "Governor Warden Back Boiler" });

        CharacterController controller = enemy.AddComponent<CharacterController>();
        controller.height = 2.9f;
        controller.radius = 0.8f;
        controller.center = Vector3.zero;

        GovernorWardenController warden = enemy.AddComponent<GovernorWardenController>();
        warden.definition = definition;
        warden.muzzle = muzzle;
        warden.maxHealth = GameBalance.GovernorWardenHealth;
        warden.detectionRange = GameBalance.GovernorWardenDetectionRange;
        warden.moveSpeed = GameBalance.GovernorWardenMoveSpeed;
        warden.stompRange = GameBalance.GovernorWardenStompRange;
        warden.stompDamage = GameBalance.GovernorWardenStompDamage;
        warden.stompCooldown = GameBalance.GovernorWardenStompCooldown;
        warden.stompWindup = GameBalance.GovernorWardenStompWindup;
        warden.fireRange = GameBalance.GovernorWardenFireRange;
        warden.fireCooldown = GameBalance.GovernorWardenFireCooldown;
        warden.fireWindup = GameBalance.GovernorWardenFireWindup;
        warden.projectileDamage = GameBalance.GovernorWardenProjectileDamage;
        warden.projectileSpeed = GameBalance.GovernorWardenProjectileSpeed;
        return warden;
    }

    private static void AddMachineMotion(GameObject machine, float scale, string bodyName, string[] leftPartNames, string[] rightPartNames, string[] pulsePartNames)
    {
        MachineMotionVfx motion = machine.AddComponent<MachineMotionVfx>();
        motion.body = machine.transform.Find(bodyName);
        motion.leftMotionParts = FindDirectChildren(machine.transform, leftPartNames);
        motion.rightMotionParts = FindDirectChildren(machine.transform, rightPartNames);
        motion.pulseParts = FindDirectChildren(machine.transform, pulsePartNames);
        motion.idleBob = 0.035f * scale;
        motion.swingDegrees = 7f * scale;
        motion.pressurePulse = 0.035f * scale;
    }

    private static Transform[] FindDirectChildren(Transform parent, string[] names)
    {
        Transform[] children = new Transform[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            children[i] = parent.Find(names[i]);
        }

        return children;
    }

    private static GuardianDefeatObjective CreateGovernorWardenDefeatObjective(GovernorWardenController target, Material ironMaterial, Material brassMaterial, Material warningMaterial)
    {
        GameObject objective = new GameObject("Governor Warden Defeat Objective");
        objective.transform.position = new Vector3(0f, 1.5f, 27.35f);

        GuardianDefeatObjective guardianObjective = objective.AddComponent<GuardianDefeatObjective>();
        guardianObjective.target = target;
        guardianObjective.completeMessage = "Governor Warden destroyed. Master override hoist unlocked.";
        guardianObjective.lockedSignal = CreateLocalCube("Governor Warden Lock Red Signal", objective.transform, new Vector3(-0.28f, 0f, 0f), new Vector3(0.18f, 0.18f, 0.18f), warningMaterial);
        guardianObjective.clearedSignal = CreateLocalCube("Governor Warden Lock Green Signal", objective.transform, new Vector3(0.28f, 0f, 0f), new Vector3(0.18f, 0.18f, 0.18f), brassMaterial);
        CreateLocalCube("Governor Warden Lock Iron Backplate", objective.transform, new Vector3(0f, 0f, -0.05f), new Vector3(0.86f, 0.32f, 0.08f), ironMaterial);
        guardianObjective.lockedSignal.SetActive(true);
        guardianObjective.clearedSignal.SetActive(false);
        return guardianObjective;
    }

    private static void CreateBellowsNodeVisual(Transform parent, Material bodyMaterial, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial)
    {
        CreateLocalCube("Bellows Node Brass Bellows Body", parent, new Vector3(0f, 0.05f, 0f), new Vector3(0.86f, 0.72f, 0.62f), brassMaterial);
        CreateLocalCube("Bellows Node Furnace Lens", parent, new Vector3(0f, 0.18f, 0.36f), new Vector3(0.48f, 0.22f, 0.08f), eyeMaterial);
        CreateLocalPrimitive("Bellows Node Pressure Bladder", PrimitiveType.Sphere, parent, new Vector3(0f, 0.3f, -0.36f), new Vector3(0.58f, 0.42f, 0.36f), bodyMaterial);
        CreateLocalPrimitive("Bellows Node Exhaust Horn", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.36f, 0.72f), new Vector3(0.18f, 0.42f, 0.18f), warningMaterial).transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalPrimitive("Bellows Node Top Pressure Gauge", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.72f, 0.08f), new Vector3(0.22f, 0.04f, 0.22f), ironMaterial);

        for (int i = 0; i < 5; i++)
        {
            float z = -0.22f + i * 0.12f;
            CreateLocalCube("Bellows Node Accordion Rib " + i, parent, new Vector3(0f, 0.05f, z), new Vector3(0.96f, 0.08f, 0.045f), ironMaterial);
        }

        CreateLocalCube("Bellows Node Left Clamp", parent, new Vector3(-0.56f, 0.04f, 0f), new Vector3(0.14f, 0.7f, 0.2f), ironMaterial);
        CreateLocalCube("Bellows Node Right Clamp", parent, new Vector3(0.56f, 0.04f, 0f), new Vector3(0.14f, 0.7f, 0.2f), ironMaterial);
        CreateLocalCube("Bellows Node Left Pipe", parent, new Vector3(-0.72f, 0.24f, -0.18f), new Vector3(0.13f, 0.16f, 0.68f), brassMaterial).transform.localRotation = Quaternion.Euler(0f, -8f, 0f);
        CreateLocalCube("Bellows Node Right Pipe", parent, new Vector3(0.72f, 0.24f, -0.18f), new Vector3(0.13f, 0.16f, 0.68f), brassMaterial).transform.localRotation = Quaternion.Euler(0f, 8f, 0f);
        CreateLocalCube("Bellows Node Iron Anchor Foot", parent, new Vector3(0f, -0.44f, 0f), new Vector3(1.12f, 0.16f, 0.82f), ironMaterial);
        CreateLocalCube("Bellows Node Warning Pressure Line", parent, new Vector3(0f, -0.18f, 0.42f), new Vector3(0.62f, 0.06f, 0.06f), warningMaterial);
    }

    private static void CreateBulwarkVisual(Transform parent, Material bodyMaterial, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial)
    {
        CreateLocalCube("Bulwark Riveted Boiler Body", parent, new Vector3(0f, 0.1f, 0f), new Vector3(1.05f, 1.22f, 0.74f), bodyMaterial);
        CreateLocalCube("Bulwark Furnace Belly", parent, new Vector3(0f, -0.08f, 0.43f), new Vector3(0.74f, 0.48f, 0.08f), eyeMaterial);
        CreateLocalCube("Bulwark Brass Chest Clamp", parent, new Vector3(0f, 0.42f, 0.45f), new Vector3(0.86f, 0.2f, 0.08f), brassMaterial);
        CreateLocalCube("Bulwark Armored Brow", parent, new Vector3(0f, 0.82f, 0.44f), new Vector3(0.68f, 0.18f, 0.09f), ironMaterial);
        CreateLocalPrimitive("Bulwark Back Pressure Tank", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.12f, -0.48f), new Vector3(0.36f, 0.78f, 0.36f), ironMaterial).transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Bulwark Left Shoulder Plate", parent, new Vector3(-0.78f, 0.34f, 0.02f), new Vector3(0.34f, 0.28f, 0.34f), brassMaterial);
        CreateLocalCube("Bulwark Right Shoulder Plate", parent, new Vector3(0.78f, 0.34f, 0.02f), new Vector3(0.34f, 0.28f, 0.34f), brassMaterial);
        CreateLocalCube("Bulwark Left Hammer Arm", parent, new Vector3(-0.96f, -0.22f, 0.18f), new Vector3(0.2f, 0.88f, 0.2f), ironMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, -8f);
        CreateLocalCube("Bulwark Right Hammer Arm", parent, new Vector3(0.96f, -0.22f, 0.18f), new Vector3(0.2f, 0.88f, 0.2f), ironMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, 8f);
        CreateLocalCube("Bulwark Left Hammer Head", parent, new Vector3(-1.06f, -0.82f, 0.34f), new Vector3(0.42f, 0.26f, 0.34f), warningMaterial);
        CreateLocalCube("Bulwark Right Hammer Head", parent, new Vector3(1.06f, -0.82f, 0.34f), new Vector3(0.42f, 0.26f, 0.34f), warningMaterial);
        CreateLocalCube("Bulwark Left Piston Leg", parent, new Vector3(-0.36f, -0.78f, 0f), new Vector3(0.26f, 0.7f, 0.26f), ironMaterial);
        CreateLocalCube("Bulwark Right Piston Leg", parent, new Vector3(0.36f, -0.78f, 0f), new Vector3(0.26f, 0.7f, 0.26f), ironMaterial);
        CreateLocalCube("Bulwark Left Heavy Foot", parent, new Vector3(-0.36f, -1.2f, 0.18f), new Vector3(0.5f, 0.16f, 0.48f), brassMaterial);
        CreateLocalCube("Bulwark Right Heavy Foot", parent, new Vector3(0.36f, -1.2f, 0.18f), new Vector3(0.5f, 0.16f, 0.48f), brassMaterial);
    }

    private static Transform CreateGovernorWardenVisual(Transform parent, Material bodyMaterial, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial)
    {
        CreateLocalCube("Governor Warden Core Body", parent, new Vector3(0f, 0.08f, 0f), new Vector3(1.45f, 1.7f, 0.95f), bodyMaterial);
        CreateLocalCube("Governor Warden Furnace Heart", parent, new Vector3(0f, 0.08f, 0.54f), new Vector3(0.82f, 0.62f, 0.08f), eyeMaterial);
        CreateLocalCube("Governor Warden Brass Rib Clamp", parent, new Vector3(0f, 0.62f, 0.58f), new Vector3(1.18f, 0.18f, 0.08f), brassMaterial);
        CreateLocalCube("Governor Warden Iron Jaw", parent, new Vector3(0f, -0.58f, 0.56f), new Vector3(1.05f, 0.24f, 0.1f), ironMaterial);
        CreateLocalPrimitive("Governor Warden Pressure Crown", PrimitiveType.Cylinder, parent, new Vector3(0f, 1.12f, 0f), new Vector3(0.72f, 0.22f, 0.72f), brassMaterial);
        CreateLocalPrimitive("Governor Warden Back Boiler", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.1f, -0.62f), new Vector3(0.58f, 1.35f, 0.58f), ironMaterial).transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Governor Warden Left Shoulder Governor", parent, new Vector3(-1.05f, 0.5f, 0.04f), new Vector3(0.38f, 0.48f, 0.42f), brassMaterial);
        CreateLocalCube("Governor Warden Right Shoulder Governor", parent, new Vector3(1.05f, 0.5f, 0.04f), new Vector3(0.38f, 0.48f, 0.42f), brassMaterial);
        CreateLocalCube("Governor Warden Left Piston Arm", parent, new Vector3(-1.32f, -0.18f, 0.18f), new Vector3(0.24f, 1.2f, 0.24f), ironMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, -7f);
        CreateLocalCube("Governor Warden Right Piston Arm", parent, new Vector3(1.32f, -0.18f, 0.18f), new Vector3(0.24f, 1.2f, 0.24f), ironMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, 7f);
        CreateLocalCube("Governor Warden Left Stomp Plate", parent, new Vector3(-0.48f, -1.32f, 0.2f), new Vector3(0.62f, 0.18f, 0.62f), warningMaterial);
        CreateLocalCube("Governor Warden Right Stomp Plate", parent, new Vector3(0.48f, -1.32f, 0.2f), new Vector3(0.62f, 0.18f, 0.62f), warningMaterial);
        CreateLocalCube("Governor Warden Spine Pressure Pipe", parent, new Vector3(0f, 0.82f, -0.82f), new Vector3(0.22f, 1.1f, 0.16f), brassMaterial);
        GameObject muzzle = CreateLocalPrimitive("Governor Warden Pressure Cannon Muzzle", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.42f, 0.86f), new Vector3(0.16f, 0.42f, 0.16f), warningMaterial);
        muzzle.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        return muzzle.transform;
    }

    private static Transform CreateLancerVisual(Transform parent, Material bodyMaterial, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial)
    {
        CreateLocalCube("Lancer Narrow Boiler Torso", parent, new Vector3(0f, 0.04f, 0f), new Vector3(0.46f, 0.92f, 0.38f), bodyMaterial);
        CreateLocalCube("Lancer Brass Rib Plate", parent, new Vector3(0f, 0.08f, 0.23f), new Vector3(0.34f, 0.64f, 0.06f), brassMaterial);
        CreateLocalCube("Lancer Furnace Lens", parent, new Vector3(0f, 0.56f, 0.27f), new Vector3(0.28f, 0.12f, 0.06f), eyeMaterial);
        CreateLocalPrimitive("Lancer Back Pressure Tank", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.06f, -0.28f), new Vector3(0.2f, 0.58f, 0.2f), ironMaterial).transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Lancer Left Tripod Leg", parent, new Vector3(-0.25f, -0.58f, 0.02f), new Vector3(0.12f, 0.62f, 0.12f), ironMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, -10f);
        CreateLocalCube("Lancer Right Tripod Leg", parent, new Vector3(0.25f, -0.58f, 0.02f), new Vector3(0.12f, 0.62f, 0.12f), ironMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, 10f);
        CreateLocalCube("Lancer Rear Tripod Leg", parent, new Vector3(0f, -0.58f, -0.22f), new Vector3(0.12f, 0.58f, 0.12f), ironMaterial);
        CreateLocalCube("Lancer Rifle Stock", parent, new Vector3(0.36f, 0.08f, 0.08f), new Vector3(0.22f, 0.16f, 0.3f), brassMaterial);
        GameObject barrel = CreateLocalPrimitive("Lancer Valve Rifle Barrel", PrimitiveType.Cylinder, parent, new Vector3(0.36f, 0.14f, 0.58f), new Vector3(0.08f, 0.5f, 0.08f), ironMaterial);
        barrel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Lancer Hot Pressure Coil", parent, new Vector3(0.36f, 0.03f, 0.48f), new Vector3(0.2f, 0.08f, 0.28f), warningMaterial);

        GameObject muzzleObject = new GameObject("Lancer Muzzle");
        muzzleObject.transform.SetParent(parent, false);
        muzzleObject.transform.localPosition = new Vector3(0.36f, 0.14f, 1.08f);
        return muzzleObject.transform;
    }

    private static void CreateScrapperVisual(Transform parent, Material bodyMaterial, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material bladeMaterial)
    {
        CreateLocalCube("Scrapper Boiler Torso", parent, new Vector3(0f, 0.05f, 0f), new Vector3(0.72f, 0.78f, 0.52f), bodyMaterial);
        CreateLocalCube("Scrapper Brass Chest Plate", parent, new Vector3(0f, 0.1f, 0.31f), new Vector3(0.5f, 0.48f, 0.08f), brassMaterial);
        CreateLocalCube("Scrapper Furnace Eye", parent, new Vector3(0f, 0.42f, 0.37f), new Vector3(0.34f, 0.14f, 0.06f), eyeMaterial);
        CreateLocalCube("Scrapper Jaw Guard", parent, new Vector3(0f, -0.22f, 0.36f), new Vector3(0.48f, 0.08f, 0.08f), ironMaterial);

        GameObject backTank = CreateLocalPrimitive("Scrapper Pressure Tank", PrimitiveType.Cylinder, parent, new Vector3(0f, 0.1f, -0.34f), new Vector3(0.28f, 0.62f, 0.28f), ironMaterial);
        backTank.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        CreateLocalCube("Scrapper Left Shoulder", parent, new Vector3(-0.48f, 0.22f, 0.05f), new Vector3(0.2f, 0.18f, 0.2f), brassMaterial);
        CreateLocalCube("Scrapper Right Shoulder", parent, new Vector3(0.48f, 0.22f, 0.05f), new Vector3(0.2f, 0.18f, 0.2f), brassMaterial);
        CreateLocalCube("Scrapper Left Piston Arm", parent, new Vector3(-0.68f, -0.08f, 0.18f), new Vector3(0.14f, 0.62f, 0.14f), ironMaterial);
        CreateLocalCube("Scrapper Right Piston Arm", parent, new Vector3(0.68f, -0.08f, 0.18f), new Vector3(0.14f, 0.62f, 0.14f), ironMaterial);
        GameObject leftBlade = CreateLocalCube("Scrapper Left Cutter", parent, new Vector3(-0.76f, -0.45f, 0.42f), new Vector3(0.16f, 0.36f, 0.08f), bladeMaterial);
        leftBlade.transform.localRotation = Quaternion.Euler(0f, 0f, -18f);
        GameObject rightBlade = CreateLocalCube("Scrapper Right Cutter", parent, new Vector3(0.76f, -0.45f, 0.42f), new Vector3(0.16f, 0.36f, 0.08f), bladeMaterial);
        rightBlade.transform.localRotation = Quaternion.Euler(0f, 0f, 18f);

        CreateLocalCube("Scrapper Left Leg", parent, new Vector3(-0.24f, -0.56f, 0f), new Vector3(0.18f, 0.48f, 0.18f), ironMaterial);
        CreateLocalCube("Scrapper Right Leg", parent, new Vector3(0.24f, -0.56f, 0f), new Vector3(0.18f, 0.48f, 0.18f), ironMaterial);
        CreateLocalCube("Scrapper Left Foot", parent, new Vector3(-0.24f, -0.84f, 0.16f), new Vector3(0.34f, 0.12f, 0.38f), brassMaterial);
        CreateLocalCube("Scrapper Right Foot", parent, new Vector3(0.24f, -0.84f, 0.16f), new Vector3(0.34f, 0.12f, 0.38f), brassMaterial);
    }

    private static void CreateObjectiveGuides(Material brassMaterial, Material warningMaterial, Material keyMaterial, Material exitMaterial)
    {
        CreateCube("Gear Key Pedestal", new Vector3(16f, 0.15f, 17f), new Vector3(1.35f, 0.3f, 1.35f), brassMaterial);
        CreateCube("Pressure Gate Warning Strip", new Vector3(0f, 0.015f, 21.25f), new Vector3(3.4f, 0.03f, 0.28f), warningMaterial);
        CreateCube("Service Lift Floor Strip", new Vector3(0f, 0.015f, 33.15f), new Vector3(3.6f, 0.03f, 0.28f), exitMaterial);
        CreateCube("Gear Key Route Floor Strip", new Vector3(8.1f, 0.015f, 17f), new Vector3(3.6f, 0.03f, 0.22f), keyMaterial);

        CreateWorldLabel("Label - Gear Key", "GEAR KEY", new Vector3(16f, 2.2f, 16.25f), new Color(1f, 0.82f, 0.28f), 0.28f);
        CreateWorldLabel("Label - Pressure Gate", "PRESSURE GATE: KEY REQUIRED", new Vector3(0f, 2.9f, 21.95f), new Color(1f, 0.45f, 0.16f), 0.22f);
        CreateWorldLabel("Label - Service Lift", "SERVICE LIFT", new Vector3(0f, 2.75f, 33.95f), new Color(0.45f, 1f, 0.52f), 0.26f);
    }

    private static void CreateLevel01FlowPolish(Material brassMaterial, Material warningMaterial, Material keyMaterial, Material exitMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Level01 Flow Polish V015");

        CreateDecoCube("Level01 Gate Preview Brass Sightline Rail Left", new Vector3(-1.15f, 0.055f, 19.35f), new Vector3(0.12f, 0.08f, 4.2f), brassMaterial, parent.transform);
        CreateDecoCube("Level01 Gate Preview Brass Sightline Rail Right", new Vector3(1.15f, 0.055f, 19.35f), new Vector3(0.12f, 0.08f, 4.2f), brassMaterial, parent.transform);
        CreateDecoCube("Level01 Gate Preview Red Locking Header", new Vector3(0f, 2.25f, 20.72f), new Vector3(2.85f, 0.12f, 0.08f), warningMaterial, parent.transform);
        CreatePressureGauge("Level01 Gate Preview Pressure Gauge", new Vector3(-1.46f, 1.72f, 20.88f), Quaternion.Euler(0f, 180f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateCagedGaslight("Level01 Gate Preview Amber Gaslight", new Vector3(1.5f, 1.86f, 20.82f), Quaternion.Euler(0f, 180f, 0f), ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreatePointLight("Level01 Gate Preview Amber Point Light", new Vector3(0f, 2.35f, 20.35f), new Color(1f, 0.52f, 0.12f), 1.65f, 5.1f);

        CreateDecoCube("Level01 Key Branch Return Brass Pipe A", new Vector3(12.25f, 0.09f, 17f), new Vector3(5.35f, 0.08f, 0.08f), brassMaterial, parent.transform);
        CreateDecoCube("Level01 Key Branch Return Brass Pipe B", new Vector3(8.25f, 0.09f, 18.28f), new Vector3(0.08f, 0.08f, 2.7f), brassMaterial, parent.transform);
        CreateDecoCube("Level01 Key Branch Return Amber Plate", new Vector3(10.05f, 0.055f, 18.85f), new Vector3(1.25f, 0.06f, 0.28f), keyMaterial, parent.transform);
        GameObject returnChevronA = CreateDecoCube("Level01 Key Branch Return Chevron A", new Vector3(8.85f, 0.065f, 18.74f), new Vector3(0.64f, 0.07f, 0.16f), keyMaterial, parent.transform);
        returnChevronA.transform.rotation = Quaternion.Euler(0f, -32f, 0f);
        GameObject returnChevronB = CreateDecoCube("Level01 Key Branch Return Chevron B", new Vector3(8.85f, 0.066f, 18.98f), new Vector3(0.64f, 0.07f, 0.16f), keyMaterial, parent.transform);
        returnChevronB.transform.rotation = Quaternion.Euler(0f, 32f, 0f);
        CreatePressureGauge("Level01 Key Branch Return Gauge", new Vector3(9.78f, 1.45f, 20.02f), Quaternion.identity, brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);

        CreateDecoCube("Level01 Service Lift Green Runway Center", new Vector3(0f, 0.055f, 31.05f), new Vector3(0.32f, 0.08f, 4.35f), exitMaterial, parent.transform);
        CreateDecoCube("Level01 Service Lift Green Runway Left Rail", new Vector3(-1.18f, 0.055f, 31.65f), new Vector3(0.11f, 0.08f, 3.15f), exitMaterial, parent.transform);
        CreateDecoCube("Level01 Service Lift Green Runway Right Rail", new Vector3(1.18f, 0.055f, 31.65f), new Vector3(0.11f, 0.08f, 3.15f), exitMaterial, parent.transform);
        GameObject liftChevronA = CreateDecoCube("Level01 Service Lift Green Chevron A", new Vector3(-0.36f, 0.07f, 32.55f), new Vector3(0.76f, 0.08f, 0.16f), exitMaterial, parent.transform);
        liftChevronA.transform.rotation = Quaternion.Euler(0f, -34f, 0f);
        GameObject liftChevronB = CreateDecoCube("Level01 Service Lift Green Chevron B", new Vector3(0.36f, 0.071f, 32.55f), new Vector3(0.76f, 0.08f, 0.16f), exitMaterial, parent.transform);
        liftChevronB.transform.rotation = Quaternion.Euler(0f, 34f, 0f);
        CreateDecoCube("Level01 Service Lift Green Overhead Pipe", new Vector3(0f, 2.78f, 32.55f), new Vector3(2.9f, 0.12f, 0.08f), exitMaterial, parent.transform);
        CreatePointLight("Level01 Service Lift Green Beacon Light", new Vector3(0f, 2.55f, 32.6f), new Color(0.16f, 1f, 0.32f), 2.2f, 6f);

        CreateDecoCube("Level01 Secret Warm Pipe Clue", new Vector3(-5.98f, 1.28f, 17.52f), new Vector3(0.08f, 0.1f, 1.55f), brassMaterial, parent.transform);
        CreateValveWheel("Level01 Secret Loose Service Valve", new Vector3(-5.95f, 1.08f, 18.42f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, warningMaterial, parent.transform);
        GameObject misalignedPlate = CreateDecoCube("Level01 Secret Misaligned Service Plate", new Vector3(-5.99f, 0.72f, 18.7f), new Vector3(0.045f, 0.62f, 0.82f), ironMaterial, parent.transform);
        misalignedPlate.transform.rotation = Quaternion.Euler(0f, -90f, 3f);
        CreatePointLight("Level01 Secret Warm Seam Light", new Vector3(-5.62f, 1.18f, 18.62f), new Color(1f, 0.44f, 0.12f), 0.95f, 2.8f);
    }

    private static void CreatePipeworksFlowPolish(Material brassMaterial, Material warningMaterial, Material exitMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Level02 Pipeworks Flow Polish V016");

        CreateDecoCube("Level02 Pipeworks Condensate Spine Center", new Vector3(0f, 0.055f, 12.4f), new Vector3(0.26f, 0.08f, 16.4f), brassMaterial, parent.transform);
        CreateDecoCube("Level02 Pipeworks Locked Boilerheart Lift Stop Bar", new Vector3(0f, 0.07f, 21.72f), new Vector3(3.25f, 0.08f, 0.22f), warningMaterial, parent.transform);
        CreateDecoCube("Level02 Pipeworks Locked Lift Pressure Line", new Vector3(0f, 2.42f, 22.25f), new Vector3(3.1f, 0.1f, 0.08f), warningMaterial, parent.transform);
        CreatePressureGauge("Level02 Pipeworks Lift Lock Gauge", new Vector3(1.65f, 1.62f, 22.25f), Quaternion.identity, brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateDecoCube("Level02 Routing Valve Floor Lead", new Vector3(-3.35f, 0.06f, 18.7f), new Vector3(0.18f, 0.08f, 3.15f), brassMaterial, parent.transform);
        CreateDecoCube("Level02 Routing Valve Return Elbow", new Vector3(-2.18f, 0.061f, 20f), new Vector3(2.45f, 0.08f, 0.18f), brassMaterial, parent.transform);
        CreateCagedGaslight("Level02 Routing Valve Amber Gaslight", new Vector3(-4.86f, 2.06f, 20.95f), Quaternion.Euler(0f, 90f, 0f), ironMaterial, brassMaterial, glowMaterial, parent.transform);
        CreatePointLight("Level02 Routing Valve Amber Point Light", new Vector3(-3.9f, 2.2f, 20f), new Color(1f, 0.58f, 0.16f), 1.55f, 4.4f);

        CreateCube("Level02 Lancer Sightline Brass Cover West", new Vector3(-1.25f, 0.55f, 15.6f), new Vector3(0.95f, 1.1f, 0.9f), brassMaterial, parent.transform);
        CreateCube("Level02 Lancer Sightline Iron Cover East", new Vector3(4.05f, 0.52f, 16.6f), new Vector3(0.78f, 1.04f, 1.05f), ironMaterial, parent.transform);
        CreateDecoCube("Level02 Lancer Sightline Warning Rail", new Vector3(2.25f, 0.065f, 14.6f), new Vector3(2.7f, 0.08f, 0.16f), warningMaterial, parent.transform);

        CreateDecoCube("Level02 Secret Cold Pipe Clue", new Vector3(-5.02f, 1.08f, 5.95f), new Vector3(0.1f, 0.1f, 1.35f), brassMaterial, parent.transform);
        CreateValveWheel("Level02 Secret Service Wheel", new Vector3(-5.02f, 1.26f, 5.35f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, warningMaterial, parent.transform);
        CreatePointLight("Level02 Secret Cool Glint Light", new Vector3(-4.75f, 1.08f, 5.4f), new Color(0.42f, 0.82f, 1f), 0.65f, 2.4f);
    }

    private static void CreateBoilerheartFlowPolish(Material brassMaterial, Material warningMaterial, Material exitMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Level03 Boilerheart Flow Polish V016");

        CreateDecoCube("Level03 Boilerheart Ring Brass Guide South", new Vector3(0f, 0.056f, 11.45f), new Vector3(4.8f, 0.08f, 0.18f), brassMaterial, parent.transform);
        CreateDecoCube("Level03 Boilerheart Ring Brass Guide North", new Vector3(0f, 0.057f, 19.65f), new Vector3(4.8f, 0.08f, 0.18f), brassMaterial, parent.transform);
        CreateDecoCube("Level03 Scattergun Trial Lane Strip", new Vector3(0f, 0.058f, 12.65f), new Vector3(0.32f, 0.08f, 2.45f), warningMaterial, parent.transform);
        CreatePressureGauge("Level03 Scattergun Display Pressure Gauge", new Vector3(1.55f, 1.62f, 13.18f), Quaternion.identity, brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);

        CreateFloorRing("Level03 Bellows Pulse Radius Marker", new Vector3(3.6f, 0.066f, 15.1f), GameBalance.BellowsNodePulseRange * 0.32f, warningMaterial, parent.transform);
        CreateDecoCube("Level03 Bellows Boost Pipe To Scrapper Lane", new Vector3(3.05f, 1.92f, 17.15f), new Vector3(0.12f, 0.12f, 4.2f), brassMaterial, parent.transform);
        CreateDecoCube("Level03 Bellows Warning Backplate", new Vector3(5.98f, 1.34f, 15.1f), new Vector3(0.08f, 0.62f, 1.52f), warningMaterial, parent.transform);
        CreatePointLight("Level03 Bellows Amber Pulse Read Light", new Vector3(3.6f, 1.85f, 15.1f), new Color(1f, 0.5f, 0.12f), 1.35f, 4.2f);

        CreateDecoCube("Level03 Valve To Lift Green Return Strip", new Vector3(2.6f, 0.06f, 21.6f), new Vector3(0.28f, 0.08f, 4.25f), exitMaterial, parent.transform);
        CreateDecoCube("Level03 Foundry Lift Locked Stop Bar", new Vector3(0f, 0.07f, 23.08f), new Vector3(3.2f, 0.08f, 0.22f), warningMaterial, parent.transform);
        CreateDecoCube("Level03 Hazard Shutdown Sight Glass", new Vector3(4.92f, 1.32f, 20.35f), new Vector3(0.1f, 0.62f, 0.34f), exitMaterial, parent.transform);
        CreatePointLight("Level03 Valve Return Green Beacon", new Vector3(2.6f, 2.2f, 21.6f), new Color(0.16f, 1f, 0.32f), 1.65f, 5.1f);
    }

    private static void CreateFoundryClimaxPolish(Material brassMaterial, Material warningMaterial, Material exitMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Level04 Foundry Climax Polish V017");

        CreateDecoCube("Level04 Furnace Timing Preview Strip", new Vector3(0f, 0.065f, 15.25f), new Vector3(4.85f, 0.08f, 0.18f), warningMaterial, parent.transform);
        CreatePressureGauge("Level04 Heat Lane Warning Gauge", new Vector3(-2.75f, 1.45f, 16.1f), Quaternion.identity, brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateDecoCube("Level04 Furnace Safe Edge Brass Rail", new Vector3(-2.92f, 0.066f, 16.8f), new Vector3(0.18f, 0.08f, 2.35f), brassMaterial, parent.transform);
        CreateDecoCube("Level04 Bulwark Hammer Bay Boundary", new Vector3(2.6f, 0.067f, 22.8f), new Vector3(3.15f, 0.08f, 0.2f), warningMaterial, parent.transform);
        CreateFloorRing("Level04 Bulwark Hammer Bay Floor Ring", new Vector3(2.6f, 0.072f, 22.8f), 2.2f, warningMaterial, parent.transform);
        CreateDecoCube("Level04 Bulwark Retreat Cover Signal West", new Vector3(-1.8f, 1.28f, 22.9f), new Vector3(0.18f, 0.72f, 1.1f), brassMaterial, parent.transform);
        CreateDecoCube("Level04 Bulwark Retreat Cover Signal East", new Vector3(4.72f, 1.28f, 22.9f), new Vector3(0.18f, 0.72f, 1.1f), brassMaterial, parent.transform);
        CreateDecoCube("Level04 Hoist Green Runway Strip", new Vector3(0f, 0.066f, 26.15f), new Vector3(0.34f, 0.08f, 3.25f), exitMaterial, parent.transform);
        CreateDecoCube("Level04 Emergency Hoist Beacon Pipe", new Vector3(0f, 2.62f, 27.35f), new Vector3(3.05f, 0.12f, 0.08f), exitMaterial, parent.transform);
        CreatePointLight("Level04 Emergency Hoist Green Beacon", new Vector3(0f, 2.45f, 27.2f), new Color(0.16f, 1f, 0.32f), 1.75f, 5.4f);
        CreateDecoCube("Level04 Coal Cache Footprint Clue", new Vector3(-5.82f, 0.06f, 24.15f), new Vector3(0.72f, 0.06f, 0.18f), ironMaterial, parent.transform);
        CreateValveWheel("Level04 Coal Cache Cool Quench Wheel", new Vector3(-6.45f, 1.18f, 24.72f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, warningMaterial, parent.transform);
    }

    private static void CreateGovernorClimaxPolish(Material brassMaterial, Material warningMaterial, Material exitMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material glowMaterial)
    {
        GameObject parent = new GameObject("Level05 Governor Climax Polish V017");

        CreateDecoCube("Level05 Warden Reveal Centerline Rail", new Vector3(0f, 0.065f, 22.4f), new Vector3(0.28f, 0.08f, 5.15f), brassMaterial, parent.transform);
        CreateDecoCube("Level05 Warden Lock Warning Stop Bar", new Vector3(0f, 0.068f, 26.55f), new Vector3(3.45f, 0.08f, 0.22f), warningMaterial, parent.transform);
        CreateFloorRing("Level05 Warden Arena Boundary Ring", new Vector3(0f, 0.073f, 24.1f), 2.8f, warningMaterial, parent.transform);
        CreateDecoCube("Level05 Boss Cover Pylon West", new Vector3(-4.6f, 1.08f, 23.4f), new Vector3(0.62f, 2.16f, 0.82f), ironMaterial, parent.transform);
        CreateDecoCube("Level05 Boss Cover Pylon East", new Vector3(4.6f, 1.08f, 23.4f), new Vector3(0.62f, 2.16f, 0.82f), ironMaterial, parent.transform);
        CreatePressureGauge("Level05 Warden Arena Pressure Gauge", new Vector3(5.95f, 1.62f, 24.08f), Quaternion.Euler(0f, -90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateDecoCube("Level05 Master Override Green Runway", new Vector3(0f, 0.066f, 27.55f), new Vector3(0.36f, 0.08f, 2.2f), exitMaterial, parent.transform);
        CreateDecoCube("Level05 Master Override Beacon Pipe", new Vector3(0f, 2.72f, 28.05f), new Vector3(3.15f, 0.12f, 0.08f), exitMaterial, parent.transform);
        CreatePointLight("Level05 Master Override Green Beacon", new Vector3(0f, 2.52f, 28.05f), new Color(0.16f, 1f, 0.32f), 1.85f, 5.6f);
    }

    private static void CreateFloorRing(string name, Vector3 center, float radius, Material material, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = center;

        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(radians) * radius, 0f, Mathf.Cos(radians) * radius);
            GameObject segment = CreateLocalCube(name + " Segment " + i, root.transform, position, new Vector3(0.62f, 0.055f, 0.11f), material);
            segment.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private static void CreateWorldLabel(string name, string text, Vector3 position, Color color, float characterSize)
    {
        GameObject label = new GameObject(name);
        label.transform.position = position;
        label.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        TextMesh textMesh = label.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.characterSize = characterSize;
        textMesh.fontSize = 48;
        textMesh.color = color;
    }

    private static void CreateLevel01SignageDecalsV1()
    {
        GameObject parent = new GameObject("Signage Decals V1 - Level01");

        CreateSignageDecal("OBJ-L01-01", "Gear Key Ahead", SignageDecalSheet.ObjectivePlates, 70f, 86f, 581f, 284f, new Vector2(1.2f, 0.3f), new Vector3(14f, 1.9f, 14.03f), Quaternion.Euler(0f, 180f, 0f), parent.transform);
        CreateSignageDecal("OBJ-L01-02", "Pressure Gate", SignageDecalSheet.ObjectivePlates, 733f, 86f, 581f, 284f, new Vector2(1.35f, 0.32f), new Vector3(0f, 2.55f, 21.87f), Quaternion.Euler(0f, 180f, 0f), parent.transform);
        CreateSignageDecal("OBJ-L01-03", "Service Lift", SignageDecalSheet.ObjectivePlates, 1396f, 86f, 581f, 284f, new Vector2(1.25f, 0.3f), new Vector3(0f, 2.42f, 35.98f), Quaternion.identity, parent.transform);

        CreateSignageDecal("HAZ-L01-01", "Pressure Locked", SignageDecalSheet.WarningHazardStrips, 70f, 86f, 581f, 284f, new Vector2(1.2f, 0.22f), new Vector3(-1.15f, 1.62f, 21.87f), Quaternion.Euler(0f, 180f, 0f), parent.transform);
        CreateSignageDecal("HAZ-L01-03", "Gate Crush", SignageDecalSheet.WarningHazardStrips, 1396f, 86f, 581f, 284f, new Vector2(1f, 0.2f), new Vector3(0f, 0.035f, 21.18f), Quaternion.Euler(90f, 0f, 0f), parent.transform);

        CreateSignageDecal("ARR-L01-01", "To Key", SignageDecalSheet.RouteArrowsChevrons, 70f, 86f, 581f, 284f, new Vector2(0.85f, 0.3f), new Vector3(7.7f, 0.04f, 17f), Quaternion.Euler(90f, 0f, 0f), parent.transform);
        CreateSignageDecal("ARR-L01-02", "To Gate", SignageDecalSheet.RouteArrowsChevrons, 733f, 86f, 581f, 284f, new Vector2(0.85f, 0.3f), new Vector3(8.2f, 0.041f, 18.9f), Quaternion.Euler(90f, 0f, 0f), parent.transform);
        CreateSignageDecal("ARR-L01-03", "To Lift", SignageDecalSheet.RouteArrowsChevrons, 1396f, 86f, 581f, 284f, new Vector2(0.85f, 0.3f), new Vector3(0f, 0.042f, 32.2f), Quaternion.Euler(90f, 0f, 0f), parent.transform);

        CreateSignageDecal("SEC-L01-01", "Warm Seam", SignageDecalSheet.SecretServiceMarks, 70f, 74f, 581f, 308f, new Vector2(0.45f, 0.18f), new Vector3(-5.98f, 0.92f, 18.08f), Quaternion.Euler(0f, -90f, 0f), parent.transform);
        CreateSignageDecal("SEC-L01-02", "Three Rivets Out", SignageDecalSheet.SecretServiceMarks, 733f, 74f, 581f, 308f, new Vector2(0.55f, 0.18f), new Vector3(-5.98f, 0.68f, 18.72f), Quaternion.Euler(0f, -90f, 0f), parent.transform);
    }

    private static void CreateLevel03SignageDecalsV1()
    {
        GameObject parent = new GameObject("Signage Decals V1 - Level03");

        CreateSignageDecal("OBJ-L03-01", "Vent Core Pressure", SignageDecalSheet.ObjectivePlates, 70f, 882f, 581f, 284f, new Vector2(1.45f, 0.32f), new Vector3(0f, 1.92f, -1.74f), Quaternion.Euler(0f, 180f, 0f), parent.transform);
        CreateSignageDecal("OBJ-L03-02", "Pressure Valve", SignageDecalSheet.ObjectivePlates, 733f, 882f, 581f, 284f, new Vector2(1.25f, 0.3f), new Vector3(5.98f, 1.9f, 17.4f), Quaternion.Euler(0f, 90f, 0f), parent.transform);
        CreateSignageDecal("OBJ-L03-03", "Foundry Lift", SignageDecalSheet.ObjectivePlates, 1396f, 882f, 581f, 284f, new Vector2(1.2f, 0.3f), new Vector3(0f, 2.42f, 25.74f), Quaternion.identity, parent.transform);

        CreateSignageDecal("HAZ-L03-01", "Furnace Leak", SignageDecalSheet.WarningHazardStrips, 70f, 882f, 581f, 284f, new Vector2(1.1f, 0.22f), new Vector3(-5.98f, 1.12f, 20.8f), Quaternion.Euler(0f, -90f, 0f), parent.transform);
        CreateSignageDecal("HAZ-L03-02", "Core Bleed", SignageDecalSheet.WarningHazardStrips, 733f, 882f, 581f, 284f, new Vector2(1f, 0.22f), new Vector3(0f, 2.42f, 14.43f), Quaternion.identity, parent.transform);
        CreateSignageDecal("HAZ-L03-03", "Pressure Pulse", SignageDecalSheet.WarningHazardStrips, 1396f, 882f, 581f, 284f, new Vector2(1.2f, 0.22f), new Vector3(3.6f, 0.04f, 15.1f), Quaternion.Euler(90f, 0f, 0f), parent.transform);

        CreateSignageDecal("ARR-L03-01", "To Valve", SignageDecalSheet.RouteArrowsChevrons, 70f, 882f, 581f, 284f, new Vector2(0.9f, 0.3f), new Vector3(-2.2f, 0.041f, 8.8f), Quaternion.Euler(90f, 0f, 0f), parent.transform);
        CreateSignageDecal("ARR-L03-02", "To Tool", SignageDecalSheet.RouteArrowsChevrons, 733f, 882f, 581f, 284f, new Vector2(0.85f, 0.3f), new Vector3(0f, 0.042f, 12.8f), Quaternion.Euler(90f, 0f, 0f), parent.transform);
        CreateSignageDecal("ARR-L03-03", "To Foundry", SignageDecalSheet.RouteArrowsChevrons, 1396f, 882f, 581f, 284f, new Vector2(1f, 0.3f), new Vector3(0f, 0.043f, 22.7f), Quaternion.Euler(90f, 0f, 0f), parent.transform);

        CreateSignageDecal("MAC-L03-01", "Boilerheart Core", SignageDecalSheet.StencilMachineryLore, 70f, 882f, 316f, 284f, new Vector2(1.05f, 0.18f), new Vector3(0f, 2.72f, 14.42f), Quaternion.identity, parent.transform);
        CreateSignageDecal("SEC-L03-01", "Gauge Lies", SignageDecalSheet.SecretServiceMarks, 70f, 870f, 581f, 308f, new Vector2(0.45f, 0.18f), new Vector3(-5.98f, 0.9f, 12.1f), Quaternion.Euler(0f, -90f, 0f), parent.transform);
        CreateSignageDecal("SEC-L03-02", "Shutter Drags", SignageDecalSheet.SecretServiceMarks, 733f, 870f, 581f, 308f, new Vector2(0.5f, 0.18f), new Vector3(-5.98f, 0.66f, 12.68f), Quaternion.Euler(0f, -90f, 0f), parent.transform);
    }

    private static void CreateLevel05SignageDecalsV1()
    {
        GameObject parent = new GameObject("Signage Decals V1 - Level05");

        CreateSignageDecal("OBJ-L05-01", "Regulator Ring", SignageDecalSheet.ObjectivePlates, 70f, 1678f, 581f, 284f, new Vector2(1.25f, 0.3f), new Vector3(0f, 1.92f, -1.74f), Quaternion.Euler(0f, 180f, 0f), parent.transform);
        CreateSignageDecal("OBJ-L05-02", "Warden Lock", SignageDecalSheet.ObjectivePlates, 733f, 1678f, 581f, 284f, new Vector2(1.15f, 0.3f), new Vector3(-1.2f, 1.8f, 29.73f), Quaternion.identity, parent.transform);
        CreateSignageDecal("OBJ-L05-03", "Master Override", SignageDecalSheet.ObjectivePlates, 1396f, 1678f, 581f, 284f, new Vector2(1.4f, 0.32f), new Vector3(1.1f, 2.42f, 29.73f), Quaternion.identity, parent.transform);

        CreateSignageDecal("HAZ-L05-01", "Regulator Leak", SignageDecalSheet.WarningHazardStrips, 70f, 1678f, 581f, 284f, new Vector2(1.2f, 0.22f), new Vector3(-6.98f, 1.12f, 20.8f), Quaternion.Euler(0f, -90f, 0f), parent.transform);
        CreateSignageDecal("HAZ-L05-02", "Surge Floor", SignageDecalSheet.WarningHazardStrips, 733f, 1678f, 581f, 284f, new Vector2(1f, 0.22f), new Vector3(0f, 0.041f, 18.9f), Quaternion.Euler(90f, 0f, 0f), parent.transform);
        CreateSignageDecal("HAZ-L05-03", "Warden Active", SignageDecalSheet.WarningHazardStrips, 1396f, 1678f, 581f, 284f, new Vector2(1.15f, 0.22f), new Vector3(6.98f, 1.24f, 22.7f), Quaternion.Euler(0f, 90f, 0f), parent.transform);

        CreateSignageDecal("ARR-L05-01", "To Core", SignageDecalSheet.RouteArrowsChevrons, 70f, 1678f, 581f, 284f, new Vector2(0.85f, 0.3f), new Vector3(0f, 0.041f, 7.5f), Quaternion.Euler(90f, 0f, 0f), parent.transform);
        CreateSignageDecal("ARR-L05-02", "To Override", SignageDecalSheet.RouteArrowsChevrons, 733f, 1678f, 581f, 284f, new Vector2(1f, 0.3f), new Vector3(0f, 0.042f, 25.2f), Quaternion.Euler(90f, 0f, 0f), parent.transform);
        CreateSignageDecal("ARR-L05-03", "Green Hoist", SignageDecalSheet.RouteArrowsChevrons, 1396f, 1678f, 581f, 284f, new Vector2(1f, 0.3f), new Vector3(0f, 0.043f, 27.35f), Quaternion.Euler(90f, 0f, 0f), parent.transform);

        CreateSignageDecal("MAC-L05-01", "Governor Core", SignageDecalSheet.StencilMachineryLore, 70f, 1678f, 316f, 284f, new Vector2(1f, 0.18f), new Vector3(0f, 2.58f, 15.54f), Quaternion.identity, parent.transform);
        CreateSignageDecal("SEC-L05-01", "Wrong Clerk Tag", SignageDecalSheet.SecretServiceMarks, 70f, 1666f, 581f, 308f, new Vector2(0.55f, 0.18f), new Vector3(-6.98f, 0.88f, 17.15f), Quaternion.Euler(0f, -90f, 0f), parent.transform);
        CreateSignageDecal("SEC-L05-02", "Index 5B", SignageDecalSheet.SecretServiceMarks, 733f, 1666f, 581f, 308f, new Vector2(0.4f, 0.16f), new Vector3(-6.98f, 0.64f, 17.65f), Quaternion.Euler(0f, -90f, 0f), parent.transform);
    }

    private static GameObject CreateSignageDecal(string id, string label, SignageDecalSheet sheet, float sourceX, float sourceY, float sourceWidth, float sourceHeight, Vector2 sizeMeters, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject decal = new GameObject("Signage Decal - " + id + " - " + label);
        decal.transform.SetParent(parent, false);
        decal.transform.position = position;
        decal.transform.rotation = rotation;

        float halfWidth = sizeMeters.x * 0.5f;
        float halfHeight = sizeMeters.y * 0.5f;
        float uMin = sourceX / SignageDecalsAtlasPixels;
        float uMax = (sourceX + sourceWidth) / SignageDecalsAtlasPixels;
        float vMax = 1f - (sourceY / SignageDecalsAtlasPixels);
        float vMin = 1f - ((sourceY + sourceHeight) / SignageDecalsAtlasPixels);

        Mesh mesh = new Mesh();
        mesh.name = "SignageDecalsV1_" + id + "_Mesh";
        mesh.vertices = new[]
        {
            new Vector3(-halfWidth, -halfHeight, 0f),
            new Vector3(halfWidth, -halfHeight, 0f),
            new Vector3(-halfWidth, halfHeight, 0f),
            new Vector3(halfWidth, halfHeight, 0f)
        };
        mesh.uv = new[]
        {
            new Vector2(uMin, vMin),
            new Vector2(uMax, vMin),
            new Vector2(uMin, vMax),
            new Vector2(uMax, vMax)
        };
        mesh.triangles = new[] { 0, 2, 1, 2, 3, 1, 0, 1, 2, 2, 1, 3 };
        mesh.normals = new[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
        mesh.RecalculateBounds();

        MeshFilter meshFilter = decal.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;
        MeshRenderer meshRenderer = decal.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = CreateSignageDecalMaterial(id, sheet);
        return decal;
    }

    private static Material CreateSignageDecalMaterial(string id, SignageDecalSheet sheet)
    {
        string texturePath = GetSignageDecalTexturePath(sheet);
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        if (texture == null)
        {
            throw new FileNotFoundException("Missing staged signage texture", texturePath);
        }

        Shader shader = Shader.Find("Unlit/Transparent");
        if (shader == null)
        {
            shader = Shader.Find("Universal Render Pipeline/Unlit");
        }

        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material material = new Material(shader);
        material.name = "M_SignageDecalsV1_" + id;
        material.mainTexture = texture;
        if (material.HasProperty("_BaseMap"))
        {
            material.SetTexture("_BaseMap", texture);
        }

        if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", Color.white);
        }

        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", Color.white);
        }

        ConfigureSignageMaterial(material);
        return material;
    }

    private static void ConfigureSignageMaterial(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        if (material.HasProperty("_Mode"))
        {
            material.SetFloat("_Mode", 3f);
        }

        if (material.HasProperty("_Surface"))
        {
            material.SetFloat("_Surface", 1f);
        }

        if (material.HasProperty("_Blend"))
        {
            material.SetFloat("_Blend", 0f);
        }

        if (material.HasProperty("_SrcBlend"))
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        }

        if (material.HasProperty("_DstBlend"))
        {
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        }

        if (material.HasProperty("_ZWrite"))
        {
            material.SetInt("_ZWrite", 0);
        }

        if (material.HasProperty("_Cull"))
        {
            material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }

        material.EnableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    private static string GetSignageDecalTexturePath(SignageDecalSheet sheet)
    {
        switch (sheet)
        {
            case SignageDecalSheet.ObjectivePlates:
                return SignageDecalsTextureFolder + "/T_SignageDecalsV1_ObjectivePlates_2048.png";
            case SignageDecalSheet.WarningHazardStrips:
                return SignageDecalsTextureFolder + "/T_SignageDecalsV1_WarningHazardStrips_2048.png";
            case SignageDecalSheet.RouteArrowsChevrons:
                return SignageDecalsTextureFolder + "/T_SignageDecalsV1_RouteArrowsChevrons_2048.png";
            case SignageDecalSheet.StencilMachineryLore:
                return SignageDecalsTextureFolder + "/T_SignageDecalsV1_StencilMachineryLore_2048.png";
            case SignageDecalSheet.SecretServiceMarks:
                return SignageDecalsTextureFolder + "/T_SignageDecalsV1_SecretServiceMarks_2048.png";
            default:
                throw new ArgumentOutOfRangeException(nameof(sheet), sheet, "Unknown signage decal sheet.");
        }
    }

    private static void CreateSecretCache(Material brassMaterial, Material ironMaterial, Material warningMaterial, Material healthMaterial, Material glassMaterial, Material fluidMaterial, Material ammoMaterial, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition)
    {
        GameObject secretRoot = new GameObject("Secret - Intake Pressure Cache");
        secretRoot.transform.position = new Vector3(-5.25f, 0.78f, 18.6f);

        BoxCollider trigger = secretRoot.AddComponent<BoxCollider>();
        trigger.size = new Vector3(1.65f, 1.55f, 1.65f);
        trigger.isTrigger = true;

        SecretArea secret = secretRoot.AddComponent<SecretArea>();
        secret.secretId = "intake-pressure-cache";
        secret.discoveryMessage = "SECRET PRESSURE CACHE FOUND";

        CreateLocalCube("Secret Pressure Cache Brass Floor Plate", secretRoot.transform, new Vector3(0f, -0.77f, 0f), new Vector3(1.45f, 0.05f, 1.45f), brassMaterial);
        CreateLocalCube("Secret Pressure Cache Iron Backplate", secretRoot.transform, new Vector3(-0.5f, 0.05f, 0f), new Vector3(0.08f, 1.05f, 1.32f), ironMaterial);
        CreateLocalCube("Secret Pressure Cache Warning Strip", secretRoot.transform, new Vector3(0f, -0.7f, -0.52f), new Vector3(1.35f, 0.06f, 0.12f), warningMaterial);

        CreateHealthVialPickup("Pickup - Secret Health Vial", new Vector3(-5.15f, 0.65f, 17.95f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Secret Pressure Cartridge Pack", new Vector3(-5.15f, 0.55f, 19.25f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        CreateWorldLabel("Label - Secret Cache", "CACHE", new Vector3(-5.25f, 1.85f, 18.6f), new Color(1f, 0.72f, 0.28f), 0.18f);
    }

    private static void CreatePipeworksSecretCache(Material brassMaterial, Material ironMaterial, Material warningMaterial, Material healthMaterial, Material glassMaterial, Material fluidMaterial, Material ammoMaterial, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition)
    {
        GameObject secretRoot = new GameObject("Secret - Pipeworks Cartridge Cache");
        secretRoot.transform.position = new Vector3(-4.35f, 0.78f, 5.4f);

        BoxCollider trigger = secretRoot.AddComponent<BoxCollider>();
        trigger.size = new Vector3(1.65f, 1.55f, 1.65f);
        trigger.isTrigger = true;

        SecretArea secret = secretRoot.AddComponent<SecretArea>();
        secret.secretId = "pipeworks-cartridge-cache";
        secret.discoveryMessage = "SECRET PIPEWORKS CACHE FOUND";

        CreateLocalCube("Secret Pipeworks Cache Brass Floor Plate", secretRoot.transform, new Vector3(0f, -0.77f, 0f), new Vector3(1.45f, 0.05f, 1.45f), brassMaterial);
        CreateLocalCube("Secret Pipeworks Cache Iron Pipe Rack", secretRoot.transform, new Vector3(-0.48f, 0.02f, 0f), new Vector3(0.12f, 0.9f, 1.28f), ironMaterial);
        CreateLocalCube("Secret Pipeworks Cache Warning Strip", secretRoot.transform, new Vector3(0f, -0.7f, -0.52f), new Vector3(1.35f, 0.06f, 0.12f), warningMaterial);
        GameObject sparePipeA = CreateLocalPrimitive("Secret Pipeworks Cache Spare Pipe A", PrimitiveType.Cylinder, secretRoot.transform, new Vector3(-0.1f, -0.36f, -0.22f), new Vector3(0.055f, 0.42f, 0.055f), brassMaterial);
        sparePipeA.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject sparePipeB = CreateLocalPrimitive("Secret Pipeworks Cache Spare Pipe B", PrimitiveType.Cylinder, secretRoot.transform, new Vector3(0.18f, -0.36f, 0.1f), new Vector3(0.055f, 0.36f, 0.055f), brassMaterial);
        sparePipeB.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        CreateHealthVialPickup("Pickup - Pipeworks Secret Health Vial", new Vector3(-4.2f, 0.65f, 4.75f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Pipeworks Secret Pressure Cartridge Pack", new Vector3(-4.2f, 0.55f, 6.05f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        CreateWorldLabel("Label - Pipeworks Secret Cache", "CARTRIDGE CACHE", new Vector3(-4.35f, 1.85f, 5.4f), new Color(1f, 0.72f, 0.28f), 0.14f);
    }

    private static void CreateFoundrySecretCache(Material brassMaterial, Material ironMaterial, Material warningMaterial, Material healthMaterial, Material glassMaterial, Material fluidMaterial, Material ammoMaterial, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition)
    {
        GameObject secretRoot = new GameObject("Secret - Foundry Coal Cache");
        secretRoot.transform.position = new Vector3(-5.2f, 0.78f, 24.7f);

        BoxCollider trigger = secretRoot.AddComponent<BoxCollider>();
        trigger.size = new Vector3(1.8f, 1.55f, 1.8f);
        trigger.isTrigger = true;

        SecretArea secret = secretRoot.AddComponent<SecretArea>();
        secret.secretId = "foundry-coal-cache";
        secret.discoveryMessage = "SECRET FOUNDRY CACHE FOUND";

        CreateLocalCube("Secret Foundry Cache Brass Floor Plate", secretRoot.transform, new Vector3(0f, -0.77f, 0f), new Vector3(1.55f, 0.05f, 1.55f), brassMaterial);
        CreateLocalCube("Secret Foundry Cache Iron Coal Bin", secretRoot.transform, new Vector3(-0.46f, -0.04f, 0f), new Vector3(0.16f, 0.88f, 1.34f), ironMaterial);
        CreateLocalCube("Secret Foundry Cache Warning Strip", secretRoot.transform, new Vector3(0f, -0.69f, -0.58f), new Vector3(1.45f, 0.06f, 0.12f), warningMaterial);
        CreateLocalPrimitive("Secret Foundry Cache Coal Lump A", PrimitiveType.Sphere, secretRoot.transform, new Vector3(0.22f, -0.47f, -0.08f), new Vector3(0.2f, 0.13f, 0.18f), ironMaterial);
        CreateLocalPrimitive("Secret Foundry Cache Coal Lump B", PrimitiveType.Sphere, secretRoot.transform, new Vector3(0.46f, -0.48f, 0.18f), new Vector3(0.17f, 0.12f, 0.16f), ironMaterial);

        CreateHealthVialPickup("Pickup - Foundry Secret Health Vial", new Vector3(-5.05f, 0.65f, 24.05f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Foundry Secret Pressure Cartridge Pack", new Vector3(-5.05f, 0.55f, 25.35f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        CreateWorldLabel("Label - Foundry Secret Cache", "COAL CACHE", new Vector3(-5.2f, 1.85f, 24.7f), new Color(1f, 0.72f, 0.28f), 0.16f);
    }

    private static void CreateSteamworksDressing(Material rivetedIronMaterial, Material oilStoneMaterial, Material brassMaterial, Material warningMaterial, Material amberMaterial, Material gaugeFaceMaterial, Material steamPuffMaterial, Material furnaceGlowMaterial)
    {
        GameObject parent = new GameObject("Steamworks Dressing - Brassworks Intake");

        CreateDecoCube("Oil Stone Patch - Start", new Vector3(0f, 0.012f, 0f), new Vector3(5.8f, 0.024f, 5.2f), oilStoneMaterial, parent.transform);
        CreateDecoCube("Oil Stone Patch - Fight", new Vector3(0f, 0.013f, 17f), new Vector3(10.5f, 0.026f, 8.5f), oilStoneMaterial, parent.transform);
        CreateDecoCube("Oil Stone Patch - Final", new Vector3(0f, 0.014f, 31f), new Vector3(11.5f, 0.028f, 8.5f), oilStoneMaterial, parent.transform);

        CreateCableRun("Copper Pipe Run - Main West", new Vector3(-6.02f, 2.35f, 17f), new Vector3(0.08f, 0.16f, 8.4f), brassMaterial, parent.transform);
        CreateCableRun("Red Pressure Pipe - Main East", new Vector3(6.02f, 2.15f, 17f), new Vector3(0.08f, 0.14f, 7.2f), warningMaterial, parent.transform);
        CreateCableRun("Copper Pipe Run - Key Room", new Vector3(14f, 2.25f, 20.02f), new Vector3(6.8f, 0.14f, 0.08f), brassMaterial, parent.transform);
        CreateCableRun("Red Pressure Pipe - Gate", new Vector3(0f, 2.7f, 22.18f), new Vector3(3.2f, 0.12f, 0.08f), warningMaterial, parent.transform);
        CreateCableRun("Service Lift Steam Pipe", new Vector3(0f, 2.55f, 34.08f), new Vector3(3.8f, 0.12f, 0.08f), brassMaterial, parent.transform);

        CreateServerStack("Boiler Stack - Final West", new Vector3(-5.75f, 1.15f, 29.2f), rivetedIronMaterial, brassMaterial, warningMaterial, parent.transform);
        CreateServerStack("Boiler Stack - Final East", new Vector3(5.75f, 1.15f, 32.8f), rivetedIronMaterial, brassMaterial, warningMaterial, parent.transform);
        CreateServerStack("Boiler Stack - Key Room", new Vector3(17.25f, 1.15f, 15.2f), rivetedIronMaterial, brassMaterial, amberMaterial, parent.transform);

        CreateDecoCube("Amber Hazard Stripe - Gate Left", new Vector3(-1.85f, 0.05f, 22.05f), new Vector3(0.18f, 0.06f, 1.2f), amberMaterial, parent.transform);
        CreateDecoCube("Amber Hazard Stripe - Gate Right", new Vector3(1.85f, 0.05f, 22.05f), new Vector3(0.18f, 0.06f, 1.2f), amberMaterial, parent.transform);
        CreateDecoCube("Riveted Iron Gate Header", new Vector3(0f, 3.25f, 22.12f), new Vector3(3.8f, 0.28f, 0.18f), rivetedIronMaterial, parent.transform);

        CreatePressureGauge("Pressure Gauge - Intake", new Vector3(-5.92f, 1.65f, 8.2f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreatePressureGauge("Pressure Gauge - Gate", new Vector3(-1.25f, 2.35f, 22.16f), Quaternion.Euler(0f, 180f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateValveWheel("Valve Wheel - Main West", new Vector3(-5.95f, 1.2f, 13.2f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, warningMaterial, parent.transform);
        CreateValveWheel("Valve Wheel - Key Room", new Vector3(13.1f, 1.25f, 20.05f), Quaternion.Euler(0f, 0f, 0f), brassMaterial, warningMaterial, parent.transform);
        CreateSteamVent("Steam Vent - Intake", new Vector3(2.7f, 0.25f, 7.6f), rivetedIronMaterial, steamPuffMaterial, parent.transform);
        CreateSteamVent("Steam Vent - Final", new Vector3(-4.2f, 0.25f, 31.4f), rivetedIronMaterial, steamPuffMaterial, parent.transform);
        CreateFurnace("Coal Furnace - Final Room", new Vector3(4.95f, 0.95f, 29.7f), rivetedIronMaterial, brassMaterial, furnaceGlowMaterial, parent.transform);
        CreatePipeBundle("Pipe Bundle - Gate Manifold", new Vector3(-2.65f, 2.55f, 22.05f), Quaternion.Euler(0f, 90f, 0f), 2.25f, brassMaterial, rivetedIronMaterial, parent.transform);
        CreatePipeBundle("Pipe Bundle - Final Boiler Feed", new Vector3(5.96f, 2.25f, 31.2f), Quaternion.identity, 3.2f, brassMaterial, rivetedIronMaterial, parent.transform);
        CreatePipeCanopy("North Star Intake Pipe Canopy", new Vector3(0f, 2.82f, 17.4f), Quaternion.identity, 8.2f, brassMaterial, rivetedIronMaterial, parent.transform);
        CreateCagedGaslight("North Star Intake Gaslight Left", new Vector3(-5.52f, 2.02f, 17.1f), Quaternion.Euler(0f, 90f, 0f), rivetedIronMaterial, brassMaterial, furnaceGlowMaterial, parent.transform);
        CreateCagedGaslight("North Star Intake Gaslight Right", new Vector3(5.52f, 2.02f, 19.6f), Quaternion.Euler(0f, -90f, 0f), rivetedIronMaterial, brassMaterial, furnaceGlowMaterial, parent.transform);
        CreateRivetBand("North Star Gate Rivet Band", new Vector3(0f, 2.92f, 21.94f), Quaternion.identity, 3.8f, rivetedIronMaterial, brassMaterial, parent.transform);
        CreateWorkOrderBoard("Work Order Board - Intake", "ORDER 17\nSEAL MAIN\nWATCH PSI", new Vector3(-5.92f, 1.55f, 10.8f), Quaternion.Euler(0f, 90f, 0f), rivetedIronMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateLorePlaque("Lore Plaque - Intake Archive", "Intake Archive", "The Brassworks sealed itself when the master governor jammed and every service machine obeyed the wrong pressure order.", new Vector3(-5.92f, 1.55f, 13.2f), Quaternion.Euler(0f, 90f, 0f), rivetedIronMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateWorkOrderBoard("Work Order Board - Gate", "KEY CREW\nBLEED LOCK\nNO OPEN FLAME", new Vector3(1.45f, 1.75f, 22.15f), Quaternion.Euler(0f, 180f, 0f), rivetedIronMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
    }

    private static void CreatePipeBundle(string name, Vector3 position, Quaternion rotation, float length, Material pipeMaterial, Material bracketMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        Vector3[] offsets =
        {
            new Vector3(-0.16f, 0f, 0f),
            new Vector3(0f, 0.12f, 0f),
            new Vector3(0.16f, 0f, 0f)
        };

        for (int i = 0; i < offsets.Length; i++)
        {
            GameObject pipe = CreateLocalPrimitive(name + " Pipe " + i, PrimitiveType.Cylinder, root.transform, offsets[i], new Vector3(0.055f, length * 0.5f, 0.055f), pipeMaterial);
            pipe.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        CreateLocalCube(name + " Bracket A", root.transform, new Vector3(0f, 0.02f, -length * 0.5f), new Vector3(0.52f, 0.08f, 0.08f), bracketMaterial);
        CreateLocalCube(name + " Bracket B", root.transform, new Vector3(0f, 0.02f, length * 0.5f), new Vector3(0.52f, 0.08f, 0.08f), bracketMaterial);
    }

    private static WallPipeGaugeClusterPrototype CreateWallPipeGaugeClusterPrototype(string name, Vector3 position, Quaternion rotation, float scale, Material brassMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material warningMaterial, string placementRole, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        WallPipeGaugeClusterPrototype prototype = root.AddComponent<WallPipeGaugeClusterPrototype>();
        prototype.placementRole = placementRole;
        prototype.pipeCount = 5;
        prototype.gaugeCount = 2;
        prototype.valveCount = 1;
        prototype.rivetCount = 14;

        GameObject backplate = CreateLocalCube(name + " Blackened Iron Mounting Plate", root.transform, Vector3.zero, new Vector3(1.72f * scale, 1.18f * scale, 0.08f * scale), ironMaterial);
        CreateLocalCube(name + " Aged Brass Header Rail", root.transform, new Vector3(0f, 0.53f * scale, -0.055f * scale), new Vector3(1.54f * scale, 0.07f * scale, 0.06f * scale), brassMaterial);
        CreateLocalCube(name + " Aged Brass Lower Rail", root.transform, new Vector3(0f, -0.53f * scale, -0.055f * scale), new Vector3(1.54f * scale, 0.07f * scale, 0.06f * scale), brassMaterial);

        GameObject pipeRoot = CreateLocalEmpty(name + " Pipe Root", root.transform, Vector3.zero, Quaternion.identity);
        GameObject primaryPipe = CreateLocalPrimitive(name + " Vertical Copper Feed Pipe 00", PrimitiveType.Cylinder, pipeRoot.transform, new Vector3(-0.58f * scale, 0f, -0.1f * scale), new Vector3(0.04f * scale, 0.54f * scale, 0.04f * scale), brassMaterial);
        GameObject secondaryPipe = CreateLocalPrimitive(name + " Vertical Blackened Return Pipe 01", PrimitiveType.Cylinder, pipeRoot.transform, new Vector3(0.58f * scale, 0f, -0.1f * scale), new Vector3(0.035f * scale, 0.54f * scale, 0.035f * scale), ironMaterial);
        GameObject centerPipe = CreateLocalPrimitive(name + " Center Copper Pressure Pipe 02", PrimitiveType.Cylinder, pipeRoot.transform, new Vector3(0f, 0.06f * scale, -0.11f * scale), new Vector3(0.032f * scale, 0.45f * scale, 0.032f * scale), brassMaterial);
        GameObject crossPipeA = CreateLocalPrimitive(name + " Upper Cross Pressure Pipe 03", PrimitiveType.Cylinder, pipeRoot.transform, new Vector3(0f, 0.34f * scale, -0.12f * scale), new Vector3(0.032f * scale, 0.62f * scale, 0.032f * scale), brassMaterial);
        crossPipeA.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        GameObject crossPipeB = CreateLocalPrimitive(name + " Lower Cross Return Pipe 04", PrimitiveType.Cylinder, pipeRoot.transform, new Vector3(0f, -0.36f * scale, -0.12f * scale), new Vector3(0.03f * scale, 0.58f * scale, 0.03f * scale), ironMaterial);
        crossPipeB.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        CreateLocalPrimitive(name + " Center Pipe Union", PrimitiveType.Sphere, pipeRoot.transform, new Vector3(0f, 0.06f * scale, -0.16f * scale), new Vector3(0.11f * scale, 0.11f * scale, 0.045f * scale), brassMaterial);

        GameObject gaugeRoot = CreateLocalEmpty(name + " Gauge Root", root.transform, Vector3.zero, Quaternion.identity);
        GameObject gaugeFaceA = CreateWallClusterGauge(name + " Upper Pressure Gauge", gaugeRoot.transform, new Vector3(-0.26f * scale, 0.15f * scale, -0.18f * scale), 0.29f * scale, brassMaterial, gaugeFaceMaterial, warningMaterial);
        CreateWallClusterGauge(name + " Lower Pressure Gauge", gaugeRoot.transform, new Vector3(0.32f * scale, -0.23f * scale, -0.18f * scale), 0.23f * scale, brassMaterial, gaugeFaceMaterial, warningMaterial);

        GameObject valveRoot = CreateLocalEmpty(name + " Valve Root", root.transform, new Vector3(0.42f * scale, 0.26f * scale, -0.18f * scale), Quaternion.identity);
        GameObject valveWheel = CreateLocalPrimitive(name + " Red Brass Valve Wheel", PrimitiveType.Cylinder, valveRoot.transform, Vector3.zero, new Vector3(0.18f * scale, 0.018f * scale, 0.18f * scale), warningMaterial);
        valveWheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube(name + " Valve Spoke Horizontal", valveRoot.transform, Vector3.zero, new Vector3(0.34f * scale, 0.025f * scale, 0.028f * scale), brassMaterial);
        CreateLocalCube(name + " Valve Spoke Vertical", valveRoot.transform, Vector3.zero, new Vector3(0.028f * scale, 0.34f * scale, 0.025f * scale), brassMaterial);
        CreateLocalPrimitive(name + " Valve Hub", PrimitiveType.Sphere, valveRoot.transform, new Vector3(0f, 0f, -0.035f * scale), new Vector3(0.055f * scale, 0.055f * scale, 0.025f * scale), brassMaterial);

        GameObject rivetRoot = CreateLocalEmpty(name + " Rivet Root", root.transform, Vector3.zero, Quaternion.identity);
        for (int i = 0; i < 7; i++)
        {
            float x = Mathf.Lerp(-0.76f * scale, 0.76f * scale, i / 6f);
            CreateLocalPrimitive(name + " Top Slotted Rivet " + i.ToString("00"), PrimitiveType.Sphere, rivetRoot.transform, new Vector3(x, 0.58f * scale, -0.085f * scale), new Vector3(0.035f * scale, 0.035f * scale, 0.018f * scale), brassMaterial);
            CreateLocalPrimitive(name + " Bottom Slotted Rivet " + i.ToString("00"), PrimitiveType.Sphere, rivetRoot.transform, new Vector3(x, -0.58f * scale, -0.085f * scale), new Vector3(0.035f * scale, 0.035f * scale, 0.018f * scale), brassMaterial);
        }

        prototype.backplateRenderer = backplate.GetComponent<Renderer>();
        prototype.primaryPipeRenderer = primaryPipe.GetComponent<Renderer>();
        prototype.secondaryPipeRenderer = secondaryPipe.GetComponent<Renderer>();
        prototype.gaugeFaceRenderer = gaugeFaceA.GetComponent<Renderer>();
        prototype.valveWheelRenderer = valveWheel.GetComponent<Renderer>();
        prototype.pipeRoot = pipeRoot.transform;
        prototype.gaugeRoot = gaugeRoot.transform;
        prototype.valveRoot = valveRoot.transform;
        prototype.rivetRoot = rivetRoot.transform;
        return prototype;
    }

    private static GameObject CreateWallClusterGauge(string name, Transform parent, Vector3 localPosition, float radius, Material brassMaterial, Material gaugeFaceMaterial, Material warningMaterial)
    {
        GameObject gaugeRoot = CreateLocalEmpty(name, parent, localPosition, Quaternion.identity);
        GameObject bezel = CreateLocalPrimitive(name + " Aged Brass Bezel", PrimitiveType.Cylinder, gaugeRoot.transform, Vector3.zero, new Vector3(radius, radius * 0.11f, radius), brassMaterial);
        bezel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject face = CreateLocalPrimitive(name + " Cream Enamel Face", PrimitiveType.Cylinder, gaugeRoot.transform, new Vector3(0f, 0f, -radius * 0.14f), new Vector3(radius * 0.78f, radius * 0.05f, radius * 0.78f), gaugeFaceMaterial);
        face.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube(name + " Red Needle", gaugeRoot.transform, new Vector3(radius * 0.17f, 0f, -radius * 0.21f), new Vector3(radius * 0.72f, radius * 0.055f, radius * 0.035f), warningMaterial);
        CreateLocalPrimitive(name + " Brass Needle Hub", PrimitiveType.Sphere, gaugeRoot.transform, new Vector3(0f, 0f, -radius * 0.24f), new Vector3(radius * 0.16f, radius * 0.16f, radius * 0.08f), brassMaterial);
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 tickPosition = new Vector3(Mathf.Sin(radians) * radius * 0.58f, Mathf.Cos(radians) * radius * 0.58f, -radius * 0.25f);
            GameObject tick = CreateLocalCube(name + " Tick " + i.ToString("00"), gaugeRoot.transform, tickPosition, new Vector3(radius * 0.03f, radius * 0.12f, radius * 0.03f), warningMaterial);
            tick.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        return face;
    }

    private static BoilerControlConsolePrototype CreateBoilerControlConsolePrototype(string name, Vector3 position, Quaternion rotation, float scale, Material brassMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material warningMaterial, string placementRole, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        BoilerControlConsolePrototype prototype = root.AddComponent<BoilerControlConsolePrototype>();
        prototype.placementRole = placementRole;
        prototype.leverCount = 3;
        prototype.gaugeCount = 2;
        prototype.lampCount = 3;
        prototype.rivetCount = 12;
        prototype.pipeCount = 3;

        GameObject baseBlock = CreateLocalCube(name + " Blackened Iron Pedestal Base", root.transform, new Vector3(0f, -0.22f * scale, 0f), new Vector3(1.2f * scale, 0.42f * scale, 0.78f * scale), ironMaterial);
        GameObject panel = CreateLocalCube(name + " Angled Iron Control Panel", root.transform, new Vector3(0f, 0.16f * scale, -0.1f * scale), new Vector3(1.18f * scale, 0.16f * scale, 0.72f * scale), ironMaterial);
        panel.transform.localRotation = Quaternion.Euler(-18f, 0f, 0f);
        GameObject brassRail = CreateLocalCube(name + " Aged Brass Front Control Rail", root.transform, new Vector3(0f, 0.22f * scale, -0.49f * scale), new Vector3(1.08f * scale, 0.07f * scale, 0.08f * scale), brassMaterial);
        CreateLocalCube(name + " Aged Brass Rear Control Rail", root.transform, new Vector3(0f, 0.38f * scale, 0.18f * scale), new Vector3(1.04f * scale, 0.06f * scale, 0.08f * scale), brassMaterial);
        CreateLocalCube(name + " Brass Left Beveled Cheek", root.transform, new Vector3(-0.64f * scale, 0.08f * scale, -0.08f * scale), new Vector3(0.08f * scale, 0.42f * scale, 0.72f * scale), brassMaterial);
        CreateLocalCube(name + " Brass Right Beveled Cheek", root.transform, new Vector3(0.64f * scale, 0.08f * scale, -0.08f * scale), new Vector3(0.08f * scale, 0.42f * scale, 0.72f * scale), brassMaterial);

        GameObject leverRoot = CreateLocalEmpty(name + " Lever Root", root.transform, Vector3.zero, Quaternion.identity);
        GameObject firstLeverHandle = null;
        for (int i = 0; i < 3; i++)
        {
            float x = Mathf.Lerp(-0.34f * scale, 0.34f * scale, i / 2f);
            GameObject leverStem = CreateLocalCube(name + " Iron Lever Stem " + i.ToString("00"), leverRoot.transform, new Vector3(x, 0.35f * scale, -0.24f * scale), new Vector3(0.045f * scale, 0.38f * scale, 0.045f * scale), ironMaterial);
            leverStem.transform.localRotation = Quaternion.Euler(-20f + (i * 12f), 0f, 0f);
            GameObject leverHandle = CreateLocalPrimitive(name + " Red Brass Lever Handle " + i.ToString("00"), PrimitiveType.Sphere, leverRoot.transform, new Vector3(x, 0.55f * scale, -0.34f * scale), new Vector3(0.09f * scale, 0.07f * scale, 0.09f * scale), warningMaterial);
            if (firstLeverHandle == null)
            {
                firstLeverHandle = leverHandle;
            }
        }

        GameObject gaugeRoot = CreateLocalEmpty(name + " Gauge Root", root.transform, Vector3.zero, Quaternion.identity);
        GameObject firstGaugeFace = CreateConsoleGauge(name + " Left Boiler Pressure Gauge", gaugeRoot.transform, new Vector3(-0.31f * scale, 0.42f * scale, 0.08f * scale), 0.18f * scale, brassMaterial, gaugeFaceMaterial, warningMaterial);
        CreateConsoleGauge(name + " Right Boiler Pressure Gauge", gaugeRoot.transform, new Vector3(0.31f * scale, 0.42f * scale, 0.08f * scale), 0.18f * scale, brassMaterial, gaugeFaceMaterial, warningMaterial);

        GameObject lampRoot = CreateLocalEmpty(name + " Lamp Root", root.transform, Vector3.zero, Quaternion.identity);
        GameObject firstLamp = null;
        for (int i = 0; i < 3; i++)
        {
            float x = Mathf.Lerp(-0.38f * scale, 0.38f * scale, i / 2f);
            GameObject lamp = CreateLocalPrimitive(name + " Amber Indicator Lamp " + i.ToString("00"), PrimitiveType.Sphere, lampRoot.transform, new Vector3(x, 0.22f * scale, -0.52f * scale), new Vector3(0.075f * scale, 0.075f * scale, 0.045f * scale), warningMaterial);
            if (firstLamp == null)
            {
                firstLamp = lamp;
            }
            CreateLocalCube(name + " Brass Lamp Bezel " + i.ToString("00"), lampRoot.transform, new Vector3(x, 0.22f * scale, -0.555f * scale), new Vector3(0.16f * scale, 0.035f * scale, 0.025f * scale), brassMaterial);
        }

        GameObject pipeRoot = CreateLocalEmpty(name + " Pipe Root", root.transform, Vector3.zero, Quaternion.identity);
        GameObject firstPipe = null;
        for (int i = 0; i < 3; i++)
        {
            float x = Mathf.Lerp(-0.45f * scale, 0.45f * scale, i / 2f);
            GameObject pipe = CreateLocalPrimitive(name + " Copper Pressure Pipe " + i.ToString("00"), PrimitiveType.Cylinder, pipeRoot.transform, new Vector3(x, -0.04f * scale, 0.48f * scale), new Vector3(0.04f * scale, 0.38f * scale, 0.04f * scale), brassMaterial);
            pipe.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            if (firstPipe == null)
            {
                firstPipe = pipe;
            }
        }

        GameObject valveWheel = CreateLocalPrimitive(name + " Side Brass Valve Wheel", PrimitiveType.Cylinder, root.transform, new Vector3(0.72f * scale, 0.18f * scale, -0.12f * scale), new Vector3(0.18f * scale, 0.025f * scale, 0.18f * scale), brassMaterial);
        valveWheel.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        CreateLocalCube(name + " Side Valve Wheel Crossbar A", root.transform, new Vector3(0.75f * scale, 0.18f * scale, -0.12f * scale), new Vector3(0.035f * scale, 0.32f * scale, 0.025f * scale), brassMaterial);
        CreateLocalCube(name + " Side Valve Wheel Crossbar B", root.transform, new Vector3(0.75f * scale, 0.18f * scale, -0.12f * scale), new Vector3(0.035f * scale, 0.025f * scale, 0.32f * scale), brassMaterial);

        GameObject rivetRoot = CreateLocalEmpty(name + " Rivet Root", root.transform, Vector3.zero, Quaternion.identity);
        for (int i = 0; i < 6; i++)
        {
            float x = Mathf.Lerp(-0.52f * scale, 0.52f * scale, i / 5f);
            CreateLocalPrimitive(name + " Front Slotted Rivet " + i.ToString("00"), PrimitiveType.Sphere, rivetRoot.transform, new Vector3(x, 0.31f * scale, -0.56f * scale), new Vector3(0.035f * scale, 0.035f * scale, 0.018f * scale), brassMaterial);
            CreateLocalPrimitive(name + " Rear Slotted Rivet " + i.ToString("00"), PrimitiveType.Sphere, rivetRoot.transform, new Vector3(x, 0.42f * scale, 0.25f * scale), new Vector3(0.035f * scale, 0.035f * scale, 0.018f * scale), brassMaterial);
        }

        prototype.baseRenderer = baseBlock.GetComponent<Renderer>();
        prototype.panelRenderer = panel.GetComponent<Renderer>();
        prototype.brassRailRenderer = brassRail.GetComponent<Renderer>();
        prototype.gaugeFaceRenderer = firstGaugeFace.GetComponent<Renderer>();
        prototype.leverHandleRenderer = firstLeverHandle.GetComponent<Renderer>();
        prototype.lampRenderer = firstLamp.GetComponent<Renderer>();
        prototype.pipeRenderer = firstPipe.GetComponent<Renderer>();
        prototype.leverRoot = leverRoot.transform;
        prototype.gaugeRoot = gaugeRoot.transform;
        prototype.lampRoot = lampRoot.transform;
        prototype.rivetRoot = rivetRoot.transform;
        prototype.pipeRoot = pipeRoot.transform;
        return prototype;
    }

    private static GameObject CreateConsoleGauge(string name, Transform parent, Vector3 localPosition, float radius, Material brassMaterial, Material gaugeFaceMaterial, Material warningMaterial)
    {
        GameObject gaugeRoot = CreateLocalEmpty(name, parent, localPosition, Quaternion.identity);
        GameObject bezel = CreateLocalPrimitive(name + " Aged Brass Bezel", PrimitiveType.Cylinder, gaugeRoot.transform, Vector3.zero, new Vector3(radius, radius * 0.1f, radius), brassMaterial);
        bezel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject face = CreateLocalPrimitive(name + " Cream Enamel Face", PrimitiveType.Cylinder, gaugeRoot.transform, new Vector3(0f, 0f, -radius * 0.12f), new Vector3(radius * 0.78f, radius * 0.045f, radius * 0.78f), gaugeFaceMaterial);
        face.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube(name + " Red Needle", gaugeRoot.transform, new Vector3(radius * 0.12f, 0f, -radius * 0.2f), new Vector3(radius * 0.56f, radius * 0.045f, radius * 0.03f), warningMaterial);
        CreateLocalPrimitive(name + " Brass Needle Hub", PrimitiveType.Sphere, gaugeRoot.transform, new Vector3(0f, 0f, -radius * 0.22f), new Vector3(radius * 0.13f, radius * 0.13f, radius * 0.07f), brassMaterial);
        return face;
    }

    private static void CreatePipeCanopy(string name, Vector3 position, Quaternion rotation, float length, Material pipeMaterial, Material bracketMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        Vector3[] offsets =
        {
            new Vector3(-0.33f, 0f, 0f),
            new Vector3(-0.11f, 0.1f, 0f),
            new Vector3(0.11f, 0.1f, 0f),
            new Vector3(0.33f, 0f, 0f)
        };

        for (int i = 0; i < offsets.Length; i++)
        {
            GameObject pipe = CreateLocalPrimitive(name + " Pipe " + i, PrimitiveType.Cylinder, root.transform, offsets[i], new Vector3(0.07f, length * 0.5f, 0.07f), pipeMaterial);
            pipe.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        for (int i = 0; i < 5; i++)
        {
            float z = Mathf.Lerp(-length * 0.42f, length * 0.42f, i / 4f);
            CreateLocalCube(name + " Iron Collar " + i, root.transform, new Vector3(0f, 0.02f, z), new Vector3(0.92f, 0.1f, 0.09f), bracketMaterial);
            CreateLocalPrimitive(name + " Collar Rivet Left " + i, PrimitiveType.Sphere, root.transform, new Vector3(-0.44f, 0.02f, z - 0.055f), new Vector3(0.04f, 0.04f, 0.04f), pipeMaterial);
            CreateLocalPrimitive(name + " Collar Rivet Right " + i, PrimitiveType.Sphere, root.transform, new Vector3(0.44f, 0.02f, z - 0.055f), new Vector3(0.04f, 0.04f, 0.04f), pipeMaterial);
        }
    }

    private static void CreateCagedGaslight(string name, Vector3 position, Quaternion rotation, Material ironMaterial, Material brassMaterial, Material glowMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        CreateLocalCube(name + " Wall Bracket", root.transform, new Vector3(0f, 0.12f, 0.16f), new Vector3(0.18f, 0.58f, 0.08f), ironMaterial);
        CreateLocalCube(name + " Brass Top Cap", root.transform, new Vector3(0f, 0.36f, -0.02f), new Vector3(0.42f, 0.08f, 0.18f), brassMaterial);
        CreateLocalCube(name + " Brass Bottom Cap", root.transform, new Vector3(0f, -0.36f, -0.02f), new Vector3(0.42f, 0.08f, 0.18f), brassMaterial);
        CreateLocalPrimitive(name + " Amber Glass", PrimitiveType.Sphere, root.transform, new Vector3(0f, 0f, -0.05f), new Vector3(0.24f, 0.38f, 0.16f), glowMaterial);

        for (int i = 0; i < 4; i++)
        {
            float x = i < 2 ? -0.23f : 0.23f;
            float z = i % 2 == 0 ? -0.13f : 0.03f;
            CreateLocalCube(name + " Iron Cage Rib " + i, root.transform, new Vector3(x, 0f, z), new Vector3(0.035f, 0.76f, 0.035f), ironMaterial);
        }

        GameObject lightObject = new GameObject(name + " Warm Point Light");
        lightObject.transform.SetParent(root.transform, false);
        lightObject.transform.localPosition = new Vector3(0f, 0f, -0.2f);
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.58f, 0.16f);
        light.intensity = 1.25f;
        light.range = 4.1f;
        light.shadows = LightShadows.None;
    }

    private static void CreateRivetBand(string name, Vector3 position, Quaternion rotation, float width, Material ironMaterial, Material brassMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        CreateLocalCube(name + " Iron Strap", root.transform, Vector3.zero, new Vector3(width, 0.12f, 0.08f), ironMaterial);
        int rivetCount = Mathf.Max(4, Mathf.RoundToInt(width / 0.32f));
        for (int i = 0; i < rivetCount; i++)
        {
            float x = Mathf.Lerp(-width * 0.45f, width * 0.45f, rivetCount == 1 ? 0f : i / (float)(rivetCount - 1));
            CreateLocalPrimitive(name + " Brass Rivet " + i, PrimitiveType.Sphere, root.transform, new Vector3(x, 0f, -0.065f), new Vector3(0.055f, 0.055f, 0.026f), brassMaterial);
        }
    }

    private static void CreateCatwalkRail(string name, Vector3 position, float length, Material ironMaterial, Material brassMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;

        CreateLocalCube(name + " Upper Brass Rail", root.transform, new Vector3(0f, 0.48f, 0f), new Vector3(length, 0.08f, 0.08f), brassMaterial);
        CreateLocalCube(name + " Lower Iron Rail", root.transform, new Vector3(0f, 0.18f, 0f), new Vector3(length, 0.07f, 0.07f), ironMaterial);
        for (int i = 0; i < 7; i++)
        {
            float x = Mathf.Lerp(-length * 0.48f, length * 0.48f, i / 6f);
            CreateLocalCube(name + " Upright " + i, root.transform, new Vector3(x, 0.18f, 0f), new Vector3(0.06f, 0.66f, 0.06f), ironMaterial);
            CreateLocalPrimitive(name + " Brass Cap " + i, PrimitiveType.Sphere, root.transform, new Vector3(x, 0.54f, 0f), new Vector3(0.07f, 0.07f, 0.07f), brassMaterial);
        }
    }

    private static void CreateRegulatorCrown(string name, Vector3 position, Material ironMaterial, Material brassMaterial, Material warningMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;

        AddSpinner(root, 9f);
        CreateLocalPrimitive(name + " Outer Gear Ring", PrimitiveType.Cylinder, root.transform, Vector3.zero, new Vector3(0.86f, 0.08f, 0.86f), brassMaterial);
        CreateLocalPrimitive(name + " Inner Iron Hub", PrimitiveType.Cylinder, root.transform, new Vector3(0f, 0.06f, 0f), new Vector3(0.36f, 0.08f, 0.36f), ironMaterial);
        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.74f, 0.06f, Mathf.Cos(radians) * 0.74f);
            GameObject tooth = CreateLocalCube(name + " Brass Tooth " + i, root.transform, toothPosition, new Vector3(0.13f, 0.09f, 0.22f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        CreateLocalCube(name + " Red Governor Needle", root.transform, new Vector3(0.28f, 0.13f, 0f), new Vector3(0.55f, 0.035f, 0.035f), warningMaterial);
    }

    private static void CreateWorkOrderBoard(string name, string text, Vector3 position, Quaternion rotation, Material boardMaterial, Material paperMaterial, Material inkMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        CreateLocalCube(name + " Iron Backboard", root.transform, Vector3.zero, new Vector3(1.34f, 0.78f, 0.08f), boardMaterial);
        CreateLocalCube(name + " Cream Work Sheet", root.transform, new Vector3(0f, 0f, -0.055f), new Vector3(1.06f, 0.58f, 0.025f), paperMaterial);
        CreateLocalCube(name + " Brass Header Clip", root.transform, new Vector3(0f, 0.34f, -0.08f), new Vector3(1.12f, 0.08f, 0.04f), inkMaterial);

        GameObject textObject = new GameObject(name + " Text");
        textObject.transform.SetParent(root.transform, false);
        textObject.transform.localPosition = new Vector3(-0.46f, 0.12f, -0.095f);
        textObject.transform.localRotation = Quaternion.identity;
        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = TextAnchor.UpperLeft;
        textMesh.alignment = TextAlignment.Left;
        textMesh.characterSize = 0.075f;
        textMesh.fontSize = 42;
        textMesh.color = new Color(0.14f, 0.055f, 0.025f);
    }

    private static LorePlaque CreateLorePlaque(string name, string title, string body, Vector3 position, Quaternion rotation, Material boardMaterial, Material plateMaterial, Material accentMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        BoxCollider trigger = root.AddComponent<BoxCollider>();
        trigger.size = new Vector3(1.2f, 0.72f, 0.18f);
        trigger.isTrigger = true;

        LorePlaque plaque = root.AddComponent<LorePlaque>();
        plaque.plaqueId = name;
        plaque.title = title;
        plaque.body = body;
        plaque.prompt = "E - read plaque";

        CreateLocalCube(name + " Riveted Backplate", root.transform, Vector3.zero, new Vector3(1.2f, 0.72f, 0.08f), boardMaterial);
        CreateLocalCube(name + " Enamel Story Plate", root.transform, new Vector3(0f, 0f, -0.055f), new Vector3(0.96f, 0.5f, 0.025f), plateMaterial);
        CreateLocalCube(name + " Brass Archive Strip", root.transform, new Vector3(0f, 0.31f, -0.08f), new Vector3(1.04f, 0.07f, 0.04f), accentMaterial);
        CreateLocalPrimitive(name + " Left Rivet", PrimitiveType.Sphere, root.transform, new Vector3(-0.52f, 0.29f, -0.1f), new Vector3(0.055f, 0.055f, 0.025f), accentMaterial);
        CreateLocalPrimitive(name + " Right Rivet", PrimitiveType.Sphere, root.transform, new Vector3(0.52f, 0.29f, -0.1f), new Vector3(0.055f, 0.055f, 0.025f), accentMaterial);

        GameObject textObject = new GameObject(name + " Text");
        textObject.transform.SetParent(root.transform, false);
        textObject.transform.localPosition = new Vector3(-0.38f, 0.14f, -0.095f);
        textObject.transform.localRotation = Quaternion.identity;
        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = "ARCHIVE\n" + title.ToUpperInvariant();
        textMesh.anchor = TextAnchor.UpperLeft;
        textMesh.alignment = TextAlignment.Left;
        textMesh.characterSize = 0.058f;
        textMesh.fontSize = 36;
        textMesh.color = new Color(0.14f, 0.055f, 0.025f);

        return plaque;
    }

    private static void CreatePressureGauge(string name, Vector3 position, Quaternion rotation, Material brassMaterial, Material faceMaterial, Material needleMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        GameObject bezel = CreateLocalPrimitive(name + " Brass Bezel", PrimitiveType.Cylinder, root.transform, Vector3.zero, new Vector3(0.42f, 0.045f, 0.42f), brassMaterial);
        bezel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject face = CreateLocalPrimitive(name + " Face", PrimitiveType.Cylinder, root.transform, new Vector3(0f, 0f, -0.035f), new Vector3(0.34f, 0.025f, 0.34f), faceMaterial);
        face.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube(name + " Needle", root.transform, new Vector3(0.08f, 0f, -0.07f), new Vector3(0.23f, 0.018f, 0.018f), needleMaterial);
    }

    private static PressureGaugePrototype CreatePressureGaugePrototype(string name, Transform parent, Vector3 localPosition, float size, Material brassMaterial, Material ironMaterial, Material faceMaterial, Material glassMaterial, Material warningMaterial, string placementRole)
    {
        float radius = size * 0.5f;
        float depth = size * 0.08f;

        GameObject root = CreateLocalEmpty(name, parent, localPosition, Quaternion.identity);
        PressureGaugePrototype prototype = root.AddComponent<PressureGaugePrototype>();
        prototype.placementRole = placementRole;
        prototype.tickMarkCount = 16;

        GameObject backplate = CreateLocalPrimitive(name + " Blackened Iron Backplate", PrimitiveType.Cylinder, root.transform, new Vector3(0f, 0f, depth * 0.45f), new Vector3(radius * 1.08f, depth * 0.8f, radius * 1.08f), ironMaterial);
        backplate.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        GameObject bezel = CreateLocalPrimitive(name + " Aged Brass Bezel", PrimitiveType.Cylinder, root.transform, Vector3.zero, new Vector3(radius, depth, radius), brassMaterial);
        bezel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        GameObject face = CreateLocalPrimitive(name + " Cream Enamel Face", PrimitiveType.Cylinder, root.transform, new Vector3(0f, 0f, -depth * 0.75f), new Vector3(radius * 0.78f, depth * 0.42f, radius * 0.78f), faceMaterial);
        face.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        GameObject glass = CreateLocalPrimitive(name + " Amber Glass Lens", PrimitiveType.Cylinder, root.transform, new Vector3(0f, 0f, -depth * 1.22f), new Vector3(radius * 0.72f, depth * 0.22f, radius * 0.72f), glassMaterial);
        glass.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        GameObject warningBand = CreateLocalCube(name + " Red Pressure Warning Band", root.transform, new Vector3(0f, -radius * 0.7f, -depth * 1.55f), new Vector3(radius * 1.05f, radius * 0.1f, depth * 0.42f), warningMaterial);
        GameObject highlight = CreateLocalCube(name + " Glass Crescent Highlight", root.transform, new Vector3(-radius * 0.2f, radius * 0.25f, -depth * 1.7f), new Vector3(radius * 0.45f, radius * 0.045f, depth * 0.34f), glassMaterial);
        highlight.transform.localRotation = Quaternion.Euler(0f, 0f, -18f);

        GameObject tickRoot = CreateLocalEmpty(name + " Tick Root", root.transform, Vector3.zero, Quaternion.identity);
        for (int i = 0; i < 16; i++)
        {
            float angle = i * 22.5f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 tickPosition = new Vector3(Mathf.Sin(radians) * radius * 0.56f, Mathf.Cos(radians) * radius * 0.56f, -depth * 1.7f);
            GameObject tick = CreateLocalCube(name + " Tick " + i.ToString("00"), tickRoot.transform, tickPosition, new Vector3(radius * 0.035f, radius * (i % 4 == 0 ? 0.18f : 0.1f), depth * 0.22f), ironMaterial);
            tick.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        GameObject rivetRoot = CreateLocalEmpty(name + " Rivet Root", root.transform, Vector3.zero, Quaternion.identity);
        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 rivetPosition = new Vector3(Mathf.Sin(radians) * radius * 0.86f, Mathf.Cos(radians) * radius * 0.86f, -depth * 0.92f);
            CreateLocalPrimitive(name + " Bezel Rivet " + i.ToString("00"), PrimitiveType.Sphere, rivetRoot.transform, rivetPosition, new Vector3(radius * 0.085f, radius * 0.085f, depth * 0.48f), brassMaterial);
        }

        GameObject needlePivot = CreateLocalEmpty(name + " Needle Pivot", root.transform, new Vector3(0f, 0f, -depth * 2f), Quaternion.Euler(0f, 0f, -32f));
        GameObject needle = CreateLocalCube(name + " Needle", needlePivot.transform, new Vector3(radius * 0.22f, 0f, 0f), new Vector3(radius * 0.82f, radius * 0.045f, depth * 0.32f), warningMaterial);
        CreateLocalPrimitive(name + " Needle Hub", PrimitiveType.Sphere, needlePivot.transform, Vector3.zero, new Vector3(radius * 0.12f, radius * 0.12f, depth * 0.5f), brassMaterial);

        GameObject lowerPipe = CreateLocalPrimitive(name + " Lower Pipe Nipple", PrimitiveType.Cylinder, root.transform, new Vector3(0f, -radius * 1.04f, -depth * 0.15f), new Vector3(radius * 0.12f, radius * 0.22f, radius * 0.12f), brassMaterial);
        lowerPipe.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        prototype.bezelRenderer = bezel.GetComponent<Renderer>();
        prototype.backplateRenderer = backplate.GetComponent<Renderer>();
        prototype.faceRenderer = face.GetComponent<Renderer>();
        prototype.glassRenderer = glass.GetComponent<Renderer>();
        prototype.warningBandRenderer = warningBand.GetComponent<Renderer>();
        prototype.needlePivot = needlePivot.transform;
        prototype.needle = needle.transform;
        prototype.tickRoot = tickRoot.transform;
        prototype.rivetRoot = rivetRoot.transform;
        return prototype;
    }

    private static PressureCoilPrototype CreatePressureCoilPrototype(string name, Transform parent, Vector3 localPosition, float size, Material brassMaterial, Material ironMaterial, Material heatMaterial, string placementRole)
    {
        float width = size;
        float height = size * 0.46f;
        float depth = size * 0.12f;

        GameObject root = CreateLocalEmpty(name, parent, localPosition, Quaternion.Euler(0f, 0f, -5f));
        PressureCoilPrototype prototype = root.AddComponent<PressureCoilPrototype>();
        prototype.placementRole = placementRole;
        prototype.coilTurnCount = 9;
        prototype.rivetCount = 18;

        GameObject backingPlate = CreateLocalCube(name + " Blackened Iron Backing Plate", root.transform, Vector3.zero, new Vector3(width, height, depth), ironMaterial);
        GameObject upperRail = CreateLocalCube(name + " Aged Brass Upper Rail", root.transform, new Vector3(0f, height * 0.6f, -depth * 0.15f), new Vector3(width * 0.95f, height * 0.13f, depth * 0.75f), brassMaterial);
        GameObject lowerRail = CreateLocalCube(name + " Aged Brass Lower Rail", root.transform, new Vector3(0f, -height * 0.6f, -depth * 0.15f), new Vector3(width * 0.95f, height * 0.13f, depth * 0.75f), brassMaterial);
        GameObject heatCore = CreateLocalCube(name + " Dull Red Ceramic Heat Core", root.transform, new Vector3(0f, 0f, -depth * 0.72f), new Vector3(width * 0.7f, height * 0.16f, depth * 0.62f), heatMaterial);
        CreateLocalCube(name + " Sooted Recess Shadow", root.transform, new Vector3(0f, 0f, -depth * 0.32f), new Vector3(width * 0.76f, height * 0.62f, depth * 0.18f), ironMaterial);

        GameObject upperManifold = CreateLocalPrimitive(name + " Upper Copper Manifold", PrimitiveType.Cylinder, root.transform, new Vector3(0f, height * 0.31f, -depth * 0.66f), new Vector3(height * 0.055f, width * 0.46f, height * 0.055f), brassMaterial);
        upperManifold.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        GameObject lowerManifold = CreateLocalPrimitive(name + " Lower Copper Manifold", PrimitiveType.Cylinder, root.transform, new Vector3(0f, -height * 0.31f, -depth * 0.66f), new Vector3(height * 0.055f, width * 0.46f, height * 0.055f), brassMaterial);
        lowerManifold.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        GameObject coilTurnRoot = CreateLocalEmpty(name + " Coil Turn Root", root.transform, Vector3.zero, Quaternion.identity);
        for (int i = 0; i < 9; i++)
        {
            float normalized = i / 8f;
            float x = Mathf.Lerp(-width * 0.36f, width * 0.36f, normalized);
            GameObject turn = CreateLocalPrimitive(name + " Oxidized Copper Coil Turn " + i.ToString("00"), PrimitiveType.Cylinder, coilTurnRoot.transform, new Vector3(x, 0f, -depth * 0.92f - (i % 2) * depth * 0.08f), new Vector3(height * 0.31f, width * 0.018f, height * 0.18f), brassMaterial);
            turn.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            CreateLocalCube(name + " Deep Coil Gap " + i.ToString("00"), coilTurnRoot.transform, new Vector3(x, 0f, -depth * 0.42f), new Vector3(width * 0.026f, height * 0.58f, depth * 0.12f), ironMaterial);
        }

        GameObject rivetRoot = CreateLocalEmpty(name + " Rivet Root", root.transform, Vector3.zero, Quaternion.identity);
        for (int i = 0; i < 9; i++)
        {
            float normalized = i / 8f;
            float x = Mathf.Lerp(-width * 0.42f, width * 0.42f, normalized);
            CreateLocalPrimitive(name + " Upper Slotted Rivet " + i.ToString("00"), PrimitiveType.Sphere, rivetRoot.transform, new Vector3(x, height * 0.68f, -depth * 0.72f), new Vector3(width * 0.035f, width * 0.035f, depth * 0.32f), brassMaterial);
            CreateLocalPrimitive(name + " Lower Slotted Rivet " + i.ToString("00"), PrimitiveType.Sphere, rivetRoot.transform, new Vector3(x, -height * 0.68f, -depth * 0.72f), new Vector3(width * 0.032f, width * 0.032f, depth * 0.3f), brassMaterial);
        }

        GameObject pressureLeadRoot = CreateLocalEmpty(name + " Pressure Lead Root", root.transform, Vector3.zero, Quaternion.identity);
        GameObject leadA = CreateLocalPrimitive(name + " Left Braided Pressure Lead", PrimitiveType.Cylinder, pressureLeadRoot.transform, new Vector3(-width * 0.55f, height * 0.38f, -depth * 0.62f), new Vector3(width * 0.025f, width * 0.18f, width * 0.025f), ironMaterial);
        leadA.transform.localRotation = Quaternion.Euler(0f, 0f, -58f);
        GameObject leadB = CreateLocalPrimitive(name + " Right Braided Pressure Lead", PrimitiveType.Cylinder, pressureLeadRoot.transform, new Vector3(width * 0.55f, -height * 0.38f, -depth * 0.62f), new Vector3(width * 0.025f, width * 0.18f, width * 0.025f), ironMaterial);
        leadB.transform.localRotation = Quaternion.Euler(0f, 0f, -58f);
        CreateLocalPrimitive(name + " Left Patina Bloom", PrimitiveType.Sphere, pressureLeadRoot.transform, new Vector3(-width * 0.41f, height * 0.48f, -depth * 0.88f), new Vector3(width * 0.055f, width * 0.025f, depth * 0.18f), brassMaterial);
        CreateLocalPrimitive(name + " Right Patina Bloom", PrimitiveType.Sphere, pressureLeadRoot.transform, new Vector3(width * 0.41f, -height * 0.48f, -depth * 0.88f), new Vector3(width * 0.055f, width * 0.025f, depth * 0.18f), brassMaterial);

        prototype.backingPlateRenderer = backingPlate.GetComponent<Renderer>();
        prototype.upperRailRenderer = upperRail.GetComponent<Renderer>();
        prototype.lowerRailRenderer = lowerRail.GetComponent<Renderer>();
        prototype.heatCoreRenderer = heatCore.GetComponent<Renderer>();
        prototype.coilTurnRoot = coilTurnRoot.transform;
        prototype.rivetRoot = rivetRoot.transform;
        prototype.pressureLeadRoot = pressureLeadRoot.transform;
        return prototype;
    }

    private static void CreateValveWheel(string name, Vector3 position, Quaternion rotation, Material brassMaterial, Material warningMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        GameObject wheelAssembly = CreateLocalEmpty(name + " Wheel Assembly", root.transform, Vector3.zero, Quaternion.Euler(90f, 0f, 0f));
        AddSpinner(wheelAssembly, 22f);
        CreateLocalPrimitive(name + " Wheel", PrimitiveType.Cylinder, wheelAssembly.transform, Vector3.zero, new Vector3(0.5f, 0.04f, 0.5f), brassMaterial);
        CreateLocalCube(name + " Spoke Horizontal", wheelAssembly.transform, new Vector3(0f, -0.02f, 0f), new Vector3(0.88f, 0.045f, 0.04f), warningMaterial);
        CreateLocalCube(name + " Spoke Vertical", wheelAssembly.transform, new Vector3(0f, -0.02f, 0f), new Vector3(0.04f, 0.045f, 0.88f), warningMaterial);
        CreateLocalPrimitive(name + " Hub", PrimitiveType.Sphere, wheelAssembly.transform, new Vector3(0f, -0.05f, 0f), new Vector3(0.14f, 0.14f, 0.14f), brassMaterial);
    }

    private static void CreateSteamVent(string name, Vector3 position, Material ironMaterial, Material steamMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;

        CreateDecoCube(name + " Grate", position, new Vector3(0.82f, 0.12f, 0.82f), ironMaterial, root.transform);
        CreateLocalPrimitive(name + " Puff Low", PrimitiveType.Sphere, root.transform, new Vector3(-0.1f, 0.5f, 0.02f), new Vector3(0.42f, 0.28f, 0.42f), steamMaterial);
        CreateLocalPrimitive(name + " Puff High", PrimitiveType.Sphere, root.transform, new Vector3(0.12f, 0.9f, -0.05f), new Vector3(0.34f, 0.46f, 0.34f), steamMaterial);
    }

    private static void CreateFurnace(string name, Vector3 position, Material ironMaterial, Material brassMaterial, Material glowMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;

        CreateLocalCube(name + " Body", root.transform, Vector3.zero, new Vector3(1.15f, 1.55f, 0.65f), ironMaterial);
        CreateLocalCube(name + " Brass Header", root.transform, new Vector3(0f, 0.62f, -0.36f), new Vector3(1.25f, 0.16f, 0.08f), brassMaterial);
        CreateLocalCube(name + " Furnace Mouth", root.transform, new Vector3(0f, -0.1f, -0.38f), new Vector3(0.82f, 0.52f, 0.08f), glowMaterial);
        CreateLocalCube(name + " Chimney", root.transform, new Vector3(0f, 1.05f, 0f), new Vector3(0.38f, 0.72f, 0.38f), ironMaterial);
    }

    private static void CreateCableRun(string name, Vector3 position, Vector3 scale, Material material, Transform parent)
    {
        CreateDecoCube(name, position, scale, material, parent);
    }

    private static void CreateServerStack(string name, Vector3 position, Material bodyMaterial, Material primaryLightMaterial, Material secondaryLightMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;

        CreateDecoCube(name + " Body", position, new Vector3(0.75f, 2.2f, 0.55f), bodyMaterial, root.transform);
        CreateDecoCube(name + " Primary Light 01", position + new Vector3(0f, 0.55f, -0.3f), new Vector3(0.52f, 0.08f, 0.04f), primaryLightMaterial, root.transform);
        CreateDecoCube(name + " Primary Light 02", position + new Vector3(0f, 0.15f, -0.3f), new Vector3(0.52f, 0.08f, 0.04f), primaryLightMaterial, root.transform);
        CreateDecoCube(name + " Secondary Light", position + new Vector3(0f, -0.35f, -0.3f), new Vector3(0.38f, 0.08f, 0.04f), secondaryLightMaterial, root.transform);
    }

    private static GameObject CreateDecoCube(string name, Vector3 position, Vector3 scale, Material material, Transform parent)
    {
        GameObject cube = CreateCube(name, position, scale, material, parent);
        Collider collider = cube.GetComponent<Collider>();
        if (collider != null)
        {
            UnityEngine.Object.DestroyImmediate(collider);
        }

        return cube;
    }

    private static GameObject CreateLocalEmpty(string name, Transform parent, Vector3 localPosition, Quaternion localRotation)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent, false);
        root.transform.localPosition = localPosition;
        root.transform.localRotation = localRotation;
        return root;
    }

    private static GameObject CreateLocalPrimitive(string name, PrimitiveType type, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
    {
        GameObject primitive = GameObject.CreatePrimitive(type);
        primitive.name = name;
        primitive.transform.SetParent(parent, false);
        primitive.transform.localPosition = localPosition;
        primitive.transform.localRotation = Quaternion.identity;
        primitive.transform.localScale = localScale;

        Collider primitiveCollider = primitive.GetComponent<Collider>();
        if (primitiveCollider != null)
        {
            UnityEngine.Object.DestroyImmediate(primitiveCollider);
        }

        Renderer renderer = primitive.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }

        return primitive;
    }

    private static GameObject CreateLocalCube(string name, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
    {
        return CreateLocalPrimitive(name, PrimitiveType.Cube, parent, localPosition, localScale, material);
    }

    private static void CreateGearKeyPickup(string name, Vector3 position, Vector3 scale, Material brassMaterial, Material ironMaterial, PickupDefinition definition)
    {
        GameObject pickup = new GameObject(name);
        pickup.transform.position = position;

        BoxCollider trigger = pickup.AddComponent<BoxCollider>();
        trigger.size = new Vector3(Mathf.Max(scale.x, 1.35f), Mathf.Max(scale.y, 1.7f), Mathf.Max(scale.z, 1.35f));
        trigger.center = new Vector3(0f, 0.25f, 0f);
        trigger.isTrigger = true;

        Pickup pickupComponent = pickup.AddComponent<Pickup>();
        ConfigurePickup(pickupComponent, definition, PickupKind.Key, 0);

        GameObject visualRoot = new GameObject(name + " Clockwork Key Visual");
        visualRoot.transform.SetParent(pickup.transform, false);
        visualRoot.transform.localPosition = Vector3.zero;
        visualRoot.transform.localRotation = Quaternion.Euler(0f, -18f, 0f);

        GameObject gearDisc = CreateLocalPrimitive(name + " Gear Face", PrimitiveType.Cylinder, visualRoot.transform, new Vector3(0f, 0.34f, 0f), new Vector3(0.48f, 0.065f, 0.48f), brassMaterial);
        gearDisc.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject innerHub = CreateLocalPrimitive(name + " Iron Hub", PrimitiveType.Cylinder, visualRoot.transform, new Vector3(0f, 0.34f, -0.06f), new Vector3(0.18f, 0.055f, 0.18f), ironMaterial);
        innerHub.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        CreateLocalCube(name + " Spoke Horizontal", visualRoot.transform, new Vector3(0f, 0.34f, -0.07f), new Vector3(0.72f, 0.055f, 0.045f), ironMaterial);
        CreateLocalCube(name + " Spoke Vertical", visualRoot.transform, new Vector3(0f, 0.34f, -0.07f), new Vector3(0.055f, 0.72f, 0.045f), ironMaterial);
        GameObject spokeDiagonalA = CreateLocalCube(name + " Spoke Diagonal A", visualRoot.transform, new Vector3(0f, 0.34f, -0.075f), new Vector3(0.6f, 0.045f, 0.045f), ironMaterial);
        spokeDiagonalA.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);
        GameObject spokeDiagonalB = CreateLocalCube(name + " Spoke Diagonal B", visualRoot.transform, new Vector3(0f, 0.34f, -0.075f), new Vector3(0.6f, 0.045f, 0.045f), ironMaterial);
        spokeDiagonalB.transform.localRotation = Quaternion.Euler(0f, 0f, -45f);

        CreateLocalCube(name + " Key Shaft", visualRoot.transform, new Vector3(0f, -0.14f, -0.03f), new Vector3(0.15f, 0.72f, 0.1f), brassMaterial);
        CreateLocalCube(name + " Key Bit Lower", visualRoot.transform, new Vector3(0.18f, -0.54f, -0.03f), new Vector3(0.36f, 0.12f, 0.1f), brassMaterial);
        CreateLocalCube(name + " Key Bit Upper", visualRoot.transform, new Vector3(-0.14f, -0.35f, -0.03f), new Vector3(0.28f, 0.1f, 0.1f), brassMaterial);
        CreateLocalCube(name + " Iron Stem Collar", visualRoot.transform, new Vector3(0f, 0.01f, -0.08f), new Vector3(0.28f, 0.1f, 0.06f), ironMaterial);

        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.52f, 0.34f + Mathf.Cos(radians) * 0.52f, -0.03f);
            GameObject tooth = CreateLocalCube(name + " Tooth " + i, visualRoot.transform, toothPosition, new Vector3(0.12f, 0.18f, 0.09f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        for (int i = 0; i < 3; i++)
        {
            float angle = i * 120f + 20f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 pinPosition = new Vector3(Mathf.Sin(radians) * 0.28f, 0.34f + Mathf.Cos(radians) * 0.28f, -0.11f);
            CreateLocalPrimitive(name + " Iron Pin " + i, PrimitiveType.Sphere, visualRoot.transform, pinPosition, new Vector3(0.055f, 0.055f, 0.055f), ironMaterial);
        }
    }

    private static void CreateHealthVialPickup(string name, Vector3 position, Material crossMaterial, Material glassMaterial, Material fluidMaterial, Material brassMaterial, PickupDefinition definition)
    {
        GameObject pickup = new GameObject(name);
        pickup.transform.position = position;

        BoxCollider trigger = pickup.AddComponent<BoxCollider>();
        trigger.size = new Vector3(0.95f, 1.2f, 0.95f);
        trigger.isTrigger = true;

        Pickup pickupComponent = pickup.AddComponent<Pickup>();
        ConfigurePickup(pickupComponent, definition, PickupKind.Health, GameBalance.HealthPickupAmount);

        CreateLocalPrimitive(name + " Frosted Glass", PrimitiveType.Cylinder, pickup.transform, new Vector3(0f, 0f, 0f), new Vector3(0.24f, 0.46f, 0.24f), glassMaterial);
        CreateLocalPrimitive(name + " Red Fluid", PrimitiveType.Cylinder, pickup.transform, new Vector3(0f, -0.12f, 0f), new Vector3(0.18f, 0.27f, 0.18f), fluidMaterial);
        CreateLocalPrimitive(name + " Top Brass Cap", PrimitiveType.Cylinder, pickup.transform, new Vector3(0f, 0.49f, 0f), new Vector3(0.28f, 0.07f, 0.28f), brassMaterial);
        CreateLocalPrimitive(name + " Bottom Brass Cap", PrimitiveType.Cylinder, pickup.transform, new Vector3(0f, -0.49f, 0f), new Vector3(0.28f, 0.07f, 0.28f), brassMaterial);
        CreateLocalCube(name + " Front Red Cross Vertical", pickup.transform, new Vector3(0f, 0.03f, 0.25f), new Vector3(0.08f, 0.36f, 0.035f), crossMaterial);
        CreateLocalCube(name + " Front Red Cross Horizontal", pickup.transform, new Vector3(0f, 0.03f, 0.27f), new Vector3(0.3f, 0.08f, 0.035f), crossMaterial);
        CreateLocalCube(name + " Brass Side Strap Left", pickup.transform, new Vector3(-0.3f, -0.03f, 0f), new Vector3(0.05f, 0.76f, 0.08f), brassMaterial);
        CreateLocalCube(name + " Brass Side Strap Right", pickup.transform, new Vector3(0.3f, -0.03f, 0f), new Vector3(0.05f, 0.76f, 0.08f), brassMaterial);
    }

    private static void CreatePressureCartridgePickup(string name, Vector3 position, Material cartridgeMaterial, Material ironMaterial, Material brassMaterial, PickupDefinition definition)
    {
        GameObject pickup = new GameObject(name);
        pickup.transform.position = position;

        BoxCollider trigger = pickup.AddComponent<BoxCollider>();
        trigger.size = new Vector3(1.25f, 1f, 1.05f);
        trigger.isTrigger = true;

        Pickup pickupComponent = pickup.AddComponent<Pickup>();
        ConfigurePickup(pickupComponent, definition, PickupKind.Ammo, GameBalance.AmmoPickupAmount);

        CreateLocalCube(name + " Brass Crate Base", pickup.transform, new Vector3(0f, -0.3f, 0f), new Vector3(0.9f, 0.16f, 0.58f), brassMaterial);
        CreateLocalCube(name + " Iron Strap Front", pickup.transform, new Vector3(0f, -0.16f, 0.33f), new Vector3(1.02f, 0.08f, 0.07f), ironMaterial);
        CreateLocalCube(name + " Iron Strap Back", pickup.transform, new Vector3(0f, -0.16f, -0.33f), new Vector3(1.02f, 0.08f, 0.07f), ironMaterial);

        for (int i = 0; i < 3; i++)
        {
            float x = (i - 1) * 0.28f;
            GameObject cartridge = CreateLocalPrimitive(name + " Pressure Cartridge " + i, PrimitiveType.Cylinder, pickup.transform, new Vector3(x, 0f, 0f), new Vector3(0.11f, 0.45f, 0.11f), cartridgeMaterial);
            cartridge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            GameObject nozzle = CreateLocalPrimitive(name + " Iron Nozzle " + i, PrimitiveType.Cylinder, pickup.transform, new Vector3(x, 0f, 0.47f), new Vector3(0.085f, 0.08f, 0.085f), ironMaterial);
            nozzle.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            GameObject cap = CreateLocalPrimitive(name + " Brass Valve Cap " + i, PrimitiveType.Cylinder, pickup.transform, new Vector3(x, 0f, -0.47f), new Vector3(0.13f, 0.08f, 0.13f), brassMaterial);
            cap.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        CreateLocalPrimitive(name + " Pressure Gauge", PrimitiveType.Cylinder, pickup.transform, new Vector3(0f, 0.28f, 0.02f), new Vector3(0.18f, 0.035f, 0.18f), brassMaterial);
        CreateLocalCube(name + " Gauge Needle", pickup.transform, new Vector3(0.04f, 0.3f, 0.02f), new Vector3(0.14f, 0.025f, 0.025f), ironMaterial);
    }

    private static void CreateSteamScattergunPickup(string name, Vector3 position, Material gripMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial, PickupDefinition definition)
    {
        GameObject pickup = new GameObject(name);
        pickup.transform.position = position;

        BoxCollider trigger = pickup.AddComponent<BoxCollider>();
        trigger.size = new Vector3(1.65f, 1.1f, 1.15f);
        trigger.isTrigger = true;

        Pickup pickupComponent = pickup.AddComponent<Pickup>();
        ConfigurePickup(pickupComponent, definition, PickupKind.Weapon, 0);
        pickupComponent.collectRadius = 1.15f;
        pickupComponent.spinDegreesPerSecond = 44f;
        pickupComponent.bobAmplitude = 0.08f;
        pickupComponent.bobSpeed = 2.3f;
        EditorUtility.SetDirty(pickupComponent);

        GameObject visualRoot = new GameObject(name + " Weapon Visual");
        visualRoot.transform.SetParent(pickup.transform, false);
        visualRoot.transform.localPosition = Vector3.zero;
        visualRoot.transform.localRotation = Quaternion.Euler(0f, 24f, 0f);

        CreateLocalCube(name + " Brass Display Stand", visualRoot.transform, new Vector3(0f, -0.58f, 0.04f), new Vector3(0.92f, 0.08f, 1.08f), brassMaterial);
        CreateLocalCube(name + " Iron Display Yoke Left", visualRoot.transform, new Vector3(-0.38f, -0.36f, 0.08f), new Vector3(0.08f, 0.42f, 0.08f), ironMaterial);
        CreateLocalCube(name + " Iron Display Yoke Right", visualRoot.transform, new Vector3(0.38f, -0.36f, 0.08f), new Vector3(0.08f, 0.42f, 0.08f), ironMaterial);
        CreateLocalCube(name + " Enamel Name Plate", visualRoot.transform, new Vector3(0f, -0.51f, -0.52f), new Vector3(0.56f, 0.05f, 0.08f), warningMaterial);
        CreateLocalCube(name + " Walnut Stock", visualRoot.transform, new Vector3(0f, -0.18f, -0.38f), new Vector3(0.28f, 0.26f, 0.58f), gripMaterial);
        CreateLocalCube(name + " Brass Receiver", visualRoot.transform, new Vector3(0f, -0.03f, 0f), new Vector3(0.54f, 0.34f, 0.42f), brassMaterial);
        CreateLocalCube(name + " Iron Trigger Guard", visualRoot.transform, new Vector3(0f, -0.28f, -0.06f), new Vector3(0.32f, 0.08f, 0.24f), ironMaterial);
        CreateLocalCube(name + " Brass Top Rib", visualRoot.transform, new Vector3(0f, 0.24f, 0.22f), new Vector3(0.5f, 0.07f, 0.9f), brassMaterial);
        CreateLocalCube(name + " Walnut Pump Grip", visualRoot.transform, new Vector3(0f, -0.16f, 0.36f), new Vector3(0.5f, 0.09f, 0.22f), gripMaterial);

        for (int i = 0; i < 3; i++)
        {
            float x = (i - 1) * 0.18f;
            GameObject barrel = CreateLocalPrimitive(name + " Pressure Barrel " + i, PrimitiveType.Cylinder, visualRoot.transform, new Vector3(x, 0.08f, 0.42f), new Vector3(0.07f, 0.62f, 0.07f), ironMaterial);
            barrel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            GameObject muzzle = CreateLocalPrimitive(name + " Brass Muzzle Ring " + i, PrimitiveType.Cylinder, visualRoot.transform, new Vector3(x, 0.08f, 0.74f), new Vector3(0.095f, 0.045f, 0.095f), brassMaterial);
            muzzle.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        GameObject pressureDrum = CreateLocalPrimitive(name + " Side Pressure Drum", PrimitiveType.Cylinder, visualRoot.transform, new Vector3(-0.38f, -0.02f, 0.04f), new Vector3(0.18f, 0.28f, 0.18f), ironMaterial);
        pressureDrum.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        GameObject pressureCoil = CreateLocalPrimitive(name + " Brass Pressure Coil", PrimitiveType.Cylinder, visualRoot.transform, new Vector3(-0.5f, -0.02f, 0.04f), new Vector3(0.22f, 0.026f, 0.22f), brassMaterial);
        pressureCoil.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        GameObject valveWheel = CreateLocalPrimitive(name + " Rear Valve Wheel", PrimitiveType.Cylinder, visualRoot.transform, new Vector3(0f, 0.04f, -0.72f), new Vector3(0.22f, 0.035f, 0.22f), brassMaterial);
        valveWheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube(name + " Valve Wheel Crossbar A", visualRoot.transform, new Vector3(0f, 0.04f, -0.75f), new Vector3(0.42f, 0.028f, 0.028f), brassMaterial);
        CreateLocalCube(name + " Valve Wheel Crossbar B", visualRoot.transform, new Vector3(0f, 0.04f, -0.75f), new Vector3(0.028f, 0.42f, 0.028f), brassMaterial);
        for (int i = 0; i < 4; i++)
        {
            float z = -0.28f + i * 0.16f;
            CreateLocalCube(name + " Brass Shell Rack Round " + i, visualRoot.transform, new Vector3(0.48f, -0.17f, z), new Vector3(0.07f, 0.07f, 0.12f), brassMaterial);
        }

        CreateLocalCube(name + " Red Pressure Line", visualRoot.transform, new Vector3(0.36f, -0.02f, 0.16f), new Vector3(0.045f, 0.06f, 0.72f), warningMaterial);
        CreateLocalPrimitive(name + " Brass Gauge", PrimitiveType.Cylinder, visualRoot.transform, new Vector3(0.32f, 0.23f, -0.1f), new Vector3(0.13f, 0.035f, 0.13f), brassMaterial).transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private static void CreateSteamScattergunPickupReadabilityCues(Material brassMaterial, Material warningMaterial, Material ironMaterial, Material gaugeFaceMaterial)
    {
        GameObject cueRoot = new GameObject("Steam Scattergun Pickup Readability Cues");

        CreateCube("Steam Scattergun Route Brass Floor Strip A", new Vector3(0f, 0.018f, 11.65f), new Vector3(0.36f, 0.035f, 1.45f), brassMaterial, cueRoot.transform);
        CreateCube("Steam Scattergun Route Brass Floor Strip B", new Vector3(0f, 0.019f, 12.85f), new Vector3(0.5f, 0.035f, 0.74f), brassMaterial, cueRoot.transform);
        CreateCube("Steam Scattergun Pickup Arrow Plate", new Vector3(0f, 0.022f, 13.35f), new Vector3(1.35f, 0.04f, 0.18f), warningMaterial, cueRoot.transform);
        CreateCube("Steam Scattergun Pickup Chevron Left", new Vector3(-0.48f, 0.026f, 13.72f), new Vector3(0.56f, 0.04f, 0.14f), warningMaterial, cueRoot.transform).transform.rotation = Quaternion.Euler(0f, 34f, 0f);
        CreateCube("Steam Scattergun Pickup Chevron Right", new Vector3(0.48f, 0.026f, 13.72f), new Vector3(0.56f, 0.04f, 0.14f), warningMaterial, cueRoot.transform).transform.rotation = Quaternion.Euler(0f, -34f, 0f);

        CreateCube("Steam Scattergun Pickup Sign Backplate", new Vector3(0f, 2.05f, 12.88f), new Vector3(2.2f, 0.56f, 0.08f), ironMaterial, cueRoot.transform);
        CreateCube("Steam Scattergun Pickup Sign Brass Header", new Vector3(0f, 2.31f, 12.82f), new Vector3(2.05f, 0.08f, 0.08f), brassMaterial, cueRoot.transform);
        CreateCube("Steam Scattergun Pickup Sign Warning Underline", new Vector3(0f, 1.8f, 12.82f), new Vector3(1.75f, 0.06f, 0.08f), warningMaterial, cueRoot.transform);
        CreateCube("Steam Scattergun Pickup Sign Left Bracket", new Vector3(-1.16f, 2.05f, 12.86f), new Vector3(0.08f, 0.66f, 0.08f), brassMaterial, cueRoot.transform);
        CreateCube("Steam Scattergun Pickup Sign Right Bracket", new Vector3(1.16f, 2.05f, 12.86f), new Vector3(0.08f, 0.66f, 0.08f), brassMaterial, cueRoot.transform);

        CreateCube("Steam Scattergun Pickup Pressure Feed Pipe", new Vector3(-1.72f, 1.85f, 13.9f), new Vector3(0.12f, 0.12f, 2.25f), brassMaterial, cueRoot.transform);
        CreateCube("Steam Scattergun Pickup Red Safety Pipe", new Vector3(1.72f, 1.55f, 13.9f), new Vector3(0.1f, 0.1f, 2.05f), warningMaterial, cueRoot.transform);
        CreateLocalPrimitive("Steam Scattergun Pickup Lamp Left", PrimitiveType.Sphere, cueRoot.transform, new Vector3(-0.9f, 1.68f, 13.1f), new Vector3(0.18f, 0.18f, 0.18f), gaugeFaceMaterial);
        CreateLocalPrimitive("Steam Scattergun Pickup Lamp Right", PrimitiveType.Sphere, cueRoot.transform, new Vector3(0.9f, 1.68f, 13.1f), new Vector3(0.18f, 0.18f, 0.18f), gaugeFaceMaterial);

        CreateWorldLabel("Label - Steam Scattergun Pickup", "BREACH TOOL", new Vector3(0f, 2.07f, 12.72f), new Color(1f, 0.78f, 0.24f), 0.19f);
        CreatePointLight("Steam Scattergun Pickup Amber Lamp", new Vector3(0f, 2.35f, 13.2f), new Color(1f, 0.58f, 0.16f), 2.4f, 5.2f);
    }

    private static void ConfigurePickup(Pickup pickup, PickupDefinition definition, PickupKind fallbackKind, int fallbackAmount)
    {
        pickup.definition = definition;
        pickup.kind = definition != null ? definition.kind : fallbackKind;
        pickup.amount = definition != null ? definition.amount : fallbackAmount;
        if (definition == null)
        {
            return;
        }

        pickup.collectRadius = definition.collectRadius;
        pickup.spinDegreesPerSecond = definition.spinDegreesPerSecond;
        pickup.bobAmplitude = definition.bobAmplitude;
        pickup.bobSpeed = definition.bobSpeed;
        EditorUtility.SetDirty(pickup);
    }

    private static void CreateLockedDoor(Material material, Material brassMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material gaugeGlassMaterial, Material warningMaterial)
    {
        GameObject frameRoot = new GameObject("Pressure Gate Frame Assembly");
        CreateDecoCube("Pressure Gate Frame Left Post", new Vector3(-1.88f, 1.67f, 22.45f), new Vector3(0.28f, 3.35f, 0.72f), ironMaterial, frameRoot.transform);
        CreateDecoCube("Pressure Gate Frame Right Post", new Vector3(1.88f, 1.67f, 22.45f), new Vector3(0.28f, 3.35f, 0.72f), ironMaterial, frameRoot.transform);
        CreateDecoCube("Pressure Gate Frame Header", new Vector3(0f, 3.38f, 22.42f), new Vector3(3.95f, 0.32f, 0.72f), ironMaterial, frameRoot.transform);
        CreateDecoCube("Pressure Gate Brass Floor Track", new Vector3(0f, 0.12f, 22.17f), new Vector3(3.82f, 0.08f, 0.18f), brassMaterial, frameRoot.transform);

        GameObject headerGearAssembly = CreateLocalEmpty("Pressure Gate Header Gear Assembly", frameRoot.transform, new Vector3(0f, 3.58f, 22.04f), Quaternion.Euler(90f, 0f, 0f));
        AddSpinner(headerGearAssembly, -16f);
        CreateLocalPrimitive("Pressure Gate Header Gear", PrimitiveType.Cylinder, headerGearAssembly.transform, Vector3.zero, new Vector3(0.34f, 0.045f, 0.34f), brassMaterial);
        for (int i = 0; i < 10; i++)
        {
            float angle = i * 36f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.38f, -0.02f, -Mathf.Cos(radians) * 0.38f);
            GameObject tooth = CreateLocalCube("Pressure Gate Header Gear Tooth " + i, headerGearAssembly.transform, toothPosition, new Vector3(0.08f, 0.07f, 0.14f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, -angle, 0f);
        }

        CreateLocalPrimitive("Pressure Gate Warning Lamp Left", PrimitiveType.Sphere, frameRoot.transform, new Vector3(-1.38f, 3.08f, 22.02f), new Vector3(0.13f, 0.13f, 0.13f), warningMaterial);
        CreateLocalPrimitive("Pressure Gate Warning Lamp Right", PrimitiveType.Sphere, frameRoot.transform, new Vector3(1.38f, 3.08f, 22.02f), new Vector3(0.13f, 0.13f, 0.13f), warningMaterial);

        GameObject door = CreateCube("Pressure Gate", new Vector3(0f, 1.5f, 22.5f), new Vector3(3f, 3f, 0.5f), material);
        LockedDoor lockedDoor = door.AddComponent<LockedDoor>();
        lockedDoor.openDistance = 2.3f;

        CreateLocalCube("Pressure Gate Left Riveted Slab", door.transform, new Vector3(-0.55f, 0f, -0.29f), new Vector3(0.82f, 1.76f, 0.07f), ironMaterial);
        CreateLocalCube("Pressure Gate Right Riveted Slab", door.transform, new Vector3(0.55f, 0f, -0.29f), new Vector3(0.82f, 1.76f, 0.07f), ironMaterial);
        CreateLocalCube("Pressure Gate Upper Brass Rail", door.transform, new Vector3(0f, 0.96f, -0.34f), new Vector3(1.34f, 0.08f, 0.09f), brassMaterial);
        CreateLocalCube("Pressure Gate Lower Brass Rail", door.transform, new Vector3(0f, -0.96f, -0.34f), new Vector3(1.34f, 0.08f, 0.09f), brassMaterial);
        CreateLocalCube("Pressure Gate Left Brace", door.transform, new Vector3(-0.45f, 0f, -0.36f), new Vector3(0.08f, 1.82f, 0.09f), brassMaterial);
        CreateLocalCube("Pressure Gate Right Brace", door.transform, new Vector3(0.45f, 0f, -0.36f), new Vector3(0.08f, 1.82f, 0.09f), brassMaterial);
        CreateLocalCube("Pressure Gate Center Lock Housing", door.transform, new Vector3(0f, 0.02f, -0.4f), new Vector3(0.78f, 0.66f, 0.12f), brassMaterial);

        GameObject socket = CreateLocalPrimitive("Pressure Gate Key Socket", PrimitiveType.Cylinder, door.transform, new Vector3(0f, 0.02f, -0.48f), new Vector3(0.31f, 0.04f, 0.31f), ironMaterial);
        socket.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Pressure Gate Socket Vertical Slot", door.transform, new Vector3(0f, -0.08f, -0.53f), new Vector3(0.07f, 0.48f, 0.035f), warningMaterial);
        CreateLocalCube("Pressure Gate Socket Bit Slot", door.transform, new Vector3(0.14f, -0.28f, -0.535f), new Vector3(0.31f, 0.07f, 0.035f), warningMaterial);

        GameObject gearAssembly = CreateLocalEmpty("Pressure Gate Gear Wheel Assembly", door.transform, new Vector3(0f, 0.04f, -0.55f), Quaternion.Euler(90f, 0f, 0f));
        AddSpinner(gearAssembly, 28f);
        CreateLocalPrimitive("Pressure Gate Gear Wheel", PrimitiveType.Cylinder, gearAssembly.transform, Vector3.zero, new Vector3(0.43f, 0.045f, 0.43f), brassMaterial);
        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.45f, -0.01f, -Mathf.Cos(radians) * 0.45f);
            GameObject tooth = CreateLocalCube("Pressure Gate Gear Tooth " + i, gearAssembly.transform, toothPosition, new Vector3(0.09f, 0.07f, 0.15f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, -angle, 0f);
        }

        CreatePressureGaugePrototype("Pressure Gate Prototype Gauge", door.transform, new Vector3(-0.58f, 0.62f, -0.47f), 0.46f, brassMaterial, ironMaterial, gaugeFaceMaterial, gaugeGlassMaterial, warningMaterial, "pressure_gate_panel");

        CreateLocalPrimitive("Pressure Gate Left Pressure Cylinder", PrimitiveType.Cylinder, door.transform, new Vector3(-1.04f, 0f, -0.4f), new Vector3(0.08f, 0.88f, 0.08f), brassMaterial);
        CreateLocalPrimitive("Pressure Gate Right Pressure Cylinder", PrimitiveType.Cylinder, door.transform, new Vector3(1.04f, 0f, -0.4f), new Vector3(0.08f, 0.88f, 0.08f), brassMaterial);
        CreateLocalCube("Pressure Gate Red Pressure Pipe", door.transform, new Vector3(0.68f, 0.64f, -0.48f), new Vector3(0.5f, 0.055f, 0.055f), warningMaterial);

        for (int row = 0; row < 4; row++)
        {
            float y = -0.72f + row * 0.48f;
            CreateLocalPrimitive("Pressure Gate Left Rivet " + row, PrimitiveType.Sphere, door.transform, new Vector3(-0.92f, y, -0.47f), new Vector3(0.06f, 0.06f, 0.06f), brassMaterial);
            CreateLocalPrimitive("Pressure Gate Right Rivet " + row, PrimitiveType.Sphere, door.transform, new Vector3(0.92f, y, -0.47f), new Vector3(0.06f, 0.06f, 0.06f), brassMaterial);
        }
    }

    private static void CreateExit(Material material, Material ironMaterial, Material brassMaterial, Material gaugeFaceMaterial)
    {
        GameObject exit = CreateServiceLiftShell("Service Lift Trigger", new Vector3(0f, 1.1f, 34.6f), material, ironMaterial, brassMaterial, gaugeFaceMaterial);
        exit.AddComponent<ExitTrigger>();
    }

    private static GameObject CreateExitAt(string name, Vector3 position, Material material, Material ironMaterial, Material brassMaterial, Material gaugeFaceMaterial)
    {
        GameObject exit = CreateServiceLiftShell(name, position, material, ironMaterial, brassMaterial, gaugeFaceMaterial);
        exit.AddComponent<ExitTrigger>();
        return exit;
    }

    private static void CreateLevelTransitionLift(Material material, Material ironMaterial, Material brassMaterial, Material gaugeFaceMaterial, string targetSceneName)
    {
        CreateLevelTransitionLiftAt("Service Lift To Pipeworks", new Vector3(0f, 1.1f, 34.6f), material, ironMaterial, brassMaterial, gaugeFaceMaterial, targetSceneName, "Service lift descending to the Pipeworks Annex");
    }

    private static GameObject CreateLevelTransitionLiftAt(string name, Vector3 position, Material material, Material ironMaterial, Material brassMaterial, Material gaugeFaceMaterial, string targetSceneName, string transitionMessage)
    {
        GameObject lift = CreateServiceLiftShell(name, position, material, ironMaterial, brassMaterial, gaugeFaceMaterial);
        LevelTransitionTrigger transition = lift.AddComponent<LevelTransitionTrigger>();
        transition.targetSceneName = targetSceneName;
        transition.transitionMessage = transitionMessage;
        return lift;
    }

    private static GameObject CreateServiceLiftShell(string name, Vector3 position, Material material, Material ironMaterial, Material brassMaterial, Material gaugeFaceMaterial)
    {
        GameObject lift = CreateCube(name, position, new Vector3(2.4f, 2.2f, 0.35f), material);
        Collider liftCollider = lift.GetComponent<Collider>();
        if (liftCollider != null)
        {
            liftCollider.isTrigger = true;
        }

        CreateLocalCube(name + " Brass Platform Deck", lift.transform, new Vector3(0f, -0.64f, -0.34f), new Vector3(1.45f, 0.1f, 0.86f), brassMaterial);
        CreateLocalCube(name + " Iron Platform Grate", lift.transform, new Vector3(0f, -0.55f, -0.42f), new Vector3(1.22f, 0.05f, 0.52f), ironMaterial);
        CreateLocalCube(name + " Brass Threshold", lift.transform, new Vector3(0f, -0.47f, -0.78f), new Vector3(1.55f, 0.08f, 0.12f), brassMaterial);

        CreateLocalCube(name + " Cage Top", lift.transform, new Vector3(0f, 0.64f, -0.58f), new Vector3(1.45f, 0.08f, 0.08f), ironMaterial);
        CreateLocalCube(name + " Cage Bottom", lift.transform, new Vector3(0f, -0.34f, -0.58f), new Vector3(1.45f, 0.08f, 0.08f), ironMaterial);
        CreateLocalCube(name + " Left Rear Rail", lift.transform, new Vector3(-0.62f, 0.08f, -0.3f), new Vector3(0.06f, 1.38f, 0.08f), brassMaterial);
        CreateLocalCube(name + " Right Rear Rail", lift.transform, new Vector3(0.62f, 0.08f, -0.3f), new Vector3(0.06f, 1.38f, 0.08f), brassMaterial);
        CreateLocalCube(name + " Left Front Rail", lift.transform, new Vector3(-0.62f, 0.08f, -0.75f), new Vector3(0.06f, 1.38f, 0.08f), brassMaterial);
        CreateLocalCube(name + " Right Front Rail", lift.transform, new Vector3(0.62f, 0.08f, -0.75f), new Vector3(0.06f, 1.38f, 0.08f), brassMaterial);

        GameObject leftBrace = CreateLocalCube(name + " Left Cross Brace", lift.transform, new Vector3(-0.62f, 0.12f, -0.78f), new Vector3(0.06f, 1.18f, 0.06f), ironMaterial);
        leftBrace.transform.localRotation = Quaternion.Euler(0f, 0f, -24f);
        GameObject rightBrace = CreateLocalCube(name + " Right Cross Brace", lift.transform, new Vector3(0.62f, 0.12f, -0.78f), new Vector3(0.06f, 1.18f, 0.06f), ironMaterial);
        rightBrace.transform.localRotation = Quaternion.Euler(0f, 0f, 24f);

        CreateLocalCube(name + " Left Lift Chain", lift.transform, new Vector3(-0.34f, 0.25f, -0.72f), new Vector3(0.045f, 1.24f, 0.045f), ironMaterial);
        CreateLocalCube(name + " Right Lift Chain", lift.transform, new Vector3(0.34f, 0.25f, -0.72f), new Vector3(0.045f, 1.24f, 0.045f), ironMaterial);

        GameObject pulleyAssembly = CreateLocalEmpty(name + " Overhead Pulley Gear Assembly", lift.transform, new Vector3(0f, 0.9f, -0.72f), Quaternion.Euler(90f, 0f, 0f));
        AddSpinner(pulleyAssembly, 24f);
        CreateLocalPrimitive(name + " Overhead Pulley Gear", PrimitiveType.Cylinder, pulleyAssembly.transform, Vector3.zero, new Vector3(0.3f, 0.045f, 0.3f), brassMaterial);
        for (int i = 0; i < 10; i++)
        {
            float angle = i * 36f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.34f, -0.02f, -Mathf.Cos(radians) * 0.34f);
            GameObject tooth = CreateLocalCube(name + " Pulley Tooth " + i, pulleyAssembly.transform, toothPosition, new Vector3(0.07f, 0.06f, 0.12f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, -angle, 0f);
        }

        CreateLocalCube(name + " Brass Call Box", lift.transform, new Vector3(0.78f, 0.08f, -0.68f), new Vector3(0.2f, 0.56f, 0.1f), brassMaterial);
        GameObject callGauge = CreateLocalPrimitive(name + " Call Box Gauge", PrimitiveType.Cylinder, lift.transform, new Vector3(0.78f, 0.2f, -0.76f), new Vector3(0.105f, 0.02f, 0.105f), gaugeFaceMaterial);
        callGauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube(name + " Call Lever", lift.transform, new Vector3(0.78f, -0.12f, -0.78f), new Vector3(0.045f, 0.26f, 0.045f), ironMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, -22f);

        CreateLocalPrimitive(name + " Green Signal Lamp Left", PrimitiveType.Sphere, lift.transform, new Vector3(-0.42f, 0.72f, -0.75f), new Vector3(0.11f, 0.11f, 0.11f), material);
        CreateLocalPrimitive(name + " Green Signal Lamp Right", PrimitiveType.Sphere, lift.transform, new Vector3(0.42f, 0.72f, -0.75f), new Vector3(0.11f, 0.11f, 0.11f), material);

        GameObject liftGauge = CreateLocalPrimitive(name + " Pressure Gauge", PrimitiveType.Cylinder, lift.transform, new Vector3(0f, 0.25f, -0.79f), new Vector3(0.18f, 0.025f, 0.18f), gaugeFaceMaterial);
        liftGauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube(name + " Pressure Gauge Needle", lift.transform, new Vector3(0.05f, 0.25f, -0.83f), new Vector3(0.14f, 0.014f, 0.014f), ironMaterial);

        return lift;
    }

    private static SteamworksSpinner AddSpinner(GameObject target, float degreesPerSecond)
    {
        SteamworksSpinner spinner = target.AddComponent<SteamworksSpinner>();
        spinner.localAxis = Vector3.up;
        spinner.degreesPerSecond = degreesPerSecond;
        return spinner;
    }

    private static T RequireObject<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            throw new InvalidOperationException($"Smoke test failed: missing {label}.");
        }

        return value;
    }
}
