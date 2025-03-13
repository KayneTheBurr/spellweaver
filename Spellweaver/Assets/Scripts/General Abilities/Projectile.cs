using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public ProjectileAbility sourceAbility;
    public AbilityData abilityData;
    protected Rigidbody rb;

    public virtual void Initialize(AbilityData abilityData, ProjectileAbility ability)
    {
        this.sourceAbility = ability;
        this.abilityData = abilityData;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        { 
            OnHitEnemy(enemy);
        }
        else
        {
            OnHitNonEnemy(col);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        {
            OnOutHitEnemy(enemy);
        }
        else
        {
            OnOutHitNonEnemy(col);
        }
    }
    public virtual void OnHitEnemy(Enemy enemy)
    {
        
    }
    public virtual void OnHitNonEnemy(Collider col)
    {

    }
    public virtual void OnOutHitEnemy(Enemy enemy)
    {
        
    }
    public virtual void OnOutHitNonEnemy(Collider col)
    {

    }
}
