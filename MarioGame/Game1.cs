using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros.Source.Core;
using SuperMarioBros.Source.Levels;
using Services;
using MarioGame;

namespace SuperMarioBros
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch Batch { get; private set; }
        private WorldGame _world;
        private WorldInitializer _worldInitializer;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _world = new WorldGame();
            _worldInitializer = new WorldInitializer(GraphicsDevice);
            _worldInitializer.Initialize();
        }

        protected override void LoadContent()
        {
            Batch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
           // _world.(gameTime);
            base.Update(gameTime);
        }// Add the missing import statement

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
         // Declare and initialize _spriteBatch variable
            Batch.Begin();
            Vector2 scale = new Vector2(1.5f);
            Batch.Draw(Sprites.Goomba1, new Vector2(100, 100), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

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
