using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _batch;
        private SpriteFont _font;
        private Texture2D _pixelTexture;
        private WorldGame _world;
        private SpriteData _spriteData;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1080,
                PreferredBackBufferHeight = 720
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _batch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Fonts/Title");
            _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _pixelTexture.SetData(new Color[] { Color.White });
            _spriteData = new SpriteData(_batch, _font, Content, _graphics, _pixelTexture);
            _world = new WorldGame(_spriteData);
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _world.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _batch?.Dispose();
                _graphics?.Dispose();
                _world?.Dispose();
                _pixelTexture?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
