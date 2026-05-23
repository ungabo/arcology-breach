using UnityEngine;

public class RuntimePerformanceProfile : MonoBehaviour
{
    public const int WindowsTargetFrameRate = 60;
    public const int WindowsVSyncCount = 0;
    public const int WindowsPixelLightCount = 2;
    public const int WindowsAntiAliasing = 0;
    public const float WindowsShadowDistance = 32f;
    public const float WindowsLodBias = 0.85f;

    public static bool Applied { get; private set; }

    private void Awake()
    {
        ApplyWindowsMidLowProfile();
    }

    public static void ApplyWindowsMidLowProfile()
    {
        Application.targetFrameRate = WindowsTargetFrameRate;
        QualitySettings.vSyncCount = WindowsVSyncCount;
        QualitySettings.pixelLightCount = WindowsPixelLightCount;
        QualitySettings.antiAliasing = WindowsAntiAliasing;
        QualitySettings.shadowDistance = WindowsShadowDistance;
        QualitySettings.lodBias = WindowsLodBias;
        QualitySettings.realtimeReflectionProbes = false;
        QualitySettings.softParticles = false;

        Camera[] cameras = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);
        foreach (Camera camera in cameras)
        {
            camera.allowMSAA = false;
            camera.allowDynamicResolution = false;
        }

        Applied = true;
    }
}
