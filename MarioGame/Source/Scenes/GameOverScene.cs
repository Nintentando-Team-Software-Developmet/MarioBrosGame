using System;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using SuperMarioBros.Source.Systems;

namespace SuperMarioBros.Source.Scenes;

public class GameOverScene : IScene, IDisposable
{
    private bool _disposed;
    private string _screen { get; set; } = "Screen";
    private GameDataSystem _gameDataSystem;

    public GameOverScene()
    {
        _gameDataSystem = new GameDataSystem();
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

    public void Draw(SpriteData spriteData)
    {
    }

    /*
     * Draws the gameover scene on the screen.
     * This method clears the graphics device, then draws various elements
     * such as information of world, time, etc
     */
    public void Draw(SpriteData spriteData, GameTime gameTime)
    {
        spriteData?.graphics.GraphicsDevice.Clear(Color.Black);

        spriteData.spriteBatch.Begin();
        DrawCoin(spriteData);
        DrawTextWithNumber($"x{_gameDataSystem.CoinsCounter.Coins}", "", 280, 10, spriteData);
        DrawTextWithNumber("WORLD", _gameDataSystem.LevelName, 550, 10, spriteData);
        DrawTextWithNumber("TIME", $"{(int)_gameDataSystem.Time.Seconds}", 900, 10, spriteData);
        DrawTextWithNumber("Mario", $"{_gameDataSystem.TotalScore.Score}", 50, 10, spriteData);
        DrawText("GAME OVER", 330, 300, spriteData);


        spriteData.spriteBatch.End();
    }

    public void Update(GameTime gameTime, SceneManager sceneManager)
    {
        //TODO: Temporal, la verdadera actualizacion esta dentro de SystemManager/GameDataSystem
        if (gameTime != null) _gameDataSystem.Time.Seconds -= gameTime.ElapsedGameTime.TotalSeconds;
    }

    public SceneType GetSceneType()
    {
        return SceneType.TransitionScene;
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
        Vector2 numberPosition = new Vector2(x, y + spriteData.spriteFont.LineSpacing * 1.5f);

        spriteData.spriteBatch.DrawString(spriteData.spriteFont, text, textPosition, Color.White, 0f, Vector2.Zero, 1.6f, SpriteEffects.None, 0f);
        spriteData.spriteBatch.DrawString(spriteData.spriteFont, number, numberPosition, Color.White, 0f, Vector2.Zero, 1.6f, SpriteEffects.None, 0f);
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

        spriteData.spriteBatch.DrawString(spriteData.spriteFont, text, textPosition, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

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

    private static void DrawCoin(SpriteData spriteData)
    {
        Vector2 position = new Vector2(250, 10);
        spriteData.spriteBatch.Draw(Sprites.CoinIcon, position, null, Color.White, 0f, Vector2.Zero, new Vector2(2f), SpriteEffects.None, 0f);
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

