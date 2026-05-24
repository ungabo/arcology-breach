using System;
using System.Collections;
using UnityEngine;

public class RuntimeBalanceEnvelopeTest : MonoBehaviour
{
    private const string BalanceArgument = "-v0BalanceSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(BalanceArgument))
        {
            yield break;
        }

        yield return null;

        PlayerInventory inventory = Require<PlayerInventory>("PlayerInventory");
        WeaponController weapon = Require<WeaponController>("WeaponController");

        RequireEqual(inventory.startingAmmo, GameBalance.StartingAmmo, "starting ammo");
        RequireWeaponDefinition(weapon.definition, WeaponController.PressurePistolId);
        RequireScattergunDefinition(weapon.steamScattergunDefinition);
        RequirePickupAmounts();
        RequireCombatEnvelope();

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_BALANCE_ENVELOPE_PASS");
        Application.Quit(0);
    }

    private void RequireWeaponDefinition(WeaponDefinition definition, string weaponId)
    {
        if (definition == null)
        {
            Fail("Balance envelope failed: missing weapon definition " + weaponId + ".");
            return;
        }

        RequireState(string.Equals(definition.weaponId, weaponId, StringComparison.OrdinalIgnoreCase), weaponId + " id");
        RequireEqual(definition.damage, GameBalance.PressurePistolDamage, weaponId + " damage");
        RequireEqual(definition.ammoCost, GameBalance.PressurePistolAmmoCost, weaponId + " ammo cost");
        RequireEqual(definition.pelletCount, GameBalance.PressurePistolPelletCount, weaponId + " pellet count");
        RequireApprox(definition.fireCooldown, GameBalance.PressurePistolCooldown, weaponId + " cooldown");
        RequireApprox(definition.range, GameBalance.PressurePistolRange, weaponId + " range");
        RequireEqual(definition.secondaryDamage, GameBalance.PressureBurstDamage, weaponId + " secondary damage");
        RequireEqual(definition.secondaryPelletCount, GameBalance.PressureBurstPelletCount, weaponId + " secondary pellet count");
        RequireEqual(definition.secondaryAmmoCost, GameBalance.PressureBurstAmmoCost, weaponId + " secondary ammo cost");
    }

    private void RequireScattergunDefinition(WeaponDefinition definition)
    {
        if (definition == null)
        {
            Fail("Balance envelope failed: missing Steam Scattergun definition.");
            return;
        }

        RequireState(string.Equals(definition.weaponId, WeaponController.SteamScattergunId, StringComparison.OrdinalIgnoreCase), "scattergun id");
        RequireEqual(definition.damage, GameBalance.SteamScattergunDamage, "scattergun damage");
        RequireEqual(definition.ammoCost, GameBalance.SteamScattergunAmmoCost, "scattergun ammo cost");
        RequireEqual(definition.pelletCount, GameBalance.SteamScattergunPelletCount, "scattergun pellet count");
        RequireApprox(definition.fireCooldown, GameBalance.SteamScattergunCooldown, "scattergun cooldown");
        RequireEqual(definition.secondaryDamage, GameBalance.SteamScattergunSlugDamage, "scattergun slug damage");
        RequireEqual(definition.secondaryAmmoCost, GameBalance.SteamScattergunSlugAmmoCost, "scattergun slug ammo cost");
        RequireApprox(definition.secondaryCooldown, GameBalance.SteamScattergunSlugCooldown, "scattergun slug cooldown");
    }

    private void RequirePickupAmounts()
    {
        bool sawHealth = false;
        bool sawAmmo = false;

        Pickup[] pickups = UnityEngine.Object.FindObjectsByType<Pickup>(FindObjectsSortMode.None);
        foreach (Pickup pickup in pickups)
        {
            if (pickup.kind == PickupKind.Health)
            {
                sawHealth = true;
                RequireEqual(pickup.amount, GameBalance.HealthPickupAmount, "health pickup amount");
            }

            if (pickup.kind == PickupKind.Ammo)
            {
                sawAmmo = true;
                RequireEqual(pickup.amount, GameBalance.AmmoPickupAmount, "ammo pickup amount");
            }
        }

        RequireState(sawHealth, "health pickup present");
        RequireState(sawAmmo, "ammo pickup present");
    }

    private void RequireCombatEnvelope()
    {
        int pressureBurstDamage = GameBalance.PressureBurstDamage * GameBalance.PressureBurstPelletCount;
        int scattergunPrimaryDamage = GameBalance.SteamScattergunDamage * GameBalance.SteamScattergunPelletCount;
        int scrapperPistolShots = ShotsToDefeat(GameBalance.ScrapperHealth, GameBalance.PressurePistolDamage);
        int scrapperAfterBurstShots = ShotsToDefeat(GameBalance.ScrapperHealth - pressureBurstDamage, GameBalance.PressurePistolDamage);

        RequireEqual(scrapperPistolShots, 3, "Scrapper pistol-shot baseline");
        RequireState(pressureBurstDamage < GameBalance.ScrapperHealth, "Pressure Burst leaves Scrapper alive");
        RequireEqual(scrapperAfterBurstShots, 2, "Pressure Burst follow-up pistol shots");
        RequireState(GameBalance.SteamScattergunSlugDamage < GameBalance.ScrapperHealth, "Scattergun slug leaves Scrapper alive");
        RequireState(scattergunPrimaryDamage >= GameBalance.ScrapperHealth, "Scattergun primary defeats Scrapper at close range");
        RequireState(scattergunPrimaryDamage < GameBalance.BulwarkHealth, "Scattergun primary does not delete Bulwark");

        RequireEqual(ShotsToDefeat(GameBalance.LancerHealth, GameBalance.PressurePistolDamage), 2, "Lancer pistol-shot baseline");
        RequireEqual(ShotsToDefeat(GameBalance.BellowsNodeHealth, GameBalance.PressurePistolDamage), 3, "Bellows Node pistol-shot baseline");
        RequireRange(ShotsToDefeat(GameBalance.BulwarkHealth, GameBalance.PressurePistolDamage), 5, 7, "Bulwark pistol-shot baseline");
        RequireRange(ShotsToDefeat(GameBalance.GovernorWardenHealth, GameBalance.PressurePistolDamage), 10, 13, "Governor Warden pistol-shot baseline");

        RequireState(GameBalance.StartingAmmo >= ShotsToDefeat(GameBalance.GovernorWardenHealth, GameBalance.PressurePistolDamage) + 8, "starting ammo supports route reserve");
        RequireState(GameBalance.AmmoPickupAmount >= GameBalance.PressureBurstAmmoCost + GameBalance.SteamScattergunAmmoCost, "ammo pickup supports special shots");
        RequireState(GameBalance.ScrapperAttackDamage < GameBalance.HealthPickupAmount, "Scrapper damage is recoverable");
        RequireState(GameBalance.LancerProjectileDamage < GameBalance.HealthPickupAmount, "Lancer damage is recoverable");
        RequireState(GameBalance.BellowsNodePulseDamage < GameBalance.HealthPickupAmount, "Bellows pulse damage is recoverable");
        RequireState(GameBalance.BulwarkAttackDamage <= GameBalance.HealthPickupAmount, "Bulwark slam damage is recoverable");
        RequireState(GameBalance.GovernorWardenStompDamage <= GameBalance.HealthPickupAmount, "Warden stomp damage is recoverable");
    }

    private static int ShotsToDefeat(int health, int damage)
    {
        return Mathf.CeilToInt(Mathf.Max(1, health) / (float)Mathf.Max(1, damage));
    }

    private void RequireRange(int actual, int minimum, int maximum, string label)
    {
        if (actual < minimum || actual > maximum)
        {
            Fail("Balance envelope failed: " + label + " expected " + minimum + "-" + maximum + " but found " + actual + ".");
        }
    }

    private void RequireEqual(int actual, int expected, string label)
    {
        if (actual != expected)
        {
            Fail("Balance envelope failed: " + label + " expected " + expected + " but found " + actual + ".");
        }
    }

    private void RequireApprox(float actual, float expected, string label)
    {
        if (!Mathf.Approximately(actual, expected))
        {
            Fail("Balance envelope failed: " + label + " expected " + expected + " but found " + actual + ".");
        }
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Balance envelope failed: " + label + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Balance envelope failed: missing " + label + ".");
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
