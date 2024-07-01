using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character player;

    [SerializeField] private EnemySpawner spawner;

    private int wave;

    private int enemiesToAddPerWave = 5;

    private float timeBetweenWaves = 3;

    [SerializeField] private GameObject waveScreen;

    public Transform PlayerTransform
    {
        get { return player.transform; }
    }

    public float playerColliderRadius { get { return player.ColliderRadius; } }

    public static GameManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        wave = 1;
    }

    private void Update()
    {
        if (spawner.EnemiesKilled == spawner.MaxEnemies) //checks if player killed the amount of max enemies per wave
        {
            spawner.enemiesKilled = 0;

            WaveWon();

            StartCoroutine(StartNewWave());
                                             
        }
    }

    public void WaveWon()
    {
        waveScreen.SetActive(true);

        spawner.gameObject.SetActive(false);

        spawner.ClearEnemies();

        wave += 1;

        //Choose random perk        
    }

    public void Lose()
    {

    }

    public IEnumerator StartNewWave()
    {        
        yield return new WaitForSeconds(timeBetweenWaves);

        waveScreen.SetActive(false);

        spawner.MaxEnemies += enemiesToAddPerWave;

        spawner.gameObject.SetActive(true);
    }


    public void Pause()
    {

    }

    public void Resume()
    {

    }

    public void GenerateRandomPerks()
    {

    }

}
