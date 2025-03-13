using System.Collections;
using UnityEngine;

public class ThunderstrikeProjectile : Projectile
{
    public float markDuration = 1.5f;
    public float stunFrozenMultiplier = 3.0f;
    public GameObject lightningEffectPrefab;

    private Vector3 strikePosition;
    private Enemy markedEnemy;
    private Transform parentEnemy;

    public float embedDepth = 0.2f;
    public float groundEmbedDepth = 0.1f;
    public LayerMask abilityProjectileLayer;

    public override void Initialize(AbilityData abilityData, ProjectileAbility ability)
    {
        base.Initialize(abilityData, ability);
        damage = abilityData.baseDamage;
    }
    private IEnumerator TriggerLightning()
    {
        yield return new WaitForSeconds(markDuration);

        if (markedEnemy != null)
        {
            strikePosition = markedEnemy.transform.position;
            float finalDamage = damage;
            
            if (markedEnemy.HasEffect<StunEffect>() || markedEnemy.HasEffect<FrozenEffect>())
            {
                finalDamage *= stunFrozenMultiplier;
            }
            
            markedEnemy.TakeDamage(finalDamage, ElementType.Lightning, sourceAbility);
        }
        

        if (lightningEffectPrefab != null)
        {
            Instantiate(lightningEffectPrefab, strikePosition, Quaternion.identity);
        }
        else
        {
            DrawLightningEffect(strikePosition);
        }

        Destroy(gameObject);
    }
    public override void OnHitEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        markedEnemy = enemy;
        StopProjectile();

        transform.position += transform.forward * embedDepth;
        transform.SetParent(enemy.transform);

        StartCoroutine(TriggerLightning());

    }
    public override void OnHitNonEnemy(Collider col)
    {
        if (((1 << col.gameObject.layer) & abilityProjectileLayer) != 0)
        {
            Debug.Log("Hit your own ability...nice! ");
            return;
        }

        StopProjectile();

        strikePosition = transform.position;

        transform.position += transform.forward * embedDepth;

        StartCoroutine(TriggerLightning());
    }
    private void StopProjectile()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
    private void DrawLightningEffect(Vector3 targetPosition)
    {
        GameObject lightningObject = new GameObject("LightningEffect");
        LineRenderer lr = lightningObject.AddComponent<LineRenderer>();

        lr.positionCount = 10;
        lr.startWidth = 0.3f;
        lr.endWidth = 0.1f;

        Vector3 start = targetPosition + Vector3.up * 30f; // Lightning starts above
        lr.SetPosition(0, start);

        for (int i = 1; i < lr.positionCount - 1; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.2f, 0.2f),
                0);
            lr.SetPosition(i, Vector3.Lerp(start, targetPosition, i / (float)lr.positionCount) + randomOffset);
        }

        lr.SetPosition(lr.positionCount - 1, targetPosition);

        Destroy(lightningObject, 0.3f);
    }
}
