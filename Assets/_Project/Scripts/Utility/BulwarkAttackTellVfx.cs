using UnityEngine;

public class BulwarkAttackTellVfx : MonoBehaviour
{
    public const string EffectName = "Bulwark Attack Tell VFX";

    public Transform leftHammerArm;
    public Transform rightHammerArm;
    public Transform leftHammerHead;
    public Transform rightHammerHead;
    public Transform furnaceBelly;
    public Transform backPressureTank;
    public Color warningColor = new Color(1f, 0.14f, 0.03f, 0.96f);
    public Color brassColor = new Color(1f, 0.66f, 0.12f, 0.95f);
    public Color steamColor = new Color(0.72f, 0.74f, 0.68f, 0.84f);

    private Transform tellRoot;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;
    private float activeDuration = 0.85f;
    private float activeAge;
    private bool active;

    public bool IsConfigured => leftHammerArm != null && rightHammerArm != null && leftHammerHead != null && rightHammerHead != null && furnaceBelly != null && backPressureTank != null;
    public bool IsActive => active;
    public int PieceCount => pieces == null ? 0 : pieces.Length;

    private void Awake()
    {
        AutoAssign();
        Build();
        SetVisible(false);
    }

    private void Update()
    {
        if (!active)
        {
            return;
        }

        activeAge += Time.deltaTime;
        float t = Mathf.Clamp01(activeAge / Mathf.Max(0.001f, activeDuration));
        float charge = Mathf.SmoothStep(0f, 1f, t);
        float pulse = 0.74f + Mathf.Sin(Time.time * 30f) * 0.26f;
        float finalSnap = Mathf.SmoothStep(0.68f, 1f, t);

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

            pieces[i].localScale = baseScales[i] * Mathf.Lerp(0.72f, 1.18f + pulse * 0.16f, charge);
        }

