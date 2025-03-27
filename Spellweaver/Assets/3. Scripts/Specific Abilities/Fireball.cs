using UnityEngine;

public class Fireball : ProjectileAbility
{
    [Header("Fireball Properties")]
    public float explosionRadius = 3f; 
    public float burnDuration = 5f; 
    public float burnTickInterval = 1f; 
    public float burnTotalDamage = 15f;
    public GameObject explosionVFX;

    public override void Execute()
    {
        base.Execute();


        FireballProjectile fireballScript = GetComponent<FireballProjectile>();

        if (fireballScript != null)
        { 
            fireballScript.SetProperties(
                explosionRadius, burnDuration, burnTickInterval, burnTotalDamage);
        }
    }


}
