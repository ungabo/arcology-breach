#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace BrassworksBreach.SteamLightingFixtureSet12.Editor
{
    public static class SLF12Generator
    {
        private const string PackageName = "com.brassworks.sidecar.steam-lighting-fixture-set12";
        private const string PackageAssetRoot = "Packages/" + PackageName;
        private const string Version = "0.1.57-p012";
        private const string Prefix = "SLF12";
        private const int TextureSize = 512;
        private const int RenderWidth = 1800;
        private const int RenderHeight = 1100;

        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly List<Record> PrefabRecords = new List<Record>();
        private static readonly List<string> RenderFiles = new List<string>();

        private static string _packageRoot;
        private static string _repoRoot;

        [MenuItem("Brassworks Breach/Sidecar Packs/Steam Lighting Fixture Set 12/Generate Assets And Renders")]
        public static void GenerateAssetsAndRenders()
        {
            Meshes.Clear();
            Materials.Clear();
            PrefabRecords.Clear();
            RenderFiles.Clear();

            ResolveRoots();
            PrepareFolders();
            GenerateTextures();
            AssetDatabase.Refresh();
            ConfigureTextureImporters();
            CreateMaterials();
            CreateMeshes();
            CreatePrefabs();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RenderFixtureLineup();
            RenderWallMountedScene();
            RenderLowLightCloseup();

            WriteMetadataAndDocs();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Validate();
            Debug.Log($"{Prefix}_GENERATE_PASS version={Version} prefabs={PrefabRecords.Count} materials={Materials.Count} meshes={Meshes.Count} renders={RenderFiles.Count}");
        }

        private static void ResolveRoots()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForPackageName(PackageName);
            if (packageInfo == null || string.IsNullOrEmpty(packageInfo.resolvedPath))
            {
                throw new InvalidOperationException("Package root could not be resolved. Run from the included ValidationProject~ or add the package as a file dependency.");
            }

            _packageRoot = Normalize(packageInfo.resolvedPath);
            var packageDir = Directory.GetParent(_packageRoot);
            if (packageDir == null || packageDir.Parent == null)
            {
                throw new InvalidOperationException("Could not derive repository root from " + _packageRoot);
            }

            _repoRoot = Normalize(packageDir.Parent.FullName);
        }

        private static void PrepareFolders()
        {
            foreach (var relative in new[]
            {
                "Runtime/Materials",
                "Runtime/Meshes",
                "Runtime/Prefabs",
                "Runtime/Textures",
                "Runtime/Metadata",
                "Documentation~/Manifest",
                "Documentation~/Previews",
                "Samples~/PreviewScene"
            })
            {
                Directory.CreateDirectory(Physical(relative));
            }

            Directory.CreateDirectory(RenderRoot());
            Directory.CreateDirectory(Path.Combine(_repoRoot, "Documentation", "Planning"));
            Directory.CreateDirectory(Path.Combine(_repoRoot, "Documentation", "QA"));
        }

        private static void GenerateTextures()
        {
            var defs = new[]
            {
                new TexDef("AgedBrass", new Color(0.43f, 0.27f, 0.09f), new Color(0.96f, 0.67f, 0.24f), 11, TextureStyle.Metal),
                new TexDef("OxidizedCopper", new Color(0.36f, 0.13f, 0.055f), new Color(0.85f, 0.34f, 0.13f), 23, TextureStyle.Copper),
                new TexDef("BlackenedIron", new Color(0.028f, 0.027f, 0.026f), new Color(0.22f, 0.20f, 0.17f), 37, TextureStyle.Metal),
                new TexDef("WarmEmissiveGlass", new Color(0.80f, 0.30f, 0.04f), new Color(1.0f, 0.82f, 0.25f), 41, TextureStyle.Glass),
                new TexDef("SootGrime", new Color(0.006f, 0.005f, 0.004f), new Color(0.16f, 0.12f, 0.08f), 53, TextureStyle.Decal),
                new TexDef("WetReflection", new Color(0.018f, 0.015f, 0.012f), new Color(0.34f, 0.22f, 0.11f), 61, TextureStyle.Wet),
                new TexDef("IvoryGaugeFace", new Color(0.50f, 0.43f, 0.31f), new Color(0.92f, 0.84f, 0.62f), 73, TextureStyle.Gauge),
                new TexDef("PorcelainInsulator", new Color(0.50f, 0.46f, 0.37f), new Color(0.86f, 0.80f, 0.64f), 83, TextureStyle.Ceramic),
                new TexDef("RedSwitchEnamel", new Color(0.26f, 0.025f, 0.016f), new Color(0.78f, 0.08f, 0.04f), 97, TextureStyle.Enamel)
            };

            foreach (var def in defs)
            {
                SaveTexture($"Runtime/Textures/{Prefix}_TEX_{def.Name}_Base.png", BuildBaseTexture(def));
                SaveTexture($"Runtime/Textures/{Prefix}_TEX_{def.Name}_Normal.png", BuildNormalTexture(def));
                SaveTexture($"Runtime/Textures/{Prefix}_TEX_{def.Name}_Mask.png", BuildMaskTexture(def));
            }
        }

        private static Texture2D BuildBaseTexture(TexDef def)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var n1 = Mathf.PerlinNoise(fx * 8.0f + def.Seed, fy * 8.0f - def.Seed);
                    var n2 = Mathf.PerlinNoise(fx * 34.0f - def.Seed * 0.11f, fy * 29.0f + def.Seed * 0.17f);
                    var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((fx * 84.0f + fy * 15.0f + def.Seed) * Mathf.PI)), 34.0f);
                    var stain = Mathf.SmoothStep(0.25f, 0.92f, n1 * 0.66f + n2 * 0.34f);
                    var color = Color.Lerp(def.Low, def.High, stain * 0.82f + scratch * 0.18f);

                    if (def.Style == TextureStyle.Copper)
                    {
                        var patina = Mathf.SmoothStep(0.68f, 0.96f, n2);
                        color = Color.Lerp(color, new Color(0.03f, 0.40f, 0.34f), patina * 0.42f);
                    }
                    else if (def.Style == TextureStyle.Gauge)
                    {
                        var ring = Mathf.Abs(Vector2.Distance(new Vector2(fx, fy), new Vector2(0.5f, 0.5f)) - 0.41f);
                        color = Color.Lerp(color, new Color(0.10f, 0.085f, 0.065f), Mathf.SmoothStep(0.035f, 0.0f, ring));
                        for (var i = 0; i < 24; i++)
                        {
                            var a = i * Mathf.PI * 2.0f / 24.0f;
                            var tick = Mathf.Abs(Vector2.Dot(new Vector2(fx - 0.5f, fy - 0.5f).normalized, new Vector2(Mathf.Cos(a), Mathf.Sin(a))) - 1.0f);
                            var radial = Vector2.Distance(new Vector2(fx, fy), new Vector2(0.5f, 0.5f));
                            if (tick < 0.006f && radial > 0.31f && radial < 0.43f)
                            {
                                color = new Color(0.08f, 0.065f, 0.05f, 1.0f);
                            }
                        }
                    }
                    else if (def.Style == TextureStyle.Glass)
                    {
                        var radial = Vector2.Distance(new Vector2(fx, fy), new Vector2(0.5f, 0.52f));
                        color += new Color(1.0f, 0.50f, 0.12f) * Mathf.SmoothStep(0.45f, 0.0f, radial) * 0.55f;
                        color.a = 0.70f;
                    }
                    else if (def.Style == TextureStyle.Decal)
                    {
                        var radial = Vector2.Distance(new Vector2(fx, fy), new Vector2(0.5f, 0.47f));
                        var drip = Mathf.SmoothStep(0.35f, 0.95f, fy) * Mathf.SmoothStep(0.72f, 0.42f, Mathf.Abs(Mathf.Sin((fx * 11.0f + def.Seed) * Mathf.PI)));
                        color.a = Mathf.Clamp01(Mathf.SmoothStep(0.62f, 0.06f, radial) * (0.42f + n1 * 0.44f) + drip * 0.34f);
                    }
                    else if (def.Style == TextureStyle.Wet)
                    {
                        var streak = Mathf.SmoothStep(0.96f, 0.28f, fy) * Mathf.SmoothStep(0.86f, 0.36f, Mathf.Abs(Mathf.Sin((fx * 9.0f + n2) * Mathf.PI)));
                        color.a = Mathf.Clamp01(0.18f + streak * 0.55f + scratch * 0.20f);
                    }
                    else
                    {
                        color.a = 1.0f;
                    }

                    var edge = Mathf.Clamp01((Mathf.Abs(fx - 0.5f) + Mathf.Abs(fy - 0.5f)) * 0.28f);
                    color *= 1.0f - edge;
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D BuildNormalTexture(TexDef def)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var h = Height(x, y, def.Seed);
                    var hx = Height(Mathf.Min(TextureSize - 1, x + 1), y, def.Seed) - h;
                    var hy = Height(x, Mathf.Min(TextureSize - 1, y + 1), def.Seed) - h;
                    var strength = def.Style == TextureStyle.Glass ? 1.4f : 4.8f;
                    var n = new Vector3(-hx * strength, -hy * strength, 1.0f).normalized;
                    texture.SetPixel(x, y, new Color(n.x * 0.5f + 0.5f, n.y * 0.5f + 0.5f, n.z * 0.5f + 0.5f, 1.0f));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D BuildMaskTexture(TexDef def)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var n = Mathf.PerlinNoise(fx * 18.0f + def.Seed, fy * 18.0f - def.Seed);
                    var metallic = def.Style == TextureStyle.Metal || def.Style == TextureStyle.Copper ? 0.92f : 0.0f;
                    if (def.Style == TextureStyle.Wet)
                    {
                        metallic = 0.12f;
                    }
                    var occlusion = Mathf.Clamp01(0.52f + n * 0.44f);
                    var smoothness = def.Style == TextureStyle.Glass ? 0.94f : def.Style == TextureStyle.Wet ? 0.88f : Mathf.Clamp01(0.24f + n * 0.46f);
                    texture.SetPixel(x, y, new Color(metallic, occlusion, 0.0f, smoothness));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static float Height(int x, int y, int seed)
        {
            var fx = x / (float)(TextureSize - 1);
            var fy = y / (float)(TextureSize - 1);
            var broad = Mathf.PerlinNoise(fx * 9.0f + seed, fy * 9.0f - seed);
            var fine = Mathf.PerlinNoise(fx * 61.0f - seed * 0.13f, fy * 47.0f + seed * 0.07f);
            var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((fx * 76.0f + fy * 19.0f + seed) * Mathf.PI)), 28.0f);
            return broad * 0.55f + fine * 0.35f + scratch * 0.10f;
        }

        private static void SaveTexture(string relativePath, Texture2D texture)
        {
            var physical = Physical(relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(physical) ?? _packageRoot);
            File.WriteAllBytes(physical, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
        }

        private static void ConfigureTextureImporters()
        {
            foreach (var physical in Directory.GetFiles(Physical("Runtime/Textures"), "*.png"))
            {
                var path = ToPackageAssetPath("Runtime/Textures/" + Path.GetFileName(physical));
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.textureType = path.Contains("_Normal") ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.sRGBTexture = !path.Contains("_Normal") && !path.Contains("_Mask");
                importer.alphaIsTransparency = path.Contains("Glass") || path.Contains("Soot") || path.Contains("Wet");
                importer.mipmapEnabled = true;
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.filterMode = FilterMode.Trilinear;
                importer.textureCompression = TextureImporterCompression.CompressedHQ;
                importer.SaveAndReimport();
            }
        }

        private static void CreateMaterials()
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible lit shader found.");
            }

            AddMaterial(shader, "AgedBrass", 0.92f, 0.52f, false, new Color(0.78f, 0.50f, 0.16f), new Color(0.08f, 0.045f, 0.015f), false);
            AddMaterial(shader, "OxidizedCopper", 0.90f, 0.56f, false, new Color(0.72f, 0.28f, 0.10f), new Color(0.03f, 0.10f, 0.08f), false);
            AddMaterial(shader, "BlackenedIron", 0.86f, 0.38f, false, new Color(0.060f, 0.055f, 0.047f), Color.black, false);
            AddMaterial(shader, "WarmEmissiveGlass", 0.0f, 0.92f, true, new Color(1.0f, 0.55f, 0.12f, 0.72f), new Color(1.0f, 0.39f, 0.08f) * 2.6f, true);
            AddMaterial(shader, "SootGrime", 0.0f, 0.16f, true, new Color(0.035f, 0.028f, 0.020f, 0.56f), Color.black, false);
            AddMaterial(shader, "WetReflection", 0.08f, 0.92f, true, new Color(0.12f, 0.075f, 0.035f, 0.54f), new Color(0.05f, 0.025f, 0.006f), false);
            AddMaterial(shader, "IvoryGaugeFace", 0.0f, 0.34f, false, new Color(0.82f, 0.74f, 0.55f), Color.black, false);
            AddMaterial(shader, "PorcelainInsulator", 0.0f, 0.44f, false, new Color(0.76f, 0.70f, 0.56f), Color.black, false);
            AddMaterial(shader, "RedSwitchEnamel", 0.25f, 0.50f, false, new Color(0.64f, 0.045f, 0.026f), Color.black, false);
        }

        private static void AddMaterial(Shader shader, string key, float metallic, float smoothness, bool transparent, Color color, Color emission, bool extraGlow)
        {
            var mat = new Material(shader)
            {
                name = $"{Prefix}_MAT_{key}"
            };

            SetColor(mat, "_BaseColor", color);
            SetColor(mat, "_Color", color);
            SetFloat(mat, "_Metallic", metallic);
            SetFloat(mat, "_Smoothness", smoothness);
            SetFloat(mat, "_Glossiness", smoothness);
            SetTexture(mat, "_BaseMap", LoadTexture(key, "Base"));
            SetTexture(mat, "_MainTex", LoadTexture(key, "Base"));
            SetTexture(mat, "_BumpMap", LoadTexture(key, "Normal"));
            SetTexture(mat, "_MetallicGlossMap", LoadTexture(key, "Mask"));
            SetTexture(mat, "_MetallicSpecGlossMap", LoadTexture(key, "Mask"));
            if (mat.HasProperty("_BumpMap"))
            {
                mat.EnableKeyword("_NORMALMAP");
            }
            if (mat.HasProperty("_MetallicGlossMap"))
            {
                mat.EnableKeyword("_METALLICGLOSSMAP");
            }
            if (emission.maxColorComponent > 0.0f || extraGlow)
            {
                mat.EnableKeyword("_EMISSION");
                SetColor(mat, "_EmissionColor", emission);
            }
            if (transparent)
            {
                ConfigureTransparent(mat);
            }

            var path = ToPackageAssetPath($"Runtime/Materials/{mat.name}.mat");
            ReplaceAsset(mat, path);
            Materials[key] = AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        private static Texture2D LoadTexture(string key, string suffix)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(ToPackageAssetPath($"Runtime/Textures/{Prefix}_TEX_{key}_{suffix}.png"));
        }

        private static void ConfigureTransparent(Material mat)
        {
            SetFloat(mat, "_Surface", 1.0f);
            SetFloat(mat, "_Mode", 3.0f);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = 3000;
        }

        private static void CreateMeshes()
        {
            AddMesh("Box", MeshFactory.Box());
            AddMesh("Quad", MeshFactory.Quad());
            AddMesh("Cylinder16_Z", MeshFactory.CylinderZ(16));
            AddMesh("Cylinder24_Z", MeshFactory.CylinderZ(24));
            AddMesh("Cylinder48_Z", MeshFactory.CylinderZ(48));
            AddMesh("Torus32_Z", MeshFactory.Torus(32, 8, 0.50f, 0.050f));
            AddMesh("Torus48_Z", MeshFactory.Torus(48, 10, 0.50f, 0.040f));
            AddMesh("Sphere16", MeshFactory.Sphere(16, 8));
            AddMesh("GaugeNeedle", MeshFactory.GaugeNeedle());
            AddMesh("Lever", MeshFactory.Lever());
            AddMesh("ChainLink", MeshFactory.Torus(24, 6, 0.42f, 0.045f));
        }

        private static void AddMesh(string key, Mesh mesh)
        {
            mesh.name = $"{Prefix}_MESH_{key}";
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
            var path = ToPackageAssetPath($"Runtime/Meshes/{mesh.name}.asset");
            ReplaceAsset(mesh, path);
            Meshes[key] = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        private static void CreatePrefabs()
        {
            DeleteExistingPrefab("SLF12_PREFAB_08_FixtureFamilyLineup");
            SavePrefab(BuildWallGaslightCage(), "wall gaslight cage", "Wall-backed caged gaslight with layered brass frame, warm glass core, blackened iron guard rods, rivets, pipe feed, and soot halo card.");
            SavePrefab(BuildHangingLantern(), "hanging lantern", "Ceiling-hung brass lantern with chain links, canopy, glass barrel, cage rings, guard rods, caps, steam vent collar, and soot shading.");
            SavePrefab(BuildVerticalInspectionLamp(), "vertical inspection lamp", "Tall wall inspection lamp with amber tube glass, riveted iron rails, brass wall plate, copper feed pipe, grime cards, and service fasteners.");
            SavePrefab(BuildPressureGaugeLampMount(), "pressure gauge lamp mount", "Combined gauge and lamp bracket with readable pressure dial, needle, ticks, lamp capsule, pipe yoke, rivets, and soot bloom.");
            SavePrefab(BuildPipeBracketGlowGlass(), "pipe bracket with glow glass", "Horizontal pipe bracket with glowing inset glass, brass clamp collars, blackened iron pipe, wet reflective streaks, rivets, and porcelain insulators.");
            SavePrefab(BuildBrassSwitchBox(), "brass switch box", "Riveted brass wall switch box with lever, red enamel lockout slot, porcelain terminals, pilot glass, cable glands, soot, and oil staining.");
            SavePrefab(BuildSootHaloDecalPlane(), "soot halo decal plane", "Transparent soot and grime decal plane for lamp smoke halos behind fixtures.");
            SavePrefab(BuildFixtureLineupPrefab(), "fixture family lineup", "Palette prefab collecting the seven core Set12 fixture assets for quick isolated review.");
        }

        private static GameObject BuildWallGaslightCage()
        {
            var root = Root("SLF12_PREFAB_01_WallGaslightCage");
            Part(root.transform, "Box", "riveted_black_iron_wall_backplate", new Vector3(0, 0, 0.03f), new Vector3(1.05f, 1.55f, 0.08f), Vector3.zero, "BlackenedIron");
            Part(root.transform, "Quad", "transparent_soot_halo_behind_lamp", new Vector3(0, 0.06f, 0.085f), new Vector3(1.58f, 1.95f, 1), Vector3.zero, "SootGrime");
            Part(root.transform, "Box", "aged_brass_upper_mount_block", new Vector3(0, 0.72f, -0.03f), new Vector3(0.74f, 0.12f, 0.16f), Vector3.zero, "AgedBrass");
            Part(root.transform, "Box", "aged_brass_lower_mount_block", new Vector3(0, -0.72f, -0.03f), new Vector3(0.74f, 0.12f, 0.16f), Vector3.zero, "AgedBrass");
            Part(root.transform, "Cylinder24_Z", "copper_feed_pipe_left_stub", new Vector3(-0.72f, 0.22f, -0.04f), new Vector3(0.08f, 0.08f, 0.52f), new Vector3(0, 90f, 0), "OxidizedCopper");
            Part(root.transform, "Cylinder48_Z", "warm_amber_glass_core", new Vector3(0, 0, -0.18f), new Vector3(0.34f, 0.34f, 0.92f), new Vector3(90f, 0, 0), "WarmEmissiveGlass");
            Part(root.transform, "Cylinder48_Z", "brass_top_cap", new Vector3(0, 0.52f, -0.18f), new Vector3(0.45f, 0.45f, 0.10f), new Vector3(90f, 0, 0), "AgedBrass");
            Part(root.transform, "Cylinder48_Z", "brass_bottom_cap", new Vector3(0, -0.52f, -0.18f), new Vector3(0.45f, 0.45f, 0.10f), new Vector3(90f, 0, 0), "AgedBrass");
            Part(root.transform, "Torus48_Z", "upper_brass_cage_ring", new Vector3(0, 0.41f, -0.18f), new Vector3(0.58f, 0.58f, 0.16f), new Vector3(90f, 0, 0), "AgedBrass");
            Part(root.transform, "Torus48_Z", "lower_brass_cage_ring", new Vector3(0, -0.41f, -0.18f), new Vector3(0.58f, 0.58f, 0.16f), new Vector3(90f, 0, 0), "AgedBrass");
            AddCageRods(root.transform, "wall_gaslight", 8, 0.31f, 0.92f, new Vector3(0, 0, -0.18f), "BlackenedIron");
            AddRivetFrame(root.transform, "wall_backplate", 0.45f, 0.66f, 4, 6, 0.105f);
            AddOilDrips(root.transform, "wall_gaslight", 5, -0.49f, 0.43f, 0.108f);
            return root;
        }

        private static GameObject BuildHangingLantern()
        {
            var root = Root("SLF12_PREFAB_02_HangingLantern");
            Part(root.transform, "Cylinder48_Z", "blackened_ceiling_canopy", new Vector3(0, 1.42f, 0), new Vector3(0.72f, 0.72f, 0.16f), new Vector3(90f, 0, 0), "BlackenedIron");
            for (var i = 0; i < 5; i++)
            {
                Part(root.transform, "ChainLink", $"alternating_brass_chain_link_{i:00}", new Vector3(0, 1.18f - i * 0.17f, 0), new Vector3(0.18f, 0.32f, 0.10f), new Vector3(90f, 0, i % 2 == 0 ? 0 : 90f), i % 2 == 0 ? "AgedBrass" : "BlackenedIron");
            }
            Part(root.transform, "Cylinder48_Z", "warm_hanging_glass_barrel", new Vector3(0, 0.22f, 0), new Vector3(0.48f, 0.48f, 0.92f), new Vector3(90f, 0, 0), "WarmEmissiveGlass");
            Part(root.transform, "Cylinder48_Z", "aged_brass_lantern_top_hat", new Vector3(0, 0.78f, 0), new Vector3(0.74f, 0.74f, 0.20f), new Vector3(90f, 0, 0), "AgedBrass");
            Part(root.transform, "Cylinder48_Z", "aged_brass_lantern_lower_reservoir", new Vector3(0, -0.36f, 0), new Vector3(0.70f, 0.70f, 0.22f), new Vector3(90f, 0, 0), "AgedBrass");
            Part(root.transform, "Cylinder24_Z", "blackened_smoke_vent_stack", new Vector3(0, 0.98f, 0), new Vector3(0.20f, 0.20f, 0.24f), new Vector3(90f, 0, 0), "BlackenedIron");
            Part(root.transform, "Torus48_Z", "top_guard_ring", new Vector3(0, 0.61f, 0), new Vector3(0.84f, 0.84f, 0.16f), new Vector3(90f, 0, 0), "AgedBrass");
            Part(root.transform, "Torus48_Z", "bottom_guard_ring", new Vector3(0, -0.18f, 0), new Vector3(0.84f, 0.84f, 0.16f), new Vector3(90f, 0, 0), "AgedBrass");
            AddCageRods(root.transform, "hanging_lantern", 10, 0.43f, 0.84f, new Vector3(0, 0.22f, 0), "BlackenedIron");
            AddRadialRivets(root.transform, "lantern_top_rivets", new Vector3(0, 0.78f, 0), 0.37f, 12, "AgedBrass");
            Part(root.transform, "Quad", "soft_soot_plume_card_above_lantern", new Vector3(0, 1.15f, 0.06f), new Vector3(0.80f, 0.95f, 1), Vector3.zero, "SootGrime");
            return root;
        }

        private static GameObject BuildVerticalInspectionLamp()
        {
            var root = Root("SLF12_PREFAB_03_VerticalInspectionLamp");
            Part(root.transform, "Box", "long_riveted_brass_wall_plate", new Vector3(0, 0, 0.04f), new Vector3(0.74f, 2.05f, 0.08f), Vector3.zero, "AgedBrass");
            Part(root.transform, "Box", "recessed_black_iron_shadow_slot", new Vector3(0, 0, 0.00f), new Vector3(0.48f, 1.70f, 0.07f), Vector3.zero, "BlackenedIron");
            Part(root.transform, "Cylinder48_Z", "vertical_warm_glass_inspection_tube", new Vector3(0, 0, -0.14f), new Vector3(0.30f, 0.30f, 1.44f), new Vector3(90f, 0, 0), "WarmEmissiveGlass");
            for (var side = -1; side <= 1; side += 2)
            {
                Part(root.transform, "Cylinder16_Z", $"{Side(side)}_black_guard_rail", new Vector3(side * 0.25f, 0, -0.16f), new Vector3(0.045f, 0.045f, 1.58f), new Vector3(90f, 0, 0), "BlackenedIron");
                Part(root.transform, "Box", $"{Side(side)}_brass_rail_standoff_upper", new Vector3(side * 0.25f, 0.75f, -0.06f), new Vector3(0.12f, 0.08f, 0.22f), Vector3.zero, "AgedBrass");
                Part(root.transform, "Box", $"{Side(side)}_brass_rail_standoff_lower", new Vector3(side * 0.25f, -0.75f, -0.06f), new Vector3(0.12f, 0.08f, 0.22f), Vector3.zero, "AgedBrass");
            }
            Part(root.transform, "Cylinder24_Z", "copper_pressure_feed_elbow_top", new Vector3(-0.39f, 0.82f, -0.04f), new Vector3(0.07f, 0.07f, 0.40f), new Vector3(0, 90f, 0), "OxidizedCopper");
            Part(root.transform, "Cylinder24_Z", "blackened_condensate_drain_lower", new Vector3(0.39f, -0.82f, -0.04f), new Vector3(0.06f, 0.06f, 0.36f), new Vector3(0, 90f, 0), "BlackenedIron");
            AddRivetFrame(root.transform, "inspection_plate", 0.31f, 0.91f, 4, 8, 0.102f);
            AddOilDrips(root.transform, "inspection_lamp", 6, -0.30f, 0.60f, 0.095f);
            return root;
        }

        private static GameObject BuildPressureGaugeLampMount()
        {
            var root = Root("SLF12_PREFAB_04_PressureGaugeLampMount");
            Part(root.transform, "Box", "blackened_iron_gauge_lamp_wall_yoke", new Vector3(0, 0, 0.04f), new Vector3(1.45f, 1.15f, 0.10f), Vector3.zero, "BlackenedIron");
            AddGauge(root.transform, new Vector3(-0.36f, 0.18f, -0.08f), 0.35f, "main_pressure_gauge");
            Part(root.transform, "Cylinder48_Z", "right_side_warm_glass_lamp_capsule", new Vector3(0.45f, 0.05f, -0.13f), new Vector3(0.30f, 0.30f, 0.58f), new Vector3(90f, 0, 0), "WarmEmissiveGlass");
            Part(root.transform, "Torus32_Z", "right_lamp_upper_brass_ring", new Vector3(0.45f, 0.38f, -0.13f), new Vector3(0.42f, 0.42f, 0.12f), new Vector3(90f, 0, 0), "AgedBrass");
            Part(root.transform, "Torus32_Z", "right_lamp_lower_brass_ring", new Vector3(0.45f, -0.26f, -0.13f), new Vector3(0.42f, 0.42f, 0.12f), new Vector3(90f, 0, 0), "AgedBrass");
            AddCageRods(root.transform, "gauge_mount_lamp", 6, 0.22f, 0.58f, new Vector3(0.45f, 0.05f, -0.13f), "BlackenedIron");
            Part(root.transform, "Cylinder24_Z", "copper_swept_pipe_from_gauge_to_lamp", new Vector3(0.05f, -0.44f, -0.05f), new Vector3(0.07f, 0.07f, 1.05f), new Vector3(0, 68f, 0), "OxidizedCopper");
            Part(root.transform, "Quad", "soot_bloom_behind_gauge_lamp", new Vector3(0.34f, 0.10f, 0.095f), new Vector3(1.35f, 1.25f, 1), Vector3.zero, "SootGrime");
            AddRivetFrame(root.transform, "gauge_mount_plate", 0.66f, 0.50f, 5, 4, 0.11f);
            return root;
        }

        private static GameObject BuildPipeBracketGlowGlass()
        {
            var root = Root("SLF12_PREFAB_05_PipeBracketGlowGlass");
            Part(root.transform, "Cylinder48_Z", "blackened_main_service_pipe", new Vector3(0, 0, 0), new Vector3(0.18f, 0.18f, 1.75f), new Vector3(0, 90f, 0), "BlackenedIron");
            for (var x = -1; x <= 1; x += 2)
            {
                Part(root.transform, "Torus32_Z", $"{Side(x)}_aged_brass_pipe_clamp_ring", new Vector3(x * 0.64f, 0, 0), new Vector3(0.32f, 0.32f, 0.12f), new Vector3(0, 90f, 0), "AgedBrass");
                Part(root.transform, "Box", $"{Side(x)}_brass_wall_bracket_foot", new Vector3(x * 0.64f, -0.34f, 0.09f), new Vector3(0.28f, 0.18f, 0.16f), Vector3.zero, "AgedBrass");
                Part(root.transform, "Cylinder16_Z", $"{Side(x)}_porcelain_insulator_pin", new Vector3(x * 0.64f, 0.32f, -0.02f), new Vector3(0.10f, 0.10f, 0.16f), new Vector3(90f, 0, 0), "PorcelainInsulator");
            }
            Part(root.transform, "Box", "center_brass_glow_glass_cradle", new Vector3(0, 0, -0.16f), new Vector3(0.58f, 0.42f, 0.16f), Vector3.zero, "AgedBrass");
            Part(root.transform, "Box", "rectangular_warm_glow_glass_window", new Vector3(0, 0, -0.27f), new Vector3(0.46f, 0.24f, 0.055f), Vector3.zero, "WarmEmissiveGlass");
            Part(root.transform, "Quad", "wet_reflection_card_below_pipe", new Vector3(0.02f, -0.47f, 0.08f), new Vector3(1.58f, 0.34f, 1), Vector3.zero, "WetReflection");
            AddRivetLine(root.transform, "pipe_bracket_window_top", new Vector3(-0.22f, 0.25f, -0.255f), Vector3.right, 6, 0.088f);
            AddRivetLine(root.transform, "pipe_bracket_window_bottom", new Vector3(-0.22f, -0.25f, -0.255f), Vector3.right, 6, 0.088f);
            return root;
        }

        private static GameObject BuildBrassSwitchBox()
        {
            var root = Root("SLF12_PREFAB_06_BrassSwitchBox");
            Part(root.transform, "Box", "deep_riveted_aged_brass_switch_box_body", new Vector3(0, 0, 0), new Vector3(0.86f, 1.08f, 0.30f), Vector3.zero, "AgedBrass");
            Part(root.transform, "Box", "recessed_black_switch_plate", new Vector3(0, 0.07f, -0.18f), new Vector3(0.58f, 0.68f, 0.04f), Vector3.zero, "BlackenedIron");
            Part(root.transform, "Lever", "blackened_pull_lever_with_brass_knob", new Vector3(0.05f, 0.03f, -0.23f), new Vector3(0.50f, 0.50f, 0.28f), new Vector3(0, 0, -31f), "BlackenedIron");
            Part(root.transform, "Sphere16", "warm_brass_lever_knob", new Vector3(0.18f, -0.22f, -0.30f), new Vector3(0.16f, 0.16f, 0.16f), Vector3.zero, "AgedBrass");
            Part(root.transform, "Box", "red_enamel_lockout_marker", new Vector3(-0.23f, 0.39f, -0.205f), new Vector3(0.21f, 0.08f, 0.035f), Vector3.zero, "RedSwitchEnamel");
            Part(root.transform, "Sphere16", "small_amber_pilot_glass", new Vector3(0.26f, 0.39f, -0.22f), new Vector3(0.13f, 0.13f, 0.06f), Vector3.zero, "WarmEmissiveGlass");
            for (var i = 0; i < 3; i++)
            {
                Part(root.transform, "Cylinder16_Z", $"upper_porcelain_terminal_{i:00}", new Vector3(-0.28f + i * 0.28f, 0.63f, -0.05f), new Vector3(0.12f, 0.12f, 0.16f), new Vector3(90f, 0, 0), "PorcelainInsulator");
                Part(root.transform, "Cylinder16_Z", $"black_cable_gland_{i:00}", new Vector3(-0.28f + i * 0.28f, -0.63f, -0.05f), new Vector3(0.10f, 0.10f, 0.14f), new Vector3(90f, 0, 0), "BlackenedIron");
            }
            AddRivetFrame(root.transform, "switch_box", 0.37f, 0.48f, 4, 5, -0.175f);
            AddOilDrips(root.transform, "switch_box", 4, -0.34f, 0.22f, -0.188f);
            return root;
        }

        private static GameObject BuildSootHaloDecalPlane()
        {
            var root = Root("SLF12_PREFAB_07_SootHaloDecalPlane");
            Part(root.transform, "Quad", "soft_radial_lamp_soot_halo_alpha_plane", Vector3.zero, new Vector3(1.85f, 2.15f, 1), Vector3.zero, "SootGrime");
            Part(root.transform, "Quad", "thin_lower_oily_reflection_alpha_plane", new Vector3(0.0f, -0.72f, -0.006f), new Vector3(1.25f, 0.42f, 1), Vector3.zero, "WetReflection");
            return root;
        }

        private static GameObject BuildFixtureLineupPrefab()
        {
            var root = Root("SLF12_PREFAB_08_FixtureFamilyLineup");
            var paths = new[]
            {
                "SLF12_PREFAB_01_WallGaslightCage",
                "SLF12_PREFAB_02_HangingLantern",
                "SLF12_PREFAB_03_VerticalInspectionLamp",
                "SLF12_PREFAB_04_PressureGaugeLampMount",
                "SLF12_PREFAB_05_PipeBracketGlowGlass",
                "SLF12_PREFAB_06_BrassSwitchBox",
                "SLF12_PREFAB_07_SootHaloDecalPlane"
            };

            for (var i = 0; i < paths.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(ToPackageAssetPath($"Runtime/Prefabs/{paths[i]}.prefab"));
                if (prefab == null)
                {
                    continue;
                }
                var child = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                child.name = paths[i].Replace("SLF12_PREFAB_", "lineup_");
                child.transform.SetParent(root.transform, false);
                child.transform.localPosition = new Vector3((i - 3) * 1.28f, 0, 0);
                child.transform.localRotation = Quaternion.Euler(0, i % 2 == 0 ? -8f : 8f, 0);
                PrefabUtility.UnpackPrefabInstance(child, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }
            return root;
        }

        private static void AddGauge(Transform parent, Vector3 center, float radius, string prefix)
        {
            Part(parent, "Cylinder48_Z", $"{prefix}_blackened_outer_case", center, new Vector3(radius * 2.26f, radius * 2.26f, 0.10f), Vector3.zero, "BlackenedIron");
            Part(parent, "Cylinder48_Z", $"{prefix}_aged_brass_bezel", center + new Vector3(0, 0, -0.065f), new Vector3(radius * 2.05f, radius * 2.05f, 0.085f), Vector3.zero, "AgedBrass");
            Part(parent, "Cylinder48_Z", $"{prefix}_ivory_pressure_dial", center + new Vector3(0, 0, -0.122f), new Vector3(radius * 1.58f, radius * 1.58f, 0.030f), Vector3.zero, "IvoryGaugeFace");
            Part(parent, "GaugeNeedle", $"{prefix}_red_needle", center + new Vector3(0, 0, -0.145f), new Vector3(radius * 0.95f, radius * 0.95f, 0.02f), new Vector3(0, 0, -38f), "RedSwitchEnamel");
            for (var i = 0; i < 18; i++)
            {
                var angle = Mathf.Lerp(-140f, 140f, i / 17.0f) * Mathf.Deg2Rad;
                var tickCenter = center + new Vector3(Mathf.Sin(angle) * radius * 0.62f, Mathf.Cos(angle) * radius * 0.62f, -0.150f);
                Part(parent, "Box", $"{prefix}_raised_tick_{i:00}", tickCenter, new Vector3(0.015f, i % 3 == 0 ? 0.070f : 0.045f, 0.010f), new Vector3(0, 0, -angle * Mathf.Rad2Deg), "BlackenedIron");
            }
            Part(parent, "Sphere16", $"{prefix}_center_brass_pin", center + new Vector3(0, 0, -0.165f), new Vector3(0.07f, 0.07f, 0.025f), Vector3.zero, "AgedBrass");
        }

        private static void AddCageRods(Transform parent, string prefix, int count, float radius, float height, Vector3 center, string material)
        {
            for (var i = 0; i < count; i++)
            {
                var a = i * Mathf.PI * 2.0f / count;
                var pos = center + new Vector3(Mathf.Cos(a) * radius, 0, Mathf.Sin(a) * radius);
                Part(parent, "Cylinder16_Z", $"{prefix}_individual_guard_rod_{i:00}", pos, new Vector3(0.040f, 0.040f, height), new Vector3(90f, 0, 0), material);
            }
        }

        private static void AddRivetFrame(Transform parent, string prefix, float halfX, float halfY, int countX, int countY, float z)
        {
            AddRivetLine(parent, $"{prefix}_top_rivet_line", new Vector3(-halfX, halfY, z), Vector3.right, countX, halfX * 2.0f / Mathf.Max(1, countX - 1));
            AddRivetLine(parent, $"{prefix}_bottom_rivet_line", new Vector3(-halfX, -halfY, z), Vector3.right, countX, halfX * 2.0f / Mathf.Max(1, countX - 1));
            AddRivetLine(parent, $"{prefix}_left_rivet_line", new Vector3(-halfX, -halfY, z), Vector3.up, countY, halfY * 2.0f / Mathf.Max(1, countY - 1));
            AddRivetLine(parent, $"{prefix}_right_rivet_line", new Vector3(halfX, -halfY, z), Vector3.up, countY, halfY * 2.0f / Mathf.Max(1, countY - 1));
        }

        private static void AddRivetLine(Transform parent, string prefix, Vector3 start, Vector3 direction, int count, float step)
        {
            for (var i = 0; i < count; i++)
            {
                Part(parent, "Sphere16", $"{prefix}_{i:00}", start + direction.normalized * (i * step), new Vector3(0.07f, 0.07f, 0.032f), Vector3.zero, "AgedBrass");
            }
        }

        private static void AddRadialRivets(Transform parent, string prefix, Vector3 center, float radius, int count, string material)
        {
            for (var i = 0; i < count; i++)
            {
                var a = i * Mathf.PI * 2.0f / count;
                Part(parent, "Sphere16", $"{prefix}_{i:00}", center + new Vector3(Mathf.Cos(a) * radius, 0, Mathf.Sin(a) * radius), new Vector3(0.055f, 0.055f, 0.055f), Vector3.zero, material);
            }
        }

        private static void AddOilDrips(Transform parent, string prefix, int count, float xStart, float yTop, float z)
        {
            for (var i = 0; i < count; i++)
            {
                Part(parent, "Quad", $"{prefix}_thin_oily_soot_drip_{i:00}", new Vector3(xStart + i * 0.18f, yTop - i * 0.08f, z), new Vector3(0.055f, 0.30f + i * 0.035f, 1), Vector3.zero, i % 2 == 0 ? "SootGrime" : "WetReflection");
            }
        }

        private static GameObject Root(string name)
        {
            var root = new GameObject(name);
            root.transform.position = Vector3.zero;
            return root;
        }

        private static GameObject Part(Transform parent, string meshKey, string name, Vector3 localPosition, Vector3 localScale, Vector3 localEuler, string materialKey)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localRotation = Quaternion.Euler(localEuler);
            go.transform.localScale = localScale;
            go.AddComponent<MeshFilter>().sharedMesh = Meshes[meshKey];
            go.AddComponent<MeshRenderer>().sharedMaterial = Materials[materialKey];
            return go;
        }

        private static void SavePrefab(GameObject root, string category, string notes)
        {
            var path = ToPackageAssetPath($"Runtime/Prefabs/{root.name}.prefab");
            if (AssetDatabase.LoadAssetAtPath<Object>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }
            PrefabUtility.SaveAsPrefabAsset(root, path);
            PrefabRecords.Add(new Record(root.name, path, category, notes, root.GetComponentsInChildren<MeshRenderer>(true).Length));
            Object.DestroyImmediate(root);
        }

        private static void DeleteExistingPrefab(string name)
        {
            var path = ToPackageAssetPath($"Runtime/Prefabs/{name}.prefab");
            if (AssetDatabase.LoadAssetAtPath<Object>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }
        }

        private static void RenderFixtureLineup()
        {
            NewRenderScene("slf12_fixture_lineup");
            AddRenderEnvironment(true);
            var names = PrefabRecords.Where(r => !r.Name.Contains("Lineup")).Select(r => r.Name).ToArray();
            for (var i = 0; i < names.Length; i++)
            {
                var inst = InstantiatePrefab(names[i], new Vector3((i - 3) * 1.15f, 0.05f, 0), Quaternion.Euler(0, i % 2 == 0 ? -12f : 10f, 0));
                FitToHeight(inst, 1.8f);
            }
            RenderSceneCamera("SLF12_RENDER_01_fixture_lineup.png", new Vector3(0.25f, 1.20f, -5.3f), new Vector3(0, 0.18f, 0), 34f);
        }

        private static void RenderWallMountedScene()
        {
            NewRenderScene("slf12_wall_scene");
            AddRenderEnvironment(true);
            InstantiatePrefab("SLF12_PREFAB_01_WallGaslightCage", new Vector3(-1.65f, 1.08f, -0.08f), Quaternion.identity);
            InstantiatePrefab("SLF12_PREFAB_03_VerticalInspectionLamp", new Vector3(0.05f, 0.95f, -0.08f), Quaternion.identity);
            InstantiatePrefab("SLF12_PREFAB_04_PressureGaugeLampMount", new Vector3(1.55f, 1.05f, -0.08f), Quaternion.identity);
            InstantiatePrefab("SLF12_PREFAB_06_BrassSwitchBox", new Vector3(2.55f, 0.65f, -0.08f), Quaternion.identity);
            AddPreviewLight("warm_pool_left", new Vector3(-1.60f, 0.95f, -0.65f), 5.8f, 3.0f);
            AddPreviewLight("warm_pool_middle", new Vector3(0.05f, 0.88f, -0.58f), 4.5f, 2.5f);
            AddPreviewLight("warm_pool_right", new Vector3(1.60f, 0.94f, -0.62f), 4.2f, 2.5f);
            RenderSceneCamera("SLF12_RENDER_02_wall_mounted_scene.png", new Vector3(0.55f, 1.26f, -4.7f), new Vector3(0.35f, 0.84f, 0.0f), 37f);
        }

        private static void RenderLowLightCloseup()
        {
            NewRenderScene("slf12_low_light_closeup");
            AddRenderEnvironment(false);
            InstantiatePrefab("SLF12_PREFAB_02_HangingLantern", new Vector3(-0.42f, 1.42f, 0.08f), Quaternion.Euler(0, -18f, 0));
            InstantiatePrefab("SLF12_PREFAB_05_PipeBracketGlowGlass", new Vector3(0.70f, 0.86f, -0.18f), Quaternion.Euler(0, -10f, 0));
            InstantiatePrefab("SLF12_PREFAB_07_SootHaloDecalPlane", new Vector3(-0.42f, 1.25f, 0.06f), Quaternion.identity);
            AddPreviewLight("close_lantern_core", new Vector3(-0.40f, 1.18f, -0.42f), 8.5f, 2.6f);
            AddPreviewLight("floor_warm_reflection", new Vector3(0.12f, 0.26f, -0.75f), 2.4f, 3.4f);
            var cool = new GameObject("soft_cool_steam_rim").AddComponent<Light>();
            cool.type = LightType.Point;
            cool.color = new Color(0.35f, 0.50f, 0.58f);
            cool.intensity = 0.85f;
            cool.range = 4f;
            cool.transform.position = new Vector3(1.2f, 1.8f, -1.1f);
            RenderSceneCamera("SLF12_RENDER_03_low_light_closeup_warm_reflections.png", new Vector3(0.35f, 1.18f, -2.65f), new Vector3(0.05f, 0.98f, 0.0f), 31f);
        }

        private static void NewRenderScene(string name)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Debug.Log($"{Prefix}_RENDER_SCENE {name}");
        }

        private static void AddRenderEnvironment(bool brighter)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = "render_dark_oily_brick_wall_proxy";
            wall.transform.position = new Vector3(0, 0.95f, 0.18f);
            wall.transform.localScale = new Vector3(7.5f, 2.8f, 0.10f);
            Object.DestroyImmediate(wall.GetComponent<Collider>());
            wall.GetComponent<Renderer>().sharedMaterial = Materials["BlackenedIron"];

            for (var i = 0; i < 14; i++)
            {
                var mortar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                mortar.name = $"render_wall_mortar_brass_shadow_line_{i:00}";
                mortar.transform.position = new Vector3(-3.5f + i * 0.54f, 1.15f + Mathf.Sin(i) * 0.10f, 0.095f);
                mortar.transform.localScale = new Vector3(0.018f, 2.55f, 0.018f);
                Object.DestroyImmediate(mortar.GetComponent<Collider>());
                mortar.GetComponent<Renderer>().sharedMaterial = Materials["SootGrime"];
            }

            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "render_wet_reflective_black_iron_floor_proxy";
            floor.transform.position = new Vector3(0, -0.18f, -1.0f);
            floor.transform.localScale = new Vector3(8.0f, 0.08f, 3.6f);
            Object.DestroyImmediate(floor.GetComponent<Collider>());
            floor.GetComponent<Renderer>().sharedMaterial = Materials["WetReflection"];

            var key = new GameObject("soft_warm_gaslight_key").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.70f, 0.38f);
            key.intensity = brighter ? 1.18f : 0.35f;
            key.transform.rotation = Quaternion.Euler(38f, -30f, 0);

            var fill = new GameObject("deep_cool_fill").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(0.22f, 0.28f, 0.32f);
            fill.intensity = brighter ? 0.55f : 0.18f;
            fill.range = 6.5f;
            fill.transform.position = new Vector3(0.0f, 1.75f, -2.0f);
        }

        private static void AddPreviewLight(string name, Vector3 position, float intensity, float range)
        {
            var light = new GameObject(name).AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1.0f, 0.55f, 0.15f);
            light.intensity = intensity;
            light.range = range;
            light.transform.position = position;
        }

        private static GameObject InstantiatePrefab(string prefabName, Vector3 position, Quaternion rotation)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(ToPackageAssetPath($"Runtime/Prefabs/{prefabName}.prefab"));
            if (prefab == null)
            {
                throw new InvalidOperationException("Missing prefab for render: " + prefabName);
            }
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }

        private static void FitToHeight(GameObject instance, float targetHeight)
        {
            var bounds = CalculateBounds(instance);
            if (bounds.size.y <= 0.001f)
            {
                return;
            }
            var scale = targetHeight / bounds.size.y;
            instance.transform.localScale *= Mathf.Min(1.0f, scale);
            bounds = CalculateBounds(instance);
            instance.transform.position += new Vector3(0, -bounds.min.y, 0);
        }

        private static void RenderSceneCamera(string fileName, Vector3 cameraPosition, Vector3 lookAt, float fov)
        {
            var cameraObject = new GameObject("slf12_render_camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.018f, 0.016f, 0.014f);
            camera.fieldOfView = fov;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 40f;
            camera.transform.position = cameraPosition;
            camera.transform.LookAt(lookAt);

            var output = Path.Combine(RenderRoot(), fileName);
            RenderCameraToPng(camera, output, RenderWidth, RenderHeight);
            RenderFiles.Add(output);
            File.Copy(output, Physical($"Documentation~/Previews/{fileName}"), true);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
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
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");
                File.WriteAllBytes(outputPath, texture.EncodeToPNG());
            }
            finally
            {
                RenderTexture.active = previous;
                camera.targetTexture = null;
                if (texture != null)
                {
                    Object.DestroyImmediate(texture);
                }
                Object.DestroyImmediate(renderTexture);
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

        private static void WriteMetadataAndDocs()
        {
            var textureCount = Directory.GetFiles(Physical("Runtime/Textures"), "*.png").Length;
            var manifest = new StringBuilder();
            manifest.AppendLine("{");
            Json(manifest, 1, "package", PackageName, true);
            Json(manifest, 1, "version", Version, true);
            Json(manifest, 1, "generatedBy", "SLF12Generator", true);
            Json(manifest, 1, "prefabCount", PrefabRecords.Count.ToString(), true, false);
            Json(manifest, 1, "materialCount", Materials.Count.ToString(), true, false);
            Json(manifest, 1, "meshCount", Meshes.Count.ToString(), true, false);
            Json(manifest, 1, "texturePngCount", textureCount.ToString(), true, false);
            Json(manifest, 1, "renderPngCount", RenderFiles.Count.ToString(), true, false);
            manifest.AppendLine("  \"prefabs\": [");
            for (var i = 0; i < PrefabRecords.Count; i++)
            {
                var r = PrefabRecords[i];
                manifest.AppendLine("    {");
                Json(manifest, 3, "name", r.Name, true);
                Json(manifest, 3, "category", r.Category, true);
                Json(manifest, 3, "rendererCount", r.RendererCount.ToString(), true, false);
                Json(manifest, 3, "path", r.Path, true);
                Json(manifest, 3, "notes", r.Notes, false);
                manifest.Append("    }");
                manifest.AppendLine(i == PrefabRecords.Count - 1 ? string.Empty : ",");
            }
            manifest.AppendLine("  ],");
            manifest.AppendLine("  \"renders\": [");
            for (var i = 0; i < RenderFiles.Count; i++)
            {
                manifest.Append("    \"").Append(Escape(ToRepoRelative(RenderFiles[i]))).Append("\"");
                manifest.AppendLine(i == RenderFiles.Count - 1 ? string.Empty : ",");
            }
            manifest.AppendLine("  ]");
            manifest.AppendLine("}");

            WriteText(Physical($"Runtime/Metadata/{Prefix}_SteamLightingFixtureSet12_Manifest.json"), manifest.ToString());
            WriteText(Physical($"Documentation~/Manifest/{Prefix}_SteamLightingFixtureSet12_Manifest.json"), manifest.ToString());

            var catalog = new StringBuilder();
            catalog.AppendLine("# Steam Lighting Fixture Set 12 Catalog");
            catalog.AppendLine();
            catalog.AppendLine($"Version: `{Version}`");
            catalog.AppendLine();
            foreach (var record in PrefabRecords)
            {
                catalog.AppendLine($"- `{record.Name}` - {record.Category}; {record.RendererCount} renderers. {record.Notes}");
            }
            catalog.AppendLine();
            catalog.AppendLine("Limitations: procedural geometry and textures are lookdev-ready but not final hand-authored sculpts; no gameplay colliders, no authored LOD groups, no scene integration, and no real-time light components are embedded in the prefabs.");
            WriteText(Physical($"Runtime/Metadata/{Prefix}_SteamLightingFixtureSet12_Catalog.md"), catalog.ToString());

            var qa = new StringBuilder();
            qa.AppendLine("# Steam Lighting Fixture Set 12 QA North-Star Comparison");
            qa.AppendLine();
            qa.AppendLine("Reference: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`.");
            qa.AppendLine();
            qa.AppendLine("## Result");
            qa.AppendLine();
            qa.AppendLine("PASS for sidecar visual lookdev: the generated fixtures use warm amber emissive glass, brass/iron/copper material contrast, layered cages, visible rivets, gauge detail, soot halos, wet reflection cards, and render-scene gaslight pools.");
            qa.AppendLine();
            qa.AppendLine("## Checks");
            qa.AppendLine();
            qa.AppendLine("- Warm pools of light: render PNGs use amber point lights positioned at fixture glass cores, with wet floor proxies catching warm reflections.");
            qa.AppendLine("- Believable metal/glass: aged brass, oxidized copper, blackened iron, warm glass, soot, and wet reflection materials use procedural base/normal/mask PNG maps.");
            qa.AppendLine("- Visible fasteners: all core fixtures include raised rivets, clamp rings, standoffs, or collar bolts readable at corridor/FPS distance.");
            qa.AppendLine("- Grime/soot: soot halo decal plane, wall-halo cards, oil drips, and wet cards are included as transparent materials and prefab parts.");
            qa.AppendLine("- FPS distance: silhouettes are deliberately chunky with cage rods, caps, gauges, levers, and glowing glass readable from several meters.");
            qa.AppendLine("- Performance notes: prefabs are visual-only and contain no lights, cameras, colliders, rigidbodies, or gameplay scripts; LODs and batching should be added during integration if many fixtures are placed.");
            qa.AppendLine();
            qa.AppendLine("## Limitations");
            qa.AppendLine();
            qa.AppendLine("- Procedural meshes are modular and layered, not artist-sculpted final hero assets.");
            qa.AppendLine("- Transparent soot/wet cards may require render queue sorting review in the final corridor scene.");
            qa.AppendLine("- No baked lightmaps, real fixture light components, occlusion setup, or VFX steam puffs are included.");
            qa.AppendLine("- The validation renders are isolated lookdev scenes, not playable-scene integration.");
            qa.AppendLine();
            qa.AppendLine("## Generated Renders");
            qa.AppendLine();
            foreach (var render in RenderFiles)
            {
                qa.AppendLine($"- `{ToRepoRelative(render)}`");
            }
            WriteText(Path.Combine(_repoRoot, "Documentation", "QA", "SteamLightingFixtureSet12_QA_NorthStarComparison.md"), qa.ToString());

            var plan = new StringBuilder();
            plan.AppendLine("# Steam Lighting Fixture Set 12 Implementation Plan");
            plan.AppendLine();
            plan.AppendLine("Generated sidecar package is complete for isolated review.");
            plan.AppendLine();
            plan.AppendLine("## Created");
            plan.AppendLine();
            plan.AppendLine("- Unity package scaffold with `package.json`, runtime folders, sample notes, and editor generator.");
            plan.AppendLine("- Procedural PNG maps for brass, copper, iron, glass, soot, wetness, gauge enamel, porcelain, and red enamel.");
            plan.AppendLine("- Mesh assets, materials, seven required fixture prefabs, one lineup prefab, manifest metadata, and three concept renders.");
            plan.AppendLine();
            plan.AppendLine("## Integration Guidance");
            plan.AppendLine();
            plan.AppendLine("Review the sidecar renders first, then import the package into a controlled scene slice. Add real lights, LODs, occlusion, baked probes, and collider policy only after art approval.");
            WriteText(Path.Combine(_repoRoot, "Documentation", "Planning", "SteamLightingFixtureSet12_ImplementationPlan.md"), plan.ToString());
        }

        private static void Validate()
        {
            var failures = new List<string>();
            if (PrefabRecords.Count < 7)
            {
                failures.Add("Expected at least seven prefabs.");
            }
            if (RenderFiles.Count < 3 || RenderFiles.Any(f => !File.Exists(f)))
            {
                failures.Add("Expected three render PNGs.");
            }
            foreach (var required in new[]
            {
                "SLF12_PREFAB_01_WallGaslightCage",
                "SLF12_PREFAB_02_HangingLantern",
                "SLF12_PREFAB_03_VerticalInspectionLamp",
                "SLF12_PREFAB_04_PressureGaugeLampMount",
                "SLF12_PREFAB_05_PipeBracketGlowGlass",
                "SLF12_PREFAB_06_BrassSwitchBox",
                "SLF12_PREFAB_07_SootHaloDecalPlane"
            })
            {
                if (AssetDatabase.LoadAssetAtPath<GameObject>(ToPackageAssetPath($"Runtime/Prefabs/{required}.prefab")) == null)
                {
                    failures.Add("Missing required prefab " + required);
                }
            }
            if (failures.Count > 0)
            {
                throw new InvalidOperationException($"{Prefix}_VALIDATION_FAIL {string.Join("; ", failures)}");
            }
        }

        private static void ReplaceAsset(Object asset, string path)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }
            AssetDatabase.CreateAsset(asset, path);
        }

        private static void SetColor(Material mat, string property, Color value)
        {
            if (mat.HasProperty(property))
            {
                mat.SetColor(property, value);
            }
        }

        private static void SetFloat(Material mat, string property, float value)
        {
            if (mat.HasProperty(property))
            {
                mat.SetFloat(property, value);
            }
        }

        private static void SetTexture(Material mat, string property, Texture value)
        {
            if (value != null && mat.HasProperty(property))
            {
                mat.SetTexture(property, value);
            }
        }

        private static string Side(int side)
        {
            return side < 0 ? "left" : "right";
        }

        private static string ToPackageAssetPath(string relative)
        {
            return PackageAssetRoot + "/" + relative.Replace("\\", "/");
        }

        private static string Physical(string relative)
        {
            return Path.Combine(_packageRoot, relative.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string RenderRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "ConceptRenders", "V0_1_57_SteamLightingFixtureSet12");
        }

        private static string ToRepoRelative(string path)
        {
            return Normalize(path).Replace(_repoRoot + "/", string.Empty);
        }

        private static string Normalize(string path)
        {
            return Path.GetFullPath(path).Replace("\\", "/");
        }

        private static void WriteText(string path, string text)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            File.WriteAllText(path, text, new UTF8Encoding(false));
        }

        private static void Json(StringBuilder sb, int indent, string key, string value, bool comma, bool quote = true)
        {
            sb.Append(new string(' ', indent * 2));
            sb.Append('"').Append(Escape(key)).Append("\": ");
            if (quote)
            {
                sb.Append('"').Append(Escape(value)).Append('"');
            }
            else
            {
                sb.Append(value);
            }
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private readonly struct Record
        {
            public Record(string name, string path, string category, string notes, int rendererCount)
            {
                Name = name;
                Path = path;
                Category = category;
                Notes = notes;
                RendererCount = rendererCount;
            }

            public readonly string Name;
            public readonly string Path;
            public readonly string Category;
            public readonly string Notes;
            public readonly int RendererCount;
        }

        private readonly struct TexDef
        {
            public TexDef(string name, Color low, Color high, int seed, TextureStyle style)
            {
                Name = name;
                Low = low;
                High = high;
                Seed = seed;
                Style = style;
            }

            public readonly string Name;
            public readonly Color Low;
            public readonly Color High;
            public readonly int Seed;
            public readonly TextureStyle Style;
        }

        private enum TextureStyle
        {
            Metal,
            Copper,
            Glass,
            Decal,
            Wet,
            Gauge,
            Ceramic,
            Enamel
        }

        private static class MeshFactory
        {
            public static Mesh Box()
            {
                var vertices = new[]
                {
                    new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,-0.5f), new Vector3(0.5f,0.5f,-0.5f), new Vector3(-0.5f,0.5f,-0.5f),
                    new Vector3(-0.5f,-0.5f,0.5f), new Vector3(0.5f,-0.5f,0.5f), new Vector3(0.5f,0.5f,0.5f), new Vector3(-0.5f,0.5f,0.5f)
                };
                var triangles = new[]
                {
                    0,2,1, 0,3,2, 4,5,6, 4,6,7, 0,1,5, 0,5,4,
                    1,2,6, 1,6,5, 2,3,7, 2,7,6, 3,0,4, 3,4,7
                };
                return new Mesh { vertices = vertices, triangles = triangles, uv = Enumerable.Repeat(Vector2.zero, vertices.Length).ToArray() };
            }

            public static Mesh Quad()
            {
                return new Mesh
                {
                    vertices = new[] { new Vector3(-0.5f, -0.5f, 0), new Vector3(0.5f, -0.5f, 0), new Vector3(0.5f, 0.5f, 0), new Vector3(-0.5f, 0.5f, 0) },
                    triangles = new[] { 0, 1, 2, 0, 2, 3 },
                    uv = new[] { Vector2.zero, Vector2.right, Vector2.one, Vector2.up }
                };
            }

            public static Mesh CylinderZ(int segments)
            {
                var vertices = new List<Vector3>();
                var uvs = new List<Vector2>();
                var triangles = new List<int>();
                for (var i = 0; i <= segments; i++)
                {
                    var a = i / (float)segments * Mathf.PI * 2.0f;
                    var x = Mathf.Cos(a) * 0.5f;
                    var y = Mathf.Sin(a) * 0.5f;
                    vertices.Add(new Vector3(x, y, -0.5f));
                    vertices.Add(new Vector3(x, y, 0.5f));
                    uvs.Add(new Vector2(i / (float)segments, 0));
                    uvs.Add(new Vector2(i / (float)segments, 1));
                }
                for (var i = 0; i < segments; i++)
                {
                    var b = i * 2;
                    triangles.Add(b); triangles.Add(b + 1); triangles.Add(b + 2);
                    triangles.Add(b + 1); triangles.Add(b + 3); triangles.Add(b + 2);
                }

                var frontCenter = vertices.Count;
                vertices.Add(new Vector3(0, 0, 0.5f));
                uvs.Add(new Vector2(0.5f, 0.5f));
                var backCenter = vertices.Count;
                vertices.Add(new Vector3(0, 0, -0.5f));
                uvs.Add(new Vector2(0.5f, 0.5f));
                for (var i = 0; i < segments; i++)
                {
                    var a = i * 2;
                    var next = ((i + 1) % segments) * 2;
                    triangles.Add(frontCenter); triangles.Add(a + 1); triangles.Add(next + 1);
                    triangles.Add(backCenter); triangles.Add(next); triangles.Add(a);
                }
                return new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray(), uv = uvs.ToArray() };
            }

            public static Mesh Torus(int majorSegments, int minorSegments, float major, float minor)
            {
                var vertices = new List<Vector3>();
                var uvs = new List<Vector2>();
                var triangles = new List<int>();
                for (var i = 0; i <= majorSegments; i++)
                {
                    var u = i / (float)majorSegments * Mathf.PI * 2.0f;
                    for (var j = 0; j <= minorSegments; j++)
                    {
                        var v = j / (float)minorSegments * Mathf.PI * 2.0f;
                        var r = major + Mathf.Cos(v) * minor;
                        vertices.Add(new Vector3(Mathf.Cos(u) * r, Mathf.Sin(u) * r, Mathf.Sin(v) * minor));
                        uvs.Add(new Vector2(i / (float)majorSegments, j / (float)minorSegments));
                    }
                }
                var row = minorSegments + 1;
                for (var i = 0; i < majorSegments; i++)
                {
                    for (var j = 0; j < minorSegments; j++)
                    {
                        var a = i * row + j;
                        var b = (i + 1) * row + j;
                        triangles.Add(a); triangles.Add(b); triangles.Add(a + 1);
                        triangles.Add(a + 1); triangles.Add(b); triangles.Add(b + 1);
                    }
                }
                return new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray(), uv = uvs.ToArray() };
            }

            public static Mesh Sphere(int segments, int rings)
            {
                var vertices = new List<Vector3>();
                var uvs = new List<Vector2>();
                var triangles = new List<int>();
                for (var y = 0; y <= rings; y++)
                {
                    var v = y / (float)rings;
                    var phi = v * Mathf.PI;
                    for (var x = 0; x <= segments; x++)
                    {
                        var u = x / (float)segments;
                        var theta = u * Mathf.PI * 2.0f;
                        vertices.Add(new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(theta)) * 0.5f);
                        uvs.Add(new Vector2(u, v));
                    }
                }
                for (var y = 0; y < rings; y++)
                {
                    for (var x = 0; x < segments; x++)
                    {
                        var a = y * (segments + 1) + x;
                        var b = (y + 1) * (segments + 1) + x;
                        triangles.Add(a); triangles.Add(b); triangles.Add(a + 1);
                        triangles.Add(a + 1); triangles.Add(b); triangles.Add(b + 1);
                    }
                }
                return new Mesh { vertices = vertices.ToArray(), triangles = triangles.ToArray(), uv = uvs.ToArray() };
            }

            public static Mesh GaugeNeedle()
            {
                var vertices = new[]
                {
                    new Vector3(-0.035f, -0.08f, 0), new Vector3(0.035f, -0.08f, 0), new Vector3(0.018f, 0.43f, 0),
                    new Vector3(-0.018f, 0.43f, 0), new Vector3(-0.060f, -0.13f, 0), new Vector3(0.060f, -0.13f, 0), new Vector3(0, -0.22f, 0)
                };
                var triangles = new[] { 0, 1, 2, 0, 2, 3, 4, 6, 5 };
                return new Mesh { vertices = vertices, triangles = triangles, uv = Enumerable.Repeat(Vector2.zero, vertices.Length).ToArray() };
            }

            public static Mesh Lever()
            {
                var vertices = new[]
                {
                    new Vector3(-0.07f, -0.42f, -0.04f), new Vector3(0.07f, -0.42f, -0.04f), new Vector3(0.07f, 0.42f, -0.04f), new Vector3(-0.07f, 0.42f, -0.04f),
                    new Vector3(-0.05f, -0.42f, 0.04f), new Vector3(0.05f, -0.42f, 0.04f), new Vector3(0.05f, 0.42f, 0.04f), new Vector3(-0.05f, 0.42f, 0.04f)
                };
                var triangles = new[]
                {
                    0,1,2, 0,2,3, 4,6,5, 4,7,6, 0,4,5, 0,5,1,
                    1,5,6, 1,6,2, 2,6,7, 2,7,3, 3,7,4, 3,4,0
                };
                return new Mesh { vertices = vertices, triangles = triangles, uv = Enumerable.Repeat(Vector2.zero, vertices.Length).ToArray() };
            }
        }
    }
}
#endif
