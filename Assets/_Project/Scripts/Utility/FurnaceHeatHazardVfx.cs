using UnityEngine;

public class FurnaceHeatHazardVfx : MonoBehaviour
{
    public Transform warningSignal;
    public Transform activeSignal;
    public Transform safeSignal;
    public Transform[] heatWaves;
    public float pulseSpeed = 3.2f;
    public float waveRise = 0.18f;
    public float waveScale = 0.16f;

    private Vector3 warningBaseScale;
    private Vector3 activeBaseScale;
    private Vector3 safeBaseScale;
    private Vector3[] waveBasePositions;
    private Vector3[] waveBaseScales;

    public int VisibleHeatPieceCount => heatWaves == null ? 0 : heatWaves.Length;
    public bool HasPhaseSignals => warningSignal != null && activeSignal != null && safeSignal != null;

    public bool ActiveHeatVisible
    {
        get
        {
            if (heatWaves == null)
            {
                return false;
            }

            for (int i = 0; i < heatWaves.Length; i++)
            {
                if (heatWaves[i] != null && heatWaves[i].gameObject.activeInHierarchy)
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void Awake()
    {
        CaptureBaseTransforms();
    }

    private void Update()
    {
        if (waveBasePositions == null || heatWaves != null && waveBasePositions.Length != heatWaves.Length)
        {
            CaptureBaseTransforms();
        }

        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        AnimatePhaseSignal(warningSignal, warningBaseScale, 1f + pulse * 0.08f);
        AnimatePhaseSignal(activeSignal, activeBaseScale, 1f + pulse * 0.12f);
        AnimatePhaseSignal(safeSignal, safeBaseScale, 1f + pulse * 0.04f);
        AnimateHeatWaves();
    }

    private void AnimatePhaseSignal(Transform signal, Vector3 baseScale, float scale)
    {
        if (signal != null && signal.gameObject.activeInHierarchy)
        {
            signal.localScale = baseScale * scale;
        }
    }

    private void AnimateHeatWaves()
    {
        if (heatWaves == null || waveBasePositions == null)
        {
            return;
        }

        bool shouldShow = activeSignal == null || activeSignal.gameObject.activeInHierarchy || warningSignal != null && warningSignal.gameObject.activeInHierarchy;
        for (int i = 0; i < heatWaves.Length; i++)
        {
            Transform wave = heatWaves[i];
            if (wave == null)
            {
                continue;
            }

            wave.gameObject.SetActive(shouldShow);
            float phase = Time.time * pulseSpeed + i * 1.35f;
            float wavePulse = (Mathf.Sin(phase) + 1f) * 0.5f;
            wave.localPosition = waveBasePositions[i] + Vector3.up * (wavePulse * waveRise);
            wave.localScale = waveBaseScales[i] * (1f + wavePulse * waveScale);
        }
    }

    private void CaptureBaseTransforms()
    {
        warningBaseScale = warningSignal == null ? Vector3.one : warningSignal.localScale;
        activeBaseScale = activeSignal == null ? Vector3.one : activeSignal.localScale;
        safeBaseScale = safeSignal == null ? Vector3.one : safeSignal.localScale;

        if (heatWaves == null)
        {
            return;
        }

        waveBasePositions = new Vector3[heatWaves.Length];
        waveBaseScales = new Vector3[heatWaves.Length];
        for (int i = 0; i < heatWaves.Length; i++)
        {
            if (heatWaves[i] != null)
            {
                waveBasePositions[i] = heatWaves[i].localPosition;
                waveBaseScales[i] = heatWaves[i].localScale;
            }
        }
    }
}
