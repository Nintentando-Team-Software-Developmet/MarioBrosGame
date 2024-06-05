using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Systems;

namespace SuperMarioBros
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch Batch { get; private set; }
        public SpriteFont Font { get; private set; }
        private WorldGame _world;
        private SpriteData spriteData;
        private Texture2D _pixelTexture;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this)
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
            Batch = new SpriteBatch(GraphicsDevice);
            Sprites.Load(Content);
            Font = Content.Load<SpriteFont>("Fonts/Title");
            _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _pixelTexture.SetData(new Color[] { Color.White });
            spriteData = new SpriteData(Batch, Font, Content, Graphics, _pixelTexture);
            _world = new WorldGame(spriteData);
            _world.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _world.Draw();
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Batch?.Dispose();
                Graphics?.Dispose();
                _world?.Dispose();
                _pixelTexture?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
