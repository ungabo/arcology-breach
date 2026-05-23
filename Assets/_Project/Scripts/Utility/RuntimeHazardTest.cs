using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeHazardTest : MonoBehaviour
{
    private const string HazardArgument = "-v0HazardSmoke";
    private const string HazardSceneName = "Level03";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(HazardArgument))
        {
            yield break;
        }

        yield return null;

        if (SceneManager.GetActiveScene().name != HazardSceneName)
        {
            SceneManager.LoadScene(HazardSceneName);
            yield break;
        }

        DisableEnemies();

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerHealth health = Require<PlayerHealth>("PlayerHealth");
        GameStateController gameState = Require<GameStateController>("GameStateController");
        SteamHazard hazard = Require<SteamHazard>("SteamHazard");

        int healthBeforeHazard = health.CurrentHealth;
        Teleport(player, hazard.transform.position);
        hazard.TryDamage(player.gameObject);
        yield return WaitUntilOrFail(() => health.CurrentHealth < healthBeforeHazard, "steam hazard damage", 1f);
        if (failed)
        {
            yield break;
        }

        RequireState(gameState.State == GameRunState.Playing, "hazard should not end the run from one tick");

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_HAZARD_PASS");
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

        Fail("Hazard smoke failed while waiting for " + step + ".");
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Hazard smoke failed: " + label + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Hazard smoke failed: missing " + label + ".");
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
