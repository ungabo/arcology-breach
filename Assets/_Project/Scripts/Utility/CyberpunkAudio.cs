using System;
using System.Collections.Generic;
using UnityEngine;

public enum CyberpunkAudioCue
{
    PulseFire,
    EmptyClick,
    HealthPickup,
    AmmoPickup,
    AccessShard,
    DoorOpen,
    DoorDenied,
    EnemyHit,
    EnemyDeath,
    PlayerHurt,
    Win
}

[RequireComponent(typeof(AudioSource))]
public class CyberpunkAudio : MonoBehaviour
{
    public static CyberpunkAudio Instance { get; private set; }

    public float masterVolume = 0.55f;

    private const int SampleRate = 44100;

    private readonly Dictionary<CyberpunkAudioCue, AudioClip> clips = new Dictionary<CyberpunkAudioCue, AudioClip>();
    private AudioSource source;

    private void Awake()
    {
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
    }

    public static void Play(CyberpunkAudioCue cue)
    {
        Instance?.PlayCue(cue, null);
    }

    public static void PlayAt(CyberpunkAudioCue cue, Vector3 position)
    {
        Instance?.PlayCue(cue, position);
    }

    private void PlayCue(CyberpunkAudioCue cue, Vector3? position)
    {
        if (!clips.TryGetValue(cue, out AudioClip clip) || clip == null)
        {
            return;
        }

        float volume = Mathf.Clamp01(masterVolume);
        if (position.HasValue)
        {
            AudioSource.PlayClipAtPoint(clip, position.Value, volume);
        }
        else
        {
            source.PlayOneShot(clip, volume);
        }
    }

    private void BuildClips()
    {
        clips[CyberpunkAudioCue.PulseFire] = CreateClip("Pulse Fire", 0.16f, (t, _) => Tone(Slide(930f, 180f, t), t) * Envelope(t, 0.005f, 0.06f, 0.16f) + Noise(t) * 0.09f * Envelope(t, 0.001f, 0.04f, 0.16f));
        clips[CyberpunkAudioCue.EmptyClick] = CreateClip("Empty Click", 0.09f, (t, _) => Noise(t) * 0.26f * Envelope(t, 0.001f, 0.025f, 0.09f));
        clips[CyberpunkAudioCue.HealthPickup] = CreateClip("Health Pickup", 0.2f, (t, _) => Tone(Slide(520f, 780f, t), t) * Envelope(t, 0.005f, 0.08f, 0.2f));
        clips[CyberpunkAudioCue.AmmoPickup] = CreateClip("Ammo Pickup", 0.18f, (t, _) => Tone(Slide(680f, 1180f, t), t) * Envelope(t, 0.003f, 0.07f, 0.18f));
        clips[CyberpunkAudioCue.AccessShard] = CreateClip("Access Shard", 0.42f, AccessShardSample);
        clips[CyberpunkAudioCue.DoorOpen] = CreateClip("Door Open", 0.65f, DoorOpenSample);
        clips[CyberpunkAudioCue.DoorDenied] = CreateClip("Door Denied", 0.24f, (t, _) => Tone(110f, t) * 0.45f * Envelope(t, 0.002f, 0.12f, 0.24f) + Tone(55f, t) * 0.25f * Envelope(t, 0.002f, 0.16f, 0.24f));
        clips[CyberpunkAudioCue.EnemyHit] = CreateClip("Enemy Hit", 0.18f, (t, _) => Noise(t) * 0.22f * Envelope(t, 0.001f, 0.07f, 0.18f) + Tone(330f, t) * 0.18f * Envelope(t, 0.002f, 0.09f, 0.18f));
        clips[CyberpunkAudioCue.EnemyDeath] = CreateClip("Enemy Death", 0.55f, (t, _) => Tone(Slide(580f, 90f, t), t) * Envelope(t, 0.004f, 0.25f, 0.55f) + Noise(t) * 0.12f * Envelope(t, 0.02f, 0.35f, 0.55f));
        clips[CyberpunkAudioCue.PlayerHurt] = CreateClip("Player Hurt", 0.24f, (t, _) => Tone(85f, t) * 0.5f * Envelope(t, 0.001f, 0.11f, 0.24f) + Noise(t) * 0.07f * Envelope(t, 0.002f, 0.06f, 0.24f));
        clips[CyberpunkAudioCue.Win] = CreateClip("Win", 0.85f, WinSample);
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

    private static float AccessShardSample(float t, int sampleIndex)
    {
        float first = Tone(720f, t) * Envelope(t, 0.002f, 0.08f, 0.14f);
        float second = t > 0.11f ? Tone(960f, t - 0.11f) * Envelope(t - 0.11f, 0.002f, 0.08f, 0.14f) : 0f;
        float third = t > 0.22f ? Tone(1320f, t - 0.22f) * Envelope(t - 0.22f, 0.002f, 0.14f, 0.2f) : 0f;
        return (first + second + third) * 0.55f + Noise(sampleIndex * 0.0001f) * 0.025f * Envelope(t, 0.01f, 0.25f, 0.42f);
    }

    private static float DoorOpenSample(float t, int sampleIndex)
    {
        float motor = Tone(52f + Mathf.Sin(t * 18f) * 9f, t) * 0.42f;
        float servo = Tone(Slide(180f, 95f, t), t) * 0.18f;
        float texture = Noise(sampleIndex * 0.0003f) * 0.06f;
        return (motor + servo + texture) * Envelope(t, 0.02f, 0.45f, 0.65f);
    }

    private static float WinSample(float t, int sampleIndex)
    {
        float a = Tone(520f, t) * Envelope(t, 0.005f, 0.2f, 0.35f);
        float b = t > 0.18f ? Tone(780f, t - 0.18f) * Envelope(t - 0.18f, 0.005f, 0.2f, 0.35f) : 0f;
        float c = t > 0.36f ? Tone(1040f, t - 0.36f) * Envelope(t - 0.36f, 0.005f, 0.28f, 0.48f) : 0f;
        return (a + b + c) * 0.38f + Noise(sampleIndex * 0.0001f) * 0.015f * Envelope(t, 0.02f, 0.3f, 0.85f);
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
