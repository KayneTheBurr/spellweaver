using UnityEngine;

public class BurnEffect : DamageOverTimeEffect
{
    public void ApplyBurn(Enemy target, float totalDamage, float duration, float tickInterval)
    {
        DOT burnDOT = new DOT
        {
            element = ElementType.Fire,
            totalDamage = totalDamage,
            duration = duration,
            tickInterval = tickInterval
        };
        Debug.Log("Trigger Burn");
        StatusEffectDatabase.instance.DiscoverEffect(Status.Burn);
        ApplyDOT(burnDOT, target);
    }
    protected override void StartEffect()
    {
        bool effectApplied = target.AddEffect(this);
        if (!effectApplied)
        {
            Debug.Log("effects mixed, base effect not applied");
            return;
        }
        target.enemyStatusManager.ApplyEffect(Status.Burn);
    }
    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Burn);
        base.RemoveEffect();
    }
}
