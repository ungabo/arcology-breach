using UnityEngine;

public class PressureBoltVfx : MonoBehaviour
{
    public float pulseSpeed = 9f;
    public float trailSpacing = 0.18f;

    private Transform[] pieces;
    private Vector3[] baseScales;
    private float scale = 1f;

    public int VisiblePieceCount => pieces == null ? 0 : pieces.Length;

    public static PressureBoltVfx Attach(GameObject boltObject, Color color, float effectScale)
    {
        PressureBoltVfx vfx = boltObject.AddComponent<PressureBoltVfx>();
        vfx.Initialize(color, effectScale);
        return vfx;
    }

    public void Initialize(Color color, float effectScale)
    {
        scale = Mathf.Max(0.2f, effectScale);

        Renderer coreRenderer = GetComponent<Renderer>();
        if (coreRenderer != null)
        {
            coreRenderer.material.color = color;
        }

        pieces = new Transform[5];
        baseScales = new Vector3[pieces.Length];
        pieces[0] = CreatePiece("Pressure Bolt Core Glow", PrimitiveType.Sphere, Vector3.zero, Vector3.one * 0.28f * scale, color);
        pieces[1] = CreatePiece("Pressure Bolt Trail A", PrimitiveType.Sphere, new Vector3(0f, 0f, -trailSpacing * scale), Vector3.one * 0.2f * scale, Color.Lerp(color, Color.white, 0.18f));
        pieces[2] = CreatePiece("Pressure Bolt Trail B", PrimitiveType.Sphere, new Vector3(0f, 0f, -trailSpacing * 2f * scale), Vector3.one * 0.15f * scale, color);
        pieces[3] = CreatePiece("Pressure Bolt Spark Left", PrimitiveType.Cube, new Vector3(-0.18f * scale, 0f, -0.09f * scale), new Vector3(0.05f, 0.05f, 0.28f) * scale, Color.Lerp(color, Color.yellow, 0.28f));
        pieces[4] = CreatePiece("Pressure Bolt Spark Right", PrimitiveType.Cube, new Vector3(0.18f * scale, 0f, -0.09f * scale), new Vector3(0.05f, 0.05f, 0.28f) * scale, Color.Lerp(color, Color.yellow, 0.28f));

        for (int i = 0; i < pieces.Length; i++)
        {
            baseScales[i] = pieces[i] == null ? Vector3.one : pieces[i].localScale;
        }
    }

    private void Update()
    {
        if (pieces == null || baseScales == null)
        {
            return;
        }

        transform.Rotate(Vector3.forward, 240f * Time.deltaTime, Space.Self);
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            float pulse = (Mathf.Sin(Time.time * pulseSpeed + i * 0.75f) + 1f) * 0.5f;
            pieces[i].localScale = baseScales[i] * (1f + pulse * 0.18f);
        }
    }

    private Transform CreatePiece(string pieceName, PrimitiveType type, Vector3 localPosition, Vector3 localScale, Color color)
    {
        GameObject piece = GameObject.CreatePrimitive(type);
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
