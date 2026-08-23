using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }

    [SerializeField] TMP_Text player1_Score_Text;
    [SerializeField] TMP_Text player2_Score_Text;
    [SerializeField] GameObject player1_Point_Text_Parent;
    [SerializeField] Transform player1_Point_Transform;
    [SerializeField] Transform player2_Point_Transform;
    [SerializeField] GameObject player2_Point_Text_Parent;
    public TMP_Text winnerText;
    public TMP_Text timerText;
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

    public void DisplayPointIncrease(PlayerNumber playerNumber, int points)
    {
        // 1. Determine which prefab to use based on the player number
        GameObject targetPrefab = (playerNumber == PlayerNumber.Player_1) ? player1_Point_Text_Parent : player2_Point_Text_Parent;
        Transform targetTransform = (playerNumber == PlayerNumber.Player_1) ? player1_Point_Transform : player2_Point_Transform;
        Quaternion targetRotation = (playerNumber == PlayerNumber.Player_1) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

        // 2. Instantiate WITH the targetTransform as the parent
        GameObject pointObj = Instantiate(targetPrefab, targetTransform.position, targetRotation, targetTransform);

        // 3. Force the local scale back to 1, 1, 1 to prevent the Canvas Scaler from blowing it up
        pointObj.transform.localScale = Vector3.one;

        // 4. Set the text (CHANGE TO TMP_Text IF YOUR PREFAB USES TEXTMESHPRO!)
        pointObj.GetComponentInChildren<TMP_Text>().text = "+" + points;

        // 5. Handle Destruction safely by reading the Clip length, not the State length
        Animator anim = pointObj.GetComponentInChildren<Animator>();
        float animDuration = 1f; // Fallback time just in case

        if (anim.runtimeAnimatorController != null)
        {
            // Get the actual animation clip attached to the animator
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
            {
                animDuration = clips[0].length;
            }
        }

        // 6. Destroy using the exact clip duration
        Destroy(pointObj, animDuration);
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
