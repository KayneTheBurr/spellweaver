using UnityEngine;

public struct DOT
{
    public ElementType element;
    public float totalDamage;
    public float duration;
    public float tickInterval;
}
public class DamageOverTimeEffect : StatusEffect
{
    public DOT dotEffect;
    private float tickDamage;
    private float tickTimer = 0f;

    public void ApplyDOT(DOT dot, Enemy target)
    {
        dotEffect = dot;
        tickDamage = dot.totalDamage / (dot.duration / dot.tickInterval);

        ApplyEffect(target, dot.duration);
    }

    public override void UpdateEffect(float timeDelta)
    {
        base.UpdateEffect(timeDelta);

        tickTimer += timeDelta;
        if(tickTimer >= dotEffect.tickInterval)
        {
            tickTimer = 0;
            target.TakeDamageOverTime(tickDamage, dotEffect.element);
        }


    }
}
