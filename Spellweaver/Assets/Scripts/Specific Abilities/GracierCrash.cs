using System.Collections;
using UnityEngine;

public class GracierCrash : HitScanAbility
{
    public float iceSpawnHeight = 18f;
    public float iceSpawnDelayMain = 0.5f;
    public float iceSpawnDelaySide = 0.3f;
    public int numberOfGlaciers = 4;
    public float destroyCloudTime = 2f;
    public GameObject glacierCrystalObject;
    public GameObject iceCloudVFX;

    private Vector3 iceSpawnPoint;

    public override void Execute()
    {
        base.Execute();
    }
    protected override void OnHitEnemy(Enemy enemy, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnHitEnemy(enemy, hitPoint, hitNormal);
    }
    protected override void OnHitEnvironment(Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnHitEnvironment(hitPoint, hitNormal);
    }
    protected override void ApplyHitEffects(Enemy enemy, GameObject hitscanObject)
    {
        StartCoroutine(SummonIce(enemy.transform.position));
        Destroy(gameObject, 3f);
    }
    protected override void ApplyHitEnviroEffects(Vector3 hitPoint, Vector3 hitNormal, GameObject hitscanObject)
    {
        StartCoroutine(SummonIce(hitPoint));
    }
    private IEnumerator SummonIce(Vector3 hitPoint)
    {
        iceSpawnPoint = hitPoint + new Vector3(0, iceSpawnHeight, 0);
        yield return new WaitForSeconds(iceSpawnDelayMain);

        SummonIceCrystal(iceSpawnPoint, true);
        if(iceCloudVFX != null)
        {
            GameObject iceStorm = Instantiate(iceCloudVFX, iceSpawnPoint, Quaternion.identity);
            Destroy(iceStorm, destroyCloudTime);
        }

        Vector3 glacierSpawn1 = iceSpawnPoint + new Vector3(0, 0, 3);
        Vector3 glacierSpawn2 = iceSpawnPoint + new Vector3(3, 0, 0);
        Vector3 glacierSpawn3 = iceSpawnPoint + new Vector3(0, 0, -3);
        Vector3 glacierSpawn4 = iceSpawnPoint + new Vector3(-3, 0, 0);

        yield return new WaitForSeconds(iceSpawnDelaySide);
        SummonIceCrystal(glacierSpawn1, false);
        yield return new WaitForSeconds(iceSpawnDelaySide);
        SummonIceCrystal(glacierSpawn2, false);
        yield return new WaitForSeconds(iceSpawnDelaySide);
        SummonIceCrystal(glacierSpawn3, false);
        yield return new WaitForSeconds(iceSpawnDelaySide);
        SummonIceCrystal(glacierSpawn4, false);


    }
    private void SummonIceCrystal(Vector3 spawnPoint, bool isMiddle)
    {
        GameObject glacier = Instantiate(glacierCrystalObject, spawnPoint, Quaternion.identity);
        glacier.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 90));
        Rigidbody rb = glacier.GetComponent<Rigidbody>();
        GlacierCrystalObject crystalScript = glacier.GetComponent<GlacierCrystalObject>();
        if(crystalScript != null)
        {
            crystalScript.Initialize(abilityData, this);
            crystalScript.BoostDamageForMiddle(isMiddle);
        }
    }

}
