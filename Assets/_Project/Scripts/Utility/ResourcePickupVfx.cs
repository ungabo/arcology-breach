using UnityEngine;

public class ResourcePickupVfx : MonoBehaviour
{
    public const string EffectName = "Resource Pickup VFX";

    public PickupKind pickupKind;
    public float lifetime = 0.65f;
    public float riseSpeed = 0.22f;
    public float spinSpeed = 160f;

    private float age;
    private Transform[] pieces;
    private Vector3[] baseScales;

    public static ResourcePickupVfx Spawn(Vector3 position, PickupKind pickupKind)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;

        ResourcePickupVfx vfx = root.AddComponent<ResourcePickupVfx>();
        vfx.pickupKind = pickupKind;
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

        Color mainColor = pickupKind == PickupKind.Health ? new Color(0.95f, 0.18f, 0.18f) : new Color(0.95f, 0.72f, 0.18f);
        Color accentColor = pickupKind == PickupKind.Health ? new Color(1f, 0.58f, 0.48f) : new Color(0.36f, 0.9f, 1f);

        pieces = new Transform[8];
        baseScales = new Vector3[pieces.Length];

        pieces[0] = CreatePrimitivePiece("Resource Pickup Ring", PrimitiveType.Cylinder, Vector3.zero, new Vector3(0.66f, 0.022f, 0.66f), mainColor);
        pieces[1] = CreatePrimitivePiece("Resource Pickup Glow", PrimitiveType.Sphere, new Vector3(0f, 0.16f, 0f), new Vector3(0.18f, 0.18f, 0.18f), accentColor);

        for (int i = 2; i < pieces.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / (pieces.Length - 2);
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.28f, 0.18f + (i % 2) * 0.08f, Mathf.Sin(angle) * 0.28f);
            Transform spark = CreatePrimitivePiece("Resource Pickup Spark " + i, PrimitiveType.Cube, position, new Vector3(0.032f, 0.032f, 0.2f), i % 2 == 0 ? mainColor : accentColor);
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 40f);
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
