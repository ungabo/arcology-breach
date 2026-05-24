using UnityEngine;

public class PressureGaugePrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.11";
    public string placementRole = "prototype";
    public Renderer bezelRenderer;
    public Renderer backplateRenderer;
    public Renderer faceRenderer;
    public Renderer glassRenderer;
    public Renderer warningBandRenderer;
    public Transform needlePivot;
    public Transform needle;
    public Transform tickRoot;
    public Transform rivetRoot;
    public int tickMarkCount = 16;

    public bool HasRequiredParts => bezelRenderer != null
        && backplateRenderer != null
        && faceRenderer != null
        && glassRenderer != null
        && warningBandRenderer != null
        && needlePivot != null
        && needle != null
        && tickRoot != null
        && rivetRoot != null
        && tickMarkCount >= 12;
}
