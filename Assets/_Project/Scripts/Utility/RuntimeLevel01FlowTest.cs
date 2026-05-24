using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeLevel01FlowTest : MonoBehaviour
{
    private const string FlowArgument = "-v0Level01FlowSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(FlowArgument))
        {
            yield break;
        }

        yield return null;

        if (!string.Equals(SceneManager.GetActiveScene().name, "Level01", StringComparison.OrdinalIgnoreCase))
        {
            Fail("Level01 flow smoke failed: expected Level01 but loaded " + SceneManager.GetActiveScene().name + ".");
            yield break;
        }

        RequireNamed("Level01 Flow Polish V015");
        RequireNamed("Level01 Gate Preview Brass Sightline Rail Left");
        RequireNamed("Level01 Gate Preview Brass Sightline Rail Right");
        RequireNamed("Level01 Gate Preview Red Locking Header");
        RequireNamed("Level01 Gate Preview Pressure Gauge");
        RequireNamed("Level01 Key Branch Return Brass Pipe A");
        RequireNamed("Level01 Key Branch Return Brass Pipe B");
        RequireNamed("Level01 Key Branch Return Chevron A");
        RequireNamed("Level01 Key Branch Return Chevron B");
        RequireNamed("Level01 Service Lift Green Runway Center");
        RequireNamed("Level01 Service Lift Green Chevron A");
        RequireNamed("Level01 Service Lift Green Beacon Light");
        RequireNamed("Level01 Secret Warm Pipe Clue");
        RequireNamed("Level01 Secret Loose Service Valve");
        RequireNamed("Level01 Secret Misaligned Service Plate");

        RequireFlowOrdering();

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_LEVEL01_FLOW_PASS");
        Application.Quit(0);
    }

    private void RequireFlowOrdering()
    {
        GameObject gate = RequireNamed("Pressure Gate");
        GameObject key = RequireNamed("Pickup - Gear Key");
        GameObject lift = RequireNamed("Service Lift To Pipeworks");
        GameObject gatePreview = RequireNamed("Level01 Gate Preview Brass Sightline Rail Left");
        GameObject keyReturn = RequireNamed("Level01 Key Branch Return Brass Pipe A");
        GameObject liftRunway = RequireNamed("Level01 Service Lift Green Runway Center");
        GameObject secret = RequireNamed("Secret - Intake Pressure Cache");
        GameObject secretClue = RequireNamed("Level01 Secret Warm Pipe Clue");

        RequireState(key.transform.position.x > 10f, "gear key remains on the visible side branch");
        RequireState(gate.transform.position.z > gatePreview.transform.position.z, "pressure gate sits beyond its preview cue");
        RequireState(keyReturn.transform.position.x > gate.transform.position.x && keyReturn.transform.position.x < key.transform.position.x, "key return cue sits between key and gate");
        RequireState(lift.transform.position.z > gate.transform.position.z, "service lift is after the pressure gate");
        RequireState(liftRunway.transform.position.z > gate.transform.position.z && liftRunway.transform.position.z < lift.transform.position.z, "service lift runway bridges final room to lift");
        RequireDistance(secretClue.transform.position, secret.transform.position, 0.85f, 2.2f, "secret clue distance from cache");
    }

    private GameObject RequireNamed(string objectName)
    {
        GameObject found = GameObject.Find(objectName);
        if (found == null)
        {
            Fail("Level01 flow smoke failed: missing " + objectName + ".");
            return new GameObject("Missing " + objectName);
        }

        return found;
    }

    private void RequireDistance(Vector3 a, Vector3 b, float minimum, float maximum, string label)
    {
        float distance = Vector3.Distance(a, b);
        if (distance < minimum || distance > maximum)
        {
            Fail("Level01 flow smoke failed: " + label + " expected " + minimum.ToString("0.00") + "-" + maximum.ToString("0.00") + "m but found " + distance.ToString("0.00") + "m.");
        }
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Level01 flow smoke failed: " + label + ".");
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
