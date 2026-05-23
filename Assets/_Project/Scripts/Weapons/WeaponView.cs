using UnityEngine;

public class WeaponView : MonoBehaviour
{
    public GameObject muzzleFlash;
    public Vector3 recoilOffset = new Vector3(0f, -0.03f, -0.08f);
    public float returnSpeed = 14f;
    public float flashDuration = 0.055f;

    private Vector3 restPosition;
    private float flashTimer;

    private void Awake()
    {
        restPosition = transform.localPosition;
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, restPosition, 1f - Mathf.Exp(-returnSpeed * Time.deltaTime));

        if (muzzleFlash == null || flashTimer <= 0f)
        {
            return;
        }

        flashTimer -= Time.deltaTime;
        if (flashTimer <= 0f)
        {
            muzzleFlash.SetActive(false);
        }
    }

    public void PlayFire()
    {
        transform.localPosition = restPosition + recoilOffset;
        flashTimer = flashDuration;

        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
        }
    }
}
