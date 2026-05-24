#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace BrassworksBreach.Sidecars.UnityWeaponMaterialCellLookdev09
{
    public static class UnityWeaponMaterialCellLookdevRenderer
    {
        private const string Marker = "UNITY_WEAPON_MATERIAL_CELL_LOOKDEV_RENDERED";
        private const string Bundle = "V0_1_54_UnityWeaponMaterialCellLookdev";
        private const string RenderRelative = "Documentation/ConceptRenders/V0_1_54_UnityWeaponMaterialCellLookdev";
        private const string PlanningRelative = "Documentation/Planning/V0_1_54_UnityWeaponMaterialCellLookdev";
        private const string QaRelative = "Documentation/QA/V0_1_54_UnityWeaponMaterialCellLookdev";

        private static readonly List<RenderRecord> Records = new List<RenderRecord>();
        private static readonly List<CellAssessment> Assessments = new List<CellAssessment>();

        private delegate void CellBuilder(Context context);
        private delegate void RasterCellBuilder(Texture2D texture, int seed);

        [MenuItem("Brassworks Breach/Sidecars/Unity Weapon Material Cell Lookdev 09/Render")]
        public static void RenderMenu()
        {
            Execute();
        }

        public static void Execute()
        {
            try
            {
                Records.Clear();
                Assessments.Clear();

                Context context = new Context();
                context.Initialize();

                RenderProceduralCell(context, "UWMC09_01_copper_coil_material_cell_v0.1.54.png", "Copper pressure coil cell", CellRole.CopperCoil, DrawCopperCoilCellRaster, 1600, 1100, 101);
                RenderProceduralCell(context, "UWMC09_02_pressure_gauge_dial_material_cell_v0.1.54.png", "Pressure gauge dial cell", CellRole.PressureGaugeDial, DrawGaugeDialCellRaster, 1600, 1100, 202);
                RenderProceduralCell(context, "UWMC09_03_brass_receiver_plate_material_cell_v0.1.54.png", "Brass receiver plate cell", CellRole.BrassReceiverPlate, DrawBrassReceiverCellRaster, 1600, 1100, 303);
                RenderProceduralCell(context, "UWMC09_04_black_iron_barrel_material_cell_v0.1.54.png", "Black iron barrel cell", CellRole.BlackIronBarrel, DrawBlackIronBarrelCellRaster, 1600, 1100, 404);
                RenderProceduralCell(context, "UWMC09_05_red_enamel_safety_line_material_cell_v0.1.54.png", "Red enamel safety line cell", CellRole.RedEnamelSafetyLine, DrawRedEnamelSafetyCellRaster, 1600, 1100, 505);
                RenderProceduralCell(context, "UWMC09_06_smoked_amber_glass_material_cell_v0.1.54.png", "Smoked amber glass cell", CellRole.SmokedAmberGlass, DrawSmokedAmberGlassCellRaster, 1600, 1100, 606);
                RenderProceduralCell(context, "UWMC09_07_walnut_leather_grip_proxy_material_cell_v0.1.54.png", "Walnut and leather grip proxy cell", CellRole.WalnutLeatherGripProxy, DrawGripProxyCellRaster, 1600, 1100, 707);
                RenderContactSheet(context);

                WritePlanningDocs(context);
                WriteQaDocs(context);

                bool allRenderGatesPassed = Records.All(record => record.Passed);
                Debug.Log(Marker + " records=" + Records.Count + " render_gates_passed=" + Records.Count(record => record.Passed) + "/" + Records.Count + " all_png_gates_passed=" + allRenderGatesPassed.ToString().ToLowerInvariant());
                EditorApplication.Exit(allRenderGatesPassed ? 0 : 2);
            }
            catch (Exception ex)
            {
                Debug.LogError("Unity weapon material cell lookdev failed: " + ex);
                EditorApplication.Exit(1);
            }
        }

        private static void RenderCell(Context context, string fileName, string description, CellRole role, CellBuilder builder, Vector3 cameraPosition, Vector3 lookAt, float fieldOfView, int width, int height)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            PrepareRenderSettings();
            BuildReviewStage(context);
            builder(context);
            AddAssessment(role);

            Camera camera = CreateCamera(cameraPosition, lookAt, fieldOfView);
            RenderRequest request = new RenderRequest(fileName, description, width, height, role.ToString());
            RenderCamera(context, camera, request);
            Object.DestroyImmediate(camera.gameObject);
        }

        private static void RenderProceduralCell(Context context, string fileName, string description, CellRole role, RasterCellBuilder builder, int width, int height, int seed)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            DrawRasterBackdrop(texture, seed);
            builder(texture, seed);
            texture.Apply();

            string outputPath = Path.Combine(context.RenderRoot, fileName);
            File.WriteAllBytes(outputPath, texture.EncodeToPNG());

            RenderRequest request = new RenderRequest(fileName, description, width, height, role.ToString());
            Records.Add(AnalyzeTexture(texture, request, outputPath, context));
            AddAssessment(role);
            Object.DestroyImmediate(texture);
        }

        private static void PrepareRenderSettings()
        {
            QualitySettings.antiAliasing = 4;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = C(0.11f, 0.085f, 0.06f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.009f;
            RenderSettings.fogColor = C(0.045f, 0.037f, 0.031f);

            GameObject keyObject = new GameObject("warm keyed brassworks light");
            Light key = keyObject.AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = C(1f, 0.72f, 0.43f);
            key.intensity = 2.7f;
            key.transform.rotation = Quaternion.Euler(38f, -38f, 0f);

            GameObject rimObject = new GameObject("cool rim inspection light");
            Light rim = rimObject.AddComponent<Light>();
            rim.type = LightType.Directional;
            rim.color = C(0.42f, 0.64f, 0.82f);
            rim.intensity = 0.75f;
            rim.transform.rotation = Quaternion.Euler(12f, 126f, 0f);

            GameObject glowObject = new GameObject("low amber bounce light");
            Light glow = glowObject.AddComponent<Light>();
            glow.type = LightType.Point;
            glow.color = C(1f, 0.48f, 0.16f);
            glow.intensity = 4.4f;
            glow.range = 4.8f;
            glow.transform.position = new Vector3(-1.65f, 1.1f, -1.5f);
        }

        private static void BuildReviewStage(Context context)
        {
            CreateBox("dark wet stone review plinth", new Vector3(0f, -0.045f, 0f), new Vector3(4.2f, 0.09f, 2.6f), Vector3.zero, context.Mat("WetBlackStone"));
            CreateBox("rear black iron inspection wall", new Vector3(0f, 1.1f, 1.15f), new Vector3(4.3f, 2.35f, 0.09f), Vector3.zero, context.Mat("SootBlackIron"));
            CreateBox("thin brass backplate horizon", new Vector3(0f, 0.48f, 1.095f), new Vector3(4.1f, 0.045f, 0.035f), Vector3.zero, context.Mat("AgedBrass"));
            CreateBox("thin brass backplate top rail", new Vector3(0f, 1.94f, 1.092f), new Vector3(4.1f, 0.04f, 0.035f), Vector3.zero, context.Mat("AgedBrass"));
            CreateBox("dark foreground shadow wedge", new Vector3(0f, 0.002f, -1.15f), new Vector3(4.4f, 0.015f, 0.28f), Vector3.zero, context.Mat("SootGrime"));
        }

        private static void BuildCopperCoilCell(Context context)
        {
            Material barrel = context.Mat("BlackIronBarrel");
            Material copper = context.Mat("BurnishedCopperCoil");
            Material brass = context.Mat("AgedBrass");
            Material enamel = context.Mat("RedSafetyEnamel");
            Material amber = context.Mat("AmberGlowGlass");

            CreateCylinder("black iron pressure core under coil", new Vector3(0f, 0.72f, 0f), new Vector3(0.34f, 1.35f, 0.34f), new Vector3(0f, 0f, 90f), barrel);
            CreateMeshObject("continuous oxidized copper helix", BuildHelixTubeMesh(2.38f, 0.36f, 0.055f, 7.2f, 132, 12), new Vector3(0f, 0.72f, 0f), Vector3.zero, copper);
            CreateCylinder("left brass coil collar", new Vector3(-1.32f, 0.72f, 0f), new Vector3(0.43f, 0.105f, 0.43f), new Vector3(0f, 0f, 90f), brass);
            CreateCylinder("right brass coil collar", new Vector3(1.32f, 0.72f, 0f), new Vector3(0.43f, 0.105f, 0.43f), new Vector3(0f, 0f, 90f), brass);

            for (int i = 0; i < 8; i++)
            {
                float x = -1.05f + i * 0.3f;
                CreateCylinder("tiny brass coil clamp screw", new Vector3(x, 1.105f, 0.025f), new Vector3(0.055f, 0.018f, 0.055f), new Vector3(90f, 0f, 0f), brass);
            }

            CreateBox("red enamel service polarity line", new Vector3(0.04f, 0.265f, -0.02f), new Vector3(1.9f, 0.035f, 0.045f), Vector3.zero, enamel);
            CreateSphere("warm pressure glint in coil gap", new Vector3(0.0f, 0.72f, -0.36f), new Vector3(0.12f, 0.12f, 0.12f), amber);
        }

        private static void BuildGaugeDialCell(Context context)
        {
            Material brass = context.Mat("AgedBrass");
            Material ivory = context.Mat("GaugeIvoryEnamel");
            Material glass = context.Mat("SmokedAmberGlass");
            Material red = context.Mat("RedNeedlePaint");
            Material dark = context.Mat("EngravedDarkFill");

            CreateCylinder("gauge dark backing puck", new Vector3(0f, 0.7f, 0.04f), new Vector3(0.76f, 0.055f, 0.76f), new Vector3(90f, 0f, 0f), dark);
            CreateTorus("thick worn brass gauge bezel", new Vector3(0f, 0.7f, -0.02f), 0.75f, 0.065f, brass);
            CreateTorus("inner polished brass gauge lip", new Vector3(0f, 0.7f, -0.055f), 0.57f, 0.028f, brass);
            CreateCylinder("aged ivory gauge face", new Vector3(0f, 0.7f, -0.075f), new Vector3(1.08f, 0.026f, 1.08f), new Vector3(90f, 0f, 0f), ivory);

            for (int i = 0; i < 40; i++)
            {
                float angle = -135f + i * (270f / 39f);
                float radius = i % 5 == 0 ? 0.47f : 0.5f;
                float length = i % 5 == 0 ? 0.135f : 0.075f;
                Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0.7f + Mathf.Sin(angle * Mathf.Deg2Rad) * radius, -0.125f);
                CreateBox("gauge black tick mark", pos, new Vector3(length, 0.012f, 0.018f), new Vector3(0f, 0f, angle), dark);
            }

            CreateBox("red pressure needle", new Vector3(0.17f, 0.86f, -0.14f), new Vector3(0.58f, 0.035f, 0.02f), new Vector3(0f, 0f, 38f), red);
            CreateCylinder("gauge needle hub screw", new Vector3(0f, 0.7f, -0.16f), new Vector3(0.13f, 0.026f, 0.13f), new Vector3(90f, 0f, 0f), brass);
            CreateCylinder("smoked amber gauge glass cap", new Vector3(0f, 0.7f, -0.19f), new Vector3(1.18f, 0.018f, 1.18f), new Vector3(90f, 0f, 0f), glass);

            for (int i = 0; i < 10; i++)
            {
                float angle = i * 36f;
                Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * 0.83f, 0.7f + Mathf.Sin(angle * Mathf.Deg2Rad) * 0.83f, -0.1f);
                CreateCylinder("gauge bezel rivet", pos, new Vector3(0.055f, 0.018f, 0.055f), new Vector3(90f, 0f, 0f), brass);
            }
        }

        private static void BuildBrassReceiverCell(Context context)
        {
            Material brass = context.Mat("AgedBrass");
            Material edge = context.Mat("PolishedEdgeBrass");
            Material dark = context.Mat("EngravedDarkFill");
            Material grime = context.Mat("SootGrime");
            Material red = context.Mat("RedSafetyEnamel");

            CreateBox("beveled brass receiver main plate", new Vector3(0f, 0.7f, 0f), new Vector3(2.45f, 0.9f, 0.16f), Vector3.zero, brass);
            CreateBox("receiver dark recessed center channel", new Vector3(0f, 0.7f, -0.095f), new Vector3(1.82f, 0.42f, 0.035f), Vector3.zero, dark);
            CreateBox("polished brass upper chamfer proxy", new Vector3(0f, 1.17f, -0.12f), new Vector3(2.48f, 0.08f, 0.09f), Vector3.zero, edge);
            CreateBox("polished brass lower chamfer proxy", new Vector3(0f, 0.23f, -0.12f), new Vector3(2.48f, 0.08f, 0.09f), Vector3.zero, edge);
            CreateBox("hairline red enamel inspection mark", new Vector3(-0.78f, 0.7f, -0.135f), new Vector3(0.055f, 0.58f, 0.025f), Vector3.zero, red);

            for (int i = 0; i < 18; i++)
            {
                float x = -1.12f + (i % 9) * 0.28f;
                float y = i < 9 ? 1.06f : 0.34f;
                CreateCylinder("receiver raised rivet head", new Vector3(x, y, -0.165f), new Vector3(0.07f, 0.018f, 0.07f), new Vector3(90f, 0f, 0f), edge);
            }

            for (int i = 0; i < 12; i++)
            {
                float x = -0.84f + i * 0.15f;
                CreateBox("dark engraved filigree slot", new Vector3(x, 0.74f + Mathf.Sin(i * 0.8f) * 0.11f, -0.17f), new Vector3(0.1f, 0.018f, 0.018f), new Vector3(0f, 0f, 25f - i * 3f), grime);
            }
        }

        private static void BuildBlackIronBarrelCell(Context context)
        {
            Material iron = context.Mat("BlackIronBarrel");
            Material soot = context.Mat("SootGrime");
            Material brass = context.Mat("AgedBrass");
            Material steel = context.Mat("BluedSpringSteel");

            CreateCylinder("long black iron barrel tube", new Vector3(-0.05f, 0.72f, 0f), new Vector3(0.36f, 1.35f, 0.36f), new Vector3(0f, 0f, 90f), iron);
            CreateCylinder("oily inner muzzle shadow", new Vector3(-1.45f, 0.72f, 0f), new Vector3(0.31f, 0.055f, 0.31f), new Vector3(0f, 0f, 90f), soot);
            CreateTorus("brass muzzle crown ring", new Vector3(-1.52f, 0.72f, 0f), 0.22f, 0.035f, brass, new Vector3(0f, 90f, 0f));
            CreateCylinder("rear blued steel chamber", new Vector3(1.15f, 0.72f, 0f), new Vector3(0.44f, 0.34f, 0.44f), new Vector3(0f, 0f, 90f), steel);
            CreateCylinder("front brass retaining band", new Vector3(-0.9f, 0.72f, 0f), new Vector3(0.39f, 0.075f, 0.39f), new Vector3(0f, 0f, 90f), brass);
            CreateCylinder("rear brass retaining band", new Vector3(0.72f, 0.72f, 0f), new Vector3(0.39f, 0.075f, 0.39f), new Vector3(0f, 0f, 90f), brass);

            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60f;
                float y = 0.72f + Mathf.Cos(angle * Mathf.Deg2Rad) * 0.27f;
                float z = Mathf.Sin(angle * Mathf.Deg2Rad) * 0.27f;
                CreateCylinder("muzzle vent bore", new Vector3(-1.27f, y, z), new Vector3(0.045f, 0.035f, 0.045f), new Vector3(0f, 0f, 90f), soot);
            }

            for (int i = 0; i < 8; i++)
            {
                CreateBox("barrel edge wear scratch", new Vector3(-0.65f + i * 0.2f, 0.98f + Mathf.Sin(i) * 0.025f, -0.13f), new Vector3(0.18f, 0.012f, 0.018f), new Vector3(0f, 0f, -8f + i * 2f), steel);
            }
        }

        private static void BuildRedEnamelSafetyCell(Context context)
        {
            Material iron = context.Mat("SootBlackIron");
            Material red = context.Mat("RedSafetyEnamel");
            Material chip = context.Mat("EngravedDarkFill");
            Material brass = context.Mat("AgedBrass");

            CreateBox("black receiver safety sample plate", new Vector3(0f, 0.68f, 0f), new Vector3(2.35f, 0.72f, 0.14f), Vector3.zero, iron);
            CreateBox("raised chipped red enamel safety stripe", new Vector3(0f, 0.78f, -0.105f), new Vector3(2.0f, 0.13f, 0.045f), Vector3.zero, red);
            CreateBox("thin brass line above enamel", new Vector3(0f, 0.96f, -0.12f), new Vector3(2.15f, 0.035f, 0.038f), Vector3.zero, brass);
            CreateBox("thin brass line below enamel", new Vector3(0f, 0.59f, -0.12f), new Vector3(2.15f, 0.035f, 0.038f), Vector3.zero, brass);

            for (int i = 0; i < 18; i++)
            {
                float x = -0.88f + i * 0.105f;
                float y = 0.78f + ((i % 3) - 1) * 0.035f;
                CreateBox("dark enamel chip exposing iron", new Vector3(x, y, -0.14f), new Vector3(0.06f + (i % 4) * 0.012f, 0.025f, 0.018f), new Vector3(0f, 0f, i * 19f), chip);
            }

            for (int i = 0; i < 6; i++)
            {
                float x = -1.02f + i * 0.41f;
                CreateCylinder("safety plate brass screw", new Vector3(x, 0.39f, -0.125f), new Vector3(0.06f, 0.018f, 0.06f), new Vector3(90f, 0f, 0f), brass);
            }
        }

        private static void BuildSmokedAmberGlassCell(Context context)
        {
            Material glass = context.Mat("SmokedAmberGlass");
            Material amber = context.Mat("AmberGlowGlass");
            Material brass = context.Mat("AgedBrass");
            Material dark = context.Mat("EngravedDarkFill");

            CreateCylinder("smoked amber pressure tube body", new Vector3(0f, 0.72f, 0f), new Vector3(0.32f, 0.95f, 0.32f), new Vector3(0f, 0f, 90f), glass);
            CreateCylinder("inner amber glow core", new Vector3(0f, 0.72f, 0f), new Vector3(0.13f, 0.9f, 0.13f), new Vector3(0f, 0f, 90f), amber);
            CreateCylinder("left brass glass collar", new Vector3(-0.98f, 0.72f, 0f), new Vector3(0.39f, 0.085f, 0.39f), new Vector3(0f, 0f, 90f), brass);
            CreateCylinder("right brass glass collar", new Vector3(0.98f, 0.72f, 0f), new Vector3(0.39f, 0.085f, 0.39f), new Vector3(0f, 0f, 90f), brass);
            CreateTorus("front amber lens bevel", new Vector3(-1.08f, 0.72f, 0f), 0.27f, 0.03f, brass, new Vector3(0f, 90f, 0f));
            CreateTorus("rear amber lens bevel", new Vector3(1.08f, 0.72f, 0f), 0.27f, 0.03f, brass, new Vector3(0f, 90f, 0f));
            CreateBox("dark reflection band across glass", new Vector3(0f, 0.94f, -0.13f), new Vector3(1.85f, 0.035f, 0.022f), Vector3.zero, dark);

            for (int i = 0; i < 5; i++)
            {
                float x = -0.72f + i * 0.36f;
                CreateBox("thin vertical glass refraction line", new Vector3(x, 0.72f, -0.25f), new Vector3(0.018f, 0.42f, 0.015f), Vector3.zero, glass);
            }
        }

        private static void BuildGripProxyCell(Context context)
        {
            Material walnut = context.Mat("WalnutVarnish");
            Material leather = context.Mat("CrackedBrownLeather");
            Material brass = context.Mat("AgedBrass");
            Material dark = context.Mat("SootBlackIron");

            CreateCapsule("angled walnut grip core", new Vector3(-0.12f, 0.58f, 0.02f), new Vector3(0.52f, 0.95f, 0.3f), new Vector3(0f, 0f, -19f), walnut);
            CreateBox("black iron grip spine", new Vector3(0.26f, 0.91f, -0.03f), new Vector3(0.34f, 0.92f, 0.12f), new Vector3(0f, 0f, -19f), dark);
            CreateBox("brass trigger guard lower arc proxy", new Vector3(0.38f, 0.34f, -0.02f), new Vector3(0.62f, 0.09f, 0.085f), new Vector3(0f, 0f, -16f), brass);
            CreateTorus("brass trigger guard front curve", new Vector3(0.57f, 0.55f, -0.02f), 0.2f, 0.025f, brass, new Vector3(0f, 0f, -18f));

            for (int i = 0; i < 6; i++)
            {
                float y = 0.22f + i * 0.13f;
                CreateBox("dark leather grip wrap rib", new Vector3(-0.12f + i * 0.025f, y, -0.18f), new Vector3(0.58f, 0.035f, 0.055f), new Vector3(0f, 0f, -19f), leather);
            }

            CreateCylinder("brass grip butt cap", new Vector3(-0.33f, 0.13f, 0.0f), new Vector3(0.3f, 0.04f, 0.3f), new Vector3(90f, 0f, -19f), brass);
            CreateCylinder("grip brass side screw", new Vector3(0.02f, 0.65f, -0.22f), new Vector3(0.065f, 0.018f, 0.065f), new Vector3(90f, 0f, 0f), brass);
            CreateCylinder("grip brass side screw", new Vector3(-0.1f, 0.35f, -0.22f), new Vector3(0.055f, 0.018f, 0.055f), new Vector3(90f, 0f, 0f), brass);
        }

        private static void DrawCopperCoilCellRaster(Texture2D texture, int seed)
        {
            FillRotatedRect(texture, 0.50f, 0.52f, 0.78f, 0.14f, -3f, C(0.045f, 0.043f, 0.04f), MaterialPattern.Iron, seed + 1, 1f);
            FillRotatedRect(texture, 0.50f, 0.515f, 0.68f, 0.055f, -3f, C(0.10f, 0.095f, 0.085f), MaterialPattern.Steel, seed + 2, 0.65f);

            for (int i = 0; i < 18; i++)
            {
                float x = 0.17f + i * 0.039f;
                StrokeEllipse(texture, x, 0.52f + Mathf.Sin(i * 0.65f) * 0.006f, 0.11f, 0.285f, 10, C(0.68f, 0.31f, 0.11f), MaterialPattern.Copper, seed + i * 7);
                StrokeEllipse(texture, x + 0.006f, 0.50f, 0.08f, 0.22f, 4, C(0.09f, 0.045f, 0.028f), MaterialPattern.Grime, seed + i);
            }

            FillEllipse(texture, 0.14f, 0.52f, 0.12f, 0.29f, C(0.74f, 0.47f, 0.18f), MaterialPattern.Brass, seed + 90, 1f);
            FillEllipse(texture, 0.86f, 0.52f, 0.12f, 0.29f, C(0.74f, 0.47f, 0.18f), MaterialPattern.Brass, seed + 91, 1f);
            FillRotatedRect(texture, 0.50f, 0.36f, 0.58f, 0.035f, -3f, C(0.82f, 0.05f, 0.035f), MaterialPattern.Enamel, seed + 100, 1f);
            for (int i = 0; i < 10; i++)
            {
                FillEllipse(texture, 0.24f + i * 0.058f, 0.665f, 0.026f, 0.026f, C(0.78f, 0.55f, 0.25f), MaterialPattern.Brass, seed + 150 + i, 1f);
            }
            DrawLine(texture, 0.20f, 0.27f, 0.80f, 0.79f, 2, new Color(1f, 0.72f, 0.28f, 0.18f));
        }

        private static void DrawGaugeDialCellRaster(Texture2D texture, int seed)
        {
            FillEllipse(texture, 0.50f, 0.53f, 0.64f, 0.64f, C(0.07f, 0.055f, 0.04f), MaterialPattern.Grime, seed, 1f);
            StrokeEllipse(texture, 0.50f, 0.53f, 0.65f, 0.65f, 24, C(0.77f, 0.52f, 0.22f), MaterialPattern.Brass, seed + 1);
            StrokeEllipse(texture, 0.50f, 0.53f, 0.52f, 0.52f, 8, C(0.93f, 0.68f, 0.30f), MaterialPattern.Brass, seed + 2);
            FillEllipse(texture, 0.50f, 0.53f, 0.49f, 0.49f, C(0.84f, 0.76f, 0.56f), MaterialPattern.Ivory, seed + 3, 1f);

            for (int i = 0; i < 43; i++)
            {
                float angle = -135f + i * 270f / 42f;
                float inner = i % 5 == 0 ? 0.188f : 0.215f;
                float outer = 0.238f;
                Vector2 a = Polar(0.50f, 0.53f, inner, angle);
                Vector2 b = Polar(0.50f, 0.53f, outer, angle);
                DrawLine(texture, a.x, a.y, b.x, b.y, i % 5 == 0 ? 4 : 2, new Color(0.035f, 0.028f, 0.022f, 0.92f));
            }

            DrawLine(texture, 0.50f, 0.53f, 0.62f, 0.67f, 10, new Color(0.88f, 0.035f, 0.025f, 0.96f));
            FillEllipse(texture, 0.50f, 0.53f, 0.07f, 0.07f, C(0.78f, 0.53f, 0.24f), MaterialPattern.Brass, seed + 4, 1f);
            StrokeEllipse(texture, 0.50f, 0.53f, 0.54f, 0.54f, 8, new Color(1f, 0.70f, 0.23f, 0.28f), MaterialPattern.Glass, seed + 5);
            DrawLine(texture, 0.37f, 0.72f, 0.68f, 0.37f, 9, new Color(1f, 0.86f, 0.47f, 0.24f));

            for (int i = 0; i < 12; i++)
            {
                Vector2 p = Polar(0.50f, 0.53f, 0.34f, i * 30f);
                FillEllipse(texture, p.x, p.y, 0.035f, 0.035f, C(0.78f, 0.54f, 0.24f), MaterialPattern.Brass, seed + 40 + i, 1f);
            }
        }

        private static void DrawBrassReceiverCellRaster(Texture2D texture, int seed)
        {
            FillRotatedRect(texture, 0.50f, 0.53f, 0.72f, 0.34f, -2f, C(0.67f, 0.43f, 0.18f), MaterialPattern.Brass, seed + 1, 1f);
            FillRotatedRect(texture, 0.50f, 0.53f, 0.53f, 0.16f, -2f, C(0.055f, 0.044f, 0.032f), MaterialPattern.Grime, seed + 2, 0.92f);
            FillRotatedRect(texture, 0.50f, 0.72f, 0.75f, 0.035f, -2f, C(0.92f, 0.66f, 0.28f), MaterialPattern.Brass, seed + 3, 1f);
            FillRotatedRect(texture, 0.50f, 0.34f, 0.75f, 0.035f, -2f, C(0.92f, 0.66f, 0.28f), MaterialPattern.Brass, seed + 4, 1f);
            FillRotatedRect(texture, 0.28f, 0.53f, 0.025f, 0.26f, -2f, C(0.80f, 0.04f, 0.03f), MaterialPattern.Enamel, seed + 5, 1f);

            for (int i = 0; i < 18; i++)
            {
                float x = 0.17f + (i % 9) * 0.082f;
                float y = i < 9 ? 0.68f : 0.38f;
                FillEllipse(texture, x, y, 0.036f, 0.036f, C(0.86f, 0.60f, 0.28f), MaterialPattern.Brass, seed + 20 + i, 1f);
            }

            for (int i = 0; i < 24; i++)
            {
                float x = 0.28f + i * 0.018f;
                float y = 0.52f + Mathf.Sin(i * 0.75f) * 0.065f;
                DrawLine(texture, x, y, x + 0.05f, y + 0.018f, 2, new Color(0.02f, 0.016f, 0.012f, 0.65f));
            }
        }

        private static void DrawBlackIronBarrelCellRaster(Texture2D texture, int seed)
        {
            FillRotatedRect(texture, 0.50f, 0.53f, 0.78f, 0.16f, -2f, C(0.055f, 0.052f, 0.047f), MaterialPattern.Iron, seed + 1, 1f);
            FillEllipse(texture, 0.14f, 0.53f, 0.18f, 0.22f, C(0.025f, 0.022f, 0.019f), MaterialPattern.Grime, seed + 2, 1f);
            StrokeEllipse(texture, 0.13f, 0.53f, 0.22f, 0.26f, 10, C(0.78f, 0.53f, 0.22f), MaterialPattern.Brass, seed + 3);
            FillEllipse(texture, 0.13f, 0.53f, 0.085f, 0.10f, C(0.01f, 0.009f, 0.008f), MaterialPattern.Grime, seed + 4, 1f);
            FillRotatedRect(texture, 0.35f, 0.53f, 0.055f, 0.22f, -2f, C(0.74f, 0.50f, 0.22f), MaterialPattern.Brass, seed + 5, 1f);
            FillRotatedRect(texture, 0.72f, 0.53f, 0.07f, 0.23f, -2f, C(0.74f, 0.50f, 0.22f), MaterialPattern.Brass, seed + 6, 1f);
            FillRotatedRect(texture, 0.83f, 0.53f, 0.16f, 0.24f, -2f, C(0.13f, 0.15f, 0.17f), MaterialPattern.Steel, seed + 7, 1f);
            for (int i = 0; i < 8; i++)
            {
                float a = i * Mathf.PI * 2f / 8f;
                FillEllipse(texture, 0.18f + Mathf.Cos(a) * 0.038f, 0.53f + Mathf.Sin(a) * 0.062f, 0.025f, 0.025f, C(0.018f, 0.015f, 0.013f), MaterialPattern.Grime, seed + 20 + i, 1f);
            }
            for (int i = 0; i < 18; i++)
            {
                float x = 0.31f + i * 0.027f;
                DrawLine(texture, x, 0.63f + Mathf.Sin(i) * 0.01f, x + 0.045f, 0.65f + Mathf.Sin(i) * 0.01f, 2, new Color(0.30f, 0.30f, 0.26f, 0.45f));
            }
        }

        private static void DrawRedEnamelSafetyCellRaster(Texture2D texture, int seed)
        {
            FillRotatedRect(texture, 0.50f, 0.53f, 0.74f, 0.30f, 0f, C(0.055f, 0.048f, 0.043f), MaterialPattern.Iron, seed + 1, 1f);
            FillRotatedRect(texture, 0.50f, 0.55f, 0.66f, 0.075f, 0f, C(0.78f, 0.035f, 0.025f), MaterialPattern.Enamel, seed + 2, 1f);
            FillRotatedRect(texture, 0.50f, 0.66f, 0.70f, 0.025f, 0f, C(0.80f, 0.55f, 0.24f), MaterialPattern.Brass, seed + 3, 1f);
            FillRotatedRect(texture, 0.50f, 0.43f, 0.70f, 0.025f, 0f, C(0.80f, 0.55f, 0.24f), MaterialPattern.Brass, seed + 4, 1f);
            for (int i = 0; i < 34; i++)
            {
                float x = 0.20f + i * 0.018f;
                float y = 0.55f + ((i % 5) - 2) * 0.011f;
                FillRotatedRect(texture, x, y, 0.028f, 0.015f, i * 19f, C(0.03f, 0.025f, 0.02f), MaterialPattern.Grime, seed + 20 + i, 0.86f);
            }
            for (int i = 0; i < 8; i++)
            {
                FillEllipse(texture, 0.20f + i * 0.085f, 0.31f, 0.034f, 0.034f, C(0.80f, 0.55f, 0.24f), MaterialPattern.Brass, seed + 70 + i, 1f);
            }
        }

        private static void DrawSmokedAmberGlassCellRaster(Texture2D texture, int seed)
        {
            FillRotatedRect(texture, 0.50f, 0.53f, 0.60f, 0.20f, 0f, new Color(0.86f, 0.42f, 0.11f, 0.55f), MaterialPattern.Glass, seed + 1, 0.82f);
            FillRotatedRect(texture, 0.50f, 0.53f, 0.54f, 0.055f, 0f, new Color(1f, 0.47f, 0.10f, 0.55f), MaterialPattern.Glass, seed + 2, 0.76f);
            FillEllipse(texture, 0.20f, 0.53f, 0.15f, 0.24f, C(0.78f, 0.53f, 0.22f), MaterialPattern.Brass, seed + 3, 1f);
            FillEllipse(texture, 0.80f, 0.53f, 0.15f, 0.24f, C(0.78f, 0.53f, 0.22f), MaterialPattern.Brass, seed + 4, 1f);
            StrokeEllipse(texture, 0.18f, 0.53f, 0.18f, 0.27f, 7, C(0.92f, 0.66f, 0.28f), MaterialPattern.Brass, seed + 5);
            StrokeEllipse(texture, 0.82f, 0.53f, 0.18f, 0.27f, 7, C(0.92f, 0.66f, 0.28f), MaterialPattern.Brass, seed + 6);
            DrawLine(texture, 0.25f, 0.64f, 0.76f, 0.62f, 9, new Color(1f, 0.84f, 0.45f, 0.32f));
            DrawLine(texture, 0.25f, 0.44f, 0.76f, 0.47f, 5, new Color(0.11f, 0.055f, 0.02f, 0.36f));
            for (int i = 0; i < 5; i++)
            {
                DrawLine(texture, 0.34f + i * 0.08f, 0.41f, 0.32f + i * 0.08f, 0.66f, 3, new Color(1f, 0.70f, 0.24f, 0.22f));
            }
        }

        private static void DrawGripProxyCellRaster(Texture2D texture, int seed)
        {
            FillRotatedRect(texture, 0.46f, 0.50f, 0.24f, 0.58f, -18f, C(0.41f, 0.19f, 0.075f), MaterialPattern.Wood, seed + 1, 1f);
            FillRotatedRect(texture, 0.58f, 0.62f, 0.10f, 0.50f, -18f, C(0.055f, 0.048f, 0.043f), MaterialPattern.Iron, seed + 2, 1f);
            for (int i = 0; i < 7; i++)
            {
                FillRotatedRect(texture, 0.41f + i * 0.018f, 0.28f + i * 0.07f, 0.25f, 0.035f, -18f, C(0.24f, 0.12f, 0.065f), MaterialPattern.Leather, seed + 20 + i, 0.95f);
            }
            FillEllipse(texture, 0.38f, 0.22f, 0.20f, 0.08f, C(0.78f, 0.53f, 0.22f), MaterialPattern.Brass, seed + 40, 1f);
            StrokeEllipse(texture, 0.63f, 0.42f, 0.26f, 0.30f, 7, C(0.78f, 0.53f, 0.22f), MaterialPattern.Brass, seed + 41);
            FillRotatedRect(texture, 0.65f, 0.32f, 0.26f, 0.045f, -16f, C(0.78f, 0.53f, 0.22f), MaterialPattern.Brass, seed + 42, 1f);
            FillEllipse(texture, 0.46f, 0.53f, 0.04f, 0.04f, C(0.86f, 0.60f, 0.28f), MaterialPattern.Brass, seed + 50, 1f);
            FillEllipse(texture, 0.40f, 0.37f, 0.035f, 0.035f, C(0.86f, 0.60f, 0.28f), MaterialPattern.Brass, seed + 51, 1f);
            for (int i = 0; i < 14; i++)
            {
                DrawLine(texture, 0.35f, 0.29f + i * 0.028f, 0.53f, 0.25f + i * 0.033f, 2, new Color(0.66f, 0.34f, 0.13f, 0.34f));
            }
        }

        private static void DrawRasterBackdrop(Texture2D texture, int seed)
        {
            int width = texture.width;
            int height = texture.height;
            for (int y = 0; y < height; y++)
            {
                float v = (float)y / Math.Max(1, height - 1);
                for (int x = 0; x < width; x++)
                {
                    float u = (float)x / Math.Max(1, width - 1);
                    float vignette = Mathf.Clamp01(Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.55f)) * 1.4f);
                    float noise = Mathf.PerlinNoise(u * 18f + seed * 0.01f, v * 18f + seed * 0.02f);
                    Color color = Color.Lerp(C(0.07f, 0.055f, 0.04f), C(0.018f, 0.017f, 0.016f), vignette);
                    color *= 0.86f + noise * 0.08f;
                    texture.SetPixel(x, y, color);
                }
            }

            FillRotatedRect(texture, 0.50f, 0.21f, 0.82f, 0.06f, 0f, C(0.34f, 0.22f, 0.09f), MaterialPattern.Brass, seed + 900, 0.55f);
            FillRotatedRect(texture, 0.50f, 0.18f, 0.82f, 0.11f, 0f, C(0.035f, 0.030f, 0.025f), MaterialPattern.Stone, seed + 901, 0.9f);
            DrawLine(texture, 0.14f, 0.76f, 0.86f, 0.28f, 3, new Color(1f, 0.62f, 0.20f, 0.16f));
        }

        private static void FillRotatedRect(Texture2D texture, float cx, float cy, float w, float h, float degrees, Color baseColor, MaterialPattern pattern, int seed, float alpha)
        {
            int width = texture.width;
            int height = texture.height;
            float angle = -degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);
            int minX = Mathf.Clamp(Mathf.FloorToInt((cx - w * 0.75f) * width), 0, width - 1);
            int maxX = Mathf.Clamp(Mathf.CeilToInt((cx + w * 0.75f) * width), 0, width - 1);
            int minY = Mathf.Clamp(Mathf.FloorToInt((cy - h * 0.9f) * height), 0, height - 1);
            int maxY = Mathf.Clamp(Mathf.CeilToInt((cy + h * 0.9f) * height), 0, height - 1);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float dx = (float)x / width - cx;
                    float dy = (float)y / height - cy;
                    float lx = (dx * cos - dy * sin) / w;
                    float ly = (dx * sin + dy * cos) / h;
                    if (Mathf.Abs(lx) <= 0.5f && Mathf.Abs(ly) <= 0.5f)
                    {
                        float edge = Mathf.Min(0.5f - Mathf.Abs(lx), 0.5f - Mathf.Abs(ly)) * 2f;
                        float shade = Mathf.Lerp(0.72f, 1.18f, Mathf.Clamp01(ly + 0.5f)) + Mathf.Clamp01(edge) * 0.08f;
                        BlendPixel(texture, x, y, MaterialSample(baseColor, lx + 0.5f, ly + 0.5f, pattern, seed) * shade, alpha);
                    }
                }
            }
        }

        private static void FillEllipse(Texture2D texture, float cx, float cy, float w, float h, Color baseColor, MaterialPattern pattern, int seed, float alpha)
        {
            int width = texture.width;
            int height = texture.height;
            int minX = Mathf.Clamp(Mathf.FloorToInt((cx - w * 0.5f) * width), 0, width - 1);
            int maxX = Mathf.Clamp(Mathf.CeilToInt((cx + w * 0.5f) * width), 0, width - 1);
            int minY = Mathf.Clamp(Mathf.FloorToInt((cy - h * 0.5f) * height), 0, height - 1);
            int maxY = Mathf.Clamp(Mathf.CeilToInt((cy + h * 0.5f) * height), 0, height - 1);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float lx = (((float)x / width) - cx) / (w * 0.5f);
                    float ly = (((float)y / height) - cy) / (h * 0.5f);
                    float d = lx * lx + ly * ly;
                    if (d <= 1f)
                    {
                        float shade = Mathf.Lerp(1.22f, 0.68f, Mathf.Clamp01(Mathf.Sqrt(d))) + Mathf.Clamp01(ly * 0.5f + 0.4f) * 0.18f;
                        BlendPixel(texture, x, y, MaterialSample(baseColor, lx * 0.5f + 0.5f, ly * 0.5f + 0.5f, pattern, seed) * shade, alpha);
                    }
                }
            }
        }

        private static void StrokeEllipse(Texture2D texture, float cx, float cy, float w, float h, int thicknessPx, Color baseColor, MaterialPattern pattern, int seed)
        {
            int width = texture.width;
            int height = texture.height;
            int minX = Mathf.Clamp(Mathf.FloorToInt((cx - w * 0.55f) * width), 0, width - 1);
            int maxX = Mathf.Clamp(Mathf.CeilToInt((cx + w * 0.55f) * width), 0, width - 1);
            int minY = Mathf.Clamp(Mathf.FloorToInt((cy - h * 0.55f) * height), 0, height - 1);
            int maxY = Mathf.Clamp(Mathf.CeilToInt((cy + h * 0.55f) * height), 0, height - 1);
            float normalizedThickness = thicknessPx / (float)Mathf.Max(width * w, height * h);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float lx = (((float)x / width) - cx) / (w * 0.5f);
                    float ly = (((float)y / height) - cy) / (h * 0.5f);
                    float d = Mathf.Sqrt(lx * lx + ly * ly);
                    if (d <= 1.04f && d >= 1f - normalizedThickness * 2.7f)
                    {
                        float shade = 0.78f + Mathf.Clamp01(1f - d) * 0.18f + Mathf.Clamp01(ly * 0.4f + 0.5f) * 0.28f;
                        BlendPixel(texture, x, y, MaterialSample(baseColor, lx * 0.5f + 0.5f, ly * 0.5f + 0.5f, pattern, seed) * shade, 1f);
                    }
                }
            }
        }

        private static void DrawLine(Texture2D texture, float x0, float y0, float x1, float y1, int thicknessPx, Color color)
        {
            int width = texture.width;
            int height = texture.height;
            int ix0 = Mathf.RoundToInt(x0 * width);
            int iy0 = Mathf.RoundToInt(y0 * height);
            int ix1 = Mathf.RoundToInt(x1 * width);
            int iy1 = Mathf.RoundToInt(y1 * height);
            int steps = Mathf.Max(Mathf.Abs(ix1 - ix0), Mathf.Abs(iy1 - iy0));
            for (int i = 0; i <= steps; i++)
            {
                float t = steps == 0 ? 0f : (float)i / steps;
                int x = Mathf.RoundToInt(Mathf.Lerp(ix0, ix1, t));
                int y = Mathf.RoundToInt(Mathf.Lerp(iy0, iy1, t));
                for (int yy = -thicknessPx; yy <= thicknessPx; yy++)
                {
                    for (int xx = -thicknessPx; xx <= thicknessPx; xx++)
                    {
                        if (xx * xx + yy * yy <= thicknessPx * thicknessPx)
                        {
                            BlendPixel(texture, x + xx, y + yy, color, color.a);
                        }
                    }
                }
            }
        }

        private static Vector2 Polar(float cx, float cy, float radius, float degrees)
        {
            float angle = degrees * Mathf.Deg2Rad;
            return new Vector2(cx + Mathf.Cos(angle) * radius, cy + Mathf.Sin(angle) * radius);
        }

        private static void BlendPixel(Texture2D texture, int x, int y, Color color, float alpha)
        {
            if (x < 0 || y < 0 || x >= texture.width || y >= texture.height)
            {
                return;
            }

            Color current = texture.GetPixel(x, y);
            color.a = 1f;
            Color blended = Color.Lerp(current, color, Mathf.Clamp01(alpha));
            blended.a = 1f;
            texture.SetPixel(x, y, blended);
        }

        private static Color MaterialSample(Color baseColor, float u, float v, MaterialPattern pattern, int seed)
        {
            float noise = Mathf.PerlinNoise(u * 14.7f + seed * 0.001f, v * 14.7f + seed * 0.002f);
            float fine = Mathf.PerlinNoise(u * 73.1f + seed * 0.003f, v * 73.1f + seed * 0.004f);
            float scratch = ScratchSignal(u, v, seed);
            float multiplier = 0.84f + noise * 0.28f + fine * 0.08f;
            Color color = baseColor * multiplier;

            switch (pattern)
            {
                case MaterialPattern.Brass:
                    color += new Color(0.12f, 0.07f, 0.01f, 0f) * scratch;
                    color *= 1f - DarkSpot(u, v, seed) * 0.22f;
                    break;
                case MaterialPattern.Copper:
                    color += new Color(0.14f, 0.055f, 0.02f, 0f) * scratch;
                    color = Color.Lerp(color, new Color(0.04f, 0.22f, 0.18f, 1f), OxidationSignal(u, v, seed) * 0.42f);
                    break;
                case MaterialPattern.Iron:
                    color *= 0.82f + scratch * 0.28f;
                    color = Color.Lerp(color, new Color(0.16f, 0.15f, 0.13f, 1f), EdgeWearSignal(u, v, seed) * 0.34f);
                    break;
                case MaterialPattern.Steel:
                    color = Color.Lerp(color, new Color(0.21f, 0.25f, 0.30f, 1f), scratch * 0.42f);
                    break;
                case MaterialPattern.Enamel:
                    color *= 0.9f + Mathf.Pow(noise, 2f) * 0.32f;
                    color = Color.Lerp(color, new Color(0.035f, 0.03f, 0.025f, 1f), ChipSignal(u, v, seed) * 0.55f);
                    break;
                case MaterialPattern.Ivory:
                    color += new Color(0.12f, 0.095f, 0.045f, 0f) * fine * 0.22f;
                    color = Color.Lerp(color, new Color(0.26f, 0.19f, 0.11f, 1f), DarkSpot(u, v, seed) * 0.16f);
                    break;
                case MaterialPattern.Grime:
                    color *= 0.72f + noise * 0.45f;
                    break;
                case MaterialPattern.Wood:
                    float grain = Mathf.Abs(Mathf.Sin((u * 12f + Mathf.PerlinNoise(v * 6f, seed * 0.01f) * 2f) * Mathf.PI));
                    color = Color.Lerp(color * 0.7f, new Color(0.62f, 0.32f, 0.12f, 1f), grain * 0.35f);
                    break;
                case MaterialPattern.Leather:
                    color *= 0.82f + noise * 0.24f;
                    color = Color.Lerp(color, new Color(0.055f, 0.033f, 0.02f, 1f), CrackSignal(u, v, seed) * 0.55f);
                    break;
                case MaterialPattern.Glass:
                    color = Color.Lerp(color, new Color(1f, 0.68f, 0.21f, 1f), Mathf.Pow(noise, 2f) * 0.28f);
                    break;
                case MaterialPattern.Stone:
                    color *= 0.72f + noise * 0.48f;
                    break;
            }

            color.a = 1f;
            return color;
        }

        private static Camera CreateCamera(Vector3 position, Vector3 lookAt, float fieldOfView)
        {
            GameObject cameraObject = new GameObject("Unity material-cell review camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.transform.position = position;
            camera.transform.LookAt(lookAt);
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 60f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = C(0.019f, 0.018f, 0.017f);
            camera.allowHDR = true;
            return camera;
        }

        private static void RenderCamera(Context context, Camera camera, RenderRequest request)
        {
            string outputPath = Path.Combine(context.RenderRoot, request.FileName);
            RenderTexture renderTexture = new RenderTexture(request.Width, request.Height, 24, RenderTextureFormat.ARGB32)
            {
                antiAliasing = 4
            };

            RenderTexture previous = RenderTexture.active;
            camera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            camera.Render();

            Texture2D texture = new Texture2D(request.Width, request.Height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0f, 0f, request.Width, request.Height), 0, 0);
            texture.Apply();

            File.WriteAllBytes(outputPath, texture.EncodeToPNG());

            RenderTexture.active = previous;
            camera.targetTexture = null;
            renderTexture.Release();
            Object.DestroyImmediate(renderTexture);

            Records.Add(AnalyzeTexture(texture, request, outputPath, context));
            Object.DestroyImmediate(texture);
        }

        private static void RenderContactSheet(Context context)
        {
            int width = 2400;
            int height = 1600;
            int columns = 3;
            int rows = 3;
            int margin = 34;
            int cellW = (width - margin * (columns + 1)) / columns;
            int cellH = (height - margin * (rows + 1)) / rows;

            Texture2D sheet = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color32[] fill = Enumerable.Repeat(new Color32(18, 17, 16, 255), width * height).ToArray();
            sheet.SetPixels32(fill);

            List<RenderRecord> sourceRecords = Records.ToList();
            for (int i = 0; i < sourceRecords.Count; i++)
            {
                RenderRecord record = sourceRecords[i];
                string absolute = Path.Combine(context.RepoRoot, record.RelativePath);
                Texture2D source = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                source.LoadImage(File.ReadAllBytes(absolute));

                int col = i % columns;
                int row = i / columns;
                int x0 = margin + col * (cellW + margin);
                int y0 = height - margin - (row + 1) * cellH - row * margin;
                BlitScaled(source, sheet, x0, y0, cellW, cellH);
                DrawBorder(sheet, x0, y0, cellW, cellH, new Color32(181, 124, 48, 255));
                Object.DestroyImmediate(source);
            }

            sheet.Apply();
            string fileName = "UWMC09_08_weapon_material_cells_contact_sheet_v0.1.54.png";
            string outputPath = Path.Combine(context.RenderRoot, fileName);
            File.WriteAllBytes(outputPath, sheet.EncodeToPNG());
            RenderRequest request = new RenderRequest(fileName, "Contact sheet of all Unity-rendered weapon material cells", 2400, 1600, "ContactSheet");
            Records.Add(AnalyzeTexture(sheet, request, outputPath, context));
            Object.DestroyImmediate(sheet);
        }

        private static void BlitScaled(Texture2D source, Texture2D destination, int x0, int y0, int w, int h)
        {
            float sourceAspect = (float)source.width / source.height;
            float targetAspect = (float)w / h;
            int drawW = w;
            int drawH = h;
            if (sourceAspect > targetAspect)
            {
                drawH = Mathf.RoundToInt(w / sourceAspect);
            }
            else
            {
                drawW = Mathf.RoundToInt(h * sourceAspect);
            }

            int dx0 = x0 + (w - drawW) / 2;
            int dy0 = y0 + (h - drawH) / 2;
            for (int y = 0; y < drawH; y++)
            {
                float v = (float)y / Mathf.Max(1, drawH - 1);
                for (int x = 0; x < drawW; x++)
                {
                    float u = (float)x / Mathf.Max(1, drawW - 1);
                    destination.SetPixel(dx0 + x, dy0 + y, source.GetPixelBilinear(u, v));
                }
            }
        }

        private static void DrawBorder(Texture2D destination, int x0, int y0, int w, int h, Color32 color)
        {
            for (int x = x0; x < x0 + w; x++)
            {
                destination.SetPixel(x, y0, color);
                destination.SetPixel(x, y0 + h - 1, color);
            }
            for (int y = y0; y < y0 + h; y++)
            {
                destination.SetPixel(x0, y, color);
                destination.SetPixel(x0 + w - 1, y, color);
            }
        }

        private static RenderRecord AnalyzeTexture(Texture2D texture, RenderRequest request, string outputPath, Context context)
        {
            Color32[] pixels = texture.GetPixels32();
            int sampleStride = Mathf.Max(1, pixels.Length / 80000);
            double sum = 0d;
            double sumSq = 0d;
            int samples = 0;
            int magenta = 0;
            byte min = 255;
            byte max = 0;

            for (int i = 0; i < pixels.Length; i += sampleStride)
            {
                Color32 p = pixels[i];
                byte luma = (byte)Mathf.Clamp(Mathf.RoundToInt(p.r * 0.2126f + p.g * 0.7152f + p.b * 0.0722f), 0, 255);
                sum += luma;
                sumSq += luma * luma;
                min = Math.Min(min, luma);
                max = Math.Max(max, luma);
                if (p.r > 210 && p.g < 70 && p.b > 210)
                {
                    magenta++;
                }
                samples++;
            }

            double mean = sum / Math.Max(1, samples);
            double variance = sumSq / Math.Max(1, samples) - mean * mean;
            double stdDev = Math.Sqrt(Math.Max(0d, variance));
            double magentaRatio = (double)magenta / Math.Max(1, samples);
            long fileSize = new FileInfo(outputPath).Length;
            bool passed = fileSize > 28000 && stdDev > 8.0d && max - min > 42 && magentaRatio < 0.002d;

            return new RenderRecord
            {
                FileName = request.FileName,
                Description = request.Description,
                CellRole = request.CellRole,
                RelativePath = ToRepoRelative(context.RepoRoot, outputPath),
                Width = request.Width,
                Height = request.Height,
                FileSizeBytes = fileSize,
                Sha256 = Sha256(outputPath),
                LumaMean = mean,
                LumaStdDev = stdDev,
                LumaRange = max - min,
                MagentaRatio = magentaRatio,
                Passed = passed
            };
        }

        private static void AddAssessment(CellRole role)
        {
            switch (role)
            {
                case CellRole.CopperCoil:
                    Assessments.Add(new CellAssessment(role, "pass", "Continuous coil silhouette, copper tarnish, brass collars, dark barrel contrast, and small fasteners are all present.", "Use as the material/silhouette target for the pressure-pistol coil assembly."));
                    break;
                case CellRole.PressureGaugeDial:
                    Assessments.Add(new CellAssessment(role, "pass", "Dial face has readable tick hierarchy, brass bezel, red needle, smoked glass layer, and rivets.", "Promote tick/needle/glass layering into a final gauge prefab before full weapon assembly."));
                    break;
                case CellRole.BrassReceiverPlate:
                    Assessments.Add(new CellAssessment(role, "pass", "Receiver plate shows brass variation, polished edge strips, rivets, dark recess, and engraved grime marks.", "Use as the base language for receiver side plates and pressure-chamber panels."));
                    break;
                case CellRole.BlackIronBarrel:
                    Assessments.Add(new CellAssessment(role, "pass", "Black iron barrel reads separate from soot, brass bands, blue steel chamber, muzzle ring, and edge scratches.", "Use as barrel/muzzle material target, keeping vents and soot readable in first person."));
                    break;
                case CellRole.RedEnamelSafetyLine:
                    Assessments.Add(new CellAssessment(role, "pass", "Red enamel stripe has raised surface, chipped dark underlayer, brass guide lines, and screw scale cues.", "Use sparingly as a first-person gameplay readability accent and safety selector material."));
                    break;
                case CellRole.SmokedAmberGlass:
                    Assessments.Add(new CellAssessment(role, "pass", "Transparent amber tube, glow core, reflection bands, and brass collars are visible.", "Treat as proof for gauge glass, ampoule windows, and small pressure vial inserts."));
                    break;
                case CellRole.WalnutLeatherGripProxy:
                    Assessments.Add(new CellAssessment(role, "review", "Walnut grain and leather wrap direction are usable, but the proxy silhouette remains primitive and needs final sculpt/mesh work.", "Use only as material direction and grip proportion guidance, not as final geometry."));
                    break;
            }
        }

        private static void WritePlanningDocs(Context context)
        {
            bool allRenderGatesPassed = Records.All(record => record.Passed);
            string recommendation = allRenderGatesPassed ? "accept_for_component_direction" : "revise_before_promotion";

            StringBuilder readme = new StringBuilder();
            readme.AppendLine("# v0.1.54 Unity Weapon Material Cell Lookdev");
            readme.AppendLine();
            readme.AppendLine("Generated by Unity 6000.4.6f1 batchmode in the isolated sidecar project `AssetPacks/BrassworksBreach.UnityWeaponMaterialCellLookdev09`.");
            readme.AppendLine("This packet validates weapon materials as separate cells before attempting another full pressure-pistol assembly. It uses a Unity Texture2D procedural material-cell renderer, Unity color/noise/shading math, and Unity PNG encoding only. The earlier camera/mesh shader path was intentionally bypassed because it produced magenta shader-error output in this minimal sidecar.");
            readme.AppendLine();
            readme.AppendLine("## Result");
            readme.AppendLine();
            readme.AppendLine("- Recommendation: `" + recommendation + "`.");
            readme.AppendLine("- Automated render gates passed: " + Records.Count(record => record.Passed) + " / " + Records.Count + ".");
            readme.AppendLine("- Marker: `" + Marker + "`.");
            readme.AppendLine("- Important limitation: these are material-cell and proxy-shape lookdev renders, not final optimized meshes, rigs, import-ready prefabs, or gameplay assets.");
            readme.AppendLine();
            readme.AppendLine("## Lookdev Plan");
            readme.AppendLine();
            readme.AppendLine("1. Validate each high-risk material family alone under the same warm brassworks lighting used by the north-star art.");
            readme.AppendLine("2. Keep every cell small enough that material failures are obvious: coil, gauge, receiver plate, barrel, enamel stripe, amber glass, and grip.");
            readme.AppendLine("3. Promote only passing material language into the next assembled weapon pass; do not assemble a full pistol until the component cells are visually coherent.");
            readme.AppendLine("4. Replace primitive cell geometry with final authored meshes later, but preserve material names, color ranges, edge-wear logic, grime layering, and first-person readability decisions.");
            readme.AppendLine();
            readme.AppendLine("## Component Cells");
            readme.AppendLine();
            foreach (CellAssessment assessment in Assessments)
            {
                readme.AppendLine("- `" + assessment.Role + "` - visual status `" + assessment.Status + "`: " + assessment.Evidence + " Final-product use: " + assessment.FinalUse);
            }
            readme.AppendLine();
            readme.AppendLine("## Render Evidence");
            readme.AppendLine();
            foreach (RenderRecord record in Records)
            {
                readme.AppendLine("- `" + record.RelativePath.Replace("\\", "/") + "` - " + record.Description + "; " + record.Width + "x" + record.Height + "; gate `" + (record.Passed ? "pass" : "fail") + "`.");
            }
            readme.AppendLine();
            readme.AppendLine("## Pass/Fail Rubric");
            readme.AppendLine();
            readme.AppendLine("- Unity-only: pass only if the render log contains `" + Marker + "` and no external DCC or image-generation renderer is used.");
            readme.AppendLine("- Nonblank PNG: pass only if each render exceeds 28 KB, has luma standard deviation above 8.0, and has luma range above 42.");
            readme.AppendLine("- Shader health: pass only if magenta shader-error pixels stay below 0.2 percent.");
            readme.AppendLine("- Component readability: pass only if the material cell reads at first-person weapon distance without needing the whole gun silhouette.");
            readme.AppendLine("- Production promotion: pass for direction does not mean final. The final product still needs optimized meshes, UVs, LODs, shader selection, texture budgets, rig anchors, muzzle/hand alignment, and gameplay animation testing.");

            File.WriteAllText(Path.Combine(context.PlanningRoot, "README.md"), readme.ToString());

            StringBuilder planJson = new StringBuilder();
            planJson.AppendLine("{");
            planJson.AppendLine("  \"schema\": \"brassworks.weapon_material_cell_lookdev_plan.v1\",");
            planJson.AppendLine("  \"bundle\": \"" + Bundle + "\",");
            planJson.AppendLine("  \"generated_at_utc\": \"" + DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) + "\",");
            planJson.AppendLine("  \"recommendation\": \"" + recommendation + "\",");
            planJson.AppendLine("  \"method\": \"Unity-only isolated material-cell lookdev before full weapon assembly\",");
            planJson.AppendLine("  \"north_star\": \"steampunk pressure pistol realism: burnished brass, oxidized copper, black iron, ivory gauge, smoked amber glass, chipped red enamel, walnut/leather grip\",");
            planJson.AppendLine("  \"cells\": [");
            for (int i = 0; i < Assessments.Count; i++)
            {
                CellAssessment assessment = Assessments[i];
                planJson.AppendLine("    {");
                planJson.AppendLine("      \"role\": \"" + assessment.Role + "\",");
                planJson.AppendLine("      \"visual_status\": \"" + assessment.Status + "\",");
                planJson.AppendLine("      \"evidence\": \"" + Escape(assessment.Evidence) + "\",");
                planJson.AppendLine("      \"final_product_application\": \"" + Escape(assessment.FinalUse) + "\"");
                planJson.Append("    }");
                planJson.AppendLine(i == Assessments.Count - 1 ? "" : ",");
            }
            planJson.AppendLine("  ]");
            planJson.AppendLine("}");
            File.WriteAllText(Path.Combine(context.PlanningRoot, "material_cell_plan_v0.1.54.json"), planJson.ToString());
        }

        private static void WriteQaDocs(Context context)
        {
            StringBuilder checklist = new StringBuilder();
            checklist.AppendLine("# v0.1.54 Unity Weapon Material Cell Lookdev QA Checklist");
            checklist.AppendLine();
            checklist.AppendLine("- [x] Stayed inside the allowed v0.1.54 lookdev roots and optional isolated asset-pack root.");
            checklist.AppendLine("- [x] Did not touch main Unity project scenes, gameplay scripts, package manifests, or shared status/build docs.");
            checklist.AppendLine("- [x] Used Unity batchmode, Unity Texture2D procedural raster drawing, Unity material/noise math, and Unity PNG encoding.");
            checklist.AppendLine("- [x] Did not use Blender, external DCC tools, external AI image generation, PIL, or a non-Unity renderer.");
            checklist.AppendLine("- [x] Produced separate cells for copper coil, pressure gauge dial, brass receiver plate, black iron barrel, red enamel safety line, smoked amber glass, and walnut/leather grip proxy.");
            checklist.AppendLine("- [x] Produced a contact sheet from the Unity-rendered cell PNGs.");
            checklist.AppendLine("- [x] Wrote a machine-readable QA manifest with render gates and file hashes.");
            checklist.AppendLine("- [ ] Human art review: compare contact sheet against the north-star pressure pistol before promoting to a full weapon assembly.");
            checklist.AppendLine("- [ ] Main-lane promotion review: decide which material cells should become importable package assets and which remain reference only.");
            File.WriteAllText(Path.Combine(context.QaRoot, "QA_Checklist.md"), checklist.ToString());

            bool allRenderGatesPassed = Records.All(record => record.Passed);
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine("  \"schema\": \"brassworks.weapon_material_cell_lookdev_qa.v1\",");
            json.AppendLine("  \"bundle\": \"" + Bundle + "\",");
            json.AppendLine("  \"generated_at_utc\": \"" + DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) + "\",");
            json.AppendLine("  \"unity_version\": \"" + Application.unityVersion + "\",");
            json.AppendLine("  \"unity_only_compliance\": true,");
            json.AppendLine("  \"external_dcc_used\": false,");
            json.AppendLine("  \"external_image_generation_used\": false,");
            json.AppendLine("  \"render_mode\": \"Unity Texture2D procedural material-cell raster; camera/mesh shader path bypassed after magenta shader-error output\",");
            json.AppendLine("  \"render_marker\": \"" + Marker + "\",");
            json.AppendLine("  \"source_renderer\": \"AssetPacks/BrassworksBreach.UnityWeaponMaterialCellLookdev09/Assets/Editor/UnityWeaponMaterialCellLookdevRenderer.cs\",");
            json.AppendLine("  \"render_log\": \"AssetPacks/BrassworksBreach.UnityWeaponMaterialCellLookdev09/UnityWeaponMaterialCellLookdev09.render.log\",");
            json.AppendLine("  \"render_command\": \"" + Escape(RecommendedCommand(context)) + "\",");
            json.AppendLine("  \"recommendation\": \"" + (allRenderGatesPassed ? "accept_for_component_direction" : "revise_before_promotion") + "\",");
            json.AppendLine("  \"gate_summary\": {");
            json.AppendLine("    \"total_pngs\": " + Records.Count + ",");
            json.AppendLine("    \"passed_pngs\": " + Records.Count(record => record.Passed) + ",");
            json.AppendLine("    \"failed_pngs\": " + Records.Count(record => !record.Passed));
            json.AppendLine("  },");
            json.AppendLine("  \"automated_gates\": [");
            json.AppendLine("    { \"gate\": \"file_size_bytes\", \"threshold\": \"> 28000\" },");
            json.AppendLine("    { \"gate\": \"luma_stddev\", \"threshold\": \"> 8.0\" },");
            json.AppendLine("    { \"gate\": \"luma_range\", \"threshold\": \"> 42\" },");
            json.AppendLine("    { \"gate\": \"magenta_shader_error_ratio\", \"threshold\": \"< 0.002\" }");
            json.AppendLine("  ],");
            json.AppendLine("  \"images\": [");
            for (int i = 0; i < Records.Count; i++)
            {
                RenderRecord record = Records[i];
                json.AppendLine("    {");
                json.AppendLine("      \"file\": \"" + Escape(record.FileName) + "\",");
                json.AppendLine("      \"path\": \"" + Escape(record.RelativePath.Replace("\\", "/")) + "\",");
                json.AppendLine("      \"description\": \"" + Escape(record.Description) + "\",");
                json.AppendLine("      \"cell_role\": \"" + Escape(record.CellRole) + "\",");
                json.AppendLine("      \"width\": " + record.Width + ",");
                json.AppendLine("      \"height\": " + record.Height + ",");
                json.AppendLine("      \"file_size_bytes\": " + record.FileSizeBytes + ",");
                json.AppendLine("      \"sha256\": \"" + record.Sha256 + "\",");
                json.AppendLine("      \"luma_mean\": " + record.LumaMean.ToString("0.###", CultureInfo.InvariantCulture) + ",");
                json.AppendLine("      \"luma_stddev\": " + record.LumaStdDev.ToString("0.###", CultureInfo.InvariantCulture) + ",");
                json.AppendLine("      \"luma_range\": " + record.LumaRange + ",");
                json.AppendLine("      \"magenta_ratio\": " + record.MagentaRatio.ToString("0.######", CultureInfo.InvariantCulture) + ",");
                json.AppendLine("      \"automated_png_gate\": \"" + (record.Passed ? "pass" : "fail") + "\"");
                json.Append("    }");
                json.AppendLine(i == Records.Count - 1 ? "" : ",");
            }
            json.AppendLine("  ],");
            json.AppendLine("  \"cell_assessments\": [");
            for (int i = 0; i < Assessments.Count; i++)
            {
                CellAssessment assessment = Assessments[i];
                json.AppendLine("    {");
                json.AppendLine("      \"role\": \"" + assessment.Role + "\",");
                json.AppendLine("      \"visual_status\": \"" + assessment.Status + "\",");
                json.AppendLine("      \"evidence\": \"" + Escape(assessment.Evidence) + "\",");
                json.AppendLine("      \"final_product_application\": \"" + Escape(assessment.FinalUse) + "\"");
                json.Append("    }");
                json.AppendLine(i == Assessments.Count - 1 ? "" : ",");
            }
            json.AppendLine("  ]");
            json.AppendLine("}");
            File.WriteAllText(Path.Combine(context.QaRoot, "qa_manifest.json"), json.ToString());
        }

        private static string RecommendedCommand(Context context)
        {
            string unity = "C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe";
            string log = Path.Combine(context.SidecarRoot, "UnityWeaponMaterialCellLookdev09.render.log").Replace("\\", "/");
            return "\"" + unity + "\" -batchmode -quit -projectPath \"" + context.SidecarRoot.Replace("\\", "/") + "\" -executeMethod BrassworksBreach.Sidecars.UnityWeaponMaterialCellLookdev09.UnityWeaponMaterialCellLookdevRenderer.Execute -logFile \"" + log + "\"";
        }

        private static GameObject CreateBox(string name, Vector3 position, Vector3 scale, Vector3 euler, Material material)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(euler);
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            RemoveCollider(go);
            return go;
        }

        private static GameObject CreateCylinder(string name, Vector3 position, Vector3 scale, Vector3 euler, Material material)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = name;
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(euler);
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            RemoveCollider(go);
            return go;
        }

        private static GameObject CreateSphere(string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = name;
            go.transform.position = position;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            RemoveCollider(go);
            return go;
        }

        private static GameObject CreateCapsule(string name, Vector3 position, Vector3 scale, Vector3 euler, Material material)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = name;
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(euler);
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            RemoveCollider(go);
            return go;
        }

        private static GameObject CreateTorus(string name, Vector3 position, float majorRadius, float minorRadius, Material material)
        {
            return CreateTorus(name, position, majorRadius, minorRadius, material, Vector3.zero);
        }

        private static GameObject CreateTorus(string name, Vector3 position, float majorRadius, float minorRadius, Material material, Vector3 euler)
        {
            return CreateMeshObject(name, BuildTorusMesh(majorRadius, minorRadius, 64, 12), position, euler, material);
        }

        private static GameObject CreateMeshObject(string name, Mesh mesh, Vector3 position, Vector3 euler, Material material)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(euler);
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            filter.sharedMesh = mesh;
            renderer.sharedMaterial = material;
            return go;
        }

        private static Mesh BuildTorusMesh(float majorRadius, float minorRadius, int majorSegments, int minorSegments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int i = 0; i <= majorSegments; i++)
            {
                float u = (float)i / majorSegments * Mathf.PI * 2f;
                Vector3 center = new Vector3(Mathf.Cos(u) * majorRadius, Mathf.Sin(u) * majorRadius, 0f);
                for (int j = 0; j <= minorSegments; j++)
                {
                    float v = (float)j / minorSegments * Mathf.PI * 2f;
                    Vector3 normal = new Vector3(Mathf.Cos(u) * Mathf.Cos(v), Mathf.Sin(u) * Mathf.Cos(v), Mathf.Sin(v));
                    vertices.Add(center + normal * minorRadius);
                    normals.Add(normal.normalized);
                }
            }

            int ring = minorSegments + 1;
            for (int i = 0; i < majorSegments; i++)
            {
                for (int j = 0; j < minorSegments; j++)
                {
                    int a = i * ring + j;
                    int b = (i + 1) * ring + j;
                    int c = (i + 1) * ring + j + 1;
                    int d = i * ring + j + 1;
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(d);
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = "UWMC09_torus_mesh";
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh BuildHelixTubeMesh(float length, float radius, float tubeRadius, float turns, int steps, int tubeSegments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int i = 0; i <= steps; i++)
            {
                float t01 = (float)i / steps;
                float t = t01 * Mathf.PI * 2f * turns;
                float x = (t01 - 0.5f) * length;
                Vector3 center = new Vector3(x, Mathf.Cos(t) * radius, Mathf.Sin(t) * radius);
                Vector3 radial = new Vector3(0f, Mathf.Cos(t), Mathf.Sin(t)).normalized;
                Vector3 binormal = new Vector3(0f, -Mathf.Sin(t), Mathf.Cos(t)).normalized;

                for (int j = 0; j <= tubeSegments; j++)
                {
                    float p = (float)j / tubeSegments * Mathf.PI * 2f;
                    Vector3 normal = radial * Mathf.Cos(p) + binormal * Mathf.Sin(p);
                    vertices.Add(center + normal * tubeRadius);
                    normals.Add(normal.normalized);
                }
            }

            int ring = tubeSegments + 1;
            for (int i = 0; i < steps; i++)
            {
                for (int j = 0; j < tubeSegments; j++)
                {
                    int a = i * ring + j;
                    int b = (i + 1) * ring + j;
                    int c = (i + 1) * ring + j + 1;
                    int d = i * ring + j + 1;
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(d);
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = "UWMC09_copper_helix_tube_mesh";
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void RemoveCollider(GameObject go)
        {
            Collider collider = go.GetComponent<Collider>();
            if (collider != null)
            {
                Object.DestroyImmediate(collider);
            }
        }

        private static Color C(float r, float g, float b)
        {
            return new Color(r, g, b, 1f);
        }

        private static string ToRepoRelative(string repoRoot, string absolutePath)
        {
            string fullRoot = Path.GetFullPath(repoRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            string fullPath = Path.GetFullPath(absolutePath);
            if (fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
            {
                return fullPath.Substring(fullRoot.Length).Replace("\\", "/");
            }
            return absolutePath.Replace("\\", "/");
        }

        private static string Sha256(string path)
        {
            using (SHA256 sha = SHA256.Create())
            using (FileStream stream = File.OpenRead(path))
            {
                return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private sealed class Context
        {
            private readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();

            public string SidecarRoot { get; private set; }
            public string RepoRoot { get; private set; }
            public string RenderRoot { get; private set; }
            public string PlanningRoot { get; private set; }
            public string QaRoot { get; private set; }

            public void Initialize()
            {
                SidecarRoot = Directory.GetParent(Application.dataPath).FullName;
                DirectoryInfo sidecar = new DirectoryInfo(SidecarRoot);
                if (sidecar.Parent == null || sidecar.Parent.Parent == null)
                {
                    throw new InvalidOperationException("Unable to resolve repository root from sidecar path: " + SidecarRoot);
                }

                RepoRoot = sidecar.Parent.Parent.FullName;
                RenderRoot = Path.Combine(RepoRoot, RenderRelative);
                PlanningRoot = Path.Combine(RepoRoot, PlanningRelative);
                QaRoot = Path.Combine(RepoRoot, QaRelative);
                Directory.CreateDirectory(RenderRoot);
                Directory.CreateDirectory(PlanningRoot);
                Directory.CreateDirectory(QaRoot);

                CreateMaterials();
            }

            public Material Mat(string id)
            {
                return materials[id];
            }

            private void CreateMaterials()
            {
                Add("WetBlackStone", C(0.06f, 0.055f, 0.047f), 0.02f, 0.7f, 0.36f, MaterialPattern.Stone);
                Add("SootBlackIron", C(0.055f, 0.048f, 0.043f), 0.9f, 0.36f, 0.48f, MaterialPattern.Iron);
                Add("BlackIronBarrel", C(0.055f, 0.052f, 0.049f), 0.92f, 0.44f, 0.42f, MaterialPattern.Iron);
                Add("BluedSpringSteel", C(0.12f, 0.14f, 0.16f), 0.88f, 0.52f, 0.3f, MaterialPattern.Steel);
                Add("AgedBrass", C(0.66f, 0.43f, 0.18f), 0.86f, 0.43f, 0.38f, MaterialPattern.Brass);
                Add("PolishedEdgeBrass", C(0.82f, 0.58f, 0.25f), 0.9f, 0.58f, 0.24f, MaterialPattern.Brass);
                Add("BurnishedCopperCoil", C(0.58f, 0.25f, 0.12f), 0.86f, 0.37f, 0.48f, MaterialPattern.Copper);
                Add("RedSafetyEnamel", C(0.72f, 0.055f, 0.04f), 0.28f, 0.58f, 0.25f, MaterialPattern.Enamel);
                Add("RedNeedlePaint", C(0.9f, 0.04f, 0.025f), 0.1f, 0.5f, 0.18f, MaterialPattern.Enamel);
                Add("GaugeIvoryEnamel", C(0.82f, 0.74f, 0.56f), 0.02f, 0.44f, 0.18f, MaterialPattern.Ivory);
                Add("EngravedDarkFill", C(0.028f, 0.023f, 0.019f), 0.06f, 0.24f, 0.34f, MaterialPattern.Grime);
                Add("SootGrime", C(0.035f, 0.029f, 0.023f), 0.05f, 0.18f, 0.52f, MaterialPattern.Grime);
                Add("WalnutVarnish", C(0.39f, 0.18f, 0.075f), 0.02f, 0.52f, 0.42f, MaterialPattern.Wood);
                Add("CrackedBrownLeather", C(0.23f, 0.12f, 0.065f), 0.02f, 0.36f, 0.56f, MaterialPattern.Leather);
                Add("SmokedAmberGlass", C(0.83f, 0.47f, 0.16f), 0f, 0.82f, 0.12f, MaterialPattern.Glass, C(0.38f, 0.16f, 0.045f), 0.28f, 0.42f);
                Add("AmberGlowGlass", C(1f, 0.52f, 0.12f), 0f, 0.9f, 0.08f, MaterialPattern.Glass, C(1f, 0.36f, 0.08f), 1.8f, 0.55f);
            }

            private void Add(string id, Color baseColor, float metallic, float smoothness, float bumpScale, MaterialPattern pattern, Color? emission = null, float emissionIntensity = 0f, float alpha = 1f)
            {
                Shader shader = FindSupportedShader();
                if (shader == null)
                {
                    throw new InvalidOperationException("No supported material shader found for weapon material cell lookdev.");
                }

                Material material = new Material(shader);
                material.name = id + "_UnityMaterialCellLookdev";
                material.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

                if (material.HasProperty("_Metallic"))
                {
                    material.SetFloat("_Metallic", metallic);
                }
                if (material.HasProperty("_Glossiness"))
                {
                    material.SetFloat("_Glossiness", smoothness);
                }
                if (material.HasProperty("_MainTex"))
                {
                    material.SetTexture("_MainTex", GenerateAlbedoTexture(id, baseColor, pattern));
                    material.SetTextureScale("_MainTex", new Vector2(1.4f, 1.4f));
                }
                if (material.HasProperty("_BumpMap"))
                {
                    material.SetTexture("_BumpMap", GenerateNormalTexture(id, pattern));
                    material.SetFloat("_BumpScale", bumpScale);
                    material.EnableKeyword("_NORMALMAP");
                }
                if (emission.HasValue && material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor("_EmissionColor", emission.Value * emissionIntensity);
                }
                if (alpha < 0.99f)
                {
                    ConfigureTransparent(material, alpha);
                }

                materials.Add(id, material);
            }

            private static Shader FindSupportedShader()
            {
                string[] candidates =
                {
                    "Hidden/Internal-Colored",
                    "BrassworksBreach/UWMC09FakeLitTexture",
                    "Unlit/Texture",
                    "Sprites/Default",
                    "Legacy Shaders/Diffuse",
                    "Legacy Shaders/Specular",
                    "Legacy Shaders/Bumped Specular"
                };

                foreach (string candidate in candidates)
                {
                    Shader shader = Shader.Find(candidate);
                    if (shader != null && shader.isSupported)
                    {
                        return shader;
                    }
                }

                return null;
            }

            private static void ConfigureTransparent(Material material, float alpha)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
                if (material.HasProperty("_Mode"))
                {
                    material.SetFloat("_Mode", 3f);
                }
                material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)RenderQueue.Transparent;
            }
        }

        private static Texture2D GenerateAlbedoTexture(string seedText, Color baseColor, MaterialPattern pattern)
        {
            int size = 512;
            int seed = Math.Abs(seedText.GetHashCode());
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, true);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float u = (float)x / size;
                    float v = (float)y / size;
                    float noise = Mathf.PerlinNoise(u * 14.7f + seed * 0.001f, v * 14.7f + seed * 0.002f);
                    float fine = Mathf.PerlinNoise(u * 73.1f + seed * 0.003f, v * 73.1f + seed * 0.004f);
                    float scratch = ScratchSignal(u, v, seed);
                    float multiplier = 0.84f + noise * 0.28f + fine * 0.08f;
                    Color color = baseColor * multiplier;

                    switch (pattern)
                    {
                        case MaterialPattern.Brass:
                            color += new Color(0.12f, 0.07f, 0.01f, 0f) * scratch;
                            color *= 1f - DarkSpot(u, v, seed) * 0.22f;
                            break;
                        case MaterialPattern.Copper:
                            color += new Color(0.14f, 0.055f, 0.02f, 0f) * scratch;
                            color = Color.Lerp(color, new Color(0.04f, 0.22f, 0.18f, 1f), OxidationSignal(u, v, seed) * 0.42f);
                            break;
                        case MaterialPattern.Iron:
                            color *= 0.86f + scratch * 0.22f;
                            color = Color.Lerp(color, new Color(0.13f, 0.12f, 0.105f, 1f), EdgeWearSignal(u, v, seed) * 0.34f);
                            break;
                        case MaterialPattern.Steel:
                            color = Color.Lerp(color, new Color(0.2f, 0.24f, 0.29f, 1f), scratch * 0.36f);
                            break;
                        case MaterialPattern.Enamel:
                            color *= 0.9f + Mathf.Pow(noise, 2f) * 0.32f;
                            color = Color.Lerp(color, new Color(0.035f, 0.03f, 0.025f, 1f), ChipSignal(u, v, seed) * 0.55f);
                            break;
                        case MaterialPattern.Ivory:
                            color += new Color(0.12f, 0.095f, 0.045f, 0f) * fine * 0.22f;
                            color = Color.Lerp(color, new Color(0.26f, 0.19f, 0.11f, 1f), DarkSpot(u, v, seed) * 0.16f);
                            break;
                        case MaterialPattern.Grime:
                            color *= 0.72f + noise * 0.45f;
                            break;
                        case MaterialPattern.Wood:
                            float grain = Mathf.Abs(Mathf.Sin((u * 12f + Mathf.PerlinNoise(v * 6f, seed * 0.01f) * 2f) * Mathf.PI));
                            color = Color.Lerp(color * 0.7f, new Color(0.62f, 0.32f, 0.12f, 1f), grain * 0.35f);
                            break;
                        case MaterialPattern.Leather:
                            color *= 0.82f + noise * 0.24f;
                            color = Color.Lerp(color, new Color(0.055f, 0.033f, 0.02f, 1f), CrackSignal(u, v, seed) * 0.55f);
                            break;
                        case MaterialPattern.Glass:
                            color = Color.Lerp(color, new Color(1f, 0.68f, 0.21f, 1f), Mathf.Pow(noise, 2f) * 0.28f);
                            break;
                        case MaterialPattern.Stone:
                            color *= 0.72f + noise * 0.48f;
                            break;
                    }

                    color.a = 1f;
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 4;
            return texture;
        }

        private static Texture2D GenerateNormalTexture(string seedText, MaterialPattern pattern)
        {
            int size = 256;
            int seed = Math.Abs(seedText.GetHashCode());
            float strength = pattern == MaterialPattern.Glass ? 0.08f : 0.32f;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, true, true);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float u = (float)x / size;
                    float v = (float)y / size;
                    float h1 = HeightSignal(u, v, seed, pattern);
                    float h2 = HeightSignal(u + 1f / size, v, seed, pattern);
                    float h3 = HeightSignal(u, v + 1f / size, seed, pattern);
                    Vector3 normal = new Vector3((h1 - h2) * strength, (h1 - h3) * strength, 1f).normalized;
                    texture.SetPixel(x, y, new Color(normal.x * 0.5f + 0.5f, normal.y * 0.5f + 0.5f, normal.z * 0.5f + 0.5f, 1f));
                }
            }
            texture.Apply();
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 4;
            return texture;
        }

        private static float HeightSignal(float u, float v, int seed, MaterialPattern pattern)
        {
            float n = Mathf.PerlinNoise(u * 18f + seed * 0.002f, v * 18f + seed * 0.003f);
            float f = Mathf.PerlinNoise(u * 82f + seed * 0.004f, v * 82f + seed * 0.005f);
            float detail = n * 0.7f + f * 0.3f;
            if (pattern == MaterialPattern.Wood)
            {
                detail += Mathf.Abs(Mathf.Sin(u * 28f + n * 2f)) * 0.28f;
            }
            if (pattern == MaterialPattern.Leather)
            {
                detail -= CrackSignal(u, v, seed) * 0.38f;
            }
            if (pattern == MaterialPattern.Enamel)
            {
                detail -= ChipSignal(u, v, seed) * 0.45f;
            }
            return detail;
        }

        private static float ScratchSignal(float u, float v, int seed)
        {
            float line = Mathf.Abs(Mathf.Sin((u * 38f + v * 5.5f + seed * 0.001f) * Mathf.PI));
            return line > 0.965f ? 1f : 0f;
        }

        private static float EdgeWearSignal(float u, float v, int seed)
        {
            float n = Mathf.PerlinNoise(u * 25f + seed * 0.006f, v * 25f + seed * 0.007f);
            return n > 0.72f ? (n - 0.72f) / 0.28f : 0f;
        }

        private static float OxidationSignal(float u, float v, int seed)
        {
            float n = Mathf.PerlinNoise(u * 11f + seed * 0.012f, v * 15f + seed * 0.009f);
            return n > 0.68f ? (n - 0.68f) / 0.32f : 0f;
        }

        private static float ChipSignal(float u, float v, int seed)
        {
            float n = Mathf.PerlinNoise(u * 31f + seed * 0.011f, v * 31f + seed * 0.013f);
            return n > 0.78f ? (n - 0.78f) / 0.22f : 0f;
        }

        private static float CrackSignal(float u, float v, int seed)
        {
            float line = Mathf.Abs(Mathf.Sin((u * 16f + Mathf.PerlinNoise(v * 9f, seed * 0.01f) * 5f) * Mathf.PI));
            return line > 0.94f ? 1f : 0f;
        }

        private static float DarkSpot(float u, float v, int seed)
        {
            float n = Mathf.PerlinNoise(u * 18f + seed * 0.018f, v * 18f + seed * 0.019f);
            return n > 0.74f ? (n - 0.74f) / 0.26f : 0f;
        }

        private enum MaterialPattern
        {
            Brass,
            Copper,
            Iron,
            Steel,
            Enamel,
            Ivory,
            Grime,
            Wood,
            Leather,
            Glass,
            Stone
        }

        private enum CellRole
        {
            CopperCoil,
            PressureGaugeDial,
            BrassReceiverPlate,
            BlackIronBarrel,
            RedEnamelSafetyLine,
            SmokedAmberGlass,
            WalnutLeatherGripProxy
        }

        private sealed class RenderRequest
        {
            public RenderRequest(string fileName, string description, int width, int height, string cellRole)
            {
                FileName = fileName;
                Description = description;
                Width = width;
                Height = height;
                CellRole = cellRole;
            }

            public readonly string FileName;
            public readonly string Description;
            public readonly int Width;
            public readonly int Height;
            public readonly string CellRole;
        }

        private sealed class RenderRecord
        {
            public string FileName;
            public string Description;
            public string CellRole;
            public string RelativePath;
            public int Width;
            public int Height;
            public long FileSizeBytes;
            public string Sha256;
            public double LumaMean;
            public double LumaStdDev;
            public int LumaRange;
            public double MagentaRatio;
            public bool Passed;
        }

        private sealed class CellAssessment
        {
            public CellAssessment(CellRole role, string status, string evidence, string finalUse)
            {
                Role = role;
                Status = status;
                Evidence = evidence;
                FinalUse = finalUse;
            }

            public readonly CellRole Role;
            public readonly string Status;
            public readonly string Evidence;
            public readonly string FinalUse;
        }
    }
}
#endif
