using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject LeaderBoardScreen;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLeaderBoard()
    {
        LeaderBoardScreen.SetActive(true);
        this.gameObject.SetActive(false);

        LeaderBoard.Instance.InstantiateScoreRow();
    }
}
