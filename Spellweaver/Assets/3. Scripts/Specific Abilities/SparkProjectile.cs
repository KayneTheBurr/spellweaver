using UnityEngine;

public class SparkProjectile : Projectile
{
    public float shockDuration = 4f;
    public GameObject sparkVFX;
    private void FixedUpdate()
    {
        
    }
    public override void OnHitEnemy(Enemy enemy)
    {
        enemy.TakeDamage(abilityData.baseDamage, abilityData.element, this.sourceAbility);

        if (PlayerManager.instance.playerCombatManager.DoesThisTriggerStatusEffect())
        {
            ApplyStatusEffect(enemy);
            Debug.Log("applied shock");
        }
        if (sparkVFX)
        {
            GameObject sparks = Instantiate(sparkVFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    public override void OnHitNonEnemy(Collider other)
    {
        Destroy(gameObject);
    }
    protected void ApplyStatusEffect(Enemy enemy)
    {
        ShockedEffect shock = new ShockedEffect();
        shock.ApplyShock(enemy, shockDuration);
    }
}

