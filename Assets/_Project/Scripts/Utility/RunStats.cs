using System.Collections.Generic;

public static class RunStats
{
    private static readonly HashSet<string> registeredSecrets = new HashSet<string>();
    private static readonly HashSet<string> discoveredSecrets = new HashSet<string>();

    public static int TotalSecrets => registeredSecrets.Count;
    public static int DiscoveredSecrets => discoveredSecrets.Count;

    public static void Reset()
    {
        registeredSecrets.Clear();
        discoveredSecrets.Clear();
    }

    public static void RegisterSecret(string secretId)
    {
        if (!string.IsNullOrWhiteSpace(secretId))
        {
            registeredSecrets.Add(secretId);
        }
    }

    public static bool MarkSecretDiscovered(string secretId)
    {
        if (string.IsNullOrWhiteSpace(secretId))
        {
            return false;
        }

        registeredSecrets.Add(secretId);
        return discoveredSecrets.Add(secretId);
    }
}
