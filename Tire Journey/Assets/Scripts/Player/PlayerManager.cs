using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;

    public static bool isGamePaused;

    public static int numberOfCoins;
    public Text coinsText;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        gameOver = false;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {

        coinsText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins", 0);

        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }


        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }

    public void PauseGame()
    {
        if (!isGamePaused && !gameOver)
        {
            Time.timeScale = 0;
            isGamePaused = true;
        }
    }

    public void ResumeGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1;
            isGamePaused = false;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("Level");
        gameOver = false;
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
