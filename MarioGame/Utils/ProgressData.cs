namespace SuperMarioBros.Utils;

public class ProgressData
{
    private double _time;
    private int _coins;
    private int _score;

    public ProgressData(double time, int coins, int score)
    {
        _time = time;
        _coins = coins;
        _score = score;
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
}
