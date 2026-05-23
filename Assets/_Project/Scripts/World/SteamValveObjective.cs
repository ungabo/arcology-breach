using UnityEngine;

public class SteamValveObjective : MonoBehaviour, IInteractable
{
    public string prompt = "E - vent boiler pressure";
    public string completePrompt = "boiler pressure vented";
    public string completeMessage = "Boilerheart pressure vented. Final lift unlocked.";
    public string objectiveAfterComplete = "Ride the foundry lift.";
    public GameObject lockedSignal;
    public GameObject ventedSignal;
    public SteamHazard[] hazardsToDisableOnComplete;

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
        DisableLinkedHazards();
        if (!string.IsNullOrWhiteSpace(objectiveAfterComplete))
        {
            GameStateController.Instance?.SetObjective(objectiveAfterComplete);
        }

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

    private void DisableLinkedHazards()
    {
        if (hazardsToDisableOnComplete == null)
        {
            return;
        }

        for (int i = 0; i < hazardsToDisableOnComplete.Length; i++)
        {
            SteamHazard hazard = hazardsToDisableOnComplete[i];
            if (hazard != null)
            {
                hazard.gameObject.SetActive(false);
            }
        }
    }
}
