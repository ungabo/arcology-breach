using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int startingAmmo = 30;

    public int Ammo { get; private set; }
    public bool HasKey { get; private set; }
    public bool HasSteamScattergun { get; private set; }

    private void Awake()
    {
        Ammo = startingAmmo;
    }

    private void Start()
    {
        UpdateHud();
    }

    public bool TryUseAmmo(int amount = 1)
    {
        if (amount <= 0)
        {
            return true;
        }

        if (Ammo < amount)
        {
            return false;
        }

        Ammo -= amount;
        UpdateHud();
        return true;
    }

    public void AddAmmo(int amount, bool showMessage = true)
    {
        if (amount <= 0)
        {
            return;
        }

        Ammo += amount;
        UpdateHud();
        if (showMessage)
        {
            HUDController.Instance?.ShowTemporaryMessage($"+{amount} ammo", 1f);
        }
    }

    public void AddKey(bool showMessage = true)
    {
        HasKey = true;
        UpdateHud();
        if (showMessage)
        {
            HUDController.Instance?.ShowTemporaryMessage("Gear key acquired", 1.5f);
        }
    }

    public void UnlockSteamScattergun(bool showMessage = true)
    {
        HasSteamScattergun = true;
        WeaponController weapon = GetComponent<WeaponController>();
        if (weapon != null)
        {
            weapon.UnlockSteamScattergun(switchToWeapon: true, showMessage: false);
        }

        if (showMessage)
        {
            HUDController.Instance?.ShowTemporaryMessage("Steam Scattergun acquired", 1.2f);
        }
    }

    public void RestoreForTransition(int ammo)
    {
        RestoreForTransition(ammo, HasSteamScattergun);
    }

    public void RestoreForTransition(int ammo, bool hasSteamScattergun)
    {
        Ammo = Mathf.Max(0, ammo);
        HasKey = false;
        HasSteamScattergun = hasSteamScattergun;
        WeaponController weapon = GetComponent<WeaponController>();
        if (weapon != null)
        {
            weapon.SetSteamScattergunUnlocked(HasSteamScattergun);
        }

        UpdateHud();
    }

    private void UpdateHud()
    {
        HUDController.Instance?.SetAmmo(Ammo);
        HUDController.Instance?.SetKey(HasKey);
    }
}
