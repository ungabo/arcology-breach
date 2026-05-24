using UnityEngine;

public class ThresholdRouteDressingPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.33";
    public string batchId = "v0.1.33_threshold_route_dressing";
    public string componentFamily = "prototype";
    public string placementRole = "prototype";
    public string gameplayAuthority = "VisualOnlyNoGameplay";
    public Renderer primaryRenderer;
    public Renderer secondaryRenderer;
    public Renderer accentRenderer;
    public Renderer grimeRenderer;
    public Transform structureRoot;
    public Transform detailRoot;
    public Transform signalRoot;
    public Transform grimeRoot;
    public int structureCount;
    public int detailCount;
    public int signalCount;
    public int grimeCount;

    public bool HasRequiredParts => !string.IsNullOrEmpty(componentFamily)
        && !string.IsNullOrEmpty(placementRole)
        && !string.IsNullOrEmpty(batchId)
        && primaryRenderer != null
        && secondaryRenderer != null
        && accentRenderer != null
        && grimeRenderer != null
        && structureRoot != null
        && detailRoot != null
        && signalRoot != null
        && grimeRoot != null
        && structureCount >= 1
        && detailCount >= 1
        && signalCount >= 1
        && grimeCount >= 1;
}
