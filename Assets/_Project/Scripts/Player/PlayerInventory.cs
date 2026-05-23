using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int startingAmmo = 30;

    public int Ammo { get; private set; }
    public bool HasKey { get; private set; }

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

    public void AddAmmo(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Ammo += amount;
        UpdateHud();
        HUDController.Instance?.ShowTemporaryMessage($"+{amount} ammo", 1f);
    }

    public void AddKey()
    {
        HasKey = true;
        UpdateHud();
        HUDController.Instance?.ShowTemporaryMessage("Key collected", 1.5f);
    }

    private void UpdateHud()
    {
        HUDController.Instance?.SetAmmo(Ammo);
        HUDController.Instance?.SetKey(HasKey);
    }
}
