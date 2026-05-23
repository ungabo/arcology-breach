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
        Require<PauseMenuController>("PauseMenuController");
        Require<SteamworksAudio>("SteamworksAudio");
        Require<RuntimePerformanceProfile>("RuntimePerformanceProfile");
        Require<RuntimeAutoPlaythroughTest>("RuntimeAutoPlaythroughTest");
        Require<RuntimeCombatTest>("RuntimeCombatTest");
        Require<RuntimeInteractionTest>("RuntimeInteractionTest");
        Require<RuntimePauseFlowTest>("RuntimePauseFlowTest");
        Require<HUDController>("HUDController");
        Require<EnemyController>("EnemyController");
        Require<Pickup>("Pickup");
        Require<LockedDoor>("LockedDoor");
        Require<LevelTransitionTrigger>("LevelTransitionTrigger");

        if (!RuntimePerformanceProfile.Applied || Application.targetFrameRate != RuntimePerformanceProfile.WindowsTargetFrameRate)
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

    private static void Require<T>(string label) where T : UnityEngine.Object
    {
        if (UnityEngine.Object.FindAnyObjectByType<T>() == null)
        {
            Debug.LogError("Runtime smoke test failed: missing " + label);
            Application.Quit(1);
        }
    }
}
