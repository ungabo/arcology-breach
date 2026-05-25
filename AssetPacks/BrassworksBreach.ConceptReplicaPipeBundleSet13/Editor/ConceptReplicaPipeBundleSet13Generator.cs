#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace BrassworksBreach.ConceptReplicaPipeBundleSet13.Editor
{
    public static class ConceptReplicaPipeBundleSet13Generator
    {
        private const string PackageName = "com.brassworks.sidecar.concept-replica-pipe-bundle-set13";
        private const string PackageAssetRoot = "Packages/" + PackageName;
        private const string Version = "0.1.57-p013";
        private const string Prefix = "CRPB13";
        private const string RenderFolderName = "V0_1_57_ConceptReplicaPipeBundleSet13";
        private const int TextureSize = 768;
        private const int RenderWidth = 1800;
        private const int RenderHeight = 1120;

        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly List<AssetRecord> TextureRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> MaterialRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> MeshRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> PrefabRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> RenderRecords = new List<AssetRecord>();

        private static string _packagePhysicalRoot = "";
        private static string _repoRoot = "";

        [MenuItem("Brassworks/Sidecars/Generate Concept Replica Pipe Bundle Set13")]
        public static void GenerateAll()
        {
            TextureRecords.Clear();
            MaterialRecords.Clear();
            MeshRecords.Clear();
            PrefabRecords.Clear();
            RenderRecords.Clear();
            Meshes.Clear();
            Materials.Clear();

            ResolveRoots();
            ResetOwnedOutputs();
            PrepareFolders();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            GenerateTextures();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            ConfigureTextureImporters();
            CreateMaterials();
            CreateMeshes();
            CreatePrefabs();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            RenderProofs();
            WriteMetadata();
            WritePackageDocs();
            WritePlanningDoc();
            var validation = ValidateVisualOnlyPrefabs();
            WriteQaDoc(validation);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            if (!validation.Passed)
            {
                throw new InvalidOperationException(Prefix + "_VALIDATION_FAIL " + string.Join("; ", validation.Failures.ToArray()));
            }

            Debug.Log(Prefix + "_GENERATE_PASS marker=" + Version +
                      " prefabs=" + PrefabRecords.Count.ToString(CultureInfo.InvariantCulture) +
                      " materials=" + MaterialRecords.Count.ToString(CultureInfo.InvariantCulture) +
                      " textures=" + TextureRecords.Count.ToString(CultureInfo.InvariantCulture) +
                      " meshes=" + MeshRecords.Count.ToString(CultureInfo.InvariantCulture) +
                      " renders=" + RenderRecords.Count.ToString(CultureInfo.InvariantCulture));
        }

        private static void ResolveRoots()
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForPackageName(PackageName);
            if (packageInfo == null || string.IsNullOrEmpty(packageInfo.resolvedPath))
            {
                throw new InvalidOperationException("Could not resolve local package " + PackageName + ".");
            }

            _packagePhysicalRoot = NormalizePath(packageInfo.resolvedPath);
            var assetPacksRoot = Directory.GetParent(_packagePhysicalRoot);
            if (assetPacksRoot == null || assetPacksRoot.Parent == null)
            {
                throw new InvalidOperationException("Could not derive repo root from " + _packagePhysicalRoot);
            }

            _repoRoot = NormalizePath(assetPacksRoot.Parent.FullName);
        }

        private static void ResetOwnedOutputs()
        {
            DeleteDirectoryIfExists(Physical("Runtime"));
            DeleteDirectoryIfExists(Physical("Documentation~"));
            DeleteDirectoryIfExists(ConceptRenderRoot());
            DeleteFileIfExists(Path.Combine(PlanningRoot(), "ConceptReplicaPipeBundleSet13_ImplementationPlan.md"));
            DeleteFileIfExists(Path.Combine(QaRoot(), "ConceptReplicaPipeBundleSet13_QA.md"));
        }

        private static void PrepareFolders()
        {
            foreach (var relative in new[]
            {
                "Runtime/Materials",
                "Runtime/Meshes",
                "Runtime/Metadata",
                "Runtime/Prefabs",
                "Runtime/Textures",
                "Documentation~/Manifest",
                "Documentation~/Previews",
                "Documentation~/QA"
            })
            {
                Directory.CreateDirectory(Physical(relative));
            }

            Directory.CreateDirectory(ConceptRenderRoot());
            Directory.CreateDirectory(PlanningRoot());
            Directory.CreateDirectory(QaRoot());
        }

        private static void GenerateTextures()
        {
            foreach (var spec in MaterialSpecs())
            {
                SaveTexture(spec.Name + "_Albedo", CreateAlbedoTexture(spec), "Runtime/Textures", false, false, spec.Tag);
                SaveTexture(spec.Name + "_Normal", CreateNormalTexture(spec), "Runtime/Textures", true, false, spec.Tag);
                SaveTexture(spec.Name + "_MetallicSmoothness", CreateMaskTexture(spec), "Runtime/Textures", false, true, spec.Tag);
                SaveTexture(spec.Name + "_Occlusion", CreateOcclusionTexture(spec), "Runtime/Textures", false, true, spec.Tag);
            }

            SaveTexture("SteamHaze_SoftAlpha", CreateSteamAlphaTexture(), "Runtime/Textures", false, false, "transparent steam silhouette");
        }

        private static void ConfigureTextureImporters()
        {
            foreach (var record in TextureRecords)
            {
                var importer = AssetImporter.GetAtPath(PackageAssetRoot + "/" + record.Path) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                importer.mipmapEnabled = true;
                importer.textureCompression = TextureImporterCompression.CompressedHQ;
                if (record.Name.EndsWith("_Normal", StringComparison.Ordinal))
                {
                    importer.textureType = TextureImporterType.NormalMap;
                    importer.sRGBTexture = false;
                }
                else if (record.Name.EndsWith("_MetallicSmoothness", StringComparison.Ordinal) ||
                         record.Name.EndsWith("_Occlusion", StringComparison.Ordinal))
                {
                    importer.sRGBTexture = false;
                }
                else if (record.Name.Contains("SteamHaze"))
                {
                    importer.alphaSource = TextureImporterAlphaSource.FromInput;
                    importer.alphaIsTransparency = true;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                }

                importer.SaveAndReimport();
            }
        }

        private static void CreateMaterials()
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible Lit/Standard shader was found.");
            }

            foreach (var spec in MaterialSpecs())
            {
                var material = new Material(shader) { name = Prefix + "_MAT_" + spec.Name };
                SetMaterialColor(material, spec.BaseColor);
                SetMaterialFloat(material, "_Metallic", spec.Metallic);
                SetMaterialFloat(material, "_Smoothness", spec.Smoothness);
                SetMaterialFloat(material, "_Glossiness", spec.Smoothness);

                SetMaterialTexture(material, "_BaseMap", "_MainTex", LoadTexture(spec.Name + "_Albedo"));
                SetMaterialTexture(material, "_BumpMap", "_BumpMap", LoadTexture(spec.Name + "_Normal"));
                material.EnableKeyword("_NORMALMAP");
                SetMaterialTexture(material, "_MetallicGlossMap", "_MetallicGlossMap", LoadTexture(spec.Name + "_MetallicSmoothness"));
                material.EnableKeyword("_METALLICGLOSSMAP");
                material.EnableKeyword("_METALLICSPECGLOSSMAP");
                SetMaterialTexture(material, "_OcclusionMap", "_OcclusionMap", LoadTexture(spec.Name + "_Occlusion"));
                Materials[spec.Key] = SaveAsset(material, "Runtime/Materials/" + material.name + ".mat", MaterialRecords, spec.Tag);
            }

            var steam = new Material(shader) { name = Prefix + "_MAT_SteamHazeSoftBillow" };
            SetMaterialColor(steam, new Color(0.72f, 0.67f, 0.58f, 0.36f));
            SetMaterialTexture(steam, "_BaseMap", "_MainTex", LoadTexture("SteamHaze_SoftAlpha"));
            ConfigureTransparent(steam);
            Materials["Steam"] = SaveAsset(steam, "Runtime/Materials/" + steam.name + ".mat", MaterialRecords, "soft transparent steam source hint");
        }

        private static void CreateMeshes()
        {
            Meshes["Box"] = SaveAsset(CreateBoxMesh(), "Runtime/Meshes/" + Prefix + "_MESH_BoxUnit.asset", MeshRecords, "beveled brick and plate primitive");
            Meshes["CylinderX"] = SaveAsset(CreateCylinderMesh(Axis.X, 48), "Runtime/Meshes/" + Prefix + "_MESH_Cylinder48_X.asset", MeshRecords, "straight horizontal pipe primitive");
            Meshes["CylinderY"] = SaveAsset(CreateCylinderMesh(Axis.Y, 48), "Runtime/Meshes/" + Prefix + "_MESH_Cylinder48_Y.asset", MeshRecords, "vertical pipe primitive");
            Meshes["CylinderZ"] = SaveAsset(CreateCylinderMesh(Axis.Z, 48), "Runtime/Meshes/" + Prefix + "_MESH_Cylinder48_Z.asset", MeshRecords, "depth connector and valve primitive");
            Meshes["TorusX"] = SaveAsset(CreateTorusMesh(Axis.X, 56, 10, 1.0f, 0.14f), "Runtime/Meshes/" + Prefix + "_MESH_Torus56_X.asset", MeshRecords, "flange ring around horizontal pipe");
            Meshes["TorusY"] = SaveAsset(CreateTorusMesh(Axis.Y, 56, 10, 1.0f, 0.14f), "Runtime/Meshes/" + Prefix + "_MESH_Torus56_Y.asset", MeshRecords, "flange ring around vertical pipe");
            Meshes["TorusZ"] = SaveAsset(CreateTorusMesh(Axis.Z, 56, 10, 1.0f, 0.12f), "Runtime/Meshes/" + Prefix + "_MESH_Torus56_Z.asset", MeshRecords, "valve handwheel and face ring");
            Meshes["ElbowXY"] = SaveAsset(CreateQuarterElbowMesh(36, 14, 1.0f, 0.20f), "Runtime/Meshes/" + Prefix + "_MESH_QuarterElbow_XY.asset", MeshRecords, "90 degree pipe elbow module");
            Meshes["DomeZ"] = SaveAsset(CreateDomeMesh(18, 7), "Runtime/Meshes/" + Prefix + "_MESH_RivetDome_Z.asset", MeshRecords, "bolt and rivet cap");
            Meshes["Plane"] = SaveAsset(CreatePlaneMesh(), "Runtime/Meshes/" + Prefix + "_MESH_SoftPlane.asset", MeshRecords, "transparent steam and wet shadow plane");
        }

        private static void CreatePrefabs()
        {
            SavePrefab(BuildStraightPipeModule(), Prefix + "_PREFAB_StraightPipeModule", "modular aged copper straight pipe");
            SavePrefab(BuildElbowPipeModule(), Prefix + "_PREFAB_ElbowPipeModule", "modular 90 degree elbow with collars");
            SavePrefab(BuildFlangeCollarModule(), Prefix + "_PREFAB_FlangeCollarModule", "reusable bolted flange/collar");
            SavePrefab(BuildWallBracketClampModule(), Prefix + "_PREFAB_WallBracketClampModule", "wall bracket and pipe clamp module");
            SavePrefab(BuildValveHandwheelModule(), Prefix + "_PREFAB_ValveHandwheelModule", "small valve and handwheel module");
            SavePrefab(BuildDarkMasonryBackingModule(), Prefix + "_PREFAB_DarkWetMasonryBackingModule", "dark wet masonry backing module");
            SavePrefab(BuildPipeBundleAssembly(), Prefix + "_PREFAB_AssembledConceptReplicaPipeBundle", "reference-style assembled pipe bundle crop");
        }

        private static GameObject BuildStraightPipeModule()
        {
            var root = new GameObject(Prefix + "_StraightPipeModule");
            AddPipeX(root, "aged_copper_pipe_core", Vector3.zero, 2.35f, 0.13f, Materials["Copper"], true);
            AddCollarX(root, "left_bolted_collar", -0.92f, 0.19f, 0.14f);
            AddCollarX(root, "center_blackened_collar", 0.00f, 0.17f, 0.10f);
            AddCollarX(root, "right_bolted_collar", 0.92f, 0.19f, 0.14f);
            AddFlangeX(root, "left_terminal_flange", -1.18f, 0.24f, 0.12f);
            AddFlangeX(root, "right_terminal_flange", 1.18f, 0.24f, 0.12f);
            AddTarnishStreaksOnPipe(root, 10, 2.18f, 0.142f, -0.18f);
            return root;
        }

        private static GameObject BuildElbowPipeModule()
        {
            var root = new GameObject(Prefix + "_ElbowPipeModule");
            AddMesh(root, "quarter_elbow_aged_copper_body", Meshes["ElbowXY"], Materials["Copper"], new Vector3(-0.46f, -0.44f, 0f), Quaternion.identity, Vector3.one * 0.48f);
            AddPipeX(root, "short_horizontal_socket", new Vector3(-0.86f, 0.04f, 0f), 0.58f, 0.12f, Materials["Copper"], false);
            AddPipeY(root, "short_vertical_socket", new Vector3(0.02f, -0.84f, 0f), 0.58f, 0.12f, Materials["Copper"], false);
            AddCollarXAt(root, "horizontal_exit_collar", new Vector3(-1.16f, 0.04f, 0f), 0.18f, 0.13f);
            AddCollarYAt(root, "vertical_exit_collar", new Vector3(0.02f, -1.14f, 0f), 0.18f, 0.13f);
            AddGrimeBandX(root, "elbow_horizontal_soot_ring", new Vector3(-0.58f, 0.04f, 0f), 0.144f, 0.035f);
            AddGrimeBandY(root, "elbow_vertical_soot_ring", new Vector3(0.02f, -0.58f, 0f), 0.144f, 0.035f);
            return root;
        }

        private static GameObject BuildFlangeCollarModule()
        {
            var root = new GameObject(Prefix + "_FlangeCollarModule");
            AddPipeX(root, "short_pipe_visible_through_collar", Vector3.zero, 0.58f, 0.11f, Materials["Copper"], true);
            AddFlangeX(root, "wide_bolted_front_flange", -0.06f, 0.29f, 0.16f);
            AddMesh(root, "dark_inner_bore_shadow", Meshes["CylinderX"], Materials["Soot"], new Vector3(-0.145f, 0f, -0.002f), Quaternion.identity, new Vector3(0.018f, 0.15f, 0.15f));
            AddBoltRing(root, "front_bolt_ring", new Vector3(-0.16f, 0f, -0.188f), 0.205f, Axis.X, 8, Materials["Brass"], 0.026f);
            return root;
        }

        private static GameObject BuildWallBracketClampModule()
        {
            var root = new GameObject(Prefix + "_WallBracketClampModule");
            AddBox(root, "blackened_wall_mount_plate", new Vector3(0f, 0f, 0.065f), new Vector3(0.64f, 0.52f, 0.09f), Materials["Iron"]);
            AddBox(root, "left_brass_side_lip", new Vector3(-0.36f, 0f, -0.005f), new Vector3(0.055f, 0.62f, 0.075f), Materials["Brass"]);
            AddBox(root, "right_brass_side_lip", new Vector3(0.36f, 0f, -0.005f), new Vector3(0.055f, 0.62f, 0.075f), Materials["Brass"]);
            AddPipeX(root, "clamped_reference_pipe", new Vector3(0f, 0f, -0.145f), 0.76f, 0.10f, Materials["Copper"], false);
            AddBox(root, "upper_iron_clamp_strap", new Vector3(0f, 0.142f, -0.185f), new Vector3(0.58f, 0.045f, 0.052f), Materials["Iron"]);
            AddBox(root, "lower_iron_clamp_strap", new Vector3(0f, -0.142f, -0.185f), new Vector3(0.58f, 0.045f, 0.052f), Materials["Iron"]);
            AddRivet(root, "upper_left_mount_bolt", new Vector3(-0.245f, 0.175f, -0.01f), 0.035f, Materials["Brass"], Quaternion.identity);
            AddRivet(root, "upper_right_mount_bolt", new Vector3(0.245f, 0.175f, -0.01f), 0.035f, Materials["Brass"], Quaternion.identity);
            AddRivet(root, "lower_left_mount_bolt", new Vector3(-0.245f, -0.175f, -0.01f), 0.035f, Materials["Brass"], Quaternion.identity);
            AddRivet(root, "lower_right_mount_bolt", new Vector3(0.245f, -0.175f, -0.01f), 0.035f, Materials["Brass"], Quaternion.identity);
            return root;
        }

        private static GameObject BuildValveHandwheelModule()
        {
            var root = new GameObject(Prefix + "_ValveHandwheelModule");
            AddPipeX(root, "small_inline_valve_pipe", Vector3.zero, 0.86f, 0.075f, Materials["Copper"], true);
            AddMesh(root, "round_valve_body", Meshes["CylinderZ"], Materials["Brass"], new Vector3(0f, 0f, -0.02f), Quaternion.identity, new Vector3(0.19f, 0.19f, 0.14f));
            AddPipeY(root, "upright_valve_stem", new Vector3(0f, 0.235f, -0.02f), 0.32f, 0.032f, Materials["Iron"], false);
            AddMesh(root, "handwheel_outer_ring", Meshes["TorusZ"], Materials["Brass"], new Vector3(0f, 0.43f, -0.02f), Quaternion.identity, Vector3.one * 0.118f);
            AddPipeX(root, "handwheel_horizontal_spoke", new Vector3(0f, 0.43f, -0.02f), 0.34f, 0.012f, Materials["Iron"], false);
            AddPipeY(root, "handwheel_vertical_spoke", new Vector3(0f, 0.43f, -0.02f), 0.34f, 0.012f, Materials["Iron"], false);
            AddRivet(root, "handwheel_center_hub", new Vector3(0f, 0.43f, -0.155f), 0.036f, Materials["Brass"], Quaternion.identity);
            AddBox(root, "tiny_red_valve_tick", new Vector3(0.14f, 0.43f, -0.14f), new Vector3(0.055f, 0.018f, 0.025f), Materials["Red"]);
            return root;
        }

        private static GameObject BuildDarkMasonryBackingModule()
        {
            var root = new GameObject(Prefix + "_DarkWetMasonryBackingModule");
            AddBox(root, "deep_mortar_backing_slab", new Vector3(0f, 1.2f, 0.10f), new Vector3(3.6f, 2.45f, 0.16f), Materials["Masonry"]);
            AddBrickField(root, new Vector3(-1.65f, 0.26f, -0.01f), 10, 7, 1513, false);
            AddBox(root, "floor_shadow_strip", new Vector3(0f, -0.08f, -0.24f), new Vector3(3.7f, 0.12f, 0.52f), Materials["Soot"]);
            AddBox(root, "wet_wall_reflection_patch_left", new Vector3(-1.05f, 0.74f, -0.092f), new Vector3(0.48f, 0.035f, 0.008f), Materials["Amber"]);
            AddBox(root, "wet_wall_reflection_patch_right", new Vector3(1.18f, 1.54f, -0.092f), new Vector3(0.34f, 0.030f, 0.008f), Materials["Amber"]);
            return root;
        }

        private static GameObject BuildPipeBundleAssembly()
        {
            var root = new GameObject(Prefix + "_AssembledConceptReplicaPipeBundle");
            var wall = BuildDarkMasonryBackingModule();
            wall.transform.SetParent(root.transform, false);

            AddPipeX(root, "low_front_horizontal_pipe", new Vector3(0.05f, 0.40f, -0.35f), 3.30f, 0.125f, Materials["Copper"], true);
            AddCollarXAt(root, "low_pipe_left_flange", new Vector3(-1.48f, 0.40f, -0.35f), 0.20f, 0.15f);
            AddCollarXAt(root, "low_pipe_mid_band_01", new Vector3(-0.73f, 0.40f, -0.35f), 0.17f, 0.12f);
            AddCollarXAt(root, "low_pipe_mid_band_02", new Vector3(0.15f, 0.40f, -0.35f), 0.18f, 0.12f);
            AddCollarXAt(root, "low_pipe_right_band", new Vector3(0.92f, 0.40f, -0.35f), 0.17f, 0.12f);
            AddFlangeX(root, "low_pipe_right_terminal_flange", 1.62f, 0.23f, 0.13f, new Vector3(0f, 0.40f, -0.35f));
            AddTarnishStreaksOnPipe(root, 14, 3.10f, 0.139f, -0.35f, 0.40f);

            AddPipeY(root, "left_vertical_riser", new Vector3(-1.40f, 1.14f, -0.34f), 1.48f, 0.125f, Materials["Copper"], true);
            AddCollarYAt(root, "left_riser_lower_collar", new Vector3(-1.40f, 0.66f, -0.34f), 0.18f, 0.12f);
            AddCollarYAt(root, "left_riser_upper_collar", new Vector3(-1.40f, 1.66f, -0.34f), 0.18f, 0.12f);
            AddMesh(root, "left_reference_elbow", Meshes["ElbowXY"], Materials["Copper"], new Vector3(-1.40f, 0.40f, -0.34f), Quaternion.Euler(0f, 0f, 180f), Vector3.one * 0.38f);
            AddGrimeBandY(root, "left_riser_soot_ring", new Vector3(-1.40f, 1.20f, -0.34f), 0.144f, 0.035f);

            AddPipeY(root, "right_vertical_pipe_front", new Vector3(0.98f, 1.28f, -0.29f), 2.05f, 0.105f, Materials["Copper"], true);
            AddPipeY(root, "right_vertical_pipe_back", new Vector3(1.36f, 1.30f, -0.17f), 2.12f, 0.100f, Materials["Copper"], true);
            AddCollarYAt(root, "front_vertical_lower_collar", new Vector3(0.98f, 0.52f, -0.29f), 0.16f, 0.11f);
            AddCollarYAt(root, "front_vertical_upper_collar", new Vector3(0.98f, 1.92f, -0.29f), 0.16f, 0.11f);
            AddCollarYAt(root, "rear_vertical_lower_collar", new Vector3(1.36f, 0.62f, -0.17f), 0.15f, 0.10f);
            AddCollarYAt(root, "rear_vertical_upper_collar", new Vector3(1.36f, 1.96f, -0.17f), 0.15f, 0.10f);

            AddPipeZ(root, "wall_branch_pipe", new Vector3(-0.24f, 1.22f, -0.18f), 0.44f, 0.085f, Materials["Copper"], true);
            AddMesh(root, "wall_branch_dark_bore", Meshes["CylinderZ"], Materials["Soot"], new Vector3(-0.24f, 1.22f, -0.43f), Quaternion.identity, new Vector3(0.072f, 0.072f, 0.025f));
            AddFlangeZ(root, "wall_branch_bolted_flange", new Vector3(-0.24f, 1.22f, -0.20f), 0.22f, 0.11f);
            AddSmallValveAt(root, new Vector3(-0.58f, 1.18f, -0.35f), 0.48f);
            AddBracketAt(root, new Vector3(0.98f, 1.16f, -0.20f), true);
            AddBracketAt(root, new Vector3(1.36f, 1.54f, -0.10f), true);
            AddBracketAt(root, new Vector3(-1.40f, 1.25f, -0.25f), true);

            AddSteamPlane(root, "left_floor_steam_puff_low", new Vector3(-1.82f, 0.42f, -0.48f), new Vector3(0.38f, 0.52f, 1f), -18f);
            AddSteamPlane(root, "upper_pipe_wisp", new Vector3(-1.22f, 1.88f, -0.48f), new Vector3(0.24f, 0.40f, 1f), 9f);
            AddWarmSpecCards(root);
            return root;
        }

        private static void AddWarmSpecCards(GameObject parent)
        {
            AddBox(parent, "subtle_low_pipe_wet_edge", new Vector3(-0.42f, 0.51f, -0.482f), new Vector3(0.42f, 0.006f, 0.004f), Materials["Soot"]);
            AddBox(parent, "subtle_vertical_pipe_wet_edge", new Vector3(1.06f, 1.32f, -0.398f), new Vector3(0.006f, 0.36f, 0.004f), Materials["Soot"]);
        }

        private static void AddSmallValveAt(GameObject parent, Vector3 position, float scale)
        {
            AddMesh(parent, "assembled_valve_body", Meshes["CylinderZ"], Materials["Brass"], position, Quaternion.identity, new Vector3(0.15f, 0.15f, 0.10f) * scale);
            AddPipeY(parent, "assembled_valve_stem", position + new Vector3(0f, 0.20f * scale, 0.005f), 0.30f * scale, 0.025f * scale, Materials["Iron"], false);
            AddMesh(parent, "assembled_valve_handwheel", Meshes["TorusZ"], Materials["Brass"], position + new Vector3(0f, 0.38f * scale, -0.04f), Quaternion.identity, Vector3.one * 0.105f * scale);
            AddPipeX(parent, "assembled_valve_wheel_spoke", position + new Vector3(0f, 0.38f * scale, -0.04f), 0.31f * scale, 0.010f * scale, Materials["Iron"], false);
            AddPipeY(parent, "assembled_valve_wheel_spoke_vertical", position + new Vector3(0f, 0.38f * scale, -0.04f), 0.31f * scale, 0.010f * scale, Materials["Iron"], false);
        }

        private static void AddBracketAt(GameObject parent, Vector3 position, bool vertical)
        {
            AddBox(parent, "wall_bracket_plate_" + SafeName(position), position + new Vector3(0f, 0f, 0.105f), new Vector3(0.40f, 0.38f, 0.055f), Materials["Iron"]);
            AddBox(parent, "wall_bracket_top_lip_" + SafeName(position), position + new Vector3(0f, 0.15f, 0.055f), new Vector3(0.44f, 0.045f, 0.052f), Materials["Brass"]);
            AddBox(parent, "wall_bracket_bottom_lip_" + SafeName(position), position + new Vector3(0f, -0.15f, 0.055f), new Vector3(0.44f, 0.045f, 0.052f), Materials["Brass"]);
            AddRivet(parent, "wall_bracket_bolt_a_" + SafeName(position), position + new Vector3(-0.15f, 0.12f, 0.025f), 0.025f, Materials["Brass"], Quaternion.identity);
            AddRivet(parent, "wall_bracket_bolt_b_" + SafeName(position), position + new Vector3(0.15f, -0.12f, 0.025f), 0.025f, Materials["Brass"], Quaternion.identity);
            if (vertical)
            {
                AddBox(parent, "vertical_pipe_clamp_strap_" + SafeName(position), position + new Vector3(0f, 0f, -0.09f), new Vector3(0.28f, 0.045f, 0.050f), Materials["Iron"]);
            }
        }

        private static void AddPipeX(GameObject parent, string name, Vector3 center, float length, float radius, Material material, bool withSeams)
        {
            AddMesh(parent, name, Meshes["CylinderX"], material, center, Quaternion.identity, new Vector3(length, radius, radius));
            if (withSeams)
            {
                AddGrimeBandX(parent, name + "_left_soot_seam", center + new Vector3(-length * 0.41f, 0f, 0f), radius * 1.04f, 0.030f);
                AddGrimeBandX(parent, name + "_right_soot_seam", center + new Vector3(length * 0.41f, 0f, 0f), radius * 1.04f, 0.030f);
            }
        }

        private static void AddPipeY(GameObject parent, string name, Vector3 center, float length, float radius, Material material, bool withSeams)
        {
            AddMesh(parent, name, Meshes["CylinderY"], material, center, Quaternion.identity, new Vector3(radius, length, radius));
            if (withSeams)
            {
                AddGrimeBandY(parent, name + "_lower_soot_seam", center + new Vector3(0f, -length * 0.40f, 0f), radius * 1.04f, 0.030f);
                AddGrimeBandY(parent, name + "_upper_soot_seam", center + new Vector3(0f, length * 0.40f, 0f), radius * 1.04f, 0.030f);
            }
        }

        private static void AddPipeZ(GameObject parent, string name, Vector3 center, float length, float radius, Material material, bool withSeams)
        {
            AddMesh(parent, name, Meshes["CylinderZ"], material, center, Quaternion.identity, new Vector3(radius, radius, length));
            if (withSeams)
            {
                AddMesh(parent, name + "_front_soot_ring", Meshes["CylinderZ"], Materials["Soot"], center + new Vector3(0f, 0f, -length * 0.38f), Quaternion.identity, new Vector3(radius * 1.09f, radius * 1.09f, 0.024f));
            }
        }

        private static void AddCollarX(GameObject parent, string name, float x, float radius, float width)
        {
            AddCollarXAt(parent, name, new Vector3(x, 0f, 0f), radius, width);
        }

        private static void AddCollarXAt(GameObject parent, string name, Vector3 center, float radius, float width)
        {
            AddMesh(parent, name + "_blackened_inner_band", Meshes["CylinderX"], Materials["Iron"], center, Quaternion.identity, new Vector3(width, radius * 0.93f, radius * 0.93f));
            AddMesh(parent, name + "_raised_brass_left_lip", Meshes["CylinderX"], Materials["Brass"], center + new Vector3(-width * 0.42f, 0f, 0f), Quaternion.identity, new Vector3(width * 0.22f, radius, radius));
            AddMesh(parent, name + "_raised_brass_right_lip", Meshes["CylinderX"], Materials["Brass"], center + new Vector3(width * 0.42f, 0f, 0f), Quaternion.identity, new Vector3(width * 0.22f, radius, radius));
            AddBoltRing(parent, name + "_visible_bolts", center + new Vector3(0f, 0f, -radius * 1.04f), radius * 0.73f, Axis.X, 6, Materials["Brass"], radius * 0.13f);
        }

        private static void AddCollarYAt(GameObject parent, string name, Vector3 center, float radius, float width)
        {
            AddMesh(parent, name + "_blackened_inner_band", Meshes["CylinderY"], Materials["Iron"], center, Quaternion.identity, new Vector3(radius * 0.93f, width, radius * 0.93f));
            AddMesh(parent, name + "_raised_brass_lower_lip", Meshes["CylinderY"], Materials["Brass"], center + new Vector3(0f, -width * 0.42f, 0f), Quaternion.identity, new Vector3(radius, width * 0.22f, radius));
            AddMesh(parent, name + "_raised_brass_upper_lip", Meshes["CylinderY"], Materials["Brass"], center + new Vector3(0f, width * 0.42f, 0f), Quaternion.identity, new Vector3(radius, width * 0.22f, radius));
            AddBoltRing(parent, name + "_visible_bolts", center + new Vector3(0f, 0f, -radius * 1.04f), radius * 0.72f, Axis.Y, 6, Materials["Brass"], radius * 0.12f);
        }

        private static void AddFlangeX(GameObject parent, string name, float x, float radius, float width)
        {
            AddFlangeX(parent, name, x, radius, width, Vector3.zero);
        }

        private static void AddFlangeX(GameObject parent, string name, float x, float radius, float width, Vector3 offset)
        {
            var center = offset + new Vector3(x, 0f, 0f);
            AddMesh(parent, name + "_thick_blackened_disk", Meshes["CylinderX"], Materials["Iron"], center, Quaternion.identity, new Vector3(width, radius, radius));
            AddMesh(parent, name + "_thin_brass_face", Meshes["CylinderX"], Materials["Brass"], center + new Vector3(-width * 0.52f, 0f, 0f), Quaternion.identity, new Vector3(width * 0.16f, radius * 1.035f, radius * 1.035f));
            AddBoltRing(parent, name + "_front_bolts", center + new Vector3(-width * 0.58f, 0f, -radius * 1.02f), radius * 0.73f, Axis.X, 8, Materials["Brass"], radius * 0.105f);
        }

        private static void AddFlangeZ(GameObject parent, string name, Vector3 center, float radius, float width)
        {
            AddMesh(parent, name + "_blackened_wall_ring", Meshes["CylinderZ"], Materials["Iron"], center, Quaternion.identity, new Vector3(radius, radius, width));
            AddMesh(parent, name + "_brass_wall_face_ring", Meshes["TorusZ"], Materials["Brass"], center + new Vector3(0f, 0f, -width * 0.56f), Quaternion.identity, Vector3.one * radius * 0.56f);
            AddBoltRing(parent, name + "_wall_face_bolts", center + new Vector3(0f, 0f, -width * 0.61f), radius * 0.76f, Axis.Z, 8, Materials["Brass"], radius * 0.095f);
        }

        private static void AddGrimeBandX(GameObject parent, string name, Vector3 center, float radius, float width)
        {
            AddMesh(parent, name, Meshes["CylinderX"], Materials["Soot"], center, Quaternion.identity, new Vector3(width, radius, radius));
        }

        private static void AddGrimeBandY(GameObject parent, string name, Vector3 center, float radius, float width)
        {
            AddMesh(parent, name, Meshes["CylinderY"], Materials["Soot"], center, Quaternion.identity, new Vector3(radius, width, radius));
        }

        private static void AddTarnishStreaksOnPipe(GameObject parent, int count, float length, float radius, float z)
        {
            AddTarnishStreaksOnPipe(parent, count, length, radius, z, 0f);
        }

        private static void AddTarnishStreaksOnPipe(GameObject parent, int count, float length, float radius, float z, float y)
        {
            for (var i = 0; i < count; i++)
            {
                var t = (i + 0.5f) / count;
                var x = -length * 0.48f + t * length;
                var yy = y + Mathf.Sin(i * 1.71f) * radius * 0.33f;
                var w = 0.07f + (i % 3) * 0.035f;
                var h = 0.010f + (i % 2) * 0.006f;
                AddBox(parent, "irregular_oil_tarnish_streak_" + i.ToString("00", CultureInfo.InvariantCulture), new Vector3(x, yy - radius * 0.18f, z - radius * 1.10f), new Vector3(w, h, 0.006f), Materials["Soot"]);
            }
        }

        private static void AddBoltRing(GameObject parent, string name, Vector3 center, float radius, Axis axis, int count, Material material, float boltRadius)
        {
            for (var i = 0; i < count; i++)
            {
                var angle = (i / (float)count) * Mathf.PI * 2f;
                var visibleBias = Mathf.Sin(angle);
                if (visibleBias > 0.72f)
                {
                    continue;
                }

                Vector3 position;
                Quaternion rotation = Quaternion.identity;
                if (axis == Axis.X)
                {
                    position = center + new Vector3(0f, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                }
                else if (axis == Axis.Y)
                {
                    position = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
                }
                else
                {
                    position = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
                }

                AddRivet(parent, name + "_" + i.ToString("00", CultureInfo.InvariantCulture), position, boltRadius, material, rotation);
            }
        }

        private static void AddRivet(GameObject parent, string name, Vector3 center, float radius, Material material, Quaternion rotation)
        {
            AddMesh(parent, name, Meshes["DomeZ"], material, center, rotation, Vector3.one * radius);
        }

        private static void AddBox(GameObject parent, string name, Vector3 center, Vector3 size, Material material)
        {
            AddMesh(parent, name, Meshes["Box"], material, center, Quaternion.identity, size);
        }

        private static void AddSteamPlane(GameObject parent, string name, Vector3 center, Vector3 scale, float zRotation)
        {
            AddMesh(parent, name, Meshes["Plane"], Materials["Steam"], center, Quaternion.Euler(0f, 0f, zRotation), scale);
        }

        private static void AddBrickField(GameObject parent, Vector3 origin, int columns, int rows, int seed, bool strongDepth)
        {
            var random = new System.Random(seed);
            for (var y = 0; y < rows; y++)
            {
                var rowOffset = (y % 2) * 0.18f;
                for (var x = 0; x < columns; x++)
                {
                    var width = 0.30f + (float)random.NextDouble() * 0.12f;
                    var height = 0.18f + (float)random.NextDouble() * 0.07f;
                    var depth = (strongDepth ? 0.055f : 0.030f) + (float)random.NextDouble() * 0.030f;
                    var px = origin.x + x * 0.36f + rowOffset + ((float)random.NextDouble() - 0.5f) * 0.035f;
                    var py = origin.y + y * 0.27f + ((float)random.NextDouble() - 0.5f) * 0.025f;
                    var pz = origin.z - depth * 0.5f;
                    AddBox(parent, "irregular_wet_masonry_block_" + x.ToString("00", CultureInfo.InvariantCulture) + "_" + y.ToString("00", CultureInfo.InvariantCulture), new Vector3(px, py, pz), new Vector3(width, height, depth), Materials["Masonry"]);
                    if ((x + y) % 5 == 0)
                    {
                        AddBox(parent, "soot_in_grout_shadow_" + x.ToString("00", CultureInfo.InvariantCulture) + "_" + y.ToString("00", CultureInfo.InvariantCulture), new Vector3(px, py - height * 0.55f, pz - depth * 0.55f), new Vector3(width * 0.82f, 0.018f, 0.006f), Materials["Soot"]);
                    }
                }
            }
        }

        private static GameObject AddMesh(GameObject parent, string name, Mesh mesh, Material material, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent.transform, false);
            child.transform.localPosition = localPosition;
            child.transform.localRotation = localRotation;
            child.transform.localScale = localScale;
            child.AddComponent<MeshFilter>().sharedMesh = mesh;
            child.AddComponent<MeshRenderer>().sharedMaterial = material;
            return child;
        }

        private static void SavePrefab(GameObject root, string fileName, string tag)
        {
            Directory.CreateDirectory(Physical("Runtime/Prefabs"));
            AssetDatabase.ImportAsset(PackageAssetRoot + "/Runtime/Prefabs", ImportAssetOptions.ForceSynchronousImport);
            var assetPath = PackageAssetRoot + "/Runtime/Prefabs/" + fileName + ".prefab";
            AssetDatabase.DeleteAsset(assetPath);
            PrefabUtility.SaveAsPrefabAsset(root, assetPath);
            Object.DestroyImmediate(root);
            PrefabRecords.Add(new AssetRecord("Runtime/Prefabs/" + fileName + ".prefab", fileName, tag));
        }

        private static void RenderProofs()
        {
            RenderBeauty();
            RenderModuleBreakdown();
            RenderMaterialCloseup();
        }

        private static void RenderBeauty()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            ConfigureRenderEnvironment();
            var root = BuildPipeBundleAssembly();
            root.transform.position = new Vector3(0f, -0.12f, 0.08f);
            AddRenderLights(new Vector3(-1.5f, 1.0f, -2.2f), new Vector3(1.25f, 1.25f, -1.8f));
            var camera = CreateCamera(new Vector3(-0.18f, 1.08f, -4.15f), new Vector3(0.02f, 1.00f, -0.28f), 37f);
            SaveRender(camera, "CRPB13_RENDER_01_reference_style_pipe_bundle_beauty.png", "beauty render matching pipe bundle crop");
        }

        private static void RenderModuleBreakdown()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            ConfigureRenderEnvironment();
            AddBox(new GameObject("Backdrop"), "dark_floor", new Vector3(0f, -0.17f, 0.10f), new Vector3(5.2f, 0.10f, 2.1f), Materials["Soot"]);
            var straight = BuildStraightPipeModule();
            straight.transform.position = new Vector3(-1.35f, 0.88f, 0f);
            var elbow = BuildElbowPipeModule();
            elbow.transform.position = new Vector3(0.55f, 1.02f, 0f);
            var flange = BuildFlangeCollarModule();
            flange.transform.position = new Vector3(1.80f, 0.95f, 0f);
            var bracket = BuildWallBracketClampModule();
            bracket.transform.position = new Vector3(-1.45f, 0.08f, 0f);
            var valve = BuildValveHandwheelModule();
            valve.transform.position = new Vector3(0.12f, 0.02f, 0f);
            var wall = BuildDarkMasonryBackingModule();
            wall.transform.position = new Vector3(1.42f, -0.28f, 0.18f);
            wall.transform.localScale = Vector3.one * 0.58f;
            AddRenderLights(new Vector3(-1.6f, 1.3f, -2.5f), new Vector3(1.55f, 1.0f, -2.0f));
            var camera = CreateCamera(new Vector3(-0.35f, 0.78f, -4.35f), new Vector3(0.10f, 0.58f, -0.02f), 34f);
            SaveRender(camera, "CRPB13_RENDER_02_modular_breakdown_contact_sheet.png", "modular breakdown contact sheet");
        }

        private static void RenderMaterialCloseup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            ConfigureRenderEnvironment();
            var wall = BuildDarkMasonryBackingModule();
            wall.transform.position = new Vector3(0.0f, -0.18f, 0.15f);
            AddPipeX(new GameObject("MaterialCloseupCopperPipe"), "grazing_pipe_with_seams", new Vector3(-0.18f, 0.62f, -0.30f), 2.85f, 0.15f, Materials["Copper"], true);
            var pipeParent = GameObject.Find("MaterialCloseupCopperPipe");
            AddCollarXAt(pipeParent, "closeup_bolted_collar_left", new Vector3(-0.72f, 0.62f, -0.30f), 0.21f, 0.16f);
            AddCollarXAt(pipeParent, "closeup_bolted_collar_right", new Vector3(0.82f, 0.62f, -0.30f), 0.21f, 0.16f);
            AddTarnishStreaksOnPipe(pipeParent, 18, 2.64f, 0.165f, -0.30f, 0.62f);
            AddRenderLights(new Vector3(-2.0f, 0.62f, -1.2f), new Vector3(1.8f, 1.6f, -2.2f));
            var camera = CreateCamera(new Vector3(-0.24f, 0.72f, -3.05f), new Vector3(0.05f, 0.72f, -0.22f), 24f);
            SaveRender(camera, "CRPB13_RENDER_03_material_grazing_light_closeup.png", "material and collar closeup under grazing light");
        }

        private static void ConfigureRenderEnvironment()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.020f, 0.017f, 0.014f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.030f, 0.026f, 0.022f);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.018f;
            if (Camera.main != null)
            {
                Object.DestroyImmediate(Camera.main.gameObject);
            }
        }

        private static void AddRenderLights(Vector3 warmLeft, Vector3 coolFill)
        {
            var key = new GameObject("CRPB13_Render_WarmGaslightKey");
            var keyLight = key.AddComponent<Light>();
            keyLight.type = LightType.Point;
            keyLight.color = new Color(1.0f, 0.56f, 0.24f);
            keyLight.range = 5.7f;
            keyLight.intensity = 5.9f;
            key.transform.position = warmLeft;

            var rim = new GameObject("CRPB13_Render_AmberRim");
            var rimLight = rim.AddComponent<Light>();
            rimLight.type = LightType.Spot;
            rimLight.color = new Color(1.0f, 0.70f, 0.35f);
            rimLight.range = 6.0f;
            rimLight.spotAngle = 54f;
            rimLight.intensity = 3.6f;
            rim.transform.position = coolFill;
            rim.transform.LookAt(new Vector3(0f, 0.85f, -0.25f));

            var low = new GameObject("CRPB13_Render_LowSpecKick");
            var lowLight = low.AddComponent<Light>();
            lowLight.type = LightType.Point;
            lowLight.color = new Color(0.65f, 0.42f, 0.22f);
            lowLight.range = 3.2f;
            lowLight.intensity = 1.2f;
            low.transform.position = new Vector3(0.8f, 0.25f, -1.45f);

            var directional = new GameObject("CRPB13_Render_SoftOverheadAmber");
            var directionalLight = directional.AddComponent<Light>();
            directionalLight.type = LightType.Directional;
            directionalLight.color = new Color(1.0f, 0.74f, 0.48f);
            directionalLight.intensity = 0.18f;
            directional.transform.rotation = Quaternion.Euler(42f, -35f, 0f);
        }

        private static Camera CreateCamera(Vector3 position, Vector3 target, float fov)
        {
            var cameraObject = new GameObject("CRPB13_RenderCamera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.transform.position = position;
            camera.transform.LookAt(target);
            camera.fieldOfView = fov;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 80f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.010f, 0.010f, 0.009f);
            camera.allowHDR = true;
            return camera;
        }

        private static void SaveRender(Camera camera, string fileName, string tag)
        {
            var renderTexture = new RenderTexture(RenderWidth, RenderHeight, 24, RenderTextureFormat.ARGB32)
            {
                antiAliasing = 4
            };
            var previous = RenderTexture.active;
            camera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            camera.Render();

            var texture = new Texture2D(RenderWidth, RenderHeight, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0, 0, RenderWidth, RenderHeight), 0, 0);
            texture.Apply();
            var bytes = texture.EncodeToPNG();
            var packagePreviewPath = Physical("Documentation~/Previews/" + fileName);
            var conceptPath = Path.Combine(ConceptRenderRoot(), fileName);
            File.WriteAllBytes(packagePreviewPath, bytes);
            File.WriteAllBytes(conceptPath, bytes);

            RenderTexture.active = previous;
            camera.targetTexture = null;
            Object.DestroyImmediate(texture);
            Object.DestroyImmediate(renderTexture);
            RenderRecords.Add(new AssetRecord("Documentation/ConceptRenders/" + RenderFolderName + "/" + fileName, fileName, tag));
        }

        private static void WriteMetadata()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"pack_id\": \"" + Prefix + "\",");
            sb.AppendLine("  \"display_name\": \"Brassworks Breach - Concept Replica Pipe Bundle Set 13\",");
            sb.AppendLine("  \"version\": \"" + Version + "\",");
            sb.AppendLine("  \"build_id\": \"v0.1.57-p013-crpb13\",");
            sb.AppendLine("  \"unity_version\": \"6000.4.6f1\",");
            sb.AppendLine("  \"sidecar_project\": \"Unity batchmode isolated validation project; temporary cache discarded after generation\",");
            sb.AppendLine("  \"owner_lane\": \"concept-replica-pipe-bundle\",");
            sb.AppendLine("  \"primary_intake_owner\": \"main Unity project quarantine import\",");
            sb.AppendLine("  \"canonical_root\": \"AssetPacks/BrassworksBreach.ConceptReplicaPipeBundleSet13\",");
            sb.AppendLine("  \"asset_counts\": { \"prefabs\": " + PrefabRecords.Count.ToString(CultureInfo.InvariantCulture) +
                          ", \"materials\": " + MaterialRecords.Count.ToString(CultureInfo.InvariantCulture) +
                          ", \"textures\": " + TextureRecords.Count.ToString(CultureInfo.InvariantCulture) +
                          ", \"meshes\": " + MeshRecords.Count.ToString(CultureInfo.InvariantCulture) +
                          ", \"renders\": " + RenderRecords.Count.ToString(CultureInfo.InvariantCulture) + " },");
            sb.AppendLine("  \"dependencies\": [\"Unity built-in renderer or URP-compatible Lit/Standard shader fallback\"],");
            sb.AppendLine("  \"required_primary_changes\": [\"Add package to main manifest only after quarantine review\", \"Place prefabs as visual-only dressing; author gameplay collision separately\"],");
            sb.AppendLine("  \"path_collisions_checked\": true,");
            sb.AppendLine("  \"guid_collisions_checked\": true,");
            sb.AppendLine("  \"import_smoke_status\": \"UNITY_BATCHMODE_PASS_CRPB13_GENERATE_PASS\",");
            sb.AppendLine("  \"known_risks\": [\"Not final AAA art\", \"Procedural masonry remains blocky\", \"Exact crop proportions need another focused pass\"],");
            sb.AppendLine("  \"rollback_path\": \"Remove the package manifest entry and delete placed CRPB13_* prefab instances from playable scenes\",");
            sb.AppendLine("  \"package\": \"" + PackageName + "\",");
            sb.AppendLine("  \"purpose\": \"Unity-only modular concept-art replica of the north-star Pipe Bundle crop.\",");
            sb.AppendLine("  \"qualityGate\": \"visual-only modular prefabs with collars, seams, flanges, brackets, valves, tarnish, dark masonry, steam hints, and warm specular lighting renders\",");
            AppendJsonArray(sb, "prefabs", PrefabRecords, true);
            AppendJsonArray(sb, "materials", MaterialRecords, true);
            AppendJsonArray(sb, "textures", TextureRecords, true);
            AppendJsonArray(sb, "meshes", MeshRecords, true);
            AppendJsonArray(sb, "renders", RenderRecords, false);
            sb.AppendLine("}");
            WriteText(Physical("Runtime/Metadata/CRPB13_ConceptReplicaPipeBundleSet13_Catalog.json"), sb.ToString());
            WriteText(Physical("Documentation~/Manifest/CRPB13_ConceptReplicaPipeBundleSet13_Catalog.json"), sb.ToString());
        }

        private static void AppendJsonArray(StringBuilder sb, string name, IReadOnlyList<AssetRecord> records, bool trailingComma)
        {
            sb.AppendLine("  \"" + name + "\": [");
            for (var i = 0; i < records.Count; i++)
            {
                var record = records[i];
                sb.Append("    { \"path\": \"" + Escape(record.Path) + "\", \"name\": \"" + Escape(record.Name) + "\", \"tag\": \"" + Escape(record.Tag) + "\" }");
                sb.AppendLine(i == records.Count - 1 ? "" : ",");
            }
            sb.AppendLine("  ]" + (trailingComma ? "," : ""));
        }

        private static void WritePackageDocs()
        {
            var readme = "# Concept Replica Pipe Bundle Set 13\n\n" +
                         "Unity-only sidecar package for an exact-reference-style pipe bundle proof. The package contains modular visual-only prefabs for straight pipes, elbows, flanges/collars, wall clamps, valves, dark masonry backing, and one assembled pipe-bundle crop.\n\n" +
                         "No gameplay scripts, colliders, rigidbodies, cameras, lights, audio sources, or animators are saved into runtime prefabs.\n";
            WriteText(Physical("Documentation~/README.md"), readme);
            WriteText(Physical("Documentation~/QA/CRPB13_QA.md"), BuildQaMarkdown(new ValidationResult(true, new List<string>())));
        }

        private static void WritePlanningDoc()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Concept Replica Pipe Bundle Set13 Implementation Plan");
            sb.AppendLine();
            sb.AppendLine("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture));
            sb.AppendLine();
            sb.AppendLine("## Target");
            sb.AppendLine("Create a Unity-only modular pipe bundle proof matching the north-star crop: aged copper/brass pipes, elbow bends, blackened collars, flanges, wall brackets, small valves, dark wet masonry, grime, steam hints, and warm gaslight reflections.");
            sb.AppendLine();
            sb.AppendLine("## Method");
            sb.AppendLine("- Build reusable mesh primitives in Unity: straight cylinders on all axes, quarter elbow tube, torus rings, rivet domes, planes, and box primitives.");
            sb.AppendLine("- Generate procedural texture maps before material creation: albedo, normal, metallic/smoothness, and occlusion for copper/brass, blackened iron, wet grime, dark masonry, amber highlights, and red valve enamel.");
            sb.AppendLine("- Save visual-only prefabs for the module kit, then compose an assembled pipe bundle against masonry to judge crop match.");
            sb.AppendLine("- Render three PNGs in Unity with warm point/spot lighting and low ambient fill: beauty, modular breakdown, and grazing-light material closeup.");
            sb.AppendLine();
            sb.AppendLine("## Integration Notes");
            sb.AppendLine("The modules are ready for a future quarantined playable visual import after a sidecar validator pass. They should remain visual-only until collision/nav blockers are authored in the main project.");
            WriteText(Path.Combine(PlanningRoot(), "ConceptReplicaPipeBundleSet13_ImplementationPlan.md"), sb.ToString());
        }

        private static void WriteQaDoc(ValidationResult validation)
        {
            WriteText(Path.Combine(QaRoot(), "ConceptReplicaPipeBundleSet13_QA.md"), BuildQaMarkdown(validation));
        }

        private static string BuildQaMarkdown(ValidationResult validation)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Concept Replica Pipe Bundle Set13 QA");
            sb.AppendLine();
            sb.AppendLine("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture));
            sb.AppendLine();
            sb.AppendLine("## Validation");
            sb.AppendLine("- Prefabs: " + PrefabRecords.Count.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine("- Materials: " + MaterialRecords.Count.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine("- Textures: " + TextureRecords.Count.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine("- Meshes: " + MeshRecords.Count.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine("- Renders: " + RenderRecords.Count.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine("- Visual-only prefab scan: " + (validation.Passed ? "PASS" : "FAIL"));
            foreach (var failure in validation.Failures)
            {
                sb.AppendLine("  - " + failure);
            }
            sb.AppendLine();
            sb.AppendLine("## Reference Self-Score");
            sb.AppendLine("| Category | Score | Notes |");
            sb.AppendLine("| --- | ---: | --- |");
            sb.AppendLine("| Silhouette/composition | 6/10 | Includes the left elbow/riser, low horizontal run, paired vertical right pipes, wall branch, steam wisps, and masonry backing. The composition is recognizable, but the elbow/branch proportions and crop do not yet match the reference precisely. |");
            sb.AppendLine("| Module reusability | 9/10 | Straight pipe, elbow, flange/collar, wall bracket, valve, and masonry modules are separate visual-only prefabs plus an assembled reference module. |");
            sb.AppendLine("| Pipe material aging | 6/10 | Copper/brass is darker and less toy-like than earlier attempts, with tarnish streaks, blackened bands, and procedural map variation. It still lacks authored wear masks, edge-specific oxidation, and convincing wet metal response. |");
            sb.AppendLine("| Flange/collar detail | 6/10 | Collars include raised lips, blackened inner bands, face bolts, soot seams, and varied sizes, but bolt placement and collar bevels still read procedural. |");
            sb.AppendLine("| Wall integration | 5/10 | Backing includes offset masonry blocks, mortar shadows, reflection patches, wall branch flange, brackets, and contact shadows. It still reads as rectangular block geometry rather than the wet, irregular brick surface in the reference. |");
            sb.AppendLine("| Lighting/render match | 5/10 | Warm low gaslight, black ambient, fog, and grazing highlights are present, but the render still lacks the reference image's bloom, wet specular breakup, dark contrast, and material depth. |");
            sb.AppendLine("| Final-art readiness | 4/10 | Useful modular proof and playable-import candidate for layout tests, but not final AAA art without authored material masks, better bevels, improved masonry sculpting, stronger render/post setup, and exact proportion matching. |");
            sb.AppendLine();
            sb.AppendLine("## Honest Verdict");
            sb.AppendLine("PASS as a focused modular pipe-bundle proof and as a future visual-only import candidate for layout tests. FAIL as an exact concept-art replica or final AAA asset; the next pass should replace the blocky masonry and procedural collars with more precise authored Unity geometry/material masks.");
            return sb.ToString();
        }

        private static ValidationResult ValidateVisualOnlyPrefabs()
        {
            var failures = new List<string>();
            foreach (var record in PrefabRecords)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageAssetRoot + "/" + record.Path);
                if (prefab == null)
                {
                    failures.Add("Missing prefab: " + record.Path);
                    continue;
                }

                foreach (var component in prefab.GetComponentsInChildren<Component>(true))
                {
                    if (component == null ||
                        component is Transform ||
                        component is MeshFilter ||
                        component is MeshRenderer)
                    {
                        continue;
                    }

                    failures.Add(record.Path + " contains forbidden component " + component.GetType().Name);
                }
            }

            return new ValidationResult(failures.Count == 0, failures);
        }

        private static Texture2D CreateAlbedoTexture(MaterialSpec spec)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var coarse = Mathf.PerlinNoise(fx * spec.CoarseScale + spec.Seed, fy * spec.CoarseScale - spec.Seed);
                    var fine = Mathf.PerlinNoise(fx * spec.FineScale - spec.Seed * 0.37f, fy * spec.FineScale + spec.Seed * 0.22f);
                    var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((fx * spec.ScratchScale + fy * spec.ScratchSkew + spec.Seed) * Mathf.PI)), spec.ScratchSharpness);
                    var stain = Mathf.SmoothStep(0.62f, 0.96f, Mathf.PerlinNoise(fx * 11.0f + spec.Seed * 0.11f, fy * 16.0f - spec.Seed * 0.19f));
                    var t = Mathf.Clamp01(coarse * 0.56f + fine * 0.30f + scratch * 0.18f);
                    var color = Color.Lerp(spec.Low, spec.High, t);
                    color = Color.Lerp(color, spec.Accent, stain * spec.AccentWeight);

                    if (spec.Key == "Masonry")
                    {
                        var mortarLine = Mathf.Max(GridLine(fx, 10.0f, 0.035f), GridLine(fy, 7.0f, 0.045f));
                        color = Color.Lerp(color, new Color(0.018f, 0.016f, 0.014f), mortarLine * 0.72f);
                    }

                    if (spec.Key == "Copper" || spec.Key == "Brass")
                    {
                        var verticalSoot = Mathf.SmoothStep(0.78f, 0.22f, fy) * Mathf.SmoothStep(0.60f, 0.95f, fine);
                        color = Color.Lerp(color, new Color(0.030f, 0.022f, 0.016f), verticalSoot * 0.24f);
                    }

                    var edgeDarken = Mathf.Clamp01((Mathf.Abs(fx - 0.5f) + Mathf.Abs(fy - 0.5f)) * 0.13f);
                    color *= 1f - edgeDarken;
                    color.a = 1f;
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D CreateNormalTexture(MaterialSpec spec)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var h = HeightAt(x, y, spec);
                    var hx = HeightAt(Mathf.Min(TextureSize - 1, x + 1), y, spec) - h;
                    var hy = HeightAt(x, Mathf.Min(TextureSize - 1, y + 1), spec) - h;
                    var normal = new Vector3(-hx * spec.NormalStrength, -hy * spec.NormalStrength, 1f).normalized;
                    texture.SetPixel(x, y, new Color(normal.x * 0.5f + 0.5f, normal.y * 0.5f + 0.5f, normal.z * 0.5f + 0.5f, 1f));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D CreateMaskTexture(MaterialSpec spec)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var noise = Mathf.PerlinNoise(fx * 19.0f + spec.Seed, fy * 21.0f - spec.Seed);
                    var roughPatch = Mathf.SmoothStep(0.58f, 0.95f, noise);
                    var smoothness = Mathf.Clamp01(spec.Smoothness - roughPatch * spec.RoughnessVariance);
                    texture.SetPixel(x, y, new Color(spec.Metallic, 0f, 0f, smoothness));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D CreateOcclusionTexture(MaterialSpec spec)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var pit = Mathf.PerlinNoise(fx * spec.FineScale + spec.Seed * 1.7f, fy * spec.FineScale - spec.Seed * 1.3f);
                    var value = Mathf.Clamp01(0.86f - Mathf.SmoothStep(0.55f, 0.98f, pit) * spec.OcclusionStrength);
                    texture.SetPixel(x, y, new Color(value, value, value, 1f));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static Texture2D CreateSteamAlphaTexture()
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var fx = x / (float)(TextureSize - 1);
                    var fy = y / (float)(TextureSize - 1);
                    var dist = Vector2.Distance(new Vector2(fx, fy), new Vector2(0.50f, 0.50f));
                    var cloud = Mathf.PerlinNoise(fx * 5.5f + 2.7f, fy * 8.5f - 1.4f) * 0.6f + Mathf.PerlinNoise(fx * 18.0f, fy * 16.0f) * 0.4f;
                    var alpha = Mathf.SmoothStep(0.62f, 0.18f, dist) * Mathf.SmoothStep(0.28f, 0.82f, cloud) * 0.44f;
                    texture.SetPixel(x, y, new Color(0.72f, 0.67f, 0.58f, alpha));
                }
            }

            texture.Apply(false, false);
            return texture;
        }

        private static float HeightAt(int x, int y, MaterialSpec spec)
        {
            var fx = x / (float)(TextureSize - 1);
            var fy = y / (float)(TextureSize - 1);
            var coarse = Mathf.PerlinNoise(fx * spec.CoarseScale + spec.Seed, fy * spec.CoarseScale - spec.Seed);
            var fine = Mathf.PerlinNoise(fx * spec.FineScale - spec.Seed * 0.37f, fy * spec.FineScale + spec.Seed * 0.22f);
            var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((fx * spec.ScratchScale + fy * spec.ScratchSkew + spec.Seed) * Mathf.PI)), spec.ScratchSharpness);
            var brickLine = spec.Key == "Masonry" ? Mathf.Max(GridLine(fx, 10.0f, 0.035f), GridLine(fy, 7.0f, 0.045f)) * -0.45f : 0f;
            return coarse * 0.55f + fine * 0.33f + scratch * 0.24f + brickLine;
        }

        private static float GridLine(float value, float cells, float width)
        {
            var cell = value * cells;
            var frac = Mathf.Abs(cell - Mathf.Floor(cell) - 0.5f);
            return Mathf.SmoothStep(width, 0f, frac);
        }

        private static IReadOnlyList<MaterialSpec> MaterialSpecs()
        {
            return new[]
            {
                new MaterialSpec("Copper", "AgedCopperBrass", new Color(0.13f, 0.060f, 0.030f), new Color(0.49f, 0.235f, 0.090f), new Color(0.020f, 0.085f, 0.070f), new Color(0.37f, 0.170f, 0.070f), 0.34f, 0.54f, 0.18f, 0.54f, 8.5f, 48f, 70f, 12f, 0.22f, 12.7f, "aged copper/brass pipe with tarnish and oily highlights"),
                new MaterialSpec("Brass", "DulledRimBrass", new Color(0.19f, 0.115f, 0.045f), new Color(0.62f, 0.390f, 0.145f), new Color(0.025f, 0.085f, 0.065f), new Color(0.48f, 0.295f, 0.105f), 0.42f, 0.52f, 0.15f, 0.42f, 7.8f, 42f, 60f, 7f, 0.19f, 27.2f, "worn brass raised lips and rivets"),
                new MaterialSpec("Iron", "BlackenedIronBands", new Color(0.014f, 0.013f, 0.012f), new Color(0.20f, 0.170f, 0.125f), new Color(0.38f, 0.19f, 0.06f), new Color(0.055f, 0.048f, 0.038f), 0.35f, 0.40f, 0.24f, 0.62f, 10.0f, 58f, 40f, 19f, 0.22f, 43.9f, "blackened iron clamp and collar bands"),
                new MaterialSpec("Soot", "WetGrimeSoot", new Color(0.006f, 0.005f, 0.004f), new Color(0.15f, 0.105f, 0.050f), new Color(0.46f, 0.24f, 0.075f), new Color(0.035f, 0.025f, 0.018f), 0.06f, 0.68f, 0.30f, 0.72f, 13.0f, 72f, 36f, 14f, 0.26f, 61.5f, "wet grime, soot, contact shadow, and oily streak material"),
                new MaterialSpec("Masonry", "DarkWetMasonry", new Color(0.020f, 0.018f, 0.016f), new Color(0.145f, 0.122f, 0.092f), new Color(0.30f, 0.18f, 0.075f), new Color(0.060f, 0.053f, 0.044f), 0.00f, 0.50f, 0.34f, 0.78f, 6.2f, 36f, 28f, 5f, 0.18f, 82.4f, "dark wet masonry backing with mortar and warm reflected grime"),
                new MaterialSpec("Amber", "WarmSpecularAmber", new Color(0.23f, 0.090f, 0.025f), new Color(0.64f, 0.36f, 0.11f), new Color(0.75f, 0.55f, 0.25f), new Color(0.46f, 0.22f, 0.065f), 0.02f, 0.58f, 0.12f, 0.18f, 5.0f, 24f, 36f, 3f, 0.08f, 91.1f, "thin warm reflected gaslight cards"),
                new MaterialSpec("Red", "ValveRedEnamel", new Color(0.19f, 0.020f, 0.012f), new Color(0.66f, 0.075f, 0.030f), new Color(0.95f, 0.38f, 0.10f), new Color(0.42f, 0.035f, 0.018f), 0.20f, 0.48f, 0.15f, 0.35f, 7.0f, 40f, 44f, 5f, 0.11f, 104.0f, "small oxidized red valve enamel accent")
            };
        }

        private static Texture2D LoadTexture(string name)
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(PackageAssetRoot + "/Runtime/Textures/" + Prefix + "_TEX_" + name + ".png");
            if (texture == null)
            {
                throw new InvalidOperationException("Missing generated texture " + name);
            }

            return texture;
        }

        private static void SaveTexture(string name, Texture2D texture, string folder, bool normal, bool linear, string tag)
        {
            var fileName = Prefix + "_TEX_" + name + ".png";
            var relative = folder + "/" + fileName;
            var physicalPath = Physical(relative);
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath) ?? ".");
            File.WriteAllBytes(physicalPath, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
            TextureRecords.Add(new AssetRecord(relative, name, tag));
        }

        private static Material SaveAsset(Material material, string relativePath, List<AssetRecord> records, string tag)
        {
            var assetPath = PackageAssetRoot + "/" + relativePath;
            Directory.CreateDirectory(Path.GetDirectoryName(Physical(relativePath)) ?? ".");
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(material, assetPath);
            records.Add(new AssetRecord(relativePath, material.name, tag));
            return AssetDatabase.LoadAssetAtPath<Material>(assetPath);
        }

        private static Mesh SaveAsset(Mesh mesh, string relativePath, List<AssetRecord> records, string tag)
        {
            mesh.name = Path.GetFileNameWithoutExtension(relativePath);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            var assetPath = PackageAssetRoot + "/" + relativePath;
            Directory.CreateDirectory(Path.GetDirectoryName(Physical(relativePath)) ?? ".");
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(mesh, assetPath);
            records.Add(new AssetRecord(relativePath, mesh.name, tag));
            return AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
        }

        private static Mesh CreateCylinderMesh(Axis axis, int segments)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var i = 0; i <= segments; i++)
            {
                var a = i / (float)segments * Mathf.PI * 2f;
                var radial = AxisRadial(axis, a);
                vertices.Add(AxisPoint(axis, -0.5f) + radial);
                vertices.Add(AxisPoint(axis, 0.5f) + radial);
                normals.Add(radial);
                normals.Add(radial);
                uvs.Add(new Vector2(i / (float)segments, 0f));
                uvs.Add(new Vector2(i / (float)segments, 1f));
            }

            for (var i = 0; i < segments; i++)
            {
                var start = i * 2;
                triangles.Add(start);
                triangles.Add(start + 1);
                triangles.Add(start + 3);
                triangles.Add(start);
                triangles.Add(start + 3);
                triangles.Add(start + 2);
            }

            AddCap(axis, -0.5f, -1f, segments, vertices, normals, uvs, triangles);
            AddCap(axis, 0.5f, 1f, segments, vertices, normals, uvs, triangles);
            return BuildMesh(vertices, normals, uvs, triangles);
        }

        private static void AddCap(Axis axis, float position, float normalSign, int segments, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> triangles)
        {
            var centerIndex = vertices.Count;
            vertices.Add(AxisPoint(axis, position));
            normals.Add(AxisPoint(axis, normalSign));
            uvs.Add(new Vector2(0.5f, 0.5f));
            for (var i = 0; i <= segments; i++)
            {
                var a = i / (float)segments * Mathf.PI * 2f;
                var radial = AxisRadial(axis, a);
                vertices.Add(AxisPoint(axis, position) + radial);
                normals.Add(AxisPoint(axis, normalSign));
                uvs.Add(new Vector2(radial.x * 0.5f + 0.5f, radial.y * 0.5f + 0.5f));
            }

            for (var i = 0; i < segments; i++)
            {
                if (normalSign < 0f)
                {
                    triangles.Add(centerIndex);
                    triangles.Add(centerIndex + i + 2);
                    triangles.Add(centerIndex + i + 1);
                }
                else
                {
                    triangles.Add(centerIndex);
                    triangles.Add(centerIndex + i + 1);
                    triangles.Add(centerIndex + i + 2);
                }
            }
        }

        private static Mesh CreateTorusMesh(Axis axis, int majorSegments, int minorSegments, float majorRadius, float tubeRadius)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var i = 0; i <= majorSegments; i++)
            {
                var u = i / (float)majorSegments * Mathf.PI * 2f;
                var radial = AxisRadial(axis, u);
                var center = radial * majorRadius;
                var axisVec = AxisPoint(axis, 1f);
                for (var j = 0; j <= minorSegments; j++)
                {
                    var v = j / (float)minorSegments * Mathf.PI * 2f;
                    var normal = (radial * Mathf.Cos(v) + axisVec * Mathf.Sin(v)).normalized;
                    vertices.Add(center + normal * tubeRadius);
                    normals.Add(normal);
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
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(a + 1);
                    triangles.Add(a + 1);
                    triangles.Add(b);
                    triangles.Add(b + 1);
                }
            }

            return BuildMesh(vertices, normals, uvs, triangles);
        }

        private static Mesh CreateQuarterElbowMesh(int arcSegments, int radialSegments, float majorRadius, float tubeRadius)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var i = 0; i <= arcSegments; i++)
            {
                var t = i / (float)arcSegments;
                var a = t * Mathf.PI * 0.5f;
                var center = new Vector3(Mathf.Cos(a) * majorRadius, Mathf.Sin(a) * majorRadius, 0f);
                var outward = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f).normalized;
                var binormal = Vector3.forward;
                for (var j = 0; j <= radialSegments; j++)
                {
                    var v = j / (float)radialSegments * Mathf.PI * 2f;
                    var normal = (outward * Mathf.Cos(v) + binormal * Mathf.Sin(v)).normalized;
                    vertices.Add(center + normal * tubeRadius);
                    normals.Add(normal);
                    uvs.Add(new Vector2(t, j / (float)radialSegments));
                }
            }

            var row = radialSegments + 1;
            for (var i = 0; i < arcSegments; i++)
            {
                for (var j = 0; j < radialSegments; j++)
                {
                    var a = i * row + j;
                    var b = (i + 1) * row + j;
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(a + 1);
                    triangles.Add(a + 1);
                    triangles.Add(b);
                    triangles.Add(b + 1);
                }
            }

            return BuildMesh(vertices, normals, uvs, triangles);
        }

        private static Mesh CreateDomeMesh(int segments, int rings)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var y = 0; y <= rings; y++)
            {
                var v = y / (float)rings;
                var theta = v * Mathf.PI * 0.5f;
                var z = Mathf.Cos(theta);
                var r = Mathf.Sin(theta);
                for (var x = 0; x <= segments; x++)
                {
                    var u = x / (float)segments;
                    var a = u * Mathf.PI * 2f;
                    var normal = new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, z).normalized;
                    vertices.Add(normal);
                    normals.Add(normal);
                    uvs.Add(new Vector2(u, v));
                }
            }

            var row = segments + 1;
            for (var y = 0; y < rings; y++)
            {
                for (var x = 0; x < segments; x++)
                {
                    var a = y * row + x;
                    var b = (y + 1) * row + x;
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(a + 1);
                    triangles.Add(a + 1);
                    triangles.Add(b);
                    triangles.Add(b + 1);
                }
            }

            return BuildMesh(vertices, normals, uvs, triangles);
        }

        private static Mesh CreateBoxMesh()
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            AddFace(vertices, normals, uvs, triangles, Vector3.forward, new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f));
            AddFace(vertices, normals, uvs, triangles, Vector3.back, new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f));
            AddFace(vertices, normals, uvs, triangles, Vector3.right, new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f));
            AddFace(vertices, normals, uvs, triangles, Vector3.left, new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, -0.5f));
            AddFace(vertices, normals, uvs, triangles, Vector3.up, new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f));
            AddFace(vertices, normals, uvs, triangles, Vector3.down, new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f));
            return BuildMesh(vertices, normals, uvs, triangles);
        }

        private static Mesh CreatePlaneMesh()
        {
            var vertices = new List<Vector3> { new Vector3(-0.5f, -0.5f, 0f), new Vector3(0.5f, -0.5f, 0f), new Vector3(0.5f, 0.5f, 0f), new Vector3(-0.5f, 0.5f, 0f) };
            var normals = Enumerable.Repeat(Vector3.back, 4).ToList();
            var uvs = new List<Vector2> { Vector2.zero, Vector2.right, Vector2.one, Vector2.up };
            var triangles = new List<int> { 0, 2, 1, 0, 3, 2 };
            return BuildMesh(vertices, normals, uvs, triangles);
        }

        private static void AddFace(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> triangles, Vector3 normal, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var index = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);
            normals.Add(normal);
            normals.Add(normal);
            normals.Add(normal);
            normals.Add(normal);
            uvs.Add(Vector2.zero);
            uvs.Add(Vector2.right);
            uvs.Add(Vector2.one);
            uvs.Add(Vector2.up);
            triangles.Add(index);
            triangles.Add(index + 1);
            triangles.Add(index + 2);
            triangles.Add(index);
            triangles.Add(index + 2);
            triangles.Add(index + 3);
        }

        private static Mesh BuildMesh(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> triangles)
        {
            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Vector3 AxisPoint(Axis axis, float value)
        {
            if (axis == Axis.X)
            {
                return new Vector3(value, 0f, 0f);
            }

            if (axis == Axis.Y)
            {
                return new Vector3(0f, value, 0f);
            }

            return new Vector3(0f, 0f, value);
        }

        private static Vector3 AxisRadial(Axis axis, float angle)
        {
            if (axis == Axis.X)
            {
                return new Vector3(0f, Mathf.Cos(angle), Mathf.Sin(angle));
            }

            if (axis == Axis.Y)
            {
                return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            }

            return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
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

        private static void SetMaterialTexture(Material material, string primary, string fallback, Texture texture)
        {
            if (material.HasProperty(primary))
            {
                material.SetTexture(primary, texture);
            }

            if (fallback != primary && material.HasProperty(fallback))
            {
                material.SetTexture(fallback, texture);
            }
        }

        private static void ConfigureTransparent(Material material)
        {
            SetMaterialFloat(material, "_Mode", 3f);
            SetMaterialFloat(material, "_Surface", 1f);
            SetMaterialFloat(material, "_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            SetMaterialFloat(material, "_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            SetMaterialFloat(material, "_ZWrite", 0f);
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        }

        private static string Physical(string relative)
        {
            return NormalizePath(Path.Combine(_packagePhysicalRoot, relative.Replace("/", Path.DirectorySeparatorChar.ToString())));
        }

        private static string ConceptRenderRoot()
        {
            return NormalizePath(Path.Combine(_repoRoot, "Documentation", "ConceptRenders", RenderFolderName));
        }

        private static string PlanningRoot()
        {
            return NormalizePath(Path.Combine(_repoRoot, "Documentation", "Planning"));
        }

        private static string QaRoot()
        {
            return NormalizePath(Path.Combine(_repoRoot, "Documentation", "QA"));
        }

        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }

        private static void DeleteDirectoryIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        private static void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static void WriteText(string path, string text)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            File.WriteAllText(path, text, new UTF8Encoding(false));
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string SafeName(Vector3 value)
        {
            return ((int)(value.x * 100f)).ToString(CultureInfo.InvariantCulture) + "_" +
                   ((int)(value.y * 100f)).ToString(CultureInfo.InvariantCulture) + "_" +
                   ((int)(value.z * 100f)).ToString(CultureInfo.InvariantCulture);
        }

        private enum Axis
        {
            X,
            Y,
            Z
        }

        private readonly struct MaterialSpec
        {
            public MaterialSpec(string key, string name, Color low, Color high, Color accent, Color baseColor, float metallic, float smoothness, float roughnessVariance, float normalStrength, float coarseScale, float fineScale, float scratchScale, float scratchSkew, float accentWeight, float seed, string tag)
            {
                Key = key;
                Name = name;
                Low = low;
                High = high;
                Accent = accent;
                BaseColor = baseColor;
                Metallic = metallic;
                Smoothness = smoothness;
                RoughnessVariance = roughnessVariance;
                NormalStrength = normalStrength;
                CoarseScale = coarseScale;
                FineScale = fineScale;
                ScratchScale = scratchScale;
                ScratchSkew = scratchSkew;
                ScratchSharpness = Mathf.Max(3.0f, scratchSkew);
                AccentWeight = accentWeight;
                OcclusionStrength = Mathf.Clamp01(roughnessVariance + 0.24f);
                Seed = seed;
                Tag = tag;
            }

            public readonly string Key;
            public readonly string Name;
            public readonly Color Low;
            public readonly Color High;
            public readonly Color Accent;
            public readonly Color BaseColor;
            public readonly float Metallic;
            public readonly float Smoothness;
            public readonly float RoughnessVariance;
            public readonly float NormalStrength;
            public readonly float CoarseScale;
            public readonly float FineScale;
            public readonly float ScratchScale;
            public readonly float ScratchSkew;
            public readonly float ScratchSharpness;
            public readonly float AccentWeight;
            public readonly float OcclusionStrength;
            public readonly float Seed;
            public readonly string Tag;
        }

        private readonly struct AssetRecord
        {
            public AssetRecord(string path, string name, string tag)
            {
                Path = path;
                Name = name;
                Tag = tag;
            }

            public readonly string Path;
            public readonly string Name;
            public readonly string Tag;
        }

        private readonly struct ValidationResult
        {
            public ValidationResult(bool passed, List<string> failures)
            {
                Passed = passed;
                Failures = failures;
            }

            public readonly bool Passed;
            public readonly List<string> Failures;
        }
    }
}
#endif
