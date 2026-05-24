using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeBellowsNodeTest : MonoBehaviour
{
    private const string BellowsNodeArgument = "-v0BellowsNodeSmoke";
    private const string BellowsNodeSceneName = "Level03";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(BellowsNodeArgument))
        {
            yield break;
        }

        yield return null;

        if (SceneManager.GetActiveScene().name != BellowsNodeSceneName)
        {
            SceneManager.LoadScene(BellowsNodeSceneName);
            yield break;
        }

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerHealth health = Require<PlayerHealth>("PlayerHealth");
        WeaponController weapon = Require<WeaponController>("WeaponController");
        SteamworksAudio audio = Require<SteamworksAudio>("SteamworksAudio");
        BellowsNodeController target = Require<BellowsNodeController>("BellowsNodeController");
        EnemyController boostTarget = Require<EnemyController>("EnemyController");

        DisableOtherEnemies(target, boostTarget);
        PlaceCombatActors(player, target, closeRange: 2.6f);
        PlaceBoostTarget(boostTarget, new Vector3(1.25f, 1f, 2.6f));

        int healthBeforePulse = health.CurrentHealth;
        target.ForcePulseForTest();
        yield return WaitUntilOrFail(() => health.CurrentHealth < healthBeforePulse, "Bellows Node pulse damage", 1f);
        if (failed)
        {
            yield break;
        }

        if (!audio.HasClip(SteamworksAudioCue.BellowsNodePulse) || !audio.HasLastSpatialCue || audio.LastSpatialCue != SteamworksAudioCue.BellowsNodePulse)
        {
            Fail("Bellows Node smoke failed: dedicated pulse audio cue did not route.");
            yield break;
        }

        BellowsNodePulseVfx pulseVfx = UnityEngine.Object.FindAnyObjectByType<BellowsNodePulseVfx>();
        if (pulseVfx == null || pulseVfx.PieceCount < 10)
        {
            Fail("Bellows Node smoke failed: pulse VFX did not spawn with enough visible pieces.");
            yield break;
        }

        if (!boostTarget.IsPressureBoosted || boostTarget.CurrentMoveSpeed <= boostTarget.moveSpeed)
        {
            Fail("Bellows Node smoke failed: pressure pulse did not boost a nearby Scrapper.");
            yield break;
        }

        PressureBoostVfx boostVfx = boostTarget.GetComponent<PressureBoostVfx>();
        if (boostVfx == null || !boostVfx.IsActive || boostVfx.VisiblePieceCount < 8)
        {
            Fail("Bellows Node smoke failed: pressure boost VFX did not appear on the nearby Scrapper.");
            yield break;
        }

        boostTarget.gameObject.SetActive(false);
        yield return null;

        target.enabled = false;
        PlaceCombatActors(player, target, closeRange: 3.2f);
        yield return new WaitForSeconds(0.6f);

        int expectedShotsToKill = Mathf.CeilToInt(target.maxHealth / (float)weapon.damage);
        if (expectedShotsToKill < 3)
        {
            Fail("Bellows Node smoke failed: Bellows Node should require at least three pressure-pistol shots.");
            yield break;
        }

        for (int shotIndex = 1; shotIndex <= expectedShotsToKill; shotIndex++)
        {
            target.TakeDamage(weapon.damage);
            yield return new WaitForSeconds(weapon.fireCooldown + 0.05f);
        }

        yield return WaitUntilOrFail(() => target == null, "Bellows Node destruction", 2f);
        if (failed)
        {
            yield break;
        }

        MachineDeathVfx deathVfx = UnityEngine.Object.FindAnyObjectByType<MachineDeathVfx>();
        if (deathVfx == null || deathVfx.PieceCount < 8)
        {
            Fail("Bellows Node smoke failed: destruction VFX did not spawn with enough visible pieces.");
            yield break;
        }

        Debug.Log("V0_BELLOWS_NODE_PASS");
        Application.Quit(0);
    }

    private static void DisableOtherEnemies(BellowsNodeController target, EnemyController boostTarget)
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != boostTarget)
            {
                enemy.gameObject.SetActive(false);
            }
        }

        RangedEnemyController[] rangedEnemies = UnityEngine.Object.FindObjectsByType<RangedEnemyController>();
        foreach (RangedEnemyController enemy in rangedEnemies)
        {
            enemy.gameObject.SetActive(false);
        }

        BulwarkEnemyController[] bulwarks = UnityEngine.Object.FindObjectsByType<BulwarkEnemyController>();
        foreach (BulwarkEnemyController enemy in bulwarks)
        {
            enemy.gameObject.SetActive(false);
        }

        BellowsNodeController[] nodes = UnityEngine.Object.FindObjectsByType<BellowsNodeController>();
        foreach (BellowsNodeController node in nodes)
        {
            if (node != target)
            {
                node.gameObject.SetActive(false);
            }
        }
    }

    private static void PlaceBoostTarget(EnemyController boostTarget, Vector3 position)
    {
        CharacterController controller = boostTarget.GetComponent<CharacterController>();
        SetControllerEnabled(controller, false);

        boostTarget.transform.position = position;
        boostTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        SetControllerEnabled(controller, true);
    }

    private static void PlaceCombatActors(PlayerController player, BellowsNodeController target, float closeRange)
    {
        CharacterController playerController = player.GetComponent<CharacterController>();
        CharacterController targetController = target.GetComponent<CharacterController>();

        SetControllerEnabled(playerController, false);
        SetControllerEnabled(targetController, false);

        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        if (player.playerCamera != null)
        {
            player.playerCamera.localRotation = Quaternion.identity;
        }

        target.transform.position = new Vector3(0f, 0.95f, closeRange);
        target.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        SetControllerEnabled(targetController, true);
        SetControllerEnabled(playerController, true);
    }

    private IEnumerator WaitUntilOrFail(Func<bool> predicate, string step, float timeoutSeconds)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < timeoutSeconds)
        {
            if (predicate())
            {
                yield break;
            }

            yield return null;
        }

        Fail("Bellows Node smoke failed while waiting for " + step + ".");
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Bellows Node smoke failed: missing " + label + ".");
        }

        return value;
    }

    private void Fail(string message)
    {
        failed = true;
        Debug.LogError(message);
        Application.Quit(1);
    }

    private static void SetControllerEnabled(CharacterController controller, bool enabled)
    {
        if (controller != null)
        {
            controller.enabled = enabled;
        }
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
