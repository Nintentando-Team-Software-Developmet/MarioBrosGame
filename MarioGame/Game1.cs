using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarioGame;

namespace SuperMarioBros
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch Batch { get; private set; }
        public SpriteFont Font { get; private set; }
        private WorldGame _world;
        private SpriteData spriteData;

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
            Font = Content.Load<SpriteFont>("Fonts/Title");
            spriteData = new SpriteData(Batch,Font );
            _world = new WorldGame(spriteData);
            _world.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
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
            }

            base.Dispose(disposing);
        }
    }
}
