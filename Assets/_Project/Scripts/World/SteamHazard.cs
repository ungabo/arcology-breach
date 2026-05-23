using UnityEngine;

public class SteamHazard : MonoBehaviour
{
    public int damagePerTick = GameBalance.SteamHazardDamage;
    public float tickInterval = GameBalance.SteamHazardTickInterval;
    public string damageMessage = "SCALDING STEAM";

    private float nextDamageTime;

    private void Awake()
    {
        Collider triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamage(other.gameObject);
    }

    public bool TryDamage(GameObject target)
    {
        if (target == null || Time.time < nextDamageTime)
        {
            return false;
        }

        PlayerHealth health = target.GetComponentInParent<PlayerHealth>();
        if (health == null || health.IsDead)
        {
            return false;
        }

        nextDamageTime = Time.time + tickInterval;
        health.TakeDamage(damagePerTick);
        HUDController.Instance?.ShowTemporaryMessage(damageMessage, 0.55f);
        return true;
    }
}
