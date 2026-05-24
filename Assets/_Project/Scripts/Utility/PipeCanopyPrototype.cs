using UnityEngine;

public class PipeCanopyPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.22";
    public string placementRole = "prototype";
    public Renderer pipeRenderer;
    public Renderer collarRenderer;
    public Renderer couplerRenderer;
    public Renderer valveRenderer;
    public Renderer rivetRenderer;
    public Transform pipeRoot;
    public Transform collarRoot;
    public Transform couplerRoot;
    public Transform valveRoot;
    public Transform rivetRoot;
    public int pipeCount = 4;
    public int collarCount = 5;
    public int couplerCount = 2;
    public int valveDetailCount = 1;
    public int rivetCount = 10;

    public bool HasRequiredParts => pipeRenderer != null
        && collarRenderer != null
        && couplerRenderer != null
        && valveRenderer != null
        && rivetRenderer != null
        && pipeRoot != null
        && collarRoot != null
        && couplerRoot != null
        && valveRoot != null
        && rivetRoot != null
        && pipeCount >= 4
        && collarCount >= 5
        && couplerCount >= 2
        && valveDetailCount >= 1
        && rivetCount >= 10;
}
