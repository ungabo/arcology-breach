using UnityEngine;

public class MachineMotionVfx : MonoBehaviour
{
    public Transform body;
    public Transform[] leftMotionParts;
    public Transform[] rightMotionParts;
    public Transform[] pulseParts;
    public float idleBob = 0.035f;
    public float moveBobMultiplier = 1.8f;
    public float idleSpeed = 2.2f;
    public float swingDegrees = 7f;
    public float pressurePulse = 0.035f;

    private Vector3 lastWorldPosition;
    private Vector3 bodyBasePosition;
    private Quaternion bodyBaseRotation;
    private Quaternion[] leftBaseRotations;
    private Quaternion[] rightBaseRotations;
    private Vector3[] pulseBaseScales;

    public int MotionPartCount => CountParts(leftMotionParts) + CountParts(rightMotionParts) + CountParts(pulseParts) + (body == null ? 0 : 1);
    public bool IsConfigured => body != null && MotionPartCount >= 3;

    private void Awake()
    {
        CaptureBaseTransforms();
        lastWorldPosition = transform.position;
    }

    private void Update()
    {
        if (!IsConfigured)
        {
            return;
        }

        float distanceMoved = (transform.position - lastWorldPosition).magnitude;
        float movement = Time.deltaTime > 0f ? Mathf.Clamp01(distanceMoved / Time.deltaTime * 0.45f) : 0f;
        float phase = Time.time * (idleSpeed + movement * 2f);
        float bob = Mathf.Sin(phase) * idleBob * Mathf.Lerp(1f, moveBobMultiplier, movement);

        body.localPosition = bodyBasePosition + Vector3.up * bob;
        body.localRotation = bodyBaseRotation * Quaternion.Euler(Mathf.Sin(phase * 0.55f) * 1.4f, 0f, Mathf.Sin(phase * 0.8f) * 1.8f);
        AnimateLimbSet(leftMotionParts, leftBaseRotations, phase, 1f, movement);
        AnimateLimbSet(rightMotionParts, rightBaseRotations, phase, -1f, movement);
        AnimatePulseParts(phase);

        lastWorldPosition = transform.position;
    }

    private void AnimateLimbSet(Transform[] parts, Quaternion[] baseRotations, float phase, float side, float movement)
    {
        if (parts == null || baseRotations == null)
        {
            return;
        }

        float swing = Mathf.Sin(phase) * swingDegrees * Mathf.Lerp(0.45f, 1f, movement);
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != null)
            {
                parts[i].localRotation = baseRotations[i] * Quaternion.Euler(swing * side, 0f, swing * 0.35f * side);
            }
        }
    }

    private void AnimatePulseParts(float phase)
    {
        if (pulseParts == null || pulseBaseScales == null)
        {
            return;
        }

        float pulse = 1f + (Mathf.Sin(phase * 1.25f) + 1f) * 0.5f * pressurePulse;
        for (int i = 0; i < pulseParts.Length; i++)
        {
            if (pulseParts[i] != null)
            {
                pulseParts[i].localScale = pulseBaseScales[i] * pulse;
            }
        }
    }

    private void CaptureBaseTransforms()
    {
        if (body != null)
        {
            bodyBasePosition = body.localPosition;
            bodyBaseRotation = body.localRotation;
        }

        leftBaseRotations = CaptureRotations(leftMotionParts);
        rightBaseRotations = CaptureRotations(rightMotionParts);
        pulseBaseScales = CaptureScales(pulseParts);
    }

    private static Quaternion[] CaptureRotations(Transform[] parts)
    {
        if (parts == null)
        {
            return null;
        }

        Quaternion[] rotations = new Quaternion[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            rotations[i] = parts[i] == null ? Quaternion.identity : parts[i].localRotation;
        }

        return rotations;
    }

    private static Vector3[] CaptureScales(Transform[] parts)
    {
        if (parts == null)
        {
            return null;
        }

        Vector3[] scales = new Vector3[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            scales[i] = parts[i] == null ? Vector3.one : parts[i].localScale;
        }

        return scales;
    }

    private static int CountParts(Transform[] parts)
    {
        if (parts == null)
        {
            return 0;
        }

        int count = 0;
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != null)
            {
                count++;
            }
        }

        return count;
    }
}
