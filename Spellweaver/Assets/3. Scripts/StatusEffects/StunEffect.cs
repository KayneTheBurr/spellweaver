using UnityEngine;

public class StunEffect : StatusEffect
{
    public float stunMultiplier = 3f;
    
    public void ApplyStun(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }

    protected override void StartEffect()
    {
        target.AddEffect(this);
        target.enemyStatusManager.ApplyEffect(Status.Stun);
        target.ModifyDamageMultiplier(this, stunMultiplier);
        target.isMoving = false;
        
    }

    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Stun);
        target.RemoveDamageMultiplier(this, stunMultiplier);
        target.isMoving = true;
        target.RemoveEffect(this);
    }
}