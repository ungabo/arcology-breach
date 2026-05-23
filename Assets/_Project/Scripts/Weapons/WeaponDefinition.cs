using UnityEngine;

[CreateAssetMenu(menuName = "Brassworks/Weapon Definition")]
public class WeaponDefinition : ScriptableObject
{
    public string displayName = "Pressure Pistol";
    public int damage = GameBalance.PressurePistolDamage;
    public float fireCooldown = GameBalance.PressurePistolCooldown;
    public float range = 40f;
    public int secondaryDamage = GameBalance.PressureBurstDamage;
    public int secondaryPelletCount = GameBalance.PressureBurstPelletCount;
    public int secondaryAmmoCost = GameBalance.PressureBurstAmmoCost;
    public float secondaryCooldown = GameBalance.PressureBurstCooldown;
    public float secondaryRange = GameBalance.PressureBurstRange;
    public float secondarySpread = GameBalance.PressureBurstSpread;
}
