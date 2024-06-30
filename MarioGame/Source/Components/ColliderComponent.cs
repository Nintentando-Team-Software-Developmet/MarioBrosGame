using System;
using System.Linq;
using System.Reflection.Metadata;

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
        public Fixture fixture { get; set; }
        public float maxSpeed { get; set; }
        public float velocity { get; set; }
        public float friction { get; set; }
        private Fixture[] _storedFixtures;
        public int width { get; set; }
        public int height { get; set; }

        public ColliderComponent(World physicsWorld, float x, float y, Rectangle rectangle, BodyType bodyType, int rotation = 0)
        {
            AetherVector2 position = new AetherVector2(x / GameConstants.pixelPerMeter, y / GameConstants.pixelPerMeter);
            collider = physicsWorld?.CreateBody(position, rotation, bodyType);
            collider.FixedRotation = true;
            fixture = collider.CreateRectangle(rectangle.Width / GameConstants.pixelPerMeter, rectangle.Height / GameConstants.pixelPerMeter, 1f, AetherVector2.Zero);
            width = rectangle.Width;
            height = rectangle.Height;
        }

        public bool isJumping()
        {
            if (collider.LinearVelocity.Y != 0) return true;
            var position = (int)collider.Position.Y * GameConstants.pixelPerMeter;
            if (position != GameConstants.positionFloor)
            {
                var contactEdge = collider.ContactList;
                while (contactEdge != null)
                {
                    var currentContact = contactEdge.Contact;
                    if (CollisionAnalyzer.GetDirectionCollision(currentContact) == CollisionType.UP)
                    {
                        return false;
                    }
                    contactEdge = contactEdge.Next;
                }
                return true;
            }
            else
            {
                return false;
            }
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

        public void RemoveCollider()
        {
            if (collider != null && collider.FixtureList.Count > 0)
            {
                _storedFixtures = collider.FixtureList.ToArray();
                foreach (var fixture in _storedFixtures)
                {
                    collider.Remove(fixture);
                }
            }
        }

        public void ResizeRectangle(int width, int height)
        {
            if (width == this.width && height == this.height) return;
            float heightPosition;
            if (height > this.height)
            {
                heightPosition = height / 4;
                collider.Position = new AetherVector2(collider.Position.X, collider.Position.Y - (heightPosition / GameConstants.pixelPerMeter));
            }
            else
            {
                heightPosition = height / 2;
                collider.Position = new AetherVector2(collider.Position.X, collider.Position.Y + (heightPosition / GameConstants.pixelPerMeter));
            }
            this.width = width;
            this.height = height;
            collider.Remove(fixture);
            fixture = collider.CreateRectangle(width / GameConstants.pixelPerMeter, height / GameConstants.pixelPerMeter, 1f, AetherVector2.Zero);

        }
    }
}
