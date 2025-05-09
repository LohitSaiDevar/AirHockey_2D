using UnityEngine;

public enum PlayerNumber
{
    Player_1,
    Player_2
}

public class Player : MonoBehaviour
{
    [SerializeField] PlayerNumber playerNumber;
    [SerializeField] Vector2 initialPos;
    public PlayerNumber PlayerNumber
    {
        get { return playerNumber; }
        private set { playerNumber = value; }
    }
    void Start()
    {
        
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
