using UnityEngine;

public class ScattergunSlugVfx : MonoBehaviour
{
    public const string EffectName = "Steam Scattergun Slug VFX";

    public float lifetime = 0.38f;
    public float travelSpeed = 2.1f;
    public float spinSpeed = 280f;

    private float age;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;

    public int PieceCount => transform.childCount;

    public static ScattergunSlugVfx Spawn(Vector3 position, Vector3 forward)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        if (forward.sqrMagnitude > 0.001f)
        {
            root.transform.rotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
        }

        ScattergunSlugVfx vfx = root.AddComponent<ScattergunSlugVfx>();
        vfx.Build();
        return vfx;
    }

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
        transform.position += transform.forward * travelSpeed * Time.deltaTime;
        transform.Rotate(transform.forward, spinSpeed * Time.deltaTime, Space.World);

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            pieces[i].localPosition = basePositions[i] * Mathf.Lerp(1f, 2.2f, t);
            pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.18f, t);
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
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];

        Color brassColor = new Color(1f, 0.68f, 0.16f, 0.96f);
        Color steamColor = new Color(0.72f, 0.78f, 0.74f, 0.92f);
        Color pressureColor = new Color(0.26f, 0.92f, 1f, 0.96f);
        Color warningColor = new Color(1f, 0.26f, 0.08f, 0.95f);

        pieces[0] = CreatePiece("Scattergun Slug Pressure Spear", PrimitiveType.Cylinder, new Vector3(0f, 0f, 0.18f), new Vector3(0.055f, 0.58f, 0.055f), pressureColor);
        pieces[0].localRotation = Quaternion.Euler(90f, 0f, 0f);
        pieces[1] = CreatePiece("Scattergun Slug Brass Collar", PrimitiveType.Cylinder, new Vector3(0f, 0f, 0.02f), new Vector3(0.24f, 0.026f, 0.24f), brassColor);
        pieces[1].localRotation = Quaternion.Euler(90f, 0f, 0f);
        pieces[2] = CreatePiece("Scattergun Slug Steam Core", PrimitiveType.Sphere, new Vector3(0f, 0f, 0.04f), new Vector3(0.18f, 0.14f, 0.18f), steamColor);

        for (int i = 3; i < pieces.Length; i++)
        {
            float angle = (i - 3) * Mathf.PI * 2f / (pieces.Length - 3);
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.18f, Mathf.Sin(angle) * 0.14f, 0.1f + (i % 2) * 0.08f);
            Transform spark = CreatePiece("Scattergun Slug Brass Spark " + i, PrimitiveType.Cube, position, new Vector3(0.028f, 0.028f, 0.22f), i % 2 == 0 ? brassColor : warningColor);
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 18f);
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
            pieceCollider.enabled = false;
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
