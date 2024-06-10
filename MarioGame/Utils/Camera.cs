using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros.Utils;

public class Camera
{
    public Matrix Transform { get; private set; }
    private Vector2 _position;
    private Viewport _viewport;
    private int _worldWidth;
    private int _worldHeight;

    public Camera(Viewport viewport, int worldWidth, int worldHeight)
    {
        _viewport = viewport;
        _worldWidth = worldWidth;
        _worldHeight = worldHeight;
        Transform = Matrix.Identity;
    }

    public void Follow(Vector2 targetPosition)
    {
        _position = new Vector2(
            MathHelper.Clamp(targetPosition.X - _viewport.Width / 2, 0, _worldWidth - _viewport.Width),
            MathHelper.Clamp(targetPosition.Y - _viewport.Height / 2, 0, _worldHeight - _viewport.Height)
        );

        Transform = Matrix.CreateTranslation(new Vector3(-_position, 0));
    }
}
