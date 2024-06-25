using System;

using Microsoft.Xna.Framework;

using MonoGame.Framework.Utilities;

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
        public void Enabled(bool enabled)
        {
            collider.Enabled = enabled;
        }

        public Vector2 Position
        {
            get
            {
                float x = collider.Position.X * GameConstants.pixelPerMeter;
                float y = collider.Position.Y * GameConstants.pixelPerMeter;
                return new Vector2(x, y);
            }
        }


    }
}
