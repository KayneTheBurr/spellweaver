using UnityEngine;

public class ProjectileAbility : Ability
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float projectileSpeed = 15f;

    public override void Execute()
    {
        base.Execute();

        projectilePrefab = abilityData.abilityPrefab;
        spawnPoint = PlayerManager.instance.GetSpellSpawnPoint();
        if (projectilePrefab != null && spawnPoint != null)
        {
            //Debug.Log("spawn in projectile!!!!");

            Transform cameraPos = Camera.main.transform;
            Vector3 shootDirection = cameraPos.forward.normalized;

            GameObject projectile = Instantiate(
                projectilePrefab, spawnPoint.position, Quaternion.LookRotation(shootDirection));
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = shootDirection * projectileSpeed;
            }
            //do damage here
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(abilityData, this);
            }
        }
    }
}
