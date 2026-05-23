using UnityEngine;

public class SecretArea : MonoBehaviour
{
    public static int DiscoveredCount { get; private set; }

    public string secretId = "secret";
    public string discoveryMessage = "SECRET CACHE FOUND";

    public bool Discovered { get; private set; }

    private void Awake()
    {
        Collider triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Discover(other.gameObject);
    }

    public bool Discover(GameObject interactor)
    {
        if (Discovered || interactor.GetComponentInParent<PlayerController>() == null)
        {
            return false;
        }

        Discovered = true;
        DiscoveredCount++;
        HUDController.Instance?.ShowTemporaryMessage(discoveryMessage, 1.8f);
        SteamworksAudio.Play(SteamworksAudioCue.GearKey);
        return true;
    }
}
