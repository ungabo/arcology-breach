using UnityEngine;

namespace BrassworksBreach.ObjectivePropsSet02
{
    [DisallowMultipleComponent]
    public sealed class ObjectivePropsSet02Identity : MonoBehaviour
    {
        public const string CurrentPackVersion = "v0.1.42";

        [SerializeField] private string packVersion = CurrentPackVersion;
        [SerializeField] private string assetId = string.Empty;
        [SerializeField] private string assetFamily = string.Empty;
        [SerializeField] private string assetRole = string.Empty;
        [SerializeField] private string readabilityCue = string.Empty;
        [SerializeField] private int rendererCount;
        [SerializeField] private string[] materialTags = new string[0];
        [SerializeField] private string[] safetyNotes = new string[0];

        public string PackVersion => packVersion;
        public string AssetId => assetId;
        public string AssetFamily => assetFamily;
        public string AssetRole => assetRole;
        public string ReadabilityCue => readabilityCue;
        public int RendererCount => rendererCount;
        public string[] MaterialTags => materialTags;
        public string[] SafetyNotes => safetyNotes;

        public void Configure(string id, string family, string role, string cue, int renderers, string[] tags, string[] notes)
        {
            packVersion = CurrentPackVersion;
            assetId = id;
            assetFamily = family;
            assetRole = role;
            readabilityCue = cue;
            rendererCount = renderers;
            materialTags = tags ?? new string[0];
            safetyNotes = notes ?? new string[0];
        }
    }
}
