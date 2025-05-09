using UnityEngine;

public class Puck : MonoBehaviour
{
    [SerializeField] Vector2 initialPos;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            ScoreManager.OnScoreChanged?.Invoke(collision);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            ScoreManager.OnPlayerTurnChanged?.Invoke(collision);
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameReset += ResetPosition;
    }
    private void OnDisable()
    {
        GameManager.OnGameReset -= ResetPosition;
    }
    void ResetPosition()
    {
        transform.position = initialPos;
    }
}
