using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour, IDamageable
{
    public int maxHealth = 50;
    public float detectionRange = 14f;
    public float moveSpeed = 2.8f;
    public float attackRange = 1.35f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;
    public float attackWindup = 0.38f;
    public Color hitFlashColor = Color.white;
    public Color attackTellColor = new Color(1f, 0.08f, 0.7f);

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
        characterController = GetComponent<CharacterController>();
        playerHealth = Object.FindAnyObjectByType<PlayerHealth>();
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        currentHealth = maxHealth;
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
            return;
        }

        if (distance > attackRange)
        {
            CancelAttackWindup();
            Vector3 direction = toPlayer.normalized;
            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                characterController.Move(direction * moveSpeed * Time.deltaTime);
            }

            return;
        }

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(toPlayer.normalized);
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
        }
        else
        {
            CyberpunkAudio.PlayAt(CyberpunkAudioCue.EnemyHit, transform.position);
            StartCoroutine(FlashHit());
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
        CyberpunkAudio.PlayAt(CyberpunkAudioCue.EnemyDeath, transform.position);
        HUDController.Instance?.ShowTemporaryMessage("Enemy down", 0.6f);
        Destroy(gameObject);
    }

    private void StartAttackWindup()
    {
        windingUpAttack = true;
        attackResolveTime = Time.time + attackWindup;
        SetColor(attackTellColor);
    }

    private void ResolveAttack(float distance)
    {
        windingUpAttack = false;
        nextAttackTime = Time.time + attackCooldown;
        RestoreColors();

        if (distance <= attackRange + 0.25f)
        {
            playerHealth.TakeDamage(attackDamage);
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
