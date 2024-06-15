using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character player;

    [SerializeField] private EnemySpawner spawner;

    private int wave;

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
        wave = 0;
    }

    public void WaveWon()
    {

    }

    public void Lose()
    {

    }

    public void StartNewWave()
    {
        
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
