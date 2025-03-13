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
}
