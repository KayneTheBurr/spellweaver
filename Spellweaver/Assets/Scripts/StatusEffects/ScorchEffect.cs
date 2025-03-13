using UnityEngine;

public class ScorchEffect : StatusEffect
{
    private float fireMultiplier = 1.5f;
    public void ApplyScorch(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }
    protected override void StartEffect()
    {
        base.StartEffect();
        target.ModifyElementMultiplier(ElementType.Fire, fireMultiplier);
    }

    public override void RemoveEffect()
    {
        target.RemoveElementMultiplier(ElementType.Fire, fireMultiplier);
        base.RemoveEffect();
    }
}
