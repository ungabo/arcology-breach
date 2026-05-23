using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        HUDController.Instance?.SetHealth(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        SteamworksAudio.Play(SteamworksAudioCue.PlayerHurt);
        HUDController.Instance?.SetHealth(CurrentHealth, maxHealth);
        HUDController.Instance?.FlashDamage();

        if (CurrentHealth <= 0)
        {
            IsDead = true;
            GameStateController.Instance?.PlayerDied();
        }
        else
        {
            HUDController.Instance?.ShowTemporaryMessage("HIT", 0.4f);
        }
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        HUDController.Instance?.SetHealth(CurrentHealth, maxHealth);
    }

    public void RestoreForTransition(int currentHealth, int restoredMaxHealth)
    {
        maxHealth = Mathf.Max(1, restoredMaxHealth);
        CurrentHealth = Mathf.Clamp(currentHealth, 1, maxHealth);
        IsDead = false;
        HUDController.Instance?.SetHealth(CurrentHealth, maxHealth);
    }
}
