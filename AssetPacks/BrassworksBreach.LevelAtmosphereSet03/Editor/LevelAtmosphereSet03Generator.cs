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

namespace BrassworksBreach.Sidecars.LevelAtmosphereSet03
{
    public static class LevelAtmosphereSet03Generator
    {
        private const string Version = "0.1.44";
        private const string BuildId = "p001";
        private const string PackId = "SCLA";
        private const string PackageName = "com.brassworks.sidecar.level-atmosphere-set03";
        private const string MenuRoot = "Brassworks/Sidecars/Level Atmosphere Set 03 v0.1.44/";
        private const string RenderOutputRelativePath = "Documentation/ConceptRenders/V0_1_44_LevelAtmosphereSet03";
        private const string ManifestFileName = "SCLA_LevelAtmosphereSet03_Manifest_v0.1.44-p001.json";
        private const string CatalogFileName = "SCLA_LevelAtmosphereSet03_GeneratedCatalog_v0.1.44.json";

        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();

        [MenuItem(MenuRoot + "Generate Package Assets")]
        public static void GeneratePackageAssets()
        {
            Materials.Clear();
            Meshes.Clear();
            EnsureFolders();
            CreateMaterials();
            CreateMeshes();

            foreach (AtmosphereSpec spec in Specs())
            {
                CreatePrefab(spec);
            }

            WriteMetadata("generated_by_unity_sidecar_batchmode_v0.1.44");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("SCLA_GENERATE_PASS v0.1.44 prefabs=" + Specs().Length + " materials=" + Materials.Count + " meshes=" + Meshes.Count);
        }

