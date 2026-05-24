using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public string gameplaySceneName = "Level01";
    public Button startButton;
    public Button quitButton;
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public Slider flashSlider;
    public Text sensitivityValueText;
    public Text volumeValueText;
    public Text flashValueText;

    private static readonly string[] AutomationArguments =
    {
        "-v0RuntimeSmoke",
        "-v0AutoPlaythrough",
        "-v0CombatSmoke",
        "-v0CombatEdgeSmoke",
        "-v0CombatScenarioSmoke",
        "-v0WeaponSwitchSmoke",
        "-v0BellowsNodeSmoke",
        "-v0RangedCombatSmoke",
        "-v0BulwarkCombatSmoke",
        "-v0WardenCombatSmoke",
        "-v0InteractionSmoke",
        "-v0HazardSmoke",
        "-v0SecretSmoke",
        "-v0PauseFlow",
        "-v0MovementSmoke",
        "-v0BalanceSmoke",
        "-v0Level01FlowSmoke"
    };

    private void Awake()
    {
        GameSettings.Load();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
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

        SyncSettingControls();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (HasArgument("-v0RangedCombatSmoke"))
        {
            RunProgress.Reset();
            SceneManager.LoadScene("Level02");
            return;
        }

        if (HasArgument("-v0BellowsNodeSmoke"))
        {
            RunProgress.Reset();
            SceneManager.LoadScene("Level03");
            return;
        }

        if (HasArgument("-v0WeaponSwitchSmoke"))
        {
            RunProgress.Reset();
            SceneManager.LoadScene("Level03");
            return;
        }

        if (HasArgument("-v0BulwarkCombatSmoke"))
        {
            RunProgress.Reset();
            SceneManager.LoadScene("Level04");
            return;
        }

        if (HasArgument("-v0WardenCombatSmoke"))
        {
            RunProgress.Reset();
            SceneManager.LoadScene("Level05");
            return;
        }

        if (HasAutomationArgument())
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        RunProgress.Reset();
        RunStats.Reset();
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
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
    }

    private static bool HasAutomationArgument()
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            for (int j = 0; j < AutomationArguments.Length; j++)
            {
                if (string.Equals(args[i], AutomationArguments[j], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool HasArgument(string argument)
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], argument, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
