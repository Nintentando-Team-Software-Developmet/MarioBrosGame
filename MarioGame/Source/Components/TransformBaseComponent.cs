using Microsoft.Xna.Framework;

namespace SuperMarioBros.Source.Components
{
    public class TransformBaseComponent : BaseComponent
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
    }
}
