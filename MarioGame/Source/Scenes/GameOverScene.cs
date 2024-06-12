using System;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.SceneCommonData;

namespace SuperMarioBros.Source.Scenes;

public class GameOverScene : IScene, IDisposable
{
    private bool _disposed;
    private string _screen { get; set; } = "Screen";


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
    public void Draw(SpriteData spriteData, GameTime gameTime)
    {
        spriteData?.graphics.GraphicsDevice.Clear(Color.Black);

        spriteData.spriteBatch.Begin();
        CommonRenders.DrawProgressData(spriteData, WorldGame.ProgressDataManager.Data.Score,
                                        WorldGame.ProgressDataManager.Data.Coins,
                                        "1-1",
                                        0);
        DrawText("GAME OVER", 330, 300, spriteData);


        spriteData.spriteBatch.End();
    }

    public void Update(GameTime gameTime, SceneManager sceneManager)
    {
        //TODO: Temporal, la verdadera actualizacion esta dentro de SystemManager/GameDataSystem
        WorldGame.ProgressDataManager.Update(gameTime);
    }

    public SceneType GetSceneType()
    {
        return SceneType.TransitionScene;
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

