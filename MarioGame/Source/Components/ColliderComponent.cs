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
        public float acceleration { get; set;}
        public float friction { get; set;}
        public CollisionType lastCollision { get; set; }

        public ColliderComponent(World physicsWorld, float x, float y, Rectangle rectangle, BodyType bodyType, int rotation = 0)
        {
            AetherVector2 position = new AetherVector2(x / Constants.pixelPerMeter, y / Constants.pixelPerMeter);
            collider = physicsWorld?.CreateBody(position, rotation, bodyType);
            collider.FixedRotation = true;
            collider.CreateRectangle(rectangle.Width / Constants.pixelPerMeter, rectangle.Height / Constants.pixelPerMeter, 1f, AetherVector2.Zero);
            collider.OnCollision += HandleCurrentCollision;
        }

        public bool isJumping()
        {
            return collider.LinearVelocity.Y != 0;
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(collider.Position.X * Constants.pixelPerMeter, collider.Position.Y * Constants.pixelPerMeter);
            }
        }

        private bool HandleCurrentCollision( Fixture fixtureA, Fixture fixtureB, Contact contact )
        {
            lastCollision = CollisionAnalyzer.GetDirectionCollision(contact);
            return true;
        }
    }
}
