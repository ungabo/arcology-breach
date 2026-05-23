using UnityEngine;

public class GearKeyPickupVfx : MonoBehaviour
{
    public const string EffectName = "Gear Key Pickup VFX";

    public float lifetime = 0.8f;
    public float riseSpeed = 0.32f;
    public float spinSpeed = 220f;

    private float age;
    private Transform[] pieces;
    private Vector3[] baseScales;

    public static GearKeyPickupVfx Spawn(Vector3 position)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;

        GearKeyPickupVfx vfx = root.AddComponent<GearKeyPickupVfx>();
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

        pieces[0] = CreatePrimitivePiece("Gear Key Pickup Ring", PrimitiveType.Cylinder, Vector3.zero, new Vector3(0.82f, 0.025f, 0.82f), new Color(1f, 0.66f, 0.12f));
        pieces[1] = CreatePrimitivePiece("Gear Key Pickup Center Glow", PrimitiveType.Sphere, new Vector3(0f, 0.18f, 0f), new Vector3(0.22f, 0.22f, 0.22f), new Color(1f, 0.82f, 0.22f));

        for (int i = 2; i < pieces.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / (pieces.Length - 2);
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.34f, 0.22f + (i % 2) * 0.08f, Mathf.Sin(angle) * 0.34f);
            Transform tooth = CreatePrimitivePiece("Gear Key Pickup Tooth Spark " + i, PrimitiveType.Cube, position, new Vector3(0.04f, 0.04f, 0.24f), i % 2 == 0 ? new Color(1f, 0.55f, 0.08f) : new Color(1f, 0.9f, 0.32f));
            tooth.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 35f);
            pieces[i] = tooth;
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
