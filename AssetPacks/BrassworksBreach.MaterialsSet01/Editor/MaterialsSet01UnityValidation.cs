using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace BrassworksBreach.MaterialsSet01.Editor
{
    public static class MaterialsSet01UnityValidation
    {
        public static void ValidatePackage()
        {
            const string packagePath = "Packages/com.brassworks.sidecar.materials-set01";
            var materialGuids = AssetDatabase.FindAssets("t:Material", new[] { packagePath + "/Runtime/Materials" });
            var textureGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { packagePath + "/Runtime/Textures" });
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(packagePath + "/Runtime/Materials");
            var resolvedPackagePath = packageInfo != null
                ? packageInfo.resolvedPath
                : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ".."));
            var manifestPath = Path.Combine(resolvedPackagePath, "Documentation~", "Manifest", "MSET01_MaterialsSet01_Manifest_v0.1.39-p001.json");
            var manifestFiles = File.Exists(manifestPath) ? 1 : 0;
            var reportDir = Environment.GetEnvironmentVariable("BB_MATERIALS_SET01_REPORT_DIR");
            if (string.IsNullOrWhiteSpace(reportDir))
            {
                reportDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "..", "Documentation", "AssetProduction", "V0_1_39_MaterialsSet01"));
            }

            Directory.CreateDirectory(reportDir);
            var reportPath = Path.Combine(reportDir, "unity_validation_report_v0.1.39.json");
            var errors = 0;
            if (materialGuids.Length != 16) errors++;
            if (textureGuids.Length != 48) errors++;
            if (manifestFiles < 1) errors++;

            var materialNames = materialGuids.Select(AssetDatabase.GUIDToAssetPath).OrderBy(path => path, StringComparer.Ordinal).ToArray();
            var json = "{\n" +
                "  \"status\": \"" + (errors == 0 ? "pass" : "fail") + "\",\n" +
                "  \"materials\": " + materialGuids.Length + ",\n" +
                "  \"textures\": " + textureGuids.Length + ",\n" +
                "  \"manifest_files\": " + manifestFiles + ",\n" +
                "  \"manifest_path\": \"" + manifestPath.Replace("\\", "/") + "\",\n" +
                "  \"material_paths\": [\n    \"" + string.Join("\",\n    \"", materialNames) + "\"\n  ]\n" +
                "}\n";
            File.WriteAllText(reportPath, json);
            UnityEngine.Debug.Log("MSET01_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL") + " materials=" + materialGuids.Length + " textures=" + textureGuids.Length + " manifest=" + manifestFiles + " report=" + reportPath);
            EditorApplication.Exit(errors == 0 ? 0 : 1);
        }
    }
}
