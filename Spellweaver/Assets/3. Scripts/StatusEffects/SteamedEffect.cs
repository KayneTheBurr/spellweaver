using UnityEngine;

public class SteamedEffect : StatusEffect
{
    private float fireResistance = 0.2f; //80% fire resistance
    public void ApplySteamed(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }
    protected override void StartEffect()
    {
        base.StartEffect();
        target.enemyStatusManager.ApplyEffect(Status.Steamed);
        target.ModifyElementMultiplier(ElementType.Fire, fireResistance);
    }

    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Steamed);
        target.RemoveElementMultiplier(ElementType.Fire, fireResistance);
        base.RemoveEffect();
    }
}
