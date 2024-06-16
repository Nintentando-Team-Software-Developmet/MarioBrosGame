using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Utils.SceneCommonData;

public static class CommonRenders
{

    /*
     * Draw dynamically all progress data of the game.
     * This includes: score, coins, level name and time.
     *
     * Parameters:
     *      entities: entities instance to recover the camera's axis
     *      spriteData: the SpriteData used to render all game.
     *      score: integer value representing the actual score
     *      coins: integer value representing the coins counter
     *      level: string that represents the current level name
     *      time: double value representing the current temporizer value
     */
    public static void DrawProgressData(IEnumerable<Entity> entities, SpriteData spriteData,
                                        int score, int coins, string level, double time)
    {
        var playerEntities = entities.Where(e => e.HasComponent<CameraComponent>());
        var enumerator = playerEntities.GetEnumerator();
        enumerator.MoveNext();
        CameraComponent camera = enumerator.Current.GetComponent<CameraComponent>();

        DrawCoin(spriteData, camera.Position.X, camera.Position.Y);
        DrawTextWithNumber("Mario", FillZeros(score, 6), camera.Position.X + 50, camera.Position.Y + 10, spriteData);
        DrawTextWithNumber($"x" + FillZeros(coins, 2), "", camera.Position.X + 435, camera.Position.Y + 40, spriteData);
        DrawTextWithNumber("WORLD", level, camera.Position.X + 678, camera.Position.Y + 10, spriteData);
        DrawTextWithNumber("TIME", time != 0 ? $"{(int)time}" : string.Empty, camera.Position.X + 1028, camera.Position.Y + 10, spriteData);
    }

    /*
     * Overload to render progress data in static scenes
     */
    public static void DrawProgressData(SpriteData spriteData,
        int score, int coins, string level, double time)
    {
        DrawCoin(spriteData, 0, 0);
        DrawTextWithNumber("Mario", FillZeros(score, 6), 50, 10, spriteData);
        DrawTextWithNumber($"x" + FillZeros(coins, 2), "", 435, 40, spriteData);
        DrawTextWithNumber("WORLD", level, 678, 10, spriteData);
        DrawTextWithNumber("TIME", time != 0 ? $"{(int)time}" : String.Empty, 1028, 10, spriteData);
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
    private static void DrawCoin(SpriteData spriteData, float positionX, float positionY)
    {
        Vector2 position = new Vector2(positionX + 405, positionY + 40);
        if (spriteData != null)
            spriteData.spriteBatch.Draw(Sprites.CoinIcon, position, null, Color.White, 0f, Vector2.Zero,
                new Vector2(2f), SpriteEffects.None, 0f);
    }

    /*
     * Draws text  at the specified position.
     *
     * Parameters:
     *   text: The text to display.
     *   x: The X-coordinate of the text position.
     *   y: The Y-coordinate of the text position.
     *   spriteData: SpriteData object containing graphics device, sprite batch, and font for drawing.
     *               If null, no drawing will occur.
     */
    public static void DrawText(string text, float x, float y, SpriteData spriteData)
    {
        Vector2 textPosition = new Vector2(x, y);
        if (spriteData != null)
            spriteData.spriteBatch.DrawString(spriteData.spriteFont, text, textPosition, Color.White, 0f, Vector2.Zero,
                1f, SpriteEffects.None, 0f);
    }

    /*
     * Help to convert integer values to string with specific length format.
     * Fill with zeros the remaining string length.
     */
    private static string FillZeros(this int number, int totalLength)
    {
        return number.ToString(CultureInfo.InvariantCulture).PadLeft(totalLength, '0');
    }

    /*
     * Draws a sprite on the screen in the specific position.
     */
    public static void DrawIcon(SpriteData spriteData, float positionX, float positionY, Texture2D sprite)
    {
        Vector2 position = new Vector2(positionX, positionY);
        if (spriteData != null)
            spriteData.spriteBatch.Draw(sprite, position, null, Color.White, 0f, Vector2.Zero,
                new Vector2(1f), SpriteEffects.None, 0f);
    }

    /*
     * Draws an entity on the screen.
     */
    public static void DrawEntity(SpriteBatch spriteBatch, AnimationComponent animation, ColliderComponent collider)
    {
        if (spriteBatch == null || animation == null || collider == null)
            return;

        float entityPosX = collider.collider.Position.X * Constants.pixelPerMeter;
        float entityPosY = collider.collider.Position.Y * Constants.pixelPerMeter;
        Vector2 entityPosition = new Vector2(entityPosX, entityPosY);

        int textureRectX = (int)entityPosition.X - animation.width / 2;
        int textureRectY = (int)entityPosition.Y - animation.height / 2;
        animation.textureRectangle = new Rectangle(textureRectX, textureRectY, animation.width, animation.height);

        Texture2D currentTexture = animation.Textures[animation.CurrentFrame];
        spriteBatch.Draw(currentTexture, animation.textureRectangle, Color.White);
    }
}
