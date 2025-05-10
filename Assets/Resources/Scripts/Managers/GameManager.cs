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
    bool isGameOver;

    
    
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


        uiManager = UIManager.Instance;
        scoreManager = ScoreManager.Instance; 
    }

    void Start()
    {
        remainingTime = initialTime;
        remainingCountdownTime = initialCountdownTime;
        uiManager.gameOverUI.SetActive(false);
        StartCoroutine(nameof(SetCountDownTimer));
    }

    private void Update()
    {
        
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
            remainingCountdownTime -= Time.deltaTime;
            uiManager.DisplayCountdownTimer((int)remainingCountdownTime);
            yield return null;
        }

        uiManager.countdownTimerText.gameObject.SetActive(false);
        StopCoroutine(nameof(SetMatchTimer));
        StartCoroutine(nameof(SetMatchTimer));
    }
    private void GameOver()
    {
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
        OnGameReset?.Invoke();
    }
}

