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
    private const string MaterialFolder = "Assets/_Project/Materials";
    private const string TextureFolder = "Assets/_Project/Textures";
    private const string DataFolder = "Assets/_Project/Data";
    private const string WindowsBuildFolder = "Builds/Windows";

    private enum ProceduralTextureKind
    {
        OilStone,
        RivetedIron,
        BrassPipe
    }

    [MenuItem("Project Tools/Rebuild v0.0 Scene")]
    public static void BuildV0()
    {
        EnsureFolders();

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
        ApplyProceduralTexture(oilStoneMaterial, "T_Steam_OilDarkStone", ProceduralTextureKind.OilStone);
        ApplyProceduralTexture(rivetedIronMaterial, "T_Steam_RivetedIron", ProceduralTextureKind.RivetedIron);
        ApplyProceduralTexture(brassGuideMaterial, "T_Steam_BrassPipe", ProceduralTextureKind.BrassPipe);
        ApplyProceduralTexture(brassHazardMaterial, "T_Steam_BrassHazardPipe", ProceduralTextureKind.BrassPipe);
        WeaponDefinition pressurePistolDefinition = CreatePressurePistolDefinition();
        EnemyDefinition scrapperDefinition = CreateScrapperDefinition();
        EnemyDefinition lancerDefinition = CreateLancerDefinition();
        PickupDefinition healthPickupDefinition = CreateHealthPickupDefinition();
        PickupDefinition ammoPickupDefinition = CreateAmmoPickupDefinition();
        PickupDefinition gearKeyDefinition = CreateGearKeyDefinition();
        PlatformQualityProfile windowsQualityProfile = CreatePlatformQualityProfiles();

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.55f);

        CreateLighting();
        CreateGreyboxLevel(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud, windowsQualityProfile, "Find the gear key. Open the pressure gate.");
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, rivetedIronMaterial, pressureWarningMaterial, pressurePistolDefinition);
        CreateEnemy("Enemy - First Room", new Vector3(0f, 1f, 16.5f), enemyMaterial, enemyEyeMaterial, brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, scrapperDefinition);
        CreateEnemy("Enemy - Key Room", new Vector3(14.5f, 1f, 17f), enemyMaterial, enemyEyeMaterial, brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, scrapperDefinition);
        CreateEnemy("Enemy - Final Left", new Vector3(-3.2f, 1f, 30.5f), enemyMaterial, enemyEyeMaterial, brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, scrapperDefinition);
        CreateEnemy("Enemy - Final Right", new Vector3(3.2f, 1f, 32.5f), enemyMaterial, enemyEyeMaterial, brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, scrapperDefinition);
        CreateHealthVialPickup("Pickup - Health Vial", new Vector3(-3.6f, 0.65f, 20f), healthMaterial, glassVialMaterial, medicinalFluidMaterial, brassGuideMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Pressure Cartridge Pack", new Vector3(4.2f, 0.55f, 19f), ammoMaterial, rivetedIronMaterial, brassGuideMaterial, ammoPickupDefinition);
        CreateGearKeyPickup("Pickup - Gear Key", new Vector3(16f, 0.55f, 17f), Vector3.one * 1.1f, keyMaterial, rivetedIronMaterial, gearKeyDefinition);
        CreateLockedDoor(doorMaterial, brassGuideMaterial, rivetedIronMaterial, gaugeFaceMaterial, pressureWarningMaterial);
        CreateLevelTransitionLift(exitMaterial, rivetedIronMaterial, brassGuideMaterial, gaugeFaceMaterial, "Level02");
        CreateAccentLights();
        CreateObjectiveGuides(brassGuideMaterial, pressureWarningMaterial, keyMaterial, exitMaterial);
        CreateSteamworksDressing(rivetedIronMaterial, oilStoneMaterial, brassGuideMaterial, pressureWarningMaterial, brassHazardMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);
        CreateSecretCache(brassGuideMaterial, rivetedIronMaterial, pressureWarningMaterial, healthMaterial, glassVialMaterial, medicinalFluidMaterial, ammoMaterial, healthPickupDefinition, ammoPickupDefinition);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ScenePath);
        CreatePipeworksAnnexScene(wallMaterial, floorMaterial, exitMaterial, enemyMaterial, enemyEyeMaterial, healthMaterial, ammoMaterial, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, brassGuideMaterial, pressureWarningMaterial, rivetedIronMaterial, oilStoneMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial, glassVialMaterial, medicinalFluidMaterial, pressurePistolDefinition, scrapperDefinition, lancerDefinition, healthPickupDefinition, ammoPickupDefinition, windowsQualityProfile);
        CreateBoilerheartScene(wallMaterial, floorMaterial, exitMaterial, enemyMaterial, enemyEyeMaterial, healthMaterial, ammoMaterial, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, brassGuideMaterial, pressureWarningMaterial, rivetedIronMaterial, oilStoneMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial, glassVialMaterial, medicinalFluidMaterial, pressurePistolDefinition, scrapperDefinition, healthPickupDefinition, ammoPickupDefinition, windowsQualityProfile);
        CreateMainMenuScene(brassGuideMaterial, rivetedIronMaterial, gaugeFaceMaterial, furnaceGlowMaterial, oilStoneMaterial, windowsQualityProfile);
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(MainMenuScenePath, true),
            new EditorBuildSettingsScene(ScenePath, true),
            new EditorBuildSettingsScene(Level02ScenePath, true),
            new EditorBuildSettingsScene(Level03ScenePath, true)
        };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("V0 scenes rebuilt at " + MainMenuScenePath + ", " + ScenePath + ", " + Level02ScenePath + ", and " + Level03ScenePath);
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
        RequireObject<RuntimeHazardTest>("RuntimeHazardTest");
        RequireObject<RuntimeSecretTest>("RuntimeSecretTest");
        RequireObject<RuntimePauseFlowTest>("RuntimePauseFlowTest");
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
        RequireObject<ExitTrigger>("Level03 ExitTrigger");
        RequireObject<SteamHazard>("Level03 SteamHazard");

        EditorSceneManager.OpenScene(MainMenuScenePath);
        RequireObject<MainMenuController>("MainMenuController");
        RequireObject<RuntimePerformanceProfile>("MainMenu RuntimePerformanceProfile");

        V0LevelValidator.ValidateProjectScenes();

        if (EditorBuildSettings.scenes.Length < 4 || EditorBuildSettings.scenes[0].path != MainMenuScenePath || EditorBuildSettings.scenes[1].path != ScenePath || EditorBuildSettings.scenes[2].path != Level02ScenePath || EditorBuildSettings.scenes[3].path != Level03ScenePath)
        {
            throw new InvalidOperationException("MainMenu, Level01, Level02, and Level03 are not the first enabled build scenes.");
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
            scenes = new[] { MainMenuScenePath, ScenePath, Level02ScenePath, Level03ScenePath },
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
        definition.damage = GameBalance.PressurePistolDamage;
        definition.fireCooldown = GameBalance.PressurePistolCooldown;
        definition.range = 40f;
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
        definition.attackRange = 1.35f;
        definition.attackDamage = GameBalance.ScrapperAttackDamage;
        definition.attackCooldown = 1f;
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
        definition.amount = 25;
        definition.collectRadius = 0.9f;
        definition.spinDegreesPerSecond = 82f;
        definition.bobAmplitude = 0.1f;
        definition.bobSpeed = 3f;
        definition.audioCue = SteamworksAudioCue.HealthPickup;
        definition.collectMessage = "+25 health";
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
        definition.amount = 15;
        definition.collectRadius = 0.9f;
        definition.spinDegreesPerSecond = 74f;
        definition.bobAmplitude = 0.1f;
        definition.bobSpeed = 2.8f;
        definition.audioCue = SteamworksAudioCue.AmmoPickup;
        definition.collectMessage = "+15 ammo";
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

    private static void CreatePipeworksAnnexScene(Material wallMaterial, Material floorMaterial, Material exitMaterial, Material enemyMaterial, Material enemyEyeMaterial, Material healthMaterial, Material ammoMaterial, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material brassMaterial, Material warningMaterial, Material ironMaterial, Material oilStoneMaterial, Material gaugeFaceMaterial, Material steamPuffMaterial, Material furnaceGlowMaterial, Material glassMaterial, Material fluidMaterial, WeaponDefinition pressurePistolDefinition, EnemyDefinition scrapperDefinition, EnemyDefinition lancerDefinition, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition, PlatformQualityProfile windowsQualityProfile)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.44f, 0.42f, 0.38f);

        CreateLighting();
        CreatePipeworksAnnexBlockout(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud, windowsQualityProfile, "Survive the Pipeworks. Ride the lift to the Boilerheart.");
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, ironMaterial, warningMaterial, pressurePistolDefinition);

        CreateEnemy("Enemy - Pipeworks Gatehouse", new Vector3(-2.2f, 1f, 9.5f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateLancerEnemy("Enemy - Pipeworks Lancer", new Vector3(2.2f, 1f, 17.5f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, lancerDefinition);
        CreateHealthVialPickup("Pickup - Annex Health Vial", new Vector3(-3.2f, 0.65f, 14f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Annex Pressure Cartridge Pack", new Vector3(3.2f, 0.55f, 13.5f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        CreateLevelTransitionLiftAt("Pipeworks Service Lift To Boilerheart", new Vector3(0f, 1.1f, 23.2f), exitMaterial, ironMaterial, brassMaterial, gaugeFaceMaterial, "Level03", "Service lift grinding toward the Boilerheart");
        CreatePipeworksDressing(ironMaterial, oilStoneMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);
        CreatePointLight("Pipeworks Exit Green Light", new Vector3(0f, 2.4f, 22.7f), new Color(0.1f, 1f, 0.3f), 2.8f, 7f);
        CreatePointLight("Pipeworks Furnace Light", new Vector3(-4.1f, 1.6f, 16f), new Color(1f, 0.36f, 0.08f), 2.2f, 5f);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), Level02ScenePath);
    }

    private static void CreateBoilerheartScene(Material wallMaterial, Material floorMaterial, Material exitMaterial, Material enemyMaterial, Material enemyEyeMaterial, Material healthMaterial, Material ammoMaterial, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material brassMaterial, Material warningMaterial, Material ironMaterial, Material oilStoneMaterial, Material gaugeFaceMaterial, Material steamPuffMaterial, Material furnaceGlowMaterial, Material glassMaterial, Material fluidMaterial, WeaponDefinition pressurePistolDefinition, EnemyDefinition scrapperDefinition, PickupDefinition healthPickupDefinition, PickupDefinition ammoPickupDefinition, PlatformQualityProfile windowsQualityProfile)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.38f, 0.32f, 0.26f);

        CreateLighting();
        CreateBoilerheartBlockout(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud, windowsQualityProfile, "Vent the Boilerheart pressure valve. Reach the final service lift.");
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, ironMaterial, warningMaterial, pressurePistolDefinition);

        CreateEnemy("Enemy - Boilerheart Floor Guard", new Vector3(-2.6f, 1f, 12.8f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateEnemy("Enemy - Boilerheart Lift Guard", new Vector3(2.4f, 1f, 19.2f), enemyMaterial, enemyEyeMaterial, brassMaterial, ironMaterial, warningMaterial, scrapperDefinition);
        CreateHealthVialPickup("Pickup - Boilerheart Health Vial", new Vector3(-3.6f, 0.65f, 9.4f), healthMaterial, glassMaterial, fluidMaterial, brassMaterial, healthPickupDefinition);
        CreatePressureCartridgePickup("Pickup - Boilerheart Pressure Cartridge Pack", new Vector3(3.4f, 0.55f, 9.8f), ammoMaterial, ironMaterial, brassMaterial, ammoPickupDefinition);
        SteamHazard[] boilerheartHazards = CreateBoilerheartDressing(ironMaterial, oilStoneMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);
        ExitTrigger finalLift = CreateExitAt("Boilerheart Final Service Lift", new Vector3(0f, 1.1f, 24.3f), exitMaterial, ironMaterial, brassMaterial, gaugeFaceMaterial).GetComponent<ExitTrigger>();
        SteamValveObjective pressureValve = CreateBoilerheartPressureValve(ironMaterial, brassMaterial, warningMaterial, gaugeFaceMaterial, steamPuffMaterial);
        pressureValve.hazardsToDisableOnComplete = boilerheartHazards;
        finalLift.requiredValve = pressureValve;
        CreatePointLight("Boilerheart Furnace Light", new Vector3(0f, 2.6f, 15.8f), new Color(1f, 0.32f, 0.08f), 4f, 10f);
        CreatePointLight("Boilerheart Final Lift Green Light", new Vector3(0f, 2.6f, 23.8f), new Color(0.1f, 1f, 0.35f), 2.8f, 7f);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), Level03ScenePath);
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
        CreatePressureGauge("Boilerheart Gauge A", new Vector3(-5.95f, 1.65f, 12.4f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateValveWheel("Boilerheart Valve A", new Vector3(5.95f, 1.45f, 17.4f), Quaternion.Euler(0f, -90f, 0f), brassMaterial, warningMaterial, parent.transform);
        CreateSteamVent("Boilerheart Steam Vent A", new Vector3(-4.8f, 0.05f, 20.8f), brassMaterial, steamMaterial, parent.transform);
        SteamHazard furnaceLeak = CreateSteamHazard("Boilerheart Steam Hazard - Furnace Leak", new Vector3(-4.8f, 0.75f, 20.8f), new Vector3(1.25f, 1.5f, 1.25f), ironMaterial, steamMaterial, warningMaterial, parent.transform);
        SteamHazard coreBleed = CreateSteamHazard("Boilerheart Steam Hazard - Core Bleed", new Vector3(2.35f, 0.75f, 15.4f), new Vector3(1.15f, 1.5f, 1.15f), ironMaterial, steamMaterial, warningMaterial, parent.transform);
        CreateWorkOrderBoard("Work Order Board - Boilerheart", "BOILERHEART ORDER\nCORE PRESSURE HIGH\nFINAL LIFT LIVE", new Vector3(5.95f, 1.55f, 10.6f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);

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

        CreateLocalCube(name + " Warning Floor Plate", hazardRoot.transform, new Vector3(0f, -0.73f, 0f), new Vector3(triggerSize.x, 0.04f, triggerSize.z), warningMaterial);
        CreateLocalPrimitive(name + " Steam Puff Low", PrimitiveType.Sphere, hazardRoot.transform, new Vector3(-0.12f, -0.15f, 0.04f), new Vector3(0.38f, 0.32f, 0.38f), steamMaterial);
        CreateLocalPrimitive(name + " Steam Puff High", PrimitiveType.Sphere, hazardRoot.transform, new Vector3(0.16f, 0.38f, -0.05f), new Vector3(0.3f, 0.46f, 0.3f), steamMaterial);
        CreateLocalCube(name + " Brass Vent Grate", hazardRoot.transform, new Vector3(0f, -0.68f, 0f), new Vector3(triggerSize.x * 0.68f, 0.08f, triggerSize.z * 0.68f), ironMaterial);

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

        CreateLocalCube("Boilerheart Pressure Valve Backplate", valveRoot.transform, Vector3.zero, new Vector3(1.12f, 1.28f, 0.08f), ironMaterial);
        GameObject wheel = CreateLocalPrimitive("Boilerheart Pressure Valve Wheel", PrimitiveType.Cylinder, valveRoot.transform, new Vector3(0f, 0.08f, -0.12f), new Vector3(0.5f, 0.045f, 0.5f), brassMaterial);
        wheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Boilerheart Pressure Valve Spoke Horizontal", valveRoot.transform, new Vector3(0f, 0.08f, -0.16f), new Vector3(0.92f, 0.045f, 0.045f), warningMaterial);
        CreateLocalCube("Boilerheart Pressure Valve Spoke Vertical", valveRoot.transform, new Vector3(0f, 0.08f, -0.16f), new Vector3(0.045f, 0.92f, 0.045f), warningMaterial);
        CreateLocalPrimitive("Boilerheart Pressure Valve Hub", PrimitiveType.Sphere, valveRoot.transform, new Vector3(0f, 0.08f, -0.2f), new Vector3(0.14f, 0.14f, 0.14f), brassMaterial);

        GameObject gauge = CreateLocalPrimitive("Boilerheart Pressure Valve Gauge", PrimitiveType.Cylinder, valveRoot.transform, new Vector3(0.32f, 0.48f, -0.14f), new Vector3(0.2f, 0.03f, 0.2f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Boilerheart Pressure Valve Gauge Needle", valveRoot.transform, new Vector3(0.37f, 0.48f, -0.18f), new Vector3(0.16f, 0.018f, 0.018f), warningMaterial);
        valve.lockedSignal = CreateLocalPrimitive("Boilerheart Valve Locked Lamp", PrimitiveType.Sphere, valveRoot.transform, new Vector3(-0.34f, 0.48f, -0.16f), new Vector3(0.13f, 0.13f, 0.13f), warningMaterial);
        valve.ventedSignal = CreateLocalPrimitive("Boilerheart Valve Vented Lamp", PrimitiveType.Sphere, valveRoot.transform, new Vector3(-0.34f, 0.48f, -0.17f), new Vector3(0.13f, 0.13f, 0.13f), steamMaterial);
        CreateLocalCube("Boilerheart Pressure Valve Outlet Pipe", valveRoot.transform, new Vector3(0f, -0.52f, -0.12f), new Vector3(0.18f, 0.62f, 0.18f), brassMaterial);

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
        CreatePressureGauge("Pipeworks Gauge A", new Vector3(4.95f, 1.65f, 7f), Quaternion.Euler(0f, -90f, 0f), brassMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
        CreateValveWheel("Pipeworks Valve A", new Vector3(-4.95f, 1.45f, 20f), Quaternion.Euler(0f, 90f, 0f), brassMaterial, warningMaterial, parent.transform);
        CreateSteamVent("Pipeworks Steam Vent A", new Vector3(3.8f, 0.05f, 5.5f), brassMaterial, steamMaterial, parent.transform);
        CreatePipeBundle("Pipeworks Triple Pipe Bundle", new Vector3(0f, 2.35f, 23.72f), Quaternion.Euler(0f, 90f, 0f), 3.6f, brassMaterial, ironMaterial, parent.transform);
        CreateWorkOrderBoard("Work Order Board - Pipeworks", "PIPEWORKS NOTICE\nBOLT FEED LIVE\nKEEP DISTANCE", new Vector3(4.95f, 1.55f, 12f), Quaternion.Euler(0f, -90f, 0f), ironMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
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

        GameObject wheel = CreateLocalPrimitive("Menu Center Gear", PrimitiveType.Cylinder, propRoot.transform, new Vector3(0f, 1.55f, -0.02f), new Vector3(0.85f, 0.07f, 0.85f), brassMaterial);
        wheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        for (int i = 0; i < 10; i++)
        {
            float angle = i * 36f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.88f, 1.55f + Mathf.Cos(radians) * 0.88f, -0.05f);
            GameObject tooth = CreateLocalCube("Menu Gear Tooth " + i, propRoot.transform, toothPosition, new Vector3(0.18f, 0.2f, 0.09f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        GameObject gauge = CreateLocalPrimitive("Menu Pressure Gauge", PrimitiveType.Cylinder, propRoot.transform, new Vector3(-2.35f, 1.65f, -0.12f), new Vector3(0.46f, 0.04f, 0.46f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Menu Gauge Needle", propRoot.transform, new Vector3(-2.24f, 1.65f, -0.16f), new Vector3(0.28f, 0.025f, 0.025f), glowMaterial);

        GameObject canvasObject = new GameObject("Main Menu Canvas");
        RuntimePerformanceProfile performanceProfile = canvasObject.AddComponent<RuntimePerformanceProfile>();
        performanceProfile.activeProfile = qualityProfile;
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
        {
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        CreateAnchoredImage("Menu Soot Vignette", canvasObject.transform, new Color(0.01f, 0.008f, 0.006f, 0.36f), Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, false);
        CreateText("Menu Title", canvasObject.transform, font, GameBranding.WorkingTitle.ToUpperInvariant(), 58, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 150f), new Vector2(860f, 92f));
        Text subtitle = CreateText("Menu Subtitle", canvasObject.transform, font, "PRESSURE BELOW. BRASS ABOVE.", 24, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 94f), new Vector2(620f, 46f));
        subtitle.color = new Color(1f, 0.78f, 0.42f);

        MainMenuController mainMenu = canvasObject.AddComponent<MainMenuController>();
        mainMenu.startButton = CreatePauseButton("Start Button", "START GAME", canvasObject.transform, font, new Vector2(0f, 10f));
        mainMenu.quitButton = CreatePauseButton("Quit Button", "QUIT", canvasObject.transform, font, new Vector2(0f, -58f));
        mainMenu.sensitivitySlider = CreateSettingsSlider("Menu Sensitivity Slider", "MOUSE", canvasObject.transform, font, new Vector2(0f, -136f), 0.6f, 5f, GameSettings.DefaultMouseSensitivity, out Text menuSensitivityValue);
        mainMenu.sensitivityValueText = menuSensitivityValue;
        mainMenu.volumeSlider = CreateSettingsSlider("Menu Volume Slider", "VOLUME", canvasObject.transform, font, new Vector2(0f, -190f), 0f, 1f, GameSettings.DefaultMasterVolume, out Text menuVolumeValue);
        mainMenu.volumeValueText = menuVolumeValue;
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
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        HUDController hud = canvasObject.AddComponent<HUDController>();

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
        {
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        hud.damageFlashImage = CreateScreenImage("Damage Flash", canvasObject.transform, new Color(1f, 0f, 0f, 0f));
        CreateAnchoredImage("Health Gauge Backplate", canvasObject.transform, new Color(0.16f, 0.085f, 0.035f, 0.86f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(14f, 14f), new Vector2(370f, 58f), false);
        hud.healthFillImage = CreateAnchoredImage("Health Gauge Fill", canvasObject.transform, new Color(0.78f, 0.08f, 0.04f, 0.88f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(24f, 20f), new Vector2(210f, 12f), true);
        CreateAnchoredImage("Ammo Gauge Backplate", canvasObject.transform, new Color(0.16f, 0.085f, 0.035f, 0.86f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-14f, 14f), new Vector2(370f, 58f), false);
        hud.ammoFillImage = CreateAnchoredImage("Ammo Gauge Fill", canvasObject.transform, new Color(0.85f, 0.55f, 0.16f, 0.88f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-24f, 20f), new Vector2(210f, 12f), true);
        CreateAnchoredImage("Gear Key Backplate", canvasObject.transform, new Color(0.16f, 0.085f, 0.035f, 0.86f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 14f), new Vector2(320f, 56f), false);
        hud.keyLampImage = CreateAnchoredImage("Gear Key Lamp", canvasObject.transform, new Color(0.95f, 0.55f, 0.08f, 0.95f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(-128f, 30f), new Vector2(24f, 24f), false);
        hud.healthText = CreateText("Health Text", canvasObject.transform, font, "HEALTH 100/100", 24, TextAnchor.LowerLeft, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(18f, 16f), new Vector2(360f, 50f));
        hud.ammoText = CreateText("Ammo Text", canvasObject.transform, font, "AMMO 30", 24, TextAnchor.LowerRight, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-18f, 16f), new Vector2(360f, 50f));
        hud.keyText = CreateText("Gear Key Text", canvasObject.transform, font, "GEAR KEY NO", 22, TextAnchor.LowerCenter, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 18f), new Vector2(300f, 45f));
        CreateText("Crosshair", canvasObject.transform, font, "+", 34, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(80f, 80f));
        hud.interactionText = CreateText("Interaction Prompt Text", canvasObject.transform, font, string.Empty, 24, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -92f), new Vector2(620f, 54f));
        hud.messageText = CreateText("Message Text", canvasObject.transform, font, string.Empty, 34, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 80f), new Vector2(760f, 220f));
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

        CreateText("Pause Title", root.transform, font, GameBranding.WorkingTitle.ToUpperInvariant(), 42, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 150f), new Vector2(620f, 72f));
        CreateText("Pause Subtitle", root.transform, font, "PRESSURE PAUSED", 24, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 100f), new Vector2(420f, 48f));

        pauseMenu.root = root;
        pauseMenu.resumeButton = CreatePauseButton("Resume Button", "RESUME", root.transform, font, new Vector2(0f, 32f));
        pauseMenu.restartButton = CreatePauseButton("Restart Button", "RESTART", root.transform, font, new Vector2(0f, -34f));
        pauseMenu.quitButton = CreatePauseButton("Quit Button", "QUIT", root.transform, font, new Vector2(0f, -100f));
        pauseMenu.sensitivitySlider = CreateSettingsSlider("Pause Sensitivity Slider", "MOUSE", root.transform, font, new Vector2(0f, -174f), 0.6f, 5f, GameSettings.DefaultMouseSensitivity, out Text pauseSensitivityValue);
        pauseMenu.sensitivityValueText = pauseSensitivityValue;
        pauseMenu.volumeSlider = CreateSettingsSlider("Pause Volume Slider", "VOLUME", root.transform, font, new Vector2(0f, -226f), 0f, 1f, GameSettings.DefaultMasterVolume, out Text pauseVolumeValue);
        pauseMenu.volumeValueText = pauseVolumeValue;

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
        rectTransform.sizeDelta = new Vector2(260f, 48f);

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.22f, 0.12f, 0.045f, 0.94f);

        Button button = buttonObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.22f, 0.12f, 0.045f, 0.94f);
        colors.highlightedColor = new Color(0.58f, 0.34f, 0.12f, 1f);
        colors.pressedColor = new Color(0.95f, 0.63f, 0.2f, 1f);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;

        Text buttonText = CreateText(name + " Text", buttonObject.transform, font, label, 24, TextAnchor.MiddleCenter, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        buttonText.color = new Color(1f, 0.84f, 0.48f);

        return button;
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
        sliderRect.sizeDelta = new Vector2(300f, 24f);

        Slider slider = sliderObject.AddComponent<Slider>();
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = defaultValue;

        GameObject backgroundObject = new GameObject(name + " Track");
        backgroundObject.transform.SetParent(sliderObject.transform, false);
        RectTransform backgroundRect = backgroundObject.AddComponent<RectTransform>();
        backgroundRect.anchorMin = new Vector2(0f, 0.35f);
        backgroundRect.anchorMax = new Vector2(1f, 0.65f);
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;
        Image backgroundImage = backgroundObject.AddComponent<Image>();
        backgroundImage.color = new Color(0.12f, 0.07f, 0.035f, 0.96f);

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
        handleRect.sizeDelta = new Vector2(22f, 22f);
        Image handleImage = handleObject.AddComponent<Image>();
        handleImage.color = new Color(0.96f, 0.72f, 0.28f, 1f);

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

    private static void CreateGameState(HUDController hud, PlatformQualityProfile qualityProfile, string startMessage)
    {
        GameObject stateObject = new GameObject("Game State");
        GameStateController state = stateObject.AddComponent<GameStateController>();
        state.hud = hud;
        state.pauseMenu = UnityEngine.Object.FindAnyObjectByType<PauseMenuController>();
        state.startMessage = startMessage;
        stateObject.AddComponent<LevelTransitionController>();
        stateObject.AddComponent<SteamworksAudio>();
        RuntimePerformanceProfile performanceProfile = stateObject.AddComponent<RuntimePerformanceProfile>();
        performanceProfile.activeProfile = qualityProfile;
        stateObject.AddComponent<RuntimeSmokeTest>();
        stateObject.AddComponent<RuntimeAutoPlaythroughTest>();
        stateObject.AddComponent<RuntimeCombatTest>();
        stateObject.AddComponent<RuntimeCombatEdgeTest>();
        stateObject.AddComponent<RuntimeCombatScenarioTest>();
        stateObject.AddComponent<RuntimeRangedCombatTest>();
        stateObject.AddComponent<RuntimeInteractionTest>();
        stateObject.AddComponent<RuntimeHazardTest>();
        stateObject.AddComponent<RuntimeSecretTest>();
        stateObject.AddComponent<RuntimePauseFlowTest>();
    }

    private static void CreatePlayer(Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material gaugeFaceMaterial, Material ironMaterial, Material warningMaterial, WeaponDefinition weaponDefinition)
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
        weapon.aimCamera = camera;
        weapon.inventory = inventory;
        weapon.damage = GameBalance.PressurePistolDamage;
        weapon.fireCooldown = GameBalance.PressurePistolCooldown;

        WeaponView weaponView = CreateWeaponView(cameraObject.transform, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial, ironMaterial, warningMaterial);
        weapon.weaponView = weaponView;
    }

    private static WeaponView CreateWeaponView(Transform cameraTransform, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material gaugeFaceMaterial, Material ironMaterial, Material warningMaterial)
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

        GameObject gaugeBezel = CreateLocalPrimitive("Pressure Pistol Gauge Bezel", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(-0.18f, 0.18f, 0.02f), new Vector3(0.2f, 0.03f, 0.2f), gunTrimMaterial);
        gaugeBezel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject gauge = CreateLocalPrimitive("Pressure Pistol Gauge Face", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(-0.18f, 0.18f, -0.01f), new Vector3(0.16f, 0.02f, 0.16f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Pressure Pistol Gauge Needle", weaponRoot.transform, new Vector3(-0.14f, 0.18f, -0.035f), new Vector3(0.12f, 0.012f, 0.012f), warningMaterial).transform.localRotation = Quaternion.Euler(0f, 0f, 18f);
        CreateLocalCube("Pressure Pistol Bolt Handle", weaponRoot.transform, new Vector3(0.31f, 0.02f, 0.05f), new Vector3(0.18f, 0.05f, 0.05f), ironMaterial);
        CreateLocalPrimitive("Pressure Pistol Bolt Knob", PrimitiveType.Sphere, weaponRoot.transform, new Vector3(0.42f, 0.02f, 0.05f), new Vector3(0.075f, 0.075f, 0.075f), gunTrimMaterial);
        GameObject steamVent = CreateLocalPrimitive("Pressure Pistol Steam Vent Chimney", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0.16f, 0.25f, 0.16f), new Vector3(0.045f, 0.16f, 0.045f), ironMaterial);
        steamVent.transform.localRotation = Quaternion.identity;
        CreateLocalPrimitive("Pressure Pistol Steam Vent Cap", PrimitiveType.Sphere, weaponRoot.transform, new Vector3(0.16f, 0.42f, 0.16f), new Vector3(0.06f, 0.04f, 0.06f), gunTrimMaterial);

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

        GameObject flash = CreateLocalCube("Muzzle Flash", weaponRoot.transform, new Vector3(0f, 0.09f, 0.91f), new Vector3(0.45f, 0.45f, 0.08f), muzzleFlashMaterial);
        flash.SetActive(false);

        WeaponView weaponView = weaponRoot.AddComponent<WeaponView>();
        weaponView.muzzleFlash = flash;
        return weaponView;
    }

    private static void CreateEnemy(string name, Vector3 position, Material material, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material bladeMaterial, EnemyDefinition definition)
    {
        GameObject enemy = new GameObject(name);
        enemy.name = name;
        enemy.transform.position = position;

        CreateScrapperVisual(enemy.transform, material, eyeMaterial, brassMaterial, ironMaterial, bladeMaterial);

        CharacterController controller = enemy.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.42f;
        controller.center = Vector3.zero;

        EnemyController enemyController = enemy.AddComponent<EnemyController>();
        enemyController.definition = definition;
        enemyController.maxHealth = GameBalance.ScrapperHealth;
        enemyController.moveSpeed = GameBalance.ScrapperMoveSpeed;
        enemyController.detectionRange = GameBalance.ScrapperDetectionRange;
        enemyController.attackDamage = GameBalance.ScrapperAttackDamage;
        enemyController.attackWindup = GameBalance.ScrapperAttackWindup;
        enemyController.obstacleProbeDistance = GameBalance.ScrapperObstacleProbeDistance;
    }

    private static void CreateLancerEnemy(string name, Vector3 position, Material material, Material eyeMaterial, Material brassMaterial, Material ironMaterial, Material warningMaterial, EnemyDefinition definition)
    {
        GameObject enemy = new GameObject(name);
        enemy.transform.position = position;

        Transform muzzle = CreateLancerVisual(enemy.transform, material, eyeMaterial, brassMaterial, ironMaterial, warningMaterial);

        CharacterController controller = enemy.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.36f;
        controller.center = Vector3.zero;

        RangedEnemyController ranged = enemy.AddComponent<RangedEnemyController>();
        ranged.definition = definition;
        ranged.muzzle = muzzle;
        ranged.maxHealth = GameBalance.LancerHealth;
        ranged.detectionRange = GameBalance.LancerDetectionRange;
        ranged.fireRange = GameBalance.LancerFireRange;
        ranged.moveSpeed = GameBalance.LancerMoveSpeed;
        ranged.fireCooldown = GameBalance.LancerFireCooldown;
        ranged.fireWindup = GameBalance.LancerFireWindup;
        ranged.projectileDamage = GameBalance.LancerProjectileDamage;
        ranged.projectileSpeed = GameBalance.LancerProjectileSpeed;
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
        CreateWorkOrderBoard("Work Order Board - Intake", "ORDER 17\nSEAL MAIN\nWATCH PSI", new Vector3(-5.92f, 1.55f, 10.8f), Quaternion.Euler(0f, 90f, 0f), rivetedIronMaterial, gaugeFaceMaterial, warningMaterial, parent.transform);
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

    private static void CreateValveWheel(string name, Vector3 position, Quaternion rotation, Material brassMaterial, Material warningMaterial, Transform parent)
    {
        GameObject root = new GameObject(name);
        root.transform.SetParent(parent);
        root.transform.position = position;
        root.transform.rotation = rotation;

        GameObject wheel = CreateLocalPrimitive(name + " Wheel", PrimitiveType.Cylinder, root.transform, Vector3.zero, new Vector3(0.5f, 0.04f, 0.5f), brassMaterial);
        wheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube(name + " Spoke Horizontal", root.transform, Vector3.zero, new Vector3(0.88f, 0.045f, 0.04f), warningMaterial);
        CreateLocalCube(name + " Spoke Vertical", root.transform, Vector3.zero, new Vector3(0.04f, 0.045f, 0.88f), warningMaterial);
        CreateLocalPrimitive(name + " Hub", PrimitiveType.Sphere, root.transform, new Vector3(0f, 0f, -0.05f), new Vector3(0.14f, 0.14f, 0.14f), brassMaterial);
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
        ConfigurePickup(pickupComponent, definition, PickupKind.Health, 25);

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
        ConfigurePickup(pickupComponent, definition, PickupKind.Ammo, 15);

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
    }

    private static void CreateLockedDoor(Material material, Material brassMaterial, Material ironMaterial, Material gaugeFaceMaterial, Material warningMaterial)
    {
        GameObject frameRoot = new GameObject("Pressure Gate Frame Assembly");
        CreateDecoCube("Pressure Gate Frame Left Post", new Vector3(-1.88f, 1.67f, 22.45f), new Vector3(0.28f, 3.35f, 0.72f), ironMaterial, frameRoot.transform);
        CreateDecoCube("Pressure Gate Frame Right Post", new Vector3(1.88f, 1.67f, 22.45f), new Vector3(0.28f, 3.35f, 0.72f), ironMaterial, frameRoot.transform);
        CreateDecoCube("Pressure Gate Frame Header", new Vector3(0f, 3.38f, 22.42f), new Vector3(3.95f, 0.32f, 0.72f), ironMaterial, frameRoot.transform);
        CreateDecoCube("Pressure Gate Brass Floor Track", new Vector3(0f, 0.12f, 22.17f), new Vector3(3.82f, 0.08f, 0.18f), brassMaterial, frameRoot.transform);

        GameObject headerGear = CreateLocalPrimitive("Pressure Gate Header Gear", PrimitiveType.Cylinder, frameRoot.transform, new Vector3(0f, 3.58f, 22.04f), new Vector3(0.34f, 0.045f, 0.34f), brassMaterial);
        headerGear.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        for (int i = 0; i < 10; i++)
        {
            float angle = i * 36f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.38f, 3.58f + Mathf.Cos(radians) * 0.38f, 22.02f);
            GameObject tooth = CreateLocalCube("Pressure Gate Header Gear Tooth " + i, frameRoot.transform, toothPosition, new Vector3(0.08f, 0.14f, 0.07f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
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

        GameObject gear = CreateLocalPrimitive("Pressure Gate Gear Wheel", PrimitiveType.Cylinder, door.transform, new Vector3(0f, 0.04f, -0.55f), new Vector3(0.43f, 0.045f, 0.43f), brassMaterial);
        gear.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.45f, 0.04f + Mathf.Cos(radians) * 0.45f, -0.56f);
            GameObject tooth = CreateLocalCube("Pressure Gate Gear Tooth " + i, door.transform, toothPosition, new Vector3(0.09f, 0.15f, 0.07f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        GameObject gauge = CreateLocalPrimitive("Pressure Gate Gauge Face", PrimitiveType.Cylinder, door.transform, new Vector3(-0.58f, 0.62f, -0.47f), new Vector3(0.2f, 0.025f, 0.2f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Pressure Gate Gauge Needle", door.transform, new Vector3(-0.53f, 0.62f, -0.51f), new Vector3(0.14f, 0.014f, 0.014f), warningMaterial);

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

    private static void CreateLevelTransitionLiftAt(string name, Vector3 position, Material material, Material ironMaterial, Material brassMaterial, Material gaugeFaceMaterial, string targetSceneName, string transitionMessage)
    {
        GameObject lift = CreateServiceLiftShell(name, position, material, ironMaterial, brassMaterial, gaugeFaceMaterial);
        LevelTransitionTrigger transition = lift.AddComponent<LevelTransitionTrigger>();
        transition.targetSceneName = targetSceneName;
        transition.transitionMessage = transitionMessage;
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

        GameObject pulley = CreateLocalPrimitive(name + " Overhead Pulley Gear", PrimitiveType.Cylinder, lift.transform, new Vector3(0f, 0.9f, -0.72f), new Vector3(0.3f, 0.045f, 0.3f), brassMaterial);
        pulley.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        for (int i = 0; i < 10; i++)
        {
            float angle = i * 36f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.34f, 0.9f + Mathf.Cos(radians) * 0.34f, -0.74f);
            GameObject tooth = CreateLocalCube(name + " Pulley Tooth " + i, lift.transform, toothPosition, new Vector3(0.07f, 0.12f, 0.06f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
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
