using UnityEngine;

public class SuperchargedEffect : StatusEffect
{
    private float iceMultiplier = 1.5f;
    private float lightningMultiplier = 1.5f;

    public void ApplySupercharged(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }

    protected override void StartEffect()
    {
        base.StartEffect();
        target.enemyStatusManager.ApplyEffect(Status.Supercharged);
        target.ModifyElementMultiplier(ElementType.Ice, iceMultiplier);
        target.ModifyElementMultiplier(ElementType.Lightning, lightningMultiplier);
    }

    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Supercharged);
        target.RemoveElementMultiplier(ElementType.Ice, iceMultiplier);
        target.RemoveElementMultiplier(ElementType.Lightning, lightningMultiplier);
        base.RemoveEffect();
    }
}
