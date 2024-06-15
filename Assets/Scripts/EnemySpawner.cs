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

    public int enemiesKilled = 0;

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
            SpawnEnemy();
            timer = 0;            
        }
    }

    public void CalculateMaxEnemies(Enemies enemyRef)
    {
        enemiesKilled++;
        enemiesAlive.Remove(enemyRef);
    }

    public void SpawnEnemy()
    {
        int i = UnityEngine.Random.Range(0, enemiesToSpawn.Count);

        Enemies newEnemy = Instantiate(enemiesToSpawn[i], new Vector3(UnityEngine.Random.Range(minOffset, maxOffset),
            transform.position.y, UnityEngine.Random.Range(minOffset, maxOffset)), transform.rotation);

        enemiesAlive.Add(newEnemy);
    }

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    public int GetMaxEnemies()
    { 
        return maxEnemies;
    }

    public void SetMaxEnemies(int enemiesToAdd)
    {
        maxEnemies += enemiesToAdd;
    }

    public void ClearEnemies() 
    {
        for (int i = 0; i < enemiesAlive.Count; i++)
        {
            Destroy(enemiesAlive[i].gameObject);
        }
        enemiesAlive.Clear();
    }
}
