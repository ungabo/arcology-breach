using UnityEngine;

public class V0134BatchPolishPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.34";
    public string batchId = "v0.1.34_playable_polish_leap";
    public string category = "prototype";
    public string targetId = "prototype";
    public string placementRole = "prototype";
    public string gameplayAuthority = "VisualOnlyNoGameplay";
    public Transform structureRoot;
    public Transform signalRoot;
    public Transform wearRoot;
    public Renderer primaryRenderer;
    public Renderer signalRenderer;
    public Renderer wearRenderer;
    public int structureCount;
    public int signalCount;
    public int wearCount;

    public bool HasRequiredParts => !string.IsNullOrEmpty(category)
        && !string.IsNullOrEmpty(targetId)
        && !string.IsNullOrEmpty(placementRole)
        && !string.IsNullOrEmpty(batchId)
        && gameplayAuthority == "VisualOnlyNoGameplay"
        && structureRoot != null
        && signalRoot != null
        && wearRoot != null
        && primaryRenderer != null
        && signalRenderer != null
        && wearRenderer != null
        && structureCount >= 2
        && signalCount >= 1
        && wearCount >= 1;
}
