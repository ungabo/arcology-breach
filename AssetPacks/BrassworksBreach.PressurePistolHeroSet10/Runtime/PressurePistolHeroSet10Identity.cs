using UnityEngine;

namespace BrassworksBreach.PressurePistolHeroSet10
{
    [DisallowMultipleComponent]
    public sealed class PressurePistolHeroSet10Identity : MonoBehaviour
    {
        [SerializeField] private string assetId = string.Empty;
        [SerializeField] private string componentRole = string.Empty;
        [SerializeField] private string acceptanceStatus = "component-pass";
        [SerializeField] private int rendererCount;
        [SerializeField] private bool visualOnly = true;
        [SerializeField] private string[] materialTags = new string[0];
        [TextArea]
        [SerializeField] private string notes = string.Empty;

        public string AssetId => assetId;
        public string ComponentRole => componentRole;
        public string AcceptanceStatus => acceptanceStatus;
        public int RendererCount => rendererCount;
        public bool VisualOnly => visualOnly;
        public string[] MaterialTags => materialTags;
        public string Notes => notes;

        public void Configure(
            string newAssetId,
            string newComponentRole,
            string newAcceptanceStatus,
            int newRendererCount,
            string[] newMaterialTags,
            string newNotes)
        {
            assetId = newAssetId;
            componentRole = newComponentRole;
            acceptanceStatus = newAcceptanceStatus;
            rendererCount = newRendererCount;
            visualOnly = true;
            materialTags = newMaterialTags ?? new string[0];
            notes = newNotes ?? string.Empty;
        }
    }
}
