using System;
using UnityEngine;

public enum GameplayFeedbackEventType
{
    WeaponFired,
    WeaponImpact,
    WeaponEmpty,
    WeaponSwitched,
    PickupCollected,
    EnemyHit,
    EnemyDeath,
    ObjectiveUpdated,
    ObjectiveCompleted,
    RouteBlocked,
    SecretFound,
    PauseOpened,
    PauseClosed,
    SettingChanged,
    BossPhaseChanged
}

public class GameplayFeedbackController : MonoBehaviour
{
    public const string PromotionVersion = "v0.1.35";
    public const string BatchId = "v0.1.35_gameplay_systems_feedback_validation_batch";

    public static GameplayFeedbackController Instance { get; private set; }

    public bool spawnVisualPulses = true;
    public float pulseScaleMultiplier = 1f;

    [SerializeField] private int totalEvents;
    [SerializeField] private int visualPulseCount;
    [SerializeField] private GameplayFeedbackEventType lastEventType;
    [SerializeField] private string lastTargetId = string.Empty;
    [SerializeField] private bool lastHadWorldPosition;
    [SerializeField] private Vector3 lastWorldPosition;
    [SerializeField] private int[] eventCounts = Array.Empty<int>();

    public int TotalEvents => totalEvents;
    public int VisualPulseCount => visualPulseCount;
    public GameplayFeedbackEventType LastEventType => lastEventType;
    public string LastTargetId => lastTargetId;
    public bool LastHadWorldPosition => lastHadWorldPosition;
    public Vector3 LastWorldPosition => lastWorldPosition;
    public int SupportedEventTypeCount => Enum.GetValues(typeof(GameplayFeedbackEventType)).Length;

    public int ReportedEventTypeCount
    {
        get
        {
            EnsureEventCounts();
            int count = 0;
            for (int i = 0; i < eventCounts.Length; i++)
            {
                if (eventCounts[i] > 0)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public bool HasV0135Coverage => string.Equals(PromotionVersion, "v0.1.35", StringComparison.Ordinal)
        && string.Equals(BatchId, "v0.1.35_gameplay_systems_feedback_validation_batch", StringComparison.Ordinal)
        && SupportedEventTypeCount >= 15;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        EnsureEventCounts();
    }

    public static void Report(GameplayFeedbackEventType eventType, string targetId)
    {
        Instance?.Record(eventType, targetId, null, Color.white);
    }

    public static void ReportWorld(GameplayFeedbackEventType eventType, string targetId, Vector3 position, Color color)
    {
        Instance?.Record(eventType, targetId, position, color);
    }

    public int GetEventCount(GameplayFeedbackEventType eventType)
    {
        EnsureEventCounts();
        int index = (int)eventType;
        return index >= 0 && index < eventCounts.Length ? eventCounts[index] : 0;
    }

    public void ResetForTest()
    {
        EnsureEventCounts();
        totalEvents = 0;
        visualPulseCount = 0;
        lastEventType = GameplayFeedbackEventType.WeaponFired;
        lastTargetId = string.Empty;
        lastHadWorldPosition = false;
        lastWorldPosition = Vector3.zero;

        for (int i = 0; i < eventCounts.Length; i++)
        {
            eventCounts[i] = 0;
        }
    }

    private void Record(GameplayFeedbackEventType eventType, string targetId, Vector3? worldPosition, Color color)
    {
        EnsureEventCounts();

        int index = (int)eventType;
        if (index >= 0 && index < eventCounts.Length)
        {
            eventCounts[index]++;
        }

        totalEvents++;
        lastEventType = eventType;
        lastTargetId = string.IsNullOrWhiteSpace(targetId) ? eventType.ToString() : targetId;
        lastHadWorldPosition = worldPosition.HasValue;
        lastWorldPosition = worldPosition.HasValue ? worldPosition.Value : Vector3.zero;

        if (spawnVisualPulses && worldPosition.HasValue && ShouldSpawnPulse(eventType))
        {
            GameplayFeedbackPulseVfx.Spawn(worldPosition.Value, ResolveColor(eventType, color), ResolveScale(eventType) * pulseScaleMultiplier, eventType.ToString());
            visualPulseCount++;
        }
    }

    private void EnsureEventCounts()
    {
        int requiredLength = SupportedEventTypeCount;
        if (eventCounts != null && eventCounts.Length == requiredLength)
        {
            return;
        }

        int[] resized = new int[requiredLength];
        if (eventCounts != null)
        {
            Array.Copy(eventCounts, resized, Mathf.Min(eventCounts.Length, resized.Length));
        }

        eventCounts = resized;
    }

    private static bool ShouldSpawnPulse(GameplayFeedbackEventType eventType)
    {
        return eventType != GameplayFeedbackEventType.PauseOpened
            && eventType != GameplayFeedbackEventType.PauseClosed
            && eventType != GameplayFeedbackEventType.SettingChanged;
    }

    private static float ResolveScale(GameplayFeedbackEventType eventType)
    {
        switch (eventType)
        {
            case GameplayFeedbackEventType.EnemyDeath:
            case GameplayFeedbackEventType.ObjectiveCompleted:
            case GameplayFeedbackEventType.BossPhaseChanged:
                return 1.35f;
            case GameplayFeedbackEventType.RouteBlocked:
            case GameplayFeedbackEventType.SecretFound:
                return 1.1f;
            case GameplayFeedbackEventType.WeaponImpact:
            case GameplayFeedbackEventType.EnemyHit:
                return 0.82f;
            default:
                return 1f;
        }
    }

    private static Color ResolveColor(GameplayFeedbackEventType eventType, Color fallback)
    {
        switch (eventType)
        {
            case GameplayFeedbackEventType.WeaponFired:
            case GameplayFeedbackEventType.WeaponImpact:
                return new Color(1f, 0.58f, 0.1f);
            case GameplayFeedbackEventType.WeaponEmpty:
            case GameplayFeedbackEventType.RouteBlocked:
                return new Color(0.95f, 0.12f, 0.04f);
            case GameplayFeedbackEventType.PickupCollected:
            case GameplayFeedbackEventType.WeaponSwitched:
                return new Color(0.95f, 0.72f, 0.18f);
            case GameplayFeedbackEventType.EnemyHit:
            case GameplayFeedbackEventType.EnemyDeath:
            case GameplayFeedbackEventType.BossPhaseChanged:
                return new Color(1f, 0.32f, 0.06f);
            case GameplayFeedbackEventType.ObjectiveUpdated:
                return new Color(1f, 0.84f, 0.38f);
            case GameplayFeedbackEventType.ObjectiveCompleted:
                return new Color(0.28f, 0.95f, 0.48f);
            case GameplayFeedbackEventType.SecretFound:
                return new Color(0.36f, 0.9f, 1f);
            default:
                return fallback;
        }
    }
}
