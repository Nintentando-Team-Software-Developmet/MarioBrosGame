using System;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.SceneCommonData;

namespace SuperMarioBros.Source.Scenes;

public class LivesScene : IScene, IDisposable
{
    private bool _disposed;
    private string _screen { get; set; } = "Screen";
    private ProgressDataManager _progressDataManager;

    public LivesScene(ProgressDataManager progressDataManager)
    {
        _progressDataManager = progressDataManager;
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
    public void Draw(SpriteData spriteData, GameTime gameTime)
    {
        spriteData?.graphics.GraphicsDevice.Clear(Color.Black);

        spriteData.spriteBatch.Begin();
        CommonRenders.DrawProgressData(spriteData, _progressDataManager.Score,
                                        _progressDataManager.Coins,
                                        "1-1",
                                        0);
        CommonRenders.DrawText("WORLD 1-1", 330, 200, spriteData);
        CommonRenders.DrawIcon(spriteData, 400, 300, Sprites.BigStop);
        CommonRenders.DrawText($"x {_progressDataManager.Lives}", 550, 350, spriteData);
        spriteData.spriteBatch.End();
    }

    public void Update(GameTime gameTime, SceneManager sceneManager)
    {
        _progressDataManager.UpdateHighScore();
    }

    public SceneType GetSceneType()
    {
        return SceneType.TransitionScene;
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

