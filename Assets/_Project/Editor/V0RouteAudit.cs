using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class V0RouteAudit
{
    private static string OutputPath => "Documentation/QA/RouteAudit/ROUTE_AUDIT_" + GameBranding.BuildVersion + ".md";

    private static readonly string[] ScenePaths =
    {
        "Assets/_Project/Scenes/Level01.unity",
        "Assets/_Project/Scenes/Level02.unity",
        "Assets/_Project/Scenes/Level03.unity",
        "Assets/_Project/Scenes/Level04.unity",
        "Assets/_Project/Scenes/Level05.unity"
    };

    [MenuItem("Project Tools/Run v0 Route Audit")]
    public static void RunRouteAudit()
    {
        StringBuilder report = new StringBuilder();
        StringBuilder findings = new StringBuilder();
        int blockingIssues = 0;

        report.AppendLine("# Brassworks Breach - Route Audit " + GameBranding.BuildVersion);
        report.AppendLine();
        report.AppendLine("Build target: `" + GameBranding.BuildVersion + "`");
        report.AppendLine();
        report.AppendLine("Purpose: deterministic scene inspection for the current five-level Windows route. This supplements, but does not replace, a human feel/playability pass.");
        report.AppendLine();
        report.AppendLine("## Scene Route Matrix");
        report.AppendLine();
        report.AppendLine("| Scene | Core Route Objects | Enemies | Pickups | Hazards | Secrets | Transition / Exit | Notes |");
        report.AppendLine("| --- | --- | --- | --- | --- | --- | --- | --- |");

        for (int i = 0; i < ScenePaths.Length; i++)
        {
            EditorSceneManager.OpenScene(ScenePaths[i]);
            Scene scene = SceneManager.GetActiveScene();
            RouteSceneMetrics metrics = CollectMetrics(scene.name);
            blockingIssues += metrics.BlockingIssues;
            report.AppendLine(metrics.ToMarkdownRow());
            findings.Append(metrics.Findings);
        }

        report.AppendLine();
        report.AppendLine("## Findings");
        report.AppendLine();
        if (findings.Length == 0)
        {
            report.AppendLine("- No route-blocking scene composition issues were found by the deterministic audit.");
        }
        else
        {
            report.Append(findings);
        }

        report.AppendLine("- The full automated matrix still remains the source of truth for objective completion, combat, hazards, secrets, settings, and build health.");
        report.AppendLine("- Human feel review is still required for movement comfort, encounter pacing, audio mix, and final art readability.");
        report.AppendLine();
        report.AppendLine("## Next Actionable Slices");
        report.AppendLine();
        report.AppendLine("- `v0.1.5`: Level01 route and onboarding polish.");
        report.AppendLine("- `v0.1.6`: Level02/Level03 midgame pacing polish.");
        report.AppendLine("- `v0.1.7`: Level04/Level05 climax polish.");
        report.AppendLine("- `v0.1.8`: audio listen, mix, and import tuning.");
        report.AppendLine("- `v0.1.9`: settings, readability, and Windows options polish.");

        string absoluteOutputPath = Path.Combine(Directory.GetCurrentDirectory(), OutputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(absoluteOutputPath));
        File.WriteAllText(absoluteOutputPath, report.ToString());
        AssetDatabase.Refresh();

        if (blockingIssues > 0)
        {
            throw new InvalidOperationException("Route audit failed with " + blockingIssues + " blocking issue(s). See " + OutputPath + ".");
        }

        Debug.Log("V0_ROUTE_AUDIT_PASS " + OutputPath);
    }

    private static RouteSceneMetrics CollectMetrics(string sceneName)
    {
        RouteSceneMetrics metrics = new RouteSceneMetrics(sceneName);

        PlayerController player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
        LockedDoor door = UnityEngine.Object.FindAnyObjectByType<LockedDoor>();
        SteamValveObjective valve = UnityEngine.Object.FindAnyObjectByType<SteamValveObjective>();
        LevelTransitionTrigger transition = UnityEngine.Object.FindAnyObjectByType<LevelTransitionTrigger>();
        ExitTrigger exit = UnityEngine.Object.FindAnyObjectByType<ExitTrigger>();
        GuardianDefeatObjective guardian = UnityEngine.Object.FindAnyObjectByType<GuardianDefeatObjective>();
        GovernorWardenController warden = UnityEngine.Object.FindAnyObjectByType<GovernorWardenController>();

        PickupCounts pickupCounts = CountPickups();
        int scrappers = Count<EnemyController>();
        int lancers = Count<RangedEnemyController>();
        int bulwarks = Count<BulwarkEnemyController>();
        int bellowsNodes = Count<BellowsNodeController>();
        int wardens = Count<GovernorWardenController>();
        int steamHazards = Count<SteamHazard>();
        int furnaceHazards = Count<FurnaceHeatHazard>();
        int secrets = Count<SecretArea>();
        int lorePlaques = Count<LorePlaque>();

        metrics.CoreRouteObjects = BuildCoreRouteSummary(sceneName, door, valve, transition, exit, guardian, warden);
        metrics.Enemies = "S:" + scrappers + " L:" + lancers + " B:" + bulwarks + " N:" + bellowsNodes + " W:" + wardens;
        metrics.Pickups = "H:" + pickupCounts.Health + " A:" + pickupCounts.Ammo + " K:" + pickupCounts.Key + " W:" + pickupCounts.Weapon;
        metrics.Hazards = "Steam:" + steamHazards + " Furnace:" + furnaceHazards;
        metrics.Secrets = secrets.ToString();
        metrics.TransitionOrExit = BuildTransitionSummary(transition, exit);
        metrics.Notes = BuildDistanceNotes(sceneName, player, door, valve, transition, exit, warden, lorePlaques);

        ValidateRoute(sceneName, metrics, door, valve, transition, exit, guardian, warden, pickupCounts, secrets);
        return metrics;
    }

    private static string BuildCoreRouteSummary(string sceneName, LockedDoor door, SteamValveObjective valve, LevelTransitionTrigger transition, ExitTrigger exit, GuardianDefeatObjective guardian, GovernorWardenController warden)
    {
        if (sceneName == "Level01")
        {
            return "Gate:" + YesNo(door != null) + " Lift:" + YesNo(transition != null);
        }

        if (sceneName == "Level02" || sceneName == "Level03")
        {
            return "Valve:" + YesNo(valve != null) + " Lift:" + YesNo(transition != null);
        }

        if (sceneName == "Level04")
        {
            return "Emergency lift:" + YesNo(transition != null);
        }

        return "Warden:" + YesNo(warden != null) + " Guardian lock:" + YesNo(guardian != null) + " Exit:" + YesNo(exit != null);
    }

    private static string BuildTransitionSummary(LevelTransitionTrigger transition, ExitTrigger exit)
    {
        if (transition != null)
        {
            return string.IsNullOrWhiteSpace(transition.targetSceneName) ? "missing target" : "to " + transition.targetSceneName + (transition.IsLocked ? " locked" : " open");
        }

        if (exit != null)
        {
            return exit.IsLocked ? "final exit locked" : "final exit open";
        }

        return "none";
    }

    private static string BuildDistanceNotes(string sceneName, PlayerController player, LockedDoor door, SteamValveObjective valve, LevelTransitionTrigger transition, ExitTrigger exit, GovernorWardenController warden, int lorePlaques)
    {
        StringBuilder notes = new StringBuilder();
        notes.Append("Plaques:" + lorePlaques);

        if (player != null && door != null)
        {
            notes.Append(" Spawn->gate " + FormatDistance(player.transform.position, door.transform.position));
        }

        if (player != null && valve != null)
        {
            notes.Append(" Spawn->valve " + FormatDistance(player.transform.position, valve.transform.position));
        }

        if (player != null && transition != null)
        {
            notes.Append(" Spawn->lift " + FormatDistance(player.transform.position, transition.transform.position));
        }

        if (warden != null && exit != null)
        {
            notes.Append(" Warden->exit " + FormatDistance(warden.transform.position, exit.transform.position));
        }

        if (sceneName == "Level05" && player != null && warden != null)
        {
            notes.Append(" Spawn->Warden " + FormatDistance(player.transform.position, warden.transform.position));
        }

        return notes.ToString();
    }

    private static void ValidateRoute(string sceneName, RouteSceneMetrics metrics, LockedDoor door, SteamValveObjective valve, LevelTransitionTrigger transition, ExitTrigger exit, GuardianDefeatObjective guardian, GovernorWardenController warden, PickupCounts pickups, int secrets)
    {
        if (sceneName == "Level01")
        {
            metrics.Require(door != null, "Level01 requires a pressure gate.");
            metrics.Require(transition != null && transition.targetSceneName == "Level02", "Level01 lift must target Level02.");
            metrics.Require(pickups.Key > 0, "Level01 needs a gear-key pickup.");
            return;
        }

        if (sceneName == "Level02")
        {
            metrics.Require(valve != null, "Level02 requires the Pipeworks routing valve.");
            metrics.Require(transition != null && transition.targetSceneName == "Level03", "Level02 lift must target Level03.");
            metrics.Require(transition != null && transition.requiredValve == valve, "Level02 lift must be linked to the routing valve.");
            metrics.Require(secrets > 0, "Level02 should retain its secret cache.");
            return;
        }

        if (sceneName == "Level03")
        {
            metrics.Require(valve != null, "Level03 requires the Boilerheart pressure valve.");
            metrics.Require(transition != null && transition.targetSceneName == "Level04", "Level03 lift must target Level04.");
            metrics.Require(transition != null && transition.requiredValve == valve, "Level03 lift must be linked to the Boilerheart valve.");
            metrics.Require(pickups.Weapon > 0, "Level03 should retain the Steam Scattergun pickup.");
            return;
        }

        if (sceneName == "Level04")
        {
            metrics.Require(transition != null && transition.targetSceneName == "Level05", "Level04 emergency hoist must target Level05.");
            metrics.Require(Count<BulwarkEnemyController>() > 0, "Level04 needs at least one Bulwark encounter.");
            metrics.Require(secrets > 0, "Level04 should retain its secret cache.");
            return;
        }

        if (sceneName == "Level05")
        {
            metrics.Require(warden != null, "Level05 requires the Governor Warden.");
            metrics.Require(exit != null, "Level05 requires the final exit.");
            metrics.Require(guardian != null && exit != null && exit.requiredGuardian == guardian, "Level05 final exit must be linked to the guardian objective.");
        }
    }

    private static int Count<T>() where T : UnityEngine.Object
    {
        return UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None).Length;
    }

    private static PickupCounts CountPickups()
    {
        PickupCounts counts = new PickupCounts();
        Pickup[] pickups = UnityEngine.Object.FindObjectsByType<Pickup>(FindObjectsSortMode.None);
        for (int i = 0; i < pickups.Length; i++)
        {
            switch (pickups[i].kind)
            {
                case PickupKind.Health:
                    counts.Health++;
                    break;
                case PickupKind.Ammo:
                    counts.Ammo++;
                    break;
                case PickupKind.Key:
                    counts.Key++;
                    break;
                case PickupKind.Weapon:
                    counts.Weapon++;
                    break;
            }
        }

        return counts;
    }

    private static string FormatDistance(Vector3 a, Vector3 b)
    {
        Vector2 flatA = new Vector2(a.x, a.z);
        Vector2 flatB = new Vector2(b.x, b.z);
        return Vector2.Distance(flatA, flatB).ToString("0.0") + "m";
    }

    private static string YesNo(bool condition)
    {
        return condition ? "yes" : "no";
    }

    private struct PickupCounts
    {
        public int Health;
        public int Ammo;
        public int Key;
        public int Weapon;
    }

    private sealed class RouteSceneMetrics
    {
        private readonly StringBuilder findings = new StringBuilder();

        public RouteSceneMetrics(string sceneName)
        {
            SceneName = sceneName;
        }

        public string SceneName { get; private set; }
        public string CoreRouteObjects { get; set; }
        public string Enemies { get; set; }
        public string Pickups { get; set; }
        public string Hazards { get; set; }
        public string Secrets { get; set; }
        public string TransitionOrExit { get; set; }
        public string Notes { get; set; }
        public int BlockingIssues { get; private set; }
        public string Findings => findings.ToString();

        public void Require(bool condition, string issue)
        {
            if (condition)
            {
                return;
            }

            BlockingIssues++;
            findings.AppendLine("- BLOCKER: " + issue);
        }

        public string ToMarkdownRow()
        {
            return "| " + SceneName + " | " + CoreRouteObjects + " | " + Enemies + " | " + Pickups + " | " + Hazards + " | " + Secrets + " | " + TransitionOrExit + " | " + Notes + " |";
        }
    }
}
