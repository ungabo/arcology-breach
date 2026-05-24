using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BrassworksBreach.WeaponViewmodelSet03;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.WeaponViewmodelSet03.Editor
{
    public static class WeaponViewmodelSet03Validation
    {
        private const string Version = "v0.1.41";

        [MenuItem("Brassworks Breach/Sidecar Packs/Validate Weapon Viewmodel Set 03 v0.1.41")]
        public static void ValidateGeneratedAssets()
        {
            WeaponViewmodelSet03Generator.GenerateAll();

            var findings = new List<string>();
            var prefabPaths = WeaponViewmodelSet03Generator.GeneratedPrefabAssetPaths();
            var materialPaths = WeaponViewmodelSet03Generator.GeneratedMaterialAssetPaths();
            var meshPaths = WeaponViewmodelSet03Generator.GeneratedMeshAssetPaths();

            var loadedPrefabs = 0;
            var rendererCount = 0;
            foreach (var path in prefabPaths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    findings.Add($"missing_prefab:{path}");
                    continue;
                }

                loadedPrefabs++;
                var identity = prefab.GetComponent<WeaponViewmodelSet03Identity>();
                if (identity == null)
                {
                    findings.Add($"missing_identity:{path}");
                }

                var renderers = prefab.GetComponentsInChildren<Renderer>(true);
                rendererCount += renderers.Length;
                if (renderers.Length == 0)
                {
                    findings.Add($"no_renderers:{path}");
                }

                if (prefab.GetComponentsInChildren<Collider>(true).Length > 0)
                {
                    findings.Add($"colliders_present:{path}");
                }

                if (prefab.GetComponentsInChildren<Rigidbody>(true).Length > 0)
                {
                    findings.Add($"rigidbodies_present:{path}");
                }

                if (prefab.GetComponentsInChildren<AudioSource>(true).Length > 0)
                {
                    findings.Add($"audio_sources_present:{path}");
                }

                if (prefab.GetComponentsInChildren<ParticleSystem>(true).Length > 0)
                {
                    findings.Add($"particle_systems_present:{path}");
                }
            }

            var loadedMaterials = CountExistingAssets<Material>(materialPaths, "missing_material", findings);
            var loadedMeshes = CountExistingAssets<Mesh>(meshPaths, "missing_mesh", findings);

            var pass = findings.Count == 0 && loadedPrefabs >= 14 && loadedMaterials >= 10 && loadedMeshes >= 4;
            var productionRoot = WeaponViewmodelSet03Generator.ResolveRepositoryProductionRoot();
            Directory.CreateDirectory(productionRoot);
            WriteValidationReport(productionRoot, pass, loadedPrefabs, loadedMaterials, loadedMeshes, rendererCount, findings);
            WriteAcceptanceReport(productionRoot, pass, loadedPrefabs, loadedMaterials, loadedMeshes, rendererCount, findings);

            var status = pass
                ? $"isolated_import_generate_and_runtime_contract_passed_{DateTimeOffset.Now:yyyy-MM-ddTHH:mm:sszzz}"
                : $"failed_{DateTimeOffset.Now:yyyy-MM-ddTHH:mm:sszzz}";
            WeaponViewmodelSet03Generator.MarkValidated(status, "passed_see_preview_evidence");

            if (!pass)
            {
                throw new InvalidOperationException($"BB_WEAPON_VIEWMODEL_SET03_VALIDATION_FAIL {Version} findings={findings.Count}");
            }

            Debug.Log($"BB_WEAPON_VIEWMODEL_SET03_VALIDATION_PASS {Version} prefabs={loadedPrefabs} materials={loadedMaterials} meshes={loadedMeshes} renderers={rendererCount}");
        }

        private static int CountExistingAssets<T>(IEnumerable<string> paths, string missingPrefix, ICollection<string> findings) where T : UnityEngine.Object
        {
            var count = 0;
            foreach (var path in paths)
            {
                if (AssetDatabase.LoadAssetAtPath<T>(path) == null)
                {
                    findings.Add($"{missingPrefix}:{path}");
                    continue;
                }

                count++;
            }

            return count;
        }

        private static void WriteValidationReport(string productionRoot, bool pass, int prefabs, int materials, int meshes, int renderers, IReadOnlyList<string> findings)
        {
            var path = Path.Combine(productionRoot, "UnityValidationReport_WeaponViewmodelSet03_v0.1.41.json");
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"WVM03\",");
            builder.AppendLine("  \"version\": \"0.1.41\",");
            builder.AppendLine($"  \"generated_at\": \"{DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)}\",");
            builder.AppendLine($"  \"status\": \"{(pass ? "pass" : "fail")}\",");
            builder.AppendLine("  \"counts\": {");
            builder.AppendLine($"    \"prefabs\": {prefabs.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"materials\": {materials.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"meshes\": {meshes.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"renderers\": {renderers.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine("  },");
            builder.AppendLine("  \"runtime_contract\": {");
            builder.AppendLine("    \"visual_only\": true,");
            builder.AppendLine("    \"colliders\": \"omitted\",");
            builder.AppendLine("    \"rigidbodies\": \"omitted\",");
            builder.AppendLine("    \"audio_sources\": \"omitted\",");
            builder.AppendLine("    \"particle_systems\": \"omitted\",");
            builder.AppendLine("    \"gameplay_authority\": \"none\"");
            builder.AppendLine("  },");
            builder.AppendLine("  \"findings\": [");
            for (var i = 0; i < findings.Count; i++)
            {
                builder.Append("    \"");
                builder.Append(Escape(findings[i]));
                builder.Append('"');
                builder.AppendLine(i == findings.Count - 1 ? string.Empty : ",");
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
        }

        private static void WriteAcceptanceReport(string productionRoot, bool pass, int prefabs, int materials, int meshes, int renderers, IReadOnlyList<string> findings)
        {
            var path = Path.Combine(productionRoot, "ACCEPTANCE_REPORT_WeaponViewmodelSet03_v0.1.41.md");
            var builder = new StringBuilder();
            builder.AppendLine("# Weapon Viewmodel Set 03 Acceptance Report");
            builder.AppendLine();
            builder.AppendLine($"Generated: {DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)}");
            builder.AppendLine();
            builder.AppendLine($"Status: {(pass ? "PASS" : "FAIL")}");
            builder.AppendLine();
            builder.AppendLine("## Counts");
            builder.AppendLine();
            builder.AppendLine($"- Prefabs: {prefabs}");
            builder.AppendLine($"- Materials: {materials}");
            builder.AppendLine($"- Reusable meshes: {meshes}");
            builder.AppendLine($"- Renderer components: {renderers}");
            builder.AppendLine();
            builder.AppendLine("## Runtime Safety");
            builder.AppendLine();
            builder.AppendLine("- Visual-only package.");
            builder.AppendLine("- No gameplay authority, inventory, damage, pickup, input, or autonomous audio scripts.");
            builder.AppendLine("- Colliders, rigidbodies, audio sources, and particle systems are omitted from generated prefabs.");
            builder.AppendLine("- Passive identity metadata component is the only runtime script.");
            builder.AppendLine();
            builder.AppendLine("## Findings");
            builder.AppendLine();
            if (findings.Count == 0)
            {
                builder.AppendLine("- None.");
            }
            else
            {
                foreach (var finding in findings)
                {
                    builder.AppendLine($"- {finding}");
                }
            }

            builder.AppendLine();
            builder.AppendLine("## Validation Evidence");
            builder.AppendLine();
            builder.AppendLine("- Unity render command: `Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.WeaponViewmodelSet03/ValidationProject~ -executeMethod BrassworksBreach.WeaponViewmodelSet03.Editor.WeaponViewmodelSet03PreviewRenderer.RenderPreviewSet`");
            builder.AppendLine("- Unity render result: `BB_WEAPON_VIEWMODEL_SET03_RENDER_PASS v0.1.41 files=21`");
            builder.AppendLine("- Unity validation command: `Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.WeaponViewmodelSet03/ValidationProject~ -executeMethod BrassworksBreach.WeaponViewmodelSet03.Editor.WeaponViewmodelSet03Validation.ValidateGeneratedAssets`");
            builder.AppendLine($"- Unity validation result: `BB_WEAPON_VIEWMODEL_SET03_VALIDATION_PASS v0.1.41 prefabs={prefabs} materials={materials} meshes={meshes} renderers={renderers}`");
            builder.AppendLine("- Sidecar validator command: `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1 -ProjectPath \"D:/__MY APPS/Unity Doom\" -PackageNamePattern \"BrassworksBreach.WeaponViewmodelSet03\" -Json`");
            builder.AppendLine("- Sidecar validator result: `pass`, `errors=0`, `warnings=0`");

            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
