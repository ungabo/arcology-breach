using UnityEngine;

public class PressureBolt : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 8;
    public float lifetime = 3f;
    public float hitRadius = 0.55f;
    public float worldHitRadius = 0.14f;

    private Vector3 direction = Vector3.forward;
    private PlayerHealth target;
    private float expireTime;
    private float impactScale = 1f;

    public void Initialize(Vector3 fireDirection, int boltDamage, float boltSpeed)
    {
        direction = fireDirection.sqrMagnitude > 0.001f ? fireDirection.normalized : Vector3.forward;
        damage = boltDamage;
        speed = boltSpeed;
        impactScale = Mathf.Clamp(boltSpeed / GameBalance.LancerProjectileSpeed, 0.85f, 1.35f);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Start()
    {
        target = Object.FindAnyObjectByType<PlayerHealth>();
        expireTime = Time.time + lifetime;
    }

    private void Update()
    {
        Vector3 startPosition = transform.position;
        Vector3 travel = direction * speed * Time.deltaTime;
        if (TryResolveSolidImpact(startPosition, travel))
        {
            return;
        }

        transform.position = startPosition + travel;

        if (target != null && !target.IsDead && Vector3.Distance(transform.position, target.transform.position + Vector3.up * 0.8f) <= hitRadius)
        {
            SpawnPlayerImpact(target.transform.position + Vector3.up * 0.8f);
            target.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (Time.time >= expireTime)
        {
            Destroy(gameObject);
        }
    }

    private bool TryResolveSolidImpact(Vector3 startPosition, Vector3 travel)
    {
        float distance = travel.magnitude;
        if (distance <= 0.001f)
        {
            return false;
        }

        RaycastHit[] hits = Physics.SphereCastAll(startPosition, worldHitRadius, direction, distance, ~0, QueryTriggerInteraction.Ignore);
        bool foundHit = false;
        RaycastHit resolvedHit = default(RaycastHit);
        float closestDistance = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            if (ShouldIgnoreHit(hits[i].collider))
            {
                continue;
            }

            if (hits[i].distance < closestDistance)
            {
                foundHit = true;
                resolvedHit = hits[i];
                closestDistance = hits[i].distance;
            }
        }

        if (!foundHit)
        {
            return false;
        }

        ResolveImpact(resolvedHit, startPosition);
        return true;
    }

    private void ResolveImpact(RaycastHit hit, Vector3 fallbackPosition)
    {
        PlayerHealth hitPlayer = hit.collider.GetComponentInParent<PlayerHealth>();
        Vector3 normal = hit.normal.sqrMagnitude > 0.001f ? hit.normal : -direction;
        Vector3 point = hit.point.sqrMagnitude > 0.001f ? hit.point : fallbackPosition + direction * Mathf.Max(hit.distance, 0.05f);

        if (hitPlayer != null && !hitPlayer.IsDead)
        {
            PressureBoltImpactVfx.Spawn(point, normal, GetImpactColor(), impactScale);
            hitPlayer.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        PressureBoltImpactVfx.Spawn(point, normal, GetImpactColor(), impactScale);
        Destroy(gameObject);
    }

    private void SpawnPlayerImpact(Vector3 playerCenter)
    {
        Vector3 normal = -direction;
        Vector3 point = playerCenter + normal * Mathf.Min(hitRadius * 0.5f, 0.28f);
        PressureBoltImpactVfx.Spawn(point, normal, GetImpactColor(), impactScale);
    }

    private bool ShouldIgnoreHit(Collider hitCollider)
    {
        if (hitCollider == null)
        {
            return true;
        }

        if (hitCollider.transform.IsChildOf(transform))
        {
            return true;
        }

        if (hitCollider.GetComponentInParent<PressureBolt>() == this)
        {
            return true;
        }

        if (hitCollider.GetComponentInParent<PlayerHealth>() != null)
        {
            return false;
        }

        return hitCollider.GetComponentInParent<EnemyController>() != null
            || hitCollider.GetComponentInParent<RangedEnemyController>() != null
            || hitCollider.GetComponentInParent<BulwarkEnemyController>() != null
            || hitCollider.GetComponentInParent<BellowsNodeController>() != null
            || hitCollider.GetComponentInParent<GovernorWardenController>() != null;
    }

    private Color GetImpactColor()
    {
        Renderer boltRenderer = GetComponent<Renderer>();
        return boltRenderer != null ? boltRenderer.material.color : new Color(1f, 0.5f, 0.08f);
    }
}
