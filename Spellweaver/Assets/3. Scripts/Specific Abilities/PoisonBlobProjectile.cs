using UnityEngine;

public class PoisonBlobProjectile : Projectile
{
    [Header("Fireball Properties")]
    public float explosionRadius = 20f;
    public float poisonDuration = 10f;
    public float poisonTickInterval = 1f;
    public float poisonTotalDamage = 1200f;
    public GameObject explosionVFX;


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
                enemy.TakeDamage(abilityData.baseDamage, abilityData.element, this.sourceAbility);

                PoisonEffect poison = new PoisonEffect();
                poison.ApplyPoison(enemy, poisonTotalDamage, poisonDuration, poisonTickInterval);
            }
        }

        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}


    

