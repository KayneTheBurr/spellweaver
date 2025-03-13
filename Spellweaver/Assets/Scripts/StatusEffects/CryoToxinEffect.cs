using UnityEngine;

public class CryoToxinEffect : StatusEffect
{
    private float iceMultiplier = 1.5f;

    public void ApplyCryotoxin(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }

    protected override void StartEffect()
    {
        base.StartEffect();
        target.ModifyElementMultiplier(ElementType.Ice, iceMultiplier);
    }

    public override void RemoveEffect()
    {
        target.RemoveElementMultiplier(ElementType.Ice, iceMultiplier);
        base.RemoveEffect();
    }
}
