using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public Transform player;
    public float tonnageSunk = 0;
    public bool pause, gameOver;
    public GameObject pauseMenu, gameOverScreen, winScreen;
    private void OnEnable()
    {
        #region Initialize
        if (current != this)
        {
            if(current != null)
            {
                Destroy(current);
            }
            current = this;
        }
        #endregion
        GameOver(false);
        Pause(false);
    }

    public void Pause(bool state)
    {
        pause = state;
        pauseMenu.SetActive(state);
        if (state == false)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void TogglePause()
    {
        if (pause)
        {
            Pause(false);
        }
        else
        {
            Pause(true);
        }
    }

    public void GameOver(bool state)
    {
        gameOver = state;
        gameOverScreen.SetActive(state);
        if (state == false)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
    public void Win(bool state)
    {
        gameOver = state;
        gameOverScreen.SetActive(state);
        if (state == false)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void Close()
    {
        Application.Quit();
    }
}
