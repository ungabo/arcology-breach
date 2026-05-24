using UnityEngine;

public class PressureBurstVfx : MonoBehaviour
{
    public const string EffectName = "Pressure Burst VFX";

    public float lifetime = 0.32f;
    public float expansion = 2.4f;
    public float driftSpeed = 0.75f;

    private float age;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;

    public int PieceCount => transform.childCount;

    public static PressureBurstVfx Spawn(Vector3 position, Vector3 forward)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        if (forward.sqrMagnitude > 0.001f)
        {
            root.transform.rotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
        }

        PressureBurstVfx vfx = root.AddComponent<PressureBurstVfx>();
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
        transform.position += transform.forward * driftSpeed * Time.deltaTime;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            pieces[i].localPosition = basePositions[i] * Mathf.Lerp(1f, expansion, t);
            pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.12f, t);
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
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];

        Color pressureColor = new Color(0.35f, 0.95f, 1f, 0.9f);
        Color brassColor = new Color(1f, 0.7f, 0.18f, 0.95f);
        Color steamColor = new Color(0.76f, 0.78f, 0.72f, 0.82f);
        Color warningColor = new Color(1f, 0.28f, 0.08f, 0.92f);

        pieces[0] = CreatePiece("Pressure Burst Front Ring", PrimitiveType.Cylinder, new Vector3(0f, 0f, 0.12f), new Vector3(0.36f, 0.026f, 0.36f), pressureColor);
        pieces[0].localRotation = Quaternion.Euler(90f, 0f, 0f);
        pieces[1] = CreatePiece("Pressure Burst Steam Core", PrimitiveType.Sphere, Vector3.zero, new Vector3(0.22f, 0.16f, 0.22f), steamColor);
        pieces[2] = CreatePiece("Pressure Burst Brass Valve Flash", PrimitiveType.Cube, new Vector3(0f, -0.04f, -0.05f), new Vector3(0.18f, 0.04f, 0.32f), brassColor);

        for (int i = 3; i < pieces.Length; i++)
        {
            float angle = (i - 3) * Mathf.PI * 2f / (pieces.Length - 3);
            float radius = i % 2 == 0 ? 0.26f : 0.18f;
            Vector3 localPosition = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius * 0.72f, 0.08f + (i % 3) * 0.04f);
            Transform shard = CreatePiece("Pressure Burst Steam Shard " + i, PrimitiveType.Cube, localPosition, new Vector3(0.032f, 0.032f, 0.24f), i % 2 == 0 ? brassColor : warningColor);
            shard.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 28f);
            pieces[i] = shard;
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
