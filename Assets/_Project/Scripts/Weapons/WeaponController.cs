using UnityEngine;

public class WeaponController : MonoBehaviour
{
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
    }

    private void Update()
    {
        if (GameStateController.Instance != null && !GameStateController.Instance.IsGameplayActive)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            TryFire();
        }
    }

    private void TryFire()
    {
        if (Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + fireCooldown;

        if (inventory != null && !inventory.TryUseAmmo())
        {
            CyberpunkAudio.Play(CyberpunkAudioCue.EmptyClick);
            HUDController.Instance?.ShowTemporaryMessage("No ammo", 0.75f);
            return;
        }

        CyberpunkAudio.Play(CyberpunkAudioCue.PulseFire);
        weaponView?.PlayFire();

        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
            damageable?.TakeDamage(damage);
            SpawnHitMarker(hit.point, hit.normal);
        }
    }

    private static void SpawnHitMarker(Vector3 point, Vector3 normal)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.name = "Hit Marker";
        marker.transform.position = point + normal * 0.04f;
        marker.transform.localScale = Vector3.one * 0.12f;

        Collider markerCollider = marker.GetComponent<Collider>();
        if (markerCollider != null)
        {
            Destroy(markerCollider);
        }

        Renderer renderer = marker.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }

        Destroy(marker, 0.2f);
    }
}
