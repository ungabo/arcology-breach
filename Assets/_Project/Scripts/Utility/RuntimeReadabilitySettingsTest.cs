using System;
using System.Collections;
using UnityEngine;

public class RuntimeReadabilitySettingsTest : MonoBehaviour
{
    private const string ReadabilityArgument = "-v0ReadabilitySmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(ReadabilityArgument))
        {
            yield break;
        }

        yield return null;

        GameStateController gameState = Require<GameStateController>("GameStateController");
        PauseMenuController pauseMenu = Require<PauseMenuController>("PauseMenuController");
        HUDController hud = Require<HUDController>("HUDController");

        GameSettings.Load();
        bool priorHighContrast = GameSettings.HighContrast;

        GameSettings.SetHighContrast(false);
        yield return null;
        RequireState(!hud.HighContrastModeApplied, "high contrast initially off");

        gameState.Pause();
        yield return null;
        RequireState(pauseMenu.IsVisible, "pause menu visible for readability settings");
        RequireState(pauseMenu.highContrastToggle != null && pauseMenu.highContrastValueText != null, "contrast controls wired");

        pauseMenu.SetHighContrast(true);
        yield return null;
        RequireState(GameSettings.HighContrast, "contrast setting toggled on");
        RequireState(pauseMenu.highContrastValueText.text == "ON", "contrast label on");
        RequireState(hud.HighContrastModeApplied, "HUD contrast applied");
        RequireState(hud.objectiveText != null && hud.objectiveText.fontStyle == FontStyle.Bold, "objective text bold in contrast mode");
        RequireState(hud.interactionText != null && hud.interactionText.fontStyle == FontStyle.Bold, "interaction text bold in contrast mode");

        pauseMenu.SetHighContrast(false);
        yield return null;
        RequireState(!GameSettings.HighContrast, "contrast setting toggled off");
        RequireState(pauseMenu.highContrastValueText.text == "OFF", "contrast label off");
        RequireState(!hud.HighContrastModeApplied, "HUD contrast cleared");
        RequireState(hud.objectiveText != null && hud.objectiveText.fontStyle == FontStyle.Normal, "objective text normal after contrast mode");

        GameSettings.SetHighContrast(priorHighContrast);
        gameState.ResumeGameplay();
        yield return null;

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_READABILITY_SETTINGS_PASS");
        Application.Quit(0);
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Readability settings smoke failed: " + label + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Readability settings smoke failed: missing " + label + ".");
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
