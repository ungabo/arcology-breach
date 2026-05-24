using UnityEngine;

public class GearKeyPlinthPrototype : MonoBehaviour
{
    public string promotionVersion = "v0.1.31";
    public string placementRole = "prototype";
    public string gameplayAuthority = "ExistingGearKeyPickup";
    public Renderer baseRenderer;
    public Renderer cradleRenderer;
    public Renderer gearToothRenderer;
    public Renderer gaugeRenderer;
    public Renderer lampRenderer;
    public Renderer trimRenderer;
    public Renderer labelRenderer;
    public Renderer rivetRenderer;
    public Renderer grimeRenderer;
    public Transform baseRoot;
    public Transform cradleRoot;
    public Transform gaugeRoot;
    public Transform lampRoot;
    public Transform trimRoot;
    public Transform labelRoot;
    public Transform rivetRoot;
    public Transform grimeRoot;
    public int baseCount = 1;
    public int cradleCount = 1;
    public int gearToothCount = 8;
    public int gaugeCount = 1;
    public int lampCount = 1;
    public int trimCount = 2;
    public int labelCount = 1;
    public int rivetCount = 12;
    public int grimeCount = 3;

    public bool HasRequiredParts => baseRenderer != null
        && cradleRenderer != null
        && gearToothRenderer != null
        && gaugeRenderer != null
        && lampRenderer != null
        && trimRenderer != null
        && labelRenderer != null
        && rivetRenderer != null
        && grimeRenderer != null
        && baseRoot != null
        && cradleRoot != null
        && gaugeRoot != null
        && lampRoot != null
        && trimRoot != null
        && labelRoot != null
        && rivetRoot != null
        && grimeRoot != null
        && baseCount >= 1
        && cradleCount >= 1
        && gearToothCount >= 8
        && gaugeCount >= 1
        && lampCount >= 1
        && trimCount >= 2
        && labelCount >= 1
        && rivetCount >= 12
        && grimeCount >= 3;
}
