using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.ObjectivePropsSet02.Editor
{
    public static class ObjectivePropsSet02PreviewRenderer
    {
        private const int PreviewWidth = 1600;
        private const int PreviewHeight = 1000;
        private const int SheetWidth = 3000;
        private const int SheetHeight = 2000;

        [MenuItem("Brassworks Breach/Sidecar Packs/Objective Props Set 02 v0.1.42/Render Preview PNGs")]
        public static void RenderPreviewSet()
        {
            ObjectivePropsSet02Generator.GenerateAll();

            var renderRoot = ObjectivePropsSet02Generator.ResolveRepositoryRenderRoot();
            Directory.CreateDirectory(renderRoot);

            var filesWritten = 0;
            foreach (var prefabPath in ObjectivePropsSet02Generator.GeneratedPrefabAssetPaths())
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning("BB_OBJECTIVE_PROPS_SET02_RENDER_SKIP missing=" + prefabPath);
                    continue;
                }

                RenderSinglePrefab(prefab, Path.Combine(renderRoot, prefab.name + "_preview.png"));
                filesWritten++;
            }

            RenderContactSheet(ObjectivePropsSet02Generator.GeneratedPrefabAssetPaths(), Path.Combine(renderRoot, "BB_OPS02_ContactSheet.png"));
            filesWritten++;

            ObjectivePropsSet02Generator.MarkValidated(
                "generated_and_preview_rendered_by_unity_sidecar_batchmode_" + DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                "passed_" + filesWritten + "_pngs",
                "pending_sidecar_validator");

            AssetDatabase.Refresh();
            Debug.Log($"BB_OBJECTIVE_PROPS_SET02_RENDER_PASS {ObjectivePropsSet02Generator.VersionLabel} files={filesWritten} output={renderRoot}");
        }

        private static void RenderSinglePrefab(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var instance = UnityEngine.Object.Instantiate(prefab);
            instance.name = prefab.name;
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(0f, -24f, 0f);

            AddPreviewFloor(new Vector3(0f, -0.86f, 0.22f), new Vector3(4.6f, 0.06f, 3.4f));
            AddPreviewWall(new Vector3(0f, 0.18f, 0.42f), new Vector3(4.6f, 2.9f, 0.06f));
            AddPreviewLights();

            var bounds = CalculateBounds(instance);
            var radius = Mathf.Max(0.72f, bounds.extents.magnitude);

            var cameraObject = new GameObject("ops02_preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.020f, 0.018f, 0.016f);
            camera.fieldOfView = 33f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 50f;
            camera.transform.position = bounds.center + new Vector3(radius * 0.72f, radius * 0.42f, -radius * 2.45f);
            camera.transform.LookAt(bounds.center + Vector3.up * radius * 0.05f);

            RenderCameraToPng(camera, outputPath, PreviewWidth, PreviewHeight);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderContactSheet(string[] prefabPaths, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddPreviewLights();

            const int columns = 6;
            const float cellX = 1.62f;
            const float cellY = 1.48f;
            for (var i = 0; i < prefabPaths.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[i]);
                if (prefab == null)
                {
                    continue;
                }

                var instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = prefab.name;
                instance.transform.rotation = Quaternion.Euler(0f, -18f, 0f);

                var bounds = CalculateBounds(instance);
                var maxDimension = Mathf.Max(0.1f, Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z)));
                var scale = Mathf.Min(1.0f, 1.02f / maxDimension);
                instance.transform.localScale *= scale;

                bounds = CalculateBounds(instance);
                var col = i % columns;
                var row = i / columns;
                var targetCenter = new Vector3((col - 2.5f) * cellX, (1.5f - row) * cellY, 0f);
                instance.transform.position += targetCenter - bounds.center;
            }

            var sheetBounds = CalculateSceneBounds();
            var cameraObject = new GameObject("ops02_contact_sheet_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.020f, 0.018f, 0.016f);
            camera.orthographic = true;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 80f;
            camera.transform.position = sheetBounds.center + new Vector3(0f, 0f, -18f);
            camera.transform.LookAt(sheetBounds.center);
            camera.orthographicSize = Mathf.Max(sheetBounds.extents.y + 0.48f, (sheetBounds.extents.x + 0.55f) * SheetHeight / SheetWidth);

            RenderCameraToPng(camera, outputPath, SheetWidth, SheetHeight);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void AddPreviewLights()
        {
            var key = new GameObject("warm_lantern_key_light").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.74f, 0.46f);
            key.intensity = 2.4f;
            key.transform.rotation = Quaternion.Euler(45f, -34f, 0f);

            var rim = new GameObject("cool_pressure_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(0.30f, 0.68f, 0.84f);
            rim.intensity = 2.0f;
            rim.range = 6.5f;
            rim.transform.position = new Vector3(-2.2f, 1.4f, -1.7f);

            var fill = new GameObject("soft_red_override_fill").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(0.95f, 0.30f, 0.18f);
            fill.intensity = 0.82f;
            fill.range = 5.5f;
            fill.transform.position = new Vector3(2.4f, -0.1f, -1.2f);
        }

        private static void AddPreviewFloor(Vector3 position, Vector3 scale)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "ops02_preview_oil_floor";
            floor.transform.position = position;
            floor.transform.localScale = scale;
            RemoveCollider(floor);
            ApplyTransientMaterial(floor, new Color(0.040f, 0.037f, 0.033f));
        }

        private static void AddPreviewWall(Vector3 position, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = "ops02_preview_soot_wall";
            wall.transform.position = position;
            wall.transform.localScale = scale;
            RemoveCollider(wall);
            ApplyTransientMaterial(wall, new Color(0.065f, 0.055f, 0.047f));
        }

        private static void RemoveCollider(GameObject gameObject)
        {
            var collider = gameObject.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }

        private static void ApplyTransientMaterial(GameObject gameObject, Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = new Material(shader)
            {
                name = gameObject.name + "_material",
                color = color
            };

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }
        }

        private static void RenderCameraToPng(Camera camera, string outputPath, int width, int height)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? string.Empty);
            var renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)
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

                renderTexture.Release();
                UnityEngine.Object.DestroyImmediate(renderTexture);
            }
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>(true);
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
