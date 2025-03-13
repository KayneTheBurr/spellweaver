using UnityEngine;

public class FireboltProjectile : Projectile
{
    public float burnDuration = 10f;
    public float burnTickInterval = 1f;
    public float burnTotalDamage = 100f;
    public GameObject explosionVFX;
    public override void OnHitEnemy(Enemy enemy)
    {
        enemy.TakeDamage(abilityData.baseDamage, abilityData.element, this.sourceAbility);

        if (PlayerManager.instance.playerCombatManager.DoesThisTriggerStatusEffect())
        {
            ApplyStatusEffect(enemy);
            Debug.Log("applied burn");
        }
        if (explosionVFX)
        {
            GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    public override void OnHitNonEnemy(Collider other)
    {
        Destroy(gameObject);
    }
    protected void ApplyStatusEffect(Enemy enemy)
    {
        DOT burnEffect = new DOT
        {
            element = ElementType.Fire,
            totalDamage = burnTotalDamage,
            duration = burnDuration,
            tickInterval = burnTickInterval
        };

        DamageOverTimeEffect burn = new DamageOverTimeEffect();
        burn.ApplyDOT(burnEffect, enemy);
    }
}
