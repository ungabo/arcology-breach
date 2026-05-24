using UnityEngine;

namespace BrassworksBreach.SteamVFXSet02
{
    [DisallowMultipleComponent]
    public sealed class SteamVfxSet02Identity : MonoBehaviour
    {
        public const string CurrentPackVersion = "v0.1.42";

        [SerializeField] private string packVersion = CurrentPackVersion;
        [SerializeField] private string vfxId = string.Empty;
        [SerializeField] private string family = string.Empty;
        [SerializeField] private string visualIntent = string.Empty;
        [SerializeField] private string recommendedSocket = string.Empty;
        [SerializeField] private float recommendedLifetimeSeconds = 1f;
        [SerializeField] private float recommendedWorldScale = 1f;
        [SerializeField] private bool looping = false;
        [SerializeField] private bool visualOnly = true;
        [SerializeField] private bool requiresExternalLifetimeOwner = true;

        public string PackVersion => packVersion;
        public string VfxId => vfxId;
        public string Family => family;
        public string VisualIntent => visualIntent;
        public string RecommendedSocket => recommendedSocket;
        public float RecommendedLifetimeSeconds => recommendedLifetimeSeconds;
        public float RecommendedWorldScale => recommendedWorldScale;
        public bool Looping => looping;
        public bool VisualOnly => visualOnly;
        public bool RequiresExternalLifetimeOwner => requiresExternalLifetimeOwner;

        public void Configure(
            string newVfxId,
            string newFamily,
            string newVisualIntent,
            string newRecommendedSocket,
            float newRecommendedLifetimeSeconds,
            float newRecommendedWorldScale,
            bool newLooping)
        {
            packVersion = CurrentPackVersion;
            vfxId = newVfxId;
            family = newFamily;
            visualIntent = newVisualIntent;
            recommendedSocket = newRecommendedSocket;
            recommendedLifetimeSeconds = Mathf.Max(0.05f, newRecommendedLifetimeSeconds);
            recommendedWorldScale = Mathf.Max(0.05f, newRecommendedWorldScale);
            looping = newLooping;
            visualOnly = true;
            requiresExternalLifetimeOwner = true;
        }
    }
}
