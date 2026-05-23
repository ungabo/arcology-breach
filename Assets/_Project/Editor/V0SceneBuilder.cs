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
    private const string ScenePath = "Assets/_Project/Scenes/Level01.unity";
    private const string MaterialFolder = "Assets/_Project/Materials";
    private const string WindowsBuildFolder = "Builds/Windows";

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
        Material ammoMaterial = CreateMaterial("M_Greybox_Ammo", new Color(0.15f, 0.45f, 1f));
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

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.55f);

        CreateLighting();
        CreateGreyboxLevel(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud);
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial);
        CreateEnemy("Enemy - First Room", new Vector3(0f, 1f, 16.5f), enemyMaterial, enemyEyeMaterial);
        CreateEnemy("Enemy - Key Room", new Vector3(14.5f, 1f, 17f), enemyMaterial, enemyEyeMaterial);
        CreateEnemy("Enemy - Final Left", new Vector3(-3.2f, 1f, 30.5f), enemyMaterial, enemyEyeMaterial);
        CreateEnemy("Enemy - Final Right", new Vector3(3.2f, 1f, 32.5f), enemyMaterial, enemyEyeMaterial);
        CreatePickup("Pickup - Health", PickupKind.Health, new Vector3(-3.6f, 0.45f, 20f), Vector3.one * 0.7f, healthMaterial, 25);
        CreatePickup("Pickup - Ammo", PickupKind.Ammo, new Vector3(4.2f, 0.45f, 19f), Vector3.one * 0.7f, ammoMaterial, 15);
        CreateGearKeyPickup("Pickup - Gear Key", new Vector3(16f, 0.55f, 17f), Vector3.one * 1.1f, keyMaterial, rivetedIronMaterial);
        CreateLockedDoor(doorMaterial, brassGuideMaterial, gaugeFaceMaterial, pressureWarningMaterial);
        CreateExit(exitMaterial, rivetedIronMaterial, brassGuideMaterial, gaugeFaceMaterial);
        CreateAccentLights();
        CreateObjectiveGuides(brassGuideMaterial, pressureWarningMaterial, keyMaterial, exitMaterial);
        CreateSteamworksDressing(rivetedIronMaterial, oilStoneMaterial, brassGuideMaterial, pressureWarningMaterial, brassHazardMaterial, gaugeFaceMaterial, steamPuffMaterial, furnaceGlowMaterial);

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ScenePath);
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(ScenePath, true)
        };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("V0 scene rebuilt at " + ScenePath);
    }

    public static void RunSmokeTest()
    {
        if (!File.Exists(ScenePath))
        {
            throw new FileNotFoundException("Missing v0 scene", ScenePath);
        }

        EditorSceneManager.OpenScene(ScenePath);

        RequireObject<PlayerController>("PlayerController");
        RequireObject<PlayerHealth>("PlayerHealth");
        RequireObject<PlayerInventory>("PlayerInventory");
        RequireObject<WeaponController>("WeaponController");
        RequireObject<GameStateController>("GameStateController");
        RequireObject<PauseMenuController>("PauseMenuController");
        RequireObject<SteamworksAudio>("SteamworksAudio");
        RequireObject<RuntimeAutoPlaythroughTest>("RuntimeAutoPlaythroughTest");
        RequireObject<RuntimeCombatTest>("RuntimeCombatTest");
        RequireObject<RuntimePauseFlowTest>("RuntimePauseFlowTest");
        RequireObject<HUDController>("HUDController");
        RequireObject<EnemyController>("EnemyController");
        RequireObject<Pickup>("Pickup");
        RequireObject<LockedDoor>("LockedDoor");
        RequireObject<ExitTrigger>("ExitTrigger");

        if (EditorBuildSettings.scenes.Length == 0 || EditorBuildSettings.scenes[0].path != ScenePath)
        {
            throw new InvalidOperationException("Level01 is not the first enabled build scene.");
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
            scenes = new[] { ScenePath },
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
            MaterialFolder
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

    private static void CreateGameState(HUDController hud)
    {
        GameObject stateObject = new GameObject("Game State");
        GameStateController state = stateObject.AddComponent<GameStateController>();
        state.hud = hud;
        state.pauseMenu = UnityEngine.Object.FindAnyObjectByType<PauseMenuController>();
        stateObject.AddComponent<SteamworksAudio>();
        stateObject.AddComponent<RuntimeSmokeTest>();
        stateObject.AddComponent<RuntimeAutoPlaythroughTest>();
        stateObject.AddComponent<RuntimeCombatTest>();
        stateObject.AddComponent<RuntimePauseFlowTest>();
    }

    private static void CreatePlayer(Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material gaugeFaceMaterial)
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

        PlayerHealth health = player.AddComponent<PlayerHealth>();
        health.maxHealth = 100;

        PlayerInventory inventory = player.AddComponent<PlayerInventory>();
        inventory.startingAmmo = 35;

        WeaponController weapon = player.AddComponent<WeaponController>();
        weapon.aimCamera = camera;
        weapon.inventory = inventory;
        weapon.damage = 25;
        weapon.fireCooldown = 0.23f;

        WeaponView weaponView = CreateWeaponView(cameraObject.transform, gunMaterial, gunTrimMaterial, muzzleFlashMaterial, gaugeFaceMaterial);
        weapon.weaponView = weaponView;
    }

    private static WeaponView CreateWeaponView(Transform cameraTransform, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial, Material gaugeFaceMaterial)
    {
        GameObject weaponRoot = new GameObject("Pressure Pistol Placeholder");
        weaponRoot.transform.SetParent(cameraTransform, false);
        weaponRoot.transform.localPosition = new Vector3(0f, -0.55f, 0.82f);
        weaponRoot.transform.localRotation = Quaternion.identity;

        CreateLocalCube("Pressure Pistol Body", weaponRoot.transform, new Vector3(0f, 0f, 0f), new Vector3(0.42f, 0.22f, 0.42f), gunMaterial);
        CreateLocalCube("Pressure Pistol Brass Barrel", weaponRoot.transform, new Vector3(0f, 0.04f, 0.36f), new Vector3(0.2f, 0.16f, 0.5f), gunTrimMaterial);
        CreateLocalCube("Pressure Pistol Walnut Grip", weaponRoot.transform, new Vector3(0f, -0.24f, -0.12f), new Vector3(0.2f, 0.36f, 0.18f), gunMaterial);
        GameObject gauge = CreateLocalPrimitive("Pressure Pistol Gauge", PrimitiveType.Cylinder, weaponRoot.transform, new Vector3(0f, 0.17f, 0.08f), new Vector3(0.18f, 0.035f, 0.18f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Pressure Pistol Gauge Needle", weaponRoot.transform, new Vector3(0.03f, 0.17f, 0.055f), new Vector3(0.12f, 0.012f, 0.012f), gunTrimMaterial);
        GameObject flash = CreateLocalCube("Muzzle Flash", weaponRoot.transform, new Vector3(0f, 0.04f, 0.68f), new Vector3(0.45f, 0.45f, 0.08f), muzzleFlashMaterial);
        flash.SetActive(false);

        WeaponView weaponView = weaponRoot.AddComponent<WeaponView>();
        weaponView.muzzleFlash = flash;
        return weaponView;
    }

    private static void CreateEnemy(string name, Vector3 position, Material material, Material eyeMaterial)
    {
        GameObject enemy = new GameObject(name);
        enemy.name = name;
        enemy.transform.position = position;

        CreateLocalPrimitive("Body", PrimitiveType.Capsule, enemy.transform, Vector3.zero, Vector3.one, material);
        CreateLocalCube("Left Eye", enemy.transform, new Vector3(-0.16f, 0.35f, 0.43f), new Vector3(0.12f, 0.1f, 0.04f), eyeMaterial);
        CreateLocalCube("Right Eye", enemy.transform, new Vector3(0.16f, 0.35f, 0.43f), new Vector3(0.12f, 0.1f, 0.04f), eyeMaterial);

        CharacterController controller = enemy.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.42f;
        controller.center = Vector3.zero;

        EnemyController enemyController = enemy.AddComponent<EnemyController>();
        enemyController.maxHealth = 50;
        enemyController.moveSpeed = 2.65f;
        enemyController.detectionRange = 13f;
        enemyController.attackDamage = 9;
        enemyController.attackWindup = 0.42f;
        enemyController.obstacleProbeDistance = 1.15f;
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

    private static void CreatePickup(string name, PickupKind kind, Vector3 position, Vector3 scale, Material material, int amount)
    {
        GameObject pickup = CreateCube(name, position, scale, material);
        Collider pickupCollider = pickup.GetComponent<Collider>();
        if (pickupCollider != null)
        {
            pickupCollider.isTrigger = true;
        }

        Pickup pickupComponent = pickup.AddComponent<Pickup>();
        pickupComponent.kind = kind;
        pickupComponent.amount = amount;
    }

    private static void CreateGearKeyPickup(string name, Vector3 position, Vector3 scale, Material brassMaterial, Material ironMaterial)
    {
        GameObject pickup = new GameObject(name);
        pickup.transform.position = position;

        BoxCollider trigger = pickup.AddComponent<BoxCollider>();
        trigger.size = scale;
        trigger.isTrigger = true;

        Pickup pickupComponent = pickup.AddComponent<Pickup>();
        pickupComponent.kind = PickupKind.Key;
        pickupComponent.amount = 0;

        CreateLocalPrimitive(name + " Gear Disc", PrimitiveType.Cylinder, pickup.transform, Vector3.zero, new Vector3(0.42f, 0.08f, 0.42f), brassMaterial);
        CreateLocalPrimitive(name + " Iron Hub", PrimitiveType.Cylinder, pickup.transform, new Vector3(0f, 0.07f, 0f), new Vector3(0.16f, 0.16f, 0.16f), ironMaterial);
        CreateLocalCube(name + " Key Shaft", pickup.transform, new Vector3(0f, -0.02f, 0.52f), new Vector3(0.16f, 0.12f, 0.74f), brassMaterial);
        CreateLocalCube(name + " Key Bit", pickup.transform, new Vector3(0.18f, -0.02f, 0.86f), new Vector3(0.34f, 0.12f, 0.16f), brassMaterial);

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            float radians = angle * Mathf.Deg2Rad;
            Vector3 toothPosition = new Vector3(Mathf.Sin(radians) * 0.43f, 0f, Mathf.Cos(radians) * 0.43f);
            GameObject tooth = CreateLocalCube(name + " Tooth " + i, pickup.transform, toothPosition, new Vector3(0.16f, 0.1f, 0.16f), brassMaterial);
            tooth.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private static void CreateLockedDoor(Material material, Material brassMaterial, Material gaugeFaceMaterial, Material warningMaterial)
    {
        GameObject door = CreateCube("Pressure Gate", new Vector3(0f, 1.5f, 22.5f), new Vector3(3f, 3f, 0.5f), material);
        LockedDoor lockedDoor = door.AddComponent<LockedDoor>();
        lockedDoor.openDistance = 2.3f;

        CreateLocalCube("Pressure Gate Upper Brass Rail", door.transform, new Vector3(0f, 0.92f, -0.28f), new Vector3(1.05f, 0.06f, 0.08f), brassMaterial);
        CreateLocalCube("Pressure Gate Lower Brass Rail", door.transform, new Vector3(0f, -0.92f, -0.28f), new Vector3(1.05f, 0.06f, 0.08f), brassMaterial);
        CreateLocalCube("Pressure Gate Left Brace", door.transform, new Vector3(-0.36f, 0f, -0.28f), new Vector3(0.07f, 1.62f, 0.08f), brassMaterial);
        CreateLocalCube("Pressure Gate Right Brace", door.transform, new Vector3(0.36f, 0f, -0.28f), new Vector3(0.07f, 1.62f, 0.08f), brassMaterial);

        GameObject gear = CreateLocalPrimitive("Pressure Gate Gear Wheel", PrimitiveType.Cylinder, door.transform, new Vector3(0f, 0.04f, -0.31f), new Vector3(0.36f, 0.045f, 0.36f), brassMaterial);
        gear.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject gauge = CreateLocalPrimitive("Pressure Gate Gauge Face", PrimitiveType.Cylinder, door.transform, new Vector3(-0.32f, 0.58f, -0.32f), new Vector3(0.18f, 0.025f, 0.18f), gaugeFaceMaterial);
        gauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        CreateLocalCube("Pressure Gate Gauge Needle", door.transform, new Vector3(-0.28f, 0.58f, -0.35f), new Vector3(0.12f, 0.014f, 0.014f), warningMaterial);
    }

    private static void CreateExit(Material material, Material ironMaterial, Material brassMaterial, Material gaugeFaceMaterial)
    {
        GameObject exit = CreateCube("Service Lift Trigger", new Vector3(0f, 1.1f, 34.6f), new Vector3(2.4f, 2.2f, 0.35f), material);
        Collider exitCollider = exit.GetComponent<Collider>();
        if (exitCollider != null)
        {
            exitCollider.isTrigger = true;
        }

        CreateLocalCube("Service Lift Cage Top", exit.transform, new Vector3(0f, 0.55f, -0.24f), new Vector3(1.1f, 0.08f, 0.08f), ironMaterial);
        CreateLocalCube("Service Lift Cage Bottom", exit.transform, new Vector3(0f, -0.55f, -0.24f), new Vector3(1.1f, 0.08f, 0.08f), ironMaterial);
        CreateLocalCube("Service Lift Left Rail", exit.transform, new Vector3(-0.42f, 0f, -0.24f), new Vector3(0.05f, 1.1f, 0.08f), brassMaterial);
        CreateLocalCube("Service Lift Right Rail", exit.transform, new Vector3(0.42f, 0f, -0.24f), new Vector3(0.05f, 1.1f, 0.08f), brassMaterial);
        GameObject liftGauge = CreateLocalPrimitive("Service Lift Pressure Gauge", PrimitiveType.Cylinder, exit.transform, new Vector3(0f, 0.25f, -0.27f), new Vector3(0.18f, 0.025f, 0.18f), gaugeFaceMaterial);
        liftGauge.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        exit.AddComponent<ExitTrigger>();
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
