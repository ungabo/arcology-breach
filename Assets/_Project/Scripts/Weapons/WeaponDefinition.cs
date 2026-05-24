using UnityEngine;

[CreateAssetMenu(menuName = "Brassworks/Weapon Definition")]
public class WeaponDefinition : ScriptableObject
{
    public string displayName = "Pressure Pistol";
    public string weaponId = "pressure_pistol";
    public int damage = GameBalance.PressurePistolDamage;
    public int ammoCost = GameBalance.PressurePistolAmmoCost;
    public int pelletCount = GameBalance.PressurePistolPelletCount;
    public float fireCooldown = GameBalance.PressurePistolCooldown;
    public float range = GameBalance.PressurePistolRange;
    public float spread = GameBalance.PressurePistolSpread;
    public int secondaryDamage = GameBalance.PressureBurstDamage;
    public int secondaryPelletCount = GameBalance.PressureBurstPelletCount;
    public int secondaryAmmoCost = GameBalance.PressureBurstAmmoCost;
    public float secondaryCooldown = GameBalance.PressureBurstCooldown;
    public float secondaryRange = GameBalance.PressureBurstRange;
    public float secondarySpread = GameBalance.PressureBurstSpread;
}
