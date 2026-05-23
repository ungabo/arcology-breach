using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionController : MonoBehaviour
{
    public static LevelTransitionController Instance { get; private set; }

    public float transitionDelay = 0.45f;

    public bool IsTransitioning { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void LoadSceneFromPlayer(string targetSceneName, PlayerController player, string transitionMessage, bool captureRunProgress = true)
    {
        if (IsTransitioning)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(targetSceneName))
        {
            Debug.LogError("Level transition failed: target scene name is empty.");
            return;
        }

        IsTransitioning = true;
        if (captureRunProgress && player != null)
        {
            RunProgress.Capture(player.GetComponent<PlayerHealth>(), player.GetComponent<PlayerInventory>());
        }

        Time.timeScale = 1f;
        if (!string.IsNullOrWhiteSpace(transitionMessage))
        {
            HUDController.Instance?.ShowTemporaryMessage(transitionMessage, 0.5f);
        }

        SteamworksAudio.Play(SteamworksAudioCue.Win);
        StartCoroutine(LoadSceneAfterCue(targetSceneName));
    }

    private IEnumerator LoadSceneAfterCue(string targetSceneName)
    {
        float delay = Mathf.Max(0f, transitionDelay);
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        SceneManager.LoadScene(targetSceneName);
    }

    public void RestartCurrentScene()
    {
        if (IsTransitioning)
        {
            return;
        }

        IsTransitioning = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
