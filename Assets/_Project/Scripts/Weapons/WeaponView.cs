using UnityEngine;

public class WeaponView : MonoBehaviour
{
    public GameObject muzzleFlash;
    public GameObject pressureDumpFlash;
    public Transform pressureGaugeNeedle;
    public Transform pressureValveWheel;
    public Transform pressureDumpLever;
    public Transform pressureChamber;
    public Vector3 recoilOffset = new Vector3(0f, -0.03f, -0.08f);
    public Vector3 secondaryRecoilOffset = new Vector3(0f, -0.06f, -0.14f);
    public Vector3 pressureChamberKickOffset = new Vector3(0f, 0.035f, -0.11f);
    public Vector3 pressureGaugeKickEuler = new Vector3(0f, 0f, 55f);
    public Vector3 pressureValveKickEuler = new Vector3(0f, 0f, 95f);
    public Vector3 pressureLeverKickEuler = new Vector3(-34f, 0f, 0f);
    public float returnSpeed = 14f;
    public float flashDuration = 0.055f;
    public float secondaryFlashDuration = 0.13f;
    public float secondaryMotionDuration = 0.2f;

    private Vector3 restPosition;
    private Vector3 pressureChamberRestPosition;
    private Quaternion pressureGaugeRestRotation;
    private Quaternion pressureValveRestRotation;
    private Quaternion pressureLeverRestRotation;
    private float flashTimer;
    private float pressureDumpFlashTimer;
    private float secondaryMotionTimer;
    private bool secondaryRestPoseCaptured;

    public bool HasSecondaryPressureCues => pressureDumpFlash != null
        && pressureGaugeNeedle != null
        && pressureValveWheel != null
        && pressureDumpLever != null
        && pressureChamber != null;

    public bool IsSecondaryPressureMotionActive => secondaryMotionTimer > 0f
        || pressureDumpFlashTimer > 0f
        || (pressureDumpFlash != null && pressureDumpFlash.activeSelf);

    private void Awake()
    {
        restPosition = transform.localPosition;
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }

        if (pressureDumpFlash != null)
        {
            pressureDumpFlash.SetActive(false);
        }

        CaptureSecondaryRestPose();
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, restPosition, 1f - Mathf.Exp(-returnSpeed * Time.deltaTime));

        CaptureSecondaryRestPose();
        UpdateFlash(muzzleFlash, ref flashTimer);
        UpdateFlash(pressureDumpFlash, ref pressureDumpFlashTimer);
        UpdateSecondaryMotion();
    }

    public void PlayFire()
    {
        PlayFire(false);
    }

    public void PlayFire(bool secondaryShot)
    {
        CaptureSecondaryRestPose();
        transform.localPosition = restPosition + recoilOffset;
        flashTimer = flashDuration;

        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
        }

        if (!secondaryShot)
        {
            return;
        }

        transform.localPosition = restPosition + secondaryRecoilOffset;
        secondaryMotionTimer = secondaryMotionDuration;
        pressureDumpFlashTimer = secondaryFlashDuration;

        if (pressureDumpFlash != null)
        {
            pressureDumpFlash.SetActive(true);
        }

        ApplySecondaryMotion(1f);
    }

    private void CaptureSecondaryRestPose()
    {
        if (secondaryRestPoseCaptured || !HasSecondaryPressureCues)
        {
            return;
        }

        if (pressureChamber != null)
        {
            pressureChamberRestPosition = pressureChamber.localPosition;
        }

        if (pressureGaugeNeedle != null)
        {
            pressureGaugeRestRotation = pressureGaugeNeedle.localRotation;
        }

        if (pressureValveWheel != null)
        {
            pressureValveRestRotation = pressureValveWheel.localRotation;
        }

        if (pressureDumpLever != null)
        {
            pressureLeverRestRotation = pressureDumpLever.localRotation;
        }

        secondaryRestPoseCaptured = true;
    }

    private static void UpdateFlash(GameObject flashObject, ref float timer)
    {
        if (flashObject == null || timer <= 0f)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            flashObject.SetActive(false);
        }
    }

    private void UpdateSecondaryMotion()
    {
        if (secondaryMotionTimer <= 0f)
        {
            ApplySecondaryMotion(0f);
            return;
        }

        secondaryMotionTimer -= Time.deltaTime;
        float kick = Mathf.Clamp01(secondaryMotionTimer / Mathf.Max(0.001f, secondaryMotionDuration));
        ApplySecondaryMotion(kick * kick);
    }

    private void ApplySecondaryMotion(float kick)
    {
        if (pressureChamber != null)
        {
            pressureChamber.localPosition = pressureChamberRestPosition + pressureChamberKickOffset * kick;
        }

        if (pressureGaugeNeedle != null)
        {
            pressureGaugeNeedle.localRotation = pressureGaugeRestRotation * Quaternion.Euler(pressureGaugeKickEuler * kick);
        }

        if (pressureValveWheel != null)
        {
            pressureValveWheel.localRotation = pressureValveRestRotation * Quaternion.Euler(pressureValveKickEuler * kick);
        }

        if (pressureDumpLever != null)
        {
            pressureDumpLever.localRotation = pressureLeverRestRotation * Quaternion.Euler(pressureLeverKickEuler * kick);
        }
    }
}
