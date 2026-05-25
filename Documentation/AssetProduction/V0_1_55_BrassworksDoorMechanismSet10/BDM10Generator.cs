using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace BrassworksBreach.AssetProduction
{
    public static class BDM10Generator
    {
        private const string PackId = "BDM10";
        private const string VersionFull = "0.1.55-p001";
        private const string PackageName = "com.brassworks.sidecar.brassworks-door-mechanism-set10";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string RuntimeRoot = PackageRoot + "/Runtime";
        private const string MaterialRoot = RuntimeRoot + "/Materials";
        private const string MeshRoot = RuntimeRoot + "/Meshes";
        private const string TextureRoot = RuntimeRoot + "/Textures";
        private const string PrefabRoot = RuntimeRoot + "/Prefabs";
        private const string MetadataRoot = RuntimeRoot + "/Metadata";
        private const string ManifestRoot = PackageRoot + "/Documentation~/Manifest";
        private const string PackagePreviewRoot = PackageRoot + "/Documentation~/Previews";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_55_BrassworksDoorMechanismSet10";
        private const string ProductionDocFolder = "Documentation/AssetProduction/V0_1_55_BrassworksDoorMechanismSet10";
        private const string PlanningDocFolder = "Documentation/Planning/V0_1_55_BrassworksDoorMechanismSet10ImportReadiness";
        private const string QaDocFolder = "Documentation/QA/V0_1_55_BrassworksDoorMechanismSet10ImportReadiness";
        private const int PreviewWidth = 1400;
        private const int PreviewHeight = 900;

        private static readonly List<MaterialRecord> MaterialRecords = new List<MaterialRecord>();
        private static readonly List<MeshRecord> MeshRecords = new List<MeshRecord>();
        private static readonly List<TextureRecord> TextureRecords = new List<TextureRecord>();
        private static readonly List<PrefabRecord> PrefabRecords = new List<PrefabRecord>();
        private static readonly List<string> PreviewRelativePaths = new List<string>();

        private enum TextureStyle
        {
            AgedBrass,
            PolishedBrass,
            Copper,
            Iron,
            Gunmetal,
            Amber,
            Ivory,
            RedPaint,
            Rubber,
            Verdigris,
            Soot,
            Oil
        }

        private readonly struct MaterialSpec
        {
            public MaterialSpec(string key, string assetName, Color color, float metallic, float smoothness, Color emission, float alpha, TextureStyle style, bool normal)
            {
                Key = key;
                AssetName = assetName;
                Color = color;
                Metallic = metallic;
                Smoothness = smoothness;
                Emission = emission;
                Alpha = alpha;
                Style = style;
                HasNormal = normal;
            }

            public readonly string Key;
            public readonly string AssetName;
            public readonly Color Color;
            public readonly float Metallic;
            public readonly float Smoothness;
            public readonly Color Emission;
            public readonly float Alpha;
            public readonly TextureStyle Style;
            public readonly bool HasNormal;
        }

        private static readonly MaterialSpec[] MaterialSpecs =
        {
            new MaterialSpec("agedBrass", "BDM10_MAT_AgedBrassPatina", C(0.61f, 0.39f, 0.16f), 0.88f, 0.46f, Color.black, 1f, TextureStyle.AgedBrass, true),
            new MaterialSpec("edgeBrass", "BDM10_MAT_PolishedEdgeBrass", C(0.93f, 0.66f, 0.27f), 0.90f, 0.62f, Color.black, 1f, TextureStyle.PolishedBrass, true),
            new MaterialSpec("copper", "BDM10_MAT_HeatStainedCopper", C(0.69f, 0.30f, 0.13f), 0.82f, 0.42f, Color.black, 1f, TextureStyle.Copper, true),
            new MaterialSpec("iron", "BDM10_MAT_BlackenedOilyIron", C(0.055f, 0.050f, 0.044f), 0.86f, 0.34f, Color.black, 1f, TextureStyle.Iron, true),
            new MaterialSpec("gunmetal", "BDM10_MAT_GunmetalScratchedPlate", C(0.22f, 0.22f, 0.20f), 0.78f, 0.30f, Color.black, 1f, TextureStyle.Gunmetal, true),
            new MaterialSpec("amber", "BDM10_MAT_AmberPressureGlass", C(1.0f, 0.50f, 0.10f), 0.02f, 0.78f, C(1.35f, 0.58f, 0.12f), 0.74f, TextureStyle.Amber, false),
            new MaterialSpec("ivory", "BDM10_MAT_IvoryGaugeEnamel", C(0.81f, 0.75f, 0.58f), 0.02f, 0.52f, Color.black, 1f, TextureStyle.Ivory, false),
            new MaterialSpec("red", "BDM10_MAT_RedPressurePaint", C(0.52f, 0.045f, 0.035f), 0.10f, 0.35f, Color.black, 1f, TextureStyle.RedPaint, false),
            new MaterialSpec("rubber", "BDM10_MAT_DarkRubberGasket", C(0.035f, 0.030f, 0.026f), 0.03f, 0.22f, Color.black, 1f, TextureStyle.Rubber, true),
            new MaterialSpec("verdigris", "BDM10_MAT_VerdigrisOxide", C(0.13f, 0.43f, 0.37f), 0.52f, 0.28f, Color.black, 1f, TextureStyle.Verdigris, false),
            new MaterialSpec("soot", "BDM10_MAT_WarmSootGrime", C(0.075f, 0.063f, 0.049f), 0.16f, 0.18f, Color.black, 1f, TextureStyle.Soot, false),
            new MaterialSpec("oil", "BDM10_MAT_WetOilSheen", C(0.018f, 0.016f, 0.013f), 0.18f, 0.82f, Color.black, 0.88f, TextureStyle.Oil, false)
        };

        [MenuItem("Brassworks Breach/Sidecars/Brassworks Door Mechanism Set 10/Generate")]
        public static void GenerateAll()
        {
            MaterialRecords.Clear();
            MeshRecords.Clear();
            TextureRecords.Clear();
            PrefabRecords.Clear();
            PreviewRelativePaths.Clear();

            EnsurePackageFolders();
            string repoRoot = RepoRoot();
            string renderRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            string productionRoot = ResolveRepoRelativeFolder(ProductionDocFolder);
            string planningRoot = ResolveRepoRelativeFolder(PlanningDocFolder);
            string qaRoot = ResolveRepoRelativeFolder(QaDocFolder);
            Directory.CreateDirectory(renderRoot);
            Directory.CreateDirectory(productionRoot);
            Directory.CreateDirectory(planningRoot);
            Directory.CreateDirectory(qaRoot);

            Dictionary<string, Texture2D> textures = CreateTextures();
            Dictionary<string, Material> materials = CreateMaterials(textures);
            Dictionary<string, Mesh> meshes = CreateMeshes();

            CreateGearHub(meshes, materials);
            CreateLockingBarPair(meshes, materials);
            CreatePistonBrace(meshes, materials);
            CreateRivetedHinge(meshes, materials);
            CreatePressureWheel(meshes, materials);
            CreateBoltCollar(meshes, materials);
            CreateTrackRail(meshes, materials);
            CreateAmberLampCapsule(meshes, materials);
            CreateGaugeValveManifold(meshes, materials);
            CreateCrossBoltLatch(meshes, materials);
            CreateGearRackDrive(meshes, materials);
            CreatePressureLockModule(meshes, materials);
            CreateSteamReleaseLever(meshes, materials);
            CreatePalettePreview(meshes, materials);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RenderPreviews(renderRoot);
            RenderContactSheet(renderRoot);
            ValidationResult validation = ValidateGeneratedContent(renderRoot);
            string manifest = BuildManifest(validation);
            WriteManifestCopies(manifest);
            WriteDocumentation(repoRoot, productionRoot, planningRoot, qaRoot, renderRoot, validation);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (!validation.Passed)
            {
                throw new InvalidOperationException("BDM10 validation failed: " + string.Join("; ", validation.Failures.ToArray()));
            }

            Debug.Log("BDM10_GENERATE_PASS prefabs=" + validation.PrefabCount.ToString(CultureInfo.InvariantCulture)
                + " materials=" + validation.MaterialCount.ToString(CultureInfo.InvariantCulture)
                + " textures=" + validation.TextureCount.ToString(CultureInfo.InvariantCulture)
                + " meshes=" + validation.MeshCount.ToString(CultureInfo.InvariantCulture)
                + " previews=" + validation.PreviewCount.ToString(CultureInfo.InvariantCulture));
        }

        private static Dictionary<string, Texture2D> CreateTextures()
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>(StringComparer.Ordinal);
            string packagePhysicalRoot = PackagePhysicalRoot();
            foreach (MaterialSpec spec in MaterialSpecs)
            {
                string albedoName = spec.AssetName.Replace("_MAT_", "_TEX_") + "_Albedo";
                string metalName = spec.AssetName.Replace("_MAT_", "_TEX_") + "_MetallicSmoothness";
                Texture2D albedo = BuildTexture(spec, 512, 512, false, false);
                Texture2D metal = BuildTexture(spec, 512, 512, true, false);
                SaveTexture(packagePhysicalRoot, albedoName, albedo, false, textures, spec.Key + "_albedo", spec.Style + " albedo");
                SaveTexture(packagePhysicalRoot, metalName, metal, false, textures, spec.Key + "_metal", spec.Style + " metallic/smoothness");
                if (spec.HasNormal)
                {
                    string normalName = spec.AssetName.Replace("_MAT_", "_TEX_") + "_Normal";
                    Texture2D normal = BuildTexture(spec, 512, 512, false, true);
                    SaveTexture(packagePhysicalRoot, normalName, normal, true, textures, spec.Key + "_normal", spec.Style + " normal");
                }
            }

            return textures;
        }

        private static void SaveTexture(string packagePhysicalRoot, string assetName, Texture2D texture, bool normalMap, IDictionary<string, Texture2D> textures, string key, string tag)
        {
            string absolutePath = Path.Combine(packagePhysicalRoot, "Runtime", "Textures", assetName + ".png");
            File.WriteAllBytes(absolutePath, ImageConversion.EncodeToPNG(texture));
            Object.DestroyImmediate(texture);
            string assetPath = TextureRoot + "/" + assetName + ".png";
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = normalMap ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.mipmapEnabled = true;
                importer.sRGBTexture = !normalMap;
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.filterMode = FilterMode.Trilinear;
                importer.SaveAndReimport();
            }

            Texture2D imported = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            textures[key] = imported;
            TextureRecords.Add(new TextureRecord { Path = "Runtime/Textures/" + assetName + ".png", Tag = tag });
        }

        private static Dictionary<string, Material> CreateMaterials(IReadOnlyDictionary<string, Texture2D> textures)
        {
            Dictionary<string, Material> result = new Dictionary<string, Material>(StringComparer.Ordinal);
            Shader shader = Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Sprites/Default");
            foreach (MaterialSpec spec in MaterialSpecs)
            {
                string path = MaterialRoot + "/" + spec.AssetName + ".mat";
                Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material == null)
                {
                    material = new Material(shader);
                    AssetDatabase.CreateAsset(material, path);
                }
                else
                {
                    material.shader = shader;
                }

                Color baseColor = spec.Color;
                baseColor.a = spec.Alpha;
                SetColor(material, "_Color", baseColor);
                SetColor(material, "_BaseColor", baseColor);
                SetFloat(material, "_Metallic", spec.Metallic);
                SetFloat(material, "_Smoothness", spec.Smoothness);
                SetFloat(material, "_Glossiness", spec.Smoothness);
                SetTexture(material, "_MainTex", textures[spec.Key + "_albedo"]);
                SetTexture(material, "_BaseMap", textures[spec.Key + "_albedo"]);
                SetTexture(material, "_MetallicGlossMap", textures[spec.Key + "_metal"]);
                if (textures.ContainsKey(spec.Key + "_normal"))
                {
                    SetTexture(material, "_BumpMap", textures[spec.Key + "_normal"]);
                    material.EnableKeyword("_NORMALMAP");
                }

                if (spec.Emission.maxColorComponent > 0f)
                {
                    SetColor(material, "_EmissionColor", spec.Emission);
                    material.EnableKeyword("_EMISSION");
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                }

                if (spec.Alpha < 0.99f)
                {
                    SetFloat(material, "_Mode", 3f);
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.renderQueue = (int)RenderQueue.Transparent;
                }

                EditorUtility.SetDirty(material);
                result[spec.Key] = material;
                MaterialRecords.Add(new MaterialRecord { Path = "Runtime/Materials/" + spec.AssetName + ".mat", Tag = spec.Style.ToString() });
            }

            return result;
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>(StringComparer.Ordinal);
            AddMesh(meshes, "box", "BDM10_MESH_BoxUnit", BuildBoxMesh(1f, 1f, 1f));
            AddMesh(meshes, "bar", "BDM10_MESH_BarLongUnit", BuildBoxMesh(4f, 0.22f, 0.22f));
            AddMesh(meshes, "cylinder24", "BDM10_MESH_Cylinder24_Z", BuildCylinderMesh(24, 0.5f, 1f));
            AddMesh(meshes, "cylinder48", "BDM10_MESH_Cylinder48_Z", BuildCylinderMesh(48, 0.5f, 1f));
            AddMesh(meshes, "ring48", "BDM10_MESH_Ring48_Z", BuildRingMesh(48, 0.34f, 0.5f, 0.12f));
            AddMesh(meshes, "gear32", "BDM10_MESH_GearRing32_Z", BuildGearMesh(32, 0.25f, 0.42f, 0.56f, 0.14f));
            AddMesh(meshes, "hexBolt", "BDM10_MESH_HexBolt_Z", BuildCylinderMesh(6, 0.5f, 0.22f));
            AddMesh(meshes, "needle", "BDM10_MESH_GaugeNeedle", BuildNeedleMesh());
            AddMesh(meshes, "railTooth", "BDM10_MESH_RailTooth", BuildWedgeMesh());
            AddMesh(meshes, "lampCapsule", "BDM10_MESH_LampCapsule_Z", BuildCylinderMesh(32, 0.32f, 0.85f));
            return meshes;
        }

        private static void AddMesh(IDictionary<string, Mesh> meshes, string key, string assetName, Mesh mesh)
        {
            string path = MeshRoot + "/" + assetName + ".asset";
            AssetDatabase.CreateAsset(mesh, path);
            meshes[key] = mesh;
            MeshRecords.Add(new MeshRecord { Path = "Runtime/Meshes/" + assetName + ".asset" });
        }

        private static void CreateGearHub(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_GearHub_LargeCogCore");
            Part(root, "outer-toothed-gear-ring", meshes["gear32"], materials["agedBrass"], V3(0, 0, 0), V3(0, 0, 0), V3(1.55f, 1.55f, 0.55f));
            Part(root, "inner-black-oil-gasket", meshes["ring48"], materials["rubber"], V3(0, 0, -0.02f), V3(0, 0, 0), V3(1.04f, 1.04f, 0.40f));
            Part(root, "central-polished-hub", meshes["cylinder48"], materials["edgeBrass"], V3(0, 0, -0.10f), V3(0, 0, 0), V3(0.58f, 0.58f, 0.44f));
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f;
                Vector3 pos = Rotate(V3(0.55f, 0, -0.08f), angle);
                Part(root, "radial-riveted-spoke-" + i.ToString("00"), meshes["box"], materials["iron"], pos, V3(0, 0, angle), V3(0.82f, 0.08f, 0.08f));
                Part(root, "outer-hex-bolt-" + i.ToString("00"), meshes["hexBolt"], materials["edgeBrass"], Rotate(V3(0.92f, 0, -0.18f), angle), V3(0, 0, angle), V3(0.13f, 0.13f, 0.22f));
            }

            SavePrefab(root, "gear hub", "Large cog core for pressure-door locking mechanism; use as center hub, floor machinery, or wall detail.");
        }

        private static void CreateLockingBarPair(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_LockingBar_TelescopingPair");
            for (int i = 0; i < 2; i++)
            {
                float y = i == 0 ? -0.24f : 0.24f;
                Part(root, "blackened-main-bolt-bar-" + i, meshes["bar"], materials["iron"], V3(0, y, 0), V3(0, 0, 0), V3(1.0f, 0.70f, 0.70f));
                Part(root, "brass-telescoping-sleeve-left-" + i, meshes["cylinder24"], materials["agedBrass"], V3(-1.10f, y, 0), V3(0, 90, 0), V3(0.28f, 0.28f, 0.70f));
                Part(root, "brass-telescoping-sleeve-right-" + i, meshes["cylinder24"], materials["agedBrass"], V3(1.10f, y, 0), V3(0, 90, 0), V3(0.28f, 0.28f, 0.70f));
                Part(root, "red-pressure-index-stripe-" + i, meshes["box"], materials["red"], V3(0.0f, y + 0.13f, -0.13f), V3(0, 0, 0), V3(0.34f, 0.035f, 0.04f));
            }

            Part(root, "center-oiled-guide-block", meshes["box"], materials["gunmetal"], V3(0, 0, 0), V3(0, 0, 0), V3(0.44f, 0.86f, 0.38f));
            AddBoltGrid(root, meshes, materials, 0.34f, 0.34f, -0.24f, 2, 2, "guide-block");
            SavePrefab(root, "locking bars", "Telescoping horizontal door bolt pair for vault doors and pressure-lock corridor gates.");
        }

        private static void CreatePistonBrace(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_PistonBrace_SteamActuated");
            Part(root, "angled-blackened-brace-bar", meshes["bar"], materials["iron"], V3(0, 0, 0), V3(0, 0, -24), V3(1.05f, 0.52f, 0.52f));
            Part(root, "left-copper-piston-cylinder", meshes["cylinder48"], materials["copper"], V3(-1.05f, -0.42f, 0), V3(0, 90, -24), V3(0.34f, 0.34f, 0.82f));
            Part(root, "right-polished-piston-rod", meshes["cylinder24"], materials["edgeBrass"], V3(0.88f, 0.36f, 0), V3(0, 90, -24), V3(0.16f, 0.16f, 1.00f));
            Part(root, "mounting-eye-left", meshes["ring48"], materials["agedBrass"], V3(-1.62f, -0.66f, 0), V3(0, 0, -24), V3(0.48f, 0.48f, 0.50f));
            Part(root, "mounting-eye-right", meshes["ring48"], materials["agedBrass"], V3(1.48f, 0.60f, 0), V3(0, 0, -24), V3(0.48f, 0.48f, 0.50f));
            SavePrefab(root, "piston brace", "Steam-actuated diagonal piston brace for heavy door frames, lift assemblies, and moving lock plates.");
        }

        private static void CreateRivetedHinge(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_RivetedHinge_TripleLeaf");
            Part(root, "vertical-hinge-pin", meshes["cylinder48"], materials["edgeBrass"], V3(0, 0, 0), V3(90, 0, 0), V3(0.18f, 0.18f, 2.30f));
            for (int i = 0; i < 3; i++)
            {
                float y = -0.76f + i * 0.76f;
                Part(root, "blackened-hinge-leaf-" + i, meshes["box"], materials["gunmetal"], V3(0.42f, y, 0), V3(0, 0, 0), V3(0.82f, 0.38f, 0.09f));
                Part(root, "brass-barrel-section-" + i, meshes["cylinder24"], materials["agedBrass"], V3(0, y, 0), V3(90, 0, 0), V3(0.28f, 0.28f, 0.36f));
                AddBoltGrid(root, meshes, materials, 0.42f, y, -0.09f, 2, 1, "hinge-leaf-" + i);
            }

            SavePrefab(root, "riveted hinge", "Triple-leaf riveted hinge subassembly for thick mechanical doors and service hatches.");
        }

        private static void CreatePressureWheel(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_PressureWheel_CrankedValve");
            Part(root, "outer-handwheel-ring", meshes["ring48"], materials["agedBrass"], V3(0, 0, 0), V3(0, 0, 0), V3(1.45f, 1.45f, 0.34f));
            Part(root, "central-valve-hub", meshes["cylinder48"], materials["edgeBrass"], V3(0, 0, -0.04f), V3(0, 0, 0), V3(0.42f, 0.42f, 0.45f));
            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60f;
                Part(root, "wheel-spoke-" + i, meshes["box"], materials["iron"], Rotate(V3(0.43f, 0, -0.06f), angle), V3(0, 0, angle), V3(0.76f, 0.055f, 0.055f));
            }

            Part(root, "offset-crank-arm", meshes["box"], materials["copper"], V3(0.62f, -0.54f, -0.18f), V3(0, 0, -35), V3(0.58f, 0.09f, 0.09f));
            Part(root, "dark-wooden-grip-proxy", meshes["cylinder24"], materials["rubber"], V3(0.94f, -0.82f, -0.18f), V3(0, 90, -35), V3(0.13f, 0.13f, 0.32f));
            SavePrefab(root, "pressure wheel", "Cranked pressure wheel for interactive lock puzzles and noninteractive door dressing.");
        }

        private static void CreateBoltCollar(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_BoltCollar_SegmentedRing");
            Part(root, "black-rubber-inner-seal", meshes["ring48"], materials["rubber"], V3(0, 0, 0), V3(0, 0, 0), V3(1.10f, 1.10f, 0.34f));
            Part(root, "outer-gunmetal-collar", meshes["ring48"], materials["gunmetal"], V3(0, 0, -0.04f), V3(0, 0, 0), V3(1.50f, 1.50f, 0.28f));
            for (int i = 0; i < 12; i++)
            {
                float angle = i * 30f;
                Part(root, "segmented-brass-clamp-" + i.ToString("00"), meshes["box"], materials["agedBrass"], Rotate(V3(0.78f, 0, -0.16f), angle), V3(0, 0, angle), V3(0.22f, 0.11f, 0.08f));
                Part(root, "collar-bolt-head-" + i.ToString("00"), meshes["hexBolt"], materials["edgeBrass"], Rotate(V3(0.94f, 0, -0.24f), angle), V3(0, 0, angle), V3(0.09f, 0.09f, 0.16f));
            }

            SavePrefab(root, "bolt collar", "Segmented ring collar for shaft penetrations, vault lock axles, and pressure wheel centers.");
        }

        private static void CreateTrackRail(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_TrackRail_RatchetedDoorGuide");
            Part(root, "main-blackened-door-track", meshes["bar"], materials["iron"], V3(0, 0, 0), V3(0, 0, 0), V3(1.35f, 0.42f, 0.42f));
            Part(root, "polished-wear-strip-upper", meshes["bar"], materials["edgeBrass"], V3(0, 0.21f, -0.13f), V3(0, 0, 0), V3(1.35f, 0.10f, 0.08f));
            Part(root, "polished-wear-strip-lower", meshes["bar"], materials["edgeBrass"], V3(0, -0.21f, -0.13f), V3(0, 0, 0), V3(1.35f, 0.10f, 0.08f));
            for (int i = 0; i < 12; i++)
            {
                Part(root, "ratchet-tooth-" + i.ToString("00"), meshes["railTooth"], materials["gunmetal"], V3(-2.25f + i * 0.41f, 0, -0.26f), V3(0, 0, 0), V3(0.28f, 0.22f, 0.20f));
            }

            SavePrefab(root, "track rail", "Ratcheted door guide rail for sliding locks, side tracks, and industrial corridor gates.");
        }

        private static void CreateAmberLampCapsule(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_AmberLampCapsule_Caged");
            Part(root, "glowing-amber-glass-capsule", meshes["lampCapsule"], materials["amber"], V3(0, 0, 0), V3(90, 0, 0), V3(0.74f, 0.74f, 1.15f));
            Part(root, "top-aged-brass-cap", meshes["cylinder48"], materials["agedBrass"], V3(0, 0.55f, 0), V3(90, 0, 0), V3(0.44f, 0.44f, 0.18f));
            Part(root, "bottom-aged-brass-cap", meshes["cylinder48"], materials["agedBrass"], V3(0, -0.55f, 0), V3(90, 0, 0), V3(0.44f, 0.44f, 0.18f));
            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60f;
                Part(root, "protective-cage-bar-" + i, meshes["box"], materials["iron"], Rotate(V3(0.36f, 0, 0), angle), V3(0, 0, angle), V3(0.035f, 1.25f, 0.035f));
            }

            SavePrefab(root, "amber lamp capsule", "Caged amber lamp capsule for door state indication, warning panels, and frame lighting.");
        }

        private static void CreateGaugeValveManifold(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_GaugeValve_ManifoldCluster");
            Part(root, "blackened-manifold-backplate", meshes["box"], materials["gunmetal"], V3(0, 0, 0.06f), V3(0, 0, 0), V3(1.85f, 0.92f, 0.12f));
            for (int i = 0; i < 3; i++)
            {
                float x = -0.55f + i * 0.55f;
                Part(root, "ivory-gauge-face-" + i, meshes["cylinder48"], materials["ivory"], V3(x, 0.22f, -0.05f), V3(0, 0, 0), V3(0.34f, 0.34f, 0.08f));
                Part(root, "gauge-brass-bezel-" + i, meshes["ring48"], materials["agedBrass"], V3(x, 0.22f, -0.10f), V3(0, 0, 0), V3(0.42f, 0.42f, 0.20f));
                Part(root, "gauge-needle-" + i, meshes["needle"], materials["red"], V3(x, 0.22f, -0.17f), V3(0, 0, -35 + i * 22), V3(0.30f, 0.30f, 0.30f));
            }

            Part(root, "lower-copper-pipe", meshes["cylinder24"], materials["copper"], V3(0, -0.30f, -0.04f), V3(0, 90, 0), V3(0.12f, 0.12f, 1.65f));
            Part(root, "small-hand-valve", meshes["ring48"], materials["edgeBrass"], V3(0.78f, -0.30f, -0.18f), V3(0, 0, 0), V3(0.34f, 0.34f, 0.22f));
            SavePrefab(root, "gauge valve manifold", "Gauge and valve cluster for pressure-door logic panels and readable environmental machinery.");
        }

        private static void CreateCrossBoltLatch(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_DoorLatch_CrossBoltAssembly");
            Part(root, "horizontal-crossbolt", meshes["bar"], materials["iron"], V3(0, 0, 0), V3(0, 0, 0), V3(1.15f, 0.62f, 0.62f));
            Part(root, "vertical-locking-yoke", meshes["bar"], materials["gunmetal"], V3(0, 0, -0.12f), V3(0, 0, 90), V3(0.64f, 0.52f, 0.52f));
            Part(root, "central-brass-release-hub", meshes["cylinder48"], materials["agedBrass"], V3(0, 0, -0.25f), V3(0, 0, 0), V3(0.48f, 0.48f, 0.28f));
            AddBoltGrid(root, meshes, materials, 0, 0, -0.40f, 2, 2, "crossbolt-center");
            SavePrefab(root, "crossbolt latch", "Heavy crossbolt latch assembly for visual door locking states and close-up interactables.");
        }

        private static void CreateGearRackDrive(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_GearRack_HorizontalDrive");
            Part(root, "linear-rack-base", meshes["bar"], materials["iron"], V3(0, -0.22f, 0), V3(0, 0, 0), V3(1.20f, 0.36f, 0.30f));
            for (int i = 0; i < 14; i++)
            {
                Part(root, "rack-tooth-" + i.ToString("00"), meshes["railTooth"], materials["edgeBrass"], V3(-2.35f + i * 0.36f, 0.02f, -0.08f), V3(0, 0, 0), V3(0.24f, 0.18f, 0.18f));
            }

            Part(root, "drive-pinion-gear", meshes["gear32"], materials["agedBrass"], V3(0, 0.48f, -0.12f), V3(0, 0, 0), V3(0.80f, 0.80f, 0.34f));
            Part(root, "pinion-center-bolt", meshes["hexBolt"], materials["edgeBrass"], V3(0, 0.48f, -0.32f), V3(0, 0, 0), V3(0.12f, 0.12f, 0.18f));
            SavePrefab(root, "gear rack", "Horizontal rack-and-pinion door drive detail for sliding mechanisms and exposed machinery walls.");
        }

        private static void CreatePressureLockModule(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_PressureLock_SideModule");
            Part(root, "rectangular-gunmetal-module-case", meshes["box"], materials["gunmetal"], V3(0, 0, 0.05f), V3(0, 0, 0), V3(1.10f, 1.55f, 0.26f));
            Part(root, "vertical-copper-pressure-tube", meshes["cylinder24"], materials["copper"], V3(-0.42f, 0, -0.12f), V3(90, 0, 0), V3(0.15f, 0.15f, 1.36f));
            Part(root, "amber-status-window", meshes["box"], materials["amber"], V3(0.33f, 0.36f, -0.12f), V3(0, 0, 0), V3(0.36f, 0.48f, 0.06f));
            Part(root, "small-round-gauge", meshes["cylinder48"], materials["ivory"], V3(0.34f, -0.34f, -0.13f), V3(0, 0, 0), V3(0.28f, 0.28f, 0.08f));
            Part(root, "gauge-bezel", meshes["ring48"], materials["agedBrass"], V3(0.34f, -0.34f, -0.18f), V3(0, 0, 0), V3(0.36f, 0.36f, 0.18f));
            AddBoltGrid(root, meshes, materials, 0, 0, -0.18f, 2, 3, "pressure-lock-case");
            SavePrefab(root, "pressure lock module", "Side-mounted pressure lock module with tube, status window, and gauge for puzzle-ready door frames.");
        }

        private static void CreateSteamReleaseLever(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_SteamReleaseValve_PullLever");
            Part(root, "base-valve-body", meshes["cylinder48"], materials["agedBrass"], V3(0, 0, 0), V3(0, 0, 0), V3(0.48f, 0.48f, 0.42f));
            Part(root, "lever-arm-blackened", meshes["bar"], materials["iron"], V3(0.72f, 0.45f, -0.08f), V3(0, 0, 35), V3(0.52f, 0.26f, 0.22f));
            Part(root, "red-release-grip", meshes["cylinder24"], materials["red"], V3(1.15f, 0.75f, -0.08f), V3(0, 90, 35), V3(0.14f, 0.14f, 0.34f));
            for (int i = 0; i < 5; i++)
            {
                Part(root, "return-spring-ring-" + i, meshes["ring48"], materials["edgeBrass"], V3(-0.12f + i * 0.13f, -0.44f, -0.08f), V3(0, 90, 0), V3(0.18f, 0.18f, 0.08f));
            }

            SavePrefab(root, "steam release lever", "Pull-lever steam release valve for door puzzle affordance, warning panels, and vent controls.");
        }

        private static void CreatePalettePreview(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = Root("BDM10_MechanismKit_PalettePreview");
            Part(root, "mini-gear-reference", meshes["gear32"], materials["agedBrass"], V3(-1.10f, 0.42f, 0), V3(0, 0, 0), V3(0.68f, 0.68f, 0.28f));
            Part(root, "mini-locking-bar-reference", meshes["bar"], materials["iron"], V3(0.78f, 0.42f, 0), V3(0, 0, 0), V3(0.52f, 0.38f, 0.38f));
            Part(root, "mini-pressure-wheel-reference", meshes["ring48"], materials["edgeBrass"], V3(-1.05f, -0.52f, 0), V3(0, 0, 0), V3(0.70f, 0.70f, 0.24f));
            Part(root, "mini-amber-lamp-reference", meshes["lampCapsule"], materials["amber"], V3(0.62f, -0.50f, 0), V3(90, 0, 0), V3(0.42f, 0.42f, 0.70f));
            Part(root, "palette-backplate", meshes["box"], materials["soot"], V3(0, 0, 0.14f), V3(0, 0, 0), V3(2.95f, 1.65f, 0.05f));
            SavePrefab(root, "palette preview", "Compact palette preview prefab showing the visual language for this package family.");
        }

        private static void AddBoltGrid(GameObject root, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, float centerX, float centerY, float z, int columns, int rows, string prefix)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    float px = centerX + (x - (columns - 1) * 0.5f) * 0.26f;
                    float py = centerY + (y - (rows - 1) * 0.5f) * 0.26f;
                    Part(root, prefix + "-rivet-" + x + "-" + y, meshes["hexBolt"], materials["edgeBrass"], V3(px, py, z), V3(0, 0, 30), V3(0.07f, 0.07f, 0.12f));
                }
            }
        }

        private static GameObject Root(string name)
        {
            GameObject root = new GameObject(name);
            root.transform.position = Vector3.zero;
            return root;
        }

        private static GameObject Part(GameObject root, string name, Mesh mesh, Material material, Vector3 pos, Vector3 euler, Vector3 scale)
        {
            GameObject child = new GameObject(name);
            child.transform.SetParent(root.transform, false);
            child.transform.localPosition = pos;
            child.transform.localRotation = Quaternion.Euler(euler);
            child.transform.localScale = scale;
            MeshFilter filter = child.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer = child.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            return child;
        }

        private static void SavePrefab(GameObject root, string role, string notes)
        {
            Bounds bounds = CalculateBounds(root);
            int rendererCount = root.GetComponentsInChildren<MeshRenderer>(true).Length;
            string path = PrefabRoot + "/" + root.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(root, path);
            PrefabRecords.Add(new PrefabRecord
            {
                Id = root.name,
                Path = "Runtime/Prefabs/" + root.name + ".prefab",
                Role = role,
                Notes = notes,
                RendererCount = rendererCount,
                Bounds = bounds.size
            });
            Object.DestroyImmediate(root);
        }

        private static void RenderPreviews(string renderRoot)
        {
            string packagePreviewPhysical = Path.Combine(PackagePhysicalRoot(), "Documentation~", "Previews");
            Directory.CreateDirectory(packagePreviewPhysical);
            foreach (PrefabRecord record in PrefabRecords)
            {
                string assetPath = PrefabRoot + "/" + record.Id + ".prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                string fileName = record.Id.Replace("_", "-").ToLowerInvariant() + ".png";
                string repoPath = Path.Combine(renderRoot, fileName);
                string packagePath = Path.Combine(packagePreviewPhysical, fileName);
                Texture2D image = RenderPrefab(prefab);
                byte[] png = ImageConversion.EncodeToPNG(image);
                File.WriteAllBytes(repoPath, png);
                File.WriteAllBytes(packagePath, png);
                Object.DestroyImmediate(image);
                record.PreviewPath = ToRepoRelative(repoPath);
                PreviewRelativePaths.Add(record.PreviewPath);
            }
        }

        private static Texture2D RenderPrefab(GameObject prefab)
        {
            GameObject root = new GameObject("BDM10_RenderRoot");
            GameObject instance = Object.Instantiate(prefab, root.transform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.Euler(12f, -26f, 0f);

            Bounds bounds = CalculateBounds(instance);
            float max = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z, 0.5f);
            instance.transform.localScale = Vector3.one * (2.3f / max);
            bounds = CalculateBounds(instance);

            GameObject cameraObject = new GameObject("PreviewCamera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = C(0.045f, 0.043f, 0.039f);
            camera.orthographic = true;
            camera.orthographicSize = Mathf.Max(bounds.size.x, bounds.size.y) * 0.72f + 0.35f;
            camera.transform.position = bounds.center + new Vector3(0.4f, 0.15f, -6.0f);
            camera.transform.LookAt(bounds.center);
            camera.nearClipPlane = 0.01f;
            camera.farClipPlane = 30f;

            AddLight("warm-key", new Vector3(-2.2f, 2.4f, -3.2f), bounds.center, C(1.0f, 0.70f, 0.40f), 2.5f);
            AddLight("cool-rim", new Vector3(2.2f, 1.4f, -2.4f), bounds.center, C(0.38f, 0.55f, 0.68f), 0.75f);
            AddLight("amber-glow", new Vector3(0.0f, -1.2f, -2.4f), bounds.center, C(1.0f, 0.46f, 0.12f), 1.35f);

            RenderTexture rt = new RenderTexture(PreviewWidth, PreviewHeight, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = rt;
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;
            camera.Render();
            Texture2D output = new Texture2D(PreviewWidth, PreviewHeight, TextureFormat.RGBA32, false);
            output.ReadPixels(new Rect(0, 0, PreviewWidth, PreviewHeight), 0, 0);
            output.Apply();
            RenderTexture.active = previous;
            camera.targetTexture = null;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(root);
            foreach (Light light in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
            {
                Object.DestroyImmediate(light.gameObject);
            }
            Object.DestroyImmediate(cameraObject);
            return output;
        }

        private static void AddLight(string name, Vector3 position, Vector3 target, Color color, float intensity)
        {
            GameObject lightObject = new GameObject(name);
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = color;
            light.intensity = intensity;
            lightObject.transform.position = position;
            lightObject.transform.LookAt(target);
        }

        private static void RenderContactSheet(string renderRoot)
        {
            int columns = 4;
            int cellWidth = 520;
            int cellHeight = 360;
            int rows = Mathf.CeilToInt(PrefabRecords.Count / (float)columns);
            Texture2D sheet = new Texture2D(columns * cellWidth, rows * cellHeight, TextureFormat.RGBA32, false);
            Color bg = C(0.035f, 0.034f, 0.031f);
            for (int y = 0; y < sheet.height; y++)
            {
                for (int x = 0; x < sheet.width; x++) sheet.SetPixel(x, y, bg);
            }

            for (int i = 0; i < PrefabRecords.Count; i++)
            {
                string previewPath = Path.Combine(RepoRoot(), PrefabRecords[i].PreviewPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                Texture2D src = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                src.LoadImage(File.ReadAllBytes(previewPath));
                int col = i % columns;
                int row = rows - 1 - (i / columns);
                BlitScaled(src, sheet, col * cellWidth + 18, row * cellHeight + 18, cellWidth - 36, cellHeight - 36);
                Object.DestroyImmediate(src);
            }

            sheet.Apply();
            string fileName = "BDM10_PREVIEW_contact-sheet.png";
            string repoPath = Path.Combine(renderRoot, fileName);
            string packagePath = Path.Combine(PackagePhysicalRoot(), "Documentation~", "Previews", fileName);
            byte[] png = ImageConversion.EncodeToPNG(sheet);
            File.WriteAllBytes(repoPath, png);
            File.WriteAllBytes(packagePath, png);
            Object.DestroyImmediate(sheet);
            PreviewRelativePaths.Add(ToRepoRelative(repoPath));
        }

        private static ValidationResult ValidateGeneratedContent(string renderRoot)
        {
            ValidationResult result = new ValidationResult();
            result.PrefabCount = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot }).Length;
            result.MaterialCount = AssetDatabase.FindAssets("t:Material", new[] { MaterialRoot }).Length;
            result.MeshCount = AssetDatabase.FindAssets("t:Mesh", new[] { MeshRoot }).Length;
            result.TextureCount = Directory.GetFiles(Path.Combine(PackagePhysicalRoot(), "Runtime", "Textures"), "*.png").Length;
            result.PreviewCount = Directory.GetFiles(renderRoot, "*.png").Length;

            if (result.PrefabCount != 14) result.Failures.Add("Expected 14 prefabs, found " + result.PrefabCount);
            if (result.MaterialCount != 12) result.Failures.Add("Expected 12 materials, found " + result.MaterialCount);
            if (result.MeshCount != 10) result.Failures.Add("Expected 10 meshes, found " + result.MeshCount);
            if (result.TextureCount != 30) result.Failures.Add("Expected 30 texture maps, found " + result.TextureCount);
            if (result.PreviewCount != 15) result.Failures.Add("Expected 15 concept preview PNGs, found " + result.PreviewCount);

            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot }))
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                result.ColliderCount += prefab.GetComponentsInChildren<Collider>(true).Length;
                result.RigidbodyCount += prefab.GetComponentsInChildren<Rigidbody>(true).Length;
                result.LightCount += prefab.GetComponentsInChildren<Light>(true).Length;
                result.AudioSourceCount += prefab.GetComponentsInChildren<AudioSource>(true).Length;
                result.CameraCount += prefab.GetComponentsInChildren<Camera>(true).Length;
                result.AnimatorCount += prefab.GetComponentsInChildren<Animator>(true).Length;
                result.MonoBehaviourCount += prefab.GetComponentsInChildren<MonoBehaviour>(true).Length;
            }

            if (result.ColliderCount != 0) result.Failures.Add("Visual-only violation: colliders=" + result.ColliderCount);
            if (result.RigidbodyCount != 0) result.Failures.Add("Visual-only violation: rigidbodies=" + result.RigidbodyCount);
            if (result.LightCount != 0) result.Failures.Add("Visual-only violation: lights=" + result.LightCount);
            if (result.AudioSourceCount != 0) result.Failures.Add("Visual-only violation: audio sources=" + result.AudioSourceCount);
            if (result.CameraCount != 0) result.Failures.Add("Visual-only violation: cameras=" + result.CameraCount);
            if (result.AnimatorCount != 0) result.Failures.Add("Visual-only limitation violation: animators=" + result.AnimatorCount);
            if (result.MonoBehaviourCount != 0) result.Failures.Add("Visual-only violation: MonoBehaviours=" + result.MonoBehaviourCount);
            return result;
        }

        private static string BuildManifest(ValidationResult validation)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            JsonLine(json, "packageId", PackageName, 1, true);
            JsonLine(json, "version", VersionFull, 1, true);
            JsonLine(json, "status", validation.Passed ? "STATIC_READY_WITH_LIMITATIONS" : "FAILED", 1, true);
            JsonLine(json, "role", "isolated Unity-only visual sidecar for brassworks door mechanism objects", 1, true);
            json.AppendLine("  \"counts\": {");
            JsonNumber(json, "prefabs", validation.PrefabCount, 2, true);
            JsonNumber(json, "materials", validation.MaterialCount, 2, true);
            JsonNumber(json, "textures", validation.TextureCount, 2, true);
            JsonNumber(json, "meshes", validation.MeshCount, 2, true);
            JsonNumber(json, "conceptPreviewPngs", validation.PreviewCount, 2, false);
            json.AppendLine("  },");
            json.AppendLine("  \"prefabs\": [");
            for (int i = 0; i < PrefabRecords.Count; i++)
            {
                PrefabRecord p = PrefabRecords[i];
                json.AppendLine("    {");
                JsonLine(json, "id", p.Id, 3, true);
                JsonLine(json, "path", p.Path, 3, true);
                JsonLine(json, "role", p.Role, 3, true);
                JsonLine(json, "preview", p.PreviewPath, 3, true);
                JsonNumber(json, "rendererCount", p.RendererCount, 3, true);
                JsonLine(json, "notes", p.Notes, 3, false);
                json.Append("    }");
                if (i < PrefabRecords.Count - 1) json.Append(",");
                json.AppendLine();
            }
            json.AppendLine("  ],");
            WriteRecordArray(json, "materials", MaterialRecords.Select(m => m.Path).ToArray(), true);
            WriteRecordArray(json, "textures", TextureRecords.Select(t => t.Path).ToArray(), true);
            WriteRecordArray(json, "meshes", MeshRecords.Select(m => m.Path).ToArray(), true);
            WriteRecordArray(json, "previews", PreviewRelativePaths.ToArray(), false);
            json.AppendLine("}");
            return json.ToString();
        }

        private static void WriteManifestCopies(string manifest)
        {
            string fileName = "BDM10_BrassworksDoorMechanismSet10_Manifest_v" + VersionFull + ".json";
            WriteText(Path.Combine(PackagePhysicalRoot(), "Runtime", "Metadata", fileName), manifest);
            WriteText(Path.Combine(PackagePhysicalRoot(), "Documentation~", "Manifest", fileName), manifest);
            WriteText(Path.Combine(ResolveRepoRelativeFolder(ProductionDocFolder), fileName), manifest);
            AssetDatabase.ImportAsset(MetadataRoot + "/" + fileName, ImportAssetOptions.ForceUpdate);
            AssetDatabase.ImportAsset(ManifestRoot + "/" + fileName, ImportAssetOptions.ForceUpdate);
        }

        private static void WriteDocumentation(string repoRoot, string productionRoot, string planningRoot, string qaRoot, string renderRoot, ValidationResult validation)
        {
            string inventoryPath = Path.Combine(productionRoot, "BDM10_AssetInventory_" + VersionFull + ".md");
            StringBuilder inventory = new StringBuilder();
            inventory.AppendLine("# BDM10 Asset Inventory " + VersionFull);
            inventory.AppendLine();
            inventory.AppendLine("Unity-only visual sidecar candidate for detailed brassworks door mechanism objects.");
            inventory.AppendLine();
            inventory.AppendLine("## Prefabs");
            foreach (PrefabRecord record in PrefabRecords)
            {
                inventory.AppendLine("- `" + record.Id + "` - " + record.Role + "; renderers " + record.RendererCount.ToString(CultureInfo.InvariantCulture) + "; preview `" + record.PreviewPath + "`");
            }
            inventory.AppendLine();
            inventory.AppendLine("## Materials");
            foreach (MaterialRecord record in MaterialRecords) inventory.AppendLine("- `" + record.Path + "` - " + record.Tag);
            inventory.AppendLine();
            inventory.AppendLine("## Texture Maps");
            foreach (TextureRecord record in TextureRecords) inventory.AppendLine("- `" + record.Path + "` - " + record.Tag);
            inventory.AppendLine();
            inventory.AppendLine("## Mesh Assets");
            foreach (MeshRecord record in MeshRecords) inventory.AppendLine("- `" + record.Path + "`");
            WriteText(inventoryPath, inventory.ToString());

            string productionReport = "# BDM10 Production Report " + VersionFull + "\n\n"
                + "Generated in Unity through an isolated temporary project. No Blender or external DCC was used.\n\n"
                + "The set focuses on reusable mechanism subassemblies that can supplement Cicero's broader DoorVaultSet10 lane without modifying it.\n\n"
                + "## Acceptance Notes\n\n"
                + "- Visual-only prefab hierarchy with descriptive child names.\n"
                + "- Brass, blackened iron, copper, amber glass, rubber, soot, oil, and gauge enamel materials generated with package-local texture maps.\n"
                + "- Concept preview PNGs are written outside the buildable game assets under `" + RenderDocFolder + "`.\n"
                + "- Candidate is not imported into the main project manifest yet.\n";
            WriteText(Path.Combine(productionRoot, "BDM10_ProductionReport_" + VersionFull + ".md"), productionReport);

            string planning = "# BDM10 Import Readiness Notes " + VersionFull + "\n\n"
                + "## Intended Import Path\n\n"
                + "Import as a quarantined sidecar package after visual review. Candidate objects are door-mechanism supplements, not final rigged or interactive mechanisms.\n\n"
                + "## Follow-up Before Mainline Promotion\n\n"
                + "- Review contact sheet and individual previews against the north-star steampunk corridor target.\n"
                + "- Choose which mechanisms should become interactive versus static dressing.\n"
                + "- Add colliders, LODs, animation hooks, and gameplay scripts only in a future integration branch.\n"
                + "- Combine with RoomMaterialSet10, PipeTankGaugeSet10, GrimeDecalWetnessSet10, and DoorVaultSet10 for corridor render validation.\n";
            WriteText(Path.Combine(planningRoot, "BDM10_ImportReadinessNotes_" + VersionFull + ".md"), planning);

            StringBuilder qa = new StringBuilder();
            qa.AppendLine("# BDM10 Validation Report " + VersionFull);
            qa.AppendLine();
            qa.AppendLine("Status: " + (validation.Passed ? "PASS" : "FAIL"));
            qa.AppendLine("Package root: " + PackagePhysicalRoot().Replace("\\", "/"));
            qa.AppendLine("Render root: " + renderRoot.Replace("\\", "/"));
            qa.AppendLine();
            qa.AppendLine("## Counts");
            qa.AppendLine("- Prefabs: " + validation.PrefabCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Materials: " + validation.MaterialCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Texture maps: " + validation.TextureCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Meshes: " + validation.MeshCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Concept preview PNGs: " + validation.PreviewCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine();
            qa.AppendLine("## Visual-only Component Check");
            qa.AppendLine("- Colliders: " + validation.ColliderCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Rigidbodies: " + validation.RigidbodyCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Lights in prefabs: " + validation.LightCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Cameras in prefabs: " + validation.CameraCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Audio sources: " + validation.AudioSourceCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- Animators: " + validation.AnimatorCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine("- MonoBehaviours: " + validation.MonoBehaviourCount.ToString(CultureInfo.InvariantCulture));
            qa.AppendLine();
            qa.AppendLine("## Preview Checksums");
            foreach (string preview in PreviewRelativePaths)
            {
                string full = Path.Combine(repoRoot, preview.Replace("/", Path.DirectorySeparatorChar.ToString()));
                qa.AppendLine("- `" + preview + "` sha256:" + Sha256(full));
            }
            qa.AppendLine();
            qa.AppendLine("## Failures");
            if (validation.Failures.Count == 0) qa.AppendLine("- None.");
            foreach (string failure in validation.Failures) qa.AppendLine("- " + failure);
            WriteText(Path.Combine(qaRoot, "BDM10_ValidationReport.md"), qa.ToString());

            string jsonValidation = "{\n"
                + "  \"status\": \"" + (validation.Passed ? "PASS" : "FAIL") + "\",\n"
                + "  \"prefabs\": " + validation.PrefabCount.ToString(CultureInfo.InvariantCulture) + ",\n"
                + "  \"materials\": " + validation.MaterialCount.ToString(CultureInfo.InvariantCulture) + ",\n"
                + "  \"textures\": " + validation.TextureCount.ToString(CultureInfo.InvariantCulture) + ",\n"
                + "  \"meshes\": " + validation.MeshCount.ToString(CultureInfo.InvariantCulture) + ",\n"
                + "  \"previews\": " + validation.PreviewCount.ToString(CultureInfo.InvariantCulture) + ",\n"
                + "  \"visualOnlyForbiddenComponents\": " + (validation.ColliderCount + validation.RigidbodyCount + validation.LightCount + validation.CameraCount + validation.AudioSourceCount + validation.AnimatorCount + validation.MonoBehaviourCount).ToString(CultureInfo.InvariantCulture) + "\n"
                + "}\n";
            WriteText(Path.Combine(qaRoot, "BDM10_ValidationReport.json"), jsonValidation);

            string finalList = string.Join("\n", Directory.GetFiles(PackagePhysicalRoot(), "*", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(productionRoot, "*", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(renderRoot, "*", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(planningRoot, "*", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(qaRoot, "*", SearchOption.AllDirectories))
                .Select(path => ToRepoRelative(path)).OrderBy(path => path, StringComparer.Ordinal)) + "\n";
            WriteText(Path.Combine(qaRoot, "BDM10_FinalFileList.txt"), finalList);
        }

        private static Texture2D BuildTexture(MaterialSpec spec, int width, int height, bool metalMap, bool normalMap)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            int seed = StableSeed(spec.AssetName + spec.Style);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float u = x / (float)(width - 1);
                    float v = y / (float)(height - 1);
                    float n = Noise(x, y, seed);
                    float scratches = Mathf.Pow(Mathf.Abs(Mathf.Sin((u * 29f + n * 0.4f) * Mathf.PI)), 14f);
                    float verticalWear = Mathf.Pow(Mathf.Abs(Mathf.Sin((v * 12f + n) * Mathf.PI)), 8f);
                    if (normalMap)
                    {
                        float nx = Noise(x + 2, y, seed) - Noise(x - 2, y, seed);
                        float ny = Noise(x, y + 2, seed) - Noise(x, y - 2, seed);
                        texture.SetPixel(x, y, new Color(0.5f + nx * 0.25f, 0.5f + ny * 0.25f, 0.92f, 1f));
                    }
                    else if (metalMap)
                    {
                        float metal = Mathf.Clamp01(spec.Metallic + scratches * 0.08f - n * 0.04f);
                        float smooth = Mathf.Clamp01(spec.Smoothness + verticalWear * 0.12f - n * 0.10f);
                        texture.SetPixel(x, y, new Color(metal, metal, metal, smooth));
                    }
                    else
                    {
                        Color c = spec.Color;
                        float wear = scratches * 0.26f + verticalWear * 0.12f;
                        float grime = n * 0.22f;
                        if (spec.Style == TextureStyle.Amber) c = Color.Lerp(c, C(1.0f, 0.84f, 0.28f), Mathf.Clamp01(wear + 0.18f));
                        else if (spec.Style == TextureStyle.Ivory) c = Color.Lerp(c, C(0.26f, 0.19f, 0.11f), grime * 0.52f);
                        else if (spec.Style == TextureStyle.Verdigris) c = Color.Lerp(c, C(0.55f, 0.85f, 0.68f), wear * 0.5f);
                        else if (spec.Style == TextureStyle.Soot || spec.Style == TextureStyle.Oil) c = Color.Lerp(c, C(0.0f, 0.0f, 0.0f), grime * 0.6f);
                        else c = Color.Lerp(Color.Lerp(c, C(0.02f, 0.018f, 0.014f), grime), C(1.0f, 0.78f, 0.42f), wear * 0.38f);
                        c.a = spec.Alpha;
                        texture.SetPixel(x, y, c);
                    }
                }
            }

            texture.Apply();
            return texture;
        }

        private static Mesh BuildBoxMesh(float sx, float sy, float sz)
        {
            float x = sx * 0.5f;
            float y = sy * 0.5f;
            float z = sz * 0.5f;
            Vector3[] v =
            {
                V3(-x,-y,-z), V3(x,-y,-z), V3(x,y,-z), V3(-x,y,-z),
                V3(-x,-y,z), V3(x,-y,z), V3(x,y,z), V3(-x,y,z)
            };
            int[] t =
            {
                0,2,1, 0,3,2, 4,5,6, 4,6,7, 0,1,5, 0,5,4,
                1,2,6, 1,6,5, 2,3,7, 2,7,6, 3,0,4, 3,4,7
            };
            return MeshFrom(v, t, "box");
        }

        private static Mesh BuildCylinderMesh(int sides, float radius, float depth)
        {
            List<Vector3> v = new List<Vector3>();
            List<int> t = new List<int>();
            float z0 = -depth * 0.5f;
            float z1 = depth * 0.5f;
            for (int i = 0; i < sides; i++)
            {
                float a = Mathf.PI * 2f * i / sides;
                v.Add(V3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, z0));
                v.Add(V3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, z1));
            }

            int centerBack = v.Count;
            v.Add(V3(0, 0, z0));
            int centerFront = v.Count;
            v.Add(V3(0, 0, z1));
            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                int a0 = i * 2;
                int a1 = i * 2 + 1;
                int b0 = next * 2;
                int b1 = next * 2 + 1;
                t.Add(a0); t.Add(a1); t.Add(b1);
                t.Add(a0); t.Add(b1); t.Add(b0);
                t.Add(centerBack); t.Add(b0); t.Add(a0);
                t.Add(centerFront); t.Add(a1); t.Add(b1);
            }

            return MeshFrom(v.ToArray(), t.ToArray(), "cylinder");
        }

        private static Mesh BuildRingMesh(int sides, float inner, float outer, float depth)
        {
            List<Vector3> v = new List<Vector3>();
            List<int> t = new List<int>();
            float z0 = -depth * 0.5f;
            float z1 = depth * 0.5f;
            for (int i = 0; i < sides; i++)
            {
                float a = Mathf.PI * 2f * i / sides;
                Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                v.Add(V3(dir.x * inner, dir.y * inner, z0));
                v.Add(V3(dir.x * outer, dir.y * outer, z0));
                v.Add(V3(dir.x * inner, dir.y * inner, z1));
                v.Add(V3(dir.x * outer, dir.y * outer, z1));
            }

            for (int i = 0; i < sides; i++)
            {
                int n = (i + 1) % sides;
                int a = i * 4;
                int b = n * 4;
                AddQuad(t, a + 1, b + 1, b + 3, a + 3);
                AddQuad(t, b + 0, a + 0, a + 2, b + 2);
                AddQuad(t, a + 2, a + 3, b + 3, b + 2);
                AddQuad(t, a + 0, b + 0, b + 1, a + 1);
            }

            return MeshFrom(v.ToArray(), t.ToArray(), "ring");
        }

        private static Mesh BuildGearMesh(int teeth, float inner, float root, float tip, float depth)
        {
            int sides = teeth * 2;
            List<Vector3> v = new List<Vector3>();
            List<int> t = new List<int>();
            float z0 = -depth * 0.5f;
            float z1 = depth * 0.5f;
            for (int i = 0; i < sides; i++)
            {
                float a = Mathf.PI * 2f * i / sides;
                float outer = (i % 2 == 0) ? tip : root;
                Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                v.Add(V3(dir.x * inner, dir.y * inner, z0));
                v.Add(V3(dir.x * outer, dir.y * outer, z0));
                v.Add(V3(dir.x * inner, dir.y * inner, z1));
                v.Add(V3(dir.x * outer, dir.y * outer, z1));
            }

            for (int i = 0; i < sides; i++)
            {
                int n = (i + 1) % sides;
                int a = i * 4;
                int b = n * 4;
                AddQuad(t, a + 1, b + 1, b + 3, a + 3);
                AddQuad(t, b + 0, a + 0, a + 2, b + 2);
                AddQuad(t, a + 2, a + 3, b + 3, b + 2);
                AddQuad(t, a + 0, b + 0, b + 1, a + 1);
            }

            return MeshFrom(v.ToArray(), t.ToArray(), "gear");
        }

        private static Mesh BuildNeedleMesh()
        {
            Vector3[] v = { V3(-0.05f, -0.04f, 0), V3(0.56f, 0, 0), V3(-0.05f, 0.04f, 0), V3(-0.16f, 0, 0) };
            int[] t = { 0, 1, 2, 0, 3, 1 };
            return MeshFrom(v, t, "needle");
        }

        private static Mesh BuildWedgeMesh()
        {
            Vector3[] v =
            {
                V3(-0.5f,-0.5f,-0.5f), V3(0.5f,-0.5f,-0.5f), V3(0.5f,0.5f,-0.5f), V3(-0.5f,0.5f,-0.5f),
                V3(-0.5f,-0.5f,0.5f), V3(0.5f,-0.5f,0.5f), V3(0.0f,0.5f,0.5f)
            };
            int[] t = { 0,2,1, 0,3,2, 4,5,6, 0,1,5, 0,5,4, 1,2,6, 1,6,5, 2,3,6, 3,0,4, 3,4,6 };
            return MeshFrom(v, t, "wedge");
        }

        private static Mesh MeshFrom(Vector3[] vertices, int[] triangles, string name)
        {
            Mesh mesh = new Mesh { name = name };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            Vector2[] uv = new Vector2[vertices.Length];
            for (int i = 0; i < uv.Length; i++) uv[i] = new Vector2(vertices[i].x, vertices[i].y);
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void AddQuad(List<int> t, int a, int b, int c, int d)
        {
            t.Add(a); t.Add(b); t.Add(c);
            t.Add(a); t.Add(c); t.Add(d);
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0) return new Bounds(Vector3.zero, Vector3.one);
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
            return bounds;
        }

        private static void BlitScaled(Texture2D source, Texture2D destination, int x0, int y0, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                float v = y / (float)Mathf.Max(1, height - 1);
                for (int x = 0; x < width; x++)
                {
                    float u = x / (float)Mathf.Max(1, width - 1);
                    destination.SetPixel(x0 + x, y0 + y, source.GetPixelBilinear(u, v));
                }
            }
        }

        private static void EnsurePackageFolders()
        {
            foreach (string path in new[] { MaterialRoot, MeshRoot, TextureRoot, PrefabRoot, MetadataRoot, ManifestRoot, PackagePreviewRoot })
            {
                string physical = Path.Combine(PackagePhysicalRoot(), path.Substring(PackageRoot.Length + 1).Replace("/", Path.DirectorySeparatorChar.ToString()));
                Directory.CreateDirectory(physical);
            }

            AssetDatabase.Refresh();
        }

        private static string PackagePhysicalRoot()
        {
            UnityEditor.PackageManager.PackageInfo info = UnityEditor.PackageManager.PackageInfo.FindForPackageName(PackageName);
            if (info != null && Directory.Exists(info.resolvedPath)) return info.resolvedPath;
            throw new DirectoryNotFoundException("Could not resolve package root for " + PackageName);
        }

        private static string RepoRoot()
        {
            string env = Environment.GetEnvironmentVariable("BDM10_REPO_ROOT");
            if (!string.IsNullOrWhiteSpace(env) && Directory.Exists(env)) return env;
            DirectoryInfo packageDir = new DirectoryInfo(PackagePhysicalRoot());
            DirectoryInfo repoDir = packageDir.Parent?.Parent;
            if (repoDir == null) throw new DirectoryNotFoundException("Could not resolve repo root from " + packageDir.FullName);
            return repoDir.FullName;
        }

        private static string ResolveRepoRelativeFolder(string relativePath)
        {
            return Path.Combine(RepoRoot(), relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        }

        private static string ToRepoRelative(string absolutePath)
        {
            string repo = RepoRoot().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string full = Path.GetFullPath(absolutePath);
            if (full.StartsWith(repo, StringComparison.OrdinalIgnoreCase))
            {
                return full.Substring(repo.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
            }

            return full.Replace("\\", "/");
        }

        private static void WriteText(string path, string contents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            File.WriteAllText(path, contents, new UTF8Encoding(false));
        }

        private static string Sha256(string path)
        {
            using (SHA256 sha = SHA256.Create())
            using (FileStream stream = File.OpenRead(path))
            {
                return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", string.Empty).ToLowerInvariant();
            }
        }

        private static Vector3 Rotate(Vector3 point, float degrees)
        {
            return Quaternion.Euler(0, 0, degrees) * point;
        }

        private static float Noise(int x, int y, int seed)
        {
            return Hash01(x, y, seed) * 0.55f + Hash01(x / 7, y / 7, seed + 11) * 0.30f + Hash01(x / 23, y / 23, seed + 97) * 0.15f;
        }

        private static int StableSeed(string value)
        {
            unchecked
            {
                int hash = 23;
                for (int i = 0; i < value.Length; i++) hash = hash * 31 + value[i];
                return hash;
            }
        }

        private static float Hash01(int x, int y, int seed)
        {
            unchecked
            {
                int h = x * 374761393 + y * 668265263 + seed * 1442695041;
                h = (h ^ (h >> 13)) * 1274126177;
                return ((h ^ (h >> 16)) & 0x7fffffff) / (float)0x7fffffff;
            }
        }

        private static Color C(float r, float g, float b)
        {
            return new Color(r, g, b, 1f);
        }

        private static Vector3 V3(float x, float y, float z)
        {
            return new Vector3(x, y, z);
        }

        private static void SetColor(Material material, string property, Color value)
        {
            if (material.HasProperty(property)) material.SetColor(property, value);
        }

        private static void SetFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property)) material.SetFloat(property, value);
        }

        private static void SetTexture(Material material, string property, Texture value)
        {
            if (value != null && material.HasProperty(property)) material.SetTexture(property, value);
        }

        private static void JsonLine(StringBuilder json, string key, string value, int indent, bool comma)
        {
            json.Append(new string(' ', indent * 2)).Append('"').Append(key).Append("\": \"").Append(Escape(value)).Append('"');
            if (comma) json.Append(',');
            json.AppendLine();
        }

        private static void JsonNumber(StringBuilder json, string key, int value, int indent, bool comma)
        {
            json.Append(new string(' ', indent * 2)).Append('"').Append(key).Append("\": ").Append(value.ToString(CultureInfo.InvariantCulture));
            if (comma) json.Append(',');
            json.AppendLine();
        }

        private static void WriteRecordArray(StringBuilder json, string key, string[] values, bool comma)
        {
            json.Append("  \"").Append(key).AppendLine("\": [");
            for (int i = 0; i < values.Length; i++)
            {
                json.Append("    \"").Append(Escape(values[i])).Append('"');
                if (i < values.Length - 1) json.Append(',');
                json.AppendLine();
            }
            json.Append("  ]");
            if (comma) json.Append(',');
            json.AppendLine();
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        private sealed class MaterialRecord
        {
            public string Path;
            public string Tag;
        }

        private sealed class MeshRecord
        {
            public string Path;
        }

        private sealed class TextureRecord
        {
            public string Path;
            public string Tag;
        }

        private sealed class PrefabRecord
        {
            public string Id;
            public string Path;
            public string Role;
            public string Notes;
            public string PreviewPath;
            public int RendererCount;
            public Vector3 Bounds;
        }

        private sealed class ValidationResult
        {
            public int PrefabCount;
            public int MaterialCount;
            public int MeshCount;
            public int TextureCount;
            public int PreviewCount;
            public int ColliderCount;
            public int RigidbodyCount;
            public int LightCount;
            public int AudioSourceCount;
            public int CameraCount;
            public int AnimatorCount;
            public int MonoBehaviourCount;
            public readonly List<string> Failures = new List<string>();
            public bool Passed => Failures.Count == 0;
        }
    }
}
