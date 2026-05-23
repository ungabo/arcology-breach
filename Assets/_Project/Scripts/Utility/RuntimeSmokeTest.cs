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
        Require<PauseMenuController>("PauseMenuController");
        Require<SteamworksAudio>("SteamworksAudio");
        Require<RuntimePerformanceProfile>("RuntimePerformanceProfile");
        Require<RuntimeAutoPlaythroughTest>("RuntimeAutoPlaythroughTest");
        Require<RuntimeCombatTest>("RuntimeCombatTest");
        Require<RuntimeInteractionTest>("RuntimeInteractionTest");
        Require<RuntimeCombatScenarioTest>("RuntimeCombatScenarioTest");
        Require<RuntimeBulwarkCombatTest>("RuntimeBulwarkCombatTest");
        Require<RuntimeWardenCombatTest>("RuntimeWardenCombatTest");
        Require<RuntimeHazardTest>("RuntimeHazardTest");
        Require<RuntimeSecretTest>("RuntimeSecretTest");
        Require<RuntimePauseFlowTest>("RuntimePauseFlowTest");
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
