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
        HUDController.Instance?.ShowTemporaryMessage(completeMessage, 2.5f);
        SteamworksAudio.Play(SteamworksAudioCue.GateOpen);
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
