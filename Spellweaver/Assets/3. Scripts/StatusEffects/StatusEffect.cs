using Unity.VisualScripting;
using UnityEngine;

public abstract class StatusEffect
{
    public AbilityData abilityData;
    public float duration;
    public float elapsedTime;
    protected Enemy target;

    public virtual void ApplyEffect(Enemy enemy, float duration)
    {
        
        this.duration = duration;
        this.elapsedTime = 0f;
        target = enemy;
        StartEffect();
    }
    public virtual void UpdateEffect(float timeDelta)
    {
        elapsedTime += timeDelta;
        if(elapsedTime > duration)
        {
            RemoveEffect();
        }
    }
    protected virtual void StartEffect()
    {
        bool effectApplied = target.AddEffect(this);
        if (!effectApplied)
        {
            Debug.Log("effects mixed, base effect not applied");
            return;
        }
    }
    public virtual void RemoveEffect()
    {
        target.RemoveEffect(this);
    }
}
