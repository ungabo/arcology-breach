using UnityEngine;

public class PressureBoltImpactVfx : MonoBehaviour
{
    public const string EffectName = "Pressure Bolt Impact VFX";

    public float lifetime = 0.68f;
    public float burstSpeed = 0.42f;

    private float age;
    private float effectScale = 1f;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;
    private Vector3[] burstDirections;

    public int PieceCount => pieces == null ? transform.childCount : pieces.Length;

    public static PressureBoltImpactVfx Spawn(Vector3 point, Vector3 normal, Color boltColor, float scale = 1f)
    {
        Vector3 resolvedNormal = normal.sqrMagnitude > 0.001f ? normal.normalized : Vector3.up;
        GameObject root = new GameObject(EffectName);
        root.transform.position = point + resolvedNormal * 0.08f;
        root.transform.rotation = Quaternion.LookRotation(resolvedNormal);

        PressureBoltImpactVfx vfx = root.AddComponent<PressureBoltImpactVfx>();
        vfx.effectScale = Mathf.Max(0.35f, scale);
        vfx.Build(boltColor);
        return vfx;
    }

    private void Update()
    {
        if (pieces == null || basePositions == null || baseScales == null || burstDirections == null)
        {
            return;
        }

        age += Time.deltaTime;
        float normalizedAge = Mathf.Clamp01(age / lifetime);
        float pressurePulse = 1f + Mathf.Sin(Time.time * 26f) * 0.04f;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            float outward = Mathf.Sin(normalizedAge * Mathf.PI) * burstSpeed;
            pieces[i].localPosition = basePositions[i] + burstDirections[i] * outward;

            if (i == 1 || i == 2)
            {
                pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 1.85f, normalizedAge) * pressurePulse;
            }
            else if (i == 3)
            {
                pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 1.35f, normalizedAge);
            }
            else
            {
                pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.3f, normalizedAge);
            }
        }

        transform.Rotate(Vector3.forward, 95f * Time.deltaTime, Space.Self);

        if (age >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void Build(Color boltColor)
    {
        if (pieces != null)
        {
            return;
        }

        Color heatColor = Color.Lerp(boltColor, new Color(1f, 0.72f, 0.14f), 0.35f);
        Color brassColor = new Color(1f, 0.62f, 0.12f);
        Color steamColor = new Color(0.72f, 0.7f, 0.62f);

        pieces = new Transform[13];
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];
        burstDirections = new Vector3[pieces.Length];

        pieces[0] = CreatePiece("Pressure Bolt Impact Flash Core", PrimitiveType.Sphere, Vector3.zero, new Vector3(0.28f, 0.28f, 0.08f) * effectScale, Color.Lerp(heatColor, Color.white, 0.4f), Quaternion.identity);
        pieces[1] = CreatePiece("Pressure Bolt Impact Pressure Ring", PrimitiveType.Cylinder, new Vector3(0f, 0f, 0.01f) * effectScale, new Vector3(0.5f, 0.018f, 0.5f) * effectScale, heatColor, Quaternion.Euler(90f, 0f, 0f));
        pieces[2] = CreatePiece("Pressure Bolt Impact Brass Ring", PrimitiveType.Cylinder, new Vector3(0f, 0f, 0.025f) * effectScale, new Vector3(0.32f, 0.014f, 0.32f) * effectScale, brassColor, Quaternion.Euler(90f, 0f, 0f));
        pieces[3] = CreatePiece("Pressure Bolt Impact Steam Pop", PrimitiveType.Sphere, new Vector3(0f, 0.05f, 0.12f) * effectScale, new Vector3(0.32f, 0.22f, 0.18f) * effectScale, steamColor, Quaternion.identity);

        for (int i = 4; i < pieces.Length; i++)
        {
            int sparkIndex = i - 4;
            float angle = sparkIndex * Mathf.PI * 2f / (pieces.Length - 4);
            Vector3 radial = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 position = (radial * (0.12f + sparkIndex % 3 * 0.035f) + Vector3.forward * (0.08f + sparkIndex * 0.012f)) * effectScale;
            Vector3 scale = new Vector3(0.028f, 0.028f, 0.18f + (sparkIndex % 4) * 0.035f) * effectScale;
            Color sparkColor = sparkIndex % 3 == 0 ? brassColor : Color.Lerp(heatColor, Color.yellow, 0.24f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg) * Quaternion.Euler(32f + sparkIndex * 5f, 0f, 0f);

            pieces[i] = CreatePiece("Pressure Bolt Impact Brass Shard " + sparkIndex, PrimitiveType.Cube, position, scale, sparkColor, rotation);
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            basePositions[i] = pieces[i] == null ? Vector3.zero : pieces[i].localPosition;
            baseScales[i] = pieces[i] == null ? Vector3.one : pieces[i].localScale;
            Vector3 planarDirection = new Vector3(basePositions[i].x, basePositions[i].y, 0f);
            burstDirections[i] = planarDirection.sqrMagnitude > 0.001f ? planarDirection.normalized : Vector3.forward;
        }
    }

    private Transform CreatePiece(string pieceName, PrimitiveType type, Vector3 localPosition, Vector3 localScale, Color color, Quaternion localRotation)
    {
        GameObject piece = GameObject.CreatePrimitive(type);
        piece.name = pieceName;
        piece.transform.SetParent(transform, false);
        piece.transform.localPosition = localPosition;
        piece.transform.localRotation = localRotation;
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
