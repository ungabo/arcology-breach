using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public const string PressurePistolId = "pressure_pistol";
    public const string SteamScattergunId = "steam_scattergun";

    public WeaponDefinition definition;
    public WeaponDefinition steamScattergunDefinition;
    public Camera aimCamera;
    public PlayerInventory inventory;
    public float range = 40f;
    public int damage = 25;
    public int ammoCost = GameBalance.PressurePistolAmmoCost;
    public int pelletCount = GameBalance.PressurePistolPelletCount;
    public float fireCooldown = 0.25f;
    public float spread = GameBalance.PressurePistolSpread;
    public int secondaryDamage = GameBalance.PressureBurstDamage;
    public int secondaryPelletCount = GameBalance.PressureBurstPelletCount;
    public int secondaryAmmoCost = GameBalance.PressureBurstAmmoCost;
    public float secondaryCooldown = GameBalance.PressureBurstCooldown;
    public float secondaryRange = GameBalance.PressureBurstRange;
    public float secondarySpread = GameBalance.PressureBurstSpread;
    public LayerMask hitMask = ~0;
    public WeaponView weaponView;
    public WeaponView pressurePistolView;
    public WeaponView steamScattergunView;

    private WeaponDefinition pressurePistolDefinition;
    private float nextFireTime;
    private bool hasSteamScattergun;
    private bool usingSteamScattergun;

    public bool HasSteamScattergun => hasSteamScattergun;
    public bool IsUsingSteamScattergun => usingSteamScattergun;
    public string ActiveWeaponName => definition != null ? definition.displayName : "Pressure Pistol";

    private void Awake()
    {
        if (aimCamera == null)
        {
            aimCamera = Camera.main;
        }

        if (inventory == null)
        {
            inventory = GetComponent<PlayerInventory>();
        }

        if (weaponView == null)
        {
            weaponView = GetComponentInChildren<WeaponView>();
        }

        if (pressurePistolView == null)
        {
            pressurePistolView = weaponView;
        }

        pressurePistolDefinition = definition;
        ApplyDefinition();
        SetActiveWeaponView(pressurePistolView);
    }

    private void ApplyDefinition()
    {
        if (definition == null)
        {
            return;
        }

        range = definition.range;
        damage = definition.damage;
        ammoCost = definition.ammoCost;
        pelletCount = definition.pelletCount;
        fireCooldown = definition.fireCooldown;
        spread = definition.spread;
        secondaryDamage = definition.secondaryDamage;
        secondaryPelletCount = definition.secondaryPelletCount;
        secondaryAmmoCost = definition.secondaryAmmoCost;
        secondaryCooldown = definition.secondaryCooldown;
        secondaryRange = definition.secondaryRange;
        secondarySpread = definition.secondarySpread;
    }

    private void Update()
    {
        if (GameStateController.Instance != null && !GameStateController.Instance.IsGameplayActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipPressurePistol();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipSteamScattergun();
        }

        if (Input.GetMouseButton(1))
        {
            FireSecondary();
        }
        else if (Input.GetMouseButton(0))
        {
            FireOnce();
        }
    }

    public bool FireOnce()
    {
        return FirePattern(ammoCost, damage, range, pelletCount, spread, fireCooldown, "No ammo", secondaryShot: false);
    }

    public bool FireSecondary()
    {
        return FirePattern(secondaryAmmoCost, secondaryDamage, secondaryRange, secondaryPelletCount, secondarySpread, secondaryCooldown, "Not enough pressure", secondaryShot: true);
    }

    public void UnlockSteamScattergun(bool switchToWeapon = true, bool showMessage = true)
    {
        if (steamScattergunDefinition == null)
        {
            return;
        }

        hasSteamScattergun = true;
        if (switchToWeapon)
        {
            EquipSteamScattergun(showMessage);
        }
        else if (showMessage)
        {
            HUDController.Instance?.ShowTemporaryMessage("Steam Scattergun unlocked", 1.1f);
        }
    }

    public void SetSteamScattergunUnlocked(bool unlocked)
    {
        hasSteamScattergun = unlocked && steamScattergunDefinition != null;
        if (!hasSteamScattergun && usingSteamScattergun)
        {
            EquipPressurePistol(showMessage: false);
        }
    }

    public bool EquipPressurePistol(bool showMessage = true)
    {
        if (pressurePistolDefinition == null)
        {
            pressurePistolDefinition = definition;
        }

        if (pressurePistolDefinition == null)
        {
            return false;
        }

        usingSteamScattergun = false;
        definition = pressurePistolDefinition;
        ApplyDefinition();
        SetActiveWeaponView(pressurePistolView);
        if (showMessage)
        {
            HUDController.Instance?.ShowTemporaryMessage("Pressure Pistol ready", 0.9f);
        }

        return true;
    }

    public bool EquipSteamScattergun(bool showMessage = true)
    {
        if (!hasSteamScattergun || steamScattergunDefinition == null)
        {
            if (showMessage)
            {
                HUDController.Instance?.ShowTemporaryMessage("Steam Scattergun not acquired", 0.9f);
            }

            return false;
        }

        usingSteamScattergun = true;
        definition = steamScattergunDefinition;
        ApplyDefinition();
        SetActiveWeaponView(steamScattergunView != null ? steamScattergunView : pressurePistolView);
        if (showMessage)
        {
            HUDController.Instance?.ShowTemporaryMessage("Steam Scattergun ready", 0.9f);
        }

        return true;
    }

    private void SetActiveWeaponView(WeaponView activeView)
    {
        if (pressurePistolView != null)
        {
            pressurePistolView.gameObject.SetActive(activeView == pressurePistolView);
        }

        if (steamScattergunView != null)
        {
            steamScattergunView.gameObject.SetActive(activeView == steamScattergunView);
        }

        if (activeView != null)
        {
            weaponView = activeView;
        }
    }

    private bool FirePattern(int ammoCost, int shotDamage, float shotRange, int pelletCount, float spread, float cooldown, string emptyMessage, bool secondaryShot)
    {
        if (Time.time < nextFireTime)
        {
            return false;
        }

        nextFireTime = Time.time + cooldown;

        if (inventory != null && !inventory.TryUseAmmo(ammoCost))
        {
            SteamworksAudio.Play(SteamworksAudioCue.EmptyClick);
            HUDController.Instance?.ShowTemporaryMessage(emptyMessage, 0.75f);
            return false;
        }

        SteamworksAudioCue fireCue = usingSteamScattergun
            ? (secondaryShot ? SteamworksAudioCue.SteamScattergunSlug : SteamworksAudioCue.SteamScattergunFire)
            : (secondaryShot ? SteamworksAudioCue.PressureBurst : SteamworksAudioCue.PressureFire);
        SteamworksAudio.Play(fireCue);
        weaponView?.PlayFire(secondaryShot);

        Ray baseRay = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (usingSteamScattergun)
        {
            if (secondaryShot)
            {
                ScattergunSlugVfx.Spawn(baseRay.origin + baseRay.direction * 0.9f, baseRay.direction);
            }
            else
            {
                ScattergunBlastVfx.Spawn(baseRay.origin + baseRay.direction * 0.85f, baseRay.direction);
            }
        }
        else if (secondaryShot)
        {
            PressureBurstVfx.Spawn(baseRay.origin + baseRay.direction * 0.78f, baseRay.direction);
        }

        int resolvedPelletCount = Mathf.Max(1, pelletCount);
        for (int pelletIndex = 0; pelletIndex < resolvedPelletCount; pelletIndex++)
        {
            Vector2 offset = GetPelletOffset(pelletIndex) * spread;
            Vector3 direction = (baseRay.direction + aimCamera.transform.right * offset.x + aimCamera.transform.up * offset.y).normalized;
            if (Physics.Raycast(baseRay.origin, direction, out RaycastHit hit, shotRange, hitMask, QueryTriggerInteraction.Ignore))
            {
                IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
                damageable?.TakeDamage(shotDamage);
                SpawnHitMarker(hit.point, hit.normal);
            }
        }

        return true;
    }

    private static Vector2 GetPelletOffset(int pelletIndex)
    {
        switch (pelletIndex)
        {
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.right;
            case 3:
                return Vector2.up;
            case 4:
                return Vector2.down;
            case 5:
                return new Vector2(0.72f, 0.72f);
            case 6:
                return new Vector2(-0.72f, -0.72f);
            default:
                return Vector2.zero;
        }
    }

    private static void SpawnHitMarker(Vector3 point, Vector3 normal)
    {
        ImpactDecalVfx.Spawn(point, normal);
    }
}
