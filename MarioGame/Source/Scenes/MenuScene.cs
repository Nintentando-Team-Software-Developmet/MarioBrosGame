using System;

using MarioGame;
using MarioGame.Source.Scenes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioBros.Source.Scenes
{
    /*
     * Represents a scene of the game's menu.
     * Implements the IScene interface for managing game scenes.
     * Also implements IDisposable for safe resource disposal.
     */
    public class MenuScene : IScene, IDisposable
    {
        private bool disposed;
        private string Screen { get; set; } = "Screen";

        /*
         * Loads resources needed for the menu scene.
         * This includes loading game sprites and background music.
         *
         * Parameters:
         *   spriteData: SpriteData object containing content manager for loading resources.
         *               If null, no resources will be loaded.
         */
        public void Load(SpriteData spriteData)
        {
            Sprites.Load(spriteData?.content);

            MediaPlayer.Play(spriteData?.content.Load<Song>("Sounds/mario-bros-remix"));
            MediaPlayer.IsRepeating = true;
        }

        //TODO: Implement
        /*protected void Update(GameTime gameTime)
        {
            var gamePadState = GamePad.GetState(PlayerIndex.One);

            if (gamePadState.Buttons.B == ButtonState.Pressed)
            {
                DrawMessage("Button pressed");
            }
        }*/

        /*
         * Unloads resources and performs cleanup operations for the menu scene.
         * This method is called when the scene is being unloaded or switched.
         * It prints the current screen information to the console.
         */
        public void Unload()
        {
            Console.WriteLine(Screen);
        }

        /*
         * Draws the menu scene on the screen.
         * This method clears the graphics device, then draws various elements
         * such as bricks, scenery, Mario character, title, and UI elements.
         */
        public void Draw(SpriteData spriteData)
        {
            spriteData?.graphics.GraphicsDevice.Clear(new Color(121, 177, 249));

            spriteData.spriteBatch.Begin();
            DrawBricks(spriteData);
            DrawScene(spriteData);
            DrawMario(spriteData);
            DrawTitle(spriteData);
            DrawTextWithNumber("MONEDAS", "000000", 70, 10, spriteData);
            DrawTextWithNumber("WORLD", "1 - 1", 550, 10, spriteData);
            DrawTextWithNumber("TIME", "-", 900, 10, spriteData);
            DrawStartButton(spriteData);

            spriteData.spriteBatch.End();
        }

        /*
         * Draws a row of brick blocks across the bottom of the screen.
         * The number of brick blocks drawn depends on the width of the viewport.
         */
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

        /*
         * Draws the scenery elements such as mountains and bushes.
         */
        private static void DrawScene(SpriteData spriteData)
        {
            Vector2 mountainPosition = new Vector2(90, spriteData.graphics.GraphicsDevice.Viewport.Height - (Sprites.MountainMenu.Height * 5.1f));
            spriteData.spriteBatch.Draw(Sprites.MountainMenu, mountainPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(3.7f), SpriteEffects.None, 0f);
            Vector2 brushPosition = new Vector2(700, spriteData.graphics.GraphicsDevice.Viewport.Height - (Sprites.BushMenu.Height * 7f));
            spriteData.spriteBatch.Draw(Sprites.BushMenu, brushPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(4f), SpriteEffects.None, 0f);
        }

        /*
         * Draws the Mario character sprite.
         */
        private static void DrawMario(SpriteData spriteData)
        {
            Vector2 marioPosition = new Vector2(200, spriteData.graphics.GraphicsDevice.Viewport.Height - (Sprites.BrickBlockBrown.Height * 7f));
            spriteData.spriteBatch.Draw(Sprites.SmallStop, marioPosition, null, Color.White, 0f, Vector2.Zero, new Vector2(4f), SpriteEffects.None, 0f);
        }

        /*
         * Draws the background for the title screen.
         * This includes a colored background with shadows and corner decorations.
         */
        private static void DrawBackground(SpriteData spriteData)
        {
            Rectangle titleBackground = new Rectangle(260, 90, 600, 300);
            Color backgroundColor = new Color(124, 64, 20);

            Rectangle shadowDown = new Rectangle(titleBackground.X + 3, titleBackground.Y + 3, titleBackground.Width, titleBackground.Height);
            Color shadowColorDown = new Color(0, 0, 0, 200);

            Rectangle shadowUp = new Rectangle(titleBackground.X - 3, titleBackground.Y - 3, titleBackground.Width, titleBackground.Height);
            Color shadowColorUp = new Color(235, 211, 170);

            spriteData.spriteBatch.Draw(spriteData.pixelTexture, shadowDown, shadowColorDown);
            spriteData.spriteBatch.Draw(spriteData.pixelTexture, shadowUp, shadowColorUp);
            spriteData.spriteBatch.Draw(spriteData.pixelTexture, titleBackground, backgroundColor);

            int padding = 10;
            int cornerSize = 12;
            Color innerColor = new Color(241, 213, 192);

            Rectangle topLeftCorner = new Rectangle(titleBackground.X + padding, titleBackground.Y + padding, cornerSize, cornerSize);
            Rectangle topRightCorner = new Rectangle(titleBackground.X + titleBackground.Width - padding - cornerSize, titleBackground.Y + padding, cornerSize, cornerSize);
            Rectangle bottomLeftCorner = new Rectangle(titleBackground.X + padding, titleBackground.Y + titleBackground.Height - padding - cornerSize, cornerSize, cornerSize);
            Rectangle bottomRightCorner = new Rectangle(titleBackground.X + titleBackground.Width - padding - cornerSize, titleBackground.Y + titleBackground.Height - padding - cornerSize, cornerSize, cornerSize);

            spriteData.spriteBatch.Draw(spriteData.pixelTexture, topLeftCorner, innerColor);
            spriteData.spriteBatch.Draw(spriteData.pixelTexture, topRightCorner, innerColor);
            spriteData.spriteBatch.Draw(spriteData.pixelTexture, bottomLeftCorner, innerColor);
            spriteData.spriteBatch.Draw(spriteData.pixelTexture, bottomRightCorner, innerColor);
        }

        /*
         * Draws the title text "MARIO BROS" with shadows on the title background.
         */
        private static void DrawTitle(SpriteData spriteData)
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

        /*
         * Draws text followed by a number at the specified position.
         *
         * Parameters:
         *   text: The text to display.
         *   number: The number to display.
         *   x: The X-coordinate of the text position.
         *   y: The Y-coordinate of the text position.
         *   spriteData: SpriteData object containing graphics device, sprite batch, and font for drawing.
         *               If null, no drawing will occur.
         */
        private static void DrawTextWithNumber(string text, string number, float x, float y, SpriteData spriteData)
        {
            Vector2 textPosition = new Vector2(x, y);
            Vector2 numberPosition = new Vector2(x, y + spriteData.spriteFont.LineSpacing);

            spriteData.spriteBatch.DrawString(spriteData.spriteFont, text, textPosition, Color.White);
            spriteData.spriteBatch.DrawString(spriteData.spriteFont, number, numberPosition, Color.White);
        }

        /*
         * Draws a "START" button at the center bottom of the screen.
         */
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

        /*
         * Performs cleanup operations and releases resources.
         * This method is called to dispose of the MenuScene object.
         */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /*
        * Releases managed resources if disposing is true.
        */
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
            }

            disposed = true;
        }
    }
}
