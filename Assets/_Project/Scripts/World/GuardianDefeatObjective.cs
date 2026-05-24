using UnityEngine;

public class GuardianDefeatObjective : MonoBehaviour
{
    public GovernorWardenController target;
    public string completeMessage = "Governor Warden destroyed. Master override hoist unlocked.";
    public GameObject lockedSignal;
    public GameObject clearedSignal;

    public bool IsComplete { get; private set; }

    private void Awake()
    {
        SetSignalState();
    }

    private void Update()
    {
        if (!IsComplete && target == null)
        {
            CompleteObjective();
        }
    }

    public void CompleteObjective()
    {
        if (IsComplete)
        {
            return;
        }

        IsComplete = true;
        SetSignalState();
        GameStateController.Instance?.SetObjective("Engage the master override hoist.");
        HUDController.Instance?.ShowTemporaryMessage(completeMessage, 2.5f);
        SteamworksAudio.Play(SteamworksAudioCue.GateOpen);
        GameplayFeedbackController.ReportWorld(GameplayFeedbackEventType.ObjectiveCompleted, "governor_warden_guardian_cleared", transform.position + Vector3.up * 0.75f, new Color(0.28f, 0.95f, 0.48f));
    }

    private void SetSignalState()
    {
        if (lockedSignal != null)
        {
            lockedSignal.SetActive(!IsComplete);
        }

        if (clearedSignal != null)
        {
            clearedSignal.SetActive(IsComplete);
        }
    }
}
