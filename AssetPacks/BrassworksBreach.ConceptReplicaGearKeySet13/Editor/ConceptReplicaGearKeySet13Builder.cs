#if UNITY_EDITOR
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
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.ConceptReplicaGearKeySet13.Editor
{
    public static class ConceptReplicaGearKeySet13Builder
    {
        private const string PackageName = "com.brassworks.sidecar.concept-replica-gear-key-set13";
        private const string PackageRootAssetPath = "Packages/" + PackageName;
        private const string Version = "0.1.57-p013";
        private const string Prefix = "CRGK13";
        private const string RenderFolderName = "V0_1_57_ConceptReplicaGearKeySet13";
        private const int TextureSize = 1024;
        private const int RenderWidth = 1080;
        private const int RenderHeight = 1500;

        private static readonly List<AssetRecord> TextureRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> MaterialRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> MeshRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> PrefabRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> RenderRecords = new List<AssetRecord>();
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();

        private static string _packageRoot = "";
        private static string _repoRoot = "";
        private static string _renderRoot = "";
        private static string _planningRoot = "";
        private static string _qaRoot = "";

        [MenuItem("Brassworks Breach/Sidecar Packs/Concept Replica Gear Key Set 13/Generate Assets And Renders")]
        public static void GenerateAssetsAndRenders()
        {
            TextureRecords.Clear();
            MaterialRecords.Clear();
            MeshRecords.Clear();
            PrefabRecords.Clear();
            RenderRecords.Clear();
            Meshes.Clear();
            Materials.Clear();

            ResolveRoots();
            PrepareFolders();
            CreateTextures();
            CreateMaterials();
            CreateMeshes();
            CreateGearKeyPrefab();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            RenderProofs();
            var validation = ValidateGeneratedContent();
            WriteMetadata(validation);
            WriteDocs(validation);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            if (!validation.Passed)
            {
                throw new InvalidOperationException(Prefix + "_VALIDATION_FAIL " + string.Join("; ", validation.Failures.ToArray()));
            }

            Debug.Log(Prefix + "_GENERATE_PASS marker=" + Version +
                      " prefabs=" + validation.PrefabCount.ToString(CultureInfo.InvariantCulture) +
                      " meshes=" + validation.MeshCount.ToString(CultureInfo.InvariantCulture) +
                      " materials=" + validation.MaterialCount.ToString(CultureInfo.InvariantCulture) +
                      " textures=" + validation.TextureCount.ToString(CultureInfo.InvariantCulture) +
                      " renders=" + validation.RenderCount.ToString(CultureInfo.InvariantCulture) +
                      " verdict=CONCEPT_PROOF_PASS_NOT_FINAL_SHIPPING");
        }

        private static void ResolveRoots()
        {
            var info = PackageInfo.FindForPackageName(PackageName);
            if (info == null || string.IsNullOrEmpty(info.resolvedPath))
            {
                throw new InvalidOperationException("Unable to resolve " + PackageName + ". Run from the package validation project.");
            }

            _packageRoot = Normalize(info.resolvedPath);
            var assetPacks = Directory.GetParent(_packageRoot);
            _repoRoot = Normalize(assetPacks != null && assetPacks.Parent != null ? assetPacks.Parent.FullName : Directory.GetCurrentDirectory());
            _renderRoot = Normalize(Path.Combine(_repoRoot, "Documentation", "ConceptRenders", RenderFolderName));
            _planningRoot = Normalize(Path.Combine(_repoRoot, "Documentation", "Planning"));
            _qaRoot = Normalize(Path.Combine(_repoRoot, "Documentation", "QA"));
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
                "Documentation~/Previews",
                "Documentation~/QA"
            })
            {
                ResetDirectory(Physical(relative));
            }

            ResetDirectory(_renderRoot);
            Directory.CreateDirectory(_planningRoot);
            Directory.CreateDirectory(_qaRoot);
        }

        private static void CreateTextures()
        {
            SaveTexture("AgedDarkBrass_Albedo", CreateAgedBrassAlbedo(TextureSize), false, "aged_dark_brass", "Base color map with bronze, soot, verdigris, worn gold edges, stains, and dark pitting.");
            SaveTexture("AgedDarkBrass_Normal", CreateMetalNormal(TextureSize, 1307, 0.92f), true, "aged_dark_brass", "Normal map with micro scratches, hammer pitting, and worn directional ridges.");
            SaveTexture("AgedDarkBrass_MetallicSmoothness", CreateMetalMask(TextureSize, 0.90f, 0.42f, 2031, 0.24f), false, "aged_dark_brass", "Metallic/smoothness mask with uneven polish and tarnished low-sheen pockets.");
            SaveTexture("AgedDarkBrass_Occlusion", CreateOcclusion(TextureSize, 3001, 0.38f), false, "aged_dark_brass", "Crevice/pit occlusion map for blackened recesses.");

            SaveTexture("BlackenedRecess_Albedo", CreateBlackenedRecessAlbedo(TextureSize / 2), false, "blackened_recess", "Warm black grime map for gear holes, shaft mouth, deep seams, and side-bit notches.");
            SaveTexture("BlackenedRecess_MetallicSmoothness", CreateMetalMask(TextureSize / 2, 0.18f, 0.18f, 4021, 0.18f), false, "blackened_recess", "Low-polish oily black mask.");
            SaveTexture("WarmEdgeHighlight_Albedo", CreateEdgeHighlightAlbedo(TextureSize / 2), false, "edge_highlight", "Subtle desaturated brass edge-highlight map, not saturated orange.");
            SaveTexture("DarkReferenceBackdrop_Albedo", CreateBackdropTexture(TextureSize), false, "render_backdrop", "Dark worn cloth/stone reference backdrop for render-only proof views.");
        }

        private static void CreateMaterials()
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible Lit/Standard shader found.");
            }

            Materials["brass"] = SaveMaterial("AgedDarkBrass_BurnishedEdges", shader, new Color(0.47f, 0.34f, 0.16f), 0.90f, 0.46f,
                TextureAsset("AgedDarkBrass_Albedo"), TextureAsset("AgedDarkBrass_Normal"), TextureAsset("AgedDarkBrass_MetallicSmoothness"), TextureAsset("AgedDarkBrass_Occlusion"),
                null, "aged_dark_brass", "Primary dark antique brass/bronze material for bow, shaft, teeth, collars, and bit.");
            Materials["dark"] = SaveMaterial("BlackenedRecesses_OilyCrevices", shader, new Color(0.025f, 0.020f, 0.015f), 0.18f, 0.18f,
                TextureAsset("BlackenedRecess_Albedo"), null, TextureAsset("BlackenedRecess_MetallicSmoothness"), null,
                null, "blackened_recess", "Dark grime and hollow openings used to keep the key from reading as clean toy brass.");
            Materials["edge"] = SaveMaterial("WarmWornEdgeGlints", shader, new Color(0.74f, 0.57f, 0.29f), 0.88f, 0.62f,
                TextureAsset("WarmEdgeHighlight_Albedo"), TextureAsset("AgedDarkBrass_Normal"), TextureAsset("AgedDarkBrass_MetallicSmoothness"), null,
                null, "edge_highlight", "Selective warmer worn metal on high rims, tooth tips, collars, and scratch streaks.");
            Materials["shadowBrass"] = SaveMaterial("SootedShadowBrass", shader, new Color(0.18f, 0.13f, 0.07f), 0.82f, 0.30f,
                TextureAsset("AgedDarkBrass_Albedo"), TextureAsset("AgedDarkBrass_Normal"), TextureAsset("AgedDarkBrass_MetallicSmoothness"), TextureAsset("AgedDarkBrass_Occlusion"),
                null, "aged_dark_brass_shadow", "Darker brass variant for recessed inner ring, underside collars, and worn bit shadows.");
            Materials["backdrop"] = SaveMaterial("DarkReferenceBackdrop", shader, new Color(0.030f, 0.030f, 0.026f), 0f, 0.22f,
                TextureAsset("DarkReferenceBackdrop_Albedo"), null, null, null,
                null, "render_backdrop", "Render-only dark background matching the crop's low-key studio card.");

            foreach (var material in Materials.Values)
            {
                material.doubleSidedGI = true;
                if (material.HasProperty("_Cull"))
                {
                    material.SetFloat("_Cull", (float)CullMode.Off);
                }
            }
        }

        private static void CreateMeshes()
        {
            Meshes["gearBow"] = SaveMesh("GearBowIrregular14Tooth_Z", CreateGearRingMesh(168, 14, 0.63f, 0.80f, 1.00f, 0.10f));
            Meshes["ringWide"] = SaveMesh("RingWide72_Z", CreateRingMesh(96, 0.49f, 0.31f, 0.10f));
            Meshes["ringNarrow"] = SaveMesh("RingNarrow72_Z", CreateRingMesh(96, 0.28f, 0.18f, 0.11f));
            Meshes["cylY48"] = SaveMesh("Cylinder48_Y", CreateCylinderMesh(48, Axis.Y));
            Meshes["cylY24"] = SaveMesh("Cylinder24_Y", CreateCylinderMesh(24, Axis.Y));
            Meshes["cylZ64"] = SaveMesh("Cylinder64_Z", CreateCylinderMesh(64, Axis.Z));
            Meshes["cylZ24"] = SaveMesh("Cylinder24_Z", CreateCylinderMesh(24, Axis.Z));
            Meshes["torusY48"] = SaveMesh("Torus48_8_Y", CreateTorusMesh(48, 8, Axis.Y, 0.50f, 0.08f));
            Meshes["torusZ48"] = SaveMesh("Torus48_8_Z", CreateTorusMesh(48, 8, Axis.Z, 0.50f, 0.055f));
            Meshes["box"] = SaveMesh("ChamferedBoxUnit", CreateChamferedBoxMesh());
            Meshes["rivet"] = SaveMesh("DomedRivet20_Z", CreateDomeMesh(20, 7));
            Meshes["chip"] = SaveMesh("TinyIrregularChip_Z", CreateIrregularChipMesh());
        }

        private static void CreateGearKeyPrefab()
        {
            var root = new GameObject(Prefix + "_PREFAB_GearKey_ConceptReplica");
            var visual = new GameObject(Prefix + "_gear_key_visual_no_scripts");
            visual.transform.SetParent(root.transform, false);

            var brass = Materials["brass"];
            var dark = Materials["dark"];
            var edge = Materials["edge"];
            var shadowBrass = Materials["shadowBrass"];

            var bowCenter = new Vector3(0.10f, 0.92f, 0f);
            AddPart(visual.transform, "irregular_missing_tooth_outer_gear_bow", Meshes["gearBow"], brass, bowCenter, Vector3.zero, new Vector3(0.50f, 0.50f, 1f));
            AddPart(visual.transform, "blackened_gear_bow_backplate_seen_through_cutouts", Meshes["cylZ64"], dark, bowCenter + new Vector3(0f, 0f, 0.07f), Vector3.zero, new Vector3(0.23f, 0.23f, 0.024f));
            AddPart(visual.transform, "raised_inner_spoke_ring", Meshes["ringWide"], shadowBrass, bowCenter + new Vector3(0.01f, -0.01f, -0.065f), Vector3.zero, new Vector3(0.46f, 0.46f, 1f));
            AddPart(visual.transform, "inner_hub_ring_worn_gold_lip", Meshes["ringNarrow"], brass, bowCenter + new Vector3(0.00f, 0.00f, -0.092f), Vector3.zero, new Vector3(0.50f, 0.50f, 1f));
            AddPart(visual.transform, "dark_circular_center_recess", Meshes["cylZ24"], dark, bowCenter + new Vector3(0f, 0f, -0.12f), Vector3.zero, new Vector3(0.13f, 0.13f, 0.030f));
            AddPart(visual.transform, "small_round_center_rivet_cap", Meshes["rivet"], edge, bowCenter + new Vector3(0f, 0f, -0.15f), Vector3.zero, new Vector3(0.085f, 0.085f, 0.045f));

            for (var i = 0; i < 5; i++)
            {
                var angle = -18f + i * 72f;
                var radial = Dir(angle);
                AddPart(visual.transform, "raised_spoke_" + i.ToString("00", CultureInfo.InvariantCulture), Meshes["box"], brass,
                    bowCenter + radial * 0.205f + new Vector3(0f, 0f, -0.118f),
                    new Vector3(0f, 0f, angle - 90f),
                    new Vector3(0.040f, 0.245f, 0.045f));
                AddPart(visual.transform, "black_grime_spoke_undercut_" + i.ToString("00", CultureInfo.InvariantCulture), Meshes["box"], dark,
                    bowCenter + radial * 0.207f + new Vector3(0f, 0f, -0.085f),
                    new Vector3(0f, 0f, angle - 90f),
                    new Vector3(0.052f, 0.270f, 0.018f));
            }

            AddBowWearAndPits(visual.transform, bowCenter);

            var shaftCenter = new Vector3(-0.22f, -0.26f, 0f);
            const float shaftLength = 1.88f;
            const float shaftAngle = 165f;
            AddPart(visual.transform, "long_taper_read_antique_bronze_key_shaft", Meshes["cylY48"], brass, shaftCenter, new Vector3(0f, 0f, shaftAngle), new Vector3(0.112f, shaftLength, 0.112f));
            AddPart(visual.transform, "shaft_dark_hollow_mouth_bottom", Meshes["cylY24"], dark, ShaftPoint(shaftCenter, shaftAngle, -0.96f), new Vector3(0f, 0f, shaftAngle), new Vector3(0.074f, 0.040f, 0.074f));
            AddPart(visual.transform, "shaft_front_worn_highlight_streak", Meshes["box"], edge, shaftCenter + Side(shaftAngle) * 0.080f + new Vector3(0f, 0f, -0.095f), new Vector3(0f, 0f, shaftAngle), new Vector3(0.018f, 1.60f, 0.018f));
            AddPart(visual.transform, "shaft_blackened_side_shadow_streak", Meshes["box"], dark, shaftCenter - Side(shaftAngle) * 0.092f + new Vector3(0f, 0f, -0.090f), new Vector3(0f, 0f, shaftAngle), new Vector3(0.012f, 1.46f, 0.014f));

            AddShaftCollar(visual.transform, shaftCenter, shaftAngle, 0.82f, "upper_socket_collar", 0.180f, 0.095f);
            AddShaftCollar(visual.transform, shaftCenter, shaftAngle, 0.69f, "stacked_dark_groove_under_bow", 0.145f, 0.045f);
            AddShaftCollar(visual.transform, shaftCenter, shaftAngle, 0.55f, "second_burnished_collar_band", 0.155f, 0.075f);
            AddShaftCollar(visual.transform, shaftCenter, shaftAngle, -0.88f, "bottom_lipped_open_end_band", 0.150f, 0.080f);
            AddShaftCollar(visual.transform, shaftCenter, shaftAngle, -0.08f, "subtle_midshaft_wear_ring", 0.126f, 0.028f);

            AddBitAssembly(visual.transform, shaftCenter, shaftAngle);
            AddShaftScratches(visual.transform, shaftCenter, shaftAngle);

            var prefabPath = PackageRootAssetPath + "/Runtime/Prefabs/" + root.name + ".prefab";
            ReplaceAsset(prefabPath);
            PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            Object.DestroyImmediate(root);
            PrefabRecords.Add(new AssetRecord("Runtime/Prefabs/" + Prefix + "_PREFAB_GearKey_ConceptReplica.prefab", "prefab", "concept_replica_gear_key", "Visual-only focused replica prefab. Contains Transform, MeshFilter, MeshRenderer only."));
        }

        private static void AddGearToothHighlights(Transform parent, Vector3 bowCenter)
        {
            var missing = new HashSet<int> { 4, 9 };
            for (var i = 0; i < 14; i++)
            {
                if (missing.Contains(i))
                {
                    continue;
                }

                var angle = i * 360f / 14f + (i % 3 == 0 ? 1.8f : -1.1f);
                var radial = Dir(angle);
                var tangentAngle = angle + 90f;
                AddPart(parent, "worn_bright_tooth_tip_" + i.ToString("00", CultureInfo.InvariantCulture), Meshes["box"], Materials["edge"],
                    bowCenter + radial * 0.548f + new Vector3(0f, 0f, -0.145f),
                    new Vector3(0f, 0f, tangentAngle),
                    new Vector3(0.082f, 0.026f, 0.024f));

                if ((i & 1) == 0)
                {
                    AddPart(parent, "blackened_gap_behind_tooth_" + i.ToString("00", CultureInfo.InvariantCulture), Meshes["box"], Materials["dark"],
                        bowCenter + radial * 0.486f + new Vector3(0f, 0f, -0.098f),
                        new Vector3(0f, 0f, tangentAngle),
                        new Vector3(0.070f, 0.024f, 0.018f));
                }
            }
        }

        private static void AddBowWearAndPits(Transform parent, Vector3 bowCenter)
        {
            for (var i = 0; i < 24; i++)
            {
                var angle = 12f + i * 360f / 24f + ((i % 5) - 2) * 1.4f;
                var radius = 0.285f + (i % 4) * 0.058f;
                var material = (i % 3 == 0) ? Materials["dark"] : Materials["edge"];
                var scale = (i % 3 == 0) ? new Vector3(0.030f, 0.014f, 0.012f) : new Vector3(0.060f, 0.010f, 0.010f);
                AddPart(parent, "bow_surface_" + ((i % 3 == 0) ? "pit_" : "scratch_") + i.ToString("00", CultureInfo.InvariantCulture), Meshes["chip"], material,
                    bowCenter + Dir(angle) * radius + new Vector3(0f, 0f, -0.154f),
                    new Vector3(0f, 0f, angle + 34f),
                    scale);
            }

            for (var i = 0; i < 7; i++)
            {
                var angle = -145f + i * 12f;
                AddPart(parent, "black_chipped_missing_lower_bow_edge_" + i.ToString("00", CultureInfo.InvariantCulture), Meshes["chip"], Materials["dark"],
                    bowCenter + Dir(angle) * 0.505f + new Vector3(0f, 0f, -0.160f),
                    new Vector3(0f, 0f, angle),
                    new Vector3(0.075f, 0.024f, 0.016f));
            }
        }

        private static void AddShaftCollar(Transform parent, Vector3 center, float angle, float along, string name, float radius, float length)
        {
            var pos = ShaftPoint(center, angle, along);
            AddPart(parent, name + "_brass_lipped_ring", Meshes["cylY48"], Materials["edge"], pos, new Vector3(0f, 0f, angle), new Vector3(radius, length, radius));
            AddPart(parent, name + "_dark_recess_shadow", Meshes["cylY24"], Materials["dark"], ShaftPoint(center, angle, along - length * 0.70f), new Vector3(0f, 0f, angle), new Vector3(radius * 0.88f, 0.018f, radius * 0.88f));
            AddPart(parent, name + "_rear_shadow_lip", Meshes["cylY24"], Materials["shadowBrass"], ShaftPoint(center, angle, along + length * 0.68f), new Vector3(0f, 0f, angle), new Vector3(radius * 0.96f, 0.018f, radius * 0.96f));
        }

        private static void AddBitAssembly(Transform parent, Vector3 shaftCenter, float shaftAngle)
        {
            var basePos = ShaftPoint(shaftCenter, shaftAngle, 0.30f);
            var side = -Side(shaftAngle);
            var angle = shaftAngle;
            AddPart(parent, "lower_bit_dark_socket_cut_in", Meshes["box"], Materials["dark"], basePos + side * 0.108f + new Vector3(0f, 0f, -0.120f), new Vector3(0f, 0f, angle), new Vector3(0.070f, 0.105f, 0.032f));
            AddPart(parent, "lower_bit_stem_from_shaft", Meshes["box"], Materials["brass"], basePos + side * 0.188f + new Vector3(0f, 0f, -0.100f), new Vector3(0f, 0f, angle), new Vector3(0.165f, 0.078f, 0.085f));
            AddPart(parent, "lower_bit_forward_tooth_block", Meshes["box"], Materials["edge"], basePos + side * 0.328f + Along(shaftAngle) * 0.070f + new Vector3(0f, 0f, -0.110f), new Vector3(0f, 0f, angle), new Vector3(0.190f, 0.078f, 0.086f));
            AddPart(parent, "lower_bit_downward_step_tooth", Meshes["box"], Materials["brass"], basePos + side * 0.255f - Along(shaftAngle) * 0.132f + new Vector3(0f, 0f, -0.106f), new Vector3(0f, 0f, angle), new Vector3(0.112f, 0.182f, 0.082f));
            AddPart(parent, "lower_bit_tiny_notch_dark_upper", Meshes["box"], Materials["dark"], basePos + side * 0.376f + Along(shaftAngle) * 0.002f + new Vector3(0f, 0f, -0.154f), new Vector3(0f, 0f, angle), new Vector3(0.056f, 0.052f, 0.020f));
            AddPart(parent, "lower_bit_tiny_notch_dark_lower", Meshes["box"], Materials["dark"], basePos + side * 0.305f - Along(shaftAngle) * 0.207f + new Vector3(0f, 0f, -0.154f), new Vector3(0f, 0f, angle), new Vector3(0.052f, 0.046f, 0.020f));
            AddPart(parent, "lower_bit_worn_tip_glint", Meshes["box"], Materials["edge"], basePos + side * 0.430f + Along(shaftAngle) * 0.092f + new Vector3(0f, 0f, -0.158f), new Vector3(0f, 0f, angle), new Vector3(0.070f, 0.016f, 0.015f));
            AddPart(parent, "lower_bit_round_rivet", Meshes["rivet"], Materials["edge"], basePos + side * 0.182f + new Vector3(0f, 0f, -0.168f), Vector3.zero, new Vector3(0.035f, 0.035f, 0.026f));
        }

        private static void AddShaftScratches(Transform parent, Vector3 shaftCenter, float shaftAngle)
        {
            for (var i = 0; i < 18; i++)
            {
                var t = -0.82f + i * 0.087f;
                var offset = (((i * 37) % 100) / 100f - 0.5f) * 0.12f;
                var material = i % 4 == 0 ? Materials["dark"] : Materials["edge"];
                var length = i % 4 == 0 ? 0.055f : 0.115f;
                AddPart(parent, "shaft_micro_" + (i % 4 == 0 ? "pit_" : "scratch_") + i.ToString("00", CultureInfo.InvariantCulture), Meshes["chip"], material,
                    ShaftPoint(shaftCenter, shaftAngle, t) + Side(shaftAngle) * offset + new Vector3(0f, 0f, -0.158f),
                    new Vector3(0f, 0f, shaftAngle + 6f + (i % 3) * 7f),
                    new Vector3(length, 0.012f, 0.010f));
            }
        }

        private static GameObject AddPart(Transform parent, string name, Mesh mesh, Material material, Vector3 position, Vector3 euler, Vector3 scale)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = position;
            go.transform.localEulerAngles = euler;
            go.transform.localScale = scale;
            var filter = go.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            var renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            return go;
        }

        private static Vector3 Dir(float degrees)
        {
            var radians = degrees * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        }

        private static Vector3 Along(float degrees)
        {
            var q = Quaternion.Euler(0f, 0f, degrees);
            return q * Vector3.up;
        }

        private static Vector3 Side(float degrees)
        {
            var q = Quaternion.Euler(0f, 0f, degrees);
            return q * Vector3.right;
        }

        private static Vector3 ShaftPoint(Vector3 center, float angle, float along)
        {
            return center + Along(angle) * along;
        }

        private static Texture2D TextureAsset(string shortName)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(PackageRootAssetPath + "/Runtime/Textures/" + Prefix + "_TEX_" + shortName + ".png");
        }

        private static Texture2D SaveTexture(string shortName, Texture2D texture, bool normalMap, string tag, string notes)
        {
            texture.name = Prefix + "_TEX_" + shortName;
            var physicalPath = Physical("Runtime/Textures/" + texture.name + ".png");
            File.WriteAllBytes(physicalPath, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);

            var assetPath = PackageRootAssetPath + "/Runtime/Textures/" + Prefix + "_TEX_" + shortName + ".png";
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
            var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
                importer.textureType = normalMap ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.sRGBTexture = !normalMap && !shortName.Contains("MetallicSmoothness", StringComparison.OrdinalIgnoreCase) && !shortName.Contains("Occlusion", StringComparison.OrdinalIgnoreCase);
                importer.mipmapEnabled = true;
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.filterMode = FilterMode.Trilinear;
                importer.maxTextureSize = TextureSize;
                importer.SaveAndReimport();
            }

            var imported = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            TextureRecords.Add(new AssetRecord("Runtime/Textures/" + Prefix + "_TEX_" + shortName + ".png", "texture", tag, notes));
            return imported;
        }

        private static Material SaveMaterial(string shortName, Shader shader, Color color, float metallic, float smoothness, Texture2D albedo, Texture2D normal, Texture2D mask, Texture2D occlusion, Color? emission, string tag, string notes)
        {
            var material = new Material(shader) { name = Prefix + "_MAT_" + shortName };
            SetMaterialColor(material, color);
            SetMaterialFloat(material, "_Metallic", metallic);
            SetMaterialFloat(material, "_Glossiness", smoothness);
            SetMaterialFloat(material, "_Smoothness", smoothness);
            SetMaterialTexture(material, "_BaseMap", "_MainTex", albedo);
            if (normal != null)
            {
                SetMaterialTexture(material, "_BumpMap", "_BumpMap", normal);
                SetMaterialFloat(material, "_BumpScale", 0.32f);
                material.EnableKeyword("_NORMALMAP");
            }

            if (mask != null)
            {
                SetMaterialTexture(material, "_MetallicGlossMap", "_MetallicGlossMap", mask);
                material.EnableKeyword("_METALLICGLOSSMAP");
                material.EnableKeyword("_METALLICSPECGLOSSMAP");
            }

            if (occlusion != null)
            {
                SetMaterialTexture(material, "_OcclusionMap", "_OcclusionMap", occlusion);
                SetMaterialFloat(material, "_OcclusionStrength", 0.86f);
            }

            if (emission.HasValue)
            {
                SetMaterialColorProperty(material, "_EmissionColor", emission.Value);
                material.EnableKeyword("_EMISSION");
            }

            var assetPath = PackageRootAssetPath + "/Runtime/Materials/" + material.name + ".mat";
            ReplaceAsset(assetPath);
            AssetDatabase.CreateAsset(material, assetPath);
            MaterialRecords.Add(new AssetRecord("Runtime/Materials/" + material.name + ".mat", "material", tag, notes));
            return AssetDatabase.LoadAssetAtPath<Material>(assetPath);
        }

        private static Mesh SaveMesh(string shortName, Mesh mesh)
        {
            mesh.name = Prefix + "_MESH_" + shortName;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            var assetPath = PackageRootAssetPath + "/Runtime/Meshes/" + mesh.name + ".asset";
            ReplaceAsset(assetPath);
            AssetDatabase.CreateAsset(mesh, assetPath);
            MeshRecords.Add(new AssetRecord("Runtime/Meshes/" + mesh.name + ".asset", "mesh", "gear_key_geometry", "Unity-generated custom mesh: " + shortName + "."));
            return AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
        }

        private static void SetMaterialColor(Material material, Color color)
        {
            SetMaterialColorProperty(material, "_BaseColor", color);
            SetMaterialColorProperty(material, "_Color", color);
        }

        private static void SetMaterialColorProperty(Material material, string property, Color color)
        {
            if (material.HasProperty(property))
            {
                material.SetColor(property, color);
            }
        }

        private static void SetMaterialFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property))
            {
                material.SetFloat(property, value);
            }
        }

        private static void SetMaterialTexture(Material material, string urpProperty, string builtInProperty, Texture texture)
        {
            if (texture == null)
            {
                return;
            }

            if (material.HasProperty(urpProperty))
            {
                material.SetTexture(urpProperty, texture);
            }

            if (material.HasProperty(builtInProperty))
            {
                material.SetTexture(builtInProperty, texture);
            }
        }

        private static Texture2D CreateAgedBrassAlbedo(int size)
        {
            var texture = NewTexture(size);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var u = x / (float)(size - 1);
                    var v = y / (float)(size - 1);
                    var large = Fbm(u * 5.5f, v * 5.5f, 1201, 5);
                    var fine = Fbm(u * 34f, v * 34f, 1207, 4);
                    var streak = Mathf.Pow(Mathf.Abs(Mathf.Sin((u * 21f + v * 2.0f + Fbm(u * 8f, v * 8f, 1211, 2)) * Mathf.PI)), 24f);
                    var pits = Hash01(Mathf.FloorToInt(u * 115f), Mathf.FloorToInt(v * 115f), 1217) > 0.945f ? 1f : 0f;
                    var verdigris = Mathf.SmoothStep(0.70f, 0.92f, Fbm(u * 11f + 4f, v * 11f - 2f, 1223, 3));
                    var polish = Mathf.SmoothStep(0.58f, 0.95f, fine) * 0.13f + streak * 0.025f;
                    var baseColor = Lerp(new Color(0.16f, 0.115f, 0.060f), new Color(0.52f, 0.38f, 0.165f), large * 0.74f + 0.12f);
                    baseColor = Lerp(baseColor, new Color(0.76f, 0.59f, 0.30f), polish);
                    baseColor = Lerp(baseColor, new Color(0.055f, 0.038f, 0.025f), pits * 0.72f);
                    baseColor = Lerp(baseColor, new Color(0.10f, 0.26f, 0.20f), verdigris * 0.18f);
                    texture.SetPixel(x, y, ClampColor(baseColor));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateBlackenedRecessAlbedo(int size)
        {
            var texture = NewTexture(size);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var u = x / (float)(size - 1);
                    var v = y / (float)(size - 1);
                    var n = Fbm(u * 12f, v * 12f, 4103, 5);
                    var oil = Mathf.SmoothStep(0.55f, 0.95f, Fbm(u * 28f, v * 20f, 4109, 3));
                    var c = Lerp(new Color(0.009f, 0.007f, 0.005f), new Color(0.055f, 0.043f, 0.030f), n);
                    c = Lerp(c, new Color(0.12f, 0.070f, 0.038f), oil * 0.24f);
                    texture.SetPixel(x, y, ClampColor(c));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateEdgeHighlightAlbedo(int size)
        {
            var texture = NewTexture(size);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var u = x / (float)(size - 1);
                    var v = y / (float)(size - 1);
                    var n = Fbm(u * 18f, v * 18f, 5101, 4);
                    var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((u * 54f + v * 8f) * Mathf.PI)), 22f);
                    var c = Lerp(new Color(0.46f, 0.30f, 0.13f), new Color(0.86f, 0.63f, 0.28f), n * 0.45f + scratch * 0.45f);
                    texture.SetPixel(x, y, ClampColor(c));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateBackdropTexture(int size)
        {
            var texture = NewTexture(size);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var u = x / (float)(size - 1);
                    var v = y / (float)(size - 1);
                    var n = Fbm(u * 8f, v * 8f, 7101, 5);
                    var vignette = Mathf.Clamp01(Vector2.Distance(new Vector2(u, v), new Vector2(0.48f, 0.52f)) * 1.45f);
                    var c = Lerp(new Color(0.020f, 0.020f, 0.018f), new Color(0.062f, 0.055f, 0.043f), n * 0.35f);
                    c = Lerp(c, new Color(0.006f, 0.006f, 0.005f), vignette * 0.62f);
                    texture.SetPixel(x, y, ClampColor(c));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateMetalMask(int size, float metallic, float smoothness, int seed, float variance)
        {
            var texture = NewTexture(size);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var u = x / (float)(size - 1);
                    var v = y / (float)(size - 1);
                    var n = Fbm(u * 19f, v * 19f, seed, 4);
                    var scratches = Mathf.Pow(Mathf.Abs(Mathf.Sin((u * 70f + v * 9f + n) * Mathf.PI)), 28f);
                    var m = Mathf.Clamp01(metallic - (1f - n) * variance * 0.16f);
                    var s = Mathf.Clamp01(smoothness + (n - 0.5f) * variance - scratches * variance * 0.55f);
                    texture.SetPixel(x, y, new Color(m, s, 0f, s));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateOcclusion(int size, int seed, float strength)
        {
            var texture = NewTexture(size);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var u = x / (float)(size - 1);
                    var v = y / (float)(size - 1);
                    var pits = Mathf.SmoothStep(0.58f, 0.92f, Fbm(u * 45f, v * 45f, seed, 4));
                    var longGrime = Mathf.SmoothStep(0.40f, 0.88f, Fbm(u * 5f, v * 19f, seed + 13, 4));
                    var occ = Mathf.Clamp01(1f - (pits * 0.75f + longGrime * 0.34f) * strength);
                    texture.SetPixel(x, y, new Color(occ, occ, occ, 1f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateMetalNormal(int size, int seed, float strength)
        {
            var height = new float[size, size];
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var u = x / (float)(size - 1);
                    var v = y / (float)(size - 1);
                    var pitted = Fbm(u * 34f, v * 34f, seed, 4) * 0.44f;
                    var broad = Fbm(u * 8f, v * 8f, seed + 17, 4) * 0.38f;
                    var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((u * 47f + v * 7f + broad * 2f) * Mathf.PI)), 44f) * 0.075f;
                    height[x, y] = broad + pitted - scratch;
                }
            }

            var texture = NewTexture(size);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var left = height[Mathf.Max(0, x - 1), y];
                    var right = height[Mathf.Min(size - 1, x + 1), y];
                    var down = height[x, Mathf.Max(0, y - 1)];
                    var up = height[x, Mathf.Min(size - 1, y + 1)];
                    var dx = (left - right) * strength;
                    var dy = (down - up) * strength;
                    var normal = new Vector3(dx, dy, 1f).normalized;
                    texture.SetPixel(x, y, new Color(normal.x * 0.5f + 0.5f, normal.y * 0.5f + 0.5f, normal.z * 0.5f + 0.5f, 1f));
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D NewTexture(int size)
        {
            return new Texture2D(size, size, TextureFormat.RGBA32, true, true)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Trilinear
            };
        }

        private static Color Lerp(Color a, Color b, float t)
        {
            return Color.Lerp(a, b, Mathf.Clamp01(t));
        }

        private static Color ClampColor(Color color)
        {
            return new Color(Mathf.Clamp01(color.r), Mathf.Clamp01(color.g), Mathf.Clamp01(color.b), Mathf.Clamp01(color.a <= 0f ? 1f : color.a));
        }

        private static float Fbm(float x, float y, int seed, int octaves)
        {
            var value = 0f;
            var amp = 0.5f;
            var freq = 1f;
            var sum = 0f;
            for (var i = 0; i < octaves; i++)
            {
                value += SmoothNoise(x * freq, y * freq, seed + i * 101) * amp;
                sum += amp;
                amp *= 0.5f;
                freq *= 2f;
            }

            return sum <= 0f ? 0f : value / sum;
        }

        private static float SmoothNoise(float x, float y, int seed)
        {
            var ix = Mathf.FloorToInt(x);
            var iy = Mathf.FloorToInt(y);
            var fx = x - ix;
            var fy = y - iy;
            var a = Hash01(ix, iy, seed);
            var b = Hash01(ix + 1, iy, seed);
            var c = Hash01(ix, iy + 1, seed);
            var d = Hash01(ix + 1, iy + 1, seed);
            var ux = fx * fx * (3f - 2f * fx);
            var uy = fy * fy * (3f - 2f * fy);
            return Mathf.Lerp(Mathf.Lerp(a, b, ux), Mathf.Lerp(c, d, ux), uy);
        }

        private static float Hash01(int x, int y, int seed)
        {
            unchecked
            {
                var h = (uint)(x * 374761393 + y * 668265263 + seed * 1442695041);
                h = (h ^ (h >> 13)) * 1274126177u;
                h ^= h >> 16;
                return (h & 0x00FFFFFF) / 16777215f;
            }
        }

        private static Mesh CreateGearRingMesh(int segments, int teeth, float innerRadius, float rootRadius, float toothRadius, float thickness)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var outerFront = new int[segments];
            var innerFront = new int[segments];
            var outerBack = new int[segments];
            var innerBack = new int[segments];
            var missing = new HashSet<int> { 4, 9 };

            for (var i = 0; i < segments; i++)
            {
                var t = i / (float)segments;
                var angle = t * Mathf.PI * 2f;
                var toothIndex = Mathf.FloorToInt(t * teeth);
                var local = t * teeth - toothIndex;
                var isMissing = missing.Contains(toothIndex);
                var toothShape = local < 0.18f || local > 0.82f ? 0f : local < 0.32f ? Mathf.InverseLerp(0.18f, 0.32f, local) : local > 0.68f ? 1f - Mathf.InverseLerp(0.68f, 0.82f, local) : 1f;
                if (isMissing)
                {
                    toothShape *= 0.18f;
                }

                var nick = (Hash01(toothIndex, i % 7, 6181) - 0.5f) * 0.030f;
                var outer = Mathf.Lerp(rootRadius, toothRadius + nick, toothShape);
                if ((toothIndex == 1 || toothIndex == 11) && local > 0.50f)
                {
                    outer -= 0.035f;
                }

                var inner = innerRadius + (Fbm(Mathf.Cos(angle) * 1.7f + 4f, Mathf.Sin(angle) * 1.7f - 3f, 6191, 2) - 0.5f) * 0.015f;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                outerFront[i] = AddVertex(vertices, uvs, new Vector3(direction.x * outer, direction.y * outer, -thickness * 0.5f), direction * 0.5f + Vector2.one * 0.5f);
                innerFront[i] = AddVertex(vertices, uvs, new Vector3(direction.x * inner, direction.y * inner, -thickness * 0.5f), direction * inner / toothRadius * 0.5f + Vector2.one * 0.5f);
                outerBack[i] = AddVertex(vertices, uvs, new Vector3(direction.x * outer * 0.985f, direction.y * outer * 0.985f, thickness * 0.5f), direction * 0.5f + Vector2.one * 0.5f);
                innerBack[i] = AddVertex(vertices, uvs, new Vector3(direction.x * inner * 1.015f, direction.y * inner * 1.015f, thickness * 0.5f), direction * inner / toothRadius * 0.5f + Vector2.one * 0.5f);
            }

            for (var i = 0; i < segments; i++)
            {
                var j = (i + 1) % segments;
                AddQuad(triangles, outerFront[i], innerFront[i], innerFront[j], outerFront[j]);
                AddQuad(triangles, outerBack[j], innerBack[j], innerBack[i], outerBack[i]);
                AddQuad(triangles, outerFront[j], outerBack[j], outerBack[i], outerFront[i]);
                AddQuad(triangles, innerFront[i], innerBack[i], innerBack[j], innerFront[j]);
            }

            return BuildMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateRingMesh(int segments, float outerRadius, float innerRadius, float thickness)
        {
            return CreateGearRingMesh(segments, segments, innerRadius, outerRadius, outerRadius, thickness);
        }

        private static Mesh CreateCylinderMesh(int segments, Axis axis)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var aFront = new int[segments];
            var aBack = new int[segments];
            var centerA = AddVertex(vertices, uvs, AxisPoint(axis, 0f, -0.5f, 0f), new Vector2(0.5f, 0.5f));
            var centerB = AddVertex(vertices, uvs, AxisPoint(axis, 0f, 0.5f, 0f), new Vector2(0.5f, 0.5f));
            for (var i = 0; i < segments; i++)
            {
                var angle = i * Mathf.PI * 2f / segments;
                var x = Mathf.Cos(angle) * 0.5f;
                var z = Mathf.Sin(angle) * 0.5f;
                aFront[i] = AddVertex(vertices, uvs, AxisPoint(axis, x, -0.5f, z), new Vector2(i / (float)segments, 0f));
                aBack[i] = AddVertex(vertices, uvs, AxisPoint(axis, x, 0.5f, z), new Vector2(i / (float)segments, 1f));
            }

            for (var i = 0; i < segments; i++)
            {
                var j = (i + 1) % segments;
                AddQuad(triangles, aFront[i], aBack[i], aBack[j], aFront[j]);
                triangles.Add(centerA);
                triangles.Add(aFront[j]);
                triangles.Add(aFront[i]);
                triangles.Add(centerB);
                triangles.Add(aBack[i]);
                triangles.Add(aBack[j]);
            }

            return BuildMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateTorusMesh(int segments, int tubeSegments, Axis axis, float majorRadius, float tubeRadius)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var indices = new int[segments, tubeSegments];

            for (var i = 0; i < segments; i++)
            {
                var a = i * Mathf.PI * 2f / segments;
                var ca = Mathf.Cos(a);
                var sa = Mathf.Sin(a);
                for (var j = 0; j < tubeSegments; j++)
                {
                    var b = j * Mathf.PI * 2f / tubeSegments;
                    var cb = Mathf.Cos(b);
                    var sb = Mathf.Sin(b);
                    var radius = majorRadius + tubeRadius * cb;
                    Vector3 point;
                    if (axis == Axis.Y)
                    {
                        point = new Vector3(ca * radius, tubeRadius * sb, sa * radius);
                    }
                    else
                    {
                        point = new Vector3(ca * radius, sa * radius, tubeRadius * sb);
                    }

                    indices[i, j] = AddVertex(vertices, uvs, point, new Vector2(i / (float)segments, j / (float)tubeSegments));
                }
            }

            for (var i = 0; i < segments; i++)
            {
                var ni = (i + 1) % segments;
                for (var j = 0; j < tubeSegments; j++)
                {
                    var nj = (j + 1) % tubeSegments;
                    AddQuad(triangles, indices[i, j], indices[ni, j], indices[ni, nj], indices[i, nj]);
                }
            }

            return BuildMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateChamferedBoxMesh()
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var b = 0.42f;
            var points = new[]
            {
                new Vector3(-b, -0.5f, -0.5f), new Vector3(b, -0.5f, -0.5f), new Vector3(0.5f, -b, -0.5f), new Vector3(0.5f, b, -0.5f),
                new Vector3(b, 0.5f, -0.5f), new Vector3(-b, 0.5f, -0.5f), new Vector3(-0.5f, b, -0.5f), new Vector3(-0.5f, -b, -0.5f),
                new Vector3(-b, -0.5f, 0.5f), new Vector3(b, -0.5f, 0.5f), new Vector3(0.5f, -b, 0.5f), new Vector3(0.5f, b, 0.5f),
                new Vector3(b, 0.5f, 0.5f), new Vector3(-b, 0.5f, 0.5f), new Vector3(-0.5f, b, 0.5f), new Vector3(-0.5f, -b, 0.5f)
            };

            foreach (var p in points)
            {
                AddVertex(vertices, uvs, p, new Vector2(p.x + 0.5f, p.y + 0.5f));
            }

            for (var i = 0; i < 8; i++)
            {
                var j = (i + 1) % 8;
                AddQuad(triangles, i, j, j + 8, i + 8);
            }

            for (var i = 1; i < 7; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(8);
                triangles.Add(8 + i + 1);
                triangles.Add(8 + i);
            }

            return BuildMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateDomeMesh(int segments, int rings)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var grid = new int[rings + 1, segments];
            for (var r = 0; r <= rings; r++)
            {
                var v = r / (float)rings;
                var theta = v * Mathf.PI * 0.5f;
                var radius = Mathf.Cos(theta) * 0.5f;
                var z = -Mathf.Sin(theta) * 0.5f;
                for (var i = 0; i < segments; i++)
                {
                    var a = i * Mathf.PI * 2f / segments;
                    grid[r, i] = AddVertex(vertices, uvs, new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, z), new Vector2(i / (float)segments, v));
                }
            }

            for (var r = 0; r < rings; r++)
            {
                for (var i = 0; i < segments; i++)
                {
                    var j = (i + 1) % segments;
                    AddQuad(triangles, grid[r, i], grid[r, j], grid[r + 1, j], grid[r + 1, i]);
                }
            }

            return BuildMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateIrregularChipMesh()
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var pts = new[]
            {
                new Vector3(-0.48f, -0.15f, -0.50f),
                new Vector3(-0.08f, -0.22f, -0.50f),
                new Vector3(0.50f, -0.08f, -0.50f),
                new Vector3(0.36f, 0.13f, -0.50f),
                new Vector3(-0.30f, 0.18f, -0.50f)
            };
            var backOffset = new Vector3(0f, 0f, 1f);
            for (var i = 0; i < pts.Length; i++)
            {
                AddVertex(vertices, uvs, pts[i], new Vector2(pts[i].x + 0.5f, pts[i].y + 0.5f));
            }

            for (var i = 0; i < pts.Length; i++)
            {
                AddVertex(vertices, uvs, pts[i] + backOffset, new Vector2(pts[i].x + 0.5f, pts[i].y + 0.5f));
            }

            for (var i = 1; i < pts.Length - 1; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(pts.Length);
                triangles.Add(pts.Length + i + 1);
                triangles.Add(pts.Length + i);
            }

            for (var i = 0; i < pts.Length; i++)
            {
                var j = (i + 1) % pts.Length;
                AddQuad(triangles, i, j, j + pts.Length, i + pts.Length);
            }

            return BuildMesh(vertices, uvs, triangles);
        }

        private static Vector3 AxisPoint(Axis axis, float x, float along, float z)
        {
            if (axis == Axis.Y)
            {
                return new Vector3(x, along, z);
            }

            return new Vector3(x, z, along);
        }

        private static int AddVertex(List<Vector3> vertices, List<Vector2> uvs, Vector3 vertex, Vector2 uv)
        {
            vertices.Add(vertex);
            uvs.Add(uv);
            return vertices.Count - 1;
        }

        private static void AddQuad(List<int> triangles, int a, int b, int c, int d)
        {
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);
            triangles.Add(a);
            triangles.Add(c);
            triangles.Add(d);
        }

        private static Mesh BuildMesh(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
        {
            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            return mesh;
        }

        private static void RenderProofs()
        {
            RenderBeauty();
            RenderBreakdown();
            RenderMaterialSheet();
        }

        private static void RenderBeauty()
        {
            var sceneRoot = CreateRenderScene("CRGK13_RenderScene_Beauty");
            AddBackdrop(new Vector3(0f, -0.10f, 0.42f), new Vector3(3.4f, 4.8f, 1f));
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageRootAssetPath + "/Runtime/Prefabs/" + Prefix + "_PREFAB_GearKey_ConceptReplica.prefab");
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = "beauty_reference_style_gear_key_instance";
            instance.transform.position = new Vector3(-0.03f, 0.04f, -0.02f);
            instance.transform.localEulerAngles = new Vector3(0f, 0f, -6f);
            AddReferenceLabel("GEAR KEY", new Vector3(0.00f, -1.72f, -0.18f), 0.135f);
            var camera = AddCamera("BeautyCamera", new Vector3(0f, 0f, -5.8f), 1.98f);
            AddKeyLights();
            SaveCamera(camera, "CRGK13_RENDER_01_exact_reference_style_beauty.png");
            Object.DestroyImmediate(sceneRoot);
        }

        private static void RenderBreakdown()
        {
            var sceneRoot = CreateRenderScene("CRGK13_RenderScene_Breakdown");
            AddBackdrop(new Vector3(0f, 0f, 0.48f), new Vector3(4.8f, 3.6f, 1f));
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageRootAssetPath + "/Runtime/Prefabs/" + Prefix + "_PREFAB_GearKey_ConceptReplica.prefab");
            var left = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            left.name = "silhouette_read_instance";
            left.transform.position = new Vector3(-0.76f, 0.05f, -0.03f);
            left.transform.localScale = Vector3.one * 0.72f;
            ReplaceInstanceMaterials(left, Materials["dark"]);
            var right = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            right.name = "detail_lit_instance";
            right.transform.position = new Vector3(0.76f, 0.05f, -0.03f);
            right.transform.localScale = Vector3.one * 0.72f;
            AddReferenceLabel("silhouette", new Vector3(-0.76f, -1.18f, -0.20f), 0.105f);
            AddReferenceLabel("gear bow / collars / bit", new Vector3(0.76f, -1.18f, -0.20f), 0.085f);
            var camera = AddCamera("BreakdownCamera", new Vector3(0f, 0f, -5.4f), 1.55f);
            AddKeyLights();
            SaveCamera(camera, "CRGK13_RENDER_02_silhouette_detail_breakdown.png");
            Object.DestroyImmediate(sceneRoot);
        }

        private static void RenderMaterialSheet()
        {
            var sceneRoot = CreateRenderScene("CRGK13_RenderScene_MaterialSheet");
            AddBackdrop(new Vector3(0f, 0f, 0.52f), new Vector3(4.8f, 3.8f, 1f));
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageRootAssetPath + "/Runtime/Prefabs/" + Prefix + "_PREFAB_GearKey_ConceptReplica.prefab");
            var gear = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            gear.name = "gear_bow_closeup";
            gear.transform.position = new Vector3(-0.75f, 0.42f, -0.04f);
            gear.transform.localScale = Vector3.one * 1.42f;
            var shaft = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            shaft.name = "shaft_bit_closeup";
            shaft.transform.position = new Vector3(0.72f, -0.18f, -0.04f);
            shaft.transform.localScale = Vector3.one * 1.18f;
            shaft.transform.localEulerAngles = new Vector3(0f, 0f, 18f);
            AddMaterialSwatch("aged brass", Materials["brass"], new Vector3(-1.12f, -1.17f, -0.10f));
            AddMaterialSwatch("black crevice", Materials["dark"], new Vector3(-0.36f, -1.17f, -0.10f));
            AddMaterialSwatch("worn edge", Materials["edge"], new Vector3(0.40f, -1.17f, -0.10f));
            AddReferenceLabel("material / closeup sheet", new Vector3(0.00f, 1.22f, -0.20f), 0.105f);
            var camera = AddCamera("MaterialCamera", new Vector3(0f, 0f, -5.2f), 1.52f);
            AddKeyLights();
            SaveCamera(camera, "CRGK13_RENDER_03_material_closeup_contact_sheet.png");
            Object.DestroyImmediate(sceneRoot);
        }

        private static GameObject CreateRenderScene(string name)
        {
            var root = new GameObject(name);
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.075f, 0.058f, 0.040f);
            RenderSettings.reflectionIntensity = 0.18f;
            return root;
        }

        private static Camera AddCamera(string name, Vector3 position, float orthographicSize)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            go.transform.rotation = Quaternion.identity;
            var camera = go.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = orthographicSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.006f, 0.006f, 0.005f);
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 20f;
            camera.allowHDR = true;
            return camera;
        }

        private static void AddKeyLights()
        {
            AddLight("warm_upper_left_softbox", LightType.Point, new Vector3(-1.25f, 1.75f, -2.10f), new Color(1.0f, 0.70f, 0.38f), 10.5f, 4.4f);
            AddLight("narrow_right_rim_glint", LightType.Point, new Vector3(1.35f, 0.85f, -2.35f), new Color(1.0f, 0.62f, 0.25f), 4.8f, 3.2f);
            AddLight("dim_cool_fill_for_silhouette", LightType.Directional, new Vector3(0.1f, 2.0f, -1.8f), new Color(0.32f, 0.37f, 0.40f), 0.25f, 10f);
        }

        private static void AddLight(string name, LightType type, Vector3 position, Color color, float intensity, float range)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            if (type == LightType.Directional)
            {
                go.transform.rotation = Quaternion.Euler(48f, -18f, 0f);
            }

            var light = go.AddComponent<Light>();
            light.type = type;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.shadows = LightShadows.None;
        }

        private static void AddBackdrop(Vector3 position, Vector3 scale)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "render_only_dark_reference_backdrop_no_prefab";
            Object.DestroyImmediate(quad.GetComponent<Collider>());
            quad.transform.position = position;
            quad.transform.localScale = scale;
            var renderer = quad.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = Materials["backdrop"];
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        private static void AddReferenceLabel(string text, Vector3 position, float size)
        {
            var go = new GameObject("render_only_label_" + text.Replace(" ", "_"));
            go.transform.position = position;
            var mesh = go.AddComponent<TextMesh>();
            mesh.text = text;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.fontSize = 96;
            mesh.characterSize = size / 10f;
            mesh.color = new Color(0.82f, 0.68f, 0.45f);
            var renderer = go.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = 10;
            }
        }

        private static void AddMaterialSwatch(string label, Material material, Vector3 position)
        {
            var swatch = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            swatch.name = "render_only_swatch_" + label.Replace(" ", "_");
            Object.DestroyImmediate(swatch.GetComponent<Collider>());
            swatch.transform.position = position;
            swatch.transform.localScale = new Vector3(0.23f, 0.23f, 0.065f);
            var renderer = swatch.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            AddReferenceLabel(label, position + new Vector3(0f, -0.25f, -0.08f), 0.056f);
        }

        private static void ReplaceInstanceMaterials(GameObject root, Material material)
        {
            foreach (var renderer in root.GetComponentsInChildren<MeshRenderer>(true))
            {
                renderer.sharedMaterial = material;
            }
        }

        private static void SaveCamera(Camera camera, string fileName)
        {
            var path = Normalize(Path.Combine(_renderRoot, fileName));
            var texture = new RenderTexture(RenderWidth, RenderHeight, 24, RenderTextureFormat.ARGB32)
            {
                antiAliasing = 8
            };
            var previous = RenderTexture.active;
            camera.targetTexture = texture;
            RenderTexture.active = texture;
            camera.Render();
            var image = new Texture2D(RenderWidth, RenderHeight, TextureFormat.RGBA32, false, false);
            image.ReadPixels(new Rect(0, 0, RenderWidth, RenderHeight), 0, 0);
            image.Apply();
            File.WriteAllBytes(path, image.EncodeToPNG());
            File.Copy(path, Physical("Documentation~/Previews/" + fileName), true);
            RenderTexture.active = previous;
            camera.targetTexture = null;
            Object.DestroyImmediate(image);
            texture.Release();
            Object.DestroyImmediate(texture);
            RenderRecords.Add(new AssetRecord("Documentation/ConceptRenders/" + RenderFolderName + "/" + fileName, "render", "concept_replica_preview", "Unity batchmode render for Gear Key concept match review."));
        }

        private static ValidationResult ValidateGeneratedContent()
        {
            var validation = new ValidationResult();
            validation.TextureCount = Directory.GetFiles(Physical("Runtime/Textures"), "*.png", SearchOption.TopDirectoryOnly).Length;
            validation.MaterialCount = Directory.GetFiles(Physical("Runtime/Materials"), "*.mat", SearchOption.TopDirectoryOnly).Length;
            validation.MeshCount = Directory.GetFiles(Physical("Runtime/Meshes"), "*.asset", SearchOption.TopDirectoryOnly).Length;
            validation.PrefabCount = Directory.GetFiles(Physical("Runtime/Prefabs"), "*.prefab", SearchOption.TopDirectoryOnly).Length;
            validation.RenderCount = Directory.GetFiles(_renderRoot, "*.png", SearchOption.TopDirectoryOnly).Length;

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageRootAssetPath + "/Runtime/Prefabs/" + Prefix + "_PREFAB_GearKey_ConceptReplica.prefab");
            if (prefab == null)
            {
                validation.Failures.Add("Missing Gear Key visual prefab.");
            }
            else
            {
                var allowed = new HashSet<Type> { typeof(Transform), typeof(MeshFilter), typeof(MeshRenderer) };
                foreach (var component in prefab.GetComponentsInChildren<Component>(true))
                {
                    if (component == null)
                    {
                        validation.Failures.Add("Prefab contains missing script/component reference.");
                        continue;
                    }

                    if (!allowed.Contains(component.GetType()))
                    {
                        validation.Failures.Add("Disallowed prefab component: " + component.GetType().Name);
                    }
                }

                var renderers = prefab.GetComponentsInChildren<MeshRenderer>(true).Length;
                if (renderers < 50)
                {
                    validation.Failures.Add("Expected high-detail proof with at least 50 mesh renderers; found " + renderers.ToString(CultureInfo.InvariantCulture) + ".");
                }
            }

            if (validation.TextureCount < 8) validation.Failures.Add("Expected at least 8 generated texture maps.");
            if (validation.MaterialCount < 5) validation.Failures.Add("Expected at least 5 materials.");
            if (validation.MeshCount < 10) validation.Failures.Add("Expected at least 10 custom mesh assets.");
            if (validation.PrefabCount < 1) validation.Failures.Add("Expected one visual prefab.");
            if (validation.RenderCount < 3) validation.Failures.Add("Expected three PNG render outputs.");

            validation.Passed = validation.Failures.Count == 0;
            return validation;
        }

        private static void WriteMetadata(ValidationResult validation)
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture);
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"package\": \"" + PackageName + "\",");
            sb.AppendLine("  \"version\": \"" + Version + "\",");
            sb.AppendLine("  \"marker\": \"" + Prefix + "\",");
            sb.AppendLine("  \"generatedAtLocal\": \"" + Escape(now) + "\",");
            sb.AppendLine("  \"conceptReference\": \"north-star Gear Key crop: round gear bow, missing teeth, central hub, aged bronze shaft, lower side bit, dark background\",");
            sb.AppendLine("  \"verdict\": \"PASS as Unity-only generation pipeline proof; FAIL as exact final concept match and not ready for playable promotion except temporary placeholder use.\",");
            sb.AppendLine("  \"validation\": {");
            sb.AppendLine("    \"passed\": " + (validation.Passed ? "true" : "false") + ",");
            sb.AppendLine("    \"prefabs\": " + validation.PrefabCount.ToString(CultureInfo.InvariantCulture) + ",");
            sb.AppendLine("    \"meshes\": " + validation.MeshCount.ToString(CultureInfo.InvariantCulture) + ",");
            sb.AppendLine("    \"materials\": " + validation.MaterialCount.ToString(CultureInfo.InvariantCulture) + ",");
            sb.AppendLine("    \"textures\": " + validation.TextureCount.ToString(CultureInfo.InvariantCulture) + ",");
            sb.AppendLine("    \"renders\": " + validation.RenderCount.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine("  },");
            sb.AppendLine("  \"assets\": [");
            var records = TextureRecords.Concat(MaterialRecords).Concat(MeshRecords).Concat(PrefabRecords).Concat(RenderRecords).ToList();
            for (var i = 0; i < records.Count; i++)
            {
                var r = records[i];
                sb.Append("    { \"path\": \"" + Escape(r.Path) + "\", \"type\": \"" + Escape(r.Type) + "\", \"tag\": \"" + Escape(r.Tag) + "\", \"notes\": \"" + Escape(r.Notes) + "\" }");
                sb.AppendLine(i == records.Count - 1 ? "" : ",");
            }
            sb.AppendLine("  ]");
            sb.AppendLine("}");

            var metadataPath = Physical("Runtime/Metadata/" + Prefix + "_ConceptReplicaGearKeySet13_Catalog.json");
            File.WriteAllText(metadataPath, sb.ToString(), Encoding.UTF8);
            AssetDatabase.ImportAsset(PackageRootAssetPath + "/Runtime/Metadata/" + Prefix + "_ConceptReplicaGearKeySet13_Catalog.json", ImportAssetOptions.ForceSynchronousImport);
        }

        private static void WriteDocs(ValidationResult validation)
        {
            var plan = new StringBuilder();
            plan.AppendLine("# Concept Replica Gear Key Set13 - Implementation Plan");
            plan.AppendLine();
            plan.AppendLine("Generated: `" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture) + "`");
            plan.AppendLine();
            plan.AppendLine("## Target");
            plan.AppendLine("Recreate the north-star Gear Key crop as a focused Unity-only visual proof: round gear bow with missing/alternating teeth, inner ring and spokes, central riveted hub, long aged bronze shaft, collar ridges, lower side bit, dark studio backdrop, and warm brass glints.");
            plan.AppendLine();
            plan.AppendLine("## Method");
            plan.AppendLine("- Custom Unity-generated meshes for the gear bow, rings, cylinders, collars, box/chip details, and domed rivets.");
            plan.AppendLine("- Procedural PBR texture maps for aged brass, blackened recesses, worn edge glints, occlusion, and roughness variation.");
            plan.AppendLine("- Render-only dark backdrop, warm upper-left key light, right rim glint, and dim fill to match the concept crop's low-key lighting.");
            plan.AppendLine("- Visual-only prefab: no colliders, no scripts, no physics, no lights, no cameras.");
            plan.AppendLine();
            plan.AppendLine("## Promotion Notes");
            plan.AppendLine("Use the prefab as a pickup visual only after a scale pass in the playable project. For final shipping art, follow with hand-authored UV refinement, extra bevel fidelity, and an in-game readability/material pass.");
            File.WriteAllText(Path.Combine(_planningRoot, "ConceptReplicaGearKeySet13_ImplementationPlan.md"), plan.ToString(), Encoding.UTF8);

            var qa = new StringBuilder();
            qa.AppendLine("# Concept Replica Gear Key Set13 - QA");
            qa.AppendLine();
            qa.AppendLine("Generated: `" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture) + "`");
            qa.AppendLine();
            qa.AppendLine("## Unity Generator Result");
            qa.AppendLine("- Marker: `" + Prefix + "_GENERATE_PASS marker=" + Version + "`");
            qa.AppendLine("- Prefabs: `" + validation.PrefabCount.ToString(CultureInfo.InvariantCulture) + "`");
            qa.AppendLine("- Meshes: `" + validation.MeshCount.ToString(CultureInfo.InvariantCulture) + "`");
            qa.AppendLine("- Materials: `" + validation.MaterialCount.ToString(CultureInfo.InvariantCulture) + "`");
            qa.AppendLine("- Textures: `" + validation.TextureCount.ToString(CultureInfo.InvariantCulture) + "`");
            qa.AppendLine("- Renders: `" + validation.RenderCount.ToString(CultureInfo.InvariantCulture) + "`");
            qa.AppendLine("- Prefab component gate: `" + (validation.Passed ? "PASS" : "FAIL") + "`");
            qa.AppendLine();
            qa.AppendLine("## Self-Score Against Reference");
            qa.AppendLine("| Category | Score | Notes |");
            qa.AppendLine("| --- | ---: | --- |");
            qa.AppendLine("| Silhouette | 6.0/10 | Diagonal key read, gear bow, long shaft, and lower side bit are present, but the bow still reads too chunky and mechanical compared with the slender crop. |");
            qa.AppendLine("| Gear bow shape | 5.0/10 | Teeth, missing zones, ring, spokes, and hub are modeled, but the negative space and tooth proportions are not close enough to call this an exact replica. |");
            qa.AppendLine("| Shaft/bit proportions | 6.5/10 | Shaft length and side bit are present. The bit now sits closer to the crop position, but the shaft is too heavy and the collars need finer scale. |");
            qa.AppendLine("| Surface aging | 5.5/10 | Procedural pits, scratches, blackened seams, tarnish, and worn glints exist, but the surface still reads generated rather than authored antique brass. |");
            qa.AppendLine("| Color/roughness | 6.0/10 | Dark bronze target is closer after the second pass, but the render remains too orange in warm highlights and needs calibrated reflection/context lighting. |");
            qa.AppendLine("| Render lighting | 6.0/10 | Dark background and warm light are in the right family. Composition is useful, but the render does not yet match the crop's painterly compactness. |");
            qa.AppendLine("| Final-art readiness | 3.5/10 | Pipeline proof only. Not AAA final art and not ready to promote as the canonical playable pickup visual. |");
            qa.AppendLine();
            qa.AppendLine("## Honest Verdict");
            qa.AppendLine("PASS as a Unity-only asset generation proof. FAIL as an exact concept-art replica and fail as final shipping/AAA art. Do not promote this as the canonical playable Gear Key pickup yet; use it only as a temporary placeholder if gameplay needs an object while the next replica pass is built.");
            if (validation.Failures.Count > 0)
            {
                qa.AppendLine();
                qa.AppendLine("## Failures");
                foreach (var failure in validation.Failures)
                {
                    qa.AppendLine("- " + failure);
                }
            }
            File.WriteAllText(Path.Combine(_qaRoot, "ConceptReplicaGearKeySet13_QA.md"), qa.ToString(), Encoding.UTF8);
            File.WriteAllText(Physical("Documentation~/QA/ConceptReplicaGearKeySet13_QA.md"), qa.ToString(), Encoding.UTF8);
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string Physical(string relative)
        {
            return Normalize(Path.Combine(_packageRoot, relative));
        }

        private static string Normalize(string path)
        {
            return path.Replace('\\', '/');
        }

        private static void ResetDirectory(string path)
        {
            var normalized = Normalize(path);
            if (Directory.Exists(normalized))
            {
                Directory.Delete(normalized, true);
            }

            Directory.CreateDirectory(normalized);
        }

        private static void ReplaceAsset(string assetPath)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
        }

        private struct AssetRecord
        {
            public readonly string Path;
            public readonly string Type;
            public readonly string Tag;
            public readonly string Notes;

            public AssetRecord(string path, string type, string tag, string notes)
            {
                Path = path;
                Type = type;
                Tag = tag;
                Notes = notes;
            }
        }

        private sealed class ValidationResult
        {
            public bool Passed;
            public int PrefabCount;
            public int MeshCount;
            public int MaterialCount;
            public int TextureCount;
            public int RenderCount;
            public readonly List<string> Failures = new List<string>();
        }

        private enum Axis
        {
            Y,
            Z
        }
    }
}
#endif
