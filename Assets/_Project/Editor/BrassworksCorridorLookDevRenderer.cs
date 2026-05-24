using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class BrassworksCorridorLookDevRenderer
{
    private const int HeroWidth = 1920;
    private const int HeroHeight = 1080;
    private const int SheetWidth = 2200;
    private const int SheetHeight = 1400;
    private const int ContactWidth = 2400;
    private const int ContactHeight = 1500;
    private const string RenderFolderRelativePath = "Documentation/ConceptRenders/BrassworksCorridor";
    private const string ReportFolderRelativePath = "Documentation/AssetProduction/BrassworksCorridorLookDev";
    private const string CorridorRenderRelativePath = RenderFolderRelativePath + "/BBW_CORRIDOR_001_modular_corridor_wet_pipe_lamps.png";
    private const string VaultRenderRelativePath = RenderFolderRelativePath + "/BBW_CORRIDOR_002_hero_round_vault_door.png";
    private const string KitRenderRelativePath = RenderFolderRelativePath + "/BBW_CORRIDOR_003_wall_kit_component_sheet.png";
    private const string ContactSheetRelativePath = RenderFolderRelativePath + "/BBW_CORRIDOR_CONTACTSHEET_unity_lookdev.png";
    private const string MetricsRelativePath = ReportFolderRelativePath + "/brassworks_corridor_lookdev_metrics.json";
    private const string ReportRelativePath = ReportFolderRelativePath + "/BRASSWORKS_CORRIDOR_LOOKDEV_REPORT.md";
    private const string BatchLogRelativePath = ReportFolderRelativePath + "/unity_brassworks_corridor_lookdev_batch.log";

    [MenuItem("Project Tools/Lookdev/Render Brassworks Corridor LookDev")]
    public static void RenderFromMenu()
    {
        RenderBatch();
    }

    public static void RenderBatch()
    {
        try
        {
            RenderLookDev();
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(0);
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }

            throw;
        }
    }

    private static void RenderLookDev()
    {
        string projectRoot = GetProjectRoot();
        Directory.CreateDirectory(Path.Combine(projectRoot, RenderFolderRelativePath));
        Directory.CreateDirectory(Path.Combine(projectRoot, ReportFolderRelativePath));

        CorridorLookDevMetrics metrics = new CorridorLookDevMetrics
        {
            timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture),
            unityVersion = Application.unityVersion,
            batchmodeEntrypoint = "BrassworksCorridorLookDevRenderer.RenderBatch",
            exactUnityCommand = "\"C:\\Program Files\\Unity\\Hub\\Editor\\6000.4.6f1\\Editor\\Unity.exe\" -batchmode -quit -projectPath \"D:\\__MY APPS\\Unity Doom\" -executeMethod BrassworksCorridorLookDevRenderer.RenderBatch -logFile \"D:\\__MY APPS\\Unity Doom\\" + BatchLogRelativePath.Replace("/", "\\") + "\"",
            renderFolder = RenderFolderRelativePath,
            reportPath = ReportRelativePath,
            metricsPath = MetricsRelativePath,
            contactSheetPath = ContactSheetRelativePath,
            corridorRenderPath = CorridorRenderRelativePath,
            vaultRenderPath = VaultRenderRelativePath,
            kitRenderPath = KitRenderRelativePath,
            toolPolicy = "Unity-only editor batch render. Temporary in-memory scenes, procedural primitives, procedural material textures, Camera.Render, PNG export. No Blender or external DCC.",
            ownedWriteScope = "Assets/_Project/Editor/BrassworksCorridorLookDevRenderer.cs plus Documentation/ConceptRenders/BrassworksCorridor/** and Documentation/AssetProduction/BrassworksCorridorLookDev/**.",
            northStarTarget = "Dark wet industrial brassworks corridor: riveted brass and blackened steel, dense pipework, pressure tanks, warm lamps, steam vents, heavy round vault door, and high-realism material cues."
        };

        List<RenderResult> results = new List<RenderResult>();
        Texture2D contactSheet = null;

        try
        {
            results.Add(RenderSingleView(projectRoot, "corridor_module", "Corridor module with pipes, lamps, and wet floor", CorridorRenderRelativePath, HeroWidth, HeroHeight, BuildCorridorModuleView, new Vector3(0f, 1.18f, -6.45f), new Vector3(0f, 1.22f, 2.35f), 54f, false, 5.2f));
            results.Add(RenderSingleView(projectRoot, "hero_vault_door", "Hero round vault door module", VaultRenderRelativePath, HeroWidth, HeroHeight, BuildHeroVaultDoorView, new Vector3(0f, 1.36f, -5.25f), new Vector3(0f, 1.38f, 0.05f), 38f, false, 5.4f));
            results.Add(RenderSingleView(projectRoot, "wall_kit_sheet", "Reusable wall-kit component sheet", KitRenderRelativePath, SheetWidth, SheetHeight, BuildWallKitSheetView, new Vector3(0f, 0f, -7.8f), new Vector3(0f, 0f, 0f), 34f, true, 4.2f));

            metrics.renders = new RenderMetric[results.Count];
            for (int i = 0; i < results.Count; i++)
            {
                metrics.renders[i] = results[i].Metric;
                metrics.totalPrimitiveCount += results[i].Stats.primitiveCount;
                metrics.totalRivets += results[i].Stats.rivets;
                metrics.totalPipeSegments += results[i].Stats.pipeSegments;
                metrics.totalLamps += results[i].Stats.lamps;
                metrics.totalSteamPuffs += results[i].Stats.steamPuffs;
                metrics.totalGauges += results[i].Stats.gauges;
                metrics.totalValves += results[i].Stats.valves;
                metrics.totalPressureTanks += results[i].Stats.pressureTanks;
                metrics.totalFloorTiles += results[i].Stats.floorTiles;
                metrics.totalWallPanels += results[i].Stats.wallPanels;
                metrics.totalWetHighlights += results[i].Stats.wetHighlights;
            }

            metrics.materialRoles = LookDevMaterials.MaterialRoleCount;
            metrics.checks = BuildChecks(metrics);
            metrics.overallStatus = DetermineOverallStatus(metrics.checks);

            contactSheet = BuildContactSheet(projectRoot, results);
            File.WriteAllBytes(Path.Combine(projectRoot, ContactSheetRelativePath), contactSheet.EncodeToPNG());
            File.WriteAllText(Path.Combine(projectRoot, MetricsRelativePath), JsonUtility.ToJson(metrics, true));
            File.WriteAllText(Path.Combine(projectRoot, ReportRelativePath), BuildReport(metrics));

            Debug.Log("Brassworks corridor lookdev contact sheet written to " + Path.Combine(projectRoot, ContactSheetRelativePath));
            Debug.Log("Brassworks corridor lookdev report written to " + Path.Combine(projectRoot, ReportRelativePath));
        }
        finally
        {
            DestroyTexture(contactSheet);
            for (int i = 0; i < results.Count; i++)
            {
                DestroyTexture(results[i].Texture);
            }

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }
    }

    private static RenderResult RenderSingleView(string projectRoot, string key, string title, string relativePath, int width, int height, ViewBuilder builder, Vector3 cameraPosition, Vector3 lookAt, float fieldOfView, bool orthographic, float orthographicSize)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings();

        LookDevMaterials materials = LookDevMaterials.Create();
        ViewStats stats = new ViewStats();
        Texture2D image = null;

        try
        {
            GameObject root = new GameObject("BrassworksCorridorLookDev_" + key);
            builder(root.transform, materials, stats);
            BuildSharedLighting(key);

            Camera camera = CreateCamera(key + " camera", cameraPosition, lookAt, fieldOfView, orthographic, orthographicSize);
            image = CaptureCamera(camera, width, height);
            image.hideFlags = HideFlags.DontUnloadUnusedAsset;

            RenderMetric metric = AnalyzeImage(key, title, relativePath, width, height, image, cameraPosition, lookAt, fieldOfView, orthographic, stats);
            string outputPath = Path.Combine(projectRoot, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            File.WriteAllBytes(outputPath, image.EncodeToPNG());
            Debug.Log("Brassworks corridor lookdev render written to " + outputPath);

            return new RenderResult
            {
                Key = key,
                Title = title,
                RelativePath = relativePath,
                Texture = image,
                Metric = metric,
                Stats = stats
            };
        }
        catch
        {
            DestroyTexture(image);
            throw;
        }
        finally
        {
            materials.DestroyAll();
        }
    }

    private delegate void ViewBuilder(Transform root, LookDevMaterials materials, ViewStats stats);

    private static void ConfigureRenderSettings()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.024f, 0.021f, 0.018f, 1f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(0.018f, 0.017f, 0.016f, 1f);
        RenderSettings.fogDensity = 0.018f;
        RenderSettings.reflectionIntensity = 0.72f;
    }

    private static void BuildCorridorModuleView(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        BuildCorridorArchitecture(root, materials, stats);
        BuildCorridorPipes(root, materials, stats);
        BuildCorridorLamps(root, materials, stats);
        BuildCorridorWetLayer(root, materials, stats);
        BuildCorridorSteam(root, materials, stats);
        BuildCorridorEndPressureDoorHint(root, materials, stats);
        stats.notes = "4m-wide wet corridor module with ribbed iron bays, brick infill, pipe canopy, caged amber lamps, pressure tanks, gauge/valve details, steam puffs, and oily floor reflections.";
    }

    private static void BuildCorridorArchitecture(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        CreateBox("one-piece wet oil-dark corridor floor base", new Vector3(0f, -0.08f, 1.7f), new Vector3(4.95f, 0.12f, 12.6f), Quaternion.identity, materials.WetStone, root, stats);
        CreateBox("left soot brick structural wall", new Vector3(-2.48f, 1.1f, 1.7f), new Vector3(0.16f, 2.55f, 12.6f), Quaternion.identity, materials.SootBrick, root, stats);
        CreateBox("right soot brick structural wall", new Vector3(2.48f, 1.1f, 1.7f), new Vector3(0.16f, 2.55f, 12.6f), Quaternion.identity, materials.SootBrick, root, stats);
        CreateBox("low blackened ceiling plate", new Vector3(0f, 2.42f, 1.7f), new Vector3(4.9f, 0.14f, 12.6f), Quaternion.identity, materials.BlackenedSteel, root, stats);

        for (int zIndex = 0; zIndex < 8; zIndex++)
        {
            float z = -3.8f + zIndex * 1.55f;
            CreateBox("wet floor tile left " + zIndex.ToString(CultureInfo.InvariantCulture), new Vector3(-1.22f, 0.005f, z), new Vector3(2.12f, 0.035f, 1.32f), Quaternion.identity, materials.WetStone, root, stats);
            CreateBox("wet floor tile right " + zIndex.ToString(CultureInfo.InvariantCulture), new Vector3(1.22f, 0.008f, z + 0.05f), new Vector3(2.12f, 0.035f, 1.27f), Quaternion.identity, materials.WetStone, root, stats);
            CreateBox("brass floor seam strip " + zIndex.ToString(CultureInfo.InvariantCulture), new Vector3(0f, 0.034f, z + 0.72f), new Vector3(4.35f, 0.024f, 0.035f), Quaternion.identity, materials.DarkBrass, root, stats);
            stats.floorTiles += 2;
        }

        for (int i = 0; i < 9; i++)
        {
            float z = -4.55f + i * 1.55f;
            CreateBox("left blackened iron corridor rib " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-2.26f, 1.22f, z), new Vector3(0.22f, 2.42f, 0.12f), Quaternion.identity, materials.BlackenedSteel, root, stats);
            CreateBox("right blackened iron corridor rib " + i.ToString(CultureInfo.InvariantCulture), new Vector3(2.26f, 1.22f, z), new Vector3(0.22f, 2.42f, 0.12f), Quaternion.identity, materials.BlackenedSteel, root, stats);
            CreateBox("ceiling cross rib " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0f, 2.27f, z), new Vector3(4.5f, 0.22f, 0.13f), Quaternion.identity, materials.BlackenedSteel, root, stats);
            AddRivetRow("left rib brass rivet row " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-2.38f, 0.26f, z - 0.07f), new Vector3(-2.38f, 2.14f, z - 0.07f), 8, 0.032f, materials.AgedBrass, root, stats);
            AddRivetRow("right rib brass rivet row " + i.ToString(CultureInfo.InvariantCulture), new Vector3(2.38f, 0.26f, z - 0.07f), new Vector3(2.38f, 2.14f, z - 0.07f), 8, 0.032f, materials.AgedBrass, root, stats);
        }

        for (int i = 0; i < 6; i++)
        {
            float z = -3.65f + i * 1.82f;
            CreateBox("left riveted black wall service panel " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-2.355f, 1.1f, z), new Vector3(0.05f, 1.18f, 0.92f), Quaternion.identity, materials.BlackenedSteel, root, stats);
            CreateBox("right riveted brass service panel " + i.ToString(CultureInfo.InvariantCulture), new Vector3(2.355f, 1.1f, z + 0.52f), new Vector3(0.05f, 1.0f, 0.78f), Quaternion.identity, i % 2 == 0 ? materials.DarkBrass : materials.BlackenedSteel, root, stats);
            AddRivetGrid("left panel rivet grid " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-2.395f, 0.62f, z - 0.36f), 2, 4, 0f, 0.24f, 0.022f, materials.AgedBrass, root, stats, Vector3.forward, 0.72f);
            AddRivetGrid("right panel rivet grid " + i.ToString(CultureInfo.InvariantCulture), new Vector3(2.395f, 0.72f, z + 0.22f), 2, 3, 0f, 0.25f, 0.022f, materials.AgedBrass, root, stats, Vector3.forward, 0.54f);
            stats.wallPanels += 2;
        }

        CreateBox("left greasy brass hand rail", new Vector3(-2.18f, 0.82f, 1.7f), new Vector3(0.08f, 0.08f, 10.2f), Quaternion.identity, materials.AgedBrass, root, stats);
        CreateBox("right greasy brass hand rail", new Vector3(2.18f, 0.82f, 1.7f), new Vector3(0.08f, 0.08f, 10.2f), Quaternion.identity, materials.AgedBrass, root, stats);
    }

    private static void BuildCorridorPipes(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        float[] zPipeYs = { 2.1f, 1.88f, 1.68f };
        for (int i = 0; i < zPipeYs.Length; i++)
        {
            Material material = i == 1 ? materials.Copper : (i == 2 ? materials.DarkPipeMetal : materials.AgedBrass);
            CreateCylinderZ("left longitudinal pipe run " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-1.72f + i * 0.13f, zPipeYs[i], 1.35f), 11.0f, 0.055f - i * 0.005f, material, root, stats);
            CreateCylinderZ("right longitudinal pipe run " + i.ToString(CultureInfo.InvariantCulture), new Vector3(1.72f - i * 0.11f, zPipeYs[i] - 0.05f, 1.4f), 10.4f, 0.05f, material, root, stats);
            stats.pipeSegments += 2;
        }

        for (int i = 0; i < 7; i++)
        {
            float z = -3.65f + i * 1.42f;
            CreateCylinderX("ceiling brass cross manifold " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0f, 2.02f, z), 2.95f, 0.042f, i % 2 == 0 ? materials.AgedBrass : materials.Copper, root, stats);
            CreateBox("left pipe clamp " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-1.6f, 2.05f, z), new Vector3(0.28f, 0.08f, 0.08f), Quaternion.identity, materials.BlackenedSteel, root, stats);
            CreateBox("right pipe clamp " + i.ToString(CultureInfo.InvariantCulture), new Vector3(1.6f, 2.0f, z), new Vector3(0.28f, 0.08f, 0.08f), Quaternion.identity, materials.BlackenedSteel, root, stats);
            stats.pipeSegments++;
        }

        BuildPressureTank(new Vector3(-2.05f, 1.2f, -1.3f), Quaternion.Euler(0f, 0f, 0f), 0.46f, 1.35f, materials, root, stats);
        BuildPressureTank(new Vector3(2.05f, 1.18f, 2.45f), Quaternion.Euler(0f, 0f, 0f), 0.38f, 1.15f, materials, root, stats);
        BuildGaugeCluster(new Vector3(-2.08f, 1.47f, 0.35f), Quaternion.Euler(0f, 90f, 0f), 0.52f, materials, root, stats);
        BuildValveWheel(new Vector3(2.1f, 1.38f, -0.75f), Quaternion.Euler(0f, 90f, 0f), 0.42f, materials, root, stats);
    }

    private static void BuildCorridorLamps(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        BuildCagedLamp(new Vector3(-2.12f, 1.58f, -3.0f), Quaternion.Euler(0f, 90f, 0f), 0.7f, materials, root, stats);
        BuildCagedLamp(new Vector3(2.12f, 1.58f, -1.0f), Quaternion.Euler(0f, -90f, 0f), 0.7f, materials, root, stats);
        BuildCagedLamp(new Vector3(-2.12f, 1.58f, 1.1f), Quaternion.Euler(0f, 90f, 0f), 0.7f, materials, root, stats);
        BuildCagedLamp(new Vector3(2.12f, 1.58f, 3.25f), Quaternion.Euler(0f, -90f, 0f), 0.7f, materials, root, stats);
    }

    private static void BuildCorridorWetLayer(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        CreateGroundQuad("long broken amber wet reflection", new Vector3(-0.45f, 0.046f, -0.92f), new Vector2(2.15f, 3.25f), Quaternion.Euler(0f, 0f, -4f), materials.WetHighlight, root, stats);
        CreateGroundQuad("right thin brass wet reflection", new Vector3(1.18f, 0.048f, 1.75f), new Vector2(0.92f, 2.2f), Quaternion.Euler(0f, 0f, 7f), materials.WetHighlight, root, stats);
        CreateGroundQuad("left oily black puddle", new Vector3(-1.45f, 0.049f, 3.15f), new Vector2(0.72f, 1.45f), Quaternion.Euler(0f, 0f, -12f), materials.OilFilm, root, stats);
        CreateGroundQuad("front oil smear", new Vector3(0.75f, 0.05f, -3.75f), new Vector2(1.1f, 0.56f), Quaternion.Euler(0f, 0f, 16f), materials.OilFilm, root, stats);
        stats.wetHighlights += 4;

        for (int i = 0; i < 5; i++)
        {
            float x = -1.6f + i * 0.8f;
            CreateBox("floor drain grate bar " + i.ToString(CultureInfo.InvariantCulture), new Vector3(x, 0.062f, 4.4f), new Vector3(0.08f, 0.025f, 1.0f), Quaternion.identity, materials.BlackenedSteel, root, stats);
        }
    }

    private static void BuildCorridorSteam(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        BuildSteamVent(new Vector3(-2.17f, 0.62f, -2.18f), Quaternion.Euler(0f, 90f, 0f), 0.72f, materials, root, stats);
        BuildSteamVent(new Vector3(2.17f, 0.72f, 1.08f), Quaternion.Euler(0f, -90f, 0f), 0.62f, materials, root, stats);
        BuildSteamPuff("ceiling pinhole steam haze", new Vector3(0.72f, 2.02f, 2.35f), new Vector2(0.62f, 0.5f), Quaternion.Euler(0f, 180f, 0f), materials.Steam, root, stats);
    }

    private static void BuildCorridorEndPressureDoorHint(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        Vector3 center = new Vector3(0f, 1.25f, 5.28f);
        CreateBox("far blackened pressure-door frame", new Vector3(0f, 1.2f, 5.45f), new Vector3(3.9f, 2.55f, 0.22f), Quaternion.identity, materials.BlackenedSteel, root, stats);
        CreateCylinderZ("far round brass pressure-door rim", center + new Vector3(0f, 0f, -0.12f), 0.12f, 1.05f, materials.DarkBrass, root, stats);
        CreateCylinderZ("far round black pressure-door face", center + new Vector3(0f, 0f, -0.21f), 0.1f, 0.92f, materials.BlackenedSteel, root, stats);
        AddRivetRing("far door rim rivet", center + new Vector3(0f, 0f, -0.3f), 0.92f, 18, 0.026f, materials.AgedBrass, root, stats);
        BuildValveWheel(center + new Vector3(0f, 0f, -0.38f), Quaternion.identity, 0.32f, materials, root, stats);
    }

    private static void BuildHeroVaultDoorView(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        CreateBox("vault wet foreground floor", new Vector3(0f, -0.08f, -1.25f), new Vector3(5.4f, 0.12f, 5.5f), Quaternion.identity, materials.WetStone, root, stats);
        CreateBox("vault soot brick rear wall", new Vector3(0f, 1.22f, 0.32f), new Vector3(5.45f, 2.85f, 0.32f), Quaternion.identity, materials.SootBrick, root, stats);
        CreateBox("vault left iron jamb", new Vector3(-2.15f, 1.22f, 0.08f), new Vector3(0.28f, 2.7f, 0.48f), Quaternion.identity, materials.BlackenedSteel, root, stats);
        CreateBox("vault right iron jamb", new Vector3(2.15f, 1.22f, 0.08f), new Vector3(0.28f, 2.7f, 0.48f), Quaternion.identity, materials.BlackenedSteel, root, stats);
        CreateBox("vault top iron lintel", new Vector3(0f, 2.44f, 0.08f), new Vector3(4.55f, 0.3f, 0.48f), Quaternion.identity, materials.BlackenedSteel, root, stats);

        Vector3 doorCenter = new Vector3(0f, 1.26f, -0.12f);
        CreateCylinderZ("hero outer brass vault seal", doorCenter + new Vector3(0f, 0f, 0.05f), 0.22f, 1.72f, materials.AgedBrass, root, stats);
        CreateCylinderZ("hero blackened round vault slab", doorCenter + new Vector3(0f, 0f, -0.09f), 0.24f, 1.52f, materials.BlackenedSteel, root, stats);
        CreateCylinderZ("hero dark brass inner seal", doorCenter + new Vector3(0f, 0f, -0.25f), 0.12f, 1.12f, materials.DarkBrass, root, stats);
        CreateCylinderZ("hero soot-dark inner pressure plate", doorCenter + new Vector3(0f, 0f, -0.34f), 0.1f, 0.92f, materials.BlackenedSteel, root, stats);
        CreateCylinderZ("hero central aged brass hub plate", doorCenter + new Vector3(0f, 0f, -0.47f), 0.16f, 0.38f, materials.AgedBrass, root, stats);

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            CreateBox("hero radial black steel brace " + i.ToString(CultureInfo.InvariantCulture), doorCenter + new Vector3(0f, 0f, -0.56f), new Vector3(0.13f, 1.72f, 0.13f), Quaternion.Euler(0f, 0f, angle), i % 2 == 0 ? materials.BlackenedSteel : materials.DarkBrass, root, stats);
        }

        AddRivetRing("hero outer rim brass bolt", doorCenter + new Vector3(0f, 0f, -0.58f), 1.56f, 32, 0.035f, materials.AgedBrass, root, stats);
        AddRivetRing("hero inner rim black bolt", doorCenter + new Vector3(0f, 0f, -0.62f), 1.08f, 24, 0.027f, materials.IronEdge, root, stats);
        BuildValveWheel(doorCenter + new Vector3(0f, 0f, -0.72f), Quaternion.identity, 0.58f, materials, root, stats);

        BuildCagedLamp(new Vector3(-1.88f, 2.0f, -0.42f), Quaternion.identity, 0.72f, materials, root, stats);
        BuildCagedLamp(new Vector3(1.88f, 2.0f, -0.42f), Quaternion.identity, 0.72f, materials, root, stats);
        BuildGaugeCluster(new Vector3(-1.72f, 1.08f, -0.62f), Quaternion.identity, 0.58f, materials, root, stats);
        BuildValveWheel(new Vector3(1.62f, 1.06f, -0.62f), Quaternion.identity, 0.42f, materials, root, stats);
        BuildPressureTank(new Vector3(-2.45f, 0.78f, -0.52f), Quaternion.Euler(0f, 0f, 90f), 0.34f, 1.1f, materials, root, stats);
        BuildPressureTank(new Vector3(2.45f, 0.78f, -0.52f), Quaternion.Euler(0f, 0f, 90f), 0.34f, 1.1f, materials, root, stats);

        CreateCylinderX("vault upper copper pipe", new Vector3(0f, 2.22f, -0.48f), 4.3f, 0.055f, materials.Copper, root, stats);
        CreateCylinderX("vault lower dark pressure pipe", new Vector3(0f, 0.38f, -0.48f), 4.05f, 0.055f, materials.DarkPipeMetal, root, stats);
        stats.pipeSegments += 2;

        CreateGroundQuad("vault broad floor reflection", new Vector3(0.1f, 0.045f, -1.9f), new Vector2(2.65f, 1.18f), Quaternion.Euler(0f, 0f, -2f), materials.WetHighlight, root, stats);
        CreateGroundQuad("vault oil shadow smear", new Vector3(-0.95f, 0.048f, -1.14f), new Vector2(1.0f, 0.6f), Quaternion.Euler(0f, 0f, 9f), materials.OilFilm, root, stats);
        stats.wetHighlights += 2;
        BuildSteamVent(new Vector3(1.95f, 0.42f, -0.55f), Quaternion.identity, 0.5f, materials, root, stats);
        stats.notes = "Front hero module for the heavy round pressure vault door with layered discs, radial braces, rivet rings, valve wheel, pressure tanks, gauges, lamps, pipe runs, wet floor, and a small steam vent.";
    }

    private static void BuildWallKitSheetView(Transform root, LookDevMaterials materials, ViewStats stats)
    {
        RenderSettings.fog = false;
        CreateBox("component sheet dark inspection backplate", new Vector3(0f, 0f, 0.65f), new Vector3(7.4f, 4.8f, 0.1f), Quaternion.identity, materials.ContactBack, root, stats);
        CreateBox("component sheet brass lower ledge", new Vector3(0f, -2.05f, 0.38f), new Vector3(7.2f, 0.08f, 0.82f), Quaternion.identity, materials.DarkBrass, root, stats);

        BuildPipeClusterComponent(new Vector3(-2.55f, 1.15f, -0.05f), 0.92f, materials, root, stats);
        BuildRivetedPanelComponent(new Vector3(0f, 1.15f, -0.08f), 1.05f, materials, root, stats);
        BuildLampComponent(new Vector3(2.55f, 1.15f, -0.05f), 1.0f, materials, root, stats);
        BuildValveWheel(new Vector3(-2.45f, -1.03f, -0.18f), Quaternion.identity, 0.68f, materials, root, stats);
        BuildPressureTank(new Vector3(0f, -1.04f, -0.12f), Quaternion.Euler(0f, 0f, 90f), 0.38f, 1.58f, materials, root, stats);
        BuildFloorTileComponent(new Vector3(2.55f, -1.08f, -0.16f), 0.96f, materials, root, stats);

        CreateBox("component separator top", new Vector3(0f, 0.07f, 0.04f), new Vector3(7.0f, 0.025f, 0.035f), Quaternion.identity, materials.IronEdge, root, stats);
        CreateBox("component separator left", new Vector3(-1.28f, 0f, 0.04f), new Vector3(0.025f, 4.1f, 0.035f), Quaternion.identity, materials.IronEdge, root, stats);
        CreateBox("component separator right", new Vector3(1.28f, 0f, 0.04f), new Vector3(0.025f, 4.1f, 0.035f), Quaternion.identity, materials.IronEdge, root, stats);

        CreateSmallStencilLabel("PIPE CLUSTER", new Vector3(-3.25f, 2.1f, -0.2f), 0.115f, root);
        CreateSmallStencilLabel("RIVETED PANEL", new Vector3(-0.7f, 2.1f, -0.2f), 0.115f, root);
        CreateSmallStencilLabel("LAMP", new Vector3(2.18f, 2.1f, -0.2f), 0.115f, root);
        CreateSmallStencilLabel("VALVE", new Vector3(-2.82f, -0.12f, -0.2f), 0.115f, root);
        CreateSmallStencilLabel("PRESSURE TANK", new Vector3(-0.65f, -0.12f, -0.2f), 0.115f, root);
        CreateSmallStencilLabel("WET FLOOR TILE", new Vector3(1.9f, -0.12f, -0.2f), 0.115f, root);
        stats.notes = "Reusable wall-kit sheet showing pipe cluster, riveted panel, caged lamp, valve wheel, pressure tank, and wet floor tile as isolated future modular art targets.";
    }

    private static void BuildPipeClusterComponent(Vector3 origin, float scale, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        CreateBox("pipe cluster rear black iron bracket", origin + new Vector3(0f, 0f, 0.09f) * scale, new Vector3(1.42f, 1.18f, 0.1f) * scale, Quaternion.identity, materials.BlackenedSteel, root, stats);
        for (int i = 0; i < 5; i++)
        {
            float y = (-0.42f + i * 0.2f) * scale;
            Material material = i % 3 == 0 ? materials.AgedBrass : (i % 3 == 1 ? materials.Copper : materials.DarkPipeMetal);
            CreateCylinderX("component pipe horizontal run " + i.ToString(CultureInfo.InvariantCulture), origin + new Vector3(0f, y, -0.12f), 1.62f * scale, (0.035f + i * 0.004f) * scale, material, root, stats);
            stats.pipeSegments++;
        }

        CreateCylinderY("component pipe vertical riser left", origin + new Vector3(-0.46f, 0.0f, -0.18f) * scale, 1.02f * scale, 0.042f * scale, materials.DarkPipeMetal, root, stats);
        CreateCylinderY("component pipe vertical copper riser right", origin + new Vector3(0.54f, -0.06f, -0.18f) * scale, 0.92f * scale, 0.032f * scale, materials.Copper, root, stats);
        stats.pipeSegments += 2;
        for (int i = 0; i < 4; i++)
        {
            CreateBox("component pipe clamp " + i.ToString(CultureInfo.InvariantCulture), origin + new Vector3(-0.72f + i * 0.48f, 0.42f, -0.24f) * scale, new Vector3(0.12f, 0.34f, 0.07f) * scale, Quaternion.identity, materials.AgedBrass, root, stats);
        }
    }

    private static void BuildRivetedPanelComponent(Vector3 origin, float scale, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        CreateBox("component soot brick panel backing", origin + new Vector3(0f, 0f, 0.06f) * scale, new Vector3(1.35f, 1.42f, 0.12f) * scale, Quaternion.identity, materials.SootBrick, root, stats);
        CreateBox("component black iron panel face", origin + new Vector3(0f, 0f, -0.04f) * scale, new Vector3(1.08f, 1.16f, 0.08f) * scale, Quaternion.identity, materials.BlackenedSteel, root, stats);
        CreateBox("component brass panel top strap", origin + new Vector3(0f, 0.52f, -0.11f) * scale, new Vector3(1.2f, 0.08f, 0.08f) * scale, Quaternion.identity, materials.AgedBrass, root, stats);
        CreateBox("component brass panel bottom strap", origin + new Vector3(0f, -0.52f, -0.11f) * scale, new Vector3(1.2f, 0.08f, 0.08f) * scale, Quaternion.identity, materials.DarkBrass, root, stats);
        AddRivetGrid("component panel rivets", origin + new Vector3(-0.48f, -0.42f, -0.2f) * scale, 4, 4, 0.32f * scale, 0.28f * scale, 0.026f * scale, materials.AgedBrass, root, stats, Vector3.zero, 0f);
        stats.wallPanels++;
    }

    private static void BuildLampComponent(Vector3 origin, float scale, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        BuildCagedLamp(origin, Quaternion.identity, scale, materials, root, stats);
        CreateBox("component lamp wall bracket plate", origin + new Vector3(0f, -0.56f, 0.08f) * scale, new Vector3(0.72f, 0.14f, 0.1f) * scale, Quaternion.identity, materials.BlackenedSteel, root, stats);
    }

    private static void BuildFloorTileComponent(Vector3 origin, float scale, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        CreateBox("component wet floor tile slab", origin + new Vector3(0f, 0f, 0.02f) * scale, new Vector3(1.42f, 1.05f, 0.1f) * scale, Quaternion.Euler(12f, 0f, 0f), materials.WetStone, root, stats);
        CreateBox("component floor brass threshold", origin + new Vector3(0f, 0.48f, -0.08f) * scale, new Vector3(1.42f, 0.08f, 0.08f) * scale, Quaternion.Euler(12f, 0f, 0f), materials.DarkBrass, root, stats);
        CreateBox("component floor drain slot", origin + new Vector3(0f, -0.34f, -0.08f) * scale, new Vector3(1.04f, 0.12f, 0.08f) * scale, Quaternion.Euler(12f, 0f, 0f), materials.BlackenedSteel, root, stats);
        CreateBox("component floor warm reflection strip", origin + new Vector3(-0.18f, 0.02f, -0.14f) * scale, new Vector3(0.72f, 0.06f, 0.08f) * scale, Quaternion.Euler(12f, 0f, -5f), materials.WetHighlightSolid, root, stats);
        stats.floorTiles++;
        stats.wetHighlights++;
    }

    private static void BuildGaugeCluster(Vector3 origin, Quaternion rotation, float scale, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        Transform group = CreateGroup("gauge cluster", origin, rotation, root);
        CreateCylinderZ("gauge black rear cup", Vector3.zero, 0.11f * scale, 0.32f * scale, materials.BlackenedSteel, group, stats);
        CreateCylinderZ("gauge brass bezel", new Vector3(0f, 0f, -0.08f * scale), 0.055f * scale, 0.29f * scale, materials.AgedBrass, group, stats);
        CreateCylinderZ("gauge cream enamel face", new Vector3(0f, 0f, -0.13f * scale), 0.018f * scale, 0.235f * scale, materials.GaugeFace, group, stats);
        for (int i = 0; i < 16; i++)
        {
            float angle = -135f + i * (270f / 15f);
            float radians = angle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Cos(radians) * 0.19f * scale, Mathf.Sin(radians) * 0.19f * scale, -0.15f * scale);
            CreateBox("gauge tick " + i.ToString(CultureInfo.InvariantCulture), position, new Vector3(0.008f, i % 4 == 0 ? 0.055f : 0.035f, 0.01f) * scale, Quaternion.Euler(0f, 0f, angle - 90f), materials.LineDark, group, stats);
        }

        CreateBox("gauge red needle", new Vector3(0.045f, 0.025f, -0.17f) * scale, new Vector3(0.018f, 0.17f, 0.011f) * scale, Quaternion.Euler(0f, 0f, -55f), materials.WarningRed, group, stats);
        CreateSphere("gauge brass hub", new Vector3(0f, 0f, -0.18f) * scale, Vector3.one * 0.07f * scale, materials.AgedBrass, group, stats);
        stats.gauges++;
    }

    private static void BuildValveWheel(Vector3 origin, Quaternion rotation, float scale, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        Transform group = CreateGroup("valve wheel", origin, rotation, root);
        int segments = 16;
        for (int i = 0; i < segments; i++)
        {
            float a0 = i * Mathf.PI * 2f / segments;
            float a1 = (i + 1) * Mathf.PI * 2f / segments;
            Vector3 p0 = new Vector3(Mathf.Cos(a0), Mathf.Sin(a0), 0f) * 0.32f * scale;
            Vector3 p1 = new Vector3(Mathf.Cos(a1), Mathf.Sin(a1), 0f) * 0.32f * scale;
            CreateCylinderBetween("valve outer ring segment " + i.ToString(CultureInfo.InvariantCulture), p0, p1, 0.018f * scale, materials.AgedBrass, group, stats);
        }

        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60f;
            CreateBox("valve wheel spoke " + i.ToString(CultureInfo.InvariantCulture), Vector3.zero, new Vector3(0.035f, 0.62f, 0.04f) * scale, Quaternion.Euler(0f, 0f, angle), i % 2 == 0 ? materials.DarkBrass : materials.AgedBrass, group, stats);
        }

        CreateCylinderZ("valve wheel central hub", new Vector3(0f, 0f, -0.04f * scale), 0.08f * scale, 0.12f * scale, materials.BlackenedSteel, group, stats);
        CreateSphere("valve wheel brass center nut", new Vector3(0f, 0f, -0.1f * scale), Vector3.one * 0.11f * scale, materials.AgedBrass, group, stats);
        stats.valves++;
    }

    private static void BuildPressureTank(Vector3 origin, Quaternion rotation, float radius, float length, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        Transform group = CreateGroup("pressure tank", origin, rotation, root);
        CreateCylinderY("pressure tank blackened cylinder body", Vector3.zero, length, radius, materials.DarkPipeMetal, group, stats);
        CreateSphere("pressure tank top cap", new Vector3(0f, length * 0.52f, 0f), new Vector3(radius * 1.92f, radius * 0.54f, radius * 1.92f), materials.BlackenedSteel, group, stats);
        CreateSphere("pressure tank bottom cap", new Vector3(0f, -length * 0.52f, 0f), new Vector3(radius * 1.92f, radius * 0.54f, radius * 1.92f), materials.BlackenedSteel, group, stats);
        CreateCylinderY("pressure tank upper brass band", new Vector3(0f, length * 0.28f, 0f), 0.06f, radius * 1.05f, materials.AgedBrass, group, stats);
        CreateCylinderY("pressure tank lower brass band", new Vector3(0f, -length * 0.28f, 0f), 0.06f, radius * 1.05f, materials.AgedBrass, group, stats);
        AddRivetRing("pressure tank band rivet top", new Vector3(0f, length * 0.29f, 0f), radius * 1.05f, 10, radius * 0.08f, materials.AgedBrass, group, stats);
        AddRivetRing("pressure tank band rivet bottom", new Vector3(0f, -length * 0.29f, 0f), radius * 1.05f, 10, radius * 0.08f, materials.AgedBrass, group, stats);
        stats.pressureTanks++;
    }

    private static void BuildCagedLamp(Vector3 origin, Quaternion rotation, float scale, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        Transform group = CreateGroup("caged amber lamp", origin, rotation, root);
        CreateCylinderY("lamp brass top cap", new Vector3(0f, 0.28f, 0f) * scale, 0.08f * scale, 0.19f * scale, materials.AgedBrass, group, stats);
        CreateCylinderY("lamp brass bottom cap", new Vector3(0f, -0.28f, 0f) * scale, 0.08f * scale, 0.19f * scale, materials.DarkBrass, group, stats);
        CreateSphere("lamp amber glass glow", Vector3.zero, new Vector3(0.32f, 0.52f, 0.32f) * scale, materials.LampGlow, group, stats);
        for (int i = 0; i < 6; i++)
        {
            float angle = i * Mathf.PI * 2f / 6f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.21f * scale, 0f, Mathf.Sin(angle) * 0.21f * scale);
            CreateCylinderY("lamp cage vertical rib " + i.ToString(CultureInfo.InvariantCulture), position, 0.68f * scale, 0.013f * scale, materials.BlackenedSteel, group, stats);
        }

        CreateCylinderY("lamp lower warm core", new Vector3(0f, -0.02f, 0f), 0.38f * scale, 0.095f * scale, materials.LampCore, group, stats);
        stats.lamps++;
    }

    private static void BuildSteamVent(Vector3 origin, Quaternion rotation, float scale, LookDevMaterials materials, Transform root, ViewStats stats)
    {
        Transform group = CreateGroup("steam vent", origin, rotation, root);
        CreateBox("steam vent slotted black grate", Vector3.zero, new Vector3(0.12f, 0.48f, 0.08f) * scale, Quaternion.identity, materials.BlackenedSteel, group, stats);
        for (int i = 0; i < 4; i++)
        {
            CreateBox("steam vent brass slit " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0f, -0.18f + i * 0.12f, -0.055f) * scale, new Vector3(0.14f, 0.025f, 0.016f) * scale, Quaternion.identity, materials.AgedBrass, group, stats);
        }

        BuildSteamPuff("steam vent soft puff near", origin + rotation * (new Vector3(0f, 0.14f, -0.22f) * scale), new Vector2(0.58f, 0.38f) * scale, rotation, materials.Steam, root, stats);
        BuildSteamPuff("steam vent soft puff far", origin + rotation * (new Vector3(0f, 0.32f, -0.38f) * scale), new Vector2(0.72f, 0.46f) * scale, rotation, materials.Steam, root, stats);
    }

    private static void BuildSteamPuff(string name, Vector3 position, Vector2 size, Quaternion rotation, Material material, Transform root, ViewStats stats)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = name;
        quad.transform.SetParent(root, true);
        quad.transform.position = position;
        quad.transform.rotation = rotation;
        quad.transform.localScale = new Vector3(size.x, size.y, 1f);
        AssignMaterial(quad, material);
        stats.primitiveCount++;
        stats.steamPuffs++;
    }

    private static void BuildSharedLighting(string key)
    {
        CreateLight("warm overhead brassworks key", LightType.Spot, new Vector3(-1.7f, 2.85f, -3.4f), Quaternion.Euler(58f, 20f, 0f), new Color(1f, 0.58f, 0.24f), key == "wall_kit_sheet" ? 5.2f : 6.5f, 52f, true);
        CreateLight("cool steel fill", LightType.Directional, new Vector3(1.4f, 2.2f, -1.8f), Quaternion.Euler(38f, -138f, 0f), new Color(0.14f, 0.17f, 0.22f), 0.28f, 0f, false);
        CreateLight("low wet floor rake", LightType.Spot, new Vector3(1.85f, 0.36f, -4.15f), Quaternion.Euler(78f, -24f, 0f), new Color(1f, 0.48f, 0.18f), 3.4f, 44f, false);
        CreateLight("small amber practical glow", LightType.Point, new Vector3(-1.9f, 1.6f, -1.1f), Quaternion.identity, new Color(1f, 0.48f, 0.16f), 1.8f, 0f, false);
        CreateLight("small right amber practical glow", LightType.Point, new Vector3(1.9f, 1.58f, 1.9f), Quaternion.identity, new Color(1f, 0.46f, 0.15f), 1.65f, 0f, false);

        if (key == "hero_vault_door")
        {
            CreateLight("vault door warm face lift", LightType.Spot, new Vector3(0f, 2.7f, -3.2f), Quaternion.Euler(54f, 0f, 0f), new Color(1f, 0.61f, 0.28f), 5.8f, 46f, true);
            CreateLight("vault door central amber glint", LightType.Point, new Vector3(0f, 1.35f, -1.2f), Quaternion.identity, new Color(1f, 0.44f, 0.12f), 1.4f, 0f, false);
        }
    }

    private static Light CreateLight(string name, LightType type, Vector3 position, Quaternion rotation, Color color, float intensity, float spotAngle, bool shadows)
    {
        GameObject lightObject = new GameObject(name);
        lightObject.transform.position = position;
        lightObject.transform.rotation = rotation;
        Light light = lightObject.AddComponent<Light>();
        light.type = type;
        light.color = color;
        light.intensity = intensity;
        light.range = type == LightType.Point ? 5.5f : 10f;
        if (type == LightType.Spot)
        {
            light.spotAngle = spotAngle;
            light.range = 11f;
        }

        light.shadows = shadows ? LightShadows.Soft : LightShadows.None;
        light.shadowStrength = 0.62f;
        return light;
    }

    private static Camera CreateCamera(string name, Vector3 position, Vector3 lookAt, float fieldOfView, bool orthographic, float orthographicSize)
    {
        GameObject cameraObject = new GameObject(name);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = position;
        camera.transform.LookAt(lookAt);
        camera.fieldOfView = fieldOfView;
        camera.orthographic = orthographic;
        camera.orthographicSize = orthographicSize;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 50f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.012f, 0.011f, 0.01f, 1f);
        camera.allowHDR = true;
        camera.allowMSAA = true;
        return camera;
    }

    private static Texture2D CaptureCamera(Camera camera, int width, int height)
    {
        RenderTexture previousActive = RenderTexture.active;
        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        renderTexture.name = "BrassworksCorridorLookDev_RT";
        renderTexture.antiAliasing = 4;
        Texture2D image = new Texture2D(width, height, TextureFormat.RGB24, false, false);
        try
        {
            camera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            camera.Render();
            image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            image.Apply(false, false);
            return image;
        }
        finally
        {
            camera.targetTexture = null;
            RenderTexture.active = previousActive;
            renderTexture.Release();
            UnityEngine.Object.DestroyImmediate(renderTexture);
        }
    }

    private static RenderMetric AnalyzeImage(string key, string title, string path, int width, int height, Texture2D image, Vector3 cameraPosition, Vector3 lookAt, float fieldOfView, bool orthographic, ViewStats stats)
    {
        Color32[] pixels = image.GetPixels32();
        int sampleStride = 2;
        int sampleCount = 0;
        int nearBlack = 0;
        int bright = 0;
        int warmMetal = 0;
        int lampGlow = 0;
        int magenta = 0;
        int content = 0;
        int minX = width;
        int minY = height;
        int maxX = 0;
        int maxY = 0;
        double luminanceSum = 0.0;
        float minLum = 1f;
        float maxLum = 0f;
        bool[] buckets = new bool[125];
        int bucketCount = 0;

        for (int y = 0; y < height; y += sampleStride)
        {
            for (int x = 0; x < width; x += sampleStride)
            {
                Color32 pixel = pixels[y * width + x];
                float r = pixel.r / 255f;
                float g = pixel.g / 255f;
                float b = pixel.b / 255f;
                float luminance = r * 0.2126f + g * 0.7152f + b * 0.0722f;
                luminanceSum += luminance;
                minLum = Mathf.Min(minLum, luminance);
                maxLum = Mathf.Max(maxLum, luminance);
                sampleCount++;

                if (luminance < 0.055f)
                {
                    nearBlack++;
                }

                if (luminance > 0.55f)
                {
                    bright++;
                }

                if (r > 0.18f && r > b * 1.35f && g > b * 1.12f && r >= g * 0.78f)
                {
                    warmMetal++;
                }

                if (r > 0.52f && g > 0.22f && b < 0.18f)
                {
                    lampGlow++;
                }

                if (r > 0.82f && b > 0.82f && g < 0.28f)
                {
                    magenta++;
                }

                bool isContent = luminance > 0.034f || Mathf.Abs(r - 0.012f) + Mathf.Abs(g - 0.011f) + Mathf.Abs(b - 0.01f) > 0.045f;
                if (isContent)
                {
                    content++;
                    minX = Mathf.Min(minX, x);
                    minY = Mathf.Min(minY, y);
                    maxX = Mathf.Max(maxX, x);
                    maxY = Mathf.Max(maxY, y);
                }

                int rBucket = Mathf.Clamp(Mathf.FloorToInt(r * 5f), 0, 4);
                int gBucket = Mathf.Clamp(Mathf.FloorToInt(g * 5f), 0, 4);
                int bBucket = Mathf.Clamp(Mathf.FloorToInt(b * 5f), 0, 4);
                int bucketIndex = rBucket + gBucket * 5 + bBucket * 25;
                if (!buckets[bucketIndex])
                {
                    buckets[bucketIndex] = true;
                    bucketCount++;
                }
            }
        }

        float averageLuminance = (float)(luminanceSum / Math.Max(1, sampleCount));
        float contrast = maxLum - minLum;
        float contentWidth = content > 0 ? (maxX - minX + sampleStride) / (float)width : 0f;
        float contentHeight = content > 0 ? (maxY - minY + sampleStride) / (float)height : 0f;
        float centerX = content > 0 ? (minX + maxX) * 0.5f / width : 0.5f;
        float centerY = content > 0 ? (minY + maxY) * 0.5f / height : 0.5f;
        float centerOffset = Vector2.Distance(new Vector2(centerX, centerY), new Vector2(0.5f, 0.5f));
        float contentPercent = sampleCount > 0 ? content * 100f / sampleCount : 0f;
        float nearBlackPercent = sampleCount > 0 ? nearBlack * 100f / sampleCount : 0f;
        float brightPercent = sampleCount > 0 ? bright * 100f / sampleCount : 0f;
        float warmMetalPercent = sampleCount > 0 ? warmMetal * 100f / sampleCount : 0f;
        float lampGlowPercent = sampleCount > 0 ? lampGlow * 100f / sampleCount : 0f;
        float magentaPercent = sampleCount > 0 ? magenta * 100f / sampleCount : 0f;

        bool passesNonblank = averageLuminance > 0.04f && contrast > 0.08f && contentPercent > 18f;
        bool passesNoMagenta = magentaPercent < 0.05f;
        bool passesMaterialSeparation = stats.materialRoleCount >= 10 && bucketCount >= 12 && warmMetalPercent > 0.55f && nearBlackPercent > 8f;
        bool passesFraming = contentWidth > 0.38f && contentHeight > 0.32f && contentWidth <= 1.0f && contentHeight <= 1.0f && centerOffset < 0.28f;

        return new RenderMetric
        {
            key = key,
            title = title,
            path = path,
            width = width,
            height = height,
            cameraPosition = FormatVector(cameraPosition),
            cameraLookAt = FormatVector(lookAt),
            fieldOfView = fieldOfView,
            orthographic = orthographic,
            primitiveCount = stats.primitiveCount,
            materialRoleCount = stats.materialRoleCount,
            rivets = stats.rivets,
            pipeSegments = stats.pipeSegments,
            lamps = stats.lamps,
            steamPuffs = stats.steamPuffs,
            gauges = stats.gauges,
            valves = stats.valves,
            pressureTanks = stats.pressureTanks,
            floorTiles = stats.floorTiles,
            wallPanels = stats.wallPanels,
            wetHighlights = stats.wetHighlights,
            averageLuminance = averageLuminance,
            contrast = contrast,
            contentPixelPercent = contentPercent,
            nearBlackPixelPercent = nearBlackPercent,
            brightPixelPercent = brightPercent,
            warmMetalPixelPercent = warmMetalPercent,
            lampGlowPixelPercent = lampGlowPercent,
            magentaPixelPercent = magentaPercent,
            colorBucketCount = bucketCount,
            contentWidthPercent = contentWidth * 100f,
            contentHeightPercent = contentHeight * 100f,
            centerOffsetPercent = centerOffset * 100f,
            passesNonblankCheck = passesNonblank,
            passesNoMagentaCheck = passesNoMagenta,
            passesMaterialSeparationCheck = passesMaterialSeparation,
            passesFramingCheck = passesFraming,
            status = passesNonblank && passesNoMagenta && passesMaterialSeparation && passesFraming ? "pass" : "needs review",
            notes = stats.notes
        };
    }

    private static RenderCheck[] BuildChecks(CorridorLookDevMetrics metrics)
    {
        bool nonblank = true;
        bool noMagenta = true;
        bool materialSeparation = true;
        bool framing = true;
        float maxMagenta = 0f;
        int minBuckets = int.MaxValue;
        for (int i = 0; i < metrics.renders.Length; i++)
        {
            RenderMetric render = metrics.renders[i];
            nonblank &= render.passesNonblankCheck;
            noMagenta &= render.passesNoMagentaCheck;
            materialSeparation &= render.passesMaterialSeparationCheck;
            framing &= render.passesFramingCheck;
            maxMagenta = Mathf.Max(maxMagenta, render.magentaPixelPercent);
            minBuckets = Mathf.Min(minBuckets, render.colorBucketCount);
        }

        return new RenderCheck[]
        {
            new RenderCheck
            {
                gate = "Unity-only procedural path",
                status = "pass",
                evidence = "The renderer builds temporary Unity scenes from primitive cubes, cylinders, spheres, quads, generated textures, and built-in materials. No external DCC or mesh import is used."
            },
            new RenderCheck
            {
                gate = "Required view coverage",
                status = metrics.renders.Length >= 3 ? "pass" : "fail",
                evidence = "Rendered corridor module, hero vault door module, and reusable wall-kit component sheet."
            },
            new RenderCheck
            {
                gate = "Nonblank images",
                status = nonblank ? "pass" : "fail",
                evidence = "Each required PNG must have average luminance > 0.04, contrast > 0.08, and content pixels > 18%."
            },
            new RenderCheck
            {
                gate = "No magenta/material-error pixels",
                status = noMagenta ? "pass" : "fail",
                evidence = "Maximum sampled magenta-like pixel rate is " + maxMagenta.ToString("0.000", CultureInfo.InvariantCulture) + "%; threshold is < 0.05%."
            },
            new RenderCheck
            {
                gate = "Material separation",
                status = materialSeparation ? "pass" : "fail",
                evidence = metrics.materialRoles.ToString(CultureInfo.InvariantCulture) + " procedural material roles, minimum sampled color buckets " + minBuckets.ToString(CultureInfo.InvariantCulture) + ", with warm brass/copper and dark steel/stone bins present in each render."
            },
            new RenderCheck
            {
                gate = "Reasonable framing",
                status = framing ? "pass" : "fail",
                evidence = "Content bounds are required to occupy at least 38% width and 32% height with center offset under 28%."
            },
            new RenderCheck
            {
                gate = "Production asset readiness",
                status = "partial",
                evidence = "This is lookdev only. It has no authored meshes, pivots, snap metadata, collision, UV2/lightmap proof, LODs, or prefab variants."
            },
            new RenderCheck
            {
                gate = "Human north-star review",
                status = "pending",
                evidence = "Objective render checks can catch blank/shader/framing failures, but final visual acceptance still requires human comparison against the north-star concept art."
            }
        };
    }

    private static string DetermineOverallStatus(RenderCheck[] checks)
    {
        for (int i = 0; i < checks.Length; i++)
        {
            if (checks[i].status == "fail")
            {
                return "Technical fail: at least one automated render gate failed.";
            }
        }

        return "Technical pass for Unity-only lookdev render generation; production art and human north-star review remain pending.";
    }

    private static Texture2D BuildContactSheet(string projectRoot, List<RenderResult> results)
    {
        Texture2D sheet = new Texture2D(ContactWidth, ContactHeight, TextureFormat.RGB24, false, false);
        FillTexture(sheet, new Color(0.018f, 0.016f, 0.014f, 1f));
        RectInt[] slots =
        {
            new RectInt(70, 600, 1080, 820),
            new RectInt(1250, 600, 1080, 820),
            new RectInt(420, 65, 1560, 455)
        };

        List<Texture2D> loadedRenders = new List<Texture2D>();
        try
        {
            for (int i = 0; i < results.Count && i < slots.Length; i++)
            {
                string absolutePath = Path.Combine(projectRoot, results[i].RelativePath);
                Texture2D source = LoadPngTexture(absolutePath, "ContactSheetSource_" + results[i].Key);
                loadedRenders.Add(source);
                RectInt fit = FitRect(source.width, source.height, slots[i]);
                DrawRect(sheet, ExpandRect(fit, 8), new Color(0.62f, 0.38f, 0.13f, 1f), 5);
                BlitScaled(source, sheet, fit);
            }

            sheet.Apply(false, false);
            return sheet;
        }
        finally
        {
            for (int i = 0; i < loadedRenders.Count; i++)
            {
                DestroyTexture(loadedRenders[i]);
            }
        }
    }

    private static Texture2D LoadPngTexture(string absolutePath, string name)
    {
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGB24, false, false);
        texture.name = name;
        if (!texture.LoadImage(File.ReadAllBytes(absolutePath), false))
        {
            UnityEngine.Object.DestroyImmediate(texture);
            throw new InvalidOperationException("Failed to load render PNG for contact sheet: " + absolutePath);
        }

        texture.hideFlags = HideFlags.DontUnloadUnusedAsset;
        return texture;
    }

    private static string BuildReport(CorridorLookDevMetrics metrics)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# Brassworks Corridor Unity LookDev Report");
        builder.AppendLine();
        builder.AppendLine("Status: " + metrics.overallStatus);
        builder.AppendLine("Date/time: " + metrics.timestamp);
        builder.AppendLine("Unity version: " + metrics.unityVersion);
        builder.AppendLine("Batchmode entrypoint: `" + metrics.batchmodeEntrypoint + "`");
        builder.AppendLine("Unity command prepared: `" + metrics.exactUnityCommand + "`");
        builder.AppendLine("Tool lane: " + metrics.toolPolicy);
        builder.AppendLine("Write scope: " + metrics.ownedWriteScope);
        builder.AppendLine();
        builder.AppendLine("## Outputs");
        builder.AppendLine();
        builder.AppendLine("| File | Purpose | Dimensions |");
        builder.AppendLine("| --- | --- | ---: |");
        for (int i = 0; i < metrics.renders.Length; i++)
        {
            RenderMetric render = metrics.renders[i];
            builder.AppendLine("| `" + render.path + "` | " + render.title + " | " + render.width.ToString(CultureInfo.InvariantCulture) + "x" + render.height.ToString(CultureInfo.InvariantCulture) + " |");
        }

        builder.AppendLine("| `" + metrics.contactSheetPath + "` | Contact sheet for the three Brassworks corridor lookdev renders | " + ContactWidth.ToString(CultureInfo.InvariantCulture) + "x" + ContactHeight.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| `" + metrics.metricsPath + "` | Machine-readable metrics and gates | n/a |");
        builder.AppendLine();
        builder.AppendLine("## Components Built");
        builder.AppendLine();
        builder.AppendLine("- Corridor module: wet tiled floor, soot-brick side walls, blackened iron ribs, brass hand rails, overhead and wall pipe runs, caged amber lamps, pressure tanks, gauges, valves, steam vents, oil smears, and broken warm reflections.");
        builder.AppendLine("- Hero vault door module: layered round pressure door, brass outer seal, blackened steel slab, radial braces, rivet rings, central valve wheel, side gauges/valves, tanks, lamps, pipes, and wet foreground.");
        builder.AppendLine("- Reusable wall-kit sheet: pipe cluster, riveted panel, caged lamp, valve wheel, pressure tank, and wet floor tile isolated as future modular asset targets.");
        builder.AppendLine();
        builder.AppendLine("## Material Approach");
        builder.AppendLine();
        builder.AppendLine("All lookdev materials are generated in the editor script. The pass uses built-in Unity `Standard`, `Unlit/Color`, and `Unlit/Transparent` shaders with procedural grime/scratch textures, generated normal-like noise, metalness/smoothness settings, transparent wet highlights, soft steam billboards, and warm unlit lamp cores. No Blender, OBJ/FBX import, authored mesh asset, or external texture package is used by this renderer.");
        builder.AppendLine();
        builder.AppendLine("Material roles: wet oil-dark stone, soot brick, blackened steel, dark pressure metal, aged brass, dark brass, copper, cream enamel gauge face, amber lamp glass, lamp core glow, wet reflection, oil film, steam, warning red, line dark, iron edge, and dark inspection backplate.");
        builder.AppendLine();
        builder.AppendLine("## Count Metrics");
        builder.AppendLine();
        builder.AppendLine("| Metric | Count |");
        builder.AppendLine("| --- | ---: |");
        builder.AppendLine("| Procedural material roles | " + metrics.materialRoles.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Primitive objects | " + metrics.totalPrimitiveCount.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Rivets/bolts | " + metrics.totalRivets.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Pipe segments | " + metrics.totalPipeSegments.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Lamps | " + metrics.totalLamps.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Steam puffs | " + metrics.totalSteamPuffs.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Gauges | " + metrics.totalGauges.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Valves | " + metrics.totalValves.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Pressure tanks | " + metrics.totalPressureTanks.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Floor tiles | " + metrics.totalFloorTiles.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Wall panels | " + metrics.totalWallPanels.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine("| Wet/oil highlight patches | " + metrics.totalWetHighlights.ToString(CultureInfo.InvariantCulture) + " |");
        builder.AppendLine();
        builder.AppendLine("## Automated Render Checks");
        builder.AppendLine();
        builder.AppendLine("| Image | Avg luminance | Contrast | Content % | Near-black % | Warm metal % | Magenta % | Buckets | Nonblank | No magenta | Material sep. | Framing |");
        builder.AppendLine("| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- | --- | --- | --- |");
        for (int i = 0; i < metrics.renders.Length; i++)
        {
            RenderMetric render = metrics.renders[i];
            builder.AppendLine("| `" + render.path + "` | " +
                               render.averageLuminance.ToString("0.000", CultureInfo.InvariantCulture) + " | " +
                               render.contrast.ToString("0.000", CultureInfo.InvariantCulture) + " | " +
                               render.contentPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + " | " +
                               render.nearBlackPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + " | " +
                               render.warmMetalPixelPercent.ToString("0.0", CultureInfo.InvariantCulture) + " | " +
                               render.magentaPixelPercent.ToString("0.000", CultureInfo.InvariantCulture) + " | " +
                               render.colorBucketCount.ToString(CultureInfo.InvariantCulture) + " | " +
                               PassFail(render.passesNonblankCheck) + " | " +
                               PassFail(render.passesNoMagentaCheck) + " | " +
                               PassFail(render.passesMaterialSeparationCheck) + " | " +
                               PassFail(render.passesFramingCheck) + " |");
        }

        builder.AppendLine();
        builder.AppendLine("## Gates");
        builder.AppendLine();
        builder.AppendLine("| Gate | Status | Evidence |");
        builder.AppendLine("| --- | --- | --- |");
        for (int i = 0; i < metrics.checks.Length; i++)
        {
            builder.AppendLine("| " + metrics.checks[i].gate + " | " + metrics.checks[i].status + " | " + metrics.checks[i].evidence + " |");
        }

        builder.AppendLine();
        builder.AppendLine("## Acceptance Criteria");
        builder.AppendLine();
        builder.AppendLine("- Three required PNG views exist under `Documentation/ConceptRenders/BrassworksCorridor`.");
        builder.AppendLine("- Each view is generated by Unity batch rendering from in-memory procedural primitives/materials only.");
        builder.AppendLine("- Corridor view clearly includes pipes, lamps, steam vents, and a wet reflective floor.");
        builder.AppendLine("- Vault-door view clearly includes a heavy round pressure door with brass/steel layering, rivets, valve hardware, tanks, and warm practical light.");
        builder.AppendLine("- Component sheet shows the reusable kit vocabulary: pipe clusters, riveted panels, lamp, valve wheel, pressure tank, and floor tile.");
        builder.AppendLine("- Automated render checks record nonblank image, no sampled magenta shader-error pixels, material separation, and framing.");
        builder.AppendLine();
        builder.AppendLine("## Honest Gaps Versus North-Star");
        builder.AppendLine();
        builder.AppendLine("This pass is a stronger Unity-only direction proof, not final environment art. Primitive cylinders and boxes cannot match the north-star image's authored bevels, sculpted seams, true grime placement, dense occlusion, parallax, volumetric steam, screen-space wet reflections, or material microdetail. The steam is intentionally restrained to avoid opaque billboard slabs. The vault door and kit parts still need real modular meshes, pivots, snap rules, colliders, UVs, lightmap/LOD proof, prefab variants, decal placement, and in-level gameplay validation before promotion.");
        return builder.ToString();
    }

    private static string PassFail(bool value)
    {
        return value ? "pass" : "fail";
    }

    private static Transform CreateGroup(string name, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject group = new GameObject(name);
        group.transform.SetParent(parent, true);
        group.transform.position = position;
        group.transform.rotation = rotation;
        return group.transform;
    }

    private static GameObject CreateCylinderX(string name, Vector3 position, float length, float radius, Material material, Transform parent, ViewStats stats)
    {
        return CreateCylinder(name, position, Quaternion.Euler(0f, 0f, 90f), length, radius, material, parent, stats);
    }

    private static GameObject CreateCylinderY(string name, Vector3 position, float length, float radius, Material material, Transform parent, ViewStats stats)
    {
        return CreateCylinder(name, position, Quaternion.identity, length, radius, material, parent, stats);
    }

    private static GameObject CreateCylinderZ(string name, Vector3 position, float length, float radius, Material material, Transform parent, ViewStats stats)
    {
        return CreateCylinder(name, position, Quaternion.Euler(90f, 0f, 0f), length, radius, material, parent, stats);
    }

    private static GameObject CreateCylinder(string name, Vector3 position, Quaternion rotation, float length, float radius, Material material, Transform parent, ViewStats stats)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, true);
        gameObject.transform.localPosition = position;
        gameObject.transform.localRotation = rotation;
        gameObject.transform.localScale = new Vector3(radius * 2f, length * 0.5f, radius * 2f);
        AssignMaterial(gameObject, material);
        stats.primitiveCount++;
        return gameObject;
    }

    private static GameObject CreateCylinderBetween(string name, Vector3 start, Vector3 end, float radius, Material material, Transform parent, ViewStats stats)
    {
        Vector3 midpoint = (start + end) * 0.5f;
        Vector3 direction = end - start;
        float length = direction.magnitude;
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = midpoint;
        gameObject.transform.localRotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
        gameObject.transform.localScale = new Vector3(radius * 2f, length * 0.5f, radius * 2f);
        AssignMaterial(gameObject, material);
        stats.primitiveCount++;
        return gameObject;
    }

    private static GameObject CreateBox(string name, Vector3 position, Vector3 scale, Quaternion rotation, Material material, Transform parent, ViewStats stats)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, true);
        gameObject.transform.localPosition = position;
        gameObject.transform.localRotation = rotation;
        gameObject.transform.localScale = scale;
        AssignMaterial(gameObject, material);
        stats.primitiveCount++;
        return gameObject;
    }

    private static GameObject CreateSphere(string name, Vector3 position, Vector3 scale, Material material, Transform parent, ViewStats stats)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.name = name;
        gameObject.transform.SetParent(parent, true);
        gameObject.transform.localPosition = position;
        gameObject.transform.localScale = scale;
        AssignMaterial(gameObject, material);
        stats.primitiveCount++;
        return gameObject;
    }

    private static void CreateGroundQuad(string name, Vector3 position, Vector2 size, Quaternion rotation, Material material, Transform parent, ViewStats stats)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = name;
        quad.transform.SetParent(parent, true);
        quad.transform.localPosition = position;
        quad.transform.localRotation = Quaternion.Euler(90f, 0f, 0f) * rotation;
        quad.transform.localScale = new Vector3(size.x, size.y, 1f);
        AssignMaterial(quad, material);
        stats.primitiveCount++;
    }

    private static void AddRivetRow(string prefix, Vector3 start, Vector3 end, int count, float radius, Material material, Transform parent, ViewStats stats)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            CreateSphere(prefix + " " + i.ToString(CultureInfo.InvariantCulture), Vector3.Lerp(start, end, t), Vector3.one * (radius * 2f), material, parent, stats);
            stats.rivets++;
        }
    }

    private static void AddRivetGrid(string prefix, Vector3 origin, int columns, int rows, float dx, float dy, float radius, Material material, Transform parent, ViewStats stats, Vector3 zDirection, float dz)
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 position = origin + new Vector3(x * dx, y * dy, 0f);
                if (zDirection != Vector3.zero && columns > 1)
                {
                    position += zDirection.normalized * (x / (float)(columns - 1)) * dz;
                }

                CreateSphere(prefix + " " + x.ToString(CultureInfo.InvariantCulture) + "_" + y.ToString(CultureInfo.InvariantCulture), position, Vector3.one * (radius * 2f), material, parent, stats);
                stats.rivets++;
            }
        }
    }

    private static void AddRivetRing(string prefix, Vector3 center, float radius, int count, float rivetRadius, Material material, Transform parent, ViewStats stats)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 position = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            CreateSphere(prefix + " " + i.ToString(CultureInfo.InvariantCulture), position, Vector3.one * (rivetRadius * 2f), material, parent, stats);
            stats.rivets++;
        }
    }

    private static void CreateSmallStencilLabel(string text, Vector3 position, float characterSize, Transform parent)
    {
        GameObject labelObject = new GameObject("stencil label " + text);
        labelObject.transform.SetParent(parent, true);
        labelObject.transform.localPosition = position;
        TextMesh textMesh = labelObject.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = TextAnchor.MiddleLeft;
        textMesh.alignment = TextAlignment.Left;
        textMesh.characterSize = characterSize;
        textMesh.fontSize = 64;
        textMesh.color = new Color(0.74f, 0.59f, 0.36f, 1f);
    }

    private static void AssignMaterial(GameObject gameObject, Material material)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }
    }

    private static void FillTexture(Texture2D texture, Color color)
    {
        Color32 fill = color;
        Color32[] pixels = new Color32[texture.width * texture.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = fill;
        }

        texture.SetPixels32(pixels);
    }

    private static void DrawRect(Texture2D texture, RectInt rect, Color color, int thickness)
    {
        FillRect(texture, new RectInt(rect.xMin, rect.yMin, rect.width, thickness), color);
        FillRect(texture, new RectInt(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        FillRect(texture, new RectInt(rect.xMin, rect.yMin, thickness, rect.height), color);
        FillRect(texture, new RectInt(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }

    private static void FillRect(Texture2D texture, RectInt rect, Color color)
    {
        int xMin = Mathf.Clamp(rect.xMin, 0, texture.width);
        int xMax = Mathf.Clamp(rect.xMax, 0, texture.width);
        int yMin = Mathf.Clamp(rect.yMin, 0, texture.height);
        int yMax = Mathf.Clamp(rect.yMax, 0, texture.height);
        for (int y = yMin; y < yMax; y++)
        {
            for (int x = xMin; x < xMax; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
    }

    private static RectInt FitRect(int sourceWidth, int sourceHeight, RectInt bounds)
    {
        float scale = Mathf.Min(bounds.width / (float)sourceWidth, bounds.height / (float)sourceHeight);
        int width = Mathf.RoundToInt(sourceWidth * scale);
        int height = Mathf.RoundToInt(sourceHeight * scale);
        return new RectInt(bounds.xMin + (bounds.width - width) / 2, bounds.yMin + (bounds.height - height) / 2, width, height);
    }

    private static RectInt ExpandRect(RectInt rect, int amount)
    {
        return new RectInt(rect.xMin - amount, rect.yMin - amount, rect.width + amount * 2, rect.height + amount * 2);
    }

    private static void BlitScaled(Texture2D source, Texture2D destination, RectInt target)
    {
        for (int y = 0; y < target.height; y++)
        {
            float v = y / Mathf.Max(1f, target.height - 1f);
            int sourceY = Mathf.Clamp(Mathf.RoundToInt(v * (source.height - 1)), 0, source.height - 1);
            for (int x = 0; x < target.width; x++)
            {
                float u = x / Mathf.Max(1f, target.width - 1f);
                int sourceX = Mathf.Clamp(Mathf.RoundToInt(u * (source.width - 1)), 0, source.width - 1);
                destination.SetPixel(target.xMin + x, target.yMin + y, source.GetPixel(sourceX, sourceY));
            }
        }
    }

    private static void DestroyTexture(Texture2D texture)
    {
        if (texture != null)
        {
            UnityEngine.Object.DestroyImmediate(texture);
        }
    }

    private static string GetProjectRoot()
    {
        return Directory.GetParent(Application.dataPath).FullName;
    }

    private static string FormatVector(Vector3 vector)
    {
        return vector.x.ToString("0.00", CultureInfo.InvariantCulture) + ", " +
               vector.y.ToString("0.00", CultureInfo.InvariantCulture) + ", " +
               vector.z.ToString("0.00", CultureInfo.InvariantCulture);
    }

    private sealed class LookDevMaterials
    {
        public const int MaterialRoleCount = 17;

        private readonly List<Material> ownedMaterials = new List<Material>();
        private readonly List<Texture> ownedTextures = new List<Texture>();

        public Material WetStone;
        public Material SootBrick;
        public Material BlackenedSteel;
        public Material DarkPipeMetal;
        public Material AgedBrass;
        public Material DarkBrass;
        public Material Copper;
        public Material GaugeFace;
        public Material LampGlow;
        public Material LampCore;
        public Material WetHighlight;
        public Material WetHighlightSolid;
        public Material OilFilm;
        public Material Steam;
        public Material WarningRed;
        public Material LineDark;
        public Material IronEdge;
        public Material ContactBack;

        public static LookDevMaterials Create()
        {
            LookDevMaterials materials = new LookDevMaterials();
            materials.WetStone = materials.CreateLitMaterial("BBWLC_WetOilDarkStone", new Color(0.045f, 0.044f, 0.039f, 1f), 0.02f, 0.88f, 1101, 0.55f, true);
            materials.SootBrick = materials.CreateLitMaterial("BBWLC_SootBrick", new Color(0.082f, 0.067f, 0.052f, 1f), 0f, 0.42f, 1202, 0.72f, true);
            materials.BlackenedSteel = materials.CreateLitMaterial("BBWLC_BlackenedSteel", new Color(0.095f, 0.087f, 0.077f, 1f), 0.86f, 0.38f, 1303, 0.62f, true);
            materials.DarkPipeMetal = materials.CreateLitMaterial("BBWLC_DarkPressureMetal", new Color(0.066f, 0.064f, 0.062f, 1f), 0.9f, 0.31f, 1404, 0.5f, true);
            materials.AgedBrass = materials.CreateLitMaterial("BBWLC_AgedBrass", new Color(0.72f, 0.44f, 0.16f, 1f), 0.88f, 0.58f, 1505, 0.44f, true);
            materials.DarkBrass = materials.CreateLitMaterial("BBWLC_DarkAgedBrass", new Color(0.36f, 0.21f, 0.08f, 1f), 0.88f, 0.45f, 1606, 0.38f, true);
            materials.Copper = materials.CreateLitMaterial("BBWLC_CopperPipe", new Color(0.73f, 0.28f, 0.105f, 1f), 0.82f, 0.54f, 1707, 0.42f, true);
            materials.GaugeFace = materials.CreateLitMaterial("BBWLC_CreamEnamelGauge", new Color(0.78f, 0.67f, 0.49f, 1f), 0.0f, 0.5f, 1808, 0.22f, false);
            materials.WarningRed = materials.CreateLitMaterial("BBWLC_WarningNeedleRed", new Color(0.72f, 0.045f, 0.025f, 1f), 0.02f, 0.42f, 1909, 0.1f, false);
            materials.IronEdge = materials.CreateLitMaterial("BBWLC_WornIronEdge", new Color(0.32f, 0.30f, 0.27f, 1f), 0.86f, 0.48f, 2001, 0.32f, false);
            materials.LineDark = materials.CreateUnlitColorMaterial("BBWLC_DarkGaugeInk", new Color(0.018f, 0.013f, 0.009f, 1f), false);
            materials.ContactBack = materials.CreateUnlitColorMaterial("BBWLC_InspectionBackplate", new Color(0.046f, 0.037f, 0.029f, 1f), false);
            materials.LampGlow = materials.CreateTransparentLitMaterial("BBWLC_AmberLampGlass", new Color(1f, 0.54f, 0.18f, 0.48f), 0f, 0.82f, true);
            materials.LampCore = materials.CreateUnlitColorMaterial("BBWLC_LampCoreGlow", new Color(1f, 0.38f, 0.07f, 1f), true);
            materials.WetHighlight = materials.CreateTransparentLitMaterial("BBWLC_BrokenWetReflection", new Color(1f, 0.56f, 0.16f, 0.22f), 0f, 0.96f, false);
            materials.WetHighlightSolid = materials.CreateLitMaterial("BBWLC_SolidWetReflection", new Color(0.92f, 0.48f, 0.14f, 1f), 0.02f, 0.92f, 2102, 0.12f, false);
            materials.OilFilm = materials.CreateTransparentTextureMaterial("BBWLC_OilFilm", CreateOilFilmTexture(256, 2203));
            materials.Steam = materials.CreateTransparentTextureMaterial("BBWLC_SoftSteam", CreateSteamTexture(256, 2304));
            return materials;
        }

        public void DestroyAll()
        {
            for (int i = 0; i < ownedMaterials.Count; i++)
            {
                if (ownedMaterials[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(ownedMaterials[i]);
                }
            }

            for (int i = 0; i < ownedTextures.Count; i++)
            {
                if (ownedTextures[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(ownedTextures[i]);
                }
            }

            ownedMaterials.Clear();
            ownedTextures.Clear();
        }

        private Material CreateLitMaterial(string name, Color baseColor, float metallic, float smoothness, int seed, float grime, bool normalMap)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
            {
                shader = Shader.Find("Diffuse");
            }

            if (shader == null)
            {
                shader = Shader.Find("Unlit/Color");
            }

            Material material = new Material(shader);
            material.name = name;
            Texture2D surface = CreateProceduralSurfaceTexture(baseColor, seed, grime);
            SetMaterialColor(material, Color.white);
            SetMaterialTexture(material, "_MainTex", surface);
            material.mainTexture = surface;
            SetMaterialFloat(material, "_Metallic", metallic);
            SetMaterialFloat(material, "_Glossiness", smoothness);
            SetMaterialFloat(material, "_Smoothness", smoothness);
            TrackTexture(surface);

            if (normalMap)
            {
                Texture2D normal = CreateProceduralNormalTexture(seed + 71);
                SetMaterialTexture(material, "_BumpMap", normal);
                SetMaterialFloat(material, "_BumpScale", 0.42f);
                material.EnableKeyword("_NORMALMAP");
                TrackTexture(normal);
            }

            ownedMaterials.Add(material);
            return material;
        }

        private Material CreateTransparentLitMaterial(string name, Color color, float metallic, float smoothness, bool emission)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            Material material = new Material(shader);
            material.name = name;
            SetMaterialColor(material, color);
            SetMaterialFloat(material, "_Metallic", metallic);
            SetMaterialFloat(material, "_Glossiness", smoothness);
            SetMaterialFloat(material, "_Smoothness", smoothness);
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = (int)RenderQueue.Transparent + 20;
            SetMaterialFloat(material, "_Mode", 3f);
            SetMaterialInt(material, "_SrcBlend", (int)BlendMode.SrcAlpha);
            SetMaterialInt(material, "_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            SetMaterialInt(material, "_ZWrite", 0);
            SetMaterialInt(material, "_Cull", (int)CullMode.Off);
            material.EnableKeyword("_ALPHABLEND_ON");
            if (emission)
            {
                SetMaterialColor(material, "_EmissionColor", new Color(1f, 0.36f, 0.08f, 1f) * 1.45f);
                material.EnableKeyword("_EMISSION");
            }

            ownedMaterials.Add(material);
            return material;
        }

        private Material CreateTransparentTextureMaterial(string name, Texture2D texture)
        {
            Shader shader = Shader.Find("Unlit/Transparent");
            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            Material material = new Material(shader);
            material.name = name;
            SetMaterialTexture(material, "_MainTex", texture);
            SetMaterialTexture(material, "_BaseMap", texture);
            SetMaterialColor(material, Color.white);
            SetMaterialInt(material, "_Cull", (int)CullMode.Off);
            SetMaterialInt(material, "_ZWrite", 0);
            material.renderQueue = (int)RenderQueue.Transparent + 40;
            TrackTexture(texture);
            ownedMaterials.Add(material);
            return material;
        }

        private Material CreateUnlitColorMaterial(string name, Color color, bool emission)
        {
            Shader shader = Shader.Find("Unlit/Color");
            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            Material material = new Material(shader);
            material.name = name;
            SetMaterialColor(material, color);
            material.color = color;
            if (emission)
            {
                SetMaterialColor(material, "_EmissionColor", color * 1.8f);
                material.EnableKeyword("_EMISSION");
            }

            ownedMaterials.Add(material);
            return material;
        }

        private void TrackTexture(Texture texture)
        {
            if (texture != null && !ownedTextures.Contains(texture))
            {
                ownedTextures.Add(texture);
            }
        }
    }

    private static Texture2D CreateProceduralSurfaceTexture(Color baseColor, int seed, float grime)
    {
        int size = 256;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, true, false);
        texture.name = "BBWLC_Surface_" + seed.ToString(CultureInfo.InvariantCulture);
        System.Random random = new System.Random(seed);
        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float largeNoise = Mathf.PerlinNoise((x + seed) * 0.026f, (y - seed) * 0.031f);
                float fineNoise = Mathf.PerlinNoise((x - seed) * 0.11f, (y + seed) * 0.09f);
                float seamDark = (x % 64 < 3 || y % 64 < 3) ? -0.12f : 0f;
                float scratch = random.NextDouble() > 0.994 ? 0.28f : 0f;
                float tarnish = Mathf.Lerp(-grime * 0.24f, grime * 0.18f, largeNoise) + (fineNoise - 0.5f) * grime * 0.22f + seamDark + scratch;
                float multiplier = Mathf.Clamp(0.86f + tarnish, 0.24f, 1.35f);
                pixels[y * size + x] = new Color(
                    Mathf.Clamp01(baseColor.r * multiplier),
                    Mathf.Clamp01(baseColor.g * multiplier),
                    Mathf.Clamp01(baseColor.b * multiplier),
                    baseColor.a);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(true, false);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.filterMode = FilterMode.Trilinear;
        texture.anisoLevel = 4;
        return texture;
    }

    private static Texture2D CreateProceduralNormalTexture(int seed)
    {
        int size = 128;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, true, true);
        texture.name = "BBWLC_Normal_" + seed.ToString(CultureInfo.InvariantCulture);
        Color[] pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float nx = (Mathf.PerlinNoise((x + seed) * 0.08f, y * 0.055f) - 0.5f) * 0.16f;
                float ny = (Mathf.PerlinNoise(x * 0.055f, (y - seed) * 0.08f) - 0.5f) * 0.16f;
                pixels[y * size + x] = new Color(0.5f + nx, 0.5f + ny, 1f, 1f);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(true, false);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.filterMode = FilterMode.Trilinear;
        texture.anisoLevel = 4;
        return texture;
    }

    private static Texture2D CreateOilFilmTexture(int size, int seed)
    {
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false, false);
        texture.name = "BBWLC_OilFilm_" + seed.ToString(CultureInfo.InvariantCulture);
        Color[] pixels = new Color[size * size];
        Vector2 center = new Vector2(size * 0.5f, size * 0.5f);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center) / (size * 0.55f);
                float noise = Mathf.PerlinNoise((x + seed) * 0.05f, (y - seed) * 0.07f);
                float alpha = Mathf.Clamp01((1f - distance) * 1.35f) * Mathf.Lerp(0.14f, 0.38f, noise);
                pixels[y * size + x] = new Color(0.035f, 0.028f, 0.018f, alpha);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(false, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        return texture;
    }

    private static Texture2D CreateSteamTexture(int size, int seed)
    {
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false, false);
        texture.name = "BBWLC_Steam_" + seed.ToString(CultureInfo.InvariantCulture);
        Color[] pixels = new Color[size * size];
        Vector2 center = new Vector2(size * 0.5f, size * 0.5f);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center) / (size * 0.5f);
                float noise = Mathf.PerlinNoise((x + seed) * 0.035f, (y - seed) * 0.04f);
                float alpha = Mathf.Clamp01((1f - distance) * 1.15f) * Mathf.Lerp(0.08f, 0.24f, noise);
                pixels[y * size + x] = new Color(0.72f, 0.70f, 0.66f, alpha);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply(false, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        return texture;
    }

    private static void SetMaterialColor(Material material, Color color)
    {
        if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", color);
        }

        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }

        material.color = color;
    }

    private static void SetMaterialColor(Material material, string property, Color color)
    {
        if (material.HasProperty(property))
        {
            material.SetColor(property, color);
        }
    }

    private static void SetMaterialTexture(Material material, string property, Texture texture)
    {
        if (texture != null && material.HasProperty(property))
        {
            material.SetTexture(property, texture);
        }
    }

    private static void SetMaterialFloat(Material material, string property, float value)
    {
        if (material.HasProperty(property))
        {
            material.SetFloat(property, value);
        }
    }

    private static void SetMaterialInt(Material material, string property, int value)
    {
        if (material.HasProperty(property))
        {
            material.SetInt(property, value);
        }
    }

    [Serializable]
    private sealed class CorridorLookDevMetrics
    {
        public string timestamp;
        public string unityVersion;
        public string batchmodeEntrypoint;
        public string exactUnityCommand;
        public string renderFolder;
        public string reportPath;
        public string metricsPath;
        public string contactSheetPath;
        public string corridorRenderPath;
        public string vaultRenderPath;
        public string kitRenderPath;
        public string toolPolicy;
        public string ownedWriteScope;
        public string northStarTarget;
        public string overallStatus;
        public int materialRoles;
        public int totalPrimitiveCount;
        public int totalRivets;
        public int totalPipeSegments;
        public int totalLamps;
        public int totalSteamPuffs;
        public int totalGauges;
        public int totalValves;
        public int totalPressureTanks;
        public int totalFloorTiles;
        public int totalWallPanels;
        public int totalWetHighlights;
        public RenderMetric[] renders;
        public RenderCheck[] checks;
    }

    [Serializable]
    private sealed class RenderMetric
    {
        public string key;
        public string title;
        public string path;
        public int width;
        public int height;
        public string cameraPosition;
        public string cameraLookAt;
        public float fieldOfView;
        public bool orthographic;
        public int primitiveCount;
        public int materialRoleCount;
        public int rivets;
        public int pipeSegments;
        public int lamps;
        public int steamPuffs;
        public int gauges;
        public int valves;
        public int pressureTanks;
        public int floorTiles;
        public int wallPanels;
        public int wetHighlights;
        public float averageLuminance;
        public float contrast;
        public float contentPixelPercent;
        public float nearBlackPixelPercent;
        public float brightPixelPercent;
        public float warmMetalPixelPercent;
        public float lampGlowPixelPercent;
        public float magentaPixelPercent;
        public int colorBucketCount;
        public float contentWidthPercent;
        public float contentHeightPercent;
        public float centerOffsetPercent;
        public bool passesNonblankCheck;
        public bool passesNoMagentaCheck;
        public bool passesMaterialSeparationCheck;
        public bool passesFramingCheck;
        public string status;
        public string notes;
    }

    [Serializable]
    private sealed class RenderCheck
    {
        public string gate;
        public string status;
        public string evidence;
    }

    private sealed class ViewStats
    {
        public int primitiveCount;
        public int rivets;
        public int pipeSegments;
        public int lamps;
        public int steamPuffs;
        public int gauges;
        public int valves;
        public int pressureTanks;
        public int floorTiles;
        public int wallPanels;
        public int wetHighlights;
        public int materialRoleCount = LookDevMaterials.MaterialRoleCount;
        public string notes;
    }

    private sealed class RenderResult
    {
        public string Key;
        public string Title;
        public string RelativePath;
        public Texture2D Texture;
        public RenderMetric Metric;
        public ViewStats Stats;
    }
}
