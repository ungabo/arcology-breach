using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class UnityWeaponAssemblyLookdevRenderer
{
    private const int MainWidth = 1800;
    private const int MainHeight = 1200;
    private static readonly Color Background = new Color(0.045f, 0.043f, 0.04f, 1f);

    private static Material agedBrass;
    private static Material brightWornBrass;
    private static Material copper;
    private static Material darkIron;
    private static Material blackGrip;
    private static Material amberGlass;
    private static Material dialFace;
    private static Material grime;
    private static Material redNeedle;
    private static Material proxyGrey;
    private static Material floorMat;

    [MenuItem("Brassworks/Render Weapon Assembly Lookdev 08")]
    public static void RunFromMenu()
    {
        RunBatch();
    }

    public static void RunBatch()
    {
        var renderDir = WorkspacePath("Documentation", "ConceptRenders", "V0_1_52_UnityWeaponAssemblyLookdev");
        Directory.CreateDirectory(renderDir);

        var rendered = new List<string>();
        rendered.Add(RenderView(renderDir, "01_full_assembly_three_quarter.png", MainWidth, MainHeight, BuildCompleteAssembly, new Vector3(4.2f, 2.0f, 3.2f), new Vector3(0.0f, 0.28f, 0.0f), 34f, Lighting.Warm));
        rendered.Add(RenderView(renderDir, "02_copper_coil_closeup.png", 1600, 1100, BuildCoilInspection, new Vector3(1.25f, 0.72f, 1.12f), new Vector3(0.0f, 0.35f, 0.0f), 25f, Lighting.Warm));
        rendered.Add(RenderView(renderDir, "03_gauge_dial_closeup.png", 1600, 1100, BuildGaugeInspection, new Vector3(0.1f, 0.78f, 1.12f), new Vector3(0.0f, 0.42f, 0.0f), 21f, Lighting.Warm));
        rendered.Add(RenderView(renderDir, "04_muzzle_cluster_closeup.png", 1600, 1100, BuildMuzzleInspection, new Vector3(-1.45f, 0.62f, 1.35f), new Vector3(-0.28f, 0.08f, 0.0f), 32f, Lighting.Warm));
        rendered.Add(RenderView(renderDir, "05_partial_assembled_exploded.png", MainWidth, MainHeight, BuildExplodedAssembly, new Vector3(4.5f, 2.35f, 4.0f), new Vector3(0.0f, 0.55f, 0.0f), 36f, Lighting.Warm));
        rendered.Add(RenderView(renderDir, "06_first_person_scale_proxy.png", MainWidth, MainHeight, BuildFirstPersonProxy, new Vector3(0.15f, 0.92f, -3.55f), new Vector3(0.0f, 0.58f, 0.0f), 48f, Lighting.Neutral));
        rendered.Add(RenderView(renderDir, "07_material_stress_warm_light.png", MainWidth, MainHeight, BuildMaterialStressRack, new Vector3(4.7f, 2.1f, 3.6f), new Vector3(0.1f, 0.45f, 0.0f), 35f, Lighting.Warm));
        rendered.Add(RenderView(renderDir, "08_material_stress_cool_light.png", MainWidth, MainHeight, BuildMaterialStressRack, new Vector3(4.7f, 2.1f, 3.6f), new Vector3(0.1f, 0.45f, 0.0f), 35f, Lighting.Cool));
        rendered.Add(RenderContactSheet(renderDir, "09_contact_sheet.png", rendered));

        Debug.Log("UNITY_WEAPON_ASSEMBLY_LOOKDEV_RENDERED:" + string.Join("|", rendered));
        AssetDatabase.Refresh();
    }

    private static string RenderView(string renderDir, string fileName, int width, int height, Action build, Vector3 cameraPosition, Vector3 target, float fov, Lighting lighting)
    {
        ResetScene();
        CreateMaterials();
        SetupEnvironment(lighting);
        build();

        var cameraObject = new GameObject("Lookdev Camera");
        var camera = cameraObject.AddComponent<Camera>();
        camera.transform.position = cameraPosition;
        camera.transform.LookAt(target);
        camera.fieldOfView = fov;
        camera.nearClipPlane = 0.02f;
        camera.farClipPlane = 100f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Background;
        camera.allowHDR = true;
        camera.allowMSAA = true;

        var path = Path.Combine(renderDir, fileName);
        RenderCameraToPng(camera, path, width, height);
        return path;
    }

    private static void ResetScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        SceneManager.SetActiveScene(scene);
        RenderSettings.ambientMode = AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.22f, 0.20f, 0.17f);
        RenderSettings.ambientEquatorColor = new Color(0.11f, 0.105f, 0.10f);
        RenderSettings.ambientGroundColor = new Color(0.035f, 0.034f, 0.032f);
        QualitySettings.antiAliasing = 8;
    }

    private static void SetupEnvironment(Lighting lighting)
    {
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Charcoal review plinth";
        floor.transform.position = new Vector3(0f, -0.025f, 0f);
        floor.transform.localScale = new Vector3(8f, 1f, 5f);
        floor.GetComponent<Renderer>().sharedMaterial = floorMat;

        AddLight("Key", LightType.Directional, lighting == Lighting.Cool ? new Color(0.70f, 0.82f, 1.0f) : new Color(1.0f, 0.76f, 0.48f), 2.25f, new Vector3(42f, -35f, 0f), new Vector3(-2f, 4f, 3f));
        AddLight("Rim", LightType.Directional, lighting == Lighting.Cool ? new Color(0.45f, 0.62f, 0.92f) : new Color(0.55f, 0.78f, 1.0f), 1.35f, new Vector3(16f, 145f, 0f), new Vector3(3f, 3f, -4f));
        AddLight("Amber Bounce", LightType.Point, new Color(1.0f, 0.52f, 0.18f), lighting == Lighting.Cool ? 0.8f : 1.6f, Vector3.zero, new Vector3(-1.1f, 0.75f, 0.75f));
    }

    private static void AddLight(string name, LightType type, Color color, float intensity, Vector3 euler, Vector3 position)
    {
        var go = new GameObject(name + " Light");
        go.transform.position = position;
        go.transform.rotation = Quaternion.Euler(euler);
        var light = go.AddComponent<Light>();
        light.type = type;
        light.color = color;
        light.intensity = intensity;
        light.shadows = LightShadows.Soft;
        light.range = 7f;
    }

    private static void BuildCompleteAssembly()
    {
        BuildWeapon(Vector3.zero, Quaternion.identity, false, true);
    }

    private static void BuildCoilInspection()
    {
        Cylinder("dark iron barrel section through coil", Vector3.zero, 0.17f, 1.28f, Quaternion.Euler(0f, 0f, 90f), darkIron);
        BuildCoil(Vector3.zero, Quaternion.identity, 0.285f, 1.05f);
        Cylinder("left aged brass coil clamp", new Vector3(-0.58f, 0f, 0f), 0.215f, 0.08f, Quaternion.Euler(0f, 0f, 90f), agedBrass);
        Cylinder("right aged brass coil clamp", new Vector3(0.58f, 0f, 0f), 0.215f, 0.08f, Quaternion.Euler(0f, 0f, 90f), agedBrass);
        Cube("soot caught under coil", new Vector3(0f, -0.265f, -0.02f), new Vector3(0.95f, 0.035f, 0.05f), grime);
    }

    private static void BuildGaugeInspection()
    {
        var root = new GameObject("standalone gauge closeup root").transform;
        root.position = Vector3.zero;
        BuildGauge(new Vector3(0f, 0.42f, 0f), Quaternion.Euler(90f, 0f, 0f), root);
        Cylinder("amber tube behind gauge", new Vector3(0f, 0.42f, 0.24f), 0.075f, 0.82f, Quaternion.Euler(0f, 0f, 90f), amberGlass);
        Cube("small brass gauge bracket", new Vector3(0f, 0.2f, 0.18f), new Vector3(0.42f, 0.1f, 0.09f), agedBrass);
        Sphere("oil bead on lower rim", new Vector3(-0.18f, 0.25f, -0.05f), Vector3.one * 0.035f, grime);
    }

    private static void BuildMuzzleInspection()
    {
        Cylinder("dark iron muzzle barrel", Vector3.zero, 0.2f, 1.05f, Quaternion.Euler(0f, 0f, 90f), darkIron);
        Torus("large worn brass muzzle crown", new Vector3(-0.55f, 0f, 0f), 0.31f, 0.04f, agedBrass, Quaternion.Euler(0f, 90f, 0f));
        Cylinder("blackened muzzle bore", new Vector3(-0.61f, 0f, 0f), 0.12f, 0.09f, Quaternion.Euler(0f, 0f, 90f), grime);
        for (int i = 0; i < 10; i++)
        {
            float a = i * Mathf.PI * 2f / 10f;
            var pos = new Vector3(-0.48f, Mathf.Sin(a) * 0.31f, Mathf.Cos(a) * 0.31f);
            Cylinder("individual crown port", pos, 0.024f, 0.26f, Quaternion.Euler(0f, 0f, 90f), brightWornBrass);
        }
        Cube("muzzle heat grime scrape", new Vector3(-0.35f, -0.19f, 0.02f), new Vector3(0.32f, 0.035f, 0.035f), grime);
    }

    private static void BuildExplodedAssembly()
    {
        BuildWeapon(Vector3.zero, Quaternion.identity, true, true);
    }

    private static void BuildFirstPersonProxy()
    {
        BuildWeapon(new Vector3(0.08f, 0.58f, -0.15f), Quaternion.Euler(-7f, 178f, 0f), false, true);

        var leftForearm = Cube("left scale proxy forearm", new Vector3(-0.48f, 0.35f, -0.95f), new Vector3(0.18f, 0.16f, 0.95f), proxyGrey);
        leftForearm.transform.rotation = Quaternion.Euler(8f, 12f, 0f);
        var rightForearm = Cube("right scale proxy forearm", new Vector3(0.46f, 0.28f, -0.92f), new Vector3(0.18f, 0.16f, 0.92f), proxyGrey);
        rightForearm.transform.rotation = Quaternion.Euler(4f, -9f, 0f);
        Sphere("left proxy glove", new Vector3(-0.28f, 0.47f, -0.25f), new Vector3(0.28f, 0.18f, 0.22f), blackGrip);
        Sphere("right proxy glove", new Vector3(0.24f, 0.43f, -0.28f), new Vector3(0.24f, 0.17f, 0.21f), blackGrip);

        for (int i = -3; i <= 3; i++)
        {
            var line = Cube("scale grid line", new Vector3(i * 0.35f, 0.001f, 0.15f), new Vector3(0.01f, 0.01f, 2.0f), grime);
            line.transform.position += Vector3.down * 0.02f;
        }
    }

    private static void BuildMaterialStressRack()
    {
        BuildWeapon(new Vector3(0f, 0f, 0f), Quaternion.identity, false, true);
        Sphere("loose amber pressure glass", new Vector3(-1.15f, 0.28f, -0.82f), new Vector3(0.28f, 0.28f, 0.28f), amberGlass);
        Cylinder("loose brass valve cap", new Vector3(-0.15f, 0.22f, -0.95f), 0.22f, 0.14f, Quaternion.Euler(90f, 0f, 0f), agedBrass);
        Torus("loose dark iron muzzle crown", new Vector3(0.9f, 0.24f, -0.93f), 0.26f, 0.035f, darkIron, Quaternion.Euler(90f, 0f, 0f));
        BuildCoil(new Vector3(1.65f, 0.23f, -0.88f), Quaternion.Euler(0f, 0f, 0f), 0.24f, 0.63f);
    }

    private static void BuildWeapon(Vector3 offset, Quaternion rotation, bool exploded, bool grimePass)
    {
        Transform root = new GameObject(exploded ? "Exploded pressure pistol assembly" : "Pressure pistol assembly").transform;
        root.position = offset;
        root.rotation = rotation;

        var barrelOffset = exploded ? new Vector3(-0.65f, 0.24f, 0f) : Vector3.zero;
        var receiverOffset = exploded ? new Vector3(0.14f, -0.04f, 0f) : Vector3.zero;
        var gaugeOffset = exploded ? new Vector3(0.22f, 0.44f, 0.25f) : Vector3.zero;
        var coilOffset = exploded ? new Vector3(0.12f, 0.18f, -0.35f) : Vector3.zero;

        Parent(Cylinder("dark iron barrel sleeve", new Vector3(-1.05f, 0.5f, 0f) + barrelOffset, 0.17f, 2.35f, Quaternion.Euler(0f, 0f, 90f), darkIron), root);
        Parent(Cylinder("inner stained muzzle bore", new Vector3(-2.34f, 0.5f, 0f) + barrelOffset, 0.105f, 0.16f, Quaternion.Euler(0f, 0f, 90f), grime), root);
        Parent(Torus("crowned muzzle ring", new Vector3(-2.23f, 0.5f, 0f) + barrelOffset, 0.23f, 0.035f, agedBrass, Quaternion.Euler(0f, 90f, 0f)), root);
        for (int i = 0; i < 8; i++)
        {
            float a = i * Mathf.PI * 2f / 8f;
            var pos = new Vector3(-2.22f, 0.5f + Mathf.Sin(a) * 0.23f, Mathf.Cos(a) * 0.23f) + barrelOffset;
            Parent(Cylinder("muzzle crown tooth", pos, 0.023f, 0.18f, Quaternion.Euler(0f, 0f, 90f), brightWornBrass), root);
        }

        Parent(Cube("dark iron receiver block", new Vector3(0.2f, 0.45f, 0f) + receiverOffset, new Vector3(1.55f, 0.46f, 0.55f), darkIron), root);
        Parent(Cube("aged brass receiver side plate", new Vector3(0.05f, 0.47f, -0.291f) + receiverOffset, new Vector3(1.28f, 0.32f, 0.035f), agedBrass), root);
        Parent(Cube("worn top rail", new Vector3(-0.1f, 0.78f, 0f) + receiverOffset, new Vector3(1.35f, 0.08f, 0.22f), brightWornBrass), root);
        Parent(Cube("blackened grip", new Vector3(0.7f, 0.02f, 0f) + receiverOffset, new Vector3(0.28f, 0.85f, 0.34f), blackGrip), root).transform.rotation *= Quaternion.Euler(0f, 0f, -16f);
        Parent(Cube("iron trigger guard", new Vector3(0.27f, 0.12f, -0.01f) + receiverOffset, new Vector3(0.52f, 0.07f, 0.08f), darkIron), root).transform.rotation *= Quaternion.Euler(0f, 0f, -18f);
        Parent(Cube("small brass trigger", new Vector3(0.31f, 0.18f, -0.02f) + receiverOffset, new Vector3(0.08f, 0.32f, 0.06f), brightWornBrass), root).transform.rotation *= Quaternion.Euler(0f, 0f, -18f);

        Parent(Cylinder("amber pressure glass tube", new Vector3(-0.18f, 0.88f, 0f), 0.115f, 0.86f, Quaternion.Euler(0f, 0f, 90f), amberGlass), root);
        Parent(Cylinder("left glass brass collar", new Vector3(-0.64f, 0.88f, 0f), 0.135f, 0.08f, Quaternion.Euler(0f, 0f, 90f), agedBrass), root);
        Parent(Cylinder("right glass brass collar", new Vector3(0.28f, 0.88f, 0f), 0.135f, 0.08f, Quaternion.Euler(0f, 0f, 90f), agedBrass), root);

        var coilRoot = new GameObject("copper induction coil").transform;
        coilRoot.position = coilOffset;
        Parent(coilRoot.gameObject, root);
        BuildCoil(new Vector3(-0.78f, 0.5f, 0f), Quaternion.identity, 0.255f, 0.92f, coilRoot);

        BuildGauge(new Vector3(0.45f, 0.86f, -0.32f) + gaugeOffset, Quaternion.Euler(68f, 0f, 0f), root);
        BuildValve(new Vector3(0.95f, 0.52f, 0.34f) + receiverOffset, Quaternion.Euler(0f, 90f, 0f), root);

        for (int i = 0; i < 7; i++)
        {
            float x = -0.55f + i * 0.21f;
            Parent(Sphere("raised brass rivet", new Vector3(x, 0.64f, -0.325f) + receiverOffset, Vector3.one * 0.055f, brightWornBrass), root);
        }

        if (grimePass)
        {
            Parent(Cube("oil grime under receiver", new Vector3(-0.12f, 0.205f, -0.33f) + receiverOffset, new Vector3(1.1f, 0.035f, 0.014f), grime), root);
            Parent(Cube("rubbed brass edge highlight", new Vector3(-0.08f, 0.71f, -0.333f) + receiverOffset, new Vector3(1.05f, 0.035f, 0.018f), brightWornBrass), root);
            Parent(Cube("muzzle soot streak", new Vector3(-2.08f, 0.39f, -0.03f) + barrelOffset, new Vector3(0.34f, 0.025f, 0.02f), grime), root);
        }
    }

    private static void BuildGauge(Vector3 position, Quaternion rotation, Transform parent)
    {
        var root = new GameObject("amber pressure gauge and dial").transform;
        root.position = position;
        root.rotation = rotation;
        Parent(root.gameObject, parent);

        ParentLocal(Cylinder("gauge dark iron case", Vector3.zero, 0.24f, 0.08f, Quaternion.identity, darkIron), root);
        ParentLocal(Cylinder("aged brass gauge rim", new Vector3(0f, 0.046f, 0f), 0.255f, 0.035f, Quaternion.identity, agedBrass), root);
        ParentLocal(Cylinder("warm ivory dial face", new Vector3(0f, 0.071f, 0f), 0.205f, 0.012f, Quaternion.identity, dialFace), root);
        ParentLocal(Cube("gauge red needle", new Vector3(0.055f, 0.085f, 0.035f), new Vector3(0.155f, 0.01f, 0.018f), redNeedle), root).transform.localRotation = Quaternion.Euler(0f, 28f, 0f);

        for (int i = 0; i < 12; i++)
        {
            float a = i * Mathf.PI * 2f / 12f;
            var tick = Cube("gauge tick mark", new Vector3(Mathf.Sin(a) * 0.15f, 0.088f, Mathf.Cos(a) * 0.15f), new Vector3(0.012f, 0.011f, i % 3 == 0 ? 0.055f : 0.036f), darkIron);
            tick.transform.localRotation = Quaternion.Euler(0f, a * Mathf.Rad2Deg, 0f);
            ParentLocal(tick, root);
        }
    }

    private static void BuildValve(Vector3 position, Quaternion rotation, Transform parent)
    {
        var root = new GameObject("brass valve wheel").transform;
        root.position = position;
        root.rotation = rotation;
        Parent(root.gameObject, parent);
        ParentLocal(Torus("valve wheel rim", Vector3.zero, 0.18f, 0.022f, agedBrass, Quaternion.identity), root);
        for (int i = 0; i < 4; i++)
        {
            var spoke = Cube("valve wheel spoke", Vector3.zero, new Vector3(0.31f, 0.026f, 0.026f), brightWornBrass);
            spoke.transform.localRotation = Quaternion.Euler(0f, 0f, i * 45f);
            ParentLocal(spoke, root);
        }
        ParentLocal(Sphere("valve hub", Vector3.zero, Vector3.one * 0.075f, darkIron), root);
    }

    private static void BuildCoil(Vector3 center, Quaternion rotation, float radius, float length, Transform parent = null)
    {
        var root = new GameObject("copper coil close wound helix").transform;
        root.position = center;
        root.rotation = rotation;
        if (parent != null) Parent(root.gameObject, parent);

        int segments = 44;
        int turns = 6;
        Vector3 previous = Vector3.zero;
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float angle = t * turns * Mathf.PI * 2f;
            var p = new Vector3((t - 0.5f) * length, Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius);
            if (i > 0)
            {
                ParentLocal(CylinderBetween("oxidized copper coil segment", previous, p, 0.024f, copper), root);
            }
            previous = p;
        }
    }

    private static GameObject CylinderBetween(string name, Vector3 a, Vector3 b, float radius, Material material)
    {
        Vector3 mid = (a + b) * 0.5f;
        Vector3 direction = b - a;
        var cylinder = Cylinder(name, mid, radius, direction.magnitude, Quaternion.identity, material);
        cylinder.transform.up = direction.normalized;
        return cylinder;
    }

    private static GameObject Cube(string name, Vector3 position, Vector3 scale, Material material)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.position = position;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = material;
        return go;
    }

    private static GameObject Sphere(string name, Vector3 position, Vector3 scale, Material material)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.transform.position = position;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = material;
        return go;
    }

    private static GameObject Cylinder(string name, Vector3 position, float radius, float height, Quaternion rotation, Material material)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = name;
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = new Vector3(radius * 2f, height * 0.5f, radius * 2f);
        go.GetComponent<Renderer>().sharedMaterial = material;
        return go;
    }

    private static GameObject Torus(string name, Vector3 position, float majorRadius, float minorRadius, Material material, Quaternion rotation)
    {
        var mesh = new Mesh();
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var triangles = new List<int>();
        int majorSegments = 64;
        int minorSegments = 12;
        for (int i = 0; i <= majorSegments; i++)
        {
            float u = i / (float)majorSegments * Mathf.PI * 2f;
            for (int j = 0; j <= minorSegments; j++)
            {
                float v = j / (float)minorSegments * Mathf.PI * 2f;
                var normal = new Vector3(Mathf.Cos(u) * Mathf.Cos(v), Mathf.Sin(v), Mathf.Sin(u) * Mathf.Cos(v));
                vertices.Add(new Vector3(Mathf.Cos(u) * (majorRadius + minorRadius * Mathf.Cos(v)), minorRadius * Mathf.Sin(v), Mathf.Sin(u) * (majorRadius + minorRadius * Mathf.Cos(v))));
                normals.Add(normal.normalized);
            }
        }
        for (int i = 0; i < majorSegments; i++)
        {
            for (int j = 0; j < minorSegments; j++)
            {
                int a = i * (minorSegments + 1) + j;
                int b = a + minorSegments + 1;
                triangles.Add(a); triangles.Add(b); triangles.Add(a + 1);
                triangles.Add(a + 1); triangles.Add(b); triangles.Add(b + 1);
            }
        }
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateBounds();

        var go = new GameObject(name);
        go.transform.position = position;
        go.transform.rotation = rotation;
        var filter = go.AddComponent<MeshFilter>();
        filter.sharedMesh = mesh;
        var renderer = go.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        return go;
    }

    private static GameObject Parent(GameObject child, Transform parent)
    {
        child.transform.SetParent(parent, true);
        return child;
    }

    private static GameObject ParentLocal(GameObject child, Transform parent)
    {
        child.transform.SetParent(parent, false);
        return child;
    }

    private static void CreateMaterials()
    {
        agedBrass = Metallic("aged brass procedural patina", new Color(0.77f, 0.55f, 0.25f), 0.9f, 0.36f, MakeNoiseTexture(new Color(0.48f, 0.34f, 0.16f), new Color(0.92f, 0.68f, 0.32f)));
        brightWornBrass = Metallic("worn polished brass edges", new Color(1.0f, 0.76f, 0.35f), 1.0f, 0.22f, null);
        copper = Metallic("heat stained copper coil", new Color(0.86f, 0.37f, 0.16f), 1.0f, 0.28f, MakeNoiseTexture(new Color(0.38f, 0.13f, 0.07f), new Color(0.97f, 0.45f, 0.19f)));
        darkIron = Metallic("blackened dark iron receiver", new Color(0.09f, 0.085f, 0.078f), 0.95f, 0.48f, MakeNoiseTexture(new Color(0.035f, 0.034f, 0.033f), new Color(0.17f, 0.15f, 0.13f)));
        blackGrip = Metallic("worn black grip", new Color(0.025f, 0.022f, 0.019f), 0.2f, 0.6f, MakeNoiseTexture(new Color(0.012f, 0.011f, 0.01f), new Color(0.09f, 0.078f, 0.062f)));
        amberGlass = Transparent("amber pressure glass", new Color(1.0f, 0.46f, 0.08f, 0.44f), 0.0f, 0.06f);
        dialFace = Metallic("aged ivory dial face", new Color(0.78f, 0.70f, 0.52f), 0.0f, 0.42f, MakeNoiseTexture(new Color(0.46f, 0.39f, 0.27f), new Color(0.92f, 0.84f, 0.62f)));
        grime = Metallic("oil grime and soot", new Color(0.018f, 0.014f, 0.01f), 0.1f, 0.82f, null);
        redNeedle = Metallic("oxide red gauge needle", new Color(0.65f, 0.05f, 0.025f), 0.4f, 0.35f, null);
        proxyGrey = Metallic("matte first person scale proxy", new Color(0.34f, 0.34f, 0.32f), 0.0f, 0.72f, null);
        floorMat = Metallic("matte charcoal review surface", new Color(0.075f, 0.071f, 0.065f), 0.0f, 0.8f, null);
    }

    private static Material Metallic(string name, Color color, float metallic, float smoothness, Texture2D texture)
    {
        var mat = new Material(Shader.Find("Standard"));
        mat.name = name;
        mat.SetColor("_Color", color);
        mat.SetFloat("_Metallic", metallic);
        mat.SetFloat("_Glossiness", smoothness);
        if (texture != null) mat.SetTexture("_MainTex", texture);
        return mat;
    }

    private static Material Transparent(string name, Color color, float metallic, float smoothness)
    {
        var mat = Metallic(name, color, metallic, smoothness, null);
        mat.SetFloat("_Mode", 3f);
        mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        return mat;
    }

    private static Texture2D MakeNoiseTexture(Color low, Color high)
    {
        var texture = new Texture2D(128, 128, TextureFormat.RGBA32, true);
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float n = Mathf.PerlinNoise(x * 0.075f, y * 0.075f);
                float scratches = Mathf.PerlinNoise(x * 0.31f, y * 0.015f) > 0.66f ? 0.18f : 0f;
                texture.SetPixel(x, y, Color.Lerp(low, high, Mathf.Clamp01(n + scratches)));
            }
        }
        texture.Apply();
        return texture;
    }

    private static void RenderCameraToPng(Camera camera, string path, int width, int height)
    {
        var rt = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32)
        {
            antiAliasing = 8
        };
        camera.targetTexture = rt;
        RenderTexture.active = rt;
        camera.Render();

        var image = new Texture2D(width, height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();
        File.WriteAllBytes(path, image.EncodeToPNG());

        camera.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
        UnityEngine.Object.DestroyImmediate(rt);
        UnityEngine.Object.DestroyImmediate(image);
    }

    private static string RenderContactSheet(string renderDir, string fileName, IReadOnlyList<string> sourceImages)
    {
        int thumbW = 640;
        int thumbH = 426;
        int margin = 36;
        int labelH = 46;
        int cols = 4;
        int rows = 2;
        int width = cols * thumbW + (cols + 1) * margin;
        int height = rows * (thumbH + labelH) + (rows + 1) * margin;
        var sheet = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Fill(sheet, new Color(0.045f, 0.043f, 0.04f, 1f));

        for (int i = 0; i < Mathf.Min(8, sourceImages.Count); i++)
        {
            var bytes = File.ReadAllBytes(sourceImages[i]);
            var src = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            src.LoadImage(bytes);
            int col = i % cols;
            int row = rows - 1 - (i / cols);
            int ox = margin + col * (thumbW + margin);
            int oy = margin + row * (thumbH + labelH + margin) + labelH;
            CopyScaled(src, sheet, ox, oy, thumbW, thumbH);
            DrawLabelBars(sheet, ox, oy - labelH + 10, thumbW, 24, i + 1);
            UnityEngine.Object.DestroyImmediate(src);
        }

        sheet.Apply();
        var path = Path.Combine(renderDir, fileName);
        File.WriteAllBytes(path, sheet.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(sheet);
        return path;
    }

    private static void Fill(Texture2D texture, Color color)
    {
        var colors = new Color[texture.width * texture.height];
        for (int i = 0; i < colors.Length; i++) colors[i] = color;
        texture.SetPixels(colors);
    }

    private static void CopyScaled(Texture2D src, Texture2D dst, int ox, int oy, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var color = src.GetPixelBilinear(x / (float)(width - 1), y / (float)(height - 1));
                dst.SetPixel(ox + x, oy + y, color);
            }
        }
    }

    private static void DrawLabelBars(Texture2D texture, int x, int y, int width, int height, int index)
    {
        var color = index % 2 == 0 ? new Color(0.76f, 0.48f, 0.18f, 1f) : new Color(0.74f, 0.64f, 0.38f, 1f);
        for (int yy = 0; yy < height; yy++)
        {
            for (int xx = 0; xx < width; xx++)
            {
                texture.SetPixel(x + xx, y + yy, color);
            }
        }
    }

    private static string WorkspacePath(params string[] parts)
    {
        var projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        var assetPacks = Directory.GetParent(projectRoot);
        var workspace = assetPacks.Parent.FullName;
        var result = workspace;
        foreach (var part in parts) result = Path.Combine(result, part);
        return result;
    }

    private enum Lighting
    {
        Warm,
        Cool,
        Neutral
    }
}
