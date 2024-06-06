namespace SuperMarioBros.Source.Components;

public class ScoreComponent : BaseComponent
{
    public int Score { get; set; }

    public ScoreComponent(int score)
    {
        Score = score;
    }
}
