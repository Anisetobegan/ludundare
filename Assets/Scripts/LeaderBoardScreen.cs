using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardScreen : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] Transform ScoreContainer;

    public void ReturnToMainMenu()
    {
        this.gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void DestroyHighScoreRows()
    {
        for (int i = ScoreContainer.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(ScoreContainer.GetChild(i).gameObject);
        }
    }
}
