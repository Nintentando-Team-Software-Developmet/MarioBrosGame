using Microsoft.Xna.Framework;

namespace SuperMarioBros.Source.Systems;

public class GameDataSystem : BaseSystem
{
    private int totalScore;
    private int coinsCounter;
    private string levelName;
    private double time;

    public GameDataSystem(int totalScore, int coinsCounter, string levelName, double time)
    {
        this.totalScore = totalScore;
        this.coinsCounter = coinsCounter;
        this.levelName = levelName;
        this.time = time;
    }

    public override void Update(GameTime gameTime)
    {
        // Todo: The systemManager is not implemented yet in World.cs,
        // so this system is included into Game1.cs directly until SystemManager is correctly implemented
    }

    public int TotalScore
    {
        get => totalScore;
        set => totalScore = value;
    }

    public int CoinsCounter
    {
        get => coinsCounter;
        set => coinsCounter = value;
    }

    public string LevelName
    {
        get => levelName;
        set => levelName = value;
    }

    public double Time
    {
        get => time;
        set => time = value;
    }
}
