using UnityEngine;

public enum PickupKind
{
    Health,
    Ammo,
    Key,
    Weapon
}

public class Pickup : MonoBehaviour
{
    public PickupDefinition definition;
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
        ApplyDefinition();

        Collider pickupCollider = GetComponent<Collider>();
        if (pickupCollider != null)
        {
            pickupCollider.isTrigger = true;
        }
    }

    private void Start()
    {
        basePosition = transform.position;

        PlayerController playerController = Object.FindAnyObjectByType<PlayerController>();
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

        if (kind == PickupKind.Key)
        {
            GearKeyPickupVfx.Spawn(transform.position + Vector3.up * 0.25f);
        }
        else if (kind == PickupKind.Weapon)
        {
            WeaponPickupVfx.Spawn(transform.position + Vector3.up * 0.24f);
        }
        else
        {
            ResourcePickupVfx.Spawn(transform.position + Vector3.up * 0.2f, kind);
        }

        GameplayFeedbackController.ReportWorld(GameplayFeedbackEventType.PickupCollected, kind.ToString(), transform.position + Vector3.up * 0.24f, GetFeedbackColor());
        ApplyPickup(inventory, health);
        SteamworksAudio.Play(GetAudioCue());

        string collectMessage = GetCollectMessage();
        if (!string.IsNullOrWhiteSpace(collectMessage))
        {
            HUDController.Instance?.ShowTemporaryMessage(collectMessage, 1f);
        }

        Destroy(gameObject);
    }

    private void ApplyDefinition()
    {
        if (definition == null)
        {
            return;
        }

        kind = definition.kind;
        amount = definition.amount;
        collectRadius = definition.collectRadius;
        spinDegreesPerSecond = definition.spinDegreesPerSecond;
        bobAmplitude = definition.bobAmplitude;
        bobSpeed = definition.bobSpeed;
    }

    private void ApplyPickup(PlayerInventory inventory, PlayerHealth health)
    {
        switch (kind)
        {
            case PickupKind.Health:
                health?.Heal(amount);
                break;
            case PickupKind.Ammo:
                inventory?.AddAmmo(amount, showMessage: false);
                break;
            case PickupKind.Key:
                inventory?.AddKey(showMessage: false);
                GameStateController.Instance?.SetObjective("Return to the pressure gate.");
                break;
            case PickupKind.Weapon:
                if (inventory != null && IsSteamScattergunPickup())
                {
                    inventory.UnlockSteamScattergun(showMessage: false);
                }
                break;
        }
    }

    private SteamworksAudioCue GetAudioCue()
    {
        if (definition != null)
        {
            return definition.audioCue;
        }

        switch (kind)
        {
            case PickupKind.Health:
                return SteamworksAudioCue.HealthPickup;
            case PickupKind.Ammo:
                return SteamworksAudioCue.AmmoPickup;
            case PickupKind.Key:
                return SteamworksAudioCue.GearKey;
            case PickupKind.Weapon:
                return SteamworksAudioCue.WeaponPickup;
            default:
                return SteamworksAudioCue.AmmoPickup;
        }
    }

    private string GetCollectMessage()
    {
        if (definition != null && !string.IsNullOrWhiteSpace(definition.collectMessage))
        {
            return definition.collectMessage;
        }

        switch (kind)
        {
            case PickupKind.Health:
                return $"+{amount} health";
            case PickupKind.Ammo:
                return $"+{amount} ammo";
            case PickupKind.Key:
                return "Gear key acquired";
            case PickupKind.Weapon:
                return "Weapon acquired";
            default:
                return string.Empty;
        }
    }

    private Color GetFeedbackColor()
    {
        switch (kind)
        {
            case PickupKind.Health:
                return new Color(0.95f, 0.18f, 0.18f);
            case PickupKind.Ammo:
                return new Color(0.95f, 0.72f, 0.18f);
            case PickupKind.Key:
                return new Color(1f, 0.82f, 0.26f);
            case PickupKind.Weapon:
                return new Color(1f, 0.58f, 0.1f);
            default:
                return Color.white;
        }
    }

    private bool IsSteamScattergunPickup()
    {
        return definition == null || string.Equals(definition.weaponUnlockId, WeaponController.SteamScattergunId, System.StringComparison.OrdinalIgnoreCase);
    }
}
