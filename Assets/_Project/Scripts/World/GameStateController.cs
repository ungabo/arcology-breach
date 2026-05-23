using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameRunState
{
    Playing,
    Paused,
    Dead,
    Won
}

public class GameStateController : MonoBehaviour
{
    public static GameStateController Instance { get; private set; }

    public HUDController hud;
    public PauseMenuController pauseMenu;
    public string startMessage = "Find the gear key. Open the pressure gate.";
    public bool suppressQuitForAutomation;
    public bool suppressRestartForAutomation;

    public GameRunState State { get; private set; } = GameRunState.Playing;
    public bool IsGameplayActive => State == GameRunState.Playing;
    public bool QuitRequestedForAutomation { get; private set; }
    public bool RestartRequestedForAutomation { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (hud == null)
        {
            hud = Object.FindAnyObjectByType<HUDController>();
        }

        if (pauseMenu == null)
        {
            pauseMenu = Object.FindAnyObjectByType<PauseMenuController>();
        }
    }

    private void Start()
    {
        ResumeGameplay();
        SetObjective(startMessage);
        if (!string.IsNullOrWhiteSpace(startMessage))
        {
            hud?.ShowTemporaryMessage(startMessage, 3f);
        }
    }

    public void SetObjective(string objective)
    {
        hud?.SetObjective(objective);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (State == GameRunState.Playing)
            {
                Pause();
            }
            else if (State == GameRunState.Paused)
            {
                ResumeGameplay();
            }
        }

        if ((State == GameRunState.Dead || State == GameRunState.Won) && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void Pause()
    {
        State = GameRunState.Paused;
        Time.timeScale = 0f;
        SetCursorLocked(false);
        hud?.ClearMessage();
        pauseMenu?.SetVisible(true);
    }

    public void ResumeGameplay()
    {
        State = GameRunState.Playing;
        Time.timeScale = 1f;
        SetCursorLocked(true);
        pauseMenu?.SetVisible(false);
        hud?.ClearMessage();
    }

    public void PlayerDied()
    {
        if (State == GameRunState.Dead)
        {
            return;
        }

        State = GameRunState.Dead;
        Time.timeScale = 1f;
        SetCursorLocked(false);
        pauseMenu?.SetVisible(false);
        SetObjective("Recover and try the run again.");
        hud?.ShowPersistentMessage("YOU DIED\nPress R to restart");
    }

    public void PlayerWon()
    {
        if (State == GameRunState.Won)
        {
            return;
        }

        State = GameRunState.Won;
        Time.timeScale = 1f;
        SetCursorLocked(false);
        pauseMenu?.SetVisible(false);
        SteamworksAudio.Play(SteamworksAudioCue.Win);
        SetObjective("Run complete.");
        string secretSummary = RunStats.TotalSecrets > 0 ? "\nSECRETS " + RunStats.DiscoveredSecrets + "/" + RunStats.TotalSecrets : string.Empty;
        hud?.ShowPersistentMessage("SERVICE LIFT REACHED" + secretSummary + "\nPress R to run again");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        pauseMenu?.SetVisible(false);
        RestartRequestedForAutomation = true;
        if (suppressRestartForAutomation)
        {
            return;
        }

        LevelTransitionController transitionController = LevelTransitionController.Instance != null ? LevelTransitionController.Instance : Object.FindAnyObjectByType<LevelTransitionController>();
        if (transitionController != null)
        {
            transitionController.RestartCurrentScene();
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        pauseMenu?.SetVisible(false);
        QuitRequestedForAutomation = true;
        if (!suppressQuitForAutomation)
        {
            Application.Quit();
        }
    }

    private static void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
