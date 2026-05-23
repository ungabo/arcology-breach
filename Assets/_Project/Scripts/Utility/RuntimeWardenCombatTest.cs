using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeWardenCombatTest : MonoBehaviour
{
    private const string WardenCombatArgument = "-v0WardenCombatSmoke";
    private const string WardenSceneName = "Level05";

    private bool failed;

    private IEnumerator Start()
    {
        if (!HasArgument(WardenCombatArgument))
        {
            yield break;
        }

        yield return null;

        if (SceneManager.GetActiveScene().name != WardenSceneName)
        {
            SceneManager.LoadScene(WardenSceneName);
            yield break;
        }

        PlayerController player = Require<PlayerController>("PlayerController");
        WeaponController weapon = Require<WeaponController>("WeaponController");
        HUDController hud = Require<HUDController>("HUDController");
        GovernorWardenController target = Require<GovernorWardenController>("GovernorWardenController");

        DisableOtherEnemies(target);
        PlaceCombatActors(player, target);
        target.enabled = false;

        int expectedShotsToKill = Mathf.CeilToInt(target.maxHealth / (float)weapon.damage);
        if (expectedShotsToKill < 8)
        {
            Fail("Warden combat smoke failed: Governor Warden should require at least eight pressure-pistol shots.");
            yield break;
        }

        bool sawDamagedBossHud = false;
        bool sawHitVfx = false;
        for (int shotIndex = 1; shotIndex <= expectedShotsToKill; shotIndex++)
        {
            if (!weapon.FireOnce())
            {
                Fail("Warden combat smoke failed: weapon did not fire on shot " + shotIndex + ".");
                yield break;
            }

            yield return new WaitForSeconds(weapon.fireCooldown + 0.05f);

            if (shotIndex == 1)
            {
                sawDamagedBossHud = hud.bossNameText != null && hud.bossNameText.enabled && hud.bossFillImage != null && hud.bossFillImage.enabled && hud.bossFillImage.fillAmount < 0.999f;
                MachineHitVfx hitVfx = UnityEngine.Object.FindAnyObjectByType<MachineHitVfx>();
                sawHitVfx = hitVfx != null && hitVfx.PieceCount >= 6;
            }
        }

        if (!sawDamagedBossHud)
        {
            Fail("Warden combat smoke failed: boss health HUD did not show damage.");
            yield break;
        }

        if (!sawHitVfx)
        {
            Fail("Warden combat smoke failed: non-lethal machine hit VFX did not spawn with enough visible pieces.");
            yield break;
        }

        yield return WaitUntilOrFail(() => target == null, "Governor Warden death from pressure pistol fire", 2f);
        if (failed)
        {
            yield break;
        }

        WardenShutdownVfx shutdownVfx = UnityEngine.Object.FindAnyObjectByType<WardenShutdownVfx>();
        if (shutdownVfx == null || shutdownVfx.PieceCount < 12)
        {
            Fail("Warden combat smoke failed: shutdown VFX did not spawn with enough visible pieces.");
            yield break;
        }

        Debug.Log("V0_WARDEN_COMBAT_PASS");
        Application.Quit(0);
    }

    private static void DisableOtherEnemies(GovernorWardenController target)
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
            bulwark.gameObject.SetActive(false);
        }

        GovernorWardenController[] wardens = UnityEngine.Object.FindObjectsByType<GovernorWardenController>();
        foreach (GovernorWardenController warden in wardens)
        {
            if (warden != target)
            {
                warden.gameObject.SetActive(false);
            }
        }
    }

    private static void PlaceCombatActors(PlayerController player, GovernorWardenController target)
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

        target.transform.position = new Vector3(0f, 1.45f, 5.4f);
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

        Fail("Warden combat smoke failed while waiting for " + step + ".");
    }

    private T Require<T>(string label) where T : UnityEngine.Object
    {
        T value = UnityEngine.Object.FindAnyObjectByType<T>();
        if (value == null)
        {
            Fail("Warden combat smoke failed: missing " + label + ".");
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
