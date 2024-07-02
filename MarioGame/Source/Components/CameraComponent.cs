using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Utils.Scene;

using AetherVector = nkast.Aether.Physics2D.Common.Vector2;
namespace SuperMarioBros.Source.Components;

public class CameraComponent : BaseComponent
{
    public Matrix Transform { get; set; }
    public Vector2 Position { get; set; }
    public Viewport Viewport { get; set; }
    public int WorldWidth { get; set; }
    public int WorldHeight { get; set; }
    public float LastXPosition { get; set; }
    public Body LeftWall { get; set; }

    public CameraComponent(Viewport viewport, int worldWidth, int worldHeight, World physicsWorld = null)
    {
        Viewport = viewport;
        WorldWidth = worldWidth;
        WorldHeight = worldHeight;
        Transform = Matrix.Identity;
        LeftWall = null;
        if(physicsWorld != null)
        {
            LeftWall = physicsWorld.CreateBody(new AetherVector(2,2), 0, BodyType.Static);
            Fixture fixture = LeftWall.CreateRectangle(0.01f, worldHeight, 1, AetherVector.Zero);
            fixture.CollisionCategories = Categories.LeftWall;
            fixture.CollidesWith = Categories.Player;
        }
    }
}
