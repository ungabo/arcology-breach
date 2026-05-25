using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace BrassworksBreach.DoorVaultSet10.Editor
{
    public static class DoorVaultSet10Generator
    {
        private const string PackId = "DV10";
        private const string Version = "0.1.55";
        private const string BuildId = "p010";
        private const string PackageName = "com.brassworks.sidecar.door-vault-set10";
        private const string VersionFull = Version + "-" + BuildId;
        private const string PackageRoot = "Packages/" + PackageName;
        private const string RuntimeRoot = PackageRoot + "/Runtime";
        private const string MaterialRoot = RuntimeRoot + "/Materials";
        private const string MeshRoot = RuntimeRoot + "/Meshes";
        private const string TextureRoot = RuntimeRoot + "/Textures";
        private const string PrefabRoot = RuntimeRoot + "/Prefabs";
        private const string MetadataRoot = RuntimeRoot + "/Metadata";
        private const string ManifestRoot = PackageRoot + "/Documentation~/Manifest";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_55_DoorVaultSet10";
        private const string ProductionDocFolder = "Documentation/AssetProduction/V0_1_55_DoorVaultSet10";
        private const string PlanningDocFolder = "Documentation/Planning/V0_1_55_DoorVaultSet10ImportReadiness";
        private const string QaDocFolder = "Documentation/QA/V0_1_55_DoorVaultSet10ImportReadiness";
        private const int PreviewWidth = 1400;
        private const int PreviewHeight = 900;
        private const int ExpectedPrefabs = 9;
        private const int ExpectedMaterials = 8;
        private const int ExpectedMeshes = 9;
        private const int ExpectedTextures = 8;
        private const int ExpectedPreviewPngs = 10;

        private static readonly List<MaterialRecord> MaterialRecords = new List<MaterialRecord>();
        private static readonly List<MeshRecord> MeshRecords = new List<MeshRecord>();
        private static readonly List<TextureRecord> TextureRecords = new List<TextureRecord>();
        private static readonly List<ComponentRecord> ComponentRecords = new List<ComponentRecord>();

        private enum TextureStyle
        {
            Brass,
            Iron,
            Plate,
            Amber,
            Gauge,
            RedPaint,
            Verdigris,
            Gasket
        }

        private readonly struct MaterialSpec
        {
            public MaterialSpec(string key, string assetName, Color color, float metallic, float smoothness, Color emission, float alpha, TextureStyle style, string tag)
            {
                Key = key;
                AssetName = assetName;
                Color = color;
                Metallic = metallic;
                Smoothness = smoothness;
                Emission = emission;
                Alpha = alpha;
                Style = style;
                Tag = tag;
            }

            public readonly string Key;
            public readonly string AssetName;
            public readonly Color Color;
            public readonly float Metallic;
            public readonly float Smoothness;
            public readonly Color Emission;
            public readonly float Alpha;
            public readonly TextureStyle Style;
            public readonly string Tag;
        }

        private static readonly MaterialSpec[] MaterialSpecs =
        {
            new MaterialSpec("brass", "DV10_MAT_AgedBrassTrim", C(0.72f, 0.49f, 0.21f), 0.88f, 0.43f, Color.black, 1f, TextureStyle.Brass, "aged_brass_trim"),
            new MaterialSpec("iron", "DV10_MAT_BlackenedRivetedIron", C(0.070f, 0.066f, 0.058f), 0.86f, 0.25f, Color.black, 1f, TextureStyle.Iron, "blackened_riveted_iron"),
            new MaterialSpec("plate", "DV10_MAT_GunmetalSegmentPlate", C(0.24f, 0.25f, 0.24f), 0.76f, 0.27f, Color.black, 1f, TextureStyle.Plate, "gunmetal_segment_plate"),
            new MaterialSpec("amber", "DV10_MAT_AmberSideLampGlass", C(1.00f, 0.55f, 0.13f), 0.02f, 0.70f, C(1.30f, 0.55f, 0.12f), 0.82f, TextureStyle.Amber, "amber_side_lamp_glass"),
            new MaterialSpec("gauge", "DV10_MAT_IvoryGaugeEnamel", C(0.82f, 0.75f, 0.58f), 0.04f, 0.44f, Color.black, 1f, TextureStyle.Gauge, "ivory_gauge_enamel"),
            new MaterialSpec("red", "DV10_MAT_RedPressureMarkPaint", C(0.58f, 0.065f, 0.045f), 0.16f, 0.33f, Color.black, 1f, TextureStyle.RedPaint, "red_pressure_mark_paint"),
            new MaterialSpec("verdigris", "DV10_MAT_VerdigrisCopperPipe", C(0.17f, 0.52f, 0.44f), 0.62f, 0.31f, Color.black, 1f, TextureStyle.Verdigris, "verdigris_copper_pipe"),
            new MaterialSpec("gasket", "DV10_MAT_OilDarkRubberGasket", C(0.045f, 0.040f, 0.034f), 0.05f, 0.18f, Color.black, 1f, TextureStyle.Gasket, "oil_dark_rubber_gasket")
        };

        [MenuItem("Brassworks Breach/Sidecars/Door Vault Set 10/Generate Assets And Renders")]
        public static void GenerateAll()
        {
            MaterialRecords.Clear();
            MeshRecords.Clear();
            TextureRecords.Clear();
            ComponentRecords.Clear();

            EnsurePackageFolders();
            DeleteStaleCandidatePrefab();
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

            CreateRoundPressureDoor(meshes, materials);
            CreateGearWheelCenter(meshes, materials);
            CreateArchedFrame(meshes, materials);
            CreateSegmentedMetalPlates(meshes, materials);
            CreatePressureLocks(meshes, materials);
            CreateAmberSideLamps(meshes, materials);
            CreateThresholdTrims(meshes, materials);
            CreateSmallGaugeCluster(meshes, materials);
            CreateCandidateAssembly();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RenderPreviews(renderRoot);
            RenderContactSheet(renderRoot);
            WriteRenderIndex(renderRoot);

            ValidationResult validation = ValidateGeneratedContent(renderRoot);
            string manifestJson = BuildManifest(validation);
            WriteManifestCopies(manifestJson);
            WritePackageDocs();
            WriteDocumentation(repoRoot, renderRoot, productionRoot, planningRoot, qaRoot, validation);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (!validation.Passed)
            {
                throw new InvalidOperationException("DV10 validation failed: " + string.Join("; ", validation.Failures.ToArray()));
            }

            Debug.Log("DV10 generation pass: prefabs=" + validation.PrefabCount.ToString(CultureInfo.InvariantCulture)
                + " materials=" + validation.MaterialCount.ToString(CultureInfo.InvariantCulture)
                + " meshes=" + validation.MeshCount.ToString(CultureInfo.InvariantCulture)
                + " textures=" + validation.TextureCount.ToString(CultureInfo.InvariantCulture)
                + " previews=" + validation.PreviewCount.ToString(CultureInfo.InvariantCulture));
        }

        private static Dictionary<string, Texture2D> CreateTextures()
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>(StringComparer.Ordinal);
            string packagePath = PackagePhysicalRoot();

            foreach (MaterialSpec spec in MaterialSpecs)
            {
                string textureName = spec.AssetName.Replace("_MAT_", "_TEX_") + "_Base";
                string assetPath = TextureRoot + "/" + textureName + ".png";
                string absolutePath = Path.Combine(packagePath, "Runtime", "Textures", textureName + ".png");
                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath) ?? packagePath);

                Texture2D generated = BuildTexture(spec, 256, 256);
                File.WriteAllBytes(absolutePath, ImageConversion.EncodeToPNG(generated));
                Object.DestroyImmediate(generated);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Default;
                    importer.mipmapEnabled = true;
                    importer.sRGBTexture = true;
                    importer.wrapMode = TextureWrapMode.Repeat;
                    importer.filterMode = FilterMode.Bilinear;
                    importer.alphaSource = TextureImporterAlphaSource.FromInput;
                    importer.SaveAndReimport();
                }

                Texture2D imported = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                textures[spec.Key] = imported;
                TextureRecords.Add(new TextureRecord { Path = "Runtime/Textures/" + textureName + ".png", Tag = spec.Tag });
            }

            return textures;
        }

        private static Dictionary<string, Material> CreateMaterials(IReadOnlyDictionary<string, Texture2D> textures)
        {
            Dictionary<string, Material> result = new Dictionary<string, Material>(StringComparer.Ordinal);
            Shader shader = FindLitShader();

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

                SetColor(material, "_Color", WithAlpha(spec.Color, spec.Alpha));
                SetColor(material, "_BaseColor", WithAlpha(spec.Color, spec.Alpha));
                SetFloat(material, "_Metallic", spec.Metallic);
                SetFloat(material, "_Smoothness", spec.Smoothness);
                SetFloat(material, "_Glossiness", spec.Smoothness);

                if (textures.TryGetValue(spec.Key, out Texture2D texture))
                {
                    SetTexture(material, "_BaseMap", texture);
                    SetTexture(material, "_MainTex", texture);
                }

                if (spec.Emission.maxColorComponent > 0.01f)
                {
                    material.EnableKeyword("_EMISSION");
                    SetColor(material, "_EmissionColor", spec.Emission);
                }
                else
                {
                    material.DisableKeyword("_EMISSION");
                    SetColor(material, "_EmissionColor", Color.black);
                }

                material.SetOverrideTag("RenderType", "Opaque");
                SetFloat(material, "_Surface", 0f);
                SetFloat(material, "_ZWrite", 1f);
                material.renderQueue = -1;

                EditorUtility.SetDirty(material);
                result[spec.Key] = material;
                MaterialRecords.Add(new MaterialRecord { Path = "Runtime/Materials/" + spec.AssetName + ".mat", Tag = spec.Tag });
            }

            return result;
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>(StringComparer.Ordinal)
            {
                ["box"] = SaveMesh("BoxUnit", Box("DV10_MESH_BoxUnit", Vector3.one)),
                ["segment"] = SaveMesh("SegmentPlate", SegmentPlate("DV10_MESH_SegmentPlate")),
                ["cylinder16"] = SaveMesh("Cylinder16_Z", CylinderZ("DV10_MESH_Cylinder16_Z", 0.5f, 1f, 16)),
                ["cylinder32"] = SaveMesh("Cylinder32_Z", CylinderZ("DV10_MESH_Cylinder32_Z", 0.5f, 1f, 32)),
                ["cylinder48"] = SaveMesh("Cylinder48_Z", CylinderZ("DV10_MESH_Cylinder48_Z", 0.5f, 1f, 48)),
                ["bolt"] = SaveMesh("BoltHead6_Z", CylinderZ("DV10_MESH_BoltHead6_Z", 0.5f, 0.36f, 6)),
                ["gear"] = SaveMesh("GearRing32_Z", GearRing("DV10_MESH_GearRing32_Z", 32, 0.30f, 0.50f, 0.16f, 0.13f)),
                ["ring"] = SaveMesh("SmoothRing48_Z", GearRing("DV10_MESH_SmoothRing48_Z", 48, 0.36f, 0.50f, 0.10f, 0f)),
                ["needle"] = SaveMesh("GaugeNeedle", GaugeNeedle("DV10_MESH_GaugeNeedle"))
            };
            return meshes;
        }

        private static void CreateRoundPressureDoor(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("DV10_RoundPressureDoor_Standalone");
            Part(root.transform, meshes["cylinder48"], "blackened_round_pressure_door_slab", V3(0f, 0f, 0f), V3(4.10f, 4.10f, 0.22f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["cylinder48"], "gunmetal_recessed_inner_boss", V3(0f, 0f, -0.14f), V3(3.32f, 3.32f, 0.08f), Vector3.zero, materials["plate"]);
            Part(root.transform, meshes["ring"], "aged_brass_outer_pressure_ring", V3(0f, 0f, -0.23f), V3(4.42f, 4.42f, 0.18f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["ring"], "aged_brass_inner_service_ring", V3(0f, 0f, -0.29f), V3(2.70f, 2.70f, 0.12f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["cylinder32"], "oil_dark_rubber_pressure_gasket", V3(0f, 0f, -0.31f), V3(2.28f, 2.28f, 0.035f), Vector3.zero, materials["gasket"]);

            for (int i = 0; i < 12; i++)
            {
                float a = i * Mathf.PI * 2f / 12f;
                Vector3 pos = V3(Mathf.Sin(a) * 1.20f, Mathf.Cos(a) * 1.20f, -0.34f);
                Part(root.transform, meshes["segment"], "radial_segmented_plate_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.58f, 1.02f, 0.075f), V3(0f, 0f, -Mathf.Rad2Deg * a), materials[i % 2 == 0 ? "plate" : "iron"]);
            }

            for (int i = 0; i < 32; i++)
            {
                float a = i * Mathf.PI * 2f / 32f;
                Vector3 pos = V3(Mathf.Sin(a) * 2.02f, Mathf.Cos(a) * 2.02f, -0.43f);
                Part(root.transform, meshes["bolt"], "outer_brass_rivet_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.15f, 0.15f, 0.075f), Vector3.zero, materials["brass"]);
            }

            for (int i = 0; i < 16; i++)
            {
                float a = i * Mathf.PI * 2f / 16f;
                Vector3 pos = V3(Mathf.Sin(a) * 1.43f, Mathf.Cos(a) * 1.43f, -0.45f);
                Part(root.transform, meshes["bolt"], "inner_blackened_rivet_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.105f, 0.105f, 0.055f), Vector3.zero, materials["iron"]);
            }

            for (int i = 0; i < 8; i++)
            {
                float a = i * Mathf.PI * 2f / 8f;
                Vector3 pos = V3(Mathf.Sin(a) * 0.78f, Mathf.Cos(a) * 0.78f, -0.46f);
                Part(root.transform, meshes["box"], "thin_black_radial_seam_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.030f, 0.92f, 0.030f), V3(0f, 0f, -Mathf.Rad2Deg * a), materials["gasket"]);
            }

            SavePrefab(root, "round_pressure_door", "component-pass", "Riveted round pressure door slab with brass rings, rubber gasket, radial segmented metal plates, and readable rivet language.");
        }

        private static void CreateGearWheelCenter(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("DV10_GearWheelCenter_Standalone");
            Part(root.transform, meshes["gear"], "toothed_center_gear_wheel", V3(0f, 0f, -0.12f), V3(1.45f, 1.45f, 0.28f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["ring"], "blackened_handwheel_shadow_ring", V3(0f, 0f, -0.18f), V3(1.08f, 1.08f, 0.16f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["cylinder32"], "aged_brass_center_hub", V3(0f, 0f, -0.31f), V3(0.46f, 0.46f, 0.30f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["cylinder16"], "red_pressure_mark_cap", V3(0f, 0f, -0.49f), V3(0.23f, 0.23f, 0.045f), Vector3.zero, materials["red"]);

            for (int i = 0; i < 6; i++)
            {
                float a = i * Mathf.PI * 2f / 6f;
                Vector3 pos = V3(Mathf.Sin(a) * 0.40f, Mathf.Cos(a) * 0.40f, -0.34f);
                Part(root.transform, meshes["box"], "aged_brass_gear_spoke_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.11f, 0.86f, 0.10f), V3(0f, 0f, -Mathf.Rad2Deg * a), materials["brass"]);
            }

            for (int i = 0; i < 8; i++)
            {
                float a = i * Mathf.PI * 2f / 8f;
                Vector3 pos = V3(Mathf.Sin(a) * 0.63f, Mathf.Cos(a) * 0.63f, -0.47f);
                Part(root.transform, meshes["bolt"], "handwheel_service_bolt_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.082f, 0.082f, 0.045f), Vector3.zero, materials["iron"]);
            }

            SavePrefab(root, "gear_wheel_center", "component-pass", "Central toothed gear handwheel with six spokes, brass hub, pressure mark cap, and service bolts.");
        }

        private static void CreateArchedFrame(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("DV10_ArchedFrame_Standalone");
            Part(root.transform, meshes["box"], "left_riveted_vertical_frame_pillar", V3(-2.72f, -0.88f, 0.04f), V3(0.44f, 3.55f, 0.54f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "right_riveted_vertical_frame_pillar", V3(2.72f, -0.88f, 0.04f), V3(0.44f, 3.55f, 0.54f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "left_inner_brass_wear_strip", V3(-2.36f, -0.88f, -0.26f), V3(0.16f, 3.30f, 0.12f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "right_inner_brass_wear_strip", V3(2.36f, -0.88f, -0.26f), V3(0.16f, 3.30f, 0.12f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "bottom_blackened_sill_backer", V3(0f, -2.70f, 0.04f), V3(5.82f, 0.42f, 0.54f), Vector3.zero, materials["iron"]);

            for (int i = 0; i < 17; i++)
            {
                float t = i / 16f;
                float degrees = Mathf.Lerp(18f, 162f, t);
                float a = degrees * Mathf.Deg2Rad;
                Vector3 pos = V3(Mathf.Cos(a) * 2.42f, Mathf.Sin(a) * 1.70f + 0.36f, 0.04f);
                Part(root.transform, meshes["box"], "arched_frame_segment_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.42f, 0.78f, 0.58f), V3(0f, 0f, degrees - 90f), materials[i % 2 == 0 ? "iron" : "plate"]);
                Part(root.transform, meshes["bolt"], "arch_segment_brass_rivet_" + i.ToString("00", CultureInfo.InvariantCulture), pos + V3(0f, 0f, -0.34f), V3(0.105f, 0.105f, 0.055f), Vector3.zero, materials["brass"]);
            }

            Part(root.transform, meshes["cylinder16"], "left_verdigis_pressure_pipe", V3(-3.05f, -0.95f, -0.34f), V3(0.10f, 0.10f, 3.70f), V3(90f, 0f, 0f), materials["verdigris"]);
            Part(root.transform, meshes["cylinder16"], "right_verdigis_pressure_pipe", V3(3.05f, -0.95f, -0.34f), V3(0.10f, 0.10f, 3.70f), V3(90f, 0f, 0f), materials["verdigris"]);

            SavePrefab(root, "arched_frame", "component-pass", "Segmented arched corridor frame with heavy side pillars, brass wear strips, pressure pipes, bottom sill, and individual arch rivets.");
        }

        private static void CreateSegmentedMetalPlates(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("DV10_SegmentedMetalPlates_Standalone");
            Part(root.transform, meshes["cylinder48"], "circular_underplate_shadow", V3(0f, 0f, 0.05f), V3(3.55f, 3.55f, 0.08f), Vector3.zero, materials["iron"]);

            for (int i = 0; i < 16; i++)
            {
                float a = i * Mathf.PI * 2f / 16f;
                Vector3 pos = V3(Mathf.Sin(a) * 1.05f, Mathf.Cos(a) * 1.05f, -0.06f);
                Part(root.transform, meshes["segment"], "replaceable_radial_armor_plate_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.40f, 1.25f, 0.09f), V3(0f, 0f, -Mathf.Rad2Deg * a), materials[i % 3 == 0 ? "iron" : "plate"]);
            }

            for (int i = 0; i < 8; i++)
            {
                float a = i * Mathf.PI * 2f / 8f + Mathf.PI / 8f;
                Vector3 pos = V3(Mathf.Sin(a) * 1.58f, Mathf.Cos(a) * 1.58f, -0.13f);
                Part(root.transform, meshes["box"], "overlapping_outer_seam_strap_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.11f, 0.72f, 0.085f), V3(0f, 0f, -Mathf.Rad2Deg * a), materials["brass"]);
            }

            for (int i = 0; i < 24; i++)
            {
                float a = i * Mathf.PI * 2f / 24f;
                Vector3 pos = V3(Mathf.Sin(a) * 1.68f, Mathf.Cos(a) * 1.68f, -0.19f);
                Part(root.transform, meshes["bolt"], "segmented_plate_fastener_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(0.085f, 0.085f, 0.045f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(root, "segmented_metal_plates", "component-pass", "Standalone segmented metal plate language sheet for door skin and vault armor replacement panels.");
        }

        private static void CreatePressureLocks(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("DV10_PressureLocks_Standalone");
            for (int i = 0; i < 4; i++)
            {
                float a = i * Mathf.PI * 2f / 4f + Mathf.PI / 4f;
                Vector3 basePos = V3(Mathf.Sin(a) * 1.82f, Mathf.Cos(a) * 1.82f, -0.05f);
                float zRot = -Mathf.Rad2Deg * a;
                Part(root.transform, meshes["box"], "blackened_dog_lock_body_" + i.ToString("00", CultureInfo.InvariantCulture), basePos, V3(0.34f, 0.72f, 0.24f), V3(0f, 0f, zRot), materials["iron"]);
                Part(root.transform, meshes["box"], "aged_brass_lock_throw_arm_" + i.ToString("00", CultureInfo.InvariantCulture), basePos + V3(Mathf.Sin(a) * -0.38f, Mathf.Cos(a) * -0.38f, -0.19f), V3(0.17f, 0.96f, 0.12f), V3(0f, 0f, zRot), materials["brass"]);
                Part(root.transform, meshes["cylinder16"], "red_pressure_pin_" + i.ToString("00", CultureInfo.InvariantCulture), basePos + V3(Mathf.Sin(a) * 0.28f, Mathf.Cos(a) * 0.28f, -0.23f), V3(0.13f, 0.13f, 0.075f), Vector3.zero, materials["red"]);
            }

            Part(root.transform, meshes["cylinder32"], "left_pressure_lock_accumulator", V3(-1.22f, -2.18f, -0.05f), V3(0.32f, 0.32f, 0.86f), V3(0f, 90f, 0f), materials["verdigris"]);
            Part(root.transform, meshes["cylinder32"], "right_pressure_lock_accumulator", V3(1.22f, -2.18f, -0.05f), V3(0.32f, 0.32f, 0.86f), V3(0f, 90f, 0f), materials["verdigris"]);
            Part(root.transform, meshes["box"], "shared_pressure_lock_manifold", V3(0f, -2.18f, -0.05f), V3(2.05f, 0.20f, 0.22f), Vector3.zero, materials["iron"]);

            for (int i = 0; i < 10; i++)
            {
                float x = -0.90f + i * 0.20f;
                Part(root.transform, meshes["bolt"], "manifold_brass_bolt_" + i.ToString("00", CultureInfo.InvariantCulture), V3(x, -2.18f, -0.22f), V3(0.080f, 0.080f, 0.040f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(root, "pressure_locks", "component-pass", "Four heavy dog locks with brass throw arms, red pressure pins, bottom accumulator tanks, manifold, and bolt detail.");
        }

        private static void CreateAmberSideLamps(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("DV10_AmberSideLamps_Standalone");
            CreateLamp(root.transform, meshes, materials, V3(-0.68f, 0f, 0f), "left");
            CreateLamp(root.transform, meshes, materials, V3(0.68f, 0f, 0f), "right");
            Part(root.transform, meshes["cylinder16"], "upper_copper_lamp_feed_pipe", V3(0f, 0.88f, 0f), V3(0.075f, 0.075f, 1.44f), V3(0f, 90f, 0f), materials["verdigris"]);
            Part(root.transform, meshes["cylinder16"], "lower_copper_lamp_return_pipe", V3(0f, -0.88f, 0f), V3(0.075f, 0.075f, 1.44f), V3(0f, 90f, 0f), materials["verdigris"]);
            SavePrefab(root, "amber_side_lamps", "component-pass", "Paired amber side lamp housings with emissive glass material, brass cage ribs, iron backing plates, and copper feed pipes. No Light components are saved.");
        }

        private static void CreateThresholdTrims(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("DV10_ThresholdTrims_Standalone");
            Part(root.transform, meshes["box"], "blackened_floor_threshold_backer", V3(0f, 0f, 0.03f), V3(5.60f, 0.46f, 0.18f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["box"], "aged_brass_front_wear_strip", V3(0f, -0.18f, -0.08f), V3(5.70f, 0.10f, 0.10f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "aged_brass_rear_wear_strip", V3(0f, 0.18f, -0.08f), V3(5.70f, 0.10f, 0.10f), Vector3.zero, materials["brass"]);
            Part(root.transform, meshes["box"], "red_pressure_warning_inset", V3(0f, 0f, -0.12f), V3(2.10f, 0.075f, 0.04f), Vector3.zero, materials["red"]);

            for (int i = 0; i < 18; i++)
            {
                float x = -2.55f + i * 0.30f;
                Part(root.transform, meshes["bolt"], "threshold_left_line_rivet_" + i.ToString("00", CultureInfo.InvariantCulture), V3(x, -0.18f, -0.16f), V3(0.072f, 0.072f, 0.036f), Vector3.zero, materials["brass"]);
                Part(root.transform, meshes["bolt"], "threshold_right_line_rivet_" + i.ToString("00", CultureInfo.InvariantCulture), V3(x, 0.18f, -0.16f), V3(0.072f, 0.072f, 0.036f), Vector3.zero, materials["brass"]);
            }

            SavePrefab(root, "threshold_trims", "component-pass", "Heavy visual threshold trim for corridor placement with brass wear strips, red pressure warning inset, and repeated rivets.");
        }

        private static void CreateSmallGaugeCluster(IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            GameObject root = NewRoot("DV10_SmallGaugeCluster_Standalone");
            AddGauge(root.transform, meshes, materials, "upper_left_pressure_gauge", V3(-0.42f, 0.38f, -0.10f), 0.42f, -24f);
            AddGauge(root.transform, meshes, materials, "upper_right_pressure_gauge", V3(0.42f, 0.38f, -0.10f), 0.42f, 31f);
            AddGauge(root.transform, meshes, materials, "lower_master_pressure_gauge", V3(0f, -0.32f, -0.10f), 0.52f, 12f);
            Part(root.transform, meshes["box"], "blackened_gauge_cluster_mounting_plate", V3(0f, 0f, 0.04f), V3(1.55f, 1.42f, 0.12f), Vector3.zero, materials["iron"]);
            Part(root.transform, meshes["cylinder16"], "gauge_cluster_bottom_pipe_stub", V3(0f, -0.96f, -0.05f), V3(0.12f, 0.12f, 0.70f), V3(90f, 0f, 0f), materials["verdigris"]);
            SavePrefab(root, "small_gauge_cluster", "component-pass", "Three small readable pressure gauges on a blackened mount plate with brass bezels, ivory faces, red danger ticks, needles, and a pipe stub.");
        }

        private static void CreateCandidateAssembly()
        {
            GameObject root = NewRoot("DV10_CandidateAssembly_DoorVault_A");
            InstantiatePrefabPart(root.transform, "DV10_ArchedFrame_Standalone", V3(0f, -0.05f, 0.08f), Vector3.one, Vector3.zero);
            InstantiatePrefabPart(root.transform, "DV10_RoundPressureDoor_Standalone", V3(0f, -0.22f, -0.33f), Vector3.one, Vector3.zero);
            InstantiatePrefabPart(root.transform, "DV10_GearWheelCenter_Standalone", V3(0f, -0.22f, -0.86f), Vector3.one, Vector3.zero);
            InstantiatePrefabPart(root.transform, "DV10_PressureLocks_Standalone", V3(0f, -0.22f, -0.76f), Vector3.one, Vector3.zero);
            InstantiatePrefabPart(root.transform, "DV10_AmberSideLamps_Standalone", V3(0f, -0.30f, -0.62f), V3(1f, 1f, 1f), Vector3.zero);
            Transform lamps = root.transform.Find("DV10_AmberSideLamps_Standalone");
            if (lamps != null)
            {
                lamps.localScale = V3(1.0f, 1.12f, 1.0f);
                lamps.localPosition = V3(0f, -0.12f, -0.84f);
            }

            InstantiatePrefabPart(root.transform, "DV10_SmallGaugeCluster_Standalone", V3(2.05f, -0.92f, -0.82f), V3(0.70f, 0.70f, 0.70f), Vector3.zero);
            InstantiatePrefabPart(root.transform, "DV10_ThresholdTrims_Standalone", V3(0f, -3.06f, -0.64f), V3(1f, 1f, 1f), Vector3.zero);

            SavePrefab(root, "candidate_assembly", "candidate-pass", "Composed visual-only north-star corridor door and vault assembly using the standalone arched frame, round pressure door, gear wheel center, pressure locks, amber side lamps, gauges, and threshold trims.");
        }

        private static void CreateLamp(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, Vector3 center, string label)
        {
            Part(parent, meshes["box"], label + "_blackened_lamp_backplate", center + V3(0f, 0f, 0.06f), V3(0.42f, 1.18f, 0.14f), Vector3.zero, materials["iron"]);
            Part(parent, meshes["cylinder32"], label + "_amber_glass_lamp_tube", center + V3(0f, 0f, -0.10f), V3(0.30f, 0.30f, 0.58f), V3(90f, 0f, 0f), materials["amber"]);
            Part(parent, meshes["ring"], label + "_aged_brass_upper_lamp_bezel", center + V3(0f, 0.40f, -0.10f), V3(0.46f, 0.46f, 0.08f), V3(90f, 0f, 0f), materials["brass"]);
            Part(parent, meshes["ring"], label + "_aged_brass_lower_lamp_bezel", center + V3(0f, -0.40f, -0.10f), V3(0.46f, 0.46f, 0.08f), V3(90f, 0f, 0f), materials["brass"]);
            for (int i = 0; i < 4; i++)
            {
                float x = i < 2 ? -0.20f : 0.20f;
                float z = i % 2 == 0 ? -0.25f : 0.05f;
                Part(parent, meshes["box"], label + "_brass_lamp_cage_rib_" + i.ToString("00", CultureInfo.InvariantCulture), center + V3(x, 0f, z), V3(0.045f, 0.96f, 0.045f), Vector3.zero, materials["brass"]);
            }
        }

        private static void AddGauge(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, string prefix, Vector3 center, float diameter, float needleDegrees)
        {
            Part(parent, meshes["cylinder32"], prefix + "_ivory_dial_face", center + V3(0f, 0f, -0.09f), V3(diameter, diameter, 0.055f), Vector3.zero, materials["gauge"]);
            Part(parent, meshes["ring"], prefix + "_aged_brass_bezel", center + V3(0f, 0f, -0.13f), V3(diameter * 1.18f, diameter * 1.18f, 0.065f), Vector3.zero, materials["brass"]);
            Part(parent, meshes["cylinder32"], prefix + "_amber_glass_cover", center + V3(0f, 0f, -0.18f), V3(diameter * 0.92f, diameter * 0.92f, 0.020f), Vector3.zero, materials["amber"]);
            Part(parent, meshes["needle"], prefix + "_black_needle", center + V3(0f, 0f, -0.21f), V3(diameter * 0.47f, diameter * 0.055f, 0.030f), V3(0f, 0f, needleDegrees), materials["iron"]);

            for (int i = 0; i < 9; i++)
            {
                float degrees = Mathf.Lerp(-125f, 125f, i / 8f);
                float a = degrees * Mathf.Deg2Rad;
                Vector3 pos = center + V3(Mathf.Sin(a) * diameter * 0.38f, Mathf.Cos(a) * diameter * 0.38f, -0.22f);
                bool danger = i > 6;
                Part(parent, meshes["box"], prefix + "_dial_tick_" + i.ToString("00", CultureInfo.InvariantCulture), pos, V3(diameter * 0.022f, diameter * 0.12f, 0.018f), V3(0f, 0f, -degrees), danger ? materials["red"] : materials["iron"]);
            }
        }

        private static GameObject NewRoot(string name)
        {
            GameObject root = new GameObject(name);
            root.transform.position = Vector3.zero;
            root.transform.rotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            return root;
        }

        private static GameObject Part(Transform parent, Mesh mesh, string name, Vector3 position, Vector3 scale, Vector3 euler, Material material)
        {
            GameObject part = new GameObject(name);
            part.transform.SetParent(parent, false);
            part.transform.localPosition = position;
            part.transform.localRotation = Quaternion.Euler(euler);
            part.transform.localScale = scale;
            MeshFilter filter = part.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer = part.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            return part;
        }

        private static void InstantiatePrefabPart(Transform parent, string assetName, Vector3 position, Vector3 scale, Vector3 euler)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + assetName + ".prefab");
            if (prefab == null)
            {
                throw new FileNotFoundException("Missing prefab for candidate assembly: " + assetName);
            }

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance == null)
            {
                throw new InvalidOperationException("Could not instantiate prefab: " + assetName);
            }

            instance.transform.SetParent(parent, false);
            instance.transform.localPosition = position;
            instance.transform.localRotation = Quaternion.Euler(euler);
            instance.transform.localScale = scale;
        }

        private static void SavePrefab(GameObject root, string role, string status, string notes)
        {
            string assetName = root.name;
            string assetPath = PrefabRoot + "/" + assetName + ".prefab";
            ReplaceAsset(assetPath);
            PrefabUtility.SaveAsPrefabAsset(root, assetPath);
            ComponentRecord record = BuildComponentRecord(root, assetName, role, status, notes);
            ComponentRecords.Add(record);
            Object.DestroyImmediate(root);
        }

        private static ComponentRecord BuildComponentRecord(GameObject root, string assetName, string role, string status, string notes)
        {
            MeshRenderer[] renderers = root.GetComponentsInChildren<MeshRenderer>(true);
            MeshFilter[] filters = root.GetComponentsInChildren<MeshFilter>(true);
            Bounds bounds = CalculateBounds(root);
            return new ComponentRecord
            {
                Id = assetName,
                Role = role,
                AcceptanceStatus = status,
                Notes = notes,
                PrefabPath = "Runtime/Prefabs/" + assetName + ".prefab",
                PreviewPath = RenderDocFolder + "/" + PreviewNameFor(assetName),
                RendererCount = renderers.Length,
                MeshPartCount = filters.Length,
                BoundsSize = bounds.size
            };
        }

        private static void RenderPreviews(string renderRoot)
        {
            foreach (ComponentRecord record in ComponentRecords)
            {
                string prefabPath = PrefabRoot + "/" + record.Id + ".prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    throw new FileNotFoundException("Missing prefab before render: " + prefabPath);
                }

                string outputPath = Path.Combine(renderRoot, Path.GetFileName(record.PreviewPath));
                RenderPrefabPreview(prefab, outputPath);
            }
        }

        private static void RenderPrefabPreview(GameObject prefab, string outputPath)
        {
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance == null)
            {
                throw new InvalidOperationException("Could not instantiate preview prefab " + prefab.name);
            }

            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.identity;
            Bounds bounds = CalculateBounds(instance);
            float aspect = PreviewWidth / (float)PreviewHeight;
            float orthographicSize = Mathf.Max(bounds.extents.y, bounds.extents.x / aspect) * 1.24f;
            orthographicSize = Mathf.Max(orthographicSize, 0.72f);

            GameObject cameraObject = new GameObject("DV10_preview_camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.105f, 0.095f, 0.080f, 1f);
            camera.orthographic = true;
            camera.orthographicSize = orthographicSize;
            camera.nearClipPlane = 0.01f;
            camera.farClipPlane = 40f;
            camera.transform.position = V3(bounds.center.x, bounds.center.y, bounds.center.z - 9.0f);
            camera.transform.rotation = Quaternion.identity;

            GameObject keyLightObject = new GameObject("DV10_preview_key_light");
            Light keyLight = keyLightObject.AddComponent<Light>();
            keyLight.type = LightType.Directional;
            keyLight.intensity = 1.85f;
            keyLight.transform.rotation = Quaternion.Euler(35f, -30f, 0f);

            GameObject fillLightObject = new GameObject("DV10_preview_fill_light");
            Light fillLight = fillLightObject.AddComponent<Light>();
            fillLight.type = LightType.Directional;
            fillLight.intensity = 0.45f;
            fillLight.color = new Color(0.65f, 0.78f, 1.0f, 1f);
            fillLight.transform.rotation = Quaternion.Euler(-20f, 45f, 0f);

            Color previousAmbient = RenderSettings.ambientLight;
            AmbientMode previousAmbientMode = RenderSettings.ambientMode;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.28f, 0.24f, 0.19f, 1f);

            RenderTexture renderTexture = new RenderTexture(PreviewWidth, PreviewHeight, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = renderTexture;
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTexture;
            camera.Render();

            Texture2D output = new Texture2D(PreviewWidth, PreviewHeight, TextureFormat.RGBA32, false);
            output.ReadPixels(new Rect(0, 0, PreviewWidth, PreviewHeight), 0, 0);
            output.Apply();
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");
            File.WriteAllBytes(outputPath, ImageConversion.EncodeToPNG(output));

            RenderTexture.active = previous;
            camera.targetTexture = null;
            renderTexture.Release();
            Object.DestroyImmediate(renderTexture);
            Object.DestroyImmediate(output);
            Object.DestroyImmediate(cameraObject);
            Object.DestroyImmediate(keyLightObject);
            Object.DestroyImmediate(fillLightObject);
            Object.DestroyImmediate(instance);
            RenderSettings.ambientLight = previousAmbient;
            RenderSettings.ambientMode = previousAmbientMode;
        }

        private static void RenderContactSheet(string renderRoot)
        {
            string[] previewPaths = ComponentRecords.Select(r => Path.Combine(renderRoot, Path.GetFileName(r.PreviewPath))).ToArray();
            int columns = 3;
            int cellWidth = 600;
            int cellHeight = 420;
            int margin = 30;
            int rows = Mathf.CeilToInt(previewPaths.Length / (float)columns);
            int width = columns * cellWidth + (columns + 1) * margin;
            int height = rows * cellHeight + (rows + 1) * margin;
            Texture2D sheet = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Fill(sheet, new Color(0.10f, 0.09f, 0.075f, 1f));

            for (int i = 0; i < previewPaths.Length; i++)
            {
                if (!File.Exists(previewPaths[i]))
                {
                    continue;
                }

                Texture2D preview = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                ImageConversion.LoadImage(preview, File.ReadAllBytes(previewPaths[i]));
                int col = i % columns;
                int row = i / columns;
                int x = margin + col * (cellWidth + margin);
                int y = height - margin - (row + 1) * cellHeight - row * margin;
                PasteScaled(sheet, preview, x, y, cellWidth, cellHeight);
                Object.DestroyImmediate(preview);
            }

            sheet.Apply();
            File.WriteAllBytes(Path.Combine(renderRoot, "DV10_PREVIEW_contact-sheet.png"), ImageConversion.EncodeToPNG(sheet));
            Object.DestroyImmediate(sheet);
        }

        private static void WriteRenderIndex(string renderRoot)
        {
            StringBuilder md = new StringBuilder();
            md.AppendLine("# Door Vault Set 10 Preview Index");
            md.AppendLine();
            md.AppendLine("Visual-only preview renders generated by Unity from package prefabs.");
            md.AppendLine();
            foreach (ComponentRecord record in ComponentRecords)
            {
                md.AppendLine("- `" + Path.GetFileName(record.PreviewPath) + "` - `" + record.Id + "` (" + record.Role + ")");
            }
            md.AppendLine("- `DV10_PREVIEW_contact-sheet.png` - contact sheet");
            WriteText(Path.Combine(renderRoot, "DV10_RENDER_INDEX.md"), md.ToString());
        }

        private static ValidationResult ValidateGeneratedContent(string renderRoot)
        {
            ValidationResult result = new ValidationResult();
            result.PrefabCount = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot }).Length;
            result.MaterialCount = AssetDatabase.FindAssets("t:Material", new[] { MaterialRoot }).Length;
            result.MeshCount = AssetDatabase.FindAssets("t:Mesh", new[] { MeshRoot }).Length;
            result.TextureCount = Directory.Exists(Path.Combine(PackagePhysicalRoot(), "Runtime", "Textures"))
                ? Directory.GetFiles(Path.Combine(PackagePhysicalRoot(), "Runtime", "Textures"), "*.png", SearchOption.TopDirectoryOnly).Length
                : 0;
            result.PreviewCount = Directory.Exists(renderRoot)
                ? Directory.GetFiles(renderRoot, "*.png", SearchOption.TopDirectoryOnly).Length
                : 0;

            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    result.Failures.Add("Could not load prefab " + path);
                    continue;
                }

                result.ColliderCount += prefab.GetComponentsInChildren<Collider>(true).Length;
                result.RigidbodyCount += prefab.GetComponentsInChildren<Rigidbody>(true).Length;
                result.LightCount += prefab.GetComponentsInChildren<Light>(true).Length;
                result.AudioSourceCount += prefab.GetComponentsInChildren<AudioSource>(true).Length;
                result.CameraCount += prefab.GetComponentsInChildren<Camera>(true).Length;
                result.AnimatorCount += prefab.GetComponentsInChildren<Animator>(true).Length;
                result.MonoBehaviourCount += prefab.GetComponentsInChildren<MonoBehaviour>(true).Length;

                foreach (MeshFilter filter in prefab.GetComponentsInChildren<MeshFilter>(true))
                {
                    if (filter.sharedMesh == null)
                    {
                        result.Failures.Add("Missing mesh on " + path + "/" + filter.name);
                    }
                }

                foreach (MeshRenderer renderer in prefab.GetComponentsInChildren<MeshRenderer>(true))
                {
                    if (renderer.sharedMaterials == null || renderer.sharedMaterials.Length == 0 || renderer.sharedMaterials.Any(m => m == null))
                    {
                        result.Failures.Add("Missing material on " + path + "/" + renderer.name);
                    }
                }
            }

            string[] csFiles = Directory.GetFiles(PackagePhysicalRoot(), "*.cs", SearchOption.AllDirectories);
            foreach (string csFile in csFiles)
            {
                string normalized = csFile.Replace("\\", "/");
                if (!normalized.Contains("/Editor/"))
                {
                    result.Failures.Add("Non-editor script found: " + ToRepoRelative(csFile));
                }
            }

            if (result.PrefabCount != ExpectedPrefabs) result.Failures.Add("Expected " + ExpectedPrefabs + " prefabs, found " + result.PrefabCount);
            if (result.MaterialCount != ExpectedMaterials) result.Failures.Add("Expected " + ExpectedMaterials + " materials, found " + result.MaterialCount);
            if (result.MeshCount != ExpectedMeshes) result.Failures.Add("Expected " + ExpectedMeshes + " meshes, found " + result.MeshCount);
            if (result.TextureCount != ExpectedTextures) result.Failures.Add("Expected " + ExpectedTextures + " texture PNGs, found " + result.TextureCount);
            if (result.PreviewCount != ExpectedPreviewPngs) result.Failures.Add("Expected " + ExpectedPreviewPngs + " preview PNGs, found " + result.PreviewCount);
            if (result.ColliderCount != 0) result.Failures.Add("Collider components found: " + result.ColliderCount);
            if (result.RigidbodyCount != 0) result.Failures.Add("Rigidbody components found: " + result.RigidbodyCount);
            if (result.LightCount != 0) result.Failures.Add("Light components found in prefabs: " + result.LightCount);
            if (result.AudioSourceCount != 0) result.Failures.Add("AudioSource components found: " + result.AudioSourceCount);
            if (result.CameraCount != 0) result.Failures.Add("Camera components found in prefabs: " + result.CameraCount);
            if (result.AnimatorCount != 0) result.Failures.Add("Animator components found: " + result.AnimatorCount);
            if (result.MonoBehaviourCount != 0) result.Failures.Add("MonoBehaviour components found in prefabs: " + result.MonoBehaviourCount);

            return result;
        }

        private static string BuildManifest(ValidationResult validation)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            Line(json, "pack_id", PackId, 1, true);
            Line(json, "display_name", "Door Vault Set 10", 1, true);
            Line(json, "version", Version, 1, true);
            Line(json, "build_id", BuildId, 1, true);
            Line(json, "package_name", PackageName, 1, true);
            Line(json, "common_schema", "brassworks.sidecar.visual_pack_manifest.v1", 1, true);
            Line(json, "unity_version", Application.unityVersion, 1, true);
            Line(json, "generated_at_utc", "2026-05-25T00:00:00Z", 1, true);
            Line(json, "owner_lane", "Worker DoorVaultSet10", 1, true);
            Line(json, "canonical_root", "AssetPacks/BrassworksBreach.DoorVaultSet10", 1, true);
            Line(json, "package_root", "AssetPacks/BrassworksBreach.DoorVaultSet10", 1, true);
            Line(json, "render_root", RenderDocFolder, 1, true);
            json.AppendLine("  \"asset_counts\": {");
            NumLine(json, "generated_prefabs", validation.PrefabCount, 2, true);
            NumLine(json, "generated_materials", validation.MaterialCount, 2, true);
            NumLine(json, "generated_meshes", validation.MeshCount, 2, true);
            NumLine(json, "runtime_texture_pngs", validation.TextureCount, 2, true);
            NumLine(json, "preview_pngs", validation.PreviewCount, 2, true);
            NumLine(json, "runtime_scripts", 0, 2, true);
            NumLine(json, "colliders", validation.ColliderCount, 2, true);
            NumLine(json, "rigidbodies", validation.RigidbodyCount, 2, true);
            NumLine(json, "lights_in_prefabs", validation.LightCount, 2, true);
            NumLine(json, "audio_sources", validation.AudioSourceCount, 2, false);
            json.AppendLine("  },");
            ArrayLine(json, "visual_contract", new[]
            {
                "visual_only_no_gameplay_authority",
                "unity_procedural_meshes_only",
                "no_blender_or_external_dcc",
                "no_runtime_scripts",
                "no_colliders_or_rigidbodies",
                "no_lights_cameras_audio_or_animators_in_prefabs",
                "no_scene_dependencies"
            }, 1, true);
            ArrayLine(json, "dependencies", new[]
            {
                "Unity Package Manager local package reference",
                "Unity built-in mesh/material/texture APIs",
                "Unity camera render path for documentation previews only"
            }, 1, true);
            WriteComponentArray(json);
            WritePathArray(json, "generated_materials", MaterialRecords.Select(r => "Packages/" + PackageName + "/" + r.Path).ToArray(), true);
            WritePathArray(json, "generated_meshes", MeshRecords.Select(r => "Packages/" + PackageName + "/" + r.Path).ToArray(), true);
            WritePathArray(json, "generated_texture_pngs", TextureRecords.Select(r => "Packages/" + PackageName + "/" + r.Path).ToArray(), true);
            WritePathArray(json, "preview_renders", ComponentRecords.Select(r => r.PreviewPath).Concat(new[] { RenderDocFolder + "/DV10_PREVIEW_contact-sheet.png" }).ToArray(), true);
            ArrayLine(json, "acceptance_gates", new[]
            {
                "Nine visual-only prefabs exist: eight standalone modules and one candidate assembly.",
                "Riveted round pressure door, gear wheel center, arched frame, segmented plates, pressure locks, amber side lamps, threshold trims, and small gauges are represented.",
                "Every prefab contains only Transform, MeshFilter, MeshRenderer, and prefab instance data.",
                "No colliders, rigidbodies, runtime scripts, lights, cameras, audio sources, animator controllers, animation clips, scenes, or gameplay authority are included.",
                "Procedural material texture PNGs are present and assigned through package materials.",
                "Preview PNGs and contact sheet are present in the assigned ConceptRenders root.",
                "Runtime and Documentation manifest copies are normalized and identical."
            }, 1, true);
            ArrayLine(json, "known_risks", new[]
            {
                "Meshes are procedural Unity lookdev geometry intended for visual staging and may need final LOD/lightmap ownership before promotion.",
                "Amber lamps use emissive materials only; actual lighting remains main-lane scene authority.",
                "Vault function, collision, door motion, locks, sounds, and gameplay state are intentionally absent.",
                "Transparent glass material may need render-pipeline tuning during primary quarantine import."
            }, 1, true);
            json.AppendLine("  \"validation\": {");
            Line(json, "status", validation.Passed ? "pass" : "fail", 2, true);
            NumLine(json, "collider_count", validation.ColliderCount, 2, true);
            NumLine(json, "rigidbody_count", validation.RigidbodyCount, 2, true);
            NumLine(json, "mono_behaviour_count", validation.MonoBehaviourCount, 2, true);
            NumLine(json, "light_count", validation.LightCount, 2, true);
            NumLine(json, "audio_source_count", validation.AudioSourceCount, 2, true);
            NumLine(json, "camera_count", validation.CameraCount, 2, true);
            NumLine(json, "animator_count", validation.AnimatorCount, 2, true);
            ArrayLine(json, "failures", validation.Failures.ToArray(), 2, false);
            json.AppendLine("  },");
            Line(json, "rollback_path", "remove local package reference " + PackageName + " and delete isolated package root", 1, true);
            Line(json, "decision", validation.Passed ? "ready_for_primary_quarantine_preview_review" : "blocked_pending_validation_fix", 1, false);
            json.AppendLine("}");
            return json.ToString();
        }

        private static void WriteComponentArray(StringBuilder json)
        {
            json.AppendLine("  \"components\": [");
            for (int i = 0; i < ComponentRecords.Count; i++)
            {
                ComponentRecord record = ComponentRecords[i];
                json.AppendLine("    {");
                Line(json, "id", record.Id, 3, true);
                Line(json, "role", record.Role, 3, true);
                Line(json, "acceptance_status", record.AcceptanceStatus, 3, true);
                Line(json, "prefab", "Packages/" + PackageName + "/" + record.PrefabPath, 3, true);
                Line(json, "preview", record.PreviewPath, 3, true);
                NumLine(json, "renderer_count", record.RendererCount, 3, true);
                NumLine(json, "mesh_part_count", record.MeshPartCount, 3, true);
                json.AppendLine("      \"bounds_size_meters\": {");
                FloatLine(json, "x", record.BoundsSize.x, 4, true);
                FloatLine(json, "y", record.BoundsSize.y, 4, true);
                FloatLine(json, "z", record.BoundsSize.z, 4, false);
                json.AppendLine("      },");
                Line(json, "notes", record.Notes, 3, false);
                json.Append("    }");
                if (i < ComponentRecords.Count - 1) json.Append(",");
                json.AppendLine();
            }

            json.AppendLine("  ],");
        }

        private static void WritePathArray(StringBuilder json, string key, string[] values, bool comma)
        {
            json.Append("  \"").Append(key).AppendLine("\": [");
            for (int i = 0; i < values.Length; i++)
            {
                json.Append("    \"").Append(Escape(values[i])).Append("\"");
                if (i < values.Length - 1) json.Append(",");
                json.AppendLine();
            }

            json.Append("  ]");
            if (comma) json.Append(",");
            json.AppendLine();
        }

        private static void WriteManifestCopies(string manifestJson)
        {
            string packagePath = PackagePhysicalRoot();
            string runtimeManifest = Path.Combine(packagePath, "Runtime", "Metadata", "DV10_DoorVaultSet10_Manifest_v0.1.55-p010.json");
            string documentationManifest = Path.Combine(packagePath, "Documentation~", "Manifest", "DV10_DoorVaultSet10_Manifest_v0.1.55-p010.json");
            WriteText(runtimeManifest, manifestJson);
            WriteText(documentationManifest, manifestJson);
            AssetDatabase.ImportAsset(MetadataRoot + "/DV10_DoorVaultSet10_Manifest_v0.1.55-p010.json", ImportAssetOptions.ForceUpdate);
        }

        private static void WriteDocumentation(string repoRoot, string renderRoot, string productionRoot, string planningRoot, string qaRoot, ValidationResult validation)
        {
            WriteText(Path.Combine(productionRoot, "README.md"), BuildProductionReadme());
            WriteText(Path.Combine(productionRoot, "DV10_ProductionReport_0.1.55-p010.md"), BuildProductionReport(validation));
            WriteText(Path.Combine(productionRoot, "DV10_AssetInventory_0.1.55-p010.md"), BuildAssetInventory());
            WriteText(Path.Combine(productionRoot, "DV10_FINAL_FILE_LIST.md"), BuildFinalFileList(repoRoot));
            WriteText(Path.Combine(planningRoot, "DV10_IMPORT_READINESS_PLAN.md"), BuildImportReadinessPlan());
            WriteText(Path.Combine(qaRoot, "DV10_QA_CHECKLIST.md"), BuildQaChecklist(validation));
            WriteText(Path.Combine(qaRoot, "DV10_STATIC_VALIDATION.md"), BuildStaticValidation(validation));
        }

        private static void WritePackageDocs()
        {
            string packagePath = PackagePhysicalRoot();
            WriteText(Path.Combine(packagePath, "README.md"), "# Door Vault Set 10\n\nUnity-only visual sidecar package for the north-star steampunk corridor door and vault assembly.\n\nAssets are visual-only: no runtime scripts, colliders, rigidbodies, lights, cameras, audio, animation controllers, scenes, or gameplay authority are saved in prefabs. The only script is the editor-only generator under `Editor`.\n\nPreview PNGs and the contact sheet are generated to `Documentation/ConceptRenders/V0_1_55_DoorVaultSet10`.\n\n## Contents\n\n- `Runtime/Prefabs`: standalone modules and one candidate assembly.\n- `Runtime/Materials`: brass, iron, gunmetal, amber glass, gauge enamel, red pressure paint, verdigris pipe, and oil-dark gasket materials.\n- `Runtime/Textures`: generated base texture PNGs used by the materials.\n- `Runtime/Meshes`: reusable procedural Unity mesh assets.\n- `Runtime/Metadata`: normalized manifest copy.\n- `Documentation~/Manifest`: import-facing normalized manifest copy.\n\n## Rebuild\n\nOpen the validation project or a quarantine project with this package referenced, then run:\n\n`Brassworks Breach/Sidecars/Door Vault Set 10/Generate Assets And Renders`\n");
            WriteText(Path.Combine(packagePath, "CHANGELOG.md"), "# Changelog\n\n## 0.1.55-p010\n\n- Added isolated Door Vault Set 10 visual sidecar package.\n- Generated visual-only prefabs for pressure door, gear wheel center, arched frame, segmented plates, pressure locks, amber side lamps, threshold trims, small gauges, and candidate assembly.\n- Added generated materials, meshes, textures, preview PNGs, contact sheet, normalized manifest, import readiness docs, and QA checklist.\n");
            WriteText(Path.Combine(packagePath, "Samples~", "PreviewScene", "README.md"), "# Door Vault Set 10 Preview Notes\n\nThis sample folder intentionally contains notes only. Use the preview PNGs in `Documentation/ConceptRenders/V0_1_55_DoorVaultSet10` for visual review. Prefabs remain visual-only and do not contain lights, cameras, colliders, rigidbodies, audio, runtime scripts, animation controllers, or gameplay authority.\n");
        }

        private static string BuildProductionReadme()
        {
            return "# Door Vault Set 10 Asset Production\n\nWorker: DoorVaultSet10\n\nScope: isolated Unity-only visual sidecar for the north-star steampunk corridor door/vault assembly. Outputs live only under the assigned package and documentation roots.\n\nPrimary deliverables:\n\n- Package root: `AssetPacks/BrassworksBreach.DoorVaultSet10`\n- Concept renders: `Documentation/ConceptRenders/V0_1_55_DoorVaultSet10`\n- Planning packet: `Documentation/Planning/V0_1_55_DoorVaultSet10ImportReadiness`\n- QA packet: `Documentation/QA/V0_1_55_DoorVaultSet10ImportReadiness`\n\nNo Blender or external DCC was used. Meshes, materials, textures, prefabs, and renders are generated through Unity editor APIs.\n";
        }

        private static string BuildProductionReport(ValidationResult validation)
        {
            StringBuilder md = new StringBuilder();
            md.AppendLine("# DV10 Production Report 0.1.55-p010");
            md.AppendLine();
            md.AppendLine("## Result");
            md.AppendLine();
            md.AppendLine("- Status: `" + (validation.Passed ? "pass" : "fail") + "`");
            md.AppendLine("- Prefabs: `" + validation.PrefabCount + "`");
            md.AppendLine("- Materials: `" + validation.MaterialCount + "`");
            md.AppendLine("- Meshes: `" + validation.MeshCount + "`");
            md.AppendLine("- Runtime texture PNGs: `" + validation.TextureCount + "`");
            md.AppendLine("- Preview PNGs: `" + validation.PreviewCount + "`");
            md.AppendLine();
            md.AppendLine("## Visual Notes");
            md.AppendLine();
            md.AppendLine("- Door reads as a riveted round pressure/vault door with brass pressure rings and segmented metal plates.");
            md.AppendLine("- Center gear wheel gives a mechanical lock focal point without defining gameplay authority.");
            md.AppendLine("- Arched frame, pressure locks, lamps, gauges, and threshold trim are separate modules for quarantine review.");
            md.AppendLine("- Amber side lamps use emissive material only; actual scene lighting is left to the main lane.");
            md.AppendLine();
            md.AppendLine("## Restrictions Honored");
            md.AppendLine();
            md.AppendLine("- No runtime scripts.");
            md.AppendLine("- No colliders or rigidbodies.");
            md.AppendLine("- No lights, cameras, audio sources, animators, animation clips, or scenes in prefabs.");
            md.AppendLine("- No Blender or external DCC assets.");
            return md.ToString();
        }

        private static string BuildAssetInventory()
        {
            StringBuilder md = new StringBuilder();
            md.AppendLine("# DV10 Asset Inventory 0.1.55-p010");
            md.AppendLine();
            md.AppendLine("## Prefabs");
            foreach (ComponentRecord record in ComponentRecords)
            {
                md.AppendLine("- `" + record.PrefabPath + "` - " + record.Role + ", renderers `" + record.RendererCount + "`");
            }
            md.AppendLine();
            md.AppendLine("## Materials");
            foreach (MaterialRecord record in MaterialRecords)
            {
                md.AppendLine("- `" + record.Path + "` - " + record.Tag);
            }
            md.AppendLine();
            md.AppendLine("## Meshes");
            foreach (MeshRecord record in MeshRecords)
            {
                md.AppendLine("- `" + record.Path + "`");
            }
            md.AppendLine();
            md.AppendLine("## Textures");
            foreach (TextureRecord record in TextureRecords)
            {
                md.AppendLine("- `" + record.Path + "` - " + record.Tag);
            }
            md.AppendLine();
            md.AppendLine("## Renders");
            foreach (ComponentRecord record in ComponentRecords)
            {
                md.AppendLine("- `" + record.PreviewPath + "`");
            }
            md.AppendLine("- `" + RenderDocFolder + "/DV10_PREVIEW_contact-sheet.png`");
            return md.ToString();
        }

        private static string BuildFinalFileList(string repoRoot)
        {
            string[] roots =
            {
                Path.Combine(repoRoot, "AssetPacks", "BrassworksBreach.DoorVaultSet10"),
                Path.Combine(repoRoot, ProductionDocFolder.Replace("/", Path.DirectorySeparatorChar.ToString())),
                Path.Combine(repoRoot, RenderDocFolder.Replace("/", Path.DirectorySeparatorChar.ToString())),
                Path.Combine(repoRoot, PlanningDocFolder.Replace("/", Path.DirectorySeparatorChar.ToString())),
                Path.Combine(repoRoot, QaDocFolder.Replace("/", Path.DirectorySeparatorChar.ToString()))
            };

            List<string> files = new List<string>();
            foreach (string root in roots)
            {
                if (!Directory.Exists(root))
                {
                    continue;
                }

                files.AddRange(Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                    .Where(p => !p.Replace("\\", "/").Contains("/Library/"))
                    .Where(p => !p.Replace("\\", "/").Contains("/Temp/"))
                    .Where(p => !p.Replace("\\", "/").Contains("/Logs/"))
                    .Where(p => !p.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                    .Select(ToRepoRelative));
            }

            files = files.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(p => p, StringComparer.OrdinalIgnoreCase).ToList();
            StringBuilder md = new StringBuilder();
            md.AppendLine("# DV10 Final File List");
            md.AppendLine();
            md.AppendLine("Primary non-meta deliverables generated under assigned roots.");
            md.AppendLine();
            foreach (string file in files)
            {
                md.AppendLine("- `" + file + "`");
            }

            return md.ToString();
        }

        private static string BuildImportReadinessPlan()
        {
            return "# DV10 Import Readiness Plan\n\n## Package\n\n- Package name: `com.brassworks.sidecar.door-vault-set10`\n- Local package root: `AssetPacks/BrassworksBreach.DoorVaultSet10`\n- Recommended intake: add as local package in a quarantine integration branch, review previews, then promote selected visual prefabs.\n\n## Import Notes\n\n- Treat all prefabs as visual modules only.\n- Do not infer collision, door motion, interaction, lock state, lighting, audio, or gameplay authority from this package.\n- Candidate assembly is a composition preview for art direction and placement discussion.\n- Amber side lamps provide emissive material color only; any real Light components belong in scene or gameplay integration work.\n- Preview PNGs are documentation evidence and should not be imported as runtime textures.\n\n## Rollback\n\nRemove the local package reference `com.brassworks.sidecar.door-vault-set10` and delete the isolated package root if quarantine review rejects the asset.\n";
        }

        private static string BuildQaChecklist(ValidationResult validation)
        {
            StringBuilder md = new StringBuilder();
            md.AppendLine("# DV10 QA Checklist");
            md.AppendLine();
            md.AppendLine("## Static Validation");
            md.AppendLine();
            md.AppendLine("- [" + (validation.PrefabCount == ExpectedPrefabs ? "x" : " ") + "] " + ExpectedPrefabs + " visual-only prefabs present.");
            md.AppendLine("- [" + (validation.MaterialCount == ExpectedMaterials ? "x" : " ") + "] " + ExpectedMaterials + " material assets present.");
            md.AppendLine("- [" + (validation.MeshCount == ExpectedMeshes ? "x" : " ") + "] " + ExpectedMeshes + " reusable mesh assets present.");
            md.AppendLine("- [" + (validation.TextureCount == ExpectedTextures ? "x" : " ") + "] " + ExpectedTextures + " runtime texture PNGs present.");
            md.AppendLine("- [" + (validation.PreviewCount == ExpectedPreviewPngs ? "x" : " ") + "] Preview PNGs plus contact sheet present.");
            md.AppendLine("- [" + (validation.ColliderCount == 0 ? "x" : " ") + "] No colliders in prefabs.");
            md.AppendLine("- [" + (validation.RigidbodyCount == 0 ? "x" : " ") + "] No rigidbodies in prefabs.");
            md.AppendLine("- [" + (validation.MonoBehaviourCount == 0 ? "x" : " ") + "] No MonoBehaviours or runtime scripts in prefabs.");
            md.AppendLine("- [" + (validation.LightCount == 0 ? "x" : " ") + "] No Light components in prefabs.");
            md.AppendLine("- [" + (validation.AudioSourceCount == 0 ? "x" : " ") + "] No AudioSource components in prefabs.");
            md.AppendLine("- [" + (validation.CameraCount == 0 ? "x" : " ") + "] No Camera components in prefabs.");
            md.AppendLine("- [" + (validation.AnimatorCount == 0 ? "x" : " ") + "] No Animator components in prefabs.");
            md.AppendLine();
            md.AppendLine("## Art Review");
            md.AppendLine();
            md.AppendLine("- [x] Steampunk material language: brass, blackened iron, verdigris copper, pressure glass, ivory gauges, red pressure marks.");
            md.AppendLine("- [x] Required door/vault motifs represented.");
            md.AppendLine("- [x] Candidate assembly reads as a corridor vault door from a first-pass play distance.");
            md.AppendLine("- [ ] Main-lane reviewer to approve final staging scale and placement.");
            return md.ToString();
        }

        private static string BuildStaticValidation(ValidationResult validation)
        {
            StringBuilder md = new StringBuilder();
            md.AppendLine("# DV10 Static Validation");
            md.AppendLine();
            md.AppendLine("- Status: `" + (validation.Passed ? "pass" : "fail") + "`");
            md.AppendLine("- Collider count: `" + validation.ColliderCount + "`");
            md.AppendLine("- Rigidbody count: `" + validation.RigidbodyCount + "`");
            md.AppendLine("- MonoBehaviour count in prefabs: `" + validation.MonoBehaviourCount + "`");
            md.AppendLine("- Light count in prefabs: `" + validation.LightCount + "`");
            md.AppendLine("- AudioSource count: `" + validation.AudioSourceCount + "`");
            md.AppendLine("- Camera count: `" + validation.CameraCount + "`");
            md.AppendLine("- Animator count: `" + validation.AnimatorCount + "`");
            md.AppendLine();
            if (validation.Failures.Count == 0)
            {
                md.AppendLine("No validation failures were reported by the Unity generator.");
            }
            else
            {
                md.AppendLine("## Failures");
                foreach (string failure in validation.Failures)
                {
                    md.AppendLine("- " + failure);
                }
            }
            return md.ToString();
        }

        private static Mesh SaveMesh(string shortName, Mesh mesh)
        {
            string assetPath = MeshRoot + "/" + mesh.name + ".asset";
            ReplaceAsset(assetPath);
            AssetDatabase.CreateAsset(mesh, assetPath);
            MeshRecords.Add(new MeshRecord { Path = "Runtime/Meshes/" + mesh.name + ".asset" });
            return mesh;
        }

        private static Mesh Box(string name, Vector3 size)
        {
            Vector3 h = size * 0.5f;
            Vector3[] corners =
            {
                V3(-h.x, -h.y, -h.z), V3(h.x, -h.y, -h.z), V3(h.x, h.y, -h.z), V3(-h.x, h.y, -h.z),
                V3(-h.x, -h.y, h.z), V3(h.x, -h.y, h.z), V3(h.x, h.y, h.z), V3(-h.x, h.y, h.z)
            };
            int[] faces = { 0, 3, 2, 1, 5, 6, 7, 4, 4, 7, 3, 0, 1, 2, 6, 5, 3, 7, 6, 2, 4, 0, 1, 5 };
            Vector3[] normals = { Vector3.back, Vector3.forward, Vector3.left, Vector3.right, Vector3.up, Vector3.down };
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normalList = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int face = 0; face < 6; face++)
            {
                int start = vertices.Count;
                for (int i = 0; i < 4; i++)
                {
                    vertices.Add(corners[faces[face * 4 + i]]);
                    normalList.Add(normals[face]);
                }

                triangles.AddRange(new[] { start, start + 1, start + 2, start, start + 2, start + 3 });
            }

            return MeshFrom(name, vertices, triangles, normalList);
        }

        private static Mesh SegmentPlate(string name)
        {
            Vector2[] outline =
            {
                new Vector2(-0.42f, -0.50f),
                new Vector2(0.42f, -0.50f),
                new Vector2(0.30f, 0.50f),
                new Vector2(-0.30f, 0.50f)
            };
            return ExtrudedPolygon(name, outline, 1f);
        }

        private static Mesh CylinderZ(string name, float radius, float thickness, int segments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float z0 = -thickness * 0.5f;
            float z1 = thickness * 0.5f;

            for (int i = 0; i < segments; i++)
            {
                float a0 = i / (float)segments * Mathf.PI * 2f;
                float a1 = (i + 1) / (float)segments * Mathf.PI * 2f;
                Vector3 p0 = V3(Mathf.Cos(a0) * radius, Mathf.Sin(a0) * radius, z0);
                Vector3 p1 = V3(Mathf.Cos(a1) * radius, Mathf.Sin(a1) * radius, z0);
                Vector3 p2 = V3(Mathf.Cos(a1) * radius, Mathf.Sin(a1) * radius, z1);
                Vector3 p3 = V3(Mathf.Cos(a0) * radius, Mathf.Sin(a0) * radius, z1);
                int s = vertices.Count;
                vertices.AddRange(new[] { p0, p1, p2, p3 });
                triangles.AddRange(new[] { s, s + 1, s + 2, s, s + 2, s + 3 });

                int front = vertices.Count;
                vertices.AddRange(new[] { Vector3.forward * z1, p3, p2 });
                triangles.AddRange(new[] { front, front + 1, front + 2 });

                int back = vertices.Count;
                vertices.AddRange(new[] { Vector3.forward * z0, p1, p0 });
                triangles.AddRange(new[] { back, back + 1, back + 2 });
            }

            return MeshFrom(name, vertices, triangles, null);
        }

        private static Mesh GearRing(string name, int teeth, float innerRadius, float outerRadius, float thickness, float toothDepth)
        {
            int segments = Mathf.Max(8, teeth * 2);
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float z0 = -thickness * 0.5f;
            float z1 = thickness * 0.5f;

            for (int i = 0; i < segments; i++)
            {
                float a0 = i / (float)segments * Mathf.PI * 2f;
                float a1 = (i + 1) / (float)segments * Mathf.PI * 2f;
                float r0 = outerRadius + ((i % 2 == 0) ? toothDepth : 0f);
                float r1 = outerRadius + (((i + 1) % 2 == 0) ? toothDepth : 0f);
                Vector3 o0f = V3(Mathf.Cos(a0) * r0, Mathf.Sin(a0) * r0, z0);
                Vector3 o1f = V3(Mathf.Cos(a1) * r1, Mathf.Sin(a1) * r1, z0);
                Vector3 i0f = V3(Mathf.Cos(a0) * innerRadius, Mathf.Sin(a0) * innerRadius, z0);
                Vector3 i1f = V3(Mathf.Cos(a1) * innerRadius, Mathf.Sin(a1) * innerRadius, z0);
                Vector3 o0b = V3(o0f.x, o0f.y, z1);
                Vector3 o1b = V3(o1f.x, o1f.y, z1);
                Vector3 i0b = V3(i0f.x, i0f.y, z1);
                Vector3 i1b = V3(i1f.x, i1f.y, z1);
                int s = vertices.Count;
                vertices.AddRange(new[] { o0f, o1f, i1f, i0f, o0b, i0b, i1b, o1b, o0f, o0b, o1b, o1f, i0f, i1f, i1b, i0b });
                triangles.AddRange(new[] { s, s + 1, s + 2, s, s + 2, s + 3 });
                triangles.AddRange(new[] { s + 4, s + 5, s + 6, s + 4, s + 6, s + 7 });
                triangles.AddRange(new[] { s + 8, s + 9, s + 10, s + 8, s + 10, s + 11 });
                triangles.AddRange(new[] { s + 12, s + 13, s + 14, s + 12, s + 14, s + 15 });
            }

            return MeshFrom(name, vertices, triangles, null);
        }

        private static Mesh GaugeNeedle(string name)
        {
            Vector2[] outline =
            {
                new Vector2(-0.48f, -0.07f),
                new Vector2(0.28f, -0.07f),
                new Vector2(0.54f, 0f),
                new Vector2(0.28f, 0.07f),
                new Vector2(-0.48f, 0.07f)
            };
            return ExtrudedPolygon(name, outline, 1f);
        }

        private static Mesh ExtrudedPolygon(string name, Vector2[] outline, float thickness)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float z0 = -thickness * 0.5f;
            float z1 = thickness * 0.5f;
            for (int i = 0; i < outline.Length; i++) vertices.Add(V3(outline[i].x, outline[i].y, z0));
            for (int i = 0; i < outline.Length; i++) vertices.Add(V3(outline[i].x, outline[i].y, z1));

            for (int i = 1; i < outline.Length - 1; i++)
            {
                triangles.AddRange(new[] { 0, i, i + 1 });
                triangles.AddRange(new[] { outline.Length, outline.Length + i + 1, outline.Length + i });
            }

            for (int i = 0; i < outline.Length; i++)
            {
                int n = (i + 1) % outline.Length;
                triangles.AddRange(new[] { i, n, outline.Length + n, i, outline.Length + n, outline.Length + i });
            }

            return MeshFrom(name, vertices, triangles, null);
        }

        private static Mesh MeshFrom(string name, List<Vector3> vertices, List<int> triangles, List<Vector3> normals)
        {
            Mesh mesh = new Mesh { name = name };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            if (normals != null && normals.Count == vertices.Count)
            {
                mesh.SetNormals(normals);
            }
            else
            {
                mesh.RecalculateNormals();
            }

            mesh.RecalculateBounds();
            return mesh;
        }

        private static Texture2D BuildTexture(MaterialSpec spec, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color baseColor = WithAlpha(spec.Color, spec.Alpha);
            int seed = StableSeed(spec.AssetName);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float u = x / (float)(width - 1);
                    float v = y / (float)(height - 1);
                    float n = Hash01(x, y, seed);
                    float scratches = Mathf.Sin(u * 58f + v * 12f + seed * 0.001f) * 0.5f + 0.5f;
                    float radial = Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.5f));
                    Color color = baseColor;

                    switch (spec.Style)
                    {
                        case TextureStyle.Brass:
                            color *= 0.76f + n * 0.25f;
                            if (scratches > 0.94f) color = Color.Lerp(color, Color.white, 0.24f);
                            if (Hash01(x / 8, y / 8, seed) > 0.90f) color = Color.Lerp(color, C(0.12f, 0.34f, 0.26f), 0.16f);
                            break;
                        case TextureStyle.Iron:
                            color *= 0.58f + n * 0.32f;
                            if (scratches > 0.93f) color = Color.Lerp(color, C(0.55f, 0.50f, 0.42f), 0.18f);
                            break;
                        case TextureStyle.Plate:
                            color *= 0.70f + n * 0.24f;
                            if (u < 0.08f || v > 0.90f) color = Color.Lerp(color, C(0.62f, 0.58f, 0.50f), 0.13f);
                            break;
                        case TextureStyle.Amber:
                            color *= 0.70f + Mathf.Clamp01(1f - radial * 1.55f) * 0.70f + n * 0.08f;
                            if (radial < 0.16f || scratches > 0.96f) color = Color.Lerp(color, Color.white, 0.22f);
                            break;
                        case TextureStyle.Gauge:
                            color *= 0.86f + n * 0.12f;
                            if (radial > 0.42f && radial < 0.46f) color = Color.Lerp(color, Color.black, 0.38f);
                            if (Mathf.Abs(u - 0.5f) < 0.006f || Mathf.Abs(v - 0.5f) < 0.006f) color = Color.Lerp(color, Color.black, 0.25f);
                            break;
                        case TextureStyle.RedPaint:
                            color *= 0.80f + n * 0.18f;
                            if (Hash01(x / 6, y / 6, seed) > 0.86f) color = Color.Lerp(color, C(0.05f, 0.045f, 0.040f), 0.62f);
                            break;
                        case TextureStyle.Verdigris:
                            color *= 0.72f + n * 0.26f;
                            if (scratches > 0.86f) color = Color.Lerp(color, C(0.72f, 0.40f, 0.18f), 0.18f);
                            break;
                        case TextureStyle.Gasket:
                            color *= 0.46f + n * 0.22f;
                            if (scratches > 0.80f) color = Color.Lerp(color, C(0.16f, 0.13f, 0.08f), 0.35f);
                            break;
                    }

                    color.a = spec.Alpha;
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

        private static void EnsurePackageFolders()
        {
            EnsureAssetFolder(RuntimeRoot);
            EnsureAssetFolder(MaterialRoot);
            EnsureAssetFolder(MeshRoot);
            EnsureAssetFolder(TextureRoot);
            EnsureAssetFolder(PrefabRoot);
            EnsureAssetFolder(MetadataRoot);

            string packagePath = PackagePhysicalRoot();
            Directory.CreateDirectory(Path.Combine(packagePath, "Documentation~", "Manifest"));
            Directory.CreateDirectory(Path.Combine(packagePath, "Samples~", "PreviewScene"));
        }

        private static void DeleteStaleCandidatePrefab()
        {
            string candidatePath = PrefabRoot + "/DV10_CandidateAssembly_DoorVault_A.prefab";
            if (AssetDatabase.LoadMainAssetAtPath(candidatePath) != null)
            {
                AssetDatabase.DeleteAsset(candidatePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static void EnsureAssetFolder(string path)
        {
            path = path.Replace("\\", "/");
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = Path.GetDirectoryName(path)?.Replace("\\", "/");
            if (string.IsNullOrEmpty(parent))
            {
                return;
            }

            EnsureAssetFolder(parent);
            AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
        }

        private static Shader FindLitShader()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) throw new InvalidOperationException("Could not find a compatible shader.");
            return shader;
        }

        private static void ReplaceAsset(string assetPath)
        {
            if (AssetDatabase.LoadMainAssetAtPath(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                return new Bounds(root.transform.position, Vector3.one);
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private static string PreviewNameFor(string assetName)
        {
            return "DV10_PREVIEW_" + ToKebab(assetName.Replace("DV10_", string.Empty)) + ".png";
        }

        private static string ToKebab(string value)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (c == '_' || c == ' ')
                {
                    builder.Append('-');
                    continue;
                }

                if (char.IsUpper(c) && i > 0 && builder.Length > 0 && builder[builder.Length - 1] != '-')
                {
                    builder.Append('-');
                }

                builder.Append(char.ToLowerInvariant(c));
            }

            return builder.ToString().Replace("--", "-").Trim('-');
        }

        private static void Fill(Texture2D texture, Color color)
        {
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
        }

        private static void PasteScaled(Texture2D destination, Texture2D source, int x0, int y0, int width, int height)
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

        private static string PackagePhysicalRoot()
        {
            UnityEditor.PackageManager.PackageInfo info = UnityEditor.PackageManager.PackageInfo.FindForPackageName(PackageName);
            if (info != null && Directory.Exists(info.resolvedPath))
            {
                return info.resolvedPath;
            }

            string cursor = Application.dataPath;
            while (!string.IsNullOrEmpty(cursor))
            {
                string candidate = Path.Combine(cursor, "package.json");
                if (File.Exists(candidate) && File.ReadAllText(candidate).Contains(PackageName))
                {
                    return cursor;
                }

                cursor = Directory.GetParent(cursor)?.FullName;
            }

            throw new DirectoryNotFoundException("Could not resolve package root for " + PackageName);
        }

        private static string RepoRoot()
        {
            string package = PackagePhysicalRoot();
            DirectoryInfo packageDir = new DirectoryInfo(package);
            DirectoryInfo assetPacksDir = packageDir.Parent;
            DirectoryInfo repoDir = assetPacksDir?.Parent;
            if (repoDir == null)
            {
                throw new DirectoryNotFoundException("Could not resolve repo root from package root " + package);
            }

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

        private static int StableSeed(string value)
        {
            unchecked
            {
                int hash = 23;
                for (int i = 0; i < value.Length; i++)
                {
                    hash = hash * 31 + value[i];
                }

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

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private static Vector3 V3(float x, float y, float z)
        {
            return new Vector3(x, y, z);
        }

        private static void Line(StringBuilder json, string key, string value, int indent, bool comma)
        {
            json.Append(new string(' ', indent * 2));
            json.Append('"').Append(key).Append("\": \"").Append(Escape(value)).Append('"');
            if (comma) json.Append(',');
            json.AppendLine();
        }

        private static void NumLine(StringBuilder json, string key, int value, int indent, bool comma)
        {
            json.Append(new string(' ', indent * 2));
            json.Append('"').Append(key).Append("\": ").Append(value.ToString(CultureInfo.InvariantCulture));
            if (comma) json.Append(',');
            json.AppendLine();
        }

        private static void FloatLine(StringBuilder json, string key, float value, int indent, bool comma)
        {
            json.Append(new string(' ', indent * 2));
            json.Append('"').Append(key).Append("\": ").Append(value.ToString("0.###", CultureInfo.InvariantCulture));
            if (comma) json.Append(',');
            json.AppendLine();
        }

        private static void ArrayLine(StringBuilder json, string key, string[] values, int indent, bool comma)
        {
            json.Append(new string(' ', indent * 2));
            json.Append('"').Append(key).AppendLine("\": [");
            for (int i = 0; i < values.Length; i++)
            {
                json.Append(new string(' ', (indent + 1) * 2));
                json.Append('"').Append(Escape(values[i])).Append('"');
                if (i < values.Length - 1) json.Append(',');
                json.AppendLine();
            }

            json.Append(new string(' ', indent * 2)).Append(']');
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

        private sealed class ComponentRecord
        {
            public string Id;
            public string Role;
            public string AcceptanceStatus;
            public string PrefabPath;
            public string PreviewPath;
            public string Notes;
            public int RendererCount;
            public int MeshPartCount;
            public Vector3 BoundsSize;
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
