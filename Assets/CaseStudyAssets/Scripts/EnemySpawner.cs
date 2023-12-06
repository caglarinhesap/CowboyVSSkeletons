using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float enemySpawnTimer = 2.5f;

    [SerializeField]
    private float monsterSpawnDistance = 22f;
    [SerializeField] 
    private List<GameObject> enemyList = new List<GameObject>();
    private bool spawning = true;


    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        if (spawning)
        {
            Vector3 spawnPosition = GenerateRandomPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemyList.Add(enemy);
            yield return new WaitForSeconds(enemySpawnTimer);
            StartCoroutine(SpawnEnemy());
        }
    }

    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

    Vector3 GenerateRandomPosition()
    {
        Vector3 newPosition = new Vector3(monsterSpawnDistance, 0, 0); //Move spawn point to the desired distance.

        int rotateDegree = Random.Range(0,360); //Rotate new position around origin to generate a random spawn point.

        newPosition.x = monsterSpawnDistance * Mathf.Cos(rotateDegree);
        newPosition.z = monsterSpawnDistance * Mathf.Sin(rotateDegree);

        newPosition += transform.position; // Move new spawn point relative to the spawner.

        return newPosition;
    }

    internal void StopSpawn()
    {
        spawning = false;
    }
}
