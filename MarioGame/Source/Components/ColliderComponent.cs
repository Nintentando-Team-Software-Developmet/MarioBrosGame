using System;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

using SuperMarioBros.Utils;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Components
{
    public class ColliderComponent : BaseComponent
    {
        public Body collider { get; set; }
        public float maxSpeed { get; set;}
        public float velocity { get; set;}
        public float friction { get; set;}
        public CollisionType lastCollision { get; set; }

        public ColliderComponent(World physicsWorld, float x, float y, Rectangle rectangle, BodyType bodyType, int rotation = 0)
        {
            AetherVector2 position = new AetherVector2(x / GameConstants.pixelPerMeter, y / GameConstants.pixelPerMeter);
            collider = physicsWorld?.CreateBody(position, rotation, bodyType);
            collider.FixedRotation = true;
            collider.CreateRectangle(rectangle.Width / GameConstants.pixelPerMeter, rectangle.Height / GameConstants.pixelPerMeter, 1f, AetherVector2.Zero);
        }

        public bool isJumping()
        {
            return collider.LinearVelocity.Y != 0;
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(collider.Position.X * GameConstants.pixelPerMeter, collider.Position.Y * GameConstants.pixelPerMeter);
            }
        }

    }
}
