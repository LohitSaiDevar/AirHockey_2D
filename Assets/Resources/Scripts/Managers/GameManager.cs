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
        StartCoroutine(nameof(SetMatchTimer));
    }
    private void GameOver()
    {
        audioManager.PlaySFX(audioManager.gameOver);
        isGameOver = true;
        uiManager.gameOverUI.SetActive(true);
    }

    void PlayerVictory()
    {
        if (isGameOver)
        {
            if (scoreManager.Player_1_Score > scoreManager.Player_2_Score)
            {
                Debug.Log("Player 1 wins");
            }
            else
            {
                Debug.Log("Player 2 wins");
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

