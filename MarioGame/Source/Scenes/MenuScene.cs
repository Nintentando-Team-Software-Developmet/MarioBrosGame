using System;

using MarioGame;
using MarioGame.Source.Scenes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioBros.Source.Scenes
{
    public class MenuScene : IScene, IDisposable
    {
        private bool disposed;
        private string Screen { get; set; } = "Screen";
        private Texture2D _pixelTexture;
        private Song _backgroundMusic;

        public void Load(SpriteData spriteData)
        {
            _pixelTexture = new Texture2D(spriteData?.graphics.GraphicsDevice, 1, 1);
            _backgroundMusic = spriteData?.content.Load<Song>("Sounds/mario-bros-remix");
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true;
        }

        public void Unload()
        {
            Console.WriteLine(Screen);
        }

        public void Draw(SpriteData spriteData)
        {
            spriteData?.graphics.GraphicsDevice.Clear(new Color(121, 177, 249));

            spriteData.spriteBatch.Begin();
            DrawBricks(spriteData);
            DrawScene(spriteData);
            DrawMario(spriteData);
            DrawTitle(spriteData);
            DrawTextWithNumber("MONEDAS", "000000", 70, 10,spriteData);
            DrawTextWithNumber("WORLD", "1 - 1", 550, 10,spriteData);
            DrawTextWithNumber("TIME", "-", 900, 10,spriteData);
            DrawStartButton(spriteData);

            spriteData.spriteBatch.End();

        }

        private static void DrawBricks(SpriteData spriteData)
        {
            int numCols = (int)Math.Ceiling(spriteData.graphics.GraphicsDevice.Viewport.Width / (Sprites.BrickBlockBrown.Width * 3f));
            for (int x = 0; x < numCols; x++)
            {
                spriteData.spriteBatch.Draw(Sprites.BrickBlockBrown,
                    new Vector2(x * Sprites.BrickBlockBrown.Width * 3f, spriteData.graphics.GraphicsDevice.Viewport.Height - (Sprites.BrickBlockBrown.Height * 3f)),
                    null, Color.White, 0f, Vector2.Zero, new Vector2(3f),
                    SpriteEffects.None, 0f
                );
            }
        }

        private static void DrawScene(SpriteData spriteData)
        {
            Vector2 mountainPosition = new Vector2(90, spriteData.graphics.GraphicsDevice.Viewport.Height - (Sprites.MountainMenu.Height * 5.1f));
            spriteData.spriteBatch.Draw(Sprites.MountainMenu, mountainPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(3.7f), SpriteEffects.None, 0f);
            Vector2 brushPosition = new Vector2(700, spriteData.graphics.GraphicsDevice.Viewport.Height - (Sprites.BushMenu.Height * 7f));
            spriteData.spriteBatch.Draw(Sprites.BushMenu, brushPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(4f), SpriteEffects.None, 0f);
        }

        private static void DrawMario(SpriteData spriteData)
        {
            Vector2 marioPosition = new Vector2(200, spriteData.graphics.GraphicsDevice.Viewport.Height - (Sprites.BrickBlockBrown.Height * 7f));
            spriteData.spriteBatch.Draw(Sprites.SmallStop, marioPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(4f), SpriteEffects.None, 0f);
        }

        private void DrawBackground(SpriteData spriteData)
        {
            Rectangle titleBackground = new Rectangle(260, 90, 600, 300);
            Color backgroundColor = new Color(124, 64, 20);

            Rectangle shadowDown = new Rectangle(titleBackground.X + 3, titleBackground.Y + 3, titleBackground.Width, titleBackground.Height);
            Color shadowColorDown = new Color(0, 0, 0, 200);

            Rectangle shadowUp = new Rectangle(titleBackground.X - 3, titleBackground.Y - 3, titleBackground.Width, titleBackground.Height);
            Color shadowColorUp = new Color(235, 211, 170);

            spriteData.spriteBatch.Draw(_pixelTexture, shadowDown, shadowColorDown);
            spriteData.spriteBatch.Draw(_pixelTexture, shadowUp, shadowColorUp);
            spriteData.spriteBatch.Draw(_pixelTexture, titleBackground, backgroundColor);

            int padding = 10;
            int cornerSize = 12;
            Color innerColor = new Color(241, 213, 192);

            Rectangle topLeftCorner = new Rectangle(titleBackground.X + padding, titleBackground.Y + padding, cornerSize, cornerSize);
            Rectangle topRightCorner = new Rectangle(titleBackground.X + titleBackground.Width - padding - cornerSize, titleBackground.Y + padding, cornerSize, cornerSize);
            Rectangle bottomLeftCorner = new Rectangle(titleBackground.X + padding, titleBackground.Y + titleBackground.Height - padding - cornerSize, cornerSize, cornerSize);
            Rectangle bottomRightCorner = new Rectangle(titleBackground.X + titleBackground.Width - padding - cornerSize, titleBackground.Y + titleBackground.Height - padding - cornerSize, cornerSize, cornerSize);

            spriteData.spriteBatch.Draw(_pixelTexture, topLeftCorner, innerColor);
            spriteData.spriteBatch.Draw(_pixelTexture, topRightCorner, innerColor);
            spriteData.spriteBatch.Draw(_pixelTexture, bottomLeftCorner, innerColor);
            spriteData.spriteBatch.Draw(_pixelTexture, bottomRightCorner, innerColor);
        }

        private void DrawTitle(SpriteData spriteData)
        {
            DrawBackground(spriteData);

            float fontSize = 60f;

            Vector2 textSize = spriteData.spriteFont.MeasureString("MARIO BROS") * (fontSize / spriteData.spriteFont.LineSpacing);

            Rectangle textRectangle = new Rectangle(260, 90, 600, 300);

            Vector2 titlePosition = new Vector2(
                textRectangle.X + (textRectangle.Width - textSize.X) / 2,
                textRectangle.Y + (textRectangle.Height - textSize.Y) / 2
            );

            Vector2 shadowOffset = new Vector2(5, 5);

            spriteData.spriteBatch.DrawString(spriteData.spriteFont, "MARIO BROS", titlePosition + shadowOffset, Color.Black, 0f, Vector2.Zero, fontSize / spriteData.spriteFont.LineSpacing, SpriteEffects.None, 0f);
            spriteData.spriteBatch.DrawString(spriteData.spriteFont, "MARIO BROS", titlePosition, new Color(235, 211, 170), 0f, Vector2.Zero, fontSize / spriteData.spriteFont.LineSpacing, SpriteEffects.None, 0f);
        }

        private static void DrawTextWithNumber(string text, string number, float x, float y, SpriteData spriteData)
        {
            Vector2 textPosition = new Vector2(x, y);
            Vector2 numberPosition = new Vector2(x, y + spriteData.spriteFont.LineSpacing);

            spriteData.spriteBatch.DrawString(spriteData.spriteFont, text, textPosition, Color.White);
            spriteData.spriteBatch.DrawString(spriteData.spriteFont, number, numberPosition, Color.White);
        }

        private static void DrawStartButton(SpriteData spriteData)
        {
            float fontSize = 30f;
            float scale = fontSize / spriteData.spriteFont.MeasureString("START").Y;
            Vector2 startPosition = new Vector2(
                (spriteData.graphics.GraphicsDevice.Viewport.Width - spriteData.spriteFont.MeasureString("START").X * scale) / 2,
                spriteData.graphics.GraphicsDevice.Viewport.Height - 200
            );
            spriteData.spriteBatch.DrawString(spriteData.spriteFont, "START", startPosition, Color.Yellow, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        private static void DrawMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _backgroundMusic?.Dispose();
                _pixelTexture?.Dispose();
            }

            disposed = true;
        }
    }
}
