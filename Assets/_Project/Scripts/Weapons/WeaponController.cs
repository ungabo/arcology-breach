using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponDefinition definition;
    public Camera aimCamera;
    public PlayerInventory inventory;
    public float range = 40f;
    public int damage = 25;
    public float fireCooldown = 0.25f;
    public int secondaryDamage = GameBalance.PressureBurstDamage;
    public int secondaryPelletCount = GameBalance.PressureBurstPelletCount;
    public int secondaryAmmoCost = GameBalance.PressureBurstAmmoCost;
    public float secondaryCooldown = GameBalance.PressureBurstCooldown;
    public float secondaryRange = GameBalance.PressureBurstRange;
    public float secondarySpread = GameBalance.PressureBurstSpread;
    public LayerMask hitMask = ~0;
    public WeaponView weaponView;

    private float nextFireTime;

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

        ApplyDefinition();
    }

    private void ApplyDefinition()
    {
        if (definition == null)
        {
            return;
        }

        range = definition.range;
        damage = definition.damage;
        fireCooldown = definition.fireCooldown;
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
        return FirePattern(1, damage, range, 1, 0f, fireCooldown, "No ammo");
    }

    public bool FireSecondary()
    {
        return FirePattern(secondaryAmmoCost, secondaryDamage, secondaryRange, secondaryPelletCount, secondarySpread, secondaryCooldown, "Not enough pressure");
    }

    private bool FirePattern(int ammoCost, int shotDamage, float shotRange, int pelletCount, float spread, float cooldown, string emptyMessage)
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

        SteamworksAudio.Play(SteamworksAudioCue.PressureFire);
        weaponView?.PlayFire();

        Ray baseRay = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
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
            default:
                return Vector2.zero;
        }
    }

    private static void SpawnHitMarker(Vector3 point, Vector3 normal)
    {
        ImpactDecalVfx.Spawn(point, normal);
    }
}
