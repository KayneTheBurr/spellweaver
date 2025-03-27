using UnityEngine;

public class BrittleEffect : StatusEffect
{
    private float damageMultiplier = 1.3f;
    private float fireMultiplier = 1.5f;
    public void ApplyBrittle(Enemy enemy, float duration)
    {
        ApplyEffect(enemy, duration);
    }
    protected override void StartEffect()
    {
        base.StartEffect();
        target.ModifyDamageMultiplier(this, damageMultiplier);
        target.ModifyElementMultiplier(ElementType.Fire, fireMultiplier);
    }

    public override void RemoveEffect()
    {
        target.RemoveDamageMultiplier(this, damageMultiplier);
        target.RemoveElementMultiplier(ElementType.Fire, fireMultiplier);
        base.RemoveEffect();
    }
}
