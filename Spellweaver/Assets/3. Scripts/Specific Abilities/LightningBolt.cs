using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningBolt : HitScanAbility
{
    [Header("Lightning Properties")]
    public int maxChains = 3;
    public float chainRange = 15f; 
    public float damageFalloff = 0.8f; //damage reduction per chain 
    public float chainDelay = 0.25f;
    public float shockDuration = 5.0f;
    public GameObject chainVFX;

    private HashSet<Enemy> hitEnemies = new HashSet<Enemy>(); // Track enemies already hit

    public override void Execute()
    {
        base.Execute();
    }
    private void Awake()
    {
        hitEnemies.Clear();
    }
    protected override void ApplyHitEffects(Enemy enemy, GameObject hitscanObject)
    {
        
        if (enemy != null)
        {
            if (!hitEnemies.Contains(enemy))
            {
                hitEnemies.Add(enemy);
            }

            if (chainVFX != null)
            {
                GameObject chainEffect = Instantiate(chainVFX, enemy.transform.position, Quaternion.identity);
                Destroy(chainEffect, 0.5f);
            }

            //check for shocked status
            ShockedEffect shockEffect = enemy.GetEffect<ShockedEffect>();
            ChargedEffect chargedEffect = enemy.GetEffect<ChargedEffect>();
            if (shockEffect != null || chargedEffect != null)
            {
                
                maxChains += 2;
            }

            //do damage here
            enemy.TakeDamage(abilityData.baseDamage, abilityData.element, this);
            
            //apply shock
            ShockedEffect shock = new ShockedEffect();
            shock.ApplyShock(enemy, shockDuration);

            StartCoroutine(DelayedStartCoroutine(enemy, abilityData.baseDamage * damageFalloff, maxChains));
        }
    }
    private IEnumerator DelayedStartCoroutine(Enemy enemy, float remainingDamage, int remainingChains)
    {
        yield return null;
        StartCoroutine(ChainLightning(enemy, remainingDamage, remainingChains));
    }
    private IEnumerator ChainLightning(Enemy currentTarget, float remainingDamage, int remainingChains)
    {
        if (remainingChains <= 0 || remainingDamage <= 0)
        {
            Destroy(gameObject, 0.1f);
            yield break;
        }

        while (remainingChains > 0 && remainingDamage > 0)
        {
            yield return new WaitForSeconds(chainDelay);

            Collider[] hitColliders = Physics.OverlapSphere(currentTarget.transform.position, chainRange);
            List<Enemy> nearbyEnemies = new List<Enemy>();

            foreach (Collider col in hitColliders)
            {
                Enemy potentialTarget = col.GetComponent<Enemy>();
                //list of all nearby enemies that are not this enemy
                if (potentialTarget != null && potentialTarget != currentTarget)
                {
                    nearbyEnemies.Add(potentialTarget);
                }
            }

            nearbyEnemies = nearbyEnemies.OrderBy(e =>
                hitEnemies.Contains(e) ? 1 : 0) // Prioritize new targets
                .ThenBy(e => Vector3.Distance(currentTarget.transform.position, e.transform.position))
                .ToList();

            if (nearbyEnemies.Count > 0)
            {
                Enemy nextTarget = nearbyEnemies[0]; // Pick the closest valid target

                hitEnemies.Add(nextTarget);
                nextTarget.TakeDamage(remainingDamage, abilityData.element, this);

                if (chainVFX != null)
                {
                    GameObject chainEffect = Instantiate(chainVFX, nextTarget.transform.position, Quaternion.identity);
                    Destroy(chainEffect, 0.5f);
                }

                ShockedEffect shock = new ShockedEffect();
                shock.ApplyShock(nextTarget, shockDuration);

                currentTarget = nextTarget; //redo loop for the new target
                remainingDamage *= damageFalloff; // reduce dmg by the daamge dropoff
                remainingChains--; // decrement chains 
                chainDelay += 0.1f;
            }
            else break; //no target, break early 
        }
        Destroy(gameObject, 0.25f);
    }
}
