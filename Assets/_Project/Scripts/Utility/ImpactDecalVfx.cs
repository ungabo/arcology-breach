using UnityEngine;

public class ImpactDecalVfx : MonoBehaviour
{
    private Transform[] pieces;
    private Vector3[] baseScales;

    public int PieceCount => pieces == null ? 0 : pieces.Length;

    public static ImpactDecalVfx Spawn(Vector3 point, Vector3 normal)
    {
        GameObject root = new GameObject("Impact Decal VFX");
        root.transform.position = point + normal.normalized * 0.045f;
        root.transform.rotation = Quaternion.LookRotation(normal.sqrMagnitude > 0.001f ? normal.normalized : Vector3.up);

        ImpactDecalVfx vfx = root.AddComponent<ImpactDecalVfx>();
        vfx.BuildPieces();
        Destroy(root, 1.2f);
        return vfx;
    }

    private void Update()
    {
        if (pieces == null || baseScales == null)
        {
            return;
        }

        float pulse = 1f + Mathf.Sin(Time.time * 16f) * 0.025f;
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                pieces[i].localScale = baseScales[i] * pulse;
            }
        }
    }

    private void BuildPieces()
    {
        pieces = new Transform[8];
        baseScales = new Vector3[pieces.Length];

        pieces[0] = CreatePiece("Pressure Impact Scorch Disc", PrimitiveType.Cylinder, Vector3.zero, new Vector3(0.26f, 0.01f, 0.26f), new Color(0.05f, 0.045f, 0.035f), Quaternion.Euler(90f, 0f, 0f));
        pieces[1] = CreatePiece("Pressure Impact Brass Ring", PrimitiveType.Cylinder, new Vector3(0f, 0f, -0.004f), new Vector3(0.18f, 0.012f, 0.18f), new Color(0.95f, 0.55f, 0.12f), Quaternion.Euler(90f, 0f, 0f));

        for (int i = 2; i < pieces.Length; i++)
        {
            float side = i - 4.5f;
            pieces[i] = CreatePiece(
                "Pressure Impact Brass Spark " + i,
                PrimitiveType.Cube,
                new Vector3(side * 0.025f, Mathf.Abs(side) * 0.018f, 0.035f + i * 0.01f),
                new Vector3(0.018f, 0.018f, 0.14f + i * 0.012f),
                i % 2 == 0 ? new Color(1f, 0.62f, 0.08f) : new Color(1f, 0.92f, 0.32f),
                Quaternion.Euler(0f, 0f, side * 18f));
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            baseScales[i] = pieces[i] == null ? Vector3.one : pieces[i].localScale;
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
