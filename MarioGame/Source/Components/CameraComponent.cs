using Microsoft.Xna.Framework;

using Entities;

namespace Components
{
    public class CameraComponent : Component
    {
        public Vector2 Position { get; set; }
        public Entity Target { get; set; }
    }
}
