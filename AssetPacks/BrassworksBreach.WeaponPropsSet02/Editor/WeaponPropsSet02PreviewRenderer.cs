using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.WeaponPropsSet02.Editor
{
    public static class WeaponPropsSet02PreviewRenderer
    {
        private const string Version = "v0.1.40";
        private const int PreviewWidth = 1600;
        private const int PreviewHeight = 1000;
        private const int SheetWidth = 2400;
        private const int SheetHeight = 1600;

        [MenuItem("Brassworks Breach/Sidecar Packs/Render Weapon Props Set 02 Previews v0.1.40")]
        public static void RenderPreviewSet()
        {
            WeaponPropsSet02Generator.GenerateAll();

            var renderRoot = WeaponPropsSet02Generator.ResolveRepositoryRenderRoot();
            Directory.CreateDirectory(renderRoot);

            var prefabs = WeaponPropsSet02Generator.GeneratedPrefabAssetPaths();
            foreach (var prefabPath in prefabs)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning($"BB_WEAPON_PROPS_SET02_RENDER_SKIP missing={prefabPath}");
                    continue;
                }

                RenderSinglePrefab(prefab, Path.Combine(renderRoot, $"{prefab.name}_preview.png"));
            }

            RenderContactSheet(prefabs, Path.Combine(renderRoot, "BB_WPS02_ContactSheet.png"));
            AssetDatabase.Refresh();
            Debug.Log($"BB_WEAPON_PROPS_SET02_RENDER_PASS {Version} root={renderRoot}");
        }

        private static void RenderSinglePrefab(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var instance = UnityEngine.Object.Instantiate(prefab);
            instance.name = prefab.name;
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(0f, -34f, 0f);

            AddPreviewFloor(new Vector3(0f, -0.86f, 0f), new Vector3(4.6f, 0.06f, 3.2f));
            AddPreviewLights();

            var cameraObject = new GameObject("wps02_preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.026f, 0.024f, 0.022f);
            camera.fieldOfView = 33f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 30f;

            var bounds = CalculateBounds(instance);
            var radius = Mathf.Max(0.62f, bounds.extents.magnitude);
            camera.transform.position = bounds.center + new Vector3(radius * 1.18f, radius * 0.72f, -radius * 2.25f);
            camera.transform.LookAt(bounds.center + Vector3.up * radius * 0.05f);

            RenderCameraToPng(camera, outputPath, PreviewWidth, PreviewHeight);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderContactSheet(string[] prefabPaths, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddPreviewLights();

            const int columns = 4;
            const float cellX = 2.0f;
            const float cellY = 1.46f;
            for (var i = 0; i < prefabPaths.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[i]);
                if (prefab == null)
                {
                    continue;
                }

                var instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = prefab.name;
                instance.transform.rotation = Quaternion.Euler(0f, -30f, 0f);

                var bounds = CalculateBounds(instance);
                var maxDimension = Mathf.Max(0.1f, Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z)));
                var scale = Mathf.Min(1.0f, 1.06f / maxDimension);
                instance.transform.localScale *= scale;

                bounds = CalculateBounds(instance);
                var col = i % columns;
                var row = i / columns;
                var targetCenter = new Vector3((col - 1.5f) * cellX, (1.45f - row) * cellY, 0f);
                instance.transform.position += targetCenter - bounds.center;
            }

            var sheetBounds = CalculateSceneBounds();
            var cameraObject = new GameObject("wps02_contact_sheet_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.026f, 0.024f, 0.022f);
            camera.orthographic = true;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 50f;
            camera.transform.position = sheetBounds.center + new Vector3(0f, 0f, -12f);
            camera.transform.LookAt(sheetBounds.center);
            camera.orthographicSize = Mathf.Max(sheetBounds.extents.y + 0.55f, (sheetBounds.extents.x + 0.65f) * SheetHeight / SheetWidth);

            RenderCameraToPng(camera, outputPath, SheetWidth, SheetHeight);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void AddPreviewLights()
        {
            var key = new GameObject("warm_lantern_key_light").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.73f, 0.43f);
            key.intensity = 2.35f;
            key.transform.rotation = Quaternion.Euler(42f, -36f, 0f);

            var rim = new GameObject("cool_pressure_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(0.47f, 0.67f, 0.74f);
            rim.intensity = 2.2f;
            rim.range = 6f;
            rim.transform.position = new Vector3(-2.0f, 1.3f, 1.8f);

            var fill = new GameObject("soft_amber_fill_light").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(1.0f, 0.48f, 0.20f);
            fill.intensity = 0.9f;
            fill.range = 5f;
            fill.transform.position = new Vector3(2.2f, -0.2f, -1.5f);
        }

        private static void AddPreviewFloor(Vector3 position, Vector3 scale)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "oily_steel_preview_floor";
            floor.transform.position = position;
            floor.transform.localScale = scale;
            var collider = floor.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = new Material(shader)
            {
                name = "wps02_preview_floor_material",
                color = new Color(0.045f, 0.043f, 0.040f)
            };

            var renderer = floor.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }
        }

        private static void RenderCameraToPng(Camera camera, string outputPath, int width, int height)
        {
            var renderTexture = new RenderTexture(width, height, 24)
            {
                antiAliasing = 4
            };

            var previous = RenderTexture.active;
            Texture2D texture = null;
            try
            {
                camera.targetTexture = renderTexture;
                camera.Render();

                RenderTexture.active = renderTexture;
                texture = new Texture2D(width, height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                texture.Apply();

                File.WriteAllBytes(outputPath, texture.EncodeToPNG());
            }
            finally
            {
                RenderTexture.active = previous;
                camera.targetTexture = null;
                if (texture != null)
                {
                    UnityEngine.Object.DestroyImmediate(texture);
                }

                UnityEngine.Object.DestroyImmediate(renderTexture);
            }
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return new Bounds(root.transform.position, Vector3.one);
            }

            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private static Bounds CalculateSceneBounds()
        {
            var renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Exclude);
            if (renderers.Length == 0)
            {
                return new Bounds(Vector3.zero, Vector3.one);
            }

            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }
    }
}
