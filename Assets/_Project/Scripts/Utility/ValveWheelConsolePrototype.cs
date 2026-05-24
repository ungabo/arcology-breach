using UnityEngine;

public class ValveWheelConsolePrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.32";
    public string placementRole = "prototype";
    public string gameplayAuthority = "VisualOnlyNoGameplay";
    public Renderer backplateRenderer;
    public Renderer raisedPanelRenderer;
    public Renderer wheelRingRenderer;
    public Renderer wheelSpokeRenderer;
    public Renderer wheelHubRenderer;
    public Renderer wheelGripRenderer;
    public Renderer gaugeFaceRenderer;
    public Renderer gaugeNeedleRenderer;
    public Renderer lampRenderer;
    public Renderer pipeRenderer;
    public Renderer rivetRenderer;
    public Renderer grimeRenderer;
    public Transform backplateRoot;
    public Transform wheelRoot;
    public Transform gaugeRoot;
    public Transform lampRoot;
    public Transform pipeRoot;
    public Transform rivetRoot;
    public Transform grimeRoot;
    public int backplateCount = 2;
    public int wheelRingCount = 1;
    public int spokeCount = 6;
    public int gripCount = 4;
    public int gaugeCount = 2;
    public int lampCount = 2;
    public int pipeCount = 4;
    public int rivetCount = 14;
    public int grimeCount = 3;

    public bool HasRequiredParts => backplateRenderer != null
        && raisedPanelRenderer != null
        && wheelRingRenderer != null
        && wheelSpokeRenderer != null
        && wheelHubRenderer != null
        && wheelGripRenderer != null
        && gaugeFaceRenderer != null
        && gaugeNeedleRenderer != null
        && lampRenderer != null
        && pipeRenderer != null
        && rivetRenderer != null
        && grimeRenderer != null
        && backplateRoot != null
        && wheelRoot != null
        && gaugeRoot != null
        && lampRoot != null
        && pipeRoot != null
        && rivetRoot != null
        && grimeRoot != null
        && backplateCount >= 2
        && wheelRingCount >= 1
        && spokeCount >= 6
        && gripCount >= 4
        && gaugeCount >= 2
        && lampCount >= 2
        && pipeCount >= 4
        && rivetCount >= 14
        && grimeCount >= 3;
}
