using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public float openDistance = 2.2f;
    public float openHeight = 3.25f;
    public float openSpeed = 4.5f;

    private PlayerInventory playerInventory;
    private Transform player;
    private Collider doorCollider;
    private Vector3 closedPosition;
    private float nextLockedMessageTime;
    private bool opened;

    private void Start()
    {
        closedPosition = transform.position;
        doorCollider = GetComponent<Collider>();
        playerInventory = Object.FindFirstObjectByType<PlayerInventory>();
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
            nextLockedMessageTime = Time.time + 1f;
            HUDController.Instance?.ShowTemporaryMessage("Need the key", 0.8f);
        }
    }

    private void Open()
    {
        opened = true;
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        HUDController.Instance?.ShowTemporaryMessage("Door opened", 1f);
    }
}
