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

    public bool isGamePaused;
    public GameObject pausePanel;

    public Text coinsText;
    public Text scoreText;

    public int countdownTime = 3;
    public Text resumeCounter;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        gameOver = false;
        isGameStarted = false;
        isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {

        coinsText.text = "Coins: " + PlayerPrefs.GetInt("TotalCoins", 0);
        scoreText.text = "Score: " + PlayerPrefs.GetInt("CurrentScore", 0);

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
            pausePanel.SetActive(true);
            countdownTime = 3;
        }
    }

    public void ResumeGame()
    {
        if (isGamePaused)
        {
            //TODO add a 3s timer to resume 
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            StartCoroutine(ResumeCountdown());
        }
    }

    IEnumerator ResumeCountdown()
    {
        resumeCounter.gameObject.SetActive(true);

        while (countdownTime > 0)
        {
            resumeCounter.text = countdownTime.ToString();
            Debug.Log(countdownTime);
            yield return new WaitForSeconds(1f);
            Debug.Log(countdownTime);
            countdownTime--;
        }

        resumeCounter.gameObject.SetActive(false);
        isGamePaused = false;
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
