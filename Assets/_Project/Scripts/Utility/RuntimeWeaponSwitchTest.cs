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
        SteamworksAudio audio = Require<SteamworksAudio>("SteamworksAudio");
        EnemyController target = FindCombatTarget();
        Pickup weaponPickup = FindWeaponPickup();

        if (weapon.steamScattergunDefinition == null)
        {
            Fail("Weapon switch smoke failed: missing Steam Scattergun definition.");
            yield break;
        }

        if (!audio.HasClip(SteamworksAudioCue.SteamScattergunFire) || !audio.HasClip(SteamworksAudioCue.WeaponPickup) || !audio.HasClip(SteamworksAudioCue.SteamScattergunSlug))
        {
            Fail("Weapon switch smoke failed: Steam Scattergun acquisition/fire/slug audio cue is missing.");
            yield break;
        }

        DisableExtraEnemies(target);
        DisableRangedEnemies();
        DisableSupportEnemies();
        target.enabled = false;

        if (weapon.HasSteamScattergun)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun should not start unlocked.");
            yield break;
        }

        Teleport(player, weaponPickup.transform.position);
        yield return WaitUntilOrFail(() => inventory.HasSteamScattergun && weapon.HasSteamScattergun && weapon.IsUsingSteamScattergun && weapon.ActiveWeaponName == "Steam Scattergun", "Steam Scattergun pickup unlock", 2f);
        if (failed)
        {
            yield break;
        }

        WeaponPickupVfx pickupVfx = UnityEngine.Object.FindAnyObjectByType<WeaponPickupVfx>();
        if (pickupVfx == null || pickupVfx.PieceCount < 10)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun pickup VFX did not spawn with enough visible pieces.");
            yield break;
        }

        if (!audio.HasLastOneShotCue || audio.LastOneShotCue != SteamworksAudioCue.WeaponPickup)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun pickup did not use its dedicated acquisition audio cue.");
            yield break;
        }

        if (weapon.steamScattergunView == null || weapon.weaponView != weapon.steamScattergunView || !weapon.steamScattergunView.gameObject.activeSelf)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun viewmodel did not become active.");
            yield break;
        }

        PlaceCombatActors(player, target);
        target.enabled = false;

        int startingAmmo = inventory.Ammo;
        if (!weapon.FireSecondary())
        {
            Fail("Weapon switch smoke failed: Steam Scattergun slug did not fire.");
            yield break;
        }

        RequireEqual(inventory.Ammo, startingAmmo - weapon.secondaryAmmoCost, "ammo after Steam Scattergun slug");
        if (!audio.HasLastOneShotCue || audio.LastOneShotCue != SteamworksAudioCue.SteamScattergunSlug)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun slug did not use its dedicated audio cue.");
            yield break;
        }

        ScattergunSlugVfx slugVfx = UnityEngine.Object.FindAnyObjectByType<ScattergunSlugVfx>();
        if (slugVfx == null || slugVfx.PieceCount < 8)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun slug VFX did not spawn with enough visible pieces.");
            yield break;
        }

        if (target == null)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun slug killed the target before primary-shot verification.");
            yield break;
        }

        yield return new WaitForSeconds(weapon.secondaryCooldown + 0.05f);

        int ammoAfterSlug = inventory.Ammo;
        if (!weapon.FireOnce())
        {
            Fail("Weapon switch smoke failed: Steam Scattergun did not fire.");
            yield break;
        }

        RequireEqual(inventory.Ammo, ammoAfterSlug - weapon.ammoCost, "ammo after Steam Scattergun shot");
        if (!audio.HasLastOneShotCue || audio.LastOneShotCue != SteamworksAudioCue.SteamScattergunFire)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun did not use its dedicated audio cue.");
            yield break;
        }

        ScattergunBlastVfx blastVfx = UnityEngine.Object.FindAnyObjectByType<ScattergunBlastVfx>();
        if (blastVfx == null || blastVfx.PieceCount < 10)
        {
            Fail("Weapon switch smoke failed: Steam Scattergun blast VFX did not spawn with enough visible pieces.");
            yield break;
        }

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

        if (weapon.pressurePistolView == null || weapon.weaponView != weapon.pressurePistolView || !weapon.pressurePistolView.gameObject.activeSelf || weapon.steamScattergunView.gameObject.activeSelf)
        {
            Fail("Weapon switch smoke failed: Pressure Pistol viewmodel did not become active again.");
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

    private static Pickup FindWeaponPickup()
    {
        Pickup[] pickups = UnityEngine.Object.FindObjectsByType<Pickup>(FindObjectsSortMode.None);
        foreach (Pickup pickup in pickups)
        {
            if (pickup.kind == PickupKind.Weapon)
            {
                return pickup;
            }
        }

        throw new InvalidOperationException("Weapon switch smoke failed: missing weapon pickup.");
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

    private static void DisableSupportEnemies()
    {
        BellowsNodeController[] nodes = UnityEngine.Object.FindObjectsByType<BellowsNodeController>(FindObjectsSortMode.None);
        foreach (BellowsNodeController node in nodes)
        {
            node.gameObject.SetActive(false);
        }
    }

    private static void Teleport(PlayerController player, Vector3 position)
    {
        CharacterController playerController = player.GetComponent<CharacterController>();
        SetControllerEnabled(playerController, false);

        player.transform.position = position;
        player.transform.rotation = Quaternion.identity;
        if (player.playerCamera != null)
        {
            player.playerCamera.localRotation = Quaternion.identity;
        }

        SetControllerEnabled(playerController, true);
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
