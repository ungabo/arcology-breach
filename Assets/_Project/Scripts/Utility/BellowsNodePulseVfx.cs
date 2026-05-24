using UnityEngine;

public class BellowsNodePulseVfx : MonoBehaviour
{
    public const string EffectName = "Bellows Node Pressure Pulse VFX";

    public float lifetime = 0.5f;
    public float ringExpansion = 2.4f;

    private float age;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;

    public int PieceCount => transform.childCount;

    public static BellowsNodePulseVfx Spawn(Vector3 position)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        BellowsNodePulseVfx vfx = root.AddComponent<BellowsNodePulseVfx>();
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
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            pieces[i].localPosition = basePositions[i] * Mathf.Lerp(1f, ringExpansion, t);
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

        pieces = new Transform[12];
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];

        Color steamColor = new Color(0.72f, 0.72f, 0.68f, 0.9f);
        Color brassColor = new Color(0.95f, 0.58f, 0.18f, 0.95f);
        Color warningColor = new Color(1f, 0.25f, 0.08f, 0.95f);

        pieces[0] = CreatePiece("Bellows Pulse Steam Core", PrimitiveType.Sphere, Vector3.zero, new Vector3(0.48f, 0.2f, 0.48f), steamColor);
        pieces[1] = CreatePiece("Bellows Pulse Brass Crown", PrimitiveType.Cylinder, new Vector3(0f, 0.02f, 0f), new Vector3(0.72f, 0.035f, 0.72f), brassColor);

        for (int i = 2; i < pieces.Length; i++)
        {
            float angle = (i - 2) * Mathf.PI * 2f / (pieces.Length - 2);
            Vector3 localPosition = new Vector3(Mathf.Cos(angle) * 0.48f, 0f, Mathf.Sin(angle) * 0.48f);
            Transform spoke = CreatePiece("Bellows Pulse Pressure Spoke " + i, PrimitiveType.Cube, localPosition, new Vector3(0.08f, 0.045f, 0.32f), i % 2 == 0 ? brassColor : warningColor);
            spoke.localRotation = Quaternion.Euler(0f, -angle * Mathf.Rad2Deg, 0f);
            pieces[i] = spoke;
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
