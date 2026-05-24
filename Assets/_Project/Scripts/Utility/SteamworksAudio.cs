using System;
using System.Collections.Generic;
using UnityEngine;

public enum SteamworksAudioCue
{
    PressureFire,
    EmptyClick,
    HealthPickup,
    AmmoPickup,
    GearKey,
    GateOpen,
    GateDenied,
    EnemyHit,
    EnemyDeath,
    PlayerHurt,
    Win,
    SteamScattergunFire,
    BellowsNodePulse,
    WeaponPickup,
    SteamScattergunSlug,
    PressureBurst,
    EnemyAttackTell,
    LancerFireTell,
    BulwarkAttackTell
}

[Serializable]
public class SteamworksAudioClipBinding
{
    public SteamworksAudioCue cue;
    public AudioClip clip;
}

[RequireComponent(typeof(AudioSource))]
public class SteamworksAudio : MonoBehaviour
{
    public static SteamworksAudio Instance { get; private set; }

    public float masterVolume = 0.55f;
    public bool ambienceEnabled = true;
    public float ambienceVolume = 0.16f;
    public bool preferAuthoredClips = true;
    public AudioClip authoredAmbienceLoop;
    public SteamworksAudioClipBinding[] authoredCueClips = Array.Empty<SteamworksAudioClipBinding>();

    private const int SampleRate = 44100;
    private const float AmbienceDuration = 4f;

    private readonly Dictionary<SteamworksAudioCue, AudioClip> clips = new Dictionary<SteamworksAudioCue, AudioClip>();
    private readonly HashSet<SteamworksAudioCue> authoredActiveCues = new HashSet<SteamworksAudioCue>();
    private AudioSource source;

    public bool AmbienceActive => source != null && source.loop && source.clip != null && source.isPlaying;
    public int AmbienceSampleCount => source != null && source.clip != null ? source.clip.samples : 0;
    public bool UsingAuthoredAmbience => preferAuthoredClips && source != null && authoredAmbienceLoop != null && source.clip == authoredAmbienceLoop;
    public int AuthoredCueCount
    {
        get
        {
            HashSet<SteamworksAudioCue> uniqueCues = new HashSet<SteamworksAudioCue>();
            if (authoredCueClips == null)
            {
                return 0;
            }

            for (int i = 0; i < authoredCueClips.Length; i++)
            {
                SteamworksAudioClipBinding binding = authoredCueClips[i];
                if (binding != null && binding.clip != null)
                {
                    uniqueCues.Add(binding.cue);
                }
            }

            return uniqueCues.Count;
        }
    }

    public bool HasLastOneShotCue { get; private set; }
    public SteamworksAudioCue LastOneShotCue { get; private set; }
    public bool HasLastSpatialCue { get; private set; }
    public SteamworksAudioCue LastSpatialCue { get; private set; }

    private void Awake()
    {
        GameSettings.Load();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f;

        BuildClips();
        StartAmbience();
    }

    private void Update()
    {
        if (source != null && source.loop)
        {
            source.volume = Mathf.Clamp01(GameSettings.MasterVolume * ambienceVolume);
        }
    }

    public static void Play(SteamworksAudioCue cue)
    {
        Instance?.PlayCue(cue, null);
    }

    public static void PlayAt(SteamworksAudioCue cue, Vector3 position)
    {
        Instance?.PlayCue(cue, position);
    }

    private void PlayCue(SteamworksAudioCue cue, Vector3? position)
    {
        if (!clips.TryGetValue(cue, out AudioClip clip) || clip == null)
        {
            return;
        }

        masterVolume = GameSettings.MasterVolume;
        float volume = Mathf.Clamp01(masterVolume);
        if (position.HasValue)
        {
            HasLastSpatialCue = true;
            LastSpatialCue = cue;
            AudioSource.PlayClipAtPoint(clip, position.Value, volume);
        }
        else
        {
            HasLastOneShotCue = true;
            LastOneShotCue = cue;
            source.PlayOneShot(clip, volume);
        }
    }

