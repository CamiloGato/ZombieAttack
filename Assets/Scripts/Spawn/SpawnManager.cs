using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int timeBetweenWaves = 5;
    public int amountOfEnemiesToSpawn = 5;
    public bool gameOver;
    
    public List<GameObject> enemies;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 1f, timeBetweenWaves);
    }

    private void Spawn()
    {
        if (enemies.Count == 0 && !gameOver)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                for (int i = 0; i < amountOfEnemiesToSpawn; i++)
                {
                    var enemy =Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, gameObject.transform);
                    enemies.Add(enemy);
                }
            }
        }
        if (gameOver) CancelInvoke(nameof(Spawn));
    }
    
}
