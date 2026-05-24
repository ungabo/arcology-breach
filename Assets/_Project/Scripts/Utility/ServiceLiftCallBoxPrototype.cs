using UnityEngine;

public class ServiceLiftCallBoxPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.30";
    public string placementRole = "prototype";
    public Renderer backplateRenderer;
    public Renderer leverRenderer;
    public Renderer gaugeRenderer;
    public Renderer lampRenderer;
    public Renderer pipeRenderer;
    public Renderer labelRenderer;
    public Renderer rivetRenderer;
    public Renderer grimeRenderer;
    public Transform backplateRoot;
    public Transform leverRoot;
    public Transform gaugeRoot;
    public Transform lampRoot;
    public Transform pipeRoot;
    public Transform labelRoot;
    public Transform rivetRoot;
    public Transform grimeRoot;
    public int backplateCount = 1;
    public int leverCount = 1;
    public int gaugeCount = 1;
    public int lampCount = 2;
    public int pipeCount = 2;
    public int labelCount = 1;
    public int rivetCount = 8;
    public int grimeCount = 2;

    public bool HasRequiredParts => backplateRenderer != null
        && leverRenderer != null
        && gaugeRenderer != null
        && lampRenderer != null
        && pipeRenderer != null
        && labelRenderer != null
        && rivetRenderer != null
        && grimeRenderer != null
        && backplateRoot != null
        && leverRoot != null
        && gaugeRoot != null
        && lampRoot != null
        && pipeRoot != null
        && labelRoot != null
        && rivetRoot != null
        && grimeRoot != null
        && backplateCount >= 1
        && leverCount >= 1
        && gaugeCount >= 1
        && lampCount >= 2
        && pipeCount >= 2
        && labelCount >= 1
        && rivetCount >= 8
        && grimeCount >= 2;
}
