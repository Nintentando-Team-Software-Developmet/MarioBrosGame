using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Components;

public class FireBoolComponent: BaseComponent
{
    public AetherVector2 originalPosition { get; set; }
    public bool collidedWithGoomba { get; set; }
    public float InitialDirection { get; set; }
}
