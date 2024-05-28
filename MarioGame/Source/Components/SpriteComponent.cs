using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    public class SpriteComponent : Component
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
    }
}
