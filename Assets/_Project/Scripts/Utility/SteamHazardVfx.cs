using UnityEngine;

public class SteamHazardVfx : MonoBehaviour
{
    public Transform[] puffs;
    public float pulseSpeed = 1.65f;
    public float riseAmplitude = 0.16f;
    public float scaleAmplitude = 0.18f;

    private Vector3[] basePositions;
    private Vector3[] baseScales;

    public int VisiblePuffCount => puffs == null ? 0 : puffs.Length;

    private void Awake()
    {
        CaptureBaseTransforms();
    }

    private void Update()
    {
        if (puffs == null || puffs.Length == 0)
        {
            return;
        }

        if (basePositions == null || basePositions.Length != puffs.Length)
        {
            CaptureBaseTransforms();
        }

        for (int i = 0; i < puffs.Length; i++)
        {
            if (puffs[i] == null)
            {
                continue;
            }

            float phase = Time.time * pulseSpeed + i * 1.7f;
            float pulse = (Mathf.Sin(phase) + 1f) * 0.5f;
            puffs[i].localPosition = basePositions[i] + Vector3.up * (pulse * riseAmplitude);
            puffs[i].localScale = baseScales[i] * (1f + pulse * scaleAmplitude);
        }
    }

    private void CaptureBaseTransforms()
    {
        if (puffs == null)
        {
            return;
        }

        basePositions = new Vector3[puffs.Length];
        baseScales = new Vector3[puffs.Length];
        for (int i = 0; i < puffs.Length; i++)
        {
            if (puffs[i] != null)
            {
                basePositions[i] = puffs[i].localPosition;
                baseScales[i] = puffs[i].localScale;
            }
        }
    }
}
