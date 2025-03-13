using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [Header("Enemy and spawns")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    private List<Enemy> activeEnemies = new List<Enemy>();

    [Header("Enemy move info ")]
    [SerializeField]private float spawnHeight = 5;
    [SerializeField]private int maxEnemyNumber = 3;
    private int enemyCount = 0;
    [HideInInspector]public float combatDuration;
    private bool isCombatActive = false;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
        else Destroy(gameObject);
    }
    public void StartSpawning()
    {
        isCombatActive = true;
        enemyCount = 0;
        activeEnemies.Clear();
        SpawnPoint[] foundSpawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        List<Transform> spawnerList = new List<Transform>();

        foreach (SpawnPoint point in foundSpawns)
        {
            spawnerList.Add(point.transform);
            
        }
        spawnPoints = spawnerList.ToArray();

        StartCoroutine(SpawnEnemiesOverTime());
    }
    private IEnumerator SpawnEnemiesOverTime()
    {
        while (isCombatActive)
        {
            if (enemyCount < maxEnemyNumber)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(combatDuration / maxEnemyNumber);
        }
    }
    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, spawnHeight, 0);
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Enemy newEnemy = enemyObject.GetComponent<Enemy>();
        if (newEnemy != null)
        {
            int waypointsToUse = Random.Range(3, 6);
            float speedMultiplier = 1f + (0.2f * enemyCount);

            activeEnemies.Add(newEnemy);
            DamageTimeManager.instance.allEnemies.Add(newEnemy);

            newEnemy.BeginFall(spawnPoint.position, 
                EnemyWaypointManager.instance.GetRandomWaypoints(waypointsToUse), speedMultiplier);
        }

        enemyCount++;
    }
    public void EndCombat()
    {
        isCombatActive = false;

        foreach (Enemy enemy in activeEnemies)
        {
            //Do something to enemies at the end of the damage phase?
            //destroy them and spawn a vfx
            //need to make sure they send their combat damage info before getiing destoryed!!!
            //if (enemy != null) Destroy(enemy.gameObject);
        }

        activeEnemies.Clear();
        enemyCount = 0;
    }
}
