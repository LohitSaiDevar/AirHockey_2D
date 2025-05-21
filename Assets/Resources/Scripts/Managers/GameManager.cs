using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    [Header("Match Timer settings")]
    [SerializeField] float initialTime;
    float remainingTime;

    [Header("Countdown Timer Settings")]
    [SerializeField] float initialCountdownTime;
    float remainingCountdownTime;

    UIManager uiManager;
    ScoreManager scoreManager;
    AudioManager audioManager;
    InputManager inputManager;
    bool isGameOver;


    private const string MainMenu = "MainMenu";
    public static Action OnGameReset;
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
        Application.targetFrameRate = 75;
    }

    void Start()
    {
        uiManager = UIManager.Instance;
        scoreManager = ScoreManager.Instance;
        audioManager = AudioManager.Instance;
        inputManager = InputManager.Instance;

        remainingTime = initialTime;
        remainingCountdownTime = initialCountdownTime;
        uiManager.gameOverUI.SetActive(false);
        StartCoroutine(nameof(SetCountDownTimer));
    }
    private void OnEnable()
    {
        GameManager.OnGameReset += OnReset;
    }
    private void OnDisable()
    {
        GameManager.OnGameReset -= OnReset;
    }

    IEnumerator SetMatchTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            uiManager.DisplayTimer(remainingTime);
            yield return null;
        }

        GameOver();
        PlayerVictory();
    }

    IEnumerator SetCountDownTimer()
    {
        while (remainingCountdownTime > 0f)
        {
            audioManager.PlaySFX(audioManager.countdownTimer);
            uiManager.DisplayCountdownTimer((int)remainingCountdownTime);
            yield return new WaitForSeconds(1f);
            remainingCountdownTime -= 1f;
        }
        uiManager.DisplayCountdownTimer((int)remainingCountdownTime);
        audioManager.PlaySFX(audioManager.countdownEnd);
        uiManager.countdownTimerText.gameObject.SetActive(false);
        inputManager.enabled = true;
        StartCoroutine(nameof(SetMatchTimer));

    }
    private void GameOver()
    {
        audioManager.PlaySFX(audioManager.gameOver);
        isGameOver = true;
        uiManager.gameOverUI.SetActive(true);
        inputManager.enabled = false;
    }

    void PlayerVictory()
    {
        if (isGameOver)
        {
            if (scoreManager.Player_1_Score > scoreManager.Player_2_Score)
            {
                uiManager.winnerText.text = "Player 1";
                uiManager.winnerText.color = new Color(0.8f, 0.13f, 0.13f, 1);
            }
            else
            {
                uiManager.winnerText.text = "Player 2";
                uiManager.winnerText.color = new Color(0, 0.75f, 1, 1);
            }
        }
    }
    
    void OnReset()
    {
        //Countdown Timer
        StopCoroutine(nameof(SetCountDownTimer));
        StopCoroutine(nameof(SetMatchTimer));
        uiManager.countdownTimerText.gameObject.SetActive(true);
        remainingCountdownTime = initialCountdownTime;
        StartCoroutine(nameof(SetCountDownTimer));
        inputManager.enabled = false;

        //Timer
        isGameOver = false;
        remainingTime = initialTime;
        
    }
    public void RestartGame()
    {
        audioManager.PlaySFX(audioManager.uiButton);
        OnGameReset?.Invoke();
    }

    public void ReturnMainMenu()
    {
        audioManager.PlaySFX(audioManager.uiButton);
        SceneManager.LoadScene(MainMenu);
    }
}

