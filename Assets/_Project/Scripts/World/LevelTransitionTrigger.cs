using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionTrigger : MonoBehaviour, IInteractable
{
    public string targetSceneName = "Level02";
    public string transitionMessage = "Service lift engaged";
    public string prompt = "E - engage service lift";
    public string lockedPrompt = "vent boiler pressure first";
    public string lockedMessage = "The service lift is pressure-locked. Vent the Boilerheart first.";
    public SteamValveObjective requiredValve;

    private bool loading;
    private float lastLockedFeedbackTime = -10f;

    public bool IsLocked => requiredValve != null && !requiredValve.IsComplete;
    public string Prompt => loading ? string.Empty : IsLocked ? lockedPrompt : prompt;

    private void Awake()
    {
        Collider triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (loading || player == null)
        {
            return;
        }

        BeginTransition(player);
    }

    public bool CanInteract(GameObject interactor)
    {
        return !loading && interactor.GetComponentInParent<PlayerController>() != null;
    }

    public void Interact(GameObject interactor)
    {
        PlayerController player = interactor.GetComponentInParent<PlayerController>();
        if (player == null)
        {
            return;
        }

        BeginTransition(player);
    }

    private void BeginTransition(PlayerController player)
    {
        if (loading)
        {
            return;
        }

        if (IsLocked)
        {
            ShowLockedFeedback();
            return;
        }

        loading = true;
        LiftActivationVfx.Spawn(transform.position + Vector3.up * 0.25f - transform.forward * 0.45f, transform.rotation);
        LevelTransitionController controller = LevelTransitionController.Instance != null ? LevelTransitionController.Instance : Object.FindAnyObjectByType<LevelTransitionController>();
        if (controller != null)
        {
            controller.LoadSceneFromPlayer(targetSceneName, player, transitionMessage);
            return;
        }

        RunProgress.Capture(player.GetComponent<PlayerHealth>(), player.GetComponent<PlayerInventory>());
        Time.timeScale = 1f;
        HUDController.Instance?.ShowTemporaryMessage(transitionMessage, 0.5f);
        SteamworksAudio.Play(SteamworksAudioCue.Win);
        SceneManager.LoadScene(targetSceneName);
    }

    private void ShowLockedFeedback()
    {
        if (Time.unscaledTime - lastLockedFeedbackTime < 1f)
        {
            return;
        }

        lastLockedFeedbackTime = Time.unscaledTime;
        HUDController.Instance?.ShowTemporaryMessage(lockedMessage, 1.4f);
        SteamworksAudio.Play(SteamworksAudioCue.GateDenied);
        GameplayFeedbackController.ReportWorld(GameplayFeedbackEventType.RouteBlocked, targetSceneName + "_lift_locked", transform.position + Vector3.up * 0.75f, new Color(0.95f, 0.12f, 0.04f));
    }
}
