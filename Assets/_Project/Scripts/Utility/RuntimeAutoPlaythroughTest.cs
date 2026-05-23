using System;
using System.Collections;
using UnityEngine;

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

        DisableEnemiesForDeterministicObjectiveTest();

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        LockedDoor door = Require<LockedDoor>("LockedDoor");
        ExitTrigger exit = Require<ExitTrigger>("ExitTrigger");
        GameStateController gameState = Require<GameStateController>("GameStateController");
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

        Teleport(player, exit.transform.position);
        yield return WaitUntilOrFail(() => gameState.State == GameRunState.Won, "service lift win state", 2f);
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
