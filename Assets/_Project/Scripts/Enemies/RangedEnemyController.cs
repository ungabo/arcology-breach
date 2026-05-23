using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RangedEnemyController : MonoBehaviour, IDamageable
{
    public EnemyDefinition definition;
    public Transform muzzle;
    public int maxHealth = 40;
    public float detectionRange = 18f;
    public float fireRange = 14f;
    public float moveSpeed = 1.8f;
    public float fireCooldown = 1.35f;
    public float fireWindup = 0.42f;
    public int projectileDamage = 8;
    public float projectileSpeed = 8f;
    public Color hitFlashColor = Color.white;
    public Color fireTellColor = new Color(1f, 0.42f, 0.08f);

    private CharacterController characterController;
    private PlayerHealth playerHealth;
    private Renderer[] renderers;
    private Color[] originalColors;
    private int currentHealth;
    private float nextFireTime;
    private float fireResolveTime;
    private bool dead;
    private bool windingUpFire;

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
        fireRange = definition.fireRange;
        moveSpeed = definition.moveSpeed;
        fireCooldown = definition.fireCooldown;
        fireWindup = definition.fireWindup;
        projectileDamage = definition.projectileDamage;
        projectileSpeed = definition.projectileSpeed;
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
            CancelFireWindup();
            return;
        }

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(toPlayer.normalized);
        }

        if (distance > fireRange)
        {
            CancelFireWindup();
            characterController.Move(toPlayer.normalized * moveSpeed * Time.deltaTime);
            return;
        }

        if (windingUpFire)
        {
            if (Time.time >= fireResolveTime)
            {
                FirePressureBolt();
            }

            return;
        }

        if (Time.time >= nextFireTime)
        {
            StartFireWindup();
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
        CancelFireWindup();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyHit, transform.position);
            StartCoroutine(FlashHit());
        }
    }

    private IEnumerator FlashHit()
    {
        SetColor(hitFlashColor);
        yield return new WaitForSeconds(0.08f);
        RestoreColors();
    }

    private void StartFireWindup()
    {
        windingUpFire = true;
        fireResolveTime = Time.time + fireWindup;
        SetColor(fireTellColor);
    }

    private void FirePressureBolt()
    {
        windingUpFire = false;
        nextFireTime = Time.time + fireCooldown;
        RestoreColors();

        Vector3 origin = muzzle != null ? muzzle.position : transform.position + Vector3.up * 1.1f + transform.forward * 0.6f;
        Vector3 targetPoint = playerHealth.transform.position + Vector3.up * 0.8f;
        Vector3 direction = (targetPoint - origin).normalized;

        GameObject boltObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        boltObject.name = "Lancer Pressure Bolt";
        boltObject.transform.position = origin;
        boltObject.transform.localScale = new Vector3(0.22f, 0.22f, 0.22f);

        Collider boltCollider = boltObject.GetComponent<Collider>();
        if (boltCollider != null)
        {
            Destroy(boltCollider);
        }

        Renderer boltRenderer = boltObject.GetComponent<Renderer>();
        if (boltRenderer != null)
        {
            boltRenderer.material.color = new Color(1f, 0.52f, 0.12f);
        }

        PressureBolt bolt = boltObject.AddComponent<PressureBolt>();
        bolt.Initialize(direction, projectileDamage, projectileSpeed);
    }

    private void CancelFireWindup()
    {
        if (!windingUpFire)
        {
            return;
        }

        windingUpFire = false;
        RestoreColors();
    }

    private void Die()
    {
        dead = true;
        SteamworksAudio.PlayAt(SteamworksAudioCue.EnemyDeath, transform.position);
        HUDController.Instance?.ShowTemporaryMessage("Lancer down", 0.6f);
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
