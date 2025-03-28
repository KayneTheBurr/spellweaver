using UnityEngine;

public class ChargedEffect : StatusEffect
{
    private float lightningMultiplier = 1.5f;

    public void ApplyCharged(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
        
    }

    protected override void StartEffect()
    {
        base.StartEffect();
        target.enemyStatusManager.ApplyEffect(Status.Charged);
        target.ModifyElementMultiplier(ElementType.Lightning, lightningMultiplier);
    }

    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Charged);
        target.RemoveElementMultiplier(ElementType.Lightning, lightningMultiplier);
        base.RemoveEffect();
    }
}
