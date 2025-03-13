using System.Collections;
using UnityEngine;

public class HitScanAbility : Ability
{
    public float range = 50f;
    public float sphereRadius = 0.5f;
    public LayerMask hitMask;
    public float abilityDuration = 2.0f;
    public GameObject hitScanPrefab;

    public override void Execute()
    {
        base.Execute();

        hitScanPrefab = abilityData.abilityPrefab;

        RaycastHit hit;
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, range, hitMask)) 
        {
            //Debug.Log($"Used {abilityData.abilityName}");

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                OnHitEnemy(enemy, hit.point, hit.normal);
            }
            else
            {
                OnHitEnvironment(hit.point, hit.normal);
            }
        }
        else
        {
            Debug.Log(" miss :( ");
        }
    }
    protected virtual void ApplyHitEffects(Enemy enemy, GameObject hitscanObject)
    {
        
    }
    protected virtual void OnHitEnemy(Enemy enemy, Vector3 hitPoint, Vector3 hitNormal)
    {
        //Debug.Log($"Hit enemy");

        //enemy.TakeDamage(abilityData.baseDamage, abilityData.element);

        if (hitScanPrefab != null)
        {
            GameObject hitPrefab = Instantiate(hitScanPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            HitScanAbility hitscanAbility = hitPrefab.GetComponent<HitScanAbility>();

            if (hitscanAbility != null)
            {
                hitscanAbility.ApplyHitEffects(enemy, hitPrefab);
            }
        }
    }
    protected virtual void OnHitEnvironment(Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("Hit wall/floor");

        if (hitScanPrefab != null)
        {
            GameObject hitPrefab = Instantiate(hitScanPrefab, 
                hitPoint, Quaternion.LookRotation(hitNormal));
            //Debug.Log($"{abilityData.abilityName} instantiated at {hitPoint}");
            
        }
    }
}
