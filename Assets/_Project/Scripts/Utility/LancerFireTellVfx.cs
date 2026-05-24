using UnityEngine;

public class LancerFireTellVfx : MonoBehaviour
{
    public const string EffectName = "Lancer Fire Tell VFX";

    public Transform muzzle;
    public Transform hotPressureCoil;
    public Transform furnaceLens;
    public Transform backPressureTank;
    public Color chargeColor = new Color(1f, 0.46f, 0.08f, 0.95f);
    public Color steamColor = new Color(0.72f, 0.74f, 0.68f, 0.8f);
    public Color brassColor = new Color(1f, 0.68f, 0.16f, 0.95f);

    private Transform tellRoot;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;
    private float activeDuration = 0.5f;
    private float activeAge;
    private bool active;

    public bool IsConfigured => muzzle != null && hotPressureCoil != null && furnaceLens != null && backPressureTank != null;
    public bool IsActive => active;
    public int PieceCount => pieces == null ? 0 : pieces.Length;

    private void Awake()
    {
        Build();
        SetVisible(false);
    }

    private void Update()
    {
        if (!active)
        {
            return;
        }

        activeAge += Time.deltaTime;
        float t = Mathf.Clamp01(activeAge / Mathf.Max(0.001f, activeDuration));
        float charge = Mathf.SmoothStep(0f, 1f, t);
        float pulse = 0.78f + Mathf.Sin(Time.time * 42f) * 0.22f;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            pieces[i].localScale = baseScales[i] * Mathf.Lerp(0.75f, 1.15f + pulse * 0.18f, charge);
        }

        pieces[0].localRotation = Quaternion.Euler(90f, Time.time * 180f, 0f);
        pieces[0].localScale = new Vector3(Mathf.Lerp(0.18f, 0.58f, charge), 0.018f, Mathf.Lerp(0.18f, 0.58f, charge));
        pieces[1].localPosition = Vector3.Lerp(basePositions[1], new Vector3(0.36f, 0.14f, 0.98f), charge);
        pieces[2].localScale = baseScales[2] * Mathf.Lerp(1f, 1.65f, charge);
        pieces[3].localScale = baseScales[3] * Mathf.Lerp(1f, 1.45f + pulse * 0.22f, charge);
        pieces[4].localScale = baseScales[4] * Mathf.Lerp(0.8f, 1.55f, charge);

        for (int i = 5; i < pieces.Length; i++)
        {
            float offset = i * 0.21f;
            pieces[i].localPosition = basePositions[i] + Vector3.up * Mathf.Sin(Time.time * 24f + offset) * 0.045f * charge;
            pieces[i].localRotation = Quaternion.Euler(Time.time * (80f + i * 9f), 0f, 32f + i * 21f);
        }

        if (activeAge >= activeDuration)
        {
            StopTell();
        }
    }

    public void StartTell(float duration)
    {
        Build();
        activeDuration = Mathf.Max(0.1f, duration);
        activeAge = 0f;
        active = true;
        SetVisible(true);
    }

    public void StopTell()
    {
        active = false;
        SetVisible(false);
    }

    private void Build()
    {
        if (pieces != null)
        {
            return;
        }

        GameObject root = new GameObject(EffectName);
        root.transform.SetParent(transform, false);
        root.transform.localPosition = Vector3.zero;
        tellRoot = root.transform;

        pieces = new Transform[9];
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];

        pieces[0] = CreatePiece("Lancer Muzzle Charge Ring", PrimitiveType.Cylinder, new Vector3(0.36f, 0.14f, 1.03f), new Vector3(0.18f, 0.018f, 0.18f), chargeColor);
        pieces[1] = CreatePiece("Lancer Pressure Needle", PrimitiveType.Cube, new Vector3(0.36f, 0.14f, 0.56f), new Vector3(0.045f, 0.045f, 0.5f), chargeColor);
        pieces[2] = CreatePiece("Lancer Hot Coil Bloom", PrimitiveType.Cube, new Vector3(0.36f, 0.03f, 0.48f), new Vector3(0.24f, 0.09f, 0.32f), chargeColor);
        pieces[3] = CreatePiece("Lancer Furnace Lens Flare", PrimitiveType.Sphere, new Vector3(0f, 0.56f, 0.31f), new Vector3(0.22f, 0.1f, 0.05f), chargeColor);
        pieces[4] = CreatePiece("Lancer Back Tank Pressure Puff", PrimitiveType.Sphere, new Vector3(0f, 0.1f, -0.36f), new Vector3(0.24f, 0.18f, 0.14f), steamColor);
        pieces[5] = CreatePiece("Lancer Charge Brass Spark A", PrimitiveType.Cube, new Vector3(0.18f, 0.22f, 0.72f), new Vector3(0.04f, 0.04f, 0.22f), brassColor);
        pieces[6] = CreatePiece("Lancer Charge Brass Spark B", PrimitiveType.Cube, new Vector3(0.54f, 0.2f, 0.72f), new Vector3(0.04f, 0.04f, 0.22f), brassColor);
        pieces[7] = CreatePiece("Lancer Side Steam Puff A", PrimitiveType.Sphere, new Vector3(0.13f, 0.14f, 0.42f), new Vector3(0.16f, 0.1f, 0.16f), steamColor);
        pieces[8] = CreatePiece("Lancer Side Steam Puff B", PrimitiveType.Sphere, new Vector3(0.58f, 0.14f, 0.42f), new Vector3(0.16f, 0.1f, 0.16f), steamColor);

        for (int i = 0; i < pieces.Length; i++)
        {
            basePositions[i] = pieces[i].localPosition;
            baseScales[i] = pieces[i].localScale;
        }
    }

    private Transform CreatePiece(string pieceName, PrimitiveType primitiveType, Vector3 localPosition, Vector3 localScale, Color color)
    {
        GameObject piece = GameObject.CreatePrimitive(primitiveType);
        piece.name = pieceName;
        piece.transform.SetParent(tellRoot, false);
        piece.transform.localPosition = localPosition;
        piece.transform.localScale = localScale;

        Collider pieceCollider = piece.GetComponent<Collider>();
        if (pieceCollider != null)
        {
            Destroy(pieceCollider);
        }

        Renderer pieceRenderer = piece.GetComponent<Renderer>();
        if (pieceRenderer != null)
        {
            pieceRenderer.material.color = color;
        }

        return piece.transform;
    }

    private void SetVisible(bool visible)
    {
        if (tellRoot != null)
        {
            tellRoot.gameObject.SetActive(visible);
        }
    }
}
