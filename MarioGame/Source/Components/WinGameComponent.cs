namespace SuperMarioBros.Source.Components;

public class WinGameComponent : BaseComponent
{
    public bool MarioContact { get; set; }

    public WinGameComponent()
    {
        MarioContact = false;
    }
}
