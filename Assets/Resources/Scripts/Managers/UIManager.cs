using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }

    [SerializeField] TMP_Text player1_Score_Text;
    [SerializeField] TMP_Text player2_Score_Text;
    [SerializeField] TMP_Text timerText;
    public TMP_Text countdownTimerText;
    public GameObject gameOverUI;
    public GameObject gamePauseUI;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject howToPlayMenu_Page_1;
    [SerializeField] GameObject howToPlayMenu_Page_2;
    ScoreManager scoreManager;
    AudioManager audioManager;
    public const string Level = "Level";
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        scoreManager = ScoreManager.Instance;
        audioManager = AudioManager.Instance;
    }
    private void OnEnable()
    {
        GameManager.OnGameReset += ResetUI;
    }
    private void OnDisable()
    {
        GameManager.OnGameReset -= ResetUI;
    }
    public void DisplayScore()
    {
        player1_Score_Text.text = "Score: " + scoreManager.Player_1_Score;
        player2_Score_Text.text = "Score: " + scoreManager.Player_2_Score;
    }

    public void DisplayTimer(float remainingTime)
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //Debug.Log("remaining time: " + remainingTime);
    }

    public void DisplayCountdownTimer(int countdownTimer)
    {
        countdownTimerText.text = "" + countdownTimer;
        //Debug.Log("countdown: " + countdownTimer);
    }

    public void DisplayGameOverUI(bool setActive)
    {
        gameOverUI.SetActive(setActive);
    }

    public void PauseGame()
    {
        audioManager.PlaySFX(audioManager.uiButton);
        Time.timeScale = 0;
        gamePauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        audioManager.PlaySFX(audioManager.uiButton);
        Time.timeScale = 1;
        gamePauseUI.SetActive(false);
    }

    public void ResetUI()
    {
        Time.timeScale = 1;
        gameOverUI.SetActive(false);
        gamePauseUI.SetActive(false);
    }

    public void StartGame()
    {
        audioManager.PlaySFX(audioManager.uiButton);
        SceneManager.LoadScene(Level);
    }

    public void HowToPlayMenu_Page1()
    {
        mainMenuUI.SetActive(false);
        howToPlayMenu_Page_1.SetActive(true);
    }
    public void HowToPlayMenu_Page2()
    {
        howToPlayMenu_Page_1.SetActive(false);
        howToPlayMenu_Page_2.SetActive(true);
    }
    public void ReturnToMainMenu()
    {
        howToPlayMenu_Page_2.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
