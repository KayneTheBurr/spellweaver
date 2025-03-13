using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElectrocuteEffect : StatusEffect
{
    private float chainDamage;
    private int remainingChains;
    private float chainRange;
    private float damageFalloff;
    private float timeSinceLastChain = 0f;
    private float chainInterval = 0.2f; 
    private Ability sourceAbility;

    public void ApplyElectrocute(Enemy enemy, float chainDamage,
        int maxChains, float chainRange, float chainDropoff, Ability sourceAbility)
    {
        this.chainDamage = chainDamage;
        this.remainingChains = maxChains;
        this.chainRange = chainRange;
        this.damageFalloff = chainDropoff;
        this.sourceAbility = sourceAbility;

        ApplyEffect(enemy, 1f);
    }
    public override void UpdateEffect(float timeDelta)
    {
        elapsedTime += timeDelta;
        if (remainingChains <= 0 || chainDamage <= 0)
        {
            //remove based on chaining not on time
            RemoveEffect(); 
            return;
        }
        timeSinceLastChain += timeDelta;
        if (timeSinceLastChain >= chainInterval)
        {
            timeSinceLastChain = 0;
            ChainElectrocute(target);
        }
    }
    private void ChainElectrocute(Enemy currentTarget)
    {
        Collider[] hitColliders = Physics.OverlapSphere(currentTarget.transform.position, chainRange);
        List<Enemy> nearbyEnemies = new List<Enemy>();

        foreach (Collider col in hitColliders)
        {
            Enemy potentialTarget = col.GetComponent<Enemy>();
            if (potentialTarget != null && potentialTarget != currentTarget)
            {
                nearbyEnemies.Add(potentialTarget);
            }
        }

        if (nearbyEnemies.Count > 0) //if something to chain to 
        {
            Enemy nextTarget = nearbyEnemies[0];

            nextTarget.TakeDamage(chainDamage, ElementType.Lightning, sourceAbility);

            ShockedEffect shock = new ShockedEffect();
            shock.ApplyShock(nextTarget, 3f);

            target = nextTarget; 
            chainDamage *= damageFalloff; 
            remainingChains--;
        }
        else
        {
            RemoveEffect(); // end if it doesnt chain
        }
    }
}
