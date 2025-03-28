using UnityEngine;

public class FrozenEffect : StatusEffect
{
    
    private float damageMultiplier = 2f;
    public void ApplyFrozen(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }

    protected override void StartEffect()
    {
        base.StartEffect();
        target.enemyStatusManager.ApplyEffect(Status.Freeze);
        target.isMoving = false;
        target.ModifyDamageMultiplier(this, damageMultiplier);
    }

    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Freeze);
        target.RemoveDamageMultiplier(this, damageMultiplier);
        target.isMoving = true;
        base.RemoveEffect();
    }
}
