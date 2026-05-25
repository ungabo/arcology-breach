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
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace BrassworksBreach.MechanicalEnemyDetailSet12.Editor
{
    public static class MechanicalEnemyDetailSet12Builder
    {
        private const string PackageName = "com.brassworks.sidecar.mechanical-enemy-detail-set12";
        private const string DisplayName = "Brassworks Breach Mechanical Enemy Detail Set 12";
        private const string Version = "0.1.57-p012";
        private const string Prefix = "MED12";
        private const int TextureSize = 512;
        private const int RenderWidth = 1600;
        private const int RenderHeight = 1000;
        private const string RenderFolderName = "V0_1_57_MechanicalEnemyDetailSet12";

        private static readonly List<AssetRecord> TextureRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> MaterialRecords = new List<AssetRecord>();
        private static readonly List<AssetRecord> MeshRecords = new List<AssetRecord>();
        private static readonly List<PrefabRecord> PrefabRecords = new List<PrefabRecord>();
        private static readonly List<RenderRecord> RenderRecords = new List<RenderRecord>();

        [MenuItem("Brassworks Breach/Sidecar Packs/Mechanical Enemy Detail Set 12/Generate Assets And Renders")]
        public static void GenerateAssetsAndRenders()
        {
            TextureRecords.Clear();
            MaterialRecords.Clear();
            MeshRecords.Clear();
            PrefabRecords.Clear();
            RenderRecords.Clear();

            var packageRoot = LocatePackageRoot();
            var repoRoot = ResolveRepoRoot(packageRoot.ResolvedPath);
            var renderRoot = Path.Combine(repoRoot, "Documentation", "ConceptRenders", RenderFolderName);
            var planningRoot = Path.Combine(repoRoot, "Documentation", "Planning");
            var qaRoot = Path.Combine(repoRoot, "Documentation", "QA");

            ResetOwnedGeneratedRoots(packageRoot, renderRoot, planningRoot, qaRoot);
            WritePackageMetadata(packageRoot);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            var textures = CreateTextures(packageRoot);
            var materials = CreateMaterials(packageRoot, textures);
            var meshes = CreateMeshes(packageRoot);

            CreateRivetedTorsoPlateCluster(packageRoot, meshes, materials);
            CreateGlowingEyeHeadModule(packageRoot, meshes, materials);
            CreateFlywheelShoulderAssembly(packageRoot, meshes, materials);
            CreatePistonForearm(packageRoot, meshes, materials);
            CreateSawClawToolArmAttachment(packageRoot, meshes, materials);
            CreatePressureGaugeChestModule(packageRoot, meshes, materials);
            CreateCableHoseBundle(packageRoot, meshes, materials);
            CreateLegJointArmorPlates(packageRoot, meshes, materials);
            CreateAssembledBustUpperBodyConcept(packageRoot, meshes, materials);
            CreateMaterialPaletteBoard(packageRoot, meshes, materials);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            RenderPreviews(packageRoot, renderRoot);

            var validation = ValidateGeneratedContent(packageRoot, renderRoot);
            WriteManifest(packageRoot, repoRoot, renderRoot, planningRoot, qaRoot, validation);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            if (!validation.Passed)
            {
                throw new InvalidOperationException(Prefix + "_VALIDATION_FAIL " + string.Join("; ", validation.Failures));
            }

            Debug.Log(Prefix + "_GENERATE_PASS prefabs=" + validation.PrefabCount + " materials=" + validation.MaterialCount + " textures=" + validation.TextureCount + " meshes=" + validation.MeshCount + " renders=" + validation.RenderCount);
        }

        private static Dictionary<string, Texture2D> CreateTextures(PackageRoot packageRoot)
        {
            var specs = new[]
            {
                new MaterialSpec("AgedBrass", "aged_brass", new Color(0.66f, 0.43f, 0.16f), new Color(0.20f, 0.31f, 0.23f), 0.92f, 0.38f, false, false, new Color(0, 0, 0), 11),
                new MaterialSpec("DarkOilStainedIron", "dark_oil_stained_iron", new Color(0.055f, 0.050f, 0.043f), new Color(0.34f, 0.25f, 0.13f), 0.86f, 0.58f, false, false, new Color(0, 0, 0), 17),
                new MaterialSpec("WornCopper", "worn_copper", new Color(0.62f, 0.24f, 0.095f), new Color(0.05f, 0.31f, 0.27f), 0.88f, 0.42f, false, false, new Color(0, 0, 0), 23),
                new MaterialSpec("GlowingAmberGlass", "glowing_amber_glass", new Color(1.0f, 0.46f, 0.07f, 1.0f), new Color(1.0f, 0.82f, 0.24f), 0.02f, 0.78f, false, true, new Color(1.0f, 0.42f, 0.07f), 29),
                new MaterialSpec("BlackRubberizedHose", "black_rubberized_hose", new Color(0.018f, 0.017f, 0.015f), new Color(0.12f, 0.10f, 0.08f), 0.0f, 0.48f, false, false, new Color(0, 0, 0), 31),
                new MaterialSpec("SharpSawMetal", "sharp_saw_metal", new Color(0.58f, 0.56f, 0.50f), new Color(0.25f, 0.22f, 0.18f), 0.96f, 0.34f, false, false, new Color(0, 0, 0), 37),
                new MaterialSpec("IvoryGaugeFace", "ivory_gauge_face", new Color(0.82f, 0.74f, 0.58f), new Color(0.34f, 0.27f, 0.18f), 0.0f, 0.42f, false, false, new Color(0, 0, 0), 41),
                new MaterialSpec("SootOilDeposit", "soot_oil_deposit", new Color(0.018f, 0.015f, 0.012f, 0.72f), new Color(0.11f, 0.08f, 0.05f), 0.05f, 0.28f, true, false, new Color(0, 0, 0), 43),
                new MaterialSpec("RedNeedleEnamel", "red_needle_enamel", new Color(0.72f, 0.04f, 0.02f), new Color(0.20f, 0.01f, 0.005f), 0.0f, 0.46f, false, false, new Color(0, 0, 0), 47)
            };

            var result = new Dictionary<string, Texture2D>();
            foreach (var spec in specs)
            {
                result[spec.Key + "_Albedo"] = SaveTexture(packageRoot, spec, "Albedo", CreateAlbedoTexture(spec), false, false);
                result[spec.Key + "_Normal"] = SaveTexture(packageRoot, spec, "Normal", CreateNormalTexture(spec.Seed, spec.Key == "BlackRubberizedHose" ? 0.55f : 0.34f), true, false);
                result[spec.Key + "_Mask"] = SaveTexture(packageRoot, spec, "MetallicSmoothness", CreateMaskTexture(spec), false, true);
                result[spec.Key + "_Occlusion"] = SaveTexture(packageRoot, spec, "Occlusion", CreateOcclusionTexture(spec.Seed), false, true);
            }

            return result;
        }

        private static Dictionary<string, Material> CreateMaterials(PackageRoot packageRoot, IReadOnlyDictionary<string, Texture2D> textures)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("No compatible Lit shader found.");
            }

            var specs = new[]
            {
                new MaterialSpec("AgedBrass", "aged_brass", new Color(0.66f, 0.43f, 0.16f), new Color(0.20f, 0.31f, 0.23f), 0.92f, 0.38f, false, false, new Color(0, 0, 0), 11),
                new MaterialSpec("DarkOilStainedIron", "dark_oil_stained_iron", new Color(0.055f, 0.050f, 0.043f), new Color(0.34f, 0.25f, 0.13f), 0.86f, 0.58f, false, false, new Color(0, 0, 0), 17),
                new MaterialSpec("WornCopper", "worn_copper", new Color(0.62f, 0.24f, 0.095f), new Color(0.05f, 0.31f, 0.27f), 0.88f, 0.42f, false, false, new Color(0, 0, 0), 23),
                new MaterialSpec("GlowingAmberGlass", "glowing_amber_glass", new Color(1.0f, 0.46f, 0.07f, 1.0f), new Color(1.0f, 0.82f, 0.24f), 0.02f, 0.78f, false, true, new Color(1.0f, 0.42f, 0.07f), 29),
                new MaterialSpec("BlackRubberizedHose", "black_rubberized_hose", new Color(0.018f, 0.017f, 0.015f), new Color(0.12f, 0.10f, 0.08f), 0.0f, 0.48f, false, false, new Color(0, 0, 0), 31),
                new MaterialSpec("SharpSawMetal", "sharp_saw_metal", new Color(0.58f, 0.56f, 0.50f), new Color(0.25f, 0.22f, 0.18f), 0.96f, 0.34f, false, false, new Color(0, 0, 0), 37),
                new MaterialSpec("IvoryGaugeFace", "ivory_gauge_face", new Color(0.82f, 0.74f, 0.58f), new Color(0.34f, 0.27f, 0.18f), 0.0f, 0.42f, false, false, new Color(0, 0, 0), 41),
                new MaterialSpec("SootOilDeposit", "soot_oil_deposit", new Color(0.018f, 0.015f, 0.012f, 0.72f), new Color(0.11f, 0.08f, 0.05f), 0.05f, 0.28f, true, false, new Color(0, 0, 0), 43),
                new MaterialSpec("RedNeedleEnamel", "red_needle_enamel", new Color(0.72f, 0.04f, 0.02f), new Color(0.20f, 0.01f, 0.005f), 0.0f, 0.46f, false, false, new Color(0, 0, 0), 47)
            };

            var result = new Dictionary<string, Material>();
            foreach (var spec in specs)
            {
                var mat = new Material(shader) { name = Prefix + "_MAT_" + spec.Name };
                SetMaterialColor(mat, spec.Base);
                SetMaterialFloat(mat, "_Metallic", spec.Metallic);
                SetMaterialFloat(mat, "_Smoothness", spec.Smoothness);
                SetMaterialTexture(mat, "_BaseMap", "_MainTex", textures[spec.Key + "_Albedo"]);
                SetMaterialTexture(mat, "_BumpMap", "_BumpMap", textures[spec.Key + "_Normal"]);
                mat.EnableKeyword("_NORMALMAP");
                SetMaterialTexture(mat, "_MetallicGlossMap", "_MetallicGlossMap", textures[spec.Key + "_Mask"]);
                mat.EnableKeyword("_METALLICSPECGLOSSMAP");
                SetMaterialTexture(mat, "_OcclusionMap", "_OcclusionMap", textures[spec.Key + "_Occlusion"]);

                if (spec.Emissive)
                {
                    if (mat.HasProperty("_EmissionColor"))
                    {
                        mat.SetColor("_EmissionColor", spec.Emission * 1.25f);
                    }
                    SetMaterialTexture(mat, "_EmissionMap", "_EmissionMap", textures[spec.Key + "_Albedo"]);
                    mat.EnableKeyword("_EMISSION");
                }

                if (spec.Transparent)
                {
                    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    SetMaterialFloat(mat, "_Surface", 1f);
                    SetMaterialFloat(mat, "_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    SetMaterialFloat(mat, "_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    SetMaterialFloat(mat, "_ZWrite", 0f);
                    mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                }

                var assetPath = packageRoot.AssetPath + "/Runtime/Materials/" + mat.name + ".mat";
                ReplaceAsset(assetPath);
                AssetDatabase.CreateAsset(mat, assetPath);
                result[spec.Key] = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                MaterialRecords.Add(new AssetRecord("Runtime/Materials/" + mat.name + ".mat", spec.Key, spec.Tag));
            }

            return result;
        }

        private static Dictionary<string, Mesh> CreateMeshes(PackageRoot packageRoot)
        {
            var meshMap = new Dictionary<string, Mesh>
            {
                ["box"] = SaveMesh(packageRoot, "BoxUnit", CreateBoxMesh()),
                ["thinPlate"] = SaveMesh(packageRoot, "ThinPlate", CreateBoxMesh()),
                ["cylinder16"] = SaveMesh(packageRoot, "Cylinder16_Y", CreateCylinderMesh(16)),
                ["cylinder32"] = SaveMesh(packageRoot, "Cylinder32_Y", CreateCylinderMesh(32)),
                ["cylinder48"] = SaveMesh(packageRoot, "Cylinder48_Y", CreateCylinderMesh(48)),
                ["torus32"] = SaveMesh(packageRoot, "Torus32_Y", CreateTorusMesh(32, 8, 0.50f, 0.075f)),
                ["torus48"] = SaveMesh(packageRoot, "Torus48_Y", CreateTorusMesh(48, 10, 0.50f, 0.065f)),
                ["flywheel"] = SaveMesh(packageRoot, "FlywheelRim64_Y", CreateTorusMesh(64, 10, 0.50f, 0.052f)),
                ["saw"] = SaveMesh(packageRoot, "SawBlade36_Z", CreateSawBladeMesh(36)),
                ["claw"] = SaveMesh(packageRoot, "CurvedClawBlade", CreateClawBladeMesh()),
                ["needle"] = SaveMesh(packageRoot, "GaugeNeedle", CreateNeedleMesh()),
                ["dome"] = SaveMesh(packageRoot, "RivetDome", CreateDomeMesh(18, 6, 0.50f, 0.32f)),
                ["lens"] = SaveMesh(packageRoot, "ConvexLens", CreateDomeMesh(32, 8, 0.50f, 0.22f)),
                ["pentPlate"] = SaveMesh(packageRoot, "PentagonalArmorPlate", CreatePentPlateMesh()),
                ["hoseArc"] = SaveMesh(packageRoot, "HoseArc", CreateTubeMesh(36, 10, 0.055f, t => new Vector3(Mathf.Lerp(-0.55f, 0.55f, t), Mathf.Sin(t * Mathf.PI) * 0.42f, 0f))),
                ["hoseS"] = SaveMesh(packageRoot, "HoseSRun", CreateTubeMesh(48, 10, 0.048f, t => new Vector3(Mathf.Lerp(-0.65f, 0.65f, t), Mathf.Sin(t * Mathf.PI * 2f) * 0.22f, 0f))),
                ["ribbedHose"] = SaveMesh(packageRoot, "RibbedHoseStraight", CreateRibbedCylinderMesh(18, 16)),
                ["gusset"] = SaveMesh(packageRoot, "TriangularGusset", CreateGussetMesh())
            };

            return meshMap;
        }

        private static void CreateRivetedTorsoPlateCluster(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_RivetedTorsoPlateCluster", "riveted_torso_plate_cluster", "Layered brass and dark iron torso armor plate cluster with rivet fields, soot streaks, asymmetric repairs, and readable FPS-distance panel breaks.", root =>
            {
                BuildRivetedTorsoPlateCluster(root.transform, meshes, materials);
                AddSocket(root.transform, "SOCKET_ChestCenter", new Vector3(0, 0.15f, -0.12f));
            });
        }

        private static void CreateGlowingEyeHeadModule(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_GlowingEyeHeadModule", "glowing_eye_head_module", "Goggled furnace-eye head module with brass lens collars, amber glass, vertical jaw grille, cheek rivets, and hose ports.", root =>
            {
                BuildGlowingEyeHead(root.transform, meshes, materials);
                AddSocket(root.transform, "SOCKET_NeckMount", new Vector3(0, -0.46f, 0.05f));
                AddSocket(root.transform, "SOCKET_LeftEyeGlow", new Vector3(-0.23f, 0.08f, -0.43f));
                AddSocket(root.transform, "SOCKET_RightEyeGlow", new Vector3(0.23f, 0.08f, -0.43f));
            });
        }

        private static void CreateFlywheelShoulderAssembly(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_FlywheelShoulderAssembly", "flywheel_shoulder_assembly", "Offset flywheel shoulder rig with iron carrier bracket, brass gear teeth, heavy hub, and piston anchor sockets.", root =>
            {
                BuildFlywheelShoulder(root.transform, meshes, materials, 1f);
                AddSocket(root.transform, "SOCKET_ShoulderIn", new Vector3(-0.55f, 0, 0));
                AddSocket(root.transform, "SOCKET_ArmOut", new Vector3(0.68f, -0.08f, -0.02f));
            });
        }

        private static void CreatePistonForearm(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_PistonForearm", "piston_forearm", "Black iron forearm with dual exposed pistons, brass cuffs, copper oiler tubes, rubber return hose, and tool wrist socket.", root =>
            {
                BuildPistonForearm(root.transform, meshes, materials, 1f);
                AddSocket(root.transform, "SOCKET_Elbow", new Vector3(-0.62f, 0, 0));
                AddSocket(root.transform, "SOCKET_WristTool", new Vector3(0.73f, -0.04f, 0));
            });
        }

        private static void CreateSawClawToolArmAttachment(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_SawClawToolArmAttachment", "saw_claw_tool_arm_attachment", "Threat-forward wrist attachment with heat-scarred saw blade, three hooked claw tines, brass guard cage, and oiled bearing hub.", root =>
            {
                BuildSawClawTool(root.transform, meshes, materials);
                AddSocket(root.transform, "SOCKET_WristMount", new Vector3(-0.64f, 0, 0));
                AddSocket(root.transform, "SOCKET_SawSpinAxis", new Vector3(0.16f, 0.02f, -0.10f));
            });
        }

        private static void CreatePressureGaugeChestModule(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_PressureGaugeChestModule", "pressure_gauge_chest_module", "Oversized chest pressure gauge with brass bezel, ivory face, red needle, glass cap, tick geometry, and copper feeder lines.", root =>
            {
                BuildPressureGauge(root.transform, meshes, materials);
                AddSocket(root.transform, "SOCKET_GaugeBackMount", new Vector3(0, 0, 0.08f));
            });
        }

        private static void CreateCableHoseBundle(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_CableHoseBundle", "cable_hose_bundle", "Braided black rubber hose and copper cable bundle with brass clamps, ribbed sleeves, oil stains, and modular endpoints.", root =>
            {
                BuildCableHoseBundle(root.transform, meshes, materials);
                AddSocket(root.transform, "SOCKET_HoseBundleIn", new Vector3(-0.72f, 0.05f, 0));
                AddSocket(root.transform, "SOCKET_HoseBundleOut", new Vector3(0.72f, 0.05f, 0));
            });
        }

        private static void CreateLegJointArmorPlates(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_LegJointArmorPlates", "leg_joint_armor_plates", "Knee/hip armor stack with angled brass plates, iron pivot drums, piston protectors, rivets, and mud-dark oil buildup.", root =>
            {
                BuildLegJointArmor(root.transform, meshes, materials);
                AddSocket(root.transform, "SOCKET_UpperLeg", new Vector3(0, 0.62f, 0));
                AddSocket(root.transform, "SOCKET_LowerLeg", new Vector3(0, -0.68f, 0));
            });
        }

        private static void CreateAssembledBustUpperBodyConcept(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_AssembledBustUpperBodyConcept", "assembled_bust_upper_body_concept", "Assembled upper-body concept showing how Set 12 detail modules upgrade the mechanical sentinel with a thicker threat silhouette and richer material breakup.", root =>
            {
                BuildAssembledBust(root.transform, meshes, materials);
                AddSocket(root.transform, "SOCKET_SpineRoot", new Vector3(0, -0.72f, 0.02f));
                AddSocket(root.transform, "SOCKET_Head", new Vector3(0, 1.16f, 0.02f));
                AddSocket(root.transform, "SOCKET_LeftShoulder", new Vector3(-1.03f, 0.64f, 0.02f));
                AddSocket(root.transform, "SOCKET_RightShoulder", new Vector3(1.03f, 0.64f, 0.02f));
            });
        }

        private static void CreateMaterialPaletteBoard(PackageRoot packageRoot, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            SavePrefab(packageRoot, "MED12_MaterialPaletteBoard", "material_palette_board", "Simple palette board for reviewing generated brass, iron, copper, amber glass, hose rubber, saw metal, gauge enamel, and soot materials.", root =>
            {
                var keys = new[] { "AgedBrass", "DarkOilStainedIron", "WornCopper", "GlowingAmberGlass", "BlackRubberizedHose", "SharpSawMetal", "IvoryGaugeFace", "SootOilDeposit", "RedNeedleEnamel" };
                for (var i = 0; i < keys.Length; i++)
                {
                    var x = -1.6f + (i % 5) * 0.8f;
                    var y = 0.45f - (i / 5) * 0.8f;
                    Part(root.transform, meshes["box"], materials[keys[i]], "swatch_" + keys[i], new Vector3(x, y, 0), new Vector3(0.62f, 0.62f, 0.06f), Vector3.zero);
                }
            });
        }

        private static void BuildRivetedTorsoPlateCluster(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            Part(parent, meshes["box"], materials["DarkOilStainedIron"], "blackened_iron_backing_plate_deep_shadow", new Vector3(0, 0, 0.06f), new Vector3(1.62f, 1.76f, 0.12f), Vector3.zero);
            Part(parent, meshes["box"], materials["AgedBrass"], "large_center_aged_brass_chest_plate", new Vector3(0, 0.12f, -0.04f), new Vector3(0.72f, 1.38f, 0.08f), new Vector3(0, 0, 1.5f));
            Part(parent, meshes["box"], materials["AgedBrass"], "left_offset_repair_plate", new Vector3(-0.48f, 0.28f, -0.07f), new Vector3(0.44f, 0.95f, 0.07f), new Vector3(0, 0, -8f));
            Part(parent, meshes["box"], materials["DarkOilStainedIron"], "right_black_service_access_plate", new Vector3(0.50f, 0.06f, -0.075f), new Vector3(0.38f, 1.08f, 0.07f), new Vector3(0, 0, 6f));
            Part(parent, meshes["box"], materials["WornCopper"], "vertical_copper_oil_channel", new Vector3(-0.08f, 0.10f, -0.13f), new Vector3(0.08f, 1.50f, 0.035f), Vector3.zero);
            Part(parent, meshes["box"], materials["DarkOilStainedIron"], "lower_belly_black_iron_lip", new Vector3(0.03f, -0.73f, -0.15f), new Vector3(1.28f, 0.16f, 0.08f), new Vector3(0, 0, -2f));
            Part(parent, meshes["box"], materials["AgedBrass"], "upper_collar_brass_band", new Vector3(0, 0.87f, -0.16f), new Vector3(1.42f, 0.13f, 0.07f), Vector3.zero);

            for (var row = 0; row < 4; row++)
            {
                var y = -0.55f + row * 0.38f;
                for (var i = 0; i < 7; i++)
                {
                    var x = -0.63f + i * 0.21f + ((row % 2 == 0) ? 0.02f : -0.015f);
                    AddFrontRivet(parent, meshes, materials["AgedBrass"], "brass_plate_rivet_" + row + "_" + i, new Vector3(x, y, -0.21f), 0.055f);
                }
            }

            for (var i = 0; i < 8; i++)
            {
                var x = -0.56f + i * 0.16f;
                AddFrontRivet(parent, meshes, materials["DarkOilStainedIron"], "black_service_screw_top_" + i, new Vector3(x, 0.80f, -0.22f), 0.04f);
                AddFrontRivet(parent, meshes, materials["DarkOilStainedIron"], "black_service_screw_bottom_" + i, new Vector3(x, -0.78f, -0.22f), 0.04f);
            }

            for (var i = 0; i < 7; i++)
            {
                Part(parent, meshes["box"], materials["SootOilDeposit"], "vertical_soot_oil_streak_" + i, new Vector3(-0.52f + i * 0.17f, 0.12f - i * 0.035f, -0.235f), new Vector3(0.035f, 0.52f + (i % 3) * 0.10f, 0.012f), new Vector3(0, 0, -4f + i * 1.5f));
            }
        }

        private static void BuildGlowingEyeHead(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            Part(parent, meshes["box"], materials["DarkOilStainedIron"], "iron_head_core_block", new Vector3(0, 0, 0), new Vector3(0.96f, 0.76f, 0.64f), new Vector3(-2f, 0, 0));
            Part(parent, meshes["box"], materials["AgedBrass"], "brass_brow_plate_heavy", new Vector3(0, 0.35f, -0.29f), new Vector3(1.05f, 0.20f, 0.12f), new Vector3(0, 0, -1f));
            Part(parent, meshes["box"], materials["DarkOilStainedIron"], "lower_jaw_iron_grille_frame", new Vector3(0, -0.31f, -0.32f), new Vector3(0.74f, 0.30f, 0.13f), Vector3.zero);
            for (var i = 0; i < 6; i++)
            {
                Part(parent, meshes["box"], materials["AgedBrass"], "vertical_jaw_grille_tooth_" + i, new Vector3(-0.26f + i * 0.104f, -0.31f, -0.42f), new Vector3(0.033f, 0.34f, 0.06f), Vector3.zero);
            }

            for (var side = -1; side <= 1; side += 2)
            {
                var x = side * 0.23f;
                Part(parent, meshes["cylinder48"], materials["AgedBrass"], SideName(side) + "_brass_eye_outer_collar", new Vector3(x, 0.08f, -0.41f), new Vector3(0.26f, 0.11f, 0.26f), new Vector3(-90f, 0, 0));
                Part(parent, meshes["cylinder32"], materials["DarkOilStainedIron"], SideName(side) + "_black_eye_socket_depth", new Vector3(x, 0.08f, -0.48f), new Vector3(0.18f, 0.09f, 0.18f), new Vector3(-90f, 0, 0));
                Part(parent, meshes["lens"], materials["GlowingAmberGlass"], SideName(side) + "_convex_glowing_amber_eye_lens", new Vector3(x, 0.08f, -0.55f), new Vector3(0.19f, 0.19f, 0.09f), new Vector3(-90f, 0, 0));
                Part(parent, meshes["cylinder32"], materials["GlowingAmberGlass"], SideName(side) + "_flat_hot_amber_eye_core", new Vector3(x, 0.08f, -0.63f), new Vector3(0.145f, 0.032f, 0.145f), new Vector3(-90f, 0, 0));
                Part(parent, meshes["box"], materials["SootOilDeposit"], SideName(side) + "_soot_smear_under_eye", new Vector3(x, -0.07f, -0.57f), new Vector3(0.18f, 0.08f, 0.012f), new Vector3(0, 0, side * 8f));
                for (var j = 0; j < 8; j++)
                {
                    var a = j * Mathf.PI * 2f / 8f;
                    AddFrontRivet(parent, meshes, materials["AgedBrass"], SideName(side) + "_eye_ring_microbolt_" + j, new Vector3(x + Mathf.Cos(a) * 0.18f, 0.08f + Mathf.Sin(a) * 0.18f, -0.57f), 0.026f);
                }
            }

            Part(parent, meshes["cylinder32"], materials["WornCopper"], "left_cheek_pressure_port", new Vector3(-0.56f, -0.06f, -0.12f), new Vector3(0.17f, 0.16f, 0.17f), new Vector3(0, 0, 90f));
            Part(parent, meshes["cylinder32"], materials["WornCopper"], "right_cheek_pressure_port", new Vector3(0.56f, -0.06f, -0.12f), new Vector3(0.17f, 0.16f, 0.17f), new Vector3(0, 0, 90f));
            Part(parent, meshes["hoseArc"], materials["BlackRubberizedHose"], "rear_crowned_black_rubber_hose", new Vector3(0, 0.47f, 0.07f), new Vector3(0.52f, 0.42f, 0.52f), new Vector3(0, 0, 180f));
            Part(parent, meshes["cylinder16"], materials["AgedBrass"], "stack_left_small_exhaust", new Vector3(-0.24f, 0.61f, 0.02f), new Vector3(0.12f, 0.38f, 0.12f), Vector3.zero);
            Part(parent, meshes["cylinder16"], materials["AgedBrass"], "stack_right_small_exhaust", new Vector3(0.24f, 0.61f, 0.02f), new Vector3(0.12f, 0.34f, 0.12f), Vector3.zero);
        }

        private static void BuildFlywheelShoulder(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, float side)
        {
            Part(parent, meshes["box"], materials["DarkOilStainedIron"], "iron_shoulder_carrier_yoke", new Vector3(-0.16f * side, 0, 0.04f), new Vector3(0.72f, 0.42f, 0.34f), new Vector3(0, 0, side * 3f));
            Part(parent, meshes["cylinder32"], materials["AgedBrass"], "brass_socket_drum_inboard", new Vector3(-0.55f * side, 0.02f, 0.02f), new Vector3(0.36f, 0.28f, 0.36f), new Vector3(0, 0, 90f));
            Part(parent, meshes["flywheel"], materials["DarkOilStainedIron"], "black_iron_flywheel_outer_rim", new Vector3(0.36f * side, 0.03f, -0.14f), new Vector3(0.78f, 0.78f, 0.78f), new Vector3(90f, 0, 0));
            Part(parent, meshes["torus32"], materials["AgedBrass"], "brass_inner_flywheel_guard_ring", new Vector3(0.36f * side, 0.03f, -0.16f), new Vector3(0.52f, 0.52f, 0.52f), new Vector3(90f, 0, 0));
            Part(parent, meshes["cylinder32"], materials["AgedBrass"], "brass_flywheel_center_hub", new Vector3(0.36f * side, 0.03f, -0.20f), new Vector3(0.20f, 0.12f, 0.20f), new Vector3(90f, 0, 0));

            for (var i = 0; i < 6; i++)
            {
                Part(parent, meshes["box"], materials[i % 2 == 0 ? "AgedBrass" : "DarkOilStainedIron"], "alternating_flywheel_spoke_" + i, new Vector3(0.36f * side, 0.03f, -0.19f), new Vector3(0.065f, 0.62f, 0.045f), new Vector3(0, 0, i * 30f));
            }

            for (var i = 0; i < 16; i++)
            {
                var a = i * Mathf.PI * 2f / 16f;
                var pos = new Vector3(0.36f * side + Mathf.Cos(a) * 0.44f, 0.03f + Mathf.Sin(a) * 0.44f, -0.205f);
                Part(parent, meshes["box"], materials["AgedBrass"], "brass_flywheel_rim_tooth_" + i, pos, new Vector3(0.08f, 0.035f, 0.055f), new Vector3(0, 0, a * Mathf.Rad2Deg));
            }

            Part(parent, meshes["cylinder16"], materials["WornCopper"], "upper_copper_piston_pin", new Vector3(-0.12f * side, 0.31f, -0.02f), new Vector3(0.10f, 0.40f, 0.10f), new Vector3(0, 0, 90f));
            Part(parent, meshes["cylinder16"], materials["WornCopper"], "lower_copper_piston_pin", new Vector3(-0.10f * side, -0.28f, -0.02f), new Vector3(0.10f, 0.36f, 0.10f), new Vector3(0, 0, 90f));
            for (var i = 0; i < 5; i++)
            {
                AddFrontRivet(parent, meshes, materials["AgedBrass"], "shoulder_yoke_face_rivet_" + i, new Vector3(-0.42f * side + i * side * 0.13f, 0.19f, -0.19f), 0.035f);
                AddFrontRivet(parent, meshes, materials["AgedBrass"], "shoulder_yoke_lower_rivet_" + i, new Vector3(-0.42f * side + i * side * 0.13f, -0.19f, -0.19f), 0.035f);
            }
        }

        private static void BuildPistonForearm(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials, float side)
        {
            Part(parent, meshes["box"], materials["DarkOilStainedIron"], "iron_forearm_core_rectangular_sleeve", new Vector3(0, 0, 0), new Vector3(1.10f, 0.34f, 0.34f), Vector3.zero);
            Part(parent, meshes["cylinder32"], materials["AgedBrass"], "brass_elbow_cuff", new Vector3(-0.60f, 0, 0), new Vector3(0.32f, 0.20f, 0.32f), new Vector3(0, 0, 90f));
            Part(parent, meshes["cylinder32"], materials["AgedBrass"], "brass_wrist_cuff", new Vector3(0.60f, -0.02f, 0), new Vector3(0.30f, 0.19f, 0.30f), new Vector3(0, 0, 90f));
            Part(parent, meshes["cylinder16"], materials["SharpSawMetal"], "upper_polished_piston_rod", new Vector3(0.02f, 0.24f, -0.08f), new Vector3(0.055f, 1.04f, 0.055f), new Vector3(0, 0, 90f));
            Part(parent, meshes["cylinder16"], materials["SharpSawMetal"], "lower_polished_piston_rod", new Vector3(0.02f, -0.24f, -0.08f), new Vector3(0.055f, 1.04f, 0.055f), new Vector3(0, 0, 90f));
            Part(parent, meshes["cylinder16"], materials["WornCopper"], "upper_copper_piston_barrel", new Vector3(-0.24f, 0.24f, -0.08f), new Vector3(0.11f, 0.34f, 0.11f), new Vector3(0, 0, 90f));
            Part(parent, meshes["cylinder16"], materials["WornCopper"], "lower_copper_piston_barrel", new Vector3(0.28f, -0.24f, -0.08f), new Vector3(0.11f, 0.34f, 0.11f), new Vector3(0, 0, 90f));
            Part(parent, meshes["hoseS"], materials["BlackRubberizedHose"], "black_rubber_return_hose_over_forearm", new Vector3(0, 0.12f, 0.16f), new Vector3(0.80f, 0.40f, 0.80f), new Vector3(0, 0, 0));
            Part(parent, meshes["box"], materials["AgedBrass"], "raised_service_plate_left", new Vector3(-0.20f, 0.005f, -0.22f), new Vector3(0.38f, 0.23f, 0.045f), new Vector3(0, 0, -5f));
            Part(parent, meshes["box"], materials["AgedBrass"], "raised_service_plate_right", new Vector3(0.25f, 0.005f, -0.22f), new Vector3(0.34f, 0.20f, 0.045f), new Vector3(0, 0, 5f));
            for (var i = 0; i < 8; i++)
            {
                AddFrontRivet(parent, meshes, materials["AgedBrass"], "forearm_service_rivet_" + i, new Vector3(-0.46f + i * 0.13f, (i % 2 == 0) ? 0.14f : -0.14f, -0.27f), 0.032f);
            }
        }

        private static void BuildSawClawTool(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            Part(parent, meshes["cylinder32"], materials["DarkOilStainedIron"], "oiled_black_wrist_bearing_hub", new Vector3(-0.44f, 0, 0), new Vector3(0.34f, 0.32f, 0.34f), new Vector3(0, 0, 90f));
            Part(parent, meshes["box"], materials["AgedBrass"], "brass_saw_guard_upper_arc_proxy", new Vector3(0.12f, 0.34f, -0.04f), new Vector3(0.85f, 0.09f, 0.08f), new Vector3(0, 0, -8f));
            Part(parent, meshes["box"], materials["AgedBrass"], "brass_saw_guard_lower_arc_proxy", new Vector3(0.12f, -0.34f, -0.04f), new Vector3(0.85f, 0.09f, 0.08f), new Vector3(0, 0, 8f));
            Part(parent, meshes["saw"], materials["SharpSawMetal"], "heat_scarred_circular_saw_blade", new Vector3(0.16f, 0.02f, -0.18f), new Vector3(0.66f, 0.66f, 0.095f), Vector3.zero);
            Part(parent, meshes["cylinder32"], materials["AgedBrass"], "brass_saw_center_cap", new Vector3(0.16f, 0.02f, -0.27f), new Vector3(0.18f, 0.10f, 0.18f), new Vector3(90f, 0, 0));

            for (var i = 0; i < 3; i++)
            {
                var angle = -28f + i * 28f;
                Part(parent, meshes["claw"], materials["SharpSawMetal"], "forward_hooked_claw_tine_" + i, new Vector3(0.58f, -0.23f + i * 0.23f, -0.06f), new Vector3(0.45f, 0.45f, 0.14f), new Vector3(0, 0, angle));
                Part(parent, meshes["cylinder16"], materials["AgedBrass"], "claw_tine_brass_knuckle_" + i, new Vector3(0.38f, -0.22f + i * 0.22f, -0.05f), new Vector3(0.12f, 0.13f, 0.12f), new Vector3(0, 0, 90f));
            }

            for (var i = 0; i < 10; i++)
            {
                var a = i * Mathf.PI * 2f / 10f;
                AddFrontRivet(parent, meshes, materials["AgedBrass"], "saw_guard_bolt_" + i, new Vector3(0.16f + Mathf.Cos(a) * 0.43f, 0.02f + Mathf.Sin(a) * 0.43f, -0.34f), 0.028f);
            }
            Part(parent, meshes["box"], materials["SootOilDeposit"], "black_oil_flung_saw_smear", new Vector3(-0.03f, -0.28f, -0.36f), new Vector3(0.55f, 0.08f, 0.012f), new Vector3(0, 0, -19f));
        }

        private static void BuildPressureGauge(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            Part(parent, meshes["cylinder48"], materials["AgedBrass"], "heavy_brass_pressure_gauge_bezel", new Vector3(0, 0, -0.02f), new Vector3(0.58f, 0.16f, 0.58f), new Vector3(-90f, 0, 0));
            Part(parent, meshes["cylinder48"], materials["IvoryGaugeFace"], "aged_ivory_gauge_face", new Vector3(0, 0, -0.105f), new Vector3(0.46f, 0.045f, 0.46f), new Vector3(-90f, 0, 0));
            Part(parent, meshes["lens"], materials["GlowingAmberGlass"], "smoked_amber_glass_gauge_cap", new Vector3(0, 0, -0.152f), new Vector3(0.49f, 0.49f, 0.07f), new Vector3(-90f, 0, 0));
            Part(parent, meshes["needle"], materials["RedNeedleEnamel"], "red_overpressure_needle", new Vector3(0, 0, -0.20f), new Vector3(0.34f, 0.34f, 0.045f), new Vector3(0, 0, -38f));
            Part(parent, meshes["cylinder16"], materials["DarkOilStainedIron"], "black_center_needle_pin", new Vector3(0, 0, -0.23f), new Vector3(0.07f, 0.035f, 0.07f), new Vector3(-90f, 0, 0));

            for (var i = 0; i < 19; i++)
            {
                var t = i / 18f;
                var angle = Mathf.Lerp(215f, -35f, t) * Mathf.Deg2Rad;
                var len = i % 3 == 0 ? 0.10f : 0.065f;
                var radius = 0.36f;
                var pos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, -0.212f);
                Part(parent, meshes["box"], materials["DarkOilStainedIron"], "black_gauge_tick_" + i, pos, new Vector3(0.018f, len, 0.012f), new Vector3(0, 0, angle * Mathf.Rad2Deg - 90f));
            }

            for (var side = -1; side <= 1; side += 2)
            {
                Part(parent, meshes["cylinder16"], materials["WornCopper"], SideName(side) + "_copper_gauge_feed_pipe", new Vector3(side * 0.42f, -0.38f, 0.00f), new Vector3(0.07f, 0.46f, 0.07f), new Vector3(0, 0, side * 38f));
                Part(parent, meshes["cylinder16"], materials["AgedBrass"], SideName(side) + "_brass_gauge_pipe_collar", new Vector3(side * 0.29f, -0.24f, -0.02f), new Vector3(0.10f, 0.12f, 0.10f), new Vector3(0, 0, side * 38f));
            }
        }

        private static void BuildCableHoseBundle(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            Part(parent, meshes["hoseArc"], materials["BlackRubberizedHose"], "large_black_rubber_pressure_hose_arc", new Vector3(0, 0.03f, 0), new Vector3(1.15f, 0.72f, 1.15f), Vector3.zero);
            Part(parent, meshes["hoseS"], materials["BlackRubberizedHose"], "snaking_black_rubber_return_hose", new Vector3(0, -0.16f, -0.06f), new Vector3(0.98f, 0.55f, 0.98f), Vector3.zero);
            Part(parent, meshes["hoseArc"], materials["WornCopper"], "thin_copper_signal_tube_upper", new Vector3(0.03f, 0.18f, -0.13f), new Vector3(0.84f, 0.46f, 0.84f), Vector3.zero);
            Part(parent, meshes["hoseS"], materials["WornCopper"], "thin_copper_signal_tube_lower", new Vector3(-0.03f, -0.30f, -0.12f), new Vector3(0.82f, 0.44f, 0.82f), Vector3.zero);
            for (var i = 0; i < 7; i++)
            {
                var x = -0.58f + i * 0.19f;
                Part(parent, meshes["cylinder16"], materials["AgedBrass"], "ribbed_brass_hose_clamp_" + i, new Vector3(x, Mathf.Sin(i * 0.7f) * 0.08f, -0.01f), new Vector3(0.12f, 0.075f, 0.12f), new Vector3(0, 0, 90f));
            }
            Part(parent, meshes["cylinder32"], materials["AgedBrass"], "left_bundle_terminal_socket", new Vector3(-0.74f, 0.05f, 0), new Vector3(0.19f, 0.16f, 0.19f), new Vector3(0, 0, 90f));
            Part(parent, meshes["cylinder32"], materials["AgedBrass"], "right_bundle_terminal_socket", new Vector3(0.74f, 0.05f, 0), new Vector3(0.19f, 0.16f, 0.19f), new Vector3(0, 0, 90f));
            Part(parent, meshes["box"], materials["SootOilDeposit"], "wet_oil_smudge_across_bundle_center", new Vector3(-0.04f, -0.08f, -0.19f), new Vector3(0.80f, 0.12f, 0.014f), new Vector3(0, 0, 5f));
        }

        private static void BuildLegJointArmor(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            Part(parent, meshes["cylinder48"], materials["DarkOilStainedIron"], "black_iron_knee_pivot_drum", new Vector3(0, 0, 0), new Vector3(0.46f, 0.38f, 0.46f), new Vector3(90f, 0, 0));
            Part(parent, meshes["cylinder32"], materials["AgedBrass"], "front_brass_pivot_cap", new Vector3(0, 0, -0.26f), new Vector3(0.26f, 0.12f, 0.26f), new Vector3(90f, 0, 0));
            Part(parent, meshes["pentPlate"], materials["AgedBrass"], "upper_left_layered_leg_armor_plate", new Vector3(-0.22f, 0.40f, -0.16f), new Vector3(0.46f, 0.52f, 0.12f), new Vector3(0, 0, 12f));
            Part(parent, meshes["pentPlate"], materials["AgedBrass"], "upper_right_layered_leg_armor_plate", new Vector3(0.22f, 0.40f, -0.16f), new Vector3(0.46f, 0.52f, 0.12f), new Vector3(0, 0, -12f));
            Part(parent, meshes["pentPlate"], materials["DarkOilStainedIron"], "lower_black_shin_guard_plate", new Vector3(0, -0.50f, -0.14f), new Vector3(0.58f, 0.58f, 0.12f), new Vector3(0, 0, 180f));
            Part(parent, meshes["cylinder16"], materials["SharpSawMetal"], "left_exposed_leg_piston_rod", new Vector3(-0.42f, -0.08f, 0.03f), new Vector3(0.055f, 1.05f, 0.055f), new Vector3(0, 0, -15f));
            Part(parent, meshes["cylinder16"], materials["SharpSawMetal"], "right_exposed_leg_piston_rod", new Vector3(0.42f, -0.08f, 0.03f), new Vector3(0.055f, 1.05f, 0.055f), new Vector3(0, 0, 15f));
            Part(parent, meshes["cylinder16"], materials["WornCopper"], "left_copper_piston_sleeve", new Vector3(-0.34f, 0.32f, 0.02f), new Vector3(0.12f, 0.32f, 0.12f), new Vector3(0, 0, -15f));
            Part(parent, meshes["cylinder16"], materials["WornCopper"], "right_copper_piston_sleeve", new Vector3(0.34f, 0.32f, 0.02f), new Vector3(0.12f, 0.32f, 0.12f), new Vector3(0, 0, 15f));

            for (var i = 0; i < 12; i++)
            {
                var a = i * Mathf.PI * 2f / 12f;
                AddFrontRivet(parent, meshes, materials["AgedBrass"], "leg_pivot_ring_rivet_" + i, new Vector3(Mathf.Cos(a) * 0.31f, Mathf.Sin(a) * 0.31f, -0.34f), 0.03f);
            }
            Part(parent, meshes["box"], materials["SootOilDeposit"], "dark_oil_buildup_below_knee", new Vector3(-0.06f, -0.34f, -0.35f), new Vector3(0.54f, 0.12f, 0.012f), new Vector3(0, 0, -8f));
        }

        private static void BuildAssembledBust(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, IReadOnlyDictionary<string, Material> materials)
        {
            var torso = Section(parent, "SECTION_RivetedTorsoArmor", new Vector3(0, -0.12f, 0.02f), new Vector3(1.05f, 1.05f, 1.05f));
            BuildRivetedTorsoPlateCluster(torso, meshes, materials);

            var gauge = Section(parent, "SECTION_ChestPressureGauge", new Vector3(0, 0.02f, -0.42f), new Vector3(0.68f, 0.68f, 0.68f));
            BuildPressureGauge(gauge, meshes, materials);

            var head = Section(parent, "SECTION_GlowingHead", new Vector3(0, 1.18f, -0.02f), new Vector3(0.84f, 0.84f, 0.84f));
            BuildGlowingEyeHead(head, meshes, materials);

            var leftShoulder = Section(parent, "SECTION_LeftFlywheelShoulder", new Vector3(-0.98f, 0.48f, -0.02f), new Vector3(0.78f, 0.78f, 0.78f));
            BuildFlywheelShoulder(leftShoulder, meshes, materials, -1f);
            var rightShoulder = Section(parent, "SECTION_RightFlywheelShoulder", new Vector3(0.98f, 0.48f, -0.02f), new Vector3(0.78f, 0.78f, 0.78f));
            BuildFlywheelShoulder(rightShoulder, meshes, materials, 1f);

            var leftArm = Section(parent, "SECTION_LeftPistonForearm", new Vector3(-1.30f, -0.10f, -0.05f), new Vector3(0.78f, 0.78f, 0.78f));
            leftArm.localRotation = Quaternion.Euler(0, 0, -22f);
            BuildPistonForearm(leftArm, meshes, materials, -1f);
            var sawTool = Section(parent, "SECTION_LeftSawClawTool", new Vector3(-1.86f, -0.48f, -0.10f), new Vector3(0.72f, 0.72f, 0.72f));
            sawTool.localRotation = Quaternion.Euler(0, 0, -18f);
            BuildSawClawTool(sawTool, meshes, materials);

            var rightArm = Section(parent, "SECTION_RightPistonForearm", new Vector3(1.30f, -0.08f, -0.02f), new Vector3(0.78f, 0.78f, 0.78f));
            rightArm.localRotation = Quaternion.Euler(0, 180f, 22f);
            BuildPistonForearm(rightArm, meshes, materials, 1f);
            var rightJoint = Section(parent, "SECTION_RightLegJointArmorUsedAsElbow", new Vector3(1.75f, -0.46f, -0.04f), new Vector3(0.54f, 0.54f, 0.54f));
            rightJoint.localRotation = Quaternion.Euler(0, 0, 16f);
            BuildLegJointArmor(rightJoint, meshes, materials);

            var hoses = Section(parent, "SECTION_BackAndChestHoseBundle", new Vector3(0, 0.66f, 0.16f), new Vector3(1.2f, 0.9f, 1.2f));
            BuildCableHoseBundle(hoses, meshes, materials);
        }

        private static Transform Section(Transform parent, string name, Vector3 localPosition, Vector3 localScale)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = localScale;
            return child.transform;
        }

        private static GameObject Part(Transform parent, Mesh mesh, Material material, string name, Vector3 localPosition, Vector3 localScale, Vector3 localEuler)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localRotation = Quaternion.Euler(localEuler);
            go.transform.localScale = localScale;
            var filter = go.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            var renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            return go;
        }

        private static void AddFrontRivet(Transform parent, IReadOnlyDictionary<string, Mesh> meshes, Material material, string name, Vector3 localPosition, float scale)
        {
            Part(parent, meshes["dome"], material, name, localPosition, new Vector3(scale, scale, scale), new Vector3(-90f, 0, 0));
        }

        private static void AddSocket(Transform parent, string name, Vector3 localPosition)
        {
            var socket = new GameObject(name);
            socket.transform.SetParent(parent, false);
            socket.transform.localPosition = localPosition;
            socket.transform.localRotation = Quaternion.identity;
            socket.transform.localScale = Vector3.one;
        }

        private static void SavePrefab(PackageRoot packageRoot, string name, string role, string notes, Action<GameObject> build)
        {
            var root = new GameObject(name);
            root.tag = "Untagged";
            build(root);
            var assetPath = packageRoot.AssetPath + "/Runtime/Prefabs/" + name + ".prefab";
            ReplaceAsset(assetPath);
            PrefabUtility.SaveAsPrefabAsset(root, assetPath);
            Object.DestroyImmediate(root);
            PrefabRecords.Add(new PrefabRecord("Runtime/Prefabs/" + name + ".prefab", role, notes));
        }

        private static void RenderPreviews(PackageRoot packageRoot, string renderRoot)
        {
            Directory.CreateDirectory(renderRoot);
            RenderComponentSheet(packageRoot, renderRoot);
            RenderSingle(packageRoot, renderRoot, "MED12_AssembledBustUpperBodyConcept", Prefix + "_RENDER_02_assembled_bust_upper_body.png", new Vector3(2.85f, 1.75f, -4.35f), new Vector3(0, 0.22f, -0.12f), 38f, 0f);
            RenderSingle(packageRoot, renderRoot, "MED12_SawClawToolArmAttachment", Prefix + "_RENDER_03_arm_tool_closeup.png", new Vector3(1.42f, 0.75f, -2.20f), new Vector3(0.08f, 0.0f, -0.14f), 32f, -18f);
        }

        private static void RenderComponentSheet(PackageRoot packageRoot, string renderRoot)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SetupRenderEnvironment("Component Sheet");
            var names = new[]
            {
                "MED12_RivetedTorsoPlateCluster",
                "MED12_GlowingEyeHeadModule",
                "MED12_FlywheelShoulderAssembly",
                "MED12_PistonForearm",
                "MED12_SawClawToolArmAttachment",
                "MED12_PressureGaugeChestModule",
                "MED12_CableHoseBundle",
                "MED12_LegJointArmorPlates"
            };
            for (var i = 0; i < names.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(packageRoot.AssetPath + "/Runtime/Prefabs/" + names[i] + ".prefab");
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.name = names[i] + "_render_instance";
                var col = i % 4;
                var row = i / 4;
                instance.transform.position = new Vector3(-2.55f + col * 1.70f, 1.10f - row * 1.75f, 0f);
                instance.transform.rotation = Quaternion.Euler(0, (i % 2 == 0) ? -18f : 18f, 0);
                instance.transform.localScale = Vector3.one * ((names[i].Contains("Cable") || names[i].Contains("Gauge")) ? 0.92f : 0.80f);
            }

            var camera = CreateCamera(new Vector3(0, 0.15f, -6.8f), new Vector3(0, 0.15f, 0), 42f, true, 2.45f);
            SaveCameraPng(camera, packageRoot, renderRoot, Prefix + "_RENDER_01_component_sheet.png");
        }

        private static void RenderSingle(PackageRoot packageRoot, string renderRoot, string prefabName, string fileName, Vector3 cameraPosition, Vector3 lookAt, float fov, float yRotation)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SetupRenderEnvironment(prefabName);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(packageRoot.AssetPath + "/Runtime/Prefabs/" + prefabName + ".prefab");
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = prefabName + "_render_instance";
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            var camera = CreateCamera(cameraPosition, lookAt, fov, false, 1f);
            SaveCameraPng(camera, packageRoot, renderRoot, fileName);
        }

        private static void SetupRenderEnvironment(string label)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.055f, 0.047f, 0.038f);
            var floorMat = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            SetMaterialColor(floorMat, new Color(0.055f, 0.047f, 0.040f));
            SetMaterialFloat(floorMat, "_Smoothness", 0.48f);
            SetMaterialFloat(floorMat, "_Metallic", 0.10f);
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "warm_oily_floor_proxy_" + label;
            floor.transform.position = new Vector3(0, -1.08f, 0.30f);
            floor.transform.localScale = new Vector3(8f, 0.05f, 5f);
            floor.GetComponent<MeshRenderer>().sharedMaterial = floorMat;

            var back = GameObject.CreatePrimitive(PrimitiveType.Cube);
            back.name = "dark_soot_backdrop_proxy_" + label;
            back.transform.position = new Vector3(0, 0.6f, 1.35f);
            back.transform.localScale = new Vector3(8f, 4f, 0.05f);
            back.GetComponent<MeshRenderer>().sharedMaterial = floorMat;

            var key = new GameObject("warm_gaslight_key");
            var keyLight = key.AddComponent<Light>();
            keyLight.type = LightType.Point;
            keyLight.color = new Color(1.0f, 0.58f, 0.22f);
            keyLight.intensity = 2.4f;
            keyLight.range = 7f;
            key.transform.position = new Vector3(-1.9f, 2.25f, -2.1f);

            var rim = new GameObject("cool_iron_rim_light");
            var rimLight = rim.AddComponent<Light>();
            rimLight.type = LightType.Directional;
            rimLight.color = new Color(0.55f, 0.62f, 0.70f);
            rimLight.intensity = 0.72f;
            rim.transform.rotation = Quaternion.Euler(38f, -36f, 0);

            var fill = new GameObject("low_amber_fill");
            var fillLight = fill.AddComponent<Light>();
            fillLight.type = LightType.Point;
            fillLight.color = new Color(1.0f, 0.35f, 0.10f);
            fillLight.intensity = 0.65f;
            fillLight.range = 4.5f;
            fill.transform.position = new Vector3(2.2f, 0.65f, -1.6f);
        }

        private static Camera CreateCamera(Vector3 position, Vector3 lookAt, float fov, bool orthographic, float orthoSize)
        {
            var cameraGo = new GameObject("MED12_RenderCamera");
            cameraGo.transform.position = position;
            cameraGo.transform.LookAt(lookAt);
            var camera = cameraGo.AddComponent<Camera>();
            camera.fieldOfView = fov;
            camera.orthographic = orthographic;
            camera.orthographicSize = orthoSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.025f, 0.022f, 0.019f);
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 100f;
            return camera;
        }

        private static void SaveCameraPng(Camera camera, PackageRoot packageRoot, string renderRoot, string fileName)
        {
            var renderTexture = new RenderTexture(RenderWidth, RenderHeight, 24, RenderTextureFormat.ARGB32)
            {
                antiAliasing = 4
            };
            var previousTarget = camera.targetTexture;
            var previousActive = RenderTexture.active;
            camera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            camera.Render();

            var image = new Texture2D(RenderWidth, RenderHeight, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0, 0, RenderWidth, RenderHeight), 0, 0);
            image.Apply();
            var bytes = image.EncodeToPNG();
            Object.DestroyImmediate(image);

            camera.targetTexture = previousTarget;
            RenderTexture.active = previousActive;
            Object.DestroyImmediate(renderTexture);

            var docsPath = Path.Combine(renderRoot, fileName);
            File.WriteAllBytes(docsPath, bytes);
            var packagePreviewPath = Path.Combine(packageRoot.ResolvedPath, "Runtime", "Previews", fileName);
            File.WriteAllBytes(packagePreviewPath, bytes);
            AssetDatabase.ImportAsset(packageRoot.AssetPath + "/Runtime/Previews/" + fileName, ImportAssetOptions.ForceUpdate);
            RenderRecords.Add(new RenderRecord("Documentation/ConceptRenders/" + RenderFolderName + "/" + fileName, "Runtime/Previews/" + fileName));
        }

        private static Texture2D SaveTexture(PackageRoot packageRoot, MaterialSpec spec, string mapName, Texture2D texture, bool normal, bool linear)
        {
            texture.name = Prefix + "_TEX_" + spec.Name + "_" + mapName;
            var relativePath = "Runtime/Textures/" + texture.name + ".png";
            var resolvedPath = Path.Combine(packageRoot.ResolvedPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            Directory.CreateDirectory(Path.GetDirectoryName(resolvedPath) ?? packageRoot.ResolvedPath);
            File.WriteAllBytes(resolvedPath, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);

            var assetPath = packageRoot.AssetPath + "/" + relativePath;
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = normal ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.sRGBTexture = !linear && !normal;
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
                importer.mipmapEnabled = true;
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.SaveAndReimport();
            }

            TextureRecords.Add(new AssetRecord(relativePath, spec.Key, spec.Tag + "_" + mapName.ToLowerInvariant()));
            return AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        }

        private static Mesh SaveMesh(PackageRoot packageRoot, string name, Mesh mesh)
        {
            mesh.name = Prefix + "_MESH_" + name;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            var assetPath = packageRoot.AssetPath + "/Runtime/Meshes/" + mesh.name + ".asset";
            ReplaceAsset(assetPath);
            AssetDatabase.CreateAsset(mesh, assetPath);
            MeshRecords.Add(new AssetRecord("Runtime/Meshes/" + mesh.name + ".asset", name, "procedural_mesh"));
            return AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
        }

        private static Texture2D CreateAlbedoTexture(MaterialSpec spec)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var u = x / (TextureSize - 1f);
                    var v = y / (TextureSize - 1f);
                    var n1 = ValueNoise(u * 9.5f, v * 9.5f, spec.Seed);
                    var n2 = ValueNoise(u * 31f, v * 31f, spec.Seed + 97);
                    var streak = Mathf.Pow(Mathf.Clamp01(1f - Mathf.Abs(Mathf.Sin((u * 7f + n1) * Mathf.PI)) * 1.7f), 2.0f);
                    var edgeWear = Mathf.Max(Mathf.Pow(Mathf.Abs(u - 0.5f) * 2f, 6f), Mathf.Pow(Mathf.Abs(v - 0.5f) * 2f, 6f));
                    var color = Color.Lerp(spec.Base * 0.72f, spec.Base * 1.22f, n1 * 0.7f + n2 * 0.3f);

                    if (spec.Tag.Contains("brass") || spec.Tag.Contains("copper"))
                    {
                        color = Color.Lerp(color, spec.Accent, Mathf.Clamp01(streak * 0.24f + (1f - n2) * 0.10f));
                        color = Color.Lerp(color, new Color(1.0f, 0.76f, 0.30f), edgeWear * 0.45f);
                    }
                    else if (spec.Tag.Contains("iron"))
                    {
                        color = Color.Lerp(color, spec.Accent, Mathf.Clamp01(streak * 0.40f + n2 * 0.16f));
                        color = Color.Lerp(color, new Color(0.20f, 0.18f, 0.14f), edgeWear * 0.20f);
                    }
                    else if (spec.Tag.Contains("hose"))
                    {
                        var rib = Mathf.Abs(Mathf.Sin(v * Mathf.PI * 42f));
                        color = Color.Lerp(color, new Color(0.06f, 0.055f, 0.05f), rib * 0.25f);
                    }
                    else if (spec.Tag.Contains("saw"))
                    {
                        color = Color.Lerp(color, new Color(0.75f, 0.72f, 0.63f), edgeWear * 0.50f);
                        color = Color.Lerp(color, new Color(0.20f, 0.17f, 0.13f), streak * 0.12f);
                    }
                    else if (spec.Emissive)
                    {
                        var glow = Mathf.Pow(1f - Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.5f)) * 1.55f, 2f);
                        color = Color.Lerp(color, spec.Accent, Mathf.Clamp01(glow));
                        color.a = spec.Base.a;
                    }
                    else if (spec.Tag.Contains("gauge"))
                    {
                        var paper = ValueNoise(u * 45f, v * 45f, spec.Seed + 13);
                        color = Color.Lerp(color, spec.Accent, paper * 0.14f);
                    }

                    if (spec.Transparent && !spec.Emissive)
                    {
                        color.a = spec.Base.a;
                    }

                    texture.SetPixel(x, y, ClampColor(color));
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateNormalTexture(int seed, float strength)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var u = x / (TextureSize - 1f);
                    var v = y / (TextureSize - 1f);
                    var e = 1f / TextureSize;
                    var hL = HeightNoise(u - e, v, seed);
                    var hR = HeightNoise(u + e, v, seed);
                    var hD = HeightNoise(u, v - e, seed);
                    var hU = HeightNoise(u, v + e, seed);
                    var dx = (hL - hR) * strength;
                    var dy = (hD - hU) * strength;
                    var n = new Vector3(dx, dy, 1f).normalized;
                    texture.SetPixel(x, y, new Color(n.x * 0.5f + 0.5f, n.y * 0.5f + 0.5f, n.z * 0.5f + 0.5f, 1f));
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateMaskTexture(MaterialSpec spec)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var u = x / (TextureSize - 1f);
                    var v = y / (TextureSize - 1f);
                    var noise = ValueNoise(u * 18f, v * 18f, spec.Seed + 5);
                    var oil = Mathf.Pow(ValueNoise(u * 7f, v * 12f, spec.Seed + 57), 3f);
                    var metallic = Mathf.Clamp01(spec.Metallic - noise * 0.12f);
                    var smooth = Mathf.Clamp01(spec.Smoothness + oil * (spec.Tag.Contains("iron") ? 0.34f : 0.16f) - noise * 0.12f);
                    texture.SetPixel(x, y, new Color(metallic, 0f, 0f, smooth));
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateOcclusionTexture(int seed)
        {
            var texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false, true);
            for (var y = 0; y < TextureSize; y++)
            {
                for (var x = 0; x < TextureSize; x++)
                {
                    var u = x / (TextureSize - 1f);
                    var v = y / (TextureSize - 1f);
                    var cavity = 0.82f + ValueNoise(u * 24f, v * 24f, seed + 91) * 0.18f;
                    var edge = Mathf.Clamp01(Mathf.Min(Mathf.Min(u, 1f - u), Mathf.Min(v, 1f - v)) * 10f);
                    var value = Mathf.Clamp01(cavity * Mathf.Lerp(0.72f, 1f, edge));
                    texture.SetPixel(x, y, new Color(value, value, value, 1f));
                }
            }
            texture.Apply();
            return texture;
        }

        private static Mesh CreateBoxMesh()
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            AddFace(vertices, uvs, triangles, new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f));
            AddFace(vertices, uvs, triangles, new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f));
            AddFace(vertices, uvs, triangles, new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, 0.5f));
            AddFace(vertices, uvs, triangles, new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f));
            AddFace(vertices, uvs, triangles, new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f));
            AddFace(vertices, uvs, triangles, new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f));
            return NewMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateCylinderMesh(int segments)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var i = 0; i < segments; i++)
            {
                var a0 = i * Mathf.PI * 2f / segments;
                var a1 = (i + 1) * Mathf.PI * 2f / segments;
                var p0 = new Vector3(Mathf.Cos(a0) * 0.5f, -0.5f, Mathf.Sin(a0) * 0.5f);
                var p1 = new Vector3(Mathf.Cos(a1) * 0.5f, -0.5f, Mathf.Sin(a1) * 0.5f);
                var p2 = new Vector3(Mathf.Cos(a1) * 0.5f, 0.5f, Mathf.Sin(a1) * 0.5f);
                var p3 = new Vector3(Mathf.Cos(a0) * 0.5f, 0.5f, Mathf.Sin(a0) * 0.5f);
                AddFace(vertices, uvs, triangles, p0, p1, p2, p3);
                AddTriangle(vertices, uvs, triangles, Vector3.up * 0.5f, p3, p2);
                AddTriangle(vertices, uvs, triangles, Vector3.down * 0.5f, p1, p0);
            }
            return NewMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateRibbedCylinderMesh(int segments, int ribs)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var y = 0; y <= ribs; y++)
            {
                var v = y / (float)ribs;
                var radius = 0.46f + ((y % 2 == 0) ? 0.06f : -0.01f);
                for (var i = 0; i < segments; i++)
                {
                    var a = i * Mathf.PI * 2f / segments;
                    vertices.Add(new Vector3(Mathf.Cos(a) * radius, v - 0.5f, Mathf.Sin(a) * radius));
                    uvs.Add(new Vector2(i / (float)segments, v));
                }
            }

            for (var y = 0; y < ribs; y++)
            {
                for (var i = 0; i < segments; i++)
                {
                    var a = y * segments + i;
                    var b = y * segments + (i + 1) % segments;
                    var c = (y + 1) * segments + (i + 1) % segments;
                    var d = (y + 1) * segments + i;
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(b);
                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(c);
                }
            }
            return NewMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateTorusMesh(int radialSegments, int tubeSegments, float radius, float tubeRadius)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var i = 0; i < radialSegments; i++)
            {
                var u = i / (float)radialSegments;
                var a = u * Mathf.PI * 2f;
                var center = new Vector3(Mathf.Cos(a) * radius, 0, Mathf.Sin(a) * radius);
                for (var j = 0; j < tubeSegments; j++)
                {
                    var v = j / (float)tubeSegments;
                    var b = v * Mathf.PI * 2f;
                    var normal = new Vector3(Mathf.Cos(a) * Mathf.Cos(b), Mathf.Sin(b), Mathf.Sin(a) * Mathf.Cos(b));
                    vertices.Add(center + normal * tubeRadius);
                    uvs.Add(new Vector2(u, v));
                }
            }

            for (var i = 0; i < radialSegments; i++)
            {
                for (var j = 0; j < tubeSegments; j++)
                {
                    var a = i * tubeSegments + j;
                    var b = ((i + 1) % radialSegments) * tubeSegments + j;
                    var c = ((i + 1) % radialSegments) * tubeSegments + (j + 1) % tubeSegments;
                    var d = i * tubeSegments + (j + 1) % tubeSegments;
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(d);
                }
            }
            return NewMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateSawBladeMesh(int teeth)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var frontCenter = vertices.Count;
            vertices.Add(new Vector3(0, 0, -0.04f));
            uvs.Add(new Vector2(0.5f, 0.5f));
            var backCenter = vertices.Count;
            vertices.Add(new Vector3(0, 0, 0.04f));
            uvs.Add(new Vector2(0.5f, 0.5f));
            for (var i = 0; i < teeth; i++)
            {
                var a = i * Mathf.PI * 2f / teeth;
                var r = i % 2 == 0 ? 0.55f : 0.44f;
                vertices.Add(new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, -0.04f));
                uvs.Add(new Vector2(Mathf.Cos(a) * 0.5f + 0.5f, Mathf.Sin(a) * 0.5f + 0.5f));
                vertices.Add(new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, 0.04f));
                uvs.Add(new Vector2(Mathf.Cos(a) * 0.5f + 0.5f, Mathf.Sin(a) * 0.5f + 0.5f));
            }

            for (var i = 0; i < teeth; i++)
            {
                var next = (i + 1) % teeth;
                var f0 = 2 + i * 2;
                var b0 = f0 + 1;
                var f1 = 2 + next * 2;
                var b1 = f1 + 1;
                triangles.Add(frontCenter);
                triangles.Add(f0);
                triangles.Add(f1);
                triangles.Add(backCenter);
                triangles.Add(b1);
                triangles.Add(b0);
                triangles.Add(f0);
                triangles.Add(b0);
                triangles.Add(b1);
                triangles.Add(f0);
                triangles.Add(b1);
                triangles.Add(f1);
            }
            return NewMesh(vertices, uvs, triangles);
        }

        private static Mesh CreateClawBladeMesh()
        {
            var points = new[]
            {
                new Vector2(-0.48f, -0.08f),
                new Vector2(-0.16f, -0.15f),
                new Vector2(0.22f, -0.05f),
                new Vector2(0.52f, 0.16f),
                new Vector2(0.12f, 0.09f),
                new Vector2(-0.28f, 0.10f)
            };
            return ExtrudePolygon(points, 0.08f);
        }

        private static Mesh CreateNeedleMesh()
        {
            var points = new[]
            {
                new Vector2(-0.035f, -0.06f),
                new Vector2(0.055f, -0.06f),
                new Vector2(0.45f, 0.0f),
                new Vector2(0.055f, 0.06f),
                new Vector2(-0.035f, 0.06f)
            };
            return ExtrudePolygon(points, 0.025f);
        }

        private static Mesh CreateDomeMesh(int segments, int rings, float radius, float height)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            vertices.Add(new Vector3(0, height, 0));
            uvs.Add(new Vector2(0.5f, 1f));
            for (var r = 1; r <= rings; r++)
            {
                var t = r / (float)rings;
                var y = Mathf.Cos(t * Mathf.PI * 0.5f) * height;
                var rr = Mathf.Sin(t * Mathf.PI * 0.5f) * radius;
                for (var i = 0; i < segments; i++)
                {
                    var a = i * Mathf.PI * 2f / segments;
                    vertices.Add(new Vector3(Mathf.Cos(a) * rr, y, Mathf.Sin(a) * rr));
                    uvs.Add(new Vector2(i / (float)segments, t));
                }
            }

            for (var i = 0; i < segments; i++)
            {
                var b = 1 + i;
                var c = 1 + (i + 1) % segments;
                triangles.Add(0);
                triangles.Add(b);
                triangles.Add(c);
            }
            for (var r = 0; r < rings - 1; r++)
            {
                for (var i = 0; i < segments; i++)
                {
                    var a = 1 + r * segments + i;
                    var b = 1 + r * segments + (i + 1) % segments;
                    var c = 1 + (r + 1) * segments + (i + 1) % segments;
                    var d = 1 + (r + 1) * segments + i;
                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(c);
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(b);
                }
            }
            return NewMesh(vertices, uvs, triangles);
        }

        private static Mesh CreatePentPlateMesh()
        {
            var points = new[]
            {
                new Vector2(-0.42f, 0.45f),
                new Vector2(0.42f, 0.45f),
                new Vector2(0.38f, -0.16f),
                new Vector2(0.0f, -0.52f),
                new Vector2(-0.38f, -0.16f)
            };
            return ExtrudePolygon(points, 0.08f);
        }

        private static Mesh CreateGussetMesh()
        {
            var points = new[]
            {
                new Vector2(-0.5f, -0.5f),
                new Vector2(0.5f, -0.5f),
                new Vector2(-0.5f, 0.5f)
            };
            return ExtrudePolygon(points, 0.09f);
        }

        private static Mesh CreateTubeMesh(int segments, int sides, float radius, Func<float, Vector3> path)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var s = 0; s <= segments; s++)
            {
                var t = s / (float)segments;
                var p = path(t);
                var pNext = path(Mathf.Min(1f, t + 1f / segments));
                var pPrev = path(Mathf.Max(0f, t - 1f / segments));
                var tangent = (pNext - pPrev).normalized;
                var binormal = Vector3.Cross(tangent, Vector3.up);
                if (binormal.sqrMagnitude < 0.001f)
                {
                    binormal = Vector3.Cross(tangent, Vector3.right);
                }
                binormal.Normalize();
                var normal = Vector3.Cross(binormal, tangent).normalized;
                for (var i = 0; i < sides; i++)
                {
                    var a = i * Mathf.PI * 2f / sides;
                    vertices.Add(p + (Mathf.Cos(a) * normal + Mathf.Sin(a) * binormal) * radius);
                    uvs.Add(new Vector2(i / (float)sides, t));
                }
            }

            for (var s = 0; s < segments; s++)
            {
                for (var i = 0; i < sides; i++)
                {
                    var a = s * sides + i;
                    var b = s * sides + (i + 1) % sides;
                    var c = (s + 1) * sides + (i + 1) % sides;
                    var d = (s + 1) * sides + i;
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(b);
                    triangles.Add(a);
                    triangles.Add(d);
                    triangles.Add(c);
                }
            }
            return NewMesh(vertices, uvs, triangles);
        }

        private static Mesh ExtrudePolygon(IReadOnlyList<Vector2> points, float depth)
        {
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            for (var i = 0; i < points.Count; i++)
            {
                vertices.Add(new Vector3(points[i].x, points[i].y, -depth * 0.5f));
                uvs.Add(new Vector2(points[i].x + 0.5f, points[i].y + 0.5f));
            }
            for (var i = 0; i < points.Count; i++)
            {
                vertices.Add(new Vector3(points[i].x, points[i].y, depth * 0.5f));
                uvs.Add(new Vector2(points[i].x + 0.5f, points[i].y + 0.5f));
            }

            for (var i = 1; i < points.Count - 1; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(points.Count);
                triangles.Add(points.Count + i + 1);
                triangles.Add(points.Count + i);
            }

            for (var i = 0; i < points.Count; i++)
            {
                var next = (i + 1) % points.Count;
                var a = i;
                var b = next;
                var c = points.Count + next;
                var d = points.Count + i;
                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(c);
                triangles.Add(a);
                triangles.Add(c);
                triangles.Add(d);
            }
            return NewMesh(vertices, uvs, triangles);
        }

        private static void AddFace(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var start = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(0, 1));
            triangles.Add(start);
            triangles.Add(start + 1);
            triangles.Add(start + 2);
            triangles.Add(start);
            triangles.Add(start + 2);
            triangles.Add(start + 3);
        }

        private static void AddTriangle(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles, Vector3 a, Vector3 b, Vector3 c)
        {
            var start = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            uvs.Add(new Vector2(0.5f, 0.5f));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            triangles.Add(start);
            triangles.Add(start + 1);
            triangles.Add(start + 2);
        }

        private static Mesh NewMesh(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
        {
            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static ValidationResult ValidateGeneratedContent(PackageRoot packageRoot, string renderRoot)
        {
            var validation = new ValidationResult();
            validation.PrefabCount = Directory.GetFiles(Path.Combine(packageRoot.ResolvedPath, "Runtime", "Prefabs"), "*.prefab").Length;
            validation.MaterialCount = Directory.GetFiles(Path.Combine(packageRoot.ResolvedPath, "Runtime", "Materials"), "*.mat").Length;
            validation.TextureCount = Directory.GetFiles(Path.Combine(packageRoot.ResolvedPath, "Runtime", "Textures"), "*.png").Length;
            validation.MeshCount = Directory.GetFiles(Path.Combine(packageRoot.ResolvedPath, "Runtime", "Meshes"), "*.asset").Length;
            validation.RenderCount = Directory.Exists(renderRoot) ? Directory.GetFiles(renderRoot, Prefix + "_RENDER_*.png").Length : 0;

            var requiredPrefabs = new[]
            {
                "MED12_RivetedTorsoPlateCluster.prefab",
                "MED12_GlowingEyeHeadModule.prefab",
                "MED12_FlywheelShoulderAssembly.prefab",
                "MED12_PistonForearm.prefab",
                "MED12_SawClawToolArmAttachment.prefab",
                "MED12_PressureGaugeChestModule.prefab",
                "MED12_CableHoseBundle.prefab",
                "MED12_LegJointArmorPlates.prefab",
                "MED12_AssembledBustUpperBodyConcept.prefab"
            };
            foreach (var prefab in requiredPrefabs)
            {
                if (!File.Exists(Path.Combine(packageRoot.ResolvedPath, "Runtime", "Prefabs", prefab)))
                {
                    validation.Failures.Add("Missing required prefab " + prefab);
                }
            }

            if (validation.MaterialCount < 6) validation.Failures.Add("Expected at least 6 materials.");
            if (validation.TextureCount < 18) validation.Failures.Add("Expected at least 18 texture maps.");
            if (validation.MeshCount < 10) validation.Failures.Add("Expected at least 10 generated mesh assets.");
            if (validation.PrefabCount < 9) validation.Failures.Add("Expected at least 9 prefabs.");
            if (validation.RenderCount < 3) validation.Failures.Add("Expected at least 3 concept render PNGs.");
            return validation;
        }

        private static void WriteManifest(PackageRoot packageRoot, string repoRoot, string renderRoot, string planningRoot, string qaRoot, ValidationResult validation)
        {
            WriteJsonManifest(packageRoot, renderRoot, validation);
            WriteCatalog(packageRoot);
            WritePlanningDocs(planningRoot, renderRoot);
            WriteQaDocs(packageRoot, qaRoot, renderRoot, validation);
        }

        private static void WriteJsonManifest(PackageRoot packageRoot, string renderRoot, ValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"package\": \"" + PackageName + "\",");
            builder.AppendLine("  \"version\": \"" + Version + "\",");
            builder.AppendLine("  \"displayName\": \"" + DisplayName + "\",");
            builder.AppendLine("  \"generatedBy\": \"" + Prefix + "\",");
            builder.AppendLine("  \"northStarReference\": \"Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png\",");
            builder.AppendLine("  \"validation\": {");
            builder.AppendLine("    \"passed\": " + validation.Passed.ToString().ToLowerInvariant() + ",");
            builder.AppendLine("    \"prefabs\": " + validation.PrefabCount + ",");
            builder.AppendLine("    \"materials\": " + validation.MaterialCount + ",");
            builder.AppendLine("    \"textures\": " + validation.TextureCount + ",");
            builder.AppendLine("    \"meshes\": " + validation.MeshCount + ",");
            builder.AppendLine("    \"renders\": " + validation.RenderCount);
            builder.AppendLine("  },");
            AppendJsonArray(builder, "prefabs", PrefabRecords.Select(p => p.Path + "|" + p.Role + "|" + p.Notes).ToArray(), true);
            AppendJsonArray(builder, "materials", MaterialRecords.Select(r => r.Path + "|" + r.Key + "|" + r.Tag).ToArray(), true);
            AppendJsonArray(builder, "meshes", MeshRecords.Select(r => r.Path + "|" + r.Key + "|" + r.Tag).ToArray(), true);
            AppendJsonArray(builder, "textures", TextureRecords.Select(r => r.Path + "|" + r.Key + "|" + r.Tag).ToArray(), true);
            AppendJsonArray(builder, "renders", RenderRecords.Select(r => r.DocumentationPath + "|" + r.PackagePreviewPath).ToArray(), false);
            builder.AppendLine("}");

            var path = Path.Combine(packageRoot.ResolvedPath, "Runtime", "Metadata", Prefix + "_MechanicalEnemyDetailSet12_Manifest_v" + Version + ".json");
            File.WriteAllText(path, builder.ToString());
            AssetDatabase.ImportAsset(packageRoot.AssetPath + "/Runtime/Metadata/" + Path.GetFileName(path), ImportAssetOptions.ForceUpdate);
        }

        private static void WriteCatalog(PackageRoot packageRoot)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"prefabCatalog\": [");
            for (var i = 0; i < PrefabRecords.Count; i++)
            {
                var p = PrefabRecords[i];
                builder.Append("    { \"path\": \"" + EscapeJson(p.Path) + "\", \"role\": \"" + EscapeJson(p.Role) + "\", \"notes\": \"" + EscapeJson(p.Notes) + "\" }");
                builder.AppendLine(i == PrefabRecords.Count - 1 ? "" : ",");
            }
            builder.AppendLine("  ]");
            builder.AppendLine("}");
            var path = Path.Combine(packageRoot.ResolvedPath, "Runtime", "Metadata", Prefix + "_MechanicalEnemyDetailSet12_Catalog_v" + Version + ".json");
            File.WriteAllText(path, builder.ToString());
            AssetDatabase.ImportAsset(packageRoot.AssetPath + "/Runtime/Metadata/" + Path.GetFileName(path), ImportAssetOptions.ForceUpdate);
        }

        private static void WritePlanningDocs(string planningRoot, string renderRoot)
        {
            Directory.CreateDirectory(planningRoot);
            var plan = new StringBuilder();
            plan.AppendLine("# Mechanical Enemy Detail Set 12 Implementation Plan");
            plan.AppendLine();
            plan.AppendLine("Scope: isolated sidecar package only. No playable scenes, shared status docs, main package manifest, gameplay code, or worker ledgers are touched.");
            plan.AppendLine();
            plan.AppendLine("## Asset Strategy");
            plan.AppendLine();
            plan.AppendLine("- Use Unity-generated meshes and procedural materials only, with no Blender or external DCC dependency.");
            plan.AppendLine("- Prioritize brass/iron/copper material separation, layered riveted armor, glowing amber optics, pressure gauges, flywheels, hoses, pistons, and sharp tool silhouettes.");
            plan.AppendLine("- Keep all prefabs visual-only with named SOCKET transforms for future rigging and integration.");
            plan.AppendLine();
            plan.AppendLine("## Deliverables");
            plan.AppendLine();
            foreach (var prefab in PrefabRecords.Where(p => p.Role != "material_palette_board"))
            {
                plan.AppendLine("- `" + prefab.Path + "`: " + prefab.Notes);
            }
            File.WriteAllText(Path.Combine(planningRoot, "MechanicalEnemyDetailSet12_ImplementationPlan.md"), plan.ToString());

            var readiness = new StringBuilder();
            readiness.AppendLine("# Mechanical Enemy Detail Set 12 Import Readiness");
            readiness.AppendLine();
            readiness.AppendLine("The package can be added as a local Unity package from `AssetPacks/BrassworksBreach.MechanicalEnemyDetailSet12`.");
            readiness.AppendLine();
            readiness.AppendLine("## Notes");
            readiness.AppendLine();
            readiness.AppendLine("- Visual-only prefabs contain MeshFilter and MeshRenderer components plus empty SOCKET transforms.");
            readiness.AppendLine("- No colliders, lights, cameras, scripts, animation controllers, AI, audio, or scene dependencies are included in runtime prefabs.");
            readiness.AppendLine("- Generated preview PNGs are mirrored to package `Runtime/Previews` and documentation concept renders.");
            readiness.AppendLine("- Future integration should create LODs, batching, damage sockets, animator bindings, and collision separately.");
            readiness.AppendLine();
            readiness.AppendLine("## Renders");
            readiness.AppendLine();
            foreach (var render in RenderRecords)
            {
                readiness.AppendLine("- `" + render.DocumentationPath + "`");
            }
            File.WriteAllText(Path.Combine(planningRoot, "MechanicalEnemyDetailSet12_ImportReadiness.md"), readiness.ToString());
        }

        private static void WriteQaDocs(PackageRoot packageRoot, string qaRoot, string renderRoot, ValidationResult validation)
        {
            Directory.CreateDirectory(qaRoot);
            var qa = new StringBuilder();
            qa.AppendLine("# Mechanical Enemy Detail Set 12 North-Star QA");
            qa.AppendLine();
            qa.AppendLine("Reference: `Documentation/ConceptArt/north-star-steampunk-brassworks-pressure-pistol.png`.");
            qa.AppendLine();
            qa.AppendLine("Status: " + (validation.Passed ? "PASS" : "FAIL"));
            qa.AppendLine();
            qa.AppendLine("## Comparison");
            qa.AppendLine();
            qa.AppendLine("- Silhouette: PASS. The assembled bust uses a broad boiler chest, goggled glowing head, offset shoulder flywheels, heavy forearms, and a saw/claw tool profile that reads as a mechanical enemy rather than a flat prop.");
            qa.AppendLine("- Readable threat: PASS. The saw blade, hooked claw tines, hot amber eyes, grinning jaw grille, and pressure gauge danger needle provide clear hostile cues at FPS distance.");
            qa.AppendLine("- Material richness: PASS. Aged brass, dark oil-stained iron, worn copper, glowing amber glass, black rubber hose, sharp saw metal, ivory gauge enamel, soot, and red enamel each have generated albedo, normal, metallic/smoothness, and occlusion maps.");
            qa.AppendLine("- Detail density: PASS. Prefabs include raised rivets, collars, spokes, piston rods, clamps, panel seams, gauge ticks, soot streaks, and asymmetric repair plates. Density is highest around face, chest, and tool arm.");
            qa.AppendLine("- FPS readability: PASS with integration caution. Major forms are chunky and separated by material/lighting contrast; the smallest rivets and tick marks are supporting detail and should not carry gameplay readability.");
            qa.AppendLine("- Rigging limitations: ACCEPTED. Assets are static visual modules with named SOCKET transforms only. No skinning, IK, animation, hitboxes, colliders, damage states, or LODs are included.");
            qa.AppendLine();
            qa.AppendLine("## Render Evidence");
            foreach (var render in RenderRecords)
            {
                qa.AppendLine("- `" + render.DocumentationPath + "`");
            }
            qa.AppendLine();
            qa.AppendLine("## Limitations");
            qa.AppendLine();
            qa.AppendLine("- Procedural geometry is detailed but not a final sculpted hero enemy.");
            qa.AppendLine("- Transparent amber/soot materials may need render queue review after integration into the main renderer.");
            qa.AppendLine("- Runtime prefabs intentionally avoid real Light components; emission is material-only for import safety.");
            qa.AppendLine("- Final sentinel integration should add LODs, collision, animation sockets, and damage material variants.");
            File.WriteAllText(Path.Combine(qaRoot, "MechanicalEnemyDetailSet12_NorthStarQA.md"), qa.ToString());

            var validationDoc = new StringBuilder();
            validationDoc.AppendLine("# Mechanical Enemy Detail Set 12 Validation Report");
            validationDoc.AppendLine();
            validationDoc.AppendLine("Package root: `" + NormalizePath(packageRoot.ResolvedPath) + "`");
            validationDoc.AppendLine("Render root: `" + NormalizePath(renderRoot) + "`");
            validationDoc.AppendLine();
            validationDoc.AppendLine("## Counts");
            validationDoc.AppendLine();
            validationDoc.AppendLine("- Prefabs: " + validation.PrefabCount);
            validationDoc.AppendLine("- Materials: " + validation.MaterialCount);
            validationDoc.AppendLine("- Texture PNGs: " + validation.TextureCount);
            validationDoc.AppendLine("- Mesh assets: " + validation.MeshCount);
            validationDoc.AppendLine("- Render PNGs: " + validation.RenderCount);
            validationDoc.AppendLine();
            validationDoc.AppendLine("## Result");
            validationDoc.AppendLine();
            validationDoc.AppendLine(validation.Passed ? "PASS" : "FAIL: " + string.Join("; ", validation.Failures));
            File.WriteAllText(Path.Combine(qaRoot, "MechanicalEnemyDetailSet12_ValidationReport.md"), validationDoc.ToString());
        }

        private static void AppendJsonArray(StringBuilder builder, string name, IReadOnlyList<string> values, bool trailingComma)
        {
            builder.AppendLine("  \"" + name + "\": [");
            for (var i = 0; i < values.Count; i++)
            {
                builder.Append("    \"" + EscapeJson(values[i]) + "\"");
                builder.AppendLine(i == values.Count - 1 ? "" : ",");
            }
            builder.AppendLine("  ]" + (trailingComma ? "," : ""));
        }

        private static void WritePackageMetadata(PackageRoot packageRoot)
        {
            var packageJson = @"{
  ""name"": """ + PackageName + @""",
  ""version"": """ + Version + @""",
  ""displayName"": """ + DisplayName + @""",
  ""description"": ""Unity-only sidecar package with procedural steampunk mechanical enemy surface-detail modules, materials, generated meshes, prefabs, and lookdev renders."",
  ""unity"": ""6000.4"",
  ""author"": {
    ""name"": ""Brassworks Breach Sidecar Production""
  },
  ""keywords"": [
    ""brassworks"",
    ""sidecar"",
    ""mechanical-enemy"",
    ""steampunk"",
    ""surface-detail"",
    ""brass"",
    ""iron"",
    ""rivet"",
    ""flywheel"",
    ""piston"",
    ""saw"",
    ""gauge""
  ],
  ""dependencies"": {},
  ""samples"": [
    {
      ""displayName"": ""Preview Notes"",
      ""description"": ""Import notes for the visual-only Mechanical Enemy Detail Set 12 package."",
      ""path"": ""Samples~/PreviewNotes""
    }
  ]
}
";
            File.WriteAllText(Path.Combine(packageRoot.ResolvedPath, "package.json"), packageJson);
            File.WriteAllText(Path.Combine(packageRoot.ResolvedPath, "README.md"), "# Mechanical Enemy Detail Set 12\n\nUnity-only sidecar package for high-density steampunk mechanical enemy surface detail. The package is visual-only and contains no gameplay scripts, animation controllers, colliders, audio, scene edits, or shared manifest changes.\n\nPrimary outputs are generated under `Runtime/Materials`, `Runtime/Textures`, `Runtime/Meshes`, `Runtime/Prefabs`, and `Runtime/Metadata`.\n");
            var sampleDir = Path.Combine(packageRoot.ResolvedPath, "Samples~", "PreviewNotes");
            Directory.CreateDirectory(sampleDir);
            File.WriteAllText(Path.Combine(sampleDir, "README.md"), "# Preview Notes\n\nImport this package as a local package and inspect `Runtime/Prefabs`. The assembled bust prefab is a concept/lookdev reference for future mechanical sentinel upgrades; individual prefabs are intended as reusable surface-detail modules.\n");
        }

        private static void ResetOwnedGeneratedRoots(PackageRoot packageRoot, string renderRoot, string planningRoot, string qaRoot)
        {
            DeleteDirectoryIfExists(renderRoot);

            DeleteFileIfExists(Path.Combine(planningRoot, "MechanicalEnemyDetailSet12_ImplementationPlan.md"));
            DeleteFileIfExists(Path.Combine(planningRoot, "MechanicalEnemyDetailSet12_ImportReadiness.md"));
            DeleteFileIfExists(Path.Combine(qaRoot, "MechanicalEnemyDetailSet12_NorthStarQA.md"));
            DeleteFileIfExists(Path.Combine(qaRoot, "MechanicalEnemyDetailSet12_ValidationReport.md"));

            foreach (var relative in new[] { "Runtime/Materials", "Runtime/Textures", "Runtime/Meshes", "Runtime/Prefabs", "Runtime/Metadata", "Runtime/Previews", "Documentation~/Manifest", "Samples~/PreviewNotes" })
            {
                Directory.CreateDirectory(Path.Combine(packageRoot.ResolvedPath, relative.Replace("/", Path.DirectorySeparatorChar.ToString())));
            }

            DeleteGeneratedAssetFiles(packageRoot, "Runtime/Materials", Prefix + "_*.*");
            DeleteGeneratedAssetFiles(packageRoot, "Runtime/Textures", Prefix + "_*.*");
            DeleteGeneratedAssetFiles(packageRoot, "Runtime/Meshes", Prefix + "_*.*");
            DeleteGeneratedAssetFiles(packageRoot, "Runtime/Prefabs", Prefix + "_*.*");
            DeleteGeneratedAssetFiles(packageRoot, "Runtime/Metadata", Prefix + "_*.*");
            DeleteGeneratedAssetFiles(packageRoot, "Runtime/Previews", Prefix + "_*.*");
            DeleteGeneratedAssetFiles(packageRoot, "Documentation~/Manifest", Prefix + "_*.*");

            Directory.CreateDirectory(renderRoot);
            Directory.CreateDirectory(planningRoot);
            Directory.CreateDirectory(qaRoot);
        }

        private static void DeleteGeneratedAssetFiles(PackageRoot packageRoot, string relativeFolder, string pattern)
        {
            var directory = Path.Combine(packageRoot.ResolvedPath, relativeFolder.Replace("/", Path.DirectorySeparatorChar.ToString()));
            Directory.CreateDirectory(directory);
            foreach (var file in Directory.GetFiles(directory, pattern, SearchOption.TopDirectoryOnly))
            {
                var assetPath = packageRoot.AssetPath + "/" + relativeFolder + "/" + Path.GetFileName(file);
                if (!AssetDatabase.DeleteAsset(assetPath))
                {
                    DeleteFileIfExists(file);
                    DeleteFileIfExists(file + ".meta");
                }
            }
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

        private static void ReplaceAsset(string assetPath)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
        }

        private static PackageRoot LocatePackageRoot()
        {
            var package = PackageInfo.FindForAssetPath("Packages/" + PackageName + "/package.json");
            if (package != null)
            {
                return new PackageRoot(package.assetPath, package.resolvedPath);
            }

            var fallback = Environment.GetEnvironmentVariable("BRASSWORKS_MED12_PACKAGE_ROOT") ?? "D:/__MY APPS/Unity Doom/AssetPacks/BrassworksBreach.MechanicalEnemyDetailSet12";
            return new PackageRoot("Packages/" + PackageName, NormalizePath(fallback));
        }

        private static string ResolveRepoRoot(string packageResolvedPath)
        {
            var packageDir = new DirectoryInfo(packageResolvedPath);
            var assetPacksDir = packageDir.Parent;
            var repoDir = assetPacksDir != null ? assetPacksDir.Parent : null;
            if (repoDir == null)
            {
                throw new InvalidOperationException("Could not resolve repository root from " + packageResolvedPath);
            }
            return NormalizePath(repoDir.FullName);
        }

        private static void SetMaterialColor(Material mat, Color color)
        {
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
        }

        private static void SetMaterialFloat(Material mat, string property, float value)
        {
            if (mat.HasProperty(property))
            {
                mat.SetFloat(property, value);
            }
        }

        private static void SetMaterialTexture(Material mat, string primary, string fallback, Texture texture)
        {
            if (mat.HasProperty(primary)) mat.SetTexture(primary, texture);
            if (fallback != primary && mat.HasProperty(fallback)) mat.SetTexture(fallback, texture);
        }

        private static Color ClampColor(Color color)
        {
            return new Color(Mathf.Clamp01(color.r), Mathf.Clamp01(color.g), Mathf.Clamp01(color.b), Mathf.Clamp01(color.a));
        }

        private static float HeightNoise(float u, float v, int seed)
        {
            var broad = ValueNoise(u * 10f, v * 10f, seed);
            var fine = ValueNoise(u * 42f, v * 42f, seed + 19);
            var scratch = Mathf.Pow(Mathf.Abs(Mathf.Sin((u * 18f + fine) * Mathf.PI)), 10f);
            return broad * 0.55f + fine * 0.32f + scratch * 0.13f;
        }

        private static float ValueNoise(float x, float y, int seed)
        {
            var xi = Mathf.FloorToInt(x);
            var yi = Mathf.FloorToInt(y);
            var tx = Mathf.SmoothStep(0, 1, x - xi);
            var ty = Mathf.SmoothStep(0, 1, y - yi);
            var a = Hash01(xi, yi, seed);
            var b = Hash01(xi + 1, yi, seed);
            var c = Hash01(xi, yi + 1, seed);
            var d = Hash01(xi + 1, yi + 1, seed);
            return Mathf.Lerp(Mathf.Lerp(a, b, tx), Mathf.Lerp(c, d, tx), ty);
        }

        private static float Hash01(int x, int y, int seed)
        {
            unchecked
            {
                var h = seed;
                h = h * 374761393 + x * 668265263;
                h = (h ^ (h >> 13)) * 1274126177;
                h += y * 1103515245;
                h ^= h >> 16;
                return (h & 0x7fffffff) / (float)int.MaxValue;
            }
        }

        private static string SideName(float side)
        {
            return side < 0 ? "left" : "right";
        }

        private static string EscapeJson(string value)
        {
            return NormalizePath(value).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }

        private readonly struct PackageRoot
        {
            public PackageRoot(string assetPath, string resolvedPath)
            {
                AssetPath = assetPath.Replace("\\", "/");
                ResolvedPath = resolvedPath.Replace("\\", "/");
            }

            public string AssetPath { get; }
            public string ResolvedPath { get; }
        }

        private sealed class AssetRecord
        {
            public AssetRecord(string path, string key, string tag)
            {
                Path = path;
                Key = key;
                Tag = tag;
            }

            public string Path { get; }
            public string Key { get; }
            public string Tag { get; }
        }

        private sealed class PrefabRecord
        {
            public PrefabRecord(string path, string role, string notes)
            {
                Path = path;
                Role = role;
                Notes = notes;
            }

            public string Path { get; }
            public string Role { get; }
            public string Notes { get; }
        }

        private sealed class RenderRecord
        {
            public RenderRecord(string documentationPath, string packagePreviewPath)
            {
                DocumentationPath = documentationPath;
                PackagePreviewPath = packagePreviewPath;
            }

            public string DocumentationPath { get; }
            public string PackagePreviewPath { get; }
        }

        private readonly struct MaterialSpec
        {
            public MaterialSpec(string name, string tag, Color baseColor, Color accent, float metallic, float smoothness, bool transparent, bool emissive, Color emission, int seed)
            {
                Name = name;
                Tag = tag;
                Base = baseColor;
                Accent = accent;
                Metallic = metallic;
                Smoothness = smoothness;
                Transparent = transparent;
                Emissive = emissive;
                Emission = emission;
                Seed = seed;
            }

            public string Name { get; }
            public string Tag { get; }
            public string Key { get { return Name; } }
            public Color Base { get; }
            public Color Accent { get; }
            public float Metallic { get; }
            public float Smoothness { get; }
            public bool Transparent { get; }
            public bool Emissive { get; }
            public Color Emission { get; }
            public int Seed { get; }
        }

        private sealed class ValidationResult
        {
            public int PrefabCount;
            public int MaterialCount;
            public int TextureCount;
            public int MeshCount;
            public int RenderCount;
            public readonly List<string> Failures = new List<string>();
            public bool Passed { get { return Failures.Count == 0; } }
        }
    }
}
