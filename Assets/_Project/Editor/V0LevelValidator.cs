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
    private const string Level04ScenePath = "Assets/_Project/Scenes/Level04.unity";
    private const string Level05ScenePath = "Assets/_Project/Scenes/Level05.unity";

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
        Require<SteamworksSpinner>("MainMenu SteamworksSpinner");

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

        Require<PauseMenuController>(sceneName + " PauseMenuController");
        Require<RuntimeInteractionTest>(sceneName + " RuntimeInteractionTest");
        Require<RuntimeCombatScenarioTest>(sceneName + " RuntimeCombatScenarioTest");
        Require<RuntimeBellowsNodeTest>(sceneName + " RuntimeBellowsNodeTest");
        Require<RuntimeBulwarkCombatTest>(sceneName + " RuntimeBulwarkCombatTest");
        Require<RuntimeWardenCombatTest>(sceneName + " RuntimeWardenCombatTest");
        Require<RuntimeHazardTest>(sceneName + " RuntimeHazardTest");
        Require<RuntimeSecretTest>(sceneName + " RuntimeSecretTest");
        Require<RuntimeWeaponSwitchTest>(sceneName + " RuntimeWeaponSwitchTest");
        Require<EnemyController>(sceneName + " EnemyController");
        Require<Pickup>(sceneName + " Pickup");

        ValidatePickups(sceneName);
        ValidateEnemies(sceneName);
        ValidateHazards(sceneName);
        ValidateSecrets(sceneName);
        ValidateEnvironmentPropVisuals(sceneName);
        ValidateLorePlaques(sceneName);
        ValidateMachineryMotion(sceneName);

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
            if (string.IsNullOrWhiteSpace(pickup.definition.collectMessage))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " pickup " + pickup.name + " definition has no collect message.");
            }

            if (pickup.kind == PickupKind.Weapon && string.IsNullOrWhiteSpace(pickup.definition.weaponUnlockId))
            {
                throw new InvalidOperationException("Level validation failed: " + sceneName + " weapon pickup " + pickup.name + " has no weapon unlock id.");
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
            RequireMachineMotion(enemy.gameObject, sceneName + " Scrapper machine motion");
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
        RequireEqual(playerInventory.startingAmmo, GameBalance.StartingAmmo, sceneName + " starting ammo balance");
        RequireEqual(weaponController.damage, GameBalance.PressurePistolDamage, sceneName + " pistol damage balance");
        RequireEqual(weaponController.ammoCost, GameBalance.PressurePistolAmmoCost, sceneName + " pistol ammo-cost balance");
        RequireEqual(weaponController.pelletCount, GameBalance.PressurePistolPelletCount, sceneName + " pistol pellet-count balance");
        RequireApprox(weaponController.fireCooldown, GameBalance.PressurePistolCooldown, sceneName + " pistol cooldown balance");
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
        RequireApprox(weaponController.definition.range, weaponController.range, sceneName + " weapon definition range");
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
        RequireNamed("Pressure Pistol Front Sight", sceneName + " pressure pistol front sight visual");
        RequireNamed("Steam Scattergun Viewmodel", sceneName + " Steam Scattergun viewmodel");
        RequireNamed("Steam Scattergun Brass Receiver", sceneName + " Steam Scattergun receiver visual");
        RequireNamed("Steam Scattergun Barrel 0", sceneName + " Steam Scattergun barrel visual");
        RequireNamed("Steam Scattergun Pressure Drum", sceneName + " Steam Scattergun pressure drum visual");
        RequireNamed("Steam Scattergun Pump Handle", sceneName + " Steam Scattergun pump handle visual");
    }

    private static void ValidateEnvironmentPropVisuals(string sceneName)
    {
        if (sceneName == "Level01")
        {
            RequireNamed("Work Order Board - Intake", sceneName + " intake work-order board visual");
            RequireNamed("Lore Plaque - Intake Archive", sceneName + " intake lore plaque visual");
            RequireNamed("Work Order Board - Gate", sceneName + " gate work-order board visual");
            RequireNamed("Pipe Bundle - Gate Manifold", sceneName + " gate pipe-bundle visual");
            RequireNamed("Secret - Intake Pressure Cache", sceneName + " secret pressure cache");
            RequireNamed("Secret Pressure Cache Brass Floor Plate", sceneName + " secret pressure cache floor plate");
        }
        else if (sceneName == "Level02")
        {
            RequireNamed("Work Order Board - Pipeworks", sceneName + " pipeworks work-order board visual");
            RequireNamed("Lore Plaque - Pipeworks Archive", sceneName + " pipeworks lore plaque visual");
            RequireNamed("Pipeworks Routing Valve Objective", sceneName + " Pipeworks routing valve objective");
            RequireNamed("Pipeworks Routing Valve Wheel", sceneName + " Pipeworks routing valve wheel visual");
            RequireNamed("Pipeworks Routing Valve Vented Lamp", sceneName + " Pipeworks routing valve vented signal");
            RequireNamed("Pipeworks Triple Pipe Bundle", sceneName + " pipeworks pipe-bundle visual");
            RequireNamed("Secret - Pipeworks Cartridge Cache", sceneName + " pipeworks secret cache");
            RequireNamed("Secret Pipeworks Cache Brass Floor Plate", sceneName + " pipeworks secret cache floor plate");
            RequireNamed("Pickup - Pipeworks Secret Pressure Cartridge Pack", sceneName + " pipeworks secret ammo reward");
        }
        else if (sceneName == "Level03")
        {
            RequireNamed("Work Order Board - Boilerheart", sceneName + " boilerheart work-order board visual");
            RequireNamed("Lore Plaque - Boilerheart Archive", sceneName + " boilerheart lore plaque visual");
            RequireNamed("Boilerheart Triple Pipe Bundle", sceneName + " boilerheart pipe-bundle visual");
            RequireNamed("Boilerheart Furnace Core", sceneName + " boilerheart furnace core visual");
            RequireNamed("Boilerheart Pressure Valve Objective", sceneName + " boilerheart pressure valve objective");
            RequireNamed("Boilerheart Pressure Valve Wheel", sceneName + " boilerheart pressure valve wheel visual");
            RequireNamed("Boilerheart Valve Vented Lamp", sceneName + " boilerheart valve vented signal");
            RequireNamed("Pickup - Steam Scattergun", sceneName + " Steam Scattergun pickup");
            RequireNamed("Pickup - Steam Scattergun Weapon Visual", sceneName + " Steam Scattergun pickup visual");
            RequireNamed("Enemy - Boilerheart Bellows Node", sceneName + " Bellows Node enemy");
            RequireNamed("Bellows Node Brass Bellows Body", sceneName + " Bellows Node body visual");
            RequireNamed("Bellows Node Furnace Lens", sceneName + " Bellows Node lens visual");
            RequireNamed("Bellows Node Exhaust Horn", sceneName + " Bellows Node horn visual");
            RequireNamed("Boilerheart Steam Hazard - Furnace Leak", sceneName + " boilerheart steam hazard");
            RequireNamed("Boilerheart Steam Hazard - Core Bleed", sceneName + " boilerheart steam hazard");
        }
        else if (sceneName == "Level04")
        {
            RequireNamed("Work Order Board - Foundry", sceneName + " foundry work-order board visual");
            RequireNamed("Lore Plaque - Foundry Archive", sceneName + " foundry lore plaque visual");
            RequireNamed("Foundry Triple Pipe Bundle", sceneName + " foundry pipe-bundle visual");
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
        }
        else if (sceneName == "Level05")
        {
            RequireNamed("Work Order Board - Governor Core", sceneName + " governor core work-order board visual");
            RequireNamed("Lore Plaque - Governor Archive", sceneName + " governor lore plaque visual");
            RequireNamed("Governor Core Triple Pipe Bundle", sceneName + " governor core pipe-bundle visual");
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
        }

        if (sceneName == "Level01")
        {
            RequireNamed("Repair Bay Cover Boiler Left", sceneName + " repair bay cover visual");
            RequireNamed("Repair Bay Cover Crate Right", sceneName + " repair bay cover visual");
            RequireNamed("Final Room Cover Stack West", sceneName + " final room cover visual");
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
