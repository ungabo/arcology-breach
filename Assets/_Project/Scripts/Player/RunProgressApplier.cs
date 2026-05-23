using UnityEngine;

public class RunProgressApplier : MonoBehaviour
{
    private void Start()
    {
        RunProgress.ApplyTo(GetComponent<PlayerHealth>(), GetComponent<PlayerInventory>());
    }
}
