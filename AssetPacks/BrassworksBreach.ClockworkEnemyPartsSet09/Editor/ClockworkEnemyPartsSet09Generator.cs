using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace BrassworksBreach.ClockworkEnemyPartsSet09.Editor
{
    public static class ClockworkEnemyPartsSet09Generator
    {
        private const string PackId = "CEPS09";
        private const string Version = "0.1.54";
        private const string BuildId = "p001";
        private const string PackageName = "com.brassworks.sidecar.clockwork-enemy-parts-set09";
        private const string PackageRoot = "Packages/" + PackageName;
        private const string RuntimeRoot = PackageRoot + "/Runtime";
        private const string MeshRoot = RuntimeRoot + "/Meshes";
        private const string MaterialRoot = RuntimeRoot + "/Materials";
        private const string TextureRoot = RuntimeRoot + "/Textures";
        private const string PrefabRoot = RuntimeRoot + "/Prefabs";
        private const string MetadataRoot = RuntimeRoot + "/Metadata";
        private const string ManifestRoot = PackageRoot + "/Documentation~/Manifest";
        private const string RenderDocFolder = "Documentation/ConceptRenders/V0_1_54_ClockworkEnemyPartsSet09";
        private const string ProductionDocFolder = "Documentation/AssetProduction/V0_1_54_ClockworkEnemyPartsSet09";
        private const int ExpectedPrefabs = 32;
        private const int ExpectedMaterials = 22;
        private const int ExpectedMeshes = 16;
        private const int ExpectedTexturePngs = 22;
        private const int ExpectedPreviewPngMinimum = 30;

        private static readonly string[] PrefabNames =
        {
            "CEPS09_Skitter_Torso_LowBoilerPan_A",
            "CEPS09_Skitter_Head_TwinAmberOptics_A",
            "CEPS09_Skitter_Limb_SplayedPistonLeg_A",
            "CEPS09_Skitter_Limb_NeedleHookLeg_B",
            "CEPS09_Skitter_Weapon_BellySaw_A",
            "CEPS09_Skitter_Weapon_SteamClaw_B",
            "CEPS09_Skitter_Attachment_BackPressureTick_A",
            "CEPS09_Skitter_Attachment_HoseLoopCage_A",
            "CEPS09_Brute_Torso_RivetedBoilerChest_A",
            "CEPS09_Brute_Head_FurnaceVisor_A",
            "CEPS09_Brute_Limb_PistonArmLeft_A",
            "CEPS09_Brute_Limb_PistonArmRight_A",
            "CEPS09_Brute_Limb_HeavyFootAssembly_A",
            "CEPS09_Brute_Weapon_SawClawArm_A",
            "CEPS09_Brute_Weapon_PressureMaul_B",
            "CEPS09_Brute_Attachment_ShoulderGearYoke_A",
            "CEPS09_Brute_Attachment_BackTwinTanks_A",
            "CEPS09_Sentry_Torso_CeilingMountHub_A",
            "CEPS09_Sentry_Head_RotaryOpticCluster_A",
            "CEPS09_Sentry_Limb_WallClampStrut_A",
            "CEPS09_Sentry_Limb_CeilingRailLeg_A",
            "CEPS09_Sentry_Weapon_TwinSteamLance_A",
            "CEPS09_Sentry_Weapon_MiniSawTurret_B",
            "CEPS09_Sentry_Attachment_CableHoseHalo_A",
            "CEPS09_Sentry_Attachment_PressureBottleRack_A",
            "CEPS09_Shared_Optic_SingleAmberLens_A",
            "CEPS09_Shared_GearCluster_ExposedGovernor_A",
            "CEPS09_Shared_Gauge_SteamPressureDial_A",
            "CEPS09_Shared_RivetPlate_DamageScab_A",
            "CEPS09_ArchetypePreview_SkitterUnit_ReadableLowProfile",
            "CEPS09_ArchetypePreview_BoilerBrute_HumanoidSilhouette",
            "CEPS09_ArchetypePreview_WallCeilingSentry_MountedProfile"
        };

        private enum TextureStyle
        {
            ScratchedMetal,
            DarkOil,
            GlowGlass,
            HotCore,
            Soot,
            CopperPipe,
            Verdigris,
            HeatSteel,
            ChippedPaint,
            RedPaint,
            SmokedGlass,
            GaugeFace,
            Rubber,
            RivetBright,
            Leather,
            Ceramic,
            Gunmetal,
            AshArmor,
            Cinder,
            TealGlass,
            Grease,
            FreshCut
        }

        private readonly struct MatSpec
        {
            public readonly string Key;
            public readonly string AssetName;
            public readonly Color Color;
            public readonly float Metallic;
            public readonly float Smoothness;
            public readonly Color Emission;
            public readonly float Alpha;
            public readonly TextureStyle Style;

            public MatSpec(string key, string assetName, Color color, float metallic, float smoothness, Color emission, float alpha, TextureStyle style)
            {
                Key = key;
                AssetName = assetName;
                Color = color;
                Metallic = metallic;
                Smoothness = smoothness;
                Emission = emission;
                Alpha = alpha;
                Style = style;
            }
        }

        private static readonly MatSpec[] Materials =
        {
            new MatSpec("Brass", "CEPS09_MAT_AgedBrassBoiler", C(0.70f, 0.48f, 0.22f), 0.86f, 0.42f, Color.black, 1f, TextureStyle.ScratchedMetal),
            new MatSpec("Iron", "CEPS09_MAT_BlackenedRivetedIron", C(0.070f, 0.065f, 0.058f), 0.90f, 0.24f, Color.black, 1f, TextureStyle.DarkOil),
            new MatSpec("AmberGlass", "CEPS09_MAT_AmberGlowGlass", C(1.00f, 0.55f, 0.12f), 0.02f, 0.72f, C(1.35f, 0.62f, 0.18f), 0.78f, TextureStyle.GlowGlass),
            new MatSpec("WhiteHot", "CEPS09_MAT_WhiteHotPressureCore", C(1.00f, 0.82f, 0.38f), 0.0f, 0.38f, C(1.65f, 0.92f, 0.32f), 1f, TextureStyle.HotCore),
            new MatSpec("Soot", "CEPS09_MAT_SootDamageAccent", C(0.026f, 0.023f, 0.020f), 0.22f, 0.12f, Color.black, 1f, TextureStyle.Soot),
            new MatSpec("Copper", "CEPS09_MAT_AgedCopperTank", C(0.78f, 0.34f, 0.16f), 0.84f, 0.38f, Color.black, 1f, TextureStyle.CopperPipe),
            new MatSpec("Verdigris", "CEPS09_MAT_VerdigrisCopperPatina", C(0.15f, 0.49f, 0.42f), 0.55f, 0.30f, Color.black, 1f, TextureStyle.Verdigris),
            new MatSpec("BluedSteel", "CEPS09_MAT_HeatBluedSawSteel", C(0.18f, 0.28f, 0.37f), 0.92f, 0.36f, Color.black, 1f, TextureStyle.HeatSteel),
            new MatSpec("Hazard", "CEPS09_MAT_ChippedHazardOchre", C(0.90f, 0.58f, 0.13f), 0.18f, 0.30f, Color.black, 1f, TextureStyle.ChippedPaint),
            new MatSpec("Crimson", "CEPS09_MAT_CrimsonPressurePaint", C(0.58f, 0.055f, 0.035f), 0.22f, 0.32f, Color.black, 1f, TextureStyle.RedPaint),
            new MatSpec("SmokedLens", "CEPS09_MAT_SmokedOpticGlass", C(0.22f, 0.30f, 0.29f), 0.0f, 0.82f, C(0.15f, 0.32f, 0.28f), 0.62f, TextureStyle.SmokedGlass),
            new MatSpec("Ivory", "CEPS09_MAT_IvoryGaugeFace", C(0.80f, 0.72f, 0.56f), 0.04f, 0.46f, Color.black, 1f, TextureStyle.GaugeFace),
            new MatSpec("Rubber", "CEPS09_MAT_OilyRubberHose", C(0.055f, 0.048f, 0.040f), 0.05f, 0.18f, Color.black, 1f, TextureStyle.Rubber),
            new MatSpec("Rivet", "CEPS09_MAT_BrassRivetHeads", C(0.95f, 0.73f, 0.35f), 0.92f, 0.58f, Color.black, 1f, TextureStyle.RivetBright),
            new MatSpec("Leather", "CEPS09_MAT_OilDarkLeatherGasket", C(0.13f, 0.075f, 0.042f), 0.0f, 0.48f, Color.black, 1f, TextureStyle.Leather),
            new MatSpec("Ceramic", "CEPS09_MAT_BoneCeramicInsulator", C(0.70f, 0.63f, 0.47f), 0.0f, 0.55f, Color.black, 1f, TextureStyle.Ceramic),
            new MatSpec("Gunmetal", "CEPS09_MAT_GunmetalEdgeWear", C(0.11f, 0.12f, 0.125f), 0.88f, 0.30f, Color.black, 1f, TextureStyle.Gunmetal),
            new MatSpec("AshArmor", "CEPS09_MAT_AshGreyArmorPlate", C(0.28f, 0.27f, 0.25f), 0.66f, 0.22f, Color.black, 1f, TextureStyle.AshArmor),
            new MatSpec("Cinder", "CEPS09_MAT_OrangeCinderGlow", C(1.00f, 0.37f, 0.075f), 0.0f, 0.30f, C(1.45f, 0.42f, 0.08f), 1f, TextureStyle.Cinder),
            new MatSpec("TealGlass", "CEPS09_MAT_TealStatusGlass", C(0.12f, 0.62f, 0.58f), 0.0f, 0.78f, C(0.06f, 0.34f, 0.31f), 0.72f, TextureStyle.TealGlass),
            new MatSpec("Grease", "CEPS09_MAT_DarkGrease", C(0.018f, 0.017f, 0.015f), 0.10f, 0.08f, Color.black, 1f, TextureStyle.Grease),
            new MatSpec("FreshCut", "CEPS09_MAT_FreshCutSteel", C(0.74f, 0.76f, 0.70f), 0.95f, 0.50f, Color.black, 1f, TextureStyle.FreshCut)
        };

        [MenuItem("Brassworks Breach/Sidecars/Clockwork Enemy Parts Set 09/Generate Package")]
        public static void GeneratePackage()
        {
            EnsurePackageFolders();
            Dictionary<string, Texture2D> textures = CreateTextures();
            Dictionary<string, Material> materials = CreateMaterials(textures);
            Dictionary<string, Mesh> meshes = CreateMeshes();
            CreatePrefabs(materials, meshes);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            WriteCatalog();
            WriteManifest();
            Debug.Log("CEPS09 generated: " + ExpectedPrefabs + " prefabs, " + ExpectedMaterials + " materials, " + ExpectedMeshes + " meshes, " + ExpectedTexturePngs + " procedural texture PNGs.");
        }

        [MenuItem("Brassworks Breach/Sidecars/Clockwork Enemy Parts Set 09/Render Preview PNGs")]
        public static void RenderPreviewPngs()
        {
            GeneratePackage();
            string outputRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            Directory.CreateDirectory(outputRoot);
            RenderAllPrefabPreviews(outputRoot);
            WriteMaterialSwatches(outputRoot);
            WriteRenderIndex(outputRoot);
            WriteManifest();
            int errors = WriteUnityValidationReport();
            if (errors != 0)
            {
                throw new InvalidOperationException("CEPS09 validation failed with " + errors.ToString(CultureInfo.InvariantCulture) + " error bucket(s).");
            }
        }

        public static void GenerateAllAndRenderPreview()
        {
            RenderPreviewPngs();
        }

        private static void EnsurePackageFolders()
        {
            EnsureAssetFolder(RuntimeRoot);
            EnsureAssetFolder(MeshRoot);
            EnsureAssetFolder(MaterialRoot);
            EnsureAssetFolder(TextureRoot);
            EnsureAssetFolder(PrefabRoot);
            EnsureAssetFolder(MetadataRoot);
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

        private static Dictionary<string, Texture2D> CreateTextures()
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>(StringComparer.Ordinal);
            string packagePath = PackagePhysicalRoot();
            for (int i = 0; i < Materials.Length; i++)
            {
                MatSpec spec = Materials[i];
                string textureName = spec.AssetName.Replace("_MAT_", "_TEX_") + "_Base";
                string assetPath = TextureRoot + "/" + textureName + ".png";
                string absolutePath = Path.Combine(packagePath, "Runtime", "Textures", textureName + ".png");
                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));

                Texture2D generated = BuildTexture(spec, 256, 256);
                File.WriteAllBytes(absolutePath, ImageConversion.EncodeToPNG(generated));
                UnityEngine.Object.DestroyImmediate(generated);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Default;
                    importer.mipmapEnabled = true;
                    importer.sRGBTexture = true;
                    importer.alphaSource = TextureImporterAlphaSource.FromInput;
                    importer.wrapMode = TextureWrapMode.Repeat;
                    importer.filterMode = FilterMode.Bilinear;
                    importer.SaveAndReimport();
                }

                Texture2D imported = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                textures[spec.Key] = imported;
            }

            return textures;
        }

        private static Texture2D BuildTexture(MatSpec spec, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color baseColor = WithAlpha(spec.Color, spec.Alpha);
            int seed = spec.AssetName.GetHashCode();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float u = x / (float)(width - 1);
                    float v = y / (float)(height - 1);
                    float n = Hash01(x, y, seed);
                    float stripe = Mathf.Sin((u * 42f) + (v * 7f) + seed * 0.001f) * 0.5f + 0.5f;
                    float radial = Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.5f));
                    Color color = baseColor;

                    switch (spec.Style)
                    {
                        case TextureStyle.ScratchedMetal:
                            color *= 0.78f + n * 0.26f;
                            if (stripe > 0.94f || Hash01(y, x * 3, seed) > 0.985f) color = Color.Lerp(color, Color.white, 0.28f);
                            break;
                        case TextureStyle.DarkOil:
                            color *= 0.62f + n * 0.30f;
                            if (v < 0.12f || u > 0.86f) color = Color.Lerp(color, C(0.55f, 0.48f, 0.38f), 0.17f);
                            break;
                        case TextureStyle.GlowGlass:
                            color *= 0.72f + Mathf.Clamp01(1f - radial * 1.55f) * 0.72f + n * 0.08f;
                            if (radial < 0.18f) color = Color.Lerp(color, Color.white, 0.24f);
                            break;
                        case TextureStyle.HotCore:
                            color = Color.Lerp(C(0.36f, 0.04f, 0.01f), color, Mathf.Clamp01(1.15f - radial * 1.9f));
                            if (stripe > 0.86f) color = Color.Lerp(color, C(1f, 0.96f, 0.62f), 0.38f);
                            break;
                        case TextureStyle.Soot:
                            color *= 0.52f + n * 0.22f;
                            if (Hash01(x / 4, y / 4, seed) > 0.88f) color = Color.Lerp(color, C(0.15f, 0.13f, 0.10f), 0.35f);
                            break;
                        case TextureStyle.CopperPipe:
                            color *= 0.76f + stripe * 0.18f + n * 0.10f;
                            if (Hash01(x / 8, y / 8, seed) > 0.90f) color = Color.Lerp(color, C(0.12f, 0.48f, 0.38f), 0.20f);
                            break;
                        case TextureStyle.Verdigris:
                            color *= 0.70f + n * 0.24f;
                            if (stripe > 0.80f) color = Color.Lerp(color, C(0.06f, 0.18f, 0.14f), 0.28f);
                            break;
                        case TextureStyle.HeatSteel:
                            color = Color.Lerp(color, C(0.42f, 0.22f, 0.52f), Mathf.Clamp01(u * 0.45f + stripe * 0.18f));
                            if (v > 0.76f) color = Color.Lerp(color, C(0.72f, 0.42f, 0.15f), 0.22f);
                            break;
                        case TextureStyle.ChippedPaint:
                        case TextureStyle.RedPaint:
                            color *= 0.82f + n * 0.20f;
                            if (Hash01(x / 6, y / 6, seed) > 0.82f || stripe > 0.96f) color = Color.Lerp(color, C(0.06f, 0.055f, 0.05f), 0.68f);
                            break;
                        case TextureStyle.SmokedGlass:
                        case TextureStyle.TealGlass:
                            color *= 0.66f + Mathf.Clamp01(1f - radial * 1.2f) * 0.42f;
                            if (stripe > 0.92f) color = Color.Lerp(color, Color.white, 0.12f);
                            break;
                        case TextureStyle.GaugeFace:
                            color *= 0.86f + n * 0.12f;
                            if (Mathf.Abs(u - 0.5f) < 0.008f || Mathf.Abs(v - 0.5f) < 0.008f) color = Color.Lerp(color, Color.black, 0.45f);
                            if (radial > 0.43f && radial < 0.47f) color = Color.Lerp(color, Color.black, 0.50f);
                            break;
                        case TextureStyle.Rubber:
                        case TextureStyle.Leather:
                            color *= 0.60f + n * 0.22f;
                            if (stripe > 0.88f) color = Color.Lerp(color, C(0.36f, 0.24f, 0.13f), 0.22f);
                            break;
                        case TextureStyle.RivetBright:
                        case TextureStyle.FreshCut:
                            color *= 0.82f + n * 0.18f;
                            if (radial < 0.16f || stripe > 0.97f) color = Color.Lerp(color, Color.white, 0.30f);
                            break;
                        case TextureStyle.Ceramic:
                            color *= 0.86f + n * 0.12f;
                            if (stripe > 0.95f) color = Color.Lerp(color, C(0.26f, 0.19f, 0.13f), 0.30f);
                            break;
                        case TextureStyle.Gunmetal:
                        case TextureStyle.AshArmor:
                            color *= 0.72f + n * 0.24f;
                            if (u < 0.08f || v > 0.90f) color = Color.Lerp(color, C(0.80f, 0.74f, 0.62f), 0.15f);
                            break;
                        case TextureStyle.Cinder:
                            color = Color.Lerp(C(0.05f, 0.018f, 0.008f), color, Mathf.Clamp01((stripe + n) * 0.55f));
                            if (n > 0.94f) color = Color.Lerp(color, C(1f, 0.82f, 0.32f), 0.55f);
                            break;
                        case TextureStyle.Grease:
                            color *= 0.45f + n * 0.20f;
                            if (stripe > 0.74f) color = Color.Lerp(color, C(0.10f, 0.085f, 0.045f), 0.40f);
                            break;
                    }

                    color.a = spec.Alpha;
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Dictionary<string, Material> CreateMaterials(Dictionary<string, Texture2D> textures)
        {
            Dictionary<string, Material> result = new Dictionary<string, Material>(StringComparer.Ordinal);
            Shader shader = FindLitShader();
            for (int i = 0; i < Materials.Length; i++)
            {
                MatSpec spec = Materials[i];
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
                SetFloat(material, "_Glossiness", spec.Smoothness);
                SetFloat(material, "_Smoothness", spec.Smoothness);
                if (textures.TryGetValue(spec.Key, out Texture2D texture) && texture != null)
                {
                    SetTexture(material, "_MainTex", texture);
                    SetTexture(material, "_BaseMap", texture);
                }

                if (spec.Emission.maxColorComponent > 0.001f)
                {
                    SetColor(material, "_EmissionColor", spec.Emission);
                    material.EnableKeyword("_EMISSION");
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                }
                else
                {
                    SetColor(material, "_EmissionColor", Color.black);
                    material.DisableKeyword("_EMISSION");
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                }

                ConfigureTransparency(material, spec.Alpha < 0.99f);
                EditorUtility.SetDirty(material);
                result[spec.Key] = material;
            }

            return result;
        }

        private static Dictionary<string, Mesh> CreateMeshes()
        {
            Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>(StringComparer.Ordinal)
            {
                ["Box"] = UpsertMesh("CEPS09_MESH_BoxUnit", Box("CEPS09_MESH_BoxUnit", new Vector3(1f, 1f, 1f))),
                ["Cylinder16"] = UpsertMesh("CEPS09_MESH_Cylinder16Unit", Cylinder("CEPS09_MESH_Cylinder16Unit", 0.5f, 0.5f, 1f, 16)),
                ["Cylinder32"] = UpsertMesh("CEPS09_MESH_Cylinder32Unit", Cylinder("CEPS09_MESH_Cylinder32Unit", 0.5f, 0.5f, 1f, 32)),
                ["BoilerTorso"] = UpsertMesh("CEPS09_MESH_BoilerTorsoOval", Cylinder("CEPS09_MESH_BoilerTorsoOval", 0.58f, 0.42f, 1.2f, 32)),
                ["PressureTank"] = UpsertMesh("CEPS09_MESH_RibbedPressureTank", Cylinder("CEPS09_MESH_RibbedPressureTank", 0.34f, 0.34f, 1.35f, 32)),
                ["GearRing"] = UpsertMesh("CEPS09_MESH_GearShoulderRing", GearRing("CEPS09_MESH_GearShoulderRing", 24, 0.30f, 0.56f, 0.12f)),
                ["SawBlade"] = UpsertMesh("CEPS09_MESH_SawBlade28", GearRing("CEPS09_MESH_SawBlade28", 28, 0.08f, 0.50f, 0.06f)),
                ["HammerHead"] = UpsertMesh("CEPS09_MESH_HammerHeadBlock", BeveledBox("CEPS09_MESH_HammerHeadBlock", new Vector3(0.86f, 0.46f, 0.42f), 0.10f)),
                ["ClawFinger"] = UpsertMesh("CEPS09_MESH_CurvedClawFinger", HoseArc("CEPS09_MESH_CurvedClawFinger", 0.38f, 0.045f, 132f, 14, 8)),
                ["ArmorPlate"] = UpsertMesh("CEPS09_MESH_ArmorPlateBeveled", ArmorPlate("CEPS09_MESH_ArmorPlateBeveled", 0.80f, 0.62f, 0.075f)),
                ["HoseArc"] = UpsertMesh("CEPS09_MESH_HoseArc", HoseArc("CEPS09_MESH_HoseArc", 0.52f, 0.055f, 180f, 20, 10)),
                ["Flywheel"] = UpsertMesh("CEPS09_MESH_FlywheelSpokedRing", GearRing("CEPS09_MESH_FlywheelSpokedRing", 18, 0.34f, 0.60f, 0.10f)),
                ["Lens"] = UpsertMesh("CEPS09_MESH_LensConvex", LensCap("CEPS09_MESH_LensConvex", 0.34f, 0.10f, 24, 8)),
                ["FurnaceCore"] = UpsertMesh("CEPS09_MESH_FurnaceCorePrism", Prism("CEPS09_MESH_FurnaceCorePrism", 6, 0.32f, 0.62f)),
                ["TripodStrut"] = UpsertMesh("CEPS09_MESH_TripodLegStrut", TaperedCylinder("CEPS09_MESH_TripodLegStrut", 0.09f, 0.055f, 0.92f, 8)),
                ["CutterArm"] = UpsertMesh("CEPS09_MESH_CutterArmSegment", BeveledBox("CEPS09_MESH_CutterArmSegment", new Vector3(0.28f, 0.28f, 0.94f), 0.065f))
            };

            return meshes;
        }

        private static Mesh UpsertMesh(string assetName, Mesh mesh)
        {
            string path = MeshRoot + "/" + assetName + ".asset";
            Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing == null)
            {
                AssetDatabase.CreateAsset(mesh, path);
                return mesh;
            }

            EditorUtility.CopySerialized(mesh, existing);
            UnityEngine.Object.DestroyImmediate(mesh);
            EditorUtility.SetDirty(existing);
            return existing;
        }

        private static void CreatePrefabs(Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            SavePrefab(BuildSkitterTorso("CEPS09_Skitter_Torso_LowBoilerPan_A", mat, mesh));
            SavePrefab(BuildSkitterHead("CEPS09_Skitter_Head_TwinAmberOptics_A", mat, mesh));
            SavePrefab(BuildSkitterLeg("CEPS09_Skitter_Limb_SplayedPistonLeg_A", 0, mat, mesh));
            SavePrefab(BuildSkitterLeg("CEPS09_Skitter_Limb_NeedleHookLeg_B", 1, mat, mesh));
            SavePrefab(BuildSkitterWeapon("CEPS09_Skitter_Weapon_BellySaw_A", 0, mat, mesh));
            SavePrefab(BuildSkitterWeapon("CEPS09_Skitter_Weapon_SteamClaw_B", 1, mat, mesh));
            SavePrefab(BuildSkitterAttachment("CEPS09_Skitter_Attachment_BackPressureTick_A", 0, mat, mesh));
            SavePrefab(BuildSkitterAttachment("CEPS09_Skitter_Attachment_HoseLoopCage_A", 1, mat, mesh));

            SavePrefab(BuildBruteTorso("CEPS09_Brute_Torso_RivetedBoilerChest_A", mat, mesh));
            SavePrefab(BuildBruteHead("CEPS09_Brute_Head_FurnaceVisor_A", mat, mesh));
            SavePrefab(BuildBruteArm("CEPS09_Brute_Limb_PistonArmLeft_A", -1f, mat, mesh));
            SavePrefab(BuildBruteArm("CEPS09_Brute_Limb_PistonArmRight_A", 1f, mat, mesh));
            SavePrefab(BuildBruteFoot("CEPS09_Brute_Limb_HeavyFootAssembly_A", mat, mesh));
            SavePrefab(BuildBruteWeapon("CEPS09_Brute_Weapon_SawClawArm_A", 0, mat, mesh));
            SavePrefab(BuildBruteWeapon("CEPS09_Brute_Weapon_PressureMaul_B", 1, mat, mesh));
            SavePrefab(BuildBruteAttachment("CEPS09_Brute_Attachment_ShoulderGearYoke_A", 0, mat, mesh));
            SavePrefab(BuildBruteAttachment("CEPS09_Brute_Attachment_BackTwinTanks_A", 1, mat, mesh));

            SavePrefab(BuildSentryTorso("CEPS09_Sentry_Torso_CeilingMountHub_A", mat, mesh));
            SavePrefab(BuildSentryHead("CEPS09_Sentry_Head_RotaryOpticCluster_A", mat, mesh));
            SavePrefab(BuildSentryLimb("CEPS09_Sentry_Limb_WallClampStrut_A", 0, mat, mesh));
            SavePrefab(BuildSentryLimb("CEPS09_Sentry_Limb_CeilingRailLeg_A", 1, mat, mesh));
            SavePrefab(BuildSentryWeapon("CEPS09_Sentry_Weapon_TwinSteamLance_A", 0, mat, mesh));
            SavePrefab(BuildSentryWeapon("CEPS09_Sentry_Weapon_MiniSawTurret_B", 1, mat, mesh));
            SavePrefab(BuildSentryAttachment("CEPS09_Sentry_Attachment_CableHoseHalo_A", 0, mat, mesh));
            SavePrefab(BuildSentryAttachment("CEPS09_Sentry_Attachment_PressureBottleRack_A", 1, mat, mesh));

            SavePrefab(BuildSharedPart("CEPS09_Shared_Optic_SingleAmberLens_A", 0, mat, mesh));
            SavePrefab(BuildSharedPart("CEPS09_Shared_GearCluster_ExposedGovernor_A", 1, mat, mesh));
            SavePrefab(BuildSharedPart("CEPS09_Shared_Gauge_SteamPressureDial_A", 2, mat, mesh));
            SavePrefab(BuildSharedPart("CEPS09_Shared_RivetPlate_DamageScab_A", 3, mat, mesh));

            SavePrefab(BuildArchetypePreview("CEPS09_ArchetypePreview_SkitterUnit_ReadableLowProfile", 0, mat, mesh));
            SavePrefab(BuildArchetypePreview("CEPS09_ArchetypePreview_BoilerBrute_HumanoidSilhouette", 1, mat, mesh));
            SavePrefab(BuildArchetypePreview("CEPS09_ArchetypePreview_WallCeilingSentry_MountedProfile", 2, mat, mesh));
        }

        private static GameObject BuildSkitterTorso(string name, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "skitter_torso_low_boiler_visual_part");
            Part(root.transform, "low_horizontal_brass_boiler_pan", mesh["BoilerTorso"], mat["Brass"], V3(0f, 0.42f, 0f), Q(0f, 0f, 90f), V3(0.74f, 1.28f, 0.52f));
            Part(root.transform, "blackened_iron_spine_band", mesh["Box"], mat["Iron"], V3(0f, 0.68f, 0.04f), Q(0f, 0f, 0f), V3(1.34f, 0.16f, 0.40f));
            Part(root.transform, "underbelly_amber_furnace_eye", mesh["Lens"], mat["AmberGlass"], V3(0f, 0.32f, -0.42f), Q(0f, 0f, 0f), V3(0.34f, 0.34f, 0.38f));
            Part(root.transform, "front_rivet_damage_plate", mesh["ArmorPlate"], mat["AshArmor"], V3(0f, 0.43f, -0.48f), Q(0f, 0f, 0f), V3(0.82f, 0.34f, 0.78f));
            Part(root.transform, "rear_pressure_socket", mesh["Cylinder16"], mat["Copper"], V3(0f, 0.62f, 0.46f), Q(90f, 0f, 0f), V3(0.34f, 0.18f, 0.34f));
            for (int i = 0; i < 4; i++)
            {
                float x = i < 2 ? -0.52f : 0.52f;
                float z = i % 2 == 0 ? -0.24f : 0.24f;
                Socket(root.transform, "SOCK_Leg_" + (i + 1).ToString(CultureInfo.InvariantCulture), V3(x, 0.36f, z));
            }

            AddRivetsOnFront(root.transform, mesh, mat, "skitter_pan_rivets", 12, 0.42f, 0.44f, -0.53f);
            Socket(root.transform, "SOCK_HeadFront", V3(0f, 0.60f, -0.58f));
            Socket(root.transform, "SOCK_WeaponUnder", V3(0f, 0.16f, -0.20f));
            Socket(root.transform, "SOCK_BackTank", V3(0f, 0.74f, 0.54f));
            Socket(root.transform, "SOCK_RigFuture_Root", V3(0f, 0.35f, 0f));
            return root;
        }

        private static GameObject BuildSkitterHead(string name, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "skitter_head_twin_optics_visual_part");
            Part(root.transform, "wedge_blackiron_head_block", mesh["ArmorPlate"], mat["Iron"], V3(0f, 0.48f, 0f), Q(0f, 0f, 0f), V3(0.86f, 0.42f, 0.72f));
            Eye(root.transform, mesh, mat, "left_amber", V3(-0.22f, 0.50f, -0.08f), 0.30f, "AmberGlass");
            Eye(root.transform, mesh, mat, "right_amber", V3(0.22f, 0.50f, -0.08f), 0.30f, "AmberGlass");
            Part(root.transform, "central_teal_rangefinder", mesh["Lens"], mat["TealGlass"], V3(0f, 0.70f, -0.12f), Q(0f, 0f, 0f), V3(0.18f, 0.18f, 0.26f));
            Part(root.transform, "needle_lower_jaw", mesh["ClawFinger"], mat["FreshCut"], V3(0f, 0.27f, -0.24f), Q(0f, 0f, -90f), V3(0.72f, 0.72f, 0.72f));
            AddRivetsOnFront(root.transform, mesh, mat, "head_rivet", 6, 0.34f, 0.49f, -0.15f);
            Socket(root.transform, "SOCK_NeckMount", V3(0f, 0.48f, 0.22f));
            Socket(root.transform, "SOCK_OpticGlow", V3(0f, 0.52f, -0.36f));
            return root;
        }

        private static GameObject BuildSkitterLeg(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "skitter_limb_visual_part");
            float lean = variant == 0 ? 26f : 40f;
            Part(root.transform, "brass_hip_socket_gear", mesh["GearRing"], mat["Brass"], V3(0f, 0.78f, 0f), Q(0f, 90f, 0f), V3(0.42f, 0.42f, 0.42f));
            Part(root.transform, "upper_piston_leg_strut", mesh["TripodStrut"], mat["Iron"], V3(0.20f, 0.54f, 0f), Q(0f, 0f, -lean), V3(1f, 0.72f, 1f));
            Part(root.transform, "exposed_copper_piston", mesh["Cylinder16"], mat["Copper"], V3(0.10f, 0.48f, -0.08f), Q(0f, 0f, -lean), V3(0.055f, 0.70f, 0.055f));
            Part(root.transform, "lower_blackiron_knee_link", mesh["TripodStrut"], mat["Gunmetal"], V3(0.43f, 0.26f, 0f), Q(0f, 0f, -lean * 0.72f), V3(0.86f, 0.58f, 0.86f));
            Part(root.transform, variant == 0 ? "wide_anchor_skitter_foot" : "needle_hook_skitter_foot", variant == 0 ? mesh["ArmorPlate"] : mesh["ClawFinger"], mat[variant == 0 ? "AshArmor" : "FreshCut"], V3(0.62f, 0.06f, -0.02f), Q(0f, 0f, variant == 0 ? 0f : -34f), V3(variant == 0 ? 0.52f : 0.88f, variant == 0 ? 0.22f : 0.88f, 0.58f));
            Socket(root.transform, "SOCK_HipMount", V3(0f, 0.82f, 0f));
            Socket(root.transform, "SOCK_FootContact", V3(0.64f, 0.02f, -0.02f));
            return root;
        }

        private static GameObject BuildSkitterWeapon(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "skitter_weapon_visual_part");
            if (variant == 0)
            {
                Part(root.transform, "underbelly_saw_disc", mesh["SawBlade"], mat["BluedSteel"], V3(0f, 0.42f, -0.18f), Q(0f, 90f, 0f), V3(0.72f, 0.72f, 0.72f));
                Part(root.transform, "brass_saw_hub", mesh["Cylinder16"], mat["Rivet"], V3(0f, 0.42f, -0.20f), Q(0f, 0f, 90f), V3(0.22f, 0.08f, 0.22f));
                Part(root.transform, "soot_guard_shroud", mesh["ArmorPlate"], mat["Soot"], V3(0f, 0.62f, -0.05f), Q(0f, 0f, 180f), V3(0.62f, 0.28f, 0.70f));
            }
            else
            {
                Part(root.transform, "steam_claw_blackiron_palm", mesh["Box"], mat["Iron"], V3(0f, 0.46f, 0f), Q(0f, 0f, 0f), V3(0.36f, 0.26f, 0.30f));
                for (int i = 0; i < 3; i++)
                {
                    Part(root.transform, "steam_claw_talon_" + i, mesh["ClawFinger"], mat["FreshCut"], V3((i - 1) * 0.15f, 0.43f, -0.18f), Q(0f, 0f, -54f + i * 28f), V3(0.78f, 0.78f, 0.78f));
                }
                Part(root.transform, "tiny_pressure_cylinder", mesh["Cylinder16"], mat["Copper"], V3(0f, 0.62f, 0.18f), Q(90f, 0f, 0f), V3(0.16f, 0.28f, 0.16f));
            }

            Socket(root.transform, "SOCK_BellyMount", V3(0f, 0.78f, 0.18f));
            Socket(root.transform, "SOCK_AttackTip", V3(0f, 0.34f, -0.58f));
            return root;
        }

        private static GameObject BuildSkitterAttachment(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "skitter_attachment_visual_part");
            if (variant == 0)
            {
                Part(root.transform, "back_tick_pressure_tank", mesh["PressureTank"], mat["Copper"], V3(0f, 0.58f, 0f), Q(0f, 0f, 90f), V3(0.48f, 0.70f, 0.48f));
                Part(root.transform, "ivory_micro_gauge", mesh["Lens"], mat["Ivory"], V3(0f, 0.58f, -0.28f), Q(0f, 0f, 0f), V3(0.20f, 0.20f, 0.24f));
                Part(root.transform, "red_release_valve", mesh["Cylinder16"], mat["Crimson"], V3(0.36f, 0.58f, 0f), Q(0f, 0f, 90f), V3(0.12f, 0.08f, 0.12f));
            }
            else
            {
                Part(root.transform, "oily_hose_loop_cage", mesh["HoseArc"], mat["Rubber"], V3(0f, 0.48f, 0f), Q(0f, 90f, 0f), V3(0.82f, 0.82f, 0.82f));
                Part(root.transform, "left_brass_hose_collar", mesh["Cylinder16"], mat["Brass"], V3(-0.42f, 0.48f, 0f), Q(0f, 0f, 90f), V3(0.14f, 0.10f, 0.14f));
                Part(root.transform, "right_brass_hose_collar", mesh["Cylinder16"], mat["Brass"], V3(0.42f, 0.48f, 0f), Q(0f, 0f, 90f), V3(0.14f, 0.10f, 0.14f));
                Part(root.transform, "soot_stiffener_bar", mesh["Box"], mat["Soot"], V3(0f, 0.72f, 0f), Q(0f, 0f, 0f), V3(0.82f, 0.055f, 0.08f));
            }

            Socket(root.transform, "SOCK_AttachmentMount", V3(0f, 0.48f, 0.24f));
            Socket(root.transform, "SOCK_SteamLine", V3(0f, 0.78f, 0f));
            return root;
        }

        private static GameObject BuildBruteTorso(string name, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "brute_torso_humanoid_boiler_visual_part");
            Part(root.transform, "riveted_brass_boiler_chest", mesh["BoilerTorso"], mat["Brass"], V3(0f, 1.02f, 0f), Q(0f, 0f, 0f), V3(1.20f, 1.38f, 0.86f));
            Part(root.transform, "blackened_iron_belly_corset", mesh["Cylinder32"], mat["Iron"], V3(0f, 0.82f, 0f), Q(0f, 0f, 0f), V3(1.34f, 0.12f, 0.96f));
            Part(root.transform, "front_armor_breastplate", mesh["ArmorPlate"], mat["AshArmor"], V3(0f, 1.06f, -0.52f), Q(0f, 0f, 0f), V3(1.04f, 1.02f, 1f));
            Part(root.transform, "large_amber_furnace_core", mesh["Lens"], mat["AmberGlass"], V3(0f, 1.08f, -0.62f), Q(0f, 0f, 0f), V3(0.54f, 0.54f, 0.62f));
            Part(root.transform, "left_shoulder_socket_gear", mesh["GearRing"], mat["Gunmetal"], V3(-0.78f, 1.42f, 0f), Q(0f, 90f, 0f), V3(0.62f, 0.62f, 0.62f));
            Part(root.transform, "right_shoulder_socket_gear", mesh["GearRing"], mat["Gunmetal"], V3(0.78f, 1.42f, 0f), Q(0f, 90f, 0f), V3(0.62f, 0.62f, 0.62f));
            for (int i = 0; i < 4; i++)
            {
                Part(root.transform, "boiler_vertical_rivet_band_" + i, mesh["Box"], mat["Rivet"], V3(-0.45f + i * 0.30f, 1.08f, -0.66f), Q(0f, 0f, 0f), V3(0.055f, 1.04f, 0.045f));
            }

            AddRivetsOnFront(root.transform, mesh, mat, "brute_chest_rivets", 14, 0.54f, 1.08f, -0.70f);
            Socket(root.transform, "SOCK_Head", V3(0f, 1.90f, -0.04f));
            Socket(root.transform, "SOCK_LeftShoulder", V3(-0.92f, 1.42f, 0f));
            Socket(root.transform, "SOCK_RightShoulder", V3(0.92f, 1.42f, 0f));
            Socket(root.transform, "SOCK_BackTanks", V3(0f, 1.18f, 0.58f));
            Socket(root.transform, "SOCK_RigFuture_Hips", V3(0f, 0.22f, 0f));
            return root;
        }

        private static GameObject BuildBruteHead(string name, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "brute_head_furnace_visor_visual_part");
            Part(root.transform, "heavy_blackiron_helmet_shell", mesh["ArmorPlate"], mat["Iron"], V3(0f, 0.66f, 0f), Q(0f, 0f, 0f), V3(1.10f, 0.62f, 0.98f));
            Part(root.transform, "horizontal_amber_furnace_visor", mesh["Box"], mat["WhiteHot"], V3(0f, 0.68f, -0.12f), Q(0f, 0f, 0f), V3(0.82f, 0.10f, 0.10f));
            Part(root.transform, "smoked_outer_lens_strip", mesh["ArmorPlate"], mat["SmokedLens"], V3(0f, 0.68f, -0.16f), Q(0f, 0f, 0f), V3(0.94f, 0.20f, 0.66f));
            Part(root.transform, "copper_brow_pipe", mesh["Cylinder16"], mat["Copper"], V3(0f, 0.92f, -0.08f), Q(0f, 0f, 90f), V3(0.055f, 0.78f, 0.055f));
            Part(root.transform, "rear_soot_exhaust_stack", mesh["Cylinder16"], mat["Soot"], V3(0f, 0.92f, 0.26f), Q(0f, 0f, 0f), V3(0.18f, 0.34f, 0.18f));
            AddRivetsOnFront(root.transform, mesh, mat, "visor_rivets", 8, 0.48f, 0.67f, -0.21f);
            Socket(root.transform, "SOCK_NeckMount", V3(0f, 0.30f, 0.08f));
            Socket(root.transform, "SOCK_OpticGlow", V3(0f, 0.68f, -0.34f));
            return root;
        }

        private static GameObject BuildBruteArm(string name, float side, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "brute_piston_arm_visual_part");
            Part(root.transform, "shoulder_gear_mount", mesh["GearRing"], mat["Brass"], V3(0f, 1.18f, 0f), Q(0f, 90f, 0f), V3(0.58f, 0.58f, 0.58f));
            Part(root.transform, "upper_blackiron_arm_segment", mesh["CutterArm"], mat["Iron"], V3(0.20f * side, 0.88f, -0.02f), Q(0f, 14f * side, -34f * side), V3(1f, 1f, 1.05f));
            Part(root.transform, "exposed_copper_upper_piston", mesh["Cylinder16"], mat["Copper"], V3(0.12f * side, 0.88f, -0.16f), Q(0f, 0f, -34f * side), V3(0.06f, 0.78f, 0.06f));
            Part(root.transform, "elbow_pressure_hub", mesh["Cylinder16"], mat["Rivet"], V3(0.40f * side, 0.62f, -0.04f), Q(0f, 0f, 90f), V3(0.24f, 0.12f, 0.24f));
            Part(root.transform, "lower_saw_mount_forearm", mesh["CutterArm"], mat["Gunmetal"], V3(0.60f * side, 0.36f, -0.08f), Q(0f, -10f * side, -26f * side), V3(0.86f, 0.86f, 0.92f));
            Socket(root.transform, "SOCK_Shoulder", V3(0f, 1.20f, 0f));
            Socket(root.transform, "SOCK_Wrist", V3(0.78f * side, 0.20f, -0.12f));
            Socket(root.transform, "SOCK_PistonDriver", V3(0.20f * side, 0.96f, -0.20f));
            return root;
        }

        private static GameObject BuildBruteFoot(string name, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "brute_heavy_foot_visual_part");
            Part(root.transform, "wide_blackiron_toe_block", mesh["HammerHead"], mat["Iron"], V3(0f, 0.16f, -0.16f), Q(0f, 0f, 0f), V3(1.08f, 0.52f, 1.18f));
            Part(root.transform, "brass_ankle_gear", mesh["GearRing"], mat["Brass"], V3(0f, 0.56f, 0.08f), Q(90f, 0f, 0f), V3(0.48f, 0.48f, 0.48f));
            Part(root.transform, "oily_piston_heel", mesh["Cylinder16"], mat["Grease"], V3(0f, 0.34f, 0.34f), Q(90f, 0f, 0f), V3(0.18f, 0.46f, 0.18f));
            Part(root.transform, "front_damage_scab_plate", mesh["ArmorPlate"], mat["Soot"], V3(0f, 0.20f, -0.54f), Q(0f, 0f, 0f), V3(0.74f, 0.34f, 0.80f));
            Socket(root.transform, "SOCK_AnkleMount", V3(0f, 0.78f, 0.08f));
            Socket(root.transform, "SOCK_FootContact", V3(0f, 0.02f, -0.10f));
            return root;
        }

        private static GameObject BuildBruteWeapon(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "brute_weapon_visual_part");
            if (variant == 0)
            {
                Part(root.transform, "oversized_saw_claw_disc", mesh["SawBlade"], mat["BluedSteel"], V3(0f, 0.52f, -0.18f), Q(0f, 90f, 0f), V3(1.06f, 1.06f, 1f));
                Part(root.transform, "hooking_upper_claw", mesh["ClawFinger"], mat["FreshCut"], V3(0f, 0.84f, -0.28f), Q(0f, 0f, -126f), V3(1.10f, 1.10f, 1.10f));
                Part(root.transform, "pressure_saw_motor", mesh["Cylinder32"], mat["Iron"], V3(0f, 0.52f, 0.10f), Q(0f, 0f, 90f), V3(0.34f, 0.22f, 0.34f));
            }
            else
            {
                Part(root.transform, "pressure_maul_strike_head", mesh["HammerHead"], mat["Iron"], V3(0f, 0.52f, -0.12f), Q(0f, 0f, 0f), V3(1.24f, 1.18f, 1.18f));
                Part(root.transform, "furnace_ram_core", mesh["Lens"], mat["Cinder"], V3(0f, 0.52f, -0.44f), Q(0f, 0f, 0f), V3(0.40f, 0.40f, 0.48f));
                Part(root.transform, "rear_piston_socket_pair", mesh["Cylinder16"], mat["Copper"], V3(0f, 0.92f, 0.24f), Q(90f, 0f, 0f), V3(0.24f, 0.44f, 0.24f));
            }

            Socket(root.transform, "SOCK_WristMount", V3(0f, 0.74f, 0.32f));
            Socket(root.transform, "SOCK_AttackTip", V3(0f, 0.52f, -0.72f));
            return root;
        }

        private static GameObject BuildBruteAttachment(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "brute_attachment_visual_part");
            if (variant == 0)
            {
                Part(root.transform, "left_shoulder_governor_gear", mesh["GearRing"], mat["Brass"], V3(-0.38f, 0.62f, 0f), Q(0f, 90f, 0f), V3(0.62f, 0.62f, 0.62f));
                Part(root.transform, "right_shoulder_governor_gear", mesh["GearRing"], mat["Brass"], V3(0.38f, 0.62f, 0f), Q(0f, 90f, 0f), V3(0.62f, 0.62f, 0.62f));
                Part(root.transform, "blackiron_yoke_crossbar", mesh["Box"], mat["Iron"], V3(0f, 0.62f, 0f), Q(0f, 0f, 0f), V3(1.10f, 0.10f, 0.16f));
            }
            else
            {
                Part(root.transform, "left_back_pressure_tank", mesh["PressureTank"], mat["Copper"], V3(-0.26f, 0.72f, 0f), Q(0f, 0f, 0f), V3(0.64f, 0.92f, 0.64f));
                Part(root.transform, "right_back_pressure_tank", mesh["PressureTank"], mat["Brass"], V3(0.26f, 0.72f, 0f), Q(0f, 0f, 0f), V3(0.64f, 0.92f, 0.64f));
                Part(root.transform, "shared_soot_mounting_frame", mesh["Box"], mat["Soot"], V3(0f, 0.72f, 0.28f), Q(0f, 0f, 0f), V3(0.88f, 0.58f, 0.10f));
                Part(root.transform, "top_red_release_valve", mesh["Cylinder16"], mat["Crimson"], V3(0f, 1.22f, -0.02f), Q(90f, 0f, 0f), V3(0.22f, 0.08f, 0.22f));
            }

            Socket(root.transform, "SOCK_BackOrShoulderMount", V3(0f, 0.62f, 0.34f));
            Socket(root.transform, "SOCK_SteamVFX", V3(0f, 1.16f, -0.04f));
            return root;
        }

        private static GameObject BuildSentryTorso(string name, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "sentry_ceiling_mount_torso_visual_part");
            Part(root.transform, "ceiling_mount_blackiron_plate", mesh["ArmorPlate"], mat["Iron"], V3(0f, 1.02f, 0f), Q(0f, 0f, 0f), V3(1.20f, 0.66f, 1f));
            Part(root.transform, "rotary_brass_hub", mesh["GearRing"], mat["Brass"], V3(0f, 0.72f, -0.04f), Q(0f, 0f, 0f), V3(0.92f, 0.92f, 0.92f));
            Part(root.transform, "central_hanging_pressure_core", mesh["FurnaceCore"], mat["Cinder"], V3(0f, 0.72f, -0.14f), Q(0f, 0f, 0f), V3(0.48f, 0.72f, 0.48f));
            Part(root.transform, "rear_copper_swivel_pipe", mesh["Cylinder16"], mat["Copper"], V3(0f, 0.72f, 0.42f), Q(90f, 0f, 0f), V3(0.15f, 0.56f, 0.15f));
            AddRivetsOnFront(root.transform, mesh, mat, "mount_plate_rivets", 10, 0.50f, 1.02f, -0.10f);
            Socket(root.transform, "SOCK_CeilingAnchor", V3(0f, 1.38f, 0f));
            Socket(root.transform, "SOCK_RotaryHead", V3(0f, 0.52f, -0.22f));
            Socket(root.transform, "SOCK_LeftWeapon", V3(-0.56f, 0.68f, -0.20f));
            Socket(root.transform, "SOCK_RightWeapon", V3(0.56f, 0.68f, -0.20f));
            return root;
        }

        private static GameObject BuildSentryHead(string name, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "sentry_rotary_optic_cluster_visual_part");
            Part(root.transform, "rotary_blackiron_eye_drum", mesh["Cylinder32"], mat["Iron"], V3(0f, 0.62f, 0f), Q(90f, 0f, 0f), V3(0.62f, 0.42f, 0.62f));
            for (int i = 0; i < 3; i++)
            {
                float angle = (i * 120f + 90f) * Mathf.Deg2Rad;
                Vector3 pos = V3(Mathf.Cos(angle) * 0.32f, 0.62f + Mathf.Sin(angle) * 0.20f, -0.30f);
                Part(root.transform, "amber_rotary_optic_" + i, mesh["Lens"], mat[i == 1 ? "TealGlass" : "AmberGlass"], pos, Q(0f, 0f, 0f), V3(0.24f, 0.24f, 0.34f));
                Part(root.transform, "brass_optic_bezel_" + i, mesh["GearRing"], mat["Brass"], pos + V3(0f, 0f, 0.04f), Q(0f, 0f, 0f), V3(0.30f, 0.30f, 0.30f));
            }

            Part(root.transform, "top_pressure_gauge", mesh["Lens"], mat["Ivory"], V3(0f, 1.02f, 0f), Q(0f, 0f, 0f), V3(0.24f, 0.24f, 0.26f));
            Socket(root.transform, "SOCK_RotaryMount", V3(0f, 0.88f, 0.20f));
            Socket(root.transform, "SOCK_OpticGlow", V3(0f, 0.62f, -0.58f));
            return root;
        }

        private static GameObject BuildSentryLimb(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "sentry_mount_limb_visual_part");
            if (variant == 0)
            {
                Part(root.transform, "wall_clamp_blackiron_plate", mesh["ArmorPlate"], mat["Iron"], V3(0f, 0.72f, 0.16f), Q(0f, 0f, 0f), V3(0.86f, 0.68f, 0.90f));
                Part(root.transform, "telescoping_wall_strut", mesh["TripodStrut"], mat["Gunmetal"], V3(0f, 0.52f, -0.34f), Q(90f, 0f, 0f), V3(1.0f, 0.90f, 1.0f));
                Part(root.transform, "copper_strut_piston", mesh["Cylinder16"], mat["Copper"], V3(0.16f, 0.52f, -0.34f), Q(90f, 0f, 0f), V3(0.06f, 0.82f, 0.06f));
            }
            else
            {
                Part(root.transform, "ceiling_rail_hook", mesh["ClawFinger"], mat["FreshCut"], V3(0f, 0.86f, 0f), Q(0f, 0f, 116f), V3(1.24f, 1.24f, 1.24f));
                Part(root.transform, "hanging_brass_shackle", mesh["GearRing"], mat["Brass"], V3(0f, 0.58f, 0f), Q(90f, 0f, 0f), V3(0.52f, 0.52f, 0.52f));
                Part(root.transform, "blackiron_drop_strut", mesh["TripodStrut"], mat["Iron"], V3(0f, 0.34f, -0.02f), Q(0f, 0f, 0f), V3(1.0f, 0.72f, 1.0f));
            }

            Socket(root.transform, "SOCK_WallOrCeilingMount", V3(0f, 0.96f, 0.22f));
            Socket(root.transform, "SOCK_SentryBody", V3(0f, 0.32f, -0.38f));
            return root;
        }

        private static GameObject BuildSentryWeapon(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "sentry_weapon_visual_part");
            if (variant == 0)
            {
                Part(root.transform, "left_steam_lance_barrel", mesh["Cylinder16"], mat["Gunmetal"], V3(-0.16f, 0.52f, -0.44f), Q(90f, 0f, 0f), V3(0.08f, 1.02f, 0.08f));
                Part(root.transform, "right_steam_lance_barrel", mesh["Cylinder16"], mat["Gunmetal"], V3(0.16f, 0.52f, -0.44f), Q(90f, 0f, 0f), V3(0.08f, 1.02f, 0.08f));
                Part(root.transform, "brass_lance_cooling_jacket", mesh["Box"], mat["Brass"], V3(0f, 0.52f, -0.12f), Q(0f, 0f, 0f), V3(0.54f, 0.18f, 0.22f));
                Part(root.transform, "white_hot_lance_tips", mesh["FurnaceCore"], mat["WhiteHot"], V3(0f, 0.52f, -0.98f), Q(90f, 0f, 0f), V3(0.20f, 0.32f, 0.20f));
            }
            else
            {
                Part(root.transform, "mini_saw_turret_disc", mesh["SawBlade"], mat["Hazard"], V3(0f, 0.54f, -0.34f), Q(0f, 90f, 0f), V3(0.72f, 0.72f, 0.72f));
                Part(root.transform, "rotary_saw_motor_pod", mesh["Cylinder32"], mat["Iron"], V3(0f, 0.54f, 0.00f), Q(0f, 0f, 90f), V3(0.34f, 0.24f, 0.34f));
                Part(root.transform, "amber_targeting_lens", mesh["Lens"], mat["AmberGlass"], V3(0f, 0.88f, -0.12f), Q(0f, 0f, 0f), V3(0.22f, 0.22f, 0.28f));
            }

            Socket(root.transform, "SOCK_TurretMount", V3(0f, 0.54f, 0.28f));
            Socket(root.transform, "SOCK_MuzzleOrBladeTip", V3(0f, 0.54f, -1.10f));
            return root;
        }

        private static GameObject BuildSentryAttachment(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "sentry_attachment_visual_part");
            if (variant == 0)
            {
                Part(root.transform, "cable_hose_halo", mesh["HoseArc"], mat["Rubber"], V3(0f, 0.56f, 0f), Q(0f, 0f, 0f), V3(1.08f, 1.08f, 1f));
                Part(root.transform, "brass_cable_collar_left", mesh["Cylinder16"], mat["Brass"], V3(-0.52f, 0.56f, 0f), Q(0f, 0f, 90f), V3(0.16f, 0.10f, 0.16f));
                Part(root.transform, "brass_cable_collar_right", mesh["Cylinder16"], mat["Brass"], V3(0.52f, 0.56f, 0f), Q(0f, 0f, 90f), V3(0.16f, 0.10f, 0.16f));
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Part(root.transform, "short_pressure_bottle_" + i, mesh["PressureTank"], mat[i == 1 ? "Brass" : "Copper"], V3((i - 1) * 0.26f, 0.56f, 0f), Q(0f, 0f, 0f), V3(0.36f, 0.62f, 0.36f));
                }
                Part(root.transform, "blackiron_bottle_rack", mesh["Box"], mat["Iron"], V3(0f, 0.56f, 0.22f), Q(0f, 0f, 0f), V3(0.92f, 0.40f, 0.10f));
            }

            Socket(root.transform, "SOCK_AttachmentMount", V3(0f, 0.56f, 0.30f));
            Socket(root.transform, "SOCK_SteamVent", V3(0f, 0.92f, 0f));
            return root;
        }

        private static GameObject BuildSharedPart(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "shared_clockwork_part_visual");
            if (variant == 0)
            {
                Eye(root.transform, mesh, mat, "single_serviceable", V3(0f, 0.56f, 0f), 0.46f, "AmberGlass");
                Socket(root.transform, "SOCK_HeadFront", V3(0f, 0.56f, 0.18f));
            }
            else if (variant == 1)
            {
                Part(root.transform, "large_exposed_governor_gear", mesh["GearRing"], mat["Brass"], V3(0f, 0.58f, 0f), Q(0f, 0f, 0f), V3(0.86f, 0.86f, 0.86f));
                Part(root.transform, "small_offset_governor_gear", mesh["GearRing"], mat["Copper"], V3(0.42f, 0.86f, -0.02f), Q(0f, 0f, 20f), V3(0.44f, 0.44f, 0.44f));
                Part(root.transform, "central_axle_rivet", mesh["Cylinder16"], mat["Rivet"], V3(0f, 0.58f, -0.08f), Q(0f, 0f, 90f), V3(0.22f, 0.10f, 0.22f));
                Socket(root.transform, "SOCK_GearAxis", V3(0f, 0.58f, 0.14f));
            }
            else if (variant == 2)
            {
                Part(root.transform, "ivory_pressure_dial_face", mesh["Lens"], mat["Ivory"], V3(0f, 0.56f, -0.04f), Q(0f, 0f, 0f), V3(0.46f, 0.46f, 0.32f));
                Part(root.transform, "brass_dial_bezel", mesh["GearRing"], mat["Brass"], V3(0f, 0.56f, 0.02f), Q(0f, 0f, 0f), V3(0.58f, 0.58f, 0.58f));
                Part(root.transform, "red_pressure_needle", mesh["Box"], mat["Crimson"], V3(0.05f, 0.60f, -0.18f), Q(0f, 0f, -34f), V3(0.045f, 0.34f, 0.035f));
                Socket(root.transform, "SOCK_GaugeMount", V3(0f, 0.56f, 0.16f));
            }
            else
            {
                Part(root.transform, "blackened_damage_scab_plate", mesh["ArmorPlate"], mat["Soot"], V3(0f, 0.58f, 0f), Q(0f, 0f, -8f), V3(0.86f, 0.58f, 0.90f));
                Part(root.transform, "chipped_ochre_warning_stripe", mesh["Box"], mat["Hazard"], V3(0f, 0.58f, -0.08f), Q(0f, 0f, 22f), V3(0.68f, 0.055f, 0.050f));
                AddRivetsOnFront(root.transform, mesh, mat, "damage_plate_rivets", 7, 0.35f, 0.58f, -0.10f);
                Socket(root.transform, "SOCK_DamagePlateMount", V3(0f, 0.58f, 0.14f));
            }

            return root;
        }

        private static GameObject BuildArchetypePreview(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "archetype_preview_visual_only_assembly");
            if (variant == 0)
            {
                Part(root.transform, "skitter_preview_low_boiler", mesh["BoilerTorso"], mat["Brass"], V3(0f, 0.62f, 0f), Q(0f, 0f, 90f), V3(0.78f, 1.34f, 0.58f));
                Part(root.transform, "skitter_preview_twin_optic_head", mesh["ArmorPlate"], mat["Iron"], V3(0f, 0.76f, -0.58f), Q(0f, 0f, 0f), V3(0.78f, 0.34f, 0.70f));
                Eye(root.transform, mesh, mat, "preview_left", V3(-0.20f, 0.78f, -0.70f), 0.24f, "AmberGlass");
                Eye(root.transform, mesh, mat, "preview_right", V3(0.20f, 0.78f, -0.70f), 0.24f, "AmberGlass");
                for (int i = 0; i < 4; i++)
                {
                    float side = i < 2 ? -1f : 1f;
                    float z = i % 2 == 0 ? -0.24f : 0.28f;
                    Part(root.transform, "preview_skitter_leg_" + i, mesh["TripodStrut"], mat["Iron"], V3(side * 0.62f, 0.36f, z), Q(0f, 0f, side * -32f), V3(1f, 0.72f, 1f));
                    Part(root.transform, "preview_skitter_foot_" + i, mesh["ArmorPlate"], mat["AshArmor"], V3(side * 0.88f, 0.06f, z - 0.05f), Q(0f, 0f, 0f), V3(0.38f, 0.18f, 0.42f));
                }
                Part(root.transform, "preview_belly_saw", mesh["SawBlade"], mat["BluedSteel"], V3(0f, 0.34f, -0.30f), Q(0f, 90f, 0f), V3(0.58f, 0.58f, 0.58f));
                Part(root.transform, "preview_back_tick_tank", mesh["PressureTank"], mat["Copper"], V3(0f, 0.92f, 0.38f), Q(0f, 0f, 90f), V3(0.42f, 0.58f, 0.42f));
            }
            else if (variant == 1)
            {
                Part(root.transform, "brute_preview_boiler_torso", mesh["BoilerTorso"], mat["Brass"], V3(0f, 1.18f, 0f), Q(0f, 0f, 0f), V3(1.20f, 1.38f, 0.86f));
                Part(root.transform, "brute_preview_chest_core", mesh["Lens"], mat["AmberGlass"], V3(0f, 1.18f, -0.62f), Q(0f, 0f, 0f), V3(0.54f, 0.54f, 0.58f));
                Part(root.transform, "brute_preview_visor_head", mesh["ArmorPlate"], mat["Iron"], V3(0f, 2.04f, -0.04f), Q(0f, 0f, 0f), V3(0.88f, 0.46f, 0.84f));
                Part(root.transform, "brute_preview_amber_visor_slit", mesh["Box"], mat["WhiteHot"], V3(0f, 2.04f, -0.20f), Q(0f, 0f, 0f), V3(0.62f, 0.08f, 0.08f));
                Part(root.transform, "brute_preview_left_arm", mesh["CutterArm"], mat["Iron"], V3(-0.88f, 1.24f, -0.06f), Q(0f, 0f, 24f), V3(1f, 1f, 1.12f));
                Part(root.transform, "brute_preview_right_saw_arm", mesh["SawBlade"], mat["BluedSteel"], V3(0.98f, 1.06f, -0.34f), Q(0f, 90f, 0f), V3(0.92f, 0.92f, 0.92f));
                Part(root.transform, "brute_preview_left_foot", mesh["HammerHead"], mat["Iron"], V3(-0.34f, 0.12f, -0.10f), Q(0f, 0f, 0f), V3(0.78f, 0.36f, 0.86f));
                Part(root.transform, "brute_preview_right_foot", mesh["HammerHead"], mat["Iron"], V3(0.34f, 0.12f, -0.10f), Q(0f, 0f, 0f), V3(0.78f, 0.36f, 0.86f));
                Part(root.transform, "brute_preview_back_tanks", mesh["PressureTank"], mat["Copper"], V3(0f, 1.34f, 0.56f), Q(0f, 0f, 0f), V3(0.72f, 0.96f, 0.72f));
            }
            else
            {
                Part(root.transform, "sentry_preview_ceiling_plate", mesh["ArmorPlate"], mat["Iron"], V3(0f, 1.88f, 0f), Q(0f, 0f, 0f), V3(1.34f, 0.58f, 1f));
                Part(root.transform, "sentry_preview_hanging_hub", mesh["GearRing"], mat["Brass"], V3(0f, 1.34f, -0.02f), Q(0f, 0f, 0f), V3(0.92f, 0.92f, 0.92f));
                Part(root.transform, "sentry_preview_rotary_head", mesh["Cylinder32"], mat["Iron"], V3(0f, 0.98f, -0.22f), Q(90f, 0f, 0f), V3(0.58f, 0.38f, 0.58f));
                Eye(root.transform, mesh, mat, "sentry_preview_center", V3(0f, 0.98f, -0.54f), 0.34f, "AmberGlass");
                Part(root.transform, "sentry_preview_left_lance", mesh["Cylinder16"], mat["Gunmetal"], V3(-0.34f, 0.86f, -0.76f), Q(90f, 0f, 0f), V3(0.07f, 0.88f, 0.07f));
                Part(root.transform, "sentry_preview_right_lance", mesh["Cylinder16"], mat["Gunmetal"], V3(0.34f, 0.86f, -0.76f), Q(90f, 0f, 0f), V3(0.07f, 0.88f, 0.07f));
                Part(root.transform, "sentry_preview_hose_halo", mesh["HoseArc"], mat["Rubber"], V3(0f, 1.24f, 0.24f), Q(0f, 90f, 0f), V3(0.96f, 0.96f, 1f));
                Part(root.transform, "sentry_preview_pressure_rack", mesh["PressureTank"], mat["Copper"], V3(0f, 1.44f, 0.48f), Q(0f, 0f, 90f), V3(0.42f, 0.72f, 0.42f));
            }

            Socket(root.transform, "SOCK_RigFuture_Root", V3(0f, 0f, 0f));
            Socket(root.transform, "SOCK_VFX_AmberOptics", V3(0f, variant == 1 ? 2.04f : 0.98f, -0.66f));
            Socket(root.transform, "SOCK_VFX_SteamVent", V3(0f, variant == 0 ? 1.06f : 1.84f, 0.48f));
            return root;
        }

        private static void SavePrefab(GameObject root)
        {
            string path = PrefabRoot + "/" + root.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(root, path);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static GameObject BuildBoilerTorso(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "boiler_torso_visual_part");
            float width = variant == 2 ? 0.72f : variant == 1 ? 1.05f : 1.18f;
            float height = variant == 2 ? 1.42f : variant == 1 ? 0.88f : 1.18f;
            Part(root.transform, "boiler_aged_brass_shell", mesh["BoilerTorso"], mat["Brass"], V3(0f, 0.70f, 0f), Q(0f, 0f, 0f), V3(width, height, width * 0.72f));
            Part(root.transform, "dark_iron_belly_band", mesh["Cylinder32"], mat["Iron"], V3(0f, 0.72f, 0f), Q(0f, 0f, 0f), V3(width * 1.05f, 0.09f, width * 0.78f));
            Part(root.transform, "furnace_grate_plate", mesh["ArmorPlate"], mat[variant == 2 ? "Gunmetal" : "AshArmor"], V3(0f, 0.73f, -0.36f * width), Q(0f, 0f, 0f), V3(0.62f, 0.55f, 1f));
            Part(root.transform, "amber_furnace_window", mesh["Lens"], mat["AmberGlass"], V3(0f, 0.76f, -0.41f * width), Q(0f, 0f, 0f), V3(0.52f, 0.52f, 0.70f));
            for (int i = 0; i < 3; i++)
            {
                Part(root.transform, "ribbed_boiler_band_" + i, mesh["Cylinder32"], mat["Iron"], V3(0f, 0.26f + i * height * 0.30f, 0f), Q(0f, 0f, 0f), V3(width * 1.08f, 0.035f, width * 0.80f));
            }

            AddRivetsOnFront(root.transform, mesh, mat, "front_rivets", 10, width * 0.40f, 0.72f, -0.43f * width);
            Socket(root.transform, "SOCK_ChestCore", V3(0f, 0.76f, -0.52f * width));
            Socket(root.transform, "SOCK_BackTank", V3(0f, 0.92f, 0.43f * width));
            return root;
        }

        private static GameObject BuildCutterArm(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "cutter_arm_visual_part");
            float sign = variant == 1 ? -1f : 1f;
            Part(root.transform, "upper_cutter_arm_segment", mesh["CutterArm"], mat["Iron"], V3(0f, 0.56f, 0f), Q(0f, 18f * sign, 82f * sign), V3(1f, 1f, variant == 2 ? 1.18f : 1f));
            Part(root.transform, "brass_elbow_cap", mesh["GearRing"], mat["Brass"], V3(0.42f * sign, 0.47f, -0.05f), Q(0f, 90f, 0f), V3(0.58f, 0.58f, 0.58f));
            Part(root.transform, "lower_blackiron_piston", mesh["Cylinder16"], mat["Gunmetal"], V3(0.18f * sign, 0.35f, -0.15f), Q(0f, 0f, 70f * sign), V3(0.10f, 0.64f, 0.10f));
            Part(root.transform, "copper_pressure_line", mesh["Cylinder16"], mat["Copper"], V3(0.04f * sign, 0.69f, -0.19f), Q(0f, 0f, 68f * sign), V3(0.052f, 0.76f, 0.052f));
            if (variant == 1)
            {
                Part(root.transform, "shear_outer_claw", mesh["ClawFinger"], mat["FreshCut"], V3(0.74f * sign, 0.28f, -0.20f), Q(0f, 0f, -34f * sign), V3(1.0f * sign, 1f, 1f));
                Part(root.transform, "shear_inner_claw", mesh["ClawFinger"], mat["BluedSteel"], V3(0.62f * sign, 0.42f, -0.19f), Q(0f, 0f, 148f * sign), V3(0.82f * sign, 0.82f, 0.82f));
            }
            else
            {
                Part(root.transform, variant == 2 ? "overcrank_heat_saw_disc" : "cutter_saw_disc", mesh["SawBlade"], mat[variant == 2 ? "BluedSteel" : "Hazard"], V3(0.78f * sign, 0.26f, -0.20f), Q(0f, 90f, 0f), V3(0.92f, 0.92f, 0.92f));
                Part(root.transform, "saw_bright_rivet_hub", mesh["Cylinder16"], mat["Rivet"], V3(0.78f * sign, 0.26f, -0.20f), Q(0f, 0f, 90f), V3(0.20f, 0.10f, 0.20f));
            }

            Socket(root.transform, "SOCK_ShoulderMount", V3(-0.48f * sign, 0.72f, 0f));
            Socket(root.transform, "SOCK_CutterTip", V3(0.92f * sign, 0.24f, -0.20f));
            return root;
        }

        private static GameObject BuildTripodLeg(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "tripod_leg_visual_part");
            float spread = variant == 1 ? 0.42f : variant == 2 ? 0.72f : 0.56f;
            float height = variant == 1 ? 0.92f : 0.76f;
            Part(root.transform, "central_hip_socket", mesh["GearRing"], mat["Brass"], V3(0f, height, 0f), Q(90f, 0f, 0f), V3(0.64f, 0.64f, 0.64f));
            for (int i = 0; i < 3; i++)
            {
                float a = (120f * i + 25f) * Mathf.Deg2Rad;
                Vector3 foot = V3(Mathf.Cos(a) * spread, 0.10f, Mathf.Sin(a) * spread);
                Vector3 mid = Vector3.Lerp(V3(0f, height, 0f), foot, 0.5f);
                Quaternion rot = Quaternion.LookRotation((foot - V3(0f, height, 0f)).normalized, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
                Part(root.transform, "tripod_strut_" + i, mesh["TripodStrut"], mat[i == 1 ? "Gunmetal" : "Iron"], mid, rot, V3(1.0f, (foot - V3(0f, height, 0f)).magnitude, 1.0f));
                Part(root.transform, "spiked_foot_" + i, mesh["FurnaceCore"], mat[variant == 1 ? "FreshCut" : "AshArmor"], foot, Q(0f, 0f, 180f), V3(0.28f, 0.30f, 0.28f));
            }

            Part(root.transform, "amber_balance_lamp", mesh["Lens"], mat["AmberGlass"], V3(0f, height + 0.04f, -0.33f), Q(0f, 0f, 0f), V3(0.30f, 0.30f, 0.55f));
            Socket(root.transform, "SOCK_HipMount", V3(0f, height + 0.20f, 0f));
            return root;
        }

        private static GameObject BuildPressureTank(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "pressure_tank_visual_part");
            if (variant == 0)
            {
                Part(root.transform, "left_capsule_tank", mesh["PressureTank"], mat["Copper"], V3(-0.22f, 0.64f, 0f), Q(0f, 0f, 0f), V3(0.72f, 0.86f, 0.72f));
                Part(root.transform, "right_capsule_tank", mesh["PressureTank"], mat["Brass"], V3(0.22f, 0.64f, 0f), Q(0f, 0f, 0f), V3(0.72f, 0.86f, 0.72f));
                Part(root.transform, "shared_iron_yoke", mesh["Box"], mat["Iron"], V3(0f, 0.64f, -0.26f), Q(0f, 0f, 0f), V3(0.72f, 0.42f, 0.12f));
            }
            else
            {
                Part(root.transform, "main_ribbed_pressure_tank", mesh["PressureTank"], mat[variant == 1 ? "Brass" : "Gunmetal"], V3(0f, 0.66f, 0f), Q(0f, 0f, 90f), V3(0.82f, variant == 1 ? 1.10f : 0.76f, 0.82f));
            }

            for (int i = 0; i < 5; i++)
            {
                Part(root.transform, "pressure_rib_band_" + i, mesh["Cylinder32"], mat["Iron"], V3(-0.36f + i * 0.18f, 0.66f, 0f), Q(0f, 0f, 90f), V3(0.44f, 0.035f, 0.44f));
            }

            Part(root.transform, "red_pressure_valve", mesh["Cylinder16"], mat["Crimson"], V3(0f, 1.06f, -0.02f), Q(90f, 0f, 0f), V3(0.34f, 0.06f, 0.34f));
            Part(root.transform, "ivory_pressure_gauge", mesh["Lens"], mat["Ivory"], V3(0f, 0.66f, -0.38f), Q(0f, 0f, 0f), V3(0.30f, 0.30f, 0.38f));
            Socket(root.transform, "SOCK_BackMount", V3(0f, 0.66f, 0.38f));
            Socket(root.transform, "SOCK_SteamVent", V3(0f, 1.18f, -0.04f));
            return root;
        }

        private static GameObject BuildEyeLens(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "eye_lens_visual_part");
            if (variant == 0)
            {
                Eye(root.transform, mesh, mat, "single", V3(0f, 0.55f, 0f), 0.58f, "AmberGlass");
            }
            else if (variant == 1)
            {
                Eye(root.transform, mesh, mat, "left", V3(-0.30f, 0.55f, 0f), 0.38f, "AmberGlass");
                Eye(root.transform, mesh, mat, "center", V3(0f, 0.62f, -0.02f), 0.42f, "TealGlass");
                Eye(root.transform, mesh, mat, "right", V3(0.30f, 0.55f, 0f), 0.38f, "AmberGlass");
            }
            else
            {
                Part(root.transform, "slit_blackiron_visor", mesh["ArmorPlate"], mat["Iron"], V3(0f, 0.56f, 0f), Q(0f, 0f, 0f), V3(1.22f, 0.44f, 1.0f));
                Part(root.transform, "horizontal_furnace_slit", mesh["Box"], mat["WhiteHot"], V3(0f, 0.57f, -0.06f), Q(0f, 0f, 0f), V3(0.94f, 0.085f, 0.08f));
                AddRivetsOnFront(root.transform, mesh, mat, "visor_rivets", 8, 0.52f, 0.55f, -0.08f);
            }

            Socket(root.transform, "SOCK_HeadFront", V3(0f, 0.55f, 0.16f));
            return root;
        }

        private static void Eye(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string label, Vector3 pos, float scale, string glass)
        {
            Part(parent, label + "_gear_bezel", mesh["GearRing"], mat["Brass"], pos + V3(0f, 0f, 0.02f), Q(0f, 0f, 0f), V3(scale, scale, scale));
            Part(parent, label + "_glass_lens", mesh["Lens"], mat[glass], pos + V3(0f, 0f, -0.05f), Q(0f, 0f, 0f), V3(scale * 0.68f, scale * 0.68f, scale * 0.55f));
            Part(parent, label + "_upper_eyelid", mesh["Box"], mat["Iron"], pos + V3(0f, scale * 0.22f, -0.08f), Q(0f, 0f, 0f), V3(scale * 0.70f, scale * 0.08f, scale * 0.08f));
        }

        private static GameObject BuildFurnaceCore(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "furnace_core_visual_part");
            Part(root.transform, "core_outer_gear_gate", mesh["GearRing"], mat["Iron"], V3(0f, 0.62f, 0f), Q(0f, 0f, 0f), V3(variant == 1 ? 1.10f : 0.92f, variant == 1 ? 1.10f : 0.92f, 0.92f));
            Part(root.transform, "glowing_furnace_prism", mesh["FurnaceCore"], mat[variant == 2 ? "WhiteHot" : "Cinder"], V3(0f, 0.62f, -0.08f), Q(0f, 0f, 0f), V3(0.76f, variant == 1 ? 1.10f : 0.82f, 0.76f));
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90f;
                Part(root.transform, "blackiron_cage_bar_" + i, mesh["Box"], mat["Gunmetal"], V3(0f, 0.62f, -0.16f), Q(0f, 0f, angle), V3(0.08f, 0.94f, 0.08f));
            }

            if (variant == 2)
            {
                Part(root.transform, "weakpoint_crimson_pressure_ring", mesh["GearRing"], mat["Crimson"], V3(0f, 0.62f, -0.20f), Q(0f, 0f, 0f), V3(1.16f, 1.16f, 1.16f));
            }

            Socket(root.transform, "SOCK_ChestInsert", V3(0f, 0.62f, 0.18f));
            Socket(root.transform, "SOCK_FurnaceGlow", V3(0f, 0.62f, -0.35f));
            return root;
        }

        private static GameObject BuildGearShoulder(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "gear_shoulder_visual_part");
            float scale = variant == 2 ? 1.10f : 0.86f;
            Part(root.transform, "main_gear_shoulder_ring", mesh["GearRing"], mat[variant == 1 ? "Iron" : "Brass"], V3(0f, 0.62f, 0f), Q(0f, 90f, 0f), V3(scale, scale, scale));
            Part(root.transform, "inner_rivet_hub", mesh["Cylinder32"], mat["Rivet"], V3(0f, 0.62f, 0f), Q(0f, 0f, 90f), V3(0.34f, 0.14f, 0.34f));
            if (variant >= 1)
            {
                Part(root.transform, "paired_secondary_gear", mesh["GearRing"], mat["Gunmetal"], V3(0.42f, 0.82f, 0.02f), Q(0f, 90f, 0f), V3(0.54f, 0.54f, 0.54f));
                Part(root.transform, "copper_yoke_bridge", mesh["Box"], mat["Copper"], V3(0.24f, 0.72f, 0f), Q(0f, 0f, -26f), V3(0.62f, 0.10f, 0.14f));
            }

            if (variant == 2)
            {
                Part(root.transform, "heavy_governor_counterweight", mesh["Cylinder16"], mat["AshArmor"], V3(-0.38f, 0.30f, 0f), Q(0f, 0f, 90f), V3(0.24f, 0.22f, 0.24f));
                Part(root.transform, "amber_status_lens", mesh["Lens"], mat["AmberGlass"], V3(0.0f, 0.62f, -0.20f), Q(0f, 0f, 0f), V3(0.24f, 0.24f, 0.34f));
            }

            Socket(root.transform, "SOCK_TorsoSide", V3(-0.12f, 0.62f, 0.20f));
            Socket(root.transform, "SOCK_ArmHinge", V3(0.30f, 0.62f, -0.20f));
            return root;
        }

        private static GameObject BuildSawBlade(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "saw_blade_visual_part");
            Part(root.transform, "toothed_saw_disc", mesh["SawBlade"], mat[variant == 2 ? "BluedSteel" : variant == 1 ? "FreshCut" : "Crimson"], V3(0f, 0.58f, 0f), Q(0f, 90f, 0f), V3(variant == 1 ? 0.72f : 1.0f, variant == 1 ? 0.72f : 1.0f, 1f));
            if (variant == 1)
            {
                Part(root.transform, "second_offset_halfmoon_saw", mesh["SawBlade"], mat["Hazard"], V3(0.14f, 0.58f, -0.03f), Q(0f, 90f, 34f), V3(0.70f, 0.70f, 1f));
            }

            Part(root.transform, "saw_axis_hub", mesh["Cylinder16"], mat["Rivet"], V3(0f, 0.58f, -0.01f), Q(0f, 0f, 90f), V3(0.28f, 0.10f, 0.28f));
            AddRivetsOnFront(root.transform, mesh, mat, "saw_hub_rivets", 6, 0.17f, 0.58f, -0.08f);
            Socket(root.transform, "SOCK_SawAxis", V3(0f, 0.58f, 0.12f));
            return root;
        }

        private static GameObject BuildHammerHead(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "hammer_head_visual_part");
            Part(root.transform, "hammer_strike_block", mesh["HammerHead"], mat[variant == 2 ? "AshArmor" : "Iron"], V3(0f, 0.62f, 0f), Q(0f, 0f, 0f), V3(variant == 1 ? 1.18f : 1f, variant == 2 ? 1.18f : 1f, variant == 2 ? 1.12f : 1f));
            Part(root.transform, "brass_side_cap_left", mesh["Cylinder16"], mat["Brass"], V3(-0.46f, 0.62f, 0f), Q(0f, 0f, 90f), V3(0.34f, 0.08f, 0.34f));
            Part(root.transform, "brass_side_cap_right", mesh["Cylinder16"], mat["Brass"], V3(0.46f, 0.62f, 0f), Q(0f, 0f, 90f), V3(0.34f, 0.08f, 0.34f));
            Part(root.transform, "rear_piston_socket", mesh["Cylinder16"], mat["Copper"], V3(0f, 0.96f, 0.22f), Q(90f, 0f, 0f), V3(0.18f, 0.34f, 0.18f));
            if (variant == 2)
            {
                Part(root.transform, "furnace_ram_glow", mesh["Lens"], mat["Cinder"], V3(0f, 0.62f, -0.24f), Q(0f, 0f, 0f), V3(0.38f, 0.38f, 0.42f));
            }

            AddRivetsOnFront(root.transform, mesh, mat, "hammer_face_rivets", 8, 0.34f, 0.62f, -0.23f);
            Socket(root.transform, "SOCK_HandleMount", V3(0f, 1.10f, 0.24f));
            Socket(root.transform, "SOCK_StrikeFace", V3(0f, 0.62f, -0.34f));
            return root;
        }

        private static GameObject BuildClawHand(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "claw_hand_visual_part");
            Part(root.transform, "blackiron_palm_block", mesh["Box"], mat["Iron"], V3(0f, 0.56f, 0f), Q(0f, 0f, 0f), V3(0.42f, 0.32f, 0.32f));
            int count = variant == 2 ? 4 : 3;
            for (int i = 0; i < count; i++)
            {
                float offset = (i - (count - 1) * 0.5f) * 0.15f;
                Part(root.transform, "articulated_claw_finger_" + i, mesh["ClawFinger"], mat[variant == 2 ? "FreshCut" : "Brass"], V3(offset, 0.52f, -0.22f), Q(0f, 0f, -40f + i * 22f), V3(variant == 1 ? 1.18f : 0.92f, variant == 1 ? 1.18f : 0.92f, 0.92f));
                Part(root.transform, "knuckle_rivet_" + i, mesh["Cylinder16"], mat["Rivet"], V3(offset, 0.56f, -0.16f), Q(90f, 0f, 0f), V3(0.12f, 0.05f, 0.12f));
            }

            if (variant == 1)
            {
                Part(root.transform, "crusher_lower_jaw", mesh["ArmorPlate"], mat["Gunmetal"], V3(0f, 0.31f, -0.22f), Q(0f, 0f, 180f), V3(0.50f, 0.30f, 0.70f));
            }

            Socket(root.transform, "SOCK_WristMount", V3(0f, 0.56f, 0.24f));
            Socket(root.transform, "SOCK_ClawGrabCenter", V3(0f, 0.48f, -0.54f));
            return root;
        }

        private static GameObject BuildHose(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "hose_visual_part");
            Part(root.transform, "arched_oily_hose", mesh["HoseArc"], mat[variant == 1 ? "Leather" : "Rubber"], V3(0f, 0.52f, 0f), Q(0f, variant == 2 ? 90f : 0f, 0f), V3(variant == 0 ? 1.10f : 0.86f, variant == 0 ? 1.10f : 0.86f, 1f));
            Part(root.transform, "left_brass_collar", mesh["Cylinder16"], mat["Brass"], V3(-0.52f, 0.52f, 0f), Q(0f, 0f, 90f), V3(0.18f, 0.12f, 0.18f));
            Part(root.transform, "right_brass_collar", mesh["Cylinder16"], mat["Brass"], V3(0.52f, 0.52f, 0f), Q(0f, 0f, 90f), V3(0.18f, 0.12f, 0.18f));
            if (variant == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    Part(root.transform, "bellows_clamp_" + i, mesh["Cylinder16"], mat["Gunmetal"], V3(-0.34f + i * 0.17f, 0.70f, 0f), Q(0f, 0f, 90f), V3(0.14f, 0.035f, 0.14f));
                }
            }

            Socket(root.transform, "SOCK_HoseA", V3(-0.58f, 0.52f, 0f));
            Socket(root.transform, "SOCK_HoseB", V3(0.58f, 0.52f, 0f));
            return root;
        }

        private static GameObject BuildArmorPlate(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "armor_plate_visual_part");
            Part(root.transform, "main_riveted_armor_plate", mesh["ArmorPlate"], mat[variant == 0 ? "AshArmor" : "Iron"], V3(0f, 0.62f, 0f), Q(0f, 0f, variant == 1 ? 14f : 0f), V3(variant == 0 ? 1.15f : 0.86f, variant == 2 ? 0.74f : 1f, 1f));
            Part(root.transform, "chipped_hazard_stripe", mesh["Box"], mat[variant == 1 ? "Hazard" : "Crimson"], V3(0f, 0.62f, -0.07f), Q(0f, 0f, -18f), V3(variant == 0 ? 0.86f : 0.62f, 0.055f, 0.055f));
            AddRivetsOnFront(root.transform, mesh, mat, "armor_rivets", variant == 0 ? 12 : 8, variant == 0 ? 0.46f : 0.34f, 0.62f, -0.09f);
            if (variant == 1)
            {
                Part(root.transform, "overlap_lame_plate", mesh["ArmorPlate"], mat["Gunmetal"], V3(0.16f, 0.42f, 0.02f), Q(0f, 0f, -8f), V3(0.72f, 0.38f, 0.78f));
            }

            Socket(root.transform, "SOCK_ArmorMount", V3(0f, 0.62f, 0.14f));
            return root;
        }

        private static GameObject BuildBackFlywheel(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "back_flywheel_visual_part");
            Part(root.transform, "main_back_flywheel", mesh["Flywheel"], mat[variant == 1 ? "Iron" : "Brass"], V3(0f, 0.72f, 0f), Q(0f, 0f, 0f), V3(variant == 1 ? 1.18f : 0.92f, variant == 1 ? 1.18f : 0.92f, 1f));
            for (int i = 0; i < 6; i++)
            {
                Part(root.transform, "flywheel_spoke_" + i, mesh["Box"], mat["Rivet"], V3(0f, 0.72f, -0.02f), Q(0f, 0f, i * 30f), V3(0.055f, variant == 1 ? 0.92f : 0.70f, 0.055f));
            }

            Part(root.transform, "central_axle_hub", mesh["Cylinder16"], mat["Gunmetal"], V3(0f, 0.72f, -0.08f), Q(0f, 0f, 90f), V3(0.26f, 0.16f, 0.26f));
            if (variant == 0)
            {
                Part(root.transform, "secondary_governor_wheel", mesh["Flywheel"], mat["Copper"], V3(0.54f, 0.92f, 0.02f), Q(0f, 0f, 0f), V3(0.48f, 0.48f, 0.48f));
            }
            else if (variant == 2)
            {
                Part(root.transform, "protective_scrapper_cage", mesh["GearRing"], mat["Gunmetal"], V3(0f, 0.72f, -0.13f), Q(0f, 0f, 15f), V3(1.22f, 1.22f, 1.22f));
            }

            Socket(root.transform, "SOCK_BackSpine", V3(0f, 0.72f, 0.22f));
            Socket(root.transform, "SOCK_FlywheelAxis", V3(0f, 0.72f, -0.22f));
            return root;
        }

        private static GameObject BuildPosePreview(string name, int variant, Dictionary<string, Material> mat, Dictionary<string, Mesh> mesh)
        {
            GameObject root = Root(name, "pose_preview_visual_only_assembly");
            float torsoWidth = variant == 1 ? 0.62f : variant == 2 ? 1.24f : 0.90f;
            float torsoHeight = variant == 1 ? 1.38f : variant == 2 ? 1.18f : 0.98f;
            Part(root.transform, "pose_boiler_torso", mesh["BoilerTorso"], mat["Brass"], V3(0f, 1.05f, 0f), Q(0f, 0f, 0f), V3(torsoWidth, torsoHeight, torsoWidth * 0.72f));
            Part(root.transform, "pose_furnace_core", mesh["Lens"], mat[variant == 3 ? "WhiteHot" : "AmberGlass"], V3(0f, 1.08f, -0.36f * torsoWidth), Q(0f, 0f, 0f), V3(0.44f, 0.44f, 0.50f));
            Part(root.transform, "pose_head_lens", mesh["GearRing"], mat["Iron"], V3(0f, 1.78f, -0.04f), Q(0f, 0f, 0f), V3(0.48f, 0.48f, 0.48f));
            Part(root.transform, "pose_eye_glass", mesh["Lens"], mat[variant == 1 ? "TealGlass" : "AmberGlass"], V3(0f, 1.78f, -0.12f), Q(0f, 0f, 0f), V3(0.30f, 0.30f, 0.38f));

            if (variant == 0)
            {
                Part(root.transform, "left_scrapper_saw", mesh["SawBlade"], mat["Hazard"], V3(-0.74f, 0.88f, -0.22f), Q(0f, 90f, -20f), V3(0.76f, 0.76f, 0.76f));
                Part(root.transform, "right_hook_claw", mesh["ClawFinger"], mat["FreshCut"], V3(0.72f, 0.84f, -0.25f), Q(0f, 0f, -40f), V3(1.08f, 1.08f, 1.08f));
                PoseLegs(root.transform, mesh, mat, 0.34f, 0.44f);
            }
            else if (variant == 1)
            {
                Part(root.transform, "forward_lancer_rail", mesh["CutterArm"], mat["Gunmetal"], V3(0f, 1.10f, -0.82f), Q(90f, 0f, 0f), V3(0.72f, 0.72f, 1.40f));
                Part(root.transform, "lance_hot_tip", mesh["FurnaceCore"], mat["FreshCut"], V3(0f, 1.10f, -1.50f), Q(90f, 0f, 0f), V3(0.28f, 0.50f, 0.28f));
                PoseLegs(root.transform, mesh, mat, 0.26f, 0.60f);
            }
            else if (variant == 2)
            {
                Part(root.transform, "bulwark_chest_apron", mesh["ArmorPlate"], mat["AshArmor"], V3(0f, 1.02f, -0.48f), Q(0f, 0f, 0f), V3(1.22f, 1.05f, 1f));
                Part(root.transform, "raised_hammer_head", mesh["HammerHead"], mat["Iron"], V3(0.88f, 1.42f, -0.15f), Q(0f, 0f, -18f), V3(1f, 1f, 1f));
                PoseLegs(root.transform, mesh, mat, 0.50f, 0.50f);
            }
            else
            {
                Part(root.transform, "warden_back_flywheel", mesh["Flywheel"], mat["Brass"], V3(0f, 1.18f, 0.42f), Q(0f, 0f, 0f), V3(1.08f, 1.08f, 1.08f));
                Part(root.transform, "command_shoulder_left", mesh["GearRing"], mat["Copper"], V3(-0.62f, 1.48f, -0.03f), Q(0f, 90f, 0f), V3(0.62f, 0.62f, 0.62f));
                Part(root.transform, "command_shoulder_right", mesh["GearRing"], mat["Copper"], V3(0.62f, 1.48f, -0.03f), Q(0f, 90f, 0f), V3(0.62f, 0.62f, 0.62f));
                PoseLegs(root.transform, mesh, mat, 0.42f, 0.52f);
            }

            Socket(root.transform, "SOCK_RigFuture_Root", V3(0f, 0f, 0f));
            Socket(root.transform, "SOCK_VFX_Furnace", V3(0f, 1.08f, -0.55f));
            Socket(root.transform, "SOCK_VFX_SteamBack", V3(0f, 1.55f, 0.55f));
            return root;
        }

        private static void PoseLegs(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, float x, float spread)
        {
            Part(parent, "left_pose_tripod_leg", mesh["TripodStrut"], mat["Iron"], V3(-x, 0.52f, 0.04f), Q(0f, 0f, -18f), V3(1.0f, 0.88f, 1.0f));
            Part(parent, "right_pose_tripod_leg", mesh["TripodStrut"], mat["Iron"], V3(x, 0.52f, 0.04f), Q(0f, 0f, 18f), V3(1.0f, 0.88f, 1.0f));
            Part(parent, "rear_pose_tripod_leg", mesh["TripodStrut"], mat["Gunmetal"], V3(0f, 0.48f, spread), Q(34f, 0f, 0f), V3(1.0f, 0.80f, 1.0f));
            Part(parent, "left_pose_foot", mesh["ArmorPlate"], mat["AshArmor"], V3(-x - 0.18f, 0.06f, -0.08f), Q(0f, 0f, 0f), V3(0.42f, 0.24f, 0.56f));
            Part(parent, "right_pose_foot", mesh["ArmorPlate"], mat["AshArmor"], V3(x + 0.18f, 0.06f, -0.08f), Q(0f, 0f, 0f), V3(0.42f, 0.24f, 0.56f));
        }

        private static GameObject Root(string name, string category)
        {
            GameObject root = new GameObject(name);
            GameObject contract = new GameObject("CONTRACT_visual_only_no_colliders_no_rigging_" + category);
            contract.transform.SetParent(root.transform, false);
            return root;
        }

        private static GameObject Part(Transform parent, string name, Mesh mesh, Material material, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = position;
            go.transform.localRotation = rotation;
            go.transform.localScale = scale;
            MeshFilter filter = go.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.On;
            renderer.receiveShadows = true;
            return go;
        }

        private static void Socket(Transform parent, string name, Vector3 position)
        {
            GameObject socket = new GameObject(name);
            socket.transform.SetParent(parent, false);
            socket.transform.localPosition = position;
        }

        private static void AddRivetsOnFront(Transform parent, Dictionary<string, Mesh> mesh, Dictionary<string, Material> mat, string prefix, int count, float radius, float y, float z)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = (i / (float)count) * Mathf.PI * 2f;
                Vector3 pos = V3(Mathf.Cos(angle) * radius, y + Mathf.Sin(angle) * radius * 0.62f, z);
                Part(parent, prefix + "_" + i, mesh["Cylinder16"], mat["Rivet"], pos, Q(90f, 0f, 0f), V3(0.070f, 0.035f, 0.070f));
            }
        }

        private static Shader FindLitShader()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Legacy Shaders/Diffuse");
            return shader;
        }

        private static void ConfigureTransparency(Material material, bool transparent)
        {
            if (transparent)
            {
                material.SetOverrideTag("RenderType", "Transparent");
                SetFloat(material, "_Surface", 1f);
                SetFloat(material, "_Mode", 3f);
                SetFloat(material, "_SrcBlend", (float)BlendMode.SrcAlpha);
                SetFloat(material, "_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
                SetFloat(material, "_ZWrite", 0f);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = (int)RenderQueue.Transparent;
            }
            else
            {
                material.SetOverrideTag("RenderType", "");
                SetFloat(material, "_Surface", 0f);
                SetFloat(material, "_Mode", 0f);
                SetFloat(material, "_SrcBlend", (float)BlendMode.One);
                SetFloat(material, "_DstBlend", (float)BlendMode.Zero);
                SetFloat(material, "_ZWrite", 1f);
                material.DisableKeyword("_ALPHABLEND_ON");
                material.renderQueue = -1;
            }
        }

        private static void SetColor(Material material, string property, Color value)
        {
            if (material.HasProperty(property))
            {
                material.SetColor(property, value);
            }
        }

        private static void SetFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property))
            {
                material.SetFloat(property, value);
            }
        }

        private static void SetTexture(Material material, string property, Texture texture)
        {
            if (material.HasProperty(property))
            {
                material.SetTexture(property, texture);
            }
        }

        private static void RenderAllPrefabPreviews(string outputRoot)
        {
            for (int i = 0; i < PrefabNames.Length; i++)
            {
                RenderSingle(outputRoot, PrefabNames[i], 1200, 1200);
            }

            RenderContactSheet(outputRoot, "CEPS09_v0.1.54_all_parts_contact_sheet.png", PrefabNames.Take(29).ToArray(), 6, 2200, 1600);
            RenderContactSheet(outputRoot, "CEPS09_v0.1.54_archetype_previews_contact_sheet.png", PrefabNames.Skip(29).ToArray(), 3, 1800, 900);
        }

        private static void RenderSingle(string outputRoot, string prefabName, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SetupPreviewWorld();
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + prefabName + ".prefab");
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Camera camera = PreviewCamera(31f);
            Frame(camera, BoundsFor(new[] { instance }), 1.34f, width / (float)height);
            RenderCamera(camera, Path.Combine(outputRoot, prefabName + "_preview.png"), width, height);
        }

        private static void RenderContactSheet(string outputRoot, string fileName, string[] prefabs, int columns, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SetupPreviewWorld();
            List<GameObject> roots = new List<GameObject>();
            for (int i = 0; i < prefabs.Length; i++)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabRoot + "/" + prefabs[i] + ".prefab");
                if (prefab == null) continue;
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.transform.position = new Vector3((i % columns - (columns - 1) * 0.5f) * 2.4f, 0f, (i / columns) * 2.3f);
                roots.Add(instance);
            }

            Camera camera = PreviewCamera(34f);
            Frame(camera, BoundsFor(roots), 1.24f, width / (float)height);
            RenderCamera(camera, Path.Combine(outputRoot, fileName), width, height);
        }

        private static void SetupPreviewWorld()
        {
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = C(0.22f, 0.19f, 0.16f);

            Material floor = new Material(FindLitShader());
            SetColor(floor, "_Color", C(0.070f, 0.062f, 0.052f));
            SetColor(floor, "_BaseColor", C(0.070f, 0.062f, 0.052f));
            GameObject floorObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.DestroyImmediate(floorObject.GetComponent<Collider>());
            floorObject.name = "preview_foundry_floor";
            floorObject.transform.position = V3(0f, -0.055f, 2.0f);
            floorObject.transform.localScale = V3(24f, 0.10f, 20f);
            floorObject.GetComponent<MeshRenderer>().sharedMaterial = floor;

            Light key = new GameObject("preview_key_amber_light").AddComponent<Light>();
            key.type = LightType.Spot;
            key.color = C(1f, 0.66f, 0.34f);
            key.intensity = 3.8f;
            key.range = 16f;
            key.spotAngle = 58f;
            key.transform.position = V3(-4.5f, 5.8f, -4.6f);
            key.transform.rotation = Quaternion.Euler(58f, 34f, 0f);

            Light fill = new GameObject("preview_cool_fill_light").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.color = C(0.10f, 0.55f, 0.68f);
            fill.intensity = 1.4f;
            fill.range = 7f;
            fill.transform.position = V3(3.8f, 2.4f, -2.6f);
        }

        private static Camera PreviewCamera(float fov)
        {
            Camera camera = new GameObject("preview_camera").AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = C(0.032f, 0.028f, 0.023f);
            camera.fieldOfView = fov;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 100f;
            return camera;
        }

        private static Bounds BoundsFor(IEnumerable<GameObject> roots)
        {
            Renderer[] renderers = roots.Where(r => r != null).SelectMany(r => r.GetComponentsInChildren<Renderer>()).ToArray();
            if (renderers.Length == 0)
            {
                return new Bounds(Vector3.up, Vector3.one);
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private static void Frame(Camera camera, Bounds bounds, float padding, float aspect)
        {
            float vertical = Mathf.Max(0.45f, bounds.extents.y);
            float horizontal = Mathf.Max(0.45f, bounds.extents.x / Mathf.Max(0.1f, aspect));
            float halfSize = Mathf.Max(vertical, horizontal) * padding;
            float distance = halfSize / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            camera.transform.position = bounds.center + V3(0f, bounds.extents.y * 0.15f, -distance - bounds.extents.z - 0.65f);
            camera.transform.LookAt(bounds.center + V3(0f, bounds.extents.y * 0.10f, 0f));
        }

        private static void RenderCamera(Camera camera, string absolutePath, int width, int height)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
            RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            RenderTexture previous = RenderTexture.active;
            camera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            camera.Render();
            Texture2D image = new Texture2D(width, height, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
            image.Apply();
            File.WriteAllBytes(absolutePath, ImageConversion.EncodeToPNG(image));
            camera.targetTexture = null;
            RenderTexture.active = previous;
            UnityEngine.Object.DestroyImmediate(image);
            UnityEngine.Object.DestroyImmediate(renderTexture);
        }

        private static void WriteMaterialSwatches(string outputRoot)
        {
            string swatchRoot = Path.Combine(outputRoot, "MaterialSwatches");
            Directory.CreateDirectory(swatchRoot);
            int size = 160;
            for (int i = 0; i < Materials.Length; i++)
            {
                Texture2D texture = BuildTexture(Materials[i], size, size);
                File.WriteAllBytes(Path.Combine(swatchRoot, Materials[i].AssetName + "_swatch.png"), ImageConversion.EncodeToPNG(texture));
                UnityEngine.Object.DestroyImmediate(texture);
            }

            int columns = 6;
            int rows = Mathf.CeilToInt(Materials.Length / (float)columns);
            Texture2D sheet = new Texture2D(size * columns, size * rows, TextureFormat.RGBA32, false);
            Fill(sheet, C(0.035f, 0.030f, 0.026f));
            for (int i = 0; i < Materials.Length; i++)
            {
                Texture2D swatch = BuildTexture(Materials[i], size - 12, size - 12);
                int x0 = (i % columns) * size + 6;
                int y0 = sheet.height - ((i / columns) + 1) * size + 6;
                CopyInto(sheet, swatch, x0, y0);
                UnityEngine.Object.DestroyImmediate(swatch);
            }

            sheet.Apply();
            File.WriteAllBytes(Path.Combine(outputRoot, "CEPS09_v0.1.54_material_swatch_sheet.png"), ImageConversion.EncodeToPNG(sheet));
            UnityEngine.Object.DestroyImmediate(sheet);
        }

        private static void WriteRenderIndex(string outputRoot)
        {
            string index = "# Clockwork Enemy Parts Set 09 Render Index\n\n" +
                "Generated preview renders and swatches for visual review. Runtime textures remain inside the package under `Runtime/Textures`; these PNGs are documentation evidence only.\n\n" +
                "- Individual prefab previews: " + PrefabNames.Length.ToString(CultureInfo.InvariantCulture) + "\n" +
                "- Material swatches: " + Materials.Length.ToString(CultureInfo.InvariantCulture) + "\n" +
                "- Contact sheets: 3\n";
            File.WriteAllText(Path.Combine(outputRoot, "README_RENDER_INDEX.md"), index);
        }

        private static int WriteUnityValidationReport()
        {
            AssetDatabase.Refresh();
            string renderRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            string reportRoot = ResolveRepoRelativeFolder(ProductionDocFolder);
            Directory.CreateDirectory(reportRoot);
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot });
            string[] materialGuids = AssetDatabase.FindAssets("t:Material", new[] { MaterialRoot });
            string[] meshGuids = AssetDatabase.FindAssets("t:Mesh", new[] { MeshRoot });
            int runtimeTexturePngs = Directory.GetFiles(Path.Combine(PackagePhysicalRoot(), "Runtime", "Textures"), "*.png", SearchOption.TopDirectoryOnly).Length;
            int previewPngs = Directory.Exists(renderRoot) ? Directory.GetFiles(renderRoot, "*.png", SearchOption.AllDirectories).Length : 0;
            int colliderCount = 0;
            int animatorCount = 0;
            int rigidbodyCount = 0;
            int monoBehaviourCount = 0;
            for (int i = 0; i < prefabGuids.Length; i++)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(prefabGuids[i]));
                colliderCount += prefab.GetComponentsInChildren<Collider>(true).Length;
                animatorCount += prefab.GetComponentsInChildren<Animator>(true).Length;
                rigidbodyCount += prefab.GetComponentsInChildren<Rigidbody>(true).Length;
                monoBehaviourCount += prefab.GetComponentsInChildren<MonoBehaviour>(true).Length;
            }

            int errors = 0;
            if (prefabGuids.Length != ExpectedPrefabs) errors++;
            if (materialGuids.Length != ExpectedMaterials) errors++;
            if (meshGuids.Length != ExpectedMeshes) errors++;
            if (runtimeTexturePngs != ExpectedTexturePngs) errors++;
            if (previewPngs < ExpectedPreviewPngMinimum) errors++;
            if (colliderCount != 0 || animatorCount != 0 || rigidbodyCount != 0 || monoBehaviourCount != 0) errors++;

            string json = "{\n" +
                "  \"status\": \"" + (errors == 0 ? "pass" : "fail") + "\",\n" +
                "  \"prefabs\": " + prefabGuids.Length + ",\n" +
                "  \"materials\": " + materialGuids.Length + ",\n" +
                "  \"meshes\": " + meshGuids.Length + ",\n" +
                "  \"runtime_texture_pngs\": " + runtimeTexturePngs + ",\n" +
                "  \"preview_pngs\": " + previewPngs + ",\n" +
                "  \"colliders_in_prefabs\": " + colliderCount + ",\n" +
                "  \"animators_in_prefabs\": " + animatorCount + ",\n" +
                "  \"rigidbodies_in_prefabs\": " + rigidbodyCount + ",\n" +
                "  \"monobehaviours_in_prefabs\": " + monoBehaviourCount + ",\n" +
                "  \"expected_prefabs\": " + ExpectedPrefabs + ",\n" +
                "  \"expected_materials\": " + ExpectedMaterials + ",\n" +
                "  \"expected_meshes\": " + ExpectedMeshes + ",\n" +
                "  \"expected_runtime_texture_pngs\": " + ExpectedTexturePngs + ",\n" +
                "  \"expected_preview_pngs_minimum\": " + ExpectedPreviewPngMinimum + "\n" +
                "}\n";
            File.WriteAllText(Path.Combine(reportRoot, "unity_validation_report_v0.1.54.json"), json);
            Debug.Log("CEPS09_UNITY_VALIDATION_" + (errors == 0 ? "PASS" : "FAIL"));
            return errors;
        }

        private static void WriteCatalog()
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            Line(json, "catalog_id", "CEPS09_ClockworkEnemyPartsCatalog", 1, true);
            Line(json, "pack_id", PackId, 1, true);
            Line(json, "version", Version + "-" + BuildId, 1, true);
            Line(json, "purpose", "Rigging-ready visual organization catalog for three steampunk mechanical enemy archetype part families.", 1, true);
            ArrayLine(json, "material_contract", Materials.Select(m => m.AssetName).ToArray(), 1, true);
            ArrayLine(json, "required_surface_coverage", new[]
            {
                "aged brass boiler shells",
                "aged copper tanks and pipes",
                "blackened riveted iron torsos",
                "amber glowing optic glass",
                "oily rubber hose loops",
                "soot and damage accents"
            }, 1, true);
            json.AppendLine("  \"archetype_sets\": [");
            WriteArchetypeCatalog(json, "small_skitter_unit", "low fast silhouette", PrefabNames.Take(8).ToArray(), new[]
            {
                "SOCK_RigFuture_Root",
                "SOCK_HeadFront",
                "SOCK_Leg_1 through SOCK_Leg_4",
                "SOCK_WeaponUnder",
                "SOCK_BackTank",
                "SOCK_SteamLine"
            }, true);
            WriteArchetypeCatalog(json, "humanoid_boiler_brute", "large readable humanoid silhouette", PrefabNames.Skip(8).Take(9).ToArray(), new[]
            {
                "SOCK_RigFuture_Hips",
                "SOCK_Head",
                "SOCK_LeftShoulder",
                "SOCK_RightShoulder",
                "SOCK_Wrist",
                "SOCK_AnkleMount",
                "SOCK_BackTanks"
            }, true);
            WriteArchetypeCatalog(json, "wall_ceiling_sentry", "mounted wall or ceiling turret silhouette", PrefabNames.Skip(17).Take(8).ToArray(), new[]
            {
                "SOCK_CeilingAnchor",
                "SOCK_RotaryHead",
                "SOCK_LeftWeapon",
                "SOCK_RightWeapon",
                "SOCK_WallOrCeilingMount",
                "SOCK_TurretMount",
                "SOCK_SteamVent"
            }, false);
            json.AppendLine("  ],");
            ArrayLine(json, "shared_parts", PrefabNames.Skip(25).Take(4).ToArray(), 1, true);
            ArrayLine(json, "archetype_previews", PrefabNames.Skip(29).ToArray(), 1, true);
            ArrayLine(json, "deferred_dependencies", new[]
            {
                "final skeleton hierarchy and bind pose per archetype",
                "skin weights or rigid parent constraints",
                "locomotion, attack, idle, recoil, and destruction animation clips",
                "hit volume and weak-point ownership in main gameplay assemblies",
                "LOD group authoring and final occlusion/collider pass"
            }, 1, true);
            Line(json, "decision", "ready_for_quarantine_visual_intake_not_gameplay_promotion", 1, false);
            json.AppendLine("}");

            string catalogPath = Path.Combine(PackagePhysicalRoot(), "Runtime", "Metadata", "CEPS09_ClockworkEnemyPartsCatalog_v0.1.54-p001.json");
            Directory.CreateDirectory(Path.GetDirectoryName(catalogPath));
            File.WriteAllText(catalogPath, json.ToString());
            AssetDatabase.ImportAsset(MetadataRoot + "/CEPS09_ClockworkEnemyPartsCatalog_v0.1.54-p001.json");
        }

        private static void WriteArchetypeCatalog(StringBuilder json, string id, string silhouetteRole, string[] prefabs, string[] sockets, bool comma)
        {
            json.AppendLine("    {");
            Line(json, "id", id, 3, true);
            Line(json, "silhouette_role", silhouetteRole, 3, true);
            ArrayLine(json, "prefabs", prefabs, 3, true);
            ArrayLine(json, "rigging_socket_expectations", sockets, 3, true);
            ArrayLine(json, "integration_notes", new[]
            {
                "visual-only prefab modules",
                "no colliders, rigidbodies, MonoBehaviours, or animation controllers",
                "socket names are stable handoff anchors for future rigging"
            }, 3, false);
            json.Append("    }");
            if (comma) json.Append(",");
            json.AppendLine();
        }

        private static void WriteManifest()
        {
            AssetDatabase.Refresh();
            string packagePath = PackagePhysicalRoot();
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabRoot }).Select(g => AssetDatabase.GUIDToAssetPath(g)).OrderBy(p => p, StringComparer.Ordinal).ToArray();
            string[] materialPaths = AssetDatabase.FindAssets("t:Material", new[] { MaterialRoot }).Select(g => AssetDatabase.GUIDToAssetPath(g)).OrderBy(p => p, StringComparer.Ordinal).ToArray();
            string[] meshPaths = AssetDatabase.FindAssets("t:Mesh", new[] { MeshRoot }).Select(g => AssetDatabase.GUIDToAssetPath(g)).OrderBy(p => p, StringComparer.Ordinal).ToArray();
            string[] texturePaths = Directory.GetFiles(Path.Combine(packagePath, "Runtime", "Textures"), "*.png", SearchOption.TopDirectoryOnly)
                .Select(p => (TextureRoot + "/" + Path.GetFileName(p)).Replace("\\", "/")).OrderBy(p => p, StringComparer.Ordinal).ToArray();
            string[] metadataPaths = Directory.GetFiles(Path.Combine(packagePath, "Runtime", "Metadata"), "*.json", SearchOption.TopDirectoryOnly)
                .Select(p => (MetadataRoot + "/" + Path.GetFileName(p)).Replace("\\", "/")).OrderBy(p => p, StringComparer.Ordinal).ToArray();
            string renderRoot = ResolveRepoRelativeFolder(RenderDocFolder);
            string[] previewPaths = Directory.Exists(renderRoot)
                ? Directory.GetFiles(renderRoot, "*.png", SearchOption.AllDirectories).Select(ToRepoRelative).OrderBy(p => p, StringComparer.Ordinal).ToArray()
                : Array.Empty<string>();

            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            Line(json, "pack_id", PackId, 1, true);
            Line(json, "display_name", "Clockwork Enemy Parts Set 09", 1, true);
            Line(json, "version", Version, 1, true);
            Line(json, "build_id", BuildId, 1, true);
            Line(json, "unity_version", "6000.4.6f1", 1, true);
            Line(json, "generated_at_utc", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), 1, true);
            Line(json, "sidecar_project", "UD-SC-ENEMY-ClockworkEnemyPartsSet09", 1, true);
            Line(json, "owner_lane", "sidecar-clockwork-enemy-parts-set09", 1, true);
            Line(json, "primary_intake_owner", "main-lane-art-integration", 1, true);
            Line(json, "canonical_root", "AssetPacks/BrassworksBreach.ClockworkEnemyPartsSet09", 1, true);
            Line(json, "package_root", "AssetPacks/BrassworksBreach.ClockworkEnemyPartsSet09", 1, true);
            Line(json, "package_name", PackageName, 1, true);
            Line(json, "common_schema", "brassworks.sidecar.visual_pack_manifest.v1", 1, true);
            json.AppendLine("  \"asset_counts\": {");
            NumLine(json, "generated_prefabs", prefabPaths.Length, 2, true);
            NumLine(json, "generated_materials", materialPaths.Length, 2, true);
            NumLine(json, "generated_meshes", meshPaths.Length, 2, true);
            NumLine(json, "runtime_texture_pngs", texturePaths.Length, 2, true);
            NumLine(json, "metadata_catalogs", metadataPaths.Length, 2, true);
            NumLine(json, "preview_pngs_current", previewPaths.Length, 2, true);
            NumLine(json, "runtime_scripts", 0, 2, true);
            NumLine(json, "colliders", 0, 2, true);
            NumLine(json, "animation_clips", 0, 2, true);
            NumLine(json, "animator_controllers", 0, 2, false);
            json.AppendLine("  },");
            ArrayLine(json, "generated_prefabs", prefabPaths, 1, true);
            ArrayLine(json, "generated_materials", materialPaths, 1, true);
            ArrayLine(json, "generated_meshes", meshPaths, 1, true);
            ArrayLine(json, "generated_texture_pngs", texturePaths, 1, true);
            ArrayLine(json, "generated_metadata_catalogs", metadataPaths, 1, true);
            ArrayLine(json, "preview_renders", previewPaths, 1, true);
            ArrayLine(json, "visual_contract", new[]
            {
                "visual_only_no_gameplay_authority",
                "modular_parts_and_pose_previews_only",
                "no_runtime_scripts",
                "no_colliders_or_rigidbodies",
                "no_animator_controllers_or_animation_clips",
                "no_rigging_or_skinned_meshes",
                "no_ai_damage_or_navmesh_authority"
            }, 1, true);
            ArrayLine(json, "part_families", new[]
            {
                "small skitter unit torso/head/limb/weapon/attachment parts",
                "humanoid boiler brute torso/head/arm/foot/weapon/attachment parts",
                "wall and ceiling sentry mount/head/strut/weapon/attachment parts",
                "shared optics, governor gears, gauges, and damage plates",
                "archetype preview assemblies"
            }, 1, true);
            ArrayLine(json, "intended_import_notes", new[]
            {
                "Import as a local package or copy into a quarantine art staging branch before main-lane placement.",
                "Use prefabs as visual modules and silhouette references; final sockets, rigging, hit volumes, damage windows, and animation ownership remain outside this package.",
                "Preview PNGs are documentation evidence and should not be imported as runtime textures.",
                "Generated Runtime/Textures PNGs are material base maps and can be replaced by final authored PBR maps later."
            }, 1, true);
            ArrayLine(json, "validation_checklist", new[]
            {
                "32 visual-only prefabs present.",
                "22 material assets present.",
                "16 reusable mesh assets present.",
                "22 generated runtime texture PNGs present.",
                "Runtime metadata catalog present.",
                "At least 30 preview/swatch PNGs present in Documentation/ConceptRenders/V0_1_54_ClockworkEnemyPartsSet09.",
                "No colliders, rigidbodies, animators, MonoBehaviours, animation clips, scenes, audio, or Blender/DCC files in runtime output."
            }, 1, true);
            ArrayLine(json, "known_risks", new[]
            {
                "Meshes are procedural Unity lookdev geometry and need final skinning/LOD ownership before gameplay promotion.",
                "Socket transforms are named for future rigging but no skeleton, skin weights, IK, or animation clips are included.",
                "Transparent glass response may need per-render-pipeline tuning during primary import.",
                "Archetype previews intentionally do not define gameplay scale, collision, animation, hit volumes, or AI authority."
            }, 1, true);
            Line(json, "import_smoke_status", "unity_batch_generation_pending_or_passed_static_validation", 1, true);
            Line(json, "rollback_path", "remove local package reference com.brassworks.sidecar.clockwork-enemy-parts-set09 and delete isolated package root", 1, true);
            Line(json, "decision", "ready_for_primary_quarantine_after_static_validation_and_preview_review", 1, false);
            json.AppendLine("}");

            string manifestPath = Path.Combine(packagePath, "Documentation~", "Manifest", "CEPS09_ClockworkEnemyPartsSet09_Manifest_v0.1.54-p001.json");
            Directory.CreateDirectory(Path.GetDirectoryName(manifestPath));
            File.WriteAllText(manifestPath, json.ToString());
            AssetDatabase.ImportAsset(ManifestRoot + "/CEPS09_ClockworkEnemyPartsSet09_Manifest_v0.1.54-p001.json");
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

                triangles.Add(start);
                triangles.Add(start + 1);
                triangles.Add(start + 2);
                triangles.Add(start);
                triangles.Add(start + 2);
                triangles.Add(start + 3);
            }

            return MeshFrom(name, vertices, triangles, normalList);
        }

        private static Mesh BeveledBox(string name, Vector3 size, float bevel)
        {
            Mesh mesh = Box(name, size);
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 n = vertices[i].normalized;
                vertices[i] -= n * Mathf.Min(bevel, 0.12f);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh Cylinder(string name, float radiusX, float radiusZ, float height, int segments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float y0 = -height * 0.5f;
            float y1 = height * 0.5f;
            for (int i = 0; i < segments; i++)
            {
                float a0 = i / (float)segments * Mathf.PI * 2f;
                float a1 = (i + 1) / (float)segments * Mathf.PI * 2f;
                Vector3 p0 = V3(Mathf.Cos(a0) * radiusX, y0, Mathf.Sin(a0) * radiusZ);
                Vector3 p1 = V3(Mathf.Cos(a1) * radiusX, y0, Mathf.Sin(a1) * radiusZ);
                Vector3 p2 = V3(Mathf.Cos(a1) * radiusX, y1, Mathf.Sin(a1) * radiusZ);
                Vector3 p3 = V3(Mathf.Cos(a0) * radiusX, y1, Mathf.Sin(a0) * radiusZ);
                int s = vertices.Count;
                vertices.AddRange(new[] { p0, p1, p2, p3 });
                triangles.AddRange(new[] { s, s + 1, s + 2, s, s + 2, s + 3 });
                int bottom = vertices.Count;
                vertices.AddRange(new[] { Vector3.up * y0, p1, p0 });
                triangles.AddRange(new[] { bottom, bottom + 1, bottom + 2 });
                int top = vertices.Count;
                vertices.AddRange(new[] { Vector3.up * y1, p3, p2 });
                triangles.AddRange(new[] { top, top + 1, top + 2 });
            }

            return MeshFrom(name, vertices, triangles, null);
        }

        private static Mesh TaperedCylinder(string name, float bottomRadius, float topRadius, float height, int segments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float y0 = -height * 0.5f;
            float y1 = height * 0.5f;
            for (int i = 0; i < segments; i++)
            {
                float a0 = i / (float)segments * Mathf.PI * 2f;
                float a1 = (i + 1) / (float)segments * Mathf.PI * 2f;
                Vector3 p0 = V3(Mathf.Cos(a0) * bottomRadius, y0, Mathf.Sin(a0) * bottomRadius);
                Vector3 p1 = V3(Mathf.Cos(a1) * bottomRadius, y0, Mathf.Sin(a1) * bottomRadius);
                Vector3 p2 = V3(Mathf.Cos(a1) * topRadius, y1, Mathf.Sin(a1) * topRadius);
                Vector3 p3 = V3(Mathf.Cos(a0) * topRadius, y1, Mathf.Sin(a0) * topRadius);
                int s = vertices.Count;
                vertices.AddRange(new[] { p0, p1, p2, p3 });
                triangles.AddRange(new[] { s, s + 1, s + 2, s, s + 2, s + 3 });
            }

            return MeshFrom(name, vertices, triangles, null);
        }

        private static Mesh GearRing(string name, int teeth, float innerRadius, float outerRadius, float thickness)
        {
            int segments = teeth * 2;
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float zFront = -thickness * 0.5f;
            float zBack = thickness * 0.5f;
            for (int i = 0; i < segments; i++)
            {
                float a0 = i / (float)segments * Mathf.PI * 2f;
                float a1 = (i + 1) / (float)segments * Mathf.PI * 2f;
                float r0 = outerRadius * (i % 2 == 0 ? 1f : 0.84f);
                float r1 = outerRadius * ((i + 1) % 2 == 0 ? 1f : 0.84f);
                Vector3 o0f = V3(Mathf.Cos(a0) * r0, Mathf.Sin(a0) * r0, zFront);
                Vector3 o1f = V3(Mathf.Cos(a1) * r1, Mathf.Sin(a1) * r1, zFront);
                Vector3 i0f = V3(Mathf.Cos(a0) * innerRadius, Mathf.Sin(a0) * innerRadius, zFront);
                Vector3 i1f = V3(Mathf.Cos(a1) * innerRadius, Mathf.Sin(a1) * innerRadius, zFront);
                Vector3 o0b = V3(o0f.x, o0f.y, zBack);
                Vector3 o1b = V3(o1f.x, o1f.y, zBack);
                Vector3 i0b = V3(i0f.x, i0f.y, zBack);
                Vector3 i1b = V3(i1f.x, i1f.y, zBack);
                int s = vertices.Count;
                vertices.AddRange(new[] { o0f, o1f, i1f, i0f, o0b, i0b, i1b, o1b, o0f, o0b, o1b, o1f, i0f, i1f, i1b, i0b });
                triangles.AddRange(new[] { s, s + 1, s + 2, s, s + 2, s + 3 });
                triangles.AddRange(new[] { s + 4, s + 5, s + 6, s + 4, s + 6, s + 7 });
                triangles.AddRange(new[] { s + 8, s + 9, s + 10, s + 8, s + 10, s + 11 });
                triangles.AddRange(new[] { s + 12, s + 13, s + 14, s + 12, s + 14, s + 15 });
            }

            return MeshFrom(name, vertices, triangles, null);
        }

        private static Mesh ArmorPlate(string name, float width, float height, float thickness)
        {
            float w = width * 0.5f;
            float h = height * 0.5f;
            float c = Mathf.Min(width, height) * 0.12f;
            Vector2[] outline =
            {
                new Vector2(-w + c, -h), new Vector2(w - c, -h), new Vector2(w, -h + c), new Vector2(w, h - c),
                new Vector2(w - c, h), new Vector2(-w + c, h), new Vector2(-w, h - c), new Vector2(-w, -h + c)
            };
            return ExtrudedPolygon(name, outline, thickness);
        }

        private static Mesh Prism(string name, int sides, float radius, float height)
        {
            Vector2[] outline = new Vector2[sides];
            for (int i = 0; i < sides; i++)
            {
                float a = (i / (float)sides) * Mathf.PI * 2f + Mathf.PI / 6f;
                outline[i] = new Vector2(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius);
            }

            Mesh mesh = ExtrudedPolygon(name, outline, height);
            mesh.vertices = mesh.vertices.Select(v => V3(v.x, v.z, v.y)).ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh ExtrudedPolygon(string name, Vector2[] outline, float thickness)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float z0 = -thickness * 0.5f;
            float z1 = thickness * 0.5f;
            for (int i = 0; i < outline.Length; i++)
            {
                vertices.Add(V3(outline[i].x, outline[i].y, z0));
            }
            for (int i = 0; i < outline.Length; i++)
            {
                vertices.Add(V3(outline[i].x, outline[i].y, z1));
            }

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

        private static Mesh HoseArc(string name, float radius, float tubeRadius, float arcDegrees, int arcSegments, int tubeSegments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float arc = arcDegrees * Mathf.Deg2Rad;
            for (int i = 0; i <= arcSegments; i++)
            {
                float t = i / (float)arcSegments;
                float a = Mathf.Lerp(Mathf.PI, Mathf.PI - arc, t);
                Vector3 center = V3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, 0f);
                Vector3 radial = V3(Mathf.Cos(a), Mathf.Sin(a), 0f);
                Vector3 binormal = Vector3.forward;
                for (int j = 0; j < tubeSegments; j++)
                {
                    float b = j / (float)tubeSegments * Mathf.PI * 2f;
                    vertices.Add(center + radial * Mathf.Cos(b) * tubeRadius + binormal * Mathf.Sin(b) * tubeRadius);
                }
            }

            for (int i = 0; i < arcSegments; i++)
            {
                for (int j = 0; j < tubeSegments; j++)
                {
                    int a = i * tubeSegments + j;
                    int b = i * tubeSegments + (j + 1) % tubeSegments;
                    int c = (i + 1) * tubeSegments + (j + 1) % tubeSegments;
                    int d = (i + 1) * tubeSegments + j;
                    triangles.AddRange(new[] { a, b, c, a, c, d });
                }
            }

            return MeshFrom(name, vertices, triangles, null);
        }

        private static Mesh LensCap(string name, float radius, float bulge, int segments, int rings)
        {
            List<Vector3> vertices = new List<Vector3> { V3(0f, 0f, -bulge) };
            List<int> triangles = new List<int>();
            for (int r = 1; r <= rings; r++)
            {
                float t = r / (float)rings;
                float rr = radius * t;
                float z = -bulge * (1f - t * t);
                for (int i = 0; i < segments; i++)
                {
                    float a = i / (float)segments * Mathf.PI * 2f;
                    vertices.Add(V3(Mathf.Cos(a) * rr, Mathf.Sin(a) * rr, z));
                }
            }

            for (int i = 0; i < segments; i++)
            {
                triangles.AddRange(new[] { 0, 1 + i, 1 + (i + 1) % segments });
            }

            for (int r = 1; r < rings; r++)
            {
                int base0 = 1 + (r - 1) * segments;
                int base1 = 1 + r * segments;
                for (int i = 0; i < segments; i++)
                {
                    int a = base0 + i;
                    int b = base0 + (i + 1) % segments;
                    int c = base1 + (i + 1) % segments;
                    int d = base1 + i;
                    triangles.AddRange(new[] { a, b, c, a, c, d });
                }
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

        private static string ResolveRepoRelativeFolder(string relativePath)
        {
            string cursor = PackagePhysicalRoot();
            while (!string.IsNullOrEmpty(cursor))
            {
                if (Directory.Exists(Path.Combine(cursor, "AssetPacks")) && Directory.Exists(Path.Combine(cursor, "Documentation")))
                {
                    return Path.Combine(cursor, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                }

                cursor = Directory.GetParent(cursor)?.FullName;
            }

            return Path.Combine(Directory.GetParent(PackagePhysicalRoot()).FullName, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        }

        private static string ToRepoRelative(string absolutePath)
        {
            string repo = ResolveRepoRelativeFolder("").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string full = Path.GetFullPath(absolutePath);
            if (full.StartsWith(repo, StringComparison.OrdinalIgnoreCase))
            {
                return full.Substring(repo.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
            }

            return full.Replace("\\", "/");
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

        private static void CopyInto(Texture2D destination, Texture2D source, int x0, int y0)
        {
            for (int y = 0; y < source.height; y++)
            {
                for (int x = 0; x < source.width; x++)
                {
                    destination.SetPixel(x0 + x, y0 + y, source.GetPixel(x, y));
                }
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

        private static Quaternion Q(float x, float y, float z)
        {
            return Quaternion.Euler(x, y, z);
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
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
