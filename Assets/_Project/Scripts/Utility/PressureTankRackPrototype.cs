using UnityEngine;

public class PressureTankRackPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.28";
    public string placementRole = "prototype";
    public Renderer rackFrameRenderer;
    public Renderer tankRenderer;
    public Renderer brassBandRenderer;
    public Renderer feederPipeRenderer;
    public Renderer valveRenderer;
    public Renderer rivetRenderer;
    public Renderer pressureTagRenderer;
    public Renderer steamSeepRenderer;
    public Transform rackRoot;
    public Transform tankRoot;
    public Transform bandRoot;
    public Transform feederPipeRoot;
    public Transform valveRoot;
    public Transform rivetRoot;
    public Transform tagRoot;
    public Transform steamRoot;
    public int rackFrameCount = 4;
    public int tankCount = 3;
    public int brassBandCount = 6;
    public int feederPipeCount = 3;
    public int valveCount = 3;
    public int rivetCount = 12;
    public int pressureTagCount = 1;
    public int steamSeepCount = 2;

    public bool HasRequiredParts => rackFrameRenderer != null
        && tankRenderer != null
        && brassBandRenderer != null
        && feederPipeRenderer != null
        && valveRenderer != null
        && rivetRenderer != null
        && pressureTagRenderer != null
        && steamSeepRenderer != null
        && rackRoot != null
        && tankRoot != null
        && bandRoot != null
        && feederPipeRoot != null
        && valveRoot != null
        && rivetRoot != null
        && tagRoot != null
        && steamRoot != null
        && rackFrameCount >= 4
        && tankCount >= 3
        && brassBandCount >= 6
        && feederPipeCount >= 3
        && valveCount >= 3
        && rivetCount >= 12
        && pressureTagCount >= 1
        && steamSeepCount >= 2;
}
