using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros.Utils.DataStructures
{
    /**
    * This class manage the sprite data.
    */
    public class SpriteData
    {
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont SpriteFont { get; set; }

        public SpriteData(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            this.SpriteBatch = spriteBatch;
            this.SpriteFont = spriteFont;
        }
    }
}
