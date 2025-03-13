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
        ApplyDOT(poisonDOT, target);

    }
}
