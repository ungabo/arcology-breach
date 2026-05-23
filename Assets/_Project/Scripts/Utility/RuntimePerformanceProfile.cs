using UnityEngine;

public class RuntimePerformanceProfile : MonoBehaviour
{
    public const int WindowsTargetFrameRate = 60;
    public const int WindowsVSyncCount = 0;
    public const int WindowsPixelLightCount = 2;
    public const int WindowsAntiAliasing = 0;
    public const float WindowsShadowDistance = 32f;
    public const float WindowsLodBias = 0.85f;

    public PlatformQualityProfile activeProfile;

    public static bool Applied { get; private set; }
    public static PlatformQualityProfile AppliedProfile { get; private set; }
    public static PlatformQualityTarget AppliedTarget { get; private set; }

    private void Awake()
    {
        if (activeProfile != null)
        {
            ApplyProfile(activeProfile);
            return;
        }

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
        AppliedProfile = null;
        AppliedTarget = PlatformQualityTarget.WindowsMidLow;
    }

    public static void ApplyProfile(PlatformQualityProfile profile)
    {
        if (profile == null)
        {
            ApplyWindowsMidLowProfile();
            return;
        }

        Application.targetFrameRate = profile.targetFrameRate;
        QualitySettings.vSyncCount = profile.vSyncCount;
        QualitySettings.pixelLightCount = profile.pixelLightCount;
        QualitySettings.antiAliasing = profile.antiAliasing;
        QualitySettings.shadowDistance = profile.shadowDistance;
        QualitySettings.lodBias = profile.lodBias;
        QualitySettings.realtimeReflectionProbes = profile.realtimeReflectionProbes;
        QualitySettings.softParticles = profile.softParticles;

        Camera[] cameras = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);
        foreach (Camera camera in cameras)
        {
            camera.allowMSAA = profile.allowCameraMsaa;
            camera.allowDynamicResolution = profile.allowDynamicResolution;
        }

        Applied = true;
        AppliedProfile = profile;
        AppliedTarget = profile.target;
    }
}
