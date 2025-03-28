using UnityEngine;

public class WetEffect : StatusEffect
{
    private float lightningMultiplier = 1.5f;

    public void ApplyWet(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }

    protected override void StartEffect()
    {
        base.StartEffect();

        target.enemyStatusManager.ApplyEffect(Status.Wet);
        target.ModifyElementMultiplier(ElementType.Lightning, lightningMultiplier);
    }

    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Wet);
        target.RemoveElementMultiplier(ElementType.Lightning, lightningMultiplier);
        base.RemoveEffect();
    }
}
