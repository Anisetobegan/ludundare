using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private List<Enemies> enemiesToSpawn;

    private int maxEnemies = 10;
    private float spawnRate = 2;
    private float timer = 0;

    private float maxOffset = 8;
    private float minOffset = -8;

    int enemiesKilled = 0;
    int enemiesSpawned = 0;

    [SerializeField] private List<Enemies> enemiesAlive;


    private void Start()
    {
        Actions.OnEnemyKilled += CalculateMaxEnemies;
    }

    private void OnDestroy()
    {
        Actions.OnEnemyKilled -= CalculateMaxEnemies;
    }

    private void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else if (timer == float.MaxValue)
        {
            timer = 0;
        }
        else
        {
            if (enemiesSpawned < maxEnemies)
            {
                SpawnEnemy();                
            }
            timer = 0;
        }
    }

    public void CalculateMaxEnemies(Enemies enemyRef)
    {
        enemiesKilled++;

        
    }

    public void SpawnEnemy()
    {
        int i = UnityEngine.Random.Range(0, enemiesToSpawn.Count);

        Enemies newEnemy = Instantiate(enemiesToSpawn[i], new Vector3(UnityEngine.Random.Range(minOffset, maxOffset),
            transform.position.y, UnityEngine.Random.Range(minOffset, maxOffset)), transform.rotation);

        enemiesSpawned++;
    }

    public int EnemiesKilled
    {
        get { return enemiesKilled; }
        set { enemiesKilled = value; } 
    }

    public int EnemiesSpawned
    {
        get { return enemiesSpawned; } set { enemiesSpawned = value; }
    }
    

    public int MaxEnemies
    {
        get { return maxEnemies;  } 
        set { maxEnemies = value; }
    }
}
