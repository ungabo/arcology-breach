using UnityEngine;

namespace BrassworksBreach.FeedbackFXAudio
{
    [DisallowMultipleComponent]
    public sealed class FeedbackFxAudioCueIdentity : MonoBehaviour
    {
        public const string CurrentPackVersion = "v0.1.38";

        [SerializeField] private string packVersion = CurrentPackVersion;
        [SerializeField] private string eventType = string.Empty;
        [SerializeField] private string cueId = string.Empty;
        [SerializeField] private string eventFamily = string.Empty;
        [SerializeField] private string visualIntent = string.Empty;
        [SerializeField] private string audioIntent = string.Empty;
        [SerializeField] private float recommendedWorldScale = 1f;
        [SerializeField] private bool prefersWorldPosition = true;

        public string PackVersion => packVersion;
        public string EventType => eventType;
        public string CueId => cueId;
        public string EventFamily => eventFamily;
        public string VisualIntent => visualIntent;
        public string AudioIntent => audioIntent;
        public float RecommendedWorldScale => recommendedWorldScale;
        public bool PrefersWorldPosition => prefersWorldPosition;

        public void Configure(
            string feedbackEventType,
            string feedbackCueId,
            string feedbackFamily,
            string visualDescription,
            string audioDescription,
            float worldScale,
            bool worldPositionPreferred)
        {
            packVersion = CurrentPackVersion;
            eventType = feedbackEventType;
            cueId = feedbackCueId;
            eventFamily = feedbackFamily;
            visualIntent = visualDescription;
            audioIntent = audioDescription;
            recommendedWorldScale = Mathf.Max(0.05f, worldScale);
            prefersWorldPosition = worldPositionPreferred;
        }
    }
}
