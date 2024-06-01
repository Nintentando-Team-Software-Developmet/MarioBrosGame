using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Source.Core;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private World _world;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1080,
                PreferredBackBufferHeight = 720
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Move initialization of World to LoadContent
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Utils.Sprites.Load(Content);

            // Now that _spriteBatch is initialized, we can initialize the world
            _world = new World(_spriteBatch);

            var playerTextures = new Texture2D[]
            {
                Utils.Sprites.BigStop,
                Utils.Sprites.BigWalk1,
                Utils.Sprites.BigWalk2,
                Utils.Sprites.BigWalk1Left,
                Utils.Sprites.BigWalk2Left,
                Utils.Sprites.BigBend,
                Utils.Sprites.BigBendLeft,
                Utils.Sprites.BigStopLeft,
                Utils.Sprites.BigJumpBack,
                Utils.Sprites.BigJumpBackLeft,
                Utils.Sprites.BigWalk3,
                Utils.Sprites.BigWalk3Left,
                Utils.Sprites.BigRun,
                Utils.Sprites.BigRunLeft
            };

            var player = new PlayerEntity(playerTextures, new Vector2(100, 100));
            _world.AddEntity(player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
                _graphics?.Dispose();
                _spriteBatch?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
