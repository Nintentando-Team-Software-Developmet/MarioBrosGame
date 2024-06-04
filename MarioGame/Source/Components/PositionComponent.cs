using Microsoft.Xna.Framework;

namespace SuperMarioBros.Source.Components;

public class PositionComponent: BaseComponent
{
    public Vector2 Position { get; set; }
    public Vector2 LastPosition { get; set; }
    public Vector2 LastPosition2 { get; set; }
    public bool pass  { get; set; }

    public PositionComponent(Vector2 initialPosition)
    {
        Position = initialPosition;

        LastPosition2 = LastPosition2 with { X = initialPosition.X };
        LastPosition2 = LastPosition2 with { Y = initialPosition.Y };

    }
}
