using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.SurfaceTextureSet05.Editor
{
    public static class SurfaceTextureSet05UnityValidation
    {
        public static void ValidatePackage()
        {
            const string packagePath = "Packages/com.brassworks.sidecar.surface-texture-set05";
            var materialGuids = AssetDatabase.FindAssets("t:Material", new[] { packagePath + "/Runtime/Materials" });
            var textureGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { packagePath + "/Runtime/Textures" });
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(packagePath + "/Runtime/Materials");
            var resolvedPackagePath = packageInfo != null ? packageInfo.resolvedPath : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ".."));
            var manifestFiles = Directory.Exists(Path.Combine(resolvedPackagePath, "Documentation~", "Manifest"))
                ? Directory.GetFiles(Path.Combine(resolvedPackagePath, "Documentation~", "Manifest"), "*.json", SearchOption.TopDirectoryOnly).Length
                : 0;
            var repoRoot = Path.GetFullPath(Path.Combine(resolvedPackagePath, "..", ".."));
            var reportDir = Path.Combine(repoRoot, "Documentation", "AssetProduction", "V0_1_47_SurfaceTextureSet05");
            var renderDir = Path.Combine(repoRoot, "Documentation", "ConceptRenders", "V0_1_47_SurfaceTextureSet05");
            Directory.CreateDirectory(reportDir);
            Directory.CreateDirectory(renderDir);

            var materialPaths = materialGuids.Select(AssetDatabase.GUIDToAssetPath).OrderBy(p => p).ToArray();
            var previews = RenderMaterialPreviews(materialPaths, renderDir);
            var errors = 0;
            if (materialGuids.Length != 14) errors++;
            if (textureGuids.Length != 42) errors++;
            if (manifestFiles < 1) errors++;
            if (previews < 2) errors++;

            var json = "{\n" +
                "  \"status\": \"" + (errors == 0 ? "pass" : "fail") + "\",\n" +
                "  \"marker\": \"STS05_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL") + "\",\n" +
                "  \"materials\": " + materialGuids.Length + ",\n" +
                "  \"textures\": " + textureGuids.Length + ",\n" +
                "  \"manifest_files\": " + manifestFiles + ",\n" +
                "  \"unity_rendered_previews\": " + previews + ",\n" +
                "  \"runtime_contract\": \"visual/material-only; no colliders, audio, scenes, gameplay scripts, or runtime authority\"\n" +
                "}\n";
            File.WriteAllText(Path.Combine(reportDir, "STS05_UnityValidationReport_v0.1.47.json"), json);
            Debug.Log("STS05_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL") + " materials=" + materialGuids.Length + " textures=" + textureGuids.Length + " previews=" + previews);
            EditorApplication.Exit(errors == 0 ? 0 : 1);
        }

        private static int RenderMaterialPreviews(string[] materialPaths, string renderDir)
        {
            var mats = materialPaths.Select(p => AssetDatabase.LoadAssetAtPath<Material>(p)).Where(m => m != null).ToArray();
            if (mats.Length == 0) return 0;

            var root = new GameObject("STS05_RenderRoot");
            var cameraObj = new GameObject("STS05_Camera");
            var camera = cameraObj.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.045f, 0.043f, 0.039f, 1f);
            camera.transform.position = new Vector3(0, 2.6f, -8.5f);
            camera.transform.rotation = Quaternion.Euler(18f, 0f, 0f);
            var lightObj = new GameObject("STS05_KeyLight");
            var light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.5f;
            light.color = new Color(1f, 0.82f, 0.56f);
            light.transform.rotation = Quaternion.Euler(42f, -32f, 0f);

            var created = 0;
            var group = new GameObject("STS05_MaterialGrid");
            group.transform.SetParent(root.transform);
            for (var i = 0; i < mats.Length; i++)
            {
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.DestroyImmediate(sphere.GetComponent<Collider>());
                sphere.name = mats[i].name;
                sphere.transform.SetParent(group.transform);
                sphere.transform.position = new Vector3((i % 7 - 3) * 1.35f, 1.3f - (i / 7) * 1.55f, 0f);
                sphere.GetComponent<Renderer>().sharedMaterial = mats[i];
            }

            created += RenderPng(camera, Path.Combine(renderDir, "STS05_UNITY_PREVIEW_material_spheres_contact_sheet_v0.1.47.png"), 1800, 900);

            for (var page = 0; page < 2; page++)
            {
                for (var i = 0; i < group.transform.childCount; i++)
                    group.transform.GetChild(i).gameObject.SetActive(i / 7 == page);
                camera.transform.position = new Vector3(0, 1.35f - page * 1.55f, -7.25f);
                camera.transform.rotation = Quaternion.Euler(10f, 0f, 0f);
                created += RenderPng(camera, Path.Combine(renderDir, "STS05_UNITY_PREVIEW_material_spheres_row_" + (page + 1) + "_v0.1.47.png"), 1600, 720);
            }

            UnityEngine.Object.DestroyImmediate(root);
            UnityEngine.Object.DestroyImmediate(cameraObj);
            UnityEngine.Object.DestroyImmediate(lightObj);
            AssetDatabase.Refresh();
            return created;
        }

        private static int RenderPng(Camera camera, string path, int width, int height)
        {
            var rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = rt;
            RenderTexture.active = rt;
            camera.Render();
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            File.WriteAllBytes(path, tex.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(tex);
            camera.targetTexture = null;
            RenderTexture.active = null;
            rt.Release();
            UnityEngine.Object.DestroyImmediate(rt);
            return 1;
        }
    }
}
