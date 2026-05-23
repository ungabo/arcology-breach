using UnityEngine;

public class ExitTrigger : MonoBehaviour, IInteractable
{
    public float fallbackWinRadius = 1.25f;
    public string prompt = "E - engage final lift";
    public string lockedPrompt = "vent boiler pressure first";
    public string guardianLockedPrompt = "defeat guardian first";
    public string lockedMessage = "The final lift is pressure-locked. Vent the Boilerheart first.";
    public string guardianLockedMessage = "The master override hoist is guarded. Destroy the Governor Warden first.";
    public SteamValveObjective requiredValve;
    public GuardianDefeatObjective requiredGuardian;

    private Transform player;
    private bool triggered;
    private float lastLockedFeedbackTime = -10f;

    public bool IsLocked => IsValveLocked || IsGuardianLocked;
    public string Prompt => triggered ? string.Empty : IsValveLocked ? lockedPrompt : IsGuardianLocked ? guardianLockedPrompt : prompt;

    private bool IsValveLocked => requiredValve != null && !requiredValve.IsComplete;
    private bool IsGuardianLocked => requiredGuardian != null && !requiredGuardian.IsComplete;

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

        if (IsLocked)
        {
            ShowLockedFeedback();
            return;
        }

        triggered = true;
        GameStateController.Instance?.PlayerWon();
    }

    private void ShowLockedFeedback()
    {
        if (Time.unscaledTime - lastLockedFeedbackTime < 1f)
        {
            return;
        }

        lastLockedFeedbackTime = Time.unscaledTime;
        string message = IsGuardianLocked ? guardianLockedMessage : lockedMessage;
        HUDController.Instance?.ShowTemporaryMessage(message, 1.4f);
        SteamworksAudio.Play(SteamworksAudioCue.GateDenied);
    }
}
