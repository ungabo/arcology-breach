using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BellowsNodeController : MonoBehaviour, IDamageable
{
    public EnemyDefinition definition;
    public int maxHealth = GameBalance.BellowsNodeHealth;
    public float detectionRange = GameBalance.BellowsNodeDetectionRange;
    public float pulseRange = GameBalance.BellowsNodePulseRange;
    public int pulseDamage = GameBalance.BellowsNodePulseDamage;
    public float pulseCooldown = GameBalance.BellowsNodePulseCooldown;
    public float pulseWindup = GameBalance.BellowsNodePulseWindup;
    public float boostDuration = GameBalance.BellowsNodeBoostDuration;
    public float boostMultiplier = GameBalance.BellowsNodeBoostMultiplier;
    public Color hitFlashColor = Color.white;
    public Color pulseTellColor = new Color(1f, 0.24f, 0.08f);

    private PlayerHealth playerHealth;
    private Renderer[] renderers;
    private Color[] originalColors;
    private int currentHealth;
    private float nextPulseTime;
    private float pulseResolveTime;
    private bool dead;
    private bool windingUpPulse;

    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        ApplyDefinition();
        playerHealth = Object.FindAnyObjectByType<PlayerHealth>();
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        currentHealth = maxHealth;
    }

    private void ApplyDefinition()
    {
        if (definition == null)
        {
            return;
        }

        maxHealth = definition.maxHealth;
        detectionRange = definition.detectionRange;
        pulseRange = definition.attackRange;
        pulseDamage = definition.attackDamage;
        pulseCooldown = definition.attackCooldown;
        pulseWindup = definition.attackWindup;
    }

    private void Update()
    {
        if (dead || playerHealth == null || playerHealth.IsDead)
        {
            return;
        }

        if (GameStateController.Instance != null && !GameStateController.Instance.IsGameplayActive)
        {
            return;
        }

        Vector3 toPlayer = playerHealth.transform.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;
        if (distance > detectionRange)
        {
            CancelPulseWindup();
            return;
        }

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(toPlayer.normalized);
        }

        if (windingUpPulse)
        {
            if (Time.time >= pulseResolveTime)
            {
                EmitPulse();
            }

            return;
        }

        if (Time.time >= nextPulseTime)
        {
            StartPulseWindup();
        }
    }

    public void ForcePulseForTest()
    {
        if (!dead)
        {
            EmitPulse();
        }
    }

    public void TakeDamage(int amount)
    {
        if (dead || amount <= 0)
        {
            return;
        }

        currentHealth -= amount;
        StopAllCoroutines();
        CancelPulseWindup();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        MachineHitVfx.Spawn(transform.position + Vector3.up * 0.78f, 1.05f);
        SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyHit, transform.position);
        if (isActiveAndEnabled)
        {
            StartCoroutine(FlashHit());
        }
    }

    private void StartPulseWindup()
    {
        windingUpPulse = true;
        pulseResolveTime = Time.time + pulseWindup;
        SetColor(pulseTellColor);
    }

    private void EmitPulse()
    {
        windingUpPulse = false;
        nextPulseTime = Time.time + pulseCooldown;
        RestoreColors();

        BellowsNodePulseVfx.Spawn(transform.position + Vector3.up * 0.32f);
        SteamworksAudio.PlayAt(SteamworksAudioCue.GateDenied, transform.position);

        if (playerHealth == null)
        {
            playerHealth = Object.FindAnyObjectByType<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            return;
        }

        Vector3 toPlayer = playerHealth.transform.position - transform.position;
        toPlayer.y = 0f;
        if (toPlayer.magnitude <= pulseRange)
        {
            playerHealth.TakeDamage(pulseDamage);
        }

        BoostNearbyScrappers();
    }

    private void BoostNearbyScrappers()
    {
        EnemyController[] scrappers = Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (EnemyController scrapper in scrappers)
        {
            if (scrapper == null || !scrapper.gameObject.activeInHierarchy)
            {
                continue;
            }

            Vector3 toScrapper = scrapper.transform.position - transform.position;
            toScrapper.y = 0f;
            if (toScrapper.magnitude <= pulseRange)
            {
                scrapper.ApplyPressureBoost(boostDuration, boostMultiplier);
                MachineHitVfx.Spawn(scrapper.transform.position + Vector3.up * 0.65f, 0.75f);
            }
        }
    }

    private IEnumerator FlashHit()
    {
        SetColor(hitFlashColor);
        yield return new WaitForSeconds(0.08f);
        RestoreColors();
    }

    private void Die()
    {
        dead = true;
        MachineDeathVfx.Spawn(transform.position + Vector3.up * 0.7f, 1.1f);
        SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyDeath, transform.position);
        HUDController.Instance?.ShowTemporaryMessage("Bellows Node vented", 0.7f);
        Destroy(gameObject);
    }

    private void CancelPulseWindup()
    {
        if (!windingUpPulse)
        {
            return;
        }

        windingUpPulse = false;
        RestoreColors();
    }

    private void SetColor(Color color)
    {
        foreach (Renderer nodeRenderer in renderers)
        {
            nodeRenderer.material.color = color;
        }
    }

    private void RestoreColors()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }
}
