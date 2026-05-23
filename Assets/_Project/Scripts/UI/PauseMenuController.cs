using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject root;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    public bool IsVisible => root != null && root.activeSelf;

    private void Awake()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartRun);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitRun);
        }

        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        if (root != null)
        {
            root.SetActive(visible);
        }
    }

    public void ResumeGame()
    {
        GameStateController.Instance?.ResumeGameplay();
    }

    public void RestartRun()
    {
        GameStateController.Instance?.RestartLevel();
    }

    public void QuitRun()
    {
        GameStateController.Instance?.QuitGame();
    }
}
