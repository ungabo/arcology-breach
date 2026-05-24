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
    public Image bossBackplateImage;
    public Image bossFillImage;
    public Sprite keyLampOffSprite;
    public Sprite keyLampOnSprite;
    public Sprite keyLampDeniedSprite;

    private float messageTimer;
    private bool messageIsPersistent;
    private float damageFlashAlpha;
    private int highestAmmoSeen = 1;

    private void Awake()
    {
        Instance = this;
        ClearMessage();
        ClearInteractionPrompt();
        ClearObjective();
        HideBossHealth();
    }

    public string CurrentObjective => objectiveText != null ? objectiveText.text : string.Empty;

    private void Update()
    {
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

        damageFlashAlpha = 0.35f;
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
        if (keyText != null)
        {
            keyText.text = hasKey ? "GEAR KEY YES" : "GEAR KEY NO";
        }

        if (keyLampImage != null)
        {
            keyLampImage.sprite = hasKey && keyLampOnSprite != null ? keyLampOnSprite : keyLampOffSprite;
            keyLampImage.color = Color.white;
        }
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
    }
}
