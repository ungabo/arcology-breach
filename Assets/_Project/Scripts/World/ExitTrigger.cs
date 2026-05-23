using UnityEngine;

public class ExitTrigger : MonoBehaviour, IInteractable
{
    public float fallbackWinRadius = 1.25f;
    public string prompt = "E - engage final lift";

    private Transform player;
    private bool triggered;

    public string Prompt => triggered ? string.Empty : prompt;

    private void Start()
    {
        PlayerController playerController = Object.FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
    }

    private void Update()
    {
        if (!triggered && player != null && Vector3.Distance(transform.position, player.position) <= fallbackWinRadius)
        {
            TriggerWin(player.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerWin(other.gameObject);
    }

    public bool CanInteract(GameObject interactor)
    {
        return !triggered && interactor.GetComponentInParent<PlayerController>() != null;
    }

    public void Interact(GameObject interactor)
    {
        TriggerWin(interactor);
    }

    private void TriggerWin(GameObject other)
    {
        if (triggered || other.GetComponentInParent<PlayerController>() == null)
        {
            return;
        }

        triggered = true;
        GameStateController.Instance?.PlayerWon();
    }
}
