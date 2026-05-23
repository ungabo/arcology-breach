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
    public Text bossNameText;
    public Image damageFlashImage;
    public Image healthFillImage;
    public Image ammoFillImage;
    public Image keyLampImage;
    public Image bossBackplateImage;
    public Image bossFillImage;

    private float messageTimer;
    private bool messageIsPersistent;
    private float damageFlashAlpha;
    private int highestAmmoSeen = 1;

    private void Awake()
    {
        Instance = this;
        ClearMessage();
        ClearInteractionPrompt();
        HideBossHealth();
    }

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
            keyLampImage.color = hasKey ? new Color(0.25f, 0.95f, 0.35f, 0.95f) : new Color(0.95f, 0.55f, 0.08f, 0.95f);
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
    }

    public void ClearInteractionPrompt()
    {
        if (interactionText != null)
        {
            interactionText.text = string.Empty;
            interactionText.enabled = false;
        }
    }
}
