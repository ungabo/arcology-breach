using UnityEngine;

public class BoilerControlConsolePrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.19";
    public string placementRole = "prototype";
    public Renderer baseRenderer;
    public Renderer panelRenderer;
    public Renderer brassRailRenderer;
    public Renderer gaugeFaceRenderer;
    public Renderer leverHandleRenderer;
    public Renderer lampRenderer;
    public Renderer pipeRenderer;
    public Transform leverRoot;
    public Transform gaugeRoot;
    public Transform lampRoot;
    public Transform rivetRoot;
    public Transform pipeRoot;
    public int leverCount = 3;
    public int gaugeCount = 2;
    public int lampCount = 3;
    public int rivetCount = 12;
    public int pipeCount = 3;

    public bool HasRequiredParts => baseRenderer != null
        && panelRenderer != null
        && brassRailRenderer != null
        && gaugeFaceRenderer != null
        && leverHandleRenderer != null
        && lampRenderer != null
        && pipeRenderer != null
        && leverRoot != null
        && gaugeRoot != null
        && lampRoot != null
        && rivetRoot != null
        && pipeRoot != null
        && leverCount >= 3
        && gaugeCount >= 2
        && lampCount >= 3
        && rivetCount >= 12
        && pipeCount >= 3;
}
