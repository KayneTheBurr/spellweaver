using UnityEngine;

public class ToxicBlazeEffect : DamageOverTimeEffect
{
    public void ApplyToxicBlaze(Enemy target, float totalDamage, float duration, float tickInterval)
    {
        DOT blazeDOT = new DOT
        {
            element = ElementType.Fire,
            totalDamage = totalDamage,
            duration = duration,
            tickInterval = tickInterval
        };
        ApplyDOT(blazeDOT, target);

    }
    protected override void StartEffect()
    {
        base.StartEffect();
        target.enemyStatusManager.ApplyEffect(Status.ToxicBlaze);
    }
    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.ToxicBlaze);
        base.RemoveEffect();
    }
}