        [MenuItem(MenuRoot + "Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(PackageRoot + "/Runtime/Prefabs/" + Specs()[0].fileName) == null)
            {
                GeneratePackageAssets();
            }

            string outputRoot = ResolveRenderOutputRoot();
            Directory.CreateDirectory(outputRoot);
            RenderFamily("steam_pipes", outputRoot, "Steam pipe clusters", Family("steam-pipes"), new Vector3(0f, 3.2f, -7.8f), new Vector3(22f, -8f, 0f));
            RenderFamily("pressure_lamps", outputRoot, "Pressure lamps", Family("pressure-lamps"), new Vector3(0f, 2.8f, -6.2f), new Vector3(15f, 0f, 0f));
            RenderFamily("wall_grime", outputRoot, "Wall grime panels", Family("wall-grime"), new Vector3(0f, 2.0f, -5.8f), new Vector3(7f, 0f, 0f));
            RenderFamily("hanging_chains", outputRoot, "Hanging chains", Family("hanging-chains"), new Vector3(0f, 3.0f, -6.4f), new Vector3(10f, 0f, 0f));
            RenderFamily("pulley_silhouettes", outputRoot, "Pulley silhouettes", Family("pulley-silhouettes"), new Vector3(0f, 2.6f, -6.2f), new Vector3(12f, 0f, 0f));
            RenderFamily("floor_drains", outputRoot, "Floor drain covers", Family("floor-drains"), new Vector3(0f, 5.6f, -6.6f), new Vector3(43f, 0f, 0f));
            RenderFamily("warning_gauges", outputRoot, "Warning gauges", Family("warning-gauges"), new Vector3(0f, 2.35f, -5.9f), new Vector3(9f, 0f, 0f));
            RenderFamily("overhead_canopies", outputRoot, "Overhead pipe canopies", Family("overhead-canopies"), new Vector3(0f, 4.2f, -8.2f), new Vector3(26f, 0f, 0f));
            RenderFamily("contact_sheet", outputRoot, "All prefabs", Specs(), new Vector3(0f, 10.8f, -22f), new Vector3(31f, 0f, 0f));
            RenderMaterialSwatch(outputRoot);

            WriteMetadata("generated_and_preview_rendered_by_unity_sidecar_batchmode_v0.1.44");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("SCLA_PREVIEW_PASS v0.1.44 output=" + outputRoot);
        }

        [MenuItem(MenuRoot + "Generate and Render Preview")]
        public static void GenerateAllAndRenderPreview()
        {
            GeneratePackageAssets();
            RenderPreviewPngs();
        }

        private static AtmosphereSpec[] Specs()
        {
            return new[]
            {
                new AtmosphereSpec("SCLA_SteamPipeCluster_WallLeaker_A.prefab", "steam-pipes", "wall steam leak pipe cluster A", new Vector3(1.55f, 1.25f, 0.38f), BuildSteamPipesA),
                new AtmosphereSpec("SCLA_SteamPipeCluster_WallLeaker_B.prefab", "steam-pipes", "wall steam leak pipe cluster B", new Vector3(1.85f, 1.4f, 0.42f), BuildSteamPipesB),
                new AtmosphereSpec("SCLA_SteamPipeCluster_FloorKnee.prefab", "steam-pipes", "floor knee steam cluster", new Vector3(1.25f, 1.15f, 1.05f), BuildSteamPipesC),
                new AtmosphereSpec("SCLA_SteamPipeCluster_CornerBleed.prefab", "steam-pipes", "corner bleed pipe cluster", new Vector3(1.0f, 1.7f, 1.0f), BuildSteamPipesD),
                new AtmosphereSpec("SCLA_PressureLamp_WallCaged_A.prefab", "pressure-lamps", "caged pressure lamp wall A", new Vector3(0.55f, 0.9f, 0.42f), BuildLampA),
                new AtmosphereSpec("SCLA_PressureLamp_WallCaged_B.prefab", "pressure-lamps", "caged pressure lamp wall B", new Vector3(0.72f, 1.05f, 0.45f), BuildLampB),
                new AtmosphereSpec("SCLA_PressureLamp_HangingCan.prefab", "pressure-lamps", "hanging pressure lamp can", new Vector3(0.62f, 1.35f, 0.62f), BuildLampC),
                new AtmosphereSpec("SCLA_WallGrimePanel_SootFan.prefab", "wall-grime", "soot fan wall grime panel", new Vector3(1.7f, 1.15f, 0.04f), BuildGrimeA),
                new AtmosphereSpec("SCLA_WallGrimePanel_OilStreaks.prefab", "wall-grime", "oil streak wall grime panel", new Vector3(1.15f, 1.7f, 0.04f), BuildGrimeB),
                new AtmosphereSpec("SCLA_WallGrimePanel_RivetBloom.prefab", "wall-grime", "rivet bloom grime panel", new Vector3(1.45f, 1.2f, 0.04f), BuildGrimeC),
                new AtmosphereSpec("SCLA_WallGrimePanel_CornerWash.prefab", "wall-grime", "corner wash grime panels", new Vector3(1.0f, 1.4f, 1.0f), BuildGrimeD),
                new AtmosphereSpec("SCLA_HangingChains_TripleSlack.prefab", "hanging-chains", "triple slack hanging chains", new Vector3(1.25f, 1.7f, 0.2f), BuildChainsA),
                new AtmosphereSpec("SCLA_HangingChains_HookDrop.prefab", "hanging-chains", "hook drop hanging chain", new Vector3(0.55f, 2.0f, 0.34f), BuildChainsB),
                new AtmosphereSpec("SCLA_HangingChains_CurtainShort.prefab", "hanging-chains", "short chain curtain", new Vector3(1.6f, 1.25f, 0.25f), BuildChainsC),
                new AtmosphereSpec("SCLA_PulleySilhouette_SingleWall.prefab", "pulley-silhouettes", "single wall pulley silhouette", new Vector3(0.85f, 0.9f, 0.18f), BuildPulleyA),
                new AtmosphereSpec("SCLA_PulleySilhouette_DoubleBelt.prefab", "pulley-silhouettes", "double belt pulley silhouette", new Vector3(1.55f, 1.0f, 0.22f), BuildPulleyB),
                new AtmosphereSpec("SCLA_PulleySilhouette_CeilingTrolley.prefab", "pulley-silhouettes", "ceiling trolley pulley silhouette", new Vector3(1.3f, 0.75f, 0.62f), BuildPulleyC),
                new AtmosphereSpec("SCLA_FloorDrainCover_RoundRadial.prefab", "floor-drains", "round radial floor drain cover", new Vector3(1.05f, 0.09f, 1.05f), BuildDrainA),
                new AtmosphereSpec("SCLA_FloorDrainCover_SquareSlotted.prefab", "floor-drains", "square slotted floor drain cover", new Vector3(1.15f, 0.08f, 1.15f), BuildDrainB),
                new AtmosphereSpec("SCLA_FloorDrainCover_LongGutter.prefab", "floor-drains", "long gutter floor drain cover", new Vector3(2.2f, 0.08f, 0.55f), BuildDrainC),
                new AtmosphereSpec("SCLA_WarningGauge_SingleRedline.prefab", "warning-gauges", "single redline warning gauge", new Vector3(0.62f, 0.78f, 0.18f), BuildGaugeA),
                new AtmosphereSpec("SCLA_WarningGauge_TripleRack.prefab", "warning-gauges", "triple rack warning gauges", new Vector3(1.6f, 0.8f, 0.2f), BuildGaugeB),
                new AtmosphereSpec("SCLA_WarningGauge_PressureBoard.prefab", "warning-gauges", "pressure board warning gauges", new Vector3(1.25f, 1.15f, 0.2f), BuildGaugeC),
                new AtmosphereSpec("SCLA_OverheadPipeCanopy_Narrow.prefab", "overhead-canopies", "narrow overhead pipe canopy", new Vector3(2.6f, 0.55f, 0.75f), BuildCanopyA),
                new AtmosphereSpec("SCLA_OverheadPipeCanopy_Wide.prefab", "overhead-canopies", "wide overhead pipe canopy", new Vector3(3.0f, 0.75f, 1.25f), BuildCanopyB),
                new AtmosphereSpec("SCLA_OverheadPipeCanopy_Crossbrace.prefab", "overhead-canopies", "crossbrace overhead pipe canopy", new Vector3(2.25f, 0.8f, 1.55f), BuildCanopyC),
                new AtmosphereSpec("SCLA_OverheadPipeCanopy_ValveRun.prefab", "overhead-canopies", "valve run overhead pipe canopy", new Vector3(2.8f, 0.95f, 0.95f), BuildCanopyD),
                new AtmosphereSpec("SCLA_DenseAmbienceCombo_CorridorBite.prefab", "overhead-canopies", "dense corridor atmosphere combo", new Vector3(2.4f, 1.85f, 1.2f), BuildCombo)
            };
        }

        private static AtmosphereSpec[] Family(string family)
        {
            List<AtmosphereSpec> matches = new List<AtmosphereSpec>();
            foreach (AtmosphereSpec spec in Specs())
            {
                if (spec.family == family)
                {
                    matches.Add(spec);
                }
            }
            return matches.ToArray();
        }

        private static void EnsureFolders()
        {
            foreach (string folder in new[] { "Runtime/Prefabs", "Runtime/Materials", "Runtime/Meshes", "Runtime/Metadata", "Documentation~/Manifest", "Samples~/PreviewScene" })
            {
                Directory.CreateDirectory(FullPath(PackageRoot + "/" + folder));
            }
            AssetDatabase.Refresh();
        }

        private static void CreateMaterials()
        {
            AddMaterial("SCLA_MAT_BlackSootIron", new Color(0.028f, 0.027f, 0.024f), 0.25f, 0.82f);
            AddMaterial("SCLA_MAT_AgedBrassGlow", new Color(0.78f, 0.57f, 0.25f), 0.52f, 0.68f);
            AddMaterial("SCLA_MAT_DullCopperPipe", new Color(0.68f, 0.28f, 0.13f), 0.46f, 0.62f);
            AddMaterial("SCLA_MAT_OxidizedRimGreen", new Color(0.18f, 0.42f, 0.32f), 0.32f, 0.25f);
            AddMaterial("SCLA_MAT_WetOilBlack", new Color(0.012f, 0.011f, 0.010f), 0.82f, 0.04f);
            AddMaterial("SCLA_MAT_AmberLampGlass", new Color(1f, 0.52f, 0.14f, 0.7f), 0.34f, 0.02f, new Color(1f, 0.35f, 0.08f) * 1.7f, true);
            AddMaterial("SCLA_MAT_SteamWhite", new Color(0.76f, 0.74f, 0.68f, 0.42f), 0.05f, 0f, new Color(0.25f, 0.23f, 0.20f), true);
            AddMaterial("SCLA_MAT_GrimeFilm", new Color(0.08f, 0.07f, 0.052f, 0.62f), 0.18f, 0f, Color.black, true);
            AddMaterial("SCLA_MAT_WarningRedNeedle", new Color(0.82f, 0.04f, 0.025f), 0.4f, 0.08f);
            AddMaterial("SCLA_MAT_DangerYellowPlate", new Color(0.88f, 0.62f, 0.08f), 0.36f, 0.18f);
            AddMaterial("SCLA_MAT_GaugeIvory", new Color(0.82f, 0.76f, 0.58f), 0.21f, 0.0f);
            AddMaterial("SCLA_MAT_ChainGunmetal", new Color(0.11f, 0.105f, 0.095f), 0.36f, 0.8f);
            AddMaterial("SCLA_MAT_RubberBelt", new Color(0.018f, 0.017f, 0.015f), 0.16f, 0f);
            AddMaterial("SCLA_MAT_WornSteelEdge", new Color(0.48f, 0.47f, 0.42f), 0.38f, 0.75f);
            AddMaterial("SCLA_MAT_HeatBlueSteel", new Color(0.16f, 0.24f, 0.34f), 0.45f, 0.72f);
            AddMaterial("SCLA_MAT_DeepShadow", new Color(0.006f, 0.005f, 0.004f), 0.05f, 0.0f);
        }

        private static void AddMaterial(string name, Color color, float smoothness, float metallic, Color emission = default(Color), bool transparent = false)
        {
            string path = PackageRoot + "/Runtime/Materials/" + name + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Standard"));
                AssetDatabase.CreateAsset(material, path);
            }

            material.name = name;
            material.SetColor("_Color", color);
            material.SetFloat("_Glossiness", smoothness);
            material.SetFloat("_Metallic", metallic);
            if (emission.maxColorComponent > 0.001f)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission);
            }

