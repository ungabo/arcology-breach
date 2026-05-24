using UnityEngine;

public class PressureReliefVentPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.25";
    public string placementRole = "prototype";
    public Renderer mountPlateRenderer;
    public Renderer ventStackRenderer;
    public Renderer reliefPipeRenderer;
    public Renderer rivetRenderer;
    public Renderer pressureTagRenderer;
    public Renderer steamPuffRenderer;
    public Transform mountRoot;
    public Transform ventRoot;
    public Transform reliefPipeRoot;
    public Transform rivetRoot;
    public Transform tagRoot;
    public Transform steamRoot;
    public int mountPlateCount = 1;
    public int ventStackCount = 1;
    public int reliefPipeCount = 1;
    public int rivetCount = 8;
    public int pressureTagCount = 1;
    public int steamPuffCount = 2;

    public bool HasRequiredParts => mountPlateRenderer != null
        && ventStackRenderer != null
        && reliefPipeRenderer != null
        && rivetRenderer != null
        && pressureTagRenderer != null
        && steamPuffRenderer != null
        && mountRoot != null
        && ventRoot != null
        && reliefPipeRoot != null
        && rivetRoot != null
        && tagRoot != null
        && steamRoot != null
        && mountPlateCount >= 1
        && ventStackCount >= 1
        && reliefPipeCount >= 1
        && rivetCount >= 8
        && pressureTagCount >= 1
        && steamPuffCount >= 2;
}
