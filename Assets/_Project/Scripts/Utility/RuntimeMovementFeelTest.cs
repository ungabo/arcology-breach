using System;
using System.Collections;
using UnityEngine;

public class RuntimeMovementFeelTest : MonoBehaviour
{
    private const string MovementArgument = "-v0MovementSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(MovementArgument))
        {
            yield break;
        }

        yield return null;

        PlayerController player = Require<PlayerController>("PlayerController");
        RequireState(Mathf.Approximately(player.moveSpeed, GameBalance.PlayerMoveSpeed), "player top speed balance");
        RequireState(Mathf.Approximately(player.moveAcceleration, GameBalance.PlayerMoveAcceleration), "player acceleration balance");
        RequireState(Mathf.Approximately(player.moveDeceleration, GameBalance.PlayerMoveDeceleration), "player deceleration balance");
        RequireState(Mathf.Approximately(player.gravity, GameBalance.PlayerGravity), "player gravity balance");
        RequireState(Mathf.Approximately(player.groundStickVelocity, GameBalance.PlayerGroundStickVelocity), "player ground stick balance");
        RequireState(Mathf.Approximately(player.pitchLimit, GameBalance.PlayerPitchLimit), "player pitch limit balance");
        RequireState(player.moveAcceleration > player.moveSpeed, "acceleration can reach top speed quickly");
        RequireState(player.moveDeceleration >= player.moveAcceleration, "deceleration is at least as responsive as acceleration");

        float priorSensitivity = GameSettings.MouseSensitivity;
        GameSettings.SetMouseSensitivity(99f);
        RequireState(Mathf.Approximately(GameSettings.MouseSensitivity, 5f), "mouse sensitivity upper clamp");
        GameSettings.SetMouseSensitivity(0.01f);
        RequireState(Mathf.Approximately(GameSettings.MouseSensitivity, 0.6f), "mouse sensitivity lower clamp");
        GameSettings.SetMouseSensitivity(priorSensitivity);

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_MOVEMENT_FEEL_PASS");
        Application.Quit(0);
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Movement feel smoke failed: " + label + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Movement feel smoke failed: missing " + label + ".");
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
