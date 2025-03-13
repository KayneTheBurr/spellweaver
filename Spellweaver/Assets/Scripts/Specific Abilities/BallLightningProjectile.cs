using System.Collections;
using UnityEngine;

public class BallLightningProjectile : Projectile
{
    private bool hasDetonated = false;
    private float pulseTimer = 0f;

    public float pulseInterval = 2f;
    public float detonateDelayTime = 0.8f;
    public float pulseRadius = 4f;
    public float detonateRadius = 8f;
    public float pulseMult = 1.5f;
    public float detonateMult = 3f;
    public LayerMask abilityHitMask;

    public GameObject lightningPulseVFX;
    public GameObject detonateVFX;

    public Color pulseColor = Color.yellow;
    public Color detonateColor = Color.red;
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = pulseColor;
        Gizmos.DrawWireSphere(transform.position, pulseRadius); // Draw pulse radius

        Gizmos.color = detonateColor;
        Gizmos.DrawWireSphere(transform.position, detonateRadius); // Draw detonation radius
    }
    public override void Initialize(AbilityData abilityData, ProjectileAbility ability)
    {
        base.Initialize(abilityData, ability);
        damage = abilityData.baseDamage;
    }
    private void Update()
    {
        pulseTimer += Time.deltaTime;
        if (pulseTimer >= pulseInterval)
        {
            if (hasDetonated) return;
            
            PulseDamage();
            pulseTimer = 0f;
        }
    }
    public override void OnHitEnemy(Enemy enemy)
    {
        PulseDamage();
        Destroy(gameObject);
    }
    public override void OnHitNonEnemy(Collider col)
    {
        if (((1 << col.gameObject.layer) & abilityHitMask) != 0) //ask tim for help on bitmask to understand it better 
        {
            Debug.Log("Detonate?");
            StartCoroutine(Detonate());
        }
        else
        {
            Debug.Log(col.gameObject.layer);
            Destroy(gameObject);
        }
    }
    private void PulseDamage()
    {
        Debug.Log("Pulse");

        if (lightningPulseVFX != null)
        {
            GameObject pulseObject = Instantiate(lightningPulseVFX, transform.position, transform.rotation);
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, pulseRadius);
        foreach (Collider enemyCol in enemies)
        {
            Enemy enemy = enemyCol.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(pulseMult * damage, ElementType.Lightning, sourceAbility);
            }
        }
    }
    private IEnumerator Detonate()
    {
        if (hasDetonated) yield break;
        hasDetonated = true;

        if(detonateVFX != null)
        {
            GameObject detonateObject = Instantiate(detonateVFX, transform.position, transform.rotation);
            Destroy(detonateObject, detonateDelayTime);
        }

        yield return new WaitForSeconds(detonateDelayTime);

        Collider[] enemies = Physics.OverlapSphere(transform.position, pulseRadius);
        foreach (Collider enemyCol in enemies)
        {
            Enemy enemy = enemyCol.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage * detonateMult, ElementType.Lightning, sourceAbility);
            }
        }
        
        Destroy(gameObject);
    }
}
