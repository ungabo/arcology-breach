#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BrassworksBreach.SteamCorridorDressingHighFidelitySet11.Editor
{
    public static class SCDHF11Generator
    {
        private const string PackageName = "com.brassworks.sidecar.steam-corridor-dressing-high-fidelity-set11";
        private const string PackageAssetRoot = "Packages/" + PackageName;
        private const string Version = "0.1.56-p001";
        private const int TextureSize = 512;
        private const int PreviewWidth = 1536;
        private const int PreviewHeight = 864;

        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly List<AssetRecord> PrefabRecords = new List<AssetRecord>();
        private static readonly List<string> PreviewFiles = new List<string>();
        private static readonly List<string> PackagePreviewFiles = new List<string>();
        private static string _packagePhysicalRoot;
        private static string _repoRoot;

        [MenuItem("Brassworks/Sidecars/Generate SCDHF11")]
        public static void GenerateAll()
        {
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
            RenderPreviews();
            WriteMetadata();
            WriteProductionDocs();
            WriteValidationDocs();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"SCDHF11_GENERATE_PASS prefabs={PrefabRecords.Count} materials={Materials.Count} meshes={Meshes.Count} previews={PreviewFiles.Count}");
        }

        private static void ResolveRoots()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForPackageName(PackageName);
            if (packageInfo == null || string.IsNullOrEmpty(packageInfo.resolvedPath))
            {
                throw new InvalidOperationException("Package root could not be resolved. Confirm the temp Unity project manifest references " + PackageName + ".");
            }

            _packagePhysicalRoot = NormalizePath(packageInfo.resolvedPath);
            var assetPacks = Directory.GetParent(_packagePhysicalRoot);
            if (assetPacks == null || assetPacks.Parent == null)
            {
                throw new InvalidOperationException("Could not derive repository root from package path " + _packagePhysicalRoot);
            }

            _repoRoot = NormalizePath(assetPacks.Parent.FullName);
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
                "Documentation~/QA"
            })
            {
                Directory.CreateDirectory(Physical(relative));
            }

            Directory.CreateDirectory(DocProductionRoot());
            Directory.CreateDirectory(ConceptRenderRoot());
            Directory.CreateDirectory(PlanningRoot());
            Directory.CreateDirectory(QaRoot());
        }

        private static void GenerateTextures()
        {
            var defs = new[]
            {
                new TexDef("AgedBrassDeepPatina", new Color(0.56f, 0.35f, 0.12f), new Color(0.96f, 0.66f, 0.25f), 11, false),
                new TexDef("BurnishedCopperWetEdge", new Color(0.44f, 0.16f, 0.08f), new Color(0.98f, 0.43f, 0.16f), 21, false),
                new TexDef("BlackenedRivetedIron", new Color(0.05f, 0.047f, 0.041f), new Color(0.23f, 0.19f, 0.15f), 31, false),
                new TexDef("OilyGunmetalPipe", new Color(0.035f, 0.04f, 0.045f), new Color(0.30f, 0.28f, 0.25f), 41, false),
                new TexDef("DarkWetStoneBacking", new Color(0.045f, 0.04f, 0.035f), new Color(0.30f, 0.24f, 0.18f), 51, false),
                new TexDef("AmberGaslightGlass", new Color(0.85f, 0.42f, 0.08f), new Color(1.00f, 0.82f, 0.26f), 61, true),
                new TexDef("PaleGaugeFace", new Color(0.62f, 0.55f, 0.43f), new Color(0.92f, 0.84f, 0.66f), 71, false),
                new TexDef("RedValveEnamel", new Color(0.30f, 0.035f, 0.025f), new Color(0.78f, 0.12f, 0.06f), 81, false),
                new TexDef("SootShadowCard", new Color(0.01f, 0.009f, 0.008f), new Color(0.10f, 0.085f, 0.06f), 91, true),
                new TexDef("WetOilStreakCard", new Color(0.018f, 0.014f, 0.011f), new Color(0.24f, 0.17f, 0.09f), 101, true),
                new TexDef("TarnishedBrassEdge", new Color(0.46f, 0.32f, 0.11f), new Color(1.0f, 0.73f, 0.30f), 111, false),
                new TexDef("HeatBluedSteel", new Color(0.05f, 0.06f, 0.075f), new Color(0.32f, 0.24f, 0.18f), 121, false),
                new TexDef("BlackMortarDust", new Color(0.025f, 0.024f, 0.022f), new Color(0.22f, 0.18f, 0.14f), 131, false),
                new TexDef("GlassHighlightCard", new Color(0.75f, 0.92f, 1.00f), new Color(1.00f, 0.90f, 0.58f), 141, true),
                new TexDef("CoalRubberGasket", new Color(0.012f, 0.012f, 0.012f), new Color(0.16f, 0.13f, 0.10f), 151, false),
                new TexDef("CopperSteamGlow", new Color(0.64f, 0.22f, 0.06f), new Color(1.0f, 0.53f, 0.18f), 161, false),
            };

            foreach (var def in defs)
            {
                SaveTexture($"SCDHF11_TEX_{def.Name}_Base.png", BuildBaseTexture(def), "Runtime/Textures");
                SaveTexture($"SCDHF11_TEX_{def.Name}_Normal.png", BuildNormalTexture(def), "Runtime/Textures");
                SaveTexture($"SCDHF11_TEX_{def.Name}_Mask.png", BuildMaskTexture(def), "Runtime/Textures");
            }
        }

        private static Texture2D BuildBaseTexture(TexDef def)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)TextureSize;
                    var fy = y / (float)TextureSize;
                    var n1 = Mathf.PerlinNoise((fx * 9.0f) + def.Seed, (fy * 9.0f) - def.Seed);
                    var n2 = Mathf.PerlinNoise((fx * 37.0f) - def.Seed * 0.37f, (fy * 19.0f) + def.Seed * 0.19f);
                    var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((fx * 72.0f + fy * 18.0f + def.Seed) * 3.14159f)), 26.0f);
                    var grime = Mathf.SmoothStep(0.36f, 0.92f, n1 * 0.68f + n2 * 0.32f);
                    var color = Color.Lerp(def.Low, def.High, grime * 0.78f + scratch * 0.22f);

                    if (def.Transparent)
                    {
                        var radial = Vector2.Distance(new Vector2(fx, fy), new Vector2(0.5f, 0.52f));
                        var streak = Mathf.SmoothStep(0.72f, 0.22f, radial) * (0.35f + n1 * 0.65f);
                        var drip = Mathf.SmoothStep(0.92f, 0.52f, fy) * Mathf.SmoothStep(0.74f, 0.45f, Mathf.Abs(Mathf.Sin((fx * 10.0f + def.Seed) * 3.14159f)));
                        color.a = Mathf.Clamp01(streak * 0.62f + drip * 0.35f);
                    }
                    else
                    {
                        var edgeDarken = Mathf.Clamp01((Mathf.Abs(fx - 0.5f) + Mathf.Abs(fy - 0.5f)) * 0.34f);
                        color *= 1.0f - edgeDarken;
                        color.a = 1.0f;
                    }

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
                    var normal = new Vector3(-hx * 4.5f, -hy * 4.5f, 1.0f).normalized;
                    texture.SetPixel(x, y, new Color(normal.x * 0.5f + 0.5f, normal.y * 0.5f + 0.5f, normal.z * 0.5f + 0.5f, 1.0f));
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
                    var fx = x / (float)TextureSize;
                    var fy = y / (float)TextureSize;
                    var n = Mathf.PerlinNoise(fx * 18.0f + def.Seed * 0.15f, fy * 18.0f - def.Seed * 0.21f);
                    var smooth = def.Transparent ? 0.82f : Mathf.Clamp01(0.34f + n * 0.45f);
                    var occlusion = Mathf.Clamp01(0.55f + n * 0.45f);
                    texture.SetPixel(x, y, new Color(0.75f, occlusion, 0.0f, smooth));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static float Height(int x, int y, int seed)
        {
            var fx = x / (float)TextureSize;
            var fy = y / (float)TextureSize;
            var large = Mathf.PerlinNoise(fx * 10.0f + seed, fy * 10.0f - seed);
            var fine = Mathf.PerlinNoise(fx * 58.0f - seed * 0.27f, fy * 58.0f + seed * 0.13f);
            var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((fx * 86.0f + fy * 21.0f + seed) * 3.14159f)), 30.0f);
            return large * 0.58f + fine * 0.34f + scratch * 0.08f;
        }

        private static void SaveTexture(string fileName, Texture2D texture, string relativeFolder)
        {
            var physical = Path.Combine(Physical(relativeFolder), fileName);
            File.WriteAllBytes(physical, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
        }

        private static void ConfigureTextureImporters()
        {
            AssetDatabase.Refresh();
            foreach (var texturePath in Directory.GetFiles(Physical("Runtime/Textures"), "*.png"))
            {
                var assetPath = ToPackageAssetPath("Runtime/Textures/" + Path.GetFileName(texturePath));
                var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.textureType = assetPath.Contains("_Normal") ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.alphaIsTransparency = assetPath.Contains("Card") || assetPath.Contains("GlassHighlight") || assetPath.Contains("SootShadow") || assetPath.Contains("WetOil");
                importer.sRGBTexture = !assetPath.Contains("_Mask") && !assetPath.Contains("_Normal");
                importer.mipmapEnabled = true;
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.filterMode = FilterMode.Trilinear;
                importer.textureCompression = TextureImporterCompression.CompressedHQ;
                importer.SaveAndReimport();
            }
        }

        private static void CreateMaterials()
        {
            var shader = Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("Standard shader is unavailable in this Unity installation.");
            }

            var defs = new[]
            {
                new MatDef("AgedBrassDeepPatina", 0.92f, 0.55f, false, new Color(0.08f, 0.045f, 0.015f)),
                new MatDef("BurnishedCopperWetEdge", 0.90f, 0.62f, false, new Color(0.10f, 0.035f, 0.015f)),
                new MatDef("BlackenedRivetedIron", 0.88f, 0.36f, false, Color.black),
                new MatDef("OilyGunmetalPipe", 0.86f, 0.68f, false, Color.black),
                new MatDef("DarkWetStoneBacking", 0.10f, 0.50f, false, Color.black),
                new MatDef("AmberGaslightGlass", 0.05f, 0.84f, true, new Color(1.0f, 0.48f, 0.10f) * 1.8f),
                new MatDef("PaleGaugeFace", 0.0f, 0.28f, false, Color.black),
                new MatDef("RedValveEnamel", 0.35f, 0.48f, false, Color.black),
                new MatDef("SootShadowCard", 0.0f, 0.12f, true, Color.black),
                new MatDef("WetOilStreakCard", 0.0f, 0.74f, true, Color.black),
                new MatDef("TarnishedBrassEdge", 0.92f, 0.72f, false, new Color(0.08f, 0.045f, 0.012f)),
                new MatDef("HeatBluedSteel", 0.92f, 0.52f, false, new Color(0.025f, 0.02f, 0.015f)),
                new MatDef("BlackMortarDust", 0.0f, 0.18f, false, Color.black),
                new MatDef("GlassHighlightCard", 0.0f, 0.90f, true, new Color(1.0f, 0.76f, 0.28f) * 1.2f),
                new MatDef("CoalRubberGasket", 0.0f, 0.22f, false, Color.black),
                new MatDef("CopperSteamGlow", 0.75f, 0.78f, false, new Color(0.45f, 0.14f, 0.03f)),
            };

            foreach (var def in defs)
            {
                var mat = new Material(shader)
                {
                    name = "SCDHF11_MAT_" + def.Name
                };
                mat.SetFloat("_Metallic", def.Metallic);
                mat.SetFloat("_Glossiness", def.Smoothness);
                mat.SetTexture("_MainTex", LoadTexture(def.Name, "Base"));
                mat.SetTexture("_BumpMap", LoadTexture(def.Name, "Normal"));
                mat.EnableKeyword("_NORMALMAP");
                mat.SetTexture("_MetallicGlossMap", LoadTexture(def.Name, "Mask"));
                mat.EnableKeyword("_METALLICGLOSSMAP");

                if (def.Emission.maxColorComponent > 0.0f)
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", def.Emission);
                }

                if (def.Transparent)
                {
                    ConfigureTransparent(mat);
                }

                var path = ToPackageAssetPath("Runtime/Materials/" + mat.name + ".mat");
                ReplaceAsset(mat, path);
                Materials[def.Name] = AssetDatabase.LoadAssetAtPath<Material>(path);
            }
        }

        private static Texture2D LoadTexture(string defName, string suffix)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(ToPackageAssetPath($"Runtime/Textures/SCDHF11_TEX_{defName}_{suffix}.png"));
        }

        private static void ConfigureTransparent(Material mat)
        {
            mat.SetFloat("_Mode", 3.0f);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }

        private static void CreateMeshes()
        {
            SaveMesh("CylinderX_32", MeshFactory.CylinderX(32), "Pipe, tank, rod, post, and flange base mesh.");
            SaveMesh("CylinderX_16", MeshFactory.CylinderX(16), "Small bolt, rod, and rivet mesh.");
            SaveMesh("Box", MeshFactory.Box(), "Panel, bracket, rail block, and grate bar mesh.");
            SaveMesh("FlatQuad", MeshFactory.Quad(), "Transparent grime, soot, and wetness card mesh.");
            SaveMesh("Torus_32_8", MeshFactory.Torus(32, 8, 0.38f, 0.055f), "Valve wheel, lamp cage ring, and gauge rim mesh.");
            SaveMesh("Torus_48_10", MeshFactory.Torus(48, 10, 0.50f, 0.045f), "Large circular valve and door threshold accent ring mesh.");
            SaveMesh("QuarterElbow_16_8", MeshFactory.QuarterElbow(16, 8, 0.42f, 0.065f), "Quarter pipe elbow mesh for silhouette breaks.");
            SaveMesh("Sphere_16_8", MeshFactory.Sphere(16, 8), "Rivets, caps, glowing bulbs, and pressure nodes.");
            SaveMesh("GaugeNeedle", MeshFactory.GaugeNeedle(), "Gauge needle mesh.");
            SaveMesh("ChevronPlate", MeshFactory.ChevronPlate(), "Layered valve bank faceplate mesh.");
            SaveMesh("FinnedVentSlat", MeshFactory.FinnedVentSlat(), "Finned wall vent slat mesh.");
            SaveMesh("CurvedHandle", MeshFactory.CurvedHandle(), "Arched grip and handle silhouette mesh.");
            SaveMesh("GrateComb", MeshFactory.GrateComb(), "Dense floor grate comb mesh.");
            SaveMesh("BracketYoke", MeshFactory.BracketYoke(), "Pipe yoke bracket mesh.");
        }

        private static void SaveMesh(string key, Mesh mesh, string description)
        {
            mesh.name = "SCDHF11_MESH_" + key;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
            var path = ToPackageAssetPath("Runtime/Meshes/" + mesh.name + ".asset");
            ReplaceAsset(mesh, path);
            Meshes[key] = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        private static void CreatePrefabs()
        {
            PrefabRecords.Clear();
            SavePrefab(BuildWallPipeRun("SCDHF11_PREFAB_WallPipeRunLayered_A", 5.5f, true, 0.0f), "layered wall pipe run", "Three-depth pipe run with flanges, brackets, rivets, valve wheel, and elbow silhouettes.");
            SavePrefab(BuildWallPipeRun("SCDHF11_PREFAB_WallPipeRunLayered_B", 6.4f, true, 0.42f), "layered wall pipe run", "Longer offset pipe run with extra elbow breaks and alternating copper/brass material rhythm.");
            SavePrefab(BuildWallPipeRun("SCDHF11_PREFAB_WallPipeRunLayered_C", 4.8f, false, -0.36f), "layered wall pipe run", "Compact staggered run for narrow corridor alcoves.");
            SavePrefab(BuildBoilerTankColumn("SCDHF11_PREFAB_BoilerTankColumn_A", 1.0f, true), "boiler tank column", "Tall riveted pressure column with domed nodes, gauge, side pipe, and wet soot cards.");
            SavePrefab(BuildBoilerTankColumn("SCDHF11_PREFAB_BoilerTankColumn_B", 0.82f, false), "boiler tank column", "Slim vertical boiler column variant with copper collar rings.");
            SavePrefab(BuildGaugeCluster("SCDHF11_PREFAB_GaugeCluster_Triple_A", 3), "gauge cluster", "Three pressure dials with individual needles, rims, bracket plate, and tiny steam pipes.");
            SavePrefab(BuildGaugeCluster("SCDHF11_PREFAB_GaugeCluster_Quad_B", 4), "gauge cluster", "Four-dial inspection station with deeper stacked rims.");
            SavePrefab(BuildCagedGaslight("SCDHF11_PREFAB_CagedGaslight_Long_A", 1.7f), "caged gaslight", "Warm amber glass cylinder surrounded by separate ring and rod cage geometry.");
            SavePrefab(BuildCagedGaslight("SCDHF11_PREFAB_CagedGaslight_Short_B", 1.15f), "caged gaslight", "Short wall lamp with thick brass rings and glass highlight cards.");
            SavePrefab(BuildValveBank("SCDHF11_PREFAB_ValveBank_Triple_A", 3), "valve bank", "Three valve wheels mounted over a riveted plate with short pipe stubs.");
            SavePrefab(BuildValveBank("SCDHF11_PREFAB_ValveBank_Quad_B", 4), "valve bank", "Four-wheel valve manifold with red enamel accents.");
            SavePrefab(BuildHandrail("SCDHF11_PREFAB_Handrail_Riveted_A", 5.8f), "handrail", "Riveted brass handrail with posts, wall brackets, and dark shadow backing.");
            SavePrefab(BuildHandrail("SCDHF11_PREFAB_Handrail_Riveted_B", 3.6f), "handrail", "Short handrail module for alcoves and door approaches.");
            SavePrefab(BuildCeilingCluster("SCDHF11_PREFAB_CeilingPipeCluster_A", 5.2f, true), "ceiling pipe cluster", "Multi-depth overhead pipe cluster with elbow drops and yoke brackets.");
            SavePrefab(BuildCeilingCluster("SCDHF11_PREFAB_CeilingPipeCluster_B", 3.8f, false), "ceiling pipe cluster", "Compact overhead service bundle with staggered pipe diameters.");
            SavePrefab(BuildFloorGrate("SCDHF11_PREFAB_FloorDrainGrate_Long_A", 4.5f, 0.9f), "floor drain grate", "Long drainage grate with raised brass rim and dense iron comb bars.");
            SavePrefab(BuildFloorGrate("SCDHF11_PREFAB_FloorDrainGrate_Square_B", 1.8f, 1.8f), "floor drain grate", "Square drain module for room intersections.");
            SavePrefab(BuildDoorTrim("SCDHF11_PREFAB_DoorThresholdTrim_A", 3.5f), "door threshold trim", "Layered threshold trim with copper wear edges, rivets, and central gear-like ring.");
            SavePrefab(BuildDoorTrim("SCDHF11_PREFAB_DoorThresholdTrim_B", 2.6f), "door threshold trim", "Narrow door sill/side trim variant.");
            SavePrefab(BuildDetailCards("SCDHF11_PREFAB_WetSootDetailCards_A"), "wet soot cards", "Transparent soot, oil streak, and glass highlight cards for adding grime over room shells.");
            SavePrefab(BuildCornerPipeJunction("SCDHF11_PREFAB_CornerPipeJunction_A"), "corner pipe junction", "Vertical/horizontal corner transition with elbows, flanges, and clamp brackets.");
            SavePrefab(BuildSteamVent("SCDHF11_PREFAB_WallSteamVentFinned_A"), "wall steam vent", "Finned brass wall vent with layered slats and soot card.");
            SavePrefab(BuildPressureManifold("SCDHF11_PREFAB_PressureManifold_Large_A"), "pressure manifold", "Large pressure manifold mixing tanks, gauge, wheel, pipe branches, and visual depth.");
        }

        private static GameObject BuildWallPipeRun(string name, float length, bool includeWheel, float yOffset)
        {
            var root = NewRoot(name);
            AddBox(root, "dark wet backing plate", "DarkWetStoneBacking", new Vector3(0, yOffset, 0.18f), Vector3.zero, new Vector3(length + 0.55f, 1.42f, 0.08f));
            var pipes = new[]
            {
                new PipeSpec(-0.44f + yOffset, -0.08f, 0.12f, "OilyGunmetalPipe"),
                new PipeSpec(0.00f + yOffset, -0.16f, 0.08f, "BurnishedCopperWetEdge"),
                new PipeSpec(0.38f + yOffset, -0.04f, 0.10f, "AgedBrassDeepPatina"),
            };

            foreach (var pipe in pipes)
            {
                AddCylinder(root, "layer pipe " + pipe.Y, pipe.Mat, new Vector3(0, pipe.Y, pipe.Z), Vector3.zero, new Vector3(length, pipe.Radius * 2.0f, pipe.Radius * 2.0f));
                AddCylinder(root, "left flange " + pipe.Y, "TarnishedBrassEdge", new Vector3(-length * 0.43f, pipe.Y, pipe.Z - 0.004f), Vector3.zero, new Vector3(0.12f, pipe.Radius * 3.05f, pipe.Radius * 3.05f));
                AddCylinder(root, "right flange " + pipe.Y, "TarnishedBrassEdge", new Vector3(length * 0.43f, pipe.Y, pipe.Z - 0.004f), Vector3.zero, new Vector3(0.12f, pipe.Radius * 3.05f, pipe.Radius * 3.05f));
                AddBox(root, "pipe bracket " + pipe.Y, "BlackenedRivetedIron", new Vector3(-length * 0.18f, pipe.Y - 0.17f, 0.11f), Vector3.zero, new Vector3(0.18f, 0.38f, 0.08f));
                AddBox(root, "pipe bracket two " + pipe.Y, "BlackenedRivetedIron", new Vector3(length * 0.20f, pipe.Y - 0.17f, 0.11f), Vector3.zero, new Vector3(0.18f, 0.38f, 0.08f));
            }

            AddElbow(root, "left elbow depth turn", "AgedBrassDeepPatina", new Vector3(-length * 0.50f, 0.38f + yOffset, -0.01f), new Vector3(0, 0, 180), Vector3.one * 1.06f);
            AddElbow(root, "right elbow depth turn", "BurnishedCopperWetEdge", new Vector3(length * 0.50f, -0.44f + yOffset, -0.12f), Vector3.zero, Vector3.one * 0.92f);

            if (includeWheel)
            {
                AddPart(root, "red enamel valve wheel", "Torus_32_8", "RedValveEnamel", new Vector3(length * 0.02f, -0.03f + yOffset, -0.33f), Vector3.zero, Vector3.one * 0.52f);
                AddCylinder(root, "valve stem", "HeatBluedSteel", new Vector3(length * 0.02f, -0.03f + yOffset, -0.20f), new Vector3(0, 90, 0), new Vector3(0.36f, 0.05f, 0.05f));
                AddBox(root, "valve spoke a", "TarnishedBrassEdge", new Vector3(length * 0.02f, -0.03f + yOffset, -0.34f), Vector3.zero, new Vector3(0.86f, 0.035f, 0.035f));
                AddBox(root, "valve spoke b", "TarnishedBrassEdge", new Vector3(length * 0.02f, -0.03f + yOffset, -0.34f), new Vector3(0, 0, 90), new Vector3(0.86f, 0.035f, 0.035f));
            }

            AddRivetLine(root, length, -0.82f + yOffset, 0.11f, 10);
            AddRivetLine(root, length, 0.82f + yOffset, 0.11f, 10);
            return root;
        }

        private static GameObject BuildBoilerTankColumn(string name, float scale, bool heavy)
        {
            var root = NewRoot(name);
            AddCylinder(root, "vertical boiler tank", "AgedBrassDeepPatina", Vector3.zero, new Vector3(0, 0, 90), new Vector3(2.7f * scale, 0.78f * scale, 0.78f * scale));
            AddPart(root, "top dome node", "Sphere_16_8", "TarnishedBrassEdge", new Vector3(0, 1.48f * scale, 0), Vector3.zero, Vector3.one * 0.82f * scale);
            AddPart(root, "bottom dome node", "Sphere_16_8", "BlackenedRivetedIron", new Vector3(0, -1.48f * scale, 0), Vector3.zero, Vector3.one * 0.82f * scale);
            for (var i = -1; i <= 1; i++)
            {
                AddCylinder(root, "collar ring " + i, "BurnishedCopperWetEdge", new Vector3(0, i * 0.74f * scale, 0), new Vector3(0, 0, 90), new Vector3(0.12f * scale, 0.95f * scale, 0.95f * scale));
            }

            AddGauge(root, "tank pressure gauge", new Vector3(0.08f, 0.35f * scale, -0.52f * scale), 0.38f * scale);
            AddCylinder(root, "side riser pipe", "OilyGunmetalPipe", new Vector3(0.62f * scale, 0, -0.18f), new Vector3(0, 0, 90), new Vector3(2.9f * scale, 0.16f * scale, 0.16f * scale));
            AddBox(root, "wall saddle", "BlackenedRivetedIron", new Vector3(0, 0, 0.34f * scale), Vector3.zero, new Vector3(1.10f * scale, 3.0f * scale, 0.13f * scale));
            AddRivetsCircle(root, 12, new Vector3(0, 0, -0.45f * scale), 0.48f * scale, 0.045f * scale, "TarnishedBrassEdge");
            if (heavy)
            {
                AddPart(root, "oil soot card", "FlatQuad", "WetOilStreakCard", new Vector3(-0.15f, -0.38f * scale, -0.61f * scale), Vector3.zero, new Vector3(0.92f * scale, 1.2f * scale, 1));
                AddCylinder(root, "low drain pipe", "HeatBluedSteel", new Vector3(0, -1.15f * scale, -0.54f * scale), Vector3.zero, new Vector3(1.26f * scale, 0.12f * scale, 0.12f * scale));
            }

            return root;
        }

        private static GameObject BuildGaugeCluster(string name, int count)
        {
            var root = NewRoot(name);
            AddBox(root, "back plate", "BlackenedRivetedIron", Vector3.zero, Vector3.zero, new Vector3(count * 0.82f + 0.45f, 1.0f, 0.12f));
            var start = -((count - 1) * 0.43f);
            for (var i = 0; i < count; i++)
            {
                AddGauge(root, "pressure gauge " + (i + 1), new Vector3(start + i * 0.86f, 0.08f * Mathf.Sin(i), -0.14f), 0.33f);
                AddCylinder(root, "gauge feed pipe " + i, i % 2 == 0 ? "BurnishedCopperWetEdge" : "OilyGunmetalPipe", new Vector3(start + i * 0.86f, -0.55f, -0.07f), new Vector3(0, 0, 90), new Vector3(0.64f, 0.06f, 0.06f));
            }

            AddRivetLine(root, count * 0.82f + 0.15f, -0.48f, -0.14f, count + 3);
            AddRivetLine(root, count * 0.82f + 0.15f, 0.52f, -0.14f, count + 3);
            return root;
        }

        private static GameObject BuildCagedGaslight(string name, float height)
        {
            var root = NewRoot(name);
            AddCylinder(root, "amber glass core", "AmberGaslightGlass", Vector3.zero, new Vector3(0, 0, 90), new Vector3(height, 0.34f, 0.34f));
            AddPart(root, "top cage ring", "Torus_32_8", "TarnishedBrassEdge", new Vector3(0, height * 0.47f, 0), new Vector3(90, 0, 0), Vector3.one * 0.52f);
            AddPart(root, "bottom cage ring", "Torus_32_8", "TarnishedBrassEdge", new Vector3(0, -height * 0.47f, 0), new Vector3(90, 0, 0), Vector3.one * 0.52f);
            for (var i = 0; i < 8; i++)
            {
                var angle = i / 8.0f * Mathf.PI * 2.0f;
                var x = Mathf.Cos(angle) * 0.25f;
                var z = Mathf.Sin(angle) * 0.25f;
                AddCylinder(root, "cage rod " + i, "BlackenedRivetedIron", new Vector3(x, 0, z), new Vector3(0, 0, 90), new Vector3(height * 0.98f, 0.035f, 0.035f));
            }

            AddBox(root, "wall mounting boss", "BlackenedRivetedIron", new Vector3(0, 0, 0.36f), Vector3.zero, new Vector3(0.82f, height + 0.28f, 0.16f));
            AddPart(root, "glass highlight", "FlatQuad", "GlassHighlightCard", new Vector3(-0.08f, 0.06f, -0.20f), Vector3.zero, new Vector3(0.34f, height * 0.70f, 1));
            AddPart(root, "warm bulb node", "Sphere_16_8", "CopperSteamGlow", Vector3.zero, Vector3.zero, Vector3.one * 0.25f);
            return root;
        }

        private static GameObject BuildValveBank(string name, int count)
        {
            var root = NewRoot(name);
            AddPart(root, "chevron face plate", "ChevronPlate", "BlackenedRivetedIron", Vector3.zero, Vector3.zero, new Vector3(count * 0.62f, 0.82f, 1.0f));
            var start = -((count - 1) * 0.36f);
            for (var i = 0; i < count; i++)
            {
                var x = start + i * 0.72f;
                AddPart(root, "valve wheel " + i, "Torus_32_8", i % 2 == 0 ? "RedValveEnamel" : "TarnishedBrassEdge", new Vector3(x, 0.02f, -0.22f), Vector3.zero, Vector3.one * 0.38f);
                AddCylinder(root, "valve stem " + i, "HeatBluedSteel", new Vector3(x, 0.02f, -0.08f), new Vector3(0, 90, 0), new Vector3(0.30f, 0.045f, 0.045f));
                AddBox(root, "valve spoke vertical " + i, "AgedBrassDeepPatina", new Vector3(x, 0.02f, -0.23f), new Vector3(0, 0, 90), new Vector3(0.58f, 0.026f, 0.03f));
                AddBox(root, "valve spoke horizontal " + i, "AgedBrassDeepPatina", new Vector3(x, 0.02f, -0.23f), Vector3.zero, new Vector3(0.58f, 0.026f, 0.03f));
                AddCylinder(root, "stub pipe " + i, "OilyGunmetalPipe", new Vector3(x, -0.42f, -0.04f), new Vector3(0, 0, 90), new Vector3(0.56f, 0.08f, 0.08f));
            }

            AddRivetLine(root, count * 0.72f, 0.44f, -0.11f, count + 3);
            AddRivetLine(root, count * 0.72f, -0.44f, -0.11f, count + 3);
            return root;
        }

        private static GameObject BuildHandrail(string name, float length)
        {
            var root = NewRoot(name);
            AddCylinder(root, "main rail", "AgedBrassDeepPatina", new Vector3(0, 0, -0.22f), Vector3.zero, new Vector3(length, 0.12f, 0.12f));
            for (var i = 0; i < 4; i++)
            {
                var x = Mathf.Lerp(-length * 0.43f, length * 0.43f, i / 3.0f);
                AddCylinder(root, "vertical rail post " + i, "TarnishedBrassEdge", new Vector3(x, -0.42f, -0.18f), new Vector3(0, 0, 90), new Vector3(0.84f, 0.08f, 0.08f));
                AddBox(root, "wall bracket " + i, "BlackenedRivetedIron", new Vector3(x, -0.42f, 0.02f), Vector3.zero, new Vector3(0.26f, 0.18f, 0.14f));
            }

            AddBox(root, "shadow backing", "DarkWetStoneBacking", new Vector3(0, -0.43f, 0.10f), Vector3.zero, new Vector3(length + 0.28f, 0.62f, 0.055f));
            AddRivetLine(root, length, -0.68f, -0.02f, 9);
            return root;
        }

        private static GameObject BuildCeilingCluster(string name, float length, bool drops)
        {
            var root = NewRoot(name);
            AddBox(root, "ceiling shadow strip", "DarkWetStoneBacking", new Vector3(0, 0.16f, 0.22f), Vector3.zero, new Vector3(length + 0.4f, 0.58f, 0.08f));
            for (var i = 0; i < 5; i++)
            {
                var y = -0.26f + i * 0.13f;
                var z = -0.16f - i * 0.035f;
                var mat = i % 2 == 0 ? "OilyGunmetalPipe" : "BurnishedCopperWetEdge";
                AddCylinder(root, "ceiling service pipe " + i, mat, new Vector3(0, y, z), Vector3.zero, new Vector3(length - i * 0.36f, 0.08f + i * 0.012f, 0.08f + i * 0.012f));
            }

            for (var i = 0; i < 4; i++)
            {
                var x = Mathf.Lerp(-length * 0.4f, length * 0.4f, i / 3.0f);
                AddPart(root, "pipe yoke " + i, "BracketYoke", "BlackenedRivetedIron", new Vector3(x, -0.20f, -0.18f), Vector3.zero, new Vector3(0.54f, 0.42f, 0.38f));
            }

            if (drops)
            {
                AddElbow(root, "left drop elbow", "AgedBrassDeepPatina", new Vector3(-length * 0.42f, -0.16f, -0.18f), new Vector3(0, 0, 180), Vector3.one * 0.76f);
                AddElbow(root, "right drop elbow", "BurnishedCopperWetEdge", new Vector3(length * 0.38f, -0.28f, -0.18f), Vector3.zero, Vector3.one * 0.70f);
                AddCylinder(root, "vertical drop pipe", "BurnishedCopperWetEdge", new Vector3(length * 0.38f + 0.30f, -0.58f, -0.18f), new Vector3(0, 0, 90), new Vector3(0.85f, 0.11f, 0.11f));
            }

            return root;
        }

        private static GameObject BuildFloorGrate(string name, float width, float depth)
        {
            var root = NewRoot(name);
            AddBox(root, "outer grate rim", "TarnishedBrassEdge", Vector3.zero, Vector3.zero, new Vector3(width, 0.12f, depth));
            AddBox(root, "inner dark well", "BlackMortarDust", new Vector3(0, -0.02f, 0), Vector3.zero, new Vector3(width - 0.32f, 0.09f, depth - 0.32f));
            AddPart(root, "dense grate comb", "GrateComb", "BlackenedRivetedIron", new Vector3(0, 0.05f, 0), Vector3.zero, new Vector3(width - 0.38f, 0.12f, depth - 0.38f));
            var bars = Mathf.Max(5, Mathf.RoundToInt(width * 2.1f));
            for (var i = 0; i < bars; i++)
            {
                var x = Mathf.Lerp(-(width - 0.42f) * 0.5f, (width - 0.42f) * 0.5f, bars == 1 ? 0.5f : i / (float)(bars - 1));
                AddBox(root, "raised grate bar " + i, "OilyGunmetalPipe", new Vector3(x, 0.11f, 0), Vector3.zero, new Vector3(0.055f, 0.08f, depth - 0.42f));
            }

            AddPart(root, "wet sheen card", "FlatQuad", "WetOilStreakCard", new Vector3(0, 0.16f, 0), new Vector3(90, 0, 0), new Vector3(width * 0.92f, depth * 0.82f, 1));
            return root;
        }

        private static GameObject BuildDoorTrim(string name, float width)
        {
            var root = NewRoot(name);
            AddBox(root, "black iron threshold slab", "BlackenedRivetedIron", Vector3.zero, Vector3.zero, new Vector3(width, 0.25f, 0.42f));
            AddBox(root, "front worn brass edge", "TarnishedBrassEdge", new Vector3(0, 0.06f, -0.25f), Vector3.zero, new Vector3(width + 0.15f, 0.14f, 0.12f));
            AddBox(root, "rear copper pressure edge", "BurnishedCopperWetEdge", new Vector3(0, 0.06f, 0.25f), Vector3.zero, new Vector3(width + 0.12f, 0.13f, 0.12f));
            AddPart(root, "central gear accent", "Torus_48_10", "AgedBrassDeepPatina", new Vector3(0, 0.18f, 0), new Vector3(90, 0, 0), Vector3.one * 0.42f);
            AddBox(root, "center wear plate", "HeatBluedSteel", new Vector3(0, 0.17f, 0), Vector3.zero, new Vector3(0.75f, 0.08f, 0.28f));
            AddRivetLine(root, width, 0.19f, -0.19f, 8);
            AddRivetLine(root, width, 0.19f, 0.19f, 8);
            return root;
        }

        private static GameObject BuildDetailCards(string name)
        {
            var root = NewRoot(name);
            AddPart(root, "soot bloom card", "FlatQuad", "SootShadowCard", new Vector3(-0.62f, 0.15f, 0), Vector3.zero, new Vector3(1.3f, 1.0f, 1));
            AddPart(root, "oil streak card", "FlatQuad", "WetOilStreakCard", new Vector3(0.55f, -0.08f, -0.01f), Vector3.zero, new Vector3(0.62f, 1.25f, 1));
            AddPart(root, "glass glint card", "FlatQuad", "GlassHighlightCard", new Vector3(0.05f, 0.46f, -0.02f), Vector3.zero, new Vector3(0.92f, 0.35f, 1));
            return root;
        }

        private static GameObject BuildCornerPipeJunction(string name)
        {
            var root = NewRoot(name);
            AddBox(root, "corner backing plate", "DarkWetStoneBacking", Vector3.zero, Vector3.zero, new Vector3(1.25f, 2.4f, 0.14f));
            AddCylinder(root, "vertical main pipe", "OilyGunmetalPipe", new Vector3(-0.25f, 0, -0.15f), new Vector3(0, 0, 90), new Vector3(2.36f, 0.18f, 0.18f));
            AddCylinder(root, "horizontal branch", "BurnishedCopperWetEdge", new Vector3(0.35f, 0.52f, -0.17f), Vector3.zero, new Vector3(1.38f, 0.13f, 0.13f));
            AddElbow(root, "corner elbow", "AgedBrassDeepPatina", new Vector3(-0.25f, 0.52f, -0.17f), Vector3.zero, Vector3.one * 0.84f);
            AddCylinder(root, "lower flange", "TarnishedBrassEdge", new Vector3(-0.25f, -0.78f, -0.16f), new Vector3(0, 0, 90), new Vector3(0.14f, 0.50f, 0.50f));
            AddGauge(root, "small junction gauge", new Vector3(0.35f, -0.34f, -0.28f), 0.24f);
            AddPart(root, "soot corner card", "FlatQuad", "SootShadowCard", new Vector3(0.18f, 0.10f, -0.31f), Vector3.zero, new Vector3(1.15f, 2.0f, 1));
            return root;
        }

        private static GameObject BuildSteamVent(string name)
        {
            var root = NewRoot(name);
            AddBox(root, "vent backing", "BlackenedRivetedIron", Vector3.zero, Vector3.zero, new Vector3(1.55f, 1.05f, 0.12f));
            for (var i = 0; i < 6; i++)
            {
                AddPart(root, "angled fin " + i, "FinnedVentSlat", i % 2 == 0 ? "TarnishedBrassEdge" : "BurnishedCopperWetEdge", new Vector3(0, -0.36f + i * 0.14f, -0.11f), new Vector3(0, 0, 0), new Vector3(1.18f, 0.14f, 1.0f));
            }

            AddCylinder(root, "side inlet pipe", "OilyGunmetalPipe", new Vector3(-0.98f, 0.0f, -0.04f), Vector3.zero, new Vector3(0.76f, 0.12f, 0.12f));
            AddPart(root, "vent soot bloom", "FlatQuad", "SootShadowCard", new Vector3(0.05f, 0.06f, -0.18f), Vector3.zero, new Vector3(1.42f, 0.96f, 1));
            AddRivetLine(root, 1.35f, 0.45f, -0.13f, 5);
            AddRivetLine(root, 1.35f, -0.45f, -0.13f, 5);
            return root;
        }

        private static GameObject BuildPressureManifold(string name)
        {
            var root = NewRoot(name);
            AddBox(root, "heavy backplate", "BlackenedRivetedIron", Vector3.zero, Vector3.zero, new Vector3(2.4f, 1.8f, 0.14f));
            AddCylinder(root, "left reservoir", "AgedBrassDeepPatina", new Vector3(-0.62f, 0.16f, -0.14f), new Vector3(0, 0, 90), new Vector3(1.42f, 0.36f, 0.36f));
            AddCylinder(root, "right reservoir", "BurnishedCopperWetEdge", new Vector3(0.62f, 0.16f, -0.18f), new Vector3(0, 0, 90), new Vector3(1.18f, 0.28f, 0.28f));
            AddCylinder(root, "cross pipe", "OilyGunmetalPipe", new Vector3(0, -0.55f, -0.16f), Vector3.zero, new Vector3(2.2f, 0.13f, 0.13f));
            AddGauge(root, "large central gauge", new Vector3(0, 0.68f, -0.31f), 0.36f);
            AddPart(root, "right control wheel", "Torus_32_8", "RedValveEnamel", new Vector3(1.05f, -0.42f, -0.32f), Vector3.zero, Vector3.one * 0.36f);
            AddBox(root, "right wheel spoke", "AgedBrassDeepPatina", new Vector3(1.05f, -0.42f, -0.33f), Vector3.zero, new Vector3(0.55f, 0.03f, 0.03f));
            AddBox(root, "right wheel spoke v", "AgedBrassDeepPatina", new Vector3(1.05f, -0.42f, -0.33f), new Vector3(0, 0, 90), new Vector3(0.55f, 0.03f, 0.03f));
            AddPart(root, "oil sheen", "FlatQuad", "WetOilStreakCard", new Vector3(-0.28f, -0.15f, -0.38f), Vector3.zero, new Vector3(1.35f, 1.42f, 1));
            AddRivetsCircle(root, 14, new Vector3(-0.62f, 0.16f, -0.35f), 0.33f, 0.035f, "TarnishedBrassEdge");
            AddRivetLine(root, 2.2f, -0.84f, -0.18f, 9);
            return root;
        }

        private static void AddGauge(GameObject root, string label, Vector3 pos, float radius)
        {
            AddCylinder(root, label + " dark rim", "BlackenedRivetedIron", pos, new Vector3(0, 90, 0), new Vector3(0.08f, radius * 2.35f, radius * 2.35f));
            AddCylinder(root, label + " brass bezel", "TarnishedBrassEdge", pos + new Vector3(0, 0, -0.04f), new Vector3(0, 90, 0), new Vector3(0.055f, radius * 2.08f, radius * 2.08f));
            AddCylinder(root, label + " pale face", "PaleGaugeFace", pos + new Vector3(0, 0, -0.08f), new Vector3(0, 90, 0), new Vector3(0.035f, radius * 1.68f, radius * 1.68f));
            AddPart(root, label + " needle", "GaugeNeedle", "RedValveEnamel", pos + new Vector3(0, 0, -0.115f), new Vector3(0, 0, 24), Vector3.one * radius * 1.30f);
            AddPart(root, label + " glass glint", "FlatQuad", "GlassHighlightCard", pos + new Vector3(-radius * 0.12f, radius * 0.14f, -0.13f), Vector3.zero, new Vector3(radius * 1.0f, radius * 0.34f, 1));
        }

        private static void AddRivetLine(GameObject root, float width, float y, float z, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var x = Mathf.Lerp(-width * 0.45f, width * 0.45f, count == 1 ? 0.5f : i / (float)(count - 1));
                AddPart(root, "rivet " + y + " " + i, "Sphere_16_8", "TarnishedBrassEdge", new Vector3(x, y, z), Vector3.zero, Vector3.one * 0.075f);
            }
        }

        private static void AddRivetsCircle(GameObject root, int count, Vector3 center, float radius, float rivetScale, string mat)
        {
            for (var i = 0; i < count; i++)
            {
                var a = i / (float)count * Mathf.PI * 2.0f;
                AddPart(root, "circle rivet " + i, "Sphere_16_8", mat, center + new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, 0), Vector3.zero, Vector3.one * rivetScale);
            }
        }

        private static GameObject NewRoot(string name)
        {
            var root = new GameObject(name);
            root.transform.position = Vector3.zero;
            root.transform.rotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            return root;
        }

        private static void AddElbow(GameObject root, string label, string mat, Vector3 pos, Vector3 euler, Vector3 scale)
        {
            AddPart(root, label, "QuarterElbow_16_8", mat, pos, euler, scale);
        }

        private static void AddCylinder(GameObject root, string label, string mat, Vector3 pos, Vector3 euler, Vector3 scale)
        {
            AddPart(root, label, "CylinderX_32", mat, pos, euler, scale);
        }

        private static void AddBox(GameObject root, string label, string mat, Vector3 pos, Vector3 euler, Vector3 scale)
        {
            AddPart(root, label, "Box", mat, pos, euler, scale);
        }

        private static GameObject AddPart(GameObject root, string label, string meshKey, string matKey, Vector3 pos, Vector3 euler, Vector3 scale)
        {
            var part = new GameObject(label);
            part.transform.SetParent(root.transform, false);
            part.transform.localPosition = pos;
            part.transform.localRotation = Quaternion.Euler(euler);
            part.transform.localScale = scale;
            part.AddComponent<MeshFilter>().sharedMesh = Meshes[meshKey];
            part.AddComponent<MeshRenderer>().sharedMaterial = Materials[matKey];
            return part;
        }

        private static void SavePrefab(GameObject root, string category, string notes)
        {
            var path = ToPackageAssetPath("Runtime/Prefabs/" + root.name + ".prefab");
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            PrefabUtility.SaveAsPrefabAsset(root, path);
            PrefabRecords.Add(new AssetRecord(root.name, path, category, notes));
            Object.DestroyImmediate(root);
        }

        private static void RenderPreviews()
        {
            PreviewFiles.Clear();
            PackagePreviewFiles.Clear();
            foreach (var record in PrefabRecords)
            {
                var external = Path.Combine(ConceptRenderRoot(), record.Name + "_PREVIEW.png");
                var package = Path.Combine(Physical("Documentation~/Previews"), record.Name + "_PREVIEW.png");
                RenderPrefab(record, external);
                File.Copy(external, package, true);
                PreviewFiles.Add(external);
                PackagePreviewFiles.Add(package);
            }

            BuildContactSheet(Path.Combine(ConceptRenderRoot(), "SCDHF11_PREVIEW_contact-sheet.png"), PreviewFiles);
            BuildContactSheet(Path.Combine(Physical("Documentation~/Previews"), "SCDHF11_PREVIEW_contact-sheet.png"), PreviewFiles);
        }

        private static void RenderPrefab(AssetRecord record, string outputPath)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(record.Path);
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance == null)
            {
                throw new InvalidOperationException("Could not instantiate " + record.Path);
            }

            var bounds = CalculateBounds(instance);
            instance.transform.position -= bounds.center;
            bounds = CalculateBounds(instance);
            var maxExtent = Mathf.Max(0.8f, bounds.extents.magnitude);

            var cameraObject = new GameObject("SCDHF11 Preview Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.018f, 0.017f, 0.015f, 1);
            camera.fieldOfView = 32.0f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 100.0f;
            camera.transform.position = bounds.center + new Vector3(maxExtent * 1.35f, maxExtent * 0.82f, -maxExtent * 2.25f);
            camera.transform.LookAt(bounds.center + new Vector3(0, maxExtent * 0.04f, 0));

            var key = new GameObject("SCDHF11 Warm Key").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.72f, 0.38f);
            key.intensity = 2.25f;
            key.transform.rotation = Quaternion.Euler(44, -32, 0);

            var rim = new GameObject("SCDHF11 Cool Rim").AddComponent<Light>();
            rim.type = LightType.Directional;
            rim.color = new Color(0.35f, 0.55f, 0.70f);
            rim.intensity = 0.75f;
            rim.transform.rotation = Quaternion.Euler(18, 145, 0);

            var glowObject = new GameObject("SCDHF11 Amber Fill");
            var glow = glowObject.AddComponent<Light>();
            glow.type = LightType.Point;
            glow.color = new Color(1.0f, 0.52f, 0.16f);
            glow.intensity = 7.0f;
            glow.range = maxExtent * 3.0f;
            glowObject.transform.position = bounds.center + new Vector3(-maxExtent * 0.35f, maxExtent * 0.45f, -maxExtent * 0.55f);

            RenderSettings.ambientLight = new Color(0.13f, 0.10f, 0.075f);
            var rt = new RenderTexture(PreviewWidth, PreviewHeight, 24, RenderTextureFormat.ARGB32);
            var previous = RenderTexture.active;
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(PreviewWidth, PreviewHeight, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, PreviewWidth, PreviewHeight), 0, 0);
            tex.Apply();
            File.WriteAllBytes(outputPath, tex.EncodeToPNG());
            RenderTexture.active = previous;
            camera.targetTexture = null;
            Object.DestroyImmediate(tex);
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(cameraObject);
            Object.DestroyImmediate(key.gameObject);
            Object.DestroyImmediate(rim.gameObject);
            Object.DestroyImmediate(glowObject);
            Object.DestroyImmediate(instance);
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return new Bounds(Vector3.zero, Vector3.one);
            }

            var bounds = renderers[0].bounds;
            foreach (var renderer in renderers.Skip(1))
            {
                bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }

        private static void BuildContactSheet(string path, List<string> files)
        {
            const int columns = 5;
            const int cellWidth = 384;
            const int cellHeight = 216;
            var rows = Mathf.CeilToInt(files.Count / (float)columns);
            var sheet = new Texture2D(columns * cellWidth, rows * cellHeight, TextureFormat.RGB24, false);
            var background = new Color(0.018f, 0.017f, 0.015f);
            for (var y = 0; y < sheet.height; y++)
            {
                for (var x = 0; x < sheet.width; x++)
                {
                    sheet.SetPixel(x, y, background);
                }
            }

            for (var i = 0; i < files.Count; i++)
            {
                var source = new Texture2D(2, 2, TextureFormat.RGB24, false);
                source.LoadImage(File.ReadAllBytes(files[i]));
                var col = i % columns;
                var row = rows - 1 - (i / columns);
                for (var y = 0; y < cellHeight; y++)
                {
                    for (var x = 0; x < cellWidth; x++)
                    {
                        var color = source.GetPixelBilinear(x / (float)(cellWidth - 1), y / (float)(cellHeight - 1));
                        sheet.SetPixel(col * cellWidth + x, row * cellHeight + y, color);
                    }
                }
                Object.DestroyImmediate(source);
            }

            sheet.Apply(false, false);
            File.WriteAllBytes(path, sheet.EncodeToPNG());
            Object.DestroyImmediate(sheet);
        }

        private static void WriteMetadata()
        {
            var manifest = BuildManifestJson();
            File.WriteAllText(Physical("Runtime/Metadata/SCDHF11_SteamCorridorDressingHighFidelitySet11_Manifest_v0.1.56-p001.json"), manifest);
            File.WriteAllText(Physical("Documentation~/Manifest/SCDHF11_SteamCorridorDressingHighFidelitySet11_Manifest_v0.1.56-p001.json"), manifest);

            var catalog = BuildCatalogJson();
            File.WriteAllText(Physical("Runtime/Metadata/SCDHF11_CorridorDressingCatalog_v0.1.56-p001.json"), catalog);
        }

        private static string BuildManifestJson()
        {
            var materialPaths = Directory.GetFiles(Physical("Runtime/Materials"), "*.mat").Select(ToRepoRelative).OrderBy(x => x).ToList();
            var meshPaths = Directory.GetFiles(Physical("Runtime/Meshes"), "*.asset").Select(ToRepoRelative).OrderBy(x => x).ToList();
            var texturePaths = Directory.GetFiles(Physical("Runtime/Textures"), "*.png").Select(ToRepoRelative).OrderBy(x => x).ToList();
            var prefabPaths = PrefabRecords.Select(x => x.Path).OrderBy(x => x).ToList();
            var previewPaths = PreviewFiles.Select(ToRepoRelative).OrderBy(x => x).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("{");
            AppendJsonValue(sb, 1, "packageName", PackageName, true);
            AppendJsonValue(sb, 1, "displayName", "Steam Corridor Dressing High Fidelity Set 11", true);
            AppendJsonValue(sb, 1, "version", Version, true);
            AppendJsonValue(sb, 1, "status", "isolated-sidecar-candidate", true);
            AppendJsonValue(sb, 1, "strongerThanSet09", "true", true, false);
            AppendJsonValue(sb, 1, "productionRule", "Unity-only; no Blender or external DCC; visual-only prefabs.", true);
            AppendJsonValue(sb, 1, "prefabCount", PrefabRecords.Count.ToString(), true, false);
            AppendJsonValue(sb, 1, "materialCount", Materials.Count.ToString(), true, false);
            AppendJsonValue(sb, 1, "meshCount", Meshes.Count.ToString(), true, false);
            AppendJsonValue(sb, 1, "textureCount", texturePaths.Count.ToString(), true, false);
            AppendJsonValue(sb, 1, "previewCount", PreviewFiles.Count.ToString(), true, false);
            AppendArray(sb, 1, "prefabs", prefabPaths, true);
            AppendArray(sb, 1, "materials", materialPaths, true);
            AppendArray(sb, 1, "meshes", meshPaths, true);
            AppendArray(sb, 1, "textures", texturePaths, true);
            AppendArray(sb, 1, "previews", previewPaths, false);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string BuildCatalogJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            AppendJsonValue(sb, 1, "packageName", PackageName, true);
            AppendJsonValue(sb, 1, "version", Version, true);
            sb.AppendLine("  \"entries\": [");
            for (var i = 0; i < PrefabRecords.Count; i++)
            {
                var r = PrefabRecords[i];
                sb.AppendLine("    {");
                AppendJsonValue(sb, 3, "name", r.Name, true);
                AppendJsonValue(sb, 3, "path", r.Path, true);
                AppendJsonValue(sb, 3, "category", r.Category, true);
                AppendJsonValue(sb, 3, "visualUse", r.Notes, false);
                sb.Append("    }");
                sb.AppendLine(i == PrefabRecords.Count - 1 ? string.Empty : ",");
            }
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void WriteProductionDocs()
        {
            var inventory = new StringBuilder();
            inventory.AppendLine("# Steam Corridor Dressing High Fidelity Set 11 - Asset Inventory");
            inventory.AppendLine();
            inventory.AppendLine("Version: " + Version);
            inventory.AppendLine("Package: `" + PackageName + "`");
            inventory.AppendLine();
            inventory.AppendLine("## Counts");
            inventory.AppendLine("- Prefabs: " + PrefabRecords.Count);
            inventory.AppendLine("- Materials: " + Materials.Count);
            inventory.AppendLine("- Mesh assets: " + Meshes.Count);
            inventory.AppendLine("- Texture maps: " + Directory.GetFiles(Physical("Runtime/Textures"), "*.png").Length);
            inventory.AppendLine("- External preview PNGs: " + PreviewFiles.Count);
            inventory.AppendLine();
            inventory.AppendLine("## Prefabs");
            foreach (var record in PrefabRecords.OrderBy(x => x.Name))
            {
                inventory.AppendLine("- `" + record.Name + "` - " + record.Category + ": " + record.Notes);
            }
            inventory.AppendLine();
            inventory.AppendLine("## Intent");
            inventory.AppendLine("This set is a direct response to the CAML10 finding that the biggest north-star gap is corridor object depth and silhouettes. Set11 concentrates on object families that create readable steampunk density before final hero polish: layered pipe runs, boiler columns, gauge clusters, real 3D cage lamps, valve banks, handrails, ceiling bundles, grate modules, trim, and grime cards.");
            File.WriteAllText(Path.Combine(DocProductionRoot(), "SCDHF11_AssetInventory_0.1.56-p001.md"), inventory.ToString());

            var report = new StringBuilder();
            report.AppendLine("# Steam Corridor Dressing High Fidelity Set 11 - Production Report");
            report.AppendLine();
            report.AppendLine("Built as an isolated Unity package. The main project manifest, generated scenes, Set09 package, and shared status documents were not modified.");
            report.AppendLine();
            report.AppendLine("## Stronger Than Set09");
            report.AppendLine("Yes. Set09 is useful as broad corridor dressing, but Set11 pushes deeper silhouettes through multi-part prefabs, separate flanges, elbows, brackets, cage rods, valve spokes, gauge needles, raised grate bars, transparent grime cards, and warmer metallic material variation.");
            report.AppendLine();
            report.AppendLine("## Remaining North-Star Gaps");
            report.AppendLine("- Still procedural and modular, not hand-sculpted final hero art.");
            report.AppendLine("- No baked global illumination, decal projector workflow, or final room composition integration yet.");
            report.AppendLine("- Materials are convincing at blockout/lookdev distance, but final AAA finish will need authored high-resolution maps, grime placement passes, and level-specific lighting.");
            report.AppendLine("- Prefabs are visual-only; collision, occlusion setup, LODs, batching, and gameplay interaction hooks are intentionally absent.");
            File.WriteAllText(Path.Combine(DocProductionRoot(), "SCDHF11_ProductionReport_0.1.56-p001.md"), report.ToString());
        }

        private static void WriteValidationDocs()
        {
            var forbidden = ValidateForbiddenComponents();
            var textureCount = Directory.GetFiles(Physical("Runtime/Textures"), "*.png").Length;
            var previewCount = PreviewFiles.Count;
            var jsonCount = Directory.GetFiles(Physical("Runtime/Metadata"), "*.json").Length;
            var pass = forbidden.TotalForbidden == 0 && PrefabRecords.Count >= 12 && previewCount >= 12 && Materials.Count >= 12 && Meshes.Count >= 10 && textureCount >= 30 && jsonCount >= 2;

            var qa = new StringBuilder();
            qa.AppendLine("# Steam Corridor Dressing High Fidelity Set 11 - Import Readiness QA");
            qa.AppendLine();
            qa.AppendLine("Result: " + (pass ? "PASS" : "FAIL"));
            qa.AppendLine();
            qa.AppendLine("## Static Validation");
            qa.AppendLine("- Visual-only prefabs checked: " + PrefabRecords.Count);
            qa.AppendLine("- Materials: " + Materials.Count);
            qa.AppendLine("- Mesh assets: " + Meshes.Count);
            qa.AppendLine("- Texture maps: " + textureCount);
            qa.AppendLine("- Runtime metadata JSON files: " + jsonCount);
            qa.AppendLine("- External preview PNGs: " + previewCount);
            qa.AppendLine("- Colliders: " + forbidden.Colliders);
            qa.AppendLine("- Rigidbodies: " + forbidden.Rigidbodies);
            qa.AppendLine("- Lights: " + forbidden.Lights);
            qa.AppendLine("- Cameras: " + forbidden.Cameras);
            qa.AppendLine("- Audio sources: " + forbidden.AudioSources);
            qa.AppendLine("- Animators: " + forbidden.Animators);
            qa.AppendLine("- MonoBehaviours: " + forbidden.MonoBehaviours);
            qa.AppendLine();
            qa.AppendLine("## North-Star Gap Comparison");
            qa.AppendLine("Set11 is materially stronger than Set09 for corridor silhouette richness. It now has 3D lamp cage depth, visible separate pipe brackets and flanges, valve wheel spokes, gauge needles, layered grate bars, and transparent soot/wetness overlays. It still falls short of the north-star image in micro-detail density, true wet brick/stone environment integration, custom sculpted wear, and cinematic room lighting.");
            qa.AppendLine();
            qa.AppendLine("## Import Limitation");
            qa.AppendLine("This package is not yet wired into `Packages/manifest.json` or the playable scenes. It is ready for isolated review and candidate import, but should be integrated only after visual approval.");
            File.WriteAllText(Path.Combine(QaRoot(), "SCDHF11_ValidationReport.md"), qa.ToString());

            var readiness = new StringBuilder();
            readiness.AppendLine("# Steam Corridor Dressing High Fidelity Set 11 - Import Readiness Notes");
            readiness.AppendLine();
            readiness.AppendLine("Recommended next integration path:");
            readiness.AppendLine("1. Import as a file package in a controlled build slice after reviewing the contact sheet and at least 12 individual previews.");
            readiness.AppendLine("2. Keep visual-only prefabs quarantined in showcase placements before converting any object into gameplay or collision geometry.");
            readiness.AppendLine("3. Pair with RoomMaterialSet10 or roomtest-derived room shell materials to test whether the object depth solves the CAML10 corridor assembly weakness.");
            readiness.AppendLine("4. Add LOD/collider/performance work only after the final object families are chosen.");
            File.WriteAllText(Path.Combine(PlanningRoot(), "SCDHF11_ImportReadinessNotes_0.1.56-p001.md"), readiness.ToString());

            var validationJson = new StringBuilder();
            validationJson.AppendLine("{");
            AppendJsonValue(validationJson, 1, "result", pass ? "PASS" : "FAIL", true);
            AppendJsonValue(validationJson, 1, "prefabCount", PrefabRecords.Count.ToString(), true, false);
            AppendJsonValue(validationJson, 1, "materialCount", Materials.Count.ToString(), true, false);
            AppendJsonValue(validationJson, 1, "meshCount", Meshes.Count.ToString(), true, false);
            AppendJsonValue(validationJson, 1, "textureCount", textureCount.ToString(), true, false);
            AppendJsonValue(validationJson, 1, "previewCount", previewCount.ToString(), true, false);
            AppendJsonValue(validationJson, 1, "forbiddenComponentCount", forbidden.TotalForbidden.ToString(), false, false);
            validationJson.AppendLine("}");
            File.WriteAllText(Path.Combine(QaRoot(), "SCDHF11_FileValidationReport_0.1.56-p001.json"), validationJson.ToString());
        }

        private static ForbiddenCounts ValidateForbiddenComponents()
        {
            var counts = new ForbiddenCounts();
            foreach (var record in PrefabRecords)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(record.Path);
                counts.Colliders += prefab.GetComponentsInChildren<Collider>(true).Length;
                counts.Rigidbodies += prefab.GetComponentsInChildren<Rigidbody>(true).Length;
                counts.Lights += prefab.GetComponentsInChildren<Light>(true).Length;
                counts.Cameras += prefab.GetComponentsInChildren<Camera>(true).Length;
                counts.AudioSources += prefab.GetComponentsInChildren<AudioSource>(true).Length;
                counts.Animators += prefab.GetComponentsInChildren<Animator>(true).Length;
                counts.MonoBehaviours += prefab.GetComponentsInChildren<MonoBehaviour>(true).Length;
            }
            return counts;
        }

        private static void ReplaceAsset(Object asset, string path)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            AssetDatabase.CreateAsset(asset, path);
        }

        private static void AppendJsonValue(StringBuilder sb, int indent, string key, string value, bool comma, bool quoteValue = true)
        {
            sb.Append(new string(' ', indent * 2));
            sb.Append('"').Append(EscapeJson(key)).Append("\": ");
            if (quoteValue)
            {
                sb.Append('"').Append(EscapeJson(value)).Append('"');
            }
            else
            {
                sb.Append(value);
            }
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static void AppendArray(StringBuilder sb, int indent, string key, List<string> values, bool comma)
        {
            sb.Append(new string(' ', indent * 2));
            sb.Append('"').Append(EscapeJson(key)).AppendLine("\": [");
            for (var i = 0; i < values.Count; i++)
            {
                sb.Append(new string(' ', (indent + 1) * 2));
                sb.Append('"').Append(EscapeJson(values[i].Replace("\\", "/"))).Append('"');
                sb.AppendLine(i == values.Count - 1 ? string.Empty : ",");
            }
            sb.Append(new string(' ', indent * 2));
            sb.Append(']');
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string ToPackageAssetPath(string relative)
        {
            return PackageAssetRoot + "/" + relative.Replace("\\", "/");
        }

        private static string Physical(string relative)
        {
            return Path.Combine(_packagePhysicalRoot, relative.Replace('/', Path.DirectorySeparatorChar));
        }

        private static string DocProductionRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "AssetProduction", "V0_1_56_SteamCorridorDressingHighFidelitySet11");
        }

        private static string ConceptRenderRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "ConceptRenders", "V0_1_56_SteamCorridorDressingHighFidelitySet11");
        }

        private static string PlanningRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "Planning", "V0_1_56_SteamCorridorDressingHighFidelitySet11ImportReadiness");
        }

        private static string QaRoot()
        {
            return Path.Combine(_repoRoot, "Documentation", "QA", "V0_1_56_SteamCorridorDressingHighFidelitySet11ImportReadiness");
        }

        private static string ToRepoRelative(string path)
        {
            return NormalizePath(path).Replace(_repoRoot + "/", string.Empty);
        }

        private static string NormalizePath(string path)
        {
            return Path.GetFullPath(path).Replace("\\", "/");
        }

        private readonly struct TexDef
        {
            public TexDef(string name, Color low, Color high, int seed, bool transparent)
            {
                Name = name;
                Low = low;
                High = high;
                Seed = seed;
                Transparent = transparent;
            }

            public readonly string Name;
            public readonly Color Low;
            public readonly Color High;
            public readonly int Seed;
            public readonly bool Transparent;
        }

        private readonly struct MatDef
        {
            public MatDef(string name, float metallic, float smoothness, bool transparent, Color emission)
            {
                Name = name;
                Metallic = metallic;
                Smoothness = smoothness;
                Transparent = transparent;
                Emission = emission;
            }

            public readonly string Name;
            public readonly float Metallic;
            public readonly float Smoothness;
            public readonly bool Transparent;
            public readonly Color Emission;
        }

        private readonly struct PipeSpec
        {
            public PipeSpec(float y, float z, float radius, string mat)
            {
                Y = y;
                Z = z;
                Radius = radius;
                Mat = mat;
            }

            public readonly float Y;
            public readonly float Z;
            public readonly float Radius;
            public readonly string Mat;
        }

        private readonly struct AssetRecord
        {
            public AssetRecord(string name, string path, string category, string notes)
            {
                Name = name;
                Path = path;
                Category = category;
                Notes = notes;
            }

            public readonly string Name;
            public readonly string Path;
            public readonly string Category;
            public readonly string Notes;
        }

        private struct ForbiddenCounts
        {
            public int Colliders;
            public int Rigidbodies;
            public int Lights;
            public int Cameras;
            public int AudioSources;
            public int Animators;
            public int MonoBehaviours;

            public int TotalForbidden => Colliders + Rigidbodies + Lights + Cameras + AudioSources + Animators + MonoBehaviours;
        }

        private static class MeshFactory
        {
            public static Mesh Box()
            {
                var v = new[]
                {
                    new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,-0.5f), new Vector3(0.5f,0.5f,-0.5f), new Vector3(-0.5f,0.5f,-0.5f),
                    new Vector3(-0.5f,-0.5f,0.5f), new Vector3(0.5f,-0.5f,0.5f), new Vector3(0.5f,0.5f,0.5f), new Vector3(-0.5f,0.5f,0.5f)
                };
                var t = new[]
                {
                    0,2,1, 0,3,2, 4,5,6, 4,6,7, 0,1,5, 0,5,4,
                    1,2,6, 1,6,5, 2,3,7, 2,7,6, 3,0,4, 3,4,7
                };
                return new Mesh { vertices = v, triangles = t, uv = Enumerable.Repeat(Vector2.zero, v.Length).ToArray() };
            }

            public static Mesh Quad()
            {
                return new Mesh
                {
                    vertices = new[] { new Vector3(-0.5f, -0.5f, 0), new Vector3(0.5f, -0.5f, 0), new Vector3(0.5f, 0.5f, 0), new Vector3(-0.5f, 0.5f, 0) },
                    triangles = new[] { 0, 1, 2, 0, 2, 3 },
                    uv = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) }
                };
            }

            public static Mesh CylinderX(int segments)
            {
                var vertices = new List<Vector3>();
                var uv = new List<Vector2>();
                var tris = new List<int>();
                for (var i = 0; i <= segments; i++)
                {
                    var a = i / (float)segments * Mathf.PI * 2.0f;
                    var y = Mathf.Cos(a) * 0.5f;
                    var z = Mathf.Sin(a) * 0.5f;
                    vertices.Add(new Vector3(-0.5f, y, z));
                    vertices.Add(new Vector3(0.5f, y, z));
                    uv.Add(new Vector2(i / (float)segments, 0));
                    uv.Add(new Vector2(i / (float)segments, 1));
                }
                for (var i = 0; i < segments; i++)
                {
                    var b = i * 2;
                    tris.Add(b); tris.Add(b + 2); tris.Add(b + 1);
                    tris.Add(b + 1); tris.Add(b + 2); tris.Add(b + 3);
                }

                var leftCenter = vertices.Count;
                vertices.Add(new Vector3(-0.5f, 0, 0));
                uv.Add(new Vector2(0.5f, 0.5f));
                var rightCenter = vertices.Count;
                vertices.Add(new Vector3(0.5f, 0, 0));
                uv.Add(new Vector2(0.5f, 0.5f));
                for (var i = 0; i < segments; i++)
                {
                    var a = i * 2;
                    var b = ((i + 1) % segments) * 2;
                    tris.Add(leftCenter); tris.Add(a); tris.Add(b);
                    tris.Add(rightCenter); tris.Add(b + 1); tris.Add(a + 1);
                }

                return new Mesh { vertices = vertices.ToArray(), triangles = tris.ToArray(), uv = uv.ToArray() };
            }

            public static Mesh Torus(int majorSegments, int minorSegments, float major, float minor)
            {
                var vertices = new List<Vector3>();
                var uv = new List<Vector2>();
                var tris = new List<int>();
                for (var i = 0; i <= majorSegments; i++)
                {
                    var u = i / (float)majorSegments * Mathf.PI * 2.0f;
                    for (var j = 0; j <= minorSegments; j++)
                    {
                        var v = j / (float)minorSegments * Mathf.PI * 2.0f;
                        var r = major + minor * Mathf.Cos(v);
                        vertices.Add(new Vector3(r * Mathf.Cos(u), r * Mathf.Sin(u), minor * Mathf.Sin(v)));
                        uv.Add(new Vector2(i / (float)majorSegments, j / (float)minorSegments));
                    }
                }
                for (var i = 0; i < majorSegments; i++)
                {
                    for (var j = 0; j < minorSegments; j++)
                    {
                        var a = i * (minorSegments + 1) + j;
                        var b = (i + 1) * (minorSegments + 1) + j;
                        tris.Add(a); tris.Add(b); tris.Add(a + 1);
                        tris.Add(a + 1); tris.Add(b); tris.Add(b + 1);
                    }
                }
                return new Mesh { vertices = vertices.ToArray(), triangles = tris.ToArray(), uv = uv.ToArray() };
            }

            public static Mesh QuarterElbow(int pathSegments, int tubeSegments, float major, float minor)
            {
                var vertices = new List<Vector3>();
                var uv = new List<Vector2>();
                var tris = new List<int>();
                for (var i = 0; i <= pathSegments; i++)
                {
                    var u = i / (float)pathSegments * Mathf.PI * 0.5f;
                    var center = new Vector3(Mathf.Cos(u) * major, Mathf.Sin(u) * major, 0);
                    var normal = new Vector3(Mathf.Cos(u), Mathf.Sin(u), 0);
                    var binormal = Vector3.forward;
                    for (var j = 0; j <= tubeSegments; j++)
                    {
                        var v = j / (float)tubeSegments * Mathf.PI * 2.0f;
                        vertices.Add(center + normal * (Mathf.Cos(v) * minor) + binormal * (Mathf.Sin(v) * minor));
                        uv.Add(new Vector2(i / (float)pathSegments, j / (float)tubeSegments));
                    }
                }
                for (var i = 0; i < pathSegments; i++)
                {
                    for (var j = 0; j < tubeSegments; j++)
                    {
                        var a = i * (tubeSegments + 1) + j;
                        var b = (i + 1) * (tubeSegments + 1) + j;
                        tris.Add(a); tris.Add(b); tris.Add(a + 1);
                        tris.Add(a + 1); tris.Add(b); tris.Add(b + 1);
                    }
                }
                return new Mesh { vertices = vertices.ToArray(), triangles = tris.ToArray(), uv = uv.ToArray() };
            }

            public static Mesh Sphere(int segments, int rings)
            {
                var vertices = new List<Vector3>();
                var uv = new List<Vector2>();
                var tris = new List<int>();
                for (var y = 0; y <= rings; y++)
                {
                    var v = y / (float)rings;
                    var phi = v * Mathf.PI;
                    for (var x = 0; x <= segments; x++)
                    {
                        var u = x / (float)segments;
                        var theta = u * Mathf.PI * 2.0f;
                        vertices.Add(new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(theta)) * 0.5f);
                        uv.Add(new Vector2(u, v));
                    }
                }
                for (var y = 0; y < rings; y++)
                {
                    for (var x = 0; x < segments; x++)
                    {
                        var a = y * (segments + 1) + x;
                        var b = (y + 1) * (segments + 1) + x;
                        tris.Add(a); tris.Add(b); tris.Add(a + 1);
                        tris.Add(a + 1); tris.Add(b); tris.Add(b + 1);
                    }
                }
                return new Mesh { vertices = vertices.ToArray(), triangles = tris.ToArray(), uv = uv.ToArray() };
            }

            public static Mesh GaugeNeedle()
            {
                return new Mesh
                {
                    vertices = new[] { new Vector3(-0.035f, -0.06f, 0), new Vector3(0.035f, -0.06f, 0), new Vector3(0.0f, 0.46f, 0), new Vector3(-0.06f, -0.08f, 0), new Vector3(0.06f, -0.08f, 0), new Vector3(0, -0.16f, 0) },
                    triangles = new[] { 0, 1, 2, 3, 5, 4 },
                    uv = new[] { Vector2.zero, Vector2.right, Vector2.up, Vector2.zero, Vector2.right, Vector2.up }
                };
            }

            public static Mesh ChevronPlate()
            {
                return new Mesh
                {
                    vertices = new[]
                    {
                        new Vector3(-0.5f,-0.36f,0), new Vector3(0.5f,-0.36f,0), new Vector3(0.5f,0.24f,0),
                        new Vector3(0.22f,0.42f,0), new Vector3(-0.22f,0.42f,0), new Vector3(-0.5f,0.24f,0)
                    },
                    triangles = new[] { 0,1,2, 0,2,5, 5,2,3, 5,3,4 },
                    uv = new[] { Vector2.zero, Vector2.right, Vector2.one, new Vector2(0.72f,1), new Vector2(0.28f,1), Vector2.up }
                };
            }

            public static Mesh FinnedVentSlat()
            {
                return new Mesh
                {
                    vertices = new[] { new Vector3(-0.5f,-0.08f,0.06f), new Vector3(0.5f,-0.08f,0.06f), new Vector3(0.5f,0.08f,-0.06f), new Vector3(-0.5f,0.08f,-0.06f) },
                    triangles = new[] { 0,1,2, 0,2,3 },
                    uv = new[] { Vector2.zero, Vector2.right, Vector2.one, Vector2.up }
                };
            }

            public static Mesh CurvedHandle()
            {
                return Torus(24, 6, 0.36f, 0.035f);
            }

            public static Mesh GrateComb()
            {
                var vertices = new List<Vector3>();
                var tris = new List<int>();
                for (var i = 0; i < 10; i++)
                {
                    var x = -0.45f + i * 0.10f;
                    var baseIndex = vertices.Count;
                    vertices.Add(new Vector3(x, 0, -0.5f));
                    vertices.Add(new Vector3(x + 0.035f, 0, -0.5f));
                    vertices.Add(new Vector3(x + 0.035f, 0, 0.5f));
                    vertices.Add(new Vector3(x, 0, 0.5f));
                    tris.Add(baseIndex); tris.Add(baseIndex + 1); tris.Add(baseIndex + 2);
                    tris.Add(baseIndex); tris.Add(baseIndex + 2); tris.Add(baseIndex + 3);
                }
                return new Mesh { vertices = vertices.ToArray(), triangles = tris.ToArray(), uv = Enumerable.Repeat(Vector2.zero, vertices.Count).ToArray() };
            }

            public static Mesh BracketYoke()
            {
                return new Mesh
                {
                    vertices = new[]
                    {
                        new Vector3(-0.5f,-0.5f,0), new Vector3(-0.34f,-0.5f,0), new Vector3(-0.34f,0.26f,0), new Vector3(0.34f,0.26f,0),
                        new Vector3(0.34f,-0.5f,0), new Vector3(0.5f,-0.5f,0), new Vector3(0.5f,0.48f,0), new Vector3(-0.5f,0.48f,0)
                    },
                    triangles = new[] { 0,1,2, 0,2,7, 7,2,3, 7,3,6, 3,4,5, 3,5,6 },
                    uv = Enumerable.Repeat(Vector2.zero, 8).ToArray()
                };
            }
        }
    }
}
#endif
