using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarioGame;

namespace SuperMarioBros
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch Batch { get; private set; }
        private WorldGame _world;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1080,
                PreferredBackBufferHeight = 720
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _world = new WorldGame();
            //TODO: Review
            //_worldInitializer = new WorldInitializer(GraphicsDevice);
            //_worldInitializer.Initialize();
        }

        protected override void LoadContent()
        {
            Batch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Batch.Begin();
            _world.Draw();
            Batch.End();
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
