
namespace SuperMarioBros.Utils;

/*
 * Represents the accumulative data that is modified during all game.
 * Contains the time, coins and score values.
 */
public class ProgressData
{
    private double _time;
    private int _coins;
    private int _score;
    private int _lives;

    public ProgressData(double time, int coins, int score, int lives)
    {
        _time = time;
        _coins = coins;
        _score = score;
        _lives = lives;
    }

    public double Time
    {
        get => _time;
        set => _time = value;
    }

    public int Coins
    {
        get => _coins;
        set => _coins = value;
    }

    public int Score
    {
        get => _score;
        set => _score = value;
    }

    public int Lives
    {
        get => _lives;
        set => _lives = value;
    }
}
