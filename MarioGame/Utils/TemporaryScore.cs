using Microsoft.Xna.Framework;

namespace SuperMarioBros.Utils;

public class TemporaryScore
{
    public Vector2 Position { get; }
    public int Value { get; set; }
    public float TimeToLive { get; set; }

    public TemporaryScore(Vector2 position, int value, float timeToLive)
    {
        Position = position;
        Value = value;
        TimeToLive = timeToLive;
    }

    public void Update(GameTime gameTime)
    {
        if (gameTime != null) TimeToLive -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public bool IsExpired()
    {
        return TimeToLive <= 0;
    }
}
