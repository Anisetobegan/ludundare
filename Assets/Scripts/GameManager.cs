using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character player;

    [SerializeField] private EnemySpawner spawner;

    private int wave;

    private int enemiesToAddPerWave = 5;

    private float timeBetweenWaves = 3;

    [SerializeField] private GameObject waveScreen;

    [SerializeField] GameObject choosePerkScreen;

    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private GameObject pauseScreen;

    [SerializeField] private GameObject saveHighScoreScreen;

    IEnumerator enumerator = null;

    public static bool isPaused = false;

    [Serializable] public class HighScore
    {
        public string name;
        public int score;

        public HighScore (string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
    List<HighScore> highScoreList = new List<HighScore>();

    [SerializeField] Button saveHighScoreButton;
    [SerializeField] TMP_InputField nameInputField;

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
        saveHighScoreButton.onClick.AddListener(OnSaveButtonClick);
        wave = 1;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && isPaused) 
        {
            Resume();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && !isPaused)
        {
            Pause();
        }

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
        gameOverScreen.SetActive(true);
        spawner.gameObject.SetActive(false);
    }

    public IEnumerator StartNewWave()
    {        
        yield return new WaitForSeconds(timeBetweenWaves);

        waveScreen.SetActive(false);

        OpenPerkSelectionScreen();

        Actions.OnWaveWon?.Invoke();

        spawner.MaxEnemies += enemiesToAddPerWave;

        spawner.gameObject.SetActive(true);

        enumerator = null;
    }

    public void OpenPerkSelectionScreen()
    {
        spawner.gameObject.SetActive(false);

        Time.timeScale = 0;
    }

    public void ClosePerkSelectionScreen() 
    {
        Time.timeScale = 1;

        spawner.gameObject.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ReturnToMainManu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenSaveScoreScreen()
    {
        saveHighScoreScreen.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    public void OnSaveButtonClick()
    {
        if (nameInputField.text != "")
        {
            HighScore newHighScore = new HighScore(nameInputField.text, wave);
            LeaderBoard.Instance.AddHighScore(newHighScore);

            saveHighScoreScreen.SetActive(false);
            gameOverScreen.SetActive(true);
        }
        else
        {
            Debug.Log("Insert a Name");
        }
    }
}
