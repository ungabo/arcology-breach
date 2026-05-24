using System;
using System.Collections;
using UnityEngine;

public class RuntimePauseFlowTest : MonoBehaviour
{
    private const string PauseFlowArgument = "-v0PauseFlow";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(PauseFlowArgument))
        {
            yield break;
        }

        yield return null;

        GameStateController gameState = Require<GameStateController>("GameStateController");
        PauseMenuController pauseMenu = Require<PauseMenuController>("PauseMenuController");

        gameState.suppressQuitForAutomation = true;
        gameState.suppressRestartForAutomation = true;

        gameState.Pause();
        yield return null;
        RequireState(gameState.State == GameRunState.Paused, "pause state");
        RequireState(Mathf.Approximately(Time.timeScale, 0f), "paused time scale");
        RequireState(pauseMenu.IsVisible, "pause menu visible");
        RequireState(pauseMenu.flashSlider != null && pauseMenu.flashValueText != null && pauseMenu.resolutionButton != null && pauseMenu.fullscreenToggle != null, "settings controls wired");

        pauseMenu.flashSlider.SetValueWithoutNotify(0.85f);
        pauseMenu.flashSlider.value = 0.42f;
        yield return null;
        RequireState(Mathf.Abs(GameSettings.FlashIntensity - 0.42f) < 0.01f, "flash intensity setting");
        RequireState(pauseMenu.flashValueText.text == "42%", "flash intensity label");
        pauseMenu.flashSlider.value = GameSettings.DefaultFlashIntensity;

        pauseMenu.ResumeGame();
        yield return null;
        RequireState(gameState.State == GameRunState.Playing, "resume state");
        RequireState(Mathf.Approximately(Time.timeScale, 1f), "resumed time scale");
        RequireState(!pauseMenu.IsVisible, "pause menu hidden after resume");

        gameState.Pause();
        yield return null;
        pauseMenu.RestartRun();
        yield return null;
        RequireState(gameState.RestartRequestedForAutomation, "restart request");
        RequireState(Mathf.Approximately(Time.timeScale, 1f), "restart restores time scale");

        gameState.Pause();
        yield return null;
        pauseMenu.QuitRun();
        yield return null;
        RequireState(gameState.QuitRequestedForAutomation, "quit request");
        RequireState(Mathf.Approximately(Time.timeScale, 1f), "quit restores time scale");

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_PAUSE_FLOW_PASS");
        Application.Quit(0);
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Pause flow failed: " + label + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Pause flow failed: missing " + label + ".");
        }

        return value;
    }

    private void Fail(string message)
    {
        failed = true;
        Debug.LogError(message);
        Application.Quit(1);
    }

    private static bool HasArgument(string argument)
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], argument, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
