using UnityEngine;

public class MachineDeathVfx : MonoBehaviour
{
    public const string EffectName = "Machine Death VFX";

    public float lifetime = 0.85f;
    public float riseSpeed = 0.28f;
    public float spinSpeed = 120f;

    private float age;
    private Transform[] pieces;
    private Vector3[] baseScales;

    public static MachineDeathVfx Spawn(Vector3 position, float effectScale = 1f)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        root.transform.localScale = Vector3.one * effectScale;

        MachineDeathVfx vfx = root.AddComponent<MachineDeathVfx>();
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

        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.25f, normalizedAge);
                pieces[i].localPosition += pieces[i].localRotation * Vector3.up * (0.12f * Time.deltaTime);
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

        pieces = new Transform[11];
        baseScales = new Vector3[pieces.Length];

        pieces[0] = CreatePrimitivePiece("Machine Death Pressure Ring", PrimitiveType.Cylinder, Vector3.zero, new Vector3(1.25f, 0.025f, 1.25f), new Color(1f, 0.48f, 0.12f));

        for (int i = 1; i <= 4; i++)
        {
            float angle = i * Mathf.PI * 0.5f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.24f, 0.26f, Mathf.Sin(angle) * 0.24f);
            Transform steam = CreatePrimitivePiece("Machine Death Steam Puff " + i, PrimitiveType.Cube, position, new Vector3(0.18f, 0.5f, 0.18f), new Color(0.76f, 0.74f, 0.67f));
            steam.localRotation = Quaternion.Euler(24f, angle * Mathf.Rad2Deg, 18f);
            pieces[i] = steam;
        }

        for (int i = 5; i < pieces.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / 6f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.44f, 0.18f + (i % 2) * 0.1f, Mathf.Sin(angle) * 0.44f);
            Transform spark = CreatePrimitivePiece("Machine Death Brass Spark " + i, PrimitiveType.Cube, position, new Vector3(0.035f, 0.035f, 0.24f), i % 2 == 0 ? new Color(1f, 0.62f, 0.08f) : new Color(1f, 0.88f, 0.32f));
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 45f + i * 12f);
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