    private void BuildClips()
    {
        clips[SteamworksAudioCue.PressureFire] = CreateClip("Pressure Fire", 0.16f, (t, _) => Tone(Slide(620f, 140f, t), t) * Envelope(t, 0.005f, 0.06f, 0.16f) + Noise(t) * 0.14f * Envelope(t, 0.001f, 0.04f, 0.16f));
        clips[SteamworksAudioCue.SteamScattergunFire] = CreateClip("Steam Scattergun Fire", 0.28f, ScattergunFireSample);
        clips[SteamworksAudioCue.BellowsNodePulse] = CreateClip("Bellows Node Pulse", 0.42f, BellowsNodePulseSample);
        clips[SteamworksAudioCue.WeaponPickup] = CreateClip("Weapon Pickup", 0.46f, WeaponPickupSample);
        clips[SteamworksAudioCue.SteamScattergunSlug] = CreateClip("Steam Scattergun Slug", 0.34f, ScattergunSlugSample);
        clips[SteamworksAudioCue.PressureBurst] = CreateClip("Pressure Burst", 0.24f, PressureBurstSample);
        clips[SteamworksAudioCue.EnemyAttackTell] = CreateClip("Enemy Attack Tell", 0.18f, EnemyAttackTellSample);
        clips[SteamworksAudioCue.LancerFireTell] = CreateClip("Lancer Fire Tell", 0.2f, LancerFireTellSample);
        clips[SteamworksAudioCue.BulwarkAttackTell] = CreateClip("Bulwark Attack Tell", 0.36f, BulwarkAttackTellSample);
        clips[SteamworksAudioCue.EmptyClick] = CreateClip("Empty Click", 0.09f, (t, _) => Noise(t) * 0.26f * Envelope(t, 0.001f, 0.025f, 0.09f));
        clips[SteamworksAudioCue.HealthPickup] = CreateClip("Health Pickup", 0.2f, (t, _) => Tone(Slide(520f, 780f, t), t) * Envelope(t, 0.005f, 0.08f, 0.2f));
        clips[SteamworksAudioCue.AmmoPickup] = CreateClip("Ammo Pickup", 0.18f, (t, _) => Tone(Slide(410f, 760f, t), t) * Envelope(t, 0.003f, 0.07f, 0.18f));
        clips[SteamworksAudioCue.GearKey] = CreateClip("Gear Key", 0.42f, GearKeySample);
        clips[SteamworksAudioCue.GateOpen] = CreateClip("Pressure Gate Open", 0.65f, GateOpenSample);
        clips[SteamworksAudioCue.GateDenied] = CreateClip("Pressure Gate Denied", 0.24f, (t, _) => Tone(110f, t) * 0.45f * Envelope(t, 0.002f, 0.12f, 0.24f) + Tone(55f, t) * 0.25f * Envelope(t, 0.002f, 0.16f, 0.24f));
        clips[SteamworksAudioCue.EnemyHit] = CreateClip("Enemy Hit", 0.18f, (t, _) => Noise(t) * 0.22f * Envelope(t, 0.001f, 0.07f, 0.18f) + Tone(330f, t) * 0.18f * Envelope(t, 0.002f, 0.09f, 0.18f));
        clips[SteamworksAudioCue.EnemyDeath] = CreateClip("Enemy Death", 0.55f, (t, _) => Tone(Slide(580f, 90f, t), t) * Envelope(t, 0.004f, 0.25f, 0.55f) + Noise(t) * 0.12f * Envelope(t, 0.02f, 0.35f, 0.55f));
        clips[SteamworksAudioCue.PlayerHurt] = CreateClip("Player Hurt", 0.24f, (t, _) => Tone(85f, t) * 0.5f * Envelope(t, 0.001f, 0.11f, 0.24f) + Noise(t) * 0.07f * Envelope(t, 0.002f, 0.06f, 0.24f));
        clips[SteamworksAudioCue.Win] = CreateClip("Win", 0.85f, WinSample);
        ApplyAuthoredCueClips();
    }

    private void StartAmbience()
    {
        if (!ambienceEnabled)
        {
            return;
        }

        source.clip = preferAuthoredClips && authoredAmbienceLoop != null
            ? authoredAmbienceLoop
            : CreateClip("Brassworks Ambience Loop", AmbienceDuration, AmbienceSample);
        source.loop = true;
        source.volume = Mathf.Clamp01(GameSettings.MasterVolume * ambienceVolume);
        source.Play();
    }

