using System;
using System.Collections;
using UnityEngine;

public class RuntimeCombatEdgeTest : MonoBehaviour
{
    private const string CombatEdgeArgument = "-v0CombatEdgeSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(CombatEdgeArgument))
        {
            yield break;
        }

        yield return null;

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerHealth health = Require<PlayerHealth>("PlayerHealth");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        WeaponController weapon = Require<WeaponController>("WeaponController");
        GameStateController gameState = Require<GameStateController>("GameStateController");
        EnemyController meleeEnemy = FindCombatTarget();

        DisableExtraEnemies(meleeEnemy);
        DisableRangedEnemies();

        yield return VerifyEmptyAmmo(weapon, inventory);
        if (failed)
        {
            yield break;
        }

        PlaceMeleeActors(player, meleeEnemy);
        int healthBeforeMelee = health.CurrentHealth;
        yield return WaitUntilOrFail(() => health.CurrentHealth < healthBeforeMelee, "Scrapper melee damage", 3f);
        if (failed)
        {
            yield break;
        }

        PlayerDamageVfx damageVfx = UnityEngine.Object.FindAnyObjectByType<PlayerDamageVfx>();
        if (damageVfx == null || damageVfx.PieceCount < 8)
        {
            Fail("Combat edge smoke failed: player damage VFX did not spawn with enough visible pieces.");
            yield break;
        }

        meleeEnemy.gameObject.SetActive(false);
        health.TakeDamage(999);
        yield return null;
        RequireState(health.IsDead, "player death flag");
        RequireState(gameState.State == GameRunState.Dead, "game death state");
        RequireState(!gameState.IsGameplayActive, "gameplay disabled after death");

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_COMBAT_EDGE_PASS");
        Application.Quit(0);
    }

    private IEnumerator VerifyEmptyAmmo(WeaponController weapon, PlayerInventory inventory)
    {
        while (inventory.Ammo > 0)
        {
            inventory.TryUseAmmo();
        }

        yield return null;

        if (weapon.FireOnce())
        {
            Fail("Combat edge smoke failed: weapon fired with empty ammo.");
        }
    }

    private static EnemyController FindCombatTarget()
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>();
        if (enemies.Length == 0)
        {
            throw new InvalidOperationException("Combat edge smoke failed: missing EnemyController.");
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

    private static void PlaceMeleeActors(PlayerController player, EnemyController target)
    {
        CharacterController playerController = player.GetComponent<CharacterController>();
        CharacterController targetController = target.GetComponent<CharacterController>();

        SetControllerEnabled(playerController, false);
        SetControllerEnabled(targetController, false);

        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        target.transform.position = new Vector3(0f, 1f, 1.05f);
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

        Fail("Combat edge smoke failed while waiting for " + step + ".");
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Combat edge smoke failed: " + label + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Combat edge smoke failed: missing " + label + ".");
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
