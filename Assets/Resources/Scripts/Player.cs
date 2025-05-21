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
    private void Update()
    {
        PosBoundary();
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

    void PosBoundary()
    {
        if (transform.position.x > 2.3f)
        {
            transform.position = new Vector2(2.3f, transform.position.y);
        }
        else if (transform.position.x < -2.3f)
        {
            transform.position = new Vector2(-2.3f, transform.position.y);
        }


        switch (playerNumber)
        {
            case PlayerNumber.Player_1:
                if (transform.position.y > -0.5f)
                {
                    transform.position = new Vector2(transform.position.x, -0.5f);
                }
                else if (transform.position.y < -4.3f)
                {
                    transform.position = new Vector2(transform.position.x, -4.3f);
                }
                break;

            case PlayerNumber.Player_2:
                if (transform.position.y < 0.5f)
                {
                    transform.position = new Vector2(transform.position.x, 0.5f);
                }
                else if (transform.position.y > 4.3f)
                {
                    transform.position = new Vector2(transform.position.x, 4.3f);
                }
                break;
        }
    }
}
