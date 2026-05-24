using UnityEngine;

public class CagedGaslightPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.21";
    public string placementRole = "prototype";
    public Renderer backplateRenderer;
    public Renderer bracketRenderer;
    public Renderer topCapRenderer;
    public Renderer bottomCapRenderer;
    public Renderer amberGlassRenderer;
    public Renderer warmCoreRenderer;
    public Renderer cageRibRenderer;
    public Renderer pipeFeedRenderer;
    public Renderer rivetRenderer;
    public Light warmPointLight;
    public Transform mountRoot;
    public Transform capRoot;
    public Transform globeRoot;
    public Transform cageRoot;
    public Transform pipeFeedRoot;
    public Transform rivetRoot;
    public Transform lightRoot;
    public int amberGlassGlobeCount = 1;
    public int cageRibCount = 4;
    public int brassCapCount = 2;
    public int mountingBracketCount = 1;
    public int warmLightEmitterCount = 1;
    public int rivetCount = 6;
    public int pipeFeedCount = 1;

    public bool HasRequiredParts => backplateRenderer != null
        && bracketRenderer != null
        && topCapRenderer != null
        && bottomCapRenderer != null
        && amberGlassRenderer != null
        && warmCoreRenderer != null
        && cageRibRenderer != null
        && pipeFeedRenderer != null
        && rivetRenderer != null
        && warmPointLight != null
        && mountRoot != null
        && capRoot != null
        && globeRoot != null
        && cageRoot != null
        && pipeFeedRoot != null
        && rivetRoot != null
        && lightRoot != null
        && amberGlassGlobeCount >= 1
        && cageRibCount >= 4
        && brassCapCount >= 2
        && mountingBracketCount >= 1
        && warmLightEmitterCount >= 1
        && rivetCount >= 6
        && pipeFeedCount >= 1;
}
