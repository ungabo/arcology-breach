using System;
using System.Collections;
using UnityEngine;

public class RuntimeWeaponSwitchTest : MonoBehaviour
{
    private const string WeaponArgument = "-v0WeaponSwitchSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(WeaponArgument))
        {
            yield break;
        }

        yield return null;

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        WeaponController weapon = Require<WeaponController>("WeaponController");
        EnemyController target = FindCombatTarget();

        if (weapon.steamScattergunDefinition == null)
        {
            Fail("Weapon switch smoke failed: missing Steam Scattergun definition.");
            yield break;
        }

        DisableExtraEnemies(target);
        DisableRangedEnemies();
        PlaceCombatActors(player, target);
        target.enabled = false;

        if (weapon.HasSteamScattergun)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun should not start unlocked.");
            yield break;
        }

        weapon.UnlockSteamScattergun(switchToWeapon: true, showMessage: false);
        if (!weapon.HasSteamScattergun || !weapon.IsUsingSteamScattergun || weapon.ActiveWeaponName != "Steam Scattergun")
        {
            Fail("Weapon switch smoke failed: Steam Scattergun did not unlock and equip.");
            yield break;
        }

        int startingAmmo = inventory.Ammo;
        if (!weapon.FireOnce())
        {
            Fail("Weapon switch smoke failed: Steam Scattergun did not fire.");
            yield break;
        }

        RequireEqual(inventory.Ammo, startingAmmo - weapon.ammoCost, "ammo after Steam Scattergun shot");
        yield return WaitUntilOrFail(() => target == null, "Scrapper death from Steam Scattergun", 2f);
        if (failed)
        {
            yield break;
        }

        if (!weapon.EquipPressurePistol(showMessage: false) || weapon.IsUsingSteamScattergun || weapon.ActiveWeaponName != "Pressure Pistol")
        {
            Fail("Weapon switch smoke failed: Pressure Pistol re-equip failed.");
            yield break;
        }

        Debug.Log("V0_WEAPON_SWITCH_PASS");
        Application.Quit(0);
    }

    private static EnemyController FindCombatTarget()
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        if (enemies.Length == 0)
        {
            throw new InvalidOperationException("Weapon switch smoke failed: missing EnemyController.");
        }

        return enemies[0];
    }

    private static void DisableExtraEnemies(EnemyController target)
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
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
        RangedEnemyController[] rangedEnemies = UnityEngine.Object.FindObjectsByType<RangedEnemyController>(FindObjectsSortMode.None);
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

        target.transform.position = new Vector3(0f, 1f, 2.6f);
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

        Fail("Weapon switch smoke failed while waiting for " + step + ".");
    }

    private void RequireEqual(int actual, int expected, string label)
    {
        if (actual != expected)
        {
            Fail("Weapon switch smoke failed: " + label + " expected " + expected + " but found " + actual + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Weapon switch smoke failed: missing " + label + ".");
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
