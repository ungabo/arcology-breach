using UnityEngine;

public enum PickupKind
{
    Health,
    Ammo,
    Key
}

public class Pickup : MonoBehaviour
{
    public PickupKind kind;
    public int amount = 25;
    public float collectRadius = 0.9f;
    public float spinDegreesPerSecond = 90f;
    public float bobAmplitude = 0.12f;
    public float bobSpeed = 3f;

    private Transform player;
    private Vector3 basePosition;
    private bool collected;

    private void Awake()
    {
        Collider pickupCollider = GetComponent<Collider>();
        if (pickupCollider != null)
        {
            pickupCollider.isTrigger = true;
        }
    }

    private void Start()
    {
        basePosition = transform.position;

        PlayerController playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, spinDegreesPerSecond * Time.deltaTime, Space.World);
        transform.position = basePosition + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobAmplitude);

        if (!collected && player != null && Vector3.Distance(transform.position, player.position) <= collectRadius)
        {
            Collect(player.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collect(other.gameObject);
    }

    private void Collect(GameObject collector)
    {
        if (collected)
        {
            return;
        }

        PlayerInventory inventory = collector.GetComponentInParent<PlayerInventory>();
        PlayerHealth health = collector.GetComponentInParent<PlayerHealth>();

        if (inventory == null && health == null)
        {
            return;
        }

        collected = true;

        switch (kind)
        {
            case PickupKind.Health:
                health?.Heal(amount);
                HUDController.Instance?.ShowTemporaryMessage($"+{amount} health", 1f);
                break;
            case PickupKind.Ammo:
                inventory?.AddAmmo(amount);
                break;
            case PickupKind.Key:
                inventory?.AddKey();
                break;
        }

        Destroy(gameObject);
    }
}
