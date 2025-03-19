using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernoBeam : Ability
{
    [Header("Beam Variables")]
    public float beamDuration = 4f;
    public float tickRate = 1f;
    public float burnDuration = 5.0f;
    public float burnDamage = 100f;
    public float burnBonusMultiplier = 1.5f;
    public LayerMask hitMask;
    public float beamLengthMult = 0.25f;

    //test length var
    public float beamLengthTest;
    
    [Header("VFX stuff")]
    public GameObject flameVFX;
    private GameObject fireStreamInstance;

    public bool isFiring = false;
    private float elapsedTime = 0f;
    private float nextTickTime = 0f;
    private Transform firePoint;
    private Vector3 beamEndPoint;

    private float currentDamage;
    private float currentBurnDamage;

    public override void Execute()
    {
        base.Execute();

        firePoint = PlayerManager.instance.GetSpellSpawnPoint(abilityData.spellSpawnNumber);
        GameObject beamObject = Instantiate(abilityData.abilityPrefab, firePoint.position, firePoint.rotation);
        InfernoBeam beamInstance = beamObject.GetComponent<InfernoBeam>();

        if (beamInstance != null)
        {
            beamInstance.StartFiring(beamInstance);
        }
    }
    private void Update()
    {
        if (isFiring)
        {
            HandleBeamVisual();
        }
    }
    private void HandleBeamVisual()
    {
        if (fireStreamInstance == null || PlayerManager.instance.GetSpellSpawnPoint(abilityData.spellSpawnNumber) == null) return;

        Vector3 origin = PlayerManager.instance.GetSpellSpawnPoint(abilityData.spellSpawnNumber).position;
        Vector3 direction = Camera.main.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, hitMask))
        {
            beamEndPoint = hit.point;
        }
        else
        {
            beamEndPoint = origin + (direction * 50f); // Default max range
        }

        // Update FireStream position & scale dynamically
        //fireStreamInstance.transform.position = origin;
        fireStreamInstance.transform.LookAt(beamEndPoint);

        float beamLength = Vector3.Distance(origin, beamEndPoint);
        beamLengthTest = beamLength;
        //fireStreamInstance.transform.localScale = new Vector3(1, 1, beamLength * beamLengthMult);

        ParticleSystem ps = fireStreamInstance.GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        mainModule.startLifetime = beamLength * beamLengthMult;
    }
    public void StartFiring(InfernoBeam instance)
    {
        if (isFiring) return;

        isFiring = true;
        elapsedTime = 0f;
        nextTickTime = 0f;

        currentDamage = abilityData.baseDamage;
        currentBurnDamage = burnDamage;

        fireStreamInstance = Instantiate(flameVFX, PlayerManager.instance.GetSpellSpawnPoint(abilityData.spellSpawnNumber).position, Quaternion.identity);
        fireStreamInstance.transform.SetParent(transform);
        fireStreamInstance.transform.localPosition = new Vector3(0, -3, 0);

        StartCoroutine(ChannelBeam());
    }
    private IEnumerator ChannelBeam()
    {
        while (elapsedTime < beamDuration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= nextTickTime)
            {
                ApplyDamage();
                nextTickTime += tickRate;
            }

            yield return null;
        }

        StopFiring();
    }
    private void ApplyDamage()
    {
        RaycastHit hit;
        Vector3 origin = PlayerManager.instance.GetSpellSpawnPoint(abilityData.spellSpawnNumber).position;
        Vector3 direction = Camera.main.transform.forward;

        if(Physics.Raycast(origin, direction, out hit, Mathf.Infinity, hitMask))
        {
            beamEndPoint = hit.point; // Update beam endpoint

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float finalBurnDamage = currentBurnDamage;
                if (enemy.HasEffect<BurnEffect>())
                {
                    finalBurnDamage *= burnBonusMultiplier;
                }

                enemy.TakeDamage(currentDamage, ElementType.Fire, this);

                // Apply burn every 2 seconds
                if ((int)elapsedTime % 2 == 0)
                {
                    BurnEffect burn = new BurnEffect();
                    burn.ApplyBurn(enemy, finalBurnDamage, burnDuration, 0.5f);
                    currentBurnDamage *= 1.2f; // Increase burn damage with each burn
                }

                currentDamage *= 1.1f;//increase damage each tick 
            }
        }
        else
        {
            beamEndPoint = origin + (direction * 40f);
        }
    }
    private void StopFiring()
    {
        isFiring = false;

        if (fireStreamInstance != null)
        {
            Destroy(fireStreamInstance, 1f);
        }

        Destroy(gameObject);
    }
}