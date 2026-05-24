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
        LorePlaque plaque = Require<LorePlaque>("LorePlaque");

        if (interaction.viewTransform == null || interaction.interactionRange <= 0f || interaction.interactKey != KeyCode.E)
        {
            Fail("PlayerInteraction is not configured for keyboard use.");
        }

        if (hud.interactionText == null)
        {
            Fail("HUDController is missing interaction prompt text.");
        }

        if (hud.interactionBackplateImage == null || hud.interactionIconImage == null)
        {
            Fail("HUDController is missing UIHudV1 interaction prompt artwork.");
        }

        hud.SetInteractionPrompt("E - vent boiler pressure");
        if (!hud.interactionBackplateImage.enabled || !hud.interactionIconImage.enabled || hud.interactionIconImage.sprite != hud.promptValveSprite)
        {
            Fail("HUDController did not show the valve prompt icon.");
        }

        hud.SetInteractionPrompt("Gear key required");
        if (hud.interactionIconImage.sprite != hud.promptWarningSprite)
        {
            Fail("HUDController did not show the warning prompt icon.");
        }

        hud.ClearInteractionPrompt();
        if (hud.interactionBackplateImage.enabled || hud.interactionIconImage.enabled)
        {
            Fail("HUDController did not hide prompt artwork.");
        }

        RequireInteractable(door, player.gameObject, "LockedDoor");
        RequireInteractable(transition, player.gameObject, "LevelTransitionTrigger");
        RequireInteractable(plaque, player.gameObject, "LorePlaque");

        hud.SetKey(false);
        door.Interact(player.gameObject);
        if (hud.keyLampImage == null || hud.keyLampDeniedSprite == null || hud.keyLampImage.sprite != hud.keyLampDeniedSprite)
        {
            Fail("LockedDoor denial did not flash the denied key-lamp sprite.");
        }

        yield return null;
        plaque.Interact(player.gameObject);
        yield return null;

        if (!plaque.WasRead)
        {
            Fail("LorePlaque did not mark itself read after interaction.");
        }

        if (hud.messageText == null || !hud.messageText.enabled || !hud.messageText.text.Contains(plaque.title))
        {
            Fail("LorePlaque interaction did not show its archive text on the HUD.");
        }

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
