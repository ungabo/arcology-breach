using UnityEngine;

public class PressureCoilPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.14";
    public string placementRole = "prototype";
    public Renderer backingPlateRenderer;
    public Renderer upperRailRenderer;
    public Renderer lowerRailRenderer;
    public Renderer heatCoreRenderer;
    public Transform coilTurnRoot;
    public Transform rivetRoot;
    public Transform pressureLeadRoot;
    public int coilTurnCount = 9;
    public int rivetCount = 16;

    public bool HasRequiredParts => backingPlateRenderer != null
        && upperRailRenderer != null
        && lowerRailRenderer != null
        && heatCoreRenderer != null
        && coilTurnRoot != null
        && rivetRoot != null
        && pressureLeadRoot != null
        && coilTurnCount >= 8
        && rivetCount >= 12;
}
