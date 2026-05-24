#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.Sidecars.SteamworksLevelKit
{
    public static class SteamworksLevelKitPreviewRenderer
    {
        private const string MenuRoot = "Brassworks/Sidecars/Steamworks Level Kit v0.1.37/";

        [MenuItem(MenuRoot + "Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            IReadOnlyList<string> prefabPaths = SteamworksLevelKitGenerator.EnsureGeneratedPrefabPaths();
            string outputRoot = Path.GetFullPath(Path.Combine(Directory.GetParent(Application.dataPath).FullName, SteamworksLevelKitGenerator.RenderOutputRelativePath));
            Directory.CreateDirectory(outputRoot);

            RenderPrefabGroup(
                "SCLVL_PREVIEW_corridor_door_room_kit.png",
                outputRoot,
                new[]
                {
                    prefabPaths[0],
                    prefabPaths[6],
                    prefabPaths[4],
                    prefabPaths[10]
                },
                new[]
                {
                    new Vector3(-4.5f, 0f, 0f),
                    new Vector3(0f, 0f, 0f),
                    new Vector3(4.2f, 0f, 0.15f),
                    new Vector3(0f, 0.15f, -2.1f)
                },
                new Vector3(0f, 2.2f, -7.6f),
                new Vector3(13f, 0f, 0f));

            RenderPrefabGroup(
                "SCLVL_PREVIEW_setpieces_boiler_vault_console.png",
                outputRoot,
                new[]
                {
                    prefabPaths[3],
                    prefabPaths[5],
                    prefabPaths[11],
                    prefabPaths[12]
                },
                new[]
                {
                    new Vector3(-4.4f, 0f, 0f),
                    new Vector3(0f, 0f, 0f),
                    new Vector3(3.25f, 0f, -0.8f),
                    new Vector3(5.0f, 0f, -0.1f)
                },
                new Vector3(0.4f, 2.1f, -7.3f),
                new Vector3(12f, -2f, 0f));

            RenderPrefabGroup(
                "SCLVL_PREVIEW_contact_sheet_all_modules.png",
                outputRoot,
                prefabPaths,
                BuildContactSheetPositions(prefabPaths.Count),
                new Vector3(0f, 7.2f, -14f),
                new Vector3(32f, 0f, 0f));

            AssetDatabase.Refresh();
            Debug.Log("Steamworks Level Kit preview PNGs written to " + outputRoot);
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
                Object.DestroyImmediate(stage);
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
                Object.DestroyImmediate(texture);
            }
            finally
            {
                RenderTexture.active = previousActive;
                camera.targetTexture = null;
                renderTexture.Release();
                Object.DestroyImmediate(renderTexture);
                Object.DestroyImmediate(cameraObject);
            }
        }
    }
}
#endif
