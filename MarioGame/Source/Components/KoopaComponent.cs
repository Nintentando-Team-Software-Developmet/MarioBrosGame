namespace SuperMarioBros.Source.Components;

public class KoopaComponent : BaseComponent
{
    public bool IsKnocked { get; set; }

    public KoopaComponent(bool isKnocked)
    {
        IsKnocked = isKnocked;
    }
}
