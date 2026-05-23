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

    public GameRunState State { get; private set; } = GameRunState.Playing;
    public bool IsGameplayActive => State == GameRunState.Playing;

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
            hud = Object.FindFirstObjectByType<HUDController>();
        }
    }

    private void Start()
    {
        ResumeGameplay();
        hud?.ShowTemporaryMessage("Find the key. Open the red door.", 3f);
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
        hud?.ShowPersistentMessage("PAUSED\nEsc to resume");
    }

    public void ResumeGameplay()
    {
        State = GameRunState.Playing;
        Time.timeScale = 1f;
        SetCursorLocked(true);
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
        hud?.ShowPersistentMessage("EXIT REACHED\nPress R to play again");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private static void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
