using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionTrigger : MonoBehaviour, IInteractable
{
    public string targetSceneName = "Level02";
    public string transitionMessage = "Service lift engaged";
    public string prompt = "E - engage service lift";

    private bool loading;

    public string Prompt => loading ? string.Empty : prompt;

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

        loading = true;
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
}
