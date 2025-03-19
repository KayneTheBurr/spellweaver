using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Combustion : HitScanAbility
{
    public float combustionMult = 2f;
    public ParticleSystem combustionEffect;
    public override void Execute()
    {
        base.Execute();
    }
    protected override void ApplyHitEffects(Enemy enemy, GameObject hitscanObject)
    {
        if (enemy != null)
        {
            DetonateDoTs(enemy);
        }
    }
    private void DetonateDoTs(Enemy target)
    {
        float combustionDamage = 0f;
        List<DamageOverTimeEffect> dots = target.activeEffects.OfType<DamageOverTimeEffect>().ToList();
        
        foreach (var dot in dots)
        {
            combustionDamage += dot.dotEffect.totalDamage;
            dot.RemoveEffect();
            target.RemoveEffect(dot);
        }
        if (combustionDamage > 0 && dots.Count > 0)
        {
            combustionDamage *= combustionMult;
            combustionDamage *= dots.Count / 3;
            target.TakeDamage(combustionDamage, ElementType.Fire, this);

            if (combustionEffect != null)
            {
                ParticleSystem vfxInstance = Instantiate(
                    combustionEffect, target.transform.position, Quaternion.identity);
                vfxInstance.Play();
                Destroy(vfxInstance.gameObject, vfxInstance.main.duration);
            }
        }

        Destroy(gameObject);
    }
}
