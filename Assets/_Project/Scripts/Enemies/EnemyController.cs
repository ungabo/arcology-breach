using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour, IDamageable
{
    public EnemyDefinition definition;
    public int maxHealth = 50;
    public float detectionRange = 14f;
    public float moveSpeed = 2.8f;
    public float attackRange = 1.35f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;
    public float attackWindup = 0.38f;
    public float obstacleProbeDistance = 1.05f;
    public float obstacleProbeRadius = 0.35f;
    public Color hitFlashColor = Color.white;
    public Color attackTellColor = new Color(1f, 0.08f, 0.7f);

    private CharacterController characterController;
    private PlayerHealth playerHealth;
    private Renderer[] renderers;
    private Color[] originalColors;
    private int currentHealth;
    private float nextAttackTime;
    private float attackResolveTime;
    private float pressureBoostUntil;
    private float pressureBoostMultiplier = 1f;
    private bool dead;
    private bool windingUpAttack;
    private int avoidanceSide = 1;

    public bool IsPressureBoosted => Time.time < pressureBoostUntil;
    public float CurrentMoveSpeed => moveSpeed * (IsPressureBoosted ? pressureBoostMultiplier : 1f);

    private void Awake()
    {
        ApplyDefinition();
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
        obstacleProbeDistance = definition.obstacleProbeDistance;
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
            Vector3 direction = GetSteeredDirection(toPlayer.normalized);
            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                characterController.Move(direction * CurrentMoveSpeed * Time.deltaTime);
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
            MachineHitVfx.Spawn(transform.position + Vector3.up * 0.65f);
            SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyHit, transform.position);
            StartCoroutine(FlashHit());
        }
    }

    public void ApplyPressureBoost(float duration, float speedMultiplier)
    {
        if (dead || duration <= 0f || speedMultiplier <= 1f)
        {
            return;
        }

        if (!IsPressureBoosted)
        {
            pressureBoostMultiplier = speedMultiplier;
        }

        pressureBoostUntil = Mathf.Max(pressureBoostUntil, Time.time + duration);
        pressureBoostMultiplier = Mathf.Max(pressureBoostMultiplier, speedMultiplier);
        PressureBoostVfx.StartBoost(gameObject, duration);
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
        MachineDeathVfx.Spawn(transform.position + Vector3.up * 0.55f);
        SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyDeath, transform.position);
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

    private Vector3 GetSteeredDirection(Vector3 desiredDirection)
    {
        if (!HasObstacle(desiredDirection))
        {
            return desiredDirection;
        }

        Vector3 right = Vector3.Cross(Vector3.up, desiredDirection).normalized;
        Vector3 primary = (desiredDirection + right * avoidanceSide * 1.15f).normalized;
        if (!HasObstacle(primary))
        {
            return primary;
        }

        Vector3 secondary = (desiredDirection - right * avoidanceSide * 1.15f).normalized;
        if (!HasObstacle(secondary))
        {
            avoidanceSide *= -1;
            return secondary;
        }

        return right * avoidanceSide;
    }

    private bool HasObstacle(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            return false;
        }

        Vector3 origin = transform.position + Vector3.up * 0.7f;
        if (!Physics.SphereCast(origin, obstacleProbeRadius, direction.normalized, out RaycastHit hit, obstacleProbeDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        if (hit.collider.GetComponentInParent<PlayerController>() != null)
        {
            return false;
        }

        EnemyController hitEnemy = hit.collider.GetComponentInParent<EnemyController>();
        return hitEnemy == null || hitEnemy != this;
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
