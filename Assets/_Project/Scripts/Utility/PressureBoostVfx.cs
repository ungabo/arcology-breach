using UnityEngine;

public class PressureBoostVfx : MonoBehaviour
{
    public const string EffectName = "Pressure Boost VFX";

    public float effectScale = 1f;
    public float spinSpeed = 82f;
    public float pulseAmount = 0.08f;

    private float boostUntil;
    private Transform[] pieces;
    private Renderer[] renderers;
    private Vector3[] basePositions;
    private Vector3[] baseScales;

    public bool IsActive => Time.time < boostUntil;

    public int VisiblePieceCount
    {
        get
        {
            Build();

            int count = 0;
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].enabled)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static PressureBoostVfx StartBoost(GameObject target, float duration, float effectScale = 1f)
    {
        PressureBoostVfx vfx = target.GetComponent<PressureBoostVfx>();
        if (vfx == null)
        {
            vfx = target.AddComponent<PressureBoostVfx>();
        }

        vfx.effectScale = effectScale;
        vfx.Begin(duration);
        return vfx;
    }

    private void Awake()
    {
        Build();
        SetVisible(false);
    }

    private void Update()
    {
        Build();

        if (!IsActive)
        {
            SetVisible(false);
            return;
        }

        SetVisible(true);
        float phase = Time.time * 5.5f;
        float pulse = 1f + (Mathf.Sin(phase) + 1f) * 0.5f * pulseAmount;
        float spin = Time.time * spinSpeed;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            if (i == 0)
            {
                pieces[i].localScale = baseScales[i] * effectScale * pulse;
                pieces[i].localRotation = Quaternion.Euler(0f, spin, 0f);
                continue;
            }

            Quaternion rotation = Quaternion.Euler(0f, spin + i * 17f, 0f);
            pieces[i].localPosition = rotation * basePositions[i] + Vector3.up * (Mathf.Sin(phase + i) * 0.035f);
            pieces[i].localScale = baseScales[i] * effectScale * Mathf.Lerp(0.92f, 1.18f, (Mathf.Sin(phase + i * 0.7f) + 1f) * 0.5f);
            pieces[i].localRotation = rotation * Quaternion.Euler(0f, 0f, 24f + i * 9f);
        }
    }

    public void Begin(float duration)
    {
        if (duration <= 0f)
        {
            return;
        }

        Build();
        boostUntil = Mathf.Max(boostUntil, Time.time + duration);
        SetVisible(true);
    }

    private void Build()
    {
        if (pieces != null)
        {
            return;
        }

        pieces = new Transform[9];
        renderers = new Renderer[pieces.Length];
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];

        Color ringColor = new Color(1f, 0.42f, 0.08f, 0.95f);
        Color steamColor = new Color(0.82f, 0.78f, 0.66f, 0.95f);
        Color brassColor = new Color(1f, 0.74f, 0.2f, 0.95f);

        pieces[0] = CreatePiece("Pressure Boost Overdrive Ring", PrimitiveType.Cylinder, new Vector3(0f, 0.58f, 0f), new Vector3(0.88f, 0.035f, 0.88f), ringColor);

        for (int i = 1; i < pieces.Length; i++)
        {
            float angle = (i - 1) * Mathf.PI * 2f / (pieces.Length - 1);
            Vector3 localPosition = new Vector3(Mathf.Cos(angle) * 0.5f, 0.52f + (i % 2) * 0.24f, Mathf.Sin(angle) * 0.5f);
            Transform spoke = CreatePiece("Pressure Boost Brass Vent " + i, PrimitiveType.Cube, localPosition, new Vector3(0.05f, 0.05f, 0.28f), i % 2 == 0 ? brassColor : steamColor);
            spoke.localRotation = Quaternion.Euler(0f, -angle * Mathf.Rad2Deg, 32f);
            pieces[i] = spoke;
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            renderers[i] = pieces[i].GetComponent<Renderer>();
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

    private void SetVisible(bool visible)
    {
        if (renderers == null)
        {
            return;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].enabled = visible;
            }
        }
    }
}
