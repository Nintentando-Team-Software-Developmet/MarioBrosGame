namespace SuperMarioBros.Source.Components;

public class CoinComponent : BaseComponent
{
    public bool IsCollected { get; set; }

    public CoinComponent()
    {
        IsCollected = false;
    }
}
