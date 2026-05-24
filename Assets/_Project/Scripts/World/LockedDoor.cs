using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    public float openDistance = 2.2f;
    public float openHeight = 3.25f;
    public float openSpeed = 4.5f;
    public string unlockedPrompt = "E - crank pressure gate";
    public string lockedPrompt = "Gear key required";

    private PlayerInventory playerInventory;
    private Transform player;
    private Collider doorCollider;
    private Vector3 closedPosition;
    private float nextLockedMessageTime;
    private bool opened;

    public string Prompt
    {
        get
        {
            if (opened)
            {
                return string.Empty;
            }

            PlayerInventory inventory = playerInventory != null ? playerInventory : Object.FindAnyObjectByType<PlayerInventory>();
            return inventory != null && inventory.HasKey ? unlockedPrompt : lockedPrompt;
        }
    }

    private void Start()
    {
        closedPosition = transform.position;
        doorCollider = GetComponent<Collider>();
        playerInventory = Object.FindAnyObjectByType<PlayerInventory>();
        if (playerInventory != null)
        {
            player = playerInventory.transform;
        }
    }

    private void Update()
    {
        if (opened)
        {
            Vector3 target = closedPosition + Vector3.up * openHeight;
            transform.position = Vector3.MoveTowards(transform.position, target, openSpeed * Time.deltaTime);
            return;
        }

        if (player == null || playerInventory == null)
        {
            return;
        }

        if (GameStateController.Instance != null && !GameStateController.Instance.IsGameplayActive)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > openDistance)
        {
            return;
        }

        if (playerInventory.HasKey)
        {
            Open();
        }
        else if (Time.time >= nextLockedMessageTime)
        {
            ShowLockedFeedback();
        }
    }

    public bool CanInteract(GameObject interactor)
    {
        return !opened && interactor.GetComponentInParent<PlayerController>() != null;
    }

    public void Interact(GameObject interactor)
    {
        if (opened || GameStateController.Instance != null && !GameStateController.Instance.IsGameplayActive)
        {
            return;
        }

        PlayerInventory inventory = interactor.GetComponentInParent<PlayerInventory>();
        if (inventory != null && inventory.HasKey)
        {
            Open();
            return;
        }

        ShowLockedFeedback();
    }

    private void ShowLockedFeedback()
    {
        nextLockedMessageTime = Time.time + 1f;
        SteamworksAudio.Play(SteamworksAudioCue.GateDenied);
        HUDController.Instance?.ShowTemporaryMessage(lockedPrompt, 0.8f);
        HUDController.Instance?.FlashKeyDenied(0.75f);
    }

    private void Open()
    {
        opened = true;
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        GateOpenVfx.Spawn(transform.position - transform.forward * 0.6f + Vector3.up * 0.55f, transform.rotation);
        SteamworksAudio.Play(SteamworksAudioCue.GateOpen);
        GameStateController.Instance?.SetObjective("Ride the service lift.");
        HUDController.Instance?.ShowTemporaryMessage("Pressure gate opened", 1f);
    }
}
