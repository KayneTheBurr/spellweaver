using UnityEngine;

public class StunEffect : StatusEffect
{
    public float stunMultiplier = 2.5f;
    public float slowEffect = 0.01f;
    
    public void ApplyStun(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }

    protected override void StartEffect()
    {
        target.AddEffect(this);
        target.ModifyDamageMultiplier(this, stunMultiplier);
        target.isMoving = false;
        
    }

    public override void RemoveEffect()
    {
        target.RemoveDamageMultiplier(this, stunMultiplier);
        target.isMoving = true;
        target.RemoveEffect(this);
    }
}