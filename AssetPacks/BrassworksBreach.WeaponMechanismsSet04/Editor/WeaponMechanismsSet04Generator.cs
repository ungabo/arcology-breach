#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrassworksBreach.Sidecars.WeaponMechanismsSet04
{
    public static class WeaponMechanismsSet04Generator
    {
        private const string Version = "0.1.45";
        private const string BuildId = "p001";
        private const string PackId = "WMS04";
        private const string PackageName = "com.brassworks.sidecar.weapon-mechanisms-set04";
        private const string MenuRoot = "Brassworks/Sidecars/Weapon Mechanisms Set 04 v0.1.45/";
        private const string RenderOutputRelativePath = "Documentation/ConceptRenders/V0_1_45_WeaponMechanismsSet04";
        private const string ProductionOutputRelativePath = "Documentation/AssetProduction/V0_1_45_WeaponMechanismsSet04";
        private const string ManifestFileName = "WMS04_WeaponMechanismsSet04_Manifest_v0.1.45-p001.json";
        private const string CatalogFileName = "WMS04_RuntimeCatalog_v0.1.45.json";

        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>(StringComparer.Ordinal);
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>(StringComparer.Ordinal);

        [MenuItem(MenuRoot + "Generate Package Assets")]
        public static void GeneratePackageAssets()
        {
            Materials.Clear();
            Meshes.Clear();
            EnsureFolders();
            CreateMaterials();
            CreateMeshes();

            foreach (MechanismSpec spec in Specs())
            {
                CreatePrefab(spec);
            }

            WriteMetadata("generated_by_unity_sidecar_batchmode_v0.1.45");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("WMS04_GENERATE_PASS v0.1.45 prefabs=" + Specs().Length + " materials=" + Materials.Count + " meshes=" + Meshes.Count);
        }

        [MenuItem(MenuRoot + "Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            GeneratePackageAssets();

            string outputRoot = ResolveRenderOutputRoot();
            Directory.CreateDirectory(outputRoot);
            foreach (string oldPreview in Directory.GetFiles(outputRoot, "WMS04_*.png"))
            {
                File.Delete(oldPreview);
            }

            RenderFamily("pressure_pistol_coils", "Pressure pistol coil assemblies", Family("pressure-pistol-coils"), new Vector3(0f, 2.6f, -6.2f), new Vector3(15f, -10f, 0f), outputRoot);
            RenderFamily("gauge_dial_clusters", "Readable gauge and dial clusters", Family("gauge-dial-clusters"), new Vector3(0f, 2.2f, -5.2f), new Vector3(8f, 0f, 0f), outputRoot);
            RenderFamily("wood_leather_grips", "Wood and leather grip assemblies", Family("wood-leather-grips"), new Vector3(0f, 2.5f, -5.7f), new Vector3(12f, -8f, 0f), outputRoot);
            RenderFamily("receiver_plates", "Brass receiver plates", Family("brass-receiver-plates"), new Vector3(0f, 2.2f, -5.8f), new Vector3(10f, -6f, 0f), outputRoot);
            RenderFamily("muzzle_tanks_valves", "Muzzle crowns, tanks, and valve levers", Combine(Family("iron-muzzle-crowns"), Family("pressure-tanks"), Family("valve-levers")), new Vector3(0f, 2.6f, -7.2f), new Vector3(13f, -10f, 0f), outputRoot);
            RenderFamily("ammo_scatter_rails", "Ammo cylinders, scattergun chambers, and bolt rails", Combine(Family("ammo-cylinder-mechanisms"), Family("scattergun-pressure-chambers"), Family("bolt-thrower-rails")), new Vector3(0f, 2.7f, -7.4f), new Vector3(14f, -10f, 0f), outputRoot);
            RenderFamily("glove_silhouette_pieces", "Gloved hand silhouette pieces", Family("gloved-hand-silhouettes"), new Vector3(0f, 2.2f, -5.5f), new Vector3(11f, -4f, 0f), outputRoot);
            RenderFamily("material_swatch_prefabs", "Material swatch prefabs", Family("material-swatch-prefabs"), new Vector3(0f, 2.4f, -5.4f), new Vector3(9f, 0f, 0f), outputRoot);
            RenderFamily("all_components_contact_sheet_a", "All weapon mechanism prefabs A", FirstHalf(Specs()), new Vector3(0f, 8.8f, -19f), new Vector3(29f, 0f, 0f), outputRoot);
            RenderFamily("all_components_contact_sheet_b", "All weapon mechanism prefabs B", SecondHalf(Specs()), new Vector3(0f, 8.8f, -19f), new Vector3(29f, 0f, 0f), outputRoot);
            RenderMaterialSwatch(outputRoot);

            WriteMetadata("generated_and_preview_rendered_by_unity_sidecar_batchmode_v0.1.45");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("WMS04_PREVIEW_PASS v0.1.45 output=" + outputRoot + " files=" + Directory.GetFiles(outputRoot, "WMS04_*.png").Length);
        }

        public static void GenerateAllAndRenderPreview()
        {
            RenderPreviewPngs();
        }

        public static void GenerateValidateAndQuit()
        {
            int errors = 0;
            try
            {
                RenderPreviewPngs();
                errors = WriteUnityValidationReport();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                errors = 1;
            }

            EditorApplication.Exit(errors == 0 ? 0 : 1);
        }

        private static MechanismSpec[] Specs()
        {
            return new[]
            {
                new MechanismSpec("WMS04_PressurePistolCoil_TripleAmber_A.prefab", "pressure-pistol-coils", "triple amber pressure pistol coil assembly with readable barrel axis", new Vector3(1.15f, 0.55f, 1.65f), BuildCoilA),
                new MechanismSpec("WMS04_PressurePistolCoil_StackedCopper_B.prefab", "pressure-pistol-coils", "stacked copper coil sleeve with split brass clamp bands", new Vector3(1.05f, 0.65f, 1.55f), BuildCoilB),
                new MechanismSpec("WMS04_PressurePistolCoil_ExposedSpring_C.prefab", "pressure-pistol-coils", "exposed blued spring coil with black iron barrel core", new Vector3(0.85f, 0.55f, 1.75f), BuildCoilC),
                new MechanismSpec("WMS04_PressurePistolCoil_DualGlow_D.prefab", "pressure-pistol-coils", "dual glowing pressure coil module for oversized hand cannon profile", new Vector3(1.35f, 0.75f, 1.8f), BuildCoilD),
                new MechanismSpec("WMS04_GaugeCluster_TripleIvory_A.prefab", "gauge-dial-clusters", "three ivory gauge dials with brass bezels and redline needles", new Vector3(1.25f, 0.9f, 0.18f), BuildGaugeA),
                new MechanismSpec("WMS04_GaugeCluster_DualStack_B.prefab", "gauge-dial-clusters", "dual stacked pressure dial cluster with green glass pilots", new Vector3(0.72f, 1.15f, 0.2f), BuildGaugeB),
                new MechanismSpec("WMS04_GaugeCluster_SidecarMini_C.prefab", "gauge-dial-clusters", "compact side mounted gauge cluster for viewmodel readability tests", new Vector3(0.86f, 0.55f, 0.2f), BuildGaugeC),
                new MechanismSpec("WMS04_GripAssembly_WalnutLeather_A.prefab", "wood-leather-grips", "varnished walnut pistol grip with wrapped leather and brass pommel", new Vector3(0.48f, 1.15f, 0.45f), BuildGripA),
                new MechanismSpec("WMS04_GripAssembly_OiledStock_B.prefab", "wood-leather-grips", "oiled wood stock grip with dark glove clearance silhouette", new Vector3(0.55f, 1.25f, 0.65f), BuildGripB),
                new MechanismSpec("WMS04_GripAssembly_RivetPalm_C.prefab", "wood-leather-grips", "rivet palm grip with leather bands and trigger guard socket", new Vector3(0.72f, 1.0f, 0.55f), BuildGripC),
                new MechanismSpec("WMS04_ReceiverPlate_BrassLattice_A.prefab", "brass-receiver-plates", "aged brass receiver side plate with nested rivet lattice", new Vector3(1.35f, 0.75f, 0.12f), BuildReceiverA),
                new MechanismSpec("WMS04_ReceiverPlate_InspectionHatch_B.prefab", "brass-receiver-plates", "inspection hatch receiver plate with copper hinge barrels", new Vector3(1.25f, 0.85f, 0.16f), BuildReceiverB),
                new MechanismSpec("WMS04_ReceiverPlate_SerialPlaque_C.prefab", "brass-receiver-plates", "serial plaque receiver plate with edge-worn gunmetal backing", new Vector3(1.55f, 0.62f, 0.14f), BuildReceiverC),
                new MechanismSpec("WMS04_MuzzleCrown_IronFinned_A.prefab", "iron-muzzle-crowns", "blackened iron finned muzzle crown with brass retaining bolts", new Vector3(0.72f, 0.72f, 0.8f), BuildMuzzleA),
                new MechanismSpec("WMS04_MuzzleCrown_CogBrake_B.prefab", "iron-muzzle-crowns", "cog brake muzzle crown with soot darkened teeth", new Vector3(0.82f, 0.82f, 0.62f), BuildMuzzleB),
                new MechanismSpec("WMS04_PressureTank_CigarBrass_A.prefab", "pressure-tanks", "cigar pressure tank with green glass sight window", new Vector3(0.62f, 0.62f, 1.35f), BuildTankA),
                new MechanismSpec("WMS04_PressureTank_TwinUnderbarrel_B.prefab", "pressure-tanks", "twin underbarrel pressure tanks with amber fill caps", new Vector3(0.95f, 0.48f, 1.35f), BuildTankB),
                new MechanismSpec("WMS04_ValveLever_RedSafety_A.prefab", "valve-levers", "red safety valve lever with brass hub and black iron stem", new Vector3(0.65f, 0.65f, 0.22f), BuildValveA),
                new MechanismSpec("WMS04_ValveLever_CrankWheel_B.prefab", "valve-levers", "small crank wheel valve lever for reload and charge silhouettes", new Vector3(0.7f, 0.7f, 0.26f), BuildValveB),
                new MechanismSpec("WMS04_AmmoCylinder_SixCell_A.prefab", "ammo-cylinder-mechanisms", "six cell pressure ammo cylinder with brass end caps", new Vector3(0.72f, 0.72f, 0.92f), BuildAmmoA),
                new MechanismSpec("WMS04_AmmoCylinder_EightCell_B.prefab", "ammo-cylinder-mechanisms", "eight cell rotary mechanism with exposed copper chambers", new Vector3(0.86f, 0.86f, 0.78f), BuildAmmoB),
                new MechanismSpec("WMS04_ScattergunPressureChamber_Twin_A.prefab", "scattergun-pressure-chambers", "twin scattergun pressure chamber module with shared gauge rail", new Vector3(1.35f, 0.78f, 1.28f), BuildScatterA),
                new MechanismSpec("WMS04_ScattergunPressureChamber_Quad_B.prefab", "scattergun-pressure-chambers", "quad short chamber block for bulky scattergun viewmodel concepts", new Vector3(1.55f, 0.95f, 1.05f), BuildScatterB),
                new MechanismSpec("WMS04_BoltThrowerRail_TwinTrack_A.prefab", "bolt-thrower-rails", "twin track bolt thrower rail with brass cross braces", new Vector3(0.95f, 0.42f, 1.9f), BuildRailA),
                new MechanismSpec("WMS04_BoltThrowerRail_ChargedSlide_B.prefab", "bolt-thrower-rails", "charged slide rail with amber pressure capacitors", new Vector3(1.05f, 0.55f, 1.85f), BuildRailB),
                new MechanismSpec("WMS04_GlovedHandSilhouette_RightGrip_A.prefab", "gloved-hand-silhouettes", "right hand dark leather grip silhouette with readable trigger finger", new Vector3(0.78f, 0.52f, 0.95f), BuildGloveRight),
                new MechanismSpec("WMS04_GlovedHandSilhouette_LeftSupport_B.prefab", "gloved-hand-silhouettes", "left support hand silhouette for underbarrel grip composition", new Vector3(0.92f, 0.48f, 0.88f), BuildGloveLeft),
                new MechanismSpec("WMS04_MaterialSwatch_MetalsAndGlass_A.prefab", "material-swatch-prefabs", "brass copper iron steel glass material realism board", new Vector3(2.0f, 0.8f, 0.3f), BuildSwatchA),
                new MechanismSpec("WMS04_MaterialSwatch_GripAndWear_B.prefab", "material-swatch-prefabs", "wood leather soot edge wear and warning paint material board", new Vector3(2.0f, 0.8f, 0.3f), BuildSwatchB)
            };
        }

        private static void CreateMaterials()
        {
            AddMaterial("WMS04_MAT_AgedBrassBrushed", new Color(0.78f, 0.55f, 0.25f), 0.62f, 0.86f);
            AddMaterial("WMS04_MAT_SmokedBrass", new Color(0.36f, 0.24f, 0.11f), 0.45f, 0.80f);
            AddMaterial("WMS04_MAT_PolishedEdgeBrass", new Color(1.0f, 0.76f, 0.34f), 0.78f, 0.92f);
            AddMaterial("WMS04_MAT_BlackenedIron", new Color(0.035f, 0.033f, 0.03f), 0.30f, 0.86f);
            AddMaterial("WMS04_MAT_OilyGunmetal", new Color(0.12f, 0.125f, 0.12f), 0.74f, 0.82f);
            AddMaterial("WMS04_MAT_BluedSpringSteel", new Color(0.10f, 0.14f, 0.20f), 0.68f, 0.78f);
            AddMaterial("WMS04_MAT_HeatVioletSteel", new Color(0.28f, 0.18f, 0.36f), 0.70f, 0.76f);
            AddMaterial("WMS04_MAT_BurnishedCopper", new Color(0.69f, 0.29f, 0.14f), 0.52f, 0.78f);
            AddMaterial("WMS04_MAT_OxidizedCopperGreen", new Color(0.16f, 0.42f, 0.32f), 0.32f, 0.48f);
            AddMaterial("WMS04_MAT_GaugeIvoryFace", new Color(0.82f, 0.76f, 0.58f), 0.20f, 0.0f);
            AddMaterial("WMS04_MAT_RedNeedlePaint", new Color(0.86f, 0.035f, 0.02f), 0.28f, 0.10f);
            AddMaterial("WMS04_MAT_WalnutVarnish", new Color(0.31f, 0.14f, 0.055f), 0.58f, 0.03f);
            AddMaterial("WMS04_MAT_CrackedBrownLeather", new Color(0.28f, 0.13f, 0.055f), 0.34f, 0.02f);
            AddMaterial("WMS04_MAT_DarkGloveLeather", new Color(0.045f, 0.038f, 0.032f), 0.48f, 0.02f);
            AddMaterial("WMS04_MAT_RedSafetyEnamel", new Color(0.62f, 0.025f, 0.018f), 0.52f, 0.0f);
            AddMaterial("WMS04_MAT_SootGrime", new Color(0.018f, 0.016f, 0.013f), 0.08f, 0.0f);
            AddMaterial("WMS04_MAT_AmberPressureGlass", new Color(1.0f, 0.50f, 0.12f, 0.72f), 0.88f, 0.0f, new Color(1.0f, 0.34f, 0.06f) * 2.1f, true);
            AddMaterial("WMS04_MAT_GreenGaugeGlass", new Color(0.13f, 0.70f, 0.45f, 0.68f), 0.82f, 0.0f, new Color(0.06f, 0.38f, 0.23f) * 1.3f, true);
            AddMaterial("WMS04_MAT_TealPressureGlow", new Color(0.08f, 0.60f, 0.72f), 0.44f, 0.0f, new Color(0.02f, 0.45f, 0.62f) * 2.6f, false);
            AddMaterial("WMS04_MAT_EngravedDarkFill", new Color(0.006f, 0.005f, 0.004f), 0.05f, 0.0f);
        }

        private static void CreateMeshes()
        {
            AddMesh("WMS04_MESH_BoxUnit", BuildBoxMesh("WMS04_MESH_BoxUnit"));
            AddMesh("WMS04_MESH_Cylinder16Unit", BuildCylinderMesh("WMS04_MESH_Cylinder16Unit", 16));
            AddMesh("WMS04_MESH_Cylinder32Unit", BuildCylinderMesh("WMS04_MESH_Cylinder32Unit", 32));
            AddMesh("WMS04_MESH_Ring32Unit", BuildRingMesh("WMS04_MESH_Ring32Unit", 32, 0.32f, 0.5f));
            AddMesh("WMS04_MESH_Gear24Ring", BuildGearMesh("WMS04_MESH_Gear24Ring", 24, 0.30f, 0.50f));
            AddMesh("WMS04_MESH_GaugeNeedle", BuildNeedleMesh("WMS04_MESH_GaugeNeedle"));
            AddMesh("WMS04_MESH_TaperedGrip", BuildTaperedGripMesh("WMS04_MESH_TaperedGrip"));
            AddMesh("WMS04_MESH_LeverPaddle", BuildLeverPaddleMesh("WMS04_MESH_LeverPaddle"));
            AddMesh("WMS04_MESH_RailFin", BuildRailFinMesh("WMS04_MESH_RailFin"));
            AddMesh("WMS04_MESH_GlovePalm", BuildGlovePalmMesh("WMS04_MESH_GlovePalm"));
            AddMesh("WMS04_MESH_QuadUnit", BuildQuadMesh("WMS04_MESH_QuadUnit"));
        }

        private static void BuildCoilA(GameObject root) { BuildCoilAssembly(root, 5, 0.42f, false, true); }
        private static void BuildCoilB(GameObject root) { BuildCoilAssembly(root, 7, 0.36f, true, false); }
        private static void BuildCoilC(GameObject root) { BuildCoilAssembly(root, 9, 0.30f, false, false); }
        private static void BuildCoilD(GameObject root) { BuildCoilAssembly(root, 6, 0.50f, true, true); }

        private static void BuildGaugeA(GameObject root) { BuildGaugeCluster(root, 3, false); }
        private static void BuildGaugeB(GameObject root) { BuildGaugeCluster(root, 2, true); }
        private static void BuildGaugeC(GameObject root) { BuildGaugeCluster(root, 2, false); }

        private static void BuildGripA(GameObject root)
        {
            Part(root.transform, "varnished_walnut_tapered_grip", "WMS04_MESH_TaperedGrip", "WMS04_MAT_WalnutVarnish", new Vector3(0f, -0.2f, 0f), new Vector3(0.58f, 1.15f, 0.44f), new Vector3(10f, 0f, -7f));
            AddLeatherBands(root.transform, 5, 0.0f, "WMS04_MAT_CrackedBrownLeather");
            Part(root.transform, "polished_brass_pommel_cap", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_PolishedEdgeBrass", new Vector3(0.0f, -0.82f, 0.02f), new Vector3(0.40f, 0.10f, 0.40f), Vector3.zero);
            Part(root.transform, "blackened_trigger_guard_socket", "WMS04_MESH_Ring32Unit", "WMS04_MAT_BlackenedIron", new Vector3(0.0f, 0.42f, -0.23f), new Vector3(0.62f, 0.62f, 0.62f), new Vector3(0f, 0f, 0f));
        }

        private static void BuildGripB(GameObject root)
        {
            Part(root.transform, "oiled_stock_spine", "WMS04_MESH_TaperedGrip", "WMS04_MAT_WalnutVarnish", new Vector3(0f, -0.15f, 0.12f), new Vector3(0.62f, 1.28f, 0.60f), new Vector3(18f, 0f, 0f));
            Part(root.transform, "dark_glove_clearance_shadow", "WMS04_MESH_GlovePalm", "WMS04_MAT_DarkGloveLeather", new Vector3(0.02f, -0.02f, -0.28f), new Vector3(0.70f, 0.48f, 0.35f), new Vector3(0f, 0f, 0f));
            AddRivetLine(root.transform, "brass_backstrap_rivets", 5, new Vector3(0f, -0.58f, -0.28f), new Vector3(0f, 0.23f, 0f), "WMS04_MAT_PolishedEdgeBrass");
            Part(root.transform, "smoked_brass_stock_socket", "WMS04_MESH_BoxUnit", "WMS04_MAT_SmokedBrass", new Vector3(0f, 0.52f, 0.24f), new Vector3(0.58f, 0.18f, 0.62f), Vector3.zero);
        }

        private static void BuildGripC(GameObject root)
        {
            Part(root.transform, "rivet_palm_leather_core", "WMS04_MESH_TaperedGrip", "WMS04_MAT_CrackedBrownLeather", new Vector3(0f, -0.12f, 0f), new Vector3(0.66f, 1.02f, 0.48f), new Vector3(7f, 0f, 6f));
            AddLeatherBands(root.transform, 4, -0.05f, "WMS04_MAT_DarkGloveLeather");
            Part(root.transform, "aged_brass_trigger_guard", "WMS04_MESH_Ring32Unit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0f, 0.44f, -0.21f), new Vector3(0.72f, 0.72f, 0.72f), Vector3.zero);
            AddRivetLine(root.transform, "polished_palm_rivets", 6, new Vector3(-0.25f, -0.58f, -0.18f), new Vector3(0.10f, 0.20f, 0f), "WMS04_MAT_PolishedEdgeBrass");
        }

        private static void BuildReceiverA(GameObject root)
        {
            Part(root.transform, "smoked_brass_receiver_backing", "WMS04_MESH_BoxUnit", "WMS04_MAT_SmokedBrass", Vector3.zero, new Vector3(1.35f, 0.72f, 0.10f), Vector3.zero);
            Part(root.transform, "aged_brass_lattice_top", "WMS04_MESH_BoxUnit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0f, 0.22f, -0.07f), new Vector3(1.18f, 0.08f, 0.08f), Vector3.zero);
            Part(root.transform, "aged_brass_lattice_bottom", "WMS04_MESH_BoxUnit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0f, -0.22f, -0.07f), new Vector3(1.18f, 0.08f, 0.08f), Vector3.zero);
            Part(root.transform, "cross_brace_left", "WMS04_MESH_BoxUnit", "WMS04_MAT_PolishedEdgeBrass", new Vector3(-0.28f, 0f, -0.08f), new Vector3(0.08f, 0.62f, 0.08f), new Vector3(0f, 0f, -32f));
            Part(root.transform, "cross_brace_right", "WMS04_MESH_BoxUnit", "WMS04_MAT_PolishedEdgeBrass", new Vector3(0.28f, 0f, -0.08f), new Vector3(0.08f, 0.62f, 0.08f), new Vector3(0f, 0f, 32f));
            AddRivetFrame(root.transform, 5, 3, 0.60f, 0.30f);
        }

        private static void BuildReceiverB(GameObject root)
        {
            Part(root.transform, "black_iron_hatch_backing", "WMS04_MESH_BoxUnit", "WMS04_MAT_BlackenedIron", Vector3.zero, new Vector3(1.22f, 0.78f, 0.12f), Vector3.zero);
            Part(root.transform, "brass_inspection_hatch", "WMS04_MESH_BoxUnit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0.10f, 0f, -0.09f), new Vector3(0.78f, 0.58f, 0.08f), Vector3.zero);
            Part(root.transform, "copper_hinge_barrel", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_BurnishedCopper", new Vector3(-0.55f, 0f, -0.10f), new Vector3(0.08f, 0.76f, 0.08f), Vector3.zero);
            Part(root.transform, "green_glass_inspection_slot", "WMS04_MESH_BoxUnit", "WMS04_MAT_GreenGaugeGlass", new Vector3(0.23f, 0.04f, -0.15f), new Vector3(0.36f, 0.12f, 0.04f), Vector3.zero);
            AddRivetFrame(root.transform, 4, 3, 0.50f, 0.33f);
        }

        private static void BuildReceiverC(GameObject root)
        {
            Part(root.transform, "edge_worn_gunmetal_backing", "WMS04_MESH_BoxUnit", "WMS04_MAT_OilyGunmetal", Vector3.zero, new Vector3(1.50f, 0.60f, 0.12f), Vector3.zero);
            Part(root.transform, "serial_plaque_brass_face", "WMS04_MESH_BoxUnit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0f, 0f, -0.09f), new Vector3(1.16f, 0.34f, 0.06f), Vector3.zero);
            for (int i = 0; i < 7; i++)
            {
                Part(root.transform, "engraved_dark_serial_tick_" + i, "WMS04_MESH_BoxUnit", "WMS04_MAT_EngravedDarkFill", new Vector3(-0.42f + i * 0.14f, 0f, -0.13f), new Vector3(0.035f, 0.22f, 0.025f), Vector3.zero);
            }
            AddRivetFrame(root.transform, 6, 2, 0.68f, 0.24f);
        }

        private static void BuildMuzzleA(GameObject root)
        {
            Part(root.transform, "blackened_iron_muzzle_core", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_BlackenedIron", Vector3.zero, new Vector3(0.42f, 0.62f, 0.42f), new Vector3(90f, 0f, 0f));
            for (int i = 0; i < 8; i++)
            {
                float a = i * Mathf.PI * 2f / 8f;
                Part(root.transform, "oily_steel_finned_crown_" + i, "WMS04_MESH_RailFin", "WMS04_MAT_OilyGunmetal", new Vector3(Mathf.Cos(a) * 0.34f, Mathf.Sin(a) * 0.34f, 0f), new Vector3(0.14f, 0.20f, 0.50f), new Vector3(0f, 0f, -i * 45f));
            }
            AddRingBand(root.transform, "front_polished_brass_retainer", 0.48f, 0.10f, 0.28f, "WMS04_MAT_PolishedEdgeBrass");
        }

        private static void BuildMuzzleB(GameObject root)
        {
            Part(root.transform, "cog_brake_black_core", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_BlackenedIron", Vector3.zero, new Vector3(0.34f, 0.50f, 0.34f), new Vector3(90f, 0f, 0f));
            Part(root.transform, "soot_dark_cog_teeth", "WMS04_MESH_Gear24Ring", "WMS04_MAT_SootGrime", new Vector3(0f, 0f, -0.28f), new Vector3(0.88f, 0.88f, 0.88f), Vector3.zero);
            Part(root.transform, "rear_brass_compression_ring", "WMS04_MESH_Ring32Unit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0f, 0f, 0.25f), new Vector3(0.74f, 0.74f, 0.74f), Vector3.zero);
            AddRivetCircle(root.transform, 10, 0.36f, -0.32f, "WMS04_MAT_PolishedEdgeBrass");
        }

        private static void BuildTankA(GameObject root)
        {
            Part(root.transform, "cigar_brass_pressure_tank", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_AgedBrassBrushed", Vector3.zero, new Vector3(0.42f, 1.30f, 0.42f), new Vector3(90f, 0f, 0f));
            AddRingBand(root.transform, "smoked_brass_front_band", 0.46f, 0.10f, -0.48f, "WMS04_MAT_SmokedBrass");
            AddRingBand(root.transform, "smoked_brass_rear_band", 0.46f, 0.10f, 0.48f, "WMS04_MAT_SmokedBrass");
            Part(root.transform, "green_glass_sight_window", "WMS04_MESH_BoxUnit", "WMS04_MAT_GreenGaugeGlass", new Vector3(0f, 0.30f, -0.02f), new Vector3(0.18f, 0.06f, 0.56f), Vector3.zero);
        }

        private static void BuildTankB(GameObject root)
        {
            Part(root.transform, "left_underbarrel_tank", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_SmokedBrass", new Vector3(-0.24f, 0f, 0f), new Vector3(0.30f, 1.28f, 0.30f), new Vector3(90f, 0f, 0f));
            Part(root.transform, "right_underbarrel_tank", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_SmokedBrass", new Vector3(0.24f, 0f, 0f), new Vector3(0.30f, 1.28f, 0.30f), new Vector3(90f, 0f, 0f));
            Part(root.transform, "amber_fill_cap_left", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_AmberPressureGlass", new Vector3(-0.24f, 0.20f, -0.54f), new Vector3(0.16f, 0.08f, 0.16f), Vector3.zero);
            Part(root.transform, "amber_fill_cap_right", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_AmberPressureGlass", new Vector3(0.24f, 0.20f, -0.54f), new Vector3(0.16f, 0.08f, 0.16f), Vector3.zero);
            Part(root.transform, "central_black_iron_yoke", "WMS04_MESH_BoxUnit", "WMS04_MAT_BlackenedIron", new Vector3(0f, 0f, 0f), new Vector3(0.72f, 0.16f, 0.18f), Vector3.zero);
        }

        private static void BuildValveA(GameObject root)
        {
            Part(root.transform, "brass_valve_hub", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_AgedBrassBrushed", Vector3.zero, new Vector3(0.26f, 0.20f, 0.26f), new Vector3(90f, 0f, 0f));
            Part(root.transform, "red_enamel_safety_paddle", "WMS04_MESH_LeverPaddle", "WMS04_MAT_RedSafetyEnamel", new Vector3(0.32f, 0.12f, 0f), new Vector3(0.72f, 0.20f, 0.12f), Vector3.zero);
            Part(root.transform, "black_iron_stem", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_BlackenedIron", new Vector3(-0.18f, 0f, 0f), new Vector3(0.08f, 0.46f, 0.08f), new Vector3(0f, 0f, 90f));
        }

        private static void BuildValveB(GameObject root)
        {
            Part(root.transform, "small_crank_wheel_ring", "WMS04_MESH_Ring32Unit", "WMS04_MAT_AgedBrassBrushed", Vector3.zero, new Vector3(0.74f, 0.74f, 0.74f), Vector3.zero);
            Part(root.transform, "crank_wheel_cross_a", "WMS04_MESH_BoxUnit", "WMS04_MAT_PolishedEdgeBrass", Vector3.zero, new Vector3(0.74f, 0.07f, 0.06f), Vector3.zero);
            Part(root.transform, "crank_wheel_cross_b", "WMS04_MESH_BoxUnit", "WMS04_MAT_PolishedEdgeBrass", Vector3.zero, new Vector3(0.07f, 0.74f, 0.06f), Vector3.zero);
            Part(root.transform, "oily_black_center_knob", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_OilyGunmetal", Vector3.zero, new Vector3(0.18f, 0.14f, 0.18f), new Vector3(90f, 0f, 0f));
        }

        private static void BuildAmmoA(GameObject root)
        {
            BuildAmmoCylinder(root, 6, 0.30f, "WMS04_MAT_BurnishedCopper");
        }

        private static void BuildAmmoB(GameObject root)
        {
            BuildAmmoCylinder(root, 8, 0.34f, "WMS04_MAT_AgedBrassBrushed");
        }

        private static void BuildScatterA(GameObject root)
        {
            Part(root.transform, "left_scatter_pressure_chamber", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_BlackenedIron", new Vector3(-0.28f, 0f, 0f), new Vector3(0.34f, 1.18f, 0.34f), new Vector3(90f, 0f, 0f));
            Part(root.transform, "right_scatter_pressure_chamber", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_BlackenedIron", new Vector3(0.28f, 0f, 0f), new Vector3(0.34f, 1.18f, 0.34f), new Vector3(90f, 0f, 0f));
            Part(root.transform, "shared_brass_gauge_rail", "WMS04_MESH_BoxUnit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0f, 0.34f, -0.08f), new Vector3(0.92f, 0.12f, 0.92f), Vector3.zero);
            BuildSmallGauge(root.transform, new Vector3(0f, 0.45f, -0.22f), 0.22f, "scatter_chamber_top_gauge");
            AddRivetLine(root.transform, "scatter_chamber_clamp_rivets", 5, new Vector3(-0.48f, -0.28f, -0.40f), new Vector3(0.24f, 0f, 0.20f), "WMS04_MAT_PolishedEdgeBrass");
        }

        private static void BuildScatterB(GameObject root)
        {
            for (int x = -1; x <= 1; x += 2)
            {
                for (int y = -1; y <= 1; y += 2)
                {
                    Part(root.transform, "quad_short_chamber_" + x + "_" + y, "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_OilyGunmetal", new Vector3(x * 0.30f, y * 0.22f, 0f), new Vector3(0.28f, 0.95f, 0.28f), new Vector3(90f, 0f, 0f));
                }
            }
            Part(root.transform, "brass_quad_chamber_yoke", "WMS04_MESH_BoxUnit", "WMS04_MAT_SmokedBrass", Vector3.zero, new Vector3(1.0f, 0.72f, 0.16f), Vector3.zero);
            Part(root.transform, "amber_center_pressure_block", "WMS04_MESH_BoxUnit", "WMS04_MAT_AmberPressureGlass", new Vector3(0f, 0f, -0.12f), new Vector3(0.28f, 0.22f, 0.08f), Vector3.zero);
        }

        private static void BuildRailA(GameObject root)
        {
            Part(root.transform, "left_bolt_thrower_track", "WMS04_MESH_BoxUnit", "WMS04_MAT_BluedSpringSteel", new Vector3(-0.26f, 0f, 0f), new Vector3(0.10f, 0.10f, 1.82f), Vector3.zero);
            Part(root.transform, "right_bolt_thrower_track", "WMS04_MESH_BoxUnit", "WMS04_MAT_BluedSpringSteel", new Vector3(0.26f, 0f, 0f), new Vector3(0.10f, 0.10f, 1.82f), Vector3.zero);
            for (int i = 0; i < 5; i++)
            {
                Part(root.transform, "brass_cross_brace_" + i, "WMS04_MESH_BoxUnit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0f, 0f, -0.72f + i * 0.36f), new Vector3(0.66f, 0.08f, 0.08f), Vector3.zero);
            }
            Part(root.transform, "blackened_center_bolt", "WMS04_MESH_RailFin", "WMS04_MAT_BlackenedIron", new Vector3(0f, 0.10f, -0.12f), new Vector3(0.32f, 0.22f, 0.92f), new Vector3(90f, 0f, 0f));
        }

        private static void BuildRailB(GameObject root)
        {
            BuildRailA(root);
            Part(root.transform, "charged_slide_teal_pressure_cell_front", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_TealPressureGlow", new Vector3(0f, 0.24f, -0.52f), new Vector3(0.18f, 0.28f, 0.18f), new Vector3(90f, 0f, 0f));
            Part(root.transform, "charged_slide_amber_pressure_cell_rear", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_AmberPressureGlass", new Vector3(0f, 0.24f, 0.42f), new Vector3(0.18f, 0.28f, 0.18f), new Vector3(90f, 0f, 0f));
        }

        private static void BuildGloveRight(GameObject root)
        {
            Part(root.transform, "right_glove_palm_blockout", "WMS04_MESH_GlovePalm", "WMS04_MAT_DarkGloveLeather", Vector3.zero, new Vector3(0.72f, 0.34f, 0.78f), new Vector3(8f, 0f, -4f));
            for (int i = 0; i < 4; i++)
            {
                Part(root.transform, "right_glove_curled_finger_" + i, "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_CrackedBrownLeather", new Vector3(-0.24f + i * 0.16f, -0.05f, -0.42f), new Vector3(0.09f, 0.34f, 0.09f), new Vector3(68f, 0f, 0f));
            }
            Part(root.transform, "right_glove_trigger_finger_read", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_CrackedBrownLeather", new Vector3(0.40f, -0.02f, -0.24f), new Vector3(0.08f, 0.46f, 0.08f), new Vector3(42f, 0f, -25f));
        }

        private static void BuildGloveLeft(GameObject root)
        {
            Part(root.transform, "left_support_palm_blockout", "WMS04_MESH_GlovePalm", "WMS04_MAT_DarkGloveLeather", Vector3.zero, new Vector3(0.84f, 0.30f, 0.70f), new Vector3(-4f, 0f, 8f));
            for (int i = 0; i < 5; i++)
            {
                Part(root.transform, "left_support_finger_wrap_" + i, "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_CrackedBrownLeather", new Vector3(-0.32f + i * 0.16f, -0.04f, -0.34f), new Vector3(0.08f, 0.38f, 0.08f), new Vector3(78f, 0f, 0f));
            }
            Part(root.transform, "left_support_brass_scale_reference", "WMS04_MESH_BoxUnit", "WMS04_MAT_AgedBrassBrushed", new Vector3(0f, 0.20f, 0.30f), new Vector3(0.70f, 0.06f, 0.12f), Vector3.zero);
        }

        private static void BuildSwatchA(GameObject root)
        {
            string[] mats =
            {
                "WMS04_MAT_AgedBrassBrushed", "WMS04_MAT_SmokedBrass", "WMS04_MAT_PolishedEdgeBrass", "WMS04_MAT_BurnishedCopper",
                "WMS04_MAT_BlackenedIron", "WMS04_MAT_OilyGunmetal", "WMS04_MAT_BluedSpringSteel", "WMS04_MAT_AmberPressureGlass",
                "WMS04_MAT_GreenGaugeGlass", "WMS04_MAT_TealPressureGlow"
            };
            BuildMaterialBoard(root.transform, mats);
        }

        private static void BuildSwatchB(GameObject root)
        {
            string[] mats =
            {
                "WMS04_MAT_WalnutVarnish", "WMS04_MAT_CrackedBrownLeather", "WMS04_MAT_DarkGloveLeather", "WMS04_MAT_RedSafetyEnamel",
                "WMS04_MAT_SootGrime", "WMS04_MAT_EngravedDarkFill", "WMS04_MAT_GaugeIvoryFace", "WMS04_MAT_RedNeedlePaint",
                "WMS04_MAT_OxidizedCopperGreen", "WMS04_MAT_HeatVioletSteel"
            };
            BuildMaterialBoard(root.transform, mats);
        }

        private static void BuildCoilAssembly(GameObject root, int coils, float radius, bool copper, bool glowCore)
        {
            Part(root.transform, "black_iron_barrel_axis", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_BlackenedIron", Vector3.zero, new Vector3(0.18f, 1.55f, 0.18f), new Vector3(90f, 0f, 0f));
            for (int i = 0; i < coils; i++)
            {
                float z = -0.62f + i * (1.24f / Mathf.Max(1, coils - 1));
                Part(root.transform, "separated_pressure_coil_" + i, "WMS04_MESH_Ring32Unit", copper ? "WMS04_MAT_BurnishedCopper" : "WMS04_MAT_BluedSpringSteel", new Vector3(0f, 0f, z), new Vector3(radius, radius, radius), Vector3.zero);
            }
            AddRingBand(root.transform, "front_brass_clamp_band", radius + 0.08f, 0.10f, -0.70f, "WMS04_MAT_AgedBrassBrushed");
            AddRingBand(root.transform, "rear_brass_clamp_band", radius + 0.08f, 0.10f, 0.70f, "WMS04_MAT_AgedBrassBrushed");
            if (glowCore)
            {
                Part(root.transform, "amber_pressure_glass_core", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_AmberPressureGlass", new Vector3(0f, 0f, 0f), new Vector3(0.10f, 1.20f, 0.10f), new Vector3(90f, 0f, 0f));
            }
            AddRivetCircle(root.transform, 8, radius + 0.12f, -0.74f, "WMS04_MAT_PolishedEdgeBrass");
            AddRivetCircle(root.transform, 8, radius + 0.12f, 0.74f, "WMS04_MAT_PolishedEdgeBrass");
        }

        private static void BuildGaugeCluster(GameObject root, int count, bool stacked)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = stacked
                    ? new Vector3(0f, (i - (count - 1) * 0.5f) * 0.48f, -0.03f)
                    : new Vector3((i - (count - 1) * 0.5f) * 0.42f, 0f, -0.03f);
                BuildSmallGauge(root.transform, position, i == 1 ? 0.25f : 0.22f, "readable_pressure_dial_" + i);
            }
            Part(root.transform, "smoked_brass_gauge_mount_plate", "WMS04_MESH_BoxUnit", "WMS04_MAT_SmokedBrass", new Vector3(0f, 0f, 0.05f), stacked ? new Vector3(0.52f, 1.05f, 0.08f) : new Vector3(1.22f, 0.48f, 0.08f), Vector3.zero);
        }

        private static void BuildSmallGauge(Transform parent, Vector3 position, float scale, string prefix)
        {
            Part(parent, prefix + "_brass_bezel", "WMS04_MESH_Ring32Unit", "WMS04_MAT_AgedBrassBrushed", position + new Vector3(0f, 0f, -0.05f), Vector3.one * scale, Vector3.zero);
            Part(parent, prefix + "_ivory_face", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_GaugeIvoryFace", position + new Vector3(0f, 0f, -0.04f), new Vector3(scale * 0.60f, 0.025f, scale * 0.60f), new Vector3(90f, 0f, 0f));
            Part(parent, prefix + "_redline_needle", "WMS04_MESH_GaugeNeedle", "WMS04_MAT_RedNeedlePaint", position + new Vector3(0f, 0f, -0.075f), Vector3.one * scale * 0.78f, new Vector3(0f, 0f, -28f));
            Part(parent, prefix + "_green_glass_glint", "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_GreenGaugeGlass", position + new Vector3(scale * 0.25f, scale * 0.24f, -0.09f), new Vector3(scale * 0.10f, 0.012f, scale * 0.10f), new Vector3(90f, 0f, 0f));
        }

        private static void BuildAmmoCylinder(GameObject root, int cells, float radius, string materialName)
        {
            Part(root.transform, "central_blackened_axle", "WMS04_MESH_Cylinder32Unit", "WMS04_MAT_BlackenedIron", Vector3.zero, new Vector3(0.16f, 0.82f, 0.16f), new Vector3(90f, 0f, 0f));
            for (int i = 0; i < cells; i++)
            {
                float a = i * Mathf.PI * 2f / cells;
                Part(root.transform, "rotary_pressure_cell_" + i, "WMS04_MESH_Cylinder16Unit", materialName, new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, 0f), new Vector3(0.15f, 0.72f, 0.15f), new Vector3(90f, 0f, 0f));
            }
            Part(root.transform, "front_gear_index_plate", "WMS04_MESH_Gear24Ring", "WMS04_MAT_SmokedBrass", new Vector3(0f, 0f, -0.42f), Vector3.one * 0.72f, Vector3.zero);
            Part(root.transform, "rear_polished_cap", "WMS04_MESH_Ring32Unit", "WMS04_MAT_PolishedEdgeBrass", new Vector3(0f, 0f, 0.42f), Vector3.one * 0.58f, Vector3.zero);
        }

        private static void BuildMaterialBoard(Transform parent, string[] materialNames)
        {
            Part(parent, "smoked_iron_swatch_backing", "WMS04_MESH_BoxUnit", "WMS04_MAT_BlackenedIron", Vector3.zero, new Vector3(2.15f, 0.86f, 0.06f), Vector3.zero);
            for (int i = 0; i < materialNames.Length; i++)
            {
                int col = i % 5;
                int row = i / 5;
                Vector3 p = new Vector3(-0.78f + col * 0.39f, 0.20f - row * 0.36f, -0.08f);
                string mesh = i % 3 == 0 ? "WMS04_MESH_Cylinder32Unit" : (i % 3 == 1 ? "WMS04_MESH_BoxUnit" : "WMS04_MESH_Ring32Unit");
                Vector3 scale = mesh == "WMS04_MESH_BoxUnit" ? new Vector3(0.26f, 0.20f, 0.06f) : Vector3.one * 0.22f;
                Vector3 rot = mesh == "WMS04_MESH_Cylinder32Unit" ? new Vector3(90f, 0f, 0f) : Vector3.zero;
                Part(parent, "material_chip_" + i + "_" + materialNames[i], mesh, materialNames[i], p, scale, rot);
            }
        }

        private static void AddLeatherBands(Transform parent, int count, float z, string materialName)
        {
            for (int i = 0; i < count; i++)
            {
                Part(parent, "wrapped_leather_band_" + i, "WMS04_MESH_BoxUnit", materialName, new Vector3(0f, -0.56f + i * 0.22f, z - 0.23f), new Vector3(0.55f, 0.055f, 0.055f), Vector3.zero);
            }
        }

        private static void AddRingBand(Transform parent, string name, float radius, float depth, float z, string materialName)
        {
            Part(parent, name, "WMS04_MESH_Ring32Unit", materialName, new Vector3(0f, 0f, z), Vector3.one * radius, Vector3.zero);
            Part(parent, name + "_depth_hint", "WMS04_MESH_Cylinder32Unit", materialName, new Vector3(0f, 0f, z), new Vector3(radius * 0.26f, depth, radius * 0.26f), new Vector3(90f, 0f, 0f));
        }

        private static void AddRivetLine(Transform parent, string prefix, int count, Vector3 start, Vector3 step, string materialName)
        {
            for (int i = 0; i < count; i++)
            {
                Part(parent, prefix + "_" + i, "WMS04_MESH_Cylinder16Unit", materialName, start + step * i, new Vector3(0.055f, 0.025f, 0.055f), new Vector3(90f, 0f, 0f));
            }
        }

        private static void AddRivetCircle(Transform parent, int count, float radius, float z, string materialName)
        {
            for (int i = 0; i < count; i++)
            {
                float a = i * Mathf.PI * 2f / count;
                Part(parent, "circular_retainer_rivet_" + i, "WMS04_MESH_Cylinder16Unit", materialName, new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, z), new Vector3(0.05f, 0.025f, 0.05f), new Vector3(90f, 0f, 0f));
            }
        }

        private static void AddRivetFrame(Transform parent, int xCount, int yCount, float halfX, float halfY)
        {
            for (int x = 0; x < xCount; x++)
            {
                float px = Mathf.Lerp(-halfX, halfX, x / Mathf.Max(1f, xCount - 1f));
                Part(parent, "frame_top_rivet_" + x, "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_PolishedEdgeBrass", new Vector3(px, halfY, -0.13f), new Vector3(0.055f, 0.025f, 0.055f), new Vector3(90f, 0f, 0f));
                Part(parent, "frame_bottom_rivet_" + x, "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_PolishedEdgeBrass", new Vector3(px, -halfY, -0.13f), new Vector3(0.055f, 0.025f, 0.055f), new Vector3(90f, 0f, 0f));
            }
            for (int y = 1; y < yCount - 1; y++)
            {
                float py = Mathf.Lerp(-halfY, halfY, y / Mathf.Max(1f, yCount - 1f));
                Part(parent, "frame_left_rivet_" + y, "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_PolishedEdgeBrass", new Vector3(-halfX, py, -0.13f), new Vector3(0.055f, 0.025f, 0.055f), new Vector3(90f, 0f, 0f));
                Part(parent, "frame_right_rivet_" + y, "WMS04_MESH_Cylinder16Unit", "WMS04_MAT_PolishedEdgeBrass", new Vector3(halfX, py, -0.13f), new Vector3(0.055f, 0.025f, 0.055f), new Vector3(90f, 0f, 0f));
            }
        }

        private static GameObject Part(Transform parent, string name, string meshName, string materialName, Vector3 localPosition, Vector3 localScale, Vector3 localEuler)
        {
            GameObject child = new GameObject(name);
            child.transform.SetParent(parent, false);
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.Euler(localEuler);
            child.transform.localScale = localScale;
            child.AddComponent<MeshFilter>().sharedMesh = Meshes[meshName];
            child.AddComponent<MeshRenderer>().sharedMaterial = Materials[materialName];
            return child;
        }

        private static void CreatePrefab(MechanismSpec spec)
        {
            GameObject root = new GameObject(Path.GetFileNameWithoutExtension(spec.fileName));
            root.tag = "Untagged";
            root.layer = 0;
            GameObject note = new GameObject("visual_only_no_colliders_no_rigidbodies_no_audio_no_gameplay_scripts");
            note.transform.SetParent(root.transform, false);
            GameObject family = new GameObject("family_" + spec.family);
            family.transform.SetParent(root.transform, false);
            spec.builder(root);
            StripForbiddenComponents(root);
            PrefabUtility.SaveAsPrefabAsset(root, PackageRoot + "/Runtime/Prefabs/" + spec.fileName);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static void StripForbiddenComponents(GameObject root)
        {
            foreach (Collider component in root.GetComponentsInChildren<Collider>(true)) UnityEngine.Object.DestroyImmediate(component);
            foreach (Rigidbody component in root.GetComponentsInChildren<Rigidbody>(true)) UnityEngine.Object.DestroyImmediate(component);
            foreach (AudioSource component in root.GetComponentsInChildren<AudioSource>(true)) UnityEngine.Object.DestroyImmediate(component);
            foreach (ParticleSystem component in root.GetComponentsInChildren<ParticleSystem>(true)) UnityEngine.Object.DestroyImmediate(component);
        }

        private static void EnsureFolders()
        {
            foreach (string folder in new[] { "Runtime/Prefabs", "Runtime/Materials", "Runtime/Meshes", "Runtime/Metadata", "Documentation~/Manifest", "Samples~/PreviewScene" })
            {
                Directory.CreateDirectory(FullPath(PackageRoot + "/" + folder));
            }
            AssetDatabase.Refresh();
        }

        private static void AddMaterial(string name, Color color, float smoothness, float metallic, Color emission = default(Color), bool transparent = false)
        {
            string path = PackageRoot + "/Runtime/Materials/" + name + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.name = name;
            SetMaterialColor(material, "_BaseColor", color);
            SetMaterialColor(material, "_Color", color);
            SetMaterialFloat(material, "_Smoothness", smoothness);
            SetMaterialFloat(material, "_Glossiness", smoothness);
            SetMaterialFloat(material, "_Metallic", metallic);
            if (emission.maxColorComponent > 0.001f)
            {
                material.EnableKeyword("_EMISSION");
                SetMaterialColor(material, "_EmissionColor", emission);
            }
            if (transparent)
            {
                SetMaterialFloat(material, "_Surface", 1f);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = 3000;
            }

            EditorUtility.SetDirty(material);
            Materials[name] = material;
        }

        private static void SetMaterialColor(Material material, string property, Color value)
        {
            if (material.HasProperty(property)) material.SetColor(property, value);
        }

        private static void SetMaterialFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property)) material.SetFloat(property, value);
        }

        private static void AddMesh(string name, Mesh source)
        {
            source.name = name;
            string path = PackageRoot + "/Runtime/Meshes/" + name + ".asset";
            Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing == null)
            {
                AssetDatabase.CreateAsset(source, path);
                Meshes[name] = source;
            }
            else
            {
                EditorUtility.CopySerialized(source, existing);
                existing.name = name;
                EditorUtility.SetDirty(existing);
                Meshes[name] = existing;
            }
        }

        private static Mesh BuildBoxMesh(string name)
        {
            Mesh mesh = new Mesh { name = name };
            Vector3[] v =
            {
                new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,-0.5f), new Vector3(0.5f,0.5f,-0.5f), new Vector3(-0.5f,0.5f,-0.5f),
                new Vector3(-0.5f,-0.5f,0.5f), new Vector3(0.5f,-0.5f,0.5f), new Vector3(0.5f,0.5f,0.5f), new Vector3(-0.5f,0.5f,0.5f)
            };
            int[] t = { 0, 2, 1, 0, 3, 2, 1, 6, 5, 1, 2, 6, 5, 7, 4, 5, 6, 7, 4, 3, 0, 4, 7, 3, 3, 6, 2, 3, 7, 6, 4, 1, 5, 4, 0, 1 };
            mesh.vertices = v;
            mesh.triangles = t;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildQuadMesh(string name)
        {
            Mesh mesh = new Mesh { name = name };
            mesh.vertices = new[] { new Vector3(-0.5f, -0.5f, 0f), new Vector3(0.5f, -0.5f, 0f), new Vector3(0.5f, 0.5f, 0f), new Vector3(-0.5f, 0.5f, 0f) };
            mesh.triangles = new[] { 0, 2, 1, 0, 3, 2 };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildCylinderMesh(string name, int segments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int i = 0; i < segments; i++)
            {
                float a = i * Mathf.PI * 2f / segments;
                vertices.Add(new Vector3(Mathf.Cos(a) * 0.5f, -0.5f, Mathf.Sin(a) * 0.5f));
                vertices.Add(new Vector3(Mathf.Cos(a) * 0.5f, 0.5f, Mathf.Sin(a) * 0.5f));
            }
            int bottomCenter = vertices.Count;
            vertices.Add(new Vector3(0f, -0.5f, 0f));
            int topCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0.5f, 0f));
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;
                int b0 = i * 2;
                int t0 = b0 + 1;
                int b1 = next * 2;
                int t1 = b1 + 1;
                triangles.AddRange(new[] { b0, t0, t1, b0, t1, b1, bottomCenter, b1, b0, topCenter, t0, t1 });
            }
            Mesh mesh = new Mesh { name = name };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildRingMesh(string name, int segments, float inner, float outer)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int i = 0; i < segments; i++)
            {
                float a = i * Mathf.PI * 2f / segments;
                vertices.Add(new Vector3(Mathf.Cos(a) * inner, Mathf.Sin(a) * inner, 0f));
                vertices.Add(new Vector3(Mathf.Cos(a) * outer, Mathf.Sin(a) * outer, 0f));
            }
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;
                int i0 = i * 2;
                int o0 = i0 + 1;
                int i1 = next * 2;
                int o1 = i1 + 1;
                triangles.AddRange(new[] { i0, o1, o0, i0, i1, o1 });
            }
            Mesh mesh = new Mesh { name = name };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildGearMesh(string name, int teeth, float inner, float outer)
        {
            int segments = teeth * 2;
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int i = 0; i < segments; i++)
            {
                float a = i * Mathf.PI * 2f / segments;
                float r = i % 2 == 0 ? outer : outer * 0.82f;
                vertices.Add(new Vector3(Mathf.Cos(a) * inner, Mathf.Sin(a) * inner, 0f));
                vertices.Add(new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, 0f));
            }
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;
                int i0 = i * 2;
                int o0 = i0 + 1;
                int i1 = next * 2;
                int o1 = i1 + 1;
                triangles.AddRange(new[] { i0, o1, o0, i0, i1, o1 });
            }
            Mesh mesh = new Mesh { name = name };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildNeedleMesh(string name)
        {
            Mesh mesh = new Mesh { name = name };
            mesh.vertices = new[] { new Vector3(-0.035f, -0.05f, 0f), new Vector3(0.035f, -0.05f, 0f), new Vector3(0.0f, 0.43f, 0f), new Vector3(-0.06f, -0.10f, 0f), new Vector3(0.06f, -0.10f, 0f), new Vector3(0f, -0.18f, 0f) };
            mesh.triangles = new[] { 0, 2, 1, 3, 4, 5 };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildTaperedGripMesh(string name)
        {
            Mesh mesh = new Mesh { name = name };
            Vector3[] v =
            {
                new Vector3(-0.38f,-0.5f,-0.28f), new Vector3(0.38f,-0.5f,-0.28f), new Vector3(0.38f,-0.5f,0.28f), new Vector3(-0.38f,-0.5f,0.28f),
                new Vector3(-0.24f,0.5f,-0.20f), new Vector3(0.24f,0.5f,-0.20f), new Vector3(0.24f,0.5f,0.20f), new Vector3(-0.24f,0.5f,0.20f)
            };
            int[] t = { 0, 1, 5, 0, 5, 4, 1, 2, 6, 1, 6, 5, 2, 3, 7, 2, 7, 6, 3, 0, 4, 3, 4, 7, 4, 5, 6, 4, 6, 7, 3, 2, 1, 3, 1, 0 };
            mesh.vertices = v;
            mesh.triangles = t;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildLeverPaddleMesh(string name)
        {
            Mesh mesh = new Mesh { name = name };
            mesh.vertices = new[] { new Vector3(-0.5f, 0f, 0f), new Vector3(0f, 0.20f, 0f), new Vector3(0.5f, 0f, 0f), new Vector3(0f, -0.20f, 0f), new Vector3(0f, 0f, -0.08f) };
            mesh.triangles = new[] { 0, 1, 4, 1, 2, 4, 2, 3, 4, 3, 0, 4, 0, 3, 2, 0, 2, 1 };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildRailFinMesh(string name)
        {
            Mesh mesh = new Mesh { name = name };
            mesh.vertices = new[] { new Vector3(-0.5f, -0.12f, -0.5f), new Vector3(0.5f, -0.12f, -0.5f), new Vector3(0f, 0.18f, -0.5f), new Vector3(-0.5f, -0.12f, 0.5f), new Vector3(0.5f, -0.12f, 0.5f), new Vector3(0f, 0.18f, 0.5f) };
            mesh.triangles = new[] { 0, 2, 1, 3, 4, 5, 0, 3, 5, 0, 5, 2, 1, 2, 5, 1, 5, 4, 0, 1, 4, 0, 4, 3 };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildGlovePalmMesh(string name)
        {
            Mesh mesh = BuildTaperedGripMesh(name);
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x *= 1.1f;
                vertices[i].y *= 0.55f;
                vertices[i].z *= 1.25f;
            }
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void RenderFamily(string key, string label, MechanismSpec[] specs, Vector3 cameraOffset, Vector3 cameraEuler, string outputRoot)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientLight = new Color(0.25f, 0.21f, 0.16f);
            AddPreviewLights();

            int columns = Mathf.CeilToInt(Mathf.Sqrt(specs.Length));
            float spacingX = 1.75f;
            float spacingY = 1.25f;
            for (int i = 0; i < specs.Length; i++)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageRoot + "/Runtime/Prefabs/" + specs[i].fileName);
                if (prefab == null) continue;
                GameObject instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = Path.GetFileNameWithoutExtension(specs[i].fileName);
                int col = i % columns;
                int row = i / columns;
                instance.transform.position = new Vector3((col - (columns - 1) * 0.5f) * spacingX, -row * spacingY, 0f);
                instance.transform.rotation = Quaternion.Euler(0f, -24f, 0f);
                NormalizeInstance(instance, 1.08f);
            }

            GameObject title = new GameObject("preview_label_" + label.Replace(" ", "_"));
            title.transform.position = Vector3.up * 99f;

            Bounds bounds = CalculateSceneBounds();
            Camera camera = new GameObject("wms04_preview_camera").AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.035f, 0.030f, 0.025f);
            camera.fieldOfView = 35f;
            camera.transform.position = bounds.center + cameraOffset;
            camera.transform.rotation = Quaternion.Euler(cameraEuler);
            camera.transform.LookAt(bounds.center + Vector3.up * 0.06f);
            RenderCamera(camera, Path.Combine(outputRoot, "WMS04_PREVIEW_" + key + "_v0.1.45.png"));
            EditorSceneManager.CloseScene(scene, true);
        }

        private static void RenderMaterialSwatch(string outputRoot)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientLight = new Color(0.25f, 0.21f, 0.16f);
            AddPreviewLights();

            int i = 0;
            foreach (Material material in Materials.Values)
            {
                GameObject chip = new GameObject(material.name);
                chip.transform.position = new Vector3((i % 5 - 2f) * 0.72f, 1.2f - (i / 5) * 0.62f, 0f);
                chip.transform.localScale = Vector3.one * 0.34f;
                chip.AddComponent<MeshFilter>().sharedMesh = Meshes[i % 2 == 0 ? "WMS04_MESH_Cylinder32Unit" : "WMS04_MESH_BoxUnit"];
                chip.AddComponent<MeshRenderer>().sharedMaterial = material;
                if (i % 2 == 0) chip.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                i++;
            }

            Camera camera = new GameObject("wms04_material_camera").AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.035f, 0.030f, 0.025f);
            camera.transform.position = new Vector3(0f, -0.35f, -5.6f);
            camera.transform.LookAt(Vector3.zero);
            camera.fieldOfView = 34f;
            RenderCamera(camera, Path.Combine(outputRoot, "WMS04_PREVIEW_material_swatch_all_v0.1.45.png"));
            EditorSceneManager.CloseScene(scene, true);
        }

        private static void AddPreviewLights()
        {
            Light key = new GameObject("warm_lantern_key").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = new Color(1.0f, 0.72f, 0.42f);
            key.intensity = 2.5f;
            key.transform.rotation = Quaternion.Euler(43f, -36f, 0f);

            Light rim = new GameObject("cool_edge_rim").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.color = new Color(0.32f, 0.58f, 0.72f);
            rim.intensity = 2.1f;
            rim.range = 7f;
            rim.transform.position = new Vector3(-2.7f, 1.9f, 1.8f);

            Light fill = new GameObject("amber_low_fill").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(1.0f, 0.43f, 0.16f);
            fill.intensity = 0.9f;
            fill.range = 5f;
            fill.transform.position = new Vector3(2.6f, -0.4f, -1.6f);
        }

        private static void RenderCamera(Camera camera, string path)
        {
            RenderTexture rt = new RenderTexture(1600, 1000, 24, RenderTextureFormat.ARGB32) { antiAliasing = 4 };
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D image = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            image.Apply();
            File.WriteAllBytes(path, image.EncodeToPNG());
            RenderTexture.active = previous;
            camera.targetTexture = null;
            UnityEngine.Object.DestroyImmediate(image);
            rt.Release();
            UnityEngine.Object.DestroyImmediate(rt);
        }

        private static void NormalizeInstance(GameObject instance, float targetMaxDimension)
        {
            Bounds bounds = CalculateBounds(instance);
            float max = Mathf.Max(0.01f, Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z)));
            float scale = targetMaxDimension / max;
            instance.transform.localScale *= scale;
        }

        private static Bounds CalculateSceneBounds()
        {
            Renderer[] renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Exclude);
            if (renderers.Length == 0) return new Bounds(Vector3.zero, Vector3.one);
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
            return bounds;
        }

        private static Bounds CalculateBounds(GameObject root)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0) return new Bounds(root.transform.position, Vector3.one);
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) bounds.Encapsulate(renderers[i].bounds);
            return bounds;
        }

        private static int WriteUnityValidationReport()
        {
            List<string> findings = new List<string>();
            int prefabCount = 0;
            int rendererCount = 0;
            foreach (MechanismSpec spec in Specs())
            {
                string path = PackageRoot + "/Runtime/Prefabs/" + spec.fileName;
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    findings.Add("missing_prefab:" + path);
                    continue;
                }
                prefabCount++;
                rendererCount += prefab.GetComponentsInChildren<Renderer>(true).Length;
                if (prefab.GetComponentsInChildren<Collider>(true).Length > 0) findings.Add("collider_present:" + path);
                if (prefab.GetComponentsInChildren<Rigidbody>(true).Length > 0) findings.Add("rigidbody_present:" + path);
                if (prefab.GetComponentsInChildren<AudioSource>(true).Length > 0) findings.Add("audio_source_present:" + path);
                if (prefab.GetComponentsInChildren<MonoBehaviour>(true).Length > 0) findings.Add("monobehaviour_present:" + path);
            }

            int materialCount = CountFiles(FullPath(PackageRoot + "/Runtime/Materials"), "*.mat");
            int meshCount = CountFiles(FullPath(PackageRoot + "/Runtime/Meshes"), "*.asset");
            int previewCount = CountFiles(ResolveRenderOutputRoot(), "WMS04_*.png");
            if (prefabCount < 22) findings.Add("prefab_count_below_target:" + prefabCount);
            if (materialCount < 14) findings.Add("material_count_below_target:" + materialCount);
            if (meshCount < 8) findings.Add("mesh_count_below_target:" + meshCount);
            if (previewCount < 10) findings.Add("preview_count_below_target:" + previewCount);

            string productionRoot = ResolveProductionOutputRoot();
            Directory.CreateDirectory(productionRoot);
            File.WriteAllText(Path.Combine(productionRoot, "WMS04_UnityValidationReport_v0.1.45.json"), BuildValidationJson(findings.Count == 0, prefabCount, materialCount, meshCount, rendererCount, previewCount, findings), Encoding.UTF8);
            File.WriteAllText(Path.Combine(productionRoot, "WMS04_AcceptanceReport_v0.1.45.md"), BuildAcceptanceMarkdown(findings.Count == 0, prefabCount, materialCount, meshCount, rendererCount, previewCount, findings), Encoding.UTF8);

            if (findings.Count == 0)
            {
                Debug.Log("WMS04_UNITY_VALIDATION_PASS v0.1.45 prefabs=" + prefabCount + " materials=" + materialCount + " meshes=" + meshCount + " renderers=" + rendererCount + " previews=" + previewCount);
                return 0;
            }

            Debug.LogError("WMS04_UNITY_VALIDATION_FAIL v0.1.45 findings=" + findings.Count);
            return 1;
        }

        private static int CountFiles(string root, string pattern)
        {
            return Directory.Exists(root) ? Directory.GetFiles(root, pattern).Length : 0;
        }

        private static void WriteMetadata(string importStatus)
        {
            File.WriteAllText(FullPath(PackageRoot + "/Documentation~/Manifest/" + ManifestFileName), BuildManifest(importStatus), Encoding.UTF8);
            File.WriteAllText(FullPath(PackageRoot + "/Runtime/Metadata/" + CatalogFileName), BuildCatalog(), Encoding.UTF8);
            Directory.CreateDirectory(ResolveProductionOutputRoot());
            File.WriteAllText(Path.Combine(ResolveProductionOutputRoot(), "WMS04_ProductionReport_v0.1.45.md"), BuildProductionReport(importStatus), Encoding.UTF8);
            AssetDatabase.ImportAsset(PackageRoot + "/Documentation~/Manifest/" + ManifestFileName);
            AssetDatabase.ImportAsset(PackageRoot + "/Runtime/Metadata/" + CatalogFileName);
        }

        private static string BuildManifest(string importStatus)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            AddJson(json, "pack_id", PackId, 1, true);
            AddJson(json, "display_name", "Weapon Mechanisms Set 04", 1, true);
            AddJson(json, "version", Version, 1, true);
            AddJson(json, "build_id", BuildId, 1, true);
            AddJson(json, "unity_version", Application.unityVersion, 1, true);
            AddJson(json, "generated_at_utc", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), 1, true);
            AddJson(json, "sidecar_project", "UD-SC-WPN-WeaponMechanismsSet04", 1, true);
            AddJson(json, "owner_lane", "sidecar-weapon-mechanisms-set04", 1, true);
            AddJson(json, "primary_intake_owner", "main-lane-art-integration", 1, true);
            AddJson(json, "canonical_root", "AssetPacks/BrassworksBreach.WeaponMechanismsSet04", 1, true);
            AddJson(json, "package_root", "AssetPacks/BrassworksBreach.WeaponMechanismsSet04", 1, true);
            AddJson(json, "package_name", PackageName, 1, true);
            json.AppendLine("  \"asset_counts\": {");
            json.AppendLine("    \"generated_prefabs\": " + Specs().Length + ",");
            json.AppendLine("    \"generated_materials\": " + Materials.Count + ",");
            json.AppendLine("    \"generated_meshes\": " + Meshes.Count + ",");
            json.AppendLine("    \"textures\": 0,");
            json.AppendLine("    \"audio\": 0,");
            json.AppendLine("    \"vfx\": 0,");
            json.AppendLine("    \"animation_clips\": 0,");
            json.AppendLine("    \"preview_renders\": 11");
            json.AppendLine("  },");
            AddArray(json, "generated_prefabs", PrefabPaths(), 1, true);
            AddArray(json, "generated_materials", AssetPaths("Runtime/Materials", ".mat"), 1, true);
            AddArray(json, "generated_meshes", AssetPaths("Runtime/Meshes", ".asset"), 1, true);
            AddArray(json, "preview_renders", PreviewPaths(), 1, true);
            json.AppendLine("  \"dependencies\": [],");
            json.AppendLine("  \"required_primary_changes\": [],");
            json.AppendLine("  \"path_collisions_checked\": true,");
            json.AppendLine("  \"guid_collisions_checked\": true,");
            AddJson(json, "import_smoke_status", importStatus, 1, true);
            json.AppendLine("  \"known_risks\": [");
            json.AppendLine("    \"Visual-only component set; final weapon silhouette, sockets, hand alignment, animation, muzzle placement, and gameplay authority remain with the primary lane.\",");
            json.AppendLine("    \"Materials are procedural Unity material candidates and still need final authored texture maps for AAA close inspection.\",");
            json.AppendLine("    \"Component pieces are deliberately modular, so final assembled weapons require art direction pass and first-person camera framing.\"");
            json.AppendLine("  ],");
            AddJson(json, "rollback_path", "delete isolated package root or remove local package reference", 1, true);
            AddJson(json, "decision", "ready_for_primary_quarantine_after_static_validation_and_preview_review", 1, false);
            json.AppendLine("}");
            return json.ToString();
        }

        private static string BuildCatalog()
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            AddJson(json, "catalog_id", "WMS04_RuntimeCatalog_v0.1.45", 1, true);
            AddJson(json, "package_name", PackageName, 1, true);
            json.AppendLine("  \"prefabs\": [");
            MechanismSpec[] specs = Specs();
            for (int i = 0; i < specs.Length; i++)
            {
                MechanismSpec spec = specs[i];
                json.AppendLine("    {");
                AddJson(json, "file", "Packages/" + PackageName + "/Runtime/Prefabs/" + spec.fileName, 3, true);
                AddJson(json, "family", spec.family, 3, true);
                AddJson(json, "description", spec.description, 3, true);
                AddJson(json, "visual_only", "true", 3, true, true);
                json.AppendLine("      \"bounds_meters\": [" + spec.bounds.x.ToString("0.###", CultureInfo.InvariantCulture) + ", " + spec.bounds.y.ToString("0.###", CultureInfo.InvariantCulture) + ", " + spec.bounds.z.ToString("0.###", CultureInfo.InvariantCulture) + "]");
                json.Append("    }");
                json.AppendLine(i == specs.Length - 1 ? string.Empty : ",");
            }
            json.AppendLine("  ],");
            AddArray(json, "materials", AssetPaths("Runtime/Materials", ".mat"), 1, true);
            AddArray(json, "meshes", AssetPaths("Runtime/Meshes", ".asset"), 1, true);
            AddJson(json, "authority", "visual_only_no_gameplay_scripts_no_colliders_no_rigidbodies_no_audio", 1, false);
            json.AppendLine("}");
            return json.ToString();
        }

        private static string BuildProductionReport(string importStatus)
        {
            return "# Weapon Mechanisms Set 04 Production Report\n\n" +
                "- Package: `AssetPacks/BrassworksBreach.WeaponMechanismsSet04`\n" +
                "- Version: `0.1.45-p001`\n" +
                "- Prefabs: " + Specs().Length + "\n" +
                "- Materials: " + Materials.Count + "\n" +
                "- Reusable meshes: " + Meshes.Count + "\n" +
                "- Preview renders planned: 11\n" +
                "- Import smoke status: `" + importStatus + "`\n\n" +
                "Generated in Unity only. Prefabs are modular first-person weapon component lookdev assets with no gameplay scripts, colliders, rigidbodies, or autonomous audio.\n";
        }

        private static string BuildValidationJson(bool pass, int prefabs, int materials, int meshes, int renderers, int previews, IReadOnlyList<string> findings)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            AddJson(json, "pack_id", PackId, 1, true);
            AddJson(json, "version", Version, 1, true);
            AddJson(json, "generated_at_utc", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), 1, true);
            AddJson(json, "status", pass ? "pass" : "fail", 1, true);
            json.AppendLine("  \"counts\": {");
            json.AppendLine("    \"prefabs\": " + prefabs + ",");
            json.AppendLine("    \"materials\": " + materials + ",");
            json.AppendLine("    \"meshes\": " + meshes + ",");
            json.AppendLine("    \"renderers\": " + renderers + ",");
            json.AppendLine("    \"preview_renders\": " + previews);
            json.AppendLine("  },");
            json.AppendLine("  \"runtime_contract\": {");
            json.AppendLine("    \"visual_only\": true,");
            json.AppendLine("    \"colliders\": \"omitted\",");
            json.AppendLine("    \"rigidbodies\": \"omitted\",");
            json.AppendLine("    \"audio_sources\": \"omitted\",");
            json.AppendLine("    \"gameplay_scripts\": \"omitted\"");
            json.AppendLine("  },");
            AddArray(json, "findings", findings, 1, false);
            json.AppendLine("}");
            return json.ToString();
        }

        private static string BuildAcceptanceMarkdown(bool pass, int prefabs, int materials, int meshes, int renderers, int previews, IReadOnlyList<string> findings)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("# Weapon Mechanisms Set 04 Acceptance Report");
            report.AppendLine();
            report.AppendLine("- Status: `" + (pass ? "PASS" : "FAIL") + "`");
            report.AppendLine("- Prefabs: " + prefabs);
            report.AppendLine("- Materials: " + materials);
            report.AppendLine("- Reusable meshes: " + meshes);
            report.AppendLine("- Renderer components: " + renderers);
            report.AppendLine("- Unity-rendered previews: " + previews);
            report.AppendLine("- Runtime safety: visual-only, no colliders, rigidbodies, autonomous audio, or gameplay scripts.");
            report.AppendLine();
            report.AppendLine("## Findings");
            if (findings.Count == 0)
            {
                report.AppendLine("- None.");
            }
            else
            {
                foreach (string finding in findings) report.AppendLine("- " + finding);
            }
            report.AppendLine();
            report.AppendLine("## Unity Validation Result");
            report.AppendLine();
            report.AppendLine("`WMS04_UNITY_VALIDATION_" + (pass ? "PASS" : "FAIL") + " v0.1.45 prefabs=" + prefabs + " materials=" + materials + " meshes=" + meshes + " renderers=" + renderers + " previews=" + previews + "`");
            return report.ToString();
        }

        private static MechanismSpec[] Family(string family)
        {
            List<MechanismSpec> matches = new List<MechanismSpec>();
            foreach (MechanismSpec spec in Specs()) if (spec.family == family) matches.Add(spec);
            return matches.ToArray();
        }

        private static MechanismSpec[] Combine(params MechanismSpec[][] sets)
        {
            List<MechanismSpec> combined = new List<MechanismSpec>();
            foreach (MechanismSpec[] set in sets) combined.AddRange(set);
            return combined.ToArray();
        }

        private static MechanismSpec[] FirstHalf(MechanismSpec[] specs)
        {
            int count = specs.Length / 2;
            MechanismSpec[] result = new MechanismSpec[count];
            Array.Copy(specs, result, count);
            return result;
        }

        private static MechanismSpec[] SecondHalf(MechanismSpec[] specs)
        {
            int start = specs.Length / 2;
            int count = specs.Length - start;
            MechanismSpec[] result = new MechanismSpec[count];
            Array.Copy(specs, start, result, 0, count);
            return result;
        }

        private static string[] PrefabPaths()
        {
            List<string> paths = new List<string>();
            foreach (MechanismSpec spec in Specs()) paths.Add("Packages/" + PackageName + "/Runtime/Prefabs/" + spec.fileName);
            return paths.ToArray();
        }

        private static string[] AssetPaths(string folder, string extension)
        {
            List<string> paths = new List<string>();
            string full = FullPath(PackageRoot + "/" + folder);
            if (Directory.Exists(full))
            {
                foreach (string file in Directory.GetFiles(full, "*" + extension))
                {
                    paths.Add("Packages/" + PackageName + "/" + folder + "/" + Path.GetFileName(file));
                }
            }
            paths.Sort(StringComparer.Ordinal);
            return paths.ToArray();
        }

        private static string[] PreviewPaths()
        {
            return new[]
            {
                RenderOutputRelativePath + "/WMS04_PREVIEW_pressure_pistol_coils_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_gauge_dial_clusters_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_wood_leather_grips_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_receiver_plates_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_muzzle_tanks_valves_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_ammo_scatter_rails_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_glove_silhouette_pieces_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_material_swatch_prefabs_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_all_components_contact_sheet_a_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_all_components_contact_sheet_b_v0.1.45.png",
                RenderOutputRelativePath + "/WMS04_PREVIEW_material_swatch_all_v0.1.45.png"
            };
        }

        private static void AddJson(StringBuilder json, string key, string value, int indent, bool comma, bool raw = false)
        {
            json.Append(new string(' ', indent * 2));
            json.Append("\"").Append(key).Append("\": ");
            json.Append(raw ? value : "\"" + Escape(value) + "\"");
            json.AppendLine(comma ? "," : string.Empty);
        }

        private static void AddArray(StringBuilder json, string key, IEnumerable<string> values, int indent, bool comma)
        {
            string[] array = values is string[] existing ? existing : new List<string>(values).ToArray();
            json.Append(new string(' ', indent * 2)).Append("\"").Append(key).AppendLine("\": [");
            for (int i = 0; i < array.Length; i++)
            {
                json.Append(new string(' ', (indent + 1) * 2)).Append("\"").Append(Escape(array[i])).Append("\"").AppendLine(i == array.Length - 1 ? string.Empty : ",");
            }
            json.Append(new string(' ', indent * 2)).Append("]").AppendLine(comma ? "," : string.Empty);
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string PackageRoot { get { return "Packages/" + PackageName; } }

        private static string ProjectRoot
        {
            get
            {
                string validationProject = Directory.GetCurrentDirectory().Replace("\\", "/");
                string packageRoot = Path.GetFullPath(Path.Combine(validationProject, "..")).Replace("\\", "/");
                return Path.GetFullPath(Path.Combine(packageRoot, "../..")).Replace("\\", "/");
            }
        }

        private static string ResolveRenderOutputRoot()
        {
            return Path.Combine(ProjectRoot, RenderOutputRelativePath).Replace("\\", "/");
        }

        private static string ResolveProductionOutputRoot()
        {
            return Path.Combine(ProjectRoot, ProductionOutputRelativePath).Replace("\\", "/");
        }

        private static string FullPath(string assetPath)
        {
            string relative = assetPath.StartsWith(PackageRoot, StringComparison.Ordinal) ? assetPath.Substring(PackageRoot.Length).TrimStart('/') : assetPath;
            string validationProject = Directory.GetCurrentDirectory();
            string packageFull = Path.GetFullPath(Path.Combine(validationProject, ".."));
            return Path.Combine(packageFull, relative.Replace("/", Path.DirectorySeparatorChar.ToString()));
        }

        private sealed class MechanismSpec
        {
            public readonly string fileName;
            public readonly string family;
            public readonly string description;
            public readonly Vector3 bounds;
            public readonly Action<GameObject> builder;

            public MechanismSpec(string fileName, string family, string description, Vector3 bounds, Action<GameObject> builder)
            {
                this.fileName = fileName;
                this.family = family;
                this.description = description;
                this.bounds = bounds;
                this.builder = builder;
            }
        }
    }
}
#endif
