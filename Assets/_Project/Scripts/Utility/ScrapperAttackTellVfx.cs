using UnityEngine;

public class ScrapperAttackTellVfx : MonoBehaviour
{
    public const string EffectName = "Scrapper Attack Tell VFX";

    public Transform leftCutter;
    public Transform rightCutter;
    public Transform furnaceEye;
    public Transform pressureTank;
    public Color warningColor = new Color(1f, 0.18f, 0.04f, 0.95f);
    public Color brassColor = new Color(1f, 0.62f, 0.12f, 0.95f);
    public Color steamColor = new Color(0.72f, 0.74f, 0.68f, 0.82f);

    private Transform tellRoot;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;
    private float activeDuration = 0.5f;
    private float activeAge;
    private bool active;

    public bool IsConfigured => leftCutter != null && rightCutter != null && furnaceEye != null && pressureTank != null;
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
        float bite = Mathf.SmoothStep(0f, 1f, t);
        float pulse = 0.78f + Mathf.Sin(Time.time * 34f) * 0.22f;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            pieces[i].localScale = baseScales[i] * Mathf.Lerp(0.72f, 1.25f + pulse * 0.18f, bite);
        }

        pieces[0].localScale = new Vector3(Mathf.Lerp(1.15f, 2.1f, bite), 0.028f, Mathf.Lerp(1.15f, 2.1f, bite));
        pieces[0].localRotation = Quaternion.Euler(0f, Time.time * 140f, 0f);
        pieces[1].localRotation = Quaternion.Euler(0f, 0f, -28f - bite * 42f);
        pieces[2].localRotation = Quaternion.Euler(0f, 0f, 28f + bite * 42f);
        pieces[3].localScale = baseScales[3] * Mathf.Lerp(1f, 1.45f + pulse * 0.24f, bite);
        pieces[4].localScale = baseScales[4] * Mathf.Lerp(0.8f, 1.6f, bite);

        for (int i = 5; i < pieces.Length; i++)
        {
            float offset = (i - 5) * 0.18f;
            pieces[i].localPosition = basePositions[i] + Vector3.up * Mathf.Sin(Time.time * 22f + offset) * 0.045f * bite;
            pieces[i].localRotation = Quaternion.Euler(0f, Time.time * (95f + i * 7f), 22f + i * 19f);
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

        pieces[0] = CreatePiece("Scrapper Attack Ground Warning Ring", PrimitiveType.Cylinder, new Vector3(0f, -0.9f, 0.36f), new Vector3(1.15f, 0.028f, 1.15f), warningColor);
        pieces[1] = CreatePiece("Scrapper Left Cutter Warning Edge", PrimitiveType.Cube, new Vector3(-0.77f, -0.45f, 0.53f), new Vector3(0.08f, 0.5f, 0.08f), warningColor);
        pieces[2] = CreatePiece("Scrapper Right Cutter Warning Edge", PrimitiveType.Cube, new Vector3(0.77f, -0.45f, 0.53f), new Vector3(0.08f, 0.5f, 0.08f), warningColor);
        pieces[3] = CreatePiece("Scrapper Furnace Windup Flare", PrimitiveType.Sphere, new Vector3(0f, 0.18f, 0.49f), new Vector3(0.26f, 0.18f, 0.08f), warningColor);
        pieces[4] = CreatePiece("Scrapper Back Pressure Surge", PrimitiveType.Sphere, new Vector3(0f, 0.16f, -0.33f), new Vector3(0.28f, 0.28f, 0.18f), steamColor);
        pieces[5] = CreatePiece("Scrapper Windup Brass Spark A", PrimitiveType.Cube, new Vector3(-0.38f, 0.02f, 0.58f), new Vector3(0.05f, 0.05f, 0.18f), brassColor);
        pieces[6] = CreatePiece("Scrapper Windup Brass Spark B", PrimitiveType.Cube, new Vector3(0.38f, 0.02f, 0.58f), new Vector3(0.05f, 0.05f, 0.18f), brassColor);
        pieces[7] = CreatePiece("Scrapper Windup Steam Puff A", PrimitiveType.Sphere, new Vector3(-0.28f, 0.34f, -0.28f), new Vector3(0.18f, 0.12f, 0.18f), steamColor);
        pieces[8] = CreatePiece("Scrapper Windup Steam Puff B", PrimitiveType.Sphere, new Vector3(0.28f, 0.34f, -0.28f), new Vector3(0.18f, 0.12f, 0.18f), steamColor);

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
