using System;

using nkast.Aether.Physics2D.Dynamics.Contacts;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;
namespace SuperMarioBros.Utils
{
    public enum CollisionType
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        HORIZONTAL,
        VERTICAL
    }
    public static class CollisionAnalyzer
    {
        public static CollisionType GetCollisionType(Contact contact)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact));
            AetherVector2 normal = contact.Manifold.LocalNormal;
            if (Math.Abs(normal.X) > Math.Abs(normal.Y))
            {
                return CollisionType.HORIZONTAL;
            }
            else
            {
                return CollisionType.VERTICAL;
            }
        }

        /**
            * <summary>
            * Returns the direction of the collision, returns the section of the rectangle that was hit.
            * </summary>
            * <param name="contact">The contact of the collision.</param>
            * <returns>The direction of the collision.</returns>
            */
        public static CollisionType GetDirectionCollision(Contact contact)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact));
            AetherVector2 normal = contact.Manifold.LocalNormal;
            if (Math.Abs(normal.X) > Math.Abs(normal.Y))
            {
                return normal.X > 0 ? CollisionType.RIGHT : CollisionType.LEFT;
            }
            else
            {
                return normal.Y > 0 ? CollisionType.UP : CollisionType.DOWN;
            }
        }
    }
}
