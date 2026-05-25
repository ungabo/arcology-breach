using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class CorridorAssemblyLookdev10Renderer
{
    private const int RenderWidth = 1920;
    private const int RenderHeight = 1080;
    private const int ContactWidth = 2400;
    private const int ContactHeight = 1500;

    private const string RepoRoot = @"D:\__MY APPS\Unity Doom";
    private const string OutputFolder = @"D:\__MY APPS\Unity Doom\Documentation\ConceptRenders\V0_1_55_CorridorAssemblyLookdev10";
    private const string AssetProductionFolder = @"D:\__MY APPS\Unity Doom\Documentation\AssetProduction\V0_1_55_CorridorAssemblyLookdev10";
    private const string QaFolder = @"D:\__MY APPS\Unity Doom\Documentation\QA\V0_1_55_CorridorAssemblyLookdev10";

    private const string RoomShellPackage = "Packages/com.brassworks.sidecar.room-shell-set07";
    private const string HeroRoomPackage = "Packages/com.brassworks.sidecar.hero-room-render-set07";
    private const string SteamDressingPackage = "Packages/com.brassworks.sidecar.steam-corridor-dressing-set09";
    private const string GaslightPackage = "Packages/com.brassworks.sidecar.gaslight-pipe-dressing-set10";
    private const string RoomMaterialPackage = "Packages/com.brassworks.sidecar.room-material-set10";

    [MenuItem("CAML10/Render Corridor Assembly Lookdev")]
    public static void RenderFromMenu()
    {
        RenderBatch();
    }

    public static void RenderBatch()
    {
        try
        {
            RenderAll();
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

    private static void RenderAll()
    {
        Directory.CreateDirectory(OutputFolder);
        Directory.CreateDirectory(AssetProductionFolder);
        Directory.CreateDirectory(QaFolder);

        LookdevRun run = new LookdevRun
        {
            timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture),
            unityVersion = Application.unityVersion,
            northStarReference = "Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png",
            sourcePolicy = "Read-only local sidecar package references plus Unity procedural staging. No main scenes, game build files, shared scripts, Blender, or external DCC.",
            writeScope = "Documentation/AssetProduction/V0_1_55_CorridorAssemblyLookdev10, Documentation/ConceptRenders/V0_1_55_CorridorAssemblyLookdev10, Documentation/QA/V0_1_55_CorridorAssemblyLookdev10."
        };

        List<ShotRecord> records = new List<ShotRecord>();
        records.Add(RenderShot("CAML10_RENDER_01_corridor_assembly_hero.png", "Accepted-family corridor hero", BuildAcceptedFamilyHero, new Vector3(0f, 1.18f, -6.6f), new Vector3(0f, 1.27f, 5.75f), 54f, run));
        records.Add(RenderShot("CAML10_RENDER_02_corner_service_density.png", "Corner / service density assembly", BuildCornerDensityShot, new Vector3(-5.0f, 2.65f, -5.4f), new Vector3(0.72f, 1.12f, 1.88f), 46f, run));
        records.Add(RenderShot("CAML10_RENDER_03_material_fixture_detail.png", "Material and fixture close review", BuildMaterialFixtureDetail, new Vector3(1.42f, 1.08f, -2.9f), new Vector3(-2.08f, 1.05f, 0.85f), 42f, run));

        string contactPath = Path.Combine(OutputFolder, "CAML10_CONTACTSHEET_corridor_assembly.png");
        Texture2D contact = BuildContactSheet(records);
        File.WriteAllBytes(contactPath, contact.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(contact);

        run.shots = records.ToArray();
        run.contactSheet = AbsoluteToRepoRelative(contactPath);

        File.WriteAllText(Path.Combine(AssetProductionFolder, "CAML10_RenderManifest.json"), BuildManifestJson(run));
        File.WriteAllText(Path.Combine(AssetProductionFolder, "CAML10_AssetAssemblyNotes.md"), BuildAssemblyNotes(run));
        File.WriteAllText(Path.Combine(QaFolder, "CAML10_BluntNorthStarComparison.md"), BuildQaReport(run));

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        AssetDatabase.Refresh();
    }

    private static ShotRecord RenderShot(string fileName, string title, SceneBuilder builder, Vector3 cameraPosition, Vector3 lookAt, float fieldOfView, LookdevRun run)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        ConfigureRenderSettings();

        AssemblyMaterials materials = AssemblyMaterials.Load();
        SceneStats stats = new SceneStats();
        GameObject root = new GameObject("CAML10_" + Path.GetFileNameWithoutExtension(fileName));

        builder(root.transform, materials, stats);
        BuildSharedLighting(stats);

        Camera camera = CreateCamera(title + " Camera", cameraPosition, lookAt, fieldOfView);
        Texture2D image = Capture(camera, RenderWidth, RenderHeight);
        image.hideFlags = HideFlags.DontUnloadUnusedAsset;
        ImageMetric metric = AnalyzeImage(fileName, image);

        string absolutePath = Path.Combine(OutputFolder, fileName);
        File.WriteAllBytes(absolutePath, image.EncodeToPNG());
        Debug.Log("CAML10 render written: " + absolutePath);

        ShotRecord record = new ShotRecord
        {
            title = title,
            path = AbsoluteToRepoRelative(absolutePath),
            width = RenderWidth,
            height = RenderHeight,
            camera = FormatVector(cameraPosition) + " -> " + FormatVector(lookAt) + ", fov " + fieldOfView.ToString("0.0", CultureInfo.InvariantCulture),
            metric = metric,
            stats = stats,
            texture = image
        };

        return record;
    }

    private delegate void SceneBuilder(Transform root, AssemblyMaterials materials, SceneStats stats);

    private static void ConfigureRenderSettings()
    {
        QualitySettings.antiAliasing = 8;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.025f, 0.022f, 0.019f, 1f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(0.018f, 0.017f, 0.016f, 1f);
        RenderSettings.fogDensity = 0.026f;
        RenderSettings.reflectionIntensity = 0.72f;
    }

    private static void BuildAcceptedFamilyHero(Transform root, AssemblyMaterials materials, SceneStats stats)
    {
        BuildCorridorEnvelope(root, materials, stats, 12.4f, true);

        SpawnPrefab(RoomShellPackage + "/Runtime/Prefabs/RSS07_PressureBypassCorridor_Straight_4m.prefab", new Vector3(0f, 0.02f, -1.4f), Quaternion.identity, Vector3.one, root, stats);
        SpawnPrefab(RoomShellPackage + "/Runtime/Prefabs/RSS07_PressureBypassCorridor_Straight_4m.prefab", new Vector3(0f, 0.02f, 2.6f), Quaternion.identity, Vector3.one, root, stats);
        SpawnPrefab(RoomShellPackage + "/Runtime/Prefabs/RSS07_PressureBypassCorridor_Straight_4m.prefab", new Vector3(0f, 0.02f, 6.6f), Quaternion.identity, Vector3.one, root, stats);
        for (int i = 0; i < 5; i++)
        {
            float z = -2.8f + i * 2.25f;
            SpawnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_001_WallPipeTripleRun_A.prefab", -1, z, 0.78f, root, stats);
            SpawnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_002_WallGaugeManifold_B.prefab", 1, z + 0.85f, 0.68f, root, stats);
            SpawnWallPiece(GaslightPackage + "/Runtime/Prefabs/GPD10_PREFAB_01_WallGaslights_A.prefab", -1, z + 1.05f, 0.64f, root, stats);
            SpawnWallPiece(GaslightPackage + "/Runtime/Prefabs/GPD10_PREFAB_02_WallGaslights_B.prefab", 1, z - 0.15f, 0.64f, root, stats);
            AddLampLight(new Vector3(-1.68f, 1.55f, z + 1.05f), 2.25f, 1.55f, stats);
            AddLampLight(new Vector3(1.68f, 1.55f, z - 0.15f), 2.15f, 1.45f, stats);
        }

        SpawnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_004_WallValveBattery_D.prefab", 1, 5.6f, 0.72f, root, stats);
        SpawnFloorPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_007_FloorWetDrainPlate_A.prefab", new Vector3(-0.82f, 0.12f, -1.2f), 0.82f, root, stats);
        SpawnFloorPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_008_FloorGrateChannel_B.prefab", new Vector3(0.82f, 0.12f, 3.8f), 0.92f, root, stats);

        BuildLongPipeRuns(root, materials, stats, 11.6f);
        BuildPressureDoorTarget(root, materials, stats);
        BuildSteamPuffs(root, materials, stats, new Vector3(-1.45f, 0.38f, 2.8f), new Vector3(1.52f, 0.58f, 6.65f), new Vector3(0.2f, 2.05f, 8.4f));
        AddWarmDoorBacklight(new Vector3(0f, 1.65f, 8.7f), stats);

        stats.notes = "Three RSS07 corridor bays, HRS07/RSS07 pressure-door end target, SCD09 wall/floor pieces, GPD10 wall gaslights, RMS10 dark wet masonry staging.";
    }

    private static void BuildCornerDensityShot(Transform root, AssemblyMaterials materials, SceneStats stats)
    {
        BuildCorridorEnvelope(root, materials, stats, 8.8f, false);
        CreateBox("right turn floor slab", new Vector3(2.1f, -0.065f, 2.05f), new Vector3(6.2f, 0.12f, 4.25f), Quaternion.identity, materials.WetFloor, root, stats);
        CreateBox("turn back masonry wall", new Vector3(2.1f, 1.35f, 4.17f), new Vector3(6.2f, 2.7f, 0.14f), Quaternion.identity, materials.DarkWetBrickWall, root, stats);
        CreateBox("turn outside wall", new Vector3(5.15f, 1.35f, 2.05f), new Vector3(0.14f, 2.7f, 4.25f), Quaternion.identity, materials.DarkWetBrickWall, root, stats);

        SpawnPrefab(RoomShellPackage + "/Runtime/Prefabs/RSS07_PressureBypassCorridor_Bent_90.prefab", new Vector3(0f, 0.02f, 0.2f), Quaternion.identity, new Vector3(1.08f, 1.08f, 1.08f), root, stats);
        SpawnPrefab(RoomShellPackage + "/Runtime/Prefabs/RSS07_PipeCanopyModule_Crossing.prefab", new Vector3(0f, 1.2f, 1.7f), Quaternion.identity, new Vector3(1.04f, 1.04f, 1.04f), root, stats);
        SpawnPrefab(RoomShellPackage + "/Runtime/Prefabs/RSS07_ShortcutGateShell_LockedBrass.prefab", new Vector3(4.1f, 0.02f, 3.85f), Quaternion.Euler(0f, 180f, 0f), new Vector3(1.08f, 1.08f, 1.08f), root, stats);

        SpawnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_003_WallTankStrapped_C.prefab", -1, -1.8f, 0.82f, root, stats);
        SpawnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_004_WallValveBattery_D.prefab", -1, 0.4f, 0.82f, root, stats);
        SpawnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_012_CeilingPipeCluster_A.prefab", 1, -0.8f, 0.75f, root, stats);
        SpawnTurnWallPiece(GaslightPackage + "/Runtime/Prefabs/GPD10_PREFAB_04_WallGaslights_D.prefab", new Vector3(1.25f, 0f, 4.05f), Quaternion.Euler(0f, 180f, 0f), new Vector3(0.72f, 0.72f, 0.72f), root, stats);
        SpawnTurnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_020_DoorwaySideTankPair_D.prefab", new Vector3(3.05f, 0f, 4.05f), Quaternion.Euler(0f, 180f, 0f), new Vector3(0.76f, 0.76f, 0.76f), root, stats);
        SpawnTurnWallPiece(GaslightPackage + "/Runtime/Prefabs/GPD10_PREFAB_22_ReflectionHelpers_B.prefab", new Vector3(3.8f, 0f, 4.01f), Quaternion.Euler(0f, 180f, 0f), new Vector3(1.05f, 1.05f, 1.05f), root, stats);
        AddLampLight(new Vector3(1.25f, 1.55f, 3.72f), 2.45f, 1.65f, stats);

        BuildLongPipeRuns(root, materials, stats, 7.6f);
        CreateCylinder("turn overhead pipe A", new Vector3(2.8f, 2.22f, 3.52f), 4.5f, 0.06f, Quaternion.Euler(0f, 0f, 90f), materials.AgedBrass, root, stats);
        CreateCylinder("turn overhead pipe B", new Vector3(2.85f, 2.02f, 3.18f), 4.2f, 0.052f, Quaternion.Euler(0f, 0f, 90f), materials.Copper, root, stats);
        BuildSteamPuffs(root, materials, stats, new Vector3(-1.45f, 0.48f, 1.6f), new Vector3(3.85f, 0.65f, 3.45f), new Vector3(0.05f, 2.15f, 0.45f));

        stats.notes = "Corner composition checks whether completed dressing families carry north-star density when the corridor turns and the shell is not just a straight tunnel.";
    }

    private static void BuildMaterialFixtureDetail(Transform root, AssemblyMaterials materials, SceneStats stats)
    {
        stats.RecordPackage(RoomMaterialPackage);
        CreateBox("detail wet floor", new Vector3(0f, -0.06f, 0.8f), new Vector3(5.2f, 0.12f, 4.8f), Quaternion.identity, materials.WetFloor, root, stats);
        CreateBox("detail masonry wall", new Vector3(-2.12f, 1.35f, 0.8f), new Vector3(0.14f, 2.7f, 4.8f), Quaternion.identity, materials.DarkWetBrickWall, root, stats);
        CreateBox("detail sooted ceiling", new Vector3(0f, 2.62f, 0.8f), new Vector3(5.2f, 0.14f, 4.8f), Quaternion.identity, materials.SootedCeiling, root, stats);
        CreateBox("detail black iron kick plate", new Vector3(-2.02f, 0.32f, 0.8f), new Vector3(0.12f, 0.42f, 4.55f), Quaternion.identity, materials.BlackenedIron, root, stats);

        SpawnWallPiece(GaslightPackage + "/Runtime/Prefabs/GPD10_PREFAB_03_WallGaslights_C.prefab", -1, 0.1f, 0.9f, root, stats);
        SpawnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_002_WallGaugeManifold_B.prefab", -1, 1.35f, 0.92f, root, stats);
        SpawnWallPiece(SteamDressingPackage + "/Runtime/Generated/Prefabs/SCD09_PREFAB_004_WallValveBattery_D.prefab", -1, 2.55f, 0.88f, root, stats);
        SpawnWallPiece(GaslightPackage + "/Runtime/Prefabs/GPD10_PREFAB_21_ReflectionHelpers_A.prefab", -1, -1.15f, 1.15f, root, stats);
        SpawnWallPiece(GaslightPackage + "/Runtime/Prefabs/GPD10_PREFAB_17_WallPlaques_A.prefab", -1, -0.9f, 0.72f, root, stats);
        AddLampLight(new Vector3(-1.66f, 1.57f, 0.1f), 2.8f, 1.9f, stats);

        CreateCylinder("detail near copper pipe", new Vector3(-1.54f, 1.86f, 0.85f), 4.3f, 0.07f, Quaternion.Euler(90f, 0f, 0f), materials.Copper, root, stats);
        CreateCylinder("detail upper black pipe", new Vector3(-1.38f, 2.13f, 0.7f), 4.2f, 0.055f, Quaternion.Euler(90f, 0f, 0f), materials.BlackenedIron, root, stats);
        CreateCylinder("detail lower brass pipe", new Vector3(-1.48f, 1.08f, 0.65f), 3.8f, 0.045f, Quaternion.Euler(90f, 0f, 0f), materials.AgedBrass, root, stats);
        AddRivetStrip(root, materials, stats, new Vector3(-1.92f, 0.68f, -1.2f), new Vector3(-1.92f, 0.68f, 3.0f), 12);
        AddWetHighlight(root, materials, stats, new Vector3(-0.3f, 0.016f, 0.3f), new Vector3(1.4f, 0.012f, 3.3f));
        BuildSteamPuffs(root, materials, stats, new Vector3(-1.5f, 0.58f, 2.85f), new Vector3(-1.32f, 1.92f, -0.75f));

        stats.notes = "Close review of RMS10 material response, GPD10 lamp silhouette, and SCD09 gauge/valve density under warm north-star lighting.";
    }

    private static void BuildCorridorEnvelope(Transform root, AssemblyMaterials materials, SceneStats stats, float length, bool addDoorBay)
    {
        stats.RecordPackage(RoomMaterialPackage);
        float zCenter = 1.6f;
        CreateBox("RMS10 wet flagstone floor staging", new Vector3(0f, -0.065f, zCenter), new Vector3(4.6f, 0.12f, length), Quaternion.identity, materials.WetFloor, root, stats);
        CreateBox("RMS10 left dark wet brick wall staging", new Vector3(-2.18f, 1.35f, zCenter), new Vector3(0.14f, 2.7f, length), Quaternion.identity, materials.DarkWetBrickWall, root, stats);
        CreateBox("RMS10 right dark wet brick wall staging", new Vector3(2.18f, 1.35f, zCenter), new Vector3(0.14f, 2.7f, length), Quaternion.identity, materials.DarkWetBrickWall, root, stats);
        CreateBox("RMS10 sooted brick ceiling staging", new Vector3(0f, 2.58f, zCenter), new Vector3(4.6f, 0.14f, length), Quaternion.identity, materials.SootedCeiling, root, stats);

        for (int i = 0; i < 8; i++)
        {
            float z = -4.0f + i * 1.55f;
            CreateBox("black iron left rib " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-2.02f, 1.34f, z), new Vector3(0.2f, 2.72f, 0.12f), Quaternion.identity, materials.BlackenedIron, root, stats);
            CreateBox("black iron right rib " + i.ToString(CultureInfo.InvariantCulture), new Vector3(2.02f, 1.34f, z), new Vector3(0.2f, 2.72f, 0.12f), Quaternion.identity, materials.BlackenedIron, root, stats);
            CreateBox("black iron ceiling rib " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0f, 2.42f, z), new Vector3(4.1f, 0.22f, 0.12f), Quaternion.identity, materials.BlackenedIron, root, stats);
            AddRivetStrip(root, materials, stats, new Vector3(-1.88f, 0.36f, z - 0.08f), new Vector3(-1.88f, 2.18f, z - 0.08f), 6);
            AddRivetStrip(root, materials, stats, new Vector3(1.88f, 0.36f, z - 0.08f), new Vector3(1.88f, 2.18f, z - 0.08f), 6);
        }

        for (int i = 0; i < 7; i++)
        {
            float z = -3.55f + i * 1.62f;
            CreateBox("offset floor plate seam " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0f, 0.018f, z), new Vector3(4.15f, 0.028f, 0.035f), Quaternion.identity, materials.BlackMortar, root, stats);
            AddWetHighlight(root, materials, stats, new Vector3(i % 2 == 0 ? -0.86f : 0.86f, 0.022f, z + 0.52f), new Vector3(0.88f, 0.012f, 0.34f));
        }

        if (addDoorBay)
        {
            CreateBox("end shadow wall behind pressure door", new Vector3(0f, 1.35f, 9.55f), new Vector3(4.55f, 2.7f, 0.16f), Quaternion.identity, materials.BlackenedIron, root, stats);
        }
    }

    private static void BuildLongPipeRuns(Transform root, AssemblyMaterials materials, SceneStats stats, float length)
    {
        CreateCylinder("left high brass pipe run", new Vector3(-1.62f, 2.12f, 1.9f), length, 0.055f, Quaternion.Euler(90f, 0f, 0f), materials.AgedBrass, root, stats);
        CreateCylinder("left mid copper pipe run", new Vector3(-1.48f, 1.88f, 1.8f), length * 0.9f, 0.05f, Quaternion.Euler(90f, 0f, 0f), materials.Copper, root, stats);
        CreateCylinder("right high black pipe run", new Vector3(1.58f, 2.05f, 1.65f), length * 0.95f, 0.055f, Quaternion.Euler(90f, 0f, 0f), materials.BlackenedIron, root, stats);
        CreateCylinder("right low brass pipe run", new Vector3(1.64f, 0.95f, 1.35f), length * 0.72f, 0.045f, Quaternion.Euler(90f, 0f, 0f), materials.AgedBrass, root, stats);

        for (int i = 0; i < 6; i++)
        {
            float z = -3.0f + i * 1.7f;
            CreateBox("left pipe clamp " + i.ToString(CultureInfo.InvariantCulture), new Vector3(-1.54f, 2.02f, z), new Vector3(0.28f, 0.08f, 0.08f), Quaternion.identity, materials.BlackenedIron, root, stats);
            CreateBox("right pipe clamp " + i.ToString(CultureInfo.InvariantCulture), new Vector3(1.58f, 2.0f, z + 0.2f), new Vector3(0.28f, 0.08f, 0.08f), Quaternion.identity, materials.BlackenedIron, root, stats);
        }
    }

    private static void BuildPressureDoorTarget(Transform root, AssemblyMaterials materials, SceneStats stats)
    {
        CreateSphere("north-star pressure door brass outer roundel", new Vector3(0f, 1.42f, 3.80f), new Vector3(2.85f, 2.85f, 0.12f), materials.AgedBrass, root, stats);
        CreateSphere("north-star pressure door black inset roundel", new Vector3(0f, 1.42f, 3.68f), new Vector3(2.18f, 2.18f, 0.14f), materials.BlackenedIron, root, stats);
        CreateSphere("north-star pressure door brass inner boss", new Vector3(0f, 1.42f, 3.56f), new Vector3(0.66f, 0.66f, 0.16f), materials.AgedBrass, root, stats);
        CreateSphere("north-star pressure door dark hub", new Vector3(0f, 1.42f, 3.46f), new Vector3(0.32f, 0.32f, 0.12f), materials.BlackenedIron, root, stats);

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            CreateBox("pressure door brass spoke " + i.ToString(CultureInfo.InvariantCulture), new Vector3(0f, 1.42f, 3.38f), new Vector3(1.82f, 0.075f, 0.05f), Quaternion.Euler(0f, 0f, angle), materials.AgedBrass, root, stats);
        }

        for (int i = 0; i < 14; i++)
        {
            float angleRadians = i * Mathf.PI * 2f / 14f;
            Vector3 position = new Vector3(Mathf.Cos(angleRadians) * 1.18f, 1.42f + Mathf.Sin(angleRadians) * 1.18f, 3.30f);
            GameObject rivet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.DestroyImmediate(rivet.GetComponent<Collider>());
            rivet.name = "CAML10 pressure door rim rivet";
            rivet.transform.SetParent(root, false);
            rivet.transform.position = position;
            rivet.transform.localScale = new Vector3(0.085f, 0.085f, 0.035f);
            rivet.GetComponent<Renderer>().sharedMaterial = materials.AgedBrass;
            stats.proceduralPrimitives++;
            stats.rivets++;
        }
    }

    private static void BuildSharedLighting(SceneStats stats)
    {
        GameObject sunObject = new GameObject("CAML10 soft overhead direction");
        Light sun = sunObject.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.color = new Color(1.0f, 0.72f, 0.46f);
        sun.intensity = 0.55f;
        sunObject.transform.rotation = Quaternion.Euler(48f, -32f, 0f);
        stats.lightCount++;

        GameObject coolFillObject = new GameObject("CAML10 cool smoke fill");
        Light fill = coolFillObject.AddComponent<Light>();
        fill.type = LightType.Point;
        fill.color = new Color(0.28f, 0.36f, 0.42f);
        fill.intensity = 0.55f;
        fill.range = 9.0f;
        coolFillObject.transform.position = new Vector3(0f, 2.4f, -4.0f);
        stats.lightCount++;
    }

    private static void AddLampLight(Vector3 position, float range, float intensity, SceneStats stats)
    {
        GameObject lamp = new GameObject("warm practical lamp");
        lamp.transform.position = position;
        Light light = lamp.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = range;
        light.intensity = intensity;
        light.color = new Color(1.0f, 0.55f, 0.17f);
        stats.lightCount++;
    }

    private static void AddWarmDoorBacklight(Vector3 position, SceneStats stats)
    {
        GameObject lamp = new GameObject("pressure door amber bloom");
        lamp.transform.position = position;
        Light light = lamp.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = 4.6f;
        light.intensity = 3.2f;
        light.color = new Color(1.0f, 0.34f, 0.05f);
        stats.lightCount++;
    }

    private static GameObject SpawnPrefab(string path, Vector3 position, Quaternion rotation, Vector3 scale, Transform root, SceneStats stats)
    {
        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (asset == null)
        {
            stats.missingAssets.Add(path);
            Debug.LogWarning("CAML10 missing prefab: " + path);
            return null;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(asset);
        instance.name = "CAML10_" + asset.name;
        instance.transform.SetParent(root, false);
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        instance.transform.localScale = scale;
        ApplyRenderSettings(instance);
        stats.prefabInstances++;
        stats.RecordPackage(path);
        return instance;
    }

    private static void SpawnWallPiece(string path, int side, float z, float scale, Transform root, SceneStats stats)
    {
        float x = side < 0 ? -2.08f : 2.08f;
        Quaternion rotation = side < 0 ? Quaternion.Euler(0f, 90f, 0f) : Quaternion.Euler(0f, -90f, 0f);
        SpawnPrefab(path, new Vector3(x, 0.02f, z), rotation, new Vector3(scale, scale, scale), root, stats);
    }

    private static void SpawnTurnWallPiece(string path, Vector3 position, Quaternion rotation, Vector3 scale, Transform root, SceneStats stats)
    {
        SpawnPrefab(path, position, rotation, scale, root, stats);
    }

    private static void SpawnFloorPiece(string path, Vector3 position, float scale, Transform root, SceneStats stats)
    {
        SpawnPrefab(path, position, Quaternion.Euler(90f, 0f, 0f), new Vector3(scale, scale, scale), root, stats);
    }

    private static void ApplyRenderSettings(GameObject instance)
    {
        Renderer[] renderers = instance.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].shadowCastingMode = ShadowCastingMode.On;
            renderers[i].receiveShadows = true;
        }
    }

    private static void BuildSteamPuffs(Transform root, AssemblyMaterials materials, SceneStats stats, params Vector3[] centers)
    {
        for (int i = 0; i < centers.Length; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector3 offset = new Vector3((j - 1.5f) * 0.08f, j * 0.11f, (j % 2 == 0 ? 0.12f : -0.12f));
                GameObject puff = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.DestroyImmediate(puff.GetComponent<Collider>());
                puff.name = "CAML10 soft steam puff";
                puff.transform.SetParent(root, false);
                puff.transform.position = centers[i] + offset;
                float size = 0.24f + j * 0.08f;
                puff.transform.localScale = new Vector3(size * 1.35f, size, size * 0.8f);
                puff.GetComponent<Renderer>().sharedMaterial = materials.SteamMist;
                stats.proceduralPrimitives++;
                stats.steamPuffs++;
            }
        }
    }

    private static void AddRivetStrip(Transform root, AssemblyMaterials materials, SceneStats stats, Vector3 start, Vector3 end, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float t = count <= 1 ? 0f : i / (float)(count - 1);
            Vector3 position = Vector3.Lerp(start, end, t);
            GameObject rivet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.DestroyImmediate(rivet.GetComponent<Collider>());
            rivet.name = "CAML10 brass rivet";
            rivet.transform.SetParent(root, false);
            rivet.transform.position = position;
            rivet.transform.localScale = new Vector3(0.07f, 0.07f, 0.034f);
            rivet.GetComponent<Renderer>().sharedMaterial = materials.AgedBrass;
            stats.proceduralPrimitives++;
            stats.rivets++;
        }
    }

    private static void AddWetHighlight(Transform root, AssemblyMaterials materials, SceneStats stats, Vector3 position, Vector3 scale)
    {
        CreateBox("broken warm wet reflection strip", position, scale, Quaternion.identity, materials.WetHighlight, root, stats);
        stats.wetHighlights++;
    }

    private static void CreateBox(string name, Vector3 position, Vector3 scale, Quaternion rotation, Material material, Transform root, SceneStats stats)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        UnityEngine.Object.DestroyImmediate(box.GetComponent<Collider>());
        box.name = "CAML10 " + name;
        box.transform.SetParent(root, false);
        box.transform.position = position;
        box.transform.rotation = rotation;
        box.transform.localScale = scale;
        box.GetComponent<Renderer>().sharedMaterial = material;
        stats.proceduralPrimitives++;
    }

    private static void CreateCylinder(string name, Vector3 position, float length, float radius, Quaternion rotation, Material material, Transform root, SceneStats stats)
    {
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        UnityEngine.Object.DestroyImmediate(cylinder.GetComponent<Collider>());
        cylinder.name = "CAML10 " + name;
        cylinder.transform.SetParent(root, false);
        cylinder.transform.position = position;
        cylinder.transform.rotation = rotation;
        cylinder.transform.localScale = new Vector3(radius * 2f, length * 0.5f, radius * 2f);
        cylinder.GetComponent<Renderer>().sharedMaterial = material;
        stats.proceduralPrimitives++;
        stats.pipeSegments++;
    }

    private static void CreateSphere(string name, Vector3 position, Vector3 scale, Material material, Transform root, SceneStats stats)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        UnityEngine.Object.DestroyImmediate(sphere.GetComponent<Collider>());
        sphere.name = "CAML10 " + name;
        sphere.transform.SetParent(root, false);
        sphere.transform.position = position;
        sphere.transform.localScale = scale;
        sphere.GetComponent<Renderer>().sharedMaterial = material;
        stats.proceduralPrimitives++;
    }

    private static Camera CreateCamera(string name, Vector3 position, Vector3 lookAt, float fieldOfView)
    {
        GameObject cameraObject = new GameObject(name);
        cameraObject.transform.position = position;
        cameraObject.transform.LookAt(lookAt);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.fieldOfView = fieldOfView;
        camera.nearClipPlane = 0.03f;
        camera.farClipPlane = 80f;
        camera.backgroundColor = new Color(0.012f, 0.011f, 0.010f, 1f);
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.allowHDR = true;
        camera.allowMSAA = true;
        return camera;
    }

    private static Texture2D Capture(Camera camera, int width, int height)
    {
        RenderTexture previous = RenderTexture.active;
        RenderTexture target = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)
        {
            antiAliasing = 8
        };
        target.Create();
        camera.targetTexture = target;
        camera.Render();
        RenderTexture.active = target;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        camera.targetTexture = null;
        RenderTexture.active = previous;
        target.Release();
        UnityEngine.Object.DestroyImmediate(target);
        return texture;
    }

    private static ImageMetric AnalyzeImage(string fileName, Texture2D texture)
    {
        Color32[] pixels = texture.GetPixels32();
        double luma = 0.0;
        int darkPixels = 0;
        int warmPixels = 0;
        int brightPixels = 0;

        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 pixel = pixels[i];
            double r = pixel.r / 255.0;
            double g = pixel.g / 255.0;
            double b = pixel.b / 255.0;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            luma += y;

            if (y < 0.18)
            {
                darkPixels++;
            }

            if (r > 0.34 && r > g * 1.15 && g > b * 0.9)
            {
                warmPixels++;
            }

            if (y > 0.72)
            {
                brightPixels++;
            }
        }

        double total = pixels.Length;
        return new ImageMetric
        {
            fileName = fileName,
            averageLuma = luma / total,
            darkPixelRatio = darkPixels / total,
            warmPixelRatio = warmPixels / total,
            brightPixelRatio = brightPixels / total
        };
    }

    private static Texture2D BuildContactSheet(List<ShotRecord> records)
    {
        Texture2D sheet = new Texture2D(ContactWidth, ContactHeight, TextureFormat.RGBA32, false, false);
        Color32 background = new Color32(16, 14, 12, 255);
        Color32 border = new Color32(126, 84, 35, 255);
        Color32[] fill = new Color32[ContactWidth * ContactHeight];
        for (int i = 0; i < fill.Length; i++)
        {
            fill[i] = background;
        }

        sheet.SetPixels32(fill);

        int cellWidth = 1120;
        int cellHeight = 630;
        int marginX = 80;
        int marginY = 80;
        for (int i = 0; i < records.Count; i++)
        {
            int col = i % 2;
            int row = i / 2;
            int x = marginX + col * (cellWidth + 80);
            int y = ContactHeight - marginY - cellHeight - row * (cellHeight + 90);
            DrawBorder(sheet, x - 6, y - 6, cellWidth + 12, cellHeight + 12, border);
            Texture2D source = LoadTextureFromFile(Path.Combine(RepoRoot, records[i].path.Replace("/", Path.DirectorySeparatorChar.ToString())));
            if (source != null)
            {
                DrawScaled(source, sheet, x, y, cellWidth, cellHeight);
                UnityEngine.Object.DestroyImmediate(source);
            }
        }

        sheet.Apply();
        return sheet;
    }

    private static Texture2D LoadTextureFromFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("CAML10 contact sheet source missing: " + path);
            return null;
        }

        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
        if (!ImageConversion.LoadImage(texture, File.ReadAllBytes(path)))
        {
            UnityEngine.Object.DestroyImmediate(texture);
            return null;
        }

        return texture;
    }

    private static void DrawScaled(Texture2D source, Texture2D destination, int dstX, int dstY, int dstWidth, int dstHeight)
    {
        for (int y = 0; y < dstHeight; y++)
        {
            int sourceY = Mathf.Clamp(y * source.height / dstHeight, 0, source.height - 1);
            for (int x = 0; x < dstWidth; x++)
            {
                int sourceX = Mathf.Clamp(x * source.width / dstWidth, 0, source.width - 1);
                destination.SetPixel(dstX + x, dstY + y, source.GetPixel(sourceX, sourceY));
            }
        }
    }

    private static void DrawBorder(Texture2D texture, int x, int y, int width, int height, Color32 color)
    {
        for (int i = 0; i < width; i++)
        {
            SafeSetPixel(texture, x + i, y, color);
            SafeSetPixel(texture, x + i, y + height - 1, color);
        }

        for (int j = 0; j < height; j++)
        {
            SafeSetPixel(texture, x, y + j, color);
            SafeSetPixel(texture, x + width - 1, y + j, color);
        }
    }

    private static void SafeSetPixel(Texture2D texture, int x, int y, Color32 color)
    {
        if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
        {
            texture.SetPixel(x, y, color);
        }
    }

    private static string BuildManifestJson(LookdevRun run)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("{");
        builder.AppendLine("  \"schema\": \"brassworks.corridor_assembly_lookdev10.render_manifest.v1\",");
        builder.AppendLine("  \"timestamp\": " + JsonQuote(run.timestamp) + ",");
        builder.AppendLine("  \"unityVersion\": " + JsonQuote(run.unityVersion) + ",");
        builder.AppendLine("  \"northStarReference\": " + JsonQuote(run.northStarReference) + ",");
        builder.AppendLine("  \"sourcePolicy\": " + JsonQuote(run.sourcePolicy) + ",");
        builder.AppendLine("  \"writeScope\": " + JsonQuote(run.writeScope) + ",");
        builder.AppendLine("  \"contactSheet\": " + JsonQuote(run.contactSheet) + ",");
        builder.AppendLine("  \"shots\": [");
        for (int i = 0; i < run.shots.Length; i++)
        {
            ShotRecord shot = run.shots[i];
            builder.AppendLine("    {");
            builder.AppendLine("      \"title\": " + JsonQuote(shot.title) + ",");
            builder.AppendLine("      \"path\": " + JsonQuote(shot.path) + ",");
            builder.AppendLine("      \"camera\": " + JsonQuote(shot.camera) + ",");
            builder.AppendLine("      \"prefabInstances\": " + shot.stats.prefabInstances.ToString(CultureInfo.InvariantCulture) + ",");
            builder.AppendLine("      \"proceduralPrimitives\": " + shot.stats.proceduralPrimitives.ToString(CultureInfo.InvariantCulture) + ",");
            builder.AppendLine("      \"packageFamilies\": [" + JsonQuoteJoin(shot.stats.packageFamilies) + "],");
            builder.AppendLine("      \"missingAssets\": [" + JsonQuoteJoin(shot.stats.missingAssets) + "],");
            builder.AppendLine("      \"averageLuma\": " + shot.metric.averageLuma.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
            builder.AppendLine("      \"darkPixelRatio\": " + shot.metric.darkPixelRatio.ToString("0.0000", CultureInfo.InvariantCulture) + ",");
            builder.AppendLine("      \"warmPixelRatio\": " + shot.metric.warmPixelRatio.ToString("0.0000", CultureInfo.InvariantCulture));
            builder.Append("    }");
            if (i < run.shots.Length - 1)
            {
                builder.Append(",");
            }

            builder.AppendLine();
        }

        builder.AppendLine("  ]");
        builder.AppendLine("}");
        return builder.ToString();
    }

    private static string BuildAssemblyNotes(LookdevRun run)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# CAML10 Asset Assembly Notes");
        builder.AppendLine();
        builder.AppendLine("- Unity version: " + run.unityVersion);
        builder.AppendLine("- North-star reference: `" + run.northStarReference + "`");
        builder.AppendLine("- Method: in-memory Unity editor scenes, Camera.Render, PNG export.");
        builder.AppendLine("- Input packages are local package references and remain unmodified.");
        builder.AppendLine();
        builder.AppendLine("## Package Families Used");
        builder.AppendLine();
        builder.AppendLine("- `com.brassworks.sidecar.room-shell-set07`: corridor bays, bend, vault/shortcut frames.");
        builder.AppendLine("- `com.brassworks.sidecar.hero-room-render-set07`: prior pressure-door/corridor render reference only; the assembly script does not modify or rely on its scenes.");
        builder.AppendLine("- `com.brassworks.sidecar.steam-corridor-dressing-set09`: pipe, gauge, tank, valve, floor/drain and doorway dressing.");
        builder.AppendLine("- `com.brassworks.sidecar.gaslight-pipe-dressing-set10`: gaslights, reflection helpers, plaques.");
        builder.AppendLine("- `com.brassworks.sidecar.room-material-set10`: dark wet brick, sooted ceiling, wet flagstone, black mortar staging materials.");
        builder.AppendLine();
        builder.AppendLine("## Render Outputs");
        builder.AppendLine();
        for (int i = 0; i < run.shots.Length; i++)
        {
            ShotRecord shot = run.shots[i];
            builder.AppendLine("- `" + shot.path + "`: " + shot.title + "; " + shot.stats.prefabInstances.ToString(CultureInfo.InvariantCulture) + " package prefab instances, " + shot.stats.proceduralPrimitives.ToString(CultureInfo.InvariantCulture) + " Unity staging primitives.");
        }

        builder.AppendLine("- `" + run.contactSheet + "`: contact sheet.");
        return builder.ToString();
    }

    private static string BuildQaReport(LookdevRun run)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("# CAML10 Blunt North-Star Comparison");
        builder.AppendLine();
        builder.AppendLine("Reference: `Documentation/ConceptArt/north-star-steampunk-level-hud-enemies-props.png`.");
        builder.AppendLine("These renders are concept/lookdev only, outside the game build.");
        builder.AppendLine();
        builder.AppendLine("## Verdict");
        builder.AppendLine();
        builder.AppendLine("The assembly is directionally useful but not production-ready. It passes the broad corridor read: dark wet shell, brass/black iron palette, warm lamps, steam haze, and sidewall mechanical language. It fails the north-star focal-detail bar: the pressure-door target is still weak/occluded, and the object silhouettes are too planar and too generator-clean next to the north-star paintover.");
        builder.AppendLine();
        builder.AppendLine("## Passes");
        builder.AppendLine();
        builder.AppendLine("- RSS07 gives a readable corridor footprint, turn, ribs, and pressure-door framing without touching main scenes.");
        builder.AppendLine("- RMS10 materially moves the shell away from gray blockout toward dark wet masonry and glossy flagstone.");
        builder.AppendLine("- GPD10 lamps are the best-performing new family in assembly: they give readable amber practicals and wall soot anchors.");
        builder.AppendLine("- RSS07 plus Unity-only staging can establish a corridor frame without touching the HRS07 render scenes.");
        builder.AppendLine("- SCD09 provides the right categories: pipes, gauges, tanks, valves, floor drains, vents, ceiling clusters, and doorway dressing.");
        builder.AppendLine();
        builder.AppendLine("## Fails");
        builder.AppendLine();
        builder.AppendLine("- SCD09 pieces read as flat kit cards in close view. The north-star needs deeper pipe offsets, elbows, valve wheels, overlapping brackets, gauge needles, and tank straps that break silhouette.");
        builder.AppendLine("- The pressure-door/focal read is not good enough. The staged target is occluded by mid-corridor dressing and does not land like the north-star locked gate.");
        builder.AppendLine("- RSS07 shell modules still expose slab-simple walls and ceiling ribs when the camera gets close. RMS10 helps the material read, but it cannot hide blockout-level geometry.");
        builder.AppendLine("- Steam and wetness are lookdev staging, not a validated VFX/material integration pass.");
        builder.AppendLine("- The assembly has corridor mood but not enough authored grime, decal history, damage variation, or hand-placed clutter.");
        builder.AppendLine("- No player weapon/HUD integration is assessed here; this is only environmental corridor lookdev.");
        builder.AppendLine();
        builder.AppendLine("## Revise Next");
        builder.AppendLine();
        builder.AppendLine("Revise `com.brassworks.sidecar.steam-corridor-dressing-set09` next. The shell and material packages are serviceable enough to keep evaluating corridor concepts, and GPD10 passes as a lighting/fixture booster. The biggest remaining gap against the north-star is object-family depth: SCD09 must become more three-dimensional, less planar, and more mechanically layered before another corridor assembly pass will feel close.");
        builder.AppendLine();
        builder.AppendLine("## Shot Metrics");
        builder.AppendLine();
        for (int i = 0; i < run.shots.Length; i++)
        {
            ShotRecord shot = run.shots[i];
            builder.AppendLine("- `" + shot.path + "`: avg luma " + shot.metric.averageLuma.ToString("0.000", CultureInfo.InvariantCulture) + ", dark ratio " + shot.metric.darkPixelRatio.ToString("0.000", CultureInfo.InvariantCulture) + ", warm ratio " + shot.metric.warmPixelRatio.ToString("0.000", CultureInfo.InvariantCulture) + ", prefabs " + shot.stats.prefabInstances.ToString(CultureInfo.InvariantCulture) + ", staging primitives " + shot.stats.proceduralPrimitives.ToString(CultureInfo.InvariantCulture) + ".");
        }

        builder.AppendLine();
        builder.AppendLine("## Missing Assets");
        bool anyMissing = false;
        for (int i = 0; i < run.shots.Length; i++)
        {
            for (int j = 0; j < run.shots[i].stats.missingAssets.Count; j++)
            {
                anyMissing = true;
                builder.AppendLine("- `" + run.shots[i].stats.missingAssets[j] + "`");
            }
        }

        if (!anyMissing)
        {
            builder.AppendLine("- None detected by the isolated render script.");
        }

        return builder.ToString();
    }

    private static string AbsoluteToRepoRelative(string path)
    {
        string normalizedRoot = RepoRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        if (path.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
        {
            return path.Substring(normalizedRoot.Length).Replace("\\", "/");
        }

        return path.Replace("\\", "/");
    }

    private static string FormatVector(Vector3 vector)
    {
        return "(" + vector.x.ToString("0.00", CultureInfo.InvariantCulture) + ", " + vector.y.ToString("0.00", CultureInfo.InvariantCulture) + ", " + vector.z.ToString("0.00", CultureInfo.InvariantCulture) + ")";
    }

    private static string JsonQuoteJoin(List<string> values)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < values.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append(JsonQuote(values[i]));
        }

        return builder.ToString();
    }

    private static string JsonQuote(string value)
    {
        if (value == null)
        {
            return "null";
        }

        return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n") + "\"";
    }

    private sealed class AssemblyMaterials
    {
        public Material DarkWetBrickWall;
        public Material SootedCeiling;
        public Material WetFloor;
        public Material BlackMortar;
        public Material AgedBrass;
        public Material Copper;
        public Material BlackenedIron;
        public Material SteamMist;
        public Material WetHighlight;

        public static AssemblyMaterials Load()
        {
            AssemblyMaterials materials = new AssemblyMaterials();
            materials.DarkWetBrickWall = LoadMaterial(RoomMaterialPackage + "/Runtime/Materials/RMS10_MAT_DarkWetBrickWall.mat", "fallback dark wet brick", new Color(0.085f, 0.068f, 0.055f), 0.02f, 0.62f);
            materials.SootedCeiling = LoadMaterial(RoomMaterialPackage + "/Runtime/Materials/RMS10_MAT_SootedBrickCeiling.mat", "fallback sooted ceiling", new Color(0.036f, 0.033f, 0.030f), 0.01f, 0.42f);
            materials.WetFloor = LoadMaterial(RoomMaterialPackage + "/Runtime/Materials/RMS10_MAT_WetUnevenFlagstoneFloor.mat", "fallback wet flagstone", new Color(0.080f, 0.088f, 0.084f), 0.02f, 0.76f);
            materials.BlackMortar = LoadMaterial(RoomMaterialPackage + "/Runtime/Materials/RMS10_MAT_BlackMortarGrime.mat", "fallback black mortar", new Color(0.018f, 0.016f, 0.014f), 0.0f, 0.28f);
            materials.AgedBrass = LoadMaterial(GaslightPackage + "/Runtime/Materials/GPD10_MAT_AgedBrassWarm.mat", "fallback aged brass", new Color(0.72f, 0.48f, 0.20f), 0.82f, 0.48f);
            materials.Copper = LoadMaterial(SteamDressingPackage + "/Runtime/Generated/Materials/SCD09_MAT_BurnishedCopper.mat", "fallback burnished copper", new Color(0.58f, 0.25f, 0.13f), 0.86f, 0.44f);
            materials.BlackenedIron = LoadMaterial(GaslightPackage + "/Runtime/Materials/GPD10_MAT_BlackenedPipeIron.mat", "fallback blackened iron", new Color(0.025f, 0.028f, 0.028f), 0.72f, 0.36f);
            materials.SteamMist = CreateTransparent("CAML10_SteamMist", new Color(0.74f, 0.70f, 0.62f, 0.22f), 0.0f, 0.85f);
            materials.WetHighlight = CreateTransparent("CAML10_WarmWetHighlight", new Color(1.0f, 0.55f, 0.18f, 0.18f), 0.0f, 0.92f);
            return materials;
        }

        private static Material LoadMaterial(string path, string fallbackName, Color fallbackColor, float metallic, float smoothness)
        {
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null)
            {
                return material;
            }

            Material roomMaterial = TryCreateRoomMaterialFromDisk(path, fallbackName, fallbackColor, metallic, smoothness);
            if (roomMaterial != null)
            {
                return roomMaterial;
            }

            Debug.LogWarning("CAML10 missing material: " + path + ". Using " + fallbackName + ".");
            return CreateOpaque(fallbackName, fallbackColor, metallic, smoothness);
        }

        private static Material TryCreateRoomMaterialFromDisk(string path, string fallbackName, Color fallbackColor, float metallic, float smoothness)
        {
            if (!path.StartsWith(RoomMaterialPackage, StringComparison.Ordinal))
            {
                return null;
            }

            string key = Path.GetFileNameWithoutExtension(path);
            string textureRoot = Path.Combine(RepoRoot, "AssetPacks", "BrassworksBreach.RoomMaterialSet10", "Runtime", "Textures");
            string albedoPath = Path.Combine(textureRoot, "Albedo", key + "_ALB.png");
            string normalPath = Path.Combine(textureRoot, "Normal", key + "_NRM.png");
            string rmaPath = Path.Combine(textureRoot, "RoughnessMetallic", key + "_RMA.png");

            Texture2D albedo = LoadDiskTexture(albedoPath, false);
            if (albedo == null)
            {
                return null;
            }

            Material material = CreateOpaque("CAML10_" + key + "_disk", fallbackColor, metallic, smoothness);
            material.SetTexture("_MainTex", albedo);
            material.SetTextureScale("_MainTex", key.Contains("Floor") ? new Vector2(2.6f, 4.8f) : new Vector2(2.0f, 2.0f));
            material.mainTexture = albedo;

            Texture2D normal = LoadDiskTexture(normalPath, true);
            if (normal != null)
            {
                material.SetTexture("_BumpMap", normal);
                material.EnableKeyword("_NORMALMAP");
                material.SetFloat("_BumpScale", key.Contains("Floor") ? 0.82f : 0.68f);
            }

            Texture2D rma = LoadDiskTexture(rmaPath, true);
            if (rma != null)
            {
                material.SetTexture("_MetallicGlossMap", rma);
                material.EnableKeyword("_METALLICGLOSSMAP");
            }

            Debug.Log("CAML10 loaded RoomMaterialSet10 textures directly from disk for " + key + ".");
            return material;
        }

        private static Texture2D LoadDiskTexture(string path, bool linear)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, linear);
            byte[] bytes = File.ReadAllBytes(path);
            if (!ImageConversion.LoadImage(texture, bytes))
            {
                UnityEngine.Object.DestroyImmediate(texture);
                return null;
            }

            texture.name = Path.GetFileNameWithoutExtension(path);
            texture.hideFlags = HideFlags.DontUnloadUnusedAsset;
            return texture;
        }

        private static Material CreateOpaque(string name, Color color, float metallic, float smoothness)
        {
            Shader shader = Shader.Find("Standard");
            Material material = new Material(shader);
            material.name = name;
            material.SetColor("_Color", color);
            material.SetFloat("_Metallic", metallic);
            material.SetFloat("_Glossiness", smoothness);
            return material;
        }

        private static Material CreateTransparent(string name, Color color, float metallic, float smoothness)
        {
            Material material = CreateOpaque(name, color, metallic, smoothness);
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetFloat("_Mode", 3f);
            material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)RenderQueue.Transparent;
            return material;
        }
    }

    [Serializable]
    private sealed class LookdevRun
    {
        public string timestamp;
        public string unityVersion;
        public string northStarReference;
        public string sourcePolicy;
        public string writeScope;
        public string contactSheet;
        public ShotRecord[] shots;
    }

    [Serializable]
    private sealed class ShotRecord
    {
        public string title;
        public string path;
        public int width;
        public int height;
        public string camera;
        public ImageMetric metric;
        public SceneStats stats;
        public Texture2D texture;
    }

    [Serializable]
    private sealed class ImageMetric
    {
        public string fileName;
        public double averageLuma;
        public double darkPixelRatio;
        public double warmPixelRatio;
        public double brightPixelRatio;
    }

    [Serializable]
    private sealed class SceneStats
    {
        public int prefabInstances;
        public int proceduralPrimitives;
        public int lightCount;
        public int rivets;
        public int pipeSegments;
        public int steamPuffs;
        public int wetHighlights;
        public string notes;
        public readonly List<string> missingAssets = new List<string>();
        public readonly List<string> packageFamilies = new List<string>();

        public void RecordPackage(string path)
        {
            string package = "unknown";
            if (path.StartsWith(RoomShellPackage, StringComparison.Ordinal))
            {
                package = "com.brassworks.sidecar.room-shell-set07";
            }
            else if (path.StartsWith(HeroRoomPackage, StringComparison.Ordinal))
            {
                package = "com.brassworks.sidecar.hero-room-render-set07";
            }
            else if (path.StartsWith(SteamDressingPackage, StringComparison.Ordinal))
            {
                package = "com.brassworks.sidecar.steam-corridor-dressing-set09";
            }
            else if (path.StartsWith(GaslightPackage, StringComparison.Ordinal))
            {
                package = "com.brassworks.sidecar.gaslight-pipe-dressing-set10";
            }
            else if (path.StartsWith(RoomMaterialPackage, StringComparison.Ordinal))
            {
                package = "com.brassworks.sidecar.room-material-set10";
            }

            if (!packageFamilies.Contains(package))
            {
                packageFamilies.Add(package);
            }
        }
    }
}