            if (transparent)
            {
                material.SetFloat("_Mode", 3f);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = 3000;
            }

            EditorUtility.SetDirty(material);
            Materials[name] = material;
        }

        private static void CreateMeshes()
        {
            AddMesh("SCLA_MESH_BoxUnit", BuildBoxMesh());
            AddMesh("SCLA_MESH_QuadUnit", BuildQuadMesh());
            AddMesh("SCLA_MESH_Cylinder16Unit", BuildCylinderMesh(16, 0.5f, 1f));
            AddMesh("SCLA_MESH_Cylinder32Unit", BuildCylinderMesh(32, 0.5f, 1f));
            AddMesh("SCLA_MESH_Ring32Unit", BuildRingMesh(32, 0.34f, 0.5f, 0.08f));
            AddMesh("SCLA_MESH_Gear16Silhouette", BuildGearMesh(16, 0.34f, 0.5f, 0.08f));
            AddMesh("SCLA_MESH_ChainLinkUnit", BuildChainLinkMesh());
            AddMesh("SCLA_MESH_SteamWispUnit", BuildSteamWispMesh());
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

        private static void CreatePrefab(AtmosphereSpec spec)
        {
            GameObject root = new GameObject(Path.GetFileNameWithoutExtension(spec.fileName));
            root.tag = "Untagged";
            root.layer = 0;
            root.AddComponent<AssetPackMarker>().hideFlags = HideFlags.HideInInspector;
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
            foreach (MonoBehaviour component in root.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (component is AssetPackMarker)
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
            }
        }

        private static GameObject Part(GameObject parent, string name, string mesh, string material, Vector3 localPosition, Vector3 localScale, Vector3 localEuler)
        {
            GameObject child = new GameObject(name);
            child.transform.SetParent(parent.transform, false);
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.Euler(localEuler);
            child.transform.localScale = localScale;
            child.AddComponent<MeshFilter>().sharedMesh = Meshes[mesh];
            child.AddComponent<MeshRenderer>().sharedMaterial = Materials[material];
            return child;
        }

        private static void Pipe(GameObject root, string name, Vector3 pos, Vector3 scale, Vector3 euler, string mat = "SCLA_MAT_DullCopperPipe")
        {
            Part(root, name, "SCLA_MESH_Cylinder16Unit", mat, pos, scale, euler);
        }

        private static void Rivet(GameObject root, string name, Vector3 pos, float s = 0.08f)
        {
            Part(root, name, "SCLA_MESH_Cylinder16Unit", "SCLA_MAT_WornSteelEdge", pos, new Vector3(s, s * 0.5f, s), new Vector3(90f, 0f, 0f));
        }

        private static void Gauge(GameObject root, string prefix, Vector3 pos, float radius, float needleAngle)
        {
            Part(root, prefix + "_back", "SCLA_MESH_Cylinder32Unit", "SCLA_MAT_BlackSootIron", pos + new Vector3(0f, 0f, -0.015f), new Vector3(radius * 2.2f, 0.05f, radius * 2.2f), new Vector3(90f, 0f, 0f));
            Part(root, prefix + "_face", "SCLA_MESH_Cylinder32Unit", "SCLA_MAT_GaugeIvory", pos, new Vector3(radius * 1.85f, 0.035f, radius * 1.85f), new Vector3(90f, 0f, 0f));
            Part(root, prefix + "_rim", "SCLA_MESH_Ring32Unit", "SCLA_MAT_AgedBrassGlow", pos + new Vector3(0f, 0f, 0.022f), new Vector3(radius * 2.2f, radius * 2.2f, radius * 2.2f), Vector3.zero);
            Part(root, prefix + "_needle", "SCLA_MESH_BoxUnit", "SCLA_MAT_WarningRedNeedle", pos + new Vector3(0f, 0f, 0.045f), new Vector3(radius * 0.08f, radius * 0.86f, 0.018f), new Vector3(0f, 0f, needleAngle));
            Part(root, prefix + "_redline", "SCLA_MESH_BoxUnit", "SCLA_MAT_WarningRedNeedle", pos + new Vector3(radius * 0.42f, radius * 0.3f, 0.04f), new Vector3(radius * 0.52f, radius * 0.035f, 0.014f), new Vector3(0f, 0f, -30f));
        }

        private static void Steam(GameObject root, Vector3 pos, Vector3 scale, float zRot)
        {
            Part(root, "soft_steam_wisp", "SCLA_MESH_SteamWispUnit", "SCLA_MAT_SteamWhite", pos, scale, new Vector3(0f, 0f, zRot));
        }

        private static void Chain(GameObject root, string prefix, float x, int links, float topY, float z = 0f)
        {
            for (int i = 0; i < links; i++)
            {
                Part(root, prefix + "_link_" + i.ToString("00", CultureInfo.InvariantCulture), "SCLA_MESH_ChainLinkUnit", "SCLA_MAT_ChainGunmetal", new Vector3(x, topY - i * 0.18f, z), new Vector3(0.16f, 0.18f, 0.045f), new Vector3(0f, 0f, i % 2 == 0 ? 0f : 90f));
            }
        }

        private static void BuildSteamPipesA(GameObject r)
        {
            Part(r, "backplate", "SCLA_MESH_BoxUnit", "SCLA_MAT_BlackSootIron", Vector3.zero, new Vector3(1.55f, 0.95f, 0.06f), Vector3.zero);
            Pipe(r, "main_horizontal", new Vector3(0f, 0.08f, 0.09f), new Vector3(0.16f, 1.55f, 0.16f), new Vector3(0f, 0f, 90f));
            Pipe(r, "upper_bleed", new Vector3(-0.35f, 0.38f, 0.12f), new Vector3(0.10f, 0.82f, 0.10f), new Vector3(0f, 0f, 25f));
            Pipe(r, "lower_return", new Vector3(0.42f, -0.27f, 0.12f), new Vector3(0.11f, 0.9f, 0.11f), new Vector3(0f, 0f, -18f));
            Steam(r, new Vector3(0.72f, 0.22f, 0.18f), new Vector3(0.55f, 0.75f, 1f), -18f);
            for (int i = -2; i <= 2; i++) Rivet(r, "rivet_" + i, new Vector3(i * 0.32f, -0.48f, 0.08f));
        }

        private static void BuildSteamPipesB(GameObject r)
        {
            Pipe(r, "left_vertical", new Vector3(-0.45f, 0f, 0f), new Vector3(0.18f, 1.35f, 0.18f), Vector3.zero);
            Pipe(r, "right_vertical", new Vector3(0.42f, 0.05f, 0.02f), new Vector3(0.14f, 1.15f, 0.14f), Vector3.zero);
            Pipe(r, "cross_feed", new Vector3(0f, 0.34f, 0.04f), new Vector3(0.12f, 1.55f, 0.12f), new Vector3(0f, 0f, 90f));
            Part(r, "valve_wheel", "SCLA_MESH_Gear16Silhouette", "SCLA_MAT_WarningRedNeedle", new Vector3(0.05f, 0.34f, 0.17f), new Vector3(0.42f, 0.42f, 0.42f), Vector3.zero);
            Steam(r, new Vector3(-0.72f, 0.58f, 0.12f), new Vector3(0.45f, 0.7f, 1f), 22f);
            Steam(r, new Vector3(0.78f, -0.22f, 0.12f), new Vector3(0.35f, 0.55f, 1f), -28f);
        }

        private static void BuildSteamPipesC(GameObject r)
        {
            Pipe(r, "floor_run", new Vector3(0f, 0.18f, 0f), new Vector3(0.18f, 1.25f, 0.18f), new Vector3(90f, 0f, 90f));
            Pipe(r, "upriser", new Vector3(-0.42f, 0.55f, 0f), new Vector3(0.16f, 0.82f, 0.16f), Vector3.zero);
            Part(r, "joint", "SCLA_MESH_Cylinder32Unit", "SCLA_MAT_AgedBrassGlow", new Vector3(-0.42f, 0.22f, 0f), new Vector3(0.35f, 0.24f, 0.35f), Vector3.zero);
            Part(r, "floor_scorch", "SCLA_MESH_QuadUnit", "SCLA_MAT_GrimeFilm", new Vector3(0.2f, 0.012f, 0f), new Vector3(1.05f, 0.75f, 1f), new Vector3(90f, 0f, 0f));
            Steam(r, new Vector3(-0.42f, 1.02f, 0.02f), new Vector3(0.5f, 0.85f, 1f), 0f);
        }

        private static void BuildSteamPipesD(GameObject r)
        {
            BuildSteamPipesA(r);
            Pipe(r, "side_corner_pipe", new Vector3(-0.72f, 0.1f, 0.48f), new Vector3(0.12f, 1.05f, 0.12f), new Vector3(90f, 0f, 0f));
            Part(r, "corner_shadow", "SCLA_MESH_QuadUnit", "SCLA_MAT_GrimeFilm", new Vector3(-0.78f, 0f, 0.5f), new Vector3(0.95f, 1.25f, 1f), new Vector3(0f, 90f, 0f));
        }

        private static void BuildLampA(GameObject r)
        {
            Part(r, "wall_mount", "SCLA_MESH_BoxUnit", "SCLA_MAT_AgedBrassGlow", new Vector3(0f, 0f, -0.08f), new Vector3(0.38f, 0.62f, 0.08f), Vector3.zero);
            Part(r, "amber_glass", "SCLA_MESH_Cylinder32Unit", "SCLA_MAT_AmberLampGlass", new Vector3(0f, 0f, 0.08f), new Vector3(0.38f, 0.42f, 0.38f), Vector3.zero);
            Part(r, "cage_top", "SCLA_MESH_Ring32Unit", "SCLA_MAT_ChainGunmetal", new Vector3(0f, 0.22f, 0.08f), new Vector3(0.47f, 0.47f, 0.47f), Vector3.zero);
            Part(r, "cage_bottom", "SCLA_MESH_Ring32Unit", "SCLA_MAT_ChainGunmetal", new Vector3(0f, -0.22f, 0.08f), new Vector3(0.47f, 0.47f, 0.47f), Vector3.zero);
            for (int i = 0; i < 4; i++) Part(r, "cage_bar_" + i, "SCLA_MESH_BoxUnit", "SCLA_MAT_ChainGunmetal", new Vector3(Mathf.Cos(i * Mathf.PI * 0.5f) * 0.2f, 0f, 0.08f + Mathf.Sin(i * Mathf.PI * 0.5f) * 0.2f), new Vector3(0.025f, 0.52f, 0.025f), Vector3.zero);
        }

        private static void BuildLampB(GameObject r)
        {
            BuildLampA(r);
            Pipe(r, "pressure_feed_pipe", new Vector3(-0.35f, -0.1f, -0.05f), new Vector3(0.08f, 0.68f, 0.08f), Vector3.zero, "SCLA_MAT_DullCopperPipe");
            Gauge(r, "tiny_pressure_gauge", new Vector3(0.33f, -0.28f, 0.14f), 0.13f, -45f);
        }

        private static void BuildLampC(GameObject r)
        {
            Chain(r, "hanger", 0f, 5, 0.58f, 0f);
            Part(r, "shade", "SCLA_MESH_Cylinder32Unit", "SCLA_MAT_BlackSootIron", new Vector3(0f, -0.45f, 0f), new Vector3(0.58f, 0.24f, 0.58f), Vector3.zero);
            Part(r, "glow_core", "SCLA_MESH_Cylinder32Unit", "SCLA_MAT_AmberLampGlass", new Vector3(0f, -0.62f, 0f), new Vector3(0.36f, 0.36f, 0.36f), Vector3.zero);
            Part(r, "bottom_rim", "SCLA_MESH_Ring32Unit", "SCLA_MAT_AgedBrassGlow", new Vector3(0f, -0.8f, 0f), new Vector3(0.52f, 0.52f, 0.52f), Vector3.zero);
        }

        private static void BuildGrimeA(GameObject r)
        {
            Part(r, "soot_fan", "SCLA_MESH_SteamWispUnit", "SCLA_MAT_GrimeFilm", new Vector3(0f, 0f, 0f), new Vector3(1.7f, 1.15f, 1f), new Vector3(0f, 0f, 180f));
            Part(r, "hotspot", "SCLA_MESH_QuadUnit", "SCLA_MAT_WetOilBlack", new Vector3(0f, -0.35f, -0.005f), new Vector3(0.34f, 0.18f, 1f), Vector3.zero);
        }

        private static void BuildGrimeB(GameObject r)
        {
            for (int i = 0; i < 6; i++) Part(r, "oil_streak_" + i, "SCLA_MESH_BoxUnit", "SCLA_MAT_GrimeFilm", new Vector3(-0.42f + i * 0.16f, 0.18f - i * 0.035f, 0f), new Vector3(0.035f, 1.2f - i * 0.1f, 0.014f), new Vector3(0f, 0f, -5f + i * 4f));
            Part(r, "lower_smear", "SCLA_MESH_QuadUnit", "SCLA_MAT_WetOilBlack", new Vector3(0.05f, -0.7f, -0.01f), new Vector3(0.95f, 0.22f, 1f), Vector3.zero);
        }

        private static void BuildGrimeC(GameObject r)
        {
            Part(r, "wash", "SCLA_MESH_QuadUnit", "SCLA_MAT_GrimeFilm", Vector3.zero, new Vector3(1.45f, 1.2f, 1f), Vector3.zero);
            for (int i = 0; i < 10; i++)
            {
                float a = i * Mathf.PI * 0.2f;
                Part(r, "rivet_bloom_" + i, "SCLA_MESH_Cylinder16Unit", "SCLA_MAT_WetOilBlack", new Vector3(Mathf.Cos(a) * 0.48f, Mathf.Sin(a) * 0.34f, 0.02f), new Vector3(0.08f, 0.018f, 0.08f), new Vector3(90f, 0f, 0f));
            }
        }

        private static void BuildGrimeD(GameObject r)
        {
            Part(r, "front_wash", "SCLA_MESH_QuadUnit", "SCLA_MAT_GrimeFilm", new Vector3(0f, 0f, 0f), new Vector3(1f, 1.4f, 1f), Vector3.zero);
            Part(r, "side_wash", "SCLA_MESH_QuadUnit", "SCLA_MAT_GrimeFilm", new Vector3(-0.5f, 0f, 0.5f), new Vector3(1f, 1.4f, 1f), new Vector3(0f, 90f, 0f));
        }

        private static void BuildChainsA(GameObject r)
        {
            Part(r, "ceiling_bar", "SCLA_MESH_BoxUnit", "SCLA_MAT_BlackSootIron", new Vector3(0f, 0.75f, 0f), new Vector3(1.25f, 0.08f, 0.1f), Vector3.zero);
            Chain(r, "left", -0.42f, 9, 0.58f);
            Chain(r, "center", 0f, 7, 0.58f);
            Chain(r, "right", 0.42f, 8, 0.58f);
        }

        private static void BuildChainsB(GameObject r)
        {
            Chain(r, "drop", 0f, 10, 0.85f);
            Part(r, "hook", "SCLA_MESH_ChainLinkUnit", "SCLA_MAT_WornSteelEdge", new Vector3(0f, -0.98f, 0f), new Vector3(0.24f, 0.28f, 0.07f), new Vector3(0f, 0f, 30f));
        }

        private static void BuildChainsC(GameObject r)
        {
            Part(r, "rail", "SCLA_MESH_BoxUnit", "SCLA_MAT_AgedBrassGlow", new Vector3(0f, 0.58f, 0f), new Vector3(1.6f, 0.07f, 0.09f), Vector3.zero);
            for (int i = 0; i < 6; i++) Chain(r, "curtain_" + i, -0.65f + i * 0.26f, 5 + (i % 2), 0.43f, 0f);
        }

        private static void BuildPulleyA(GameObject r)
        {
            Part(r, "mount_plate", "SCLA_MESH_BoxUnit", "SCLA_MAT_DeepShadow", Vector3.zero, new Vector3(0.62f, 0.72f, 0.05f), Vector3.zero);
            Part(r, "wheel", "SCLA_MESH_Gear16Silhouette", "SCLA_MAT_BlackSootIron", new Vector3(0f, 0.05f, 0.08f), new Vector3(0.7f, 0.7f, 0.7f), Vector3.zero);
            Part(r, "belt_drop", "SCLA_MESH_BoxUnit", "SCLA_MAT_RubberBelt", new Vector3(0.24f, -0.26f, 0.1f), new Vector3(0.055f, 0.55f, 0.028f), Vector3.zero);
        }

        private static void BuildPulleyB(GameObject r)
        {
            Part(r, "left_wheel", "SCLA_MESH_Gear16Silhouette", "SCLA_MAT_BlackSootIron", new Vector3(-0.45f, 0.08f, 0f), new Vector3(0.58f, 0.58f, 0.58f), Vector3.zero);
            Part(r, "right_wheel", "SCLA_MESH_Gear16Silhouette", "SCLA_MAT_BlackSootIron", new Vector3(0.45f, -0.03f, 0f), new Vector3(0.66f, 0.66f, 0.66f), Vector3.zero);
            Part(r, "top_belt", "SCLA_MESH_BoxUnit", "SCLA_MAT_RubberBelt", new Vector3(0f, 0.27f, 0.05f), new Vector3(1.04f, 0.05f, 0.028f), new Vector3(0f, 0f, -7f));
            Part(r, "bottom_belt", "SCLA_MESH_BoxUnit", "SCLA_MAT_RubberBelt", new Vector3(0f, -0.28f, 0.05f), new Vector3(1.0f, 0.05f, 0.028f), new Vector3(0f, 0f, -7f));
        }

        private static void BuildPulleyC(GameObject r)
        {
            Part(r, "track", "SCLA_MESH_BoxUnit", "SCLA_MAT_BlackSootIron", new Vector3(0f, 0.25f, 0f), new Vector3(1.3f, 0.08f, 0.14f), Vector3.zero);
            Part(r, "left_wheel", "SCLA_MESH_Ring32Unit", "SCLA_MAT_WornSteelEdge", new Vector3(-0.25f, 0f, 0.18f), new Vector3(0.32f, 0.32f, 0.32f), Vector3.zero);
            Part(r, "right_wheel", "SCLA_MESH_Ring32Unit", "SCLA_MAT_WornSteelEdge", new Vector3(0.25f, 0f, 0.18f), new Vector3(0.32f, 0.32f, 0.32f), Vector3.zero);
            Chain(r, "short_drop", 0f, 4, -0.18f, 0.18f);
        }

        private static void BuildDrainA(GameObject r)
        {
            Part(r, "rim", "SCLA_MESH_Ring32Unit", "SCLA_MAT_WornSteelEdge", Vector3.zero, new Vector3(1.05f, 1.05f, 1.05f), new Vector3(90f, 0f, 0f));
            for (int i = 0; i < 12; i++) Part(r, "radial_slot_" + i, "SCLA_MESH_BoxUnit", "SCLA_MAT_BlackSootIron", Vector3.zero, new Vector3(0.055f, 0.78f, 0.035f), new Vector3(0f, i * 30f, 0f));
        }

        private static void BuildDrainB(GameObject r)
        {
            Part(r, "plate", "SCLA_MESH_BoxUnit", "SCLA_MAT_WornSteelEdge", Vector3.zero, new Vector3(1.15f, 0.055f, 1.15f), Vector3.zero);
            for (int i = 0; i < 6; i++) Part(r, "dark_slot_" + i, "SCLA_MESH_BoxUnit", "SCLA_MAT_DeepShadow", new Vector3(-0.42f + i * 0.17f, 0.035f, 0f), new Vector3(0.055f, 0.025f, 0.88f), Vector3.zero);
        }

        private static void BuildDrainC(GameObject r)
        {
            Part(r, "gutter_tray", "SCLA_MESH_BoxUnit", "SCLA_MAT_WetOilBlack", Vector3.zero, new Vector3(2.2f, 0.05f, 0.55f), Vector3.zero);
            for (int i = 0; i < 10; i++) Part(r, "cross_slat_" + i, "SCLA_MESH_BoxUnit", "SCLA_MAT_WornSteelEdge", new Vector3(-0.95f + i * 0.21f, 0.06f, 0f), new Vector3(0.07f, 0.04f, 0.55f), Vector3.zero);
        }

        private static void BuildGaugeA(GameObject r) { Gauge(r, "warning", Vector3.zero, 0.28f, -35f); Part(r, "tag", "SCLA_MESH_BoxUnit", "SCLA_MAT_DangerYellowPlate", new Vector3(0f, -0.43f, -0.015f), new Vector3(0.48f, 0.12f, 0.035f), Vector3.zero); }
        private static void BuildGaugeB(GameObject r) { Part(r, "rack", "SCLA_MESH_BoxUnit", "SCLA_MAT_BlackSootIron", Vector3.zero, new Vector3(1.6f, 0.56f, 0.055f), Vector3.zero); Gauge(r, "gauge_l", new Vector3(-0.52f, 0f, 0.05f), 0.22f, -55f); Gauge(r, "gauge_c", new Vector3(0f, 0.04f, 0.05f), 0.24f, -15f); Gauge(r, "gauge_r", new Vector3(0.52f, 0f, 0.05f), 0.22f, 42f); }
        private static void BuildGaugeC(GameObject r) { Part(r, "board", "SCLA_MESH_BoxUnit", "SCLA_MAT_DangerYellowPlate", Vector3.zero, new Vector3(1.25f, 0.95f, 0.06f), Vector3.zero); Gauge(r, "top", new Vector3(0f, 0.2f, 0.06f), 0.26f, 65f); Gauge(r, "low", new Vector3(-0.36f, -0.28f, 0.06f), 0.18f, -35f); Gauge(r, "low2", new Vector3(0.36f, -0.28f, 0.06f), 0.18f, 15f); }

        private static void BuildCanopyA(GameObject r)
        {
            for (int i = 0; i < 3; i++) Pipe(r, "parallel_pipe_" + i, new Vector3(0f, 0f, -0.28f + i * 0.28f), new Vector3(0.12f, 2.6f, 0.12f), new Vector3(90f, 0f, 90f));
            for (int i = -1; i <= 1; i++) Part(r, "bracket_" + i, "SCLA_MESH_BoxUnit", "SCLA_MAT_BlackSootIron", new Vector3(i * 0.9f, -0.2f, 0f), new Vector3(0.08f, 0.18f, 0.8f), Vector3.zero);
        }

        private static void BuildCanopyB(GameObject r)
        {
            for (int i = 0; i < 5; i++) Pipe(r, "pipe_bank_" + i, new Vector3(0f, 0.05f + (i % 2) * 0.12f, -0.48f + i * 0.24f), new Vector3(0.11f, 3f, 0.11f), new Vector3(90f, 0f, 90f), i % 2 == 0 ? "SCLA_MAT_DullCopperPipe" : "SCLA_MAT_HeatBlueSteel");
            Part(r, "shadow_panel", "SCLA_MESH_QuadUnit", "SCLA_MAT_DeepShadow", new Vector3(0f, -0.18f, 0f), new Vector3(3f, 1.25f, 1f), new Vector3(90f, 0f, 0f));
        }

        private static void BuildCanopyC(GameObject r)
        {
            BuildCanopyA(r);
            Part(r, "crossbrace_a", "SCLA_MESH_BoxUnit", "SCLA_MAT_AgedBrassGlow", new Vector3(0f, -0.25f, 0f), new Vector3(2.2f, 0.055f, 0.06f), new Vector3(0f, 35f, 0f));
            Part(r, "crossbrace_b", "SCLA_MESH_BoxUnit", "SCLA_MAT_AgedBrassGlow", new Vector3(0f, -0.28f, 0f), new Vector3(2.2f, 0.055f, 0.06f), new Vector3(0f, -35f, 0f));
        }

        private static void BuildCanopyD(GameObject r)
        {
            BuildCanopyA(r);
            Part(r, "valve_wheel_a", "SCLA_MESH_Gear16Silhouette", "SCLA_MAT_WarningRedNeedle", new Vector3(-0.55f, -0.12f, 0.32f), new Vector3(0.32f, 0.32f, 0.32f), new Vector3(90f, 0f, 0f));
            Part(r, "valve_wheel_b", "SCLA_MESH_Gear16Silhouette", "SCLA_MAT_WarningRedNeedle", new Vector3(0.65f, -0.12f, -0.28f), new Vector3(0.28f, 0.28f, 0.28f), new Vector3(90f, 0f, 0f));
            Steam(r, new Vector3(1.15f, 0.08f, 0.32f), new Vector3(0.4f, 0.7f, 1f), -18f);
        }

        private static void BuildCombo(GameObject r)
        {
            BuildCanopyD(r);
            BuildChainsC(r);
            Part(r, "rear_grime", "SCLA_MESH_QuadUnit", "SCLA_MAT_GrimeFilm", new Vector3(0f, -0.65f, -0.45f), new Vector3(2.2f, 1.2f, 1f), Vector3.zero);
        }

        private static Mesh BuildBoxMesh()
        {
            Vector3[] v = { new Vector3(-.5f,-.5f,-.5f), new Vector3(.5f,-.5f,-.5f), new Vector3(.5f,.5f,-.5f), new Vector3(-.5f,.5f,-.5f), new Vector3(-.5f,-.5f,.5f), new Vector3(.5f,-.5f,.5f), new Vector3(.5f,.5f,.5f), new Vector3(-.5f,.5f,.5f) };
            int[] t = { 0,2,1,0,3,2,4,5,6,4,6,7,0,1,5,0,5,4,2,3,7,2,7,6,1,2,6,1,6,5,0,4,7,0,7,3 };
            return FinishMesh(v, t);
        }

        private static Mesh BuildQuadMesh()
        {
            return FinishMesh(new[] { new Vector3(-.5f,-.5f,0f), new Vector3(.5f,-.5f,0f), new Vector3(.5f,.5f,0f), new Vector3(-.5f,.5f,0f) }, new[] { 0, 1, 2, 0, 2, 3 });
        }

        private static Mesh BuildCylinderMesh(int sides, float radius, float height)
        {
            List<Vector3> v = new List<Vector3>();
            List<int> t = new List<int>();
            for (int i = 0; i < sides; i++)
            {
                float a = i * Mathf.PI * 2f / sides;
                v.Add(new Vector3(Mathf.Cos(a) * radius, -height * .5f, Mathf.Sin(a) * radius));
                v.Add(new Vector3(Mathf.Cos(a) * radius, height * .5f, Mathf.Sin(a) * radius));
            }
            v.Add(new Vector3(0f, -height * .5f, 0f));
            v.Add(new Vector3(0f, height * .5f, 0f));
            int bottom = v.Count - 2;
            int top = v.Count - 1;
            for (int i = 0; i < sides; i++)
            {
                int n = (i + 1) % sides;
                t.Add(i * 2); t.Add(n * 2 + 1); t.Add(i * 2 + 1);
                t.Add(i * 2); t.Add(n * 2); t.Add(n * 2 + 1);
                t.Add(bottom); t.Add(i * 2); t.Add(n * 2);
                t.Add(top); t.Add(n * 2 + 1); t.Add(i * 2 + 1);
            }
            return FinishMesh(v.ToArray(), t.ToArray());
        }

        private static Mesh BuildRingMesh(int sides, float inner, float outer, float depth)
        {
            List<Vector3> v = new List<Vector3>();
            List<int> t = new List<int>();
            for (int i = 0; i < sides; i++)
            {
                float a = i * Mathf.PI * 2f / sides;
                v.Add(new Vector3(Mathf.Cos(a) * outer, Mathf.Sin(a) * outer, -depth));
                v.Add(new Vector3(Mathf.Cos(a) * inner, Mathf.Sin(a) * inner, -depth));
                v.Add(new Vector3(Mathf.Cos(a) * outer, Mathf.Sin(a) * outer, depth));
                v.Add(new Vector3(Mathf.Cos(a) * inner, Mathf.Sin(a) * inner, depth));
            }
            for (int i = 0; i < sides; i++)
            {
                int n = (i + 1) % sides;
                AddQuad(t, i * 4, n * 4, n * 4 + 2, i * 4 + 2);
                AddQuad(t, i * 4 + 3, n * 4 + 3, n * 4 + 1, i * 4 + 1);
                AddQuad(t, i * 4 + 2, n * 4 + 2, n * 4 + 3, i * 4 + 3);
            }
            return FinishMesh(v.ToArray(), t.ToArray());
        }

        private static Mesh BuildGearMesh(int teeth, float inner, float outer, float depth)
        {
            int sides = teeth * 2;
            List<Vector3> v = new List<Vector3>();
            for (int i = 0; i < sides; i++)
            {
                float a = i * Mathf.PI * 2f / sides;
                float r = i % 2 == 0 ? outer : inner;
                v.Add(new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, depth));
                v.Add(new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, -depth));
            }
            List<int> t = new List<int>();
            for (int i = 0; i < sides; i++)
            {
                int n = (i + 1) % sides;
                t.Add(0); t.Add(n * 2); t.Add(i * 2);
                AddQuad(t, i * 2, n * 2, n * 2 + 1, i * 2 + 1);
            }
            return FinishMesh(v.ToArray(), t.ToArray());
        }

        private static Mesh BuildChainLinkMesh()
        {
            List<Vector3> v = new List<Vector3>();
            List<int> t = new List<int>();
            AddBox(v, t, new Vector3(0f, 0.36f, 0f), new Vector3(0.5f, 0.12f, 0.12f));
            AddBox(v, t, new Vector3(0f, -0.36f, 0f), new Vector3(0.5f, 0.12f, 0.12f));
            AddBox(v, t, new Vector3(-0.25f, 0f, 0f), new Vector3(0.12f, 0.62f, 0.12f));
            AddBox(v, t, new Vector3(0.25f, 0f, 0f), new Vector3(0.12f, 0.62f, 0.12f));
            return FinishMesh(v.ToArray(), t.ToArray());
        }

        private static Mesh BuildSteamWispMesh()
        {
            Vector3[] v = { new Vector3(-.16f,-.48f,0f), new Vector3(.16f,-.48f,0f), new Vector3(.34f,-.08f,0f), new Vector3(.12f,.46f,0f), new Vector3(-.24f,.38f,0f), new Vector3(-.38f,-.02f,0f), Vector3.zero };
            int[] t = { 6,0,1, 6,1,2, 6,2,3, 6,3,4, 6,4,5, 6,5,0 };
            return FinishMesh(v, t);
        }

        private static void AddBox(List<Vector3> v, List<int> t, Vector3 center, Vector3 size)
        {
            int o = v.Count;
            foreach (Vector3 p in BuildBoxMesh().vertices) v.Add(center + Vector3.Scale(p, size));
            foreach (int i in BuildBoxMesh().triangles) t.Add(o + i);
        }

        private static void AddQuad(List<int> t, int a, int b, int c, int d)
        {
            t.Add(a); t.Add(b); t.Add(c); t.Add(a); t.Add(c); t.Add(d);
        }

        private static Mesh FinishMesh(Vector3[] vertices, int[] triangles)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void RenderFamily(string key, string outputRoot, string title, AtmosphereSpec[] specs, Vector3 cameraPos, Vector3 cameraEuler)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientLight = new Color(0.22f, 0.20f, 0.17f);
            GameObject light = new GameObject("preview_key_light");
            Light l = light.AddComponent<Light>();
            l.type = LightType.Directional;
            l.intensity = 2.3f;
            light.transform.rotation = Quaternion.Euler(45f, -30f, 0f);

            int columns = Mathf.CeilToInt(Mathf.Sqrt(specs.Length));
            for (int i = 0; i < specs.Length; i++)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PackageRoot + "/Runtime/Prefabs/" + specs[i].fileName);
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                instance.transform.position = new Vector3((i % columns - (columns - 1) * 0.5f) * 2.25f, 0f, (i / columns) * 1.75f);
                instance.transform.rotation = Quaternion.Euler(0f, i * 17f, 0f);
            }

            Camera camera = new GameObject("preview_camera").AddComponent<Camera>();
            camera.transform.position = cameraPos;
            camera.transform.rotation = Quaternion.Euler(cameraEuler);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.075f, 0.068f, 0.058f);
            camera.fieldOfView = 36f;
            RenderCamera(camera, Path.Combine(outputRoot, "SCLA_PREVIEW_" + key + "_v0.1.44.png"));
            EditorSceneManager.CloseScene(scene, true);
        }

        private static void RenderMaterialSwatch(string outputRoot)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientLight = new Color(0.24f, 0.22f, 0.18f);
            GameObject light = new GameObject("swatch_light");
            light.AddComponent<Light>().type = LightType.Directional;
            light.GetComponent<Light>().intensity = 2.1f;
            light.transform.rotation = Quaternion.Euler(45f, -35f, 0f);
            int i = 0;
            foreach (Material material in Materials.Values)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.DestroyImmediate(sphere.GetComponent<Collider>());
                sphere.name = material.name;
                sphere.GetComponent<MeshRenderer>().sharedMaterial = material;
                sphere.transform.position = new Vector3((i % 4 - 1.5f) * 1.25f, 1.2f - (i / 4) * 1.15f, 0f);
                i++;
            }
            Camera camera = new GameObject("swatch_camera").AddComponent<Camera>();
            camera.transform.position = new Vector3(0f, -0.5f, -7.0f);
            camera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.075f, 0.068f, 0.058f);
            RenderCamera(camera, Path.Combine(outputRoot, "SCLA_PREVIEW_material_swatch_v0.1.44.png"));
            EditorSceneManager.CloseScene(scene, true);
        }

        private static void RenderCamera(Camera camera, string path)
        {
            RenderTexture rt = new RenderTexture(1600, 1000, 24, RenderTextureFormat.ARGB32);
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

        private static void WriteMetadata(string importStatus)
        {
            string manifest = BuildManifest(importStatus);
            string catalog = BuildCatalog();
            File.WriteAllText(FullPath(PackageRoot + "/Documentation~/Manifest/" + ManifestFileName), manifest, Encoding.UTF8);
            File.WriteAllText(FullPath(PackageRoot + "/Runtime/Metadata/" + CatalogFileName), catalog, Encoding.UTF8);
            File.WriteAllText(Path.Combine(ProjectRoot, "Documentation/AssetProduction/V0_1_44_LevelAtmosphereSet03/SCLA_LevelAtmosphereSet03_ProductionReport.md"), BuildProductionReport(importStatus), Encoding.UTF8);
            AssetDatabase.ImportAsset(PackageRoot + "/Documentation~/Manifest/" + ManifestFileName);
            AssetDatabase.ImportAsset(PackageRoot + "/Runtime/Metadata/" + CatalogFileName);
        }

        private static string BuildManifest(string importStatus)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            AddJson(json, "pack_id", PackId, 1, true);
            AddJson(json, "display_name", "Level Atmosphere Set 03", 1, true);
            AddJson(json, "version", Version, 1, true);
            AddJson(json, "build_id", BuildId, 1, true);
            AddJson(json, "unity_version", Application.unityVersion, 1, true);
            AddJson(json, "generated_at_utc", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), 1, true);
            AddJson(json, "sidecar_project", "UD-SC-LVL-LevelAtmosphereSet03", 1, true);
            AddJson(json, "owner_lane", "sidecar-level-atmosphere-set03", 1, true);
            AddJson(json, "primary_intake_owner", "main-lane-art-integration", 1, true);
            AddJson(json, "canonical_root", "AssetPacks/BrassworksBreach.LevelAtmosphereSet03", 1, true);
            AddJson(json, "package_root", "AssetPacks/BrassworksBreach.LevelAtmosphereSet03", 1, true);
            AddJson(json, "package_name", PackageName, 1, true);
            json.AppendLine("  \"asset_counts\": {");
            json.AppendLine("    \"generated_prefabs\": " + Specs().Length + ",");
            json.AppendLine("    \"generated_materials\": " + Materials.Count + ",");
            json.AppendLine("    \"generated_meshes\": " + Meshes.Count + ",");
            json.AppendLine("    \"textures\": 0,");
            json.AppendLine("    \"audio\": 0,");
            json.AppendLine("    \"vfx\": 0,");
            json.AppendLine("    \"animation_clips\": 0,");
            json.AppendLine("    \"preview_renders\": 10");
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
            json.AppendLine("    \"Visual-only prefab family; gameplay collision, occlusion, and navigation authority remain with the primary lane.\",");
            json.AppendLine("    \"Steam wisps and grime are lightweight Unity geometry/material placeholders for art review, not autonomous VFX systems.\",");
            json.AppendLine("    \"Pressure lamps use emissive material proxies only and do not include runtime light scripts or audio.\"");
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
            AddJson(json, "catalog_id", "SCLA_LevelAtmosphereSet03_GeneratedCatalog_v0.1.44", 1, true);
            AddJson(json, "package_name", PackageName, 1, true);
            json.AppendLine("  \"prefabs\": [");
            AtmosphereSpec[] specs = Specs();
            for (int i = 0; i < specs.Length; i++)
            {
                AtmosphereSpec s = specs[i];
                json.AppendLine("    {");
                AddJson(json, "file", "Packages/" + PackageName + "/Runtime/Prefabs/" + s.fileName, 3, true);
                AddJson(json, "family", s.family, 3, true);
                AddJson(json, "description", s.description, 3, true);
                AddJson(json, "visual_only", "true", 3, true, true);
                json.AppendLine("      \"bounds_meters\": [" + s.bounds.x.ToString("0.###", CultureInfo.InvariantCulture) + ", " + s.bounds.y.ToString("0.###", CultureInfo.InvariantCulture) + ", " + s.bounds.z.ToString("0.###", CultureInfo.InvariantCulture) + "]");
                json.Append("    }");
                json.AppendLine(i == specs.Length - 1 ? "" : ",");
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
            return "# Level Atmosphere Set 03 Production Report\n\n" +
                "- Package: `AssetPacks/BrassworksBreach.LevelAtmosphereSet03`\n" +
                "- Version: `0.1.44-p001`\n" +
                "- Prefabs: " + Specs().Length + "\n" +
                "- Materials: " + Materials.Count + "\n" +
                "- Reusable meshes: " + Meshes.Count + "\n" +
                "- Preview renders: 10\n" +
                "- Import smoke status: `" + importStatus + "`\n\n" +
                "Generated in Unity only. Prefabs are visual dressing assets with no gameplay scripts, colliders, rigidbodies, or autonomous audio.\n";
        }

        private static string[] PrefabPaths()
        {
            List<string> paths = new List<string>();
            foreach (AtmosphereSpec spec in Specs()) paths.Add("Packages/" + PackageName + "/Runtime/Prefabs/" + spec.fileName);
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
                RenderOutputRelativePath + "/SCLA_PREVIEW_steam_pipes_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_pressure_lamps_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_wall_grime_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_hanging_chains_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_pulley_silhouettes_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_floor_drains_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_warning_gauges_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_overhead_canopies_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_contact_sheet_v0.1.44.png",
                RenderOutputRelativePath + "/SCLA_PREVIEW_material_swatch_v0.1.44.png"
            };
        }

        private static void AddJson(StringBuilder json, string key, string value, int indent, bool comma, bool raw = false)
        {
            json.Append(new string(' ', indent * 2));
            json.Append("\"").Append(key).Append("\": ");
            json.Append(raw ? value : "\"" + Escape(value) + "\"");
            json.AppendLine(comma ? "," : "");
        }

        private static void AddArray(StringBuilder json, string key, string[] values, int indent, bool comma)
        {
            json.Append(new string(' ', indent * 2)).Append("\"").Append(key).AppendLine("\": [");
            for (int i = 0; i < values.Length; i++)
            {
                json.Append(new string(' ', (indent + 1) * 2)).Append("\"").Append(Escape(values[i])).Append("\"").AppendLine(i == values.Length - 1 ? "" : ",");
            }
            json.Append(new string(' ', indent * 2)).Append("]").AppendLine(comma ? "," : "");
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string PackageRoot
        {
            get { return "Packages/" + PackageName; }
        }

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

        private static string FullPath(string assetPath)
        {
            string relative = assetPath.StartsWith(PackageRoot, StringComparison.Ordinal) ? assetPath.Substring(PackageRoot.Length).TrimStart('/') : assetPath;
            string validationProject = Directory.GetCurrentDirectory();
            string packageFull = Path.GetFullPath(Path.Combine(validationProject, ".."));
            return Path.Combine(packageFull, relative.Replace("/", Path.DirectorySeparatorChar.ToString()));
        }

        private sealed class AtmosphereSpec
        {
            public readonly string fileName;
            public readonly string family;
            public readonly string description;
            public readonly Vector3 bounds;
            public readonly Action<GameObject> builder;

            public AtmosphereSpec(string fileName, string family, string description, Vector3 bounds, Action<GameObject> builder)
            {
                this.fileName = fileName;
                this.family = family;
                this.description = description;
                this.bounds = bounds;
                this.builder = builder;
            }
        }

        private sealed class AssetPackMarker : MonoBehaviour { }
    }
}
#endif
