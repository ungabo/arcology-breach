using UnityEngine;

public class SteamValveObjective : MonoBehaviour, IInteractable
{
    public string prompt = "E - vent boiler pressure";
    public string completePrompt = "boiler pressure vented";
    public string completeMessage = "Boilerheart pressure vented. Final lift unlocked.";
    public GameObject lockedSignal;
    public GameObject ventedSignal;

    public bool IsComplete { get; private set; }
    public string Prompt => IsComplete ? completePrompt : prompt;

    private void Awake()
    {
        Collider triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }

        SetSignalState();
    }

    public bool CanInteract(GameObject interactor)
    {
        return !IsComplete && interactor.GetComponentInParent<PlayerController>() != null;
    }

    public void Interact(GameObject interactor)
    {
        if (!CanInteract(interactor))
        {
            return;
        }

        CompleteObjective();
    }

    public void CompleteObjective()
    {
        if (IsComplete)
        {
            return;
        }

        IsComplete = true;
        SetSignalState();
        HUDController.Instance?.ShowTemporaryMessage(completeMessage, 2.5f);
        SteamworksAudio.Play(SteamworksAudioCue.GateOpen);
    }

    private void SetSignalState()
    {
        if (lockedSignal != null)
        {
            lockedSignal.SetActive(!IsComplete);
        }

        if (ventedSignal != null)
        {
            ventedSignal.SetActive(IsComplete);
        }
    }
}
