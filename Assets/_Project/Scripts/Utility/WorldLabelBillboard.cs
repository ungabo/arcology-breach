using UnityEngine;

public class WorldLabelBillboard : MonoBehaviour
{
    public string labelId;
    public TextMesh textMesh;
    public Renderer backplateRenderer;
    public bool yawOnly = true;
    public float normalCharacterSize = 0.2f;
    public float highContrastCharacterSize = 0.24f;
    public Color normalTextColor = new Color(1f, 0.82f, 0.28f, 1f);
    public Color highContrastTextColor = Color.white;
    public Color normalBackplateColor = new Color(0.03f, 0.025f, 0.018f, 1f);
    public Color highContrastBackplateColor = Color.black;

    public bool CurrentHighContrastApplied { get; private set; }
    public float LastCameraDistance { get; private set; }
    public bool V0137ReadabilityReady => textMesh != null
        && backplateRenderer != null
        && normalCharacterSize > 0f
        && highContrastCharacterSize >= normalCharacterSize
        && !string.IsNullOrWhiteSpace(labelId);

    private Camera cachedCamera;
    private bool styleInitialized;

    private void Awake()
    {
        ResolveReferences();
        ApplyReadabilityStyle(force: true);
    }

    private void LateUpdate()
    {
        FaceCamera(ResolveCamera());
        ApplyReadabilityStyle(force: false);
    }

    public void ApplyReadabilityForTest()
    {
        ResolveReferences();
        ApplyReadabilityStyle(force: true);
    }

    public void ForceFaceCameraForTest(Camera camera)
    {
        FaceCamera(camera);
    }

    private void ResolveReferences()
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMesh>();
        }

        if (string.IsNullOrWhiteSpace(labelId))
        {
            labelId = gameObject.name;
        }
    }

    private Camera ResolveCamera()
    {
        if (cachedCamera != null && cachedCamera.isActiveAndEnabled)
        {
            return cachedCamera;
        }

        cachedCamera = Camera.main;
        if (cachedCamera == null)
        {
            cachedCamera = Object.FindAnyObjectByType<Camera>();
        }

        return cachedCamera;
    }

    private void FaceCamera(Camera camera)
    {
        if (camera == null)
        {
            return;
        }

        Vector3 direction = transform.position - camera.transform.position;
        LastCameraDistance = direction.magnitude;
        if (yawOnly)
        {
            direction.y = 0f;
        }

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    }

    private void ApplyReadabilityStyle(bool force)
    {
        GameSettings.Load();
        bool highContrast = GameSettings.HighContrast;
        if (!force && styleInitialized && CurrentHighContrastApplied == highContrast)
        {
            return;
        }

        styleInitialized = true;
        CurrentHighContrastApplied = highContrast;

        if (textMesh != null)
        {
            textMesh.color = highContrast ? highContrastTextColor : normalTextColor;
            textMesh.characterSize = highContrast ? highContrastCharacterSize : normalCharacterSize;
            textMesh.fontStyle = FontStyle.Bold;
        }

        if (backplateRenderer != null)
        {
            backplateRenderer.enabled = true;
            Material material = backplateRenderer.material;
            if (material != null)
            {
                material.color = highContrast ? highContrastBackplateColor : normalBackplateColor;
            }
        }
    }
}
