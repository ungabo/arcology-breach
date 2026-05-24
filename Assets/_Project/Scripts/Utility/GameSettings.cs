using UnityEngine;

public static class GameSettings
{
    public const float DefaultMouseSensitivity = 2.2f;
    public const float DefaultMasterVolume = 0.55f;
    public const float DefaultFlashIntensity = 1f;
    public const float MinFlashIntensity = 0.15f;
    public const float MaxFlashIntensity = 1f;
    public const int MinResolutionIndex = 0;
    public const int MaxResolutionIndex = 2;
    public const int DefaultResolutionIndex = 1;
    public const bool DefaultFullscreen = false;

    private const string MouseSensitivityKey = "BrassworksBreach.MouseSensitivity";
    private const string MasterVolumeKey = "BrassworksBreach.MasterVolume";
    private const string FlashIntensityKey = "BrassworksBreach.FlashIntensity";
    private const string ResolutionIndexKey = "BrassworksBreach.ResolutionIndex";
    private const string FullscreenKey = "BrassworksBreach.Fullscreen";

    private static readonly Vector2Int[] ResolutionPresets =
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080)
    };

    private static bool loaded;

    public static float MouseSensitivity { get; private set; } = DefaultMouseSensitivity;
    public static float MasterVolume { get; private set; } = DefaultMasterVolume;
    public static float FlashIntensity { get; private set; } = DefaultFlashIntensity;
    public static int ResolutionIndex { get; private set; } = DefaultResolutionIndex;
    public static bool Fullscreen { get; private set; } = DefaultFullscreen;
    public static int ResolutionWidth => ResolutionPresets[ResolutionIndex].x;
    public static int ResolutionHeight => ResolutionPresets[ResolutionIndex].y;
    public static string ResolutionLabel => ResolutionWidth + "x" + ResolutionHeight;

    public static void Load()
    {
        if (loaded)
        {
            return;
        }

        MouseSensitivity = Mathf.Clamp(PlayerPrefs.GetFloat(MouseSensitivityKey, DefaultMouseSensitivity), 0.6f, 5f);
        MasterVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(MasterVolumeKey, DefaultMasterVolume));
        FlashIntensity = ClampFlashIntensity(PlayerPrefs.GetFloat(FlashIntensityKey, DefaultFlashIntensity));
        ResolutionIndex = ClampResolutionIndex(PlayerPrefs.GetInt(ResolutionIndexKey, DefaultResolutionIndex));
        Fullscreen = PlayerPrefs.GetInt(FullscreenKey, DefaultFullscreen ? 1 : 0) == 1;
        AudioListener.volume = MasterVolume;
        ApplyDisplayMode();
        loaded = true;
    }

    public static void SetMouseSensitivity(float value)
    {
        Load();
        MouseSensitivity = Mathf.Clamp(value, 0.6f, 5f);
        PlayerPrefs.SetFloat(MouseSensitivityKey, MouseSensitivity);
        PlayerPrefs.Save();
    }

    public static void SetMasterVolume(float value)
    {
        Load();
        MasterVolume = Mathf.Clamp01(value);
        AudioListener.volume = MasterVolume;
        PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
        PlayerPrefs.Save();
    }

    public static void SetFlashIntensity(float value)
    {
        Load();
        FlashIntensity = ClampFlashIntensity(value);
        PlayerPrefs.SetFloat(FlashIntensityKey, FlashIntensity);
        PlayerPrefs.Save();
    }

    public static void SetResolutionIndex(int value)
    {
        Load();
        ResolutionIndex = ClampResolutionIndex(value);
        PlayerPrefs.SetInt(ResolutionIndexKey, ResolutionIndex);
        PlayerPrefs.Save();
        ApplyDisplayMode();
    }

    public static void CycleResolution()
    {
        Load();
        int nextIndex = ResolutionIndex + 1;
        if (nextIndex > MaxResolutionIndex)
        {
            nextIndex = MinResolutionIndex;
        }

        SetResolutionIndex(nextIndex);
    }

    public static void SetFullscreen(bool value)
    {
        Load();
        Fullscreen = value;
        PlayerPrefs.SetInt(FullscreenKey, Fullscreen ? 1 : 0);
        PlayerPrefs.Save();
        ApplyDisplayMode();
    }

    public static float ClampFlashIntensity(float value)
    {
        return Mathf.Clamp(value, MinFlashIntensity, MaxFlashIntensity);
    }

    public static int ClampResolutionIndex(int value)
    {
        return Mathf.Clamp(value, MinResolutionIndex, MaxResolutionIndex);
    }

    private static void ApplyDisplayMode()
    {
        if (Application.isBatchMode)
        {
            return;
        }

        FullScreenMode mode = Fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(ResolutionWidth, ResolutionHeight, mode);
    }
}
