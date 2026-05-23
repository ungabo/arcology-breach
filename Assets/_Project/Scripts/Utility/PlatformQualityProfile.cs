using UnityEngine;

public enum PlatformQualityTarget
{
    WindowsMidLow,
    AndroidPhone,
    WebGLBrowser,
    PcVr,
    MetaQuest
}

[CreateAssetMenu(menuName = "Brassworks/Platform Quality Profile")]
public class PlatformQualityProfile : ScriptableObject
{
    public PlatformQualityTarget target = PlatformQualityTarget.WindowsMidLow;
    public int targetFrameRate = 60;
    public int vSyncCount = 0;
    public int pixelLightCount = 2;
    public int antiAliasing = 0;
    public float shadowDistance = 32f;
    public float lodBias = 0.85f;
    public bool realtimeReflectionProbes;
    public bool softParticles;
    public bool allowCameraMsaa;
    public bool allowDynamicResolution;
    public int maxTextureSize = 1024;
    public int maxDynamicLights = 3;
    public int targetBuildSizeMegabytes = 600;
    public string notes = "Windows mid/low profile.";
}
