using UnityEngine;

public class LiftActivationVfx : MonoBehaviour
{
    public const string EffectName = "Service Lift Activation VFX";

    public float lifetime = 0.75f;
    public float riseSpeed = 0.24f;
    public float pulseSpeed = 4f;

    private float age;
    private Transform[] pieces;
    private Vector3[] baseScales;

    public static LiftActivationVfx Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        root.transform.rotation = rotation;

        LiftActivationVfx vfx = root.AddComponent<LiftActivationVfx>();
        vfx.Build();
        return vfx;
    }

    public int PieceCount => transform.childCount;

    private void Awake()
    {
        if (transform.childCount == 0)
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

        age += Time.deltaTime;
        float normalizedAge = Mathf.Clamp01(age / lifetime);
        float pulse = 1f + Mathf.Sin(age * pulseSpeed * Mathf.PI) * 0.08f;
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                pieces[i].localScale = baseScales[i] * pulse * Mathf.Lerp(1f, 0.35f, normalizedAge);
            }
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

        pieces = new Transform[9];
        baseScales = new Vector3[pieces.Length];

        pieces[0] = CreatePrimitivePiece("Lift Activation Floor Pressure Ring", PrimitiveType.Cylinder, new Vector3(0f, -0.35f, 0f), new Vector3(1.15f, 0.025f, 1.15f), new Color(0.24f, 0.95f, 0.45f));
        pieces[1] = CreatePrimitivePiece("Lift Activation Center Steam", PrimitiveType.Cube, new Vector3(0f, 0.08f, -0.1f), new Vector3(0.2f, 0.75f, 0.2f), new Color(0.76f, 0.75f, 0.68f));

        for (int i = 2; i < pieces.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / (pieces.Length - 2);
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.52f, 0.14f + (i % 2) * 0.12f, Mathf.Sin(angle) * 0.28f);
            Transform spark = CreatePrimitivePiece("Lift Activation Brass Contact " + i, PrimitiveType.Cube, position, new Vector3(0.04f, 0.04f, 0.22f), i % 2 == 0 ? new Color(0.34f, 1f, 0.48f) : new Color(1f, 0.7f, 0.14f));
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 45f);
            pieces[i] = spark;
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            baseScales[i] = pieces[i].localScale;
        }
    }

    private Transform CreatePrimitivePiece(string pieceName, PrimitiveType primitiveType, Vector3 localPosition, Vector3 localScale, Color color)
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
