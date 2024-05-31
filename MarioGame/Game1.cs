using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros.Source.Core;
using SuperMarioBros.Source.Levels;
using Services;

namespace SuperMarioBros
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch Batch { get; private set; }
        private World _world;
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
            _world = new World();
            _worldInitializer = new WorldInitializer(GraphicsDevice);
            _worldInitializer.Initialize(_world);
        }

        protected override void LoadContent()
        {
            Batch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            base.Update(gameTime);
        }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        Vector2 scale = new Vector2(1.5f);
        _spriteBatch.Draw(Sprites.Goomba1, new Vector2(100, 100), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        _spriteBatch.End();
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
