using System;
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Managers;

/*
 * Manages the Progress Data globally by modifying in real time according to events.
 * Values modified are: time, coins and score.
 */
public class ProgressDataManager
{
    private ProgressData _data;
    private const int DefaultTime = 400;
    private HighScoreManager _highScoreManager;
    private Collection<TemporaryScore> _temporaryScores = new();

    public ProgressDataManager()
    {
        _data = new ProgressData(DefaultTime, 0, 0, 3, new PlayerComponent());
        _highScoreManager = new HighScoreManager();
    }

    public void Update(GameTime gameTime)
    {
        if (gameTime == null)
            throw new ArgumentNullException(nameof(gameTime));

        if (!_data.PlayerComponent.HasReachedEnd)
        {
            _data.Time -= gameTime.ElapsedGameTime.TotalSeconds;
        }
        else if (_data.PlayerComponent.HasReachedEnd)
        {
            _data.Score += (int)_data.Time * 100;
            _data.Time = 0;
        }
    }

    public void AddCollectItem(int points)
    {
        _data.Score += points;
        _temporaryScores.Add(new TemporaryScore(new Vector2(_data.PlayerComponent.PlayerPositionX, _data.PlayerComponent.PlayerPositionY), points, 1.5f));
    }

    public void CalculatePoleHeight()
    {
        int jumpHeight = _data.PlayerComponent.PlayerPositionY;

        int points = jumpHeight switch
        {
            < 150 => 10000,
            < 350 => 5000,
            < 450 => 2000,
            < 500 => 1000,
            _ => 100
        };

        _data.Score += points;
        _temporaryScores.Add(new TemporaryScore(new Vector2(_data.PlayerComponent.PlayerPositionX, _data.PlayerComponent.PlayerPositionY), points, 1.5f));
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

    public int Lives
    {
        get => _data.Lives;
        set => _data.Lives = value;
    }

    public Collection<TemporaryScore> TemporaryScores
    {
        get => _temporaryScores;
    }

    public void ResetTime()
    {
        _data.Time = DefaultTime;
    }


    public void ResetLevel()
    {
        ResetTime();
        _data.Coins = 0;
        _data.Score = 0;
        _data.Lives = 3;
        _data.PlayerComponent = new PlayerComponent();
    }
    public void ResetPlayer()
    {
        _data.PlayerComponent = new PlayerComponent();
    }

    public void UpdateHighScore()
    {
        _highScoreManager.UpdateHighScore(_data.Score);
    }

    public int GetHighScore()
    {
        return _highScoreManager.GetHighScore();
    }

    public void IncreaseScore(int score)
    {
        _data.Score += score;
    }
}
