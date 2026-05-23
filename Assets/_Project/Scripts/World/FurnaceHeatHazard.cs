using UnityEngine;

public class FurnaceHeatHazard : MonoBehaviour
{
    public int damagePerTick = GameBalance.FurnaceHeatHazardDamage;
    public float tickInterval = GameBalance.FurnaceHeatHazardTickInterval;
    public float warningDuration = GameBalance.FurnaceHeatHazardWarningDuration;
    public float activeDuration = GameBalance.FurnaceHeatHazardActiveDuration;
    public float cooldownDuration = GameBalance.FurnaceHeatHazardCooldownDuration;
    public float phaseOffset;
    public string damageMessage = "FURNACE HEAT SURGE";
    public GameObject warningSignal;
    public GameObject activeSignal;
    public GameObject safeSignal;

    private float nextDamageTime;
    private float forceActiveUntil = -1f;
    private bool isWarning;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        Collider triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }

        UpdateState();
    }

    private void Update()
    {
        UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamage(other.gameObject);
    }

    public void ForceActiveForTest(float duration)
    {
        forceActiveUntil = Time.time + Mathf.Max(0.05f, duration);
        UpdateState();
    }

    public bool TryDamage(GameObject target)
    {
        UpdateState();
        if (!IsActive || target == null || Time.time < nextDamageTime)
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

    private void UpdateState()
    {
        bool forced = Time.time < forceActiveUntil;
        float cycleDuration = Mathf.Max(0.1f, warningDuration + activeDuration + cooldownDuration);
        float cycleTime = Mathf.Repeat(Time.time + phaseOffset, cycleDuration);

        isWarning = !forced && cycleTime < warningDuration;
        IsActive = forced || cycleTime >= warningDuration && cycleTime < warningDuration + activeDuration;

        if (warningSignal != null)
        {
            warningSignal.SetActive(isWarning);
        }

        if (activeSignal != null)
        {
            activeSignal.SetActive(IsActive);
        }

        if (safeSignal != null)
        {
            safeSignal.SetActive(!isWarning && !IsActive);
        }
    }
}
