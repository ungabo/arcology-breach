using UnityEngine;

public class PlayerDamageVfx : MonoBehaviour
{
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;
    private float effectIntensity = 1f;

    public int PieceCount => pieces == null ? 0 : pieces.Length;

    public static PlayerDamageVfx Spawn(Transform fallbackParent)
    {
        Camera camera = Camera.main;
        Transform parent = camera != null ? camera.transform : fallbackParent;

        GameObject root = new GameObject("Player Damage VFX");
        if (parent != null)
        {
            root.transform.SetParent(parent, false);
            root.transform.localPosition = new Vector3(0f, 0f, 0.62f);
            root.transform.localRotation = Quaternion.identity;
        }

        PlayerDamageVfx vfx = root.AddComponent<PlayerDamageVfx>();
        GameSettings.Load();
        vfx.effectIntensity = GameSettings.FlashIntensity;
        vfx.BuildPieces();
        Destroy(root, Mathf.Lerp(0.35f, 0.65f, vfx.effectIntensity));
        return vfx;
    }

    private void Update()
    {
        if (pieces == null || basePositions == null || baseScales == null)
        {
            return;
        }

        float agePulse = Mathf.Sin(Time.time * 18f) * 0.04f;
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            float side = i % 2 == 0 ? -1f : 1f;
            pieces[i].localPosition = basePositions[i] + new Vector3(side * agePulse, Mathf.Abs(agePulse) * 0.6f, 0f);
            pieces[i].localScale = baseScales[i] * (1f + Mathf.Abs(agePulse) * 2.5f);
        }
    }

    private void BuildPieces()
    {
        pieces = new Transform[8];
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];

        pieces[0] = CreatePiece("Player Damage Left Pressure Slash", PrimitiveType.Cube, new Vector3(-0.34f, 0.05f, 0f), new Vector3(0.035f, 0.22f, 0.02f), new Color(0.95f, 0.08f, 0.035f));
        pieces[1] = CreatePiece("Player Damage Right Pressure Slash", PrimitiveType.Cube, new Vector3(0.34f, 0.05f, 0f), new Vector3(0.035f, 0.22f, 0.02f), new Color(0.95f, 0.08f, 0.035f));
        pieces[2] = CreatePiece("Player Damage Top Steam Edge", PrimitiveType.Cube, new Vector3(-0.15f, 0.22f, 0.01f), new Vector3(0.22f, 0.028f, 0.02f), new Color(1f, 0.36f, 0.08f));
        pieces[3] = CreatePiece("Player Damage Top Steam Edge R", PrimitiveType.Cube, new Vector3(0.15f, 0.22f, 0.01f), new Vector3(0.22f, 0.028f, 0.02f), new Color(1f, 0.36f, 0.08f));
        pieces[4] = CreatePiece("Player Damage Brass Spark A", PrimitiveType.Cube, new Vector3(-0.22f, -0.16f, 0.02f), new Vector3(0.024f, 0.024f, 0.16f), new Color(1f, 0.68f, 0.16f));
        pieces[5] = CreatePiece("Player Damage Brass Spark B", PrimitiveType.Cube, new Vector3(0.22f, -0.16f, 0.02f), new Vector3(0.024f, 0.024f, 0.16f), new Color(1f, 0.68f, 0.16f));
        pieces[6] = CreatePiece("Player Damage Lower Heat Edge", PrimitiveType.Cube, new Vector3(-0.1f, -0.24f, 0.015f), new Vector3(0.18f, 0.026f, 0.02f), new Color(0.75f, 0.02f, 0.02f));
        pieces[7] = CreatePiece("Player Damage Lower Heat Edge R", PrimitiveType.Cube, new Vector3(0.1f, -0.24f, 0.015f), new Vector3(0.18f, 0.026f, 0.02f), new Color(0.75f, 0.02f, 0.02f));

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                basePositions[i] = pieces[i].localPosition;
                baseScales[i] = pieces[i].localScale;
            }
        }
    }

    private Transform CreatePiece(string pieceName, PrimitiveType type, Vector3 localPosition, Vector3 localScale, Color color)
    {
        GameObject piece = GameObject.CreatePrimitive(type);
        piece.name = pieceName;
        piece.transform.SetParent(transform, false);
        piece.transform.localPosition = localPosition;
        piece.transform.localScale = localScale * Mathf.Lerp(0.45f, 1f, effectIntensity);

        Collider pieceCollider = piece.GetComponent<Collider>();
        if (pieceCollider != null)
        {
            Destroy(pieceCollider);
        }

        Renderer pieceRenderer = piece.GetComponent<Renderer>();
        if (pieceRenderer != null)
        {
            pieceRenderer.material.color = Color.Lerp(new Color(0.16f, 0.05f, 0.03f), color, effectIntensity);
        }

        return piece.transform;
    }
}
