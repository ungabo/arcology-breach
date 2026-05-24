using UnityEngine;

public class RivetBandPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.23";
    public string placementRole = "prototype";
    public Renderer backingRailRenderer;
    public Renderer faceRailRenderer;
    public Renderer endCapRenderer;
    public Renderer rivetRenderer;
    public Renderer pressurePlateRenderer;
    public Transform railRoot;
    public Transform endCapRoot;
    public Transform rivetRoot;
    public Transform pressurePlateRoot;
    public int backingRailCount = 1;
    public int faceRailCount = 1;
    public int endCapCount = 2;
    public int rivetCount = 8;
    public int pressurePlateCount = 1;

    public bool HasRequiredParts => backingRailRenderer != null
        && faceRailRenderer != null
        && endCapRenderer != null
        && rivetRenderer != null
        && pressurePlateRenderer != null
        && railRoot != null
        && endCapRoot != null
        && rivetRoot != null
        && pressurePlateRoot != null
        && backingRailCount >= 1
        && faceRailCount >= 1
        && endCapCount >= 2
        && rivetCount >= 8
        && pressurePlateCount >= 1;
}
