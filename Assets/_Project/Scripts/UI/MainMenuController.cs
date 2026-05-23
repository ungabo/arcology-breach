using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public string gameplaySceneName = "Level01";
    public Button startButton;
    public Button quitButton;

    private static readonly string[] AutomationArguments =
    {
        "-v0RuntimeSmoke",
        "-v0AutoPlaythrough",
        "-v0CombatSmoke",
        "-v0PauseFlow"
    };

    private void Awake()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (HasAutomationArgument())
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private static bool HasAutomationArgument()
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            for (int j = 0; j < AutomationArguments.Length; j++)
            {
                if (string.Equals(args[i], AutomationArguments[j], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
