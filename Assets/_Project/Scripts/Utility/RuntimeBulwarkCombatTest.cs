using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeBulwarkCombatTest : MonoBehaviour
{
    private const string BulwarkCombatArgument = "-v0BulwarkCombatSmoke";
    private const string BulwarkSceneName = "Level04";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(BulwarkCombatArgument))
        {
            yield break;
        }

        yield return null;

        if (SceneManager.GetActiveScene().name != BulwarkSceneName)
        {
            SceneManager.LoadScene(BulwarkSceneName);
            yield break;
        }

        PlayerController player = Require<PlayerController>("PlayerController");
        PlayerHealth health = Require<PlayerHealth>("PlayerHealth");
        WeaponController weapon = Require<WeaponController>("WeaponController");
        BulwarkEnemyController target = Require<BulwarkEnemyController>("BulwarkEnemyController");

        DisableOtherEnemies(target);
        PlaceAttackActors(player, target);

        int healthBeforeTell = health.CurrentHealth;
        yield return WaitUntilOrFail(() => HasActiveBulwarkAttackTell(target), "Bulwark hammer windup tell", 1.5f);
        if (failed)
        {
            yield break;
        }

        if (health.CurrentHealth != healthBeforeTell)
        {
            Fail("Bulwark combat smoke failed: Bulwark damage landed before the hammer windup tell could be observed.");
            yield break;
        }

        SteamworksAudio audio = UnityEngine.Object.FindAnyObjectByType<SteamworksAudio>();
        if (audio == null || !audio.HasClip(SteamworksAudioCue.BulwarkAttackTell) || !audio.HasLastSpatialCue || audio.LastSpatialCue != SteamworksAudioCue.BulwarkAttackTell)
        {
            Fail("Bulwark combat smoke failed: Bulwark hammer windup audio did not play before slam damage.");
            yield break;
        }

        yield return WaitUntilOrFail(() => health.CurrentHealth < healthBeforeTell, "Bulwark slam damage after tell", 2f);
        if (failed)
        {
            yield break;
        }

        PlaceCombatActors(player, target);
        target.enabled = false;

        int expectedShotsToKill = Mathf.CeilToInt(target.maxHealth / (float)weapon.damage);
        if (expectedShotsToKill < 4)
        {
            Fail("Bulwark combat smoke failed: Bulwark should require at least four pressure-pistol shots.");
            yield break;
        }

        bool sawHitVfx = false;
        for (int shotIndex = 1; shotIndex <= expectedShotsToKill; shotIndex++)
        {
            if (!weapon.FireOnce())
            {
                Fail("Bulwark combat smoke failed: weapon did not fire on shot " + shotIndex + ".");
                yield break;
            }

            yield return new WaitForSeconds(weapon.fireCooldown + 0.05f);

            if (shotIndex == 1)
            {
                MachineHitVfx hitVfx = UnityEngine.Object.FindAnyObjectByType<MachineHitVfx>();
                sawHitVfx = hitVfx != null && hitVfx.PieceCount >= 6;
            }
        }

        if (!sawHitVfx)
        {
            Fail("Bulwark combat smoke failed: non-lethal machine hit VFX did not spawn with enough visible pieces.");
            yield break;
        }

        yield return WaitUntilOrFail(() => target == null, "Bulwark death from pressure pistol fire", 2f);
        if (failed)
        {
            yield break;
        }

        MachineDeathVfx deathVfx = UnityEngine.Object.FindAnyObjectByType<MachineDeathVfx>();
        if (deathVfx == null || !deathVfx.HasBulwarkShutdownDetail || deathVfx.PieceCount < 22)
        {
            Fail("Bulwark combat smoke failed: Bulwark shutdown VFX did not spawn with enough visible detail.");
            yield break;
        }

        Debug.Log("V0_BULWARK_COMBAT_PASS");
        Application.Quit(0);
    }

    private static bool HasActiveBulwarkAttackTell(BulwarkEnemyController target)
    {
        if (target == null)
        {
            return false;
        }

        BulwarkAttackTellVfx attackTell = target.GetComponent<BulwarkAttackTellVfx>();
        return attackTell != null && attackTell.IsActive && attackTell.PieceCount >= 10;
    }

    private static void DisableOtherEnemies(BulwarkEnemyController target)
    {
        EnemyController[] enemies = UnityEngine.Object.FindObjectsByType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }

        RangedEnemyController[] rangedEnemies = UnityEngine.Object.FindObjectsByType<RangedEnemyController>();
        foreach (RangedEnemyController enemy in rangedEnemies)
        {
            enemy.gameObject.SetActive(false);
        }

        BulwarkEnemyController[] bulwarks = UnityEngine.Object.FindObjectsByType<BulwarkEnemyController>();
        foreach (BulwarkEnemyController bulwark in bulwarks)
        {
            if (bulwark != target)
            {
                bulwark.gameObject.SetActive(false);
            }
        }
    }

    private static void PlaceCombatActors(PlayerController player, BulwarkEnemyController target)
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

        target.transform.position = new Vector3(0f, 1.15f, 4.2f);
        target.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        SetControllerEnabled(targetController, true);
        SetControllerEnabled(playerController, true);
    }

    private static void PlaceAttackActors(PlayerController player, BulwarkEnemyController target)
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

        target.transform.position = new Vector3(0f, 1.15f, 1.35f);
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

        Fail("Bulwark combat smoke failed while waiting for " + step + ".");
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Bulwark combat smoke failed: missing " + label + ".");
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
