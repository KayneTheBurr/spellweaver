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
        target.ModifyElementMultiplier(ElementType.Lightning, lightningMultiplier);
    }

    public override void RemoveEffect()
    {
        target.RemoveElementMultiplier(ElementType.Lightning, lightningMultiplier);
        base.RemoveEffect();
    }
}
