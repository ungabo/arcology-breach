using UnityEngine;

public class WallPipeGaugeClusterPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.17";
    public string placementRole = "prototype";
    public Renderer backplateRenderer;
    public Renderer primaryPipeRenderer;
    public Renderer secondaryPipeRenderer;
    public Renderer gaugeFaceRenderer;
    public Renderer valveWheelRenderer;
    public Transform pipeRoot;
    public Transform gaugeRoot;
    public Transform valveRoot;
    public Transform rivetRoot;
    public int pipeCount = 5;
    public int gaugeCount = 2;
    public int valveCount = 1;
    public int rivetCount = 14;

    public bool HasRequiredParts => backplateRenderer != null
        && primaryPipeRenderer != null
        && secondaryPipeRenderer != null
        && gaugeFaceRenderer != null
        && valveWheelRenderer != null
        && pipeRoot != null
        && gaugeRoot != null
        && valveRoot != null
        && rivetRoot != null
        && pipeCount >= 5
        && gaugeCount >= 2
        && valveCount >= 1
        && rivetCount >= 12;
}
