using UnityEngine;

namespace BrassworksBreach.WeaponViewmodelSet03
{
    [DisallowMultipleComponent]
    public sealed class WeaponViewmodelSet03Identity : MonoBehaviour
    {
        public const string CurrentPackVersion = "v0.1.41";

        [SerializeField] private string packVersion = CurrentPackVersion;
        [SerializeField] private string assetId = string.Empty;
        [SerializeField] private string assetRole = string.Empty;
        [SerializeField] private int rendererCount;
        [SerializeField] private string[] materialTags = new string[0];
        [SerializeField] private string[] promotionNotes = new string[0];

        public string PackVersion => packVersion;
        public string AssetId => assetId;
        public string AssetRole => assetRole;
        public int RendererCount => rendererCount;
        public string[] MaterialTags => materialTags;
        public string[] PromotionNotes => promotionNotes;

        public void Configure(string id, string role, int renderers, string[] tags, string[] notes)
        {
            packVersion = CurrentPackVersion;
            assetId = id;
            assetRole = role;
            rendererCount = renderers;
            materialTags = tags ?? new string[0];
            promotionNotes = notes ?? new string[0];
        }
    }
}
