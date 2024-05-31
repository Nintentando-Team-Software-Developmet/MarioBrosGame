using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarioGame;

using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Source.Components;

namespace SuperMarioBros
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch Batch { get; private set; }
        private WorldGame _world;
        private Player player { get; set; }


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
            Sprites.Load(Content);
            player = new Player(Sprites.BigStop,
                Sprites.BigWalk1,
                Sprites.BigWalk2,
                Sprites.BigWalk1Left,
                Sprites.BigWalk2Left,
                Sprites.BigBend,
                Sprites.BigBendLeft,
                Sprites.BigStopLeft,
                Sprites.BigJumpBack,
                Sprites.BigJumpBackLeft,
                Sprites.BigWalk3,
                Sprites.BigWalk3Left
                ,Sprites.BigRun,
                Sprites.BigRunLeft);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            player.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Batch.Begin();
            _world.Draw();
            player.Draw(Batch,gameTime);
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
