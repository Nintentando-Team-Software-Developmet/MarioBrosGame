using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros
{
    /**
    * This class manage the sprite data.
    */
    public class SpriteData {
        public SpriteBatch spriteBatch { get; set; }
        public SpriteFont spriteFont { get; set; }

        public SpriteData(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
        }
    }
}