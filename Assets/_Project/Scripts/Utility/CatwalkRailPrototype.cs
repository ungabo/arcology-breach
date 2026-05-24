using UnityEngine;

public class CatwalkRailPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.26";
    public string placementRole = "prototype";
    public Renderer upperRailRenderer;
    public Renderer lowerRailRenderer;
    public Renderer uprightRenderer;
    public Renderer capRenderer;
    public Renderer footPlateRenderer;
    public Renderer rivetRenderer;
    public Transform railRoot;
    public Transform uprightRoot;
    public Transform capRoot;
    public Transform footRoot;
    public Transform rivetRoot;
    public int upperRailCount = 1;
    public int lowerRailCount = 1;
    public int uprightCount = 5;
    public int capCount = 5;
    public int footPlateCount = 5;
    public int rivetCount = 10;

    public bool HasRequiredParts => upperRailRenderer != null
        && lowerRailRenderer != null
        && uprightRenderer != null
        && capRenderer != null
        && footPlateRenderer != null
        && rivetRenderer != null
        && railRoot != null
        && uprightRoot != null
        && capRoot != null
        && footRoot != null
        && rivetRoot != null
        && upperRailCount >= 1
        && lowerRailCount >= 1
        && uprightCount >= 5
        && capCount >= 5
        && footPlateCount >= 5
        && rivetCount >= 10;
}
