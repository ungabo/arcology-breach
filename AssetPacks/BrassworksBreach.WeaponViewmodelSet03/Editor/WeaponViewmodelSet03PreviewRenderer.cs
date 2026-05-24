using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.WeaponViewmodelSet03.Editor
{
    public static class WeaponViewmodelSet03PreviewRenderer
    {
        private const string Version = "v0.1.41";
        private const int PreviewWidth = 1600;
        private const int PreviewHeight = 1000;
        private const int SheetWidth = 2800;
        private const int SheetHeight = 1800;

        [MenuItem("Brassworks Breach/Sidecar Packs/Render Weapon Viewmodel Set 03 Previews v0.1.41")]
        public static void RenderPreviewSet()
        {
            WeaponViewmodelSet03Generator.GenerateAll();

            var renderRoot = WeaponViewmodelSet03Generator.ResolveRepositoryRenderRoot();
            Directory.CreateDirectory(renderRoot);

            var renderedFiles = new List<string>();
            var prefabs = WeaponViewmodelSet03Generator.GeneratedPrefabAssetPaths();
            foreach (var prefabPath in prefabs)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning($"BB_WEAPON_VIEWMODEL_SET03_RENDER_SKIP missing={prefabPath}");
                    continue;
                }

                var outputPath = Path.Combine(renderRoot, $"{prefab.name}_preview.png");
                RenderSinglePrefab(prefab, outputPath);
                renderedFiles.Add(outputPath);
            }

            var contactSheetPath = Path.Combine(renderRoot, "BB_WVM03_ContactSheet.png");
            RenderContactSheet(prefabs, contactSheetPath);
            renderedFiles.Add(contactSheetPath);
            WritePreviewEvidence(renderRoot, renderedFiles);
            WeaponViewmodelSet03Generator.MarkValidated($"isolated_import_generate_and_preview_render_passed_{DateTimeOffset.Now:yyyy-MM-ddTHH:mm:sszzz}", "passed");

            AssetDatabase.Refresh();
            Debug.Log($"BB_WEAPON_VIEWMODEL_SET03_RENDER_PASS {Version} root={renderRoot} files={renderedFiles.Count}");
        }

        private static void RenderSinglePrefab(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var instance = UnityEngine.Object.Instantiate(prefab);
            instance.name = prefab.name;
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(0f, -34f, 0f);

            AddPreviewFloor(new Vector3(0f, -0.88f, 0f), new Vector3(4.8f, 0.06f, 3.4f));
            AddPreviewLights();

            var cameraObject = new GameObject("wvm03_preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.026f, 0.024f, 0.022f);
            camera.fieldOfView = 32f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 40f;

            var bounds = CalculateBounds(instance);
            var radius = Mathf.Max(0.65f, bounds.extents.magnitude);
            camera.transform.position = bounds.center + new Vector3(radius * 1.20f, radius * 0.72f, -radius * 2.30f);
            camera.transform.LookAt(bounds.center + Vector3.up * radius * 0.04f);

            RenderCameraToPng(camera, outputPath, PreviewWidth, PreviewHeight);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void RenderContactSheet(string[] prefabPaths, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddPreviewLights();

            const int columns = 5;
            const float cellX = 1.82f;
            const float cellY = 1.36f;
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
                var scale = Mathf.Min(1.0f, 1.00f / maxDimension);
                instance.transform.localScale *= scale;

                bounds = CalculateBounds(instance);
                var col = i % columns;
                var row = i / columns;
                var targetCenter = new Vector3((col - 2f) * cellX, (1.55f - row) * cellY, 0f);
                instance.transform.position += targetCenter - bounds.center;
            }

            var sheetBounds = CalculateSceneBounds();
            var cameraObject = new GameObject("wvm03_contact_sheet_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.026f, 0.024f, 0.022f);
            camera.orthographic = true;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 60f;
            camera.transform.position = sheetBounds.center + new Vector3(0f, 0f, -14f);
            camera.transform.LookAt(sheetBounds.center);
            camera.orthographicSize = Mathf.Max(sheetBounds.extents.y + 0.45f, (sheetBounds.extents.x + 0.55f) * SheetHeight / SheetWidth);

            RenderCameraToPng(camera, outputPath, SheetWidth, SheetHeight);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void AddPreviewLights()
        {
            var key = new GameObject("warm_lantern_key_light").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.73f, 0.43f);
            key.intensity = 2.45f;
            key.transform.rotation = Quaternion.Euler(43f, -36f, 0f);

            var rim = new GameObject("cool_pressure_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(0.44f, 0.67f, 0.76f);
            rim.intensity = 2.25f;
            rim.range = 6.5f;
            rim.transform.position = new Vector3(-2.2f, 1.4f, 1.8f);

            var fill = new GameObject("soft_amber_fill_light").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(1.0f, 0.48f, 0.20f);
            fill.intensity = 0.95f;
            fill.range = 5.5f;
            fill.transform.position = new Vector3(2.3f, -0.2f, -1.6f);
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
                name = "wvm03_preview_floor_material",
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

        private static void WritePreviewEvidence(string renderRoot, IReadOnlyList<string> files)
        {
            var productionRoot = WeaponViewmodelSet03Generator.ResolveRepositoryProductionRoot();
            Directory.CreateDirectory(productionRoot);
            var evidencePath = Path.Combine(productionRoot, "PreviewPixelEvidence_WeaponViewmodelSet03_v0.1.41.json");
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"WVM03\",");
            builder.AppendLine("  \"version\": \"0.1.41\",");
            builder.AppendLine($"  \"generated_at\": \"{DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)}\",");
            builder.AppendLine($"  \"render_root\": \"{Escape(NormalizePath(renderRoot))}\",");
            builder.AppendLine($"  \"file_count\": {files.Count.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine("  \"files\": [");
            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var info = new FileInfo(file);
                var dimensions = TryReadDimensions(file);
                builder.AppendLine("    {");
                builder.AppendLine($"      \"path\": \"{Escape(NormalizePath(file))}\",");
                builder.AppendLine($"      \"bytes\": {info.Length.ToString(CultureInfo.InvariantCulture)},");
                builder.AppendLine($"      \"width\": {dimensions.x.ToString(CultureInfo.InvariantCulture)},");
                builder.AppendLine($"      \"height\": {dimensions.y.ToString(CultureInfo.InvariantCulture)},");
                builder.AppendLine($"      \"nonempty_png\": {(info.Length > 1024 ? "true" : "false")}");
                builder.Append("    }");
                builder.AppendLine(i == files.Count - 1 ? string.Empty : ",");
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(evidencePath, builder.ToString(), Encoding.UTF8);
        }

        private static Vector2Int TryReadDimensions(string file)
        {
            var bytes = File.ReadAllBytes(file);
            var texture = new Texture2D(2, 2);
            try
            {
                if (texture.LoadImage(bytes))
                {
                    return new Vector2Int(texture.width, texture.height);
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(texture);
            }

            return Vector2Int.zero;
        }

        private static string NormalizePath(string path)
        {
            return path.Replace("\\", "/");
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
