using System;
using System.Collections;
using UnityEngine;

public class RuntimeWorldLabelReadabilityTest : MonoBehaviour
{
    private const string WorldLabelArgument = "-v0WorldLabelReadabilitySmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(WorldLabelArgument))
        {
            yield break;
        }

        yield return null;

        Camera camera = Require<Camera>("Camera");
        WorldLabelBillboard[] labels = UnityEngine.Object.FindObjectsByType<WorldLabelBillboard>(FindObjectsSortMode.None);
        RequireState(labels.Length >= 5, "Level01 world label count");

        WorldLabelBillboard gearKeyLabel = RequireNamedLabel(labels, "Label - Gear Key");
        WorldLabelBillboard pressureGateLabel = RequireNamedLabel(labels, "Label - Pressure Gate");
        RequireState(gearKeyLabel.V0137ReadabilityReady, "gear key label readiness metadata");
        RequireState(pressureGateLabel.V0137ReadabilityReady, "pressure gate label readiness metadata");

        GameSettings.Load();
        bool priorHighContrast = GameSettings.HighContrast;

        GameSettings.SetHighContrast(false);
        ApplyLabels(labels);
        yield return null;

        float normalSize = gearKeyLabel.textMesh.characterSize;
        RequireState(!gearKeyLabel.CurrentHighContrastApplied, "normal label style applied");
        RequireState(gearKeyLabel.backplateRenderer != null && gearKeyLabel.backplateRenderer.enabled, "normal label backplate enabled");

        GameSettings.SetHighContrast(true);
        ApplyLabels(labels);
        yield return null;

        RequireState(gearKeyLabel.CurrentHighContrastApplied, "high contrast label style applied");
        RequireState(gearKeyLabel.textMesh.fontStyle == FontStyle.Bold, "high contrast label bold");
        RequireState(gearKeyLabel.textMesh.characterSize > normalSize, "high contrast label size increase");
        RequireState(IsNearColor(gearKeyLabel.textMesh.color, Color.white, 0.04f), "high contrast label text color");

        gearKeyLabel.ForceFaceCameraForTest(camera);
        RequireState(gearKeyLabel.LastCameraDistance > 0.5f, "label billboard camera distance");
        RequireState(Quaternion.Angle(gearKeyLabel.transform.rotation, Quaternion.identity) > 1f, "label billboard rotation updated");

        GameSettings.SetHighContrast(priorHighContrast);
        ApplyLabels(labels);
        yield return null;

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_WORLD_LABEL_READABILITY_PASS");
        Application.Quit(0);
    }

    private static void ApplyLabels(WorldLabelBillboard[] labels)
    {
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i].ApplyReadabilityForTest();
        }
    }

    private WorldLabelBillboard RequireNamedLabel(WorldLabelBillboard[] labels, string name)
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i].gameObject.name == name)
            {
                return labels[i];
            }
        }

        Fail("World label readability smoke failed: missing " + name + ".");
        return null;
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("World label readability smoke failed: missing " + label + ".");
        }

        return value;
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("World label readability smoke failed: " + label + ".");
        }
    }

    private void Fail(string message)
    {
        failed = true;
        Debug.LogError(message);
        Application.Quit(1);
    }

    private static bool IsNearColor(Color actual, Color expected, float tolerance)
    {
        return Mathf.Abs(actual.r - expected.r) <= tolerance
            && Mathf.Abs(actual.g - expected.g) <= tolerance
            && Mathf.Abs(actual.b - expected.b) <= tolerance
            && Mathf.Abs(actual.a - expected.a) <= tolerance;
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
