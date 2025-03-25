using UnityEngine;

public class VenomTide : Ability
{
    public GameObject wavePrefab; // VFX Object
    
    public override void Execute()
    {
        base.Execute();

        Vector3 spawnPosition = PlayerManager.instance.GetSpellSpawnPoint(abilityData.spellSpawnNumber).position;
        spawnPosition.y = 0f;

        Vector3 shootDirection = Camera.main.transform.forward;
        shootDirection.y = 0;
        shootDirection.Normalize();

        GameObject waveObject = Instantiate(wavePrefab, spawnPosition, Quaternion.LookRotation(shootDirection));
        VenomTideProjectile waveScript = waveObject.GetComponent<VenomTideProjectile>();
        Debug.Log(waveObject);

        if (waveScript != null)
        {
            waveScript.Initialize(abilityData, this);
        }
    }
}
