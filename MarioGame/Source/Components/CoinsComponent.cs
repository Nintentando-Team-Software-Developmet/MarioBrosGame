namespace SuperMarioBros.Source.Components;

public class CoinsComponent : BaseComponent
{
    public int Coins { get; set; }

    public CoinsComponent(int coins)
    {
        Coins = coins;
    }
}
