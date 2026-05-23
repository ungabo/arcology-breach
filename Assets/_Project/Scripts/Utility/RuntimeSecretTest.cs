using System;
using System.Collections;
using UnityEngine;

public class RuntimeSecretTest : MonoBehaviour
{
    private const string SecretArgument = "-v0SecretSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(SecretArgument))
        {
            yield break;
        }

        yield return null;

        DisableEnemies();

        PlayerController player = Require<PlayerController>("PlayerController");
        SecretArea secret = Require<SecretArea>("SecretArea");

        if (RunStats.TotalSecrets <= 0)
        {
            Fail("Secret smoke failed: run stats did not register total secrets.");
            yield break;
        }

        int discoveredBefore = RunStats.DiscoveredSecrets;
        Teleport(player, secret.transform.position);
        secret.Discover(player.gameObject);
        yield return WaitUntilOrFail(() => secret.Discovered && RunStats.DiscoveredSecrets > discoveredBefore, "secret discovery", 1f);
        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_SECRET_PASS");
        Application.Quit(0);
    }

    private static void DisableEnemies()
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

    private static void Teleport(PlayerController player, Vector3 position)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        player.transform.position = new Vector3(position.x, 0f, position.z);

        if (controller != null)
        {
            controller.enabled = true;
        }
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

        Fail("Secret smoke failed while waiting for " + step + ".");
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Secret smoke failed: missing " + label + ".");
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
