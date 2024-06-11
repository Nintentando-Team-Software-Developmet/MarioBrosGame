using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros.Source.Components;

public class CameraComponent : BaseComponent
{
    public Matrix Transform { get; set; }
    public Vector2 Position { get; set; }
    public Viewport Viewport { get; set; }
    public int WorldWidth { get; set; }
    public int WorldHeight { get; set; }
    public float LastXPosition { get; set; }

    public CameraComponent(Viewport viewport, int worldWidth, int worldHeight)
    {
        Viewport = viewport;
        WorldWidth = worldWidth;
        WorldHeight = worldHeight;
        Transform = Matrix.Identity;
        LastXPosition = 0;
    }
}
