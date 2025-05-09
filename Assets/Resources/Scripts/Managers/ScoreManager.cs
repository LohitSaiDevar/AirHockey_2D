using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; set; }

    public static Action<Collision2D> OnScoreChanged;
    public static Action<Collision2D> OnPlayerTurnChanged;

    private int player_1_Score = 0;
    private int player_2_Score = 0;
    public bool player1_turn;

    UIManager uiManager;
    private void Start()
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
    }

    private void OnEnable()
    {
        OnPlayerTurnChanged += PlayerOneOrTwo;
        OnScoreChanged += UpdateScore;
        GameManager.OnGameReset += ResetScore;
    }

    private void OnDisable()
    {
        OnPlayerTurnChanged -= PlayerOneOrTwo;
        OnScoreChanged -= UpdateScore;
        GameManager.OnGameReset -= ResetScore;
    }

    void PlayerOneOrTwo(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Player player))
        {
            switch (player.PlayerNumber)
            {
                case PlayerNumber.Player_1:
                    player1_turn = true;
                    Debug.Log("Player 1");
                    break;

                case PlayerNumber.Player_2:
                    player1_turn = false;
                    Debug.Log("Player 2");
                    break;

                default:
                    Debug.Log("There's no such player");
                    break;
            }
        }
    }

    void UpdateScore(Collision2D collision)
    {
        Barrier barrier = collision.gameObject.GetComponent<Barrier>();
        if (player1_turn && barrier.PlayerSide == PlayerBarrier.Player_2)
        {
            player_1_Score += barrier.YourScore;
            
            Debug.Log("Player 1 score: " + player_1_Score);
        }
        else if(!player1_turn && barrier.PlayerSide == PlayerBarrier.Player_1)
        {
            player_2_Score += barrier.YourScore;
            Debug.Log("Player 2 score: " + player_2_Score);
        }
        uiManager.DisplayScore();
    }

    void ResetScore()
    {
        player1_turn = false;
        player_1_Score = 0;
        player_2_Score = 0;
    }

    public int Player_1_Score
    {
        get { return player_1_Score; }
        set { player_1_Score = value; }
    }

    public int Player_2_Score
    {
        get { return player_2_Score; }
        set { player_2_Score = value; }
    }
}
