using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems;

public class GameDataSystem : BaseSystem
{
    private ScoreComponent _totalScore;
    private CoinsComponent _coinsCounter;
    private string _levelName;
    private TimeComponent _time;
    private const int DEFAULT_TIME = 300;

    public GameDataSystem()
    {
        _totalScore = new ScoreComponent(123);
        _coinsCounter = new CoinsComponent(546);
        _levelName = "1-1";
        _time = new TimeComponent(DEFAULT_TIME);
    }

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        if (gameTime != null) _time.Seconds -= gameTime.ElapsedGameTime.TotalSeconds;
    }

    public ScoreComponent TotalScore
    {
        get => _totalScore;
        set => _totalScore = value;
    }

    public CoinsComponent CoinsCounter
    {
        get => _coinsCounter;
        set => _coinsCounter = value;
    }

    public string LevelName
    {
        get => _levelName;
        set => _levelName = value;
    }

    public TimeComponent Time
    {
        get => _time;
        set => _time = value;
    }
}
