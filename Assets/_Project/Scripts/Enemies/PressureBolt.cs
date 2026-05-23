using UnityEngine;

public class PressureBolt : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 8;
    public float lifetime = 3f;
    public float hitRadius = 0.55f;

    private Vector3 direction = Vector3.forward;
    private PlayerHealth target;
    private float expireTime;

    public void Initialize(Vector3 fireDirection, int boltDamage, float boltSpeed)
    {
        direction = fireDirection.sqrMagnitude > 0.001f ? fireDirection.normalized : Vector3.forward;
        damage = boltDamage;
        speed = boltSpeed;
    }

    private void Start()
    {
        target = Object.FindAnyObjectByType<PlayerHealth>();
        expireTime = Time.time + lifetime;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (target != null && !target.IsDead && Vector3.Distance(transform.position, target.transform.position + Vector3.up * 0.8f) <= hitRadius)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (Time.time >= expireTime)
        {
            Destroy(gameObject);
        }
    }
}
