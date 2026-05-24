using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }

    [Header("HUD Text")]
    public Text healthText;
    public Text ammoText;
    public Text keyText;
    public Text messageText;
    public Text interactionText;
    public Text objectiveText;
    public Text bossNameText;
    public Image damageFlashImage;
    public Image healthFillImage;
    public Image ammoFillImage;
    public Image keyLampImage;
    public Image objectiveBackplateImage;
    public Image interactionBackplateImage;
    public Image interactionIconImage;
    public Image bossBackplateImage;
    public Image bossFillImage;
    public Sprite keyLampOffSprite;
    public Sprite keyLampOnSprite;
    public Sprite keyLampDeniedSprite;
    public Sprite promptInteractSprite;
    public Sprite promptGearKeySprite;
    public Sprite promptValveSprite;
    public Sprite promptLiftSprite;
    public Sprite promptAmmoSprite;
    public Sprite promptHealthSprite;
    public Sprite promptWarningSprite;
    public Sprite promptSecretSprite;
    public Sprite promptPauseSprite;
    public Sprite promptMouseRightSprite;

    public bool HighContrastModeApplied { get; private set; }

    private float messageTimer;
    private bool messageIsPersistent;
    private float damageFlashAlpha;
    private float keyDeniedTimer;
    private bool hasGearKey;
    private int highestAmmoSeen = 1;
    private bool readabilityInitialized;

    private void Awake()
    {
        Instance = this;
        ApplyReadabilitySettings(true);
        ClearMessage();
        ClearInteractionPrompt();
        ClearObjective();
        HideBossHealth();
    }

    public string CurrentObjective => objectiveText != null ? objectiveText.text : string.Empty;

    private void Update()
    {
        ApplyReadabilitySettings(false);
        UpdateKeyDeniedFlash();

        if (messageIsPersistent || messageTimer <= 0f)
        {
            FadeDamageFlash();
            return;
        }

        messageTimer -= Time.unscaledDeltaTime;
        if (messageTimer <= 0f)
        {
            ClearMessage();
        }

        FadeDamageFlash();
    }

    private void FadeDamageFlash()
    {
        if (damageFlashImage == null || damageFlashAlpha <= 0f)
        {
            return;
        }

        damageFlashAlpha = Mathf.Max(0f, damageFlashAlpha - Time.unscaledDeltaTime * 2.8f);
        Color color = damageFlashImage.color;
        color.a = damageFlashAlpha;
        damageFlashImage.color = color;
    }

    public void FlashDamage()
    {
        if (damageFlashImage == null)
        {
            return;
        }

        GameSettings.Load();
        damageFlashAlpha = 0.35f * GameSettings.FlashIntensity;
        Color color = damageFlashImage.color;
        color.a = damageFlashAlpha;
        damageFlashImage.color = color;
    }

    public void SetHealth(int current, int max)
    {
        if (healthText != null)
        {
            healthText.text = $"HEALTH {current}/{max}";
        }

        if (healthFillImage != null && max > 0)
        {
            healthFillImage.fillAmount = Mathf.Clamp01(current / (float)max);
        }
    }

    public void SetAmmo(int ammo)
    {
        highestAmmoSeen = Mathf.Max(highestAmmoSeen, ammo);
        if (ammoText != null)
        {
            ammoText.text = $"AMMO {ammo}";
        }

        if (ammoFillImage != null)
        {
            ammoFillImage.fillAmount = Mathf.Clamp01(ammo / (float)highestAmmoSeen);
        }
    }

    public void SetKey(bool hasKey)
    {
        hasGearKey = hasKey;
        if (keyText != null)
        {
            keyText.text = hasKey ? "GEAR KEY YES" : "GEAR KEY NO";
        }

        RefreshKeyLamp();
    }

    public void FlashKeyDenied(float seconds = 0.75f)
    {
        if (hasGearKey || keyLampImage == null || keyLampDeniedSprite == null)
        {
            return;
        }

        keyDeniedTimer = Mathf.Max(keyDeniedTimer, seconds);
        keyLampImage.sprite = keyLampDeniedSprite;
        keyLampImage.color = Color.white;
    }

    private void UpdateKeyDeniedFlash()
    {
        if (keyDeniedTimer <= 0f)
        {
            return;
        }

        keyDeniedTimer = Mathf.Max(0f, keyDeniedTimer - Time.unscaledDeltaTime);
        if (keyDeniedTimer <= 0f)
        {
            RefreshKeyLamp();
        }
    }

    private void RefreshKeyLamp()
    {
        if (keyLampImage == null)
        {
            return;
        }

        keyLampImage.sprite = hasGearKey && keyLampOnSprite != null ? keyLampOnSprite : keyLampOffSprite;
        keyLampImage.color = Color.white;
    }

    public void ShowBossHealth(string bossName, int current, int max)
    {
        if (bossNameText != null)
        {
            bossNameText.text = bossName;
            bossNameText.enabled = true;
        }

        if (bossBackplateImage != null)
        {
            bossBackplateImage.enabled = true;
        }

        if (bossFillImage != null)
        {
            bossFillImage.enabled = true;
            bossFillImage.fillAmount = max > 0 ? Mathf.Clamp01(current / (float)max) : 0f;
        }
    }

    public void HideBossHealth()
    {
        if (bossNameText != null)
        {
            bossNameText.text = string.Empty;
            bossNameText.enabled = false;
        }

        if (bossBackplateImage != null)
        {
            bossBackplateImage.enabled = false;
        }

        if (bossFillImage != null)
        {
            bossFillImage.fillAmount = 0f;
            bossFillImage.enabled = false;
        }
    }

    public void SetObjective(string objective)
    {
        bool hasObjective = !string.IsNullOrWhiteSpace(objective);
        if (objectiveText != null)
        {
            objectiveText.text = hasObjective ? "OBJECTIVE: " + objective : string.Empty;
            objectiveText.enabled = hasObjective;
        }

        if (objectiveBackplateImage != null)
        {
            objectiveBackplateImage.enabled = hasObjective;
        }
    }

    public void ClearObjective()
    {
        if (objectiveText != null)
        {
            objectiveText.text = string.Empty;
            objectiveText.enabled = false;
        }

        if (objectiveBackplateImage != null)
        {
            objectiveBackplateImage.enabled = false;
        }
    }

    public void ShowTemporaryMessage(string message, float seconds = 1.5f)
    {
        if (messageText == null)
        {
            return;
        }

        messageText.text = message;
        messageText.enabled = true;
        messageTimer = seconds;
        messageIsPersistent = false;
    }

    public void ShowPersistentMessage(string message)
    {
        if (messageText == null)
        {
            return;
        }

        messageText.text = message;
        messageText.enabled = true;
        messageTimer = 0f;
        messageIsPersistent = true;
    }

    public void ClearMessage()
    {
        messageTimer = 0f;
        messageIsPersistent = false;

        if (messageText != null)
        {
            messageText.text = string.Empty;
            messageText.enabled = false;
        }
    }

    public void SetInteractionPrompt(string prompt)
    {
        if (interactionText == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(prompt))
        {
            ClearInteractionPrompt();
            return;
        }

        interactionText.text = prompt;
        interactionText.enabled = true;

        if (interactionBackplateImage != null)
        {
            interactionBackplateImage.enabled = true;
        }

        if (interactionIconImage != null)
        {
            interactionIconImage.sprite = ChooseInteractionIcon(prompt);
            interactionIconImage.enabled = interactionIconImage.sprite != null;
        }
    }

    public void ClearInteractionPrompt()
    {
        if (interactionText != null)
        {
            interactionText.text = string.Empty;
            interactionText.enabled = false;
        }

        if (interactionBackplateImage != null)
        {
            interactionBackplateImage.enabled = false;
        }

        if (interactionIconImage != null)
        {
            interactionIconImage.enabled = false;
        }
    }

    private Sprite ChooseInteractionIcon(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return promptInteractSprite;
        }

        string normalized = prompt.ToLowerInvariant();
        if (normalized.Contains("locked") || normalized.Contains("required") || normalized.Contains("warden"))
        {
            return promptWarningSprite != null ? promptWarningSprite : promptInteractSprite;
        }

        if (normalized.Contains("key"))
        {
            return promptGearKeySprite != null ? promptGearKeySprite : promptInteractSprite;
        }

        if (normalized.Contains("valve") || normalized.Contains("vent"))
        {
            return promptValveSprite != null ? promptValveSprite : promptInteractSprite;
        }

        if (normalized.Contains("lift") || normalized.Contains("hoist"))
        {
            return promptLiftSprite != null ? promptLiftSprite : promptInteractSprite;
        }

        if (normalized.Contains("ammo") || normalized.Contains("cartridge"))
        {
            return promptAmmoSprite != null ? promptAmmoSprite : promptInteractSprite;
        }

        if (normalized.Contains("health"))
        {
            return promptHealthSprite != null ? promptHealthSprite : promptInteractSprite;
        }

        if (normalized.Contains("secret") || normalized.Contains("cache"))
        {
            return promptSecretSprite != null ? promptSecretSprite : promptInteractSprite;
        }

        if (normalized.Contains("pause"))
        {
            return promptPauseSprite != null ? promptPauseSprite : promptInteractSprite;
        }

        if (normalized.Contains("right") || normalized.Contains("burst"))
        {
            return promptMouseRightSprite != null ? promptMouseRightSprite : promptInteractSprite;
        }

        return promptInteractSprite;
    }

    private void ApplyReadabilitySettings(bool force)
    {
        GameSettings.Load();
        if (!force && readabilityInitialized && HighContrastModeApplied == GameSettings.HighContrast)
        {
            return;
        }

        HighContrastModeApplied = GameSettings.HighContrast;
        readabilityInitialized = true;

        FontStyle style = HighContrastModeApplied ? FontStyle.Bold : FontStyle.Normal;
        Color primaryText = HighContrastModeApplied ? Color.white : new Color(0.96f, 0.91f, 0.78f, 1f);
        Color systemText = HighContrastModeApplied ? Color.white : new Color(1f, 0.84f, 0.48f, 1f);
        Color warningText = HighContrastModeApplied ? new Color(1f, 0.98f, 0.82f, 1f) : Color.white;

        ApplyTextReadability(healthText, systemText, style);
        ApplyTextReadability(ammoText, systemText, style);
        ApplyTextReadability(keyText, systemText, style);
        ApplyTextReadability(objectiveText, warningText, style);
        ApplyTextReadability(interactionText, warningText, style);
        ApplyTextReadability(messageText, primaryText, style);
        ApplyTextReadability(bossNameText, warningText, style);

        ApplyImageReadability(objectiveBackplateImage, HighContrastModeApplied ? 1f : 0.92f);
        ApplyImageReadability(interactionBackplateImage, HighContrastModeApplied ? 1f : 0.92f);
        ApplyImageReadability(bossBackplateImage, HighContrastModeApplied ? 1f : 0.95f);
    }

    private static void ApplyTextReadability(Text text, Color color, FontStyle style)
    {
        if (text == null)
        {
            return;
        }

        text.color = color;
        text.fontStyle = style;
    }

    private static void ApplyImageReadability(Image image, float alpha)
    {
        if (image == null)
        {
            return;
        }

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
