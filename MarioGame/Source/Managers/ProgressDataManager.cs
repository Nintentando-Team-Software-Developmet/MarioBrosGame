using Microsoft.Xna.Framework;

using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Managers;

public class ProgressDataManager
{
    private ProgressData _data;
    private const int DefaultTime = 300;

    public ProgressDataManager()
    {
        _data = new ProgressData(DefaultTime, 456, 987654);
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
}
