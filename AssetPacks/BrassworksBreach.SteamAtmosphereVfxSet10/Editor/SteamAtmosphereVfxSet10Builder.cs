using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace BrassworksBreach.SteamAtmosphereVfxSet10.Editor
{
    public static class SteamAtmosphereVfxSet10Builder
    {
        private const string PackageName = "com.brassworks.sidecar.steam-atmosphere-vfx-set10";
        private const string PackageAssetRoot = "Packages/" + PackageName;
        private const string Version = "0.1.55";
        private const string BuildId = "p020";
        private const int PreviewWidth = 1536;
        private const int PreviewHeight = 864;

        private static readonly EffectSpec[] Effects =
        {
            new EffectSpec("WarmGaslightHaze", "warm gaslight haze wall bloom", "gaslight-haze", new Color(1.00f, 0.59f, 0.22f, 1f), 0.58f, new Vector2(2.8f, 2.0f), 1, "broad amber glow behind cage lamps"),
            new EffectSpec("ThinSteamWisp", "thin vertical steam wisp", "steam-wisp", new Color(0.82f, 0.78f, 0.68f, 1f), 0.46f, new Vector2(1.25f, 2.8f), 3, "semi-transparent rising steam trails for pipe leaks"),
            new EffectSpec("LowFloorMist", "low floor mist shelf", "floor-mist", new Color(0.70f, 0.67f, 0.58f, 1f), 0.34f, new Vector2(4.2f, 1.0f), 3, "thin rolling fog layer that catches warm floor reflections"),
            new EffectSpec("CondensationWetGlint", "condensation wet glint cards", "wet-glint", new Color(1.00f, 0.83f, 0.48f, 1f), 0.50f, new Vector2(2.0f, 0.8f), 2, "small anisotropic shine streaks for wet stone and brass edges"),
            new EffectSpec("SootStreakOverlay", "soot streak wall overlay", "soot-streak", new Color(0.05f, 0.045f, 0.035f, 1f), 0.62f, new Vector2(2.6f, 2.2f), 2, "vertical blackened grime cards under vents and pressure seams"),
            new EffectSpec("AmberLightShaft", "subtle amber diagonal light shaft", "light-shaft", new Color(1.00f, 0.68f, 0.25f, 1f), 0.36f, new Vector2(3.2f, 1.5f), 2, "transparent beam cards for gaslight through haze"),
            new EffectSpec("HeatShimmerProxy", "heat shimmer proxy plane", "heat-shimmer", new Color(0.88f, 0.74f, 0.48f, 1f), 0.22f, new Vector2(1.6f, 1.7f), 2, "low-alpha wavy proxy for hot boilers and vent exhaust"),
            new EffectSpec("PipeLeakSteamJet", "angled pipe leak steam jet", "steam-jet", new Color(0.88f, 0.84f, 0.74f, 1f), 0.44f, new Vector2(1.9f, 1.0f), 3, "narrow directional steam plume for nozzle and valve dressing"),
            new EffectSpec("BacklitDoorFog", "backlit pressure-door fog", "door-fog", new Color(0.90f, 0.78f, 0.54f, 1f), 0.42f, new Vector2(3.0f, 1.8f), 3, "soft fog halo for sealed door silhouettes and end caps"),
            new EffectSpec("CornerBloomHaze", "corner amber bloom haze", "corner-bloom", new Color(1.00f, 0.53f, 0.19f, 1f), 0.44f, new Vector2(2.4f, 2.4f), 2, "warm corner spill where lamps meet wet brick and pipework"),
            new EffectSpec("CeilingSmokeRoll", "ceiling smoke roll", "ceiling-smoke", new Color(0.48f, 0.43f, 0.35f, 1f), 0.40f, new Vector2(4.0f, 1.2f), 3, "soft soot-laden layer for low ceiling pipes"),
            new EffectSpec("DustMoteSparkle", "gaslight dust mote sparkle", "dust-motes", new Color(1.00f, 0.78f, 0.38f, 1f), 0.35f, new Vector2(2.5f, 1.8f), 1, "tiny warm flecks suspended in lamp haze")
        };

        [MenuItem("Brassworks/Sidecars/Generate Steam Atmosphere VFX Set 10")]
        public static void GeneratePackageAssets()
        {
            string packageRoot = ResolvePackageRoot();
            string projectRoot = ResolveMainProjectRoot(packageRoot);
            string productionRoot = Path.Combine(projectRoot, "Documentation", "AssetProduction", "V0_1_55_SteamAtmosphereVfxSet10");
            string conceptRoot = Path.Combine(projectRoot, "Documentation", "ConceptRenders", "V0_1_55_SteamAtmosphereVfxSet10");
            string planningRoot = Path.Combine(projectRoot, "Documentation", "Planning", "V0_1_55_SteamAtmosphereVfxSet10ImportReadiness");
            string qaRoot = Path.Combine(projectRoot, "Documentation", "QA", "V0_1_55_SteamAtmosphereVfxSet10ImportReadiness");

            EnsurePackageFolders(packageRoot);
            ResetGeneratedFolders(packageRoot, productionRoot, conceptRoot, planningRoot, qaRoot);

            var textures = new Dictionary<string, string>();
            var materials = new Dictionary<string, string>();
            var prefabs = new List<string>();
            var packagePreviews = new List<string>();
            var conceptPreviews = new List<string>();

            foreach (EffectSpec effect in Effects)
            {
                string texturePath = $"{PackageAssetRoot}/Runtime/Textures/SAV10_TEX_{effect.Key}.png";
                SaveTextureAsset(texturePath, CreateEffectTexture(effect, 1024, 1024), false);
                textures.Add(effect.Key, texturePath);

                string materialPath = $"{PackageAssetRoot}/Runtime/Materials/SAV10_MAT_{effect.Key}.mat";
                CreateTransparentMaterial(materialPath, texturePath, effect);
                materials.Add(effect.Key, materialPath);

                string prefabPath = $"{PackageAssetRoot}/Runtime/Prefabs/SAV10_PREFAB_{effect.Key}.prefab";
                CreateCardPrefab(prefabPath, materialPath, effect);
                prefabs.Add(prefabPath);

                Texture2D preview = CreatePreview(effect, 0);
                string fileName = $"SAV10_PREVIEW_{effect.Key}.png";
                string packagePreview = Path.Combine(packageRoot, "Documentation~", "Previews", fileName);
                string conceptPreview = Path.Combine(conceptRoot, fileName);
                SavePngPhysical(packagePreview, preview);
                SavePngPhysical(conceptPreview, preview);
                packagePreviews.Add(ToProjectRelative(packagePreview, projectRoot));
                conceptPreviews.Add(ToProjectRelative(conceptPreview, projectRoot));
            }

            CreateBundlePrefab($"{PackageAssetRoot}/Runtime/Prefabs/SAV10_PREFAB_CorridorAtmosphereBundle_A.prefab", materials);
            prefabs.Add($"{PackageAssetRoot}/Runtime/Prefabs/SAV10_PREFAB_CorridorAtmosphereBundle_A.prefab");
            CreateWetLightBundlePrefab($"{PackageAssetRoot}/Runtime/Prefabs/SAV10_PREFAB_WetLightLayerBundle_A.prefab", materials);
            prefabs.Add($"{PackageAssetRoot}/Runtime/Prefabs/SAV10_PREFAB_WetLightLayerBundle_A.prefab");

            Texture2D contactSheet = CreateContactSheet();
            string packageContact = Path.Combine(packageRoot, "Documentation~", "Previews", "SAV10_PREVIEW_contact-sheet.png");
            string conceptContact = Path.Combine(conceptRoot, "SAV10_PREVIEW_contact-sheet.png");
            SavePngPhysical(packageContact, contactSheet);
            SavePngPhysical(conceptContact, contactSheet);
            packagePreviews.Add(ToProjectRelative(packageContact, projectRoot));
            conceptPreviews.Add(ToProjectRelative(conceptContact, projectRoot));

            AssetDatabase.Refresh();

            string catalogPath = $"{PackageAssetRoot}/Runtime/Metadata/SAV10_SteamAtmosphereVfxCatalog_v{Version}-{BuildId}.json";
            WriteTextAsset(catalogPath, BuildCatalogJson(textures.Values, materials.Values, prefabs));
            string manifestPath = $"{PackageAssetRoot}/Runtime/Metadata/SAV10_SteamAtmosphereVfxSet10_Manifest_v{Version}-{BuildId}.json";
            WriteTextAsset(manifestPath, BuildManifestJson(textures.Values, materials.Values, prefabs, packagePreviews, conceptPreviews));
            string manifestDocPath = Path.Combine(packageRoot, "Documentation~", "Manifest", $"SAV10_SteamAtmosphereVfxSet10_Manifest_v{Version}-{BuildId}.json");
            File.WriteAllText(manifestDocPath, File.ReadAllText(ToPhysicalPath(manifestPath, packageRoot)), Encoding.UTF8);

            WriteReadmeFiles(packageRoot);
            WriteProductionDocs(productionRoot, planningRoot, qaRoot, conceptRoot, textures.Values, materials.Values, prefabs, conceptPreviews);
            AssetDatabase.Refresh();

            Debug.Log($"SAV10_GENERATE_PASS package={packageRoot} prefabs={prefabs.Count} materials={materials.Count} textures={textures.Count} previews={conceptPreviews.Count}");
        }

        private static string ResolvePackageRoot()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(SteamAtmosphereVfxSet10Builder).Assembly);
            if (packageInfo != null && !string.IsNullOrWhiteSpace(packageInfo.resolvedPath))
            {
                return NormalizePath(packageInfo.resolvedPath);
            }

            DirectoryInfo projectDir = Directory.GetParent(Application.dataPath);
            return NormalizePath(Path.Combine(projectDir.FullName, "Packages", PackageName));
        }

        private static string ResolveMainProjectRoot(string packageRoot)
        {
            DirectoryInfo packageDir = new DirectoryInfo(packageRoot);
            DirectoryInfo assetPacksDir = packageDir.Parent;
            DirectoryInfo projectDir = assetPacksDir == null ? null : assetPacksDir.Parent;
            if (projectDir != null && Directory.Exists(Path.Combine(projectDir.FullName, "Documentation")))
            {
                return NormalizePath(projectDir.FullName);
            }

            DirectoryInfo tempProjectDir = Directory.GetParent(Application.dataPath);
            return NormalizePath(tempProjectDir.FullName);
        }

        private static void EnsurePackageFolders(string packageRoot)
        {
            string[] folders =
            {
                "Runtime/Materials",
                "Runtime/Textures",
                "Runtime/Prefabs",
                "Runtime/Metadata",
                "Documentation~/Manifest",
                "Documentation~/Previews",
                "Samples~/PreviewNotes",
                "Tools"
            };

            foreach (string folder in folders)
            {
                Directory.CreateDirectory(Path.Combine(packageRoot, folder.Replace('/', Path.DirectorySeparatorChar)));
            }
        }

        private static void ResetGeneratedFolders(string packageRoot, params string[] docRoots)
        {
            string[] packageFolders =
            {
                "Runtime/Materials",
                "Runtime/Textures",
                "Runtime/Prefabs",
                "Runtime/Metadata",
                "Documentation~/Manifest",
                "Documentation~/Previews",
                "Samples~/PreviewNotes"
            };

            foreach (string folder in packageFolders)
            {
                string fullPath = Path.Combine(packageRoot, folder.Replace('/', Path.DirectorySeparatorChar));
                if (Directory.Exists(fullPath))
                {
                    foreach (string file in Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories))
                    {
                        File.Delete(file);
                    }
                }
                Directory.CreateDirectory(fullPath);
            }

            foreach (string root in docRoots)
            {
                if (Directory.Exists(root))
                {
                    foreach (string file in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
                    {
                        File.Delete(file);
                    }
                }
                Directory.CreateDirectory(root);
            }
        }

        private static void SaveTextureAsset(string assetPath, Texture2D texture, bool normalMap)
        {
            string packageRoot = ResolvePackageRoot();
            string physicalPath = ToPhysicalPath(assetPath, packageRoot);
            SavePngPhysical(physicalPath, texture);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            if (AssetImporter.GetAtPath(assetPath) is TextureImporter importer)
            {
                importer.textureType = normalMap ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.alphaIsTransparency = !normalMap;
                importer.sRGBTexture = !normalMap;
                importer.mipmapEnabled = true;
                importer.maxTextureSize = 1024;
                importer.SaveAndReimport();
            }
        }

        private static void CreateTransparentMaterial(string materialPath, string texturePath, EffectSpec effect)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.name = $"SAV10_MAT_{effect.Key}";
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            material.SetTexture("_MainTex", texture);
            material.SetColor("_Color", new Color(effect.Tint.r, effect.Tint.g, effect.Tint.b, effect.Alpha));
            material.SetFloat("_Mode", 2f);
            material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetFloat("_Glossiness", effect.Category == "wet-glint" ? 0.86f : 0.18f);
            material.SetFloat("_Metallic", effect.Category == "wet-glint" ? 0.08f : 0f);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.enableInstancing = true;
            material.renderQueue = 3000;
            material.SetOverrideTag("RenderType", "Transparent");
            if (effect.Category.Contains("gaslight") || effect.Category.Contains("light") || effect.Category == "corner-bloom" || effect.Category == "dust-motes")
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", effect.Tint * 0.45f);
            }

            AssetDatabase.CreateAsset(material, materialPath);
        }

        private static void CreateCardPrefab(string prefabPath, string materialPath, EffectSpec effect)
        {
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            GameObject root = new GameObject($"SAV10_PREFAB_{effect.Key}");
            root.transform.position = Vector3.zero;

            for (int i = 0; i < effect.Layers; i++)
            {
                GameObject card = GameObject.CreatePrimitive(PrimitiveType.Quad);
                card.name = $"layer_{i + 1:00}_{effect.Category}";
                UnityEngine.Object.DestroyImmediate(card.GetComponent<Collider>());
                card.transform.SetParent(root.transform, false);
                float zOffset = i * 0.015f;
                card.transform.localPosition = new Vector3((i - 1) * 0.07f, i * 0.04f, zOffset);
                card.transform.localRotation = Quaternion.Euler(0f, 0f, LayerRotation(effect, i));
                card.transform.localScale = new Vector3(effect.Scale.x * (1f + i * 0.18f), effect.Scale.y * (1f + i * 0.08f), 1f);

                MeshRenderer renderer = card.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                renderer.lightProbeUsage = LightProbeUsage.Off;
                renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            }

            PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static void CreateBundlePrefab(string prefabPath, IReadOnlyDictionary<string, string> materials)
        {
            GameObject root = new GameObject("SAV10_PREFAB_CorridorAtmosphereBundle_A");
            string[] keys = { "WarmGaslightHaze", "ThinSteamWisp", "LowFloorMist", "AmberLightShaft", "BacklitDoorFog", "CeilingSmokeRoll" };
            Vector3[] positions =
            {
                new Vector3(-2.2f, 1.6f, 0f),
                new Vector3(-1.1f, 0.7f, 0.05f),
                new Vector3(0f, -1.0f, 0.1f),
                new Vector3(1.0f, 1.5f, 0.15f),
                new Vector3(2.0f, 0.8f, 0.2f),
                new Vector3(0.1f, 2.1f, 0.25f)
            };

            for (int i = 0; i < keys.Length; i++)
            {
                EffectSpec effect = Effects.First(e => e.Key == keys[i]);
                GameObject card = GameObject.CreatePrimitive(PrimitiveType.Quad);
                card.name = $"bundle_{i + 1:00}_{effect.Category}";
                UnityEngine.Object.DestroyImmediate(card.GetComponent<Collider>());
                card.transform.SetParent(root.transform, false);
                card.transform.localPosition = positions[i];
                card.transform.localScale = new Vector3(effect.Scale.x, effect.Scale.y, 1f);
                card.transform.localRotation = Quaternion.Euler(0f, 0f, i == 3 ? -18f : 0f);
                MeshRenderer renderer = card.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(materials[keys[i]]);
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static void CreateWetLightBundlePrefab(string prefabPath, IReadOnlyDictionary<string, string> materials)
        {
            GameObject root = new GameObject("SAV10_PREFAB_WetLightLayerBundle_A");
            string[] keys = { "CondensationWetGlint", "SootStreakOverlay", "DustMoteSparkle", "CornerBloomHaze", "HeatShimmerProxy", "PipeLeakSteamJet" };

            for (int i = 0; i < keys.Length; i++)
            {
                EffectSpec effect = Effects.First(e => e.Key == keys[i]);
                GameObject card = GameObject.CreatePrimitive(PrimitiveType.Quad);
                card.name = $"wet_light_{i + 1:00}_{effect.Category}";
                UnityEngine.Object.DestroyImmediate(card.GetComponent<Collider>());
                card.transform.SetParent(root.transform, false);
                card.transform.localPosition = new Vector3(-2f + i * 0.78f, -0.4f + (i % 2) * 0.45f, i * 0.02f);
                card.transform.localScale = new Vector3(effect.Scale.x * 0.82f, effect.Scale.y * 0.82f, 1f);
                card.transform.localRotation = Quaternion.Euler(0f, 0f, LayerRotation(effect, i));
                MeshRenderer renderer = card.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(materials[keys[i]]);
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static float LayerRotation(EffectSpec effect, int layer)
        {
            return effect.Category switch
            {
                "light-shaft" => -23f + layer * 4f,
                "steam-jet" => 17f + layer * 5f,
                "soot-streak" => -2f + layer * 3f,
                "heat-shimmer" => 4f + layer * 8f,
                _ => -4f + layer * 5f
            };
        }

        private static Texture2D CreateEffectTexture(EffectSpec effect, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color32[] pixels = new Color32[width * height];
            for (int y = 0; y < height; y++)
            {
                float v = y / (float)(height - 1);
                for (int x = 0; x < width; x++)
                {
                    float u = x / (float)(width - 1);
                    float alpha = EffectAlpha(effect.Category, u, v, x, y);
                    Color color = effect.Tint;
                    if (effect.Category == "soot-streak")
                    {
                        color = Color.Lerp(new Color(0.015f, 0.012f, 0.010f, 1f), new Color(0.09f, 0.075f, 0.055f, 1f), Noise01(x * 7 + y * 13 + 31));
                    }
                    else if (effect.Category == "wet-glint")
                    {
                        color = Color.Lerp(new Color(0.45f, 0.36f, 0.20f, 1f), new Color(1.0f, 0.88f, 0.56f, 1f), Mathf.Clamp01(alpha * 2.1f));
                    }

                    pixels[y * width + x] = ToColor32(color, Mathf.Clamp01(alpha * effect.Alpha));
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply();
            return texture;
        }

        private static float EffectAlpha(string category, float u, float v, int x, int y)
        {
            float n = FractalNoise(u * 8.0f, v * 8.0f);
            float cx = u - 0.5f;
            float cy = v - 0.5f;
            return category switch
            {
                "gaslight-haze" => Mathf.Exp(-(cx * cx * 5.2f + cy * cy * 8.0f)) * (0.62f + n * 0.38f),
                "steam-wisp" => SteamColumn(u, v, n),
                "floor-mist" => Mathf.SmoothStep(0.25f, 0.0f, Mathf.Abs(v - 0.18f)) * (0.35f + n * 0.65f),
                "wet-glint" => WetGlints(u, v, n),
                "soot-streak" => SootStreaks(u, v, n),
                "light-shaft" => LightShaft(u, v, n),
                "heat-shimmer" => HeatShimmer(u, v, n),
                "steam-jet" => SteamJet(u, v, n),
                "door-fog" => Mathf.Exp(-(cx * cx * 7.0f + cy * cy * 2.7f)) * (0.45f + n * 0.55f),
                "corner-bloom" => Mathf.Exp(-(u * u * 4.0f + (1f - v) * (1f - v) * 5.0f)) * (0.55f + n * 0.45f),
                "ceiling-smoke" => Mathf.SmoothStep(0.15f, 0.0f, Mathf.Abs(v - 0.82f)) * (0.35f + n * 0.65f),
                "dust-motes" => DustMotes(u, v, x, y),
                _ => n * 0.5f
            };
        }

        private static float SteamColumn(float u, float v, float n)
        {
            float center = 0.5f + Mathf.Sin(v * 12f) * 0.10f + Mathf.Sin(v * 27f) * 0.035f;
            float width = Mathf.Lerp(0.06f, 0.24f, v);
            float plume = Mathf.Exp(-Mathf.Pow((u - center) / width, 2f));
            return plume * Mathf.SmoothStep(0.02f, 0.18f, v) * Mathf.SmoothStep(1.0f, 0.70f, v) * (0.45f + n * 0.75f);
        }

        private static float SteamJet(float u, float v, float n)
        {
            float line = v - (0.25f + u * 0.45f + Mathf.Sin(u * 16f) * 0.025f);
            float body = Mathf.Exp(-(line * line) / 0.012f) * Mathf.SmoothStep(0.04f, 0.26f, u) * Mathf.SmoothStep(1.0f, 0.72f, u);
            return body * (0.50f + n * 0.65f);
        }

        private static float WetGlints(float u, float v, float n)
        {
            float lineA = Mathf.Abs(Mathf.Sin((u * 26f + v * 5f) * Mathf.PI));
            float lineB = Mathf.Abs(Mathf.Sin((u * 11f - v * 17f) * Mathf.PI));
            float sparkle = Mathf.Pow(1f - Mathf.Min(lineA, lineB), 9f);
            float patch = Mathf.SmoothStep(0.34f, 0.0f, Mathf.Abs(v - 0.42f)) * Mathf.SmoothStep(0.48f, 0.0f, Mathf.Abs(u - 0.5f));
            return Mathf.Clamp01((sparkle * 1.2f + n * 0.18f) * patch);
        }

        private static float SootStreaks(float u, float v, float n)
        {
            float streak = Mathf.Pow(1f - Mathf.Abs(Mathf.Sin((u * 13f + n * 0.8f) * Mathf.PI)), 3.5f);
            float drip = Mathf.SmoothStep(1.0f, 0.15f, v);
            return Mathf.Clamp01(streak * drip * (0.35f + n * 0.75f));
        }

        private static float LightShaft(float u, float v, float n)
        {
            float diagonal = Mathf.Abs(v - (0.78f - u * 0.48f));
            float shaft = Mathf.SmoothStep(0.22f, 0.0f, diagonal) * Mathf.SmoothStep(0.02f, 0.25f, u) * Mathf.SmoothStep(1.0f, 0.64f, u);
            return shaft * (0.45f + n * 0.4f);
        }

        private static float HeatShimmer(float u, float v, float n)
        {
            float waves = Mathf.Abs(Mathf.Sin((u * 19f + Mathf.Sin(v * 21f) * 0.4f) * Mathf.PI));
            float envelope = Mathf.Exp(-((u - 0.5f) * (u - 0.5f) * 5f + (v - 0.5f) * (v - 0.5f) * 2f));
            return Mathf.Pow(1f - waves, 2.4f) * envelope * (0.25f + n * 0.4f);
        }

        private static float DustMotes(float u, float v, int x, int y)
        {
            float h = Noise01(x * 1299721 + y * 31337);
            if (h < 0.993f)
            {
                return 0f;
            }

            float vignette = Mathf.Exp(-((u - 0.5f) * (u - 0.5f) * 3.2f + (v - 0.5f) * (v - 0.5f) * 3.2f));
            return Mathf.Clamp01((h - 0.993f) * 120f) * vignette;
        }

        private static Texture2D CreatePreview(EffectSpec effect, int variant)
        {
            Texture2D preview = new Texture2D(PreviewWidth, PreviewHeight, TextureFormat.RGBA32, false);
            Color32[] pixels = new Color32[PreviewWidth * PreviewHeight];
            for (int y = 0; y < PreviewHeight; y++)
            {
                float v = y / (float)(PreviewHeight - 1);
                for (int x = 0; x < PreviewWidth; x++)
                {
                    float u = x / (float)(PreviewWidth - 1);
                    Color baseColor = CorridorBackground(u, v, x, y);
                    float effectU = Mathf.Clamp01((u - 0.12f) / 0.76f);
                    float effectV = Mathf.Clamp01((v - 0.10f) / 0.80f);
                    float alpha = EffectAlpha(effect.Category, effectU, effectV, x + variant * 19, y + variant * 31) * effect.Alpha;
                    Color effectColor = effect.Tint;
                    if (effect.Category == "soot-streak")
                    {
                        effectColor = new Color(0.018f, 0.014f, 0.011f, 1f);
                        alpha *= 0.85f;
                    }
                    else if (effect.Category == "wet-glint")
                    {
                        effectColor = new Color(1.0f, 0.78f, 0.36f, 1f);
                        alpha *= 1.2f;
                    }

                    Color blended = Color.Lerp(baseColor, effectColor, Mathf.Clamp01(alpha));
                    if (effect.Category.Contains("gaslight") || effect.Category.Contains("light") || effect.Category == "corner-bloom")
                    {
                        blended += effect.Tint * Mathf.Clamp01(alpha) * 0.35f;
                    }

                    pixels[y * PreviewWidth + x] = ToColor32(blended, 1f);
                }
            }

            preview.SetPixels32(pixels);
            preview.Apply();
            return preview;
        }

        private static Color CorridorBackground(float u, float v, int x, int y)
        {
            bool floor = v < 0.38f;
            bool ceiling = v > 0.78f;
            float n = FractalNoise(u * 14f, v * 14f);
            Color brickBase = ceiling
                ? new Color(0.14f, 0.105f, 0.075f)
                : new Color(0.12f, 0.10f, 0.085f);
            Color floorBase = new Color(0.16f, 0.13f, 0.10f);
            Color color = floor ? floorBase : brickBase;
            color *= 0.82f + n * 0.34f;

            float brickUScale = floor ? 14f : 22f;
            float brickVScale = floor ? 5f : 12f;
            float row = Mathf.Floor(v * brickVScale);
            float offset = (row % 2f) * 0.5f;
            float mortarU = Mathf.Min(Frac(u * brickUScale + offset), 1f - Frac(u * brickUScale + offset));
            float mortarV = Mathf.Min(Frac(v * brickVScale), 1f - Frac(v * brickVScale));
            float mortar = Mathf.SmoothStep(0.018f, 0.0f, Mathf.Min(mortarU, mortarV));
            color = Color.Lerp(color, new Color(0.035f, 0.030f, 0.026f), mortar * 0.82f);

            float leftLamp = LampGlow(u, v, 0.17f, 0.55f);
            float rightLamp = LampGlow(u, v, 0.83f, 0.55f);
            float warm = Mathf.Clamp01(leftLamp + rightLamp);
            color += new Color(1.0f, 0.55f, 0.22f) * warm * 0.55f;

            if (floor)
            {
                float reflect = (Mathf.Exp(-Mathf.Abs(u - 0.17f) * 10f) + Mathf.Exp(-Mathf.Abs(u - 0.83f) * 10f)) * Mathf.SmoothStep(0.38f, 0.06f, v);
                color += new Color(1.0f, 0.62f, 0.26f) * reflect * 0.28f;
            }

            float vignette = Mathf.SmoothStep(0.94f, 0.28f, Mathf.Abs(u - 0.5f)) * Mathf.SmoothStep(1.05f, 0.08f, Mathf.Abs(v - 0.50f));
            color *= 0.56f + vignette * 0.52f;
            return color;
        }

        private static float LampGlow(float u, float v, float cx, float cy)
        {
            float dx = (u - cx) * 2.1f;
            float dy = (v - cy) * 2.8f;
            return Mathf.Exp(-(dx * dx + dy * dy) * 7.0f);
        }

        private static Texture2D CreateContactSheet()
        {
            int columns = 4;
            int rows = 3;
            int tileW = 512;
            int tileH = 288;
            Texture2D sheet = new Texture2D(columns * tileW, rows * tileH, TextureFormat.RGBA32, false);
            Color32[] clear = Enumerable.Repeat(ToColor32(new Color(0.025f, 0.025f, 0.023f), 1f), columns * tileW * rows * tileH).ToArray();
            sheet.SetPixels32(clear);

            for (int i = 0; i < Effects.Length; i++)
            {
                Texture2D preview = CreatePreview(Effects[i], i);
                int ox = (i % columns) * tileW;
                int oy = (rows - 1 - i / columns) * tileH;
                for (int y = 0; y < tileH; y++)
                {
                    for (int x = 0; x < tileW; x++)
                    {
                        int sx = Mathf.Clamp(Mathf.RoundToInt(x / (float)(tileW - 1) * (PreviewWidth - 1)), 0, PreviewWidth - 1);
                        int sy = Mathf.Clamp(Mathf.RoundToInt(y / (float)(tileH - 1) * (PreviewHeight - 1)), 0, PreviewHeight - 1);
                        Color c = preview.GetPixel(sx, sy);
                        bool border = x < 3 || y < 3 || x > tileW - 4 || y > tileH - 4;
                        if (border)
                        {
                            c = Color.Lerp(c, new Color(0.90f, 0.53f, 0.20f), 0.75f);
                        }
                        sheet.SetPixel(ox + x, oy + y, c);
                    }
                }
            }

            sheet.Apply();
            return sheet;
        }

        private static void SavePngPhysical(string path, Texture2D texture)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllBytes(path, texture.EncodeToPNG());
        }

        private static void WriteTextAsset(string assetPath, string text)
        {
            string packageRoot = ResolvePackageRoot();
            string physicalPath = ToPhysicalPath(assetPath, packageRoot);
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath)!);
            File.WriteAllText(physicalPath, text, Encoding.UTF8);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        private static string BuildCatalogJson(IEnumerable<string> textures, IEnumerable<string> materials, IEnumerable<string> prefabs)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"schema\": \"brassworks.sidecar.atmosphere_catalog.v1\",");
            sb.AppendLine("  \"pack_id\": \"SAV10\",");
            sb.AppendLine($"  \"version\": \"{Version}-{BuildId}\",");
            sb.AppendLine("  \"asset_families\": [");
            for (int i = 0; i < Effects.Length; i++)
            {
                EffectSpec effect = Effects[i];
                sb.AppendLine("    {");
                sb.AppendLine($"      \"key\": \"{effect.Key}\",");
                sb.AppendLine($"      \"category\": \"{effect.Category}\",");
                sb.AppendLine($"      \"display_name\": \"{JsonEscape(effect.DisplayName)}\",");
                sb.AppendLine($"      \"intended_use\": \"{JsonEscape(effect.Intent)}\",");
                sb.AppendLine($"      \"material\": \"AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10/Runtime/Materials/SAV10_MAT_{effect.Key}.mat\",");
                sb.AppendLine($"      \"texture\": \"AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10/Runtime/Textures/SAV10_TEX_{effect.Key}.png\",");
                sb.AppendLine($"      \"prefab\": \"AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10/Runtime/Prefabs/SAV10_PREFAB_{effect.Key}.prefab\"");
                sb.AppendLine(i == Effects.Length - 1 ? "    }" : "    },");
            }
            sb.AppendLine("  ],");
            sb.AppendLine("  \"counts\": {");
            sb.AppendLine($"    \"textures\": {textures.Count()},");
            sb.AppendLine($"    \"materials\": {materials.Count()},");
            sb.AppendLine($"    \"prefabs\": {prefabs.Count()}");
            sb.AppendLine("  }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string BuildManifestJson(IEnumerable<string> textures, IEnumerable<string> materials, IEnumerable<string> prefabs, IEnumerable<string> packagePreviews, IEnumerable<string> conceptPreviews)
        {
            string generatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            var exportArtifacts = new List<string>
            {
                "AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10/package.json",
                "AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10/README.md",
                $"AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10/Runtime/Metadata/SAV10_SteamAtmosphereVfxCatalog_v{Version}-{BuildId}.json"
            };
            exportArtifacts.AddRange(textures.Select(ToMainProjectPath));
            exportArtifacts.AddRange(materials.Select(ToMainProjectPath));
            exportArtifacts.AddRange(prefabs.Select(ToMainProjectPath));
            exportArtifacts.AddRange(packagePreviews);
            exportArtifacts.AddRange(conceptPreviews);

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"pack_id\": \"SAV10\",");
            sb.AppendLine("  \"display_name\": \"Steam Atmosphere VFX Set 10\",");
            sb.AppendLine($"  \"version\": \"{Version}\",");
            sb.AppendLine($"  \"build_id\": \"{BuildId}\",");
            sb.AppendLine("  \"unity_version\": \"6000.4.6f1\",");
            sb.AppendLine($"  \"generated_at_utc\": \"{generatedAt}\",");
            sb.AppendLine("  \"sidecar_project\": \"UD-SC-VFX-SteamAtmosphereVfxSet10\",");
            sb.AppendLine("  \"owner_lane\": \"sidecar-steam-atmosphere-vfx-set10\",");
            sb.AppendLine("  \"primary_intake_owner\": \"main-lane-art-integration\",");
            sb.AppendLine("  \"canonical_root\": \"AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10\",");
            sb.AppendLine("  \"package_root\": \"AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10\",");
            sb.AppendLine($"  \"package_name\": \"{PackageName}\",");
            sb.AppendLine("  \"common_schema\": \"brassworks.sidecar.visual_pack_manifest.v1\",");
            sb.AppendLine("  \"export_artifacts\": [");
            for (int i = 0; i < exportArtifacts.Count; i++)
            {
                sb.Append("    \"").Append(JsonEscape(exportArtifacts[i])).Append("\"");
                sb.AppendLine(i == exportArtifacts.Count - 1 ? string.Empty : ",");
            }
            sb.AppendLine("  ],");
            sb.AppendLine("  \"asset_counts\": {");
            sb.AppendLine($"    \"generated_prefabs\": {prefabs.Count()},");
            sb.AppendLine($"    \"generated_materials\": {materials.Count()},");
            sb.AppendLine("    \"generated_meshes\": 0,");
            sb.AppendLine($"    \"runtime_texture_pngs\": {textures.Count()},");
            sb.AppendLine("    \"metadata_catalogs\": 2,");
            sb.AppendLine($"    \"preview_pngs_package\": {packagePreviews.Count()},");
            sb.AppendLine($"    \"preview_pngs_documentation\": {conceptPreviews.Count()},");
            sb.AppendLine("    \"runtime_scripts\": 0,");
            sb.AppendLine("    \"editor_generation_scripts\": 1,");
            sb.AppendLine("    \"colliders\": 0,");
            sb.AppendLine("    \"audio\": 0,");
            sb.AppendLine("    \"gameplay_scripts\": 0");
            sb.AppendLine("  },");
            sb.AppendLine("  \"dependencies\": [");
            sb.AppendLine("    \"Unity Package Manager local package reference\",");
            sb.AppendLine("    \"Unity built-in Standard shader\",");
            sb.AppendLine("    \"Unity transparent material settings\",");
            sb.AppendLine("    \"No Blender or external DCC source files\"");
            sb.AppendLine("  ],");
            sb.AppendLine("  \"limitations\": [");
            sb.AppendLine("    \"Transparent cards are static visual proxies; animation should be added later through main project VFX systems or shader graph equivalents.\",");
            sb.AppendLine("    \"Heat shimmer is represented as a low-alpha wavy card, not a true refraction shader.\",");
            sb.AppendLine("    \"Unity temp-project import generation passed; main project import still requires deliberate package manifest intake.\"");
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void WriteReadmeFiles(string packageRoot)
        {
            string sampleDir = Path.Combine(packageRoot, "Samples~", "PreviewNotes");
            Directory.CreateDirectory(sampleDir);
            File.WriteAllText(Path.Combine(sampleDir, "README.md"),
                "# Steam Atmosphere Preview Notes\n\nThese prefabs are visual-only transparent card layers. Place them near lamps, pipes, wet floor seams, pressure doors, and ceiling runs. They include no colliders or gameplay scripts.\n",
                Encoding.UTF8);

            File.WriteAllText(Path.Combine(packageRoot, "Tools", "README.md"),
                "# Tools\n\nThis package was generated through the Editor-only `SteamAtmosphereVfxSet10Builder` in the package `Editor` folder using a temporary isolated Unity project. No main project manifest edits are required to review the file package.\n",
                Encoding.UTF8);
        }

        private static void WriteProductionDocs(string productionRoot, string planningRoot, string qaRoot, string conceptRoot, IEnumerable<string> textures, IEnumerable<string> materials, IEnumerable<string> prefabs, IEnumerable<string> conceptPreviews)
        {
            Directory.CreateDirectory(productionRoot);
            Directory.CreateDirectory(planningRoot);
            Directory.CreateDirectory(qaRoot);
            Directory.CreateDirectory(conceptRoot);

            string inventoryPath = Path.Combine(productionRoot, $"SAV10_AssetInventory_{Version}-{BuildId}.md");
            var inventory = new StringBuilder();
            inventory.AppendLine("# Steam Atmosphere VFX Set 10 Asset Inventory");
            inventory.AppendLine();
            inventory.AppendLine($"Version: {Version}-{BuildId}");
            inventory.AppendLine();
            inventory.AppendLine("## Counts");
            inventory.AppendLine();
            inventory.AppendLine($"- Runtime texture PNGs: {textures.Count()}");
            inventory.AppendLine($"- Materials: {materials.Count()}");
            inventory.AppendLine($"- Prefabs: {prefabs.Count()}");
            inventory.AppendLine($"- Concept preview PNGs: {conceptPreviews.Count()}");
            inventory.AppendLine("- Runtime scripts: 0");
            inventory.AppendLine("- Colliders: 0");
            inventory.AppendLine();
            inventory.AppendLine("## Prefab Families");
            inventory.AppendLine();
            foreach (EffectSpec effect in Effects)
            {
                inventory.AppendLine($"- `SAV10_PREFAB_{effect.Key}.prefab`: {effect.Intent}");
            }
            inventory.AppendLine("- `SAV10_PREFAB_CorridorAtmosphereBundle_A.prefab`: combined room/corridor atmosphere layering block.");
            inventory.AppendLine("- `SAV10_PREFAB_WetLightLayerBundle_A.prefab`: combined wet-glint, soot, heat, and sparkle layering block.");
            File.WriteAllText(inventoryPath, inventory.ToString(), Encoding.UTF8);

            string productionPath = Path.Combine(productionRoot, $"SAV10_ProductionReport_{Version}-{BuildId}.md");
            File.WriteAllText(productionPath,
                "# Steam Atmosphere VFX Set 10 Production Report\n\n" +
                "Status: package candidate generated.\n\n" +
                "The set targets the steampunk north-star corridor by adding the soft visual layer missing from hard surface kits: gaslight diffusion, steam presence, low floor mist, wet specular cards, soot overlays, amber shafts, and heat-shimmer proxies.\n\n" +
                "Generated with Unity Editor APIs through an isolated temporary project. No Blender or external DCC was used.\n",
                Encoding.UTF8);

            string planningPath = Path.Combine(planningRoot, $"SAV10_ImportReadinessNotes_{Version}-{BuildId}.md");
            File.WriteAllText(planningPath,
                "# Steam Atmosphere VFX Set 10 Import Readiness\n\n" +
                "Status: static package candidate ready for art-intake review.\n\n" +
                "Recommended intake path:\n\n" +
                "1. Add the package as a local UPM reference only after visual review.\n" +
                "2. Place bundled atmosphere prefabs into quarantine showcase alcoves first.\n" +
                "3. Replace static heat-shimmer proxies with runtime-safe shader/VFX equivalents only after the Windows route remains stable.\n" +
                "4. Keep all prefabs visual-only; do not add colliders or gameplay components.\n",
                Encoding.UTF8);

            string qaPath = Path.Combine(qaRoot, "SAV10_ValidationReport.md");
            File.WriteAllText(qaPath,
                "# Steam Atmosphere VFX Set 10 Validation Report\n\n" +
                "Status: PASS - generated by Unity batch execution.\n\n" +
                "## Validation Summary\n\n" +
                $"- Prefabs: {prefabs.Count()}\n" +
                $"- Materials: {materials.Count()}\n" +
                $"- Runtime texture PNGs: {textures.Count()}\n" +
                $"- Concept preview PNGs: {conceptPreviews.Count()}\n" +
                "- Runtime scripts: 0\n" +
                "- Gameplay scripts: 0\n" +
                "- Colliders intentionally removed from generated primitive cards.\n\n" +
                "## Limitations\n\n" +
                "- Static transparent-card VFX only; no animation controllers or particle systems yet.\n" +
                "- Heat shimmer is approximated with a wavy low-alpha card.\n" +
                "- Main project package import remains a later deliberate intake step.\n",
                Encoding.UTF8);

            File.WriteAllText(Path.Combine(qaRoot, "SAV10_FinalFileList.txt"),
                string.Join(Environment.NewLine, Directory.GetFiles(Path.Combine(ResolvePackageRoot()), "*", SearchOption.AllDirectories).Select(NormalizePath)) + Environment.NewLine,
                Encoding.UTF8);
        }

        private static Color32 ToColor32(Color color, float alpha)
        {
            color.r = Mathf.Clamp01(color.r);
            color.g = Mathf.Clamp01(color.g);
            color.b = Mathf.Clamp01(color.b);
            alpha = Mathf.Clamp01(alpha);
            return new Color32((byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), (byte)(alpha * 255f));
        }

        private static float FractalNoise(float x, float y)
        {
            float value = 0f;
            float amplitude = 0.5f;
            float frequency = 1f;
            for (int i = 0; i < 4; i++)
            {
                value += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
                frequency *= 2.03f;
                amplitude *= 0.52f;
            }
            return Mathf.Clamp01(value);
        }

        private static float Noise01(int seed)
        {
            unchecked
            {
                uint x = (uint)seed;
                x ^= x << 13;
                x ^= x >> 17;
                x ^= x << 5;
                return (x & 0x00FFFFFF) / 16777215f;
            }
        }

        private static float Frac(float value)
        {
            return value - Mathf.Floor(value);
        }

        private static string ToPhysicalPath(string assetPath, string packageRoot)
        {
            string prefix = PackageAssetRoot + "/";
            if (!assetPath.StartsWith(prefix, StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"Asset path must be inside {PackageAssetRoot}: {assetPath}");
            }

            string relative = assetPath.Substring(prefix.Length).Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(packageRoot, relative);
        }

        private static string ToMainProjectPath(string packageAssetPath)
        {
            return packageAssetPath.Replace(PackageAssetRoot, "AssetPacks/BrassworksBreach.SteamAtmosphereVfxSet10");
        }

        private static string ToProjectRelative(string physicalPath, string projectRoot)
        {
            string normalizedPhysical = NormalizePath(Path.GetFullPath(physicalPath));
            string normalizedProject = NormalizePath(Path.GetFullPath(projectRoot)).TrimEnd('/');
            if (normalizedPhysical.StartsWith(normalizedProject + "/", StringComparison.OrdinalIgnoreCase))
            {
                return normalizedPhysical.Substring(normalizedProject.Length + 1);
            }

            return normalizedPhysical;
        }

        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }

        private static string JsonEscape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private readonly struct EffectSpec
        {
            public EffectSpec(string key, string displayName, string category, Color tint, float alpha, Vector2 scale, int layers, string intent)
            {
                Key = key;
                DisplayName = displayName;
                Category = category;
                Tint = tint;
                Alpha = alpha;
                Scale = scale;
                Layers = layers;
                Intent = intent;
            }

            public string Key { get; }
            public string DisplayName { get; }
            public string Category { get; }
            public Color Tint { get; }
            public float Alpha { get; }
            public Vector2 Scale { get; }
            public int Layers { get; }
            public string Intent { get; }
        }
    }
}
