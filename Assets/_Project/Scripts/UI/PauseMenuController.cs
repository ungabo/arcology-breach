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
    public Text sensitivityValueText;
    public Text volumeValueText;

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
    }
}
