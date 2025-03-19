using System.Collections;
using UnityEngine;

public class GlacierCrystalObject : Projectile
{
    public float chillDuration = 5f;
    public float slowEffect = 0.6f;
    public float fallDelayTime = 0.5f;
    public float fallForce = 100f;
    private bool isBoostedDamage = false;
    private bool hasLanded = false;
    public GameObject impactVFX;

    public override void Initialize(AbilityData abilityData, Ability ability)
    {
        base.Initialize(abilityData, ability);
        StartCoroutine(DelayedFall());
    }
    public void BoostDamageForMiddle(bool isMiddleCrystal)
    {
        if(isMiddleCrystal)
        {
            isBoostedDamage = true;
        }
    }
    private IEnumerator DelayedFall()
    {
        yield return new WaitForSeconds(fallDelayTime);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * fallForce, ForceMode.Impulse);
        }
    }
    public override void OnHitEnemy(Enemy enemy)
    {
        if (hasLanded) return;

        float damageDone = abilityData.baseDamage;
        if(isBoostedDamage)
        {
            damageDone *= 2;
        }
        enemy.TakeDamage(damageDone, ElementType.Ice, sourceAbility);

        ChillEffect chill = new ChillEffect();
        chill.ApplyChill(enemy, chillDuration, slowEffect);
    }
    public override void OnHitNonEnemy(Collider col)
    {
        if (hasLanded) return;

        if (impactVFX != null)
        {
            Instantiate(impactVFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
