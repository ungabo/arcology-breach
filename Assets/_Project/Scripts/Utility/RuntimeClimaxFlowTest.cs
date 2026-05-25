using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeClimaxFlowTest : MonoBehaviour
{
    private const string ClimaxArgument = "-v0ClimaxFlowSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(ClimaxArgument))
        {
            yield break;
        }

        yield return null;

        string sceneName = SceneManager.GetActiveScene().name;
        if (string.Equals(sceneName, "Level04", StringComparison.OrdinalIgnoreCase))
        {
            VerifyFoundry();
            if (failed)
            {
                yield break;
            }

            SceneManager.LoadScene("Level05");
            yield break;
        }

        if (string.Equals(sceneName, "Level05", StringComparison.OrdinalIgnoreCase))
        {
            VerifyGovernorCore();
            if (failed)
            {
                yield break;
            }

            Debug.Log("V0_CLIMAX_FLOW_PASS");
            Application.Quit(0);
            yield break;
        }

        Fail("Climax flow smoke failed: expected Level04 or Level05 but loaded " + sceneName + ".");
    }

    private void VerifyFoundry()
    {
        GameObject bulwark = RequireNamed("Enemy - Foundry Hammer Bulwark");
        GameObject hoist = RequireNamed("Foundry Emergency Hoist");
        GameObject coalCache = RequireNamed("Secret - Foundry Coal Cache");
        GameObject timingStrip = RequireNamed("Level04 Furnace Timing Preview Strip");
        GameObject bulwarkRing = RequireNamed("Level04 Bulwark Hammer Bay Floor Ring");
        GameObject hoistRunway = RequireNamed("Level04 Hoist Green Runway Strip");
        GameObject coalClue = RequireNamed("Level04 Coal Cache Footprint Clue");
        GameObject intakeLabel = RequireNamed("Label - L04 Intake Control");
        GameObject primerLabel = RequireNamed("Label - L04 Pump Primer");
        GameObject returnLabel = RequireNamed("Label - L04 Pressure Return");
        GameObject feedLabel = RequireNamed("Label - L04 Observatory Feed");
        GameObject rejoinLabel = RequireNamed("Label - L04 Pumpworks Rejoin");
        GameObject pumpRevealGauge = RequireNamed("L04 Pump State Reveal Gauge");
        GameObject northSafePocket = RequireNamed("L04 Pumpworks North Jet Safe Pocket");
        GameObject southSafePocket = RequireNamed("L04 Pumpworks South Jet Safe Pocket");

        RequireNamed("Level04 Foundry Climax Polish V017");
        RequireNamed("Level04 Heat Lane Warning Gauge");
        RequireNamed("Level04 Furnace Safe Edge Brass Rail");
        RequireNamed("Level04 Bulwark Retreat Cover Signal West");
        RequireNamed("Level04 Emergency Hoist Green Beacon");
        RequireNamed("L04 Arena Overpressure Warning Gauge");
        RequireNamed("L04 Pumpworks Arena Warning Floor Strip");
        RequireNamed("L04 Gear Sweep Telegraph Brass Tick");
        RequireNamed("L04 Observatory Return Duct Gauge Clue");
        RequireNamed("L04 Pumpworks Bulwark Release Buffer");

        RequireState(timingStrip.transform.position.z < bulwark.transform.position.z, "furnace timing read appears before Bulwark bay");
        RequireDistance(bulwarkRing.transform.position, bulwark.transform.position, 0f, 1.1f, "Bulwark bay ring center");
        RequireState(hoist.transform.position.z > bulwark.transform.position.z, "emergency hoist remains after Bulwark bay");
        RequireState(hoistRunway.transform.position.z > bulwark.transform.position.z && hoistRunway.transform.position.z < hoist.transform.position.z, "green runway bridges Bulwark bay to hoist");
        RequireDistance(coalClue.transform.position, coalCache.transform.position, 0.45f, 1.7f, "foundry coal cache clue distance");
        RequireState(intakeLabel.transform.position.z < primerLabel.transform.position.z, "pumpworks intake label precedes pump primer");
        RequireState(primerLabel.transform.position.z < returnLabel.transform.position.z, "pump primer label precedes pressure return");
        RequireState(returnLabel.transform.position.z < feedLabel.transform.position.z && feedLabel.transform.position.z < rejoinLabel.transform.position.z, "pump route state labels preserve route order");
        RequireDistance(returnLabel.transform.position, pumpRevealGauge.transform.position, 0.1f, 3.0f, "pump reveal gauge distance");
        RequireDistance(northSafePocket.transform.position, southSafePocket.transform.position, 1.0f, 4.0f, "pumpworks safe pocket separation");
    }

    private void VerifyGovernorCore()
    {
        GameObject warden = RequireNamed("Enemy - Governor Core Warden");
        GameObject hoist = RequireNamed("Governor Core Master Override Hoist");
        GameObject revealRail = RequireNamed("Level05 Warden Reveal Centerline Rail");
        GameObject arenaRing = RequireNamed("Level05 Warden Arena Boundary Ring");
        GameObject stopBar = RequireNamed("Level05 Warden Lock Warning Stop Bar");
        GameObject runway = RequireNamed("Level05 Master Override Green Runway");

        RequireNamed("Level05 Governor Climax Polish V017");
        RequireNamed("Level05 Boss Cover Pylon West");
        RequireNamed("Level05 Boss Cover Pylon East");
        RequireNamed("Level05 Warden Arena Pressure Gauge");
        RequireNamed("Level05 Master Override Green Beacon");

        RequireState(revealRail.transform.position.z < warden.transform.position.z, "Warden reveal rail approaches the boss");
        RequireDistance(arenaRing.transform.position, warden.transform.position, 0f, 1.5f, "Warden arena ring center");
        RequireState(hoist.transform.position.z > warden.transform.position.z, "master override hoist remains behind Warden");
        RequireState(stopBar.transform.position.z > warden.transform.position.z && stopBar.transform.position.z < hoist.transform.position.z, "Warden lock warning stop bar blocks hoist approach");
        RequireState(runway.transform.position.z > warden.transform.position.z && runway.transform.position.z < hoist.transform.position.z, "master override runway bridges Warden arena to hoist");
    }

    private GameObject RequireNamed(string objectName)
    {
        GameObject found = GameObject.Find(objectName);
        if (found == null)
        {
            Fail("Climax flow smoke failed: missing " + objectName + ".");
            return new GameObject("Missing " + objectName);
        }

        return found;
    }

    private void RequireDistance(Vector3 a, Vector3 b, float minimum, float maximum, string label)
    {
        float distance = Vector3.Distance(a, b);
        if (distance < minimum || distance > maximum)
        {
            Fail("Climax flow smoke failed: " + label + " expected " + minimum.ToString("0.00") + "-" + maximum.ToString("0.00") + "m but found " + distance.ToString("0.00") + "m.");
        }
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Climax flow smoke failed: " + label + ".");
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