    public bool HasClip(SteamworksAudioCue cue)
    {
        return clips.TryGetValue(cue, out AudioClip clip) && clip != null;
    }

    public int GetClipSampleCount(SteamworksAudioCue cue)
    {
        return clips.TryGetValue(cue, out AudioClip clip) && clip != null ? clip.samples : 0;
    }

    public bool HasAuthoredClip(SteamworksAudioCue cue)
    {
        return TryGetAuthoredClip(cue, out _);
    }

    public bool IsUsingAuthoredClip(SteamworksAudioCue cue)
    {
        return authoredActiveCues.Contains(cue);
    }

    public string GetClipName(SteamworksAudioCue cue)
    {
        return clips.TryGetValue(cue, out AudioClip clip) && clip != null ? clip.name : string.Empty;
    }

    private void ApplyAuthoredCueClips()
    {
        authoredActiveCues.Clear();
        if (!preferAuthoredClips || authoredCueClips == null)
        {
            return;
        }

        for (int i = 0; i < authoredCueClips.Length; i++)
        {
            SteamworksAudioClipBinding binding = authoredCueClips[i];
            if (binding == null || binding.clip == null)
            {
                continue;
            }

            clips[binding.cue] = binding.clip;
            authoredActiveCues.Add(binding.cue);
        }
    }

    private bool TryGetAuthoredClip(SteamworksAudioCue cue, out AudioClip clip)
    {
        clip = null;
        if (authoredCueClips == null)
        {
            return false;
        }

        for (int i = 0; i < authoredCueClips.Length; i++)
        {
            SteamworksAudioClipBinding binding = authoredCueClips[i];
            if (binding != null && binding.cue == cue && binding.clip != null)
            {
                clip = binding.clip;
                return true;
            }
        }

        return false;
    }

