using UnityEngine;

public static class RunProgress
{
    public static bool HasSnapshot { get; private set; }
    public static int CurrentHealth { get; private set; }
    public static int MaxHealth { get; private set; }
    public static int Ammo { get; private set; }

    public static void Reset()
    {
        HasSnapshot = false;
        CurrentHealth = 0;
        MaxHealth = 0;
        Ammo = 0;
    }

    public static void Capture(PlayerHealth health, PlayerInventory inventory)
    {
        if (health == null || inventory == null)
        {
            return;
        }

        HasSnapshot = true;
        CurrentHealth = Mathf.Max(1, health.CurrentHealth);
        MaxHealth = Mathf.Max(1, health.maxHealth);
        Ammo = Mathf.Max(0, inventory.Ammo);
    }

    public static void ApplyTo(PlayerHealth health, PlayerInventory inventory)
    {
        if (!HasSnapshot)
        {
            return;
        }

        health?.RestoreForTransition(CurrentHealth, MaxHealth);
        inventory?.RestoreForTransition(Ammo);
    }
}
