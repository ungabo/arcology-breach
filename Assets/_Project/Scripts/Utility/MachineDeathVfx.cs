using UnityEngine;

public class MachineDeathVfx : MonoBehaviour
{
    public const string EffectName = "Machine Death VFX";

    public enum DeathStyle
    {
        Standard,
        ScrapperShutdown
    }

    public float lifetime = 0.85f;
    public float riseSpeed = 0.28f;
    public float spinSpeed = 120f;

    private float age;
    private Transform[] pieces;
    private Vector3[] basePositions;
    private Vector3[] baseScales;
    private Vector3[] driftDirections;
    private float[] driftSpeeds;
    private DeathStyle deathStyle = DeathStyle.Standard;

    public static MachineDeathVfx Spawn(Vector3 position, float effectScale = 1f)
    {
        return SpawnWithStyle(position, effectScale, DeathStyle.Standard);
    }

    public static MachineDeathVfx SpawnScrapperShutdown(Vector3 position)
    {
        return SpawnWithStyle(position, 1f, DeathStyle.ScrapperShutdown);
    }

    private static MachineDeathVfx SpawnWithStyle(Vector3 position, float effectScale, DeathStyle style)
    {
        GameObject root = new GameObject(EffectName);
        root.transform.position = position;
        root.transform.localScale = Vector3.one * effectScale;

        MachineDeathVfx vfx = root.AddComponent<MachineDeathVfx>();
        vfx.deathStyle = style;
        vfx.Build();
        return vfx;
    }

    public int PieceCount => transform.childCount;
    public bool IsScrapperShutdown => deathStyle == DeathStyle.ScrapperShutdown;
    public bool HasScrapperShutdownDetail => IsScrapperShutdown && PieceCount >= 18;

    private void Awake()
    {
    }

    private void Update()
    {
        if (pieces == null)
        {
            Build();
        }

        age += Time.deltaTime;
        float normalizedAge = Mathf.Clamp01(age / lifetime);

        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                pieces[i].localScale = baseScales[i] * Mathf.Lerp(1f, 0.25f, normalizedAge);
                pieces[i].localPosition = basePositions[i]
                    + driftDirections[i] * (driftSpeeds[i] * normalizedAge)
                    + pieces[i].localRotation * Vector3.up * (0.12f * age);
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

        pieces = new Transform[deathStyle == DeathStyle.ScrapperShutdown ? 20 : 11];
        basePositions = new Vector3[pieces.Length];
        baseScales = new Vector3[pieces.Length];
        driftDirections = new Vector3[pieces.Length];
        driftSpeeds = new float[pieces.Length];

        int pieceIndex = 0;
        pieces[pieceIndex++] = CreatePrimitivePiece("Machine Death Pressure Ring", PrimitiveType.Cylinder, Vector3.zero, new Vector3(1.25f, 0.025f, 1.25f), new Color(1f, 0.48f, 0.12f));

        for (int i = 1; i <= 4; i++)
        {
            float angle = i * Mathf.PI * 0.5f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.24f, 0.26f, Mathf.Sin(angle) * 0.24f);
            Transform steam = CreatePrimitivePiece("Machine Death Steam Puff " + i, PrimitiveType.Cube, position, new Vector3(0.18f, 0.5f, 0.18f), new Color(0.76f, 0.74f, 0.67f));
            steam.localRotation = Quaternion.Euler(24f, angle * Mathf.Rad2Deg, 18f);
            pieces[pieceIndex++] = steam;
        }

        for (int i = 0; i < 6; i++)
        {
            float angle = i * Mathf.PI * 2f / 6f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * 0.44f, 0.18f + (i % 2) * 0.1f, Mathf.Sin(angle) * 0.44f);
            Transform spark = CreatePrimitivePiece("Machine Death Brass Spark " + i, PrimitiveType.Cube, position, new Vector3(0.035f, 0.035f, 0.24f), i % 2 == 0 ? new Color(1f, 0.62f, 0.08f) : new Color(1f, 0.88f, 0.32f));
            spark.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 45f + i * 12f);
            pieces[pieceIndex++] = spark;
        }

        if (deathStyle == DeathStyle.ScrapperShutdown)
        {
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Boiler Cap", PrimitiveType.Cylinder, new Vector3(0f, 0.34f, -0.04f), new Vector3(0.34f, 0.12f, 0.34f), new Color(0.5f, 0.36f, 0.22f));
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Chest Plate", PrimitiveType.Cube, new Vector3(0f, 0.18f, 0.38f), new Vector3(0.48f, 0.12f, 0.12f), new Color(0.88f, 0.55f, 0.18f));
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Left Cutter Shard", PrimitiveType.Cube, new Vector3(-0.42f, 0.08f, 0.36f), new Vector3(0.08f, 0.32f, 0.08f), new Color(0.72f, 0.7f, 0.64f));
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Right Cutter Shard", PrimitiveType.Cube, new Vector3(0.42f, 0.08f, 0.36f), new Vector3(0.08f, 0.32f, 0.08f), new Color(0.72f, 0.7f, 0.64f));
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Flywheel Gear A", PrimitiveType.Cylinder, new Vector3(-0.28f, 0.42f, -0.22f), new Vector3(0.2f, 0.045f, 0.2f), new Color(0.95f, 0.65f, 0.2f));
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Flywheel Gear B", PrimitiveType.Cylinder, new Vector3(0.3f, 0.32f, -0.2f), new Vector3(0.16f, 0.04f, 0.16f), new Color(0.95f, 0.65f, 0.2f));
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Pressure Tank Burst", PrimitiveType.Sphere, new Vector3(0f, 0.24f, -0.42f), new Vector3(0.28f, 0.2f, 0.18f), new Color(0.72f, 0.74f, 0.68f));
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Valve Wheel", PrimitiveType.Cylinder, new Vector3(0.5f, 0.2f, -0.1f), new Vector3(0.16f, 0.035f, 0.16f), new Color(0.8f, 0.46f, 0.14f));
            pieces[pieceIndex++] = CreatePrimitivePiece("Scrapper Shutdown Furnace Flash", PrimitiveType.Sphere, new Vector3(0f, 0.24f, 0.48f), new Vector3(0.22f, 0.14f, 0.08f), new Color(1f, 0.28f, 0.04f));
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            basePositions[i] = pieces[i].localPosition;
            baseScales[i] = pieces[i].localScale;
            driftDirections[i] = BuildDriftDirection(i, basePositions[i]);
            driftSpeeds[i] = deathStyle == DeathStyle.ScrapperShutdown ? 0.22f + i * 0.018f : 0.12f + i * 0.01f;
        }
    }

    private static Vector3 BuildDriftDirection(int index, Vector3 basePosition)
    {
        float angle = (index * 137.5f) * Mathf.Deg2Rad;
        Vector3 outward = new Vector3(Mathf.Cos(angle), 0.25f + (index % 3) * 0.12f, Mathf.Sin(angle));
        if (basePosition.sqrMagnitude > 0.001f)
        {
            outward += basePosition.normalized * 0.65f;
        }

        return outward.normalized;
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
