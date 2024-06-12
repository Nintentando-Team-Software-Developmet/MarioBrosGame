using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Utils.SceneCommonData;

public static class CommonRenders
{

    /*
     * Draw all progress data of the game.
     * This includes: score, coins, level name and time.
     *
     * Parameters:
     *      spriteData: the SpriteData used to render all game.
     *      score: integer value representing the actual score
     *      coins: integer value representing the coins counter
     *      level: string that represents the current level name
     *      time: double value representing the current temporizer value
     */
    public static void DrawProgressData(SpriteData spriteData, int score, int coins, string level, double time)
    {
        DrawCoin(spriteData);
        DrawTextWithNumber("Mario", $"{score}", 50, 10, spriteData);
        DrawTextWithNumber($"x{coins}", "", 280, 10, spriteData);
        DrawTextWithNumber("WORLD", level, 550, 10, spriteData);
        DrawTextWithNumber("TIME", time != 0 ? $"{(int)time}" : String.Empty, 900, 10, spriteData);
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
        if (spriteData != null)
        {
            Vector2 numberPosition = new Vector2(x, y + spriteData.spriteFont.LineSpacing);

            spriteData.spriteBatch.DrawString(spriteData.spriteFont, text, textPosition, Color.White);
            spriteData.spriteBatch.DrawString(spriteData.spriteFont, number, numberPosition, Color.White);
        }
    }

    /*
     * Draws the coin icon on the screen to allow the coins counter render.
     */
    private static void DrawCoin(SpriteData spriteData)
    {
        Vector2 position = new Vector2(250, 10);
        if (spriteData != null)
            spriteData.spriteBatch.Draw(Sprites.CoinIcon, position, null, Color.White, 0f, Vector2.Zero,
                new Vector2(2f), SpriteEffects.None, 0f);
    }
}
