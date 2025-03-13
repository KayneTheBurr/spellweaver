using UnityEngine;

public class ChillEffect : StatusEffect
{
    public float slowEffect = 0.75f;
    
    public void ApplyChill(Enemy target, float duration, float multiplier)
    {
        Debug.Log(" Trigger Chill");
        slowEffect = multiplier;
        ApplyEffect(target, duration);
    }
    protected override void StartEffect()
    {
        base.StartEffect();
        
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
