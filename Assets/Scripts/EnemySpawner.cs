using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private List<Enemies> enemiesToSpawn;

    private int maxEnemies = 10;
    private float spawnRate = 3;
    private float timer = 0;

    private float maxOffset = 8;
    private float minOffset = -8;

    [SerializeField] private List<Enemies> enemiesAlive;

    private Action OnEnemiesKilled;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (enemiesAlive.Count < maxEnemies)
            {
                SpawnEnemy();
                timer = 0;
            }
        }
    }

    public void CalculateMaxEnemies()
    {

    }

    public void SpawnEnemy()
    {
        int i = UnityEngine.Random.Range(0, enemiesToSpawn.Count);

        Enemies newEnemy = Instantiate(enemiesToSpawn[i], new Vector3(UnityEngine.Random.Range(minOffset, maxOffset),
            transform.position.y, UnityEngine.Random.Range(minOffset, maxOffset)), transform.rotation);

        enemiesAlive.Add(newEnemy);
    }

}
