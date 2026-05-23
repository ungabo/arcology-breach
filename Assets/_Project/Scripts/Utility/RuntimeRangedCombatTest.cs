using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeRangedCombatTest : MonoBehaviour
{
    private const string RangedCombatArgument = "-v0RangedCombatSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(RangedCombatArgument))
        {
            yield break;
        }

        yield return null;

        if (SceneManager.GetActiveScene().name != "Level02")
        {
            Fail("Ranged combat smoke failed: expected Level02.");
            yield break;
        }

        EnemyController[] meleeEnemies = UnityEngine.Object.FindObjectsByType<EnemyController>();
        foreach (EnemyController enemy in meleeEnemies)
        {
            enemy.gameObject.SetActive(false);
        }

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerHealth health = Require<PlayerHealth>("PlayerHealth");
        RangedEnemyController lancer = Require<RangedEnemyController>("RangedEnemyController");

        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        player.transform.position = lancer.transform.position + Vector3.back * 8f;

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        int startingHealth = health.CurrentHealth;
        yield return WaitUntilOrFail(HasPressureBoltVfx, "Lancer pressure bolt VFX", 5f);
        if (failed)
        {
            yield break;
        }

        yield return WaitUntilOrFail(() => health.CurrentHealth < startingHealth, "Lancer pressure bolt damage", 5f);
        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_RANGED_COMBAT_PASS");
        Application.Quit(0);
    }

    private static bool HasPressureBoltVfx()
    {
        PressureBoltVfx boltVfx = UnityEngine.Object.FindAnyObjectByType<PressureBoltVfx>();
        return boltVfx != null && boltVfx.VisiblePieceCount >= 5;
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

        Fail("Ranged combat smoke failed while waiting for " + step + ".");
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Ranged combat smoke failed: missing " + label + ".");
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
