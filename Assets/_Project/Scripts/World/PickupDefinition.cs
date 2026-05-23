using UnityEngine;

[CreateAssetMenu(menuName = "Brassworks/Pickup Definition")]
public class PickupDefinition : ScriptableObject
{
    public string displayName = "Pickup";
    public PickupKind kind = PickupKind.Ammo;
    public int amount = 1;
    public float collectRadius = 0.9f;
    public float spinDegreesPerSecond = 90f;
    public float bobAmplitude = 0.12f;
    public float bobSpeed = 3f;
    public SteamworksAudioCue audioCue = SteamworksAudioCue.AmmoPickup;
    public string collectMessage = "+1 ammo";
}