        pieces[0].localScale = new Vector3(Mathf.Lerp(1.25f, 2.85f, charge), 0.026f, Mathf.Lerp(1.25f, 2.85f, charge));
        pieces[0].localRotation = Quaternion.Euler(0f, Time.time * 105f, 0f);
        pieces[1].localPosition = Vector3.Lerp(basePositions[1], new Vector3(-1.08f, -0.66f, 0.78f), charge);
        pieces[1].localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-8f, -28f, charge));
        pieces[2].localPosition = Vector3.Lerp(basePositions[2], new Vector3(1.08f, -0.66f, 0.78f), charge);
        pieces[2].localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(8f, 28f, charge));
        pieces[3].localScale = baseScales[3] * Mathf.Lerp(1f, 1.75f + pulse * 0.22f, charge);
        pieces[4].localScale = baseScales[4] * Mathf.Lerp(0.82f, 1.68f, charge);
        pieces[5].localRotation = Quaternion.Euler(90f, Time.time * 185f, 0f);
        pieces[5].localScale = new Vector3(Mathf.Lerp(0.24f, 0.78f, charge), 0.018f, Mathf.Lerp(0.24f, 0.78f, charge));
        pieces[6].localScale = baseScales[6] * Mathf.Lerp(0.85f, 1.85f, finalSnap);
        pieces[7].localScale = baseScales[7] * Mathf.Lerp(0.85f, 1.85f, finalSnap);

        for (int i = 8; i < pieces.Length; i++)
        {
            float offset = i * 0.19f;
            pieces[i].localPosition = basePositions[i] + Vector3.up * Mathf.Sin(Time.time * 23f + offset) * 0.06f * charge;
            pieces[i].localRotation = Quaternion.Euler(Time.time * (70f + i * 8f), 0f, 18f + i * 23f);
        }

        if (activeAge >= activeDuration)
        {
            StopTell();
        }
    }

    public void StartTell(float duration)
    {
        AutoAssign();
        Build();
        activeDuration = Mathf.Max(0.1f, duration);
        activeAge = 0f;
        active = true;
        SetVisible(true);
    }

    public void StopTell()
    {
        active = false;
        SetVisible(false);
    }

    private void AutoAssign()
    {
        if (leftHammerArm == null)
        {
            leftHammerArm = transform.Find("Bulwark Left Hammer Arm");
        }

        if (rightHammerArm == null)
        {
            rightHammerArm = transform.Find("Bulwark Right Hammer Arm");
        }

        if (leftHammerHead == null)
        {
            leftHammerHead = transform.Find("Bulwark Left Hammer Head");
        }

        if (rightHammerHead == null)
        {
            rightHammerHead = transform.Find("Bulwark Right Hammer Head");
        }

        if (furnaceBelly == null)
        {
            furnaceBelly = transform.Find("Bulwark Furnace Belly");
        }

        if (backPressureTank == null)
        {
            backPressureTank = transform.Find("Bulwark Back Pressure Tank");
        }
    }

    private void Build()
    {
        if (pieces != null)
        {
            return;
        }

        GameObject root = new GameObject(EffectName);
        root.transform.SetParent(transform, false);
        root.transform.localPosition = Vector3.zero;
        tellRoot = root.transform;

        pieces = new Transform[12];
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];

        pieces[0] = CreatePiece("Bulwark Slam Ground Warning Ring", PrimitiveType.Cylinder, new Vector3(0f, -1.22f, 0.46f), new Vector3(1.25f, 0.026f, 1.25f), warningColor);
        pieces[1] = CreatePiece("Bulwark Left Hammer Lift Plate", PrimitiveType.Cube, new Vector3(-1.08f, -0.84f, 0.52f), new Vector3(0.56f, 0.08f, 0.44f), warningColor);
        pieces[2] = CreatePiece("Bulwark Right Hammer Lift Plate", PrimitiveType.Cube, new Vector3(1.08f, -0.84f, 0.52f), new Vector3(0.56f, 0.08f, 0.44f), warningColor);
        pieces[3] = CreatePiece("Bulwark Furnace Belly Overpressure", PrimitiveType.Sphere, new Vector3(0f, -0.08f, 0.52f), new Vector3(0.52f, 0.28f, 0.08f), warningColor);
        pieces[4] = CreatePiece("Bulwark Back Tank Steam Surge", PrimitiveType.Sphere, new Vector3(0f, 0.14f, -0.52f), new Vector3(0.34f, 0.3f, 0.2f), steamColor);
        pieces[5] = CreatePiece("Bulwark Chest Pressure Timing Ring", PrimitiveType.Cylinder, new Vector3(0f, 0.42f, 0.58f), new Vector3(0.24f, 0.018f, 0.24f), brassColor);
        pieces[6] = CreatePiece("Bulwark Left Hammer Spark Crown", PrimitiveType.Cube, new Vector3(-1.08f, -0.66f, 0.82f), new Vector3(0.08f, 0.08f, 0.36f), brassColor);
        pieces[7] = CreatePiece("Bulwark Right Hammer Spark Crown", PrimitiveType.Cube, new Vector3(1.08f, -0.66f, 0.82f), new Vector3(0.08f, 0.08f, 0.36f), brassColor);
        pieces[8] = CreatePiece("Bulwark Windup Steam Vent A", PrimitiveType.Sphere, new Vector3(-0.42f, 0.56f, -0.34f), new Vector3(0.2f, 0.14f, 0.2f), steamColor);
        pieces[9] = CreatePiece("Bulwark Windup Steam Vent B", PrimitiveType.Sphere, new Vector3(0.42f, 0.56f, -0.34f), new Vector3(0.2f, 0.14f, 0.2f), steamColor);
        pieces[10] = CreatePiece("Bulwark Brass Warning Spark A", PrimitiveType.Cube, new Vector3(-0.58f, 0.16f, 0.62f), new Vector3(0.05f, 0.05f, 0.2f), brassColor);
        pieces[11] = CreatePiece("Bulwark Brass Warning Spark B", PrimitiveType.Cube, new Vector3(0.58f, 0.16f, 0.62f), new Vector3(0.05f, 0.05f, 0.2f), brassColor);

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
        piece.transform.SetParent(tellRoot, false);
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

    private void SetVisible(bool visible)
    {
        if (tellRoot != null)
        {
            tellRoot.gameObject.SetActive(visible);
        }
    }
}
