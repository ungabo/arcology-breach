using System;
using System.Collections;
using UnityEngine;

public class RuntimeInteractionTest : MonoBehaviour
{
    private const string InteractionArgument = "-v0InteractionSmoke";

    private IEnumerator Start()
    {
        if (!HasArgument(InteractionArgument))
        {
            yield break;
        }

        yield return null;

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerInteraction interaction = Require<PlayerInteraction>("PlayerInteraction");
        HUDController hud = Require<HUDController>("HUDController");
        LockedDoor door = Require<LockedDoor>("LockedDoor");
        LevelTransitionTrigger transition = Require<LevelTransitionTrigger>("LevelTransitionTrigger");

        if (interaction.viewTransform == null || interaction.interactionRange <= 0f || interaction.interactKey != KeyCode.E)
        {
            Fail("PlayerInteraction is not configured for keyboard use.");
        }

        if (hud.interactionText == null)
        {
            Fail("HUDController is missing interaction prompt text.");
        }

        RequireInteractable(door, player.gameObject, "LockedDoor");
        RequireInteractable(transition, player.gameObject, "LevelTransitionTrigger");

        Debug.Log("V0_INTERACTION_SMOKE_PASS");
        Application.Quit(0);
    }

    private static void RequireInteractable(IInteractable interactable, GameObject player, string label)
    {
        if (interactable == null)
        {
            Fail(label + " does not implement IInteractable.");
        }

        if (!interactable.CanInteract(player))
        {
            Fail(label + " is not interactable for the player.");
        }

        if (string.IsNullOrWhiteSpace(interactable.Prompt))
        {
            Fail(label + " has no interaction prompt.");
        }
    }

    private static bool HasArgument(string target)
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], target, StringComparison.OrdinalIgnoreCase))
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
            Fail("Missing " + label + ".");
        }

        return value;
    }

    private static void Fail(string message)
    {
        Debug.LogError("Runtime interaction smoke failed: " + message);
        Application.Quit(1);
    }
}
