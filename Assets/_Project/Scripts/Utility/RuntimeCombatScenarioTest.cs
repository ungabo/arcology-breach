using System;
using System.Collections;
using UnityEngine;

public class RuntimeCombatScenarioTest : MonoBehaviour
{
    private const string CombatScenarioArgument = "-v0CombatScenarioSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(CombatScenarioArgument))
        {
            yield break;
        }

        yield return null;

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        WeaponController weapon = Require<WeaponController>("WeaponController");
        EnemyController target = FindCombatTarget();

        DisableExtraEnemies(target);
        DisableRangedEnemies();
        PlaceCombatActors(player, target);
        target.enabled = false;

        int startingAmmo = inventory.Ammo;
        int expectedShotsToKill = Mathf.CeilToInt(target.maxHealth / (float)weapon.damage);

        if (expectedShotsToKill < 2)
        {
            Fail("Combat scenario failed: expected at least two shots to verify survival before final hit.");
            yield break;
        }

        if (!weapon.FireOnce())
        {
            Fail("Combat scenario failed: first weapon shot did not fire.");
            yield break;
        }

        int ammoAfterFirstShot = inventory.Ammo;
        RequireEqual(ammoAfterFirstShot, startingAmmo - 1, "ammo after first shot");
        yield return null;

        MachineHitVfx hitVfx = UnityEngine.Object.FindAnyObjectByType<MachineHitVfx>();
        if (hitVfx == null || hitVfx.PieceCount < 6)
        {
            Fail("Combat scenario failed: non-lethal machine hit VFX did not spawn with enough visible pieces.");
            yield break;
        }

        if (weapon.FireOnce())
        {
            Fail("Combat scenario failed: weapon fired again during cooldown.");
            yield break;
        }

        RequireEqual(inventory.Ammo, ammoAfterFirstShot, "ammo unchanged during cooldown rejection");
        yield return new WaitForSeconds(weapon.fireCooldown + 0.05f);

        for (int shotIndex = 2; shotIndex < expectedShotsToKill; shotIndex++)
        {
            if (!weapon.FireOnce())
            {
                Fail("Combat scenario failed: weapon did not fire on shot " + shotIndex + ".");
                yield break;
            }

            RequireEqual(inventory.Ammo, startingAmmo - shotIndex, "ammo after shot " + shotIndex);
            yield return null;

            if (target == null)
            {
                Fail("Combat scenario failed: enemy died before final expected shot.");
                yield break;
            }

            yield return new WaitForSeconds(weapon.fireCooldown + 0.05f);
        }

        if (!weapon.FireOnce())
        {
            Fail("Combat scenario failed: final weapon shot did not fire.");
            yield break;
        }

        RequireEqual(inventory.Ammo, startingAmmo - expectedShotsToKill, "ammo after final shot");
        yield return WaitUntilOrFail(() => target == null, "enemy death after final shot", 2f);
        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_COMBAT_SCENARIO_PASS");
        Application.Quit(0);
    }

    private static EnemyController FindCombatTarget()
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>();
        if (enemies.Length == 0)
        {
            throw new InvalidOperationException("Combat scenario failed: missing EnemyController.");
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

    private static void DisableRangedEnemies()
    {
        RangedEnemyController[] rangedEnemies = UnityEngine.Object.FindObjectsByType<RangedEnemyController>();
        foreach (RangedEnemyController enemy in rangedEnemies)
        {
            enemy.gameObject.SetActive(false);
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

        Fail("Combat scenario failed while waiting for " + step + ".");
    }

    private void RequireEqual(int actual, int expected, string label)
    {
        if (actual != expected)
        {
            Fail("Combat scenario failed: " + label + " expected " + expected + " but found " + actual + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Combat scenario failed: missing " + label + ".");
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
