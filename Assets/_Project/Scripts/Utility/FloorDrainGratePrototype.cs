using UnityEngine;

public class FloorDrainGratePrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.27";
    public string placementRole = "prototype";
    public Renderer frameRenderer;
    public Renderer brassTrimRenderer;
    public Renderer grateBarRenderer;
    public Renderer rivetRenderer;
    public Renderer oilStainRenderer;
    public Renderer steamSeepRenderer;
    public Transform frameRoot;
    public Transform trimRoot;
    public Transform grateRoot;
    public Transform rivetRoot;
    public Transform stainRoot;
    public Transform steamRoot;
    public int framePlateCount = 4;
    public int brassTrimCount = 4;
    public int grateBarCount = 6;
    public int rivetCount = 8;
    public int oilStainCount = 2;
    public int steamSeepCount = 2;

    public bool HasRequiredParts => frameRenderer != null
        && brassTrimRenderer != null
        && grateBarRenderer != null
        && rivetRenderer != null
        && oilStainRenderer != null
        && steamSeepRenderer != null
        && frameRoot != null
        && trimRoot != null
        && grateRoot != null
        && rivetRoot != null
        && stainRoot != null
        && steamRoot != null
        && framePlateCount >= 4
        && brassTrimCount >= 4
        && grateBarCount >= 6
        && rivetCount >= 8
        && oilStainCount >= 2
        && steamSeepCount >= 2;
}
