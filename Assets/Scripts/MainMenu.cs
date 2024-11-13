using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject LeaderBoardScreen;
    [SerializeField] AudioClip selectClip;

    public void StartGame()
    {
        AudioManager.Instance.PlaySFX(selectClip);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX(selectClip);
        Application.Quit();
    }

    public void OpenLeaderBoard()
    {
        AudioManager.Instance.PlaySFX(selectClip);

        LeaderBoardScreen.SetActive(true);
        this.gameObject.SetActive(false);

        LeaderBoard.Instance.InstantiateScoreRow();
    }
}
