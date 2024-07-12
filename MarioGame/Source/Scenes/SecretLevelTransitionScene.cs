using System;

using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Scenes;

public class SecretLevelTransitionScene : IScene, IDisposable
{
    private bool _disposed;
    private ProgressDataManager _progressDataManager;
    private double _displayTime;
    public const double MaxDisplayTime = 2.0;
    private SceneName _nextScene;

    public SecretLevelTransitionScene(ProgressDataManager progressDataManager, SceneName sceneName)
    {
        _progressDataManager = progressDataManager;
        _displayTime = 0;
        _nextScene = sceneName;
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
        MediaPlayer.Play(spriteData.content.Load<Song>("Sounds/StockTune"));
        MediaPlayer.IsRepeating = true;

    }

    /*
     * Unloads resources and performs cleanup operations for the game over scene.
     * This method is called when the scene is being unloaded or switched.
     * It prints the current screen information to the console.
     */
    public void Unload()
    {
        _displayTime = 0;
        MediaPlayer.Stop();
    }

    /*
     * Draws the gameover scene on the screen.
     * This method clears the graphics device, then draws various elements
     * such as information of world, time, etc
     */
    public void Draw(SpriteData spriteData, GameTime gameTime)
    {
        spriteData?.graphics.GraphicsDevice.Clear(Color.Black);
    }


    public void Update(GameTime gameTime, SceneManager sceneManager)
    {
        if (sceneManager == null) throw new ArgumentNullException(nameof(sceneManager));
        if (gameTime != null) _displayTime += gameTime.ElapsedGameTime.TotalSeconds;
        if (_displayTime >= MaxDisplayTime)
        {
            _displayTime = 0;
            sceneManager.ChangeScene(_nextScene);
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


