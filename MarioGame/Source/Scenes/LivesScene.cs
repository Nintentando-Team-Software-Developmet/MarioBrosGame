using System;

using MarioGame;

using Microsoft.Xna.Framework;


using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.SceneCommonData;

namespace SuperMarioBros.Source.Scenes;

public class LivesScene : IScene, IDisposable
{
    private bool _disposed;
    private ProgressDataManager _progressDataManager;
    private double _displayTime;
    public const double MaxDisplayTime = 3.0;

    public LivesScene(ProgressDataManager progressDataManager)
    {
        _progressDataManager = progressDataManager;
        _displayTime = 0;
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
        _displayTime = 0;
    }

    /*
     * Draws the gameover scene on the screen.
     * This method clears the graphics device, then draws various elements
     * such as information of world, time, etc
     */
    public void Draw(SpriteData spriteData, GameTime gameTime)
    {
        spriteData?.graphics.GraphicsDevice.Clear(Color.Black);

        if (spriteData != null)
        {
            spriteData.spriteBatch.Begin();
            CommonRenders.DrawProgressData(spriteData, _progressDataManager.Score,
                _progressDataManager.Coins,
                "1-1",
                0);
            CommonRenders.DrawText("WORLD 1-1", 420, 200, spriteData);
            CommonRenders.DrawIcon(spriteData, 430, 320, Sprites.SmallStop);
            CommonRenders.DrawText($"x {_progressDataManager.Lives}", 550, 350, spriteData);
            spriteData.spriteBatch.End();
        }
    }

    public void Update(GameTime gameTime, SceneManager sceneManager)
    {
        if (sceneManager == null) throw new ArgumentNullException(nameof(sceneManager));
        if (gameTime != null) _displayTime += gameTime.ElapsedGameTime.TotalSeconds;
        if (_displayTime >= MaxDisplayTime)
        {
            _displayTime = 0;
            if (_progressDataManager.Lives > 0)
            {
                _progressDataManager.ResetTime();
                sceneManager.ChangeScene(SceneName.Level1);
            }
            else
            {
                sceneManager.ChangeScene(SceneName.GameOver);
            }
        }
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
