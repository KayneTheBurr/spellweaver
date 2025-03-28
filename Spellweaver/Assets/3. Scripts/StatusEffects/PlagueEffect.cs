using UnityEngine;

public class PlagueEffect : DamageOverTimeEffect
{
    private float pulseInterval = 2.5f;
    private float pulseTimer = 0f;
    private float spreadPoisonDamage = 10f;
    private float plagueMult = 0.1f;
    private float spreadRange = 5.0f;
    public void ApplyPlague(Enemy target, float totalDamage, float duration, float tickInterval)
    {
        DOT poisonDOT = new DOT
        {
            element = ElementType.Poison,
            totalDamage = totalDamage,
            duration = duration,
            tickInterval = tickInterval
        };
        spreadPoisonDamage = totalDamage * plagueMult;
        ApplyDOT(poisonDOT, target);

    }
    protected override void StartEffect()
    {
        base.StartEffect();
        target.enemyStatusManager.ApplyEffect(Status.Plague);
    }
    public override void UpdateEffect(float timeDelta)
    {
        base.UpdateEffect(timeDelta);
        //pulse the poison here 

        pulseTimer += timeDelta;
        if (pulseTimer >= pulseInterval)
        {
            pulseTimer = 0;
            SpreadPlague();
        }
    }
    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Plague);
        base.RemoveEffect();
    }
    private void SpreadPlague()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(target.transform.position, spreadRange);

        foreach (Collider col in nearbyEnemies)
        {
            Enemy newTarget = col.GetComponent<Enemy>();
            if (newTarget != null && newTarget != target)
            {
                PoisonEffect poison = new PoisonEffect();
                poison.ApplyPoison(newTarget, spreadPoisonDamage, 7f, 1f);
            }
        }
    }
}
