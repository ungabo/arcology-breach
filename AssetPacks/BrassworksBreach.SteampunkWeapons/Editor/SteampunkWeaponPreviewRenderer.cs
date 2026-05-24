using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.SteampunkWeapons.Editor
{
    public static class SteampunkWeaponPreviewRenderer
    {
        private const string Version = "v0.1.37";
        private const int Width = 1600;
        private const int Height = 1000;

        [MenuItem("Brassworks Breach/Sidecar Packs/Render Steampunk Weapon Previews v0.1.37")]
        public static void RenderPreviewSet()
        {
            SteampunkWeaponPackGenerator.GenerateAll();

            var renderRoot = ResolveRenderRoot();
            Directory.CreateDirectory(renderRoot);

            foreach (var prefabPath in SteampunkWeaponPackGenerator.GeneratedPrefabAssetPaths())
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning($"Skipping missing preview prefab: {prefabPath}");
                    continue;
                }

                RenderPrefab(prefab, Path.Combine(renderRoot, $"{prefab.name}_preview.png"));
            }

            AssetDatabase.Refresh();
            Debug.Log($"BB_STEAMPUNK_WEAPONS_RENDER_PASS {Version} root={renderRoot}");
        }

        private static void RenderPrefab(GameObject prefab, string outputPath)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var instance = UnityEngine.Object.Instantiate(prefab);
            instance.name = prefab.name;
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(0f, -32f, 0f);

            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "oily_wet_stone_preview_floor";
            floor.transform.position = new Vector3(0f, -0.85f, 0f);
            floor.transform.localScale = new Vector3(4.6f, 0.06f, 3.2f);
            var floorRenderer = floor.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                floorRenderer.sharedMaterial = new Material(Shader.Find("Standard"))
                {
                    color = new Color(0.045f, 0.042f, 0.038f)
                };
            }

            var key = new GameObject("warm_lantern_key_light").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.72f, 0.42f);
            key.intensity = 2.3f;
            key.transform.rotation = Quaternion.Euler(42f, -36f, 0f);

            var rim = new GameObject("cool_metal_rim_light").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(0.55f, 0.66f, 0.72f);
            rim.intensity = 2.0f;
            rim.range = 5f;
            rim.transform.position = new Vector3(-1.8f, 1.2f, 1.6f);

            var cameraObject = new GameObject("preview_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.025f, 0.023f, 0.021f);
            camera.fieldOfView = 34f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 20f;

            var bounds = CalculateBounds(instance);
            var radius = Mathf.Max(0.6f, bounds.extents.magnitude);
            camera.transform.position = bounds.center + new Vector3(radius * 1.15f, radius * 0.70f, -radius * 2.2f);
            camera.transform.LookAt(bounds.center + Vector3.up * radius * 0.04f);

            var renderTexture = new RenderTexture(Width, Height, 24)
            {
                antiAliasing = 4
            };
            try
            {
                camera.targetTexture = renderTexture;
                camera.Render();

                var previous = RenderTexture.active;
                RenderTexture.active = renderTexture;
                var texture = new Texture2D(Width, Height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
                texture.Apply();
                RenderTexture.active = previous;

                File.WriteAllBytes(outputPath, texture.EncodeToPNG());
                UnityEngine.Object.DestroyImmediate(texture);
            }
            finally
            {
                camera.targetTexture = null;
                UnityEngine.Object.DestroyImmediate(renderTexture);
            }

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
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

        private static string ResolveRenderRoot()
        {
            var explicitRoot = Environment.GetEnvironmentVariable("BRASSWORKS_RENDER_ROOT");
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                return explicitRoot;
            }

            return Path.GetFullPath(Path.Combine(
                Application.dataPath,
                "..",
                "Documentation",
                "ConceptRenders",
                "V0_1_37_SteampunkWeaponsSidecar"));
        }
    }
}
