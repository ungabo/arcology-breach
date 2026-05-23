using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class V0LevelValidator
{
    private const string MainMenuScenePath = "Assets/_Project/Scenes/MainMenu.unity";
    private const string Level01ScenePath = "Assets/_Project/Scenes/Level01.unity";
    private const string Level02ScenePath = "Assets/_Project/Scenes/Level02.unity";
    private const string Level03ScenePath = "Assets/_Project/Scenes/Level03.unity";

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
        RuntimePerformanceProfile mainMenuPerformanceProfile = Require<RuntimePerformanceProfile>("MainMenu RuntimePerformanceProfile");
        ValidatePlatformQualityProfile("MainMenu", mainMenuPerformanceProfile);

        EditorSceneManager.OpenScene(Level01ScenePath);
        ValidateGameplayScene("Level01", requirePressureGate: true, requireTransition: true, requireFinalExit: false, requireRangedEnemy: false);

        EditorSceneManager.OpenScene(Level02ScenePath);
        ValidateGameplayScene("Level02", requirePressureGate: false, requireTransition: true, requireFinalExit: false, requireRangedEnemy: true);

        EditorSceneManager.OpenScene(Level03ScenePath);
        ValidateGameplayScene("Level03", requirePressureGate: false, requireTransition: false, requireFinalExit: true, requireRangedEnemy: false);
    }

    private static void ValidateBuildSceneOrder()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        if (scenes.Length < 4 || scenes[0].path != MainMenuScenePath || scenes[1].path != Level01ScenePath || scenes[2].path != Level02ScenePath || scenes[3].path != Level03ScenePath)
        {
            throw new InvalidOperationException("Level validation failed: build scenes must be MainMenu, Level01, Level02, Level03.");
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
        Require<GameStateController>(sceneName + " GameStateController");
        Require<LevelTransitionController>(sceneName + " LevelTransitionController");
        RuntimePerformanceProfile performanceProfile = Require<RuntimePerformanceProfile>(sceneName + " RuntimePerformanceProfile");
        ValidatePlatformQualityProfile(sceneName, performanceProfile);
        HUDController hud = Require<HUDController>(sceneName + " HUDController");
        if (hud.interactionText == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " HUDController is missing interaction prompt text.");
        }

        Require<PauseMenuController>(sceneName + " PauseMenuController");
        Require<RuntimeInteractionTest>(sceneName + " RuntimeInteractionTest");
        Require<RuntimeCombatScenarioTest>(sceneName + " RuntimeCombatScenarioTest");
        Require<EnemyController>(sceneName + " EnemyController");
        Require<Pickup>(sceneName + " Pickup");

        ValidatePickups(sceneName);
        ValidateEnemies(sceneName);
        ValidateEnvironmentPropVisuals(sceneName);

        if (requirePressureGate)
        {
            LockedDoor door = Require<LockedDoor>(sceneName + " LockedDoor");
            RequireCollider(door.gameObject, sceneName + " LockedDoor collider");
            RequireInteractable(door, sceneName + " pressure gate interactable");
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
            RequireInteractable(transition, sceneName + " service lift interactable");
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
            RequireInteractable(exit, sceneName + " final lift interactable");
            ValidateServiceLiftVisuals(exit.gameObject, sceneName + " final service lift");
            if (sceneName == "Level03")
            {
                SteamValveObjective valve = Require<SteamValveObjective>(sceneName + " SteamValveObjective");
                RequireTrigger(valve.gameObject, sceneName + " SteamValveObjective trigger");
                RequireInteractable(valve, sceneName + " boilerheart pressure valve interactable");
                if (exit.requiredValve != valve)
                {
                    throw new InvalidOperationException("Level validation failed: " + sceneName + " final lift is not linked to the Boilerheart pressure valve.");
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
            if (string.IsNullOrWhiteSpace(pickup.definition.collectMessage))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " pickup " + pickup.name + " definition has no collect message.");
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
            RequireEqual(enemy.attackDamage, GameBalance.ScrapperAttackDamage, sceneName + " Scrapper damage balance");
            RequireApprox(enemy.attackWindup, GameBalance.ScrapperAttackWindup, sceneName + " Scrapper windup balance");
            RequireApprox(enemy.obstacleProbeDistance, GameBalance.ScrapperObstacleProbeDistance, sceneName + " Scrapper obstacle probe balance");
            if (enemy.definition == null)
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " Scrapper is missing an EnemyDefinition.");
            }

            RequireEqual((int)enemy.definition.attackStyle, (int)EnemyAttackStyle.Melee, sceneName + " Scrapper definition style");
            RequireEqual(enemy.definition.maxHealth, GameBalance.ScrapperHealth, sceneName + " Scrapper definition health");
            RequireApprox(enemy.definition.moveSpeed, GameBalance.ScrapperMoveSpeed, sceneName + " Scrapper definition speed");
            RequireEqual(enemy.definition.attackDamage, GameBalance.ScrapperAttackDamage, sceneName + " Scrapper definition damage");
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
        }
    }

    private static void ValidateBalanceValues(string sceneName, PlayerController playerController, PlayerInventory playerInventory, WeaponController weaponController)
    {
        RequireApprox(playerController.moveSpeed, GameBalance.PlayerMoveSpeed, sceneName + " player speed balance");
        RequireEqual(playerInventory.startingAmmo, GameBalance.StartingAmmo, sceneName + " starting ammo balance");
        RequireEqual(weaponController.damage, GameBalance.PressurePistolDamage, sceneName + " pistol damage balance");
        RequireApprox(weaponController.fireCooldown, GameBalance.PressurePistolCooldown, sceneName + " pistol cooldown balance");
        if (weaponController.definition == null)
        {
            throw new InvalidOperationException("Level validation failed: " + sceneName + " WeaponController is missing a WeaponDefinition.");
        }

        RequireEqual(weaponController.definition.damage, GameBalance.PressurePistolDamage, sceneName + " weapon definition damage");
        RequireApprox(weaponController.definition.fireCooldown, GameBalance.PressurePistolCooldown, sceneName + " weapon definition cooldown");
        RequireApprox(weaponController.definition.range, weaponController.range, sceneName + " weapon definition range");
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
        else if (sceneName == "Level03")
        {
            RequireNamed("Work Order Board - Boilerheart", sceneName + " boilerheart work-order board visual");
            RequireNamed("Boilerheart Triple Pipe Bundle", sceneName + " boilerheart pipe-bundle visual");
            RequireNamed("Boilerheart Furnace Core", sceneName + " boilerheart furnace core visual");
            RequireNamed("Boilerheart Pressure Valve Objective", sceneName + " boilerheart pressure valve objective");
            RequireNamed("Boilerheart Pressure Valve Wheel", sceneName + " boilerheart pressure valve wheel visual");
            RequireNamed("Boilerheart Valve Vented Lamp", sceneName + " boilerheart valve vented signal");
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
