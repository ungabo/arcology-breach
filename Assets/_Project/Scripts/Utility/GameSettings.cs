using UnityEngine;

public static class GameSettings
{
    public const float DefaultMouseSensitivity = 2.2f;
    public const float DefaultMasterVolume = 0.55f;
    public const float DefaultFlashIntensity = 1f;
    public const float MinFlashIntensity = 0.15f;
    public const float MaxFlashIntensity = 1f;

    private const string MouseSensitivityKey = "BrassworksBreach.MouseSensitivity";
    private const string MasterVolumeKey = "BrassworksBreach.MasterVolume";
    private const string FlashIntensityKey = "BrassworksBreach.FlashIntensity";

    private static bool loaded;

    public static float MouseSensitivity { get; private set; } = DefaultMouseSensitivity;
    public static float MasterVolume { get; private set; } = DefaultMasterVolume;
    public static float FlashIntensity { get; private set; } = DefaultFlashIntensity;

    public static void Load()
    {
        if (loaded)
        {
            return;
        }

        MouseSensitivity = Mathf.Clamp(PlayerPrefs.GetFloat(MouseSensitivityKey, DefaultMouseSensitivity), 0.6f, 5f);
        MasterVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(MasterVolumeKey, DefaultMasterVolume));
        FlashIntensity = ClampFlashIntensity(PlayerPrefs.GetFloat(FlashIntensityKey, DefaultFlashIntensity));
        AudioListener.volume = MasterVolume;
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

    public static float ClampFlashIntensity(float value)
    {
        return Mathf.Clamp(value, MinFlashIntensity, MaxFlashIntensity);
    }
}
