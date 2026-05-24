using UnityEngine;

public class WeaponPickupVfx : MonoBehaviour
{
    public const string EffectName = "Weapon Pickup VFX";

    public float lifetime = 0.78f;
    public float riseSpeed = 0.18f;
    public float spinSpeed = 145f;

    private float age;
    private Transform[] pieces;
    private Vector3[] baseScales;

    public int PieceCount => transform.childCount;

    public static WeaponPickupVfx Spawn(Vector3 position)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;

        WeaponPickupVfx vfx = root.AddComponent<WeaponPickupVfx>();
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
        float normalizedAge = Mathf.Clamp01(age / lifetime);
        float pulse = 1f + Mathf.Sin(Time.time * 16f) * 0.05f;

        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                pieces[i].localScale = baseScales[i] * Mathf.Lerp(pulse, 0.2f, normalizedAge);
                pieces[i].localPosition += pieces[i].localRotation * Vector3.forward * (0.08f * Time.deltaTime);
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

        Color brassColor = new Color(1f, 0.68f, 0.18f);
        Color steamColor = new Color(0.78f, 0.82f, 0.78f);
        Color pressureColor = new Color(0.22f, 0.95f, 1f);
        Color warningColor = new Color(1f, 0.32f, 0.08f);

        pieces[0] = CreatePiece("Weapon Pickup Pressure Ring", PrimitiveType.Cylinder, Vector3.zero, new Vector3(0.92f, 0.026f, 0.92f), brassColor);
        pieces[1] = CreatePiece("Weapon Pickup Steam Core", PrimitiveType.Sphere, new Vector3(0f, 0.22f, 0f), new Vector3(0.24f, 0.18f, 0.24f), steamColor);
        pieces[2] = CreatePiece("Weapon Pickup Pressure Globe", PrimitiveType.Sphere, new Vector3(0f, 0.34f, 0f), new Vector3(0.16f, 0.16f, 0.16f), pressureColor);

        for (int i = 3; i < pieces.Length; i++)
        {
            float angle = (i - 3) * Mathf.PI * 2f / (pieces.Length - 3);
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.38f, 0.18f + (i % 2) * 0.12f, Mathf.Sin(angle) * 0.38f);
            Transform spark = CreatePiece("Weapon Pickup Brass Shell " + i, PrimitiveType.Cube, position, new Vector3(0.045f, 0.045f, 0.24f), i % 2 == 0 ? brassColor : warningColor);
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 38f);
            pieces[i] = spark;
        }

        for (int i = 0; i < pieces.Length; i++)
        {
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
