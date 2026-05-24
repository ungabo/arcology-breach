using UnityEngine;

public class WallValveWheelPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.24";
    public string placementRole = "prototype";
    public Renderer backplateRenderer;
    public Renderer wheelRenderer;
    public Renderer spokeRenderer;
    public Renderer hubRenderer;
    public Renderer rivetRenderer;
    public Renderer pointerRenderer;
    public Renderer labelPlateRenderer;
    public Transform backplateRoot;
    public Transform wheelRoot;
    public Transform rivetRoot;
    public Transform labelRoot;
    public int backplateCount = 1;
    public int wheelRingCount = 1;
    public int spokeCount = 4;
    public int hubCount = 1;
    public int rivetCount = 8;
    public int pointerCount = 1;
    public int labelPlateCount = 1;

    public bool HasRequiredParts => backplateRenderer != null
        && wheelRenderer != null
        && spokeRenderer != null
        && hubRenderer != null
        && rivetRenderer != null
        && pointerRenderer != null
        && labelPlateRenderer != null
        && backplateRoot != null
        && wheelRoot != null
        && rivetRoot != null
        && labelRoot != null
        && backplateCount >= 1
        && wheelRingCount >= 1
        && spokeCount >= 4
        && hubCount >= 1
        && rivetCount >= 8
        && pointerCount >= 1
        && labelPlateCount >= 1;
}
