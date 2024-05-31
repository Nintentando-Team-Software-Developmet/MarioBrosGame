using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros.Source.Components
{
    public class SpriteBaseComponent : BaseComponent
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
    }
}
