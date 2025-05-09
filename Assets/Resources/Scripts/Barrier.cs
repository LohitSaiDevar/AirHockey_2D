using UnityEngine;

public enum Score
{
    Low = 5,
    Medium = 10,
    High = 20
}

public enum PlayerBarrier
{
    Player_1,
    Player_2
}

public class Barrier : MonoBehaviour
{
    [SerializeField] Score fixedScore;
    int yourScore;

    [SerializeField] PlayerBarrier playerSide;

    private void Awake()
    {
        yourScore = (int)fixedScore;
    }
    public int YourScore
    {
        get { return yourScore; }
        private set { yourScore = value; }
    }

    public PlayerBarrier PlayerSide
    {
        get { return playerSide; }
        private set { playerSide = value; }
    }
}
