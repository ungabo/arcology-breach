using UnityEngine;

public class GateOpenVfx : MonoBehaviour
{
    public const string EffectName = "Pressure Gate Open VFX";

    public float lifetime = 0.9f;
    public float riseSpeed = 0.32f;
    public float spreadSpeed = 0.22f;

    private float age;
    private Transform[] pieces;
    private Vector3[] baseScales;
    private Vector3[] driftDirections;

    public static GateOpenVfx Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        root.transform.rotation = rotation;

        GateOpenVfx vfx = root.AddComponent<GateOpenVfx>();
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
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.3f, normalizedAge);
                pieces[i].localPosition += driftDirections[i] * spreadSpeed * Time.deltaTime;
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

        pieces = new Transform[10];
        baseScales = new Vector3[pieces.Length];
        driftDirections = new Vector3[pieces.Length];

        pieces[0] = CreatePrimitivePiece("Gate Open Green Pressure Wash", PrimitiveType.Cylinder, new Vector3(0f, 0f, 0f), new Vector3(1.2f, 0.025f, 1.2f), new Color(0.26f, 0.9f, 0.42f));
        driftDirections[0] = Vector3.zero;

        for (int i = 1; i <= 4; i++)
        {
            float x = i % 2 == 0 ? 0.54f : -0.54f;
            float y = 0.22f + i * 0.12f;
            Transform steam = CreatePrimitivePiece("Gate Open Steam Jet " + i, PrimitiveType.Cube, new Vector3(x, y, -0.1f), new Vector3(0.16f, 0.68f, 0.16f), new Color(0.78f, 0.77f, 0.68f));
            steam.localRotation = Quaternion.Euler(i % 2 == 0 ? 12f : -12f, 0f, i % 2 == 0 ? -18f : 18f);
            pieces[i] = steam;
            driftDirections[i] = new Vector3(x * 0.2f, 0.55f, -0.15f).normalized;
        }

        for (int i = 5; i < pieces.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / 5f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.42f, 0.36f + (i % 2) * 0.1f, Mathf.Sin(angle) * 0.2f);
            Transform spark = CreatePrimitivePiece("Gate Open Brass Spark " + i, PrimitiveType.Cube, position, new Vector3(0.035f, 0.035f, 0.28f), i % 2 == 0 ? new Color(1f, 0.62f, 0.08f) : new Color(0.36f, 1f, 0.48f));
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 35f + i * 10f);
            pieces[i] = spark;
            driftDirections[i] = position.normalized;
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
