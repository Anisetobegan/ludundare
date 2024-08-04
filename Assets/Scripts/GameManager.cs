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

    [SerializeField] GameObject choosePerkScreen;

    IEnumerator enumerator = null;

    public Character PlayerGet { get { return player; } }

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
            spawner.EnemiesKilled = 0;

            spawner.EnemiesSpawned = 0;

            WaveWon();

            enumerator = StartNewWave();
            StartCoroutine(enumerator);
                                             
        }
    }

    public void WaveWon()
    {
        waveScreen.SetActive(true);

        spawner.gameObject.SetActive(false);

        wave += 1;
    }

    public void Lose()
    {

    }

    public IEnumerator StartNewWave()
    {        
        yield return new WaitForSeconds(timeBetweenWaves);

        waveScreen.SetActive(false);

        spawner.MaxEnemies += enemiesToAddPerWave;        

        OpenPerkSelectionScreen();

        enumerator = null;
    }

    public void OpenPerkSelectionScreen()
    {
        choosePerkScreen.SetActive(true);

        spawner.gameObject.SetActive(false);
    }

    public void ClosePerkSelectionScreen() 
    {
        choosePerkScreen.SetActive(false);

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
