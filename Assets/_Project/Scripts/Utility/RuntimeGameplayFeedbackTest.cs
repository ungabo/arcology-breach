using System;
using System.Collections;
using UnityEngine;

public class RuntimeGameplayFeedbackTest : MonoBehaviour
{
    private const string FeedbackArgument = "-v0GameplayFeedbackSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(FeedbackArgument))
        {
            yield break;
        }

        yield return null;

        GameplayFeedbackController feedback = Require<GameplayFeedbackController>("GameplayFeedbackController");
        GameStateController gameState = Require<GameStateController>("GameStateController");
        PauseMenuController pauseMenu = Require<PauseMenuController>("PauseMenuController");

        feedback.ResetForTest();
        VerifyTaxonomy(feedback);
        feedback.ResetForTest();
        VerifyWorldPulse(feedback);
        VerifyHookedRuntimeEvents(feedback, gameState, pauseMenu);

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_GAMEPLAY_FEEDBACK_PASS");
        Application.Quit(0);
    }

    private void VerifyTaxonomy(GameplayFeedbackController feedback)
    {
        foreach (GameplayFeedbackEventType eventType in Enum.GetValues(typeof(GameplayFeedbackEventType)))
        {
            GameplayFeedbackController.Report(eventType, "taxonomy_" + eventType);
        }

        RequireState(feedback.HasV0135Coverage, "v0.1.35 feedback coverage metadata is present");
        RequireState(feedback.ReportedEventTypeCount == feedback.SupportedEventTypeCount, "every feedback event type can be reported");
        RequireState(feedback.TotalEvents == feedback.SupportedEventTypeCount, "taxonomy event count matches supported count");
    }

    private void VerifyWorldPulse(GameplayFeedbackController feedback)
    {
        int previousPulses = feedback.VisualPulseCount;
        GameplayFeedbackController.ReportWorld(GameplayFeedbackEventType.WeaponImpact, "smoke_pressure_impact", transform.position + Vector3.up, new Color(1f, 0.58f, 0.1f));
        RequireState(feedback.VisualPulseCount > previousPulses, "world feedback pulse spawned");
        RequireState(UnityEngine.Object.FindObjectsByType<GameplayFeedbackPulseVfx>(FindObjectsSortMode.None).Length > 0, "feedback pulse object exists");
        RequireState(feedback.LastHadWorldPosition && feedback.LastEventType == GameplayFeedbackEventType.WeaponImpact, "last world feedback metadata captured");
    }

    private void VerifyHookedRuntimeEvents(GameplayFeedbackController feedback, GameStateController gameState, PauseMenuController pauseMenu)
    {
        float priorVolume = GameSettings.MasterVolume;

        gameState.SetObjective("Feedback smoke objective");
        RequireState(feedback.GetEventCount(GameplayFeedbackEventType.ObjectiveUpdated) > 0, "objective update hook reported");

        gameState.Pause();
        RequireState(feedback.GetEventCount(GameplayFeedbackEventType.PauseOpened) > 0, "pause opened hook reported");
        RequireState(pauseMenu.IsVisible, "pause menu visible during feedback smoke");

        float testVolume = priorVolume > 0.5f ? 0.42f : 0.72f;
        pauseMenu.SetMasterVolume(testVolume);
        RequireState(feedback.GetEventCount(GameplayFeedbackEventType.SettingChanged) > 0, "settings changed hook reported");

        gameState.ResumeGameplay();
        RequireState(feedback.GetEventCount(GameplayFeedbackEventType.PauseClosed) > 0, "pause closed hook reported");

        GameSettings.SetMasterVolume(priorVolume);
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Gameplay feedback smoke failed: missing " + label + ".");
        }

        return value;
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Gameplay feedback smoke failed: " + label + ".");
        }
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
