using UnityEngine;

public class MachineHitVfx : MonoBehaviour
{
    public const string EffectName = "Machine Hit VFX";

    public float lifetime = 0.45f;
    public float riseSpeed = 0.18f;
    public float spinSpeed = 180f;

    private float age;
    private Transform[] pieces;
    private Vector3[] baseScales;

    public static MachineHitVfx Spawn(Vector3 position, float effectScale = 1f)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        root.transform.localScale = Vector3.one * effectScale;

        MachineHitVfx vfx = root.AddComponent<MachineHitVfx>();
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
                pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.2f, normalizedAge);
                pieces[i].localPosition += pieces[i].localRotation * Vector3.forward * (0.18f * Time.deltaTime);
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

        pieces = new Transform[7];
        baseScales = new Vector3[pieces.Length];

        pieces[0] = CreatePrimitivePiece("Machine Hit Steam Pop", PrimitiveType.Cube, new Vector3(0f, 0.12f, 0f), new Vector3(0.16f, 0.34f, 0.16f), new Color(0.74f, 0.72f, 0.64f));

        for (int i = 1; i < pieces.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / (pieces.Length - 1);
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.18f, 0.1f + (i % 2) * 0.08f, Mathf.Sin(angle) * 0.18f);
            Transform spark = CreatePrimitivePiece("Machine Hit Brass Spark " + i, PrimitiveType.Cube, position, new Vector3(0.03f, 0.03f, 0.22f), i % 2 == 0 ? new Color(1f, 0.55f, 0.08f) : new Color(1f, 0.86f, 0.25f));
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 36f + i * 11f);
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
