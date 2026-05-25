using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeMidgameFlowTest : MonoBehaviour
{
    private const string MidgameArgument = "-v0MidgameFlowSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(MidgameArgument))
        {
            yield break;
        }

        yield return null;

        string sceneName = SceneManager.GetActiveScene().name;
        if (string.Equals(sceneName, "Level02", StringComparison.OrdinalIgnoreCase))
        {
            VerifyPipeworks();
            if (failed)
            {
                yield break;
            }

            SceneManager.LoadScene("Level03");
            yield break;
        }

        if (string.Equals(sceneName, "Level03", StringComparison.OrdinalIgnoreCase))
        {
            VerifyBoilerheart();
            if (failed)
            {
                yield break;
            }

            Debug.Log("V0_MIDGAME_FLOW_PASS");
            Application.Quit(0);
            yield break;
        }

        Fail("Midgame flow smoke failed: expected Level02 or Level03 but loaded " + sceneName + ".");
    }

    private void VerifyPipeworks()
    {
        GameObject lift = RequireNamed("Pipeworks Service Lift To Boilerheart");
        GameObject valve = RequireNamed("Pipeworks Routing Valve Objective");
        GameObject lancer = RequireNamed("Enemy - Pipeworks Lancer");
        GameObject secret = RequireNamed("Secret - Pipeworks Cartridge Cache");
        GameObject stopBar = RequireNamed("Level02 Pipeworks Locked Boilerheart Lift Stop Bar");
        GameObject valveLead = RequireNamed("Level02 Routing Valve Floor Lead");
        GameObject cover = RequireNamed("Level02 Lancer Sightline Brass Cover West");
        GameObject secretClue = RequireNamed("Level02 Secret Cold Pipe Clue");
        GameObject bypassManualBleed = RequireNamed("Label - L02 Manual Bleed");
        GameObject bypassRejoin = RequireNamed("Label - L02 Mainline Rejoin");
        GameObject bypassRejoinGauge = RequireNamed("L02 Pressure Bypass Rejoin Green Gauge");
        GameObject bypassTeachPocket = RequireNamed("L02 Pressure Bypass Teach Vent Safe Pocket");
        GameObject bypassPumpPocket = RequireNamed("L02 Pressure Bypass Pump Vent Safe Pocket");

        RequireNamed("Level02 Pipeworks Flow Polish V016");
        RequireNamed("Level02 Pipeworks Condensate Spine Center");
        RequireNamed("Level02 Pipeworks Lift Lock Gauge");
        RequireNamed("Level02 Routing Valve Amber Gaslight");
        RequireNamed("Level02 Lancer Sightline Warning Rail");
        RequireNamed("L02 Pump Vent Preview Warning Gauge");
        RequireNamed("L02 Pressure Bypass Steam Idle Tell");
        RequireNamed("L02 Pressure Bypass Redline Around Pump Vent");
        RequireNamed("L02 Pressure Bypass Secret Bleed Wheel Clue");

        RequireState(lift.transform.position.z > valve.transform.position.z, "Boilerheart lift remains north of routing valve");
        RequireState(stopBar.transform.position.z < lift.transform.position.z, "locked-lift stop bar is before lift trigger");
        RequireState(valveLead.transform.position.x < 0f && valveLead.transform.position.z < lift.transform.position.z, "routing valve lead points toward west valve branch");
        RequireState(lancer.transform.position.z > cover.transform.position.z, "Lancer sits beyond first cover break");
        RequireDistance(secretClue.transform.position, secret.transform.position, 0.55f, 1.8f, "Pipeworks secret clue distance");
        RequireState(bypassManualBleed.transform.position.z < bypassRejoin.transform.position.z, "pressure bypass manual bleed label precedes rejoin label");
        RequireDistance(bypassRejoin.transform.position, bypassRejoinGauge.transform.position, 0.1f, 3.2f, "pressure bypass rejoin gauge distance");
        RequireState(bypassTeachPocket.transform.position.z < bypassPumpPocket.transform.position.z, "pressure bypass teach pocket precedes pump pocket");
    }

    private void VerifyBoilerheart()
    {
        GameObject scattergun = RequireNamed("Pickup - Steam Scattergun");
        GameObject bellows = RequireNamed("Enemy - Boilerheart Bellows Node");
        GameObject valve = RequireNamed("Boilerheart Pressure Valve Objective");
        GameObject lift = RequireNamed("Boilerheart Service Lift To Foundry");
        GameObject trialLane = RequireNamed("Level03 Scattergun Trial Lane Strip");
        GameObject radiusMarker = RequireNamed("Level03 Bellows Pulse Radius Marker");
        GameObject returnStrip = RequireNamed("Level03 Valve To Lift Green Return Strip");
        GameObject stopBar = RequireNamed("Level03 Foundry Lift Locked Stop Bar");
        GameObject foundryFloor = RequireNamed("Label - L03 Foundry Floor");
        GameObject upperGantry = RequireNamed("Label - L03 Upper Gantry");
        GameObject controlWalkway = RequireNamed("Label - L03 Control Walkway");
        GameObject highRejoin = RequireNamed("Label - L03 High Rejoin");
        GameObject breathPocket = RequireNamed("L03 Gantry Breath Pocket Green Gauge");

        RequireNamed("Level03 Boilerheart Flow Polish V016");
        RequireNamed("Level03 Boilerheart Ring Brass Guide South");
        RequireNamed("Level03 Boilerheart Ring Brass Guide North");
        RequireNamed("Level03 Scattergun Display Pressure Gauge");
        RequireNamed("Level03 Bellows Boost Pipe To Scrapper Lane");
        RequireNamed("Level03 Bellows Amber Pulse Read Light");
        RequireNamed("Level03 Hazard Shutdown Sight Glass");
        RequireNamed("Level03 Valve Return Green Beacon");
        RequireNamed("Label - L03 Crane Return");
        RequireNamed("L03 Furnace West Preview Strip");
        RequireNamed("L03 Furnace East Safe Lane Brass Edge");
        RequireNamed("L03 Slag Vent Safe Preview Gauge");
        RequireNamed("L03 Narrow Span One Threat Limit Marker");
        RequireNamed("L03 Crucible Shelf Coolant Leak Clue");

        RequireState(scattergun.transform.position.z < bellows.transform.position.z, "Steam Scattergun pickup precedes Bellows Node teaching spike");
        RequireDistance(trialLane.transform.position, scattergun.transform.position, 0.5f, 2.0f, "scattergun trial lane distance");
        RequireDistance(radiusMarker.transform.position, bellows.transform.position, 0.0f, 1.1f, "Bellows pulse marker center");
        RequireState(valve.transform.position.x > 4f, "Boilerheart pressure valve remains on east branch");
        RequireState(lift.transform.position.z > valve.transform.position.z, "foundry lift remains after pressure valve");
        RequireState(returnStrip.transform.position.z > valve.transform.position.z && returnStrip.transform.position.z < lift.transform.position.z, "green return strip bridges valve route to lift");
        RequireState(stopBar.transform.position.z < lift.transform.position.z, "foundry lift stop bar is before lift trigger");
        RequireState(foundryFloor.transform.position.z < controlWalkway.transform.position.z, "foundry floor label precedes control walkway");
        RequireState(upperGantry.transform.position.z < highRejoin.transform.position.z, "upper gantry label precedes high rejoin");
        RequireState(breathPocket.transform.position.z > controlWalkway.transform.position.z, "gantry breath pocket appears after control walkway read");
    }

    private GameObject RequireNamed(string objectName)
    {
        GameObject found = GameObject.Find(objectName);
        if (found == null)
        {
            Fail("Midgame flow smoke failed: missing " + objectName + ".");
            return new GameObject("Missing " + objectName);
        }

        return found;
    }

    private void RequireDistance(Vector3 a, Vector3 b, float minimum, float maximum, string label)
    {
        float distance = Vector3.Distance(a, b);
        if (distance < minimum || distance > maximum)
        {
            Fail("Midgame flow smoke failed: " + label + " expected " + minimum.ToString("0.00") + "-" + maximum.ToString("0.00") + "m but found " + distance.ToString("0.00") + "m.");
        }
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Midgame flow smoke failed: " + label + ".");
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
