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

        Material wallMaterial = CreateMaterial("M_Greybox_Wall", new Color(0.48f, 0.48f, 0.5f));
        Material floorMaterial = CreateMaterial("M_Greybox_Floor", new Color(0.18f, 0.18f, 0.2f));
        Material doorMaterial = CreateMaterial("M_Greybox_RedDoor", new Color(0.75f, 0.08f, 0.05f));
        Material keyMaterial = CreateMaterial("M_Greybox_Key", new Color(1f, 0.82f, 0.05f));
        Material exitMaterial = CreateMaterial("M_Greybox_Exit", new Color(0.1f, 0.9f, 0.25f));
        Material enemyMaterial = CreateMaterial("M_Greybox_Enemy", new Color(1f, 0.22f, 0.05f));
        Material enemyEyeMaterial = CreateMaterial("M_Greybox_EnemyEyes", new Color(1f, 0.95f, 0.25f));
        Material healthMaterial = CreateMaterial("M_Greybox_Health", new Color(0.95f, 0.1f, 0.1f));
        Material ammoMaterial = CreateMaterial("M_Greybox_Ammo", new Color(0.15f, 0.45f, 1f));
        Material gunMaterial = CreateMaterial("M_Greybox_Gun", new Color(0.08f, 0.08f, 0.09f));
        Material gunTrimMaterial = CreateMaterial("M_Greybox_GunTrim", new Color(0.42f, 0.42f, 0.46f));
        Material muzzleFlashMaterial = CreateMaterial("M_Greybox_MuzzleFlash", new Color(1f, 0.72f, 0.08f));
        Material cyanGuideMaterial = CreateMaterial("M_Greybox_CyanGuide", new Color(0.05f, 0.85f, 1f));
        Material magentaGuideMaterial = CreateMaterial("M_Greybox_MagentaGuide", new Color(1f, 0.08f, 0.7f));

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.55f);

        CreateLighting();
        CreateGreyboxLevel(wallMaterial, floorMaterial);
        HUDController hud = CreateHud();
        CreateGameState(hud);
        CreatePlayer(gunMaterial, gunTrimMaterial, muzzleFlashMaterial);
        CreateEnemy("Enemy - First Room", new Vector3(0f, 1f, 16.5f), enemyMaterial, enemyEyeMaterial);
        CreateEnemy("Enemy - Key Room", new Vector3(14.5f, 1f, 17f), enemyMaterial, enemyEyeMaterial);
        CreateEnemy("Enemy - Final Left", new Vector3(-3.2f, 1f, 30.5f), enemyMaterial, enemyEyeMaterial);
        CreateEnemy("Enemy - Final Right", new Vector3(3.2f, 1f, 32.5f), enemyMaterial, enemyEyeMaterial);
        CreatePickup("Pickup - Health", PickupKind.Health, new Vector3(-3.6f, 0.45f, 20f), Vector3.one * 0.7f, healthMaterial, 25);
        CreatePickup("Pickup - Ammo", PickupKind.Ammo, new Vector3(4.2f, 0.45f, 19f), Vector3.one * 0.7f, ammoMaterial, 15);
        CreatePickup("Pickup - Access Shard", PickupKind.Key, new Vector3(16f, 0.55f, 17f), Vector3.one * 0.9f, keyMaterial, 0);
        CreateLockedDoor(doorMaterial);
        CreateExit(exitMaterial);
        CreateAccentLights();
        CreateObjectiveGuides(cyanGuideMaterial, magentaGuideMaterial, keyMaterial, exitMaterial);

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
        RequireObject<CyberpunkAudio>("CyberpunkAudio");
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

        string buildDirectory = Path.Combine(Directory.GetCurrentDirectory(), WindowsBuildFolder, GameBranding.CheckpointVersion);
        Directory.CreateDirectory(buildDirectory);

        string executablePath = Path.Combine(buildDirectory, GameBranding.ExecutableStem + "_" + GameBranding.CheckpointVersion + ".exe");

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
        GameObject parent = new GameObject("Aster Gate Intake Blockout");

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
        hud.healthText = CreateText("Health Text", canvasObject.transform, font, "HEALTH 100/100", 24, TextAnchor.LowerLeft, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(18f, 16f), new Vector2(360f, 50f));
        hud.ammoText = CreateText("Ammo Text", canvasObject.transform, font, "AMMO 30", 24, TextAnchor.LowerRight, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-18f, 16f), new Vector2(360f, 50f));
        hud.keyText = CreateText("Access Shard Text", canvasObject.transform, font, "SHARD NO", 22, TextAnchor.LowerCenter, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 18f), new Vector2(260f, 45f));
        CreateText("Crosshair", canvasObject.transform, font, "+", 34, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(80f, 80f));
        hud.messageText = CreateText("Message Text", canvasObject.transform, font, string.Empty, 34, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 80f), new Vector2(760f, 220f));

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();

        return hud;
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
        stateObject.AddComponent<CyberpunkAudio>();
        stateObject.AddComponent<RuntimeSmokeTest>();
    }

    private static void CreatePlayer(Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial)
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

        WeaponView weaponView = CreateWeaponView(cameraObject.transform, gunMaterial, gunTrimMaterial, muzzleFlashMaterial);
        weapon.weaponView = weaponView;
    }

    private static WeaponView CreateWeaponView(Transform cameraTransform, Material gunMaterial, Material gunTrimMaterial, Material muzzleFlashMaterial)
    {
        GameObject weaponRoot = new GameObject("Pulse Pistol Placeholder");
        weaponRoot.transform.SetParent(cameraTransform, false);
        weaponRoot.transform.localPosition = new Vector3(0f, -0.55f, 0.82f);
        weaponRoot.transform.localRotation = Quaternion.identity;

        CreateLocalCube("Gun Body", weaponRoot.transform, new Vector3(0f, 0f, 0f), new Vector3(0.42f, 0.22f, 0.42f), gunMaterial);
        CreateLocalCube("Gun Barrel", weaponRoot.transform, new Vector3(0f, 0.04f, 0.36f), new Vector3(0.2f, 0.16f, 0.5f), gunTrimMaterial);
        CreateLocalCube("Gun Grip", weaponRoot.transform, new Vector3(0f, -0.24f, -0.12f), new Vector3(0.2f, 0.36f, 0.18f), gunMaterial);
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
    }

    private static void CreateObjectiveGuides(Material cyanMaterial, Material magentaMaterial, Material keyMaterial, Material exitMaterial)
    {
        CreateCube("Access Shard Pedestal", new Vector3(16f, 0.15f, 17f), new Vector3(1.35f, 0.3f, 1.35f), cyanMaterial);
        CreateCube("Gate Warning Floor Strip", new Vector3(0f, 0.015f, 21.25f), new Vector3(3.4f, 0.03f, 0.28f), magentaMaterial);
        CreateCube("Emergency Lift Floor Strip", new Vector3(0f, 0.015f, 33.15f), new Vector3(3.6f, 0.03f, 0.28f), exitMaterial);
        CreateCube("Shard Route Floor Strip", new Vector3(8.1f, 0.015f, 17f), new Vector3(3.6f, 0.03f, 0.22f), keyMaterial);

        CreateWorldLabel("Label - Access Shard", "ACCESS SHARD", new Vector3(16f, 2.2f, 16.25f), new Color(1f, 0.85f, 0.15f), 0.28f);
        CreateWorldLabel("Label - Lockdown Gate", "LOCKDOWN: SHARD REQUIRED", new Vector3(0f, 2.9f, 21.95f), new Color(1f, 0.08f, 0.7f), 0.22f);
        CreateWorldLabel("Label - Emergency Lift", "EMERGENCY LIFT", new Vector3(0f, 2.75f, 33.95f), new Color(0.2f, 1f, 0.45f), 0.26f);
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

    private static void CreateLockedDoor(Material material)
    {
        GameObject door = CreateCube("Corporate Lockdown Gate", new Vector3(0f, 1.5f, 22.5f), new Vector3(3f, 3f, 0.5f), material);
        LockedDoor lockedDoor = door.AddComponent<LockedDoor>();
        lockedDoor.openDistance = 2.3f;
    }

    private static void CreateExit(Material material)
    {
        GameObject exit = CreateCube("Emergency Lift Trigger", new Vector3(0f, 1.1f, 34.6f), new Vector3(2.4f, 2.2f, 0.35f), material);
        Collider exitCollider = exit.GetComponent<Collider>();
        if (exitCollider != null)
        {
            exitCollider.isTrigger = true;
        }

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
