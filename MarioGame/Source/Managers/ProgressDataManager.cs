using Microsoft.Xna.Framework;

using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Managers;

/*
 * Manages the Progress Data globally by modifying in real time according to events.
 * Values modified are: time, coins and score.
 */
public class ProgressDataManager
{
    private ProgressData _data;
    private const int DefaultTime = 300;
    private HighScoreManager _highScoreManager;

    public ProgressDataManager()
    {
        _data = new ProgressData(DefaultTime, 456, 987654);
        _highScoreManager = new HighScoreManager();
    }

    public void Update(GameTime gameTime)
    {
        if (gameTime != null) _data.Time -= gameTime.ElapsedGameTime.TotalSeconds;
    }

    public ProgressData Data
    {
        get => _data;
        set => _data = value;
    }

    public double Time
    {
        get => _data.Time;
        set => _data.Time = value;
    }

    public int Coins
    {
        get => _data.Coins;
        set => _data.Coins = value;
    }

    public int Score
    {
        get => _data.Score;
        set => _data.Score = value;
    }

    public void UpdateHighScore()
    {
        _highScoreManager.UpdateHighScore(_data.Score);
    }

    public int GetHighScore()
    {
        return _highScoreManager.GetHighScore();
    }
}
