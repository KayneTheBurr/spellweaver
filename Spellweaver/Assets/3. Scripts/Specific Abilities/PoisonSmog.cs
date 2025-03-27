using System.Collections.Generic;
using UnityEngine;

public class PoisonSmog : Projectile
{
    [Header("Smog Values")]
    public float smogDuration = 3f;
    public float smogTickDamage = 50f;
    public float smogTickRate = 0.5f;
    public float poisonMult = 2f;
    
    private float elapsedTime = 0f;
    private HashSet<Enemy> enemiesInside = new HashSet<Enemy>();

    public override void Initialize(AbilityData abilityData, Ability ability)
    {
        base.Initialize(abilityData, ability);
        Destroy(gameObject, smogDuration);
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= smogTickRate)
        {
            elapsedTime = 0f;
            ApplyDamage();
        }
    }
    private void ApplyDamage()
    {
       
        if (enemiesInside.Count == 0) return;

        Debug.Log("damaging " + enemiesInside.Count + "enemies");

        foreach (Enemy enemy in enemiesInside)
        {
            if (enemy != null)
            {
                float finalDamage = smogTickDamage;

                // Increase damage if enemy is poisoned
                if (enemy.HasEffect<PoisonEffect>())
                {
                    finalDamage *= poisonMult;
                }

                enemy.TakeDamage(finalDamage, ElementType.Poison, sourceAbility); // No source ability needed
            }
        }
    }
    public override void OnHitEnemy(Enemy enemy)
    {
        enemiesInside.Add(enemy);
        Debug.Log(enemiesInside.Count);
    }
    public override void OnOutHitEnemy(Enemy enemy)
    {
        enemiesInside.Remove(enemy);
        Debug.Log(enemiesInside.Count);
    }
}
