using UnityEngine;

namespace BrassworksBreach.SteampunkWeapons
{
    [DisallowMultipleComponent]
    public sealed class SteampunkWeaponPackIdentity : MonoBehaviour
    {
        public const string CurrentPackVersion = "v0.1.37";

        [SerializeField] private string packVersion = CurrentPackVersion;
        [SerializeField] private string assetId = string.Empty;
        [SerializeField] private string assetRole = string.Empty;
        [SerializeField] private int rendererCount;
        [SerializeField] private string[] materialTags = new string[0];

        public string PackVersion => packVersion;
        public string AssetId => assetId;
        public string AssetRole => assetRole;
        public int RendererCount => rendererCount;
        public string[] MaterialTags => materialTags;

        public void Configure(string id, string role, int renderers, string[] tags)
        {
            packVersion = CurrentPackVersion;
            assetId = id;
            assetRole = role;
            rendererCount = renderers;
            materialTags = tags ?? new string[0];
        }
    }
}
