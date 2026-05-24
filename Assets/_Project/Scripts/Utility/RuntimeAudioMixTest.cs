using System;
using System.Collections;
using UnityEngine;

public class RuntimeAudioMixTest : MonoBehaviour
{
    private const string AudioMixArgument = "-v0AudioMixSmoke";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(AudioMixArgument))
        {
            yield break;
        }

        yield return null;

        GameSettings.Load();
        SteamworksAudio audio = Require<SteamworksAudio>("SteamworksAudio");
        if (failed)
        {
            yield break;
        }

        VerifyAuthoredRouting(audio);
        VerifyMixProfile(audio);
        VerifyPlaybackRouting(audio);

        if (failed)
        {
            yield break;
        }

        Debug.Log("V0_AUDIO_MIX_PASS");
        Application.Quit(0);
    }

    private void VerifyAuthoredRouting(SteamworksAudio audio)
    {
        RequireState(audio.AmbienceActive, "ambience source is active");
        RequireState(audio.UsingAuthoredAmbience, "authored AudioV1 ambience is active");
        RequireState(audio.AmbienceSampleCount > 0, "ambience clip has samples");

        foreach (SteamworksAudioCue cue in Enum.GetValues(typeof(SteamworksAudioCue)))
        {
            RequireState(audio.HasClip(cue), cue + " has an active clip");
            RequireState(audio.IsUsingAuthoredClip(cue), cue + " is using an authored AudioV1 clip");
            RequireState(audio.GetClipSampleCount(cue) > 0, cue + " clip has samples");
        }
    }

    private void VerifyMixProfile(SteamworksAudio audio)
    {
        int expectedCueCount = Enum.GetValues(typeof(SteamworksAudioCue)).Length;
        RequireState(audio.MixBindingCount >= expectedCueCount, "every cue has a serialized mix binding");
        RequireRange(audio.ambienceVolume, 0.18f, 0.32f, "ambience mix volume");

        foreach (SteamworksAudioCue cue in Enum.GetValues(typeof(SteamworksAudioCue)))
        {
            RequireState(audio.HasMixBinding(cue), cue + " has a mix binding");
            RequireRange(audio.GetCueVolumeMultiplier(cue), 0.45f, 1f, cue + " volume multiplier");
        }

        RequireState(audio.GetCueVolumeMultiplier(SteamworksAudioCue.PressureBurst) > audio.GetCueVolumeMultiplier(SteamworksAudioCue.PressureFire), "Pressure Burst is mixed louder than primary fire");
        RequireState(audio.GetCueVolumeMultiplier(SteamworksAudioCue.SteamScattergunFire) > audio.GetCueVolumeMultiplier(SteamworksAudioCue.PressureFire), "Steam Scattergun is mixed louder than primary fire");
        RequireState(audio.GetCueVolumeMultiplier(SteamworksAudioCue.EmptyClick) < audio.GetCueVolumeMultiplier(SteamworksAudioCue.PressureFire), "empty click is below primary fire");
        RequireState(audio.GetCueVolumeMultiplier(SteamworksAudioCue.BulwarkAttackTell) > audio.GetCueVolumeMultiplier(SteamworksAudioCue.EnemyAttackTell), "Bulwark tell is above Scrapper tell");

        RequireSpatial(audio, SteamworksAudioCue.EnemyAttackTell);
        RequireSpatial(audio, SteamworksAudioCue.LancerFireTell);
        RequireSpatial(audio, SteamworksAudioCue.BulwarkAttackTell);
        RequireSpatial(audio, SteamworksAudioCue.BellowsNodePulse);
        RequireSpatial(audio, SteamworksAudioCue.GateOpen);
        RequireSpatial(audio, SteamworksAudioCue.EnemyDeath);

        RequireNonSpatial(audio, SteamworksAudioCue.EmptyClick);
        RequireNonSpatial(audio, SteamworksAudioCue.HealthPickup);
        RequireNonSpatial(audio, SteamworksAudioCue.AmmoPickup);
        RequireNonSpatial(audio, SteamworksAudioCue.PlayerHurt);
        RequireNonSpatial(audio, SteamworksAudioCue.WeaponPickup);
    }

    private void VerifyPlaybackRouting(SteamworksAudio audio)
    {
        SteamworksAudio.Play(SteamworksAudioCue.PressureFire);
        RequireState(audio.HasLastOneShotCue && audio.LastOneShotCue == SteamworksAudioCue.PressureFire, "primary fire routes through one-shot audio");
        RequireRange(audio.GetEffectiveCueVolume(SteamworksAudioCue.PressureFire), 0.01f, 1f, "primary fire effective volume");

        SteamworksAudio.PlayAt(SteamworksAudioCue.LancerFireTell, transform.position + Vector3.forward);
        RequireState(audio.HasLastSpatialCue && audio.LastSpatialCue == SteamworksAudioCue.LancerFireTell, "Lancer fire tell routes through spatial audio");
        RequireRange(audio.GetEffectiveCueVolume(SteamworksAudioCue.LancerFireTell), 0.01f, 1f, "Lancer fire tell effective volume");
    }

    private void RequireSpatial(SteamworksAudio audio, SteamworksAudioCue cue)
    {
        RequireState(audio.IsSpatialMixCue(cue), cue + " is marked as a spatial mix cue");
    }

    private void RequireNonSpatial(SteamworksAudio audio, SteamworksAudioCue cue)
    {
        RequireState(!audio.IsSpatialMixCue(cue), cue + " remains a non-spatial mix cue");
    }

    private void RequireRange(float value, float minimum, float maximum, string label)
    {
        if (value < minimum || value > maximum)
        {
            Fail("Audio mix smoke failed: " + label + " expected " + minimum.ToString("0.00") + "-" + maximum.ToString("0.00") + " but found " + value.ToString("0.00") + ".");
        }
    }

    private void RequireState(bool condition, string label)
    {
        if (!condition)
        {
            Fail("Audio mix smoke failed: " + label + ".");
        }
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Audio mix smoke failed: missing " + label + ".");
        }

        return value;
    }

    private void Fail(string message)
    {
        failed = true;
        Debug.LogError(message);
        Application.Quit(1);
    }

    private static bool HasArgument(string argument)
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], argument, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
