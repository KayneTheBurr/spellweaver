using UnityEngine;

public class PoisonEffect : DamageOverTimeEffect
{
    public void ApplyPoison(Enemy target, float totalDamage, float duration, float tickInterval)
    {
        DOT poisonDOT = new DOT
        {
            element = ElementType.Poison,
            totalDamage = totalDamage,
            duration = duration,
            tickInterval = tickInterval
        };
        Debug.Log("Trigger Poison");
        StatusEffectDatabase.instance.DiscoverEffect(Status.Poison);
        ApplyDOT(poisonDOT, target);
    }
    protected override void StartEffect()
    {
        bool effectApplied = target.AddEffect(this);
        if (!effectApplied)
        {
            Debug.Log("effects mixed, base effect not applied");
            return;
        }
        target.enemyStatusManager.ApplyEffect(Status.Poison);
    }
    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Poison);
        base.RemoveEffect();
    }
}
