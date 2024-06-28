namespace SuperMarioBros.Source.Components;

public class WinPoleSensorComponent : BaseComponent
{
    public bool MarioContact { get; set; }

    public WinPoleSensorComponent()
    {
        MarioContact = false;
    }
}