    private static AudioClip CreateClip(string name, float duration, Func<float, int, float> generator)
    {
        int sampleCount = Mathf.Max(1, Mathf.CeilToInt(duration * SampleRate));
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)SampleRate;
            data[i] = Mathf.Clamp(generator(t, i), -1f, 1f);
        }

        AudioClip clip = AudioClip.Create(name, sampleCount, 1, SampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    private static float GearKeySample(float t, int sampleIndex)
    {
        float first = Tone(720f, t) * Envelope(t, 0.002f, 0.08f, 0.14f);
        float second = t > 0.11f ? Tone(960f, t - 0.11f) * Envelope(t - 0.11f, 0.002f, 0.08f, 0.14f) : 0f;
        float third = t > 0.22f ? Tone(1320f, t - 0.22f) * Envelope(t - 0.22f, 0.002f, 0.14f, 0.2f) : 0f;
        return (first + second + third) * 0.55f + Noise(sampleIndex * 0.0001f) * 0.025f * Envelope(t, 0.01f, 0.25f, 0.42f);
    }

    private static float GateOpenSample(float t, int sampleIndex)
    {
        float motor = Tone(52f + Mathf.Sin(t * 18f) * 9f, t) * 0.42f;
        float servo = Tone(Slide(180f, 95f, t), t) * 0.18f;
        float texture = Noise(sampleIndex * 0.0003f) * 0.06f;
        return (motor + servo + texture) * Envelope(t, 0.02f, 0.45f, 0.65f);
    }

    private static float ScattergunFireSample(float t, int sampleIndex)
    {
        float blast = Tone(Slide(210f, 62f, t / 0.28f), t) * 0.5f * Envelope(t, 0.001f, 0.18f, 0.28f);
        float brassClack = t < 0.055f ? Tone(880f, t) * Envelope(t, 0.001f, 0.035f, 0.055f) * 0.22f : 0f;
        float steam = Noise(sampleIndex * 0.00018f) * 0.32f * Envelope(t, 0.002f, 0.22f, 0.28f);
        float pipeResonance = Tone(118f + Mathf.Sin(t * 70f) * 12f, t) * 0.16f * Envelope(t, 0.004f, 0.2f, 0.28f);
        return Mathf.Clamp(blast + brassClack + steam + pipeResonance, -1f, 1f);
    }

    private static float BellowsNodePulseSample(float t, int sampleIndex)
    {
        float normalized = t / 0.42f;
        float bellows = Tone(Slide(168f, 58f, normalized), t) * 0.46f * Envelope(t, 0.003f, 0.3f, 0.42f);
        float valveThump = Tone(42f + Mathf.Sin(t * 32f) * 7f, t) * 0.36f * Envelope(t, 0.001f, 0.24f, 0.42f);
        float brassSnap = t < 0.075f ? Tone(760f, t) * 0.2f * Envelope(t, 0.001f, 0.045f, 0.075f) : 0f;
        float steam = Noise(sampleIndex * 0.00022f) * 0.3f * Envelope(t, 0.004f, 0.36f, 0.42f);
        return Mathf.Clamp(bellows + valveThump + brassSnap + steam, -1f, 1f);
    }

    private static float WeaponPickupSample(float t, int sampleIndex)
    {
        float normalized = t / 0.46f;
        float brassLatch = t < 0.09f ? Tone(920f, t) * 0.28f * Envelope(t, 0.001f, 0.055f, 0.09f) : 0f;
        float pressureRise = Tone(Slide(260f, 720f, normalized), t) * 0.22f * Envelope(t, 0.006f, 0.34f, 0.46f);
        float gearChimeA = t > 0.12f ? Tone(980f, t - 0.12f) * 0.18f * Envelope(t - 0.12f, 0.002f, 0.12f, 0.2f) : 0f;
        float gearChimeB = t > 0.24f ? Tone(1320f, t - 0.24f) * 0.14f * Envelope(t - 0.24f, 0.002f, 0.16f, 0.22f) : 0f;
        float steamBloom = Noise(sampleIndex * 0.00016f) * 0.18f * Envelope(t, 0.012f, 0.38f, 0.46f);
        return Mathf.Clamp(brassLatch + pressureRise + gearChimeA + gearChimeB + steamBloom, -1f, 1f);
    }

    private static float ScattergunSlugSample(float t, int sampleIndex)
    {
        float normalized = t / 0.34f;
        float pressureCrack = Tone(Slide(340f, 92f, normalized), t) * 0.48f * Envelope(t, 0.001f, 0.22f, 0.34f);
        float boltClang = t < 0.06f ? Tone(1180f, t) * 0.22f * Envelope(t, 0.001f, 0.032f, 0.06f) : 0f;
        float pipeWhistle = Tone(Slide(920f, 460f, normalized), t) * 0.12f * Envelope(t, 0.004f, 0.26f, 0.34f);
        float steamJet = Noise(sampleIndex * 0.0002f) * 0.2f * Envelope(t, 0.002f, 0.27f, 0.34f);
        return Mathf.Clamp(pressureCrack + boltClang + pipeWhistle + steamJet, -1f, 1f);
    }

    private static float PressureBurstSample(float t, int sampleIndex)
    {
        float normalized = t / 0.24f;
        float valveDump = Tone(Slide(760f, 180f, normalized), t) * 0.34f * Envelope(t, 0.001f, 0.15f, 0.24f);
        float brassSnap = t < 0.045f ? Tone(1280f, t) * 0.2f * Envelope(t, 0.001f, 0.026f, 0.045f) : 0f;
        float pressureWash = Noise(sampleIndex * 0.00024f) * 0.28f * Envelope(t, 0.001f, 0.19f, 0.24f);
        float pipeRing = Tone(310f + Mathf.Sin(t * 96f) * 22f, t) * 0.14f * Envelope(t, 0.002f, 0.17f, 0.24f);
        return Mathf.Clamp(valveDump + brassSnap + pressureWash + pipeRing, -1f, 1f);
    }

    private static float EnemyAttackTellSample(float t, int sampleIndex)
    {
        float normalized = t / 0.18f;
        float ratchet = t < 0.075f ? Tone(1040f, t) * 0.28f * Envelope(t, 0.001f, 0.045f, 0.075f) : 0f;
        float pressureRise = Tone(Slide(220f, 520f, normalized), t) * 0.24f * Envelope(t, 0.004f, 0.13f, 0.18f);
        float cutterScrape = Noise(sampleIndex * 0.00032f) * 0.18f * Envelope(t, 0.001f, 0.15f, 0.18f);
        return Mathf.Clamp(ratchet + pressureRise + cutterScrape, -1f, 1f);
    }

    private static float LancerFireTellSample(float t, int sampleIndex)
    {
        float normalized = t / 0.2f;
        float valveTick = t < 0.055f ? Tone(1280f, t) * 0.24f * Envelope(t, 0.001f, 0.035f, 0.055f) : 0f;
        float coilCharge = Tone(Slide(330f, 860f, normalized), t) * 0.26f * Envelope(t, 0.006f, 0.16f, 0.2f);
        float steamNeedle = Noise(sampleIndex * 0.00027f) * 0.16f * Envelope(t, 0.002f, 0.18f, 0.2f);
        return Mathf.Clamp(valveTick + coilCharge + steamNeedle, -1f, 1f);
    }

    private static float BulwarkAttackTellSample(float t, int sampleIndex)
    {
        float normalized = t / 0.36f;
        float hammerRatchet = t < 0.12f ? Tone(820f, t) * 0.22f * Envelope(t, 0.002f, 0.085f, 0.12f) : 0f;
        float boilerRise = Tone(Slide(96f, 310f, normalized), t) * 0.38f * Envelope(t, 0.006f, 0.28f, 0.36f);
        float chainDrag = Noise(sampleIndex * 0.00021f) * 0.24f * Envelope(t, 0.003f, 0.32f, 0.36f);
        float preImpactKnock = t > 0.24f ? Tone(54f, t - 0.24f) * 0.42f * Envelope(t - 0.24f, 0.001f, 0.08f, 0.12f) : 0f;
        return Mathf.Clamp(hammerRatchet + boilerRise + chainDrag + preImpactKnock, -1f, 1f);
    }

    private static float WinSample(float t, int sampleIndex)
    {
        float a = Tone(520f, t) * Envelope(t, 0.005f, 0.2f, 0.35f);
        float b = t > 0.18f ? Tone(780f, t - 0.18f) * Envelope(t - 0.18f, 0.005f, 0.2f, 0.35f) : 0f;
        float c = t > 0.36f ? Tone(1040f, t - 0.36f) * Envelope(t - 0.36f, 0.005f, 0.28f, 0.48f) : 0f;
        return (a + b + c) * 0.38f + Noise(sampleIndex * 0.0001f) * 0.015f * Envelope(t, 0.02f, 0.3f, 0.85f);
    }

    private static float AmbienceSample(float t, int sampleIndex)
    {
        float rumble = Tone(38f, t) * 0.12f + Tone(64f, t) * 0.07f;
        float flywheel = Tone(96f, t) * 0.035f * (0.65f + Mathf.Sin(Mathf.PI * 2f * 0.5f * t) * 0.35f);
        float piston = Mathf.Max(0f, Mathf.Sin(Mathf.PI * 2f * 1.5f * t)) * Tone(118f, t) * 0.035f;
        float steam = Noise(sampleIndex * 0.000035f) * 0.025f * (0.65f + Mathf.Sin(Mathf.PI * 2f * 0.25f * t) * 0.35f);
        return Mathf.Clamp((rumble + flywheel + piston + steam) * 0.5f, -1f, 1f);
    }

    private static float Envelope(float t, float attack, float releaseStart, float duration)
    {
        if (t < attack)
        {
            return Mathf.InverseLerp(0f, attack, t);
        }

        if (t > releaseStart)
        {
            return Mathf.InverseLerp(duration, releaseStart, t);
        }

        return 1f;
    }

    private static float Slide(float startFrequency, float endFrequency, float normalizedTime)
    {
        return Mathf.Lerp(startFrequency, endFrequency, Mathf.Clamp01(normalizedTime));
    }

    private static float Tone(float frequency, float t)
    {
        return Mathf.Sin(Mathf.PI * 2f * frequency * t);
    }

    private static float Noise(float seed)
    {
        return Mathf.PerlinNoise(seed * 97.13f, seed * 31.41f) * 2f - 1f;
    }
}
