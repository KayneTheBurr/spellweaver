using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShockedEffect : StatusEffect
{
    public float damageMultiplier = 1.2f;

    public void ApplyShock(Enemy target, float duration)
    {
        StatusEffectDatabase.instance.DiscoverEffect(Status.Shock);
        //Debug.Log("Trigger Shock");
        ApplyEffect(target, duration);
    }
    protected override void StartEffect()
    {
        bool effectApplied = target.AddEffect(this);
        if (!effectApplied)
        {
            Debug.Log("effects mixed, base effect not applied");
            return;
        }
        //if (target.HasEffect<StunEffect>())
        //{
        //    Debug.Log("already shocked");
        //    return;
        //}
        //if (target.HasEffect<ChargedEffect>())
        //{
        //    Debug.Log("already charged");
        //    return;
        //}
        //if (target.HasEffect<ElectrocuteEffect>()) return;
        target.enemyStatusManager.ApplyEffect(Status.Shock);
        Debug.Log("no electric effect, apply shock");
        target.ModifyDamageMultiplier(this, damageMultiplier);

    }
    public override void UpdateEffect(float timeDelta)
    {
        base.UpdateEffect(timeDelta);

    }
    public override void RemoveEffect()
    {
        target.enemyStatusManager.RemoveEffect(Status.Shock);
        target.RemoveDamageMultiplier(this, damageMultiplier);
        base.RemoveEffect();
    }
}
