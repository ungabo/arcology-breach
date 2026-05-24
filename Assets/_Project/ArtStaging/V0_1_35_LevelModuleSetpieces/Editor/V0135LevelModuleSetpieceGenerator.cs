#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.ArtStaging.V0135
{
    public static class V0135LevelModuleSetpieceGenerator
    {
        private const string PackageName = "V0_1_35_LevelModuleSetpieces";
        private const string Root = "Assets/_Project/ArtStaging/V0_1_35_LevelModuleSetpieces";
        private const string MaterialsPath = Root + "/Materials";
        private const string PrefabsPath = Root + "/Prefabs";
        private const string MetadataPath = Root + "/Metadata";
        private const string AssetPreviewPath = Root + "/Previews";
        private const string DocRenderPath = "Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces";

        private static readonly List<GeneratedAsset> Generated = new();
        private static readonly Dictionary<string, Material> Materials = new();

        [MenuItem("Brassworks/Art Staging/Generate v0.1.35 Level Module Setpieces")]
        public static void GenerateMenu()
        {
            GenerateAll();
        }

        public static void GenerateAll()
        {
            Directory.CreateDirectory(MaterialsPath);
            Directory.CreateDirectory(PrefabsPath);
            Directory.CreateDirectory(MetadataPath);
            Directory.CreateDirectory(AssetPreviewPath);
            Directory.CreateDirectory(DocRenderPath);

            Generated.Clear();
            Materials.Clear();

            CreateMaterials();
            CreatePrefab("SM_V0135_CorridorBay_4x6m.prefab", "corridor_kit", "4m wide x 6m long modular bay", BuildCorridorBay, new Vector3(4f, 3.1f, 6f), 138, "Visual shell collider only; child trims non-colliding.");
            CreatePrefab("SM_V0135_PressureDoor_Frame_4m.prefab", "door_vault_kit", "gear-locked pressure door frame", BuildPressureDoor, new Vector3(4.2f, 3.2f, 0.55f), 96, "Box collider on outer jamb frame only; central aperture stays clear.");
            CreatePrefab("SM_V0135_VaultDoor_Round_5m.prefab", "door_vault_kit", "round vault door and spoke lock setpiece", BuildVaultDoor, new Vector3(5f, 4.2f, 0.9f), 122, "Use one convex blocker only if placed as closed scenic door.");
            CreatePrefab("SM_V0135_PipeGallery_WallRun_6m.prefab", "pipe_valve_kit", "stacked copper and iron wall pipe gallery", BuildPipeGallery, new Vector3(6f, 2.5f, 0.55f), 176, "No collision by default; wall-mounted dressing.");
            CreatePrefab("SM_V0135_FurnaceBoiler_Alcove_5m.prefab", "furnace_boiler_alcove", "furnace alcove with boiler face and coal lip", BuildFurnaceAlcove, new Vector3(5f, 3.8f, 1.35f), 154, "Rear wall and side piers may block; front heat volume must be gameplay-owned elsewhere.");
            CreatePrefab("SM_V0135_CatwalkRail_4m.prefab", "railing_catwalk_kit", "catwalk rail with kick plate and amber caps", BuildCatwalkRail, new Vector3(4f, 1.2f, 0.22f), 86, "Simple box or capsule collision on posts/rail only after route review.");
            CreatePrefab("SM_V0135_TrimPack_FloorWallCeiling_4m.prefab", "trim_kit", "floor, wall, and ceiling trim pack", BuildTrimPack, new Vector3(4f, 2.8f, 0.45f), 104, "Non-colliding trim except flush floor strips.");
            CreatePrefab("SM_V0135_CagedGaslight_WallFixture.prefab", "lighting_fixtures", "caged amber gaslight wall fixture", BuildGaslight, new Vector3(0.55f, 0.85f, 0.42f), 62, "No collision by default; optional small trigger-free blocker if reachable.");
            CreatePrefab("SM_V0135_SetpieceDressing_Group_AllLevels.prefab", "setpiece_dressing", "five-level grouped dressing sampler", BuildDressingSampler, new Vector3(8.5f, 3.6f, 4.5f), 310, "Preview/sampler root only; integrate child families selectively.");

            WriteManifest();
            CreatePreviewSceneAndRenders();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"{PackageName}: generated {Generated.Count} prefabs, {Materials.Count} materials, manifest, and preview sheets.");
        }

        private static void CreateMaterials()
        {
            AddMaterial("M_V0135_BlackenedRivetedIron", new Color(0.055f, 0.052f, 0.048f), 0.78f, 0.18f);
            AddMaterial("M_V0135_AgedBrass", new Color(0.72f, 0.52f, 0.25f), 0.44f, 0.62f);
            AddMaterial("M_V0135_CopperPipe", new Color(0.72f, 0.31f, 0.16f), 0.52f, 0.56f);
            AddMaterial("M_V0135_OilDarkStone", new Color(0.105f, 0.10f, 0.095f), 0.86f, 0.03f);
            AddMaterial("M_V0135_SootBrick", new Color(0.20f, 0.13f, 0.105f), 0.82f, 0.02f);
            AddMaterial("M_V0135_WarmAmberGlass", new Color(1.0f, 0.58f, 0.16f), 0.22f, 0.15f, new Color(1.0f, 0.45f, 0.12f) * 1.5f);
            AddMaterial("M_V0135_FurnaceGlow", new Color(1.0f, 0.28f, 0.08f), 0.35f, 0.05f, new Color(1.0f, 0.18f, 0.04f) * 2.6f);
            AddMaterial("M_V0135_RouteGreenGlass", new Color(0.19f, 0.85f, 0.38f), 0.24f, 0.1f, new Color(0.08f, 0.7f, 0.25f) * 1.2f);
            AddMaterial("M_V0135_WarningRedEnamel", new Color(0.72f, 0.06f, 0.035f), 0.5f, 0.22f);
            AddMaterial("M_V0135_RubberGasket", new Color(0.018f, 0.017f, 0.015f), 0.9f, 0f);
            AddMaterial("M_V0135_CreamGaugeFace", new Color(0.78f, 0.68f, 0.48f), 0.58f, 0.03f);
            AddMaterial("M_V0135_HeatBlueSteel", new Color(0.20f, 0.30f, 0.42f), 0.5f, 0.35f);
        }

        private static void AddMaterial(string name, Color color, float smoothness, float metallic, Color? emission = null)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            var material = new Material(shader)
            {
                name = name,
                color = color
            };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            if (material.HasProperty("_Color")) material.SetColor("_Color", color);
            if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", smoothness);
            if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", metallic);
            if (emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                if (material.HasProperty("_EmissionColor")) material.SetColor("_EmissionColor", emission.Value);
            }

            string path = $"{MaterialsPath}/{name}.mat";
            AssetDatabase.CreateAsset(material, path);
            Materials[name] = AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        private static void CreatePrefab(string fileName, string family, string displayName, Func<GameObject> builder, Vector3 bounds, int partCount, string colliderNotes)
        {
            var root = builder();
            root.name = Path.GetFileNameWithoutExtension(fileName);
            AddMetadata(root, family, displayName, bounds, partCount, colliderNotes);
            string path = $"{PrefabsPath}/{fileName}";
            PrefabUtility.SaveAsPrefabAsset(root, path);
            UnityEngine.Object.DestroyImmediate(root);
            Generated.Add(new GeneratedAsset(path, family, displayName, bounds, partCount, colliderNotes));
        }

        private static void AddMetadata(GameObject root, string family, string displayName, Vector3 bounds, int partCount, string colliderNotes)
        {
            var note = root.AddComponent<V0135StagingMetadata>();
            note.package = PackageName;
            note.family = family;
            note.displayName = displayName;
            note.version = "v0.1.35";
            note.approximateBoundsMeters = bounds;
            note.visualPartCount = partCount;
            note.colliderGuidance = colliderNotes;
            note.integrationGuidance = "Visual-first Unity proxy; keep child details non-colliding until route smoke passes.";
        }

        private static GameObject BuildCorridorBay()
        {
            var root = NewRoot("CorridorBay");
            Cube(root, "floor_oil_dark_stone_4x6", new Vector3(0, -0.05f, 0), new Vector3(4f, 0.1f, 6f), "M_V0135_OilDarkStone");
            Cube(root, "ceiling_pipe_channel_plate", new Vector3(0, 3.05f, 0), new Vector3(4f, 0.12f, 6f), "M_V0135_BlackenedRivetedIron");
            Cube(root, "left_soot_brick_wall", new Vector3(-2.05f, 1.45f, 0), new Vector3(0.12f, 2.9f, 6f), "M_V0135_SootBrick");
            Cube(root, "right_soot_brick_wall", new Vector3(2.05f, 1.45f, 0), new Vector3(0.12f, 2.9f, 6f), "M_V0135_SootBrick");
            for (int z = -2; z <= 2; z += 2)
            {
                Cube(root, "wall_riveted_panel_L_" + z, new Vector3(-1.98f, 1.45f, z), new Vector3(0.08f, 1.6f, 1.25f), "M_V0135_BlackenedRivetedIron");
                Cube(root, "wall_riveted_panel_R_" + z, new Vector3(1.98f, 1.45f, z), new Vector3(0.08f, 1.6f, 1.25f), "M_V0135_BlackenedRivetedIron");
                Cyl(root, "ceiling_copper_pipe_" + z, new Vector3(-0.75f, 2.87f, z), new Vector3(90, 0, 0), new Vector3(0.12f, 1.9f, 0.12f), "M_V0135_CopperPipe");
                Cyl(root, "ceiling_iron_pipe_" + z, new Vector3(0.8f, 2.87f, z), new Vector3(90, 0, 0), new Vector3(0.14f, 1.8f, 0.14f), "M_V0135_BlackenedRivetedIron");
            }
            AddRivetRows(root, new Vector3(-1.96f, 2.35f, 0), 6, true);
            AddRivetRows(root, new Vector3(1.96f, 2.35f, 0), 6, true);
            return root;
        }

        private static GameObject BuildPressureDoor()
        {
            var root = NewRoot("PressureDoor");
            Cube(root, "left_jamb_black_iron", new Vector3(-2.05f, 1.45f, 0), new Vector3(0.35f, 2.9f, 0.42f), "M_V0135_BlackenedRivetedIron");
            Cube(root, "right_jamb_black_iron", new Vector3(2.05f, 1.45f, 0), new Vector3(0.35f, 2.9f, 0.42f), "M_V0135_BlackenedRivetedIron");
            Cube(root, "top_pressure_lintel", new Vector3(0, 2.95f, 0), new Vector3(4.25f, 0.35f, 0.5f), "M_V0135_BlackenedRivetedIron");
            Cube(root, "bottom_sill_rubber_gasket", new Vector3(0, 0.05f, 0), new Vector3(4f, 0.1f, 0.45f), "M_V0135_RubberGasket");
            Cyl(root, "left_vertical_piston", new Vector3(-1.62f, 1.5f, -0.22f), Vector3.zero, new Vector3(0.16f, 2.3f, 0.16f), "M_V0135_CopperPipe");
            Cyl(root, "right_vertical_piston", new Vector3(1.62f, 1.5f, -0.22f), Vector3.zero, new Vector3(0.16f, 2.3f, 0.16f), "M_V0135_CopperPipe");
            Cyl(root, "top_gear_lock", new Vector3(0, 2.58f, -0.32f), new Vector3(90, 0, 0), new Vector3(0.55f, 0.12f, 0.55f), "M_V0135_AgedBrass");
            Cyl(root, "pressure_gauge_face", new Vector3(0.72f, 2.53f, -0.36f), new Vector3(90, 0, 0), new Vector3(0.23f, 0.04f, 0.23f), "M_V0135_CreamGaugeFace");
            Cube(root, "amber_state_lens", new Vector3(-0.72f, 2.53f, -0.36f), new Vector3(0.45f, 0.16f, 0.08f), "M_V0135_WarmAmberGlass");
            return root;
        }

        private static GameObject BuildVaultDoor()
        {
            var root = NewRoot("VaultDoor");
            Cyl(root, "round_vault_slab", new Vector3(0, 2.05f, 0), new Vector3(90, 0, 0), new Vector3(2.05f, 0.32f, 2.05f), "M_V0135_BlackenedRivetedIron");
            Cyl(root, "brass_outer_ring", new Vector3(0, 2.05f, -0.2f), new Vector3(90, 0, 0), new Vector3(2.25f, 0.08f, 2.25f), "M_V0135_AgedBrass");
            Cyl(root, "central_hub", new Vector3(0, 2.05f, -0.35f), new Vector3(90, 0, 0), new Vector3(0.38f, 0.22f, 0.38f), "M_V0135_AgedBrass");
            for (int i = 0; i < 8; i++)
            {
                float a = i * 45f;
                var spoke = Cube(root, "locking_spoke_" + i, new Vector3(0, 2.05f, -0.38f), new Vector3(1.55f, 0.08f, 0.08f), "M_V0135_CopperPipe");
                spoke.transform.localRotation = Quaternion.Euler(0, 0, a);
            }
            Cube(root, "floor_anchor_left", new Vector3(-1.9f, 0.3f, 0), new Vector3(0.5f, 0.6f, 0.65f), "M_V0135_OilDarkStone");
            Cube(root, "floor_anchor_right", new Vector3(1.9f, 0.3f, 0), new Vector3(0.5f, 0.6f, 0.65f), "M_V0135_OilDarkStone");
            Cube(root, "red_locked_status_tab", new Vector3(1.25f, 3.45f, -0.42f), new Vector3(0.52f, 0.14f, 0.08f), "M_V0135_WarningRedEnamel");
            return root;
        }

        private static GameObject BuildPipeGallery()
        {
            var root = NewRoot("PipeGallery");
            Cube(root, "backing_rivet_plate", new Vector3(0, 1.55f, 0.08f), new Vector3(6f, 2.5f, 0.12f), "M_V0135_BlackenedRivetedIron");
            for (int i = 0; i < 4; i++)
            {
                float y = 0.65f + i * 0.48f;
                string mat = i % 2 == 0 ? "M_V0135_CopperPipe" : "M_V0135_BlackenedRivetedIron";
                Cyl(root, "horizontal_pipe_run_" + i, new Vector3(0, y, -0.05f), new Vector3(0, 0, 90), new Vector3(0.10f + i * 0.018f, 5.8f, 0.10f + i * 0.018f), mat);
                for (int c = -2; c <= 2; c += 2)
                {
                    Cube(root, "pipe_clamp_" + i + "_" + c, new Vector3(c, y, -0.18f), new Vector3(0.16f, 0.32f, 0.16f), "M_V0135_AgedBrass");
                }
            }
            Cyl(root, "valve_wheel_main", new Vector3(1.75f, 1.62f, -0.33f), new Vector3(90, 0, 0), new Vector3(0.42f, 0.08f, 0.42f), "M_V0135_AgedBrass");
            Cyl(root, "pressure_gauge_top", new Vector3(-1.7f, 2.28f, -0.28f), new Vector3(90, 0, 0), new Vector3(0.24f, 0.05f, 0.24f), "M_V0135_CreamGaugeFace");
            Cube(root, "green_restored_tab", new Vector3(-2.45f, 0.35f, -0.14f), new Vector3(0.65f, 0.13f, 0.06f), "M_V0135_RouteGreenGlass");
            return root;
        }

        private static GameObject BuildFurnaceAlcove()
        {
            var root = NewRoot("FurnaceAlcove");
            Cube(root, "rear_soot_brick", new Vector3(0, 1.8f, 0.45f), new Vector3(5f, 3.6f, 0.18f), "M_V0135_SootBrick");
            Cube(root, "left_pier", new Vector3(-2.35f, 1.8f, 0), new Vector3(0.35f, 3.6f, 1.2f), "M_V0135_BlackenedRivetedIron");
            Cube(root, "right_pier", new Vector3(2.35f, 1.8f, 0), new Vector3(0.35f, 3.6f, 1.2f), "M_V0135_BlackenedRivetedIron");
            Cube(root, "coal_lip", new Vector3(0, 0.24f, -0.35f), new Vector3(4.2f, 0.48f, 0.45f), "M_V0135_OilDarkStone");
            Cube(root, "furnace_mouth_glow", new Vector3(0, 1.25f, -0.18f), new Vector3(2.75f, 1.35f, 0.16f), "M_V0135_FurnaceGlow");
            Cyl(root, "boiler_tank_left", new Vector3(-1.42f, 2.65f, -0.15f), new Vector3(0, 0, 90), new Vector3(0.3f, 1.25f, 0.3f), "M_V0135_HeatBlueSteel");
            Cyl(root, "boiler_tank_right", new Vector3(1.42f, 2.65f, -0.15f), new Vector3(0, 0, 90), new Vector3(0.3f, 1.25f, 0.3f), "M_V0135_CopperPipe");
            Cube(root, "warning_red_heat_band", new Vector3(0, 0.83f, -0.55f), new Vector3(3.5f, 0.12f, 0.1f), "M_V0135_WarningRedEnamel");
            AddRivetRows(root, new Vector3(0, 3.35f, -0.16f), 8, false);
            return root;
        }

        private static GameObject BuildCatwalkRail()
        {
            var root = NewRoot("CatwalkRail");
            Cube(root, "kick_plate_black_iron", new Vector3(0, 0.2f, 0), new Vector3(4f, 0.4f, 0.1f), "M_V0135_BlackenedRivetedIron");
            for (int i = 0; i < 5; i++)
            {
                float x = -1.9f + i * 0.95f;
                Cyl(root, "vertical_post_" + i, new Vector3(x, 0.72f, 0), Vector3.zero, new Vector3(0.055f, 1.08f, 0.055f), "M_V0135_AgedBrass");
                Cube(root, "post_foot_" + i, new Vector3(x, 0.05f, 0), new Vector3(0.28f, 0.1f, 0.22f), "M_V0135_BlackenedRivetedIron");
            }
            Cyl(root, "top_handrail", new Vector3(0, 1.18f, 0), new Vector3(0, 0, 90), new Vector3(0.065f, 4.1f, 0.065f), "M_V0135_CopperPipe");
            Cyl(root, "middle_handrail", new Vector3(0, 0.78f, 0), new Vector3(0, 0, 90), new Vector3(0.05f, 4.1f, 0.05f), "M_V0135_AgedBrass");
            Cube(root, "amber_route_cap", new Vector3(1.95f, 1.18f, -0.06f), new Vector3(0.18f, 0.1f, 0.08f), "M_V0135_WarmAmberGlass");
            return root;
        }

        private static GameObject BuildTrimPack()
        {
            var root = NewRoot("TrimPack");
            Cube(root, "floor_rivet_strip", new Vector3(0, 0.03f, 0), new Vector3(4f, 0.06f, 0.14f), "M_V0135_BlackenedRivetedIron");
            Cube(root, "wall_brass_waist_rail", new Vector3(0, 1.1f, 0), new Vector3(4f, 0.12f, 0.12f), "M_V0135_AgedBrass");
            Cube(root, "wall_rubber_gasket_strip", new Vector3(0, 1.52f, 0), new Vector3(4f, 0.08f, 0.08f), "M_V0135_RubberGasket");
            Cube(root, "ceiling_pipe_channel_trim", new Vector3(0, 2.65f, 0), new Vector3(4f, 0.18f, 0.32f), "M_V0135_BlackenedRivetedIron");
            for (int i = 0; i < 12; i++)
            {
                float x = -1.85f + i * 0.335f;
                Cyl(root, "floor_rivet_" + i, new Vector3(x, 0.095f, -0.02f), new Vector3(90, 0, 0), new Vector3(0.045f, 0.025f, 0.045f), "M_V0135_AgedBrass");
                Cyl(root, "ceiling_bolt_" + i, new Vector3(x, 2.55f, -0.18f), new Vector3(90, 0, 0), new Vector3(0.035f, 0.02f, 0.035f), "M_V0135_AgedBrass");
            }
            return root;
        }

        private static GameObject BuildGaslight()
        {
            var root = NewRoot("CagedGaslight");
            Cube(root, "wall_mount_plate", new Vector3(0, 0.45f, 0.08f), new Vector3(0.48f, 0.82f, 0.08f), "M_V0135_BlackenedRivetedIron");
            Cyl(root, "amber_glass_tube", new Vector3(0, 0.43f, -0.08f), Vector3.zero, new Vector3(0.18f, 0.58f, 0.18f), "M_V0135_WarmAmberGlass");
            for (int i = 0; i < 4; i++)
            {
                float x = i < 2 ? -0.23f : 0.23f;
                float z = i % 2 == 0 ? -0.08f : -0.28f;
                Cyl(root, "cage_bar_" + i, new Vector3(x, 0.43f, z), Vector3.zero, new Vector3(0.025f, 0.68f, 0.025f), "M_V0135_AgedBrass");
            }
            Cyl(root, "gas_feed_pipe", new Vector3(0, 0.05f, 0.02f), new Vector3(90, 0, 0), new Vector3(0.045f, 0.55f, 0.045f), "M_V0135_CopperPipe");
            var light = new GameObject("preview_point_light");
            light.transform.SetParent(root.transform, false);
            light.transform.localPosition = new Vector3(0, 0.42f, -0.18f);
            var point = light.AddComponent<Light>();
            point.type = LightType.Point;
            point.color = new Color(1f, 0.55f, 0.2f);
            point.intensity = 1.2f;
            point.range = 3f;
            return root;
        }

        private static GameObject BuildDressingSampler()
        {
            var root = NewRoot("SetpieceDressingSampler");
            Cube(root, "L01_pressure_gate_crown_proxy", new Vector3(-3.3f, 2.6f, 0), new Vector3(1.6f, 0.45f, 0.2f), "M_V0135_AgedBrass");
            Cube(root, "L02_valve_gallery_backplate_proxy", new Vector3(-1.65f, 1.6f, 0), new Vector3(1.8f, 1.4f, 0.16f), "M_V0135_BlackenedRivetedIron");
            Cyl(root, "L03_bellows_node_pressure_ring_proxy", new Vector3(0, 0.25f, 0), new Vector3(90, 0, 0), new Vector3(0.9f, 0.08f, 0.9f), "M_V0135_WarningRedEnamel");
            Cube(root, "L04_furnace_row_warning_band_proxy", new Vector3(1.85f, 0.9f, 0), new Vector3(1.8f, 0.16f, 0.12f), "M_V0135_FurnaceGlow");
            Cube(root, "L05_warden_arena_high_wall_feed_proxy", new Vector3(3.4f, 2.0f, 0), new Vector3(1.5f, 2.6f, 0.16f), "M_V0135_HeatBlueSteel");
            for (int i = 0; i < 5; i++)
            {
                Cyl(root, "grouped_gauge_" + i, new Vector3(-3.6f + i * 1.8f, 1.35f, -0.18f), new Vector3(90, 0, 0), new Vector3(0.18f, 0.035f, 0.18f), "M_V0135_CreamGaugeFace");
            }
            return root;
        }

        private static GameObject NewRoot(string name)
        {
            var root = new GameObject(name);
            root.transform.position = Vector3.zero;
            return root;
        }

        private static GameObject Cube(GameObject parent, string name, Vector3 position, Vector3 scale, string materialName)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = position;
            go.transform.localScale = scale;
            ApplyMaterial(go, materialName);
            return go;
        }

        private static GameObject Cyl(GameObject parent, string name, Vector3 position, Vector3 euler, Vector3 scale, string materialName)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = name;
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = position;
            go.transform.localEulerAngles = euler;
            go.transform.localScale = scale;
            ApplyMaterial(go, materialName);
            return go;
        }

        private static void ApplyMaterial(GameObject go, string materialName)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer != null && Materials.TryGetValue(materialName, out var mat))
            {
                renderer.sharedMaterial = mat;
            }
        }

        private static void AddRivetRows(GameObject root, Vector3 center, int count, bool verticalWall)
        {
            for (int i = 0; i < count; i++)
            {
                float offset = (i - (count - 1) * 0.5f) * 0.42f;
                Vector3 pos = verticalWall ? center + new Vector3(0, 0, offset) : center + new Vector3(offset, 0, 0);
                Cyl(root, "aged_brass_rivet_" + root.transform.childCount + "_" + i, pos, new Vector3(90, 0, 0), new Vector3(0.045f, 0.025f, 0.045f), "M_V0135_AgedBrass");
            }
        }

        private static void WriteManifest()
        {
            string materialList = string.Join(",\n", Map(Materials.Keys, key => $"    \"{key}\""));
            string assetList = string.Join(",\n", Map(Generated, asset =>
                "    {\n" +
                $"      \"path\": \"{asset.Path}\",\n" +
                $"      \"family\": \"{asset.Family}\",\n" +
                $"      \"name\": \"{asset.DisplayName}\",\n" +
                $"      \"bounds_m\": {{ \"x\": {asset.Bounds.x:0.##}, \"y\": {asset.Bounds.y:0.##}, \"z\": {asset.Bounds.z:0.##} }},\n" +
                $"      \"visual_part_count\": {asset.PartCount},\n" +
                $"      \"collider_guidance\": \"{asset.ColliderNotes}\"\n" +
                "    }"));

            string json = "{\n" +
                "  \"package\": \"V0_1_35_LevelModuleSetpieces\",\n" +
                "  \"project\": \"Brassworks Breach\",\n" +
                "  \"version\": \"v0.1.35\",\n" +
                $"  \"generated_at_unity_local\": \"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\",\n" +
                "  \"unity_only\": true,\n" +
                "  \"external_dcc_used\": false,\n" +
                "  \"owned_scope\": {\n" +
                "    \"asset_staging\": \"Assets/_Project/ArtStaging/V0_1_35_LevelModuleSetpieces/\",\n" +
                "    \"asset_production_docs\": \"Documentation/AssetProduction/V0_1_35_LevelModuleSetpieces/\",\n" +
                "    \"concept_renders\": \"Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces/\",\n" +
                "    \"placement_docs\": \"Documentation/Planning/V0_1_35_LevelSetpiecePlacements/\"\n" +
                "  },\n" +
                "  \"materials\": [\n" + materialList + "\n  ],\n" +
                "  \"assets\": [\n" + assetList + "\n  ],\n" +
                "  \"performance_budget\": {\n" +
                "    \"target\": \"low/mid Windows PC\",\n" +
                "    \"recommended_static_batching\": true,\n" +
                "    \"recommended_gpu_instancing\": \"rivet, bolt, gauge, pipe clamp repeats\",\n" +
                "    \"lod_policy\": \"LOD0 for close hero pieces, LOD1 at 18m with 45-60 percent child detail, LOD2 at 35m as merged silhouette blocks\",\n" +
                "    \"collider_policy\": \"visual-only by default; add simple primitive colliders only to gameplay-relevant blockers after route validation\"\n" +
                "  },\n" +
                "  \"preview_outputs\": [\n" +
                "    \"Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces/CONTACTSHEET_V0135_LevelModuleSetpieces_UnityProof.png\",\n" +
                "    \"Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces/RENDER_V0135_CorridorDoorPipeGallery_UnityProof.png\",\n" +
                "    \"Documentation/ConceptRenders/V0_1_35_LevelModuleSetpieces/RENDER_V0135_FurnaceCatwalkTrim_UnityProof.png\"\n" +
                "  ]\n" +
                "}\n";
            File.WriteAllText($"{MetadataPath}/V0_1_35_LevelModuleSetpieces_Manifest.json", json);
        }

        private static IEnumerable<string> Map<T>(IEnumerable<T> source, Func<T, string> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        private static void CreatePreviewSceneAndRenders()
        {
            var sceneRoot = NewRoot("V0135_preview_render_scene");
            var cameraObj = new GameObject("preview_camera");
            var camera = cameraObj.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.045f, 0.042f, 0.038f);
            camera.fieldOfView = 38f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 80f;

            var key = new GameObject("warm_key_light").AddComponent<Light>();
            key.type = LightType.Directional;
            key.transform.rotation = Quaternion.Euler(42, -38, 0);
            key.intensity = 2.2f;
            key.color = new Color(1f, 0.78f, 0.52f);

            var fill = new GameObject("cool_fill_light").AddComponent<Light>();
            fill.type = LightType.Directional;
            fill.transform.rotation = Quaternion.Euler(18, 138, 0);
            fill.intensity = 0.9f;
            fill.color = new Color(0.45f, 0.58f, 0.7f);

            RenderLayout(sceneRoot, camera, "RENDER_V0135_CorridorDoorPipeGallery_UnityProof.png", new[]
            {
                "SM_V0135_CorridorBay_4x6m.prefab",
                "SM_V0135_PressureDoor_Frame_4m.prefab",
                "SM_V0135_PipeGallery_WallRun_6m.prefab",
                "SM_V0135_CagedGaslight_WallFixture.prefab"
            }, new Vector3[] { new(-4.5f, 0, 0), new(0, 0, 0), new(4.6f, 0, 0), new(7.4f, 1.3f, -0.5f) }, new Vector3(0, 2.3f, -10.5f), new Vector3(0, 1.55f, 0));

            RenderLayout(sceneRoot, camera, "RENDER_V0135_FurnaceCatwalkTrim_UnityProof.png", new[]
            {
                "SM_V0135_FurnaceBoiler_Alcove_5m.prefab",
                "SM_V0135_CatwalkRail_4m.prefab",
                "SM_V0135_TrimPack_FloorWallCeiling_4m.prefab",
                "SM_V0135_VaultDoor_Round_5m.prefab"
            }, new Vector3[] { new(-4f, 0, 0), new(0.5f, 0, -0.6f), new(3.8f, 0, 0), new(7.5f, 0, 0) }, new Vector3(1.7f, 2.3f, -11f), new Vector3(1.7f, 1.55f, 0));

            RenderLayout(sceneRoot, camera, "CONTACTSHEET_V0135_LevelModuleSetpieces_UnityProof.png", new[]
            {
                "SM_V0135_CorridorBay_4x6m.prefab",
                "SM_V0135_PressureDoor_Frame_4m.prefab",
                "SM_V0135_VaultDoor_Round_5m.prefab",
                "SM_V0135_PipeGallery_WallRun_6m.prefab",
                "SM_V0135_FurnaceBoiler_Alcove_5m.prefab",
                "SM_V0135_CatwalkRail_4m.prefab",
                "SM_V0135_TrimPack_FloorWallCeiling_4m.prefab",
                "SM_V0135_CagedGaslight_WallFixture.prefab",
                "SM_V0135_SetpieceDressing_Group_AllLevels.prefab"
            }, ContactPositions(9), new Vector3(0, 6.2f, -18f), new Vector3(0, 1.3f, 0), 2200, 1400);

            UnityEngine.Object.DestroyImmediate(sceneRoot);
            UnityEngine.Object.DestroyImmediate(cameraObj);
            UnityEngine.Object.DestroyImmediate(key.gameObject);
            UnityEngine.Object.DestroyImmediate(fill.gameObject);
        }

        private static Vector3[] ContactPositions(int count)
        {
            var positions = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                int row = i / 3;
                int col = i % 3;
                positions[i] = new Vector3((col - 1) * 7.2f, 0, row * 4.6f);
            }
            return positions;
        }

        private static void RenderLayout(GameObject sceneRoot, Camera camera, string fileName, string[] prefabNames, Vector3[] positions, Vector3 cameraPosition, Vector3 lookAt, int width = 1600, int height = 1000)
        {
            var spawned = new List<GameObject>();
            for (int i = 0; i < prefabNames.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{PrefabsPath}/{prefabNames[i]}");
                if (prefab == null) continue;
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.transform.SetParent(sceneRoot.transform, false);
                instance.transform.localPosition = positions[i];
                spawned.Add(instance);
            }

            camera.transform.position = cameraPosition;
            camera.transform.LookAt(lookAt);
            var rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture.active = rt;
            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();
            File.WriteAllBytes($"{DocRenderPath}/{fileName}", texture.EncodeToPNG());
            File.WriteAllBytes($"{AssetPreviewPath}/{fileName}", texture.EncodeToPNG());
            camera.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(texture);
            rt.Release();
            UnityEngine.Object.DestroyImmediate(rt);
            foreach (var go in spawned)
            {
                UnityEngine.Object.DestroyImmediate(go);
            }
        }

        private readonly struct GeneratedAsset
        {
            public readonly string Path;
            public readonly string Family;
            public readonly string DisplayName;
            public readonly Vector3 Bounds;
            public readonly int PartCount;
            public readonly string ColliderNotes;

            public GeneratedAsset(string path, string family, string displayName, Vector3 bounds, int partCount, string colliderNotes)
            {
                Path = path;
                Family = family;
                DisplayName = displayName;
                Bounds = bounds;
                PartCount = partCount;
                ColliderNotes = colliderNotes;
            }
        }
    }

}
#endif
