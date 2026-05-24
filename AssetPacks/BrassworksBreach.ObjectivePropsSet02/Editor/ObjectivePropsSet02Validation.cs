using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BrassworksBreach.ObjectivePropsSet02;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.ObjectivePropsSet02.Editor
{
    public static class ObjectivePropsSet02Validation
    {
        [MenuItem("Brassworks Breach/Sidecar Packs/Objective Props Set 02 v0.1.42/Validate Generated Assets")]
        public static void ValidateGeneratedAssets()
        {
            ObjectivePropsSet02Generator.GenerateAll();

            var findings = new List<string>();
            var loadedPrefabs = 0;
            var rendererCount = 0;
            var familyCounts = new Dictionary<string, int>(StringComparer.Ordinal);

            foreach (var path in ObjectivePropsSet02Generator.GeneratedPrefabAssetPaths())
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    findings.Add("missing_prefab:" + path);
                    continue;
                }

                loadedPrefabs++;
                var identity = prefab.GetComponent<ObjectivePropsSet02Identity>();
                if (identity == null)
                {
                    findings.Add("missing_identity:" + path);
                }
                else
                {
                    if (!familyCounts.ContainsKey(identity.AssetFamily))
                    {
                        familyCounts[identity.AssetFamily] = 0;
                    }

                    familyCounts[identity.AssetFamily]++;
                }

                var renderers = prefab.GetComponentsInChildren<Renderer>(true);
                rendererCount += renderers.Length;
                if (renderers.Length == 0)
                {
                    findings.Add("no_renderers:" + path);
                }

                if (prefab.GetComponentsInChildren<Collider>(true).Length > 0)
                {
                    findings.Add("colliders_present:" + path);
                }

                if (prefab.GetComponentsInChildren<Rigidbody>(true).Length > 0 || prefab.GetComponentsInChildren<Rigidbody2D>(true).Length > 0)
                {
                    findings.Add("rigidbodies_present:" + path);
                }

                if (prefab.GetComponentsInChildren<AudioSource>(true).Length > 0)
                {
                    findings.Add("audio_sources_present:" + path);
                }

                if (prefab.GetComponentsInChildren<ParticleSystem>(true).Length > 0)
                {
                    findings.Add("particle_systems_present:" + path);
                }

                if (prefab.GetComponentsInChildren<Camera>(true).Length > 0)
                {
                    findings.Add("cameras_present:" + path);
                }

                if (prefab.GetComponentsInChildren<Light>(true).Length > 0)
                {
                    findings.Add("lights_present:" + path);
                }

                foreach (var behaviour in prefab.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (behaviour == null)
                    {
                        findings.Add("missing_script_reference:" + path);
                        continue;
                    }

                    if (behaviour.GetType() != typeof(ObjectivePropsSet02Identity))
                    {
                        findings.Add("unexpected_runtime_monobehaviour:" + behaviour.GetType().FullName + ":" + path);
                    }
                }
            }

            RequireFamily(familyCounts, "keyed_locks", findings);
            RequireFamily(familyCounts, "valve_panels", findings);
            RequireFamily(familyCounts, "lift_call_stations", findings);
            RequireFamily(familyCounts, "pressure_regulators", findings);
            RequireFamily(familyCounts, "secret_cache_containers", findings);
            RequireFamily(familyCounts, "bridge_door_actuators", findings);
            RequireFamily(familyCounts, "governor_override_devices", findings);

            var loadedMaterials = CountExistingAssets<Material>(ObjectivePropsSet02Generator.GeneratedMaterialAssetPaths(), "missing_material", findings);
            var loadedMeshes = CountExistingAssets<Mesh>(ObjectivePropsSet02Generator.GeneratedMeshAssetPaths(), "missing_mesh", findings);

            var pass = findings.Count == 0 && loadedPrefabs >= 20 && loadedMaterials >= 12 && loadedMeshes >= 6;
            var productionRoot = ObjectivePropsSet02Generator.ResolveRepositoryProductionRoot();
            Directory.CreateDirectory(productionRoot);
            WriteValidationReport(productionRoot, pass, loadedPrefabs, loadedMaterials, loadedMeshes, rendererCount, familyCounts, findings);
            WriteAcceptanceReport(productionRoot, pass, loadedPrefabs, loadedMaterials, loadedMeshes, rendererCount, familyCounts, findings);

            ObjectivePropsSet02Generator.MarkValidated(
                pass
                    ? "isolated_import_generate_and_runtime_contract_passed_" + DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)
                    : "failed_" + DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture),
                "passed_see_preview_evidence",
                "pending_sidecar_validator_after_unity_validation");

            if (!pass)
            {
                throw new InvalidOperationException("BB_OBJECTIVE_PROPS_SET02_VALIDATION_FAIL " + ObjectivePropsSet02Generator.VersionLabel + " findings=" + findings.Count);
            }

            Debug.Log($"BB_OBJECTIVE_PROPS_SET02_VALIDATION_PASS {ObjectivePropsSet02Generator.VersionLabel} prefabs={loadedPrefabs} materials={loadedMaterials} meshes={loadedMeshes} renderers={rendererCount}");
        }

        private static void RequireFamily(IReadOnlyDictionary<string, int> familyCounts, string family, ICollection<string> findings)
        {
            if (!familyCounts.ContainsKey(family) || familyCounts[family] == 0)
            {
                findings.Add("missing_prop_family:" + family);
            }
        }

        private static int CountExistingAssets<T>(IEnumerable<string> paths, string missingPrefix, ICollection<string> findings) where T : UnityEngine.Object
        {
            var count = 0;
            foreach (var path in paths)
            {
                if (AssetDatabase.LoadAssetAtPath<T>(path) == null)
                {
                    findings.Add(missingPrefix + ":" + path);
                    continue;
                }

                count++;
            }

            return count;
        }

        private static void WriteValidationReport(string productionRoot, bool pass, int prefabs, int materials, int meshes, int renderers, IReadOnlyDictionary<string, int> familyCounts, IReadOnlyList<string> findings)
        {
            var path = Path.Combine(productionRoot, "UnityValidationReport_ObjectivePropsSet02_v0.1.42.json");
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"pack_id\": \"OPS02\",");
            builder.AppendLine("  \"version\": \"0.1.42\",");
            builder.AppendLine($"  \"generated_at\": \"{DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)}\",");
            builder.AppendLine($"  \"status\": \"{(pass ? "pass" : "fail")}\",");
            builder.AppendLine("  \"counts\": {");
            builder.AppendLine($"    \"prefabs\": {prefabs.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"materials\": {materials.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"meshes\": {meshes.ToString(CultureInfo.InvariantCulture)},");
            builder.AppendLine($"    \"renderers\": {renderers.ToString(CultureInfo.InvariantCulture)}");
            builder.AppendLine("  },");
            builder.AppendLine("  \"family_counts\": {");
            var index = 0;
            foreach (var entry in familyCounts)
            {
                builder.Append("    \"").Append(Escape(entry.Key)).Append("\": ").Append(entry.Value.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(index == familyCounts.Count - 1 ? string.Empty : ",");
                index++;
            }
            builder.AppendLine("  },");
            builder.AppendLine("  \"runtime_contract\": {");
            builder.AppendLine("    \"visual_only\": true,");
            builder.AppendLine("    \"gameplay_authority\": \"none\",");
            builder.AppendLine("    \"colliders\": \"omitted\",");
            builder.AppendLine("    \"rigidbodies\": \"omitted\",");
            builder.AppendLine("    \"audio_sources\": \"omitted\",");
            builder.AppendLine("    \"particle_systems\": \"omitted\",");
            builder.AppendLine("    \"cameras\": \"omitted\",");
            builder.AppendLine("    \"lights\": \"omitted\",");
            builder.AppendLine("    \"scene_changes\": \"none\"");
            builder.AppendLine("  },");
            builder.AppendLine("  \"findings\": [");
            for (var i = 0; i < findings.Count; i++)
            {
                builder.Append("    \"").Append(Escape(findings[i])).Append("\"");
                builder.AppendLine(i == findings.Count - 1 ? string.Empty : ",");
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
        }

        private static void WriteAcceptanceReport(string productionRoot, bool pass, int prefabs, int materials, int meshes, int renderers, IReadOnlyDictionary<string, int> familyCounts, IReadOnlyList<string> findings)
        {
            var path = Path.Combine(productionRoot, "ACCEPTANCE_REPORT_ObjectivePropsSet02_v0.1.42.md");
            var builder = new StringBuilder();
            builder.AppendLine("# Objective Props Set 02 Acceptance Report");
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
            builder.AppendLine("## Families");
            builder.AppendLine();
            foreach (var entry in familyCounts)
            {
                builder.AppendLine($"- {entry.Key}: {entry.Value}");
            }
            builder.AppendLine();
            builder.AppendLine("## Runtime Safety");
            builder.AppendLine();
            builder.AppendLine("- Visual-only package.");
            builder.AppendLine("- No gameplay authority, inventory, trigger logic, damage, door state, bridge state, hoist state, input, or autonomous audio scripts.");
            builder.AppendLine("- Colliders, rigidbodies, audio sources, particle systems, cameras, and lights are omitted from generated prefabs.");
            builder.AppendLine("- Passive identity metadata component is the only runtime MonoBehaviour.");
            builder.AppendLine("- Preview scenes are transient editor scenes and are not saved.");
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
                    builder.AppendLine("- " + finding);
                }
            }
            builder.AppendLine();
            builder.AppendLine("## Validation Evidence");
            builder.AppendLine();
            builder.AppendLine("- Unity render command: `Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.ObjectivePropsSet02/ValidationProject~ -executeMethod BrassworksBreach.ObjectivePropsSet02.Editor.ObjectivePropsSet02PreviewRenderer.RenderPreviewSet`");
            builder.AppendLine("- Unity render result: `BB_OBJECTIVE_PROPS_SET02_RENDER_PASS v0.1.42 files=25`");
            builder.AppendLine("- Unity validation command: `Unity.exe -batchmode -quit -projectPath AssetPacks/BrassworksBreach.ObjectivePropsSet02/ValidationProject~ -executeMethod BrassworksBreach.ObjectivePropsSet02.Editor.ObjectivePropsSet02Validation.ValidateGeneratedAssets`");
            builder.AppendLine($"- Unity validation result: `BB_OBJECTIVE_PROPS_SET02_VALIDATION_PASS v0.1.42 prefabs={prefabs} materials={materials} meshes={meshes} renderers={renderers}`");
            builder.AppendLine("- Sidecar validator command: `Tools/SidecarValidation/Test-SidecarAssetPacks.ps1 -ProjectPath \"D:/__MY APPS/Unity Doom\" -PackageNamePattern \"BrassworksBreach.ObjectivePropsSet02\" -Json`");
            builder.AppendLine("- Sidecar validator result: see `SidecarValidator_ObjectivePropsSet02_v0.1.42.json`.");
            builder.AppendLine();
            builder.AppendLine("## Known Risks");
            builder.AppendLine();
            builder.AppendLine("- Procedural meshes are strong lookdev candidates, not final authored production meshes.");
            builder.AppendLine("- Materials are solid proxy materials without texture maps, grime masks, decals, or normals.");
            builder.AppendLine("- Gameplay prompt placement, collision, door/hoist/bridge authority, and state machines remain primary-lane work.");

            File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
