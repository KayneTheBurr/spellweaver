using UnityEngine;
using UnityEngine.VFX;

public class VenomTideProjectile : Projectile
{
    [Header("Wave Values")]
    public float waveSpeed = 10f;
    public float waveDuration = 3f;

    [Header("Poison Values")]
    public float poisonTotalDamage = 750f;
    public float poisonDuration = 15f;
    public float poisonTickRate = 1f;

    [Header("Smog Values")]
    public GameObject poisonSmogPrefab; // Smog Effect
    public float smogSpawnInterval = 0.5f;
    public float smogDuration = 4f;

    private float elapsedTime = 0;
    private VisualEffect vfxGraph;
    
    public override void Initialize(AbilityData abilityData, Ability ability)
    {
        base.Initialize(abilityData, ability);
        
        vfxGraph = GetComponent<VisualEffect>();
        
        rb.linearVelocity = transform.forward * waveSpeed;
        if (vfxGraph != null)
        {
            vfxGraph.SetFloat("WaveDuration", waveDuration);
        }
        Destroy(gameObject, waveDuration);
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= smogSpawnInterval)
        {
            SpawnSmog();
            elapsedTime = 0f;
        }
    }
    public override void OnHitEnemy(Enemy enemy)
    {
        enemy.TakeDamage(abilityData.baseDamage, abilityData.element, sourceAbility);

        PoisonEffect poison = new PoisonEffect();
        poison.ApplyPoison(enemy, poisonTotalDamage, poisonDuration, poisonTickRate);
    }
    public override void OnHitNonEnemy(Collider col)
    {
        
        //Destroy(gameObject);
    }
    private void SpawnSmog()
    {
        if (poisonSmogPrefab != null)
        {
            GameObject smog = Instantiate(poisonSmogPrefab, transform.position, Quaternion.identity);
            PoisonSmog smogScript = smog.GetComponent<PoisonSmog>();
            smogScript.Initialize(abilityData, sourceAbility);
        }
    }
}
