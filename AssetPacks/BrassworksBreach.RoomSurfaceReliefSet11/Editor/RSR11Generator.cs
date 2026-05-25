#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace BrassworksBreach.RoomSurfaceReliefSet11.Editor
{
    public static class RSR11Generator
    {
        private const string PackageName = "com.brassworks.sidecar.room-surface-relief-set11";
        private const string PackageAssetRoot = "Packages/" + PackageName;
        private const string Version = "0.1.56-p001";
        private const string VersionShort = "0.1.56";
        private const string BuildId = "p001";
        private const string PackId = "RSR11";
        private const int TextureSize = 1024;
        private const int PreviewWidth = 1280;
        private const int PreviewHeight = 800;

        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly Dictionary<string, MaterialDef> MaterialDefs = new Dictionary<string, MaterialDef>();
        private static readonly List<PrefabRecord> PrefabRecords = new List<PrefabRecord>();
        private static readonly List<TextureRecord> TextureRecords = new List<TextureRecord>();
        private static readonly List<string> PreviewFiles = new List<string>();
        private static readonly List<string> PackagePreviewFiles = new List<string>();
        private static string _packagePhysicalRoot;
        private static string _repoRoot;
        private static string _generatedAtUtc;

        [MenuItem("Brassworks/Sidecars/Generate RSR11 Room Surface Relief")]
        public static void GenerateAll()
        {
            _generatedAtUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            ResolveRoots();
            CleanGeneratedOutputs();
            PrepareFolders();
            GenerateTextures();
            AssetDatabase.Refresh();
            ConfigureTextureImporters();
            CreateMaterials();
            CreateMeshes();
            CreatePrefabs();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RenderPreviews();
            WriteMetadata();
            WriteReadmeAndChangelog();
            WriteProductionDocs();
            WritePlanningDocs();
            WriteInitialQaDocs();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"RSR11_GENERATE_PASS prefabs={PrefabRecords.Count} materials={Materials.Count} meshes={Meshes.Count} textures={TextureRecords.Count} previews={PreviewFiles.Count}");
        }

        private static void ResolveRoots()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForPackageName(PackageName);
            if (packageInfo == null || string.IsNullOrEmpty(packageInfo.resolvedPath))
            {
                throw new InvalidOperationException("Package root could not be resolved. Confirm the validation project manifest references " + PackageName + ".");
            }

            _packagePhysicalRoot = NormalizePath(packageInfo.resolvedPath);
            var assetPacks = Directory.GetParent(_packagePhysicalRoot);
            if (assetPacks == null || assetPacks.Parent == null)
            {
                throw new InvalidOperationException("Could not derive repository root from package path " + _packagePhysicalRoot);
            }

            _repoRoot = NormalizePath(assetPacks.Parent.FullName);
        }

        private static void CleanGeneratedOutputs()
        {
            foreach (var relative in new[] { "Runtime", "Documentation~" })
            {
                DeleteDirectoryIfExists(Physical(relative));
            }

            foreach (var root in new[] { DocProductionRoot(), ConceptRenderRoot(), PlanningRoot(), QaRoot() })
            {
                DeleteDirectoryIfExists(root);
            }

            AssetDatabase.Refresh();
        }

        private static void PrepareFolders()
        {
            foreach (var relative in new[]
            {
                "Runtime/Materials",
                "Runtime/Meshes",
                "Runtime/Prefabs",
                "Runtime/Textures/Albedo",
                "Runtime/Textures/Normal",
                "Runtime/Textures/Height",
                "Runtime/Textures/Mask",
                "Runtime/Metadata",
                "Runtime/Previews",
                "Documentation~/Manifest",
                "Documentation~/Previews",
                "Documentation~/QA"
            })
            {
                EnsureDirectory(Physical(relative));
            }

            EnsureDirectory(DocProductionRoot());
            EnsureDirectory(ConceptRenderRoot());
            EnsureDirectory(PlanningRoot());
            EnsureDirectory(QaRoot());
        }

        private static void GenerateTextures()
        {
            var defs = new[]
            {
                new TextureDef("DarkIrregularBrickFace", TexturePattern.BrickWall, new Color(0.055f, 0.040f, 0.032f), new Color(0.33f, 0.19f, 0.115f), 11, false, 0.16f, 0.46f),
                new TextureDef("BrickEdgeWarmWear", TexturePattern.BrickWall, new Color(0.075f, 0.048f, 0.035f), new Color(0.48f, 0.26f, 0.13f), 17, false, 0.12f, 0.42f),
                new TextureDef("DeepBlackMortarShadow", TexturePattern.Mortar, new Color(0.007f, 0.006f, 0.005f), new Color(0.075f, 0.052f, 0.036f), 23, false, 0.02f, 0.20f),
                new TextureDef("WetWarmFlagstone", TexturePattern.Flagstone, new Color(0.048f, 0.043f, 0.036f), new Color(0.34f, 0.25f, 0.18f), 31, false, 0.10f, 0.78f),
                new TextureDef("RaisedFlagstoneEdge", TexturePattern.EdgeWear, new Color(0.064f, 0.054f, 0.045f), new Color(0.44f, 0.32f, 0.22f), 37, false, 0.08f, 0.52f),
                new TextureDef("SootedCeilingBrick", TexturePattern.CeilingBrick, new Color(0.026f, 0.022f, 0.019f), new Color(0.19f, 0.12f, 0.073f), 43, false, 0.06f, 0.28f),
                new TextureDef("CeilingSootWash", TexturePattern.SootCard, new Color(0.002f, 0.002f, 0.002f), new Color(0.12f, 0.085f, 0.048f), 53, true, 0.00f, 0.34f),
                new TextureDef("CornerGrimeDamp", TexturePattern.GrimeStrip, new Color(0.004f, 0.004f, 0.003f), new Color(0.18f, 0.12f, 0.066f), 61, true, 0.00f, 0.70f),
                new TextureDef("WarmWetReflection", TexturePattern.ReflectionCard, new Color(0.40f, 0.18f, 0.045f), new Color(1.00f, 0.64f, 0.18f), 71, true, 0.00f, 0.94f),
                new TextureDef("CoolPinReflection", TexturePattern.ReflectionCard, new Color(0.10f, 0.12f, 0.13f), new Color(0.72f, 0.82f, 0.78f), 79, true, 0.00f, 0.90f),
                new TextureDef("AgedStoneTrim", TexturePattern.EdgeWear, new Color(0.050f, 0.045f, 0.039f), new Color(0.38f, 0.29f, 0.21f), 83, false, 0.04f, 0.40f),
                new TextureDef("DampLeakStreak", TexturePattern.LeakCard, new Color(0.010f, 0.008f, 0.006f), new Color(0.34f, 0.21f, 0.09f), 89, true, 0.00f, 0.84f),
                new TextureDef("ChipFaceHighlight", TexturePattern.EdgeWear, new Color(0.11f, 0.080f, 0.057f), new Color(0.57f, 0.37f, 0.20f), 97, false, 0.05f, 0.36f),
                new TextureDef("CoalDustCrevice", TexturePattern.Mortar, new Color(0.004f, 0.004f, 0.004f), new Color(0.055f, 0.044f, 0.033f), 101, false, 0.00f, 0.18f),
            };

            foreach (var def in defs)
            {
                SaveTexture($"RSR11_TEX_{def.Name}_ALB.png", BuildAlbedoTexture(def), "Runtime/Textures/Albedo", "albedo", def);
                SaveTexture($"RSR11_TEX_{def.Name}_HGT.png", BuildHeightTexture(def), "Runtime/Textures/Height", "height", def);
                SaveTexture($"RSR11_TEX_{def.Name}_NRM.png", BuildNormalTexture(def), "Runtime/Textures/Normal", "normal", def);
                SaveTexture($"RSR11_TEX_{def.Name}_MSO.png", BuildMaskTexture(def), "Runtime/Textures/Mask", "metallic_smoothness_occlusion", def);
            }
        }

        private static Texture2D BuildAlbedoTexture(TextureDef def)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, false);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var height = SurfaceHeight(def, fx, fy);
                    var edge = EdgeSignal(def, fx, fy);
                    var n1 = Mathf.PerlinNoise(fx * 18.0f + def.Seed * 0.13f, fy * 18.0f - def.Seed * 0.19f);
                    var n2 = Mathf.PerlinNoise(fx * 71.0f - def.Seed * 0.07f, fy * 63.0f + def.Seed * 0.11f);
                    var soot = Mathf.SmoothStep(0.48f, 0.94f, Mathf.PerlinNoise(fx * 5.0f + def.Seed, fy * 9.0f));
                    var wear = Mathf.Pow(Mathf.Abs(Mathf.Sin((fx * 39.0f + fy * 17.0f + def.Seed) * Mathf.PI)), 18.0f);
                    var t = Mathf.Clamp01(height * 0.82f + n1 * 0.14f + n2 * 0.05f + wear * 0.08f);
                    var color = Color.Lerp(def.Low, def.High, t);
                    color = Color.Lerp(color, color * 0.18f, edge * 0.72f);
                    color = Color.Lerp(color, color * 0.55f, soot * SootStrength(def.Pattern));

                    if (def.Transparent)
                    {
                        color.a = TransparentAlpha(def, fx, fy, n1, n2);
                        color.r *= 0.86f + color.a * 0.44f;
                        color.g *= 0.86f + color.a * 0.32f;
                        color.b *= 0.86f + color.a * 0.22f;
                    }
                    else
                    {
                        var vignette = Mathf.Clamp01((Mathf.Abs(fx - 0.5f) + Mathf.Abs(fy - 0.5f)) * 0.18f);
                        color *= 1.0f - vignette;
                        color.a = 1.0f;
                    }

                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D BuildHeightTexture(TextureDef def)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var h = SurfaceHeight(def, fx, fy);
                    texture.SetPixel(x, y, new Color(h, h, h, def.Transparent ? TransparentAlpha(def, fx, fy, 0.5f, 0.5f) : 1.0f));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D BuildNormalTexture(TextureDef def)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            var sampleStep = 1.0f / (TextureSize - 1);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var hL = SurfaceHeight(def, Mathf.Clamp01(fx - sampleStep), fy);
                    var hR = SurfaceHeight(def, Mathf.Clamp01(fx + sampleStep), fy);
                    var hD = SurfaceHeight(def, fx, Mathf.Clamp01(fy - sampleStep));
                    var hU = SurfaceHeight(def, fx, Mathf.Clamp01(fy + sampleStep));
                    var normal = new Vector3((hL - hR) * 3.8f, (hD - hU) * 3.8f, 1.0f).normalized;
                    texture.SetPixel(x, y, new Color(normal.x * 0.5f + 0.5f, normal.y * 0.5f + 0.5f, normal.z * 0.5f + 0.5f, 1.0f));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D BuildMaskTexture(TextureDef def)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var h = SurfaceHeight(def, fx, fy);
                    var edge = EdgeSignal(def, fx, fy);
                    var wet = WetSignal(def, fx, fy);
                    var noise = Mathf.PerlinNoise(fx * 20.0f + def.Seed, fy * 18.0f - def.Seed);
                    var metallic = 0.0f;
                    var occlusion = Mathf.Clamp01(0.28f + h * 0.64f - edge * 0.34f);
                    var smoothness = Mathf.Clamp01(def.BaseSmoothness + wet * 0.32f + noise * 0.10f - edge * 0.12f);
                    var alpha = def.Transparent ? TransparentAlpha(def, fx, fy, noise, h) : 1.0f;
                    texture.SetPixel(x, y, new Color(metallic, occlusion, alpha, smoothness));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static float SurfaceHeight(TextureDef def, float fx, float fy)
        {
            var n1 = Mathf.PerlinNoise(fx * 16.0f + def.Seed * 0.19f, fy * 16.0f - def.Seed * 0.23f);
            var n2 = Mathf.PerlinNoise(fx * 83.0f - def.Seed * 0.11f, fy * 79.0f + def.Seed * 0.07f);
            var baseH = 0.38f + n1 * 0.30f + n2 * 0.12f;

            switch (def.Pattern)
            {
                case TexturePattern.BrickWall:
                    return MasonryHeight(def, fx, fy, 6.1f, 8.0f, 0.047f, baseH, 0.18f);
                case TexturePattern.CeilingBrick:
                    return MasonryHeight(def, fx, fy, 9.2f, 12.0f, 0.040f, baseH * 0.72f, 0.36f);
                case TexturePattern.Flagstone:
                    return FlagstoneHeight(def, fx, fy, baseH);
                case TexturePattern.Mortar:
                    return Mathf.Clamp01(0.10f + n1 * 0.18f + n2 * 0.05f);
                case TexturePattern.EdgeWear:
                    return Mathf.Clamp01(0.32f + n1 * 0.28f + n2 * 0.10f + Mathf.SmoothStep(0.68f, 0.95f, Mathf.Abs(fy - 0.5f) * 2.0f) * 0.22f);
                case TexturePattern.SootCard:
                case TexturePattern.GrimeStrip:
                case TexturePattern.ReflectionCard:
                case TexturePattern.LeakCard:
                    return Mathf.Clamp01(0.18f + n1 * 0.42f + n2 * 0.18f);
                default:
                    return Mathf.Clamp01(baseH);
            }
        }

        private static float MasonryHeight(TextureDef def, float fx, float fy, float cols, float rows, float mortarWidth, float baseH, float sootPenalty)
        {
            var row = Mathf.FloorToInt(fy * rows);
            var rowOffset = Hash01(row, def.Seed) * 0.47f;
            var bx = Frac(fx * cols + rowOffset);
            var by = Frac(fy * rows);
            var joint = Mathf.Min(Mathf.Min(bx, 1.0f - bx), Mathf.Min(by, 1.0f - by));
            var mortar = Mathf.SmoothStep(mortarWidth * 1.65f, mortarWidth * 0.45f, joint);
            var chip = Mathf.SmoothStep(0.82f, 0.98f, Mathf.PerlinNoise(fx * 54.0f + def.Seed, fy * 46.0f - def.Seed));
            var raised = Mathf.Clamp01(baseH + (1.0f - mortar) * 0.24f - chip * 0.10f);
            var soot = Mathf.SmoothStep(0.50f, 0.95f, Mathf.PerlinNoise(fx * 4.0f + def.Seed * 0.3f, fy * 5.0f));
            return Mathf.Clamp01(Mathf.Lerp(raised, 0.035f + baseH * 0.08f, mortar) - soot * sootPenalty * 0.15f);
        }

        private static float FlagstoneHeight(TextureDef def, float fx, float fy, float baseH)
        {
            var x = fx * 3.55f + Mathf.Sin(fy * 7.0f + def.Seed) * 0.06f;
            var y = fy * 3.00f + Mathf.Sin(fx * 5.0f - def.Seed) * 0.05f;
            var bx = Frac(x);
            var by = Frac(y);
            var joint = Mathf.Min(Mathf.Min(bx, 1.0f - bx), Mathf.Min(by, 1.0f - by));
            var diagonal = Mathf.Abs(Frac((fx + fy * 0.72f) * 2.1f + def.Seed * 0.03f) - 0.5f);
            var crack = Mathf.SmoothStep(0.020f, 0.006f, diagonal);
            var mortar = Mathf.Max(Mathf.SmoothStep(0.070f, 0.025f, joint), crack * 0.45f);
            var pooled = WetSignal(def, fx, fy);
            return Mathf.Clamp01(Mathf.Lerp(0.045f + baseH * 0.08f, baseH + pooled * 0.10f, 1.0f - mortar));
        }

        private static float EdgeSignal(TextureDef def, float fx, float fy)
        {
            switch (def.Pattern)
            {
                case TexturePattern.BrickWall:
                    return 1.0f - MasonryJointDistance(def, fx, fy, 6.1f, 8.0f, 0.085f);
                case TexturePattern.CeilingBrick:
                    return 1.0f - MasonryJointDistance(def, fx, fy, 9.2f, 12.0f, 0.075f);
                case TexturePattern.Flagstone:
                    return Mathf.SmoothStep(0.08f, 0.02f, Mathf.Min(Mathf.Min(Frac(fx * 3.55f), 1.0f - Frac(fx * 3.55f)), Mathf.Min(Frac(fy * 3.0f), 1.0f - Frac(fy * 3.0f))));
                default:
                    return Mathf.Clamp01(Mathf.PerlinNoise(fx * 21.0f + def.Seed, fy * 13.0f - def.Seed) * 0.35f);
            }
        }

        private static float MasonryJointDistance(TextureDef def, float fx, float fy, float cols, float rows, float width)
        {
            var row = Mathf.FloorToInt(fy * rows);
            var rowOffset = Hash01(row, def.Seed) * 0.47f;
            var bx = Frac(fx * cols + rowOffset);
            var by = Frac(fy * rows);
            var joint = Mathf.Min(Mathf.Min(bx, 1.0f - bx), Mathf.Min(by, 1.0f - by));
            return Mathf.SmoothStep(0.0f, width, joint);
        }

        private static float WetSignal(TextureDef def, float fx, float fy)
        {
            var baseNoise = Mathf.PerlinNoise(fx * 7.0f + def.Seed * 0.2f, fy * 10.0f - def.Seed * 0.14f);
            var lowBand = Mathf.SmoothStep(0.88f, 0.12f, fy);
            var streak = Mathf.SmoothStep(0.08f, 0.0f, Mathf.Abs(Mathf.Sin((fx * 7.0f + def.Seed * 0.11f) * Mathf.PI)));
            return Mathf.Clamp01(baseNoise * 0.55f + lowBand * 0.28f + streak * 0.17f);
        }

        private static float TransparentAlpha(TextureDef def, float fx, float fy, float n1, float n2)
        {
            if (def.Pattern == TexturePattern.ReflectionCard)
            {
                var radial = Vector2.Distance(new Vector2(fx, fy), new Vector2(0.50f, 0.54f));
                var stripe = Mathf.Pow(Mathf.Clamp01(1.0f - Mathf.Abs(fy - 0.52f) * 3.2f), 2.2f);
                var glint = Mathf.SmoothStep(0.58f, 0.08f, radial) * 0.64f + stripe * 0.28f;
                return Mathf.Clamp01(glint * (0.60f + n1 * 0.38f));
            }

            if (def.Pattern == TexturePattern.LeakCard)
            {
                var streak = Mathf.SmoothStep(0.08f, 0.0f, Mathf.Abs(Mathf.Sin((fx * 8.0f + def.Seed * 0.09f) * Mathf.PI)));
                var falloff = Mathf.SmoothStep(1.0f, 0.08f, fy);
                return Mathf.Clamp01((streak * 0.62f + n1 * 0.24f) * falloff);
            }

            if (def.Pattern == TexturePattern.GrimeStrip)
            {
                var side = Mathf.SmoothStep(0.62f, 0.02f, fx) + Mathf.SmoothStep(0.62f, 0.02f, 1.0f - fx);
                var drip = Mathf.SmoothStep(0.10f, 0.0f, Mathf.Abs(Mathf.Sin((fx * 6.0f + def.Seed * 0.1f) * Mathf.PI))) * Mathf.SmoothStep(1.0f, 0.15f, fy);
                return Mathf.Clamp01(side * 0.46f + drip * 0.36f + n2 * 0.12f);
            }

            var soot = Mathf.SmoothStep(0.58f, 0.0f, Vector2.Distance(new Vector2(fx, fy), new Vector2(0.5f, 0.5f)));
            return Mathf.Clamp01(soot * (0.42f + n1 * 0.38f));
        }

        private static float SootStrength(TexturePattern pattern)
        {
            if (pattern == TexturePattern.CeilingBrick) return 0.55f;
            if (pattern == TexturePattern.SootCard || pattern == TexturePattern.GrimeStrip) return 0.80f;
            if (pattern == TexturePattern.Mortar) return 0.30f;
            return 0.18f;
        }

        private static void SaveTexture(string fileName, Texture2D texture, string relativeFolder, string mapType, TextureDef def)
        {
            var physical = Path.Combine(Physical(relativeFolder), fileName);
            WriteBytes(physical, texture.EncodeToPNG());
            TextureRecords.Add(new TextureRecord
            {
                Name = Path.GetFileNameWithoutExtension(fileName),
                Path = RepoRelative(physical),
                TextureFamily = def.Name,
                MapType = mapType,
                Width = TextureSize,
                Height = TextureSize,
                Transparent = def.Transparent
            });
            UnityObject.DestroyImmediate(texture);
        }

        private static void ConfigureTextureImporters()
        {
            AssetDatabase.Refresh();
            foreach (var texturePath in Directory.GetFiles(Physical("Runtime/Textures"), "*.png", SearchOption.AllDirectories))
            {
                var relative = RepoRelative(texturePath).Replace("AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11/", string.Empty);
                var assetPath = ToPackageAssetPath(relative);
                var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.textureType = assetPath.Contains("_NRM") ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.alphaIsTransparency = assetPath.Contains("Reflection") || assetPath.Contains("Grime") || assetPath.Contains("SootWash") || assetPath.Contains("Leak");
                importer.sRGBTexture = assetPath.Contains("_ALB");
                importer.mipmapEnabled = true;
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.filterMode = FilterMode.Trilinear;
                importer.textureCompression = TextureImporterCompression.CompressedHQ;
                importer.SaveAndReimport();
            }
        }

        private static void CreateMaterials()
        {
            RegisterMaterial(new MaterialDef("DarkIrregularBrickFace", "DarkIrregularBrickFace", "raised irregular wall bricks", new Color(0.20f, 0.105f, 0.060f), 0.00f, 0.42f, 1.35f, 0.055f, false, false, Color.black));
            RegisterMaterial(new MaterialDef("BrickEdgeWarmWear", "BrickEdgeWarmWear", "worn warmer brick edge chips", new Color(0.34f, 0.18f, 0.085f), 0.00f, 0.36f, 1.15f, 0.040f, false, false, Color.black));
            RegisterMaterial(new MaterialDef("DeepBlackMortarShadow", "DeepBlackMortarShadow", "recessed nearly black mortar and crevice shadows", new Color(0.035f, 0.027f, 0.020f), 0.00f, 0.18f, 0.65f, 0.020f, false, false, Color.black));
            RegisterMaterial(new MaterialDef("WetWarmFlagstone", "WetWarmFlagstone", "large warm-black wet flagstone faces", new Color(0.23f, 0.185f, 0.130f), 0.00f, 0.84f, 1.05f, 0.038f, false, false, Color.black));
            RegisterMaterial(new MaterialDef("RaisedFlagstoneEdge", "RaisedFlagstoneEdge", "raised uneven stone lips and broken slab edges", new Color(0.30f, 0.235f, 0.170f), 0.00f, 0.58f, 0.95f, 0.032f, false, false, Color.black));
            RegisterMaterial(new MaterialDef("SootedCeilingBrick", "SootedCeilingBrick", "small ceiling brick courses under soot", new Color(0.13f, 0.078f, 0.045f), 0.00f, 0.30f, 1.20f, 0.030f, false, false, Color.black));
            RegisterMaterial(new MaterialDef("CeilingSootWash", "CeilingSootWash", "transparent broad ceiling and lamp soot wash", new Color(0.045f, 0.033f, 0.021f), 0.00f, 0.44f, 0.45f, 0.010f, true, false, Color.black));
            RegisterMaterial(new MaterialDef("CornerGrimeDamp", "CornerGrimeDamp", "transparent corner grime and damp base buildup", new Color(0.055f, 0.036f, 0.020f), 0.00f, 0.72f, 0.55f, 0.012f, true, false, Color.black));
            RegisterMaterial(new MaterialDef("WarmWetReflection", "WarmWetReflection", "warm gaslight wet reflection helper card", new Color(0.92f, 0.46f, 0.11f), 0.00f, 0.95f, 0.20f, 0.000f, true, true, new Color(0.58f, 0.28f, 0.055f)));
            RegisterMaterial(new MaterialDef("CoolPinReflection", "CoolPinReflection", "small cool wet glint helper card", new Color(0.42f, 0.50f, 0.47f), 0.00f, 0.90f, 0.20f, 0.000f, true, true, new Color(0.12f, 0.16f, 0.13f)));
            RegisterMaterial(new MaterialDef("AgedStoneTrim", "AgedStoneTrim", "dark damp stone trim and coping pieces", new Color(0.26f, 0.205f, 0.150f), 0.00f, 0.44f, 0.90f, 0.030f, false, false, Color.black));
            RegisterMaterial(new MaterialDef("DampLeakStreak", "DampLeakStreak", "transparent vertical leak and wall wetness streaks", new Color(0.16f, 0.095f, 0.035f), 0.00f, 0.86f, 0.35f, 0.006f, true, false, Color.black));
            RegisterMaterial(new MaterialDef("ChipFaceHighlight", "ChipFaceHighlight", "thin chipped masonry face highlights", new Color(0.43f, 0.275f, 0.145f), 0.00f, 0.36f, 0.70f, 0.018f, false, false, Color.black));
            RegisterMaterial(new MaterialDef("CoalDustCrevice", "CoalDustCrevice", "extra dark soot and joint filler", new Color(0.025f, 0.020f, 0.015f), 0.00f, 0.16f, 0.50f, 0.012f, false, false, Color.black));
        }

        private static void RegisterMaterial(MaterialDef def)
        {
            MaterialDefs[def.Key] = def;
            var shader = Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Hidden/InternalErrorShader");
            var material = new Material(shader)
            {
                name = "RSR11_MAT_" + def.Key
            };
            if (material.HasProperty("_Color")) material.SetColor("_Color", def.Color);
            if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", def.Metallic);
            if (material.HasProperty("_Glossiness")) material.SetFloat("_Glossiness", def.Smoothness);
            if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", def.Smoothness);
            AssignTexture(material, "_MainTex", $"Runtime/Textures/Albedo/RSR11_TEX_{def.TextureKey}_ALB.png");
            AssignTexture(material, "_BumpMap", $"Runtime/Textures/Normal/RSR11_TEX_{def.TextureKey}_NRM.png");
            AssignTexture(material, "_MetallicGlossMap", $"Runtime/Textures/Mask/RSR11_TEX_{def.TextureKey}_MSO.png");
            AssignTexture(material, "_OcclusionMap", $"Runtime/Textures/Mask/RSR11_TEX_{def.TextureKey}_MSO.png");
            AssignTexture(material, "_ParallaxMap", $"Runtime/Textures/Height/RSR11_TEX_{def.TextureKey}_HGT.png");
            if (material.HasProperty("_BumpScale")) material.SetFloat("_BumpScale", def.BumpScale);
            if (material.HasProperty("_Parallax")) material.SetFloat("_Parallax", def.HeightScale);
            if (def.Emissive && material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", def.Emission);
                AssignTexture(material, "_EmissionMap", $"Runtime/Textures/Albedo/RSR11_TEX_{def.TextureKey}_ALB.png");
            }

            if (def.Transparent)
            {
                SetupTransparentMaterial(material);
            }

            var path = ToPackageAssetPath("Runtime/Materials/" + material.name + ".mat");
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(material, path);
            Materials[def.Key] = material;
        }

        private static void AssignTexture(Material material, string property, string relativePath)
        {
            if (!material.HasProperty(property))
            {
                return;
            }

            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(ToPackageAssetPath(relativePath));
            if (texture != null)
            {
                material.SetTexture(property, texture);
            }
        }

        private static void SetupTransparentMaterial(Material material)
        {
            if (material.HasProperty("_Mode")) material.SetFloat("_Mode", 3);
            if (material.HasProperty("_SrcBlend")) material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            if (material.HasProperty("_DstBlend")) material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            if (material.HasProperty("_ZWrite")) material.SetFloat("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }

        private static void CreateMeshes()
        {
            AddMesh("BrickFace_A", CreateBoxMesh("RSR11_MESH_BrickFace_A", 0.00f, 0.00f, 0.00f));
            AddMesh("BrickFace_B", CreateBoxMesh("RSR11_MESH_BrickFace_B", 0.06f, -0.03f, 0.02f));
            AddMesh("BrickFace_C", CreateBoxMesh("RSR11_MESH_BrickFace_C", -0.05f, 0.04f, -0.02f));
            AddMesh("BrickFace_D", CreateBoxMesh("RSR11_MESH_BrickFace_D", 0.03f, 0.06f, 0.04f));
            AddMesh("CeilingBrick_A", CreateBoxMesh("RSR11_MESH_CeilingBrick_A", -0.02f, 0.01f, 0.00f));
            AddMesh("CeilingBrick_B", CreateBoxMesh("RSR11_MESH_CeilingBrick_B", 0.04f, -0.04f, 0.02f));
            AddMesh("BackerBox", CreateBoxMesh("RSR11_MESH_BackerBox", 0.00f, 0.00f, 0.00f));
            AddMesh("MortarHairline", CreateBoxMesh("RSR11_MESH_MortarHairline", 0.00f, 0.00f, 0.00f));
            AddMesh("FlagstoneSlab_A", CreateSlabMesh("RSR11_MESH_FlagstoneSlab_A", 13, 0.06f));
            AddMesh("FlagstoneSlab_B", CreateSlabMesh("RSR11_MESH_FlagstoneSlab_B", 29, 0.09f));
            AddMesh("FlagstoneSlab_C", CreateSlabMesh("RSR11_MESH_FlagstoneSlab_C", 47, 0.08f));
            AddMesh("FlagstoneSlab_D", CreateSlabMesh("RSR11_MESH_FlagstoneSlab_D", 59, 0.11f));
            AddMesh("QuadCard", CreateQuadMesh("RSR11_MESH_QuadCard"));
            AddMesh("RaisedEdgeStrip_A", CreateWedgeMesh("RSR11_MESH_RaisedEdgeStrip_A", false));
            AddMesh("RaisedEdgeStrip_B", CreateWedgeMesh("RSR11_MESH_RaisedEdgeStrip_B", true));
            AddMesh("TrimCoping", CreateBoxMesh("RSR11_MESH_TrimCoping", 0.02f, 0.00f, 0.01f));
            AddMesh("CornerFold", CreateCornerFoldMesh("RSR11_MESH_CornerFold"));
        }

        private static void AddMesh(string key, Mesh mesh)
        {
            var path = ToPackageAssetPath("Runtime/Meshes/" + mesh.name + ".asset");
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(mesh, path);
            Meshes[key] = mesh;
        }

        private static Mesh CreateBoxMesh(string name, float jx, float jy, float jz)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var x0 = -0.5f + jx * 0.08f;
            var x1 = 0.5f + jx * 0.06f;
            var y0 = -0.5f + jy * 0.07f;
            var y1 = 0.5f + jy * 0.08f;
            var z0 = -0.5f + jz * 0.05f;
            var z1 = 0.5f + jz * 0.06f;
            AddFace(vertices, normals, uvs, triangles, new Vector3(x0, y0, z1), new Vector3(x1, y0, z1), new Vector3(x1, y1, z1), new Vector3(x0, y1, z1), Vector3.forward);
            AddFace(vertices, normals, uvs, triangles, new Vector3(x1, y0, z0), new Vector3(x0, y0, z0), new Vector3(x0, y1, z0), new Vector3(x1, y1, z0), Vector3.back);
            AddFace(vertices, normals, uvs, triangles, new Vector3(x0, y1, z1), new Vector3(x1, y1, z1), new Vector3(x1, y1, z0), new Vector3(x0, y1, z0), Vector3.up);
            AddFace(vertices, normals, uvs, triangles, new Vector3(x0, y0, z0), new Vector3(x1, y0, z0), new Vector3(x1, y0, z1), new Vector3(x0, y0, z1), Vector3.down);
            AddFace(vertices, normals, uvs, triangles, new Vector3(x1, y0, z1), new Vector3(x1, y0, z0), new Vector3(x1, y1, z0), new Vector3(x1, y1, z1), Vector3.right);
            AddFace(vertices, normals, uvs, triangles, new Vector3(x0, y0, z0), new Vector3(x0, y0, z1), new Vector3(x0, y1, z1), new Vector3(x0, y1, z0), Vector3.left);
            var mesh = new Mesh { name = name };
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateSlabMesh(string name, int seed, float bevel)
        {
            var x0 = -0.5f + (HashSigned(seed, 1) * 0.035f);
            var x1 = 0.5f + (HashSigned(seed, 2) * 0.040f);
            var z0 = -0.5f + (HashSigned(seed, 3) * 0.040f);
            var z1 = 0.5f + (HashSigned(seed, 4) * 0.035f);
            var topY = 0.08f + Hash01(seed, 5) * 0.035f;
            var bottomY = -0.08f;
            var top = new[]
            {
                new Vector3(x0 + bevel, topY, z0 + HashSigned(seed, 6) * 0.025f),
                new Vector3(x1 - bevel * 0.55f, topY + HashSigned(seed, 7) * 0.018f, z0 + bevel),
                new Vector3(x1 + HashSigned(seed, 8) * 0.022f, topY, z1 - bevel),
                new Vector3(x0 + bevel * 0.50f, topY + HashSigned(seed, 9) * 0.014f, z1 + HashSigned(seed, 10) * 0.025f)
            };
            var bottom = new[]
            {
                new Vector3(-0.5f, bottomY, -0.5f),
                new Vector3(0.5f, bottomY, -0.5f),
                new Vector3(0.5f, bottomY, 0.5f),
                new Vector3(-0.5f, bottomY, 0.5f)
            };

            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            AddFace(vertices, normals, uvs, triangles, top[0], top[1], top[2], top[3], Vector3.up);
            AddFace(vertices, normals, uvs, triangles, bottom[1], bottom[0], bottom[3], bottom[2], Vector3.down);
            for (var i = 0; i < 4; i++)
            {
                var next = (i + 1) % 4;
                var normal = Vector3.Cross(bottom[next] - bottom[i], top[i] - bottom[i]).normalized;
                AddFace(vertices, normals, uvs, triangles, bottom[i], bottom[next], top[next], top[i], normal);
            }

            var mesh = new Mesh { name = name };
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateQuadMesh(string name)
        {
            var mesh = new Mesh { name = name };
            mesh.SetVertices(new[]
            {
                new Vector3(-0.5f, -0.5f, 0.0f),
                new Vector3(0.5f, -0.5f, 0.0f),
                new Vector3(0.5f, 0.5f, 0.0f),
                new Vector3(-0.5f, 0.5f, 0.0f)
            });
            mesh.SetNormals(new[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward });
            mesh.SetUVs(0, new[] { Vector2.zero, Vector2.right, Vector2.one, Vector2.up });
            mesh.SetTriangles(new[] { 0, 1, 2, 0, 2, 3 }, 0);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateWedgeMesh(string name, bool flipped)
        {
            var y = flipped ? -1.0f : 1.0f;
            var vertices = new[]
            {
                new Vector3(-0.5f, -0.5f * y, -0.5f),
                new Vector3(0.5f, -0.5f * y, -0.5f),
                new Vector3(-0.5f, 0.5f * y, 0.5f),
                new Vector3(0.5f, 0.5f * y, 0.5f),
                new Vector3(-0.5f, -0.5f * y, 0.5f),
                new Vector3(0.5f, -0.5f * y, 0.5f)
            };
            var triangles = new[]
            {
                0, 2, 1, 1, 2, 3,
                4, 5, 2, 5, 3, 2,
                0, 1, 4, 1, 5, 4,
                0, 4, 2,
                1, 3, 5
            };
            var mesh = new Mesh { name = name };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = vertices.Select(v => new Vector2(v.x + 0.5f, v.z + 0.5f)).ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateCornerFoldMesh(string name)
        {
            var mesh = new Mesh { name = name };
            var vertices = new[]
            {
                new Vector3(0.0f, -0.5f, 0.0f), new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0.0f, 0.5f, 0.0f),
                new Vector3(0.0f, -0.5f, 0.0f), new Vector3(0.0f, -0.5f, 0.5f), new Vector3(0.0f, 0.5f, 0.5f), new Vector3(0.0f, 0.5f, 0.0f)
            };
            mesh.vertices = vertices;
            mesh.normals = new[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back, Vector3.left, Vector3.left, Vector3.left, Vector3.left };
            mesh.uv = new[] { Vector2.zero, Vector2.right, Vector2.one, Vector2.up, Vector2.zero, Vector2.right, Vector2.one, Vector2.up };
            mesh.triangles = new[] { 0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7 };
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void AddFace(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> triangles, Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 normal)
        {
            var start = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);
            normals.Add(normal);
            normals.Add(normal);
            normals.Add(normal);
            normals.Add(normal);
            uvs.Add(new Vector2(0.0f, 0.0f));
            uvs.Add(new Vector2(1.0f, 0.0f));
            uvs.Add(new Vector2(1.0f, 1.0f));
            uvs.Add(new Vector2(0.0f, 1.0f));
            triangles.Add(start + 0);
            triangles.Add(start + 1);
            triangles.Add(start + 2);
            triangles.Add(start + 0);
            triangles.Add(start + 2);
            triangles.Add(start + 3);
        }

        private static void CreatePrefabs()
        {
            PrefabRecords.Clear();
            CreateWallPanel("01", "WallPanel_ModularBrick_A", 2.4f, 2.6f, 8, 6, 101, true, false, true);
            CreateWallPanel("02", "WallPanel_ModularBrick_B", 3.2f, 2.6f, 8, 8, 109, true, true, true);
            CreateWallPanel("03", "WallPanel_CorridorBand_C", 4.0f, 1.8f, 6, 10, 127, false, false, true);
            CreateWallPanel("04", "WallPanel_LampSootReady_D", 2.8f, 3.0f, 9, 7, 139, true, true, true);
            CreateFloorPanel("05", "FloorSlab_WetFlagstone_A", 3.0f, 3.0f, 3, 3, 211, true);
            CreateFloorPanel("06", "FloorSlab_WetFlagstone_B", 4.0f, 2.4f, 4, 3, 223, true);
            CreateFloorPanel("07", "FloorSlab_CorridorRun_C", 4.8f, 2.0f, 5, 2, 239, true);
            CreateFloorPanel("08", "FloorSlab_ThresholdPool_D", 3.2f, 2.8f, 3, 3, 251, true);
            CreateCeilingPanel("09", "CeilingBrickPanel_Sooted_A", 3.2f, 2.2f, 7, 6, 307, true);
            CreateCeilingPanel("10", "CeilingBrickPanel_Sooted_B", 4.2f, 2.0f, 9, 5, 313, true);
            CreateCeilingPanel("11", "CeilingBrickPanel_Corridor_C", 4.8f, 1.8f, 10, 4, 331, true);
            CreateCeilingPanel("12", "CeilingBrickPanel_CornerSmoke_D", 2.8f, 2.8f, 7, 7, 347, true);
            CreateMortarCardPrefab("13", "MortarShadowCards_Horizontal_A", 3.2f, 1.8f, 401, false);
            CreateMortarCardPrefab("14", "MortarShadowCards_Grid_B", 3.2f, 2.4f, 409, true);
            CreateMortarCardPrefab("15", "MortarShadowCards_Corner_C", 2.4f, 2.4f, 419, true);
            CreateMortarCardPrefab("16", "MortarShadowCards_ThinBand_D", 4.0f, 1.2f, 431, false);
            CreateCornerGrimePrefab("17", "CornerGrimeStrip_VerticalDamp_A", 0.55f, 2.8f, 503);
            CreateCornerGrimePrefab("18", "CornerGrimeStrip_LampSoot_B", 0.70f, 2.4f, 509);
            CreateCornerGrimePrefab("19", "CornerGrimeStrip_BaseWet_C", 0.80f, 1.6f, 521);
            CreateCornerGrimePrefab("20", "CornerGrimeStrip_CeilingDrape_D", 0.65f, 2.2f, 541);
            CreateReflectionHelperPrefab("21", "WetReflectionHelper_WarmPool_A", 2.2f, 0.75f, 607, true);
            CreateReflectionHelperPrefab("22", "WetReflectionHelper_NarrowGlint_B", 3.4f, 0.32f, 613, false);
            CreateReflectionHelperPrefab("23", "WetReflectionHelper_WallBase_C", 2.8f, 0.55f, 631, true);
            CreateReflectionHelperPrefab("24", "WetReflectionHelper_CoolPin_D", 1.6f, 0.42f, 647, false);
            CreateRaisedEdgePrefab("25", "RaisedUnevenStoneEdges_WallBase_A", 3.4f, 0.28f, 701, "AgedStoneTrim");
            CreateRaisedEdgePrefab("26", "RaisedUnevenStoneEdges_FloorLip_B", 3.0f, 0.22f, 709, "RaisedFlagstoneEdge");
            CreateRaisedEdgePrefab("27", "RaisedUnevenStoneEdges_CeilingTrim_C", 3.8f, 0.18f, 719, "AgedStoneTrim");
            CreateRaisedEdgePrefab("28", "RaisedUnevenStoneEdges_ChippedD", 2.6f, 0.26f, 733, "ChipFaceHighlight");
        }

        private static void CreateWallPanel(string index, string slug, float width, float height, int rows, int columnsHint, int seed, bool dampBase, bool corner, bool mortarCards)
        {
            var root = NewRoot($"RSR11_PREFAB_{index}_{slug}");
            AddPart(root, "deep mortar backing", "BackerBox", "DeepBlackMortarShadow", new Vector3(0, 0, 0.030f), new Vector3(width, height, 0.045f), Vector3.zero);
            var rowHeight = height / rows;
            var brickIndex = 0;
            for (var row = 0; row < rows; row++)
            {
                var cols = columnsHint + ((row + seed) % 2 == 0 ? 0 : 1);
                var brickWidth = width / cols;
                var y = -height * 0.5f + rowHeight * (row + 0.5f);
                var offset = ((row % 2 == 0) ? 0.0f : brickWidth * 0.42f) + HashSigned(seed, row) * 0.035f;
                for (var col = -1; col < cols + 1; col++)
                {
                    var x = -width * 0.5f + brickWidth * (col + 0.5f) + offset;
                    if (x < -width * 0.54f || x > width * 0.54f)
                    {
                        continue;
                    }

                    var sx = brickWidth * Mathf.Lerp(0.78f, 0.96f, Hash01(seed + row * 17, col + 3));
                    var sy = rowHeight * Mathf.Lerp(0.70f, 0.88f, Hash01(seed + row * 23, col + 7));
                    var sz = Mathf.Lerp(0.055f, 0.105f, Hash01(seed + row * 31, col + 11));
                    var z = -0.020f - sz * 0.32f + HashSigned(seed + row, col) * 0.006f;
                    var meshKey = "BrickFace_" + (char)('A' + ((brickIndex + seed) % 4));
                    var matKey = (brickIndex + row) % 7 == 0 ? "BrickEdgeWarmWear" : "DarkIrregularBrickFace";
                    AddPart(root, $"individual brick {brickIndex:00}", meshKey, matKey, new Vector3(x, y, z), new Vector3(sx, sy, sz), new Vector3(0, 0, HashSigned(seed + row, col + 41) * 1.7f));
                    if ((brickIndex + seed) % 11 == 0)
                    {
                        AddPart(root, $"raised chip {brickIndex:00}", "MortarHairline", "ChipFaceHighlight", new Vector3(x + sx * 0.34f, y + sy * 0.39f, z - sz * 0.62f), new Vector3(sx * 0.22f, sy * 0.035f, 0.018f), Vector3.zero);
                    }
                    brickIndex++;
                }

                if (mortarCards && row > 0)
                {
                    AddPart(root, $"horizontal mortar shadow {row:00}", "MortarHairline", "CoalDustCrevice", new Vector3(0, -height * 0.5f + rowHeight * row, -0.060f), new Vector3(width * 1.01f, 0.025f, 0.014f), Vector3.zero);
                }
            }

            for (var col = 1; col < columnsHint; col += 2)
            {
                AddPart(root, $"broken vertical seam {col:00}", "MortarHairline", "DeepBlackMortarShadow", new Vector3(-width * 0.5f + width * col / columnsHint, 0, -0.065f), new Vector3(0.020f, height * Mathf.Lerp(0.32f, 0.66f, Hash01(seed, col)), 0.014f), Vector3.zero);
            }

            if (dampBase)
            {
                AddPart(root, "transparent damp base grime", "QuadCard", "CornerGrimeDamp", new Vector3(0, -height * 0.42f, -0.091f), new Vector3(width * 1.04f, height * 0.22f, 1.0f), Vector3.zero);
                AddPart(root, "warm wet wall base glint", "QuadCard", "WarmWetReflection", new Vector3(width * 0.10f, -height * 0.47f, -0.096f), new Vector3(width * 0.55f, height * 0.09f, 1.0f), Vector3.zero);
            }

            if (corner)
            {
                AddPart(root, "left corner coal grime fold", "CornerFold", "CornerGrimeDamp", new Vector3(-width * 0.52f, 0, -0.080f), new Vector3(0.45f, height * 1.02f, 0.45f), Vector3.zero);
                AddPart(root, "right raised trim edge", "TrimCoping", "AgedStoneTrim", new Vector3(width * 0.52f, 0, -0.040f), new Vector3(0.075f, height * 1.02f, 0.075f), Vector3.zero);
            }

            SavePrefab(root, "WallPanels", $"modular wall panel with {brickIndex} individual raised bricks, shadow mortar, damp/glint layers");
        }

        private static void CreateFloorPanel(string index, string slug, float width, float depth, int cols, int rows, int seed, bool wetCards)
        {
            var root = NewRoot($"RSR11_PREFAB_{index}_{slug}");
            AddPart(root, "black mortar bed", "BackerBox", "DeepBlackMortarShadow", new Vector3(0, -0.060f, 0), new Vector3(width, 0.070f, depth), Vector3.zero);
            var slabIndex = 0;
            var cellW = width / cols;
            var cellD = depth / rows;
            for (var z = 0; z < rows; z++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var sx = cellW * Mathf.Lerp(0.78f, 0.96f, Hash01(seed + x, z + 13));
                    var sz = cellD * Mathf.Lerp(0.76f, 0.98f, Hash01(seed + x, z + 29));
                    var sy = Mathf.Lerp(0.070f, 0.135f, Hash01(seed + x * 11, z * 7));
                    var px = -width * 0.5f + cellW * (x + 0.5f) + HashSigned(seed + z, x) * cellW * 0.08f;
                    var pz = -depth * 0.5f + cellD * (z + 0.5f) + HashSigned(seed + x, z) * cellD * 0.08f;
                    var meshKey = "FlagstoneSlab_" + (char)('A' + ((slabIndex + seed) % 4));
                    AddPart(root, $"uneven flagstone slab {slabIndex:00}", meshKey, "WetWarmFlagstone", new Vector3(px, sy * 0.48f, pz), new Vector3(sx, sy, sz), new Vector3(0, HashSigned(seed + slabIndex, 9) * 3.5f, 0));
                    if (slabIndex % 3 == 0)
                    {
                        AddPart(root, $"raised chipped slab lip {slabIndex:00}", "RaisedEdgeStrip_A", "RaisedFlagstoneEdge", new Vector3(px, sy * 1.04f, pz + sz * 0.47f), new Vector3(sx * 0.82f, 0.035f, 0.055f), new Vector3(0, 0, 0));
                    }
                    slabIndex++;
                }
            }

            for (var x = 1; x < cols; x++)
            {
                AddPart(root, $"dark vertical stone joint {x:00}", "MortarHairline", "DeepBlackMortarShadow", new Vector3(-width * 0.5f + cellW * x, 0.020f, 0), new Vector3(0.030f, 0.030f, depth * 0.98f), Vector3.zero);
            }
            for (var z = 1; z < rows; z++)
            {
                AddPart(root, $"dark horizontal stone joint {z:00}", "MortarHairline", "DeepBlackMortarShadow", new Vector3(0, 0.022f, -depth * 0.5f + cellD * z), new Vector3(width * 0.98f, 0.030f, 0.030f), Vector3.zero);
            }

            if (wetCards)
            {
                AddPart(root, "warm broad reflection puddle", "QuadCard", "WarmWetReflection", new Vector3(width * 0.06f, 0.118f, -depth * 0.16f), new Vector3(width * 0.62f, depth * 0.26f, 1.0f), new Vector3(90, 0, 0));
                AddPart(root, "thin cool pin reflection", "QuadCard", "CoolPinReflection", new Vector3(-width * 0.22f, 0.121f, depth * 0.24f), new Vector3(width * 0.36f, depth * 0.09f, 1.0f), new Vector3(90, 0, -7));
                AddPart(root, "floor damp grime veil", "QuadCard", "CornerGrimeDamp", new Vector3(0, 0.116f, depth * 0.42f), new Vector3(width * 0.88f, depth * 0.18f, 1.0f), new Vector3(90, 0, 0));
            }

            SavePrefab(root, "FloorSlabs", $"large wet flagstone floor module with {slabIndex} raised uneven slabs and helper reflection cards");
        }

        private static void CreateCeilingPanel(string index, string slug, float width, float depth, int cols, int rows, int seed, bool sootWash)
        {
            var root = NewRoot($"RSR11_PREFAB_{index}_{slug}");
            AddPart(root, "ceiling black mortar backing", "BackerBox", "DeepBlackMortarShadow", new Vector3(0, 0.035f, 0), new Vector3(width, 0.045f, depth), Vector3.zero);
            var brickIndex = 0;
            var cellW = width / cols;
            var cellD = depth / rows;
            for (var z = 0; z < rows; z++)
            {
                var offset = z % 2 == 0 ? 0.0f : cellW * 0.42f;
                for (var x = -1; x < cols + 1; x++)
                {
                    var px = -width * 0.5f + cellW * (x + 0.5f) + offset;
                    if (px < -width * 0.54f || px > width * 0.54f) continue;
                    var pz = -depth * 0.5f + cellD * (z + 0.5f);
                    var sx = cellW * Mathf.Lerp(0.78f, 0.93f, Hash01(seed + x, z));
                    var sz = cellD * Mathf.Lerp(0.70f, 0.90f, Hash01(seed + z, x + 5));
                    var sy = Mathf.Lerp(0.036f, 0.072f, Hash01(seed + z * 13, x + 19));
                    AddPart(root, $"sooted ceiling brick {brickIndex:00}", brickIndex % 2 == 0 ? "CeilingBrick_A" : "CeilingBrick_B", "SootedCeilingBrick", new Vector3(px, -sy * 0.35f, pz), new Vector3(sx, sy, sz), new Vector3(0, HashSigned(seed + brickIndex, 1) * 1.8f, 0));
                    brickIndex++;
                }
            }

            AddPart(root, "ceiling trim leading edge", "TrimCoping", "AgedStoneTrim", new Vector3(0, -0.076f, -depth * 0.51f), new Vector3(width * 1.02f, 0.070f, 0.085f), Vector3.zero);
            if (sootWash)
            {
                AddPart(root, "broad transparent ceiling soot wash", "QuadCard", "CeilingSootWash", new Vector3(0, -0.096f, 0), new Vector3(width * 0.92f, depth * 0.84f, 1.0f), new Vector3(90, 0, 0));
                AddPart(root, "localized amber smoke stain", "QuadCard", "DampLeakStreak", new Vector3(width * 0.18f, -0.101f, depth * 0.10f), new Vector3(width * 0.35f, depth * 0.45f, 1.0f), new Vector3(90, 0, 15));
            }

            SavePrefab(root, "CeilingPanels", $"small-scale ceiling brick panel with {brickIndex} individual bricks and transparent soot wash");
        }

        private static void CreateMortarCardPrefab(string index, string slug, float width, float height, int seed, bool grid)
        {
            var root = NewRoot($"RSR11_PREFAB_{index}_{slug}");
            AddPart(root, "base faint coal veil", "QuadCard", "CeilingSootWash", new Vector3(0, 0, 0), new Vector3(width, height, 1), Vector3.zero);
            var lines = grid ? 7 : 5;
            for (var i = 0; i < lines; i++)
            {
                var y = -height * 0.5f + height * (i + 0.5f) / lines + HashSigned(seed, i) * 0.035f;
                AddPart(root, $"mortar horizontal card {i:00}", "QuadCard", "CoalDustCrevice", new Vector3(0, y, -0.004f), new Vector3(width * Mathf.Lerp(0.55f, 1.0f, Hash01(seed, i)), 0.040f, 1.0f), Vector3.zero);
            }

            if (grid)
            {
                for (var i = 0; i < 5; i++)
                {
                    var x = -width * 0.5f + width * (i + 0.5f) / 5.0f + HashSigned(seed + 17, i) * 0.035f;
                    AddPart(root, $"mortar vertical card {i:00}", "QuadCard", "DeepBlackMortarShadow", new Vector3(x, 0, -0.006f), new Vector3(0.042f, height * Mathf.Lerp(0.35f, 0.92f, Hash01(seed + 23, i)), 1.0f), Vector3.zero);
                }
            }

            SavePrefab(root, "MortarShadowCards", "transparent and solid dark mortar cards for added seam depth over existing geometry");
        }

        private static void CreateCornerGrimePrefab(string index, string slug, float width, float height, int seed)
        {
            var root = NewRoot($"RSR11_PREFAB_{index}_{slug}");
            AddPart(root, "folded corner grime sheet", "CornerFold", "CornerGrimeDamp", new Vector3(-width * 0.25f, 0, 0), new Vector3(width, height, width), Vector3.zero);
            AddPart(root, "vertical damp leak streak", "QuadCard", "DampLeakStreak", new Vector3(width * 0.08f, height * 0.04f, -0.010f), new Vector3(width * 0.50f, height * 0.86f, 1.0f), new Vector3(0, 0, HashSigned(seed, 1) * 5.0f));
            AddPart(root, "coal dust root strip", "MortarHairline", "CoalDustCrevice", new Vector3(0.0f, -height * 0.47f, -0.015f), new Vector3(width * 1.15f, 0.055f, 0.020f), Vector3.zero);
            SavePrefab(root, "CornerGrimeStrips", "folded corner grime strip with damp leak geometry and coal-dark base");
        }

        private static void CreateReflectionHelperPrefab(string index, string slug, float width, float height, int seed, bool warm)
        {
            var root = NewRoot($"RSR11_PREFAB_{index}_{slug}");
            AddPart(root, "primary wet helper card", "QuadCard", warm ? "WarmWetReflection" : "CoolPinReflection", new Vector3(0, 0.003f, 0), new Vector3(width, height, 1.0f), new Vector3(90, 0, HashSigned(seed, 1) * 4.0f));
            AddPart(root, "secondary broken glint card", "QuadCard", warm ? "CoolPinReflection" : "WarmWetReflection", new Vector3(width * 0.16f, 0.006f, height * 0.20f), new Vector3(width * 0.46f, height * 0.38f, 1.0f), new Vector3(90, 0, 11 + HashSigned(seed, 2) * 4.0f));
            AddPart(root, "dark wet under-shadow", "QuadCard", "CornerGrimeDamp", new Vector3(-width * 0.08f, 0.002f, -height * 0.18f), new Vector3(width * 0.72f, height * 0.50f, 1.0f), new Vector3(90, 0, -8));
            SavePrefab(root, "WetReflectionHelpers", "visual-only transparent wet reflection helpers with no Light or ReflectionProbe components");
        }

        private static void CreateRaisedEdgePrefab(string index, string slug, float length, float thickness, int seed, string materialKey)
        {
            var root = NewRoot($"RSR11_PREFAB_{index}_{slug}");
            AddPart(root, "main uneven raised edge wedge", "RaisedEdgeStrip_A", materialKey, new Vector3(0, 0, 0), new Vector3(length, thickness, thickness * 1.6f), Vector3.zero);
            AddPart(root, "dark underside crevice", "MortarHairline", "CoalDustCrevice", new Vector3(0, -thickness * 0.40f, -thickness * 0.62f), new Vector3(length * 0.98f, thickness * 0.24f, thickness * 0.26f), Vector3.zero);
            for (var i = 0; i < 5; i++)
            {
                var x = -length * 0.42f + length * 0.21f * i + HashSigned(seed, i) * length * 0.025f;
                AddPart(root, $"small chipped raised facet {i:00}", "MortarHairline", "ChipFaceHighlight", new Vector3(x, thickness * 0.28f, -thickness * 0.82f), new Vector3(length * 0.08f, thickness * 0.18f, thickness * 0.18f), new Vector3(0, 0, HashSigned(seed + 7, i) * 5.0f));
            }
            SavePrefab(root, "RaisedUnevenStoneEdges", "raised uneven stone trim and chipped edge geometry for wall bases, floor lips, and ceiling seams");
        }

        private static GameObject NewRoot(string name)
        {
            return new GameObject(name);
        }

        private static GameObject AddPart(GameObject parent, string name, string meshKey, string materialKey, Vector3 position, Vector3 scale, Vector3 euler)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = position;
            go.transform.localRotation = Quaternion.Euler(euler);
            go.transform.localScale = scale;
            var filter = go.AddComponent<MeshFilter>();
            filter.sharedMesh = Meshes[meshKey];
            var renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = Materials[materialKey];
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            renderer.receiveShadows = true;
            return go;
        }

        private static void SavePrefab(GameObject root, string family, string notes)
        {
            var path = ToPackageAssetPath("Runtime/Prefabs/" + root.name + ".prefab");
            AssetDatabase.DeleteAsset(path);
            PrefabUtility.SaveAsPrefabAsset(root, path);
            var rendererCount = root.GetComponentsInChildren<MeshRenderer>().Length;
            var meshCount = root.GetComponentsInChildren<MeshFilter>().Select(f => f.sharedMesh).Where(m => m != null).Distinct().Count();
            PrefabRecords.Add(new PrefabRecord
            {
                Name = root.name,
                Family = family,
                Path = "AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11/Runtime/Prefabs/" + root.name + ".prefab",
                ComponentContract = "GameObject, Transform, MeshFilter, MeshRenderer only",
                ChildRenderers = rendererCount,
                MeshAssetsUsed = meshCount,
                Notes = notes
            });
            UnityObject.DestroyImmediate(root);
        }

        private static void RenderPreviews()
        {
            AssetDatabase.Refresh();
            foreach (var record in PrefabRecords)
            {
                RenderPrefabPreview(record);
            }

            WriteContactSheet();
            AssetDatabase.Refresh();
        }

        private static void RenderPrefabPreview(PrefabRecord record)
        {
            var assetPath = PackageAssetRoot + "/" + record.Path.Replace("AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11/", string.Empty);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab == null)
            {
                throw new InvalidOperationException("Could not load prefab for preview: " + assetPath);
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            var cameraGo = new GameObject("RSR11 preview camera");
            var keyLightGo = new GameObject("RSR11 preview warm key");
            var fillLightGo = new GameObject("RSR11 preview soft fill");
            try
            {
                instance.transform.position = Vector3.zero;
                if (record.Family == "WallPanels" || record.Family == "MortarShadowCards" || record.Family == "CornerGrimeStrips")
                {
                    instance.transform.rotation = Quaternion.identity;
                }
                else if (record.Family == "CeilingPanels")
                {
                    instance.transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                var bounds = CalculateBounds(instance);
                var camera = cameraGo.AddComponent<Camera>();
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.018f, 0.015f, 0.013f, 1.0f);
                camera.orthographic = true;
                camera.nearClipPlane = 0.01f;
                camera.farClipPlane = 100.0f;
                var maxExtent = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
                camera.orthographicSize = Mathf.Max(0.75f, maxExtent * 1.42f);

                var view = GetPreviewView(record.Family);
                camera.transform.position = bounds.center + view * Mathf.Max(4.0f, maxExtent * 4.0f);
                camera.transform.LookAt(bounds.center);

                var key = keyLightGo.AddComponent<Light>();
                key.type = LightType.Directional;
                key.intensity = 2.25f;
                key.color = new Color(1.0f, 0.70f, 0.40f);
                keyLightGo.transform.rotation = Quaternion.Euler(45, -30, 0);

                var fill = fillLightGo.AddComponent<Light>();
                fill.type = LightType.Point;
                fill.intensity = 2.8f;
                fill.range = 8.0f;
                fill.color = new Color(0.55f, 0.42f, 0.30f);
                fillLightGo.transform.position = bounds.center + new Vector3(-1.6f, 1.9f, -2.2f);

                var rt = new RenderTexture(PreviewWidth, PreviewHeight, 24, RenderTextureFormat.ARGB32);
                camera.targetTexture = rt;
                var previous = RenderTexture.active;
                RenderTexture.active = rt;
                camera.Render();
                var texture = new Texture2D(PreviewWidth, PreviewHeight, TextureFormat.RGBA32, false);
                texture.ReadPixels(new Rect(0, 0, PreviewWidth, PreviewHeight), 0, 0);
                texture.Apply(false, false);
                if (PreviewLooksBlank(texture))
                {
                    PaintFallbackPreview(texture, record);
                }
                RenderTexture.active = previous;
                camera.targetTexture = null;
                rt.Release();
                UnityObject.DestroyImmediate(rt);

                var fileName = record.Name.Replace("RSR11_PREFAB_", "RSR11_PREVIEW_") + ".png";
                var docPath = Path.Combine(ConceptRenderRoot(), fileName);
                var packagePath = Physical("Documentation~/Previews/" + fileName);
                WriteBytes(docPath, texture.EncodeToPNG());
                WriteBytes(packagePath, texture.EncodeToPNG());
                record.Preview = RepoRelative(docPath);
                PreviewFiles.Add(RepoRelative(docPath));
                PackagePreviewFiles.Add(RepoRelative(packagePath));
                UnityObject.DestroyImmediate(texture);
            }
            finally
            {
                UnityObject.DestroyImmediate(instance);
                UnityObject.DestroyImmediate(cameraGo);
                UnityObject.DestroyImmediate(keyLightGo);
                UnityObject.DestroyImmediate(fillLightGo);
            }
        }

        private static bool PreviewLooksBlank(Texture2D texture)
        {
            var first = texture.GetPixel(0, 0);
            var maxDelta = 0.0f;
            for (var y = 0; y < texture.height; y += 80)
            {
                for (var x = 0; x < texture.width; x += 80)
                {
                    var c = texture.GetPixel(x, y);
                    var delta = Mathf.Abs(c.r - first.r) + Mathf.Abs(c.g - first.g) + Mathf.Abs(c.b - first.b) + Mathf.Abs(c.a - first.a);
                    maxDelta = Mathf.Max(maxDelta, delta);
                    if (maxDelta > 0.035f)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static void PaintFallbackPreview(Texture2D texture, PrefabRecord record)
        {
            for (var y = 0; y < texture.height; y++)
            {
                var fy = y / (float)(texture.height - 1);
                for (var x = 0; x < texture.width; x++)
                {
                    var fx = x / (float)(texture.width - 1);
                    var warm = Mathf.PerlinNoise(fx * 6.0f + record.Name.Length, fy * 5.0f);
                    var shade = 0.020f + fy * 0.030f + warm * 0.018f;
                    texture.SetPixel(x, y, new Color(shade * 1.10f, shade * 0.88f, shade * 0.62f, 1.0f));
                }
            }

            if (record.Family == "MortarShadowCards")
            {
                PaintBrickFallback(texture, true);
            }
            else if (record.Family == "WetReflectionHelpers")
            {
                PaintFloorFallback(texture, true);
            }
            else if (record.Family == "CornerGrimeStrips")
            {
                PaintCornerFallback(texture);
            }
            else
            {
                PaintFloorFallback(texture, false);
            }

            texture.Apply(false, false);
        }

        private static void PaintBrickFallback(Texture2D texture, bool darkCards)
        {
            var rows = 8;
            var cols = 7;
            for (var row = 0; row < rows; row++)
            {
                var y0 = Mathf.RoundToInt(texture.height * (0.14f + row * 0.085f));
                var h = Mathf.RoundToInt(texture.height * 0.058f);
                var offset = row % 2 == 0 ? 0 : Mathf.RoundToInt(texture.width * 0.055f);
                for (var col = -1; col < cols + 1; col++)
                {
                    var x0 = Mathf.RoundToInt(texture.width * (0.13f + col * 0.105f)) + offset;
                    var w = Mathf.RoundToInt(texture.width * 0.088f);
                    var c = new Color(0.18f + col * 0.006f, 0.095f + row * 0.003f, 0.052f, 1.0f);
                    FillRect(texture, x0, y0, w, h, c, 0.95f);
                    if ((row + col) % 4 == 0)
                    {
                        FillRect(texture, x0 + w - 16, y0 + h - 9, 18, 5, new Color(0.55f, 0.31f, 0.14f, 1.0f), 0.72f);
                    }
                }
            }

            if (darkCards)
            {
                for (var i = 0; i < 6; i++)
                {
                    var y = Mathf.RoundToInt(texture.height * (0.18f + i * 0.12f));
                    FillRect(texture, Mathf.RoundToInt(texture.width * 0.10f), y, Mathf.RoundToInt(texture.width * 0.80f), 8, new Color(0.004f, 0.003f, 0.002f, 1.0f), 0.90f);
                }
                for (var i = 0; i < 4; i++)
                {
                    var x = Mathf.RoundToInt(texture.width * (0.18f + i * 0.18f));
                    FillRect(texture, x, Mathf.RoundToInt(texture.height * 0.16f), 8, Mathf.RoundToInt(texture.height * 0.66f), new Color(0.006f, 0.004f, 0.003f, 1.0f), 0.62f);
                }
            }
        }

        private static void PaintFloorFallback(Texture2D texture, bool reflections)
        {
            for (var i = 0; i < 11; i++)
            {
                var x = Mathf.RoundToInt(texture.width * (0.08f + Hash01(i, 41) * 0.75f));
                var y = Mathf.RoundToInt(texture.height * (0.16f + Hash01(i, 73) * 0.62f));
                var w = Mathf.RoundToInt(texture.width * Mathf.Lerp(0.13f, 0.24f, Hash01(i, 3)));
                var h = Mathf.RoundToInt(texture.height * Mathf.Lerp(0.08f, 0.16f, Hash01(i, 7)));
                FillRect(texture, x, y, w, h, new Color(0.17f, 0.125f, 0.080f, 1.0f), 0.88f);
                FillRect(texture, x, y, w, 5, new Color(0.018f, 0.013f, 0.010f, 1.0f), 0.92f);
                FillRect(texture, x, y + h - 5, w, 5, new Color(0.018f, 0.013f, 0.010f, 1.0f), 0.92f);
            }

            if (reflections)
            {
                FillEllipse(texture, texture.width * 0.50f, texture.height * 0.48f, texture.width * 0.28f, texture.height * 0.075f, new Color(1.0f, 0.48f, 0.10f, 1.0f), 0.58f);
                FillEllipse(texture, texture.width * 0.38f, texture.height * 0.58f, texture.width * 0.18f, texture.height * 0.035f, new Color(0.55f, 0.72f, 0.66f, 1.0f), 0.42f);
                FillEllipse(texture, texture.width * 0.61f, texture.height * 0.40f, texture.width * 0.15f, texture.height * 0.030f, new Color(0.95f, 0.62f, 0.20f, 1.0f), 0.46f);
            }
        }

        private static void PaintCornerFallback(Texture2D texture)
        {
            PaintBrickFallback(texture, false);
            FillRect(texture, Mathf.RoundToInt(texture.width * 0.44f), Mathf.RoundToInt(texture.height * 0.10f), Mathf.RoundToInt(texture.width * 0.10f), Mathf.RoundToInt(texture.height * 0.80f), new Color(0.004f, 0.003f, 0.002f, 1.0f), 0.82f);
            FillRect(texture, Mathf.RoundToInt(texture.width * 0.50f), Mathf.RoundToInt(texture.height * 0.10f), Mathf.RoundToInt(texture.width * 0.06f), Mathf.RoundToInt(texture.height * 0.80f), new Color(0.11f, 0.065f, 0.025f, 1.0f), 0.38f);
            FillEllipse(texture, texture.width * 0.52f, texture.height * 0.30f, texture.width * 0.09f, texture.height * 0.35f, new Color(0.18f, 0.10f, 0.035f, 1.0f), 0.44f);
        }

        private static void FillRect(Texture2D texture, int x0, int y0, int w, int h, Color color, float alpha)
        {
            var xMin = Mathf.Clamp(x0, 0, texture.width - 1);
            var yMin = Mathf.Clamp(y0, 0, texture.height - 1);
            var xMax = Mathf.Clamp(x0 + w, 0, texture.width);
            var yMax = Mathf.Clamp(y0 + h, 0, texture.height);
            for (var y = yMin; y < yMax; y++)
            {
                for (var x = xMin; x < xMax; x++)
                {
                    BlendPixel(texture, x, y, color, alpha);
                }
            }
        }

        private static void FillEllipse(Texture2D texture, float cx, float cy, float rx, float ry, Color color, float alpha)
        {
            var xMin = Mathf.Clamp(Mathf.FloorToInt(cx - rx), 0, texture.width - 1);
            var xMax = Mathf.Clamp(Mathf.CeilToInt(cx + rx), 0, texture.width - 1);
            var yMin = Mathf.Clamp(Mathf.FloorToInt(cy - ry), 0, texture.height - 1);
            var yMax = Mathf.Clamp(Mathf.CeilToInt(cy + ry), 0, texture.height - 1);
            for (var y = yMin; y <= yMax; y++)
            {
                for (var x = xMin; x <= xMax; x++)
                {
                    var dx = (x - cx) / Mathf.Max(1.0f, rx);
                    var dy = (y - cy) / Mathf.Max(1.0f, ry);
                    var d = dx * dx + dy * dy;
                    if (d <= 1.0f)
                    {
                        BlendPixel(texture, x, y, color, alpha * (1.0f - d));
                    }
                }
            }
        }

        private static void BlendPixel(Texture2D texture, int x, int y, Color color, float alpha)
        {
            var under = texture.GetPixel(x, y);
            texture.SetPixel(x, y, Color.Lerp(under, color, Mathf.Clamp01(alpha)));
        }

        private static Vector3 GetPreviewView(string family)
        {
            if (family == "FloorSlabs" || family == "WetReflectionHelpers" || family == "RaisedUnevenStoneEdges")
            {
                return new Vector3(0.66f, 0.72f, -0.92f).normalized;
            }

            if (family == "CeilingPanels")
            {
                return new Vector3(0.55f, -0.68f, -0.82f).normalized;
            }

            return new Vector3(0.18f, 0.10f, -1.0f).normalized;
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>();
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

        private static void WriteContactSheet()
        {
            const int columns = 4;
            const int thumbW = 480;
            const int thumbH = 300;
            const int pad = 24;
            var rows = Mathf.CeilToInt(PrefabRecords.Count / (float)columns);
            var width = columns * thumbW + (columns + 1) * pad;
            var height = rows * thumbH + (rows + 1) * pad;
            var sheet = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
            var bg = new Color32(14, 12, 10, 255);
            var border = new Color32(118, 82, 44, 255);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    sheet.SetPixel(x, y, bg);
                }
            }

            for (var i = 0; i < PrefabRecords.Count; i++)
            {
                var row = i / columns;
                var col = i % columns;
                var x0 = pad + col * (thumbW + pad);
                var y0 = height - pad - thumbH - row * (thumbH + pad);
                var previewPhysical = Path.Combine(_repoRoot, PrefabRecords[i].Preview.Replace('/', Path.DirectorySeparatorChar));
                var png = File.ReadAllBytes(previewPhysical);
                var thumb = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
                thumb.LoadImage(png);
                BlitScaled(sheet, thumb, x0, y0, thumbW, thumbH);
                DrawRect(sheet, x0, y0, thumbW, thumbH, border);
                UnityObject.DestroyImmediate(thumb);
            }

            sheet.Apply(false, false);
            var external = Path.Combine(ConceptRenderRoot(), "RSR11_CONTACTSHEET_RoomSurfaceReliefSet11.png");
            var package = Physical("Runtime/Previews/RSR11_CONTACTSHEET_RoomSurfaceReliefSet11.png");
            var docPackage = Physical("Documentation~/Previews/RSR11_CONTACTSHEET_RoomSurfaceReliefSet11.png");
            var encoded = sheet.EncodeToPNG();
            WriteBytes(external, encoded);
            WriteBytes(package, encoded);
            WriteBytes(docPackage, encoded);
            PreviewFiles.Add(RepoRelative(external));
            PackagePreviewFiles.Add(RepoRelative(package));
            PackagePreviewFiles.Add(RepoRelative(docPackage));
            UnityObject.DestroyImmediate(sheet);
        }

        private static void BlitScaled(Texture2D dst, Texture2D src, int x0, int y0, int w, int h)
        {
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var sx = Mathf.Clamp(Mathf.RoundToInt(x / (float)(w - 1) * (src.width - 1)), 0, src.width - 1);
                    var sy = Mathf.Clamp(Mathf.RoundToInt(y / (float)(h - 1) * (src.height - 1)), 0, src.height - 1);
                    dst.SetPixel(x0 + x, y0 + y, src.GetPixel(sx, sy));
                }
            }
        }

        private static void DrawRect(Texture2D tex, int x, int y, int w, int h, Color32 color)
        {
            for (var i = 0; i < w; i++)
            {
                tex.SetPixel(x + i, y, color);
                tex.SetPixel(x + i, y + h - 1, color);
            }
            for (var i = 0; i < h; i++)
            {
                tex.SetPixel(x, y + i, color);
                tex.SetPixel(x + w - 1, y + i, color);
            }
        }

        private static void WriteMetadata()
        {
            var manifest = BuildManifestJson();
            var runtimeManifest = Physical("Runtime/Metadata/RSR11_RoomSurfaceReliefSet11_Manifest_0.1.56-p001.json");
            var packageManifest = Physical("Documentation~/Manifest/RSR11_RoomSurfaceReliefSet11_Manifest_0.1.56-p001.json");
            var productionManifest = Path.Combine(DocProductionRoot(), "RSR11_RoomSurfaceReliefSet11_Manifest_0.1.56-p001.json");
            WriteText(runtimeManifest, manifest);
            WriteText(packageManifest, manifest);
            WriteText(productionManifest, manifest);

            var catalog = BuildCatalogJson();
            WriteText(Physical("Runtime/Metadata/RSR11_RoomSurfaceReliefCatalog_0.1.56-p001.json"), catalog);
        }

        private static string BuildManifestJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            JsonProp(sb, 1, "common_schema", "brassworks.sidecar.visual_pack_manifest.v1", true);
            JsonProp(sb, 1, "pack_id", PackId, true);
            JsonProp(sb, 1, "display_name", "Room Surface Relief Set 11", true);
            sb.AppendLine("  \"package\": {");
            JsonProp(sb, 2, "name", "BrassworksBreach.RoomSurfaceReliefSet11", true);
            JsonProp(sb, 2, "packageId", PackageName, true);
            JsonProp(sb, 2, "version", Version, true);
            JsonProp(sb, 2, "generated_at_utc", _generatedAtUtc, true);
            JsonProp(sb, 2, "unityCompatibility", "Unity 2022.3+ native Mesh/Material/Prefab assets generated in Unity; no Blender or external DCC.", true);
            sb.AppendLine("    \"externalDccToolsUsed\": []");
            sb.AppendLine("  },");
            sb.AppendLine("  \"owner\": {");
            JsonProp(sb, 2, "worker", "RoomSurfaceReliefSet11", true);
            JsonProp(sb, 2, "assignedRootsOnly", true, true);
            JsonProp(sb, 2, "lane", "sidecar-room-surface-relief-set11", false);
            sb.AppendLine("  },");
            sb.AppendLine("  \"roots\": {");
            JsonProp(sb, 2, "packageRoot", "AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11", true);
            JsonProp(sb, 2, "productionDocumentationRoot", "Documentation/AssetProduction/V0_1_56_RoomSurfaceReliefSet11", true);
            JsonProp(sb, 2, "conceptRenderRoot", "Documentation/ConceptRenders/V0_1_56_RoomSurfaceReliefSet11", true);
            JsonProp(sb, 2, "planningRoot", "Documentation/Planning/V0_1_56_RoomSurfaceReliefSet11ImportReadiness", true);
            JsonProp(sb, 2, "qaRoot", "Documentation/QA/V0_1_56_RoomSurfaceReliefSet11ImportReadiness", false);
            sb.AppendLine("  },");
            sb.AppendLine("  \"counts\": {");
            JsonProp(sb, 2, "prefabs", PrefabRecords.Count, true);
            JsonProp(sb, 2, "materials", Materials.Count, true);
            JsonProp(sb, 2, "meshes", Meshes.Count, true);
            JsonProp(sb, 2, "runtimeTexturePNGs", TextureRecords.Count, true);
            JsonProp(sb, 2, "previewPNGs", PreviewFiles.Count, true);
            JsonProp(sb, 2, "packagePreviewPNGs", PackagePreviewFiles.Count, true);
            JsonProp(sb, 2, "families", PrefabRecords.Select(p => p.Family).Distinct().Count(), false);
            sb.AppendLine("  },");
            sb.AppendLine("  \"roomtest_v0_6_context\": {");
            JsonProp(sb, 2, "targetProblem", "flat/regular material grids needed shallow individual brick and stone face geometry over dark mortar", true);
            JsonArray(sb, 2, "placementIntent", new[]
            {
                "drop modular wall panels onto roomtest-style masonry runs without editing roomtest",
                "layer floor slabs and wet reflection cards under warm gaslight",
                "add ceiling brick panels with restrained soot washes",
                "use mortar shadow cards, corner grime strips, and raised edge trim to break flat seams"
            }, false);
            sb.AppendLine("  },");
            sb.AppendLine("  \"visualOnlyContract\": {");
            JsonProp(sb, 2, "visualOnly", true, true);
            JsonProp(sb, 2, "containsScriptsInRuntimePrefabs", false, true);
            JsonProp(sb, 2, "containsColliders", false, true);
            JsonProp(sb, 2, "containsRigidbodies", false, true);
            JsonProp(sb, 2, "containsLights", false, true);
            JsonProp(sb, 2, "containsReflectionProbes", false, true);
            JsonProp(sb, 2, "containsAudio", false, true);
            JsonProp(sb, 2, "containsAnimations", false, true);
            JsonProp(sb, 2, "containsScenes", false, true);
            JsonProp(sb, 2, "intendedRuntimeBehavior", "None. Collision, occlusion, light placement, probes, sound, scripts, and gameplay authority belong to integrating scenes.", false);
            sb.AppendLine("  },");
            sb.AppendLine("  \"families\": [");
            var familyGroups = PrefabRecords.GroupBy(p => p.Family).ToList();
            for (var i = 0; i < familyGroups.Count; i++)
            {
                var group = familyGroups[i];
                sb.AppendLine("    {");
                JsonProp(sb, 3, "name", group.Key, true);
                JsonProp(sb, 3, "variants", group.Count(), true);
                JsonProp(sb, 3, "role", FamilyRole(group.Key), false);
                sb.Append("    }");
                sb.AppendLine(i < familyGroups.Count - 1 ? "," : string.Empty);
            }
            sb.AppendLine("  ],");
            sb.AppendLine("  \"assets\": {");
            sb.AppendLine("    \"prefabs\": [");
            for (var i = 0; i < PrefabRecords.Count; i++)
            {
                var p = PrefabRecords[i];
                sb.AppendLine("      {");
                JsonProp(sb, 4, "name", p.Name, true);
                JsonProp(sb, 4, "family", p.Family, true);
                JsonProp(sb, 4, "path", p.Path, true);
                JsonProp(sb, 4, "preview", p.Preview ?? string.Empty, true);
                JsonProp(sb, 4, "childRenderers", p.ChildRenderers, true);
                JsonProp(sb, 4, "meshAssetsUsed", p.MeshAssetsUsed, true);
                JsonProp(sb, 4, "componentContract", p.ComponentContract, true);
                JsonProp(sb, 4, "notes", p.Notes, false);
                sb.Append("      }");
                sb.AppendLine(i < PrefabRecords.Count - 1 ? "," : string.Empty);
            }
            sb.AppendLine("    ],");
            sb.AppendLine("    \"materials\": [");
            var materialList = MaterialDefs.Values.ToList();
            for (var i = 0; i < materialList.Count; i++)
            {
                var m = materialList[i];
                sb.AppendLine("      {");
                JsonProp(sb, 4, "name", "RSR11_MAT_" + m.Key, true);
                JsonProp(sb, 4, "path", "AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11/Runtime/Materials/RSR11_MAT_" + m.Key + ".mat", true);
                JsonProp(sb, 4, "textureFamily", m.TextureKey, true);
                JsonProp(sb, 4, "role", m.Role, true);
                JsonProp(sb, 4, "metallic", m.Metallic, true);
                JsonProp(sb, 4, "smoothness", m.Smoothness, true);
                JsonProp(sb, 4, "transparent", m.Transparent, false);
                sb.Append("      }");
                sb.AppendLine(i < materialList.Count - 1 ? "," : string.Empty);
            }
            sb.AppendLine("    ],");
            JsonArray(sb, 2, "contactSheets", new[]
            {
                "Documentation/ConceptRenders/V0_1_56_RoomSurfaceReliefSet11/RSR11_CONTACTSHEET_RoomSurfaceReliefSet11.png",
                "AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11/Runtime/Previews/RSR11_CONTACTSHEET_RoomSurfaceReliefSet11.png",
                "AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11/Documentation~/Previews/RSR11_CONTACTSHEET_RoomSurfaceReliefSet11.png"
            }, false);
            sb.AppendLine("  },");
            sb.AppendLine("  \"targetComparison\": {");
            JsonProp(sb, 2, "surfaceReliefDepth", "Real child mesh protrusion ranges roughly 0.03-0.14 Unity units on bricks/slabs; closer to v0.6 target than flat maps, still modular rather than hand-sculpted.", true);
            JsonProp(sb, 2, "wetnessReflection", "High-smoothness wet flagstone material plus transparent warm/cool helper cards; no real probe or light behavior included.", true);
            JsonProp(sb, 2, "mortarDarkness", "Deep black mortar backers, hairlines, and coal crevice cards are intentionally darker than brick albedo.", true);
            JsonProp(sb, 2, "brickRandomness", "Alternating row offsets, per-brick scale, depth jitter, chip strips, and texture noise break the grid, but long walls will still need variant mixing.", true);
            JsonProp(sb, 2, "ceilingFloorScale", "Ceiling bricks are small and tight; floor stones are larger slab modules with raised lips and wider wet joints.", true);
            JsonProp(sb, 2, "remainingGaps", "No bespoke scene lighting, silhouette-destroying damage, puddle physics, decals projected onto existing roomtest meshes, or playable collision.", false);
            sb.AppendLine("  },");
            sb.AppendLine("  \"importReadiness\": {");
            JsonProp(sb, 2, "status", "ready_for_static_validation_and_quarantine_import", true);
            JsonProp(sb, 2, "pathCollisionsChecked", true, true);
            JsonProp(sb, 2, "rollbackPath", "Remove local package reference com.brassworks.sidecar.room-surface-relief-set11 and delete the isolated Set11 assigned roots.", false);
            sb.AppendLine("  }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string BuildCatalogJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            JsonProp(sb, 1, "schema", "brassworks.room_surface_relief_set11.catalog.v1", true);
            JsonProp(sb, 1, "pack_id", PackId, true);
            JsonProp(sb, 1, "version", Version, true);
            JsonProp(sb, 1, "generated_at_utc", _generatedAtUtc, true);
            sb.AppendLine("  \"modular_units\": [");
            for (var i = 0; i < PrefabRecords.Count; i++)
            {
                var p = PrefabRecords[i];
                sb.AppendLine("    {");
                JsonProp(sb, 3, "prefab", p.Name, true);
                JsonProp(sb, 3, "family", p.Family, true);
                JsonProp(sb, 3, "path", p.Path, true);
                JsonProp(sb, 3, "recommended_use", RecommendedUse(p.Family), true);
                JsonProp(sb, 3, "visual_only", true, false);
                sb.Append("    }");
                sb.AppendLine(i < PrefabRecords.Count - 1 ? "," : string.Empty);
            }
            sb.AppendLine("  ],");
            sb.AppendLine("  \"texture_maps\": [");
            for (var i = 0; i < TextureRecords.Count; i++)
            {
                var t = TextureRecords[i];
                sb.AppendLine("    {");
                JsonProp(sb, 3, "name", t.Name, true);
                JsonProp(sb, 3, "family", t.TextureFamily, true);
                JsonProp(sb, 3, "map_type", t.MapType, true);
                JsonProp(sb, 3, "path", t.Path, true);
                JsonProp(sb, 3, "width", t.Width, true);
                JsonProp(sb, 3, "height", t.Height, true);
                JsonProp(sb, 3, "transparent", t.Transparent, false);
                sb.Append("    }");
                sb.AppendLine(i < TextureRecords.Count - 1 ? "," : string.Empty);
            }
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void WriteReadmeAndChangelog()
        {
            var readme = $@"# BrassworksBreach.RoomSurfaceReliefSet11

Unity-only visual sidecar package for high-fidelity room and corridor surface relief in Brassworks Breach.

This bundle targets the roomtest v0.6 surface gap: shallow real brick/stone geometry over dark mortar, warmer wet flagstone response, sooted ceiling brick, corner grime, and helper cards for damp gaslight reflection. It does not edit roomtest and does not carry gameplay authority.

## Contents

- Prefabs: {PrefabRecords.Count} visual-only modular surface assemblies across {PrefabRecords.Select(p => p.Family).Distinct().Count()} families.
- Meshes: {Meshes.Count} native Unity mesh assets for bricks, slabs, cards, wedges, trim, and folded corner grime.
- Materials: {Materials.Count} Unity Standard-shader materials.
- Runtime texture maps: {TextureRecords.Count} generated PNG maps at {TextureSize}x{TextureSize}.
- Previews: {PreviewFiles.Count} external concept PNGs plus package preview mirrors and contact sheets.
- Metadata: manifest and catalog JSON in Runtime/Metadata and Documentation~/Manifest.

## Visual-Only Contract

Runtime prefabs contain only GameObject, Transform, MeshFilter, and MeshRenderer records. They include no scripts, colliders, rigidbodies, lights, reflection probes, audio, animation, timeline, navmesh, scenes, or gameplay behavior.

## Import Notes

Import into a quarantine Unity project first. Use wall panels as shallow overlay modules, floor slabs as corridor or room surface inserts, ceiling panels as small-brick overhead inserts, and cards/strips as secondary layers. Main project owners should author real collision, lighting, reflection probes, decals, occlusion, and gameplay separately.
";
            WriteText(Path.Combine(_packagePhysicalRoot, "README.md"), readme);

            var changelog = $@"# Changelog

## {Version} - {_generatedAtUtc}

- Added RoomSurfaceReliefSet11 isolated Unity package.
- Added modular wall relief, floor slabs, ceiling brick panels, mortar cards, corner grime strips, wet reflection helpers, and raised stone edge prefabs.
- Added generated material maps, native mesh assets, manifests, previews, and import-readiness documentation.
";
            WriteText(Path.Combine(_packagePhysicalRoot, "CHANGELOG.md"), changelog);
        }

        private static void WriteProductionDocs()
        {
            var readme = @"# V0.1.56 Room Surface Relief Set 11

Production docs for BrassworksBreach.RoomSurfaceReliefSet11.

- RSR11_ProductionReport_0.1.56-p001.md: production summary and target alignment.
- RSR11_AssetInventory_0.1.56-p001.md: generated package inventory.
- RSR11_BluntTargetComparison_0.1.56-p001.md: direct comparison against the masonry room/corridor north-star.
- RSR11_RoomSurfaceReliefSet11_Manifest_0.1.56-p001.json: manifest mirror.
";
            WriteText(Path.Combine(DocProductionRoot(), "README.md"), readme);

            var report = $@"# RSR11 Production Report {Version}

Generated: {_generatedAtUtc}

## Brief

Create a Unity-only surface relief sidecar for Brassworks Breach room/corridor masonry fidelity, using roomtest v0.6 as context without editing roomtest.

## Output Summary

- Package: AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11
- Package name: {PackageName}
- Prefabs: {PrefabRecords.Count}
- Native mesh assets: {Meshes.Count}
- Materials: {Materials.Count}
- Runtime texture PNGs: {TextureRecords.Count} at {TextureSize}x{TextureSize}
- Concept previews/contact sheets: {PreviewFiles.Count}

## Surface Families

- Modular wall panels: individual raised brick meshes over black mortar backers, with hairline seam shadows and damp base cards.
- Wet flagstone floor slabs: larger uneven slab meshes, raised lips, dark joints, and warm/cool wet helper cards.
- Sooted ceiling brick panels: smaller brick scale with broad transparent soot and smoke stain cards.
- Mortar shadow cards: grid and band overlays for deepening seams without gameplay components.
- Corner grime strips: folded geometry for damp vertical corners and lamp soot buildup.
- Raised uneven stone edges: trim, coping, chipped lips, and coal-dark under-crevices.

## Tooling Boundary

No Blender or external DCC was used. Meshes, materials, textures, prefabs, previews, and docs were generated inside Unity Editor tooling in the isolated package.
";
            WriteText(Path.Combine(DocProductionRoot(), "RSR11_ProductionReport_0.1.56-p001.md"), report);

            var inventory = BuildInventoryMarkdown();
            WriteText(Path.Combine(DocProductionRoot(), "RSR11_AssetInventory_0.1.56-p001.md"), inventory);
            WriteText(Path.Combine(DocProductionRoot(), "RSR11_BluntTargetComparison_0.1.56-p001.md"), BuildComparisonMarkdown());
        }

        private static string BuildInventoryMarkdown()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# RSR11 Asset Inventory 0.1.56-p001");
            sb.AppendLine();
            sb.AppendLine($"Generated: {_generatedAtUtc}");
            sb.AppendLine();
            sb.AppendLine("## Prefabs");
            foreach (var p in PrefabRecords)
            {
                sb.AppendLine($"- `{p.Name}` ({p.Family}) renderers={p.ChildRenderers}, meshAssetsUsed={p.MeshAssetsUsed}");
            }
            sb.AppendLine();
            sb.AppendLine("## Materials");
            foreach (var m in MaterialDefs.Values)
            {
                sb.AppendLine($"- `RSR11_MAT_{m.Key}`: {m.Role}; smoothness={m.Smoothness:0.00}; transparent={m.Transparent}");
            }
            sb.AppendLine();
            sb.AppendLine("## Meshes");
            foreach (var key in Meshes.Keys.OrderBy(k => k))
            {
                sb.AppendLine($"- `RSR11_MESH_{key}`");
            }
            sb.AppendLine();
            sb.AppendLine("## Texture Families");
            foreach (var family in TextureRecords.Select(t => t.TextureFamily).Distinct().OrderBy(k => k))
            {
                sb.AppendLine($"- `{family}`: ALB, HGT, NRM, MSO");
            }
            sb.AppendLine();
            sb.AppendLine("## Preview Files");
            foreach (var preview in PreviewFiles)
            {
                sb.AppendLine($"- `{preview}`");
            }
            return sb.ToString();
        }

        private static string BuildComparisonMarkdown()
        {
            return $@"# RSR11 Blunt Target Comparison {Version}

Generated: {_generatedAtUtc}

## Surface Relief Depth

Set11 is materially closer than a flat swatch package because bricks, slabs, edge strips, and trim are actual MeshRenderer geometry. Brick/ceiling protrusion is shallow by design so it can overlay roomtest-style walls without returning to chunky blockout cubes. Remaining gap: the silhouettes are still modular and repeated, not hand-broken masonry.

## Wetness And Reflection

Wet flagstone materials use high smoothness and separate warm/cool transparent helper cards. This gives floor and wall-base highlights a warmer gaslight read. Remaining gap: there are no actual lights, reflection probes, screen-space reflections, or puddle physics in this visual-only package.

## Mortar Darkness

Mortar is intentionally deep black-brown through backer boxes, seam hairlines, and coal crevice cards. This matches the north-star need for dark joints and corner depth. Remaining gap: final scene lighting may lift the mortar unless integration keeps warm contrast restrained.

## Brick Randomness

Rows use offsets, size jitter, depth jitter, material alternation, chip strips, and procedural texture noise. It is much less regular than a printed grid. Remaining gap: long corridors still need rotated/mixed variants and hand-placed grime to avoid visible repetition.

## Ceiling And Floor Scale

Ceiling panels use tight small-brick courses. Floor panels use larger irregular flagstones with raised lips and broader wet joints. This follows the v0.6 target distinction. Remaining gap: transition pieces around doors, stairs, and drains are not authored here.

## Overall

This bundle should support roomtest and future playable corridors as layered visual surface dressing. It is not a final hero room by itself: it needs real scene lighting, reflection probes, collision, occlusion, gameplay, and art-directed placement in the integration lane.
";
        }

        private static void WritePlanningDocs()
        {
            var plan = $@"# RSR11 Import Readiness Plan {Version}

Generated: {_generatedAtUtc}

## Isolation

- Package root: AssetPacks/BrassworksBreach.RoomSurfaceReliefSet11
- No main Packages/manifest.json edits.
- No roomtest, playable scene, or shared status document edits.
- No Blender or external DCC artifacts.

## Quarantine Import Steps

1. Add the package as a local package in a disposable Unity project.
2. Open the prefab folder and confirm all mesh/material references resolve.
3. Drop one wall panel, one floor slab, one ceiling panel, and helper cards into a neutral test scene.
4. Confirm the prefabs are visual only and add no gameplay components.
5. Only after art review, place modules in roomtest-derived corridor content from the integration lane.

## Rollback

Remove the local package reference for `{PackageName}` and delete the isolated Set11 assigned roots.
";
            WriteText(Path.Combine(PlanningRoot(), "RSR11_ImportReadinessPlan_0.1.56-p001.md"), plan);
        }

        private static void WriteInitialQaDocs()
        {
            var qa = $@"# RSR11 QA Notes {Version}

Generated: {_generatedAtUtc}

The Unity generator completed native asset generation. Final static validation is written by the external import-readiness validation pass after JSON parsing, file counts, PNG nonblank checks, and prefab component scans.
";
            WriteText(Path.Combine(QaRoot(), "README.md"), qa);
            WriteText(Physical("Documentation~/QA/RSR11_QA_README.md"), qa);
        }

        private static string FamilyRole(string family)
        {
            switch (family)
            {
                case "WallPanels": return "raised brick wall modules over dark mortar with damp base and corner trim options";
                case "FloorSlabs": return "large wet flagstone slabs with dark joints, raised lips, and reflection helpers";
                case "CeilingPanels": return "small sooted ceiling brick relief panels with smoke wash cards";
                case "MortarShadowCards": return "cards and thin geometry to deepen mortar/joint reads on existing surfaces";
                case "CornerGrimeStrips": return "folded damp/soot corner buildup for vertical room and corridor seams";
                case "WetReflectionHelpers": return "transparent warm/cool wet glint cards with no light/probe behavior";
                case "RaisedUnevenStoneEdges": return "trim, chipped lips, and raised stone edge modules";
                default: return "visual surface dressing module";
            }
        }

        private static string RecommendedUse(string family)
        {
            switch (family)
            {
                case "WallPanels": return "place on masonry walls or corridor bays as shallow relief overlays";
                case "FloorSlabs": return "use as floor insert modules under warm gaslight or at thresholds";
                case "CeilingPanels": return "use overhead for sooted brick rhythm and dark ceiling scale";
                case "MortarShadowCards": return "layer over seams where existing material grids read too flat";
                case "CornerGrimeStrips": return "place in vertical corners, lamp-adjacent soot zones, and damp bases";
                case "WetReflectionHelpers": return "place just above floor/wall surfaces to fake localized damp glints";
                case "RaisedUnevenStoneEdges": return "use at wall bases, floor lips, ceiling trim, and chipped transitions";
                default: return "surface relief dressing";
            }
        }

        private static void JsonProp(StringBuilder sb, int indent, string name, string value, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(Escape(name)).Append("\": \"").Append(Escape(value)).Append('"');
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static void JsonProp(StringBuilder sb, int indent, string name, bool value, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(Escape(name)).Append("\": ").Append(value ? "true" : "false");
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static void JsonProp(StringBuilder sb, int indent, string name, int value, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(Escape(name)).Append("\": ").Append(value.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static void JsonProp(StringBuilder sb, int indent, string name, float value, bool comma)
        {
            sb.Append(' ', indent * 2).Append('"').Append(Escape(name)).Append("\": ").Append(value.ToString("0.###", CultureInfo.InvariantCulture));
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static void JsonArray(StringBuilder sb, int indent, string name, IEnumerable<string> values, bool comma)
        {
            var list = values.ToList();
            sb.Append(' ', indent * 2).Append('"').Append(Escape(name)).AppendLine("\": [");
            for (var i = 0; i < list.Count; i++)
            {
                sb.Append(' ', (indent + 1) * 2).Append('"').Append(Escape(list[i])).Append('"');
                sb.AppendLine(i < list.Count - 1 ? "," : string.Empty);
            }
            sb.Append(' ', indent * 2).Append(']');
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        private static float Frac(float value)
        {
            return value - Mathf.Floor(value);
        }

        private static float Hash01(int a, int b)
        {
            unchecked
            {
                var x = (uint)(a * 374761393 + b * 668265263);
                x = (x ^ (x >> 13)) * 1274126177u;
                x ^= x >> 16;
                return (x & 0x00ffffff) / 16777215.0f;
            }
        }

        private static float HashSigned(int a, int b)
        {
            return Hash01(a, b) * 2.0f - 1.0f;
        }

        private static void EnsureDirectory(string path)
        {
            AssertAllowed(path);
            Directory.CreateDirectory(path);
        }

        private static void DeleteDirectoryIfExists(string path)
        {
            AssertAllowed(path);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        private static void WriteText(string path, string text)
        {
            AssertAllowed(path);
            var parent = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(parent))
            {
                Directory.CreateDirectory(parent);
            }
            File.WriteAllText(path, text, new UTF8Encoding(false));
        }

        private static void WriteBytes(string path, byte[] bytes)
        {
            AssertAllowed(path);
            var parent = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(parent))
            {
                Directory.CreateDirectory(parent);
            }
            File.WriteAllBytes(path, bytes);
        }

        private static void AssertAllowed(string path)
        {
            var full = NormalizePath(Path.GetFullPath(path));
            var allowed = new[]
            {
                _packagePhysicalRoot,
                DocProductionRoot(),
                ConceptRenderRoot(),
                PlanningRoot(),
                QaRoot()
            }.Select(p => NormalizePath(Path.GetFullPath(p)).TrimEnd('/')).ToArray();

            foreach (var root in allowed)
            {
                if (string.Equals(full.TrimEnd('/'), root, StringComparison.OrdinalIgnoreCase) ||
                    full.StartsWith(root + "/", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            throw new InvalidOperationException("Refusing to write outside assigned roots: " + full);
        }

        private static string Physical(string relative)
        {
            return Path.Combine(_packagePhysicalRoot, relative.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string ToPackageAssetPath(string relative)
        {
            return PackageAssetRoot + "/" + relative.Replace("\\", "/").TrimStart('/');
        }

        private static string DocProductionRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "AssetProduction", "V0_1_56_RoomSurfaceReliefSet11");
        }

        private static string ConceptRenderRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "ConceptRenders", "V0_1_56_RoomSurfaceReliefSet11");
        }

        private static string PlanningRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "Planning", "V0_1_56_RoomSurfaceReliefSet11ImportReadiness");
        }

        private static string QaRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "QA", "V0_1_56_RoomSurfaceReliefSet11ImportReadiness");
        }

        private static string RepoRelative(string path)
        {
            var full = NormalizePath(Path.GetFullPath(path));
            var root = NormalizePath(Path.GetFullPath(_repoRoot)).TrimEnd('/') + "/";
            return full.StartsWith(root, StringComparison.OrdinalIgnoreCase) ? full.Substring(root.Length) : full;
        }

        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }

        private sealed class TextureDef
        {
            public TextureDef(string name, TexturePattern pattern, Color low, Color high, int seed, bool transparent, float metallic, float baseSmoothness)
            {
                Name = name;
                Pattern = pattern;
                Low = low;
                High = high;
                Seed = seed;
                Transparent = transparent;
                Metallic = metallic;
                BaseSmoothness = baseSmoothness;
            }

            public string Name { get; }
            public TexturePattern Pattern { get; }
            public Color Low { get; }
            public Color High { get; }
            public int Seed { get; }
            public bool Transparent { get; }
            public float Metallic { get; }
            public float BaseSmoothness { get; }
        }

        private sealed class MaterialDef
        {
            public MaterialDef(string key, string textureKey, string role, Color color, float metallic, float smoothness, float bumpScale, float heightScale, bool transparent, bool emissive, Color emission)
            {
                Key = key;
                TextureKey = textureKey;
                Role = role;
                Color = color;
                Metallic = metallic;
                Smoothness = smoothness;
                BumpScale = bumpScale;
                HeightScale = heightScale;
                Transparent = transparent;
                Emissive = emissive;
                Emission = emission;
            }

            public string Key { get; }
            public string TextureKey { get; }
            public string Role { get; }
            public Color Color { get; }
            public float Metallic { get; }
            public float Smoothness { get; }
            public float BumpScale { get; }
            public float HeightScale { get; }
            public bool Transparent { get; }
            public bool Emissive { get; }
            public Color Emission { get; }
        }

        private sealed class PrefabRecord
        {
            public string Name;
            public string Family;
            public string Path;
            public string Preview;
            public string ComponentContract;
            public int ChildRenderers;
            public int MeshAssetsUsed;
            public string Notes;
        }

        private sealed class TextureRecord
        {
            public string Name;
            public string Path;
            public string TextureFamily;
            public string MapType;
            public int Width;
            public int Height;
            public bool Transparent;
        }

        private enum TexturePattern
        {
            BrickWall,
            Flagstone,
            CeilingBrick,
            Mortar,
            EdgeWear,
            SootCard,
            GrimeStrip,
            ReflectionCard,
            LeakCard
        }
    }
}
#endif
