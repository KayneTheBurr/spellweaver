using UnityEngine;

public class IceSpearProjectile : Projectile
{
    [Header("Projectile Specific things")]
    public float outHitMult = 0.5f;
    public float pierceDamageMult = 2f;
    private int pierceCount = 0;

    [Header("IceSpear Properties")]
    public int maxPierces = 2;
    public float chillDuration = 5f;
    public float slowEffect = 0.75f;
    public GameObject hitVFX;
    public GameObject travelVFX;

    
    public override void OnHitEnemy(Enemy enemy)
    {
        base.OnHitEnemy(enemy);
        
        float finalDamage = abilityData.baseDamage * Mathf.Pow(pierceDamageMult, pierceCount);
        enemy.TakeDamage(finalDamage, abilityData.element, this.sourceAbility);

        //apply chill
        ChillEffect chill = new ChillEffect();
        
        chill.ApplyChill(enemy, chillDuration, slowEffect);

        pierceCount++;

        if (hitVFX != null)
        {
            //Debug.Log("Ice shatter effect");
            GameObject shatter = Instantiate(hitVFX, transform.position, Quaternion.identity);
        }
    }
    public override void OnOutHitEnemy(Enemy enemy)
    {
        base.OnOutHitEnemy(enemy);

        float finalDamage = abilityData.baseDamage * Mathf.Pow(pierceDamageMult, pierceCount - 1) * outHitMult;
        enemy.TakeDamage(finalDamage, abilityData.element, this.sourceAbility);

        if(pierceCount > maxPierces)
        {
            Destroy(gameObject);
        }
    }
    public override void OnHitNonEnemy(Collider other)
    {
        if (hitVFX != null)
        {
            Debug.Log("Ice shatter effect");
            GameObject shatter = Instantiate(hitVFX, transform.position, Quaternion.identity);

        }
        Destroy(gameObject);
    }
    
    
}
