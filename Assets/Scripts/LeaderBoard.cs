using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] Transform ScoreContainer;
    [SerializeField] Transform HighScoreRow;

    int maxHighScoreCount = 10;

    List<HighScore> highScoreList = new List<HighScore>();

    public static LeaderBoard Instance
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
        LoadHighScore();

        /*while (highScoreList.Count > maxHighScoreCount)
        {
            highScoreList.RemoveAt(maxHighScoreCount);
        }

        for (int i = 0; i < highScoreList.Count; i++)
        {
            InstantiateScoreRow(highScoreList[i]);
        } */       
    }

    public void InstantiateScoreRow()
    {
        for (int i = 0; i < highScoreList.Count; i++)
        {
            GameObject newRow = Instantiate(HighScoreRow.gameObject, ScoreContainer);
            newRow.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = highScoreList[i].name.ToString();
            newRow.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = highScoreList[i].score.ToString();
        }        
    }

    public void AddHighScore(HighScore newHighScore)
    {
        for (int i = 0; i < maxHighScoreCount; i++)
        {
            if (highScoreList.Count == 0 || newHighScore.score > highScoreList[i].score)
            {
                highScoreList.Insert(i, newHighScore);
            }

            while (highScoreList.Count > maxHighScoreCount)
            {
                highScoreList.RemoveAt(maxHighScoreCount);
            }

            SaveHighScore();

            break;
        }
    }

    public class HighScoreList
    {
        public List<HighScore> highScoreList;
    }

    public void SaveHighScore()
    {
        HighScoreList highScore = new HighScoreList() { highScoreList = this.highScoreList };
        string json = JsonUtility.ToJson(highScore);
        PlayerPrefs.SetString("LeaderBoard", json);
        PlayerPrefs.Save();
    }

    void LoadHighScore()
    {
        string jsonString = PlayerPrefs.GetString("LeaderBoard");
        HighScoreList highScore = JsonUtility.FromJson<HighScoreList>(jsonString);
        highScoreList = highScore.highScoreList;
        if (highScoreList == null)
        {
            highScoreList = new List<HighScore>();
        }
    }    
}
