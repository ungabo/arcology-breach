using UnityEngine;

public class SteamworksSpinner : MonoBehaviour
{
    public Vector3 localAxis = Vector3.up;
    public float degreesPerSecond = 36f;

    private void Update()
    {
        if (localAxis.sqrMagnitude <= 0.001f || Mathf.Approximately(degreesPerSecond, 0f))
        {
            return;
        }

        transform.Rotate(localAxis.normalized, degreesPerSecond * Time.deltaTime, Space.Self);
    }
}
