using TMPro;
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
    ScoreManager scoreManager;

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
        Time.timeScale = 0;
        gamePauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
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
        SceneManager.LoadScene(Level);
    }
}
