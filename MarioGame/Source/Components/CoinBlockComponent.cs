using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components;

public class CoinBlockComponent : BaseComponent
{
    public EntitiesName TypeContent { get; set; }
    public int Quantity { get; set; }
    public bool HasMoved { get; set; }

    public CoinBlockComponent(EntitiesName typeContent, int quantity)
    {
        TypeContent = typeContent;
        Quantity = quantity;
    }
}
