using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class V0LevelValidator
{
    private const string MainMenuScenePath = "Assets/_Project/Scenes/MainMenu.unity";
    private const string Level01ScenePath = "Assets/_Project/Scenes/Level01.unity";
    private const string Level02ScenePath = "Assets/_Project/Scenes/Level02.unity";

    [MenuItem("Project Tools/Validate v0 Levels")]
    public static void RunValidation()
    {
        ValidateProjectScenes();
        Debug.Log("V0_LEVEL_VALIDATION_PASS");
    }

    public static void ValidateProjectScenes()
    {
        ValidateBuildSceneOrder();

        EditorSceneManager.OpenScene(MainMenuScenePath);
        Require<MainMenuController>("MainMenuController");
        Require<RuntimePerformanceProfile>("MainMenu RuntimePerformanceProfile");

        EditorSceneManager.OpenScene(Level01ScenePath);
        ValidateGameplayScene("Level01", requirePressureGate: true, requireTransition: true, requireFinalExit: false, requireRangedEnemy: false);

        EditorSceneManager.OpenScene(Level02ScenePath);
        ValidateGameplayScene("Level02", requirePressureGate: false, requireTransition: false, requireFinalExit: true, requireRangedEnemy: true);
    }

    private static void ValidateBuildSceneOrder()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        if (scenes.Length < 3 || scenes[0].path != MainMenuScenePath || scenes[1].path != Level01ScenePath || scenes[2].path != Level02ScenePath)
        {
            throw new InvalidOperationException("Level validation failed: build scenes must be MainMenu, Level01, Level02.");
        }
    }

    private static void ValidateGameplayScene(string sceneName, bool requirePressureGate, bool requireTransition, bool requireFinalExit, bool requireRangedEnemy)
    {
        Require<PlayerController>(sceneName + " PlayerController");
        Require<PlayerHealth>(sceneName + " PlayerHealth");
        Require<PlayerInventory>(sceneName + " PlayerInventory");
        Require<WeaponController>(sceneName + " WeaponController");
        Require<GameStateController>(sceneName + " GameStateController");
        Require<RuntimePerformanceProfile>(sceneName + " RuntimePerformanceProfile");
        Require<HUDController>(sceneName + " HUDController");
        Require<PauseMenuController>(sceneName + " PauseMenuController");
        Require<EnemyController>(sceneName + " EnemyController");
        Require<Pickup>(sceneName + " Pickup");

        ValidatePickups(sceneName);
        ValidateEnemies(sceneName);

        if (requirePressureGate)
        {
            LockedDoor door = Require<LockedDoor>(sceneName + " LockedDoor");
            RequireCollider(door.gameObject, sceneName + " LockedDoor collider");
        }

        if (requireTransition)
        {
            LevelTransitionTrigger transition = Require<LevelTransitionTrigger>(sceneName + " LevelTransitionTrigger");
            RequireTrigger(transition.gameObject, sceneName + " LevelTransitionTrigger trigger");
            if (string.IsNullOrWhiteSpace(transition.targetSceneName))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " transition has no target scene.");
            }
        }

        if (requireFinalExit)
        {
            ExitTrigger exit = Require<ExitTrigger>(sceneName + " ExitTrigger");
            RequireTrigger(exit.gameObject, sceneName + " ExitTrigger trigger");
        }

        if (requireRangedEnemy)
        {
            RangedEnemyController ranged = Require<RangedEnemyController>(sceneName + " RangedEnemyController");
            if (ranged.muzzle == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " RangedEnemyController is missing muzzle.");
            }
        }
    }

    private static void ValidatePickups(string sceneName)
    {
        Pickup[] pickups = UnityEngine.Object.FindObjectsByType<Pickup>(FindObjectsSortMode.None);
        foreach (Pickup pickup in pickups)
        {
            RequireTrigger(pickup.gameObject, sceneName + " pickup trigger " + pickup.name);
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
        }

        RangedEnemyController[] rangedEnemies = UnityEngine.Object.FindObjectsByType<RangedEnemyController>(FindObjectsSortMode.None);
        foreach (RangedEnemyController enemy in rangedEnemies)
        {
            if (enemy.GetComponent<CharacterController>() == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Lancer missing CharacterController.");
            }
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
}
