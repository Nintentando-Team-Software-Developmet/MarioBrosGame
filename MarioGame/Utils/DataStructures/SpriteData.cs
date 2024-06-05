using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros.Utils.DataStructures
{
    /**
    * This class manage the sprite data.
    */
    public class SpriteData
    {
        public SpriteBatch spriteBatch { get; set; }
        public SpriteFont spriteFont { get; set; }
        public ContentManager content { get; set; }
        public GraphicsDeviceManager graphics { get; set; }
        public Texture2D pixelTexture { get; set; }

        public SpriteData(SpriteBatch spriteBatch, SpriteFont spriteFont, ContentManager content, GraphicsDeviceManager graphics, Texture2D pixelTexture)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.content = content;
            this.graphics = graphics;
            this.pixelTexture = pixelTexture;
        }

    }
}
