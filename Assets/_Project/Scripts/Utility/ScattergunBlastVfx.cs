using UnityEngine;

public class ScattergunBlastVfx : MonoBehaviour
{
    public const string EffectName = "Steam Scattergun Blast VFX";

    public float lifetime = 0.42f;
    public float spreadSpeed = 1.35f;
    public float spinSpeed = 220f;

    private float age;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;

    public static ScattergunBlastVfx Spawn(Vector3 position, Vector3 forward)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        if (forward.sqrMagnitude > 0.001f)
        {
            root.transform.rotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
        }

        ScattergunBlastVfx vfx = root.AddComponent<ScattergunBlastVfx>();
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
        float t = Mathf.Clamp01(age / lifetime);
        transform.position += transform.forward * spreadSpeed * Time.deltaTime;
        transform.Rotate(transform.forward, spinSpeed * Time.deltaTime, Space.World);

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            pieces[i].localPosition = basePositions[i] * Mathf.Lerp(1f, 1.85f, t);
            pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.2f, t);
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
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];

        Color steamColor = new Color(0.72f, 0.72f, 0.68f, 0.92f);
        Color brassColor = new Color(1f, 0.65f, 0.16f, 0.95f);
        Color warningColor = new Color(1f, 0.25f, 0.08f, 0.95f);

        pieces[0] = CreatePiece("Scattergun Steam Core", PrimitiveType.Sphere, new Vector3(0f, 0f, 0.05f), new Vector3(0.28f, 0.2f, 0.28f), steamColor);
        pieces[1] = CreatePiece("Scattergun Brass Pressure Ring", PrimitiveType.Cylinder, new Vector3(0f, 0f, 0.02f), new Vector3(0.46f, 0.025f, 0.46f), brassColor);
        pieces[1].localRotation = Quaternion.Euler(90f, 0f, 0f);

        for (int i = 2; i < pieces.Length; i++)
        {
            float angle = (i - 2) * Mathf.PI * 2f / (pieces.Length - 2);
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.26f, Mathf.Sin(angle) * 0.18f, 0.1f + (i % 2) * 0.08f);
            Transform spark = CreatePiece("Scattergun Brass Spark " + i, PrimitiveType.Cube, position, new Vector3(0.035f, 0.035f, 0.24f), i % 2 == 0 ? brassColor : warningColor);
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 35f);
            pieces[i] = spark;
        }

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
