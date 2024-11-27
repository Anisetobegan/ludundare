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

    bool spawnPointSet = false;

    Camera cam;
    [SerializeField] LayerMask ground;

    [SerializeField] private List<Enemies> enemiesAlive;

    enum Direction
    {
        Top,
        Bottom, 
        Left, 
        Right
    }
    Direction direction;


    private void Start()
    {
        Actions.OnEnemyKilled += CalculateMaxEnemies;

        cam = Camera.main;
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
                if (!spawnPointSet)
                {
                    SpawnEnemy();
                }
                else
                {
                    timer = 0;
                    spawnPointSet = false;
                }
            }            
        }
    }

    public void CalculateMaxEnemies(Enemies enemyRef)
    {
        enemiesKilled++;

        
    }

    public void SpawnEnemy()
    {
        int i = UnityEngine.Random.Range(0, enemiesToSpawn.Count);

        // Enemies newEnemy = Instantiate(enemiesToSpawn[i], new Vector3(UnityEngine.Random.Range(minOffset, maxOffset),
        // transform.position.y, UnityEngine.Random.Range(minOffset, maxOffset)), transform.rotation);

        //ObjectPool.Instance.SpawnFromPool(enemiesToSpawn[i].gameObject.name, new Vector3(UnityEngine.Random.Range(minOffset, maxOffset),
        //    transform.position.y, UnityEngine.Random.Range(minOffset, maxOffset)), transform.rotation);
        //newEnemy.transform.position = new Vector3(UnityEngine.Random.Range(minOffset, maxOffset), transform.position.y, UnityEngine.Random.Range(minOffset, maxOffset));

        Vector3 spawnPoint = SearchSpawnPoint();
        if (spawnPointSet)
        {
            Enemies newEnemy = ObjectPoolManager.Instance.GetFromPool(enemiesToSpawn[i]);

            newEnemy.transform.position = spawnPoint;
            newEnemy.transform.rotation = transform.rotation;

            enemiesSpawned++;
        }
    }

    Vector3 SearchSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        RaycastHit hit;
        Ray ray;

        direction = (Direction)UnityEngine.Random.Range(0, 4);

        switch (direction)
        {
            case Direction.Left:

                ray = cam.ScreenPointToRay(new Vector2(0, UnityEngine.Random.Range(0, Screen.height)));

                if (Physics.Raycast(ray, out hit, 1000, ground))
                {
                    spawnPointSet = true;
                    spawnPoint = hit.point;
                    Vector3 offset = spawnPoint + (spawnPoint - GameManager.Instance.PlayerTransform.position).normalized * 10f;
                    return offset;
                }
                spawnPointSet = false;
                return spawnPoint;

            case Direction.Right:

                ray = cam.ScreenPointToRay(new Vector2(Screen.width, UnityEngine.Random.Range(0, Screen.height)));

                if (Physics.Raycast(ray, out hit, 1000, ground))
                {
                    spawnPointSet = true;
                    spawnPoint = hit.point;
                    Vector3 offset = spawnPoint + (spawnPoint - GameManager.Instance.PlayerTransform.position).normalized * 5f;
                    return offset;
                }
                spawnPointSet = false;
                return spawnPoint;                

            case Direction.Bottom:

                ray = cam.ScreenPointToRay(new Vector2(UnityEngine.Random.Range(0, Screen.width), 0));

                if (Physics.Raycast(ray, out hit, 1000, ground))
                {
                    spawnPointSet = true;
                    spawnPoint = hit.point;
                    Vector3 offset = spawnPoint + (spawnPoint - GameManager.Instance.PlayerTransform.position).normalized * 10f;
                    return offset;
                }
                spawnPointSet = false;
                return spawnPoint;

            case Direction.Top:

                ray = cam.ScreenPointToRay(new Vector2(UnityEngine.Random.Range(0, Screen.width), Screen.height));

                if (Physics.Raycast(ray, out hit, 1000, ground))
                {
                    spawnPointSet = true;
                    spawnPoint = hit.point;
                    Vector3 offset = spawnPoint + (spawnPoint - GameManager.Instance.PlayerTransform.position).normalized * 10f;
                    return offset;
                }
                spawnPointSet = false;
                return spawnPoint;
        }

        spawnPointSet = false;
        return spawnPoint;
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
