using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GovernorWardenController : MonoBehaviour, IDamageable
{
    public EnemyDefinition definition;
    public Transform muzzle;
    public int maxHealth = GameBalance.GovernorWardenHealth;
    public float detectionRange = GameBalance.GovernorWardenDetectionRange;
    public float moveSpeed = GameBalance.GovernorWardenMoveSpeed;
    public float stompRange = GameBalance.GovernorWardenStompRange;
    public int stompDamage = GameBalance.GovernorWardenStompDamage;
    public float stompCooldown = GameBalance.GovernorWardenStompCooldown;
    public float stompWindup = GameBalance.GovernorWardenStompWindup;
    public float fireRange = GameBalance.GovernorWardenFireRange;
    public float fireCooldown = GameBalance.GovernorWardenFireCooldown;
    public float fireWindup = GameBalance.GovernorWardenFireWindup;
    public int projectileDamage = GameBalance.GovernorWardenProjectileDamage;
    public float projectileSpeed = GameBalance.GovernorWardenProjectileSpeed;
    public Color hitFlashColor = Color.white;
    public Color attackTellColor = new Color(1f, 0.32f, 0.04f);
    public Color enragedColor = new Color(1f, 0.08f, 0.02f);

    private CharacterController characterController;
    private PlayerHealth playerHealth;
    private Renderer[] renderers;
    private Color[] originalColors;
    private int currentHealth;
    private float nextStompTime;
    private float nextFireTime;
    private float actionResolveTime;
    private bool dead;
    private bool enraged;
    private ActionMode actionMode = ActionMode.None;

    private enum ActionMode
    {
        None,
        Stomp,
        Fire
    }

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
        stompRange = definition.attackRange;
        stompDamage = definition.attackDamage;
        stompCooldown = definition.attackCooldown;
        stompWindup = definition.attackWindup;
        fireRange = definition.fireRange;
        fireCooldown = definition.fireCooldown;
        fireWindup = definition.fireWindup;
        projectileDamage = definition.projectileDamage;
        projectileSpeed = definition.projectileSpeed;
    }

    private void Start()
    {
        UpdateBossHud();
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
            CancelAction();
            return;
        }

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(toPlayer.normalized);
        }

        if (actionMode != ActionMode.None)
        {
            if (Time.time >= actionResolveTime)
            {
                ResolveAction(distance);
            }

            return;
        }

        if (distance <= stompRange && Time.time >= nextStompTime)
        {
            StartAction(ActionMode.Stomp, stompWindup);
            return;
        }

        float activeFireCooldown = enraged ? fireCooldown * 0.72f : fireCooldown;
        if (distance <= fireRange && Time.time >= nextFireTime)
        {
            StartAction(ActionMode.Fire, fireWindup);
            nextFireTime = Time.time + activeFireCooldown;
            return;
        }

        if (distance > stompRange * 0.9f)
        {
            characterController.Move(toPlayer.normalized * moveSpeed * Time.deltaTime);
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
        CancelAction();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        UpdateBossHud();

        if (!enraged && currentHealth <= maxHealth / 2)
        {
            enraged = true;
            HUDController.Instance?.ShowTemporaryMessage("GOVERNOR PRESSURE RISING", 0.9f);
        }

        SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyHit, transform.position);
        StartCoroutine(FlashHit());
    }

    private IEnumerator FlashHit()
    {
        SetColor(hitFlashColor);
        yield return new WaitForSeconds(0.08f);
        RestoreColors();
    }

    private void StartAction(ActionMode mode, float windup)
    {
        actionMode = mode;
        actionResolveTime = Time.time + windup;
        SetColor(attackTellColor);
    }

    private void ResolveAction(float distance)
    {
        ActionMode resolvedMode = actionMode;
        actionMode = ActionMode.None;
        RestoreColors();

        if (resolvedMode == ActionMode.Stomp)
        {
            nextStompTime = Time.time + stompCooldown;
            if (distance <= stompRange + 0.45f)
            {
                playerHealth.TakeDamage(stompDamage);
                HUDController.Instance?.ShowTemporaryMessage("WARDEN PRESSURE STOMP", 0.65f);
            }

            return;
        }

        FireGovernorBolt();
    }

    private void FireGovernorBolt()
    {
        Vector3 origin = muzzle != null ? muzzle.position : transform.position + Vector3.up * 1.5f + transform.forward * 0.75f;
        Vector3 targetPoint = playerHealth.transform.position + Vector3.up * 0.8f;
        Vector3 direction = (targetPoint - origin).normalized;

        GameObject boltObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        boltObject.name = "Governor Warden Pressure Bolt";
        boltObject.transform.position = origin;
        boltObject.transform.localScale = new Vector3(0.34f, 0.34f, 0.34f);

        Collider boltCollider = boltObject.GetComponent<Collider>();
        if (boltCollider != null)
        {
            Destroy(boltCollider);
        }

        Renderer boltRenderer = boltObject.GetComponent<Renderer>();
        if (boltRenderer != null)
        {
            boltRenderer.material.color = enraged ? enragedColor : attackTellColor;
        }

        PressureBolt bolt = boltObject.AddComponent<PressureBolt>();
        bolt.Initialize(direction, projectileDamage, projectileSpeed);
    }

    private void CancelAction()
    {
        if (actionMode == ActionMode.None)
        {
            return;
        }

        actionMode = ActionMode.None;
        RestoreColors();
    }

    private void Die()
    {
        dead = true;
        WardenShutdownVfx.Spawn(transform.position + Vector3.up * 0.45f);
        SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyDeath, transform.position);
        HUDController.Instance?.ShowTemporaryMessage("Governor Warden vented", 0.9f);
        HUDController.Instance?.HideBossHealth();
        Destroy(gameObject);
    }

    private void UpdateBossHud()
    {
        HUDController.Instance?.ShowBossHealth("GOVERNOR WARDEN", currentHealth, maxHealth);
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
            renderers[i].material.color = enraged ? Color.Lerp(originalColors[i], enragedColor, 0.32f) : originalColors[i];
        }
    }
}
