using UnityEngine;

[CreateAssetMenu(menuName = "Brassworks/Weapon Definition")]
public class WeaponDefinition : ScriptableObject
{
    public string displayName = "Pressure Pistol";
    public int damage = GameBalance.PressurePistolDamage;
    public float fireCooldown = GameBalance.PressurePistolCooldown;
    public float range = 40f;
}
