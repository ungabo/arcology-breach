using System;
using System.Collections;
using UnityEngine;

public class RuntimeDisplaySettingsTest : MonoBehaviour
{
    private const string DisplaySettingsArgument = "-v0DisplaySettingsSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(DisplaySettingsArgument))
        {
            yield break;
        }

        yield return null;

        GameStateController gameState = Require<GameStateController>("GameStateController");
        PauseMenuController pauseMenu = Require<PauseMenuController>("PauseMenuController");

        GameSettings.Load();
        int priorResolutionIndex = GameSettings.ResolutionIndex;
        bool priorFullscreen = GameSettings.Fullscreen;

        RequireState(GameSettings.ClampResolutionIndex(-99) == GameSettings.MinResolutionIndex, "resolution lower clamp");
        RequireState(GameSettings.ClampResolutionIndex(99) == GameSettings.MaxResolutionIndex, "resolution upper clamp");

        GameSettings.SetResolutionIndex(GameSettings.MinResolutionIndex);
        RequireState(GameSettings.ResolutionLabel == "1280x720", "low resolution label");
        GameSettings.SetResolutionIndex(GameSettings.MaxResolutionIndex);
        RequireState(GameSettings.ResolutionLabel == "1920x1080", "high resolution label");

        gameState.Pause();
        yield return null;
        RequireState(pauseMenu.IsVisible, "pause menu visible for display settings");
        RequireState(pauseMenu.resolutionButton != null && pauseMenu.fullscreenToggle != null && pauseMenu.resolutionValueText != null && pauseMenu.fullscreenValueText != null, "display controls wired");

        pauseMenu.CycleResolution();
        yield return null;
        RequireState(!string.IsNullOrWhiteSpace(pauseMenu.resolutionValueText.text), "resolution label populated");
        RequireState(pauseMenu.resolutionValueText.text == GameSettings.ResolutionLabel, "resolution label synced");

        pauseMenu.SetFullscreen(!priorFullscreen);
        yield return null;
        RequireState(GameSettings.Fullscreen != priorFullscreen, "fullscreen setting toggled");
        RequireState(pauseMenu.fullscreenValueText.text == (GameSettings.Fullscreen ? "ON" : "OFF"), "fullscreen label synced");

        GameSettings.SetResolutionIndex(priorResolutionIndex);
        GameSettings.SetFullscreen(priorFullscreen);
        pauseMenu.SetVisible(false);
        gameState.ResumeGameplay();
        yield return null;

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_DISPLAY_SETTINGS_PASS");
        Application.Quit(0);
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Display settings smoke failed: " + label + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Display settings smoke failed: missing " + label + ".");
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
