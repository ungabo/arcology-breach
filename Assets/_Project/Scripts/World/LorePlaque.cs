using UnityEngine;

public class LorePlaque : MonoBehaviour, IInteractable
{
    public string plaqueId = "lore-plaque";
    public string title = "Archive Plaque";
    [TextArea(2, 5)]
    public string body = "A worn brass plaque records a fragment of the Brassworks.";
    public string prompt = "E - read plaque";
    public float readSeconds = 4.2f;

    public string Prompt => prompt;
    public bool WasRead { get; private set; }

    public bool CanInteract(GameObject interactor)
    {
        return interactor != null && interactor.GetComponentInParent<PlayerController>() != null;
    }

    public void Interact(GameObject interactor)
    {
        if (!CanInteract(interactor))
        {
            return;
        }

        WasRead = true;
        HUDController.Instance?.ShowTemporaryMessage(title + ": " + body, readSeconds);
    }
}
