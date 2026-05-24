using UnityEngine;

public enum EnemyAttackStyle
{
    Melee,
    Ranged,
    Heavy,
    Boss,
    Support
}

[CreateAssetMenu(menuName = "Brassworks/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    public string displayName = "Clockwork Enemy";
    public EnemyAttackStyle attackStyle = EnemyAttackStyle.Melee;
    public int maxHealth = GameBalance.ScrapperHealth;
    public float detectionRange = GameBalance.ScrapperDetectionRange;
    public float moveSpeed = GameBalance.ScrapperMoveSpeed;

    public float attackRange = 1.35f;
    public int attackDamage = GameBalance.ScrapperAttackDamage;
    public float attackCooldown = 1f;
    public float attackWindup = GameBalance.ScrapperAttackWindup;
    public float obstacleProbeDistance = GameBalance.ScrapperObstacleProbeDistance;

    public float fireRange = GameBalance.LancerFireRange;
    public float fireCooldown = GameBalance.LancerFireCooldown;
    public float fireWindup = GameBalance.LancerFireWindup;
    public int projectileDamage = GameBalance.LancerProjectileDamage;
    public float projectileSpeed = GameBalance.LancerProjectileSpeed;
}
