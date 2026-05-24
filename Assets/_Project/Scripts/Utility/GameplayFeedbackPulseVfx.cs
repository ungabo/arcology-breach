using UnityEngine;

public class GameplayFeedbackPulseVfx : MonoBehaviour
{
    public const string EffectName = "Gameplay Feedback Pulse VFX";

    public float lifetime = 0.7f;
    public float riseSpeed = 0.2f;
    public float spinSpeed = 96f;
    public Color pulseColor = new Color(1f, 0.62f, 0.1f);
    public string feedbackType = "feedback";

    private float age;
    private Transform[] pieces;
    private Vector3[] baseScales;
    private Vector3[] driftDirections;

    public int PieceCount => pieces == null ? transform.childCount : pieces.Length;

    public static GameplayFeedbackPulseVfx Spawn(Vector3 position, Color color, float scale, string feedbackType)
    {
        GameObject root = new GameObject(EffectName + " - " + feedbackType);
        root.transform.position = position;
        root.transform.localScale = Vector3.one * Mathf.Max(0.1f, scale);

        GameplayFeedbackPulseVfx vfx = root.AddComponent<GameplayFeedbackPulseVfx>();
        vfx.pulseColor = color;
        vfx.feedbackType = feedbackType;
        vfx.Build();
        return vfx;
    }

    private void Awake()
    {
        if (pieces == null)
        {
            Build();
        }
    }

    private void Update()
    {
        if (pieces == null)
        {
            Build();
        }

        age += Time.unscaledDeltaTime;
        float normalizedAge = Mathf.Clamp01(age / lifetime);
        transform.Rotate(Vector3.up, spinSpeed * Time.unscaledDeltaTime, Space.World);
        transform.position += Vector3.up * riseSpeed * Time.unscaledDeltaTime;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.18f, normalizedAge);
            pieces[i].localPosition += driftDirections[i] * (0.32f * Time.unscaledDeltaTime);
        }

        if (age >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void Build()
    {
        if (pieces != null)
        {
            return;
        }

        pieces = new Transform[10];
        baseScales = new Vector3[pieces.Length];
        driftDirections = new Vector3[pieces.Length];

        Color warmCore = Color.Lerp(pulseColor, Color.white, 0.18f);
        pieces[0] = CreatePiece("Feedback Brass Ring", PrimitiveType.Cylinder, Vector3.zero, new Vector3(0.7f, 0.018f, 0.7f), pulseColor);
        pieces[1] = CreatePiece("Feedback Pressure Core", PrimitiveType.Sphere, new Vector3(0f, 0.16f, 0f), new Vector3(0.16f, 0.16f, 0.16f), warmCore);

        for (int i = 2; i < pieces.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / (pieces.Length - 2);
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.28f, 0.14f + (i % 2) * 0.08f, Mathf.Sin(angle) * 0.28f);
            pieces[i] = CreatePiece("Feedback Spark " + i.ToString("00"), PrimitiveType.Cube, position, new Vector3(0.028f, 0.028f, 0.2f), i % 2 == 0 ? pulseColor : warmCore);
            pieces[i].localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 42f);
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            baseScales[i] = pieces[i].localScale;
            Vector3 direction = pieces[i].localPosition.sqrMagnitude > 0.001f ? pieces[i].localPosition.normalized : Vector3.up;
            driftDirections[i] = (direction + Vector3.up * 0.35f).normalized;
        }
    }

    private Transform CreatePiece(string pieceName, PrimitiveType primitiveType, Vector3 localPosition, Vector3 localScale, Color color)
    {
        GameObject piece = GameObject.CreatePrimitive(primitiveType);
        piece.name = pieceName;
        piece.transform.SetParent(transform, false);
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
}
