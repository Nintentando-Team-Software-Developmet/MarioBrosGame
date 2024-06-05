using Microsoft.Xna.Framework;

namespace SuperMarioBros.Source.Components;

public class VelocityComponent : BaseComponent
{
    public Vector2 Velocity { get; set; }

    public VelocityComponent(Vector2 initialVelocity)
    {
        Velocity = initialVelocity;
    }
}
