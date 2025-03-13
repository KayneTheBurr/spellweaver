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
        ApplyDOT(burnDOT, target);
    }
}
