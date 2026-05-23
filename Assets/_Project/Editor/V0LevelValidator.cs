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
        PlayerController playerController = Require<PlayerController>(sceneName + " PlayerController");
        Require<PlayerHealth>(sceneName + " PlayerHealth");
        PlayerInventory playerInventory = Require<PlayerInventory>(sceneName + " PlayerInventory");
        WeaponController weaponController = Require<WeaponController>(sceneName + " WeaponController");
        ValidateBalanceValues(sceneName, playerController, playerInventory, weaponController);
        ValidateWeaponVisuals(sceneName);
        Require<GameStateController>(sceneName + " GameStateController");
        Require<RuntimePerformanceProfile>(sceneName + " RuntimePerformanceProfile");
        Require<HUDController>(sceneName + " HUDController");
        Require<PauseMenuController>(sceneName + " PauseMenuController");
        Require<EnemyController>(sceneName + " EnemyController");
        Require<Pickup>(sceneName + " Pickup");

        ValidatePickups(sceneName);
        ValidateEnemies(sceneName);
        ValidateEnvironmentPropVisuals(sceneName);

        if (requirePressureGate)
        {
            LockedDoor door = Require<LockedDoor>(sceneName + " LockedDoor");
            RequireCollider(door.gameObject, sceneName + " LockedDoor collider");
            RequireNamed("Pressure Gate Frame Assembly", sceneName + " pressure gate frame visual");
            RequireNamed("Pressure Gate Key Socket", sceneName + " pressure gate key socket visual");
            RequireNamed("Pressure Gate Warning Lamp Left", sceneName + " pressure gate warning lamp visual");
            RequireNamed("Pickup - Gear Key Clockwork Key Visual", sceneName + " gear-key visual root");
            RequireNamed("Pickup - Gear Key Key Bit Lower", sceneName + " gear-key bit visual");
        }

        if (requireTransition)
        {
            LevelTransitionTrigger transition = Require<LevelTransitionTrigger>(sceneName + " LevelTransitionTrigger");
            RequireTrigger(transition.gameObject, sceneName + " LevelTransitionTrigger trigger");
            ValidateServiceLiftVisuals(transition.gameObject, sceneName + " transition lift");
            if (string.IsNullOrWhiteSpace(transition.targetSceneName))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " transition has no target scene.");
            }
        }

        if (requireFinalExit)
        {
            ExitTrigger exit = Require<ExitTrigger>(sceneName + " ExitTrigger");
            RequireTrigger(exit.gameObject, sceneName + " ExitTrigger trigger");
            ValidateServiceLiftVisuals(exit.gameObject, sceneName + " final service lift");
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

            RequireEqual(enemy.maxHealth, GameBalance.ScrapperHealth, sceneName + " Scrapper health balance");
            RequireApprox(enemy.moveSpeed, GameBalance.ScrapperMoveSpeed, sceneName + " Scrapper speed balance");
            RequireApprox(enemy.detectionRange, GameBalance.ScrapperDetectionRange, sceneName + " Scrapper detection balance");
            RequireEqual(enemy.attackDamage, GameBalance.ScrapperAttackDamage, sceneName + " Scrapper damage balance");
            RequireApprox(enemy.attackWindup, GameBalance.ScrapperAttackWindup, sceneName + " Scrapper windup balance");
            RequireApprox(enemy.obstacleProbeDistance, GameBalance.ScrapperObstacleProbeDistance, sceneName + " Scrapper obstacle probe balance");
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
        }
    }

    private static void ValidateBalanceValues(string sceneName, PlayerController playerController, PlayerInventory playerInventory, WeaponController weaponController)
    {
        RequireApprox(playerController.moveSpeed, GameBalance.PlayerMoveSpeed, sceneName + " player speed balance");
        RequireEqual(playerInventory.startingAmmo, GameBalance.StartingAmmo, sceneName + " starting ammo balance");
        RequireEqual(weaponController.damage, GameBalance.PressurePistolDamage, sceneName + " pistol damage balance");
        RequireApprox(weaponController.fireCooldown, GameBalance.PressurePistolCooldown, sceneName + " pistol cooldown balance");
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
        RequireNamed("Pressure Pistol Front Sight", sceneName + " pressure pistol front sight visual");
    }

    private static void ValidateEnvironmentPropVisuals(string sceneName)
    {
        if (sceneName == "Level01")
        {
            RequireNamed("Work Order Board - Intake", sceneName + " intake work-order board visual");
            RequireNamed("Work Order Board - Gate", sceneName + " gate work-order board visual");
            RequireNamed("Pipe Bundle - Gate Manifold", sceneName + " gate pipe-bundle visual");
        }
        else if (sceneName == "Level02")
        {
            RequireNamed("Work Order Board - Pipeworks", sceneName + " pipeworks work-order board visual");
            RequireNamed("Pipeworks Triple Pipe Bundle", sceneName + " pipeworks pipe-bundle visual");
        }

        if (sceneName == "Level01")
        {
            RequireNamed("Repair Bay Cover Boiler Left", sceneName + " repair bay cover visual");
            RequireNamed("Repair Bay Cover Crate Right", sceneName + " repair bay cover visual");
            RequireNamed("Final Room Cover Stack West", sceneName + " final room cover visual");
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
