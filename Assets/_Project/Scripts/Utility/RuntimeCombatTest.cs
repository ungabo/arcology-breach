using System;
using System.Collections;
using UnityEngine;

public class RuntimeCombatTest : MonoBehaviour
{
    private const string CombatArgument = "-v0CombatSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(CombatArgument))
        {
            yield break;
        }

        yield return null;

        PlayerController player = Require<PlayerController>("PlayerController");
        WeaponController weapon = Require<WeaponController>("WeaponController");
        EnemyController target = FindCombatTarget();

        DisableExtraEnemies(target);
        PlaceCombatActors(player, target);
        target.enabled = false;

        yield return null;

        for (int i = 0; i < 4 && target != null; i++)
        {
            bool fired = weapon.FireOnce();
            if (!fired)
            {
                Fail("Combat smoke failed: weapon did not fire on shot " + (i + 1) + ".");
                yield break;
            }

            yield return new WaitForSeconds(weapon.fireCooldown + 0.05f);
        }

        yield return WaitUntilOrFail(() => target == null, "Scrapper death from pressure pistol fire", 2f);
        if (failed)
        {
            yield break;
        }

        MachineDeathVfx deathVfx = UnityEngine.Object.FindAnyObjectByType<MachineDeathVfx>();
        if (deathVfx == null || deathVfx.PieceCount < 8)
        {
            Fail("Combat smoke failed: machine death VFX did not spawn with enough visible pieces.");
            yield break;
        }

        Debug.Log("V0_COMBAT_SMOKE_PASS");
        Application.Quit(0);
    }

    private static EnemyController FindCombatTarget()
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>();
        if (enemies.Length == 0)
        {
            throw new InvalidOperationException("Combat smoke failed: missing EnemyController.");
        }

        return enemies[0];
    }

    private static void DisableExtraEnemies(EnemyController target)
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != target)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    private static void PlaceCombatActors(PlayerController player, EnemyController target)
    {
        CharacterController playerController = player.GetComponent<CharacterController>();
        CharacterController targetController = target.GetComponent<CharacterController>();

        SetControllerEnabled(playerController, false);
        SetControllerEnabled(targetController, false);

        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        if (player.playerCamera != null)
        {
            player.playerCamera.localRotation = Quaternion.identity;
        }

        target.transform.position = new Vector3(0f, 1f, 3.2f);
        target.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        SetControllerEnabled(targetController, true);
        SetControllerEnabled(playerController, true);
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

        Fail("Combat smoke failed while waiting for " + step + ".");
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Combat smoke failed: missing " + label + ".");
        }

        return value;
    }

    private void Fail(string message)
    {
        failed = true;
        Debug.LogError(message);
        Application.Quit(1);
    }

    private static void SetControllerEnabled(CharacterController controller, bool enabled)
    {
        if (controller != null)
        {
            controller.enabled = enabled;
        }
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
