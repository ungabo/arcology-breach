#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.Sidecars.SteamworksLevelKit
{
    public static class SteamworksLevelKitPreviewRenderer
    {
        private const string MenuRoot = "Brassworks/Sidecars/Steamworks Level Kit v0.1.39/";

        [MenuItem(MenuRoot + "Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            IReadOnlyList<string> prefabPaths = SteamworksLevelKitGenerator.EnsureGeneratedPrefabPaths();
            string outputRoot = ResolveOutputRoot();
            Directory.CreateDirectory(outputRoot);

            RenderPrefabGroup(
                "SCLVL_PREVIEW_corridor_composition_v0.1.39.png",
                outputRoot,
                new[]
                {
                    FindPrefabPath(prefabPaths, "SCLVL_CorridorStraight_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_CorridorStraight_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_CorridorCorner_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_RivetedWallSection_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_CeilingPipeCluster_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_PipeBundle_Wall_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_LanternWallMount.prefab")
                },
                new[]
                {
                    new Vector3(-4.2f, 0f, 0f),
                    new Vector3(0f, 0f, 0f),
                    new Vector3(4.25f, 0f, 0f),
                    new Vector3(0f, 0f, 2.35f),
                    new Vector3(0f, 0.16f, -1.9f),
                    new Vector3(-0.2f, 1.3f, 2.05f),
                    new Vector3(1.55f, 1.0f, 1.95f)
                },
                new Vector3(0.2f, 2.2f, -8.2f),
                new Vector3(12f, 0f, 0f));

            RenderPrefabGroup(
                "SCLVL_PREVIEW_pressure_door_composition_v0.1.39.png",
                outputRoot,
                new[]
                {
                    FindPrefabPath(prefabPaths, "SCLVL_ArchedPressureDoor_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_RivetedVaultDoor_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_PressureLockDoorFrame_4m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_ValveCluster_2m.prefab"),
                    FindPrefabPath(prefabPaths, "SCLVL_SteamVentEmitter_Wall.prefab")
                },
                new[]
                {
                    new Vector3(-4.2f, 0f, 0f),
                    new Vector3(0f, 0f, 0f),
                    new Vector3(4.2f, 0f, 0f),
                    new Vector3(-1.6f, 0f, -1.0f),
                    new Vector3(1.6f, 0.2f, -1.0f)
                },
                new Vector3(0f, 2.35f, -8.1f),
                new Vector3(12f, 0f, 0f));

            RenderPrefabGroup(
                "SCLVL_PREVIEW_object_lineup_v0.1.39.png",
                outputRoot,
                prefabPaths,
                BuildContactSheetPositions(prefabPaths.Count),
                new Vector3(0f, 8.4f, -18f),
                new Vector3(30f, 0f, 0f));

            RenderMaterialReadabilitySwatch(
                "SCLVL_PREVIEW_material_readability_swatch_v0.1.39.png",
                outputRoot,
                ResolvePackageRoot(prefabPaths));

            AssetDatabase.Refresh();
            Debug.Log("SCLVL_PREVIEW_PASS v0.1.39 output=" + outputRoot);
        }

        private static string ResolveOutputRoot()
        {
            string configuredRoot = Environment.GetEnvironmentVariable("BRASSWORKS_SCLVL_PREVIEW_ROOT");
            if (!string.IsNullOrWhiteSpace(configuredRoot))
            {
                return Path.GetFullPath(configuredRoot);
            }

            return Path.GetFullPath(Path.Combine(Directory.GetParent(Application.dataPath).FullName, SteamworksLevelKitGenerator.RenderOutputRelativePath));
        }

        private static string FindPrefabPath(IReadOnlyList<string> prefabPaths, string fileName)
        {
            for (int i = 0; i < prefabPaths.Count; i++)
            {
                if (prefabPaths[i].EndsWith("/" + fileName, StringComparison.Ordinal))
                {
                    return prefabPaths[i];
                }
            }

            Debug.LogWarning("Steamworks Level Kit preview requested unknown prefab: " + fileName);
            return prefabPaths[0];
        }

        private static string ResolvePackageRoot(IReadOnlyList<string> prefabPaths)
        {
            string marker = "/Runtime/Prefabs/";
            int markerIndex = prefabPaths[0].IndexOf(marker, StringComparison.Ordinal);
            if (markerIndex < 0)
            {
                return "Packages/" + SteamworksLevelKitGenerator.PackageName;
            }

            return prefabPaths[0].Substring(0, markerIndex);
        }

        private static Vector3[] BuildContactSheetPositions(int count)
        {
            Vector3[] positions = new Vector3[count];
            int columns = 5;
            for (int i = 0; i < count; i++)
            {
                int row = i / columns;
                int column = i % columns;
                positions[i] = new Vector3((column - 2) * 4.6f, 0f, row * 4.3f);
            }

            return positions;
        }

        private static void RenderMaterialReadabilitySwatch(string fileName, string outputRoot, string packageRoot)
        {
            string[] materialNames =
            {
                "SCLVL_BlackenedIron",
                "SCLVL_AgedBrass",
                "SCLVL_CopperSteamPipe",
                "SCLVL_SootBrick",
                "SCLVL_OilWetStone",
                "SCLVL_WarmAmberGlass",
                "SCLVL_PressureGreenGlass",
                "SCLVL_HeatRedEnamel",
                "SCLVL_BoilerGlow",
                "SCLVL_GaugeIvory",
                "SCLVL_WalnutWood",
                "SCLVL_OxidizedCopper"
            };

            GameObject stage = new GameObject("SCLVL_RenderStage_" + Path.GetFileNameWithoutExtension(fileName));
            try
            {
                for (int i = 0; i < materialNames.Length; i++)
                {
                    int column = i % 4;
                    int row = i / 4;
                    GameObject swatch = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    swatch.name = "swatch_" + materialNames[i];
                    swatch.transform.SetParent(stage.transform, false);
                    swatch.transform.localPosition = new Vector3((column - 1.5f) * 1.25f, 1.55f - row * 0.62f, 0f);
                    swatch.transform.localScale = new Vector3(1.0f, 0.45f, 0.14f);
                    Renderer renderer = swatch.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material material = AssetDatabase.LoadAssetAtPath<Material>(packageRoot + "/Runtime/Materials/" + materialNames[i] + ".mat");
                        renderer.sharedMaterial = material;
                    }
                }

                BoxPrimitive(stage, "dark_readability_backplate", new Vector3(0f, 0.78f, 0.15f), new Vector3(5.6f, 2.6f, 0.1f), new Color(0.018f, 0.016f, 0.014f));
                AddLighting(stage);
                RenderStageToPng(stage, Path.Combine(outputRoot, fileName), new Vector3(0f, 0.9f, -6.2f), new Vector3(0f, 0f, 0f));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(stage);
            }
        }

        private static void BoxPrimitive(GameObject parent, string name, Vector3 position, Vector3 scale, Color color)
        {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.name = name;
            box.transform.SetParent(parent.transform, false);
            box.transform.localPosition = position;
            box.transform.localScale = scale;
            Renderer renderer = box.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = new Material(Shader.Find("Standard"));
                material.color = color;
                renderer.sharedMaterial = material;
            }
        }

        private static void RenderPrefabGroup(string fileName, string outputRoot, IReadOnlyList<string> prefabPaths, IReadOnlyList<Vector3> positions, Vector3 cameraPosition, Vector3 cameraEuler)
        {
            GameObject stage = new GameObject("SCLVL_RenderStage_" + Path.GetFileNameWithoutExtension(fileName));
            try
            {
                for (int i = 0; i < prefabPaths.Count; i++)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[i]);
                    if (prefab == null)
                    {
                        Debug.LogWarning("Missing Steamworks Level Kit prefab for render: " + prefabPaths[i]);
                        continue;
                    }

                    GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    instance.transform.SetParent(stage.transform, false);
                    instance.transform.localPosition = positions[i];
                    instance.transform.localRotation = Quaternion.identity;
                }

                AddRenderFloor(stage);
                AddLighting(stage);
                RenderStageToPng(stage, Path.Combine(outputRoot, fileName), cameraPosition, cameraEuler);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(stage);
            }
        }

        private static void AddRenderFloor(GameObject stage)
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "preview_oil_wet_floor";
            floor.transform.SetParent(stage.transform, false);
            floor.transform.localPosition = new Vector3(0f, -0.11f, 4f);
            floor.transform.localScale = new Vector3(26f, 0.06f, 18f);
            Renderer renderer = floor.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = new Material(Shader.Find("Standard"));
                material.color = new Color(0.055f, 0.052f, 0.048f);
                renderer.sharedMaterial = material;
            }
        }

        private static void AddLighting(GameObject stage)
        {
            GameObject key = new GameObject("preview_key_light");
            key.transform.SetParent(stage.transform, false);
            key.transform.localRotation = Quaternion.Euler(45f, -35f, 0f);
            Light keyLight = key.AddComponent<Light>();
            keyLight.type = LightType.Directional;
            keyLight.intensity = 2.4f;
            keyLight.color = new Color(1f, 0.72f, 0.45f);

            GameObject fill = new GameObject("preview_fill_light");
            fill.transform.SetParent(stage.transform, false);
            fill.transform.localPosition = new Vector3(-3f, 4f, -3f);
            Light fillLight = fill.AddComponent<Light>();
            fillLight.type = LightType.Point;
            fillLight.range = 11f;
            fillLight.intensity = 2.2f;
            fillLight.color = new Color(0.95f, 0.54f, 0.2f);

            GameObject rim = new GameObject("preview_green_pressure_rim");
            rim.transform.SetParent(stage.transform, false);
            rim.transform.localPosition = new Vector3(4f, 2.5f, -2f);
            Light rimLight = rim.AddComponent<Light>();
            rimLight.type = LightType.Point;
            rimLight.range = 8f;
            rimLight.intensity = 0.8f;
            rimLight.color = new Color(0.17f, 0.8f, 0.38f);
        }

        private static void RenderStageToPng(GameObject stage, string path, Vector3 cameraPosition, Vector3 cameraEuler)
        {
            GameObject cameraObject = new GameObject("SCLVL_preview_camera");
            cameraObject.transform.SetParent(stage.transform, false);
            cameraObject.transform.position = cameraPosition;
            cameraObject.transform.rotation = Quaternion.Euler(cameraEuler);

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.02f, 0.018f, 0.015f);
            camera.fieldOfView = 48f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 100f;

            RenderTexture renderTexture = new RenderTexture(1600, 900, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = renderTexture;
            RenderTexture previousActive = RenderTexture.active;
            try
            {
                camera.Render();
                RenderTexture.active = renderTexture;
                Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply();
                File.WriteAllBytes(path, texture.EncodeToPNG());
                UnityEngine.Object.DestroyImmediate(texture);
            }
            finally
            {
                RenderTexture.active = previousActive;
                camera.targetTexture = null;
                renderTexture.Release();
                UnityEngine.Object.DestroyImmediate(renderTexture);
                UnityEngine.Object.DestroyImmediate(cameraObject);
            }
        }
    }
}
#endif
