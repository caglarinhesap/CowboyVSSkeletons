using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    private int coinAmount = 50;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private GameObject coinParent;

    private int spawnMinX = -50;
    private int spawnMaxX = 50;
    private int spawnMinZ = -50;
    private int spawnMaxZ = 50;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int i=0;i<coinAmount;i++)
        {
            Vector3 spawnPosition = GenerateRandomPosition();
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity, coinParent.transform);
        }
    }

    Vector3 GenerateRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(spawnMinX, spawnMaxX + 1), 0f, Random.Range(spawnMinZ, spawnMaxZ + 1));
        return randomPosition;
    }
}
