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
        GameObject root = new GameObject("Impact Sparks");
        root.transform.position = point + normal * 0.04f;
        root.transform.rotation = Quaternion.LookRotation(normal);

        for (int i = 0; i < 6; i++)
        {
            float side = i - 2.5f;
            GameObject spark = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spark.name = "Impact Spark " + i;
            spark.transform.SetParent(root.transform, false);
            spark.transform.localPosition = new Vector3(side * 0.025f, Mathf.Abs(side) * 0.015f, 0.03f + i * 0.012f);
            spark.transform.localRotation = Quaternion.Euler(0f, 0f, side * 19f);
            spark.transform.localScale = new Vector3(0.018f, 0.018f, 0.16f + i * 0.012f);

            Collider sparkCollider = spark.GetComponent<Collider>();
            if (sparkCollider != null)
            {
                Destroy(sparkCollider);
            }

            Renderer sparkRenderer = spark.GetComponent<Renderer>();
            if (sparkRenderer != null)
            {
                sparkRenderer.material.color = i % 2 == 0 ? new Color(1f, 0.62f, 0.08f) : new Color(1f, 0.92f, 0.32f);
            }
        }

        Destroy(root, 0.25f);
    }
}
