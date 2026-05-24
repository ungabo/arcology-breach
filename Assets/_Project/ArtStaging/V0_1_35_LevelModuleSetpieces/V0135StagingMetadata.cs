using UnityEngine;

namespace BrassworksBreach.ArtStaging.V0135
{
    public sealed class V0135StagingMetadata : MonoBehaviour
    {
        public string package;
        public string version;
        public string family;
        public string displayName;
        public Vector3 approximateBoundsMeters;
        public int visualPartCount;
        [TextArea] public string colliderGuidance;
        [TextArea] public string integrationGuidance;
    }
}
