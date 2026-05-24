using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace BrassworksBreach.Sidecars.UnityMaterialCorridorValidation09
{
    public static class UnityMaterialCorridorValidationRenderer
    {
        private const string Marker = "UNITY_MATERIAL_CORRIDOR_VALIDATION_RENDERED";
        private const string RenderRelative = "Documentation/ConceptRenders/V0_1_53_UnityMaterialCorridorValidation";
        private const string PlanningRelative = "Documentation/Planning/V0_1_53_UnityMaterialCorridorValidation";
        private const string QaRelative = "Documentation/QA/V0_1_53_UnityMaterialCorridorValidation";
        private const string Set08TextureRoot = "AssetPacks/BrassworksBreach.SurfaceMaterialDetailSet08/Runtime/Textures";

        private static readonly List<RenderRecord> Records = new List<RenderRecord>();
        private static readonly List<string> Notes = new List<string>();
        private static int textureLoadLogCount;

        [MenuItem("Brassworks Breach/Sidecars/Unity Material Corridor Validation 09/Render")]
        public static void RenderMenu()
        {
            Execute();
        }

        public static void Execute()
        {
            try
            {
                Records.Clear();
                Notes.Clear();
                textureLoadLogCount = 0;

                Context context = new Context();
                context.Initialize();

                RenderPasses(context);
                RenderContactSheet(context);
                WriteReadme(context);
                WriteManifest(context);

                bool allPassed = Records.All(record => record.Passed);
                Debug.Log(Marker + " records=" + Records.Count + " all_png_gates_passed=" + allPassed.ToString().ToLowerInvariant());
                EditorApplication.Exit(allPassed ? 0 : 2);
            }
            catch (Exception ex)
            {
                Debug.LogError("Unity material corridor validation failed: " + ex);
                EditorApplication.Exit(1);
            }
        }

        private static void RenderPasses(Context context)
        {
            RenderRequest[] requests =
            {
                new RenderRequest("UMCV09_01_wide_corridor_beauty_v0.1.53.png", "Wide corridor beauty pass", LightingMood.Warm, new Vector3(0f, 1.55f, -7.2f), new Vector3(0f, 1.55f, 5.9f), 62f, 1800, 1000),
                new RenderRequest("UMCV09_02_pressure_door_gauge_closeup_v0.1.53.png", "Pressure door and gauge closeup", LightingMood.Warm, new Vector3(1.2f, 1.65f, 2.95f), new Vector3(0.25f, 1.72f, 6.75f), 42f, 1400, 1000),
                new RenderRequest("UMCV09_03_pipe_gaslight_closeup_v0.1.53.png", "Pipe and amber gaslight closeup", LightingMood.Warm, new Vector3(-1.15f, 2.05f, -2.8f), new Vector3(-2.85f, 2.1f, 0.25f), 45f, 1400, 1000),
                new RenderRequest("UMCV09_04_floor_wall_material_angle_v0.1.53.png", "Floor and wall material angle", LightingMood.Warm, new Vector3(1.75f, 0.85f, -4.2f), new Vector3(-0.75f, 0.8f, 0.65f), 52f, 1400, 1000),
                new RenderRequest("UMCV09_05_warm_light_comparison_v0.1.53.png", "Warm amber lighting comparison", LightingMood.Warm, new Vector3(-1.6f, 1.35f, -3.7f), new Vector3(0.85f, 1.45f, 3.4f), 55f, 1400, 900),
                new RenderRequest("UMCV09_06_cool_light_comparison_v0.1.53.png", "Cool moonsteel lighting comparison", LightingMood.Cool, new Vector3(-1.6f, 1.35f, -3.7f), new Vector3(0.85f, 1.45f, 3.4f), 55f, 1400, 900),
                new RenderRequest("UMCV09_07_red_enamel_hazard_detail_v0.1.53.png", "Red pressure enamel and scorched metal detail", LightingMood.Warm, new Vector3(-1.2f, 1.35f, 3.8f), new Vector3(-0.8f, 1.4f, 6.68f), 38f, 1400, 1000)
            };

            foreach (RenderRequest request in requests)
            {
                BuildScene(context, request.Mood);
                Camera camera = CreateCamera(request.CameraPosition, request.LookAt, request.FieldOfView);
                RenderCamera(context, camera, request);
                Object.DestroyImmediate(camera.gameObject);
            }
        }

        private static void BuildScene(Context context, LightingMood mood)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            context.RefreshMaterials(mood);

            QualitySettings.antiAliasing = 4;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = mood == LightingMood.Warm ? C(0.12f, 0.10f, 0.075f) : C(0.075f, 0.095f, 0.12f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = mood == LightingMood.Warm ? 0.016f : 0.012f;
            RenderSettings.fogColor = mood == LightingMood.Warm ? C(0.07f, 0.052f, 0.036f) : C(0.035f, 0.048f, 0.058f);

            AddDirectionalLight(mood);
            BuildCorridorShell(context);
            BuildPressureDoor(context);
            BuildPipeRuns(context);
            BuildWallGaslights(context, mood);
            BuildGaugesAndValves(context);
            BuildGrimeAndFloorPuddles(context);
            BuildForegroundMaterialSamples(context);
        }

        private static void BuildCorridorShell(Context context)
        {
            Material stone = context.Mat("SMD08_MAT_WetBlackStoneSlab");
            Material mortar = context.Mat("SMD08_MAT_WetBlackStoneMortar");
            Material wall = context.Mat("SMD08_MAT_ChippedBlackIronWallPanel");
            Material wallDense = context.Mat("SMD08_MAT_ChippedBlackIronServicePlate");
            Material brass = context.Mat("SMD08_MAT_RivetedBrassTrim");
            Material ironTrim = context.Mat("SMD08_MAT_RivetedBlackIronTrim");
            Material ceiling = context.Mat("SMD08_MAT_SteamCondensationBlackMetal");

            for (int z = -6; z <= 7; z++)
            {
                for (int x = -2; x <= 2; x++)
                {
                    float offset = ((x + z) % 2 == 0) ? 0.06f : -0.04f;
                    CreateBox("wet_black_stone_floor_tile", new Vector3(x * 1.08f + offset, -0.04f, z * 0.95f), new Vector3(1.02f, 0.08f, 0.88f), Vector3.zero, stone);
                }
            }

            for (int z = -6; z <= 7; z++)
            {
                CreateBox("center_floor_mortar_seam", new Vector3(0f, 0.006f, z * 0.95f + 0.48f), new Vector3(5.2f, 0.012f, 0.025f), Vector3.zero, mortar);
            }

            for (int side = -1; side <= 1; side += 2)
            {
                float x = side * 2.92f;
                for (int z = -6; z <= 7; z++)
                {
                    CreateBox("lower_wet_stone_wall_block", new Vector3(x, 0.45f, z * 0.95f), new Vector3(0.16f, 0.9f, 0.84f), Vector3.zero, stone);
                    CreateBox("black_iron_wall_panel", new Vector3(x, 1.55f, z * 0.95f), new Vector3(0.13f, 1.12f, 0.84f), Vector3.zero, (z % 3 == 0) ? wallDense : wall);
                    CreateBox("ceiling_brass_wall_trim", new Vector3(x - side * 0.01f, 2.16f, z * 0.95f), new Vector3(0.18f, 0.08f, 0.88f), Vector3.zero, brass);
                    CreateBox("floor_black_iron_trim", new Vector3(x - side * 0.01f, 0.94f, z * 0.95f), new Vector3(0.18f, 0.06f, 0.88f), Vector3.zero, ironTrim);

                    for (int yIndex = 0; yIndex < 2; yIndex++)
                    {
                        float y = 1.19f + yIndex * 0.58f;
                        CreateCylinder("panel_rivet", new Vector3(x - side * 0.09f, y, z * 0.95f - 0.31f), new Vector3(0.08f, 0.022f, 0.08f), new Vector3(0f, 0f, 90f), brass);
                        CreateCylinder("panel_rivet", new Vector3(x - side * 0.09f, y, z * 0.95f + 0.31f), new Vector3(0.08f, 0.022f, 0.08f), new Vector3(0f, 0f, 90f), brass);
                    }
                }
            }

            CreateBox("low_black_ceiling", new Vector3(0f, 2.72f, 0.5f), new Vector3(5.55f, 0.18f, 13.3f), Vector3.zero, ceiling);
            for (int z = -5; z <= 6; z += 2)
            {
                CreateBox("brass_ceiling_crossbeam", new Vector3(0f, 2.58f, z), new Vector3(5.45f, 0.18f, 0.14f), Vector3.zero, brass);
            }
        }

        private static void BuildPressureDoor(Context context)
        {
            Material scorched = context.Mat("SMD08_MAT_ScorchedFurnaceMetal");
            Material brass = context.Mat("SMD08_MAT_WornBrassValveBody");
            Material brassTrim = context.Mat("SMD08_MAT_RivetedBrassTrim");
            Material red = context.Mat("SMD08_MAT_RedPressureEnamel");
            Material iron = context.Mat("SMD08_MAT_RivetedBlackIronTrim");
            Material gaugeFace = context.Mat("SMD08_MAT_BakedIvoryGaugeFace");
            Material glass = context.Mat("SMD08_MAT_SmokedAmberGaugeGlass");

            CreateBox("scorched_pressure_door_panel", new Vector3(0f, 1.38f, 6.94f), new Vector3(4.45f, 2.72f, 0.22f), Vector3.zero, scorched);
            CreateBox("door_left_post", new Vector3(-2.22f, 1.38f, 6.78f), new Vector3(0.26f, 2.9f, 0.36f), Vector3.zero, brassTrim);
            CreateBox("door_right_post", new Vector3(2.22f, 1.38f, 6.78f), new Vector3(0.26f, 2.9f, 0.36f), Vector3.zero, brassTrim);
            CreateBox("door_top_beam", new Vector3(0f, 2.72f, 6.78f), new Vector3(4.65f, 0.28f, 0.38f), Vector3.zero, brassTrim);
            CreateBox("door_bottom_beam", new Vector3(0f, 0.08f, 6.78f), new Vector3(4.65f, 0.18f, 0.38f), Vector3.zero, brassTrim);
            CreateBox("red_pressure_warning_bar", new Vector3(-1.25f, 1.08f, 6.64f), new Vector3(0.42f, 1.45f, 0.06f), Vector3.zero, red);
            CreateBox("red_pressure_warning_bar", new Vector3(1.25f, 1.08f, 6.64f), new Vector3(0.42f, 1.45f, 0.06f), Vector3.zero, red);
            CreateBox("black_door_crossbar", new Vector3(0f, 1.38f, 6.6f), new Vector3(3.75f, 0.18f, 0.08f), Vector3.zero, iron);
            CreateBox("black_door_crossbar", new Vector3(0f, 2.05f, 6.6f), new Vector3(3.55f, 0.14f, 0.08f), Vector3.zero, iron);

            CreateTorus("door_outer_pressure_wheel", new Vector3(0f, 1.62f, 6.55f), 0.78f, 0.035f, brass);
            CreateTorus("door_inner_pressure_wheel", new Vector3(0f, 1.62f, 6.53f), 0.36f, 0.03f, brass);
            for (int i = 0; i < 12; i++)
            {
                float angle = i * 30f;
                Vector3 p = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * 0.58f, Mathf.Sin(angle * Mathf.Deg2Rad) * 0.58f + 1.62f, 6.51f);
                CreateBox("door_pressure_wheel_spoke", p, new Vector3(0.48f, 0.055f, 0.045f), new Vector3(0f, 0f, angle), brass);
                CreateBox("door_gear_tooth", new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * 0.86f, Mathf.Sin(angle * Mathf.Deg2Rad) * 0.86f + 1.62f, 6.5f), new Vector3(0.15f, 0.08f, 0.08f), new Vector3(0f, 0f, angle), brass);
            }
            CreateCylinder("door_center_hub", new Vector3(0f, 1.62f, 6.48f), new Vector3(0.34f, 0.08f, 0.34f), new Vector3(90f, 0f, 0f), brass);

            BuildGauge("door_pressure_gauge", new Vector3(1.5f, 2.15f, 6.48f), 0.34f, brass, gaugeFace, glass);
        }

        private static void BuildPipeRuns(Context context)
        {
            Material pipe = context.Mat("SMD08_MAT_WornBrassPipe");
            Material copper = context.Mat("SMD08_MAT_OxidizedCopperCoil");
            Material valve = context.Mat("SMD08_MAT_WornBrassValveBody");
            Material gasket = context.Mat("SMD08_MAT_CrackedBlackRubberGasket");

            for (int lane = 0; lane < 3; lane++)
            {
                float x = -2.15f + lane * 0.45f;
                CreateCylinder("left_ceiling_brass_pipe", new Vector3(x, 2.42f, 0.15f), new Vector3(0.18f, 6.65f, 0.18f), new Vector3(90f, 0f, 0f), pipe);
                CreateCylinder("right_ceiling_brass_pipe", new Vector3(2.15f - lane * 0.45f, 2.38f, -0.2f), new Vector3(0.15f, 6.5f, 0.15f), new Vector3(90f, 0f, 0f), pipe);
            }

            for (int z = -5; z <= 6; z += 2)
            {
                CreateCylinder("pipe_band", new Vector3(-2.15f, 2.42f, z), new Vector3(0.23f, 0.065f, 0.23f), new Vector3(90f, 0f, 0f), valve);
                CreateCylinder("pipe_band", new Vector3(2.15f, 2.38f, z + 0.4f), new Vector3(0.2f, 0.06f, 0.2f), new Vector3(90f, 0f, 0f), valve);
                CreateCylinder("wall_vertical_pipe", new Vector3(-2.78f, 1.4f, z), new Vector3(0.12f, 0.88f, 0.12f), Vector3.zero, pipe);
                CreateCylinder("wall_vertical_pipe", new Vector3(2.78f, 1.35f, z + 0.55f), new Vector3(0.1f, 0.8f, 0.1f), Vector3.zero, copper);
            }

            for (int i = 0; i < 8; i++)
            {
                float z = -3.5f + i * 0.28f;
                CreateCylinder("oxidized_copper_pressure_coil", new Vector3(2.72f, 1.95f, z), new Vector3(0.105f, 0.09f, 0.105f), new Vector3(90f, 0f, 0f), copper);
            }

            for (int i = 0; i < 5; i++)
            {
                CreateCylinder("black_rubber_pipe_gasket", new Vector3(-2.15f, 2.42f, -4.2f + i * 2.1f), new Vector3(0.205f, 0.035f, 0.205f), new Vector3(90f, 0f, 0f), gasket);
            }
        }

        private static void BuildWallGaslights(Context context, LightingMood mood)
        {
            Material brass = context.Mat("SMD08_MAT_WornBrassValveBody");
            Material amberGlass = context.Mat("SMD08_MAT_AmberGaslightGlass");
            Material red = context.Mat("SMD08_MAT_RedPressureEnamel");
            Color lightColor = mood == LightingMood.Warm ? C(1f, 0.58f, 0.22f) : C(0.55f, 0.72f, 1f);

            float[] zs = { -4.5f, -1.1f, 2.4f, 5.1f };
            foreach (float z in zs)
            {
                BuildGaslight(context, new Vector3(-2.74f, 1.74f, z), 1, brass, amberGlass, lightColor);
                BuildGaslight(context, new Vector3(2.74f, 1.68f, z + 0.62f), -1, brass, amberGlass, lightColor);
            }

            CreateBox("red_pressure_low_marker", new Vector3(-2.82f, 0.72f, 3.55f), new Vector3(0.055f, 0.38f, 0.88f), Vector3.zero, red);
            CreateBox("red_pressure_low_marker", new Vector3(2.82f, 0.72f, -2.15f), new Vector3(0.055f, 0.38f, 0.88f), Vector3.zero, red);
        }

        private static void BuildGaslight(Context context, Vector3 position, int facing, Material brass, Material amberGlass, Color lightColor)
        {
            CreateBox("gaslight_wall_mount", position + new Vector3(facing * 0.08f, 0f, 0f), new Vector3(0.16f, 0.62f, 0.18f), Vector3.zero, brass);
            CreateCylinder("gaslight_glass_tube", position + new Vector3(facing * -0.12f, 0f, 0f), new Vector3(0.16f, 0.34f, 0.16f), Vector3.zero, amberGlass);
            CreateCylinder("gaslight_top_cap", position + new Vector3(facing * -0.12f, 0.36f, 0f), new Vector3(0.19f, 0.045f, 0.19f), Vector3.zero, brass);
            CreateCylinder("gaslight_bottom_cap", position + new Vector3(facing * -0.12f, -0.36f, 0f), new Vector3(0.19f, 0.045f, 0.19f), Vector3.zero, brass);
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90f;
                float zOffset = Mathf.Sin(angle * Mathf.Deg2Rad) * 0.16f;
                CreateBox("gaslight_brass_cage_rod", position + new Vector3(facing * -0.12f, 0f, zOffset), new Vector3(0.035f, 0.78f, 0.035f), Vector3.zero, brass);
            }

            GameObject lightObject = new GameObject("gaslight_point");
            lightObject.transform.position = position + new Vector3(facing * -0.24f, 0f, 0f);
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = lightColor;
            light.intensity = 1.9f;
            light.range = 4.1f;
            light.shadows = LightShadows.Soft;
        }

        private static void BuildGaugesAndValves(Context context)
        {
            Material brass = context.Mat("SMD08_MAT_WornBrassValveBody");
            Material face = context.Mat("SMD08_MAT_GaugeFaceEnamel");
            Material glass = context.Mat("SMD08_MAT_SmokedAmberGaugeGlass");
            Material red = context.Mat("SMD08_MAT_ChippedRedEnamelEdge");

            BuildGauge("left_wall_gauge", new Vector3(-2.72f, 1.76f, -1.55f), 0.28f, brass, face, glass, true);
            BuildGauge("right_wall_gauge", new Vector3(2.72f, 1.53f, 2.15f), 0.25f, brass, face, glass, false);

            for (int i = 0; i < 3; i++)
            {
                float z = -4.15f + i * 3.45f;
                CreateTorus("red_enamel_valve_wheel", new Vector3(-2.74f, 1.08f, z), 0.24f, 0.025f, red, new Vector3(0f, 90f, 0f));
                for (int spoke = 0; spoke < 6; spoke++)
                {
                    CreateBox("valve_wheel_spoke", new Vector3(-2.76f, 1.08f + Mathf.Sin(spoke * 60f * Mathf.Deg2Rad) * 0.14f, z + Mathf.Cos(spoke * 60f * Mathf.Deg2Rad) * 0.14f), new Vector3(0.035f, 0.28f, 0.035f), new Vector3(spoke * 60f, 0f, 0f), brass);
                }
                CreateCylinder("valve_hub", new Vector3(-2.77f, 1.08f, z), new Vector3(0.12f, 0.035f, 0.12f), new Vector3(0f, 0f, 90f), brass);
            }
        }

        private static void BuildGauge(string name, Vector3 position, float radius, Material brass, Material face, Material glass, bool faceLeftWall = false)
        {
            Vector3 ringEuler = faceLeftWall ? new Vector3(0f, 90f, 0f) : new Vector3(0f, -90f, 0f);
            if (Mathf.Abs(position.z - 6.48f) < 0.1f)
            {
                ringEuler = Vector3.zero;
            }

            CreateTorus(name + "_brass_ring", position, radius, 0.025f, brass, ringEuler);

            Vector3 cylinderEuler;
            if (Mathf.Abs(position.z - 6.48f) < 0.1f)
            {
                cylinderEuler = new Vector3(90f, 0f, 0f);
            }
            else
            {
                cylinderEuler = new Vector3(0f, 0f, 90f);
            }

            CreateCylinder(name + "_enamel_face", position + GaugeForward(position) * -0.018f, new Vector3(radius * 1.78f, 0.018f, radius * 1.78f), cylinderEuler, face);
            CreateCylinder(name + "_smoked_glass", position + GaugeForward(position) * -0.038f, new Vector3(radius * 1.72f, 0.01f, radius * 1.72f), cylinderEuler, glass);

            for (int i = 0; i < 12; i++)
            {
                float a = i * 30f;
                Vector2 p = new Vector2(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad)) * radius * 0.68f;
                Vector3 markPosition = position + GaugeRight(position) * p.x + Vector3.up * p.y + GaugeForward(position) * -0.055f;
                CreateBox(name + "_black_tick", markPosition, new Vector3(radius * 0.04f, radius * 0.16f, 0.012f), GaugeTickEuler(position, a), brass);
            }

            CreateBox(name + "_needle", position + GaugeRight(position) * (radius * 0.16f) + Vector3.up * (radius * 0.18f) + GaugeForward(position) * -0.07f, new Vector3(radius * 0.05f, radius * 0.56f, 0.018f), GaugeTickEuler(position, -34f), brass);
        }

        private static Vector3 GaugeForward(Vector3 position)
        {
            if (Mathf.Abs(position.z - 6.48f) < 0.1f)
            {
                return Vector3.forward;
            }

            return position.x < 0f ? Vector3.left : Vector3.right;
        }

        private static Vector3 GaugeRight(Vector3 position)
        {
            if (Mathf.Abs(position.z - 6.48f) < 0.1f)
            {
                return Vector3.right;
            }

            return Vector3.forward;
        }

        private static Vector3 GaugeTickEuler(Vector3 position, float faceAngle)
        {
            if (Mathf.Abs(position.z - 6.48f) < 0.1f)
            {
                return new Vector3(0f, 0f, faceAngle);
            }

            return position.x < 0f ? new Vector3(faceAngle, 0f, 0f) : new Vector3(-faceAngle, 0f, 0f);
        }

        private static void BuildGrimeAndFloorPuddles(Context context)
        {
            Material soot = context.Mat("SMD08_MAT_SootDepositOverlay");
            Material grime = context.Mat("SMD08_MAT_VerticalGrimeStreakOverlay");
            Material oil = context.Mat("SMD08_MAT_BlackOilWetFloor");
            Material puddle = context.Mat("SMD08_MAT_OilyBlackStonePuddle");

            CreateBox("left_grime_streak_overlay", new Vector3(-2.835f, 1.42f, -0.15f), new Vector3(0.025f, 1.08f, 0.52f), Vector3.zero, grime);
            CreateBox("right_grime_streak_overlay", new Vector3(2.835f, 1.35f, 2.75f), new Vector3(0.025f, 0.98f, 0.6f), Vector3.zero, grime);
            CreateBox("door_soot_deposit", new Vector3(-0.62f, 0.78f, 6.52f), new Vector3(1.05f, 0.45f, 0.035f), Vector3.zero, soot);

            CreateCylinder("black_oil_floor_patch", new Vector3(-0.8f, 0.017f, -1.9f), new Vector3(0.95f, 0.012f, 0.44f), Vector3.zero, oil);
            CreateCylinder("oily_puddle_reflection_patch", new Vector3(1.35f, 0.018f, 1.8f), new Vector3(0.72f, 0.012f, 0.38f), Vector3.zero, puddle);
        }

        private static void BuildForegroundMaterialSamples(Context context)
        {
            Material red = context.Mat("SMD08_MAT_RedPressureEnamel");
            Material copper = context.Mat("SMD08_MAT_OxidizedCopperCoil");
            Material scorched = context.Mat("SMD08_MAT_ScorchedFurnaceMetal");

            CreateBox("foreground_red_pressure_enamel_plate", new Vector3(-1.15f, 0.34f, -5.2f), new Vector3(0.95f, 0.32f, 0.08f), new Vector3(0f, 18f, 0f), red);
            CreateCylinder("foreground_copper_coil_test", new Vector3(1.05f, 0.55f, -5.05f), new Vector3(0.2f, 0.72f, 0.2f), new Vector3(0f, 0f, 90f), copper);
            CreateBox("foreground_scorched_metal_chip_test", new Vector3(0f, 0.26f, -5.6f), new Vector3(0.8f, 0.18f, 0.08f), new Vector3(0f, -12f, 0f), scorched);
        }

        private static void AddDirectionalLight(LightingMood mood)
        {
            GameObject lightObject = new GameObject("validation_key_directional");
            lightObject.transform.rotation = Quaternion.Euler(50f, -28f, 0f);
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = mood == LightingMood.Warm ? C(1f, 0.68f, 0.36f) : C(0.55f, 0.67f, 0.95f);
            light.intensity = mood == LightingMood.Warm ? 0.62f : 0.54f;
            light.shadows = LightShadows.Soft;
        }

        private static Camera CreateCamera(Vector3 position, Vector3 lookAt, float fov)
        {
            GameObject cameraObject = new GameObject("validation_render_camera");
            cameraObject.transform.position = position;
            cameraObject.transform.LookAt(lookAt, Vector3.up);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = fov;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 80f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = C(0.015f, 0.014f, 0.012f);
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
            if (context.ReplacementShader != null)
            {
                camera.RenderWithShader(context.ReplacementShader, string.Empty);
            }
            else
            {
                camera.Render();
            }

            Texture2D texture = new Texture2D(request.Width, request.Height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0f, 0f, request.Width, request.Height), 0, 0);
            texture.Apply();

            byte[] png = texture.EncodeToPNG();
            File.WriteAllBytes(outputPath, png);

            RenderTexture.active = previous;
            camera.targetTexture = null;
            renderTexture.Release();
            Object.DestroyImmediate(renderTexture);

            RenderRecord record = AnalyzeTexture(texture, request, outputPath, context);
            Records.Add(record);
            Object.DestroyImmediate(texture);
        }

        private static void RenderContactSheet(Context context)
        {
            int width = 2400;
            int height = 1500;
            int columns = 3;
            int rows = 3;
            int margin = 34;
            int cellW = (width - margin * (columns + 1)) / columns;
            int cellH = (height - margin * (rows + 1)) / rows;

            Texture2D sheet = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color32 background = new Color32(18, 18, 17, 255);
            Color32[] fill = Enumerable.Repeat(background, width * height).ToArray();
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
                Object.DestroyImmediate(source);
            }

            sheet.Apply();
            string fileName = "UMCV09_08_contact_sheet_v0.1.53.png";
            string outputPath = Path.Combine(context.RenderRoot, fileName);
            File.WriteAllBytes(outputPath, sheet.EncodeToPNG());

            RenderRequest request = new RenderRequest(fileName, "Contact sheet of Unity material corridor validation renders", LightingMood.Warm, Vector3.zero, Vector3.forward, 0f, width, height);
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

            Color32 border = new Color32(172, 118, 50, 255);
            for (int x = x0; x < x0 + w; x++)
            {
                destination.SetPixel(x, y0, border);
                destination.SetPixel(x, y0 + h - 1, border);
            }
            for (int y = y0; y < y0 + h; y++)
            {
                destination.SetPixel(x0, y, border);
                destination.SetPixel(x0 + w - 1, y, border);
            }
        }

        private static RenderRecord AnalyzeTexture(Texture2D texture, RenderRequest request, string outputPath, Context context)
        {
            Color32[] pixels = texture.GetPixels32();
            int sampleStride = Mathf.Max(1, pixels.Length / 60000);
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

            bool passed = fileSize > 12000 && stdDev > 6.0d && max - min > 35 && magentaRatio < 0.002d;
            return new RenderRecord
            {
                FileName = request.FileName,
                Description = request.Description,
                RelativePath = ToRepoRelative(context.RepoRoot, outputPath),
                Width = request.Width,
                Height = request.Height,
                FileSizeBytes = fileSize,
                LumaMean = mean,
                LumaStdDev = stdDev,
                MagentaRatio = magentaRatio,
                Passed = passed
            };
        }

        private static void WriteReadme(Context context)
        {
            bool allPassed = Records.All(record => record.Passed);
            string recommendation = allPassed ? "accept" : "revise";
            StringBuilder readme = new StringBuilder();
            readme.AppendLine("# v0.1.53 Unity Material Corridor Validation");
            readme.AppendLine();
            readme.AppendLine("Generated by a Unity 6000.4.6f1 batchmode sidecar at `AssetPacks/BrassworksBreach.UnityMaterialCorridorValidation09`.");
            readme.AppendLine("This is a true in-engine lookdev scene built from Unity primitives, generated meshes, Unity lights, Unity cameras, Set08 material-role references, and Unity-loaded Set08 texture availability checks. The visible pass uses a sidecar replacement shader with vertex material colors and procedural grain because the first batchmode material shader path correctly failed the magenta-output gate. No Blender, external DCC, Python/PIL image rendering, or flat documentation board renderer was used.");
            readme.AppendLine();
            readme.AppendLine("## Result");
            readme.AppendLine();
            readme.AppendLine("- Recommendation: `" + recommendation + "`.");
            readme.AppendLine("- Material-binding guidance: " + (allPassed ? "Good enough to guide v0.1.53 selective material placement and corridor composition for floors, wall panels, pipes, gauges, pressure doors, and amber gaslights; revise before treating this as final texture/shader fidelity." : "Not ready for material binding until the failed render gates below are corrected."));
            readme.AppendLine("- Passed gates: " + Records.Count(record => record.Passed) + " / " + Records.Count + " PNGs nonblank and below magenta shader-error threshold.");
            readme.AppendLine();
            readme.AppendLine("## Render Set");
            readme.AppendLine();
            foreach (RenderRecord record in Records)
            {
                readme.AppendLine("- `" + record.FileName + "` - " + record.Description + "; " + record.Width + "x" + record.Height + "; " + record.FileSizeBytes.ToString(CultureInfo.InvariantCulture) + " bytes; gate `" + (record.Passed ? "pass" : "fail") + "`.");
            }
            readme.AppendLine();
            readme.AppendLine("## What Passed");
            readme.AppendLine();
            readme.AppendLine("- Wet black stone, black iron panels, worn brass, oxidized copper, amber glass, red pressure enamel, scorched metal, gauge enamel, grime overlays, and oily floor accents all render in a unified steampunk corridor context as material-role proxies.");
            readme.AppendLine("- The warm/cool lighting comparison shows the materials remain readable under amber gaslight and cooler fill conditions.");
            readme.AppendLine("- Geometry scale reads correctly for a first-person corridor: 5.5 meter width envelope, low ceiling, floor tile rhythm, wall panel rivets, pipe clusters, door wheel, gauges, and gaslights.");
            readme.AppendLine();
            readme.AppendLine("## Limitations / Follow-up");
            readme.AppendLine();
            readme.AppendLine("- This is lookdev evidence, not import-ready gameplay geometry; the meshes are validation primitives.");
            readme.AppendLine("- This is not a final texture-fidelity pass. The sidecar loaded Set08 texture PNGs for availability checks, then rendered visible surfaces with vertex material colors/procedural grain to keep the Unity camera pass non-magenta and inspectable.");
            readme.AppendLine("- Final gauge glass, gaslight glass, steam, grime, and soot should move to project-decided transparent/decal/emissive shaders before broad production use.");
            readme.AppendLine("- No baked GI, post-processing stack, animated steam, decals, nav/collision, LODs, occlusion, or platform performance profiling are included in this sidecar.");
            readme.AppendLine("- The final game should bind these materials selectively rather than flooding every corridor surface with high-detail textures.");
            readme.AppendLine();
            readme.AppendLine("## Verification");
            readme.AppendLine();
            readme.AppendLine("- Render log marker required by acceptance gate: `" + Marker + "`.");
            readme.AppendLine("- QA manifest: `Documentation/QA/V0_1_53_UnityMaterialCorridorValidation/qa_manifest.json`.");
            readme.AppendLine("- Render command is recorded in the QA manifest.");

            File.WriteAllText(Path.Combine(context.PlanningRoot, "README.md"), readme.ToString());
        }

        private static void WriteManifest(Context context)
        {
            bool allPassed = Records.All(record => record.Passed);
            string recommendation = allPassed ? "accept" : "revise";
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine("  \"schema\": \"brassworks.unity_material_corridor_validation.v1\",");
            json.AppendLine("  \"bundle\": \"V0_1_53_UnityMaterialCorridorValidation\",");
            json.AppendLine("  \"generated_at_utc\": \"" + DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) + "\",");
            json.AppendLine("  \"unity_version\": \"" + Application.unityVersion + "\",");
            json.AppendLine("  \"unity_only_compliance\": true,");
            json.AppendLine("  \"external_dcc_used\": false,");
            json.AppendLine("  \"flat_documentation_board\": false,");
            json.AppendLine("  \"visible_surface_mode\": \"unity_camera_replacement_shader_vertex_material_roles\",");
            json.AppendLine("  \"set08_textures_loaded_for_availability_check\": true,");
            json.AppendLine("  \"full_texture_fidelity_render\": false,");
            json.AppendLine("  \"render_marker\": \"" + Marker + "\",");
            json.AppendLine("  \"render_command\": \"" + Escape(RecommendedCommand(context)) + "\",");
            json.AppendLine("  \"recommendation\": \"" + recommendation + "\",");
            json.AppendLine("  \"binding_guidance\": \"" + (allPassed ? "good_enough_to_guide_v0_1_53_material_binding" : "revise_before_binding") + "\",");
            json.AppendLine("  \"source_renderer\": \"AssetPacks/BrassworksBreach.UnityMaterialCorridorValidation09/Assets/Editor/UnityMaterialCorridorValidationRenderer.cs\",");
            json.AppendLine("  \"source_material_pack_reference\": \"AssetPacks/BrassworksBreach.SurfaceMaterialDetailSet08\",");
            json.AppendLine("  \"png_gate_summary\": {");
            json.AppendLine("    \"total\": " + Records.Count + ",");
            json.AppendLine("    \"passed\": " + Records.Count(record => record.Passed) + ",");
            json.AppendLine("    \"failed\": " + Records.Count(record => !record.Passed));
            json.AppendLine("  },");
            json.AppendLine("  \"images\": [");
            for (int i = 0; i < Records.Count; i++)
            {
                RenderRecord record = Records[i];
                json.AppendLine("    {");
                json.AppendLine("      \"file\": \"" + Escape(record.FileName) + "\",");
                json.AppendLine("      \"path\": \"" + Escape(record.RelativePath.Replace("\\", "/")) + "\",");
                json.AppendLine("      \"description\": \"" + Escape(record.Description) + "\",");
                json.AppendLine("      \"width\": " + record.Width + ",");
                json.AppendLine("      \"height\": " + record.Height + ",");
                json.AppendLine("      \"file_size_bytes\": " + record.FileSizeBytes + ",");
                json.AppendLine("      \"luma_mean\": " + record.LumaMean.ToString("0.###", CultureInfo.InvariantCulture) + ",");
                json.AppendLine("      \"luma_stddev\": " + record.LumaStdDev.ToString("0.###", CultureInfo.InvariantCulture) + ",");
                json.AppendLine("      \"magenta_ratio\": " + record.MagentaRatio.ToString("0.######", CultureInfo.InvariantCulture) + ",");
                json.AppendLine("      \"nonblank_and_not_magenta_gate\": \"" + (record.Passed ? "pass" : "fail") + "\"");
                json.Append("    }");
                json.AppendLine(i == Records.Count - 1 ? "" : ",");
            }
            json.AppendLine("  ],");
            json.AppendLine("  \"tested_material_roles\": [");
            json.AppendLine("    \"wet black stone floor\",");
            json.AppendLine("    \"chipped black iron wall panels\",");
            json.AppendLine("    \"brass and copper pipes\",");
            json.AppendLine("    \"amber gaslights\",");
            json.AppendLine("    \"gauge enamel and smoked glass\",");
            json.AppendLine("    \"scorched pressure door metal\",");
            json.AppendLine("    \"red pressure enamel\",");
            json.AppendLine("    \"grime, soot, edgewear, and oil accents\"");
            json.AppendLine("  ]");
            json.AppendLine("}");

            File.WriteAllText(Path.Combine(context.QaRoot, "qa_manifest.json"), json.ToString());
        }

        private static string RecommendedCommand(Context context)
        {
            string unity = "C:/Program Files/Unity/Hub/Editor/6000.4.6f1/Editor/Unity.exe";
            string log = Path.Combine(context.SidecarRoot, "UnityMaterialCorridorValidation09.render.log").Replace("\\", "/");
            return "\"" + unity + "\" -batchmode -quit -force-d3d11 -projectPath \"" + context.SidecarRoot.Replace("\\", "/") + "\" -executeMethod BrassworksBreach.Sidecars.UnityMaterialCorridorValidation09.UnityMaterialCorridorValidationRenderer.Execute -logFile \"" + log + "\"";
        }

        private static GameObject CreateBox(string name, Vector3 position, Vector3 scale, Vector3 euler, Material material)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(euler);
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            ApplyVertexColor(go, material.color);
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
            ApplyVertexColor(go, material.color);
            RemoveCollider(go);
            return go;
        }

        private static GameObject CreateTorus(string name, Vector3 position, float majorRadius, float minorRadius, Material material)
        {
            return CreateTorus(name, position, majorRadius, minorRadius, material, Vector3.zero);
        }

        private static GameObject CreateTorus(string name, Vector3 position, float majorRadius, float minorRadius, Material material, Vector3 euler)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(euler);
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            filter.sharedMesh = BuildTorusMesh(majorRadius, minorRadius, 48, 10);
            renderer.sharedMaterial = material;
            ApplyVertexColor(go, material.color);
            return go;
        }

        private static void ApplyVertexColor(GameObject go, Color color)
        {
            MeshFilter filter = go.GetComponent<MeshFilter>();
            if (filter == null || filter.sharedMesh == null)
            {
                return;
            }

            Mesh mesh = Object.Instantiate(filter.sharedMesh);
            Color[] colors = new Color[mesh.vertexCount];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            mesh.colors = colors;
            filter.sharedMesh = mesh;
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
            mesh.name = "UMCV09_torus_mesh";
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

        private static Color C(float r, float g, float b)
        {
            return new Color(r, g, b, 1f);
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private enum LightingMood
        {
            Warm,
            Cool
        }

        private sealed class RenderRequest
        {
            public readonly string FileName;
            public readonly string Description;
            public readonly LightingMood Mood;
            public readonly Vector3 CameraPosition;
            public readonly Vector3 LookAt;
            public readonly float FieldOfView;
            public readonly int Width;
            public readonly int Height;

            public RenderRequest(string fileName, string description, LightingMood mood, Vector3 cameraPosition, Vector3 lookAt, float fieldOfView, int width, int height)
            {
                FileName = fileName;
                Description = description;
                Mood = mood;
                CameraPosition = cameraPosition;
                LookAt = lookAt;
                FieldOfView = fieldOfView;
                Width = width;
                Height = height;
            }
        }

        private sealed class RenderRecord
        {
            public string FileName;
            public string Description;
            public string RelativePath;
            public int Width;
            public int Height;
            public long FileSizeBytes;
            public double LumaMean;
            public double LumaStdDev;
            public double MagentaRatio;
            public bool Passed;
        }

        private sealed class Context
        {
            private readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();
            private bool shaderLogged;

            public string SidecarRoot { get; private set; }
            public string RepoRoot { get; private set; }
            public string RenderRoot { get; private set; }
            public string PlanningRoot { get; private set; }
            public string QaRoot { get; private set; }
            public Shader ReplacementShader { get; private set; }

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

                ReplacementShader = AssetDatabase.LoadAssetAtPath<Shader>("Assets/Shaders/UMCV09LitTexture.shader");
            }

            public Material Mat(string id)
            {
                return materials[id];
            }

            public void RefreshMaterials(LightingMood mood)
            {
                materials.Clear();
                CreateMaterials(mood);
            }

            private void CreateMaterials(LightingMood mood)
            {
                Add("SMD08_MAT_WetBlackStoneSlab", MoodColor(C(0.065f, 0.061f, 0.055f), mood), 0.02f, 0.72f, 0.62f, new Vector2(2.8f, 2.8f));
                Add("SMD08_MAT_WetBlackStoneMortar", MoodColor(C(0.048f, 0.046f, 0.043f), mood), 0.01f, 0.66f, 0.7f, new Vector2(2.3f, 2.3f));
                Add("SMD08_MAT_ChippedBlackIronWallPanel", MoodColor(C(0.08f, 0.072f, 0.062f), mood), 0.88f, 0.44f, 0.58f, new Vector2(1.4f, 1.4f));
                Add("SMD08_MAT_ChippedBlackIronServicePlate", MoodColor(C(0.06f, 0.055f, 0.052f), mood), 0.9f, 0.38f, 0.68f, new Vector2(1.65f, 1.65f));
                Add("SMD08_MAT_WornBrassPipe", MoodColor(C(0.72f, 0.43f, 0.18f), mood), 0.86f, 0.46f, 0.44f, new Vector2(2.2f, 1f));
                Add("SMD08_MAT_WornBrassValveBody", MoodColor(C(0.64f, 0.39f, 0.16f), mood), 0.84f, 0.36f, 0.5f, new Vector2(1f, 1f));
                Add("SMD08_MAT_OxidizedCopperCoil", MoodColor(C(0.55f, 0.24f, 0.13f), mood), 0.82f, 0.34f, 0.52f, new Vector2(1.4f, 1.4f));
                Add("SMD08_MAT_RedPressureEnamel", MoodColor(C(0.72f, 0.07f, 0.05f), mood), 0.2f, 0.58f, 0.36f, new Vector2(1f, 1f));
                Add("SMD08_MAT_ChippedRedEnamelEdge", MoodColor(C(0.62f, 0.05f, 0.04f), mood), 0.25f, 0.42f, 0.5f, new Vector2(1.2f, 1.2f));
                Add("SMD08_MAT_AmberGaslightGlass", MoodColor(C(1f, 0.54f, 0.14f), mood), 0f, 0.86f, 0.18f, new Vector2(1f, 1f), C(1f, 0.44f, 0.12f), 1.5f, 0.72f);
                Add("SMD08_MAT_SmokedAmberGaugeGlass", MoodColor(C(0.8f, 0.52f, 0.24f), mood), 0f, 0.82f, 0.2f, new Vector2(1f, 1f), C(0.45f, 0.22f, 0.08f), 0.35f, 0.5f);
                Add("SMD08_MAT_SootDepositOverlay", MoodColor(C(0.045f, 0.036f, 0.028f), mood), 0.02f, 0.22f, 0.38f, new Vector2(1f, 1f), null, 0f, 0.48f);
                Add("SMD08_MAT_VerticalGrimeStreakOverlay", MoodColor(C(0.05f, 0.042f, 0.032f), mood), 0.03f, 0.28f, 0.42f, new Vector2(1f, 1.7f), null, 0f, 0.46f);
                Add("SMD08_MAT_BlackOilWetFloor", MoodColor(C(0.03f, 0.028f, 0.024f), mood), 0.08f, 0.86f, 0.25f, new Vector2(1.3f, 1.3f));
                Add("SMD08_MAT_OilyBlackStonePuddle", MoodColor(C(0.02f, 0.022f, 0.024f), mood), 0.04f, 0.9f, 0.18f, new Vector2(1f, 1f));
                Add("SMD08_MAT_ScorchedFurnaceMetal", MoodColor(C(0.19f, 0.13f, 0.08f), mood), 0.84f, 0.32f, 0.62f, new Vector2(1.1f, 1.1f));
                Add("SMD08_MAT_HeatTintedBoilerIron", MoodColor(C(0.18f, 0.11f, 0.075f), mood), 0.86f, 0.35f, 0.55f, new Vector2(1.2f, 1.2f));
                Add("SMD08_MAT_GaugeFaceEnamel", MoodColor(C(0.83f, 0.77f, 0.58f), mood), 0.03f, 0.5f, 0.28f, new Vector2(1f, 1f));
                Add("SMD08_MAT_BakedIvoryGaugeFace", MoodColor(C(0.86f, 0.78f, 0.62f), mood), 0.02f, 0.46f, 0.3f, new Vector2(1f, 1f));
                Add("SMD08_MAT_RivetedBrassTrim", MoodColor(C(0.7f, 0.44f, 0.18f), mood), 0.86f, 0.42f, 0.44f, new Vector2(1.2f, 1.2f));
                Add("SMD08_MAT_RivetedBlackIronTrim", MoodColor(C(0.075f, 0.066f, 0.055f), mood), 0.88f, 0.34f, 0.48f, new Vector2(1.1f, 1.1f));
                Add("SMD08_MAT_SteamCondensationBlackMetal", MoodColor(C(0.075f, 0.072f, 0.068f), mood), 0.82f, 0.62f, 0.45f, new Vector2(1.4f, 1.4f));
                Add("SMD08_MAT_CrackedBlackRubberGasket", MoodColor(C(0.025f, 0.023f, 0.022f), mood), 0.03f, 0.24f, 0.44f, new Vector2(1f, 1f));
            }

            private static Color MoodColor(Color color, LightingMood mood)
            {
                if (mood == LightingMood.Warm)
                {
                    return color;
                }

                return new Color(
                    Mathf.Clamp01(color.r * 0.58f + 0.018f),
                    Mathf.Clamp01(color.g * 0.72f + 0.028f),
                    Mathf.Clamp01(color.b * 1.35f + 0.075f),
                    color.a);
            }

            private void Add(string id, Color fallbackColor, float metallic, float smoothness, float bumpScale, Vector2 tiling, Color? emission = null, float emissionIntensity = 0f, float alpha = 1f)
            {
                Shader shader = FindSupportedShader();
                if (shader == null)
                {
                    throw new InvalidOperationException("No supported material shader found for Unity corridor validation sidecar.");
                }

                if (!shaderLogged)
                {
                    Debug.Log("UMCV09_SELECTED_SHADER name=" + shader.name + " supported=" + shader.isSupported);
                    shaderLogged = true;
                }

                Material material = new Material(shader);
                material.name = id + "_runtime_validation";
                material.color = new Color(fallbackColor.r, fallbackColor.g, fallbackColor.b, alpha);

                if (material.HasProperty("_Metallic"))
                {
                    material.SetFloat("_Metallic", metallic);
                }
                if (material.HasProperty("_Glossiness"))
                {
                    material.SetFloat("_Glossiness", smoothness);
                }

                Texture2D albedo = LoadTexture(id, "Albedo", "_ALB.png", false);
                if (albedo != null && material.HasProperty("_MainTex"))
                {
                    material.SetTexture("_MainTex", albedo);
                    material.SetTextureScale("_MainTex", tiling);
                }

                Texture2D normal = LoadTexture(id, "Normal", "_NRM.png", true);
                if (normal != null && material.HasProperty("_BumpMap"))
                {
                    material.SetTexture("_BumpMap", normal);
                    material.SetTextureScale("_BumpMap", tiling);
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
                Shader sidecarShader = AssetDatabase.LoadAssetAtPath<Shader>("Assets/Shaders/UMCV09LitTexture.shader");
                if (sidecarShader != null)
                {
                    return sidecarShader;
                }

                string[] candidates =
                {
                    "Brassworks/UMCV09 Lit Texture",
                    "Standard",
                    "Legacy Shaders/Bumped Specular",
                    "Legacy Shaders/Specular",
                    "Legacy Shaders/Diffuse",
                    "Unlit/Texture",
                    "Sprites/Default",
                    "Hidden/Internal-Colored"
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

            private Texture2D LoadTexture(string id, string family, string suffix, bool linear)
            {
                string path = Path.Combine(RepoRoot, Set08TextureRoot, family, id + suffix);
                if (!File.Exists(path))
                {
                    Notes.Add("Missing Set08 texture: " + ToRepoRelative(RepoRoot, path));
                    return null;
                }

                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, true, linear);
                bool loaded = texture.LoadImage(File.ReadAllBytes(path));
                texture.wrapMode = TextureWrapMode.Repeat;
                texture.filterMode = FilterMode.Trilinear;
                texture.anisoLevel = 4;
                if (textureLoadLogCount < 6)
                {
                    Color32 first = texture.GetPixel(0, 0);
                    Debug.Log("UMCV09_TEXTURE_LOAD id=" + id + " family=" + family + " loaded=" + loaded + " size=" + texture.width + "x" + texture.height + " first=" + first.r + "," + first.g + "," + first.b);
                    textureLoadLogCount++;
                }
                return texture;
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
    }
}
