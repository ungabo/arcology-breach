using UnityEngine;

public class RivetedPressureDoorFramePrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.20";
    public string placementRole = "prototype";
    public Renderer archRenderer;
    public Renderer brassRibRenderer;
    public Renderer gearHubRenderer;
    public Renderer pressureCylinderRenderer;
    public Renderer gaugeFaceRenderer;
    public Renderer lampRenderer;
    public Renderer crossBraceRenderer;
    public Transform archRoot;
    public Transform brassRibRoot;
    public Transform gearRoot;
    public Transform cylinderRoot;
    public Transform gaugeRoot;
    public Transform lampRoot;
    public Transform rivetRoot;
    public Transform crossBraceRoot;
    public int pressureCylinderCount = 2;
    public int warningLampCount = 2;
    public int pressureGaugeCount = 1;
    public int gearHubCount = 1;
    public int rivetCount = 20;
    public int crossBraceCount = 2;

    public bool HasRequiredParts => archRenderer != null
        && brassRibRenderer != null
        && gearHubRenderer != null
        && pressureCylinderRenderer != null
        && gaugeFaceRenderer != null
        && lampRenderer != null
        && crossBraceRenderer != null
        && archRoot != null
        && brassRibRoot != null
        && gearRoot != null
        && cylinderRoot != null
        && gaugeRoot != null
        && lampRoot != null
        && rivetRoot != null
        && crossBraceRoot != null
        && pressureCylinderCount >= 2
        && warningLampCount >= 2
        && pressureGaugeCount >= 1
        && gearHubCount >= 1
        && rivetCount >= 16
        && crossBraceCount >= 2;
}
