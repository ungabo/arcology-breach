using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class V0135MechanicalEnemyPackBuilder
{
    private const string PackRoot = "Assets/_Project/ArtStaging/V0_1_35_MechanicalEnemyPack";
    private const string DocRoot = "Documentation/AssetProduction/V0_1_35_MechanicalEnemyPack";
    private const string RenderRoot = "Documentation/ConceptRenders/V0_1_35_MechanicalEnemyPack";
    private const string Version = "v0.1.35";

    private static readonly Dictionary<string, Material> Mats = new Dictionary<string, Material>();
    private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();

    [MenuItem("Tools/Art Staging/Build v0.1.35 Mechanical Enemy Pack")]
    public static void BuildPack()
    {
        EnsureFolders();
        BuildMaterials();
        BuildReusableMeshes();
        BuildEnemyFamilies();
        WriteDocumentation();
        RenderPreviewSheets();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Built v0.1.35 Mechanical Enemy Pack staging package.");
    }

    private static void EnsureFolders()
    {
        EnsureAssetFolder(PackRoot);
        foreach (var folder in new[] { "Materials", "Meshes", "Prefabs", "Metadata", "Editor" })
        {
            EnsureAssetFolder($"{PackRoot}/{folder}");
        }

        Directory.CreateDirectory(DocRoot);
        Directory.CreateDirectory(RenderRoot);
    }

    private static void EnsureAssetFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        var parent = Path.GetDirectoryName(path).Replace("\\", "/");
        var name = Path.GetFileName(path);
        EnsureAssetFolder(parent);
        AssetDatabase.CreateFolder(parent, name);
    }

    private static void BuildMaterials()
    {
        CreateMat("MAT_V0135_AgedBrass", new Color(0.72f, 0.48f, 0.18f), 0.78f, 0.38f);
        CreateMat("MAT_V0135_BlackenedIron", new Color(0.055f, 0.052f, 0.047f), 0.82f, 0.23f);
        CreateMat("MAT_V0135_OilyLeather", new Color(0.13f, 0.075f, 0.038f), 0.12f, 0.58f);
        CreateMat("MAT_V0135_GrimyGlass", new Color(0.46f, 0.62f, 0.55f, 0.46f), 0.02f, 0.8f);
        CreateMat("MAT_V0135_AmberFurnaceLamp", new Color(1.0f, 0.47f, 0.08f), 0.04f, 0.68f, new Color(1.7f, 0.62f, 0.08f));
        CreateMat("MAT_V0135_CyanBoltTell", new Color(0.05f, 0.82f, 1.0f), 0.02f, 0.76f, new Color(0.08f, 1.25f, 1.7f));
        CreateMat("MAT_V0135_SootWear", new Color(0.018f, 0.015f, 0.012f), 0.0f, 0.34f);
        CreateMat("MAT_V0135_HazardTrim", new Color(1.0f, 0.75f, 0.08f), 0.16f, 0.43f);
        CreateMat("MAT_V0135_ShutdownDimMetal", new Color(0.24f, 0.22f, 0.18f), 0.52f, 0.18f);
        CreateMat("MAT_V0135_SilhouetteTagInk", new Color(0.0f, 0.0f, 0.0f), 0.0f, 0.12f);
        CreateMat("MAT_V0135_WeakPointRedGlass", new Color(1.0f, 0.06f, 0.025f), 0.03f, 0.7f, new Color(1.35f, 0.04f, 0.02f));
    }

    private static void CreateMat(string name, Color color, float metallic, float smoothness, Color? emission = null)
    {
        var mat = new Material(Shader.Find("Standard"));
        mat.name = name;
        mat.SetColor("_Color", color);
        mat.SetFloat("_Metallic", metallic);
        mat.SetFloat("_Glossiness", smoothness);
        if (emission.HasValue)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emission.Value);
        }

        var path = $"{PackRoot}/Materials/{name}.mat";
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(mat, path);
        Mats[name] = mat;
    }

    private static void BuildReusableMeshes()
    {
        SaveMesh("MESH_V0135_BoxUnit", MeshBox(Vector3.one));
        SaveMesh("MESH_V0135_TorsoCageTall", MeshBox(new Vector3(1.1f, 1.75f, 0.58f)));
        SaveMesh("MESH_V0135_TorsoCageWide", MeshBox(new Vector3(1.55f, 1.35f, 0.72f)));
        SaveMesh("MESH_V0135_TorsoCageHeavy", MeshBox(new Vector3(1.9f, 1.65f, 0.92f)));
        SaveMesh("MESH_V0135_LimbBar", MeshBox(new Vector3(0.28f, 1.05f, 0.28f)));
        SaveMesh("MESH_V0135_ShieldPlate", MeshBox(new Vector3(1.45f, 1.75f, 0.16f)));
        SaveMesh("MESH_V0135_HammerHead", MeshBox(new Vector3(0.98f, 0.38f, 0.5f)));
        SaveMesh("MESH_V0135_ClawBlade", MeshWedge(0.24f, 1.0f, 0.16f));
        SaveMesh("MESH_V0135_LanceNeedle", MeshCone(0.18f, 1.6f, 12));
        SaveMesh("MESH_V0135_SawDisc", MeshCylinder(0.52f, 0.14f, 24));
        SaveMesh("MESH_V0135_CoilRing", MeshTorus(0.34f, 0.055f, 24, 8));
        SaveMesh("MESH_V0135_LampOrb", MeshUvSphere(0.18f, 12, 8));
        SaveMesh("MESH_V0135_Rivet", MeshUvSphere(0.055f, 8, 6));
        SaveMesh("MESH_V0135_ShutdownFragment", MeshWedge(0.42f, 0.52f, 0.22f));
        SaveMesh("MESH_V0135_TagPanel", MeshBox(new Vector3(0.74f, 0.22f, 0.035f)));
        SaveMesh("MESH_V0135_PistonRod", MeshCylinder(0.07f, 1.15f, 12));
    }

    private static void SaveMesh(string name, Mesh mesh)
    {
        mesh.name = name;
        var path = $"{PackRoot}/Meshes/{name}.asset";
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(mesh, path);
        Meshes[name] = mesh;
    }

    private static void BuildEnemyFamilies()
    {
        BuildScrapper();
        BuildLancer();
        BuildBulwark();
        BuildWarden();
        BuildFoundryOverseer();
    }

    private static GameObject Root(string name, string family, float scaleMeters, string tag)
    {
        var root = new GameObject(name);
        var metadata = root.AddComponent<V0135MechanicalEnemyPrototype>();
        metadata.version = Version;
        metadata.family = family;
        metadata.scaleMeters = scaleMeters;
        metadata.silhouetteTag = tag;
        metadata.gameplayAuthority = "VisualOnlyNoGameplay";
        metadata.animationReady = true;
        return root;
    }

    private static void BuildScrapper()
    {
        var root = Root("ENEMY_V0135_Scrapper_MechanicalPack.prefab", "Scrapper", 1.55f, "low fast saw-claw silhouette");
        Part(root.transform, "TORSO_CAGE_Scrapper_RivetedBoiler", "MESH_V0135_TorsoCageTall", "MAT_V0135_AgedBrass", V3(0, 0.92f, 0), V3(0, 0, 0), V3(0.82f, 0.78f, 0.74f));
        LimbSet(root.transform, "Scrapper", 0.55f, 0.48f, 0.35f, true);
        Tool(root.transform, "SAW_TOOL_LeftForearm", "MESH_V0135_SawDisc", "MAT_V0135_BlackenedIron", V3(-0.62f, 0.82f, -0.18f), V3(0, 0, 90), V3(0.9f, 0.9f, 0.9f));
        Tool(root.transform, "CLAW_TOOL_RightForearm", "MESH_V0135_ClawBlade", "MAT_V0135_BlackenedIron", V3(0.68f, 0.72f, -0.18f), V3(0, 0, -32), V3(0.7f, 0.95f, 0.7f));
        Signals(root.transform, "Scrapper", V3(0, 1.35f, -0.34f), V3(0.34f, 1.1f, -0.36f));
        Fragments(root.transform, "Scrapper", 5);
        Sockets(root.transform, new[] { "SOCK_Hips", "SOCK_SpineCage", "SOCK_LeftSaw", "SOCK_RightClaw", "SOCK_WeakLamp", "SOCK_ShutdownBurst" });
        Tag(root.transform, "TAG_SCRAPPER_FAST_SAW", V3(0, 1.92f, -0.42f));
        SavePrefab(root);
    }

    private static void BuildLancer()
    {
        var root = Root("ENEMY_V0135_Lancer_MechanicalPack.prefab", "Lancer", 1.9f, "tall forward lance and cyan charge coil");
        Part(root.transform, "TORSO_CAGE_Lancer_NarrowPressureFrame", "MESH_V0135_TorsoCageTall", "MAT_V0135_BlackenedIron", V3(0, 1.08f, 0), V3(0, 0, 0), V3(0.72f, 0.98f, 0.65f));
        LimbSet(root.transform, "Lancer", 0.72f, 0.62f, 0.48f, false);
        Tool(root.transform, "LANCE_TOOL_RightChargeNeedle", "MESH_V0135_LanceNeedle", "MAT_V0135_BlackenedIron", V3(0.88f, 1.12f, -0.38f), V3(0, 0, -62), V3(1.0f, 1.0f, 1.0f));
        CoilStack(root.transform, "CHARGE_COILS_Lancer_Backpack", V3(-0.18f, 1.25f, 0.42f), 3);
        Signals(root.transform, "Lancer", V3(0, 1.62f, -0.36f), V3(0.46f, 1.38f, -0.38f));
        Fragments(root.transform, "Lancer", 5);
        Sockets(root.transform, new[] { "SOCK_Hips", "SOCK_SpineCage", "SOCK_RightLance", "SOCK_CoilBackpack", "SOCK_FireTell", "SOCK_ShutdownBurst" });
        Tag(root.transform, "TAG_LANCER_AIMING_SPIRE", V3(0, 2.25f, -0.42f));
        SavePrefab(root);
    }

    private static void BuildBulwark()
    {
        var root = Root("ENEMY_V0135_Bulwark_MechanicalPack.prefab", "Bulwark", 2.15f, "wide shield wall with hammer shoulder");
        Part(root.transform, "TORSO_CAGE_Bulwark_HeavyBoiler", "MESH_V0135_TorsoCageHeavy", "MAT_V0135_BlackenedIron", V3(0, 1.12f, 0), V3(0, 0, 0), V3(0.98f, 0.98f, 0.92f));
        LimbSet(root.transform, "Bulwark", 0.86f, 0.72f, 0.58f, false);
        Part(root.transform, "SHIELD_PLATE_LeftRivetedDoor", "MESH_V0135_ShieldPlate", "MAT_V0135_AgedBrass", V3(-0.9f, 1.12f, -0.48f), V3(0, 7, 0), V3(1.05f, 1.1f, 1));
        Tool(root.transform, "HAMMER_TOOL_RightSteamMaul", "MESH_V0135_HammerHead", "MAT_V0135_BlackenedIron", V3(0.95f, 0.88f, -0.35f), V3(0, 0, -8), V3(1.2f, 1.1f, 1.0f));
        Signals(root.transform, "Bulwark", V3(0, 1.58f, -0.52f), V3(-0.88f, 1.56f, -0.58f));
        Fragments(root.transform, "Bulwark", 7);
        Sockets(root.transform, new[] { "SOCK_Hips", "SOCK_SpineCage", "SOCK_LeftShield", "SOCK_RightHammer", "SOCK_GuardBreakLamp", "SOCK_ShutdownBurst" });
        Tag(root.transform, "TAG_BULWARK_WIDE_SHIELD", V3(0, 2.38f, -0.56f));
        SavePrefab(root);
    }

    private static void BuildWarden()
    {
        var root = Root("ENEMY_V0135_Warden_MechanicalPack.prefab", "Warden", 2.35f, "tall command cage and twin bolt coils");
        Part(root.transform, "TORSO_CAGE_Warden_CommandGovernor", "MESH_V0135_TorsoCageWide", "MAT_V0135_AgedBrass", V3(0, 1.36f, 0), V3(0, 0, 0), V3(1.08f, 1.0f, 0.96f));
        LimbSet(root.transform, "Warden", 0.74f, 0.82f, 0.64f, false);
        CoilStack(root.transform, "CHARGE_COILS_Warden_Left", V3(-0.56f, 1.62f, 0.38f), 4);
        CoilStack(root.transform, "CHARGE_COILS_Warden_Right", V3(0.56f, 1.62f, 0.38f), 4);
        Tool(root.transform, "CLAW_TOOL_Warden_OrderPincer", "MESH_V0135_ClawBlade", "MAT_V0135_BlackenedIron", V3(-0.82f, 1.05f, -0.28f), V3(0, 0, 28), V3(0.85f, 1.2f, 0.85f));
        Tool(root.transform, "HAMMER_TOOL_Warden_GavelArm", "MESH_V0135_HammerHead", "MAT_V0135_BlackenedIron", V3(0.9f, 1.02f, -0.28f), V3(0, 0, -18), V3(0.9f, 0.9f, 0.9f));
        Signals(root.transform, "Warden", V3(0, 1.85f, -0.5f), V3(0, 1.36f, -0.54f));
        Fragments(root.transform, "Warden", 8);
        Sockets(root.transform, new[] { "SOCK_Hips", "SOCK_SpineCage", "SOCK_LeftCoil", "SOCK_RightCoil", "SOCK_CommandLamp", "SOCK_ShutdownBurst" });
        Tag(root.transform, "TAG_WARDEN_COMMAND_TOWER", V3(0, 2.62f, -0.58f));
        SavePrefab(root);
    }

    private static void BuildFoundryOverseer()
    {
        var root = Root("ENEMY_V0135_FoundryOverseer_Elite_MechanicalPack.prefab", "FoundryOverseerElite", 2.75f, "elite miniboss tri-tool furnace silhouette");
        Part(root.transform, "TORSO_CAGE_Overseer_TriBoiler", "MESH_V0135_TorsoCageHeavy", "MAT_V0135_AgedBrass", V3(0, 1.52f, 0), V3(0, 0, 0), V3(1.22f, 1.18f, 1.05f));
        LimbSet(root.transform, "Overseer", 0.92f, 0.92f, 0.76f, false);
        Part(root.transform, "SHIELD_PLATE_Overseer_CenterApron", "MESH_V0135_ShieldPlate", "MAT_V0135_HazardTrim", V3(0, 1.25f, -0.64f), V3(0, 0, 0), V3(0.8f, 1.0f, 1));
        Tool(root.transform, "SAW_TOOL_Overseer_Left", "MESH_V0135_SawDisc", "MAT_V0135_BlackenedIron", V3(-1.05f, 1.15f, -0.38f), V3(0, 0, 90), V3(1.12f, 1.12f, 1.12f));
        Tool(root.transform, "HAMMER_TOOL_Overseer_Right", "MESH_V0135_HammerHead", "MAT_V0135_BlackenedIron", V3(1.1f, 1.05f, -0.36f), V3(0, 0, -10), V3(1.25f, 1.15f, 1.0f));
        Tool(root.transform, "LANCE_TOOL_Overseer_BackSpine", "MESH_V0135_LanceNeedle", "MAT_V0135_BlackenedIron", V3(0, 2.1f, 0.26f), V3(32, 0, 0), V3(1.2f, 1.2f, 1.2f));
        CoilStack(root.transform, "CHARGE_COILS_Overseer_Crown", V3(0, 2.0f, 0.42f), 5);
        Signals(root.transform, "Overseer", V3(0, 2.0f, -0.58f), V3(0, 1.48f, -0.68f));
        Fragments(root.transform, "Overseer", 10);
        Sockets(root.transform, new[] { "SOCK_Hips", "SOCK_SpineCage", "SOCK_LeftSaw", "SOCK_RightHammer", "SOCK_BackLance", "SOCK_CrownCoil", "SOCK_BossWeakLamp", "SOCK_ShutdownBurst" });
        Tag(root.transform, "TAG_ELITE_FOUNDRY_OVERSEER", V3(0, 3.02f, -0.72f));
        SavePrefab(root);
    }

    private static void LimbSet(Transform root, string family, float x, float armY, float legY, bool crouched)
    {
        var armScale = crouched ? V3(0.82f, 0.72f, 0.82f) : V3(0.92f, 0.92f, 0.92f);
        Part(root, $"ARM_L_{family}_PistonUpper", "MESH_V0135_LimbBar", "MAT_V0135_BlackenedIron", V3(-x, armY + 0.45f, 0), V3(0, 0, -18), armScale);
        Part(root, $"ARM_R_{family}_PistonUpper", "MESH_V0135_LimbBar", "MAT_V0135_BlackenedIron", V3(x, armY + 0.45f, 0), V3(0, 0, 18), armScale);
        Part(root, $"LEG_L_{family}_HydraulicStilt", "MESH_V0135_LimbBar", "MAT_V0135_BlackenedIron", V3(-x * 0.45f, legY, 0.06f), V3(0, 0, crouched ? -8 : -3), V3(0.9f, 0.82f, 0.9f));
        Part(root, $"LEG_R_{family}_HydraulicStilt", "MESH_V0135_LimbBar", "MAT_V0135_BlackenedIron", V3(x * 0.45f, legY, 0.06f), V3(0, 0, crouched ? 8 : 3), V3(0.9f, 0.82f, 0.9f));
        Part(root, $"PISTON_L_{family}_ExposedRod", "MESH_V0135_PistonRod", "MAT_V0135_AgedBrass", V3(-x * 0.35f, legY + 0.38f, -0.2f), V3(0, 0, -8), V3(1, 1, 1));
        Part(root, $"PISTON_R_{family}_ExposedRod", "MESH_V0135_PistonRod", "MAT_V0135_AgedBrass", V3(x * 0.35f, legY + 0.38f, -0.2f), V3(0, 0, 8), V3(1, 1, 1));
    }

    private static void Signals(Transform root, string family, Vector3 lampPos, Vector3 tellPos)
    {
        Part(root, $"WEAK_POINT_LAMP_{family}_FurnaceEye", "MESH_V0135_LampOrb", "MAT_V0135_AmberFurnaceLamp", lampPos, Vector3.zero, V3(1.0f, 1.0f, 1.0f));
        Part(root, $"WEAK_POINT_LAMP_{family}_BreakGlass", "MESH_V0135_LampOrb", "MAT_V0135_WeakPointRedGlass", lampPos + V3(0.22f, -0.18f, -0.02f), Vector3.zero, V3(0.72f, 0.72f, 0.72f));
        Part(root, $"BOLT_TELL_{family}_CyanRead", "MESH_V0135_CoilRing", "MAT_V0135_CyanBoltTell", tellPos, V3(90, 0, 0), V3(0.9f, 0.9f, 0.9f));
        for (var i = 0; i < 8; i++)
        {
            var angle = i * Mathf.PI * 2f / 8f;
            Part(root, $"RIVET_{family}_{i:00}", "MESH_V0135_Rivet", "MAT_V0135_AgedBrass", lampPos + V3(Mathf.Cos(angle) * 0.34f, Mathf.Sin(angle) * 0.24f, 0.02f), Vector3.zero, V3(1, 1, 1));
        }
    }

    private static void CoilStack(Transform root, string name, Vector3 center, int count)
    {
        for (var i = 0; i < count; i++)
        {
            Part(root, $"{name}_{i:00}", "MESH_V0135_CoilRing", i % 2 == 0 ? "MAT_V0135_CyanBoltTell" : "MAT_V0135_AgedBrass", center + V3(0, (i - count * 0.5f) * 0.14f, 0), V3(90, 0, 0), V3(0.82f + i * 0.03f, 0.82f + i * 0.03f, 0.82f));
        }
    }

    private static void Fragments(Transform root, string family, int count)
    {
        for (var i = 0; i < count; i++)
        {
            var offset = V3((i - count * 0.5f) * 0.16f, 0.12f + (i % 3) * 0.08f, 0.48f + (i % 2) * 0.08f);
            Part(root, $"SHUTDOWN_FRAGMENT_{family}_{i:00}", "MESH_V0135_ShutdownFragment", i % 2 == 0 ? "MAT_V0135_ShutdownDimMetal" : "MAT_V0135_SootWear", offset, V3(i * 19, i * 31, i * 13), V3(0.48f, 0.48f, 0.48f));
        }
    }

    private static void Sockets(Transform root, IEnumerable<string> names)
    {
        foreach (var socket in names)
        {
            var go = new GameObject(socket);
            go.transform.SetParent(root, false);
            go.transform.localPosition = SocketPosition(socket);
        }
    }

    private static Vector3 SocketPosition(string socket)
    {
        if (socket.Contains("Hips")) return V3(0, 0.72f, 0);
        if (socket.Contains("Spine")) return V3(0, 1.45f, 0);
        if (socket.Contains("Left")) return V3(-0.75f, 1.25f, -0.28f);
        if (socket.Contains("Right")) return V3(0.75f, 1.25f, -0.28f);
        if (socket.Contains("Back") || socket.Contains("Coil")) return V3(0, 1.65f, 0.42f);
        if (socket.Contains("Lamp") || socket.Contains("Weak")) return V3(0, 1.55f, -0.48f);
        return V3(0, 1.0f, 0.0f);
    }

    private static GameObject Part(Transform root, string name, string mesh, string mat, Vector3 pos, Vector3 euler, Vector3 scale)
    {
        var go = new GameObject(name);
        go.transform.SetParent(root, false);
        go.transform.localPosition = pos;
        go.transform.localEulerAngles = euler;
        go.transform.localScale = scale;
        go.AddComponent<MeshFilter>().sharedMesh = Meshes[mesh];
        go.AddComponent<MeshRenderer>().sharedMaterial = Mats[mat];
        return go;
    }

    private static GameObject Tool(Transform root, string name, string mesh, string mat, Vector3 pos, Vector3 euler, Vector3 scale)
    {
        var tool = Part(root, name, mesh, mat, pos, euler, scale);
        var grip = Part(tool.transform, $"{name}_LeatherGrip", "MESH_V0135_PistonRod", "MAT_V0135_OilyLeather", V3(0, -0.38f, 0), V3(0, 0, 0), V3(0.85f, 0.7f, 0.85f));
        grip.transform.SetParent(tool.transform, false);
        return tool;
    }

    private static void Tag(Transform root, string label, Vector3 pos)
    {
        Part(root, label, "MESH_V0135_TagPanel", "MAT_V0135_SilhouetteTagInk", pos, Vector3.zero, V3(1, 1, 1));
    }

    private static void SavePrefab(GameObject root)
    {
        PrefabUtility.SaveAsPrefabAsset(root, $"{PackRoot}/Prefabs/{root.name}");
        UnityEngine.Object.DestroyImmediate(root);
    }

    private static Mesh MeshBox(Vector3 size)
    {
        var x = size.x * 0.5f; var y = size.y * 0.5f; var z = size.z * 0.5f;
        var v = new[]
        {
            V3(-x,-y,-z), V3(x,-y,-z), V3(x,y,-z), V3(-x,y,-z),
            V3(-x,-y,z), V3(x,-y,z), V3(x,y,z), V3(-x,y,z)
        };
        var t = new[] { 0, 2, 1, 0, 3, 2, 1, 2, 6, 1, 6, 5, 4, 5, 6, 4, 6, 7, 0, 4, 7, 0, 7, 3, 3, 7, 6, 3, 6, 2, 0, 1, 5, 0, 5, 4 };
        return Mesh(v, t);
    }

    private static Mesh MeshWedge(float width, float height, float depth)
    {
        var x = width * 0.5f; var z = depth * 0.5f;
        var v = new[] { V3(-x, -height * 0.5f, -z), V3(x, -height * 0.5f, -z), V3(-x, -height * 0.5f, z), V3(x, -height * 0.5f, z), V3(0, height * 0.5f, -z), V3(0, height * 0.5f, z) };
        var t = new[] { 0, 4, 1, 2, 3, 5, 0, 2, 5, 0, 5, 4, 1, 4, 5, 1, 5, 3, 0, 1, 3, 0, 3, 2 };
        return Mesh(v, t);
    }

    private static Mesh MeshCone(float radius, float height, int segments)
    {
        var verts = new List<Vector3> { V3(0, height * 0.5f, 0), V3(0, -height * 0.5f, 0) };
        var tris = new List<int>();
        for (var i = 0; i < segments; i++)
        {
            var a = Mathf.PI * 2f * i / segments;
            verts.Add(V3(Mathf.Cos(a) * radius, -height * 0.5f, Mathf.Sin(a) * radius));
        }
        for (var i = 0; i < segments; i++)
        {
            var next = (i + 1) % segments;
            tris.AddRange(new[] { 0, 2 + i, 2 + next, 1, 2 + next, 2 + i });
        }
        return Mesh(verts.ToArray(), tris.ToArray());
    }

    private static Mesh MeshCylinder(float radius, float height, int segments)
    {
        var verts = new List<Vector3>();
        var tris = new List<int>();
        for (var y = 0; y < 2; y++)
        {
            for (var i = 0; i < segments; i++)
            {
                var a = Mathf.PI * 2f * i / segments;
                verts.Add(V3(Mathf.Cos(a) * radius, y == 0 ? -height * 0.5f : height * 0.5f, Mathf.Sin(a) * radius));
            }
        }
        verts.Add(V3(0, -height * 0.5f, 0));
        verts.Add(V3(0, height * 0.5f, 0));
        var bottom = segments * 2;
        var top = bottom + 1;
        for (var i = 0; i < segments; i++)
        {
            var next = (i + 1) % segments;
            tris.AddRange(new[] { i, segments + i, segments + next, i, segments + next, next, bottom, next, i, top, segments + i, segments + next });
        }
        return Mesh(verts.ToArray(), tris.ToArray());
    }

    private static Mesh MeshTorus(float major, float minor, int majorSegments, int minorSegments)
    {
        var verts = new List<Vector3>();
        var tris = new List<int>();
        for (var i = 0; i < majorSegments; i++)
        {
            var a = Mathf.PI * 2f * i / majorSegments;
            var center = V3(Mathf.Cos(a) * major, 0, Mathf.Sin(a) * major);
            for (var j = 0; j < minorSegments; j++)
            {
                var b = Mathf.PI * 2f * j / minorSegments;
                verts.Add(center + V3(Mathf.Cos(a) * Mathf.Cos(b) * minor, Mathf.Sin(b) * minor, Mathf.Sin(a) * Mathf.Cos(b) * minor));
            }
        }
        for (var i = 0; i < majorSegments; i++)
        {
            for (var j = 0; j < minorSegments; j++)
            {
                var a = i * minorSegments + j;
                var b = ((i + 1) % majorSegments) * minorSegments + j;
                var c = ((i + 1) % majorSegments) * minorSegments + (j + 1) % minorSegments;
                var d = i * minorSegments + (j + 1) % minorSegments;
                tris.AddRange(new[] { a, b, c, a, c, d });
            }
        }
        return Mesh(verts.ToArray(), tris.ToArray());
    }

    private static Mesh MeshUvSphere(float radius, int longitude, int latitude)
    {
        var verts = new List<Vector3>();
        var tris = new List<int>();
        for (var y = 0; y <= latitude; y++)
        {
            var v = y / (float)latitude;
            var phi = v * Mathf.PI;
            for (var x = 0; x <= longitude; x++)
            {
                var u = x / (float)longitude;
                var theta = u * Mathf.PI * 2f;
                verts.Add(V3(Mathf.Sin(phi) * Mathf.Cos(theta), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(theta)) * radius);
            }
        }
        for (var y = 0; y < latitude; y++)
        {
            for (var x = 0; x < longitude; x++)
            {
                var a = y * (longitude + 1) + x;
                var b = a + longitude + 1;
                tris.AddRange(new[] { a, b, a + 1, a + 1, b, b + 1 });
            }
        }
        return Mesh(verts.ToArray(), tris.ToArray());
    }

    private static Mesh Mesh(Vector3[] vertices, int[] triangles)
    {
        var mesh = new Mesh { vertices = vertices, triangles = triangles };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    private static Vector3 V3(float x, float y, float z) => new Vector3(x, y, z);

    private static void WriteDocumentation()
    {
        var manifest = @"{
  ""version"": ""v0.1.35"",
  ""packId"": ""V0_1_35_MechanicalEnemyPack"",
  ""unityOnlyProof"": true,
  ""gameplayAuthority"": ""VisualOnlyNoGameplay"",
  ""families"": [
    { ""id"": ""Scrapper"", ""scaleMeters"": 1.55, ""silhouette"": ""low fast saw-claw"", ""primaryTools"": [""saw"", ""claw""], ""weakPoints"": [""furnace eye"", ""break glass""] },
    { ""id"": ""Lancer"", ""scaleMeters"": 1.90, ""silhouette"": ""tall forward lance"", ""primaryTools"": [""lance"", ""cyan charge coil""], ""weakPoints"": [""head lamp"", ""coil backpack""] },
    { ""id"": ""Bulwark"", ""scaleMeters"": 2.15, ""silhouette"": ""wide shield wall"", ""primaryTools"": [""shield"", ""steam hammer""], ""weakPoints"": [""guard-break lamp"", ""shield lamp""] },
    { ""id"": ""Warden"", ""scaleMeters"": 2.35, ""silhouette"": ""command tower with twin coils"", ""primaryTools"": [""gavel"", ""pincer"", ""twin bolt coils""], ""weakPoints"": [""command lamp"", ""center lamp""] },
    { ""id"": ""FoundryOverseerElite"", ""scaleMeters"": 2.75, ""silhouette"": ""elite tri-tool furnace miniboss"", ""primaryTools"": [""saw"", ""hammer"", ""back lance"", ""crown coils""], ""weakPoints"": [""boss furnace lamp"", ""center apron lamp""] }
  ],
  ""materials"": [""aged brass"", ""blackened iron"", ""oily leather"", ""grimy glass"", ""amber furnace lamps"", ""cyan/blue bolt tells"", ""soot/wear"", ""hazard trims""],
  ""acceptanceGates"": [
    ""Each prefab remains visual-only and includes socket placeholders."",
    ""All readable attack tells use cyan/blue emission and weak-point lamps use amber/red emission."",
    ""Collider guidance supports capsule body plus simple box/sphere tool triggers."",
    ""LOD0/LOD1/LOD2 reduction targets are documented before gameplay promotion.""
  ]
}";
        File.WriteAllText($"{PackRoot}/Metadata/V0_1_35_MechanicalEnemyPack_Manifest.json", manifest);
        File.WriteAllText($"{DocRoot}/V0_1_35_MechanicalEnemyPack_Manifest.json", manifest);

        File.WriteAllText($"{DocRoot}/V0_1_35_MechanicalEnemyPack_IntegrationNotes.md",
            "# v0.1.35 Mechanical Enemy Pack\n\n" +
            "Unity-only staging package for the steampunk/brassworks enemy direction. The pack contains integration-ready proxy meshes, prefab assemblies, material recipes, sockets, shutdown fragments, and rendered proof sheets.\n\n" +
            "## Families\n\n" +
            "- Scrapper: 1.55 m low fast profile, left saw, right claw, crouched piston legs.\n" +
            "- Lancer: 1.90 m narrow spire profile, forward lance, cyan backpack coils.\n" +
            "- Bulwark: 2.15 m shield-wall profile, heavy boiler cage, shield plate, steam hammer.\n" +
            "- Warden: 2.35 m command profile, twin charge coils, pincer/gavel arms.\n" +
            "- Foundry Overseer Elite: 2.75 m miniboss silhouette with saw, hammer, back lance, crown coils.\n\n" +
            "## Rigging Sockets\n\n" +
            "Socket placeholders are named with `SOCK_` and should be preserved when replacing proxies with final rigged meshes. Key sockets: hips, spine cage, tool hands, coil/backpack, weak lamp, and shutdown burst.\n\n" +
            "## Animation Readiness\n\n" +
            "Keep torso cages as stable root masses, arms/legs as separated piston bars, tools as child transforms, and coils/lamps as independent children for readable anticipation, damage, and shutdown animation. Shutdown fragments are staged near the feet/back for burst or crumble effects.\n\n" +
            "## LOD And Collider Guidance\n\n" +
            "LOD0 keeps all rivets, coils, tags, weak lamps, and shutdown fragments. LOD1 may remove every other rivet and simplify coil stacks. LOD2 should collapse tools to broad silhouette meshes while preserving cyan tell and amber weak lamp cards. Use one capsule or box for body collision, separate simple boxes/spheres for shield, hammer/saw/lance damage windows, and one small sphere trigger around each weak-point lamp.\n\n" +
            "## Material Recipes\n\n" +
            "- Aged brass: warm metallic brass, medium smoothness, soot masks on concave seams.\n" +
            "- Blackened iron: dark high-metal body metal with low-mid smoothness, dry edge wear.\n" +
            "- Oily leather: dark brown low-metal grips, higher smoothness, uneven oil shine.\n" +
            "- Grimy glass: green-grey transparent read for lenses and lamp covers.\n" +
            "- Amber furnace lamps: emissive orange weak-point heat language.\n" +
            "- Cyan/blue bolt tells: emissive charge and ranged-attack readability.\n" +
            "- Soot/wear: almost-black grime for cavities, feet, shutdown fragments, and silhouettes.\n" +
            "- Hazard trims: yellow enamel/paint for shield and miniboss danger accents.\n\n" +
            "## Acceptance Gates\n\n" +
            "- Five prefabs import under `Assets/_Project/ArtStaging/V0_1_35_MechanicalEnemyPack/Prefabs` with no gameplay references.\n" +
            "- Material count covers the eight required recipes plus weak/shutdown/tag utility materials.\n" +
            "- Each family has torso cage, limbs, a role-defining tool, weak-point lamps, shutdown fragments, and a silhouette tag.\n" +
            "- Preview sheets exist under `Documentation/ConceptRenders/V0_1_35_MechanicalEnemyPack/` and are generated from Unity, not external DCC.\n");
    }

    private static void RenderPreviewSheets()
    {
        RenderContactSheet("CONTACTSHEET_V0135_MechanicalEnemyPack_Lineup_UnityProof.png", new[] {
            "ENEMY_V0135_Scrapper_MechanicalPack.prefab",
            "ENEMY_V0135_Lancer_MechanicalPack.prefab",
            "ENEMY_V0135_Bulwark_MechanicalPack.prefab",
            "ENEMY_V0135_Warden_MechanicalPack.prefab",
            "ENEMY_V0135_FoundryOverseer_Elite_MechanicalPack.prefab"
        });
        RenderMaterialSheet("CONTACTSHEET_V0135_MechanicalEnemyPack_MaterialRecipes_UnityProof.png");
    }

    private static void RenderContactSheet(string file, string[] prefabNames)
    {
        var width = 1600;
        var height = 900;
        var rt = new RenderTexture(width, height, 24);
        var sceneRoot = new GameObject("V0135_RenderScene");
        var camObj = new GameObject("V0135_RenderCamera");
        var cam = camObj.AddComponent<Camera>();
        cam.targetTexture = rt;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.09f, 0.085f, 0.075f);
        cam.fieldOfView = 32f;
        cam.transform.position = V3(0, 1.7f, -9.8f);
        cam.transform.LookAt(V3(0, 1.25f, 0));
        var lightObj = new GameObject("V0135_KeyLight");
        var light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.25f;
        light.transform.rotation = Quaternion.Euler(50, -32, 0);

        for (var i = 0; i < prefabNames.Length; i++)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{PackRoot}/Prefabs/{prefabNames[i]}");
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            instance.transform.SetParent(sceneRoot.transform, false);
            instance.transform.position = V3((i - 2) * 1.9f, 0, 0);
            instance.transform.rotation = Quaternion.Euler(0, 18, 0);
        }

        SaveCameraPng(cam, rt, $"{RenderRoot}/{file}");
        UnityEngine.Object.DestroyImmediate(sceneRoot);
        UnityEngine.Object.DestroyImmediate(camObj);
        UnityEngine.Object.DestroyImmediate(lightObj);
        UnityEngine.Object.DestroyImmediate(rt);
    }

    private static void RenderMaterialSheet(string file)
    {
        var width = 1200;
        var height = 700;
        var rt = new RenderTexture(width, height, 24);
        var camObj = new GameObject("V0135_MatCamera");
        var cam = camObj.AddComponent<Camera>();
        cam.targetTexture = rt;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.075f, 0.07f, 0.065f);
        cam.fieldOfView = 35f;
        cam.transform.position = V3(0, 1.1f, -6f);
        cam.transform.LookAt(V3(0, 0.7f, 0));
        var lightObj = new GameObject("V0135_MatLight");
        var light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.35f;
        light.transform.rotation = Quaternion.Euler(45, -25, 0);
        var keys = new List<string>(Mats.Keys);
        var swatches = new List<GameObject>();
        for (var i = 0; i < keys.Count; i++)
        {
            var go = new GameObject($"SWATCH_{keys[i]}");
            go.transform.position = V3((i % 4 - 1.5f) * 1.1f, 1.4f - (i / 4) * 0.9f, 0);
            go.AddComponent<MeshFilter>().sharedMesh = Meshes["MESH_V0135_LampOrb"];
            go.AddComponent<MeshRenderer>().sharedMaterial = Mats[keys[i]];
            swatches.Add(go);
        }
        SaveCameraPng(cam, rt, $"{RenderRoot}/{file}");
        foreach (var go in swatches)
        {
            UnityEngine.Object.DestroyImmediate(go);
        }
        UnityEngine.Object.DestroyImmediate(camObj);
        UnityEngine.Object.DestroyImmediate(lightObj);
        UnityEngine.Object.DestroyImmediate(rt);
    }

    private static void SaveCameraPng(Camera cam, RenderTexture rt, string path)
    {
        cam.Render();
        RenderTexture.active = rt;
        var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(tex);
        RenderTexture.active = null;
    }
}
