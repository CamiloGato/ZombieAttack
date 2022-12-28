using System;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int amount;
    
    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < amount; i++)
            {
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, gameObject.transform);
            }
        }
    }
}
