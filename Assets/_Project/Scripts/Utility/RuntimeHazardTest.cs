using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeHazardTest : MonoBehaviour
{
    private const string HazardArgument = "-v0HazardSmoke";
    private const string SteamHazardSceneName = "Level03";
    private const string FurnaceHazardSceneName = "Level04";

    private static bool steamHazardVerified;

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(HazardArgument))
        {
            yield break;
        }

        yield return null;

        if (!steamHazardVerified)
        {
            if (SceneManager.GetActiveScene().name != SteamHazardSceneName)
            {
                SceneManager.LoadScene(SteamHazardSceneName);
                yield break;
            }

            yield return VerifySteamHazard();
            if (failed)
            {
                yield break;
            }

            steamHazardVerified = true;
            SceneManager.LoadScene(FurnaceHazardSceneName);
            yield break;
        }

        if (SceneManager.GetActiveScene().name != FurnaceHazardSceneName)
        {
            SceneManager.LoadScene(FurnaceHazardSceneName);
            yield break;
        }

        yield return VerifyFurnaceHeatHazard();
        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_HAZARD_PASS");
        Application.Quit(0);
    }

    private IEnumerator VerifySteamHazard()
    {
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

        SteamHazardVfx steamVfx = hazard.GetComponent<SteamHazardVfx>();
        if (steamVfx == null || steamVfx.VisiblePuffCount < 2)
        {
            Fail("Hazard smoke failed: steam hazard VFX did not expose enough visible puffs.");
            yield break;
        }

        RequireState(gameState.State == GameRunState.Playing, "hazard should not end the run from one tick");
    }

    private IEnumerator VerifyFurnaceHeatHazard()
    {
        DisableEnemies();

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerHealth health = Require<PlayerHealth>("PlayerHealth");
        GameStateController gameState = Require<GameStateController>("GameStateController");
        FurnaceHeatHazard hazard = Require<FurnaceHeatHazard>("FurnaceHeatHazard");

        int healthBeforeHazard = health.CurrentHealth;
        Teleport(player, hazard.transform.position);
        hazard.ForceActiveForTest(0.75f);
        yield return null;
        hazard.TryDamage(player.gameObject);
        yield return WaitUntilOrFail(() => health.CurrentHealth < healthBeforeHazard, "furnace heat hazard damage", 1f);
        if (failed)
        {
            yield break;
        }

        FurnaceHeatHazardVfx furnaceVfx = hazard.GetComponent<FurnaceHeatHazardVfx>();
        if (furnaceVfx == null || !furnaceVfx.HasPhaseSignals || furnaceVfx.VisibleHeatPieceCount < 2 || !furnaceVfx.ActiveHeatVisible)
        {
            Fail("Hazard smoke failed: furnace heat VFX did not expose active heat ripples.");
            yield break;
        }

        RequireState(gameState.State == GameRunState.Playing, "furnace heat should not end the run from one pulse");
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

        BulwarkEnemyController[] bulwarks = UnityEngine.Object.FindObjectsByType<BulwarkEnemyController>();
        foreach (BulwarkEnemyController enemy in bulwarks)
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
