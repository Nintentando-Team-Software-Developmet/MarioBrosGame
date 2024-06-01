using System;

using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioBros
{
    public class MenuGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _titleFont;
        private Texture2D _pixelTexture;
        private Song _backgroundMusic;

        public MenuGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;

            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Sprites.Load(Content);
            _titleFont = Content.Load<SpriteFont>("Fonts/Title");
            _backgroundMusic = Content.Load<Song>("Sounds/mario-bros-remix");
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            var gamePadState = GamePad.GetState(PlayerIndex.One);

            if (gamePadState.Buttons.B == ButtonState.Pressed)
            {
                DrawMessage("Button pressed");
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(121, 177, 249));
            _spriteBatch.Begin();
            DrawBricks();
            DrawScene();
            DrawMario();
            DrawTitle();
            DrawTextWithNumber("MONEDAS", "000000", 70, 10);
            DrawTextWithNumber("WORLD", "1 - 1", 550, 10);
            DrawTextWithNumber("TIME", "-", 900, 10);
            DrawStartButton();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawBricks()
        {
            int numCols = (int)Math.Ceiling(GraphicsDevice.Viewport.Width / (Sprites.BrickBlockBrown.Width * 3f));
            for (int x = 0; x < numCols; x++)
            {
                _spriteBatch.Draw(Sprites.BrickBlockBrown,
                    new Vector2(x * Sprites.BrickBlockBrown.Width * 3f, GraphicsDevice.Viewport.Height - (Sprites.BrickBlockBrown.Height * 3f)),
                    null, Color.White, 0f, Vector2.Zero, new Vector2(3f),
                    SpriteEffects.None, 0f
                );
            }
        }

        private void DrawScene()
        {
            Vector2 mountainPosition = new Vector2(90, GraphicsDevice.Viewport.Height - (Sprites.MountainMenu.Height * 5.1f));
            _spriteBatch.Draw(Sprites.MountainMenu, mountainPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(3.7f), SpriteEffects.None, 0f);
            Vector2 brushPosition = new Vector2(700, GraphicsDevice.Viewport.Height - (Sprites.BushMenu.Height * 7f));
            _spriteBatch.Draw(Sprites.BushMenu, brushPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(4f), SpriteEffects.None, 0f);
        }

        private void DrawMario()
        {
            Vector2 marioPosition = new Vector2(200, GraphicsDevice.Viewport.Height - (Sprites.BrickBlockBrown.Height * 7f));
            _spriteBatch.Draw(Sprites.SmallStop, marioPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(4f), SpriteEffects.None, 0f);
        }

        private void DrawBackground()
        {
            Rectangle titleBackground = new Rectangle(260, 90, 600, 300);
            Color backgroundColor = new Color(124, 64, 20);

            Rectangle shadowDown = new Rectangle(titleBackground.X + 3, titleBackground.Y + 3, titleBackground.Width, titleBackground.Height);
            Color shadowColorDown = new Color(0, 0, 0, 200);

            Rectangle shadowUp = new Rectangle(titleBackground.X - 3, titleBackground.Y - 3, titleBackground.Width, titleBackground.Height);
            Color shadowColorUp = new Color(235, 211, 170);

            _spriteBatch.Draw(_pixelTexture, shadowDown, shadowColorDown);
            _spriteBatch.Draw(_pixelTexture, shadowUp, shadowColorUp);
            _spriteBatch.Draw(_pixelTexture, titleBackground, backgroundColor);

            int padding = 10;
            int cornerSize = 12;
            Color innerColor = new Color(241, 213, 192);

            Rectangle topLeftCorner = new Rectangle(titleBackground.X + padding, titleBackground.Y + padding, cornerSize, cornerSize);
            Rectangle topRightCorner = new Rectangle(titleBackground.X + titleBackground.Width - padding - cornerSize, titleBackground.Y + padding, cornerSize, cornerSize);
            Rectangle bottomLeftCorner = new Rectangle(titleBackground.X + padding, titleBackground.Y + titleBackground.Height - padding - cornerSize, cornerSize, cornerSize);
            Rectangle bottomRightCorner = new Rectangle(titleBackground.X + titleBackground.Width - padding - cornerSize, titleBackground.Y + titleBackground.Height - padding - cornerSize, cornerSize, cornerSize);

            _spriteBatch.Draw(_pixelTexture, topLeftCorner, innerColor);
            _spriteBatch.Draw(_pixelTexture, topRightCorner, innerColor);
            _spriteBatch.Draw(_pixelTexture, bottomLeftCorner, innerColor);
            _spriteBatch.Draw(_pixelTexture, bottomRightCorner, innerColor);
        }

        private void DrawTitle()
        {
            DrawBackground();

            float fontSize = 60f;

            Vector2 textSize = _titleFont.MeasureString("MARIO BROS") * (fontSize / _titleFont.LineSpacing);

            Rectangle textRectangle = new Rectangle(260, 90, 600, 300);

            Vector2 titlePosition = new Vector2(
                textRectangle.X + (textRectangle.Width - textSize.X) / 2,
                textRectangle.Y + (textRectangle.Height - textSize.Y) / 2
            );

            Vector2 shadowOffset = new Vector2(5, 5);

            _spriteBatch.DrawString(_titleFont, "MARIO BROS", titlePosition + shadowOffset, Color.Black, 0f, Vector2.Zero, fontSize / _titleFont.LineSpacing, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(_titleFont, "MARIO BROS", titlePosition, new Color(235, 211, 170), 0f, Vector2.Zero, fontSize / _titleFont.LineSpacing, SpriteEffects.None, 0f);
        }

        private void DrawTextWithNumber(string text, string number, float x, float y)
        {
            Vector2 textPosition = new Vector2(x, y);
            Vector2 numberPosition = new Vector2(x, y + _titleFont.LineSpacing);

            _spriteBatch.DrawString(_titleFont, text, textPosition, Color.White);
            _spriteBatch.DrawString(_titleFont, number, numberPosition, Color.White);
        }

        private void DrawStartButton()
        {
            float fontSize = 30f;
            float scale = fontSize / _titleFont.MeasureString("START").Y;
            Vector2 startPosition = new Vector2(
                (GraphicsDevice.Viewport.Width - _titleFont.MeasureString("START").X * scale) / 2,
                GraphicsDevice.Viewport.Height - 200
            );
            _spriteBatch.DrawString(_titleFont, "START", startPosition, Color.Yellow, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        private static void DrawMessage(string message)
        {
            Console.WriteLine(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _spriteBatch?.Dispose();
                _graphics?.Dispose();
                _pixelTexture?.Dispose();
                _backgroundMusic?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
