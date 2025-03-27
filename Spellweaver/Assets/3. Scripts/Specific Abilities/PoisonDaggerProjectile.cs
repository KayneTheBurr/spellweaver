using UnityEngine;

public class PoisonDaggerProjectile : Projectile
{
    public float poisonTotalDamage = 300f;
    public float poisonDuration = 10f;
    public float poisonTickRate = 1f;
    public GameObject poisonVFX;

    public override void OnHitEnemy(Enemy enemy)
    {
        enemy.TakeDamage(abilityData.baseDamage, abilityData.element, this.sourceAbility);

        if (PlayerManager.instance.playerCombatManager.DoesThisTriggerStatusEffect())
        {
            ApplyStatusEffect(enemy);
            Debug.Log("applied poison");
        }
        if (poisonVFX)
        {
            GameObject dagger = Instantiate(poisonVFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    public override void OnHitNonEnemy(Collider other)
    {
        Destroy(gameObject);
    }
    protected void ApplyStatusEffect(Enemy enemy)
    {
        PoisonEffect poison = new PoisonEffect();
        poison.ApplyPoison(enemy, poisonTotalDamage, poisonDuration, poisonTickRate);
    }
}
