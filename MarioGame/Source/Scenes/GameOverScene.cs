using System;

using MarioGame;
using MarioGame.Source.Scenes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioBros.Source.Scenes;

public class GameOverScene : IScene, IDisposable
{
    private bool _disposed;
    private string _screen { get; set; } = "Screen";
    private int _remainingLives;
    private int _coins;
    private string _world;
    private int _time;

    public GameOverScene(int remainingLives, int coins, string world, int time)
    {
        _remainingLives = remainingLives;
        _coins = coins;
        _world = world;
        _time = time;
    }


    /*
     * Loads resources needed for the game over scene.
     * This includes loading game sprites and background music.
     *
     * Parameters:
     *   spriteData: SpriteData object containing content manager for loading resources.
     *               If null, no resources will be loaded.
     */
    public void Load(SpriteData spriteData)
    {
        Sprites.Load(spriteData?.content);
    }

    /*
     * Unloads resources and performs cleanup operations for the game over scene.
     * This method is called when the scene is being unloaded or switched.
     * It prints the current screen information to the console.
     */
    public void Unload()
    {
        Console.WriteLine(_screen);
    }

    /*
     * Draws the gameover scene on the screen.
     * This method clears the graphics device, then draws various elements
     * such as information of world, time, etc
     */
    public void Draw(SpriteData spriteData)
    {
        spriteData?.graphics.GraphicsDevice.Clear(Color.Black);

        spriteData.spriteBatch.Begin();
        DrawTextWithNumber("COINS", $"{_coins}", 70, 10, spriteData);
        DrawTextWithNumber("WORLD", _world, 550, 10, spriteData);
        DrawTextWithNumber("TIME", $"{_time}", 900, 10, spriteData);
        DrawText("GAME OVER", 500, 300, spriteData);
        DrawText($"MARIO x{_remainingLives}", 500, 330, spriteData);


        spriteData.spriteBatch.End();
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
     * Draws text  at the specified position.
     *
     * Parameters:
     *   text: The text to display.
     *   x: The X-coordinate of the text position.
     *   y: The Y-coordinate of the text position.
     *   spriteData: SpriteData object containing graphics device, sprite batch, and font for drawing.
     *               If null, no drawing will occur.
     */
    private static void DrawText(string text, float x, float y, SpriteData spriteData)
    {
        Vector2 textPosition = new Vector2(x, y);

        spriteData.spriteBatch.DrawString(spriteData.spriteFont, text, textPosition, Color.White);
    }


    /*
     * Performs cleanup operations and releases resources.
     * This method is called to dispose of the GameOver object.
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
        if (_disposed)
            return;

        _disposed = true;
    }
}

