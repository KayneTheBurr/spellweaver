using System.Collections;
using UnityEngine;

public class FireballProjectile : Projectile
{
    public float aoeDamageReduction = 0.75f;

    public float explosionRadius;
    public float burnDuration;
    public float burnTickInterval;
    public float burnTotalDamage;
    public GameObject explosionVFX;

    public void SetProperties(float radius, float duration, float tickInterval, float totalDamage)
    {
        explosionRadius = radius;
        burnDuration = duration;
        burnTickInterval = tickInterval;
        burnTotalDamage = totalDamage;
        explosionVFX = GetComponent<Fireball>().explosionVFX;
    }
    public override void OnHitEnemy(Enemy enemy)
    {
        base.OnHitEnemy(enemy);
        enemy.TakeDamage(abilityData.baseDamage, abilityData.element, this.sourceAbility);
        Explode();
    }

    public override void OnHitNonEnemy(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        if (explosionVFX)
        {
            GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(abilityData.baseDamage * aoeDamageReduction, abilityData.element, this.sourceAbility);

                DOT burnEffect = new DOT
                {
                    element = ElementType.Fire,
                    totalDamage = burnTotalDamage,
                    duration = burnDuration,
                    tickInterval = burnTickInterval
                };

                BurnEffect burn = new BurnEffect();
                burn.ApplyDOT(burnEffect, enemy);
            }
        }

        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    
}
