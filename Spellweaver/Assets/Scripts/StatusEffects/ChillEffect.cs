using UnityEngine;

public class ChillEffect : StatusEffect
{
    public float slowEffect = 0.75f;
    
    public void ApplyChill(Enemy target, float duration, float multiplier)
    {
        StatusEffectDatabase.instance.DiscoverEffect(Status.Chill);
        Debug.Log(" Trigger Chill");
        slowEffect = multiplier;
        ApplyEffect(target, duration);
    }
    protected override void StartEffect()
    {
        bool effectApplied = target.AddEffect(this);
        if (!effectApplied)
        {
            Debug.Log("effects mixed, base effect not applied");
            return;
        }
        if (target.HasEffect<BrittleEffect>())
        {
            Debug.Log("already shocked");
            target.speedMult = 1f;
            return;
        }
        if (target != null)
        {
            target.ModifySpeedMultiplier(slowEffect);
        }
    }
    public override void UpdateEffect(float timeDelta)
    {
        base.UpdateEffect(timeDelta);

    }
    public override void RemoveEffect()
    {
        if (target != null)
        {
            target.RemoveSpeedMultiplier(slowEffect); 
        }
        base.RemoveEffect();
    }
}
