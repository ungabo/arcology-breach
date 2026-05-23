using UnityEngine;

public class WardenShutdownVfx : MonoBehaviour
{
    public const string EffectName = "Warden Shutdown VFX";

    public float lifetime = 1.35f;
    public float riseSpeed = 0.42f;
    public float spinSpeed = 80f;

    private float age;
    private Transform pressureRing;
    private Transform[] steamJets;
    private Transform[] sparks;
    private Vector3[] steamBaseScales;
    private Vector3[] sparkBaseScales;

    public static WardenShutdownVfx Spawn(Vector3 position)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;

        WardenShutdownVfx vfx = root.AddComponent<WardenShutdownVfx>();
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
        if (steamJets == null || sparks == null)
        {
            Build();
        }

        age += Time.deltaTime;
        float normalizedAge = Mathf.Clamp01(age / lifetime);

        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        if (pressureRing != null)
        {
            float ringScale = Mathf.Lerp(1f, 1.75f, normalizedAge);
            pressureRing.localScale = new Vector3(2.3f * ringScale, 0.035f, 2.3f * ringScale);
        }

        for (int i = 0; i < steamJets.Length; i++)
        {
            if (steamJets[i] != null)
            {
                steamJets[i].localScale = steamBaseScales[i] * Mathf.Lerp(1f, 1.85f, normalizedAge);
                steamJets[i].localPosition += steamJets[i].localRotation * Vector3.up * (0.18f * Time.deltaTime);
            }
        }

        for (int i = 0; i < sparks.Length; i++)
        {
            if (sparks[i] != null)
            {
                sparks[i].localScale = sparkBaseScales[i] * Mathf.Lerp(1f, 0.35f, normalizedAge);
                sparks[i].Rotate(Vector3.forward, 480f * Time.deltaTime);
            }
        }

        if (age >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void Build()
    {
        if (steamJets != null || sparks != null)
        {
            return;
        }

        pressureRing = CreatePrimitivePiece("Warden Shutdown Pressure Ring", PrimitiveType.Cylinder, Vector3.zero, new Vector3(2.3f, 0.035f, 2.3f), new Color(1f, 0.42f, 0.08f));

        steamJets = new Transform[8];
        steamBaseScales = new Vector3[steamJets.Length];
        for (int i = 0; i < steamJets.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / steamJets.Length;
            Vector3 localPosition = new Vector3(Mathf.Cos(angle) * 0.42f, 0.35f + (i % 2) * 0.12f, Mathf.Sin(angle) * 0.42f);
            Transform jet = CreatePrimitivePiece("Warden Shutdown Steam Jet " + i, PrimitiveType.Cube, localPosition, new Vector3(0.16f, 0.8f, 0.16f), new Color(0.78f, 0.76f, 0.68f));
            jet.localRotation = Quaternion.Euler(22f, -angle * Mathf.Rad2Deg, 18f);
            steamJets[i] = jet;
            steamBaseScales[i] = jet.localScale;
        }

        sparks = new Transform[10];
        sparkBaseScales = new Vector3[sparks.Length];
        for (int i = 0; i < sparks.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / sparks.Length;
            Vector3 localPosition = new Vector3(Mathf.Cos(angle) * 0.72f, 0.18f + (i % 3) * 0.14f, Mathf.Sin(angle) * 0.72f);
            Transform spark = CreatePrimitivePiece("Warden Shutdown Brass Spark " + i, PrimitiveType.Cube, localPosition, new Vector3(0.045f, 0.045f, 0.34f), i % 2 == 0 ? new Color(1f, 0.62f, 0.08f) : new Color(1f, 0.88f, 0.32f));
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 50f + i * 9f);
            sparks[i] = spark;
            sparkBaseScales[i] = spark.localScale;
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
