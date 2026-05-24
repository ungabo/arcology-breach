using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject root;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public Slider flashSlider;
    public Button resolutionButton;
    public Toggle fullscreenToggle;
    public Text sensitivityValueText;
    public Text volumeValueText;
    public Text flashValueText;
    public Text resolutionValueText;
    public Text fullscreenValueText;

    public bool IsVisible => root != null && root.activeSelf;

    private void Awake()
    {
        GameSettings.Load();

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartRun);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitRun);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (flashSlider != null)
        {
            flashSlider.onValueChanged.AddListener(SetFlashIntensity);
        }

        if (resolutionButton != null)
        {
            resolutionButton.onClick.AddListener(CycleResolution);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }

        SetVisible(false);
        SyncSettingControls();
    }

    public void SetVisible(bool visible)
    {
        if (root != null)
        {
            root.SetActive(visible);
        }

        if (visible)
        {
            SyncSettingControls();
        }
    }

    public void ResumeGame()
    {
        GameStateController.Instance?.ResumeGameplay();
    }

    public void RestartRun()
    {
        GameStateController.Instance?.RestartLevel();
    }

    public void QuitRun()
    {
        GameStateController.Instance?.QuitGame();
    }

    public void SetMouseSensitivity(float value)
    {
        GameSettings.SetMouseSensitivity(value);
        UpdateSettingLabels();
    }

    public void SetMasterVolume(float value)
    {
        GameSettings.SetMasterVolume(value);
        UpdateSettingLabels();
    }

    public void SetFlashIntensity(float value)
    {
        GameSettings.SetFlashIntensity(value);
        UpdateSettingLabels();
    }

    public void CycleResolution()
    {
        GameSettings.CycleResolution();
        UpdateSettingLabels();
    }

    public void SetFullscreen(bool value)
    {
        GameSettings.SetFullscreen(value);
        UpdateSettingLabels();
    }

    private void SyncSettingControls()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.SetValueWithoutNotify(GameSettings.MouseSensitivity);
        }

        if (volumeSlider != null)
        {
            volumeSlider.SetValueWithoutNotify(GameSettings.MasterVolume);
        }

        if (flashSlider != null)
        {
            flashSlider.SetValueWithoutNotify(GameSettings.FlashIntensity);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(GameSettings.Fullscreen);
        }

        UpdateSettingLabels();
    }

    private void UpdateSettingLabels()
    {
        if (sensitivityValueText != null)
        {
            sensitivityValueText.text = GameSettings.MouseSensitivity.ToString("0.0");
        }

        if (volumeValueText != null)
        {
            volumeValueText.text = Mathf.RoundToInt(GameSettings.MasterVolume * 100f) + "%";
        }

        if (flashValueText != null)
        {
            flashValueText.text = Mathf.RoundToInt(GameSettings.FlashIntensity * 100f) + "%";
        }

        if (resolutionValueText != null)
        {
            resolutionValueText.text = GameSettings.ResolutionLabel;
        }

        if (fullscreenValueText != null)
        {
            fullscreenValueText.text = GameSettings.Fullscreen ? "ON" : "OFF";
        }
    }
}
