using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeAutoPlaythroughTest : MonoBehaviour
{
    private const string AutoPlaythroughArgument = "-v0AutoPlaythrough";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(AutoPlaythroughArgument))
        {
            yield break;
        }

        yield return null;

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level05")
        {
            yield return RunLevel05Exit();
            yield break;
        }

        if (sceneName == "Level04")
        {
            yield return RunLevel04Transition();
            yield break;
        }

        if (sceneName == "Level03")
        {
            yield return RunLevel03Transition();
            yield break;
        }

        if (sceneName == "Level02")
        {
            yield return RunLevel02Transition();
            yield break;
        }

        yield return RunLevel01Transition();
    }

    private IEnumerator RunLevel01Transition()
    {
        DisableEnemiesForDeterministicObjectiveTest();

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        LockedDoor door = Require<LockedDoor>("LockedDoor");
        LevelTransitionTrigger transition = Require<LevelTransitionTrigger>("LevelTransitionTrigger");
        Pickup gearKey = FindPickup(PickupKind.Key);

        Collider doorCollider = door.GetComponent<Collider>();
        if (doorCollider == null)
        {
            Fail("Auto-playthrough failed: pressure gate is missing a collider.");
            yield break;
        }

        Teleport(player, door.transform.position + Vector3.back * 1.2f);
        yield return new WaitForSeconds(0.25f);
        if (failed)
        {
            yield break;
        }

        if (!doorCollider.enabled)
        {
            Fail("Auto-playthrough failed: pressure gate opened before gear key pickup.");
            yield break;
        }

        Teleport(player, gearKey.transform.position);
        yield return WaitUntilOrFail(() => inventory.HasKey, "gear key pickup", 2f);
        if (failed)
        {
            yield break;
        }

        if (!ObjectiveContains("Return to the pressure gate."))
        {
            Fail("Auto-playthrough failed: objective HUD did not update after gear key pickup.");
            yield break;
        }

        Teleport(player, door.transform.position + Vector3.back * 1.2f);
        yield return WaitUntilOrFail(() => doorCollider == null || !doorCollider.enabled, "pressure gate opening", 2f);
        if (failed)
        {
            yield break;
        }

        if (!ObjectiveContains("Ride the service lift."))
        {
            Fail("Auto-playthrough failed: objective HUD did not update after pressure gate opening.");
            yield break;
        }

        GateOpenVfx gateOpenVfx = UnityEngine.Object.FindAnyObjectByType<GateOpenVfx>();
        if (gateOpenVfx == null || gateOpenVfx.PieceCount < 8)
        {
            Fail("Auto-playthrough failed: pressure gate open VFX did not spawn with enough visible pieces.");
            yield break;
        }

        inventory.TryUseAmmo(2);
        string targetSceneName = transition.targetSceneName;
        Teleport(player, transition.transform.position);
        yield return WaitUntilOrFail(HasVisibleLiftActivationVfx, "service lift activation VFX", 0.4f);
        if (failed)
        {
            yield break;
        }

        yield return WaitUntilOrFail(() => SceneManager.GetActiveScene().name == targetSceneName, "service lift level transition", 2f);
        if (failed)
        {
            yield break;
        }
    }

    private IEnumerator RunLevel02Transition()
    {
        DisableEnemiesForDeterministicObjectiveTest();

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        LevelTransitionTrigger transition = Require<LevelTransitionTrigger>("LevelTransitionTrigger");
        if (!RunProgress.HasSnapshot || inventory.Ammo != RunProgress.Ammo)
        {
            Fail("Auto-playthrough failed: run progress did not persist into Level02.");
            yield break;
        }

        string targetSceneName = transition.targetSceneName;
        Teleport(player, transition.transform.position);
        yield return WaitUntilOrFail(() => SceneManager.GetActiveScene().name == targetSceneName, "level 02 service lift transition", 2f);
        if (failed)
        {
            yield break;
        }
    }

    private IEnumerator RunLevel03Transition()
    {
        DisableEnemiesForDeterministicObjectiveTest();

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        LevelTransitionTrigger transition = Require<LevelTransitionTrigger>("LevelTransitionTrigger");
        SteamValveObjective valve = Require<SteamValveObjective>("SteamValveObjective");
        if (!RunProgress.HasSnapshot || inventory.Ammo != RunProgress.Ammo)
        {
            Fail("Auto-playthrough failed: run progress did not persist into Level03.");
            yield break;
        }

        string startingSceneName = SceneManager.GetActiveScene().name;
        Teleport(player, transition.transform.position);
        yield return new WaitForSeconds(0.35f);
        if (SceneManager.GetActiveScene().name != startingSceneName || !transition.IsLocked)
        {
            Fail("Auto-playthrough failed: foundry lift unlocked before Boilerheart pressure valve.");
            yield break;
        }

        Teleport(player, valve.transform.position);
        valve.Interact(player.gameObject);
        yield return WaitUntilOrFail(() => valve.IsComplete && !transition.IsLocked && LinkedHazardsDisabled(valve), "boilerheart pressure valve venting", 2f);
        if (failed)
        {
            yield break;
        }

        if (!ObjectiveContains("Ride the foundry lift."))
        {
            Fail("Auto-playthrough failed: objective HUD did not update after Boilerheart valve venting.");
            yield break;
        }

        string targetSceneName = transition.targetSceneName;
        Teleport(player, transition.transform.position);
        yield return WaitUntilOrFail(() => SceneManager.GetActiveScene().name == targetSceneName, "level 03 foundry lift transition", 2f);
        if (failed)
        {
            yield break;
        }
    }

    private IEnumerator RunLevel04Transition()
    {
        DisableEnemiesForDeterministicObjectiveTest();

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        LevelTransitionTrigger transition = Require<LevelTransitionTrigger>("LevelTransitionTrigger");
        if (!RunProgress.HasSnapshot || inventory.Ammo != RunProgress.Ammo)
        {
            Fail("Auto-playthrough failed: run progress did not persist into Level04.");
            yield break;
        }

        string targetSceneName = transition.targetSceneName;
        Teleport(player, transition.transform.position);
        yield return WaitUntilOrFail(() => SceneManager.GetActiveScene().name == targetSceneName, "level 04 emergency hoist transition", 2f);
        if (failed)
        {
            yield break;
        }
    }

    private IEnumerator RunLevel05Exit()
    {
        DisableEnemiesForDeterministicObjectiveTest(keepWardenActive: true);

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        ExitTrigger exit = Require<ExitTrigger>("ExitTrigger");
        GameStateController gameState = Require<GameStateController>("GameStateController");
        GuardianDefeatObjective guardianObjective = Require<GuardianDefeatObjective>("GuardianDefeatObjective");
        GovernorWardenController warden = Require<GovernorWardenController>("GovernorWardenController");
        if (!RunProgress.HasSnapshot || inventory.Ammo != RunProgress.Ammo)
        {
            Fail("Auto-playthrough failed: run progress did not persist into Level05.");
            yield break;
        }

        Teleport(player, exit.transform.position);
        yield return new WaitForSeconds(0.35f);
        if (gameState.State == GameRunState.Won || !exit.IsLocked)
        {
            Fail("Auto-playthrough failed: master override hoist unlocked before Governor Warden defeat.");
            yield break;
        }

        Teleport(player, new Vector3(0f, 0f, 14f));
        yield return null;

        warden.TakeDamage(GameBalance.GovernorWardenHealth);
        yield return WaitUntilOrFail(() => guardianObjective.IsComplete && !exit.IsLocked, "Governor Warden defeat unlocking final hoist", 2f);
        if (failed)
        {
            yield break;
        }

        if (!ObjectiveContains("Engage the master override hoist."))
        {
            Fail("Auto-playthrough failed: objective HUD did not update after Governor Warden defeat.");
            yield break;
        }

        Teleport(player, exit.transform.position);
        yield return WaitUntilOrFail(() => gameState.State == GameRunState.Won, "level 05 master override hoist win state", 2f);
        if (failed)
        {
            yield break;
        }

        if (!ObjectiveContains("Run complete."))
        {
            Fail("Auto-playthrough failed: objective HUD did not update after win state.");
            yield break;
        }

        if (RunStats.TotalSecrets < 2)
        {
            Fail("Auto-playthrough failed: multi-level run secret totals did not persist to win state.");
            yield break;
        }

        Debug.Log("V0_AUTO_PLAYTHROUGH_PASS");
        Application.Quit(0);
    }

    private static bool LinkedHazardsDisabled(SteamValveObjective valve)
    {
        if (valve.hazardsToDisableOnComplete == null || valve.hazardsToDisableOnComplete.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < valve.hazardsToDisableOnComplete.Length; i++)
        {
            SteamHazard hazard = valve.hazardsToDisableOnComplete[i];
            if (hazard != null && hazard.gameObject.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    private static bool ObjectiveContains(string expected)
    {
        HUDController hud = UnityEngine.Object.FindAnyObjectByType<HUDController>();
        return hud != null && hud.CurrentObjective.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static bool HasVisibleLiftActivationVfx()
    {
        LiftActivationVfx vfx = UnityEngine.Object.FindAnyObjectByType<LiftActivationVfx>();
        return vfx != null && vfx.PieceCount >= 8;
    }

    private static void DisableEnemiesForDeterministicObjectiveTest(bool keepWardenActive = false)
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }

        RangedEnemyController[] rangedEnemies = UnityEngine.Object.FindObjectsByType<RangedEnemyController>();
        foreach (RangedEnemyController enemy in rangedEnemies)
        {
            enemy.gameObject.SetActive(false);
        }

        BulwarkEnemyController[] bulwarks = UnityEngine.Object.FindObjectsByType<BulwarkEnemyController>();
        foreach (BulwarkEnemyController enemy in bulwarks)
        {
            enemy.gameObject.SetActive(false);
        }

        GovernorWardenController[] wardens = UnityEngine.Object.FindObjectsByType<GovernorWardenController>();
        foreach (GovernorWardenController enemy in wardens)
        {
            if (keepWardenActive)
            {
                enemy.enabled = false;
            }
            else
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    private static Pickup FindPickup(PickupKind kind)
    {
        Pickup[] pickups = UnityEngine.Object.FindObjectsByType<Pickup>();
        foreach (Pickup pickup in pickups)
        {
            if (pickup.kind == kind)
            {
                return pickup;
            }
        }

        throw new InvalidOperationException("Auto-playthrough failed: missing pickup kind " + kind);
    }

    private IEnumerator WaitUntilOrFail(Func<bool> predicate, string step, float timeoutSeconds)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < timeoutSeconds)
        {
            if (predicate())
            {
                yield break;
            }

            yield return null;
        }

        Fail("Auto-playthrough failed while waiting for " + step + ".");
    }

    private static void Teleport(PlayerController player, Vector3 position)
    {
        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        player.transform.position = new Vector3(position.x, 0f, position.z);

        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Auto-playthrough failed: missing " + label + ".");
        }

        return value;
    }

    private void Fail(string message)
    {
        failed = true;
        Debug.LogError(message);
        Application.Quit(1);
    }

    private static bool HasArgument(string argument)
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], argument, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
