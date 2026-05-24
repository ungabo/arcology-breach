using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BulwarkEnemyController : MonoBehaviour, IDamageable
{
    public EnemyDefinition definition;
    public int maxHealth = GameBalance.BulwarkHealth;
    public float detectionRange = GameBalance.BulwarkDetectionRange;
    public float moveSpeed = GameBalance.BulwarkMoveSpeed;
    public float attackRange = GameBalance.BulwarkAttackRange;
    public int attackDamage = GameBalance.BulwarkAttackDamage;
    public float attackCooldown = GameBalance.BulwarkAttackCooldown;
    public float attackWindup = GameBalance.BulwarkAttackWindup;
    public Color hitFlashColor = Color.white;
    public Color attackTellColor = new Color(1f, 0.2f, 0.04f);
    public BulwarkAttackTellVfx attackTellVfx;

    private CharacterController characterController;
    private PlayerHealth playerHealth;
    private Renderer[] renderers;
    private Color[] originalColors;
    private int currentHealth;
    private float nextAttackTime;
    private float attackResolveTime;
    private bool dead;
    private bool windingUpAttack;

    private void Awake()
    {
        ApplyDefinition();
        characterController = GetComponent<CharacterController>();
        playerHealth = Object.FindAnyObjectByType<PlayerHealth>();
        if (attackTellVfx == null)
        {
            attackTellVfx = GetComponent<BulwarkAttackTellVfx>();
        }

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
        moveSpeed = definition.moveSpeed;
        attackRange = definition.attackRange;
        attackDamage = definition.attackDamage;
        attackCooldown = definition.attackCooldown;
        attackWindup = definition.attackWindup;
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
            CancelAttackWindup();
            return;
        }

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(toPlayer.normalized);
        }

        if (distance > attackRange)
        {
            CancelAttackWindup();
            characterController.Move(toPlayer.normalized * moveSpeed * Time.deltaTime);
            return;
        }

        if (windingUpAttack)
        {
            if (Time.time >= attackResolveTime)
            {
                ResolveAttack(distance);
            }

            return;
        }

        if (Time.time >= nextAttackTime)
        {
            StartAttackWindup();
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
        CancelAttackWindup();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        MachineHitVfx.Spawn(transform.position + Vector3.up * 0.85f, 1.25f);
        SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyHit, transform.position);
        StartCoroutine(FlashHit());
    }

    private IEnumerator FlashHit()
    {
        SetColor(hitFlashColor);
        yield return new WaitForSeconds(0.08f);
        RestoreColors();
    }

    private void StartAttackWindup()
    {
        windingUpAttack = true;
        attackResolveTime = Time.time + attackWindup;
        SetColor(attackTellColor);
        attackTellVfx?.StartTell(attackWindup);
        SteamworksAudio.PlayAt(SteamworksAudioCue.BulwarkAttackTell, transform.position);
    }

    private void ResolveAttack(float distance)
    {
        windingUpAttack = false;
        nextAttackTime = Time.time + attackCooldown;
        RestoreColors();
        attackTellVfx?.StopTell();

        if (distance <= attackRange + 0.35f)
        {
            playerHealth.TakeDamage(attackDamage);
            HUDController.Instance?.ShowTemporaryMessage("BULWARK HAMMER SLAM", 0.65f);
        }
    }

    private void CancelAttackWindup()
    {
        if (!windingUpAttack)
        {
            return;
        }

        windingUpAttack = false;
        RestoreColors();
        attackTellVfx?.StopTell();
    }

    private void Die()
    {
        dead = true;
        MachineDeathVfx.Spawn(transform.position + Vector3.up * 0.75f, 1.35f);
        SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyDeath, transform.position);
        HUDController.Instance?.ShowTemporaryMessage("Bulwark down", 0.6f);
        Destroy(gameObject);
    }

    private void SetColor(Color color)
    {
        foreach (Renderer enemyRenderer in renderers)
        {
            enemyRenderer.material.color = color;
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
