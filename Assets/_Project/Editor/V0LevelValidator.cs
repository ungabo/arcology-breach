using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public static class V0LevelValidator
{
    private const string MainMenuScenePath = "Assets/_Project/Scenes/MainMenu.unity";
    private const string Level01ScenePath = "Assets/_Project/Scenes/Level01.unity";
    private const string Level02ScenePath = "Assets/_Project/Scenes/Level02.unity";
    private const string Level03ScenePath = "Assets/_Project/Scenes/Level03.unity";
    private const string Level04ScenePath = "Assets/_Project/Scenes/Level04.unity";
    private const string Level05ScenePath = "Assets/_Project/Scenes/Level05.unity";
    private const string MaterialFolder = "Assets/_Project/Materials";
    private const string FinalMaterialsTextureFolder = "Assets/_Project/ArtStaging/FinalMaterialsV1/Textures";
    private const string SignageObjectiveTexturePath = "Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_ObjectivePlates_2048.png";
    private const string SignageWarningTexturePath = "Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_WarningHazardStrips_2048.png";
    private const string SignageRouteTexturePath = "Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_RouteArrowsChevrons_2048.png";
    private const string SignageStencilTexturePath = "Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_StencilMachineryLore_2048.png";
    private const string SignageSecretTexturePath = "Assets/_Project/ArtStaging/SignageDecalsV1/Textures/T_SignageDecalsV1_SecretServiceMarks_2048.png";
    private const string UIHudRoot = "Assets/_Project/ArtStaging/UIHudV1";
    private const string AudioV1Folder = "Assets/_Project/ArtStaging/AudioV1";

    [MenuItem("Project Tools/Validate v0 Levels")]
    public static void RunValidation()
    {
        ValidateProjectScenes();
        Debug.Log("V0_LEVEL_VALIDATION_PASS");
    }

    public static void ValidateProjectScenes()
    {
        ValidateBuildSceneOrder();
        ValidateFinalMaterialsV1();
        ValidateUIHudV1SpriteImports();
        ValidateAudioV1Imports();

        EditorSceneManager.OpenScene(MainMenuScenePath);
        MainMenuController mainMenu = Require<MainMenuController>("MainMenuController");
        RuntimePerformanceProfile mainMenuPerformanceProfile = Require<RuntimePerformanceProfile>("MainMenu RuntimePerformanceProfile");
        ValidatePlatformQualityProfile("MainMenu", mainMenuPerformanceProfile);
        Require<SteamworksSpinner>("MainMenu SteamworksSpinner");
        ValidateMainMenuUIHudV1();
        ValidateMainMenuSettings(mainMenu);

        EditorSceneManager.OpenScene(Level01ScenePath);
        ValidateGameplayScene("Level01", requirePressureGate: true, requireTransition: true, requireFinalExit: false, requireRangedEnemy: false);

        EditorSceneManager.OpenScene(Level02ScenePath);
        ValidateGameplayScene("Level02", requirePressureGate: false, requireTransition: true, requireFinalExit: false, requireRangedEnemy: true);

        EditorSceneManager.OpenScene(Level03ScenePath);
        ValidateGameplayScene("Level03", requirePressureGate: false, requireTransition: true, requireFinalExit: false, requireRangedEnemy: false);

        EditorSceneManager.OpenScene(Level04ScenePath);
        ValidateGameplayScene("Level04", requirePressureGate: false, requireTransition: true, requireFinalExit: false, requireRangedEnemy: true);

        EditorSceneManager.OpenScene(Level05ScenePath);
        ValidateGameplayScene("Level05", requirePressureGate: false, requireTransition: false, requireFinalExit: true, requireRangedEnemy: true);
    }

    private static void ValidateBuildSceneOrder()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        if (scenes.Length < 6 || scenes[0].path != MainMenuScenePath || scenes[1].path != Level01ScenePath || scenes[2].path != Level02ScenePath || scenes[3].path != Level03ScenePath || scenes[4].path != Level04ScenePath || scenes[5].path != Level05ScenePath)
        {
            throw new InvalidOperationException("Level validation failed: build scenes must be MainMenu, Level01, Level02, Level03, Level04, Level05.");
        }
    }

    private static void ValidateFinalMaterialsV1()
    {
        RequireFinalMaterialBinding("M_Greybox_SootBrickWall", "SootBrick");
        RequireFinalMaterialBinding("M_Greybox_OilStoneFloor", "WetOilDarkStone");
        RequireFinalMaterialBinding("M_Greybox_PressureGate", "BlackenedRivetedIron");
        RequireFinalMaterialBinding("M_Greybox_GearKey", "AgedBrass");
        RequireFinalMaterialBinding("M_Greybox_BrassTrim", "AgedBrass");
        RequireFinalMaterialBinding("M_Greybox_BrassGuide", "CopperPipe");
        RequireFinalMaterialBinding("M_Greybox_PressureWarning", "HazardEnamel");
        RequireFinalMaterialBinding("M_Steam_RivetedIron", "BlackenedRivetedIron");
        RequireFinalMaterialBinding("M_Steam_OilDarkStone", "WetOilDarkStone");
        RequireFinalMaterialBinding("M_Steam_CreamGaugeFace", "CreamEnamelGauge");
        RequireFinalMaterialBinding("M_Greybox_WalnutGrip", "GreasyWalnut");
    }

    private static void ValidateUIHudV1SpriteImports()
    {
        RequireUiSpriteImport("Gauges/HUD_HealthGauge_Frame_512x96.png");
        RequireUiSpriteImport("Gauges/HUD_HealthGauge_Fill_Red_384x32.png");
        RequireUiSpriteImport("Gauges/HUD_PressureAmmoGauge_Frame_512x96.png");
        RequireUiSpriteImport("Gauges/HUD_PressureAmmoGauge_Fill_Amber_384x32.png");
        RequireUiSpriteImport("Gauges/HUD_BossPressureGauge_Frame_768x96.png");
        RequireUiSpriteImport("Gauges/HUD_BossPressureGauge_Fill_Red_704x24.png");
        RequireUiSpriteImport("Panels/HUD_ObjectiveBackplate_640x72.png");
        RequireUiSpriteImport("Panels/HUD_PromptBackplate_640x80.png");
        RequireUiSpriteImport("Panels/PANEL_Menu_BrassPanel_768x384.png");
        RequireUiSpriteImport("Panels/PANEL_Menu_Header_768x96.png");
        RequireUiSpriteImport("Panels/PANEL_Menu_Button_Normal_320x64.png");
        RequireUiSpriteImport("Panels/PANEL_Menu_Button_Hover_320x64.png");
        RequireUiSpriteImport("Panels/PANEL_Menu_Button_Pressed_320x64.png");
        RequireUiSpriteImport("Panels/PANEL_Menu_SliderTrack_360x40.png");
        RequireUiSpriteImport("Panels/PANEL_Menu_SliderHandle_48x48.png");
        RequireUiSpriteImport("Icons/HUD_KeyLamp_Off_96x96.png");
        RequireUiSpriteImport("Icons/HUD_KeyLamp_On_96x96.png");
        RequireUiSpriteImport("Icons/HUD_KeyLamp_Denied_96x96.png");
        RequireUiSpriteImport("Reticles/RETICLE_BrassCrosshair_64x64.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_InteractE_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_GearKey_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_Valve_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_Lift_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_Ammo_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_Health_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_Warning_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_Secret_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_Pause_96x96.png");
        RequireUiSpriteImport("Icons/ICON_Prompt_MouseRight_96x96.png");
    }

    private static void ValidateAudioV1Imports()
    {
        string[] audioGuids = AssetDatabase.FindAssets("t:AudioClip", new[] { AudioV1Folder });
        if (audioGuids.Length < 30)
        {
            throw new InvalidOperationException("Level validation failed: expected at least 30 staged AudioV1 clips but found " + audioGuids.Length + ".");
        }

        for (int i = 0; i < audioGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(audioGuids[i]);
            if (!path.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            if (clip == null || clip.samples <= 0 || clip.frequency != 48000)
            {
                throw new InvalidOperationException("Level validation failed: AudioV1 clip did not import at the expected 48 kHz shape: " + path + ".");
            }

            AudioImporter importer = AssetImporter.GetAtPath(path) as AudioImporter;
            if (importer == null)
            {
                throw new InvalidOperationException("Level validation failed: AudioV1 clip has no AudioImporter: " + path + ".");
            }

            bool isLoop = IsAudioV1Loop(path);
            AudioImporterSampleSettings settings = importer.defaultSampleSettings;
            AudioClipLoadType expectedLoadType = isLoop ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
            AudioCompressionFormat expectedCompression = isLoop ? AudioCompressionFormat.Vorbis : AudioCompressionFormat.ADPCM;
            float expectedQuality = isLoop ? 0.72f : 1f;

            if (settings.loadType != expectedLoadType || settings.compressionFormat != expectedCompression || !Mathf.Approximately(settings.quality, expectedQuality) || settings.sampleRateSetting != AudioSampleRateSetting.PreserveSampleRate)
            {
                throw new InvalidOperationException("Level validation failed: AudioV1 import settings are not tuned for " + path + ".");
            }

            if (importer.forceToMono || importer.loadInBackground != isLoop || settings.preloadAudioData == isLoop)
            {
                throw new InvalidOperationException("Level validation failed: AudioV1 background/preload settings are not tuned for " + path + ".");
            }
        }
    }

    private static bool IsAudioV1Loop(string path)
    {
        return path.IndexOf("_loop", StringComparison.OrdinalIgnoreCase) >= 0
            || path.IndexOf("Loop.wav", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static void RequireUiSpriteImport(string relativePath)
    {
        string path = UIHudRoot + "/" + relativePath;
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
        {
            throw new InvalidOperationException("Level validation failed: missing UIHudV1 texture " + path + ".");
        }

        if (importer.textureType != TextureImporterType.Sprite || importer.spriteImportMode != SpriteImportMode.Single || importer.mipmapEnabled || !importer.alphaIsTransparency)
        {
            throw new InvalidOperationException("Level validation failed: UIHudV1 texture is not imported for runtime UI: " + path + ".");
        }

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null)
        {
            throw new InvalidOperationException("Level validation failed: UIHudV1 texture did not load as a sprite: " + path + ".");
        }
    }

    private static void ValidateMainMenuUIHudV1()
    {
        RequireImageSprite("MainMenu", "Menu Brass Panel UIHudV1", "Panels/PANEL_Menu_BrassPanel_768x384.png");
        RequireImageSprite("MainMenu", "Menu Header Panel UIHudV1", "Panels/PANEL_Menu_Header_768x96.png");
        RequireImageSprite("MainMenu", "Start Button", "Panels/PANEL_Menu_Button_Normal_320x64.png");
        RequireImageSprite("MainMenu", "Quit Button", "Panels/PANEL_Menu_Button_Normal_320x64.png");
        RequireImageSprite("MainMenu", "Menu Resolution Button", "Panels/PANEL_Menu_Button_Normal_320x64.png");
        RequireImageSprite("MainMenu", "Menu Sensitivity Slider Track", "Panels/PANEL_Menu_SliderTrack_360x40.png");
        RequireImageSprite("MainMenu", "Menu Sensitivity Slider Valve Handle", "Panels/PANEL_Menu_SliderHandle_48x48.png");
        RequireImageSprite("MainMenu", "Menu Volume Slider Track", "Panels/PANEL_Menu_SliderTrack_360x40.png");
        RequireImageSprite("MainMenu", "Menu Volume Slider Valve Handle", "Panels/PANEL_Menu_SliderHandle_48x48.png");
        RequireImageSprite("MainMenu", "Menu Flash Slider Track", "Panels/PANEL_Menu_SliderTrack_360x40.png");
        RequireImageSprite("MainMenu", "Menu Flash Slider Valve Handle", "Panels/PANEL_Menu_SliderHandle_48x48.png");
    }

    private static void ValidateMainMenuSettings(MainMenuController mainMenu)
    {
        if (mainMenu.sensitivitySlider == null || mainMenu.volumeSlider == null || mainMenu.flashSlider == null || mainMenu.resolutionButton == null || mainMenu.fullscreenToggle == null || mainMenu.highContrastToggle == null || mainMenu.sensitivityValueText == null || mainMenu.volumeValueText == null || mainMenu.flashValueText == null || mainMenu.resolutionValueText == null || mainMenu.fullscreenValueText == null || mainMenu.highContrastValueText == null)
        {
            throw new InvalidOperationException("Level validation failed: MainMenu is missing settings slider wiring.");
        }

        ValidateCoreSettingsControls("MainMenu", mainMenu.sensitivitySlider, mainMenu.volumeSlider, mainMenu.fullscreenToggle, mainMenu.highContrastToggle);
        ValidateFlashSlider("MainMenu", mainMenu.flashSlider);
    }

    private static void ValidatePauseMenuSettings(string sceneName, PauseMenuController pauseMenu)
    {
        if (pauseMenu.sensitivitySlider == null || pauseMenu.volumeSlider == null || pauseMenu.flashSlider == null || pauseMenu.resolutionButton == null || pauseMenu.fullscreenToggle == null || pauseMenu.highContrastToggle == null || pauseMenu.sensitivityValueText == null || pauseMenu.volumeValueText == null || pauseMenu.flashValueText == null || pauseMenu.resolutionValueText == null || pauseMenu.fullscreenValueText == null || pauseMenu.highContrastValueText == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " PauseMenu is missing settings slider wiring.");
        }

        ValidateCoreSettingsControls(sceneName + " PauseMenu", pauseMenu.sensitivitySlider, pauseMenu.volumeSlider, pauseMenu.fullscreenToggle, pauseMenu.highContrastToggle);
        ValidateFlashSlider(sceneName + " PauseMenu", pauseMenu.flashSlider);
    }

    private static void ValidateCoreSettingsControls(string label, Slider sensitivitySlider, Slider volumeSlider, Toggle fullscreenToggle, Toggle highContrastToggle)
    {
        RequireApprox(sensitivitySlider.minValue, 0.6f, label + " sensitivity slider minimum");
        RequireApprox(sensitivitySlider.maxValue, 5f, label + " sensitivity slider maximum");
        RequireApprox(volumeSlider.minValue, 0f, label + " volume slider minimum");
        RequireApprox(volumeSlider.maxValue, 1f, label + " volume slider maximum");

        if (fullscreenToggle.targetGraphic == null || fullscreenToggle.graphic == null)
        {
            throw new InvalidOperationException("Level validation failed: " + label + " fullscreen toggle is missing graphics.");
        }

        if (highContrastToggle.targetGraphic == null || highContrastToggle.graphic == null)
        {
            throw new InvalidOperationException("Level validation failed: " + label + " contrast toggle is missing graphics.");
        }
    }

    private static void ValidateFlashSlider(string label, Slider slider)
    {
        RequireApprox(slider.minValue, GameSettings.MinFlashIntensity, label + " flash slider minimum");
        RequireApprox(slider.maxValue, GameSettings.MaxFlashIntensity, label + " flash slider maximum");
    }

    private static void ValidateGameplayUIHudV1(string sceneName, HUDController hud)
    {
        if (hud.interactionBackplateImage == null || hud.interactionIconImage == null || hud.keyLampOffSprite == null || hud.keyLampOnSprite == null || hud.keyLampDeniedSprite == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " HUDController is missing UIHudV1 sprite wiring.");
        }

        if (hud.promptInteractSprite == null || hud.promptGearKeySprite == null || hud.promptValveSprite == null || hud.promptLiftSprite == null || hud.promptWarningSprite == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " HUDController is missing UIHudV1 prompt icon sprites.");
        }

        RequireImageSprite(sceneName, "Health Gauge Frame UIHudV1", "Gauges/HUD_HealthGauge_Frame_512x96.png");
        RequireImageSprite(sceneName, "Health Gauge Fill UIHudV1", "Gauges/HUD_HealthGauge_Fill_Red_384x32.png");
        RequireImageSprite(sceneName, "Ammo Gauge Frame UIHudV1", "Gauges/HUD_PressureAmmoGauge_Frame_512x96.png");
        RequireImageSprite(sceneName, "Ammo Gauge Fill UIHudV1", "Gauges/HUD_PressureAmmoGauge_Fill_Amber_384x32.png");
        RequireImageSprite(sceneName, "Gear Key Lamp UIHudV1", "Icons/HUD_KeyLamp_Off_96x96.png");
        RequireImageSprite(sceneName, "Objective Backplate UIHudV1", "Panels/HUD_ObjectiveBackplate_640x72.png");
        RequireImageSprite(sceneName, "Interaction Prompt Backplate UIHudV1", "Panels/HUD_PromptBackplate_640x80.png");
        RequireImageSprite(sceneName, "Interaction Prompt Icon UIHudV1", "Icons/ICON_Prompt_InteractE_96x96.png");
        RequireImageSprite(sceneName, "Boss Gauge Frame UIHudV1", "Gauges/HUD_BossPressureGauge_Frame_768x96.png");
        RequireImageSprite(sceneName, "Boss Gauge Fill UIHudV1", "Gauges/HUD_BossPressureGauge_Fill_Red_704x24.png");
        RequireImageSprite(sceneName, "Brass Reticle UIHudV1", "Reticles/RETICLE_BrassCrosshair_64x64.png");
        RequireImageSprite(sceneName, "Pause Brass Panel UIHudV1", "Panels/PANEL_Menu_BrassPanel_768x384.png");
        RequireImageSprite(sceneName, "Pause Header Panel UIHudV1", "Panels/PANEL_Menu_Header_768x96.png");
        RequireImageSprite(sceneName, "Pause Resolution Button", "Panels/PANEL_Menu_Button_Normal_320x64.png");
        RequireImageSprite(sceneName, "Pause Flash Slider Track", "Panels/PANEL_Menu_SliderTrack_360x40.png");
        RequireImageSprite(sceneName, "Pause Flash Slider Valve Handle", "Panels/PANEL_Menu_SliderHandle_48x48.png");
    }

    private static void ValidateHudReadability(string sceneName, HUDController hud)
    {
        ValidateReadableText(sceneName, "objective", hud.objectiveText, HorizontalWrapMode.Wrap, 16, 20);
        ValidateReadableText(sceneName, "interaction", hud.interactionText, HorizontalWrapMode.Wrap, 18, 24);
        ValidateReadableText(sceneName, "message", hud.messageText, HorizontalWrapMode.Wrap, 24, 34);
    }

    private static void ValidateReadableText(string sceneName, string label, Text text, HorizontalWrapMode expectedWrap, int minSize, int maxSize)
    {
        if (text == null || !text.resizeTextForBestFit || text.horizontalOverflow != expectedWrap || text.verticalOverflow != VerticalWrapMode.Truncate || text.resizeTextMinSize != minSize || text.resizeTextMaxSize != maxSize)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " " + label + " text is missing v0.1.9 readability sizing.");
        }
    }

    private static void ValidateWorldLabelReadability(string sceneName)
    {
        WorldLabelBillboard[] labels = UnityEngine.Object.FindObjectsByType<WorldLabelBillboard>(FindObjectsSortMode.None);
        int minimumCount = sceneName == "Level01" ? 5 : 1;
        if (labels.Length < minimumCount)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " expected at least " + minimumCount + " v0.1.37 world-label readability components but found " + labels.Length + ".");
        }

        for (int i = 0; i < labels.Length; i++)
        {
            WorldLabelBillboard label = labels[i];
            if (!label.V0137ReadabilityReady)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " world label is missing v0.1.37 readability metadata: " + label.name + ".");
            }

            if (label.GetComponent<Collider>() != null || label.GetComponent<Rigidbody>() != null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " world label has gameplay physics: " + label.name + ".");
            }

            if (label.textMesh.anchor != TextAnchor.MiddleCenter || label.textMesh.alignment != TextAlignment.Center)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " world label is not center anchored for billboard readability: " + label.name + ".");
            }

            if (label.backplateRenderer.GetComponent<Collider>() != null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " world label backplate has a collider: " + label.name + ".");
            }
        }

        if (sceneName == "Level01")
        {
            RequireNamed("Label - Gear Key", sceneName + " world label gear key");
            RequireNamed("Label - Gear Key Readability Backplate", sceneName + " world label gear key backplate");
            RequireNamed("Label - Pressure Gate", sceneName + " world label pressure gate");
            RequireNamed("Label - Pressure Gate Readability Backplate", sceneName + " world label pressure gate backplate");
            RequireNamed("Label - Service Lift", sceneName + " world label service lift");
            RequireNamed("Label - Service Lift Readability Backplate", sceneName + " world label service lift backplate");
        }
    }

    private static void RequireFinalMaterialBinding(string materialName, string familyName)
    {
        Material material = AssetDatabase.LoadAssetAtPath<Material>(MaterialFolder + "/" + materialName + ".mat");
        if (material == null)
        {
            throw new InvalidOperationException("Level validation failed: missing FinalMaterialsV1-bound material " + materialName + ".");
        }

        RequireMaterialTexture(material, materialName, familyName, "BaseColor", "_BaseMap", "_MainTex");
        RequireMaterialTexture(material, materialName, familyName, "Normal", "_BumpMap", null);
        RequireMaterialTexture(material, materialName, familyName, "ORM", "_OcclusionMap", null);
    }

    private static void RequireMaterialTexture(Material material, string materialName, string familyName, string suffix, string primaryProperty, string fallbackProperty)
    {
        Texture texture = null;
        if (material.HasProperty(primaryProperty))
        {
            texture = material.GetTexture(primaryProperty);
        }

        if (texture == null && !string.IsNullOrEmpty(fallbackProperty) && material.HasProperty(fallbackProperty))
        {
            texture = material.GetTexture(fallbackProperty);
        }

        if (texture == null && suffix == "BaseColor")
        {
            texture = material.mainTexture;
        }

        if (texture == null)
        {
            throw new InvalidOperationException("Level validation failed: material " + materialName + " is missing its FinalMaterialsV1 " + suffix + " texture.");
        }

        string expectedPath = FinalMaterialsTextureFolder + "/T_BBW_" + familyName + "_" + suffix + "_2048.png";
        string actualPath = AssetDatabase.GetAssetPath(texture).Replace("\\", "/");
        if (actualPath != expectedPath)
        {
            throw new InvalidOperationException("Level validation failed: material " + materialName + " expected " + expectedPath + " but found " + actualPath + ".");
        }
    }

    private static void ValidateGameplayScene(string sceneName, bool requirePressureGate, bool requireTransition, bool requireFinalExit, bool requireRangedEnemy)
    {
        PlayerController playerController = Require<PlayerController>(sceneName + " PlayerController");
        Require<PlayerHealth>(sceneName + " PlayerHealth");
        PlayerInventory playerInventory = Require<PlayerInventory>(sceneName + " PlayerInventory");
        PlayerInteraction playerInteraction = Require<PlayerInteraction>(sceneName + " PlayerInteraction");
        WeaponController weaponController = Require<WeaponController>(sceneName + " WeaponController");
        ValidateBalanceValues(sceneName, playerController, playerInventory, weaponController);
        ValidateInteractionSystem(sceneName, playerController, playerInteraction);
        ValidateWeaponVisuals(sceneName);
        GameStateController gameState = Require<GameStateController>(sceneName + " GameStateController");
        ValidateStartMessage(sceneName, gameState);
        Require<LevelTransitionController>(sceneName + " LevelTransitionController");
        SteamworksAudio audio = Require<SteamworksAudio>(sceneName + " SteamworksAudio");
        ValidateSteamworksAudio(sceneName, audio);
        GameplayFeedbackController feedback = Require<GameplayFeedbackController>(sceneName + " GameplayFeedbackController");
        ValidateGameplayFeedback(sceneName, feedback);
        RuntimePerformanceProfile performanceProfile = Require<RuntimePerformanceProfile>(sceneName + " RuntimePerformanceProfile");
        ValidatePlatformQualityProfile(sceneName, performanceProfile);
        HUDController hud = Require<HUDController>(sceneName + " HUDController");
        if (hud.interactionText == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " HUDController is missing interaction prompt text.");
        }

        if (hud.objectiveText == null || hud.objectiveBackplateImage == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " HUDController is missing objective UI wiring.");
        }

        if (hud.bossNameText == null || hud.bossBackplateImage == null || hud.bossFillImage == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " HUDController is missing boss health UI wiring.");
        }

        ValidateGameplayUIHudV1(sceneName, hud);
        ValidateHudReadability(sceneName, hud);
        ValidateWorldLabelReadability(sceneName);

        PauseMenuController pauseMenu = Require<PauseMenuController>(sceneName + " PauseMenuController");
        ValidatePauseMenuSettings(sceneName, pauseMenu);
        Require<RuntimeInteractionTest>(sceneName + " RuntimeInteractionTest");
        Require<RuntimeCombatScenarioTest>(sceneName + " RuntimeCombatScenarioTest");
        Require<RuntimeBellowsNodeTest>(sceneName + " RuntimeBellowsNodeTest");
        Require<RuntimeBulwarkCombatTest>(sceneName + " RuntimeBulwarkCombatTest");
        Require<RuntimeWardenCombatTest>(sceneName + " RuntimeWardenCombatTest");
        Require<RuntimeHazardTest>(sceneName + " RuntimeHazardTest");
        Require<RuntimeSecretTest>(sceneName + " RuntimeSecretTest");
        Require<RuntimeWeaponSwitchTest>(sceneName + " RuntimeWeaponSwitchTest");
        Require<RuntimeMovementFeelTest>(sceneName + " RuntimeMovementFeelTest");
        Require<RuntimeBalanceEnvelopeTest>(sceneName + " RuntimeBalanceEnvelopeTest");
        Require<RuntimeLevel01FlowTest>(sceneName + " RuntimeLevel01FlowTest");
        Require<RuntimeMidgameFlowTest>(sceneName + " RuntimeMidgameFlowTest");
        Require<RuntimeClimaxFlowTest>(sceneName + " RuntimeClimaxFlowTest");
        Require<RuntimeAudioMixTest>(sceneName + " RuntimeAudioMixTest");
        Require<RuntimeDisplaySettingsTest>(sceneName + " RuntimeDisplaySettingsTest");
        Require<RuntimeReadabilitySettingsTest>(sceneName + " RuntimeReadabilitySettingsTest");
        Require<RuntimeGameplayFeedbackTest>(sceneName + " RuntimeGameplayFeedbackTest");
        Require<RuntimeWorldLabelReadabilityTest>(sceneName + " RuntimeWorldLabelReadabilityTest");
        Require<EnemyController>(sceneName + " EnemyController");
        Require<Pickup>(sceneName + " Pickup");

        ValidatePickups(sceneName);
        ValidateEnemies(sceneName);
        ValidateHazards(sceneName);
        ValidateSecrets(sceneName);
        ValidateEnvironmentPropVisuals(sceneName);
        ValidateLorePlaques(sceneName);
        ValidateSignageDecalsV1(sceneName);
        ValidateMachineryMotion(sceneName);

        if (requirePressureGate)
        {
            LockedDoor door = Require<LockedDoor>(sceneName + " LockedDoor");
            RequireCollider(door.gameObject, sceneName + " LockedDoor collider");
            RequireInteractable(door, sceneName + " pressure gate interactable");
            RequireNamed("Pressure Gate Frame Assembly", sceneName + " pressure gate frame visual");
            RequireNamed("Pressure Gate Key Socket", sceneName + " pressure gate key socket visual");
            RequireNamed("Pressure Gate Warning Lamp Left", sceneName + " pressure gate warning lamp visual");
            ValidatePressureGaugePrototype(sceneName, "Pressure Gate Prototype Gauge", "pressure_gate_panel");
            RequireNamed("Pickup - Gear Key Clockwork Key Visual", sceneName + " gear-key visual root");
            RequireNamed("Pickup - Gear Key Key Bit Lower", sceneName + " gear-key bit visual");
        }

        if (requireTransition)
        {
            LevelTransitionTrigger transition = Require<LevelTransitionTrigger>(sceneName + " LevelTransitionTrigger");
            RequireTrigger(transition.gameObject, sceneName + " LevelTransitionTrigger trigger");
            RequireInteractable(transition, sceneName + " service lift interactable");
            ValidateServiceLiftVisuals(transition.gameObject, sceneName + " transition lift");
            if (string.IsNullOrWhiteSpace(transition.targetSceneName))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " transition has no target scene.");
            }

            if (sceneName == "Level02")
            {
                SteamValveObjective valve = Require<SteamValveObjective>(sceneName + " SteamValveObjective");
                RequireTrigger(valve.gameObject, sceneName + " Pipeworks routing valve trigger");
                RequireInteractable(valve, sceneName + " Pipeworks routing valve interactable");
                if (transition.requiredValve != valve)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " Boilerheart lift is not linked to the Pipeworks routing valve.");
                }

                if (!transition.IsLocked)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " Boilerheart lift must start pressure-locked.");
                }

                if (string.IsNullOrWhiteSpace(valve.objectiveAfterComplete))
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " Pipeworks routing valve has no follow-up objective.");
                }
            }

            if (sceneName == "Level03")
            {
                SteamValveObjective valve = Require<SteamValveObjective>(sceneName + " SteamValveObjective");
                RequireTrigger(valve.gameObject, sceneName + " SteamValveObjective trigger");
                RequireInteractable(valve, sceneName + " boilerheart pressure valve interactable");
                if (transition.requiredValve != valve)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " foundry lift is not linked to the Boilerheart pressure valve.");
                }

                if (!transition.IsLocked)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " foundry lift must start pressure-locked.");
                }

                if (valve.hazardsToDisableOnComplete == null || valve.hazardsToDisableOnComplete.Length < 2)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " Boilerheart pressure valve is not linked to steam hazards.");
                }
            }
        }

        if (requireFinalExit)
        {
            ExitTrigger exit = Require<ExitTrigger>(sceneName + " ExitTrigger");
            RequireTrigger(exit.gameObject, sceneName + " ExitTrigger trigger");
            RequireInteractable(exit, sceneName + " final lift interactable");
            ValidateServiceLiftVisuals(exit.gameObject, sceneName + " final service lift");

            if (sceneName == "Level05")
            {
                GuardianDefeatObjective guardianObjective = Require<GuardianDefeatObjective>(sceneName + " GuardianDefeatObjective");
                if (exit.requiredGuardian != guardianObjective)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " final hoist is not linked to the Governor Warden objective.");
                }

                if (!exit.IsLocked)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " final hoist must start guardian-locked.");
                }

                if (guardianObjective.target == null || guardianObjective.lockedSignal == null || guardianObjective.clearedSignal == null)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " Governor Warden objective is missing target or signals.");
                }
            }
        }

        if (requireRangedEnemy)
        {
            RangedEnemyController ranged = Require<RangedEnemyController>(sceneName + " RangedEnemyController");
            if (ranged.muzzle == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " RangedEnemyController is missing muzzle.");
            }
        }

        if (sceneName == "Level04" || sceneName == "Level05")
        {
            Require<BulwarkEnemyController>(sceneName + " BulwarkEnemyController");
        }

        if (sceneName == "Level03")
        {
            Require<BellowsNodeController>(sceneName + " BellowsNodeController");
        }

        if (sceneName == "Level05")
        {
            GovernorWardenController warden = Require<GovernorWardenController>(sceneName + " GovernorWardenController");
            if (warden.muzzle == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " GovernorWardenController is missing muzzle.");
            }
        }
    }

    private static void ValidatePickups(string sceneName)
    {
        Pickup[] pickups = UnityEngine.Object.FindObjectsByType<Pickup>(FindObjectsSortMode.None);
        foreach (Pickup pickup in pickups)
        {
            RequireTrigger(pickup.gameObject, sceneName + " pickup trigger " + pickup.name);
            if (pickup.definition == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " pickup " + pickup.name + " is missing a PickupDefinition.");
            }

            RequireEqual((int)pickup.kind, (int)pickup.definition.kind, sceneName + " pickup kind definition " + pickup.name);
            RequireEqual(pickup.amount, pickup.definition.amount, sceneName + " pickup amount definition " + pickup.name);
            RequireApprox(pickup.collectRadius, pickup.definition.collectRadius, sceneName + " pickup collect radius definition " + pickup.name);
            if (pickup.kind == PickupKind.Health)
            {
                RequireEqual(pickup.amount, GameBalance.HealthPickupAmount, sceneName + " health pickup amount balance " + pickup.name);
            }

            if (pickup.kind == PickupKind.Ammo)
            {
                RequireEqual(pickup.amount, GameBalance.AmmoPickupAmount, sceneName + " ammo pickup amount balance " + pickup.name);
            }

            if (string.IsNullOrWhiteSpace(pickup.definition.collectMessage))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " pickup " + pickup.name + " definition has no collect message.");
            }

            if (pickup.kind == PickupKind.Weapon && string.IsNullOrWhiteSpace(pickup.definition.weaponUnlockId))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " weapon pickup " + pickup.name + " has no weapon unlock id.");
            }

            if (pickup.kind == PickupKind.Weapon && pickup.definition.audioCue != SteamworksAudioCue.WeaponPickup)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " weapon pickup " + pickup.name + " does not use the weapon pickup audio cue.");
            }
        }
    }

    private static void ValidateInteractionSystem(string sceneName, PlayerController playerController, PlayerInteraction playerInteraction)
    {
        if (playerInteraction.viewTransform == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " PlayerInteraction is missing viewTransform.");
        }

        if (playerController.playerCamera != null && playerInteraction.viewTransform != playerController.playerCamera)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " PlayerInteraction viewTransform does not match player camera.");
        }

        if (playerInteraction.interactKey != KeyCode.E)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " PlayerInteraction interact key must be E.");
        }

        if (playerInteraction.interactionRange < 2.5f)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " PlayerInteraction range is too short.");
        }
    }

    private static void ValidateEnemies(string sceneName)
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.GetComponent<CharacterController>() == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Scrapper missing CharacterController.");
            }

            RequireEqual(enemy.maxHealth, GameBalance.ScrapperHealth, sceneName + " Scrapper health balance");
            RequireApprox(enemy.moveSpeed, GameBalance.ScrapperMoveSpeed, sceneName + " Scrapper speed balance");
            RequireApprox(enemy.detectionRange, GameBalance.ScrapperDetectionRange, sceneName + " Scrapper detection balance");
            RequireApprox(enemy.attackRange, GameBalance.ScrapperAttackRange, sceneName + " Scrapper attack range balance");
            RequireEqual(enemy.attackDamage, GameBalance.ScrapperAttackDamage, sceneName + " Scrapper damage balance");
            RequireApprox(enemy.attackCooldown, GameBalance.ScrapperAttackCooldown, sceneName + " Scrapper cooldown balance");
            RequireApprox(enemy.attackWindup, GameBalance.ScrapperAttackWindup, sceneName + " Scrapper windup balance");
            RequireApprox(enemy.obstacleProbeDistance, GameBalance.ScrapperObstacleProbeDistance, sceneName + " Scrapper obstacle probe balance");
            if (enemy.definition == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Scrapper is missing an EnemyDefinition.");
            }

            RequireEqual((int)enemy.definition.attackStyle, (int)EnemyAttackStyle.Melee, sceneName + " Scrapper definition style");
            RequireEqual(enemy.definition.maxHealth, GameBalance.ScrapperHealth, sceneName + " Scrapper definition health");
            RequireApprox(enemy.definition.moveSpeed, GameBalance.ScrapperMoveSpeed, sceneName + " Scrapper definition speed");
            RequireApprox(enemy.definition.attackRange, GameBalance.ScrapperAttackRange, sceneName + " Scrapper definition attack range");
            RequireEqual(enemy.definition.attackDamage, GameBalance.ScrapperAttackDamage, sceneName + " Scrapper definition damage");
            RequireApprox(enemy.definition.attackCooldown, GameBalance.ScrapperAttackCooldown, sceneName + " Scrapper definition cooldown");
            RequireMachineMotion(enemy.gameObject, sceneName + " Scrapper machine motion");
            RequireScrapperAttackTell(enemy.gameObject, sceneName + " Scrapper attack tell");
        }

        RangedEnemyController[] rangedEnemies = UnityEngine.Object.FindObjectsByType<RangedEnemyController>(FindObjectsSortMode.None);
        foreach (RangedEnemyController enemy in rangedEnemies)
        {
            if (enemy.GetComponent<CharacterController>() == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Lancer missing CharacterController.");
            }

            RequireEqual(enemy.maxHealth, GameBalance.LancerHealth, sceneName + " Lancer health balance");
            RequireApprox(enemy.detectionRange, GameBalance.LancerDetectionRange, sceneName + " Lancer detection balance");
            RequireApprox(enemy.fireRange, GameBalance.LancerFireRange, sceneName + " Lancer fire range balance");
            RequireApprox(enemy.moveSpeed, GameBalance.LancerMoveSpeed, sceneName + " Lancer speed balance");
            RequireApprox(enemy.fireCooldown, GameBalance.LancerFireCooldown, sceneName + " Lancer cooldown balance");
            RequireApprox(enemy.fireWindup, GameBalance.LancerFireWindup, sceneName + " Lancer windup balance");
            RequireEqual(enemy.projectileDamage, GameBalance.LancerProjectileDamage, sceneName + " Lancer projectile damage balance");
            RequireApprox(enemy.projectileSpeed, GameBalance.LancerProjectileSpeed, sceneName + " Lancer projectile speed balance");
            if (enemy.definition == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Lancer is missing an EnemyDefinition.");
            }

            RequireEqual((int)enemy.definition.attackStyle, (int)EnemyAttackStyle.Ranged, sceneName + " Lancer definition style");
            RequireEqual(enemy.definition.maxHealth, GameBalance.LancerHealth, sceneName + " Lancer definition health");
            RequireApprox(enemy.definition.fireCooldown, GameBalance.LancerFireCooldown, sceneName + " Lancer definition cooldown");
            RequireEqual(enemy.definition.projectileDamage, GameBalance.LancerProjectileDamage, sceneName + " Lancer definition projectile damage");
            RequireMachineMotion(enemy.gameObject, sceneName + " Lancer machine motion");
            RequireLancerFireTell(enemy.gameObject, sceneName + " Lancer fire tell");
        }

        BulwarkEnemyController[] bulwarks = UnityEngine.Object.FindObjectsByType<BulwarkEnemyController>(FindObjectsSortMode.None);
        foreach (BulwarkEnemyController enemy in bulwarks)
        {
            if (enemy.GetComponent<CharacterController>() == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Bulwark missing CharacterController.");
            }

            RequireEqual(enemy.maxHealth, GameBalance.BulwarkHealth, sceneName + " Bulwark health balance");
            RequireApprox(enemy.detectionRange, GameBalance.BulwarkDetectionRange, sceneName + " Bulwark detection balance");
            RequireApprox(enemy.moveSpeed, GameBalance.BulwarkMoveSpeed, sceneName + " Bulwark speed balance");
            RequireApprox(enemy.attackRange, GameBalance.BulwarkAttackRange, sceneName + " Bulwark attack range balance");
            RequireEqual(enemy.attackDamage, GameBalance.BulwarkAttackDamage, sceneName + " Bulwark damage balance");
            RequireApprox(enemy.attackCooldown, GameBalance.BulwarkAttackCooldown, sceneName + " Bulwark cooldown balance");
            RequireApprox(enemy.attackWindup, GameBalance.BulwarkAttackWindup, sceneName + " Bulwark windup balance");
            if (enemy.definition == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Bulwark is missing an EnemyDefinition.");
            }

            RequireEqual((int)enemy.definition.attackStyle, (int)EnemyAttackStyle.Heavy, sceneName + " Bulwark definition style");
            RequireEqual(enemy.definition.maxHealth, GameBalance.BulwarkHealth, sceneName + " Bulwark definition health");
            RequireApprox(enemy.definition.moveSpeed, GameBalance.BulwarkMoveSpeed, sceneName + " Bulwark definition speed");
            RequireEqual(enemy.definition.attackDamage, GameBalance.BulwarkAttackDamage, sceneName + " Bulwark definition damage");
            RequireMachineMotion(enemy.gameObject, sceneName + " Bulwark machine motion");
            RequireBulwarkAttackTell(enemy.gameObject, sceneName + " Bulwark attack tell");
        }

        BellowsNodeController[] bellowsNodes = UnityEngine.Object.FindObjectsByType<BellowsNodeController>(FindObjectsSortMode.None);
        foreach (BellowsNodeController enemy in bellowsNodes)
        {
            if (enemy.GetComponent<CharacterController>() == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Bellows Node missing CharacterController.");
            }

            RequireEqual(enemy.maxHealth, GameBalance.BellowsNodeHealth, sceneName + " Bellows Node health balance");
            RequireApprox(enemy.detectionRange, GameBalance.BellowsNodeDetectionRange, sceneName + " Bellows Node detection balance");
            RequireApprox(enemy.pulseRange, GameBalance.BellowsNodePulseRange, sceneName + " Bellows Node pulse range balance");
            RequireEqual(enemy.pulseDamage, GameBalance.BellowsNodePulseDamage, sceneName + " Bellows Node pulse damage balance");
            RequireApprox(enemy.pulseCooldown, GameBalance.BellowsNodePulseCooldown, sceneName + " Bellows Node pulse cooldown balance");
            RequireApprox(enemy.pulseWindup, GameBalance.BellowsNodePulseWindup, sceneName + " Bellows Node pulse windup balance");
            RequireApprox(enemy.boostDuration, GameBalance.BellowsNodeBoostDuration, sceneName + " Bellows Node boost duration balance");
            RequireApprox(enemy.boostMultiplier, GameBalance.BellowsNodeBoostMultiplier, sceneName + " Bellows Node boost multiplier balance");
            if (enemy.definition == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Bellows Node is missing an EnemyDefinition.");
            }

            RequireEqual((int)enemy.definition.attackStyle, (int)EnemyAttackStyle.Support, sceneName + " Bellows Node definition style");
            RequireEqual(enemy.definition.maxHealth, GameBalance.BellowsNodeHealth, sceneName + " Bellows Node definition health");
            RequireApprox(enemy.definition.attackRange, GameBalance.BellowsNodePulseRange, sceneName + " Bellows Node definition pulse range");
            RequireEqual(enemy.definition.attackDamage, GameBalance.BellowsNodePulseDamage, sceneName + " Bellows Node definition pulse damage");
            RequireMachineMotion(enemy.gameObject, sceneName + " Bellows Node machine motion");
        }

        GovernorWardenController[] wardens = UnityEngine.Object.FindObjectsByType<GovernorWardenController>(FindObjectsSortMode.None);
        foreach (GovernorWardenController enemy in wardens)
        {
            if (enemy.GetComponent<CharacterController>() == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Governor Warden missing CharacterController.");
            }

            RequireEqual(enemy.maxHealth, GameBalance.GovernorWardenHealth, sceneName + " Governor Warden health balance");
            RequireApprox(enemy.detectionRange, GameBalance.GovernorWardenDetectionRange, sceneName + " Governor Warden detection balance");
            RequireApprox(enemy.moveSpeed, GameBalance.GovernorWardenMoveSpeed, sceneName + " Governor Warden speed balance");
            RequireApprox(enemy.stompRange, GameBalance.GovernorWardenStompRange, sceneName + " Governor Warden stomp range balance");
            RequireEqual(enemy.stompDamage, GameBalance.GovernorWardenStompDamage, sceneName + " Governor Warden stomp damage balance");
            RequireApprox(enemy.stompCooldown, GameBalance.GovernorWardenStompCooldown, sceneName + " Governor Warden stomp cooldown balance");
            RequireApprox(enemy.stompWindup, GameBalance.GovernorWardenStompWindup, sceneName + " Governor Warden stomp windup balance");
            RequireApprox(enemy.fireRange, GameBalance.GovernorWardenFireRange, sceneName + " Governor Warden fire range balance");
            RequireApprox(enemy.fireCooldown, GameBalance.GovernorWardenFireCooldown, sceneName + " Governor Warden fire cooldown balance");
            RequireApprox(enemy.fireWindup, GameBalance.GovernorWardenFireWindup, sceneName + " Governor Warden fire windup balance");
            RequireEqual(enemy.projectileDamage, GameBalance.GovernorWardenProjectileDamage, sceneName + " Governor Warden projectile damage balance");
            RequireApprox(enemy.projectileSpeed, GameBalance.GovernorWardenProjectileSpeed, sceneName + " Governor Warden projectile speed balance");
            if (enemy.definition == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Governor Warden is missing an EnemyDefinition.");
            }

            RequireEqual((int)enemy.definition.attackStyle, (int)EnemyAttackStyle.Boss, sceneName + " Governor Warden definition style");
            RequireEqual(enemy.definition.maxHealth, GameBalance.GovernorWardenHealth, sceneName + " Governor Warden definition health");
            RequireApprox(enemy.definition.moveSpeed, GameBalance.GovernorWardenMoveSpeed, sceneName + " Governor Warden definition speed");
            RequireEqual(enemy.definition.attackDamage, GameBalance.GovernorWardenStompDamage, sceneName + " Governor Warden definition damage");
            RequireEqual(enemy.definition.projectileDamage, GameBalance.GovernorWardenProjectileDamage, sceneName + " Governor Warden definition projectile damage");
            RequireMachineMotion(enemy.gameObject, sceneName + " Governor Warden machine motion");
        }
    }

    private static void RequireMachineMotion(GameObject enemy, string label)
    {
        MachineMotionVfx motion = enemy.GetComponent<MachineMotionVfx>();
        if (motion == null || !motion.IsConfigured)
        {
            throw new InvalidOperationException("Level validation failed: " + label + " is missing configured MachineMotionVfx.");
        }
    }

    private static void RequireScrapperAttackTell(GameObject enemy, string label)
    {
        ScrapperAttackTellVfx attackTell = enemy.GetComponent<ScrapperAttackTellVfx>();
        if (attackTell == null || !attackTell.IsConfigured)
        {
            throw new InvalidOperationException("Level validation failed: " + label + " is missing configured ScrapperAttackTellVfx.");
        }
    }

    private static void RequireLancerFireTell(GameObject enemy, string label)
    {
        LancerFireTellVfx fireTell = enemy.GetComponent<LancerFireTellVfx>();
        if (fireTell == null || !fireTell.IsConfigured)
        {
            throw new InvalidOperationException("Level validation failed: " + label + " is missing configured LancerFireTellVfx.");
        }
    }

    private static void RequireBulwarkAttackTell(GameObject enemy, string label)
    {
        BulwarkAttackTellVfx attackTell = enemy.GetComponent<BulwarkAttackTellVfx>();
        if (attackTell == null || !attackTell.IsConfigured)
        {
            throw new InvalidOperationException("Level validation failed: " + label + " is missing configured BulwarkAttackTellVfx.");
        }
    }

    private static void ValidateHazards(string sceneName)
    {
        SteamHazard[] hazards = UnityEngine.Object.FindObjectsByType<SteamHazard>(FindObjectsSortMode.None);
        if ((sceneName == "Level03" || sceneName == "Level04" || sceneName == "Level05") && hazards.Length == 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " is missing SteamHazard.");
        }

        foreach (SteamHazard hazard in hazards)
        {
            RequireTrigger(hazard.gameObject, sceneName + " steam hazard trigger " + hazard.name);
            RequireEqual(hazard.damagePerTick, GameBalance.SteamHazardDamage, sceneName + " steam hazard damage " + hazard.name);
            RequireApprox(hazard.tickInterval, GameBalance.SteamHazardTickInterval, sceneName + " steam hazard tick interval " + hazard.name);
            SteamHazardVfx steamVfx = hazard.GetComponent<SteamHazardVfx>();
            if (steamVfx == null || steamVfx.VisiblePuffCount < 2)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " steam hazard is missing animated steam puffs " + hazard.name);
            }
        }

        FurnaceHeatHazard[] furnaceHazards = UnityEngine.Object.FindObjectsByType<FurnaceHeatHazard>(FindObjectsSortMode.None);
        if ((sceneName == "Level04" || sceneName == "Level05") && furnaceHazards.Length == 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " is missing FurnaceHeatHazard.");
        }

        foreach (FurnaceHeatHazard hazard in furnaceHazards)
        {
            RequireTrigger(hazard.gameObject, sceneName + " furnace heat hazard trigger " + hazard.name);
            RequireEqual(hazard.damagePerTick, GameBalance.FurnaceHeatHazardDamage, sceneName + " furnace heat hazard damage " + hazard.name);
            RequireApprox(hazard.tickInterval, GameBalance.FurnaceHeatHazardTickInterval, sceneName + " furnace heat hazard tick interval " + hazard.name);
            RequireApprox(hazard.warningDuration, GameBalance.FurnaceHeatHazardWarningDuration, sceneName + " furnace heat warning duration " + hazard.name);
            RequireApprox(hazard.activeDuration, GameBalance.FurnaceHeatHazardActiveDuration, sceneName + " furnace heat active duration " + hazard.name);
            RequireApprox(hazard.cooldownDuration, GameBalance.FurnaceHeatHazardCooldownDuration, sceneName + " furnace heat cooldown duration " + hazard.name);
            if (hazard.warningSignal == null || hazard.activeSignal == null || hazard.safeSignal == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " furnace heat hazard is missing phase signals.");
            }

            FurnaceHeatHazardVfx furnaceVfx = hazard.GetComponent<FurnaceHeatHazardVfx>();
            if (furnaceVfx == null || !furnaceVfx.HasPhaseSignals || furnaceVfx.VisibleHeatPieceCount < 2)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " furnace heat hazard is missing animated heat VFX " + hazard.name);
            }
        }
    }

    private static void ValidateSecrets(string sceneName)
    {
        SecretArea[] secrets = UnityEngine.Object.FindObjectsByType<SecretArea>(FindObjectsSortMode.None);
        if ((sceneName == "Level01" || sceneName == "Level02" || sceneName == "Level04") && secrets.Length == 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " is missing SecretArea.");
        }

        foreach (SecretArea secret in secrets)
        {
            RequireTrigger(secret.gameObject, sceneName + " secret trigger " + secret.name);
            if (string.IsNullOrWhiteSpace(secret.secretId) || string.IsNullOrWhiteSpace(secret.discoveryMessage))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " secret is missing id or discovery message.");
            }
        }
    }

    private static void ValidateMachineryMotion(string sceneName)
    {
        SteamworksSpinner[] spinners = UnityEngine.Object.FindObjectsByType<SteamworksSpinner>(FindObjectsSortMode.None);
        if (spinners.Length == 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " is missing animated machinery spinners.");
        }

        foreach (SteamworksSpinner spinner in spinners)
        {
            if (spinner.localAxis.sqrMagnitude <= 0.001f || Mathf.Approximately(spinner.degreesPerSecond, 0f))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " has an inert machinery spinner.");
            }
        }
    }

    private static void ValidateBalanceValues(string sceneName, PlayerController playerController, PlayerInventory playerInventory, WeaponController weaponController)
    {
        RequireApprox(playerController.moveSpeed, GameBalance.PlayerMoveSpeed, sceneName + " player speed balance");
        RequireApprox(playerController.moveAcceleration, GameBalance.PlayerMoveAcceleration, sceneName + " player acceleration balance");
        RequireApprox(playerController.moveDeceleration, GameBalance.PlayerMoveDeceleration, sceneName + " player deceleration balance");
        RequireApprox(playerController.gravity, GameBalance.PlayerGravity, sceneName + " player gravity balance");
        RequireApprox(playerController.groundStickVelocity, GameBalance.PlayerGroundStickVelocity, sceneName + " player ground stick balance");
        RequireApprox(playerController.pitchLimit, GameBalance.PlayerPitchLimit, sceneName + " player pitch limit balance");
        RequireEqual(playerInventory.startingAmmo, GameBalance.StartingAmmo, sceneName + " starting ammo balance");
        RequireEqual(weaponController.damage, GameBalance.PressurePistolDamage, sceneName + " pistol damage balance");
        RequireEqual(weaponController.ammoCost, GameBalance.PressurePistolAmmoCost, sceneName + " pistol ammo-cost balance");
        RequireEqual(weaponController.pelletCount, GameBalance.PressurePistolPelletCount, sceneName + " pistol pellet-count balance");
        RequireApprox(weaponController.fireCooldown, GameBalance.PressurePistolCooldown, sceneName + " pistol cooldown balance");
        RequireApprox(weaponController.range, GameBalance.PressurePistolRange, sceneName + " pistol range balance");
        RequireApprox(weaponController.spread, GameBalance.PressurePistolSpread, sceneName + " pistol spread balance");
        RequireEqual(weaponController.secondaryDamage, GameBalance.PressureBurstDamage, sceneName + " pressure burst damage balance");
        RequireEqual(weaponController.secondaryPelletCount, GameBalance.PressureBurstPelletCount, sceneName + " pressure burst pellet balance");
        RequireEqual(weaponController.secondaryAmmoCost, GameBalance.PressureBurstAmmoCost, sceneName + " pressure burst ammo-cost balance");
        RequireApprox(weaponController.secondaryCooldown, GameBalance.PressureBurstCooldown, sceneName + " pressure burst cooldown balance");
        RequireApprox(weaponController.secondaryRange, GameBalance.PressureBurstRange, sceneName + " pressure burst range balance");
        RequireApprox(weaponController.secondarySpread, GameBalance.PressureBurstSpread, sceneName + " pressure burst spread balance");
        if (weaponController.definition == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " WeaponController is missing a WeaponDefinition.");
        }

        RequireEqual(weaponController.definition.damage, GameBalance.PressurePistolDamage, sceneName + " weapon definition damage");
        RequireEqual(weaponController.definition.ammoCost, GameBalance.PressurePistolAmmoCost, sceneName + " weapon definition ammo cost");
        RequireEqual(weaponController.definition.pelletCount, GameBalance.PressurePistolPelletCount, sceneName + " weapon definition pellet count");
        RequireApprox(weaponController.definition.fireCooldown, GameBalance.PressurePistolCooldown, sceneName + " weapon definition cooldown");
        RequireApprox(weaponController.definition.range, GameBalance.PressurePistolRange, sceneName + " weapon definition range");
        RequireApprox(weaponController.definition.spread, GameBalance.PressurePistolSpread, sceneName + " weapon definition spread");
        RequireEqual(weaponController.definition.secondaryDamage, GameBalance.PressureBurstDamage, sceneName + " weapon definition secondary damage");
        RequireEqual(weaponController.definition.secondaryPelletCount, GameBalance.PressureBurstPelletCount, sceneName + " weapon definition secondary pellet count");
        RequireEqual(weaponController.definition.secondaryAmmoCost, GameBalance.PressureBurstAmmoCost, sceneName + " weapon definition secondary ammo cost");
        RequireApprox(weaponController.definition.secondaryCooldown, GameBalance.PressureBurstCooldown, sceneName + " weapon definition secondary cooldown");
        RequireApprox(weaponController.definition.secondaryRange, GameBalance.PressureBurstRange, sceneName + " weapon definition secondary range");
        RequireApprox(weaponController.definition.secondarySpread, GameBalance.PressureBurstSpread, sceneName + " weapon definition secondary spread");
        if (weaponController.steamScattergunDefinition == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " WeaponController is missing a Steam Scattergun definition.");
        }

        RequireEqual(weaponController.steamScattergunDefinition.damage, GameBalance.SteamScattergunDamage, sceneName + " scattergun definition damage");
        RequireEqual(weaponController.steamScattergunDefinition.ammoCost, GameBalance.SteamScattergunAmmoCost, sceneName + " scattergun definition ammo cost");
        RequireEqual(weaponController.steamScattergunDefinition.pelletCount, GameBalance.SteamScattergunPelletCount, sceneName + " scattergun definition pellet count");
        RequireApprox(weaponController.steamScattergunDefinition.fireCooldown, GameBalance.SteamScattergunCooldown, sceneName + " scattergun definition cooldown");
        RequireApprox(weaponController.steamScattergunDefinition.range, GameBalance.SteamScattergunRange, sceneName + " scattergun definition range");
        RequireApprox(weaponController.steamScattergunDefinition.spread, GameBalance.SteamScattergunSpread, sceneName + " scattergun definition spread");
        RequireEqual(weaponController.steamScattergunDefinition.secondaryDamage, GameBalance.SteamScattergunSlugDamage, sceneName + " scattergun slug definition damage");
        RequireEqual(weaponController.steamScattergunDefinition.secondaryAmmoCost, GameBalance.SteamScattergunSlugAmmoCost, sceneName + " scattergun slug definition ammo cost");
        RequireApprox(weaponController.steamScattergunDefinition.secondaryCooldown, GameBalance.SteamScattergunSlugCooldown, sceneName + " scattergun slug definition cooldown");
    }

    private static void ValidatePlatformQualityProfile(string sceneName, RuntimePerformanceProfile performanceProfile)
    {
        PlatformQualityProfile profile = performanceProfile.activeProfile;
        if (profile == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " RuntimePerformanceProfile is missing an active PlatformQualityProfile.");
        }

        RequireEqual((int)profile.target, (int)PlatformQualityTarget.WindowsMidLow, sceneName + " platform quality target");
        RequireEqual(profile.targetFrameRate, RuntimePerformanceProfile.WindowsTargetFrameRate, sceneName + " platform target frame rate");
        RequireEqual(profile.vSyncCount, RuntimePerformanceProfile.WindowsVSyncCount, sceneName + " platform vSync");
        RequireEqual(profile.pixelLightCount, RuntimePerformanceProfile.WindowsPixelLightCount, sceneName + " platform pixel light count");
        RequireEqual(profile.antiAliasing, RuntimePerformanceProfile.WindowsAntiAliasing, sceneName + " platform anti-aliasing");
        RequireApprox(profile.shadowDistance, RuntimePerformanceProfile.WindowsShadowDistance, sceneName + " platform shadow distance");
        RequireApprox(profile.lodBias, RuntimePerformanceProfile.WindowsLodBias, sceneName + " platform LOD bias");
        if (profile.allowCameraMsaa || profile.allowDynamicResolution)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " Windows profile must disable camera MSAA and dynamic resolution.");
        }
    }

    private static void ValidateSteamworksAudio(string sceneName, SteamworksAudio audio)
    {
        if (!audio.ambienceEnabled || audio.ambienceVolume <= 0f)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " SteamworksAudio ambience is not configured.");
        }

        if (!audio.preferAuthoredClips || audio.authoredAmbienceLoop == null || !audio.authoredAmbienceLoop.name.StartsWith("AUDV1_", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " SteamworksAudio is not using staged AudioV1 ambience.");
        }

        int expectedCueCount = Enum.GetValues(typeof(SteamworksAudioCue)).Length;
        if (audio.AuthoredCueCount < expectedCueCount)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " SteamworksAudio is missing staged AudioV1 cue bindings.");
        }

        foreach (SteamworksAudioCue cue in Enum.GetValues(typeof(SteamworksAudioCue)))
        {
            if (!audio.HasAuthoredClip(cue))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " SteamworksAudio missing AudioV1 binding for " + cue + ".");
            }

            if (!audio.HasMixBinding(cue))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " SteamworksAudio missing mix binding for " + cue + ".");
            }

            float volumeMultiplier = audio.GetCueVolumeMultiplier(cue);
            if (volumeMultiplier < 0.45f || volumeMultiplier > 1f)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " SteamworksAudio mix binding for " + cue + " is outside the v0.1.8 mix range.");
            }
        }

        if (audio.ambienceVolume < 0.18f || audio.ambienceVolume > 0.32f)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " SteamworksAudio ambience mix volume is outside the v0.1.8 range.");
        }

        if (!audio.IsSpatialMixCue(SteamworksAudioCue.EnemyAttackTell) || !audio.IsSpatialMixCue(SteamworksAudioCue.LancerFireTell) || !audio.IsSpatialMixCue(SteamworksAudioCue.BulwarkAttackTell) || !audio.IsSpatialMixCue(SteamworksAudioCue.BellowsNodePulse))
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " SteamworksAudio priority enemy/hazard tells must be marked as spatial mix cues.");
        }
    }

    private static void ValidateGameplayFeedback(string sceneName, GameplayFeedbackController feedback)
    {
        if (!feedback.HasV0135Coverage || feedback.SupportedEventTypeCount < 15 || feedback.pulseScaleMultiplier <= 0f)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " GameplayFeedbackController is missing v0.1.35 feedback coverage.");
        }
    }

    private static void ValidateStartMessage(string sceneName, GameStateController gameState)
    {
        string expectedMessage = sceneName == "Level01"
            ? "Find the gear key. Open the pressure gate."
            : sceneName == "Level02"
                ? "Route pipe pressure. Ride the lift to the Boilerheart."
                : sceneName == "Level03"
                    ? "Vent the Boilerheart pressure valve. Ride the foundry lift."
                    : sceneName == "Level04"
                        ? "Cross the Furnace Foundry. Reach the emergency hoist."
                        : sceneName == "Level05"
                            ? "Breach the Governor Core. Reach the master override hoist."
                            : string.Empty;

        if (string.IsNullOrWhiteSpace(expectedMessage))
        {
            return;
        }

        if (gameState.startMessage != expectedMessage)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " start message is not scene-specific.");
        }
    }

    private static T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            throw new InvalidOperationException("Level validation failed: missing " + label + ".");
        }

        return value;
    }

    private static GameObject RequireNamed(string objectName, string label)
    {
        GameObject value = GameObject.Find(objectName);
        if (value == null)
        {
            throw new InvalidOperationException("Level validation failed: missing " + label + " (" + objectName + ").");
        }

        return value;
    }

    private static void ValidateServiceLiftVisuals(GameObject lift, string label)
    {
        RequireNamed(lift.name + " Brass Platform Deck", label + " platform deck visual");
        RequireNamed(lift.name + " Overhead Pulley Gear", label + " pulley gear visual");
        RequireNamed(lift.name + " Brass Call Box", label + " call box visual");
        RequireNamed(lift.name + " Green Signal Lamp Left", label + " signal lamp visual");
    }

    private static void ValidateWeaponVisuals(string sceneName)
    {
        RequireNamed("Pressure Pistol Viewmodel", sceneName + " pressure pistol viewmodel");
        RequireNamed("Pressure Pistol Pressure Tank", sceneName + " pressure pistol pressure tank visual");
        RequireNamed("Pressure Pistol Muzzle Crown", sceneName + " pressure pistol muzzle crown visual");
        RequireNamed("Pressure Pistol Steam Vent Chimney", sceneName + " pressure pistol steam vent visual");
        RequireNamed("Pressure Pistol Pressure Relief Nozzle", sceneName + " pressure pistol pressure relief nozzle visual");
        RequireNamed("Pressure Pistol Pressure Dump Lever", sceneName + " pressure pistol pressure dump lever visual");
        ValidatePressureGaugePrototype(sceneName, "Pressure Pistol Prototype Pressure Gauge", "viewmodel");
        ValidatePressureCoilPrototype(sceneName, "Pressure Pistol Prototype Copper Coil Pack", "viewmodel");
        RequireNamed("Pressure Pistol Valve Wheel", sceneName + " pressure pistol valve wheel visual");
        RequireNamed("Pressure Pistol Front Sight", sceneName + " pressure pistol front sight visual");
        RequireNamed("Steam Scattergun Viewmodel", sceneName + " Steam Scattergun viewmodel");
        RequireNamed("Steam Scattergun Brass Receiver", sceneName + " Steam Scattergun receiver visual");
        RequireNamed("Steam Scattergun Barrel 0", sceneName + " Steam Scattergun barrel visual");
        RequireNamed("Steam Scattergun Pressure Drum", sceneName + " Steam Scattergun pressure drum visual");
        RequireNamed("Steam Scattergun Pump Handle", sceneName + " Steam Scattergun pump handle visual");
    }

    private static void ValidatePressureGaugePrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " pressure gauge prototype root");
        PressureGaugePrototype prototype = root.GetComponent<PressureGaugePrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure gauge prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.11" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure gauge prototype metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.tickRoot.childCount < 16 || prototype.rivetRoot.childCount < 12)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure gauge prototype does not have the required tick/rivet count (" + objectName + ").");
        }

        RequireNamed(objectName + " Aged Brass Bezel", sceneName + " pressure gauge brass bezel");
        RequireNamed(objectName + " Blackened Iron Backplate", sceneName + " pressure gauge iron backplate");
        RequireNamed(objectName + " Cream Enamel Face", sceneName + " pressure gauge enamel face");
        RequireNamed(objectName + " Amber Glass Lens", sceneName + " pressure gauge glass lens");
        RequireNamed(objectName + " Red Pressure Warning Band", sceneName + " pressure gauge warning band");
        RequireNamed(objectName + " Needle Pivot", sceneName + " pressure gauge needle pivot");
        RequireNamed(objectName + " Tick 00", sceneName + " pressure gauge tick mark");
        RequireNamed(objectName + " Bezel Rivet 00", sceneName + " pressure gauge bezel rivet");

        RequireRendererMaterial(prototype.bezelRenderer, sceneName + " pressure gauge bezel", "Brass");
        RequireRendererMaterial(prototype.backplateRenderer, sceneName + " pressure gauge backplate", "Iron");
        RequireRendererMaterial(prototype.faceRenderer, sceneName + " pressure gauge face", "CreamGaugeFace");
        RequireRendererMaterial(prototype.glassRenderer, sceneName + " pressure gauge glass", "Glass");
        RequireRendererMaterial(prototype.warningBandRenderer, sceneName + " pressure gauge warning band", "PressureWarning");
    }

    private static void ValidatePressureCoilPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " pressure coil prototype root");
        PressureCoilPrototype prototype = root.GetComponent<PressureCoilPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure coil prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.14" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure coil prototype metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.coilTurnRoot.childCount < 18 || prototype.rivetRoot.childCount < 18 || prototype.pressureLeadRoot.childCount < 4)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure coil prototype does not have the required coil/rivet/lead detail count (" + objectName + ").");
        }

        RequireNamed(objectName + " Blackened Iron Backing Plate", sceneName + " pressure coil backing plate");
        RequireNamed(objectName + " Aged Brass Upper Rail", sceneName + " pressure coil upper rail");
        RequireNamed(objectName + " Aged Brass Lower Rail", sceneName + " pressure coil lower rail");
        RequireNamed(objectName + " Dull Red Ceramic Heat Core", sceneName + " pressure coil heat core");
        RequireNamed(objectName + " Upper Copper Manifold", sceneName + " pressure coil upper manifold");
        RequireNamed(objectName + " Lower Copper Manifold", sceneName + " pressure coil lower manifold");
        RequireNamed(objectName + " Oxidized Copper Coil Turn 00", sceneName + " pressure coil visible turn");
        RequireNamed(objectName + " Upper Slotted Rivet 00", sceneName + " pressure coil rivet");
        RequireNamed(objectName + " Left Braided Pressure Lead", sceneName + " pressure coil pressure lead");

        RequireRendererMaterial(prototype.backingPlateRenderer, sceneName + " pressure coil backing plate", "Iron");
        RequireRendererMaterial(prototype.upperRailRenderer, sceneName + " pressure coil upper rail", "Brass");
        RequireRendererMaterial(prototype.lowerRailRenderer, sceneName + " pressure coil lower rail", "Brass");
        RequireRendererMaterial(prototype.heatCoreRenderer, sceneName + " pressure coil heat core", "PressureWarning");
    }

    private static void ValidateWallPipeGaugeClusterPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " wall pipe gauge cluster prototype root");
        WallPipeGaugeClusterPrototype prototype = root.GetComponent<WallPipeGaugeClusterPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " wall pipe gauge cluster prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.17" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " wall pipe gauge cluster metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.pipeRoot.childCount < 5 || prototype.gaugeRoot.childCount < 2 || prototype.valveRoot.childCount < 4 || prototype.rivetRoot.childCount < 14)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " wall pipe gauge cluster does not have the required pipe/gauge/valve/rivet detail counts (" + objectName + ").");
        }

        RequireNamed(objectName + " Blackened Iron Mounting Plate", sceneName + " wall pipe gauge cluster mounting plate");
        RequireNamed(objectName + " Aged Brass Header Rail", sceneName + " wall pipe gauge cluster header rail");
        RequireNamed(objectName + " Vertical Copper Feed Pipe 00", sceneName + " wall pipe gauge cluster copper feed pipe");
        RequireNamed(objectName + " Vertical Blackened Return Pipe 01", sceneName + " wall pipe gauge cluster return pipe");
        RequireNamed(objectName + " Upper Cross Pressure Pipe 03", sceneName + " wall pipe gauge cluster cross pipe");
        RequireNamed(objectName + " Upper Pressure Gauge Cream Enamel Face", sceneName + " wall pipe gauge cluster upper gauge face");
        RequireNamed(objectName + " Lower Pressure Gauge Cream Enamel Face", sceneName + " wall pipe gauge cluster lower gauge face");
        RequireNamed(objectName + " Red Brass Valve Wheel", sceneName + " wall pipe gauge cluster valve wheel");
        RequireNamed(objectName + " Top Slotted Rivet 00", sceneName + " wall pipe gauge cluster rivet");

        RequireRendererMaterial(prototype.backplateRenderer, sceneName + " wall pipe gauge cluster backplate", "Iron");
        RequireRendererMaterial(prototype.primaryPipeRenderer, sceneName + " wall pipe gauge cluster primary pipe", "Brass");
        RequireRendererMaterial(prototype.secondaryPipeRenderer, sceneName + " wall pipe gauge cluster return pipe", "Iron");
        RequireRendererMaterial(prototype.gaugeFaceRenderer, sceneName + " wall pipe gauge cluster gauge face", "CreamGaugeFace");
        RequireRendererMaterial(prototype.valveWheelRenderer, sceneName + " wall pipe gauge cluster valve wheel", "PressureWarning");
    }

    private static void ValidateBoilerControlConsolePrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " boiler control console prototype root");
        BoilerControlConsolePrototype prototype = root.GetComponent<BoilerControlConsolePrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " boiler control console prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.19" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " boiler control console metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.leverRoot.childCount < 6 || prototype.gaugeRoot.childCount < 2 || prototype.lampRoot.childCount < 6 || prototype.rivetRoot.childCount < 12 || prototype.pipeRoot.childCount < 3)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " boiler control console does not have the required lever/gauge/lamp/rivet/pipe detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " boiler control console must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Blackened Iron Pedestal Base", sceneName + " boiler control console base");
        RequireNamed(objectName + " Angled Iron Control Panel", sceneName + " boiler control console angled panel");
        RequireNamed(objectName + " Aged Brass Front Control Rail", sceneName + " boiler control console brass rail");
        RequireNamed(objectName + " Red Brass Lever Handle 00", sceneName + " boiler control console lever handle");
        RequireNamed(objectName + " Left Boiler Pressure Gauge Cream Enamel Face", sceneName + " boiler control console left gauge face");
        RequireNamed(objectName + " Right Boiler Pressure Gauge Cream Enamel Face", sceneName + " boiler control console right gauge face");
        RequireNamed(objectName + " Amber Indicator Lamp 00", sceneName + " boiler control console lamp");
        RequireNamed(objectName + " Copper Pressure Pipe 00", sceneName + " boiler control console pressure pipe");
        RequireNamed(objectName + " Side Brass Valve Wheel", sceneName + " boiler control console side valve wheel");
        RequireNamed(objectName + " Front Slotted Rivet 00", sceneName + " boiler control console rivet");

        RequireRendererMaterial(prototype.baseRenderer, sceneName + " boiler control console base", "Iron");
        RequireRendererMaterial(prototype.panelRenderer, sceneName + " boiler control console panel", "Iron");
        RequireRendererMaterial(prototype.brassRailRenderer, sceneName + " boiler control console rail", "Brass");
        RequireRendererMaterial(prototype.gaugeFaceRenderer, sceneName + " boiler control console gauge face", "CreamGaugeFace");
        RequireRendererMaterial(prototype.leverHandleRenderer, sceneName + " boiler control console lever handle", "PressureWarning");
        RequireRendererMaterial(prototype.lampRenderer, sceneName + " boiler control console lamp", "PressureWarning");
        RequireRendererMaterial(prototype.pipeRenderer, sceneName + " boiler control console pipe", "Brass");
    }

    private static void ValidateRivetedPressureDoorFramePrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " riveted pressure door frame prototype root");
        RivetedPressureDoorFramePrototype prototype = root.GetComponent<RivetedPressureDoorFramePrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " riveted pressure door frame prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.20" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " riveted pressure door frame metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.archRoot.childCount < 5 || prototype.brassRibRoot.childCount < 4 || prototype.gearRoot.childCount < 10 || prototype.cylinderRoot.childCount < 6 || prototype.gaugeRoot.childCount < 1 || prototype.lampRoot.childCount < 6 || prototype.rivetRoot.childCount < 16 || prototype.crossBraceRoot.childCount < 2)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " riveted pressure door frame does not have the required arch/rib/gear/cylinder/gauge/lamp/rivet/brace detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " riveted pressure door frame must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Blackened Iron Left Door Column", sceneName + " riveted pressure door frame left column");
        RequireNamed(objectName + " Blackened Iron Right Door Column", sceneName + " riveted pressure door frame right column");
        RequireNamed(objectName + " Aged Brass Upper Arch Rib", sceneName + " riveted pressure door frame arch rib");
        RequireNamed(objectName + " Central Brass Gear Hub", sceneName + " riveted pressure door frame gear hub");
        RequireNamed(objectName + " Left Pressure Cylinder", sceneName + " riveted pressure door frame left cylinder");
        RequireNamed(objectName + " Right Pressure Cylinder", sceneName + " riveted pressure door frame right cylinder");
        RequireNamed(objectName + " Cream Pressure Gauge Face", sceneName + " riveted pressure door frame gauge face");
        RequireNamed(objectName + " Amber Warning Lamp 00", sceneName + " riveted pressure door frame lamp");
        RequireNamed(objectName + " Amber Warning Lamp 01", sceneName + " riveted pressure door frame second lamp");
        RequireNamed(objectName + " Cross Brace A", sceneName + " riveted pressure door frame cross brace");
        RequireNamed(objectName + " Face Rivet 00", sceneName + " riveted pressure door frame rivet");

        RequireRendererMaterial(prototype.archRenderer, sceneName + " riveted pressure door frame arch", "Iron");
        RequireRendererMaterial(prototype.brassRibRenderer, sceneName + " riveted pressure door frame brass rib", "Brass");
        RequireRendererMaterial(prototype.gearHubRenderer, sceneName + " riveted pressure door frame gear hub", "Brass");
        RequireRendererMaterial(prototype.pressureCylinderRenderer, sceneName + " riveted pressure door frame pressure cylinder", "Brass");
        RequireRendererMaterial(prototype.gaugeFaceRenderer, sceneName + " riveted pressure door frame gauge face", "CreamGaugeFace");
        RequireRendererMaterial(prototype.lampRenderer, sceneName + " riveted pressure door frame warning lamp", "PressureWarning");
        RequireRendererMaterial(prototype.crossBraceRenderer, sceneName + " riveted pressure door frame cross brace", "Iron");
    }

    private static void ValidateCagedGaslightPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " caged gaslight prototype root");
        CagedGaslightPrototype prototype = root.GetComponent<CagedGaslightPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " caged gaslight prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.21" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " caged gaslight metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.mountRoot.childCount < 2 || prototype.capRoot.childCount < 2 || prototype.globeRoot.childCount < 1 || prototype.cageRoot.childCount < 4 || prototype.pipeFeedRoot.childCount < 2 || prototype.rivetRoot.childCount < 6 || prototype.lightRoot.childCount < 2)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " caged gaslight does not have the required mount/cap/globe/cage/pipe/rivet/light detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " caged gaslight must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        if (!prototype.warmPointLight.enabled || prototype.warmPointLight.type != LightType.Point || prototype.warmPointLight.range < 3f)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " caged gaslight warm point light is missing or too weak (" + objectName + ").");
        }

        RequireNamed(objectName + " Soot Dark Mount Backplate", sceneName + " caged gaslight mount backplate");
        RequireNamed(objectName + " Blackened Iron Wall Bracket", sceneName + " caged gaslight wall bracket");
        RequireNamed(objectName + " Small Brass Pipe Feed", sceneName + " caged gaslight pipe feed");
        RequireNamed(objectName + " Aged Brass Valve Detail", sceneName + " caged gaslight valve detail");
        RequireNamed(objectName + " Aged Brass Top Cap", sceneName + " caged gaslight top cap");
        RequireNamed(objectName + " Aged Brass Bottom Cap", sceneName + " caged gaslight bottom cap");
        RequireNamed(objectName + " Amber Glass Globe", sceneName + " caged gaslight amber globe");
        RequireNamed(objectName + " Blackened Iron Cage Rib 00", sceneName + " caged gaslight cage rib");
        RequireNamed(objectName + " Dark Rivet 00", sceneName + " caged gaslight rivet");
        RequireNamed(objectName + " Warm Light Core", sceneName + " caged gaslight warm core");
        RequireNamed(objectName + " Warm Point Light", sceneName + " caged gaslight point light");

        RequireRendererMaterial(prototype.backplateRenderer, sceneName + " caged gaslight backplate", "Iron");
        RequireRendererMaterial(prototype.bracketRenderer, sceneName + " caged gaslight bracket", "Iron");
        RequireRendererMaterial(prototype.topCapRenderer, sceneName + " caged gaslight top cap", "Brass");
        RequireRendererMaterial(prototype.bottomCapRenderer, sceneName + " caged gaslight bottom cap", "Brass");
        RequireRendererMaterial(prototype.amberGlassRenderer, sceneName + " caged gaslight amber globe", "FurnaceGlow");
        RequireRendererMaterial(prototype.warmCoreRenderer, sceneName + " caged gaslight warm core", "FurnaceGlow");
        RequireRendererMaterial(prototype.cageRibRenderer, sceneName + " caged gaslight cage rib", "Iron");
        RequireRendererMaterial(prototype.pipeFeedRenderer, sceneName + " caged gaslight pipe feed", "Brass");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " caged gaslight rivet", "Iron");
    }

    private static void ValidatePipeCanopyPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " pipe canopy prototype root");
        PipeCanopyPrototype prototype = root.GetComponent<PipeCanopyPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pipe canopy prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.22" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pipe canopy metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.pipeRoot.childCount < 4 || prototype.collarRoot.childCount < 5 || prototype.couplerRoot.childCount < 2 || prototype.valveRoot.childCount < 1 || prototype.rivetRoot.childCount < 10)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pipe canopy does not have the required pipe/collar/coupler/valve/rivet detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pipe canopy must remain non-blocking overhead route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Pipe Root", sceneName + " pipe canopy pipe root");
        RequireNamed(objectName + " Collar Root", sceneName + " pipe canopy collar root");
        RequireNamed(objectName + " Coupler Root", sceneName + " pipe canopy coupler root");
        RequireNamed(objectName + " Valve Detail Root", sceneName + " pipe canopy valve root");
        RequireNamed(objectName + " Rivet Root", sceneName + " pipe canopy rivet root");
        RequireNamed(objectName + " Aged Brass Pipe 00", sceneName + " pipe canopy aged brass pipe");
        RequireNamed(objectName + " Blackened Iron Collar 00", sceneName + " pipe canopy blackened iron collar");
        RequireNamed(objectName + " Collar Rivet Left 00", sceneName + " pipe canopy collar rivet");
        RequireNamed(objectName + " Aged Brass Coupler 00", sceneName + " pipe canopy coupler");
        RequireNamed(objectName + " Aged Brass Valve Wheel", sceneName + " pipe canopy valve wheel");
        RequireNamed(objectName + " Valve Spoke Horizontal", sceneName + " pipe canopy valve spoke");
        RequireNamed(objectName + " Valve Hub", sceneName + " pipe canopy valve hub");

        RequireRendererMaterial(prototype.pipeRenderer, sceneName + " pipe canopy pipe", "Brass");
        RequireRendererMaterial(prototype.collarRenderer, sceneName + " pipe canopy collar", "Iron");
        RequireRendererMaterial(prototype.couplerRenderer, sceneName + " pipe canopy coupler", "Brass");
        RequireRendererMaterial(prototype.valveRenderer, sceneName + " pipe canopy valve", "Brass");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " pipe canopy rivet", "Brass");
    }

    private static void ValidateRivetBandPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " rivet band prototype root");
        RivetBandPrototype prototype = root.GetComponent<RivetBandPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " rivet band prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.23" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " rivet band metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.railRoot.childCount < 2 || prototype.endCapRoot.childCount < 2 || prototype.rivetRoot.childCount < 8 || prototype.pressurePlateRoot.childCount < 2)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " rivet band does not have the required rail/cap/rivet/plate detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " rivet band must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Rail Root", sceneName + " rivet band rail root");
        RequireNamed(objectName + " End Cap Root", sceneName + " rivet band end cap root");
        RequireNamed(objectName + " Rivet Root", sceneName + " rivet band rivet root");
        RequireNamed(objectName + " Pressure Plate Root", sceneName + " rivet band pressure plate root");
        RequireNamed(objectName + " Blackened Iron Backing Rail", sceneName + " rivet band backing rail");
        RequireNamed(objectName + " Aged Brass Face Rib", sceneName + " rivet band brass face rib");
        RequireNamed(objectName + " Aged Brass End Cap Left", sceneName + " rivet band left end cap");
        RequireNamed(objectName + " Aged Brass End Cap Right", sceneName + " rivet band right end cap");
        RequireNamed(objectName + " Brass Rivet 00", sceneName + " rivet band visible rivet");
        RequireNamed(objectName + " Small Pressure Tag Plate", sceneName + " rivet band pressure tag");
        RequireNamed(objectName + " Pressure Tag Scribe Mark", sceneName + " rivet band pressure tag scribe mark");

        RequireRendererMaterial(prototype.backingRailRenderer, sceneName + " rivet band backing rail", "Iron");
        RequireRendererMaterial(prototype.faceRailRenderer, sceneName + " rivet band face rib", "Brass");
        RequireRendererMaterial(prototype.endCapRenderer, sceneName + " rivet band end cap", "Brass");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " rivet band rivet", "Brass");
        RequireRendererMaterial(prototype.pressurePlateRenderer, sceneName + " rivet band pressure tag", "Brass");
    }

    private static void ValidateWallValveWheelPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " wall valve wheel prototype root");
        WallValveWheelPrototype prototype = root.GetComponent<WallValveWheelPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " wall valve wheel prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.24" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " wall valve wheel metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.backplateRoot.childCount < 2 || prototype.wheelRoot.childCount < 6 || prototype.rivetRoot.childCount < 8 || prototype.labelRoot.childCount < 2)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " wall valve wheel does not have the required plate/wheel/rivet/label detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " wall valve wheel must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Backplate Root", sceneName + " wall valve wheel backplate root");
        RequireNamed(objectName + " Wheel Root", sceneName + " wall valve wheel wheel root");
        RequireNamed(objectName + " Rivet Root", sceneName + " wall valve wheel rivet root");
        RequireNamed(objectName + " Label Root", sceneName + " wall valve wheel label root");
        RequireNamed(objectName + " Blackened Iron Wall Backplate", sceneName + " wall valve wheel backplate");
        RequireNamed(objectName + " Aged Brass Lower Mount Rail", sceneName + " wall valve wheel lower mount rail");
        RequireNamed(objectName + " Aged Brass Valve Wheel", sceneName + " wall valve wheel brass wheel");
        RequireNamed(objectName + " Blackened Iron Wheel Spoke Horizontal", sceneName + " wall valve wheel horizontal spoke");
        RequireNamed(objectName + " Blackened Iron Wheel Spoke Vertical", sceneName + " wall valve wheel vertical spoke");
        RequireNamed(objectName + " Aged Brass Central Spindle Hub", sceneName + " wall valve wheel spindle hub");
        RequireNamed(objectName + " Brass Mount Rivet 00", sceneName + " wall valve wheel rivet");
        RequireNamed(objectName + " Cream Pressure Label Plate", sceneName + " wall valve wheel pressure label");
        RequireNamed(objectName + " Amber Pressure Pointer Mark", sceneName + " wall valve wheel pointer");

        RequireRendererMaterial(prototype.backplateRenderer, sceneName + " wall valve wheel backplate", "Iron");
        RequireRendererMaterial(prototype.wheelRenderer, sceneName + " wall valve wheel wheel", "Brass");
        RequireRendererMaterial(prototype.spokeRenderer, sceneName + " wall valve wheel spoke", "Iron");
        RequireRendererMaterial(prototype.hubRenderer, sceneName + " wall valve wheel hub", "Brass");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " wall valve wheel rivet", "Brass");
        RequireRendererMaterial(prototype.pointerRenderer, sceneName + " wall valve wheel pointer", "Warning");
        RequireRendererMaterial(prototype.labelPlateRenderer, sceneName + " wall valve wheel label", "Gauge");
    }

    private static void ValidateValveWheelConsolePrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " valve wheel console prototype root");
        ValveWheelConsolePrototype prototype = root.GetComponent<ValveWheelConsolePrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " valve wheel console prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.32" || prototype.placementRole != expectedPlacementRole || prototype.gameplayAuthority != "VisualOnlyNoGameplay" || !prototype.HasRequiredParts || root.name.IndexOf(expectedPlacementRole, StringComparison.OrdinalIgnoreCase) < 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " valve wheel console metadata, role, authority, root name, or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.backplateRoot.childCount < 6 || prototype.wheelRoot.childCount < 12 || prototype.gaugeRoot.childCount < 2 || prototype.lampRoot.childCount < 4 || prototype.pipeRoot.childCount < 4 || prototype.rivetRoot.childCount < 14 || prototype.grimeRoot.childCount < 3)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " valve wheel console does not have the required backplate/wheel/gauge/lamp/pipe/rivet/grime detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " valve wheel console must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<NavMeshObstacle>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " valve wheel console must not add NavMeshObstacle components (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Pickup>().Length > 0 || root.GetComponentsInChildren<PlayerInteraction>().Length > 0 || root.GetComponentsInChildren<LevelTransitionTrigger>().Length > 0 || root.GetComponentsInChildren<ExitTrigger>().Length > 0 || root.GetComponentsInChildren<SteamValveObjective>().Length > 0 || root.GetComponentsInChildren<SteamHazard>().Length > 0 || root.GetComponentsInChildren<FurnaceHeatHazard>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " valve wheel console must remain visual dressing with no pickup, interaction, objective, or route-authority components (" + objectName + ").");
        }

        RequireNamed(objectName + " Backplate Root", sceneName + " valve wheel console backplate root");
        RequireNamed(objectName + " Valve Wheel Root", sceneName + " valve wheel console wheel root");
        RequireNamed(objectName + " Gauge Cluster Root", sceneName + " valve wheel console gauge root");
        RequireNamed(objectName + " Pilot Lamp Root", sceneName + " valve wheel console lamp root");
        RequireNamed(objectName + " Pipework Root", sceneName + " valve wheel console pipe root");
        RequireNamed(objectName + " Fastener Root", sceneName + " valve wheel console rivet root");
        RequireNamed(objectName + " Grime Detail Root", sceneName + " valve wheel console grime root");
        RequireNamed(objectName + " Blackened Iron Wall Backplate", sceneName + " valve wheel console backplate");
        RequireNamed(objectName + " Blackened Iron Raised Console Panel", sceneName + " valve wheel console raised panel");
        RequireNamed(objectName + " Aged Brass Valve Wheel Ring", sceneName + " valve wheel console wheel ring");
        RequireNamed(objectName + " Blackened Iron Wheel Spoke 00", sceneName + " valve wheel console wheel spoke");
        RequireNamed(objectName + " Aged Brass Wheel Hub", sceneName + " valve wheel console wheel hub");
        RequireNamed(objectName + " Aged Brass Wheel Grip 00", sceneName + " valve wheel console wheel grip");
        RequireNamed(objectName + " Left Pressure Gauge Cream Enamel Face", sceneName + " valve wheel console gauge face");
        RequireNamed(objectName + " Left Pressure Gauge Dark Gauge Needle", sceneName + " valve wheel console gauge needle");
        RequireNamed(objectName + " Amber Pilot Lamp 00", sceneName + " valve wheel console amber lamp");
        RequireNamed(objectName + " Brass Pressure Pipe Inlet", sceneName + " valve wheel console inlet pipe");
        RequireNamed(objectName + " Brass Pressure Pipe Outlet", sceneName + " valve wheel console outlet pipe");
        RequireNamed(objectName + " Brass Mount Rivet 00", sceneName + " valve wheel console rivet");
        RequireNamed(objectName + " Oil Grime Streak Below Wheel", sceneName + " valve wheel console oil grime");
        RequireNamed(objectName + " Soot Dark Pipe Joint Smudge", sceneName + " valve wheel console soot smudge");

        RequireRendererMaterial(prototype.backplateRenderer, sceneName + " valve wheel console backplate", "Iron");
        RequireRendererMaterial(prototype.raisedPanelRenderer, sceneName + " valve wheel console panel", "Iron");
        RequireRendererMaterial(prototype.wheelRingRenderer, sceneName + " valve wheel console wheel ring", "Brass");
        RequireRendererMaterial(prototype.wheelSpokeRenderer, sceneName + " valve wheel console spoke", "Iron");
        RequireRendererMaterial(prototype.wheelHubRenderer, sceneName + " valve wheel console hub", "Brass");
        RequireRendererMaterial(prototype.wheelGripRenderer, sceneName + " valve wheel console grip", "Brass");
        RequireRendererMaterial(prototype.gaugeFaceRenderer, sceneName + " valve wheel console gauge face", "Gauge");
        RequireRendererMaterial(prototype.gaugeNeedleRenderer, sceneName + " valve wheel console gauge needle", "Warning");
        RequireRendererMaterial(prototype.lampRenderer, sceneName + " valve wheel console lamp", "Warning");
        RequireRendererMaterial(prototype.pipeRenderer, sceneName + " valve wheel console pipe", "Brass");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " valve wheel console rivet", "Brass");
        RequireRendererMaterial(prototype.grimeRenderer, sceneName + " valve wheel console grime", "Oil");
    }

    private static void ValidateThresholdRouteDressingPrototype(string sceneName, string objectName, string expectedFamily, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " threshold route dressing root");
        ThresholdRouteDressingPrototype prototype = root.GetComponent<ThresholdRouteDressingPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " threshold route dressing is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.33"
            || prototype.batchId != "v0.1.33_threshold_route_dressing"
            || prototype.componentFamily != expectedFamily
            || prototype.placementRole != expectedPlacementRole
            || prototype.gameplayAuthority != "VisualOnlyNoGameplay"
            || !prototype.HasRequiredParts
            || root.name.IndexOf(expectedFamily, StringComparison.OrdinalIgnoreCase) < 0
            || root.name.IndexOf(expectedPlacementRole, StringComparison.OrdinalIgnoreCase) < 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " threshold route dressing metadata, family, role, authority, root name, or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.structureRoot.childCount < 1 || prototype.detailRoot.childCount < 1 || prototype.signalRoot.childCount < 1 || prototype.grimeRoot.childCount < 1)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " threshold route dressing does not have the required structure/detail/signal/grime counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " threshold route dressing must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<NavMeshObstacle>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " threshold route dressing must not add NavMeshObstacle components (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Pickup>().Length > 0 || root.GetComponentsInChildren<PlayerInteraction>().Length > 0 || root.GetComponentsInChildren<LevelTransitionTrigger>().Length > 0 || root.GetComponentsInChildren<ExitTrigger>().Length > 0 || root.GetComponentsInChildren<SteamValveObjective>().Length > 0 || root.GetComponentsInChildren<SteamHazard>().Length > 0 || root.GetComponentsInChildren<FurnaceHeatHazard>().Length > 0 || root.GetComponentsInChildren<GuardianDefeatObjective>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " threshold route dressing must remain visual dressing with no pickup, interaction, objective, damage, or route-authority components (" + objectName + ").");
        }

        RequireNamed(objectName + " Structure Root", sceneName + " threshold route dressing structure root");
        RequireNamed(objectName + " Detail Root", sceneName + " threshold route dressing detail root");
        RequireNamed(objectName + " Signal Root", sceneName + " threshold route dressing signal root");
        RequireNamed(objectName + " Grime Root", sceneName + " threshold route dressing grime root");
        RequireNamed(objectName + " Blackened Iron Structure 00", sceneName + " threshold route dressing blackened iron structure");
        RequireNamed(objectName + " Aged Brass Detail 00", sceneName + " threshold route dressing aged brass detail");
        RequireNamed(objectName + " Amber Signal 00", sceneName + " threshold route dressing amber signal");
        RequireNamed(objectName + " Oil Soot Grime 00", sceneName + " threshold route dressing oil soot grime");

        RequireRendererMaterial(prototype.primaryRenderer, sceneName + " threshold route dressing primary material", "Iron");
        RequireRendererMaterial(prototype.secondaryRenderer, sceneName + " threshold route dressing secondary material", "Brass");
        RequireRendererMaterial(prototype.grimeRenderer, sceneName + " threshold route dressing grime material", "Oil");
    }

    private static void ValidateThresholdRouteDressingBatch(string sceneName, string levelKey)
    {
        string[] families =
        {
            "PistonDoorBracePrototype",
            "PipeClampCouplerSetPrototype",
            "OilSootGrimePanelSetPrototype",
            "AmberIndicatorPlatePrototype",
            "BrassThresholdKickPlatePrototype",
            "RivetedPatchRepairPlatePrototype",
            "PressureSealGasketRingPrototype",
            "RouteReturnPipeMarkerPrototype",
            "SteamVentResidueCollarPrototype",
            "HoistChainAnchorPlatePrototype"
        };

        string[] roles;
        if (levelKey == "intake")
        {
            roles = new[]
            {
                "intake_pressure_gate_brace",
                "intake_gate_pipe_clamps",
                "intake_gate_lift_grime",
                "intake_key_gate_route_plates",
                "intake_gate_lift_kick_plates",
                "intake_route_wall_patches",
                "intake_pressure_gate_seals",
                "intake_key_return_pipe_markers",
                "intake_relief_vent_residue",
                "intake_service_lift_anchors"
            };
        }
        else if (levelKey == "pipeworks")
        {
            roles = new[]
            {
                "pipeworks_boiler_lift_brace",
                "pipeworks_routing_valve_couplers",
                "pipeworks_valve_lift_grime",
                "pipeworks_valve_route_plates",
                "pipeworks_lift_kick_plates",
                "pipeworks_service_wall_patches",
                "pipeworks_locked_lift_seals",
                "pipeworks_restored_flow_markers",
                "pipeworks_pipe_leak_residue",
                "pipeworks_lift_anchors"
            };
        }
        else if (levelKey == "boilerheart")
        {
            roles = new[]
            {
                "boilerheart_foundry_lift_brace",
                "boilerheart_pressure_valve_couplers",
                "boilerheart_valve_heat_grime",
                "boilerheart_valve_lift_plates",
                "boilerheart_lift_ring_kick_plates",
                "boilerheart_boiler_wall_patches",
                "boilerheart_pressure_valve_seals",
                "boilerheart_valve_return_markers",
                "boilerheart_steam_hazard_residue",
                "boilerheart_lift_anchors"
            };
        }
        else if (levelKey == "foundry")
        {
            roles = new[]
            {
                "foundry_emergency_hoist_brace",
                "foundry_heat_bypass_couplers",
                "foundry_furnace_hoist_grime",
                "foundry_hoist_route_plates",
                "foundry_hoist_kick_plates",
                "foundry_battered_wall_patches",
                "foundry_heat_gate_seals",
                "foundry_emergency_flow_markers",
                "foundry_furnace_residue",
                "foundry_emergency_hoist_anchors"
            };
        }
        else
        {
            roles = new[]
            {
                "governor_final_hoist_brace",
                "governor_final_pressure_couplers",
                "governor_warden_hoist_grime",
                "governor_final_route_plates",
                "governor_final_hoist_kick_plates",
                "governor_regulator_wall_patches",
                "governor_master_hoist_seals",
                "governor_final_flow_markers",
                "governor_regulator_residue",
                "governor_final_hoist_anchors"
            };
        }

        for (int i = 0; i < families.Length; i++)
        {
            ValidateThresholdRouteDressingPrototype(sceneName, families[i] + "_" + roles[i], families[i], roles[i]);
        }
    }

    private static void ValidateV0134BatchPolishCoverage(string sceneName, int minWeaponPropCount, int minEnemyReadabilityCount, string[] requiredTargets)
    {
        V0134BatchPolishPrototype[] prototypes = UnityEngine.Object.FindObjectsByType<V0134BatchPolishPrototype>(FindObjectsSortMode.None);
        int weaponPropCount = 0;
        int enemyReadabilityCount = 0;

        for (int i = 0; i < prototypes.Length; i++)
        {
            V0134BatchPolishPrototype prototype = prototypes[i];
            GameObject root = prototype.gameObject;
            if (prototype.promotionVersion != "v0.1.34"
                || prototype.batchId != "v0.1.34_playable_polish_leap"
                || prototype.gameplayAuthority != "VisualOnlyNoGameplay"
                || !prototype.HasRequiredParts)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " v0.1.34 polish metadata or required parts are incomplete (" + root.name + ").");
            }

            if (prototype.structureRoot.childCount < 2 || prototype.signalRoot.childCount < 1 || prototype.wearRoot.childCount < 1)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " v0.1.34 polish lacks required structure/signal/wear counts (" + root.name + ").");
            }

            if (root.GetComponentsInChildren<Collider>().Length > 0 || root.GetComponentsInChildren<NavMeshObstacle>().Length > 0)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " v0.1.34 polish must remain visual-only and non-blocking (" + root.name + ").");
            }

            if (root.GetComponentsInChildren<Pickup>().Length > 0 || root.GetComponentsInChildren<PlayerInteraction>().Length > 0 || root.GetComponentsInChildren<LevelTransitionTrigger>().Length > 0 || root.GetComponentsInChildren<ExitTrigger>().Length > 0 || root.GetComponentsInChildren<SteamValveObjective>().Length > 0 || root.GetComponentsInChildren<SteamHazard>().Length > 0 || root.GetComponentsInChildren<FurnaceHeatHazard>().Length > 0 || root.GetComponentsInChildren<GuardianDefeatObjective>().Length > 0)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " v0.1.34 polish must not own gameplay-authority components (" + root.name + ").");
            }

            RequireNamed(root.name + " Structure Root", sceneName + " v0.1.34 polish structure root");
            RequireNamed(root.name + " Signal Root", sceneName + " v0.1.34 polish signal root");
            RequireNamed(root.name + " Wear Root", sceneName + " v0.1.34 polish wear root");
            RequireNamed(root.name + " Brass Readability Frame", sceneName + " v0.1.34 polish frame");
            RequireNamed(root.name + " Iron Shadow Plate", sceneName + " v0.1.34 polish shadow plate");
            RequireNamed(root.name + " Signal Accent", sceneName + " v0.1.34 polish signal accent");
            RequireNamed(root.name + " Soot Wear", sceneName + " v0.1.34 polish wear accent");

            if (prototype.category == "weapon_prop")
            {
                weaponPropCount++;
            }
            else if (prototype.category == "enemy_readability")
            {
                enemyReadabilityCount++;
            }
            else
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " v0.1.34 polish category is unknown (" + root.name + ").");
            }
        }

        if (weaponPropCount < minWeaponPropCount || enemyReadabilityCount < minEnemyReadabilityCount)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " v0.1.34 polish coverage is too small. Weapon props: " + weaponPropCount + ", enemy readability: " + enemyReadabilityCount + ".");
        }

        for (int i = 0; i < requiredTargets.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < prototypes.Length; j++)
            {
                if (prototypes[j].targetId == requiredTargets[i])
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " v0.1.34 polish target missing: " + requiredTargets[i] + ".");
            }
        }
    }

    private static void ValidatePressureReliefVentPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " pressure relief vent prototype root");
        PressureReliefVentPrototype prototype = root.GetComponent<PressureReliefVentPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure relief vent prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.25" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure relief vent metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.mountRoot.childCount < 2 || prototype.ventRoot.childCount < 3 || prototype.reliefPipeRoot.childCount < 2 || prototype.rivetRoot.childCount < 8 || prototype.tagRoot.childCount < 2 || prototype.steamRoot.childCount < 2)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure relief vent does not have the required mount/vent/pipe/rivet/tag/steam detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure relief vent must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Mount Root", sceneName + " pressure relief vent mount root");
        RequireNamed(objectName + " Vent Root", sceneName + " pressure relief vent vent root");
        RequireNamed(objectName + " Relief Pipe Root", sceneName + " pressure relief vent pipe root");
        RequireNamed(objectName + " Rivet Root", sceneName + " pressure relief vent rivet root");
        RequireNamed(objectName + " Pressure Tag Root", sceneName + " pressure relief vent tag root");
        RequireNamed(objectName + " Ambient Steam Root", sceneName + " pressure relief vent steam root");
        RequireNamed(objectName + " Blackened Iron Mount Plate", sceneName + " pressure relief vent mount plate");
        RequireNamed(objectName + " Aged Brass Relief Vent Stack", sceneName + " pressure relief vent brass stack");
        RequireNamed(objectName + " Blackened Iron Vent Louver Low", sceneName + " pressure relief vent louver");
        RequireNamed(objectName + " Small Aged Brass Relief Pipe", sceneName + " pressure relief vent pipe");
        RequireNamed(objectName + " Brass Vent Bolt 00", sceneName + " pressure relief vent bolt");
        RequireNamed(objectName + " Amber Pressure Tag", sceneName + " pressure relief vent pressure tag");
        RequireNamed(objectName + " Amber Pressure Pointer", sceneName + " pressure relief vent pressure pointer");
        RequireNamed(objectName + " Pale Ambient Steam Puff Low", sceneName + " pressure relief vent steam puff");

        RequireRendererMaterial(prototype.mountPlateRenderer, sceneName + " pressure relief vent mount plate", "Iron");
        RequireRendererMaterial(prototype.ventStackRenderer, sceneName + " pressure relief vent stack", "Brass");
        RequireRendererMaterial(prototype.reliefPipeRenderer, sceneName + " pressure relief vent relief pipe", "Brass");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " pressure relief vent bolt", "Brass");
        RequireRendererMaterial(prototype.pressureTagRenderer, sceneName + " pressure relief vent pressure tag", "Warning");
        RequireRendererMaterial(prototype.steamPuffRenderer, sceneName + " pressure relief vent ambient steam", "Steam");
    }

    private static void ValidateCatwalkRailPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " catwalk rail prototype root");
        CatwalkRailPrototype prototype = root.GetComponent<CatwalkRailPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " catwalk rail prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.26" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " catwalk rail metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.railRoot.childCount < 2 || prototype.uprightRoot.childCount < 5 || prototype.capRoot.childCount < 5 || prototype.footRoot.childCount < 5 || prototype.rivetRoot.childCount < 10)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " catwalk rail does not have the required rail/upright/cap/foot/rivet detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " catwalk rail must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Rail Root", sceneName + " catwalk rail rail root");
        RequireNamed(objectName + " Upright Root", sceneName + " catwalk rail upright root");
        RequireNamed(objectName + " Cap Root", sceneName + " catwalk rail cap root");
        RequireNamed(objectName + " Foot Root", sceneName + " catwalk rail foot root");
        RequireNamed(objectName + " Rivet Root", sceneName + " catwalk rail rivet root");
        RequireNamed(objectName + " Aged Brass Upper Rail", sceneName + " catwalk rail brass upper rail");
        RequireNamed(objectName + " Blackened Iron Lower Rail", sceneName + " catwalk rail iron lower rail");
        RequireNamed(objectName + " Blackened Iron Upright 00", sceneName + " catwalk rail upright");
        RequireNamed(objectName + " Aged Brass Post Cap 00", sceneName + " catwalk rail post cap");
        RequireNamed(objectName + " Blackened Iron Bolted Foot 00", sceneName + " catwalk rail bolted foot");
        RequireNamed(objectName + " Brass Foot Rivet 00", sceneName + " catwalk rail foot rivet");

        RequireRendererMaterial(prototype.upperRailRenderer, sceneName + " catwalk rail upper rail", "Brass");
        RequireRendererMaterial(prototype.lowerRailRenderer, sceneName + " catwalk rail lower rail", "Iron");
        RequireRendererMaterial(prototype.uprightRenderer, sceneName + " catwalk rail upright", "Iron");
        RequireRendererMaterial(prototype.capRenderer, sceneName + " catwalk rail post cap", "Brass");
        RequireRendererMaterial(prototype.footPlateRenderer, sceneName + " catwalk rail foot", "Iron");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " catwalk rail rivet", "Brass");
    }

    private static void ValidateFloorDrainGratePrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " floor drain grate prototype root");
        FloorDrainGratePrototype prototype = root.GetComponent<FloorDrainGratePrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " floor drain grate prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.27" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " floor drain grate metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.frameRoot.childCount < 4 || prototype.trimRoot.childCount < 4 || prototype.grateRoot.childCount < 6 || prototype.rivetRoot.childCount < 8 || prototype.stainRoot.childCount < 2 || prototype.steamRoot.childCount < 2)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " floor drain grate does not have the required frame/trim/grate/rivet/stain/steam detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " floor drain grate must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Frame Root", sceneName + " floor drain frame root");
        RequireNamed(objectName + " Brass Trim Root", sceneName + " floor drain brass trim root");
        RequireNamed(objectName + " Slotted Grate Root", sceneName + " floor drain grate root");
        RequireNamed(objectName + " Rivet Root", sceneName + " floor drain rivet root");
        RequireNamed(objectName + " Oil Stain Root", sceneName + " floor drain stain root");
        RequireNamed(objectName + " Steam Seep Root", sceneName + " floor drain steam root");
        RequireNamed(objectName + " Blackened Iron Drain Frame North", sceneName + " floor drain iron frame");
        RequireNamed(objectName + " Aged Brass Drain Trim North", sceneName + " floor drain brass trim");
        RequireNamed(objectName + " Blackened Iron Slotted Grate Bar 00", sceneName + " floor drain slotted grate bar");
        RequireNamed(objectName + " Brass Drain Bolt 00", sceneName + " floor drain brass bolt");
        RequireNamed(objectName + " Oil Dark Stone Stain Plate A", sceneName + " floor drain oil stain");
        RequireNamed(objectName + " Pale Steam Seep Low", sceneName + " floor drain steam seep");

        RequireRendererMaterial(prototype.frameRenderer, sceneName + " floor drain frame", "Iron");
        RequireRendererMaterial(prototype.brassTrimRenderer, sceneName + " floor drain trim", "Brass");
        RequireRendererMaterial(prototype.grateBarRenderer, sceneName + " floor drain grate bar", "Iron");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " floor drain bolt", "Brass");
        RequireRendererMaterial(prototype.oilStainRenderer, sceneName + " floor drain oil stain", "Oil");
        RequireRendererMaterial(prototype.steamSeepRenderer, sceneName + " floor drain steam seep", "Steam");
    }

    private static void ValidatePressureTankRackPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " pressure tank rack prototype root");
        PressureTankRackPrototype prototype = root.GetComponent<PressureTankRackPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure tank rack prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.28" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure tank rack metadata or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.rackRoot.childCount < 4 || prototype.tankRoot.childCount < 3 || prototype.bandRoot.childCount < 6 || prototype.feederPipeRoot.childCount < 3 || prototype.valveRoot.childCount < 3 || prototype.rivetRoot.childCount < 12 || prototype.tagRoot.childCount < 2 || prototype.steamRoot.childCount < 2)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure tank rack does not have the required rack/tank/band/pipe/valve/rivet/tag/steam detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " pressure tank rack must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        RequireNamed(objectName + " Rack Root", sceneName + " pressure tank rack rack root");
        RequireNamed(objectName + " Tank Root", sceneName + " pressure tank rack tank root");
        RequireNamed(objectName + " Brass Band Root", sceneName + " pressure tank rack band root");
        RequireNamed(objectName + " Feeder Pipe Root", sceneName + " pressure tank rack feeder pipe root");
        RequireNamed(objectName + " Valve Root", sceneName + " pressure tank rack valve root");
        RequireNamed(objectName + " Rivet Root", sceneName + " pressure tank rack rivet root");
        RequireNamed(objectName + " Pressure Tag Root", sceneName + " pressure tank rack tag root");
        RequireNamed(objectName + " Ambient Steam Root", sceneName + " pressure tank rack steam root");
        RequireNamed(objectName + " Blackened Iron Lower Rack Rail", sceneName + " pressure tank rack frame");
        RequireNamed(objectName + " Blackened Iron Pressure Tank 00", sceneName + " pressure tank rack tank");
        RequireNamed(objectName + " Aged Brass Tank Band Low 00", sceneName + " pressure tank rack brass band");
        RequireNamed(objectName + " Aged Brass Feeder Pipe 00", sceneName + " pressure tank rack feeder pipe");
        RequireNamed(objectName + " Aged Brass Valve Cap 00", sceneName + " pressure tank rack valve");
        RequireNamed(objectName + " Brass Rack Bolt 00", sceneName + " pressure tank rack bolt");
        RequireNamed(objectName + " Amber Pressure Tag", sceneName + " pressure tank rack pressure tag");
        RequireNamed(objectName + " Pale Ambient Steam Seep Low", sceneName + " pressure tank rack steam seep");

        RequireRendererMaterial(prototype.rackFrameRenderer, sceneName + " pressure tank rack frame", "Iron");
        RequireRendererMaterial(prototype.tankRenderer, sceneName + " pressure tank rack tank", "Iron");
        RequireRendererMaterial(prototype.brassBandRenderer, sceneName + " pressure tank rack band", "Brass");
        RequireRendererMaterial(prototype.feederPipeRenderer, sceneName + " pressure tank rack feeder pipe", "Brass");
        RequireRendererMaterial(prototype.valveRenderer, sceneName + " pressure tank rack valve", "Brass");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " pressure tank rack bolt", "Brass");
        RequireRendererMaterial(prototype.pressureTagRenderer, sceneName + " pressure tank rack pressure tag", "Warning");
        RequireRendererMaterial(prototype.steamSeepRenderer, sceneName + " pressure tank rack steam seep", "Steam");
    }

    private static void ValidateServiceLiftCallBoxPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " service lift call box prototype root");
        ServiceLiftCallBoxPrototype prototype = root.GetComponent<ServiceLiftCallBoxPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " service lift call box prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.30" || prototype.placementRole != expectedPlacementRole || !prototype.HasRequiredParts || root.name.IndexOf(expectedPlacementRole, StringComparison.OrdinalIgnoreCase) < 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " service lift call box metadata, role, root name, or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.backplateRoot.childCount < 1 || prototype.leverRoot.childCount < 1 || prototype.gaugeRoot.childCount < 1 || prototype.lampRoot.childCount < 2 || prototype.pipeRoot.childCount < 2 || prototype.labelRoot.childCount < 1 || prototype.rivetRoot.childCount < 8 || prototype.grimeRoot.childCount < 2)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " service lift call box does not have the required backplate/lever/gauge/lamp/pipe/label/rivet/grime detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " service lift call box must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<NavMeshObstacle>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " service lift call box must not add NavMeshObstacle components (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<LevelTransitionTrigger>().Length > 0 || root.GetComponentsInChildren<ExitTrigger>().Length > 0 || root.GetComponentsInChildren<SteamValveObjective>().Length > 0 || root.GetComponentsInChildren<Pickup>().Length > 0 || root.GetComponentsInChildren<SteamHazard>().Length > 0 || root.GetComponentsInChildren<FurnaceHeatHazard>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " service lift call box must remain visual dressing with no gameplay authority components (" + objectName + ").");
        }

        RequireNamed(objectName + " Backplate Root", sceneName + " service lift call box backplate root");
        RequireNamed(objectName + " Lever Root", sceneName + " service lift call box lever root");
        RequireNamed(objectName + " Gauge Root", sceneName + " service lift call box gauge root");
        RequireNamed(objectName + " Lamp Root", sceneName + " service lift call box lamp root");
        RequireNamed(objectName + " Pipe Root", sceneName + " service lift call box pipe root");
        RequireNamed(objectName + " Label Root", sceneName + " service lift call box label root");
        RequireNamed(objectName + " Rivet Root", sceneName + " service lift call box rivet root");
        RequireNamed(objectName + " Grime Root", sceneName + " service lift call box grime root");
        RequireNamed(objectName + " Blackened Iron Call Box Backplate", sceneName + " service lift call box backplate");
        RequireNamed(objectName + " Aged Brass Pull Lever", sceneName + " service lift call box pull lever");
        RequireNamed(objectName + " Aged Brass Lever Guard Left", sceneName + " service lift call box lever guard");
        RequireNamed(objectName + " Cream Enamel Lift Pressure Gauge", sceneName + " service lift call box gauge");
        RequireNamed(objectName + " Dark Lift Gauge Needle", sceneName + " service lift call box gauge needle");
        RequireNamed(objectName + " Amber Lift Ready Lamp", sceneName + " service lift call box amber lamp");
        RequireNamed(objectName + " Red Lift Locked Lamp", sceneName + " service lift call box red lamp");
        RequireNamed(objectName + " Green Service Lift Lamp", sceneName + " service lift call box green lamp");
        RequireNamed(objectName + " Copper Pressure Feed Pipe A", sceneName + " service lift call box feed pipe");
        RequireNamed(objectName + " Copper Pressure Return Pipe B", sceneName + " service lift call box return pipe");
        RequireNamed(objectName + " Stamped Brass Hoist Call Label", sceneName + " service lift call box label plate");
        RequireNamed(objectName + " Brass Call Box Rivet 00", sceneName + " service lift call box rivet");
        RequireNamed(objectName + " Oil Streak Plate Low", sceneName + " service lift call box oil streak");
        RequireNamed(objectName + " Soot Scorch Plate Upper", sceneName + " service lift call box scorch mark");

        RequireRendererMaterial(prototype.backplateRenderer, sceneName + " service lift call box backplate", "Iron");
        RequireRendererMaterial(prototype.leverRenderer, sceneName + " service lift call box lever", "Brass");
        RequireRendererMaterial(prototype.gaugeRenderer, sceneName + " service lift call box gauge", "CreamGaugeFace");
        RequireRendererMaterial(prototype.lampRenderer, sceneName + " service lift call box green lamp", "ServiceLift");
        RequireRendererMaterial(prototype.pipeRenderer, sceneName + " service lift call box pressure pipe", "Brass");
        RequireRendererMaterial(prototype.labelRenderer, sceneName + " service lift call box label", "Brass");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " service lift call box rivet", "Brass");
        RequireRendererMaterial(prototype.grimeRenderer, sceneName + " service lift call box grime", "Oil");
    }

    private static void ValidateGearKeyPlinthPrototype(string sceneName, string objectName, string expectedPlacementRole)
    {
        GameObject root = RequireNamed(objectName, sceneName + " gear key plinth prototype root");
        GearKeyPlinthPrototype prototype = root.GetComponent<GearKeyPlinthPrototype>();
        if (prototype == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " gear key plinth prototype is missing its marker component (" + objectName + ").");
        }

        if (prototype.promotionVersion != "v0.1.31" || prototype.placementRole != expectedPlacementRole || prototype.gameplayAuthority != "ExistingGearKeyPickup" || !prototype.HasRequiredParts || root.name.IndexOf(expectedPlacementRole, StringComparison.OrdinalIgnoreCase) < 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " gear key plinth metadata, authority, role, root name, or required parts are incomplete (" + objectName + ").");
        }

        if (prototype.baseRoot.childCount < 1 || prototype.cradleRoot.childCount < 9 || prototype.gaugeRoot.childCount < 1 || prototype.lampRoot.childCount < 1 || prototype.trimRoot.childCount < 2 || prototype.labelRoot.childCount < 1 || prototype.rivetRoot.childCount < 12 || prototype.grimeRoot.childCount < 3)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " gear key plinth does not have the required pedestal/cradle/gauge/lamp/trim/label/rivet/grime detail counts (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Collider>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " gear key plinth must remain non-blocking route dressing with no colliders (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<NavMeshObstacle>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " gear key plinth must not add NavMeshObstacle components (" + objectName + ").");
        }

        if (root.GetComponentsInChildren<Pickup>().Length > 0 || root.GetComponentsInChildren<PlayerInteraction>().Length > 0 || root.GetComponentsInChildren<LevelTransitionTrigger>().Length > 0 || root.GetComponentsInChildren<ExitTrigger>().Length > 0 || root.GetComponentsInChildren<SteamValveObjective>().Length > 0 || root.GetComponentsInChildren<SteamHazard>().Length > 0 || root.GetComponentsInChildren<FurnaceHeatHazard>().Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " gear key plinth must remain visual dressing with no pickup, interaction, or route-authority components (" + objectName + ").");
        }

        GameObject gearKey = RequireNamed("Pickup - Gear Key", sceneName + " existing gear key pickup");
        Pickup gearKeyPickup = gearKey.GetComponent<Pickup>();
        if (gearKeyPickup == null || gearKeyPickup.kind != PickupKind.Key || gearKey.transform.IsChildOf(root.transform) || Vector3.Distance(gearKey.transform.position, root.transform.position) > 1.25f || gearKey.transform.position.y < 0.45f)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " existing gear key pickup must remain the reachable authority object near, but outside, the plinth hierarchy.");
        }

        RequireNamed(objectName + " Pedestal Root", sceneName + " gear key plinth pedestal root");
        RequireNamed(objectName + " Gear Cradle Root", sceneName + " gear key plinth cradle root");
        RequireNamed(objectName + " Label Gauge Root", sceneName + " gear key plinth gauge root");
        RequireNamed(objectName + " Amber Lamp Root", sceneName + " gear key plinth lamp root");
        RequireNamed(objectName + " Brass Trim Root", sceneName + " gear key plinth trim root");
        RequireNamed(objectName + " Metadata Root", sceneName + " gear key plinth label root");
        RequireNamed(objectName + " Rivet Hardware Root", sceneName + " gear key plinth rivet root");
        RequireNamed(objectName + " Grime Wear Root", sceneName + " gear key plinth grime root");
        RequireNamed(objectName + " Blackened Iron Gear Key Pedestal Body", sceneName + " gear key plinth pedestal body");
        RequireNamed(objectName + " Aged Brass Gear Key Cradle Ring", sceneName + " gear key plinth cradle ring");
        RequireNamed(objectName + " Aged Brass Gear Tooth 00", sceneName + " gear key plinth gear tooth");
        RequireNamed(objectName + " Cream Enamel Key Pressure Gauge", sceneName + " gear key plinth gauge");
        RequireNamed(objectName + " Amber Gear Key Ready Lamp", sceneName + " gear key plinth lamp");
        RequireNamed(objectName + " Aged Brass Top Plate", sceneName + " gear key plinth trim");
        RequireNamed(objectName + " Cream Enamel Gear Key Label", sceneName + " gear key plinth label");
        RequireNamed(objectName + " Brass Plinth Rivet 00", sceneName + " gear key plinth rivet");
        RequireNamed(objectName + " Oil Streak Below Key", sceneName + " gear key plinth oil streak");
        RequireNamed(objectName + " Soot Scorch On Top Plate", sceneName + " gear key plinth soot scorch");

        RequireRendererMaterial(prototype.baseRenderer, sceneName + " gear key plinth base", "Iron");
        RequireRendererMaterial(prototype.cradleRenderer, sceneName + " gear key plinth cradle", "Brass");
        RequireRendererMaterial(prototype.gearToothRenderer, sceneName + " gear key plinth tooth", "Brass");
        RequireRendererMaterial(prototype.gaugeRenderer, sceneName + " gear key plinth gauge", "CreamGaugeFace");
        RequireRendererMaterial(prototype.lampRenderer, sceneName + " gear key plinth lamp", "GearKey");
        RequireRendererMaterial(prototype.trimRenderer, sceneName + " gear key plinth trim", "Brass");
        RequireRendererMaterial(prototype.labelRenderer, sceneName + " gear key plinth label", "CreamGaugeFace");
        RequireRendererMaterial(prototype.rivetRenderer, sceneName + " gear key plinth rivet", "Brass");
        RequireRendererMaterial(prototype.grimeRenderer, sceneName + " gear key plinth grime", "Oil");
    }

    private static void RequireRendererMaterial(Renderer renderer, string label, string expectedNameFragment)
    {
        if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.shader == null)
        {
            throw new InvalidOperationException("Level validation failed: missing renderer material for " + label + ".");
        }

        if (renderer.sharedMaterial.name.IndexOf(expectedNameFragment, StringComparison.OrdinalIgnoreCase) < 0)
        {
            throw new InvalidOperationException("Level validation failed: " + label + " expected material containing '" + expectedNameFragment + "' but found '" + renderer.sharedMaterial.name + "'.");
        }
    }

    private static void ValidateEnvironmentPropVisuals(string sceneName)
    {
        ValidateSidecarQuarantineShowcase(sceneName);

        if (sceneName == "Level01")
        {
            RequireNamed("Work Order Board - Intake", sceneName + " intake work-order board visual");
            RequireNamed("Lore Plaque - Intake Archive", sceneName + " intake lore plaque visual");
            RequireNamed("Work Order Board - Gate", sceneName + " gate work-order board visual");
            RequireNamed("Pipe Bundle - Gate Manifold", sceneName + " gate pipe-bundle visual");
            RequireNamed("North Star Intake Pipe Canopy", sceneName + " north-star intake pipe canopy");
            ValidatePipeCanopyPrototype(sceneName, "North Star Intake Pipe Canopy", "intake_route_pipe_canopy");
            RequireNamed("North Star Intake Gaslight Left", sceneName + " north-star intake gaslight");
            RequireNamed("North Star Gate Rivet Band", sceneName + " north-star gate rivet band");
            ValidateRivetBandPrototype(sceneName, "North Star Gate Rivet Band", "intake_gate_rivet_band");
            ValidateWallValveWheelPrototype(sceneName, "North Star Intake Wall Valve Wheel", "intake_wall_valve_wheel");
            ValidatePressureReliefVentPrototype(sceneName, "North Star Intake Pressure Relief Vent", "intake_pressure_relief_vent");
            ValidateFloorDrainGratePrototype(sceneName, "North Star Intake Floor Drain Grate", "intake_floor_drain_grate");
            ValidatePressureTankRackPrototype(sceneName, "North Star Intake Pressure Tank Rack", "intake_pressure_tank_rack");
            ValidateServiceLiftCallBoxPrototype(sceneName, "ServiceLiftCallBoxPrototype_intake_service_lift_call_box", "intake_service_lift_call_box");
            ValidateGearKeyPlinthPrototype(sceneName, "GearKeyPlinthPrototype_intake_gear_key_plinth", "intake_gear_key_plinth");
            ValidateThresholdRouteDressingBatch(sceneName, "intake");
            ValidateV0134BatchPolishCoverage(sceneName, 3, 4, new[] { "pressure_pistol", "steam_scattergun", "pressure_cartridge_pack", "scrapper" });
            RequireNamed("Secret - Intake Pressure Cache", sceneName + " secret pressure cache");
            RequireNamed("Secret Pressure Cache Brass Floor Plate", sceneName + " secret pressure cache floor plate");
            RequireNamed("Level01 Flow Polish V015", sceneName + " flow polish prop root");
            RequireNamed("Level01 Gate Preview Brass Sightline Rail Left", sceneName + " gate preview sightline rail");
            RequireNamed("Level01 Gate Preview Red Locking Header", sceneName + " gate preview locking header");
            RequireNamed("Level01 Key Branch Return Brass Pipe A", sceneName + " key branch return pipe");
            RequireNamed("Level01 Key Branch Return Chevron A", sceneName + " key branch return chevron");
            RequireNamed("Level01 Service Lift Green Runway Center", sceneName + " service lift runway");
            RequireNamed("Level01 Service Lift Green Beacon Light", sceneName + " service lift beacon");
            RequireNamed("Level01 Secret Warm Pipe Clue", sceneName + " secret clue pipe");
            RequireNamed("Level01 Secret Misaligned Service Plate", sceneName + " secret clue plate");
        }
        else if (sceneName == "Level02")
        {
            RequireNamed("Work Order Board - Pipeworks", sceneName + " pipeworks work-order board visual");
            RequireNamed("Lore Plaque - Pipeworks Archive", sceneName + " pipeworks lore plaque visual");
            RequireNamed("Pipeworks Routing Valve Objective", sceneName + " Pipeworks routing valve objective");
            RequireNamed("Pipeworks Routing Valve Wheel", sceneName + " Pipeworks routing valve wheel visual");
            RequireNamed("Pipeworks Routing Valve Vented Lamp", sceneName + " Pipeworks routing valve vented signal");
            RequireNamed("Pipeworks Triple Pipe Bundle", sceneName + " pipeworks pipe-bundle visual");
            RequireNamed("North Star Pipeworks Pipe Canopy", sceneName + " north-star pipeworks pipe canopy");
            ValidatePipeCanopyPrototype(sceneName, "North Star Pipeworks Pipe Canopy", "pipeworks_route_pipe_canopy");
            RequireNamed("North Star Pipeworks Gaslight", sceneName + " north-star pipeworks gaslight");
            ValidateCagedGaslightPrototype(sceneName, "North Star Pipeworks Gaslight", "pipeworks_route_gaslight");
            RequireNamed("North Star Pipeworks Wall Rivet Band", sceneName + " north-star pipeworks rivet band");
            ValidateRivetBandPrototype(sceneName, "North Star Pipeworks Wall Rivet Band", "pipeworks_wall_rivet_band");
            ValidateWallValveWheelPrototype(sceneName, "North Star Pipeworks Route Valve Wheel", "pipeworks_route_valve_wheel");
            ValidatePressureReliefVentPrototype(sceneName, "North Star Pipeworks Pressure Relief Vent", "pipeworks_pressure_relief_vent");
            ValidateCatwalkRailPrototype(sceneName, "North Star Pipeworks Service Rail", "pipeworks_service_rail");
            ValidateFloorDrainGratePrototype(sceneName, "North Star Pipeworks Floor Drain Grate", "pipeworks_floor_drain_grate");
            ValidatePressureTankRackPrototype(sceneName, "North Star Pipeworks Pressure Tank Rack", "pipeworks_pressure_tank_rack");
            ValidateServiceLiftCallBoxPrototype(sceneName, "ServiceLiftCallBoxPrototype_pipeworks_service_lift_call_box", "pipeworks_service_lift_call_box");
            ValidateWallPipeGaugeClusterPrototype(sceneName, "Pipeworks Prototype Wall Pipe Gauge Cluster", "pipeworks_route_wall");
            ValidateBoilerControlConsolePrototype(sceneName, "Pipeworks Prototype Boiler Control Console", "pipeworks_route_console");
            ValidateRivetedPressureDoorFramePrototype(sceneName, "Pipeworks Prototype Riveted Pressure Door Frame", "pipeworks_route_pressure_frame");
            ValidateValveWheelConsolePrototype(sceneName, "ValveWheelConsolePrototype_pipeworks_pressure_console", "pipeworks_pressure_console");
            ValidateThresholdRouteDressingBatch(sceneName, "pipeworks");
            ValidateV0134BatchPolishCoverage(sceneName, 4, 2, new[] { "pressure_pistol", "steam_scattergun", "pressure_cartridge_pack", "scrapper", "lancer" });
            RequireNamed("Secret - Pipeworks Cartridge Cache", sceneName + " pipeworks secret cache");
            RequireNamed("Secret Pipeworks Cache Brass Floor Plate", sceneName + " pipeworks secret cache floor plate");
            RequireNamed("Pickup - Pipeworks Secret Pressure Cartridge Pack", sceneName + " pipeworks secret ammo reward");
            RequireNamed("Level02 Pipeworks Flow Polish V016", sceneName + " Pipeworks flow polish root");
            RequireNamed("Level02 Pipeworks Locked Boilerheart Lift Stop Bar", sceneName + " Pipeworks locked lift stop bar");
            RequireNamed("Level02 Routing Valve Floor Lead", sceneName + " Pipeworks routing valve floor lead");
            RequireNamed("Level02 Lancer Sightline Brass Cover West", sceneName + " Pipeworks Lancer cover");
            RequireNamed("Level02 Secret Cold Pipe Clue", sceneName + " Pipeworks secret clue");
        }
        else if (sceneName == "Level03")
        {
            RequireNamed("Work Order Board - Boilerheart", sceneName + " boilerheart work-order board visual");
            RequireNamed("Lore Plaque - Boilerheart Archive", sceneName + " boilerheart lore plaque visual");
            RequireNamed("Boilerheart Triple Pipe Bundle", sceneName + " boilerheart pipe-bundle visual");
            RequireNamed("Boilerheart Furnace Core", sceneName + " boilerheart furnace core visual");
            RequireNamed("North Star Boilerheart Pipe Canopy", sceneName + " north-star boilerheart pipe canopy");
            ValidatePipeCanopyPrototype(sceneName, "North Star Boilerheart Pipe Canopy", "boilerheart_route_pipe_canopy");
            RequireNamed("North Star Boilerheart Lamp Cage", sceneName + " north-star boilerheart lamp cage");
            ValidateCagedGaslightPrototype(sceneName, "North Star Boilerheart Lamp Cage", "boilerheart_route_gaslight");
            RequireNamed("North Star Boilerheart Core Rivet Band", sceneName + " north-star boilerheart rivet band");
            ValidateRivetBandPrototype(sceneName, "North Star Boilerheart Core Rivet Band", "boilerheart_core_rivet_band");
            ValidateWallValveWheelPrototype(sceneName, "North Star Boilerheart Core Valve Wheel", "boilerheart_core_valve_wheel");
            ValidateCatwalkRailPrototype(sceneName, "North Star Boilerheart Service Rail", "boilerheart_service_rail");
            ValidateWallPipeGaugeClusterPrototype(sceneName, "Boilerheart Prototype Wall Pipe Gauge Cluster", "boilerheart_route_wall");
            ValidateBoilerControlConsolePrototype(sceneName, "Boilerheart Prototype Boiler Control Console", "boilerheart_route_console");
            ValidateRivetedPressureDoorFramePrototype(sceneName, "Boilerheart Prototype Riveted Pressure Door Frame", "boilerheart_route_pressure_frame");
            ValidateServiceLiftCallBoxPrototype(sceneName, "ServiceLiftCallBoxPrototype_boilerheart_service_lift_call_box", "boilerheart_service_lift_call_box");
            ValidateValveWheelConsolePrototype(sceneName, "ValveWheelConsolePrototype_boilerheart_pressure_console", "boilerheart_pressure_console");
            ValidateThresholdRouteDressingBatch(sceneName, "boilerheart");
            ValidateV0134BatchPolishCoverage(sceneName, 4, 2, new[] { "pressure_pistol", "steam_scattergun", "pressure_cartridge_pack", "steam_scattergun_pickup", "scrapper" });
            RequireNamed("Boilerheart Pressure Valve Objective", sceneName + " boilerheart pressure valve objective");
            RequireNamed("Boilerheart Pressure Valve Wheel", sceneName + " boilerheart pressure valve wheel visual");
            RequireNamed("Boilerheart Valve Vented Lamp", sceneName + " boilerheart valve vented signal");
            RequireNamed("Pickup - Steam Scattergun", sceneName + " Steam Scattergun pickup");
            RequireNamed("Pickup - Steam Scattergun Weapon Visual", sceneName + " Steam Scattergun pickup visual");
            RequireNamed("Pickup - Steam Scattergun Brass Display Stand", sceneName + " Steam Scattergun display stand visual");
            RequireNamed("Pickup - Steam Scattergun Brass Top Rib", sceneName + " Steam Scattergun top rib visual");
            RequireNamed("Pickup - Steam Scattergun Walnut Pump Grip", sceneName + " Steam Scattergun pump grip visual");
            RequireNamed("Pickup - Steam Scattergun Brass Pressure Coil", sceneName + " Steam Scattergun pressure coil visual");
            RequireNamed("Pickup - Steam Scattergun Rear Valve Wheel", sceneName + " Steam Scattergun valve wheel visual");
            RequireNamed("Pickup - Steam Scattergun Brass Shell Rack Round 0", sceneName + " Steam Scattergun shell rack visual");
            RequireNamed("Pickup - Steam Scattergun Enamel Name Plate", sceneName + " Steam Scattergun display name plate visual");
            RequireNamed("Steam Scattergun Pickup Readability Cues", sceneName + " Steam Scattergun readability cue root");
            RequireNamed("Steam Scattergun Route Brass Floor Strip A", sceneName + " Steam Scattergun route strip");
            RequireNamed("Steam Scattergun Pickup Arrow Plate", sceneName + " Steam Scattergun pickup arrow plate");
            RequireNamed("Steam Scattergun Pickup Sign Backplate", sceneName + " Steam Scattergun sign backplate");
            RequireNamed("Steam Scattergun Pickup Pressure Feed Pipe", sceneName + " Steam Scattergun pickup pressure pipe");
            RequireNamed("Steam Scattergun Pickup Amber Lamp", sceneName + " Steam Scattergun pickup amber light");
            RequireNamed("Label - Steam Scattergun Pickup", sceneName + " Steam Scattergun pickup label");
            RequireNamed("Enemy - Boilerheart Bellows Node", sceneName + " Bellows Node enemy");
            RequireNamed("Bellows Node Brass Bellows Body", sceneName + " Bellows Node body visual");
            RequireNamed("Bellows Node Furnace Lens", sceneName + " Bellows Node lens visual");
            RequireNamed("Bellows Node Exhaust Horn", sceneName + " Bellows Node horn visual");
            RequireNamed("Boilerheart Steam Hazard - Furnace Leak", sceneName + " boilerheart steam hazard");
            RequireNamed("Boilerheart Steam Hazard - Core Bleed", sceneName + " boilerheart steam hazard");
            RequireNamed("Level03 Boilerheart Flow Polish V016", sceneName + " Boilerheart flow polish root");
            RequireNamed("Level03 Boilerheart Ring Brass Guide South", sceneName + " Boilerheart ring guide");
            RequireNamed("Level03 Scattergun Trial Lane Strip", sceneName + " Boilerheart scattergun trial lane");
            RequireNamed("Level03 Bellows Pulse Radius Marker", sceneName + " Bellows pulse radius marker");
            RequireNamed("Level03 Bellows Boost Pipe To Scrapper Lane", sceneName + " Bellows boost pipe");
            RequireNamed("Level03 Valve To Lift Green Return Strip", sceneName + " Boilerheart valve-to-lift return strip");
            RequireNamed("Level03 Foundry Lift Locked Stop Bar", sceneName + " Boilerheart foundry lift locked stop bar");
            RequireNamed("Level03 Hazard Shutdown Sight Glass", sceneName + " Boilerheart hazard shutdown sight glass");
        }
        else if (sceneName == "Level04")
        {
            RequireNamed("Work Order Board - Foundry", sceneName + " foundry work-order board visual");
            RequireNamed("Lore Plaque - Foundry Archive", sceneName + " foundry lore plaque visual");
            RequireNamed("Foundry Triple Pipe Bundle", sceneName + " foundry pipe-bundle visual");
            RequireNamed("North Star Foundry Pipe Canopy", sceneName + " north-star foundry pipe canopy");
            ValidatePipeCanopyPrototype(sceneName, "North Star Foundry Pipe Canopy", "foundry_route_pipe_canopy");
            RequireNamed("North Star Foundry Catwalk Rail", sceneName + " north-star foundry catwalk rail");
            ValidateCatwalkRailPrototype(sceneName, "North Star Foundry Catwalk Rail", "foundry_catwalk_rail");
            RequireNamed("North Star Foundry Gaslight West", sceneName + " north-star foundry gaslight");
            ValidateCagedGaslightPrototype(sceneName, "North Star Foundry Gaslight West", "foundry_route_gaslight");
            ValidatePressureReliefVentPrototype(sceneName, "North Star Foundry Pressure Relief Vent", "foundry_pressure_relief_vent");
            ValidateFloorDrainGratePrototype(sceneName, "North Star Foundry Floor Drain Grate", "foundry_floor_drain_grate");
            ValidateServiceLiftCallBoxPrototype(sceneName, "ServiceLiftCallBoxPrototype_foundry_emergency_hoist_call_box", "foundry_emergency_hoist_call_box");
            ValidateThresholdRouteDressingBatch(sceneName, "foundry");
            ValidateV0134BatchPolishCoverage(sceneName, 4, 4, new[] { "pressure_pistol", "steam_scattergun", "pressure_cartridge_pack", "scrapper", "lancer", "bulwark" });
            RequireNamed("Foundry Furnace Row", sceneName + " foundry furnace row visual");
            RequireNamed("Foundry Steam Hazard - Casting Leak", sceneName + " foundry steam hazard");
            RequireNamed("Foundry Steam Hazard - Crucible Bleed", sceneName + " foundry steam hazard");
            RequireNamed("Foundry Furnace Heat Hazard - Pour Lane", sceneName + " foundry furnace heat hazard");
            RequireNamed("Foundry Furnace Heat Hazard - Hoist Lane", sceneName + " foundry furnace heat hazard");
            RequireNamed("Foundry Furnace Heat Hazard - Pour Lane Furnace Glow Plate", sceneName + " foundry heat glow signal");
            RequireNamed("Enemy - Foundry Hammer Bulwark", sceneName + " foundry Bulwark enemy");
            RequireNamed("Bulwark Riveted Boiler Body", sceneName + " Bulwark body visual");
            RequireNamed("Bulwark Furnace Belly", sceneName + " Bulwark furnace belly visual");
            RequireNamed("Bulwark Right Hammer Head", sceneName + " Bulwark hammer visual");
            RequireNamed("Secret - Foundry Coal Cache", sceneName + " foundry secret cache");
            RequireNamed("Secret Foundry Cache Brass Floor Plate", sceneName + " foundry secret cache floor plate");
            RequireNamed("Secret Foundry Cache Coal Lump A", sceneName + " foundry secret coal prop");
            RequireNamed("Foundry Emergency Hoist", sceneName + " emergency hoist visual");
            RequireNamed("Level04 Foundry Climax Polish V017", sceneName + " Foundry climax polish root");
            RequireNamed("Level04 Furnace Timing Preview Strip", sceneName + " Foundry furnace timing preview strip");
            RequireNamed("Level04 Bulwark Hammer Bay Floor Ring", sceneName + " Bulwark hammer bay ring");
            RequireNamed("Level04 Hoist Green Runway Strip", sceneName + " Foundry hoist runway strip");
            RequireNamed("Level04 Coal Cache Footprint Clue", sceneName + " Foundry coal cache clue");
        }
        else if (sceneName == "Level05")
        {
            RequireNamed("Work Order Board - Governor Core", sceneName + " governor core work-order board visual");
            RequireNamed("Lore Plaque - Governor Archive", sceneName + " governor lore plaque visual");
            RequireNamed("Governor Core Triple Pipe Bundle", sceneName + " governor core pipe-bundle visual");
            RequireNamed("North Star Governor Pipe Canopy", sceneName + " north-star governor pipe canopy");
            ValidatePipeCanopyPrototype(sceneName, "North Star Governor Pipe Canopy", "governor_route_pipe_canopy");
            RequireNamed("North Star Governor Gaslight Left", sceneName + " north-star governor gaslight");
            ValidateCagedGaslightPrototype(sceneName, "North Star Governor Gaslight Left", "governor_route_gaslight");
            ValidatePressureTankRackPrototype(sceneName, "North Star Governor Pressure Tank Rack", "governor_pressure_tank_rack");
            ValidateServiceLiftCallBoxPrototype(sceneName, "ServiceLiftCallBoxPrototype_governor_master_hoist_call_box", "governor_master_hoist_call_box");
            ValidateThresholdRouteDressingBatch(sceneName, "governor");
            ValidateV0134BatchPolishCoverage(sceneName, 3, 4, new[] { "pressure_pistol", "steam_scattergun", "pressure_cartridge_pack", "scrapper", "lancer", "bulwark", "warden" });
            RequireNamed("North Star Governor Regulator Crown", sceneName + " north-star governor regulator crown");
            RequireNamed("Governor Core Regulator Pillar", sceneName + " governor core regulator visual");
            RequireNamed("Governor Core Steam Hazard - Regulator Leak", sceneName + " governor core steam hazard");
            RequireNamed("Governor Core Furnace Heat Hazard - Regulator Surge", sceneName + " governor core furnace heat hazard");
            RequireNamed("Governor Core Master Override Hoist", sceneName + " governor core final hoist visual");
            RequireNamed("Enemy - Governor Core Bulwark", sceneName + " governor core Bulwark enemy");
            RequireNamed("Enemy - Governor Core Warden", sceneName + " governor core Warden enemy");
            RequireNamed("Bulwark Riveted Boiler Body", sceneName + " Bulwark body visual");
            RequireNamed("Bulwark Furnace Belly", sceneName + " Bulwark furnace belly visual");
            RequireNamed("Bulwark Right Hammer Head", sceneName + " Bulwark hammer visual");
            RequireNamed("Governor Warden Core Body", sceneName + " Governor Warden body visual");
            RequireNamed("Governor Warden Furnace Heart", sceneName + " Governor Warden furnace heart visual");
            RequireNamed("Governor Warden Pressure Crown", sceneName + " Governor Warden crown visual");
            RequireNamed("Governor Warden Pressure Cannon Muzzle", sceneName + " Governor Warden pressure cannon visual");
            RequireNamed("Governor Warden Defeat Objective", sceneName + " Governor Warden defeat objective");
            RequireNamed("Governor Warden Lock Red Signal", sceneName + " Governor Warden locked signal");
            RequireNamed("Level05 Governor Climax Polish V017", sceneName + " Governor climax polish root");
            RequireNamed("Level05 Warden Reveal Centerline Rail", sceneName + " Warden reveal centerline rail");
            RequireNamed("Level05 Warden Arena Boundary Ring", sceneName + " Warden arena boundary ring");
            RequireNamed("Level05 Boss Cover Pylon West", sceneName + " Warden arena cover pylon");
            RequireNamed("Level05 Master Override Green Runway", sceneName + " master override runway");
            RequireNamed("Level05 Master Override Green Beacon", sceneName + " master override beacon");
        }

        if (sceneName == "Level01")
        {
            RequireNamed("Repair Bay Cover Boiler Left", sceneName + " repair bay cover visual");
            RequireNamed("Repair Bay Cover Crate Right", sceneName + " repair bay cover visual");
            RequireNamed("Final Room Cover Stack West", sceneName + " final room cover visual");
        }
    }

    private static void ValidateSidecarQuarantineShowcase(string sceneName)
    {
        GameObject root = RequireNamed("Sidecar Quarantine Showcase - " + sceneName, sceneName + " sidecar quarantine showcase root");
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length < GetMinimumSidecarShowcaseRendererCount(sceneName))
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " sidecar showcase has too few renderers.");
        }

        if (root.GetComponentsInChildren<Collider>(true).Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " sidecar showcase must not add gameplay colliders.");
        }

        if (root.GetComponentsInChildren<Rigidbody>(true).Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " sidecar showcase must not add rigidbodies.");
        }

        if (root.GetComponentsInChildren<AudioSource>(true).Length > 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " sidecar showcase must not add autonomous audio sources.");
        }

        string[] requiredNames = GetRequiredSidecarShowcaseNames(sceneName);
        for (int i = 0; i < requiredNames.Length; i++)
        {
            RequireNamed("SidecarVisual_" + sceneName + "_" + requiredNames[i], sceneName + " required sidecar showcase asset " + requiredNames[i]);
        }

        string[] requiredSwatches = GetRequiredSidecarMaterialSwatches(sceneName);
        for (int i = 0; i < requiredSwatches.Length; i++)
        {
            RequireNamed("SidecarMaterialSwatch_" + sceneName + "_" + requiredSwatches[i], sceneName + " required sidecar material swatch " + requiredSwatches[i]);
        }
    }

    private static int GetMinimumSidecarShowcaseRendererCount(string sceneName)
    {
        switch (sceneName)
        {
            case "Level01":
                return 42;
            case "Level02":
            case "Level03":
            case "Level04":
            case "Level05":
                return 32;
            default:
                return 1;
        }
    }

    private static string[] GetRequiredSidecarShowcaseNames(string sceneName)
    {
        switch (sceneName)
        {
            case "Level01":
                return new[] { "PressurePistolCore", "SCK2NorthStarCorridor", "EE02PressureSpindleHarpoon", "WVM03PressurePistolFull", "WPS02PressurePistolFrame", "SCLDGaugePanelTriple", "OPS02KeyedLockTriGearVault", "BBSVFX02SteamVentSoftColumn", "SCLAPressureLampWallCagedA", "EAP01ScrapperAshcanIdleBrace" };
            case "Level02":
                return new[] { "CorridorStraight", "SCK2PressureGaugeColumn", "EE02AshcanSawScout", "WVM03BoltThrowerFull", "SCLDPipeJunctionX", "MEV01RivetLancerRail", "WPS02PressureCell", "OPS02ValvePanelTwinPressurePuzzle", "BBSVFX02PressureLeakRuptureCone", "SCLASteamPipeWallLeakerA", "EAP01LancerPressureAimLine" };
            case "Level03":
                return new[] { "TJunction", "SCK2RoomGaugeNest", "EE02AshcanOvercrank", "WVM03ScattergunFull", "MEV01SawScrapperBoiler", "WPS02ScattergunTwin", "SCLDDrainChannel", "OPS02LiftCallStationBrassCage", "BBSVFX02FurnaceBlastDoorBelch", "SCLAHangingChainsTripleSlack", "EAP01BulwarkHammerRaise" };
            case "Level04":
                return new[] { "ArchedDoor", "SCK2BulkheadDoor", "EE02GatehammerShielded", "WVM03AmmoShellStrip", "MEV01BulwarkShieldBoiler", "SCLDPressureTankFloor", "WPS02AmmoCabinetOpen", "OPS02ActuatorBridgeThrowLever", "BBSVFX02SparkRicochetWallHit", "SCLAOverheadPipeValveRun", "EAP01WardenGovernorSignalRaise" };
            case "Level05":
                return new[] { "VaultDoor", "SCK2PressureLockDoor", "EE02GovernorWardenTall", "WVM03GaugeClusterTriple", "MEV01WardenTall", "SCLDGearHousingOpen", "WPS02WallWeaponRack", "OPS02GovernorOverrideBossKillSwitch", "BBSVFX02BossPhaseGovernorOvercrank", "SCLADenseAmbienceCorridorBite", "EAP01ScrapperSawLunge" };
            default:
                return Array.Empty<string>();
        }
    }

    private static string[] GetRequiredSidecarMaterialSwatches(string sceneName)
    {
        switch (sceneName)
        {
            case "Level01":
                return new[] { "AgedBrass", "SCK2OilWetFloor", "EE02FurnaceEye", "WVM03AgedBrass", "OPS02RedOverrideEnamel", "BBSVFX02SteamDense", "SCLAAmberLampGlass", "EAP01AgedBrass" };
            case "Level02":
                return new[] { "WetStone", "SCK2PressureGreenGlass", "EE02ChippedHazardYellow", "WVM03BluedSpringSteel", "OPS02AgedBrass", "BBSVFX02PressureAmber", "SCLADullCopperPipe", "EAP01CyanPressurePilot" };
            case "Level03":
                return new[] { "HazardPaint", "SCK2DarkRivetedIron", "EE02SootBlackGrime", "WVM03VarnishedWalnut", "OPS02GreenSignalGlass", "BBSVFX02FurnaceOrange", "SCLAChainGunmetal", "EAP01FurnaceOrangeGlow" };
            case "Level04":
                return new[] { "OxidizedCopper", "SCK2BurnishedCopper", "EE02RedOverheatTell", "WVM03OxidizedCopper", "OPS02BurnishedCopper", "BBSVFX02HotSpark", "SCLAWarningRedNeedle", "EAP01BurnishedCopper" };
            case "Level05":
                return new[] { "PressureGaugeGlass", "SCK2GaugeIvory", "EE02CyanPressureTell", "WVM03GreenGaugeGlass", "OPS02PressureGlowCyan", "BBSVFX02BossBluePressure", "SCLAAgedBrassGlow", "EAP01RedHotIronSlit" };
            default:
                return Array.Empty<string>();
        }
    }

    private static void ValidateLorePlaques(string sceneName)
    {
        LorePlaque[] plaques = UnityEngine.Object.FindObjectsByType<LorePlaque>(FindObjectsSortMode.None);
        if (plaques.Length == 0)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " is missing a LorePlaque.");
        }

        foreach (LorePlaque plaque in plaques)
        {
            RequireTrigger(plaque.gameObject, sceneName + " lore plaque trigger " + plaque.name);
            RequireInteractable(plaque, sceneName + " lore plaque interactable " + plaque.name);

            if (string.IsNullOrWhiteSpace(plaque.plaqueId) || string.IsNullOrWhiteSpace(plaque.title) || string.IsNullOrWhiteSpace(plaque.body))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " lore plaque " + plaque.name + " is missing narrative text.");
            }
        }
    }

    private static void ValidateSignageDecalsV1(string sceneName)
    {
        if (sceneName == "Level01")
        {
            RequireSignageRoot(sceneName, "Signage Decals V1 - Level01", 10);
            RequireSignageDecal(sceneName, "OBJ-L01-01", "Gear Key Ahead", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "OBJ-L01-02", "Pressure Gate", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "OBJ-L01-03", "Service Lift", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "HAZ-L01-01", "Pressure Locked", SignageWarningTexturePath);
            RequireSignageDecal(sceneName, "HAZ-L01-03", "Gate Crush", SignageWarningTexturePath);
            RequireSignageDecal(sceneName, "ARR-L01-01", "To Key", SignageRouteTexturePath);
            RequireSignageDecal(sceneName, "ARR-L01-03", "To Lift", SignageRouteTexturePath);
            RequireSignageDecal(sceneName, "SEC-L01-01", "Warm Seam", SignageSecretTexturePath);
            RequireSignageDecal(sceneName, "SEC-L01-02", "Three Rivets Out", SignageSecretTexturePath);
        }
        else if (sceneName == "Level03")
        {
            RequireSignageRoot(sceneName, "Signage Decals V1 - Level03", 12);
            RequireSignageDecal(sceneName, "OBJ-L03-01", "Vent Core Pressure", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "OBJ-L03-02", "Pressure Valve", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "OBJ-L03-03", "Foundry Lift", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "HAZ-L03-01", "Furnace Leak", SignageWarningTexturePath);
            RequireSignageDecal(sceneName, "HAZ-L03-02", "Core Bleed", SignageWarningTexturePath);
            RequireSignageDecal(sceneName, "HAZ-L03-03", "Pressure Pulse", SignageWarningTexturePath);
            RequireSignageDecal(sceneName, "ARR-L03-01", "To Valve", SignageRouteTexturePath);
            RequireSignageDecal(sceneName, "ARR-L03-02", "To Tool", SignageRouteTexturePath);
            RequireSignageDecal(sceneName, "ARR-L03-03", "To Foundry", SignageRouteTexturePath);
            RequireSignageDecal(sceneName, "MAC-L03-01", "Boilerheart Core", SignageStencilTexturePath);
            RequireSignageDecal(sceneName, "SEC-L03-01", "Gauge Lies", SignageSecretTexturePath);
        }
        else if (sceneName == "Level05")
        {
            RequireSignageRoot(sceneName, "Signage Decals V1 - Level05", 12);
            RequireSignageDecal(sceneName, "OBJ-L05-01", "Regulator Ring", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "OBJ-L05-02", "Warden Lock", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "OBJ-L05-03", "Master Override", SignageObjectiveTexturePath);
            RequireSignageDecal(sceneName, "HAZ-L05-01", "Regulator Leak", SignageWarningTexturePath);
            RequireSignageDecal(sceneName, "HAZ-L05-02", "Surge Floor", SignageWarningTexturePath);
            RequireSignageDecal(sceneName, "HAZ-L05-03", "Warden Active", SignageWarningTexturePath);
            RequireSignageDecal(sceneName, "ARR-L05-01", "To Core", SignageRouteTexturePath);
            RequireSignageDecal(sceneName, "ARR-L05-02", "To Override", SignageRouteTexturePath);
            RequireSignageDecal(sceneName, "ARR-L05-03", "Green Hoist", SignageRouteTexturePath);
            RequireSignageDecal(sceneName, "MAC-L05-01", "Governor Core", SignageStencilTexturePath);
            RequireSignageDecal(sceneName, "SEC-L05-01", "Wrong Clerk Tag", SignageSecretTexturePath);
        }
    }

    private static void RequireSignageRoot(string sceneName, string rootName, int minimumChildren)
    {
        GameObject root = RequireNamed(rootName, sceneName + " SignageDecalsV1 root");
        if (root.transform.childCount < minimumChildren)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " SignageDecalsV1 root has only " + root.transform.childCount + " decals.");
        }
    }

    private static void RequireSignageDecal(string sceneName, string id, string label, string expectedTexturePath)
    {
        string objectName = "Signage Decal - " + id + " - " + label;
        GameObject decal = RequireNamed(objectName, sceneName + " SignageDecalsV1 decal " + id);

        MeshFilter meshFilter = decal.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null || meshFilter.sharedMesh.uv == null || meshFilter.sharedMesh.uv.Length < 4)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " signage decal " + id + " has no sliced quad mesh.");
        }

        Renderer renderer = decal.GetComponent<Renderer>();
        if (renderer == null || renderer.sharedMaterial == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " signage decal " + id + " has no material.");
        }

        Material material = renderer.sharedMaterial;
        if (!material.name.Contains("M_SignageDecalsV1_" + id))
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " signage decal " + id + " has the wrong material name.");
        }

        Texture texture = material.mainTexture;
        if (texture == null && material.HasProperty("_BaseMap"))
        {
            texture = material.GetTexture("_BaseMap");
        }

        if (texture == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " signage decal " + id + " has no atlas texture.");
        }

        string texturePath = AssetDatabase.GetAssetPath(texture).Replace("\\", "/");
        if (texturePath != expectedTexturePath)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " signage decal " + id + " expected texture " + expectedTexturePath + " but found " + texturePath + ".");
        }
    }

    private static void RequireImageSprite(string sceneName, string objectName, string expectedRelativePath)
    {
        GameObject gameObject = RequireNamed(objectName, sceneName + " UIHudV1 object " + objectName);
        Image image = gameObject.GetComponent<Image>();
        if (image == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " UIHudV1 object " + objectName + " has no Image component.");
        }

        if (image.sprite == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " UIHudV1 object " + objectName + " has no sprite.");
        }

        string expectedPath = UIHudRoot + "/" + expectedRelativePath;
        string actualPath = AssetDatabase.GetAssetPath(image.sprite).Replace("\\", "/");
        if (actualPath != expectedPath)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " UIHudV1 object " + objectName + " expected " + expectedPath + " but found " + actualPath + ".");
        }
    }

    private static void RequireCollider(GameObject gameObject, string label)
    {
        if (gameObject.GetComponent<Collider>() == null)
        {
            throw new InvalidOperationException("Level validation failed: missing " + label + ".");
        }
    }

    private static void RequireTrigger(GameObject gameObject, string label)
    {
        Collider trigger = gameObject.GetComponent<Collider>();
        if (trigger == null || !trigger.isTrigger)
        {
            throw new InvalidOperationException("Level validation failed: missing trigger collider for " + label + ".");
        }
    }

    private static void RequireInteractable(IInteractable interactable, string label)
    {
        if (interactable == null)
        {
            throw new InvalidOperationException("Level validation failed: missing " + label + ".");
        }

        if (string.IsNullOrWhiteSpace(interactable.Prompt))
        {
            throw new InvalidOperationException("Level validation failed: " + label + " has no prompt.");
        }
    }

    private static void RequireEqual(int actual, int expected, string label)
    {
        if (actual != expected)
        {
            throw new InvalidOperationException("Level validation failed: " + label + " expected " + expected + " but found " + actual + ".");
        }
    }

    private static void RequireApprox(float actual, float expected, string label)
    {
        if (!Mathf.Approximately(actual, expected))
        {
            throw new InvalidOperationException("Level validation failed: " + label + " expected " + expected + " but found " + actual + ".");
        }
    }
}
