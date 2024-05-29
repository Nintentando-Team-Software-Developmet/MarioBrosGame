using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Components
{
    public class CameraComponent : BaseComponent
    {
        public Vector2 Position { get; set; }
        public Entity Target { get; set; }
    }
}
