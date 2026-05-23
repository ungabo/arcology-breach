using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponDefinition definition;
    public Camera aimCamera;
    public PlayerInventory inventory;
    public float range = 40f;
    public int damage = 25;
    public float fireCooldown = 0.25f;
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
    }

    private void Update()
    {
        if (GameStateController.Instance != null && !GameStateController.Instance.IsGameplayActive)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            FireOnce();
        }
    }

    public bool FireOnce()
    {
        if (Time.time < nextFireTime)
        {
            return false;
        }

        nextFireTime = Time.time + fireCooldown;

        if (inventory != null && !inventory.TryUseAmmo())
        {
            SteamworksAudio.Play(SteamworksAudioCue.EmptyClick);
            HUDController.Instance?.ShowTemporaryMessage("No ammo", 0.75f);
            return false;
        }

        SteamworksAudio.Play(SteamworksAudioCue.PressureFire);
        weaponView?.PlayFire();

        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
            damageable?.TakeDamage(damage);
            SpawnHitMarker(hit.point, hit.normal);
        }

        return true;
    }

    private static void SpawnHitMarker(Vector3 point, Vector3 normal)
    {
        ImpactDecalVfx.Spawn(point, normal);
    }
}
