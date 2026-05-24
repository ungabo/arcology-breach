using System;
using System.Collections;
using UnityEngine;

public class RuntimeSmokeTest : MonoBehaviour
{
    private const string SmokeArgument = "-v0RuntimeSmoke";

    private IEnumerator Start()
    {
        if (!HasSmokeArgument())
        {
            yield break;
        }

        yield return null;

        Require<PlayerController>("PlayerController");
        Require<PlayerHealth>("PlayerHealth");
        Require<PlayerInventory>("PlayerInventory");
        Require<PlayerInteraction>("PlayerInteraction");
        Require<WeaponController>("WeaponController");
        Require<GameStateController>("GameStateController");
        Require<LevelTransitionController>("LevelTransitionController");
        PauseMenuController pauseMenu = Require<PauseMenuController>("PauseMenuController");
        if (pauseMenu.flashSlider == null || pauseMenu.flashValueText == null)
        {
            Debug.LogError("Runtime smoke test failed: flash intensity controls are not wired.");
            Application.Quit(1);
        }

        GameSettings.Load();
        if (GameSettings.FlashIntensity < GameSettings.MinFlashIntensity || GameSettings.FlashIntensity > GameSettings.MaxFlashIntensity)
        {
            Debug.LogError("Runtime smoke test failed: flash intensity setting is outside the supported range.");
            Application.Quit(1);
        }

        SteamworksAudio audio = Require<SteamworksAudio>("SteamworksAudio");
        if (!audio.AmbienceActive || audio.AmbienceSampleCount <= 0)
        {
            Debug.LogError("Runtime smoke test failed: brassworks ambience is not active.");
            Application.Quit(1);
        }

        if (!audio.UsingAuthoredAmbience || !AllAudioV1CuesActive(audio))
        {
            Debug.LogError("Runtime smoke test failed: expected AudioV1 ambience and cue bindings are not active.");
            Application.Quit(1);
        }

        Require<RuntimePerformanceProfile>("RuntimePerformanceProfile");
        Require<RuntimeAutoPlaythroughTest>("RuntimeAutoPlaythroughTest");
        Require<RuntimeCombatTest>("RuntimeCombatTest");
        Require<RuntimeInteractionTest>("RuntimeInteractionTest");
        Require<RuntimeCombatScenarioTest>("RuntimeCombatScenarioTest");
        Require<RuntimeBellowsNodeTest>("RuntimeBellowsNodeTest");
        Require<RuntimeBulwarkCombatTest>("RuntimeBulwarkCombatTest");
        Require<RuntimeWardenCombatTest>("RuntimeWardenCombatTest");
        Require<RuntimeHazardTest>("RuntimeHazardTest");
        Require<RuntimeSecretTest>("RuntimeSecretTest");
        Require<RuntimeWeaponSwitchTest>("RuntimeWeaponSwitchTest");
        Require<RuntimePauseFlowTest>("RuntimePauseFlowTest");
        Require<RuntimeMovementFeelTest>("RuntimeMovementFeelTest");
        Require<RuntimeBalanceEnvelopeTest>("RuntimeBalanceEnvelopeTest");
        Require<RuntimeLevel01FlowTest>("RuntimeLevel01FlowTest");
        Require<RuntimeMidgameFlowTest>("RuntimeMidgameFlowTest");
        Require<RuntimeClimaxFlowTest>("RuntimeClimaxFlowTest");
        Require<RuntimeAudioMixTest>("RuntimeAudioMixTest");
        Require<SteamworksSpinner>("SteamworksSpinner");
        MachineMotionVfx machineMotion = Require<MachineMotionVfx>("MachineMotionVfx");
        if (!machineMotion.IsConfigured)
        {
            Debug.LogError("Runtime smoke test failed: machine motion VFX is not configured.");
            Application.Quit(1);
        }

        HUDController hud = Require<HUDController>("HUDController");
        if (hud.bossNameText == null || hud.bossBackplateImage == null || hud.bossFillImage == null)
        {
            Debug.LogError("Runtime smoke test failed: missing boss health HUD wiring.");
            Application.Quit(1);
        }

        if (hud.objectiveText == null || hud.objectiveBackplateImage == null || string.IsNullOrWhiteSpace(hud.CurrentObjective))
        {
            Debug.LogError("Runtime smoke test failed: missing active objective HUD wiring.");
            Application.Quit(1);
        }
        Require<EnemyController>("EnemyController");
        Require<Pickup>("Pickup");
        Require<LockedDoor>("LockedDoor");
        Require<LevelTransitionTrigger>("LevelTransitionTrigger");

        if (!RuntimePerformanceProfile.Applied || RuntimePerformanceProfile.AppliedProfile == null || RuntimePerformanceProfile.AppliedTarget != PlatformQualityTarget.WindowsMidLow || Application.targetFrameRate != RuntimePerformanceProfile.WindowsTargetFrameRate)
        {
            Debug.LogError("Runtime smoke test failed: performance profile was not applied.");
            Application.Quit(1);
        }

        Debug.Log("V0_RUNTIME_SMOKE_PASS");
        Application.Quit(0);
    }

    private static bool HasSmokeArgument()
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], SmokeArgument, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static bool AllAudioV1CuesActive(SteamworksAudio audio)
    {
        foreach (SteamworksAudioCue cue in Enum.GetValues(typeof(SteamworksAudioCue)))
        {
            if (!audio.HasClip(cue) || !audio.IsUsingAuthoredClip(cue) || audio.GetClipSampleCount(cue) <= 0)
            {
                return false;
            }
        }

        return true;
    }

    private static T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Debug.LogError("Runtime smoke test failed: missing " + label);
            Application.Quit(1);
        }

        return value;
    }
}
