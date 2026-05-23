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
        if (sceneName == "Level03")
        {
            yield return RunLevel03Exit();
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

        Teleport(player, door.transform.position + Vector3.back * 1.2f);
        yield return WaitUntilOrFail(() => doorCollider == null || !doorCollider.enabled, "pressure gate opening", 2f);
        if (failed)
        {
            yield break;
        }

        inventory.TryUseAmmo(2);
        string targetSceneName = transition.targetSceneName;
        Teleport(player, transition.transform.position);
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

    private IEnumerator RunLevel03Exit()
    {
        DisableEnemiesForDeterministicObjectiveTest();

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        ExitTrigger exit = Require<ExitTrigger>("ExitTrigger");
        SteamValveObjective valve = Require<SteamValveObjective>("SteamValveObjective");
        GameStateController gameState = Require<GameStateController>("GameStateController");
        if (!RunProgress.HasSnapshot || inventory.Ammo != RunProgress.Ammo)
        {
            Fail("Auto-playthrough failed: run progress did not persist into Level03.");
            yield break;
        }

        Teleport(player, exit.transform.position);
        yield return new WaitForSeconds(0.35f);
        if (gameState.State == GameRunState.Won)
        {
            Fail("Auto-playthrough failed: final lift unlocked before Boilerheart pressure valve.");
            yield break;
        }

        Teleport(player, valve.transform.position);
        valve.Interact(player.gameObject);
        yield return WaitUntilOrFail(() => valve.IsComplete && !exit.IsLocked, "boilerheart pressure valve venting", 2f);
        if (failed)
        {
            yield break;
        }

        Teleport(player, exit.transform.position);
        yield return WaitUntilOrFail(() => gameState.State == GameRunState.Won, "level 03 service lift win state", 2f);
        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_AUTO_PLAYTHROUGH_PASS");
        Application.Quit(0);
    }

    private static void DisableEnemiesForDeterministicObjectiveTest()
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
