using Microsoft.Xna.Framework;

namespace SuperMarioBros.Source.Components;

public class PositionComponent: BaseComponent
{
    public Vector2 Position { get; set; }
    public Vector2 LastPosition { get; set; }

    public PositionComponent(Vector2 initialPosition)
    {
        Position = initialPosition;
    }
}
