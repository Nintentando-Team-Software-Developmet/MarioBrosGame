using System.Reflection.Metadata;

using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Utils;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Components
{
    public class ColliderComponent : BaseComponent
    {
        public Body collider { get; set; }

        public ColliderComponent(World physicsWorld, float x, float y, Rectangle rectangle, BodyType bodyType, int rotation = 0)
        {
            AetherVector2 position = new AetherVector2(x / Constants.pixelPerMeter, y / Constants.pixelPerMeter);
            collider = physicsWorld?.CreateBody(position, rotation, bodyType);
            collider.FixedRotation = true;
            collider.CreateRectangle(rectangle.Width / Constants.pixelPerMeter, rectangle.Height / Constants.pixelPerMeter, 1f, AetherVector2.Zero);
        }
    }
}
