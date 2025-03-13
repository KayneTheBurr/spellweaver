using UnityEngine;

public class IceShardProjectile : Projectile
{
    public float chillDuration = 3f;
    public float slowEffect = 0.5f;
    public GameObject hitVFX;

    public override void OnHitEnemy(Enemy enemy)
    {
        enemy.TakeDamage(abilityData.baseDamage, abilityData.element, this.sourceAbility);

        if (PlayerManager.instance.playerCombatManager.DoesThisTriggerStatusEffect())
        {
            ApplyStatusEffect(enemy);
            Debug.Log("applied chill  1");
        }
        if (hitVFX != null)
        {
            GameObject shatter = Instantiate(hitVFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    public override void OnHitNonEnemy(Collider other)
    {
        Destroy(gameObject);
    }
    protected void ApplyStatusEffect(Enemy enemy)
    {
        ChillEffect chill = new ChillEffect();
        chill.ApplyChill(enemy, chillDuration, slowEffect);
        Debug.Log("applied chill  2");
    }
}
