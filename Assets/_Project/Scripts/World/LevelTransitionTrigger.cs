using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionTrigger : MonoBehaviour
{
    public string targetSceneName = "Level02";
    public string transitionMessage = "Service lift engaged";

    private bool loading;

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
        if (loading || other.GetComponentInParent<PlayerController>() == null)
        {
            return;
        }

        loading = true;
        Time.timeScale = 1f;
        HUDController.Instance?.ShowTemporaryMessage(transitionMessage, 0.5f);
        SteamworksAudio.Play(SteamworksAudioCue.Win);
        SceneManager.LoadScene(targetSceneName);
    }
}
